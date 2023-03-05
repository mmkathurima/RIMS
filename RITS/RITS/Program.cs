using System;
using System.Windows.Forms;

namespace RITS;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		Application.EnableVisualStyles();
		Application.SetHighDpiMode(HighDpiMode.SystemAware);
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new RITS(args));
	}
}
