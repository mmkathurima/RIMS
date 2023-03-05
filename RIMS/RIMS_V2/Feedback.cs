using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIMS_V2;

public class Feedback : Form
{
	private IContainer components;

	private Label label1;

	private Button button1;

	public Feedback()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Hide();
		base.DialogResult = DialogResult.OK;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.Feedback));
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(18, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(225, 39);
		this.label1.TabIndex = 0;
		this.label1.Text = "Please report bugs or provide feedback by\r\nsending email to: \r\nRITools@programmingembeddedsystems.com";
		this.button1.Location = new System.Drawing.Point(88, 66);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(93, 29);
		this.button1.TabIndex = 1;
		this.button1.Text = "Close";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(269, 107);
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Feedback";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		this.Text = "Feedback";
		base.TopMost = true;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
