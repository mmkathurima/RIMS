using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RITS;

internal class popup : Form
{
	private IContainer components;

	private Label label1;

	private RichTextBox richTextBox1;

	private Button ok;

	public string AssemblyTitle
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), inherit: false);
			if (customAttributes.Length != 0)
			{
				AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
				if (assemblyTitleAttribute.Title != "")
				{
					return assemblyTitleAttribute.Title;
				}
			}
			return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
		}
	}

	public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

	public string AssemblyDescription
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
		}
	}

	public string AssemblyProduct
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyProductAttribute)customAttributes[0]).Product;
		}
	}

	public string AssemblyCopyright
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
		}
	}

	public string AssemblyCompany
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(popup));
		this.label1 = new System.Windows.Forms.Label();
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.ok = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(14, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(231, 39);
		this.label1.TabIndex = 0;
		this.label1.Text = "Riverside-Irvine Timing-Diagram Solution (RITS)\r\nCopyright © 2008 - 2013 UC Regents\r\nVersion 2.8";
		this.richTextBox1.Location = new System.Drawing.Point(12, 64);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.Size = new System.Drawing.Size(241, 182);
		this.richTextBox1.TabIndex = 1;
		this.richTextBox1.Text = "This software comes with NO WARRANTY, to the extent allowed by law.\n\nSend questions, comments, or bugs to RITools@programmingembeddedsystems.com\n\nwww.programmingembeddedsystems.com";
		this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(richTextBox1_LinkClicked);
		this.ok.Location = new System.Drawing.Point(188, 252);
		this.ok.Name = "ok";
		this.ok.Size = new System.Drawing.Size(65, 26);
		this.ok.TabIndex = 2;
		this.ok.Text = "OK";
		this.ok.UseVisualStyleBackColor = true;
		this.ok.Click += new System.EventHandler(ok_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(265, 286);
		base.Controls.Add(this.ok);
		base.Controls.Add(this.richTextBox1);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(271, 314);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(271, 314);
		base.Name = "popup";
		base.Padding = new System.Windows.Forms.Padding(9);
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "About RITS";
		base.Load += new System.EventHandler(popup_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public popup()
	{
		InitializeComponent();
	}

	private void popup_Load(object sender, EventArgs e)
	{
	}

	private void ok_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		Process.Start(e.LinkText);
	}
}
