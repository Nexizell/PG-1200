using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003Clightning_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Lightning _003C_003E4__this;

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
		public _003Clightning_003Ed__5(int _003C_003E1__state)
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
				goto IL_002a;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.child.SetActive(true);
				_003C_003E4__this.sound.SetActive(false);
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.child.SetActive(false);
				if (Defs.isSoundFX)
				{
					_003C_003E4__this.sound.SetActive(true);
				}
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.child.SetActive(true);
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.child.SetActive(false);
					goto IL_002a;
				}
				IL_002a:
				_003C_003E2__current = new WaitForSeconds(UnityEngine.Random.Range(_003C_003E4__this.lightningMinTime, _003C_003E4__this.lightningMaxTime));
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

	public GameObject sound;

	public float lightningMinTime = 30f;

	public float lightningMaxTime = 90f;

	private void Start()
	{
		if (!(child == null))
		{
			if (sound != null)
			{
				sound.SetActive(false);
			}
			StartCoroutine(lightning());
		}
	}

	private IEnumerator lightning()
	{
		while (true)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(lightningMinTime, lightningMaxTime));
			child.SetActive(true);
			sound.SetActive(false);
			yield return new WaitForSeconds(0.1f);
			child.SetActive(false);
			if (Defs.isSoundFX)
			{
				sound.SetActive(true);
			}
			yield return new WaitForSeconds(0.1f);
			child.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			child.SetActive(false);
		}
	}
}
