using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RIBS_V3;

internal class RIMSInterface
{
	private int port;

	private bool connected;

	private Socket socket;

	private IPEndPoint ep;

	public RIMSInterface(int p)
	{
		connected = false;
		port = p;
		IPAddress address = IPAddress.Parse("127.0.0.1");
		ep = new IPEndPoint(address, port);
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
		try
		{
			socket.Connect(ep);
			connected = true;
		}
		catch (SocketException ex)
		{
			MessageBox.Show("Socket error trying to have RIMS start a simulation for Easynotify. Is RIMS open?\r\n" + ex.Message);
			connected = false;
		}
	}

	public void StartSim(string filename, string[] state_vars)
	{
		if (connected)
		{
			string text = filename + "::";
			foreach (string text2 in state_vars)
			{
				text = text + text2 + "::";
			}
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			socket.Send(bytes);
		}
	}

	public void EndSim()
	{
		if (connected)
		{
			byte[] bytes = Encoding.ASCII.GetBytes("END");
			socket.Send(bytes);
		}
	}

	public void Close()
	{
		if (connected)
		{
			socket.Close();
			connected = false;
		}
	}
}
