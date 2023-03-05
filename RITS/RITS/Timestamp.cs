using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace RITS;

internal class Timestamp
{
	public double time;

	public int value;

	public string name;

	public string show_name;

	public int shown;

	public void rescale_x(double start, double end, Chart chart1)
	{
		chart1.ChartAreas["ChartArea1"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
		if (end - start >= 1.0)
		{
			start = Math.Round(start, 2);
			end = Math.Round(end, 2);
		}
		if (end - start >= 5.0)
		{
			start = Math.Round(start, 1);
			end = Math.Round(end, 1);
		}
		if (end - start >= 10.0)
		{
			start = (int)start;
			end = (int)end;
		}
		if (end - start < 60.0)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 2.0;
		}
		if (end - start < 30.0)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
		}
		if (end - start < 7.5)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.5;
		}
		if (end - start < 1.5)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
		}
		if (end - start < 0.75)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.05;
		}
		if (end - start < 0.15)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.01;
		}
		if (end - start < 0.075)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.005;
		}
		if (end - start < 0.015)
		{
			chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.001;
		}
		start = Math.Round(start, 3);
		end = Math.Round(end, 3);
		chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(start, end);
	}

	public void redraw(List<List<Timestamp>> time_table, double max, Chart chart1, double start, int highest, double end)
	{
		chart1.ChartAreas["ChartArea1"].AxisY.CustomLabels.Clear();
		chart1.ChartAreas["ChartArea1"].AxisY.Maximum = highest;
		chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.0;
		int num = 5;
		for (int i = 0; i < time_table.Count(); i++)
		{
			string text = time_table[i][0].name;
			chart1.Series[text].Points.Clear();
			if (time_table[i][0].shown > 0)
			{
				chart1.Series[text].Points.AddXY(0.0, time_table[i][0].shown);
                CustomLabel customLabel = new CustomLabel
                {
                    FromPosition = (double)time_table[i][0].shown - 2.5,
                    ToPosition = (double)time_table[i][0].shown + 2.5,
                    Text = time_table[i][0].show_name
                };
                chart1.ChartAreas["ChartArea1"].AxisY.CustomLabels.Add(customLabel);
			}
		}
		num = 5;
		for (int j = 0; j < time_table.Count(); j++)
		{
			string text2 = time_table[j][0].name;
			if (time_table[j][0].shown <= 0)
			{
				continue;
			}
			num += 5;
			int num2 = 0;
			for (int k = 0; k < time_table[j].Count(); k++)
			{
				if (k > 0 && time_table[j][k].time < time_table[j][k - 1].time)
				{
					num2 = 1;
				}
				if (num2 == 0)
				{
					chart1.Series[text2].Points.AddXY(time_table[j][k].time, (double)time_table[j][k].value * 2.5 + (double)time_table[j][0].shown);
				}
			}
		}
		num = 5;
		for (int l = 0; l < time_table.Count(); l++)
		{
			string label = time_table[l][0].name;
			if (time_table[l][0].shown > 0)
			{
				num += 5;
				chart1.Series[label].Points.AddXY((int)max + 1, (double)time_table[l][^1].value * 2.5 + (double)time_table[l][0].shown);
				chart1.Series[label].Points[1].Label = label;
			}
		}
		time_table[0][0].rescale_x(start, end, chart1);
	}

	public Timestamp(double time1, int val, string name1)
	{
		time = time1;
		value = val;
		name = name1;
		show_name = "";
	}

	public Timestamp(double time1, int val, string name1, string show_name1)
	{
		time = time1;
		value = val;
		name = name1;
		show_name = show_name1;
	}
}
