using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class Ad : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public AdType AdType { get; set; }

		public long Identifier { get; set; }

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
					{ "adType", AdType },
					{ "identifier", Identifier }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static Ad CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				Ad ad = new Ad();
				if (jsonMap.ContainsKey("adType"))
				{
					ad.AdType = (AdType)Enum.Parse(typeof(AdType), (string)jsonMap["adType"]);
				}
				if (jsonMap.ContainsKey("identifier"))
				{
					ad.Identifier = (long)jsonMap["identifier"];
				}
				return ad;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static Ad CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, Ad> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, Ad> dictionary = new Dictionary<string, Ad>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				Ad value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<Ad> ListFromJson(List<object> array)
		{
			List<Ad> list = new List<Ad>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
