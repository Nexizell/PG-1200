using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class RankIndicatorGuiElement : GuiElementBase
	{
		[CompilerGenerated]
		internal sealed class _003CBlinkingCoroutine_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public RankIndicatorGuiElement _003C_003E4__this;

			private int _003Ci_003E5__1;

			public bool rankChanged;

			public AudioClip sound;

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
			public _003CBlinkingCoroutine_003Ed__27(int _003C_003E1__state)
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
					_003C_003E1__state = -1;
					_003C_003E4__this._currentProgress.enabled = true;
					_003C_003E2__current = new WaitForSeconds(0.15f);
					_003C_003E1__state = 2;
					return true;
				case 2:
				{
					_003C_003E1__state = -1;
					int num = _003Ci_003E5__1 + 1;
					_003Ci_003E5__1 = num;
					break;
				}
				}
				if (_003Ci_003E5__1 != 4)
				{
					_003C_003E4__this._currentProgress.enabled = false;
					_003C_003E2__current = new WaitForSeconds(0.15f);
					_003C_003E1__state = 1;
					return true;
				}
				if (!rankChanged)
				{
					_003C_003E4__this.NewProgress = _003C_003E4__this.CurrentProgress;
				}
				else
				{
					_003C_003E4__this.SetExp(false);
				}
				if (Defs.isSoundFX)
				{
					NGUITools.PlaySound(sound);
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

		[Header("property bindings")]
		[SerializeField]
		protected internal UILabel _experienceLabel;

		[SerializeField]
		protected internal UISprite _currentProgress;

		[SerializeField]
		protected internal UISprite _newProgress;

		[SerializeField]
		protected internal UISprite _rankSprite;

		public override bool IsVisible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		private string ExperienceLabel
		{
			get
			{
				if (!(_experienceLabel != null))
				{
					return string.Empty;
				}
				return _experienceLabel.text;
			}
			set
			{
				if (_experienceLabel != null)
				{
					_experienceLabel.text = value ?? string.Empty;
				}
			}
		}

		private float CurrentProgress
		{
			get
			{
				if (!(_currentProgress != null))
				{
					return 0f;
				}
				return _currentProgress.transform.localScale.x;
			}
			set
			{
				if (_currentProgress != null)
				{
					_currentProgress.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
				}
			}
		}

		private float NewProgress
		{
			get
			{
				if (!(_newProgress != null))
				{
					return 0f;
				}
				return _newProgress.transform.localScale.x;
			}
			set
			{
				if (_newProgress != null)
				{
					_newProgress.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
				}
			}
		}

		private int RankSpriteValue
		{
			get
			{
				if (_rankSprite == null)
				{
					return 1;
				}
				string s = _rankSprite.spriteName.Replace("Rank_", string.Empty);
				int result = 0;
				if (!int.TryParse(s, out result))
				{
					return 1;
				}
				return result;
			}
			set
			{
				if (_rankSprite != null)
				{
					string spriteName = string.Format("Rank_{0}", new object[1] { value });
					_rankSprite.spriteName = spriteName;
				}
			}
		}

		protected override void WhenPush()
		{
			base.WhenPush();
			base.gameObject.SetActive(true);
		}

		protected override void WhenPop()
		{
			base.WhenPop();
			base.gameObject.SetActive(false);
		}

		protected override void Awake()
		{
			ExperienceController.OnPlayerProgressChanged += ExperienceControllerOnPlayerProgressChanged;
			CloudSyncController.ApplyCompleted += HandleCloudSyncControllerApplyCompleted;
			CloudSyncController.ExplicitSyncCompleted += HandleCloudSyncControllerApplyCompleted;
		}

		private void HandleCloudSyncControllerApplyCompleted()
		{
			SetExp(true);
		}

		protected override void OnEnable()
		{
			SetExp(false);
		}

		protected override void OnDisable()
		{
			StopAllCoroutines();
			SetExp(false);
			if (GuiElementBase.InStack(this))
			{
				PopRequest();
			}
		}

		protected override void OnDestroy()
		{
			CloudSyncController.ExplicitSyncCompleted -= HandleCloudSyncControllerApplyCompleted;
			CloudSyncController.ApplyCompleted -= HandleCloudSyncControllerApplyCompleted;
			ExperienceController.OnPlayerProgressChanged -= ExperienceControllerOnPlayerProgressChanged;
		}

		private void SetExp(bool playBlink)
		{
			RankSpriteValue = ExpController.Instance.Rank;
			if (!(ExperienceController.sharedController == null) && ExperienceController.sharedController.CurrentProgress != null)
			{
				int currentExperience = ExperienceController.sharedController.CurrentExperience;
				int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
				float num2 = (float)currentExperience / (float)num;
				if (ExperienceController.sharedController.currentLevel == 36)
				{
					num2 = 1f;
				}
				ExperienceLabel = ExpController.FormatExperienceLabel(currentExperience, num);
				CurrentProgress = num2;
				if (NewProgress > CurrentProgress)
				{
					NewProgress = 0f;
				}
				if (playBlink && base.gameObject.activeInHierarchy)
				{
					StartCoroutine(BlinkingCoroutine(true, ExperienceController.sharedController.CurrentProgress.Exp2));
				}
				else
				{
					NewProgress = num2;
				}
			}
		}

		private void ExperienceControllerOnPlayerProgressChanged(ExperienceController.PlayerProgress progress)
		{
			SetExp(true);
		}

		private IEnumerator BlinkingCoroutine(bool rankChanged, AudioClip sound)
		{
			int i = 0;
			while (i != 4)
			{
				_currentProgress.enabled = false;
				yield return new WaitForSeconds(0.15f);
				_currentProgress.enabled = true;
				yield return new WaitForSeconds(0.15f);
				int num = i + 1;
				i = num;
			}
			if (!rankChanged)
			{
				NewProgress = CurrentProgress;
			}
			else
			{
				SetExp(false);
			}
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(sound);
			}
		}
	}
}
