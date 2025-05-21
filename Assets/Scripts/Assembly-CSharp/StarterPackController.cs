using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class StarterPackController : MonoBehaviour
{
	public delegate void OnStarterPackEnableDelegate(bool enable);

	[CompilerGenerated]
	internal sealed class _003CDownloadDataAboutEvent_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StarterPackController _003C_003E4__this;

		private WWW _003CdownloadData_003E5__1;

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
		public _003CDownloadDataAboutEvent_003Ed__33(int _003C_003E1__state)
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
				if (_003C_003E4__this._isDownloadDataRun)
				{
					return false;
				}
				_003C_003E4__this._isDownloadDataRun = true;
				string urlForDownloadEventData = StarterPackModel.GetUrlForDownloadEventData();
				_003CdownloadData_003E5__1 = Tools.CreateWwwIfNotConnected(urlForDownloadEventData);
				if (_003CdownloadData_003E5__1 == null)
				{
					_003C_003E4__this._isDownloadDataRun = false;
					return false;
				}
				_003C_003E2__current = _003CdownloadData_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CdownloadData_003E5__1.error))
				{
					UnityEngine.Debug.LogFormat("DownloadDataAboutEvent error: {0}", _003CdownloadData_003E5__1.error);
					_003C_003E4__this._starterPacksData.Clear();
					_003C_003E4__this._isDownloadDataRun = false;
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(URLs.Sanitize(_003CdownloadData_003E5__1)) as Dictionary<string, object>;
				if (dictionary == null)
				{
					UnityEngine.Debug.Log("DownloadDataAboutEvent eventData = null");
					_003C_003E4__this._isDownloadDataRun = false;
					return false;
				}
				_003C_003E4__this._starterPacksData.Clear();
				if (!dictionary.ContainsKey("packs"))
				{
					_003C_003E4__this._isDownloadDataRun = false;
					return false;
				}
				List<object> list = dictionary["packs"] as List<object>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
						if (dictionary2 == null)
						{
							continue;
						}
						StarterPackData starterPackData = new StarterPackData();
						if (dictionary2.ContainsKey("blockLevel"))
						{
							starterPackData.blockLevel = Convert.ToInt32((long)dictionary2["blockLevel"]);
						}
						if (dictionary2.ContainsKey("coinsCost"))
						{
							starterPackData.coinsCost = Convert.ToInt32((long)dictionary2["coinsCost"]);
						}
						else if (dictionary2.ContainsKey("gemsCost"))
						{
							starterPackData.gemsCost = Convert.ToInt32((long)dictionary2["gemsCost"]);
						}
						if (dictionary2.ContainsKey("enable"))
						{
							int num = Convert.ToInt32((long)dictionary2["enable"]);
							starterPackData.enable = num == 1;
						}
						if (dictionary2.ContainsKey("items"))
						{
							List<object> list2 = dictionary2["items"] as List<object>;
							if (list2 != null)
							{
								starterPackData.items = new List<StarterPackItemData>();
								for (int j = 0; j < list2.Count; j++)
								{
									Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
									StarterPackItemData starterPackItemData = new StarterPackItemData();
									if (dictionary3 == null)
									{
										continue;
									}
									if (dictionary3.ContainsKey("tagsVariant"))
									{
										List<object> list3 = dictionary3["tagsVariant"] as List<object>;
										if (list3 != null)
										{
											for (int k = 0; k < list3.Count; k++)
											{
												starterPackItemData.variantTags.Add((string)list3[k]);
											}
										}
									}
									if (dictionary3.ContainsKey("count"))
									{
										starterPackItemData.count = Convert.ToInt32((long)dictionary3["count"]);
									}
									starterPackData.items.Add(starterPackItemData);
								}
							}
						}
						if (dictionary2.ContainsKey("sale"))
						{
							starterPackData.sale = Convert.ToInt32((long)dictionary2["sale"]);
						}
						if (dictionary2.ContainsKey("coinsCount"))
						{
							starterPackData.coinsCount = Convert.ToInt32((long)dictionary2["coinsCount"]);
						}
						if (dictionary2.ContainsKey("gemsCount"))
						{
							starterPackData.gemsCount = Convert.ToInt32((long)dictionary2["gemsCount"]);
						}
						_003C_003E4__this._starterPacksData.Add(starterPackData);
					}
				}
				_003C_003E4__this._isDownloadDataRun = false;
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
	internal sealed class _003CCheckStartStarterPackEvent_003Ed__43 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StarterPackController _003C_003E4__this;

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
		public _003CCheckStartStarterPackEvent_003Ed__43(int _003C_003E1__state)
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
				if (!TrainingController.TrainingCompleted)
				{
					_003C_003E4__this.CheckCancelCurrentStarterPack();
					return false;
				}
				if (!_003C_003E4__this.IsPlayerNotPayBeforeStartEvent() || ExpController.LobbyLevel < 3)
				{
					_003C_003E4__this.CheckCancelCurrentStarterPack();
					return false;
				}
				if (Time.time - _003C_003E4__this.timeUpdateConfig > 3600f)
				{
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDataAboutEvent());
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.timeUpdateConfig = Time.time;
				break;
			}
			if (_003C_003E4__this.IsEventInEndState() && !_003C_003E4__this.IsCooldownEventEnd())
			{
				return false;
			}
			_003C_003E4__this._orderCurrentPack = _003C_003E4__this.GetOrderCurrentPack();
			if (_003C_003E4__this._orderCurrentPack == -1)
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			int maxValidOrderPack = _003C_003E4__this.GetMaxValidOrderPack();
			if (maxValidOrderPack == -1)
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (maxValidOrderPack > _003C_003E4__this._orderCurrentPack)
			{
				_003C_003E4__this._orderCurrentPack = maxValidOrderPack;
				Storager.setInt("NextNumberStarterPack", _003C_003E4__this._orderCurrentPack);
				Storager.setString("StartTimeShowStarterPack", string.Empty);
			}
			else if (maxValidOrderPack < _003C_003E4__this._orderCurrentPack)
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (!_003C_003E4__this.IsCurrentPackEnable())
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (_003C_003E4__this.IsStarterPackBuyOnWp8(_003C_003E4__this._orderCurrentPack))
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (_003C_003E4__this.IsStarterPackBuyByPackOrder(_003C_003E4__this._orderCurrentPack))
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (_003C_003E4__this.IsInvalidCurrentPack())
			{
				_003C_003E4__this.CheckCancelCurrentStarterPack();
				return false;
			}
			if (string.IsNullOrEmpty(Storager.getString("StartTimeShowStarterPack")))
			{
				_003C_003E4__this.InitializeEvent();
			}
			else
			{
				_003C_003E4__this._timeStartEvent = StarterPackModel.GetTimeDataEvent("StartTimeShowStarterPack");
			}
			bool isEventActive = _003C_003E4__this.isEventActive;
			_003C_003E4__this.isEventActive = true;
			if (isEventActive != _003C_003E4__this.isEventActive)
			{
				_003C_003E4__this.CheckSendEventChangeEnabled();
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
	internal sealed class _003CTryRestoreStarterPackByProductId_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string productId;

		public StarterPackController _003C_003E4__this;

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
		public _003CTryRestoreStarterPackByProductId_003Ed__60(int _003C_003E1__state)
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
				if (Application.loadedLevelName == "Loading")
				{
					return false;
				}
				if (!StoreKitEventListener.starterPackIds.Contains(productId))
				{
					return false;
				}
				if (_003C_003E4__this.IsStarterPackBuy(productId))
				{
					return false;
				}
				if (_003C_003E4__this._starterPacksData.Count == 0)
				{
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDataAboutEvent());
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			int orderPackByProductId = _003C_003E4__this.GetOrderPackByProductId(productId);
			if (_003C_003E4__this.IsInvalidPack(orderPackByProductId))
			{
				return false;
			}
			if (_003C_003E4__this.GetPackType(orderPackByProductId) != 0)
			{
				return false;
			}
			if (_003C_003E4__this.TryTakePurchases(productId, orderPackByProductId, true))
			{
				FyberFacade.Instance.SetUserPaying("1");
				StoreKitEventListener.SetLastPaymentTime();
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

	private DateTime _timeStartEvent;

	private TimeSpan _timeLiveEvent;

	private TimeSpan _timeToEndEvent;

	private List<StarterPackData> _starterPacksData;

	private int _orderCurrentPack;

	private bool _isDownloadDataRun;

	private StoreKitEventListener _storeKitEventListener;

	private float _lastCheckEventTime;

	private float timeUpdateConfig = -3600f;

	public bool isEventActive { get; private set; }

	public static StarterPackController Get { get; private set; }

	private List<string> BuyWp8StarterPack { get; set; }

	public static event OnStarterPackEnableDelegate OnStarterPackEnable;

	private void Start()
	{
		Get = this;
		_timeLiveEvent = default(TimeSpan);
		_starterPacksData = new List<StarterPackData>();
		_orderCurrentPack = -1;
		_storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		BuyWp8StarterPack = new List<string>();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Destroy()
	{
		Get = null;
	}

	private void Update()
	{
		if (isEventActive && Time.realtimeSinceStartup - _lastCheckEventTime >= 1f)
		{
			_timeLiveEvent = DateTime.UtcNow - _timeStartEvent;
			isEventActive = _timeLiveEvent <= StarterPackModel.MaxLiveTimeEvent;
			if (!isEventActive)
			{
				FinishCurrentStarterPack();
			}
			_lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	private void FinishCurrentStarterPack()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty);
		Storager.setString("TimeEndStarterPack", DateTime.UtcNow.ToString("s"));
		Storager.setInt("NextNumberStarterPack", _orderCurrentPack + 1);
		isEventActive = false;
		CheckSendEventChangeEnabled();
	}

	private void CancelCurrentEvent()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty);
		Storager.setString("TimeEndStarterPack", string.Empty);
		Storager.setInt("NextNumberStarterPack", 0);
		PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		PlayerPrefs.SetInt("CountShownStarterPack", 1);
		PlayerPrefs.Save();
		isEventActive = false;
		CheckSendEventChangeEnabled();
	}

	private void ResetToDefaultStateIfNeed()
	{
		if (Storager.hasKey("StartTimeShowStarterPack") && !string.IsNullOrEmpty(Storager.getString("StartTimeShowStarterPack")))
		{
			Storager.setString("StartTimeShowStarterPack", string.Empty);
		}
		if (Storager.hasKey("TimeEndStarterPack") && !string.IsNullOrEmpty(Storager.getString("TimeEndStarterPack")))
		{
			Storager.setString("TimeEndStarterPack", string.Empty);
		}
		if (Storager.getInt("NextNumberStarterPack") > 0)
		{
			Storager.setInt("NextNumberStarterPack", 0);
		}
		if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty)))
		{
			PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		}
		if (PlayerPrefs.GetInt("CountShownStarterPack", 1) != 1)
		{
			PlayerPrefs.SetInt("CountShownStarterPack", 1);
		}
	}

	private void CheckCancelCurrentStarterPack()
	{
		ResetToDefaultStateIfNeed();
		if (isEventActive)
		{
			isEventActive = false;
			CheckSendEventChangeEnabled();
		}
	}

	public void CheckFindStoreKitEventListner()
	{
		if (!(_storeKitEventListener != null))
		{
			_storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		}
	}

	private IEnumerator DownloadDataAboutEvent()
	{
		if (_isDownloadDataRun)
		{
			yield break;
		}
		_isDownloadDataRun = true;
		string urlForDownloadEventData = StarterPackModel.GetUrlForDownloadEventData();
		WWW downloadData = Tools.CreateWwwIfNotConnected(urlForDownloadEventData);
		if (downloadData == null)
		{
			_isDownloadDataRun = false;
			yield break;
		}
		yield return downloadData;
		if (!string.IsNullOrEmpty(downloadData.error))
		{
			UnityEngine.Debug.LogFormat("DownloadDataAboutEvent error: {0}", downloadData.error);
			_starterPacksData.Clear();
			_isDownloadDataRun = false;
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(URLs.Sanitize(downloadData)) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.Log("DownloadDataAboutEvent eventData = null");
			_isDownloadDataRun = false;
			yield break;
		}
		_starterPacksData.Clear();
		if (!dictionary.ContainsKey("packs"))
		{
			_isDownloadDataRun = false;
			yield break;
		}
		List<object> list = dictionary["packs"] as List<object>;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				if (dictionary2 == null)
				{
					continue;
				}
				StarterPackData starterPackData = new StarterPackData();
				if (dictionary2.ContainsKey("blockLevel"))
				{
					starterPackData.blockLevel = Convert.ToInt32((long)dictionary2["blockLevel"]);
				}
				if (dictionary2.ContainsKey("coinsCost"))
				{
					starterPackData.coinsCost = Convert.ToInt32((long)dictionary2["coinsCost"]);
				}
				else if (dictionary2.ContainsKey("gemsCost"))
				{
					starterPackData.gemsCost = Convert.ToInt32((long)dictionary2["gemsCost"]);
				}
				if (dictionary2.ContainsKey("enable"))
				{
					int num = Convert.ToInt32((long)dictionary2["enable"]);
					starterPackData.enable = num == 1;
				}
				if (dictionary2.ContainsKey("items"))
				{
					List<object> list2 = dictionary2["items"] as List<object>;
					if (list2 != null)
					{
						starterPackData.items = new List<StarterPackItemData>();
						for (int j = 0; j < list2.Count; j++)
						{
							Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
							StarterPackItemData starterPackItemData = new StarterPackItemData();
							if (dictionary3 == null)
							{
								continue;
							}
							if (dictionary3.ContainsKey("tagsVariant"))
							{
								List<object> list3 = dictionary3["tagsVariant"] as List<object>;
								if (list3 != null)
								{
									for (int k = 0; k < list3.Count; k++)
									{
										starterPackItemData.variantTags.Add((string)list3[k]);
									}
								}
							}
							if (dictionary3.ContainsKey("count"))
							{
								starterPackItemData.count = Convert.ToInt32((long)dictionary3["count"]);
							}
							starterPackData.items.Add(starterPackItemData);
						}
					}
				}
				if (dictionary2.ContainsKey("sale"))
				{
					starterPackData.sale = Convert.ToInt32((long)dictionary2["sale"]);
				}
				if (dictionary2.ContainsKey("coinsCount"))
				{
					starterPackData.coinsCount = Convert.ToInt32((long)dictionary2["coinsCount"]);
				}
				if (dictionary2.ContainsKey("gemsCount"))
				{
					starterPackData.gemsCount = Convert.ToInt32((long)dictionary2["gemsCount"]);
				}
				_starterPacksData.Add(starterPackData);
			}
		}
		_isDownloadDataRun = false;
	}

	private bool IsPlayerNotPayBeforeStartEvent()
	{
		if (Storager.getInt("PayingUser") == 0)
		{
			return true;
		}
		if (isEventActive)
		{
			return true;
		}
		return false;
	}

	private int GetMaxValidOrderPack()
	{
		int result = -1;
		int currentLevel = ExperienceController.GetCurrentLevel();
		for (int i = 0; i < _starterPacksData.Count; i++)
		{
			if (_starterPacksData[i].blockLevel <= currentLevel)
			{
				result = i;
			}
		}
		return result;
	}

	private int GetOrderCurrentPack()
	{
		int @int = Storager.getInt("NextNumberStarterPack");
		if (@int >= _starterPacksData.Count)
		{
			return -1;
		}
		return @int;
	}

	private bool IsCurrentPackEnable()
	{
		StarterPackData currentPackData = GetCurrentPackData();
		if (currentPackData == null)
		{
			return true;
		}
		return currentPackData.enable;
	}

	private bool IsStarterPackBuyByPackOrder(int packOrder)
	{
		string storageIdByPackOrder = GetStorageIdByPackOrder(packOrder);
		if (string.IsNullOrEmpty(storageIdByPackOrder))
		{
			return false;
		}
		return IsStarterPackBuy(storageIdByPackOrder);
	}

	private bool IsInvalidCurrentPack()
	{
		return IsInvalidPack(_orderCurrentPack);
	}

	private bool IsInvalidPack(int packOrder)
	{
		if (GetPackType(packOrder) != 0)
		{
			return false;
		}
		List<StarterPackItemData> items = _starterPacksData[packOrder].items;
		for (int i = 0; i < items.Count; i++)
		{
			if (string.IsNullOrEmpty(items[i].validTag))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsEventInEndState()
	{
		string @string = Storager.getString("StartTimeShowStarterPack");
		string string2 = Storager.getString("TimeEndStarterPack");
		if (@string == string.Empty)
		{
			return !string.IsNullOrEmpty(string2);
		}
		return false;
	}

	private bool IsCooldownEventEnd()
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime timeDataEvent = StarterPackModel.GetTimeDataEvent("TimeEndStarterPack");
		return utcNow - timeDataEvent >= StarterPackModel.CooldownTimeEvent;
	}

	private IEnumerator CheckStartStarterPackEvent()
	{
		if (!TrainingController.TrainingCompleted)
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (!IsPlayerNotPayBeforeStartEvent() || ExpController.LobbyLevel < 3)
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (Time.time - timeUpdateConfig > 3600f)
		{
			yield return StartCoroutine(DownloadDataAboutEvent());
			timeUpdateConfig = Time.time;
		}
		if (IsEventInEndState() && !IsCooldownEventEnd())
		{
			yield break;
		}
		_orderCurrentPack = GetOrderCurrentPack();
		if (_orderCurrentPack == -1)
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		int maxValidOrderPack = GetMaxValidOrderPack();
		if (maxValidOrderPack == -1)
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (maxValidOrderPack > _orderCurrentPack)
		{
			_orderCurrentPack = maxValidOrderPack;
			Storager.setInt("NextNumberStarterPack", _orderCurrentPack);
			Storager.setString("StartTimeShowStarterPack", string.Empty);
		}
		else if (maxValidOrderPack < _orderCurrentPack)
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (!IsCurrentPackEnable())
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (IsStarterPackBuyOnWp8(_orderCurrentPack))
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (IsStarterPackBuyByPackOrder(_orderCurrentPack))
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (IsInvalidCurrentPack())
		{
			CheckCancelCurrentStarterPack();
			yield break;
		}
		if (string.IsNullOrEmpty(Storager.getString("StartTimeShowStarterPack")))
		{
			InitializeEvent();
		}
		else
		{
			_timeStartEvent = StarterPackModel.GetTimeDataEvent("StartTimeShowStarterPack");
		}
		bool num = isEventActive;
		isEventActive = true;
		if (num != isEventActive)
		{
			CheckSendEventChangeEnabled();
		}
	}

	public void CheckShowStarterPack()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			UnityEngine.Debug.Log("Skipping CheckShowStarterPack() on WSA.");
		}
		else
		{
			StartCoroutine(CheckStartStarterPackEvent());
		}
	}

	private void InitializeEvent()
	{
		_timeStartEvent = DateTime.UtcNow;
		Storager.setString("StartTimeShowStarterPack", _timeStartEvent.ToString("s"));
		Storager.setString("TimeEndStarterPack", string.Empty);
	}

	public StarterPackData GetCurrentPackData()
	{
		if (_orderCurrentPack == -1)
		{
			return null;
		}
		return _starterPacksData[_orderCurrentPack];
	}

	public StarterPackModel.TypePack GetPackType(int packOrder)
	{
		if (packOrder == -1)
		{
			return StarterPackModel.TypePack.None;
		}
		if (_starterPacksData[packOrder].items != null)
		{
			return StarterPackModel.TypePack.Items;
		}
		if (_starterPacksData[packOrder].coinsCount > 0)
		{
			return StarterPackModel.TypePack.Coins;
		}
		if (_starterPacksData[packOrder].gemsCount > 0)
		{
			return StarterPackModel.TypePack.Gems;
		}
		return StarterPackModel.TypePack.None;
	}

	public StarterPackModel.TypePack GetCurrentPackType()
	{
		return GetPackType(_orderCurrentPack);
	}

	public bool TryTakePurchasesForCurrentPack(string productId, bool isRestore = false)
	{
		return TryTakePurchases(productId, _orderCurrentPack, isRestore);
	}

	private bool IsStarterPackBuy(string storageId)
	{
		if (Storager.hasKey(storageId))
		{
			return Storager.getInt(storageId) == 1;
		}
		return false;
	}

	private bool TryTakePurchases(string productId, int packOrder, bool isRestore = false)
	{
		if (_starterPacksData.Count == 0)
		{
			return false;
		}
		if (packOrder == -1)
		{
			return false;
		}
		StarterPackModel.TypePack packType = GetPackType(packOrder);
		if (packType == StarterPackModel.TypePack.None)
		{
			return false;
		}
		if (IsStarterPackBuy(productId))
		{
			return false;
		}
		StarterPackData starterPackData = _starterPacksData[packOrder];
		switch (packType)
		{
		case StarterPackModel.TypePack.Coins:
			BankController.AddCoins(starterPackData.coinsCount, true, AnalyticsConstants.AccrualType.Purchased);
			break;
		case StarterPackModel.TypePack.Gems:
			BankController.AddGems(starterPackData.gemsCount, true, AnalyticsConstants.AccrualType.Purchased);
			break;
		case StarterPackModel.TypePack.Items:
		{
			if (starterPackData.items.Count == 0)
			{
				break;
			}
			for (int i = 0; i < starterPackData.items.Count; i++)
			{
				string validTag = starterPackData.items[i].validTag;
				int itemCategory = ItemDb.GetItemCategory(validTag);
				int count = starterPackData.items[i].count;
				if (itemCategory == 7 || ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemCategory))
				{
					ShopNGUIController.FireWeaponOrArmorBought();
				}
				ShopNGUIController.ProvideItem((ShopNGUIController.CategoryNames)itemCategory, validTag, count);
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
			{
				ShopNGUIController.sharedShop.wearEquipAction(ShopNGUIController.CategoryNames.ArmorCategory, "", "");
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory);
			}
			break;
		}
		}
		Storager.setInt(productId, 1);
		FinishCurrentStarterPack();
		if (!isRestore)
		{
			AnalyticsStuff.LogSales(StoreKitEventListener.inAppsReadableNames.ContainsKey(productId) ? StoreKitEventListener.inAppsReadableNames[productId] : productId, "Starter Pack");
		}
		return true;
	}

	public void CheckSendEventChangeEnabled()
	{
		if (StarterPackController.OnStarterPackEnable != null)
		{
			StarterPackController.OnStarterPackEnable(isEventActive);
		}
	}

	public ItemPrice GetPriceDataForItemsPack()
	{
		StarterPackData currentPackData = GetCurrentPackData();
		if (currentPackData == null)
		{
			return null;
		}
		if (currentPackData.coinsCost <= 0 && currentPackData.gemsCost <= 0)
		{
			return null;
		}
		string currency = string.Empty;
		int price = 0;
		if (currentPackData.coinsCost > 0)
		{
			currency = "Coins";
			price = currentPackData.coinsCost;
		}
		else if (currentPackData.gemsCost > 0)
		{
			currency = "GemsCurrency";
			price = currentPackData.gemsCost;
		}
		return new ItemPrice(price, currency);
	}

	public bool IsPackSellForGameMoney()
	{
		StarterPackModel.TypeCost typeCostPack = GetTypeCostPack();
		if (typeCostPack != StarterPackModel.TypeCost.Gems)
		{
			return typeCostPack == StarterPackModel.TypeCost.Money;
		}
		return true;
	}

	public void CheckBuyPackForGameMoney(StarterPackView view)
	{
		ItemPrice priceDataForItemsPack = GetPriceDataForItemsPack();
		if (priceDataForItemsPack != null)
		{
			ShopNGUIController.TryToBuy(view.gameObject, priceDataForItemsPack, delegate
			{
				string storageIdByPackOrder = GetStorageIdByPackOrder(_orderCurrentPack);
				TryTakePurchasesForCurrentPack(storageIdByPackOrder);
				view.HideWindow();
			});
		}
	}

	private StarterPackModel.TypeCost GetTypeCostPack()
	{
		StarterPackData currentPackData = GetCurrentPackData();
		if (currentPackData == null)
		{
			return StarterPackModel.TypeCost.None;
		}
		if (currentPackData.coinsCost > 0)
		{
			return StarterPackModel.TypeCost.Money;
		}
		if (currentPackData.gemsCost > 0)
		{
			return StarterPackModel.TypeCost.Gems;
		}
		return StarterPackModel.TypeCost.InApp;
	}

	public void CheckBuyRealMoney()
	{
		if (_orderCurrentPack >= StoreKitEventListener.starterPackIds.Length)
		{
			UnityEngine.Debug.Log("Not purchase data for starter pack number: " + _orderCurrentPack);
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		StoreKitEventListener.purchaseInProcess = true;
		string productId = StoreKitEventListener.starterPackIds[_orderCurrentPack];
	}

	private int GetOrderPackByProductId(string productId)
	{
		if (!StoreKitEventListener.starterPackIds.Contains(productId))
		{
			return -1;
		}
		return Array.IndexOf(StoreKitEventListener.starterPackIds, productId);
	}

	private string GetStorageIdByPackOrder(int packOrder)
	{
		if (packOrder < 0 || packOrder >= StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		return StoreKitEventListener.starterPackIds[packOrder];
	}

	private IEnumerator TryRestoreStarterPackByProductId(string productId)
	{
		if (!(Application.loadedLevelName == "Loading") && StoreKitEventListener.starterPackIds.Contains(productId) && !IsStarterPackBuy(productId))
		{
			if (_starterPacksData.Count == 0)
			{
				yield return StartCoroutine(DownloadDataAboutEvent());
			}
			int orderPackByProductId = GetOrderPackByProductId(productId);
			if (!IsInvalidPack(orderPackByProductId) && GetPackType(orderPackByProductId) == StarterPackModel.TypePack.Items && TryTakePurchases(productId, orderPackByProductId, true))
			{
				FyberFacade.Instance.SetUserPaying("1");
				StoreKitEventListener.SetLastPaymentTime();
			}
		}
	}

	public void TryRestoreStarterPack(string productId)
	{
		StartCoroutine(TryRestoreStarterPackByProductId(productId));
	}

	public string GetTimeToEndEvent()
	{
		if (!isEventActive)
		{
			return string.Empty;
		}
		_timeToEndEvent = StarterPackModel.MaxLiveTimeEvent - _timeLiveEvent;
		return string.Format("{0:00}:{1:00}:{2:00}", new object[3] { _timeToEndEvent.Hours, _timeToEndEvent.Minutes, _timeToEndEvent.Seconds });
	}

	public string GetPriceLabelForCurrentPack()
	{
		if (_orderCurrentPack >= StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		if (Application.isEditor)
		{
			return string.Format("{0}$", new object[1] { VirtualCurrencyHelper.starterPackFakePrice[_orderCurrentPack] });
		}
		string productId = StoreKitEventListener.starterPackIds[_orderCurrentPack];
		UnityEngine.Debug.LogWarning("marketProduct == null,    id: " + productId);
		return string.Empty;
	}

	public string GetCurrentPackName()
	{
		if (_orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (_orderCurrentPack >= StarterPackModel.packNameLocalizeKey.Length)
		{
			return string.Empty;
		}
		return LocalizationStore.Get(StarterPackModel.packNameLocalizeKey[_orderCurrentPack]);
	}

	public Texture2D GetCurrentPackImage()
	{
		StarterPackModel.TypePack currentPackType = GetCurrentPackType();
		string text = string.Empty;
		switch (currentPackType)
		{
		case StarterPackModel.TypePack.Coins:
			text = "Textures/Bank/Coins_Shop_5";
			break;
		case StarterPackModel.TypePack.Gems:
			text = "Textures/Bank/Coins_Shop_Gem_5";
			break;
		case StarterPackModel.TypePack.Items:
			text = "Textures/Bank/StarterPack_Weapon";
			break;
		}
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return Resources.Load<Texture2D>(text);
	}

	public void UpdateCountShownWindowByShowCondition()
	{
		if (PlayerPrefs.GetInt("CountShownStarterPack", 1) != 0)
		{
			PlayerPrefs.SetString("LastTimeShowStarterPack", DateTime.UtcNow.ToString("s"));
			int @int = PlayerPrefs.GetInt("CountShownStarterPack", 1);
			PlayerPrefs.SetInt("CountShownStarterPack", @int - 1);
			PlayerPrefs.Save();
		}
	}

	public bool IsNeedShowEventWindow()
	{
		int @int = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		if (isEventActive && @int > 0)
		{
			return SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene);
		}
		return false;
	}

	public void UpdateCountShownWindowByTimeCondition()
	{
		string @string = PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			DateTime result = default(DateTime);
			if (DateTime.TryParse(@string, out result) && DateTime.UtcNow - result >= StarterPackModel.TimeOutShownWindow)
			{
				PlayerPrefs.SetInt("CountShownStarterPack", 1);
			}
		}
	}

	public string GetSavingMoneyByCarrentPack()
	{
		int orderCurrentPack = GetOrderCurrentPack();
		if (orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (orderCurrentPack >= StarterPackModel.savingMoneyForBuyPack.Length)
		{
			return string.Empty;
		}
		if (IsPackSellForGameMoney())
		{
			return string.Empty;
		}
		return string.Format("{0} {1}$", new object[2]
		{
			LocalizationStore.Get("Key_1047"),
			StarterPackModel.savingMoneyForBuyPack[orderCurrentPack]
		});
	}

	public void AddBuyingStarterPack(List<string> buyingList, string starterPackId)
	{
		if (!buyingList.Contains(starterPackId))
		{
			buyingList.Add(starterPackId);
		}
	}

	public void RestoreBuyingStarterPack(List<string> buyingList)
	{
		for (int i = 0; i < buyingList.Count; i++)
		{
			string productId = buyingList[i];
			TryRestoreStarterPack(productId);
		}
	}

	private bool IsStarterPackBuying(List<string> buyingList, int orderPack)
	{
		string storageIdByPackOrder = GetStorageIdByPackOrder(orderPack);
		if (string.IsNullOrEmpty(storageIdByPackOrder))
		{
			return false;
		}
		return buyingList.Contains(storageIdByPackOrder);
	}

	public void AddBuyWp8StarterPack(string starterPackId)
	{
		AddBuyingStarterPack(BuyWp8StarterPack, starterPackId);
	}

	public void RestoreStarterPackForWp8()
	{
		RestoreBuyingStarterPack(BuyWp8StarterPack);
	}

	private bool IsStarterPackBuyOnWp8(int orderPack)
	{
		return IsStarterPackBuying(BuyWp8StarterPack, orderPack);
	}

	internal bool IsNeedResoreStarterPackWp8()
	{
		return BuyWp8StarterPack.Count > 0;
	}

	private bool IsProductNeedConsume(string productId)
	{
		if (productId == StoreKitEventListener.starterPackIds[1])
		{
			return true;
		}
		if (productId == StoreKitEventListener.starterPackIds[3])
		{
			return true;
		}
		if (productId == StoreKitEventListener.starterPackIds[5])
		{
			return true;
		}
		return false;
	}
}
