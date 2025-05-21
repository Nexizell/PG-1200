using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	[Serializable]
	public class LerpControlledBob 
	{
		[CompilerGenerated]
		internal sealed class _003CDoBobCycle_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LerpControlledBob _003C_003E4__this;

			private float _003Ct_003E5__1;

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
			public _003CDoBobCycle_003Ed__4(int _003C_003E1__state)
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
					_003Ct_003E5__1 = 0f;
					goto IL_008e;
				case 1:
					_003C_003E1__state = -1;
					goto IL_008e;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_008e:
					if (_003Ct_003E5__1 < _003C_003E4__this.BobDuration)
					{
						_003C_003E4__this.m_Offset = Mathf.Lerp(0f, _003C_003E4__this.BobAmount, _003Ct_003E5__1 / _003C_003E4__this.BobDuration);
						_003Ct_003E5__1 += Time.deltaTime;
						_003C_003E2__current = new WaitForFixedUpdate();
						_003C_003E1__state = 1;
						return true;
					}
					_003Ct_003E5__1 = 0f;
					break;
				}
				if (_003Ct_003E5__1 < _003C_003E4__this.BobDuration)
				{
					_003C_003E4__this.m_Offset = Mathf.Lerp(_003C_003E4__this.BobAmount, 0f, _003Ct_003E5__1 / _003C_003E4__this.BobDuration);
					_003Ct_003E5__1 += Time.deltaTime;
					_003C_003E2__current = new WaitForFixedUpdate();
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E4__this.m_Offset = 0f;
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

		public float BobDuration;

		public float BobAmount;

		private float m_Offset;

		public float Offset()
		{
			return m_Offset;
		}

		public IEnumerator DoBobCycle()
		{
			float t2 = 0f;
			while (t2 < BobDuration)
			{
				m_Offset = Mathf.Lerp(0f, BobAmount, t2 / BobDuration);
				t2 += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			t2 = 0f;
			while (t2 < BobDuration)
			{
				m_Offset = Mathf.Lerp(BobAmount, 0f, t2 / BobDuration);
				t2 += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			m_Offset = 0f;
		}
	}
}
