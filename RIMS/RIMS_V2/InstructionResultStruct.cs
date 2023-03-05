using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct InstructionResultStruct
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public byte[] bytes;

	public byte width;
}
