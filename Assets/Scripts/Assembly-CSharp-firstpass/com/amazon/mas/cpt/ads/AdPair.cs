using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdPair : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public Ad AdOne { get; set; }

		public Ad AdTwo { get; set; }

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
				return new Dictionary<string, object>
				{
					{
						"adOne",
						(AdOne != null) ? AdOne.GetObjectDictionary() : null
					},
					{
						"adTwo",
						(AdTwo != null) ? AdTwo.GetObjectDictionary() : null
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static AdPair CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				AdPair adPair = new AdPair();
				if (jsonMap.ContainsKey("adOne"))
				{
					adPair.AdOne = Ad.CreateFromDictionary(jsonMap["adOne"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("adTwo"))
				{
					adPair.AdTwo = Ad.CreateFromDictionary(jsonMap["adTwo"] as Dictionary<string, object>);
				}
				return adPair;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static AdPair CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, AdPair> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AdPair> dictionary = new Dictionary<string, AdPair>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				AdPair value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<AdPair> ListFromJson(List<object> array)
		{
			List<AdPair> list = new List<AdPair>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
