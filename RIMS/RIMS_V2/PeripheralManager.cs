using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RIMS_V2;

public class PeripheralManager : Form
{
	private IContainer components;

	private ListBox peripherals_box;

	private CheckBox enable_periph;

	private Dictionary<string, bool> periph_views;

	private List<Peripheral> periphs;

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
		this.peripherals_box = new System.Windows.Forms.ListBox();
		this.enable_periph = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.peripherals_box.FormattingEnabled = true;
		this.peripherals_box.Location = new System.Drawing.Point(13, 13);
		this.peripherals_box.Name = "peripherals_box";
		this.peripherals_box.Size = new System.Drawing.Size(220, 121);
		this.peripherals_box.TabIndex = 0;
		this.peripherals_box.SelectedIndexChanged += new System.EventHandler(peripherals_box_SelectedIndexChanged);
		this.enable_periph.AutoSize = true;
		this.enable_periph.Location = new System.Drawing.Point(13, 140);
		this.enable_periph.Name = "enable_periph";
		this.enable_periph.Size = new System.Drawing.Size(108, 17);
		this.enable_periph.TabIndex = 1;
		this.enable_periph.Text = "Enable peripheral";
		this.enable_periph.UseVisualStyleBackColor = true;
		this.enable_periph.CheckedChanged += new System.EventHandler(enable_periph_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(245, 163);
		base.Controls.Add(this.enable_periph);
		base.Controls.Add(this.peripherals_box);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "PeripheralManager";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		this.Text = "PeripheralManager";
		base.TopMost = true;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PeripheralManager_FormClosing);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public PeripheralManager(ref List<Peripheral> periphs)
	{
		InitializeComponent();
		this.periphs = periphs;
		periph_views = new Dictionary<string, bool>();
		foreach (Peripheral periph in periphs)
		{
			periph_views[periph.name] = false;
			peripherals_box.Items.Add(periph.name);
		}
	}

	public void ShowWindow()
	{
		Show();
	}

	private Peripheral SelectedPeripheral()
	{
		try
		{
			string selected_periph = peripherals_box.SelectedItem.ToString();
			return periphs.First((Peripheral c) => c.name == selected_periph);
		}
		catch (NullReferenceException)
		{
			return null;
		}
	}

	private void enable_periph_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox checkBox = (CheckBox)sender;
		Peripheral peripheral = SelectedPeripheral();
		if (peripheral != null)
		{
			periph_views[peripheral.name] = checkBox.Checked;
			if (checkBox.Checked)
			{
				peripheral.Show();
			}
			else
			{
				peripheral.Hide();
			}
		}
	}

	private void PeripheralManager_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason != CloseReason.WindowsShutDown && e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.TaskManagerClosing)
		{
			e.Cancel = true;
			Hide();
		}
	}

	private void peripherals_box_SelectedIndexChanged(object sender, EventArgs e)
	{
		Peripheral peripheral = SelectedPeripheral();
		if (peripheral != null)
		{
			enable_periph.Checked = periph_views[peripheral.name];
		}
	}
}
