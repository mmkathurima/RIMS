using System;
using System.Runtime.InteropServices;

namespace RIMS_V2;

public class TimerInterface
{
	public static uint INFINITE = 268435455u;

	[DllImport("winmm.dll")]
	public static extern uint timeBeginPeriod(uint period);

	[DllImport("winmm.dll")]
	public static extern uint timeEndPeriod(uint period);

	[DllImport("winmm.dll")]
	public static extern uint timeSetEvent(uint delay, uint resolution, IntPtr callback, IntPtr user, uint Event);

	[DllImport("winmm.dll")]
	public static extern uint timeKillEvent(uint timer_id);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	[DllImport("kernel32.dll")]
	public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

	[DllImport("kernel32.dll")]
	public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);

	[DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
	public static extern int WaitForSingleObject(IntPtr handle, int milliseconds);

	[DllImport("kernel32.dll")]
	public static extern uint SleepEx(uint dwMilliseconds, bool bAlertable);
}
