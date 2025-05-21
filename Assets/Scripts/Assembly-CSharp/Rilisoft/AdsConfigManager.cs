using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AdsConfigManager
	{
		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass7_0
		{
			public Task futureToWait;

			internal bool _003CGetAdvertInfoLoop_003Eb__0()
			{
				return futureToWait.IsCompleted;
			}
		}

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass7_1
		{
			public float advertGetInfoStartTime;

			internal bool _003CGetAdvertInfoLoop_003Eb__1()
			{
				return Time.realtimeSinceStartup - advertGetInfoStartTime < 960f;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CGetAdvertInfoLoop_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Task futureToWait;

			public AdsConfigManager _003C_003E4__this;

			private _003C_003Ec__DisplayClass7_1 _003C_003E8__1;

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
			public _003CGetAdvertInfoLoop_003Ed__7(int _003C_003E1__state)
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
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass7_0
					{
						futureToWait = futureToWait
					};
					if (CS_0024_003C_003E8__locals0.futureToWait != null)
					{
						_003C_003E2__current = new WaitUntil(() => CS_0024_003C_003E8__locals0.futureToWait.IsCompleted);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0088;
				}
				case 1:
					_003C_003E1__state = -1;
					goto IL_0088;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0088;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00b0;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new WaitWhile(() => Time.realtimeSinceStartup - _003C_003E8__1.advertGetInfoStartTime < 960f);
					_003C_003E1__state = 5;
					return true;
				case 5:
					{
						_003C_003E1__state = -1;
						_003C_003E8__1 = null;
						goto IL_00b7;
					}
					IL_00b0:
					if (!FriendsController.isReadABTestAdvertConfig)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00b7;
					IL_0088:
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b0;
					IL_00b7:
					_003C_003E8__1 = new _003C_003Ec__DisplayClass7_1();
					_003C_003E8__1.advertGetInfoStartTime = Time.realtimeSinceStartup;
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(_003C_003E4__this.GetAdvertInfoOnce());
					_003C_003E1__state = 4;
					return true;
				}
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
		internal sealed class _003CGetAdvertInfoOnce_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private WWW _003Cresponse_003E5__1;

			public AdsConfigManager _003C_003E4__this;

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
			public _003CGetAdvertInfoOnce_003Ed__9(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				string text;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					string newAdvertisingConfig = URLs.NewAdvertisingConfig;
					if (!string.IsNullOrEmpty(configFromABTestAdvert))
					{
						text = configFromABTestAdvert;
						break;
					}
					string value = PersistentCacheManager.Instance.GetValue(newAdvertisingConfig);
					if (!string.IsNullOrEmpty(value))
					{
						PersistentCacheManager.DebugReportCacheHit(newAdvertisingConfig);
						text = value;
						break;
					}
					PersistentCacheManager.DebugReportCacheMiss(newAdvertisingConfig);
					_003Cresponse_003E5__1 = Tools.CreateWww(newAdvertisingConfig);
					_003C_003E2__current = _003Cresponse_003E5__1;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					try
					{
						if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
						{
							UnityEngine.Debug.LogWarningFormat("Advert response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
							return false;
						}
						text = URLs.Sanitize(_003Cresponse_003E5__1);
						if (string.IsNullOrEmpty(text))
						{
							UnityEngine.Debug.LogWarning("Advert response is empty");
							return false;
						}
						PersistentCacheManager.Instance.SetValue(_003Cresponse_003E5__1.url, text);
					}
					finally
					{
						if (Application.isEditor)
						{
							UnityEngine.Debug.Log("<color=teal>AdsConfigManager.GetAdvertInfoOnce(): response.Dispose()</color>");
						}
						_003Cresponse_003E5__1.Dispose();
					}
					_003Cresponse_003E5__1 = null;
					break;
				}
				_003C_003E4__this._lastLoadedConfig = AdsConfigMemento.FromJson(text);
				EventHandler configLoaded = _003C_003E4__this.ConfigLoaded;
				if (configLoaded != null)
				{
					configLoaded(_003C_003E4__this, EventArgs.Empty);
				}
				if (_003C_003E4__this._lastLoadedConfig.Exception != null)
				{
					UnityEngine.Debug.LogWarning(_003C_003E4__this._lastLoadedConfig.Exception);
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

		public static string configFromABTestAdvert = string.Empty;

		private const float AdvertInfoTimeout = 960f;

		private static readonly Lazy<AdsConfigManager> s_instance = new Lazy<AdsConfigManager>(() => new AdsConfigManager());

		private AdsConfigMemento _lastLoadedConfig;

		public static AdsConfigManager Instance
		{
			get
			{
				return s_instance.Value;
			}
		}

		public AdsConfigMemento LastLoadedConfig
		{
			get
			{
				return _lastLoadedConfig;
			}
		}

		public event EventHandler ConfigLoaded;

		internal IEnumerator GetAdvertInfoLoop(Task futureToWait)
		{
			if (futureToWait != null)
			{
				yield return new WaitUntil(() => futureToWait.IsCompleted);
			}
			while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
			{
				yield return null;
			}
			while (!FriendsController.isReadABTestAdvertConfig)
			{
				yield return null;
			}
			while (true)
			{
				float advertGetInfoStartTime = Time.realtimeSinceStartup;
				yield return CoroutineRunner.Instance.StartCoroutine(GetAdvertInfoOnce());
				yield return new WaitWhile(() => Time.realtimeSinceStartup - advertGetInfoStartTime < 960f);
			}
		}

		private IEnumerator GetAdvertInfoOnce()
		{
			string newAdvertisingConfig = URLs.NewAdvertisingConfig;
			string text;
			if (!string.IsNullOrEmpty(configFromABTestAdvert))
			{
				text = configFromABTestAdvert;
			}
			else
			{
				string value = PersistentCacheManager.Instance.GetValue(newAdvertisingConfig);
				if (!string.IsNullOrEmpty(value))
				{
					PersistentCacheManager.DebugReportCacheHit(newAdvertisingConfig);
					text = value;
				}
				else
				{
					PersistentCacheManager.DebugReportCacheMiss(newAdvertisingConfig);
					WWW response = Tools.CreateWww(newAdvertisingConfig);
					yield return response;
					try
					{
						if (response == null || !string.IsNullOrEmpty(response.error))
						{
							UnityEngine.Debug.LogWarningFormat("Advert response error: {0}", (response != null) ? response.error : "null");
							yield break;
						}
						text = URLs.Sanitize(response);
						if (string.IsNullOrEmpty(text))
						{
							UnityEngine.Debug.LogWarning("Advert response is empty");
							yield break;
						}
						PersistentCacheManager.Instance.SetValue(response.url, text);
					}
					finally
					{
						if (Application.isEditor)
						{
							UnityEngine.Debug.Log("<color=teal>AdsConfigManager.GetAdvertInfoOnce(): response.Dispose()</color>");
						}
						response.Dispose();
					}
				}
			}
			_lastLoadedConfig = AdsConfigMemento.FromJson(text);
			EventHandler configLoaded = this.ConfigLoaded;
			if (configLoaded != null)
			{
				configLoaded(this, EventArgs.Empty);
			}
			if (_lastLoadedConfig.Exception != null)
			{
				UnityEngine.Debug.LogWarning(_lastLoadedConfig.Exception);
			}
		}

		internal static CheatingMethods GetCheatingMethods(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return CheatingMethods.None;
			}
			if (config.CheaterConfig.CheckSignatureTampering && (Switcher.AbuseMethod & AbuseMetod.AndroidPackageSignature) != 0)
			{
				return CheatingMethods.SignatureTampering;
			}
			if (Storager.getInt("Coins") >= config.CheaterConfig.CoinThreshold)
			{
				return CheatingMethods.CoinThreshold;
			}
			if (Storager.getInt("GemsCurrency") >= config.CheaterConfig.GemThreshold)
			{
				return CheatingMethods.GemThreshold;
			}
			return CheatingMethods.None;
		}

		internal static string GetPlayerCategory(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return string.Empty;
			}
			if (GetCheatingMethods(config) != 0)
			{
				return "cheater";
			}
			bool flag = StoreKitEventListener.IsPayingUser();
			int @int = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1);
			int num = Mathf.FloorToInt(((NotificationController.instance != null) ? NotificationController.instance.currentPlayTime : Time.realtimeSinceStartup) / 60f);
			foreach (KeyValuePair<string, PlayerStateMemento> playerState in config.PlayerStates)
			{
				PlayerStateMemento value = playerState.Value;
				if (value.IsPaying != flag)
				{
					continue;
				}
				if (value.MinInGameMinutes.HasValue && value.MaxInGameMinutes.HasValue)
				{
					if (num < value.MinInGameMinutes.Value || num > value.MaxInGameMinutes.Value)
					{
						continue;
					}
				}
				else if (@int < value.MinDay || @int > value.MaxDay)
				{
					continue;
				}
				return value.Id;
			}
			return string.Empty;
		}

		internal static int GetInterstitialDisabledReasonCode(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return 10;
			}
			if (config.InterstitialConfig == null)
			{
				return 20;
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return 40;
			}
			double timeSpanSinceLastShowInMinutes = GetTimeSpanSinceLastShowInMinutes(serverTime.Value);
			string playerCategory = GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			int disabledReasonCode = config.InterstitialConfig.GetDisabledReasonCode(playerCategory, deviceModel, timeSpanSinceLastShowInMinutes);
			if (disabledReasonCode != 0)
			{
				return 30 + disabledReasonCode;
			}
			return 0;
		}

		internal static string GetInterstitialDisabledReason(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return config.Exception.Message;
			}
			if (config.InterstitialConfig == null)
			{
				return "`InterstitialConfig == null` (probably not received yet)";
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return "Server time not received";
			}
			string playerCategory = GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			double timeSpanSinceLastShowInMinutes = GetTimeSpanSinceLastShowInMinutes(serverTime.Value);
			return config.InterstitialConfig.GetDisabledReason(playerCategory, deviceModel, timeSpanSinceLastShowInMinutes);
		}

		internal static string GetVideoDisabledReason(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return config.Exception.Message;
			}
			string playerCategory = GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			return config.VideoConfig.GetDisabledReason(playerCategory, deviceModel);
		}

		internal static double GetTimeSpanSinceLastShowInMinutes(DateTime now)
		{
			string @string = PlayerPrefs.GetString(Defs.LastTimeShowBanerKey, string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				return 3.4028234663852886E+38;
			}
			DateTime result;
			if (!DateTime.TryParse(@string, out result))
			{
				return 3.4028234663852886E+38;
			}
			double totalMinutes = now.Subtract(result).TotalMinutes;
			if (totalMinutes < 0.0)
			{
				PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, now.ToString("s"));
				PlayerPrefs.Save();
				return 0.0;
			}
			return totalMinutes;
		}

		private AdsConfigManager()
		{
		}
	}
}
