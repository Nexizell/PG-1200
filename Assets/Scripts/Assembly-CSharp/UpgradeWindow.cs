using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CResetToBeginning_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TweenColor tw;

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
		public _003CResetToBeginning_003Ed__2(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				tw.ResetToBeginning();
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

	public TweenColor[] upgrades;

	public void SetUpgrade(int num, int minBoughtIndex)
	{
		for (int i = 0; i < upgrades.Length; i++)
		{
			upgrades[i].gameObject.SetActive(i <= num);
			if (i <= minBoughtIndex)
			{
				upgrades[i].enabled = false;
				upgrades[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				upgrades[i].enabled = true;
				StartCoroutine(ResetToBeginning(upgrades[i]));
				upgrades[i].ResetToBeginning();
			}
		}
	}

	private IEnumerator ResetToBeginning(TweenColor tw)
	{
		yield return null;
		tw.ResetToBeginning();
	}
}
