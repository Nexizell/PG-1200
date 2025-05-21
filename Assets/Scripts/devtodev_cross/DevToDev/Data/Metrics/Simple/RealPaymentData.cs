using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Simple
{
	internal class RealPaymentData : ISaveable
	{
		private string orderId;

		private float price;

		private string currencyCode;

		private int cheater;

		private long timestamp;

		private string inAppName;

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

		public string InAppName
		{
			get
			{
				return inAppName;
			}
			set
			{
				inAppName = value;
			}
		}

		public string OrderId
		{
			get
			{
				return orderId;
			}
			set
			{
				orderId = value;
			}
		}

		public float Price
		{
			get
			{
				return price;
			}
			set
			{
				price = value;
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

		public string CurrencyCode
		{
			get
			{
				return currencyCode;
			}
			set
			{
				currencyCode = value;
			}
		}

		public int Cheater
		{
			get
			{
				return cheater;
			}
			set
			{
				cheater = value;
			}
		}

		public RealPaymentData()
		{
		}

		public RealPaymentData(ObjectInfo info)
		{
			try
			{
				orderId = info.GetValue("orderId", typeof(string)) as string;
				currencyCode = info.GetValue("currencyCode", typeof(string)) as string;
				inAppName = info.GetValue("inAppName", typeof(string)) as string;
				price = (float)info.GetValue("price", typeof(float));
				cheater = (int)info.GetValue("cheater", typeof(int));
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
				info.AddValue("orderId", orderId);
				info.AddValue("price", price);
				info.AddValue("currencyCode", currencyCode);
				info.AddValue("cheater", cheater);
				info.AddValue("timestamp", timestamp);
				info.AddValue("inAppName", inAppName);
				info.AddValue("pendingData", pendingData);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public RealPaymentData(string orderId, float price, string inAppName, string currencyCode)
		{
			this.orderId = orderId;
			this.price = price;
			this.currencyCode = currencyCode;
			cheater = 0;
			this.inAppName = inAppName;
			timestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
		}
	}
}
