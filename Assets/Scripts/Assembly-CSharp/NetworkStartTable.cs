using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using FyberPlugin;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.WP8;
using UnityEngine;

public sealed class NetworkStartTable : MonoBehaviour
{
	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	[CompilerGenerated]
	internal sealed class _003CStartPlayerCoroutine_003Ed__146 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTable _003C_003E4__this;

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
		public _003CStartPlayerCoroutine_003Ed__146(int _003C_003E1__state)
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
				Defs.inRespawnWindow = false;
				if (Defs.isMulti && Defs.isInet)
				{
					_003C_003E4__this.photonView.RPC("SynchGameRating", PhotonTargets.Others, _003C_003E4__this.gameRating);
				}
				if (Defs.isMulti && GameConnect.isDuel)
				{
					ShopNGUIController.sharedShop.onEquipSkinAction = delegate
					{
						_003C_003E4__this.sendMySkin();
					};
				}
				if (_003C_003E4__this.isStartPlayerCoroutine)
				{
					return false;
				}
				_003C_003E4__this.isStartPlayerCoroutine = true;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (Defs.isMulti && Defs.isInet && PhotonNetwork.time > -0.01 && PhotonNetwork.time < 0.01)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.isStartPlayerCoroutine = false;
			if (Defs.isMulti && !GameConnect.isMiniGame && !GameConnect.isDuel)
			{
				TimeGameController.sharedController.StartMatch();
			}
			if (GameConnect.isDaterRegim)
			{
				int num = 5;
				num = ((!Defs.isInet) ? ((!PlayerPrefs.GetString("MaxKill", "9").Equals("")) ? int.Parse(PlayerPrefs.GetString("MaxKill", "5")) : 5) : ((int)PhotonNetwork.room.customProperties[GameConnect.maxKillProperty]));
				AnalyticsStuff.LogSandboxTimeGamePopularity(num, true);
			}
			_003C_003E4__this._matchStopwatch.Start();
			StoreKitEventListener.State.PurchaseKey = "In game";
			StoreKitEventListener.State.Parameters.Clear();
			if (_003C_003E4__this.networkStartTableNGUIController != null)
			{
				_003C_003E4__this.networkStartTableNGUIController.shopAnchor.SetActive(false);
			}
			_003C_003E4__this.RemoveShop(!BankController.Instance.InterfaceEnabled);
			if (GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
			{
				_003C_003E4__this.timerFlag = _003C_003E4__this.maxTimerFlag;
			}
			if (_003C_003E4__this.myRanks != _003C_003E4__this.expController.currentLevel)
			{
				_003C_003E4__this.SetRanks();
			}
			_003C_003E4__this._cam = GameObject.FindGameObjectWithTag("CamTemp");
			_003C_003E4__this.SetSceneCameraEnabled(false);
			_003C_003E4__this._weaponManager.useCam = null;
			_003C_003E4__this.zoneCreatePlayer = GameObject.FindGameObjectsWithTag((GameConnect.gameMode == GameConnect.GameMode.TimeBattle) ? "MultyPlayerCreateZoneCOOP" : ((GameConnect.gameMode == GameConnect.GameMode.TeamFight) ? ("MultyPlayerCreateZoneCommand" + _003C_003E4__this.myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.FlagCapture) ? ("MultyPlayerCreateZoneFlagCommand" + _003C_003E4__this.myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.CapturePoints) ? ("MultyPlayerCreateZonePointZone" + _003C_003E4__this.myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.Duel) ? ("MultyPlayerCreateZoneDuel" + DuelController.instance.myRespawnPoints) : "MultyPlayerCreateZone")))));
			GameObject gameObject = null;
			int num2 = 0;
			int num3 = 0;
			if (GameConnect.isMiniGame)
			{
				if (GameConnect.isHunger && !StartAfterDisconnect)
				{
					_003C_003E4__this._weaponManager.Reset();
				}
				int iD = _003C_003E4__this.photonView.owner.ID;
				for (int i = 0; i < Initializer.networkTables.Count; i++)
				{
					PhotonPlayer owner = Initializer.networkTables[i].GetComponent<PhotonView>().owner;
					if (owner != null && owner.ID < iD)
					{
						num2++;
					}
				}
				if (num2 >= _003C_003E4__this.zoneCreatePlayer.Length)
				{
					num2 = _003C_003E4__this.zoneCreatePlayer.Length - 1;
					UnityEngine.Debug.LogError("Not enough spawn points for players! Add more");
				}
				if (GameConnect.isHunger)
				{
					num3 = num2;
					for (int j = 0; j < _003C_003E4__this.zoneCreatePlayer.Length; j++)
					{
						if (_003C_003E4__this.zoneCreatePlayer[j].GetComponent<NumberZone>().numberZone == num2)
						{
							num2 = j;
							break;
						}
					}
					if (!StartAfterDisconnect)
					{
						GameObject[] array = GameObject.FindGameObjectsWithTag("ChestCreateZone");
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k].GetComponent<NumberZone>().numberZone == num3)
							{
								gameObject = array[k];
								_003C_003E4__this.photonView.RPC("CreateChestRPC", PhotonTargets.MasterClient, gameObject.transform.position, gameObject.transform.rotation);
								break;
							}
						}
					}
					_003C_003E4__this.playerCountInHunger = Initializer.networkTables.Count;
				}
			}
			GameObject gameObject2 = _003C_003E4__this.zoneCreatePlayer[GameConnect.isMiniGame ? num2 : UnityEngine.Random.Range(0, _003C_003E4__this.zoneCreatePlayer.Length - 1)];
			BoxCollider component = gameObject2.GetComponent<BoxCollider>();
			Vector2 vector = new Vector2(component.size.x * gameObject2.transform.localScale.x, component.size.z * gameObject2.transform.localScale.z);
			Rect rect = new Rect(gameObject2.transform.position.x - vector.x / 2f, gameObject2.transform.position.z - vector.y / 2f, vector.x, vector.y);
			Vector3 position = ((!_003C_003E4__this.isHunger) ? new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject2.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height)) : gameObject2.transform.position);
			Quaternion rotation = gameObject2.transform.rotation;
			if (StartAfterDisconnect && GlobalGameController.healthMyPlayer > 0f)
			{
				position = GlobalGameController.posMyPlayer;
			}
			GameObject gameObject3;
			if (_003C_003E4__this.isInet)
			{
				gameObject3 = PhotonNetwork.Instantiate("Player", position, rotation, 0);
			}
			else
			{
				if (_003C_003E4__this._playerPrefab == null)
				{
					_003C_003E4__this._playerPrefab = Resources.Load("Player") as GameObject;
				}
				gameObject3 = null;
			}
			NickLabelController.currentCamera = gameObject3.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
			_003C_003E4__this._weaponManager.myPlayer = gameObject3;
			_003C_003E4__this._weaponManager.myPlayerMoveC = gameObject3.GetComponent<SkinName>().playerMoveC;
			if (!_003C_003E4__this.isInet && _003C_003E4__this.isServer)
			{
				UnityEngine.Debug.Log("networkView.RPC(RunGame, RPCMode.OthersBuffered);");
			}
			Initializer.Instance.SetupObjectThatNeedsPlayer();
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				NetworkStartTableNGUIController.sharedController.HideStartInterface();
			}
			_003C_003E4__this.showTable = false;
			if (ABTestController.useBuffSystem)
			{
				BuffSystem.instance.BuffsActive(!GameConnect.isDaterRegim && !GameConnect.isHunger && !GameConnect.isCOOP && Defs.isMulti && Defs.isInet && !LocalOrPasswordRoom());
			}
			else
			{
				KillRateCheck.instance.SetActive(!GameConnect.isDaterRegim && !GameConnect.isHunger && !GameConnect.isCOOP && Defs.isMulti && Defs.isInet && !LocalOrPasswordRoom() && WeaponManager.sharedManager._currentFilterMap == 0, TimeGameController.sharedController != null && TimeGameController.sharedController.timerToEndMatch > 30.0);
			}
			if (Defs.isMulti && Defs.isInet && !StartAfterDisconnect)
			{
				AnalyticsStuff.LogMultiplayer();
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
	internal sealed class _003CLoadInGameGUI_003Ed__175 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private ResourceRequest _003Crequest_003E5__1;

		public NetworkStartTable _003C_003E4__this;

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
		public _003CLoadInGameGUI_003Ed__175(int _003C_003E1__state)
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
				_003Crequest_003E5__1 = Resources.LoadAsync("InGameGUI");
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.inGameGuiPrefab = _003Crequest_003E5__1.asset as GameObject;
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
	internal sealed class _003CWaitInterstitialRequestAndShowCoroutine_003Ed__188 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task<Ad> request;

		private Task<AdResult> _003Cfuture_003E5__1;

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
		public _003CWaitInterstitialRequestAndShowCoroutine_003Ed__188(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			Dictionary<string, string> eventParams;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Waiting until interstitial request is completed...");
				}
				goto IL_006c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_006c;
			case 2:
				_003C_003E1__state = -1;
				if (WeaponManager.sharedManager.myPlayer != null)
				{
					UnityEngine.Debug.LogWarning("Stop waiting: WeaponManager.sharedManager.myPlayer != null");
					return false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				if (NetworkStartTableNGUIController.sharedController.rewardWindow != null)
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Waiting until Reward panel is closed...");
					}
					goto IL_0148;
				}
				goto IL_0214;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0148;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0184;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0184;
			case 7:
				_003C_003E1__state = -1;
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log(string.Format("Waiting until Level up panel is closed if displayed ({0})...", new object[1] { ExpController.Instance.IsLevelUpShown }));
				}
				goto IL_01ee;
			case 8:
				_003C_003E1__state = -1;
				goto IL_01ee;
			case 9:
				_003C_003E1__state = -1;
				goto IL_0214;
			case 10:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01ee:
				if (ExpController.Instance.IsLevelUpShown)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 8;
					return true;
				}
				goto IL_0214;
				IL_006c:
				if (!((Task)request).IsCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (((Task)request).IsFaulted)
				{
					UnityEngine.Debug.LogWarning("Interstitial request after match failed: " + ((Exception)(object)((Task)request).Exception).InnerException.Message);
					return false;
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Interstitial request after match succeeded. Trying to show interstitial...");
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
				IL_0184:
				if (ExpController.Instance.WaitingForLevelUpView)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 6;
					return true;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 7;
				return true;
				IL_0214:
				if (ShopNGUIController.GuiActive)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 9;
					return true;
				}
				eventParams = new Dictionary<string, string>
				{
					{ "af_content_type", "Interstitial" },
					{ "af_content_id", "Interstitial (NetworkTable)" }
				};
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
				MenuBackgroundMusic.sharedMusic.Stop();
				_003Cfuture_003E5__1 = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string> { { "Context", "Multiplayer Table" } }, "NetworkStartTable.WaitInterstitialRequestAndShow()");
				break;
				IL_0148:
				if (NetworkStartTableNGUIController.sharedController.isRewardShow)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 5;
				return true;
			}
			if (!((Task)_003Cfuture_003E5__1).IsCompleted)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 10;
				return true;
			}
			MenuBackgroundMusic.sharedMusic.Start();
			if (((Task)_003Cfuture_003E5__1).IsFaulted)
			{
				UnityEngine.Debug.LogWarningFormat("Interstitial show after match failed: {0}", ((Exception)(object)((Task)_003Cfuture_003E5__1).Exception).InnerException.Message);
			}
			else
			{
				UnityEngine.Debug.LogFormat("Interstitial show finished with status {0}: {1}", _003Cfuture_003E5__1.Result.Status, _003Cfuture_003E5__1.Result.Message);
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

	public static bool StartAfterDisconnect = false;

	public static int matchPlayerID = 0;

	public static string matchRoomId = "";

	public string pixelBookID = "-1";

	public int playerUniqID = -1;

	private SaltedInt _scoreCommandFlag1 = new SaltedInt(818919);

	private SaltedInt _scoreCommandFlag2 = new SaltedInt(823016);

	public double timerFlag;

	private float maxTimerFlag = 150f;

	private float timerUpdateTimerFlag;

	private float maxTimerUpdateTimerFlag = 1f;

	public bool isShowAvard;

	public bool isShowFinished;

	private bool isEndedInMinigames;

	private int addExperience;

	public GameObject guiObj;

	public NetworkStartTableNGUIController networkStartTableNGUIController;

	public bool isRegimVidos;

	private int numberPlayerCun;

	private int numberPlayerCunId;

	public Player_move_c currentPlayerMoveCVidos;

	private bool oldIsZomming;

	private InGameGUI inGameGUI;

	public string playerVidosNick;

	public string playerVidosClanName;

	public Texture playerVidosClanTexture;

	public GameObject currentCamPlayer;

	public GameObject currentFPSPlayer;

	private GameObject currentBodyMech;

	public GameObject currentGameObjectPlayer;

	public bool isGoRandomRoom;

	public Texture mySkin;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private GameObject _playerPrefab;

	public GameObject tempCam;

	public GameObject zombieManagerPrefab;

	public Texture2D serverLeftTheGame;

	public ExperienceController experienceController;

	private int addCoins;

	public bool isDeadInMiniGame;

	private bool showMessagFacebook;

	private bool clickButtonFacebook;

	public bool isIwin;

	public int myCommand;

	public int myCommandOld;

	private bool isLocal;

	private bool isMine;

	private bool isCOOP;

	private bool isServer;

	private bool isCompany;

	private bool isMulti;

	private bool isInet;

	private float timeNotRunZombiManager;

	private bool isSendZaprosZombiManager;

	private bool isGetZaprosZombiManager;

	private ExperienceController expController;

	public Texture myClanTexture;

	public string myClanID = "";

	public string myClanName = "";

	public string myClanLeaderID = "";

	private LANBroadcastService lanScan;

	private bool isSetNewMapButton;

	public bool exitFromMenu;

	private GameObject inGameGuiPrefab;

	public List<infoClient> players = new List<infoClient>();

	public GUIStyle labelStyle;

	public GUIStyle messagesStyle;

	public GUIStyle ozidanieStyle;

	private Vector2 scrollPosition = Vector2.zero;

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public bool _showTable;

	public bool _isShowNickTable;

	public bool runGame = true;

	public GameObject[] zoneCreatePlayer;

	private GameObject _cam;

	public bool showDisconnectFromServer;

	public bool showDisconnectFromMasterServer;

	private float timerShow = -1f;

	public string NamePlayer = "Player";

	private SaltedInt _CountKills;

	public int oldCountKills;

	public ScoreTableItem[] oldPlayersList;

	public ScoreTableItem[] oldRedPlayersList;

	public ScoreTableItem[] oldBluePlayersList;

	private GameObject tc;

	public int scoreOld;

	public PhotonView photonView;

	private float timerSynchScore = -1f;

	private int commandWinner;

	private bool _canUserUseFacebookComposer;

	private bool _hasPublishPermission;

	private bool _hasPublishActions;

	private SaltedInt _score;

	private static System.Random _prng = new System.Random(19937);

	public int myRanks = 1;

	public Player_move_c myPlayerMoveC;

	private bool isHunger;

	private int _gameRating = -1;

	private ShopNGUIController _shopInstance;

	private bool notSendAnaliticStartBattle;

	private int playerCountInHunger;

	private bool isStartPlayerCoroutine;

	private Pauser _pauser;

	private Stopwatch _matchStopwatch = new Stopwatch();

	private static List<NetworkStartTable> _tabsBuffer = new List<NetworkStartTable>(10);

	private static List<ScoreTableItem> _scoresBuffer = new List<ScoreTableItem>(10);

	private int killCountMatch;

	private int deathCountMatch;

	public int scoreCommandFlag1
	{
		get
		{
			return _scoreCommandFlag1.Value;
		}
		set
		{
			_scoreCommandFlag1 = value;
		}
	}

	public int scoreCommandFlag2
	{
		get
		{
			return _scoreCommandFlag2.Value;
		}
		set
		{
			_scoreCommandFlag2 = value;
		}
	}

	public bool showTable
	{
		get
		{
			return _showTable;
		}
		set
		{
			_showTable = value;
			if (isMine)
			{
				Defs.showTableInNetworkStartTable = value;
			}
		}
	}

	public bool isShowNickTable
	{
		get
		{
			return _isShowNickTable;
		}
		set
		{
			_isShowNickTable = value;
			if (isMine)
			{
				Defs.showNickTableInNetworkStartTable = value;
			}
		}
	}

	public int CountKills
	{
		get
		{
			return _CountKills.Value;
		}
		set
		{
			_CountKills = new SaltedInt(_prng.Next(), value);
		}
	}

	public int score
	{
		get
		{
			return _score.Value;
		}
		set
		{
			_score = new SaltedInt(_prng.Next(), value);
		}
	}

	public int gameRating
	{
		get
		{
			if (!Defs.isMulti || !isMine)
			{
				return _gameRating;
			}
			return RatingSystem.instance.currentRating;
		}
		set
		{
			_gameRating = value;
		}
	}

	public static Vector2 ExperiencePosRanks
	{
		get
		{
			return new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	private string _SocialMessage()
	{
		int @int = Storager.getInt(Defs.COOPScore);
		bool flag = GameConnect.isCOOP;
		int int2 = Storager.getInt("Rating");
		string text = "http://goo.gl/dQMf4n";
		if (isIwin)
		{
			if (!flag)
			{
				return string.Format("I won the match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", new object[2] { int2, text });
			}
			return string.Format(" Now I have {0} score in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", new object[2] { @int, text });
		}
		if (!flag)
		{
			return string.Format("I played a match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", new object[2] { int2, text });
		}
		return string.Format("I received {0} points in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", new object[2] { @int, text });
	}

	private string _SocialSentSuccess(string SocialName)
	{
		return "Message was sent to " + SocialName;
	}

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
			return;
		}
		Prime31.Utils.logObject(result);
		showMessagFacebook = true;
		Invoke("hideMessag", 3f);
	}

	private void Awake()
	{
		isLocal = !Defs.isInet;
		isInet = Defs.isInet;
		isCOOP = GameConnect.gameMode == GameConnect.GameMode.TimeBattle;
		if (isInet)
		{
			isServer = PhotonNetwork.isMasterClient;
		}
		isMulti = Defs.isMulti;
		isCompany = GameConnect.gameMode == GameConnect.GameMode.TeamFight;
		isHunger = GameConnect.gameMode == GameConnect.GameMode.DeadlyGames;
		experienceController = ExperienceController.sharedController;
		if (GameConnect.gameMode == GameConnect.GameMode.TimeBattle)
		{
			string[] array = null;
			array = new string[10] { "1", "15", "14", "2", "3", "9", "11", "12", "10", "16" };
			for (int i = 0; i < array.Length; i++)
			{
				GameObject item = Resources.Load("Enemies/Enemy" + array[i] + "_go") as GameObject;
				zombiePrefabs.Add(item);
			}
		}
		if (GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
		{
			maxTimerFlag = (float)int.Parse(PhotonNetwork.room.customProperties[GameConnect.maxKillProperty].ToString()) * 60f;
		}
		photonView = PhotonView.Get(this);
		Initializer.networkTables.Add(this);
		if ((bool)photonView && photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
	}

	public void ImDeadInSpleef()
	{
		SetSceneCameraEnabled(true);
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		photonView.RPC("ImDeadInHungerGamesRPC", PhotonTargets.Others);
		isDeadInMiniGame = true;
		isIwin = true;
		try
		{
			AnalyticsStuff.SpleefWeaponsDeath(ItemDb.GetByPrefabName(WeaponManager.sharedManager.currentWeaponSounds.nameNoClone()).Tag);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in SpleefWeaponsDeath send: {0}", ex);
		}
	}

	public void ImDeadInHungerGames()
	{
		SetSceneCameraEnabled(true);
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		photonView.RPC("ImDeadInHungerGamesRPC", PhotonTargets.Others);
		isDeadInMiniGame = true;
	}

	[PunRPC]
	[RPC]
	public void ImDeadInHungerGamesRPC()
	{
		isDeadInMiniGame = true;
		if (GameConnect.isSpleef)
		{
			if (!WeaponManager.sharedManager.myNetworkStartTable.isDeadInMiniGame && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.stillAlive);
			}
			try
			{
				AnalyticsStuff.SpleefWeaponsSurvive(ItemDb.GetByPrefabName(WeaponManager.sharedManager.currentWeaponSounds.nameNoClone()).Tag);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpleefWeaponsSurvive send: {0}", ex);
			}
		}
	}

	public void setScoreFromGlobalGameController()
	{
		score = GlobalGameController.Score;
		SynhScore();
	}

	[RPC]
	[PunRPC]
	private void RunGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<NetworkStartTable>().runGame = true;
		}
	}

	public void RemoveShop(bool disable = true)
	{
		ShopTapReceiver.ShopClicked -= HandleShopButton;
		if (_shopInstance != null)
		{
			if (disable)
			{
				ShopNGUIController.GuiActive = false;
			}
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance = null;
		}
	}

	public void HandleShopButton()
	{
		NetworkStartTableNGUIController sharedController = NetworkStartTableNGUIController.sharedController;
		if ((!(sharedController != null) || (!(sharedController.goMapInEndGameButtons.FirstOrDefault((GoMapInEndGame button) => button.IsLeavingRoom) != null) && !(sharedController.goMapInEndGameButtonsDuel.FirstOrDefault((GoMapInEndGame button) => button.IsLeavingRoom) != null))) && _shopInstance == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled))
		{
			_shopInstance = ShopNGUIController.sharedShop;
			if (_shopInstance != null)
			{
				_shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				_shopInstance.resumeAction = HandleResumeFromShop;
			}
			else
			{
				UnityEngine.Debug.LogWarning("sharedShop == null");
			}
		}
	}

	public void HandleResumeFromShop()
	{
		if (_shopInstance != null)
		{
			expController.isShowRanks = true;
			ShopNGUIController.GuiActive = false;
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance = null;
		}
	}

	public void BackButtonPress()
	{
		if (ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		NetworkStartTableNGUIController sharedController = NetworkStartTableNGUIController.sharedController;
		if (sharedController != null && sharedController.CheckHideInternalPanel())
		{
			return;
		}
		networkStartTableNGUIController.shopAnchor.SetActive(false);
		RemoveShop();
		if (!isInet)
		{
			if (isServer)
			{
				GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
			}
			ActivityIndicator.IsActiveIndicator = false;
			ConnectScene.Local();
		}
		else
		{
			ActivityIndicator.IsActiveIndicator = false;
			Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
			GameConnect.Disconnect();
		}
	}

	public void StartPlayerButtonClick(int _command)
	{
		if (!notSendAnaliticStartBattle)
		{
			int num = PlayerPrefs.GetInt("CountMatch", 0) + 1;
			if (!notSendAnaliticStartBattle && num <= 5)
			{
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Battle_Start, num);
			}
			else
			{
				notSendAnaliticStartBattle = true;
			}
		}
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideEndInterface();
		}
		myCommand = _command;
		StartPlayerMatch();
	}

	public void StartPlayerMatch()
	{
		Initializer.Instance.isLastMatchWin = false;
		isShowNickTable = false;
		CountKills = 0;
		score = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		SynhCommand();
		SynhCountKills();
		SynhScore();
		startPlayer();
	}

	public void RandomRoomClickBtnInHunger()
	{
		ItemPrice price = new ItemPrice(BalanceController.ParametersForMiniGameType(GameConnect.gameMode).TicketsPrice, "TicketsCurrency");
		Action onSuccess = delegate
		{
			try
			{
				isGoRandomRoom = true;
				if (isRegimVidos)
				{
					isRegimVidos = false;
					if (inGameGUI != null)
					{
						inGameGUI.ResetScope();
					}
				}
				Defs.typeDisconnectGame = Defs.DisconectGameType.RandomGameInHunger;
				PhotonNetwork.LeaveRoom();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in RandomRoomClickBtnInHunger: {0}", ex);
			}
		};
		ShopNGUIController.TryToBuy(networkStartTableNGUIController.allInterfaceContainer, price, onSuccess, null, null, null, null, null, false, true);
	}

	public void RandomRoomClickBtnInDuel()
	{
		isGoRandomRoom = true;
		Defs.typeDisconnectGame = Defs.DisconectGameType.RandomGameInDuel;
		PhotonNetwork.LeaveRoom();
	}

	public void SetRegimVidos(bool _isRegimVidos)
	{
		bool flag = isRegimVidos;
		isRegimVidos = _isRegimVidos;
		if (isRegimVidos != flag && !isRegimVidos && inGameGUI != null)
		{
			inGameGUI.ResetScope();
		}
	}

	private void playersTable()
	{
		if (!isShowAvard)
		{
			ShopTapReceiver.AddClickHndIfNotExist(HandleShopButton);
			networkStartTableNGUIController.shopAnchor.SetActive((!isShowFinished || isHunger) && _shopInstance == null && (expController == null || !expController.isShowNextPlashka));
			if (_shopInstance != null)
			{
				_shopInstance.SetInGame(false);
			}
		}
	}

	public void PostFacebookBtnClick()
	{
		UnityEngine.Debug.Log("show facebook dialog");
		FacebookController.ShowPostDialog();
	}

	public void PostTwitterBtnClick()
	{
		if (TwitterController.Instance != null)
		{
			TwitterController.Instance.PostStatusUpdate(_SocialMessage());
		}
	}

	private IEnumerator StartPlayerCoroutine()
	{
		Defs.inRespawnWindow = false;
		if (Defs.isMulti && Defs.isInet)
		{
			photonView.RPC("SynchGameRating", PhotonTargets.Others, gameRating);
		}
		if (Defs.isMulti && GameConnect.isDuel)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate
			{
				sendMySkin();
			};
		}
		if (isStartPlayerCoroutine)
		{
			yield break;
		}
		isStartPlayerCoroutine = true;
		while (Defs.isMulti && Defs.isInet && PhotonNetwork.time > -0.01 && PhotonNetwork.time < 0.01)
		{
			yield return null;
		}
		isStartPlayerCoroutine = false;
		if (Defs.isMulti && !GameConnect.isMiniGame && !GameConnect.isDuel)
		{
			TimeGameController.sharedController.StartMatch();
		}
		if (GameConnect.isDaterRegim)
		{
			int timeGame = ((!Defs.isInet) ? ((!PlayerPrefs.GetString("MaxKill", "9").Equals("")) ? int.Parse(PlayerPrefs.GetString("MaxKill", "5")) : 5) : ((int)PhotonNetwork.room.customProperties[GameConnect.maxKillProperty]));
			AnalyticsStuff.LogSandboxTimeGamePopularity(timeGame, true);
		}
		_matchStopwatch.Start();
		StoreKitEventListener.State.PurchaseKey = "In game";
		StoreKitEventListener.State.Parameters.Clear();
		if (networkStartTableNGUIController != null)
		{
			networkStartTableNGUIController.shopAnchor.SetActive(false);
		}
		RemoveShop(!BankController.Instance.InterfaceEnabled);
		if (GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
		{
			timerFlag = maxTimerFlag;
		}
		if (myRanks != expController.currentLevel)
		{
			SetRanks();
		}
		_cam = GameObject.FindGameObjectWithTag("CamTemp");
		SetSceneCameraEnabled(false);
		_weaponManager.useCam = null;
		zoneCreatePlayer = GameObject.FindGameObjectsWithTag((GameConnect.gameMode == GameConnect.GameMode.TimeBattle) ? "MultyPlayerCreateZoneCOOP" : ((GameConnect.gameMode == GameConnect.GameMode.TeamFight) ? ("MultyPlayerCreateZoneCommand" + myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.FlagCapture) ? ("MultyPlayerCreateZoneFlagCommand" + myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.CapturePoints) ? ("MultyPlayerCreateZonePointZone" + myCommand) : ((GameConnect.gameMode == GameConnect.GameMode.Duel) ? ("MultyPlayerCreateZoneDuel" + DuelController.instance.myRespawnPoints) : "MultyPlayerCreateZone")))));
		int num = 0;
		if (GameConnect.isMiniGame)
		{
			if (GameConnect.isHunger && !StartAfterDisconnect)
			{
				_weaponManager.Reset();
			}
			int iD = photonView.owner.ID;
			for (int i = 0; i < Initializer.networkTables.Count; i++)
			{
				PhotonPlayer owner = Initializer.networkTables[i].GetComponent<PhotonView>().owner;
				if (owner != null && owner.ID < iD)
				{
					num++;
				}
			}
			if (num >= zoneCreatePlayer.Length)
			{
				num = zoneCreatePlayer.Length - 1;
				UnityEngine.Debug.LogError("Not enough spawn points for players! Add more");
			}
			if (GameConnect.isHunger)
			{
				int num2 = num;
				for (int j = 0; j < zoneCreatePlayer.Length; j++)
				{
					if (zoneCreatePlayer[j].GetComponent<NumberZone>().numberZone == num)
					{
						num = j;
						break;
					}
				}
				if (!StartAfterDisconnect)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag("ChestCreateZone");
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k].GetComponent<NumberZone>().numberZone == num2)
						{
							GameObject gameObject = array[k];
							photonView.RPC("CreateChestRPC", PhotonTargets.MasterClient, gameObject.transform.position, gameObject.transform.rotation);
							break;
						}
					}
				}
				playerCountInHunger = Initializer.networkTables.Count;
			}
		}
		GameObject gameObject2 = zoneCreatePlayer[GameConnect.isMiniGame ? num : UnityEngine.Random.Range(0, zoneCreatePlayer.Length - 1)];
		BoxCollider component = gameObject2.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject2.transform.localScale.x, component.size.z * gameObject2.transform.localScale.z);
		Rect rect = new Rect(gameObject2.transform.position.x - vector.x / 2f, gameObject2.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = ((!isHunger) ? new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject2.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height)) : gameObject2.transform.position);
		Quaternion rotation = gameObject2.transform.rotation;
		if (StartAfterDisconnect && GlobalGameController.healthMyPlayer > 0f)
		{
			position = GlobalGameController.posMyPlayer;
		}
		GameObject gameObject3;
		if (isInet)
		{
			gameObject3 = PhotonNetwork.Instantiate("Player", position, rotation, 0);
		}
		else
		{
			if (_playerPrefab == null)
			{
				_playerPrefab = Resources.Load("Player") as GameObject;
			}
			gameObject3 = null;
		}
		NickLabelController.currentCamera = gameObject3.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
		_weaponManager.myPlayer = gameObject3;
		_weaponManager.myPlayerMoveC = gameObject3.GetComponent<SkinName>().playerMoveC;
		if (!isInet && isServer)
		{
			UnityEngine.Debug.Log("networkView.RPC(RunGame, RPCMode.OthersBuffered);");
		}
		Initializer.Instance.SetupObjectThatNeedsPlayer();
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideStartInterface();
		}
		showTable = false;
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.BuffsActive(!GameConnect.isDaterRegim && !GameConnect.isHunger && !GameConnect.isCOOP && Defs.isMulti && Defs.isInet && !LocalOrPasswordRoom());
		}
		else
		{
			KillRateCheck.instance.SetActive(!GameConnect.isDaterRegim && !GameConnect.isHunger && !GameConnect.isCOOP && Defs.isMulti && Defs.isInet && !LocalOrPasswordRoom() && WeaponManager.sharedManager._currentFilterMap == 0, TimeGameController.sharedController != null && TimeGameController.sharedController.timerToEndMatch > 30.0);
		}
		if (Defs.isMulti && Defs.isInet && !StartAfterDisconnect)
		{
			AnalyticsStuff.LogMultiplayer();
		}
	}

	public void startPlayer()
	{
		StartCoroutine(StartPlayerCoroutine());
	}

	[RPC]
	[PunRPC]
	public void CreateChestRPC(Vector3 pos, Quaternion rot)
	{
		PhotonNetwork.InstantiateSceneObject("HungerGames/Chest", pos, rot, 0, null);
	}

	[PunRPC]
	[RPC]
	private void SetPlayerUniqID(int uniqID)
	{
		playerUniqID = uniqID;
	}

	[PunRPC]
	[RPC]
	private void SetPixelBookID(string _pixelBookID)
	{
		pixelBookID = _pixelBookID;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if ((bool)photonView && photonView.isMine)
		{
			if (GameConnect.isFlag && !isShowFinished)
			{
				photonView.RPC("SynchScoreCommandRPC", player, 1, scoreCommandFlag1);
				photonView.RPC("SynchScoreCommandRPC", player, 2, scoreCommandFlag2);
			}
			SynhCommand(player);
			SynhCountKills(player);
			SendSynhScore(player);
			if (Defs.isMulti && Defs.isInet && isMine)
			{
				photonView.RPC("SynchGameRating", player, gameRating);
			}
		}
	}

	public void SetNewNick()
	{
		NamePlayer = ProfileController.GetPlayerNameOrDefault();
		if (Defs.isInet)
		{
			PhotonNetwork.playerName = NamePlayer;
			photonView.RPC("SynhNickNameRPC", PhotonTargets.OthersBuffered, NamePlayer);
		}
	}

	[RPC]
	[PunRPC]
	private void SynhNickNameRPC(string _nick)
	{
		if (!isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(_nick + " " + LocalizationStore.Get("Key_0995"), new Color(1f, 0.7f, 0f));
		}
		NamePlayer = _nick;
	}

	public void UpdateRanks()
	{
		if (myRanks != expController.currentLevel)
		{
			SetRanks();
		}
	}

	public void SetRanks()
	{
		myRanks = expController.currentLevel;
		if (Defs.isInet)
		{
			photonView.RPC("SynhRanksRPC", PhotonTargets.OthersBuffered, myRanks);
		}
	}

	[RPC]
	[PunRPC]
	private void SynhRanksRPC(int _ranks)
	{
		myRanks = _ranks;
	}

	public void SynhCommand(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				photonView.RPC("SynhCommandRPC", PhotonTargets.Others, myCommand, myCommandOld);
			}
			else
			{
				photonView.RPC("SynhCommandRPC", player, myCommand, myCommandOld);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SynhCommandRPC(int _command, int _oldCommand)
	{
		myCommand = _command;
		myCommandOld = _oldCommand;
		if (myPlayerMoveC != null)
		{
			myPlayerMoveC.myCommand = myCommand;
			if (Initializer.redPlayers.Contains(myPlayerMoveC) && myCommand == 1)
			{
				Initializer.redPlayers.Remove(myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(myPlayerMoveC) && myCommand == 2)
			{
				Initializer.bluePlayers.Remove(myPlayerMoveC);
			}
			if (myCommand == 1 && !Initializer.bluePlayers.Contains(myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(myPlayerMoveC);
			}
			if (myCommand == 2 && !Initializer.redPlayers.Contains(myPlayerMoveC))
			{
				Initializer.redPlayers.Add(myPlayerMoveC);
			}
			if (myPlayerMoveC.myNickLabelController != null)
			{
				myPlayerMoveC.myNickLabelController.SetCommandColor(myCommand);
			}
		}
	}

	public void SynhCountKills(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				photonView.RPC("SynhCountKillsRPC", PhotonTargets.Others, CountKills, oldCountKills);
			}
			else
			{
				photonView.RPC("SynhCountKillsRPC", player, CountKills, oldCountKills);
			}
		}
	}

	[PunRPC]
	[RPC]
	private void SynhCountKillsRPC(int _countKills, int _oldCountKills)
	{
		CountKills = _countKills;
		oldCountKills = _oldCountKills;
	}

	public void SynhScore()
	{
		if (timerSynchScore < 0f)
		{
			timerSynchScore = 1f;
		}
	}

	public void ResetOldScore()
	{
		scoreOld = 0;
		score = 0;
		SynhScore();
		oldCountKills = 0;
		CountKills = 0;
		SynhCountKills();
		GetMyTeam();
	}

	public void SendSynhScore(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				photonView.RPC("SynhScoreRPC", PhotonTargets.Others, score, scoreOld);
			}
			else
			{
				photonView.RPC("SynhScoreRPC", player, score, scoreOld);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SynhScoreRPC(int _score, int _oldScore)
	{
		score = _score;
		scoreOld = _oldScore;
	}

	private void hideMessag()
	{
		showMessagFacebook = false;
	}

	private void Start()
	{
		lanScan = GetComponent<LANBroadcastService>();
		try
		{
			StartUnsafe();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		if (isMine && !TrainingController.TrainingCompleted)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Table_Battle);
		}
	}

	private void StartUnsafe()
	{
		if (StartAfterDisconnect && GlobalGameController.CheckForNewMatch())
		{
			StartAfterDisconnect = false;
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (isMulti && !isLocal)
		{
			isMine = photonView.isMine;
		}
		if (isMine)
		{
			StartCoroutine(LoadInGameGUI());
			networkStartTableNGUIController = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("NetworkStartTableNGUI"))).GetComponent<NetworkStartTableNGUIController>();
			_cam = GameObject.FindGameObjectWithTag("CamTemp");
			StoreKitEventListener.State.PurchaseKey = "Start table";
			if (FriendsController.sharedController.clanLogo != null && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				myClanTexture = texture2D;
				if (isInet)
				{
					photonView.RPC("SetMyClanTexture", PhotonTargets.AllBuffered, FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName, FriendsController.sharedController.clanLeaderID);
				}
			}
			Invoke("GetMyTeam", 1.5f);
			if (Defs.isMulti && Defs.isInet)
			{
				photonView.RPC("SynchGameRating", PhotonTargets.Others, gameRating);
			}
			if (GameConnect.isDuel)
			{
				DuelController.instance.StartDuelMode();
			}
		}
		expController = ExperienceController.sharedController;
		expController.posRanks = ExperiencePosRanks;
		_weaponManager = WeaponManager.sharedManager;
		if (isMulti && isMine)
		{
			if (Defs.isInet && !matchRoomId.Equals(PhotonNetwork.room.name))
			{
				matchPlayerID = photonView.ownerId;
				matchRoomId = PhotonNetwork.room.name;
			}
			if (!StartAfterDisconnect)
			{
				if (GameConnect.isMiniGame && (MiniGamesController.Instance.playerDead || MiniGamesController.Instance.isEnd))
				{
					if (Defs.isInet && NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons();
						isSetNewMapButton = true;
					}
					isDeadInMiniGame = MiniGamesController.Instance.playerDead;
					if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.ShowEndInterface("", 0);
					}
				}
				else if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.ShowStartInterface();
				}
				showTable = true;
			}
			else
			{
				showTable = GlobalGameController.showTableMyPlayer;
				isDeadInMiniGame = GlobalGameController.imDeadInHungerGame;
				if (showTable || (GameConnect.isMiniGame && MiniGamesController.Instance.isEnd) || (GameConnect.isDuel && DuelController.instance.roomStatus == DuelController.RoomStatus.None))
				{
					if ((!isDeadInMiniGame && !GameConnect.isMiniGame) || !MiniGamesController.Instance.isEnd)
					{
						if (NetworkStartTableNGUIController.sharedController != null)
						{
							NetworkStartTableNGUIController.sharedController.ShowStartInterface();
						}
					}
					else
					{
						if (Defs.isInet && NetworkStartTableNGUIController.sharedController != null)
						{
							NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons();
							isSetNewMapButton = true;
						}
						if (NetworkStartTableNGUIController.sharedController != null)
						{
							NetworkStartTableNGUIController.sharedController.ShowEndInterface("", 0);
						}
					}
				}
				else
				{
					if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.HideStartInterface();
					}
					Invoke("startPlayer", 0.1f);
				}
			}
			NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
			tempCam.SetActive(true);
			string namePlayer = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			NamePlayer = namePlayer;
			pixelBookID = FriendsController.sharedController.id;
			if (GameConnect.isMiniGame && isInet)
			{
				photonView.RPC("SetPlayerUniqID", PhotonTargets.OthersBuffered, matchPlayerID);
			}
			if (isInet)
			{
				photonView.RPC("SetPixelBookID", PhotonTargets.OthersBuffered, pixelBookID);
			}
			if (isServer && !isInet)
			{
				lanScan.serverMessage.name = PlayerPrefs.GetString("ServerName");
				lanScan.serverMessage.map = PlayerPrefs.GetString("MapName");
				lanScan.serverMessage.connectedPlayers = 0;
				lanScan.serverMessage.playerLimit = int.Parse(PlayerPrefs.GetString("PlayersLimits"));
				lanScan.serverMessage.comment = PlayerPrefs.GetString("MaxKill");
				lanScan.serverMessage.regim = (int)GameConnect.gameMode;
				lanScan.StartAnnounceBroadCasting();
				UnityEngine.Debug.Log("lanScan.serverMessage.regim=" + lanScan.serverMessage.regim);
			}
			else
			{
				lanScan.enabled = false;
			}
			if (StartAfterDisconnect)
			{
				CountKills = GlobalGameController.CountKills;
				score = GlobalGameController.Score;
				Invoke("synchState", 1f);
			}
			else
			{
				CountKills = -1;
				score = -1;
				GlobalGameController.CountKills = 0;
				GlobalGameController.Score = 0;
				Invoke("synchState", 1f);
			}
			expController = ExperienceController.sharedController;
			SetNewNick();
			SetRanks();
			SynhCountKills();
			SynhScore();
			sendMySkin();
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate
			{
				sendMySkin();
			};
		}
		else
		{
			showTable = false;
		}
		stopwatch.Stop();
	}

	private void GetMyTeam()
	{
		if (isMine && !LocalOrPasswordRoom() && (GameConnect.gameMode == GameConnect.GameMode.CapturePoints || GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture))
		{
			myCommand = GetMyCommandOnStart();
			SynhCommand();
		}
	}

	[PunRPC]
	[RPC]
	private void SetMyClanTexture(string str, string _clanID, string _clanName, string _clanLeaderId)
	{
		try
		{
			byte[] data = Convert.FromBase64String(str);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			myClanTexture = texture2D;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.Log(message);
		}
		myClanID = _clanID;
		myClanName = _clanName;
		myClanLeaderID = _clanLeaderId;
	}

	[RPC]
	[PunRPC]
	private void SetMySkinFromName(string _skinName)
	{
		byte[] array = (SkinsController.standardSkinsIds.Contains(_skinName) ? SkinsController.skinsForPers[_skinName] : SkinsController.skinsForPers["0"]).EncodeToPNG();
		SetMySkin(array);
	}

	[PunRPC]
	[RPC]
	private void SetMySkin(byte[] _skinByte)
	{
		if (photonView == null || !Defs.isMulti)
		{
			return;
		}
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		mySkin = texture2D;
		foreach (Player_move_c player in Initializer.players)
		{
			if (player.mySkinName.photonView.owner != null && player.mySkinName.photonView.owner.Equals(photonView.owner))
			{
				if (player.myNetworkStartTable == null)
				{
					player.setMyTamble(base.gameObject);
					continue;
				}
				player._skin = mySkin;
				player.SetTextureForBodyPlayer(player._skin);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetMySkinLocal(string str1, string str2)
	{
		byte[] data = Convert.FromBase64String(str1 + str2);
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		mySkin = texture2D;
	}

	private IEnumerator LoadInGameGUI()
	{
		ResourceRequest request = Resources.LoadAsync("InGameGUI");
		while (!request.isDone)
		{
			yield return null;
		}
		inGameGuiPrefab = request.asset as GameObject;
	}

	public void sendMySkin()
	{
		if (isInet && SkinsController.standardSkinsIds.Contains(SkinsController.currentSkinNameForPers))
		{
			UnityEngine.Debug.Log("SetMySkinFromName " + SkinsController.currentSkinNameForPers);
			photonView.RPC("SetMySkinFromName", PhotonTargets.AllBuffered, SkinsController.currentSkinNameForPers);
			return;
		}
		mySkin = SkinsController.currentSkinForPers;
		byte[] array = (mySkin as Texture2D).EncodeToPNG();
		if (isInet)
		{
			photonView.RPC("SetMySkin", PhotonTargets.AllBuffered, array);
		}
	}

	public void ResetCamPlayer(int _nextPrev = 0)
	{
		if (_nextPrev != 0 && Initializer.players.Count == 1)
		{
			return;
		}
		if (Initializer.players.Count > 0)
		{
			if (_nextPrev == 0)
			{
				numberPlayerCun = UnityEngine.Random.Range(0, Initializer.players.Count);
				numberPlayerCunId = Initializer.players[numberPlayerCun].mySkinName.photonView.ownerId;
			}
			if (_nextPrev == 1)
			{
				int num = 10000000;
				int num2 = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c player in Initializer.players)
				{
					int ownerId = player.mySkinName.photonView.ownerId;
					if (ownerId < num2)
					{
						num2 = ownerId;
					}
					if (ownerId > numberPlayerCunId && ownerId < num)
					{
						num = ownerId;
					}
				}
				if (num == 10000000)
				{
					numberPlayerCunId = num2;
				}
				else
				{
					numberPlayerCunId = num;
				}
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (Initializer.players[i].mySkinName.photonView.ownerId == numberPlayerCunId)
					{
						numberPlayerCun = i;
						break;
					}
				}
			}
			if (_nextPrev == -1)
			{
				int num3 = -1;
				int num4 = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c player2 in Initializer.players)
				{
					int ownerId2 = player2.mySkinName.photonView.ownerId;
					if (ownerId2 > num4)
					{
						num4 = ownerId2;
					}
					if (ownerId2 < numberPlayerCunId)
					{
						num3 = ownerId2;
					}
				}
				if (num3 == -1)
				{
					numberPlayerCunId = num4;
				}
				else
				{
					numberPlayerCunId = num3;
				}
				for (int j = 0; j < Initializer.players.Count; j++)
				{
					if (Initializer.players[j].mySkinName.photonView.ownerId == numberPlayerCunId)
					{
						numberPlayerCun = j;
						break;
					}
				}
			}
			if (currentCamPlayer != null)
			{
				currentCamPlayer.SetActive(false);
				if (!currentPlayerMoveCVidos.isMechActive)
				{
					currentFPSPlayer.SetActive(true);
				}
				if (currentBodyMech != null)
				{
					currentBodyMech.SetActive(true);
				}
				Player_move_c.SetLayerRecursively(currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
				currentCamPlayer.transform.parent.GetComponent<PlayerSynchStream>().sglajEnabledVidos = false;
				currentCamPlayer = null;
				currentFPSPlayer = null;
				currentBodyMech = null;
				currentGameObjectPlayer = null;
				currentPlayerMoveCVidos = null;
			}
			SkinName mySkinName = Initializer.players[numberPlayerCun].mySkinName;
			if (mySkinName.firstPersonControl.isFirstPersonCamera)
			{
				mySkinName.camPlayer.SetActive(true);
			}
			playerVidosNick = mySkinName.NickName;
			playerVidosClanName = mySkinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanName;
			playerVidosClanTexture = mySkinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanTexture;
			currentPlayerMoveCVidos = mySkinName.playerMoveC;
			currentCamPlayer = mySkinName.camPlayer;
			currentFPSPlayer = mySkinName.FPSplayerObject;
			currentBodyMech = (GameConnect.isDaterRegim ? mySkinName.playerMoveC.mechBearBody : ((mySkinName.playerMoveC.currentMech != null) ? mySkinName.playerMoveC.currentMech.body : null));
			Initializer.players[numberPlayerCun].myPersonNetwork.sglajEnabledVidos = true;
			currentGameObjectPlayer = mySkinName.playerGameObject;
			currentFPSPlayer.SetActive(false);
			if (currentBodyMech != null)
			{
				currentBodyMech.SetActive(false);
			}
			NickLabelController.currentCamera = mySkinName.camPlayer.GetComponent<Camera>();
			Player_move_c.SetLayerRecursively(currentGameObjectPlayer.transform.GetChild(0).gameObject, 9);
		}
		else
		{
			SetSceneCameraEnabled(true);
			showTable = true;
			isRegimVidos = false;
			NickLabelController.currentCamera = _cam.GetComponent<Camera>();
			if (inGameGUI != null)
			{
				inGameGUI.ResetScope();
			}
		}
	}

	private int GetMyCommandOnStart()
	{
		if (myCommand > 0)
		{
			return myCommand;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (Initializer.networkTables[i].myCommand == 1)
			{
				num++;
			}
			if (Initializer.networkTables[i].myCommand == 2)
			{
				num2++;
			}
		}
		if (num2 < num)
		{
			return 2;
		}
		if (num2 > num)
		{
			return 1;
		}
		float num3 = (ABTestController.useBuffSystem ? BuffSystem.instance.GetKillrateByInteractions() : KillRateCheck.instance.GetKillRate());
		int winningTeam = GetWinningTeam();
		if (winningTeam == 0)
		{
			return UnityEngine.Random.Range(1, 3);
		}
		if (num3 < 1f)
		{
			return winningTeam;
		}
		if (winningTeam != 1)
		{
			return 1;
		}
		return 2;
	}

	private void ReplaceCommand()
	{
		myCommand = ((myCommand != 1) ? 1 : 2);
		SynhCommand();
		score = 0;
		CountKills = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.countKills = 0;
			WeaponManager.sharedManager.myPlayerMoveC.myCommand = myCommand;
			WeaponManager.sharedManager.myPlayerMoveC.myBaza = null;
			WeaponManager.sharedManager.myPlayerMoveC.myFlag = null;
			WeaponManager.sharedManager.myPlayerMoveC.enemyFlag = null;
			if (Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && myCommand == 1)
			{
				Initializer.redPlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && myCommand == 2)
			{
				Initializer.bluePlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (myCommand == 1 && !Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (myCommand == 2 && !Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.redPlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
		}
	}

	private void Update()
	{
		if (isMine)
		{
			if (inGameGUI == null)
			{
				inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (timerSynchScore > 0f)
			{
				timerSynchScore -= Time.deltaTime;
				if (timerSynchScore < 0f)
				{
					SendSynhScore();
				}
			}
			bool flag = isShowNickTable || showDisconnectFromServer || showDisconnectFromMasterServer || showTable || showMessagFacebook;
			if (guiObj.activeSelf != flag)
			{
				guiObj.SetActive(flag);
			}
			if (inGameGUI == null)
			{
				inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (_pauser == null)
			{
				_pauser = Pauser.sharedPauser;
			}
			if (ShopNGUIController.GuiActive || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled))
			{
				expController.isShowRanks = SkinEditorController.sharedController == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled);
			}
			else if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
			{
				expController.isShowRanks = false;
			}
			else if (_pauser != null && _pauser.paused)
			{
				if (PauseNGUIController.sharedController != null)
				{
					expController.isShowRanks = !PauseNGUIController.sharedController.SettingsJoysticksPanel.activeInHierarchy;
				}
			}
			else if ((showTable || isShowNickTable) && !isRegimVidos && _shopInstance == null && !LoadingInAfterGame.isShowLoading && !isGoRandomRoom)
			{
				expController.isShowRanks = !isShowFinished && (!(networkStartTableNGUIController != null) || networkStartTableNGUIController.rentScreenPoint.childCount == 0);
			}
			else
			{
				expController.isShowRanks = false;
			}
			if (isRegimVidos && isDeadInMiniGame && _cam.activeInHierarchy && Initializer.players.Count > 0)
			{
				SetSceneCameraEnabled(false);
				ResetCamPlayer();
			}
			if (isRegimVidos && isDeadInMiniGame && currentCamPlayer == null)
			{
				ResetCamPlayer();
			}
			if (!isRegimVidos && isDeadInMiniGame && currentCamPlayer != null)
			{
				currentCamPlayer.SetActive(false);
				if (!currentPlayerMoveCVidos.isMechActive)
				{
					currentFPSPlayer.SetActive(true);
				}
				if (currentBodyMech != null)
				{
					currentBodyMech.SetActive(true);
				}
				currentCamPlayer = null;
				currentFPSPlayer = null;
				currentBodyMech = null;
				SetSceneCameraEnabled(true);
			}
			if (isRegimVidos && inGameGUI != null && currentPlayerMoveCVidos.isZooming != oldIsZomming)
			{
				oldIsZomming = currentPlayerMoveCVidos.isZooming;
				if (oldIsZomming)
				{
					string text = "";
					float fieldOfView = 60f;
					if (currentGameObjectPlayer.transform.childCount > 0)
					{
						try
						{
							text = ItemDb.GetByPrefabName(currentGameObjectPlayer.transform.GetChild(0).name.Replace("(Clone)", "")).Tag;
						}
						catch (Exception ex)
						{
							if (Application.isEditor)
							{
								UnityEngine.Debug.LogWarning("Exception  tagWeapon = ItemDb.GetByPrefabName(currentGameObjectPlayer.transform.GetChild(0).name.Replace(\"(Clone)\",\"\")).Tag:  " + ex);
							}
						}
						fieldOfView = currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().fieldOfViewZomm;
					}
					if (!text.Equals(""))
					{
						inGameGUI.SetScopeForWeapon(string.Concat(currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().scopeNum));
					}
					currentPlayerMoveCVidos.myCamera.fieldOfView = fieldOfView;
					currentPlayerMoveCVidos.gunCamera.fieldOfView = 1f;
				}
				else
				{
					currentPlayerMoveCVidos.myCamera.fieldOfView = 44f;
					currentPlayerMoveCVidos.gunCamera.fieldOfView = 75f;
					inGameGUI.ResetScope();
				}
			}
			if (GameConnect.isFlag || GameConnect.isCompany || GameConnect.isCapturePoints)
			{
				if (Defs.isInet && myCommand > 0)
				{
					int num = 0;
					for (int i = 0; i < Initializer.networkTables.Count; i++)
					{
						if (Initializer.networkTables[i] != null && Initializer.networkTables[i].myCommand == myCommand)
						{
							num++;
						}
					}
					if (num > 5)
					{
						int num2 = -1;
						for (int j = 0; j < Initializer.networkTables.Count; j++)
						{
							if (Initializer.networkTables[j] != null && Initializer.networkTables[j].myCommand == myCommand && Initializer.networkTables[j].photonView.ownerId > num2)
							{
								num2 = Initializer.networkTables[j].photonView.ownerId;
							}
						}
						if (num2 == photonView.ownerId)
						{
							ReplaceCommand();
						}
					}
				}
				if (GameConnect.isFlag)
				{
					timerFlag = TimeGameController.sharedController.timerToEndMatch;
					if (timerFlag < 0.0)
					{
						timerFlag = 0.0;
					}
					if (timerFlag < 0.10000000149011612)
					{
						if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.enabled)
						{
							WeaponManager.sharedManager.myPlayerMoveC.enabled = false;
							InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
							Invoke("ClearScoreCommandInFlagGame", 0.5f);
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable[GameConnect.timeProperty] = -9000000.0;
							PhotonNetwork.room.SetCustomProperties(hashtable);
							if (scoreCommandFlag1 > scoreCommandFlag2)
							{
								win("", 1, scoreCommandFlag1, scoreCommandFlag2);
							}
							else if (scoreCommandFlag1 < scoreCommandFlag2)
							{
								win("", 2, scoreCommandFlag1, scoreCommandFlag2);
							}
							else
							{
								win("", 0, scoreCommandFlag1, scoreCommandFlag2);
							}
						}
					}
					else if (inGameGUI != null && inGameGUI.message_draw.activeSelf)
					{
						inGameGUI.message_draw.SetActive(false);
					}
				}
			}
			if (GameConnect.isFlag && isInet && PhotonNetwork.isMasterClient && Initializer.flag1 == null)
			{
				AddFlag();
			}
		}
		if (!isLocal && isMine)
		{
			GlobalGameController.showTableMyPlayer = showTable;
			GlobalGameController.imDeadInHungerGame = isDeadInMiniGame;
		}
		if (isLocal && isServer && lanScan != null)
		{
			lanScan.serverMessage.connectedPlayers = GameObject.FindGameObjectsWithTag("NetworkTable").Length;
		}
		if (timerShow >= 0f)
		{
			timerShow -= Time.deltaTime;
			if (timerShow < 0f)
			{
				ActivityIndicator.IsActiveIndicator = false;
				ConnectScene.Local();
			}
		}
	}

	public void WinInHunger()
	{
		if (!isIwin)
		{
			isIwin = true;
			photonView.RPC("winInHungerRPC", PhotonTargets.AllBuffered, NamePlayer);
		}
	}

	public void WinInSpleef()
	{
		if (isIwin)
		{
			return;
		}
		if (GameConnect.isSpleef)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.spleefWinner, Mathf.RoundToInt(MiniGamesController.Instance.gameTimer));
			}
			try
			{
				AnalyticsStuff.SpleefTime(MiniGamesController.Instance.timeInGame);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpleefTime send: {0}", ex);
			}
		}
		isIwin = true;
		Invoke("WinInSpleefEnd", 2.5f);
	}

	public void WinInSpleefEnd()
	{
		photonView.RPC("winInHungerRPC", PhotonTargets.AllBuffered, NamePlayer);
	}

	[RPC]
	[PunRPC]
	public void winInHungerRPC(string winner)
	{
		MiniGamesController.Instance.isEnd = true;
		if (_weaponManager != null && _weaponManager.myTable != null)
		{
			_weaponManager.myTable.GetComponent<NetworkStartTable>().win(winner);
		}
	}

	public static void IncreaseTimeInMode(int mode, double minutes)
	{
		if (!(ExperienceController.sharedController != null))
		{
			return;
		}
		string key = mode.ToString();
		string key2 = "Statistics.TimeInMode.Level" + ExperienceController.sharedController.currentLevel;
		if (PlayerPrefs.HasKey(key2))
		{
			string @string = PlayerPrefs.GetString(key2, "{}");
			UnityEngine.Debug.Log("Time in mode string:    " + @string);
			try
			{
				Dictionary<string, object> dictionary = (Rilisoft.MiniJson.Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				object value;
				if (dictionary.TryGetValue(key, out value))
				{
					double num = Convert.ToDouble(value) + minutes;
					dictionary[key] = num;
				}
				else
				{
					dictionary.Add(key, minutes);
				}
				string value2 = Rilisoft.MiniJson.Json.Serialize(dictionary);
				PlayerPrefs.SetString(key2, value2);
			}
			catch (OverflowException exception)
			{
				UnityEngine.Debug.LogError("Cannot deserialize time-in-mode:    " + @string);
				UnityEngine.Debug.LogException(exception);
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogError("Unknown exception:    " + @string);
				UnityEngine.Debug.LogException(exception2);
			}
		}
		string key3 = "Statistics.RoundsInMode.Level" + ExperienceController.sharedController.currentLevel;
		if (PlayerPrefs.HasKey(key3))
		{
			Dictionary<string, object> dictionary2 = (Rilisoft.MiniJson.Json.Deserialize(PlayerPrefs.GetString(key3)) as Dictionary<string, object>) ?? new Dictionary<string, object>();
			object value3;
			if (dictionary2.TryGetValue(key, out value3))
			{
				int num2 = Convert.ToInt32(value3) + 1;
				dictionary2[key] = num2;
			}
			else
			{
				dictionary2.Add(key, 1);
			}
			string value4 = Rilisoft.MiniJson.Json.Serialize(dictionary2);
			PlayerPrefs.SetString(key3, value4);
		}
		PlayerPrefs.Save();
	}

	private IEnumerator WaitInterstitialRequestAndShowCoroutine(Task<Ad> request)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Waiting until interstitial request is completed...");
		}
		while (!((Task)request).IsCompleted)
		{
			yield return null;
		}
		if (((Task)request).IsFaulted)
		{
			UnityEngine.Debug.LogWarning("Interstitial request after match failed: " + ((Exception)(object)((Task)request).Exception).InnerException.Message);
			yield break;
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Interstitial request after match succeeded. Trying to show interstitial...");
		}
		yield return null;
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			UnityEngine.Debug.LogWarning("Stop waiting: WeaponManager.sharedManager.myPlayer != null");
			yield break;
		}
		yield return null;
		if (NetworkStartTableNGUIController.sharedController.rewardWindow != null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Waiting until Reward panel is closed...");
			}
			while (NetworkStartTableNGUIController.sharedController.isRewardShow)
			{
				yield return null;
			}
			yield return null;
			while (ExpController.Instance.WaitingForLevelUpView)
			{
				yield return null;
			}
			yield return null;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Format("Waiting until Level up panel is closed if displayed ({0})...", new object[1] { ExpController.Instance.IsLevelUpShown }));
			}
			while (ExpController.Instance.IsLevelUpShown)
			{
				yield return null;
			}
		}
		while (ShopNGUIController.GuiActive)
		{
			yield return null;
		}
		Dictionary<string, string> eventParams = new Dictionary<string, string>
		{
			{ "af_content_type", "Interstitial" },
			{ "af_content_id", "Interstitial (NetworkTable)" }
		};
		AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
		MenuBackgroundMusic.sharedMusic.Stop();
		Task<AdResult> future = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string> { { "Context", "Multiplayer Table" } }, "NetworkStartTable.WaitInterstitialRequestAndShow()");
		while (!((Task)future).IsCompleted)
		{
			yield return null;
		}
		MenuBackgroundMusic.sharedMusic.Start();
		if (((Task)future).IsFaulted)
		{
			UnityEngine.Debug.LogWarningFormat("Interstitial show after match failed: {0}", ((Exception)(object)((Task)future).Exception).InnerException.Message);
		}
		else
		{
			UnityEngine.Debug.LogFormat("Interstitial show finished with status {0}: {1}", future.Result.Status, future.Result.Message);
		}
	}

	public static bool LocalOrPasswordRoom()
	{
		if (Defs.isInet)
		{
			if (PhotonNetwork.room != null)
			{
				return !PhotonNetwork.room.customProperties[GameConnect.passwordProperty].Equals("");
			}
			return false;
		}
		return true;
	}

	private bool CheckForDeadheatInDuel()
	{
		bool flag = false;
		if (DuelController.instance.opponentNetworkTable != null)
		{
			NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
			NetworkStartTable opponentNetworkTable = DuelController.instance.opponentNetworkTable;
			int num = ((opponentNetworkTable.CountKills != -1) ? opponentNetworkTable.CountKills : opponentNetworkTable.oldCountKills);
			int num2 = ((opponentNetworkTable.score != -1) ? opponentNetworkTable.score : opponentNetworkTable.scoreOld);
			return myNetworkStartTable.CountKills == num && myNetworkStartTable.score == num2;
		}
		return Initializer.networkTables.Count < 2 && (DuelController.instance.playingTime < 30f || WeaponManager.sharedManager.myNetworkStartTable.score < 5);
	}

	private bool CheckForWin(int myPlace, int winnerTeam, int killCount, int myscore, bool scoreMatterForTeam = true)
	{
		switch (GameConnect.gameMode)
		{
		case GameConnect.GameMode.Deathmatch:
		case GameConnect.GameMode.TimeBattle:
			if (myPlace == 0)
			{
				return myscore > 0;
			}
			return false;
		case GameConnect.GameMode.TeamFight:
		case GameConnect.GameMode.FlagCapture:
		case GameConnect.GameMode.CapturePoints:
			if (myCommand == winnerTeam)
			{
				if (myscore <= 0)
				{
					return !scoreMatterForTeam;
				}
				return true;
			}
			return false;
		case GameConnect.GameMode.DeadlyGames:
			if (killCount > 0)
			{
				return isIwin;
			}
			return false;
		case GameConnect.GameMode.Duel:
			if (myPlace == 0 && myscore > 0)
			{
				return !CheckForDeadheatInDuel();
			}
			return false;
		case GameConnect.GameMode.Spleef:
			if (myscore > 0)
			{
				return isIwin;
			}
			return false;
		default:
			return false;
		}
	}

	public void win(string winner, int _commandWin = 0, int blueCount = 0, int redCount = 0)
	{
		if (NetworkStartTableNGUIController.sharedController.isRewardShow || isShowFinished || isEndedInMinigames)
		{
			if (GameConnect.isHunger && isDeadInMiniGame)
			{
				NetworkStartTableNGUIController.sharedController.MathFinishedDeadInHunger();
			}
			return;
		}
		if (GameConnect.isMiniGame)
		{
			isEndedInMinigames = true;
		}
		try
		{
			if (!CloudSyncController.IsSynchronizingWithCloud)
			{
				CloudSyncController.Instance.Pull(true);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in CloudSyncController.Instance.Pull: {0}", ex);
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnMatchEnd();
		}
		if (Defs.isInet)
		{
			PhotonNetwork.FetchServerTimestamp();
		}
		_matchStopwatch.Stop();
		double totalMinutes = _matchStopwatch.Elapsed.TotalMinutes;
		if (GameConnect.isMiniGame)
		{
			MiniGamesController.Instance.isEnd = true;
		}
		if (GameConnect.isDaterRegim)
		{
			Storager.setInt("DaterDayLived", Storager.getInt("DaterDayLived") + 1);
			int num = 5;
			num = ((!Defs.isInet) ? ((!PlayerPrefs.GetString("MaxKill", "9").Equals("")) ? int.Parse(PlayerPrefs.GetString("MaxKill", "5")) : 5) : ((int)PhotonNetwork.room.customProperties[GameConnect.maxKillProperty]));
			AnalyticsStuff.LogSandboxTimeGamePopularity(num, false);
		}
		StoreKitEventListener.State.PurchaseKey = "End match";
		int num2 = PlayerPrefs.GetInt("CountMatch", 0) + 1;
		PlayerPrefs.SetInt("CountMatch", num2);
		if (num2 <= 5)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Battle_End, num2);
		}
		Dictionary<string, object> parameters = new Dictionary<string, object> { { "count", num2 } };
		AnalyticsFacade.SendCustomEventToFacebook("games_multiplayer_count", parameters);
		if (ExperienceController.sharedController != null)
		{
			string key = "Statistics.MatchCount.Level" + ExperienceController.sharedController.currentLevel;
			int @int = PlayerPrefs.GetInt(key, 0);
			PlayerPrefs.SetInt(key, @int + 1);
		}
		IncreaseTimeInMode((int)GameConnect.gameMode, _matchStopwatch.Elapsed.TotalMinutes);
		_matchStopwatch.Reset();
		isShowAvard = false;
		commandWinner = _commandWin;
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		List<ScoreTableItem> list = new List<ScoreTableItem>();
		List<ScoreTableItem> list2 = new List<ScoreTableItem>();
		List<ScoreTableItem> sortedScoresList = GetSortedScoresList(true);
		oldPlayersList = sortedScoresList.ToArray();
		int num3 = 0;
		for (int i = 0; i < oldPlayersList.Length; i++)
		{
			int num4 = oldPlayersList[i].table.myCommand;
			if (num4 == -1)
			{
				num4 = oldPlayersList[i].table.myCommandOld;
			}
			if (num4 == 0 && oldPlayersList[i].isMine)
			{
				num3 = i;
			}
			if (num4 == 1)
			{
				if (oldPlayersList[i].isMine)
				{
					num3 = list.Count;
				}
				list.Add(oldPlayersList[i]);
			}
			if (num4 == 2)
			{
				if (oldPlayersList[i].isMine)
				{
					num3 = list2.Count;
				}
				list2.Add(oldPlayersList[i]);
			}
		}
		oldRedPlayersList = list2.ToArray();
		oldBluePlayersList = list.ToArray();
		bool flag = false;
		if (GameConnect.gameMode == GameConnect.GameMode.Deathmatch)
		{
			flag = true;
			for (int j = 0; j < oldPlayersList.Length; j++)
			{
				if (oldPlayersList[j].score >= AdminSettingsController.minScoreDeathMath)
				{
					flag = false;
				}
			}
		}
		addCoins = 0;
		addExperience = 0;
		bool flag2 = false;
		if (GameConnect.isDuel)
		{
			flag2 = CheckForDeadheatInDuel();
			if (flag2)
			{
				num3 = 1;
			}
		}
		bool flag3 = false;
		if (GameConnect.isSpleef)
		{
			flag3 = !isDeadInMiniGame && Initializer.players.Count == 1;
			num3 = (flag3 ? Mathf.Max(MiniGamesController.Instance.playersOnStart - 1, 0) : Mathf.Max(MiniGamesController.Instance.playersOnStart - Initializer.players.Count, 0));
		}
		bool flag4 = CheckForWin(num3, _commandWin, CountKills, score) && (!GameConnect.isSpleef || flag3);
		bool iAmWinnerInTeam = CheckForWin(num3, _commandWin, CountKills, score, false) && (!GameConnect.isSpleef || flag3);
		KillRateCheck.instance.LogFirstBattlesResult(flag4);
		RatingSystem.RatingChange ratingChange = CalculateMatchRating(false);
		Singleton<EggsManager>.Instance.OnMathEnded(flag4);
		ExchangeWindow.Hide();
		if (GameConnect.isDuel)
		{
			SceneInfoController.instance.UpdateListAvaliableMap();
		}
		if (GameConnect.isMiniGame || GameConnect.isDaterRegim)
		{
			int num5 = MiniGamesPlayerScoreManager.Instance.GetScore(GameConnect.gameMode);
			int scores = num5;
			bool flag5 = false;
			switch (GameConnect.gameMode)
			{
			case GameConnect.GameMode.TimeBattle:
			case GameConnect.GameMode.DeadlyGames:
			case GameConnect.GameMode.Spleef:
				if (num5 < score)
				{
					scores = score;
					flag5 = true;
				}
				break;
			case GameConnect.GameMode.Dater:
				if (num5 < CountKills)
				{
					scores = CountKills;
					flag5 = true;
				}
				break;
			case GameConnect.GameMode.DeathEscape:
				if (num5 == 0 || num5 > CountKills)
				{
					scores = CountKills;
					flag5 = true;
				}
				break;
			}
			if (flag5)
			{
				MiniGamesPlayerScoreManager.Instance.SetScore(GameConnect.gameMode, scores);
				FriendsController.sharedController.SendScoreInMiniGames((int)GameConnect.gameMode, scores);
			}
		}
		if (isInet)
		{
			if (myCommand == _commandWin || (!GameConnect.isCompany && !GameConnect.isFlag && !GameConnect.isCapturePoints) || ExperienceController.sharedController.currentLevel < 2)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && (GameConnect.isCompany || GameConnect.isFlag || GameConnect.isCapturePoints))
				{
					isIwin = myCommand == _commandWin;
				}
				int timeGame = int.Parse(PhotonNetwork.room.customProperties[GameConnect.maxKillProperty].ToString());
				AdminSettingsController.Avard avardAfterMatch = AdminSettingsController.GetAvardAfterMatch(GameConnect.gameMode, timeGame, num3, score, CountKills, isIwin);
				addCoins = avardAfterMatch.coin;
				addExperience = avardAfterMatch.expierense;
			}
			if (isMine)
			{
				double num6 = totalMinutes;
				string reasonToDismissInterstitialAfterMatch = AfterMatchInterstitialRunner.GetReasonToDismissInterstitialAfterMatch(flag4, num6);
				if (string.IsNullOrEmpty(reasonToDismissInterstitialAfterMatch))
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.LogFormat("<color=magenta>{0}.win(), winner: {1}, matchDuration: {2:f2}</color>", GetType().Name, flag4, num6);
					}
					new AfterMatchInterstitialRunner().Run();
				}
				else
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing interstitial. {0}</color>" : "Dismissing interstitial. {0}", reasonToDismissInterstitialAfterMatch);
				}
			}
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
			}
			if (flag4)
			{
				if (!LocalOrPasswordRoom())
				{
					QuestMediator.NotifyWin(GameConnect.gameMode, Application.loadedLevelName);
				}
				if (GameConnect.isFlag)
				{
					int val = Storager.getInt(Defs.RatingFlag) + 1;
					Storager.setInt(Defs.RatingFlag, val);
				}
				if (GameConnect.isCompany)
				{
					int val2 = Storager.getInt(Defs.RatingTeamBattle) + 1;
					Storager.setInt(Defs.RatingTeamBattle, val2);
				}
				if (GameConnect.isCapturePoints)
				{
					int val3 = Storager.getInt(Defs.RatingCapturePoint) + 1;
					Storager.setInt(Defs.RatingCapturePoint, val3);
				}
				if (GameConnect.isDuel)
				{
					int val4 = Storager.getInt(Defs.RatingDuel) + 1;
					Storager.setInt(Defs.RatingDuel, val4);
				}
				if (GameConnect.gameMode == GameConnect.GameMode.Deathmatch)
				{
					int val5 = Storager.getInt(Defs.RatingDeathmatch) + 1;
					Storager.setInt(Defs.RatingDeathmatch, val5);
				}
				if (GameConnect.isHunger)
				{
					int val6 = Storager.getInt(Defs.RatingHunger) + 1;
					Storager.setInt(Defs.RatingHunger, val6);
					if (FriendsController.sharedController != null)
					{
						FriendsController.sharedController.TryIncrementWinCountTimestamp();
					}
				}
				if (GameConnect.isCOOP)
				{
					int val7 = Storager.getInt(Defs.RatingKey) + 1;
					Storager.setInt(Defs.RatingKey, val7);
					if (FriendsController.sharedController != null)
					{
						FriendsController.sharedController.TryIncrementWinCountTimestamp();
					}
				}
				if (ExperienceController.sharedController != null)
				{
					string key2 = "Statistics.WinCount.Level" + ExperienceController.sharedController.currentLevel;
					int int2 = PlayerPrefs.GetInt(key2, 0);
					PlayerPrefs.SetInt(key2, int2 + 1);
				}
				if (!GameConnect.isCOOP)
				{
					FriendsController.sharedController.SendRoundWon();
					if (PlayerPrefs.GetInt("LogCountMatch", 0) == 1)
					{
						PlayerPrefs.SetInt("LogCountMatch", 0);
						if (Social.localUser.authenticated)
						{
							Social.ReportProgress("CgkIr8rGkPIJEAIQAg", 100.0, delegate(bool success)
							{
								UnityEngine.Debug.Log("Achievement First Win completed: " + success);
							});
						}
					}
				}
			}
			if (addCoins > 0)
			{
				Initializer.Instance.isLastMatchWin = true;
			}
			if (addCoins > 0 || (ExperienceController.sharedController.currentLevel < 31 && addExperience > 0))
			{
				isShowAvard = true;
				if (PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[GameConnect.passwordProperty].Equals(""))
				{
					addCoins = 0;
					addExperience = 0;
					isShowAvard = false;
				}
			}
		}
		bool flag6 = false;
		int num7 = 0;
		NetworkStartTable networkStartTable = null;
		if (GameConnect.isDaterRegim)
		{
			for (int k = 0; k < oldPlayersList.Length; k++)
			{
				if (oldPlayersList[k].killCount > num7)
				{
					networkStartTable = oldPlayersList[k].table;
					flag6 = false;
					num7 = oldPlayersList[k].killCount;
				}
				else if (oldPlayersList[k].killCount > 0 && oldPlayersList[k].killCount == num7)
				{
					flag6 = true;
				}
			}
		}
		myCommandOld = myCommand;
		oldCountKills = CountKills;
		scoreOld = score;
		score = -1;
		GlobalGameController.Score = -1;
		scoreCommandFlag1 = 0;
		scoreCommandFlag2 = 0;
		CountKills = -1;
		if (isCompany || GameConnect.isFlag || GameConnect.isCapturePoints)
		{
			myCommand = -1;
		}
		SynhCommand();
		SynhCountKills();
		SynhScore();
		if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.showRanks)
		{
			NetworkStartTableNGUIController.sharedController.BackPressFromRanksTable();
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DamageFrame");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		int winnerCommand = 0;
		string winner2;
		if (GameConnect.isDaterRegim)
		{
			winner2 = ((!(networkStartTable != null)) ? LocalizationStore.Get("Key_1427") : (flag6 ? LocalizationStore.Get("Key_1764") : ((!networkStartTable.Equals(this)) ? string.Format(LocalizationStore.Get("Key_1763"), new object[1] { networkStartTable.NamePlayer }) : LocalizationStore.Get("Key_1762"))));
		}
		else if (!isCompany && !GameConnect.isFlag && !GameConnect.isCapturePoints)
		{
			winner2 = (((GameConnect.isMiniGame && MiniGamesController.Instance.isDraw) || flag) ? LocalizationStore.Key_0568 : ((!flag4) ? LocalizationStore.Get("Key_1116") : LocalizationStore.Get("Key_1115")));
		}
		else
		{
			string key_ = LocalizationStore.Key_0571;
			winner2 = ((commandWinner == 0) ? key_ : ((commandWinner == myCommandOld) ? LocalizationStore.Get("Key_1793") : LocalizationStore.Get("Key_1794")));
			winnerCommand = ((commandWinner != 0) ? ((commandWinner == myCommandOld) ? 1 : 2) : 0);
		}
		isShowFinished = true;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			if (!GameConnect.isDaterRegim && Defs.isInet && !isSetNewMapButton)
			{
				NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons();
			}
			if (GameConnect.isDuel)
			{
				NetworkStartTableNGUIController.sharedController.StartCoroutine(NetworkStartTableNGUIController.sharedController.MatchFinishedInDuelInterface(ratingChange, isShowAvard, addCoins, addExperience, LocalOrPasswordRoom(), num3 == 0, flag2));
			}
			else
			{
				NetworkStartTableNGUIController.sharedController.StartCoroutine(NetworkStartTableNGUIController.sharedController.MatchFinishedInterface(winner2, ratingChange, isShowAvard, addCoins, addExperience, LocalOrPasswordRoom(), GameConnect.isHunger ? isIwin : (num3 == 0), iAmWinnerInTeam, winnerCommand, blueCount, redCount));
			}
		}
		isShowAvard = false;
		showTable = false;
		isShowNickTable = true;
	}

	public int GetPlaceInMatch()
	{
		if (GameConnect.isMiniGame)
		{
			return GetPlaceInAllScores();
		}
		return GetPlaceInTable();
	}

	private int GetPlaceInAllScores()
	{
		int result = 0;
		List<ScoreTableItem> sortedScoresList = GetSortedScoresList();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < sortedScoresList.Count; i++)
		{
			int num4 = sortedScoresList[i].table.myCommand;
			if (num4 == -1)
			{
				num4 = sortedScoresList[i].table.myCommandOld;
			}
			if (num4 == 0)
			{
				if (sortedScoresList[i].isMine)
				{
					result = num;
				}
				num++;
			}
			if (num4 == 1)
			{
				if (sortedScoresList[i].isMine)
				{
					result = num2;
				}
				num2++;
			}
			if (num4 == 2)
			{
				if (sortedScoresList[i].isMine)
				{
					result = num3;
				}
				num3++;
			}
		}
		return result;
	}

	public static List<NetworkStartTable> GetSortedTablesList()
	{
		_tabsBuffer.Clear();
		int count = Initializer.networkTables.Count;
		for (int i = 0; i != count; i++)
		{
			NetworkStartTable item = Initializer.networkTables[i];
			_tabsBuffer.Add(item);
		}
		List<NetworkStartTable> tabsBuffer = _tabsBuffer;
		int count2 = tabsBuffer.Count;
		for (int j = 1; j < count2; j++)
		{
			NetworkStartTable networkStartTable = tabsBuffer[j];
			for (int k = 0; k < j; k++)
			{
				NetworkStartTable networkStartTable2 = tabsBuffer[k];
				bool flag = !GameConnect.isDuel && !GameConnect.isFlag && !GameConnect.isCapturePoints;
				if ((GameConnect.isDeathEscape && ((networkStartTable.CountKills > 0 && networkStartTable.CountKills < networkStartTable2.CountKills) || (networkStartTable.CountKills == networkStartTable2.CountKills && networkStartTable.score > networkStartTable2.score))) || (!GameConnect.isDeathEscape && ((flag && (networkStartTable.score > networkStartTable2.score || (networkStartTable.score == networkStartTable2.score && networkStartTable.CountKills > networkStartTable2.CountKills))) || (!flag && (networkStartTable.CountKills > networkStartTable2.CountKills || (networkStartTable.CountKills == networkStartTable2.CountKills && networkStartTable.score > networkStartTable2.score))))))
				{
					NetworkStartTable value = tabsBuffer[j];
					for (int num = j - 1; num >= k; num--)
					{
						tabsBuffer[num + 1] = tabsBuffer[num];
					}
					tabsBuffer[k] = value;
					break;
				}
			}
		}
		return _tabsBuffer;
	}

	public static List<ScoreTableItem> GetSortedScoresList(bool updateScores = false)
	{
		_scoresBuffer.Clear();
		if (GameConnect.isMiniGame)
		{
			if (updateScores)
			{
				MiniGamesController.Instance.UpdatePlayerScores();
			}
			foreach (KeyValuePair<string, ScoreTableItem> playerScore in MiniGamesController.Instance.playerScores)
			{
				_scoresBuffer.Add(playerScore.Value);
			}
		}
		else
		{
			for (int i = 0; i < Initializer.networkTables.Count; i++)
			{
				_scoresBuffer.Add(new ScoreTableItem(Initializer.networkTables[i]));
			}
		}
		for (int j = 1; j < _scoresBuffer.Count; j++)
		{
			ScoreTableItem scoreTableItem = _scoresBuffer[j];
			for (int k = 0; k < j; k++)
			{
				ScoreTableItem scoreTableItem2 = _scoresBuffer[k];
				bool flag = !GameConnect.isDuel && !GameConnect.isFlag && !GameConnect.isCapturePoints;
				if ((GameConnect.isDeathEscape && ((scoreTableItem.killCount > 0 && scoreTableItem.killCount < scoreTableItem2.killCount) || (scoreTableItem.killCount == scoreTableItem2.killCount && scoreTableItem.score > scoreTableItem2.score))) || (!GameConnect.isDeathEscape && ((flag && (scoreTableItem.score > scoreTableItem2.score || (scoreTableItem.score == scoreTableItem2.score && scoreTableItem.killCount > scoreTableItem2.killCount))) || (!flag && (scoreTableItem.killCount > scoreTableItem2.killCount || (scoreTableItem.killCount == scoreTableItem2.killCount && scoreTableItem.score > scoreTableItem2.score))))))
				{
					ScoreTableItem value = _scoresBuffer[j];
					for (int num = j - 1; num >= k; num--)
					{
						_scoresBuffer[num + 1] = _scoresBuffer[num];
					}
					_scoresBuffer[k] = value;
					break;
				}
			}
		}
		return _scoresBuffer;
	}

	private int GetPlaceInTable()
	{
		int result = 0;
		List<NetworkStartTable> sortedTablesList = GetSortedTablesList();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < sortedTablesList.Count; i++)
		{
			if (sortedTablesList[i].myCommand == 0)
			{
				if (sortedTablesList[i] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num;
				}
				num++;
			}
			if (sortedTablesList[i].myCommand == 1)
			{
				if (sortedTablesList[i] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num2;
				}
				num2++;
			}
			if (sortedTablesList[i].myCommand == 2)
			{
				if (sortedTablesList[i] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num3;
				}
				num3++;
			}
		}
		_tabsBuffer.Clear();
		return result;
	}

	public int GetWinningTeam()
	{
		int result = 0;
		if (GameConnect.isFlag)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2)
				{
					result = 1;
				}
				else if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1)
				{
					result = 2;
				}
			}
		}
		else if (GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			if (CapturePointController.sharedController.scoreBlue > CapturePointController.sharedController.scoreRed)
			{
				result = 1;
			}
			else if (CapturePointController.sharedController.scoreRed > CapturePointController.sharedController.scoreBlue)
			{
				result = 2;
			}
		}
		else if (myPlayerMoveC != null)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed)
			{
				result = 1;
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue)
			{
				result = 2;
			}
		}
		else if (GlobalGameController.countKillsBlue > GlobalGameController.countKillsRed)
		{
			result = 1;
		}
		else if (GlobalGameController.countKillsRed > GlobalGameController.countKillsBlue)
		{
			result = 2;
		}
		return result;
	}

	public void CalculateMatchRatingOnDisconnect()
	{
		if (myPlayerMoveC != null && ((!GameConnect.isCOOP && !GameConnect.isCompany && !GameConnect.isFlag && !GameConnect.isCapturePoints) || myPlayerMoveC.liveTime > 90f || exitFromMenu))
		{
			CalculateMatchRating(true);
		}
	}

	public void IncrementKills()
	{
		killCountMatch++;
	}

	public void IncrementDeath()
	{
		deathCountMatch++;
	}

	public float GetMatchKillrate()
	{
		if (GameConnect.isCOOP)
		{
			return 1f;
		}
		if (deathCountMatch != 0)
		{
			return (float)killCountMatch / (float)deathCountMatch;
		}
		return killCountMatch;
	}

	public void ClearKillrate()
	{
		killCountMatch = 0;
		deathCountMatch = 0;
	}

	public bool IsRatingMatch()
	{
		if (LocalOrPasswordRoom() || !RatingSystem.instance.ratingMatch)
		{
			return false;
		}
		if (myPlayerMoveC == null)
		{
			return false;
		}
		if (GameConnect.isDaterRegim)
		{
			return false;
		}
		if (GameConnect.isMiniGame)
		{
			return false;
		}
		return true;
	}

	public bool CheckNeedRatingChange(bool ratingWinner, bool ratingDeadHeat)
	{
		if (GameConnect.isDuel && (ratingDeadHeat || (!ratingWinner && DuelController.instance.playingTime < 60f)))
		{
			return false;
		}
		if (GameConnect.isHunger && CountKills <= 0 && ratingWinner)
		{
			return false;
		}
		if (!GameConnect.isHunger && !GameConnect.isDuel && !ratingWinner && myPlayerMoveC.liveTime < 90f)
		{
			return false;
		}
		return true;
	}

	public RatingSystem.MatchStat GetCurrentRatingMatchStat()
	{
		int placeInMatch = GetPlaceInMatch();
		int count = Initializer.networkTables.Count;
		int winningTeam = GetWinningTeam();
		bool flag = CheckForWin(placeInMatch, winningTeam, CountKills, score, false);
		int num = 0;
		int num2 = 0;
		bool flag2 = false;
		bool flag3 = false;
		if (isCompany || GameConnect.isFlag || GameConnect.isCapturePoints)
		{
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < Initializer.networkTables.Count; i++)
			{
				if (Initializer.networkTables[i].myCommand == 1)
				{
					num4++;
				}
				else
				{
					num3++;
				}
			}
			int num5 = ((num3 > num4) ? num3 : num4);
			num2 = num5 * 2;
			num = placeInMatch + ((!flag) ? num5 : 0);
			flag2 = winningTeam == 0;
			flag3 = flag;
		}
		else if (GameConnect.isHunger)
		{
			placeInMatch = ((!flag) ? (Initializer.players.Count - 1) : 0);
			num2 = playerCountInHunger;
			num = Mathf.Clamp(placeInMatch, 0, playerCountInHunger - 1);
			flag2 = false;
			flag3 = placeInMatch < Mathf.CeilToInt(num2 / 2);
		}
		else
		{
			num2 = count;
			num = placeInMatch;
			flag2 = GameConnect.isDuel && CheckForDeadheatInDuel();
			flag3 = placeInMatch < Mathf.CeilToInt(num2 / 2);
		}
		return new RatingSystem.MatchStat(num, num2, flag3, flag2);
	}

	public int GetCurrentRatingChange(bool onExit)
	{
		if (!IsRatingMatch())
		{
			return 0;
		}
		if (!onExit && !GameConnect.isHunger && score <= 0 && !myPlayerMoveC.killedInMatch)
		{
			return 0;
		}
		if (onExit && !GameConnect.isDuel && Initializer.players.Count < 2)
		{
			return 0;
		}
		RatingSystem.MatchStat matchStat = (onExit ? RatingSystem.MatchStat.LooseStat : GetCurrentRatingMatchStat());
		if (!onExit && !CheckNeedRatingChange(matchStat.winner, matchStat.deadHeat))
		{
			return 0;
		}
		return RatingSystem.instance.GetRatingValueForParams(matchStat.playerCount, matchStat.place, GetMatchKillrate(), matchStat.deadHeat);
	}

	public RatingSystem.RatingChange CalculateMatchRating(bool disconnecting)
	{
		RatingSystem.RatingChange currentRatingChange = RatingSystem.instance.currentRatingChange;
		if (!IsRatingMatch())
		{
			return currentRatingChange;
		}
		if (!exitFromMenu && !GameConnect.isHunger && score <= 0 && !myPlayerMoveC.killedInMatch)
		{
			return currentRatingChange;
		}
		RatingSystem.MatchStat matchStat = (exitFromMenu ? RatingSystem.MatchStat.LooseStat : GetCurrentRatingMatchStat());
		if (disconnecting && matchStat.winner)
		{
			return currentRatingChange;
		}
		if (!exitFromMenu && !CheckNeedRatingChange(matchStat.winner, matchStat.deadHeat))
		{
			return currentRatingChange;
		}
		currentRatingChange = RatingSystem.instance.CalculateRating(matchStat.playerCount, matchStat.place, GetMatchKillrate(), matchStat.deadHeat);
		if (disconnecting && currentRatingChange.addRating < 0)
		{
			PlayerPrefs.SetInt("leave_from_duel_penalty", currentRatingChange.addRating);
			PlayerPrefs.Save();
		}
		return currentRatingChange;
	}

	public RatingSystem.RatingChange CalculateMatchRatingOld(bool disconnecting)
	{
		RatingSystem.RatingChange currentRatingChange = RatingSystem.instance.currentRatingChange;
		if (LocalOrPasswordRoom() || !RatingSystem.instance.ratingMatch)
		{
			return currentRatingChange;
		}
		if (GameConnect.isHunger && isDeadInMiniGame)
		{
			return currentRatingChange;
		}
		int num = GetPlaceInMatch();
		int winningTeam = GetWinningTeam();
		bool flag = CheckForWin(num, winningTeam, CountKills, score, false);
		if (myPlayerMoveC != null && (GameConnect.isHunger || score > 0 || myPlayerMoveC.killedInMatch))
		{
			List<int> list = new List<int>();
			if (isCompany || GameConnect.isFlag || GameConnect.isCapturePoints)
			{
				for (int i = 0; i < Initializer.networkTables.Count; i++)
				{
					if (Initializer.networkTables[i] != this)
					{
						list.Add((Initializer.networkTables[i].gameRating != -1) ? Initializer.networkTables[i].gameRating : gameRating);
					}
				}
				if (list.Count == 0)
				{
					list.Add(gameRating);
				}
			}
			else if (!GameConnect.isHunger)
			{
				for (int j = 0; j < Initializer.networkTables.Count; j++)
				{
					if (Initializer.networkTables[j] != this)
					{
						list.Add((Initializer.networkTables[j].gameRating != -1) ? Initializer.networkTables[j].gameRating : gameRating);
					}
				}
				if (list.Count == 0)
				{
					return currentRatingChange;
				}
				float num2 = (float)Initializer.networkTables.Count / 2f;
				flag = (float)(num + 1) <= num2;
				if (!flag)
				{
					num -= Mathf.FloorToInt(num2);
				}
			}
			UnityEngine.Debug.Log(string.Format("<color=orange>My place: {0}, team winner: {1}, rating winner - {2}</color>", new object[3]
			{
				num.ToString(),
				winningTeam.ToString(),
				flag.ToString()
			}));
			if (!flag && !GameConnect.isHunger && myPlayerMoveC.liveTime < 60f)
			{
				return currentRatingChange;
			}
			bool flag2 = disconnecting && flag;
			return currentRatingChange;
		}
		return currentRatingChange;
	}

	public void SetSceneCameraEnabled(bool enable)
	{
		if (_cam != null)
		{
			_cam.SetActive(enable);
		}
	}

	public void DestroyPlayer()
	{
		isShowFinished = false;
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		if (_cam != null)
		{
			_cam.SetActive(true);
			_cam.GetComponent<RPG_Camera>().enabled = false;
		}
		if (!isInet)
		{
			DestroyMyPlayer();
		}
		else if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
		{
			PhotonNetwork.Destroy(_weaponManager.myPlayer);
		}
	}

	private void DestroyMyPlayer()
	{
	}

	private void finishTable()
	{
		playersTable();
	}

	public void MyOnGUI()
	{
		if (experienceController.isShowAdd)
		{
			GUI.enabled = false;
		}
		if (showDisconnectFromServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)serverLeftTheGame.width * 0.5f * koofScreen, (float)(Screen.height / 2) - (float)serverLeftTheGame.height * 0.5f * koofScreen, (float)serverLeftTheGame.width * koofScreen, (float)serverLeftTheGame.height * koofScreen), serverLeftTheGame);
			GUI.enabled = false;
		}
		if (showDisconnectFromMasterServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)serverLeftTheGame.width * 0.5f * koofScreen, (float)(Screen.height / 2) - (float)serverLeftTheGame.height * 0.5f * koofScreen, (float)serverLeftTheGame.width * koofScreen, (float)serverLeftTheGame.height * koofScreen), serverLeftTheGame);
		}
		if (showTable)
		{
			playersTable();
		}
		if (isShowNickTable)
		{
			finishTable();
		}
		if (showMessagFacebook)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Tools.SuccessMessageRect(), _SocialSentSuccess("Facebook"), labelStyle);
		}
		GUI.enabled = true;
	}

	public void ClearScoreCommandInFlagGame()
	{
		photonView.RPC("ClearScoreCommandInFlagGameRPC", PhotonTargets.Others);
	}

	[PunRPC]
	[RPC]
	public void ClearScoreCommandInFlagGameRPC()
	{
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag1 = 0;
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag2 = 0;
		}
	}

	private void AddFlag()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
		PhotonNetwork.InstantiateSceneObject("Flags/Flag1", gameObject.transform.position, gameObject.transform.rotation, 0, null);
		PhotonNetwork.InstantiateSceneObject("Flags/Flag2", gameObject2.transform.position, gameObject2.transform.rotation, 0, null);
	}

	[PunRPC]
	[RPC]
	private void AddPaticleBazeRPC(int _command)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("BazaZoneCommand" + _command);
		UnityEngine.Object.Instantiate(Resources.Load((_command == WeaponManager.sharedManager.myNetworkStartTable.myCommand) ? "Ring_Particle_Blue" : "Ring_Particle_Red"), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.22f, gameObject.transform.position.z), gameObject.transform.rotation);
	}

	public void AddScore()
	{
		CountKills++;
		GlobalGameController.CountKills = CountKills;
		photonView.RPC("AddPaticleBazeRPC", PhotonTargets.All, myCommand);
		if (myCommand == 1)
		{
			photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, 1, scoreCommandFlag1 + 1);
		}
		else
		{
			photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, 2, scoreCommandFlag2 + 1);
		}
		SynhCountKills();
	}

	public void AddToScore(int addscore)
	{
		int num = score;
		num = (GlobalGameController.Score = Mathf.Max(0, num + addscore));
		score = num;
		SynhScore();
	}

	[PunRPC]
	[RPC]
	private void SynchScoreCommandRPC(int _command, int _score)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < array.Length; i++)
		{
			if (_command == 1)
			{
				array[i].GetComponent<NetworkStartTable>().scoreCommandFlag1 = _score;
			}
			else
			{
				array[i].GetComponent<NetworkStartTable>().scoreCommandFlag2 = _score;
			}
		}
	}

	private void OnDestroy()
	{
		if (isMine)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			RemoveShop(false);
			if (networkStartTableNGUIController != null && !networkStartTableNGUIController.isRewardShow)
			{
				UnityEngine.Object.Destroy(networkStartTableNGUIController.gameObject);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
			if (InGameGUI.sharedInGameGUI != null)
			{
				UnityEngine.Object.Destroy(InGameGUI.sharedInGameGUI.gameObject);
			}
		}
		if (!isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(NamePlayer + " " + LocalizationStore.Get("Key_0996"), new Color(1f, 0f, 0f));
		}
		if (Initializer.networkTables.Contains(this))
		{
			Initializer.networkTables.Remove(this);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	[PunRPC]
	[RPC]
	private void SynchGameRating(int _rating)
	{
		gameRating = _rating;
	}
}
