namespace RIMS_V2;

internal class PidInterface
{
	public static byte actuatorToByte(string sActuator)
	{
		return byte.Parse(sActuator);
	}

	public static byte[] byteToActual(byte actual)
	{
		return new byte[4]
		{
			(byte)(actual & 1u),
			(byte)((uint)(actual >> 1) & 1u),
			(byte)((uint)(actual >> 2) & 1u),
			(byte)((uint)(actual >> 3) & 1u)
		};
	}

	public static double desiredToDouble(string sDesired)
	{
		int num = int.Parse(sDesired);
		return num & 0xF;
	}
}
