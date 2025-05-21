using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public sealed class NotifyFulfillmentInput : Jsonable
	{
		public string ReceiptId { get; set; }

		public string FulfillmentResult { get; set; }

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
					{ "receiptId", ReceiptId },
					{ "fulfillmentResult", FulfillmentResult }
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static NotifyFulfillmentInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput();
				if (jsonMap.ContainsKey("receiptId"))
				{
					notifyFulfillmentInput.ReceiptId = (string)jsonMap["receiptId"];
				}
				if (jsonMap.ContainsKey("fulfillmentResult"))
				{
					notifyFulfillmentInput.FulfillmentResult = (string)jsonMap["fulfillmentResult"];
				}
				return notifyFulfillmentInput;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static NotifyFulfillmentInput CreateFromJson(string jsonMessage)
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

		public static Dictionary<string, NotifyFulfillmentInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, NotifyFulfillmentInput> dictionary = new Dictionary<string, NotifyFulfillmentInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				NotifyFulfillmentInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<NotifyFulfillmentInput> ListFromJson(List<object> array)
		{
			List<NotifyFulfillmentInput> list = new List<NotifyFulfillmentInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
