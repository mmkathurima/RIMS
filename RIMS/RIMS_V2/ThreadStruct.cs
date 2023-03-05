using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct ThreadStruct
{
	private IntPtr handle;

	private uint tid;
}
