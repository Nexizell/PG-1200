using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated
{
	internal sealed class InAppPurchase : AggregatedEvent
	{
		private static readonly string PURCHASE_ID = "purchaseId";

		private static readonly string PURCHASE_TYPE = "purchaseType";

		private static readonly string PURCHASE_AMOUNT = "purchaseAmount";

		private static readonly string PURCHASE_PRICE = "purchasePrice";

		private static readonly string PURCHASE_CURRENCY = "purchasePriceCurrency";

		private List<InAppPurchaseData> aggregatedInGamePurchaseDatas;

		private readonly InAppPurchaseData data;

		public List<InAppPurchaseData> AggregatedInGamePurchaseDatas
		{
			get
			{
				return aggregatedInGamePurchaseDatas;
			}
			set
			{
				aggregatedInGamePurchaseDatas = value;
			}
		}

		public InAppPurchase()
		{
		}

		public InAppPurchase(ObjectInfo info)
			: base(info)
		{
			try
			{
				aggregatedInGamePurchaseDatas = info.GetValue("aggregatedInGamePurchaseDatas", typeof(List<InAppPurchaseData>)) as List<InAppPurchaseData>;
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
				info.AddValue("aggregatedInGamePurchaseDatas", aggregatedInGamePurchaseDatas);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, float purchasePrice, string purchasePriceCurrency)
			: base(EventType.InAppPurchase)
		{
			data = new InAppPurchaseData(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchasePriceCurrency);
			aggregatedInGamePurchaseDatas = new List<InAppPurchaseData>();
			SelfAggregate();
		}

		public override JSONNode GetAdditionalDataJson()
		{
			JSONArray jSONArray = new JSONArray();
			foreach (InAppPurchaseData aggregatedInGamePurchaseData in aggregatedInGamePurchaseDatas)
			{
				JSONClass jSONClass = new JSONClass();
				jSONArray.Add(jSONClass);
				jSONClass.Add(PURCHASE_ID, Uri.EscapeDataString(aggregatedInGamePurchaseData.PurchaseId));
				jSONClass.Add(PURCHASE_TYPE, Uri.EscapeDataString(aggregatedInGamePurchaseData.PurchaseType));
				jSONClass.Add(PURCHASE_AMOUNT, new JSONData(aggregatedInGamePurchaseData.PurchaseAmount));
				jSONClass.Add(PURCHASE_PRICE, new JSONData(aggregatedInGamePurchaseData.PurchasePrice));
				jSONClass.Add(PURCHASE_CURRENCY, Uri.EscapeDataString(aggregatedInGamePurchaseData.PurchasePriceCurrency));
				jSONClass.Add(Event.TIMESTAMP, new JSONData(aggregatedInGamePurchaseData.TimeStamp));
				AddPendingToJSON(jSONClass, aggregatedInGamePurchaseData.PendingData);
			}
			return jSONArray;
		}

		public override void AddEvent(AggregatedEvent metric)
		{
			if (metric != null)
			{
				List<InAppPurchaseData> collection = ((InAppPurchase)metric).aggregatedInGamePurchaseDatas;
				aggregatedInGamePurchaseDatas.AddRange(collection);
			}
		}

		public override void AddPendingEvents(List<string> events)
		{
			foreach (InAppPurchaseData aggregatedInGamePurchaseData in aggregatedInGamePurchaseDatas)
			{
				aggregatedInGamePurchaseData.PendingData = events;
			}
		}

		protected override void SelfAggregate()
		{
			aggregatedInGamePurchaseDatas = new List<InAppPurchaseData>();
			aggregatedInGamePurchaseDatas.Add(data);
		}

		public override bool IsReadyToSend()
		{
			return true;
		}

		public override bool IsNeedToClear()
		{
			return true;
		}

		public override bool IsEqualToMetric(Event other)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			InAppPurchase inAppPurchase = other as InAppPurchase;
			if (data == null || inAppPurchase.data == null)
			{
				return false;
			}
			return data.PurchaseId.Equals(inAppPurchase.data.PurchaseId);
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
