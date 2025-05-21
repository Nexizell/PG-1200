using System.Collections.Generic;

namespace DevToDev.Core.Data.Consts
{
	internal static class EventConst
	{
		public static readonly Dictionary<EventType, KeyValuePair<string, string>> EventsInfo = new Dictionary<EventType, KeyValuePair<string, string>>
		{
			{
				EventType.ReceiptValidation,
				new KeyValuePair<string, string>("Receipt", "rc")
			},
			{
				EventType.Age,
				new KeyValuePair<string, string>("Age", "ag")
			},
			{
				EventType.ApplicationInfo,
				new KeyValuePair<string, string>("Application Info", "ai")
			},
			{
				EventType.Cheater,
				new KeyValuePair<string, string>("Cheater", "ch")
			},
			{
				EventType.GameSession,
				new KeyValuePair<string, string>("Game Session", "gs")
			},
			{
				EventType.Gender,
				new KeyValuePair<string, string>("Gender", "gr")
			},
			{
				EventType.Location,
				new KeyValuePair<string, string>("Location", "cr")
			},
			{
				EventType.SocialNetworkConnect,
				new KeyValuePair<string, string>("Connect To Social Network", "sc")
			},
			{
				EventType.SocialNetworkPost,
				new KeyValuePair<string, string>("Social Network Post", "sp")
			},
			{
				EventType.Tutorial,
				new KeyValuePair<string, string>("Tutorial", "tr")
			},
			{
				EventType.DeviceInfo,
				new KeyValuePair<string, string>("Device Info", "di")
			},
			{
				EventType.UserInfo,
				new KeyValuePair<string, string>("User Info", "ui")
			},
			{
				EventType.InAppPurchase,
				new KeyValuePair<string, string>("In App Purchase", "ip")
			},
			{
				EventType.RealPayment,
				new KeyValuePair<string, string>("Real Payment", "rp")
			},
			{
				EventType.CustomEvent,
				new KeyValuePair<string, string>("Custom event", "ce")
			},
			{
				EventType.ApplicationList,
				new KeyValuePair<string, string>("ApplicationsList", "al")
			},
			{
				EventType.BannerClick,
				new KeyValuePair<string, string>("Banner Click", "bc")
			},
			{
				EventType.BannerShow,
				new KeyValuePair<string, string>("Banner Show", "bs")
			},
			{
				EventType.FacebookInfo,
				new KeyValuePair<string, string>("Facebook Info", "fb")
			},
			{
				EventType.InstallEvent,
				new KeyValuePair<string, string>("Install Event", "it")
			},
			{
				EventType.SelfInfo,
				new KeyValuePair<string, string>("Self Info", "si")
			},
			{
				EventType.PushToken,
				new KeyValuePair<string, string>("Push Token", "pt")
			},
			{
				EventType.PushReceived,
				new KeyValuePair<string, string>("Push Received", "pr")
			},
			{
				EventType.PushOpen,
				new KeyValuePair<string, string>("Push Open", "po")
			},
			{
				EventType.GetServerNode,
				new KeyValuePair<string, string>("Get Server Node", "sn")
			},
			{
				EventType.Referral,
				new KeyValuePair<string, string>("Referral", "rf")
			},
			{
				EventType.UserCard,
				new KeyValuePair<string, string>("User Card", "pl")
			},
			{
				EventType.ProgressionEvent,
				new KeyValuePair<string, string>("Progression Event", "pe")
			}
		};

		public static EventType GetEventTypeByCode(string code)
		{
			foreach (KeyValuePair<EventType, KeyValuePair<string, string>> item in EventsInfo)
			{
				if (item.Value.Value.ToUpper().Equals(code.ToUpper()))
				{
					return item.Key;
				}
			}
			return EventType.Undefined;
		}

		public static bool IsFastSend(EventType eventType)
		{
			if (eventType != EventType.RealPayment && eventType != EventType.ReceiptValidation && eventType != EventType.GameSession && eventType != EventType.BannerClick && eventType != EventType.BannerShow && eventType != EventType.PushToken && eventType != EventType.PushReceived)
			{
				return eventType == EventType.PushOpen;
			}
			return true;
		}

		public static bool IsReplacing(EventType eventType)
		{
			if (eventType != EventType.ApplicationInfo && eventType != EventType.DeviceInfo && eventType != EventType.UserInfo)
			{
				return eventType == EventType.PushToken;
			}
			return true;
		}
	}
}
