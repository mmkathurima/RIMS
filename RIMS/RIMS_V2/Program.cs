using System;
using System.Windows.Forms;

namespace RIMS_V2;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		Application.EnableVisualStyles();
		Application.SetHighDpiMode(HighDpiMode.SystemAware);
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new MainForm(args));
	}
}
