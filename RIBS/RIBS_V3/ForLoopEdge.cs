using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class ForLoopEdge : Edge, ISerializable
{
	public ForLoopEdge()
	{
	}

	public ForLoopEdge(Node tail)
	{
		SetT(tail);
		is_bezier = true;
	}

	public ForLoopEdge(SerializationInfo info, StreamingContext ctxt)
		: base(info, ctxt)
	{
	}

	public override void Draw(Graphics g)
	{
		Pen pen = new Pen(Brushes.CornflowerBlue, 2f);
		if (head != null)
		{
			Point point = new Point(tail.ForRect.X + tail.ForRect.Width, tail.ForRect.Y + 3);
			Point pdst = ComputeLaunchPoint(head, point);
			Point h = arc.h1;
			Point h2 = arc.h2;
			arccenter = ComputeBezierPoints(g, point, h, h2, pdst);
			Intersect[0] = point;
			ref Point reference = ref Intersect[1];
			reference = ComputeLaunchPoint(head, point);
			if (is_selected)
			{
				DrawHandles(g, point, h, h2, pdst);
				pen.Brush = Brushes.Red;
			}
			else
			{
				pen.Brush = Brushes.CornflowerBlue;
			}
			Point[] array = new Point[num_drawbezpts];
			for (int i = 0; i < array.Length; i++)
			{
				ref Point reference2 = ref array[i];
				reference2 = DrawBezPoints[i];
			}
			if (array.Length > 1)
			{
				g.DrawLines(pen, array);
			}
			if (num_drawbezpts > 1)
			{
				DrawArrow(g, DrawBezPoints[num_drawbezpts - 1], DrawBezPoints[num_drawbezpts - 2].X - h2.X, DrawBezPoints[num_drawbezpts - 2].Y - h2.Y);
			}
			Font font = new Font("Arial", 8f);
			Point point2 = new Point(arccenter.X - 5, arccenter.Y + 5);
			if (condition.Length > 0)
			{
				point2.X -= 3 * (condition.Length - 2);
			}
			if (actions.Length == 0)
			{
				string s = "!(" + tail.loop.condition + ")";
				g.DrawString(s, font, Brushes.Black, point2);
			}
			else
			{
				string text = "!(" + tail.loop.condition + ")";
				g.DrawString(text + " /\n" + actions, font, Brushes.Black, point2);
			}
		}
	}

	public override void SetCondition(string s)
	{
	}
}
