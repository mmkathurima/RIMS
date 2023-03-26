using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RIBS_V3;

public class RIBS : Form
{
    private enum Mode
    {
        Idle,
        AddEdge,
        MovingNode,
        MovingHandle,
        Simulation,
        Scroll
    }

    private delegate void StopSimulationDelegate();

    private const int MAX_SCROLL = 400;

    private const int BUF_SZ = 1500;

    private const int SRCCOPY = 13369376;

    private IContainer components;

    private Button InsertTransition_Button;

    private Button GenerateC_Button;

    private Button SimulateStateMachine_Button;

    private Label label10;

    private Label label11;

    private Label label12;

    private MenuStrip menuStrip1;

    private ToolStripMenuItem fileToolStripMenuItem;

    private ToolStripMenuItem openToolStripMenuItem;

    private ToolStripMenuItem saveAsToolStripMenuItem;

    private ToolStripMenuItem helpToolStripMenuItem;

    private Button InsertState_Button;

    private Label label9;

    private GroupBox ControlsGroupBox;

    private ToolStripMenuItem aboutToolStripMenuItem1;

    private ContextMenuStrip EdgeContextMenu;

    private ToolStripMenuItem convertToLineToolStripMenuItem;

    private ToolStripMenuItem convertToBezierToolStripMenuItem;

    private ToolStripSeparator toolStripSeparator1;

    private ToolStripMenuItem exitToolStripMenuItem;

    private ToolTip InsertTransitionTooltip;

    private ToolTip EditTransitionToolTip;

    private ToolTip StateActionTooltip;

    private ToolTip TransitionConditionTooltip;

    private ToolTip EnableTimerTooltip;

    private ToolTip EnableUartTooltip;

    private ToolStripMenuItem newToolStripMenuItem;

    private ToolStripMenuItem saveToolStripMenuItem;

    private ToolStripMenuItem editToolStripMenuItem;

    private ToolStripMenuItem disableTooltipsToolStripMenuItem1;

    private ToolStripMenuItem onlineHelpToolStripMenuItem;

    private ToolStripMenuItem reportsBugsFeedbackToolStripMenuItem;

    private ToolStripMenuItem reservedVariableNamesToolStripMenuItem;

    private GroupBox groupBox2;

    private TextBox MacroCode_Textbox;

    private TextBox projectnamebox;

    private Label label4;

    private CheckBox globaluartcheckbox;

    private GroupBox StateMachineGroupBox;

    private Label label13;

    private GroupBox groupBox1;

    private TextBox LocalCode_TextBox;

    private TextBox StateMachineName_Textbox;

    private TextBox StateMachinePeriod_Textbox;

    private TextBox StateMachinePrefix_Textbox;

    private Label label1;

    private Label label2;

    private Label label3;

    private GroupBox ObjectGroupBox;

    private CheckBox ObjectInitialState_Checkbox;

    private Button ObjectDelete_Button;

    private TextBox ObjectCondition_Textbox;

    private TextBox ObjectName_Textbox;

    private Label label6;

    private Label label8;

    private Label label7;

    private SuperTabControl tabs;

    private ImageList tabimages;

    private GroupBox groupBox4;

    public Panel backgroundpanel;

    private TabPage plustab;

    private ToolStripMenuItem insertStateMachineToolStripMenuItem;

    private ToolStripMenuItem closeStateMachineToolStripMenuItem;

    private ToolStripMenuItem licenseKeyToolStripMenuItem;

    private System.Windows.Forms.Timer pulsetimer;

    private ToolStripMenuItem openSampleToolStripMenuItem;

    private Label label5;

    private ToolStripMenuItem exportAsJPGToolStripMenuItem;

    private ToolStripSeparator toolStripSeparator2;

    private HScrollBar hScrollBar1;

    private VScrollBar vScrollBar1;

    private Label label14;

    private ComboBox smType;

    private ContextMenuStrip NodeContextMenu;

    private ToolStripMenuItem ifelse;

    private ToolStripMenuItem basic;

    private ToolStripMenuItem forloop;

    public TextBox ObjectActions_Textbox;

    private Socket rimsSocket;

    private Socket rimsWorkerSocket;

    private int rimsanimationPort;

    private Process simulator;

    private Thread simthread;

    private bool is_running;

    private About about;

    private ReservedNames ReservedNames;

    private EmptyStateMachineWarning emptystatemachinewarning;

    private abbreviationrepeat abbreviationrepeat;

    private warning warning;

    private Feedback feedback;

    private ToolStripMenuItem clicked_item;

    private bool editingtext_mode;

    private int view_mode;

    private BufferedGraphics graphic;

    private BufferedGraphicsContext graphicmanager;

    private List<Graph> graph = new List<Graph>();

    private int current_graph;

    private uint num_graphs;

    private int next_socket = 1098;

    private Node selected_node;

    private Edge selected_edge;

    private int selected_handle;

    private Edge newedge;

    private int x_offset;

    private int y_offset;

    private float scalefactor;

    private Point click_offset;

    private int oldx;

    private int oldy;

    private int origx;

    private int origy;

    private int scroll_offset = -200;

    private bool tooltips_enabled;

    private bool is_saved = true;

    private bool rims_mode;

    private bool just_loaded;

    private string executable_directory;

    private string documents_directory;

    private string c_filename;

    private string filename = "";

    private string filepath = "";

    private string default_path = "";

    private Form toClose;

    private bool insTransActive;

    private Bitmap memImage;

    private Mode mode;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIBS_V3.RIBS));
        this.InsertTransition_Button = new System.Windows.Forms.Button();
        this.GenerateC_Button = new System.Windows.Forms.Button();
        this.SimulateStateMachine_Button = new System.Windows.Forms.Button();
        this.label10 = new System.Windows.Forms.Label();
        this.label11 = new System.Windows.Forms.Label();
        this.label12 = new System.Windows.Forms.Label();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openSampleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.exportAsJPGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.insertStateMachineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.closeStateMachineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.disableTooltipsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.onlineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.reportsBugsFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.reservedVariableNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.licenseKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.InsertState_Button = new System.Windows.Forms.Button();
        this.label9 = new System.Windows.Forms.Label();
        this.ControlsGroupBox = new System.Windows.Forms.GroupBox();
        this.EdgeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.convertToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.convertToBezierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.InsertTransitionTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.EditTransitionToolTip = new System.Windows.Forms.ToolTip(this.components);
        this.StateActionTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.TransitionConditionTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.EnableTimerTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.EnableUartTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.MacroCode_Textbox = new System.Windows.Forms.TextBox();
        this.projectnamebox = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.globaluartcheckbox = new System.Windows.Forms.CheckBox();
        this.tabimages = new System.Windows.Forms.ImageList(this.components);
        this.StateMachineGroupBox = new System.Windows.Forms.GroupBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.LocalCode_TextBox = new System.Windows.Forms.TextBox();
        this.label13 = new System.Windows.Forms.Label();
        this.StateMachineName_Textbox = new System.Windows.Forms.TextBox();
        this.StateMachinePeriod_Textbox = new System.Windows.Forms.TextBox();
        this.StateMachinePrefix_Textbox = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.backgroundpanel = new System.Windows.Forms.Panel();
        this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
        this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
        this.ObjectGroupBox = new System.Windows.Forms.GroupBox();
        this.ObjectInitialState_Checkbox = new System.Windows.Forms.CheckBox();
        this.ObjectActions_Textbox = new System.Windows.Forms.TextBox();
        this.ObjectDelete_Button = new System.Windows.Forms.Button();
        this.ObjectCondition_Textbox = new System.Windows.Forms.TextBox();
        this.ObjectName_Textbox = new System.Windows.Forms.TextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.label8 = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        this.pulsetimer = new System.Windows.Forms.Timer(this.components);
        this.label14 = new System.Windows.Forms.Label();
        this.smType = new System.Windows.Forms.ComboBox();
        this.NodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.basic = new System.Windows.Forms.ToolStripMenuItem();
        this.ifelse = new System.Windows.Forms.ToolStripMenuItem();
        this.forloop = new System.Windows.Forms.ToolStripMenuItem();
        this.tabs = new RIBS_V3.SuperTabControl();
        this.plustab = new System.Windows.Forms.TabPage();
        this.menuStrip1.SuspendLayout();
        this.ControlsGroupBox.SuspendLayout();
        this.EdgeContextMenu.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.StateMachineGroupBox.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.groupBox4.SuspendLayout();
        this.backgroundpanel.SuspendLayout();
        this.ObjectGroupBox.SuspendLayout();
        this.NodeContextMenu.SuspendLayout();
        this.tabs.SuspendLayout();
        base.SuspendLayout();
        this.InsertTransition_Button.Enabled = false;
        this.InsertTransition_Button.Location = new System.Drawing.Point(197, 12);
        this.InsertTransition_Button.Name = "InsertTransition_Button";
        this.InsertTransition_Button.Size = new System.Drawing.Size(93, 29);
        this.InsertTransition_Button.TabIndex = 2;
        this.InsertTransition_Button.Text = "Insert transition";
        this.InsertTransition_Button.UseVisualStyleBackColor = true;
        this.InsertTransition_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(InsertTransition_Button_MouseClick);
        this.GenerateC_Button.Enabled = false;
        this.GenerateC_Button.Location = new System.Drawing.Point(353, 12);
        this.GenerateC_Button.Name = "GenerateC_Button";
        this.GenerateC_Button.Size = new System.Drawing.Size(93, 29);
        this.GenerateC_Button.TabIndex = 3;
        this.GenerateC_Button.Text = "Generate C";
        this.GenerateC_Button.UseVisualStyleBackColor = true;
        this.GenerateC_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(GenerateC_Button_MouseClick);
        this.SimulateStateMachine_Button.Enabled = false;
        this.SimulateStateMachine_Button.Location = new System.Drawing.Point(507, 12);
        this.SimulateStateMachine_Button.Name = "SimulateStateMachine_Button";
        this.SimulateStateMachine_Button.Size = new System.Drawing.Size(93, 29);
        this.SimulateStateMachine_Button.TabIndex = 4;
        this.SimulateStateMachine_Button.Text = "RIMS simulation";
        this.SimulateStateMachine_Button.UseVisualStyleBackColor = true;
        this.SimulateStateMachine_Button.Click += new System.EventHandler(SimulateStateMachine_Button_Click);
        this.label10.AutoSize = true;
        this.label10.Location = new System.Drawing.Point(463, 20);
        this.label10.Name = "label10";
        this.label10.Size = new System.Drawing.Size(38, 13);
        this.label10.TabIndex = 27;
        this.label10.Text = "Step 4";
        this.label11.AutoSize = true;
        this.label11.Location = new System.Drawing.Point(309, 20);
        this.label11.Name = "label11";
        this.label11.Size = new System.Drawing.Size(38, 13);
        this.label11.TabIndex = 28;
        this.label11.Text = "Step 3";
        this.label12.AutoSize = true;
        this.label12.Location = new System.Drawing.Point(156, 20);
        this.label12.Name = "label12";
        this.label12.Size = new System.Drawing.Size(38, 13);
        this.label12.TabIndex = 29;
        this.label12.Text = "Step 2";
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.fileToolStripMenuItem, this.editToolStripMenuItem, this.helpToolStripMenuItem });
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.TabStop = true;
        this.menuStrip1.Text = "menuStrip1";
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.newToolStripMenuItem, this.openToolStripMenuItem, this.openSampleToolStripMenuItem, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.toolStripSeparator1, this.exportAsJPGToolStripMenuItem, this.toolStripSeparator2, this.exitToolStripMenuItem });
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "File";
        this.newToolStripMenuItem.Name = "newToolStripMenuItem";
        this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.newToolStripMenuItem.Text = "New file";
        this.newToolStripMenuItem.Click += new System.EventHandler(newToolStripMenuItem_Click);
        this.openToolStripMenuItem.Name = "openToolStripMenuItem";
        this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.openToolStripMenuItem.Text = "Open file";
        this.openToolStripMenuItem.Click += new System.EventHandler(fileToolStripMenuItem1_Click);
        this.openSampleToolStripMenuItem.Name = "openSampleToolStripMenuItem";
        this.openSampleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.openSampleToolStripMenuItem.Text = "Open sample";
        this.openSampleToolStripMenuItem.Click += new System.EventHandler(sampleToolStripMenuItem_Click);
        this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.saveToolStripMenuItem.Text = "Save file";
        this.saveToolStripMenuItem.Click += new System.EventHandler(saveToolStripMenuItem_Click);
        this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
        this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.saveAsToolStripMenuItem.Text = "Save file as...";
        this.saveAsToolStripMenuItem.Click += new System.EventHandler(saveAsToolStripMenuItem_Click);
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
        this.exportAsJPGToolStripMenuItem.Name = "exportAsJPGToolStripMenuItem";
        this.exportAsJPGToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.exportAsJPGToolStripMenuItem.Text = "Export as JPG";
        this.exportAsJPGToolStripMenuItem.Click += new System.EventHandler(exportAsJPGToolStripMenuItem_Click);
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
        this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.exitToolStripMenuItem.Text = "Exit";
        this.exitToolStripMenuItem.Click += new System.EventHandler(exitToolStripMenuItem_Click);
        this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.insertStateMachineToolStripMenuItem, this.closeStateMachineToolStripMenuItem, this.disableTooltipsToolStripMenuItem1 });
        this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
        this.editToolStripMenuItem.Text = "Edit";
        this.insertStateMachineToolStripMenuItem.Name = "insertStateMachineToolStripMenuItem";
        this.insertStateMachineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
        this.insertStateMachineToolStripMenuItem.Text = "Insert state machine";
        this.insertStateMachineToolStripMenuItem.Click += new System.EventHandler(insertStateMachineToolStripMenuItem_Click);
        this.closeStateMachineToolStripMenuItem.Name = "closeStateMachineToolStripMenuItem";
        this.closeStateMachineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
        this.closeStateMachineToolStripMenuItem.Text = "Close state machine";
        this.closeStateMachineToolStripMenuItem.Click += new System.EventHandler(closeStateMachineToolStripMenuItem_Click);
        this.disableTooltipsToolStripMenuItem1.CheckOnClick = true;
        this.disableTooltipsToolStripMenuItem1.Name = "disableTooltipsToolStripMenuItem1";
        this.disableTooltipsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
        this.disableTooltipsToolStripMenuItem1.Text = "Disable tooltips";
        this.disableTooltipsToolStripMenuItem1.Click += new System.EventHandler(disableTooltipsToolStripMenuItem_Click);
        this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.onlineHelpToolStripMenuItem, this.reportsBugsFeedbackToolStripMenuItem, this.reservedVariableNamesToolStripMenuItem, this.licenseKeyToolStripMenuItem, this.aboutToolStripMenuItem1 });
        this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.helpToolStripMenuItem.Text = "Help";
        this.onlineHelpToolStripMenuItem.Name = "onlineHelpToolStripMenuItem";
        this.onlineHelpToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
        this.onlineHelpToolStripMenuItem.Text = "Help online";
        this.onlineHelpToolStripMenuItem.Click += new System.EventHandler(onlineHelpToolStripMenuItem_Click);
        this.reportsBugsFeedbackToolStripMenuItem.Name = "reportsBugsFeedbackToolStripMenuItem";
        this.reportsBugsFeedbackToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
        this.reportsBugsFeedbackToolStripMenuItem.Text = "Reports bugs/feedback";
        this.reportsBugsFeedbackToolStripMenuItem.Click += new System.EventHandler(reportsBugsFeedbackToolStripMenuItem_Click);
        this.reservedVariableNamesToolStripMenuItem.Name = "reservedVariableNamesToolStripMenuItem";
        this.reservedVariableNamesToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
        this.reservedVariableNamesToolStripMenuItem.Text = "Reserved variable names";
        this.reservedVariableNamesToolStripMenuItem.Click += new System.EventHandler(reservedVariableNamesToolStripMenuItem_Click);
        this.licenseKeyToolStripMenuItem.Name = "licenseKeyToolStripMenuItem";
        this.licenseKeyToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
        this.licenseKeyToolStripMenuItem.Text = "License key";
        this.licenseKeyToolStripMenuItem.Click += new System.EventHandler(licenseKeyToolStripMenuItem_Click);
        this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
        this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(203, 22);
        this.aboutToolStripMenuItem1.Text = "About";
        this.aboutToolStripMenuItem1.Click += new System.EventHandler(aboutToolStripMenuItem1_Click);
        this.InsertState_Button.Location = new System.Drawing.Point(56, 12);
        this.InsertState_Button.Name = "InsertState_Button";
        this.InsertState_Button.Size = new System.Drawing.Size(93, 29);
        this.InsertState_Button.TabIndex = 1;
        this.InsertState_Button.Text = "Insert state";
        this.InsertState_Button.UseVisualStyleBackColor = true;
        this.InsertState_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(InsertState_Button_MouseClick);
        this.label9.AutoSize = true;
        this.label9.Location = new System.Drawing.Point(12, 20);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(38, 13);
        this.label9.TabIndex = 26;
        this.label9.Text = "Step 1";
        this.ControlsGroupBox.Controls.Add(this.SimulateStateMachine_Button);
        this.ControlsGroupBox.Controls.Add(this.label10);
        this.ControlsGroupBox.Controls.Add(this.GenerateC_Button);
        this.ControlsGroupBox.Controls.Add(this.InsertTransition_Button);
        this.ControlsGroupBox.Controls.Add(this.label11);
        this.ControlsGroupBox.Controls.Add(this.label12);
        this.ControlsGroupBox.Controls.Add(this.InsertState_Button);
        this.ControlsGroupBox.Controls.Add(this.label9);
        this.ControlsGroupBox.Location = new System.Drawing.Point(6, 35);
        this.ControlsGroupBox.Name = "ControlsGroupBox";
        this.ControlsGroupBox.Size = new System.Drawing.Size(613, 55);
        this.ControlsGroupBox.TabIndex = 35;
        this.ControlsGroupBox.TabStop = false;
        this.ControlsGroupBox.Text = "Controls";
        this.EdgeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.convertToLineToolStripMenuItem, this.convertToBezierToolStripMenuItem });
        this.EdgeContextMenu.Name = "EdgeContextMenu";
        this.EdgeContextMenu.Size = new System.Drawing.Size(168, 48);
        this.convertToLineToolStripMenuItem.Name = "convertToLineToolStripMenuItem";
        this.convertToLineToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.convertToLineToolStripMenuItem.Text = "Convert To Line";
        this.convertToLineToolStripMenuItem.Click += new System.EventHandler(convertToLineToolStripMenuItem_Click);
        this.convertToBezierToolStripMenuItem.Name = "convertToBezierToolStripMenuItem";
        this.convertToBezierToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.convertToBezierToolStripMenuItem.Text = "Convert To Bezier";
        this.convertToBezierToolStripMenuItem.Click += new System.EventHandler(convertToBezierToolStripMenuItem_Click);
        this.InsertTransitionTooltip.AutomaticDelay = 1;
        this.InsertTransitionTooltip.AutoPopDelay = 10000;
        this.InsertTransitionTooltip.InitialDelay = 1000;
        this.InsertTransitionTooltip.IsBalloon = true;
        this.InsertTransitionTooltip.ReshowDelay = 100;
        this.InsertTransitionTooltip.Tag = "";
        this.InsertTransitionTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.InsertTransitionTooltip.ToolTipTitle = "Tip!";
        this.InsertTransitionTooltip.UseAnimation = false;
        this.InsertTransitionTooltip.UseFading = false;
        this.EditTransitionToolTip.Active = false;
        this.EditTransitionToolTip.AutomaticDelay = 1;
        this.EditTransitionToolTip.AutoPopDelay = 10000;
        this.EditTransitionToolTip.InitialDelay = 1000;
        this.EditTransitionToolTip.IsBalloon = true;
        this.EditTransitionToolTip.ReshowDelay = 100;
        this.EditTransitionToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.EditTransitionToolTip.ToolTipTitle = "Tip!";
        this.EditTransitionToolTip.UseAnimation = false;
        this.EditTransitionToolTip.UseFading = false;
        this.EditTransitionToolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(EditTransitionToolTip_Draw);
        this.StateActionTooltip.AutomaticDelay = 1;
        this.StateActionTooltip.AutoPopDelay = 10000;
        this.StateActionTooltip.InitialDelay = 1000;
        this.StateActionTooltip.IsBalloon = true;
        this.StateActionTooltip.ReshowDelay = 100;
        this.StateActionTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.StateActionTooltip.ToolTipTitle = "Tip!";
        this.StateActionTooltip.UseAnimation = false;
        this.StateActionTooltip.UseFading = false;
        this.TransitionConditionTooltip.AutomaticDelay = 1;
        this.TransitionConditionTooltip.AutoPopDelay = 10000;
        this.TransitionConditionTooltip.InitialDelay = 1000;
        this.TransitionConditionTooltip.IsBalloon = true;
        this.TransitionConditionTooltip.ReshowDelay = 100;
        this.TransitionConditionTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.TransitionConditionTooltip.ToolTipTitle = "Tip!";
        this.TransitionConditionTooltip.UseAnimation = false;
        this.TransitionConditionTooltip.UseFading = false;
        this.EnableTimerTooltip.AutomaticDelay = 1;
        this.EnableTimerTooltip.AutoPopDelay = 10000;
        this.EnableTimerTooltip.InitialDelay = 1000;
        this.EnableTimerTooltip.IsBalloon = true;
        this.EnableTimerTooltip.ReshowDelay = 100;
        this.EnableTimerTooltip.ShowAlways = true;
        this.EnableTimerTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.EnableTimerTooltip.ToolTipTitle = "Tip!";
        this.EnableTimerTooltip.UseAnimation = false;
        this.EnableTimerTooltip.UseFading = false;
        this.EnableUartTooltip.AutomaticDelay = 1;
        this.EnableUartTooltip.AutoPopDelay = 10000;
        this.EnableUartTooltip.InitialDelay = 1000;
        this.EnableUartTooltip.IsBalloon = true;
        this.EnableUartTooltip.ReshowDelay = 100;
        this.EnableUartTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.EnableUartTooltip.ToolTipTitle = "Tip!";
        this.EnableUartTooltip.UseAnimation = false;
        this.EnableUartTooltip.UseFading = false;
        this.groupBox2.Controls.Add(this.MacroCode_Textbox);
        this.groupBox2.Location = new System.Drawing.Point(625, 27);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(381, 97);
        this.groupBox2.TabIndex = 41;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Global variables and functions";
        this.MacroCode_Textbox.Enabled = false;
        this.MacroCode_Textbox.Location = new System.Drawing.Point(6, 19);
        this.MacroCode_Textbox.Multiline = true;
        this.MacroCode_Textbox.Name = "MacroCode_Textbox";
        this.MacroCode_Textbox.ReadOnly = true;
        this.MacroCode_Textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.MacroCode_Textbox.Size = new System.Drawing.Size(369, 72);
        this.MacroCode_Textbox.TabIndex = 8;
        this.MacroCode_Textbox.TabStop = false;
        this.MacroCode_Textbox.Text = "/*This code will be accessible from all State Machines.*/";
        this.MacroCode_Textbox.TextChanged += new System.EventHandler(MacroCode_Textbox_TextChanged);
        this.MacroCode_Textbox.Enter += new System.EventHandler(MacroCode_Textbox_Enter);
        this.MacroCode_Textbox.Leave += new System.EventHandler(MacroCode_Textbox_Leave);
        this.projectnamebox.Location = new System.Drawing.Point(83, 96);
        this.projectnamebox.Name = "projectnamebox";
        this.projectnamebox.Size = new System.Drawing.Size(119, 20);
        this.projectnamebox.TabIndex = 5;
        this.projectnamebox.Text = "My Project";
        this.projectnamebox.Enter += new System.EventHandler(projectnamebox_Enter);
        this.projectnamebox.Leave += new System.EventHandler(projectnamebox_Leave);
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(3, 100);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(72, 13);
        this.label4.TabIndex = 48;
        this.label4.Text = "Project name:";
        this.globaluartcheckbox.AutoSize = true;
        this.globaluartcheckbox.Location = new System.Drawing.Point(425, 96);
        this.globaluartcheckbox.Name = "globaluartcheckbox";
        this.globaluartcheckbox.Size = new System.Drawing.Size(92, 17);
        this.globaluartcheckbox.TabIndex = 7;
        this.globaluartcheckbox.Text = "Enable UART";
        this.globaluartcheckbox.UseVisualStyleBackColor = true;
        this.tabimages.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("tabimages.ImageStream");
        this.tabimages.TransparentColor = System.Drawing.Color.Yellow;
        this.tabimages.Images.SetKeyName(0, "tab_nosel.bmp");
        this.tabimages.Images.SetKeyName(1, "tab_sel.bmp");
        this.tabimages.Images.SetKeyName(2, "plusright.bmp");
        this.StateMachineGroupBox.Controls.Add(this.groupBox1);
        this.StateMachineGroupBox.Controls.Add(this.label13);
        this.StateMachineGroupBox.Controls.Add(this.StateMachineName_Textbox);
        this.StateMachineGroupBox.Controls.Add(this.StateMachinePeriod_Textbox);
        this.StateMachineGroupBox.Controls.Add(this.StateMachinePrefix_Textbox);
        this.StateMachineGroupBox.Controls.Add(this.label1);
        this.StateMachineGroupBox.Controls.Add(this.label2);
        this.StateMachineGroupBox.Controls.Add(this.label3);
        this.StateMachineGroupBox.Controls.Add(this.label5);
        this.StateMachineGroupBox.Location = new System.Drawing.Point(6, 164);
        this.StateMachineGroupBox.Name = "StateMachineGroupBox";
        this.StateMachineGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.StateMachineGroupBox.Size = new System.Drawing.Size(721, 106);
        this.StateMachineGroupBox.TabIndex = 36;
        this.StateMachineGroupBox.TabStop = false;
        this.StateMachineGroupBox.Text = "State machine";
        this.groupBox1.Controls.Add(this.LocalCode_TextBox);
        this.groupBox1.Location = new System.Drawing.Point(223, 7);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(495, 94);
        this.groupBox1.TabIndex = 40;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Variables and functions";
        this.LocalCode_TextBox.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.LocalCode_TextBox.Location = new System.Drawing.Point(4, 14);
        this.LocalCode_TextBox.Multiline = true;
        this.LocalCode_TextBox.Name = "LocalCode_TextBox";
        this.LocalCode_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.LocalCode_TextBox.Size = new System.Drawing.Size(485, 77);
        this.LocalCode_TextBox.TabIndex = 13;
        this.LocalCode_TextBox.Text = "/*Define user variables for this state machine here. No functions; make them global.*/";
        this.LocalCode_TextBox.Enter += new System.EventHandler(LocalCode_TextBox_Enter);
        this.LocalCode_TextBox.Leave += new System.EventHandler(LocalCode_TextBox_Leave);
        this.label13.AutoSize = true;
        this.label13.Location = new System.Drawing.Point(138, 50);
        this.label13.Name = "label13";
        this.label13.Size = new System.Drawing.Size(79, 26);
        this.label13.TabIndex = 18;
        this.label13.Text = "  Inputs: A7-A0\r\nOutputs: B7-B0";
        this.StateMachineName_Textbox.Location = new System.Drawing.Point(47, 22);
        this.StateMachineName_Textbox.Name = "StateMachineName_Textbox";
        this.StateMachineName_Textbox.Size = new System.Drawing.Size(119, 20);
        this.StateMachineName_Textbox.TabIndex = 10;
        this.StateMachineName_Textbox.Text = "State Machine 1";
        this.StateMachineName_Textbox.Enter += new System.EventHandler(StateMachineName_Textbox_Enter);
        this.StateMachineName_Textbox.Leave += new System.EventHandler(StateMachineName_Textbox_Leave);
        this.StateMachinePeriod_Textbox.Location = new System.Drawing.Point(47, 76);
        this.StateMachinePeriod_Textbox.MaxLength = 6;
        this.StateMachinePeriod_Textbox.Name = "StateMachinePeriod_Textbox";
        this.StateMachinePeriod_Textbox.Size = new System.Drawing.Size(65, 20);
        this.StateMachinePeriod_Textbox.TabIndex = 12;
        this.StateMachinePeriod_Textbox.Text = "1000";
        this.StateMachinePeriod_Textbox.WordWrap = false;
        this.StateMachinePeriod_Textbox.Enter += new System.EventHandler(StateMachinePeriod_Textbox_Enter);
        this.StateMachinePeriod_Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(StateMachinePeriod_Textbox_KeyPress);
        this.StateMachinePeriod_Textbox.Leave += new System.EventHandler(StateMachinePeriod_Textbox_Leave);
        this.StateMachinePrefix_Textbox.Location = new System.Drawing.Point(47, 48);
        this.StateMachinePrefix_Textbox.Name = "StateMachinePrefix_Textbox";
        this.StateMachinePrefix_Textbox.Size = new System.Drawing.Size(65, 20);
        this.StateMachinePrefix_Textbox.TabIndex = 11;
        this.StateMachinePrefix_Textbox.Text = "SM1";
        this.StateMachinePrefix_Textbox.Enter += new System.EventHandler(StateMachinePrefix_Textbox_Enter);
        this.StateMachinePrefix_Textbox.Leave += new System.EventHandler(StateMachinePrefix_Textbox_Leave);
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(3, 25);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(38, 13);
        this.label1.TabIndex = 50;
        this.label1.Text = "Name:";
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(3, 79);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(40, 13);
        this.label2.TabIndex = 50;
        this.label2.Text = "Period:";
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(3, 51);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(36, 13);
        this.label3.TabIndex = 50;
        this.label3.Text = "Prefix:";
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(110, 79);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(20, 13);
        this.label5.TabIndex = 41;
        this.label5.Text = "ms";
        this.groupBox4.BackColor = System.Drawing.Color.Transparent;
        this.groupBox4.Controls.Add(this.backgroundpanel);
        this.groupBox4.Location = new System.Drawing.Point(6, 276);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.groupBox4.Size = new System.Drawing.Size(721, 463);
        this.groupBox4.TabIndex = 38;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "Canvas";
        this.backgroundpanel.BackColor = System.Drawing.SystemColors.Window;
        this.backgroundpanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.backgroundpanel.Controls.Add(this.vScrollBar1);
        this.backgroundpanel.Controls.Add(this.hScrollBar1);
        this.backgroundpanel.Location = new System.Drawing.Point(4, 20);
        this.backgroundpanel.Name = "backgroundpanel";
        this.backgroundpanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.backgroundpanel.Size = new System.Drawing.Size(711, 420);
        this.backgroundpanel.TabIndex = 19;
        this.backgroundpanel.Paint += new System.Windows.Forms.PaintEventHandler(backgroundpanel_Paint);
        this.backgroundpanel.MouseDown += new System.Windows.Forms.MouseEventHandler(backgroundpanel_MouseDown);
        this.backgroundpanel.MouseMove += new System.Windows.Forms.MouseEventHandler(backgroundpanel_MouseMove);
        this.backgroundpanel.MouseUp += new System.Windows.Forms.MouseEventHandler(backgroundpanel_MouseUp);
        this.vScrollBar1.LargeChange = 2;
        this.vScrollBar1.Location = new System.Drawing.Point(689, 1);
        this.vScrollBar1.Maximum = 1;
        this.vScrollBar1.Name = "vScrollBar1";
        this.vScrollBar1.Size = new System.Drawing.Size(20, 423);
        this.vScrollBar1.SmallChange = 2;
        this.vScrollBar1.TabIndex = 1;
        this.vScrollBar1.Visible = false;
        this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(vScrollBar1_Scroll);
        this.hScrollBar1.LargeChange = 2;
        this.hScrollBar1.Location = new System.Drawing.Point(1, 425);
        this.hScrollBar1.Maximum = 1;
        this.hScrollBar1.Name = "hScrollBar1";
        this.hScrollBar1.Size = new System.Drawing.Size(707, 20);
        this.hScrollBar1.SmallChange = 2;
        this.hScrollBar1.TabIndex = 0;
        this.hScrollBar1.Visible = false;
        this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(hScrollBar1_Scroll);
        this.ObjectGroupBox.Controls.Add(this.ObjectInitialState_Checkbox);
        this.ObjectGroupBox.Controls.Add(this.ObjectActions_Textbox);
        this.ObjectGroupBox.Controls.Add(this.ObjectDelete_Button);
        this.ObjectGroupBox.Controls.Add(this.ObjectCondition_Textbox);
        this.ObjectGroupBox.Controls.Add(this.ObjectName_Textbox);
        this.ObjectGroupBox.Controls.Add(this.label6);
        this.ObjectGroupBox.Controls.Add(this.label8);
        this.ObjectGroupBox.Controls.Add(this.label7);
        this.ObjectGroupBox.Location = new System.Drawing.Point(733, 164);
        this.ObjectGroupBox.Name = "ObjectGroupBox";
        this.ObjectGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.ObjectGroupBox.Size = new System.Drawing.Size(263, 566);
        this.ObjectGroupBox.TabIndex = 37;
        this.ObjectGroupBox.TabStop = false;
        this.ObjectGroupBox.Text = "Object";
        this.ObjectInitialState_Checkbox.AutoSize = true;
        this.ObjectInitialState_Checkbox.Enabled = false;
        this.ObjectInitialState_Checkbox.Location = new System.Drawing.Point(9, 47);
        this.ObjectInitialState_Checkbox.Name = "ObjectInitialState_Checkbox";
        this.ObjectInitialState_Checkbox.Size = new System.Drawing.Size(76, 17);
        this.ObjectInitialState_Checkbox.TabIndex = 15;
        this.ObjectInitialState_Checkbox.Text = "Initial state";
        this.ObjectInitialState_Checkbox.UseVisualStyleBackColor = true;
        this.ObjectInitialState_Checkbox.Click += new System.EventHandler(ObjectInitialState_Checkbox_Click);
        this.ObjectActions_Textbox.Enabled = false;
        this.ObjectActions_Textbox.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.ObjectActions_Textbox.Location = new System.Drawing.Point(6, 87);
        this.ObjectActions_Textbox.Multiline = true;
        this.ObjectActions_Textbox.Name = "ObjectActions_Textbox";
        this.ObjectActions_Textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.ObjectActions_Textbox.Size = new System.Drawing.Size(251, 233);
        this.ObjectActions_Textbox.TabIndex = 17;
        this.ObjectActions_Textbox.Text = "//Actions must be valid C code";
        this.ObjectActions_Textbox.TextChanged += new System.EventHandler(ObjectActions_Textbox_TextChanged);
        this.ObjectActions_Textbox.Enter += new System.EventHandler(ObjectActions_Textbox_Enter);
        this.ObjectActions_Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ObjectActions_Textbox_KeyPress);
        this.ObjectActions_Textbox.Leave += new System.EventHandler(ObjectActions_Textbox_Leave);
        this.ObjectDelete_Button.BackColor = System.Drawing.Color.MistyRose;
        this.ObjectDelete_Button.Enabled = false;
        this.ObjectDelete_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.ObjectDelete_Button.Location = new System.Drawing.Point(152, 47);
        this.ObjectDelete_Button.Name = "ObjectDelete_Button";
        this.ObjectDelete_Button.Size = new System.Drawing.Size(105, 34);
        this.ObjectDelete_Button.TabIndex = 16;
        this.ObjectDelete_Button.Text = "Delete";
        this.ObjectDelete_Button.UseVisualStyleBackColor = false;
        this.ObjectDelete_Button.Click += new System.EventHandler(ObjectDelete_Button_Click);
        this.ObjectCondition_Textbox.Enabled = false;
        this.ObjectCondition_Textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.ObjectCondition_Textbox.Location = new System.Drawing.Point(6, 339);
        this.ObjectCondition_Textbox.Multiline = true;
        this.ObjectCondition_Textbox.Name = "ObjectCondition_Textbox";
        this.ObjectCondition_Textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.ObjectCondition_Textbox.Size = new System.Drawing.Size(251, 221);
        this.ObjectCondition_Textbox.TabIndex = 18;
        this.ObjectCondition_Textbox.Text = "/*Edge conditions must be valid C code that fits inside the parenthesis\r\nof an \"if (  )\" expression*/";
        this.ObjectCondition_Textbox.TextChanged += new System.EventHandler(ObjectCondition_Textbox_TextChanged);
        this.ObjectCondition_Textbox.Enter += new System.EventHandler(ObjectCondition_Textbox_Enter);
        this.ObjectCondition_Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ObjectCondition_Textbox_KeyPress);
        this.ObjectCondition_Textbox.Leave += new System.EventHandler(ObjectCondition_Textbox_Leave);
        this.ObjectName_Textbox.Enabled = false;
        this.ObjectName_Textbox.Location = new System.Drawing.Point(47, 19);
        this.ObjectName_Textbox.Name = "ObjectName_Textbox";
        this.ObjectName_Textbox.Size = new System.Drawing.Size(210, 20);
        this.ObjectName_Textbox.TabIndex = 14;
        this.ObjectName_Textbox.TextChanged += new System.EventHandler(ObjectName_Textbox_TextChanged);
        this.ObjectName_Textbox.Enter += new System.EventHandler(ObjectName_Textbox_Enter);
        this.ObjectName_Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ObjectName_Textbox_KeyPress);
        this.ObjectName_Textbox.Leave += new System.EventHandler(ObjectName_Textbox_Leave);
        this.label6.AutoSize = true;
        this.label6.Location = new System.Drawing.Point(6, 71);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(42, 13);
        this.label6.TabIndex = 20;
        this.label6.Text = "Actions";
        this.label8.AutoSize = true;
        this.label8.Location = new System.Drawing.Point(6, 22);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(35, 13);
        this.label8.TabIndex = 23;
        this.label8.Text = "Name";
        this.label7.AutoSize = true;
        this.label7.Location = new System.Drawing.Point(3, 323);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(51, 13);
        this.label7.TabIndex = 21;
        this.label7.Text = "Condition";
        this.pulsetimer.Interval = 5;
        this.pulsetimer.Tick += new System.EventHandler(pulsetimer_Tick);
        this.label14.AutoSize = true;
        this.label14.Location = new System.Drawing.Point(222, 100);
        this.label14.Name = "label14";
        this.label14.Size = new System.Drawing.Size(34, 13);
        this.label14.TabIndex = 54;
        this.label14.Text = "Type:";
        this.smType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.smType.Items.AddRange(new object[2] { "Synchronous SM", "Asynchronous SM" });
        this.smType.Location = new System.Drawing.Point(262, 97);
        this.smType.MaxDropDownItems = 2;
        this.smType.Name = "smType";
        this.smType.Size = new System.Drawing.Size(121, 21);
        this.smType.TabIndex = 6;
        this.NodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.basic, this.ifelse, this.forloop });
        this.NodeContextMenu.Name = "NodeContextMenu";
        this.NodeContextMenu.Size = new System.Drawing.Size(179, 70);
        this.NodeContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(NodeContextMenu_Closed);
        this.NodeContextMenu.Opened += new System.EventHandler(NodeContextMenu_Opened);
        this.NodeContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(NodeContextMenu_ItemClicked);
        this.basic.Checked = true;
        this.basic.CheckOnClick = true;
        this.basic.CheckState = System.Windows.Forms.CheckState.Checked;
        this.basic.Name = "basic";
        this.basic.Size = new System.Drawing.Size(178, 22);
        this.basic.Text = "Basic transitions";
        this.ifelse.CheckOnClick = true;
        this.ifelse.Name = "ifelse";
        this.ifelse.Size = new System.Drawing.Size(178, 22);
        this.ifelse.Text = "If-else transitions";
        this.forloop.CheckOnClick = true;
        this.forloop.Name = "forloop";
        this.forloop.Size = new System.Drawing.Size(178, 22);
        this.forloop.Text = "For-loop transitions";
        this.tabs.AllowDrop = true;
        this.tabs.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.tabs.Controls.Add(this.plustab);
        this.tabs.ImageList = this.tabimages;
        this.tabs.ItemSize = new System.Drawing.Size(100, 20);
        this.tabs.Location = new System.Drawing.Point(6, 134);
        this.tabs.Name = "tabs";
        this.tabs.Padding = new System.Drawing.Point(3, 3);
        this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.tabs.SelectedIndex = 0;
        this.tabs.Size = new System.Drawing.Size(1000, 26);
        this.tabs.TabIndex = 9;
        this.plustab.AllowDrop = true;
        this.plustab.BackColor = System.Drawing.Color.NavajoWhite;
        this.plustab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.plustab.ImageKey = "plusright.bmp";
        this.plustab.Location = new System.Drawing.Point(4, 24);
        this.plustab.Name = "plustab";
        this.plustab.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.plustab.Size = new System.Drawing.Size(992, 0);
        this.plustab.TabIndex = 1;
        this.plustab.UseVisualStyleBackColor = true;
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 18f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //base.ClientSize = new System.Drawing.Size(1008, 700);
        //this.MinimumSize = new Size(1000, 700);
        //this.MaximumSize = new Size(1000, 700);
        this.Size = new Size(1000, 800);
        base.Controls.Add(this.label14);
        base.Controls.Add(this.smType);
        base.Controls.Add(this.label4);
        base.Controls.Add(this.ObjectGroupBox);
        base.Controls.Add(this.groupBox2);
        base.Controls.Add(this.StateMachineGroupBox);
        base.Controls.Add(this.projectnamebox);
        base.Controls.Add(this.groupBox4);
        base.Controls.Add(this.ControlsGroupBox);
        base.Controls.Add(this.tabs);
        base.Controls.Add(this.menuStrip1);
        base.Controls.Add(this.globaluartcheckbox);
        this.DoubleBuffered = true;
        base.Icon = (System.Drawing.Icon)resources.GetObject("MainIcon");
        base.MainMenuStrip = this.menuStrip1;
        base.MaximizeBox = false;
        base.Name = "RIBS";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Riverside-Irvine Builder: Statemachines";
        base.Activated += new System.EventHandler(MainForm_Activated);
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RIBS_FormClosing);
        base.Paint += new System.Windows.Forms.PaintEventHandler(MainForm_Paint);
        base.MouseMove += new System.Windows.Forms.MouseEventHandler(RIBS_MouseMove);
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ControlsGroupBox.ResumeLayout(false);
        this.ControlsGroupBox.PerformLayout();
        this.EdgeContextMenu.ResumeLayout(false);
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.StateMachineGroupBox.ResumeLayout(false);
        this.StateMachineGroupBox.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox4.ResumeLayout(false);
        this.backgroundpanel.ResumeLayout(false);
        this.ObjectGroupBox.ResumeLayout(false);
        this.ObjectGroupBox.PerformLayout();
        this.NodeContextMenu.ResumeLayout(false);
        this.tabs.ResumeLayout(false);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public RIBS(string[] args)
    {
        InitializeComponent();
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
        tabs.TabClosed += tabclosed;
        tabs.AddTab += addtab;
        tabs.ChangeTab += changetab;
        view_mode = 0;
        graphicmanager = BufferedGraphicsManager.Current;
        graphicmanager.MaximumBuffer = new Size(backgroundpanel.Width * 3, backgroundpanel.Height * 3);
        Rectangle targetRectangle = new Rectangle(backgroundpanel.Location, new Size(backgroundpanel.Width * 3, backgroundpanel.Height * 3));
        graphic = graphicmanager.Allocate(backgroundpanel.CreateGraphics(), targetRectangle);
        graphic.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphic.Graphics.FillRectangle(Brushes.White, backgroundpanel.ClientRectangle.X, backgroundpanel.ClientRectangle.Y, backgroundpanel.ClientRectangle.Width * 2, backgroundpanel.ClientRectangle.Height * 2);
        about = new About();
        warning = new warning();
        feedback = new Feedback();
        simulator = new Process();
        emptystatemachinewarning = new EmptyStateMachineWarning();
        abbreviationrepeat = new abbreviationrepeat();
        ReservedNames = new ReservedNames();
        documents_directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\RIMS\\";
        if (!Directory.Exists(documents_directory))
        {
            Directory.CreateDirectory(documents_directory);
        }
        default_path = documents_directory + "statemachine.sm";
        executable_directory = Path.GetDirectoryName(Application.ExecutablePath);
        is_running = false;
        x_offset = 55;
        y_offset = 55;
        oldx = 0;
        oldy = 0;
        mode = Mode.Idle;
        scalefactor = 1f;
        tooltips_enabled = true;
        InsertTransitionTooltip.SetToolTip(InsertTransition_Button, "To insert a transition first click the source state, then the destination state.");
        EditTransitionToolTip.SetToolTip(backgroundpanel, "You can edit a transition curve by left clicking it,\r\nor right click to convert to a straight line.");
        StateActionTooltip.SetToolTip(ObjectActions_Textbox, "Actions must be valid C statements");
        TransitionConditionTooltip.SetToolTip(ObjectCondition_Textbox, "The transition condition must be a valid C expression that must fit into the parentheses of an \"if ( )\" statement");
        EnableUartTooltip.SetToolTip(globaluartcheckbox, "Enabling the UART allows for the use of the serial communication peripheral.\r\nUse the variable 'T' to set a character to transmit.\r\nUse the variable 'R' to read a received character.\r\n'TxReady' and 'RxComplete' are flags that can be read to control the UART.");
        if (args.Length != 0)
        {
            OpenProject(args[0], def: false);
        }
        else
        {
            newToolStripMenuItem_Click(null, null);
        }
        is_saved = true;
        StateMachineName_Textbox.Text = graph[current_graph].Name;
        StateMachinePeriod_Textbox.Text = graph[current_graph].Period;
        StateMachinePrefix_Textbox.Text = graph[current_graph].Abbrv;
        smType.SelectedIndex = ((!graph[current_graph].EnableTimer) ? 1 : 0);
        globaluartcheckbox.Checked = graph[current_graph].EnableUart;
        LocalCode_TextBox.Text = graph[current_graph].GlobalCode;
        InsertTransition_Button.Enabled = graph[current_graph].nodes.Count > 0;
        GenerateC_Button.Enabled = graph[current_graph].nodes.Count > 0;
        SimulateStateMachine_Button.Enabled = false;
        MacroCode_Textbox.Enabled = graph.Count > 1;
        MacroCode_Textbox.ReadOnly = graph.Count <= 1;
        groupBox1.Text = "Variables and functions";
        StateMachineName_Textbox.TextChanged += StateMachineName_Textbox_TextChanged;
        StateMachinePeriod_Textbox.TextChanged += StateMachinePeriod_Textbox_TextChanged;
        StateMachinePrefix_Textbox.TextChanged += StateMachinePrefix_Textbox_TextChanged;
        smType.SelectionChangeCommitted += smType_SelectionChangeCommitted;
        globaluartcheckbox.Click += globaluartcheckbox_Click;
        LocalCode_TextBox.TextChanged += GlobalCode_Textbox_TextChanged;
        pickPort();
    }

    private void pickPort()
    {
        rimsSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        bool flag = true;
        while (flag)
        {
            rimsanimationPort = next_socket;
            try
            {
                IPEndPoint localEP = new IPEndPoint(IPAddress.Loopback, rimsanimationPort);
                rimsSocket.Bind(localEP);
                rimsSocket.Listen(2);
                flag = false;
                next_socket++;
            }
            catch (Exception)
            {
                next_socket++;
            }
        }
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
        SuperTabControlEventArgs superTabControlEventArgs = new SuperTabControlEventArgs(null, 0, TabControlAction.Selecting);
        switch (keyData)
        {
            case Keys.F4 | Keys.Control:
                superTabControlEventArgs.closed_index = current_graph;
                tabclosed(null, superTabControlEventArgs);
                return true;
            case Keys.T | Keys.Control:
                addtab(null, superTabControlEventArgs);
                return true;
            case Keys.E | Keys.Control:
                InsertState_Button_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                return true;
            case Keys.S | Keys.Control:
                saveToolStripMenuItem_Click(null, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                return true;
            case Keys.R | Keys.Control:
                if (graph[current_graph].nodes.Count > 0)
                {
                    InsertTransition_Button_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                }
                return true;
            case Keys.Delete:
                if (selected_node != null || selected_edge != null)
                {
                    ObjectDelete_Button_Click(null, null);
                }
                return true;
            default:
                return base.ProcessDialogKey(keyData);
        }
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
        try
        {
            lock (graphic)
            {
                if (current_graph < graph.Count)
                {
                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                    graphic.Render();
                }
            }
        }
        catch (Exception)
        {
        }
    }

    public void MainForm_PaintCallback()
    {
        MainForm_Paint(this, null);
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        lock (graphic)
        {
            graphic.Render();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        lock (graphic)
        {
            base.OnPaintBackground(e);
        }
    }

    private void RIBS_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.WindowsShutDown)
        {
            return;
        }
        if (!is_saved)
        {
            switch (MessageBox.Show("The project has been edited since the last save.  Exiting now will cause all changes to be lost.\r\nSave now?", "Warning:", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    SaveSM();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
            return;
        }
        try
        {
            if (!simulator.HasExited)
            {
                simulator.Kill();
            }
        }
        catch (Exception)
        {
        }
    }

    private void backgroundpanel_Paint(object sender, PaintEventArgs e)
    {
        lock (graphic)
        {
            OnPaint(e);
        }
    }

    private void backgroundpanel_MouseDown(object sender, MouseEventArgs e)
    {
        Point point = TranslateMouse(e.Location);
        int result = 0;
        if (e.Button == MouseButtons.Left)
        {
            switch (mode)
            {
                case Mode.Idle:
                    switch (graph[current_graph].IsOverObject(point.X, point.Y))
                    {
                        case 0:
                            if (selected_node != null)
                            {
                                selected_node.SetIsSelected(b: false);
                                selected_node = null;
                            }
                            if (selected_edge != null)
                            {
                                if (selected_edge.is_selected)
                                {
                                    selected_edge.is_selected = false;
                                }
                                selected_edge = null;
                            }
                            lock (graphic)
                            {
                                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                                graphic.Render();
                            }
                            DisableObjectDialog();
                            mode = Mode.Scroll;
                            break;
                        case 1:
                            if (selected_node != null)
                            {
                                selected_node.SetIsSelected(b: false);
                                selected_node = null;
                            }
                            selected_node = graph[current_graph].IsNode(point.X, point.Y);
                            selected_node.SetIsSelected(b: true);
                            click_offset = new Point(selected_node.CenterRect.X - point.X, selected_node.CenterRect.Y - point.Y);
                            if (selected_edge != null)
                            {
                                selected_edge.is_selected = false;
                                selected_edge = null;
                            }
                            LoadObjectDialog();
                            lock (graphic)
                            {
                                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                                graphic.Render();
                            }
                            mode = Mode.MovingNode;
                            break;
                        case 2:
                            selected_handle = 1;
                            mode = Mode.MovingHandle;
                            break;
                        case 3:
                            selected_handle = 2;
                            mode = Mode.MovingHandle;
                            break;
                        case 4:
                            if (selected_node != null)
                            {
                                selected_node.SetIsSelected(b: false);
                                selected_node = null;
                            }
                            if (selected_edge != null)
                            {
                                selected_edge.is_selected = false;
                            }
                            selected_edge = graph[current_graph].IsEdge(point.X, point.Y);
                            selected_edge.is_selected = true;
                            LoadObjectDialog();
                            lock (graphic)
                            {
                                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                                graphic.Render();
                                break;
                            }
                    }
                    break;
                case Mode.AddEdge:
                    graph[current_graph].SetNewEdge(ref result, ref newedge, point.X, point.Y);
                    switch (result)
                    {
                        case -1:
                            {
                                graph[current_graph].edges.Remove(newedge);
                                newedge.Delete();
                                if (selected_node != null)
                                {
                                    selected_node.SetIsSelected(b: false);
                                    selected_node = null;
                                }
                                if (selected_edge != null)
                                {
                                    selected_edge.is_selected = false;
                                }
                                DisableObjectDialog();
                                menuStrip1.Enabled = true;
                                InsertState_Button.Enabled = true;
                                InsertTransition_Button.Enabled = true;
                                tabs.Enabled = true;
                                bool enabled2 = true;
                                foreach (Graph item in graph)
                                {
                                    if (item.nodes.Count == 0)
                                    {
                                        enabled2 = false;
                                    }
                                }
                                GenerateC_Button.Enabled = enabled2;
                                mode = Mode.Idle;
                                break;
                            }
                        case 2:
                            {
                                if (selected_node != null)
                                {
                                    selected_node.SetIsSelected(b: false);
                                    selected_node = null;
                                }
                                if (selected_edge != null)
                                {
                                    selected_edge.is_selected = false;
                                }
                                selected_edge = newedge;
                                selected_edge.is_selected = true;
                                LoadObjectDialog();
                                is_saved = false;
                                while (selected_edge.arc.h1.Y < backgroundpanel.Location.Y + 10)
                                {
                                    selected_edge.arc.h1.Y += 5;
                                    selected_edge.arc.h2.Y += 5;
                                }
                                menuStrip1.Enabled = true;
                                InsertState_Button.Enabled = true;
                                InsertTransition_Button.Enabled = true;
                                tabs.Enabled = true;
                                bool enabled = true;
                                foreach (Graph item2 in graph)
                                {
                                    if (item2.nodes.Count == 0)
                                    {
                                        enabled = false;
                                    }
                                }
                                GenerateC_Button.Enabled = enabled;
                                mode = Mode.Idle;
                                lock (graphic)
                                {
                                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                                    graphic.Render();
                                    break;
                                }
                            }
                        case 0:
                        case 1:
                            break;
                    }
                    break;
                case Mode.MovingNode:
                case Mode.MovingHandle:
                case Mode.Simulation:
                    break;
            }
        }
        else
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            switch (mode)
            {
                case Mode.Idle:
                    switch (graph[current_graph].IsOverObject(point.X, point.Y))
                    {
                        case 1:
                            if (selected_node != null)
                            {
                                selected_node.SetIsSelected(b: false);
                            }
                            selected_node = graph[current_graph].IsNode(point.X, point.Y);
                            selected_node.SetIsSelected(b: true);
                            if (selected_edge != null)
                            {
                                selected_edge.is_selected = false;
                                selected_edge = null;
                            }
                            LoadObjectDialog();
                            break;
                        case 4:
                            if (selected_edge != null)
                            {
                                selected_edge.is_selected = false;
                            }
                            selected_edge = graph[current_graph].IsEdge(point.X, point.Y);
                            selected_edge.is_selected = true;
                            if (selected_node != null)
                            {
                                selected_node.SetIsSelected(b: false);
                                selected_node = null;
                            }
                            LoadObjectDialog();
                            EdgeContextMenu.Show(backgroundpanel, point.X, point.Y);
                            break;
                        case 0:
                        case 2:
                        case 3:
                            break;
                    }
                    break;
            }
        }
    }

    private void backgroundpanel_MouseMove(object sender, MouseEventArgs e)
    {
        Point point = TranslateMouse(e.Location);
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        switch (mode)
        {
            case Mode.MovingNode:
                if (point.X <= backgroundpanel.Left + backgroundpanel.Width + 180 && point.X >= backgroundpanel.Left && point.Y <= backgroundpanel.Top + backgroundpanel.Height + 180 - 27 && point.Y >= backgroundpanel.Top)
                {
                    num = point.X - 27 + click_offset.X;
                    num2 = point.Y - 27 + click_offset.Y;
                    graph[current_graph].UpdateNodePos(selected_node, num, num2);
                    lock (graphic)
                    {
                        graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                        graphic.Render();
                    }
                    is_saved = false;
                }
                break;
            case Mode.MovingHandle:
                if (selected_edge == null)
                {
                    return;
                }
                num = ((point.X < backgroundpanel.Left) ? (backgroundpanel.Left + 5) : point.X);
                num = ((point.X > backgroundpanel.Right * 2) ? (backgroundpanel.Right - 5) : num);
                num2 = ((point.Y > backgroundpanel.Bottom * 2) ? (backgroundpanel.Bottom - 15) : point.Y);
                num2 = ((point.Y < backgroundpanel.Top) ? backgroundpanel.Top : num2);
                switch (selected_handle)
                {
                    case 1:
                        selected_edge.arc.h1.X = num;
                        selected_edge.arc.h1.Y = num2;
                        break;
                    case 2:
                        selected_edge.arc.h2.X = num;
                        selected_edge.arc.h2.Y = num2;
                        break;
                }
                lock (graphic)
                {
                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                    graphic.Render();
                }
                is_saved = false;
                break;
            case Mode.Scroll:
                num = e.X - oldx;
                num2 = e.Y - oldy;
                ScrollCanvas(num, num2, fromcontrol: false);
                lock (graphic)
                {
                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                    graphic.Render();
                }
                break;
        }
        num3 = graph[current_graph].IsOverObject(point.X, point.Y);
        switch ((mode == Mode.Scroll) ? 5 : num3)
        {
            case 0:
                EditTransitionToolTip.Active = false;
                Cursor = Cursors.Arrow;
                break;
            case 2:
            case 3:
                if (selected_edge != null)
                {
                    EditTransitionToolTip.Active = false;
                    Cursor = Cursors.Hand;
                }
                break;
            case 4:
                if (tooltips_enabled)
                {
                    EditTransitionToolTip.Active = true;
                }
                Cursor = Cursors.Hand;
                break;
            case 5:
                Cursor = Cursors.Hand;
                break;
            default:
                Cursor = Cursors.Hand;
                break;
        }
        oldx = e.X;
        oldy = e.Y;
    }

    private void ScrollCanvas(int dx, int dy, bool fromcontrol)
    {
        bool flag = false;
        if (origx + scroll_offset + dx > -400 && origx + scroll_offset + dx <= 0)
        {
            lock (graphic)
            {
                graphic.Graphics.TranslateTransform(dx, 0f);
            }
            graph[current_graph].Shift(dx, 0);
            origx += dx;
            flag = true;
        }
        if (origy + scroll_offset + dy > -400 && origy + scroll_offset + dy <= 0)
        {
            lock (graphic)
            {
                graphic.Graphics.TranslateTransform(0f, dy);
            }
            graph[current_graph].Shift(0, dy);
            origy += dy;
            flag = true;
        }
        if (!flag)
        {
            return;
        }
        int num = 0;
        int num2 = 0;
        foreach (Node node in graph[current_graph].nodes)
        {
            int num3 = node.CenterRect.X;
            int num4 = node.CenterRect.Y;
            num = ((num3 > num) ? num3 : num);
            num2 = ((num4 > num2) ? num4 : num2);
        }
        num -= backgroundpanel.Width;
        num2 -= backgroundpanel.Height;
        if (!fromcontrol)
        {
            hScrollBar1.Maximum = (((origx + scroll_offset) * -1 > 400) ? ((origx + scroll_offset) * -1) : 400);
            vScrollBar1.Maximum = (((origy + scroll_offset) * -1 > 400) ? ((origy + scroll_offset) * -1) : 400);
            hScrollBar1.Value = (origx + scroll_offset) * -1;
            vScrollBar1.Value = (origy + scroll_offset) * -1;
        }
        hScrollBar1.Visible = hScrollBar1.Maximum > 0;
        vScrollBar1.Visible = vScrollBar1.Maximum > 0;
        hScrollBar1.LargeChange = hScrollBar1.Maximum / 4;
        hScrollBar1.SmallChange = hScrollBar1.Maximum / 10;
        vScrollBar1.LargeChange = vScrollBar1.Maximum / 4;
        vScrollBar1.SmallChange = vScrollBar1.Maximum / 10;
    }

    private void backgroundpanel_MouseUp(object sender, MouseEventArgs e)
    {
        _ = e.X;
        _ = e.Y;
        switch (mode)
        {
            case Mode.MovingNode:
                ScrollCanvas(0, 0, fromcontrol: false);
                mode = Mode.Idle;
                break;
            case Mode.MovingHandle:
                mode = Mode.Idle;
                break;
            case Mode.Scroll:
                mode = Mode.Idle;
                break;
            case Mode.Idle:
            case Mode.AddEdge:
            case Mode.Simulation:
                break;
        }
    }

    private void RIBS_MouseMove(object sender, MouseEventArgs e)
    {
        if (!tooltips_enabled)
        {
            EditTransitionToolTip.Active = false;
            InsertTransitionTooltip.Active = false;
            EnableTimerTooltip.Active = false;
            EnableUartTooltip.Active = false;
            StateActionTooltip.Active = false;
            TransitionConditionTooltip.Active = false;
        }
        else
        {
            EditTransitionToolTip.Active = true;
            InsertTransitionTooltip.Active = true;
            EnableTimerTooltip.Active = true;
            EnableUartTooltip.Active = true;
            StateActionTooltip.Active = true;
            TransitionConditionTooltip.Active = true;
        }
    }

    private Point TranslateMouse(Point mouse)
    {
        Point[] array = new Point[1]
        {
            new Point(mouse.X, mouse.Y)
        };
        Matrix matrix = new Matrix();
        matrix.Translate(-origx, -origy);
        matrix.TransformPoints(array);
        return array[0];
    }

    private void InsertState_Button_MouseClick(object sender, MouseEventArgs e)
    {
        if (mode == Mode.Idle)
        {
            if (selected_node != null)
            {
                selected_node.SetIsSelected(b: false);
                selected_node = null;
            }
            selected_node = graph[current_graph].AddNode(backgroundpanel.Left + x_offset, backgroundpanel.Top + y_offset);
            selected_node.SetIsSelected(b: true);
            Point p = new Point(selected_node.CenterRect.X + backgroundpanel.Left, selected_node.CenterRect.Y + backgroundpanel.Top);
            Cursor.Position.Offset(p);
            if (selected_edge != null)
            {
                selected_edge.is_selected = false;
                selected_edge = null;
            }
            LoadObjectDialog();
            if (x_offset < backgroundpanel.Width - 110 - 30)
            {
                x_offset += 110;
            }
            else
            {
                x_offset = 55;
                y_offset += 110;
            }
            if (y_offset >= backgroundpanel.Height - 110 - 30)
            {
                y_offset = 55;
            }
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
            graph[current_graph].x_offset = x_offset;
            graph[current_graph].y_offset = y_offset;
            is_saved = false;
            SimulateStateMachine_Button.Enabled = false;
        }
        bool enabled = true;
        foreach (Graph item in graph)
        {
            if (item.nodes.Count == 0)
            {
                enabled = false;
            }
        }
        GenerateC_Button.Enabled = enabled;
        InsertTransition_Button.Enabled = true;
    }

    private void InsertTransition_Button_MouseClick(object sender, MouseEventArgs e)
    {
        if (graph[current_graph].nodes.Count <= 1)
        {
            MessageBox.Show("Please create a state first.", "Warning:", MessageBoxButtons.OK);
            return;
        }
        switch (mode)
        {
            case Mode.Idle:
                if (selected_edge != null)
                {
                    selected_edge.is_selected = false;
                }
                if (selected_node != null)
                {
                    selected_node.SetIsSelected(b: false);
                }
                newedge = graph[current_graph].AddEdge();
                lock (graphic)
                {
                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                    graphic.Render();
                }
                mode = Mode.AddEdge;
                break;
            case Mode.AddEdge:
                if (newedge.GetT() != null)
                {
                    newedge.GetT().edges.Remove(newedge);
                    newedge.SetT(null);
                }
                break;
        }
        menuStrip1.Enabled = false;
        InsertState_Button.Enabled = false;
        GenerateC_Button.Enabled = false;
        SimulateStateMachine_Button.Enabled = false;
        tabs.Enabled = false;
    }

    private void GenerateC_Button_MouseClick(object sender, MouseEventArgs e)
    {
        foreach (Graph item in graph)
        {
            if (item.nodes.Count == 0)
            {
                DialogResult dialogResult = MessageBox.Show("You have an empty state machine. All your state machines must have at least one state.", "Error", MessageBoxButtons.OK);
                DialogResult dialogResult2 = dialogResult;
                if (dialogResult2 != DialogResult.OK)
                {
                }
                return;
            }
        }
        if (filename == "" || !is_saved)
        {
            SaveSM();
        }
        foreach (Graph item2 in graph)
        {
            if (item2.nodes.Count == 0)
            {
                DialogResult dialogResult3 = MessageBox.Show("You have an empty state machine. All your state machines must have at least one state.", "Error", MessageBoxButtons.OK);
                DialogResult dialogResult4 = dialogResult3;
                if (dialogResult4 != DialogResult.OK)
                {
                }
                return;
            }
            if (item2.GetInitialStateName() == "")
            {
                MessageBox.Show("All state machines must have an initial state selected.");
                return;
            }
            foreach (Graph item3 in graph)
            {
                if (item2.Abbrv == item3.Abbrv && item2 != item3)
                {
                    DialogResult dialogResult5 = MessageBox.Show("At least two of your state machines have the same abbreviation. Each state machine must have a unique abbreviation.", "Error", MessageBoxButtons.OK);
                    DialogResult dialogResult6 = dialogResult5;
                    if (dialogResult6 != DialogResult.OK)
                    {
                    }
                    return;
                }
            }
        }
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.DefaultExt = "c";
        saveFileDialog.FileName = filename;
        saveFileDialog.InitialDirectory = filepath;
        saveFileDialog.OverwritePrompt = true;
        saveFileDialog.AddExtension = true;
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.Filter = "RIBS C Code (*.c)|*.c";
        DialogResult dialogResult7 = saveFileDialog.ShowDialog();
        if (graph.Count == 1)
        {
            switch (dialogResult7)
            {
                default:
                    return;
                case DialogResult.OK:
                    break;
            }
            Generator generator = new Generator(graph[current_graph]);
            StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
            streamWriter.Write(generator.GenerateCCode((smType.SelectedIndex == 0) ? true : false, globaluartcheckbox.Checked));
            streamWriter.Close();
            c_filename = saveFileDialog.FileName;
        }
        else
        {
            switch (dialogResult7)
            {
                default:
                    return;
                case DialogResult.OK:
                    break;
            }
            MultiGenerator multiGenerator = new MultiGenerator(MacroCode_Textbox.Text);
            StreamWriter streamWriter2 = new StreamWriter(saveFileDialog.FileName);
            streamWriter2.Write(multiGenerator.GenerateCCode(graph));
            streamWriter2.Close();
            c_filename = saveFileDialog.FileName;
        }
        SimulateStateMachine_Button.Enabled = true;
    }

    private void SimulateStateMachine_Button_Click(object sender, EventArgs e)
    {
        if (!is_running)
        {
            if (rimsSocket == null)
            {
                pickPort();
            }
            is_running = true;
            SimulateStateMachine_Button.Text = "End Simulation";
            simulator.StartInfo.FileName = executable_directory + "\\RIMS.exe";
            simulator.StartInfo.Arguments = "/sm_animation:\"" + c_filename + "\"";
            ProcessStartInfo startInfo = simulator.StartInfo;
            startInfo.Arguments = startInfo.Arguments + " /ribs_port:" + rimsanimationPort;
            simulator.StartInfo.Arguments += " /state_vars:";
            foreach (Graph item in graph)
            {
                if (item.Abbrv != "")
                {
                    ProcessStartInfo startInfo2 = simulator.StartInfo;
                    startInfo2.Arguments = startInfo2.Arguments + item.Abbrv + "_State,";
                }
            }
            if (File.Exists(simulator.StartInfo.FileName))
            {
                simulator.EnableRaisingEvents = true;
                simulator.Exited += RIMS_Exited;
                simulator.Start();
                simthread = new Thread(SimThread);
                simthread.IsBackground = true;
                simthread.Start();
                InsertState_Button.Enabled = false;
                InsertTransition_Button.Enabled = false;
                GenerateC_Button.Enabled = false;
                SimulateStateMachine_Button.Enabled = true;
                StateMachineGroupBox.Enabled = false;
                ObjectGroupBox.Enabled = false;
                disableTooltipsToolStripMenuItem1.Enabled = false;
                aboutToolStripMenuItem1.Enabled = false;
                reportsBugsFeedbackToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                openSampleToolStripMenuItem.Enabled = false;
                licenseKeyToolStripMenuItem.Enabled = false;
                insertStateMachineToolStripMenuItem.Enabled = false;
                closeStateMachineToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                tabs.Enabled = true;
                graph[current_graph].CleanColors();
                lock (graphic)
                {
                    graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                    graphic.Render();
                    return;
                }
            }
            MessageBox.Show("RIMS does not seem to exist. Verify that RIMS is located in the same directory as the RIBS executable");
        }
        else
        {
            is_running = false;
            simulator.Kill();
        }
    }

    private void ObjectDelete_Button_Click(object sender, EventArgs e)
    {
        if (!rims_mode && mode == Mode.Idle && !editingtext_mode)
        {
            graph[current_graph].DeleteObject(selected_node, selected_edge, graphic.Graphics);
            DisableObjectDialog();
            if (selected_node != null)
            {
                selected_node.IsInitial = false;
            }
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
            is_saved = false;
            SimulateStateMachine_Button.Enabled = false;
            if (graph[current_graph].nodes.Count == 0)
            {
                GenerateC_Button.Enabled = false;
                InsertTransition_Button.Enabled = false;
            }
        }
    }

    private void smType_SelectionChangeCommitted(object sender, EventArgs e)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            graph[i].enable_timer = ((smType.SelectedIndex == 0) ? true : false);
        }
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
        StateMachinePeriod_Textbox.Enabled = graph[current_graph].enable_timer;
    }

    private void globaluartcheckbox_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            graph[i].enable_uart = globaluartcheckbox.Checked;
        }
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void ObjectInitialState_Checkbox_Click(object sender, EventArgs e)
    {
        if (selected_node == null)
        {
            return;
        }
        int num = 0;
        if (!ObjectInitialState_Checkbox.Checked)
        {
            graph[current_graph].SetInitialState(selected_node, enabled: false);
        }
        else
        {
            switch (graph[current_graph].SetInitialState(selected_node, enabled: true))
            {
                case 0:
                    MessageBox.Show("Initial state already selected!\r\nMust first deselect current initial state", "Warning");
                    ObjectInitialState_Checkbox.Checked = false;
                    break;
                case 1:
                    graph[current_graph].SetInitialState(selected_node, enabled: true);
                    break;
            }
        }
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void GlobalCode_Textbox_TextChanged(object sender, EventArgs e)
    {
        graph[current_graph].SetLocalCode_Text(LocalCode_TextBox.Text);
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void ObjectName_Textbox_TextChanged(object sender, EventArgs e)
    {
        if (selected_node != null)
        {
            selected_node.Name = ObjectName_Textbox.Text;
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
        }
    }

    private void ObjectActions_Textbox_TextChanged(object sender, EventArgs e)
    {
        if (selected_node != null)
        {
            selected_node.Actions = ObjectActions_Textbox.Text;
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
                return;
            }
        }
        if (selected_edge != null)
        {
            selected_edge.SetActions(ObjectActions_Textbox.Text);
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
        }
    }

    private void ObjectCondition_Textbox_TextChanged(object sender, EventArgs e)
    {
        if (selected_edge != null)
        {
            selected_edge.SetCondition(ObjectCondition_Textbox.Text);
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
        }
    }

    private void ObjectInitialState_Checkbox_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void StateMachinePrefix_Textbox_TextChanged(object sender, EventArgs e)
    {
        graph[current_graph].Abbrv = StateMachinePrefix_Textbox.Text;
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void StateMachineName_Textbox_TextChanged(object sender, EventArgs e)
    {
        string name = StateMachineName_Textbox.Text;
        graph[current_graph].Name = name;
        tabs.TabPages[current_graph].Text = name;
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void StateMachinePeriod_Textbox_TextChanged(object sender, EventArgs e)
    {
        graph[current_graph].Period = StateMachinePeriod_Textbox.Text;
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void MacroCode_Textbox_TextChanged(object sender, EventArgs e)
    {
        is_saved = false;
    }

    private void StateMachinePeriod_Textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void ObjectName_Textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void ObjectCondition_Textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void ObjectActions_Textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        if (!is_saved)
        {
            switch (MessageBox.Show("The project has been edited since the last save.  Would you like to save it now before opening?", "Save Project", MessageBoxButtons.YesNoCancel))
            {
                default:
                    return;
                case DialogResult.Yes:
                    SaveSM();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        x_offset = 55;
        y_offset = 55;
        oldx = 0;
        oldy = 0;
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            DefaultExt = "sm",
            FileName = filename,
            InitialDirectory = filepath,
            AddExtension = true,
            CheckPathExists = true,
            Filter = "RIBS State Machine (*.sm)|*.sm"
        };
        openFileDialog.InitialDirectory = filepath;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            OpenProject(openFileDialog.FileName, def: false);
            is_saved = true;
            c_filename = "";
        }
    }

    private void sampleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!is_saved)
        {
            switch (MessageBox.Show("The project has been edited since the last save.  Would you like to save it now?", "Save state machine", MessageBoxButtons.YesNoCancel))
            {
                default:
                    return;
                case DialogResult.Yes:
                    SaveSM();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        Samples samples = new Samples();
        samples.ShowDialog();
        string resourceName = samples.GetResourceName();
        if (resourceName.Length != 0)
        {
            string name = "RIBS_V3.resources." + resourceName;
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
            filename = resourceName;
            Text = resourceName;
            OpenSampleProject(manifestResourceStream);
            is_saved = false;
        }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            DefaultExt = "sm",
            FileName = filename,
            InitialDirectory = filepath,
            OverwritePrompt = true,
            AddExtension = true,
            CheckPathExists = true,
            Filter = "RIBS State Machine (*.sm)|*.sm"
        };
        switch (saveFileDialog.ShowDialog())
        {
            case DialogResult.OK:
                SaveProject(saveFileDialog.FileName);
                Text = filepath;
                is_saved = true;
                break;
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (filepath.Length == 0 || filepath == default_path)
        {
            saveAsToolStripMenuItem_Click(sender, e);
            return;
        }
        SaveProject(filepath);
        Text = filepath;
        is_saved = true;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!is_saved)
        {
            switch (MessageBox.Show("The project has been edited since the last save.  Exiting now will cause all changes to be lost.\r\nSave now?", "Warning:", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    SaveSMOnExit();
                    break;
                case DialogResult.No:
                    Application.Exit();
                    break;
            }
        }
        else
        {
            Application.Exit();
        }
    }

    private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        about.Show();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!is_saved)
        {
            switch (MessageBox.Show("The project has been edited since the last save.  Would you like to save it now before opening a new project?", "New Project", MessageBoxButtons.YesNoCancel))
            {
                default:
                    return;
                case DialogResult.Yes:
                    SaveSM();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        while (tabs.TabPages.Count > 1)
        {
            tabs.TabPages.RemoveAt(0);
        }
        this.graph.Clear();
        Graph graph = new Graph();
        graph.Abbrv = "SM1";
        graph.Name = "State machine 1";
        graph.Period = "1000";
        this.graph.Add(graph);
        current_graph = 0;
        num_graphs = 1u;
        tabs.TabPages.Insert(0, graph.Name);
        tabs.TabPages[0].ImageIndex = 0;
        tabs.SelectedIndex = 0;
        StateMachineName_Textbox.Text = "State machine 1";
        StateMachinePeriod_Textbox.Text = "1000";
        StateMachinePrefix_Textbox.Text = "SM1";
        projectnamebox.Text = "My system";
        smType.SelectedIndex = 0;
        globaluartcheckbox.Checked = false;
        LocalCode_TextBox.Text = "/*Define user variables and functions for this state machine here.*/";
        MacroCode_Textbox.Text = "/*This code will be shared between state machines.*/";
        MacroCode_Textbox.Enabled = false;
        groupBox1.Text = "Variables and functions";
        ObjectActions_Textbox.Text = "";
        ObjectCondition_Textbox.Text = "";
        ObjectName_Textbox.Text = "";
        Text = "";
        ObjectInitialState_Checkbox.Checked = false;
        InsertTransition_Button.Enabled = false;
        GenerateC_Button.Enabled = false;
        SimulateStateMachine_Button.Enabled = false;
        x_offset = 55;
        y_offset = 55;
        lock (graphic)
        {
            this.graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
        is_saved = true;
        filepath = "";
        filename = "";
        c_filename = "";
    }

    private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = "https://www.programmingembeddedsystems.com/RITools/help/"
        });
    }

    private void reportsBugsFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
    {
        feedback.Show();
    }

    private void reservedVariableNamesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ReservedNames.Show();
    }

    private void disableTooltipsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        tooltips_enabled = !tooltips_enabled;
    }

    private void insertStateMachineToolStripMenuItem_Click(object sender, EventArgs e)
    {
        addtab(null, null);
    }

    private void closeStateMachineToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SuperTabControlEventArgs superTabControlEventArgs = new SuperTabControlEventArgs(null, 0, TabControlAction.Selecting);
        superTabControlEventArgs.closed_index = current_graph;
        tabclosed(null, superTabControlEventArgs);
    }

    private void licenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyValidation keyValidation = new KeyValidation();
        keyValidation.ShowDialog();
        keyValidation.Size = keyValidation.MaximumSize = keyValidation.MinimumSize = new Size(this.Width, this.Height + 50);
    }

    public void SimThread()
    {
        try
        {
            rimsSocket.BeginAccept(OnClientConnect, null);
        }
        catch (SocketException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        rims_mode = true;
        while (rims_mode)
        {
            Animate();
            Thread.Sleep(1);
        }
        if (rimsWorkerSocket != null && rimsWorkerSocket.Connected)
        {
            rimsWorkerSocket.Close();
            rimsWorkerSocket = null;
        }
        if (rimsSocket != null)
        {
            rimsSocket.Close();
            rimsSocket = null;
        }
    }

    private void OnClientConnect(IAsyncResult asyn)
    {
        try
        {
            WaitForStart(asyn);
        }
        catch (ObjectDisposedException)
        {
        }
        catch (SocketException ex2)
        {
            MessageBox.Show(ex2.Message);
        }
    }

    private void WaitForStart(IAsyncResult asyn)
    {
        if (rimsSocket != null)
        {
            rimsWorkerSocket = rimsSocket.EndAccept(asyn);
            rimsWorkerSocket.ReceiveTimeout = 5000;
            byte[] array = new byte[1500];
            SocketError errorCode = SocketError.Success;
            string text = "";
            while (!text.Contains("start"))
            {
                try
                {
                    rimsWorkerSocket.Receive(array, 0, 1500, SocketFlags.None, out errorCode);
                }
                catch (Exception)
                {
                    rimsWorkerSocket.Close();
                    rimsWorkerSocket = null;
                    rims_mode = false;
                    break;
                }
                text = Encoding.ASCII.GetString(array, 0, (int)strlen(array));
                ZeroBytes(array);
                Thread.Sleep(25);
            }
        }
        else
        {
            rims_mode = false;
        }
    }

    public void Animate()
    {
        byte[] array = new byte[1500];
        SocketError errorCode = SocketError.Success;
        string text = "";
        try
        {
            if (rimsWorkerSocket != null && rimsWorkerSocket.Connected)
            {
                rimsWorkerSocket.Receive(array, 0, 200, SocketFlags.None, out errorCode);
            }
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode != SocketError.TimedOut)
            {
                foreach (Graph item in graph)
                {
                    item.GetCurrentNode().rect.Height = 55;
                    item.GetCurrentNode().rect.Width = 55;
                }
                rimsWorkerSocket.Close();
                rimsWorkerSocket = null;
                return;
            }
        }
        text = Encoding.ASCII.GetString(array, 0, (int)strlen(array));
        char[] separator = new char[1] { ':' };
        string[] array2 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (array2.Length == 1 && array2[0] == "stop")
        {
            foreach (Graph item2 in graph)
            {
                item2.GetCurrentNode().rect.Height = 55;
                item2.GetCurrentNode().rect.Width = 55;
            }
            rims_mode = false;
            return;
        }
        for (int i = 0; i < array2.Length; i++)
        {
            string[] array3 = array2[i].Split('=');
            if (array3.Length == 2)
            {
                string varname = array3[0];
                int currentNode = Convert.ToInt32(array3[1]);
                graph.Find((Graph g) => g.Abbrv + "_State" == varname)?.SetCurrentNode(currentNode);
            }
        }
        try
        {
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
        }
        catch
        {
        }
        ZeroBytes(array);
    }

    private void RIMS_Exited(object sender, EventArgs e)
    {
        StopSimulationDelegate method = StopSimulation;
        try
        {
            Invoke(method);
        }
        catch (Exception)
        {
        }
    }

    private void StopSimulation()
    {
        is_running = false;
        SimulateStateMachine_Button.Text = "RIMS simulation";
        InsertState_Button.Enabled = true;
        InsertTransition_Button.Enabled = true;
        GenerateC_Button.Enabled = true;
        StateMachineGroupBox.Enabled = true;
        ObjectGroupBox.Enabled = true;
        aboutToolStripMenuItem1.Enabled = true;
        reportsBugsFeedbackToolStripMenuItem.Enabled = true;
        disableTooltipsToolStripMenuItem1.Enabled = true;
        helpToolStripMenuItem.Enabled = true;
        newToolStripMenuItem.Enabled = true;
        openToolStripMenuItem.Enabled = true;
        openSampleToolStripMenuItem.Enabled = true;
        insertStateMachineToolStripMenuItem.Enabled = true;
        closeStateMachineToolStripMenuItem.Enabled = true;
        licenseKeyToolStripMenuItem.Enabled = true;
        saveToolStripMenuItem.Enabled = true;
        saveAsToolStripMenuItem.Enabled = true;
        MacroCode_Textbox.Enabled = true;
        tabs.Enabled = true;
        graph[current_graph].CleanColors();
        foreach (Graph item in graph)
        {
            item.GetCurrentNode().rect.Height = 55;
            item.GetCurrentNode().rect.Width = 55;
        }
        foreach (Graph item2 in graph)
        {
            item2.CleanColors();
        }
        rims_mode = false;
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
        }
        pulsetimer.Enabled = false;
    }

    private void DisableObjectDialog()
    {
        ObjectActions_Textbox.Text = "";
        ObjectCondition_Textbox.Text = "";
        ObjectName_Textbox.Text = "";
        ObjectActions_Textbox.Enabled = false;
        ObjectCondition_Textbox.Enabled = false;
        ObjectName_Textbox.Enabled = false;
        ObjectInitialState_Checkbox.Enabled = false;
        ObjectDelete_Button.Enabled = false;
    }

    private void LoadObjectDialog()
    {
        ObjectActions_Textbox.Enabled = true;
        ObjectDelete_Button.Enabled = true;
        if (selected_node != null)
        {
            ObjectCondition_Textbox.Enabled = false;
            ObjectInitialState_Checkbox.Enabled = true;
            ObjectName_Textbox.Enabled = true;
            ObjectCondition_Textbox.Text = "";
            ObjectName_Textbox.Text = selected_node.Name;
            ObjectActions_Textbox.Text = selected_node.Actions;
            if (selected_node.IsInitial)
            {
                ObjectInitialState_Checkbox.Checked = true;
            }
            else
            {
                ObjectInitialState_Checkbox.Checked = false;
            }
        }
        else
        {
            if (selected_edge.GetType() == typeof(InitEdge))
            {
                ObjectCondition_Textbox.Enabled = false;
            }
            else
            {
                ObjectCondition_Textbox.Enabled = true;
            }
            ObjectInitialState_Checkbox.Enabled = false;
            ObjectName_Textbox.Enabled = false;
            ObjectName_Textbox.Text = "";
            ObjectActions_Textbox.Text = selected_edge.GetActions();
            ObjectCondition_Textbox.Text = selected_edge.GetCondition();
        }
    }

    private void convertToBezierToolStripMenuItem_Click(object sender, EventArgs e)
    {
        selected_edge.ConvertToBezier();
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
        is_saved = false;
        SimulateStateMachine_Button.Enabled = false;
    }

    private void convertToLineToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (selected_edge.tail.priority_enabled)
        {
            MessageBox.Show("Cannot convert leaving edges in if/else state to lines.", "Warning:", MessageBoxButtons.OK);
            return;
        }
        selected_edge.ConvertToLine();
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
    }

    private void SaveSM()
    {
        if (filepath.Length == 0 || filepath == default_path)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "sm";
            saveFileDialog.FileName = filename;
            saveFileDialog.InitialDirectory = filepath;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "RIBS State Machine (*.sm)|*.sm";
            switch (saveFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    SaveProject(saveFileDialog.FileName);
                    Text = filepath;
                    break;
            }
        }
        else
        {
            SaveProject(filepath);
        }
    }

    private void SaveSMOnExit()
    {
        if (filepath.Length == 0 || filepath == default_path)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "sm";
            saveFileDialog.FileName = filename;
            saveFileDialog.InitialDirectory = filepath;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "RIBS State Machine (*.sm)|*.sm";
            switch (saveFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    {
                        Project project = new Project(graph, projectnamebox.Text, MacroCode_Textbox.Text);
                        SMSerializer.SerializeObject(saveFileDialog.FileName, project);
                        Application.Exit();
                        break;
                    }
            }
        }
        else
        {
            SaveProject(filepath);
            Application.Exit();
        }
    }

    private void SaveProject(string f)
    {
        string[] array = f.Split('\\');
        string text = array[array.Length - 1];
        string[] array2 = text.Split('.');
        text = array2[0];
        filename = text;
        filepath = f;
        Project project = new Project(graph, projectnamebox.Text, MacroCode_Textbox.Text);
        SMSerializer.SerializeObject(f, project);
        is_saved = true;
    }

    private void OpenSampleProject(Stream s)
    {
        while (tabs.TabPages.Count > 1)
        {
            tabs.TabPages.RemoveAt(0);
        }
        graph.Clear();
        Project project = SMSerializer.DeSerializeObjectStream(s);
        graph = project.graphs;
        projectnamebox.Text = project.pname;
        MacroCode_Textbox.Text = project.macrocode;
        if (graph.Count > 1)
        {
            MacroCode_Textbox.Enabled = true;
            MacroCode_Textbox.ReadOnly = false;
            groupBox1.Text = "Variables";
        }
        else
        {
            MacroCode_Textbox.Enabled = false;
            MacroCode_Textbox.ReadOnly = true;
            groupBox1.Text = "Variables and functions";
        }
        filepath = string.Empty;
        tabs.SelectedIndex = 0;
        is_saved = false;
        bool enabled = true;
        int num = 0;
        foreach (Graph item in graph)
        {
            TabPage tabPage = new TabPage("hello")
            {
                Text = item.Name,
                BackColor = Color.White,
                ImageIndex = 0
            };
            tabs.TabPages.Insert(tabs.TabCount - 1, tabPage);
            tabs.TabPages[tabs.TabCount - 2].Name = item.Name;
            tabs.TabPages[tabs.TabCount - 2].BackColor = Color.White;
            tabPage.ImageIndex = 0;
            tabPage.AllowDrop = true;
            if (item.nodes.Count > 0)
            {
                InsertTransition_Button.Enabled = true;
            }
            else
            {
                enabled = false;
            }
            num++;
        }
        current_graph = 0;
        num_graphs = (uint)graph.Count;
        GenerateC_Button.Enabled = enabled;
        SimulateStateMachine_Button.Enabled = false;
        SuperTabControlEventArgs superTabControlEventArgs = new SuperTabControlEventArgs(null, 0, TabControlAction.Selecting);
        superTabControlEventArgs.closed_index = current_graph;
        changetab(null, superTabControlEventArgs);
    }

    private void OpenProject(string f, bool def)
    {
        while (tabs.TabPages.Count > 1)
        {
            tabs.TabPages.RemoveAt(0);
        }
        graph.Clear();
        Project project;
        if (!def)
        {
            project = SMSerializer.DeSerializeObject(f);
        }
        else
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIBS_V3.resources.default_project.sm");
            project = SMSerializer.DeSerializeObjectStream(manifestResourceStream);
        }
        graph = project.graphs;
        projectnamebox.Text = project.pname;
        MacroCode_Textbox.Text = project.macrocode;
        if (graph.Count > 1)
        {
            MacroCode_Textbox.Enabled = true;
            MacroCode_Textbox.ReadOnly = false;
            groupBox1.Text = "Variables";
        }
        else
        {
            MacroCode_Textbox.Enabled = false;
            MacroCode_Textbox.ReadOnly = true;
            groupBox1.Text = "Variables and functions";
        }
        string[] array = f.Split('\\');
        string text = array[array.Length - 1];
        string[] array2 = text.Split('.');
        text = array2[0];
        filename = text;
        filepath = f;
        tabs.SelectedIndex = 0;
        Text = filepath;
        bool enabled = true;
        foreach (Graph item in graph)
        {
            TabPage tabPage = new TabPage("hello")
            {
                Text = item.Name,
                BackColor = Color.White,
                ImageIndex = 0
            };
            tabs.TabPages.Insert(tabs.TabCount - 1, tabPage);
            tabs.TabPages[tabs.TabCount - 2].Name = item.Name;
            tabs.TabPages[tabs.TabCount - 2].BackColor = Color.White;
            tabPage.ImageIndex = 0;
            tabPage.AllowDrop = true;
            if (item.nodes.Count > 0)
            {
                InsertTransition_Button.Enabled = true;
            }
            else
            {
                enabled = false;
            }
            foreach (Edge edge in item.edges)
            {
                edge.is_selected = false;
            }
            foreach (Node node in item.nodes)
            {
                node.is_selected = false;
            }
        }
        current_graph = 0;
        num_graphs = (uint)graph.Count;
        GenerateC_Button.Enabled = enabled;
        SimulateStateMachine_Button.Enabled = false;
        SuperTabControlEventArgs superTabControlEventArgs = new SuperTabControlEventArgs(null, 0, TabControlAction.Selecting);
        superTabControlEventArgs.closed_index = current_graph;
        just_loaded = true;
        changetab(null, superTabControlEventArgs);
    }

    private void tabclosed(object sender, SuperTabControlEventArgs e)
    {
        if (tabs.TabCount > 2)
        {
            if (!is_saved)
            {
                switch (MessageBox.Show("Your current Project is not saved.  Would you like to save it now before closing this tab?", "Save Project", MessageBoxButtons.YesNoCancel))
                {
                    default:
                        return;
                    case DialogResult.Yes:
                        SaveSM();
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            tabs.TabPages.Remove(tabs.TabPages[e.closed_index]);
            graph.RemoveAt(e.closed_index);
            if (tabs.TabCount == 2)
            {
                DialogResult dialogResult = MessageBox.Show("Closing this state machine will CLEAR the global code box, make sure any needed code has been moved. Proceed?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No || dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                MacroCode_Textbox.Enabled = false;
                MacroCode_Textbox.ReadOnly = true;
                MacroCode_Textbox.Text = "/*This code will be shared between state machines.*/";
                groupBox1.Text = "Variables and functions";
                if (graph[0].GlobalCode.Contains("/*Define user variables for this state machine here. No functions; make them global.*/"))
                {
                    graph[0].SetLocalCode_Text(graph[0].GlobalCode.Replace("/*Define user variables for this state machine here. No functions; make them global.*/", "/*Define user variables and functions for this state machine here.*/"));
                }
            }
            if (e.closed_index > 0)
            {
                e.closed_index--;
            }
            current_graph = e.closed_index;
            changetab(null, e);
        }
        else
        {
            DialogResult dialogResult2 = MessageBox.Show("Must have at least 1 open state machine", "Error", MessageBoxButtons.OK);
            DialogResult dialogResult3 = dialogResult2;
            if (dialogResult3 != DialogResult.OK)
            {
                return;
            }
        }
        if (tabs.TabCount == 2 && graph[0].GlobalCode.Contains("/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n"))
        {
            graph[0].SetLocalCode_Text(graph[0].GlobalCode.Replace("/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n", ""));
            LocalCode_TextBox.Text = graph[0].GlobalCode;
        }
    }

    private void addtab(object sender, SuperTabControlEventArgs e)
    {
        if (is_running)
        {
            tabs.SelectedIndex = current_graph;
            return;
        }
        if (tabs.TabCount > 9)
        {
            MessageBox.Show("Maximum number of state machines reached", "Error");
            return;
        }
        if (tabs.TabCount > 1)
        {
            MacroCode_Textbox.Enabled = true;
            MacroCode_Textbox.ReadOnly = false;
            groupBox1.Text = "Variables";
        }
        num_graphs++;
        string text = "State machine " + num_graphs;
        string abbrv = "SM" + num_graphs;
        TabPage tabPage = new TabPage(text);
        tabs.TabPages.Insert(tabs.TabCount - 1, tabPage);
        tabs.TabPages[tabs.TabCount - 2].Name = text;
        tabs.TabPages[tabs.TabCount - 2].BackColor = Color.White;
        tabPage.ImageIndex = 0;
        tabPage.AllowDrop = true;
        if (selected_node != null)
        {
            selected_node.SetIsSelected(b: false);
            selected_node = null;
        }
        if (selected_edge != null)
        {
            if (selected_edge.is_selected)
            {
                selected_edge.is_selected = false;
            }
            selected_edge = null;
        }
        Graph graph = new Graph();
        this.graph.Add(graph);
        current_graph = this.graph.Count - 1;
        graph.SetLocalCode_Text("/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n/*Define user variables for this state machine here. No functions; make them global.*/");
        graph.Period = "1000";
        tabs.SelectedTab = tabs.TabPages[text];
        StateMachineName_Textbox.Text = text;
        StateMachinePrefix_Textbox.Text = abbrv;
        graph.Name = text;
        graph.Abbrv = abbrv;
        ObjectName_Textbox.Text = "";
        ObjectInitialState_Checkbox.Checked = false;
        InsertTransition_Button.Enabled = false;
        GenerateC_Button.Enabled = false;
        SimulateStateMachine_Button.Enabled = false;
        LocalCode_TextBox.Text = graph.GlobalCode;
        StateMachinePeriod_Textbox.Text = "1000";
        if (this.graph[0].GlobalCode.Contains("/*Define user variables and functions for this state machine here.*/"))
        {
            this.graph[0].SetLocalCode_Text(this.graph[0].GlobalCode.Replace("/*Define user variables and functions for this state machine here.*/", "/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n/*Define user variables for this state machine here. No functions; make them global.*/"));
        }
        if (!this.graph[0].GlobalCode.Contains("/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n"))
        {
            this.graph[0].SetLocalCode_Text("/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n" + this.graph[0].GlobalCode);
        }
        y_offset = graph.Getyoffset;
        x_offset = graph.Getxoffset;
        DisableObjectDialog();
        is_saved = true;
        MouseEventArgs e2 = new MouseEventArgs(MouseButtons.Left, 1, 10, 10, 10);
        backgroundpanel_MouseDown(sender, e2);
        backgroundpanel_MouseUp(sender, e2);
        for (int i = 0; i < this.graph.Count; i++)
        {
            this.graph[i].enable_timer = ((smType.SelectedIndex == 0) ? true : false);
            this.graph[i].enable_uart = globaluartcheckbox.Checked;
        }
    }

    private void changetab(object sender, SuperTabControlEventArgs e)
    {
        if (mode == Mode.AddEdge)
        {
            graph[current_graph].DeleteObject(null, newedge, graphic.Graphics);
        }
        mode = Mode.Idle;
        if (selected_node != null)
        {
            selected_node.SetIsSelected(b: false);
            selected_node = null;
        }
        if (selected_edge != null)
        {
            if (selected_edge.is_selected)
            {
                selected_edge.is_selected = false;
            }
            selected_edge = null;
        }
        Node currentNode = graph[current_graph].GetCurrentNode();
        if (currentNode != null)
        {
            lock (graphic)
            {
                currentNode.IsCurrentNode= false;
                currentNode.Draw(graphic.Graphics);
            }
        }
        if (!just_loaded)
        {
            int shiftx = graph[current_graph].shiftx;
            int shifty = graph[current_graph].shifty;
            ScrollCanvas(-shiftx, -shifty, fromcontrol: false);
            graph[current_graph].Shift(shiftx, shifty);
        }
        else
        {
            just_loaded = false;
        }
        current_graph = e.closed_index;
        tabs.SelectedIndex = current_graph;
        StateMachineName_Textbox.Text = graph[current_graph].Name;
        StateMachinePeriod_Textbox.Text = graph[current_graph].Period;
        StateMachinePrefix_Textbox.Text = graph[current_graph].Abbrv;
        LocalCode_TextBox.Text = graph[current_graph].GlobalCode;
        globaluartcheckbox.Checked = graph[current_graph].enable_uart;
        if (graph[current_graph].enable_timer)
        {
            smType.SelectedIndex = 0;
        }
        else
        {
            smType.SelectedIndex = 1;
        }
        smType_SelectionChangeCommitted(null, null);
        fixShifts();
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
        if (graph[current_graph].nodes.Count == 0 || is_running)
        {
            InsertTransition_Button.Enabled = false;
        }
        else
        {
            InsertTransition_Button.Enabled = true;
        }
        bool enabled = true;
        foreach (Graph item in graph)
        {
            if (item.nodes.Count == 0)
            {
                enabled = false;
            }
        }
        if (!is_running)
        {
            GenerateC_Button.Enabled = enabled;
        }
        else
        {
            GenerateC_Button.Enabled = false;
        }
        if (is_running)
        {
            SimulateStateMachine_Button.Enabled = true;
        }
        y_offset = graph[current_graph].Getyoffset;
        x_offset = graph[current_graph].Getxoffset;
        DisableObjectDialog();
    }

    private void fixShifts()
    {
        int shiftx = graph[current_graph].shiftx;
        int shifty = graph[current_graph].shifty;
        graph[current_graph].Shift(-shiftx, -shifty);
        ScrollCanvas(shiftx, shifty, fromcontrol: false);
        oldx = -shiftx;
        oldy = -shifty;
    }

    private void EditTransitionToolTip_Draw(object sender, DrawToolTipEventArgs e)
    {
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
        }
    }

    public uint strlen(byte[] arg)
    {
        uint num;
        for (num = 0u; num < arg.Length && arg[num] != 0; num++)
        {
        }
        return num;
    }

    public void ZeroBytes(byte[] arg)
    {
        for (uint num = 0u; num < arg.Length; num++)
        {
            arg[num] = 0;
        }
    }

    private void pulsetimer_Tick(object sender, EventArgs e)
    {
        foreach (Graph item in graph)
        {
            if (item.GetCurrentNode() != null && item.GetCurrentNode().rect.Width > 55)
            {
                item.GetCurrentNode().rect.Width--;
                item.GetCurrentNode().rect.Height--;
            }
        }
        try
        {
            lock (graphic)
            {
                graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
                graphic.Render();
            }
        }
        catch
        {
        }
    }

    private void ObjectActions_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void ObjectActions_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void ObjectCondition_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void ObjectCondition_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void StateMachineName_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void StateMachineName_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void StateMachinePrefix_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void StateMachinePrefix_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void StateMachinePeriod_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void StateMachinePeriod_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void LocalCode_TextBox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void LocalCode_TextBox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void MacroCode_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void MacroCode_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void ObjectName_Textbox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void ObjectName_Textbox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    private void projectnamebox_Enter(object sender, EventArgs e)
    {
        editingtext_mode = true;
    }

    private void projectnamebox_Leave(object sender, EventArgs e)
    {
        editingtext_mode = false;
    }

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

    private void exportAsJPGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Title = "Export image to:",
            Filter = "JPG images (*.jpg)|*.jpg",
            CheckPathExists = true
        };
        string text = "default.jpg";
        DialogResult dialogResult = saveFileDialog.ShowDialog();
        DialogResult dialogResult2 = dialogResult;
        if (dialogResult2 == DialogResult.OK)
        {
            text = saveFileDialog.FileName;
            Graphics graphics = backgroundpanel.CreateGraphics();
            Size size = backgroundpanel.Size;
            memImage = new Bitmap(size.Width, size.Height, graphics);
            Graphics graphics2 = Graphics.FromImage(memImage);
            IntPtr hdc = graphics.GetHdc();
            IntPtr hdc2 = graphics2.GetHdc();
            BitBlt(hdc2, 0, 0, backgroundpanel.ClientRectangle.Width, backgroundpanel.ClientRectangle.Height, hdc, 0, 0, 13369376);
            graphics.ReleaseHdc(hdc);
            graphics2.ReleaseHdc(hdc2);
            memImage.Save(text, ImageFormat.Jpeg);
        }
    }

    private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
    {
        int num = e.NewValue - e.OldValue;
        ScrollEventType type = e.Type;
        if (type != ScrollEventType.EndScroll)
        {
            ScrollCanvas(-num, 0, fromcontrol: true);
        }
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
    }

    private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
    {
        int num = e.NewValue - e.OldValue;
        ScrollEventType type = e.Type;
        if (type != ScrollEventType.EndScroll)
        {
            ScrollCanvas(0, -num, fromcontrol: true);
        }
        lock (graphic)
        {
            graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
            graphic.Render();
        }
    }

    private void NodeContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
        clicked_item = e.ClickedItem as ToolStripMenuItem;
        if (e.ClickedItem == NodeContextMenu.Items["basic"])
        {
            bool flag = false;
            if (selected_node.forloop_enabled)
            {
                foreach (Edge edge in graph[current_graph].edges)
                {
                    if (edge.GetT() == selected_node)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (Edge edge2 in selected_node.edges)
                {
                    if (edge2.tail == selected_node)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
            {
                MessageBox.Show("Please delete all leaving edges first.", "Warning:", MessageBoxButtons.OK);
            }
            else
            {
                selected_node.priority_enabled = false;
                selected_node.forloop_enabled = false;
            }
        }
        else if (e.ClickedItem == NodeContextMenu.Items["ifelse"])
        {
            bool flag2 = false;
            bool flag3 = true;
            if (selected_node.forloop_enabled)
            {
                foreach (Edge edge3 in graph[current_graph].edges)
                {
                    if (edge3.GetT() == selected_node)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (Edge edge4 in selected_node.edges)
                {
                    if (edge4.tail == selected_node)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (selected_node.edges.Count > 9)
            {
                MessageBox.Show("Maximum of 9 leaving edges allowed.", "Warning:", MessageBoxButtons.OK);
                flag3 = false;
            }
            if (flag2 && selected_node.forloop_enabled)
            {
                MessageBox.Show("Please delete all leaving edges first.", "Warning:", MessageBoxButtons.OK);
            }
            else if (flag2 && flag3)
            {
                selected_node.priority_enabled = true;
                selected_node.forloop_enabled = false;
                TransitionIfElseForm transitionIfElseForm = new TransitionIfElseForm();
                transitionIfElseForm.Bind(selected_node);
                transitionIfElseForm.ShowDialog();
                toClose = transitionIfElseForm;
            }
            else if (!flag2 && flag3)
            {
                MessageBox.Show("Please create a leaving edge first.", "Warning:", MessageBoxButtons.OK);
            }
        }
        else if (e.ClickedItem == NodeContextMenu.Items["forloop"])
        {
            bool flag4 = false;
            if (selected_node.edges.Count >= 1)
            {
                foreach (Edge edge5 in selected_node.edges)
                {
                    if (edge5.GetT() == selected_node)
                    {
                        flag4 = true;
                        break;
                    }
                }
            }
            if (flag4)
            {
                MessageBox.Show("Please delete all leaving edges first.", "Warning:", MessageBoxButtons.OK);
            }
            else
            {
                selected_node.priority_enabled = false;
                selected_node.forloop_enabled = true;
                TransitionForLoopForm transitionForLoopForm = new TransitionForLoopForm();
                transitionForLoopForm.Bind(selected_node);
                transitionForLoopForm.ShowDialog();
                toClose = transitionForLoopForm;
            }
        }
        is_saved = false;
    }

    private void NodeContextMenu_Opened(object sender, EventArgs e)
    {
        foreach (ToolStripMenuItem item in NodeContextMenu.Items)
        {
            item.Checked = false;
        }
        if (selected_node.priority_enabled)
        {
            ((ToolStripMenuItem)NodeContextMenu.Items["basic"]).Checked = false;
            ((ToolStripMenuItem)NodeContextMenu.Items["ifelse"]).Checked = true;
            ((ToolStripMenuItem)NodeContextMenu.Items["forloop"]).Checked = false;
        }
        else if (selected_node.forloop_enabled)
        {
            ((ToolStripMenuItem)NodeContextMenu.Items["basic"]).Checked = false;
            ((ToolStripMenuItem)NodeContextMenu.Items["ifelse"]).Checked = false;
            ((ToolStripMenuItem)NodeContextMenu.Items["forloop"]).Checked = true;
        }
        else
        {
            ((ToolStripMenuItem)NodeContextMenu.Items["basic"]).Checked = true;
            ((ToolStripMenuItem)NodeContextMenu.Items["ifelse"]).Checked = false;
            ((ToolStripMenuItem)NodeContextMenu.Items["forloop"]).Checked = false;
        }
    }

    private void NodeContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
    {
        graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
        graphic.Render();
    }

    private void loadCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        newToolStripMenuItem_Click(null, null);
        LoadFromC loadFromC = new LoadFromC(this);
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = "c";
        openFileDialog.FileName = "";
        openFileDialog.InitialDirectory = filepath;
        openFileDialog.AddExtension = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.Filter = "C Code (*.c)|*.c";
        openFileDialog.InitialDirectory = filepath;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            loadFromC.Load(openFileDialog.FileName);
            c_filename = openFileDialog.FileName;
            projectnamebox.Text = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf(Path.PathSeparator) + 1, openFileDialog.FileName.IndexOf('.') - openFileDialog.FileName.LastIndexOf(Path.PathSeparator) - 1);
            is_saved = true;
        }
    }

    public void addLocalCode(int index, string code)
    {
        graph[index].SetLocalCode_Text(code);
        LocalCode_TextBox.Text = code;
    }

    public void setSMName(int index, string smname)
    {
        StateMachineName_Textbox.Text = smname;
    }

    public void setSMPrefix(int index, string prefix)
    {
        StateMachinePrefix_Textbox.Text = prefix;
    }

    public void addState(string stateName, bool isInitial)
    {
        InsertState_Button_MouseClick(null, null);
        ObjectName_Textbox.Text = stateName;
        if (isInitial)
        {
            ObjectInitialState_Checkbox.Checked = true;
            graph[current_graph].SetInitialState(selected_node, enabled: true);
        }
    }

    public void addState(string stateName, Point pos, bool isInitial)
    {
        addState(stateName, isInitial);
        graph[current_graph].GetNodeByName(stateName).UpdatePos(pos.X, pos.Y);
        graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
        graphic.Render();
    }

    public void addTransition(string startState, string endState, string trans, string action)
    {
        MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1, graph[current_graph].GetNodeByName(startState).rect.X + origx, graph[current_graph].GetNodeByName(startState).rect.Y + origy, 0);
        InsertTransition_Button_MouseClick(null, null);
        backgroundpanel_MouseDown(null, e);
        Thread.Sleep(100);
        e = new MouseEventArgs(MouseButtons.Left, 1, graph[current_graph].GetNodeByName(endState).rect.X + origx, graph[current_graph].GetNodeByName(endState).rect.Y + origy, 0);
        backgroundpanel_MouseDown(null, e);
        ObjectActions_Textbox.Text = action;
        ObjectCondition_Textbox.Text = trans;
    }

    public void addTransition(string startState, string endState, Point h1, Point h2, string trans, string action)
    {
        addTransition(startState, endState, trans, action);
        newedge.arc.h1 = h1;
        newedge.arc.h2 = h2;
        graph[current_graph].Draw(graphic.Graphics, backgroundpanel);
        graphic.Render();
    }

    public void addStateAction(string action)
    {
        ObjectActions_Textbox.Text = action;
    }

    public void setPeriod(string period)
    {
        StateMachinePeriod_Textbox.Text = period;
    }

    public void setIfElseEdge(string stateName)
    {
        selected_node = graph[current_graph].GetNodeByName(stateName);
        selected_node.priority_enabled = true;
        selected_node.forloop_enabled = false;
    }

    public void setForEdge(string stateName, string initial, string condition, string update)
    {
        selected_node = graph[current_graph].GetNodeByName(stateName);
        selected_node.priority_enabled = false;
        selected_node.forloop_enabled = true;
        selected_node.loop.initial = initial;
        selected_node.loop.condition = condition;
        selected_node.loop.update = update;
    }

    public void setUartSMType(bool uartEnabled, bool async)
    {
        if (uartEnabled)
        {
            globaluartcheckbox.Checked = true;
            graph[current_graph].enable_uart = globaluartcheckbox.Checked;
            SimulateStateMachine_Button.Enabled = false;
        }
        if (async)
        {
            smType.SelectedIndex = 1;
        }
    }

    public void addSM()
    {
        addtab(null, null);
    }

    public void addGlobalCode(string globals)
    {
        MacroCode_Textbox.Text = globals;
    }
}
