using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BombsCratorNucl : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BombsCratorNucl _003C_003E4__this;

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
		public _003CStart_003Ed__3(int _003C_003E1__state)
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
				if (!Defs.isMulti)
				{
					break;
				}
				_003C_003E4__this.oldBombPeriod = (int)PhotonNetwork.time / (int)_003C_003E4__this.time;
				goto IL_004a;
			case 1:
				_003C_003E1__state = -1;
				goto IL_004a;
			case 2:
				{
					_003C_003E1__state = -1;
					if (_003C_003E4__this.bmb != null)
					{
						UnityEngine.Object.Instantiate(_003C_003E4__this.bmb);
					}
					break;
				}
				IL_004a:
				if (_003C_003E4__this.oldBombPeriod < (int)PhotonNetwork.time / (int)_003C_003E4__this.time)
				{
					_003C_003E4__this.oldBombPeriod = (int)PhotonNetwork.time / (int)_003C_003E4__this.time;
					UnityEngine.Object.Instantiate(_003C_003E4__this.bmb);
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.time);
			_003C_003E1__state = 2;
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

	public float time = 120f;

	public GameObject bmb;

	public int oldBombPeriod;

	private IEnumerator Start()
	{
		if (Defs.isMulti)
		{
			oldBombPeriod = (int)PhotonNetwork.time / (int)time;
			while (true)
			{
				if (oldBombPeriod < (int)PhotonNetwork.time / (int)time)
				{
					oldBombPeriod = (int)PhotonNetwork.time / (int)time;
					UnityEngine.Object.Instantiate(bmb);
				}
				yield return null;
			}
		}
		while (true)
		{
			yield return new WaitForSeconds(time);
			if (bmb != null)
			{
				UnityEngine.Object.Instantiate(bmb);
			}
		}
	}
}
