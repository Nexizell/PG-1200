using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SynchNetworkAnimation : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CUpdateState_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SynchNetworkAnimation _003C_003E4__this;

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
		public _003CUpdateState_003Ed__3(int _003C_003E1__state)
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
			_003C_003E4__this.currState.normalizedTime = (float)(PhotonNetwork.time % (double)_003C_003E4__this.anim.clip.length) / _003C_003E4__this.anim.clip.length;
			_003C_003E2__current = new WaitForSeconds(10f);
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

	private Animation anim;

	private AnimationState currState;

	private void Start()
	{
		if (Defs.isMulti && Defs.isInet)
		{
			anim = GetComponent<Animation>();
			currState = anim[anim.clip.name];
			currState.normalizedTime = (float)(PhotonNetwork.time % (double)anim.clip.length) / anim.clip.length;
			anim.Play();
			StartCoroutine(UpdateState());
		}
	}

	private IEnumerator UpdateState()
	{
		while (true)
		{
			currState.normalizedTime = (float)(PhotonNetwork.time % (double)anim.clip.length) / anim.clip.length;
			yield return new WaitForSeconds(10f);
		}
	}
}
