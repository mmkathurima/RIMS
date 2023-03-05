using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
public struct MappedSymbolRecord
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string name;

	public uint value;
}
