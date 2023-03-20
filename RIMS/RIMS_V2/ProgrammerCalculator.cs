using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RIMS_V2;

public partial class ProgrammerCalculator : Form
{
    private bool handled = false;
    private int brckts = 0;

    public ProgrammerCalculator()
    {
        InitializeComponent();
        comboBox1.SelectedItem = comboBox1.Items[0];
        expTxt.ShortcutsEnabled = false;
    }

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
        IEnumerable<char> range = Enumerable.Range(0, 10).Select((int x) => x.ToString()[0])
            .Append('\b'), hex = range.Concat(Enumerable.Range('A', 'F' - 'A' + 1)
                    .Select((int x) => (char)x));
        TextBox txt = (TextBox)sender;
        char c = e.KeyChar;
        switch (txt.Name)
        {
            case "decimalTxt":
                if (this.handled = !range.Contains<char>(c))
                    e.Handled = true;
                break;
            case "binaryTxt":
                var bin = new[] { '0', '1', '\b' };
                if (this.handled = !bin.Contains<char>(c))
                    e.Handled = true;
                break;
            case "hexadecimalTxt":
                if (this.handled = !hex.Contains<char>(char.ToUpper(c)))
                    e.Handled = true;
                break;
            case "octalTxt":
                if (this.handled = !range.Take(8).Append('\b').Contains<char>(c))
                    e.Handled = true;
                break;
            case "expTxt":
                if (!hex.Concat<char>(new[] { '(', ')', '/', '*', '+', '-', '%', ' ',
                '.', 'X', 'B', '|', '&', '~', '^', '<', '>', '!', '=' })
                    .Contains<char>(char.ToUpper(c)))
                    e.Handled = true;
                break;
        }

    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {

    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        TextBox txt = (TextBox)sender;
        var t = string.IsNullOrEmpty(txt.Text) ? "0" : txt.Text;
        switch (txt.Name)
        {
            case "decimalTxt":
                if (!this.handled)
                {
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(t, 10), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 10), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(t, 10), 8);
                }
                break;
            case "binaryTxt":
                if (!this.handled)
                {
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 2), 10);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 2), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(t, 2), 8);
                }
                break;
            case "hexadecimalTxt":
                if (!this.handled)
                {
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(t, 16), 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 16), 10);
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(t, 16), 8);
                }
                break;
            case "octalTxt":
                if (!this.handled)
                {
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(t, 8), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 8), 16).ToUpper();
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(t, 8), 10);
                }
                break;
        }


    }

    private async void OnClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.Name)
        {
            case "btnEq":
                try
                {
                    int @base = 10;
                    dynamic res = await CSharpScript.EvaluateAsync(expTxt.Text);
                    string result = res?.ToString();
                    ansTxt.Text = comboBox1.SelectedItem switch
                    {
                        "BIN" => Convert.ToString(Convert.ToInt32(result, 10), 2),
                        "HEX" => Convert.ToString(Convert.ToInt32(result, 10), 16),
                        "OCT" => Convert.ToString(Convert.ToInt32(result, 10), 8),
                        _ => Convert.ToString(Convert.ToInt32(result, 10), 10),
                    };
                }
                catch (NullReferenceException)
                {
                    ansTxt.Text = "No expression received.";
                }
                catch (CompilationErrorException)
                {
                    ansTxt.Text = "Invalid expression received.";
                }
                break;
            case "decLeftShift":
                if (!string.IsNullOrEmpty(decimalTxt.Text))
                {
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10) << 1, 10);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 8);
                }

                break;
            case "decRightShift":
                if (!string.IsNullOrEmpty(decimalTxt.Text))
                {
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10) >> 1, 10);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 8);
                }
                break;
            case "binLeftShift":
                if (!string.IsNullOrEmpty(binaryTxt.Text))
                {
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2) << 1, 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 10);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 8);
                }
                break;
            case "binRightShift":
                if (!string.IsNullOrEmpty(binaryTxt.Text))
                {
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2) >> 1, 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 10);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 8);
                }
                break;
            case "hexLeftShift":
                if (!string.IsNullOrEmpty(hexadecimalTxt.Text))
                {
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16) << 1, 16);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 10);
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 8);
                }
                break;
            case "hexRightShift":
                if (!string.IsNullOrEmpty(hexadecimalTxt.Text))
                {
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16) >> 1, 16);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 10);
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 8);
                }
                break;
            case "octLeftShift":
                if (!string.IsNullOrEmpty(octalTxt.Text))
                {
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8) >> 1, 16);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 16).ToUpper();
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 10);
                }
                break;
            case "octRightShift":
                if (!string.IsNullOrEmpty(octalTxt.Text))
                {
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8) << 1, 8);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 16).ToUpper();
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 10);
                }
                break;
            case "decimalNot":
                if (!string.IsNullOrEmpty(decimalTxt.Text))
                {
                    decimalTxt.Text = Convert.ToString(~Convert.ToInt32(decimalTxt.Text, 10), 10);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(decimalTxt.Text, 10), 8);
                }
                break;
            case "binaryNot":
                if (!string.IsNullOrEmpty(binaryTxt.Text))
                {
                    binaryTxt.Text = Convert.ToString(~Convert.ToInt32(binaryTxt.Text, 2), 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 10);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 16).ToUpper();
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(binaryTxt.Text, 2), 8);
                }
                break;
            case "hexNot":
                if (!string.IsNullOrEmpty(hexadecimalTxt.Text))
                {
                    hexadecimalTxt.Text = Convert.ToString(~Convert.ToInt32(hexadecimalTxt.Text, 16), 16);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 2);
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 10);
                    octalTxt.Text = Convert.ToString(Convert.ToInt32(hexadecimalTxt.Text, 16), 8);
                }
                break;
            case "octNot":
                if (!string.IsNullOrEmpty(octalTxt.Text))
                {
                    octalTxt.Text = Convert.ToString(~Convert.ToInt32(octalTxt.Text, 8), 16);
                    binaryTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 2);
                    hexadecimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 16).ToUpper();
                    decimalTxt.Text = Convert.ToString(Convert.ToInt32(octalTxt.Text, 8), 10);
                }
                break;
        }
    }
}
