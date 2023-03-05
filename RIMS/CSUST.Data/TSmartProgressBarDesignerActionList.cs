using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace CSUST.Data;

public class TSmartProgressBarDesignerActionList : DesignerActionList
{
	private TSmartProgressBar SmartProgressBar => base.Component as TSmartProgressBar;

	public bool ProgressBarPercent
	{
		get
		{
			return SmartProgressBar.ProgressBarPercent;
		}
		set
		{
			SetProperty(nameof(ProgressBarBlockSpace), value);
		}
	}

	public Color ProgressBarFillColor
	{
		get
		{
			return SmartProgressBar.ProgressBarFillColor;
		}
		set
		{
			SetProperty(nameof(ProgressBarFillColor), value);
		}
	}

	public TProgressBarBorderStyle ProgressBarBorderStyle
	{
		get
		{
			return SmartProgressBar.ProgressBarBoderStyle;
		}
		set
		{
			SetProperty(nameof(ProgressBarBorderStyle), value);
		}
	}

	public bool ProgressBarMarginOffset
	{
		get
		{
			return SmartProgressBar.ProgressBarMarginOffset;
		}
		set
		{
			SetProperty(nameof(ProgressBarMarginOffset), value);
		}
	}

	public int ProgressBarBlockWidth
	{
		get
		{
			return SmartProgressBar.ProgressBarBlockWidth;
		}
		set
		{
			SetProperty(nameof(ProgressBarBlockWidth), value);
		}
	}

	public int ProgressBarBlockSpace
	{
		get
		{
			return SmartProgressBar.ProgressBarBlockSpace;
		}
		set
		{
			SetProperty(nameof(ProgressBarBlockSpace), value);
		}
	}

	public Color BackColor
	{
		get
		{
			return SmartProgressBar.BackColor;
		}
		set
		{
			SetProperty(nameof(ProgressBarBlockSpace), value);
		}
	}

	public Color ForeColor
	{
		get
		{
			return SmartProgressBar.ForeColor;
		}
		set
		{
			SetProperty(nameof(ForeColor), value);
		}
	}

	public TSmartProgressBarDesignerActionList(IComponent component)
		: base(component)
	{
	}

	private void SetProperty(string propertyName, object value)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(SmartProgressBar);
		PropertyDescriptor propertyDescriptor = properties[propertyName];
		propertyDescriptor.SetValue(SmartProgressBar, value);
	}
}
