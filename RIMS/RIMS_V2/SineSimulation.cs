using System;
using System.Threading;

namespace RIMS_V2;

internal class SineSimulation
{
	public delegate void ResponseDel(double val);

	private const int TIMER_PERIOD = 10;

	private Thread waveSimThread;

	private double max;

	private double min;

	private bool isRunning;

	private bool paused;

	private ResponseDel responseDel;

	private double amplitude;

	private double frequency;

	private double phase;

	private double bias;

	private double val;

	private int timeCnt;

	private MainForm rimsRef;

	public SineSimulation(MainForm rimsRef, ResponseDel responseDel, double amplitude, double frequency, double phase, double bias)
	{
		this.rimsRef = rimsRef;
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.phase = phase;
		this.bias = bias;
		this.responseDel = responseDel;
		max = 31.0;
		min = -32.0;
	}

	private void init()
	{
		isRunning = false;
		paused = false;
		timeCnt = 0;
		val = 0.0;
		waveSimThread = new Thread((ThreadStart)delegate
		{
			run();
		});
	}

	public void start()
	{
		init();
		waveSimThread.Start();
	}

	public void pause()
	{
		paused = true;
	}

	public void unpause()
	{
		paused = false;
	}

	private void run()
	{
		isRunning = true;
		while (isRunning)
		{
			if (!paused)
			{
				updateOutput();
				Thread.Sleep(10);
			}
		}
	}

	public void stop()
	{
		if (isRunning)
		{
			isRunning = false;
			rimsRef.Invoke((Action)delegate
			{
				waveSimThread.Join();
			});
		}
	}

	private void updateOutput()
	{
		val = amplitude * Math.Sin(Math.PI * 2.0 * frequency * ((double)(timeCnt * 10) / 1000.0) + phase) + bias;
		timeCnt++;
		if (Math.PI * 2.0 * frequency * ((double)(timeCnt * 10) / 1000.0) >= Math.PI * 2.0)
		{
			timeCnt = 0;
		}
		responseDel(ADC(val));
	}

	private int ADC(double input)
	{
		int num = 0;
		if (input >= 0.0)
		{
			return (int)(max * (input / amplitude));
		}
		return (int)(min * (input / (0.0 - amplitude)));
	}
}
