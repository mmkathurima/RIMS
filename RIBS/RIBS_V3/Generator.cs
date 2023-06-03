using System;
using System.Windows.Forms;

namespace RIBS_V3;

internal class Generator
{
	private Graph graph;

	private string output;

	private bool enablet;

	private bool enableu;

	private int CPriority;

	public Generator(Graph g)
	{
		graph = g;
		output = "";
		enablet = false;
		enableu = false;
	}

	public string GenerateCCode(bool et, bool eu)
	{
		enablet = et;
		enableu = eu;
		GenerateSingle();
		return output;
	}

	private void GenerateSingle()
	{
		output += GenerateHeader();
		if (enablet)
		{
			output = output + "unsigned char " + graph.Abbrv + "_Clk;\r\n";
			output = output + "void TimerISR() {\r\n   " + graph.Abbrv + "_Clk = 1;\r\n}\r\n\r\n";
		}
		if (enableu)
		{
			output = output + "unsigned char " + graph.Abbrv + "_rx_flag = 0;\r\n";
			output = output + "void RxISR() {\r\n   " + graph.Abbrv + "_rx_flag = 1;\r\n}\r\n\r\n";
		}
		output = output + "enum " + graph.Abbrv + "_States { ";
		int num = 0;
		foreach (Node node in graph.nodes)
		{
			num++;
			if (!(node.Name == "NONAME"))
			{
				if (node.Name.Length == 0)
				{
					MessageBox.Show("Error: All states must be named.", "Error");
					output = "";
					return;
				}
				output = output + graph.Abbrv + "_" + node.Name;
				if (num < graph.NumStates)
				{
					output += ", ";
				}
				else
				{
					output = output + " } " + graph.Abbrv + "_State;\r\n\r\n";
				}
			}
		}
		output = output + "TickFct_" + graph.Name.Replace(' ', '_') + "() {\r\n   ";
		foreach (Node node2 in graph.nodes)
		{
			if (node2.forloop_enabled)
			{
				string text = output;
				output = text + "static int " + graph.Abbrv + "_" + node2.Name + "_" + node2.loop.initial + ";\n   ";
				string[] array = node2.loop.initial.Split('=');
				node2.loop.loopvar = graph.Abbrv + "_" + node2.Name + "_" + array[0];
				string update = node2.loop.update;
				node2.loop.update_cvar = update.Replace(array[0], node2.loop.loopvar);
				update = node2.loop.condition;
				node2.loop.condition_cvar = update.Replace(array[0], node2.loop.loopvar);
			}
		}
		output += GenerateStateMachineExternal();
		output += "}\r\n\r\n";
		output += "int main() {\r\n\r\n   ";
		if (enablet)
		{
			string text2 = output;
			output = text2 + "const unsigned int period" + graph.Name.Replace(' ', '_') + " = " + graph.Period + ";";
			if (graph.Period == "1000")
			{
				output += " // 1000 ms default\r\n   ";
			}
			else
			{
				output += "\r\n   ";
			}
			output = output + "TimerSet(period" + graph.Name.Replace(' ', '_') + ");\r\n   TimerOn();\r\n   ";
		}
		if (enableu)
		{
			output += "UARTOn();\r\n\r\n";
		}
		if (graph.InitialStateName.Length != 0)
		{
			output = output + "\r\n   " + graph.Abbrv + "_State = -1; // Initial state\r\n   ";
			output += "B = 0; // Init outputs\r\n\r\n   ";
			output = output + "while(1) {\r\n      TickFct_" + graph.Name.Replace(' ', '_') + "();";
			if (enablet)
			{
				output = output + "\r\n      while(!" + graph.Abbrv + "_Clk);";
				output = output + "\r\n      " + graph.Abbrv + "_Clk = 0;\r\n   ";
			}
			output += "} // while (1)\r\n} // Main";
		}
		else
		{
			MessageBox.Show("Error: Initial state not selected", "Error");
			output = "";
		}
	}

	private string GenerateHeader()
	{
		DateTime dateTime = default(DateTime);
		dateTime = DateTime.Now;
		string text = "/*\r\nThis code was automatically generated using the Riverside-Irvine State machine Builder tool\r\nVersion " + 2.9 + " --- " + dateTime.Month + "/" + dateTime.Day + "/" + dateTime.Year + " " + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + " PST\r\n*/\r\n\r\n";
		text += "#include \"rims.h\"\r\n\r\n";
		if (graph.GlobalCode.Length > 0)
		{
			text = text + graph.GlobalCode + "\r\n";
		}
		return text;
	}

	private string GenerateStateMachineExternal()
	{
		string text = "";
		bool flag = false;
		text = text + "switch(" + graph.Abbrv + "_State) { // Transitions\r\n   ";
		text += "   case -1:\r\n";
		if (graph.initedge.Actions != "")
		{
			text = text + "         " + graph.initedge.Actions.Replace("\n", "\r\n         ") + "\r\n";
		}
		string text6 = text;
		text = text6 + "         " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n         break;\r\n      ";
		foreach (Node node in graph.nodes)
		{
			if (node.Name == "NONAME")
			{
				continue;
			}
			if (!node.priority_enabled)
			{
				_ = node.forloop_enabled;
			}
			string text7 = text;
			text = text7 + "   case " + graph.Abbrv + "_" + node.Name + ": ";
			bool flag2 = false;
			int index = -1;
			int num = 0;
			CPriority = 1;
			if (node.priority_enabled)
			{
				int num2 = 1;
				foreach (Edge edge in graph.edges)
				{
					if (edge.Condition.Length == 0)
					{
						MessageBox.Show("Error: Empty condition in transition from state " + edge.T.Name, "Error");
						return "";
					}
					if (edge.T == node)
					{
						num2++;
					}
				}
				while (CPriority < num2)
				{
					foreach (Edge edge2 in graph.edges)
					{
						bool flag3 = false;
						if (edge2.T == node)
						{
							if (edge2.Condition.ToLower() != "other")
							{
								if (edge2.Priority == CPriority && CPriority == 1)
								{
									string text8 = text;
									text = text8 + "\r\n         if (" + edge2.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge2.H.Name + ";";
									CPriority++;
									flag3 = true;
								}
								else if (edge2.Priority == CPriority)
								{
									string text9 = text;
									text = text9 + "\r\n         else if (" + edge2.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge2.H.Name + ";";
									CPriority++;
									flag3 = true;
								}
								if (edge2.Actions.Length > 0 && flag3)
								{
									text = text + "\r\n            " + edge2.Actions.Replace("\n", "\r\n            ") + "\r\n         }";
								}
								else if (flag3)
								{
									text += "\r\n         }";
								}
							}
							else
							{
								if (flag2)
								{
									MessageBox.Show("Error: Multiple 'other' transitions exist for state " + node.Name, "Error");
									return "";
								}
								flag2 = true;
								index = num;
							}
						}
						num++;
					}
				}
			}
			else
			{
				foreach (Edge edge3 in graph.edges)
				{
					if (edge3.Condition.Length == 0)
					{
						MessageBox.Show("Error: Empty condition in transition from state " + edge3.T.Name, "Error");
						return "";
					}
					if (node.forloop_enabled && !flag)
					{
						string text10 = text;
						text = text10 + "\r\n         if (" + node.loop.condition_cvar + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + node.Name + ";\n            " + node.loop.update_cvar + ";\n         }";
						flag = true;
					}
					if (edge3.T == node)
					{
						if (edge3.Condition.ToLower() != "other")
						{
							if (!flag)
							{
								string text11 = text;
								text = text11 + "\r\n         if (" + edge3.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge3.H.Name + ";";
								flag = true;
							}
							else
							{
								if (node.forloop_enabled)
								{
									string text12 = text;
									text = text12 + "\r\n         else if (!(" + node.loop.condition_cvar + ")) {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge3.H.Name + ";";
									string text13 = text;
									text = text13 + "\r\n            " + graph.Abbrv + "_" + node.Name + "_" + node.loop.initial + ";";
									text = ((edge3.Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + edge3.Actions.Replace("\n", "\r\n            ") + "\r\n         }"));
									num++;
									break;
								}
								string text2 = text;
								text = text2 + "\r\n         else if (" + edge3.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge3.H.Name + ";";
							}
							text = ((edge3.Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + edge3.Actions.Replace("\n", "\r\n            ") + "\r\n         }"));
						}
						else
						{
							if (flag2)
							{
								MessageBox.Show("Error: Multiple 'other' transitions exist for state " + node.Name, "Error");
								return "";
							}
							flag2 = true;
							index = num;
						}
					}
					num++;
				}
			}
			if (flag2)
			{
				string text3 = text;
				text = text3 + "\r\n         else {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.edges[index].H.Name + ";";
				text = ((graph.edges[index].Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + graph.edges[index].Actions.Replace("\n", "\r\n            ") + "\r\n         }"));
			}
			flag = false;
			text += "\r\n         break;\r\n   ";
		}
		string text4 = text;
		text = text4 + "   default:\r\n         " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n   } // Transitions\r\n\r\n   ";
		text = text + "switch(" + graph.Abbrv + "_State) { // State actions\r\n   ";
		foreach (Node node2 in graph.nodes)
		{
			if (!(node2.Name == "NONAME"))
			{
				string text5 = text;
				text = text5 + "   case " + graph.Abbrv + "_" + node2.Name + ":\r\n         ";
				text += node2.Actions.Replace("\n", "\n         ");
				if (node2.Actions.Length > 2 && node2.Actions[node2.Actions.Length - 2] != '\\')
				{
					text += "\r\n         ";
				}
				text += "break;\r\n   ";
			}
		}
		text += "   default: // ADD default behaviour below\r\n      break;\r\n   ";
		return text + "} // State actions\r\n\r\n";
	}

	private string GenerateStateMachineInternal()
	{
		string text = "";
		text = text + "switch(" + graph.Abbrv + "_State) { // State actions\r\n      ";
		foreach (Node node in graph.nodes)
		{
			string text2 = text;
			text = text2 + "   case " + graph.Abbrv + "_" + node.Name + ":\r\n         ";
			text += node.Actions;
			if (node.Actions.Length > 2 && node.Actions[node.Actions.Length - 2] != '\\')
			{
				text += "\r\n          ";
			}
			text += "break;\r\n      ";
		}
		text += "   default: // ADD default behaviour below\r\n         break;\r\n      ";
		text += "} // State actions\r\n\r\n";
		if (enablet)
		{
			text = text + "      while(!" + graph.Abbrv + "_Clk);\r\n      ";
			text = text + graph.Abbrv + "_Clk = 0;\r\n\r\n";
		}
		else
		{
			text += "\r\n      ";
		}
		bool flag = false;
		text = text + "      switch(" + graph.Abbrv + "_State) { // Transitions\r\n      ";
		foreach (Node node2 in graph.nodes)
		{
			if (!node2.priority_enabled)
			{
				_ = node2.forloop_enabled;
			}
			string text3 = text;
			text = text3 + "   case " + graph.Abbrv + "_" + node2.Name + ":";
			bool flag2 = false;
			int index = -1;
			int num = 0;
			foreach (Edge edge in graph.edges)
			{
				if (edge.Condition.Length == 0)
				{
					MessageBox.Show("Error: Empty condition in transition from state " + edge.T.Name, "Error");
					return "";
				}
				if (edge.T == node2)
				{
					if (edge.Condition.ToLower() != "other")
					{
						if (!flag)
						{
							string text4 = text;
							text = text4 + "\r\n            if (" + edge.Condition + ") {\r\n               " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge.H.Name + ";";
							flag = true;
						}
						else
						{
							string text5 = text;
							text = text5 + "\r\n            else if (" + edge.Condition + ") {\r\n               " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge.H.Name + ";";
						}
						text = ((edge.Actions.Length <= 0) ? (text + "\r\n            }") : (text + "\r\n               " + edge.Actions + "\r\n            }"));
					}
					else
					{
						if (flag2)
						{
							MessageBox.Show("Error: Multiple 'other' transitions exist for state " + node2.Name, "Error");
							return "";
						}
						flag2 = true;
						index = num;
					}
				}
				num++;
			}
			if (flag2)
			{
				string text6 = text;
				text = text6 + "\r\n            else {\r\n               " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.edges[index].H.Name + ";";
				text = ((graph.edges[index].Actions.Length <= 0) ? (text + "\r\n            }") : (text + "\r\n               " + graph.edges[index].Actions + "\r\n            }"));
			}
			flag = false;
			text += "\r\n            break;\r\n      ";
		}
		string text7 = text;
		return text7 + "   default:\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n      } // Transitions\r\n   ";
	}
}
