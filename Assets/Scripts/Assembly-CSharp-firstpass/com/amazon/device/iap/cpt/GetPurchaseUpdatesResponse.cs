using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetPurchaseUpdatesResponse : Jsonable
	{
		public string RequestId { get; set; }

		public AmazonUserData AmazonUserData { get; set; }

		public List<PurchaseReceipt> Receipts { get; set; }

		public string Status { get; set; }

		public bool HasMore { get; set; }

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
						"receipts",
						(Receipts != null) ? Jsonable.unrollObjectIntoList(Receipts) : null
					},
					{ "status", Status },
					{ "hasMore", HasMore }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static GetPurchaseUpdatesResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				GetPurchaseUpdatesResponse getPurchaseUpdatesResponse = new GetPurchaseUpdatesResponse();
				if (jsonMap.ContainsKey("requestId"))
				{
					getPurchaseUpdatesResponse.RequestId = (string)jsonMap["requestId"];
				}
				if (jsonMap.ContainsKey("amazonUserData"))
				{
					getPurchaseUpdatesResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("receipts"))
				{
					getPurchaseUpdatesResponse.Receipts = PurchaseReceipt.ListFromJson(jsonMap["receipts"] as List<object>);
				}
				if (jsonMap.ContainsKey("status"))
				{
					getPurchaseUpdatesResponse.Status = (string)jsonMap["status"];
				}
				if (jsonMap.ContainsKey("hasMore"))
				{
					getPurchaseUpdatesResponse.HasMore = (bool)jsonMap["hasMore"];
				}
				return getPurchaseUpdatesResponse;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static GetPurchaseUpdatesResponse CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, GetPurchaseUpdatesResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetPurchaseUpdatesResponse> dictionary = new Dictionary<string, GetPurchaseUpdatesResponse>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				GetPurchaseUpdatesResponse value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<GetPurchaseUpdatesResponse> ListFromJson(List<object> array)
		{
			List<GetPurchaseUpdatesResponse> list = new List<GetPurchaseUpdatesResponse>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
