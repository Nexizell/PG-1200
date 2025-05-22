using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using Rilisoft.WP8;
using RilisoftBot;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Initializer : MonoBehaviour
{
	public class TargetsList
	{
		[CompilerGenerated]
		internal sealed class _003CGetEnumerator_003Ed__5 : IEnumerator<Transform>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private Transform _003C_003E2__current;

			public TargetsList _003C_003E4__this;

			private int _003Ci_003E5__1;

			private int _003Ci_003E5__2;

			private int _003Ci_003E5__3;

			private int _003Ci_003E5__4;

			private int _003Ci_003E5__5;

			Transform IEnumerator<Transform>.Current
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
			public _003CGetEnumerator_003Ed__5(int _003C_003E1__state)
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
					if (_003C_003E4__this.includeSelf)
					{
						_003C_003E2__current = _003C_003E4__this.forPlayer.myPlayerTransform;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0065;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0065;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0111;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01a0;
				case 4:
					_003C_003E1__state = -1;
					goto IL_022f;
				case 5:
					_003C_003E1__state = -1;
					goto IL_02a6;
				case 6:
					{
						_003C_003E1__state = -1;
						goto IL_0349;
					}
					IL_02b6:
					if (_003Ci_003E5__4 < enemiesObj.Count)
					{
						if (!enemiesObj[_003Ci_003E5__4].GetComponent<BaseBot>().IsDeath)
						{
							_003C_003E2__current = enemiesObj[_003Ci_003E5__4].transform;
							_003C_003E1__state = 5;
							return true;
						}
						goto IL_02a6;
					}
					goto IL_02c8;
					IL_0065:
					if (Defs.isMulti && GameConnect.gameMode != GameConnect.GameMode.TimeBattle)
					{
						_003Ci_003E5__1 = 0;
						goto IL_0121;
					}
					_003Ci_003E5__4 = 0;
					goto IL_02b6;
					IL_0359:
					if (_003Ci_003E5__5 < damageableObjects.Count)
					{
						IDamageable component = damageableObjects[_003Ci_003E5__5].GetComponent<IDamageable>();
						if (!component.IsDead() && component.IsEnemyTo(_003C_003E4__this.forPlayer) && (_003C_003E4__this.includeExplosions || !(component is DamagedExplosionObject)))
						{
							_003C_003E2__current = damageableObjects[_003Ci_003E5__5].transform;
							_003C_003E1__state = 6;
							return true;
						}
						goto IL_0349;
					}
					return false;
					IL_01a0:
					_003Ci_003E5__2++;
					goto IL_01b0;
					IL_0121:
					if (_003Ci_003E5__1 < players.Count)
					{
						if (!players[_003Ci_003E5__1].Equals(_003C_003E4__this.forPlayer) && !players[_003Ci_003E5__1].isKilled && IsEnemyTarget(players[_003Ci_003E5__1].myPlayerTransform, _003C_003E4__this.forPlayer))
						{
							_003C_003E2__current = players[_003Ci_003E5__1].myPlayerTransform;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_0111;
					}
					_003Ci_003E5__2 = 0;
					goto IL_01b0;
					IL_0111:
					_003Ci_003E5__1++;
					goto IL_0121;
					IL_01b0:
					if (_003Ci_003E5__2 < turretsObj.Count)
					{
						TurretController component2 = turretsObj[_003Ci_003E5__2].GetComponent<TurretController>();
						if (!component2.isKilled && IsEnemyTarget(turretsObj[_003Ci_003E5__2].transform, _003C_003E4__this.forPlayer))
						{
							_003C_003E2__current = component2.transform;
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_01a0;
					}
					_003Ci_003E5__3 = 0;
					goto IL_023f;
					IL_02c8:
					_003Ci_003E5__5 = 0;
					goto IL_0359;
					IL_023f:
					if (_003Ci_003E5__3 < petsObj.Count)
					{
						if (IsEnemyTarget(petsObj[_003Ci_003E5__3].transform, _003C_003E4__this.forPlayer))
						{
							PetEngine component3 = petsObj[_003Ci_003E5__3].GetComponent<PetEngine>();
							if (component3.IsAlive)
							{
								_003C_003E2__current = component3.transform;
								_003C_003E1__state = 4;
								return true;
							}
						}
						goto IL_022f;
					}
					goto IL_02c8;
					IL_02a6:
					_003Ci_003E5__4++;
					goto IL_02b6;
					IL_0349:
					_003Ci_003E5__5++;
					goto IL_0359;
					IL_022f:
					_003Ci_003E5__3++;
					goto IL_023f;
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

		private Player_move_c forPlayer;

		private bool includeSelf;

		private bool includeExplosions;

		public TargetsList()
		{
			forPlayer = WeaponManager.sharedManager.myPlayerMoveC;
			includeSelf = false;
			includeExplosions = true;
		}

		public TargetsList(Player_move_c forPlayer, bool includeMyPlayer = false, bool includeExplosions = true)
		{
			this.forPlayer = forPlayer;
			includeSelf = includeMyPlayer;
			this.includeExplosions = includeExplosions;
		}

		public IEnumerator<Transform> GetEnumerator()
		{
			if (includeSelf)
			{
				yield return forPlayer.myPlayerTransform;
			}
			if (Defs.isMulti && GameConnect.gameMode != GameConnect.GameMode.TimeBattle)
			{
				for (int m = 0; m < players.Count; m++)
				{
					if (!players[m].Equals(forPlayer) && !players[m].isKilled && IsEnemyTarget(players[m].myPlayerTransform, forPlayer))
					{
						yield return players[m].myPlayerTransform;
					}
				}
				for (int l = 0; l < turretsObj.Count; l++)
				{
					TurretController component = turretsObj[l].GetComponent<TurretController>();
					if (!component.isKilled && IsEnemyTarget(turretsObj[l].transform, forPlayer))
					{
						yield return component.transform;
					}
				}
				for (int k = 0; k < petsObj.Count; k++)
				{
					if (IsEnemyTarget(petsObj[k].transform, forPlayer))
					{
						PetEngine component2 = petsObj[k].GetComponent<PetEngine>();
						if (component2.IsAlive)
						{
							yield return component2.transform;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < enemiesObj.Count; j++)
				{
					if (!enemiesObj[j].GetComponent<BaseBot>().IsDeath)
					{
						yield return enemiesObj[j].transform;
					}
				}
			}
			for (int i = 0; i < damageableObjects.Count; i++)
			{
				IDamageable component3 = damageableObjects[i].GetComponent<IDamageable>();
				if (!component3.IsDead() && component3.IsEnemyTo(forPlayer) && (includeExplosions || !(component3 is DamagedExplosionObject)))
				{
					yield return damageableObjects[i].transform;
				}
			}
		}
	}

	[CompilerGenerated]
	internal sealed class _003CMoveToGameScene_003Ed__95 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Initializer _003C_003E4__this;

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
		public _003CMoveToGameScene_003Ed__95(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			SceneInfo infoScene;
			AsyncOperation asyncOperation;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap && WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(_003C_003E4__this.goMapName) ? Defs.filterMaps[_003C_003E4__this.goMapName] : 0);
				}
				UnityEngine.Debug.Log("MoveToGameScene");
				goto IL_0098;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0098;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0098:
				if (PhotonNetwork.room == null)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				PhotonNetwork.isMessageQueueRunning = false;
				infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
				LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + infoScene.NameScene) as Texture2D;
				LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
				LoadConnectScene.sceneToLoad = infoScene.NameScene;
				LoadConnectScene.noteToShow = null;
				asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
				FriendsController.sharedController.GetFriendsData();
				_003C_003E2__current = asyncOperation;
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

	public Texture killstreakeAtlas;

	public GameObject tc;

	public GameObject tempCam;

	public bool isDisconnect;

	public int countConnectToRoom;

	public float timerShowNotConnectToRoom;

	public UIButton buttonCancel;

	public UILabel descriptionLabel;

	public bool isCancelReConnect;

	private GameObject _playerPrefab;

	private UnityEngine.Object networkTablePref;

	private bool _isMultiplayer;

	private bool isLeavingRoom;

	public bool isNotConnectRoom;

	private Vector2 scrollPosition = Vector2.zero;

	private List<Vector3> _initPlayerPositions = new List<Vector3>();

	private List<float> _rots = new List<float>();

	public static List<NetworkStartTable> networkTables = new List<NetworkStartTable>();

	public static readonly List<Player_move_c> players = new List<Player_move_c>();

	public static List<Player_move_c> bluePlayers = new List<Player_move_c>();

	public static List<Player_move_c> redPlayers = new List<Player_move_c>();

	public static List<GameObject> playersObj = new List<GameObject>();

	public static List<GameObject> enemiesObj = new List<GameObject>();

	public static List<GameObject> turretsObj = new List<GameObject>();

	public static List<GameObject> petsObj = new List<GameObject>();

	public static List<GameObject> damageableObjects = new List<GameObject>();

	public static List<SingularityHole> singularities = new List<SingularityHole>();

	public static FlagController flag1;

	public static FlagController flag2;

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public float timerShow = -1f;

	public Transform playerPrefab;

	public Texture fonLoadingScene;

	private bool showLoading;

	private bool isMulti;

	private bool isInet;

	private PauseONGuiDrawer _onGUIDrawer;

	public static int GameModeCampaign = 100;

	public static int GameModeSurvival = 101;

	public static int lastGameMode = -1;

	private Stopwatch _gameSessionStopwatch = new Stopwatch();

	public string goMapName = "";

	public static bool isLocalServer = false;

	private bool _needReconnectShow;

	private bool _roomNotExistShow;

	private IDisposable someWindowBackFromReconnectSubscription;

	[NonSerialized]
	public bool isLastMatchWin;

	public LoadingNGUIController _loadingNGUIController;

	private static readonly Rilisoft.Lazy<string> _separator = new Rilisoft.Lazy<string>(InitialiseSeparatorWrapper);

	public static Initializer Instance { get; private set; }

	internal static string Separator
	{
		get
		{
			return _separator.Value;
		}
	}

	public static event Action PlayerAddedEvent;

	public static bool IsEnemyTarget(Transform _target, Player_move_c forPlayer = null)
	{
		if (forPlayer == null)
		{
			forPlayer = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (forPlayer == null || _target == null || _target.Equals(forPlayer.myPlayerTransform))
		{
			return false;
		}
		IDamageable component = _target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(forPlayer))
		{
			return false;
		}
		return true;
	}

	public static Player_move_c GetPlayerMoveCWithPhotonOwnerID(int id)
	{
		foreach (Player_move_c player in players)
		{
			if (player.mySkinName.photonView != null && player.mySkinName.photonView.ownerId == id)
			{
				return player;
			}
		}
		return null;
	}

	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", GetType().Name);
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			Instance = this;
			isMulti = Defs.isMulti;
			isInet = Defs.isInet;
			lastGameMode = -1;
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			if (!GameConnect.isSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted))
			{
				networkTablePref = Resources.Load("NetworkTable");
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
			GameObject gameObject = null;
			string text = SceneManager.GetActiveScene().name;
			if (!Defs.isMulti)
			{
				gameObject = ((CurrentCampaignGame.currentLevel != 0) ? (Resources.Load("BackgroundMusic/BackgroundMusic_Level" + CurrentCampaignGame.currentLevel) as GameObject) : (Resources.Load("BackgroundMusic/" + (GameConnect.isSurvival ? "BackgroundMusic_Level0" : (GameConnect.isSpeedrun ? "BackgroundMusic_Speedrun" : "Background_Training"))) as GameObject));
			}
			else
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(text);
				if (infoScene != null)
				{
					GlobalGameController.currentLevel = infoScene.indexMap;
				}
				gameObject = Resources.Load("BackgroundMusic/BackgroundMusic_Level" + GlobalGameController.currentLevel) as GameObject;
			}
			if ((bool)gameObject)
			{
				UnityEngine.Object.Instantiate(gameObject);
			}
			if (!Defs.isMulti && GameConnect.isCampaign)
			{
				StoreKitEventListener.State.PurchaseKey = "In game";
				StoreKitEventListener.State.Parameters.Clear();
				StoreKitEventListener.State.Parameters.Add("Level", text + " In game");
				GameObject[] array = GameObject.FindGameObjectsWithTag("Configurator");
				if (array.Length != 0)
				{
					bool flag = !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None;
					for (int i = 0; i != array.Length; i++)
					{
						CoinConfigurator component = array[i].GetComponent<CoinConfigurator>();
						if (!(component == null) && component.CoinIsPresent)
						{
							VirtualCurrencyBonusType bonusType = ((component.BonusType == VirtualCurrencyBonusType.None) ? VirtualCurrencyBonusType.Coin : component.BonusType);
							if (!CoinBonus.GetLevelsWhereGotBonus(bonusType).Contains(text) || flag)
							{
								CreateBonusAtPosition((component.coinCreatePoint == null) ? component.pos : component.coinCreatePoint.position, bonusType);
							}
						}
					}
				}
				lastGameMode = GameModeCampaign;
			}
			else if (!Defs.isMulti && GameConnect.isSurvival)
			{
				lastGameMode = GameModeSurvival;
			}
			else if (Defs.isMulti)
			{
				lastGameMode = (int)GameConnect.gameMode;
			}
			string abuseKey_d4d3cbab = GetAbuseKey_d4d3cbab(3570650027u);
			if (Storager.hasKey(abuseKey_d4d3cbab))
			{
				string @string = Storager.getString(abuseKey_d4d3cbab);
				if (!string.IsNullOrEmpty(@string) && @string != "0")
				{
					long num = DateTime.UtcNow.Ticks >> 1;
					long result = num;
					if (long.TryParse(@string, out result))
					{
						result = Math.Min(num, result);
						Storager.setString(abuseKey_d4d3cbab, result.ToString());
					}
					else
					{
						Storager.setString(abuseKey_d4d3cbab, num.ToString());
					}
					TimeSpan timeSpan = TimeSpan.FromTicks(num - result);
					Player_move_c.AnotherNeedApply = (Player_move_c.NeedApply = (Defs.IsDeveloperBuild ? (timeSpan.TotalMinutes >= 3.0) : (timeSpan.TotalDays >= 1.0)));
				}
			}
			GameConnect.OnJoinToMap = (Action<string>)Delegate.Combine(GameConnect.OnJoinToMap, new Action<string>(OnJoinToMap));
			GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Combine(GameConnect.OnJoinRoomFailed, new Action<bool>(OnPhotonJoinRoomFailed));
			GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Combine(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnectToPhoton));
			GameConnect.OnConnectedMaster = (Action)Delegate.Combine(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
			GameConnect.OnDisconnected = (Action)Delegate.Combine(GameConnect.OnDisconnected, new Action(OnDisconnectedFromPhoton));
			GameConnect.OnConnectionFailed = (GameConnect.OnDisconnectReason)Delegate.Combine(GameConnect.OnConnectionFailed, new GameConnect.OnDisconnectReason(OnConnectionFail));
			GameConnect.OnJoinedToRoom = (Action)Delegate.Combine(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
			GameConnect.OnLeftRoomEvent = (Action)Delegate.Combine(GameConnect.OnLeftRoomEvent, new Action(OnLeftRoom));
		}
	}

	private static string GetAbuseKey_d4d3cbab(uint pad)
	{
		return (0x1039BA92u ^ pad).ToString("x");
	}

	internal static GameObject CreateBonusAtPosition(Vector3 position, VirtualCurrencyBonusType bonusType)
	{
		string empty = string.Empty;
		switch (bonusType)
		{
		case VirtualCurrencyBonusType.Coin:
			empty = "coin";
			break;
		case VirtualCurrencyBonusType.CoinSpeedrun:
			empty = "coinSpeedrun";
			break;
		case VirtualCurrencyBonusType.Gem:
			empty = "gem";
			break;
		default:
			UnityEngine.Debug.LogErrorFormat("Failed to determine resource for '{0}'", bonusType);
			return null;
		}
		UnityEngine.Object @object = Resources.Load(empty);
		if (@object == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to load '{0}'", empty);
			return null;
		}
		UnityEngine.Object object2 = UnityEngine.Object.Instantiate(@object, position, Quaternion.identity);
		if (object2 == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to instantiate '{0}'", empty);
			return null;
		}
		GameObject gameObject = object2 as GameObject;
		if (gameObject == null)
		{
			return gameObject;
		}
		CoinBonus component = gameObject.GetComponent<CoinBonus>();
		if (component == null)
		{
			UnityEngine.Debug.LogErrorFormat("Cannot find '{0}' script.", typeof(CoinBonus).Name);
			return gameObject;
		}
		component.BonusType = bonusType;
		return gameObject;
	}

	private bool CheckRoom()
	{
		if (PhotonNetwork.room != null)
		{
			if (PhotonNetwork.room.maxPlayers < 2 || PhotonNetwork.room.maxPlayers > (GameConnect.isCOOP ? 4 : (GameConnect.isDuel ? 2 : 10)))
			{
				goToConnect();
			}
			if (GameConnect.isDuel && (DuelController.RoomStatus)PhotonNetwork.room.customProperties[GameConnect.roomStatusProperty] == DuelController.RoomStatus.Closed)
			{
				Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
				isDisconnect = true;
				isLeavingRoom = true;
				PhotonNetwork.LeaveRoom();
				countConnectToRoom = 6;
				OnPhotonJoinRoomFailed();
				return false;
			}
		}
		return true;
	}

	private void Start()
	{
		if (!MainMenuController.NavigateToMinigame.HasValue && Defs.isMulti && !Defs.isGameFromFriends)
		{
			ConnectScene.isReturnFromGame = true;
		}
		FriendsController.sharedController.profileInfo.Clear();
		FriendsController.sharedController.notShowAddIds.Clear();
		FacebookController.LogEvent("Campaign_ACHIEVED_LEVEL");
		Defs.inRespawnWindow = false;
		NetworkStartTable.StartAfterDisconnect = false;
		PhotonNetwork.isMessageQueueRunning = true;
		_isMultiplayer = Defs.isMulti;
		_weaponManager = WeaponManager.sharedManager;
		_weaponManager.players.Clear();
		CheckRoom();
		if (PhotonNetwork.room != null)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
			goMapName = infoScene.NameScene;
		}
		if (!_isMultiplayer)
		{
			_initPlayerPositions.Add(new Vector3(12f, 1f, 9f));
			_initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			_initPlayerPositions.Add(new Vector3(-42f, 1f, -10.487f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 19.5f));
			_initPlayerPositions.Add(new Vector3(-33f, 1.2f, -13f));
			_initPlayerPositions.Add(new Vector3(-2.67f, 1f, 2.67f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(19f, 1f, -0.8f));
			_initPlayerPositions.Add(new Vector3(-28.5f, 1.75f, -3.73f));
			_initPlayerPositions.Add(new Vector3(-2.5f, 1.75f, 0f));
			_initPlayerPositions.Add(new Vector3(-1.596549f, 2.5f, 2.684792f));
			_initPlayerPositions.Add(new Vector3(-6.611357f, 1.5f, -105.2573f));
			_initPlayerPositions.Add(new Vector3(-20.3f, 2f, 17.6f));
			_initPlayerPositions.Add(new Vector3(5f, 2.5f, 0f));
			_initPlayerPositions.Add(new Vector3(0f, 2.5f, 0f));
			_initPlayerPositions.Add(new Vector3(-7.3f, 3.6f, 6.46f));
			_initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			_initPlayerPositions.Add(new Vector3(17f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(0.2f, 11.2f, -0.28f));
			_initPlayerPositions.Add(new Vector3(-1.76f, 100.9f, 20.8f));
			_initPlayerPositions.Add(new Vector3(20f, -0.4f, 17f));
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(180f);
			_rots.Add(180f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(270f);
			_rots.Add(270f);
			_rots.Add(270f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(90f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(90f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(0f);
			if (Storager.getInt(Defs.EarnedCoins) > 0)
			{
				UnityEngine.Object.Instantiate(Resources.Load("MessageCoinsObject") as GameObject);
			}
			AddPlayer();
		}
		else
		{
			tc = UnityEngine.Object.Instantiate(tempCam, Vector3.zero, Quaternion.identity);
			if (!Defs.isInet)
			{
				if (isLocalServer)
				{
				}
			}
			else
			{
				_weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
				if (_weaponManager.myTable != null)
				{
					_weaponManager.myNetworkStartTable = _weaponManager.myTable.GetComponent<NetworkStartTable>();
				}
				else
				{
					OnConnectionFail(1040);
				}
			}
		}
		_gameSessionStopwatch.Start();
	}

	
	[PunRPC]
	private void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
	{
		if (networkTablePref != null)
		{
			(UnityEngine.Object.Instantiate(networkTablePref, pos, rot) as Transform).GetComponent<PhotonView>().viewID = id1;
		}
	}

	private void AddPlayer()
	{
		_playerPrefab = Resources.Load<GameObject>("Player");
		GameObject gameObject = UnityEngine.Object.Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
		SetPlayerInStartPoint(gameObject);
		NickLabelController.currentCamera = gameObject.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
		Invoke("SetupObjectThatNeedsPlayer", 0.01f);
	}

	public void SetPlayerInStartPoint(GameObject player)
	{
		Vector3 position = Vector3.zero;
		float y = 0f;
		if (GameConnect.isSurvival)
		{
			if (SceneLoader.ActiveSceneName.Equals("Arena_Underwater"))
			{
				position = new Vector3(0f, 3.5f, 0f);
				y = 0f;
			}
			else if (SceneLoader.ActiveSceneName.Equals("Pizza"))
			{
				position = new Vector3(-32.48f, 2.46f, 2.01f);
				y = 90f;
			}
			else
			{
				position = new Vector3(0f, 2.5f, 0f);
				y = 0f;
			}
		}
		else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			TrainingController trainingController = UnityEngine.Object.FindObjectOfType<TrainingController>();
			position = ((trainingController != null) ? trainingController.PlayerDesiredPosition : TrainingController.PlayerDefaultPosition);
			y = 0f;
		}
		else if (GameConnect.isCampaign)
		{
			int index = Mathf.Max(0, CurrentCampaignGame.currentLevel - 1);
			position = ((CurrentCampaignGame.currentLevel == 0) ? new Vector3(-0.72f, 1.75f, -13.23f) : _initPlayerPositions[index]);
			y = ((CurrentCampaignGame.currentLevel == 0) ? 0f : _rots[index]);
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerRespawnPoint");
			if (gameObject != null)
			{
				position = gameObject.transform.position;
				y = gameObject.transform.rotation.eulerAngles.y;
			}
		}
		else
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("PlayerRespawnPoint");
			if (gameObject2 != null)
			{
				position = gameObject2.transform.position;
				y = gameObject2.transform.rotation.eulerAngles.y;
			}
			else
			{
				UnityEngine.Debug.LogError("No respawn point for this mode!");
			}
		}
		player.transform.position = position;
		player.transform.rotation = Quaternion.Euler(0f, y, 0f);
	}

	public void SetupObjectThatNeedsPlayer()
	{
		if (Defs.isMulti)
		{
			if (Initializer.PlayerAddedEvent != null)
			{
				Initializer.PlayerAddedEvent();
			}
			return;
		}
		if (GameConnect.isCampaign || GameConnect.isSurvival)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("CoinBonus");
			for (int i = 0; i != array.Length; i++)
			{
				CoinBonus component = array[i].GetComponent<CoinBonus>();
				if (component != null)
				{
					component.SetPlayer();
				}
			}
			if (TrainingController.TrainingCompleted)
			{
				ZombieCreator.sharedCreator.BeganCreateEnemies();
			}
			GetComponent<BonusCreator>().BeginCreateBonuses();
		}
		if (Initializer.PlayerAddedEvent != null)
		{
			Initializer.PlayerAddedEvent();
		}
	}

	private void ShowDescriptionLabel(string text)
	{
		descriptionLabel.gameObject.SetActive(true);
		descriptionLabel.text = text;
	}

	public void HideReconnectInterface()
	{
		descriptionLabel.gameObject.SetActive(false);
		buttonCancel.gameObject.SetActive(false);
		if (someWindowBackFromReconnectSubscription != null)
		{
			someWindowBackFromReconnectSubscription.Dispose();
			someWindowBackFromReconnectSubscription = null;
		}
	}

	public void OnCancelButtonClick()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || interfaceEnabled2 || ExpController.Instance.LevelUpPanelOpened || (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			Invoke("OnCancelButtonClick", 60f);
			return;
		}
		isCancelReConnect = true;
		goToConnect();
	}

	private void ReconnectGUI()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.Map((ProfileController p) => p.InterfaceEnabled);
		if (guiActive || interfaceEnabled || flag || !isDisconnect || (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			return;
		}
		if (timerShowNotConnectToRoom > 0f)
		{
			timerShowNotConnectToRoom -= Time.deltaTime;
			if (timerShowNotConnectToRoom > 0f)
			{
				if (!_needReconnectShow)
				{
					_needReconnectShow = true;
					ShowDescriptionLabel(LocalizationStore.Get((GameConnect.isMiniGame && MiniGamesController.Instance.isGo) ? "Key_3153" : "Key_1005"));
					buttonCancel.gameObject.SetActive(false);
					if (someWindowBackFromReconnectSubscription != null)
					{
						someWindowBackFromReconnectSubscription.Dispose();
						someWindowBackFromReconnectSubscription = null;
					}
				}
			}
			else
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(goMapName);
				if (infoScene != null && (!GameConnect.isMiniGame || !MiniGamesController.Instance.isGo))
				{
					isDisconnect = false;
					JoinRandomRoom(infoScene);
				}
				else
				{
					goToConnect();
				}
			}
		}
		else if (!_roomNotExistShow)
		{
			_roomNotExistShow = true;
			ShowDescriptionLabel(LocalizationStore.Get("Key_1004"));
			bool flag2 = !ShopNGUIController.GuiActive && !ProfileController.Instance.InterfaceEnabled;
			buttonCancel.gameObject.SetActive(flag2);
			if (flag2 && someWindowBackFromReconnectSubscription == null)
			{
				someWindowBackFromReconnectSubscription = BackSystem.Instance.Register(OnCancelButtonClick, "Cancel from reconnect");
			}
			if (!flag2 && someWindowBackFromReconnectSubscription != null)
			{
				someWindowBackFromReconnectSubscription.Dispose();
				someWindowBackFromReconnectSubscription = null;
			}
		}
	}

	private void Update()
	{
		if ((bool)_onGUIDrawer)
		{
			_onGUIDrawer.gameObject.SetActive(isDisconnect || showLoading);
		}
		if (timerShow > 0f)
		{
			timerShow -= Time.deltaTime;
			showLoading = true;
			fonLoadingScene = null;
			Invoke("goToConnect", 0.1f);
		}
		ReconnectGUI();
	}

	private void OnConnectedToServer()
	{
	}

	private void OnDestroy()
	{
		if (!MainMenuController.NavigateToMinigame.HasValue && isLastMatchWin && ReviewController.IsNeedActive)
		{
			ConnectScene.NeedShowReviewInConnectScene = true;
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		Instance = null;
		players.Clear();
		bluePlayers.Clear();
		redPlayers.Clear();
		Defs.showTableInNetworkStartTable = false;
		Defs.showNickTableInNetworkStartTable = false;
		if ((bool)_onGUIDrawer)
		{
			_onGUIDrawer.act = null;
		}
		_gameSessionStopwatch.Stop();
		if (lastGameMode == GameModeCampaign || lastGameMode == GameModeSurvival)
		{
			NetworkStartTable.IncreaseTimeInMode(lastGameMode, _gameSessionStopwatch.Elapsed.TotalMinutes);
		}
		ExperienceController.sharedController.isShowRanks = false;
		if (ConnectScene.NeedShowReviewInConnectScene)
		{
			UnityEngine.Debug.Log("[Rilisoft] Skip requesting interstitial: `ConnectScene.NeedShowReviewInConnectScene`");
		}
		else if (!Defs.isMulti)
		{
			UnityEngine.Debug.Log("[Rilisoft] Skip requesting interstitial: `!Defs.isMulti`");
		}
		else
		{
			bool flag = string.IsNullOrEmpty(ConnectScene.GetReasonToDismissFakeInterstitial()) && ReplaceAdmobPerelivController.sharedController != null;
			if (Tools.IsEditor)
			{
				flag = true;
			}
			UnityEngine.Debug.LogFormat("[Rilisoft] Prepare requesting interstitial: `shouldShowReplaceAdmob = {0}`", flag);
			if (flag)
			{
				ReplaceAdmobPerelivController.IncreaseTimesCounter();
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
				{
					ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Requesting interstitial: ConnectScene.ReplaceAdmobWithPerelivRequest = true");
				}
				ConnectScene.ReplaceAdmobWithPerelivRequest = true;
			}
			else
			{
				string reasonToDismissInterstitialConnectScene = ConnectScene.GetReasonToDismissInterstitialConnectScene();
				if (!string.IsNullOrEmpty(reasonToDismissInterstitialConnectScene))
				{
					UnityEngine.Debug.LogFormat("Initializer.OnDestroy(): Dismissing interstitial, '{0}'", reasonToDismissInterstitialConnectScene);
				}
				else
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Requesting interstitial: ConnectScene.InterstitialRequest = true");
					}
					ConnectScene.InterstitialRequest = true;
				}
			}
		}
		Defs.inComingMessagesCounter = 0;
		NetworkStartTable.StartAfterDisconnect = false;
		GameConnect.OnJoinToMap = (Action<string>)Delegate.Remove(GameConnect.OnJoinToMap, new Action<string>(OnJoinToMap));
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Remove(GameConnect.OnJoinRoomFailed, new Action<bool>(OnPhotonJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Remove(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnectToPhoton));
		GameConnect.OnConnectedMaster = (Action)Delegate.Remove(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Remove(GameConnect.OnDisconnected, new Action(OnDisconnectedFromPhoton));
		GameConnect.OnConnectionFailed = (GameConnect.OnDisconnectReason)Delegate.Remove(GameConnect.OnConnectionFailed, new GameConnect.OnDisconnectReason(OnConnectionFail));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Remove(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
		GameConnect.OnLeftRoomEvent = (Action)Delegate.Remove(GameConnect.OnLeftRoomEvent, new Action(OnLeftRoom));
	}

	public void goToConnect()
	{
		UnityEngine.Debug.Log("goToConnect()");
		ConnectScene.Local();
	}

	private void GoToRandomRoom()
	{
	}

	private void ShowLoadingGUI(string _mapName)
	{
		_loadingNGUIController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		_loadingNGUIController.SceneToLoad = _mapName;
		_loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load(_mapName.Equals("main_loading") ? "" : ("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _mapName)) as Texture2D;
		_loadingNGUIController.transform.localPosition = Vector3.zero;
		_loadingNGUIController.Init();
		ExperienceController.sharedController.isShowRanks = false;
	}

	public void OnLeftRoom()
	{
		UnityEngine.Debug.Log("OnLeftRoom (local) init");
		NickLabelController.currentCamera = null;
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			showLoading = true;
			fonLoadingScene = null;
			Invoke("goToConnect", 0.1f);
			ShowLoadingGUI("main_loading");
			if (_weaponManager == null || _weaponManager.myTable == null)
			{
				return;
			}
			_weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
			_weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			bool guiActive = ShopNGUIController.GuiActive;
			bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
			bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
			if (!guiActive && !interfaceEnabled && !interfaceEnabled2)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (string.IsNullOrEmpty(goMapName))
			{
				ShowLoadingGUI(goMapName);
			}
		}
	}

	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.Debug.Log("OnDisconnectedFromPhotoninit");
		OnConnectionFail(0);
	}

	private void OnConnectionFail(int cause)
	{
		BankController.canShowIndication = true;
		Defs.inRespawnWindow = false;
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap || Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInDuel || Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInHunger)
		{
			if (_loadingNGUIController != null)
			{
				UnityEngine.Object.Destroy(_loadingNGUIController.gameObject);
				_loadingNGUIController = null;
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			goToConnect();
			return;
		}
		if (WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		timerShowNotConnectToRoom = -1f;
		isCancelReConnect = false;
		isNotConnectRoom = false;
		countConnectToRoom = 0;
		UnityEngine.Debug.Log("OnConnectionFail " + (DisconnectCause)cause);
		tc.SetActive(true);
		if (BonusController.sharedController != null)
		{
			BonusController.sharedController.ClearBonuses();
		}
		for (int i = 0; i < enemiesObj.Count; i++)
		{
			UnityEngine.Object.Destroy(enemiesObj[i]);
		}
		GameObject gameObject = ((InGameGUI.sharedInGameGUI != null) ? InGameGUI.sharedInGameGUI.gameObject : GameObject.FindGameObjectWithTag("InGameGUI"));
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		isDisconnect = true;
		Invoke("ConnectToPhoton", 3f);
		Invoke("OnCancelButtonClick", 60f);
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (!guiActive && !interfaceEnabled && !interfaceEnabled2 && !ExpController.Instance.LevelUpPanelOpened && (!(NetworkStartTableNGUIController.sharedController != null) || !NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			ActivityIndicator.IsActiveIndicator = true;
			ExperienceController.sharedController.isShowRanks = false;
			if (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.shopAnchor != null)
			{
				NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
			}
		}
	}

	private void ConnectToPhoton()
	{
		if (isCancelReConnect)
		{
			return;
		}
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || interfaceEnabled2 || ExpController.Instance.LevelUpPanelOpened || (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow) || ExpController.Instance.WaitingForLevelUpView)
		{
			Invoke("ConnectToPhoton", 3f);
			return;
		}
		UnityEngine.Debug.Log("ConnectToPhoton ");
		ActivityIndicator.IsActiveIndicator = true;
		ExperienceController.sharedController.isShowRanks = false;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
		}
		PhotonNetwork.autoJoinLobby = false;
		GameConnect.ConnectToPhoton();
	}

	private static string InitialiseSeparatorWrapper()
	{
		return InitializeSeparator();
	}

	private static string InitializeSeparator()
	{
		return "7cada1d8";
	}

	private void OnFailedToConnectToPhoton(int parameters)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("NetworkStartTableNGUI");
		if (gameObject != null)
		{
			NetworkStartTableNGUIController component = gameObject.GetComponent<NetworkStartTableNGUIController>();
			if (component != null)
			{
				component.shopAnchor.SetActive(false);
			}
		}
		UnityEngine.Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + (DisconnectCause)parameters);
		if (!isCancelReConnect)
		{
			Invoke("ConnectToPhoton", 3f);
		}
	}

	public void OnConnectedToMaster()
	{
		if (isLeavingRoom)
		{
			isLeavingRoom = false;
		}
		else
		{
			ConnectToRoom();
		}
	}

	private void ConnectToRoom()
	{
		CancelInvoke("OnCancelButtonClick");
		SceneInfo map = ((!string.IsNullOrEmpty(goMapName)) ? SceneInfoController.instance.GetInfoScene(goMapName) : null);
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInHunger)
		{
			UnityEngine.Debug.Log("JoinRandomRoom");
			isCancelReConnect = true;
			ItemPrice itemPrice = new ItemPrice(BalanceController.ParametersForMiniGameType(GameConnect.gameMode).TicketsPrice, "TicketsCurrency");
			AnalyticsStuff.TicketsSpended(GameConnect.gameMode.ToString(), itemPrice.Price);
			AnalyticsStuff.MiniGames(GameConnect.gameMode);
			BankController.SpendMoney(itemPrice);
			ActivityIndicator.IsActiveIndicator = true;
			GameConnect.ConnectToRandomRoom();
		}
		else if (Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInDuel)
		{
			UnityEngine.Debug.Log("JoinRandomRoom");
			isCancelReConnect = true;
			ActivityIndicator.IsActiveIndicator = true;
			GameConnect.ConnectToRandomRoom();
		}
		else if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			UnityEngine.Debug.Log("ConnectToRoom() " + goMapName);
			JoinRandomRoom(map);
		}
		else
		{
			UnityEngine.Debug.Log("ConnectToRoom " + PlayerPrefs.GetString("RoomName"));
			if (!isCancelReConnect)
			{
				PhotonNetwork.JoinRoom(PlayerPrefs.GetString("RoomName"));
			}
		}
	}

	private void OnPhotonJoinRoomFailed(bool onCreate = false)
	{
		if (GameConnect.gameTier != GameConnect.GetTierForRoom())
		{
			GameConnect.gameTier = GameConnect.GetTierForRoom();
			GameConnect.Disconnect();
			return;
		}
		countConnectToRoom++;
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		isNotConnectRoom = true;
		if (countConnectToRoom < 6)
		{
			Invoke("ConnectToRoom", 3f);
		}
		else
		{
			timerShowNotConnectToRoom = 3f;
		}
	}

	private void JoinRandomRoom(SceneInfo _map)
	{
		GameConnect.ConnectToRandomRoom(_map);
	}

	public void OnJoinToMap(string mapName)
	{
		if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap)
		{
			goMapName = mapName;
		}
		UnityEngine.Debug.Log("JoinRandomRoom " + goMapName);
		if (!string.IsNullOrEmpty(goMapName))
		{
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(goMapName) ? Defs.filterMaps[goMapName] : 0);
			}
			ActivityIndicator.IsActiveIndicator = true;
		}
	}

	private void StartGameAfterDisconnectInvoke()
	{
		if (GameConnect.AllowReconnect && !Defs.showTableInNetworkStartTable && !Defs.showNickTableInNetworkStartTable)
		{
			NetworkStartTable.StartAfterDisconnect = true;
		}
		_weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
		_weaponManager.myNetworkStartTable = _weaponManager.myTable.GetComponent<NetworkStartTable>();
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void OnJoinedRoom()
	{
		if (CheckRoom())
		{
			UnityEngine.Debug.Log("OnJoinedRoom - init");
			PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
			Instance.goMapName = infoScene.NameScene;
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Instance.goMapName) ? Defs.filterMaps[Instance.goMapName] : 0);
			}
			if (isDisconnect && GameConnect.AllowReconnectWithScore)
			{
				Invoke("StartGameAfterDisconnectInvoke", 3f);
			}
			else
			{
				GlobalGameController.healthMyPlayer = 0f;
				NetworkStartTable.StartAfterDisconnect = false;
				PhotonNetwork.isMessageQueueRunning = false;
				StartCoroutine(MoveToGameScene());
			}
			isDisconnect = false;
			_roomNotExistShow = false;
			_needReconnectShow = false;
			HideReconnectInterface();
		}
	}

	private IEnumerator MoveToGameScene()
	{
		if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap && WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(goMapName) ? Defs.filterMaps[goMapName] : 0);
		}
		UnityEngine.Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PhotonNetwork.isMessageQueueRunning = false;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + infoScene.NameScene) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = infoScene.NameScene;
		LoadConnectScene.noteToShow = null;
		AsyncOperation asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
		FriendsController.sharedController.GetFriendsData();
		yield return asyncOperation;
	}
}
