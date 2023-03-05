using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class Edge
{
	protected int version = 1;

	private int priority;

	public Node head;

	public Node tail;

	public ArcSeg arc;

	protected double[,] BezJ;

	protected Point[] BezPoints;

	protected Point[] DrawBezPoints;

	protected Point[] Intersect;

	protected Point arccenter;

	protected int id;

	protected int num_drawbezpts;

	protected string actions;

	protected string condition;

	public bool is_selected;

	public bool is_bezier;

	public Edge(SerializationInfo info, StreamingContext ctxt)
	{
		try
		{
			version = (int)info.GetValue("version", typeof(int));
		}
		catch
		{
			version = 0;
		}
		int num = version;
		if (num == 1)
		{
			priority = (int)info.GetValue("priority", typeof(int));
		}
		head = (Node)info.GetValue("head", typeof(Node));
		tail = (Node)info.GetValue("tail", typeof(Node));
		arc = (ArcSeg)info.GetValue("arc", typeof(ArcSeg));
		BezJ = (double[,])info.GetValue("BezJ", typeof(double[,]));
		BezPoints = (Point[])info.GetValue("BezPoints", typeof(Point[]));
		DrawBezPoints = (Point[])info.GetValue("DrawBezPoints", typeof(Point[]));
		Intersect = (Point[])info.GetValue("Intersect", typeof(Point[]));
		arccenter = (Point)info.GetValue("arccenter", typeof(Point));
		id = (int)info.GetValue("id", typeof(int));
		num_drawbezpts = (int)info.GetValue("num_drawbezpts", typeof(int));
		actions = (string)info.GetValue("actions", typeof(string));
		condition = (string)info.GetValue("condition", typeof(string));
		is_selected = (bool)info.GetValue("is_selected", typeof(bool));
		is_bezier = (bool)info.GetValue("is_bezier", typeof(bool));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("version", version);
		info.AddValue("head", head);
		info.AddValue("tail", tail);
		info.AddValue("priority", priority);
		info.AddValue("arc", arc);
		info.AddValue("BezJ", BezJ);
		info.AddValue("BezPoints", BezPoints);
		info.AddValue("DrawBezPoints", DrawBezPoints);
		info.AddValue("Intersect", Intersect);
		info.AddValue("arccenter", arccenter);
		info.AddValue("id", id);
		info.AddValue("num_drawbezpts", num_drawbezpts);
		info.AddValue("actions", actions);
		info.AddValue("condition", condition);
		info.AddValue("is_selected", is_selected);
		info.AddValue("is_bezier", is_bezier);
	}

	public Edge()
	{
		head = null;
		tail = null;
		priority = 0;
		num_drawbezpts = 0;
		actions = "";
		condition = "1";
		arc = new ArcSeg();
		BezJ = new double[Constants.MAXSEGS, 4];
		BezPoints = new Point[Constants.MAXSEGS - 1];
		DrawBezPoints = new Point[Constants.MAXSEGS - 1];
		Intersect = new Point[10];
		arccenter = default(Point);
		id = -1;
		is_selected = false;
		is_bezier = true;
		ComputeBezJ();
	}

	public Edge(Edge[] edges)
	{
		head = null;
		tail = null;
		priority = 0;
		num_drawbezpts = 0;
		actions = "";
		condition = "1";
		arc = new ArcSeg();
		BezJ = new double[Constants.MAXSEGS, 4];
		BezPoints = new Point[Constants.MAXSEGS - 1];
		DrawBezPoints = new Point[Constants.MAXSEGS - 1];
		Intersect = new Point[10];
		arccenter = default(Point);
		is_selected = false;
		is_bezier = true;
		for (int i = 0; i < edges.Length; i++)
		{
			if (edges[i] == null || edges[i].id == -1)
			{
				id = i;
				break;
			}
		}
		ComputeBezJ();
	}

	public void Delete()
	{
		SetID(-1);
	}

	public int GetPriority()
	{
		return priority;
	}

	public void SetPriority(int p)
	{
		priority = p;
	}

	public string GetActions()
	{
		return actions;
	}

	public string GetCondition()
	{
		return condition;
	}

	public void SetIsBezier(bool b)
	{
		is_bezier = b;
	}

	public void SetActions(string s)
	{
		actions = s;
	}

	public virtual void SetCondition(string s)
	{
		condition = s;
	}

	private int Fact(int n)
	{
		int num = 1;
		if (n == 0)
		{
			num = 1;
		}
		else
		{
			for (int i = 1; i <= n; i++)
			{
				num *= i;
			}
		}
		return num;
	}

	private int Comb(int n, int i)
	{
		return Fact(n) / (Fact(i) * Fact(n - i));
	}

	private void ComputeBezJ()
	{
		for (int i = 0; i < Constants.MAXSEGS; i++)
		{
			double num = (double)i / (double)Constants.MAXSEGS;
			for (int j = 0; j < 4; j++)
			{
				BezJ[i, j] = (double)Comb(3, j) * Math.Pow(num, j) * Math.Pow(1.0 - num, 3 - j);
			}
		}
	}

	public void SetH(Node h)
	{
		head = h;
	}

	public Node GetH()
	{
		return head;
	}

	public Node GetT()
	{
		return tail;
	}

	public void SetT(Node t)
	{
		tail = t;
	}

	public void SetID(int newid)
	{
		id = newid;
	}

	public void ConvertToLine()
	{
		if (tail != head)
		{
			is_bezier = false;
		}
	}

	public void ConvertToBezier()
	{
		is_bezier = true;
	}

	private Point SegmentIntersects(LineSeg v1, LineSeg v2)
	{
		Point result = default(Point);
		long num = v2.q.X - v2.p.X;
		long num3 = v2.q.Y - v2.p.Y;
		long num4 = v1.p.X - v1.q.X;
		long num5 = v1.p.Y - v1.q.Y;
		long num6 = v1.q.X - v2.p.X;
		long num7 = v1.q.Y - v2.p.Y;
		long num8 = num4 * num3 - num5 * num;
		if ((double)Math.Abs(num8) < 1E-06)
		{
			result.X = 0;
			result.Y = 0;
		}
		else
		{
			double num9 = 1.0 / (double)num8;
			double num10 = (double)(num4 * num7 - num5 * num6) * num9;
			double num2 = (double)(num * num7 - num3 * num6) * num9;
			if (num10 < 0.0 || num10 > 1.0 || num2 < 0.0 || num2 > 1.0)
			{
				result.X = 0;
				result.Y = 0;
			}
			else
			{
				result.X = v2.p.X + (int)Math.Truncate((double)num * num10);
				result.Y = v2.p.Y + (int)Math.Truncate((double)num3 * num10);
			}
		}
		return result;
	}

	protected Point ComputeLaunchPoint(Node n, Point p)
	{
		Point point = default(Point);
		point = n.CenterRect;
		SquareSegs squareSegs = new SquareSegs();
		LineSeg lineSeg = new LineSeg();
		Point point2 = default(Point);
		lineSeg.p.X = n.CenterRect.X;
		lineSeg.p.Y = n.CenterRect.Y;
		lineSeg.q.X = p.X;
		lineSeg.q.Y = p.Y;
		squareSegs.ComputeSquareSegsFromRect(n.Rect);
		for (int i = 0; i < 4; i++)
		{
			point2 = SegmentIntersects(squareSegs.array[i], lineSeg);
			if (point2.X != 0 || point2.Y != 0)
			{
				point = point2;
				break;
			}
		}
		return point;
	}

	protected Point ComputeLaunchPointBezOrigin(Node n, Point p)
	{
		Point point = default(Point);
		point = n.CenterRect;
		SquareSegs squareSegs = new SquareSegs();
		LineSeg lineSeg = new LineSeg();
		Point point2 = default(Point);
		lineSeg.p.X = n.CenterRect.X;
		lineSeg.p.Y = n.CenterRect.Y;
		lineSeg.q.X = p.X;
		lineSeg.q.Y = p.Y;
		squareSegs.ComputeSquareSegsFromRectInCircle(n.Rect);
		for (int i = 0; i < 4; i++)
		{
			point2 = SegmentIntersects(squareSegs.array[i], lineSeg);
			if (point2.X != 0 || point2.Y != 0)
			{
				point = point2;
				break;
			}
		}
		return point;
	}

	protected Point ComputeLaunchPointBez(Node n, Point p)
	{
		Point point = default(Point);
		point = n.CenterRect;
		SquareSegs squareSegs = new SquareSegs();
		LineSeg lineSeg = new LineSeg();
		Point point2 = default(Point);
		lineSeg.p.X = n.CenterRect.X;
		lineSeg.p.Y = n.CenterRect.Y;
		lineSeg.q.X = p.X;
		lineSeg.q.Y = p.Y;
		bool flag = false;
		squareSegs.ComputeSquareSegsFromRect(n.Rect);
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			LineSeg lineSeg2 = new LineSeg();
			for (int num2 = num_drawbezpts - 1; num2 > 0; num2--)
			{
				lineSeg2.p.X = BezPoints[num2].X;
				lineSeg2.p.Y = BezPoints[num2].Y;
				lineSeg2.q.X = BezPoints[num2 - 1].X;
				lineSeg2.q.Y = BezPoints[num2 - 1].Y;
				point2 = SegmentIntersects(squareSegs.array[i], lineSeg2);
				if ((point2.X != 0 || point2.Y != 0) && (num2 > num || !flag))
				{
					num = num2;
					point = point2;
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return point;
		}
		for (int j = 0; j < 4; j++)
		{
			point2 = SegmentIntersects(squareSegs.array[j], lineSeg);
			if (point2.X != 0 || point2.Y != 0)
			{
				point = point2;
				break;
			}
		}
		return point;
	}

	private int LineLength(Point p, Point q)
	{
		int num = q.X - p.X;
		int num2 = q.Y - p.Y;
		return (int)Math.Truncate(Math.Sqrt(num * num + num2 * num2));
	}

	public void SetBezierSelf()
	{
		arc.h1.X = GetT().CenterRect.X + 53;
		arc.h1.Y = GetT().CenterRect.Y - 80;
		arc.h2.X = GetT().CenterRect.X - 53;
		arc.h2.Y = GetT().CenterRect.Y - 85;
	}

	public void ComputeBezier()
	{
		Point p = ComputeLaunchPoint(tail, head.CenterRect);
		Point q = ComputeLaunchPoint(head, tail.CenterRect);
		int num = p.X + (q.X - p.X) / 2;
		int num3 = p.Y + (q.Y - p.Y) / 2;
		int num4 = LineLength(p, q);
		if (p.X - q.X == 0)
		{
			int num7 = num + 25;
			int num10 = num - 25;
		}
		else
		{
			double num11 = ((double)p.Y - (double)q.Y) / ((double)p.X - (double)q.X);
			if (Math.Abs(num11) > 0.3)
			{
				double num12 = -1.0 / num11;
				int num13 = num3 - (int)Math.Truncate(num12 * (double)num);
				int num14 = (int)Math.Truncate((double)num4 * 0.075);
				int num6 = num + num14;
				int num9 = num - num14;
				Math.Truncate(num12 * (double)num6);
				Math.Truncate(num12 * (double)num9);
			}
			else
			{
				int num5 = num;
				int num8 = num;
			}
		}
		int x = p.X + (q.X - p.X) / 2;
		int num2 = p.Y + (q.Y - p.Y) / 2;
		Random random = new Random();
		arc.h1.X = x;
		arc.h1.Y = num2 + random.Next(-30, 30);
		arc.h2 = arc.h1;
	}

	protected Point ComputeBezierPoints(Graphics g, Point psrc, Point h1, Point h2, Point pdst)
	{
		Pen pen = new Pen(Brushes.CornflowerBlue, 2f);
		Point[] array = new Point[Constants.MAXSEGS];
		LongPoint[] array2 = new LongPoint[4];
		Point[] array3 = new Point[4] { psrc, h1, h2, pdst };
		for (int k = 0; k < 4; k++)
		{
			array2[k] = new LongPoint();
			array2[k].x = (long)array3[k].X * 1000L;
			array2[k].y = (long)array3[k].Y * 1000L;
		}
		for (int j = 1; j < Constants.MAXSEGS; j++)
		{
			array[j].X = 0;
			array[j].Y = 0;
			for (int l = 0; l < 4; l++)
			{
				array[j].X += (int)Math.Truncate((double)array2[l].x * BezJ[j, l]);
				array[j].Y += (int)Math.Truncate((double)array2[l].y * BezJ[j, l]);
			}
			BezPoints[j - 1].X = array[j].X / 1000;
			BezPoints[j - 1].Y = array[j].Y / 1000;
			pen.Brush = Brushes.CornflowerBlue;
		}
		num_drawbezpts = 0;
		for (int i = 0; i < Constants.MAXSEGS - 1; i++)
		{
			if (head == tail)
			{
				if (!IsInRect(tail.Rect, BezPoints[i]) && !IsInRect(head.Rect, BezPoints[i]))
				{
					ref Point reference = ref DrawBezPoints[num_drawbezpts];
					reference = BezPoints[i];
					num_drawbezpts++;
				}
			}
			else
			{
				ref Point reference2 = ref DrawBezPoints[num_drawbezpts];
				reference2 = BezPoints[i];
				num_drawbezpts++;
			}
		}
		int num = Constants.MAXSEGS / 2;
		return BezPoints[num];
	}

	private Point ComputeBezLineIntersection(Node n)
	{
		LineSeg lineSeg = new LineSeg();
		SquareSegs squareSegs = new SquareSegs();
		squareSegs.ComputeSquareSegsFromRect(n.Rect);
		Point result = default(Point);
		Point point = default(Point);
		for (int i = 0; i < Constants.MAXSEGS - 2; i++)
		{
			lineSeg.p = BezPoints[i];
			lineSeg.q = BezPoints[i + 1];
			for (int j = 0; j < 4; j++)
			{
				point = SegmentIntersects(squareSegs.array[j], lineSeg);
				if (point.X != 0 || point.Y != 0)
				{
					result = point;
					break;
				}
			}
		}
		return result;
	}

	public virtual void Draw(Graphics g)
	{
		Pen pen = new Pen(Brushes.CornflowerBlue, 2f);
		lock (g)
		{
			if (is_bezier)
			{
				if (head == null || tail == null)
				{
					return;
				}
				Point pdst = ((head == tail) ? SourceFromPriority() : ComputeLaunchPointBezOrigin(head, tail.CenterRect));
				Point point;
				if (!tail.priority_enabled)
				{
					point = ComputeLaunchPointBezOrigin(tail, head.CenterRect);
				}
				else
				{
					point = SourceFromPriority();
					if (priority == 1)
					{
						g.DrawString(priority.ToString(), new Font("Arial", 6f), Brushes.Black, new Point(point.X + 4, point.Y));
					}
					else
					{
						g.DrawString(priority.ToString(), new Font("Arial", 6f), Brushes.Black, point);
					}
				}
				Point h = arc.h1;
				Point h2 = arc.h2;
				arccenter = ComputeBezierPoints(g, point, h, h2, pdst);
				Point point3 = ComputeLaunchPointBezOrigin(tail, head.CenterRect);
				Intersect[0] = point3;
				point3 = ComputeLaunchPointBez(head, tail.CenterRect);
				Intersect[1] = point3;
				if (is_selected)
				{
					DrawHandles(g, point, h, h2, pdst);
					pen.Brush = Brushes.Red;
				}
				else if (tail.priority_enabled)
				{
					int num = 0;
					int green;
					if (priority > 8)
					{
						green = 248;
						num = 200;
					}
					else
					{
						green = priority * 31;
						num = 0;
					}
					SolidBrush brush = new SolidBrush(Color.FromArgb(num, green, 255));
					pen.Brush = brush;
				}
				else
				{
					pen.Brush = Brushes.CornflowerBlue;
				}
				int num2 = 0;
				for (int i = 0; i < num_drawbezpts; i++)
				{
					if (!head.Rect.Contains(DrawBezPoints[i]))
					{
						num2++;
					}
				}
				Point[] array = new Point[num2];
				for (int j = 0; j < array.Length; j++)
				{
					ref Point reference = ref array[j];
					reference = DrawBezPoints[j];
				}
				if (array.Length > 1)
				{
					g.DrawLines(pen, array);
				}
				if (num_drawbezpts > 1)
				{
					DrawArrow(g, Intersect[1], DrawBezPoints[num_drawbezpts - 2].X - h2.X, DrawBezPoints[num_drawbezpts - 2].Y - h2.Y);
				}
				Font font = new Font("Arial", 8f);
				Point point4 = new Point(arccenter.X - 5, arccenter.Y + 5);
				if (condition.Length > 0)
				{
					point4.X -= 3 * (condition.Length - 2);
				}
				if (actions.Length == 0)
				{
					g.DrawString(condition, font, Brushes.Black, point4);
				}
				else
				{
					g.DrawString(condition + " /\n" + actions, font, Brushes.Black, point4);
				}
			}
			else
			{
				Point point2 = ComputeLaunchPoint(tail, head.CenterRect);
				Point pdst2 = ComputeLaunchPoint(head, tail.CenterRect);
				if (is_selected)
				{
					pen.Brush = Brushes.Red;
				}
				else
				{
					pen.Brush = Brushes.CornflowerBlue;
				}
				DrawArrow(g, pdst2, pdst2.X - ComputeMidPoint(point2, pdst2).X, pdst2.Y - ComputeMidPoint(point2, pdst2).Y);
				g.DrawLine(pen, point2, pdst2);
				Font font2 = new Font("Arial", 8f);
				Point point5 = default(Point);
				point5 = ComputeMidPoint(point2, pdst2);
				if (condition.Length > 0)
				{
					point5.X -= 3 * (condition.Length - 2);
				}
				if (actions.Length == 0)
				{
					g.DrawString(condition, font2, Brushes.Black, point5);
				}
				else
				{
					g.DrawString(condition + " /\n" + actions, font2, Brushes.Black, point5);
				}
			}
		}
	}

	private Point SourceFromPriority()
	{
		float num = priority * 40 - 60;
		int num2 = 27;
		Point centerRect = tail.CenterRect;
		float num3 = (float)((double)num2 * Math.Cos((double)num * Math.PI / 180.0)) + (float)centerRect.X;
		float num4 = (float)((double)num2 * Math.Sin((double)num * Math.PI / 180.0)) + (float)centerRect.Y;
		return new Point((int)num3, (int)num4);
	}

	protected Point ComputeMidPoint(Point p1, Point p2)
	{
		Point result = default(Point);
		int num = Math.Abs(p1.X - p2.X);
		int num2 = Math.Abs(p1.Y - p2.Y);
		result.X = ((p2.X > p1.X) ? (p1.X + num / 2) : (p2.X + num / 2));
		result.Y = ((p2.Y > p1.Y) ? (p1.Y + num2 / 2) : (p2.Y + num2 / 2));
		return result;
	}

	protected void DrawHandles(Graphics g, Point psrc, Point h1, Point h2, Point pdst)
	{
		Pen pen = new Pen(Brushes.Red, 1f);
		g.DrawLine(pen, psrc, h1);
		g.DrawLine(pen, h2, pdst);
		g.DrawEllipse(pen, h1.X - 10, h1.Y - 10, 10, 10);
		g.DrawEllipse(pen, h2.X - 10, h2.Y - 10, 10, 10);
	}

	protected void DrawArrow(Graphics g, Point tip, double dxdt, double dydt)
	{
		Point point = default(Point);
		Point point2 = default(Point);
		Point point3 = default(Point);
		double num = 0.0 - Angle(tip.X - head.CenterRect.X, tip.Y - head.CenterRect.Y);
		double num2 = Math.Cos(num);
		double num3 = Math.Sin(num);
		point.X = (int)Math.Truncate(num2 + 16.0 * num3);
		point.Y = (int)Math.Truncate(-1.0 * num3 + 16.0 * num2);
		point2.X = (int)Math.Truncate(num2 + num3);
		point2.Y = (int)Math.Truncate(-1.0 * num3 + num2);
		point3.X = (int)Math.Truncate(13.0 * num2 + 10.0 * num3);
		point3.Y = (int)Math.Truncate(-13.0 * num3 + 10.0 * num2);
		int num4 = tip.X - point3.X;
		int num5 = tip.Y - point3.Y;
		point.X += num4;
		point.Y += num5;
		point2.X += num4;
		point2.Y += num5;
		Point[] points = new Point[3] { point, point2, tip };
		if (!is_selected)
		{
			if (tail.priority_enabled)
			{
				int num6 = 0;
				int green;
				if (priority > 8)
				{
					green = 248;
					num6 = 200;
				}
				else
				{
					green = priority * 31;
					num6 = 0;
				}
				SolidBrush brush = new SolidBrush(Color.FromArgb(num6, green, 255));
				g.FillPolygon(brush, points);
			}
			else
			{
				g.FillPolygon(Brushes.CornflowerBlue, points);
			}
		}
		else
		{
			g.FillPolygon(Brushes.Red, points);
		}
	}

	private double Angle(double x, double y)
	{
		double num = 0.0;
		if (Math.Abs(x) < 1E-05)
		{
			if (Math.Abs(y) < 1E-05)
			{
				return 0.0;
			}
			if (y > 0.0)
			{
				return Math.PI / 2.0;
			}
			return 4.71238898038469;
		}
		if (x < 0.0)
		{
			return Math.Atan2(y, x) + Math.PI;
		}
		return Math.Atan2(y, x) + Math.PI;
	}

	public bool PtOnBezier(int x, int y)
	{
		Point pt = new Point(x, y);
		Point[] array = new Point[Constants.MAXSEGS];
		LongPoint[] array2 = new LongPoint[Constants.MAXSEGS];
		LongPoint[] array3 = new LongPoint[4];
		Point[] array4 = new Point[4]
		{
			Intersect[0],
			arc.h1,
			arc.h2,
			Intersect[1]
		};
		bool result = false;
		if (head == null)
		{
			return false;
		}
		if (is_bezier)
		{
			for (int i = 0; i < 4; i++)
			{
				array3[i] = new LongPoint();
				array3[i].x = array4[i].X * 1000;
				array3[i].y = array4[i].Y * 1000;
			}
			for (int j = 0; j < Constants.MAXSEGS; j++)
			{
				array2[j] = new LongPoint();
			}
			array2[0].x = 0L;
			array2[0].y = 0L;
			for (int k = 0; k < 4; k++)
			{
				array2[0].x = array2[0].x + (long)Math.Truncate((double)array3[k].x * BezJ[0, k]);
				array2[0].y = array2[0].y + (long)Math.Truncate((double)array3[k].y * BezJ[0, k]);
			}
			array[0].X = (int)(array2[0].x / 1000);
			array[0].Y = (int)(array2[0].y / 1000);
			for (int l = 1; l < Constants.MAXSEGS; l++)
			{
				array2[l].x = 0L;
				array2[l].y = 0L;
				for (int m = 0; m < 4; m++)
				{
					array2[l].x = array2[l].x + (long)Math.Truncate((double)array3[m].x * BezJ[l, m]);
					array2[l].y = array2[l].y + (long)Math.Truncate((double)array3[m].y * BezJ[l, m]);
				}
				array[l].X = (int)(array2[l].x / 1000);
				array[l].Y = (int)(array2[l].y / 1000);
				if (PtOnLine(array[l - 1], array[l], pt))
				{
					result = true;
				}
			}
		}
		else
		{
			double num = Dist(head.CenterRect, tail.CenterRect, pt);
			if (num == -1.0)
			{
				result = false;
			}
			else if (num > 0.0 && num < 5.0)
			{
				result = true;
			}
		}
		return result;
	}

	private double Dist(Point Pt1, Point Pt0, Point pt)
	{
		int num = Pt1.X - Pt0.X;
		int num2 = Pt1.Y - Pt0.Y;
		double num3 = Math.Sqrt(Math.Pow(num, 2.0) + Math.Pow(num2, 2.0));
		double num4 = (double)((pt.X - Pt0.X) * num + (pt.Y - Pt0.Y) * num2) / (double)(num * num + num2 * num2);
		if (num4 * (1.0 - num4) < 0.0)
		{
			return -1.0;
		}
		return (double)Math.Abs(num * (pt.Y - Pt0.Y) - num2 * (pt.X - Pt0.X)) / num3;
	}

	private bool PtOnLine(Point p1, Point p2, Point pt)
	{
		bool result = false;
		double value = p2.X - p1.X;
		double value2 = p2.Y - p1.Y;
		if (Math.Abs(value) > 0.0 && Math.Abs(value2) < Math.Abs(value))
		{
			double num2 = (pt.X - p1.X) / (p2.X - p1.X);
			if (num2 >= 0.0 && num2 <= 1.0)
			{
				double num3 = (double)p1.Y + (double)(p2.Y - p1.Y) * num2;
				if (num3 - 5.0 <= (double)pt.Y && (double)pt.Y <= num3 + 5.0)
				{
					result = true;
				}
			}
		}
		else if (Math.Abs(value2) > 0.0)
		{
			double num = (pt.Y - p1.Y) / (p2.Y - p1.Y);
			if (num >= 0.0 && num <= 1.0)
			{
				double num4 = (double)p1.X + (double)(p2.X - p1.X) * num;
				if (num4 - 5.0 <= (double)pt.X && (double)pt.X <= num4 + 5.0)
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int PtOnHandle(int x, int y)
	{
		if (PointInCircle(x, y, arc.h1))
		{
			return 1;
		}
		if (PointInCircle(x, y, arc.h2))
		{
			return 2;
		}
		return 0;
	}

	private bool PointInCircle2(int x, int y, Point p)
	{
		Rectangle rect = new Rectangle(p.X - 55, p.Y - 55, 55, 55);
		Point p2 = new Point(x, y);
		if (IsInRect(rect, p2))
		{
			return true;
		}
		return false;
	}

	private bool PointInCircle(int x, int y, Point p)
	{
		Rectangle rect = new Rectangle(p.X - 10, p.Y - 10, 10, 10);
		Point p2 = new Point(x, y);
		if (IsInRect(rect, p2))
		{
			return true;
		}
		return false;
	}

	public bool IsInRect(Rectangle rect, Point p)
	{
		if (!rect.Contains(p.X, p.Y))
		{
			return false;
		}
		return true;
	}

	public bool GetIsBezier()
	{
		return is_bezier;
	}

	public void Shift(int dx, int dy)
	{
		arc.h1.X += dx;
		arc.h1.Y += dy;
		arc.h2.X += dx;
		arc.h2.Y += dy;
	}

	public void Scale(Graphics g, float factor)
	{
		Point[] array = new Point[2]
		{
			new Point(arc.h1.X, arc.h1.Y),
			new Point(arc.h2.X, arc.h2.Y)
		};
		Matrix matrix = new Matrix();
		matrix.Scale(factor, factor);
		matrix.TransformPoints(array);
		arc.h1 = array[0];
		arc.h2 = array[1];
	}
}
