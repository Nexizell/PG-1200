using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using UnityEngine;

public class InRoomTime : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetRoomStartTimestamp_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InRoomTime _003C_003E4__this;

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
		public _003CSetRoomStartTimestamp_003Ed__8(int _003C_003E1__state)
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
				if (_003C_003E4__this.IsRoomTimeSet || !PhotonNetwork.isMasterClient)
				{
					return false;
				}
				if (PhotonNetwork.ServerTimestamp == 0)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable["#rt"] = PhotonNetwork.ServerTimestamp;
			PhotonNetwork.room.SetCustomProperties(hashtable);
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

	private int roomStartTimestamp;

	private const string StartTimeKey = "#rt";

	public double RoomTime
	{
		get
		{
			return (double)(uint)RoomTimestamp / 1000.0;
		}
	}

	public int RoomTimestamp
	{
		get
		{
			if (!PhotonNetwork.inRoom)
			{
				return 0;
			}
			return PhotonNetwork.ServerTimestamp - roomStartTimestamp;
		}
	}

	public bool IsRoomTimeSet
	{
		get
		{
			if (PhotonNetwork.inRoom)
			{
				return PhotonNetwork.room.customProperties.ContainsKey("#rt");
			}
			return false;
		}
	}

	internal IEnumerator SetRoomStartTimestamp()
	{
		if (!IsRoomTimeSet && PhotonNetwork.isMasterClient)
		{
			if (PhotonNetwork.ServerTimestamp == 0)
			{
				yield return 0;
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable["#rt"] = PhotonNetwork.ServerTimestamp;
			PhotonNetwork.room.SetCustomProperties(hashtable);
		}
	}

	public void OnJoinedRoom()
	{
		StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("#rt"))
		{
			roomStartTimestamp = (int)propertiesThatChanged["#rt"];
		}
	}
}
