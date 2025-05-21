using System;
using System.Collections.Generic;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev
{
	internal class AsyncOperationDispatcher : MonoBehaviour
	{
		private static readonly string GAMEOBJECT_NAME = "[devtodev_AsyncOperationDispatcher]";

		private GameObject handeledGameObject;

		private ConcurrentQueue<Action> executeOnMainThread;

		private static AsyncOperationDispatcher instance;

		private void OnApplicationFocus(bool focus)
		{
			if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (focus)
				{
					SDKClient.Instance.Resume();
				}
				else
				{
					SDKClient.Instance.Suspend(0L);
				}
			}
			else if (!focus)
			{
				if (SDKClient.Instance.IsSessionActive())
				{
					SDKClient.Instance.Suspend(0L);
					SDKClient.Instance.Resume();
				}
				else
				{
					SDKClient.Instance.SaveAll();
				}
			}
		}

		internal static AsyncOperationDispatcher Create()
		{
			if ((object)instance != null)
			{
				return instance;
			}
			try
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(AsyncOperationDispatcher)) as AsyncOperationDispatcher;
			}
			catch
			{
			}
			if ((object)instance != null)
			{
				return instance;
			}
			Log.D(GAMEOBJECT_NAME + " was destroyed, creating the new one!");
			GameObject gameObject = new GameObject(GAMEOBJECT_NAME);
			gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
			instance = gameObject.AddComponent<AsyncOperationDispatcher>();
			instance.handeledGameObject = gameObject;
			return instance;
		}

		public void Dispose()
		{
			if (handeledGameObject != null)
			{
				UnityEngine.Object.Destroy(handeledGameObject);
			}
		}

		public AsyncOperationDispatcher()
		{
			executeOnMainThread = new ConcurrentQueue<Action>();
			InitializationPlatform.StartSessionTracker(GAMEOBJECT_NAME);
		}

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void Update()
		{
			try
			{
				if (SDKClient.Instance.MetricsController != null)
				{
					SDKClient.Instance.MetricsController.WebTimerTick();
				}
			}
			catch (Exception ex)
			{
				Log.E("Message : " + ex.Message + " trace: " + ex.StackTrace);
			}
			Action action = null;
			while ((action = executeOnMainThread.Dequeue()) != null)
			{
				try
				{
					action();
				}
				catch (Exception ex2)
				{
					Log.E("Message : " + ex2.Message + " trace: " + ex2.StackTrace);
				}
			}
		}

		public void DispatchOnMainThread(Action action)
		{
			try
			{
				executeOnMainThread.Enqueue(action);
			}
			catch (Exception)
			{
			}
		}

		public void StartSession(string message)
		{
			Log.D("Session counter - got start session: " + message);
			SDKClient.Instance.Resume();
		}

		public void removeLostSession()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.DTDApplicationLifecycle");
				androidJavaClass.CallStatic("removeLostSession");
			}
		}

		public void EndSession(string message)
		{
			Log.D("Session counter - got stop session: " + message);
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				long result = 0L;
				if (message != null && long.TryParse(message, out result))
				{
					SDKClient.Instance.Suspend(result);
				}
				else
				{
					SDKClient.Instance.Suspend(0L);
				}
				removeLostSession();
			}
			else
			{
				SDKClient.Instance.Suspend(0L);
			}
		}

		private void OnDestroy()
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				SDKClient.Instance.Suspend(0L);
			}
			Log.D("On Destroy was called.");
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				{
					SDKClient.Instance.Suspend(0L);
				}
				Log.D("On Application paused");
			}
		}

		private void OnApplicationQuit()
		{
			Log.D("On Application Quit was called.");
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				SDKClient.Instance.Suspend(0L);
			}
			else if (SDKClient.Instance.IsSessionActive())
			{
				SDKClient.Instance.Suspend(0L);
			}
			else
			{
				SDKClient.Instance.SaveAll();
			}
		}

		public void onRegisteredForPushNotifications(string token)
		{
			Log.D("Unity onRegisteredForPushNotifications: " + token);
			if (PushManager.PushClient != null)
			{
				PushManager.PushClient.onRegisteredForPushNotifications(token);
			}
		}

		public void onFailedToRegisteredForPushNotifications(string errorString)
		{
			Log.D("Unity onFailedToRegisteredForPushNotifications: " + errorString);
			if (PushManager.PushClient != null)
			{
				PushManager.PushClient.onFailedToRegisteredForPushNotifications(errorString);
			}
		}

		public void onPushNotificationsReceived(string messageString)
		{
			Log.D("Unity onPushNotificationsReceived: " + messageString);
			if (PushManager.PushClient != null)
			{
				List<DevToDev.Data.Metrics.Event> events = null;
				if (PushManager.PushClient != null)
				{
					events = PushManager.PushClient.GetEvents();
				}
				SDKClient.Instance.UsersStorage.ActiveUser.StartFastSendSession();
				SDKClient.Instance.UsersStorage.AddEvents(events);
				SDKClient.Instance.UsersStorage.ActiveUser.StopFastSendSession();
				if (UnityPlayerPlatform.isUnityWSAPlatform() || Application.platform == RuntimePlatform.Android)
				{
					PushManager.PushClient.onPushNotificationsReceived(null, messageString);
				}
			}
		}
	}
}
