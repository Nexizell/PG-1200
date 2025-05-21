using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FindFriendsFromLocalLAN : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pause;

		public FindFriendsFromLocalLAN _003C_003E4__this;

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
		public _003COnApplicationPause_003Ed__13(int _003C_003E1__state)
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
					_003C_003E4__this.StopBroadCasting();
					break;
				}
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
				_003C_003E4__this.StartBroadcastingSession();
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

	public static bool isFindLocalFriends = false;

	private string ipaddress;

	public static List<string> lanPlayerInfo = new List<string>();

	public static Action lanPlayerInfoUpdate = null;

	private float periodSendMyInfo = 30f;

	private float timeSendMyInfo;

	private bool isGetMessage;

	private bool isActiveFriends;

	private List<string> idsForInfo = new List<string>();

	private void Start()
	{
		StartBroadcastingSession();
	}

	private void StartBroadcastingSession()
	{
	}

	public void StopBroadCasting()
	{
	}

	private void BeginAsyncReceive()
	{
	}

	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			StopBroadCasting();
			yield break;
		}
		yield return null;
		yield return null;
		yield return null;
		StartBroadcastingSession();
	}

	private void GetAsyncReceive(IAsyncResult objResult)
	{
	}

	private void SendMyInfo()
	{
		string.IsNullOrEmpty(FriendsController.sharedController.id);
	}

	private void Update()
	{
		isActiveFriends = FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled;
		if (idsForInfo.Count > 0)
		{
			FriendsController.sharedController.GetInfoAboutPlayers(idsForInfo);
			idsForInfo.Clear();
		}
		if ((isActiveFriends || isGetMessage) && Time.time - timeSendMyInfo > periodSendMyInfo)
		{
			isGetMessage = false;
			SendMyInfo();
		}
	}
}
