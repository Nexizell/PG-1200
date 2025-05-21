using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class ApplicationKey : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public string StringValue { get; set; }

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
				return new Dictionary<string, object> { { "stringValue", StringValue } };
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static ApplicationKey CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				ApplicationKey applicationKey = new ApplicationKey();
				if (jsonMap.ContainsKey("stringValue"))
				{
					applicationKey.StringValue = (string)jsonMap["stringValue"];
				}
				return applicationKey;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static ApplicationKey CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, ApplicationKey> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ApplicationKey> dictionary = new Dictionary<string, ApplicationKey>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				ApplicationKey value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<ApplicationKey> ListFromJson(List<object> array)
		{
			List<ApplicationKey> list = new List<ApplicationKey>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
