using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

public class Samples : Form
{
	private List<string> samples;

	private IContainer components;

	public ListBox sampleslist;

	private Button OKBtn;

	public Samples()
	{
		InitializeComponent();
        samples = new List<string>
        {
            "RIBS_ch3_ApplauseMeter_Mealy_sample.sm",
            "RIBS_ch3_ApplauseMeter_Moore_sample.sm",
            "RIBS_ch3_Latch_sample.sm",
            "RIBS_ch5_DoorBell_sample.sm",
            "RIBS_ch5_GlitchFilter_sample.sm",
            "RIBS_ch5_Pwm_sample.sm",
            "RIBS_ch5_SpeakerProject_sample.sm",
            "RIBS_ch6_LedShow_sample.sm",
            "RIBS_ch6_MotionLamp_sample.sm",
            "RIBS_ch7_LedShowMultiPeriod_sample.sm",
            "RIBS_ch12_PID_sample.sm",
            "RIBS_Frequency_Reader.sm"
        };
        foreach (string sample in samples)
		{
			sampleslist.Items.Add(sample);
		}
	}

	private void OKBtn_Click(object sender, EventArgs e)
	{
		Close();
	}

	public string GetResourceName()
	{
		if (sampleslist.SelectedIndex > -1)
		{
			return samples[sampleslist.SelectedIndex];
		}
		return "";
	}

	private void sampleslist_DoubleClick(object sender, EventArgs e)
	{
		OKBtn_Click(sender, e);
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
		this.sampleslist = new System.Windows.Forms.ListBox();
		this.OKBtn = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.sampleslist.FormattingEnabled = true;
		this.sampleslist.Location = new System.Drawing.Point(13, 16);
		this.sampleslist.Name = "sampleslist";
		this.sampleslist.Size = new System.Drawing.Size(233, 186);
		this.sampleslist.TabIndex = 0;
		this.sampleslist.DoubleClick += new System.EventHandler(sampleslist_DoubleClick);
		this.OKBtn.Location = new System.Drawing.Point(183, 208);
		this.OKBtn.Name = "OKBtn";
		this.OKBtn.Size = new System.Drawing.Size(64, 23);
		this.OKBtn.TabIndex = 2;
		this.OKBtn.Text = "OK";
		this.OKBtn.UseVisualStyleBackColor = true;
		this.OKBtn.Click += new System.EventHandler(OKBtn_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(259, 238);
		base.Controls.Add(this.OKBtn);
		base.Controls.Add(this.sampleslist);
		base.Name = "Samples";
		this.Text = "Samples";
		base.ResumeLayout(false);
	}
}
