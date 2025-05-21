using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyMiniGamesButton : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CUpdateButtonStateCoroutine_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyMiniGamesButton _003C_003E4__this;

			private UIButton[] _003Cbuttons_003E5__1;

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
			public _003CUpdateButtonStateCoroutine_003Ed__10(int _003C_003E1__state)
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
					_003Cbuttons_003E5__1 = _003C_003E4__this.GetComponents<UIButton>();
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
					{
						_003Cbuttons_003E5__1.ForEach(delegate(UIButton b)
						{
							b.isEnabled = false;
						});
						return false;
					}
					goto IL_010e;
				case 1:
					_003C_003E1__state = -1;
					goto IL_010e;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E4__this._showAnimationViewed.Value = true;
					_003C_003E4__this._unlockAnimationObject.SetActive(true);
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003Cbuttons_003E5__1.ForEach(delegate(UIButton b)
					{
						b.isEnabled = true;
					});
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_010e:
					if (ExperienceController.sharedController == null || ExperienceController.sharedController.currentLevel < 1)
					{
						_003Cbuttons_003E5__1.ForEach(delegate(UIButton b)
						{
							b.isEnabled = false;
						});
						_003C_003E4__this._levelRequiredLabel.text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { BalanceController.MiniGamesLevelRequired });
						_003C_003E4__this._levelRequiredLabel.gameObject.SetActiveSafeSelf(true);
						_003C_003E2__current = new WaitForSeconds(0.2f);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E4__this._levelRequiredLabel.gameObject.SetActiveSafeSelf(false);
					_003Cbuttons_003E5__1.ForEach(delegate(UIButton b)
					{
						b.isEnabled = _003C_003E4__this._showAnimationViewed.Value;
					});
					if (!_003C_003E4__this._showAnimationViewed.Value)
					{
						_003C_003E2__current = new WaitForSeconds(1f);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E4__this._unlockAnimationObject.SetActive(false);
					break;
				}
				_003C_003E4__this._ticketsIndicatorSprite.enabled = FreeTicketsController.Instance.NumOfAccumulatedTicketsToGive() > 0;
				try
				{
					MainMenuController sharedController = MainMenuController.sharedController;
					if (sharedController != null)
					{
						List<int> list = sharedController.NewAvailableMiniGameModes();
						if (list != null)
						{
							_003C_003E4__this._newModesIndicatorSprite.gameObject.SetActiveSafeSelf(list.Count > 0);
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in setting state of new mini game modes sprite: {0}", ex);
				}
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

		[SerializeField]
		protected internal UISprite _ticketsIndicatorSprite;

		[SerializeField]
		protected internal UISprite _newModesIndicatorSprite;

		[SerializeField]
		protected internal UILabel _levelRequiredLabel;

		[SerializeField]
		protected internal GameObject _unlockAnimationObject;

		private PrefsBoolCachedProperty _showAnimationViewed = new PrefsBoolCachedProperty("minigames_button_show_animation_viewed");

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
		}

		private void OnDisable()
		{
			StopCoroutine(UpdateButtonStateCoroutine());
		}

		private IEnumerator UpdateButtonStateCoroutine()
		{
			UIButton[] buttons = GetComponents<UIButton>();
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
			{
				buttons.ForEach(delegate(UIButton b)
				{
					b.isEnabled = false;
				});
				yield break;
			}
			while (ExperienceController.sharedController == null || ExperienceController.sharedController.currentLevel < 1)
			{
				buttons.ForEach(delegate(UIButton b)
				{
					b.isEnabled = false;
				});
				_levelRequiredLabel.text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { BalanceController.MiniGamesLevelRequired });
				_levelRequiredLabel.gameObject.SetActiveSafeSelf(true);
				yield return new WaitForSeconds(0.2f);
			}
			_levelRequiredLabel.gameObject.SetActiveSafeSelf(false);
			buttons.ForEach(delegate(UIButton b)
			{
				b.isEnabled = _showAnimationViewed.Value;
			});
			if (!_showAnimationViewed.Value)
			{
				yield return new WaitForSeconds(1f);
				_showAnimationViewed.Value = true;
				_unlockAnimationObject.SetActive(true);
				yield return null;
				buttons.ForEach(delegate(UIButton b)
				{
					b.isEnabled = true;
				});
			}
			else
			{
				_unlockAnimationObject.SetActive(false);
			}
			while (true)
			{
				_ticketsIndicatorSprite.enabled = FreeTicketsController.Instance.NumOfAccumulatedTicketsToGive() > 0;
				try
				{
					MainMenuController sharedController = MainMenuController.sharedController;
					if (sharedController != null)
					{
						List<int> list = sharedController.NewAvailableMiniGameModes();
						if (list != null)
						{
							_newModesIndicatorSprite.gameObject.SetActiveSafeSelf(list.Count > 0);
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in setting state of new mini game modes sprite: {0}", ex);
				}
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}
