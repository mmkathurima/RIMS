using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RIBS_V3;

internal class TransitionIfElseForm : Form
{
	private DataTable table;

	private BindingSource bs;

	private Node node;

	private Rectangle dragbox;

	private int drag_index;

	private bool otherexists;

	private IContainer components;

	private DataGridView dataGridView1;

	private Button button1;

	private Button button2;

	public TransitionIfElseForm()
	{
		InitializeComponent();
		otherexists = false;
		dragbox = Rectangle.Empty;
		dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
		dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
	}

	private DataTable GenerateData()
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("Order");
		dataTable.Columns.Add("Priority");
		dataTable.Columns.Add("Condition");
		dataTable.Columns.Add("Destination");
		foreach (Edge edge in node.edges)
		{
			if (edge.GetT() == node)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Destination"] = edge.GetH().Name;
				dataRow["Condition"] = edge.GetCondition();
				dataRow["Priority"] = edge.GetPriority();
				dataTable.Rows.Add(dataRow);
			}
		}
		if (!otherexists)
		{
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["Destination"] = node.Name;
			dataRow2["Condition"] = "other";
			if (node.edges.Count >= 1)
			{
				dataRow2["Priority"] = node.edges.Max((Edge m) => m.GetPriority()) + 1;
			}
			else
			{
				dataRow2["Priority"] = 0;
			}
			dataTable.Rows.Add(dataRow2);
		}
		return dataTable;
	}

	public void Bind(Node n)
	{
		node = n;
		Edge edge = node.edges.Find((Edge ed) => ed.GetT() == node && ed.GetCondition() == "other");
		otherexists = edge != null;
		table = GenerateData();
		bs = new BindingSource(table, null);
		dataGridView1.DataSource = bs;
		dataGridView1.Columns["Order"].FillWeight = 35f;
		dataGridView1.Columns["Priority"].Visible = false;
		foreach (DataGridViewColumn column in dataGridView1.Columns)
		{
			column.ReadOnly = true;
		}
		dataGridView1.RowsDefaultCellStyle.BackColor = SystemColors.Window;
		dataGridView1.RowsDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		dataGridView1.Sort(dataGridView1.Columns["Priority"], ListSortDirection.Ascending);
	}

	private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
	{
		drag_index = dataGridView1.HitTest(e.X, e.Y).RowIndex;
		if (drag_index != -1)
		{
			Size dragSize = SystemInformation.DragSize;
			dragbox = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
			return;
		}
		foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
		{
			selectedRow.Selected = false;
		}
		dragbox = Rectangle.Empty;
	}

	private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && dragbox != Rectangle.Empty && !dragbox.Contains(e.X, e.Y))
		{
			dataGridView1.DoDragDrop(dataGridView1.Rows[drag_index], DragDropEffects.Move);
		}
	}

	private void dataGridView1_DragOver(object sender, DragEventArgs e)
	{
		e.Effect = DragDropEffects.Move;
	}

	private void dataGridView1_DragDrop(object sender, DragEventArgs e)
	{
		Point point = dataGridView1.PointToClient(new Point(e.X, e.Y));
		int rowIndex = dataGridView1.HitTest(point.X, point.Y).RowIndex;
		if (rowIndex == -1)
		{
			drag_index = -1;
			dragbox = Rectangle.Empty;
			return;
		}
		if (drag_index == dataGridView1.Rows.Count - 1 || rowIndex == dataGridView1.Rows.Count - 1)
		{
			drag_index = -1;
			dragbox = Rectangle.Empty;
			return;
		}
		if (e.Effect == DragDropEffects.Move)
		{
			DataGridViewRow dataGridViewRow = dataGridView1.Rows[drag_index];
			DataGridViewRow dataGridViewRow2 = dataGridView1.Rows[rowIndex];
			string destination = dataGridViewRow.Cells["Destination"].Value.ToString();
			string condition = dataGridViewRow.Cells["Condition"].Value.ToString();
			Edge edge = node.edges.Find((Edge ed) => ed.GetT() == node && ed.GetH().Name == destination && ed.GetCondition() == condition);
			if (edge == null)
			{
				throw new Exception("Could not find matching edge");
			}
			int priority = Convert.ToInt32(dataGridViewRow2.Cells["Priority"].Value);
			edge.SetPriority(priority);
			destination = dataGridViewRow2.Cells["Destination"].Value.ToString();
			condition = dataGridViewRow2.Cells["Condition"].Value.ToString();
			edge = node.edges.Find((Edge ed) => ed.GetT() == node && ed.GetH().Name == destination && ed.GetCondition() == condition);
			if (edge == null)
			{
				throw new Exception("Could not find matching edge");
			}
			int priority2 = Convert.ToInt32(dataGridViewRow.Cells["Priority"].Value);
			edge.SetPriority(priority2);
            (dataGridViewRow2.Cells["Priority"].Value, dataGridViewRow.Cells["Priority"].Value) = 
				(dataGridViewRow.Cells["Priority"].Value, dataGridViewRow2.Cells["Priority"].Value);
            table.AcceptChanges();
		}
		dataGridView1.Sort(dataGridView1.Columns["Priority"], ListSortDirection.Ascending);
	}

	private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
	{
		e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
	}

	private void dataGridView1_Sorted(object sender, EventArgs e)
	{
		for (int i = 0; i < dataGridView1.Rows.Count; i++)
		{
			if (i == 0)
			{
				dataGridView1.Rows[i].Cells["Order"].Value = "if";
			}
			else if (i == dataGridView1.Rows.Count - 1)
			{
				dataGridView1.Rows[i].Cells["Order"].Value = "else";
			}
			else
			{
				dataGridView1.Rows[i].Cells["Order"].Value = "else if";
			}
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (dataGridView1.SelectedRows.Count == 1)
		{
			drag_index = dataGridView1.SelectedRows[0].Index;
			int num = drag_index - 1;
			if (num == -1)
			{
				drag_index = -1;
				return;
			}
			if (drag_index == dataGridView1.Rows.Count - 1 || num == dataGridView1.Rows.Count - 1)
			{
				drag_index = -1;
				dragbox = Rectangle.Empty;
				return;
			}
			bs.RaiseListChangedEvents = false;
			DataGridViewRow dataGridViewRow = dataGridView1.Rows[drag_index];
			DataGridViewRow dataGridViewRow2 = dataGridView1.Rows[num];
			UpdatePriority(dataGridViewRow2, dataGridViewRow);
            (dataGridViewRow2.Cells["Priority"].Value, dataGridViewRow.Cells["Priority"].Value) =
				(dataGridViewRow.Cells["Priority"].Value, dataGridViewRow2.Cells["Priority"].Value);
            dataGridView1.Sort(dataGridView1.Columns["Priority"], ListSortDirection.Ascending);
			dataGridView1.Rows[num].Selected = true;
			bs.RaiseListChangedEvents = true;
			bs.ResetBindings(metadataChanged: false);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (dataGridView1.SelectedRows.Count == 1)
		{
			drag_index = dataGridView1.SelectedRows[0].Index;
			int num = drag_index + 1;
			if (num >= dataGridView1.Rows.Count)
			{
				drag_index = -1;
				return;
			}
			if (drag_index == dataGridView1.Rows.Count - 1 || num == dataGridView1.Rows.Count - 1)
			{
				drag_index = -1;
				dragbox = Rectangle.Empty;
				return;
			}
			bs.RaiseListChangedEvents = false;
			DataGridViewRow dataGridViewRow = dataGridView1.Rows[drag_index];
			DataGridViewRow dataGridViewRow2 = dataGridView1.Rows[num];
			UpdatePriority(dataGridViewRow2, dataGridViewRow);
			object obj = dataGridViewRow.Cells["Priority"].Value.ToString();
			dataGridViewRow.Cells["Priority"].Value = dataGridViewRow2.Cells["Priority"].Value.ToString();
			dataGridViewRow2.Cells["Priority"].Value = obj.ToString();
			dataGridView1.Sort(dataGridView1.Columns["Priority"], ListSortDirection.Ascending);
			dataGridView1.Rows[num].Selected = true;
			bs.RaiseListChangedEvents = true;
			bs.ResetBindings(metadataChanged: false);
		}
	}

	private void UpdatePriority(DataGridViewRow rowToMove, DataGridViewRow fromRow)
	{
		string destination = rowToMove.Cells["Destination"].Value.ToString();
		string condition = rowToMove.Cells["Condition"].Value.ToString();
		Edge edge = node.edges.Find((Edge ed) => ed.GetT() == node && ed.GetH().Name == destination && ed.GetCondition() == condition);
		if (edge == null)
		{
			throw new Exception("Could not find matching edge");
		}
		int priority = Convert.ToInt32(fromRow.Cells["Priority"].Value);
		edge.SetPriority(priority);
		destination = fromRow.Cells["Destination"].Value.ToString();
		condition = fromRow.Cells["Condition"].Value.ToString();
		edge = node.edges.Find((Edge ed) => ed.GetT() == node && ed.GetH().Name == destination && ed.GetCondition() == condition);
		if (edge == null)
		{
			throw new Exception("Could not find matching edge");
		}
		int priority2 = Convert.ToInt32(rowToMove.Cells["Priority"].Value);
		edge.SetPriority(priority2);
	}

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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.TransitionIfElseForm));
		this.dataGridView1 = new System.Windows.Forms.DataGridView();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
		base.SuspendLayout();
		this.dataGridView1.AllowDrop = true;
		this.dataGridView1.AllowUserToAddRows = false;
		this.dataGridView1.AllowUserToDeleteRows = false;
		this.dataGridView1.AllowUserToResizeRows = false;
		this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		this.dataGridView1.Location = new System.Drawing.Point(9, 6);
		this.dataGridView1.MultiSelect = false;
		this.dataGridView1.Name = "dataGridView1";
		this.dataGridView1.ReadOnly = true;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		this.dataGridView1.RowHeadersVisible = false;
		this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
		this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
		this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView1.Size = new System.Drawing.Size(280, 173);
		this.dataGridView1.TabIndex = 2;
		this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(dataGridView1_MouseDown);
		this.dataGridView1.Sorted += new System.EventHandler(dataGridView1_Sorted);
		this.dataGridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(dataGridView1_MouseMove);
		this.dataGridView1.DragOver += new System.Windows.Forms.DragEventHandler(dataGridView1_DragOver);
		this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(dataGridView1_DragDrop);
		this.dataGridView1.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(dataGridView1_ColumnAdded);
		this.button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
		this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button1.Location = new System.Drawing.Point(290, 6);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(22, 25);
		this.button1.TabIndex = 3;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Image = (System.Drawing.Image)resources.GetObject("button2.Image");
		this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button2.Location = new System.Drawing.Point(290, 154);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(22, 25);
		this.button2.TabIndex = 4;
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(314, 191);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.dataGridView1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "TransitionIfElseForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		this.Text = "Transition order";
		base.TopMost = true;
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
		base.ResumeLayout(false);
	}
}
