using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(UIButton))]
	public class LobbyCraftButton : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CWaitAndShowHint_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

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
			public _003CWaitAndShowHint_003Ed__13(int _003C_003E1__state)
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
				if (HintController.instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				HintController.instance.ShowHintByName("craft");
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
		internal sealed class _003CUpdateButtonStateCoroutine_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyCraftButton _003C_003E4__this;

			private UIButton _003Cbutton_003E5__1;

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
			public _003CUpdateButtonStateCoroutine_003Ed__14(int _003C_003E1__state)
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
					_003Cbutton_003E5__1 = _003C_003E4__this.GetComponent<UIButton>();
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
					{
						_003Cbutton_003E5__1.isEnabled = false;
						return false;
					}
					goto IL_00d2;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00d2;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E4__this._showAnimationViewed.Value = true;
					_003C_003E4__this._unlockAnimationObject.SetActive(true);
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003Cbutton_003E5__1.isEnabled = true;
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00d2:
					if (ExperienceController.sharedController == null || ExperienceController.sharedController.currentLevel < 1)
					{
						_003Cbutton_003E5__1.isEnabled = false;
						_003C_003E4__this._levelRequiredLabel.text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { BalanceController.LobbyItemsLevelRequired });
						_003C_003E4__this._levelRequiredLabel.gameObject.SetActiveSafeSelf(true);
						_003C_003E2__current = new WaitForSeconds(0.2f);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E4__this._levelRequiredLabel.gameObject.SetActiveSafeSelf(false);
					_003Cbutton_003E5__1.isEnabled = _003C_003E4__this._showAnimationViewed.Value;
					if (!_003C_003E4__this._showAnimationViewed.Value)
					{
						_003C_003E2__current = new WaitForSeconds(1f);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E4__this._unlockAnimationObject.SetActive(false);
					break;
				}
				_003C_003E4__this._craftedItemExists.SetActive(Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem != null);
				_003C_003E2__current = new WaitForSeconds(0.2f);
				_003C_003E1__state = 4;
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

		[CompilerGenerated]
		internal sealed class _003CUpdateNewItemsCountCoroutine_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyCraftButton _003C_003E4__this;

			private UIButton _003Cbutton_003E5__1;

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
			public _003CUpdateNewItemsCountCoroutine_003Ed__15(int _003C_003E1__state)
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
					_003Cbutton_003E5__1 = _003C_003E4__this.GetComponent<UIButton>();
					_003C_003E4__this._newItemsCountLabel.gameObject.SetActive(false);
				}
				if (_003Cbutton_003E5__1.isEnabled)
				{
					int num2 = Singleton<LobbyItemsController>.Instance.AllItems.Count((LobbyItem i) => i.CanShowIsNew);
					if (num2 > 0)
					{
						_003C_003E4__this._newItemsCountLabel.gameObject.SetActive(true);
						_003C_003E4__this._newItemsCountLabel.text = num2.ToString();
					}
					else
					{
						_003C_003E4__this._newItemsCountLabel.gameObject.SetActive(false);
					}
				}
				_003C_003E2__current = new WaitForSeconds(1f);
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

		[SerializeField]
		protected internal GameObject _craftedItemExists;

		[SerializeField]
		protected internal UILabel _levelRequiredLabel;

		[SerializeField]
		protected internal UILabel _newItemsCountLabel;

		[SerializeField]
		protected internal GameObject _unlockAnimationObject;

		private const string HINT_NAME = "craft";

		private PrefsBoolCachedProperty _showAnimationViewed = new PrefsBoolCachedProperty("craft_button_show_animation_viewed");

		private PrefsBoolCachedProperty _hintViewed = new PrefsBoolCachedProperty("craft_button_hint_viewed");

		private void Awake()
		{
			CloudSyncController.ApplyCompleted += CloudSyncController_ApplyCompleted;
		}

		private void OnDestroy()
		{
			CloudSyncController.ApplyCompleted -= CloudSyncController_ApplyCompleted;
		}

		private void CloudSyncController_ApplyCompleted()
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(UpdateButtonStateCoroutine());
			}
		}

		private void OnEnable()
		{
			StartCoroutine(UpdateButtonStateCoroutine());
			StartCoroutine(UpdateNewItemsCountCoroutine());
			if (LobbyItemsController.TutorialCompleted)
			{
				_hintViewed.Value = true;
			}
			if (!_hintViewed.Value && PlayerPrefs.GetInt("NestHintShowed", 0) > 0)
			{
				StartCoroutine(WaitAndShowHint());
			}
		}

		private void OnDisable()
		{
			StopCoroutine(UpdateButtonStateCoroutine());
		}

		private void OnClick()
		{
			if (!_hintViewed.Value)
			{
				_hintViewed.Value = true;
				HintController.instance.HideHintByName("craft");
			}
		}

		private IEnumerator WaitAndShowHint()
		{
			while (HintController.instance == null)
			{
				yield return null;
			}
			HintController.instance.ShowHintByName("craft");
		}

		private IEnumerator UpdateButtonStateCoroutine()
		{
			UIButton button = GetComponent<UIButton>();
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
			{
				button.isEnabled = false;
				yield break;
			}
			while (ExperienceController.sharedController == null || ExperienceController.sharedController.currentLevel < 1)
			{
				button.isEnabled = false;
				_levelRequiredLabel.text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { BalanceController.LobbyItemsLevelRequired });
				_levelRequiredLabel.gameObject.SetActiveSafeSelf(true);
				yield return new WaitForSeconds(0.2f);
			}
			_levelRequiredLabel.gameObject.SetActiveSafeSelf(false);
			button.isEnabled = _showAnimationViewed.Value;
			if (!_showAnimationViewed.Value)
			{
				yield return new WaitForSeconds(1f);
				_showAnimationViewed.Value = true;
				_unlockAnimationObject.SetActive(true);
				yield return null;
				button.isEnabled = true;
			}
			else
			{
				_unlockAnimationObject.SetActive(false);
			}
			while (true)
			{
				_craftedItemExists.SetActive(Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem != null);
				yield return new WaitForSeconds(0.2f);
			}
		}

		private IEnumerator UpdateNewItemsCountCoroutine()
		{
			UIButton button = GetComponent<UIButton>();
			_newItemsCountLabel.gameObject.SetActive(false);
			while (true)
			{
				if (button.isEnabled)
				{
					int num = Singleton<LobbyItemsController>.Instance.AllItems.Count((LobbyItem i) => i.CanShowIsNew);
					if (num > 0)
					{
						_newItemsCountLabel.gameObject.SetActive(true);
						_newItemsCountLabel.text = num.ToString();
					}
					else
					{
						_newItemsCountLabel.gameObject.SetActive(false);
					}
				}
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
