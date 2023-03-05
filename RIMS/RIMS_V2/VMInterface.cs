using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RIMS_V2;

public class VMInterface
{
	private const string Module = "RIMS_DLL.dll";

	[DllImport("RIMS_DLL.dll")]
	public static extern void GetLastAsmLoc(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder sb, uint max_len);

	[DllImport("RIMS_DLL.dll")]
	public static extern void Step(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern IntPtr CreateVM();

	[DllImport("RIMS_DLL.dll")]
	public static extern void DestroyVM(IntPtr VM, IntPtr ThreadInfo);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetFilename(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] string filename);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetLccName(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] string filename);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetBaseDirectory(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] string dir);

	[DllImport("RIMS_DLL.dll")]
	public static extern void GetBaseDirectory(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder sb, uint max_len);

	[DllImport("RIMS_DLL.dll")]
	public static extern int Compile(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern int getCurrentLine(IntPtr VM, bool isAsm);

	[DllImport("RIMS_DLL.dll")]
	public static extern void executeInstruction(IntPtr VM, int opcode, string arg1, string arg2, string arg3, IntPtr result);

	[DllImport("RIMS_DLL.dll")]
	public static extern int Assemble(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void setExternal(IntPtr VM, bool val);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetErrors(IntPtr VM, IntPtr Errors);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint Initialize(IntPtr VM, IntPtr init_struct);

	[DllImport("RIMS_DLL.dll")]
	public static extern void ResetVM(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void Run(IntPtr VM, IntPtr ThreadInfo);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte GetPin(IntPtr VM, Pins which);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte SetPin(IntPtr VM, Pins which, byte what);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetTimerPeriod(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetTimerValue(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetIPP(IntPtr VM, uint ipp);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte IsUARTEnabled(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte GetUARTTx(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte GetUARTRx(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetNumDebugCharsWaiting(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte GetNextDebugChar(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetNextDebugBuffer(IntPtr VM, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sb, uint len);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte IsRunning(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte SetUnBroken(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern int GetIPS(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern double GetBatteryCharge(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void ResetBatteryCharge(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte IsUARTReady(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte IsUARTDone(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SendToUART(IntPtr VM, byte data);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte ReceiveFromUART(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SendToInput(IntPtr VM, char[] data, uint size);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetSymbol(IntPtr VM, int which, IntPtr symbol_record);

	[DllImport("RIMS_DLL.dll")]
	public static extern int GetSymbolIndex(IntPtr VM, [MarshalAs(UnmanagedType.LPStr)] string sb);

	[DllImport("RIMS_DLL.dll")]
	public static extern void GenerateSignalLog(IntPtr VM, [MarshalAs(UnmanagedType.LPWStr)] string filename);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetData(IntPtr VM, ushort addr, MemoryWidth width);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetData(IntPtr VM, ushort addr, MemoryWidth width, ulong data);

	[DllImport("RIMS_DLL.dll")]
	public static extern void AddBreakpoint(IntPtr VM, uint line);

	[DllImport("RIMS_DLL.dll")]
	public static extern void RemoveBreakpoint(IntPtr VM, uint line);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte IsBroken(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte AtBreakpoint(IntPtr VM);

	[DllImport("RIMS_DLL.dll", CharSet = CharSet.Unicode)]
	public static extern uint GetLine(IntPtr VM, IntPtr tagstruct);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint GetElapsedCycles(IntPtr VM);

	[DllImport("RIMS_DLL.dll", EntryPoint = "BreakImmediately")]
	public static extern void Break(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern uint Continue(IntPtr VM);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte SetStepModeVM(IntPtr VM, bool AStep);

	[DllImport("RIMS_DLL.dll")]
	public static extern byte SetNestedInterrupts(IntPtr VM, bool enable);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetASMMode(IntPtr VM, bool is_asm_mode);

	[DllImport("RIMS_DLL.dll")]
	public static extern void RegisterSymbol(IntPtr VM, IntPtr symbol_record);

	[DllImport("RIMS_DLL.dll")]
	public static extern void SetMappedIOWrite(IntPtr VM, MappedIOWrite cb);
}
