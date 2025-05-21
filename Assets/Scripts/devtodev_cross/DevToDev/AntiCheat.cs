using DevToDev.Cheat;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev
{
	public static class AntiCheat
	{
		public static void VerifyReceipt(string receipt, string signature, string publicKey, OnReceiptVerifyCallback callback)
		{
			if (SDKClient.Instance.IsInitialized && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || UnityPlayerPlatform.isUnityWSAPlatform()) && receipt != null && callback != null)
			{
				SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(delegate
				{
					CheatClient.VerifyPayment(receipt, signature, publicKey, callback);
				});
			}
		}

		public static void VerifyTime(OnTimeVerifyCallback callback)
		{
			if (SDKClient.Instance.IsInitialized && callback != null)
			{
				SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(delegate
				{
					TimeManager.Instance.CheckTime(callback);
				});
			}
		}
	}
}
