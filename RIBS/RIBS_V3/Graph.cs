using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace RIBS_V3;

[Serializable]
public class Graph : ISerializable
{
    private int version;

    private InitState init;

    public InitEdge initedge;

    public List<Node> nodes;

    public List<Edge> edges;

    public int num_nodes;

    public int num_edges;

    public bool enable_timer;

    public bool enable_uart;

    private string name;

    private string abbrv;

    private string period;

    private string globalcode;

    private string localcode;

    private int current_node;

    public int x_offset;

    public int y_offset;

    public int shiftx;

    public int shifty;

    public Graph()
    {
        version = 2;
        init = new InitState();
        initedge = new InitEdge(init);
        nodes = new List<Node>();
        edges = new List<Edge>();
        period = "1000";
        num_nodes = 0;
        num_edges = 0;
        enable_timer = true;
        enable_uart = false;
        name = "State Machine";
        abbrv = "SM";
        globalcode = "/*Define user variables and functions for this state machine here.*/";
        localcode = "";
        current_node = -1;
        x_offset = 55;
        y_offset = 55;
        shiftx = 0;
        shifty = 0;
        nodes.Add(init);
        edges.Add(initedge);
    }

    public Graph(SerializationInfo info, StreamingContext ctxt)
    {
        try
        {
            version = (int)info.GetValue("version", typeof(int));
        }
        catch
        {
            version = 0;
        }
        nodes = (List<Node>)info.GetValue("nodes", typeof(List<Node>));
        edges = (List<Edge>)info.GetValue("edges", typeof(List<Edge>));
        num_nodes = (int)info.GetValue("num_nodes", typeof(int));
        num_edges = (int)info.GetValue("num_edges", typeof(int));
        enable_timer = (bool)info.GetValue("enable_timer", typeof(bool));
        enable_uart = (bool)info.GetValue("enable_uart", typeof(bool));
        name = (string)info.GetValue("name", typeof(string));
        abbrv = (string)info.GetValue("abbrv", typeof(string));
        period = (string)info.GetValue("period", typeof(string));
        globalcode = (string)info.GetValue("globalcode", typeof(string));
        localcode = (string)info.GetValue("localcode", typeof(string));
        current_node = (int)info.GetValue("current_node", typeof(int));
        x_offset = (int)info.GetValue("x_offset", typeof(int));
        y_offset = (int)info.GetValue("y_offset", typeof(int));
        switch (version)
        {
            case 1:
                init = (InitState)info.GetValue("init", typeof(InitState));
                initedge = (InitEdge)info.GetValue("initedge", typeof(InitEdge));
                shiftx = 0;
                shifty = 0;
                break;
            case 2:
                init = (InitState)info.GetValue("init", typeof(InitState));
                initedge = (InitEdge)info.GetValue("initedge", typeof(InitEdge));
                shiftx = (int)info.GetValue("shiftx", typeof(int));
                shifty = (int)info.GetValue("shifty", typeof(int));
                break;
            default:
                init = new InitState();
                initedge = new InitEdge(init);
                shiftx = 0;
                shifty = 0;
                break;
        }
        foreach (Node node in nodes)
        {
            if (node.IsInitial)
            {
                initedge.                H = node;
            }
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("version", version);
        info.AddValue("init", init);
        info.AddValue("initedge", initedge);
        info.AddValue("nodes", nodes);
        info.AddValue("edges", edges);
        info.AddValue("num_nodes", num_nodes);
        info.AddValue("num_edges", num_edges);
        info.AddValue("enable_timer", enable_timer);
        info.AddValue("enable_uart", enable_uart);
        info.AddValue("name", name);
        info.AddValue("abbrv", abbrv);
        info.AddValue("period", period);
        info.AddValue("globalcode", globalcode);
        info.AddValue("localcode", localcode);
        info.AddValue("current_node", current_node);
        info.AddValue("x_offset", x_offset);
        info.AddValue("y_offset", y_offset);
        info.AddValue("shiftx", shiftx);
        info.AddValue("shifty", shifty);
    }

    public void Shift(int dx, int dy)
    {
        foreach (Node node in nodes)
        {
            node.Shift(dx, dy);
        }
        foreach (Edge edge in edges)
        {
            edge.Shift(dx, dy);
        }
        shiftx += dx;
        shifty += dy;
    }

    public void Scale(Graphics g, float factor)
    {
        foreach (Node node in nodes)
        {
            node.Scale(g, factor);
        }
        foreach (Edge edge in edges)
        {
            edge.Scale(g, factor);
        }
    }

    public static Graph Open(string f)
    {
        try
        {
            StreamReader streamReader = new StreamReader(f);
            Graph graph = new Graph
            {
                name = streamReader.ReadLine().Replace("name: ", ""),
                abbrv = streamReader.ReadLine().Replace("abbrv: ", ""),
                period = streamReader.ReadLine().Replace("period: ", ""),
                num_nodes = Convert.ToInt32(streamReader.ReadLine().Replace("num_nodes: ", "")),
                num_edges = Convert.ToInt32(streamReader.ReadLine().Replace("num_edges: ", "")),
                globalcode = streamReader.ReadLine().Replace("globalcode: ", "").Replace("\\r\\n", "\r\n"),
                localcode = streamReader.ReadLine().Replace("localcode: ", "").Replace("\\r\\n", "\r\n"),
                enable_timer = Convert.ToBoolean(streamReader.ReadLine().Replace("enable_timer: ", "")),
                enable_uart = Convert.ToBoolean(streamReader.ReadLine().Replace("enable_uart: ", ""))
            };
            for (int i = 0; i < graph.num_nodes; i++)
            {
                if (streamReader.ReadLine() == "node:")
                {
                    Node node = new Node();
                    node.                    Name = streamReader.ReadLine().Replace("name: ", "");
                    node.                    ID = Convert.ToInt32(streamReader.ReadLine().Replace("id: ", ""));
                    node.                    IsInitial = Convert.ToBoolean(streamReader.ReadLine().Replace("isinitial: ", ""));
                    node.                    Actions = streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("actions: ", "");
                    node.SetRect(Convert.ToInt32(streamReader.ReadLine().Replace("x: ", "")), Convert.ToInt32(streamReader.ReadLine().Replace("y: ", "")));
                    graph.nodes.Add(node);
                }
            }
            for (int j = 0; j < graph.num_edges; j++)
            {
                if (streamReader.ReadLine() == "edge:")
                {
                    Edge edge = new Edge();
                    edge.SetID(Convert.ToInt32(streamReader.ReadLine().Replace("id: ", "")));
                    edge.                    T = graph.nodes[Convert.ToInt32(streamReader.ReadLine().Replace("tail: ", ""))];
                    edge.                    H = graph.nodes[Convert.ToInt32(streamReader.ReadLine().Replace("head: ", ""))];
                    edge.                    Actions = streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("actions: ", "");
                    edge.                    Condition = streamReader.ReadLine().Replace("\\r\\n", "\r\n").Replace("condition: ", "");
                    edge.                    IsBezier = Convert.ToBoolean(streamReader.ReadLine().Replace("isbezier: ", ""));
                    edge.arc.h1.X = Convert.ToInt32(streamReader.ReadLine().Replace("h1x: ", ""));
                    edge.arc.h1.Y = Convert.ToInt32(streamReader.ReadLine().Replace("h1y: ", ""));
                    edge.arc.h2.X = Convert.ToInt32(streamReader.ReadLine().Replace("h2x: ", ""));
                    edge.arc.h2.Y = Convert.ToInt32(streamReader.ReadLine().Replace("h2y: ", ""));
                    if (edge.H == edge.T)
                    {
                        edge.SetBezierSelf();
                    }
                    edge.                    H.AddEdge(edge);
                    edge.                    T.AddEdge(edge);
                    graph.edges.Add(edge);
                }
            }
            return graph;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    public void DeleteObject(Node n, Edge e, Graphics g)
    {
        if (n != null)
        {
            foreach (Edge edge in n.edges)
            {
                if (edge.H == n)
                {
                    foreach (Edge edge2 in edge.T.edges)
                    {
                        if (edge2.Priority > edge.Priority)
                        {
                            edge2.                            Priority = edge2.Priority - 1;
                        }
                    }
                    if (edge.H != edge.T)
                    {
                        edge.                        T.edges.Remove(edge);
                    }
                    if (edge.T.forloop_enabled)
                    {
                        edge.                        T.for_edge = null;
                    }
                }
                edges.Remove(edge);
                edge.Delete();
            }
            if (current_node == nodes.IndexOf(n))
            {
                current_node = -1;
            }
            nodes.Remove(n);
            if (n.IsInitial)
            {
                initedge.                H = null;
            }
            n.Delete();
        }
        else if (!e.T.forloop_enabled)
        {
            foreach (Edge edge3 in e.T.edges)
            {
                if (edge3.Priority > e.Priority)
                {
                    edge3.                    Priority = edge3.Priority - 1;
                }
            }
            edges.Remove(e);
            e.            T.edges.Remove(e);
            e.            H.edges.Remove(e);
            e.Delete();
        }
        else
        {
            edges.Remove(e);
            e.            T.for_edge = null;
            e.Delete();
        }
    }

    public int SetInitialState(Node n, bool enabled)
    {
        if (!enabled)
        {
            n.            IsInitial = false;
            initedge.            H = null;
            return -1;
        }
        int result = 0;
        bool flag = false;
        foreach (Node node in nodes)
        {
            if (node.IsInitial && n != node)
            {
                result = 0;
                flag = true;
            }
        }
        if (!flag)
        {
            initedge.            H = n;
            int dx = n.rect.X - init.rect.X - 100;
            int dy = n.rect.Y - init.rect.Y - 100;
            init.Shift(dx, dy);
            n.            IsInitial = true;
            result = 1;
        }
        return result;
    }

    public string InitialStateName
    {
        get
        {
            foreach (Node node in nodes)
            {
                if (node.IsInitial)
                {
                    return node.Name;
                }
            }
            return "";
        }
    }

    public Node AddNode(int x, int y)
    {
        Node node = new Node(x, y);
        node.        Name = "s" + nodes.Count;
        if (nodes.Count == 0)
        {
            SetInitialState(node, enabled: true);
        }
        nodes.Add(node);
        return node;
    }

    public Edge AddEdge()
    {
        Edge edge = new Edge();
        edges.Add(edge);
        foreach (Node node in nodes)
        {
            do
            {
                edge.arc.h1.Y++;
            }
            while (node.IsInRect(edge.arc.h1.X, edge.arc.h1.Y));
            do
            {
                edge.arc.h2.Y++;
            }
            while (node.IsInRect(edge.arc.h2.X, edge.arc.h2.Y));
        }
        return edge;
    }

    public void SetNewEdge(ref int result, ref Edge e, int x, int y)
    {
        foreach (Node node in nodes)
        {
            if (!node.IsInRect(x, y))
            {
                continue;
            }
            if (e.T == null)
            {
                if (!node.forloop_enabled)
                {
                    e.tail = node;
                    node.AddEdge(e);
                    result = 1;
                }
                else if (node.for_edge == null)
                {
                    edges.Remove(e);
                    e.Delete();
                    e = new ForLoopEdge(node)
                    {
                        tail = node
                    };
                    edges.Add(e);
                    node.for_edge = (ForLoopEdge)e;
                    result = 1;
                }
                else
                {
                    MessageBox.Show("Only one transition from a for-loop enabled state is allowed");
                    result = -1;
                }
            }
            else
            {
                SetHead(node, e);
                result = 2;
            }
        }
    }

    public void SetHead(Node n, Edge e)
    {
        if (n == null)
        {
            return;
        }
        e.        H = n;
        if (n == e.T)
        {
            e.SetBezierSelf();
        }
        else
        {
            e.ComputeBezier();
        }
        int num = 0;
        if (!e.T.forloop_enabled)
        {
            foreach (Edge edge in e.T.edges)
            {
                if (edge.T == e.T && edge.Priority > num && edge != e)
                {
                    num = edge.Priority;
                }
            }
        }
        e.        Priority = num + 1;
        n.AddEdge(e);
    }

    public Node GetNodeByName(string name)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].Name == name)
            {
                current_node = i;
                return nodes[i];
            }
        }
        return null;
    }

    public Node CurrentNode
    {
        get
        {
            if (current_node == -1 || current_node >= nodes.Count)
            {
                return null;
            }
            return nodes[current_node];
        }
    }

    public Node SetCurrentNode(int n)
    {
        if (current_node != -1 && current_node < nodes.Count)
        {
            nodes[current_node].            IsCurrentNode = false;
        }
        nodes[n].        IsCurrentNode = true;
        current_node = n;
        return nodes[n];
    }

    public void Draw(Graphics g, Panel p)
    {
        try
        {
            lock (p)
            {
                lock (g)
                {
                    g.Clear(Color.White);
                    foreach (Node node in nodes)
                    {
                        node.Draw(g);
                    }
                    initedge.Draw(g);
                }
            }
        }
        catch (InvalidOperationException)
        {
        }
    }

    public Node IsNode(int x, int y)
    {
        foreach (Node node in nodes)
        {
            if (node.IsInRect(x, y))
            {
                return node;
            }
        }
        return null;
    }

    public Edge IsEdge(int x, int y)
    {
        foreach (Edge edge in edges)
        {
            if (edge.PtOnBezier(x, y))
            {
                return edge;
            }
        }
        return null;
    }

    public int IsOverObject(int x, int y)
    {
        foreach (Node node in nodes)
        {
            if (node.IsInRect(x, y) && node != init)
            {
                return 1;
            }
        }
        foreach (Edge edge in edges)
        {
            switch (edge.PtOnHandle(x, y))
            {
                case 1:
                    return 2;
                case 2:
                    return 3;
            }
        }
        foreach (Edge edge2 in edges)
        {
            if (edge2.PtOnBezier(x, y))
            {
                return 4;
            }
        }
        return 0;
    }

    public void SetLocalCode_Text(string s)
    {
        globalcode = s;
    }

    public string GlobalCode => globalcode;

    public string LocalCode
    {
        get => localcode;
        set => localcode = value;
    }
    public string Abbrv
    {
        get => abbrv;
        set => abbrv = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }
    public string Period
    {
        get => period;
        set => period = value;
    }

    public int NumStates => nodes.Count;

    public int NumEdges => edges.Count;

    public bool EnableTimer
    {
        get => enable_timer;
        set => enable_timer = value;
    }
    public bool EnableUart
    {
        get => enable_uart;
        set => enable_uart = value;
    }

    public int YOffset => y_offset;

    public int XOffset => x_offset;

    public void CleanColors()
    {
        foreach (Node node in nodes)
        {
            node.            IsCurrentNode = false;
            node.SetIsSelected(b: false);
        }
        foreach (Edge edge in edges)
        {
            edge.is_selected = false;
        }
    }

    internal void UpdateNodePos(Node n, int x, int y)
    {
        int dx = x - n.rect.X;
        int dy = y - n.rect.Y;
        n.UpdatePos(x, y);
        if (n.IsInitial)
        {
            init.Shift(dx, dy);
        }
    }
}
