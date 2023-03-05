using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct InitStruct
{
	public IntPtr clock;

	public IntPtr breakpoint_pulse;
}
