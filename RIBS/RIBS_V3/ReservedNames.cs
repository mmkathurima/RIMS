using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

public class ReservedNames : Form
{
	private IContainer components;

	private GroupBox ReservedVariablesGroupBox;

	private Label IOText;

	private ListBox listBox1;

	private Button button1;

	public ReservedNames()
	{
		InitializeComponent();
	}

	private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (listBox1.SelectedIndex)
		{
		case 0:
			IOText.Text = "Input (8 bits):\r\n\r\nA is an 8 bit input to the state\r\nmachine.  Single bits can\r\nbe referenced by using\r\nA7,A6,...";
			break;
		case 1:
			IOText.Text = "Output (8 bits):\r\n\r\nB is an 8 bit output from the\r\nstate machine. Single bits can\r\nbe referenced by using\r\nB7,B6,...";
			break;
		case 2:
			IOText.Text = "Register (8 bits):\r\n\r\nT is the transmit data register\r\nfor the UART serial peripheral.";
			break;
		case 3:
			IOText.Text = "Register (8 bits):\r\n\r\nR is the received data register\r\nfor the UART serial peripheral.";
			break;
		case 4:
			IOText.Text = "Register (1 bit): \r\n\r\nRxComplete is set to 1 when\r\na byte has been received from\r\nthe UART.It is cleared\r\nautomatically when R is read.";
			break;
		case 5:
			IOText.Text = "Register (1 bit): \r\n\r\nTxReady is a flag set when\r\nthe UART is ready to transmit.\r\nIt is cleared automatically\r\nwhen T is loaded with data.";
			break;
		case 6:
			IOText.Text = "Transition Condition: \r\n\r\n'other' is a special\r\ncondition used to signify that the\r\ntransition should be taken\r\nwhen no other transition of the\r\nstate is true.";
			break;
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.ReservedNames));
		this.ReservedVariablesGroupBox = new System.Windows.Forms.GroupBox();
		this.IOText = new System.Windows.Forms.Label();
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.button1 = new System.Windows.Forms.Button();
		this.ReservedVariablesGroupBox.SuspendLayout();
		base.SuspendLayout();
		this.ReservedVariablesGroupBox.Controls.Add(this.IOText);
		this.ReservedVariablesGroupBox.Controls.Add(this.listBox1);
		this.ReservedVariablesGroupBox.Location = new System.Drawing.Point(12, 12);
		this.ReservedVariablesGroupBox.Name = "ReservedVariablesGroupBox";
		this.ReservedVariablesGroupBox.Size = new System.Drawing.Size(242, 126);
		this.ReservedVariablesGroupBox.TabIndex = 40;
		this.ReservedVariablesGroupBox.TabStop = false;
		this.ReservedVariablesGroupBox.Text = "Reserved variables";
		this.IOText.AutoSize = true;
		this.IOText.Location = new System.Drawing.Point(83, 19);
		this.IOText.Name = "IOText";
		this.IOText.Size = new System.Drawing.Size(150, 65);
		this.IOText.TabIndex = 1;
		this.IOText.Text = "Input (8 bits):\r\n\r\nA is an 8 bit input to the state\r\nmachine.  Single bits can be\r\nreferenced by using A7, A6, ...";
		this.listBox1.FormattingEnabled = true;
		this.listBox1.Items.AddRange(new object[7] { "A", "B", "T", "R", "RxComplete", "TxReady", "other" });
		this.listBox1.Location = new System.Drawing.Point(9, 19);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(68, 95);
		this.listBox1.TabIndex = 0;
		this.listBox1.TabStop = false;
		this.listBox1.SelectedIndexChanged += new System.EventHandler(listBox1_SelectedIndexChanged);
		this.button1.Location = new System.Drawing.Point(179, 144);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 41;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(266, 172);
		base.FormBorderStyle = FormBorderStyle.FixedDialog;
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.ReservedVariablesGroupBox);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "ReservedNames";
		this.Text = "Reserved Variable Names";
		this.ReservedVariablesGroupBox.ResumeLayout(false);
		this.ReservedVariablesGroupBox.PerformLayout();
		base.ResumeLayout(false);
	}
}
