using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyBankShopView : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CUpdateX3Coroutine_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyBankShopView _003C_003E4__this;

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
			public _003CUpdateX3Coroutine_003Ed__7(int _003C_003E1__state)
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
				_003C_003E4__this.UpdateEventX3RemainedTime();
				_003C_003E2__current = new WaitForRealSeconds(_003C_003E4__this._updateX3Delay);
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
		protected internal TextGroup _textGroupX3;

		[SerializeField]
		protected internal float _updateX3Delay = 0.5f;

		[SerializeField]
		protected internal TweenColor _colorBlinkForX3;

		private string _localizeSaleLabel;

		private void Start()
		{
			PromoActionsManager.EventX3Updated += OnEventX3Updated;
			OnEventX3Updated();
			LocalizationStore.AddEventCallAfterLocalize(ChangeLocalizeLabel);
			ChangeLocalizeLabel();
		}

		private void OnEnable()
		{
			if (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active)
			{
				StartCoroutine(UpdateX3Coroutine());
			}
		}

		private void OnDestroy()
		{
			PromoActionsManager.EventX3Updated -= OnEventX3Updated;
			LocalizationStore.DelEventCallAfterLocalize(ChangeLocalizeLabel);
		}

		private IEnumerator UpdateX3Coroutine()
		{
			while (true)
			{
				UpdateEventX3RemainedTime();
				yield return new WaitForRealSeconds(_updateX3Delay);
			}
		}

		private void UpdateEventX3RemainedTime()
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(PromoActionsManager.sharedManager.EventX3RemainedTime);
			string text = ((timeSpan.Days <= 0) ? string.Format("{0}: {1:00}:{2:00}:{3:00}", _localizeSaleLabel, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", _localizeSaleLabel, timeSpan.Days, (timeSpan.Days == 1) ? "Day" : "Days", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
			_textGroupX3.Text = text;
			if (_colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !_colorBlinkForX3.enabled)
			{
				_colorBlinkForX3.enabled = true;
			}
		}

		private void ChangeLocalizeLabel()
		{
			_localizeSaleLabel = LocalizationStore.Get("Key_0419");
		}

		private void OnEventX3Updated()
		{
			_textGroupX3.gameObject.SetActive(PromoActionsManager.sharedManager.IsEventX3Active);
			if (base.gameObject.activeInHierarchy && PromoActionsManager.sharedManager.IsEventX3Active)
			{
				StopCoroutine(UpdateX3Coroutine());
				StartCoroutine(UpdateX3Coroutine());
			}
		}

		private void OnClick()
		{
			MainMenuController.sharedController.ShowBankWindow();
		}
	}
}
