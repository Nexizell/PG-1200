using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class PremiumAccountController : MonoBehaviour
{
	public delegate void OnAccountChangedDelegate();

	public enum AccountType
	{
		OneDay = 0,
		ThreeDay = 1,
		SevenDays = 2,
		Month = 3,
		None = 4
	}

	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pause;

		public PremiumAccountController _003C_003E4__this;

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
		public _003COnApplicationPause_003Ed__29(int _003C_003E1__state)
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
				if (pause)
				{
					_003C_003E4__this.UpdateLastLoggedTime();
					break;
				}
				_003C_003E4__this.CheckTimeHack();
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
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadPremInfo());
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
	internal sealed class _003CGetPremInfoLoop_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PremiumAccountController _003C_003E4__this;

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
		public _003CGetPremInfoLoop_003Ed__53(int _003C_003E1__state)
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
				if (Time.realtimeSinceStartup - _003C_003E4__this._premGetInfoStartTime < 1200f)
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
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadPremInfo());
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
	internal sealed class _003CDownloadPremInfo_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PremiumAccountController _003C_003E4__this;

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
		public _003CDownloadPremInfo_003Ed__54(int _003C_003E1__state)
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
				if (_003C_003E4__this._isGetPremInfoRunning)
				{
					return false;
				}
				_003C_003E4__this._premGetInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetPremInfoRunning = true;
				if (string.IsNullOrEmpty(URLs.PremiumAccount))
				{
					_003C_003E4__this._isGetPremInfoRunning = false;
					return false;
				}
				_003Cresponse_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.PremiumAccount);
				if (_003Cresponse_003E5__1 == null)
				{
					_003C_003E4__this._isGetPremInfoRunning = false;
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
					UnityEngine.Debug.LogWarningFormat("Premium Account response error: {0}", _003Cresponse_003E5__1.error);
					_003C_003E4__this._isGetPremInfoRunning = false;
					return false;
				}
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Debug.LogWarning("Prem response is empty");
					_003C_003E4__this._isGetPremInfoRunning = false;
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				if (dictionary == null)
				{
					UnityEngine.Debug.LogWarning("Prem response is bad");
					_003C_003E4__this._isGetPremInfoRunning = false;
					return false;
				}
				if (dictionary.ContainsKey("enable"))
				{
					long num = (long)dictionary["enable"];
					Storager.setInt(Defs.PremiumEnabledFromServer, (num == 1) ? 1 : 0);
				}
				_003C_003E4__this._isGetPremInfoRunning = false;
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

	private DateTime _timeStart;

	private DateTime _timeEnd;

	private TimeSpan _timeToEndAccount;

	private float _lastCheckTime;

	private int _additionalAccountDays;

	private long _lastLoggedAccountTime;

	private int _countCeilDays;

	private const float PremInfoTimeout = 1200f;

	private bool _isGetPremInfoRunning;

	private float _premGetInfoStartTime;

	public static PremiumAccountController Instance { get; private set; }

	public bool isAccountActive { get; private set; }

	public static bool AccountHasExpired { get; set; }

	public int RewardCoeff
	{
		get
		{
			if (!isAccountActive)
			{
				return 1;
			}
			return 2;
		}
	}

	public static float VirtualCurrencyMultiplier
	{
		get
		{
			if (Instance == null)
			{
				UnityEngine.Debug.LogError("VirtualCurrencyMultiplier Instance == null");
				return 1f;
			}
			switch (Instance.GetCurrentAccount())
			{
			case AccountType.SevenDays:
				return 1.05f;
			case AccountType.Month:
				return 1.1f;
			default:
				return 1f;
			}
		}
	}

	public static event OnAccountChangedDelegate OnAccountChanged;

	public static bool MapAvailableDueToPremiumAccount(string mapName)
	{
		if (mapName == null || Instance == null)
		{
			return false;
		}
		if (Defs.PremiumMaps != null && Defs.PremiumMaps.ContainsKey(mapName))
		{
			return Instance.isAccountActive;
		}
		return false;
	}

	private void Start()
	{
		Instance = this;
		_timeToEndAccount = default(TimeSpan);
		_additionalAccountDays = GetAllTimeOtherAccountFromHistory();
		isAccountActive = CheckInitializeCurrentAccount();
		CheckTimeHack();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		StartCoroutine(GetPremInfoLoop());
	}

	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			UpdateLastLoggedTime();
			yield break;
		}
		CheckTimeHack();
		yield return null;
		yield return null;
		yield return null;
		StartCoroutine(DownloadPremInfo());
	}

	private void Destroy()
	{
		UpdateLastLoggedTime();
		Instance = null;
	}

	private void CheckTimeHack()
	{
		_lastLoggedAccountTime = GetLastLoggedTime();
		if (_lastLoggedAccountTime != 0L && PromoActionsManager.CurrentUnixTime < _lastLoggedAccountTime)
		{
			StopAccountsWork();
		}
	}

	private long GetLastLoggedTime()
	{
		if (!isAccountActive)
		{
			return 0L;
		}
		if (!Storager.hasKey("LastLoggedTimePremiumAccount"))
		{
			return 0L;
		}
		long result;
		long.TryParse(Storager.getString("LastLoggedTimePremiumAccount"), out result);
		return result;
	}

	private void UpdateLastLoggedTime()
	{
		if (isAccountActive)
		{
			Storager.setString("LastLoggedTimePremiumAccount", PromoActionsManager.CurrentUnixTime.ToString());
		}
	}

	private bool CheckInitializeCurrentAccount()
	{
		DateTime parsedDate = default(DateTime);
		bool num = Tools.ParseDateTimeFromPlayerPrefs("StartTimePremiumAccount", out parsedDate);
		DateTime parsedDate2 = default(DateTime);
		bool flag = Tools.ParseDateTimeFromPlayerPrefs("EndTimePremiumAccount", out parsedDate2);
		if (!num || !flag)
		{
			return false;
		}
		_timeStart = parsedDate;
		_timeEnd = parsedDate2;
		return true;
	}

	private void Update()
	{
		if (isAccountActive && Time.realtimeSinceStartup - _lastCheckTime >= 1f)
		{
			_timeToEndAccount = _timeEnd - DateTime.UtcNow;
			isAccountActive = DateTime.UtcNow <= _timeEnd;
			if (!isAccountActive)
			{
				ChangeCurrentAccount();
			}
			_lastCheckTime = Time.realtimeSinceStartup;
		}
	}

	private void ChangeCurrentAccount()
	{
		if (!ChangeAccountOnNext())
		{
			StopAccountsWork();
		}
	}

	private DateTime GetTimeEndAccount(DateTime startTime, AccountType accountType)
	{
		DateTime result = startTime;
		switch (accountType)
		{
		case AccountType.OneDay:
			result = result.AddDays(1.0);
			break;
		case AccountType.ThreeDay:
			result = result.AddDays(3.0);
			break;
		case AccountType.SevenDays:
			result = result.AddDays(7.0);
			break;
		case AccountType.Month:
			result = result.AddDays(30.0);
			break;
		}
		return result;
	}

	public void BuyAccount(AccountType accountType)
	{
		if (GetCurrentAccount() == AccountType.None)
		{
			StartNewAccount(accountType);
		}
		AddBoughtAccountInHistory(accountType);
		_additionalAccountDays = GetAllTimeOtherAccountFromHistory();
		AccountHasExpired = false;
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	private void StartNewAccount(AccountType accountType)
	{
		isAccountActive = true;
		_timeStart = DateTime.UtcNow;
		Storager.setString("StartTimePremiumAccount", _timeStart.ToString("s"));
		_timeEnd = GetTimeEndAccount(_timeStart, accountType);
		Storager.setString("EndTimePremiumAccount", _timeEnd.ToString("s"));
	}

	private void AddBoughtAccountInHistory(AccountType accountType)
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount");
		@string = ((!string.IsNullOrEmpty(@string)) ? (@string + string.Format(",{0}", new object[1] { (int)accountType })) : string.Format("{0}", new object[1] { (int)accountType }));
		Storager.setString("BuyHistoryPremiumAccount", @string);
	}

	private void DeleteBoughtAccountFromHistory()
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount");
		if (!string.IsNullOrEmpty(@string))
		{
			int num = @string.IndexOf(',');
			@string = ((num <= 0) ? string.Empty : @string.Remove(0, num + 1));
			Storager.setString("BuyHistoryPremiumAccount", @string);
		}
	}

	public AccountType GetCurrentAccount()
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount");
		if (string.IsNullOrEmpty(@string))
		{
			return AccountType.None;
		}
		string[] array = @string.Split(',');
		if (array.Length == 0)
		{
			return AccountType.None;
		}
		int result = 0;
		if (!int.TryParse(array[0], out result))
		{
			return AccountType.None;
		}
		return (AccountType)result;
	}

	private bool ChangeAccountOnNext()
	{
		DeleteBoughtAccountFromHistory();
		_additionalAccountDays = GetAllTimeOtherAccountFromHistory();
		AccountType currentAccount = GetCurrentAccount();
		if (currentAccount == AccountType.None)
		{
			return false;
		}
		StartNewAccount(currentAccount);
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return true;
	}

	private void StopAccountsWork()
	{
		isAccountActive = false;
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		Storager.setString("StartTimePremiumAccount", string.Empty);
		Storager.setString("EndTimePremiumAccount", string.Empty);
		Storager.setString("BuyHistoryPremiumAccount", string.Empty);
		_timeToEndAccount = TimeSpan.FromMinutes(0.0);
		_additionalAccountDays = 0;
		_countCeilDays = 0;
		AccountHasExpired = true;
	}

	private int GetDaysAccountByType(int codeAccount)
	{
		switch ((AccountType)codeAccount)
		{
		case AccountType.OneDay:
			return 1;
		case AccountType.ThreeDay:
			return 3;
		case AccountType.SevenDays:
			return 7;
		case AccountType.Month:
			return 30;
		default:
			return 0;
		}
	}

	private int GetAllTimeOtherAccountFromHistory()
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount");
		if (string.IsNullOrEmpty(@string))
		{
			return 0;
		}
		string[] array = @string.Split(',');
		if (array.Length == 0)
		{
			return 0;
		}
		int result = 0;
		int num = 0;
		for (int i = 1; i < array.Length; i++)
		{
			int.TryParse(array[i], out result);
			num += GetDaysAccountByType(result);
		}
		return num;
	}

	public string GetTimeToEndAllAccounts()
	{
		if (!isAccountActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = _timeToEndAccount.Add(TimeSpan.FromDays(_additionalAccountDays));
		if (timeSpan.Days > 0)
		{
			string text = "Days";
			_countCeilDays = Mathf.CeilToInt((float)_timeToEndAccount.TotalDays) + _additionalAccountDays;
			return string.Format("{0}: {1}", new object[2] { text, _countCeilDays });
		}
		return string.Format("{0:00}:{1:00}:{2:00}", new object[3] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
	}

	public int GetDaysToEndAllAccounts()
	{
		return _countCeilDays + _additionalAccountDays;
	}

	private IEnumerator GetPremInfoLoop()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(DownloadPremInfo());
			while (Time.realtimeSinceStartup - _premGetInfoStartTime < 1200f)
			{
				yield return null;
			}
		}
	}

	private IEnumerator DownloadPremInfo()
	{
		if (_isGetPremInfoRunning)
		{
			yield break;
		}
		_premGetInfoStartTime = Time.realtimeSinceStartup;
		_isGetPremInfoRunning = true;
		if (string.IsNullOrEmpty(URLs.PremiumAccount))
		{
			_isGetPremInfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.PremiumAccount);
		if (response == null)
		{
			_isGetPremInfoRunning = false;
			yield break;
		}
		yield return response;
		string text = URLs.Sanitize(response);
		if (!string.IsNullOrEmpty(response.error))
		{
			UnityEngine.Debug.LogWarningFormat("Premium Account response error: {0}", response.error);
			_isGetPremInfoRunning = false;
			yield break;
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("Prem response is empty");
			_isGetPremInfoRunning = false;
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("Prem response is bad");
			_isGetPremInfoRunning = false;
			yield break;
		}
		if (dictionary.ContainsKey("enable"))
		{
			long num = (long)dictionary["enable"];
			Storager.setInt(Defs.PremiumEnabledFromServer, (num == 1) ? 1 : 0);
		}
		_isGetPremInfoRunning = false;
	}

	public bool IsActiveOrWasActiveBeforeStartMatch()
	{
		if (isAccountActive)
		{
			return true;
		}
		Player_move_c player_move_c = ((WeaponManager.sharedManager == null) ? null : WeaponManager.sharedManager.myPlayerMoveC);
		if (player_move_c == null)
		{
			return false;
		}
		return player_move_c.isNeedTakePremiumAccountRewards;
	}

	public int GetRewardCoeffByActiveOrActiveBeforeMatch()
	{
		if (!IsActiveOrWasActiveBeforeStartMatch())
		{
			return 1;
		}
		return 2;
	}
}
