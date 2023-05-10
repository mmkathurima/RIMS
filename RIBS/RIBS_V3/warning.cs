using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

public class Warning : Form
{
	private IContainer components;

	private RichTextBox richTextBox1;

	private Button button1;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.Warning));
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.richTextBox1.Location = new System.Drawing.Point(12, 12);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.Size = new System.Drawing.Size(260, 55);
		this.richTextBox1.TabIndex = 1;
		this.richTextBox1.Text = "WARNING! Local variables and functions for this state machine MUST be declared static! Make your local code to the Global Box or declare it STATIC!";
		this.button1.Location = new System.Drawing.Point(197, 73);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 5;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 105);
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.richTextBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "Warning";
		this.Text = "Warning";
		base.ResumeLayout(false);
	}

	public Warning()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Hide();
		base.DialogResult = DialogResult.OK;
	}
}
