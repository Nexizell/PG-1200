using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class ParticleSystemDestroyer : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CStart_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ParticleSystemDestroyer _003C_003E4__this;

			private float _003CstopTime_003E5__1;

			private ParticleSystem[] _003Csystems_003E5__2;

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
			public _003CStart_003Ed__4(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				ParticleSystem[] array;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Csystems_003E5__2 = _003C_003E4__this.GetComponentsInChildren<ParticleSystem>();
					array = _003Csystems_003E5__2;
					foreach (ParticleSystem particleSystem in array)
					{
						_003C_003E4__this.m_MaxLifetime = Mathf.Max(particleSystem.startLifetime, _003C_003E4__this.m_MaxLifetime);
					}
					_003CstopTime_003E5__1 = Time.time + UnityEngine.Random.Range(_003C_003E4__this.minDuration, _003C_003E4__this.maxDuration);
					goto IL_00ad;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00ad;
				case 2:
					{
						_003C_003E1__state = -1;
						UnityEngine.Object.Destroy(_003C_003E4__this.gameObject);
						return false;
					}
					IL_00ad:
					if (Time.time < _003CstopTime_003E5__1 || _003C_003E4__this.m_EarlyStop)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					UnityEngine.Debug.Log("stopping " + _003C_003E4__this.name);
					array = _003Csystems_003E5__2;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].enableEmission = false;
					}
					_003C_003E4__this.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
					_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.m_MaxLifetime);
					_003C_003E1__state = 2;
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

		public float minDuration = 8f;

		public float maxDuration = 10f;

		private float m_MaxLifetime;

		private bool m_EarlyStop;

		private IEnumerator Start()
		{
			ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
			ParticleSystem[] array = systems;
			foreach (ParticleSystem particleSystem in array)
			{
				m_MaxLifetime = Mathf.Max(particleSystem.startLifetime, m_MaxLifetime);
			}
			float stopTime = Time.time + UnityEngine.Random.Range(minDuration, maxDuration);
			while (Time.time < stopTime || m_EarlyStop)
			{
				yield return null;
			}
			UnityEngine.Debug.Log("stopping " + name);
			array = systems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enableEmission = false;
			}
			BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(m_MaxLifetime);
			UnityEngine.Object.Destroy(gameObject);
		}

		public void Stop()
		{
			m_EarlyStop = true;
		}
	}
}
