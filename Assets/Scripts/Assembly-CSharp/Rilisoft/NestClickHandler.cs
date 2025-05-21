using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class NestClickHandler : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CShowHints_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
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
			public _003CShowHints_003Ed__3(int _003C_003E1__state)
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
					HintController.instance.ShowHintByName("incubator");
					_003C_003E2__current = new WaitForSeconds(5f);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					HintController.instance.HideHintByName("incubator");
					HintController.instance.ShowHintByName("incubator_2");
					return false;
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

		private bool hintShowed;

		private void OnClick()
		{
			if (Nest.Instance != null)
			{
				Nest.Instance.Click();
				HideAndSaveHint();
			}
		}

		private void Start()
		{
			if (!(HintController.instance != null) || ExperienceController.sharedController.currentLevel < 2 || !Nest.Instance.NestCanShow() || PlayerPrefs.GetInt("NestHintShowed", 0) != 0)
			{
				return;
			}
			BindedBillboard byGameObjectName = BindedBillboard.GetByGameObjectName("ToNestBindedBillboard");
			if (byGameObjectName != null)
			{
				byGameObjectName.enabled = true;
				byGameObjectName.BindTo(() => Nest.Instance.NestGameObject.transform);
			}
			hintShowed = true;
			StartCoroutine(ShowHints());
		}

		private IEnumerator ShowHints()
		{
			HintController.instance.ShowHintByName("incubator");
			yield return new WaitForSeconds(5f);
			HintController.instance.HideHintByName("incubator");
			HintController.instance.ShowHintByName("incubator_2");
		}

		private void OnDisable()
		{
			HideAndSaveHint();
		}

		private void HideAndSaveHint()
		{
			if (hintShowed && HintController.instance != null)
			{
				hintShowed = false;
				HintController.instance.HideHintByName("incubator");
				HintController.instance.HideHintByName("incubator_2");
				PlayerPrefs.SetInt("NestHintShowed", 1);
			}
		}
	}
}
