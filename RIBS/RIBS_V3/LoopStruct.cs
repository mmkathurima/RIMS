using System;
using System.Runtime.Serialization;

namespace RIBS_V3;

[Serializable]
public struct LoopStruct : ISerializable
{
	private int version;

	public string initial;

	public string condition;

	public string update;

	public string condition_cvar;

	public string update_cvar;

	public string loopvar;

	public LoopStruct(SerializationInfo info, StreamingContext ctxt)
	{
		try
		{
			version = (int)info.GetValue("version", typeof(int));
		}
		catch
		{
			version = 1;
		}
		int num = version;
		if (num == 1)
		{
			initial = (string)info.GetValue("initial", typeof(string));
			condition = (string)info.GetValue("condition", typeof(string));
			update = (string)info.GetValue("update", typeof(string));
			condition_cvar = (string)info.GetValue("condition_cvar", typeof(string));
			update_cvar = (string)info.GetValue("update_cvar", typeof(string));
			loopvar = (string)info.GetValue("loopvar", typeof(string));
		}
		else
		{
			initial = "";
			condition = "";
			update = "";
			condition_cvar = "";
			update_cvar = "";
			loopvar = "";
		}
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("initial", initial);
		info.AddValue("condition", condition);
		info.AddValue("update", update);
		info.AddValue("condition_cvar", condition_cvar);
		info.AddValue("update_cvar", update_cvar);
		info.AddValue("loopvar", loopvar);
	}

	public string GetString()
	{
		return "for (" + initial + "; " + condition + "; " + update + ")";
	}
}
