using System.Windows.Forms;

namespace RIBS_V3;

public class SuperTabControlEventArgs : TabControlEventArgs
{
	public int closed_index;

	public SuperTabControlEventArgs(TabPage page, int index, TabControlAction action)
		: base(page, index, action)
	{
	}
}
