using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RIMS_V2;

public class KeyValidation : Form
{
    private bool valid;

    private IContainer components;

    private TextBox textBox2;

    private Button button2;

    private TextBox textBox3;

    private Label label1;

    private Button button1;

    private Button button3;

    private RichTextBox textBox1;

    public KeyValidation()
    {
        InitializeComponent();
        valid = false;
        textBox2.Text = LoadLicenseKey();
        button2_Click(this, new EventArgs());
        base.DialogResult = DialogResult.No;
        this.Size = this.MaximumSize = this.MinimumSize = new Size(this.Width, this.Height + 30);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        string cdkey = textBox2.Text;
        uint num = 0u;
        try
        {
            num = Validation.validate(cdkey);
        }
        catch (Exception ex)
        {
            textBox3.Text = ex.Message;
            valid = false;
            button1.Enabled = false;
            return;
        }
        switch (num)
        {
            case 0u:
                if (textBox2.Text == "<none>")
                {
                    textBox3.Text = "No valid license key file found.\r\n(Key is in \"My Documents\\RIMSlicense.lic\").\r\nPlease do not remove this file.";
                }
                else
                {
                    textBox3.Text = "Invalid license key.";
                }
                valid = false;
                button1.Enabled = false;
                return;
            case uint.MaxValue:
                textBox3.Text = "Valid License Key -- Expires: NEVER";
                if (!SaveLicenseKey(cdkey))
                {
                    MessageBox.Show("Couldn't save your license file.  Please restart the program as an administrator.  If the error persists, please send a bug report to RIToolsFeedback@gmail.com.", "Couldn't Save License", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                }
                valid = true;
                button1.Enabled = true;
                return;
        }
        double value = Convert.ToDouble(num);
        DateTime value2 = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(value).ToLocalTime();
        int num2 = DateTime.Now.CompareTo(value2);
        if (num2 >= 0)
        {
            textBox3.Text = "The license key expired on " + value2.ToShortDateString() + " at " + value2.ToShortTimeString() + ".\r\nContact info@programmingembeddedsystems.com regarding purchasing a new time-limited license.";
            valid = false;
            button1.Enabled = false;
            return;
        }
        textBox3.Text = "Valid license key.\r\nExpires on " + value2.ToShortDateString() + " at " + value2.ToShortTimeString() + ".";
        if (!SaveLicenseKey(cdkey))
        {
            MessageBox.Show("Couldn't save your license file.  Please restart the program as an administrator.  If the error persists, please send a bug report to RIToolsFeedback@gmail.com.", "Couldn't Save License", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Application.Exit();
        }
        valid = true;
        button1.Enabled = true;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (valid)
        {
            base.DialogResult = DialogResult.Yes;
            Close();
        }
    }

    private bool SaveLicenseKey(string cdkey)
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        folderPath += "\\RIMSlicense.lic";
        try
        {
            FileStream fileStream = File.Open(folderPath, FileMode.Create);
            byte[] array = new byte[cdkey.Length + 1];
            array = Encoding.ASCII.GetBytes(cdkey);
            fileStream.Write(array, 0, cdkey.Length);
            fileStream.Close();
        }
        catch (IOException)
        {
            return false;
        }
        return true;
    }

    private string LoadLicenseKey()
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        folderPath += "\\RIMSlicense.lic";
        try
        {
            StreamReader streamReader = new StreamReader(folderPath);
            string result = streamReader.ReadLine();
            streamReader.Close();
            return result;
        }
        catch (IOException)
        {
            return "<none>";
        }
    }

    private void button3_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.No;
        Close();
    }

    private void textBox1_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        Process.Start(e.LinkText);
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
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.button2 = new System.Windows.Forms.Button();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.textBox1 = new System.Windows.Forms.RichTextBox();
        base.SuspendLayout();
        this.textBox2.Location = new System.Drawing.Point(12, 41);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(268, 20);
        this.textBox2.TabIndex = 2;
        this.button2.Location = new System.Drawing.Point(286, 38);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 3;
        this.button2.Text = "Validate key";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(button2_Click);
        this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.textBox3.Location = new System.Drawing.Point(12, 67);
        this.textBox3.Multiline = true;
        this.textBox3.Name = "textBox3";
        this.textBox3.ReadOnly = true;
        this.textBox3.Size = new System.Drawing.Size(345, 43);
        this.textBox3.TabIndex = 4;
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(13, 13);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(288, 13);
        this.label1.TabIndex = 5;
        this.label1.Text = "Press 'Validate key' to validate and save a new license key.";
        this.button1.Enabled = false;
        this.button1.Location = new System.Drawing.Point(367, 148);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 6;
        this.button1.Text = "Continue";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(button1_Click);
        this.button3.Location = new System.Drawing.Point(367, 38);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 7;
        this.button3.Text = "Cancel";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(button3_Click);
        this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.textBox1.Location = new System.Drawing.Point(12, 126);
        this.textBox1.Name = "textBox1";
        this.textBox1.ReadOnly = true;
        this.textBox1.Size = new System.Drawing.Size(349, 45);
        this.textBox1.TabIndex = 8;
        this.textBox1.Text = "Your license key is included with your purchase of Programming Embedded Systems.  For more information on how to purchase this book, please visit www.programmingembeddedsystems.com";
        this.textBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(textBox1_LinkClicked);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(446, 170);
        base.ControlBox = false;
        base.Controls.Add(this.textBox1);
        base.Controls.Add(this.button3);
        base.Controls.Add(this.button1);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.textBox3);
        base.Controls.Add(this.button2);
        base.Controls.Add(this.textBox2);
        base.MaximizeBox = false;
        this.MaximumSize = new System.Drawing.Size(462, 209);
        base.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(462, 189);
        base.Name = "KeyValidation";
        base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.Text = "Update License Key";
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}
