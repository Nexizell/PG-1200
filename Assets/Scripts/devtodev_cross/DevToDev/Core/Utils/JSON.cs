namespace DevToDev.Core.Utils
{
	public static class JSON
	{
		public static readonly string Empty = "{}";

		public static JSONNode Parse(string aJSON)
		{
			return JSONNode.Parse(aJSON);
		}
	}
}
