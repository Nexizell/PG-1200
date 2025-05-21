using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Aggregated
{
	internal class InAppPurchaseData : ISaveable
	{
		private string purchaseId;

		private string purchaseType;

		private int purchaseAmount;

		private float purchasePrice;

		private string purchasePriceCurrency;

		private long timestamp;

		private List<string> pendingData;

		public List<string> PendingData
		{
			get
			{
				return pendingData;
			}
			set
			{
				pendingData = value;
			}
		}

		public string PurchaseId
		{
			get
			{
				return purchaseId;
			}
			set
			{
				purchaseId = value;
			}
		}

		public string PurchaseType
		{
			get
			{
				return purchaseType;
			}
			set
			{
				purchaseType = value;
			}
		}

		public long TimeStamp
		{
			get
			{
				return timestamp;
			}
			set
			{
				timestamp = value;
			}
		}

		public int PurchaseAmount
		{
			get
			{
				return purchaseAmount;
			}
			set
			{
				purchaseAmount = value;
			}
		}

		public float PurchasePrice
		{
			get
			{
				return purchasePrice;
			}
			set
			{
				purchasePrice = value;
			}
		}

		public string PurchasePriceCurrency
		{
			get
			{
				return purchasePriceCurrency;
			}
			set
			{
				purchasePriceCurrency = value;
			}
		}

		public InAppPurchaseData()
		{
		}

		public InAppPurchaseData(ObjectInfo info)
		{
			try
			{
				purchaseId = info.GetValue("purchaseId", typeof(string)) as string;
				purchaseType = info.GetValue("purchaseType", typeof(string)) as string;
				purchasePriceCurrency = info.GetValue("purchasePriceCurrency", typeof(string)) as string;
				purchasePrice = (float)info.GetValue("purchasePrice", typeof(float));
				purchaseAmount = (int)info.GetValue("purchaseAmount", typeof(int));
				timestamp = (long)info.GetValue("timestamp", typeof(long));
				pendingData = info.GetValue("pendingData", typeof(List<string>)) as List<string>;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("purchaseId", purchaseId);
				info.AddValue("purchaseType", purchaseType);
				info.AddValue("purchaseAmount", purchaseAmount);
				info.AddValue("purchasePrice", purchasePrice);
				info.AddValue("purchasePriceCurrency", purchasePriceCurrency);
				info.AddValue("timestamp", timestamp);
				info.AddValue("pendingData", pendingData);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public InAppPurchaseData(string purchaseId, string purchaseType, int purchaseAmount, float purchasePrice, string purchasePriceCurrency)
		{
			this.purchaseId = purchaseId;
			this.purchaseType = purchaseType;
			this.purchaseAmount = purchaseAmount;
			this.purchasePrice = purchasePrice;
			this.purchasePriceCurrency = purchasePriceCurrency;
			timestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
		}
	}
}
