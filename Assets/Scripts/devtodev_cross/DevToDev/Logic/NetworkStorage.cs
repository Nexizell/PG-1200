using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.CustomEvent;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Logic
{
	internal class NetworkStorage : IStorageable<NetworkStorage>
	{
		private static readonly string WORKER = "worker";

		private static readonly string COUNT_FOR_REQUEST = "countForRequest";

		private static readonly string EVENT_PARAMS_COUNT = "eventParamsCount";

		private static readonly string TIME_FOR_REQUEST = "timeForRequest";

		private static readonly string SERVER_TIME = "serverTime";

		private static readonly string CACHE_TIMEOUT = "cacheTimeout";

		private static readonly string CONFIG_VERSION = "configVersion";

		private static readonly string USE_CUSTOM_UDID = "useCustomUDID";

		private static readonly string SESSION_DELAY = "sessionDelay";

		private static readonly string EXCLUDE = "exclude";

		private static readonly string EXCLUDE_ALL_PARAM = "all";

		private static readonly string RESULT = "result";

		private string worker;

		private string configVersion;

		private long serverTime;

		private long sessionDelay = 20L;

		private bool excludeAll;

		private bool isUseCUID;

		private int sendCount = 10;

		private int customEventParamsCount = 10;

		private int timeForRequest = 120;

		private int cacheTimeout;

		private Dictionary<EventType, List<Event>> excludeMetrics;

		private List<string> excludedUserData;

		public string ActiveNodeUrl
		{
			get
			{
				return worker;
			}
		}

		public int TimeForRequest
		{
			get
			{
				return timeForRequest;
			}
		}

		public int CountForRequest
		{
			get
			{
				return sendCount;
			}
		}

		public int EventParamsCount
		{
			get
			{
				return customEventParamsCount;
			}
		}

		public long ServerTime
		{
			get
			{
				return serverTime;
			}
		}

		public int CacheTimeout
		{
			get
			{
				return cacheTimeout;
			}
		}

		public string ConfigVersion
		{
			get
			{
				return configVersion;
			}
		}

		public bool UseCustomUdid
		{
			get
			{
				return isUseCUID;
			}
		}

		public Dictionary<EventType, List<Event>> ExcludeMetrics
		{
			get
			{
				return excludeMetrics;
			}
		}

		public bool ExcludeAll
		{
			get
			{
				return excludeAll;
			}
		}

		public long SessionDelay
		{
			get
			{
				return sessionDelay;
			}
		}

		public List<string> ExcludedUserData
		{
			get
			{
				return excludedUserData;
			}
		}

		public override string StorageName()
		{
			return "networkStorage.dat";
		}

		public override ISaveable GetBlankObject()
		{
			return new NetworkStorage();
		}

		public override ISaveable GetObject(byte[] data)
		{
			return new Formatter<NetworkStorage>().Load(data);
		}

		public override byte[] SaveObject(ISaveable obj)
		{
			return new Formatter<NetworkStorage>().Save(obj as NetworkStorage);
		}

		public NetworkStorage()
		{
		}

		public NetworkStorage(ObjectInfo info)
		{
			try
			{
				worker = info.GetValue("worker", typeof(string)) as string;
				configVersion = info.GetValue("configVersion", typeof(string)) as string;
				sessionDelay = (long)info.GetValue("sessionDelay", typeof(long));
				excludeAll = (bool)info.GetValue("excludeAll", typeof(bool));
				isUseCUID = (bool)info.GetValue("isUseCUID", typeof(bool));
				sendCount = (int)info.GetValue("sendCount", typeof(int));
				customEventParamsCount = (int)info.GetValue("customEventParamsCount", typeof(int));
				timeForRequest = (int)info.GetValue("timeForRequest", typeof(int));
				cacheTimeout = (int)info.GetValue("cacheTimeout", typeof(int));
				excludeMetrics = info.GetValue("excludeMetrics", typeof(Dictionary<EventType, List<Event>>)) as Dictionary<EventType, List<Event>>;
				serverTime = (long)info.GetValue("serverTime", typeof(long));
				excludedUserData = info.GetValue("excludedUserData", typeof(List<string>)) as List<string>;
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
				info.AddValue("worker", worker);
				info.AddValue("configVersion", configVersion);
				info.AddValue("serverTime", serverTime);
				info.AddValue("sessionDelay", sessionDelay);
				info.AddValue("excludeAll", excludeAll);
				info.AddValue("isUseCUID", isUseCUID);
				info.AddValue("sendCount", sendCount);
				info.AddValue("customEventParamsCount", customEventParamsCount);
				info.AddValue("timeForRequest", timeForRequest);
				info.AddValue("cacheTimeout", cacheTimeout);
				info.AddValue("excludeMetrics", excludeMetrics);
				info.AddValue("excludedUserData", excludedUserData);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void Load(string jsonParams)
		{
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(jsonParams);
			}
			catch (Exception ex)
			{
				Log.D(ex.StackTrace);
				return;
			}
			if (jSONNode[WORKER] != null)
			{
				worker = jSONNode[WORKER];
			}
			if (jSONNode[COUNT_FOR_REQUEST] != null)
			{
				sendCount = jSONNode[COUNT_FOR_REQUEST].AsInt;
			}
			if (jSONNode[EVENT_PARAMS_COUNT] != null)
			{
				customEventParamsCount = jSONNode[EVENT_PARAMS_COUNT].AsInt;
			}
			if (jSONNode[TIME_FOR_REQUEST] != null)
			{
				timeForRequest = jSONNode[TIME_FOR_REQUEST].AsInt;
			}
			if (jSONNode[CONFIG_VERSION] != null)
			{
				configVersion = jSONNode[CONFIG_VERSION];
			}
			if (jSONNode[SERVER_TIME] != null)
			{
				serverTime = jSONNode[SERVER_TIME].AsLong;
			}
			if (jSONNode[CACHE_TIMEOUT] != null)
			{
				serverTime = jSONNode[CACHE_TIMEOUT].AsInt;
			}
			if (jSONNode[SESSION_DELAY] != null)
			{
				sessionDelay = jSONNode[SESSION_DELAY].AsInt;
			}
			if (jSONNode[USE_CUSTOM_UDID] != null)
			{
				isUseCUID = jSONNode[USE_CUSTOM_UDID].AsBool;
			}
			else
			{
				isUseCUID = false;
			}
			if (jSONNode[RESULT] != null)
			{
				switch ((ServerConfigResult)jSONNode[RESULT].AsInt)
				{
				case ServerConfigResult.UpdateConfig:
					LoadExcludeData(jSONNode[EXCLUDE]);
					break;
				case ServerConfigResult.ClearAllRestrictions:
					ClearExcludeData();
					configVersion = null;
					break;
				}
			}
			else if (jSONNode[EXCLUDE] != null)
			{
				LoadExcludeData(jSONNode[EXCLUDE]);
			}
			Log.D("Got worker: " + worker);
		}

		public bool ShouldAddMetric(Event metric)
		{
			if (ExcludeAll)
			{
				return false;
			}
			if (ExcludeMetrics == null)
			{
				return true;
			}
			if (!ExcludeMetrics.ContainsKey(metric.MetricType))
			{
				return true;
			}
			if (ExcludeMetrics.ContainsKey(metric.MetricType) && ExcludeMetrics[metric.MetricType] == null)
			{
				return false;
			}
			foreach (Event item in ExcludeMetrics[metric.MetricType])
			{
				if (metric.IsEqualToMetric(item))
				{
					return false;
				}
			}
			return true;
		}

		private void LoadExcludeData(JSONNode data)
		{
			if (data is JSONClass)
			{
				excludeAll = false;
				excludeMetrics = new Dictionary<EventType, List<Event>>();
				{
					foreach (KeyValuePair<string, JSONNode> item in (JSONClass)data)
					{
						EventType eventTypeByCode = EventConst.GetEventTypeByCode(item.Key);
						if (eventTypeByCode == EventType.UserCard)
						{
							JSONArray asArray = item.Value.AsArray;
							if (excludedUserData == null)
							{
								excludedUserData = new List<string>();
							}
							if (asArray == null || asArray.Count == 0)
							{
								if (excludeMetrics == null)
								{
									excludeMetrics = new Dictionary<EventType, List<Event>>();
								}
								excludeMetrics.Add(eventTypeByCode, null);
								continue;
							}
							foreach (JSONNode item2 in asArray)
							{
								excludedUserData.Add(item2.ToString());
							}
						}
						else
						{
							JSONArray asArray2 = item.Value.AsArray;
							if (ExcludeMetrics == null)
							{
								excludeMetrics = new Dictionary<EventType, List<Event>>();
							}
							if (asArray2 == null || asArray2.Count == 0)
							{
								excludeMetrics.Add(eventTypeByCode, null);
								continue;
							}
							List<Event> list = new List<Event>();
							AddMetricsToList(eventTypeByCode, list, asArray2);
							excludeMetrics.Add(eventTypeByCode, list);
						}
					}
					return;
				}
			}
			if (data.ToString().Equals(EXCLUDE_ALL_PARAM))
			{
				excludeAll = true;
			}
		}

		private void AddMetricsToList(EventType type, List<Event> metricList, JSONArray metricsParams)
		{
			foreach (JSONNode metricsParam in metricsParams)
			{
				switch (type)
				{
				case EventType.CustomEvent:
				{
					CustomEventData customEventData = CustomEventData.SingleData(metricsParam, null);
					metricList.Add(new CustomEvent(customEventData));
					break;
				}
				case EventType.Tutorial:
					metricList.Add(new TutorialEvent(metricsParam.AsInt));
					break;
				case EventType.InAppPurchase:
					metricList.Add(new InAppPurchase(metricsParam, "", -1, -1f, ""));
					break;
				case EventType.SocialNetworkConnect:
					metricList.Add(new SocialNetworkConnectEvent(SocialNetwork.Custom(metricsParam)));
					break;
				case EventType.SocialNetworkPost:
					metricList.Add(new SocialNetworkConnectEvent(SocialNetwork.Custom(metricsParam)));
					break;
				}
			}
		}

		private void ClearExcludeData()
		{
			excludeAll = false;
			excludeMetrics = null;
		}
	}
}
