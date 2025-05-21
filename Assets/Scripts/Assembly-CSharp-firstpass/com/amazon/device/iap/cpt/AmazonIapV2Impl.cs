using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public abstract class AmazonIapV2Impl : IAmazonIapV2
	{
		internal abstract class AmazonIapV2Base : AmazonIapV2Impl
		{
			private static readonly object startLock = new object();

			private static volatile bool startCalled = false;

			protected void Start()
			{
				if (startCalled)
				{
					return;
				}
				lock (startLock)
				{
					if (!startCalled)
					{
						Init();
						RegisterCallback();
						RegisterEventListener();
						RegisterCrossPlatformTool();
						startCalled = true;
					}
				}
			}

			protected abstract void Init();

			protected abstract void RegisterCallback();

			protected abstract void RegisterEventListener();

			protected abstract void RegisterCrossPlatformTool();

			public AmazonIapV2Base()
			{
				logger = new AmazonLogger(GetType().Name);
			}

			public override void UnityFireEvent(string jsonMessage)
			{
				FireEvent(jsonMessage);
			}

			public override RequestOutput GetUserData()
			{
				Start();
				return RequestOutput.CreateFromJson(GetUserDataJson("{}"));
			}

			private string GetUserDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = NativeGetUserDataJson(jsonMessage);
				stopwatch.Stop();
				logger.Debug(string.Format("Successfully called native code in {0} ms", new object[1] { stopwatch.ElapsedMilliseconds }));
				return result;
			}

			protected abstract string NativeGetUserDataJson(string jsonMessage);

			public override RequestOutput Purchase(SkuInput skuInput)
			{
				Start();
				return RequestOutput.CreateFromJson(PurchaseJson(skuInput.ToJson()));
			}

			private string PurchaseJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = NativePurchaseJson(jsonMessage);
				stopwatch.Stop();
				logger.Debug(string.Format("Successfully called native code in {0} ms", new object[1] { stopwatch.ElapsedMilliseconds }));
				return result;
			}

			protected abstract string NativePurchaseJson(string jsonMessage);

			public override RequestOutput GetProductData(SkusInput skusInput)
			{
				Start();
				return RequestOutput.CreateFromJson(GetProductDataJson(skusInput.ToJson()));
			}

			private string GetProductDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = NativeGetProductDataJson(jsonMessage);
				stopwatch.Stop();
				logger.Debug(string.Format("Successfully called native code in {0} ms", new object[1] { stopwatch.ElapsedMilliseconds }));
				return result;
			}

			protected abstract string NativeGetProductDataJson(string jsonMessage);

			public override RequestOutput GetPurchaseUpdates(ResetInput resetInput)
			{
				Start();
				return RequestOutput.CreateFromJson(GetPurchaseUpdatesJson(resetInput.ToJson()));
			}

			private string GetPurchaseUpdatesJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = NativeGetPurchaseUpdatesJson(jsonMessage);
				stopwatch.Stop();
				logger.Debug(string.Format("Successfully called native code in {0} ms", new object[1] { stopwatch.ElapsedMilliseconds }));
				return result;
			}

			protected abstract string NativeGetPurchaseUpdatesJson(string jsonMessage);

			public override void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput)
			{
				Start();
				Jsonable.CheckForErrors(Json.Deserialize(NotifyFulfillmentJson(notifyFulfillmentInput.ToJson())) as Dictionary<string, object>);
			}

			private string NotifyFulfillmentJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = NativeNotifyFulfillmentJson(jsonMessage);
				stopwatch.Stop();
				logger.Debug(string.Format("Successfully called native code in {0} ms", new object[1] { stopwatch.ElapsedMilliseconds }));
				return result;
			}

			protected abstract string NativeNotifyFulfillmentJson(string jsonMessage);

			public override void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				Start();
				string key = "getUserDataResponse";
				lock (eventLock)
				{
					if (eventListeners.ContainsKey(key))
					{
						eventListeners[key].Add(new GetUserDataResponseDelegator(responseDelegate));
						return;
					}
					List<IDelegator> list = new List<IDelegator>();
					list.Add(new GetUserDataResponseDelegator(responseDelegate));
					eventListeners.Add(key, list);
				}
			}

			public override void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				Start();
				string key = "getUserDataResponse";
				lock (eventLock)
				{
					if (!eventListeners.ContainsKey(key))
					{
						return;
					}
					foreach (GetUserDataResponseDelegator item in eventListeners[key])
					{
						if (item.responseDelegate == responseDelegate)
						{
							eventListeners[key].Remove(item);
							break;
						}
					}
				}
			}

			public override void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				Start();
				string key = "purchaseResponse";
				lock (eventLock)
				{
					if (eventListeners.ContainsKey(key))
					{
						eventListeners[key].Add(new PurchaseResponseDelegator(responseDelegate));
						return;
					}
					List<IDelegator> list = new List<IDelegator>();
					list.Add(new PurchaseResponseDelegator(responseDelegate));
					eventListeners.Add(key, list);
				}
			}

			public override void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				Start();
				string key = "purchaseResponse";
				lock (eventLock)
				{
					if (!eventListeners.ContainsKey(key))
					{
						return;
					}
					foreach (PurchaseResponseDelegator item in eventListeners[key])
					{
						if (item.responseDelegate == responseDelegate)
						{
							eventListeners[key].Remove(item);
							break;
						}
					}
				}
			}

			public override void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				Start();
				string key = "getProductDataResponse";
				lock (eventLock)
				{
					if (eventListeners.ContainsKey(key))
					{
						eventListeners[key].Add(new GetProductDataResponseDelegator(responseDelegate));
						return;
					}
					List<IDelegator> list = new List<IDelegator>();
					list.Add(new GetProductDataResponseDelegator(responseDelegate));
					eventListeners.Add(key, list);
				}
			}

			public override void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				Start();
				string key = "getProductDataResponse";
				lock (eventLock)
				{
					if (!eventListeners.ContainsKey(key))
					{
						return;
					}
					foreach (GetProductDataResponseDelegator item in eventListeners[key])
					{
						if (item.responseDelegate == responseDelegate)
						{
							eventListeners[key].Remove(item);
							break;
						}
					}
				}
			}

			public override void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				Start();
				string key = "getPurchaseUpdatesResponse";
				lock (eventLock)
				{
					if (eventListeners.ContainsKey(key))
					{
						eventListeners[key].Add(new GetPurchaseUpdatesResponseDelegator(responseDelegate));
						return;
					}
					List<IDelegator> list = new List<IDelegator>();
					list.Add(new GetPurchaseUpdatesResponseDelegator(responseDelegate));
					eventListeners.Add(key, list);
				}
			}

			public override void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				Start();
				string key = "getPurchaseUpdatesResponse";
				lock (eventLock)
				{
					if (!eventListeners.ContainsKey(key))
					{
						return;
					}
					foreach (GetPurchaseUpdatesResponseDelegator item in eventListeners[key])
					{
						if (item.responseDelegate == responseDelegate)
						{
							eventListeners[key].Remove(item);
							break;
						}
					}
				}
			}
		}

		internal class AmazonIapV2Default : AmazonIapV2Base
		{
			protected override void Init()
			{
			}

			protected override void RegisterCallback()
			{
			}

			protected override void RegisterEventListener()
			{
			}

			protected override void RegisterCrossPlatformTool()
			{
			}

			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativePurchaseJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return "{}";
			}
		}

		internal abstract class AmazonIapV2DelegatesBase : AmazonIapV2Base
		{
			private const string CrossPlatformTool = "XAMARIN";

			protected CallbackDelegate callbackDelegate;

			protected CallbackDelegate eventDelegate;

			protected override void Init()
			{
				NativeInit();
			}

			protected override void RegisterCallback()
			{
				callbackDelegate = callback;
				NativeRegisterCallback(callbackDelegate);
			}

			protected override void RegisterEventListener()
			{
				eventDelegate = FireEvent;
				NativeRegisterEventListener(eventDelegate);
			}

			protected override void RegisterCrossPlatformTool()
			{
				NativeRegisterCrossPlatformTool("XAMARIN");
			}

			public override void UnityFireEvent(string jsonMessage)
			{
				throw new NotSupportedException("UnityFireEvent is not supported");
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCallback(CallbackDelegate callback);

			protected abstract void NativeRegisterEventListener(CallbackDelegate callback);

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);
		}

		protected delegate void CallbackDelegate(string jsonMessage);

		internal class Builder
		{
			internal static readonly IAmazonIapV2 instance;

			static Builder()
			{
				instance = new AmazonIapV2Default();
			}
		}

		private static AmazonLogger logger;

		private static readonly Dictionary<string, IDelegator> callbackDictionary = new Dictionary<string, IDelegator>();

		private static readonly object callbackLock = new object();

		private static readonly Dictionary<string, List<IDelegator>> eventListeners = new Dictionary<string, List<IDelegator>>();

		private static readonly object eventLock = new object();

		public static IAmazonIapV2 Instance
		{
			get
			{
				return Builder.instance;
			}
		}

		private AmazonIapV2Impl()
		{
		}

		public static void callback(string jsonMessage)
		{
			try
			{
				logger.Debug("Executing callback");
				Dictionary<string, object> obj = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				callbackCaller(callerId: obj["callerId"] as string, response: obj["response"] as Dictionary<string, object>);
			}
			catch (KeyNotFoundException inner)
			{
				logger.Debug("callerId not found in callback");
				throw new AmazonException("Internal Error: Unknown callback id", inner);
			}
			catch (AmazonException ex)
			{
				logger.Debug("Async call threw exception: " + ex.ToString());
			}
		}

		private static void callbackCaller(Dictionary<string, object> response, string callerId)
		{
			IDelegator delegator = null;
			try
			{
				Jsonable.CheckForErrors(response);
				lock (callbackLock)
				{
					delegator = callbackDictionary[callerId];
					callbackDictionary.Remove(callerId);
					delegator.ExecuteSuccess(response);
				}
			}
			catch (AmazonException e)
			{
				lock (callbackLock)
				{
					if (delegator == null)
					{
						delegator = callbackDictionary[callerId];
					}
					callbackDictionary.Remove(callerId);
					delegator.ExecuteError(e);
				}
			}
		}

		public static void FireEvent(string jsonMessage)
		{
			try
			{
				logger.Debug("eventReceived");
				Dictionary<string, object> dictionary = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				string key = dictionary["eventId"] as string;
				Dictionary<string, object> dictionary2 = null;
				if (dictionary.ContainsKey("response"))
				{
					dictionary2 = dictionary["response"] as Dictionary<string, object>;
					Jsonable.CheckForErrors(dictionary2);
				}
				lock (eventLock)
				{
					foreach (IDelegator item in eventListeners[key])
					{
						if (dictionary2 != null)
						{
							item.ExecuteSuccess(dictionary2);
						}
						else
						{
							item.ExecuteSuccess();
						}
					}
				}
			}
			catch (AmazonException ex)
			{
				logger.Debug("Event call threw exception: " + ex.ToString());
			}
		}

		public abstract RequestOutput GetUserData();

		public abstract RequestOutput Purchase(SkuInput skuInput);

		public abstract RequestOutput GetProductData(SkusInput skusInput);

		public abstract RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		public abstract void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		public abstract void UnityFireEvent(string jsonMessage);

		public abstract void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public abstract void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public abstract void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		public abstract void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);
	}
}
