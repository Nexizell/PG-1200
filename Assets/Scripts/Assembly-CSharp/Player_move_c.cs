using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using Rilisoft.WP8;
using RilisoftBot;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player_move_c : MonoBehaviour
{
	public enum TypeBonuses
	{
		Ammo = 0,
		Health = 1,
		Armor = 2,
		Grenade = 3
	}

	public enum TypeKills
	{
		none = 0,
		himself = 1,
		headshot = 2,
		explosion = 3,
		zoomingshot = 4,
		flag = 5,
		grenade = 6,
		grenade_hell = 7,
		turret = 8,
		killTurret = 9,
		mech = 10,
		like = 11,
		poison = 12,
		reflector = 13,
		burning = 14,
		pet = 15,
		mob = 16,
		bleeding = 17,
		critical = 18
	}

	public struct SystemMessage
	{
		public string nick1;

		public string message2;

		public string nick2;

		public string message;

		public Color textColor;

		public SystemMessage(string nick1, string message2, string nick2, string message, Color textColor)
		{
			this.nick1 = nick1;
			this.message2 = message2;
			this.nick2 = nick2;
			this.message = message;
			this.textColor = textColor;
		}
	}

	public struct MessageChat
	{
		public string text;

		public float time;

		public int ID;

		public int command;

		public bool isClanMessage;

		public Texture clanLogo;

		public string clanID;

		public string clanName;

		public NetworkViewID IDLocal;

		public string iconName;
	}

	public delegate void OnMessagesUpdate();

	public enum PlayerEffect
	{
		none = 0,
		burning = 1,
		fireMushroom = 2,
		disabler = 3,
		blackMark = 4,
		dragon = 5,
		lightningEnemies = 6,
		disablerEffect = 7,
		resurrection = 8,
		attrackPlayer = 9,
		timeTravel = 10,
		lightningSelf = 11,
		rocketFly = 12,
		clearPoisons = 13,
		charm = 14,
		voodooSnowman = 15
	}

	public struct ActivePlayerEffect
	{
		public PlayerEffect effect;

		public float lifeTime;

		public Player_move_c sender;

		public ActivePlayerEffect(PlayerEffect effect, float time)
		{
			this.effect = effect;
			lifeTime = Time.time + time;
			sender = null;
		}

		public ActivePlayerEffect(PlayerEffect effect, float time, int sender)
		{
			this.effect = effect;
			lifeTime = Time.time + time;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.players[i].skinNamePixelView.viewID == sender)
				{
					this.sender = Initializer.players[i];
					return;
				}
			}
			this.sender = null;
		}

		public ActivePlayerEffect UpdateTime(float time)
		{
			return new ActivePlayerEffect(effect, time);
		}

		public ActivePlayerEffect UpdateTime(float time, int sender)
		{
			return new ActivePlayerEffect(effect, time, sender);
		}
	}

	public enum GadgetEffect
	{
		reflector = 0,
		mech = 1,
		invisible = 2,
		jetpack = 3,
		demon = 4,
		disabler = 5,
		drumSupport = 6,
		petAdrenaline = 7,
		protectionAmulet = 8
	}

	public struct GadgetEffectParams
	{
		public GadgetEffect effect;

		public string gadgetID;

		public GadgetEffectParams(GadgetEffect effect, string gadgetID)
		{
			this.effect = effect;
			this.gadgetID = gadgetID;
		}
	}

	public enum PoisonType
	{
		None = 0,
		Toxic = 1,
		Burn = 2,
		Bleeding = 3
	}

	public struct PoisonParameters
	{
		public PoisonType poisonType;

		public float multiplayerDamage;

		public float poisonTime;

		public int poisonCount;

		public string weaponName;

		public WeaponSounds.TypeDead typeDead;

		public PoisonParameters(PoisonType poisonType, float dmPercent, float damageMultiplayer, float poisonTime, int poisonCount, string weaponName, WeaponSounds.TypeDead typeDead)
		{
			this.poisonType = poisonType;
			multiplayerDamage = damageMultiplayer * dmPercent;
			this.poisonTime = poisonTime;
			this.poisonCount = poisonCount;
			this.weaponName = weaponName;
			this.typeDead = typeDead;
		}

		public PoisonParameters(float damageMultiplayer, WeaponSounds weapon)
		{
			poisonType = weapon.poisonType;
			multiplayerDamage = damageMultiplayer * weapon.poisonDamageMultiplier;
			poisonTime = weapon.poisonTime;
			poisonCount = weapon.poisonCount;
			weaponName = weapon.gameObject.name.Replace("(Clone)", string.Empty);
			typeDead = weapon.typeDead;
		}
	}

	internal class PoisonTarget
	{
		public IDamageable target;

		public int hitCount;

		public float nextHitTime;

		public PoisonParameters param;

		public PoisonTarget(IDamageable target, PoisonParameters poison)
		{
			this.target = target;
			hitCount = poison.poisonCount;
			param = poison;
			nextHitTime = Time.time;
		}

		public void UpdatePoison(PoisonParameters poison)
		{
			hitCount = poison.poisonCount;
			param = poison;
		}
	}

	public struct RayHitsInfo
	{
		public RaycastHit[] hits;

		public bool obstacleFound;

		public float lenRay;

		public Ray rayReflect;
	}

	private int tierForKilledRate;

	private readonly Dictionary<string, int> weKillForKillRate = new Dictionary<string, int>();

	private readonly Dictionary<string, int> weWereKilledForKillRate = new Dictionary<string, int>();

	public PixelView pixelView;

	public ColliderCollisions[] petPointsFlying;

	public ColliderCollisions[] petPointsGround;

	public TextMesh nickLabel;

	public AudioClip mechBearActivSound;

	public AudioClip potionSound;

	public AudioClip invisibleActivSound;

	public AudioClip invisibleDeactivSound;

	public AudioClip jetpackActivSound;

	public AudioClip portalSound;

	public PlayerScoreController myScoreController;

	public bool isRocketJump;

	public float armorSynch;

	public bool isReloading;

	public bool _isPlacemarker;

	public GameObject placemarkerMark;

	public GameObject blackMark;

	public Player_move_c placemarkerMoveC;

	public bool isResurrectionKill;

	public Player_move_c resurrectionMoveC;

	private static string weaponKey = "Weapon";

	[Header("0-Ammo; 1-Health; 2-Armor; 3-Grenade")]
	public ParticleBonuse[] bonusesParticles;

	public KillStreakMapper killStreakParticles;

	public GameObject particleBonusesPoint;

	public Transform myTransform;

	public Transform myPlayerTransform;

	public int myPlayerID;

	public SkinName mySkinName;

	public GameObject mechBearPoint;

	public GameObject mechBearBody;

	public GameObject mechBearHands;

	public Animation mechBearBodyAnimation;

	public GameObject fpsPlayerBody;

	public GameObject myCurrentWeapon;

	public WeaponSounds myCurrentWeaponSounds;

	public GameObject mechExplossion;

	public GameObject bearExplosion;

	public AudioSource mechExplossionSound;

	public AudioSource mechBearExplosionSound;

	public SkinnedMeshRenderer playerBodyRenderer;

	public SkinnedMeshRenderer mechBearBodyRenderer;

	public SkinnedMeshRenderer mechBearHandRenderer;

	public SynhRotationWithGameObject mechBearSyncRot;

	public Transform PlayerHeadTransform;

	public Transform MechHeadTransform;

	public CapsuleCollider bodyCollayder;

	public CapsuleCollider headCollayder;

	private int numShootInDoubleShot = 1;

	public bool isMechActive;

	public bool isBearActive;

	public AudioClip flagGetClip;

	public AudioClip flagLostClip;

	public AudioClip flagScoreEnemyClip;

	public AudioClip flagScoreMyCommandClip;

	public float deltaAngle;

	public GameObject playerDeadPrefab;

	public PlayerSynchStream myPersonNetwork;

	public GameObject likePrefab;

	public GameObject turretPrefab;

	public GameObject turretPoint;

	public GameObject currentTurret;

	public float liveMech = 9f;

	public float[] liveMechByTier;

	private ThrowGadget currentGrenadeGadget;

	public int currentWeaponBeforeTurret = -1;

	private int currentWeaponBeforeGrenade = -1;

	private float stdFov = 90f;

	private AudioSource myAudioSource;

	private int countMultyFlag;

	private string[] iconShotName = new string[19]
	{
		"", "Chat_Death", "Chat_HeadShot", "Chat_Explode", "Chat_Sniper", "Chat_Flag", "Chat_grenade", "Chat_grenade_hell", "", "Chat_Turret_Explode",
		"", "Smile_1_15", "Chat_Poison", "Chat_Reflection", "Chat_Burning", "", "", "Chat_bleeding", ""
	};

	public bool isImVisible;

	public bool isWeaponSet;

	public NetworkStartTableNGUIController networkStartTableNGUIController;

	public GameObject invisibleParticle;

	public bool isInvisible;

	private bool isInvisByWeapon;

	private bool isInvisByGadget;

	public bool isBigHead;

	public float maxTimeSetTimerShow = 0.5f;

	private float _koofDamageWeaponFromPotoins;

	public bool isRegenerationLiveZel;

	private float maxTimerRegenerationLiveZel = 5f;

	public bool isRegenerationLiveCape;

	private float maxTimerRegenerationLiveCape = 15f;

	private float timerRegenerationLiveZel;

	private float timerRegenerationLiveCape;

	private float timerRegenerationArmor;

	private Shader[] oldShadersInInvisible;

	private Color[] oldColorInInvisible;

	public bool isCaptureFlag;

	public GameObject myBaza;

	public Camera myCamera;

	public Camera gunCamera;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public GameObject flagPoint;

	public GameObject bootsPoint;

	public GameObject armorPoint;

	public GameObject arrowToPortalPoint;

	public bool isZooming;

	public AudioClip headShotSound;

	public AudioClip flagCaptureClip;

	public AudioClip flagPointClip;

	public GameObject headShotParticle;

	public GameObject healthParticle;

	public GameObject chatViewer;

	public GameObject myTable;

	public NetworkStartTable myNetworkStartTable;

	private float[] _byCatDamageModifs = new float[6];

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	public Transform GunFlash;

	public int BulletForce = 5000;

	public bool killed;

	public ZombiManager zombiManager;

	public NickLabelController myNickLabelController;

	public visibleObjPhoton visibleObj;

	public string textChat;

	public bool showGUI = true;

	public bool showRanks;

	public SystemMessage[] killedSpisok = new SystemMessage[3];

	private Vector3 camPosition;

	private Quaternion camRotaion;

	public bool showChat;

	public bool showChatOld;

	public bool showRanksOld;

	private bool isDeadFrame;

	private int _myCommand;

	public bool respawnedForGUI = true;

	public PixelView skinNamePixelView;

	public PlayerDamageable myDamageable;

	private int _nickColorInd;

	public float liveTime;

	public float timerShowUp;

	public float timerShowLeft;

	public float timerShowDown;

	public float timerShowRight;

	public string myIp = string.Empty;

	public TrainingController trainigController;

	public bool isKilled;

	public bool theEnd;

	public string nickPobeditel;

	public Texture hitTexture;

	public Texture poisonTexture;

	public Texture _skin;

	public float showNoInetTimer = 5f;

	private static System.Random _prng = new System.Random(41472);

	private SaltedInt _killCount;

	private float _timeWhenPurchShown;

	private bool inAppOpenedFromPause;

	public bool isMulti;

	public bool isInet;

	public bool isMine;

	public bool isCompany;

	public bool isCOOP;

	private ExperienceController expController;

	private float inGameTime;

	public int multiKill;

	private bool isHunger;

	public static float maxTimerShowMultyKill = 3f;

	public FlagController flag1;

	public FlagController flag2;

	public FlagController myFlag;

	public FlagController enemyFlag;

	private GameObject rocketToLaunch;

	public bool isStartAngel;

	public float maxTimerImmortality = 3f;

	private bool _isImmortalyVal = true;

	private float timerImmortality = 3f;

	private float timerImmortalityForAlpha = 3f;

	private readonly KillerInfo _killerInfo = new KillerInfo();

	private bool showGrenadeHint = true;

	private bool showZoomHint = true;

	private bool showChangeWeaponHint = true;

	private int respawnCountForTraining;

	[NonSerialized]
	public string currentWeaponForKillCam;

	[NonSerialized]
	public int turretUpgrade;

	private bool _weaponPopularityCacheIsDirty;

	public const int GADGET_SERIAL = 6;

	public const int PETKILL_SERIAL = 7;

	private int counterPetSerial;

	private int[] counterSerials = new int[8];

	private float _curBaseArmor;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	public float[] timerShow = new float[3] { -1f, -1f, -1f };

	public AudioClip deadPlayerSound;

	public AudioClip damagePlayerSound;

	public AudioClip damageArmorPlayerSound;

	public AudioClip fallPlayerSound;

	public GameObject[] zoneCreatePlayer;

	private float mySens;

	private GameObject damage;

	private bool damageShown;

	private Pauser _pauser;

	private bool _backWasPressed;

	public GameObject _player;

	public GameObject bulletPrefab;

	public GameObject bulletPrefabRed;

	public GameObject bulletPrefabFor252;

	public GameObject _bulletSpawnPoint;

	private GameObject _inAppGameObject;

	public StoreKitEventListener _listener;

	public InGameGUI inGameGUI;

	private Dictionary<string, Action<string>> _actionsForPurchasedItems = new Dictionary<string, Action<string>>();

	public bool _isInappWinOpen;

	private WeaponManager ___weaponManager;

	private SaltedInt _countKillsCommandBlue = new SaltedInt(180068360);

	private SaltedInt _countKillsCommandRed = new SaltedInt(180068361);

	private bool canReceiveSwipes = true;

	public float slideMagnitudeX;

	public float slideMagnitudeY;

	public AudioClip ChangeWeaponClip;

	public AudioClip ChangeGrenadeClip;

	public AudioClip WeaponBonusClip;

	public PhotonView photonView;

	public AudioClip clickShop;

	public List<MessageChat> messages = new List<MessageChat>();

	public bool isSurvival;

	public string myTableId;

	private const string keyKilledPlayerCharactersCount = "KilledPlayerCharactersCount";

	private int oldKilledPlayerCharactersCount;

	public Material _bodyMaterial;

	public Material _mechMaterial;

	public Material _bearMaterial;

	public Material curMainSelect;

	public GameObject jetPackPoint;

	public GameObject wingsPoint;

	public GameObject wingsPointBear;

	public Animation wingsAnimation;

	public Animation wingsBearAnimation;

	private bool isPlayerFlying;

	public ParticleSystem[] jetPackParticle;

	public GameObject jetPackSound;

	public AudioSource wingsSound;

	private bool jetpackEnabled;

	private Weapon deathEscapeHandsWeapon;

	public string turretGadgetPrefabName;

	private int indexWeapon;

	private bool shouldSetMaxAmmoWeapon;

	private bool _changingWeapon;

	private string _sendingNameWeapon;

	private string _sendingAlternativeNameWeapon;

	private string _sendingSkinId;

	private IDisposable _backSubscription;

	private bool BonusEffectForArmorWorksInThisMatch;

	private bool ArmorBonusGiven;

	private bool isDaterRegim;

	private static KeyValuePair<int, string> _countdownMemo = new KeyValuePair<int, string>(0, "0:00");

	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();

	private bool pausedRating;

	public float _currentReloadAnimationSpeed = 1f;

	private int countHouseKeeperEvent;

	private bool isJumpPresedOld;

	private int countFixedUpdateForResetLabel;

	private float _chanceToIgnoreHeadshot;

	private float _protectionShieldValue = 1f;

	private bool isShieldActivated;

	private bool roomTierInitialized;

	private int roomTier;

	private bool _escapePressed;

	private float oldAlphaImmortality = -1f;

	private bool isSetVisible;

	private float _timeOfSlowdown;

	private bool isDetectCh;

	private int sendRateOld = -1;

	private Material oldWeaponHandMaterial;

	private Material[] oldWeaponMaterials;

	private SaltedInt numberOfGrenadesOnStart = new SaltedInt(45853678);

	private SaltedInt numberOfGrenades = new SaltedInt(29076718);

	private GameObject myPetValue;

	private List<ActivePlayerEffect> playerEffects = new List<ActivePlayerEffect>(3);

	private GameObject burningEffect;

	private GameObject charmEffect;

	private GameObject bleedingEffect;

	private List<GadgetEffectParams> activatedGadgetEffects = new List<GadgetEffectParams>(3);

	private Vector3[] timePositions = new Vector3[10];

	private Quaternion[] timeRotations = new Quaternion[10];

	private Quaternion[] timeGunRotations = new Quaternion[10];

	private bool firstPositionsReached;

	private int currentTimeIndex;

	private float nextTimeAdd;

	private float disablerRadius = 35f;

	private float mushroomRadius = 6f;

	private float mushroomBurnTime = 0.5f;

	private int mushroomBurnCount = 6;

	private float mushroomBurnDamage = 0.04f;

	public AudioClip mushroomActivationSound;

	public AudioClip mushroomShotSound;

	private float drumSupportRadius = 10f;

	private bool drumActive;

	private float drumDamageMultiplier;

	public AudioClip timeWatchSound;

	public AudioClip medkitSound;

	public AudioClip reflectorOnSound;

	public AudioClip reflectorOffSound;

	public AudioClip resurrectionSound;

	public GameObject reflectorPulseSound;

	public AudioClip dragonWhistleSound;

	public AudioClip disablerActiveSound;

	public AudioClip disablerEffectSound;

	public AudioClip drumActiveSound;

	public GameObject drumLoopSound;

	public AudioClip petBoosterActiveSound;

	public GameObject reflectorParticles;

	public GameObject reflectorParticlesDeactivation;

	public GameObject resurrectionEffect;

	public GameObject timeTravelEffect;

	public bool wasResurrected;

	public bool deadInCollider;

	public Vector3 resurrectionPosition;

	public PlayerMechBody mechBodyScript;

	public PlayerMechBody demonBodyScript;

	[HideInInspector]
	public PlayerMechBody currentMech;

	[HideInInspector]
	public Gadget daterLikeGadget = new DaterLikeGadget();

	public bool gadgetsDisabled;

	public Action GadgetsOnMechKill;

	public Action GadgetsOnPetKill;

	public bool gadgetWasPreused;

	private bool _isTimeJump;

	public float timeBuyHealth = -10000f;

	private SaltedFloatEps _curHealthSalt = new SaltedFloatEps();

	public float synhHealth = -10000000f;

	public double synchTimeHealth;

	public bool isSuicided;

	public bool killedInMatch;

	private float damageBuff = 1f;

	private float protectionBuff = 1f;

	private bool getLocalHurt;

	private bool timeSettedAfterRegenerationSwitchedOn;

	public bool isFallDown;

	private bool isRaiderMyPoint;

	private int countMySpotEvent;

	public Vector3 pointAutoAim;

	private float timerUpdatePointAutoAi = -1f;

	private Ray rayAutoAim;

	private List<PoisonTarget> poisonTargets = new List<PoisonTarget>();

	private bool _isShootingVal;

	public bool isShootingLoop;

	public float chargeValue;

	private float lastChargeValue;

	private const string startShootAnimName = "Shoot_start";

	private const string endShootAnimName = "Shoot_end";

	private CancellationTokenSource ctsShootLoop = new CancellationTokenSource();

	private int _countShootInBurst;

	private float _timerDelayInShootingBurst = -1f;

	private int ammoInClipBeforeCharge;

	private int lastChargeWeaponIndex;

	private float nextChargeConsumeTime = -1f;

	private bool firstChargePlay;

	private bool fullyCharged;

	private float lastShotTime;

	private float nextFrostHitTime;

	private float timeGrenadePress;

	public bool isGrenadePress;

	private const float slowdownCoefConst = 0.75f;

	private bool playChargeLoopAnim;

	private FreezerRay[] freezeRays = new FreezerRay[2];

	public bool isPlacemarker
	{
		get
		{
			return _isPlacemarker;
		}
		set
		{
			_isPlacemarker = value;
			placemarkerMark.SetActive(value);
		}
	}

	public Animation mechGunAnimation
	{
		get
		{
			if (currentMech != null)
			{
				return currentMech.gunAnimation;
			}
			return null;
		}
	}

	public int CurrentWeaponBeforeGrenade
	{
		get
		{
			return currentWeaponBeforeGrenade;
		}
		set
		{
			currentWeaponBeforeGrenade = value;
		}
	}

	public float koofDamageWeaponFromPotoins
	{
		get
		{
			return _koofDamageWeaponFromPotoins;
		}
		set
		{
			_koofDamageWeaponFromPotoins = value;
		}
	}

	private float maxTimerRegenerationArmor
	{
		get
		{
			return EffectsController.RegeneratingArmorTime;
		}
	}

	public float[] byCatDamageModifs
	{
		get
		{
			return _byCatDamageModifs;
		}
	}

	public int myCommand
	{
		get
		{
			return _myCommand;
		}
		set
		{
			_myCommand = value;
			UpdateNickLabelColor();
		}
	}

	public int countKills
	{
		get
		{
			return _killCount.Value;
		}
		set
		{
			_killCount = new SaltedInt(_prng.Next(), value);
		}
	}

	public bool isImmortality
	{
		get
		{
			return _isImmortalyVal;
		}
		set
		{
			if (isMine)
			{
				bool isImmortalyVal = _isImmortalyVal;
				_isImmortalyVal = value;
				if (isImmortalyVal != _isImmortalyVal && this.OnMyImmortalyChanged != null)
				{
					this.OnMyImmortalyChanged(value);
				}
			}
			else
			{
				_isImmortalyVal = value;
			}
		}
	}

	public KillerInfo killerInfo
	{
		get
		{
			return _killerInfo;
		}
	}

	internal static bool NeedApply { get; set; }

	internal static bool AnotherNeedApply { get; set; }

	private float maxBaseArmor
	{
		get
		{
			return 9f + EffectsController.ArmorBonus;
		}
	}

	private float CurrentBaseArmor
	{
		get
		{
			return _curBaseArmor;
		}
		set
		{
			_curBaseArmor = value;
		}
	}

	public bool isInappWinOpen
	{
		get
		{
			return _isInappWinOpen;
		}
		set
		{
			_isInappWinOpen = value;
			ShopNGUIController.GuiActive = value;
			if (!(myCurrentWeaponSounds != null))
			{
				return;
			}
			if (PauseGUIController.Instance != null && PauseGUIController.Instance.IsPaused)
			{
				myCurrentWeaponSounds.animationObject.SetActive(false);
			}
			else
			{
				myCurrentWeaponSounds.animationObject.SetActive(!value);
			}
			if (myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
			{
				if (value)
				{
					myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
				}
				else
				{
					myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public WeaponManager _weaponManager
	{
		get
		{
			return ___weaponManager;
		}
		set
		{
			___weaponManager = value;
		}
	}

	public int countKillsCommandBlue
	{
		get
		{
			return _countKillsCommandBlue.Value;
		}
		set
		{
			_countKillsCommandBlue.Value = value;
		}
	}

	public int countKillsCommandRed
	{
		get
		{
			return _countKillsCommandRed.Value;
		}
		set
		{
			_countKillsCommandRed.Value = value;
		}
	}

	private Material mainDamageMaterial
	{
		get
		{
			if (isMechActive)
			{
				curMainSelect = _mechMaterial;
				return _mechMaterial;
			}
			if (isBearActive)
			{
				curMainSelect = _bearMaterial;
				return _bearMaterial;
			}
			curMainSelect = _bodyMaterial;
			return _bodyMaterial;
		}
	}

	public bool IsPlayerFlying
	{
		get
		{
			return isPlayerFlying;
		}
	}

	public bool isNeedTakePremiumAccountRewards { get; private set; }

	public int GrenadeCount
	{
		get
		{
			return numberOfGrenades.Value;
		}
		set
		{
			numberOfGrenades.Value = value;
		}
	}

	private float WearedCurrentArmor
	{
		get
		{
			return CurrentBodyArmor + CurrentHatArmor;
		}
	}

	private float CurrentBodyArmor
	{
		get
		{
			SaltedFloat value;
			if (Wear.curArmor.TryGetValue(Storager.getString(Defs.ArmorNewEquppedSN) ?? "", out value))
			{
				return value.value;
			}
			return 0f;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.ArmorNewEquppedSN) ?? ""))
			{
				Wear.curArmor[Storager.getString(Defs.ArmorNewEquppedSN) ?? ""].value = value;
			}
		}
	}

	private float CurrentHatArmor
	{
		get
		{
			SaltedFloat value;
			if (Wear.curArmor.TryGetValue(Storager.getString(Defs.HatEquppedSN) ?? "", out value))
			{
				return value.value;
			}
			return 0f;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.HatEquppedSN) ?? ""))
			{
				Wear.curArmor[Storager.getString(Defs.HatEquppedSN) ?? ""].value = value;
			}
		}
	}

	public static int _ShootRaycastLayerMask
	{
		get
		{
			return -2053 & ~(1 << LayerMask.NameToLayer("DamageCollider")) & ~(1 << LayerMask.NameToLayer("TransparentFX")) & ~(1 << LayerMask.NameToLayer("IgnoreRocketsAndBullets"));
		}
	}

	public PetEngine myPetEngine { get; private set; }

	private GameObject myPet
	{
		get
		{
			return myPetValue;
		}
		set
		{
			myPetValue = value;
			myPetEngine = ((myPetValue != null) ? myPet.GetComponent<PetEngine>() : null);
		}
	}

	public WeaponSounds mechWeaponSounds
	{
		get
		{
			if (currentMech != null)
			{
				return currentMech.weapon;
			}
			return null;
		}
	}

	public bool canUseGadgets
	{
		get
		{
			if (showGadgetsPanel && Defs.isGrenateFireEnable && !gadgetsDisabled && !isMechActive)
			{
				return !isKilled;
			}
			return false;
		}
	}

	public bool showGadgetsPanel
	{
		get
		{
			if (gadgetsPanelEnabled && !Defs.isZooming && !Defs.isTurretWeapon && InGameGUI.sharedInGameGUI != null && !InGameGUI.sharedInGameGUI.isTurretInterfaceActive)
			{
				if (GameConnect.isMiniGame)
				{
					return MiniGamesController.Instance.isGo;
				}
				return true;
			}
			return false;
		}
	}

	public bool gadgetsPanelEnabled
	{
		get
		{
			if (!GameConnect.isHunger && !GameConnect.isCOOP && !GameConnect.isDaterRegim && GameConnect.gameMode != GameConnect.GameMode.DeathEscape && !GameConnect.isSpleef && !GameConnect.isSpeedrun && WeaponManager.sharedManager._currentFilterMap != 1 && WeaponManager.sharedManager._currentFilterMap != 2 && GameConnect.gameMode != GameConnect.GameMode.Arena)
			{
				if (!TrainingController.TrainingCompleted)
				{
					return TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None;
				}
				return true;
			}
			return false;
		}
	}

	public bool wasTimeJump
	{
		get
		{
			if (_isTimeJump)
			{
				_isTimeJump = false;
				return true;
			}
			return false;
		}
	}

	public static int MaxPlayerGUIHealth
	{
		get
		{
			return 9;
		}
	}

	private float _curHealth
	{
		get
		{
			return _curHealthSalt.value;
		}
		set
		{
			_curHealthSalt.value = value;
		}
	}

	public float CurHealth
	{
		get
		{
			return _curHealth;
		}
		set
		{
			float num = _curHealth - value;
			_curHealth -= num;
		}
	}

	public float MaxHealth
	{
		get
		{
			if (!GameConnect.isHunger && !GameConnect.isSurvival && !GameConnect.isCOOP)
			{
				return ExperienceController.HealthByLevel[(Defs.isMulti && myNetworkStartTable != null) ? myNetworkStartTable.myRanks : ExperienceController.sharedController.currentLevel];
			}
			return ExperienceController.HealthByLevel[1];
		}
	}

	public float curArmor
	{
		get
		{
			return CurrentBaseArmor + CurrentBodyArmor + CurrentHatArmor;
		}
		set
		{
			float num = curArmor - value;
			if (num >= 0f)
			{
				if (CurrentHatArmor >= num)
				{
					CurrentHatArmor -= num;
					return;
				}
				num -= CurrentHatArmor;
				CurrentHatArmor = 0f;
				if (CurrentBodyArmor >= num)
				{
					CurrentBodyArmor -= num;
					return;
				}
				num -= CurrentBodyArmor;
				CurrentBodyArmor = 0f;
				CurrentBaseArmor -= num;
			}
			else if (num < 0f)
			{
				num *= -1f;
				num = ((!(WearedMaxArmor > 0f)) ? 1f : ((!(WearedMaxArmor > 5f)) ? (WearedMaxArmor - WearedCurrentArmor) : Mathf.Min(WearedMaxArmor - WearedCurrentArmor, WearedMaxArmor * 0.5f)));
				AddArmor(num);
			}
		}
	}

	public float MaxArmor
	{
		get
		{
			return maxBaseArmor + WearedMaxArmor;
		}
	}

	public float WearedMaxArmor
	{
		get
		{
			float num = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN), TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1)));
			float num2 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN), TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1)));
			return num + num2;
		}
	}

	private bool isNeedShowRespawnWindow
	{
		get
		{
			if (!isHunger && !Defs.isRegimVidosDebug && !_killerInfo.isSuicide && Defs.isMulti && !GameConnect.isCOOP)
			{
				return !wasResurrected;
			}
			return false;
		}
	}

	private bool isShooting
	{
		get
		{
			return _isShootingVal;
		}
		set
		{
			if (_isShootingVal != value)
			{
				_isShootingVal = value;
				if (((isMulti && isMine) || !isMulti) && Player_move_c.OnMyShootingStateSchanged != null)
				{
					Player_move_c.OnMyShootingStateSchanged(_isShootingVal);
				}
			}
		}
	}

	public static event Action StopBlinkShop;

	public event Action<bool> OnMyImmortalyChanged;

	public static event Action OnMyPlayerMoveCCreated;

	public static event Action<float> OnMyPlayerMoveCDestroyed;

	public event OnMessagesUpdate messageDelegate;

	public event EventHandler<EventArgs> WeaponChanged;

	public event Action OnMyKillMechInDemon;

	public event Action OnMyPlayerResurected;

	public static event Action<bool> OnMyShootingStateSchanged;

	private void SaveKillRate()
	{
		try
		{
			if (!isMulti || GameConnect.isHunger || GameConnect.isCOOP || (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None) || GameConnect.isSurvival)
			{
				return;
			}
			Action<Dictionary<string, int>, Dictionary<string, Dictionary<int, int>>> obj = delegate(Dictionary<string, int> battleDict, Dictionary<string, Dictionary<int, int>> dictToDisk)
			{
				foreach (KeyValuePair<string, int> item in battleDict)
				{
					if (dictToDisk.ContainsKey(item.Key))
					{
						Dictionary<int, int> dictionary = dictToDisk[item.Key];
						if (dictionary.ContainsKey(tierForKilledRate))
						{
							dictionary[tierForKilledRate] += item.Value;
						}
						else
						{
							dictionary.Add(tierForKilledRate, item.Value);
						}
					}
					else
					{
						dictToDisk.Add(item.Key, new Dictionary<int, int> { { tierForKilledRate, item.Value } });
					}
				}
			};
			obj(weKillForKillRate, KillRateStatisticsManager.WeKillOld);
			obj(weWereKilledForKillRate, KillRateStatisticsManager.WeWereKilledOld);
			Dictionary<string, object> obj2 = new Dictionary<string, object>
			{
				{
					"version",
					GlobalGameController.AppVersion
				},
				{
					"wekill",
					KillRateStatisticsManager.WeKillOld
				},
				{
					"wewerekilled",
					KillRateStatisticsManager.WeWereKilledOld
				}
			};
			Storager.setString("KillRateKeyStatistics", Json.Serialize(obj2));
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in save kill rate statistics: " + ex);
		}
	}

	private void AddWeKillStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			UnityEngine.Debug.LogError("AddWeKillStatisctics string.IsNullOrEmpty (weaponName)");
		}
		else if (weKillForKillRate.ContainsKey(weaponName))
		{
			weKillForKillRate[weaponName]++;
		}
		else
		{
			weKillForKillRate.Add(weaponName, 1);
		}
	}

	private void AddWeWereKilledStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			UnityEngine.Debug.LogError("AddWeWereKilledStatisctics string.IsNullOrEmpty (weaponName)");
		}
		else if (weWereKilledForKillRate.ContainsKey(weaponName))
		{
			weWereKilledForKillRate[weaponName]++;
		}
		else
		{
			weWereKilledForKillRate.Add(weaponName, 1);
		}
	}

	private void UpdateNickLabelColor()
	{
		if (GameConnect.gameMode == GameConnect.GameMode.CapturePoints || GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable == null || WeaponManager.sharedManager.myNetworkStartTable.myCommand == 0)
			{
				if (_nickColorInd != 0)
				{
					nickLabel.color = Color.white;
					_nickColorInd = 0;
				}
			}
			else if (WeaponManager.sharedManager.myNetworkStartTable.myCommand == myCommand)
			{
				if (_nickColorInd != 1)
				{
					nickLabel.color = Color.blue;
					_nickColorInd = 1;
				}
			}
			else if (_nickColorInd != 2)
			{
				nickLabel.color = Color.red;
				_nickColorInd = 2;
			}
		}
		else if (GameConnect.isDaterRegim)
		{
			if (_nickColorInd != 0)
			{
				nickLabel.color = Color.white;
				_nickColorInd = 0;
			}
		}
		else if (GameConnect.isCOOP)
		{
			if (_nickColorInd != 1)
			{
				nickLabel.color = Color.blue;
				_nickColorInd = 1;
			}
		}
		else if (_nickColorInd != 2)
		{
			nickLabel.color = Color.red;
			_nickColorInd = 2;
		}
	}

	public void IndicateDamage()
	{
		isDeadFrame = true;
		Invoke("setisDeadFrameFalse", 1f);
	}

	private void AddArmor(float dt)
	{
		if (GameConnect.isHunger || GameConnect.isSurvival || GameConnect.isCOOP)
		{
			if (!Mathf.Approximately(0f, dt))
			{
				CurrentBaseArmor = 9f;
			}
		}
		else if (WearedMaxArmor > 0f)
		{
			float num = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN), TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1))) - CurrentBodyArmor;
			if (num < 0f)
			{
				num = 0f;
			}
			if (dt <= num)
			{
				CurrentBodyArmor += dt;
				return;
			}
			CurrentBodyArmor += num;
			dt -= num;
			float num2 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN), TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1))) - CurrentHatArmor;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			CurrentHatArmor += Mathf.Min(num2, dt);
		}
		else
		{
			float num3 = maxBaseArmor - CurrentBaseArmor;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (dt <= num3)
			{
				CurrentBaseArmor += dt;
			}
			else
			{
				CurrentBaseArmor += num3;
			}
		}
	}

	private void Awake()
	{
		isSurvival = GameConnect.isSurvival;
		isMulti = Defs.isMulti;
		isInet = Defs.isInet;
		isCompany = GameConnect.isCompany;
		isCOOP = GameConnect.isCOOP;
		isHunger = GameConnect.isHunger;
		myAudioSource = GetComponent<AudioSource>();
		myCamera.fieldOfView = stdFov;
		myDamageable = mySkinName.GetComponent<PlayerDamageable>();
		skinNamePixelView = mySkinName.GetComponent<PixelView>();
	}

	public void ActivateJetpackGadget(bool isEnabled)
	{
		if (!Defs.isMulti || isMine)
		{
			SetJetpackEnabled(isEnabled);
		}
		else
		{
			SetJetpackEnabledRPC(isEnabled);
		}
	}

	public void SetJetpackEnabled(bool _isEnabled)
	{
		Defs.isJetpackEnabled = _isEnabled;
		if (Defs.isSoundFX && _isEnabled)
		{
			AudioSource component = GetComponent<AudioSource>();
			if (component != null)
			{
				component.PlayOneShot(jetpackActivSound);
			}
		}
		if (GameConnect.isDaterRegim && Defs.isMulti && Defs.isInet && photonView != null)
		{
			photonView.RPC("SetJetpackEnabledRPC", PhotonTargets.Others, _isEnabled);
		}
	}

	
	[PunRPC]
	public void SetJetpackEnabledRPC(bool _isEnabled)
	{
		jetpackEnabled = _isEnabled;
		if (Defs.isSoundFX && _isEnabled)
		{
			GetComponent<AudioSource>().PlayOneShot(jetpackActivSound);
		}
		if (GameConnect.isDaterRegim)
		{
			wingsPoint.SetActive(_isEnabled);
			wingsPointBear.SetActive(_isEnabled);
		}
		else
		{
			jetPackPoint.SetActive(_isEnabled);
			if (isMechActive && currentMech != null)
			{
				currentMech.jetpackObject.SetActive(_isEnabled);
			}
		}
		if (!_isEnabled)
		{
			for (int i = 0; i < jetPackParticle.Length; i++)
			{
				jetPackParticle[i].enableEmission = _isEnabled;
			}
		}
	}

	public void SetJetpackParticleEnabled(bool _isEnabled)
	{
		if (_isEnabled)
		{
			isPlayerFlying = true;
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !GameConnect.isDaterRegim)
			{
				jetPackSound.SetActive(!IsGadgetEffectActive(GadgetEffect.demon));
			}
		}
		else
		{
			isPlayerFlying = false;
			if (!GameConnect.isDaterRegim)
			{
				jetPackSound.SetActive(false);
			}
		}
		if (Defs.isMulti && Defs.isInet)
		{
			photonView.RPC("SetJetpackParticleEnabledRPC", PhotonTargets.Others, _isEnabled);
		}
	}

	
	[PunRPC]
	public void SetJetpackParticleEnabledRPC(bool _isEnabled)
	{
		if (_isEnabled)
		{
			isPlayerFlying = true;
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !GameConnect.isDaterRegim)
			{
				jetPackSound.SetActive(!IsGadgetEffectActive(GadgetEffect.demon));
			}
		}
		else
		{
			isPlayerFlying = false;
			if (!GameConnect.isDaterRegim)
			{
				jetPackSound.SetActive(false);
			}
		}
		for (int i = 0; i < jetPackParticle.Length; i++)
		{
			jetPackParticle[i].enableEmission = _isEnabled;
		}
	}

	[PunRPC]
	
	private void SendChatMessageWithIcon(string text, string _iconName)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayerMoveC == null) && isInet)
		{
			_weaponManager.myPlayerMoveC.AddMessage(text, Time.time, mySkinName.photonView.viewID, myCommand, _iconName);
		}
	}

	private void SendChatMessage(string text)
	{
		SendChatMessageWithIcon(text, string.Empty);
	}

	public void SendChat(string text, bool clanMode, string iconName)
	{
		if (text.Length <= 50)
		{
			text = (text.Equals("-=ATTACK!=-") ? LocalizationStore.Get("Key_1086") : (text.Equals("-=HELP!=-") ? LocalizationStore.Get("Key_1087") : (text.Equals("-=OK!=-") ? LocalizationStore.Get("Key_1088") : ((!text.Equals("-=NO!=-")) ? FilterBadWorld.FilterString(text) : LocalizationStore.Get("Key_1089")))));
			if ((!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(iconName)) && isInet)
			{
				photonView.RPC("SendChatMessageWithIcon", PhotonTargets.All, "< " + _weaponManager.myNetworkStartTable.NamePlayer + " > " + text, iconName);
			}
		}
	}

	public void SendDaterChat(string nick1, string text, string nick2)
	{
		if (text != "" && isInet)
		{
			photonView.RPC("SendDaterChatRPC", PhotonTargets.All, nick1, text, nick2);
		}
	}

	
	[PunRPC]
	public void SendDaterChatRPC(string nick1, string text, string nick2)
	{
		text = "< " + nick1 + "[-] > " + LocalizationStore.Get(text) + " < " + nick2 + "[-] >";
		SendChatMessage(text);
	}

	public void AddMessage(string text, float time, int ID, int _command, string iconName)
	{
		MessageChat item = default(MessageChat);
		item.text = text;
		item.iconName = iconName;
		item.time = time;
		item.ID = ID;
		item.command = _command;
		item.clanLogo = null;
		messages.Add(item);
		if (messages.Count > 30)
		{
			messages.RemoveAt(0);
		}
		OnMessagesUpdate onMessagesUpdate = this.messageDelegate;
		if (onMessagesUpdate != null)
		{
			onMessagesUpdate();
		}
	}

	public void WalkAnimation()
	{
		/*if (_singleOrMultiMine() || GameConnect.isDeathEscape || (GameConnect.isDaterRegim && isBearActive))
		{
			if (!GameConnect.isDaterRegim && isMechActive && !mechGunAnimation.IsPlaying("Shoot") && !mechGunAnimation.IsPlaying("Shoot1") && !mechGunAnimation.IsPlaying("Shoot2"))
			{
				mechGunAnimation.CrossFade("Walk");
			}
			if ((!___weaponManager.currentWeaponSounds.isCharging || !(chargeValue > 0f)) && (bool)_weaponManager && (bool)_weaponManager.currentWeaponSounds && _weaponManager.currentWeaponSounds.animationObject != null && (!(___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Peg") != null) || !___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Peg")))
			{
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Walk");
			}
		}*/
	}

	public void IdleAnimation()
	{
		if (_singleOrMultiMine() || (GameConnect.isDaterRegim && isBearActive))
		{
			if (!GameConnect.isDaterRegim && isMechActive && !mechGunAnimation.IsPlaying("Shoot") && !mechGunAnimation.IsPlaying("Shoot1") && !mechGunAnimation.IsPlaying("Shoot2"))
			{
				mechGunAnimation.CrossFade("Idle");
			}
			if ((!___weaponManager.currentWeaponSounds.isCharging || !(chargeValue > 0f)) && (bool)___weaponManager && (bool)___weaponManager.currentWeaponSounds && ___weaponManager.currentWeaponSounds.animationObject != null && (!(___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Peg") != null) || !___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Peg")))
			{
				___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Idle");
			}
		}
	}

	public void ZoomPress()
	{
		if (WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			showZoomHint = false;
			HintController.instance.HideHintByName("use_zoom");
		}
		isZooming = !isZooming;
		if (isZooming)
		{
			if (Defs.isSoundFX && _weaponManager.currentWeaponSounds.zoomIn != null)
			{
				GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.zoomIn);
			}
			myCamera.fieldOfView = _weaponManager.currentWeaponSounds.fieldOfViewZomm;
			gunCamera.gameObject.SetActive(false);
			inGameGUI.SetScopeForWeapon(_weaponManager.currentWeaponSounds.scopeNum.ToString());
			myTransform.localPosition = new Vector3(myTransform.localPosition.x, myTransform.localPosition.y, myTransform.localPosition.z);
		}
		else
		{
			if (Defs.isSoundFX && _weaponManager.currentWeaponSounds.zoomOut != null)
			{
				GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.zoomOut);
			}
			myCamera.fieldOfView = stdFov;
			gunCamera.fieldOfView = 90f;
			gunCamera.gameObject.SetActive(true);
			if (inGameGUI != null)
			{
				inGameGUI.ResetScope();
			}
		}
		if (isMulti && isInet)
		{
			photonView.RPC("SynhIsZoming", PhotonTargets.All, isZooming);
		}
	}

	
	[PunRPC]
	private void SynhIsZoming(bool _isZoomming)
	{
		isZooming = _isZoomming;
	}

	public void hideGUI()
	{
		showGUI = false;
	}

	public void setMyTamble(GameObject _myTable)
	{
		if (myTable == null || _myTable == null)
		{
			return;
		}
		NetworkStartTable component = myTable.GetComponent<NetworkStartTable>();
		if (component == null)
		{
			return;
		}
		component.myPlayerMoveC = this;
		myTable = _myTable;
		myNetworkStartTable = myTable.GetComponent<NetworkStartTable>();
		if (myNetworkStartTable == null)
		{
			return;
		}
		CurHealth = MaxHealth;
		myCommand = myNetworkStartTable.myCommand;
		if (Initializer.redPlayers.Contains(this) && myCommand == 1)
		{
			Initializer.redPlayers.Remove(this);
		}
		if (Initializer.bluePlayers.Contains(this) && myCommand == 2)
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (myCommand == 1 && !Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Add(this);
		}
		if (myCommand == 2 && !Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Add(this);
		}
		_skin = myNetworkStartTable.mySkin;
		SetTextureForBodyPlayer(_skin);
		if (isMine)
		{
			if (ABTestController.useBuffSystem)
			{
				BuffSystem.instance.CheckForPlayerBuff();
			}
			else if (KillRateCheck.instance.buffEnabled)
			{
				SetupBuffParameters(KillRateCheck.instance.damageBuff, KillRateCheck.instance.healthBuff);
			}
			else
			{
				SetupBuffParameters(1f, 1f);
			}
		}
	}

	public void SetupBuffParameters(float damage, float protection)
	{
		bool num = damageBuff != damage || protectionBuff != protection;
		SetBuffParameters(damage, protection);
		if (num && Defs.isMulti && Defs.isInet)
		{
			photonView.RPC("SendBuffParameters", PhotonTargets.Others, damageBuff, protectionBuff);
		}
	}

	private void SetBuffParameters(float damage, float protection)
	{
		damageBuff = Mathf.Clamp(damage, 0.01f, 10f);
		protectionBuff = Mathf.Clamp(protection, 0.01f, 10f);
		UnityEngine.Debug.Log(string.Format("<color=green>{0}Damage: {1}, Protection: {2}</color>", new object[3]
		{
			isMine ? "(you) " : ("(" + mySkinName.NickName + ") "),
			damageBuff,
			protectionBuff
		}));
	}

	[PunRPC]
	private void SendBuffParameters(float damage, float protection)
	{
		if (!isMine)
		{
			SetBuffParameters(damage, protection);
		}
	}

	public void AddWeapon(GameObject weaponPrefab)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			int num = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == weaponPrefab.GetComponent<WeaponSounds>().categoryNabor);
			if (num >= 0)
			{
				ChangeWeapon(num, false);
			}
			return;
		}
		WeaponSounds component = weaponPrefab.GetComponent<WeaponSounds>();
		int result;
		int result2;
		if ((component != null && WeaponManager.sharedManager != null && !component.IsAvalibleFromFilter(WeaponManager.sharedManager.CurrentFilterMap)) || (GameConnect.isHunger && component != null && int.TryParse(component.nameNoClone().Substring("Weapon".Length), out result) && result != 9 && !ChestController.weaponForHungerGames.Contains(result)) || ((GameConnect.isSurvival || GameConnect.isCOOP) && component != null && int.TryParse(component.nameNoClone().Substring("Weapon".Length), out result2) && result2 != 9 && result2 != 1 && ((GameConnect.isSurvival && !ZombieCreator.WeaponsAddedInWaves.SelectMany((List<string> weapons) => weapons).Contains(component.nameNoClone())) || (GameConnect.isCOOP && !ChestController.weaponForHungerGames.Contains(result2)))))
		{
			return;
		}
		int score;
		if (_weaponManager.AddWeapon(weaponPrefab, out score))
		{
			if (indexWeapon < 1000)
			{
				ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
			}
		}
		else if (ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", "")).Tag))
		{
			GlobalGameController.Score += score;
			if (Defs.isSoundFX)
			{
				if (WeaponBonusClip != null)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(WeaponBonusClip);
				}
				else
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
				}
			}
		}
		else
		{
			if (indexWeapon >= 1000)
			{
				return;
			}
			foreach (Weapon playerWeapon in _weaponManager.playerWeapons)
			{
				if (playerWeapon.weaponPrefab == weaponPrefab)
				{
					ChangeWeapon(_weaponManager.playerWeapons.IndexOf(playerWeapon), false);
					break;
				}
			}
		}
	}

	
	[PunRPC]
	public void StartFlashRPC()
	{
		StartCoroutine(Flash(myPlayerTransform.gameObject));
	}

	public void SendStartFlashMine()
	{
		if (isInet)
		{
			photonView.RPC("StartFlashRPC", PhotonTargets.All);
		}
	}

	public void StartFlash(GameObject _obj)
	{
		StartCoroutine(Flash(_obj));
	}

	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		int childCount = obj.transform.childCount;
		Transform transform = obj.transform;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(null == child))
			{
				SetLayerRecursively(child.gameObject, newLayer);
			}
		}
	}

	public static void PerformActionRecurs(GameObject obj, Action<Transform> act)
	{
		if (act == null || null == obj)
		{
			return;
		}
		act(obj.transform);
		int childCount = obj.transform.childCount;
		Transform transform = obj.transform;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(null == child))
			{
				PerformActionRecurs(child.gameObject, act);
			}
		}
	}

	private Weapon GetWeaponByIndex(int index)
	{
		if (GameConnect.isDeathEscape || GameConnect.isSpeedrun)
		{
			if (deathEscapeHandsWeapon == null)
			{
				WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/Weapon298");
				deathEscapeHandsWeapon = new Weapon();
				deathEscapeHandsWeapon.weaponPrefab = weaponSounds.gameObject;
				deathEscapeHandsWeapon.currentAmmoInBackpack = weaponSounds.InitialAmmoWithEffectsApplied;
				deathEscapeHandsWeapon.currentAmmoInClip = weaponSounds.ammoInClip;
			}
			return deathEscapeHandsWeapon;
		}
		return _weaponManager.playerWeapons[index] as Weapon;
	}

	public void ChangeWeapon(int index, bool shouldSetMaxAmmo = true)
	{
		if (index == 1001)
		{
			currentWeaponBeforeTurret = WeaponManager.sharedManager.CurrentWeaponIndex;
		}
		indexWeapon = index;
		shouldSetMaxAmmoWeapon = shouldSetMaxAmmo;
		StopCoroutine("ChangeWeaponCorutine");
		StopCoroutine(BazookaShoot());
		StopCoroutine("BazookaShoot");
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine("ChangeWeaponCorutine");
		}
		if (GetComponent<AudioSource>() != null && !isMechActive)
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public void ResetWeaponChange()
	{
		_changingWeapon = false;
		deltaAngle = 0f;
	}

	private IEnumerator ChangeWeaponCorutine()
	{
		_changingWeapon = true;
		if (inGameGUI != null)
		{
			inGameGUI.StopAllCircularIndicators();
		}
		photonView.synchronization = ViewSynchronization.Off;
		if (!Defs.isTurretWeapon)
		{
			while (deltaAngle < 40f && !Defs.isTurretWeapon && !isMechActive)
			{
				deltaAngle += 300f * Time.deltaTime;
				yield return null;
			}
		}
		else
		{
			if (!isMechActive)
			{
				deltaAngle = 40f;
			}
			Defs.isTurretWeapon = false;
		}
		GameObject _weaponPrefab;
		string path;
		if (indexWeapon == 1000)
		{
			_weaponPrefab = Resources.Load("GadgetsContent/" + currentGrenadeGadget.GrenadeGadgetId) as GameObject;
			path = ResPath.Combine(Defs.GadgetContentFolder, currentGrenadeGadget.GrenadeGadgetId + Defs.InnerWeapons_Suffix);
		}
		else if (indexWeapon == 1001)
		{
			_weaponPrefab = turretPrefab;
			path = ResPath.Combine(Defs.GadgetContentFolder, _weaponPrefab.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		}
		else
		{
			_weaponPrefab = GetWeaponByIndex(indexWeapon).weaponPrefab;
			path = ResPath.Combine(Defs.InnerWeaponsFolder, _weaponPrefab.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		}
		LoadAsyncTool.ObjectRequest weaponRequest = LoadAsyncTool.Get(path);
		while (!weaponRequest.isDone)
		{
			yield return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(_weaponPrefab, Vector3.zero, Quaternion.identity);
		gameObject.GetComponent<WeaponSounds>().Initialize(weaponRequest.asset as GameObject);
		ChangeWeaponReal(_weaponPrefab, gameObject, indexWeapon, shouldSetMaxAmmoWeapon);
		if (indexWeapon != 1001 && !isMechActive)
		{
			while (deltaAngle > 0f)
			{
				deltaAngle -= 300f * Time.deltaTime;
				if (deltaAngle < 0f)
				{
					deltaAngle = -0.01f;
				}
				yield return null;
			}
		}
		if (indexWeapon == 1001)
		{
			deltaAngle = 0f;
		}
		photonView.synchronization = ViewSynchronization.Unreliable;
		_changingWeapon = false;
	}

	public void ChangeWeaponReal(int index, bool shouldSetMaxAmmo = true)
	{
		GameObject gameObject = null;
		GameObject gameObject2;
		string a;
		switch (index)
		{
		case 1000:
			gameObject2 = Resources.Load(Defs.GadgetContentFolder + "/" + currentGrenadeGadget.GrenadeGadgetId) as GameObject;
			a = Defs.GadgetContentFolder;
			break;
		case 1001:
			gameObject2 = turretPrefab;
			a = Defs.GadgetContentFolder;
			break;
		default:
			gameObject2 = GetWeaponByIndex(index).weaponPrefab;
			a = Defs.InnerWeaponsFolder;
			break;
		}
		GameObject pref = LoadAsyncTool.Get(ResPath.Combine(a, gameObject2.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix), true).asset as GameObject;
		gameObject = UnityEngine.Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity);
		gameObject.GetComponent<WeaponSounds>().Initialize(pref);
		ChangeWeaponReal(gameObject2, gameObject, index, shouldSetMaxAmmo);
	}

	public void ChangeWeaponReal(GameObject _weaponPrefab, GameObject nw, int index, bool shouldSetMaxAmmo = true)
	{
		if (inGameGUI != null)
		{
			inGameGUI.StopAllCircularIndicators();
		}
		EventHandler<EventArgs> weaponChanged = this.WeaponChanged;
		if (weaponChanged != null)
		{
			weaponChanged(this, EventArgs.Empty);
		}
		if (isZooming)
		{
			ZoomPress();
		}
		photonView = PhotonView.Get(this);
		Quaternion rotation = Quaternion.identity;
		if ((bool)_player)
		{
			rotation = _player.transform.rotation;
		}
		ShotUnPressed(true);
		if ((bool)_weaponManager.currentWeaponSounds)
		{
			rotation = _weaponManager.currentWeaponSounds.gameObject.transform.rotation;
			_weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
			UnityEngine.Object.Destroy(_weaponManager.currentWeaponSounds.gameObject);
			_weaponManager.currentWeaponSounds = null;
		}
		ResetShootingBurst();
		myCurrentWeapon = nw;
		myCurrentWeaponSounds = myCurrentWeapon.GetComponent<WeaponSounds>();
		if (!ShopNGUIController.GuiActive && myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		if (!isMechActive)
		{
			if (myCurrentWeaponSounds.isDoubleShot)
			{
				gunCamera.transform.localPosition = Vector3.zero;
			}
			else
			{
				gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
		}
		nw.transform.parent = base.gameObject.transform;
		nw.transform.rotation = rotation;
		myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
		if (isMechActive || GameConnect.isSpeedrun || GameConnect.isDeathEscape)
		{
			myCurrentWeapon.SetActive(false);
		}
		if (GameConnect.isDaterRegim)
		{
			SetWeaponVisible(!isBearActive);
		}
		if (myCurrentWeaponSounds != null && PhotonNetwork.room != null)
		{
			Statistics.Instance.IncrementWeaponPopularity(LocalizationStore.GetByDefault(myCurrentWeaponSounds.localizeWeaponKey), false);
			_weaponPopularityCacheIsDirty = true;
		}
		WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(_weaponPrefab.nameNoClone());
		if (skinForWeapon != null)
		{
			skinForWeapon.SetTo(myCurrentWeaponSounds.gameObject);
		}
		if (isMulti)
		{
			string sendingSkinId = ((skinForWeapon != null) ? skinForWeapon.Id : string.Empty);
			_sendingNameWeapon = _weaponPrefab.name;
			_sendingAlternativeNameWeapon = _weaponPrefab.GetComponent<WeaponSounds>().alternativeName;
			_sendingSkinId = sendingSkinId;
			if (isInet)
			{
				short result = 0;
				short result2 = 0;
				if (_sendingNameWeapon.Length > 6 && _sendingAlternativeNameWeapon.Length > 6 && short.TryParse(_sendingNameWeapon.Substring(6), out result) && short.TryParse(_sendingAlternativeNameWeapon.Substring(6), out result2))
				{
					photonView.RPC("SetWeaponRPCFromIndex", PhotonTargets.Others, result, result2, _sendingSkinId);
				}
				else
				{
					photonView.RPC("SetWeaponRPC", PhotonTargets.Others, _sendingNameWeapon, _sendingAlternativeNameWeapon, _sendingSkinId);
				}
			}
		}
		if (index == 1000)
		{
			WeaponSounds component = _weaponPrefab.GetComponent<WeaponSounds>();
			if (currentGrenadeGadget != null)
			{
				currentGrenadeGadget.CreateRocket(component);
			}
		}
		if (index == 1001)
		{
			Defs.isTurretWeapon = true;
			string text = ResPath.Combine(Defs.GadgetContentFolder, turretGadgetPrefabName);
			GameObject gameObject = ((!isMulti) ? UnityEngine.Object.Instantiate(Resources.Load(text) as GameObject, new Vector3(-10000f, -10000f, -10000f), base.transform.rotation) : (isInet ? PhotonNetwork.Instantiate(text, Vector3.down * -10000f, Quaternion.identity, 0) : null));
			if (gameObject != null)
			{
				gameObject.GetComponent<TurretController>();
				gameObject.GetComponent<Rigidbody>().useGravity = false;
				gameObject.GetComponent<Rigidbody>().isKinematic = true;
				if (Defs.isMulti)
				{
					bool isInet2 = Defs.isInet;
				}
			}
			currentTurret = gameObject;
		}
		if (!myCurrentWeaponSounds.isMelee)
		{
			foreach (Transform item in nw.transform)
			{
				if (item.gameObject.name.Equals("BulletSpawnPoint") && item.childCount > 0)
				{
					WeaponManager.SetGunFlashActive(item.GetChild(0).gameObject, false);
					break;
				}
			}
		}
		SetTextureForBodyPlayer(_skin);
		SetLayerRecursively(nw, 9);
		_weaponManager.currentWeaponSounds = myCurrentWeaponSounds;
		if (index < 1000)
		{
			_weaponManager.CurrentWeaponIndex = index;
			_weaponManager.SaveWeaponAsLastUsed(_weaponManager.CurrentWeaponIndex);
			if (inGameGUI != null)
			{
				if (_weaponManager.currentWeaponSounds.isMelee && !_weaponManager.currentWeaponSounds.isShotMelee && !isMechActive)
				{
					inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
				else
				{
					inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
			}
		}
		if (nw.transform.parent == null)
		{
			UnityEngine.Debug.LogWarning("nw.transform.parent == null");
		}
		else if (_weaponManager.currentWeaponSounds == null)
		{
			UnityEngine.Debug.LogWarning("_weaponManager.currentWeaponSounds == null");
		}
		else
		{
			nw.transform.position = nw.transform.parent.TransformPoint(_weaponManager.currentWeaponSounds.gunPosition);
		}
		TouchPadController rightJoystick = JoystickController.rightJoystick;
		if (index < 1000 && rightJoystick != null)
		{
			if (GetWeaponByIndex(index).currentAmmoInClip > 0 || (_weaponManager.currentWeaponSounds.isMelee && !_weaponManager.currentWeaponSounds.isShotMelee))
			{
				rightJoystick.HasAmmo();
				if (inGameGUI != null)
				{
					inGameGUI.BlinkNoAmmo(0);
				}
			}
			else if (GetWeaponByIndex(index).currentAmmoInBackpack <= 0)
			{
				rightJoystick.NoAmmo();
				if (inGameGUI != null)
				{
					inGameGUI.BlinkNoAmmo(1);
				}
			}
		}
		if (_weaponManager.currentWeaponSounds.animationObject != null)
		{
			if (_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
			{
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
			}
			if (!_weaponManager.currentWeaponSounds.isDoubleShot)
			{
				if (_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
				{
					_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
				}
			}
			else
			{
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
			}
		}
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			foreach (Transform item2 in _weaponManager.currentWeaponSounds.gameObject.transform)
			{
				if (item2.name.Equals("BulletSpawnPoint"))
				{
					_bulletSpawnPoint = item2.gameObject;
					break;
				}
			}
			GunFlash = _bulletSpawnPoint.transform.GetChild(0);
		}
		if (Defs.isSoundFX && !GameConnect.isDaterRegim && !isMechActive && index != 1000)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
		}
		if (!GameConnect.isDaterRegim && isInvisible)
		{
			SetInVisibleShaders(isInvisible);
		}
		if (inGameGUI != null)
		{
			if (isMechActive)
			{
				inGameGUI.SetCrosshair(mechWeaponSounds);
			}
			else
			{
				inGameGUI.SetCrosshair(_weaponManager.currentWeaponSounds);
			}
		}
		UpdateEffectsForCurrentWeapon(mySkinName.currentCape, mySkinName.currentMask, mySkinName.currentHat);
		if (myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showZoomHint)
		{
			Invoke("TrainingShowZoomHint", 3f);
		}
	}

	private void TrainingShowZoomHint()
	{
		if (myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showZoomHint)
		{
			HintController.instance.ShowHintByName("use_zoom");
		}
	}

	
	[PunRPC]
	private void SetWeaponRPCFromIndex(short _nameWeaponIndex, short _alternativeNameWeaponIndex, string skinId)
	{
		StartCoroutine(SetWeaponRPC(weaponKey + _nameWeaponIndex, weaponKey + _alternativeNameWeaponIndex, skinId));
	}

	
	[PunRPC]
	private IEnumerator SetWeaponRPC(string _nameWeapon, string _alternativeNameWeapon, string skinId)
	{
		isWeaponSet = true;
		string a = Defs.InnerWeaponsFolder;
		GameObject _weapon;
		if (!_nameWeapon.Contains("Weapon"))
		{
			if (_nameWeapon.Equals("Like"))
			{
				_weapon = likePrefab;
				a = Defs.InnerWeaponsFolder;
			}
			else if (_nameWeapon.Equals("Turret"))
			{
				_weapon = turretPrefab;
				a = Defs.GadgetContentFolder;
			}
			else
			{
				_weapon = Resources.Load("GadgetsContent/" + _nameWeapon) as GameObject;
				a = Defs.GadgetContentFolder;
			}
		}
		else
		{
			if (_nameWeapon != null && _alternativeNameWeapon != null && WeaponManager.Removed150615_PrefabNames.Contains(_nameWeapon))
			{
				_nameWeapon = _alternativeNameWeapon;
			}
			_weapon = Resources.Load("Weapons/" + _nameWeapon) as GameObject;
			if (_weapon != null)
			{
				WeaponSounds component = _weapon.GetComponent<WeaponSounds>();
				if (component != null && component.tier > 100)
				{
					_weapon = null;
				}
			}
			if (_weapon != null)
			{
				currentWeaponForKillCam = ItemDb.GetByPrefabName(_weapon.name.Replace("(Clone)", "")).Tag;
			}
		}
		playChargeLoopAnim = false;
		if (_weapon == null)
		{
			_weapon = Resources.Load("Weapons/" + _alternativeNameWeapon) as GameObject;
			if (_weapon != null)
			{
				currentWeaponForKillCam = ItemDb.GetByPrefabName(_weapon.name.Replace("(Clone)", "")).Tag;
			}
		}
		if (_weapon != null)
		{
			string path = ResPath.Combine(a, _weapon.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
			LoadAsyncTool.ObjectRequest weaponRequest = LoadAsyncTool.Get(path, _nameWeapon.Equals("WeaponTurret"));
			while (!weaponRequest.isDone)
			{
				yield return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(_weapon, Vector3.zero, Quaternion.identity);
			gameObject.GetComponent<WeaponSounds>().Initialize(weaponRequest.asset as GameObject);
			if (isMechActive || GameConnect.isDeathEscape)
			{
				gameObject.SetActive(false);
			}
			myCurrentWeapon = gameObject;
			myCurrentWeaponSounds = myCurrentWeapon.GetComponent<WeaponSounds>();
			if (myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
			{
				myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
			}
			if (GameConnect.isDaterRegim)
			{
				SetWeaponVisible(!isBearActive);
			}
			GunFlash = myCurrentWeaponSounds.gunFlash;
			Transform transform = mySkinName.armorPoint.transform;
			if (transform.childCount > 0)
			{
				ArmorRefs component2 = transform.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
				component2.leftBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.LeftArmorHand;
				component2.rightBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.RightArmorHand;
			}
			foreach (Transform item in this.transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			gameObject.transform.parent = this.gameObject.transform;
			gameObject.transform.position = Vector3.zero;
			if (!myCurrentWeaponSounds.isMelee)
			{
				foreach (Transform item2 in gameObject.transform)
				{
					if (item2.gameObject.name.Equals("BulletSpawnPoint") && item2.childCount > 0)
					{
						WeaponManager.SetGunFlashActive(item2.GetChild(0).gameObject, false);
						break;
					}
				}
			}
			if (this.transform.Find("BulletSpawnPoint") != null)
			{
				_bulletSpawnPoint = this.transform.Find("BulletSpawnPoint").gameObject;
			}
			this.transform.localPosition = new Vector3(0f, 0.4f, 0f);
			gameObject.transform.localPosition = new Vector3(0f, -1.4f, 0f);
			gameObject.transform.rotation = this.transform.rotation;
			SetTextureForBodyPlayer(_skin);
		}
		UpdateEffectsForCurrentWeapon(mySkinName.currentCape, mySkinName.currentMask, mySkinName.currentHat);
		if (!skinId.IsNullOrEmpty() && myCurrentWeapon != null)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin != null)
			{
				skin.SetTo(myCurrentWeapon);
			}
		}
	}

	public void SetStealthModifier()
	{
		bool flag3 = _player != null;
	}

	public bool NeedAmmo()
	{
		if (_weaponManager == null)
		{
			return false;
		}
		int currentWeaponIndex = _weaponManager.CurrentWeaponIndex;
		return GetWeaponByIndex(currentWeaponIndex).currentAmmoInBackpack < _weaponManager.currentWeaponSounds.MaxAmmoWithEffectApplied;
	}

	private void SwitchPause()
	{
		if (CurHealth > 0f)
		{
			SetPause();
		}
	}

	private void ShopPressed()
	{
		ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (TrainingController.stepTrainingList.ContainsKey("InterTheShop"))
			{
				TrainingController.isNextStep = TrainingState.EnterTheShop;
				if (Player_move_c.StopBlinkShop != null)
				{
					Player_move_c.StopBlinkShop();
				}
			}
			else
			{
				TrainingController.isNextStep = TrainingState.TapToShoot;
			}
		}
		if (CurHealth > 0f)
		{
			SetInApp();
			SetPause(false);
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(clickShop);
			}
		}
	}

	public void PlayPortalSound()
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				photonView.RPC("PlayPortalSoundRPC", PhotonTargets.All);
			}
		}
		else
		{
			PlayPortalSoundRPC();
		}
	}

	
	[PunRPC]
	public void PlayPortalSoundRPC()
	{
		if (Defs.isSoundFX && portalSound != null)
		{
			GetComponent<AudioSource>().PlayOneShot(portalSound);
		}
	}

	public void AddButtonHandlers()
	{
		PauseTapReceiver.PauseClicked += SwitchPause;
		ShopTapReceiver.ShopClicked += ShopPressed;
		RanksTapReceiver.RanksClicked += RanksPressed;
		TopPanelsTapReceiver.OnClicked += RanksPressed;
		ChatTapReceiver.ChatClicked += ShowChat;
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(true);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(true);
		}
	}

	public void RemoveButtonHandelrs()
	{
		PauseTapReceiver.PauseClicked -= SwitchPause;
		ShopTapReceiver.ShopClicked -= ShopPressed;
		RanksTapReceiver.RanksClicked -= RanksPressed;
		TopPanelsTapReceiver.OnClicked -= RanksPressed;
		ChatTapReceiver.ChatClicked -= ShowChat;
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(false);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(false);
		}
	}

	public void RanksPressed()
	{
		if (!mySkinName.playerMoveC.isKilled)
		{
			ShotUnPressed(true);
			JoystickController.rightJoystick.jumpPressed = false;
			JoystickController.leftTouchPad.isJumpPressed = false;
			JoystickController.rightJoystick.Reset();
			RemoveButtonHandelrs();
			showRanks = true;
			networkStartTableNGUIController.winnerPanelCom1.SetActive(false);
			networkStartTableNGUIController.winnerPanelCom2.SetActive(false);
			networkStartTableNGUIController.ShowRanksTable();
			inGameGUI.gameObject.SetActive(false);
		}
	}

	public void BackRanksPressed()
	{
		AddButtonHandlers();
		showRanks = false;
		if (inGameGUI != null && inGameGUI.interfacePanel != null)
		{
			inGameGUI.gameObject.SetActive(true);
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	
	[PunRPC]
	private void setIp(string _ip)
	{
		myIp = _ip;
	}

	private void CheckTimeCondition()
	{
		CampaignLevel campaignLevel = null;
		foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
		{
			if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
			{
				continue;
			}
			foreach (CampaignLevel level in campaignBox.levels)
			{
				if (level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
				{
					campaignLevel = level;
					break;
				}
			}
			break;
		}
		float timeToComplete = campaignLevel.timeToComplete;
		if (inGameTime >= timeToComplete)
		{
			CurrentCampaignGame.completeInTime = false;
		}
	}

	private IEnumerator GetHardwareKeysInput()
	{
		while (true)
		{
			bool flag = false;
			if (true)
			{
				if (_escapePressed)
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.Log("[Escape] presed in PlayerMoveC");
					}
					_escapePressed = false;
					_backWasPressed = true;
				}
				else
				{
					if (_backWasPressed)
					{
						flag = true;
					}
					_backWasPressed = false;
				}
			}
			if (flag && !isInappWinOpen)
			{
				if (inGameGUI == null || inGameGUI.pausePanel == null)
				{
					yield return null;
					continue;
				}
				if (inGameGUI.blockedCollider.activeSelf)
				{
					yield return null;
					continue;
				}
				SwitchPause();
			}
			yield return null;
		}
	}

	private void InitiailizeIcnreaseArmorEffectFlags()
	{
		BonusEffectForArmorWorksInThisMatch = EffectsController.IcnreaseEquippedArmorPercentage > 1f;
		ArmorBonusGiven = EffectsController.ArmorBonus > 0f;
	}

	private IEnumerator Start()
	{
		if (GameConnect.isSpleef)
		{
			myPlayerTransform.GetComponent<CharacterController>().radius = 0.15f;
		}
		string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.Start()", GetType().Name);
		IEnumerator startSteps = StartSteps();
		int i = 0;
		while (true)
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "Step {0}", i);
			ScopeLogger scopeLogger = new ScopeLogger(thisMethod, callee, Defs.IsDeveloperBuild && !Application.isEditor);
			try
			{
				if (!startSteps.MoveNext())
				{
					break;
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
			yield return startSteps.Current;
			int num = i + 1;
			i = num;
		}
	}

	private void SetPetSpawnPointsActive()
	{
		for (int i = 0; i < petPointsGround.Length; i++)
		{
			if (petPointsGround[i] != null)
			{
				petPointsGround[i].gameObject.SetActive(isMine);
			}
		}
		for (int j = 0; j < petPointsFlying.Length; j++)
		{
			if (petPointsFlying[j] != null)
			{
				petPointsFlying[j].gameObject.SetActive(isMine);
			}
		}
	}

	private IEnumerator StartSteps()
	{
		string caller = string.Format(CultureInfo.InvariantCulture, "{0}.StartSteps()", GetType().Name);
		_bodyMaterial = playerBodyRenderer.material;
		playerBodyRenderer.sharedMaterial = _bodyMaterial;
		if ((GameConnect.isSpeedrun || GameConnect.isDeathEscape) && mySkinName.thirdPersonAnimation != null)
		{
			Renderer[] playerRenderers = mySkinName.thirdPersonAnimation.playerRenderers;
			for (int i = 0; i < playerRenderers.Length; i++)
			{
				playerRenderers[i].sharedMaterial = _bodyMaterial;
			}
		}
		_bearMaterial = new Material(mechBearBodyRenderer.material);
		mechBearBodyRenderer.sharedMaterial = _bearMaterial;
		mechBearHandRenderer.sharedMaterial = _bearMaterial;
		SetMaterialForArms();
		try
		{
			tierForKilledRate = ExpController.OurTierForAnyPlace() + 1;
			weKillForKillRate.Clear();
			weWereKilledForKillRate.Clear();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in cleaning kill rate stats Player_move_c.Start(): " + ex);
			if (weKillForKillRate != null)
			{
				weKillForKillRate.Clear();
			}
			if (weWereKilledForKillRate != null)
			{
				weWereKilledForKillRate.Clear();
			}
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			isImmortality = false;
			timerImmortality = 0f;
		}
		isDaterRegim = GameConnect.isDaterRegim;
		_killerInfo.Reset();
		isNeedTakePremiumAccountRewards = PremiumAccountController.Instance.isAccountActive;
		InitiailizeIcnreaseArmorEffectFlags();
		Initializer.players.Add(this);
		Initializer.playersObj.Add(myPlayerTransform.gameObject);
		if (!Defs.isMulti)
		{
			WeaponManager.sharedManager.myPlayerMoveC = this;
			WeaponManager.sharedManager.myPlayer = myPlayerTransform.gameObject;
		}
		if (GameConnect.isFlag)
		{
			flag1 = Initializer.flag1;
			flag2 = Initializer.flag2;
		}
		timerRegenerationLiveZel = maxTimerRegenerationLiveZel;
		timerRegenerationLiveCape = maxTimerRegenerationLiveCape;
		timerRegenerationArmor = maxTimerRegenerationArmor;
		photonView = PhotonView.Get(this);
		if (isMulti)
		{
			if (isInet)
			{
				if (photonView == null)
				{
					UnityEngine.Debug.Log("Player_move_c.Start():    photonView == null");
				}
				else
				{
					isMine = photonView.isMine;
				}
			}
			SetPetSpawnPointsActive();
		}
		if (Defs.isMulti && Defs.isInet && isMine)
		{
			PhotonNetwork.sendRate = 10;
			PhotonNetwork.sendRateOnSerialize = 10;
		}
		if (drumLoopSound != null && drumLoopSound.transform.GetChild(0) != null)
		{
			drumLoopSound.transform.GetChild(0).gameObject.SetActive(isMulti && !isMine);
		}
		if (!isMulti || isMine)
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(HandleEscape, "Player Move C");
		}
		if ((bool)photonView && photonView.isMine)
		{
			PhotonObjectCacher.AddObject(this.gameObject);
		}
		if (!isMulti || isMine)
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				if (!GameConnect.isDaterRegim && Storager.getInt("GrenadeID") <= 0)
				{
					Storager.setInt("GrenadeID", 1);
				}
				if (GameConnect.isDaterRegim && Storager.getInt("LikeID") <= 0)
				{
					Storager.setInt("LikeID", 1);
				}
			}
			EffectsController.SlowdownCoeff = 1f;
			ScopeLogger scopeLogger = new ScopeLogger(caller, "Resources.Load('InGameGui')", Defs.IsDeveloperBuild && !Application.isEditor);
			UnityEngine.Object original = Resources.Load("InGameGUI");
			scopeLogger.Dispose();
			scopeLogger = new ScopeLogger(caller, "Instantiate(inGameGuiPrefab)", Defs.IsDeveloperBuild && !Application.isEditor);
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.up * 10000f, Quaternion.identity);
			scopeLogger.Dispose();
			inGameGUI = gameObject.GetComponent<InGameGUI>();
			inGameGUI.aimPanel.SetActive(mySkinName.firstPersonControl.isFirstPersonCamera);
			SetGrenateFireEnabled();
			Defs.isJetpackEnabled = false;
			Defs.isTurretWeapon = false;
			oldKilledPlayerCharactersCount = (Storager.hasKey("KilledPlayerCharactersCount") ? Storager.getInt("KilledPlayerCharactersCount") : 0);
		}
		if (!isMulti)
		{
			_skin = SkinsController.currentSkinForPers;
			_skin.filterMode = FilterMode.Point;
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate
			{
				UpdateSkin();
			};
		}
		if (!Defs.isMulti)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("TrainingController");
			if (gameObject2 != null)
			{
				trainigController = gameObject2.GetComponent<TrainingController>();
			}
		}
		expController = ExperienceController.sharedController;
		if (isMulti && isInet)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].GetComponent<PhotonView>().owner == transform.GetComponent<PhotonView>().owner)
				{
					myTable = array[j];
					setMyTamble(myTable);
					break;
				}
			}
		}
		if (isMulti && isInet)
		{
			myPlayerID = myPlayerTransform.GetComponent<PhotonView>().viewID;
		}
		if (!isMulti || isMine)
		{
			UpdatePet();
			ShopNGUIController.EquippedPet += EquippedPet;
			ShopNGUIController.UnequippedPet += UnequipPet;
		}
		if (isMulti && !isMine)
		{
			transform.localPosition = new Vector3(0f, 0.4f, 0f);
		}
		if (!isMulti)
		{
			CurrentCampaignGame.ResetConditionParameters();
			CurrentCampaignGame._levelStartedAtTime = Time.time;
			ZombieCreator.BossKilled += CheckTimeCondition;
		}
		if (GameConnect.isCampaign)
		{
			AnalyticsStuff.AllCombatActivity("Campaign");
		}
		if (isMulti && isCompany && isMine)
		{
			countKillsCommandBlue = GlobalGameController.countKillsBlue;
			countKillsCommandRed = GlobalGameController.countKillsRed;
		}
		if (isMulti && isCOOP)
		{
			zombiManager = ZombiManager.sharedManager;
		}
		if (isMulti && isMine)
		{
			networkStartTableNGUIController = NetworkStartTableNGUIController.sharedController;
		}
		if (!isMulti || isMine)
		{
			InitPurchaseActions();
			ActivityIndicator.IsActiveIndicator = false;
		}
		if (!Defs.isMulti || isMine)
		{
			_inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
			_listener = _inAppGameObject.GetComponent<StoreKitEventListener>();
		}
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		if (isMulti)
		{
			showGUI = isMine;
		}
		if (!isMulti || isMine)
		{
			_player = myPlayerTransform.gameObject;
		}
		else
		{
			_player = null;
		}
		_weaponManager = WeaponManager.sharedManager;
		if (Defs.isMulti && Defs.isInet && photonView.isMine && !NetworkStartTable.StartAfterDisconnect)
		{
			foreach (Weapon allAvailablePlayerWeapon in _weaponManager.allAvailablePlayerWeapons)
			{
				allAvailablePlayerWeapon.currentAmmoInClip = allAvailablePlayerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				allAvailablePlayerWeapon.currentAmmoInBackpack = allAvailablePlayerWeapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
			}
		}
		try
		{
			if (!isMulti || isMine)
			{
				GameObject original2 = Resources.Load("Damage") as GameObject;
				damage = UnityEngine.Object.Instantiate(original2);
				Color color = damage.GetComponent<UnityEngine.UI.RawImage>().color;
				color.a = 0f;
				damage.GetComponent<UnityEngine.UI.RawImage>().color = color;
			}
		}
		catch
		{
		}
		if (!isMulti || isMine)
			{
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("GameController");
				if (gameObject3 != null)
				{
					_pauser = gameObject3.GetComponent<Pauser>();
				}
				if (_pauser == null)
				{
					UnityEngine.Debug.LogWarning("Start(): _pauser is null.");
				}
			}
		if (_singleOrMultiMine())
		{
			numberOfGrenadesOnStart.Value = ((!GameConnect.isHunger) ? ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != 0) ? Storager.getInt(GameConnect.isDaterRegim ? "LikeID" : "GrenadeID") : 0) : 0);
			numberOfGrenades.Value = numberOfGrenadesOnStart.Value;
			if (!isMulti)
			{
				indexWeapon = _weaponManager.CurrentWeaponIndex;
				ChangeWeaponReal(_weaponManager.CurrentWeaponIndex, false);
			}
			else
			{
				int num;
				if (GameConnect.isSpleef)
				{
					if (WeaponManager.sharedManager.SpleefGuns.Any())
					{
						num = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList().FindIndex((Weapon w) => w.weaponPrefab.nameNoClone() == WeaponManager.sharedManager.SpleefGuns.Last());
						if (num == -1)
						{
							UnityEngine.Debug.LogErrorFormat("spleef initial weapon index is -1, last spleef gun is {0}", WeaponManager.sharedManager.SpleefGuns.Last());
							num = 0;
						}
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = _weaponManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons();
				}
				ChangeWeaponReal(num, false);
			}
			_weaponManager.myGun = this.gameObject;
			if (_weaponManager.currentWeaponSounds != null)
			{
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
			}
		}
		if (isMulti && isMine)
		{
			string text = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			if (isInet)
			{
				photonView.RPC("SetNickName", PhotonTargets.AllBuffered, text);
			}
		}
		CurrentBaseArmor = EffectsController.ArmorBonus;
		CurHealth = MaxHealth;
		if (!isMulti || isMine)
		{
			Wear.RenewCurArmor(TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1)));
			string @string = Storager.getString(Defs.ArmorEquppedSN);
			if (_actionsForPurchasedItems.ContainsKey(@string))
			{
				_actionsForPurchasedItems[@string](@string);
				Storager.setString(Defs.ArmorEquppedSN, Defs.ArmorNoneEqupped);
			}
			if (Storager.getInt(Defs.AmmoBoughtSN) == 1)
			{
				if (_actionsForPurchasedItems.ContainsKey("bigammopack"))
				{
					_actionsForPurchasedItems["bigammopack"]("bigammopack");
				}
				Storager.setInt(Defs.AmmoBoughtSN, 0);
			}
		}
		if (_singleOrMultiMine())
		{
			StartCoroutine(GetHardwareKeysInput());
			inGameGUI.health = () => Mathf.Clamp(isMechActive ? liveMech : CurHealth, 0f, float.MaxValue);
			inGameGUI.armor = () => curArmor;
			inGameGUI.killsToMaxKills = () => myScoreController.currentScore.ToString();
			inGameGUI.timeLeft = delegate
			{
				float num4 = (GameConnect.isMiniGame ? ((!(MiniGamesController.Instance != null)) ? 0f : MiniGamesController.Instance.gameTimer) : ((!GameConnect.isDuel) ? ((float)TimeGameController.sharedController.timerToEndMatch) : DuelController.instance.timeLeft));
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				return FormatTime(num4);
			};
			AddButtonHandlers();
			ShopNGUIController.sharedShop.buyAction = PurchaseSuccessful;
			ShopNGUIController.sharedShop.equipAction = delegate
			{
				ChangeWeaponReal(_weaponManager.CurrentWeaponIndex, false);
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.ReloadWeaponFromSet(WeaponManager.sharedManager.CurrentWeaponIndex);
				}
			};
			ShopNGUIController.sharedShop.activatePotionAction = delegate(string potion)
			{
				Storager.setInt(potion, Storager.getInt(potion) - 1);
				PotionsController.sharedController.ActivatePotion(potion, this, new Dictionary<string, object>());
			};
			ShopNGUIController.sharedShop.resumeAction = delegate
			{
				if (base.gameObject != null)
				{
					SetInApp();
					if (inAppOpenedFromPause)
					{
						inAppOpenedFromPause = false;
						if (inGameGUI != null && inGameGUI.pausePanel != null)
						{
							inGameGUI.pausePanel.SetActive(true);
							PauseNGUIController component = inGameGUI.pausePanel.GetComponent<PauseNGUIController>();
							if (component != null && component.settingsPanel != null)
							{
								component.settingsPanel.SetActive(true);
							}
						}
						ExperienceController.sharedController.isShowRanks = true;
					}
					else
					{
						SetPause();
					}
				}
				else
				{
					ShopNGUIController.GuiActive = false;
				}
			};
			ShopNGUIController.sharedShop.wearEquipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem, string equippedItem)
			{
				if (!BonusEffectForArmorWorksInThisMatch)
				{
					float num2 = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN) ?? string.Empty, TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1))) * (EffectsController.IcnreaseEquippedArmorPercentage - 1f);
					float num3 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN) ?? string.Empty, TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1))) * (EffectsController.IcnreaseEquippedArmorPercentage - 1f);
					BonusEffectForArmorWorksInThisMatch = (double)(num2 + num3) > 0.001;
					AddArmor(num2 + num3);
				}
				if (!ArmorBonusGiven)
				{
					ArmorBonusGiven = (double)EffectsController.ArmorBonus > 0.001;
					CurrentBaseArmor += EffectsController.ArmorBonus;
				}
				mySkinName.SetWearVisible();
				if (category == ShopNGUIController.CategoryNames.CapesCategory)
				{
					mySkinName.SetCape();
				}
				if (category == ShopNGUIController.CategoryNames.MaskCategory)
				{
					mySkinName.SetMask();
				}
				if (category == ShopNGUIController.CategoryNames.HatsCategory)
				{
					mySkinName.SetHat();
					if (equippedItem != null && unequippedItem != null && (!Wear.NonArmorHat(equippedItem) || !Wear.NonArmorHat(unequippedItem)))
					{
						CurrentBaseArmor = 0f;
					}
				}
				if (category == ShopNGUIController.CategoryNames.BootsCategory)
				{
					mySkinName.SetBoots();
				}
				if (category == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					mySkinName.SetArmor();
					respawnedForGUI = true;
					CurrentBaseArmor = 0f;
				}
			};
			ShopNGUIController.sharedShop.wearUnequipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem)
			{
				mySkinName.SetWearVisible();
				if (category == ShopNGUIController.CategoryNames.CapesCategory)
				{
					mySkinName.SetCape();
				}
				if (category == ShopNGUIController.CategoryNames.MaskCategory)
				{
					mySkinName.SetMask();
				}
				if (category == ShopNGUIController.CategoryNames.HatsCategory)
				{
					mySkinName.SetHat();
					if (!Wear.NonArmorHat(unequippedItem))
					{
						CurrentBaseArmor = 0f;
					}
				}
				if (category == ShopNGUIController.CategoryNames.BootsCategory)
				{
					mySkinName.SetBoots();
				}
				if (category == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					mySkinName.SetArmor();
					CurrentBaseArmor = 0f;
				}
			};
			ShopNGUIController.ShowArmorChanged += HandleShowArmorChanged;
			ShopNGUIController.ShowWearChanged += HandleShowWearChanged;
		}
		if (NetworkStartTable.StartAfterDisconnect && Defs.isMulti && Defs.isInet && photonView.isMine)
		{
			countKills = GlobalGameController.CountKills;
			myScoreController.currentScore = Mathf.Max(0, GlobalGameController.Score);
			if (countKills < 0)
			{
				countKills = 0;
			}
			if (GlobalGameController.healthMyPlayer > 0f || GameConnect.isHunger)
			{
				CurHealth = GlobalGameController.healthMyPlayer;
				myPlayerTransform.position = GlobalGameController.posMyPlayer;
				myPlayerTransform.rotation = GlobalGameController.rotMyPlayer;
				curArmor = GlobalGameController.armorMyPlayer;
			}
			RatingSystem.instance.BackupLastRatingTake();
			NetworkStartTable.StartAfterDisconnect = false;
		}
		yield return null;
		if (_singleOrMultiMine())
		{
			PotionsController.sharedController.ReactivatePotions(this, new Dictionary<string, object>());
			string string2 = Storager.getString(Defs.HatEquppedSN);
			if (!string2.Equals(Defs.HatNoneEqupped) && Wear.hatsMethods.ContainsKey(string2))
			{
				Wear.hatsMethods[string2].Key(this, new Dictionary<string, object>());
			}
			string string3 = Storager.getString(Defs.CapeEquppedSN);
			if (!string3.Equals(Defs.CapeNoneEqupped) && Wear.capesMethods.ContainsKey(string3))
			{
				Wear.capesMethods[string3].Key(this, new Dictionary<string, object>());
			}
			string string4 = Storager.getString(Defs.BootsEquppedSN);
			if (!string4.Equals(Defs.BootsNoneEqupped) && Wear.bootsMethods.ContainsKey(string4))
			{
				Wear.bootsMethods[string4].Key(this, new Dictionary<string, object>());
			}
			string string5 = Storager.getString(Defs.ArmorNewEquppedSN);
			if (!string5.Equals(Defs.ArmorNewNoneEqupped) && Wear.armorMethods.ContainsKey(string5))
			{
				Wear.armorMethods[string5].Key(this, new Dictionary<string, object>());
			}
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.SetJoystickActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.MakeActive();
			}
			if (JoystickController.leftTouchPad != null)
			{
				JoystickController.leftTouchPad.SetJoystickActive(true);
			}
		}
		if (isMulti && myTable != null)
		{
			_skin = myNetworkStartTable.mySkin;
			if (_skin != null)
			{
				SetTextureForBodyPlayer(_skin);
			}
		}
		if (isMine)
		{
			bool trainingCompleted = TrainingController.TrainingCompleted;
		}
		for (int k = 0; k < Initializer.players.Count; k++)
		{
			Initializer.players[k].SetNicklabelVisible();
		}
		if (isMine && Player_move_c.OnMyPlayerMoveCCreated != null)
		{
			Player_move_c.OnMyPlayerMoveCCreated();
		}
	}

	public static string FormatTime(float timeDown)
	{
		int num = Mathf.FloorToInt(timeDown);
		if (num != _countdownMemo.Key)
		{
			int value = num / 60;
			int num2 = num % 60;
			_sharedStringBuilder.Length = 0;
			string value2 = ((num2 < 10) ? ":0" : ":");
			_sharedStringBuilder.Append(value).Append(value2).Append(num2);
			string value3 = _sharedStringBuilder.ToString();
			_sharedStringBuilder.Length = 0;
			_countdownMemo = new KeyValuePair<int, string>(num, value3);
		}
		return _countdownMemo.Value;
	}

	private void ActualizeNumberOfGrenades()
	{
		if (!GameConnect.isHunger && !SceneLoader.ActiveSceneName.Equals(Defs.TrainingSceneName) && numberOfGrenades.Value != numberOfGrenadesOnStart.Value)
		{
			Storager.setInt(GameConnect.isDaterRegim ? "LikeID" : "GrenadeID", numberOfGrenades.Value);
			numberOfGrenadesOnStart.Value = numberOfGrenades.Value;
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (!_singleOrMultiMine() || !Defs.isMulti)
		{
			return;
		}
		if (pause)
		{
			ActualizeNumberOfGrenades();
			if ((!GameConnect.isCOOP && !GameConnect.isCompany && !GameConnect.isFlag && !GameConnect.isCapturePoints) || liveTime > 90f)
			{
				pausedRating = myNetworkStartTable.CalculateMatchRating(true).addRating < 0;
			}
		}
		else if (pausedRating && PhotonNetwork.connected && PhotonNetwork.inRoom)
		{
			pausedRating = false;
			RatingSystem.instance.BackupLastRatingTake();
		}
	}

	private void HandleShowArmorChanged()
	{
		mySkinName.SetArmor();
		mySkinName.SetHat();
	}

	private void HandleShowWearChanged()
	{
		mySkinName.SetWearVisible();
	}

	public void UpdateSkin()
	{
		if (!isMulti)
		{
			_skin = SkinsController.currentSkinForPers;
			_skin.filterMode = FilterMode.Point;
			SetTextureForBodyPlayer(_skin);
		}
	}

	public void SetIDMyTable(string _id)
	{
		myTableId = _id;
		Invoke("SetIDMyTableInvoke", 0.1f);
	}

	private void SetIDMyTableInvoke()
	{
	}

	
	[PunRPC]
	private void SetIDMyTableRPC(string _id)
	{
		myTableId = _id;
	}

	[PunRPC]
	
	public void SetNickName(string _nickName)
	{
		photonView = PhotonView.Get(this);
		mySkinName.NickName = _nickName;
		if (Defs.isMulti && !isMine)
		{
			nickLabel.gameObject.SetActive(true);
			nickLabel.text = _nickName;
		}
	}

	public bool _singleOrMultiMine()
	{
		if (isMulti)
		{
			return isMine;
		}
		return true;
	}

	private void OnDestroy()
	{
		DestroyEffects();
		if (isMine && Player_move_c.OnMyPlayerMoveCDestroyed != null)
		{
			Player_move_c.OnMyPlayerMoveCDestroyed(liveTime);
		}
		if (isMine && Defs.isMulti && Defs.isInet && ABTestController.useBuffSystem)
		{
			BuffSystem.instance.PlayerLeaved();
		}
		if (!isMulti || isMine)
		{
			ShopNGUIController.EquippedPet -= EquippedPet;
			ShopNGUIController.UnequippedPet -= UnequipPet;
			if (myPetEngine != null)
			{
				myPetEngine.Destroy();
				myPet = null;
			}
		}
		_bodyMaterial = null;
		_mechMaterial = null;
		_bearMaterial = null;
		Initializer.players.Remove(this);
		Initializer.playersObj.Remove(myPlayerTransform.gameObject);
		if (Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Remove(this);
		}
		if (GameConnect.isCapturePoints && CapturePointController.sharedController != null)
		{
			for (int i = 0; i < CapturePointController.sharedController.basePointControllers.Length; i++)
			{
				if (CapturePointController.sharedController.basePointControllers[i].capturePlayers.Contains(this))
				{
					CapturePointController.sharedController.basePointControllers[i].capturePlayers.Remove(this);
				}
			}
		}
		if (_weaponPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveWeaponPopularity();
			_weaponPopularityCacheIsDirty = false;
		}
		if (!isMulti)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
		}
		if (_singleOrMultiMine())
		{
			ActualizeNumberOfGrenades();
			SaveKillRate();
			if (networkStartTableNGUIController != null)
			{
				networkStartTableNGUIController.ranksInterface.SetActive(false);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
			if ((bool)inGameGUI && (bool)inGameGUI.gameObject)
			{
				if (!isHunger && !Defs.isRegimVidosDebug)
				{
					UnityEngine.Object.Destroy(inGameGUI.gameObject);
				}
				else
				{
					inGameGUI.topAnchor.SetActive(false);
					inGameGUI.leftAnchor.SetActive(false);
					inGameGUI.rightAnchor.SetActive(false);
					inGameGUI.joystickContainer.SetActive(false);
					inGameGUI.bottomAnchor.SetActive(false);
					inGameGUI.fastShopPanel.SetActive(false);
					inGameGUI.swipeWeaponPanel.gameObject.SetActive(false);
					inGameGUI.turretPanel.SetActive(false);
					for (int j = 0; j < 3; j++)
					{
						if (inGameGUI.messageAddScore[j].gameObject.activeSelf)
						{
							inGameGUI.messageAddScore[j].gameObject.SetActive(false);
						}
					}
				}
			}
			if (ChatViewrController.sharedController != null)
			{
				ChatViewrController.sharedController.CloseChat(true);
			}
			if (coinsShop.thisScript != null && coinsShop.thisScript.enabled)
			{
				coinsShop.ExitFromShop(false);
			}
			coinsPlashka.hidePlashka();
		}
		if (isMulti && isMine && CameraSceneController.sharedController != null)
		{
			CameraSceneController.sharedController.SetTargetKillCam();
		}
		if ((!isMulti || isMine) && Defs.isTurretWeapon && currentTurret != null)
		{
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					PhotonNetwork.Destroy(currentTurret);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(currentTurret);
			}
		}
		if (_singleOrMultiMine() || (_weaponManager != null && _weaponManager.myPlayer == myPlayerTransform.gameObject))
		{
			if (_pauser != null && (bool)_pauser && _pauser.paused)
			{
				_pauser.paused = !_pauser.paused;
				Time.timeScale = 1f;
				AddButtonHandlers();
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("DamageFrame");
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			RemoveButtonHandelrs();
			ShopNGUIController.sharedShop.buyAction = null;
			ShopNGUIController.sharedShop.equipAction = null;
			ShopNGUIController.sharedShop.activatePotionAction = null;
			ShopNGUIController.sharedShop.resumeAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
			ZombieCreator.BossKilled -= CheckTimeCondition;
			ShopNGUIController.ShowArmorChanged -= HandleShowArmorChanged;
			ShopNGUIController.ShowArmorChanged -= HandleShowWearChanged;
		}
		if (isMulti && isMine)
		{
			ProfileController.ResaveStatisticToKeychain();
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		if (Defs.isMulti && GameConnect.isCOOP)
		{
			int @int = Storager.getInt(Defs.COOPScore);
			int num = ((myNetworkStartTable.score == -1) ? myNetworkStartTable.scoreOld : myNetworkStartTable.score);
			if (num > @int)
			{
				Storager.setInt(Defs.COOPScore, num);
			}
		}
	}

	public void setInString(string nick)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", new object[2]
			{
				nick,
				LocalizationStore.Get("Key_0995")
			}));
		}
	}

	public void setOutString(string nick)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", new object[2]
			{
				nick,
				LocalizationStore.Get("Key_0996")
			}));
		}
	}

	public void AddSystemMessage(string _nick1, string _message2, string _nick2, string _message = null)
	{
		AddSystemMessage(_nick1, _message2, _nick2, Color.white, _message);
	}

	public void AddSystemMessage(string _nick1, string _message2, string _nick2, Color color, string _message = null)
	{
		killedSpisok[2] = killedSpisok[1];
		killedSpisok[1] = killedSpisok[0];
		killedSpisok[0] = new SystemMessage(_nick1, _message2, _nick2, _message, color);
		timerShow[2] = timerShow[1];
		timerShow[1] = timerShow[0];
		timerShow[0] = 3f;
	}

	public void AddSystemMessage(string nick1, int _typeKills, Color color)
	{
		AddSystemMessage(nick1, iconShotName[_typeKills], string.Empty, color);
	}

	public void AddSystemMessage(string nick1, int _typeKills)
	{
		AddSystemMessage(nick1, iconShotName[_typeKills], string.Empty);
	}

	public void AddSystemMessage(string nick1, int _typeKills, string nick2, Color color, string iconWeapon = null)
	{
		AddSystemMessage(nick1, iconShotName[_typeKills], nick2, color, iconWeapon);
	}

	public void AddSystemMessage(string nick1, int _typeKills, string nick2, string iconWeapon = null)
	{
		AddSystemMessage(nick1, iconShotName[_typeKills], nick2, iconWeapon);
	}

	public void AddSystemMessage(string _message)
	{
		AddSystemMessage(_message, string.Empty, string.Empty);
	}

	public void AddSystemMessage(string _message, Color color)
	{
		AddSystemMessage(_message, string.Empty, string.Empty, color);
	}

	[PunRPC]
	
	public void SendSystemMessegeFromFlagDroppedRPC(bool isBlueFlag, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1) || (!isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", new object[2]
				{
					nick,
					LocalizationStore.Get("Key_1798")
				}));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", new object[2]
				{
					nick,
					LocalizationStore.Get("Key_1799")
				}));
			}
		}
	}

	public void SendSystemMessegeFromFlagReturned(bool isBlueFlag)
	{
		photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, isBlueFlag);
	}

	
	[PunRPC]
	public void SendSystemMessegeFromFlagReturnedRPC(bool isBlueFlag)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1) || (!isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1800"));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1801"));
			}
		}
	}

	
	[PunRPC]
	public void SendSystemMessegeFromFlagCaptureRPC(bool isBlueFlag, string nick)
	{
		if (!(WeaponManager.sharedManager.myPlayer != null))
		{
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1 == isBlueFlag)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", new object[2]
			{
				nick,
				LocalizationStore.Get("Key_1001")
			}));
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(flagLostClip);
			}
		}
		else
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1002"));
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(flagGetClip);
			}
		}
	}

	[PunRPC]
	
	public void SendSystemMessegeFromFlagAddScoreRPC(bool isCommandBlue, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot((isCommandBlue == (_weaponManager.myPlayerMoveC.myCommand == 1)) ? flagScoreMyCommandClip : flagScoreEnemyClip);
			}
			isCaptureFlag = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(nick, 5);
		}
	}

	public void SendHouseKeeperEvent()
	{
		countHouseKeeperEvent++;
		if (countHouseKeeperEvent == 1)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.houseKeeperPoint);
		}
		if (countHouseKeeperEvent == 3)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.defenderPoint);
		}
		if (countHouseKeeperEvent == 5)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.guardianPoint);
		}
		if (countHouseKeeperEvent == 10)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.oneManArmyPoint);
		}
	}

	private void ResetHouseKeeperEvent()
	{
		countHouseKeeperEvent = 0;
	}

	public void ShowBonuseParticle(TypeBonuses _type)
	{
		if (Defs.isMulti && Defs.isInet && photonView != null)
		{
			photonView.RPC("ShowBonuseParticleRPC", PhotonTargets.Others, (int)_type);
		}
	}

	[PunRPC]
	
	public void ShowBonuseParticleRPC(int _type)
	{
		if (bonusesParticles.Length >= _type)
		{
			bonusesParticles[_type].ShowParticle();
		}
	}

	public void SetTextureForBodyPlayer(Texture needTx)
	{
		SetMaterialForArms();
		if (_bodyMaterial != null)
		{
			_bodyMaterial.mainTexture = needTx;
		}
	}

	public void SetTextureForActiveMesh(Texture needTx)
	{
		SetMaterialForArms();
		if (mainDamageMaterial != null)
		{
			mainDamageMaterial.mainTexture = needTx;
		}
	}

	private void SetMaterialForArms()
	{
		if (myCurrentWeaponSounds != null && !isBearActive)
		{
			myCurrentWeaponSounds._innerPars.SetMaterialForArms(_bodyMaterial);
		}
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs)
	{
		Transform transform = obj.transform;
		int childCount = obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			bool flag = false;
			foreach (GameObject obj2 in stopObjs)
			{
				if (child.gameObject.Equals(obj2))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			if ((bool)child.gameObject.GetComponent<Renderer>() && (bool)child.gameObject.GetComponent<Renderer>().material)
			{
				child.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
			}
			flag = false;
			foreach (GameObject obj3 in stopObjs)
			{
				if (child.gameObject.Equals(obj3))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				SetTextureRecursivelyFrom(child.gameObject, txt, stopObjs);
			}
		}
	}

	private IEnumerator Flash(GameObject _obj, bool poison = false)
	{
		if (!isDaterRegim)
		{
			SetTextureForBodyPlayer(poison ? SkinsController.poisonHitTexture : SkinsController.damageHitTexture);
			if (isMechActive && currentMech != null)
			{
				currentMech.ShowHitMaterial(true, poison);
			}
			yield return new WaitForSeconds(0.125f);
			SetTextureForBodyPlayer(_skin);
			if (isMechActive && currentMech != null)
			{
				currentMech.ShowHitMaterial(false);
			}
		}
	}

	public static GameObject[] GetStopObjFromPlayer(GameObject _obj)
	{
		List<GameObject> list = new List<GameObject>();
		Transform transform = _obj.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!child.gameObject.name.Equals("GameObject") || child.transform.childCount <= 0)
			{
				continue;
			}
			for (int j = 0; j < child.transform.childCount; j++)
			{
				GameObject gameObject = null;
				GameObject gameObject2 = null;
				WeaponSounds component = child.transform.GetChild(j).gameObject.GetComponent<WeaponSounds>();
				gameObject = component.bonusPrefab;
				if (!component.isMelee)
				{
					gameObject2 = child.transform.GetChild(j).Find("BulletSpawnPoint").gameObject;
				}
				if (component.noFillObjects != null && component.noFillObjects.Length != 0)
				{
					for (int k = 0; k < component.noFillObjects.Length; k++)
					{
						list.Add(component.noFillObjects[k]);
					}
				}
				if (gameObject != null)
				{
					list.Add(gameObject);
				}
				if (gameObject2 != null)
				{
					list.Add(gameObject2);
				}
				if (component.LeftArmorHand != null)
				{
					list.Add(component.LeftArmorHand.gameObject);
				}
				if (component.RightArmorHand != null)
				{
					list.Add(component.RightArmorHand.gameObject);
				}
				if (component.grenatePoint != null)
				{
					list.Add(component.grenatePoint.gameObject);
				}
				if (component.animationObject != null && component.animationObject.GetComponent<InnerWeaponPars>() != null && component.animationObject.GetComponent<InnerWeaponPars>().particlePoint != null)
				{
					list.Add(component.animationObject.GetComponent<InnerWeaponPars>().particlePoint);
				}
				List<GameObject> listWeaponAnimEffects = component.GetListWeaponAnimEffects();
				if (listWeaponAnimEffects != null)
				{
					list.AddRange(listWeaponAnimEffects);
				}
			}
			break;
		}
		if (_obj != null && _obj.GetComponent<SkinName>() != null)
		{
			SkinName component2 = _obj.GetComponent<SkinName>();
			list.Add(component2.capesPoint);
			list.Add(component2.hatsPoint);
			list.Add(component2.maskPoint);
			list.Add(component2.armorPoint);
			list.Add(component2.onGroundEffectsPoint.gameObject);
			if (component2.playerMoveC != null)
			{
				list.Add(component2.playerMoveC.flagPoint);
				list.Add(component2.playerMoveC.invisibleParticle);
				list.Add(component2.playerMoveC.jetPackPoint);
				list.Add(component2.playerMoveC.wingsPoint);
				list.Add(component2.playerMoveC.wingsPointBear);
				list.Add(component2.playerMoveC.turretPoint);
				list.Add(component2.playerMoveC.currentMech.body);
				list.Add(component2.playerMoveC.mechBearPoint);
				list.Add(component2.playerMoveC.mechExplossion);
				list.Add(component2.playerMoveC.bearExplosion);
				if (GameConnect.isDaterRegim && component2.playerMoveC.myCurrentWeaponSounds != null)
				{
					list.Add(component2.playerMoveC.myCurrentWeaponSounds.BearWeaponObject);
				}
				list.Add(component2.playerMoveC.particleBonusesPoint);
				component2.playerMoveC.arrowToPortalPoint.Do(list.Add);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Condition failed: “_obj != null && _obj.GetComponent<SkinName>() != null”");
		}
		return list.ToArray();
	}

	private IEnumerator RunOnGroundEffectCoroutine(string name, float tm)
	{
		yield return new WaitForSeconds(tm);
		RunOnGroundEffect(name);
	}

	private void FixedUpdate()
	{
		if (rocketToLaunch != null && rocketToLaunch.GetComponent<Rocket>().currentRocketSettings != null)
		{
			Rocket component = rocketToLaunch.GetComponent<Rocket>();
			rocketToLaunch.GetComponent<Rigidbody>().AddForce(component.currentRocketSettings.startForce * rocketToLaunch.transform.forward);
			rocketToLaunch = null;
		}
		if (!isMulti || isMine)
		{
			ShopNGUIController.sharedShop.SetInGame(true);
			if (((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled) != isJumpPresedOld && (Defs.isJetpackEnabled || isJumpPresedOld))
			{
				SetJetpackParticleEnabled((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled);
				isJumpPresedOld = (JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled;
			}
		}
		if (isMulti && isMine)
		{
			bool flag3 = Camera.main == null;
		}
	}

	public static int TierOfCurrentRoom()
	{
		if (PhotonNetwork.room != null && PhotonNetwork.room.customProperties.ContainsKey(GameConnect.tierProperty))
		{
			return (int)PhotonNetwork.room.customProperties[GameConnect.tierProperty];
		}
		return ExpController.Instance.OurTier;
	}

	public int TierOrRoomTier(int tier)
	{
		if (Defs.useRatingLobbySystem)
		{
			return tier;
		}
		if (!roomTierInitialized)
		{
			roomTierInitialized = true;
			roomTier = TierOfCurrentRoom();
		}
		return Math.Min(tier, roomTier);
	}

	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		if (currentObject == null)
		{
			UnityEngine.Debug.LogWarningFormat("{0}: currentObject == null", GetType().Name);
			yield break;
		}
		UnityEngine.UI.RawImage texture = currentObject.GetComponent<UnityEngine.UI.RawImage>();
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			if (texture == null)
			{
				UnityEngine.Debug.LogWarningFormat("{0}: texture == null", GetType().Name);
				break;
			}
			Color color = texture.color;
			color.a = Mathf.Lerp(start, end, i);
			texture.color = color;
			yield return 0;
			if (texture == null)
			{
				UnityEngine.Debug.LogWarningFormat("{0}: texture == null", GetType().Name);
				break;
			}
			Color color2 = texture.color;
			color2.a = end;
			texture.color = color2;
		}
	}

	private IEnumerator SetCanReceiveSwipes()
	{
		yield return new WaitForSeconds(0.1f);
		canReceiveSwipes = true;
	}

	private void setisDeadFrameFalse()
	{
		isDeadFrame = false;
	}

	public void UpdateImmortalityAlpColor(float _alpha)
	{
		if (Mathf.Abs(_alpha - oldAlphaImmortality) < 0.001f)
		{
			return;
		}
		oldAlphaImmortality = _alpha;
		if (myCurrentWeaponSounds != null)
		{
			playerBodyRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			Shader shader = Shader.Find("Mobile/Diffuse-Color");
			if (shader != null && myCurrentWeaponSounds.bonusPrefab != null && myCurrentWeaponSounds.bonusPrefab.transform.parent != null)
			{
				myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.shader = shader;
				myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			}
		}
	}

	private void Update()
	{
		if (GameConnect.isHunger && !MiniGamesController.Instance.isGo && isMine)
		{
			isImmortality = true;
			timerImmortality = maxTimerImmortality;
		}
		liveTime += Time.deltaTime;
		if (isMulti && isMine && !isDetectCh && CurHealth > 50f)
		{
			isDetectCh = true;
			Switcher.AppendAbuseMethod(AbuseMetod.health);
		}
		if (_timerDelayInShootingBurst > 0f)
		{
			_timerDelayInShootingBurst -= Time.deltaTime;
		}
		UpdateHealth();
		UpdateNickLabelColor();
		GadgetsUpdate();
		UpdateEffects();
		if (Defs.isInet && isMine && !GameConnect.isCOOP && !GameConnect.isSpleef)
		{
			bool flag = showRanks || showChat || ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy || (_pauser != null && _pauser.paused) || isKilled;
			int num = ((myPetEngine != null && myPetEngine.CurrentState.StateId != PetState.idle && myPetEngine.CurrentState.StateId != PetState.respawn) ? 10 : (flag ? 2 : ((Input.touchCount > 0 || (Application.isEditor && (Input.anyKey || Input.GetAxis("Mouse X") > 0.1f || Input.GetAxis("Mouse Y") > 0.1f))) ? 10 : 5)));
			if (sendRateOld != num)
			{
				if (flag)
				{
					PhotonNetwork.sendRate = num;
					PhotonNetwork.sendRateOnSerialize = num;
				}
				else
				{
					PhotonNetwork.sendRate = num;
					PhotonNetwork.sendRateOnSerialize = num;
				}
				sendRateOld = num;
			}
		}
		if (timerUpdatePointAutoAi > 0f)
		{
			timerUpdatePointAutoAi -= Time.deltaTime;
		}
		if ((!isMulti || isMine) && _timeOfSlowdown > 0f)
		{
			_timeOfSlowdown -= Time.deltaTime;
			if (_timeOfSlowdown <= 0f)
			{
				EffectsController.SlowdownCoeff = 1f;
			}
		}
		if (!isMulti || isMine)
		{
			Defs.isZooming = isZooming;
		}
		if (!isKilled && timerImmortality > 0f)
		{
			timerImmortality -= Time.deltaTime;
			if (timerImmortality <= 0f)
			{
				isImmortality = false;
			}
		}
		if (!isInvisible)
		{
			if (isImmortality && !GameConnect.isDeathEscape)
			{
				float num2 = 1f;
				timerImmortalityForAlpha += Time.deltaTime;
				float num3 = 2f * (timerImmortalityForAlpha - Mathf.Floor(timerImmortalityForAlpha / num2) * num2) / num2;
				if (num3 > 1f)
				{
					num3 = 2f - num3;
				}
				UpdateImmortalityAlpColor(0.5f + num3 * 0.4f);
				isSetVisible = false;
			}
			else if (!isSetVisible)
			{
				UpdateImmortalityAlpColor(1f);
				isSetVisible = true;
			}
		}
		if (isMulti && isMine)
		{
			if ((isCompany || GameConnect.isFlag) && myCommand == 0 && myTable != null)
			{
				myCommand = myNetworkStartTable.myCommand;
			}
			if (GameConnect.isFlag && myBaza == null && myCommand != 0)
			{
				if (myCommand == 1)
				{
					myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
				}
				else
				{
					myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
				}
			}
			if (GameConnect.isFlag && (myFlag == null || enemyFlag == null) && myCommand != 0)
			{
				myFlag = ((myCommand == 1) ? flag1 : flag2);
				enemyFlag = ((myCommand == 1) ? flag2 : flag1);
			}
			if (GameConnect.isFlag && myFlag != null && enemyFlag != null)
			{
				if (!myFlag.isCapture && !myFlag.isBaza && Vector3.SqrMagnitude(myPlayerTransform.position - myFlag.transform.position) < 2.25f)
				{
					photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, myFlag.isBlue);
					myFlag.GoBaza();
				}
				if (!enemyFlag.isCapture && !isKilled && enemyFlag.GetComponent<FlagController>().flagModel.activeSelf && Vector3.SqrMagnitude(myPlayerTransform.position - enemyFlag.transform.position) < 2.25f)
				{
					enemyFlag.SetCapture(photonView.ownerId);
					isCaptureFlag = true;
					photonView.RPC("SendSystemMessegeFromFlagCaptureRPC", PhotonTargets.All, enemyFlag.isBlue, mySkinName.NickName);
				}
			}
			if (isCaptureFlag && Vector3.SqrMagnitude(myPlayerTransform.position - myBaza.transform.position) < 2.25f)
			{
				if (myFlag.isBaza)
				{
					if (Defs.isSoundFX)
					{
						GetComponent<AudioSource>().PlayOneShot(flagScoreMyCommandClip);
					}
					if (myTable != null)
					{
						myNetworkStartTable.AddScore();
					}
					countMultyFlag++;
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyCapture(GameConnect.GameMode.FlagCapture);
					}
					myScoreController.AddScoreOnEvent((countMultyFlag == 3) ? PlayerEventScoreController.ScoreEvent.flagTouchDownTriple : ((countMultyFlag == 2) ? PlayerEventScoreController.ScoreEvent.flagTouchDouble : PlayerEventScoreController.ScoreEvent.flagTouchDown));
					isCaptureFlag = false;
					photonView.RPC("SendSystemMessegeFromFlagAddScoreRPC", PhotonTargets.Others, !enemyFlag.isBlue, mySkinName.NickName);
					AddSystemMessage(LocalizationStore.Get("Key_1003"));
					enemyFlag.GoBaza();
				}
				else if (!inGameGUI.message_returnFlag.activeSelf)
				{
					inGameGUI.message_returnFlag.SetActive(true);
				}
			}
			else if (inGameGUI.message_returnFlag.activeSelf)
			{
				inGameGUI.message_returnFlag.SetActive(false);
			}
			if (GameConnect.isFlag && inGameGUI != null)
			{
				if (isCaptureFlag)
				{
					if (!inGameGUI.flagRedCaptureTexture.activeSelf)
					{
						inGameGUI.flagRedCaptureTexture.SetActive(true);
					}
				}
				else if (inGameGUI.flagRedCaptureTexture.activeSelf)
				{
					inGameGUI.flagRedCaptureTexture.SetActive(false);
				}
			}
		}
		if (!isMulti || isMine)
		{
			if (GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).currentAmmoInClip == 0 && !_changingWeapon && GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).currentAmmoInBackpack > 0 && (!_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot") || (_weaponManager.currentWeaponSounds.countInSeriaBazooka <= 1 && GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).currentAmmoInClip == 0 && !(lastShotTime + 0.2f > Time.time))) && !isReloading)
			{
				ReloadPressed();
			}
			if (!GameConnect.isMiniGame || MiniGamesController.Instance.isGo)
			{
				PotionsController.sharedController.Step(Time.deltaTime, this);
			}
		}
		if (!isMulti)
		{
			inGameTime += Time.deltaTime;
		}
		if ((isCompany || GameConnect.isFlag) && myCommand == 0 && myTable != null)
		{
			myCommand = myNetworkStartTable.myCommand;
		}
		if (isMulti && isMine && _weaponManager.myPlayer != null)
		{
			GlobalGameController.posMyPlayer = _weaponManager.myPlayer.transform.position;
			GlobalGameController.rotMyPlayer = _weaponManager.myPlayer.transform.rotation;
			GlobalGameController.healthMyPlayer = (isFallDown ? 0f : CurHealth);
			GlobalGameController.armorMyPlayer = curArmor;
			if (PhotonNetwork.connected && GameConnect.gameMode == GameConnect.GameMode.Deathmatch)
			{
				GlobalGameController.roomTimeOnReconnect = Convert.ToDouble(PhotonNetwork.room.customProperties[GameConnect.timeProperty]);
			}
		}
		if (!isMulti || isMine)
		{
			if (timerShow[0] > 0f)
			{
				timerShow[0] -= Time.deltaTime;
			}
			if (timerShow[1] > 0f)
			{
				timerShow[1] -= Time.deltaTime;
			}
			if (timerShow[2] > 0f)
			{
				timerShow[2] -= Time.deltaTime;
			}
		}
		if ((!isMulti || isMine) && !((Func<bool>)(() => _pauser != null && _pauser.paused))())
		{
			bool canReceiveSwipe = canReceiveSwipes;
		}
		if (GameConnect.isDaterRegim && isPlayerFlying)
		{
			if (!isMine)
			{
				if (!wingsAnimation.isPlaying)
				{
					wingsAnimation.Play();
				}
				if (!wingsBearAnimation.isPlaying)
				{
					wingsBearAnimation.Play();
				}
			}
			if (Defs.isSoundFX && !wingsSound.isPlaying)
			{
				wingsSound.Play();
			}
		}
		if (!isMulti || isMine)
		{
			ShootUpdate();
		}
	}

	public void SetImmortality(float time)
	{
		isImmortality = true;
		timerImmortality = time;
	}

	
	[PunRPC]
	public void SetWeaponSkinRPC(string skinId, string weaponName)
	{
		if (myCurrentWeapon.nameNoClone() == weaponName)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skinId != null)
			{
				skin.SetTo(myCurrentWeapon);
			}
		}
	}

	private void HandleEscape()
	{
		if (trainigController != null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Ignoring [Escape] in training scene.");
			}
			return;
		}
		if (isMulti && !isMine)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("Ignoring [Escape]; isMulti: {0}, isMine: {1}", isMulti, isMine);
			}
			return;
		}
		if (!Cursor.visible)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Handling [Escape]. Cursor locked.");
			}
			_escapePressed = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			return;
		}
		if (showRanks)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("Ignoring [Escape]; showRanks: {0}", showRanks);
			}
			return;
		}
		if (RespawnWindow.Instance != null && RespawnWindow.Instance.isShown)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Handling [Escape] in Respawn Window.");
			}
			RespawnWindow.Instance.OnBtnGoBattleClick();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject == null)
		{
			if (!isInappWinOpen && Cursor.lockState != CursorLockMode.Locked)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("Handling [Escape]; isInappWinOpen: {0}, lockState: '{1}'", isInappWinOpen, Cursor.lockState);
				}
				_escapePressed = true;
			}
		}
		else if (!gameObject.GetComponent<ChatViewrController>().buySmileBannerPrefab.activeSelf)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Handling [Escape]. Closing chat");
			}
			gameObject.GetComponent<ChatViewrController>().CloseChat();
		}
	}

	public void GoToShopFromPause()
	{
		SetInApp();
		inAppOpenedFromPause = true;
	}

	public void QuitGame()
	{
		Time.timeScale = 1f;
		Time.timeScale = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = Defs.MainMenuScene;
			SceneManager.LoadScene("LevelToCompleteProm");
		}
		else if (isMulti)
		{
			if (!isInet)
			{
				ActivityIndicator.IsActiveIndicator = false;
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				ConnectScene.Local();
			}
			else
			{
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
				PhotonNetwork.LeaveRoom();
			}
		}
		else if (GameConnect.isSurvival)
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
			{
				GlobalGameController.HasSurvivalRecord = true;
				PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
				FriendsController.sharedController.survivalScore = GlobalGameController.Score;
				FriendsController.sharedController.SendOurData();
			}
			if (Storager.getInt("SendFirstResaltArena") != 1)
			{
				Storager.setInt("SendFirstResaltArena", 1);
				AnalyticsStuff.LogArenaFirst(true, false);
			}
			UnityEngine.Debug.Log("Player_move_c.QuitGame(): Trying to report survival score: " + GlobalGameController.Score);
			LevelCompleteScript.LastGameResult = GameResult.Quit;
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			SceneManager.LoadScene("LevelToCompleteProm");
		}
		else if (GameConnect.isCampaign)
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "ChooseLevel";
			bool flag = !isMulti;
			if (flag)
			{
				string reasonToDismissInterstitialCampaign = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialCampaign(false);
				if (string.IsNullOrEmpty(reasonToDismissInterstitialCampaign))
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.Log("<color=magenta>QuitGame()</color>");
					}
					new LevelCompleteInterstitialRunner().Run();
				}
				else
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing interstitial. {0}</color>" : "Dismissing interstitial. {0}", reasonToDismissInterstitialCampaign);
				}
			}
			SceneManager.LoadScene(flag ? "LevelToCompleteProm" : Defs.MainMenuScene);
		}
		else
		{
			LevelCompleteScript.LastGameResult = GameResult.Quit;
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			SceneManager.LoadScene("LevelToCompleteProm");
		}
	}

	public void SetPlayerPause(bool pause)
	{
		if (_pauser.paused != pause)
		{
			SetPause(false);
		}
	}

	public void SetPause(bool showGUI = true)
	{
		ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (_pauser == null)
		{
			UnityEngine.Debug.LogWarning("SetPause(): _pauser is null.");
			return;
		}
		_pauser.paused = !_pauser.paused;
		if (myCurrentWeaponSounds != null && !GameConnect.isDeathEscape && !GameConnect.isSpeedrun)
		{
			myCurrentWeaponSounds.animationObject.SetActive(!_pauser.paused);
		}
		if (_pauser.paused)
		{
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(false);
		}
		else
		{
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(InGameGUI.sharedInGameGUI.isTurretInterfaceActive);
		}
		InGameGUI.sharedInGameGUI.deathEscapeUI.gameObject.SetActive(!_pauser.paused && GameConnect.isDeathEscape);
		if (showGUI && inGameGUI != null && inGameGUI.pausePanel != null)
		{
			inGameGUI.pausePanel.SetActive(_pauser.paused);
			inGameGUI.fastShopPanel.SetActive(!_pauser.paused);
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = _pauser.paused;
				ExpController.Instance.InterfaceEnabled = _pauser.paused;
			}
		}
		if (_pauser.paused)
		{
			if (!isMulti)
			{
				Time.timeScale = 0f;
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
				{
					TrainingController.isPause = true;
				}
			}
		}
		else
		{
			Time.timeScale = 1f;
			TrainingController.isPause = false;
		}
		if (_pauser.paused)
		{
			RemoveButtonHandelrs();
		}
		else
		{
			AddButtonHandlers();
		}
	}

	public void WinFromTimer()
	{
		if (!base.enabled)
		{
			return;
		}
		base.enabled = false;
		InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
		if (GameConnect.isCompany)
		{
			int commandWin = 0;
			if (countKillsCommandBlue > countKillsCommandRed)
			{
				commandWin = 1;
			}
			if (countKillsCommandRed > countKillsCommandBlue)
			{
				commandWin = 2;
			}
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.win("", commandWin, countKillsCommandBlue, countKillsCommandRed);
			}
		}
		else if (GameConnect.isCOOP)
		{
			ZombiManager.sharedManager.EndMatch();
		}
		else if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win("");
		}
	}

	private void SetInApp()
	{
		isInappWinOpen = !isInappWinOpen;
		if (isInappWinOpen)
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!isMulti)
			{
				Time.timeScale = 0f;
			}
			return;
		}
		if (InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.activeSelf)
		{
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None);
		}
		if (InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.activeSelf)
		{
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(true);
		}
		ActivityIndicator.IsActiveIndicator = false;
		if (_pauser == null)
		{
			UnityEngine.Debug.LogWarning("SetInApp(): _pauser is null.");
		}
		else if (!_pauser.paused)
		{
			Time.timeScale = 1f;
		}
	}

	private void ProvideAmmo(string inShopId)
	{
		_listener.ProvideContent();
		_weaponManager.SetMaxAmmoFrAllWeapons();
		if (JoystickController.rightJoystick != null)
		{
			if (inGameGUI != null)
			{
				inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
	}

	public void PurchaseSuccessful(string id)
	{
		if (_actionsForPurchasedItems.ContainsKey(id))
		{
			_actionsForPurchasedItems[id](id);
		}
		_timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if ((bool)photonView && photonView.isMine)
		{
			photonView.RPC("CountKillsCommandSynch", player, countKillsCommandBlue, countKillsCommandRed);
			if (GameConnect.isDaterRegim ? isBigHead : isInvisible)
			{
				photonView.RPC("SetInvisibleRPC", player, GameConnect.isDaterRegim ? isBigHead : isInvisible, isInvisByWeapon);
			}
			photonView.RPC("SetWeaponRPC", player, _sendingNameWeapon, _sendingAlternativeNameWeapon, _sendingSkinId);
			SendSynhHealth(true, player);
			if (GameConnect.isDaterRegim && Defs.isJetpackEnabled)
			{
				photonView.RPC("SetJetpackEnabledRPC", player, Defs.isJetpackEnabled);
			}
			GadgetsOnPlayerConnected();
			if (isBearActive)
			{
				photonView.RPC("ActivateMechRPC", player);
			}
			photonView.RPC("SynhIsZoming", player, isZooming);
			if (ABTestController.useBuffSystem || KillRateCheck.instance.buffEnabled)
			{
				photonView.RPC("SendBuffParameters", player, damageBuff, protectionBuff);
			}
		}
	}

	public void ShowChat()
	{
		if (isKilled)
		{
			return;
		}
		ShotUnPressed(true);
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.jumpPressed = false;
			JoystickController.leftTouchPad.isJumpPressed = false;
			JoystickController.rightJoystick.Reset();
		}
		RemoveButtonHandelrs();
		showChat = true;
		if (inGameGUI.gameObject != null)
		{
			inGameGUI.gameObject.SetActive(false);
		}
		if (!GameConnect.isDeathEscape)
		{
			_weaponManager.currentWeaponSounds.gameObject.SetActive(false);
		}
		if (isMechActive)
		{
			if (GameConnect.isDaterRegim)
			{
				mechBearPoint.SetActive(false);
			}
			else if (currentMech != null)
			{
				currentMech.point.SetActive(false);
			}
		}
		UnityEngine.Object.Instantiate(chatViewer);
	}

	public void SetInvisible(bool _isInvisible, bool byWeapon = false)
	{
		if (isMulti)
		{
			if (isInet && photonView != null)
			{
				photonView.RPC("SetInvisibleRPC", PhotonTargets.All, _isInvisible, byWeapon);
			}
		}
		else
		{
			SetInvisibleRPC(_isInvisible, byWeapon);
		}
	}

	public void SetNicklabelVisible()
	{
		if (!isMine && Defs.isMulti)
		{
			nickLabel.gameObject.SetActive(!isInvisible || ((GameConnect.gameMode == GameConnect.GameMode.CapturePoints || GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture) && WeaponManager.sharedManager.myPlayerMoveC != null && myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand));
		}
	}

	
	[PunRPC]
	private void SetInvisibleRPC(bool _isInvisible, bool _isInvisByWeapon = false)
	{
		if (!GameConnect.isDaterRegim)
		{
			if (_isInvisByWeapon)
			{
				isInvisByWeapon = _isInvisible;
			}
			else
			{
				isInvisByGadget = _isInvisible;
			}
			bool flag = isInvisByGadget || isInvisByWeapon;
			if (flag == isInvisible)
			{
				return;
			}
			isInvisible = flag;
			if (!isMulti || isMine)
			{
				SetInVisibleShaders(flag);
				return;
			}
			SetNicklabelVisible();
			if (!flag)
			{
				invisibleParticle.SetActive(false);
				if (isMechActive)
				{
					if (GameConnect.isDaterRegim)
					{
						mechBearPoint.SetActive(true);
					}
					else if (currentMech != null)
					{
						currentMech.point.SetActive(true);
					}
				}
				else
				{
					mySkinName.FPSplayerObject.SetActive(true);
				}
			}
			else
			{
				invisibleParticle.SetActive(true);
				mySkinName.FPSplayerObject.SetActive(false);
				if (GameConnect.isDaterRegim)
				{
					mechBearPoint.SetActive(false);
				}
				else if (currentMech != null)
				{
					currentMech.point.SetActive(false);
				}
			}
			if (myCurrentWeaponSounds != null)
			{
				myCurrentWeaponSounds.CheckForInvisible();
			}
		}
		else
		{
			if (isBigHead == _isInvisible)
			{
				return;
			}
			isBigHead = _isInvisible;
			if (Defs.isSoundFX && _isInvisible)
			{
				GetComponent<AudioSource>().PlayOneShot(potionSound);
			}
			if (!isMulti || isMine)
			{
				return;
			}
			if (_isInvisible)
			{
				MechHeadTransform.localScale = Vector3.one * 2f;
				PlayerHeadTransform.localScale = Vector3.one * 2f;
				if (isBearActive)
				{
					nickLabel.transform.localPosition = 2.549f * Vector3.up;
				}
				else
				{
					nickLabel.transform.localPosition = 1.678f * Vector3.up;
				}
			}
			else
			{
				MechHeadTransform.localScale = Vector3.one;
				PlayerHeadTransform.localScale = Vector3.one;
				if (isBearActive)
				{
					nickLabel.transform.localPosition = Vector3.up * 1.54f;
				}
				else
				{
					nickLabel.transform.localPosition = Vector3.up * 1.08f;
				}
			}
		}
	}

	private void SetInVisibleShaders(bool _isInvisible)
	{
		WeaponSounds currentWeaponSounds = WeaponManager.sharedManager.currentWeaponSounds;
		if (_isInvisible)
		{
			if (!isGrenadePress)
			{
				Shader shader = Shader.Find("Mobile/Diffuse-Color");
				oldWeaponHandMaterial = currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial;
				Material material = UnityEngine.Object.Instantiate(currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material);
				currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial = material;
				material.shader = shader;
				Color color2;
				if (oldWeaponHandMaterial.HasProperty("_Color"))
				{
					Color color = oldWeaponHandMaterial.GetColor("_Color");
					color2 = new Color(color.r, color.g, color.b, 0.5f);
				}
				else
				{
					color2 = new Color(1f, 1f, 1f, 0.5f);
				}
				material.SetColor("_ColorRili", color2);
				if (currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
				{
					oldWeaponMaterials = currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().sharedMaterials;
					for (int i = 0; i < currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials.Length; i++)
					{
						currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].shader = shader;
						if (oldWeaponMaterials[i].HasProperty("_Color"))
						{
							Color color3 = oldWeaponMaterials[i].GetColor("_Color");
							color2 = new Color(color3.r, color3.g, color3.b, 0.5f);
						}
						else
						{
							color2 = new Color(1f, 1f, 1f, 0.5f);
						}
						currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].SetColor("_ColorRili", color2);
					}
				}
			}
			if (isMechActive && currentMech != null)
			{
				currentMech.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
				currentMech.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			}
			_bodyMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			return;
		}
		if (!isGrenadePress)
		{
			currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial = oldWeaponHandMaterial;
			if (currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
			{
				currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().sharedMaterials = oldWeaponMaterials;
			}
		}
		if (isMechActive && currentMech != null)
		{
			currentMech.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
			currentMech.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		}
		_bodyMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
	}

	
	[PunRPC]
	public void ActivateMechRPC(int num)
	{
		ActivateMech();
	}

	[PunRPC]
	
	public void ActivateMechRPC()
	{
		ActivateMech();
	}

	[PunRPC]
	
	public void DeactivateMechRPC()
	{
		DeactivateMech();
	}

	private void SetWeaponVisible(bool visible)
	{
		Transform transform = ((myCurrentWeaponSounds.grenatePoint != null && myCurrentWeaponSounds.grenatePoint.transform.childCount > 0) ? myCurrentWeaponSounds.grenatePoint.transform.GetChild(0) : null);
		if (transform != null)
		{
			transform.parent = null;
		}
		myCurrentWeaponSounds.SetDaterBearHandsAnim(!visible);
		if (transform != null)
		{
			transform.parent = myCurrentWeaponSounds.grenatePoint;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}

	public void ActivateBear()
	{
		if (isBearActive)
		{
			return;
		}
		float num = -1f;
		if (myCurrentWeaponSounds != null && myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			num = myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		mechExplossionSound = mechBearExplosionSound;
		mechExplossion = bearExplosion;
		if ((!Defs.isMulti || isMine) && isZooming)
		{
			ZoomPress();
		}
		deltaAngle = 0f;
		if (Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(mechBearActivSound);
		}
		isBearActive = true;
		fpsPlayerBody.SetActive(false);
		if (myCurrentWeapon != null)
		{
			SetWeaponVisible(false);
		}
		if (isMine || (!isMine && !isInvisible) || !isMulti)
		{
			mechBearPoint.SetActive(true);
		}
		mechBearPoint.GetComponent<DisableObjectFromTimer>().timer = -1f;
		if (!isMulti || isMine)
		{
			base.transform.localPosition = myCamera.transform.localPosition;
			mechBearBody.SetActive(false);
			mechBearSyncRot.enabled = true;
			mechBearPoint.transform.localPosition = Vector3.zero;
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (myCurrentWeaponSounds.animationObject != null)
			{
				if (myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (!myCurrentWeaponSounds.isDoubleShot)
				{
					if (myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
					{
						myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
					}
				}
				else
				{
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
			}
		}
		else
		{
			bodyCollayder.height = 2.07f;
			bodyCollayder.center = new Vector3(0f, 0.19f, 0f);
			headCollayder.center = new Vector3(0f, 0.54f, 0f);
			if (isBigHead)
			{
				nickLabel.transform.localPosition = 2.549f * Vector3.up;
			}
			else
			{
				nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
		}
		liveMech = liveMechByTier[0];
		if (isMulti && isMine && Defs.isInet)
		{
			photonView.RPC("ActivateMechRPC", PhotonTargets.Others);
		}
		if (num != -1f)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = num;
		}
		mySkinName.SetAnim(mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	public void DeactivateBear()
	{
		if (!isBearActive)
		{
			return;
		}
		isBearActive = false;
		float num = -1f;
		if (myCurrentWeaponSounds != null && myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			num = myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		if (myCurrentWeapon != null)
		{
			SetWeaponVisible(true);
		}
		myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
		if (Defs.isSoundFX)
		{
			mechExplossionSound.Play();
		}
		if (isMulti && !isMine)
		{
			if (!isInvisible)
			{
				fpsPlayerBody.SetActive(true);
			}
			bodyCollayder.height = 1.51f;
			bodyCollayder.center = Vector3.zero;
			headCollayder.center = Vector3.zero;
			mechExplossion.SetActive(true);
			mechExplossion.GetComponent<DisableObjectFromTimer>().timer = 1f;
			mechBearBodyAnimation.Play("Dead");
			mechBearPoint.GetComponent<DisableObjectFromTimer>().timer = 0.46f;
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (myCurrentWeaponSounds.animationObject != null)
			{
				if (myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (!myCurrentWeaponSounds.isDoubleShot)
				{
					if (myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
					{
						myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
					}
				}
				else
				{
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
			}
			if (isBigHead)
			{
				nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
			else
			{
				nickLabel.transform.localPosition = Vector3.up * 1.08f;
			}
		}
		else
		{
			mechBearPoint.SetActive(false);
			gunCamera.fieldOfView = 90f;
			base.transform.localPosition = myCamera.transform.localPosition;
			gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
		}
		if (!isMulti || isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Mech, this);
		}
		if (isMulti && isMine && Defs.isInet)
		{
			photonView.RPC("DeactivateMechRPC", PhotonTargets.Others);
		}
		if (num != -1f)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = Mathf.Min(num, myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].length);
		}
		mySkinName.SetAnim(mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	public void ActivateMech(string id = "")
	{
		if (!isMechActive && GameConnect.isDaterRegim)
		{
			ActivateBear();
		}
	}

	public void DeactivateMech()
	{
		if (GameConnect.isDaterRegim)
		{
			DeactivateBear();
		}
		else
		{
			bool isMechActive2 = isMechActive;
		}
	}

	public void UpdateEffectsForCurrentWeapon(string currentCape, string currentMask, string currentHat)
	{
		if (!(myCurrentWeaponSounds == null))
		{
			if (!isMine)
			{
				_chanceToIgnoreHeadshot = EffectsController.GetChanceToIgnoreHeadshot(myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
			}
			_currentReloadAnimationSpeed = EffectsController.GetReloadAnimationSpeed(myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
			_protectionShieldValue = 1f;
			bool flag = !isMechActive && myCurrentWeaponSounds.specialEffect == WeaponSounds.SpecialEffects.PlayerShield;
			_protectionShieldValue = (flag ? myCurrentWeaponSounds.protectionEffectValue : 1f);
		}
	}

	public void BlockPlayerInEnd()
	{
		mySkinName.BlockFirstPersonController();
		myCurrentWeaponSounds.animationObject.GetComponent<Animation>().enabled = false;
		if (GunFlash != null)
		{
			GunFlash.gameObject.SetActive(false);
		}
		mySkinName.character.enabled = false;
		base.enabled = false;
	}

	public Transform GetPointForFlyingPet()
	{
		for (int i = 0; i < petPointsFlying.Length; i++)
		{
			if (!petPointsFlying[i].isCollision)
			{
				return petPointsFlying[i].thisTransform;
			}
		}
		return null;
	}

	public Transform GetPointForGroundPet()
	{
		for (int i = 0; i < petPointsGround.Length; i++)
		{
			if (!petPointsGround[i].isCollision)
			{
				return petPointsGround[i].thisTransform;
			}
		}
		return null;
	}

	public void EquippedPet(string newPet, string oldPet)
	{
		UpdatePet();
	}

	public void UnequipPet(string oldPet)
	{
		UpdatePet();
	}

	private void UpdatePet()
	{
		if (GameConnect.isHunger || GameConnect.isDeathEscape || GameConnect.isSpleef || GameConnect.isSpeedrun || GameConnect.isSurvival || GameConnect.isCOOP)
		{
			return;
		}
		string eqipedPetId = Singleton<PetsManager>.Instance.GetEqipedPetId();
		mySkinName.SetPet();
		if (myPet != null && myPet.name.Replace("(Clone)", "") != eqipedPetId)
		{
			myPetEngine.Destroy();
			myPet = null;
		}
		if (string.IsNullOrEmpty(eqipedPetId) || myPet != null)
		{
			return;
		}
		string relativePrefabPath = Singleton<PetsManager>.Instance.GetRelativePrefabPath(eqipedPetId);
		PetInfo info = Singleton<PetsManager>.Instance.GetInfo(eqipedPetId);
		Vector3 position = ((info != null) ? ((info.Behaviour == PetInfo.BehaviourType.Ground) ? GetPointForGroundPet().position : GetPointForFlyingPet().position) : GetPointForFlyingPet().position);
		if (isMulti)
		{
			if (Defs.isInet)
			{
				myPet = PhotonNetwork.Instantiate(relativePrefabPath, position, myPlayerTransform.rotation, 0);
			}
		}
		else
		{
			GameObject original = Resources.Load(relativePrefabPath) as GameObject;
			myPet = UnityEngine.Object.Instantiate(original, position, myPlayerTransform.rotation);
		}
		myPet.GetComponent<PetEngine>().SetInfo(eqipedPetId);
	}

	public void SendPlayerEffect(int effectIndex, float effectTime = 0f, int senderPixelID = -1)
	{
		if (Defs.isMulti && Defs.isInet)
		{
			if (senderPixelID == -1)
			{
				photonView.RPC("PlayerEffectRPC", PhotonTargets.Others, effectIndex, effectTime);
			}
			else
			{
				photonView.RPC("PlayerEffectWithSenderRPC", PhotonTargets.Others, effectIndex, effectTime, senderPixelID);
			}
		}
		PlayerEffectRPC(effectIndex, effectTime);
	}

	public void SendPlayerEffect(PhotonPlayer photonPlayer, int effectIndex, float effectTime = 0f, int senderPixelID = -1)
	{
		if (Defs.isMulti && Defs.isInet)
		{
			if (senderPixelID == -1)
			{
				photonView.RPC("PlayerEffectRPC", photonPlayer, effectIndex, effectTime);
			}
			else
			{
				photonView.RPC("PlayerEffectWithSenderRPC", photonPlayer, effectIndex, effectTime, senderPixelID);
			}
		}
	}

	
	[PunRPC]
	public void PlayerEffectRPC(int effectIndex, float effectTime)
	{
		if (effectTime == 0f)
		{
			ActivatePlayerEffect((PlayerEffect)effectIndex, effectTime);
			return;
		}
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == (PlayerEffect)effectIndex)
			{
				playerEffects[i] = playerEffects[i].UpdateTime(effectTime);
				return;
			}
		}
		playerEffects.Add(new ActivePlayerEffect((PlayerEffect)effectIndex, effectTime));
		ActivatePlayerEffect((PlayerEffect)effectIndex, effectTime);
	}

	
	[PunRPC]
	public void PlayerEffectWithSenderRPC(int effectIndex, float effectTime, int senderPixelID)
	{
		if (effectTime == 0f)
		{
			ActivatePlayerEffect((PlayerEffect)effectIndex, effectTime);
			return;
		}
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == (PlayerEffect)effectIndex && (effectIndex != 14 || playerEffects[i].sender.mySkinName.pixelView.viewID.Equals(senderPixelID)))
			{
				playerEffects[i] = playerEffects[i].UpdateTime(effectTime, senderPixelID);
				return;
			}
		}
		playerEffects.Add(new ActivePlayerEffect((PlayerEffect)effectIndex, effectTime, senderPixelID));
		ActivatePlayerEffect((PlayerEffect)effectIndex, effectTime);
	}

	public bool IsPlayerEffectActive(PlayerEffect effect, Player_move_c sender)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == effect && playerEffects[i].sender.Equals(sender))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPlayerEffectActive(PlayerEffect effect)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	private void ActivatePlayerEffect(PlayerEffect effect, float time)
	{
		bool flag = !Defs.isMulti || isMine;
		switch (effect)
		{
		case PlayerEffect.burning:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.burningEffect.Play(time);
				}
				break;
			}
			burningEffect = ParticleStacks.instance.fireStack.GetParticle();
			if (burningEffect != null)
			{
				burningEffect.transform.SetParent(myPlayerTransform, false);
				burningEffect.transform.localPosition = Vector3.zero;
			}
			break;
		case PlayerEffect.charm:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.charmEffect.Play(time);
				}
				break;
			}
			charmEffect = ParticleStacks.instance.charmStack.GetParticle();
			if (charmEffect != null)
			{
				charmEffect.transform.SetParent(myPlayerTransform, false);
				charmEffect.transform.localPosition = Vector3.zero;
			}
			break;
		case PlayerEffect.voodooSnowman:
			if (flag && InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.snowmanEffect.Play(time);
			}
			break;
		case PlayerEffect.fireMushroom:
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(mushroomShotSound);
			}
			RunOnGroundEffect("Weapon278");
			break;
		case PlayerEffect.disabler:
			if (flag)
			{
				gadgetsDisabled = true;
				if (Defs.isSoundFX)
				{
					myAudioSource.PlayOneShot(disablerEffectSound);
				}
			}
			break;
		case PlayerEffect.disablerEffect:
			if (flag && inGameGUI != null)
			{
				inGameGUI.disablerEffect.Play();
			}
			RunOnGroundEffect("gadget_disabler");
			break;
		case PlayerEffect.blackMark:
			if (!flag)
			{
				blackMark.SetActive(true);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.blackMarkEffect.Play(time);
			}
			break;
		case PlayerEffect.dragon:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(dragonWhistleSound);
			}
			UnityEngine.Object.Instantiate(ParticleStacks.instance.dragonPrefab, myPlayerTransform.position, myPlayerTransform.rotation).SetActive(true);
			break;
		case PlayerEffect.lightningEnemies:
		{
			foreach (Transform item in new Initializer.TargetsList(this))
			{
				ParticleStacks.instance.lightningStack.GetParticle().transform.position = item.position;
			}
			break;
		}
		case PlayerEffect.lightningSelf:
			ParticleStacks.instance.lightningStack.GetParticle().transform.position = myPlayerTransform.position;
			break;
		case PlayerEffect.resurrection:
			if (!flag)
			{
				resurrectionEffect.SetActive(true);
				resurrectionEffect.GetComponent<DisableObjectFromTimer>().timer = 2f;
			}
			break;
		case PlayerEffect.timeTravel:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(timeWatchSound);
			}
			if (!flag)
			{
				timeTravelEffect.SetActive(true);
				timeTravelEffect.GetComponent<DisableObjectFromTimer>().timer = 2f;
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.timeTravelEffect.Play();
			}
			break;
		case PlayerEffect.clearPoisons:
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				for (int i = 0; i < WeaponManager.sharedManager.myPlayerMoveC.poisonTargets.Count; i++)
				{
					if (WeaponManager.sharedManager.myPlayerMoveC.poisonTargets[i].target.Equals(myDamageable))
					{
						WeaponManager.sharedManager.myPlayerMoveC.poisonTargets[i].hitCount = 0;
					}
				}
			}
			RemoveEffect(PlayerEffect.burning);
			break;
		case PlayerEffect.attrackPlayer:
		case PlayerEffect.rocketFly:
			break;
		}
	}

	private void DeactivatePlayerEffect(PlayerEffect effect)
	{
		bool flag = !Defs.isMulti || isMine;
		switch (effect)
		{
		case PlayerEffect.burning:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.burningEffect.Stop();
				}
			}
			else if (burningEffect != null && ParticleStacks.instance != null)
			{
				ParticleStacks.instance.fireStack.ReturnParticle(burningEffect);
				burningEffect = null;
			}
			break;
		case PlayerEffect.charm:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.charmEffect.Stop();
				}
			}
			else if (charmEffect != null && ParticleStacks.instance != null)
			{
				ParticleStacks.instance.charmStack.ReturnParticle(charmEffect);
				charmEffect = null;
			}
			break;
		case PlayerEffect.voodooSnowman:
			if (flag && InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.snowmanEffect.Stop();
			}
			break;
		case PlayerEffect.disabler:
			if (flag)
			{
				gadgetsDisabled = false;
			}
			break;
		case PlayerEffect.blackMark:
			if (!flag)
			{
				blackMark.SetActive(false);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.blackMarkEffect.Stop();
			}
			break;
		}
	}

	private void UpdateEffects()
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].lifeTime < Time.time)
			{
				PlayerEffect effect = playerEffects[i].effect;
				playerEffects.RemoveAt(i);
				i--;
				if (!IsPlayerEffectActive(effect))
				{
					DeactivatePlayerEffect(effect);
				}
			}
		}
	}

	private void DestroyEffects()
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == PlayerEffect.burning || playerEffects[i].effect == PlayerEffect.charm)
			{
				DeactivatePlayerEffect(playerEffects[i].effect);
				playerEffects.RemoveAt(i);
				i--;
			}
		}
	}

	public void RemoveEffect(PlayerEffect effect)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == effect)
			{
				DeactivatePlayerEffect(effect);
				playerEffects.RemoveAt(i);
				break;
			}
		}
	}

	private void GadgetsUpdate()
	{
		if (Defs.isMulti && !isMine)
		{
			return;
		}
		AddTimeWatchPositions();
		if (gadgetsPanelEnabled)
		{
			Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Step(Time.deltaTime);
			}
			enumerator.Dispose();
		}
		bool drumActive2 = drumActive;
		drumActive = false;
		drumDamageMultiplier = 1f;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c player_move_c = Initializer.players[i];
			if (((GameConnect.isCOOP || player_move_c.myDamageable.IsEnemyTo(this)) && !player_move_c.Equals(this)) || !((player_move_c.myPlayerTransform.position - myPlayerTransform.position).sqrMagnitude < drumSupportRadius * drumSupportRadius))
			{
				continue;
			}
			for (int j = 0; j < player_move_c.activatedGadgetEffects.Count; j++)
			{
				if (player_move_c.activatedGadgetEffects[j].effect == GadgetEffect.drumSupport)
				{
					drumActive = true;
					drumDamageMultiplier = Mathf.Max(drumDamageMultiplier, 1f + 0.01f * GadgetsInfo.info[player_move_c.activatedGadgetEffects[j].gadgetID].Amplification);
					break;
				}
			}
		}
	}

	public void GadgetsOnMatchEnd()
	{
		if (!Defs.isMulti || isMine)
		{
			Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.OnMatchEnd();
			}
			enumerator.Dispose();
		}
	}

	private void GadgetsOnPlayerConnected()
	{
		for (int i = 0; i < activatedGadgetEffects.Count; i++)
		{
			SetGadgetEffectActivation(activatedGadgetEffects[i].effect, true, activatedGadgetEffects[i].gadgetID);
		}
	}

	public Gadget CurrentDefaultGadget()
	{
		Gadget value = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out value))
		{
			return value;
		}
		return null;
	}

	public void CurrentGadgetPreUse(GadgetInfo.GadgetCategory cat)
	{
		if (canUseGadgets)
		{
			Gadget value = null;
			if (InGameGadgetSet.CurrentSet.TryGetValue(cat, out value) && value.CanUse)
			{
				value.PreUse();
				gadgetWasPreused = true;
			}
		}
	}

	public void CurrentGadgetUse(GadgetInfo.GadgetCategory cat)
	{
		if (canUseGadgets && gadgetWasPreused)
		{
			gadgetWasPreused = false;
			Gadget value = null;
			if (InGameGadgetSet.CurrentSet.TryGetValue(cat, out value) && value.CanUse)
			{
				value.Use();
			}
		}
	}

	public void GadgetOnPlayerDeath(bool inDeathCollider = false)
	{
		if (gadgetsDisabled || !gadgetsPanelEnabled)
		{
			return;
		}
		Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Gadget value = enumerator.Current.Value;
			if (value.CanUse)
			{
				value.OnKill(inDeathCollider);
			}
		}
		enumerator.Dispose();
	}

	public void SetGadgetEffectActivation(GadgetEffect effect, bool acitve, string gadgetID = "")
	{
		SetGadgetEffectActiveRPC((int)effect, acitve, gadgetID);
		if (Defs.isMulti && Defs.isInet)
		{
			photonView.RPC("SetGadgetEffectActiveRPC", PhotonTargets.Others, (int)effect, acitve, gadgetID);
		}
	}

	public bool IsGadgetSelected(string name)
	{
		Gadget value = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out value) && value.CanUse)
		{
			return value.Info.Id.Equals(name);
		}
		return false;
	}

	public bool IsGadgetEffectActive(GadgetEffect effect)
	{
		for (int i = 0; i < activatedGadgetEffects.Count; i++)
		{
			if (activatedGadgetEffects[i].effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	[PunRPC]
	
	private void SetGadgetEffectActiveRPC(int effectIndex, bool active, string gadgetId = "")
	{
		if (active == IsGadgetEffectActive((GadgetEffect)effectIndex))
		{
			return;
		}
		if (active)
		{
			activatedGadgetEffects.Add(new GadgetEffectParams((GadgetEffect)effectIndex, gadgetId));
			ActivateGadgetEffect((GadgetEffect)effectIndex, gadgetId);
			return;
		}
		for (int i = 0; i < activatedGadgetEffects.Count; i++)
		{
			if (activatedGadgetEffects[i].effect == (GadgetEffect)effectIndex)
			{
				activatedGadgetEffects.RemoveAt(i);
			}
		}
		DeactivateGadgetEffect((GadgetEffect)effectIndex);
	}

	private void ActivateGadgetEffect(GadgetEffect effect, string gadgetID)
	{
		switch (effect)
		{
		case GadgetEffect.mech:
		case GadgetEffect.demon:
			ActivateBody(effect, GadgetsInfo.info[gadgetID]);
			break;
		case GadgetEffect.invisible:
			SetInvisibleRPC(true);
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(invisibleActivSound);
			}
			break;
		case GadgetEffect.jetpack:
			ActivateJetpackGadget(true);
			break;
		case GadgetEffect.reflector:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(reflectorOnSound);
				reflectorPulseSound.SetActive(true);
			}
			if (Defs.isMulti && !isMine)
			{
				reflectorParticles.SetActive(true);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.shieldEffect.Play(GadgetsInfo.info[gadgetID].Duration);
			}
			break;
		case GadgetEffect.petAdrenaline:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(petBoosterActiveSound);
			}
			break;
		case GadgetEffect.drumSupport:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(drumActiveSound);
			}
			if (isMine)
			{
				InGameGUI.sharedInGameGUI.drumEffect.Play(true);
			}
			if (drumLoopSound != null)
			{
				drumLoopSound.SetActive(true);
			}
			break;
		case GadgetEffect.disabler:
			break;
		}
	}

	private void DeactivateGadgetEffect(GadgetEffect effect)
	{
		switch (effect)
		{
		case GadgetEffect.mech:
		case GadgetEffect.demon:
			DeactivateBody(effect);
			break;
		case GadgetEffect.invisible:
			SetInvisibleRPC(false);
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(invisibleDeactivSound);
			}
			break;
		case GadgetEffect.jetpack:
			ActivateJetpackGadget(false);
			break;
		case GadgetEffect.reflector:
			if (Defs.isSoundFX)
			{
				myAudioSource.PlayOneShot(reflectorOffSound);
			}
			reflectorPulseSound.SetActive(false);
			if (Defs.isMulti && !isMine)
			{
				reflectorParticles.SetActive(false);
			}
			break;
		case GadgetEffect.drumSupport:
			if (isMine)
			{
				InGameGUI.sharedInGameGUI.drumEffect.Play(false);
			}
			if (drumLoopSound != null)
			{
				drumLoopSound.SetActive(false);
			}
			break;
		case GadgetEffect.disabler:
			break;
		}
	}

	public bool ApplyMedkit(GadgetInfo _info)
	{
		if (CurHealth >= MaxHealth && (myPetEngine == null || !myPetEngine.IsAlive || myPetEngine.CurrentHealth >= myPetEngine.Info.HP))
		{
			return false;
		}
		SendPlayerEffect(13);
		float num = CurHealth + _info.Heal;
		if (num >= MaxHealth)
		{
			CurHealth = MaxHealth;
		}
		else
		{
			CurHealth = num;
		}
		if (Defs.isMulti)
		{
			ShowBonuseParticle(TypeBonuses.Health);
		}
		if (Defs.isSoundFX)
		{
			myAudioSource.PlayOneShot(medkitSound);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.healEffect.Play();
		}
		if (myPetEngine != null)
		{
			myPetEngine.AddCurrentHealth(_info.Heal);
		}
		return true;
	}

	public void ApplyResurrection(bool inDeathCollider = false)
	{
		if (!wasResurrected)
		{
			resurrectionPosition = myPlayerTransform.position;
		}
		wasResurrected = true;
		deadInCollider = inDeathCollider;
	}

	public void ResetWatchPositions()
	{
		firstPositionsReached = false;
		currentTimeIndex = 0;
		nextTimeAdd = 0f;
	}

	private void AddTimeWatchPositions()
	{
		if (isKilled || CurHealth <= 0f || myPlayerTransform.position.y < -5000f)
		{
			ResetWatchPositions();
		}
		else if (nextTimeAdd < Time.time)
		{
			timeRotations[currentTimeIndex] = myPlayerTransform.rotation;
			timeGunRotations[currentTimeIndex] = myCamera.transform.rotation;
			timePositions[currentTimeIndex++] = myPlayerTransform.position;
			if (currentTimeIndex >= timePositions.Length)
			{
				firstPositionsReached = true;
				currentTimeIndex = 0;
			}
			nextTimeAdd = Time.time + 0.3f;
		}
	}

	public void ApplyTimeWatch()
	{
		_isTimeJump = true;
		Vector3 position = timePositions[firstPositionsReached ? currentTimeIndex : 0];
		Quaternion rotation = timeRotations[firstPositionsReached ? currentTimeIndex : 0];
		Quaternion rotation2 = timeGunRotations[firstPositionsReached ? currentTimeIndex : 0];
		myPlayerTransform.position = position;
		myPlayerTransform.rotation = rotation;
		myCamera.transform.rotation = rotation2;
		StartCoroutine(StartTimeWatchEffect());
		ResetWatchPositions();
		SendPlayerEffect(10);
	}

	private IEnumerator StartTimeWatchEffect()
	{
		myCamera.fieldOfView = 1f;
		while (myCamera.fieldOfView < stdFov)
		{
			myCamera.fieldOfView = Mathf.MoveTowards(myCamera.fieldOfView, stdFov, Time.deltaTime * (10f + 10f * (stdFov - myCamera.fieldOfView)));
			yield return null;
		}
		myCamera.fieldOfView = stdFov;
	}

	public void ResurrectionEvent()
	{
		if (Defs.isMulti)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.resurrection);
		}
		wasResurrected = false;
	}

	private void ActivateBody(GadgetEffect type, GadgetInfo param)
	{
		if (isMechActive || currentMech != null)
		{
			return;
		}
		SendSynhHealth(true);
		isMechActive = true;
		bool flag = !Defs.isMulti || isMine;
		if (flag)
		{
			if (isZooming)
			{
				ZoomPress();
			}
			InGameGUI.sharedInGameGUI.StopAllCircularIndicators();
			ShotUnPressed(true);
		}
		deltaAngle = 0f;
		fpsPlayerBody.SetActive(false);
		if (myCurrentWeapon != null)
		{
			myCurrentWeapon.SetActive(false);
		}
		PlayerMechBody playerMechBody = null;
		switch (type)
		{
		case GadgetEffect.mech:
			playerMechBody = mechBodyScript;
			break;
		case GadgetEffect.demon:
			playerMechBody = demonBodyScript;
			break;
		}
		if (playerMechBody == null)
		{
			return;
		}
		currentMech = playerMechBody;
		if (Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(playerMechBody.activationSound);
		}
		if (flag || !isInvisible)
		{
			playerMechBody.point.SetActive(true);
		}
		if (param != null)
		{
			liveMech = param.Durability;
			for (int i = 0; i < playerMechBody.weapon.damageByTier.Length; i++)
			{
				playerMechBody.weapon.damageByTier[i] = param.Damage;
			}
		}
		if (flag)
		{
			SetLayerRecursively(playerMechBody.gun, 9);
			playerMechBody.body.SetActive(false);
			myCamera.transform.localPosition = playerMechBody.cameraPosition;
			playerMechBody.transform.localPosition = new Vector3(0f, -0.3f, 0f);
			gunCamera.fieldOfView = playerMechBody.gunCameraFieldOfView;
			gunCamera.transform.localPosition = playerMechBody.gunCameraPosition;
			if (inGameGUI != null)
			{
				inGameGUI.fireButtonSprite.spriteName = "controls_fire";
				inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
			}
			if (isInvisible)
			{
				playerMechBody.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
				playerMechBody.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			}
			else
			{
				playerMechBody.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
				playerMechBody.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
			}
			if (inGameGUI != null)
			{
				inGameGUI.SetCrosshair(playerMechBody.weapon);
			}
		}
		else
		{
			playerMechBody.ShowActivationEffect();
			playerMechBody.gun.SetActive(!playerMechBody.dontShowHandsInThirdPerson);
			bodyCollayder.height = playerMechBody.bodyColliderHeight;
			bodyCollayder.center = playerMechBody.bodyColliderCenter;
			headCollayder.center = playerMechBody.headColliderCenter;
			nickLabel.transform.localPosition = Vector3.up * playerMechBody.nickLabelYPoision;
			currentMech.jetpackObject.SetActive(jetpackEnabled);
		}
		if (!playerMechBody.weapon.isMelee)
		{
			for (int j = 0; j < playerMechBody.weapon.gunFlashDouble.Length; j++)
			{
				playerMechBody.weapon.gunFlashDouble[j].GetChild(0).gameObject.SetActive(false);
			}
		}
		playerMechBody.ShowHitMaterial(false);
		mySkinName.SetAnim(mySkinName.currentAnim, EffectsController.WeAreStealth);
		UpdateEffectsForCurrentWeapon(mySkinName.currentCape, mySkinName.currentMask, mySkinName.currentHat);
	}

	private void DeactivateBody(GadgetEffect type)
	{
		PlayerMechBody playerMechBody = null;
		switch (type)
		{
		case GadgetEffect.mech:
			playerMechBody = mechBodyScript;
			break;
		case GadgetEffect.demon:
			playerMechBody = demonBodyScript;
			break;
		}
		if (!(playerMechBody == null) && !(playerMechBody != currentMech))
		{
			DeactivateCurrentBody();
		}
	}

	private void DeactivateCurrentBody()
	{
		if (!isMechActive || currentMech == null)
		{
			return;
		}
		isMechActive = false;
		int num;
		if (Defs.isMulti)
		{
			num = (isMine ? 1 : 0);
			if (num == 0)
			{
				goto IL_0044;
			}
		}
		else
		{
			num = 1;
		}
		if (GadgetsOnMechKill != null)
		{
			GadgetsOnMechKill();
		}
		goto IL_0044;
		IL_0044:
		if (myCurrentWeapon != null)
		{
			myCurrentWeapon.SetActive(true);
		}
		PlayerMechBody playerMechBody = currentMech;
		currentMech = null;
		if (Defs.isSoundFX)
		{
			playerMechBody.explosionSound.Play();
		}
		if (num != 0)
		{
			myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
			playerMechBody.point.SetActive(false);
			gunCamera.fieldOfView = 75f;
			if (myCurrentWeaponSounds.isDoubleShot)
			{
				gunCamera.transform.localPosition = Vector3.zero;
			}
			else
			{
				gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
			if (inGameGUI != null)
			{
				if (_weaponManager.currentWeaponSounds.isMelee && !_weaponManager.currentWeaponSounds.isShotMelee)
				{
					inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
				else
				{
					inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
			}
			if (inGameGUI != null)
			{
				inGameGUI.SetCrosshair(_weaponManager.currentWeaponSounds);
			}
		}
		else
		{
			if (!isInvisible)
			{
				fpsPlayerBody.SetActive(true);
			}
			bodyCollayder.height = 1.51f;
			bodyCollayder.center = Vector3.zero;
			headCollayder.center = Vector3.zero;
			playerMechBody.ShowExplosionEffect();
			playerMechBody.bodyAnimation.Play("Dead");
			playerMechBody.gunAnimation.Play("Dead");
			nickLabel.transform.localPosition = Vector3.up * 1.08f;
		}
		mySkinName.SetAnim(mySkinName.currentAnim, EffectsController.WeAreStealth);
		UpdateEffectsForCurrentWeapon(mySkinName.currentCape, mySkinName.currentMask, mySkinName.currentHat);
	}

	public void ActivateFireMushroom()
	{
		if (Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(mushroomActivationSound);
		}
	}

	public void FireMushroomShot(GadgetInfo info)
	{
		if (isKilled || CurHealth <= 0f)
		{
			return;
		}
		SendPlayerEffect(2);
		float num = mushroomRadius * mushroomRadius;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if ((item.position - _player.transform.position).sqrMagnitude < num)
			{
				DamageTarget(item.gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, TypeKills.none);
				PoisonShotWithEffect(item.gameObject, new PoisonParameters(PoisonType.Burn, mushroomBurnDamage, info.Damage, mushroomBurnTime, mushroomBurnCount, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion));
			}
		}
	}

	public void FirecracketsShot(GadgetInfo info)
	{
		if (!isKilled && !(CurHealth <= 0f))
		{
			Vector3 vector = new Vector3(mySkinName.character.velocity.x, 0f, mySkinName.character.velocity.z);
			vector = ((!(vector != Vector3.zero)) ? myPlayerTransform.forward : vector.normalized);
			CreateRocket((Resources.Load("GadgetsContent/" + GadgetsInfo.BaseName(info.Id)) as GameObject).GetComponent<WeaponSounds>(), myPlayerTransform.position - vector, UnityEngine.Random.rotation).multiplayerDamage = info.Damage;
		}
	}

	public void DisablerGadget(GadgetInfo info)
	{
		if (Defs.isSoundFX)
		{
			myAudioSource.PlayOneShot(disablerActiveSound);
		}
		SendPlayerEffect(7);
		if (GameConnect.isCOOP)
		{
			return;
		}
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (!Initializer.players[i].Equals(this) && (!GameConnect.isTeamRegim || myCommand != Initializer.players[i].myCommand) && (Initializer.players[i].myPlayerTransform.position - myPlayerTransform.position).sqrMagnitude < disablerRadius * disablerRadius)
			{
				Initializer.players[i].SendPlayerEffect(3, info.Duration);
			}
		}
	}

	public void BlackMarkPlayer(Player_move_c player, GadgetInfo info)
	{
		player.SendPlayerEffect(4, info.Duration);
	}

	public void UseDragonWhistle(GadgetInfo info)
	{
		SendPlayerEffect(5);
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		float num = 23f;
		float num2 = 2f;
		Vector3 center = myPlayerTransform.position + myPlayerTransform.rotation * new Vector3(0f, 0f, num * 0.6f);
		Vector3 halfExtents = new Vector3(num2, num2, num);
		Collider[] array = Physics.OverlapBox(center, halfExtents, myPlayerTransform.rotation);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i].transform.root.gameObject;
			if (Initializer.IsEnemyTarget(gameObject.transform) && !gameObject.Equals(myPlayerTransform.gameObject) && !hashSet.Contains(gameObject))
			{
				DamageTarget(gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, TypeKills.none);
				PoisonShotWithEffect(gameObject, new PoisonParameters(PoisonType.Burn, 0.02f, info.Damage, 1f, 6, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion));
				hashSet.Add(gameObject);
			}
		}
	}

	public void UsePandoraBox(GadgetInfo info, bool pandoraSuccess)
	{
		if (pandoraSuccess)
		{
			SendPlayerEffect(6);
			foreach (Transform item in new Initializer.TargetsList())
			{
				DamageTarget(item.gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, TypeKills.none);
			}
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.pandoraSuccess);
		}
		else
		{
			SendPlayerEffect(11);
			CurHealth = 1f;
			SendSynhHealth(false);
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.pandoraFail);
		}
	}

	public float DamageMultiplierByGadgets()
	{
		float num = 1f;
		if (drumActive)
		{
			num *= drumDamageMultiplier;
		}
		return num;
	}

	public bool AddHealth(float heal)
	{
		if (CurHealth >= MaxHealth)
		{
			return false;
		}
		CurHealth = Mathf.Min(MaxHealth, CurHealth + heal);
		SendSynhDeltaHealth(heal);
		return true;
	}

	public bool MinusMechHealth(float _minus)
	{
		liveMech -= _minus;
		if (liveMech <= 0f)
		{
			DeactivateCurrentBody();
			return true;
		}
		return false;
	}

	public void SuicidePenalty()
	{
		if (!GameConnect.isDeathEscape && !GameConnect.isSpleef && !GameConnect.isCOOP && WeaponManager.sharedManager.myNetworkStartTable.score > 0)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.suicide);
		}
		if (GameConnect.isDuel)
		{
			countKills = Mathf.Max(0, countKills - 1);
			_weaponManager.myNetworkStartTable.CountKills = countKills;
			_weaponManager.myNetworkStartTable.SynhCountKills();
		}
		if (countKills >= 0)
		{
			GlobalGameController.CountKills = countKills;
		}
	}

	public void ImSuicide()
	{
		isSuicided = true;
		respawnedForGUI = true;
		if (!wasResurrected)
		{
			Invoke("SuicidePenalty", 1.5f);
		}
		if (GameConnect.isFlag && isCaptureFlag)
		{
			enemyFlag.GoBaza();
			isCaptureFlag = false;
			SendSystemMessegeFromFlagReturned(enemyFlag.isBlue);
		}
		if (countKills >= 0)
		{
			GlobalGameController.CountKills = countKills;
		}
		if (!GameConnect.isDeathEscape)
		{
			_weaponManager.myNetworkStartTable.CountKills = countKills;
			_weaponManager.myNetworkStartTable.SynhCountKills();
		}
		if (!wasResurrected)
		{
			sendImDeath(mySkinName.NickName);
		}
		else
		{
			_killerInfo.isSuicide = true;
		}
	}

	private void UpdateHealth()
	{
		if (isMulti && isMine && CurHealth + curArmor - synhHealth > 0.1f)
		{
			SendSynhHealth(true);
		}
		if (!isMulti || isMine)
		{
			if (!isRegenerationLiveCape)
			{
				timerRegenerationLiveCape = maxTimerRegenerationLiveCape;
			}
			if (isRegenerationLiveCape)
			{
				if (timerRegenerationLiveCape > 0f)
				{
					timerRegenerationLiveCape -= Time.deltaTime;
				}
				else
				{
					timerRegenerationLiveCape = maxTimerRegenerationLiveCape;
					if (CurHealth < MaxHealth)
					{
						CurHealth++;
					}
				}
			}
			if (!EffectsController.IsRegeneratingArmor)
			{
				timeSettedAfterRegenerationSwitchedOn = false;
			}
			if (EffectsController.IsRegeneratingArmor)
			{
				if (!timeSettedAfterRegenerationSwitchedOn)
				{
					timeSettedAfterRegenerationSwitchedOn = true;
					timerRegenerationArmor = maxTimerRegenerationArmor;
				}
				if (timerRegenerationArmor > 0f)
				{
					timerRegenerationArmor -= Time.deltaTime;
				}
				else
				{
					timerRegenerationArmor = maxTimerRegenerationArmor;
					if (curArmor < MaxArmor && Storager.getString(Defs.ArmorNewEquppedSN) != Defs.ArmorNewNoneEqupped)
					{
						AddArmor(1f);
					}
				}
			}
			if (!isRegenerationLiveZel)
			{
				timerRegenerationLiveZel = maxTimerRegenerationLiveZel;
			}
			if (isRegenerationLiveZel)
			{
				if (timerRegenerationLiveZel > 0f)
				{
					timerRegenerationLiveZel -= Time.deltaTime;
				}
				else
				{
					timerRegenerationLiveZel = maxTimerRegenerationLiveZel;
					if (CurHealth < MaxHealth)
					{
						CurHealth++;
					}
				}
			}
			if (timerShowUp > 0f)
			{
				timerShowUp -= Time.deltaTime;
			}
			if (timerShowDown > 0f)
			{
				timerShowDown -= Time.deltaTime;
			}
			if (timerShowLeft > 0f)
			{
				timerShowLeft -= Time.deltaTime;
			}
			if (timerShowRight > 0f)
			{
				timerShowRight -= Time.deltaTime;
			}
		}
		if ((isMulti && !isMine) || !(CurHealth <= 0f) || isKilled)
		{
			return;
		}
		bool flag = showRanks || showChat || ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy || (_pauser != null && _pauser.paused);
		if (isMulti && GameConnect.isDeathEscape)
		{
			try
			{
				AnalyticsStuff.DeathEscapeDeath(Mathf.Max(0f, CheckpointController.instance.reachedDistance));
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeDeath send: {0}", ex);
			}
		}
		bool flag2 = isFallDown;
		isFallDown = false;
		GadgetOnPlayerDeath();
		DestroyEffects();
		if (!wasResurrected || deadInCollider)
		{
			countMultyFlag = 0;
			ResetMySpotEvent();
			ResetHouseKeeperEvent();
		}
		if (myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
		}
		if (!wasResurrected && GameConnect.isCOOP)
		{
			SendImKilled();
			SendSynhHealth(false);
		}
		inGameGUI.ResetDamageTaken();
		if (InGameGUI.sharedInGameGUI.isTurretInterfaceActive || Defs.isTurretWeapon)
		{
			CancelTurret();
			InGameGUI.sharedInGameGUI.HideTurretInterface();
			Defs.isTurretWeapon = false;
		}
		if (isGrenadePress)
		{
			ReturnWeaponAfterGrenade();
			isGrenadePress = false;
		}
		if (isZooming)
		{
			ZoomPress();
		}
		inGameGUI.StopAllCircularIndicators();
		if (isMulti)
		{
			if ((!isMulti || isMine) && _player != null && (bool)_player)
			{
				ImpactReceiverTrampoline component = _player.GetComponent<ImpactReceiverTrampoline>();
				if (component != null)
				{
					UnityEngine.Object.Destroy(component);
				}
			}
			if (GameConnect.isFlag && isCaptureFlag)
			{
				isCaptureFlag = false;
				photonView.RPC("SendSystemMessegeFromFlagDroppedRPC", PhotonTargets.All, enemyFlag.isBlue, mySkinName.NickName);
				enemyFlag.SetNOCapture(flagPoint.transform.position, flagPoint.transform.rotation);
			}
			resetMultyKill();
			isKilled = true;
			if (!wasResurrected || deadInCollider)
			{
				if (GameConnect.isCOOP && !isSuicided)
				{
					killedInMatch = true;
				}
				if (Defs.isMulti && isMine && !GameConnect.isHunger && !isSuicided && UnityEngine.Random.Range(0, 100) < 50)
				{
					BonusController.sharedController.AddBonusAfterKillPlayer(new Vector3(myPlayerTransform.position.x, myPlayerTransform.position.y - 1f, myPlayerTransform.position.z));
				}
			}
			isSuicided = false;
			if ((!wasResurrected || deadInCollider) && isHunger && GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).weaponPrefab.name.Replace("(Clone)", "") != WeaponManager.KnifeWN)
			{
				BonusController.sharedController.AddWeaponAfterKillPlayer(GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).weaponPrefab.name, myPlayerTransform.position);
			}
			if (!wasResurrected && !flag2 && Defs.isSoundFX)
			{
				base.gameObject.GetComponent<AudioSource>().PlayOneShot(deadPlayerSound);
			}
			if (!wasResurrected && isCOOP)
			{
				_weaponManager.myNetworkStartTable.score -= 1000;
				if (_weaponManager.myNetworkStartTable.score < 0)
				{
					_weaponManager.myNetworkStartTable.score = 0;
				}
				GlobalGameController.Score = _weaponManager.myNetworkStartTable.score;
				_weaponManager.myNetworkStartTable.SynhScore();
			}
			isDeadFrame = true;
			bool NeedShowWindow = isNeedShowRespawnWindow && !flag;
			if (wasResurrected)
			{
				AutoFade.fadeKilled(0.5f, 1.5f, 0.1f, Color.white);
			}
			else
			{
				AutoFade.fadeKilled(0.5f, (NeedShowWindow && !Defs.inRespawnWindow) ? 0.5f : (GameConnect.isDeathEscape ? 0.5f : 1.5f), 0.5f, Color.white);
			}
			Invoke("setisDeadFrameFalse", 1f);
			StartCoroutine(FlashWhenDead());
			SetJoysticksActive(false);
			if (Defs.inRespawnWindow)
			{
				Defs.inRespawnWindow = false;
				RespawnPlayer();
				return;
			}
			Vector3 localPosition = myPlayerTransform.localPosition;
			inGameGUI.blockedCollider.SetActive(true);
			TweenParms p_parms = new TweenParms().Prop("localPosition", new Vector3(localPosition.x, localPosition.y + (flag2 ? (-40f) : 100f), localPosition.z)).Ease((!flag2) ? EaseType.EaseInCubic : EaseType.Linear).OnComplete((TweenDelegate.TweenCallback)delegate
			{
				myPlayerTransform.localPosition = new Vector3(0f, -1000f, 0f);
				if (wasResurrected)
				{
					ResurrectPlayer();
				}
				else if (NeedShowWindow && !Defs.inRespawnWindow && _killerInfo.killerTransform != null)
				{
					SetMapCameraActive(true);
					KillCam();
				}
				else
				{
					Defs.inRespawnWindow = false;
					RespawnPlayer();
				}
				inGameGUI.blockedCollider.SetActive(false);
			});
			HOTween.To(myPlayerTransform, NeedShowWindow ? 0.75f : (GameConnect.isDeathEscape ? 1f : 2f), p_parms);
		}
		else if (!wasResurrected)
		{
			if (GameConnect.isSurvival)
			{
				if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
				{
					GlobalGameController.HasSurvivalRecord = true;
					PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
					PlayerPrefs.Save();
					FriendsController.sharedController.survivalScore = GlobalGameController.Score;
					FriendsController.sharedController.SendOurData();
				}
				if (ZombieCreator.sharedCreator != null)
				{
					if (Storager.getInt("SendFirstResaltArena") != 1)
					{
						Storager.setInt("SendFirstResaltArena", 1);
						AnalyticsStuff.LogArenaFirst(false, ZombieCreator.sharedCreator.currentWave > 0);
					}
					AnalyticsStuff.LogArenaWavesPassed(ZombieCreator.sharedCreator.currentWave);
				}
			}
			else if (GameConnect.isCampaign && GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
			{
				PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
			}
			if (!GameConnect.isSpeedrun || !SpeedrunTrackController.instance.canRespawn)
			{
				if (GameConnect.isSpeedrun)
				{
					AnalyticsStuff.SpeedrunDistance(GlobalGameController.Score);
				}
				LevelCompleteScript.LastGameResult = GameResult.Death;
				LevelCompleteLoader.action = null;
				LevelCompleteLoader.sceneName = "LevelComplete";
				Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm");
				return;
			}
			isKilled = true;
			isSuicided = false;
			isDeadFrame = true;
			AutoFade.fadeKilled(0.5f, 0.5f, 0.5f, Color.white);
			Invoke("setisDeadFrameFalse", 1f);
			StartCoroutine(FlashWhenDead());
			SetJoysticksActive(false);
			Vector3 localPosition2 = myPlayerTransform.localPosition;
			inGameGUI.blockedCollider.SetActive(true);
			TweenParms p_parms2 = new TweenParms().Prop("localPosition", new Vector3(localPosition2.x, localPosition2.y + (flag2 ? (-40f) : 100f), localPosition2.z)).Ease((!flag2) ? EaseType.EaseInCubic : EaseType.Linear).OnComplete((TweenDelegate.TweenCallback)delegate
			{
				myPlayerTransform.localPosition = new Vector3(0f, -1000f, 0f);
				RespawnPlayer();
				inGameGUI.blockedCollider.SetActive(false);
			});
			HOTween.To(myPlayerTransform, 1f, p_parms2);
		}
		else
		{
			isKilled = true;
			isSuicided = false;
			isDeadFrame = true;
			AutoFade.fadeKilled(0.5f, 1.5f, 0.1f, Color.white);
			Invoke("setisDeadFrameFalse", 1f);
			StartCoroutine(FlashWhenDead());
			SetJoysticksActive(false);
			Vector3 localPosition3 = myPlayerTransform.localPosition;
			TweenParms p_parms3 = new TweenParms().Prop("localPosition", new Vector3(localPosition3.x, localPosition3.y + 100f, localPosition3.z)).Ease(EaseType.EaseInCubic).OnComplete((TweenDelegate.TweenCallback)delegate
			{
				ResurrectPlayer();
			});
			HOTween.To(myPlayerTransform, 2f, p_parms3);
		}
	}

	private void SetJoysticksActive(bool active)
	{
		if (JoystickController.leftJoystick != null)
		{
			if (!active)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
			}
			JoystickController.leftJoystick.SetJoystickActive(active);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(active);
		}
		if (JoystickController.rightJoystick != null)
		{
			if (!active)
			{
				JoystickController.rightJoystick.gameObject.SetActive(false);
				JoystickController.rightJoystick.MakeInactive();
			}
			else
			{
				JoystickController.rightJoystick.MakeActive();
			}
		}
	}

	public void SuicideFall()
	{
		if ((!isMulti || isMine) && !isKilled && !(CurHealth <= 0f) && !isFallDown)
		{
			isFallDown = true;
			if (GameConnect.isSpeedrun && mySkinName.thirdPersonAnimation != null)
			{
				mySkinName.thirdPersonAnimation.StartFall();
			}
			StartCoroutine(SuicideFallCoroutine());
		}
	}

	public IEnumerator SuicideFallCoroutine()
	{
		if (mySkinName.firstPersonControl.thirdCamera != null)
		{
			mySkinName.firstPersonControl.thirdCamera.DontFollowPivot();
		}
		if (Defs.isSoundFX)
		{
			gameObject.GetComponent<AudioSource>().PlayOneShot(fallPlayerSound);
		}
		yield return new WaitForSeconds((Defs.isMulti || (GameConnect.isSpeedrun && SpeedrunTrackController.instance.canRespawn)) ? 0.3f : 1f);
		curArmor = 0f;
		CurHealth = 0f;
		GadgetOnPlayerDeath(true);
		if (Defs.isMulti)
		{
			ImSuicide();
		}
	}

	public void KillSelf(WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.angel)
	{
		if ((isMulti && !isMine) || isKilled || CurHealth <= 0f)
		{
			return;
		}
		curArmor = 0f;
		CurHealth = 0f;
		GadgetOnPlayerDeath(true);
		if (Defs.isMulti)
		{
			ImSuicide();
			if (GameConnect.isCOOP)
			{
				return;
			}
			SendImKilled(typeDead);
			if (GameConnect.isDeathEscape)
			{
				fpsPlayerBody.SetActive(false);
				if (myCurrentWeapon != null)
				{
					myCurrentWeapon.SetActive(false);
				}
				if (mySkinName.thirdPersonAnimation != null)
				{
					mySkinName.thirdPersonAnimation.gameObject.SetActive(false);
				}
			}
		}
		else if (GameConnect.isSpeedrun && SpeedrunTrackController.instance.canRespawn)
		{
			ImKilled(myPlayerTransform.position, myPlayerTransform.rotation, (int)typeDead);
			if (mySkinName.thirdPersonAnimation != null)
			{
				mySkinName.thirdPersonAnimation.gameObject.SetActive(false);
			}
		}
		else
		{
			StartFlash(mySkinName.gameObject);
		}
	}

	private bool GetDamageForSync(float damage)
	{
		if (!Defs.isMulti || isMine)
		{
			return false;
		}
		synhHealth -= damage;
		if (synhHealth < 0f)
		{
			synhHealth = 0f;
		}
		if (armorSynch > damage)
		{
			armorSynch -= damage;
		}
		else
		{
			armorSynch = 0f;
		}
		return synhHealth <= 0f;
	}

	private void GetDamageInternal(float damage, TypeKills _typeKills, WeaponSounds.TypeDead typeDead, Vector3 posKiller, string weaponName, int idKiller)
	{
		MinusLiveRPCEffects(_typeKills);
		if ((Defs.isMulti && !isMine) || isKilled || isImmortality)
		{
			return;
		}
		if (_typeKills != TypeKills.mob && (idKiller == skinNamePixelView.viewID || idKiller == 0))
		{
			_typeKills = TypeKills.himself;
		}
		float num = 0f;
		if (isMechActive)
		{
			MinusMechHealth(damage);
		}
		else
		{
			num = damage - curArmor;
			if (num < 0f)
			{
				curArmor -= damage;
				num = 0f;
			}
			else
			{
				curArmor = 0f;
				if (!Defs.isMulti)
				{
					CurrentCampaignGame.withoutHits = false;
				}
			}
		}
		if (CurHealth > 0f)
		{
			CurHealth -= num;
			if (Defs.isMulti && CurHealth <= 0f)
			{
				GadgetOnPlayerDeath();
				switch (_typeKills)
				{
				case TypeKills.himself:
					ImSuicide();
					if (!GameConnect.isCOOP)
					{
						SendImKilled();
					}
					break;
				default:
				{
					if (!wasResurrected)
					{
						try
						{
							if (!WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
							{
								WeaponManager.sharedManager.myPlayerMoveC.AddWeWereKilledStatisctics((WeaponManager.sharedManager.currentWeaponSounds.name ?? "").Replace("(Clone)", ""));
							}
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError("Exception we were killed AddWeWereKilledStatisctics: " + ex);
						}
						if (placemarkerMoveC != null)
						{
							placemarkerMoveC.isPlacemarker = false;
						}
						if (Defs.isInet)
						{
							photonView.RPC("KilledPhoton", PhotonTargets.All, idKiller, (int)_typeKills, weaponName, (int)typeDead);
						}
						break;
					}
					for (int i = 0; i < Initializer.players.Count; i++)
					{
						if (Initializer.players[i].mySkinName.pixelView != null && Initializer.players[i].mySkinName.pixelView.viewID == idKiller)
						{
							isResurrectionKill = true;
							resurrectionMoveC = Initializer.players[i];
							break;
						}
					}
					break;
				}
				case TypeKills.mob:
					break;
				}
			}
			SynhHealthRPC(CurHealth + curArmor, curArmor, false);
		}
		if (posKiller != Vector3.zero && _typeKills != TypeKills.burning && _typeKills != TypeKills.poison && _typeKills != TypeKills.bleeding)
		{
			ShowDamageDirection(posKiller);
			if (myPetEngine != null)
			{
				myPetEngine.OwnerAttacked(_typeKills, idKiller);
			}
		}
	}

	private void KillMechInDemon()
	{
		if (this.OnMyKillMechInDemon != null)
		{
			this.OnMyKillMechInDemon();
		}
	}

	public void GetDamage(float damage, TypeKills _typeKills, WeaponSounds.TypeDead _typeDead = WeaponSounds.TypeDead.angel, Vector3 posKiller = default(Vector3), string weaponName = "", int idKiller = 0)
	{
		if (GameConnect.isDaterRegim || isImmortality)
		{
			return;
		}
		if (Defs.isMulti && !isMine && !GameConnect.isCOOP && _typeKills == TypeKills.burning && _typeKills == TypeKills.poison)
		{
			ProfileController.OnGameHit();
		}
		damage *= _protectionShieldValue;
		damage = ((ABTestController.useBuffSystem && BuffSystem.instance.haveBuffForWeapon(weaponName)) ? (damage * BuffSystem.instance.weaponBuffValue) : (damage * WeaponManager.sharedManager.myPlayerMoveC.damageBuff));
		damage /= protectionBuff;
		if (WeaponManager.sharedManager.myPlayerMoveC.IsPlayerEffectActive(PlayerEffect.blackMark))
		{
			damage *= 0.5f;
		}
		if (Defs.isMulti && !isMine)
		{
			if (isMechActive)
			{
				PlayerEventScoreController.ScoreEvent @event = ((currentMech != null) ? currentMech.scoreEventOnKill : PlayerEventScoreController.ScoreEvent.deadMech);
				bool flag = IsGadgetEffectActive(GadgetEffect.mech);
				if (MinusMechHealth(damage))
				{
					WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(@event);
					damage = 1000f;
					if (flag && WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(GadgetEffect.demon))
					{
						WeaponManager.sharedManager.myPlayerMoveC.KillMechInDemon();
					}
					if (_typeKills != TypeKills.mech && _typeKills != TypeKills.turret)
					{
						try
						{
							WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? "").Replace("(Clone)", ""));
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError("Exception we were killed AddWeKillStatisctics: " + ex);
						}
					}
				}
			}
			else if (synhHealth > 0f)
			{
				getLocalHurt = true;
				if (GetDamageForSync(damage))
				{
					if (_typeKills != TypeKills.mech && _typeKills != TypeKills.turret)
					{
						try
						{
							WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? "").Replace("(Clone)", ""));
						}
						catch (Exception ex2)
						{
							UnityEngine.Debug.LogError("Exception we were killed AddWeKillStatisctics: " + ex2);
						}
					}
					damage = 10000f;
					if (isCaptureFlag)
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deadWithFlag);
						if (!NetworkStartTable.LocalOrPasswordRoom())
						{
							QuestMediator.NotifyKillOtherPlayerWithFlag();
						}
					}
					if (GameConnect.isCapturePoints && WeaponManager.sharedManager.myPlayerMoveC != null)
					{
						for (int i = 0; i < CapturePointController.sharedController.basePointControllers.Length; i++)
						{
							if (CapturePointController.sharedController.basePointControllers[i].captureConmmand == (BasePointController.TypeCapture)WeaponManager.sharedManager.myPlayerMoveC.myCommand && CapturePointController.sharedController.basePointControllers[i].capturePlayers.Contains(this))
							{
								isRaiderMyPoint = true;
								break;
							}
						}
					}
					if (getLocalHurt)
					{
						getLocalHurt = false;
					}
					ImKilled(myPlayerTransform.position, myPlayerTransform.rotation, (int)_typeDead);
					myPersonNetwork.StartAngel();
					if (GameConnect.isFlag && isCaptureFlag)
					{
						FlagController flagController = null;
						if (flag1.targetTrasform == flagPoint.transform)
						{
							flagController = flag1;
						}
						if (flag2.targetTrasform == flagPoint.transform)
						{
							flagController = flag2;
						}
						if (flagController != null)
						{
							flagController.SetNOCaptureRPC(myPlayerTransform.position, myPlayerTransform.rotation);
						}
					}
				}
			}
		}
		if (Defs.isMulti && Defs.isInet)
		{
			if (idKiller == 0)
			{
				if (posKiller == Vector3.zero)
				{
					photonView.RPC("GetDamageNoKillerRPC", PhotonTargets.Others, damage, (int)_typeKills);
				}
				else
				{
					photonView.RPC("GetDamageNoKillerRPC", PhotonTargets.Others, damage, (int)_typeKills, posKiller);
				}
			}
			else
			{
				short result = 0;
				if (weaponName.Length > 6 && short.TryParse(weaponName.Substring(6), out result))
				{
					photonView.RPC("GetDamageRPCWithWeaponIndex", PhotonTargets.Others, damage, (byte)_typeKills, (byte)_typeDead, idKiller, result);
				}
				else
				{
					photonView.RPC("GetDamageRPC", PhotonTargets.Others, damage, (byte)_typeKills, (byte)_typeDead, idKiller, weaponName);
				}
			}
		}
		GetDamageInternal(damage, _typeKills, _typeDead, posKiller, weaponName, idKiller);
	}

	[PunRPC]
	public void GetDamageNoKillerRPC(float minus, int _typeKills)
	{
		if (Defs.isMulti && !isMine)
		{
			GetDamageForSync(minus);
		}
		GetDamageInternal(minus, (TypeKills)_typeKills, WeaponSounds.TypeDead.angel, Vector3.zero, "", 0);
	}

	[PunRPC]
	public void GetDamageNoKillerRPC(float minus, int _typeKills, Vector3 enemyPos)
	{
		if (Defs.isMulti && !isMine)
		{
			GetDamageForSync(minus);
		}
		GetDamageInternal(minus, (TypeKills)_typeKills, WeaponSounds.TypeDead.angel, enemyPos, "", 0);
	}

	[PunRPC]
	
	public void GetDamageRPCWithWeaponIndex(float minus, byte _typeKills, byte _typeWeapon, int idKiller, short weaponId)
	{
		GetDamageRPC(minus, _typeKills, _typeWeapon, idKiller, "Weapon" + weaponId);
	}

	[PunRPC]
	
	public void GetDamageRPCLocal(float minus, int _typeKills, int _typeWeapon, int idKiller, string weaponName)
	{
		GetDamageRPC(minus, (byte)_typeKills, (byte)_typeWeapon, idKiller, weaponName);
	}

	[PunRPC]
	
	public void GetDamageRPC(float minus, byte _typeKills, byte _typeWeapon, int idKiller, string weaponName)
	{
		if (Defs.isMulti && !isMine)
		{
			GetDamageForSync(minus);
		}
		Vector3 posKiller = Vector3.zero;
		if (idKiller > 0)
		{
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.players[i].pixelView.viewID == idKiller)
				{
					posKiller = Initializer.players[i].myPlayerTransform.position;
				}
			}
		}
		GetDamageInternal(minus, (TypeKills)_typeKills, (WeaponSounds.TypeDead)_typeWeapon, posKiller, weaponName, idKiller);
	}

	private void MinusLiveRPCEffects(TypeKills _typeKills)
	{
		if (!Device.isPixelGunLow && Defs.isMulti && !isDaterRegim && !isMine)
		{
			switch (_typeKills)
			{
			case TypeKills.headshot:
			{
				HitParticle currentParticle2 = HeadShotStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle2 != null)
				{
					currentParticle2.StartShowParticle(myPlayerTransform.position, myPlayerTransform.rotation, false);
				}
				break;
			}
			case TypeKills.poison:
			{
				HitParticle currentParticle4 = ParticleStacks.instance.poisonHitStack.GetCurrentParticle(false);
				if (currentParticle4 != null)
				{
					currentParticle4.StartShowParticle(myPlayerTransform.position, myPlayerTransform.rotation, false);
				}
				break;
			}
			case TypeKills.critical:
			{
				HitParticle currentParticle5 = ParticleStacks.instance.criticalHitStack.GetCurrentParticle(false);
				if (currentParticle5 != null)
				{
					currentParticle5.StartShowParticle(myPlayerTransform.position, myPlayerTransform.rotation, false);
				}
				break;
			}
			case TypeKills.bleeding:
			{
				HitParticle currentParticle3 = ParticleStacks.instance.bleedHitStack.GetCurrentParticle(false);
				if (currentParticle3 != null)
				{
					currentParticle3.StartShowParticle(myPlayerTransform.position, myPlayerTransform.rotation, false);
				}
				break;
			}
			default:
			{
				HitParticle currentParticle = ParticleStacks.instance.hitStack.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShowParticle(myPlayerTransform.position, myPlayerTransform.rotation, false);
				}
				break;
			}
			}
		}
		if (!Defs.isMulti || isMine)
		{
			switch (_typeKills)
			{
			case TypeKills.poison:
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.poisonEffect.Play();
				}
				break;
			case TypeKills.bleeding:
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.bleedEffect.Play();
				}
				break;
			}
		}
		if (Defs.isSoundFX)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot((curArmor > 0f || isMechActive) ? damageArmorPlayerSound : ((_typeKills == TypeKills.headshot) ? headShotSound : damagePlayerSound));
		}
		StartCoroutine(Flash(myPlayerTransform.gameObject, _typeKills == TypeKills.poison));
	}

	public void SendSynhHealth(bool isUp, PhotonPlayer player = null)
	{
		if (isMulti && isMine && Defs.isInet)
		{
			if (player == null)
			{
				photonView.RPC("SynhHealthRPC", PhotonTargets.All, CurHealth + curArmor, curArmor, isUp);
			}
			else
			{
				photonView.RPC("SynhHealthRPC", player, CurHealth + curArmor, curArmor, isUp);
			}
		}
	}

	public void SendSynhDeltaHealth(float deltaHealth)
	{
		if (isMulti && isMine && Defs.isInet)
		{
			photonView.RPC("SynhDeltaHealthRPC", PhotonTargets.All, deltaHealth);
		}
	}

	
	[PunRPC]
	private void SynhDeltaHealthRPC(float healthDelta)
	{
		SynhHealthRPC(Mathf.Min(synhHealth + healthDelta, MaxHealth), armorSynch, healthDelta > 0f);
	}

	[PunRPC]
	
	private void SynhHealthRPC(float _synhHealth, float _synchArmor, bool isUp)
	{
		if (isMine)
		{
			synhHealth = _synhHealth;
		}
		else if (!isUp)
		{
			if (_synhHealth < synhHealth)
			{
				synhHealth = _synhHealth;
			}
			if (_synchArmor < armorSynch)
			{
				armorSynch = _synchArmor;
			}
		}
		else
		{
			synhHealth = _synhHealth;
			armorSynch = _synchArmor;
			isRaiderMyPoint = false;
		}
		if (synhHealth > 0f)
		{
			isStartAngel = false;
			myPersonNetwork.isStartAngel = false;
		}
	}

	private void ShowDamageDirection(Vector3 posDamage)
	{
		if (isDaterRegim)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		Vector3 vector = posDamage - myPlayerTransform.position;
		if (!(vector == Vector3.zero))
		{
			float num = Mathf.Atan(vector.z / vector.x);
			num = num * 180f / (float)Math.PI;
			if (vector.x > 0f)
			{
				num = 90f - num;
			}
			if (vector.x < 0f)
			{
				num = 270f - num;
			}
			float y = myPlayerTransform.rotation.eulerAngles.y;
			float num2 = num - y;
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			if (num2 < -180f)
			{
				num2 += 360f;
			}
			if (inGameGUI != null)
			{
				inGameGUI.AddDamageTaken(num);
			}
			if (num2 > -45f && num2 <= 45f)
			{
				flag3 = true;
			}
			if (num2 < -45f && num2 >= -135f)
			{
				flag = true;
			}
			if (num2 > 45f && num2 <= 135f)
			{
				flag2 = true;
			}
			if (num2 < -135f || num2 >= 135f)
			{
				flag4 = true;
			}
			if (flag3)
			{
				timerShowUp = maxTimeSetTimerShow;
			}
			if (flag4)
			{
				timerShowDown = maxTimeSetTimerShow;
			}
			if (flag)
			{
				timerShowLeft = maxTimeSetTimerShow;
			}
			if (flag2)
			{
				timerShowRight = maxTimeSetTimerShow;
			}
		}
	}

	private void UpdateKillerInfo(Player_move_c killerPlayerMoveC, int killType)
	{
		SkinName skinName = killerPlayerMoveC.mySkinName;
		_killerInfo.nickname = skinName.NickName;
		if (killerPlayerMoveC.myTable != null)
		{
			NetworkStartTable component = killerPlayerMoveC.myTable.GetComponent<NetworkStartTable>();
			int myRanks = component.myRanks;
			if (myRanks > 0 && myRanks < expController.marks.Length)
			{
				_killerInfo.rankTex = ExperienceController.sharedController.marks[myRanks];
				_killerInfo.rank = myRanks;
			}
			if (component.myClanTexture != null)
			{
				_killerInfo.clanLogoTex = component.myClanTexture;
			}
			_killerInfo.clanName = component.myClanName;
		}
		_killerInfo.weapon = killerPlayerMoveC.currentWeaponForKillCam;
		_killerInfo.skinTex = killerPlayerMoveC._skin;
		_killerInfo.hat = skinName.currentHat;
		_killerInfo.mask = skinName.currentMask;
		_killerInfo.armor = skinName.currentArmor;
		_killerInfo.cape = skinName.currentCape;
		_killerInfo.capeTex = skinName.currentCapeTex;
		_killerInfo.boots = skinName.currentBoots;
		_killerInfo.pet = skinName.currentPet;
		_killerInfo.gadgetSupport = skinName.currentGadgetSupport;
		_killerInfo.gadgetTools = skinName.currentGadgetTools;
		_killerInfo.gadgetTrowing = skinName.currentGadgetThrowing;
		_killerInfo.turretUpgrade = killerPlayerMoveC.turretUpgrade;
		_killerInfo.killerTransform = killerPlayerMoveC.myPlayerTransform;
		_killerInfo.healthValue = Mathf.CeilToInt((killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch > 0f) ? (killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch) : 0f);
		_killerInfo.armorValue = Mathf.CeilToInt(killerPlayerMoveC.armorSynch);
	}

	[PunRPC]
	
	public void imDeath(string _name)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayerMoveC.AddSystemMessage(_name, 1, Color.white);
		}
	}

	public void sendImDeath(string _name)
	{
		if (isInet)
		{
			photonView.RPC("imDeath", PhotonTargets.All, _name);
		}
		_killerInfo.isSuicide = true;
	}

	[PunRPC]
	
	public void KilledPhoton(int idKiller, int _typekill, string weaponName, int _typeWeapon)
	{
		if (_weaponManager == null || _weaponManager.myPlayer == null)
		{
			return;
		}
		string nick = string.Empty;
		string nickName = mySkinName.NickName;
		resurrectionMoveC = null;
		isResurrectionKill = false;
		if (weaponName.Contains("pet_"))
		{
			_typekill = 15;
		}
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (!(Initializer.players[i].mySkinName.pixelView != null) || Initializer.players[i].mySkinName.pixelView.viewID != idKiller)
			{
				continue;
			}
			SkinName skinName = Initializer.players[i].mySkinName;
			Player_move_c player_move_c = Initializer.players[i];
			nick = skinName.NickName;
			if (isMine && Defs.isJetpackEnabled && !mySkinName.character.isGrounded)
			{
				player_move_c.AddScoreDuckHunt();
			}
			if (_weaponManager != null && Initializer.players[i] == _weaponManager.myPlayerMoveC)
			{
				ProfileController.OnGameTotalKills();
				if (ABTestController.useBuffSystem)
				{
					BuffSystem.instance.KillInteraction();
				}
				else
				{
					KillRateCheck.instance.IncrementKills();
				}
				WeaponManager.sharedManager.myNetworkStartTable.IncrementKills();
				if (isRaiderMyPoint)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SendHouseKeeperEvent();
					isRaiderMyPoint = false;
				}
				if (Defs.isJetpackEnabled && !_weaponManager.myPlayerMoveC.mySkinName.character.isGrounded && _typekill != 8)
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deathFromAbove);
				}
				if (player_move_c.isRocketJump && _typekill != 8)
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.rocketJumpKill);
				}
				if (IsPlayerEffectActive(PlayerEffect.blackMark))
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.blackMarked);
				}
				if (_typekill == 13)
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.ricochet);
				}
				if (_typekill == 15)
				{
					AddPetCountSerials(player_move_c);
				}
				if (weaponName.Contains("Weapon"))
				{
					if (_typekill != 8 && _typekill != 10)
					{
						GameObject gameObject = Resources.Load("Weapons/" + weaponName) as GameObject;
						if (gameObject != null && gameObject.GetComponent<WeaponSounds>() != null)
						{
							AddCountSerials(gameObject.GetComponent<WeaponSounds>().categoryNabor - 1, player_move_c);
						}
					}
				}
				else if (GadgetsInfo.info.ContainsKey(weaponName))
				{
					GadgetInfo gadgetInfo = GadgetsInfo.info[weaponName];
					if (gadgetInfo.KillStreakType != PlayerEventScoreController.ScoreEvent.none)
					{
						player_move_c.myScoreController.AddScoreOnEvent(gadgetInfo.KillStreakType);
					}
					AddCountSerials(6, player_move_c);
				}
				if (multiKill > 1)
				{
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyBreakSeries();
					}
					if (multiKill == 2)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill2);
					}
					else if (multiKill == 3)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill3);
					}
					else if (multiKill == 4)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill4);
					}
					else if (multiKill == 5)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill5);
					}
					else if (multiKill < 10)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill6);
					}
					else if (multiKill < 20)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill10);
					}
					else if (multiKill < 50)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill20);
					}
					else
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill50);
					}
					multiKill = 0;
				}
				if (!GameConnect.isFlag)
				{
					player_move_c.ImKill(idKiller, _typekill);
				}
				ShopNGUIController.CategoryNames weaponSlot = (ShopNGUIController.CategoryNames)(-1);
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
				if (byPrefabName != null)
				{
					weaponSlot = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(byPrefabName.Tag);
				}
				TypeKills typeKills = (TypeKills)_typekill;
				if (!NetworkStartTable.LocalOrPasswordRoom())
				{
					QuestMediator.NotifyKillOtherPlayer(GameConnect.gameMode, weaponSlot, typeKills == TypeKills.headshot, false, isPlacemarker, isInvisible);
					QuestMediator.NotifyKillOtherPlayerOnFly(IsPlayerFlying && Defs.isJump, player_move_c.IsPlayerFlying);
				}
				PlayerScoreController playerScoreController = player_move_c.myScoreController;
				int @event;
				switch (_typekill)
				{
				default:
					@event = 9;
					break;
				case 15:
					@event = 96;
					break;
				case 3:
					@event = 61;
					break;
				case 2:
					@event = 10;
					break;
				case 9:
					@event = 8;
					break;
				}
				playerScoreController.AddScoreOnEvent((PlayerEventScoreController.ScoreEvent)@event);
				if (isInvisible)
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.invisibleKill);
				}
				if (isPlacemarker)
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.revenge);
				}
				if (Equals(_weaponManager.myPlayerMoveC.placemarkerMoveC))
				{
					_weaponManager.myPlayerMoveC.placemarkerMoveC = null;
					isPlacemarker = false;
				}
				if (_weaponManager.myPlayerMoveC.isResurrectionKill && Equals(_weaponManager.myPlayerMoveC.resurrectionMoveC))
				{
					player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.hellraiser);
					_weaponManager.myPlayerMoveC.resurrectionMoveC = null;
					_weaponManager.myPlayerMoveC.isResurrectionKill = false;
				}
				if (getLocalHurt)
				{
					getLocalHurt = false;
				}
			}
			if (isMine)
			{
				player_move_c.isPlacemarker = true;
				placemarkerMoveC = player_move_c;
			}
			UpdateKillerInfo(Initializer.players[i], _typekill);
			break;
		}
		RemoveEffect(PlayerEffect.blackMark);
		ImKilled(myPlayerTransform.position, myPlayerTransform.rotation, _typeWeapon);
		if ((bool)_weaponManager && _weaponManager.myPlayerMoveC != null)
		{
			_weaponManager.myPlayerMoveC.AddSystemMessage(nick, _typekill, nickName, Color.white, weaponName);
		}
	}

	public void PetKilled()
	{
		if (!Defs.isMulti || isMine)
		{
			counterPetSerial = 0;
			if (GadgetsOnPetKill != null)
			{
				GadgetsOnPetKill();
			}
		}
	}

	public void AddPetCountSerials(Player_move_c killerPlayerMoveC)
	{
		killerPlayerMoveC.counterPetSerial++;
		switch (killerPlayerMoveC.counterPetSerial)
		{
		case 2:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.tamer);
			break;
		case 3:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.packLeader);
			break;
		case 5:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.kingOfBeasts);
			break;
		case 4:
			break;
		}
	}

	private void AddCountSerials(int categoryNabor, Player_move_c killerPlayerMoveC)
	{
		killerPlayerMoveC.counterSerials[categoryNabor]++;
		switch (killerPlayerMoveC.counterSerials[categoryNabor])
		{
		case 1:
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee);
			}
			break;
		case 2:
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee2);
			}
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.hunter);
			}
			break;
		case 3:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary1);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup1);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee3);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special1);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper1);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium1);
			}
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.gadgetMaster);
			}
			break;
		case 5:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary2);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup2);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee5);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special2);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper2);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium2);
			}
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.gadgetManiac);
			}
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.poacher);
			}
			break;
		case 7:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary3);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup3);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee7);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special3);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper3);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium3);
			}
			break;
		case 9:
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.mechanist);
			}
			break;
		case 10:
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.animalsFear);
			}
			break;
		case 4:
		case 6:
		case 8:
			break;
		}
	}

	public void SendImKilled(WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.angel)
	{
		if (!wasResurrected && Defs.isInet)
		{
			photonView.RPC("ImKilled", PhotonTargets.All, myPlayerTransform.position, myPlayerTransform.rotation, (int)typeDead);
			SendSynhHealth(false);
		}
	}

	
	[PunRPC]
	private void ImKilled(Vector3 pos, Quaternion rot)
	{
		ImKilled(pos, rot, 0);
	}

	
	[PunRPC]
	private void ImKilled(Vector3 pos, Quaternion rot, int _typeDead = 0)
	{
		if (Device.isPixelGunLow)
		{
			_typeDead = 0;
		}
		if (!isStartAngel || GameConnect.isCOOP)
		{
			isStartAngel = true;
			if (Defs.inComingMessagesCounter < 15)
			{
				PlayerDeadController currentParticle = PlayerDeadStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShow(pos, rot, _typeDead, false, _skin);
				}
				if (Defs.isSoundFX)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(deadPlayerSound);
				}
			}
		}
		if (!isMine && getLocalHurt)
		{
			WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killAssist);
			getLocalHurt = false;
		}
	}

	private IEnumerator FlashWhenHit()
	{
		damageShown = true;
		try {
			Color color = damage.GetComponent<UnityEngine.UI.RawImage>().color;
			color.a = 0f;
			damage.GetComponent<UnityEngine.UI.RawImage>().color = color;
		} catch {

		}
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		yield return new WaitForSeconds(0.01f);
		yield return StartCoroutine(Fade(1f, 0f, danageTime, damage));
		damageShown = false;
	}

	private IEnumerator FlashWhenDead()
	{
		damageShown = true;
		try {
			Color color = damage.GetComponent<UnityEngine.UI.RawImage>().color;
			color.a = 0f;
			damage.GetComponent<UnityEngine.UI.RawImage>().color = color;
		} catch {

		}
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		while (isDeadFrame)
		{
			yield return null;
		}
		yield return StartCoroutine(Fade(1f, 0f, danageTime / 3f, damage));
		damageShown = false;
	}

	private void KillCam()
	{
		ProfileController.OnGameDeath();
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.DeathInteraction();
		}
		else
		{
			KillRateCheck.instance.IncrementDeath();
		}
		myNetworkStartTable.IncrementDeath();
		killedInMatch = true;
		StartCoroutine(InGameGUI.sharedInGameGUI.ShowRespawnWindow(_killerInfo, 15f));
	}

	private void SetMapCameraActive(bool active)
	{
		InGameGUI.sharedInGameGUI.SetInterfaceVisible(!active);
		Camera component = Initializer.Instance.tc.GetComponent<Camera>();
		Camera camera = (mySkinName.firstPersonControl.isFirstPersonCamera ? myCamera : mySkinName.firstPersonControl.thirdCamera.GetComponent<Camera>());
		component.gameObject.SetActive(active);
		camera.gameObject.SetActive(!active);
		NickLabelController.currentCamera = (active ? component : camera);
	}

	private void SetNoKilled()
	{
		isKilled = false;
		resetMultyKill();
	}

	private void ChangePositionAfterRespawn()
	{
		myPlayerTransform.position += Vector3.forward * 0.01f;
	}

	public void SetPlayerInSpawnPoint()
	{
		GameObject gameObject = null;
		if (!GameConnect.isDeathEscape || CheckpointController.instance.savedCheckpoint == null)
		{
			zoneCreatePlayer = GameObject.FindGameObjectsWithTag(isCOOP ? "MultyPlayerCreateZoneCOOP" : (isCompany ? ("MultyPlayerCreateZoneCommand" + myCommand) : (GameConnect.isFlag ? ("MultyPlayerCreateZoneFlagCommand" + myCommand) : (GameConnect.isCapturePoints ? ("MultyPlayerCreateZonePointZone" + myCommand) : "MultyPlayerCreateZone"))));
			gameObject = zoneCreatePlayer[UnityEngine.Random.Range(0, zoneCreatePlayer.Length)];
		}
		else
		{
			gameObject = CheckpointController.instance.savedCheckpoint.checkPointRespawnPoint;
		}
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		Quaternion rotation = gameObject.transform.rotation;
		myPlayerTransform.position = position;
		myPlayerTransform.rotation = rotation;
		if (mySkinName.firstPersonControl.thirdCamera != null)
		{
			mySkinName.firstPersonControl.thirdCamera.SetFollowPivot();
			mySkinName.firstPersonControl.thirdCamera.mouseX = rotation.eulerAngles.y;
			mySkinName.firstPersonControl.thirdCamera.mouseY = 15f;
			mySkinName.firstPersonControl.thirdCamera.mouseYSmooth = 15f;
		}
		Vector3 eulerAngles = myCamera.transform.rotation.eulerAngles;
		myCamera.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z);
	}

	public void RespawnPlayer()
	{
		if ((GameConnect.isDeathEscape || GameConnect.isSpeedrun) && mySkinName.thirdPersonAnimation != null)
		{
			mySkinName.thirdPersonAnimation.gameObject.SetActive(true);
		}
		Defs.inRespawnWindow = false;
		respawnedForGUI = true;
		if (Defs.isMulti)
		{
			SetMapCameraActive(false);
		}
		_killerInfo.Reset();
		if (inGameGUI != null)
		{
			inGameGUI.StopAllCircularIndicators();
		}
		if ((bool)myCurrentWeaponSounds && myCurrentWeaponSounds.animationObject != null && myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Reload");
		}
		Func<bool> func = () => _pauser != null && _pauser.paused;
		if (base.transform.parent == null)
		{
			UnityEngine.Debug.Log("transform.parent == null");
			return;
		}
		myPlayerTransform.localScale = new Vector3(1f, 1f, 1f);
		myTransform.localRotation = Quaternion.Euler(Vector3.zero);
		if (GameConnect.isMiniGame && !MiniGamesController.Instance.OnPlayerRespawn())
		{
			return;
		}
		InitiailizeIcnreaseArmorEffectFlags();
		isDeadFrame = false;
		isImmortality = true;
		timerImmortality = maxTimerImmortality;
		SetNoKilled();
		if (_weaponManager.myPlayer == null)
		{
			UnityEngine.Debug.Log("_weaponManager.myPlayer == null");
			return;
		}
		_weaponManager.myPlayerMoveC.mySkinName.camPlayer.transform.parent = _weaponManager.myPlayer.transform;
		if (!func())
		{
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.gameObject.SetActive(true);
				JoystickController.rightJoystick._isFirstFrame = false;
			}
		}
		SetJoysticksActive(true);
		if (JoystickController.rightJoystick != null)
		{
			if (inGameGUI != null)
			{
				inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
		CurHealth = MaxHealth;
		Wear.RenewCurArmor(TierOrRoomTier((ExpController.Instance != null) ? ExpController.Instance.OurTier : (ExpController.LevelsForTiers.Length - 1)));
		CurrentBaseArmor = EffectsController.ArmorBonus;
		if (Defs.isMulti)
		{
			SetPlayerInSpawnPoint();
		}
		else
		{
			Initializer.Instance.SetPlayerInStartPoint(myPlayerTransform.gameObject);
			if (mySkinName.firstPersonControl.thirdCamera != null)
			{
				mySkinName.firstPersonControl.thirdCamera.SetFollowPivot();
				mySkinName.firstPersonControl.thirdCamera.mouseX = myPlayerTransform.rotation.eulerAngles.y;
				mySkinName.firstPersonControl.thirdCamera.mouseY = 15f;
				mySkinName.firstPersonControl.thirdCamera.mouseYSmooth = 15f;
			}
		}
		if (Storager.getInt("GrenadeID") <= 0)
		{
			Storager.setInt("GrenadeID", 1);
		}
		if (myCurrentWeaponSounds != null && myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		Invoke("ChangePositionAfterRespawn", 0.01f);
		foreach (Weapon allAvailablePlayerWeapon in _weaponManager.allAvailablePlayerWeapons)
		{
			allAvailablePlayerWeapon.currentAmmoInClip = allAvailablePlayerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			allAvailablePlayerWeapon.currentAmmoInBackpack = allAvailablePlayerWeapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
		}
		if (WeaponManager.sharedManager != null)
		{
			for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
			{
				WeaponSounds component = (WeaponManager.sharedManager.playerWeapons[i] as Weapon).weaponPrefab.GetComponent<WeaponSounds>();
				if (component != null && (!component.isMelee || component.isShotMelee))
				{
					WeaponManager.sharedManager.ReloadWeaponFromSet(i);
				}
			}
		}
		EffectsController.SlowdownCoeff = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showGrenadeHint && ++respawnCountForTraining == 2)
		{
			try
			{
				if (InGameGadgetSet.CurrentSet.Count == 1)
				{
					HintController.instance.ShowHintByName("use_grenade", 5f);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in showing grenade hint: {0}", ex);
			}
			respawnCountForTraining = 0;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showChangeWeaponHint)
		{
			HintController.instance.ShowHintByName("change_weapon", 5f);
		}
		if (GameConnect.isSpeedrun)
		{
			SpeedrunTrackController.instance.ResetRun();
		}
	}

	public void ResurrectPlayer()
	{
		ResurrectionEvent();
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.reviveEffect.Play();
		}
		if (Defs.isSoundFX)
		{
			myAudioSource.PlayOneShot(resurrectionSound);
		}
		SendPlayerEffect(8);
		Defs.inRespawnWindow = false;
		respawnedForGUI = true;
		Func<bool> func = () => _pauser != null && _pauser.paused;
		if (base.transform.parent == null)
		{
			UnityEngine.Debug.Log("transform.parent == null");
			return;
		}
		myPlayerTransform.localScale = Vector3.one;
		if (deadInCollider)
		{
			myTransform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		}
		InitiailizeIcnreaseArmorEffectFlags();
		isDeadFrame = false;
		isImmortality = true;
		timerImmortality = maxTimerImmortality;
		SetNoKilled();
		if (this.OnMyPlayerResurected != null)
		{
			this.OnMyPlayerResurected();
		}
		if (_weaponManager.myPlayer == null)
		{
			UnityEngine.Debug.Log("_weaponManager.myPlayer == null");
			return;
		}
		_weaponManager.myPlayerMoveC.mySkinName.camPlayer.transform.parent = _weaponManager.myPlayer.transform;
		if (!func())
		{
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.gameObject.SetActive(true);
				JoystickController.rightJoystick._isFirstFrame = false;
			}
		}
		SetJoysticksActive(true);
		if (JoystickController.rightJoystick != null)
		{
			if (inGameGUI != null)
			{
				inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
		CurHealth = MaxHealth;
		if (!deadInCollider)
		{
			myPlayerTransform.position = resurrectionPosition;
		}
		else if (Defs.isMulti)
		{
			SetPlayerInSpawnPoint();
		}
		else
		{
			Initializer.Instance.SetPlayerInStartPoint(myPlayerTransform.gameObject);
		}
		if (myCurrentWeaponSounds != null && myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		if (deadInCollider)
		{
			Invoke("ChangePositionAfterRespawn", 0.01f);
		}
		EffectsController.SlowdownCoeff = 1f;
		deadInCollider = false;
	}

	public void HideChangeWeaponTrainingHint()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showChangeWeaponHint)
		{
			showChangeWeaponHint = false;
			HintController.instance.HideHintByName("change_weapon");
		}
	}

	
	[PunRPC]
	private void CountKillsCommandSynch(int _blue, int _red)
	{
		GlobalGameController.countKillsBlue = _blue;
		GlobalGameController.countKillsRed = _red;
	}

	
	[PunRPC]
	private void plusCountKillsCommand(int _command)
	{
		UnityEngine.Debug.Log("plusCountKillsCommand: " + _command);
		if (_command == 1)
		{
			if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
			{
				_weaponManager.myPlayerMoveC.countKillsCommandBlue++;
			}
			else
			{
				GlobalGameController.countKillsBlue++;
			}
		}
		if (_command == 2)
		{
			if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
			{
				_weaponManager.myPlayerMoveC.countKillsCommandRed++;
			}
			else
			{
				GlobalGameController.countKillsRed++;
			}
		}
	}

	public void addMultyKill()
	{
		multiKill++;
		if (multiKill > 1)
		{
			PlayerEventScoreController.ScoreEvent scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
			if (multiKill > 1 && !NetworkStartTable.LocalOrPasswordRoom())
			{
				QuestMediator.NotifyMakeSeries();
			}
			switch (multiKill)
			{
			case 2:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill2;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 3:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill3;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 4:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill4;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 5:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill5;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 6:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 10:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill10;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 20:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill20;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			case 50:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill50;
				myScoreController.AddScoreOnEvent(scoreEvent);
				break;
			}
			if (Defs.isMulti && Defs.isInet)
			{
				photonView.RPC("ShowMultyKillRPC", PhotonTargets.Others, multiKill);
			}
		}
	}

	[PunRPC]
	
	public void ShowMultyKillRPC(int countMulty)
	{
		multiKill = countMulty;
		if (multiKill > 1)
		{
			PlayerEventScoreController.ScoreEvent scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
			switch (multiKill)
			{
			case 2:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill2;
				break;
			case 3:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill3;
				break;
			case 4:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill4;
				break;
			case 5:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill5;
				break;
			case 6:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
				break;
			case 10:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill10;
				break;
			case 20:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill20;
				break;
			case 50:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill50;
				break;
			}
			string spriteName = PlayerEventScoreController.pictureNameOnEvent[scoreEvent.ToString()];
			killStreakParticles.GetStreak(spriteName);
		}
	}

	public void resetMultyKill()
	{
		multiKill = 0;
		for (int i = 0; i < counterSerials.Length; i++)
		{
			counterSerials[i] = 0;
		}
	}

	public void ImKill(NetworkViewID idKiller, int _typeKill)
	{
		countKills++;
		GlobalGameController.CountKills = countKills;
		CheckRookieKillerAchievement();
		addMultyKill();
		if (isCompany)
		{
			if (myCommand == 1)
			{
				countKillsCommandBlue++;
				if (isInet)
				{
					photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 1);
				}
			}
			if (myCommand == 2)
			{
				countKillsCommandRed++;
				if (isInet)
				{
					photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 2);
				}
			}
		}
		_weaponManager.myNetworkStartTable.CountKills = countKills;
		_weaponManager.myNetworkStartTable.SynhCountKills();
	}

	public void ImKill(int idKiller, int _typeKill)
	{
		if (_typeKill == 8)
		{
			QuestMediator.NotifyTurretKill();
		}
		countKills++;
		GlobalGameController.CountKills = countKills;
		CheckRookieKillerAchievement();
		if (_typeKill != 15)
		{
			addMultyKill();
		}
		if (isCompany)
		{
			if (myCommand == 1)
			{
				countKillsCommandBlue++;
				if (isInet)
				{
					photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 1);
				}
			}
			if (myCommand == 2)
			{
				countKillsCommandRed++;
				if (isInet)
				{
					photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 2);
				}
			}
		}
		_weaponManager.myNetworkStartTable.CountKills = countKills;
		_weaponManager.myNetworkStartTable.SynhCountKills();
	}

	private void CheckRookieKillerAchievement()
	{
		int num = oldKilledPlayerCharactersCount + 1;
		if (num <= 15)
		{
			Storager.setInt("KilledPlayerCharactersCount", num);
		}
		oldKilledPlayerCharactersCount = num;
		if (!Social.localUser.authenticated || Storager.hasKey("RookieKillerAchievmentCompleted") || num < 15)
		{
			return;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQBw", 1, delegate(bool success)
			{
				UnityEngine.Debug.Log("Achievement Rookie Killer incremented: " + success);
			});
		}
		Storager.setInt("RookieKillerAchievmentCompleted", 1);
	}

	public void AddScoreDuckHunt()
	{
		if (Defs.isInet)
		{
			photonView.RPC("AddScoreDuckHuntRPC", PhotonTargets.All);
		}
	}

	[PunRPC]
	
	public void AddScoreDuckHuntRPC()
	{
		if (isMine)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.duckHunt);
		}
	}

	[PunRPC]
	
	public void pobeda(NetworkViewID idKiller)
	{
		foreach (Player_move_c player in Initializer.players)
		{
			Player_move_c player_move_c = player;
		}
		if ((bool)_weaponManager && (bool)_weaponManager.myTable)
		{
			_weaponManager.myNetworkStartTable.win(nickPobeditel);
		}
	}

	public void SendMySpotEvent()
	{
		countMySpotEvent++;
		if (countMySpotEvent == 1)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.mySpotPoint);
		}
		if (countMySpotEvent == 2)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.unstoppablePoint);
		}
		if (countMySpotEvent >= 3)
		{
			myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.monopolyPoint);
		}
	}

	private void ResetMySpotEvent()
	{
		countMySpotEvent = 0;
	}

	private void ProvideHealth(string inShopId)
	{
		CurHealth = MaxHealth;
		CurrentCampaignGame.withoutHits = true;
	}

	public Vector3 GetPointAutoAim(Vector3 _posTo)
	{
		if (timerUpdatePointAutoAi < 0f)
		{
			rayAutoAim = myCamera.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(rayAutoAim, out hitInfo, 300f, Tools.AllWithoutDamageCollidersMaskAndWithoutRocket))
			{
				if (hitInfo.collider.gameObject.name.Equals("Rocket(Clone)"))
				{
					UnityEngine.Debug.Log("Rocket(Clone)");
				}
				pointAutoAim = hitInfo.point;
			}
			else
			{
				pointAutoAim = Vector3.down * 10000f;
			}
			timerUpdatePointAutoAi = 0.2f;
		}
		if (pointAutoAim.y < -1000f)
		{
			return rayAutoAim.GetPoint(Vector3.Magnitude(myCamera.transform.position - _posTo) * 1.1f);
		}
		return pointAutoAim;
	}

	private void ShootUpdate()
	{
		for (int i = 0; i < poisonTargets.Count; i++)
		{
			if (poisonTargets[i].target.Equals(null) || poisonTargets[i].target.IsDead() || poisonTargets[i].hitCount <= 0)
			{
				poisonTargets.RemoveAt(i);
				i--;
			}
			else if (poisonTargets[i].nextHitTime < Time.time)
			{
				poisonTargets[i].hitCount--;
				ApplyDamageToTarget(poisonTargets[i].target, poisonTargets[i].param.multiplayerDamage, poisonTargets[i].param.weaponName, (poisonTargets[i].param.poisonType == PoisonType.Burn) ? WeaponSounds.TypeDead.explosion : poisonTargets[i].param.typeDead, (poisonTargets[i].param.poisonType == PoisonType.Toxic) ? TypeKills.poison : ((poisonTargets[i].param.poisonType == PoisonType.Burn) ? TypeKills.burning : ((poisonTargets[i].param.poisonType == PoisonType.Bleeding) ? TypeKills.bleeding : TypeKills.none)));
				poisonTargets[i].nextHitTime = Time.time + poisonTargets[i].param.poisonTime;
			}
		}
		bool flag = isShooting;
		isShooting = JoystickController.rightJoystick.isShooting || JoystickController.rightJoystick.isShootingPressure || JoystickController.leftTouchPad.isShooting;
		bool flag2 = !isShooting && flag;
		bool flag3 = TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled;
		if (isShooting)
		{
			if (flag3 && (!GameConnect.isMiniGame || MiniGamesController.Instance.isGo) && !myCurrentWeaponSounds.isGrenadeWeapon)
			{
				ShotPressed();
			}
		}
		else
		{
			if (flag2)
			{
				ShotUnPressed();
			}
			ResetShootingBurst();
		}
		if (myCurrentWeaponSounds.isFrostSword)
		{
			FrostSwordUpdate(myCurrentWeaponSounds);
		}
	}

	public void ShotUnPressed(bool weaponChanged = false)
	{
		if (_weaponManager.currentWeaponSounds.isLoopShoot && isShootingLoop)
		{
			StopLoopShot();
		}
		if (_weaponManager.currentWeaponSounds.isCharging)
		{
			UnchargeGun(weaponChanged);
		}
	}

	private void UnchargeGun(bool weaponChanged)
	{
		fullyCharged = false;
		GetComponent<AudioSource>().Stop();
		inGameGUI.ChargeValue.gameObject.SetActive(false);
		if (_weaponManager.currentWeaponSounds.invisWhenCharged)
		{
			SetInvisible(false, true);
		}
		if (!(chargeValue > 0f))
		{
			return;
		}
		lastChargeValue = chargeValue;
		Weapon weapon = (Weapon)_weaponManager.playerWeapons[lastChargeWeaponIndex];
		if (!weaponChanged)
		{
			_Shot();
		}
		else
		{
			if (_weaponManager.currentWeaponSounds.isMelee)
			{
				weapon.currentAmmoInClip = ammoInClipBeforeCharge;
			}
			Animation animation = (isMechActive ? mechGunAnimation : _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>());
			if (animation != null && animation.IsPlaying("Charge"))
			{
				animation.Stop();
				animation.Play("Idle");
			}
			GetComponent<AudioSource>().clip = null;
		}
		if (isMulti && isMine && isInet)
		{
			photonView.RPC("ChargeGunAnimation", PhotonTargets.Others, false);
		}
		UnityEngine.Debug.Log("Charge release: " + chargeValue);
		chargeValue = 0f;
	}

	public void StartLoopShot()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		if (isMulti && isMine && isInet)
		{
			photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, true);
		}
		isShootingLoop = true;
		Animation component = myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		float lengthAnimShootDown = 0f;
		if (component.GetClip("Shoot_start") != null)
		{
			if (component.IsPlaying("Shoot_end"))
			{
				lengthAnimShootDown = component["Shoot_end"].length - component["Shoot_end"].time;
			}
			else
			{
				component.Stop();
				lengthAnimShootDown = component["Shoot_start"].length;
				component.Play("Shoot_start");
			}
		}
		else
		{
			component.Stop();
		}
		ctsShootLoop.Cancel();
		ctsShootLoop = new CancellationTokenSource();
		StartCoroutine(ShootLoop(ctsShootLoop.Token, lengthAnimShootDown));
	}

	private void StopLoopShot()
	{
		if (isMulti && isMine && isInet)
		{
			photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, false);
		}
		isShootingLoop = false;
		ctsShootLoop.Cancel();
		Animation component = myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		if (component.IsPlaying("Shoot"))
		{
			component.Stop();
		}
		else if (component.IsPlaying("Shoot_start"))
		{
			float length = component["Shoot_start"].length;
			float time = component["Shoot_start"].time;
			component.Stop();
			component["Shoot_end"].time = component["Shoot_start"].length - component["Shoot_start"].time;
		}
		if (component["Shoot_end"] != null)
		{
			component.Play("Shoot_end");
		}
		if (Defs.isSoundFX)
		{
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().clip = myCurrentWeaponSounds.idle;
			myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
	}

	private IEnumerator ShootLoop(CancellationToken token, float _lengthAnimShootDown)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		yield return new WaitForSeconds(_lengthAnimShootDown);
		Animation component = myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		float lengthAnimShoot = component["Shoot"].length;
		if (!token.IsCancellationRequested)
		{
			component.Play("Shoot");
			if (Defs.isSoundFX)
			{
				myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().clip = myCurrentWeaponSounds.shoot;
				myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
			}
		}
		if (!Defs.isMulti || isMine)
		{
			while (!token.IsCancellationRequested)
			{
				shootS();
				yield return new WaitForSeconds(lengthAnimShoot);
			}
		}
	}

	[PunRPC]
	
	private void StartShootLoopRPC(bool isStart)
	{
		if (isStart && !isShootingLoop)
		{
			StartLoopShot();
		}
		if (!isStart && isShootingLoop)
		{
			StopLoopShot();
		}
	}

	public void ResetShootingBurst()
	{
		_countShootInBurst = 0;
		_timerDelayInShootingBurst = -1f;
	}

	public void ShotPressed()
	{
		if (GameConnect.isSpeedrun || GameConnect.isDeathEscape || deltaAngle > 10f)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			TrainingController.isNextStep = TrainingState.TapToShoot;
		}
		if ((isMulti && isInet && (bool)photonView && !photonView.isMine) || _weaponManager == null || _weaponManager.currentWeaponSounds == null || _weaponManager.currentWeaponSounds.animationObject == null || _weaponManager.currentWeaponSounds.name.Contains("WeaponGrenade") || Defs.isTurretWeapon)
		{
			return;
		}
		if (!isMechActive && _weaponManager.currentWeaponSounds.isLoopShoot)
		{
			if (!isShootingLoop)
			{
				StartLoopShot();
			}
			return;
		}
		Animation animation = (isMechActive ? mechGunAnimation : _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>());
		Weapon weaponByIndex = GetWeaponByIndex(_weaponManager.CurrentWeaponIndex);
		if (animation.IsPlaying("Shoot1") || animation.IsPlaying("Shoot2") || animation.IsPlaying("Shoot") || animation.IsPlaying("Reload") || isReloading || animation.IsPlaying("Empty") || _timerDelayInShootingBurst > 0f)
		{
			return;
		}
		lastShotTime = Time.time;
		if (!isMechActive && _weaponManager.currentWeaponSounds.isCharging)
		{
			if ((weaponByIndex.currentAmmoInClip > 0 || _weaponManager.currentWeaponSounds.isMelee) && chargeValue < 1f)
			{
				if (chargeValue == 0f)
				{
					ammoInClipBeforeCharge = weaponByIndex.currentAmmoInClip;
					lastChargeWeaponIndex = _weaponManager.CurrentWeaponIndex;
					firstChargePlay = false;
				}
				if (nextChargeConsumeTime < Time.time)
				{
					nextChargeConsumeTime = Time.time + _weaponManager.currentWeaponSounds.chargeTime / (float)_weaponManager.currentWeaponSounds.chargeMax;
					chargeValue = Math.Min(1f, chargeValue + 1f / (float)_weaponManager.currentWeaponSounds.chargeMax);
					animation["Charge"].speed = 1f;
					if (!_weaponManager.currentWeaponSounds.isMelee)
					{
						weaponByIndex.currentAmmoInClip--;
					}
					if (inGameGUI != null)
					{
						inGameGUI.ChargeValue.gameObject.SetActive(true);
						inGameGUI.ChargeValue.fillAmount = chargeValue;
						inGameGUI.ChargeValue.color = new Color(1f, 1f - chargeValue, 0f);
					}
					if (!fullyCharged && chargeValue == 1f)
					{
						fullyCharged = true;
						if (_weaponManager.currentWeaponSounds.invisWhenCharged)
						{
							SetInvisible(true, true);
						}
					}
				}
			}
			else
			{
				if (!_weaponManager.currentWeaponSounds.chargeLoop)
				{
					animation["Charge"].speed = 0f;
				}
				if (chargeValue == 0f)
				{
					ShowNoAmmo();
				}
			}
			if (!(chargeValue > 0f))
			{
				return;
			}
			if (!animation.IsPlaying("Charge") && animation.GetClip("Charge") != null)
			{
				animation.Stop();
				animation.Play("Charge");
				if (!firstChargePlay)
				{
					firstChargePlay = true;
					if (isMulti && isMine && isInet)
					{
						photonView.RPC("ChargeGunAnimation", PhotonTargets.Others, true);
					}
				}
			}
			if (Defs.isSoundFX && _weaponManager.currentWeaponSounds.charge != null && (!GetComponent<AudioSource>().isPlaying || GetComponent<AudioSource>().clip != _weaponManager.currentWeaponSounds.charge))
			{
				GetComponent<AudioSource>().clip = _weaponManager.currentWeaponSounds.charge;
				GetComponent<AudioSource>().Play();
			}
			return;
		}
		animation.Stop();
		if (_weaponManager.currentWeaponSounds.isBurstShooting)
		{
			_countShootInBurst++;
			if (_countShootInBurst >= _weaponManager.currentWeaponSounds.countShootInBurst)
			{
				_timerDelayInShootingBurst = _weaponManager.currentWeaponSounds.delayInBurstShooting;
				_countShootInBurst = 0;
			}
		}
		if (_weaponManager.currentWeaponSounds.isMelee && !_weaponManager.currentWeaponSounds.isShotMelee && !isMechActive)
		{
			_Shot();
		}
		else if (weaponByIndex.currentAmmoInClip > 0 || isMechActive)
		{
			if (!isMechActive)
			{
				weaponByIndex.currentAmmoInClip--;
				if (weaponByIndex.currentAmmoInClip == 0)
				{
					if (weaponByIndex.currentAmmoInBackpack > 0)
					{
						if (_weaponManager.currentWeaponSounds.isShotMelee)
						{
							Reload();
						}
					}
					else
					{
						TouchPadController rightJoystick = JoystickController.rightJoystick;
						if ((bool)rightJoystick)
						{
							rightJoystick.NoAmmo();
						}
						if (inGameGUI != null)
						{
							inGameGUI.BlinkNoAmmo(3);
							inGameGUI.PlayLowResourceBeep(3);
						}
					}
				}
			}
			_Shot();
		}
		else
		{
			ShowNoAmmo();
		}
	}

	private void ShowNoAmmo()
	{
		Weapon weaponByIndex = GetWeaponByIndex(_weaponManager.CurrentWeaponIndex);
		if (inGameGUI != null)
		{
			inGameGUI.BlinkNoAmmo(1);
			if (weaponByIndex.currentAmmoInBackpack == 0)
			{
				inGameGUI.PlayLowResourceBeepIfNotPlaying(1);
			}
		}
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			if (!isMechActive && weaponByIndex.currentAmmoInBackpack <= 0 && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && showChangeWeaponHint)
			{
				HintController.instance.ShowHintByName("change_weapon", 2f);
			}
			_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Empty");
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.empty);
			}
		}
	}

	private void _Shot()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowFire = 1000f;
			HintController.instance.HideHintByName("press_fire");
		}
		if (isGrenadePress || showChat)
		{
			return;
		}
		if (Defs.isMulti && !GameConnect.isCOOP)
		{
			ProfileController.OnGameShoot();
		}
		float num = 0f;
		if (isMechActive)
		{
			if (!mechWeaponSounds.isDoubleShot)
			{
				mechGunAnimation.Play("Shoot");
				num = mechGunAnimation["Shoot"].length;
			}
			else
			{
				int numShootInDouble = GetNumShootInDouble();
				mechGunAnimation.Play("Shoot" + numShootInDouble);
				num = mechGunAnimation["Shoot" + numShootInDouble].length;
			}
			if (Defs.isSoundFX && currentMech != null)
			{
				GetComponent<AudioSource>().PlayOneShot(currentMech.shootSound);
			}
		}
		else
		{
			if (!_weaponManager.currentWeaponSounds.isDoubleShot)
			{
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
				num = _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].length;
			}
			else
			{
				int numShootInDouble2 = GetNumShootInDouble();
				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot" + numShootInDouble2);
				num = _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot" + numShootInDouble2].length;
			}
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.shoot);
			}
		}
		if (inGameGUI != null)
		{
			inGameGUI.StartFireCircularIndicators(num);
		}
		shootS();
	}

	public RayHitsInfo GetHitsFromRay(Ray ray, bool getAll = true)
	{
		RayHitsInfo result = default(RayHitsInfo);
		result.obstacleFound = false;
		result.lenRay = 150f;
		RaycastHit[] array = Physics.RaycastAll(ray, 150f, _ShootRaycastLayerMask);
		if (array == null || array.Length == 0)
		{
			array = new RaycastHit[0];
		}
		if (!getAll)
		{
			Array.Sort(array, delegate(RaycastHit hit1, RaycastHit hit2)
			{
				float num = (hit1.point - GunFlash.position).sqrMagnitude - (hit2.point - GunFlash.position).sqrMagnitude;
				return (num > 0f) ? 1 : ((num != 0f) ? (-1) : 0);
			});
			List<RaycastHit> list = new List<RaycastHit>();
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit item = array2[i];
				bool flag = false;
				if (isHunger && item.collider.gameObject != null && item.collider.gameObject.CompareTag("Chest"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.parent != null && item.collider.gameObject.transform.parent.CompareTag("Enemy"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.parent != null && item.collider.gameObject.transform.parent.CompareTag("Player"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.root != null && item.collider.gameObject.transform.root.CompareTag("Pet"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject != null && item.collider.gameObject.CompareTag("Turret"))
				{
					list.Add(item);
					if (item.collider.gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
					{
						flag = true;
					}
				}
				else if (item.collider.gameObject != null && item.collider.gameObject.CompareTag("DamagedExplosion"))
				{
					list.Add(item);
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					result.obstacleFound = true;
					Vector3 point = item.point;
					Vector3 a = point - ray.origin;
					result.lenRay = Vector3.Magnitude(a);
					result.rayReflect = new Ray(point, Vector3.Reflect(ray.direction, item.normal));
					break;
				}
			}
			result.hits = list.ToArray();
		}
		else
		{
			result.hits = array;
		}
		return result;
	}

	private IEnumerator ShowRayWithDelay(Vector3 _origin, Vector3 _direction, string _railName, float _len, float _delay)
	{
		yield return new WaitForSeconds(_delay);
		WeaponManager.AddRay(_origin, _direction, _railName, _len);
	}

	private void FrostSwordUpdate(WeaponSounds weapon)
	{
		if (!(nextFrostHitTime < Time.time))
		{
			return;
		}
		nextFrostHitTime = Time.time + weapon.slowdownTime;
		float num = weapon.frostRadius * weapon.frostRadius;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if ((item.position - _player.transform.position).sqrMagnitude < num)
			{
				DamageTargetWithWeapon(weapon, item.gameObject, false, 0f, weapon.frostDamageMultiplier);
			}
		}
	}

	private void SnowStormShot(WeaponSounds weapon)
	{
		SendFireFlash();
		ShowGunFlash();
		GameObject gameObject = null;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, weapon.range, _ShootRaycastLayerMask) && hitInfo.collider.gameObject != null)
		{
			gameObject = ((!hitInfo.transform.parent || (!hitInfo.transform.parent.CompareTag("Enemy") && !hitInfo.transform.parent.CompareTag("Player"))) ? hitInfo.transform.gameObject : hitInfo.transform.parent.gameObject);
			DamageTargetWithWeapon(weapon, gameObject, false, hitInfo.distance * hitInfo.distance);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if (!(gameObject == item))
			{
				Vector3 to = item.position - _player.transform.position;
				float sqrMagnitude = to.sqrMagnitude;
				if (sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					DamageTargetWithWeapon(weapon, item.gameObject, false, sqrMagnitude);
				}
			}
		}
	}

	private void FlamethrowerShot(WeaponSounds weapon)
	{
		SendFireFlash();
		ShowGunFlash();
		GameObject gameObject = null;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, weapon.range, _ShootRaycastLayerMask) && hitInfo.collider.gameObject != null)
		{
			gameObject = ((!hitInfo.transform.parent || (!hitInfo.transform.parent.CompareTag("Enemy") && !hitInfo.transform.parent.CompareTag("Player"))) ? hitInfo.transform.gameObject : hitInfo.transform.parent.gameObject);
			DamageTargetWithWeapon(weapon, gameObject);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if (!(gameObject == item))
			{
				Vector3 to = item.position - _player.transform.position;
				if (to.sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					DamageTargetWithWeapon(weapon, item.gameObject);
				}
			}
		}
	}

	private void ShockerShot(WeaponSounds weapon)
	{
		SendFireFlash();
		ShowGunFlash();
		GameObject gameObject = null;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, weapon.range, _ShootRaycastLayerMask) && hitInfo.collider.gameObject != null)
		{
			gameObject = ((!hitInfo.transform.parent || (!hitInfo.transform.parent.CompareTag("Enemy") && !hitInfo.transform.parent.CompareTag("Player") && !hitInfo.transform.parent.CompareTag("Pet"))) ? hitInfo.transform.gameObject : hitInfo.transform.root.gameObject);
			DamageTargetWithWeapon(weapon, gameObject);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		float num2 = weapon.shockerRange * weapon.shockerRange;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if (!(gameObject == item))
			{
				Vector3 to = item.position - _player.transform.position;
				if (to.sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					DamageTargetWithWeapon(weapon, item.gameObject);
				}
				if (to.sqrMagnitude < num2)
				{
					DamageTargetWithWeapon(weapon, item.gameObject, false, 0f, weapon.shockerDamageMultiplier);
				}
			}
		}
	}

	private IEnumerator MeleeShot(WeaponSounds weapon)
	{
		SendFireFlash(false, weapon.isDoubleShot ? numShootInDoubleShot : 0);
		yield return new WaitForSeconds(TimeOfMeleeAttack(weapon));
		if (weapon == null)
		{
			yield break;
		}
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		float num = float.MaxValue;
		bool isHeadshot = false;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, weapon.range, _ShootRaycastLayerMask) && hitInfo.collider.gameObject != null)
		{
			if ((bool)hitInfo.transform.parent)
			{
				gameObject = hitInfo.transform.gameObject;
				if (hitInfo.transform.parent.CompareTag("Enemy"))
				{
					gameObject = hitInfo.transform.parent.gameObject;
					isHeadshot = hitInfo.collider.GetType() == typeof(SphereCollider);
				}
				if (hitInfo.transform.parent.CompareTag("Player") || hitInfo.transform.parent.CompareTag("Dummy"))
				{
					gameObject = hitInfo.transform.parent.gameObject;
					isHeadshot = hitInfo.transform.name == "HeadCollider";
				}
			}
			else
			{
				gameObject = hitInfo.transform.gameObject;
				isHeadshot = hitInfo.transform.name == "HeadCollider";
			}
			gameObject2 = gameObject;
			num = 0f;
		}
		float num2 = weapon.range * weapon.range;
		foreach (Transform item in new Initializer.TargetsList())
		{
			if (!(gameObject == item))
			{
				Vector3 to = item.position - _player.transform.position;
				float sqrMagnitude = to.sqrMagnitude;
				if (sqrMagnitude < num && sqrMagnitude < num2 && Vector3.Angle(this.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					num = sqrMagnitude;
					gameObject2 = item.gameObject;
				}
			}
		}
		if (gameObject2 != null)
		{
			DamageTargetWithWeapon(weapon, gameObject2, isHeadshot);
		}
	}

	private void RailgunShot(WeaponSounds weapon)
	{
		weapon.fire();
		SendFireFlash(true, weapon.isDoubleShot ? numShootInDoubleShot : 0);
		ShowGunFlash();
		float num = weapon.tekKoof * Defs.Coef;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * num) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * num)), ((float)Screen.height - weapon.startZone.y * num) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * num)), 0f));
		if (weapon.freezer)
		{
			RayHitsInfo hitsFromRay = GetHitsFromRay(ray, false);
			RaycastHit[] hits = hitsFromRay.hits;
			foreach (RaycastHit hit in hits)
			{
				_DoHit(weapon, hit, true);
			}
			AddFreezerRayWithLength(hitsFromRay.lenRay);
			if (isMulti && isInet)
			{
				photonView.RPC("AddFreezerRayWithLength", PhotonTargets.Others, hitsFromRay.lenRay);
			}
			return;
		}
		bool flag = false;
		int num2 = 0;
		do
		{
			RayHitsInfo hitsFromRay2 = GetHitsFromRay(ray, weapon.countReflectionRay == 1);
			RaycastHit[] hits = hitsFromRay2.hits;
			foreach (RaycastHit hit2 in hits)
			{
				_DoHit(weapon, hit2);
			}
			bool num3 = num2 == 0 && (weapon.countReflectionRay == 1 || !hitsFromRay2.obstacleFound);
			Vector3 origin = ((num2 == 0) ? GunFlash.gameObject.transform.parent.position : ray.origin);
			Vector3 direction = (num3 ? GunFlash.gameObject.transform.parent.parent.forward : ((num2 == 0) ? (hitsFromRay2.rayReflect.origin - GunFlash.gameObject.transform.parent.position) : ray.direction));
			float len = (num3 ? 150f : ((num2 == 0) ? (hitsFromRay2.rayReflect.origin - GunFlash.gameObject.transform.parent.position).magnitude : hitsFromRay2.lenRay));
			StartCoroutine(ShowRayWithDelay(origin, direction, weapon.railName, len, (float)num2 * 0.05f));
			if (hitsFromRay2.obstacleFound)
			{
				ray = hitsFromRay2.rayReflect;
				flag = true;
			}
			num2++;
		}
		while (flag && num2 < weapon.countReflectionRay);
	}

	private void BulletShot(WeaponSounds weapon)
	{
		ShowGunFlash();
		int num = ((!weapon.isShotGun) ? 1 : weapon.countShots);
		float maxDistance = (weapon.isShotGun ? 30f : 100f);
		Vector3[] array = null;
		Quaternion[] array2 = null;
		bool[] array3 = null;
		int num2 = Mathf.Min(7, num);
		bool flag = false;
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		if (weapon.bulletExplode)
		{
			maxDistance = 250f;
		}
		for (int i = 0; i < num; i++)
		{
			float num3 = weapon.tekKoof * Defs.Coef;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * num3) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * num3)), ((float)Screen.height - weapon.startZone.y * num3) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * num3)), 0f));
			if ((weapon.isDoubleShot ? weapon.gunFlashDouble[numShootInDoubleShot - 1] : GunFlash) != null && !GameConnect.isDaterRegim)
			{
				GameObject currentBullet = BulletStackController.sharedController.GetCurrentBullet((int)weapon.typeTracer);
				if (currentBullet != null)
				{
					currentBullet.transform.rotation = myTransform.rotation;
					Bullet component = currentBullet.GetComponent<Bullet>();
					component.endPos = ray.GetPoint(200f);
					component.startPos = (weapon.isDoubleShot ? weapon.gunFlashDouble[numShootInDoubleShot - 1].position : GunFlash.position);
					component.StartBullet();
				}
				weapon.fire();
			}
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, maxDistance, _ShootRaycastLayerMask))
			{
				continue;
			}
			if (!weapon.bulletExplode)
			{
				if (!hitInfo.collider.gameObject.transform.CompareTag("DamagedExplosion") && !hitInfo.collider.gameObject.name.Equals("StopCollider"))
				{
					zero = hitInfo.point + hitInfo.normal * 0.001f;
					identity = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
					Transform root = hitInfo.collider.gameObject.transform.root;
					flag = ((root.CompareTag("Enemy") || root.CompareTag("Player") || root.CompareTag("Pet") || root.CompareTag("SpleefBlockItem")) ? true : false);
					HoleRPC(flag, zero, identity);
					if (isMulti && isInet && num2 > 1 && i < num2)
					{
						if (array == null)
						{
							array = new Vector3[num2];
							array2 = new Quaternion[num2];
							array3 = new bool[num2];
						}
						array[i] = zero;
						array2[i] = identity;
						array3[i] = flag;
					}
				}
				_DoHit(weapon, hitInfo);
			}
			else
			{
				Rocket rocket = CreateRocket(myCurrentWeaponSounds, hitInfo.point, Quaternion.identity);
				rocket.StartCoroutine(rocket.KillRocket(hitInfo.collider));
			}
		}
		SendFireFlash(true, weapon.isDoubleShot ? numShootInDoubleShot : 0);
	}

	public void PoisonShotWithEffect(GameObject target, float damageMultiplayer, WeaponSounds weapon)
	{
		PoisonShotWithEffect(target, new PoisonParameters(damageMultiplayer, weapon));
	}

	public void PoisonShotWithEffect(GameObject target, PoisonParameters param)
	{
		if (GameConnect.isDaterRegim)
		{
			return;
		}
		switch (target.tag)
		{
		case "Enemy":
			if (param.poisonType == PoisonType.Burn)
			{
				AdvancedEffects component4 = target.GetComponent<AdvancedEffects>();
				if (component4 != null)
				{
					component4.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
				}
			}
			PoisonShot(target, param);
			break;
		case "Player":
			if (param.poisonType == PoisonType.Burn)
			{
				target.GetComponent<SkinName>().playerMoveC.SendPlayerEffect(1, param.poisonTime * (float)param.poisonCount);
			}
			PoisonShot(target, param);
			break;
		case "Turret":
		{
			TurretController component2 = target.GetComponent<TurretController>();
			if (!(component2 != null) || ((param.poisonType != PoisonType.Burn || !component2.isEnemyTurret) && !(component2 is VoodooSnowman)) || !component2.isRun)
			{
				break;
			}
			if (param.poisonType == PoisonType.Burn)
			{
				AdvancedEffects component3 = target.GetComponent<AdvancedEffects>();
				if (component3 != null)
				{
					component3.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
				}
			}
			PoisonShot(target, param);
			break;
		}
		case "DamagedExplosion":
			if (param.poisonType == PoisonType.Burn)
			{
				PoisonShot(target, param);
			}
			break;
		case "Pet":
			if (param.poisonType == PoisonType.Burn)
			{
				AdvancedEffects component = target.GetComponent<AdvancedEffects>();
				if (component != null)
				{
					component.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
				}
			}
			PoisonShot(target, param);
			break;
		}
	}

	public void CharmTarget(GameObject target, float time)
	{
		if (target.tag.Equals("Player"))
		{
			Player_move_c playerMoveC = target.GetComponent<SkinName>().playerMoveC;
			if (Defs.isInet && !playerMoveC.photonView.owner.Equals(PhotonNetwork.player))
			{
				playerMoveC.SendPlayerEffect(playerMoveC.photonView.owner, 14, time, skinNamePixelView.viewID);
			}
			playerMoveC.PlayerEffectWithSenderRPC(14, time, skinNamePixelView.viewID);
		}
		else
		{
			if (!target.tag.Equals("Enemy") && !target.tag.Equals("Turret") && !target.tag.Equals("Pet"))
			{
				return;
			}
			AdvancedEffects component = target.GetComponent<AdvancedEffects>();
			if (!(component != null))
			{
				return;
			}
			if (Defs.isMulti && Defs.isInet)
			{
				PhotonPlayer photonPlayer = null;
				if (target.tag.Equals("Enemy"))
				{
					photonPlayer = PhotonNetwork.masterClient;
				}
				else
				{
					PhotonView component2 = component.GetComponent<PhotonView>();
					if (component2 != null)
					{
						photonPlayer = component2.owner;
					}
				}
				if (photonPlayer != null && !photonPlayer.Equals(PhotonNetwork.player))
				{
					component.SendAdvancedEffect(photonPlayer, 2, time, skinNamePixelView.viewID);
				}
			}
			component.AdvancedEffectWithSenderRPC(2, time, skinNamePixelView.viewID);
		}
	}

	public void PoisonShot(GameObject target, PoisonParameters poison)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(this))
		{
			return;
		}
		for (int i = 0; i < poisonTargets.Count; i++)
		{
			if (poisonTargets[i].target.Equals(component) && poisonTargets[i].param.poisonType == poison.poisonType)
			{
				poisonTargets[i].UpdatePoison(poison);
				return;
			}
		}
		PoisonTarget item = new PoisonTarget(component, poison);
		poisonTargets.Add(item);
	}

	public void shootS()
	{
		if (!isGrenadePress)
		{
			WeaponSounds weaponSounds = (isMechActive ? mechWeaponSounds : _weaponManager.currentWeaponSounds);
			if (weaponSounds.bazooka)
			{
				StartCoroutine("BazookaShoot");
			}
			else if (weaponSounds.railgun || weaponSounds.freezer)
			{
				RailgunShot(weaponSounds);
			}
			else if (weaponSounds.flamethrower)
			{
				FlamethrowerShot(weaponSounds);
			}
			else if (weaponSounds.snowStorm)
			{
				SnowStormShot(weaponSounds);
			}
			else if (weaponSounds.shocker)
			{
				ShockerShot(weaponSounds);
			}
			else if (weaponSounds.isRoundMelee)
			{
				StartCoroutine(HitRoundMelee(weaponSounds));
			}
			else if (weaponSounds.isMelee)
			{
				StartCoroutine(MeleeShot(weaponSounds));
			}
			else
			{
				BulletShot(weaponSounds);
			}
		}
	}

	public void GrenadePress(ThrowGadget gadget)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			showGrenadeHint = false;
			HintController.instance.HideHintByName("use_grenade");
		}
		if (indexWeapon == 1001)
		{
			return;
		}
		currentGrenadeGadget = gadget;
		isGrenadePress = true;
		currentWeaponBeforeGrenade = WeaponManager.sharedManager.CurrentWeaponIndex;
		ChangeWeapon(1000, false);
		timeGrenadePress = Time.realtimeSinceStartup;
		if (inGameGUI != null && inGameGUI.blockedCollider != null)
		{
			inGameGUI.blockedCollider.SetActive(true);
		}
		if (inGameGUI != null && inGameGUI.blockedCollider2 != null)
		{
			inGameGUI.blockedCollider2.SetActive(true);
		}
		if (inGameGUI != null && inGameGUI.blockedColliderDater != null)
		{
			inGameGUI.blockedColliderDater.SetActive(true);
		}
		if (inGameGUI != null)
		{
			for (int i = 0; i < inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = false;
			}
			for (int j = 0; j < inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = false;
			}
		}
	}

	public void GrenadeFire()
	{
		if (isGrenadePress)
		{
			float num = Time.realtimeSinceStartup - timeGrenadePress;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
			{
				TrainingController.isNextStep = TrainingState.TapToThrowGrenade;
			}
			Defs.isGrenateFireEnable = false;
			float num2 = 0.4f;
			if (indexWeapon == 1000)
			{
				num2 = (Resources.Load("GadgetsContent/" + currentGrenadeGadget.GrenadeGadgetId) as GameObject).GetComponent<WeaponSounds>().grenadeUseTime;
			}
			if (num - num2 > 0f)
			{
				GrenadeStartFire();
			}
			else
			{
				Invoke("GrenadeStartFire", num2 - num);
			}
			if (currentGrenadeGadget != null)
			{
				currentGrenadeGadget.ShowThrowingEffect(num2 - num);
			}
		}
	}

	public void GrenadeStartFire()
	{
		if (isMulti)
		{
			if (isInet)
			{
				photonView.RPC("fireFlash", PhotonTargets.All, false, 0);
			}
		}
		else
		{
			fireFlash(false, 0);
		}
		GrenadeCount--;
		float time = ((myCurrentWeaponSounds != null) ? myCurrentWeaponSounds.grenadeThrowTime : 0.2667f);
		Invoke("RunGrenade", time);
		Invoke("SetGrenateFireEnabled", 1f);
	}

	private void SetGrenateFireEnabled()
	{
		Defs.isGrenateFireEnable = true;
	}

	private void RunGrenade()
	{
		if (currentGrenadeGadget != null)
		{
			currentGrenadeGadget.ThrowGrenade();
		}
		Invoke("ReturnWeaponAfterGrenade", 0.5f);
		isGrenadePress = false;
	}

	private void ReturnWeaponAfterGrenade()
	{
		if (currentWeaponBeforeGrenade != -1)
		{
			ChangeWeapon(currentWeaponBeforeGrenade, false);
		}
		currentWeaponBeforeGrenade = -1;
		if (inGameGUI != null && inGameGUI.blockedCollider != null)
		{
			inGameGUI.blockedCollider.SetActive(false);
		}
		if (inGameGUI != null && inGameGUI.blockedCollider2 != null)
		{
			inGameGUI.blockedCollider2.SetActive(false);
		}
		if (inGameGUI != null && inGameGUI.blockedColliderDater != null)
		{
			inGameGUI.blockedColliderDater.SetActive(false);
		}
		if (inGameGUI != null)
		{
			for (int i = 0; i < inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = true;
			}
			for (int j = 0; j < inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = true;
			}
		}
	}

	public static Rocket CreateRocket(WeaponSounds _currentWeaponSounds, Vector3 pos, Quaternion rot, float _chargePower = 1f)
	{
		GameObject rocket = RocketStack.sharedController.GetRocket();
		rocket.transform.position = pos;
		rocket.transform.rotation = rot;
		Rocket component = rocket.GetComponent<Rocket>();
		string weaponName = _currentWeaponSounds.gameObject.name.Replace("(Clone)", string.Empty);
		float radiusImpulse = _currentWeaponSounds.bazookaImpulseRadius * (1f + EffectsController.ExplosionImpulseRadiusIncreaseCoef);
		component.SendSetRocketActive(weaponName, radiusImpulse, _chargePower);
		return component;
	}

	private IEnumerator BazookaShoot()
	{
		for (int i = 0; i < _weaponManager.currentWeaponSounds.countInSeriaBazooka; i++)
		{
			SendFireFlash();
			if (_weaponManager.currentWeaponSounds.bazookaDelay > 0f)
			{
				yield return new WaitForSeconds(_weaponManager.currentWeaponSounds.bazookaDelay);
			}
			ShowGunFlash();
			_weaponManager.currentWeaponSounds.fire();
			float num = 0.2f;
			Rocket rocket = CreateRocket(myCurrentWeaponSounds, (_weaponManager.currentWeaponSounds.gunFlash != null) ? _weaponManager.currentWeaponSounds.gunFlash.position : (myTransform.position + myTransform.forward * num), myTransform.rotation, _weaponManager.currentWeaponSounds.isCharging ? lastChargeValue : 1f);
			rocketToLaunch = rocket.gameObject;
			if (i != _weaponManager.currentWeaponSounds.countInSeriaBazooka - 1)
			{
				yield return new WaitForSeconds(_weaponManager.currentWeaponSounds.stepTimeInSeriaBazooka);
			}
		}
	}

	private IEnumerator RunShockerEffect()
	{
		myCurrentWeaponSounds._innerPars.shockerEffect.SetActive(true);
		yield return new WaitForSeconds(1f);
		myCurrentWeaponSounds._innerPars.shockerEffect.SetActive(false);
	}

	private void RunOnGroundEffect(string name)
	{
		if (name == null || mySkinName == null)
		{
			return;
		}
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName("OnGroundWeaponEffects/" + name + "_OnGroundEffect");
		if (!(objectFromName == null))
		{
			PerformActionRecurs(objectFromName, delegate(Transform t)
			{
				t.gameObject.SetActive(false);
			});
			objectFromName.transform.parent = mySkinName.onGroundEffectsPoint;
			objectFromName.transform.localPosition = Vector3.zero;
			objectFromName.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
			PerformActionRecurs(objectFromName, delegate(Transform t)
			{
				t.gameObject.SetActive(true);
			});
			ParticleSystem component = objectFromName.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Play();
			}
		}
	}

	private IEnumerator HitRoundMelee(WeaponSounds weapon)
	{
		SendFireFlash(false, weapon.isDoubleShot ? numShootInDoubleShot : 0);
		yield return new WaitForSeconds(TimeOfMeleeAttack(weapon));
		if (weapon == null)
		{
			yield break;
		}
		RunOnGroundEffect(weapon.gameObject.name.Replace("(Clone)", string.Empty));
		float num = weapon.radiusRoundMelee * weapon.radiusRoundMelee;
		foreach (Transform item in new Initializer.TargetsList())
		{
			float sqrMagnitude = (item.position - _player.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				DamageTargetWithWeapon(weapon, item.gameObject, false, sqrMagnitude);
			}
		}
	}

	private void _DoHit(WeaponSounds weapon, RaycastHit _hit, bool slowdown = false)
	{
		bool isHeadshot = _hit.collider.name == "HeadCollider" || _hit.collider is SphereCollider;
		if ((bool)_hit.transform.root && _hit.transform.root.GetComponent<IDamageable>() != null)
		{
			DamageTargetWithWeapon(weapon, _hit.transform.root.gameObject, isHeadshot);
		}
		else
		{
			DamageTargetWithWeapon(weapon, _hit.transform.gameObject, isHeadshot);
		}
	}

	private float TimeOfMeleeAttack(WeaponSounds ws)
	{
		if (isMechActive)
		{
			return mechGunAnimation[ws.isDoubleShot ? "Shoot1" : "Shoot"].length * ws.meleeAttackTimeModifier;
		}
		return ws.animationObject.GetComponent<Animation>()[ws.isDoubleShot ? "Shoot1" : "Shoot"].length * ws.meleeAttackTimeModifier;
	}

	private void SendFireFlash(bool isFlash = true, int numFlash = 0)
	{
		if (!myCurrentWeaponSounds.isLoopShoot && isMulti && isInet)
		{
			photonView.RPC("fireFlash", PhotonTargets.Others, isFlash, numFlash);
		}
	}

	private void _FireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash = true, int numFlash = 0)
	{
		if (isMulti && isInet)
		{
			photonView.RPC("fireFlashWithHole", PhotonTargets.Others, _isBloodParticle, _pos, _rot, isFlash, numFlash);
		}
	}

	private void _FireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash = true, int numFlash = 0)
	{
		if (isMulti && isInet)
		{
			photonView.RPC("fireFlashWithManyHoles", PhotonTargets.Others, _isBloodParticle, _pos, _rot, isFlash, numFlash);
		}
	}

	
	[PunRPC]
	private void fireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash, int numFlash)
	{
		fireFlash(isFlash, numFlash);
		if (_isBloodParticle != null)
		{
			for (int i = 0; i < _isBloodParticle.Length; i++)
			{
				HoleRPC(_isBloodParticle[i], _pos[i], _rot[i]);
			}
		}
	}

	[PunRPC]
	
	private void fireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash, int numFlash)
	{
		fireFlash(isFlash, numFlash);
		HoleRPC(_isBloodParticle, _pos, _rot);
	}

	[PunRPC]
	
	private void fireFlash(bool isFlash, int numFlash)
	{
		WeaponSounds weaponSounds = (isMechActive ? mechWeaponSounds : myCurrentWeaponSounds);
		if (weaponSounds == null)
		{
			return;
		}
		if (isFlash)
		{
			if (weaponSounds.bazooka && weaponSounds.bazookaDelay > 0f)
			{
				ShowGunFlashDelayed(numFlash, weaponSounds.bazookaDelay);
			}
			else
			{
				ShowGunFlash(numFlash);
			}
			if (weaponSounds.railgun)
			{
				ShowRailgunRay(numFlash);
			}
		}
		if (weaponSounds.isRoundMelee)
		{
			float tm = TimeOfMeleeAttack(weaponSounds);
			StartCoroutine(RunOnGroundEffectCoroutine(weaponSounds.gameObject.name.Replace("(Clone)", string.Empty), tm));
		}
		string animation = (weaponSounds.isDoubleShot ? ("Shoot" + numFlash) : "Shoot");
		if (isMechActive)
		{
			mechGunAnimation.Play(animation);
			if (Defs.isSoundFX && currentMech != null)
			{
				GetComponent<AudioSource>().PlayOneShot(currentMech.shootSound);
			}
		}
		else
		{
			weaponSounds.animationObject.GetComponent<Animation>().Play(animation);
		}
		if (Defs.isSoundFX && !isMechActive)
		{
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().PlayOneShot(weaponSounds.shoot);
		}
		playChargeLoopAnim = false;
	}

	private FlashFire GetFlashFireByIndex(int numFlash)
	{
		WeaponSounds weaponSounds = (isMechActive ? mechWeaponSounds : myCurrentWeaponSounds);
		FlashFire result = null;
		if (numFlash == 0)
		{
			result = weaponSounds.GetComponent<FlashFire>();
		}
		else if (weaponSounds.gunFlashDouble.Length > numFlash - 1)
		{
			result = weaponSounds.gunFlashDouble[numFlash - 1].GetComponent<FlashFire>();
		}
		return result;
	}

	private void ShowGunFlash()
	{
		WeaponSounds weaponSounds = (isMechActive ? mechWeaponSounds : myCurrentWeaponSounds);
		if (weaponSounds.isDoubleShot && !weaponSounds.isMelee)
		{
			ShowGunFlash(numShootInDoubleShot);
		}
		else
		{
			ShowGunFlash(0);
		}
	}

	private void ShowGunFlashDelayed(int numFlash, float delay)
	{
		FlashFire flashFireByIndex = GetFlashFireByIndex(numFlash);
		if (flashFireByIndex != null)
		{
			flashFireByIndex.StartGunFlashDelayed(delay);
		}
	}

	private void ShowGunFlash(int numFlash)
	{
		FlashFire flashFireByIndex = GetFlashFireByIndex(numFlash);
		if (flashFireByIndex != null)
		{
			flashFireByIndex.StartGunFlash();
		}
	}

	private void ShowRailgunRay(int numFlash)
	{
		FlashFire flashFireByIndex = GetFlashFireByIndex(numFlash);
		if (flashFireByIndex != null)
		{
			flashFireByIndex.StartRailgunRay(this);
		}
	}

	
	[PunRPC]
	public void ChargeGunAnimation(bool active)
	{
		if (myCurrentWeaponSounds == null || !myCurrentWeaponSounds.isCharging)
		{
			return;
		}
		if (!active)
		{
			playChargeLoopAnim = false;
			if (!myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot"))
			{
				myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
			}
			if (Defs.isSoundFX && !isMechActive)
			{
				GetComponent<AudioSource>().Stop();
			}
		}
		else if (myCurrentWeaponSounds.chargeLoop)
		{
			playChargeLoopAnim = true;
			StartCoroutine(PlayChargeLoopAnim());
		}
		else
		{
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Charge");
			if (Defs.isSoundFX && !isMechActive)
			{
				GetComponent<AudioSource>().clip = myCurrentWeaponSounds.charge;
				GetComponent<AudioSource>().Play();
			}
		}
	}

	private IEnumerator PlayChargeLoopAnim()
	{
		while (playChargeLoopAnim && myCurrentWeaponSounds != null && myCurrentWeaponSounds.isCharging && myCurrentWeaponSounds.chargeLoop)
		{
			if (!myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Charge"))
			{
				myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Charge");
				if (Defs.isSoundFX && !isMechActive)
				{
					GetComponent<AudioSource>().clip = myCurrentWeaponSounds.charge;
					GetComponent<AudioSource>().Play();
				}
			}
			yield return null;
		}
		playChargeLoopAnim = false;
	}

	
	[PunRPC]
	public void HoleRPC(bool _isBloodParticle, Vector3 _pos, Quaternion _rot)
	{
		if (!Device.isPixelGunLow && !_isBloodParticle)
		{
			HoleScript currentHole = HoleBulletStackController.sharedController.GetCurrentHole(false);
			if (currentHole != null)
			{
				currentHole.StartShowHole(_pos, _rot, false);
			}
		}
	}

	private float GetRawWeaponDamage(WeaponSounds weapon)
	{
		if (weapon.isMechWeapon)
		{
			return weapon.DamageByTier[0];
		}
		float num = ((ExpController.Instance != null && ExpController.Instance.OurTier < weapon.DamageByTier.Length) ? weapon.DamageByTier[TierOrRoomTier(ExpController.Instance.OurTier)] : ((weapon.DamageByTier.Length != 0) ? weapon.DamageByTier[0] : 0f));
		return num * (1f + koofDamageWeaponFromPotoins + EffectsController.DamageModifsByCats(weapon.categoryNabor - 1));
	}

	private float GetWeaponDamage(WeaponSounds weapon, float sqrDistance, bool isHeadshot)
	{
		float num = GetRawWeaponDamage(weapon);
		if (weapon.isRoundMelee)
		{
			float num2 = num * 0.7f;
			float num3 = num;
			num = num2 + (num3 - num2) * (1f - sqrDistance / (weapon.radiusRoundMelee * weapon.radiusRoundMelee));
		}
		else if (weapon.snowStorm)
		{
			float num4 = num;
			num = num4 * (1f - sqrDistance / (weapon.range * weapon.range));
			if (sqrDistance < weapon.snowStormBonusRange)
			{
				num += num4 * weapon.snowStormBonusMultiplier;
			}
		}
		else if (isHeadshot)
		{
			num *= (weapon.isMechWeapon ? 2f : (2f + EffectsController.AddingForHeadshot(weapon.categoryNabor - 1)));
		}
		if (weapon.isCharging)
		{
			num *= lastChargeValue;
		}
		return num;
	}

	public void DamageTargetWithWeapon(WeaponSounds weapon, GameObject target, bool isHeadshot = false, float sqrDistance = 0f, float damageMultiplier = 1f)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(this))
		{
			return;
		}
		if (GameConnect.isDaterRegim && weapon.isDaterWeapon && component is PlayerDamageable)
		{
			Player_move_c myPlayer = (component as PlayerDamageable).myPlayer;
			myPlayer.SendDaterChat(mySkinName.NickName, weapon.daterMessage, myPlayer.mySkinName.NickName);
			return;
		}
		if (component is PlayerDamageable)
		{
			Player_move_c myPlayer2 = (component as PlayerDamageable).myPlayer;
			if (isHeadshot)
			{
				isHeadshot = UnityEngine.Random.Range(0f, 1f) >= myPlayer2._chanceToIgnoreHeadshot;
			}
			if (weapon.snowStorm && Defs.isInet)
			{
				SendPlayerEffect(myPlayer2.photonView.owner, 9, 0.5f);
			}
		}
		if (weapon.isSlowdown)
		{
			SlowdownTarget(component, weapon.slowdownTime, weapon.slowdownCoeff);
		}
		float num = GetWeaponDamage(weapon, sqrDistance, isHeadshot) * damageMultiplier;
		num *= DamageMultiplierByGadgets();
		bool flag = weapon.criticalHitChance >= UnityEngine.Random.Range(0, 100);
		if (flag)
		{
			num *= weapon.criticalHitCoef;
		}
		string weaponName = ((!isMechActive) ? weapon.gameObject.name.Replace("(Clone)", string.Empty) : ((currentMech != null) ? currentMech.killChatIcon : "Chat_Mech"));
		TypeKills typeKill = (isMechActive ? TypeKills.mech : (flag ? TypeKills.critical : (isHeadshot ? TypeKills.headshot : (isZooming ? TypeKills.zoomingshot : TypeKills.none))));
		ApplyDamageToTarget(component, num, weaponName, weapon.typeDead, typeKill);
		if (weapon.isPoisoning)
		{
			PoisonShotWithEffect(target, num, weapon);
		}
		if (weapon.isCharm)
		{
			CharmTarget(target, weapon.charmTime);
		}
	}

	public void DamageTarget(GameObject target, float damage, string weaponName, WeaponSounds.TypeDead typeDead, TypeKills typeKill)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component != null && component.IsEnemyTo(this))
		{
			ApplyDamageToTarget(component, damage, weaponName, typeDead, typeKill);
		}
	}

	private void ApplyDamageToTarget(IDamageable damageable, float damage, string weaponName, WeaponSounds.TypeDead typeDead, TypeKills typeKill)
	{
		damageable.ApplyDamage(damage, myDamageable, typeKill, typeDead, weaponName, skinNamePixelView.viewID);
	}

	public void SlowdownTarget(GameObject target, float slowdownTime, float slowdownCoeff)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component != null)
		{
			SlowdownTarget(component, slowdownTime, slowdownCoeff);
		}
	}

	public void SlowdownTarget(IDamageable damageable, float slowdownTime, float slowdownCoeff)
	{
		if (damageable is BaseBot)
		{
			(damageable as BaseBot).ApplyDebuffByMode(BotDebuffType.DecreaserSpeed, slowdownTime, slowdownCoeff);
		}
		if (damageable is PlayerDamageable)
		{
			(damageable as PlayerDamageable).myPlayer.SlowdownPlayer(slowdownCoeff, slowdownTime);
		}
	}

	public void SlowdownPlayer(float coef, float time)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				photonView.RPC("SlowdownRPC", photonView.owner, coef, time);
			}
		}
		else
		{
			SlowdownRPC(coef, time);
		}
	}

	
	[PunRPC]
	public void SlowdownRPC(float coef, float time)
	{
		if (!Defs.isMulti || isMine)
		{
			EffectsController.SlowdownCoeff = coef;
			_timeOfSlowdown = time;
		}
	}

	public void ImpactPlayer(Vector3 point, float radiusImpulse, float impulseForce, float impulseForceSelf = 0f, bool selfForce = false)
	{
		Vector3 dir = myPlayerTransform.position - point;
		float sqrMagnitude = dir.sqrMagnitude;
		float num = radiusImpulse * radiusImpulse;
		if (sqrMagnitude < num)
		{
			ImpactReceiver impactReceiver = myPlayerTransform.gameObject.GetComponent<ImpactReceiver>();
			if (impactReceiver == null)
			{
				impactReceiver = myPlayerTransform.gameObject.AddComponent<ImpactReceiver>();
			}
			float num2 = 100f;
			if (radiusImpulse != 0f)
			{
				num2 = Mathf.Sqrt(sqrMagnitude / num);
			}
			float num3 = Mathf.Max(0f, 1f - num2);
			float num4 = (selfForce ? impulseForceSelf : impulseForce);
			num4 *= num3;
			impactReceiver.AddImpact(dir, num4);
			if (selfForce && num3 > 0.01f)
			{
				isRocketJump = true;
			}
		}
	}

	[PunRPC]
	
	private void ReloadGun()
	{
		if (!(myCurrentWeaponSounds == null))
		{
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].speed = _currentReloadAnimationSpeed;
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(myCurrentWeaponSounds.reload);
			}
		}
	}

	private bool Reload()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && inGameGUI != null)
		{
			if (WeaponManager.sharedManager.currentWeaponSounds.ammoInClip > 1 || !WeaponManager.sharedManager.currentWeaponSounds.isShotMelee)
			{
				AnimationClip clip = WeaponManager.sharedManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload");
				if (!(clip != null))
				{
					return false;
				}
				inGameGUI.ShowCircularIndicatorOnReload(clip.length / _currentReloadAnimationSpeed);
			}
			else
			{
				WeaponManager.sharedManager.ReloadAmmo();
			}
		}
		WeaponManager.sharedManager.Reload();
		return true;
	}

	public void ReloadPressed()
	{
		if ((myCurrentWeaponSounds.isCharging && chargeValue > 0f) || isGrenadePress || isReloading || isMechActive || (_weaponManager.currentWeaponSounds.isMelee && !_weaponManager.currentWeaponSounds.isShotMelee))
		{
			return;
		}
		if (isZooming)
		{
			ZoomPress();
		}
		if (_weaponManager.CurrentWeaponIndex < 0 || _weaponManager.CurrentWeaponIndex >= _weaponManager.playerWeapons.Count || GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).currentAmmoInBackpack <= 0 || GetWeaponByIndex(_weaponManager.CurrentWeaponIndex).currentAmmoInClip == _weaponManager.currentWeaponSounds.ammoInClip || !Reload() || _weaponManager.currentWeaponSounds.isShotMelee)
		{
			return;
		}
		if (isMulti && isInet)
		{
			photonView.RPC("ReloadGun", PhotonTargets.Others);
		}
		if (Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.reload);
		}
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.HasAmmo();
			if (inGameGUI != null)
			{
				inGameGUI.BlinkNoAmmo(0);
			}
		}
		else
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
	}

	public void RemoveRay(FreezerRay ray)
	{
		for (int i = 0; i < freezeRays.Length; i++)
		{
			if ((bool)(freezeRays[i] = ray))
			{
				freezeRays[i] = null;
			}
		}
	}

	public void AddScoreKillPet()
	{
		myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.petKnockout);
		AddCountSerials(7, this);
	}

	private void AddFreezerRayForGunFlash(int freezeInd, Transform gf, float len)
	{
		if (gf == null)
		{
			return;
		}
		if (freezeRays[freezeInd] != null)
		{
			freezeRays[freezeInd].UpdatePosition(len);
			return;
		}
		GameObject gameObject = WeaponManager.AddRay(gf.gameObject.transform.parent.position, gf.gameObject.transform.parent.parent.forward, gf.gameObject.transform.parent.parent.GetComponent<WeaponSounds>().railName, len);
		if (gameObject != null)
		{
			freezeRays[freezeInd] = gameObject.GetComponent<FreezerRay>();
			if (freezeRays[freezeInd] != null)
			{
				freezeRays[freezeInd].Activate(this, gf);
			}
		}
	}

	
	[PunRPC]
	public void AddFreezerRayWithLength(float len)
	{
		if (myCurrentWeaponSounds == null)
		{
			return;
		}
		if (myCurrentWeaponSounds.isDoubleShot)
		{
			for (int i = 0; i < myCurrentWeaponSounds.gunFlashDouble.Length; i++)
			{
				Transform gf = myCurrentWeaponSounds.gunFlashDouble[i];
				AddFreezerRayForGunFlash(i, gf, len);
			}
			return;
		}
		Transform gunFlash = GunFlash;
		if (gunFlash == null && myTransform.childCount > 0)
		{
			FlashFire component = myTransform.GetChild(0).GetComponent<FlashFire>();
			if (component != null && component.gunFlashObj != null)
			{
				gunFlash = component.gunFlashObj.transform;
			}
		}
		AddFreezerRayForGunFlash(0, gunFlash, len);
	}

	public void RunTurret()
	{
		if (Defs.isTurretWeapon)
		{
			if (GameConnect.isDaterRegim)
			{
				string key = (GameConnect.isDaterRegim ? GearManager.MusicBox : GearManager.Turret);
				Storager.setInt(key, Storager.getInt(key) - 1);
				PotionsController.sharedController.ActivatePotion(GearManager.Turret, this, new Dictionary<string, object>());
			}
			currentTurret.transform.parent = null;
			currentTurret.GetComponent<TurretController>().StartTurret();
			ChangeWeapon(currentWeaponBeforeTurret, false);
			currentWeaponBeforeTurret = -1;
		}
	}

	public void CancelTurret()
	{
		ChangeWeapon(currentWeaponBeforeTurret, false);
		currentWeaponBeforeTurret = -1;
		if (!(currentTurret != null))
		{
			return;
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				PhotonNetwork.Destroy(currentTurret);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(currentTurret);
		}
	}

	public void SendLike(Player_move_c whomMoveC)
	{
		if (whomMoveC != null)
		{
			whomMoveC.SendDaterChat(mySkinName.NickName, "Key_1803", whomMoveC.mySkinName.NickName);
		}
		if (Defs.isInet)
		{
			photonView.RPC("LikeRPC", PhotonTargets.All, photonView.ownerId, whomMoveC.photonView.ownerId);
		}
	}

	
	[PunRPC]
	private void LikeRPC(int idWho, int idWhom)
	{
		Player_move_c player_move_c = null;
		Player_move_c player_move_c2 = null;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c player_move_c3 = Initializer.players[i];
			if (idWho == player_move_c3.photonView.ownerId)
			{
				player_move_c = player_move_c3;
			}
			if (idWhom == player_move_c3.photonView.ownerId)
			{
				player_move_c2 = player_move_c3;
			}
		}
		if (player_move_c != null && player_move_c2 != null)
		{
			Like(player_move_c, player_move_c2);
		}
	}

	private void Like(Player_move_c whoMoveC, Player_move_c whomMoveC)
	{
		if (whomMoveC.Equals(WeaponManager.sharedManager.myPlayerMoveC))
		{
			countKills++;
			GlobalGameController.CountKills = countKills;
			WeaponManager.sharedManager.myNetworkStartTable.CountKills = countKills;
			WeaponManager.sharedManager.myNetworkStartTable.SynhCountKills();
			ProfileController.OnGetLike();
		}
	}

	private int GetNumShootInDouble()
	{
		numShootInDoubleShot++;
		if (numShootInDoubleShot == 3)
		{
			numShootInDoubleShot = 1;
		}
		return numShootInDoubleShot;
	}

	
	[PunRPC]
	private void SyncTurretUpgrade(int turretUpgrade)
	{
		this.turretUpgrade = turretUpgrade;
	}

	private void InitPurchaseActions()
	{
		_actionsForPurchasedItems.Add("bigammopack", ProvideAmmo);
		_actionsForPurchasedItems.Add("Fullhealth", ProvideHealth);
		_actionsForPurchasedItems.Add(StoreKitEventListener.elixirID, delegate
		{
			Defs.NumberOfElixirs++;
		});
		string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(true);
		for (int i = 0; i < canBuyWeaponTags.Length; i++)
		{
			string shopIdByTag = ItemDb.GetShopIdByTag(canBuyWeaponTags[i]);
			_actionsForPurchasedItems.Add(shopIdByTag, AddWeaponToInv);
		}
	}

	private void AddWeaponToInv(string shopId)
	{
		string tagByShopId = ItemDb.GetTagByShopId(shopId);
		ItemRecord byTag = ItemDb.GetByTag(tagByShopId);
		if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && byTag != null && !byTag.TemporaryGun)
		{
			SaveWeaponInPrefs(tagByShopId);
		}
		GameObject prefabByTag = _weaponManager.GetPrefabByTag(tagByShopId);
		AddWeapon(prefabByTag);
	}

	public static void SaveWeaponInPrefs(string weaponTag, int timeForRentIndex = 0)
	{
		string storageIdByTag = ItemDb.GetStorageIdByTag(weaponTag);
		if (storageIdByTag == null)
		{
			int tm = TempItemsController.RentTimeForIndex(timeForRentIndex);
			TempItemsController.sharedController.AddTemporaryItem(weaponTag, tm);
			return;
		}
		Storager.setInt(storageIdByTag, 1);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}
}
