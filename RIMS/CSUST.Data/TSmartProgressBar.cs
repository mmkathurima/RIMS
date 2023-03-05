using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace CSUST.Data;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(ProgressBar))]
[Designer(typeof(TSmartProgressBarDesigner), typeof(IDesigner))]
public class TSmartProgressBar : Label
{
	private const int m_MaxBarWidth = 20;

	private const int m_MaxBarSpace = 10;

	private int m_Value;

	private int m_Maximum = 100;

	private int m_ProgressBarBlockWidth = 6;

	private int m_ProgressBarBlockSpace = 1;

	public bool m_ProgressBarPercent = true;

	private bool m_ProgressBarMarginOffset = true;

	private TProgressBarBorderStyle m_ProgressBarBorderStyle;

	private SolidBrush m_ProgressBarFillBrush;

	[DefaultValue(typeof(Color), "Coral")]
	[Category("Custom")]
	[Description("Set/Get progress bar fill color.")]
	public Color ProgressBarFillColor
	{
		get
		{
			return m_ProgressBarFillBrush.Color;
		}
		set
		{
			if (m_ProgressBarFillBrush.Color != value)
			{
				m_ProgressBarFillBrush.Color = value;
				Invalidate();
			}
		}
	}

	[Category("Custom")]
	[DefaultValue(6)]
	[Description("Set/Get progress small bar width.")]
	public int ProgressBarBlockWidth
	{
		get
		{
			return m_ProgressBarBlockWidth;
		}
		set
		{
			if (m_ProgressBarBlockWidth != value)
			{
				if (value < 1)
				{
					m_ProgressBarBlockWidth = 1;
				}
				else if (value > 20)
				{
					m_ProgressBarBlockWidth = 20;
				}
				else
				{
					m_ProgressBarBlockWidth = value;
				}
				Invalidate();
			}
		}
	}

	[Category("Custom")]
	[DefaultValue(1)]
	[Description("Set/Get progress bar space width(smooth when 0).")]
	public int ProgressBarBlockSpace
	{
		get
		{
			return m_ProgressBarBlockSpace;
		}
		set
		{
			if (m_ProgressBarBlockSpace != value)
			{
				if (value < 0)
				{
					m_ProgressBarBlockSpace = 0;
				}
				else if (value > 10)
				{
					m_ProgressBarBlockSpace = 10;
				}
				else
				{
					m_ProgressBarBlockSpace = value;
				}
				Invalidate();
			}
		}
	}

	[DefaultValue(typeof(TProgressBarBorderStyle), "Flat")]
	[Category("Custom")]
	[Description("Set/Get progress bar boder style.")]
	public TProgressBarBorderStyle ProgressBarBoderStyle
	{
		get
		{
			return m_ProgressBarBorderStyle;
		}
		set
		{
			if (m_ProgressBarBorderStyle != value)
			{
				m_ProgressBarBorderStyle = value;
				Invalidate();
			}
		}
	}

	[Description("Set/Get show percent text or not.")]
	[DefaultValue(true)]
	[Category("Custom")]
	public bool ProgressBarPercent
	{
		get
		{
			return m_ProgressBarPercent;
		}
		set
		{
			if (m_ProgressBarPercent != value)
			{
				m_ProgressBarPercent = value;
				Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Set/Get if progress bar has margin offset.")]
	[Category("Custom")]
	public bool ProgressBarMarginOffset
	{
		get
		{
			return m_ProgressBarMarginOffset;
		}
		set
		{
			if (m_ProgressBarMarginOffset != value)
			{
				m_ProgressBarMarginOffset = value;
				Invalidate();
			}
		}
	}

	[DefaultValue(typeof(Color), "White")]
	[Category("Custom")]
	[Description("Set/Get progress bar background color.")]
	public new Color BackColor
	{
		get
		{
			return base.BackColor;
		}
		set
		{
			if (base.BackColor != value)
			{
				base.BackColor = value;
				Invalidate();
			}
		}
	}

	[DefaultValue(typeof(Color), "Blue")]
	[Description("Set/Get progress bar text color.")]
	[Category("Custom")]
	public new Color ForeColor
	{
		get
		{
			return base.ForeColor;
		}
		set
		{
			if (base.ForeColor != value)
			{
				base.ForeColor = value;
				Invalidate();
			}
		}
	}

	[DefaultValue(100)]
	[Description("Set/Get progress bar maximum value.")]
	[Category("Custom")]
	public int Maximum
	{
		get
		{
			return m_Maximum;
		}
		set
		{
			if (m_Maximum != value)
			{
				if (value < 1)
				{
					m_Maximum = 1;
				}
				else
				{
					m_Maximum = value;
				}
				if (m_Maximum < m_Value)
				{
					m_Value = m_Maximum;
				}
				Invalidate();
			}
		}
	}

	[Description("Set/Get progress bar current value.")]
	[Category("Custom")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			else if (value > m_Maximum)
			{
				m_Value = m_Maximum;
			}
			else
			{
				m_Value = value;
			}
			Invalidate();
			Update();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool AutoSize => base.AutoSize;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new bool AutoEllipsis => base.AutoEllipsis;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new ContentAlignment TextAlign => base.TextAlign;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool CausesValidation => base.CausesValidation;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool AllowDrop => base.AllowDrop;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new Padding Padding => base.Padding;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new ImeMode ImeMode => base.ImeMode;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool TabStop => base.TabStop;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool UseCompatibleTextRendering => base.UseCompatibleTextRendering;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new bool UseMnemonic => base.UseMnemonic;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new BorderStyle BorderStyle => base.BorderStyle;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new FlatStyle FlatStyle => base.FlatStyle;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new Image Image => base.Image;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new ContentAlignment ImageAlign => base.ImageAlign;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new int ImageIndex => base.ImageIndex;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new string ImageKey => base.ImageKey;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public new ImageList ImageList => base.ImageList;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new string Text => base.Text;

	public TSmartProgressBar()
	{
		m_ProgressBarFillBrush = new SolidBrush(Color.Coral);
		base.BackColor = Color.White;
		base.ForeColor = Color.Blue;
		base.AutoSize = false;
		base.TextAlign = ContentAlignment.MiddleCenter;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			m_ProgressBarFillBrush.Dispose();
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		DrawPrgressBarBorder(e.Graphics);
		DrawProgressBar(e.Graphics);
		if (m_ProgressBarPercent)
		{
			base.Text = ((double)m_Value / (double)m_Maximum).ToString("##0 %");
		}
		else
		{
			base.Text = string.Empty;
		}
		base.OnPaint(e);
	}

	private int GetTopOffSet()
	{
		if (!m_ProgressBarMarginOffset)
		{
			if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Sunken || m_ProgressBarBorderStyle == TProgressBarBorderStyle.Flat)
			{
				return 2;
			}
			if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.None)
			{
				return 0;
			}
			return 1;
		}
		if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Flat || m_ProgressBarBorderStyle == TProgressBarBorderStyle.Sunken)
		{
			return 3;
		}
		if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.None)
		{
			return 1;
		}
		return 2;
	}

	private int GetLeftOffSet()
	{
		if (!m_ProgressBarMarginOffset)
		{
			if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Flat)
			{
				return 2;
			}
			if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.None)
			{
				return 0;
			}
			return 1;
		}
		if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Flat || m_ProgressBarBorderStyle == TProgressBarBorderStyle.Sunken)
		{
			return 3;
		}
		if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.None)
		{
			return 1;
		}
		return 2;
	}

	private void DrawProgressBar(Graphics g)
	{
		decimal num = (decimal)m_Value / (decimal)m_Maximum;
		int num2 = (int)((decimal)(base.ClientRectangle.Width - GetLeftOffSet() * 2) * num);
		int num3 = m_ProgressBarBlockWidth + m_ProgressBarBlockSpace;
		int num4 = num2 / num3 * num3;
		if (num > 0.99m && base.ClientRectangle.Width - GetLeftOffSet() * 2 - num4 > 0)
		{
			num4 += (base.ClientRectangle.Width - GetLeftOffSet() * 2 - num4) / num3;
		}
		int num5 = base.ClientRectangle.Left + GetLeftOffSet();
		int num6 = base.ClientRectangle.Top + GetTopOffSet();
		int num7 = base.ClientRectangle.Height - GetTopOffSet() * 2;
		for (int j = num3; j <= num4; j += num3)
		{
			g.FillRectangle(m_ProgressBarFillBrush, num5, num6, m_ProgressBarBlockWidth, num7);
			num5 += num3;
		}
		int num8 = base.ClientRectangle.Width - num5 - GetLeftOffSet();
		if (num8 > 0 && num8 < num3)
		{
			int i = base.ClientRectangle.Width - num5 - GetLeftOffSet();
			if (i > 0)
			{
				g.FillRectangle(m_ProgressBarFillBrush, num5, num6, i, num7);
			}
		}
	}

	private void DrawPrgressBarBorder(Graphics g)
	{
		if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Single)
		{
			ControlPaint.DrawBorder(g, base.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
		}
		else if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Flat)
		{
			ControlPaint.DrawBorder3D(g, base.ClientRectangle, Border3DStyle.Flat);
		}
		else if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.Sunken)
		{
			ControlPaint.DrawBorder3D(g, base.ClientRectangle, Border3DStyle.Sunken);
		}
		else if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.SunkenInner)
		{
			ControlPaint.DrawBorder3D(g, base.ClientRectangle, Border3DStyle.SunkenInner);
		}
		else if (m_ProgressBarBorderStyle == TProgressBarBorderStyle.SunkenOut)
		{
			ControlPaint.DrawBorder3D(g, base.ClientRectangle, Border3DStyle.SunkenOuter);
		}
	}
}
