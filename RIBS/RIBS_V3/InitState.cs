using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class InitState : Node, ISerializable
{
	public InitState()
	{
	}

	public InitState(SerializationInfo info, StreamingContext ctxt)
		: base(info, ctxt)
	{
	}

	public override void Draw(Graphics g)
	{
		Rectangle rectangle = new Rectangle(rect.X + 55 - 12, rect.Y + 55 - 12, 13, 13);
		LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Black, Color.DarkGray, LinearGradientMode.Horizontal);
		g.FillEllipse(brush, rectangle);
	}
}
