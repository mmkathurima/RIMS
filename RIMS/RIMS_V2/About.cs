using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RIMS_V2;

public class About : Form
{
    private IContainer components;

    private RichTextBox richTextBox1;

    private Label label1;

    private Label label2;

    private Label label3;

    private Button button1;

    private Label label4;

    private Label label5;

    public About()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.OK;
    }

    private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = e.LinkText
        });
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.About));
        this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        base.SuspendLayout();
        this.richTextBox1.Location = new System.Drawing.Point(15, 86);
        this.richTextBox1.Name = "richTextBox1";
        this.richTextBox1.Size = new System.Drawing.Size(260, 189);
        this.richTextBox1.TabIndex = 0;
        this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
        this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(richTextBox1_LinkClicked);
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 9);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(234, 13);
        this.label1.TabIndex = 1;
        this.label1.Text = "Riverside-Irvine Microcontroller Simulator (RIMS)";
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(12, 35);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(69, 13);
        this.label2.TabIndex = 2;
        this.label2.Text = "Version 2.9";
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(12, 22);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(123, 13);
        this.label3.TabIndex = 3;
        this.label3.Text = "Copyright © 2008 - 2013";
        this.button1.Location = new System.Drawing.Point(200, 281);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 4;
        this.button1.Text = "OK";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(button1_Click);
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(15, 57);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(229, 13);
        this.label4.TabIndex = 5;
        this.label4.Text = "Scintilla Copyright © 1998 - 2006 Neil Hodgson";
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(15, 70);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(256, 13);
        this.label5.TabIndex = 6;
        this.label5.Text = "ScintillaNET Copyright © 2002 - 2006 Garrett Serack";
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(300, 330);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.MaximizeBox = false;
        base.Controls.Add(this.label5);
        base.Controls.Add(this.label4);
        base.Controls.Add(this.button1);
        base.Controls.Add(this.label3);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.richTextBox1);
        base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
        base.Name = "About";
        base.ShowIcon = false;
        base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.Text = "About RIMS";
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}
