using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SymbolRecord
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string name;

	public uint address;

	public uint content_length;

	public byte in_data_segment;
}
