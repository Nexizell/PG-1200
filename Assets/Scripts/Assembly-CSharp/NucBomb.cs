using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NucBomb : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NucBomb _003C_003E4__this;

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
		public _003CStart_003Ed__2(int _003C_003E1__state)
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
				_003C_003E4__this.GetComponent<AudioSource>().Play();
				_003C_003E4__this.GetComponent<AudioSource>().mute = !Defs.isSoundFX;
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.BeforeActivate);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.transform.GetChild(0).gameObject.SetActive(true);
				_003C_003E2__current = new WaitForSeconds(Mathf.Max(0f, _003C_003E4__this.BeforeDestroy - _003C_003E4__this.BeforeActivate));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				UnityEngine.Object.Destroy(_003C_003E4__this.gameObject);
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

	public float BeforeActivate = 12f;

	public float BeforeDestroy = 90f;

	private IEnumerator Start()
	{
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().mute = !Defs.isSoundFX;
		yield return new WaitForSeconds(BeforeActivate);
		transform.GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds(Mathf.Max(0f, BeforeDestroy - BeforeActivate));
		UnityEngine.Object.Destroy(gameObject);
	}

	private void FixedUpdate()
	{
		GetComponent<AudioSource>().mute = !Defs.isSoundFX;
	}
}
