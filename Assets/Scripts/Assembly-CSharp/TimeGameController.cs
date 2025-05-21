using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public class TimeGameController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CFetchServerTimestamp_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003CFetchServerTimestamp_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
			}
			PhotonNetwork.FetchServerTimestamp();
			_003C_003E2__current = new WaitForRealSeconds(60f);
			_003C_003E1__state = 1;
			return true;
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
	internal sealed class _003COnUnpause_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003COnUnpause_003Ed__22(int _003C_003E1__state)
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
				PhotonNetwork.isMessageQueueRunning = true;
				return false;
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

	public static TimeGameController sharedController;

	public double timeEndMatch;

	public double timerToEndMatch;

	public double networkTime;

	public PhotonView photonView;

	public double timeLocalServer;

	public string ipServera;

	private long pauseTime;

	private bool paused;

	private bool wasPaused;

	public bool isEndMatch = true;

	private bool matchEnding;

	private int matchEndingPos;

	private int writtedMatchEndingPos;

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!PhotonNetwork.connected)
		{
			return;
		}
		if (pauseStatus)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !GameConnect.isDuel)
			{
				paused = true;
				wasPaused = true;
				PhotonNetwork.isMessageQueueRunning = false;
			}
			else
			{
				GameConnect.Disconnect();
			}
		}
		else
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !GameConnect.isDuel)
			{
				CheckPause();
			}
			PhotonNetwork.FetchServerTimestamp();
		}
	}

	private void Awake()
	{
		if (!Defs.isMulti || GameConnect.isMiniGame || GameConnect.isDuel)
		{
			base.enabled = false;
		}
		else
		{
			StartCoroutine(FetchServerTimestamp());
		}
	}

	private IEnumerator FetchServerTimestamp()
	{
		while (true)
		{
			PhotonNetwork.FetchServerTimestamp();
			yield return new WaitForRealSeconds(60f);
		}
	}

	private void Start()
	{
		sharedController = this;
	}

	public void SinchServerTimeInvoke()
	{
	}

	public void StartMatch()
	{
		bool flag = false;
		matchEnding = false;
		if (CapturePointController.sharedController != null)
		{
			CapturePointController.sharedController.isEndMatch = false;
		}
		if ((GameConnect.isCapturePoints || GameConnect.isFlag) && Convert.ToDouble(PhotonNetwork.room.customProperties[GameConnect.timeProperty]) < -5000000.0)
		{
			flag = true;
		}
		if (Defs.isInet && ((timeEndMatch < PhotonNetwork.time && !GameConnect.isFlag) || Initializer.players.Count == 0 || (GameConnect.isFlag && flag)))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			double num = PhotonNetwork.time + (double)((GameConnect.isCOOP ? 4 : ((int)PhotonNetwork.room.customProperties[GameConnect.maxKillProperty])) * 60);
			if (num > 4294967.0 && PhotonNetwork.time < 4294967.0)
			{
				num = 4294967.0;
			}
			hashtable[GameConnect.timeProperty] = num;
			hashtable[GameConnect.endingProperty] = 0;
			PhotonNetwork.room.SetCustomProperties(hashtable);
			matchEndingPos = 0;
			timerToEndMatch = num - PhotonNetwork.time;
		}
		if (!Defs.isInet && (timeEndMatch < networkTime || Initializer.players.Count == 0))
		{
			timeEndMatch = networkTime + (double)(((!PlayerPrefs.GetString("MaxKill", "9").Equals("")) ? int.Parse(PlayerPrefs.GetString("MaxKill", "5")) : 5) * 60);
		}
	}

	private void CheckPause()
	{
		paused = false;
		long currentUnixTime = Tools.CurrentUnixTime;
		if (pauseTime > currentUnixTime || pauseTime + 60 < currentUnixTime)
		{
			GameConnect.Disconnect();
		}
	}

	private void Update()
	{
		if (paused && Defs.isInet && Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CheckPause();
			if (!PhotonNetwork.connected)
			{
				return;
			}
		}
		ipServera = PhotonNetwork.ServerAddress;
		if (Defs.isInet && PhotonNetwork.room != null && PhotonNetwork.room.customProperties[GameConnect.timeProperty] != null)
		{
			double num = networkTime - PhotonNetwork.time;
			if (WeaponManager.sharedManager.myPlayerMoveC != null && num > 3.5 && num < 600.0)
			{
				UnityEngine.Debug.LogError("Speedhack detected! Delta: " + num + ", Photon time: " + PhotonNetwork.time + ", Last time: " + networkTime);
				GameConnect.Disconnect();
			}
			networkTime = PhotonNetwork.time;
			if (networkTime < 0.1)
			{
				return;
			}
			timeEndMatch = Convert.ToDouble(PhotonNetwork.room.customProperties[GameConnect.timeProperty]);
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && timeEndMatch > PhotonNetwork.time + 1500.0)
			{
				Initializer.Instance.goToConnect();
			}
			if (PhotonNetwork.room.customProperties.ContainsKey(GameConnect.endingProperty))
			{
				matchEndingPos = (int)PhotonNetwork.room.customProperties[GameConnect.endingProperty];
			}
			writtedMatchEndingPos = matchEndingPos;
			switch (matchEndingPos)
			{
			case 0:
				if (timeEndMatch < PhotonNetwork.time + (double)(PhotonNetwork.isMasterClient ? 130 : 110))
				{
					matchEndingPos = 2;
					UnityEngine.Debug.Log("two minutes remain");
				}
				break;
			case 2:
				if (timeEndMatch < PhotonNetwork.time + (double)(PhotonNetwork.isMasterClient ? 70 : 50))
				{
					UnityEngine.Debug.Log("one minute remain");
					matchEndingPos = 1;
				}
				break;
			}
			if (writtedMatchEndingPos != matchEndingPos)
			{
				UnityEngine.Debug.Log("Write ending: " + matchEndingPos);
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable[GameConnect.endingProperty] = matchEndingPos;
				PhotonNetwork.room.SetCustomProperties(hashtable);
			}
			if (timeEndMatch > 4290000.0 && networkTime < 2000000.0)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				double num2 = networkTime + 60.0;
				hashtable2[GameConnect.timeProperty] = num2;
				PhotonNetwork.room.SetCustomProperties(hashtable2);
			}
			if (timeEndMatch > 0.0)
			{
				timerToEndMatch = timeEndMatch - networkTime;
			}
			else
			{
				timerToEndMatch = -1.0;
			}
		}
		bool isInet = Defs.isInet;
		if (timerToEndMatch < 0.0 && !GameConnect.isFlag)
		{
			if (GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
			{
				if (CapturePointController.sharedController != null && !isEndMatch)
				{
					CapturePointController.sharedController.EndMatch();
					isEndMatch = true;
				}
			}
			else if (GameConnect.gameMode == GameConnect.GameMode.TimeBattle)
			{
				if (!isEndMatch)
				{
					ZombiManager.sharedManager.EndMatch();
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.WinFromTimer();
			}
			else
			{
				GlobalGameController.countKillsRed = 0;
				GlobalGameController.countKillsBlue = 0;
			}
		}
		else
		{
			isEndMatch = false;
		}
		if (wasPaused)
		{
			wasPaused = false;
			StartCoroutine(OnUnpause());
		}
		pauseTime = Tools.CurrentUnixTime;
	}

	private IEnumerator OnUnpause()
	{
		yield return null;
		yield return null;
		PhotonNetwork.isMessageQueueRunning = true;
	}

	[RPC]
	[PunRPC]
	private void SynchTimeEnd(float synchTime)
	{
		timeEndMatch = synchTime;
	}

	[RPC]
	[PunRPC]
	private void SynchTimeServer(float synchTime)
	{
		if (networkTime < (double)synchTime)
		{
			networkTime = synchTime;
		}
	}

	private void OnDestroy()
	{
		sharedController = null;
	}
}
