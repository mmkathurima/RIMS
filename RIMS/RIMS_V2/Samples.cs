using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIMS_V2;

public class Samples : Form
{
	private IContainer components;

	public ListBox sampleslist;

	private Button OKBtn;

	private List<string> samples;

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

	public Samples()
	{
		InitializeComponent();
        samples = new List<string>
        {
            "RIMS_ch2_ParkingLot_sample.c",
            "RIMS_ch3_ApplauseMeter_Mealy_sample.c",
            "RIMS_ch3_ApplauseMeter_Moore_sample.c",
            "RIMS_ch3_Latch_sample.c",
            "RIMS_ch5_DoorBell_sample.c",
            "RIMS_ch5_GlitchFilter_sample.c",
            "RIMS_ch5_Pwm_sample.c",
            "RIMS_ch5_SpeakerProject_sample.c",
            "RIMS_ch6_LedShow_sample.c",
            "RIMS_ch6_MotionLamp_sample.c",
            "RIMS_ch7_LedShowMultiPeriod_sample.c",
            "RIMS_ch9_ThreeLedsTriggered_sample.c",
            "RIMS_ch12_PID_sample.c",
            "RIMS_Frequency_Reader.c"
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

    public string ResourceName
    {
        get
        {
            if (sampleslist.SelectedIndex >= 0)
            {
                return samples[sampleslist.SelectedIndex];
            }
            return "";
        }
    }

    private void sampleslist_DoubleClick(object sender, EventArgs e)
	{
		Close();
	}
}
