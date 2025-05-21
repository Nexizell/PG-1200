using System;
using System.Collections.Generic;
using UnityEngine;

public class LANBroadcastService : MonoBehaviour
{
	public delegate void delJoinServer(string strIP);

	public delegate void delStartServer();

	internal enum enuState
	{
		NotActive = 0,
		Searching = 1,
		Announcing = 2
	}

	public struct ReceivedMessage
	{
		public string ipAddress;

		public string name;

		public string map;

		public int connectedPlayers;

		public int playerLimit;

		public string comment;

		public float fTime;

		public int regim;

		public string protocol;
	}

	public ReceivedMessage serverMessage;

	private string strMessage = "";

	private enuState currentState;

	public List<ReceivedMessage> lstReceivedMessages;

	private delJoinServer delWhenServerFound;

	private delStartServer delWhenServerMustStarted;

	private string strServerNotReady = "wanttobeaserver";

	private string strServerReady = "iamaserver";

	private float fTimeLastMessageSent;

	private float fIntervalMessageSending = 1f;

	private float fTimeMessagesLive = 5f;

	private float fTimeToSearch = 5f;

	private float fTimeSearchStarted;

	private string ipaddress;

	public string Message
	{
		get
		{
			return strMessage;
		}
	}

	private void Start()
	{
		lstReceivedMessages = new List<ReceivedMessage>();
	}

	private void Update()
	{
	}

	private void BeginAsyncReceive()
	{
	}

	private void EndAsyncReceive(IAsyncResult objResult)
	{
	}

	private void StartAnnouncing()
	{
		currentState = enuState.Announcing;
		strMessage = "Announcing we are a server...";
	}

	private void StopAnnouncing()
	{
		currentState = enuState.NotActive;
		strMessage = "Announcements stopped.";
	}

	private void StartSearching()
	{
		if (lstReceivedMessages == null)
		{
			lstReceivedMessages = new List<ReceivedMessage>();
		}
		lstReceivedMessages.Clear();
		BeginAsyncReceive();
		fTimeSearchStarted = Time.time;
		currentState = enuState.Searching;
		strMessage = "Searching for other players...";
	}

	private void StopSearching()
	{
		currentState = enuState.NotActive;
		strMessage = "Search stopped.";
	}

	public void StartSearchBroadCasting(delJoinServer connectToServer)
	{
		delWhenServerFound = connectToServer;
		StartBroadcastingSession();
		StartSearching();
	}

	public void StartAnnounceBroadCasting()
	{
		StartBroadcastingSession();
		StartAnnouncing();
	}

	private void StartBroadcastingSession()
	{
		if (currentState != 0)
		{
			StopBroadCasting();
		}
	}

	public void StopBroadCasting()
	{
	}
}
