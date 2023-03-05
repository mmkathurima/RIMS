using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RIMS_V2;

public class PIDSimSystem
{
    private class RCSystem
    {
        private double V;

        private double V_p;

        public double R;

        public double C;

        private double l;

        public RCSystem()
        {
            V = 0.0;
            R = 0.01;
            C = 1000.0;
            l = 0.1;
        }

        public double Update(double Vin)
        {
            Vin *= 0.001;
            V_p = V + (-1.0 * (V / (R * C)) + Vin / R - l) * 0.011760099999999999;
            V = V_p switch
            {
                > 15.0 => 15.0,
                < 0.0 => 0.0,
                _ => V_p,
            };
            return V;
        }
    }

    public delegate double ResponseDel(double Actual, double timePassed, int timeCnt);

    private const int TIMER_PERIOD = 10;

    private const double SIM_SPEED = 11.7601;

    private const double DT = 0.011760099999999999;

    private int timeCnt;

    private RCSystem rc;

    private double Actual;

    private double Actuator;

    private MainForm rimsRef;

    private Thread pidSimThread;

    private bool isRunning;

    private bool paused;

    private ResponseDel responseDel;

    public PIDSimSystem(MainForm mf, ResponseDel responseDel)
    {
        rimsRef = mf;
        this.responseDel = responseDel;
    }

    public void UpdateParameter(string name, double value)
    {
        try
        {
            if (name == "R")
            {
                rc.R = value;
            }
            else if (name == "C")
            {
                rc.C = value;
            }
        }
        catch (NullReferenceException)
        {
        }
    }

    private void init()
    {
        rc = new RCSystem();
        Actual = 0.0;
        isRunning = false;
        paused = false;
        timeCnt = 0;
        pidSimThread = new Thread((ThreadStart)delegate
        {
            run();
        });
    }

    public void start()
    {
        init();
        pidSimThread.Start();
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
                updateSystem();
                Thread.Sleep(10);
            }
        }
    }

    public void stop()
    {
        if (isRunning)
        {
            isRunning = false;
            try
            {
                pidSimThread.Abort();
            }
            catch (PlatformNotSupportedException pe)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    MessageBox.Show(null, pe.ToString() + "\nExiting...", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }

    private void updateSystem()
    {
        Actuator = responseDel(Actual, (double)timeCnt * 0.011760099999999999, timeCnt);
        Actual = rc.Update(Actuator);
        timeCnt++;
    }
}
