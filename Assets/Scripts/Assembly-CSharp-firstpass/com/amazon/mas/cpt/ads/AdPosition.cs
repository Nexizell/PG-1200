using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdPosition : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public Ad Ad { get; set; }

		public int XCoordinate { get; set; }

		public int YCoordinate { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

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
						"ad",
						(Ad != null) ? Ad.GetObjectDictionary() : null
					},
					{ "xCoordinate", XCoordinate },
					{ "yCoordinate", YCoordinate },
					{ "width", Width },
					{ "height", Height }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static AdPosition CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				AdPosition adPosition = new AdPosition();
				if (jsonMap.ContainsKey("ad"))
				{
					adPosition.Ad = Ad.CreateFromDictionary(jsonMap["ad"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("xCoordinate"))
				{
					adPosition.XCoordinate = Convert.ToInt32(jsonMap["xCoordinate"]);
				}
				if (jsonMap.ContainsKey("yCoordinate"))
				{
					adPosition.YCoordinate = Convert.ToInt32(jsonMap["yCoordinate"]);
				}
				if (jsonMap.ContainsKey("width"))
				{
					adPosition.Width = Convert.ToInt32(jsonMap["width"]);
				}
				if (jsonMap.ContainsKey("height"))
				{
					adPosition.Height = Convert.ToInt32(jsonMap["height"]);
				}
				return adPosition;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static AdPosition CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, AdPosition> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AdPosition> dictionary = new Dictionary<string, AdPosition>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				AdPosition value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<AdPosition> ListFromJson(List<object> array)
		{
			List<AdPosition> list = new List<AdPosition>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
