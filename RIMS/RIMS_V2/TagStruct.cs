using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
public struct TagStruct
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string file;

	public int line;

	public int aline;
}
