using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RIMS_V2.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("RIMS_V2.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string EULA => ResourceManager.GetString("EULA", resourceCulture);

	internal static Bitmap fan
	{
		get
		{
			object @object = ResourceManager.GetObject("fan", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static Bitmap PIDBlock
	{
		get
		{
			object @object = ResourceManager.GetObject("PIDBlock", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static string RIMS_ch2_ParkingLot => ResourceManager.GetString("RIMS_ch2_ParkingLot", resourceCulture);

	internal static string RIMS_ch3_Latch => ResourceManager.GetString("RIMS_ch3_Latch", resourceCulture);

	internal static string RIMS_ch7_LedShow => ResourceManager.GetString("RIMS_ch7_LedShow", resourceCulture);

	internal static string RIMS_ch9_ThreeLedsTriggered => ResourceManager.GetString("RIMS_ch9_ThreeLedsTriggered", resourceCulture);

	internal static string RIMS_CrosswalkExample => ResourceManager.GetString("RIMS_CrosswalkExample", resourceCulture);

	internal static string RIMS_Example_other => ResourceManager.GetString("RIMS_Example_other", resourceCulture);

	internal static string RIMS_IOExample_Addition => ResourceManager.GetString("RIMS_IOExample_Addition", resourceCulture);

	internal static string RIMS_IOExample_C2F => ResourceManager.GetString("RIMS_IOExample_C2F", resourceCulture);

	internal static string RIMS_LibExample_Debug => ResourceManager.GetString("RIMS_LibExample_Debug", resourceCulture);

	internal static string RIMS_LibExample_Misc => ResourceManager.GetString("RIMS_LibExample_Misc", resourceCulture);

	internal static string RIMS_LibExample_Strings => ResourceManager.GetString("RIMS_LibExample_Strings", resourceCulture);

	internal static string RIMS_ProducerConsumer_multi => ResourceManager.GetString("RIMS_ProducerConsumer_multi", resourceCulture);

	internal static string RIMS_sample_code => ResourceManager.GetString("RIMS_sample_code", resourceCulture);

	internal static string RIMS_TimerExample_Basic => ResourceManager.GetString("RIMS_TimerExample_Basic", resourceCulture);

	internal static string RIMS_TimerExample_Toggle => ResourceManager.GetString("RIMS_TimerExample_Toggle", resourceCulture);

	internal static string RIMS_UARTExample_Echo => ResourceManager.GetString("RIMS_UARTExample_Echo", resourceCulture);

	internal static string RIMS_UARTExample_Messenger => ResourceManager.GetString("RIMS_UARTExample_Messenger", resourceCulture);

	internal Resources()
	{
	}
}
