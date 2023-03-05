using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void MappedIOWrite(IntPtr map_record);
