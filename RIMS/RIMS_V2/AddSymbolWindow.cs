using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RIMS_V2;

public class AddSymbolWindow : Form
{
	private IContainer components;

	private ListView SymbolsList;

	private ColumnHeader Symbol;

	private ColumnHeader Address;

	private Button AddSymbol;

	private ColumnHeader CurrentValue;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.AddSymbolWindow));
		this.SymbolsList = new System.Windows.Forms.ListView();
		this.Symbol = new System.Windows.Forms.ColumnHeader();
		this.Address = new System.Windows.Forms.ColumnHeader();
		this.CurrentValue = new System.Windows.Forms.ColumnHeader();
		this.AddSymbol = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.SymbolsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.Symbol, this.Address, this.CurrentValue });
		this.SymbolsList.FullRowSelect = true;
		this.SymbolsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.SymbolsList.LabelWrap = false;
		this.SymbolsList.Location = new System.Drawing.Point(12, 12);
		this.SymbolsList.Name = "SymbolsList";
		this.SymbolsList.Size = new System.Drawing.Size(316, 211);
		this.SymbolsList.TabIndex = 23;
		this.SymbolsList.TabStop = false;
		this.SymbolsList.UseCompatibleStateImageBehavior = false;
		this.SymbolsList.View = System.Windows.Forms.View.Details;
		this.Symbol.Text = "Name";
		this.Symbol.Width = 160;
		this.Address.Text = "Address";
		this.Address.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Address.Width = 50;
		this.CurrentValue.Text = "Current value";
		this.CurrentValue.Width = 80;
		this.AddSymbol.Enabled = false;
		this.AddSymbol.Location = new System.Drawing.Point(197, 229);
		this.AddSymbol.Name = "AddSymbol";
		this.AddSymbol.Size = new System.Drawing.Size(84, 23);
		this.AddSymbol.TabIndex = 24;
		this.AddSymbol.Text = "Watch these";
		this.AddSymbol.UseVisualStyleBackColor = true;
		this.AddSymbol.Click += new System.EventHandler(AddSymbol_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(339, 262);
		base.Controls.Add(this.AddSymbol);
		base.Controls.Add(this.SymbolsList);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.Name = "AddSymbolWindow";
		this.Text = "Add Symbols...";
		base.ResumeLayout(false);
	}

	public int strlen(byte[] str)
	{
		int i;
		for (i = 0; str[i] != 0; i++)
		{
		}
		return i;
	}

	public unsafe AddSymbolWindow()
	{
		InitializeComponent();
		SymbolsList.Items.Clear();
		uint symbol = VMInterface.GetSymbol(MainForm.vm.vm, -1, IntPtr.Zero);
		for (int i = 0; i < symbol; i++)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
			intPtr.ToPointer();
			VMInterface.GetSymbol(MainForm.vm.vm, i, intPtr);
			SymbolRecord symbolRecord = (SymbolRecord)Marshal.PtrToStructure(intPtr, typeof(SymbolRecord));
			Marshal.FreeHGlobal(intPtr);
			if (symbolRecord.in_data_segment == 1)
			{
				string name = symbolRecord.name;
				ListViewItem value = new ListViewItem(name)
				{
					SubItems = 
					{
						symbolRecord.address.ToString(),
						((int)VMInterface.GetData(MainForm.vm.vm, (ushort)symbolRecord.address, (MemoryWidth)symbolRecord.content_length)).ToString()
					},
					Tag = name
				};
				SymbolsList.Items.Add(value);
				if (!AddSymbol.Enabled)
				{
					AddSymbol.Enabled = true;
				}
			}
		}
	}

	private void AddSymbol_Click(object sender, EventArgs e)
	{
		List<string> list = new List<string>();
		foreach (ListViewItem lv in SymbolsList.SelectedItems)
		{
			list.Add(lv.Tag as string);
		}
		base.Tag = list;
		base.DialogResult = DialogResult.OK;
	}
}
