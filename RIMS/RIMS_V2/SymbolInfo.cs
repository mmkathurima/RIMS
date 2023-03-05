using System;

namespace RIMS_V2;

public struct SymbolInfo : ICloneable
{
	public string name;

	public uint width;

	public int value;

	public object Clone()
	{
		SymbolInfo symbolInfo = default(SymbolInfo);
		symbolInfo.name = name;
		symbolInfo.width = width;
		symbolInfo.value = value;
		return symbolInfo;
	}
}
