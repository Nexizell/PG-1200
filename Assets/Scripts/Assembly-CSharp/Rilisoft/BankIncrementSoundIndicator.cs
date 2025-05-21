using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class BankIncrementSoundIndicator : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CPlaySounds_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public float delay;

			public bool isGems;

			public bool oneCoin;

			public BankIncrementSoundIndicator _003C_003E4__this;

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
			public _003CPlaySounds_003Ed__8(int _003C_003E1__state)
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
					if (SceneLoader.ActiveSceneName.Equals("LevelComplete"))
					{
						return false;
					}
					if (!Defs.isSoundFX)
					{
						return false;
					}
					_003C_003E2__current = new WaitForRealSeconds(delay);
					_003C_003E1__state = 1;
					return true;
				case 1:
				{
					_003C_003E1__state = -1;
					AudioClip audioClip = null;
					audioClip = ((!isGems) ? ((oneCoin && _003C_003E4__this.ClipCoinAdded != null) ? _003C_003E4__this.ClipCoinAdded : _003C_003E4__this.ClipCoinsAdded) : ((oneCoin && _003C_003E4__this.ClipCoinAdded != null) ? _003C_003E4__this.ClipGemAdded : _003C_003E4__this.ClipGemsAdded));
					NGUITools.PlaySound(audioClip);
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

		public float PlayDelay = 0.1f;

		public AudioClip ClipCoinAdded;

		public AudioClip ClipCoinsAdded;

		public AudioClip ClipGemAdded;

		public AudioClip ClipGemsAdded;

		private void OnEnable()
		{
			CoinsMessage.CoinsLabelDisappeared += OnCurrencyGetted;
		}

		private void OnDisable()
		{
			CoinsMessage.CoinsLabelDisappeared -= OnCurrencyGetted;
		}

		private void OnCurrencyGetted(bool isGems, int count)
		{
			float delay = ((!Defs.isMulti && GameConnect.isCampaign && TrainingController.TrainingCompleted) ? 0f : PlayDelay);
			StartCoroutine(PlaySounds(isGems, count < 2, delay));
		}

		private IEnumerator PlaySounds(bool isGems, bool oneCoin, float delay)
		{
			if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && Defs.isSoundFX)
			{
				yield return new WaitForRealSeconds(delay);
				AudioClip clip = ((!isGems) ? ((oneCoin && ClipCoinAdded != null) ? ClipCoinAdded : ClipCoinsAdded) : ((oneCoin && ClipCoinAdded != null) ? ClipGemAdded : ClipGemsAdded));
				NGUITools.PlaySound(clip);
			}
		}
	}
}
