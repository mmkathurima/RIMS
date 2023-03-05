using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RIMS_V2;

public class Peripheral
{
	public List<SymbolInfo> symbols;

	public string name;

	private Assembly assembly;

	private Dictionary<string, MethodInfo> methods;

	private object peripheral;

	private bool shown;

	public Peripheral(string assembly_path)
	{
		shown = false;
		assembly = Assembly.LoadFrom(assembly_path);
		name = Path.GetFileNameWithoutExtension(assembly_path);
		LoadMethods();
	}

	~Peripheral()
	{
	}

	public bool Show()
	{
		Invoke("ShowWindow", null);
		shown = true;
		return shown;
	}

	public bool Hide()
	{
		Invoke("HideWindow", null);
		shown = false;
		return shown;
	}

	public void ToggleShow()
	{
		shown = (shown ? Hide() : Show());
	}

	public void Reset()
	{
		Invoke("Reset", null);
	}

	private bool AssemblyLoaded()
	{
		if (assembly != null)
		{
			return true;
		}
		return false;
	}

	private void LoadMethods()
	{
		if (!AssemblyLoaded())
		{
			MessageBox.Show("Assembly not loaded.");
		}
		methods = new Dictionary<string, MethodInfo>();
		Type[] types = assembly.GetTypes();
		Type type = types.First((Type t) => t.Name == name);
		MethodInfo[] array = type.GetMethods();
		MethodInfo[] array2 = array;
		foreach (MethodInfo methodInfo in array2)
		{
			methods[methodInfo.Name] = methodInfo;
		}
		peripheral = Activator.CreateInstance(type);
	}

	public void WriteSymbol(string name, object value)
	{
		Invoke("WritePeripheral", name, value);
	}

	public void LoadSymbols()
	{
		symbols = (List<SymbolInfo>)Invoke("RegisterSymbols", null);
	}

	public List<SymbolInfo> GetSymbolUpdates()
	{
		return (List<SymbolInfo>)Invoke("GetSymbolUpdates", null);
	}

	private object Invoke(string function, params object[] args)
	{
		if (!AssemblyLoaded())
		{
			MessageBox.Show("Can't invoke function on unloaded assembly.");
		}
		MethodInfo methodInfo = methods[function];
		if (methodInfo == null)
		{
			MessageBox.Show("Could not find function in peripheral.");
		}
		return methodInfo.Invoke(peripheral, args);
	}
}
