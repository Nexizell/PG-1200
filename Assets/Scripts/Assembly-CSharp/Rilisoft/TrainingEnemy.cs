using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public sealed class TrainingEnemy : MonoBehaviour, IDamageable
	{
		internal enum State
		{
			None = 0,
			Awakened = 1,
			Dead = 2
		}

		[CompilerGenerated]
		internal sealed class _003CHighlightHitCoroutine_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TrainingEnemy _003C_003E4__this;

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
			public _003CHighlightHitCoroutine_003Ed__23(int _003C_003E1__state)
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
					_003C_003E4__this.SetTexture(_003C_003E4__this.hitTexture);
					_003C_003E2__current = new WaitForSeconds(0.125f);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E4__this.SetTexture(_003C_003E4__this.skinTexture);
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
		internal sealed class _003CAwakeCoroutine_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public float delaySeconds;

			public TrainingEnemy _003C_003E4__this;

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
			public _003CAwakeCoroutine_003Ed__25(int _003C_003E1__state)
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
					if (delaySeconds > 0f)
					{
						_003C_003E2__current = new WaitForSeconds(delaySeconds);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0050;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0050;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_00d7;
					}
					IL_00d7:
					if (_003C_003E4__this._animation.isPlaying)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					if (_003C_003E4__this.aimTarget != null)
					{
						_003C_003E4__this.aimTarget.SetActive(true);
					}
					break;
					IL_0050:
					if (!(_003C_003E4__this._animation != null))
					{
						break;
					}
					if (_003C_003E4__this._audioSource != null && _003C_003E4__this.wakeUpAudioClip != null)
					{
						_003C_003E4__this._audioSource.PlayOneShot(_003C_003E4__this.wakeUpAudioClip);
					}
					_003C_003E4__this._animation.Play("Dummy_Up", PlayMode.StopSameLayer);
					goto IL_00d7;
				}
				_003C_003E4__this._currentState = State.Awakened;
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

		[CompilerGenerated]
		internal sealed class _003CDieCoroutine_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TrainingEnemy _003C_003E4__this;

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
			public _003CDieCoroutine_003Ed__26(int _003C_003E1__state)
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
					if (!(_003C_003E4__this._animation != null))
					{
						break;
					}
					goto IL_0051;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0051;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_00d9;
					}
					IL_0051:
					if (_003C_003E4__this._animation.IsPlaying("Dummy_Damage"))
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					if (_003C_003E4__this._audioSource != null && _003C_003E4__this.dieAudioClip != null)
					{
						_003C_003E4__this._audioSource.PlayOneShot(_003C_003E4__this.dieAudioClip);
					}
					_003C_003E4__this._animation.Play("Dead", PlayMode.StopSameLayer);
					goto IL_00d9;
					IL_00d9:
					if (_003C_003E4__this._animation.isPlaying)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				if (ZombieCreator.sharedCreator != null)
				{
					ZombieCreator.sharedCreator.NumOfDeadZombies++;
				}
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

		public AudioClip wakeUpAudioClip;

		public AudioClip dieAudioClip;

		public GameObject aimTarget;

		public int hitPoints = 3;

		private SkinnedMeshRenderer meshRender;

		public Texture hitTexture;

		private Texture skinTexture;

		[ReadOnly]
		public int baseHitPoints;

		public float heightFlyOutHitEffect = 1.75f;

		private Collider _headCol;

		private AudioSource _audioSource;

		private Animation _animation;

		private State _currentState;

		private Collider HeadCollider
		{
			get
			{
				if (_headCol != null)
				{
					return _headCol;
				}
				GameObject gameObject = base.gameObject.Descendants("HeadCollider").FirstOrDefault();
				if (gameObject != null)
				{
					_headCol = gameObject.GetComponent<Collider>();
				}
				return _headCol;
			}
		}

		public bool isLivingTarget
		{
			get
			{
				return false;
			}
		}

		public void WakeUp(float delaySeconds = 0f)
		{
			StartCoroutine(AwakeCoroutine(delaySeconds));
		}

		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
		{
			ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, "");
		}

		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
		{
			ApplyDamage(damage, typeKill == Player_move_c.TypeKills.headshot);
		}

		public bool IsEnemyTo(Player_move_c player)
		{
			return true;
		}

		public bool IsDead()
		{
			return _currentState == State.Dead;
		}

		public void ApplyDamage(float damage, bool isHeadShot)
		{
			if (_currentState != State.Awakened)
			{
				return;
			}
			StartCoroutine(HighlightHitCoroutine());
			if (isHeadShot)
			{
				ShowHeadShotEffect();
			}
			else
			{
				ShowHitEffect();
			}
			hitPoints--;
			if (_animation != null)
			{
				_animation.Play("Dummy_Damage", PlayMode.StopSameLayer);
			}
			if (hitPoints <= 0)
			{
				_currentState = State.Dead;
				if (aimTarget != null)
				{
					UnityEngine.Object.Destroy(aimTarget);
					aimTarget = null;
				}
				StartCoroutine(DieCoroutine());
			}
		}

		private void Awake()
		{
			baseHitPoints = hitPoints;
			if (aimTarget != null)
			{
				aimTarget.SetActive(false);
			}
		}

		private void Start()
		{
			_audioSource = GetComponent<AudioSource>();
			_animation = GetComponent<Animation>();
			if (_animation != null)
			{
				_animation.Play("Dummy_Idle", PlayMode.StopSameLayer);
			}
			meshRender = GetComponentInChildren<SkinnedMeshRenderer>();
			if ((bool)meshRender)
			{
				meshRender.sharedMaterial = new Material(meshRender.sharedMaterial);
				skinTexture = meshRender.sharedMaterial.mainTexture;
			}
		}

		private IEnumerator HighlightHitCoroutine()
		{
			SetTexture(hitTexture);
			yield return new WaitForSeconds(0.125f);
			SetTexture(skinTexture);
		}

		public void SetTexture(Texture needTx)
		{
			if (meshRender != null)
			{
				meshRender.sharedMaterial.mainTexture = needTx;
			}
		}

		private IEnumerator AwakeCoroutine(float delaySeconds = 0f)
		{
			if (delaySeconds > 0f)
			{
				yield return new WaitForSeconds(delaySeconds);
			}
			if (_animation != null)
			{
				if (_audioSource != null && wakeUpAudioClip != null)
				{
					_audioSource.PlayOneShot(wakeUpAudioClip);
				}
				_animation.Play("Dummy_Up", PlayMode.StopSameLayer);
				while (_animation.isPlaying)
				{
					yield return null;
				}
				if (aimTarget != null)
				{
					aimTarget.SetActive(true);
				}
			}
			_currentState = State.Awakened;
		}

		private IEnumerator DieCoroutine()
		{
			if (_animation != null)
			{
				while (_animation.IsPlaying("Dummy_Damage"))
				{
					yield return null;
				}
				if (_audioSource != null && dieAudioClip != null)
				{
					_audioSource.PlayOneShot(dieAudioClip);
				}
				_animation.Play("Dead", PlayMode.StopSameLayer);
				while (_animation.isPlaying)
				{
					yield return null;
				}
			}
			if (ZombieCreator.sharedCreator != null)
			{
				ZombieCreator.sharedCreator.NumOfDeadZombies++;
			}
		}

		private void ShowHitEffect()
		{
			if (!Device.isPixelGunLow)
			{
				HitParticle currentParticle = ParticleStacks.instance.hitStack.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * heightFlyOutHitEffect);
				}
			}
		}

		private void ShowHeadShotEffect()
		{
			if (!Device.isPixelGunLow)
			{
				HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null && HeadCollider != null)
				{
					Vector3 position = ((HeadCollider is BoxCollider) ? ((BoxCollider)HeadCollider).center : ((SphereCollider)HeadCollider).center);
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, HeadCollider.transform.TransformPoint(position));
				}
			}
		}
	}
}
