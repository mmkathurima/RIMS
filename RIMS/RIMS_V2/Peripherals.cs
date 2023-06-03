using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RIMS_V2;

public class Peripherals
{
	private MappedIOWrite del;

	private List<Peripheral> peripherals;

	private PeripheralManager periph_manager;

	public Peripherals(string path)
	{
		peripherals = new List<Peripheral>();
		LoadPeripherals(path);
		periph_manager = new PeripheralManager(ref peripherals);
		del = write_dispatch;
	}

	public void ShowManager(object sender, EventArgs e)
	{
		periph_manager.ShowWindow();
	}

	public void write_dispatch(IntPtr rec_ptr)
	{
		MappedSymbolRecord rec = (MappedSymbolRecord)Marshal.PtrToStructure(rec_ptr, typeof(MappedSymbolRecord));
		Peripheral peripheral = peripherals.Find((Peripheral p) => p.symbols.Exists((SymbolInfo s) => s.name == rec.name));
		if (peripheral == null)
		{
			MessageBox.Show("Unknown peripheral symbol being written.");
		}
		else
		{
			peripheral.WriteSymbol(rec.name, rec.value);
		}
	}

	private void LoadPeripherals(string path)
	{
		string[] files = Directory.GetFiles(path, "*_periph.dll");
		string[] array = files;
		string[] array2 = array;
		foreach (string assembly_path in array2)
		{
			Peripheral item = new Peripheral(assembly_path);
			peripherals.Add(item);
		}
	}

	public void RegisterPeripheralsWithVM(VM vm)
	{
		foreach (Peripheral peripheral in peripherals)
		{
			peripheral.LoadSymbols();
			foreach (SymbolInfo symbol in peripheral.symbols)
			{
				SymbolRecord symbolRecord = default;
				symbolRecord.name = symbol.name;
				symbolRecord.content_length = symbol.width;
				symbolRecord.address = 0u;
				symbolRecord.in_data_segment = 0;
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
				Marshal.StructureToPtr(symbolRecord, intPtr, fDeleteOld: false);
				VMInterface.RegisterSymbol(vm.vm, intPtr);
			}
			peripheral.Reset();
		}
		VMInterface.SetMappedIOWrite(vm.vm, del);
	}

	public void UpdateSymbols(VM vm)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
		foreach (Peripheral peripheral in peripherals)
		{
			List<SymbolInfo> symbolUpdates = peripheral.SymbolUpdates;
			foreach (SymbolInfo item in symbolUpdates)
			{
				int symbolIndex = VMInterface.GetSymbolIndex(vm.vm, item.name);
				VMInterface.GetSymbol(vm.vm, symbolIndex, intPtr);
				SymbolRecord symbolRecord = (SymbolRecord)Marshal.PtrToStructure(intPtr, typeof(SymbolRecord));
				VMInterface.SetData(vm.vm, (ushort)symbolRecord.address, (MemoryWidth)symbolRecord.content_length, (uint)item.value);
			}
		}
		Marshal.FreeHGlobal(intPtr);
	}

	public void ToggleView(string name)
	{
		peripherals.Find((Peripheral c) => c.name == name).ToggleShow();
	}

	public void ShowAll()
	{
		foreach (Peripheral peripheral in peripherals)
		{
			peripheral.Show();
		}
	}
}
