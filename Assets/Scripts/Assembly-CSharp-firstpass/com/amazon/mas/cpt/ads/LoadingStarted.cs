using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class LoadingStarted : Jsonable
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

		public static LoadingStarted CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				LoadingStarted loadingStarted = new LoadingStarted();
				if (jsonMap.ContainsKey("booleanValue"))
				{
					loadingStarted.BooleanValue = (bool)jsonMap["booleanValue"];
				}
				return loadingStarted;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static LoadingStarted CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, LoadingStarted> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, LoadingStarted> dictionary = new Dictionary<string, LoadingStarted>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				LoadingStarted value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<LoadingStarted> ListFromJson(List<object> array)
		{
			List<LoadingStarted> list = new List<LoadingStarted>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
