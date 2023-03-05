using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace RIBS_V3;

[Serializable]
public class Project
{
    public List<Graph> graphs;

    public string pname;

    public string macrocode;

    public int version;

    public Project()
    {
        version = 1;
        graphs = new List<Graph>();
    }

    public Project(List<Graph> g, string n, string c)
    {
        version = 1;
        graphs = g;
        pname = n;
        macrocode = c;
    }

    public Project(SerializationInfo info, StreamingContext ctxt)
    {
        try
        {
            version = (int)info.GetValue("version", typeof(int));
        }
        catch
        {
            version = 0;
        }
        graphs = (List<Graph>)info.GetValue("graphs", typeof(List<Graph>));
        pname = (string)info.GetValue("pname", typeof(string));
        macrocode = (string)info.GetValue("macrocode", typeof(string));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("version", version);
        info.AddValue("graphs", graphs);
        info.AddValue("pname", pname);
        info.AddValue("macrocode", macrocode);
    }

    public static Project LegacyOpen(string f)
    {
        Project project = new Project();
        StreamReader streamReader = new StreamReader(f);
        string text = streamReader.ReadLine();
        if (!text.Contains("pname"))
        {
            Graph graph = new Graph();
            graph = Graph.Open(f);
            project.graphs.Add(graph);
            project.pname = "Project";
            streamReader.Close();
        }
        else
        {
            project.pname = text.Replace("pname: ", "");
            string value = streamReader.ReadLine().Replace("Number of Graphs: ", "");
            double num = Convert.ToInt32(value);
            if (num > 1.0)
            {
                project.macrocode = streamReader.ReadLine().Replace("Macrocode: ", "").Replace("\\r\\n", "\r\n");
            }
            else
            {
                streamReader.ReadLine();
            }
            for (int i = 0; (double)i < num; i++)
            {
                try
                {
                    Graph graph2 = new Graph
                    {
                        Name = streamReader.ReadLine().Replace("name: ", ""),
                        Abbrv = streamReader.ReadLine().Replace("abbrv: ", ""),
                        Period = streamReader.ReadLine().Replace("period: ", "")
                    };
                    string value2 = streamReader.ReadLine().Replace("num_nodes: ", "");
                    graph2.num_nodes = Convert.ToInt32(value2);
                    graph2.num_edges = Convert.ToInt32(streamReader.ReadLine().Replace("num_edges: ", ""));
                    graph2.SetLocalCode_Text(streamReader.ReadLine().Replace("globalcode: ", "").Replace("\\r\\n", "\r\n"));
                    graph2.LocalCode = streamReader.ReadLine().Replace("localcode: ", "").Replace("\\r\\n", "\r\n");
                    graph2.EnableTimer = Convert.ToBoolean(streamReader.ReadLine().Replace("enable_timer: ", ""));
                    graph2.EnableUart = Convert.ToBoolean(streamReader.ReadLine().Replace("enable_uart: ", ""));
                    for (int j = 0; j < graph2.num_nodes; j++)
                    {
                        if (streamReader.ReadLine() == "node:")
                        {
                            Node node = new Node
                            {
                                Name = streamReader.ReadLine().Replace("name: ", ""),
                                ID = Convert.ToInt32(streamReader.ReadLine().Replace("id: ", "")),
                                IsInitial = Convert.ToBoolean(streamReader.ReadLine().Replace("isinitial: ", "")),
                                Actions = streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("actions: ", "")
                            };
                            node.SetRect(Convert.ToInt32(streamReader.ReadLine().Replace("x: ", "")), Convert.ToInt32(streamReader.ReadLine().Replace("y: ", "")));
                            graph2.nodes.Add(node);
                        }
                    }
                    for (int k = 0; k < graph2.num_edges; k++)
                    {
                        if (streamReader.ReadLine() == "edge:")
                        {
                            Edge edge = new Edge();
                            edge.SetID(Convert.ToInt32(streamReader.ReadLine().Replace("id: ", "")));
                            edge.SetT(graph2.nodes[Convert.ToInt32(streamReader.ReadLine().Replace("tail: ", ""))]);
                            edge.SetH(graph2.nodes[Convert.ToInt32(streamReader.ReadLine().Replace("head: ", ""))]);
                            edge.SetActions(streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("actions: ", ""));
                            edge.SetCondition(streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("condition: ", ""));
                            edge.SetIsBezier(Convert.ToBoolean(streamReader.ReadLine().Replace("isbezier: ", "")));
                            edge.arc.h1.X = Convert.ToInt32(streamReader.ReadLine().Replace("h1x: ", ""));
                            edge.arc.h1.Y = Convert.ToInt32(streamReader.ReadLine().Replace("h1y: ", ""));
                            edge.arc.h2.X = Convert.ToInt32(streamReader.ReadLine().Replace("h2x: ", ""));
                            edge.arc.h2.Y = Convert.ToInt32(streamReader.ReadLine().Replace("h2y: ", ""));
                            if (edge.GetH() == edge.GetT())
                            {
                                edge.SetBezierSelf();
                            }
                            edge.GetH().AddEdge(edge);
                            edge.GetT().AddEdge(edge);
                            graph2.edges.Add(edge);
                        }
                    }
                    project.graphs.Add(graph2);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: Statemachine file is corrupt.", "Error");
                    return null;
                }
            }
        }
        streamReader.Close();
        return project;
    }
}
