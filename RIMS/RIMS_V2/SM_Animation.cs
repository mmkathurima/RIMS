using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RIMS_V2;

internal class SM_Animation
{
	private const string start = "start";

	private const string stop = "stop";

	private const string update = "update";

	private Socket channel;

	private bool connected;

	private int port;

	private IPEndPoint remote;

	private bool initialized;

	private bool sent_start;

	public SM_Animation()
	{
		channel = null;
		connected = false;
		sent_start = false;
		port = 0;
		remote = null;
		initialized = false;
		sent_start = false;
	}

	public SM_Animation(int port)
	{
		this.port = port;
		IPAddress address = IPAddress.Parse("127.0.0.1");
		remote = new IPEndPoint(address, port);
		channel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
		initialized = true;
		connected = false;
		sent_start = false;
		sent_start = false;
	}

	public bool Close()
	{
		channel.Close();
		return true;
	}

	public bool Connect()
	{
		if (!initialized)
		{
			throw new Exception("Not ready!");
		}
		if (channel.Connected)
		{
			connected = false;
			sent_start = false;
			throw new Exception("Already connected!");
		}
		channel.Close();
		channel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
		try
		{
			channel.Connect(remote);
		}
		catch (SocketException)
		{
			connected = false;
			sent_start = false;
			return false;
		}
		if (!channel.Connected)
		{
			connected = false;
			sent_start = false;
			return false;
		}
		connected = true;
		return true;
	}

	public bool UpdateState(string vname, int value)
	{
		if (!initialized || !connected || !sent_start)
		{
			sent_start = false;
		}
		string s = vname + "=" + value + ":";
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
		socketAsyncEventArgs.SetBuffer(bytes, 0, bytes.Length);
		socketAsyncEventArgs.UserToken = "update";
		socketAsyncEventArgs.Completed += send_completed;
		if (!channel.SendAsync(socketAsyncEventArgs))
		{
			if (socketAsyncEventArgs.SocketError == SocketError.Success)
			{
				return true;
			}
			connected = false;
			sent_start = false;
		}
		return false;
	}

	public bool SendStart()
	{
		if (!initialized || !connected)
		{
			throw new Exception("Not ready!");
		}
		string s = "start";
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
		socketAsyncEventArgs.SetBuffer(bytes, 0, bytes.Length);
		socketAsyncEventArgs.UserToken = "start";
		socketAsyncEventArgs.Completed += send_completed;
		if (!channel.SendAsync(socketAsyncEventArgs))
		{
			if (socketAsyncEventArgs.SocketError != 0)
			{
				connected = false;
				sent_start = false;
				return false;
			}
			return true;
		}
		return true;
	}

	public bool SendStop()
	{
		if (!initialized || !connected || !sent_start)
		{
			throw new Exception("Not ready!");
		}
		string s = "stop";
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
		socketAsyncEventArgs.SetBuffer(bytes, 0, bytes.Length);
		socketAsyncEventArgs.UserToken = "stop";
		socketAsyncEventArgs.Completed += send_completed;
		if (!channel.SendAsync(socketAsyncEventArgs))
		{
			if (socketAsyncEventArgs.SocketError == SocketError.Success)
			{
				return true;
			}
			connected = false;
			sent_start = false;
		}
		return false;
	}

	private void send_completed(object sender, SocketAsyncEventArgs e)
	{
		if (e.SocketError != 0)
		{
			Application.Exit();
			connected = false;
			sent_start = false;
		}
		else if (e.UserToken != null && (string)e.UserToken == "start")
		{
			sent_start = true;
		}
		else if ((string)e.UserToken == "stop")
		{
			channel.Shutdown(SocketShutdown.Both);
			channel.Close();
		}
	}

	public bool ReadyForUpdates()
	{
		return sent_start;
	}
}
