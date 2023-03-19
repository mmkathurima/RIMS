namespace RIMS_V2;

partial class ProgrammerCalculator
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgrammerCalculator));
        this.decimalTxt = new System.Windows.Forms.TextBox();
        this.binaryTxt = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.hexadecimalTxt = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.octalTxt = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.decRightShift = new System.Windows.Forms.Button();
        this.decLeftShift = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.ansTxt = new System.Windows.Forms.TextBox();
        this.expTxt = new System.Windows.Forms.TextBox();
        this.btnEq = new System.Windows.Forms.Button();
        this.comboBox1 = new System.Windows.Forms.ComboBox();
        this.binRightShift = new System.Windows.Forms.Button();
        this.binLeftShift = new System.Windows.Forms.Button();
        this.hexRightShift = new System.Windows.Forms.Button();
        this.hexLeftShift = new System.Windows.Forms.Button();
        this.octRightShift = new System.Windows.Forms.Button();
        this.octLeftShift = new System.Windows.Forms.Button();
        this.label5 = new System.Windows.Forms.Label();
        this.groupBox1.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.SuspendLayout();
        // 
        // decimalTxt
        // 
        this.decimalTxt.Location = new System.Drawing.Point(6, 42);
        this.decimalTxt.Name = "decimalTxt";
        this.decimalTxt.Size = new System.Drawing.Size(296, 22);
        this.decimalTxt.TabIndex = 0;
        this.decimalTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.decimalTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        this.decimalTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        // 
        // binaryTxt
        // 
        this.binaryTxt.Location = new System.Drawing.Point(6, 116);
        this.binaryTxt.Name = "binaryTxt";
        this.binaryTxt.Size = new System.Drawing.Size(296, 22);
        this.binaryTxt.TabIndex = 1;
        this.binaryTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.binaryTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        this.binaryTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(6, 20);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(57, 16);
        this.label1.TabIndex = 1;
        this.label1.Text = "Decimal";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(6, 97);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(45, 16);
        this.label2.TabIndex = 1;
        this.label2.Text = "Binary";
        // 
        // hexadecimalTxt
        // 
        this.hexadecimalTxt.Location = new System.Drawing.Point(6, 193);
        this.hexadecimalTxt.Name = "hexadecimalTxt";
        this.hexadecimalTxt.Size = new System.Drawing.Size(296, 22);
        this.hexadecimalTxt.TabIndex = 2;
        this.hexadecimalTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.hexadecimalTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        this.hexadecimalTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(6, 174);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(87, 16);
        this.label3.TabIndex = 1;
        this.label3.Text = "Hexadecimal";
        // 
        // octalTxt
        // 
        this.octalTxt.Location = new System.Drawing.Point(6, 268);
        this.octalTxt.Name = "octalTxt";
        this.octalTxt.Size = new System.Drawing.Size(296, 22);
        this.octalTxt.TabIndex = 3;
        this.octalTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.octalTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        this.octalTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(6, 249);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(38, 16);
        this.label4.TabIndex = 1;
        this.label4.Text = "Octal";
        // 
        // decRightShift
        // 
        this.decRightShift.Location = new System.Drawing.Point(90, 70);
        this.decRightShift.Name = "decRightShift";
        this.decRightShift.Size = new System.Drawing.Size(75, 23);
        this.decRightShift.TabIndex = 0;
        this.decRightShift.Text = ">>";
        this.decRightShift.UseVisualStyleBackColor = true;
        this.decRightShift.Click += new System.EventHandler(this.OnClick);
        // 
        // decLeftShift
        // 
        this.decLeftShift.Location = new System.Drawing.Point(9, 71);
        this.decLeftShift.Name = "decLeftShift";
        this.decLeftShift.Size = new System.Drawing.Size(75, 23);
        this.decLeftShift.TabIndex = 4;
        this.decLeftShift.Text = "<<";
        this.decLeftShift.UseVisualStyleBackColor = true;
        this.decLeftShift.Click += new System.EventHandler(this.OnClick);
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.binaryTxt);
        this.groupBox1.Controls.Add(this.octLeftShift);
        this.groupBox1.Controls.Add(this.hexLeftShift);
        this.groupBox1.Controls.Add(this.binLeftShift);
        this.groupBox1.Controls.Add(this.octRightShift);
        this.groupBox1.Controls.Add(this.hexRightShift);
        this.groupBox1.Controls.Add(this.decLeftShift);
        this.groupBox1.Controls.Add(this.binRightShift);
        this.groupBox1.Controls.Add(this.decimalTxt);
        this.groupBox1.Controls.Add(this.decRightShift);
        this.groupBox1.Controls.Add(this.hexadecimalTxt);
        this.groupBox1.Controls.Add(this.label4);
        this.groupBox1.Controls.Add(this.octalTxt);
        this.groupBox1.Controls.Add(this.label3);
        this.groupBox1.Controls.Add(this.label1);
        this.groupBox1.Controls.Add(this.label2);
        this.groupBox1.Location = new System.Drawing.Point(12, 12);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(368, 322);
        this.groupBox1.TabIndex = 5;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Convert Bases";
        // 
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.label5);
        this.groupBox3.Controls.Add(this.comboBox1);
        this.groupBox3.Controls.Add(this.ansTxt);
        this.groupBox3.Controls.Add(this.expTxt);
        this.groupBox3.Controls.Add(this.btnEq);
        this.groupBox3.Location = new System.Drawing.Point(386, 12);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(494, 322);
        this.groupBox3.TabIndex = 7;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Evaluate Mathematical Expression";
        // 
        // ansTxt
        // 
        this.ansTxt.Location = new System.Drawing.Point(79, 78);
        this.ansTxt.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
        this.ansTxt.Multiline = true;
        this.ansTxt.Name = "ansTxt";
        this.ansTxt.ReadOnly = true;
        this.ansTxt.Size = new System.Drawing.Size(411, 50);
        this.ansTxt.TabIndex = 34;
        this.ansTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // expTxt
        // 
        this.expTxt.Location = new System.Drawing.Point(6, 24);
        this.expTxt.Multiline = true;
        this.expTxt.Name = "expTxt";
        this.expTxt.Size = new System.Drawing.Size(484, 51);
        this.expTxt.TabIndex = 33;
        this.expTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.expTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        this.expTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        // 
        // btnEq
        // 
        this.btnEq.BackColor = System.Drawing.Color.LightSalmon;
        this.btnEq.FlatAppearance.BorderColor = System.Drawing.Color.White;
        this.btnEq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnEq.Location = new System.Drawing.Point(6, 130);
        this.btnEq.Name = "btnEq";
        this.btnEq.Size = new System.Drawing.Size(64, 53);
        this.btnEq.TabIndex = 26;
        this.btnEq.Text = "=";
        this.btnEq.UseVisualStyleBackColor = false;
        this.btnEq.Click += new System.EventHandler(this.OnClick);
        // 
        // comboBox1
        // 
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Items.AddRange(new object[] {
        "DEC",
        "BIN",
        "HEX",
        "OCT"});
        this.comboBox1.Location = new System.Drawing.Point(7, 82);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new System.Drawing.Size(66, 24);
        this.comboBox1.TabIndex = 35;
        // 
        // binRightShift
        // 
        this.binRightShift.Location = new System.Drawing.Point(90, 144);
        this.binRightShift.Name = "binRightShift";
        this.binRightShift.Size = new System.Drawing.Size(75, 23);
        this.binRightShift.TabIndex = 0;
        this.binRightShift.Text = ">>";
        this.binRightShift.UseVisualStyleBackColor = true;
        this.binRightShift.Click += new System.EventHandler(this.OnClick);
        // 
        // binLeftShift
        // 
        this.binLeftShift.Location = new System.Drawing.Point(9, 144);
        this.binLeftShift.Name = "binLeftShift";
        this.binLeftShift.Size = new System.Drawing.Size(75, 23);
        this.binLeftShift.TabIndex = 4;
        this.binLeftShift.Text = "<<";
        this.binLeftShift.UseVisualStyleBackColor = true;
        this.binLeftShift.Click += new System.EventHandler(this.OnClick);
        // 
        // hexRightShift
        // 
        this.hexRightShift.Location = new System.Drawing.Point(87, 221);
        this.hexRightShift.Name = "hexRightShift";
        this.hexRightShift.Size = new System.Drawing.Size(75, 23);
        this.hexRightShift.TabIndex = 0;
        this.hexRightShift.Text = ">>";
        this.hexRightShift.UseVisualStyleBackColor = true;
        this.hexRightShift.Click += new System.EventHandler(this.OnClick);
        // 
        // hexLeftShift
        // 
        this.hexLeftShift.Location = new System.Drawing.Point(6, 221);
        this.hexLeftShift.Name = "hexLeftShift";
        this.hexLeftShift.Size = new System.Drawing.Size(75, 23);
        this.hexLeftShift.TabIndex = 4;
        this.hexLeftShift.Text = "<<";
        this.hexLeftShift.UseVisualStyleBackColor = true;
        this.hexLeftShift.Click += new System.EventHandler(this.OnClick);
        // 
        // octRightShift
        // 
        this.octRightShift.Location = new System.Drawing.Point(90, 296);
        this.octRightShift.Name = "octRightShift";
        this.octRightShift.Size = new System.Drawing.Size(75, 23);
        this.octRightShift.TabIndex = 0;
        this.octRightShift.Text = ">>";
        this.octRightShift.UseVisualStyleBackColor = true;
        this.octRightShift.Click += new System.EventHandler(this.OnClick);
        // 
        // octLeftShift
        // 
        this.octLeftShift.Location = new System.Drawing.Point(9, 296);
        this.octLeftShift.Name = "octLeftShift";
        this.octLeftShift.Size = new System.Drawing.Size(75, 23);
        this.octLeftShift.TabIndex = 4;
        this.octLeftShift.Text = "<<";
        this.octLeftShift.UseVisualStyleBackColor = true;
        this.octLeftShift.Click += new System.EventHandler(this.OnClick);
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(77, 130);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(381, 80);
        this.label5.TabIndex = 36;
        this.label5.Text = resources.GetString("label5.Text");
        // 
        // ProgrammerCalculator
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(891, 342);
        this.Controls.Add(this.groupBox3);
        this.Controls.Add(this.groupBox1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "ProgrammerCalculator";
        this.Text = "Programmer Calculator";
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox3.ResumeLayout(false);
        this.groupBox3.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox decimalTxt;
    private System.Windows.Forms.TextBox binaryTxt;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox hexadecimalTxt;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox octalTxt;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button decRightShift;
    private System.Windows.Forms.Button decLeftShift;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox3;
    internal System.Windows.Forms.TextBox ansTxt;
    internal System.Windows.Forms.TextBox expTxt;
    internal System.Windows.Forms.Button btnEq;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Button octLeftShift;
    private System.Windows.Forms.Button hexLeftShift;
    private System.Windows.Forms.Button binLeftShift;
    private System.Windows.Forms.Button octRightShift;
    private System.Windows.Forms.Button hexRightShift;
    private System.Windows.Forms.Button binRightShift;
    private System.Windows.Forms.Label label5;
}