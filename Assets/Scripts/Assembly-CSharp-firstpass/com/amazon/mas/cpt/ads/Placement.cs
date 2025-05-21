using System;
using System.Collections.Generic;
using com.amazon.mas.cpt.ads.json;

namespace com.amazon.mas.cpt.ads
{
	public sealed class Placement : Jsonable
	{
		private static AmazonLogger logger = new AmazonLogger("Pi");

		public Dock Dock { get; set; }

		public HorizontalAlign HorizontalAlign { get; set; }

		public AdFit AdFit { get; set; }

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
					{ "dock", Dock },
					{ "horizontalAlign", HorizontalAlign },
					{ "adFit", AdFit }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static Placement CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				Placement placement = new Placement();
				if (jsonMap.ContainsKey("dock"))
				{
					placement.Dock = (Dock)Enum.Parse(typeof(Dock), (string)jsonMap["dock"]);
				}
				if (jsonMap.ContainsKey("horizontalAlign"))
				{
					placement.HorizontalAlign = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), (string)jsonMap["horizontalAlign"]);
				}
				if (jsonMap.ContainsKey("adFit"))
				{
					placement.AdFit = (AdFit)Enum.Parse(typeof(AdFit), (string)jsonMap["adFit"]);
				}
				return placement;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static Placement CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, Placement> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, Placement> dictionary = new Dictionary<string, Placement>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				Placement value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<Placement> ListFromJson(List<object> array)
		{
			List<Placement> list = new List<Placement>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
