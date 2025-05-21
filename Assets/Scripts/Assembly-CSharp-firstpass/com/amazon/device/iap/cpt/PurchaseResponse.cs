using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponse : Jsonable
	{
		public string RequestId { get; set; }

		public AmazonUserData AmazonUserData { get; set; }

		public PurchaseReceipt PurchaseReceipt { get; set; }

		public string Status { get; set; }

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
					{ "requestId", RequestId },
					{
						"amazonUserData",
						(AmazonUserData != null) ? AmazonUserData.GetObjectDictionary() : null
					},
					{
						"purchaseReceipt",
						(PurchaseReceipt != null) ? PurchaseReceipt.GetObjectDictionary() : null
					},
					{ "status", Status }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static PurchaseResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				PurchaseResponse purchaseResponse = new PurchaseResponse();
				if (jsonMap.ContainsKey("requestId"))
				{
					purchaseResponse.RequestId = (string)jsonMap["requestId"];
				}
				if (jsonMap.ContainsKey("amazonUserData"))
				{
					purchaseResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("purchaseReceipt"))
				{
					purchaseResponse.PurchaseReceipt = PurchaseReceipt.CreateFromDictionary(jsonMap["purchaseReceipt"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("status"))
				{
					purchaseResponse.Status = (string)jsonMap["status"];
				}
				return purchaseResponse;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static PurchaseResponse CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, PurchaseResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseResponse> dictionary = new Dictionary<string, PurchaseResponse>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				PurchaseResponse value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<PurchaseResponse> ListFromJson(List<object> array)
		{
			List<PurchaseResponse> list = new List<PurchaseResponse>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
