using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class LocalNotificationController : MonoBehaviour
	{
		internal enum Priority
		{
			Gacha = 0,
			Pets = 1,
			Tickets = 2,
			Craft = 3,
			Eggs = 4,
			DoubleReward = 5,
			Leprechaun = 6,
			Return = 7
		}

		internal struct PreparedLocalNotification
		{
			private string _type;

			private object _platformSpecificLocalNotification;

			private int _priority;

			private DateTime _scheduledMoment;

			internal string Type
			{
				get
				{
					return _type ?? string.Empty;
				}
			}

			internal object PlatformSpecificLocalNotification
			{
				get
				{
					return _platformSpecificLocalNotification;
				}
			}

			internal int Priority
			{
				get
				{
					return _priority;
				}
			}

			internal DateTime ScheduledMoment
			{
				get
				{
					return _scheduledMoment;
				}
			}

			internal PreparedLocalNotification(string type, object platformSpecificLocalNotification, int priority, DateTime scheduledMoment)
			{
				_type = type ?? string.Empty;
				_platformSpecificLocalNotification = platformSpecificLocalNotification;
				_priority = priority;
				_scheduledMoment = scheduledMoment;
			}
		}

		internal struct LocalNotification
		{
			private readonly string _title;

			private readonly string _subtitle;

			private readonly string _ticker;

			public string Title
			{
				get
				{
					return _title ?? string.Empty;
				}
			}

			public string Subtitle
			{
				get
				{
					return _subtitle ?? string.Empty;
				}
			}

			public string Ticker
			{
				get
				{
					return _ticker ?? string.Empty;
				}
			}

			public LocalNotification(string title, string subtitle, string ticker)
			{
				_title = title ?? string.Empty;
				_subtitle = subtitle ?? string.Empty;
				_ticker = ticker ?? string.Empty;
			}

			public static LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey, string tickerKey)
			{
				string title = LocalizationStore.Get(titleKey ?? string.Empty);
				string subtitle = LocalizationStore.Get(subtitleKey ?? string.Empty);
				string ticker = LocalizationStore.Get(tickerKey ?? string.Empty);
				return new LocalNotification(title, subtitle, ticker);
			}

			public static LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey)
			{
				return FromLocalizationKeys(titleKey, subtitleKey, titleKey);
			}
		}

		private bool _paused;

		private static readonly Dictionary<string, string> s_emojiKeys = new Dictionary<string, string>
		{
			{ "Key_2225", "Key_2973" },
			{ "Key_2239", "Key_2968" },
			{ "Key_2240", "Key_2969" },
			{ "Key_2247", "Key_2975" },
			{ "Key_2248", "Key_2976" },
			{ "Key_2284", "Key_2974" },
			{ "Key_2800", "Key_2967" },
			{ "Key_2801", "Key_2965" },
			{ "Key_2802", "Key_2966" },
			{ "Key_2803", "Key_2970" },
			{ "Key_2804", "Key_2971" },
			{ "Key_2805", "Key_2972" },
			{ "Key_2988", "Key_2958" },
			{ "Key_2989", "Key_2960" },
			{ "Key_3211", "Key_3210" },
			{ "Key_3213", "Key_3212" },
			{ "Key_3215", "Key_3214" }
		};

		private const int GachaNotificationId = 1000;

		private const int ReturnNotificationId = 2000;

		private const int EggNotificationId = 3000;

		private const int PetNotificationId = 4000;

		private const int LeprechaunNotificationId = 5000;

		private const int CraftNotificationId = 6000;

		private const int TicketsNotificationId = 7000;

		private const int DoubleRewardNotificationId = 8000;

		private readonly List<LocalNotification> _returnNotifications = new List<LocalNotification>(4);

		private bool EggsNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool GachaNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool ReturnNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool PetsNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool LeprechaunNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool CraftNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool TicketsNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private bool DoubleRewardNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private List<LocalNotification> ReturnNotifications
		{
			get
			{
				return _returnNotifications;
			}
		}

		public LocalNotificationController()
		{
			Func<string, string> func = GetCorrespondingRichEmojiKey;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2225"), func("Key_2239"), func("Key_2247")));
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2225"), func("Key_2240"), func("Key_2248")));
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2225"), func("Key_2239"), func("Key_2248")));
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2225"), func("Key_2240"), func("Key_2247")));
			}
			else
			{
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2239"), func("Key_2284")));
				_returnNotifications.Add(LocalNotification.FromLocalizationKeys(func("Key_2240"), func("Key_2284")));
			}
		}

		private void Awake()
		{
			CancelNotifications();
		}

		private void OnApplicationQuit()
		{
			string callee = "LocalNotificationController.OnApplicationQuit()";
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild && !Tools.IsEditor);
			try
			{
				if (Application.platform != RuntimePlatform.IPhonePlayer || !_paused)
				{
					ScheduleNotifications();
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			string callee = string.Format("{0}.OnApplicationPause({1})", new object[2]
			{
				GetType().Name,
				pauseStatus
			});
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild && !Tools.IsEditor);
			try
			{
				_paused = pauseStatus;
				if (pauseStatus)
				{
					ScheduleNotifications();
				}
				else
				{
					CancelNotifications();
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		private void ScheduleNotifications()
		{
			string text = "LocalNotificationController.ScheduleNotifications()";
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild && !Tools.IsEditor);
			try
			{
				List<PreparedLocalNotification> list = new List<PreparedLocalNotification>(3);
				ScheduleEggsNotifications(list);
				SchedulePetsNotifications(list, text);
				ScheduleGachaNotifications(list, text);
				ScheduleReturnNotifications(list);
				ScheduleLeprechaunNotifications(list);
				ScheduleCraftNotifications(list);
				ScheduleTicketsNotifications(list);
				ScheduleDoubleRewardNotifications(list);
				DateTime now = DateTime.Now;
				foreach (PreparedLocalNotification item in (from p in list
					group p by Convert.ToInt32(Math.Floor(p.ScheduledMoment.Subtract(now).TotalDays)) into g
					select g.OrderBy((PreparedLocalNotification p) => p.Priority).Take(3)).SelectMany((IEnumerable<PreparedLocalNotification> ps) => ps).ToList())
				{
					if ((object)null == null)
					{
						Debug.LogFormat("`{0}`: platformSpecificLocalNotification == null", item.Type);
					}
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		private void ScheduleEggsNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (EggsNotificationEnabled && !(Nest.Instance == null) && !Nest.Instance.EggIsReady && Nest.Instance.TimeLeft.HasValue)
			{
				long value = Nest.Instance.TimeLeft.Value;
				DateTime scheduledMoment = DateTime.Now.AddSeconds(value);
				object platformSpecificLocalNotification = null;
				PreparedLocalNotification item = new PreparedLocalNotification("Eggs", platformSpecificLocalNotification, 4, scheduledMoment);
				preparedLocalNotifications.Add(item);
			}
		}

		private void SchedulePetsNotifications(List<PreparedLocalNotification> preparedLocalNotifications, string caller)
		{
			if (!PetsNotificationEnabled || Singleton<EggsManager>.Instance == null)
			{
				return;
			}
			List<Egg> playerEggs = Singleton<EggsManager>.Instance.GetPlayerEggs();
			if (playerEggs == null || playerEggs.Count == 0)
			{
				return;
			}
			Egg egg = null;
			for (int i = 0; i != playerEggs.Count; i++)
			{
				Egg egg2 = playerEggs[i];
				if (egg2.HatchedType == EggHatchedType.Time && egg2.IncubationTimeLeft.HasValue && !egg2.CheckReady())
				{
					if (egg == null)
					{
						egg = egg2;
					}
					else if (egg2.IncubationTimeLeft.Value < egg.IncubationTimeLeft.Value)
					{
						egg = egg2;
					}
				}
			}
			if (egg == null)
			{
				return;
			}
			long value = egg.IncubationTimeLeft.Value;
			DateTime scheduledMoment = DateTime.Now.AddSeconds(value);
			string callee = string.Format(CultureInfo.InvariantCulture, "Scheduling pet notification in {0}", value);
			using (new ScopeLogger(caller, callee, true))
			{
				object platformSpecificLocalNotification = null;
				PreparedLocalNotification item = new PreparedLocalNotification("Pets", platformSpecificLocalNotification, 1, scheduledMoment);
				preparedLocalNotifications.Add(item);
			}
		}

		private void ScheduleGachaNotifications(List<PreparedLocalNotification> preparedLocalNotifications, string caller)
		{
			if (!GachaNotificationEnabled || ExperienceController.GetCurrentLevel() < 2 || GiftController.Instance == null)
			{
				return;
			}
			TimeSpan freeGachaAvailableIn = GiftController.Instance.FreeGachaAvailableIn;
			long num = (long)freeGachaAvailableIn.TotalSeconds;
			if (num <= 0)
			{
				return;
			}
			DateTime scheduledMoment = DateTime.Now.AddSeconds(num);
			string callee = string.Format(CultureInfo.InvariantCulture, "Scheduling gacha notification in {0}", freeGachaAvailableIn);
			using (new ScopeLogger(caller, callee, true))
			{
				object platformSpecificLocalNotification = null;
				PreparedLocalNotification item = new PreparedLocalNotification("Gacha", platformSpecificLocalNotification, 0, scheduledMoment);
				preparedLocalNotifications.Add(item);
			}
		}

		private void ScheduleReturnNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (ReturnNotificationEnabled)
			{
				DateTime now = DateTime.Now;
				List<DateTime> list = new List<DateTime>(3)
				{
					ClampTimeOfTheDay(now.AddDays(3.0)),
					ClampTimeOfTheDay(now.AddDays(7.0))
				};
				int num = UnityEngine.Random.Range(0, ReturnNotifications.Count);
				int count = list.Count;
				for (int i = 0; i != count; i++)
				{
					DateTime dateTime = list[i];
					Convert.ToInt32((dateTime - now).TotalSeconds);
					int index = (num + i) % ReturnNotifications.Count;
					LocalNotification localNotification = ReturnNotifications[index];
					object platformSpecificLocalNotification = null;
					PreparedLocalNotification item = new PreparedLocalNotification("Return", platformSpecificLocalNotification, 7, dateTime);
					preparedLocalNotifications.Add(item);
				}
			}
		}

		private void ScheduleLeprechaunNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (!LeprechaunNotificationEnabled || !Singleton<LeprechauntManager>.Instance.LeprechauntExists)
			{
				return;
			}
			TimeSpan timeSpan = TimeSpan.FromDays(2.0);
			double totalSecond = timeSpan.TotalSeconds;
			if (Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
			{
				DateTime scheduledMoment = DateTime.Now + timeSpan;
				object platformSpecificLocalNotification = null;
				PreparedLocalNotification item = new PreparedLocalNotification("Leprechaun", platformSpecificLocalNotification, 6, scheduledMoment);
				preparedLocalNotifications.Add(item);
				return;
			}
			float? rewardDropSecsLeft = Singleton<LeprechauntManager>.Instance.RewardDropSecsLeft;
			if (rewardDropSecsLeft.HasValue)
			{
				long num = (long)rewardDropSecsLeft.Value;
				if (num > 0)
				{
					DateTime dateTime = DateTime.Now.AddSeconds(num) + timeSpan;
				}
			}
		}

		private void ScheduleCraftNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (CraftNotificationEnabled && !(Singleton<LobbyItemsController>.Instance == null) && Singleton<LobbyItemsController>.Instance.CraftingItem != null)
			{
				long craftTimeLeft = Singleton<LobbyItemsController>.Instance.CraftingItem.CraftTimeLeft;
				if (craftTimeLeft > 0)
				{
					DateTime scheduledMoment = DateTime.Now.AddSeconds(craftTimeLeft);
					object platformSpecificLocalNotification = null;
					PreparedLocalNotification item = new PreparedLocalNotification("Craft", platformSpecificLocalNotification, 3, scheduledMoment);
					preparedLocalNotifications.Add(item);
				}
			}
		}

		private void ScheduleTicketsNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (TicketsNotificationEnabled && !(FreeTicketsController.Instance == null) && FreeTicketsController.Instance.IsCountingFreeTickets())
			{
				long num = FreeTicketsController.Instance.EstimatedTimeUntilAllTicketsInSeconds;
				if (num > 0)
				{
					DateTime scheduledMoment = DateTime.Now.AddSeconds(num);
					object platformSpecificLocalNotification = null;
					PreparedLocalNotification item = new PreparedLocalNotification("Tickets", platformSpecificLocalNotification, 2, scheduledMoment);
					preparedLocalNotifications.Add(item);
				}
			}
		}

		private void ScheduleDoubleRewardNotifications(List<PreparedLocalNotification> preparedLocalNotifications)
		{
			if (DoubleRewardNotificationEnabled && !(ExperienceController.sharedController == null) && ExperienceController.sharedController.currentLevel >= 2 && !DoubleReward.Instance.SupportedModes.Where((GameConnect.GameMode m) => !GameModeLocker.Instance.IsLocked(m)).All(DoubleReward.Instance.NeedDoubleReward))
			{
				long? num = DoubleReward.Instance.EstimateNextTimeForDoubleRewardInSeconds();
				if (num.HasValue)
				{
					DateTime scheduledMoment = DateTime.Now.AddSeconds(num.Value);
					object platformSpecificLocalNotification = null;
					PreparedLocalNotification item = new PreparedLocalNotification("DoubleReward", platformSpecificLocalNotification, 5, scheduledMoment);
					preparedLocalNotifications.Add(item);
				}
			}
		}

		private void CancelNotifications()
		{
			string callee = "LocalNotificationController.CancelNotifications()";
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild && !Tools.IsEditor);
			try
			{
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		private static bool IsNightTime(DateTime dateTime)
		{
			if (dateTime.Hour < 22)
			{
				return dateTime.Hour < 10;
			}
			return true;
		}

		private DateTime ClampTimeOfTheDay(DateTime rawDateTime)
		{
			TimeSpan timeSpan = new TimeSpan(16, 0, 0);
			TimeSpan timeSpan2 = TimeSpan.FromMinutes(UnityEngine.Random.Range(-30f, 30f));
			return rawDateTime.Date + timeSpan + timeSpan2;
		}

		private static int SafeGetSdkLevel()
		{
			try
			{
				return AndroidSystem.GetSdkVersion();
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				return 0;
			}
		}

		private static string GetCorrespondingRichEmojiKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			if (!EmojiSupported())
			{
				return key;
			}
			string value;
			if (s_emojiKeys.TryGetValue(key, out value))
			{
				return value ?? key;
			}
			return key;
		}

		private static bool EmojiSupported()
		{
			if (Tools.IsEditor)
			{
				return true;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return SafeGetSdkLevel() >= 23;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return true;
			}
			return false;
		}
	}
}
