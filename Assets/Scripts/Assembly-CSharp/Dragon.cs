using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dragon : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003Cdragonfly_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Dragon _003C_003E4__this;

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
		public _003Cdragonfly_003Ed__5(int _003C_003E1__state)
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
				goto IL_0032;
			case 1:
				_003C_003E1__state = -1;
				if (Defs.isSoundFX)
				{
					_003C_003E4__this.wingsFirst.SetActive(true);
				}
				_003C_003E2__current = new WaitForSeconds(3.2333333f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.wingsFirst.SetActive(false);
				_003C_003E2__current = new WaitForSeconds(6.6666665f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.child.SetActive(true);
				_003C_003E4__this.childSound.enabled = Defs.isSoundFX;
				_003C_003E2__current = new WaitForSeconds(5f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E4__this.child.SetActive(false);
				if (Defs.isSoundFX)
				{
					_003C_003E4__this.wingsSecond.SetActive(true);
				}
				_003C_003E2__current = new WaitForSeconds(4f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E4__this.wingsSecond.SetActive(false);
				_003C_003E2__current = new WaitForSeconds(23.71f);
				_003C_003E1__state = 6;
				return true;
			case 6:
				{
					_003C_003E1__state = -1;
					goto IL_0032;
				}
				IL_0032:
				_003C_003E2__current = new WaitForSeconds(6.6666665f);
				_003C_003E1__state = 1;
				return true;
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

	public GameObject child;

	private AudioSource childSound;

	public GameObject wingsFirst;

	public GameObject wingsSecond;

	private void Start()
	{
		if (!(child == null) && !(wingsFirst == null) && !(wingsSecond == null))
		{
			childSound = child.GetComponent<AudioSource>();
			StartCoroutine(dragonfly());
		}
	}

	private IEnumerator dragonfly()
	{
		while (true)
		{
			yield return new WaitForSeconds(6.6666665f);
			if (Defs.isSoundFX)
			{
				wingsFirst.SetActive(true);
			}
			yield return new WaitForSeconds(3.2333333f);
			wingsFirst.SetActive(false);
			yield return new WaitForSeconds(6.6666665f);
			child.SetActive(true);
			childSound.enabled = Defs.isSoundFX;
			yield return new WaitForSeconds(5f);
			child.SetActive(false);
			if (Defs.isSoundFX)
			{
				wingsSecond.SetActive(true);
			}
			yield return new WaitForSeconds(4f);
			wingsSecond.SetActive(false);
			yield return new WaitForSeconds(23.71f);
		}
	}
}
