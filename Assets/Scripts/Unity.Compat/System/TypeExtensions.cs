using System.Reflection;

namespace System
{
	public static class TypeExtensions
	{
		public static Type AsType(this Type type)
		{
			return type;
		}

		public static TypeInfo GetTypeInfo(this Type type)
		{
			return TypeInfo.FromType(type);
		}
	}
}
