using System;
using System.Collections.Generic;
using DevToDev.Cheat;
using DevToDev.Core.Network;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.CustomEvent;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Simple;
using DevToDev.Data.Metrics.Specific;
using DevToDev.Logic.Cross;
using DevToDev.Push.Data.Metrics.Simple;
using UnityEngine;

namespace DevToDev.Logic
{
	internal class SDKClient
	{
		private static List<ISaveable> storages;

		private string appKey;

		private string appSecret;

		private NetworkStorage networkStorage;

		private UsersStorage usersStorage;

		private MetricsController metricsController;

		private List<DevToDev.Data.Metrics.Event> futureEvents;

		private List<Action> futureExecutions;

		private List<Action> futureExecutionsAfterInit;

		private bool isInited;

		private bool initWasCalled;

		private People activeUser;

		private static SDKClient instance;

		public NetworkStorage NetworkStorage
		{
			get
			{
				if (networkStorage == null)
				{
					networkStorage = new NetworkStorage().Load() as NetworkStorage;
				}
				return networkStorage;
			}
		}

		public AsyncOperationDispatcher AsyncOperationDispatcher
		{
			get
			{
				return AsyncOperationDispatcher.Create();
			}
		}

		public UsersStorage UsersStorage
		{
			get
			{
				if (usersStorage == null)
				{
					usersStorage = new UsersStorage().Load() as UsersStorage;
					try
					{
						usersStorage.LoadNative();
					}
					catch (Exception ex)
					{
						Log.E("Error loading native data " + ex.Message + "\r\n" + ex.StackTrace);
						new NativeDataLoader().RemoveNativeData();
					}
				}
				return usersStorage;
			}
		}

		public MetricsController MetricsController
		{
			get
			{
				if (metricsController == null)
				{
					metricsController = new MetricsController();
				}
				return metricsController;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return isInited;
			}
		}

		public static SDKClient Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new SDKClient();
				}
				return instance;
			}
		}

		public string ApplicationVersion
		{
			get
			{
				if (!initWasCalled)
				{
					return null;
				}
				return UsersStorage.ApplicationVersion;
			}
			set
			{
				ExecuteFirstUsers(delegate
				{
					UsersStorage.ApplicationVersion = value;
				});
			}
		}

		public string ActiveUserId
		{
			get
			{
				if (!initWasCalled)
				{
					return null;
				}
				return UsersStorage.ActiveUserId;
			}
			set
			{
				ExecuteFirstUsers(delegate
				{
					UsersStorage.ActiveUserId = value;
				});
			}
		}

		public string AppKey
		{
			get
			{
				return appKey;
			}
		}

		public string AppSecret
		{
			get
			{
				return appSecret;
			}
		}

		public People ActiveUser
		{
			get
			{
				if (activeUser == null)
				{
					activeUser = new People();
				}
				return activeUser;
			}
		}

		private SDKClient()
		{
			isInited = false;
			initWasCalled = false;
			futureEvents = new List<DevToDev.Data.Metrics.Event>();
			futureExecutions = new List<Action>();
			futureExecutionsAfterInit = new List<Action>();
			storages = new List<ISaveable>();
			storages.Add(new RFC4122(new ObjectInfo()));
			storages.Add(new DataStorage(new ObjectInfo()));
			storages.Add(new User(new ObjectInfo()));
			storages.Add(new UsersStorage(new ObjectInfo()));
			storages.Add(new Device(new ObjectInfo()));
			storages.Add(new LevelData(new ObjectInfo()));
			storages.Add(new MetricsStorage(new ObjectInfo()));
			storages.Add(new NetworkStorage(new ObjectInfo()));
			storages.Add(new SessionStorage(new ObjectInfo()));
			storages.Add(new UserMetrics(new ObjectInfo()));
			storages.Add(new AgeEvent(new ObjectInfo()));
			storages.Add(new ApplicationInfoEvent(new ObjectInfo()));
			storages.Add(new CheaterEvent(new ObjectInfo()));
			storages.Add(new DeviceInfoEvent(new ObjectInfo()));
			storages.Add(new GameSessionEvent(new ObjectInfo()));
			storages.Add(new GenderEvent(new ObjectInfo()));
			storages.Add(new GetServerNodeEvent(new ObjectInfo()));
			storages.Add(new LocationEvent(new ObjectInfo()));
			storages.Add(new SocialNetworkConnectEvent(new ObjectInfo()));
			storages.Add(new SocialNetworkPostEvent(new ObjectInfo()));
			storages.Add(new TutorialEvent(new ObjectInfo()));
			storages.Add(new UserInfoEvent(new ObjectInfo()));
			storages.Add(new RealPayment(new ObjectInfo()));
			storages.Add(new InAppPurchase(new ObjectInfo()));
			storages.Add(new CustomEvent(new ObjectInfo()));
			storages.Add(new CustomEventData(new ObjectInfo()));
			storages.Add(new CustomEventParams(new ObjectInfo()));
			storages.Add(new RealPaymentData(new ObjectInfo()));
			storages.Add(new InAppPurchaseData(new ObjectInfo()));
			storages.Add(new TokenSendEvent(new ObjectInfo()));
			storages.Add(new PushOpenEvent(new ObjectInfo()));
			storages.Add(new PushReceivedEvent(new ObjectInfo()));
			storages.Add(new VerifyEvent(new ObjectInfo()));
			storages.Add(new CheatData(new ObjectInfo()));
			storages.Add(new ReferralEvent(new ObjectInfo()));
			storages.Add(new PeopleEvent(new ObjectInfo()));
			storages.Add(new PeopleLogic(new ObjectInfo()));
			storages.Add(new ProgressionEventParams(new ObjectInfo()));
			storages.Add(new LocationEventParams(new ObjectInfo()));
			storages.Add(new ProgressionEvent(new ObjectInfo()));
		}

		public void Initialize(string appKey, string appSecret)
		{
			if (appKey == null || appSecret == null)
			{
				Log.R("Application keys can't be null.");
			}
			else if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appSecret))
			{
				Log.R("Application keys can't be empty.");
			}
			else
			{
				if (initWasCalled || isInited)
				{
					return;
				}
				initWasCalled = true;
				try
				{
					AsyncOperationDispatcher.Create();
				}
				catch (Exception)
				{
					throw new Exception("Initialize method should be called from Unity main thread. Don't worry, this is the only method that must be called from the main thread.");
				}
				this.appKey = appKey;
				this.appSecret = appSecret;
				Log.R(string.Format("Initializing sdk with key {0} and version {1}", this.appKey, Analytics.SDKVersion));
				MetricsController obj = MetricsController;
				obj.PeriodicSendHandler = (EventHandler)Delegate.Combine(obj.PeriodicSendHandler, new EventHandler(UsersStorage.OnPeriodicSend));
				MetricsController.Resume();
				foreach (Action item in futureExecutionsAfterInit)
				{
					item();
				}
				futureExecutionsAfterInit.Clear();
				SDKRequests.GetServerNodeV3(OnServerConfigReceived);
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.DTDApplicationLifecycle");
					string text = androidJavaClass.CallStatic<string>("readLostSession", new object[0]);
					Log.D("Last session was lost on " + text);
					long result = 0L;
					if (long.TryParse(text, out result))
					{
						UsersStorage.SessionClose(result, true);
					}
					StartSession();
					Execute(delegate
					{
						SaveAll();
					});
				}
				else
				{
					StartSession();
				}
			}
		}

		public void Resume()
		{
			MetricsController.Resume();
			StartSession();
		}

		public void Suspend(long timestamp = 0L)
		{
			EndSession(timestamp);
			MetricsController.Suspend();
		}

		public void StartSession()
		{
			Execute(delegate
			{
				Log.D("Executing start session");
				UsersStorage.SessionOpen();
			});
		}

		public bool IsSessionActive()
		{
			return usersStorage.ActiveUser.IsSessionActive();
		}

		public void EndSession(long timestamp = 0L)
		{
			Execute(delegate
			{
				Log.D("Executing stop session");
				UsersStorage.SessionClose(timestamp);
				SaveAll();
			});
		}

		public void LevelUp(int newLevel, Dictionary<string, int> values)
		{
			Execute(delegate
			{
				UsersStorage.ActiveUser.LevelUp(newLevel, values);
			});
		}

		public void Referral(IDictionary<ReferralProperty, string> referralData)
		{
			Execute(delegate
			{
				AddEvent(new ReferralEvent(referralData));
			});
		}

		public void Tutorial(int step)
		{
			if (step < TutorialState.Finish)
			{
				Log.R("Tutorial step cannot be less then -2");
				return;
			}
			Execute(delegate
			{
				AddEvent(new TutorialEvent(step));
			});
		}

		public void RealPayment(string transactionId, float inAppPrice, string inAppName, string inAppCurrencyISOCode)
		{
			if (transactionId != null && inAppName != null && inAppCurrencyISOCode != null)
			{
				Execute(delegate
				{
					AddEvent(new RealPayment(transactionId, inAppPrice, inAppName, inAppCurrencyISOCode));
				});
			}
		}

		public void SocialNetworkConnect(SocialNetwork network)
		{
			Execute(delegate
			{
				AddEvent(new SocialNetworkConnectEvent(network));
			});
		}

		public void SocialNetworkPost(SocialNetwork network, string reason)
		{
			if (reason != null)
			{
				Execute(delegate
				{
					AddEvent(new SocialNetworkPostEvent(network, reason));
				});
			}
		}

		public void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, Dictionary<string, int> resources)
		{
			if (purchaseId == null || purchaseType == null || resources == null)
			{
				return;
			}
			Execute(delegate
			{
				bool flag = true;
				foreach (KeyValuePair<string, int> resource in resources)
				{
					usersStorage.ActiveUser.UpSpend(resource.Value, resource.Key);
					AddEvent(new InAppPurchase(purchaseId, purchaseType, flag ? purchaseAmount : 0, resource.Value, resource.Key));
					flag = false;
				}
			});
		}

		public void CurrencyAccrual(string currencyName, int amount, AccrualType accrualType)
		{
			if (currencyName == null)
			{
				return;
			}
			Execute(delegate
			{
				if (accrualType == AccrualType.Purchased)
				{
					Log.D("Accrual Purchased: " + amount + " of " + currencyName);
					usersStorage.ActiveUser.UpBought(amount, currencyName);
				}
				else if (accrualType == AccrualType.Earned)
				{
					Log.D("Accrual Earned: " + amount + " of " + currencyName);
					usersStorage.ActiveUser.UpEarned(amount, currencyName);
				}
			});
		}

		public void CustomEvent(string eventName, CustomEventParams parameters)
		{
			if (eventName != null)
			{
				Execute(delegate
				{
					AddEvent(new CustomEvent(CustomEventData.SingleData(eventName, parameters)));
				});
			}
		}

		public void StartProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			if (eventId != null && eventParams != null)
			{
				Execute(delegate
				{
					eventParams.SetEventName(eventId);
					eventParams.SetStartTime(DeviceHelper.Instance.GetUnixTime());
					AddEvent(new ProgressionEvent(eventParams));
				});
			}
		}

		public void EndProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			if (eventId != null && eventParams != null)
			{
				Execute(delegate
				{
					eventParams.SetEventName(eventId);
					eventParams.SetFinishTime(DeviceHelper.Instance.GetUnixTime());
					AddEvent(new ProgressionEvent(eventParams));
				});
			}
		}

		public void Age(int age)
		{
			Execute(delegate
			{
				AddEvent(new AgeEvent(age));
			});
		}

		public void Gender(Gender gender)
		{
			Execute(delegate
			{
				AddEvent(new GenderEvent(gender));
			});
		}

		public void Cheater(bool isCheater)
		{
			Execute(delegate
			{
				AddEvent(new CheaterEvent(isCheater));
			});
		}

		public void SendBufferedEvents()
		{
			Execute(delegate
			{
				UsersStorage.ForceSendEvents();
			});
		}

		public void SetCurrentLevel(int currentLevel)
		{
			ExecuteFirstLevels(delegate
			{
				UsersStorage.ActiveUser.Level = currentLevel;
			});
		}

		public void ReplaceUserId(string fromUserId, string toUserId)
		{
			if (fromUserId != null && toUserId != null)
			{
				Execute(delegate
				{
					UsersStorage.ReplaceUserId(fromUserId, toUserId);
				});
			}
		}

		private void OnServerConfigReceived(Response response, object callbackData)
		{
			if (response.ResponseState == ResponseState.Success && response.ResposeString != null && !response.ResposeString.Equals(string.Empty))
			{
				Log.D("Server config received: " + response.ResposeString);
				NetworkStorage.Load(response.ResposeString);
				networkStorage.Save(networkStorage);
			}
			try
			{
				isInited = true;
				UsersStorage.ActiveUser.StartFastSendSession();
				UsersStorage.OnInitialized(futureEvents);
				futureEvents.Clear();
				foreach (Action futureExecution in futureExecutions)
				{
					futureExecution();
				}
				futureExecutions.Clear();
				if (PushManager.PushClient != null)
				{
					List<DevToDev.Data.Metrics.Event> events = PushManager.PushClient.GetEvents();
					UsersStorage.AddEvents(events);
				}
				UsersStorage.ActiveUser.StopFastSendSession();
			}
			catch (Exception ex)
			{
				Log.E(ex.Message + "\r\n" + ex.StackTrace);
			}
		}

		public void SaveAll()
		{
			if (IsInitialized)
			{
				usersStorage.Save(usersStorage);
			}
		}

		public void AddEvent(DevToDev.Data.Metrics.Event eventData)
		{
			if (!IsInitialized)
			{
				futureEvents.Add(eventData);
			}
			else
			{
				UsersStorage.AddEvent(eventData);
			}
		}

		public void Execute(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action();
			}
		}

		public void ExecuteFirstUsers(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action();
			}
		}

		public void ExecuteFirstLevels(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action();
			}
		}

		public void SetActiveLog(bool isActive)
		{
			Log.LogEnabled = isActive;
		}
	}
}
