using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	public class FiringShotBot : BaseShootingBot
	{
		[CompilerGenerated]
		internal sealed class _003CShowFireEffect_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FiringShotBot _003C_003E4__this;

			public GameObject effect;

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
			public _003CShowFireEffect_003Ed__5(int _003C_003E1__state)
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
					if (_003C_003E4__this._isEffectFireShow)
					{
						return false;
					}
					_003C_003E4__this._isEffectFireShow = true;
					effect.SetActive(true);
					_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.timeShowFireEffect);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					effect.SetActive(false);
					_003C_003E4__this._isEffectFireShow = false;
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

		[CompilerGenerated]
		internal sealed class _003CShowFireEffect_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FiringShotBot _003C_003E4__this;

			public ParticleSystem effect;

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
			public _003CShowFireEffect_003Ed__6(int _003C_003E1__state)
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
					if (_003C_003E4__this._isEffectFireShow)
					{
						return false;
					}
					_003C_003E4__this._isEffectFireShow = true;
					effect.Play();
					_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.timeShowFireEffect);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					effect.Stop();
					_003C_003E4__this._isEffectFireShow = false;
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

		[CompilerGenerated]
		internal sealed class _003CShowFireEffect_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FiringShotBot _003C_003E4__this;

			public ParticleSystem effect;

			public Transform pointFire;

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
			public _003CShowFireEffect_003Ed__8(int _003C_003E1__state)
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
					if (_003C_003E4__this._isEffectFireShow)
					{
						return false;
					}
					_003C_003E4__this._isEffectFireShow = true;
					effect.transform.position = pointFire.position;
					effect.transform.rotation = pointFire.rotation;
					effect.Play();
					_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.timeShowFireEffect);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					effect.Stop();
					_003C_003E4__this._isEffectFireShow = false;
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

		[Header("Firing settings")]
		[Range(0.1f, 1f)]
		public float chanceMakeDamage = 1f;

		public float timeShowFireEffect = 2f;

		private bool _isEffectFireShow;

		protected override void InitializeShotsPool(int sizePool)
		{
			base.InitializeShotsPool(sizePool);
			Transform parent = ((firePoints.Length == 1) ? firePoints[0] : base.transform);
			for (int i = 0; i < bulletsEffectPool.Length; i++)
			{
				bulletsEffectPool[i].transform.parent = parent;
				bulletsEffectPool[i].transform.localPosition = Vector3.zero;
				bulletsEffectPool[i].transform.rotation = Quaternion.identity;
				bulletsEffectPool[i].GetComponent<ParticleSystem>().Stop();
			}
		}

		private ParticleSystem GetFireShotEffectFromPool()
		{
			return GetShotEffectFromPool().GetComponent<ParticleSystem>();
		}

		private IEnumerator ShowFireEffect(GameObject effect)
		{
			if (!_isEffectFireShow)
			{
				_isEffectFireShow = true;
				effect.SetActive(true);
				yield return new WaitForSeconds(timeShowFireEffect);
				effect.SetActive(false);
				_isEffectFireShow = false;
			}
		}

		private IEnumerator ShowFireEffect(ParticleSystem effect)
		{
			if (!_isEffectFireShow)
			{
				_isEffectFireShow = true;
				effect.Play();
				yield return new WaitForSeconds(timeShowFireEffect);
				effect.Stop();
				_isEffectFireShow = false;
			}
		}

		protected override void Fire(Transform pointFire, GameObject target)
		{
			ParticleSystem fireShotEffectFromPool = GetFireShotEffectFromPool();
			if (firePoints.Length == 1)
			{
				StartCoroutine(ShowFireEffect(fireShotEffectFromPool));
			}
			else
			{
				StartCoroutine(ShowFireEffect(pointFire, fireShotEffectFromPool));
			}
			if (chanceMakeDamage >= UnityEngine.Random.value)
			{
				MakeDamage(target.transform);
			}
		}

		private IEnumerator ShowFireEffect(Transform pointFire, ParticleSystem effect)
		{
			if (!_isEffectFireShow)
			{
				_isEffectFireShow = true;
				effect.transform.position = pointFire.position;
				effect.transform.rotation = pointFire.rotation;
				effect.Play();
				yield return new WaitForSeconds(timeShowFireEffect);
				effect.Stop();
				_isEffectFireShow = false;
			}
		}
	}
}
