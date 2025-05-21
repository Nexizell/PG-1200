using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.NullExtensions;
using Rilisoft.TimeManagment;
using UnityEngine;

namespace Rilisoft
{
	public class Nest : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CWaitMainMenu_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Nest _003C_003E4__this;

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
			public _003CWaitMainMenu_003Ed__41(int _003C_003E1__state)
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
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (MainMenuController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this._banner = new LazyObject<NestBanner>(_003C_003E4__this._bannerPrefab, MainMenuController.sharedController.gameObject);
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

		private const string KEY_DROPPED_EGGS_COUNTER = "nest_dropped_eggs_counter";

		private const string AP_IS_ENABLED = "IsEnabled";

		private const string AP_IS_OPEN = "IsOpen";

		private const string TIMER_KEY = "drop_eggs";

		public const string HINT_SHOWED = "NestHintShowed";

		private Timer _dropTimer;

		public static Nest Instance;

		public static List<long> timerIntervalDelays = new List<long>
		{
			0L, 900L, 900L, 1800L, 1800L, 3600L, 3600L, 7200L, 7200L, 14400L,
			14400L, 21600L, 21600L, 43200L
		};

		private SaltedInt _dropCounter = new SaltedInt(178956970, -1);

		[SerializeField]
		protected internal GameObject NestGO;

		[SerializeField]
		protected internal Animator _animator;

		[SerializeField]
		protected internal AnimationHandler _animationHandler;

		[SerializeField]
		protected internal GameObject _bannerPrefab;

		private LazyObject<NestBanner> _banner;

		private PrefsBoolCachedProperty _eggIsReady = new PrefsBoolCachedProperty("nest_egg_ready");

		private bool _getEggProcessed;

		private NickLabelController _nickLabelControllerValue;

		private Vector3 _prevPos = Vector3.zero;

		private Timer DropTimer
		{
			get
			{
				if (_dropTimer == null)
				{
					if (Timer.TimerExists("drop_eggs"))
					{
						_dropTimer = Timer.GetTimer<StoragedSafityTimer>("drop_eggs");
					}
					else
					{
						_dropTimer = new StoragedSafityTimer("drop_eggs", TimerInterval, DropTimeMultiplier);
					}
				}
				return _dropTimer;
			}
		}

		private float DropTimeMultiplier
		{
			get
			{
				float effect = LobbyItemsController.GetEffect(LobbyItemInfo.LobbyItemBuffType.EggDropAccelerator);
				return Mathf.Max(1f, effect);
			}
		}

		public static long CurrentTime
		{
			get
			{
				return FriendsController.ServerTime;
			}
		}

		private long TimerInterval
		{
			get
			{
				if (timerIntervalDelays.Count <= DropCounter)
				{
					return timerIntervalDelays.Last();
				}
				return timerIntervalDelays[DropCounter];
			}
		}

		private int DropCounter
		{
			get
			{
				if (_dropCounter.Value < timerIntervalDelays.Count - 1 && PlayerPrefs.GetInt("nest_first_egg_getted", 0) > 0)
				{
					_dropCounter = timerIntervalDelays.Count - 1;
					PlayerPrefs.SetInt("nest_dropped_eggs_counter", _dropCounter.Value);
				}
				if (_dropCounter.Value < 0)
				{
					_dropCounter = PlayerPrefs.GetInt("nest_dropped_eggs_counter", 0);
				}
				return _dropCounter.Value;
			}
			set
			{
				_dropCounter = value;
				PlayerPrefs.SetInt("nest_dropped_eggs_counter", _dropCounter.Value);
			}
		}

		public GameObject NestGameObject
		{
			get
			{
				return NestGO;
			}
		}

		public bool BannerIsVisible
		{
			get
			{
				if (_banner != null && _banner.HasValue)
				{
					return _banner.Value.IsVisible;
				}
				return false;
			}
		}

		internal bool EggIsReady
		{
			get
			{
				if (DropCounter != 0)
				{
					return _eggIsReady.Value;
				}
				return true;
			}
		}

		public long? TimeLeft
		{
			get
			{
				if (CurrentTime < 1)
				{
					return null;
				}
				return (long)DropTimer.TimeLeftWithSpeedMultiplier;
			}
		}

		private NickLabelController _nickLabelController
		{
			get
			{
				if (_nickLabelControllerValue == null)
				{
					if (NickLabelStack.sharedStack != null)
					{
						_nickLabelControllerValue = NickLabelStack.sharedStack.GetNextCurrentLabel();
					}
					if (_nickLabelControllerValue != null)
					{
						_nickLabelControllerValue.StartShow(NickLabelController.TypeNickLabel.Nest, NestGO.transform);
					}
				}
				return _nickLabelControllerValue;
			}
		}

		public event Func<bool> NickLabelVisiblCheckers;

		public bool NestCanShow()
		{
			if (CurrentTime >= 1 && TrainingController.TrainingCompleted && ExperienceController.sharedController.currentLevel >= 2)
			{
				if (!(MainMenuController.sharedController != null) || (!MainMenuController.sharedController.SettingsJoysticksPanel.activeInHierarchy && !MainMenuController.sharedController.settingsPanel.activeInHierarchy && !MainMenuController.sharedController.FreePanelIsActive && !MainMenuController.sharedController.InMiniGamesScreen))
				{
					if (FeedbackMenuController.Instance != null)
					{
						return !FeedbackMenuController.Instance.gameObject.activeInHierarchy;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		private void Awake()
		{
			Instance = this;
			StartCoroutine(WaitMainMenu());
			if (!_eggIsReady.Value)
			{
				DropTimer.Start();
			}
			Timer dropTimer = DropTimer;
			dropTimer.OnTick = (Action<int>)Delegate.Combine(dropTimer.OnTick, new Action<int>(TimerTick));
			_animationHandler.OnAnimationEvent += _animationHandler_OnAnimationEvent;
			NickLabelVisiblCheckers -= NickLabelVisibleChecker_Craft;
			NickLabelVisiblCheckers += NickLabelVisibleChecker_Craft;
		}

		private void OnDestroy()
		{
			if (_dropTimer != null)
			{
				Timer dropTimer = _dropTimer;
				dropTimer.OnTick = (Action<int>)Delegate.Remove(dropTimer.OnTick, new Action<int>(TimerTick));
				DropTimer.Stop();
			}
		}

		private bool NickLabelVisibleChecker_Craft()
		{
			if (!(LobbyCraftController.Instance == null))
			{
				return !LobbyCraftController.Instance.InterfaceEnabled;
			}
			return true;
		}

		private IEnumerator WaitMainMenu()
		{
			while (MainMenuController.sharedController == null)
			{
				yield return null;
			}
			_banner = new LazyObject<NestBanner>(_bannerPrefab, MainMenuController.sharedController.gameObject);
		}

		private void Update()
		{
			if (!NestCanShow())
			{
				NestGO.SetActiveSafe(false);
				if (_nickLabelController != null)
				{
					_nickLabelController.gameObject.SetActiveSafe(false);
				}
				if (_banner != null && _banner.HasValue)
				{
					_banner.Value.EnableTouchBlocker(false);
				}
				return;
			}
			NestGO.SetActiveSafe(true);
			if (_nickLabelController != null)
			{
				_nickLabelController.gameObject.SetActiveSafe(this.NickLabelVisiblCheckers == null || this.NickLabelVisiblCheckers());
			}
			if (DropCounter == 0)
			{
				_animator.SetBool("IsEnabled", EggIsReady);
				ShowLobbyHeader(false, 0L);
			}
			else
			{
				DropTimer.SpeedMultiplier = DropTimeMultiplier;
				_animator.SetBool("IsEnabled", EggIsReady);
				ShowLobbyHeader(!EggIsReady, TimeLeft.Value);
			}
		}

		private void TimerTick(int intervalsElapsed)
		{
			_eggIsReady.Value = true;
			DropTimer.Stop();
		}

		public void Click()
		{
			if ((!(MainMenuController.sharedController != null) || !MainMenuController.sharedController.LeaderboardsIsOpening) && (!(LobbyCraftController.Instance != null) || !LobbyCraftController.Instance.InterfaceEnabled))
			{
				GetEgg();
			}
		}

		private void GetEgg()
		{
			if (EggIsReady && NestGO.activeInHierarchy && !_getEggProcessed)
			{
				_getEggProcessed = true;
				SetMenuInteractionEnabled(false);
				_animator.SetBool("IsOpen", true);
			}
		}

		private void _animationHandler_OnAnimationEvent(string animName, AnimationHandler.AnimState animState)
		{
			if (animName == "Open" && animState == AnimationHandler.AnimState.Finished && _getEggProcessed)
			{
				DropEgg();
			}
		}

		private void DropEgg()
		{
			if (!EggIsReady)
			{
				OnBannerClose();
				return;
			}
			Egg egg = null;
			if (DropCounter == 0)
			{
				EggData data = Singleton<EggsManager>.Instance.GetAllEggs().FirstOrDefault((EggData e) => e.Id == "egg_simple_rating");
				egg = Singleton<EggsManager>.Instance.AddEgg(data);
			}
			else
			{
				egg = Singleton<EggsManager>.Instance.AddRandomEgg();
			}
			if (egg != null && egg.Data != null)
			{
				AnalyticsStuff.LogEggDelivery(egg.Data.Id);
			}
			ResetTimer();
			NestBanner.OnClose += OnBannerClose;
			_banner.Value.Show(egg);
		}

		private void OnBannerClose()
		{
			NestBanner.OnClose -= OnBannerClose;
			_animator.SetBool("IsOpen", false);
			_animator.SetBool("IsEnabled", false);
			SetMenuInteractionEnabled(true);
			_getEggProcessed = false;
		}

		private void ResetTimer()
		{
			if (CurrentTime >= 1)
			{
				_eggIsReady.Value = false;
				DropCounter++;
				DropTimer.Stop();
				DropTimer.Duration = TimerInterval;
				DropTimer.SpeedMultiplier = DropTimeMultiplier;
				DropTimer.Reset();
				DropTimer.Start();
			}
		}

		private void SetMenuInteractionEnabled(bool enabled)
		{
			if (!enabled)
			{
				_banner.Value.EnableTouchBlocker(true);
				if (FreeAwardShowHandler.Instance != null)
				{
					FreeAwardShowHandler.Instance.IsInteractable = false;
				}
			}
			else
			{
				_banner.Value.EnableTouchBlocker(false);
				if (FreeAwardShowHandler.Instance != null)
				{
					FreeAwardShowHandler.Instance.IsInteractable = true;
				}
			}
		}

		private void ShowLobbyHeader(bool visible, long timeLeft)
		{
			if (_nickLabelController == null)
			{
				return;
			}
			if (_getEggProcessed)
			{
				visible = false;
			}
			_nickLabelController.NestTimerLabel.gameObject.SetActiveSafe(visible && timeLeft > 0);
			_nickLabelController.NestGO.transform.localPosition = ((timeLeft <= 0) ? _nickLabelController.NestLabelPosWithoutTimer : _nickLabelController.NestLabelPos);
			if (_nickLabelController.NestGO.transform.localPosition != _prevPos)
			{
				_nickLabelController.GetComponent<UIPanel>().Do(delegate(UIPanel p)
				{
					p.SetDirty();
					p.Refresh();
				});
			}
			_prevPos = _nickLabelController.NestGO.transform.localPosition;
			if (visible)
			{
				_nickLabelController.NestTimerLabel.text = RiliExtensions.GetTimeString(timeLeft);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			SetMenuInteractionEnabled(true);
			_getEggProcessed = false;
		}
	}
}
