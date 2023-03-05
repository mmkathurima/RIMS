using System.Net.Sockets;
using System.Text;

namespace RIBS_V3;

internal class StateObject
{
	public const int BUFFER_SIZE = 200;

	public Socket workSocket;

	public byte[] buffer = new byte[200];

	public StringBuilder sb = new StringBuilder();
}
