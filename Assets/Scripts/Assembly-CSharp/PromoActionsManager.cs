using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class PromoActionsManager : MonoBehaviour
{
	public sealed class AdvertInfo
	{
		public bool enabled;

		public string imageUrl;

		public string adUrl;

		public string message;

		public bool showAlways;

		public bool btnClose;

		public int minLevel;

		public int maxLevel;
	}

	public class MobileAdvertInfo
	{
		private List<string> _admobImageAdUnitIds = new List<string>();

		private List<string> _admobVideoAdUnitIds = new List<string>();

		private List<int> _adProviders = new List<int>();

		private double _daysOfBeingPayingUser = double.MaxValue;

		private List<int> _interstitialProviders = new List<int>();

		private List<List<string>> _admobImageIdGroups = new List<List<string>>();

		private List<List<string>> _admobVideoIdGroups = new List<List<string>>();

		[Obsolete]
		public bool ImageEnabled { get; set; }

		public bool VideoEnabled { get; set; }

		public bool VideoShowPaying { get; set; }

		public bool VideoShowNonpaying { get; set; }

		public int TimeoutBetweenShowInterstitial { get; set; }

		public int CountSessionNewPlayer { get; set; }

		public int CountRoundReplaceProviders { get; set; }

		public int TimeoutSkipVideoPaying { get; set; }

		public int TimeoutSkipVideoNonpaying { get; set; }

		public double ConnectSceneDelaySeconds { get; set; }

		public double DaysOfBeingPayingUser
		{
			get
			{
				return _daysOfBeingPayingUser;
			}
			set
			{
				_daysOfBeingPayingUser = Math.Max(0.0, value);
			}
		}

		public string AdmobVideoAdUnitId { get; set; }

		public List<string> AdmobImageAdUnitIds
		{
			get
			{
				return _admobImageAdUnitIds;
			}
			set
			{
				_admobImageAdUnitIds = value ?? new List<string>();
			}
		}

		public List<string> AdmobVideoAdUnitIds
		{
			get
			{
				return _admobVideoAdUnitIds;
			}
			set
			{
				_admobVideoAdUnitIds = value ?? new List<string>();
			}
		}

		public List<List<string>> AdmobImageIdGroups
		{
			get
			{
				return _admobImageIdGroups;
			}
			set
			{
				_admobImageIdGroups = value ?? new List<List<string>>();
			}
		}

		public List<List<string>> AdmobVideoIdGroups
		{
			get
			{
				return _admobVideoIdGroups;
			}
			set
			{
				_admobVideoIdGroups = value ?? new List<List<string>>();
			}
		}

		public int AdProvider { get; set; }

		public List<int> AdProviders
		{
			get
			{
				return _adProviders;
			}
			set
			{
				_adProviders = value ?? new List<int>();
			}
		}

		public List<int> InterstitialProviders
		{
			get
			{
				return _interstitialProviders;
			}
			set
			{
				_interstitialProviders = value ?? new List<int>();
			}
		}

		public float TimeoutWaitingInterstitialAfterMatchSeconds { get; set; }

		public double MinMatchDurationMinutes { get; set; }
	}

	internal sealed class ReplaceAdmobPerelivInfo
	{
		public bool enabled;

		public readonly List<string> imageUrls = new List<string>();

		public readonly List<string> adUrls = new List<string>();

		public int ShowEveryTimes { get; set; }

		public int ShowTimesTotal { get; set; }

		public bool ShowToPaying { get; set; }

		public bool ShowToNew { get; set; }

		public int MinLevel { get; set; }

		public int MaxLevel { get; set; }
	}

	internal sealed class AmazonEventInfo
	{
		public DateTime StartTime { get; set; }

		public DateTime EndTime
		{
			get
			{
				return StartTime + TimeSpan.FromSeconds(DurationSeconds);
			}
		}

		public float DurationSeconds { get; set; }

		public List<int> Timezones { get; set; }

		public float Percentage { get; set; }

		public string Caption { get; set; }

		public AmazonEventInfo()
		{
			StartTime = DateTime.MaxValue;
			Timezones = new List<int>();
			Caption = string.Empty;
		}
	}

	public delegate void OnDayOfValorEnableDelegate(bool enable);

	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__73 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pause;

		public PromoActionsManager _003C_003E4__this;

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
		public _003COnApplicationPause_003Ed__73(int _003C_003E1__state)
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
				if (!pause)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.SaveUnlockedItems();
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.GetActions());
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.GetEventX3Info());
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.GetAmazonEventCoroutine());
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.GetAdvertInfo());
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadBestBuyInfo());
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDayOfValorInfo());
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass74_0
	{
		public Task futureToWait;

		internal bool _003CGetActionsLoop_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetActionsLoop_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task futureToWait;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetActionsLoop_003Ed__74(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass74_0
				{
					futureToWait = futureToWait
				}._003CGetActionsLoop_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0078;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0078;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_00b7;
				}
				IL_0078:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0087;
				IL_0087:
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.GetActions());
				goto IL_00b7;
				IL_00b7:
				if (Time.realtimeSinceStartup - _003C_003E4__this.startTime < 900f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0087;
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
	internal sealed class _003CGetEventX3InfoLoop_003Ed__75 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetEventX3InfoLoop_003Ed__75(int _003C_003E1__state)
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
				_003C_003E4__this.UpdateNewbieEventX3Info();
				goto IL_0031;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.GetAmazonEventCoroutine());
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a2;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_00a2;
				}
				IL_00a2:
				if (Time.realtimeSinceStartup - _003C_003E4__this._eventX3GetInfoStartTime < 930f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0031;
				IL_0031:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.GetEventX3Info());
				_003C_003E1__state = 1;
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
	internal sealed class _003C_003Ec__DisplayClass76_0
	{
		public Task futureToWait;

		internal bool _003CGetAdvertInfoLoop_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetAdvertInfoLoop_003Ed__76 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task futureToWait;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetAdvertInfoLoop_003Ed__76(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass76_0
				{
					futureToWait = futureToWait
				}._003CGetAdvertInfoLoop_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_007c;
			case 2:
				_003C_003E1__state = -1;
				goto IL_007c;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00d0;
			case 4:
				{
					_003C_003E1__state = -1;
					goto IL_00d0;
				}
				IL_00d0:
				if (Time.realtimeSinceStartup - _003C_003E4__this._advertGetInfoStartTime < 960f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_008b;
				IL_008b:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.GetAdvertInfo());
				_003C_003E1__state = 3;
				return true;
				IL_007c:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_008b;
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
	internal sealed class _003CGetAmazonEventCoroutine_003Ed__78 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private WWW _003Cresponse_003E5__1;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetAmazonEventCoroutine_003Ed__78(int _003C_003E1__state)
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
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
				{
					return false;
				}
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
				{
					return false;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.AmazonEvent);
				if (_003Cresponse_003E5__1 == null)
				{
					return false;
				}
				_003C_003E2__current = _003Cresponse_003E5__1;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				string text = URLs.Sanitize(_003Cresponse_003E5__1);
				if (!string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
				{
					UnityEngine.Debug.LogWarning("Amazon event response error: " + _003Cresponse_003E5__1.error);
					return false;
				}
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("Amazon event response is empty");
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				if (dictionary == null)
				{
					UnityEngine.Debug.LogWarning("Amazon event bad response: " + text);
					return false;
				}
				if (_003C_003E4__this.IsNeedCheckAmazonEventX3())
				{
					_003C_003E4__this._amazonEventInfo = new AmazonEventInfo();
					object value;
					if (dictionary.TryGetValue("startTime", out value))
					{
						try
						{
							_003C_003E4__this._amazonEventInfo.StartTime = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogException(exception);
						}
					}
					object value2;
					if (dictionary.TryGetValue("durationSeconds", out value2))
					{
						try
						{
							_003C_003E4__this._amazonEventInfo.DurationSeconds = Convert.ToSingle(value2);
						}
						catch (Exception exception2)
						{
							UnityEngine.Debug.LogException(exception2);
						}
					}
					object value3;
					if (dictionary.TryGetValue("timezones", out value3))
					{
						List<object> source = (value3 as List<object>) ?? new List<object>();
						try
						{
							_003C_003E4__this._amazonEventInfo.Timezones = source.Select(Convert.ToInt32).ToList();
						}
						catch (Exception exception3)
						{
							UnityEngine.Debug.LogException(exception3);
						}
					}
					object value4;
					if (dictionary.TryGetValue("percentage", out value4))
					{
						try
						{
							_003C_003E4__this._amazonEventInfo.Percentage = Convert.ToSingle(value4);
						}
						catch (Exception exception4)
						{
							UnityEngine.Debug.LogException(exception4);
						}
					}
					object value5;
					if (dictionary.TryGetValue("caption", out value5))
					{
						try
						{
							_003C_003E4__this._amazonEventInfo.Caption = Convert.ToString(value5, CultureInfo.InvariantCulture) ?? string.Empty;
						}
						catch (Exception exception5)
						{
							UnityEngine.Debug.LogException(exception5);
						}
					}
					_003C_003E4__this.RefreshAmazonEvent();
				}
				return false;
			}
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
	internal sealed class _003CGetEventX3Info_003Ed__84 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

		private WWW _003Cresponse_003E5__1;

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
		public _003CGetEventX3Info_003Ed__84(int _003C_003E1__state)
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
				if (_003C_003E4__this._isGetEventX3InfoRunning)
				{
					return false;
				}
				_003C_003E4__this._eventX3GetInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetEventX3InfoRunning = true;
				if (string.IsNullOrEmpty(URLs.EventX3))
				{
					_003C_003E4__this._isGetEventX3InfoRunning = false;
					return false;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.EventX3);
				_003C_003E2__current = _003Cresponse_003E5__1;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				string text = URLs.Sanitize(_003Cresponse_003E5__1);
				if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.LogWarningFormat("EventX3 response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
					}
					_003C_003E4__this._isGetEventX3InfoRunning = false;
					_003C_003E4__this._eventX3GetInfoStartTime = Time.realtimeSinceStartup - 930f + 15f;
					return false;
				}
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("EventX3 response is empty");
					_003C_003E4__this._isGetEventX3InfoRunning = false;
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				if (dictionary == null || !dictionary.ContainsKey("start") || !dictionary.ContainsKey("duration"))
				{
					UnityEngine.Debug.LogWarning("EventX3 response is bad");
					_003C_003E4__this._isGetEventX3InfoRunning = false;
					return false;
				}
				long eventX3StartTime = (long)dictionary["start"];
				long eventX3Duration = (long)dictionary["duration"];
				_003C_003E4__this._eventX3StartTime = eventX3StartTime;
				_003C_003E4__this._eventX3Duration = eventX3Duration;
				_003C_003E4__this.CheckEventX3Active();
				x3InfoDownloadaedOnceDuringCurrentRun = true;
				_003C_003E4__this._isGetEventX3InfoRunning = false;
				return false;
			}
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
	internal sealed class _003CGetAdvertInfo_003Ed__104 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

		private WWW _003Cresponse_003E5__1;

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
		public _003CGetAdvertInfo_003Ed__104(int _003C_003E1__state)
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
				if (_003C_003E4__this._isGetAdvertInfoRunning)
				{
					return false;
				}
				_003C_003E4__this._advertGetInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetAdvertInfoRunning = true;
				_paidAdvert.enabled = false;
				_freeAdvert.enabled = false;
				string advert = URLs.Advert;
				if (string.IsNullOrEmpty(advert))
				{
					_003C_003E4__this._isGetAdvertInfoRunning = false;
					return false;
				}
				string value = PersistentCacheManager.Instance.GetValue(advert);
				if (!string.IsNullOrEmpty(value))
				{
					text = value;
					break;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(advert);
				_003C_003E2__current = _003Cresponse_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
				{
					UnityEngine.Debug.LogWarningFormat("Advert response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
					_003C_003E4__this._isGetAdvertInfoRunning = false;
					return false;
				}
				text = URLs.Sanitize(_003Cresponse_003E5__1);
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("Advert response is empty");
					_003C_003E4__this._isGetAdvertInfoRunning = false;
					return false;
				}
				PersistentCacheManager.Instance.SetValue(_003Cresponse_003E5__1.url, text);
				_003Cresponse_003E5__1 = null;
				break;
			}
			object obj = Json.Deserialize(text);
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (obj == null)
			{
				_003C_003E4__this._isGetAdvertInfoRunning = false;
				return false;
			}
			if (dictionary.ContainsKey("paid"))
			{
				_003C_003E4__this.ParseAdvertInfo(dictionary["paid"], _paidAdvert);
			}
			if (dictionary.ContainsKey("free"))
			{
				_003C_003E4__this.ParseAdvertInfo(dictionary["free"], _freeAdvert);
			}
			if (dictionary.ContainsKey("replace_admob_pereliv_10_2_0"))
			{
				ParseReplaceAdmobPereliv(dictionary["replace_admob_pereliv_10_2_0"] as Dictionary<string, object>, _replaceAdmobPereliv);
			}
			else
			{
				UnityEngine.Debug.Log("Advert response doesn't contain “replace_admob_pereliv_10_2_0” property.");
			}
			_003C_003E4__this._isGetAdvertInfoRunning = false;
			MobileAdvertIsReady = true;
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
	internal sealed class _003CGetActions_003Ed__112 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

		private WWW _003Cdownload_003E5__1;

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
		public _003CGetActions_003Ed__112(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string text2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.startTime = Time.realtimeSinceStartup;
				string value = PersistentCacheManager.Instance.GetValue(_003C_003E4__this.promoActionAddress);
				if (!string.IsNullOrEmpty(value))
				{
					text2 = value;
					break;
				}
				_003Cdownload_003E5__1 = Tools.CreateWwwIfNotConnected(_003C_003E4__this.promoActionAddress);
				if (_003Cdownload_003E5__1 == null)
				{
					return false;
				}
				_003C_003E2__current = _003Cdownload_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				string text = URLs.Sanitize(_003Cdownload_003E5__1);
				if (string.IsNullOrEmpty(_003Cdownload_003E5__1.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log("GetActions response:    " + text);
				}
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__1.error))
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogWarning("GetActions error:    " + _003Cdownload_003E5__1.error);
					}
					_003C_003E4__this.ClearAll();
					ActionsAvailable = false;
					return false;
				}
				text2 = text;
				PersistentCacheManager.Instance.SetValue(_003Cdownload_003E5__1.url, text2);
				_003Cdownload_003E5__1 = null;
				break;
			}
			}
			if (_003C_003E4__this._previousResponseText != null && text2 != null && text2 == _003C_003E4__this._previousResponseText)
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log("GetActions:    responseText == _previousResponseText");
				}
				return false;
			}
			_003C_003E4__this._previousResponseText = text2;
			ActionsAvailable = true;
			_003C_003E4__this.ClearAll();
			Dictionary<string, object> dictionary = Json.Deserialize(text2) as Dictionary<string, object>;
			if (dictionary == null)
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning(" GetActions actions = null");
				}
				return false;
			}
			object value2;
			if (dictionary.TryGetValue("discounts_up", out value2))
			{
				List<object> list2 = value2 as List<object>;
				if (list2 != null)
				{
					try
					{
						var list3 = (from list in list2.OfType<List<object>>()
							where list.Count > 1
							select new
							{
								ItemID = ((list[0] as string) ?? ""),
								Discount = Convert.ToInt32((long)list[1])
							}).ToList();
						var anon = list3.FirstOrDefault(entry => entry.ItemID == "shmot");
						var anon2 = list3.FirstOrDefault(entry => entry.ItemID == "armor");
						if (anon != null)
						{
							foreach (string item3 in AllIdsForPromosExceptArmor().Except(list3.Select(item => item.ItemID)))
							{
								list3.Add(new
								{
									ItemID = item3,
									Discount = anon.Discount
								});
							}
							list3.RemoveAll(item => item.ItemID == "shmot");
						}
						if (anon2 != null)
						{
							foreach (string item4 in Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Except(list3.Select(item => item.ItemID)))
							{
								list3.Add(new
								{
									ItemID = item4,
									Discount = anon2.Discount
								});
							}
							list3.RemoveAll(item => item.ItemID == "armor");
						}
						foreach (var item5 in list3)
						{
							List<SaltedInt> value3 = new List<SaltedInt>
							{
								new SaltedInt(11259645, item5.Discount)
							};
							_003C_003E4__this.discounts.Add(item5.ItemID, value3);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("Exception in processing discounts from server: " + ex);
					}
				}
			}
			object value4;
			if (dictionary.TryGetValue("topSellers_up", out value4))
			{
				List<object> list4 = value4 as List<object>;
				if (list4 != null)
				{
					foreach (string item6 in list4)
					{
						_003C_003E4__this.topSellers.Add(item6);
					}
				}
			}
			if (PromoActionsManager.ActionsUUpdated != null)
			{
				PromoActionsManager.ActionsUUpdated();
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

	[CompilerGenerated]
	internal sealed class _003CDownloadBestBuyInfo_003Ed__120 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

		private WWW _003Cresponse_003E5__1;

		private string _003Curl_003E5__2;

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
		public _003CDownloadBestBuyInfo_003Ed__120(int _003C_003E1__state)
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
				if (_003C_003E4__this._isGetBestBuyInfoRunning)
				{
					return false;
				}
				_003C_003E4__this._bestBuyGetInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetBestBuyInfoRunning = true;
				_003Curl_003E5__2 = URLs.BestBuy;
				if (string.IsNullOrEmpty(_003Curl_003E5__2))
				{
					_003C_003E4__this._isGetBestBuyInfoRunning = false;
					return false;
				}
				string value = PersistentCacheManager.Instance.GetValue(_003Curl_003E5__2);
				if (!string.IsNullOrEmpty(value))
				{
					text = value;
					break;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.BestBuy);
				_003C_003E2__current = _003Cresponse_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
				{
					UnityEngine.Debug.LogWarningFormat("Best buy response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
					_003C_003E4__this._bestBuyIds.Clear();
					_003C_003E4__this._isGetBestBuyInfoRunning = false;
					return false;
				}
				text = URLs.Sanitize(_003Cresponse_003E5__1);
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("Best buy response is empty");
					_003C_003E4__this._bestBuyIds.Clear();
					_003C_003E4__this._isGetBestBuyInfoRunning = false;
					return false;
				}
				PersistentCacheManager.Instance.SetValue(_003Curl_003E5__2, text);
				_003Cresponse_003E5__1 = null;
				break;
			}
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary == null || !dictionary.ContainsKey("bestBuy"))
			{
				UnityEngine.Debug.LogWarning("Best buy response is bad");
				_003C_003E4__this._bestBuyIds.Clear();
				_003C_003E4__this._isGetBestBuyInfoRunning = false;
				return false;
			}
			List<object> list = dictionary["bestBuy"] as List<object>;
			if (list != null)
			{
				_003C_003E4__this._bestBuyIds.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					string item = (string)list[i];
					_003C_003E4__this._bestBuyIds.Add(item);
				}
			}
			if (PromoActionsManager.BestBuyStateUpdate != null)
			{
				PromoActionsManager.BestBuyStateUpdate();
			}
			_003C_003E4__this._isGetBestBuyInfoRunning = false;
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
	internal sealed class _003C_003Ec__DisplayClass122_0
	{
		public Task futureToWait;

		internal bool _003CGetBestBuyInfoLoop_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetBestBuyInfoLoop_003Ed__122 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task futureToWait;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetBestBuyInfoLoop_003Ed__122(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass122_0
				{
					futureToWait = futureToWait
				}._003CGetBestBuyInfoLoop_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005f;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a4;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_00a4;
				}
				IL_00a4:
				if (Time.realtimeSinceStartup - _003C_003E4__this._bestBuyGetInfoStartTime < 1020f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_005f;
				IL_005f:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadBestBuyInfo());
				_003C_003E1__state = 2;
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
	internal sealed class _003CDownloadDayOfValorInfo_003Ed__144 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

		private WWW _003Cresponse_003E5__1;

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
		public _003CDownloadDayOfValorInfo_003Ed__144(int _003C_003E1__state)
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
				if (_003C_003E4__this._isGetDayOfValorInfoRunning)
				{
					return false;
				}
				_003C_003E4__this._dayOfValorGetInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetDayOfValorInfoRunning = true;
				if (string.IsNullOrEmpty(URLs.DayOfValor))
				{
					_003C_003E4__this._isGetDayOfValorInfoRunning = false;
					return false;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.DayOfValor);
				_003C_003E2__current = _003Cresponse_003E5__1;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				string text = URLs.Sanitize(_003Cresponse_003E5__1);
				if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
				{
					UnityEngine.Debug.LogWarningFormat("Day of valor response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
					_003C_003E4__this._isGetDayOfValorInfoRunning = false;
					_003C_003E4__this.ClearDataDayOfValor();
					return false;
				}
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("Best buy response is empty");
					_003C_003E4__this._isGetDayOfValorInfoRunning = false;
					_003C_003E4__this.ClearDataDayOfValor();
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				if (dictionary == null)
				{
					UnityEngine.Debug.LogWarning("Day of valor response is bad");
					_003C_003E4__this._isGetDayOfValorInfoRunning = false;
					_003C_003E4__this.ClearDataDayOfValor();
					return false;
				}
				_003C_003E4__this.ClearDataDayOfValor();
				if (dictionary.ContainsKey("startTime"))
				{
					_003C_003E4__this._dayOfValorStartTime = (long)dictionary["startTime"];
				}
				if (dictionary.ContainsKey("endTime"))
				{
					_003C_003E4__this._dayOfValorEndTime = (long)dictionary["endTime"];
				}
				if (dictionary.ContainsKey("multiplyerForExp"))
				{
					_003C_003E4__this._dayOfValorMultiplyerForExp = (long)dictionary["multiplyerForExp"];
				}
				if (dictionary.ContainsKey("multiplyerForMoney"))
				{
					_003C_003E4__this._dayOfValorMultiplyerForMoney = (long)dictionary["multiplyerForMoney"];
				}
				_003C_003E4__this._isGetDayOfValorInfoRunning = false;
				return false;
			}
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
	internal sealed class _003CGetDayOfValorInfoLoop_003Ed__145 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsManager _003C_003E4__this;

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
		public _003CGetDayOfValorInfoLoop_003Ed__145(int _003C_003E1__state)
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
				goto IL_003f;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 2:
				_003C_003E1__state = -1;
				goto IL_008b;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_008b;
				}
				IL_008b:
				if (Time.realtimeSinceStartup - _003C_003E4__this._dayOfValorGetInfoStartTime < 1050f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0046;
				IL_003f:
				if (!TrainingController.TrainingCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0046;
				IL_0046:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDayOfValorInfo());
				_003C_003E1__state = 2;
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

	public static PromoActionsManager sharedManager;

	public static bool ActionsAvailable = true;

	public Dictionary<string, List<SaltedInt>> discounts = new Dictionary<string, List<SaltedInt>>();

	public List<string> topSellers = new List<string>();

	private float startTime;

	private string promoActionAddress = URLs.PromoActionsTest;

	private const float OffersUpdateTimeout = 900f;

	private const float EventX3UpdateTimeout = 930f;

	private const float AdvertInfoTimeout = 960f;

	private float _eventX3GetInfoStartTime;

	private float _eventX3LastCheckTime;

	private long _newbieEventX3StartTime;

	private long _newbieEventX3StartTimeAdditional;

	private long _eventX3StartTime;

	private long _eventX3Duration;

	private bool _eventX3Active;

	private long _eventX3AmazonEventStartTime;

	private long _eventX3AmazonEventEndTime;

	private readonly List<long> _eventX3AmazonEventValidTimeZone = new List<long>();

	private bool _eventX3AmazonEventActive;

	private float _advertGetInfoStartTime;

	private static readonly AdvertInfo _paidAdvert = new AdvertInfo();

	private static readonly AdvertInfo _freeAdvert = new AdvertInfo();

	private static readonly ReplaceAdmobPerelivInfo _replaceAdmobPereliv = new ReplaceAdmobPerelivInfo();

	private static readonly MobileAdvertInfo _mobileAdvert = new MobileAdvertInfo();

	public static float startupTime = 0f;

	private bool _isGetEventX3InfoRunning;

	private AmazonEventInfo _amazonEventInfo;

	public static bool x3InfoDownloadaedOnceDuringCurrentRun = false;

	public const long NewbieEventX3Duration = 259200L;

	public const long NewbieEventX3Timeout = 259200L;

	private bool _isGetAdvertInfoRunning;

	private string _previousResponseText;

	private const float BestBuyInfoTimeout = 1020f;

	private List<string> _bestBuyIds = new List<string>();

	private bool _isGetBestBuyInfoRunning;

	private float _bestBuyGetInfoStartTime;

	public const int ShownCountDaysOfValor = 1;

	private const float DayOfValorInfoTimeout = 1050f;

	private long _dayOfValorStartTime;

	private long _dayOfValorEndTime;

	private long _dayOfValorMultiplyerForExp;

	private long _dayOfValorMultiplyerForMoney;

	private bool _isGetDayOfValorInfoRunning;

	private float _dayOfValorGetInfoStartTime;

	private static TimeSpan TimeToShowDaysOfValor = TimeSpan.FromHours(12.0);

	private TimeSpan _timeToEndDayOfValor;

	private const string UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY = "PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY";

	private const string UNLOCKED_ITEMS_KEY = "UnlockedItems";

	private List<string> m_itemsToRemoveFromUnlocked = new List<string>();

	private List<string> m_unlockedItems = new List<string>();

	public List<string> news
	{
		get
		{
			return NewsHandler.News;
		}
	}

	public bool IsEventX3Active
	{
		get
		{
			return _eventX3Active;
		}
	}

	public List<string> UnlockedItems
	{
		get
		{
			return m_unlockedItems;
		}
		private set
		{
			m_unlockedItems = value;
		}
	}

	public List<string> ItemsToRemoveFromUnlocked
	{
		get
		{
			return m_itemsToRemoveFromUnlocked;
		}
		private set
		{
			m_itemsToRemoveFromUnlocked = value;
		}
	}

	public bool IsAmazonEventX3Active
	{
		get
		{
			if (_amazonEventInfo == null)
			{
				return false;
			}
			if (_amazonEventInfo.DurationSeconds <= float.Epsilon)
			{
				return false;
			}
			if (!CheckTimezone(_amazonEventInfo.Timezones))
			{
				return false;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (_amazonEventInfo.StartTime <= utcNow)
			{
				return utcNow <= _amazonEventInfo.EndTime;
			}
			return false;
		}
	}

	public long EventX3RemainedTime
	{
		get
		{
			if (IsEventX3Active)
			{
				return _eventX3StartTime + _eventX3Duration - CurrentUnixTime;
			}
			return 0L;
		}
	}

	public static AdvertInfo Advert
	{
		get
		{
			if (!StoreKitEventListener.IsPayingUser())
			{
				return _freeAdvert;
			}
			return _paidAdvert;
		}
	}

	internal static ReplaceAdmobPerelivInfo ReplaceAdmobPereliv
	{
		get
		{
			return _replaceAdmobPereliv;
		}
	}

	public static MobileAdvertInfo MobileAdvert
	{
		get
		{
			return _mobileAdvert;
		}
	}

	public static bool MobileAdvertIsReady { get; private set; }

	internal AmazonEventInfo AmazonEvent
	{
		get
		{
			return _amazonEventInfo;
		}
	}

	public bool IsNewbieEventX3Active
	{
		get
		{
			if (_newbieEventX3StartTime == 0L)
			{
				return false;
			}
			long currentUnixTime = CurrentUnixTime;
			long num = _newbieEventX3StartTime + 259200 + 259200;
			if (currentUnixTime >= num)
			{
				ResetNewbieX3StartTime();
				return false;
			}
			if (_newbieEventX3StartTime < currentUnixTime)
			{
				return currentUnixTime < _newbieEventX3StartTime + 259200;
			}
			return false;
		}
	}

	private bool IsX3StartTimeAfterNewbieX3TimeoutEndTime
	{
		get
		{
			if (_newbieEventX3StartTimeAdditional == 0L)
			{
				return true;
			}
			long num = _newbieEventX3StartTimeAdditional + 259200 + 259200;
			return _eventX3StartTime >= num;
		}
	}

	public static long CurrentUnixTime
	{
		get
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return (long)(DateTime.UtcNow - dateTime).TotalSeconds;
		}
	}

	public bool IsDayOfValorEventActive { get; private set; }

	public int DayOfValorMultiplyerForExp
	{
		get
		{
			if (!IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)_dayOfValorMultiplyerForExp;
		}
	}

	public int DayOfValorMultiplyerForMoney
	{
		get
		{
			if (!IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)_dayOfValorMultiplyerForMoney;
		}
	}

	public static event Action OnUnlockedItemsUpdated;

	public static event Action ActionsUUpdated;

	public static event Action EventX3Updated;

	public static event Action EventAmazonX3Updated;

	public static event Action BestBuyStateUpdate;

	public static event OnDayOfValorEnableDelegate OnDayOfValorEnable;

	public static void FireUnlockedItemsUpdated()
	{
		Action onUnlockedItemsUpdated = PromoActionsManager.OnUnlockedItemsUpdated;
		if (onUnlockedItemsUpdated != null)
		{
			onUnlockedItemsUpdated();
		}
	}

	public void RemoveItemFromUnlocked(string item)
	{
		try
		{
			UnlockedItems.Remove(item);
			ItemsToRemoveFromUnlocked.Remove(item);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in RemoveItemFromUnlocked: {0}", ex);
		}
	}

	public void ReplaceUnlockedItemsWith(List<string> itemsViewed)
	{
		try
		{
			UnlockedItems.Clear();
			UnlockedItems.AddRange(itemsViewed);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in ReplaceUnlockedItemsWith: {0}", ex);
		}
	}

	public void RemoveViewedUnlockedItems()
	{
		ItemsToRemoveFromUnlocked.Clear();
	}

	public int ItemsViewed(List<string> itemsViewed)
	{
		try
		{
			List<string> itemsToRemoveFromUnlocked = UnlockedItems.Intersect(itemsViewed).ToList();
			int num = itemsToRemoveFromUnlocked.Count();
			if (num > 0)
			{
				UnlockedItems.RemoveAll((string item) => itemsToRemoveFromUnlocked.Contains(item));
				ItemsToRemoveFromUnlocked = Enumerable.Union(second: itemsToRemoveFromUnlocked.ToList(), first: ItemsToRemoveFromUnlocked).ToList();
			}
			return num;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in ItemsViewed: {0}", ex);
			return 0;
		}
	}

	private void Awake()
	{
		LoadUnlockedItems();
		startupTime = Time.realtimeSinceStartup;
		promoActionAddress = URLs.PromoActions;
	}

	private void Start()
	{
		sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Task futureToWait = PersistentCacheManager.Instance.StartDownloadSignaturesLoop();
		StartCoroutine(GetActionsLoop(futureToWait));
		StartCoroutine(GetEventX3InfoLoop());
		StartCoroutine(GetAdvertInfoLoop(futureToWait));
		StartCoroutine(AdsConfigManager.Instance.GetAdvertInfoLoop(futureToWait));
		StartCoroutine(PerelivConfigManager.Instance.GetPerelivConfigLoop(futureToWait));
		StartCoroutine(GetBestBuyInfoLoop(futureToWait));
		StartCoroutine(GetDayOfValorInfoLoop());
		StartCoroutine(MiniGameRatingDownloader.Instance.GetMiniGameRatingLoop());
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - _eventX3LastCheckTime >= 1f)
		{
			CheckEventX3Active();
			if (Time.frameCount % 120 == 0)
			{
				RefreshAmazonEvent();
			}
			CheckDayOfValorActive();
			_eventX3LastCheckTime = Time.realtimeSinceStartup;
		}
	}

	private IEnumerator OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			yield return null;
			yield return null;
			yield return null;
			StartCoroutine(GetActions());
			StartCoroutine(GetEventX3Info());
			StartCoroutine(GetAmazonEventCoroutine());
			StartCoroutine(GetAdvertInfo());
			StartCoroutine(DownloadBestBuyInfo());
			StartCoroutine(DownloadDayOfValorInfo());
		}
		else
		{
			SaveUnlockedItems();
		}
	}

	private IEnumerator GetActionsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		while (true)
		{
			StartCoroutine(GetActions());
			while (Time.realtimeSinceStartup - startTime < 900f)
			{
				yield return null;
			}
		}
	}

	private IEnumerator GetEventX3InfoLoop()
	{
		UpdateNewbieEventX3Info();
		while (true)
		{
			yield return StartCoroutine(GetEventX3Info());
			yield return StartCoroutine(GetAmazonEventCoroutine());
			while (Time.realtimeSinceStartup - _eventX3GetInfoStartTime < 930f)
			{
				yield return null;
			}
		}
	}

	private IEnumerator GetAdvertInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(GetAdvertInfo());
			while (Time.realtimeSinceStartup - _advertGetInfoStartTime < 960f)
			{
				yield return null;
			}
		}
	}

	private IEnumerator GetAmazonEventCoroutine()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.AmazonEvent);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		string text = URLs.Sanitize(response);
		if (!string.IsNullOrEmpty(response.error))
		{
			UnityEngine.Debug.LogWarning("Amazon event response error: " + response.error);
			yield break;
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("Amazon event response is empty");
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("Amazon event bad response: " + text);
		}
		else
		{
			if (!IsNeedCheckAmazonEventX3())
			{
				yield break;
			}
			_amazonEventInfo = new AmazonEventInfo();
			object value;
			if (dictionary.TryGetValue("startTime", out value))
			{
				try
				{
					_amazonEventInfo.StartTime = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			object value2;
			if (dictionary.TryGetValue("durationSeconds", out value2))
			{
				try
				{
					_amazonEventInfo.DurationSeconds = Convert.ToSingle(value2);
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogException(exception2);
				}
			}
			object value3;
			if (dictionary.TryGetValue("timezones", out value3))
			{
				List<object> source = (value3 as List<object>) ?? new List<object>();
				try
				{
					_amazonEventInfo.Timezones = source.Select(Convert.ToInt32).ToList();
				}
				catch (Exception exception3)
				{
					UnityEngine.Debug.LogException(exception3);
				}
			}
			object value4;
			if (dictionary.TryGetValue("percentage", out value4))
			{
				try
				{
					_amazonEventInfo.Percentage = Convert.ToSingle(value4);
				}
				catch (Exception exception4)
				{
					UnityEngine.Debug.LogException(exception4);
				}
			}
			object value5;
			if (dictionary.TryGetValue("caption", out value5))
			{
				try
				{
					_amazonEventInfo.Caption = Convert.ToString(value5, CultureInfo.InvariantCulture) ?? string.Empty;
				}
				catch (Exception exception5)
				{
					UnityEngine.Debug.LogException(exception5);
				}
			}
			RefreshAmazonEvent();
		}
	}

	private IEnumerator GetEventX3Info()
	{
		if (_isGetEventX3InfoRunning)
		{
			yield break;
		}
		_eventX3GetInfoStartTime = Time.realtimeSinceStartup;
		_isGetEventX3InfoRunning = true;
		if (string.IsNullOrEmpty(URLs.EventX3))
		{
			_isGetEventX3InfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.EventX3);
		yield return response;
		string text = URLs.Sanitize(response);
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.LogWarningFormat("EventX3 response error: {0}", (response != null) ? response.error : "null");
			}
			_isGetEventX3InfoRunning = false;
			_eventX3GetInfoStartTime = Time.realtimeSinceStartup - 930f + 15f;
			yield break;
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("EventX3 response is empty");
			_isGetEventX3InfoRunning = false;
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("start") || !dictionary.ContainsKey("duration"))
		{
			UnityEngine.Debug.LogWarning("EventX3 response is bad");
			_isGetEventX3InfoRunning = false;
			yield break;
		}
		long eventX3StartTime = (long)dictionary["start"];
		long eventX3Duration = (long)dictionary["duration"];
		_eventX3StartTime = eventX3StartTime;
		_eventX3Duration = eventX3Duration;
		CheckEventX3Active();
		x3InfoDownloadaedOnceDuringCurrentRun = true;
		_isGetEventX3InfoRunning = false;
	}

	private bool IsNeedCheckAmazonEventX3()
	{
		if (Defs.IsDeveloperBuild)
		{
			return true;
		}
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			return false;
		}
		return true;
	}

	private static bool CheckTimezone(List<int> timezones)
	{
		if (timezones == null)
		{
			return false;
		}
		return timezones.Any(DateTimeOffset.Now.Offset.Hours.Equals);
	}

	private bool CheckAvailabelTimeZoneForAmazonEvent()
	{
		if (!_eventX3Active)
		{
			return false;
		}
		if (_eventX3AmazonEventValidTimeZone == null || _eventX3AmazonEventValidTimeZone.Count == 0)
		{
			return false;
		}
		TimeSpan offset = DateTimeOffset.Now.Offset;
		for (int i = 0; i < _eventX3AmazonEventValidTimeZone.Count; i++)
		{
			if (_eventX3AmazonEventValidTimeZone[i] == offset.Hours)
			{
				return true;
			}
		}
		return false;
	}

	private void ParseAmazonEventData(Dictionary<string, object> jsonData)
	{
		if (jsonData.ContainsKey("startAmazonEventTime"))
		{
			_eventX3AmazonEventStartTime = (long)jsonData["startAmazonEventTime"];
		}
		if (jsonData.ContainsKey("endAmazonEventTime"))
		{
			_eventX3AmazonEventEndTime = (long)jsonData["endAmazonEventTime"];
		}
		if (jsonData.ContainsKey("timeZonesForEventAmazon"))
		{
			List<object> list = jsonData["timeZonesForEventAmazon"] as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				_eventX3AmazonEventValidTimeZone.Add((long)list[i]);
			}
		}
	}

	private void RefreshAmazonEvent()
	{
		if (PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	[Obsolete]
	private void CheckAmazonEventX3Active()
	{
		if (!_eventX3Active || !CheckAvailabelTimeZoneForAmazonEvent())
		{
			_eventX3AmazonEventActive = false;
			return;
		}
		bool eventX3AmazonEventActive = _eventX3AmazonEventActive;
		if (_eventX3AmazonEventStartTime != 0L && _eventX3AmazonEventEndTime != 0L)
		{
			long currentUnixTime = CurrentUnixTime;
			_eventX3AmazonEventActive = _eventX3StartTime < currentUnixTime && currentUnixTime < _eventX3AmazonEventEndTime;
		}
		else
		{
			_eventX3AmazonEventStartTime = 0L;
			_eventX3AmazonEventEndTime = 0L;
			_eventX3AmazonEventActive = false;
		}
		if (_eventX3AmazonEventActive != eventX3AmazonEventActive && PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	public void ForceCheckEventX3Active()
	{
		CheckEventX3Active();
	}

	private void CheckEventX3Active()
	{
		bool eventX3Active = _eventX3Active;
		if (IsNewbieEventX3Active)
		{
			_eventX3StartTime = _newbieEventX3StartTime;
			_eventX3Duration = 259200L;
			_eventX3Active = true;
		}
		else if (_eventX3StartTime != 0L && _eventX3Duration != 0L && IsX3StartTimeAfterNewbieX3TimeoutEndTime)
		{
			long currentUnixTime = CurrentUnixTime;
			_eventX3Active = _eventX3StartTime < currentUnixTime && currentUnixTime < _eventX3StartTime + _eventX3Duration;
		}
		else
		{
			_eventX3StartTime = 0L;
			_eventX3Duration = 0L;
			_eventX3Active = false;
		}
		if (_eventX3Active != eventX3Active)
		{
			if (_eventX3Active)
			{
				PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, 1);
				PlayerPrefs.Save();
			}
			if (PromoActionsManager.EventX3Updated != null)
			{
				PromoActionsManager.EventX3Updated();
			}
		}
	}

	private void ResetNewbieX3StartTime()
	{
		if (_newbieEventX3StartTime != 0L)
		{
			Storager.setString(Defs.NewbieEventX3StartTime, 0L.ToString());
			_newbieEventX3StartTime = 0L;
		}
	}

	public static long GetUnixTimeFromStorage(string storageId)
	{
		long result = 0L;
		if (Storager.hasKey(storageId))
		{
			long.TryParse(Storager.getString(storageId), out result);
		}
		return result;
	}

	public void UpdateNewbieEventX3Info()
	{
		_newbieEventX3StartTime = GetUnixTimeFromStorage(Defs.NewbieEventX3StartTime);
		_newbieEventX3StartTimeAdditional = GetUnixTimeFromStorage(Defs.NewbieEventX3StartTimeAdditional);
	}

	private long GetNewbieEventX3LastLoggedTime()
	{
		if (_newbieEventX3StartTime != 0L)
		{
			return GetUnixTimeFromStorage(Defs.NewbieEventX3LastLoggedTime);
		}
		return 0L;
	}

	private IEnumerator GetAdvertInfo()
	{
		if (_isGetAdvertInfoRunning)
		{
			yield break;
		}
		_advertGetInfoStartTime = Time.realtimeSinceStartup;
		_isGetAdvertInfoRunning = true;
		_paidAdvert.enabled = false;
		_freeAdvert.enabled = false;
		string advert = URLs.Advert;
		if (string.IsNullOrEmpty(advert))
		{
			_isGetAdvertInfoRunning = false;
			yield break;
		}
		string value = PersistentCacheManager.Instance.GetValue(advert);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(advert);
			yield return response;
			if (response == null || !string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarningFormat("Advert response error: {0}", (response != null) ? response.error : "null");
				_isGetAdvertInfoRunning = false;
				yield break;
			}
			text = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogWarning("Advert response is empty");
				_isGetAdvertInfoRunning = false;
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(response.url, text);
		}
		object obj = Json.Deserialize(text);
		Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
		if (obj == null)
		{
			_isGetAdvertInfoRunning = false;
			yield break;
		}
		if (dictionary.ContainsKey("paid"))
		{
			ParseAdvertInfo(dictionary["paid"], _paidAdvert);
		}
		if (dictionary.ContainsKey("free"))
		{
			ParseAdvertInfo(dictionary["free"], _freeAdvert);
		}
		if (dictionary.ContainsKey("replace_admob_pereliv_10_2_0"))
		{
			ParseReplaceAdmobPereliv(dictionary["replace_admob_pereliv_10_2_0"] as Dictionary<string, object>, _replaceAdmobPereliv);
		}
		else
		{
			UnityEngine.Debug.Log("Advert response doesn't contain “replace_admob_pereliv_10_2_0” property.");
		}
		_isGetAdvertInfoRunning = false;
		MobileAdvertIsReady = true;
	}

	private static void ParseReplaceAdmobPereliv(Dictionary<string, object> replaceAdmob, ReplaceAdmobPerelivInfo replaceAdmobObj)
	{
		if (replaceAdmob != null)
		{
			try
			{
				foreach (string item in (replaceAdmob["imageUrls"] as List<object>).OfType<string>().ToList())
				{
					replaceAdmobObj.imageUrls.Add(item);
				}
				foreach (string item2 in (replaceAdmob["adUrls"] as List<object>).OfType<string>().ToList())
				{
					replaceAdmobObj.adUrls.Add(item2);
				}
				replaceAdmobObj.enabled = Convert.ToBoolean(replaceAdmob["enabled"]);
				replaceAdmobObj.ShowEveryTimes = Mathf.Max(Convert.ToInt32(replaceAdmob["showEveryTimes"]), 1);
				replaceAdmobObj.ShowTimesTotal = Mathf.Max(Convert.ToInt32(replaceAdmob["showTimesTotal"]), 0);
				replaceAdmobObj.ShowToPaying = Convert.ToBoolean(replaceAdmob["showToPaying"]);
				replaceAdmobObj.ShowToNew = Convert.ToBoolean(replaceAdmob["showToNew"]);
				try
				{
					replaceAdmobObj.MinLevel = Convert.ToInt32(replaceAdmob["minLevel"]);
				}
				catch
				{
					replaceAdmobObj.MinLevel = -1;
				}
				try
				{
					replaceAdmobObj.MaxLevel = Convert.ToInt32(replaceAdmob["maxLevel"]);
					return;
				}
				catch
				{
					replaceAdmobObj.MaxLevel = -1;
					return;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogWarning(message);
				return;
			}
		}
		UnityEngine.Debug.LogWarning("replaceAdmob == null");
	}

	private void ParseAdvertInfo(object advertInfoObj, AdvertInfo advertInfo)
	{
		Dictionary<string, object> dictionary = advertInfoObj as Dictionary<string, object>;
		if (dictionary != null)
		{
			advertInfo.imageUrl = Convert.ToString(dictionary["imageUrl"]);
			advertInfo.adUrl = Convert.ToString(dictionary["adUrl"]);
			advertInfo.message = Convert.ToString(dictionary["text"]);
			advertInfo.showAlways = Convert.ToBoolean(dictionary["showAlways"]);
			advertInfo.btnClose = Convert.ToBoolean(dictionary["btnClose"]);
			advertInfo.minLevel = Convert.ToInt32(dictionary["minLevel"]);
			advertInfo.maxLevel = Convert.ToInt32(dictionary["maxLevel"]);
			advertInfo.enabled = Convert.ToBoolean(dictionary["enabled"]);
		}
	}

	private void ClearAll()
	{
		discounts.Clear();
		topSellers.Clear();
	}

	public static List<string> AllIdsForPromosExceptArmor()
	{
		IEnumerable<string> second = from kvp in WeaponManager.tagToStoreIDMapping
			where kvp.Value != null && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(kvp.Value)
			select kvp.Key;
		return Wear.wear.SelectMany((KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> kvp) => (kvp.Key == ShopNGUIController.CategoryNames.ArmorCategory) ? new List<List<string>>() : kvp.Value).SelectMany((List<string> list) => list).Except(new List<string> { "hat_Adamant_3" })
			.Except(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0])
			.Concat(SkinsController.shopKeyFromNameSkin.Values)
			.Concat(second)
			.Concat(WeaponSkinsManager.AllSkins.Select((WeaponSkin info) => info.Id))
			.Concat(GadgetsInfo.info.Keys)
			.Distinct()
			.ToList();
	}

	public IEnumerator GetActions()
	{
		startTime = Time.realtimeSinceStartup;
		string value = PersistentCacheManager.Instance.GetValue(promoActionAddress);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW download = Tools.CreateWwwIfNotConnected(promoActionAddress);
			if (download == null)
			{
				yield break;
			}
			yield return download;
			string text2 = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text2) && UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("GetActions response:    " + text2);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("GetActions error:    " + download.error);
				}
				ClearAll();
				ActionsAvailable = false;
				yield break;
			}
			text = text2;
			PersistentCacheManager.Instance.SetValue(download.url, text);
		}
		if (_previousResponseText != null && text != null && text == _previousResponseText)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("GetActions:    responseText == _previousResponseText");
			}
			yield break;
		}
		_previousResponseText = text;
		ActionsAvailable = true;
		ClearAll();
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetActions actions = null");
			}
			yield break;
		}
		object value2;
		if (dictionary.TryGetValue("discounts_up", out value2))
		{
			List<object> list2 = value2 as List<object>;
			if (list2 != null)
			{
				try
				{
					var list3 = (from list in list2.OfType<List<object>>()
						where list.Count > 1
						select new
						{
							ItemID = ((list[0] as string) ?? ""),
							Discount = Convert.ToInt32((long)list[1])
						}).ToList();
					var anon = list3.FirstOrDefault(entry => entry.ItemID == "shmot");
					var anon2 = list3.FirstOrDefault(entry => entry.ItemID == "armor");
					if (anon != null)
					{
						foreach (string item3 in AllIdsForPromosExceptArmor().Except(list3.Select(item => item.ItemID)))
						{
							list3.Add(new
							{
								ItemID = item3,
								Discount = anon.Discount
							});
						}
						list3.RemoveAll(item => item.ItemID == "shmot");
					}
					if (anon2 != null)
					{
						foreach (string item4 in Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Except(list3.Select(item => item.ItemID)))
						{
							list3.Add(new
							{
								ItemID = item4,
								Discount = anon2.Discount
							});
						}
						list3.RemoveAll(item => item.ItemID == "armor");
					}
					foreach (var item5 in list3)
					{
						List<SaltedInt> value3 = new List<SaltedInt>
						{
							new SaltedInt(11259645, item5.Discount)
						};
						discounts.Add(item5.ItemID, value3);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in processing discounts from server: " + ex);
				}
			}
		}
		object value4;
		if (dictionary.TryGetValue("topSellers_up", out value4))
		{
			List<object> list4 = value4 as List<object>;
			if (list4 != null)
			{
				foreach (string item6 in list4)
				{
					topSellers.Add(item6);
				}
			}
		}
		if (PromoActionsManager.ActionsUUpdated != null)
		{
			PromoActionsManager.ActionsUUpdated();
		}
	}

	private IEnumerator DownloadBestBuyInfo()
	{
		if (_isGetBestBuyInfoRunning)
		{
			yield break;
		}
		_bestBuyGetInfoStartTime = Time.realtimeSinceStartup;
		_isGetBestBuyInfoRunning = true;
		string url = URLs.BestBuy;
		if (string.IsNullOrEmpty(url))
		{
			_isGetBestBuyInfoRunning = false;
			yield break;
		}
		string value = PersistentCacheManager.Instance.GetValue(url);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(URLs.BestBuy);
			yield return response;
			if (response == null || !string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarningFormat("Best buy response error: {0}", (response != null) ? response.error : "null");
				_bestBuyIds.Clear();
				_isGetBestBuyInfoRunning = false;
				yield break;
			}
			text = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogWarning("Best buy response is empty");
				_bestBuyIds.Clear();
				_isGetBestBuyInfoRunning = false;
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(url, text);
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("bestBuy"))
		{
			UnityEngine.Debug.LogWarning("Best buy response is bad");
			_bestBuyIds.Clear();
			_isGetBestBuyInfoRunning = false;
			yield break;
		}
		List<object> list = dictionary["bestBuy"] as List<object>;
		if (list != null)
		{
			_bestBuyIds.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				string item = (string)list[i];
				_bestBuyIds.Add(item);
			}
		}
		if (PromoActionsManager.BestBuyStateUpdate != null)
		{
			PromoActionsManager.BestBuyStateUpdate();
		}
		_isGetBestBuyInfoRunning = false;
	}

	public bool IsBankItemBestBuy(PurchaseEventArgs purchaseInfo)
	{
		if (_bestBuyIds.Count == 0 || purchaseInfo == null)
		{
			return false;
		}
		string text = ((purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Gems) ? "gems" : "coins");
		string item = string.Format("{0}_{1}", new object[2]
		{
			text,
			purchaseInfo.Index + 1
		});
		return _bestBuyIds.Contains(item);
	}

	private IEnumerator GetBestBuyInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (true)
		{
			yield return StartCoroutine(DownloadBestBuyInfo());
			while (Time.realtimeSinceStartup - _bestBuyGetInfoStartTime < 1020f)
			{
				yield return null;
			}
		}
	}

	private void ClearDataDayOfValor()
	{
		_dayOfValorStartTime = 0L;
		_dayOfValorEndTime = 0L;
		_dayOfValorMultiplyerForExp = 1L;
		_dayOfValorMultiplyerForMoney = 1L;
	}

	private IEnumerator DownloadDayOfValorInfo()
	{
		if (_isGetDayOfValorInfoRunning)
		{
			yield break;
		}
		_dayOfValorGetInfoStartTime = Time.realtimeSinceStartup;
		_isGetDayOfValorInfoRunning = true;
		if (string.IsNullOrEmpty(URLs.DayOfValor))
		{
			_isGetDayOfValorInfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.DayOfValor);
		yield return response;
		string text = URLs.Sanitize(response);
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			UnityEngine.Debug.LogWarningFormat("Day of valor response error: {0}", (response != null) ? response.error : "null");
			_isGetDayOfValorInfoRunning = false;
			ClearDataDayOfValor();
			yield break;
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("Best buy response is empty");
			_isGetDayOfValorInfoRunning = false;
			ClearDataDayOfValor();
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("Day of valor response is bad");
			_isGetDayOfValorInfoRunning = false;
			ClearDataDayOfValor();
			yield break;
		}
		ClearDataDayOfValor();
		if (dictionary.ContainsKey("startTime"))
		{
			_dayOfValorStartTime = (long)dictionary["startTime"];
		}
		if (dictionary.ContainsKey("endTime"))
		{
			_dayOfValorEndTime = (long)dictionary["endTime"];
		}
		if (dictionary.ContainsKey("multiplyerForExp"))
		{
			_dayOfValorMultiplyerForExp = (long)dictionary["multiplyerForExp"];
		}
		if (dictionary.ContainsKey("multiplyerForMoney"))
		{
			_dayOfValorMultiplyerForMoney = (long)dictionary["multiplyerForMoney"];
		}
		_isGetDayOfValorInfoRunning = false;
	}

	private IEnumerator GetDayOfValorInfoLoop()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(DownloadDayOfValorInfo());
			while (Time.realtimeSinceStartup - _dayOfValorGetInfoStartTime < 1050f)
			{
				yield return null;
			}
		}
	}

	private void CheckDayOfValorActive()
	{
		bool isDayOfValorEventActive = IsDayOfValorEventActive;
		if (_dayOfValorStartTime != 0L && _dayOfValorEndTime != 0L && ExpController.LobbyLevel >= 1)
		{
			long currentUnixTime = CurrentUnixTime;
			IsDayOfValorEventActive = _dayOfValorStartTime < currentUnixTime && currentUnixTime < _dayOfValorEndTime;
			_timeToEndDayOfValor = TimeSpan.FromSeconds(_dayOfValorEndTime - currentUnixTime);
		}
		else
		{
			ClearDataDayOfValor();
			IsDayOfValorEventActive = false;
		}
		if (IsDayOfValorEventActive != isDayOfValorEventActive && PromoActionsManager.OnDayOfValorEnable != null)
		{
			PromoActionsManager.OnDayOfValorEnable(IsDayOfValorEventActive);
		}
	}

	public static void UpdateDaysOfValorShownCondition()
	{
		string @string = PlayerPrefs.GetString("LastTimeShowDaysOfValor", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			DateTime result = default(DateTime);
			if (DateTime.TryParse(@string, out result) && DateTime.UtcNow - result >= TimeToShowDaysOfValor)
			{
				PlayerPrefs.SetInt("DaysOfValorShownCount", 1);
			}
		}
	}

	public string GetTimeToEndDaysOfValor()
	{
		if (!IsDayOfValorEventActive)
		{
			return string.Empty;
		}
		if (_timeToEndDayOfValor.Days > 0)
		{
			return string.Format("{0} days {1:00}:{2:00}:{3:00}", _timeToEndDayOfValor.Days, _timeToEndDayOfValor.Hours, _timeToEndDayOfValor.Minutes, _timeToEndDayOfValor.Seconds);
		}
		return string.Format("{0:00}:{1:00}:{2:00}", new object[3] { _timeToEndDayOfValor.Hours, _timeToEndDayOfValor.Minutes, _timeToEndDayOfValor.Seconds });
	}

	internal void SaveUnlockedItems()
	{
		try
		{
			Dictionary<string, List<string>> obj = new Dictionary<string, List<string>> { { "UnlockedItems", UnlockedItems } };
			Storager.setString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY", Json.Serialize(obj));
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in SaveUnlockedItems: {0}", ex);
		}
	}

	private void LoadUnlockedItems()
	{
		try
		{
			if (!Storager.hasKey("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY"))
			{
				Dictionary<string, List<string>> obj = new Dictionary<string, List<string>> { 
				{
					"UnlockedItems",
					new List<string>()
				} };
				Storager.setString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY", Json.Serialize(obj));
			}
			Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY")) as Dictionary<string, object>;
			UnlockedItems = (dictionary["UnlockedItems"] as List<object>).OfType<string>().ToList();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in LoadUnlockedItems: {0}", ex);
			m_unlockedItems = new List<string>();
		}
	}
}
