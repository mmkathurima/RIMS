using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public class ArcSeg
{
	public Point h1;

	public Point h2;

	public ArcSeg()
	{
		h1 = default(Point);
		h2 = default(Point);
	}

	public ArcSeg(SerializationInfo info, StreamingContext ctxt)
	{
		h1 = (Point)info.GetValue("h1", typeof(Point));
		h2 = (Point)info.GetValue("h2", typeof(Point));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("h1", h1);
		info.AddValue("h2", h2);
	}
}
