using System;
using System.Reflection;
using System.Windows.Forms;

namespace RIMS_V2;

public class VM
{
    public IntPtr vm;

    public IntPtr ts;

    public VM()
    {
        vm = VMInterface.CreateVM();
        string location = Assembly.GetExecutingAssembly().Location;
        string[] array = location.Split('\\');
        location = "";
        uint num = 0u;
        while (array.Length > 1 && num < array.Length - 1)
        {
            location += array[num];
            if (array.Length <= 2 || num != array.Length - 2)
            {
                location += "\\";
            }
            num++;
        }
        VMInterface.SetBaseDirectory(vm, location);
    }

    ~VM()
    {
    }

    public void Dispose()
    {
        try
        {
            VMInterface.DestroyVM(vm, ts);
        }
        catch (Exception e)
        {
            //TODO: Look into this
            //MessageBox.Show(owner: null, e.ToString());
        }
    }
}
