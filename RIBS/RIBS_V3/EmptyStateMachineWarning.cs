using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

public class EmptyStateMachineWarning : Form
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.EmptyStateMachineWarning));
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.richTextBox1.Location = new System.Drawing.Point(12, 12);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.Size = new System.Drawing.Size(260, 55);
		this.richTextBox1.TabIndex = 2;
		this.richTextBox1.Text = "Your Project contains at least one empty state machine. You must either delete or edit this machine!";
		this.button1.Location = new System.Drawing.Point(197, 78);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 6;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 107);
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.richTextBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "EmptyStateMachineWarning";
		this.Text = "Error - Empty State Machine";
		base.ResumeLayout(false);
	}

	public EmptyStateMachineWarning()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Hide();
		base.DialogResult = DialogResult.OK;
	}
}
