using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class InitEdge : Edge, ISerializable
{
	public InitEdge()
	{
	}

	public InitEdge(InitState tail)
	{
		is_bezier = false;
		T = tail;
	}

	public InitEdge(SerializationInfo info, StreamingContext ctxt)
		: base(info, ctxt)
	{
	}

	public override void Draw(Graphics g)
	{
		Pen pen = new Pen(Brushes.CornflowerBlue, 2f);
		if (head != null)
		{
			Point point = ComputeLaunchPoint(tail, head.CenterRect);
			Point point2 = ComputeLaunchPoint(head, tail.CenterRect);
			pen.Brush = is_selected ? Brushes.Red : Brushes.CornflowerBlue;
            DrawArrow(g, point2, point2.X - ComputeMidPoint(point, point2).X, point2.Y - ComputeMidPoint(point, point2).Y);
			g.DrawLine(pen, point, point2);
			Font font = new Font("Arial", 8f);
			Point point3 = default(Point);
			point3 = ComputeMidPoint(point, point2);
			if (condition.Length > 0)
			{
				point3.X -= 3 * (condition.Length - 2);
			}
			point3.X -= 30;
			point3.Y -= 15;
			if (actions.Length == 0)
			{
				g.DrawString(condition, font, Brushes.Black, point3);
			}
			else
			{
				g.DrawString(condition + " /\n" + actions, font, Brushes.Black, point3);
			}
		}
	}
}
