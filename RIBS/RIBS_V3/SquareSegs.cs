using System;
using System.Drawing;

namespace RIBS_V3;

internal class SquareSegs
{
	public LineSeg[] array;

	public SquareSegs()
	{
		array = new LineSeg[4];
	}

	public Point PointOnCircle(int radius, int angleInDegrees, Point origin)
	{
		int x = (int)((double)radius * Math.Cos((double)angleInDegrees * Math.PI / 180.0)) + origin.X;
		int y = (int)((double)radius * Math.Sin((double)angleInDegrees * Math.PI / 180.0)) + origin.Y;
		return new Point(x, y);
	}

	public void ComputeSquareSegsFromRect(Rectangle rect)
	{
		LineSeg lineSeg = new LineSeg();
		LineSeg lineSeg2 = new LineSeg();
		LineSeg lineSeg3 = new LineSeg();
		LineSeg lineSeg4 = new LineSeg();
		lineSeg.p.X = rect.Left;
		lineSeg.p.Y = rect.Top;
		lineSeg.q.X = rect.Right;
		lineSeg.q.Y = rect.Top;
		array[0] = lineSeg;
		lineSeg2.p.X = rect.Left;
		lineSeg2.p.Y = rect.Bottom;
		lineSeg2.q.X = rect.Right;
		lineSeg2.q.Y = rect.Bottom;
		array[1] = lineSeg2;
		lineSeg3.p.X = rect.Left;
		lineSeg3.p.Y = rect.Top;
		lineSeg3.q.X = rect.Left;
		lineSeg3.q.Y = rect.Bottom;
		array[2] = lineSeg3;
		lineSeg4.p.X = rect.Right;
		lineSeg4.p.Y = rect.Top;
		lineSeg4.q.X = rect.Right;
		lineSeg4.q.Y = rect.Bottom;
		array[3] = lineSeg4;
	}

	public void ComputeSquareSegsFromRectInCircle(Rectangle rect)
	{
		LineSeg lineSeg = new LineSeg();
		LineSeg lineSeg2 = new LineSeg();
		LineSeg lineSeg3 = new LineSeg();
		LineSeg lineSeg4 = new LineSeg();
		Point origin = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
		Point point = PointOnCircle(rect.Height / 2, 45, origin);
		Point point2 = PointOnCircle(rect.Height / 2, 135, origin);
		Point point3 = PointOnCircle(rect.Height / 2, 225, origin);
		Point point4 = PointOnCircle(rect.Height / 2, 315, origin);
		lineSeg.p.X = point.X;
		lineSeg.p.Y = point.Y;
		lineSeg.q.X = point2.X;
		lineSeg.q.Y = point2.Y;
		array[0] = lineSeg;
		lineSeg2.p.X = point3.X;
		lineSeg2.p.Y = point3.Y;
		lineSeg2.q.X = point4.X;
		lineSeg2.q.Y = point4.Y;
		array[1] = lineSeg2;
		lineSeg3.p.X = point.X;
		lineSeg3.p.Y = point.Y;
		lineSeg3.q.X = point3.X;
		lineSeg3.q.Y = point3.Y;
		array[2] = lineSeg3;
		lineSeg4.p.X = point2.X;
		lineSeg4.p.Y = point2.Y;
		lineSeg4.q.X = point4.X;
		lineSeg4.q.Y = point4.Y;
		array[3] = lineSeg4;
	}
}
