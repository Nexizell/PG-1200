using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class LeprechauntLobbyView : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CMainLoopCoroutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LeprechauntLobbyView _003C_003E4__this;

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
			public _003CMainLoopCoroutine_003Ed__16(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -1;
				}
				else
				{
					_003C_003E1__state = -1;
				}
				if (!_003C_003E4__this.CanShow())
				{
					_003C_003E4__this._model.SetActiveSafe(false);
					_003C_003E4__this.HideNickLabel();
				}
				else
				{
					_003C_003E4__this._model.SetActiveSafe(Singleton<LeprechauntManager>.Instance.LeprechauntExists);
					if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
					{
						_003C_003E4__this.HideNickLabel();
					}
					else
					{
						_003C_003E4__this.ShowNickLabel();
					}
					_003C_003E4__this.SetupNicklabel();
				}
				_003C_003E2__current = new WaitForRealSeconds(0.2f);
				_003C_003E1__state = 1;
				return true;
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

		public static LeprechauntLobbyView Instance;

		private const string AnimBoolRewardAvailable = "RewardAvailable";

		private const string AnimTriggerGetReward = "GetReward";

		private const string AnimTriggerTap = "Tap";

		[SerializeField]
		protected internal GameObject _model;

		[SerializeField]
		protected internal Animator _animator;

		private NickLabelController _nickLabelValue;

		private bool _waitEndRewardAnimation;

		private float _showNickLabelStartAt = -1f;

		private NickLabelController NickLabel
		{
			get
			{
				if (_nickLabelValue == null)
				{
					if (NickLabelStack.sharedStack != null)
					{
						_nickLabelValue = NickLabelStack.sharedStack.GetNextCurrentLabel();
					}
					if (_nickLabelValue != null)
					{
						_nickLabelValue.StartShow(NickLabelController.TypeNickLabel.Leprechaunt, base.transform);
						_nickLabelValue.gameObject.SetActive(false);
					}
				}
				return _nickLabelValue;
			}
		}

		public bool CanShow()
		{
			if ((!(Singleton<LeprechauntManager>.Instance != null) || Singleton<LeprechauntManager>.Instance.CurrentTime.HasValue) && TrainingController.TrainingCompleted && ExperienceController.sharedController.currentLevel >= 2)
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
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			_waitEndRewardAnimation = false;
		}

		private void OnDestroy()
		{
			_waitEndRewardAnimation = false;
			Instance = null;
		}

		private void OnEnable()
		{
			_waitEndRewardAnimation = false;
			_model.SetActive(Singleton<LeprechauntManager>.Instance.LeprechauntExists);
			StartCoroutine(MainLoopCoroutine());
		}

		private void Update()
		{
			if (_animator.gameObject.activeInHierarchy && _animator.GetBool("RewardAvailable") != Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
			{
				_animator.SetBool("RewardAvailable", Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop);
			}
		}

		private IEnumerator MainLoopCoroutine()
		{
			while (true)
			{
				if (!CanShow())
				{
					_model.SetActiveSafe(false);
					HideNickLabel();
				}
				else
				{
					_model.SetActiveSafe(Singleton<LeprechauntManager>.Instance.LeprechauntExists);
					if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
					{
						HideNickLabel();
					}
					else
					{
						ShowNickLabel();
					}
					SetupNicklabel();
				}
				yield return new WaitForRealSeconds(0.2f);
			}
		}

		private void SetupNicklabel()
		{
			if (!Singleton<LeprechauntManager>.Instance.LeprechauntExists || _waitEndRewardAnimation)
			{
				NickLabel.LeprechauntDaysLeft.gameObject.SetActiveSafe(false);
				NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(false);
				NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(false);
				return;
			}
			if (Singleton<LeprechauntManager>.Instance.LeprechauntLifeTimeLeft.HasValue)
			{
				NickLabel.LeprechauntDaysLeft.gameObject.SetActiveSafe(true);
				int num = Singleton<LeprechauntManager>.Instance.LeprechauntLifeTimeLeft.Value / 3600 / 24 + 1;
				NickLabel.LeprechauntDaysLeft.text = string.Format(LocalizationStore.Get("Key_2913"), new object[1] { num });
			}
			if (Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
			{
				NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(true);
				NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(false);
				NickLabel.LeprechauntGemsRewardAvailable.text = string.Format(LocalizationStore.Get("Key_2914"), new object[1] { Singleton<LeprechauntManager>.Instance.RewardReadyToDrop });
				if (Singleton<LeprechauntManager>.Instance.RewardCurrency == "GemsCurrency")
				{
					NickLabel.LeprechauntGemsIcon.SetActiveSafe(true);
					NickLabel.LeprechauntCoinsIcon.SetActiveSafe(false);
				}
				else
				{
					NickLabel.LeprechauntGemsIcon.SetActiveSafe(false);
					NickLabel.LeprechauntCoinsIcon.SetActiveSafe(true);
				}
			}
			else
			{
				NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(false);
				NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(true);
				NickLabel.LeprechauntRewardTimeLeft.text = RiliExtensions.GetTimeString((long)Singleton<LeprechauntManager>.Instance.RewardDropSecsLeft.Value);
			}
		}

		private void ShowNickLabel()
		{
			if (!NickLabel.gameObject.activeSelf)
			{
				if (_showNickLabelStartAt < 0f)
				{
					_showNickLabelStartAt = Time.realtimeSinceStartup;
				}
				if (Time.realtimeSinceStartup > _showNickLabelStartAt + 1f)
				{
					NickLabel.gameObject.SetActive(true);
					_showNickLabelStartAt = -1f;
				}
			}
		}

		private void HideNickLabel()
		{
			NickLabel.gameObject.SetActiveSafe(false);
		}

		public void Tap()
		{
			if ((!(LobbyCraftController.Instance != null) || !LobbyCraftController.Instance.InterfaceEnabled) && !_waitEndRewardAnimation)
			{
				if (Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
				{
					_waitEndRewardAnimation = true;
					StopCoroutine("MainLoopCoroutine");
					NickLabel.gameObject.SetActiveSafe(false);
					_animator.SetTrigger("GetReward");
				}
				else
				{
					_animator.SetTrigger("Tap");
				}
			}
		}

		public void OnAnimatorStateExit(string stateName)
		{
			if (stateName == "Close")
			{
				_waitEndRewardAnimation = false;
				_animator.SetBool("RewardAvailable", false);
				Singleton<LeprechauntManager>.Instance.DropReward();
				StartCoroutine(MainLoopCoroutine());
			}
		}
	}
}
