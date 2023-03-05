using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RIBS_V3;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(TabControl))]
public class SuperTabControl : TabControl
{
	private IContainer components;

	public event EventHandler<SuperTabControlEventArgs> TabClosed;

	public event EventHandler<SuperTabControlEventArgs> AddTab;

	public event EventHandler<SuperTabControlEventArgs> ChangeTab;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}

	public SuperTabControl()
	{
		InitializeComponent();
		_ = base.Handle;
	}

	protected virtual void OnTabClosed(SuperTabControlEventArgs e)
	{
		this.TabClosed?.Invoke(this, e);
	}

	protected virtual void OnAddTab(SuperTabControlEventArgs e)
	{
		this.AddTab?.Invoke(this, e);
	}

	protected virtual void OnChangeTab(SuperTabControlEventArgs e)
	{
		this.ChangeTab?.Invoke(this, e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		Point location = e.Location;
		if (GetTabRect(base.TabCount - 1).Contains(location))
		{
			this.AddTab(null, null);
			return;
		}
		for (int i = 0; i < base.TabPages.Count - 1; i++)
		{
			Point location2 = GetTabRect(i).Location;
			if (base.SelectedIndex == i)
			{
				location2.X += 5;
				location2.Y += 7;
			}
			else
			{
				location2.X += 3;
				location2.Y += 9;
			}
			if (Distance(location2, location) < 12.0)
			{
                SuperTabControlEventArgs superTabControlEventArgs = new SuperTabControlEventArgs(base.TabPages[i], 0, TabControlAction.Deselecting)
                {
                    closed_index = i
                };
                this.TabClosed(null, superTabControlEventArgs);
				return;
			}
		}
		for (int j = 0; j < base.TabPages.Count - 1; j++)
		{
			if (GetTabRect(j).Contains(location))
			{
                SuperTabControlEventArgs superTabControlEventArgs2 = new SuperTabControlEventArgs(base.TabPages[j], 0, TabControlAction.Deselecting)
                {
                    closed_index = j
                };
                this.ChangeTab(null, superTabControlEventArgs2);
				break;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		Point location = e.Location;
		for (int i = 0; i < base.TabPages.Count - 1; i++)
		{
			Point location2 = GetTabRect(i).Location;
			if (base.SelectedIndex == i)
			{
				location2.X += 5;
				location2.Y += 7;
			}
			else
			{
				location2.X += 3;
				location2.Y += 9;
			}
			if (Distance(location2, location) < 12.0)
			{
				if (base.TabPages[i].ImageIndex == 0)
				{
					base.TabPages[i].ImageIndex = 1;
					break;
				}
			}
			else if (base.TabPages[i].ImageIndex == 1)
			{
				base.TabPages[i].ImageIndex = 0;
				break;
			}
		}
	}

    private double Distance(Point a, Point b) => Math.Sqrt(Math.Pow(b.X - a.X, 2.0) - Math.Pow(b.Y - a.Y, 2.0));
}
