using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct VM_Struct
{
	public IntPtr vm;

	public IntPtr loader;

	public IntPtr assembler;
}
