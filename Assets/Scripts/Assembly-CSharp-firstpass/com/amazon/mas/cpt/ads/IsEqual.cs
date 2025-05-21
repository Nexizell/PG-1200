using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class IsEqual : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public bool BooleanValue { get; set; }

		public string ToJson()
		{
			try
			{
				return Json.Serialize(GetObjectDictionary());
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while Jsoning", inner);
			}
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			try
			{
				return new Dictionary<string, object> { { "booleanValue", BooleanValue } };
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static IsEqual CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				IsEqual isEqual = new IsEqual();
				if (jsonMap.ContainsKey("booleanValue"))
				{
					isEqual.BooleanValue = (bool)jsonMap["booleanValue"];
				}
				return isEqual;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static IsEqual CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
		}

		public static Dictionary<string, IsEqual> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, IsEqual> dictionary = new Dictionary<string, IsEqual>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				IsEqual value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<IsEqual> ListFromJson(List<object> array)
		{
			List<IsEqual> list = new List<IsEqual>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
