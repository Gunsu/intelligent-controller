using System.Reflection;
using System.IO;

namespace IC.Core
{
	internal static class Constants
	{
		public static string EXECUTABLE_PATH = Assembly.GetEntryAssembly().Location;
		public static string BLOCK_TYPES_XML_PATH = Path.Combine(EXECUTABLE_PATH, "BlockTypes.xml");
	}
}
