using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using RITS.Properties;

namespace RITS;

public class RITS : Form
{
	private IContainer components;

	private Button open;

	private Button print;

	private Chart chart1;

	private Button hor_zoomin;

	private Button hor_zoomout;

	private MenuStrip menuStrip2;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem openToolStripMenuItem;

	private ToolStripMenuItem saveAsJpegToolStripMenuItem;

	private ToolStripMenuItem helpToolStripMenuItem1;

	private ToolStripMenuItem aboutToolStripMenuItem1;

	private ToolStripMenuItem aboutToolStripMenuItem2;

	private ToolStripMenuItem helpOnlineToolStripMenuItem;

	private ToolStripMenuItem licenseKeyToolStripMenuItem;

	private string ofFileName;

	private int highest = 5;

	private List<List<Timestamp>> time_table = new List<List<Timestamp>>();

	private double min;

	private double max;

	private double start;

	private double end;

	private bool launched_mode;

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
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(global::RITS.RITS));
		this.open = new System.Windows.Forms.Button();
		this.print = new System.Windows.Forms.Button();
		this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.menuStrip2 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveAsJpegToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.helpOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.licenseKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.hor_zoomin = new System.Windows.Forms.Button();
		this.hor_zoomout = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.chart1).BeginInit();
		this.menuStrip2.SuspendLayout();
		base.SuspendLayout();
		this.open.Location = new System.Drawing.Point(0, 27);
		this.open.Name = "open";
		this.open.Size = new System.Drawing.Size(62, 45);
		this.open.TabIndex = 2;
		this.open.Text = "Open";
		this.open.UseVisualStyleBackColor = true;
		this.open.Click += new System.EventHandler(open_Click);
		this.print.Location = new System.Drawing.Point(68, 27);
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(62, 45);
		this.print.TabIndex = 12;
		this.print.Text = "Save JPEG";
		this.print.UseVisualStyleBackColor = true;
		this.print.Click += new System.EventHandler(print_Click);
		chartArea.Name = "ChartArea1";
		this.chart1.ChartAreas.Add(chartArea);
		legend.Name = "Legend1";
		this.chart1.Legends.Add(legend);
		this.chart1.Location = new System.Drawing.Point(0, 78);
		this.chart1.Name = "chart1";
		series.ChartArea = "ChartArea1";
		series.Legend = "Legend1";
		series.Name = "Series1";
		this.chart1.Series.Add(series);
		this.chart1.Size = new System.Drawing.Size(1008, 437);
		this.chart1.TabIndex = 13;
		this.chart1.Text = "chart1";
		this.chart1.Click += new System.EventHandler(chart1_Click);
		this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.fileToolStripMenuItem, this.helpToolStripMenuItem1 });
		this.menuStrip2.Location = new System.Drawing.Point(0, 0);
		this.menuStrip2.Name = "menuStrip2";
		this.menuStrip2.Size = new System.Drawing.Size(1008, 24);
		this.menuStrip2.TabIndex = 19;
		this.menuStrip2.Text = "menuStrip2";
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.openToolStripMenuItem, this.saveAsJpegToolStripMenuItem });
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.fileToolStripMenuItem.Text = "File";
		this.openToolStripMenuItem.Name = "openToolStripMenuItem";
		this.openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
		this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
		this.openToolStripMenuItem.Text = "Open";
		this.openToolStripMenuItem.Click += new System.EventHandler(openToolStripMenuItem_Click);
		this.saveAsJpegToolStripMenuItem.Name = "saveAsJpegToolStripMenuItem";
		this.saveAsJpegToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
		this.saveAsJpegToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
		this.saveAsJpegToolStripMenuItem.Text = "Save As Jpeg";
		this.saveAsJpegToolStripMenuItem.Click += new System.EventHandler(saveAsJpegToolStripMenuItem_Click);
		this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.helpOnlineToolStripMenuItem, this.aboutToolStripMenuItem2, this.licenseKeyToolStripMenuItem, this.aboutToolStripMenuItem1 });
		this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
		this.helpToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
		this.helpToolStripMenuItem1.Text = "Help";
		this.helpOnlineToolStripMenuItem.Name = "helpOnlineToolStripMenuItem";
		this.helpOnlineToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
		this.helpOnlineToolStripMenuItem.Text = "Help Online";
		this.helpOnlineToolStripMenuItem.Click += new System.EventHandler(helpOnlineToolStripMenuItem_Click);
		this.aboutToolStripMenuItem2.Name = "aboutToolStripMenuItem2";
		this.aboutToolStripMenuItem2.Size = new System.Drawing.Size(193, 22);
		this.aboutToolStripMenuItem2.Text = "Report Bugs/Feedback";
		this.aboutToolStripMenuItem2.Click += new System.EventHandler(aboutToolStripMenuItem2_Click);
		this.licenseKeyToolStripMenuItem.Name = "licenseKeyToolStripMenuItem";
		this.licenseKeyToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
		this.licenseKeyToolStripMenuItem.Text = "License Key";
		this.licenseKeyToolStripMenuItem.Click += new System.EventHandler(licenseKeyToolStripMenuItem_Click);
		this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
		this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
		this.aboutToolStripMenuItem1.Text = "About";
		this.aboutToolStripMenuItem1.Click += new System.EventHandler(aboutToolStripMenuItem1_Click);
		this.hor_zoomin.Image = global::RITS.Properties.Resources.plus;
		this.hor_zoomin.Location = new System.Drawing.Point(878, 27);
		this.hor_zoomin.Name = "hor_zoomin";
		this.hor_zoomin.Size = new System.Drawing.Size(62, 45);
		this.hor_zoomin.TabIndex = 16;
		this.hor_zoomin.UseVisualStyleBackColor = true;
		this.hor_zoomin.Click += new System.EventHandler(hor_zoomin_Click);
		this.hor_zoomout.Image = global::RITS.Properties.Resources.minus;
		this.hor_zoomout.Location = new System.Drawing.Point(946, 27);
		this.hor_zoomout.Name = "hor_zoomout";
		this.hor_zoomout.Size = new System.Drawing.Size(62, 45);
		this.hor_zoomout.TabIndex = 15;
		this.hor_zoomout.UseVisualStyleBackColor = true;
		this.hor_zoomout.Click += new System.EventHandler(hor_zoomout_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1008, 516);
		base.Controls.Add(this.hor_zoomin);
		base.Controls.Add(this.hor_zoomout);
		base.Controls.Add(this.chart1);
		base.Controls.Add(this.print);
		base.Controls.Add(this.open);
		base.Controls.Add(this.menuStrip2);
		base.Icon = RITS_Properties_Resources.MainIcon;
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1024, 554);
		this.MinimumSize = new System.Drawing.Size(1024, 554);
		base.Name = "Form1";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		this.Text = "RITS (Riverside-Irvine Timing Diagram Solution)";
		((System.ComponentModel.ISupportInitialize)this.chart1).EndInit();
		this.menuStrip2.ResumeLayout(false);
		this.menuStrip2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public RITS(string[] args)
	{
		InitializeComponent();
		if (args.Length != 0)
		{
			launched_mode = true;
			ofFileName = args[0];
		}
		chart1.ChartAreas["ChartArea1"].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
		chart1.ChartAreas["ChartArea1"].AxisY.InterlacedColor = Color.FromArgb(60, Color.Blue);
		chart1.ChartAreas["ChartArea1"].AxisY.IsInterlaced = true;
		chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
		chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.0;
		chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5.0;
		chart1.ChartAreas["ChartArea1"].AxisY.IntervalOffset = 2.5;
		chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = true;
		chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = true;
		chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Transparent;
		chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
		chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
		chart1.Legends[0].Enabled = false;
		AxisScaleView scaleView = new AxisScaleView
		{
			Zoomable = true,
			SizeType = DateTimeIntervalType.Seconds
		};
		chart1.ChartAreas["ChartArea1"].AxisX.ScaleView = scaleView;
		chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
		if (launched_mode)
		{
			load_file(ofFileName);
		}
	}

	private void load_file(string filename)
	{
		string text = "";
		if (File.Exists(ofFileName))
		{
			StreamReader streamReader = new StreamReader(ofFileName);
			if (ofFileName.Contains("vcd"))
			{
				start = 0.0;
				end = 0.0;
				min = 0.0;
				max = 0.0;
				highest = 5;
				for (int i = 0; i < time_table.Count; i++)
				{
					time_table[i].Clear();
				}
				time_table.Clear();
				double num = 0.0;
				int val = 0;
				string text2;
				while ((text2 = streamReader.ReadLine()) != null)
				{
					char[] separator = new char[1] { ' ' };
					string[] array = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length < 1)
					{
						continue;
					}
					if (array[0] == "$var")
					{
						time_table.Add(new List<Timestamp>());
						time_table[time_table.Count() - 1].Add(new Timestamp(-1.0, 0, array[3], array[4]));
					}
					else if (array[0] == "$timescale")
					{
						text2 = streamReader.ReadLine();
						string[] array2 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (array2.Length < 2)
						{
							MessageBox.Show("Invalid timescale found in VCD file -- are you sure this is really a VCD file?", "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							for (int j = 0; j < time_table.Count; j++)
							{
								time_table[j].Clear();
							}
							time_table.Clear();
							return;
						}
						text = array2[1];
					}
					else
					{
						if (!(array[0].Substring(0, 1) == "#"))
						{
							continue;
						}
						string value = array[0].Substring(1, array[0].Length - 1);
						num = Convert.ToDouble(value);
						text2 = streamReader.ReadLine();
						string[] array3 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						string text3;
						if (array3[0] == "$dumpvars")
						{
							while ((text2 = streamReader.ReadLine()) != "$end")
							{
								string[] array4 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
								if (array4[0] == "b0")
								{
									val = 0;
								}
								else if (array4[0] == "b1")
								{
									val = 1;
								}
								text3 = array4[1];
								for (int k = 0; k < time_table.Count; k++)
								{
									if (time_table[k][0].name == text3)
									{
										time_table[k].Add(new Timestamp(num, val, text3));
									}
								}
							}
						}
						else
						{
							if (array3[0] == "b0")
							{
								val = 0;
							}
							else if (array3[0] == "b1")
							{
								val = 1;
							}
							try
							{
								text3 = array3[1];
							}
							catch
							{
								continue;
							}
							for (int l = 0; l < time_table.Count; l++)
							{
								if (time_table[l][0].name == text3)
								{
									time_table[l].Add(new Timestamp(num, val, text3));
								}
							}
						}
						num = 0.0;
						text3 = " ";
						value = " ";
						val = 0;
					}
				}
				for (int m = 0; m < time_table.Count(); m++)
				{
					int num2 = 0;
					for (int n = 0; n < chart1.Series.Count(); n++)
					{
						if (chart1.Series[n].Name == time_table[m][0].name)
						{
							num2 = 1;
						}
					}
					if (num2 == 0)
					{
						chart1.Series.Add(time_table[m][0].name);
					}
					chart1.Series[time_table[m][0].name].Points.Clear();
					chart1.Series[time_table[m][0].name].ChartType = SeriesChartType.StepLine;
					chart1.Series[time_table[m][0].name].Color = Color.Black;
				}
				for (int num3 = 0; num3 < time_table.Count(); num3++)
				{
					if (time_table[num3][time_table[num3].Count() - 1].time > end)
					{
						end = time_table[num3][time_table[num3].Count() - 1].time;
						max = end;
					}
				}
				chart1.ChartAreas["ChartArea1"].AxisX.Minimum = start;
				chart1.ChartAreas["ChartArea1"].AxisX.Maximum = end;
				chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Size = end - start;
				for (int num4 = 0; num4 < time_table.Count(); num4++)
				{
					if (time_table[num4][0].shown == 0)
					{
						time_table[num4][0].shown = highest;
						highest += 5;
					}
				}
				time_table[0][0].redraw(time_table, max, chart1, start, highest, end);
			}
			int num5 = 0;
			for (int num6 = 0; num6 < time_table.Count; num6++)
			{
				num5 = ((time_table[num6][0].show_name.Length > num5) ? time_table[num6][0].show_name.Length : num5);
			}
			string text4 = "{0,";
			text4 += num5;
			text4 += "}";
			for (int num7 = 0; num7 < time_table.Count; num7++)
			{
				time_table[num7][0].show_name = string.Format(text4, time_table[num7][0].show_name);
			}
			chart1.ChartAreas["ChartArea1"].AxisX.Title = "Time (" + text + ")";
		}
		else
		{
			MessageBox.Show(ofFileName + " Does Not Exist", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void open_Click(object sender, EventArgs e)
	{
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "VCD File|*.vcd*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\RIMS"
        };
        if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			ofFileName = openFileDialog.FileName;
			load_file(ofFileName);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "JPEG Image|*.jpg",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\RIMS",
            Title = "Save an Image File"
        };
        if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
		{
			chart1.SaveImage(saveFileDialog.FileName, ChartImageFormat.Jpeg);
		}
	}

	private void left_Click(object sender, EventArgs e)
	{
		start = chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
		end = chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
		double num = start - min;
		double num2 = end;
		end -= (end - start) / 10.0;
		start -= (num2 - start) / 10.0;
		if (start < min)
		{
			start = min;
			end = num2 - num;
		}
		if (time_table.Count > 0)
		{
			time_table[0][0].rescale_x(start, end, chart1);
		}
	}

	private void right_Click(object sender, EventArgs e)
	{
		double num = max - end;
		double num2 = start;
		double num3 = end;
		end += (end - start) / 10.0;
		start += (num3 - start) / 10.0;
		if (end > max)
		{
			end = max;
			start = num2 + num;
		}
		if (time_table.Count > 0)
		{
			time_table[0][0].rescale_x(start, end, chart1);
		}
	}

	private void hor_zoomin_Click(object sender, EventArgs e)
	{
		start = chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
		end = chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
		double num = start;
		double num2 = end;
		end -= (end - start) / 10.0;
		start += (num2 - start) / 10.0;
		if (end - start < 0.01)
		{
			end = num2;
			start = num;
		}
		if (time_table.Count > 0)
		{
			time_table[0][0].rescale_x(start, end, chart1);
		}
	}

	private void hor_zoomout_Click(object sender, EventArgs e)
	{
		start = chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
		end = chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
		double num = end;
		end += (end - start) / 8.0;
		start -= (num - start) / 8.0;
		if (end > max)
		{
			end = max;
		}
		if (start < min)
		{
			start = min;
		}
		if (time_table.Count > 0)
		{
			time_table[0][0].rescale_x(start, end, chart1);
		}
	}

	private void openToolStripMenuItem_Click(object sender, EventArgs e)
	{
		open_Click(sender, e);
	}

	private void saveAsJpegToolStripMenuItem_Click(object sender, EventArgs e)
	{
		print_Click(sender, e);
	}

	private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		hor_zoomin_Click(sender, e);
	}

	private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		hor_zoomout_Click(sender, e);
	}

	private void scrollLeftToolStripMenuItem_Click(object sender, EventArgs e)
	{
		left_Click(sender, e);
	}

	private void scrollRightToolStripMenuItem_Click(object sender, EventArgs e)
	{
		right_Click(sender, e);
	}

	private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
	{
		popup popup2 = new popup();
		popup2.Show();
	}

	private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
	{
		Feedback feedback = new Feedback();
		feedback.Show();
	}

	private void helpOnlineToolStripMenuItem_Click(object sender, EventArgs e)
	{
        Process.Start(new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = "https://www.programmingembeddedsystems.com/RITools/help/"
        });
    }

    private void chart1_Click(object sender, EventArgs e)
	{
	}

	private void licenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		KeyValidation keyValidation = new KeyValidation();
		keyValidation.ShowDialog();
        keyValidation.Size = keyValidation.MaximumSize = keyValidation.MinimumSize = new Size(this.Width, this.Height + 50);
    }
}
