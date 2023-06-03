using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RIBS_V3;

internal class MultiGenerator
{
	private List<Graph> graphs;

	private string output;

	private string globes;

	private bool enableu;

	public MultiGenerator(string global)
	{
		graphs = new List<Graph>();
		globes = global;
		enableu = false;
		output = "";
	}

	private int GCD(int a, int b)
	{
		while (b != 0)
		{
			int num = a % b;
			a = b;
			b = num;
		}
		return a;
	}

	public string GenerateCCode(List<Graph> gr)
	{
		graphs = gr;
		foreach (Graph graph in graphs)
		{
			if (!graph.EnableTimer)
			{
				MessageBox.Show("Error: Multiple state machine generation is valid for synchronous state machines only.");
				return "";
			}
		}
		foreach (Graph graph2 in graphs)
		{
			if (graph2.EnableUart)
			{
				enableu = true;
			}
		}
		Generate();
		return output;
	}

	private bool CheckPeriods()
	{
		if (graphs.Count == 0)
		{
			return true;
		}
		for (int i = 1; i < graphs.Count; i++)
		{
			if (graphs[i - 1].Period != graphs[i].Period)
			{
				return false;
			}
		}
		return true;
	}

	private void Generate()
	{
		if (CheckPeriods())
		{
			GenerateMultiRR();
		}
		else
		{
			GenerateMultiRTOS();
		}
	}

	private void GenerateMultiRTOS()
	{
		output += GenerateHeader();
		output = output + globes + "\r\n";
		output += "typedef struct task {\r\n   int state;\r\n   unsigned long period;\r\n   unsigned long elapsedTime;\r\n   int (*TickFct)(int);\r\n} task;\r\n\r\n";
		object obj = output;
		output = string.Concat(obj, "task tasks[", graphs.Count, "];\r\n\r\n");
		if (enableu)
		{
			output += "unsigned char rx_flag = 0;\r\n";
			output += "void RxISR() {\r\n   rx_flag = 1;\r\n}\r\n";
		}
		object obj2 = output;
		output = string.Concat(obj2, "const unsigned char tasksNum = ", graphs.Count, ";\r\n");
		foreach (Graph graph in graphs)
		{
			string text = output;
			output = text + "const unsigned long period" + graph.Name.Replace(' ', '_') + " = " + graph.Period + ";\r\n";
		}
		output += "\r\nconst unsigned long tasksPeriodGCD = ";
		int num = Convert.ToInt32(graphs[0].Period);
		foreach (Graph graph2 in graphs)
		{
			num = GCD(num, Convert.ToInt32(graph2.Period));
		}
		output = output + num + ";\r\n\r\n";
		foreach (Graph graph3 in graphs)
		{
			output = output + "int TickFct_" + graph3.Name.Replace(' ', '_') + "(int state);\r\n";
		}
		output += "\r\nunsigned char processingRdyTasks = 0;\r\n";
		output += "void TimerISR() {\r\n";
		output += "   unsigned char i;\r\n";
		output += "   if (processingRdyTasks) {\r\n";
		output += "      printf(\"Period too short to complete tasks\\n\");\r\n";
		output += "   }\r\n";
		output += "   processingRdyTasks = 1;\r\n";
		output += "   for (i = 0; i < tasksNum; ++i) { // Heart of scheduler code\r\n";
		output += "      if ( tasks[i].elapsedTime >= tasks[i].period ) { // Ready\r\n";
		output += "         tasks[i].state = tasks[i].TickFct(tasks[i].state);\r\n";
		output += "         tasks[i].elapsedTime = 0;\r\n";
		output += "      }\r\n";
		output += "      tasks[i].elapsedTime += tasksPeriodGCD;\r\n";
		output += "   }\r\n";
		output += "   processingRdyTasks = 0;\r\n";
		output += "}\r\n";
		output += "int main() {\r\n";
		output += "   // Priority assigned to lower position tasks in array\r\n";
		output += "   unsigned char i=0;\r\n";
		for (int i = 0; i < graphs.Count; i++)
		{
			output += "   tasks[i].state = -1;\r\n";
			output = output + "   tasks[i].period = period" + graphs[i].Name.Replace(' ', '_') + ";\r\n";
			output += "   tasks[i].elapsedTime = tasks[i].period;\r\n";
			output = output + "   tasks[i].TickFct = &TickFct_" + graphs[i].Name.Replace(' ', '_') + ";\r\n\r\n";
			output += "   ++i;\r\n";
		}
		if (enableu)
		{
			output += "   UARTOn();\r\n";
		}
		output += "   TimerSet(tasksPeriodGCD);\r\n";
		output += "   TimerOn();\r\n   ";
		output += "\r\n   while(1) { Sleep(); }\r\n";
		output += "\r\n   return 0;\r\n}\r\n";
		foreach (Graph graph4 in graphs)
		{
			output = output + "\r\nenum " + graph4.Abbrv + "_States { ";
			int num2 = 0;
			foreach (Node node in graph4.nodes)
			{
				num2++;
				if (!(node.Name == "NONAME"))
				{
					if (node.Name.Length == 0)
					{
						MessageBox.Show("Error: All states must be named.", "Error");
						output = "";
						return;
					}
					output = output + graph4.Abbrv + "_" + node.Name;
					if (num2 < graph4.NumStates)
					{
						output += ", ";
					}
					else
					{
						output = output + " } " + graph4.Abbrv + "_State;\r\n";
					}
				}
			}
			string text2 = output;
			output = text2 + "int TickFct_" + graph4.Name.Replace(' ', '_') + "(int state) {\r\n   " + GenerateStateMachineExternalRTOS(graph4);
			output = output.Remove(output.Length - 3);
			output += "   return state;\r\n}\r\n\r\n";
		}
	}

	private void GenerateMultiRR()
	{
		output += GenerateHeader();
		output = output + globes + "\r\n";
		output += "unsigned char TimerFlag = 0;\r\nvoid TimerISR() {\r\n   TimerFlag = 1;\r\n}\r\n\r\n";
		if (enableu)
		{
			output += "unsigned char rx_flag = 0;\r\n";
			output += "void RxISR() {\r\n   rx_flag = 1;\r\n}\r\n";
		}
		foreach (Graph graph in graphs)
		{
			output = output + "\r\nenum " + graph.Abbrv + "_States { ";
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
			string text = output;
			output = text + "TickFct_" + graph.Name.Replace(' ', '_') + "() {\r\n   " + GenerateStateMachineExternal(graph);
		}
		output += "int main() {\r\n   ";
		output += "B = 0; //Init outputs\r\n   ";
		output = output + "TimerSet(" + graphs[0].Period + ");\r\n   ";
		output += "TimerOn();\r\n   ";
		if (enableu)
		{
			output += "UARTOn();\r\n";
		}
		foreach (Graph graph2 in graphs)
		{
			output = output + graph2.Abbrv + "_State = -1;\r\n   ";
		}
		output += "while(1) {\r\n      ";
		foreach (Graph graph3 in graphs)
		{
			output = output + "TickFct_" + graph3.Name.Replace(' ', '_') + "();\r\n      ";
		}
		output += "while (!TimerFlag);\r\n      TimerFlag = 0;\r\n   }\r\n}";
	}

	private string GenerateHeader()
	{
		DateTime dateTime = default(DateTime);
		dateTime = DateTime.Now;
		string text = "/*\r\nThis code was automatically generated using the Riverside-Irvine State machine Builder tool\r\nVersion " + 2.9 + " --- " + dateTime.Month + "/" + dateTime.Day + "/" + dateTime.Year + " " + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + " PST\r\n*/\r\n\r\n";
		return text + "#include \"rims.h\"\r\n\r\n";
	}

	private string GenerateStateMachineExternal(Graph graph)
	{
		string text = "";
		text = text + graph.GlobalCode + "\r\n";
		bool flag = false;
		text = text + "   switch(" + graph.Abbrv + "_State) { // Transitions\r\n   ";
		text += "   case -1:\r\n";
		if (graph.initedge.Actions != "")
		{
			text = text + "         " + graph.initedge.Actions.Replace("\n", "\r\n         ") + "\r\n";
		}
		string text2 = text;
		text = text2 + "         " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n         break;\r\n   ";
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
			string text3 = text;
			text = text3 + "   case " + graph.Abbrv + "_" + node.Name + ":";
			bool flag2 = false;
			int index = -1;
			int num = 0;
			foreach (Edge edge in graph.edges)
			{
				if (edge.Condition.Length == 0)
				{
					MessageBox.Show("Error: Empty condition in transition from state " + edge.T.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return "";
				}
				if (edge.T == node)
				{
					if (edge.Condition.ToLower() != "other")
					{
						if (!flag)
						{
							string text4 = text;
							text = text4 + "\r\n         if (" + edge.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge.H.Name + ";";
							flag = true;
						}
						else
						{
							string text5 = text;
							text = text5 + "\r\n         else if (" + edge.Condition + ") {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + edge.H.Name + ";";
						}
						text = ((edge.Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + edge.Actions + "\r\n         }"));
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
			if (flag2)
			{
				string text6 = text;
				text = text6 + "\r\n         else {\r\n            " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.edges[index].H.Name + ";";
				text = ((graph.edges[index].Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + graph.edges[index].Actions + "\r\n         }"));
			}
			flag = false;
			text += "\r\n         break;\r\n   ";
		}
		string text7 = text;
		text = text7 + "   default:\r\n         " + graph.Abbrv + "_State = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n      } // Transitions\r\n\r\n   ";
		text = text + "switch(" + graph.Abbrv + "_State) { // State actions\r\n";
		foreach (Node node2 in graph.nodes)
		{
			if (!(node2.Name == "NONAME"))
			{
				string text8 = text;
				text = text8 + "      case " + graph.Abbrv + "_" + node2.Name + ":\r\n         ";
				text += node2.Actions.Replace("\n", "\n         ");
				if (node2.Actions.Length > 2 && node2.Actions[node2.Actions.Length - 2] != '\\')
				{
					text += "\r\n         ";
				}
				text += "break;\r\n";
			}
		}
		text += "      default: // ADD default behaviour below\r\n         break;\r\n   ";
		return text + "} // State actions\r\n}\r\n";
	}

	private string GenerateStateMachineExternalRTOS(Graph graph)
	{
		string text = "";
		text = text + graph.GlobalCode + "\r\n";
		bool flag = false;
		text += "   switch(state) { // Transitions\r\n   ";
		text += "   case -1:\r\n";
		if (graph.initedge.Actions != "")
		{
			text = text + "         " + graph.initedge.Actions.Replace("\n", "\r\n         ") + "\r\n";
		}
		string text2 = text;
		text = text2 + "         state = " + graph.Abbrv + "_" + graph.InitialStateName + ";\r\n         break;\r\n   ";
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
			string text3 = text;
			text = text3 + "   case " + graph.Abbrv + "_" + node.Name + ":";
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
				if (edge.T == node)
				{
					if (edge.Condition.ToLower() != "other")
					{
						if (!flag)
						{
							string text4 = text;
							text = text4 + "\r\n         if (" + edge.Condition + ") {\r\n            state = " + graph.Abbrv + "_" + edge.H.Name + ";";
							flag = true;
						}
						else
						{
							string text5 = text;
							text = text5 + "\r\n         else if (" + edge.Condition + ") {\r\n            state = " + graph.Abbrv + "_" + edge.H.Name + ";";
						}
						text = ((edge.Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + edge.Actions + "\r\n         }"));
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
			if (flag2)
			{
				string text6 = text;
				text = text6 + "\r\n         else {\r\n            state = " + graph.Abbrv + "_" + graph.edges[index].H.Name + ";";
				text = ((graph.edges[index].Actions.Length <= 0) ? (text + "\r\n         }") : (text + "\r\n            " + graph.edges[index].Actions + "\r\n         }"));
			}
			flag = false;
			text += "\r\n         break;\r\n   ";
		}
		text += "   default:\r\n         state = -1;\r\n      } // Transitions\r\n\r\n   ";
		text += "switch(state) { // State actions\r\n";
		foreach (Node node2 in graph.nodes)
		{
			if (!(node2.Name == "NONAME"))
			{
				string text7 = text;
				text = text7 + "      case " + graph.Abbrv + "_" + node2.Name + ":\r\n         ";
				text += node2.Actions.Replace("\n", "\n         ");
				if (node2.Actions.Length > 2 && node2.Actions[node2.Actions.Length - 2] != '\\')
				{
					text += "\r\n         ";
				}
				text += "break;\r\n";
			}
		}
		text += "      default: // ADD default behaviour below\r\n         break;\r\n   ";
		return text + "} // State actions\r\n   " + graph.Abbrv + "_State = state;\r\n}\r\n";
	}
}
