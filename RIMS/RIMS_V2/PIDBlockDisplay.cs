using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using RIMS_V2.Properties;

namespace RIMS_V2;

public class PIDBlockDisplay : Form
{
	private IContainer components;

	private PictureBox pictureBox1;

	private TableLayoutPanel tableLayoutPanel1;

	private Chart chart1;

	private Panel panel1;

	public Label actualTxt;

	public Label actuatorTxt;

	private Label label1;

	public Label actualInternalTxt;

	private Panel SystemPanel;

	private Label SystemText;

	private Label Rlabel;

	private TrackBar Rslider;

	private Label Rvalue;

	private int maxDisplay;

	private int maxPoints;

	private int windowStart;

	private double minX;

	private bool adjustWindow;

	private bool equations_shown;

	private int waitToInc;

	private double prevDesired;

	private EventHandler<ChartPaintEventArgs> handler;

	private PIDAnimation ball_animation;

	public PIDSimSystemParamUpdate SimDel;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.PIDBlockDisplay));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.actuatorTxt = new System.Windows.Forms.Label();
		this.actualTxt = new System.Windows.Forms.Label();
		this.SystemPanel = new System.Windows.Forms.Panel();
		this.Rvalue = new System.Windows.Forms.Label();
		this.Rlabel = new System.Windows.Forms.Label();
		this.Rslider = new System.Windows.Forms.TrackBar();
		this.actualInternalTxt = new System.Windows.Forms.Label();
		this.SystemText = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chart1).BeginInit();
		this.panel1.SuspendLayout();
		this.SystemPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Rslider).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Image = RIMS_V2.Properties.Resources.PIDBlock;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(463, 397);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121f));
		this.tableLayoutPanel1.Controls.Add(this.chart1, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.20642f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.79358f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(590, 717);
		this.tableLayoutPanel1.TabIndex = 2;
		chartArea.AxisX.Title = "Time (s)";
		chartArea.AxisY.Title = "Postion (m)";
		chartArea.Name = "ChartArea1";
		this.chart1.ChartAreas.Add(chartArea);
		this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Name = "Legend1";
		this.chart1.Legends.Add(legend);
		this.chart1.Location = new System.Drawing.Point(3, 406);
		this.chart1.Name = "chart1";
		series.ChartArea = "ChartArea1";
		series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
		series.Legend = "Legend1";
		series.Name = "Ball position";
		this.chart1.Series.Add(series);
		this.chart1.Size = new System.Drawing.Size(463, 308);
		this.chart1.TabIndex = 1;
		this.chart1.Text = "chart1";
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.actuatorTxt);
		this.panel1.Controls.Add(this.actualTxt);
		this.panel1.Controls.Add(this.SystemPanel);
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(463, 397);
		this.panel1.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(206, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(254, 13);
		this.label1.TabIndex = 5;
		this.label1.Text = "(Consider opening sample RIMS_ch12_PID_sample)";
		this.actuatorTxt.AutoSize = true;
		this.actuatorTxt.Location = new System.Drawing.Point(447, 352);
		this.actuatorTxt.Name = "actuatorTxt";
		this.actuatorTxt.Size = new System.Drawing.Size(13, 13);
		this.actuatorTxt.TabIndex = 4;
		this.actuatorTxt.Text = "0";
		this.actualTxt.AutoSize = true;
		this.actualTxt.Location = new System.Drawing.Point(170, 303);
		this.actualTxt.Name = "actualTxt";
		this.actualTxt.Size = new System.Drawing.Size(13, 13);
		this.actualTxt.TabIndex = 3;
		this.actualTxt.Text = "0";
		this.SystemPanel.BackColor = System.Drawing.Color.FromArgb(200, 225, 255);
		this.SystemPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.SystemPanel.Controls.Add(this.Rvalue);
		this.SystemPanel.Controls.Add(this.Rlabel);
		this.SystemPanel.Controls.Add(this.Rslider);
		this.SystemPanel.Controls.Add(this.actualInternalTxt);
		this.SystemPanel.Controls.Add(this.SystemText);
		this.SystemPanel.Cursor = System.Windows.Forms.Cursors.Hand;
		this.SystemPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SystemPanel.Location = new System.Drawing.Point(226, 278);
		this.SystemPanel.Name = "SystemPanel";
		this.SystemPanel.Size = new System.Drawing.Size(160, 116);
		this.SystemPanel.TabIndex = 7;
		this.SystemPanel.Click += new System.EventHandler(SystemPanel_Click);
		this.Rvalue.AutoSize = true;
		this.Rvalue.Location = new System.Drawing.Point(125, 75);
		this.Rvalue.Name = "Rvalue";
		this.Rvalue.Size = new System.Drawing.Size(21, 13);
		this.Rvalue.TabIndex = 11;
		this.Rvalue.Text = "10";
		this.Rlabel.AutoSize = true;
		this.Rlabel.Location = new System.Drawing.Point(24, 54);
		this.Rlabel.Name = "Rlabel";
		this.Rlabel.Size = new System.Drawing.Size(99, 13);
		this.Rlabel.TabIndex = 8;
		this.Rlabel.Text = "Responsiveness";
		this.Rslider.Location = new System.Drawing.Point(21, 70);
		this.Rslider.Maximum = 30;
		this.Rslider.Minimum = 1;
		this.Rslider.Name = "Rslider";
		this.Rslider.Size = new System.Drawing.Size(108, 45);
		this.Rslider.TabIndex = 8;
		this.Rslider.TickFrequency = 5;
		this.Rslider.Value = 10;
		this.Rslider.ValueChanged += new System.EventHandler(Rslider_ValueChanged);
		this.actualInternalTxt.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.actualInternalTxt.AutoSize = true;
		this.actualInternalTxt.BackColor = System.Drawing.SystemColors.Control;
		this.actualInternalTxt.Location = new System.Drawing.Point(3, 0);
		this.actualInternalTxt.Name = "actualInternalTxt";
		this.actualInternalTxt.Size = new System.Drawing.Size(32, 13);
		this.actualInternalTxt.TabIndex = 6;
		this.actualInternalTxt.Text = "0.00";
		this.SystemText.AutoSize = true;
		this.SystemText.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.SystemText.Location = new System.Drawing.Point(54, 0);
		this.SystemText.Name = "SystemText";
		this.SystemText.Size = new System.Drawing.Size(53, 18);
		this.SystemText.TabIndex = 8;
		this.SystemText.Text = "System";
		this.SystemText.Click += new System.EventHandler(SystemText_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(590, 717);
		base.Controls.Add(this.tableLayoutPanel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.MaximizeBox = false;
		base.Name = "PIDBlockDisplay";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
		this.Text = "PID simulation overview";
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.chart1).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.SystemPanel.ResumeLayout(false);
		this.SystemPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Rslider).EndInit();
		base.ResumeLayout(false);
	}

	public PIDBlockDisplay()
	{
		InitializeComponent();
		actualInternalTxt.Parent = SystemPanel;
		actualInternalTxt.BackColor = Color.Transparent;
		tableLayoutPanel1.SetColumnSpan(pictureBox1, 2);
		chart1.ChartAreas["ChartArea1"].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
		chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
		chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Black;
		chart1.ChartAreas["ChartArea1"].AxisX.Interval = 5.0;
		chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 30.0;
		chart1.ChartAreas["ChartArea1"].AxisY.Minimum = -1.0;
		chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 15.0;
		chart1.ChartAreas["ChartArea1"].AxisY.Interval = 1.0;
		chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = true;
		chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = true;
		chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Black;
		chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
		chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
		chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = true;
		chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = true;
		chart1.Legends[0].Enabled = false;
		AxisScaleView scaleView = new AxisScaleView
		{
			Zoomable = true,
			SizeType = DateTimeIntervalType.Seconds
		};
		chart1.ChartAreas["ChartArea1"].AxisX.ScaleView = scaleView;
		chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
		ball_animation = new PIDAnimation();
		tableLayoutPanel1.Controls.Add(ball_animation, 1, 1);
		maxPoints = 5000;
		maxDisplay = 145;
		windowStart = 0;
		minX = 0.0;
		waitToInc = 0;
		adjustWindow = false;
		prevDesired = -1.0;
		equations_shown = false;
	}

	public void addPoint(double x, double y, double desired)
	{
		windowStart = (windowStart + 1) % maxPoints;
		if (windowStart >= maxPoints - 1)
		{
			handler = delegate
			{
				softClear();
				chart1.Series["Ball position"].Points.AddXY(x, y);
				setDesired(desired);
				chart1.PostPaint -= handler;
			};
			chart1.PostPaint += handler;
		}
		else
		{
			windowAdjust();
			chart1.Series["Ball position"].Points.AddXY(x, y);
			setDesired(desired);
			ball_animation.Update(y, desired, Convert.ToDouble(actuatorTxt.Text));
		}
	}

	private void windowAdjust()
	{
		if ((windowStart > maxDisplay && !adjustWindow) || (adjustWindow && waitToInc >= 4))
		{
			minX += 1.0;
			chart1.ChartAreas["ChartArea1"].AxisX.Minimum = minX;
			chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Math.Floor(chart1.ChartAreas["ChartArea1"].AxisX.Maximum) + 1.0;
			adjustWindow = true;
			waitToInc = 0;
		}
		else if (adjustWindow)
		{
			waitToInc++;
		}
	}

	private void setDesired(double desired)
	{
		if (prevDesired == -1.0)
		{
			prevDesired = desired;
			double num = prevDesired;
			if (num == 15.0)
			{
				num = 14.9999;
			}
			chart1.ChartAreas["ChartArea1"].AxisY.StripLines.Clear();
			chart1.ChartAreas["ChartArea1"].AxisY.StripLines.Add(new StripLine
			{
				BorderColor = Color.Goldenrod,
				IntervalOffset = num,
				Text = "Desired"
			});
		}
		if (prevDesired != desired)
		{
			double num2 = desired;
			if (num2 == 15.0)
			{
				num2 = 14.9999;
			}
			chart1.ChartAreas["ChartArea1"].AxisY.StripLines.Clear();
			chart1.ChartAreas["ChartArea1"].AxisY.StripLines.Add(new StripLine
			{
				BorderColor = Color.Goldenrod,
				IntervalOffset = num2,
				Text = "Desired"
			});
		}
		prevDesired = desired;
	}

	public void clear()
	{
		chart1.Series["Ball position"].Points.Clear();
		windowStart = 0;
		minX = 0.0;
		waitToInc = 0;
		chart1.ChartAreas["ChartArea1"].AxisX.Minimum = minX;
		chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 30.0;
		adjustWindow = false;
		prevDesired = -1.0;
	}

	private void softClear()
	{
		minX = Math.Floor(chart1.ChartAreas["ChartArea1"].AxisX.Maximum);
		chart1.ChartAreas["ChartArea1"].AxisX.Minimum = minX;
		chart1.ChartAreas["ChartArea1"].AxisX.Maximum = minX + 30.0;
		chart1.Series["Ball position"].Points.Clear();
		windowStart = 0;
		waitToInc = 0;
		adjustWindow = false;
	}

	private void SystemPanel_Click(object sender, EventArgs e)
	{
		if (!equations_shown)
		{
			SystemText.Left -= 50;
			SystemText.Top -= 25;
			SystemText.Text = "Floating ball equations:\na_ball = (F_fan / m_ball) - g\ndv_ball/dt = a_ball\ndv_pos/dt = v_ball";
			equations_shown = true;
		}
		else
		{
			SystemText.Left += 50;
			SystemText.Top += 25;
			SystemText.Text = "System";
			equations_shown = false;
		}
	}

	private void SystemText_Click(object sender, EventArgs e)
	{
		SystemPanel_Click(sender, e);
	}

	public void Rslider_ValueChanged(object sender, EventArgs e)
	{
		TrackBar trackBar = (TrackBar)sender;
		double num = trackBar.Value;
		Rvalue.Text = num.ToString();
		SimDel("R", ((double)(trackBar.Maximum + 1) - num) * 0.001);
	}
}
