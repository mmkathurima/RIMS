using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScintillaNet;

namespace RIMS_V2;

public class ASMView : Form
{
	private IContainer components;

	private Scintilla ASMCB;

	public RetOpenedDel RetOpened;

	private static int CUR_LINE_MARKER_NUMBER = 3;

	private static Color CUR_LINE_COLOR = Color.SeaGreen;

	private static Color LIGHTER_CUR_LINE_COLOR = Color.FromArgb(255, 85, 255, 161);

	private static MarkerSymbol CUR_LINE_SYMBOL = MarkerSymbol.Arrow;

	private int lastline = -1;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.ASMCB = new ScintillaNet.Scintilla();
		((System.ComponentModel.ISupportInitialize)this.ASMCB).BeginInit();
		base.SuspendLayout();
		this.ASMCB.ConfigurationManager.Language = "asm";
		this.ASMCB.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ASMCB.Location = new System.Drawing.Point(0, 0);
		this.ASMCB.Margins.Margin0.Width = 40;
		this.ASMCB.Margins.Margin1.Width = 20;
		this.ASMCB.Margins.Margin2.Width = 15;
		this.ASMCB.Name = "ASMCB";
		this.ASMCB.Size = new System.Drawing.Size(567, 421);
		this.ASMCB.Styles.BraceBad.FontName = "Verdana";
		this.ASMCB.Styles.BraceLight.FontName = "Verdana";
		this.ASMCB.Styles.ControlChar.FontName = "Verdana";
		this.ASMCB.Styles.Default.FontName = "Verdana";
		this.ASMCB.Styles.IndentGuide.FontName = "Verdana";
		this.ASMCB.Styles.LastPredefined.FontName = "Verdana";
		this.ASMCB.Styles.LineNumber.FontName = "Verdana";
		this.ASMCB.Styles.Max.FontName = "Verdana";
		this.ASMCB.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(567, 421);
		base.Controls.Add(this.ASMCB);
		base.Name = "ASMView";
		this.Text = "ASMView";
		((System.ComponentModel.ISupportInitialize)this.ASMCB).EndInit();
		base.ResumeLayout(false);
	}

	public ASMView(string asmloc)
	{
		InitializeComponent();
		TextReader textReader = new StreamReader(asmloc);
		ASMCB.Text = textReader.ReadToEnd();
		ASMCB.UndoRedo.EmptyUndoBuffer();
		textReader.Close();
		ASMCB.IsReadOnly = true;
	}

	public void SetMarkers(int currline)
	{
		Marker marker = ASMCB.Markers[CUR_LINE_MARKER_NUMBER];
		marker.Number = CUR_LINE_MARKER_NUMBER;
		marker.Symbol = CUR_LINE_SYMBOL;
		Color color = (marker.BackColor = CUR_LINE_COLOR);
		Color foreColor = color;
		marker.ForeColor = foreColor;
		if (lastline != -1)
		{
			ASMCB.Lines[lastline].DeleteMarker(CUR_LINE_MARKER_NUMBER);
		}
		if (currline != -1)
		{
			ASMCB.Lines[currline].AddMarker(marker);
		}
		lastline = currline;
		ASMCB.Caret.Goto(ASMCB.Lines[currline].StartPosition);
		ASMCB.Caret.EnsureVisible();
	}

	public void CompileUpdate(bool cmped, string asmloc)
	{
		if (cmped)
		{
			if (lastline != -1)
			{
				ASMCB.Lines[lastline].DeleteAllMarkers();
				lastline = -1;
			}
			ASMCB.IsReadOnly = false;
			TextReader textReader = new StreamReader(asmloc);
			ASMCB.Text = textReader.ReadToEnd();
			ASMCB.UndoRedo.EmptyUndoBuffer();
			textReader.Close();
			ASMCB.IsReadOnly = true;
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);
		if (e.CloseReason != CloseReason.WindowsShutDown)
		{
			DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to close ASMView?", "Closing", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.No)
			{
				e.Cancel = true;
			}
			else
			{
				RetOpened(asmop: false);
			}
		}
	}
}
