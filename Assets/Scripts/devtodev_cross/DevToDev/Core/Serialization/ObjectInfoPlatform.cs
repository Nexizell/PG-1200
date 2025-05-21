using System.Reflection;

namespace DevToDev.Core.Serialization
{
	public class ObjectInfoPlatform
	{
		public static FieldInfo GetFieldInfo(object saveableObject, string fieldName)
		{
			return saveableObject.GetType().GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
		}

		public static bool IsPrimitive(object data)
		{
			return data.GetType().IsPrimitive;
		}
	}
}
