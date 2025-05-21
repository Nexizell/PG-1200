using System;
using System.Text;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics.Simple;
using UnityEngine;

namespace DevToDev.Cheat
{
	internal class VerifyEvent : SimpleEvent
	{
		private static readonly string KEY = "key";

		private static readonly string RECEIPT = "receipt";

		private static readonly string RECEIPT_DATA = "receipt-data";

		private static readonly string PASSWORD = "password";

		private static readonly string BUNDLE = "bundle";

		private static readonly string PUBLIC_KEY = "key";

		private static readonly string SIGNATURE = "sig";

		private static readonly string PLATFORM = "platform";

		private static readonly string ANDROID_PLATFORM = "android";

		private static readonly string IOS_PLATFORM = "ios";

		private static readonly string WS_PLATFORM = "Windows";

		public VerifyEvent(string receipt, byte[] signature, string publicKey)
			: base(DevToDev.Core.Data.Consts.EventType.ReceiptValidation)
		{
			if (UnityPlayerPlatform.isUnityWSAPlatform())
			{
				parameters.Add(PLATFORM, WS_PLATFORM);
				parameters.Add(KEY, Convert.ToBase64String(signature));
				parameters.Add(RECEIPT, Convert.ToBase64String(Encoding.UTF8.GetBytes(receipt)));
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				parameters.Add(PLATFORM, IOS_PLATFORM);
				parameters.Add(BUNDLE, ApplicationHelper.GetAppBundle());
				string value = "{\"" + RECEIPT_DATA + "\":\"" + receipt + "\"}";
				parameters.Add(RECEIPT, value);
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				parameters.Add(PLATFORM, ANDROID_PLATFORM);
				parameters.Add(SIGNATURE, Encoding.UTF8.GetString(signature, 0, signature.Length));
				parameters.Add(RECEIPT, receipt);
				parameters.Add(PUBLIC_KEY, publicKey);
			}
		}

		public VerifyEvent(ObjectInfo info)
			: base(info)
		{
		}
	}
}
