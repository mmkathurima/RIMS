using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIMS_V2;

public class InstructionList : Form
{
	private IContainer components;

	private TextBox textBox1;

	private Button button1;

	private TableLayoutPanel tableLayoutPanel1;

	public InstructionList()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Hide();
		base.DialogResult = DialogResult.OK;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		textBox1.SelectionStart = textBox1.Text.Length;
		textBox1.DeselectAll();
	}

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.InstructionList));
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox1.Location = new System.Drawing.Point(3, 3);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.ReadOnly = true;
		this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox1.Size = new System.Drawing.Size(1198, 600);
		this.textBox1.TabIndex = 0;
		this.textBox1.TabStop = false;
		this.textBox1.Text = resources.GetString("textBox1.Text");
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(1095, 636);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(106, 22);
		this.button1.TabIndex = 1;
		this.button1.Text = "Close";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.tableLayoutPanel1.ColumnCount = 1;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.78886f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.211143f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1204, 661);
		this.tableLayoutPanel1.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		base.ClientSize = new System.Drawing.Size(1204, 661);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Name = "InstructionList";
		this.Text = "MIPS Instruction List";
		this.Icon = MainForm.ActiveForm.Icon;
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
