using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics.Aggregated;

namespace DevToDev.Data.Metrics.Simple
{
	internal sealed class RealPayment : AggregatedEvent
	{
		private static readonly string NAME = "name";

		private static readonly string ENTRIES = "entries";

		private static readonly string ORDER_ID = "orderId";

		private static readonly string CURRENCY_CODE = "currencyCode";

		private static readonly string PRICE = "price";

		private Dictionary<string, List<RealPaymentData>> aggregatedRealPaymentDatas;

		private readonly RealPaymentData data;

		public Dictionary<string, List<RealPaymentData>> AggregatedRealPaymentDatas
		{
			get
			{
				return aggregatedRealPaymentDatas;
			}
			set
			{
				aggregatedRealPaymentDatas = value;
			}
		}

		public RealPayment()
		{
		}

		public RealPayment(string orderId, float price, string inAppName, string currencyCode)
			: base(EventType.RealPayment)
		{
			data = new RealPaymentData(orderId, price, inAppName, currencyCode);
			aggregatedRealPaymentDatas = new Dictionary<string, List<RealPaymentData>>();
			SelfAggregate();
		}

		public RealPayment(ObjectInfo info)
			: base(info)
		{
			try
			{
				aggregatedRealPaymentDatas = info.GetValue("aggregatedRealPaymentDatas", typeof(Dictionary<string, List<RealPaymentData>>)) as Dictionary<string, List<RealPaymentData>>;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("aggregatedRealPaymentDatas", aggregatedRealPaymentDatas);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void AddPendingEvents(List<string> events)
		{
			foreach (KeyValuePair<string, List<RealPaymentData>> aggregatedRealPaymentData in aggregatedRealPaymentDatas)
			{
				foreach (RealPaymentData item in aggregatedRealPaymentData.Value)
				{
					item.PendingData = events;
				}
			}
		}

		public override JSONNode GetAdditionalDataJson()
		{
			JSONArray jSONArray = new JSONArray();
			foreach (KeyValuePair<string, List<RealPaymentData>> aggregatedRealPaymentData in aggregatedRealPaymentDatas)
			{
				JSONClass jSONClass = new JSONClass();
				jSONArray.Add(jSONClass);
				JSONArray jSONArray2 = new JSONArray();
				jSONClass.Add(ENTRIES, jSONArray2);
				jSONClass.Add(NAME, Uri.EscapeDataString(aggregatedRealPaymentData.Key));
				foreach (RealPaymentData item in aggregatedRealPaymentData.Value)
				{
					JSONClass jSONClass2 = new JSONClass();
					jSONArray2.Add(jSONClass2);
					jSONClass2.Add(ORDER_ID, Uri.EscapeDataString(item.OrderId));
					jSONClass2.Add(CURRENCY_CODE, item.CurrencyCode);
					jSONClass2.Add(PRICE, new JSONData(item.Price));
					jSONClass2.Add(Event.TIMESTAMP, new JSONData(item.TimeStamp));
					AddPendingToJSON(jSONClass2, item.PendingData);
				}
			}
			return jSONArray;
		}

		public override void AddEvent(AggregatedEvent metric)
		{
			if (metric == null)
			{
				return;
			}
			Dictionary<string, List<RealPaymentData>> dictionary = (metric as RealPayment).aggregatedRealPaymentDatas;
			foreach (KeyValuePair<string, List<RealPaymentData>> item in dictionary)
			{
				if (aggregatedRealPaymentDatas.ContainsKey(item.Key))
				{
					aggregatedRealPaymentDatas[item.Key].AddRange(item.Value);
				}
				else
				{
					aggregatedRealPaymentDatas.Add(item.Key, item.Value);
				}
			}
		}

		protected override void SelfAggregate()
		{
			aggregatedRealPaymentDatas = new Dictionary<string, List<RealPaymentData>>();
			aggregatedRealPaymentDatas.Add(data.InAppName, new List<RealPaymentData> { data });
		}

		public override bool IsReadyToSend()
		{
			return true;
		}

		public override bool IsNeedToClear()
		{
			return true;
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
