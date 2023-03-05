using System.Drawing;

namespace RIBS_V3;

public class Constants
{
	public const double VERSION = 2.8;

	public const int PROJECTVERSION = 1;

	public const int GRAPHVERSION = 2;

	public const int NODEVERSION = 1;

	public const int EDGEVERSION = 1;

	public const int NODE_HEIGHT = 55;

	public const int NODE_WIDTH = 55;

	public const int NODE_THICKNESS = 2;

	public const int EDGE_THICKNESS = 2;

	public const int HANDLE_RADIUS = 10;

	public const string NAME = "State machine 1";

	public const string PREFIX = "SM1";

	public const string PERIOD = "1000";

	public const string PNAME = "My system";

	public const string GLOBALCODE = "/*Define user variables for this state machine here. No functions; make them global.*/";

	public const string LOCALCODE = "/*Define user variables for this state machine here. No functions; make them global.*/";

	public const string LOCALCODE_SINGLESM = "/*Define user variables and functions for this state machine here.*/";

	public const string LOCALCODE_SAMPLE = "/*VARIABLES MUST BE DECLARED STATIC*/\r\n/*e.g., static int x = 0;*/\r\n";

	public const string MACROCODE = "/*This code will be shared between state machines.*/";

	public const string LOCALCODE_BOX_TITLE = "Variables and functions";

	public const string LOCALCODE_BOX_TITLE_MULT = "Variables";

	public const int ENABLETIMER = 0;

	public const bool ENABLEUART = false;

	public static int MAXSEGS = 145;

	public Brush EDGE_SELECTED_COLOR = Brushes.Red;

	public Brush EDGE_COLOR = Brushes.CornflowerBlue;
}
