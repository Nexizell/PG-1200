using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RemoveRay : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public RemoveRay _003C_003E4__this;

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
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (Time.realtimeSinceStartup - _003CstartTime_003E5__1 < _003C_003E4__this.lifetime)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			UnityEngine.Object.Destroy(_003C_003E4__this.gameObject);
			return false;
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

	public float lifetime = 0.7f;

	public float length = 100f;

	private IEnumerator Start()
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < lifetime)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(gameObject);
	}
}
