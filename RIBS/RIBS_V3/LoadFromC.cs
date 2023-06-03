using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RIBS_V3;

internal class LoadFromC
{
	public class SMHelper
	{
		public string prefix;

		public string SM_Name;

		public string initialState;

		public string period;

		public string currState;

		public string globals;

		public string locals;

		public string currStateForVar;

		public bool uartEnabled;

		public bool async;

		public Dictionary<string, string> stateActions;

		public Dictionary<string, Dictionary<string, string>> stateTrans;

		public Dictionary<string, Dictionary<string, string>> stateTransActions;

		public Dictionary<string, Dictionary<string, KeyValuePair<Point, Point>>> stateTransHandles;

		public Dictionary<string, Point> statePos;

		public Dictionary<string, string> stateType;

		public Dictionary<string, Dictionary<string, string>> stateFor;

		public List<string> states;

		public List<GroupCollection> stateLoopIniCand;

		public SMHelper()
		{
			prefix = string.Empty;
			SM_Name = string.Empty;
			initialState = string.Empty;
			period = string.Empty;
			currState = string.Empty;
			globals = string.Empty;
			locals = string.Empty;
			currStateForVar = string.Empty;
			uartEnabled = false;
			async = false;
			stateActions = new Dictionary<string, string>();
			stateTrans = new Dictionary<string, Dictionary<string, string>>();
			stateTransActions = new Dictionary<string, Dictionary<string, string>>();
			stateTransHandles = new Dictionary<string, Dictionary<string, KeyValuePair<Point, Point>>>();
			statePos = new Dictionary<string, Point>();
			stateType = new Dictionary<string, string>();
			stateFor = new Dictionary<string, Dictionary<string, string>>();
			states = new List<string>();
			stateLoopIniCand = new List<GroupCollection>();
		}
	}

	private RIBS ribsRef;

	private List<char> bracketStack = new List<char>();

	private List<char> parenStack = new List<char>();

	private string curr_line;

	private bool needsReading;

	private List<SMHelper> SMs = new List<SMHelper>();

	private int currentSM;

	private StreamReader readStream;

	public LoadFromC(RIBS refer)
	{
		ribsRef = refer;
	}

	public bool Load(string filename)
	{
		try
		{
			StreamReader streamReader = new StreamReader(filename);
			string input = streamReader.ReadToEnd();
			streamReader.Close();
			Match match = Regex.Match(input, "typedef\\s*struct\\s*task");
			MatchCollection matchCollection = Regex.Matches(input, "TickFct_\\w+");
			if (match.Success)
			{
				match = Regex.Match(input, "const\\s*unsigned\\s*char\\s*tasksNum\\s*=\\s*(\\d+)");
				if (match.Success)
				{
					return loadMultRTOS(filename, int.Parse(match.Groups[1].Value));
				}
			}
			if (matchCollection.Count > 2)
			{
				return loadMultSMRR(filename, matchCollection.Count / 2);
			}
			if (matchCollection.Count == 2)
			{
				return loadSingleSM(filename);
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Something bad happened :*(", "Load failed");
		}
		return false;
	}

	private bool loadSingleSM(string filename)
	{
		bool result = true;
		try
		{
			readStream = new StreamReader(filename);
			SMs.Add(new SMHelper());
			currentSM = 0;
			handleGlobals();
			handlePrefix();
			handleSMName();
			handleTickFct();
			while ((curr_line = readLine(readStream)) != null && (!curr_line.Contains("const") || !curr_line.Contains("unsigned") || !curr_line.Contains("int") || !curr_line.Contains("period") || !curr_line.Contains(SMs[currentSM].SM_Name)))
			{
			}
			if (curr_line != null)
			{
				curr_line = curr_line.Replace(";", "");
				string[] array = Regex.Split(curr_line.Trim(), "\\s+");
				SMs[currentSM].period = array[5];
			}
			else
			{
				SMs[currentSM].async = true;
			}
			buildSM();
			readStream.Close();
			return result;
		}
		catch (Exception)
		{
			MessageBox.Show("Something bad happened :*(", "Load failed");
			return false;
		}
	}

	private bool loadMultSMRR(string filename, int numSMs)
	{
		bool result = true;
		try
		{
			readStream = new StreamReader(filename);
			for (int i = 0; i < numSMs; i++)
			{
				currentSM = i;
				SMs.Add(new SMHelper());
				if (currentSM == 0)
				{
					handleGlobals();
				}
				else
				{
					SMs[currentSM].globals = SMs[currentSM - 1].globals;
				}
				handlePrefix();
				handleSMName();
				handleTickFct();
			}
			string input = readStream.ReadToEnd();
			Match match = Regex.Match(input, "TimerSet\\(\\s*(\\d+)\\s*\\)");
			if (match.Success)
			{
				foreach (SMHelper sM in SMs)
				{
					sM.period = match.Groups[1].Value;
				}
			}
			for (int j = 0; j < SMs.Count; j++)
			{
				currentSM = j;
				if (currentSM != 0)
				{
					ribsRef.addSM();
				}
				buildSM();
			}
			ribsRef.addLocalCode(0, SMs[0].locals);
			for (int k = 0; k < SMs.Count; k++)
			{
				currentSM = k;
				ribsRef.addGlobalCode(SMs[currentSM].globals);
			}
			readStream.Close();
			return result;
		}
		catch (Exception)
		{
			MessageBox.Show("Something bad happened :*(", "Load failed");
			return false;
		}
	}

	private bool loadMultRTOS(string filename, int numSMs)
	{
		bool result = true;
		try
		{
			readStream = new StreamReader(filename);
			for (int i = 0; i < numSMs; i++)
			{
				currentSM = i;
				if (currentSM >= SMs.Count)
				{
					SMs.Add(new SMHelper());
				}
				if (currentSM == 0)
				{
					handleGlobals();
					handleRTOSPeriods();
				}
				else
				{
					SMs[currentSM].globals = SMs[currentSM - 1].globals;
				}
				handlePrefix();
				handleSMName();
				handleTickFct();
			}
			for (int j = 0; j < SMs.Count; j++)
			{
				currentSM = j;
				if (currentSM != 0)
				{
					ribsRef.addSM();
				}
				buildSM();
			}
			ribsRef.addLocalCode(0, SMs[0].locals);
			for (int k = 0; k < SMs.Count; k++)
			{
				currentSM = k;
				ribsRef.addGlobalCode(SMs[currentSM].globals);
			}
			readStream.Close();
			return result;
		}
		catch (Exception)
		{
			MessageBox.Show("Something bad happened :*(", "Load failed");
			return false;
		}
	}

	private void handleRTOSPeriods()
	{
		int num = 0;
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("int main"))
		{
			Match match = Regex.Match(curr_line, "const\\s*unsigned\\s*long\\s*period\\w+\\s*=\\s*(\\d+)");
			if (match.Success)
			{
				if (num >= SMs.Count)
				{
					SMs.Add(new SMHelper());
				}
				SMs[num++].period = match.Groups[1].Value;
			}
		}
	}

	private void handleTickFct()
	{
		if (needsReading)
		{
			Match match = Regex.Match(curr_line, "static\\s*int\\s*(\\w+)_(\\w+)_(\\w+)\\s*=\\s*(\\d+)\\s*;\\s*");
			if (match.Success)
			{
				SMs[currentSM].stateLoopIniCand.Add(match.Groups);
			}
			if (curr_line.Trim() != string.Empty)
			{
				SMHelper sMHelper = SMs[currentSM];
				sMHelper.locals = sMHelper.locals + curr_line + "\r\n";
			}
		}
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("switch"))
		{
			Match match2 = Regex.Match(curr_line, "static\\s*int\\s*(\\w+)_(\\w+)_(\\w+)\\s*=\\s*(\\d+)\\s*;\\s*");
			if (match2.Success)
			{
				SMs[currentSM].stateLoopIniCand.Add(match2.Groups);
			}
			if (curr_line.Trim() != string.Empty)
			{
				SMHelper sMHelper2 = SMs[currentSM];
				sMHelper2.locals = sMHelper2.locals + curr_line + "\r\n";
			}
		}
		bracketStack.Clear();
		updateNesting(curr_line);
		while ((curr_line = readLine(readStream)) != null && bracketStack.Count < 1)
		{
		}
		do
		{
			if (!curr_line.Contains("case"))
			{
				continue;
			}
			curr_line = curr_line.Replace(':', ' ');
			string[] array = Regex.Split(curr_line.Trim(), "\\s+");
			if (array[1].Contains("-1"))
			{
				handleInitialState(curr_line);
				continue;
			}
			SMs[currentSM].currState = array[1].Substring(array[1].IndexOf('_') + 1);
			if (!SMs[currentSM].states.Contains(SMs[currentSM].currState))
			{
				SMs[currentSM].states.Add(SMs[currentSM].currState);
				SMs[currentSM].stateTrans[SMs[currentSM].currState] = new Dictionary<string, string>();
				SMs[currentSM].stateTransActions[SMs[currentSM].currState] = new Dictionary<string, string>();
				SMs[currentSM].stateTransHandles[SMs[currentSM].currState] = new Dictionary<string, KeyValuePair<Point, Point>>();
				SMs[currentSM].stateActions[SMs[currentSM].currState] = "";
				SMs[currentSM].stateType[SMs[currentSM].currState] = "basic";
				SMs[currentSM].stateFor[SMs[currentSM].currState] = new Dictionary<string, string>();
			}
			Match match3 = Regex.Match(curr_line, "//\\s*RIBS_EDGE_TYPE\\s*=\\s*(\\w+)");
			if (match3.Success)
			{
				if (match3.Groups[1].Value == "ifelse" || match3.Groups[1].Value == "for" || match3.Groups[1].Value == "basic")
				{
					SMs[currentSM].stateType[SMs[currentSM].currState] = match3.Groups[1].Value;
				}
				if (SMs[currentSM].stateType[SMs[currentSM].currState] == "for")
				{
					foreach (GroupCollection item in SMs[currentSM].stateLoopIniCand)
					{
						if (item[1].Value == SMs[currentSM].prefix && item[2].Value == SMs[currentSM].currState)
						{
							SMs[currentSM].stateFor[SMs[currentSM].currState]["initial"] = item[3].Value + "=" + item[4].Value;
							SMs[currentSM].currStateForVar = item[3].Value;
						}
					}
				}
			}
			handleTransitionCase(curr_line);
			SMs[currentSM].currStateForVar = string.Empty;
		}
		while (bracketStack.Count > 0 && (curr_line = readLine(readStream)) != null);
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("switch"))
		{
		}
		while ((curr_line = readLine(readStream)) != null && bracketStack.Count < 1)
		{
		}
		do
		{
			if (!curr_line.Contains("case"))
			{
				continue;
			}
			curr_line = curr_line.Replace(':', ' ');
			string[] array2 = Regex.Split(curr_line.Trim(), "\\s+");
			if (array2[1].Contains("-1"))
			{
				continue;
			}
			SMs[currentSM].currState = array2[1].Substring(array2[1].IndexOf('_') + 1);
			Match match4 = Regex.Match(curr_line, "//\\s*RIBS\\(X,Y\\)\\s*=\\s*\\((\\d+),(\\d+)\\)");
			if (match4.Success)
			{
				SMs[currentSM].statePos[SMs[currentSM].currState] = new Point(int.Parse(match4.Groups[1].Value), int.Parse(match4.Groups[2].Value));
			}
			string text = "";
			while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("break"))
			{
				if (text != string.Empty && !text.EndsWith("\n"))
				{
					text += "\r\n";
				}
				text += curr_line.Trim();
			}
			SMs[currentSM].stateActions[SMs[currentSM].currState] = text;
		}
		while (bracketStack.Count > 0 && (curr_line = readLine(readStream)) != null);
	}

	private void handleSMName()
	{
		needsReading = false;
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("TickFct"))
		{
		}
		string[] array = Regex.Split(curr_line.Trim(), "\\s+");
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num = -1;
			if (text.Contains("TickFct") && (num = text.IndexOf("_")) != -1)
			{
				SMs[currentSM].SM_Name = text.Substring(num + 1, text.IndexOf('(') - num - 1);
				break;
			}
		}
		if (!curr_line.Contains("{"))
		{
			while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("{"))
			{
			}
			if (curr_line.IndexOf("{") < curr_line.Length - 1)
			{
				curr_line = curr_line.Substring(curr_line.IndexOf("{") + 1);
				needsReading = true;
			}
		}
	}

	private void handlePrefix()
	{
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("_States"))
		{
			if (curr_line != null && curr_line.Contains("RxISR"))
			{
				SMs[currentSM].uartEnabled = true;
			}
		}
		string[] array = Regex.Split(curr_line.Trim(), "\\s+");
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num = -1;
			if ((num = text.IndexOf("_States")) != -1)
			{
				SMs[currentSM].prefix = text.Substring(0, num);
				break;
			}
		}
	}

	private void handleGlobals()
	{
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("RIBS_Globals begin"))
		{
		}
		curr_line = readLine(readStream);
		SMs[currentSM].globals = curr_line;
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("RIBS_Globals end"))
		{
			SMHelper sMHelper = SMs[currentSM];
			sMHelper.globals = sMHelper.globals + "\r\n" + curr_line;
		}
	}

	private void handleInitialState(string curr_line)
	{
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains(SMs[currentSM].prefix + "_State") && !curr_line.Contains("state"))
		{
		}
		string text = curr_line.Substring(curr_line.IndexOf("=") + 1);
		SMs[currentSM].initialState = text.Substring(text.IndexOf("_") + 1, text.IndexOf(";") - text.IndexOf("_") - 1);
		if (!SMs[currentSM].states.Contains(SMs[currentSM].initialState))
		{
			SMs[currentSM].states.Add(SMs[currentSM].initialState);
			SMs[currentSM].stateTrans[SMs[currentSM].initialState] = new Dictionary<string, string>();
			SMs[currentSM].stateTransActions[SMs[currentSM].initialState] = new Dictionary<string, string>();
			SMs[currentSM].stateTransHandles[SMs[currentSM].initialState] = new Dictionary<string, KeyValuePair<Point, Point>>();
			SMs[currentSM].stateActions[SMs[currentSM].initialState] = "";
			SMs[currentSM].stateType[SMs[currentSM].initialState] = "basic";
			SMs[currentSM].stateFor[SMs[currentSM].initialState] = new Dictionary<string, string>();
		}
	}

	private void handleTransitionCase(string curr_line)
	{
		while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("break"))
		{
			string value = "1";
			string text = "";
			string key = "";
			if (curr_line.Contains("if"))
			{
				int num = updateParenNesting(curr_line, 1);
				if (num == -1)
				{
					value = curr_line.Substring(curr_line.IndexOf("(") + 1);
					while ((curr_line = readLine(readStream)) != null && (num = updateParenNesting(curr_line, 1)) == -1 && parenStack.Count >= 1)
					{
						value += curr_line.Trim();
					}
					value += curr_line.Substring(0, num - 1).Trim();
				}
				else
				{
					value = curr_line.Substring(curr_line.IndexOf("(") + 1, num - curr_line.IndexOf("(") - 2);
				}
				if (SMs[currentSM].stateType[SMs[currentSM].currState] == "for")
				{
					Match match = Regex.Match(curr_line, "(\\w+)_(\\w+)_(\\w+)\\s*([<>=]+)\\s*(\\d+)\\s*");
					if (match.Success)
					{
						SMs[currentSM].stateFor[SMs[currentSM].currState]["condition"] = match.Groups[3].Value + match.Groups[4].Value + match.Groups[5].Value;
						string text2 = curr_line;
						while (bracketStack.Count <= 1 && (curr_line = readLine(readStream)) != null)
						{
						}
						if (curr_line == text2)
						{
							curr_line = readLine(readStream);
						}
						while (curr_line != null && bracketStack.Count > 1)
						{
							if (curr_line.Contains(SMs[currentSM].prefix + "_State") || curr_line.Contains("state"))
							{
								string text3 = curr_line.Substring(curr_line.IndexOf("=") + 1);
								key = text3.Substring(text3.IndexOf("_") + 1, text3.IndexOf(";") - text3.IndexOf("_") - 1);
								Match match2 = Regex.Match(curr_line, "//\\s*RIBS_EDGE\\s*=\\s*\\((\\d+),(\\d+)\\)\\((\\d+),(\\d+)\\)");
								if (match2.Success)
								{
									SMs[currentSM].stateTransHandles[SMs[currentSM].currState][key] = new KeyValuePair<Point, Point>(new Point(int.Parse(match2.Groups[1].Value), int.Parse(match2.Groups[2].Value)), new Point(int.Parse(match2.Groups[3].Value), int.Parse(match2.Groups[4].Value)));
								}
							}
							else if (curr_line != string.Empty)
							{
								Match match3 = Regex.Match(curr_line, "(\\w+)_(\\w+)_(\\w+)\\s*([^-\\s]+);\\s*");
								if (match3.Success)
								{
									SMs[currentSM].stateFor[SMs[currentSM].currState]["update"] = match3.Groups[3].Value + match3.Groups[4].Value;
									SMs[currentSM].stateTrans[SMs[currentSM].currState][key] = "for";
									SMs[currentSM].stateTransActions[SMs[currentSM].currState][key] = "";
									while ((curr_line = readLine(readStream)) != null && !curr_line.Contains("break"))
									{
										if (curr_line.Contains(SMs[currentSM].prefix + "_State") || curr_line.Contains("state"))
										{
											string text4 = curr_line.Substring(curr_line.IndexOf("=") + 1);
											key = text4.Substring(text4.IndexOf("_") + 1, text4.IndexOf(";") - text4.IndexOf("_") - 1);
											Match match4 = Regex.Match(curr_line, "//\\s*RIBS_EDGE\\s*=\\s*\\((\\d+),(\\d+)\\)\\((\\d+),(\\d+)\\)");
											if (match3.Success)
											{
												SMs[currentSM].stateTransHandles[SMs[currentSM].currState][key] = new KeyValuePair<Point, Point>(new Point(int.Parse(match4.Groups[1].Value), int.Parse(match4.Groups[2].Value)), new Point(int.Parse(match4.Groups[3].Value), int.Parse(match4.Groups[4].Value)));
											}
											SMs[currentSM].stateTrans[SMs[currentSM].currState][key] = "1";
											SMs[currentSM].stateTransActions[SMs[currentSM].currState][key] = "";
										}
									}
									return;
								}
							}
							curr_line = readLine(readStream);
						}
					}
				}
				else
				{
					curr_line = readLine(readStream);
					while (bracketStack.Count <= 1 && (curr_line = readLine(readStream)) != null)
					{
					}
					if (curr_line.Contains('{'))
					{
						curr_line = ((curr_line.IndexOf('{') >= curr_line.Length - 1) ? readLine(readStream) : curr_line.Substring(curr_line.IndexOf('{') + 1));
					}
					while (curr_line != null && bracketStack.Count > 1)
					{
						if (curr_line.Contains(SMs[currentSM].prefix + "_State") || curr_line.Contains("state"))
						{
							string text5 = curr_line.Substring(curr_line.IndexOf("=") + 1);
							key = text5.Substring(text5.IndexOf("_") + 1, text5.IndexOf(";") - text5.IndexOf("_") - 1);
							Match match5 = Regex.Match(curr_line, "//\\s*RIBS_EDGE\\s*=\\s*\\((\\d+),(\\d+)\\)\\((\\d+),(\\d+)\\)");
							if (match5.Success)
							{
								SMs[currentSM].stateTransHandles[SMs[currentSM].currState][key] = new KeyValuePair<Point, Point>(new Point(int.Parse(match5.Groups[1].Value), int.Parse(match5.Groups[2].Value)), new Point(int.Parse(match5.Groups[3].Value), int.Parse(match5.Groups[4].Value)));
							}
						}
						else
						{
							if (text != string.Empty && !curr_line.EndsWith("\n"))
							{
								text += "\r\n";
							}
							text += curr_line.Trim();
						}
						curr_line = readLine(readStream);
					}
				}
				SMs[currentSM].stateTrans[SMs[currentSM].currState][key] = value;
				SMs[currentSM].stateTransActions[SMs[currentSM].currState][key] = text;
			}
			else
			{
				if (!curr_line.Contains("else"))
				{
					continue;
				}
				curr_line = readLine(readStream);
				while (bracketStack.Count <= 1 && (curr_line = readLine(readStream)) != null)
				{
				}
				if (curr_line.Contains('{'))
				{
					curr_line = ((curr_line.IndexOf('{') >= curr_line.Length - 1) ? readLine(readStream) : curr_line.Substring(curr_line.IndexOf('{') + 1));
				}
				while (curr_line != null && bracketStack.Count > 1)
				{
					if (curr_line.Contains(SMs[currentSM].prefix + "_State") || curr_line.Contains("state"))
					{
						string text6 = curr_line.Substring(curr_line.IndexOf("=") + 1);
						key = text6.Substring(text6.IndexOf("_") + 1, text6.IndexOf(";") - text6.IndexOf("_") - 1);
						Match match6 = Regex.Match(curr_line, "//\\s*RIBS_EDGE\\s*=\\s*\\((\\d+),(\\d+)\\)\\((\\d+),(\\d+)\\)");
						if (match6.Success)
						{
							SMs[currentSM].stateTransHandles[SMs[currentSM].currState][key] = new KeyValuePair<Point, Point>(new Point(int.Parse(match6.Groups[1].Value), int.Parse(match6.Groups[2].Value)), new Point(int.Parse(match6.Groups[3].Value), int.Parse(match6.Groups[4].Value)));
						}
					}
					else
					{
						if (text != string.Empty && !curr_line.EndsWith("\n"))
						{
							text += "\r\n";
						}
						text += curr_line.Trim();
					}
					curr_line = readLine(readStream);
				}
				SMs[currentSM].stateTrans[SMs[currentSM].currState][key] = value;
				SMs[currentSM].stateTransActions[SMs[currentSM].currState][key] = text;
			}
		}
	}

	private string readLine(StreamReader read)
	{
		string text = null;
		text = read.ReadLine();
		updateNesting(text);
		return text;
	}

	private int updateParenNesting(string line, int level)
	{
		if (line == null)
		{
			return -1;
		}
		foreach (char c in line)
		{
			switch (c)
			{
			case '(':
				parenStack.Add(c);
				break;
			case ')':
				if (parenStack.Count >= 1)
				{
					parenStack.RemoveAt(parenStack.Count - 1);
				}
				break;
			}
		}
		if (parenStack.Count < level)
		{
			return line.LastIndexOf(')') + 1;
		}
		return -1;
	}

	private void updateNesting(string line)
	{
		if (line == null)
		{
			return;
		}
		foreach (char c in line)
		{
			switch (c)
			{
			case '{':
				bracketStack.Add(c);
				break;
			case '}':
				if (bracketStack.Count >= 1)
				{
					bracketStack.RemoveAt(bracketStack.Count - 1);
				}
				break;
			}
		}
	}

	private void buildSM()
	{
		ribsRef.SetPeriod(SMs[currentSM].period);
		ribsRef.setUartSMType(SMs[currentSM].uartEnabled, SMs[currentSM].async);
		if (SMs[currentSM].locals != string.Empty && SMs.Count > 1)
		{
			ribsRef.addLocalCode(currentSM, SMs[currentSM].locals);
		}
		else
		{
			ribsRef.addLocalCode(currentSM, SMs[currentSM].globals);
		}
		ribsRef.setSMName(currentSM, SMs[currentSM].SM_Name);
		ribsRef.setSMPrefix(currentSM, SMs[currentSM].prefix);
		foreach (string state in SMs[currentSM].states)
		{
			if (state == SMs[currentSM].initialState)
			{
				if (SMs[currentSM].statePos.Keys.Contains(state))
				{
					ribsRef.addState(state, SMs[currentSM].statePos[state], isInitial: true);
				}
				else
				{
					ribsRef.addState(state, isInitial: true);
				}
				ribsRef.addStateAction(SMs[currentSM].stateActions[state]);
			}
			else
			{
				if (SMs[currentSM].statePos.Keys.Contains(state))
				{
					ribsRef.addState(state, SMs[currentSM].statePos[state], isInitial: false);
				}
				else
				{
					ribsRef.addState(state, isInitial: false);
				}
				ribsRef.addStateAction(SMs[currentSM].stateActions[state]);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> stateTran in SMs[currentSM].stateTrans)
		{
			foreach (KeyValuePair<string, string> item in stateTran.Value)
			{
				if (item.Value == "for")
				{
					if (SMs[currentSM].stateType[stateTran.Key] == "for")
					{
						ribsRef.SetForEdge(stateTran.Key, SMs[currentSM].stateFor[stateTran.Key]["initial"], SMs[currentSM].stateFor[stateTran.Key]["condition"], SMs[currentSM].stateFor[stateTran.Key]["update"]);
					}
				}
				else if (SMs[currentSM].stateTransHandles.Keys.Contains(stateTran.Key) && SMs[currentSM].stateTransHandles[stateTran.Key].Keys.Contains(item.Key))
				{
					ribsRef.addTransition(stateTran.Key, item.Key, SMs[currentSM].stateTransHandles[stateTran.Key][item.Key].Key, SMs[currentSM].stateTransHandles[stateTran.Key][item.Key].Value, item.Value, SMs[currentSM].stateTransActions[stateTran.Key][item.Key]);
				}
				else
				{
					ribsRef.addTransition(stateTran.Key, item.Key, item.Value, SMs[currentSM].stateTransActions[stateTran.Key][item.Key]);
				}
			}
			if (SMs[currentSM].stateType[stateTran.Key] == "ifelse")
			{
				ribsRef.SetIfElseEdge(stateTran.Key);
			}
		}
	}
}
