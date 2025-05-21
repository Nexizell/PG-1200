using System;
using Rilisoft;
using UnityEngine;

public class ConnectionControl : MonoBehaviour
{
	public ConnectPanel connectionPanel;

	public bool cantCancel;

	private IDisposable _someWindowSubscription;

	private Action actAfterConnectToPhoton;

	private Action actAfterConnectToRoom;

	private void OnEnable()
	{
		SubscribeToNetworkEvents();
	}

	public void SetConnectPanel(ConnectPanel panel)
	{
		connectionPanel = panel;
		connectionPanel.cancelButton.Clicked += CancelBtn;
	}

	private void OnDisable()
	{
		UnsubscribeToNetworkEvents();
		UnsubscribeBackSystem();
	}

	private void SubscribeToNetworkEvents()
	{
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Combine(GameConnect.OnJoinRoomFailed, new Action<bool>(OnJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Combine(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnect));
		GameConnect.OnConnectedMaster = (Action)Delegate.Combine(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Combine(GameConnect.OnDisconnected, new Action(OnDisconnected));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Combine(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
	}

	private void UnsubscribeToNetworkEvents()
	{
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Remove(GameConnect.OnJoinRoomFailed, new Action<bool>(OnJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Remove(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnect));
		GameConnect.OnConnectedMaster = (Action)Delegate.Remove(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Remove(GameConnect.OnDisconnected, new Action(OnDisconnected));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Remove(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
	}

	public void CancelBtn(object sender, EventArgs e)
	{
		CancelBtn();
	}

	private void UnsubscribeBackSystem()
	{
		if (_someWindowSubscription != null)
		{
			_someWindowSubscription.Dispose();
			_someWindowSubscription = null;
		}
	}

	private void HidePanel()
	{
		UnsubscribeBackSystem();
		if (connectionPanel.failInternetLabel != null)
		{
			connectionPanel.failInternetLabel.SetActive(false);
		}
		if (connectionPanel.gameObject != null)
		{
			connectionPanel.gameObject.SetActive(false);
		}
		actAfterConnectToPhoton = null;
	}

	public void CancelBtn()
	{
		if (!cantCancel)
		{
			if (PhotonNetwork.connected || actAfterConnectToPhoton == null)
			{
				GameConnect.Disconnect();
			}
			HidePanel();
			GameConnect.SetGameMode(GameConnect.GameMode.Deathmatch);
		}
	}

	public void JoinRandomRoom(Action joinedRoomAction)
	{
		actAfterConnectToRoom = joinedRoomAction;
		connectionPanel.gameObject.SetActive(true);
		GameConnect.ConnectToRandomRoom();
	}

	public void ConnectToPhoton(Action actionAfterConnect)
	{
		cantCancel = false;
		_someWindowSubscription = BackSystem.Instance.Register(CancelBtn, "Connect to Photon panel");
		connectionPanel.gameObject.SetActive(true);
		actAfterConnectToPhoton = actionAfterConnect;
		ConnectToPhoton();
	}

	public void ConnectToPhoton()
	{
		if (!(FriendsController.sharedController != null) || FriendsController.sharedController.Banned != 1)
		{
			GameConnect.ConnectToPhoton(GameConnect.GetTierForRoom());
		}
	}

	private void OnDisconnected()
	{
		Debug.Log("OnDisconnectedFromPhoton");
		if (actAfterConnectToPhoton != null)
		{
			Invoke("ConnectToPhoton", 0.5f);
		}
		if (connectionPanel.gameObject.activeSelf)
		{
			connectionPanel.failInternetLabel.SetActive(true);
		}
	}

	private void OnFailedToConnect(int parameters)
	{
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + (DisconnectCause)parameters);
		if (connectionPanel.gameObject.activeSelf)
		{
			connectionPanel.failInternetLabel.SetActive(true);
		}
		Invoke("ConnectToPhoton", 1f);
	}

	public void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		if (connectionPanel.gameObject.activeSelf)
		{
			connectionPanel.gameObject.SetActive(false);
		}
		if (actAfterConnectToPhoton != null)
		{
			actAfterConnectToPhoton();
			actAfterConnectToPhoton = null;
		}
		else
		{
			GameConnect.Disconnect();
		}
	}

	private void OnJoinedRoom()
	{
		cantCancel = true;
		UnsubscribeBackSystem();
		actAfterConnectToPhoton = null;
		if (actAfterConnectToRoom != null)
		{
			actAfterConnectToRoom();
			actAfterConnectToRoom = null;
		}
		else
		{
			Debug.LogError("No action on join room!");
		}
	}

	private void OnJoinRoomFailed(bool onRoomCreate)
	{
		if (connectionPanel.gameObject.activeSelf)
		{
			connectionPanel.gameObject.SetActive(false);
		}
	}
}
