using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

public class About : Form
{
	private IContainer components;

	private RichTextBox richTextBox1;

	private Label label1;

	private Label label2;

	private Label label3;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.About));
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.richTextBox1.Location = new System.Drawing.Point(12, 51);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.Size = new System.Drawing.Size(260, 203);
		this.richTextBox1.TabIndex = 0;
		this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
		this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(richTextBox1_LinkClicked);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(191, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Riverside-Irvine Builder: Statemachines";
		this.label2.AutoSize = true;
		this.label2.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.label2.Location = new System.Drawing.Point(12, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "Version 2.9.0";
		this.label2.Click += new System.EventHandler(label2_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(12, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(123, 13);
		this.label3.TabIndex = 3;
		this.label3.Text = "Copyright © 2008 - 2013";
		this.button1.Location = new System.Drawing.Point(197, 260);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 4;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 290);
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.richTextBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "About";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "About RIBS";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public About()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Hide();
		base.DialogResult = DialogResult.OK;
	}

	private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		Process.Start(e.LinkText);
	}

	private void label2_Click(object sender, EventArgs e)
	{
	}
}
