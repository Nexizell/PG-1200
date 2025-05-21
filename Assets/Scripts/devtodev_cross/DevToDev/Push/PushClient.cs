using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Logic;
using DevToDev.Push.Data.Metrics.Simple;

namespace DevToDev.Push
{
	internal class PushClient
	{
		private static readonly string PUSH_NATIVE_DATA = "push_data";

		public OnPushTokenReceivedHandler OnPushTokenReceived;

		public OnPushTokenFailedHandler OnPushTokenFailed;

		public OnPushReceivedHandler OnPushReceived;

		private PushClientPlatform pushClientPlatform;

		public void onRegisteredForPushNotifications(string token)
		{
			if (token != null)
			{
				SDKClient.Instance.AddEvent(new TokenSendEvent(token));
			}
			OnPushTokenReceived(token);
		}

		public void onFailedToRegisteredForPushNotifications(string errorString)
		{
			OnPushTokenFailed(errorString);
		}

		public void onPushNotificationsReceived(Event metric, string messageString)
		{
			if (metric != null)
			{
				SDKClient.Instance.AddEvent(metric);
			}
			if (messageString != null)
			{
				OnPushReceived(PushType.ToastNotification, DecodePushData(messageString));
			}
		}

		public PushClient()
		{
			pushClientPlatform = new PushClientPlatform();
		}

		public void Initialize()
		{
			pushClientPlatform.Initialize(delegate(string token)
			{
				onRegisteredForPushNotifications(token);
			}, delegate(string error)
			{
				onFailedToRegisteredForPushNotifications(error);
			}, delegate
			{
				GetEvents();
			});
		}

		private string GetNativeEvents()
		{
			return pushClientPlatform.GetNativeEvents();
		}

		private void ClearNativeEvents()
		{
			pushClientPlatform.ClearNativeEvents();
		}

		public List<Event> GetEvents()
		{
			List<Event> result = new List<Event>();
			string nativeEvents = GetNativeEvents();
			if (nativeEvents == null)
			{
				return result;
			}
			Log.D("Native push data: " + nativeEvents);
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(nativeEvents);
			}
			catch (Exception ex)
			{
				Log.E(ex.StackTrace);
				return result;
			}
			if (!(jSONNode is JSONClass))
			{
				return result;
			}
			if (jSONNode[EventConst.EventsInfo[EventType.PushOpen].Value] != null)
			{
				JSONArray asArray = jSONNode[EventConst.EventsInfo[EventType.PushOpen].Value].AsArray;
				foreach (JSONNode item in asArray)
				{
					try
					{
						PushOpenEvent metric = PushOpenEvent.CreateFromJSON(item);
						if (item[PUSH_NATIVE_DATA] != null)
						{
							onPushNotificationsReceived(metric, item[PUSH_NATIVE_DATA].Value);
						}
						else
						{
							onPushNotificationsReceived(metric, null);
						}
					}
					catch
					{
					}
				}
			}
			if (jSONNode[EventConst.EventsInfo[EventType.PushReceived].Value] != null)
			{
				JSONArray asArray2 = jSONNode[EventConst.EventsInfo[EventType.PushReceived].Value].AsArray;
				foreach (JSONNode item2 in asArray2)
				{
					try
					{
						Log.D(item2.Value);
						PushReceivedEvent metric2 = PushReceivedEvent.CreateFromJSON(item2);
						if (item2[PUSH_NATIVE_DATA] != null)
						{
							onPushNotificationsReceived(metric2, item2[PUSH_NATIVE_DATA].Value);
						}
						else
						{
							onPushNotificationsReceived(metric2, null);
						}
					}
					catch
					{
					}
				}
			}
			ClearNativeEvents();
			return result;
		}

		private Dictionary<string, string> DecodePushData(string pushMessage)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (pushMessage == null)
			{
				return dictionary;
			}
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(pushMessage);
			}
			catch (Exception ex)
			{
				Log.E(ex.StackTrace);
				return dictionary;
			}
			foreach (KeyValuePair<string, JSONNode> item in jSONNode as JSONClass)
			{
				dictionary.Add(item.Key, item.Value.Value);
			}
			return dictionary;
		}

		public void SetCustomSmallIcon(string pathToResource)
		{
			pushClientPlatform.SetCustomSmallIcon(pathToResource);
		}

		public void SetCustomLargeIcon(string pathToResource)
		{
			pushClientPlatform.SetCustomLargeIcon(pathToResource);
		}
	}
}
