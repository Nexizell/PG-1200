using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;

public class GameConnect : MonoBehaviour
{
	public enum GameMode
	{
		Deathmatch = 0,
		TimeBattle = 1,
		TeamFight = 2,
		DeadlyGames = 3,
		FlagCapture = 4,
		CapturePoints = 5,
		InFriendWindow = 6,
		InClanWindow = 7,
		Duel = 8,
		Dater = 9,
		DeathEscape = 10,
		Campaign = 11,
		Arena = 12,
		SpeedRun = 13,
		Spleef = 14
	}

	public enum PlatformConnect
	{
		ios = 1,
		android = 2,
		custom = 3
	}

	public enum ABTestParams
	{
		Old = 1,
		Rating = 2,
		Buff = 3
	}

	public delegate void OnDisconnectReason(int cause);

	public static GameMode[] MiniGames = new GameMode[8]
	{
		GameMode.Dater,
		GameMode.DeathEscape,
		GameMode.Campaign,
		GameMode.Arena,
		GameMode.SpeedRun,
		GameMode.Spleef,
		GameMode.TimeBattle,
		GameMode.DeadlyGames
	};

	public static readonly IDictionary<int, string> gameModesLocalizeKey = new Dictionary<int, string>
	{
		{ 0, "Key_0104" },
		{ 1, "Key_0135" },
		{ 2, "Key_0130" },
		{ 3, "Key_0121" },
		{ 4, "Key_0113" },
		{ 5, "Key_1263" },
		{ 6, "Key_1465" },
		{ 7, "Key_1466" },
		{ 8, "Key_2428" },
		{ 9, "Key_1567" },
		{ 10, "Key_3164" },
		{ 14, "Key_3166" },
		{ 13, "Key_3165" }
	};

	public static readonly IDictionary<int, string> gameModesRulesLocalizeKey = new Dictionary<int, string>
	{
		{ 0, "Key_0550" },
		{ 1, "Key_0552" },
		{ 2, "Key_0551" },
		{ 3, "Key_0553" },
		{ 4, "Key_0554" },
		{ 5, "Key_1368" },
		{ 8, "Key_2406" },
		{ 9, "Key_1538" },
		{ 10, "Key_3181" },
		{ 14, "Key_3183" },
		{ 13, "Key_3182" }
	};

	public static readonly IDictionary<int, string> storeKitEventMode = new Dictionary<int, string>
	{
		{ 0, "Deathmatch Wordwide" },
		{ 1, "Time Survival" },
		{ 2, "Team Battle" },
		{ 3, "Deadly Games" },
		{ 4, "Flag Capture" },
		{ 5, "Capture points" },
		{ 8, "Duel" },
		{ 9, "Dater" },
		{ 10, "Death Escape" },
		{ 11, "Campaign" },
		{ 12, "Survival" },
		{ 14, "Spleef" },
		{ 13, "SpeedRun" }
	};

	private static GameMode _gameMode = GameMode.Deathmatch;

	public static PlatformConnect myPlatformConnect = PlatformConnect.ios;

	public static int gameTier;

	public static readonly string mapProperty = "C0";

	public static readonly string passwordProperty = "C1";

	public static readonly string platformProperty = "C2";

	public static readonly string endingProperty = "C3";

	public static readonly string maxKillProperty = "C4";

	public static readonly string ABTestProperty = "C5";

	public static readonly string ABTestEnum = "C6";

	public static readonly string RatingProperty = "C7";

	public static readonly string roomStatusProperty = "Closed";

	public static readonly string timeProperty = "TimeMatchEnd";

	public static readonly string tierProperty = "tier";

	public static readonly string specialBonusProperty = "SpecialBonus";

	public static OnDisconnectReason OnFailedToConnect;

	public static OnDisconnectReason OnConnectionFailed;

	public static Action OnDisconnected;

	public static Action OnConnectedMaster;

	public static Action<string> OnJoinToMap;

	public static Action OnJoinedToRoom;

	public static Action<bool> OnJoinRoomFailed;

	public static Action OnLeftRoomEvent;

	public static Action OnReceivedRoomList;

	private static GameConnect _instance;

	private bool ratingSearchFirstTry;

	private int joinNewRoundTries;

	private SceneInfo tryJoinScene;

	public static GameMode gameMode
	{
		get
		{
			return _gameMode;
		}
		set
		{
			SetGameMode(value);
		}
	}

	public static GameConnect instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject("GameConnect");
				UnityEngine.Object.DontDestroyOnLoad(obj);
				_instance = obj.AddComponent<GameConnect>();
			}
			return _instance;
		}
	}

	public static bool AllowReconnect
	{
		get
		{
			if (gameMode != GameMode.FlagCapture && gameMode != GameMode.TeamFight)
			{
				return gameMode != GameMode.CapturePoints;
			}
			return false;
		}
	}

	public static bool AllowReconnectWithScore
	{
		get
		{
			if (gameMode != 0 && gameMode != GameMode.Duel && gameMode != GameMode.Dater)
			{
				return isMiniGame;
			}
			return true;
		}
	}

	public static bool wearEffectsDisabled
	{
		get
		{
			if (!isHunger && !isDeathEscape && !isSpleef && !isSpeedrun && !isCOOP)
			{
				return isSurvival;
			}
			return true;
		}
	}

	public static bool isTeamRegim
	{
		get
		{
			if (Defs.isMulti)
			{
				if (gameMode != GameMode.TeamFight && gameMode != GameMode.FlagCapture)
				{
					return gameMode == GameMode.CapturePoints;
				}
				return true;
			}
			return false;
		}
	}

	public static bool isMiniGame
	{
		get
		{
			if (Defs.isMulti)
			{
				return IsMiniGameMode(gameMode);
			}
			return false;
		}
	}

	public static bool isCOOP
	{
		get
		{
			return gameMode == GameMode.TimeBattle;
		}
	}

	public static bool isCompany
	{
		get
		{
			return gameMode == GameMode.TeamFight;
		}
	}

	public static bool isFlag
	{
		get
		{
			return gameMode == GameMode.FlagCapture;
		}
	}

	public static bool isHunger
	{
		get
		{
			return gameMode == GameMode.DeadlyGames;
		}
		set
		{
			gameMode = (value ? GameMode.DeadlyGames : GameMode.Deathmatch);
		}
	}

	public static bool isCapturePoints
	{
		get
		{
			return gameMode == GameMode.CapturePoints;
		}
	}

	public static bool isDuel
	{
		get
		{
			return gameMode == GameMode.Duel;
		}
	}

	public static bool isDaterRegim
	{
		get
		{
			return gameMode == GameMode.Dater;
		}
	}

	public static bool isDeathEscape
	{
		get
		{
			return gameMode == GameMode.DeathEscape;
		}
	}

	public static bool isSpleef
	{
		get
		{
			return gameMode == GameMode.Spleef;
		}
	}

	public static bool isSurvival
	{
		get
		{
			return gameMode == GameMode.Arena;
		}
	}

	public static bool isCampaign
	{
		get
		{
			return gameMode == GameMode.Campaign;
		}
	}

	public static bool isSpeedrun
	{
		get
		{
			return gameMode == GameMode.SpeedRun;
		}
	}

	private static string GetConnectGameVersion()
	{
		string text = "";
		text = ((!Defs.useRatingLobbySystem) ? gameTier.ToString() : ("l" + gameTier));
		return Initializer.Separator + (isDaterRegim ? "Deathmatch" : gameMode.ToString()) + (isHunger ? "0" : ((isMiniGame || isDaterRegim) ? "M" : text)) + "v" + GlobalGameController.MultiplayerProtocolVersion;
	}

	public static int GetTierForRoom()
	{
		if (Defs.useRatingLobbySystem)
		{
			if (!(RatingSystem.instance != null))
			{
				return 0;
			}
			return (int)RatingSystem.instance.currentLeague;
		}
		if (!(ExpController.Instance != null))
		{
			return 1;
		}
		return ExpController.Instance.OurTier;
	}

	private static void Initialize()
	{
		if (instance != null)
		{
			Debug.Log("GameConnect initialized");
		}
	}

	public static void Disconnect()
	{
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
	}

	public static void ConnectToRandomRoom(SceneInfo scene = null)
	{
		if (!PhotonNetwork.connected)
		{
			Debug.LogError("Not connected!");
		}
		else
		{
			instance.JoinToRandomRoom(gameMode, scene);
		}
	}

	private void Awake()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	private void OnDestroy()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void JoinToRandomRoom(GameMode mode, SceneInfo scene)
	{
		joinNewRoundTries = 0;
		tryJoinScene = scene;
		ratingSearchFirstTry = Defs.useRatingLobbySystem && RatingSystem.instance.currentLeague != RatingSystem.RatingLeague.Adamant;
		if (tryJoinScene != null && OnJoinToMap != null)
		{
			OnJoinToMap(tryJoinScene.NameScene);
		}
		JoinRandomGameRoom((tryJoinScene == null) ? (-1) : tryJoinScene.indexMap, mode, joinNewRoundTries, ratingSearchFirstTry);
	}

	private void OnPhotonRandomJoinFailed()
	{
		if (tryJoinScene == null)
		{
			tryJoinScene = GetRandomMap(gameMode);
			if (tryJoinScene == null)
			{
				return;
			}
		}
		if (joinNewRoundTries >= 2 && ratingSearchFirstTry)
		{
			ratingSearchFirstTry = false;
			joinNewRoundTries = 0;
		}
		if (joinNewRoundTries < 2)
		{
			Debug.Log("No rooms with new round: " + joinNewRoundTries + (ratingSearchFirstTry ? " <color=yellow>first rating search</color>" : ""));
			joinNewRoundTries++;
			JoinRandomGameRoom(tryJoinScene.indexMap, gameMode, joinNewRoundTries, ratingSearchFirstTry);
			return;
		}
		int matchMinutesForMode = GetMatchMinutesForMode(gameMode);
		int maxPlayersForMode = GetMaxPlayersForMode(gameMode);
		if (OnJoinToMap != null)
		{
			OnJoinToMap(tryJoinScene.name);
		}
		CreateGameRoom(null, maxPlayersForMode, tryJoinScene.indexMap, matchMinutesForMode, "", gameMode);
	}

	private void OnPhotonCreateRoomFailed()
	{
		if (OnJoinRoomFailed != null)
		{
			OnJoinRoomFailed(true);
		}
	}

	private void OnPhotonJoinRoomFailed()
	{
		if (OnJoinRoomFailed != null)
		{
			OnJoinRoomFailed(false);
		}
	}

	private void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("OnFailedToConnectToPhoton: " + cause);
		if (OnFailedToConnect != null)
		{
			OnFailedToConnect((int)cause);
		}
	}

	private void OnConnectedToMaster()
	{
		if (OnConnectedMaster != null)
		{
			OnConnectedMaster();
		}
	}

	private void OnDisconnectedFromPhoton()
	{
		if (OnDisconnected != null)
		{
			OnDisconnected();
		}
	}

	private void OnConnectedToPhoton()
	{
	}

	private void OnConnectionFail(DisconnectCause cause)
	{
		if (OnConnectionFailed != null)
		{
			OnConnectionFailed((int)cause);
		}
	}

	private void OnJoinedRoom()
	{
		if (OnJoinedToRoom != null)
		{
			OnJoinedToRoom();
		}
	}

	private void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby: " + PhotonNetwork.lobby.Name);
		if (OnConnectedMaster != null)
		{
			OnConnectedMaster();
		}
	}

	private void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
	}

	private void OnLeftRoom()
	{
		if (OnLeftRoomEvent != null)
		{
			OnLeftRoomEvent();
		}
	}

	private void OnReceivedRoomListUpdate()
	{
		if (OnReceivedRoomList != null)
		{
			OnReceivedRoomList();
		}
	}

	public static bool ConnectToPhoton(int tier)
	{
		gameTier = tier;
		return ConnectToPhoton();
	}

	public static bool ConnectToPhoton()
	{
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			return false;
		}
		Initialize();
		PhotonNetwork.lobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
		return PhotonNetwork.ConnectUsingSettings(GetConnectGameVersion());
	}

	public static void SetGameMode(GameMode newGameMode)
	{
		_gameMode = newGameMode;
		StoreKitEventListener.State.Mode = storeKitEventMode[(int)newGameMode];
		StoreKitEventListener.State.Parameters.Clear();
	}

	public static void CreateGameRoom(string roomName, int playerLimit, int mapIndex, int MaxKill, string _password, GameMode gameMode)
	{
		string[] roomPropsInLobby = new string[12]
		{
			mapProperty, passwordProperty, platformProperty, endingProperty, maxKillProperty, timeProperty, tierProperty, ABTestProperty, ABTestEnum, specialBonusProperty,
			roomStatusProperty, RatingProperty
		};
		Hashtable hashtable = new Hashtable();
		hashtable[mapProperty] = mapIndex;
		hashtable[passwordProperty] = _password;
		hashtable[platformProperty] = (int)(string.IsNullOrEmpty(_password) ? myPlatformConnect : PlatformConnect.custom);
		hashtable[endingProperty] = 0;
		hashtable[maxKillProperty] = MaxKill;
		hashtable[timeProperty] = PhotonNetwork.time;
		hashtable[tierProperty] = ((ExpController.Instance != null) ? ExpController.Instance.OurTier : 0);
		if (GetTierForRoom() == 0)
		{
			hashtable[ABTestProperty] = (Defs.isABTestBalansCohortActual ? 1 : 0);
		}
		if (Defs.isActivABTestBuffSystem)
		{
			hashtable[ABTestEnum] = ((!ABTestController.useBuffSystem) ? 1 : 3);
		}
		hashtable[specialBonusProperty] = 0;
		hashtable[roomStatusProperty] = 0;
		hashtable[RatingProperty] = RatingSystem.instance.GetRatingForRooms();
		PhotonCreateRoom(roomName, true, true, (playerLimit > 10) ? 10 : playerLimit, hashtable, roomPropsInLobby);
	}

	public static void PhotonCreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, Hashtable roomProps, string[] roomPropsInLobby)
	{
		RoomOptions roomOptions = new RoomOptions
		{
			customRoomProperties = roomProps,
			customRoomPropertiesForLobby = roomPropsInLobby
		};
		roomOptions.MaxPlayers = (byte)maxPlayers;
		roomOptions.IsOpen = isOpen;
		roomOptions.IsVisible = isVisible;
		TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
	}

	public static void JoinRandomGameRoom(int mapIndex, GameMode gameMode, int joinToNewRound, bool ratingFirstTry = true)
	{
		string text = "";
		if (mapIndex == -1)
		{
			int[] array = (from m in SceneInfoController.instance.GetListScenesForMode(gameMode).avaliableScenes
				select m.indexMap into m
				where m != 1010
				select m).ToArray();
			text += "( ";
			for (int i = 0; i < array.Length; i++)
			{
				text = text + mapProperty + " = " + array[i];
				if (i + 1 < array.Length)
				{
					text += " OR ";
				}
			}
			text += " )";
		}
		else
		{
			text = mapProperty + " = " + mapIndex;
		}
		text = text + " AND " + passwordProperty + " = \"\"";
		if (!isDaterRegim)
		{
			string[] obj = new string[5] { text, " AND ", platformProperty, " = ", null };
			int num = (int)myPlatformConnect;
			obj[4] = num.ToString();
			text = string.Concat(obj);
		}
		switch (joinToNewRound)
		{
		case 0:
			text = text + " AND " + endingProperty + " = 0";
			break;
		case 1:
			text = text + " AND " + endingProperty + " = 2";
			break;
		}
		if (ExpController.Instance != null && GetTierForRoom() == 0)
		{
			text = ((!Defs.isABTestBalansCohortActual) ? (text + " AND " + ABTestProperty + " = 0") : (text + " AND " + ABTestProperty + " = 1"));
		}
		if (Defs.useRatingLobbySystem && RatingSystem.instance.currentLeague != RatingSystem.RatingLeague.Adamant && ratingFirstTry)
		{
			int lowBorderForRooms = RatingSystem.instance.GetLowBorderForRooms();
			if (lowBorderForRooms > 0)
			{
				Debug.Log("Low border = " + lowBorderForRooms);
				text = text + " AND " + RatingProperty + " > " + lowBorderForRooms;
			}
			int highBorderForRooms = RatingSystem.instance.GetHighBorderForRooms();
			Debug.Log("High border = " + highBorderForRooms);
			text = text + " AND " + RatingProperty + " < " + highBorderForRooms;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[passwordProperty] = string.Empty;
		if (!isDaterRegim && gameMode != GameMode.DeadlyGames && gameMode != GameMode.TimeBattle)
		{
			hashtable[maxKillProperty] = 3;
		}
		if (joinToNewRound == 0)
		{
			hashtable[endingProperty] = 0;
		}
		if (!isDaterRegim)
		{
			hashtable[platformProperty] = (int)myPlatformConnect;
		}
		if (ExpController.Instance != null && GetTierForRoom() == 0)
		{
			if (Defs.isABTestBalansCohortActual)
			{
				hashtable[ABTestProperty] = 1;
			}
			else
			{
				hashtable[ABTestProperty] = 0;
			}
		}
		TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
		Debug.Log(text);
		PhotonNetwork.JoinRandomRoom(hashtable, 0, MatchmakingMode.FillRoom, typedLobby, text);
	}

	public static int GetMaxPlayersForMode(GameMode mode)
	{
		int result = 10;
		switch (mode)
		{
		case GameMode.TimeBattle:
			result = 4;
			break;
		case GameMode.TeamFight:
			result = 10;
			break;
		case GameMode.DeadlyGames:
			result = 6;
			break;
		case GameMode.Duel:
			result = 2;
			break;
		case GameMode.Spleef:
			result = 4;
			break;
		case GameMode.DeathEscape:
			result = 4;
			break;
		}
		return result;
	}

	public static int GetMatchMinutesForMode(GameMode mode)
	{
		int result = 4;
		switch (mode)
		{
		case GameMode.DeadlyGames:
			result = 10;
			break;
		case GameMode.Dater:
			result = 10;
			break;
		case GameMode.DeathEscape:
			result = 10;
			break;
		}
		return result;
	}

	public static SceneInfo GetRandomMap(GameMode mode)
	{
		bool flag = true;
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(mode);
		if (listScenesForMode == null)
		{
			return null;
		}
		int count = listScenesForMode.avaliableScenes.Count;
		int num = UnityEngine.Random.Range(0, count);
		int num2 = 0;
		SceneInfo sceneInfo;
		do
		{
			if (num2 > count)
			{
				return null;
			}
			sceneInfo = listScenesForMode.avaliableScenes[num];
			if (!(sceneInfo == null))
			{
				num++;
				num2++;
				if (num >= count)
				{
					num = 0;
				}
				flag = (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key") == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene)) || sceneInfo.indexMap == 1010;
			}
		}
		while (flag);
		return sceneInfo;
	}

	public static int GetRandomMapIndex(GameMode mode)
	{
		bool flag = true;
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(mode);
		if (listScenesForMode == null)
		{
			return -1;
		}
		int count = listScenesForMode.avaliableScenes.Count;
		int num = UnityEngine.Random.Range(0, count);
		int num2 = 0;
		SceneInfo sceneInfo;
		do
		{
			if (num2 > count)
			{
				return -1;
			}
			sceneInfo = listScenesForMode.avaliableScenes[num];
			if (!(sceneInfo == null))
			{
				num++;
				num2++;
				if (num >= count)
				{
					num = 0;
				}
				flag = sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key") == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene);
			}
		}
		while (flag);
		return sceneInfo.indexMap;
	}

	public static bool IsMiniGameMode(GameMode mode)
	{
		if (mode != GameMode.DeadlyGames && mode != GameMode.TimeBattle && mode != GameMode.DeathEscape)
		{
			return mode == GameMode.Spleef;
		}
		return true;
	}
}
