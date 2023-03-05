using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RIBS_V3.Properties;

[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
				ResourceManager resourceManager = new ResourceManager("RIBS_V3.Properties.Resources", typeof(Resources).Assembly);
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

	internal static byte[] default_project
	{
		get
		{
			object @object = ResourceManager.GetObject("default_project", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static Bitmap down_arrow
	{
		get
		{
			object @object = ResourceManager.GetObject("down_arrow", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static byte[] RIBS_ch3_ApplauseMeter_Mealy
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch3_ApplauseMeter_Mealy", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch3_ApplauseMeter_Moore
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch3_ApplauseMeter_Moore", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch3_Latch
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch3_Latch", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch5_GlitchFilter
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch5_GlitchFilter", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch5_Pwm
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch5_Pwm", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch5_SpeakerProject
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch5_SpeakerProject", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch6_LedShow
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch6_LedShow", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static byte[] RIBS_ch6_MotionLamp
	{
		get
		{
			object @object = ResourceManager.GetObject("RIBS_ch6_MotionLamp", resourceCulture);
			return (byte[])@object;
		}
	}

	internal static Bitmap up_arrow
	{
		get
		{
			object @object = ResourceManager.GetObject("up_arrow", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal Resources()
	{
	}
}
