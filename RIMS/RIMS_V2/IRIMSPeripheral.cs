using System.Collections.Generic;

namespace RIMS_V2;

public interface IRIMSPeripheral
{
	List<SymbolInfo> RegisterSymbols();

	List<SymbolInfo> GetSymbolUpdates();

	void WritePeripheral(string name, object value);

	void Reset();

	void ShowWindow();

	void HideWindow();

	void Dispose();
}
