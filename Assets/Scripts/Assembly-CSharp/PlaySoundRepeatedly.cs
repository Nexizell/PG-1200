using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaySoundRepeatedly : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSoundCoroutine_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlaySoundRepeatedly _003C_003E4__this;

		private int _003Ci_003E5__1;

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
		public _003CSoundCoroutine_003Ed__5(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.Delay);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_004c;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__1++;
				break;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_004c;
				}
				IL_004c:
				_003Ci_003E5__1 = 0;
				break;
			}
			if (_003Ci_003E5__1 < _003C_003E4__this.Repeats)
			{
				if (Defs.isSoundFX)
				{
					_003C_003E4__this.GetComponent<AudioSource>().Play();
				}
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.Between);
				_003C_003E1__state = 2;
				return true;
			}
			_003C_003E2__current = new WaitForSeconds(Mathf.Max(0f, _003C_003E4__this.Interval - _003C_003E4__this.Between * (float)_003C_003E4__this.Repeats));
			_003C_003E1__state = 3;
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

	public float Delay;

	public int Repeats = 3;

	public float Between = 1f;

	public float Interval = 60f;

	private void OnEnable()
	{
		StartCoroutine(SoundCoroutine());
	}

	private IEnumerator SoundCoroutine()
	{
		yield return new WaitForSeconds(Delay);
		while (true)
		{
			for (int i = 0; i < Repeats; i++)
			{
				if (Defs.isSoundFX)
				{
					GetComponent<AudioSource>().Play();
				}
				yield return new WaitForSeconds(Between);
			}
			yield return new WaitForSeconds(Mathf.Max(0f, Interval - Between * (float)Repeats));
		}
	}
}
