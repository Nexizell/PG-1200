using System;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public class MiniGamesController : MonoBehaviour
{
	internal struct MiniGamesParameters
	{
		public float startTimer;

		public float goTimer;

		public int minPlayerCount;
	}

	private static readonly IDictionary<GameConnect.GameMode, MiniGamesParameters> paramsByGameMode = new Dictionary<GameConnect.GameMode, MiniGamesParameters>
	{
		{
			GameConnect.GameMode.TimeBattle,
			new MiniGamesParameters
			{
				startTimer = 15f,
				goTimer = 6f,
				minPlayerCount = 2
			}
		},
		{
			GameConnect.GameMode.DeadlyGames,
			new MiniGamesParameters
			{
				startTimer = 15f,
				goTimer = 6f,
				minPlayerCount = 4
			}
		},
		{
			GameConnect.GameMode.DeathEscape,
			new MiniGamesParameters
			{
				startTimer = 10f,
				goTimer = 6f,
				minPlayerCount = 1
			}
		},
		{
			GameConnect.GameMode.Spleef,
			new MiniGamesParameters
			{
				startTimer = 15f,
				goTimer = 6f,
				minPlayerCount = 2
			}
		}
	};

	private static MiniGamesController _instance;

	public bool isStartGame;

	public bool isStartTimer;

	public bool isShowGo;

	public bool isGo;

	public bool theEnd;

	public bool isRunPlayer;

	public bool playerDead;

	public float startTimer = 30f;

	public float goTimer = 10.5f;

	public float gameTimer = 600f;

	[HideInInspector]
	public float timeInGame;

	private float timeToSynchTimer = 2f;

	private float timerShowGo = 1f;

	private int maxCountPlayers = 10;

	public int minCountPlayer = 2;

	public bool isEnd;

	public bool isDraw;

	private int playersWasInMatch;

	[HideInInspector]
	public int playersOnStart;

	[HideInInspector]
	public PhotonView photonView;

	public Dictionary<string, ScoreTableItem> playerScores = new Dictionary<string, ScoreTableItem>(10);

	private string waitingPlayerLocalize;

	private string matchLocalize;

	private string preparingLocalize;

	public static MiniGamesController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (!GameConnect.isMiniGame)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (paramsByGameMode.ContainsKey(GameConnect.gameMode))
		{
			MiniGamesParameters miniGamesParameters = paramsByGameMode[GameConnect.gameMode];
			startTimer = miniGamesParameters.startTimer;
			goTimer = miniGamesParameters.goTimer;
			minCountPlayer = miniGamesParameters.minPlayerCount;
		}
		maxCountPlayers = PhotonNetwork.room.maxPlayers;
		gameTimer = int.Parse(PhotonNetwork.room.customProperties[GameConnect.maxKillProperty].ToString()) * 60;
		photonView = GetComponent<PhotonView>();
		_instance = this;
		waitingPlayerLocalize = LocalizationStore.Key_0565;
		matchLocalize = LocalizationStore.Key_0566;
		preparingLocalize = LocalizationStore.Key_0567;
	}

	public void UpdatePlayerScore(NetworkStartTable table)
	{
		if (!string.IsNullOrEmpty(table.pixelBookID) && !table.pixelBookID.Equals("-1"))
		{
			string key = table.pixelBookID + "-" + table.playerUniqID;
			if (playerScores.ContainsKey(key))
			{
				playerScores[key].UpdateFromTable(table);
				return;
			}
			ScoreTableItem value = new ScoreTableItem(table);
			playerScores.Add(key, value);
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	private void Update()
	{
		if (isStartTimer && startTimer > 0f)
		{
			startTimer -= Time.deltaTime;
		}
		if (isStartGame && goTimer > 0f)
		{
			goTimer -= Time.deltaTime;
		}
		if (goTimer < 0f)
		{
			goTimer = 0f;
		}
		if (isShowGo && timerShowGo >= 0f)
		{
			timerShowGo -= Time.deltaTime;
		}
		if (isShowGo && timerShowGo < 0f)
		{
			isShowGo = false;
		}
		if (isGo && gameTimer > 0f && Initializer.players.Count > 0)
		{
			gameTimer -= Time.deltaTime;
		}
		if (gameTimer <= 0f && !theEnd && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			EndGame();
			try
			{
				AnalyticsStuff.SpleefTime(Instance.timeInGame);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in SpleefTime send: {0}", ex);
			}
		}
		if (PhotonNetwork.isMasterClient)
		{
			timeToSynchTimer -= Time.deltaTime;
			if (isGo && timeToSynchTimer < 0f)
			{
				timeToSynchTimer = 0.5f;
				photonView.RPC("SynchGameTimer", PhotonTargets.Others, gameTimer);
			}
			int count = Initializer.networkTables.Count;
			if (!isStartGame)
			{
				if (!isStartTimer && count >= minCountPlayer)
				{
					photonView.RPC("StartTimer", PhotonTargets.AllBuffered, true);
				}
				if (timeToSynchTimer < 0f)
				{
					timeToSynchTimer = 0.5f;
					photonView.RPC("SynchStartTimer", PhotonTargets.Others, startTimer);
				}
				if ((!isStartGame && isStartTimer && startTimer < 0.1f && count >= minCountPlayer) || (!isStartGame && isStartTimer && count == PhotonNetwork.room.maxPlayers))
				{
					photonView.RPC("StartGame", PhotonTargets.AllBuffered);
					PhotonNetwork.room.visible = false;
				}
			}
			else
			{
				if (timeToSynchTimer < 0f)
				{
					timeToSynchTimer = 0.5f;
					photonView.RPC("SynchTimerGo", PhotonTargets.Others, goTimer);
				}
				if (!isGo && Mathf.FloorToInt(goTimer) <= 0)
				{
					photonView.RPC("Go", PhotonTargets.AllBuffered);
				}
			}
		}
		if (!isStartGame)
		{
			string text = "";
			if (!isStartTimer)
			{
				text = waitingPlayerLocalize;
			}
			else
			{
				if (startTimer > 0f && !isStartGame)
				{
					float num = startTimer;
					text = matchLocalize + " " + Mathf.FloorToInt(num / 60f) + ":" + ((Mathf.FloorToInt(num - (float)(Mathf.FloorToInt(num / 60f) * 60)) < 10) ? "0" : "") + Mathf.FloorToInt(num - (float)(Mathf.FloorToInt(num / 60f) * 60));
				}
				if (startTimer < 0f && !isStartGame)
				{
					text = preparingLocalize;
				}
			}
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				NetworkStartTableNGUIController.sharedController.HungerStartLabel.text = text;
			}
		}
		if (isStartGame && !isRunPlayer && !isEnd && WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			Debug.Log("Start minigames player");
			isRunPlayer = true;
			isDraw = false;
			StartMatch();
		}
		if (!theEnd && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			timeInGame += Time.deltaTime;
			CheckMatchEndConditions();
		}
		playersWasInMatch = Mathf.Max(playersWasInMatch, Initializer.networkTables.Count);
		if (isGo)
		{
			UpdatePlayerScores();
		}
	}

	public void UpdatePlayerScores()
	{
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			UpdatePlayerScore(Initializer.networkTables[i]);
		}
	}

	private void StartMatch()
	{
		WeaponManager.sharedManager.myNetworkStartTable.StartPlayerMatch();
	}

	private void GoMatch()
	{
		isGo = true;
		isShowGo = true;
		playersOnStart = Initializer.networkTables.Count;
		if (GameConnect.isDeathEscape)
		{
			CheckpointController.instance.StartRun();
		}
	}

	public void FinishGame()
	{
		if (GameConnect.isDeathEscape)
		{
			CheckpointController.instance.EndRun();
		}
		EndGame();
	}

	private void EndGame()
	{
		if (!theEnd)
		{
			theEnd = true;
			isDraw = true;
			if (GameConnect.isCOOP)
			{
				ZombiManager.sharedManager.EndMatch();
				return;
			}
			if (WeaponManager.sharedManager.myPlayer != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.WinFromTimer();
				return;
			}
			GlobalGameController.countKillsRed = 0;
			GlobalGameController.countKillsBlue = 0;
		}
	}

	private void EndDeadheat()
	{
	}

	public bool OnPlayerRespawn()
	{
		if (GameConnect.isSpleef)
		{
			playerDead = true;
			WeaponManager.sharedManager.myNetworkStartTable.ImDeadInSpleef();
			EndGame();
			PhotonNetwork.Destroy(WeaponManager.sharedManager.myPlayer);
			return false;
		}
		if (GameConnect.isHunger)
		{
			playerDead = true;
			WeaponManager.sharedManager.myNetworkStartTable.ImDeadInHungerGames();
			EndGame();
			PhotonNetwork.Destroy(WeaponManager.sharedManager.myPlayer);
			return false;
		}
		return true;
	}

	protected virtual void CheckMatchEndConditions()
	{
		if (GameConnect.isSpleef)
		{
			bool flag = InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.pausePanel.activeSelf;
			if (playersWasInMatch > 1 && Initializer.players.Count == 1 && isGo && timeInGame > 10f && !flag)
			{
				WeaponManager.sharedManager.myNetworkStartTable.WinInSpleef();
			}
		}
	}

	
	[PunRPC]
	public void StartTimer(bool _isStartTimer)
	{
		isStartTimer = _isStartTimer;
	}

	
	[PunRPC]
	public void SynchStartTimer(float _startTimer)
	{
		startTimer = _startTimer;
	}

	[PunRPC]
	
	public void SynchTimerGo(float _goTimer)
	{
		goTimer = _goTimer;
	}

	[PunRPC]
	
	public void SynchGameTimer(float _gameTimer)
	{
		gameTimer = _gameTimer;
	}

	
	[PunRPC]
	public void StartGame()
	{
		isStartGame = true;
	}

	
	[PunRPC]
	public void Go()
	{
		GoMatch();
	}
}
