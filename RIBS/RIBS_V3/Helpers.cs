using System.Windows.Forms;

namespace RIBS_V3;

public class Helpers
{
	public bool PointInDrawingArea(Panel p, int x, int y)
	{
		bool result = false;
		if (x < p.Left + p.Width + 55 && x > p.Left + 55 && y > p.Top + p.Height + 55 && y < p.Bottom + 55)
		{
			result = true;
		}
		return result;
	}
}
