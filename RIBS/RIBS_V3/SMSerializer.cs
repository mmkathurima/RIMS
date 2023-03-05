using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RIBS_V3;

internal class SMSerializer
{
	public static void SerializeObject(string filename, Project project)
	{
		Stream stream = File.Open(filename, FileMode.Create);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(stream, project);
		stream.Close();
	}

	public static Project DeSerializeObjectStream(Stream stream)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		try
		{
			Project result = (Project)binaryFormatter.Deserialize(stream);
			stream.Close();
			return result;
		}
		catch (SerializationException)
		{
			stream.Close();
			throw new Exception("Error deserializing a project stream");
		}
	}

	public static Project DeSerializeObject(string filename)
	{
		Stream stream = File.Open(filename, FileMode.Open);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		try
		{
			Project result = (Project)binaryFormatter.Deserialize(stream);
			stream.Close();
			return result;
		}
		catch (SerializationException)
		{
			stream.Close();
			return Project.LegacyOpen(filename);
		}
	}
}
