using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RIMS_V2.Properties;

namespace RIMS_V2;

public class PIDAnimation : UserControl
{
	private class BalloonObj
	{
		private Panel canvas;

		private LinearGradientBrush brush;

		private double px_scale;

		private Size base_size = new Size(25, 25);

		public BalloonObj(Panel canvas)
		{
			this.canvas = canvas;
			brush = new LinearGradientBrush(new Point(10, 10), new Point(10, 150), Color.White, Color.Red);
			brush.SetBlendTriangularShape(0.5f);
			px_scale = 12.75;
		}

		public void Draw(Graphics g, double size, double target)
		{
			size *= 4.0;
			target *= 4.0;
			int num = base_size.Width + (int)size;
			int num2 = base_size.Height + (int)size;
			int num3 = base_size.Width + (int)target;
			int num4 = base_size.Height + (int)target;
			Size size2 = new Size(num, num2);
			Size size3 = new Size(num3, num4);
			Point point = new Point(canvas.Width / 2, canvas.Height / 2);
			Point location = new Point(point.X - num / 2, point.Y - num2 / 2);
			Point location2 = new Point(point.X - num3 / 2, point.Y - num4 / 2);
			g.FillEllipse(brush, new Rectangle(location, size2));
			Pen pen = new Pen(Color.Goldenrod);
			pen.Width = 2f;
			pen.DashStyle = DashStyle.Dash;
			g.DrawEllipse(pen, new Rectangle(location2, size3));
		}
	}

	private class FanObj
	{
		private PictureBox fan_img;

		private Image img;

		private float angle;

		public FanObj(PictureBox fan_img)
		{
			angle = 0f;
			this.fan_img = fan_img;
			img = (Image)this.fan_img.Image.Clone();
		}

		public void Draw(double speed)
		{
			angle = (angle + (float)(speed / 255.0) * 100f) % 360f;
			fan_img.Image = RotateImage(img, angle);
		}

		public Bitmap RotateImage(Image image, float angle)
		{
			Bitmap bitmap = new Bitmap(image.Width, image.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.TranslateTransform((float)image.Width / 2f, (float)image.Height / 2f);
			graphics.RotateTransform(angle);
			graphics.TranslateTransform((0f - (float)image.Width) / 2f, (0f - (float)image.Height) / 2f);
			graphics.DrawImage(image, new Point(0, 0));
			graphics.Dispose();
			return bitmap;
		}
	}

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel ball_canvas;

	private PictureBox pictureBox1;

	private BalloonObj balloon;

	private FanObj fan;

	private Graphics gball;

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
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.ball_canvas = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.tableLayoutPanel1.ColumnCount = 1;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Controls.Add(this.ball_canvas, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.60976f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.39024f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(140, 178);
		this.tableLayoutPanel1.TabIndex = 0;
		this.ball_canvas.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.ball_canvas.Location = new System.Drawing.Point(3, 3);
		this.ball_canvas.Name = "ball_canvas";
		this.ball_canvas.Size = new System.Drawing.Size(134, 128);
		this.ball_canvas.TabIndex = 0;
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pictureBox1.Image = RIMS_V2.Properties.Resources.fan;
		this.pictureBox1.Location = new System.Drawing.Point(51, 137);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(37, 37);
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tableLayoutPanel1);
		base.Name = "PIDAnimation";
		base.Size = new System.Drawing.Size(142, 208);
		this.tableLayoutPanel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}

	public PIDAnimation()
	{
		InitializeComponent();
		balloon = new BalloonObj(ball_canvas);
		fan = new FanObj(pictureBox1);
		gball = ball_canvas.CreateGraphics();
	}

	private void ClearCanvas()
	{
		gball.FillRectangle(SystemBrushes.Control, ball_canvas.ClientRectangle);
	}

	public void Update(double param, double target, double speed)
	{
		pictureBox1.Visible = true;
		ClearCanvas();
		balloon.Draw(gball, param, target);
		fan.Draw(speed);
		pictureBox1.Refresh();
	}
}
