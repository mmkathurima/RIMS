using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct ErrorStruct
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	public IntPtr[] errors;
}
