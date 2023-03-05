using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace RIMS_V2;

internal class Validation
{
	private delegate void AddTextCallback(string text);

	private const ushort PORT = 80;

	private volatile bool connected;

	private Socket speaker;

	[DllImport("validation.dll")]
	public static extern uint validate([MarshalAs(UnmanagedType.LPStr)] string cdkey);

	private void AddText(string text)
	{
	}

	private void HandleText(string text)
	{
	}

	public void Connect(string server)
	{
		IPAddress[] hostAddresses = Dns.GetHostAddresses(server);
		if (hostAddresses.Length == 0)
		{
			throw new Exception("No server by that name.");
		}
		IPAddress address = hostAddresses[0];
		IPEndPoint remoteEP = new IPEndPoint(address, 80);
		speaker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
		try
		{
			speaker.Connect(remoteEP);
		}
		catch (SocketException)
		{
			HandleText("<Couldn't connect...>\r\n");
			connected = false;
		}
		if (speaker.Connected)
		{
			HandleText("<Connected!>\r\n");
			connected = true;
		}
		else
		{
			HandleText("<Couldn't Connect...>\r\n");
			connected = false;
		}
	}

	public void SendMessage(string msg)
	{
		if (!connected || !speaker.Connected)
		{
			connected = false;
			speaker.Close();
		}
		else
		{
			byte[] bytes = Encoding.ASCII.GetBytes(msg);
			speaker.Send(bytes);
		}
	}

	public void CloseConnection()
	{
		speaker.Close();
		connected = false;
	}
}
