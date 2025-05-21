using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class NotificationController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CappStart_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CappStart_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
			timeStartApp = Time.time;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pauseStatus;

		public NotificationController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003COnApplicationPause_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (pauseStatus)
				{
					if (Initializer.Instance != null)
					{
						_003C_003E4__this.SaveTimeValues();
					}
					_003C_003E4__this.appStop();
					if (PhotonNetwork.connected && TimeGameController.sharedController == null && (Application.platform == RuntimePlatform.Android || !PhotonNetwork.isMessageQueueRunning || ConnectScene.sharedController != null))
					{
						GameConnect.Disconnect();
					}
					break;
				}
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.appStart());
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				Tools.AddSessionNumber();
				CoroutineRunner.Instance.StartCoroutine(AnalyticsStuff.WaitInitializationThenLogGameDayCountCoroutine());
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static bool isGetEveryDayMoney;

	public static float timeStartApp;

	public bool pauserTemp;

	private float playTime;

	private float playTimeInMatch;

	public float savedPlayTime;

	public float savedPlayTimeInMatch;

	public static NotificationController instance;

	private static bool _paused;

	private readonly List<int> _notificationIds = new List<int>();

	private const string ScheduledNotificationsKey = "Scheduled Notifications";

	public float currentPlayTimeMatch
	{
		get
		{
			return savedPlayTimeInMatch + playTimeInMatch;
		}
	}

	public float currentPlayTime
	{
		get
		{
			return savedPlayTime + playTime;
		}
	}

	internal static bool Paused
	{
		get
		{
			return _paused;
		}
	}

	private void Start()
	{
		using (new ScopeLogger("NotificationController.Start()", false))
		{
			base.gameObject.AddComponent<LocalNotificationController>();
			if (!Load.LoadBool("bilZapuskKey"))
			{
				Save.SaveBool("bilZapuskKey", true);
			}
			else
			{
				StartCoroutine(appStart());
			}
			instance = this;
			float result;
			if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime"), out result))
			{
				savedPlayTime = result;
			}
			float result2;
			if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch"), out result2))
			{
				savedPlayTimeInMatch = result2;
			}
		}
	}

	private void Update()
	{
		if (pauserTemp)
		{
			pauserTemp = false;
			_paused = true;
			GameConnect.Disconnect();
		}
		if (!FriendsController.sharedController.idle)
		{
			playTime += Time.deltaTime;
			if (Initializer.Instance != null && (PhotonNetwork.room == null || PhotonNetwork.room.customProperties[GameConnect.passwordProperty].Equals("")) && !GameConnect.isDaterRegim && !GameConnect.isHunger && !GameConnect.isCOOP && !NetworkStartTable.LocalOrPasswordRoom())
			{
				playTimeInMatch += Time.deltaTime;
			}
		}
		if (MemoryCheatHook.CheckForCheating())
		{
			CheatDetectedBanner.ShowAndClearProgress();
		}
	}

	public void SaveTimeValues()
	{
		InGameTimeKeeper.Instance.Save();
		if (playTime > 0f)
		{
			savedPlayTime += playTime;
			UnityEngine.Debug.Log(string.Format("PlayTime saved: {0} (+{1})", new object[2] { savedPlayTime, playTime }));
			playTime = 0f;
			Storager.setString("PlayTime", savedPlayTime.ToString());
			try
			{
				TechnicalCloudInfo technicalCloudInfo = JsonUtility.FromJson<TechnicalCloudInfo>(Storager.getString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY")) ?? new TechnicalCloudInfo();
				float num = Math.Max(technicalCloudInfo.InGameSeconds, savedPlayTime);
				if (technicalCloudInfo.InGameSeconds != num)
				{
					technicalCloudInfo.InGameSeconds = num;
					string val = JsonUtility.ToJson(technicalCloudInfo);
					Storager.setString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY", val);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		if (playTimeInMatch > 0f)
		{
			savedPlayTimeInMatch += playTimeInMatch;
			UnityEngine.Debug.Log(string.Format("PlayTimeInMatch saved: {0} (+{1})", new object[2] { savedPlayTimeInMatch, playTimeInMatch }));
			playTimeInMatch = 0f;
			Storager.setString("PlayTimeInMatch", savedPlayTimeInMatch.ToString());
		}
	}

	internal static void ResetPaused()
	{
		_paused = false;
	}

	private void appStop()
	{
		bool interfaceEnabled;
		int num4;
		if (BankController.Instance != null)
		{
			interfaceEnabled = BankController.Instance.InterfaceEnabled;
		}
		else
			num4 = 0;
		if (PhotonNetwork.connected)
		{
			_paused = true;
		}
		int hour = DateTime.Now.Hour;
		int num = 82800;
		hour += 23;
		if (hour > 24)
		{
			hour -= 24;
		}
		int num2 = ((hour > 16) ? (24 - hour + 16) : (16 - hour));
		num += num2 * 3600;
		DateTime dateTime = DateTime.Now + TimeSpan.FromHours(23.0);
		if (dateTime.Hour >= 16)
		{
			dateTime.Date.AddHours(40.0);
		}
		else
		{
			dateTime.Date.AddHours(16.0);
		}
		TimeSpan.FromDays(1.0);
		int num3 = 0;
		for (int i = 0; i < num3; i++)
		{
			int num5 = num + i * 86400;
			UnityEngine.Random.Range(0, 3600);
		}
	}

	private IEnumerator appStart()
	{
		timeStartApp = Time.time;
		yield break;
	}

	private IEnumerator OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			if (Initializer.Instance != null)
			{
				SaveTimeValues();
			}
			appStop();
			if (PhotonNetwork.connected && TimeGameController.sharedController == null && (Application.platform == RuntimePlatform.Android || !PhotonNetwork.isMessageQueueRunning || ConnectScene.sharedController != null))
			{
				GameConnect.Disconnect();
			}
		}
		else
		{
			StartCoroutine(appStart());
			yield return null;
			yield return null;
			Tools.AddSessionNumber();
			CoroutineRunner.Instance.StartCoroutine(AnalyticsStuff.WaitInitializationThenLogGameDayCountCoroutine());
		}
	}
}
