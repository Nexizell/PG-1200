using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkuInput : Jsonable
	{
		public string Sku { get; set; }

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
				return new Dictionary<string, object> { { "sku", Sku } };
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static SkuInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				SkuInput skuInput = new SkuInput();
				if (jsonMap.ContainsKey("sku"))
				{
					skuInput.Sku = (string)jsonMap["sku"];
				}
				return skuInput;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static SkuInput CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, SkuInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkuInput> dictionary = new Dictionary<string, SkuInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				SkuInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<SkuInput> ListFromJson(List<object> array)
		{
			List<SkuInput> list = new List<SkuInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
