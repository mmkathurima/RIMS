using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CSUST.Data;
using ScintillaNet;
using UILibrary;

namespace RIMS_V2;

public class MainForm : Form
{
    public enum EditorState
    {
        ASM,
        C
    }

    private const uint PULSE_DELAY = 50u;

    private IContainer components;

    private ContextMenuStrip[] aContextMenu;

    private ToolStripMenuItem[] switchMenuItem;

    private ToolStripMenuItem[] buttonMenuItem;

    private ContextMenuStrip[] bContextMenu;

    private ToolStripMenuItem[] ledMenuItem;

    private ToolStripMenuItem[] speakerMenuItem;

    private Button Open;

    private Button Compile;

    private GroupBox groupBox3;

    private Button Run;

    private MenuStrip menuStrip;

    private ToolStripMenuItem fileToolStripMenuItem;

    private ToolStripMenuItem helpToolStripMenuItem;

    private PictureBox A7;

    private PictureBox A6;

    private PictureBox A5;

    private PictureBox A4;

    private PictureBox A3;

    private PictureBox A2;

    private PictureBox A1;

    private PictureBox A0;

    private OpenFileDialog OpenCode;

    private SaveFileDialog SaveCode;

    private TextBox UARTInBuf;

    private ListView SymbolsList;

    private StatusStrip statusStrip;

    private TabControl TabControl;

    private PictureBox B0;

    private PictureBox B1;

    private PictureBox B2;

    private PictureBox B3;

    private PictureBox B4;

    private PictureBox B5;

    private PictureBox B6;

    private PictureBox B7;

    private Label label1;

    private Label label2;

    private Label label3;

    private Label label4;

    private System.Windows.Forms.Timer UpdateTimer;

    private TrackBar IPSSlider;

    private TSmartProgressBar TimerBar;

    private TabPage tabPage1;

    private TextBox UARTRxReg;

    private TextBox UARTTxReg;

    private ToolStripMenuItem OpenSample;

    private ToolStripMenuItem saveAsToolStripMenuItem;

    private ToolStripSeparator toolStripSeparator1;

    private ToolStripMenuItem exitToolStripMenuItem;

    private ToolStripMenuItem aboutToolStripMenuItem;

    private GroupBox groupBox2;

    private GroupBox groupBox1;

    private Label label5;

    private Label label6;

    private PictureBox R_pin_7;

    private PictureBox R_pin_6;

    private PictureBox R_pin_5;

    private PictureBox R_pin_4;

    private PictureBox R_pin_3;

    private PictureBox R_pin_2;

    private PictureBox R_pin_1;

    private PictureBox R_pin_0;

    private PictureBox pictureBox2;

    private PictureBox pictureBox3;

    private PictureBox pictureBox4;

    private PictureBox pictureBox5;

    private PictureBox pictureBox6;

    private PictureBox pictureBox7;

    private PictureBox pictureBox8;

    private PictureBox pictureBox9;

    private PictureBox pictureBox1;

    private Label AValue_Dec;

    private Label BValue_Dec;

    private Label AValue_Hex;

    private Label BValue_Hex;

    private Label label7;

    private ColumnHeader Symbol;

    private ColumnHeader Value;

    private GroupBox groupBox4;

    private GroupBox groupBox5;

    private GroupBox groupBox7;

    private Button RemoveSymbol;

    private Button AddSymbol;

    private Button ClearOutputBuffer;

    private System.Windows.Forms.Timer SymbolUpdateTimer;

    private ToolStripMenuItem OpenFile;

    private Button SampleButton;

    private Label label8;

    private TextBox ElapsedTime;

    private Button BreakBtn;

    private Button StepBtn;

    private SaveFileDialog SignalLog;

    private Button SaveBtn;

    private Label label9;

    private ToolStripMenuItem newToolStripMenuItem;

    private ToolStripMenuItem saveToolStripMenuItem;

    private ToolStripMenuItem edToolStripMenuItem;

    private ToolStripMenuItem undoToolStripMenuItem;

    private ToolStripMenuItem redoToolStripMenuItem;

    private ToolStripMenuItem findToolStripMenuItem;

    private ToolStripMenuItem replaceToolStripMenuItem;

    private ToolStripMenuItem regressionTestToolStripMenuItem;

    private ToolStripMenuItem helpOnlineToolStripMenuItem;

    private ToolStripMenuItem reportBugsFeedbackToolStripMenuItem;

    private ToolStripMenuItem licensToolStripMenuItem;

    private ContextMenuStrip outputTypeMenu;

    private ToolStripMenuItem outputTypeMenuItem1;

    private ToolStripMenuItem outputTypeMenuItem2;

    private ContextMenuStrip inputTypeMenuStrip1;

    private ToolStripMenuItem inputTypeMenuItem1;

    private ToolStripMenuItem inputTypeMenuItem2;

    private ToolStripMenuItem viewToolStripMenuItem;

    private ToolStripMenuItem inputsToolStripMenuItem;

    private ToolStripMenuItem setAllAsSwitchesToolStripMenuItem;

    private ToolStripMenuItem setAllButtonsToolStripMenuItem;

    private ToolStripMenuItem outputsToolStripMenuItem;

    private ToolStripMenuItem setAllLEDsToolStripMenuItem;

    private ToolStripMenuItem setAllSpeakersToolStripMenuItem;

    private ShellControl terminal;

    private ToolStripMenuItem exportToolStripMenuItem;

    private ToolStripMenuItem exportAssemblyToolStripMenuItem;

    private ToolStripMenuItem viewAssemblyToolStripMenuItem;

    private ToolStripMenuItem enableNestedInterruptsToolStripMenuItem;

    private Scintilla CodeBox;

    private Button Assemble;

    private ToolStripMenuItem toolsToolStripMenuItem;

    private ToolStripMenuItem developCToolStripMenuItem;

    private ToolStripMenuItem developASMToolStripMenuItem;

    private ToolStripMenuItem viewInstructionListToolStripMenuItem;

    private OpenFileDialog openInputVectorFile;

    private ToolStripSeparator toolStripSeparator2;

    private ToolStripMenuItem generateTimingDiagramToolStripMenuItem;

    private GroupBox testVectorBox;

    private Button saveVector;

    private Button loadVector;

    private Button useTestVectors;

    private SaveFileDialog saveInputVectorFile;

    private Scintilla testVectorText;

    private ToolStripMenuItem pIDToolStripMenuItem;

    private ToolStripMenuItem setToWaveToolStripMenuItem;

    private ToolStripMenuItem offToolStripMenuItem;

    private ToolStripMenuItem hzToolStripMenuItem;

    private ToolStripMenuItem hzToolStripMenuItem1;

    private ToolStripMenuItem hzToolStripMenuItem2;

    private EditorState editorState = EditorState.C;

    public SetValueMarker SetMarkers;

    public CompileUpdateChk CompileUpdate;

    private static int[] sound_flags = new int[8];

    private static IntPtr breakpoint_step;

    public static VM vm;

    public static VM vm_terminate;

    private static int DEFAULT_IPP = 250;

    private static uint INSTR_PER_SEC = 5000u;

    private Thread runFromInputVectorsThread;

    private static int BREAKPOINT_MARKER_NUMBER = 2;

    private static Color BREAKPOINT_COLOR = Color.Salmon;

    private static MarkerSymbol BREAKPOINT_SYMBOL = MarkerSymbol.Circle;

    private static int CUR_LINE_MARKER_NUMBER = 3;

    private static Color CUR_LINE_COLOR = Color.SeaGreen;

    private static Color LIGHTER_CUR_LINE_COLOR = Color.FromArgb(255, 85, 255, 161);

    private static MarkerSymbol CUR_LINE_SYMBOL = MarkerSymbol.Arrow;

    private static int COMPILE_ERROR_NUMBER = 4;

    private static Color COMPILE_ERROR_COLOR = Color.LightPink;

    private static MarkerSymbol COMPILE_ERROR_SYMBOL = MarkerSymbol.Background;

    public uint timer_id;

    public uint ipp;

    private bool nested_interrupts_enabled;

    private uint last_value = 2147483647u;

    private string filename;

    private string documents_directory;

    private string asmloc = "";

    private bool runFromInputVectorsThreadIsRunning;

    private bool ASMWinOpened;

    private int asm_line_offset;

    private string old_file;

    private string old_c_file;

    private int old_line_highlighted;

    private bool is_line_highlighted;

    private bool code_is_unmodified;

    private bool just_opened;

    private bool modified_since_opened;

    private bool using_testvectors;

    private int B_selected;

    private bool pidEnabled;

    private bool adjusting;

    private object vmLock = new object();

    private bool testVectorModified = true;

    public static string OpenFirstMessage = "You must load or save a file before compiling";

    public static Image Led_off = null;

    public static Image Led_on = null;

    public static Image Switch_off = null;

    public static Image Switch_on = null;

    public static Image Switch_off_dis = null;

    public static Image Switch_on_dis = null;

    public static Image Music_off = null;

    public static Image Music_on = null;

    public static Image Button_out = null;

    public static Image Button_in = null;

    public static Image Button_out_dis = null;

    public static Image Button_in_dis = null;

    public static List<Image> A_Images_Off = null;

    public static List<Image> A_Images_On = null;

    public static List<Image> B_Images_Off = null;

    public static List<Image> B_Images_On = null;

    public static int[] B_Image_Location = new int[8];

    public static int[] A_Image_Location = new int[8];

    public List<string> watches;

    private PIDBlockDisplay pidBlockDisplay;

    public static string watch_msg = "Set to \"Slowest\" speed";

    public string[] arguments;

    private bool animation_mode;

    private SM_Animation anim_comm;

    private bool recentlySaved;

    private bool waveEnabled;

    private int[] anim_var_indices;

    private string[] anim_var_names;

    public ExternalLogicBlock block;

    private PIDSimSystem pidSimSystem;

    private Dictionary<string, int> state_var_map;

    private List<uint> breakpoints;

    private bool running;

    private StringBuilder sb;

    private SineSimulation sineSimulation;

    private Peripherals peripherals;

    private List<WMPLib.WindowsMediaPlayer> players = new List<WMPLib.WindowsMediaPlayer>();

    private Form frmSevenSegment, dialFrm;
    private SevenSegment sevenSegment;
    private bool sevenSegmentShown = false, dialShown = false, programmerCalcShown = false;
    private KnobControl dialKnob;
    private ProgrammerCalculator programmerCalculator;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.MainForm));
        this.Open = new System.Windows.Forms.Button();
        this.Compile = new System.Windows.Forms.Button();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.StepBtn = new System.Windows.Forms.Button();
        this.Run = new System.Windows.Forms.Button();
        this.BreakBtn = new System.Windows.Forms.Button();
        this.menuStrip = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
        this.OpenSample = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.exportAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.edToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.enableNestedInterruptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.inputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.setAllAsSwitchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.setAllButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.setToWaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.hzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.hzToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.hzToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        this.outputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.setAllLEDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.setAllSpeakersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.viewAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.developCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.developASMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.generateTimingDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.pIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.helpOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.reportBugsFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.licensToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.regressionTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.viewInstructionListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.A7 = new System.Windows.Forms.PictureBox();
        this.inputTypeMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.inputTypeMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.inputTypeMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        this.A6 = new System.Windows.Forms.PictureBox();
        this.A5 = new System.Windows.Forms.PictureBox();
        this.A4 = new System.Windows.Forms.PictureBox();
        this.A3 = new System.Windows.Forms.PictureBox();
        this.A2 = new System.Windows.Forms.PictureBox();
        this.A1 = new System.Windows.Forms.PictureBox();
        this.A0 = new System.Windows.Forms.PictureBox();
        this.OpenCode = new System.Windows.Forms.OpenFileDialog();
        this.SaveCode = new System.Windows.Forms.SaveFileDialog();
        this.UARTInBuf = new System.Windows.Forms.TextBox();
        this.SymbolsList = new System.Windows.Forms.ListView();
        this.Symbol = new System.Windows.Forms.ColumnHeader();
        this.Value = new System.Windows.Forms.ColumnHeader();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.TabControl = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.ElapsedTime = new System.Windows.Forms.TextBox();
        this.UARTRxReg = new System.Windows.Forms.TextBox();
        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        this.CodeBox = new ScintillaNet.Scintilla();
        this.UARTTxReg = new System.Windows.Forms.TextBox();
        this.TimerBar = new CSUST.Data.TSmartProgressBar();
        this.B0 = new System.Windows.Forms.PictureBox();
        this.outputTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.outputTypeMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.outputTypeMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        this.B1 = new System.Windows.Forms.PictureBox();
        this.B2 = new System.Windows.Forms.PictureBox();
        this.B3 = new System.Windows.Forms.PictureBox();
        this.B4 = new System.Windows.Forms.PictureBox();
        this.B5 = new System.Windows.Forms.PictureBox();
        this.B6 = new System.Windows.Forms.PictureBox();
        this.B7 = new System.Windows.Forms.PictureBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
        this.IPSSlider = new System.Windows.Forms.TrackBar();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.Assemble = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.label9 = new System.Windows.Forms.Label();
        this.SaveBtn = new System.Windows.Forms.Button();
        this.label5 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.R_pin_7 = new System.Windows.Forms.PictureBox();
        this.R_pin_6 = new System.Windows.Forms.PictureBox();
        this.R_pin_5 = new System.Windows.Forms.PictureBox();
        this.R_pin_4 = new System.Windows.Forms.PictureBox();
        this.R_pin_3 = new System.Windows.Forms.PictureBox();
        this.R_pin_2 = new System.Windows.Forms.PictureBox();
        this.R_pin_1 = new System.Windows.Forms.PictureBox();
        this.R_pin_0 = new System.Windows.Forms.PictureBox();
        this.pictureBox2 = new System.Windows.Forms.PictureBox();
        this.pictureBox3 = new System.Windows.Forms.PictureBox();
        this.pictureBox4 = new System.Windows.Forms.PictureBox();
        this.pictureBox5 = new System.Windows.Forms.PictureBox();
        this.pictureBox6 = new System.Windows.Forms.PictureBox();
        this.pictureBox7 = new System.Windows.Forms.PictureBox();
        this.pictureBox8 = new System.Windows.Forms.PictureBox();
        this.pictureBox9 = new System.Windows.Forms.PictureBox();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.AValue_Dec = new System.Windows.Forms.Label();
        this.BValue_Dec = new System.Windows.Forms.Label();
        this.AValue_Hex = new System.Windows.Forms.Label();
        this.BValue_Hex = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.groupBox5 = new System.Windows.Forms.GroupBox();
        this.terminal = new UILibrary.ShellControl();
        this.ClearOutputBuffer = new System.Windows.Forms.Button();
        this.groupBox7 = new System.Windows.Forms.GroupBox();
        this.RemoveSymbol = new System.Windows.Forms.Button();
        this.AddSymbol = new System.Windows.Forms.Button();
        this.SymbolUpdateTimer = new System.Windows.Forms.Timer(this.components);
        this.SampleButton = new System.Windows.Forms.Button();
        this.label8 = new System.Windows.Forms.Label();
        this.SignalLog = new System.Windows.Forms.SaveFileDialog();
        this.openInputVectorFile = new System.Windows.Forms.OpenFileDialog();
        this.testVectorBox = new System.Windows.Forms.GroupBox();
        this.testVectorText = new ScintillaNet.Scintilla();
        this.saveVector = new System.Windows.Forms.Button();
        this.loadVector = new System.Windows.Forms.Button();
        this.useTestVectors = new System.Windows.Forms.Button();
        this.saveInputVectorFile = new System.Windows.Forms.SaveFileDialog();
        this.groupBox3.SuspendLayout();
        this.menuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.A7).BeginInit();
        this.inputTypeMenuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.A6).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A5).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A4).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A3).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.A0).BeginInit();
        this.TabControl.SuspendLayout();
        this.tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.CodeBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B0).BeginInit();
        this.outputTypeMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.B1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B3).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B4).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B5).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B6).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.B7).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.IPSSlider).BeginInit();
        this.groupBox2.SuspendLayout();
        this.groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_7).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_6).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_5).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_4).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_3).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_0).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox7).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox8).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox9).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
        this.groupBox4.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.groupBox7.SuspendLayout();
        this.testVectorBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.testVectorText).BeginInit();
        base.SuspendLayout();
        this.Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.Open.Location = new System.Drawing.Point(6, 15);
        this.Open.Name = "Open";
        this.Open.Size = new System.Drawing.Size(75, 25);
        this.Open.TabIndex = 0;
        this.Open.Text = "Open...";
        this.Open.UseVisualStyleBackColor = true;
        this.Open.Click += new System.EventHandler(Open_Click);
        this.Compile.Enabled = false;
        this.Compile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.Compile.Location = new System.Drawing.Point(7, 15);
        this.Compile.Name = "Compile";
        this.Compile.Size = new System.Drawing.Size(75, 25);
        this.Compile.TabIndex = 1;
        this.Compile.Text = "Compile";
        this.Compile.UseVisualStyleBackColor = true;
        this.Compile.Click += new System.EventHandler(Compile_Click);
        this.groupBox3.Controls.Add(this.StepBtn);
        this.groupBox3.Controls.Add(this.Run);
        this.groupBox3.Controls.Add(this.BreakBtn);
        this.groupBox3.Location = new System.Drawing.Point(374, 26);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(271, 45);
        this.groupBox3.TabIndex = 4;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Step 3";
        this.StepBtn.Enabled = false;
        this.StepBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.StepBtn.Location = new System.Drawing.Point(186, 14);
        this.StepBtn.Name = "StepBtn";
        this.StepBtn.Size = new System.Drawing.Size(75, 25);
        this.StepBtn.TabIndex = 72;
        this.StepBtn.Text = "Step";
        this.StepBtn.UseVisualStyleBackColor = true;
        this.StepBtn.Click += new System.EventHandler(StepBtn_Click);
        this.Run.Enabled = false;
        this.Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.Run.Location = new System.Drawing.Point(6, 14);
        this.Run.Name = "Run";
        this.Run.Size = new System.Drawing.Size(80, 25);
        this.Run.TabIndex = 5;
        this.Run.Text = "Run";
        this.Run.UseVisualStyleBackColor = true;
        this.Run.Click += new System.EventHandler(Run_Click);
        this.BreakBtn.Enabled = false;
        this.BreakBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.BreakBtn.Location = new System.Drawing.Point(96, 14);
        this.BreakBtn.Name = "BreakBtn";
        this.BreakBtn.Size = new System.Drawing.Size(75, 25);
        this.BreakBtn.TabIndex = 6;
        this.BreakBtn.Text = "Break";
        this.BreakBtn.UseVisualStyleBackColor = true;
        this.BreakBtn.Click += new System.EventHandler(BreakBtn_Click);
        this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.fileToolStripMenuItem, this.edToolStripMenuItem, this.viewToolStripMenuItem, this.toolsToolStripMenuItem, this.helpToolStripMenuItem });
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuStrip";
        this.menuStrip.Size = new System.Drawing.Size(844, 24);
        this.menuStrip.TabIndex = 5;
        this.menuStrip.Text = "menuStrip1";
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.newToolStripMenuItem, this.OpenFile, this.OpenSample, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.exportToolStripMenuItem, this.toolStripSeparator1, this.exitToolStripMenuItem });
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "File";
        this.newToolStripMenuItem.Name = "newToolStripMenuItem";
        this.newToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.newToolStripMenuItem.Text = "New";
        this.newToolStripMenuItem.Click += new System.EventHandler(newToolStripMenuItem_Click);
        this.OpenFile.Name = "OpenFile";
        this.OpenFile.Size = new System.Drawing.Size(153, 22);
        this.OpenFile.Text = "Open file...";
        this.OpenFile.Click += new System.EventHandler(OpenFile_Click);
        this.OpenSample.Name = "OpenSample";
        this.OpenSample.Size = new System.Drawing.Size(153, 22);
        this.OpenSample.Text = "Open sample...";
        this.OpenSample.Click += new System.EventHandler(OpenSample_Click);
        this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        this.saveToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.saveToolStripMenuItem.Text = "Save";
        this.saveToolStripMenuItem.Click += new System.EventHandler(saveToolStripMenuItem_Click);
        this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
        this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.saveAsToolStripMenuItem.Text = "Save as";
        this.saveAsToolStripMenuItem.Click += new System.EventHandler(saveAsToolStripMenuItem_Click);
        this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.exportAssemblyToolStripMenuItem });
        this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        this.exportToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.exportToolStripMenuItem.Text = "Export";
        this.exportAssemblyToolStripMenuItem.Enabled = false;
        this.exportAssemblyToolStripMenuItem.Name = "exportAssemblyToolStripMenuItem";
        this.exportAssemblyToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
        this.exportAssemblyToolStripMenuItem.Text = "Export assembly";
        this.exportAssemblyToolStripMenuItem.Click += new System.EventHandler(exportAssemblyToolStripMenuItem_Click);
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
        this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        this.exitToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.exitToolStripMenuItem.Text = "Exit";
        this.exitToolStripMenuItem.Click += new System.EventHandler(exitToolStripMenuItem_Click);
        this.edToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.undoToolStripMenuItem, this.redoToolStripMenuItem, this.findToolStripMenuItem, this.replaceToolStripMenuItem, this.enableNestedInterruptsToolStripMenuItem });
        this.edToolStripMenuItem.Name = "edToolStripMenuItem";
        this.edToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
        this.edToolStripMenuItem.Text = "Edit";
        this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
        this.undoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
        this.undoToolStripMenuItem.Text = "Undo";
        this.undoToolStripMenuItem.Click += new System.EventHandler(undoToolStripMenuItem_Click);
        this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
        this.redoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
        this.redoToolStripMenuItem.Text = "Redo";
        this.redoToolStripMenuItem.Click += new System.EventHandler(redoToolStripMenuItem_Click);
        this.findToolStripMenuItem.Name = "findToolStripMenuItem";
        this.findToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
        this.findToolStripMenuItem.Text = "Find...";
        this.findToolStripMenuItem.Click += new System.EventHandler(findToolStripMenuItem_Click);
        this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
        this.replaceToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
        this.replaceToolStripMenuItem.Text = "Find and replace...";
        this.replaceToolStripMenuItem.Click += new System.EventHandler(replaceToolStripMenuItem_Click);
        this.enableNestedInterruptsToolStripMenuItem.CheckOnClick = true;
        this.enableNestedInterruptsToolStripMenuItem.Name = "enableNestedInterruptsToolStripMenuItem";
        this.enableNestedInterruptsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
        this.enableNestedInterruptsToolStripMenuItem.Text = "Enable nested interrupts";
        this.enableNestedInterruptsToolStripMenuItem.Click += new System.EventHandler(enableNestedInterruptsToolStripMenuItem_Click);
        this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.inputsToolStripMenuItem, this.outputsToolStripMenuItem, this.viewAssemblyToolStripMenuItem });
        this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.viewToolStripMenuItem.Text = "View";
        this.inputsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.setAllAsSwitchesToolStripMenuItem, this.setAllButtonsToolStripMenuItem, this.setToWaveToolStripMenuItem });
        this.inputsToolStripMenuItem.Name = "inputsToolStripMenuItem";
        this.inputsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
        this.inputsToolStripMenuItem.Text = "Inputs";
        this.setAllAsSwitchesToolStripMenuItem.Name = "setAllAsSwitchesToolStripMenuItem";
        this.setAllAsSwitchesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.setAllAsSwitchesToolStripMenuItem.Text = "Set all switches";
        this.setAllAsSwitchesToolStripMenuItem.Click += new System.EventHandler(setAllAsSwitchesToolStripMenuItem_Click);
        this.setAllButtonsToolStripMenuItem.Name = "setAllButtonsToolStripMenuItem";
        this.setAllButtonsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.setAllButtonsToolStripMenuItem.Text = "Set all buttons";
        this.setAllButtonsToolStripMenuItem.Click += new System.EventHandler(setAllButtonsToolStripMenuItem_Click);
        this.setToWaveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.offToolStripMenuItem, this.hzToolStripMenuItem, this.hzToolStripMenuItem1, this.hzToolStripMenuItem2 });
        this.setToWaveToolStripMenuItem.Name = "setToWaveToolStripMenuItem";
        this.setToWaveToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.setToWaveToolStripMenuItem.Text = "Wave input";
        this.offToolStripMenuItem.Checked = true;
        this.offToolStripMenuItem.CheckOnClick = true;
        this.offToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        this.offToolStripMenuItem.Name = "offToolStripMenuItem";
        this.offToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
        this.offToolStripMenuItem.Text = "Off";
        this.offToolStripMenuItem.Click += new System.EventHandler(offToolStripMenuItem_Click);
        this.hzToolStripMenuItem.CheckOnClick = true;
        this.hzToolStripMenuItem.Name = "hzToolStripMenuItem";
        this.hzToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
        this.hzToolStripMenuItem.Text = "1 Hz";
        this.hzToolStripMenuItem.Click += new System.EventHandler(hzToolStripMenuItem_Click);
        this.hzToolStripMenuItem1.CheckOnClick = true;
        this.hzToolStripMenuItem1.Name = "hzToolStripMenuItem1";
        this.hzToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
        this.hzToolStripMenuItem1.Text = "6 Hz";
        this.hzToolStripMenuItem1.Click += new System.EventHandler(hzToolStripMenuItem1_Click);
        this.hzToolStripMenuItem2.CheckOnClick = true;
        this.hzToolStripMenuItem2.Name = "hzToolStripMenuItem2";
        this.hzToolStripMenuItem2.Size = new System.Drawing.Size(103, 22);
        this.hzToolStripMenuItem2.Text = "20 Hz";
        this.hzToolStripMenuItem2.Click += new System.EventHandler(hzToolStripMenuItem2_Click);
        this.outputsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.setAllLEDsToolStripMenuItem, this.setAllSpeakersToolStripMenuItem });
        this.outputsToolStripMenuItem.Name = "outputsToolStripMenuItem";
        this.outputsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
        this.outputsToolStripMenuItem.Text = "Outputs";
        this.setAllLEDsToolStripMenuItem.Name = "setAllLEDsToolStripMenuItem";
        this.setAllLEDsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.setAllLEDsToolStripMenuItem.Text = "Set all leds";
        this.setAllLEDsToolStripMenuItem.Click += new System.EventHandler(setAllLEDsToolStripMenuItem_Click);
        this.setAllSpeakersToolStripMenuItem.Name = "setAllSpeakersToolStripMenuItem";
        this.setAllSpeakersToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
        this.setAllSpeakersToolStripMenuItem.Text = "Set all speakers";
        this.setAllSpeakersToolStripMenuItem.Click += new System.EventHandler(setAllSpeakersToolStripMenuItem_Click);
        this.viewAssemblyToolStripMenuItem.Name = "viewAssemblyToolStripMenuItem";
        this.viewAssemblyToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
        this.viewAssemblyToolStripMenuItem.Text = "View assembly";
        this.viewAssemblyToolStripMenuItem.Click += new System.EventHandler(viewAssemblyToolStripMenuItem_Click_1);
        this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.developCToolStripMenuItem, this.developASMToolStripMenuItem, this.toolStripSeparator2, this.generateTimingDiagramToolStripMenuItem, this.pIDToolStripMenuItem });
        this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
        this.toolsToolStripMenuItem.Text = "Tools";
        this.developCToolStripMenuItem.Checked = true;
        this.developCToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        this.developCToolStripMenuItem.Name = "developCToolStripMenuItem";
        this.developCToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        this.developCToolStripMenuItem.Text = "Develop C";
        this.developCToolStripMenuItem.Click += new System.EventHandler(developCToolStripMenuItem_Click);
        this.developASMToolStripMenuItem.Name = "developASMToolStripMenuItem";
        this.developASMToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        this.developASMToolStripMenuItem.Text = "Develop ASM";
        this.developASMToolStripMenuItem.Click += new System.EventHandler(developASMToolStripMenuItem_Click);
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
        this.generateTimingDiagramToolStripMenuItem.Name = "generateTimingDiagramToolStripMenuItem";
        this.generateTimingDiagramToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        this.generateTimingDiagramToolStripMenuItem.Text = "Generate timing diagram";
        this.generateTimingDiagramToolStripMenuItem.Click += new System.EventHandler(generateTimingDiagramToolStripMenuItem_Click);
        this.pIDToolStripMenuItem.Name = "pIDToolStripMenuItem";
        this.pIDToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        this.pIDToolStripMenuItem.Text = "Enable PID simulation";
        this.pIDToolStripMenuItem.Click += new System.EventHandler(pIDToolStripMenuItem_Click);
        this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.helpOnlineToolStripMenuItem, this.reportBugsFeedbackToolStripMenuItem, this.licensToolStripMenuItem, this.aboutToolStripMenuItem, this.regressionTestToolStripMenuItem, this.viewInstructionListToolStripMenuItem });
        this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.helpToolStripMenuItem.Text = "Help";
        this.helpOnlineToolStripMenuItem.Name = "helpOnlineToolStripMenuItem";
        this.helpOnlineToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.helpOnlineToolStripMenuItem.Text = "Help online";
        this.helpOnlineToolStripMenuItem.Click += new System.EventHandler(helpOnlineToolStripMenuItem_Click);
        this.reportBugsFeedbackToolStripMenuItem.Name = "reportBugsFeedbackToolStripMenuItem";
        this.reportBugsFeedbackToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.reportBugsFeedbackToolStripMenuItem.Text = "Report bugs/feedback";
        this.reportBugsFeedbackToolStripMenuItem.Click += new System.EventHandler(reportBugsFeedbackToolStripMenuItem_Click);
        this.licensToolStripMenuItem.Name = "licensToolStripMenuItem";
        this.licensToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.licensToolStripMenuItem.Text = "License key";
        this.licensToolStripMenuItem.Click += new System.EventHandler(licensToolStripMenuItem_Click);
        this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        this.aboutToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.aboutToolStripMenuItem.Text = "About";
        this.aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click);
        this.regressionTestToolStripMenuItem.Name = "regressionTestToolStripMenuItem";
        this.regressionTestToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.regressionTestToolStripMenuItem.Text = "Regression test";
        this.regressionTestToolStripMenuItem.Visible = false;
        this.regressionTestToolStripMenuItem.Click += new System.EventHandler(regressionTestToolStripMenuItem_Click);
        this.viewInstructionListToolStripMenuItem.Name = "viewInstructionListToolStripMenuItem";
        this.viewInstructionListToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.viewInstructionListToolStripMenuItem.Text = "View instruction list";
        this.viewInstructionListToolStripMenuItem.Click += new System.EventHandler(viewInstructionListToolStripMenuItem_Click_1);
        this.A7.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A7.Image = (System.Drawing.Image)resources.GetObject("A7.Image");
        this.A7.Location = new System.Drawing.Point(0, 349);
        this.A7.Name = "A7";
        this.A7.Size = new System.Drawing.Size(55, 20);
        this.A7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A7.TabIndex = 8;
        this.A7.TabStop = false;
        this.A7.Tag = "Off";
        this.A7.Click += new System.EventHandler(A7_Click);
        this.inputTypeMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.inputTypeMenuItem1, this.inputTypeMenuItem2 });
        this.inputTypeMenuStrip1.Name = "outputTypeMenu";
        this.inputTypeMenuStrip1.Size = new System.Drawing.Size(111, 48);
        this.inputTypeMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(inputTypeMenuStrip1_ItemClicked);
        this.inputTypeMenuItem1.Name = "inputTypeMenuItem1";
        this.inputTypeMenuItem1.Size = new System.Drawing.Size(110, 22);
        this.inputTypeMenuItem1.Text = "Switch";
        this.inputTypeMenuItem2.Name = "inputTypeMenuItem2";
        this.inputTypeMenuItem2.Size = new System.Drawing.Size(110, 22);
        this.inputTypeMenuItem2.Text = "Button";
        this.A6.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A6.Image = (System.Drawing.Image)resources.GetObject("A6.Image");
        this.A6.Location = new System.Drawing.Point(0, 316);
        this.A6.Name = "A6";
        this.A6.Size = new System.Drawing.Size(55, 20);
        this.A6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A6.TabIndex = 9;
        this.A6.TabStop = false;
        this.A6.Tag = "Off";
        this.A6.Click += new System.EventHandler(A6_Click);
        this.A5.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A5.Image = (System.Drawing.Image)resources.GetObject("A5.Image");
        this.A5.Location = new System.Drawing.Point(0, 283);
        this.A5.Name = "A5";
        this.A5.Size = new System.Drawing.Size(55, 20);
        this.A5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A5.TabIndex = 10;
        this.A5.TabStop = false;
        this.A5.Tag = "Off";
        this.A5.Click += new System.EventHandler(A5_Click);
        this.A4.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A4.Image = (System.Drawing.Image)resources.GetObject("A4.Image");
        this.A4.Location = new System.Drawing.Point(0, 250);
        this.A4.Name = "A4";
        this.A4.Size = new System.Drawing.Size(55, 20);
        this.A4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A4.TabIndex = 11;
        this.A4.TabStop = false;
        this.A4.Tag = "Off";
        this.A4.Click += new System.EventHandler(A4_Click);
        this.A3.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A3.Image = (System.Drawing.Image)resources.GetObject("A3.Image");
        this.A3.Location = new System.Drawing.Point(0, 217);
        this.A3.Name = "A3";
        this.A3.Size = new System.Drawing.Size(55, 20);
        this.A3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A3.TabIndex = 12;
        this.A3.TabStop = false;
        this.A3.Tag = "Off";
        this.A3.Click += new System.EventHandler(A3_Click);
        this.A2.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A2.Image = (System.Drawing.Image)resources.GetObject("A2.Image");
        this.A2.Location = new System.Drawing.Point(0, 184);
        this.A2.Name = "A2";
        this.A2.Size = new System.Drawing.Size(55, 20);
        this.A2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A2.TabIndex = 13;
        this.A2.TabStop = false;
        this.A2.Tag = "Off";
        this.A2.Click += new System.EventHandler(A2_Click);
        this.A1.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A1.Image = (System.Drawing.Image)resources.GetObject("A1.Image");
        this.A1.Location = new System.Drawing.Point(0, 151);
        this.A1.Name = "A1";
        this.A1.Size = new System.Drawing.Size(55, 20);
        this.A1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A1.TabIndex = 14;
        this.A1.TabStop = false;
        this.A1.Tag = "Off";
        this.A1.Click += new System.EventHandler(A1_Click);
        this.A0.ContextMenuStrip = this.inputTypeMenuStrip1;
        this.A0.Image = (System.Drawing.Image)resources.GetObject("A0.Image");
        this.A0.Location = new System.Drawing.Point(0, 116);
        this.A0.Name = "A0";
        this.A0.Size = new System.Drawing.Size(55, 20);
        this.A0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.A0.TabIndex = 15;
        this.A0.TabStop = false;
        this.A0.Tag = "Off";
        this.A0.Click += new System.EventHandler(A0_Click);
        this.OpenCode.DefaultExt = "c";
        this.OpenCode.Filter = "RIMS C Code (*.c)|*.c|ASM files (*.s)|*.s";
        this.SaveCode.DefaultExt = "c";
        this.SaveCode.Filter = "RIMS C Code (*.c)|*.c|ASM files (*.s)|*.s";
        this.UARTInBuf.AcceptsReturn = true;
        this.UARTInBuf.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.UARTInBuf.Location = new System.Drawing.Point(6, 16);
        this.UARTInBuf.Multiline = true;
        this.UARTInBuf.Name = "UARTInBuf";
        this.UARTInBuf.Size = new System.Drawing.Size(72, 20);
        this.UARTInBuf.TabIndex = 20;
        this.SymbolsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.Symbol, this.Value });
        this.SymbolsList.Enabled = false;
        this.SymbolsList.FullRowSelect = true;
        this.SymbolsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.SymbolsList.Location = new System.Drawing.Point(2, 15);
        this.SymbolsList.Name = "SymbolsList";
        this.SymbolsList.Size = new System.Drawing.Size(195, 180);
        this.SymbolsList.TabIndex = 22;
        this.SymbolsList.TabStop = false;
        this.SymbolsList.UseCompatibleStateImageBehavior = false;
        this.SymbolsList.View = System.Windows.Forms.View.Details;
        this.Symbol.Text = "Name";
        this.Symbol.Width = 124;
        this.Value.Text = "Value";
        this.Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        this.Value.Width = 50;
        this.statusStrip.Location = new System.Drawing.Point(0, 675);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(844, 22);
        this.statusStrip.TabIndex = 25;
        this.statusStrip.Text = "statusStrip1";
        this.TabControl.Controls.Add(this.tabPage1);
        this.TabControl.Location = new System.Drawing.Point(92, 77);
        this.TabControl.Margin = new System.Windows.Forms.Padding(0);
        this.TabControl.Name = "TabControl";
        this.TabControl.Padding = new System.Drawing.Point(0, 0);
        this.TabControl.SelectedIndex = 0;
        this.TabControl.Size = new System.Drawing.Size(641, 375);
        this.TabControl.TabIndex = 26;
        this.tabPage1.BackgroundImage = (System.Drawing.Image)resources.GetObject("tabPage1.BackgroundImage");
        this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.tabPage1.Controls.Add(this.ElapsedTime);
        this.tabPage1.Controls.Add(this.UARTRxReg);
        this.tabPage1.Controls.Add(this.CodeBox);
        this.tabPage1.Controls.Add(this.UARTTxReg);
        this.tabPage1.Controls.Add(this.TimerBar);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(633, 349);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "(No file)";
        this.tabPage1.UseVisualStyleBackColor = true;
        this.ElapsedTime.BackColor = System.Drawing.Color.Red;
        this.ElapsedTime.Enabled = false;
        this.ElapsedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.ElapsedTime.Location = new System.Drawing.Point(392, 324);
        this.ElapsedTime.Name = "ElapsedTime";
        this.ElapsedTime.ReadOnly = true;
        this.ElapsedTime.Size = new System.Drawing.Size(120, 20);
        this.ElapsedTime.TabIndex = 20;
        this.ElapsedTime.Text = "0.000 Seconds";
        this.ElapsedTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.UARTRxReg.Enabled = false;
        this.UARTRxReg.Location = new System.Drawing.Point(3, 325);
        this.UARTRxReg.Name = "UARTRxReg";
        this.UARTRxReg.ReadOnly = true;
        this.UARTRxReg.Size = new System.Drawing.Size(100, 20);
        this.UARTRxReg.TabIndex = 17;
        this.UARTRxReg.Text = "UART Rx register";
        this.CodeBox.AutoComplete.ListString = "";
        this.CodeBox.AutoComplete.MaxHeight = 50;
        this.CodeBox.AutoComplete.MaxWidth = 40;
        this.CodeBox.AutoComplete.SingleLineAccept = true;
        this.CodeBox.ConfigurationManager.Language = "cs";
        this.CodeBox.Folding.Flags = ScintillaNet.FoldFlag.LineBeforeExpanded;
        this.CodeBox.Folding.MarkerScheme = ScintillaNet.FoldMarkerScheme.PlusMinus;
        this.CodeBox.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.CodeBox.Indentation.IndentWidth = 3;
        this.CodeBox.Indentation.TabWidth = 3;
        this.CodeBox.IsBraceMatching = true;
        this.CodeBox.Lexing.Lexer = ScintillaNet.Lexer.Null;
        this.CodeBox.Lexing.LexerName = "null";
        this.CodeBox.Lexing.LineCommentPrefix = "";
        this.CodeBox.Lexing.StreamCommentPrefix = "";
        this.CodeBox.Lexing.StreamCommentSufix = "";
        this.CodeBox.LineWrap.Mode = ScintillaNet.WrapMode.Word;
        this.CodeBox.LineWrap.StartIndent = 8;
        this.CodeBox.LineWrap.VisualFlags = ScintillaNet.WrapVisualFlag.Start;
        this.CodeBox.Location = new System.Drawing.Point(5, 6);
        this.CodeBox.Margins.Margin0.Width = 30;
        this.CodeBox.Margins.Margin1.IsClickable = true;
        this.CodeBox.Margins.Margin1.Width = 20;
        this.CodeBox.Margins.Margin2.Width = 15;
        this.CodeBox.Name = "CodeBox";
        this.CodeBox.Size = new System.Drawing.Size(622, 312);
        this.CodeBox.Styles.BraceBad.FontName = "Verdana";
        this.CodeBox.Styles.BraceLight.FontName = "Verdana";
        this.CodeBox.Styles.ControlChar.FontName = "Verdana";
        this.CodeBox.Styles.Default.FontName = "Verdana";
        this.CodeBox.Styles.IndentGuide.FontName = "Verdana";
        this.CodeBox.Styles.LastPredefined.FontName = "Verdana";
        this.CodeBox.Styles.LineNumber.FontName = "Verdana";
        this.CodeBox.Styles.Max.FontName = "Verdana";
        this.CodeBox.TabIndex = 19;
        this.UARTTxReg.Enabled = false;
        this.UARTTxReg.Location = new System.Drawing.Point(527, 323);
        this.UARTTxReg.Name = "UARTTxReg";
        this.UARTTxReg.ReadOnly = true;
        this.UARTTxReg.Size = new System.Drawing.Size(100, 20);
        this.UARTTxReg.TabIndex = 18;
        this.UARTTxReg.Text = "UART Tx register";
        this.TimerBar.BackColor = System.Drawing.SystemColors.Control;
        this.TimerBar.ForeColor = System.Drawing.Color.Black;
        this.TimerBar.Location = new System.Drawing.Point(128, 324);
        this.TimerBar.Name = "TimerBar";
        this.TimerBar.ProgressBarBlockSpace = 0;
        this.TimerBar.ProgressBarBlockWidth = 1;
        this.TimerBar.ProgressBarBoderStyle = CSUST.Data.TProgressBarBorderStyle.SunkenOut;
        this.TimerBar.ProgressBarFillColor = System.Drawing.Color.LimeGreen;
        this.TimerBar.Size = new System.Drawing.Size(262, 20);
        this.TimerBar.TabIndex = 17;
        this.B0.ContextMenuStrip = this.outputTypeMenu;
        this.B0.Image = (System.Drawing.Image)resources.GetObject("B0.Image");
        this.B0.Location = new System.Drawing.Point(771, 120);
        this.B0.Name = "B0";
        this.B0.Size = new System.Drawing.Size(30, 20);
        this.B0.TabIndex = 27;
        this.B0.TabStop = false;
        this.B0.Tag = "Off";
        this.outputTypeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.outputTypeMenuItem1, this.outputTypeMenuItem2 });
        this.outputTypeMenu.Name = "outputTypeMenu";
        this.outputTypeMenu.Size = new System.Drawing.Size(116, 48);
        this.outputTypeMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(outputTypeMenu_ItemClicked);
        this.outputTypeMenuItem1.Name = "outputTypeMenuItem1";
        this.outputTypeMenuItem1.Size = new System.Drawing.Size(115, 22);
        this.outputTypeMenuItem1.Text = "LED";
        this.outputTypeMenuItem2.Name = "outputTypeMenuItem2";
        this.outputTypeMenuItem2.Size = new System.Drawing.Size(115, 22);
        this.outputTypeMenuItem2.Text = "Speaker";
        this.B1.ContextMenuStrip = this.outputTypeMenu;
        this.B1.Image = (System.Drawing.Image)resources.GetObject("B1.Image");
        this.B1.Location = new System.Drawing.Point(771, 154);
        this.B1.Name = "B1";
        this.B1.Size = new System.Drawing.Size(30, 20);
        this.B1.TabIndex = 28;
        this.B1.TabStop = false;
        this.B1.Tag = "Off";
        this.B2.ContextMenuStrip = this.outputTypeMenu;
        this.B2.Image = (System.Drawing.Image)resources.GetObject("B2.Image");
        this.B2.Location = new System.Drawing.Point(771, 189);
        this.B2.Name = "B2";
        this.B2.Size = new System.Drawing.Size(30, 20);
        this.B2.TabIndex = 29;
        this.B2.TabStop = false;
        this.B2.Tag = "Off";
        this.B3.ContextMenuStrip = this.outputTypeMenu;
        this.B3.Image = (System.Drawing.Image)resources.GetObject("B3.Image");
        this.B3.Location = new System.Drawing.Point(771, 220);
        this.B3.Name = "B3";
        this.B3.Size = new System.Drawing.Size(30, 20);
        this.B3.TabIndex = 30;
        this.B3.TabStop = false;
        this.B3.Tag = "Off";
        this.B4.ContextMenuStrip = this.outputTypeMenu;
        this.B4.Image = (System.Drawing.Image)resources.GetObject("B4.Image");
        this.B4.Location = new System.Drawing.Point(771, 254);
        this.B4.Name = "B4";
        this.B4.Size = new System.Drawing.Size(30, 20);
        this.B4.TabIndex = 31;
        this.B4.TabStop = false;
        this.B4.Tag = "Off";
        this.B5.ContextMenuStrip = this.outputTypeMenu;
        this.B5.Image = (System.Drawing.Image)resources.GetObject("B5.Image");
        this.B5.Location = new System.Drawing.Point(771, 289);
        this.B5.Name = "B5";
        this.B5.Size = new System.Drawing.Size(30, 20);
        this.B5.TabIndex = 32;
        this.B5.TabStop = false;
        this.B5.Tag = "Off";
        this.B6.ContextMenuStrip = this.outputTypeMenu;
        this.B6.Image = (System.Drawing.Image)resources.GetObject("B6.Image");
        this.B6.Location = new System.Drawing.Point(771, 324);
        this.B6.Name = "B6";
        this.B6.Size = new System.Drawing.Size(30, 20);
        this.B6.TabIndex = 33;
        this.B6.TabStop = false;
        this.B6.Tag = "Off";
        this.B7.ContextMenuStrip = this.outputTypeMenu;
        this.B7.Image = (System.Drawing.Image)resources.GetObject("B7.Image");
        this.B7.Location = new System.Drawing.Point(771, 357);
        this.B7.Name = "B7";
        this.B7.Size = new System.Drawing.Size(30, 20);
        this.B7.TabIndex = 34;
        this.B7.TabStop = false;
        this.B7.Tag = "Off";
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label1.Location = new System.Drawing.Point(19, 102);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(22, 13);
        this.label1.TabIndex = 35;
        this.label1.Text = "A0";
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label2.Location = new System.Drawing.Point(19, 377);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(22, 13);
        this.label2.TabIndex = 36;
        this.label2.Text = "A7";
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label3.Location = new System.Drawing.Point(768, 380);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(22, 13);
        this.label3.TabIndex = 37;
        this.label3.Text = "B7";
        this.label4.AutoSize = true;
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label4.Location = new System.Drawing.Point(768, 102);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(22, 13);
        this.label4.TabIndex = 38;
        this.label4.Text = "B0";
        this.UpdateTimer.Enabled = true;
        this.UpdateTimer.Interval = 17;
        this.UpdateTimer.Tick += new System.EventHandler(UpdateTimer_Tick);
        this.IPSSlider.AutoSize = false;
        this.IPSSlider.LargeChange = 1;
        this.IPSSlider.Location = new System.Drawing.Point(654, 40);
        this.IPSSlider.Margin = new System.Windows.Forms.Padding(0);
        this.IPSSlider.Maximum = 5;
        this.IPSSlider.Minimum = 1;
        this.IPSSlider.Name = "IPSSlider";
        this.IPSSlider.Size = new System.Drawing.Size(171, 31);
        this.IPSSlider.TabIndex = 41;
        this.IPSSlider.Value = 3;
        this.IPSSlider.Scroll += new System.EventHandler(IPSSlider_Scroll);
        this.groupBox2.Controls.Add(this.Compile);
        this.groupBox2.Controls.Add(this.Assemble);
        this.groupBox2.Location = new System.Drawing.Point(281, 25);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(87, 46);
        this.groupBox2.TabIndex = 39;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Step 2";
        this.Assemble.Enabled = false;
        this.Assemble.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.Assemble.Location = new System.Drawing.Point(7, 15);
        this.Assemble.Name = "Assemble";
        this.Assemble.Size = new System.Drawing.Size(75, 25);
        this.Assemble.TabIndex = 1;
        this.Assemble.Text = "Assemble";
        this.Assemble.UseVisualStyleBackColor = true;
        this.Assemble.Visible = false;
        this.Assemble.Click += new System.EventHandler(Assemble_Click);
        this.groupBox1.Controls.Add(this.label9);
        this.groupBox1.Controls.Add(this.SaveBtn);
        this.groupBox1.Controls.Add(this.Open);
        this.groupBox1.Location = new System.Drawing.Point(92, 25);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(183, 46);
        this.groupBox1.TabIndex = 40;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Step 1";
        this.label9.AutoSize = true;
        this.label9.Location = new System.Drawing.Point(83, 20);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(16, 13);
        this.label9.TabIndex = 2;
        this.label9.Text = "or";
        this.SaveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.SaveBtn.Location = new System.Drawing.Point(102, 15);
        this.SaveBtn.Name = "SaveBtn";
        this.SaveBtn.Size = new System.Drawing.Size(75, 25);
        this.SaveBtn.TabIndex = 1;
        this.SaveBtn.Text = "Save";
        this.SaveBtn.UseVisualStyleBackColor = true;
        this.SaveBtn.Click += new System.EventHandler(SaveBtn_Click);
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(647, 27);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(44, 13);
        this.label5.TabIndex = 42;
        this.label5.Text = "Slowest";
        this.label6.AutoSize = true;
        this.label6.Location = new System.Drawing.Point(792, 27);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(41, 13);
        this.label6.TabIndex = 43;
        this.label6.Text = "Fastest";
        this.R_pin_7.Image = (System.Drawing.Image)resources.GetObject("R_pin_7.Image");
        this.R_pin_7.Location = new System.Drawing.Point(732, 122);
        this.R_pin_7.Name = "R_pin_7";
        this.R_pin_7.Size = new System.Drawing.Size(37, 15);
        this.R_pin_7.TabIndex = 44;
        this.R_pin_7.TabStop = false;
        this.R_pin_6.Image = (System.Drawing.Image)resources.GetObject("R_pin_6.Image");
        this.R_pin_6.Location = new System.Drawing.Point(732, 156);
        this.R_pin_6.Name = "R_pin_6";
        this.R_pin_6.Size = new System.Drawing.Size(37, 15);
        this.R_pin_6.TabIndex = 45;
        this.R_pin_6.TabStop = false;
        this.R_pin_5.Image = (System.Drawing.Image)resources.GetObject("R_pin_5.Image");
        this.R_pin_5.Location = new System.Drawing.Point(732, 189);
        this.R_pin_5.Name = "R_pin_5";
        this.R_pin_5.Size = new System.Drawing.Size(37, 15);
        this.R_pin_5.TabIndex = 46;
        this.R_pin_5.TabStop = false;
        this.R_pin_4.Image = (System.Drawing.Image)resources.GetObject("R_pin_4.Image");
        this.R_pin_4.Location = new System.Drawing.Point(732, 222);
        this.R_pin_4.Name = "R_pin_4";
        this.R_pin_4.Size = new System.Drawing.Size(37, 15);
        this.R_pin_4.TabIndex = 47;
        this.R_pin_4.TabStop = false;
        this.R_pin_3.Image = (System.Drawing.Image)resources.GetObject("R_pin_3.Image");
        this.R_pin_3.Location = new System.Drawing.Point(732, 257);
        this.R_pin_3.Name = "R_pin_3";
        this.R_pin_3.Size = new System.Drawing.Size(37, 15);
        this.R_pin_3.TabIndex = 48;
        this.R_pin_3.TabStop = false;
        this.R_pin_2.Image = (System.Drawing.Image)resources.GetObject("R_pin_2.Image");
        this.R_pin_2.Location = new System.Drawing.Point(732, 291);
        this.R_pin_2.Name = "R_pin_2";
        this.R_pin_2.Size = new System.Drawing.Size(37, 15);
        this.R_pin_2.TabIndex = 49;
        this.R_pin_2.TabStop = false;
        this.R_pin_1.Image = (System.Drawing.Image)resources.GetObject("R_pin_1.Image");
        this.R_pin_1.Location = new System.Drawing.Point(732, 326);
        this.R_pin_1.Name = "R_pin_1";
        this.R_pin_1.Size = new System.Drawing.Size(37, 15);
        this.R_pin_1.TabIndex = 50;
        this.R_pin_1.TabStop = false;
        this.R_pin_0.Image = (System.Drawing.Image)resources.GetObject("R_pin_0.Image");
        this.R_pin_0.Location = new System.Drawing.Point(732, 359);
        this.R_pin_0.Name = "R_pin_0";
        this.R_pin_0.Size = new System.Drawing.Size(37, 15);
        this.R_pin_0.TabIndex = 51;
        this.R_pin_0.TabStop = false;
        this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
        this.pictureBox2.Location = new System.Drawing.Point(58, 119);
        this.pictureBox2.Name = "pictureBox2";
        this.pictureBox2.Size = new System.Drawing.Size(37, 15);
        this.pictureBox2.TabIndex = 52;
        this.pictureBox2.TabStop = false;
        this.pictureBox3.Image = (System.Drawing.Image)resources.GetObject("pictureBox3.Image");
        this.pictureBox3.Location = new System.Drawing.Point(58, 153);
        this.pictureBox3.Name = "pictureBox3";
        this.pictureBox3.Size = new System.Drawing.Size(37, 15);
        this.pictureBox3.TabIndex = 53;
        this.pictureBox3.TabStop = false;
        this.pictureBox4.Image = (System.Drawing.Image)resources.GetObject("pictureBox4.Image");
        this.pictureBox4.Location = new System.Drawing.Point(58, 186);
        this.pictureBox4.Name = "pictureBox4";
        this.pictureBox4.Size = new System.Drawing.Size(37, 15);
        this.pictureBox4.TabIndex = 54;
        this.pictureBox4.TabStop = false;
        this.pictureBox5.Image = (System.Drawing.Image)resources.GetObject("pictureBox5.Image");
        this.pictureBox5.Location = new System.Drawing.Point(58, 219);
        this.pictureBox5.Name = "pictureBox5";
        this.pictureBox5.Size = new System.Drawing.Size(37, 15);
        this.pictureBox5.TabIndex = 55;
        this.pictureBox5.TabStop = false;
        this.pictureBox6.Image = (System.Drawing.Image)resources.GetObject("pictureBox6.Image");
        this.pictureBox6.Location = new System.Drawing.Point(58, 252);
        this.pictureBox6.Name = "pictureBox6";
        this.pictureBox6.Size = new System.Drawing.Size(37, 15);
        this.pictureBox6.TabIndex = 56;
        this.pictureBox6.TabStop = false;
        this.pictureBox7.Image = (System.Drawing.Image)resources.GetObject("pictureBox7.Image");
        this.pictureBox7.Location = new System.Drawing.Point(58, 285);
        this.pictureBox7.Name = "pictureBox7";
        this.pictureBox7.Size = new System.Drawing.Size(37, 15);
        this.pictureBox7.TabIndex = 57;
        this.pictureBox7.TabStop = false;
        this.pictureBox8.Image = (System.Drawing.Image)resources.GetObject("pictureBox8.Image");
        this.pictureBox8.Location = new System.Drawing.Point(58, 318);
        this.pictureBox8.Name = "pictureBox8";
        this.pictureBox8.Size = new System.Drawing.Size(37, 15);
        this.pictureBox8.TabIndex = 58;
        this.pictureBox8.TabStop = false;
        this.pictureBox9.Image = (System.Drawing.Image)resources.GetObject("pictureBox9.Image");
        this.pictureBox9.Location = new System.Drawing.Point(58, 351);
        this.pictureBox9.Name = "pictureBox9";
        this.pictureBox9.Size = new System.Drawing.Size(37, 15);
        this.pictureBox9.TabIndex = 59;
        this.pictureBox9.TabStop = false;
        this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
        this.pictureBox1.Location = new System.Drawing.Point(58, 79);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(26, 36);
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox1.TabIndex = 6;
        this.pictureBox1.TabStop = false;
        this.pictureBox1.Visible = false;
        this.AValue_Dec.AutoSize = true;
        this.AValue_Dec.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.AValue_Dec.Location = new System.Drawing.Point(3, 392);
        this.AValue_Dec.Name = "AValue_Dec";
        this.AValue_Dec.Size = new System.Drawing.Size(54, 24);
        this.AValue_Dec.TabIndex = 60;
        this.AValue_Dec.Text = "A = 0";
        this.BValue_Dec.AutoSize = true;
        this.BValue_Dec.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.BValue_Dec.Location = new System.Drawing.Point(752, 395);
        this.BValue_Dec.Name = "BValue_Dec";
        this.BValue_Dec.Size = new System.Drawing.Size(53, 24);
        this.BValue_Dec.TabIndex = 61;
        this.BValue_Dec.Text = "B = 0";
        this.AValue_Hex.AutoSize = true;
        this.AValue_Hex.Font = new System.Drawing.Font("Courier New", 14f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.AValue_Hex.Location = new System.Drawing.Point(4, 414);
        this.AValue_Hex.Name = "AValue_Hex";
        this.AValue_Hex.Size = new System.Drawing.Size(65, 22);
        this.AValue_Hex.TabIndex = 62;
        this.AValue_Hex.Text = "= x00";
        this.BValue_Hex.AutoSize = true;
        this.BValue_Hex.Font = new System.Drawing.Font("Courier New", 14f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.BValue_Hex.Location = new System.Drawing.Point(753, 417);
        this.BValue_Hex.Name = "BValue_Hex";
        this.BValue_Hex.Size = new System.Drawing.Size(65, 22);
        this.BValue_Hex.TabIndex = 63;
        this.BValue_Hex.Text = "= x00";
        this.label7.AutoSize = true;
        this.label7.Location = new System.Drawing.Point(697, 71);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(86, 13);
        this.label7.TabIndex = 64;
        this.label7.Text = "Execution speed";
        this.groupBox4.Controls.Add(this.UARTInBuf);
        this.groupBox4.Location = new System.Drawing.Point(4, 439);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(85, 42);
        this.groupBox4.TabIndex = 66;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "UART input";
        this.groupBox5.Controls.Add(this.terminal);
        this.groupBox5.Controls.Add(this.ClearOutputBuffer);
        this.groupBox5.Location = new System.Drawing.Point(427, 458);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(406, 222);
        this.groupBox5.TabIndex = 67;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "Terminal";
        this.terminal.Location = new System.Drawing.Point(7, 15);
        this.terminal.Name = "terminal";
        this.terminal.Prompt = ">>>";
        this.terminal.ShellTextBackColor = System.Drawing.Color.White;
        this.terminal.ShellTextFont = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.terminal.ShellTextForeColor = System.Drawing.Color.DarkBlue;
        this.terminal.Size = new System.Drawing.Size(393, 180);
        this.terminal.TabIndex = 23;
        this.ClearOutputBuffer.Location = new System.Drawing.Point(7, 197);
        this.ClearOutputBuffer.Name = "ClearOutputBuffer";
        this.ClearOutputBuffer.Size = new System.Drawing.Size(75, 23);
        this.ClearOutputBuffer.TabIndex = 22;
        this.ClearOutputBuffer.Text = "Clear";
        this.ClearOutputBuffer.UseVisualStyleBackColor = true;
        this.ClearOutputBuffer.Click += new System.EventHandler(ClearOutputBuffer_Click);
        this.groupBox7.Controls.Add(this.RemoveSymbol);
        this.groupBox7.Controls.Add(this.AddSymbol);
        this.groupBox7.Controls.Add(this.SymbolsList);
        this.groupBox7.Location = new System.Drawing.Point(228, 458);
        this.groupBox7.Name = "groupBox7";
        this.groupBox7.Size = new System.Drawing.Size(200, 222);
        this.groupBox7.TabIndex = 69;
        this.groupBox7.TabStop = false;
        this.groupBox7.Text = "Symbol watch";
        this.RemoveSymbol.Location = new System.Drawing.Point(98, 197);
        this.RemoveSymbol.Name = "RemoveSymbol";
        this.RemoveSymbol.Size = new System.Drawing.Size(100, 23);
        this.RemoveSymbol.TabIndex = 24;
        this.RemoveSymbol.Text = "Remove symbols";
        this.RemoveSymbol.UseVisualStyleBackColor = true;
        this.RemoveSymbol.Click += new System.EventHandler(RemoveSymbol_Click);
        this.AddSymbol.Location = new System.Drawing.Point(2, 197);
        this.AddSymbol.Name = "AddSymbol";
        this.AddSymbol.Size = new System.Drawing.Size(85, 23);
        this.AddSymbol.TabIndex = 23;
        this.AddSymbol.Text = "Add symbols...";
        this.AddSymbol.UseVisualStyleBackColor = true;
        this.AddSymbol.Click += new System.EventHandler(AddSymbol_Click);
        this.SymbolUpdateTimer.Enabled = true;
        this.SymbolUpdateTimer.Interval = 33;
        this.SymbolUpdateTimer.Tick += new System.EventHandler(SymbolUpdateTimer_Tick);
        this.SampleButton.Location = new System.Drawing.Point(4, 40);
        this.SampleButton.Name = "SampleButton";
        this.SampleButton.Size = new System.Drawing.Size(85, 25);
        this.SampleButton.TabIndex = 20;
        this.SampleButton.Text = "Open sample";
        this.SampleButton.UseVisualStyleBackColor = true;
        this.SampleButton.Click += new System.EventHandler(SampleButton_Click);
        this.label8.AutoSize = true;
        this.label8.Location = new System.Drawing.Point(717, 25);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(48, 13);
        this.label8.TabIndex = 71;
        this.label8.Text = "Realtime";
        this.SignalLog.DefaultExt = "vcd";
        this.SignalLog.Filter = "Value Change Dump (*.vcd)|*.vcd";
        this.SignalLog.InitialDirectory = "C:\\";
        this.openInputVectorFile.Filter = "Input vector files (*.txt)|*.txt*";
        this.testVectorBox.Controls.Add(this.testVectorText);
        this.testVectorBox.Controls.Add(this.saveVector);
        this.testVectorBox.Controls.Add(this.loadVector);
        this.testVectorBox.Location = new System.Drawing.Point(13, 484);
        this.testVectorBox.Name = "testVectorBox";
        this.testVectorBox.Size = new System.Drawing.Size(207, 194);
        this.testVectorBox.TabIndex = 72;
        this.testVectorBox.TabStop = false;
        this.testVectorBox.Text = "Input vectors";
        this.testVectorBox.Visible = false;
        this.testVectorText.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.testVectorText.Location = new System.Drawing.Point(1, 19);
        this.testVectorText.Margins.Margin0.Width = 30;
        this.testVectorText.Name = "testVectorText";
        this.testVectorText.Size = new System.Drawing.Size(200, 96);
        this.testVectorText.Styles.BraceBad.FontName = "Verdana";
        this.testVectorText.Styles.BraceLight.FontName = "Verdana";
        this.testVectorText.Styles.ControlChar.FontName = "Verdana";
        this.testVectorText.Styles.Default.FontName = "Verdana";
        this.testVectorText.Styles.IndentGuide.FontName = "Verdana";
        this.testVectorText.Styles.LastPredefined.FontName = "Verdana";
        this.testVectorText.Styles.LineNumber.FontName = "Verdana";
        this.testVectorText.Styles.Max.FontName = "Verdana";
        this.testVectorText.TabIndex = 2;
        this.testVectorText.Text = resources.GetString("testVectorText.Text");
        this.testVectorText.TextChanged += new System.EventHandler<System.EventArgs>(testVectorText_TextChanged);
        this.saveVector.Location = new System.Drawing.Point(87, 171);
        this.saveVector.Name = "saveVector";
        this.saveVector.Size = new System.Drawing.Size(75, 23);
        this.saveVector.TabIndex = 1;
        this.saveVector.Text = "Save";
        this.saveVector.UseVisualStyleBackColor = true;
        this.saveVector.Click += new System.EventHandler(saveVector_Click);
        this.loadVector.Location = new System.Drawing.Point(6, 171);
        this.loadVector.Name = "loadVector";
        this.loadVector.Size = new System.Drawing.Size(75, 23);
        this.loadVector.TabIndex = 0;
        this.loadVector.Text = "Load";
        this.loadVector.UseVisualStyleBackColor = true;
        this.loadVector.Click += new System.EventHandler(loadVector_Click);
        this.useTestVectors.Location = new System.Drawing.Point(92, 458);
        this.useTestVectors.Name = "useTestVectors";
        this.useTestVectors.Size = new System.Drawing.Size(132, 23);
        this.useTestVectors.TabIndex = 73;
        this.useTestVectors.Text = "Use test vectors";
        this.useTestVectors.UseVisualStyleBackColor = true;
        this.useTestVectors.Click += new System.EventHandler(useTestVectors_Click);
        this.saveInputVectorFile.Filter = "Input vector files (*.txt)|*.txt*";
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        base.ClientSize = new System.Drawing.Size(844, 697);
        base.Controls.Add(this.useTestVectors);
        base.Controls.Add(this.testVectorBox);
        base.Controls.Add(this.label8);
        base.Controls.Add(this.SampleButton);
        base.Controls.Add(this.groupBox7);
        base.Controls.Add(this.groupBox5);
        base.Controls.Add(this.A0);
        base.Controls.Add(this.groupBox4);
        base.Controls.Add(this.label7);
        base.Controls.Add(this.menuStrip);
        base.Controls.Add(this.BValue_Hex);
        base.Controls.Add(this.AValue_Hex);
        base.Controls.Add(this.BValue_Dec);
        base.Controls.Add(this.AValue_Dec);
        base.Controls.Add(this.pictureBox9);
        base.Controls.Add(this.pictureBox8);
        base.Controls.Add(this.pictureBox1);
        base.Controls.Add(this.pictureBox7);
        base.Controls.Add(this.pictureBox6);
        base.Controls.Add(this.pictureBox5);
        base.Controls.Add(this.pictureBox4);
        base.Controls.Add(this.pictureBox3);
        base.Controls.Add(this.pictureBox2);
        base.Controls.Add(this.R_pin_0);
        base.Controls.Add(this.R_pin_1);
        base.Controls.Add(this.R_pin_2);
        base.Controls.Add(this.R_pin_3);
        base.Controls.Add(this.R_pin_4);
        base.Controls.Add(this.R_pin_5);
        base.Controls.Add(this.R_pin_6);
        base.Controls.Add(this.R_pin_7);
        base.Controls.Add(this.label6);
        base.Controls.Add(this.label5);
        base.Controls.Add(this.groupBox1);
        base.Controls.Add(this.groupBox2);
        base.Controls.Add(this.label4);
        base.Controls.Add(this.label3);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.B7);
        base.Controls.Add(this.B6);
        base.Controls.Add(this.B5);
        base.Controls.Add(this.B4);
        base.Controls.Add(this.B3);
        base.Controls.Add(this.B2);
        base.Controls.Add(this.B1);
        base.Controls.Add(this.B0);
        base.Controls.Add(this.TabControl);
        base.Controls.Add(this.statusStrip);
        base.Controls.Add(this.IPSSlider);
        base.Controls.Add(this.A1);
        base.Controls.Add(this.A2);
        base.Controls.Add(this.A3);
        base.Controls.Add(this.A4);
        base.Controls.Add(this.A5);
        base.Controls.Add(this.A6);
        base.Controls.Add(this.A7);
        base.Controls.Add(this.groupBox3);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        base.Icon = RIMS_V2_MainForm.Icon4;
        base.MainMenuStrip = this.menuStrip;
        base.MaximizeBox = false;
        this.MaximumSize = new System.Drawing.Size(850, 700);
        this.MinimumSize = new System.Drawing.Size(850, 700);
        base.Name = "MainForm";
        base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
        this.Text = "Riverside-Irvine Microcontroller Simulator (RIMS)";
        base.Load += new System.EventHandler(MainForm_Load);
        this.groupBox3.ResumeLayout(false);
        this.menuStrip.ResumeLayout(false);
        this.menuStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.A7).EndInit();
        this.inputTypeMenuStrip1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.A6).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A5).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A4).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A3).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A2).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.A0).EndInit();
        this.TabControl.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.CodeBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B0).EndInit();
        this.outputTypeMenu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.B1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B2).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B3).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B4).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B5).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B6).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.B7).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.IPSSlider).EndInit();
        this.groupBox2.ResumeLayout(false);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_7).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_6).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_5).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_4).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_3).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_2).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.R_pin_0).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox7).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox8).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox9).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
        this.groupBox4.ResumeLayout(false);
        this.groupBox4.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.groupBox7.ResumeLayout(false);
        this.testVectorBox.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.testVectorText).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    [DllImport("winmm.dll")]
    private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

    public int strlen(byte[] str)
    {
        int i;
        for (i = 0; str[i] != 0; i++)
        {
        }
        return i;
    }

    public unsafe MainForm(string[] args)
    {
        InitializeComponent();
        Text = "Riverside-Irvine Microcontroller Simulator (Instance: " + RIMSProcessInfo.GetNewRIMSName() + ")";
        arguments = args;
        int num = -1;
        string path = string.Empty;
        base.TopMost = false;
        string text = arguments.FirstOrDefault((string arg) => arg.StartsWith("/sm_animation"));
        if (text != null)
        {
            animation_mode = true;
            path = text["/sm_animation:".Length..];
            text = arguments.FirstOrDefault((string arg) => arg.StartsWith("/ribs_port"));
            if (text != null)
            {
                num = Convert.ToInt32(text.Split(':')[1]);
            }
            else
            {
                MessageBox.Show("Must specify /ribs_port:<port> option");
                Environment.Exit(-1);
            }
        }
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.led_off.png");
        Led_off = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.led_on.png");
        Led_on = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.switch_off.png");
        Switch_off = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.switch_on.png");
        Switch_on = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.music_off.png");
        Music_off = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.music_on.png");
        Music_on = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.button_out.png");
        Button_out = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.button_in.png");
        Button_in = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.switch_off_dis.png");
        Switch_off_dis = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.switch_on_dis.png");
        Switch_on_dis = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.button_out_dis.png");
        Button_out_dis = new Bitmap(manifestResourceStream);
        manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.button_in_dis.png");
        Button_in_dis = new Bitmap(manifestResourceStream);
        string tempPath = Path.GetTempPath();
        for (int i = 0; i < 8; i++)
        {
            Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.songs.song" + i + ".wav");
            try
            {
                string text2 = tempPath + "\\song" + i + ".mp3";
                using Stream stream = new FileStream(text2, FileMode.Create, FileAccess.Write);
                byte[] array = new byte[32768];
                int count;
                while ((count = manifestResourceStream2.Read(array, 0, array.Length)) > 0)
                    stream.Write(array, 0, count);
                //mciSendString("open \"" + text2 + "\" type waveaudio alias Song0", null, 0, IntPtr.Zero);
            }
            catch (IOException e)
            {
                MessageBox.Show(owner: this, text: e.ToString(), this.Text);
            }
        }
        A_Images_Off = new List<Image>();
        B_Images_Off = new List<Image>();
        A_Images_On = new List<Image>();
        B_Images_On = new List<Image>();
        A_Images_Off.Add(Switch_off);
        A_Images_Off.Add(Button_out);
        A_Images_Off.Add(Switch_off_dis);
        A_Images_Off.Add(Button_out_dis);
        A_Images_On.Add(Switch_on);
        A_Images_On.Add(Button_in);
        A_Images_On.Add(Switch_on_dis);
        A_Images_On.Add(Button_in_dis);
        B_Images_Off.Add(Led_off);
        B_Images_Off.Add(Music_off);
        B_Images_On.Add(Led_on);
        B_Images_On.Add(Music_on);
        base.FormClosing += MainForm_FormClosing;
        documents_directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\RIMS\\";
        if (!Directory.Exists(documents_directory))
        {
            Directory.CreateDirectory(documents_directory);
        }
        vm = new VM();
        vm_terminate = new VM();
        code_is_unmodified = true;
        just_opened = false;
        modified_since_opened = false;
        ipp = (uint)DEFAULT_IPP;
        watches = new List<string>();
        breakpoints = new List<uint>();
        running = false;
        LoadPeriphs();
        CodeBox.UseBackColor = true;
        CodeBox.Indentation.UseTabs = false;
        CodeBox.Indentation.IndentWidth = 3;
        CodeBox.Indentation.TabWidth = 3;
        CodeBox.ConfigurationManager.Language = "cs";
        sb = new StringBuilder(1000);
        generateTimingDiagramToolStripMenuItem.Enabled = false;
        using_testvectors = false;
        testVectorModified = false;
        recentlySaved = true;
        if (!animation_mode)
        {
            CodeBox.KeyDown += CodeBox_KeyDown;
            CodeBox.TextChanged += CodeBox_TextChanged;
            terminal.CommandEntered += terminal_TextChanged;
            Stream manifestResourceStream3 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.resources.RIMS_sample_code.c");
            byte[] array2 = new byte[4096];
            if (manifestResourceStream3.Read(array2, 0, (int)manifestResourceStream3.Length) != manifestResourceStream3.Length)
                CodeBox.Text = "";
            else
                CodeBox.Text = Encoding.UTF8.GetString(array2);

            CodeBox.UndoRedo.EmptyUndoBuffer();
            manifestResourceStream3.Close();
            TabControl.TabPages[0].Text = "(No file)";
            just_opened = true;
            modified_since_opened = false;
            anim_comm = null;
        }
        else
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(path);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Couldn't find input file! Are you sure you didn't delete it?", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(5);
            }
            CodeBox.Text = streamReader.ReadToEnd();
            streamReader.Close();
            TabControl.TabPages[0].Text = path;
            CodeBox.IsReadOnly = true;
            CodeBox.BackColor = SystemColors.Control;
            CodeBox.Margins.FoldMarginColor = SystemColors.Control;
            Open.Enabled = false;
            Compile.Enabled = false;
            Run.Enabled = false;
            exportAssemblyToolStripMenuItem.Enabled = false;
            OpenSample.Enabled = false;
            OpenFile.Enabled = false;
            SampleButton.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            newToolStripMenuItem.Enabled = false;
            generateTimingDiagramToolStripMenuItem.Enabled = true;
            BreakBtn.Enabled = false;
            StepBtn.Enabled = false;
            vm.vm = VMInterface.CreateVM();
            ListViewItem value = new ListViewItem()
            {
                Text = watch_msg
            };
            SymbolsList.Items.Add(value);
            watches.Clear();
            string location = Assembly.GetExecutingAssembly().Location;
            string[] array3 = location.Split('\\');
            location = "";
            uint num2 = 0u;
            while (array3.Length > 1 && num2 < array3.Length - 1)
            {
                location += array3[num2];
                location += "\\";
                num2++;
            }
            VMInterface.SetBaseDirectory(vm.vm, location);
            VMInterface.SetFilename(vm.vm, path);
            int num3 = VMInterface.Compile(vm.vm);
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ErrorStruct)));
            byte* ptr = (byte*)intPtr.ToPointer();
            for (int j = 0; j < Marshal.SizeOf(typeof(ErrorStruct)); j++)
                ptr[j] = 0;

            VMInterface.GetErrors(vm.vm, intPtr);
            ErrorStruct errorStruct = (ErrorStruct)Marshal.PtrToStructure(intPtr, typeof(ErrorStruct));
            Marshal.FreeHGlobal(intPtr);
            if (num3 > 0)
            {
                animation_mode = false;
                string text3 = "";
                for (int k = 0; k < num3; k++)
                    text3 = text3 + Marshal.PtrToStringAnsi(errorStruct.errors[k]) + "\r\n";

                try
                {
                    MessageBox.Show("There were errors while compiling the generated C code.\r\nRe-check your state machine for errors.  If you cannot find any errors and believe this is a bug, please see programmingembeddedsystems.com for an email address to report RI tool bugs.\r\n****************************************\r\n" + text3 + "****************************************", "Compilation error(s).", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Environment.Exit(1);
                }
                catch (Exception)
                {
                }
            }
            IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InitStruct)));
            byte* ptr2 = (byte*)intPtr2.ToPointer();
            for (int l = 0; l < Marshal.SizeOf(typeof(InitStruct)); l++)
                ptr2[l] = 0;

            VMInterface.Initialize(vm.vm, intPtr2);
            InitStruct initStruct = (InitStruct)Marshal.PtrToStructure(intPtr2, typeof(InitStruct));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
            IntPtr clock = initStruct.clock;
            breakpoint_step = initStruct.breakpoint_pulse;
            if (vm.ts != IntPtr.Zero)
                Marshal.FreeHGlobal(vm.ts);

            vm.ts = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ThreadStruct)));
            _ = (ThreadStruct)Marshal.PtrToStructure(vm.ts, typeof(ThreadStruct));
            TimerInterface.timeBeginPeriod(10u);
            VMInterface.SetIPP(vm.vm, ipp);
            text = arguments.FirstOrDefault((string arg) => arg.StartsWith("/state_vars"));
            if (text == null)
            {
                MessageBox.Show("Must specify /state_vars:<SM_State>,<SM2_State>");
                Environment.Exit(-1);
            }
            state_var_map = new Dictionary<string, int>();
            List<string> list = text.Substring("/state_vars:".Length).Split(',').ToList();
            list.RemoveAll((string a) => a.Length == 0);
            for (int m = 0; m < list.Count; m++)
            {
                string text4 = list[m];
                state_var_map[text4] = VMInterface.GetSymbolIndex(vm.vm, text4);
                if (state_var_map[text4] == -1)
                {
                    MessageBox.Show("Couldn't find symbol \"" + text4 + "\" in the loaded program... Please report this to programmingembeddedsystems.com", "Couldn't find symbol", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Environment.Exit(2);
                }
            }
            anim_comm = new SM_Animation(num);
            if (!anim_comm.Connect())
            {
                MessageBox.Show($"Couldn't connect to localhost on port {num}.", "Couldn't connect", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(3);
            }
            if (!anim_comm.SendStart())
            {
                MessageBox.Show("Couldn't send \"start\" command to server.", "Couldn't start connection", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(4);
            }
            VMInterface.Run(vm.vm, vm.ts);
            running = true;
            timer_id = TimerInterface.timeSetEvent(50u, 0u, clock, vm.vm, 1u);
        }
        UpdateTimer.Start();

        for (int i = 0; i < 8; i++)
        {
            //players[i].URL = Path.Join(Path.GetTempPath(), string.Format("song{0}.mp3", i));
            players.Add(new WMPLib.WindowsMediaPlayer());
            players[i].settings.setMode("loop", true);
            players[i].PlayStateChange += (int NewState) =>
            {
                if (i < 8 && (WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
                    players[i].close();
            };
        }

        ToolStripItem showSegment = new ToolStripMenuItem()
        {
            Name = "showSevenSegmentDisplay",
            Size = new System.Drawing.Size(153, 22),
            Text = "Show Seven Segment Display",
        };
        showSegment.Click += new System.EventHandler((object sender, EventArgs e) =>
        {
            if (!sevenSegmentShown)
            {
                frmSevenSegment = new Form()
                {
                    Width = 250,
                    //FormBorderStyle = FormBorderStyle.FixedSingle;
                    MaximizeBox = false,
                    Icon = this.Icon,
                    Location = new Point(this.Location.X + this.Width, this.Location.Y),
                    Text = "Seven Segment Display",

                };

                sevenSegment = new SevenSegment()
                {
                    Dock = DockStyle.Fill,
                    ColonOn = false,
                    ColonShow = false,
                    ColorBackground = Color.FromArgb(20, 24, 27),
                    ColorDark = Color.FromArgb(170, 174, 179),
                    ColorLight = Color.FromArgb(241, 11, 34),
                    CustomPattern = 0,
                    DecimalOn = false,
                    DecimalShow = false,
                    ElementWidth = 10,
                    Name = "sevenSegment1",
                    TabStop = false,
                };
                frmSevenSegment.Controls.Add(sevenSegment);
                frmSevenSegment.Show();
                frmSevenSegment.FormClosed += new FormClosedEventHandler((object sender, FormClosedEventArgs fcea) =>
                {
                    sevenSegmentShown = false;
                });
                sevenSegmentShown = true;
            }
            else if (frmSevenSegment != null)
                frmSevenSegment.Focus();
        });

        this.outputsToolStripMenuItem.DropDownItems.Add(showSegment);

        ToolStripMenuItem programmerCalc = new ToolStripMenuItem()
        {
            Name = "programmerCalculator",
            Size = new System.Drawing.Size(206, 22),
            Text = "Programmer Calculator"
        };
        programmerCalc.Click += (object sender, EventArgs e) =>
            {
                if (!this.programmerCalcShown)
                {
                    this.programmerCalculator = new ProgrammerCalculator();
                    this.programmerCalculator.FormClosed += (object sender, FormClosedEventArgs fcea) =>
                    {
                        this.programmerCalcShown = false;
                    };
                    this.programmerCalculator.Show();
                    this.programmerCalcShown = true;
                }
                else this.programmerCalculator?.Focus();
            };
        this.toolsToolStripMenuItem.DropDownItems.Add(programmerCalc);

        ToolStripMenuItem dialToolStripMenuItem = new ToolStripMenuItem()
        {
            Name = "dialToolStripMenuItem",
            Size = new System.Drawing.Size(153, 22),
            Text = "Show Dial"
        };
        dialToolStripMenuItem.Click += (object sender, EventArgs e) =>
        {
            if (!this.dialShown)
            {
                //TODO: Consider whether to turn up to 255 or leave it at 9.
                dialKnob = new KnobControl()
                {
                    EndAngle = 405F,
                    ImeMode = System.Windows.Forms.ImeMode.On,
                    KnobBackColor = System.Drawing.Color.White,
                    KnobPointerStyle = KnobControl.KnobPointerStyles.line,
                    LargeChange = 1,
                    Location = new System.Drawing.Point(13, 14),
                    Margin = new System.Windows.Forms.Padding(4, 5, 4, 5),
                    Maximum = byte.MaxValue,
                    Minimum = 0,
                    MouseWheelBarPartitions = byte.MaxValue,
                    Name = "knobControl3",
                    PointerColor = System.Drawing.Color.Black,
                    ScaleColor = System.Drawing.Color.Black,
                    ScaleDivisions = 16,
                    ScaleFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
                    ScaleFontAutoSize = false,
                    ScaleSubDivisions = 4,
                    ShowLargeScale = true,
                    ShowSmallScale = false,
                    Size = new System.Drawing.Size(200, 200),
                    SmallChange = 1,
                    StartAngle = 135F,
                    TabIndex = 2,
                    Value = 0,
                };

                dialFrm = new Form()
                {
                    AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F),
                    AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font,
                    ClientSize = new System.Drawing.Size(235, 228),
                    Margin = new System.Windows.Forms.Padding(4, 5, 4, 5),
                    Name = "dialFrm",
                    Text = "Dial",
                    Icon = this.Icon,
                    MaximizeBox = false,
                    Location = new Point(this.Location.X + this.Width, this.Location.Y),
                    FormBorderStyle = FormBorderStyle.FixedDialog
                };
                dialFrm.FormClosing += (object sender, FormClosingEventArgs e) =>
                {
                    if (this.dialShown && this.running)
                        e.Cancel = true;
                };
                dialFrm.FormClosed += (object sender, FormClosedEventArgs e) =>
                {
                    this.dialShown = false;
                    testVectorCleanup();
                };
                dialFrm.Controls.Add(dialKnob);
                dialFrm.Show();
                this.dialShown = true;

                Invoke((Action)delegate
                {
                    var temp_marker = testVectorText.Markers[CUR_LINE_MARKER_NUMBER];
                    temp_marker.Number = CUR_LINE_MARKER_NUMBER;
                    temp_marker.Symbol = CUR_LINE_SYMBOL;
                    Marker marker = temp_marker;
                    Color color = (temp_marker.BackColor = CUR_LINE_COLOR);
                    Color foreColor = color;
                    marker.ForeColor = foreColor;
                    A0.Enabled = false;
                    A1.Enabled = false;
                    A2.Enabled = false;
                    A3.Enabled = false;
                    A4.Enabled = false;
                    A5.Enabled = false;
                    A6.Enabled = false;
                    A7.Enabled = false;
                    for (int i = 0; i < 8; i++)
                    {
                        if (A_Image_Location[i] < 2)
                            A_Image_Location[i] += 2;
                        UpdateA_Images();
                    }
                });
            }
            else this.dialFrm?.Focus();
        };
        this.inputsToolStripMenuItem.DropDownItems.Add(dialToolStripMenuItem);
    }

    public MainForm()
    {
    }

    private void LoadPeriphs()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        currentDirectory += "\\..\\Peripherals\\";
        peripherals = new Peripherals(currentDirectory);
        ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)menuStrip.Items[3];
        using ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem
        {
            Checked = false,
            CheckOnClick = true,
            Text = "Show extra ports"
        };
        toolStripMenuItem2.Click += delegate
        {
            peripherals.ToggleView("ExtraPorts_periph");
        };
        toolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
    }

    private void CodeBox_TextChanged(object sender, EventArgs e)
    {
        if (!just_opened)
        {
            if (code_is_unmodified)
            {
                TabControl.TabPages[0].Text += "*";
                Compile.Enabled = false;
                Run.Enabled = false;
                pIDToolStripMenuItem.Enabled = true;
                exportAssemblyToolStripMenuItem.Enabled = false;
                Assemble.Enabled = false;
            }
            code_is_unmodified = false;
            modified_since_opened = true;
        }
        else
            just_opened = false;
    }

    private void CodeBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
            DoSave();
    }

    private void Open_Click(object sender, EventArgs e)
    {
        generateTimingDiagramToolStripMenuItem.Enabled = false;
        if (editorState == EditorState.C)
            OpenCode.Filter = "RIMS C Code (*.c)|*.c";
        else if (editorState == EditorState.ASM)
            OpenCode.Filter = "ASM files (*.s)|*.s";

        if (OpenCode.ShowDialog() != DialogResult.OK)
            return;

        lock (typeof(MainForm))
        {
            TimerInterface.timeKillEvent(timer_id);
            vm.Dispose();
            vm.vm = VMInterface.CreateVM();
            VMInterface.SetFilename(vm.vm, OpenCode.FileName);
            VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
        }
        filename = OpenCode.FileName;
        SaveCode.FileName = OpenCode.FileName;
        StreamReader streamReader = new StreamReader(filename);
        if (filename.EndsWith(".s"))
        {
            string text = streamReader.ReadLine();
            string text2 = "";
            while (text != null)
            {
                if (text.Length > 0)
                {
                    if (text[0] != '\'')
                    {
                        text2 += text;
                        if (text[^1] != '\n')
                            text2 += "\n";

                    }
                }
                else
                {
                    text2 += "\n";
                }
                text = streamReader.ReadLine();
            }
            CodeBox.IsReadOnly = false;
            CodeBox.Text = text2;
            editorState = EditorState.ASM;
            developCToolStripMenuItem.Checked = false;
            developASMToolStripMenuItem.Checked = true;
            viewAssemblyToolStripMenuItem.Enabled = false;
        }
        else
        {
            CodeBox.IsReadOnly = false;
            CodeBox.Text = streamReader.ReadToEnd();
        }
        streamReader.Close();
        updateEditorState(filename);
        enableCompileAssemble();
        code_is_unmodified = true;
        just_opened = true;
        modified_since_opened = false;
        TabControl.TabPages[0].Text = OpenCode.FileName;
        CodeBox.IsReadOnly = false;
        UARTRxReg.Text = "UART Rx Register";
        UARTTxReg.Text = "UART Tx Register";
        Run.Text = "Run";
        SaveBtn.Enabled = true;
        Run.Enabled = false;
        pIDToolStripMenuItem.Enabled = true;
        exportAssemblyToolStripMenuItem.Enabled = false;
        BreakBtn.Enabled = false;
        StepBtn.Enabled = false;
        SaveCode.FileName = OpenCode.FileName;
        lock (SymbolsList)
        {
            SymbolsList.Items.Clear();
            if (IPSSlider.Value != 1)
            {
                ListViewItem listViewItem = new ListViewItem
                {
                    Text = watch_msg
                };
                SymbolsList.Items.Add(listViewItem);
            }
        }
        running = false;
        ElapsedTime.BackColor = Color.Red;
        CodeBox.BackColor = Color.White;
        CodeBox.Margins.FoldMarginColor = Color.White;
        CodeBox.Margins.FoldMarginHighlightColor = Color.White;
        ElapsedTime.Text = "0.000 Seconds";
        watches.Clear();
        if (File.Exists(asmloc))
        {
            File.Delete(asmloc);
        }
    }

    private void InitSize()
    {
        this.Size = this.MaximumSize = new Size(this.Width, this.Height - 100);
        this.terminal.Size = this.terminal.MaximumSize = new System.Drawing.Size(this.terminal.Width, 160);
        this.groupBox5.Size = this.groupBox5.MaximumSize = new System.Drawing.Size(this.groupBox5.Width, 202);
        this.SymbolsList.Size = this.SymbolsList.MaximumSize = new System.Drawing.Size(this.SymbolsList.Width, 160);
        this.groupBox7.Size = this.groupBox7.MaximumSize = new System.Drawing.Size(this.groupBox7.Width, 170);
    }

    private void Terminal_OnCommandEntered(object sender, CommandEnteredEventArgs e)
    {
        Process p = Process.Start(new ProcessStartInfo("cmd.exe")
        {
            Arguments = "/C " + e.Command,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        });
        string output = p.StandardOutput.ReadToEnd();
        string error = p.StandardError.ReadToEnd();
        p.WaitForExit();
        if (output.Length != 0)
            this.terminal.WriteText(output);
        else if (error.Length != 0)
            this.terminal.WriteText(error);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        CodeBox.MarginClick += CodeBox_MarginClick;
        this.terminal.CommandEntered += new EventCommandEntered(Terminal_OnCommandEntered);
        InitSize();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        lock (typeof(MainForm))
        {
            TimerInterface.timeKillEvent(timer_id);
            TimerInterface.timeEndPeriod(10u);
            if (animation_mode && anim_comm.ReadyForUpdates())
                anim_comm.SendStop();

        }
        if (File.Exists(asmloc))
        {
            File.Delete(asmloc);
        }
        if (runFromInputVectorsThread != null)
        {
            try
            {
                runFromInputVectorsThreadIsRunning = false;
                runFromInputVectorsThread.Abort();
                runFromInputVectorsThread = null;
            }
            catch (PlatformNotSupportedException pe)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    MessageBox.Show(this, pe.ToString() + "\nExiting...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        if (pidEnabled)
        {
            pidSimSystem.stop();
            setToWaveToolStripMenuItem.Enabled = false;
        }
        if (waveEnabled)
        {
            sineSimulation.stop();
        }
    }

    private void Compile_Click(object sender, EventArgs e)
    {
        if (SaveCode.FileName.Length == 0)
        {
            MessageBox.Show(OpenFirstMessage, "Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }
        if (!code_is_unmodified)
        {
            StreamWriter streamWriter = new StreamWriter(SaveCode.FileName);
            streamWriter.Write(CodeBox.Text);
            streamWriter.Close();
            TabControl.TabPages[0].Text = SaveCode.FileName;
            code_is_unmodified = true;
        }
        Compile.Enabled = false;
        if (DoCompile())
        {
            if (block != null && !block.compile())
            {
                Compile.Enabled = true;
                Run.Enabled = false;
                pIDToolStripMenuItem.Enabled = true;
                exportAssemblyToolStripMenuItem.Enabled = false;
                return;
            }
            Run.Text = "Run";
            if (ASMWinOpened)
                CompileUpdate(comped: true, asmloc);

        }
        else if (block != null && !block.compile())
            return;

        Compile.Enabled = true;
    }

    private unsafe bool DoCompile()
    {
        bool flag = true;
        peripherals.RegisterPeripheralsWithVM(vm);
        string location = Assembly.GetExecutingAssembly().Location;
        string[] array = location.Split('\\');
        location = "";
        uint num = 0u;
        while (array.Length > 1 && num < array.Length - 1)
        {
            location += array[num];
            location += "\\";
            num++;
        }
        VMInterface.SetBaseDirectory(vm.vm, location);
        if (!File.Exists(Environment.GetEnvironmentVariable("TEMP") + "\\RIMS.h"))
        {
            File.Copy(location + "RIMS.h", Environment.GetEnvironmentVariable("TEMP") + "\\RIMS.h", overwrite: true);
        }

        int num2 = VMInterface.Compile(vm.vm);
        int num3 = 1024;
        StringBuilder stringBuilder = new StringBuilder(num3);
        VMInterface.GetLastAsmLoc(vm.vm, stringBuilder, (uint)num3);
        asmloc = Environment.GetEnvironmentVariable("TEMP") + stringBuilder.ToString();
        try
        {
            File.Delete(Environment.GetEnvironmentVariable("TEMP") + "\\temp.c");
        }
        catch (FileNotFoundException)
        {
        }
        IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ErrorStruct)));
        byte* ptr = (byte*)intPtr.ToPointer();
        for (int i = 0; i < Marshal.SizeOf(typeof(ErrorStruct)); i++)
        {
            ptr[i] = 0;
        }
        VMInterface.GetErrors(vm.vm, intPtr);
        ErrorStruct errorStruct = (ErrorStruct)Marshal.PtrToStructure(intPtr, typeof(ErrorStruct));
        Marshal.FreeHGlobal(intPtr);
        foreach (Line line in CodeBox.Lines)
        {
            line.DeleteMarker(COMPILE_ERROR_NUMBER);
        }
        CodeBox.Invalidate();
        if (num2 == -2)
        {
            terminal.Clear();
            flag = false;
        }
        else if (num2 == -1)
        {
            terminal.WriteText("Invalid file selected");
            flag = false;
        }
        else if (num2 > 0)
        {
            flag = false;
            string[] array2 = new string[num2];
            Regex regex = new Regex(":(\\d*):", RegexOptions.IgnoreCase);
            for (int j = 0; j < num2; j++)
            {
                array2[j] = Marshal.PtrToStringAnsi(errorStruct.errors[j]);
                MatchCollection matchCollection = regex.Matches(array2[j]);
                foreach ((int num4, Marker marker) in from Match item in matchCollection
                                                      let num4 = Convert.ToInt32(item.Groups[1].Value)
                                                      let marker = CodeBox.Markers[COMPILE_ERROR_NUMBER]
                                                      select (num4, marker))
                {
                    marker.Number = COMPILE_ERROR_NUMBER;
                    marker.Symbol = COMPILE_ERROR_SYMBOL;
                    Color color = (marker.BackColor = COMPILE_ERROR_COLOR);
                    Color foreColor = color;
                    marker.ForeColor = foreColor;
                    CodeBox.Lines[num4 - 1].AddMarker(marker);
                }
            }
            terminal.Clear();
            for (int k = 0; k < array2.Length - 1; k++)
            {
                terminal.WriteText(array2[k]);
                terminal.WriteText("\r\n");
                if (array2[k].ToUpper().Contains("INVALID") && (array2[k].ToUpper().Contains(".D") || array2[k].ToUpper().Contains(".S")))
                {
                    terminal.WriteText("RIMS does not currently support floating point operations.");
                    terminal.WriteText("\r\n");
                }
            }
            if (array2.Length != 0)
            {
                terminal.WriteText(array2[^1]);
                terminal.WriteText("\r\n");
                if (array2[^1].ToUpper().Contains("INVALID") && (array2[^1].ToUpper().Contains(".D") || array2[^1].ToUpper().Contains(".S")))
                {
                    terminal.WriteText("RIMS does not currently support floating point operations.");
                    terminal.WriteText("\r\n");
                }
            }
        }
        else
        {
            terminal.Clear();
            Run.Enabled = true;
            pIDToolStripMenuItem.Enabled = true;
            exportAssemblyToolStripMenuItem.Enabled = true;
            flag = true;
            ElapsedTime.BackColor = Color.PowderBlue;
        }
        for (int l = 0; l < watches.Count; l++)
        {
            if (VMInterface.GetSymbolIndex(vm.vm, watches[l]) == -1)
            {
                watches.RemoveAt(l);
                l--;
            }
        }
        if (IPSSlider.Value == 1)
        {
            RePopulateSymbolTable();
        }
        else
        {
            SymbolsList.Items.Clear();
            ListViewItem listViewItem = new ListViewItem()
            {
                Text = watch_msg
            };
            SymbolsList.Items.Add(listViewItem);
        }
        Debug.WriteLine(terminal.ShellText.ToUpper());
        if (terminal.ShellText.Contains(@"Could not find include file ""rims.h""", StringComparison.InvariantCultureIgnoreCase))
        {
            terminal.Clear();
            DoCompile();
        }
        return flag;
    }

    public bool DoSave()
    {
        if (SaveCode.FileName == "" && !DoSaveCodeDialogBox())
        {
            return false;
        }
        filename = SaveCode.FileName;
        if (block != null && !block.isReady())
        {
            block.setFileDir(filename[..filename.LastIndexOf("\\")], string.Concat(filename.AsSpan(filename.LastIndexOf("\\") + 1, filename[(filename.LastIndexOf("\\") + 1)..].LastIndexOf(".")), "_extIOcode"));
        }
        else if (block != null)
        {
            block.DoSave();
        }
        if (!code_is_unmodified)
        {
            TabControl.TabPages[0].Text = SaveCode.FileName;
        }
        VMInterface.SetFilename(vm.vm, SaveCode.FileName);
        VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
        VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
        VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
        if (VMInterface.IsRunning(vm.vm) == 0)
        {
            updateEditorState(SaveCode.FileName);
            enableCompileAssemble();
        }
        StreamWriter streamWriter = new StreamWriter(SaveCode.FileName);
        streamWriter.Write(CodeBox.Text);
        streamWriter.Close();
        code_is_unmodified = true;
        updateEditorState(SaveCode.FileName);
        enableCompileAssemble();
        return true;
    }

    private static bool IsBreakpointMarker(Marker e)
    {
        if (e.Number == BREAKPOINT_MARKER_NUMBER)
        {
            return true;
        }
        return false;
    }

    public uint getAssemblyBreak(uint linenumber)
    {
        if (editorState == EditorState.ASM)
        {
            return (uint)((int)linenumber + asm_line_offset - 1);
        }
        return 0u;
    }

    private void CodeBox_MarginClick(object sender, MarginClickEventArgs e)
    {
        if (!e.Margin.IsMarkerMargin || (editorState == EditorState.ASM && asm_line_offset == 0))
        {
            return;
        }
        e.Line.EnsureVisible();
        uint num = (uint)(e.Line.Number + 1);
        if (editorState == EditorState.ASM)
        {
            num = getAssemblyBreak(num);
        }
        List<Marker> markers = e.Line.GetMarkers();
        if (markers.Exists(IsBreakpointMarker))
        {
            e.Line.DeleteMarker(BREAKPOINT_MARKER_NUMBER);
            VMInterface.RemoveBreakpoint(vm.vm, num);
            int num2 = breakpoints.IndexOf(num);
            if (num2 >= 0)
            {
                breakpoints.RemoveAt(num2);
            }
        }
        else
        {
            Marker marker = CodeBox.Markers[BREAKPOINT_MARKER_NUMBER];
            marker.Number = BREAKPOINT_MARKER_NUMBER;
            marker.Symbol = BREAKPOINT_SYMBOL;
            Color color = (marker.BackColor = BREAKPOINT_COLOR);
            Color foreColor = color;
            marker.ForeColor = foreColor;
            e.Line.AddMarker(marker);
            VMInterface.AddBreakpoint(vm.vm, num);
            breakpoints.Add(num);
        }
    }

    private unsafe void Run_Click(object sender, EventArgs e)
    {
        if (!running)
        {
            if (using_testvectors)
            {
                if (testVectorModified)
                {
                    if (MessageBox.Show("You must save the test vector file to proceed, click yes to save and begin code execution.", "Test vector modified", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    saveVector_Click(null, null);
                    if (testVectorModified)
                    {
                        return;
                    }
                }
                runFromInputVectors();
            }
            if (this.dialShown)
            {
                Invoke((Action)delegate
                {
                    var temp_marker = testVectorText.Markers[CUR_LINE_MARKER_NUMBER];
                    temp_marker.Number = CUR_LINE_MARKER_NUMBER;
                    temp_marker.Symbol = CUR_LINE_SYMBOL;
                    Marker marker = temp_marker;
                    Color color = (temp_marker.BackColor = CUR_LINE_COLOR);
                    Color foreColor = color;
                    marker.ForeColor = foreColor;
                    A0.Enabled = false;
                    A1.Enabled = false;
                    A2.Enabled = false;
                    A3.Enabled = false;
                    A4.Enabled = false;
                    A5.Enabled = false;
                    A6.Enabled = false;
                    A7.Enabled = false;
                    for (int i = 0; i < 8; i++)
                    {
                        if (A_Image_Location[i] < 2)
                            A_Image_Location[i] += 2;
                        UpdateA_Images();
                    }
                });
            }
            block?.run();
            vm_terminate = new VM();
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InitStruct)));
            byte* ptr = (byte*)intPtr.ToPointer();
            for (int i = 0; i < Marshal.SizeOf(typeof(InitStruct)); i++)
            {
                ptr[i] = 0;
            }
            VMInterface.Initialize(vm.vm, intPtr);
            InitStruct initStruct = (InitStruct)Marshal.PtrToStructure(intPtr, typeof(InitStruct));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
            IntPtr clock = initStruct.clock;
            breakpoint_step = initStruct.breakpoint_pulse;
            CodeBox.UseBackColor = true;
            CodeBox.IsReadOnly = true;
            CodeBox.BackColor = SystemColors.Control;
            CodeBox.Margins.FoldMarginColor = SystemColors.Control;
            CodeBox.Margins.FoldMarginHighlightColor = SystemColors.Control;
            if (vm.ts != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(vm.ts);
            }
            terminal.Clear();
            SymbolUpdateTimer.Enabled = true;
            vm.ts = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ThreadStruct)));
            ElapsedTime.Text = "0.000 Seconds";
            ElapsedTime.BackColor = Color.Red;
            TimerInterface.timeBeginPeriod(10u);
            VMInterface.SetIPP(vm.vm, ipp);
            Compile.Enabled = false;
            Assemble.Enabled = false;
            SaveBtn.Enabled = false;
            BreakBtn.Enabled = true;
            StepBtn.Enabled = false;
            VMInterface.Run(vm.vm, vm.ts);
            timer_id = TimerInterface.timeSetEvent(50u, 0u, clock, vm.vm, 1u);
            running = true;
            Run.Text = "Terminate";
            pIDToolStripMenuItem.Enabled = false;
            generateTimingDiagramToolStripMenuItem.Enabled = true;
            if (pidEnabled)
            {
                pidBlockDisplay.clear();
                pidSimSystem.start();
            }
        }
        else
        {
            if (this.dialShown)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (A_Image_Location[i] >= 2)
                        A_Image_Location[i] -= 2;
                    //UpdateA_Images();
                }
            }

            block?.run();
            lock (typeof(MainForm))
            {
                TimerInterface.timeKillEvent(timer_id);
                vm_terminate.ts = vm.ts;
                vm_terminate.vm = vm.vm;
                vm.vm = VMInterface.CreateVM();
                VMInterface.SetFilename(vm.vm, filename);
                VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
                VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
            }
            running = false;
            Run.Text = "Run";
            SymbolUpdateTimer.Enabled = false;
            enableCompileAssemble();
            Run.Enabled = false;
            pIDToolStripMenuItem.Enabled = true;
            exportAssemblyToolStripMenuItem.Enabled = false;
            if (runFromInputVectorsThreadIsRunning)
            {
                try
                {
                    testVectorCleanup();
                    runFromInputVectorsThread.Abort();
                }
                catch (PlatformNotSupportedException pe)
                {
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        MessageBox.Show(this, pe.ToString() + "\nExiting...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
            if (editorState == EditorState.C)
            {
                Compile.Enabled = true;
            }
            else if (editorState == EditorState.ASM)
            {
                Assemble.Enabled = true;
            }
            BreakBtn.Enabled = false;
            StepBtn.Enabled = false;
            SaveBtn.Enabled = true;
            CodeBox.IsReadOnly = false;
            CodeBox.BackColor = Color.White;
            CodeBox.Margins.FoldMarginColor = Color.White;
            CodeBox.Markers.DeleteAll();
            ElapsedTime.Text = "0.000 Seconds";
            ElapsedTime.BackColor = Color.Red;
            if (pidEnabled)
            {
                pidSimSystem.stop();
            }
            running = false;
            asm_line_offset = 0;
            breakpoints.Clear();
        }
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        if (running)
        {
            useTestVectors.Enabled = false;
            developCToolStripMenuItem.Enabled = false;
            developASMToolStripMenuItem.Enabled = false;
        }
        else
        {
            if (!waveEnabled && !pidEnabled)
            {
                useTestVectors.Enabled = true;
            }
            else
            {
                useTestVectors.Enabled = false;
            }
            developCToolStripMenuItem.Enabled = true;
            developASMToolStripMenuItem.Enabled = true;
        }
        lock (vmLock)
        {
            UpdateB();
            UpdateUART();
            UpdateTimerBar();
            UpdateDebugOutput();
            UpdateSMAnimation();
            peripherals.UpdateSymbols(vm);

            if (this.dialShown && this.dialKnob != null)
            {
                string s = string.Join("", Convert.ToString(this.dialKnob.Value, 2).PadLeft(8, '0').Reverse());
                //Debug.WriteLine(s);
                for (int i1 = 0; i1 < s.Length; i1++)
                    adjustInputToState($"A{i1}", s[i1] == '1' ? 1 : 0);
                UpdateA_Value();
            }
        }
    }

    public void UpdateB_Images()
    {
        if ((string)B0.Tag == "On")
        {
            B0.Image = B_Images_On[B_Image_Location[0]];
        }
        else
        {
            B0.Image = B_Images_Off[B_Image_Location[0]];
        }
        if ((string)B1.Tag == "On")
        {
            B1.Image = B_Images_On[B_Image_Location[1]];
        }
        else
        {
            B1.Image = B_Images_Off[B_Image_Location[1]];
        }
        if ((string)B2.Tag == "On")
        {
            B2.Image = B_Images_On[B_Image_Location[2]];
        }
        else
        {
            B2.Image = B_Images_Off[B_Image_Location[2]];
        }
        if ((string)B3.Tag == "On")
        {
            B3.Image = B_Images_On[B_Image_Location[3]];
        }
        else
        {
            B3.Image = B_Images_Off[B_Image_Location[3]];
        }
        if ((string)B4.Tag == "On")
        {
            B4.Image = B_Images_On[B_Image_Location[4]];
        }
        else
        {
            B4.Image = B_Images_Off[B_Image_Location[4]];
        }
        if ((string)B5.Tag == "On")
        {
            B5.Image = B_Images_On[B_Image_Location[5]];
        }
        else
        {
            B5.Image = B_Images_Off[B_Image_Location[5]];
        }
        if ((string)B6.Tag == "On")
        {
            B6.Image = B_Images_On[B_Image_Location[6]];
        }
        else
        {
            B6.Image = B_Images_Off[B_Image_Location[6]];
        }
        if ((string)B7.Tag == "On")
        {
            B7.Image = B_Images_On[B_Image_Location[7]];
        }
        else
        {
            B7.Image = B_Images_Off[B_Image_Location[7]];
        }
    }

    public void UpdateB_Sounds()
    {
        try
        {
            if ((string)B0.Tag == "On" && B_Image_Location[0] == 1)
            {
                if (sound_flags[0] == 0)
                {
                    //mciSendString("play Song0 REPEAT", null, 0, IntPtr.Zero);
                    players[0].currentPlaylist.appendItem(players[0].newMedia(Path.Join(Path.GetTempPath(), "song0.mp3")));
                    players[0].controls.play();
                    sound_flags[0] = 1;
                }
            }
            else
            {
                //mciSendString("pause Song0", null, 0, IntPtr.Zero);
                players[0].controls.stop();
                sound_flags[0] = 0;
            }
            if ((string)B1.Tag == "On" && B_Image_Location[1] == 1)
            {
                if (sound_flags[1] == 0)
                {
                    //mciSendString("play Song1 REPEAT", null, 0, IntPtr.Zero);
                    players[1].currentPlaylist.appendItem(players[1].newMedia(Path.Join(Path.GetTempPath(), "song1.mp3")));
                    players[1].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song1", null, 0, IntPtr.Zero);
                players[1].controls.stop();
                sound_flags[1] = 0;
            }
            if ((string)B2.Tag == "On" && B_Image_Location[2] == 1)
            {
                if (sound_flags[2] == 0)
                {
                    //mciSendString("play Song2 REPEAT", null, 0, IntPtr.Zero);
                    players[2].currentPlaylist.appendItem(players[2].newMedia(Path.Join(Path.GetTempPath(), "song2.mp3")));
                    players[2].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song2", null, 0, IntPtr.Zero);
                players[2].controls.stop();
                sound_flags[2] = 0;
            }
            if ((string)B3.Tag == "On" && B_Image_Location[3] == 1)
            {
                if (sound_flags[3] == 0)
                {
                    //mciSendString("play Song3 REPEAT", null, 0, IntPtr.Zero);
                    players[3].currentPlaylist.appendItem(players[3].newMedia(Path.Join(Path.GetTempPath(), "song3.mp3")));
                    players[3].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song3", null, 0, IntPtr.Zero);
                players[3].controls.stop();
                sound_flags[3] = 0;
            }
            if ((string)B4.Tag == "On" && B_Image_Location[4] == 1)
            {
                if (sound_flags[4] == 0)
                {
                    //mciSendString("play Song4 REPEAT", null, 0, IntPtr.Zero);
                    players[4].currentPlaylist.appendItem(players[4].newMedia(Path.Join(Path.GetTempPath(), "song4.mp3")));
                    players[4].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song4", null, 0, IntPtr.Zero);
                players[4].controls.stop();
                sound_flags[4] = 0;
            }
            if ((string)B5.Tag == "On" && B_Image_Location[5] == 1)
            {
                if (sound_flags[5] == 0)
                {
                    //mciSendString("play Song5 REPEAT", null, 0, IntPtr.Zero);
                    players[5].currentPlaylist.appendItem(players[5].newMedia(Path.Join(Path.GetTempPath(), "song5.mp3")));
                    players[5].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song5", null, 0, IntPtr.Zero);
                players[5].controls.stop();
                sound_flags[5] = 0;
            }
            if ((string)B6.Tag == "On" && B_Image_Location[6] == 1)
            {
                if (sound_flags[6] == 0)
                {
                    //mciSendString("play Song6 REPEAT", null, 0, IntPtr.Zero);
                    players[6].currentPlaylist.appendItem(players[6].newMedia(Path.Join(Path.GetTempPath(), "song6.mp3")));
                    players[6].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song6", null, 0, IntPtr.Zero);
                players[6].controls.stop();
                sound_flags[6] = 0;
            }
            if ((string)B7.Tag == "On" && B_Image_Location[7] == 1)
            {
                if (sound_flags[7] == 0)
                {
                    //mciSendString("play Song7 REPEAT", null, 0, IntPtr.Zero);
                    players[7].currentPlaylist.appendItem(players[7].newMedia(Path.Join(Path.GetTempPath(), "song7.mp3")));
                    players[7].controls.play();
                }
            }
            else
            {
                //mciSendString("pause Song7", null, 0, IntPtr.Zero);
                players[7].controls.stop();
                sound_flags[7] = 0;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(owner: this, text: e.ToString(), this.Text);
        }
    }

    public void UpdateB()
    {
        try
        {
            byte pin;
            lock (typeof(MainForm))
            {
                pin = VMInterface.GetPin(vm.vm, Pins.B);
            }
            if (((uint)pin & (true ? 1u : 0u)) != 0 && (string)B0.Tag != "On")
            {
                B0.Tag = "On";
                B0.Image = B_Images_On[B_Image_Location[0]];
            }
            else if ((pin & 1) == 0 && (string)B0.Tag != "Off")
            {
                B0.Tag = "Off";
                B0.Image = B_Images_Off[B_Image_Location[0]];
            }
            if ((pin & 2u) != 0 && (string)B1.Tag != "On")
            {
                B1.Tag = "On";
                B1.Image = B_Images_On[B_Image_Location[1]];
            }
            else if ((pin & 2) == 0 && (string)B1.Tag != "Off")
            {
                B1.Tag = "Off";
                B1.Image = B_Images_Off[B_Image_Location[1]];
            }
            if ((pin & 4u) != 0 && (string)B2.Tag != "On")
            {
                B2.Tag = "On";
                B2.Image = B_Images_On[B_Image_Location[2]];
            }
            else if ((pin & 4) == 0 && (string)B2.Tag != "Off")
            {
                B2.Tag = "Off";
                B2.Image = B_Images_Off[B_Image_Location[2]];
            }
            if ((pin & 8u) != 0 && (string)B3.Tag != "On")
            {
                B3.Tag = "On";
                B3.Image = B_Images_On[B_Image_Location[3]];
            }
            else if ((pin & 8) == 0 && (string)B3.Tag != "Off")
            {
                B3.Tag = "Off";
                B3.Image = B_Images_Off[B_Image_Location[3]];
            }
            if ((pin & 0x10u) != 0 && (string)B4.Tag != "On")
            {
                B4.Tag = "On";
                B4.Image = B_Images_On[B_Image_Location[4]];
            }
            else if ((pin & 0x10) == 0 && (string)B4.Tag != "Off")
            {
                B4.Tag = "Off";
                B4.Image = B_Images_Off[B_Image_Location[4]];
            }
            if ((pin & 0x20u) != 0 && (string)B5.Tag != "On")
            {
                B5.Tag = "On";
                B5.Image = B_Images_On[B_Image_Location[5]];
            }
            else if ((pin & 0x20) == 0 && (string)B5.Tag != "Off")
            {
                B5.Tag = "Off";
                B5.Image = B_Images_Off[B_Image_Location[5]];
            }
            if ((pin & 0x40u) != 0 && (string)B6.Tag != "On")
            {
                B6.Tag = "On";
                B6.Image = B_Images_On[B_Image_Location[6]];
            }
            else if ((pin & 0x40) == 0 && (string)B6.Tag != "Off")
            {
                B6.Tag = "Off";
                B6.Image = B_Images_Off[B_Image_Location[6]];
            }
            if ((pin & 0x80u) != 0 && (string)B7.Tag != "On")
            {
                B7.Tag = "On";
                B7.Image = B_Images_On[B_Image_Location[7]];
            }
            else if ((pin & 0x80) == 0 && (string)B7.Tag != "Off")
            {
                B7.Tag = "Off";
                B7.Image = B_Images_Off[B_Image_Location[7]];
            }
            UpdateB_Sounds();
            UpdateB_Value();
        }
        catch (Exception)
        {
        }
    }

    private void UpdateUART()
    {
        if (VMInterface.IsUARTEnabled(vm.vm) != 0)
        {
            byte uARTRx = VMInterface.GetUARTRx(vm.vm);
            byte uARTTx = VMInterface.GetUARTTx(vm.vm);
            string text = "";
            string text2 = "";
            if (uARTRx != 0)
            {
                text = "'";
                string obj;
                if (uARTRx == 10)
                {
                    obj = text + "\\n";
                }
                else
                {
                    string obj2 = text;
                    char c3 = (char)uARTRx;
                    obj = obj2 + c3;
                }
                text = obj;
                text += "' (";
                for (int num = 7; num >= 0; num--)
                {
                    text += (((uARTRx & (1 << num)) >> num == 1) ? "1" : "0");
                }
                text += ")";
            }
            if (uARTTx != 0)
            {
                text2 = "'";
                string obj3;
                if (uARTTx == 10)
                {
                    obj3 = text2 + "\\n";
                }
                else
                {
                    string obj4 = text2;
                    char c3 = (char)uARTTx;
                    obj3 = obj4 + c3;
                }
                text2 = obj3;
                text2 += "' (";
                for (int num2 = 7; num2 >= 0; num2--)
                {
                    text2 += (((uARTTx & (1 << num2)) >> num2 == 1) ? "1" : "0");
                }
                text2 += ")";
            }
            UARTRxReg.Text = text;
            UARTTxReg.Text = text2;
        }
        if (VMInterface.IsUARTDone(vm.vm) > 0)
        {
            char c = (char)VMInterface.ReceiveFromUART(vm.vm);
            switch (c)
            {
                case '\0':
                    terminal.WriteText("{\\0}");
                    break;
                default:
                    {
                        string text3 = "";
                        text3 += c;
                        terminal.WriteText(text3);
                        break;
                    }
                case '\r':
                    break;
            }
        }
        if (VMInterface.IsUARTReady(vm.vm) > 0 && UARTInBuf.Text.Length > 0)
        {
            char c2 = UARTInBuf.Text[0];
            VMInterface.SendToUART(vm.vm, (byte)c2);
            UARTInBuf.Text = UARTInBuf.Text.Substring(1);
            UARTInBuf.SelectionStart = UARTInBuf.Text.Length;
        }
    }

    private void UpdateTimerBar()
    {
        uint num;
        uint timerValue;
        lock (typeof(MainForm))
        {
            num = VMInterface.GetTimerPeriod(vm.vm) * (INSTR_PER_SEC / 1000u);
            timerValue = VMInterface.GetTimerValue(vm.vm);
        }
        double num2 = (double)num / 100.0;
        if (num != 0)
        {
            num = (uint)((double)num / num2);
            timerValue = (uint)((double)timerValue / num2);
            TimerBar.Value = (int)timerValue;
            TimerBar.m_ProgressBarPercent = true;
        }
        else
        {
            TimerBar.Value = 0;
            TimerBar.m_ProgressBarPercent = true;
        }
        if (VMInterface.IsRunning(vm.vm) != 0)
        {
            uint elapsedCycles = VMInterface.GetElapsedCycles(vm.vm);
            double num3 = elapsedCycles / Convert.ToDouble(VMInterface.GetIPS(vm.vm));
            ElapsedTime.Text = num3.ToString("#0.000") + " Seconds";
        }
        else
        {
            ElapsedTime.Text = "0.000 Seconds";
        }
    }

    private void UpdateDebugOutput()
    {
        uint numDebugCharsWaiting = VMInterface.GetNumDebugCharsWaiting(vm.vm);
        if (numDebugCharsWaiting != 0)
        {
            sb.EnsureCapacity((int)numDebugCharsWaiting);
            VMInterface.GetNextDebugBuffer(vm.vm, sb, numDebugCharsWaiting);
            try
            {
                terminal.WriteText(sb.ToString()[..(int)numDebugCharsWaiting]);
            }
            catch
            {
            }
        }
    }

    private unsafe void UpdateSMAnimation()
    {
        if (!animation_mode || anim_comm == null || !anim_comm.ReadyForUpdates())
        {
            return;
        }
        foreach (KeyValuePair<string, int> item in state_var_map)
        {
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
            intPtr.ToPointer();
            VMInterface.GetSymbol(vm.vm, item.Value, intPtr);
            SymbolRecord symbolRecord = (SymbolRecord)Marshal.PtrToStructure(intPtr, typeof(SymbolRecord));
            Marshal.FreeHGlobal(intPtr);
            uint data = VMInterface.GetData(vm.vm, (ushort)symbolRecord.address, (MemoryWidth)symbolRecord.content_length);
            anim_comm.UpdateState(item.Key, (int)(data + 1));
        }
    }

    public void UpdateA_Images()
    {
        if ((string)A0.Tag == "On")
        {
            A0.Image = A_Images_On[A_Image_Location[0]];
        }
        else
        {
            A0.Image = A_Images_Off[A_Image_Location[0]];
        }
        if ((string)A1.Tag == "On")
        {
            A1.Image = A_Images_On[A_Image_Location[1]];
        }
        else
        {
            A1.Image = A_Images_Off[A_Image_Location[1]];
        }
        if ((string)A2.Tag == "On")
        {
            A2.Image = A_Images_On[A_Image_Location[2]];
        }
        else
        {
            A2.Image = A_Images_Off[A_Image_Location[2]];
        }
        if ((string)A3.Tag == "On")
        {
            A3.Image = A_Images_On[A_Image_Location[3]];
        }
        else
        {
            A3.Image = A_Images_Off[A_Image_Location[3]];
        }
        if ((string)A4.Tag == "On")
        {
            A4.Image = A_Images_On[A_Image_Location[4]];
        }
        else
        {
            A4.Image = A_Images_Off[A_Image_Location[4]];
        }
        if ((string)A5.Tag == "On")
        {
            A5.Image = A_Images_On[A_Image_Location[5]];
        }
        else
        {
            A5.Image = A_Images_Off[A_Image_Location[5]];
        }
        if ((string)A6.Tag == "On")
        {
            A6.Image = A_Images_On[A_Image_Location[6]];
        }
        else
        {
            A6.Image = A_Images_Off[A_Image_Location[6]];
        }
        if ((string)A7.Tag == "On")
        {
            A7.Image = A_Images_On[A_Image_Location[7]];
        }
        else
        {
            A7.Image = A_Images_Off[A_Image_Location[7]];
        }
    }

    private void A7_Click(object sender, EventArgs e)
    {
        if (A7.Enabled)
        {
            if ((string)A7.Tag == "Off")
            {
                A7.Image = A_Images_On[A_Image_Location[7]];
                A7.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A7, 1);
            }
            else
            {
                A7.Image = A_Images_Off[A_Image_Location[7]];
                A7.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A7, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A7", VMInterface.GetPin(vm.vm, Pins.A7));
            }
        }
    }

    private void A6_Click(object sender, EventArgs e)
    {
        if (A6.Enabled)
        {
            if ((string)A6.Tag == "Off")
            {
                A6.Image = A_Images_On[A_Image_Location[6]];
                A6.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A6, 1);
            }
            else
            {
                A6.Image = A_Images_Off[A_Image_Location[6]];
                A6.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A6, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A6", VMInterface.GetPin(vm.vm, Pins.A6));
            }
        }
    }

    private void A5_Click(object sender, EventArgs e)
    {
        if (A5.Enabled)
        {
            if ((string)A5.Tag == "Off")
            {
                A5.Image = A_Images_On[A_Image_Location[5]];
                A5.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A5, 1);
            }
            else
            {
                A5.Image = A_Images_Off[A_Image_Location[5]];
                A5.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A5, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A5", VMInterface.GetPin(vm.vm, Pins.A5));
            }
        }
    }

    private void A4_Click(object sender, EventArgs e)
    {
        if (A4.Enabled)
        {
            if ((string)A4.Tag == "Off")
            {
                A4.Image = A_Images_On[A_Image_Location[4]];
                A4.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A4, 1);
            }
            else
            {
                A4.Image = A_Images_Off[A_Image_Location[4]];
                A4.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A4, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A4", VMInterface.GetPin(vm.vm, Pins.A4));
            }
        }
    }

    private void A3_Click(object sender, EventArgs e)
    {
        if (A3.Enabled)
        {
            if ((string)A3.Tag == "Off")
            {
                A3.Image = A_Images_On[A_Image_Location[3]];
                A3.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A3, 1);
            }
            else
            {
                A3.Image = A_Images_Off[A_Image_Location[3]];
                A3.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A3, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A3", VMInterface.GetPin(vm.vm, Pins.A3));
            }
        }
    }

    private void A2_Click(object sender, EventArgs e)
    {
        if (A2.Enabled)
        {
            if ((string)A2.Tag == "Off")
            {
                A2.Image = A_Images_On[A_Image_Location[2]];
                A2.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A2, 1);
            }
            else
            {
                A2.Image = A_Images_Off[A_Image_Location[2]];
                A2.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A2, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A2", VMInterface.GetPin(vm.vm, Pins.A2));
            }
        }
    }

    private void A1_Click(object sender, EventArgs e)
    {
        if (A1.Enabled)
        {
            if ((string)A1.Tag == "Off")
            {
                A1.Image = A_Images_On[A_Image_Location[1]];
                A1.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A1, 1);
            }
            else
            {
                A1.Image = A_Images_Off[A_Image_Location[1]];
                A1.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A1, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A1", VMInterface.GetPin(vm.vm, Pins.A1));
            }
        }
    }

    private void A0_Click(object sender, EventArgs e)
    {
        if (A0.Enabled)
        {
            if ((string)A0.Tag == "Off")
            {
                A0.Image = A_Images_On[A_Image_Location[0]];
                A0.Tag = "On";
                VMInterface.SetPin(vm.vm, Pins.A0, 1);
            }
            else
            {
                A0.Image = A_Images_Off[A_Image_Location[0]];
                A0.Tag = "Off";
                VMInterface.SetPin(vm.vm, Pins.A0, 0);
            }
            UpdateA_Value();
            if (block != null)
            {
                block.pinClicked("A0", VMInterface.GetPin(vm.vm, Pins.A0));
            }
        }
    }

    public void UpdateA_Value()
    {
        byte pin = VMInterface.GetPin(vm.vm, Pins.A);
        string text = $"{pin:X}";
        text = ((text.Length != 1) ? ("= x" + text) : ("= x0" + text));
        string arg = pin.ToString();
        arg = "A = " + arg;
        AValue_Dec.Text = arg;
        AValue_Hex.Text = text;
        AValue_Dec.Update();
        AValue_Hex.Update();
    }

    public void UpdateB_Value()
    {
        byte pin = VMInterface.GetPin(vm.vm, Pins.B);
        string text = $"{pin:X}";
        text = ((text.Length != 1) ? ("= x" + text) : ("= x0" + text));
        string arg = pin.ToString();
        arg = "B = " + arg;
        BValue_Dec.Text = arg;
        BValue_Hex.Text = text;
        if (sevenSegment != null)
            sevenSegment.CustomPattern = pin;
    }

    private void IPSSlider_Scroll(object sender, EventArgs e)
    {
        int num;
        switch (IPSSlider.Value)
        {
            case 1:
                num = 1;
                SymbolUpdateTimer.Enabled = true;
                SymbolsList.Enabled = true;
                RePopulateSymbolTable();
                break;
            case 2:
                num = 125;
                if (SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = false;
                }
                break;
            case 3:
                num = DEFAULT_IPP;
                if (SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = false;
                }
                break;
            case 4:
                num = 500;
                if (SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = false;
                }
                break;
            case 5:
                num = 1000000;
                if (SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = false;
                }
                break;
            default:
                num = DEFAULT_IPP;
                if (SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = false;
                }
                break;
        }
        ipp = (uint)num;
        VMInterface.SetIPP(vm.vm, (uint)num);
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        About about = new About();
        about.ShowDialog();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (DoSaveCodeDialogBox())
        {
            VMInterface.SetFilename(vm.vm, SaveCode.FileName);
            VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
            if (editorState == EditorState.C)
            {
                Compile.Enabled = true;
            }
            else if (editorState == EditorState.ASM)
            {
                Assemble.Enabled = true;
            }
            StreamWriter streamWriter = new StreamWriter(SaveCode.FileName);
            streamWriter.Write(CodeBox.Text);
            streamWriter.Close();
            code_is_unmodified = true;
        }
    }

    private bool DoSaveCodeDialogBox()
    {
        if (editorState == EditorState.C)
        {
            SaveCode.Filter = "RIMS C Code (*.c)|*.c";
        }
        else if (editorState == EditorState.ASM)
        {
            SaveCode.Filter = "ASM files (*.s)|*.s";
        }
        if (SaveCode.ShowDialog() == DialogResult.OK)
        {
            if (vm.ts != IntPtr.Zero)
            {
                VMInterface.SetFilename(vm.vm, SaveCode.FileName);
            }
            TabControl.TabPages[0].Text = SaveCode.FileName;
            code_is_unmodified = true;
            return true;
        }
        return false;
    }

    private void SymbolUpdateTimer_Tick(object sender, EventArgs e)
    {
        _ = vm.vm;
        try
        {
            bool flag = false;
            bool flag2 = false;
            lock (vmLock)
            {
                flag = VMInterface.IsBroken(vm.vm) == 1;
                flag2 = VMInterface.IsRunning(vm.vm) == 1;
            }
            if (flag2)
            {
                Run.Text = "Terminate";
                pIDToolStripMenuItem.Enabled = false;
                SaveBtn.Enabled = false;
                BreakBtn.Enabled = true;
                if (flag)
                {
                    BreakBtn.Text = "Continue";
                    StepBtn.Enabled = true;
                    ElapsedTime.BackColor = Color.Yellow;
                    if (pidEnabled)
                    {
                        pidSimSystem.pause();
                    }
                }
                else
                {
                    BreakBtn.Text = "Break";
                    StepBtn.Enabled = false;
                    ElapsedTime.BackColor = Color.LawnGreen;
                    if (pidEnabled)
                    {
                        pidSimSystem.unpause();
                    }
                }
            }
            else if (running)
            {
                running = false;
                ElapsedTime.BackColor = Color.Red;
                BreakBtn.Enabled = false;
                SaveBtn.Enabled = true;
                CodeBox.IsReadOnly = false;
                CodeBox.BackColor = Color.White;
                CodeBox.Margins.FoldMarginColor = Color.White;
                enableCompileAssemble();
                Run.Enabled = true;
                pIDToolStripMenuItem.Enabled = true;
                exportAssemblyToolStripMenuItem.Enabled = true;
                Run.Text = "Run";
                BreakBtn.Text = "Break";
                StepBtn.Enabled = false;
                if (pidEnabled)
                {
                    MessageBox.Show("WAT1");
                    pidSimSystem.stop();
                }
                if (is_line_highlighted && old_line_highlighted > 0)
                {
                    CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                    is_line_highlighted = false;
                    old_line_highlighted = -1;
                }
                asm_line_offset = 0;
                breakpoints.Clear();
                MessageBox.Show("End of program reached.");
            }
            if (flag || IPSSlider.Value == 1)
            {
                if (!SymbolsList.Enabled)
                {
                    SymbolsList.Enabled = true;
                    RePopulateSymbolTable();
                }
                if (!AddSymbol.Enabled)
                {
                    AddSymbol.Enabled = true;
                }
                if (!RemoveSymbol.Enabled)
                {
                    RemoveSymbol.Enabled = true;
                }
                RecalculateSymbolValues();
            }
            if (SymbolsList.Enabled && !flag && IPSSlider.Value != 1)
            {
                SymbolsList.Enabled = false;
            }
            if (AddSymbol.Enabled && !flag && IPSSlider.Value != 1)
            {
                AddSymbol.Enabled = false;
            }
            if (RemoveSymbol.Enabled && !flag && IPSSlider.Value != 1)
            {
                RemoveSymbol.Enabled = false;
            }
            if (!flag && IPSSlider.Value != 1 && (SymbolsList.Items.Count != 1 || SymbolsList.Items[0].Text != watch_msg))
            {
                SymbolsList.Items.Clear();
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = watch_msg;
                SymbolsList.Items.Add(listViewItem);
            }
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TagStruct)));
            VMInterface.GetLine(vm.vm, intPtr);
            TagStruct tagStruct = (TagStruct)Marshal.PtrToStructure(intPtr, typeof(TagStruct));
            Marshal.FreeHGlobal(intPtr);
            int num = tagStruct.line - 1;
            int aline = tagStruct.aline;
            int num2 = aline - asm_line_offset;
            string file = tagStruct.file;
            if (ASMWinOpened)
            {
                VMInterface.SetStepModeVM(vm.vm, AStep: true);
            }
            else
            {
                VMInterface.SetStepModeVM(vm.vm, AStep: false);
            }
            if (editorState == EditorState.C)
            {
                VMInterface.SetASMMode(vm.vm, is_asm_mode: false);
            }
            else
            {
                VMInterface.SetASMMode(vm.vm, is_asm_mode: true);
            }
            if (is_line_highlighted && !flag && IPSSlider.Value != 1)
            {
                CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                is_line_highlighted = false;
                old_line_highlighted = -1;
            }
            if (flag)
            {
                if (editorState == EditorState.ASM)
                {
                    if (num2 < 0 || num2 >= CodeBox.Lines.Count)
                    {
                        return;
                    }
                    if (ASMWinOpened)
                    {
                        SetMarkers(aline);
                    }
                    if (num2 != old_line_highlighted)
                    {
                        Marker marker = CodeBox.Markers[CUR_LINE_MARKER_NUMBER];
                        marker.Number = CUR_LINE_MARKER_NUMBER;
                        marker.Symbol = CUR_LINE_SYMBOL;
                        Color color = (marker.BackColor = CUR_LINE_COLOR);
                        Color foreColor = color;
                        marker.ForeColor = foreColor;
                        if (old_line_highlighted != -1)
                        {
                            CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                        }
                        CodeBox.Lines[num2].AddMarker(marker);
                        if (VMInterface.IsRunning(vm.vm) == 1)
                        {
                            CodeBox.Caret.Goto(CodeBox.Lines[num2].StartPosition);
                            CodeBox.Caret.EnsureVisible();
                        }
                        old_line_highlighted = num2;
                        is_line_highlighted = true;
                    }
                }
                else
                {
                    if (num < 0 || num >= CodeBox.Lines.Count)
                    {
                        return;
                    }
                    if (ASMWinOpened)
                    {
                        SetMarkers(aline);
                    }
                    if (num != old_line_highlighted)
                    {
                        Marker marker2 = CodeBox.Markers[CUR_LINE_MARKER_NUMBER];
                        marker2.Number = CUR_LINE_MARKER_NUMBER;
                        marker2.Symbol = CUR_LINE_SYMBOL;
                        Color color = (marker2.BackColor = CUR_LINE_COLOR);
                        Color foreColor2 = color;
                        marker2.ForeColor = foreColor2;
                        if (old_line_highlighted != -1)
                        {
                            CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                        }
                        CodeBox.Lines[num].AddMarker(marker2);
                        if (VMInterface.IsRunning(vm.vm) == 1)
                        {
                            CodeBox.Caret.Goto(CodeBox.Lines[num].StartPosition);
                            CodeBox.Caret.EnsureVisible();
                        }
                        old_line_highlighted = num;
                        is_line_highlighted = true;
                    }
                }
            }
            else
            {
                if (IPSSlider.Value != 1 || !flag2)
                {
                    return;
                }
                if (editorState == EditorState.ASM)
                {
                    if (num2 >= 0 && num2 < CodeBox.Lines.Count)
                    {
                        Marker marker3 = CodeBox.Markers[CUR_LINE_MARKER_NUMBER];
                        marker3.Number = CUR_LINE_MARKER_NUMBER;
                        marker3.Symbol = CUR_LINE_SYMBOL;
                        Color color = (marker3.BackColor = CUR_LINE_COLOR);
                        Color foreColor3 = color;
                        marker3.ForeColor = foreColor3;
                        if (old_line_highlighted != -1)
                        {
                            CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                        }
                        CodeBox.Lines[num2].AddMarker(marker3);
                        CodeBox.Caret.Goto(CodeBox.Lines[num2].StartPosition);
                        CodeBox.Caret.EnsureVisible();
                        old_line_highlighted = num2;
                        is_line_highlighted = true;
                    }
                }
                else if (file.ToUpper().EndsWith("\\TEMP.C\"") && num >= 0 && num < CodeBox.Lines.Count)
                {
                    Marker marker4 = CodeBox.Markers[CUR_LINE_MARKER_NUMBER];
                    marker4.Number = CUR_LINE_MARKER_NUMBER;
                    marker4.Symbol = CUR_LINE_SYMBOL;
                    Color color = (marker4.BackColor = CUR_LINE_COLOR);
                    Color foreColor4 = color;
                    marker4.ForeColor = foreColor4;
                    if (old_line_highlighted != -1)
                    {
                        CodeBox.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                    }
                    CodeBox.Lines[num].AddMarker(marker4);
                    CodeBox.Caret.Goto(CodeBox.Lines[num].StartPosition);
                    CodeBox.Caret.EnsureVisible();
                    old_line_highlighted = num;
                    is_line_highlighted = true;
                }
            }
        }
        catch (NullReferenceException)
        {
        }
        catch (Exception)
        {
        }
    }

    private unsafe void RecalculateSymbolValues()
    {
        lock (SymbolsList)
        {
            for (int i = 0; i < SymbolsList.Items.Count; i++)
            {
                int which = (int)SymbolsList.Items[i].Tag;
                IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
                intPtr.ToPointer();
                VMInterface.GetSymbol(vm.vm, which, intPtr);
                SymbolRecord symbolRecord = (SymbolRecord)Marshal.PtrToStructure(intPtr, typeof(SymbolRecord));
                Marshal.FreeHGlobal(intPtr);
                string text = ((int)VMInterface.GetData(vm.vm, (ushort)symbolRecord.address, (MemoryWidth)symbolRecord.content_length)).ToString();
                if (text != SymbolsList.Items[i].SubItems[1].Text)
                {
                    SymbolsList.Items[i].SubItems[1].Text = text;
                }
            }
        }
    }

    private unsafe void RePopulateSymbolTable()
    {
        SymbolsList.Items.Clear();
        for (int i = 0; i < watches.Count; i++)
        {
            int symbolIndex = VMInterface.GetSymbolIndex(vm.vm, watches[i]);
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SymbolRecord)));
            intPtr.ToPointer();
            VMInterface.GetSymbol(vm.vm, symbolIndex, intPtr);
            SymbolRecord symbolRecord = (SymbolRecord)Marshal.PtrToStructure(intPtr, typeof(SymbolRecord));
            Marshal.FreeHGlobal(intPtr);
            if (symbolRecord.in_data_segment == 1)
            {
                string name = symbolRecord.name;
                ListViewItem listViewItem = new ListViewItem(name);
                listViewItem.SubItems.Add(((int)VMInterface.GetData(vm.vm, (ushort)symbolRecord.address, (MemoryWidth)symbolRecord.content_length)).ToString());
                listViewItem.Tag = symbolIndex;
                SymbolsList.Items.Add(listViewItem);
            }
        }
    }

    private void AddSymbol_Click(object sender, EventArgs e)
    {
        AddSymbolWindow addSymbolWindow = new AddSymbolWindow();
        if (addSymbolWindow.ShowDialog() != DialogResult.OK)
        {
            return;
        }
        List<string> list = (List<string>)addSymbolWindow.Tag;
        for (int i = 0; i < list.Count; i++)
        {
            if (!watches.Contains(list[i]))
            {
                watches.Add(list[i]);
            }
        }
        RePopulateSymbolTable();
    }

    private void ClearOutputBuffer_Click(object sender, EventArgs e)
    {
        terminal.Clear();
    }

    private void RemoveSymbol_Click(object sender, EventArgs e)
    {
        lock (SymbolsList)
        {
            for (int i = 0; i < SymbolsList.SelectedItems.Count; i++)
            {
                try
                {
                    if (watches.Contains((string)SymbolsList.SelectedItems[i].Tag))
                    {
                        watches.Remove((string)SymbolsList.SelectedItems[i].Tag);
                    }
                }
                catch (InvalidCastException)
                {
                    watches.Clear();
                }
            }
        }
        RePopulateSymbolTable();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void OpenFile_Click(object sender, EventArgs e)
    {
        Open_Click(sender, e);
    }

    private void OpenSample_Click(object sender, EventArgs e)
    {
        Samples samples = new Samples();
        samples.ShowDialog();
        string resourceName = samples.GetResourceName();
        if (resourceName.Length == 0)
        {
            return;
        }
        string name = "RIMS_V2.resources." + resourceName;
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(executingAssembly.GetManifestResourceStream(name));
        }
        catch (ArgumentNullException)
        {
            MessageBox.Show("Couldn't find sample");
            return;
        }
        string value = streamReader.ReadToEnd();
        string text = documents_directory + resourceName;
        StreamWriter streamWriter = new StreamWriter(text);
        streamWriter.Write(value);
        streamWriter.Close();
        CodeBox.Text = value;
        generateTimingDiagramToolStripMenuItem.Enabled = false;
        lock (typeof(MainForm))
        {
            TimerInterface.timeKillEvent(timer_id);
            vm.Dispose();
            vm.vm = VMInterface.CreateVM();
            VMInterface.SetFilename(vm.vm, text);
            VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
        }
        filename = text;
        StreamReader streamReader2 = new StreamReader(text);
        CodeBox.IsReadOnly = false;
        CodeBox.Text = streamReader2.ReadToEnd();
        streamReader2.Close();
        code_is_unmodified = true;
        just_opened = true;
        modified_since_opened = false;
        TabControl.TabPages[0].Text = text;
        CodeBox.IsReadOnly = false;
        UARTRxReg.Text = "UART Rx Register";
        UARTTxReg.Text = "UART Tx Register";
        Run.Text = "Run";
        SaveBtn.Enabled = true;
        updateEditorState(resourceName);
        enableCompileAssemble();
        Run.Enabled = false;
        exportAssemblyToolStripMenuItem.Enabled = false;
        BreakBtn.Enabled = false;
        StepBtn.Enabled = false;
        SaveCode.FileName = text;
        lock (SymbolsList)
        {
            SymbolsList.Items.Clear();
            if (IPSSlider.Value != 1)
            {
                ListViewItem listViewItem = new ListViewItem
                {
                    Text = watch_msg
                };
                SymbolsList.Items.Add(listViewItem);
            }
        }
        running = false;
        ElapsedTime.BackColor = Color.Red;
        CodeBox.BackColor = Color.White;
        CodeBox.Margins.FoldMarginColor = Color.White;
        CodeBox.Margins.FoldMarginHighlightColor = Color.White;
        ElapsedTime.Text = "0.000 Seconds";
        watches.Clear();
        if (File.Exists(asmloc))
        {
            File.Delete(asmloc);
        }
        name = "RIMS_V2.resources." + resourceName.Replace(".c", "_tv.txt");
        try
        {
            streamReader = new StreamReader(executingAssembly.GetManifestResourceStream(name));
        }
        catch (ArgumentNullException)
        {
            recentlySaved = true;
            testVectorModified = false;
            return;
        }
        testVectorText.Text = streamReader.ReadToEnd();
        streamReader.Close();
        recentlySaved = true;
        testVectorModified = false;
    }

    private void SampleButton_Click(object sender, EventArgs e)
    {
        OpenSample_Click(sender, e);
    }

    private void BreakBtn_Click(object sender, EventArgs e)
    {
        if (VMInterface.IsBroken(vm.vm) == 0)
        {
            VMInterface.Break(vm.vm);
            BreakBtn.Text = "Continue";
            if (pidEnabled)
            {
                pidSimSystem.pause();
            }
            return;
        }
        VMInterface.Step(vm.vm);
        VMInterface.SetUnBroken(vm.vm);
        if (pidEnabled)
        {
            pidSimSystem.unpause();
        }
        BreakBtn.Text = "Break";
    }

    private void StepBtn_Click(object sender, EventArgs e)
    {
        VMInterface.AtBreakpoint(vm.vm);
        VMInterface.Step(vm.vm);
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        DoSave();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
        testVectorText.Text = "b00000000\r\nwait 1 s\r\n3\r\nwait 1 s\r\nassert 1\r\nassert b00000001\r\nassert 0x01\r\nwait 0.5 s\r\nb00000010\r\nwait 3000 ms\r\nb11111101\r\nwait 200 ms\r\nb00010101\r\nwait 500 ms\r\nb11000011\r\nwait 2 s\r\nb01000010\r\nwait 1 s\r\n0xFF\r\nwait 1 s\r\ngeneratetd";
        testVectorModified = false;
        recentlySaved = true;
        if (block != null)
        {
            block.Close();
            block = null;
        }
        filename = "";
        if (editorState == EditorState.C)
        {
            newCCode();
        }
        else if (editorState == EditorState.ASM)
        {
            newASMCode();
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DoSave();
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CodeBox.UndoRedo.Undo();
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CodeBox.UndoRedo.Redo();
    }

    private void findToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CodeBox.FindReplace.ShowFind();
    }

    private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CodeBox.FindReplace.ShowReplace();
    }

    private void terminal_TextChanged(object sender, CommandEnteredEventArgs e)
    {
        string command = e.Command;
        command += "\r\n";
        char[] data = command.ToCharArray();
        VMInterface.SendToInput(vm.vm, data, (uint)command.Length);
    }

    private void regressionTestToolStripMenuItem_Click(object sender, EventArgs e)
    {
        RegressionTestDialog regressionTestDialog = new RegressionTestDialog();
        regressionTestDialog.ShowDialog();
    }

    private void reportBugsFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Feedback feedback = new Feedback();
        feedback.Show();
    }

    private void helpOnlineToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = "https://www.programmingembeddedsystems.com/RITools/help/"
        });
    }

    private void licensToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyValidation keyValidation = new KeyValidation();
        keyValidation.ShowDialog();
    }

    private void outputTypeMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
        ContextMenuStrip contextMenuStrip = (ContextMenuStrip)sender;
        PictureBox pictureBox = (PictureBox)contextMenuStrip.SourceControl;
        int num = Convert.ToInt32(pictureBox.Name[1]) - 48;
        B_Image_Location[num] = ((!(e.ClickedItem.Text == "LED")) ? 1 : 0);
        UpdateA_Images();
        UpdateA_Value();
        UpdateB_Images();
    }

    private void inputTypeMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
        ContextMenuStrip contextMenuStrip = (ContextMenuStrip)sender;
        PictureBox pictureBox = (PictureBox)contextMenuStrip.SourceControl;
        int num = Convert.ToInt32(pictureBox.Name[1]) - 48;
        A_Image_Location[num] = ((!(e.ClickedItem.Text == "Switch")) ? 1 : 0);
        UpdateA_Images();
        UpdateA_Value();
    }

    private void setAllAsSwitchesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (waveEnabled)
        {
            offToolStripMenuItem_Click(offToolStripMenuItem, null);
        }
        for (int i = 0; i < 8; i++)
        {
            if (A_Image_Location[i] >= 2)
            {
                A_Image_Location[i] = 2;
            }
            else
            {
                A_Image_Location[i] = 0;
            }
            UpdateA_Images();
        }
    }

    private void setAllButtonsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (waveEnabled)
        {
            offToolStripMenuItem_Click(offToolStripMenuItem, null);
        }
        for (int i = 0; i < 8; i++)
        {
            if (A_Image_Location[i] >= 2)
            {
                A_Image_Location[i] = 3;
            }
            else
            {
                A_Image_Location[i] = 1;
            }
            UpdateA_Images();
        }
    }

    private void setAllLEDsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < 8; i++)
        {
            B_Image_Location[i] = 0;
            UpdateB_Images();
        }
    }

    private void setAllSpeakersToolStripMenuItem_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < 8; i++)
        {
            B_Image_Location[i] = 1;
            UpdateB_Images();
        }
    }

    private void RetOpenedfn(bool asmop)
    {
        ASMWinOpened = asmop;
    }

    private void viewAssemblyToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
        if (!ASMWinOpened)
        {
            if ((Compile.Enabled || running || Assemble.Enabled) && File.Exists(asmloc))
            {
                ASMView aSMView = new ASMView(asmloc);
                SetMarkers = (SetValueMarker)Delegate.Combine(SetMarkers, new SetValueMarker(aSMView.SetMarkers));
                CompileUpdate = (CompileUpdateChk)Delegate.Combine(CompileUpdate, new CompileUpdateChk(aSMView.CompileUpdate));
                aSMView.RetOpened = RetOpenedfn;
                ASMWinOpened = true;
                aSMView.Show();
            }
            else
            {
                MessageBox.Show("Please compile first.");
            }
        }
    }

    private void exportAssemblyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Assembly File|*.s";
        saveFileDialog.Title = "Save Assembly File";
        saveFileDialog.ShowDialog();
        if (saveFileDialog.FileName != "")
        {
            StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
            TextReader textReader = new StreamReader(asmloc);
            for (string text = textReader.ReadLine(); text != null; text = textReader.ReadLine())
            {
                streamWriter.Write(text);
                streamWriter.Write("\n");
            }
            streamWriter.Close();
        }
    }

    private void enableNestedInterruptsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        nested_interrupts_enabled = enableNestedInterruptsToolStripMenuItem.Checked;
        VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
    }

    private void updateEditorState(string filename)
    {
        if (filename.EndsWith(".s"))
        {
            editorState = EditorState.ASM;
            Compile.Visible = false;
            Assemble.Visible = true;
            developCToolStripMenuItem.Checked = false;
            developASMToolStripMenuItem.Checked = true;
            viewAssemblyToolStripMenuItem.Enabled = false;
        }
        else if (filename.EndsWith(".c"))
        {
            editorState = EditorState.C;
            Assemble.Visible = false;
            Compile.Visible = true;
            developCToolStripMenuItem.Checked = true;
            developASMToolStripMenuItem.Checked = false;
            viewAssemblyToolStripMenuItem.Enabled = true;
        }
    }

    private void enableCompileAssemble()
    {
        if (editorState == EditorState.C)
        {
            Assemble.Enabled = false;
            Assemble.Visible = false;
            Compile.Enabled = true;
            Compile.Visible = true;
            developCToolStripMenuItem.Checked = true;
            developASMToolStripMenuItem.Checked = false;
            viewAssemblyToolStripMenuItem.Enabled = true;
        }
        else if (editorState == EditorState.ASM)
        {
            Assemble.Enabled = true;
            Assemble.Visible = true;
            Compile.Enabled = false;
            Compile.Visible = false;
            developCToolStripMenuItem.Checked = false;
            developASMToolStripMenuItem.Checked = true;
            viewAssemblyToolStripMenuItem.Enabled = false;
        }
    }

    private void Assemble_Click(object sender, EventArgs e)
    {
        if (SaveCode.FileName.Length == 0)
        {
            MessageBox.Show(OpenFirstMessage, "Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }
        if (!code_is_unmodified)
        {
            StreamWriter streamWriter = new StreamWriter(SaveCode.FileName);
            streamWriter.Write(CodeBox.Text);
            streamWriter.Close();
            TabControl.TabPages[0].Text = SaveCode.FileName;
            code_is_unmodified = true;
        }
        Assemble.Enabled = false;
        string location = Assembly.GetExecutingAssembly().Location;
        string[] array = location.Split('\\');
        location = "";
        uint num = 0u;
        while (array.Length > 1 && num < array.Length - 1)
        {
            location += array[num];
            location += "\\";
            num++;
        }
        VMInterface.SetBaseDirectory(vm.vm, location);
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.resources.ASMtemplate.c"));
        }
        catch (ArgumentNullException)
        {
            MessageBox.Show("couldn't find ASMtemplate.c");
            return;
        }
        string value = streamReader.ReadToEnd();
        string path = documents_directory + "ASMtemplate.c";
        StreamWriter streamWriter2 = new StreamWriter(path);
        streamWriter2.Write(value);
        streamWriter2.Close();
        streamReader.Close();
        VMInterface.SetFilename(vm.vm, path);
        if (!DoCompile())
        {
            Assemble.Enabled = true;
            return;
        }
        Run.Enabled = false;
        exportAssemblyToolStripMenuItem.Enabled = false;
        string text = "";
        StreamReader streamReader2 = new StreamReader(asmloc);
        string text2 = streamReader2.ReadLine();
        bool flag = false;
        int num2 = 0;
        if (CodeBox.Text.Contains("main:"))
        {
            flag = true;
        }
        while (text2 != null && !flag)
        {
            if (text2.Length > 0 && text2[text2.Length - 1] != '\n')
            {
                text2 += "\n";
            }
            if (text2.Contains("main:"))
            {
                text += text2;
                num2++;
                for (int i = 0; i < 8; i++)
                {
                    text2 = streamReader2.ReadLine();
                    num2++;
                    if (text2.Length > 0)
                    {
                        if (text2[text2.Length - 1] != '\n')
                        {
                            text2 += "\n";
                        }
                        text += text2;
                    }
                }
                asm_line_offset = num2;
                text = text + CodeBox.Text + "\n.text\n";
            }
            else
            {
                text += text2;
            }
            text2 = streamReader2.ReadLine();
            num2++;
        }
        while (text2 != null && flag)
        {
            if (text2.Length > 0 && text2[^1] != '\n')
            {
                text2 += "\n";
            }
            if (text2.Contains(".end scan"))
            {
                text += text2;
                num2++;
                while (!text2.Contains(".globl DYNAMIC_MEMORY"))
                {
                    text2 = streamReader2.ReadLine();
                }
                asm_line_offset = num2;
                string text3 = text;
                text = text3 + CodeBox.Text + "\n.text\n" + text2 + "\n";
            }
            else
            {
                text += text2;
            }
            text2 = streamReader2.ReadLine();
            num2++;
        }
        streamReader2.Close();
        StreamWriter streamWriter3 = new StreamWriter(asmloc);
        streamWriter3.Write(text);
        streamWriter3.Close();
        if (VMInterface.Assemble(vm.vm) == 0)
        {
            terminal.Clear();
            Run.Enabled = true;
            pIDToolStripMenuItem.Enabled = true;
            exportAssemblyToolStripMenuItem.Enabled = editorState == EditorState.C;
            ElapsedTime.BackColor = Color.PowderBlue;
            for (int j = 0; j < watches.Count; j++)
            {
                if (VMInterface.GetSymbolIndex(vm.vm, watches[j]) == -1)
                {
                    watches.RemoveAt(j);
                    j--;
                }
            }
            if (IPSSlider.Value == 1)
            {
                RePopulateSymbolTable();
            }
            else
            {
                SymbolsList.Items.Clear();
                ListViewItem listViewItem = new ListViewItem
                {
                    Text = watch_msg
                };
                SymbolsList.Items.Add(listViewItem);
            }
            Assemble.Enabled = true;
            Run.Text = "Run";
            Run.Enabled = true;
            pIDToolStripMenuItem.Enabled = true;
            exportAssemblyToolStripMenuItem.Enabled = editorState == EditorState.C;
            return;
        }
        Run.Enabled = false;
        pIDToolStripMenuItem.Enabled = true;
        exportAssemblyToolStripMenuItem.Enabled = false;
        Assemble.Enabled = true;
        terminal.Clear();
        try
        {
            StreamReader streamReader3 = new StreamReader(Environment.GetEnvironmentVariable("TEMP") + "\\error.txt");
            string text4 = streamReader3.ReadToEnd();
            Match match = Regex.Match(text4, "(Invalid instruction .+ on line )(\\d+)");
            if (match.Success)
            {
                text4 = match.Groups[1].Value + (int.Parse(match.Groups[2].Value) - asm_line_offset) + "  " + match.Groups[2].Value;
                if (match.Groups[1].Value.Contains(".d") || match.Groups[1].Value.Contains(".s"))
                {
                    text4 += "\nRIMS does not currently support floating point operations.";
                }
            }
            streamReader3.Close();
            terminal.WriteText(text4);
        }
        catch (FileNotFoundException)
        {
        }
    }

    private void newCCode()
    {
        lock (typeof(MainForm))
        {
            generateTimingDiagramToolStripMenuItem.Enabled = false;
            TimerInterface.timeKillEvent(timer_id);
            vm.Dispose();
            vm.vm = VMInterface.CreateVM();
            VMInterface.SetFilename(vm.vm, OpenCode.FileName);
            VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
        }
        CodeBox.IsReadOnly = false;
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.resources.RIMS_sample_code.c");
        byte[] array = new byte[4096];
        if (manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length) != manifestResourceStream.Length)
        {
            CodeBox.Text = "";
        }
        else
        {
            CodeBox.Text = Encoding.UTF8.GetString(array);
        }
        manifestResourceStream.Close();
        code_is_unmodified = true;
        just_opened = true;
        modified_since_opened = false;
        SaveBtn.Enabled = true;
        CodeBox.IsReadOnly = false;
        UARTRxReg.Text = "UART Rx Register";
        UARTTxReg.Text = "UART Tx Register";
        updateEditorState(".c");
        Run.Enabled = false;
        exportAssemblyToolStripMenuItem.Enabled = false;
        BreakBtn.Enabled = false;
        Compile.Enabled = false;
        StepBtn.Enabled = false;
        SaveCode.FileName = "";
        TabControl.TabPages[0].Text = "(No file)";
        lock (SymbolsList)
        {
            SymbolsList.Items.Clear();
            if (IPSSlider.Value != 1)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = watch_msg;
                SymbolsList.Items.Add(listViewItem);
            }
        }
        running = false;
        ElapsedTime.BackColor = Color.Red;
        ElapsedTime.Text = "0.000 Seconds";
        watches.Clear();
    }

    private void newASMCode()
    {
        lock (typeof(MainForm))
        {
            generateTimingDiagramToolStripMenuItem.Enabled = false;
            TimerInterface.timeKillEvent(timer_id);
            vm.Dispose();
            vm.vm = VMInterface.CreateVM();
            VMInterface.SetFilename(vm.vm, OpenCode.FileName);
            VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
            VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
            VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
        }
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RIMS_V2.resources.ASMUserTemplate.s");
        byte[] array = new byte[4096];
        if (manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length) != manifestResourceStream.Length)
        {
            CodeBox.Text = "";
        }
        else
        {
            CodeBox.Text = Encoding.UTF8.GetString(array);
        }
        manifestResourceStream.Close();
        code_is_unmodified = true;
        just_opened = true;
        modified_since_opened = false;
        SaveBtn.Enabled = true;
        CodeBox.IsReadOnly = false;
        UARTRxReg.Text = "UART Rx Register";
        UARTTxReg.Text = "UART Tx Register";
        updateEditorState(".s");
        Assemble.Enabled = false;
        Run.Enabled = false;
        exportAssemblyToolStripMenuItem.Enabled = false;
        BreakBtn.Enabled = false;
        StepBtn.Enabled = false;
        SaveCode.FileName = "";
        TabControl.TabPages[0].Text = "(No file)";
        lock (SymbolsList)
        {
            SymbolsList.Items.Clear();
            if (IPSSlider.Value != 1)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = watch_msg;
                SymbolsList.Items.Add(listViewItem);
            }
        }
        running = false;
        ElapsedTime.BackColor = Color.Red;
        ElapsedTime.Text = "0.000 Seconds";
        watches.Clear();
    }

    private void developCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (developCToolStripMenuItem.Checked)
        {
            return;
        }
        if (old_c_file != null && !modified_since_opened && old_c_file != string.Empty)
        {
            updateEditorState(".c");
            StreamReader streamReader = new StreamReader(old_c_file);
            CodeBox.Text = streamReader.ReadToEnd();
            streamReader.Close();
            just_opened = true;
            code_is_unmodified = true;
            TabControl.TabPages[0].Text = old_c_file;
            filename = old_c_file;
            SaveCode.FileName = filename;
            old_c_file = string.Empty;
        }
        else if (!code_is_unmodified)
        {
            switch (MessageBox.Show("File is not saved, would you like to save before switching to developing in C?", "Switching to C", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    if (!DoSave())
                    {
                        return;
                    }
                    break;
            }
            updateEditorState(".c");
            newToolStripMenuItem_Click(sender, e);
        }
        else
        {
            updateEditorState(".c");
            newToolStripMenuItem_Click(sender, e);
        }
        CodeBox.UndoRedo.EmptyUndoBuffer();
    }

    private void developASMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (developASMToolStripMenuItem.Checked)
        {
            return;
        }
        if (block != null)
        {
            block.Close();
            block = null;
        }
        if (TabControl.TabPages[0].Text != "(No file)")
        {
            if (SaveCode.FileName.Length == 0)
            {
                switch (MessageBox.Show("File is not saved, would you like to save before compiling and switching to developing in assembly?", "Switching to assembly", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.No:
                        updateEditorState(".s");
                        newToolStripMenuItem_Click(sender, e);
                        return;
                    case DialogResult.Yes:
                        if (!DoSave())
                        {
                            return;
                        }
                        break;
                }
            }
            else if (!Run.Enabled && MessageBox.Show("Your code will be compiled and you will switch to developing in assembly", "Switching to assembly", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
            {
                return;
            }
            old_c_file = filename;
            Compile_Click(sender, e);
            if (!Run.Enabled)
            {
                return;
            }
            updateEditorState(".s");
            string text = "";
            StreamReader streamReader = new StreamReader(asmloc);
            for (string text2 = streamReader.ReadLine(); text2 != null; text2 = streamReader.ReadLine())
            {
                if (text2.Length > 0 && text2[^1] != '\n')
                {
                    text2 += "\n";
                }
                if (text2.Contains(".end scan"))
                {
                    text2 = streamReader.ReadLine();
                    while (text2 != null && !text2.Contains(".globl DYNAMIC_MEMORY"))
                    {
                        if (text2.Length > 0 && text2[^1] != '\n')
                        {
                            text2 += "\n";
                        }
                        if (text2.Contains(".loc") || text2.StartsWith("'"))
                        {
                            text2 = streamReader.ReadLine();
                            continue;
                        }
                        text += text2;
                        text2 = streamReader.ReadLine();
                    }
                    while (text2 != null && !text2.Contains(".rdata"))
                    {
                        text2 = streamReader.ReadLine();
                    }
                    text += "#string literal definitions\n";
                    while (text2 != null)
                    {
                        if (text2.Length > 0 && text2[^1] != '\n')
                        {
                            text2 += "\n";
                        }
                        if (text2.Contains(".loc") || text2.StartsWith("'"))
                        {
                            text2 = streamReader.ReadLine();
                            continue;
                        }
                        text += text2;
                        text2 = streamReader.ReadLine();
                    }
                }
            }
            streamReader.Close();
            CodeBox.Text = text;
            updateEditorState(".s");
            lock (typeof(MainForm))
            {
                generateTimingDiagramToolStripMenuItem.Enabled = false;
                TimerInterface.timeKillEvent(timer_id);
                vm.Dispose();
                vm.vm = VMInterface.CreateVM();
                VMInterface.SetFilename(vm.vm, OpenCode.FileName);
                VMInterface.SetPin(vm.vm, Pins.A0, (byte)(((string)A0.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A1, (byte)(((string)A1.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A2, (byte)(((string)A2.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A3, (byte)(((string)A3.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A4, (byte)(((string)A4.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A5, (byte)(((string)A5.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A6, (byte)(((string)A6.Tag == "On") ? 1u : 0u));
                VMInterface.SetPin(vm.vm, Pins.A7, (byte)(((string)A7.Tag == "On") ? 1u : 0u));
                VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
            }
            code_is_unmodified = true;
            just_opened = true;
            SaveBtn.Enabled = true;
            CodeBox.IsReadOnly = false;
            UARTRxReg.Text = "UART Rx Register";
            UARTTxReg.Text = "UART Tx Register";
            Assemble.Enabled = false;
            Run.Enabled = false;
            exportAssemblyToolStripMenuItem.Enabled = false;
            BreakBtn.Enabled = false;
            StepBtn.Enabled = false;
            SaveCode.FileName = "";
            TabControl.TabPages[0].Text = "(No file)";
            lock (SymbolsList)
            {
                SymbolsList.Items.Clear();
                if (IPSSlider.Value != 1)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = watch_msg;
                    SymbolsList.Items.Add(listViewItem);
                }
            }
            running = false;
            ElapsedTime.BackColor = Color.Red;
            ElapsedTime.Text = "0.000 Seconds";
            watches.Clear();
        }
        else
        {
            updateEditorState(".s");
            newToolStripMenuItem_Click(sender, e);
        }
        CodeBox.UndoRedo.EmptyUndoBuffer();
    }

    private void viewInstructionListToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
        InstructionList instructionList = new InstructionList();
        instructionList.Show();
    }

    public void setSaveFile(string fileName)
    {
        SaveCode.FileName = fileName;
    }

    public PictureBox getA0()
    {
        return A0;
    }

    public PictureBox getA1()
    {
        return A1;
    }

    public PictureBox getA2()
    {
        return A2;
    }

    public PictureBox getA3()
    {
        return A3;
    }

    public PictureBox getA4()
    {
        return A4;
    }

    public PictureBox getA5()
    {
        return A5;
    }

    public PictureBox getA6()
    {
        return A6;
    }

    public PictureBox getA7()
    {
        return A7;
    }

    public ToolStripMenuItem getToolStripMenuItem(string itemName)
    {
        if (1 == 0)
        {
        }
        ToolStripMenuItem result = itemName switch
        {
            "developASMToolStripMenuItem" => developASMToolStripMenuItem,
            "developCToolStripMenuItem" => developCToolStripMenuItem,
            "newToolStripMenuItem" => newToolStripMenuItem,
            "aboutToolStripMenuItem" => aboutToolStripMenuItem,
            "viewInstructionListToolStripMenuItem" => viewInstructionListToolStripMenuItem,
            _ => null,
        };
        if (1 == 0)
        {
        }
        return result;
    }

    public void clickToolStripMenuItem(ToolStripMenuItem toolStripMenuItem)
    {
        if (toolStripMenuItem == developASMToolStripMenuItem)
        {
            developASMToolStripMenuItem_Click(null, null);
        }
        else if (toolStripMenuItem == developCToolStripMenuItem)
        {
            developCToolStripMenuItem_Click(null, null);
        }
        else if (toolStripMenuItem == newToolStripMenuItem)
        {
            newToolStripMenuItem_Click(null, null);
        }
        else if (toolStripMenuItem == aboutToolStripMenuItem)
        {
            aboutToolStripMenuItem_Click(null, null);
        }
        else if (toolStripMenuItem == viewInstructionListToolStripMenuItem)
        {
            viewInstructionListToolStripMenuItem_Click_1(null, null);
        }
    }

    public Button getButton(string buttonName)
    {
        if (1 == 0)
        {
        }
        Button result = buttonName switch
        {
            "Run" => Run,
            "Compile" => Compile,
            "Assemble" => Assemble,
            "Break" => BreakBtn,
            "Step" => StepBtn,
            _ => null,
        };
        if (1 == 0)
        {
        }
        return result;
    }

    public void clickButton(Button button)
    {
        if (button == Compile)
        {
            Compile_Click(null, null);
        }
        else if (button == Run)
        {
            Run_Click(null, null);
        }
        else if (button == Assemble)
        {
            Assemble_Click(null, null);
        }
        else if (button == BreakBtn)
        {
            BreakBtn_Click(null, null);
        }
        else if (button == StepBtn)
        {
            StepBtn_Click(null, null);
        }
    }

    public void adjustIPS(int value)
    {
        IPSSlider.Value = value;
        IPSSlider_Scroll(null, null);
    }

    public void clickPictureBox(PictureBox pictureBox)
    {
        if (pictureBox == A0)
        {
            A0_Click(null, null);
        }
        else if (pictureBox == A1)
        {
            A1_Click(null, null);
        }
        else if (pictureBox == A2)
        {
            A2_Click(null, null);
        }
        else if (pictureBox == A3)
        {
            A3_Click(null, null);
        }
        else if (pictureBox == A4)
        {
            A4_Click(null, null);
        }
        else if (pictureBox == A5)
        {
            A5_Click(null, null);
        }
        else if (pictureBox == A6)
        {
            A6_Click(null, null);
        }
        else if (pictureBox == A7)
        {
            A7_Click(null, null);
        }
    }

    public void adjustInputToState(string input, int state)
    {
        adjusting = true;
        switch (input.ToUpper())
        {
            case "A0":
                {
                    int num3 = ((!((string)A0.Tag == "Off")) ? 1 : 0);
                    if (num3 != state)
                    {
                        if ((string)A0.Tag == "Off")
                        {
                            A0.Image = A_Images_On[A_Image_Location[0]];
                            A0.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A0, 1);
                        }
                        else
                        {
                            A0.Image = A_Images_Off[A_Image_Location[0]];
                            A0.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A0, 0);
                        }
                    }
                    break;
                }
            case "A1":
                {
                    int num6 = ((!((string)A1.Tag == "Off")) ? 1 : 0);
                    if (num6 != state)
                    {
                        if ((string)A1.Tag == "Off")
                        {
                            A1.Image = A_Images_On[A_Image_Location[1]];
                            A1.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A1, 1);
                        }
                        else
                        {
                            A1.Image = A_Images_Off[A_Image_Location[1]];
                            A1.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A1, 0);
                        }
                    }
                    break;
                }
            case "A2":
                {
                    int num8 = ((!((string)A2.Tag == "Off")) ? 1 : 0);
                    if (num8 != state)
                    {
                        if ((string)A2.Tag == "Off")
                        {
                            A2.Image = A_Images_On[A_Image_Location[2]];
                            A2.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A2, 1);
                        }
                        else
                        {
                            A2.Image = A_Images_Off[A_Image_Location[2]];
                            A2.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A2, 0);
                        }
                    }
                    break;
                }
            case "A3":
                {
                    int num7 = ((!((string)A3.Tag == "Off")) ? 1 : 0);
                    if (num7 != state)
                    {
                        if ((string)A3.Tag == "Off")
                        {
                            A3.Image = A_Images_On[A_Image_Location[3]];
                            A3.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A3, 1);
                        }
                        else
                        {
                            A3.Image = A_Images_Off[A_Image_Location[3]];
                            A3.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A3, 0);
                        }
                    }
                    break;
                }
            case "A4":
                {
                    int num5 = ((!((string)A4.Tag == "Off")) ? 1 : 0);
                    if (num5 != state)
                    {
                        if ((string)A4.Tag == "Off")
                        {
                            A4.Image = A_Images_On[A_Image_Location[4]];
                            A4.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A4, 1);
                        }
                        else
                        {
                            A4.Image = A_Images_Off[A_Image_Location[4]];
                            A4.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A4, 0);
                        }
                    }
                    break;
                }
            case "A5":
                {
                    int num4 = ((!((string)A5.Tag == "Off")) ? 1 : 0);
                    if (num4 != state)
                    {
                        if ((string)A5.Tag == "Off")
                        {
                            A5.Image = A_Images_On[A_Image_Location[5]];
                            A5.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A5, 1);
                        }
                        else
                        {
                            A5.Image = A_Images_Off[A_Image_Location[5]];
                            A5.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A5, 0);
                        }
                    }
                    break;
                }
            case "A6":
                {
                    int num2 = ((!((string)A6.Tag == "Off")) ? 1 : 0);
                    if (num2 != state)
                    {
                        if ((string)A6.Tag == "Off")
                        {
                            A6.Image = A_Images_On[A_Image_Location[6]];
                            A6.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A6, 1);
                        }
                        else
                        {
                            A6.Image = A_Images_Off[A_Image_Location[6]];
                            A6.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A6, 0);
                        }
                    }
                    break;
                }
            case "A7":
                {
                    int num = ((!((string)A7.Tag == "Off")) ? 1 : 0);
                    if (num != state)
                    {
                        if ((string)A7.Tag == "Off")
                        {
                            A7.Image = A_Images_On[A_Image_Location[7]];
                            A7.Tag = "On";
                            VMInterface.SetPin(vm.vm, Pins.A7, 1);
                        }
                        else
                        {
                            A7.Image = A_Images_Off[A_Image_Location[7]];
                            A7.Tag = "Off";
                            VMInterface.SetPin(vm.vm, Pins.A7, 0);
                        }
                    }
                    break;
                }
        }
        adjusting = false;
        A0.Update();
        A1.Update();
        A2.Update();
        A3.Update();
        A4.Update();
        A5.Update();
        A6.Update();
        A7.Update();
    }

    private List<byte> getPinValuesFromFileLine(string line, int lineno)
    {
        List<byte> list = new List<byte>();
        if (line.Length > 0 && line.ToUpper()[0] == 'B')
        {
            if (line.Length < 9)
            {
                MessageBox.Show("Error:" + lineno + ": must have values for all 8 inputs");
                return null;
            }
            foreach (char item in line[1..].Reverse())
            {
                if (item == '0' || item == '1')
                {
                    list.Add((byte)char.GetNumericValue(item));
                    continue;
                }
                MessageBox.Show("Error:" + lineno + ": input values must be either 0 or 1");
                return null;
            }
            return list;
        }
        if (line.Length > 2 && line[..2].ToUpper() == "0X")
        {
            if (line.Length < 4)
            {
                MessageBox.Show("Error:" + lineno + ": must have values for all 8 inputs");
                return null;
            }
            foreach (char item2 in line[2..].Reverse())
            {
                try
                {
                    byte b = Convert.ToByte(item2.ToString(), 16);
                    list.Add((byte)(b & 1u));
                    list.Add((byte)((uint)(b >> 1) & 1u));
                    list.Add((byte)((uint)(b >> 2) & 1u));
                    list.Add((byte)((uint)(b >> 3) & 1u));
                }
                catch (Exception)
                {
                    MessageBox.Show("Error:" + lineno + ": hex value invalid");
                    return null;
                }
            }
            return list;
        }
        try
        {
            int num = Convert.ToInt32(line);
            list.Add((byte)((uint)num & 1u));
            list.Add((byte)((uint)(num >> 1) & 1u));
            list.Add((byte)((uint)(num >> 2) & 1u));
            list.Add((byte)((uint)(num >> 3) & 1u));
            list.Add((byte)((uint)(num >> 4) & 1u));
            list.Add((byte)((uint)(num >> 5) & 1u));
            list.Add((byte)((uint)(num >> 6) & 1u));
            list.Add((byte)((uint)(num >> 7) & 1u));
            return list;
        }
        catch (Exception)
        {
            MessageBox.Show("Error:" + lineno + ": decimal value invalid");
            return null;
        }
    }

    private void testVectorCleanup()
    {
        testVectorText.IsReadOnly = false;
        runFromInputVectorsThreadIsRunning = false;
        loadVector.Enabled = true;
        saveVector.Enabled = true;
        useTestVectors.Enabled = true;
        testVectorText.Markers.DeleteAll();
        IPSSlider.Enabled = true;
        A0.Enabled = true;
        A1.Enabled = true;
        A2.Enabled = true;
        A3.Enabled = true;
        A4.Enabled = true;
        A5.Enabled = true;
        A6.Enabled = true;
        A7.Enabled = true;
        for (int i = 0; i < 8; i++)
        {
            if (A_Image_Location[i] >= 2)
            {
                A_Image_Location[i] -= 2;
            }
            UpdateA_Images();
        }
    }

    private void runFromInputVectors()
    {
        List<string> input;
        int lineno;
        Marker temp_marker;
        runFromInputVectorsThread = new Thread((ThreadStart)delegate
        {
            try
            {
                input = new List<string>();
                Invoke((Action)delegate
                {
                    testVectorText.IsReadOnly = true;
                    loadVector.Enabled = false;
                    saveVector.Enabled = false;
                    setToWaveToolStripMenuItem.Enabled = false;
                    if (IPSSlider.Value == 5)
                    {
                        IPSSlider.Value = 3;
                        IPSSlider_Scroll(null, null);
                    }
                    IPSSlider.Enabled = false;
                    for (int j = 0; j < testVectorText.Lines.Count; j++)
                    {
                        string text3 = testVectorText.Lines[j].Text.Replace("\n", string.Empty);
                        text3 = text3.Replace("\r", string.Empty).Trim();
                        input.Add(text3);
                    }
                });
                lineno = 1;
                while (!running)
                {
                }
                temp_marker = null;
                Invoke((Action)delegate
                {
                    temp_marker = testVectorText.Markers[CUR_LINE_MARKER_NUMBER];
                    temp_marker.Number = CUR_LINE_MARKER_NUMBER;
                    temp_marker.Symbol = CUR_LINE_SYMBOL;
                    Marker marker = temp_marker;
                    Color color = (temp_marker.BackColor = CUR_LINE_COLOR);
                    Color foreColor = color;
                    marker.ForeColor = foreColor;
                    A0.Enabled = false;
                    A1.Enabled = false;
                    A2.Enabled = false;
                    A3.Enabled = false;
                    A4.Enabled = false;
                    A5.Enabled = false;
                    A6.Enabled = false;
                    A7.Enabled = false;
                    for (int i = 0; i < 8; i++)
                    {
                        if (A_Image_Location[i] < 2)
                            A_Image_Location[i] += 2;
                        UpdateA_Images();
                    }
                });
                using List<string>.Enumerator enumerator = input.GetEnumerator();
                for (; enumerator.MoveNext(); lineno++)
                {
                    string current = enumerator.Current;
                    if (!runFromInputVectorsThreadIsRunning || !running)
                    {
                        break;
                    }
                    Invoke((Action)delegate
                    {
                        if (lineno > 1)
                        {
                            testVectorText.Lines[lineno - 2].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                        }
                        testVectorText.Lines[lineno - 1].AddMarker(temp_marker);
                        testVectorText.Caret.Goto(testVectorText.Lines[lineno - 1].StartPosition);
                        testVectorText.Caret.EnsureVisible();
                    });
                    if (current.Length > 0)
                    {
                        if (current.ToUpper().Contains("GENERATETD"))
                        {
                            runFromInputVectorsThreadIsRunning = false;
                            Invoke((Action)delegate
                            {
                                if (running)
                                {
                                    Run_Click(null, null);
                                }
                                generateTimingDiagramToolStripMenuItem_Click(null, null);
                            });
                            break;
                        }
                        if (current.ToUpper().Contains("WAIT"))
                        {
                            string[] array = current.Split();
                            if (array.Length != 3)
                            {
                                MessageBox.Show("Error:" + lineno + ": format is 'wait number scale'");
                                runFromInputVectorsThreadIsRunning = false;
                                break;
                            }
                            double num;
                            try
                            {
                                num = double.Parse(array[1]);
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show("Error:" + lineno + ": format is 'wait number scale'");
                                runFromInputVectorsThreadIsRunning = false;
                                break;
                            }
                            string text = array[2].ToUpper();
                            string text2 = text;
                            if (!(text2 == "MS"))
                            {
                                if (text2 == "S")
                                {
                                    int num2 = (int)(num * 1000.0 * 1.0 / (double)((float)(ipp * 20) / (float)INSTR_PER_SEC));
                                    if (num2 >= 1)
                                    {
                                        Thread.Sleep(num2);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error:" + lineno + ": scale must be either ms or s");
                                    runFromInputVectorsThreadIsRunning = false;
                                }
                            }
                            else
                            {
                                int num3 = (int)num * (int)(1f / ((float)(ipp * 20) / (float)INSTR_PER_SEC));
                                if (num3 >= 1)
                                {
                                    Thread.Sleep(num3);
                                }
                            }
                        }
                        else
                        {
                            if (current.ToUpper().Contains("ASSERT"))
                            {
                                string[] array2 = current.Split();
                                if (array2.Length != 2)
                                {
                                    MessageBox.Show("Error:" + lineno + ": format is 'assert ########'");
                                    runFromInputVectorsThreadIsRunning = false;
                                }
                                else
                                {
                                    List<byte> pinValuesFromFileLine = getPinValuesFromFileLine(array2[1], lineno);
                                    if (pinValuesFromFileLine == null)
                                    {
                                        runFromInputVectorsThreadIsRunning = false;
                                    }
                                    else
                                    {
                                        lock (vmLock)
                                        {
                                            if (VMInterface.GetPin(vm.vm, Pins.B0) != pinValuesFromFileLine[0])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B0 not equal " + pinValuesFromFileLine[0]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B1) != pinValuesFromFileLine[1])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B1 not equal " + pinValuesFromFileLine[1]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B2) != pinValuesFromFileLine[2])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B2 not equal " + pinValuesFromFileLine[2]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B3) != pinValuesFromFileLine[3])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B3 not equal " + pinValuesFromFileLine[3]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B4) != pinValuesFromFileLine[4])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B4 not equal " + pinValuesFromFileLine[4]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B5) != pinValuesFromFileLine[5])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B5 not equal " + pinValuesFromFileLine[5]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else if (VMInterface.GetPin(vm.vm, Pins.B6) != pinValuesFromFileLine[6])
                                            {
                                                MessageBox.Show("Assert failed:" + lineno + ": B6 not equal " + pinValuesFromFileLine[6]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                            else
                                            {
                                                if (VMInterface.GetPin(vm.vm, Pins.B7) == pinValuesFromFileLine[7])
                                                {
                                                    continue;
                                                }
                                                MessageBox.Show("Assert failed:" + lineno + ": B7 not equal " + pinValuesFromFileLine[7]);
                                                runFromInputVectorsThreadIsRunning = false;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                            if (current.Split().Length != 1)
                            {
                                MessageBox.Show("Error:" + lineno + ": unknown command: " + current);
                                runFromInputVectorsThreadIsRunning = false;
                                break;
                            }
                            List<byte> values = getPinValuesFromFileLine(current, lineno);
                            if (values == null)
                            {
                                runFromInputVectorsThreadIsRunning = false;
                                break;
                            }
                            Invoke((Action)delegate
                            {
                                adjustInputToState("A0", values[0]);
                                adjustInputToState("A1", values[1]);
                                adjustInputToState("A2", values[2]);
                                adjustInputToState("A3", values[3]);
                                adjustInputToState("A4", values[4]);
                                adjustInputToState("A5", values[5]);
                                adjustInputToState("A6", values[6]);
                                adjustInputToState("A7", values[7]);
                                UpdateA_Value();
                            });
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                if (!ex2.Message.ToUpper().Contains("ABORT"))
                {
                    MessageBox.Show(ex2.Message + "\n" + ex2.StackTrace, "Caught exception running test vector");
                }
            }
            Invoke((Action)delegate
            {
                if (running)
                {
                    Run_Click(null, null);
                }
                testVectorCleanup();
            });
        });
        runFromInputVectorsThreadIsRunning = true;
        runFromInputVectorsThread.Start();
    }

    public ShellControl Terminal => terminal;

    private void runExtIOCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (block == null)
        {
            block = new ExternalLogicBlock(this);
            if (filename != null && filename.Length > 0)
            {
                block.setFileDir(filename[..filename.LastIndexOf("\\")], filename.Substring(filename.LastIndexOf("\\") + 1, filename[(filename.LastIndexOf("\\") + 1)..].LastIndexOf(".")) + "_extIOcode");
            }
            block.Show();
        }
    }

    private void generateTimingDiagramToolStripMenuItem_Click(object sender, EventArgs e)
    {
        generateTimingDiagramToolStripMenuItem.Enabled = false;
        string text = string.Empty;
        if (sender == null && e == null)
        {
            text = ((openInputVectorFile.FileName == string.Empty && saveInputVectorFile.FileName == string.Empty) ? (SaveCode.FileName[..SaveCode.FileName.LastIndexOf(Path.DirectorySeparatorChar)] + Path.DirectorySeparatorChar + "inputVector.vcd") : ((!(openInputVectorFile.FileName == string.Empty)) ? (openInputVectorFile.FileName + ".vcd") : (saveInputVectorFile.FileName + ".vcd")));
        }
        else
        {
            SignalLog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\RIMS";
            if (SignalLog.ShowDialog() == DialogResult.OK)
            {
                text = SignalLog.FileName;
            }
        }
        if (!text.Equals(string.Empty))
        {
            if (vm_terminate.ts != IntPtr.Zero)
            {
                VMInterface.GenerateSignalLog(vm_terminate.vm, text);
            }
            else
            {
                VMInterface.GenerateSignalLog(vm.vm, text);
            }
            try
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string[] array = location.Split(new string[2] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
                location = "";
                for (int i = 0; i < array.Length - 1; i++)
                {
                    location = location + array[i] + "\\";
                }
                location += "RITS.exe";
                Process.Start(location, "\"" + text + "\"");
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Couldn't start RITS.exe -- Win32Exception.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Couldn't start RITS.exe -- ObjectDisposedException.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Couldn't start RITS.exe -- FileNotFoundException.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't find RITS.exe -- please reinstall the toolkit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        generateTimingDiagramToolStripMenuItem.Enabled = true;
    }

    private void useTestVectors_Click(object sender, EventArgs e)
    {
        if (!using_testvectors)
        {
            useTestVectors.Text = "Use graphical inputs";
            using_testvectors = true;
            testVectorBox.Visible = true;
            setToWaveToolStripMenuItem.Enabled = false;
        }
        else
        {
            useTestVectors.Text = "Use test vectors";
            using_testvectors = false;
            testVectorBox.Visible = false;
            setToWaveToolStripMenuItem.Enabled = true;
        }
    }

    private void saveVector_Click(object sender, EventArgs e)
    {
        if (saveInputVectorFile.ShowDialog() != DialogResult.OK || saveInputVectorFile.FileName.Length <= 0)
        {
            return;
        }
        try
        {
            if (!saveInputVectorFile.FileName.EndsWith(".txt"))
            {
                saveInputVectorFile.FileName += ".txt";
            }
            StreamWriter streamWriter = new StreamWriter(saveInputVectorFile.FileName);
            streamWriter.Write(testVectorText.Text);
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        testVectorModified = false;
    }

    private void loadVector_Click(object sender, EventArgs e)
    {
        if (openInputVectorFile.ShowDialog() == DialogResult.OK && openInputVectorFile.FileName.Length > 0)
        {
            try
            {
                StreamReader streamReader = new StreamReader(openInputVectorFile.FileName);
                testVectorText.Text = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            recentlySaved = true;
            testVectorModified = false;
        }
    }

    private void testVectorText_TextChanged(object sender, EventArgs e)
    {
        if (recentlySaved)
        {
            recentlySaved = false;
        }
        else
        {
            testVectorModified = true;
        }
    }

    private void pIDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!pidEnabled)
        {
            if (waveEnabled)
            {
                offToolStripMenuItem_Click(offToolStripMenuItem, null);
            }
            pIDToolStripMenuItem.Text = "Disable PID simulation";
            setToWaveToolStripMenuItem.Enabled = false;
            for (int i = 4; i < 8; i++)
            {
                if (A_Image_Location[i] < 2)
                {
                    A_Image_Location[i] += 2;
                }
                UpdateA_Images();
            }
            A4.Enabled = false;
            A5.Enabled = false;
            A6.Enabled = false;
            A7.Enabled = false;
            pidBlockDisplay = new PIDBlockDisplay();
            pidBlockDisplay.FormClosed += delegate
            {
                Invoke((Action)delegate
                {
                    if (running)
                    {
                        Run_Click(null, null);
                    }
                    pIDToolStripMenuItem.Text = "Enable PID simulation";
                    setToWaveToolStripMenuItem.Enabled = true;
                    for (int k = 4; k < 8; k++)
                    {
                        if (A_Image_Location[k] >= 2)
                        {
                            A_Image_Location[k] -= 2;
                        }
                        UpdateA_Images();
                    }
                    A4.Enabled = true;
                    A5.Enabled = true;
                    A6.Enabled = true;
                    A7.Enabled = true;
                    pidBlockDisplay = null;
                    pidEnabled = false;
                });
            };
            pidBlockDisplay.Show();
            pidEnabled = true;
            pidSimSystem = new PIDSimSystem(this, delegate (double actual, double timePassed, int timeCnt)
            {
                double actuator = 0.0;
                string bvalue = string.Empty;
                string avalue = string.Empty;
                Invoke((Action)delegate
                {
                    bvalue = BValue_Dec.Text.Trim();
                    avalue = AValue_Dec.Text.Trim();
                });
                actuator = (int)PidInterface.actuatorToByte(Regex.Split(bvalue, "\\s+")[2]);
                byte[] actualB = PidInterface.byteToActual((byte)actual);
                double desired = PidInterface.desiredToDouble(Regex.Split(avalue, "\\s+")[2]);
                Invoke((Action)delegate
                {
                    adjustInputToState("A4", actualB[0]);
                    adjustInputToState("A5", actualB[1]);
                    adjustInputToState("A6", actualB[2]);
                    adjustInputToState("A7", actualB[3]);
                    UpdateA_Value();
                    pidBlockDisplay.actualTxt.Text = $"{actual:0}";
                    pidBlockDisplay.actualInternalTxt.Text = $"{actual:0.00}";
                    pidBlockDisplay.actuatorTxt.Text = $"{actuator:0.00}";
                    if (timeCnt % 17 == 0)
                    {
                        pidBlockDisplay.addPoint(timePassed, actual, desired);
                    }
                });
                return actuator;
            });
            PIDSimSystemParamUpdate simDel = pidSimSystem.UpdateParameter;
            pidBlockDisplay.SimDel = simDel;
            return;
        }
        setToWaveToolStripMenuItem.Enabled = true;
        pIDToolStripMenuItem.Text = "Enable PID simulation";
        for (int j = 4; j < 8; j++)
        {
            if (A_Image_Location[j] >= 2)
            {
                A_Image_Location[j] -= 2;
            }
            UpdateA_Images();
        }
        A4.Enabled = true;
        A5.Enabled = true;
        A6.Enabled = true;
        A7.Enabled = true;
        if (pidBlockDisplay != null)
        {
            pidBlockDisplay.Close();
            pidBlockDisplay = null;
        }
        pidEnabled = false;
        pidSimSystem.stop();
    }

    public void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem)
    {
        selectedMenuItem.Checked = true;
        foreach (ToolStripMenuItem item2 in from object item in selectedMenuItem.Owner.Items
                                            let ltoolStripMenuItem = item as ToolStripMenuItem
                                            where ltoolStripMenuItem != null
                                            where !item.Equals(selectedMenuItem)
                                            select ltoolStripMenuItem)
        {
            item2.Checked = false;
        }
    }

    private void turnOnWave(double hertz)
    {
        if (waveEnabled)
        {
            turnOffWave();
        }
        if (waveEnabled)
        {
            return;
        }
        for (int i = 0; i < 6; i++)
        {
            if (A_Image_Location[i] < 2)
            {
                A_Image_Location[i] += 2;
            }
            UpdateA_Images();
        }
        A0.Enabled = false;
        A1.Enabled = false;
        A2.Enabled = false;
        A3.Enabled = false;
        A4.Enabled = false;
        A5.Enabled = false;
        waveEnabled = true;
        sineSimulation = new SineSimulation(this, delegate (double val)
        {
            Invoke((Action)delegate
            {
                byte[] array = new byte[6]
                {
                    (byte)((uint)(int)val & 1u),
                    (byte)((uint)((int)val >> 1) & 1u),
                    (byte)((uint)((int)val >> 2) & 1u),
                    (byte)((uint)((int)val >> 3) & 1u),
                    (byte)((uint)((int)val >> 4) & 1u),
                    (byte)((uint)((int)val >> 5) & 1u)
                };
                adjustInputToState("A0", array[0]);
                adjustInputToState("A1", array[1]);
                adjustInputToState("A2", array[2]);
                adjustInputToState("A3", array[3]);
                adjustInputToState("A4", array[4]);
                adjustInputToState("A5", array[5]);
                UpdateA_Value();
            });
        }, 1.0, hertz, 0.0, 0.0);
        sineSimulation.start();
    }

    private void turnOffWave()
    {
        for (int i = 0; i < 6; i++)
        {
            if (A_Image_Location[i] >= 2)
            {
                A_Image_Location[i] -= 2;
            }
            UpdateA_Images();
        }
        A0.Enabled = true;
        A1.Enabled = true;
        A2.Enabled = true;
        A3.Enabled = true;
        A4.Enabled = true;
        A5.Enabled = true;
        waveEnabled = false;
        sineSimulation.stop();
    }

    private void offToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        turnOffWave();
    }

    private void hzToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        turnOnWave(1.0);
    }

    private void hzToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        turnOnWave(6.0);
    }

    private void hzToolStripMenuItem2_Click(object sender, EventArgs e)
    {
        UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        turnOnWave(20.0);
    }
}
