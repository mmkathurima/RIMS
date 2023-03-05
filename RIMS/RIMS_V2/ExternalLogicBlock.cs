using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ScintillaNet;
using System.Linq;

namespace RIMS_V2;

public class ExternalLogicBlock : Form
{
    private const uint PULSE_DELAY = 50u;

    private IContainer components;

    private Scintilla scintilla1;

    private Timer UpdateTimer;

    private MenuStrip menuStrip1;

    private ToolStripMenuItem fileToolStripMenuItem;

    private ToolStripMenuItem newToolStripMenuItem;

    private ToolStripMenuItem openBlockToolStripMenuItem;

    private OpenFileDialog OpenCode;

    private Button Break;

    private Button Step;

    private Timer SymbolUpdateTimer;

    private MainForm mainVM;

    private static IntPtr breakpoint_step;

    public static VM vm;

    public static VM vm_terminate;

    private static int DEFAULT_IPP = 250;

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

    private FileSystemWatcher file_watcher;

    private string asmloc = Environment.GetEnvironmentVariable("TEMP") + "\\extlcc.s";

    private string old_file;

    private int old_line_highlighted;

    private bool is_line_highlighted;

    private bool code_is_unmodified;

    private bool just_opened;

    private int B_selected;

    public static string OpenFirstMessage = "You must load or save a file before compiling";

    public List<string> watches;

    public static string watch_msg = "Set to \"Slowest\" speed";

    private List<uint> breakpoints;

    private bool running;

    private StringBuilder sb;

    private string blockName;

    private Dictionary<string, string> inputMap = new Dictionary<string, string>();

    private Dictionary<string, string> outputMap = new Dictionary<string, string>();

    private Dictionary<string, byte> oldValues = new Dictionary<string, byte>();

    private List<string> feedbackPins = new List<string>();

    private bool ready;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIMS_V2.ExternalLogicBlock));
        this.scintilla1 = new ScintillaNet.Scintilla();
        this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.OpenCode = new System.Windows.Forms.OpenFileDialog();
        this.Break = new System.Windows.Forms.Button();
        this.Step = new System.Windows.Forms.Button();
        this.SymbolUpdateTimer = new System.Windows.Forms.Timer(this.components);
        ((System.ComponentModel.ISupportInitialize)this.scintilla1).BeginInit();
        this.menuStrip1.SuspendLayout();
        base.SuspendLayout();
        this.scintilla1.AutoComplete.ListString = "";
        this.scintilla1.AutoComplete.MaxHeight = 50;
        this.scintilla1.AutoComplete.MaxWidth = 40;
        this.scintilla1.AutoComplete.SingleLineAccept = true;
        this.scintilla1.ConfigurationManager.Language = "cs";
        this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.scintilla1.Folding.Flags = ScintillaNet.FoldFlag.LineBeforeExpanded;
        this.scintilla1.Folding.MarkerScheme = ScintillaNet.FoldMarkerScheme.PlusMinus;
        this.scintilla1.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.scintilla1.Indentation.IndentWidth = 3;
        this.scintilla1.Indentation.TabWidth = 3;
        this.scintilla1.IsBraceMatching = true;
        this.scintilla1.Lexing.Lexer = ScintillaNet.Lexer.Null;
        this.scintilla1.Lexing.LexerName = "null";
        this.scintilla1.Lexing.LineCommentPrefix = "";
        this.scintilla1.Lexing.StreamCommentPrefix = "";
        this.scintilla1.Lexing.StreamCommentSufix = "";
        this.scintilla1.LineWrap.Mode = ScintillaNet.WrapMode.Word;
        this.scintilla1.LineWrap.StartIndent = 8;
        this.scintilla1.LineWrap.VisualFlags = ScintillaNet.WrapVisualFlag.Start;
        this.scintilla1.Location = new System.Drawing.Point(0, 24);
        this.scintilla1.Margins.Margin0.Width = 30;
        this.scintilla1.Margins.Margin1.IsClickable = true;
        this.scintilla1.Margins.Margin1.Width = 20;
        this.scintilla1.Margins.Margin2.Width = 15;
        this.scintilla1.Name = "scintilla1";
        this.scintilla1.Size = new System.Drawing.Size(622, 288);
        this.scintilla1.Styles.BraceBad.FontName = "Verdana";
        this.scintilla1.Styles.BraceLight.FontName = "Verdana";
        this.scintilla1.Styles.ControlChar.FontName = "Verdana";
        this.scintilla1.Styles.Default.FontName = "Verdana";
        this.scintilla1.Styles.IndentGuide.FontName = "Verdana";
        this.scintilla1.Styles.LastPredefined.FontName = "Verdana";
        this.scintilla1.Styles.LineNumber.FontName = "Verdana";
        this.scintilla1.Styles.Max.FontName = "Verdana";
        this.scintilla1.TabIndex = 0;
        this.scintilla1.Text = resources.GetString("scintilla1.Text");
        this.scintilla1.TextChanged += new System.EventHandler<System.EventArgs>(scintilla1_TextChanged);
        this.UpdateTimer.Enabled = true;
        this.UpdateTimer.Interval = 17;
        this.UpdateTimer.Tick += new System.EventHandler(UpdateTimer_Tick);
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.fileToolStripMenuItem });
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(622, 24);
        this.menuStrip1.TabIndex = 1;
        this.menuStrip1.Text = "menuStrip1";
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.newToolStripMenuItem, this.openBlockToolStripMenuItem });
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "File";
        this.newToolStripMenuItem.Name = "newToolStripMenuItem";
        this.newToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
        this.newToolStripMenuItem.Text = "New";
        this.newToolStripMenuItem.Click += new System.EventHandler(newToolStripMenuItem_Click);
        this.openBlockToolStripMenuItem.Name = "openBlockToolStripMenuItem";
        this.openBlockToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
        this.openBlockToolStripMenuItem.Text = "Open external code...";
        this.openBlockToolStripMenuItem.Click += new System.EventHandler(openBlockToolStripMenuItem_Click);
        this.OpenCode.Filter = "RIMS C Code|*.c";
        this.Break.Enabled = false;
        this.Break.Location = new System.Drawing.Point(454, 1);
        this.Break.Name = "Break";
        this.Break.Size = new System.Drawing.Size(75, 23);
        this.Break.TabIndex = 2;
        this.Break.Text = "Break";
        this.Break.UseVisualStyleBackColor = true;
        this.Break.Click += new System.EventHandler(Break_Click);
        this.Step.Enabled = false;
        this.Step.Location = new System.Drawing.Point(535, 1);
        this.Step.Name = "Step";
        this.Step.Size = new System.Drawing.Size(75, 23);
        this.Step.TabIndex = 3;
        this.Step.Text = "Step";
        this.Step.UseVisualStyleBackColor = true;
        this.Step.Click += new System.EventHandler(Step_Click);
        this.SymbolUpdateTimer.Interval = 33;
        this.SymbolUpdateTimer.Tick += new System.EventHandler(SymbolUpdateTimer_Tick);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(622, 312);
        base.Controls.Add(this.Step);
        base.Controls.Add(this.Break);
        base.Controls.Add(this.scintilla1);
        base.Controls.Add(this.menuStrip1);
        base.MainMenuStrip = this.menuStrip1;
        base.Name = "ExternalLogicBlock";
        this.Text = "External I/O Code";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExternalLogicBlock_FormClosing);
        ((System.ComponentModel.ISupportInitialize)this.scintilla1).EndInit();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public ExternalLogicBlock(MainForm mainVM)
    {
        InitializeComponent();
        this.mainVM = mainVM;
        vm = new VM();
        vm_terminate = new VM();
        code_is_unmodified = true;
        just_opened = false;
        ipp = (uint)DEFAULT_IPP;
        watches = new List<string>();
        breakpoints = new List<uint>();
        running = false;
        scintilla1.UseBackColor = true;
        scintilla1.Indentation.UseTabs = false;
        scintilla1.Indentation.IndentWidth = 3;
        UpdateTimer.Enabled = false;
        sb = new StringBuilder(1000);
    }

    public bool isReady()
    {
        return ready;
    }

    public void setFileDir(string currDir, string blockName)
    {
        this.blockName = blockName;
        Text = Text + ": " + blockName;
        filename = currDir + "\\" + blockName + ".c";
        asmloc = currDir + "\\" + blockName + "lcc.s";
        ready = true;
    }

    public void mapInputPin(string mainPin, string blockPin)
    {
        inputMap.Add(mainPin, blockPin);
    }

    public void mapOutputPin(string blockPin, string mainPin)
    {
        outputMap.Add(blockPin, mainPin);
    }

    public void clearMapping()
    {
        inputMap.Clear();
        outputMap.Clear();
    }

    private void updateOutputPin(string blockPin, string mainPin)
    {
        Pins which = Pins.B0;
        switch (blockPin.ToUpper())
        {
            case "B0":
                which = Pins.B0;
                break;
            case "B1":
                which = Pins.B1;
                break;
            case "B2":
                which = Pins.B2;
                break;
            case "B3":
                which = Pins.B3;
                break;
            case "B4":
                which = Pins.B4;
                break;
            case "B5":
                which = Pins.B5;
                break;
            case "B6":
                which = Pins.B6;
                break;
            case "B7":
                which = Pins.B7;
                break;
        }
        mainVM.adjustInputToState(mainPin, VMInterface.GetPin(vm.vm, which));
    }

    private void updateInputPin(string mainPin, string blockPin)
    {
        Pins which = Pins.B0;
        switch (mainPin.ToUpper())
        {
            case "B0":
                which = Pins.B0;
                break;
            case "B1":
                which = Pins.B1;
                break;
            case "B2":
                which = Pins.B2;
                break;
            case "B3":
                which = Pins.B3;
                break;
            case "B4":
                which = Pins.B4;
                break;
            case "B5":
                which = Pins.B5;
                break;
            case "B6":
                which = Pins.B6;
                break;
            case "B7":
                which = Pins.B7;
                break;
        }
        Pins which2 = Pins.A0;
        switch (blockPin.ToUpper())
        {
            case "A0":
                which2 = Pins.A0;
                break;
            case "A1":
                which2 = Pins.A1;
                break;
            case "A2":
                which2 = Pins.A2;
                break;
            case "A3":
                which2 = Pins.A3;
                break;
            case "A4":
                which2 = Pins.A4;
                break;
            case "A5":
                which2 = Pins.A5;
                break;
            case "A6":
                which2 = Pins.A6;
                break;
            case "A7":
                which2 = Pins.A7;
                break;
        }
        VMInterface.SetPin(vm.vm, which2, VMInterface.GetPin(MainForm.vm.vm, which));
    }

    public void DoSave()
    {
        StreamWriter streamWriter = new StreamWriter(filename);
        streamWriter.Write(scintilla1.Text);
        streamWriter.Close();
    }

    public unsafe bool compile()
    {
        if (!code_is_unmodified)
        {
            StreamWriter streamWriter = new StreamWriter(filename);
            streamWriter.Write(scintilla1.Text);
            streamWriter.Close();
            code_is_unmodified = true;
        }
        VMInterface.SetLccName(vm.vm, blockName + "lcc.s");
        VMInterface.SetFilename(vm.vm, filename);
        bool flag = false;
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
        int num2 = VMInterface.Compile(vm.vm);
        IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ErrorStruct)));
        byte* ptr = (byte*)intPtr.ToPointer();
        for (int i = 0; i < Marshal.SizeOf(typeof(ErrorStruct)); i++)
        {
            ptr[i] = 0;
        }
        VMInterface.GetErrors(vm.vm, intPtr);
        ErrorStruct errorStruct = (ErrorStruct)Marshal.PtrToStructure(intPtr, typeof(ErrorStruct));
        Marshal.FreeHGlobal(intPtr);
        foreach (Line line in scintilla1.Lines)
        {
            line.DeleteMarker(COMPILE_ERROR_NUMBER);
        }
        scintilla1.Invalidate();
        if (num2 == -2)
        {
            flag = false;
        }
        else if (num2 == -1)
        {
            mainVM.Terminal.WriteText("Invalid file selected");
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
                foreach (var (num3, marker) in from Match item in matchCollection
                                               let num3 = Convert.ToInt32(item.Groups[1].Value)
                                               let marker = scintilla1.Markers[COMPILE_ERROR_NUMBER]
                                               select (num3, marker))
                {
                    marker.Number = COMPILE_ERROR_NUMBER;
                    marker.Symbol = COMPILE_ERROR_SYMBOL;
                    Color color = (marker.BackColor = COMPILE_ERROR_COLOR);
                    Color foreColor = color;
                    marker.ForeColor = foreColor;
                    scintilla1.Lines[num3 - 1].AddMarker(marker);
                }
            }
            for (int k = 0; k < array2.Length - 1; k++)
            {
                mainVM.Terminal.WriteText(array2[k]);
                mainVM.Terminal.WriteText("\r\n");
            }
            if (array2.Length != 0)
            {
                mainVM.Terminal.WriteText(array2[array2.Length - 1]);
            }
        }
        else
        {
            flag = true;
        }
        for (int l = 0; l < watches.Count; l++)
        {
            if (VMInterface.GetSymbolIndex(vm.vm, watches[l]) == -1)
            {
                watches.RemoveAt(l);
                l--;
            }
        }
        return flag;
    }

    private void scintilla1_TextChanged(object sender, EventArgs e)
    {
        mainVM.getButton("Run").Enabled = false;
        if (!just_opened)
        {
            code_is_unmodified = false;
        }
        else
        {
            just_opened = false;
        }
    }

    private void updateMappedPins()
    {
        foreach (KeyValuePair<string, string> item in inputMap)
        {
            updateInputPin(item.Key, item.Value);
        }
        foreach (KeyValuePair<string, string> item2 in outputMap)
        {
            updateOutputPin(item2.Key, item2.Value);
        }
    }

    public unsafe void run()
    {
        if (!running)
        {
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
            scintilla1.UseBackColor = true;
            scintilla1.IsReadOnly = true;
            scintilla1.BackColor = SystemColors.Control;
            scintilla1.Margins.FoldMarginColor = SystemColors.Control;
            scintilla1.Margins.FoldMarginHighlightColor = SystemColors.Control;
            if (vm.ts != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(vm.ts);
            }
            SymbolUpdateTimer.Enabled = true;
            vm.ts = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ThreadStruct)));
            TimerInterface.timeBeginPeriod(10u);
            VMInterface.SetIPP(vm.vm, ipp);
            Break.Enabled = true;
            Step.Enabled = false;
            VMInterface.setExternal(vm.vm, val: true);
            initialSyncPins();
            VMInterface.Run(vm.vm, vm.ts);
            timer_id = TimerInterface.timeSetEvent(50u, 0u, clock, vm.vm, 1u);
            running = true;
            UpdateTimer.Enabled = true;
        }
        else
        {
            lock (typeof(MainForm))
            {
                TimerInterface.timeKillEvent(timer_id);
                vm_terminate.ts = vm.ts;
                vm_terminate.vm = vm.vm;
                vm.vm = VMInterface.CreateVM();
                VMInterface.SetFilename(vm.vm, filename);
                VMInterface.SetNestedInterrupts(vm.vm, nested_interrupts_enabled);
            }
            running = false;
            SymbolUpdateTimer.Enabled = false;
            Break.Enabled = false;
            Step.Enabled = false;
            scintilla1.IsReadOnly = false;
            scintilla1.BackColor = Color.White;
            scintilla1.Margins.FoldMarginColor = Color.White;
            scintilla1.Markers.DeleteAll();
            running = false;
            oldValues.Clear();
            feedbackPins.Clear();
            breakpoints.Clear();
            UpdateTimer.Enabled = false;
        }
    }

    private void initialSyncPins()
    {
        oldValues.Clear();
        feedbackPins.Clear();
        oldValues.Add("A0", VMInterface.GetPin(vm.vm, Pins.A0));
        oldValues.Add("A1", VMInterface.GetPin(vm.vm, Pins.A1));
        oldValues.Add("A2", VMInterface.GetPin(vm.vm, Pins.A2));
        oldValues.Add("A3", VMInterface.GetPin(vm.vm, Pins.A3));
        oldValues.Add("A4", VMInterface.GetPin(vm.vm, Pins.A4));
        oldValues.Add("A5", VMInterface.GetPin(vm.vm, Pins.A5));
        oldValues.Add("A6", VMInterface.GetPin(vm.vm, Pins.A6));
        oldValues.Add("A7", VMInterface.GetPin(vm.vm, Pins.A7));
    }

    public void pinClicked(string pin, byte val)
    {
        if (!feedbackPins.Contains(pin))
        {
            oldValues[pin] = val;
            VMInterface.SetPin(vm.vm, getPinValue(pin), val);
            feedbackPins.Remove(pin);
        }
    }

    private Pins getPinValue(string pinName)
    {
        string text = pinName.ToUpper();
        if (1 == 0)
        {
        }
        Pins result = text switch
        {
            "A" => Pins.A,
            "A0" => Pins.A0,
            "A1" => Pins.A1,
            "A2" => Pins.A2,
            "A3" => Pins.A3,
            "A4" => Pins.A4,
            "A5" => Pins.A5,
            "A6" => Pins.A6,
            "A7" => Pins.A7,
            _ => Pins.A,
        };
        if (1 == 0)
        {
        }
        return result;
    }

    private void updateAllPins()
    {
        feedbackPins.AddRange(oldValues.Where(oldValue => VMInterface.GetPin(vm.vm, getPinValue(oldValue.Key)) != oldValue.Value && !feedbackPins.Contains(oldValue.Key)).Select(oldValue => oldValue.Key));
        List<string> list = new List<string>();
        foreach (string feedbackPin in feedbackPins)
        {
            mainVM.adjustInputToState(feedbackPin, VMInterface.GetPin(vm.vm, getPinValue(feedbackPin)));
            oldValues[feedbackPin] = VMInterface.GetPin(vm.vm, getPinValue(feedbackPin));
            list.Add(feedbackPin);
        }
        foreach (string item in list)
        {
            feedbackPins.Remove(item);
        }
        VMInterface.SetPin(vm.vm, Pins.B, VMInterface.GetPin(MainForm.vm.vm, Pins.B));
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        updateAllPins();
    }

    private void ExternalLogicBlock_FormClosing(object sender, FormClosingEventArgs e)
    {
        lock (typeof(MainForm))
        {
            TimerInterface.timeKillEvent(timer_id);
            TimerInterface.timeEndPeriod(10u);
        }
        if (File.Exists(asmloc))
        {
            File.Delete(asmloc);
        }
        UpdateTimer.Enabled = false;
        mainVM.block = null;
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
        scintilla1.Text = "// C statements describing external system.\r\n// Statements can READ RIMS outputs B and/or B0, B1, ..., B7.\r\n// Statements can WRITE RIMS inputs A and/or A0, A1, ..., A7.\r\n// Such writes override any manual configurations of RIMS inputs.\r\n// Statements CANNOT READ RIMS inputs A and/or A0, A1, ..., A7.\r\n// Statements CANNOT WRITE RIMS inputs B and/or B0, B1, ..., B7.\r\n\r\n#include \"rims.h\"\r\n\r\nint main()\r\n{\r\n   while (1) {\r\n      A0 = 1;\r\n      A1 = 1;\r\n   }\r\n}\r\n";
        mainVM.getButton("Run").Enabled = false;
    }

    private void openBlockToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (OpenCode.ShowDialog() == DialogResult.OK)
        {
            filename = OpenCode.FileName;
            StreamReader streamReader = new StreamReader(filename);
            scintilla1.Text = streamReader.ReadToEnd();
            streamReader.Close();
            mainVM.getButton("Run").Enabled = false;
        }
    }

    private void Break_Click(object sender, EventArgs e)
    {
        if (VMInterface.IsBroken(vm.vm) == 0)
        {
            VMInterface.Break(vm.vm);
            Break.Text = "Continue";
        }
        else
        {
            VMInterface.Step(vm.vm);
            VMInterface.SetUnBroken(vm.vm);
            Break.Text = "Break";
        }
    }

    private void Step_Click(object sender, EventArgs e)
    {
        VMInterface.Step(vm.vm);
    }

    public void writeToCodeBox(string toWrite)
    {
        scintilla1.Text = toWrite;
    }

    private void SymbolUpdateTimer_Tick(object sender, EventArgs e)
    {
        _ = vm.vm;
        try
        {
            bool flag = VMInterface.IsBroken(vm.vm) == 1;
            if (VMInterface.IsRunning(vm.vm) == 1)
            {
                Break.Enabled = true;
                if (flag)
                {
                    Break.Text = "Continue";
                    Step.Enabled = true;
                }
                else
                {
                    Break.Text = "Break";
                    Step.Enabled = false;
                }
            }
            else if (running)
            {
                running = false;
                Break.Enabled = false;
                scintilla1.IsReadOnly = false;
                scintilla1.BackColor = Color.White;
                scintilla1.Margins.FoldMarginColor = Color.White;
                if (is_line_highlighted && old_line_highlighted > 0)
                {
                    scintilla1.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                    is_line_highlighted = false;
                    old_line_highlighted = -1;
                }
                breakpoints.Clear();
                MessageBox.Show("End of program reached.");
            }
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TagStruct)));
            VMInterface.GetLine(vm.vm, intPtr);
            TagStruct tagStruct = (TagStruct)Marshal.PtrToStructure(intPtr, typeof(TagStruct));
            Marshal.FreeHGlobal(intPtr);
            int num = tagStruct.line - 1;
            if (is_line_highlighted && !flag)
            {
                scintilla1.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                is_line_highlighted = false;
                old_line_highlighted = -1;
            }
            if (flag && num >= 0 && num < scintilla1.Lines.Count && num != old_line_highlighted)
            {
                Marker marker = scintilla1.Markers[CUR_LINE_MARKER_NUMBER];
                marker.Number = CUR_LINE_MARKER_NUMBER;
                marker.Symbol = CUR_LINE_SYMBOL;
                Color color = (marker.BackColor = CUR_LINE_COLOR);
                Color foreColor = color;
                marker.ForeColor = foreColor;
                if (old_line_highlighted != -1)
                {
                    scintilla1.Lines[old_line_highlighted].DeleteMarker(CUR_LINE_MARKER_NUMBER);
                }
                scintilla1.Lines[num].AddMarker(marker);
                if (VMInterface.IsRunning(vm.vm) == 1)
                {
                    scintilla1.Caret.Goto(scintilla1.Lines[num].StartPosition);
                    scintilla1.Caret.EnsureVisible();
                }
                old_line_highlighted = num;
                is_line_highlighted = true;
            }
        }
        catch (NullReferenceException)
        {
        }
        catch (Exception)
        {
        }
    }
}
