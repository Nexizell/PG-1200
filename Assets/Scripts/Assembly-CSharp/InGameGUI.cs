using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public sealed class InGameGUI : MonoBehaviour
{
	public delegate float GetFloatVAlue();

	public delegate string GetString();

	public delegate int GetIntVAlue();

	[CompilerGenerated]
	internal sealed class _003CPlayLowResourceBeepCoroutine_003Ed__204 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InGameGUI _003C_003E4__this;

		private int _003Ci_003E5__1;

		public int count;

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
		public _003CPlayLowResourceBeepCoroutine_003Ed__204(int _003C_003E1__state)
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
				_003Ci_003E5__1 = 0;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__1 + 1;
				_003Ci_003E5__1 = num;
				break;
			}
			}
			if (_003Ci_003E5__1 < count)
			{
				if (Defs.isSoundFX)
				{
					NGUITools.PlaySound(_003C_003E4__this.lowResourceBeep);
				}
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this._lowResourceBeepRoutine = null;
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
	internal sealed class _003C_DisableSwiping_003Ed__273 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InGameGUI _003C_003E4__this;

		public float tm;

		private MyCenterOnChild _003C_center_003E5__1;

		private int _003Cbef_003E5__2;

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
		public _003C_DisableSwiping_003Ed__273(int _003C_003E1__state)
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
				_003C_center_003E5__1 = _003C_003E4__this.changeWeaponWrap.GetComponent<MyCenterOnChild>();
				if (_003C_center_003E5__1 == null || _003C_center_003E5__1.centeredObject == null)
				{
					return false;
				}
				if (!int.TryParse(_003C_center_003E5__1.centeredObject.name.Replace("WeaponCat_", string.Empty), out _003Cbef_003E5__2))
				{
					return false;
				}
				_003C_003E4__this._disabled = true;
				_003C_003E2__current = new WaitForSeconds(tm);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this._disabled = false;
				if (_003C_center_003E5__1.centeredObject == null || _003C_center_003E5__1.centeredObject.name.Equals("WeaponCat_" + _003Cbef_003E5__2))
				{
					return false;
				}
				Transform transform = null;
				foreach (Transform item in _003C_center_003E5__1.transform)
				{
					if (item.gameObject.name.Equals("WeaponCat_" + _003Cbef_003E5__2))
					{
						transform = item;
						break;
					}
				}
				if (transform != null)
				{
					_003C_center_003E5__1.CenterOn(transform);
				}
				return false;
			}
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

	[CompilerGenerated]
	internal sealed class _003CAnimateHealth_003Ed__308 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InGameGUI _003C_003E4__this;

		private WaitForSeconds _003Cawaiter_003E5__1;

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
		public _003CAnimateHealth_003Ed__308(int _003C_003E1__state)
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
				_003C_003E4__this.healthInAnim = true;
				_003C_003E4__this.currentHealthStep = Mathf.CeilToInt(_003C_003E4__this.pastHealth);
				_003Cawaiter_003E5__1 = new WaitForSeconds(0.05f);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003C_003E4__this.currentHealthStep != Mathf.CeilToInt(_003C_003E4__this.health()))
			{
				int num = _003C_003E4__this.currentHealthStep - 9 * Mathf.FloorToInt((float)_003C_003E4__this.currentHealthStep / 9f);
				int num2 = Mathf.CeilToInt(_003C_003E4__this.health());
				if (num2 < 0)
				{
					num2 = 0;
				}
				bool flag = _003C_003E4__this.currentHealthStep > num2;
				if (flag)
				{
					num--;
					if (num < 0)
					{
						num = 8;
					}
				}
				_003C_003E4__this.currentHealthStep += ((!flag) ? 1 : (-1));
				_003C_003E4__this.hearts[num].Animate(flag ? Mathf.FloorToInt((float)_003C_003E4__this.currentHealthStep / 9f) : Mathf.CeilToInt((float)_003C_003E4__this.currentHealthStep / 9f), HeartEffect.IndicatorType.Hearts);
				_003C_003E2__current = _003Cawaiter_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.healthInAnim = false;
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
	internal sealed class _003CAnimateArmor_003Ed__309 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InGameGUI _003C_003E4__this;

		private WaitForSeconds _003Cawaiter_003E5__1;

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
		public _003CAnimateArmor_003Ed__309(int _003C_003E1__state)
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
				_003C_003E4__this.armorInAnim = true;
				_003C_003E4__this.currentArmorStep = Mathf.CeilToInt(_003C_003E4__this.pastArmor);
				_003Cawaiter_003E5__1 = new WaitForSeconds(0.05f);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003C_003E4__this.currentArmorStep != Mathf.CeilToInt(_003C_003E4__this.armor()))
			{
				int num = _003C_003E4__this.currentArmorStep - 9 * Mathf.FloorToInt((float)_003C_003E4__this.currentArmorStep / 9f);
				int num2 = Mathf.CeilToInt(_003C_003E4__this.armor());
				if (num2 < 0)
				{
					num2 = 0;
				}
				bool flag = _003C_003E4__this.currentArmorStep > num2;
				if (flag)
				{
					num--;
					if (num < 0)
					{
						num = 8;
					}
				}
				_003C_003E4__this.currentArmorStep += ((!flag) ? 1 : (-1));
				_003C_003E4__this.armorShields[num].Animate(flag ? Mathf.FloorToInt((float)_003C_003E4__this.currentArmorStep / 9f) : Mathf.CeilToInt((float)_003C_003E4__this.currentArmorStep / 9f), HeartEffect.IndicatorType.Armor);
				_003C_003E2__current = _003Cawaiter_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.armorInAnim = false;
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
	internal sealed class _003CAnimateMechHealth_003Ed__310 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InGameGUI _003C_003E4__this;

		private WaitForSeconds _003Cawaiter_003E5__1;

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
		public _003CAnimateMechHealth_003Ed__310(int _003C_003E1__state)
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
				_003C_003E4__this.mechInAnim = true;
				_003C_003E4__this.currentMechHealthStep = Mathf.CeilToInt(_003C_003E4__this.pastMechHealth);
				_003Cawaiter_003E5__1 = new WaitForSeconds(0.05f);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003C_003E4__this.currentMechHealthStep != Mathf.CeilToInt(_003C_003E4__this.playerMoveC.liveMech))
			{
				int num = _003C_003E4__this.currentMechHealthStep - 18 * Mathf.FloorToInt((float)_003C_003E4__this.currentMechHealthStep / 18f);
				int num2 = Mathf.CeilToInt(_003C_003E4__this.playerMoveC.liveMech);
				if (num2 < 0)
				{
					num2 = 0;
				}
				bool flag = _003C_003E4__this.currentMechHealthStep > num2;
				if (flag)
				{
					num--;
					if (num < 0)
					{
						num = 17;
					}
				}
				_003C_003E4__this.currentMechHealthStep += ((!flag) ? 1 : (-1));
				_003C_003E4__this.mechShields[num].Animate(flag ? Mathf.FloorToInt((float)_003C_003E4__this.currentMechHealthStep / 18f) : Mathf.CeilToInt((float)_003C_003E4__this.currentMechHealthStep / 18f), HeartEffect.IndicatorType.Mech);
				_003C_003E2__current = _003Cawaiter_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.mechInAnim = false;
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
	internal sealed class _003CShowRespawnWindow_003Ed__315 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KillerInfo killerInfo;

		public InGameGUI _003C_003E4__this;

		public float seconds;

		private bool _003CshowCharacter_003E5__1;

		private SkinName _003CskinNameComponent_003E5__2;

		private float _003CcloseTime_003E5__3;

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
		public _003CShowRespawnWindow_003Ed__315(int _003C_003E1__state)
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
				CameraSceneController.sharedController.killCamController.lastDistance = 1f;
				CameraSceneController.sharedController.SetTargetKillCam(killerInfo.killerTransform);
				Defs.inRespawnWindow = true;
				_003C_003E4__this.respawnWindow.Show(killerInfo);
				_003C_003E4__this.respawnWindow.characterDrag.SetActive(false);
				_003C_003E4__this.respawnWindow.cameraDrag.SetActive(true);
				_003CskinNameComponent_003E5__2 = killerInfo.killerTransform.GetComponent<SkinName>();
				_003CcloseTime_003E5__3 = seconds;
				_003CshowCharacter_003E5__1 = false;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (Defs.inRespawnWindow)
			{
				if (!_003CshowCharacter_003E5__1 && (killerInfo.killerTransform == null || killerInfo.killerTransform.position.y <= -5000f || _003CskinNameComponent_003E5__2.playerMoveC.isKilled))
				{
					_003CshowCharacter_003E5__1 = true;
					_003C_003E4__this.respawnWindow.characterDrag.SetActive(true);
					_003C_003E4__this.respawnWindow.cameraDrag.SetActive(false);
					RespawnWindow.Instance.ShowCharacter(killerInfo);
					CameraSceneController.sharedController.SetTargetKillCam();
				}
				_003CcloseTime_003E5__3 -= Time.deltaTime;
				_003C_003E4__this.respawnWindow.SetCurrentTime(_003CcloseTime_003E5__3);
				if (_003CcloseTime_003E5__3 <= 0f)
				{
					_003C_003E4__this.respawnWindow.CloseRespawnWindow();
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
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

	public UILabel Wave1_And_Counter;

	public UILabel reloadLabel;

	public GameObject reloadBar;

	public UIPlayTween impactTween;

	public UITexture reloadCircularSprite;

	public UITexture fireCircularSprite;

	public UITexture fireAdditionalCrcualrSprite;

	private UITexture[] circularSprites;

	public GameObject centerAnhor;

	public UILabel newWave;

	public UILabel waveDone;

	public UILabel SurvivalWaveNumber;

	[Header("Timer/Score Container")]
	public GameObject deathmatchContainer;

	[Header("Timer/Score Container")]
	public GameObject daterContainer;

	[Header("Timer/Score Container")]
	public GameObject teamBattleContainer;

	[Header("Timer/Score Container")]
	public GameObject timeBattleContainer;

	[Header("Timer/Score Container")]
	public GameObject deadlygamesContainer;

	[Header("Timer/Score Container")]
	public GameObject flagCaptureContainer;

	[Header("Timer/Score Container")]
	public GameObject survivalContainer;

	[Header("Timer/Score Container")]
	public GameObject CampaignContainer;

	[Header("Timer/Score Container")]
	public GameObject CapturePointContainer;

	[Header("Timer/Score Container")]
	public GameObject duelContainer;

	[Header("Timer/Score Container")]
	public GameObject waitDuelLabel;

	[Header("DeathEscape")]
	public GameObject deathEscapeContainer;

	public GameObject panelFinishLap;

	public DeathEscapeUI deathEscapeUI;

	public UILabel currentLapTime;

	public UILabel bestLapTime;

	public UILabel recordLapTime;

	public UILabel noRecordLabel;

	public UILabel recordLabel;

	public UIButton lapFinishButton;

	public UIButton newLapButton;

	[Header("-")]
	[Header("Runner")]
	public GameObject runnerShieldBuy;

	public UIButton runnerShieldBuyButton;

	public UILabel runnerShieldBuyPrice;

	public GameObject speedrunContainer;

	public GameObject speedrunSafeZone;

	[Header("-")]
	public UILabel waitDuelLabelTimer;

	public GameObject[] hidesPanelInTurrel;

	public GameObject turretPanel;

	public GameObject topPanelsTapReceiver;

	public ButtonHandler runTurrelButton;

	public ButtonHandler cancelTurrelButton;

	[Range(1f, 1000f)]
	public float minLength = 300f;

	[Range(1f, 1000f)]
	public float maxLength = 550f;

	[Range(1f, 1000f)]
	public float defaultPanelLength = 486f;

	public Transform sideObjGearShop;

	public static InGameGUI sharedInGameGUI;

	public GameObject pausePanel;

	public Transform shopPanelForTap;

	public Transform shopPanelForSwipe;

	public Transform shopPanelForTapDater;

	public Transform shopPanelForSwipeDater;

	public Transform swipeWeaponPanel;

	public Transform swipeArrowLeft;

	public Transform swipeArrowRight;

	public static Vector3 swipeWeaponPanelPos;

	public static Vector3 shopPanelForTapPos;

	public static Vector3 shopPanelForSwipePos;

	public GameObject blockedCollider;

	public GameObject blockedCollider2;

	public GameObject blockedColliderDater;

	public GameObject zoomButton;

	public GameObject reloadButton;

	public GameObject jumpButton;

	public GameObject fireButton;

	public GameObject fireButtonInJoystick;

	public GameObject joystick;

	public GameObject grenadeButton;

	public GameObject chooseGadgetPanel;

	public GameObject bottomPanel;

	public UISprite fireButtonSprite;

	public UISprite fireButtonSprite2;

	public GameObject aimPanel;

	public GameObject flagBlueCaptureTexture;

	public GameObject flagRedCaptureTexture;

	public GameObject message_draw;

	public GameObject message_now;

	public GameObject message_wait;

	public GameObject message_returnFlag;

	public float timerShowNow;

	public GameObject interfacePanel;

	public UILabel timerStartHungerLabel;

	public GameObject enemiesLeftLabel;

	public GameObject duel;

	public GameObject downBloodTexture;

	public GameObject upBloodTexture;

	public GameObject leftBloodTexture;

	public GameObject rightBloodTexture;

	public GameObject aimUp;

	public GameObject aimDown;

	public GameObject aimRight;

	public GameObject aimLeft;

	public GameObject aimCenter;

	public GameObject aimUpLeft;

	public GameObject aimDownLeft;

	public GameObject aimDownRight;

	public GameObject aimUpRight;

	[HideInInspector]
	public UISprite aimUpSprite;

	[HideInInspector]
	public UISprite aimDownSprite;

	[HideInInspector]
	public UISprite aimRightSprite;

	[HideInInspector]
	public UISprite aimLeftSprite;

	[HideInInspector]
	public UISprite aimCenterSprite;

	[HideInInspector]
	public UISprite aimUpLeftSprite;

	[HideInInspector]
	public UISprite aimDownLeftSprite;

	[HideInInspector]
	public UISprite aimDownRightSprite;

	[HideInInspector]
	public UISprite aimUpRightSprite;

	public UISprite aimRect;

	public GameObject topAnchor;

	public GameObject leftAnchor;

	public GameObject rightAnchor;

	public GameObject bottomAnchor;

	public GetFloatVAlue health;

	public GetFloatVAlue armor;

	public GetIntVAlue armorType;

	public GetString killsToMaxKills;

	public GetString timeLeft;

	public UIButton gearToogle;

	public UIButton[] weaponCategoriesButtons;

	public UILabel[] ammoCategoriesLabels;

	public UIButton[] weaponCategoriesButtonsDater;

	public UILabel[] ammoCategoriesLabelsDater;

	public GameObject fonBig;

	public GameObject fonSmall;

	public GameObject pointCaptureBar;

	public UISprite teamColorSprite;

	public UISprite captureBarSprite;

	public UILabel pointCaptureName;

	public HeartEffect[] hearts;

	public HeartEffect[] armorShields;

	public HeartEffect[] mechShields;

	public DamageTakenController[] damageTakenControllers;

	private int curDamageTakenController;

	private float timerShowPotion = -1f;

	private float timerShowPotionMax = 10f;

	public SetChatLabelController[] killLabels;

	public GameObject[] chatLabels;

	public UILabel[] messageAddScore;

	public GameObject elixir;

	public GameObject scoreLabel;

	public GameObject enemiesLabel;

	public GameObject timeLabel;

	public GameObject killsLabel;

	public GameObject scopeText;

	public GameObject joystickContainer;

	public GameObject nightVisionEffect;

	public UILabel rulesLabel;

	public Player_move_c playerMoveC;

	private ZombieCreator zombieCreator;

	public UIPanel multyKillPanel;

	public UISprite multyKillSprite;

	private bool isMulti;

	private bool isChatOn;

	private bool isInet;

	public GameObject[] upButtonsInShopPanel;

	public GameObject[] upButtonsInShopPanelSwipeRegim;

	public GameObject healthAddButton;

	public GameObject healthAddButtonDater;

	public GameObject ammoAddButton;

	public GameObject ammoAddButtonDater;

	public UITexture[] weaponIcons;

	public UITexture[] weaponIconsDater;

	public GameObject fastShopPanel;

	public UIScrollView changeWeaponScroll;

	public UIWrapContent changeWeaponWrap;

	public GameObject weaponPreviewPrefab;

	public int weaponIndexInScroll;

	public int weaponIndexInScrollOld;

	public int widthWeaponScrollPreview;

	public AudioClip lowResourceBeep;

	public UIPanel joystikPanel;

	public UIPanel shopPanel;

	public UIPanel bloodPanel;

	public UILabel perfectLabels;

	[SerializeField]
	protected internal PrefabHandler _respawnWindowPrefab;

	private LazyObject<RespawnWindow> _lazyRespWindow;

	public UIPanel offGameGuiPanel;

	public UIButton pauseButton;

	public UIButton exitButton;

	public GameObject mineRed;

	public GameObject mineBlue;

	public GameObject winningBlue;

	public GameObject winningRed;

	public GameObject firstPlaceGO;

	public GameObject firstPlaceCoop;

	public UILabel placeDeathmatchLabel;

	public UILabel placeCoopLabel;

	public BankShopViewGuiElement shopView;

	private IEnumerator _lowResourceBeepRoutine;

	private float timerBlinkNoAmmo;

	private float periodBlink = 2f;

	public UILabel blinkNoAmmoLabel;

	private float timerBlinkNoHeath;

	public UILabel blinkNoHeathLabel;

	public UISprite[] blinkNoHeathFrames;

	private int oldCountHeath;

	public float timerShowScorePict;

	public float maxTimerShowScorePict = 3f;

	public string scorePictName = string.Empty;

	public UISprite ChargeValue;

	public PlayGadgetSFX timeTravelEffect;

	public PlayGadgetSFX burningEffect;

	public PlayGadgetSFX reviveEffect;

	public PlayGadgetSFX healEffect;

	public PlayGadgetSFX pandoraSuccessEffect;

	public PlayGadgetSFX pandoraFailEffect;

	public PlayGadgetSFX disablerEffect;

	public PlayGadgetSFX blackMarkEffect;

	public PlayGadgetSFX medStationEffect;

	public PlayGadgetSFX shieldEffect;

	public PlayGadgetSFX drumEffect;

	public PlayGadgetSFX frozeEffect;

	public PlayGadgetSFX bleedEffect;

	public PlayGadgetSFX poisonEffect;

	public PlayGadgetSFX snowmanEffect;

	public PlayGadgetSFX charmEffect;

	[SerializeField]
	protected internal GameObject _subpanelsContainer;

	private bool hideHealthInInterface;

	private bool _kBlockPauseShopButton;

	public bool isTurretInterfaceActive;

	private IDisposable _backSubscription;

	private bool _disabled;

	private bool crosshairVisible;

	private bool aimRectVisible;

	private Vector2[] aimPositions = new Vector2[6];

	private CrosshairData.aimSprite defaultAimCenter = new CrosshairData.aimSprite("", new Vector2(12f, 12f), new Vector2(0f, 0f));

	private CrosshairData.aimSprite defaultAimDown = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	private CrosshairData.aimSprite defaultAimUp = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	private CrosshairData.aimSprite defaultAimLeftCenter = new CrosshairData.aimSprite("pricel_h", new Vector2(12f, 12f), new Vector2(8f, 0f));

	private CrosshairData.aimSprite defaultAimLeftDown = new CrosshairData.aimSprite("", new Vector2(12f, 12f), new Vector2(8f, 8f));

	private CrosshairData.aimSprite defaultAimLeftUp = new CrosshairData.aimSprite("", new Vector2(12f, 12f), new Vector2(8f, 8f));

	private KeyValuePair<int, string>[] _formatMeleeAmmoMemo;

	private KeyValuePair<Ammo, string>[] _formatShootingAmmoMemo;

	private readonly StringBuilder _stringBuilder = new StringBuilder();

	private KeyValuePair<int, string> _rankMemo = new KeyValuePair<int, string>(0, "1");

	private float pastHealth;

	private float pastMechHealth;

	private float pastArmor;

	private bool mechWasActive;

	private int currentHealthStep;

	private int currentMechHealthStep;

	private int currentArmorStep;

	private bool healthInAnim;

	private bool armorInAnim;

	private bool mechInAnim;

	private const string weaponCat = "WeaponCat_";

	public RespawnWindow respawnWindow
	{
		get
		{
			if (_lazyRespWindow == null)
			{
				_lazyRespWindow = new LazyObject<RespawnWindow>(_respawnWindowPrefab.ResourcePath, SubpanelsContainer);
			}
			return _lazyRespWindow.Value;
		}
	}

	public GameObject SubpanelsContainer
	{
		get
		{
			return _subpanelsContainer;
		}
	}

	public void ShowCircularIndicatorOnReload(float length)
	{
		StopAllCircularIndicators();
		reloadBar.SetActive(true);
		reloadLabel.gameObject.SetActive(true);
		Invoke("ReloadAmmo", length);
		if (playerMoveC != null)
		{
			playerMoveC.isReloading = true;
		}
		RunCircularSpriteOn(reloadCircularSprite, length, delegate
		{
		});
	}

	private void ReloadAmmo()
	{
		reloadLabel.gameObject.SetActive(false);
		reloadBar.SetActive(false);
		WeaponManager.sharedManager.ReloadAmmo();
	}

	public void StartFireCircularIndicators(float length)
	{
		StopAllCircularIndicators();
		RunCircularSpriteOn(fireCircularSprite, length);
		RunCircularSpriteOn(fireAdditionalCrcualrSprite, length);
	}

	private void RunCircularSpriteOn(UITexture sprite, float length, Action onComplete = null)
	{
		sprite.fillAmount = 0f;
		HOTween.To(sprite, length, new TweenParms().Prop("fillAmount", 1f).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear)
			.OnComplete((TweenDelegate.TweenCallback)delegate
			{
				sprite.fillAmount = 0f;
				if (onComplete != null)
				{
					onComplete();
				}
			}));
	}

	public void StopAllCircularIndicators()
	{
		CancelInvoke("ReloadAmmo");
		if (playerMoveC != null)
		{
			playerMoveC.isReloading = false;
		}
		if (circularSprites == null)
		{
			UnityEngine.Debug.LogWarning("Circular sprites is null!");
			return;
		}
		UITexture[] array = circularSprites;
		foreach (UITexture obj in array)
		{
			HOTween.Kill(obj);
			obj.fillAmount = 0f;
		}
		reloadLabel.gameObject.SetActive(false);
		reloadBar.SetActive(false);
	}

	public void PlayLowResourceBeep(int count)
	{
		StopPlayingLowResourceBeep();
		_lowResourceBeepRoutine = PlayLowResourceBeepCoroutine(count);
		StartCoroutine(_lowResourceBeepRoutine);
	}

	public void SetEnablePerfectLabel(bool enabled)
	{
		if (!(perfectLabels == null))
		{
			perfectLabels.gameObject.SetActive(enabled);
		}
	}

	public void PlayLowResourceBeepIfNotPlaying(int count)
	{
		if (_lowResourceBeepRoutine == null)
		{
			PlayLowResourceBeep(count);
		}
	}

	public void StopPlayingLowResourceBeep()
	{
		if (_lowResourceBeepRoutine != null)
		{
			StopCoroutine(_lowResourceBeepRoutine);
			_lowResourceBeepRoutine = null;
		}
	}

	private IEnumerator PlayLowResourceBeepCoroutine(int count)
	{
		int i = 0;
		while (i < count)
		{
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(lowResourceBeep);
			}
			yield return new WaitForSeconds(1f);
			int num = i + 1;
			i = num;
		}
		_lowResourceBeepRoutine = null;
	}

	private void HandleChatSettUpdated()
	{
		isChatOn = Defs.IsChatOn;
	}

	private void Awake()
	{
		hideHealthInInterface = GameConnect.isDaterRegim || GameConnect.isDeathEscape || GameConnect.isSpleef || GameConnect.isSpeedrun;
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", GetType().Name);
		using (new ScopeLogger(callee, false))
		{
			sharedInGameGUI = this;
			circularSprites = new UITexture[3] { reloadCircularSprite, fireCircularSprite, fireAdditionalCrcualrSprite };
			changeWeaponScroll.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, (float)widthWeaponScrollPreview * 1.3f, (float)widthWeaponScrollPreview * 1.3f);
			changeWeaponWrap.itemSize = widthWeaponScrollPreview;
			HandleChatSettUpdated();
			PauseNGUIController.ChatSettUpdated += HandleChatSettUpdated;
			ControlsSettingsBase.ControlsChanged += AdjustToPlayerHands;
			if (GameConnect.isDaterRegim)
			{
				shopPanelForTap = shopPanelForTapDater;
				shopPanelForSwipe = shopPanelForSwipeDater;
				ammoAddButton = ammoAddButtonDater;
				healthAddButton = healthAddButtonDater;
				for (int i = 0; i < weaponCategoriesButtons.Length; i++)
				{
					weaponCategoriesButtons[i] = weaponCategoriesButtonsDater[i];
				}
				for (int j = 0; j < ammoCategoriesLabels.Length; j++)
				{
					ammoCategoriesLabels[j] = ammoCategoriesLabelsDater[j];
				}
				for (int k = 0; k < weaponIcons.Length; k++)
				{
					weaponIcons[k] = weaponIconsDater[k];
				}
			}
			shopPanelForTap.gameObject.SetActive(true);
			shopPanelForSwipe.gameObject.SetActive(true);
			swipeWeaponPanelPos = swipeWeaponPanel.localPosition;
			shopPanelForTapPos = shopPanelForTap.localPosition;
			shopPanelForSwipePos = shopPanelForSwipe.localPosition;
			SetSwitchingWeaponPanel();
			isMulti = Defs.isMulti;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				centerAnhor.SetActive(false);
			}
			isInet = Defs.isInet;
			aimUpSprite = aimUp.GetComponent<UISprite>();
			aimDownSprite = aimDown.GetComponent<UISprite>();
			aimRightSprite = aimRight.GetComponent<UISprite>();
			aimLeftSprite = aimLeft.GetComponent<UISprite>();
			aimCenterSprite = aimCenter.GetComponent<UISprite>();
			aimUpLeftSprite = aimUpLeft.GetComponent<UISprite>();
			aimDownLeftSprite = aimDownLeft.GetComponent<UISprite>();
			aimDownRightSprite = aimDownRight.GetComponent<UISprite>();
			aimUpRightSprite = aimUpRight.GetComponent<UISprite>();
			impactTween.gameObject.SetActive(false);
		}
	}

	public void ShowImpact()
	{
		impactTween.gameObject.SetActive(true);
		impactTween.Play(true);
		if (Defs.isSoundFX)
		{
			impactTween.GetComponent<UIPlaySound>().Play();
		}
	}

	public void SetSwipeWeaponPanelVisibility(bool visible)
	{
		swipeWeaponPanel.localPosition = (visible ? swipeWeaponPanelPos : (swipeWeaponPanelPos + new Vector3(10000f, 0f, 0f)));
	}

	public void SetSwitchingWeaponPanel()
	{
		if (GlobalGameController.switchingWeaponSwipe)
		{
			SetSwipeWeaponPanelVisibility(!GameConnect.isDeathEscape && !GameConnect.isSpeedrun);
			sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
			sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(true);
			return;
		}
		sharedInGameGUI.swipeWeaponPanel.localPosition = new Vector3(10000f, sharedInGameGUI.swipeWeaponPanel.localPosition.y, sharedInGameGUI.swipeWeaponPanel.localPosition.z);
		sharedInGameGUI.shopPanelForTap.gameObject.SetActive(!GameConnect.isDeathEscape && !GameConnect.isSpeedrun);
		sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
		for (int i = 0; i < sharedInGameGUI.upButtonsInShopPanel.Length; i++)
		{
			if (!PotionsController.sharedController.PotionIsActive(sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myPotion.name))
			{
				sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myLabelTime.gameObject.SetActive(false);
			}
		}
	}

	public void AddDamageTaken(float alpha)
	{
		curDamageTakenController++;
		if (curDamageTakenController >= damageTakenControllers.Length)
		{
			curDamageTakenController = 0;
		}
		damageTakenControllers[curDamageTakenController].reset(alpha);
	}

	public void ResetDamageTaken()
	{
		for (int i = 0; i < damageTakenControllers.Length; i++)
		{
			damageTakenControllers[i].Remove();
		}
	}

	private void AdjustToPlayerHands()
	{
		float num = (GlobalGameController.LeftHanded ? 1 : (-1));
		Vector3[] array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (array == null || array.Length < 7)
		{
			Defs.InitCoordsIphone();
			zoomButton.transform.localPosition = new Vector3((float)Defs.ZoomButtonX * num, Defs.ZoomButtonY, zoomButton.transform.localPosition.z);
			reloadButton.transform.localPosition = new Vector3((float)Defs.ReloadButtonX * num, Defs.ReloadButtonY, reloadButton.transform.localPosition.z);
			jumpButton.transform.localPosition = new Vector3((float)Defs.JumpButtonX * num, Defs.JumpButtonY, jumpButton.transform.localPosition.z);
			fireButton.transform.localPosition = new Vector3((float)Defs.FireButtonX * num, Defs.FireButtonY, fireButton.transform.localPosition.z);
			joystick.transform.localPosition = new Vector3((float)Defs.JoyStickX * num, Defs.JoyStickY, joystick.transform.localPosition.z);
			grenadeButton.transform.localPosition = new Vector3((float)Defs.GrenadeX * num, Defs.GrenadeY, grenadeButton.transform.localPosition.z);
			chooseGadgetPanel.transform.localPosition = grenadeButton.transform.localPosition;
			fireButtonInJoystick.transform.localPosition = new Vector3((float)Defs.FireButton2X * num, Defs.FireButton2Y, fireButtonInJoystick.transform.localPosition.z);
		}
		else
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].x *= num;
			}
			zoomButton.transform.localPosition = array[0];
			reloadButton.transform.localPosition = array[1];
			jumpButton.transform.localPosition = array[2];
			fireButton.transform.localPosition = array[3];
			joystick.transform.localPosition = array[4];
			grenadeButton.transform.localPosition = array[5];
			chooseGadgetPanel.transform.localPosition = grenadeButton.transform.localPosition;
			fireButtonInJoystick.transform.localPosition = array[6];
		}
		UISprite[] array2 = new GameObject[8] { zoomButton, reloadButton, jumpButton, fireButton, joystick, grenadeButton, fireButtonInJoystick, bottomPanel }.Select((GameObject go) => go.GetComponent<UISprite>()).ToArray();
		List<object> list = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]")) as List<object>;
		if (list == null)
		{
			list = new List<object>(array2.Length);
			UnityEngine.Debug.LogWarning(list.GetType().FullName);
		}
		int[] array3 = list.Select(Convert.ToInt32).ToArray();
		if (array3.Length == 0)
		{
			array3 = Defs.controlButtonSizes;
		}
		int num2 = Math.Min(array3.Length, array2.Length);
		for (int j = 0; j != num2; j++)
		{
			int num3 = array3[j];
			if (num3 <= 0)
			{
				continue;
			}
			UISprite uISprite = array2[j];
			if (uISprite == null)
			{
				continue;
			}
			array2[j].keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
			array2[j].width = num3;
			if (uISprite.gameObject == grenadeButton)
			{
				chooseGadgetPanel.GetComponent<ChooseGadgetPanel>().gadgetButtonScript.cachedSprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
				chooseGadgetPanel.GetComponent<ChooseGadgetPanel>().gadgetButtonScript.cachedSprite.width = num3;
			}
			if (uISprite.gameObject == joystick)
			{
				UIJoystick component = uISprite.GetComponent<UIJoystick>();
				if (!(component == null))
				{
					float num4 = component.radius / 144f;
					component.ActualRadius = num4 * (float)num3;
				}
			}
		}
	}

	private void Start()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			SetSwipeWeaponPanelVisibility(false);
		}
		bool flag = !Device.isPixelGunLow && !GameConnect.isDuel && !GameConnect.isDeathEscape && !GameConnect.isSpleef && !GameConnect.isSpeedrun;
		shopView.HideButtons = !flag;
		shopView.UpdateView();
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		if (!Defs.isMulti && GameConnect.isCampaign)
		{
			CampaignContainer.SetActive(true);
		}
		if (!Defs.isMulti && GameConnect.isSurvival)
		{
			survivalContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.Deathmatch)
		{
			deathmatchContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.Dater)
		{
			daterContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.Duel)
		{
			topPanelsTapReceiver.SetActive(false);
			duelContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.TimeBattle)
		{
			timeBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.TeamFight)
		{
			teamBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
		{
			flagCaptureContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.DeadlyGames)
		{
			deadlygamesContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			CapturePointContainer.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.DeathEscape)
		{
			deathEscapeContainer.SetActive(true);
			deathEscapeUI.gameObject.SetActive(true);
		}
		if (Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.Spleef)
		{
			deathEscapeContainer.SetActive(true);
		}
		if (!Defs.isMulti && GameConnect.gameMode == GameConnect.GameMode.SpeedRun)
		{
			speedrunContainer.SetActive(true);
			runnerShieldBuyButton.onClick.Add(new EventDelegate(BuyShieldButtonClick));
		}
		turretPanel.SetActive(false);
		if (runTurrelButton != null)
		{
			runTurrelButton.Clicked += RunTurret;
		}
		if (cancelTurrelButton != null)
		{
			cancelTurrelButton.Clicked += CancelTurret;
		}
		if (isMulti)
		{
			enemiesLeftLabel.SetActive(false);
		}
		else
		{
			zombieCreator = ZombieCreator.sharedCreator;
		}
		AdjustToPlayerHands();
		PauseNGUIController.PlayerHandUpdated += AdjustToPlayerHands;
		PauseNGUIController.SwitchingWeaponsUpdated += SetSwitchingWeaponPanel;
		WeaponManager.WeaponEquipped += HandleWeaponEquipped;
		int num;
		if (!isMulti)
		{
			num = WeaponManager.sharedManager.CurrentWeaponIndex;
		}
		else if (GameConnect.isSpleef)
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
			num = WeaponManager.sharedManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons();
		}
		HandleWeaponEquipped(((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>());
		if (num < changeWeaponWrap.transform.childCount)
		{
			changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
			changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(changeWeaponWrap.transform.GetChild(num));
			changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
		}
		else
		{
			UnityEngine.Debug.LogError("InGameGUI: not weapon icon with index " + (((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1));
		}
		if (gearToogle != null)
		{
			gearToogle.gameObject.GetComponent<ButtonHandler>().Clicked += HandleGearToogleClicked;
		}
		if (weaponCategoriesButtons[0] != null)
		{
			weaponCategoriesButtons[0].gameObject.GetComponent<ButtonHandler>().Clicked += HandlePrimaryToogleClicked;
		}
		if (weaponCategoriesButtons[1] != null)
		{
			weaponCategoriesButtons[1].gameObject.GetComponent<ButtonHandler>().Clicked += HandleBackupToogleClicked;
		}
		if (weaponCategoriesButtons[2] != null)
		{
			weaponCategoriesButtons[2].gameObject.GetComponent<ButtonHandler>().Clicked += HandleMeleeToogleClicked;
		}
		if (weaponCategoriesButtons[3] != null)
		{
			weaponCategoriesButtons[3].gameObject.GetComponent<ButtonHandler>().Clicked += HandleSpecialToogleClicked;
		}
		if (weaponCategoriesButtons[4] != null)
		{
			weaponCategoriesButtons[4].gameObject.GetComponent<ButtonHandler>().Clicked += HandleSniperToogleClicked;
		}
		if (weaponCategoriesButtons[5] != null)
		{
			weaponCategoriesButtons[5].gameObject.GetComponent<ButtonHandler>().Clicked += HandlePremiumToogleClicked;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			gearToogle.GetComponent<UIToggle>().value = false;
			HandleGearToogleClicked(null, null);
		}
		for (int i = 0; i < upButtonsInShopPanel.Length; i++)
		{
			StartUpdatePotionButton(upButtonsInShopPanel[i]);
		}
		for (int j = 0; j < upButtonsInShopPanelSwipeRegim.Length; j++)
		{
			StartUpdatePotionButton(upButtonsInShopPanelSwipeRegim[j]);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			fastShopPanel.transform.localPosition = new Vector3(-1000f, -1000f, -1f);
			gearToogle.isEnabled = false;
		}
		if (GameConnect.isDeathEscape && CheckpointController.instance != null && CheckpointController.instance.finishedLap)
		{
			CheckpointController.instance.ShowLapFinishedInterface();
		}
		SetNGUITouchDragThreshold(1f);
	}

	private void BuyShieldButtonClick()
	{
		SpeedrunTrackController.instance.TryBuyShield();
	}

	public void ShowTurretInterface(string nameTurret)
	{
		if (!turretPanel.activeSelf)
		{
			isTurretInterfaceActive = true;
			swipeWeaponPanel.gameObject.SetActive(false);
			shopPanelForSwipe.gameObject.SetActive(false);
			shopPanelForTap.gameObject.SetActive(false);
			runTurrelButton.GetComponent<UIButton>().isEnabled = false;
			turretPanel.SetActive(true);
			if (playerMoveC != null)
			{
				playerMoveC.turretGadgetPrefabName = nameTurret;
			}
			playerMoveC.ChangeWeapon(1001, false);
			_kBlockPauseShopButton = true;
		}
	}

	public void HideTurretInterface()
	{
		isTurretInterfaceActive = false;
		if (GlobalGameController.switchingWeaponSwipe)
		{
			shopPanelForSwipe.gameObject.SetActive(true);
		}
		else
		{
			shopPanelForTap.gameObject.SetActive(true);
		}
		swipeWeaponPanel.gameObject.SetActive(true);
		turretPanel.SetActive(false);
		_kBlockPauseShopButton = false;
	}

	private void RunTurret(object sender, EventArgs e)
	{
		if (playerMoveC != null)
		{
			playerMoveC.RunTurret();
		}
		if (!GameConnect.isDaterRegim)
		{
			Gadget value = null;
			if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out value))
			{
				(value as TurretGadget).StartedCurrentTurret(playerMoveC.currentTurret.GetComponent<TurretController>());
			}
		}
		HideTurretInterface();
	}

	private void CancelTurret(object sender, EventArgs e)
	{
		if (playerMoveC != null)
		{
			playerMoveC.CancelTurret();
		}
		HideTurretInterface();
	}

	public void ShowLapFinishedInterface(int curLap, int bestLap, int recordLap, bool newRecord)
	{
		SetLapFinishedInterfaceVisible(true);
		lapFinishButton.onClick.Clear();
		newLapButton.onClick.Clear();
		lapFinishButton.onClick.Add(new EventDelegate(DeathEscape_FinishButtonClick));
		newLapButton.onClick.Add(new EventDelegate(DeathEscape_NewLapButtonClick));
		currentLapTime.text = DeathEscapeUI.FormatTime(curLap);
		bestLapTime.text = DeathEscapeUI.FormatTime(bestLap);
		recordLapTime.text = DeathEscapeUI.FormatTime(recordLap);
		noRecordLabel.gameObject.SetActive(!newRecord);
		recordLabel.gameObject.SetActive(newRecord);
	}

	private void SetLapFinishedInterfaceVisible(bool visible)
	{
		WeaponManager.sharedManager.myPlayerMoveC.SetPlayerPause(visible);
		interfacePanel.SetActive(!visible);
		panelFinishLap.SetActive(visible);
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		if (visible)
		{
			_backSubscription = BackSystem.Instance.Register(DeathEscape_FinishButtonClick, "Death Escape finish");
		}
	}

	private void DeathEscape_FinishButtonClick()
	{
		MiniGamesController.Instance.FinishGame();
		SetLapFinishedInterfaceVisible(false);
		AnalyticsStuff.DeathEscapeRestart("Rejected");
	}

	public void DeathEscape_NewLapButtonClick()
	{
		CheckpointController.instance.StartNewLap();
		SetLapFinishedInterfaceVisible(false);
		AnalyticsStuff.DeathEscapeRestart("Accepted");
	}

	private void StartUpdatePotionButton(GameObject potionButton)
	{
		if (potionButton != null)
		{
			potionButton.gameObject.GetComponent<ButtonHandler>().Clicked += HandlePotionClicked;
			ElexirInGameButtonController component = potionButton.GetComponent<ElexirInGameButtonController>();
			string text = component.myPotion.name;
			string key = (GameConnect.isDaterRegim ? GearManager.HolderQuantityForID(component.idForPriceInDaterRegim) : text);
			if (PotionsController.sharedController.PotionIsActive(text))
			{
				UIButton component2 = potionButton.GetComponent<UIButton>();
				component.isActivePotion = true;
				component.myLabelTime.gameObject.SetActive(true);
				component.myLabelTime.enabled = true;
				component.priceLabel.SetActive(false);
				component.myLabelCount.gameObject.SetActive(true);
				component.plusSprite.SetActive(false);
				component.myLabelCount.text = Storager.getInt(key).ToString();
				component2.isEnabled = false;
			}
		}
	}

	public void HandleBuyGrenadeClicked(object sender, EventArgs e)
	{
		if (!GameConnect.isDaterRegim)
		{
			return;
		}
		GearManager.AnalyticsIDForOneItemOfGear(GearManager.Like, true);
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear("LikeID", GearManager.CurrentNumberOfUphradesForGear("LikeID")), ShopNGUIController.CategoryNames.GearCategory);
		ItemPrice itemPrice = new ItemPrice(priceByShopId.Price, priceByShopId.Currency);
		int priceAmount = itemPrice.Price;
		string priceCurrency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, itemPrice, delegate
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount++;
			}
			AnalyticsStuff.LogSales(GearManager.Like, "Gear");
			AnalyticsFacade.InAppPurchase(GearManager.Like, "Gear", 1, priceAmount, priceCurrency);
		}, JoystickController.leftJoystick.Reset);
	}

	private void ClickPotionButton(int index)
	{
		timerShowPotion = timerShowPotionMax;
		ElexirInGameButtonController myController = upButtonsInShopPanel[index].GetComponent<ElexirInGameButtonController>();
		ElexirInGameButtonController myController2 = upButtonsInShopPanelSwipeRegim[index].GetComponent<ElexirInGameButtonController>();
		UIButton myButton = upButtonsInShopPanel[index].GetComponent<UIButton>();
		UIButton myButton2 = upButtonsInShopPanelSwipeRegim[index].GetComponent<UIButton>();
		string text = myController.myPotion.name;
		string myStaragerKey = (GameConnect.isDaterRegim ? GearManager.HolderQuantityForID(myController.idForPriceInDaterRegim) : text);
		if (Storager.getInt(myStaragerKey) > 0)
		{
			if (text.Equals(GearManager.Turret))
			{
				ShowTurretInterface("MusicBox");
			}
			else
			{
				if (GameConnect.isDaterRegim)
				{
					Storager.setInt(myStaragerKey, Storager.getInt(myStaragerKey) - 1);
				}
				PotionsController.sharedController.ActivatePotion(text, playerMoveC, new Dictionary<string, object>());
			}
			string text2 = Storager.getInt(myStaragerKey).ToString();
			myController.myLabelCount.gameObject.SetActive(true);
			myController.plusSprite.SetActive(false);
			myController.myLabelCount.text = text2;
			myController.isActivePotion = true;
			myButton.isEnabled = false;
			myController.myLabelTime.enabled = true;
			myController.myLabelTime.gameObject.SetActive(true);
			myController2.myLabelCount.gameObject.SetActive(true);
			myController2.plusSprite.SetActive(false);
			myController2.myLabelCount.text = text2;
			myController2.isActivePotion = true;
			myButton2.isEnabled = false;
			myController2.myLabelTime.enabled = true;
			myController2.myLabelTime.gameObject.SetActive(true);
			return;
		}
		GearManager.AnalyticsIDForOneItemOfGear(myStaragerKey ?? "Potion", true);
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear(myStaragerKey, GearManager.CurrentNumberOfUphradesForGear(myStaragerKey)), ShopNGUIController.CategoryNames.GearCategory);
		int priceAmount = priceByShopId.Price;
		string priceCurrency = priceByShopId.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, priceByShopId, delegate
		{
			Storager.setInt(myStaragerKey, Storager.getInt(myStaragerKey) + 1);
			myButton.normalSprite = "game_clear";
			myButton.pressedSprite = "game_clear_n";
			myController.myLabelCount.gameObject.SetActive(true);
			myController.plusSprite.SetActive(false);
			myController.priceLabel.SetActive(false);
			myController.myLabelCount.text = Storager.getInt(myStaragerKey).ToString();
			myButton2.normalSprite = "game_clear";
			myButton2.pressedSprite = "game_clear_n";
			myController2.myLabelCount.gameObject.SetActive(true);
			myController2.plusSprite.SetActive(false);
			myController2.priceLabel.SetActive(false);
			myController2.myLabelCount.text = Storager.getInt(myStaragerKey).ToString();
			if (myStaragerKey != null)
			{
				AnalyticsStuff.LogSales(GearManager.HolderQuantityForID(myStaragerKey), "Gear");
				AnalyticsFacade.InAppPurchase(GearManager.HolderQuantityForID(myStaragerKey), "Gear", 1, priceAmount, priceCurrency);
			}
		}, JoystickController.leftJoystick.Reset);
	}

	private void HandlePotionClicked(object sender, EventArgs e)
	{
		int index = 0;
		for (int i = 0; i < upButtonsInShopPanel.Length; i++)
		{
			if (upButtonsInShopPanel[i].name.Equals(((ButtonHandler)sender).gameObject.name))
			{
				index = i;
				break;
			}
		}
		ClickPotionButton(index);
	}

	private void HandleGearToogleClicked(object sender, EventArgs e)
	{
		bool value = gearToogle.GetComponent<UIToggle>().value;
		fonBig.SetActive(value);
		if (value)
		{
			timerShowPotion = timerShowPotionMax;
		}
		else
		{
			timerShowPotion = -1f;
		}
		for (int i = 0; i < upButtonsInShopPanel.Length; i++)
		{
			upButtonsInShopPanel[i].SetActive(value);
		}
	}

	private void HandlePrimaryToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(1);
	}

	private void HandleBackupToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(2);
	}

	private void HandleMeleeToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(3);
	}

	private void HandleSpecialToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(4);
	}

	private void HandleSniperToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(5);
	}

	private void HandlePremiumToogleClicked(object sender, EventArgs e)
	{
		SelectWeaponFromCategory(6);
	}

	private void SelectWeaponFromCategory(int category, bool isUpdateSwipe = true)
	{
		for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
		{
			if (((Weapon)WeaponManager.sharedManager.playerWeapons[i]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == category)
			{
				SelectWeaponFromIndex(i, isUpdateSwipe);
				break;
			}
		}
	}

	private void SelectWeaponFromIndex(int _index, bool updateSwipe = true)
	{
		bool[] array = new bool[6];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = false;
		}
		int num = 0;
		foreach (Weapon playerWeapon in WeaponManager.sharedManager.playerWeapons)
		{
			int num2 = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			array[num2] = true;
			num++;
		}
		for (int j = 0; j < weaponCategoriesButtons.Length; j++)
		{
			weaponCategoriesButtons[j].isEnabled = array[j];
			if (j == ((Weapon)WeaponManager.sharedManager.playerWeapons[_index]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1)
			{
				weaponCategoriesButtons[j].GetComponent<UIToggle>().value = true;
			}
			else
			{
				weaponCategoriesButtons[j].GetComponent<UIToggle>().value = false;
			}
		}
		SetChangeWeapon(_index, updateSwipe);
	}

	private void SetChangeWeapon(int index, bool isUpdateSwipe)
	{
		if (isUpdateSwipe)
		{
			if (index < changeWeaponWrap.transform.childCount)
			{
				changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
				changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(changeWeaponWrap.transform.GetChild(index));
				changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
			}
			else
			{
				UnityEngine.Debug.LogError("InGameGUI: not weapon icon with index " + index);
			}
		}
		if (WeaponManager.sharedManager.CurrentWeaponIndex == index)
		{
			return;
		}
		WeaponManager.sharedManager.CurrentWeaponIndex = index;
		WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
		if (playerMoveC != null)
		{
			if (playerMoveC.currentWeaponBeforeTurret >= 0)
			{
				playerMoveC.currentWeaponBeforeTurret = index;
				return;
			}
			if (playerMoveC.CurrentWeaponBeforeGrenade > -1)
			{
				playerMoveC.CurrentWeaponBeforeGrenade = index;
				return;
			}
			playerMoveC.ChangeWeapon(index, false);
			playerMoveC.HideChangeWeaponTrainingHint();
		}
	}

	private void CheckWeaponScrollChanged()
	{
		if (!_disabled)
		{
			if (changeWeaponScroll.transform.localPosition.x > 0f)
			{
				weaponIndexInScroll = Mathf.RoundToInt((changeWeaponScroll.transform.localPosition.x - (float)(Mathf.FloorToInt(changeWeaponScroll.transform.localPosition.x / (float)widthWeaponScrollPreview / (float)changeWeaponWrap.transform.childCount) * widthWeaponScrollPreview * changeWeaponWrap.transform.childCount)) / (float)widthWeaponScrollPreview);
				weaponIndexInScroll = changeWeaponWrap.transform.childCount - weaponIndexInScroll;
			}
			else
			{
				weaponIndexInScroll = -1 * Mathf.RoundToInt((changeWeaponScroll.transform.localPosition.x - (float)(Mathf.CeilToInt(changeWeaponScroll.transform.localPosition.x / (float)widthWeaponScrollPreview / (float)changeWeaponWrap.transform.childCount) * widthWeaponScrollPreview * changeWeaponWrap.transform.childCount)) / (float)widthWeaponScrollPreview);
			}
			if (weaponIndexInScroll == changeWeaponWrap.transform.childCount)
			{
				weaponIndexInScroll = 0;
			}
			if (weaponIndexInScroll != weaponIndexInScrollOld)
			{
				SelectWeaponFromCategory(((Weapon)WeaponManager.sharedManager.playerWeapons[weaponIndexInScroll]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor, false);
			}
			weaponIndexInScrollOld = weaponIndexInScroll;
		}
	}

	public IEnumerator _DisableSwiping(float tm)
	{
		MyCenterOnChild _center = changeWeaponWrap.GetComponent<MyCenterOnChild>();
		int bef;
		if (_center == null || _center.centeredObject == null || !int.TryParse(_center.centeredObject.name.Replace("WeaponCat_", string.Empty), out bef))
		{
			yield break;
		}
		_disabled = true;
		yield return new WaitForSeconds(tm);
		_disabled = false;
		if (_center.centeredObject == null || _center.centeredObject.name.Equals("WeaponCat_" + bef))
		{
			yield break;
		}
		Transform transform = null;
		foreach (Transform item in _center.transform)
		{
			if (item.gameObject.name.Equals("WeaponCat_" + bef))
			{
				transform = item;
				break;
			}
		}
		if (transform != null)
		{
			_center.CenterOn(transform);
		}
	}

	private void UpdateCrosshairPositions()
	{
		if (playerMoveC == null || !playerMoveC.isMechActive)
		{
			float num = WeaponManager.sharedManager.currentWeaponSounds.tekKoof * WeaponManager.sharedManager.currentWeaponSounds.startZone.y * 0.5f;
			aimDown.transform.localPosition = new Vector3(0f, 0f - aimPositions[1].y - num, 0f);
			aimUp.transform.localPosition = new Vector3(0f, aimPositions[2].y + num, 0f);
			aimLeft.transform.localPosition = new Vector3(0f - aimPositions[3].x - num, 0f, 0f);
			aimDownLeft.transform.localPosition = new Vector3(0f - aimPositions[4].x - num, 0f - aimPositions[4].y - num, 0f);
			aimUpLeft.transform.localPosition = new Vector3(0f - aimPositions[5].x - num, aimPositions[5].y + num, 0f);
			aimRight.transform.localPosition = new Vector3(aimPositions[3].x + num, 0f, 0f);
			aimDownRight.transform.localPosition = new Vector3(aimPositions[4].x + num, 0f - aimPositions[4].y - num, 0f);
			aimUpRight.transform.localPosition = new Vector3(aimPositions[5].x + num, aimPositions[5].y + num, 0f);
		}
		else
		{
			float num = 12f + playerMoveC.mechWeaponSounds.tekKoof * playerMoveC.mechWeaponSounds.startZone.y * 0.5f;
			aimUp.transform.localPosition = new Vector3(0f, num, 0f);
			aimUpRight.transform.localPosition = new Vector3(num, num, 0f);
			aimRight.transform.localPosition = new Vector3(num, 0f, 0f);
			aimDownRight.transform.localPosition = new Vector3(num, 0f - num, 0f);
			aimDown.transform.localPosition = new Vector3(0f, 0f - num, 0f);
			aimDownLeft.transform.localPosition = new Vector3(0f - num, 0f - num, 0f);
			aimLeft.transform.localPosition = new Vector3(0f - num, 0f, 0f);
			aimUpLeft.transform.localPosition = new Vector3(0f - num, num, 0f);
		}
	}

	private void SetCrosshairVisibility(bool visible)
	{
		if (crosshairVisible != visible)
		{
			crosshairVisible = visible;
			aimCenterSprite.enabled = visible;
			aimDownSprite.enabled = visible;
			aimUpSprite.enabled = visible;
			aimLeftSprite.enabled = visible;
			aimRightSprite.enabled = visible;
			aimUpLeftSprite.enabled = visible;
			aimUpRightSprite.enabled = visible;
			aimDownLeftSprite.enabled = visible;
			aimDownRightSprite.enabled = visible;
		}
	}

	private void SetCrosshairPart(UISprite sprite, CrosshairData.aimSprite param, bool mirror = false)
	{
		if (!string.IsNullOrEmpty(param.spriteName))
		{
			sprite.gameObject.SetActive(true);
			sprite.spriteName = param.spriteName;
			sprite.width = Mathf.RoundToInt(param.spriteSize.x);
			sprite.height = Mathf.RoundToInt(param.spriteSize.y);
			sprite.transform.localPosition = (mirror ? new Vector2(param.offset.x, 0f - param.offset.y) : param.offset);
		}
		else
		{
			sprite.gameObject.SetActive(false);
		}
	}

	public void SetCrosshair(WeaponSounds weaponSounds)
	{
		WeaponCustomCrosshair component = weaponSounds.GetComponent<WeaponCustomCrosshair>();
		if (component != null)
		{
			SetCrosshairPart(aimCenterSprite, component.Data.center);
			SetCrosshairPart(aimDownSprite, component.Data.down);
			SetCrosshairPart(aimUpSprite, component.Data.up);
			SetCrosshairPart(aimLeftSprite, component.Data.left);
			SetCrosshairPart(aimRightSprite, component.Data.left, true);
			SetCrosshairPart(aimUpLeftSprite, component.Data.leftUp);
			SetCrosshairPart(aimUpRightSprite, component.Data.leftUp, true);
			SetCrosshairPart(aimDownLeftSprite, component.Data.leftDown);
			SetCrosshairPart(aimDownRightSprite, component.Data.leftDown, true);
			aimPositions[0] = component.Data.center.offset;
			aimPositions[1] = component.Data.down.offset;
			aimPositions[2] = component.Data.up.offset;
			aimPositions[3] = component.Data.left.offset;
			aimPositions[4] = component.Data.leftDown.offset;
			aimPositions[5] = component.Data.leftUp.offset;
		}
		else
		{
			SetCrosshairPart(aimCenterSprite, defaultAimCenter);
			SetCrosshairPart(aimDownSprite, defaultAimDown);
			SetCrosshairPart(aimUpSprite, defaultAimUp);
			SetCrosshairPart(aimLeftSprite, defaultAimLeftCenter);
			SetCrosshairPart(aimRightSprite, defaultAimLeftCenter, true);
			SetCrosshairPart(aimUpLeftSprite, defaultAimLeftUp);
			SetCrosshairPart(aimUpRightSprite, defaultAimLeftUp, true);
			SetCrosshairPart(aimDownLeftSprite, defaultAimLeftDown);
			SetCrosshairPart(aimDownRightSprite, defaultAimLeftDown, true);
			aimPositions[0] = defaultAimCenter.offset;
			aimPositions[1] = defaultAimDown.offset;
			aimPositions[2] = defaultAimUp.offset;
			aimPositions[3] = defaultAimLeftCenter.offset;
			aimPositions[4] = defaultAimLeftDown.offset;
			aimPositions[5] = defaultAimLeftUp.offset;
		}
		UpdateCrosshairPositions();
	}

	private string FormatMeleeAmmoLabel(int index, int currentAmmoInClip, int currentAmmoInBackpack)
	{
		if (_formatMeleeAmmoMemo == null || _formatMeleeAmmoMemo.Length < ammoCategoriesLabels.Length)
		{
			_formatMeleeAmmoMemo = new KeyValuePair<int, string>[ammoCategoriesLabels.Length];
		}
		int num = currentAmmoInClip + currentAmmoInBackpack;
		if (num != _formatMeleeAmmoMemo[index].Key)
		{
			string value = num.ToString(CultureInfo.InvariantCulture);
			_formatMeleeAmmoMemo[index] = new KeyValuePair<int, string>(num, value);
		}
		return _formatMeleeAmmoMemo[index].Value ?? "0";
	}

	private string FormatShootingAmmoLabel(int index, int currentAmmoInClip, int currentAmmoInBackpack, int fullClipAmmo)
	{
		if (_formatShootingAmmoMemo == null || _formatShootingAmmoMemo.Length < ammoCategoriesLabels.Length)
		{
			_formatShootingAmmoMemo = new KeyValuePair<Ammo, string>[ammoCategoriesLabels.Length];
		}
		Ammo key = new Ammo(currentAmmoInClip, GameConnect.isSpleef ? fullClipAmmo : currentAmmoInBackpack);
		if (!key.Equals(_formatShootingAmmoMemo[index].Key))
		{
			_stringBuilder.Length = 0;
			_stringBuilder.Append(currentAmmoInClip).Append("/").Append(GameConnect.isSpleef ? fullClipAmmo : currentAmmoInBackpack);
			string value = _stringBuilder.ToString();
			_stringBuilder.Length = 0;
			_formatShootingAmmoMemo[index] = new KeyValuePair<Ammo, string>(key, value);
		}
		return _formatShootingAmmoMemo[index].Value ?? "0/0";
	}

	private void Update()
	{
		CheckWeaponScrollChanged();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (timerBlinkNoAmmo > 0f)
		{
			timerBlinkNoAmmo -= Time.deltaTime;
		}
		if (timerBlinkNoAmmo > 0f && playerMoveC != null && !playerMoveC.isMechActive)
		{
			blinkNoAmmoLabel.gameObject.SetActive(true);
			float num = timerBlinkNoAmmo % periodBlink / periodBlink;
			blinkNoAmmoLabel.color = new Color(blinkNoAmmoLabel.color.r, blinkNoAmmoLabel.color.g, blinkNoAmmoLabel.color.b, (num < 0.5f) ? (num * 2f) : ((1f - num) * 2f));
		}
		if ((timerBlinkNoAmmo < 0f || (playerMoveC != null && playerMoveC.isMechActive)) && blinkNoAmmoLabel.gameObject.activeSelf)
		{
			blinkNoAmmoLabel.gameObject.SetActive(false);
		}
		if (playerMoveC != null && !GameConnect.isSpeedrun && !GameConnect.isDeathEscape)
		{
			int num2 = Mathf.FloorToInt(playerMoveC.CurHealth);
			if (num2 < oldCountHeath && timerBlinkNoHeath < 0f && num2 < 3)
			{
				timerBlinkNoHeath = periodBlink * 3f;
			}
			if (num2 > 2)
			{
				timerBlinkNoHeath = -1f;
			}
			oldCountHeath = num2;
			if (timerBlinkNoHeath > 0f)
			{
				timerBlinkNoHeath -= Time.deltaTime;
			}
			if (timerBlinkNoHeath > 0f && !playerMoveC.isMechActive)
			{
				if (num2 > 0)
				{
					PlayLowResourceBeepIfNotPlaying(1);
				}
				blinkNoHeathLabel.gameObject.SetActive(true);
				float num3 = timerBlinkNoHeath % periodBlink / periodBlink;
				float a = ((num3 < 0.5f) ? (num3 * 2f) : ((1f - num3) * 2f));
				blinkNoHeathLabel.color = new Color(blinkNoHeathLabel.color.r, blinkNoHeathLabel.color.g, blinkNoHeathLabel.color.b, a);
				for (int i = 0; i < blinkNoHeathFrames.Length; i++)
				{
					blinkNoHeathFrames[i].gameObject.SetActive(true);
					blinkNoHeathFrames[i].color = new Color(1f, 1f, 1f, a);
				}
			}
		}
		if ((timerBlinkNoHeath < 0f || playerMoveC == null || (playerMoveC != null && playerMoveC.isMechActive)) && blinkNoHeathLabel.gameObject.activeSelf)
		{
			blinkNoHeathLabel.gameObject.SetActive(false);
			for (int j = 0; j < blinkNoHeathFrames.Length; j++)
			{
				blinkNoHeathFrames[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < ammoCategoriesLabels.Length; k++)
		{
			if (!(ammoCategoriesLabels[k] != null))
			{
				continue;
			}
			bool flag = false;
			if (weaponCategoriesButtons[k].isEnabled)
			{
				for (int l = 0; l < WeaponManager.sharedManager.playerWeapons.Count; l++)
				{
					Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[l];
					WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
					if ((!component.isMelee || component.isShotMelee) && component.categoryNabor == k + 1)
					{
						ammoCategoriesLabels[k].text = (component.isShotMelee ? FormatMeleeAmmoLabel(k, weapon.currentAmmoInClip, weapon.currentAmmoInBackpack) : FormatShootingAmmoLabel(k, weapon.currentAmmoInClip, weapon.currentAmmoInBackpack, component.ammoInClip));
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				ammoCategoriesLabels[k].text = string.Empty;
			}
		}
		if (timerShowNow > 0f)
		{
			timerShowNow -= Time.deltaTime;
			if (!message_now.activeSelf)
			{
				message_now.SetActive(true);
			}
		}
		else if (message_now.activeSelf)
		{
			message_now.SetActive(false);
		}
		if (isMulti && playerMoveC == null && WeaponManager.sharedManager.myPlayer != null)
		{
			playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (!isMulti && playerMoveC == null)
		{
			playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (isMulti && playerMoveC != null)
		{
			for (int m = 0; m < 3; m++)
			{
				messageAddScore[m].GetComponent<UIPlaySound>().volume = (Defs.isSoundFX ? 1 : 0);
				float num4 = 0.3f;
				float num5 = 0.2f;
				if (m == 0)
				{
					float num6 = 1f;
					if (playerMoveC.myScoreController.maxTimerSumMessage - playerMoveC.myScoreController.timerAddScoreShow[m] < num4)
					{
						num6 = 1f + num5 * (playerMoveC.myScoreController.maxTimerSumMessage - playerMoveC.myScoreController.timerAddScoreShow[m]) / num4;
					}
					if (playerMoveC.myScoreController.maxTimerSumMessage - playerMoveC.myScoreController.timerAddScoreShow[m] - num4 < num4)
					{
						num6 = 1f + num5 * (1f - (playerMoveC.myScoreController.maxTimerSumMessage - playerMoveC.myScoreController.timerAddScoreShow[m] - num4) / num4);
					}
					messageAddScore[m].transform.localScale = new Vector3(num6, num6, num6);
				}
				if (playerMoveC.timerShow[m] > 0f)
				{
					killLabels[m].gameObject.SetActive(true);
					killLabels[m].SetChatLabelText(playerMoveC.killedSpisok[m]);
				}
				else
				{
					killLabels[m].gameObject.SetActive(false);
				}
				if (playerMoveC.myScoreController.timerAddScoreShow[m] > 0f)
				{
					if (!messageAddScore[m].gameObject.activeSelf)
					{
						messageAddScore[m].gameObject.SetActive(true);
					}
					messageAddScore[m].text = playerMoveC.myScoreController.addScoreString[m];
					messageAddScore[m].color = new Color(1f, 1f, 1f, (playerMoveC.myScoreController.timerAddScoreShow[m] > 1f) ? 1f : playerMoveC.myScoreController.timerAddScoreShow[m]);
				}
				else if (messageAddScore[m].gameObject.activeSelf)
				{
					messageAddScore[m].gameObject.SetActive(false);
				}
			}
			if (isChatOn)
			{
				int num7 = 0;
				int num8 = playerMoveC.messages.Count - 1;
				while (num8 >= 0 && playerMoveC.messages.Count - num8 - 1 < 3)
				{
					if (Time.time - playerMoveC.messages[num8].time < 10f)
					{
						if (isInet && playerMoveC.messages[num8].ID == WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID)
						{
							chatLabels[num7].GetComponent<UILabel>().color = new Color(0f, 1f, 0.15f, 1f);
						}
						else
						{
							if (playerMoveC.messages[num8].command == 0)
							{
								chatLabels[num7].GetComponent<UILabel>().color = new Color(1f, 1f, 0.15f, 1f);
							}
							if (playerMoveC.messages[num8].command == 1)
							{
								chatLabels[num7].GetComponent<UILabel>().color = new Color(0f, 0f, 0.9f, 1f);
							}
							if (playerMoveC.messages[num8].command == 2)
							{
								chatLabels[num7].GetComponent<UILabel>().color = new Color(1f, 0f, 0f, 1f);
							}
						}
						ChatLabel component2 = chatLabels[num7].GetComponent<ChatLabel>();
						component2.nickLabel.text = playerMoveC.messages[num8].text;
						component2.iconSprite.spriteName = playerMoveC.messages[num8].iconName;
						Transform transform = component2.iconSprite.transform;
						transform.localPosition = new Vector3(component2.nickLabel.width + 20, transform.localPosition.y, transform.localPosition.z);
						component2.clanTexture.mainTexture = playerMoveC.messages[num8].clanLogo;
						chatLabels[num7].SetActive(true);
					}
					else
					{
						chatLabels[num7].SetActive(false);
					}
					num7++;
					num8--;
				}
				for (int n = num7; n < 3; n++)
				{
					chatLabels[num7].SetActive(false);
				}
			}
			if (timerShowScorePict > 0f)
			{
				timerShowScorePict -= Time.deltaTime;
			}
			if (GameConnect.isHunger && Initializer.players.Count == 2 && MiniGamesController.Instance.isGo && MiniGamesController.Instance.timeInGame > 10f)
			{
				duel.SetActive(true);
				multyKillPanel.gameObject.SetActive(false);
			}
			else
			{
				if (duel.activeSelf)
				{
					duel.SetActive(false);
				}
				if (timerShowScorePict > 0f)
				{
					if ((!multyKillPanel.gameObject.activeSelf || scorePictName != multyKillSprite.spriteName) && (PauseGUIController.Instance == null || !PauseGUIController.Instance.IsPaused))
					{
						multyKillSprite.spriteName = scorePictName;
						multyKillPanel.gameObject.SetActive(true);
						multyKillPanel.GetComponent<MultyKill>().PlayTween();
					}
				}
				else if (multyKillPanel.gameObject.activeSelf)
				{
					multyKillPanel.gameObject.SetActive(false);
				}
			}
			if (GameConnect.isMiniGame)
			{
				if (!MiniGamesController.Instance.isGo)
				{
					timerStartHungerLabel.gameObject.SetActive(true);
					int num9 = Mathf.FloorToInt(MiniGamesController.Instance.goTimer);
					string text;
					if (num9 == 0)
					{
						text = "GO!";
						timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
					}
					else
					{
						text = string.Concat(num9);
						timerStartHungerLabel.color = new Color(1f, 0f, 0f, 1f);
					}
					if (timerStartHungerLabel.text != text)
					{
						timerStartHungerLabel.GetComponent<UITweener>().PlayForward();
					}
					timerStartHungerLabel.text = text;
				}
				else if (MiniGamesController.Instance.isGo && MiniGamesController.Instance.isShowGo)
				{
					timerStartHungerLabel.gameObject.SetActive(true);
					timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
					timerStartHungerLabel.text = "GO!";
				}
				else
				{
					timerStartHungerLabel.gameObject.SetActive(false);
				}
			}
		}
		if (playerMoveC != null)
		{
			if (GameConnect.isSpeedrun)
			{
				speedrunSafeZone.SetActive(SpeedrunTrackController.instance.showSafeZone);
				if (!SpeedrunTrackController.instance.runStarted)
				{
					int num10 = Mathf.FloorToInt(SpeedrunTrackController.instance.goTimer);
					timerStartHungerLabel.gameObject.SetActive(true);
					string text2;
					if (num10 == 0)
					{
						text2 = "GO!";
						timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
					}
					else
					{
						text2 = string.Concat(num10);
						timerStartHungerLabel.color = new Color(1f, 0f, 0f, 1f);
					}
					if (timerStartHungerLabel.text != text2)
					{
						timerStartHungerLabel.GetComponent<UITweener>().PlayForward();
					}
					timerStartHungerLabel.text = text2;
				}
				else if (SpeedrunTrackController.instance.runStarted && SpeedrunTrackController.instance.isShowGo)
				{
					timerStartHungerLabel.gameObject.SetActive(true);
					timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
					timerStartHungerLabel.text = "GO!";
				}
				else
				{
					timerStartHungerLabel.gameObject.SetActive(false);
				}
			}
			if (playerMoveC.timerShowDown > 0f && playerMoveC.timerShowDown < playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				downBloodTexture.SetActive(true);
			}
			else
			{
				downBloodTexture.SetActive(false);
			}
			if (playerMoveC.timerShowUp > 0f && playerMoveC.timerShowUp < playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				upBloodTexture.SetActive(true);
			}
			else
			{
				upBloodTexture.SetActive(false);
			}
			if (playerMoveC.timerShowLeft > 0f && playerMoveC.timerShowLeft < playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				leftBloodTexture.SetActive(true);
			}
			else
			{
				leftBloodTexture.SetActive(false);
			}
			if (playerMoveC.timerShowRight > 0f && playerMoveC.timerShowRight < playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				rightBloodTexture.SetActive(true);
			}
			else
			{
				rightBloodTexture.SetActive(false);
			}
			if (!playerMoveC.isZooming && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != 0 || !TrainingController.isPressSkip))
			{
				SetCrosshairVisibility(true);
				UpdateCrosshairPositions();
			}
			else
			{
				SetCrosshairVisibility(false);
			}
		}
		string text3 = SceneManager.GetActiveScene().name;
		bool flag2 = true;
		if (text3 == Defs.TrainingSceneName)
		{
			flag2 = false;
		}
		bool flag3 = flag2 && !turretPanel.activeSelf && !_kBlockPauseShopButton;
		if (shopView.InteractionEnabled != flag3)
		{
			shopView.InteractionEnabled = flag3;
			shopView.UpdateView();
		}
		if (!isMulti && zombieCreator != null)
		{
			int num11 = GlobalGameController.GetEnemiesToKill(text3) - zombieCreator.NumOfDeadZombies;
			if ((GameConnect.isCampaign || !TrainingController.TrainingCompleted) && num11 == 0)
			{
				string text4 = ((!LevelBox.weaponsFromBosses.ContainsKey(text3)) ? LocalizationStore.Get("Key_0854") : LocalizationStore.Get("Key_0192"));
				if (zombieCreator.bossShowm)
				{
					text4 = LocalizationStore.Get("Key_0855");
				}
				enemiesLeftLabel.SetActive(perfectLabels == null || !perfectLabels.gameObject.activeInHierarchy);
				enemiesLeftLabel.GetComponent<UILabel>().text = text4;
			}
			else
			{
				enemiesLeftLabel.SetActive(false);
			}
		}
		if (playerMoveC != null && playerMoveC.isMechActive)
		{
			if (!mechWasActive)
			{
				currentHealthStep = Mathf.CeilToInt(health());
				currentArmorStep = Mathf.CeilToInt(armor());
				pastMechHealth = playerMoveC.liveMech;
				SetMechHealth();
				mechWasActive = true;
			}
			else if (!mechInAnim && pastMechHealth != playerMoveC.liveMech)
			{
				StartCoroutine(AnimateMechHealth());
			}
			pastMechHealth = playerMoveC.liveMech;
		}
		else
		{
			if (hideHealthInInterface)
			{
				for (int num12 = 0; num12 < Player_move_c.MaxPlayerGUIHealth; num12++)
				{
					hearts[num12].gameObject.SetActive(false);
				}
			}
			else
			{
				if (playerMoveC.respawnedForGUI || mechWasActive)
				{
					currentMechHealthStep = Mathf.CeilToInt(playerMoveC.liveMech);
					pastHealth = health();
					SetHealth();
				}
				else if (!healthInAnim && pastHealth != health())
				{
					StartCoroutine(AnimateHealth());
				}
				pastHealth = health();
			}
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				if (hideHealthInInterface)
				{
					for (int num13 = 0; num13 < Player_move_c.MaxPlayerGUIHealth; num13++)
					{
						armorShields[num13].gameObject.SetActive(false);
					}
					pastArmor = 0f;
				}
				else
				{
					if (playerMoveC.respawnedForGUI || mechWasActive)
					{
						currentMechHealthStep = Mathf.CeilToInt(playerMoveC.liveMech);
						pastArmor = armor();
						SetArmor();
					}
					else if (!armorInAnim && pastArmor != armor())
					{
						StartCoroutine(AnimateArmor());
					}
					pastArmor = armor();
				}
			}
			else
			{
				for (int num14 = 0; num14 < Player_move_c.MaxPlayerGUIHealth; num14++)
				{
					armorShields[num14].gameObject.SetActive(false);
				}
			}
			mechWasActive = false;
			playerMoveC.respawnedForGUI = false;
		}
		if (Defs.isMulti && (GameConnect.gameMode == GameConnect.GameMode.CapturePoints || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.TeamFight))
		{
			int winningTeam = WeaponManager.sharedManager.myNetworkStartTable.GetWinningTeam();
			mineBlue.SetActive(WeaponManager.sharedManager.myNetworkStartTable.myCommand > 0);
			bool flag4 = WeaponManager.sharedManager.myNetworkStartTable.myCommand == winningTeam;
			winningBlue.SetActive(winningTeam != 0 && flag4);
			winningRed.SetActive(winningTeam != 0 && !flag4);
		}
		if (!GameConnect.isDaterRegim && Defs.isMulti && (GameConnect.gameMode == GameConnect.GameMode.Deathmatch || GameConnect.gameMode == GameConnect.GameMode.TimeBattle || GameConnect.gameMode == GameConnect.GameMode.Duel))
		{
			int placeInMatch = WeaponManager.sharedManager.myNetworkStartTable.GetPlaceInMatch();
			string text5 = FormatRank(placeInMatch);
			placeDeathmatchLabel.text = text5;
			placeCoopLabel.text = text5;
			firstPlaceGO.SetActive(placeInMatch == 0);
			firstPlaceCoop.SetActive(placeInMatch == 0);
		}
		if (PauseGUIController.Instance != null)
		{
			bool flag5 = !PauseGUIController.Instance.IsPaused;
			if (leftAnchor != null && leftAnchor.activeInHierarchy != flag5)
			{
				leftAnchor.SetActive(flag5);
			}
			if (swipeWeaponPanel != null && swipeWeaponPanel.gameObject.activeInHierarchy != flag5)
			{
				swipeWeaponPanel.gameObject.SetActive(flag5);
			}
		}
	}

	private string FormatRank(int rank)
	{
		if (rank != _rankMemo.Key)
		{
			string value = (rank + 1).ToString();
			_rankMemo = new KeyValuePair<int, string>(rank, value);
		}
		return _rankMemo.Value;
	}

	private void HandlePCWeaponSwitch()
    {
        if (playerMoveC != null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && !playerMoveC.isMechActive && !WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectWeaponFromCategory(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectWeaponFromCategory(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SelectWeaponFromCategory(3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                SelectWeaponFromCategory(4);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                SelectWeaponFromCategory(5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                SelectWeaponFromCategory(6);
        }
    }

    private void InputCompatLayer()
    {
        if (!Application.isMobilePlatform)
        {
            HandlePCWeaponSwitch();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (playerMoveC != null && !playerMoveC.isInappWinOpen) GlobalControls.MouseLocked = false;
            }
        }
    }

    private void LateUpdate()
    {
        InputCompatLayer();
    }

	private void SetMechHealth()
	{
		currentHealthStep = Mathf.FloorToInt(pastMechHealth);
		for (int i = 0; i < mechShields.Length; i++)
		{
			mechShields[i].SetIndex(Mathf.CeilToInt((pastMechHealth - (float)i) / 18f), HeartEffect.IndicatorType.Mech);
		}
	}

	private void SetHealth()
	{
		currentHealthStep = Mathf.FloorToInt(pastHealth);
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].SetIndex(Mathf.CeilToInt((pastHealth - (float)i) / 9f), HeartEffect.IndicatorType.Hearts);
		}
	}

	private void SetArmor()
	{
		currentArmorStep = Mathf.FloorToInt(pastArmor);
		for (int i = 0; i < armorShields.Length; i++)
		{
			armorShields[i].SetIndex(Mathf.CeilToInt((pastArmor - (float)i) / 9f), HeartEffect.IndicatorType.Armor);
		}
	}

	private IEnumerator AnimateHealth()
	{
		healthInAnim = true;
		currentHealthStep = Mathf.CeilToInt(pastHealth);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (currentHealthStep != Mathf.CeilToInt(health()))
		{
			int num = currentHealthStep - 9 * Mathf.FloorToInt((float)currentHealthStep / 9f);
			int num2 = Mathf.CeilToInt(health());
			if (num2 < 0)
			{
				num2 = 0;
			}
			bool flag = currentHealthStep > num2;
			if (flag)
			{
				num--;
				if (num < 0)
				{
					num = 8;
				}
			}
			currentHealthStep += ((!flag) ? 1 : (-1));
			hearts[num].Animate(flag ? Mathf.FloorToInt((float)currentHealthStep / 9f) : Mathf.CeilToInt((float)currentHealthStep / 9f), HeartEffect.IndicatorType.Hearts);
			yield return awaiter;
		}
		healthInAnim = false;
	}

	private IEnumerator AnimateArmor()
	{
		armorInAnim = true;
		currentArmorStep = Mathf.CeilToInt(pastArmor);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (currentArmorStep != Mathf.CeilToInt(armor()))
		{
			int num = currentArmorStep - 9 * Mathf.FloorToInt((float)currentArmorStep / 9f);
			int num2 = Mathf.CeilToInt(armor());
			if (num2 < 0)
			{
				num2 = 0;
			}
			bool flag = currentArmorStep > num2;
			if (flag)
			{
				num--;
				if (num < 0)
				{
					num = 8;
				}
			}
			currentArmorStep += ((!flag) ? 1 : (-1));
			armorShields[num].Animate(flag ? Mathf.FloorToInt((float)currentArmorStep / 9f) : Mathf.CeilToInt((float)currentArmorStep / 9f), HeartEffect.IndicatorType.Armor);
			yield return awaiter;
		}
		armorInAnim = false;
	}

	private IEnumerator AnimateMechHealth()
	{
		mechInAnim = true;
		currentMechHealthStep = Mathf.CeilToInt(pastMechHealth);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (currentMechHealthStep != Mathf.CeilToInt(playerMoveC.liveMech))
		{
			int num = currentMechHealthStep - 18 * Mathf.FloorToInt((float)currentMechHealthStep / 18f);
			int num2 = Mathf.CeilToInt(playerMoveC.liveMech);
			if (num2 < 0)
			{
				num2 = 0;
			}
			bool flag = currentMechHealthStep > num2;
			if (flag)
			{
				num--;
				if (num < 0)
				{
					num = 17;
				}
			}
			currentMechHealthStep += ((!flag) ? 1 : (-1));
			mechShields[num].Animate(flag ? Mathf.FloorToInt((float)currentMechHealthStep / 18f) : Mathf.CeilToInt((float)currentMechHealthStep / 18f), HeartEffect.IndicatorType.Mech);
			yield return awaiter;
		}
		mechInAnim = false;
	}

	public void SetScopeForWeapon(string num)
	{
		scopeText.SetActive(true);
		string path = ((Device.isWeakDevice || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64) ? ResPath.Combine("Scopes", "Scope_" + num + "_small") : ResPath.Combine("Scopes", "Scope_" + num));
		scopeText.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(path);
	}

	public void ResetScope()
	{
		scopeText.GetComponent<UITexture>().mainTexture = null;
		scopeText.SetActive(false);
	}

	public void HandleWeaponEquipped(WeaponSounds ws)
	{
		int result;
		int result2;
		if ((ws != null && WeaponManager.sharedManager != null && !ws.IsAvalibleFromFilter(WeaponManager.sharedManager.CurrentFilterMap)) || (GameConnect.isHunger && ws != null && int.TryParse(ws.nameNoClone().Substring("Weapon".Length), out result) && result != 9 && !ChestController.weaponForHungerGames.Contains(result)) || ((GameConnect.isSurvival || GameConnect.isCOOP) && ws != null && int.TryParse(ws.nameNoClone().Substring("Weapon".Length), out result2) && result2 != 9 && result2 != 1 && ((GameConnect.isSurvival && !ZombieCreator.WeaponsAddedInWaves.SelectMany((List<string> weapons) => weapons).Contains(ws.nameNoClone())) || (GameConnect.isCOOP && !ChestController.weaponForHungerGames.Contains(result2)))))
		{
			return;
		}
		int num = 0;
		foreach (Weapon playerWeapon in WeaponManager.sharedManager.playerWeapons)
		{
			int categoryNabor = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor;
			num++;
		}
		for (int i = changeWeaponWrap.transform.childCount; i < num; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(weaponPreviewPrefab);
			obj.name = "WeaponCat_" + i;
			obj.transform.parent = changeWeaponWrap.transform;
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.GetComponent<UITexture>().width = Mathf.RoundToInt((float)widthWeaponScrollPreview * 0.7f);
			obj.GetComponent<UITexture>().height = Mathf.RoundToInt((float)widthWeaponScrollPreview * 0.7f);
			obj.GetComponent<BoxCollider>().size = new Vector3((float)widthWeaponScrollPreview * 1.3f, (float)widthWeaponScrollPreview * 1.3f, 1f);
		}
		changeWeaponWrap.SortAlphabetically();
		changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = false;
		changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = true;
		UpdateWeaponIconsForWrap();
		for (int j = 0; j < WeaponManager.sharedManager.playerWeapons.Count; j++)
		{
			changeWeaponWrap.transform.GetChild(j).GetComponent<WeaponIconController>().myWeaponSounds = ((Weapon)WeaponManager.sharedManager.playerWeapons[j]).weaponPrefab.GetComponent<WeaponSounds>();
		}
		SelectWeaponFromCategory(ws.categoryNabor);
	}

	public IEnumerator ShowRespawnWindow(KillerInfo killerInfo, float seconds)
	{
		CameraSceneController.sharedController.killCamController.lastDistance = 1f;
		CameraSceneController.sharedController.SetTargetKillCam(killerInfo.killerTransform);
		Defs.inRespawnWindow = true;
		respawnWindow.Show(killerInfo);
		respawnWindow.characterDrag.SetActive(false);
		respawnWindow.cameraDrag.SetActive(true);
		SkinName skinNameComponent = killerInfo.killerTransform.GetComponent<SkinName>();
		float closeTime = seconds;
		bool showCharacter = false;
		while (Defs.inRespawnWindow)
		{
			if (!showCharacter && (killerInfo.killerTransform == null || killerInfo.killerTransform.position.y <= -5000f || skinNameComponent.playerMoveC.isKilled))
			{
				showCharacter = true;
				respawnWindow.characterDrag.SetActive(true);
				respawnWindow.cameraDrag.SetActive(false);
				RespawnWindow.Instance.ShowCharacter(killerInfo);
				CameraSceneController.sharedController.SetTargetKillCam();
			}
			closeTime -= Time.deltaTime;
			respawnWindow.SetCurrentTime(closeTime);
			if (closeTime <= 0f)
			{
				respawnWindow.CloseRespawnWindow();
			}
			yield return null;
		}
	}

	public void UpdateWeaponIconsForWrap()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			Texture texture = ShopNGUIController.TextureForEquippedWeaponOrWear(i);
			if (!(texture != null))
			{
				continue;
			}
			weaponIcons[i].mainTexture = texture;
			foreach (Transform item in changeWeaponWrap.transform)
			{
				if (item.name.Equals("WeaponCat_" + num))
				{
					item.GetComponent<UITexture>().mainTexture = texture;
					break;
				}
			}
			num++;
		}
	}

	public void BlinkNoAmmo(int count)
	{
		if (count == 0)
		{
			StopPlayingLowResourceBeep();
		}
		timerBlinkNoAmmo = (float)count * periodBlink;
		blinkNoAmmoLabel.color = new Color(blinkNoAmmoLabel.color.r, blinkNoAmmoLabel.color.g, blinkNoAmmoLabel.color.b, 0f);
	}

	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layerNumber;
		}
	}

	private void OnDestroy()
	{
		SetNGUITouchDragThreshold(40f);
		sharedInGameGUI = null;
		WeaponManager.WeaponEquipped -= HandleWeaponEquipped;
		PauseNGUIController.ChatSettUpdated -= HandleChatSettUpdated;
		PauseNGUIController.PlayerHandUpdated -= AdjustToPlayerHands;
		ControlsSettingsBase.ControlsChanged -= AdjustToPlayerHands;
		PauseNGUIController.SwitchingWeaponsUpdated -= SetSwitchingWeaponPanel;
	}

	public void SetInterfaceVisible(bool visible)
	{
		interfacePanel.GetComponent<UIPanel>().gameObject.SetActive(visible);
		joystikPanel.gameObject.SetActive(visible);
		shopPanel.gameObject.SetActive(visible);
		bloodPanel.gameObject.SetActive(visible);
	}

	private void SetNGUITouchDragThreshold(float newValue)
	{
		if (UICamera.mainCamera != null && UICamera.mainCamera.GetComponent<UICamera>() != null)
		{
			UICamera.mainCamera.GetComponent<UICamera>().touchDragThreshold = newValue;
		}
		else
		{
			UnityEngine.Debug.LogWarning("UICamera.mainCamera is null");
		}
	}

	public void ShowControlSchemeConfigurator()
	{
	}
}
