using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class Node : ISerializable
{
    protected int version = 1;

    public Rectangle rect;

    public Rectangle for_rect;

    public ForLoopEdge for_edge;

    protected int id;

    protected bool isinitial;

    public List<Edge> edges;

    protected string name;

    protected string actions;

    public bool is_selected;

    protected bool is_current;

    public bool priority_enabled;

    public bool forloop_enabled;

    public LoopStruct loop;

    public Node(SerializationInfo info, StreamingContext ctxt)
    {
        try
        {
            version = (int)info.GetValue("version", typeof(int));
        }
        catch
        {
            version = 0;
        }
        rect = (Rectangle)info.GetValue("rect", typeof(Rectangle));
        id = (int)info.GetValue("id", typeof(int));
        isinitial = (bool)info.GetValue("isinitial", typeof(bool));
        edges = (List<Edge>)info.GetValue("edges", typeof(List<Edge>));
        name = (string)info.GetValue("name", typeof(string));
        actions = (string)info.GetValue("actions", typeof(string));
        is_selected = (bool)info.GetValue("is_selected", typeof(bool));
        is_current = (bool)info.GetValue("is_current", typeof(bool));
        int num = version;
        if (num == 1)
        {
            priority_enabled = (bool)info.GetValue("priority_enabled", typeof(bool));
            forloop_enabled = (bool)info.GetValue("forloop_enabled", typeof(bool));
            loop = (LoopStruct)info.GetValue("loop", typeof(LoopStruct));
            for_rect = (Rectangle)info.GetValue("for_rect", typeof(Rectangle));
            for_edge = (ForLoopEdge)info.GetValue("for_edge", typeof(ForLoopEdge));
        }
        else
        {
            priority_enabled = false;
            forloop_enabled = false;
            loop.initial = "";
            loop.condition = "";
            loop.update = "";
            loop.condition_cvar = "";
            loop.update_cvar = "";
            loop.loopvar = "";
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("version", version);
        info.AddValue("rect", rect);
        info.AddValue("id", id);
        info.AddValue("isinitial", isinitial);
        info.AddValue("edges", edges);
        info.AddValue("name", name);
        info.AddValue("actions", actions);
        info.AddValue("is_selected", is_selected);
        info.AddValue("is_current", is_current);
        info.AddValue("priority_enabled", priority_enabled);
        info.AddValue("forloop_enabled", forloop_enabled);
        info.AddValue("loop", loop);
        info.AddValue("for_rect", for_rect);
        info.AddValue("for_edge", for_edge);
    }

    public Node()
    {
        name = "NONAME";
        actions = "";
        id = -1;
        isinitial = false;
        is_selected = false;
        is_current = false;
        rect = new Rectangle(0, 0, 55, 55);
        edges = new List<Edge>();
        priority_enabled = false;
        forloop_enabled = false;
        loop.initial = "";
        loop.condition = "";
        loop.update = "";
        loop.condition_cvar = "";
        loop.update_cvar = "";
        loop.loopvar = "";
    }

    public Node(int x, int y)
    {
        actions = "";
        isinitial = false;
        is_selected = false;
        is_current = false;
        rect = new Rectangle(x, y, 55, 55);
        edges = new List<Edge>();
        priority_enabled = false;
        forloop_enabled = false;
        loop.initial = "";
        loop.condition = "";
        loop.update = "";
        loop.condition_cvar = "";
        loop.update_cvar = "";
        loop.loopvar = "";
    }

    public void Shift(int dx, int dy)
    {
        rect.X += dx;
        rect.Y += dy;
    }

    public void Scale(Graphics g, float factor)
    {
        Point[] array = new Point[3]
        {
            new Point(rect.Left, rect.Top),
            new Point(rect.Right, rect.Top),
            new Point(rect.Left, rect.Bottom)
        };
        Matrix matrix = new Matrix();
        matrix.Scale(factor, factor);
        matrix.TransformPoints(array);
        rect = new Rectangle(array[0], new Size(array[1].X - array[0].X, array[2].Y - array[0].Y));
    }

    public void Delete()
    {
    }

    public void AddEdge(Edge e)
    {
        if (!edges.Contains(e))
        {
            edges.Add(e);
        }
    }

    public Rectangle Rect => rect;

    public Point CenterRect => new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);

    public int ID
    {
        get => id;
        set => id = value;
    }

    public string Actions
    {
        get => actions;
        set => actions = value;
    }

    public bool IsInitial
    {
        get => isinitial;
        set => isinitial = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public void SetRect(int x, int y)
    {
        rect = new Rectangle(x, y, 55, 55);
    }

    public void SetIsInitial(bool b, Graphics g)
    {
        isinitial = b;
        Draw(g);
    }

    public bool IsCurrentNode
    {
        get => is_current;
        set
        {
            is_current = value;
            if (!value)
            {
                rect.Height = 55;
                rect.Width = 55;
            }
            else
            {
                rect.Height = 75;
                rect.Width = 75;
            }
        }
    }

    public void SetIsSelected(bool b)
    {
        is_selected = b;
    }

    public virtual void Draw(Graphics g)
    {
        lock (g)
        {
            foreach (Edge edge in edges)
            {
                if (edge.T == this)
                {
                    edge.Draw(g);
                }
            }
            Pen pen = new Pen(Color.Black, 2f);
            g.DrawEllipse(pen, rect);
            if (is_current)
            {
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Green, Color.Goldenrod, LinearGradientMode.Horizontal);
                g.FillEllipse(brush, rect);
            }
            else if (!is_selected)
            {
                LinearGradientBrush brush2 = new LinearGradientBrush(rect, Color.Wheat, Color.Goldenrod, LinearGradientMode.Horizontal);
                g.FillEllipse(brush2, rect);
            }
            else
            {
                LinearGradientBrush brush3 = new LinearGradientBrush(rect, Color.Red, Color.Goldenrod, LinearGradientMode.Horizontal);
                g.FillEllipse(brush3, rect);
            }
            if (priority_enabled)
            {
                LinearGradientBrush brush4 = new LinearGradientBrush(rect, Color.Black, Color.Green, LinearGradientMode.ForwardDiagonal);
                Point[] array = new Point[3];
                array[0].X = CenterRect.X + 5;
                array[0].Y = CenterRect.Y - 27 + 5;
                array[1].X = CenterRect.X + 20;
                array[1].Y = CenterRect.Y - 27 + 7;
                array[2].X = CenterRect.X + 10;
                array[2].Y = CenterRect.Y - 27 - 5;
                g.FillPolygon(brush4, array);
                Pen pen2 = new Pen(Color.Black, 4f);
                Point[] points = new Point[4]
                {
                    new Point(CenterRect.X - 8, CenterRect.Y - 27 + 1),
                    new Point(CenterRect.X, CenterRect.Y - 27),
                    new Point(CenterRect.X + 4, CenterRect.Y - 27),
                    new Point(CenterRect.X + 8, CenterRect.Y - 27 + 1)
                };
                g.DrawLines(pen2, points);
            }
            else if (forloop_enabled)
            {
                string @string = loop.GetString();
                SizeF sizeF = g.MeasureString(@string, new Font("Arial", 8f));
                for_rect = new Rectangle(new Point(CenterRect.X - (int)sizeF.Width / 2, rect.Location.Y - 15), new Size((int)sizeF.Width + 1, 15));
                g.DrawRectangle(new Pen(Color.Black), for_rect);
                g.DrawString(loop.GetString(), new Font("Arial", 8f), Brushes.Black, for_rect);
                if (for_edge != null)
                {
                    for_edge.Draw(g);
                }
            }
            Font font = new Font("Arial", 10f);
            Point point = new Point(CenterRect.X - 9, CenterRect.Y - 5);
            if (name.Length > 2)
            {
                point.X -= 3 * (name.Length - 2);
            }
            g.DrawString(name, font, Brushes.Black, point);
            point.X = CenterRect.X - 27;
            point.Y = CenterRect.Y + 27 + 1;
            g.DrawString(actions, font, Brushes.Black, point);
        }
    }

    public void UpdatePos(int x, int y)
    {
        rect.X = x;
        rect.Y = y;
    }

    public bool IsInRect(int x, int y)
    {
        if (!rect.Contains(x, y))
        {
            return false;
        }
        return true;
    }

    private int ComputeActionsHeight()
    {
        string text = actions;
        int num = 1;
        for (int i = 0; i < text.Length - 1; i++)
        {
            if (text[i] == '\r' && text[i + 1] == '\n')
            {
                num++;
            }
        }
        return num;
    }

    private int ComputeActionsWidth()
    {
        string text = actions;
        int num = 0;
        int num2 = 0;
        for (int i = 0; i < text.Length - 1; i++)
        {
            if (text[i] == '\r' && text[i + 1] == '\n')
            {
                if (num > num2)
                {
                    num2 = num;
                }
                num = 0;
                i += 2;
            }
            else
            {
                num++;
            }
        }
        if (num > num2)
        {
            num2 = num;
        }
        return num2;
    }

    public Rectangle ForRect => for_rect;
}
