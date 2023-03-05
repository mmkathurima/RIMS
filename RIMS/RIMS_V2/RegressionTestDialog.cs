using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RIMS_V2;

public class RegressionTestDialog : Form
{
	private const uint PULSE_DELAY = 50u;

	private static VM vm;

	private IntPtr clock_addr;

	private IntPtr breakpoint_step;

	private ThreadStruct thread_info;

	private InitStruct init_info;

	private uint ipp;

	private IContainer components;

	private Button RunRegressionTestButton;

	private Label label1;

	public RegressionTestDialog()
	{
		InitializeComponent();
		ipp = 50u;
		vm = new VM();
	}

	private unsafe void RunRegressionTest(object sender, EventArgs e)
	{
		vm.vm = VMInterface.CreateVM();
		vm.ts = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ThreadStruct)));
		thread_info = (ThreadStruct)Marshal.PtrToStructure(vm.ts, typeof(ThreadStruct));
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InitStruct)));
		byte* ptr = (byte*)intPtr.ToPointer();
		for (int i = 0; i < Marshal.SizeOf(typeof(InitStruct)); i++)
		{
			ptr[i] = 0;
		}
		VMInterface.Initialize(vm.vm, intPtr);
		init_info = (InitStruct)Marshal.PtrToStructure(intPtr, typeof(InitStruct));
		clock_addr = init_info.clock;
		breakpoint_step = init_info.breakpoint_pulse;
		TimerInterface.timeBeginPeriod(10u);
		VMInterface.SetIPP(vm.vm, ipp);
		thread_info = default(ThreadStruct);
		init_info = default(InitStruct);
		Marshal.FreeHGlobal(vm.ts);
		Marshal.FreeHGlobal(intPtr);
		vm.vm = IntPtr.Zero;
		vm.ts = IntPtr.Zero;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.RegressionTestDialog));
		this.RunRegressionTestButton = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.RunRegressionTestButton.Location = new System.Drawing.Point(159, 124);
		this.RunRegressionTestButton.Name = "RunRegressionTestButton";
		this.RunRegressionTestButton.Size = new System.Drawing.Size(116, 23);
		this.RunRegressionTestButton.TabIndex = 0;
		this.RunRegressionTestButton.Text = "Run regression test";
		this.RunRegressionTestButton.UseVisualStyleBackColor = true;
		this.RunRegressionTestButton.Click += new System.EventHandler(RunRegressionTest);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(393, 91);
		this.label1.TabIndex = 1;
		this.label1.Text = resources.GetString("label1.Text");
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(428, 159);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.RunRegressionTestButton);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "RegressionTestDialog";
		this.Text = "Run Regression Test";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
