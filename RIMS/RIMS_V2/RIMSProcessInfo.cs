using System.Diagnostics;

namespace RIMS_V2;

internal class RIMSProcessInfo
{
	public static string GetNewRIMSName()
	{
		Process[] processesByName = Process.GetProcessesByName("RIMS");
		return "RIMS_" + processesByName.Length;
	}
}
