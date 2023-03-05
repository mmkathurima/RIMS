using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RITS.Properties;

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
				ResourceManager resourceManager = new ResourceManager("RITS.Properties.Resources", typeof(Resources).Assembly);
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

	internal static Bitmap left
	{
		get
		{
			object @object = ResourceManager.GetObject("left", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static Bitmap minus
	{
		get
		{
			object @object = ResourceManager.GetObject("minus", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static Bitmap plus
	{
		get
		{
			object @object = ResourceManager.GetObject("plus", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal static Bitmap right
	{
		get
		{
			object @object = ResourceManager.GetObject("right", resourceCulture);
			return (Bitmap)@object;
		}
	}

	internal Resources()
	{
	}
}
