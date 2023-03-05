using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

internal class TransitionForLoopForm : Form
{
	public Node node;

	private string initial;

	private string condition;

	private string update;

	private char[] parsable = new char[7] { '*', '<', '>', '=', '+', '-', '/' };

	private string convar;

	private string upvar;

	private string initvar;

	private bool closeable;

	private IContainer components;

	private Label label1;

	private Label label2;

	private Label label3;

	private Panel panel1;

	private Label label4;

	private TextBox textBox1;

	private TextBox textBox2;

	private TextBox textBox3;

	private TextBox textBox4;

	public TransitionForLoopForm()
	{
		InitializeComponent();
	}

	public void Bind(Node n)
	{
		node = n;
		if (node.loop.initial == "" && node.loop.condition == "" && node.loop.update == "")
		{
			initial = textBox1.Text;
			condition = textBox2.Text;
			update = textBox3.Text;
		}
		else
		{
			initial = node.loop.initial;
			condition = node.loop.condition;
			update = node.loop.update;
		}
		textBox1.Text = initial;
		textBox2.Text = condition;
		textBox3.Text = update;
		UpdateResult();
	}

	private void updateVars()
	{
		string[] array = initial.Split('=');
		initvar = array[0];
		node.loop.loopvar = array[0];
		string[] array2 = condition.Split(parsable);
		convar = array2[0];
		string[] array3 = update.Split(parsable);
		upvar = array3[0];
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		initial = textBox1.Text;
		node.loop.initial = initial;
		updateVars();
		UpdateResult();
	}

	private void textBox2_TextChanged(object sender, EventArgs e)
	{
		condition = textBox2.Text;
		node.loop.condition = condition;
		updateVars();
		UpdateResult();
	}

	private void textBox3_TextChanged(object sender, EventArgs e)
	{
		update = textBox3.Text;
		node.loop.update = update;
		updateVars();
		UpdateResult();
	}

	private void UpdateResult()
	{
		string text = "for (" + initial + "; " + condition + "; " + update + ")";
		textBox4.Text = text;
	}

	private bool CheckVar()
	{
		if (initial[^1..] == ";" || condition[^1..] == ";" || update[^1..] == ";")
		{
			MessageBox.Show("Unnecessary ';'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		if (!(convar == initvar) || !(upvar == initvar))
		{
			MessageBox.Show("Mismatched variables", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		node.loop.loopvar = initvar;
		return true;
	}

	private void TransitionForLoopForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		node.loop.initial = initial;
		node.loop.condition = condition;
		node.loop.update = update;
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);
		if (e.CloseReason != CloseReason.WindowsShutDown && !CheckVar())
		{
			e.Cancel = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.TransitionForLoopForm));
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(9, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(113, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Initialization statement:";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(9, 54);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(54, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "Condition:";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(9, 93);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(98, 13);
		this.label3.TabIndex = 2;
		this.label3.Text = "Update expression:";
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.panel1.Controls.Add(this.textBox4);
		this.panel1.Location = new System.Drawing.Point(12, 149);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(225, 21);
		this.panel1.TabIndex = 3;
		this.textBox4.BackColor = System.Drawing.SystemColors.Control;
		this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox4.Location = new System.Drawing.Point(3, 3);
		this.textBox4.Name = "textBox4";
		this.textBox4.Size = new System.Drawing.Size(215, 13);
		this.textBox4.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(11, 134);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(37, 13);
		this.label4.TabIndex = 4;
		this.label4.Text = "Result";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Location = new System.Drawing.Point(126, 15);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(107, 20);
		this.textBox1.TabIndex = 5;
		this.textBox1.Text = "i=0";
		this.textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox2.Location = new System.Drawing.Point(126, 54);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(107, 20);
		this.textBox2.TabIndex = 6;
		this.textBox2.Text = "i<10";
		this.textBox2.TextChanged += new System.EventHandler(textBox2_TextChanged);
		this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox3.Location = new System.Drawing.Point(126, 93);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(107, 20);
		this.textBox3.TabIndex = 7;
		this.textBox3.Text = "i++";
		this.textBox3.TextChanged += new System.EventHandler(textBox3_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(245, 181);
		base.Controls.Add(this.textBox3);
		base.Controls.Add(this.textBox2);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "TransitionForLoopForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		this.Text = "Transitionforloop";
		base.TopMost = true;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(TransitionForLoopForm_FormClosing);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
