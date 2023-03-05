using System;
using System.Windows.Forms;

namespace RIBS_V3;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		Application.EnableVisualStyles();
		Application.SetHighDpiMode(HighDpiMode.SystemAware);
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.DoEvents();
		Application.Run(new RIBS(args));
	}
}
