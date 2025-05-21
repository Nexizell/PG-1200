using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.WP8;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public abstract class PetEngine : StateMachine<PetState>, IDamageable
	{
		protected class PetIdleState : State<PetState>
		{
			private PetEngine ctx;

			public PetIdleState(PetEngine context)
				: base(PetState.idle, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				ctx.ShowModel();
				ctx.PlaySound(null);
			}

			public override void Update()
			{
				if (ctx.Owner.isKilled || ctx.Owner.transform.position.y < -5000f || !ctx.InRange(ctx.ThisTransform.position, ctx.MovePosition, ctx.Info.MaxToOwnerDistance + 1f))
				{
					To(PetState.teleport);
				}
				else if (!ctx.IsAlive)
				{
					To(PetState.death);
				}
				else if (ctx.Target != null && ctx.IsVisible(ctx.Target.gameObject))
				{
					if (!ctx.InRange(ctx.Target.position, ctx.ThisTransform.position, ctx.Info.AttackStopDistance))
					{
						To(PetState.moveToTarget);
					}
					else if (!ctx.InAttackState)
					{
						To(PetState.attack);
					}
				}
				else if (ctx.CanMoveToPlayer && !ctx.InRange(ctx.ThisTransform.position, ctx.MovePosition, ctx.Info.MinToOwnerDistance + 1f))
				{
					To(PetState.moveToOwner);
				}
			}
		}

		protected class PetAttackState : State<PetState>
		{
			private PetEngine ctx;

			private float _animationTime;

			private float _animationTimeElapsed;

			private Transform _lockedTarget;

			public PetAttackState(PetEngine context)
				: base(PetState.attack, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				ctx.InAttackState = true;
				if (ctx.Owner.isKilled)
				{
					To(PetState.teleport);
					return;
				}
				if (ctx.Target == null)
				{
					To(PetState.idle);
					return;
				}
				if ((ctx.MoveToTargetPosition.Value - ctx.ThisTransform.position).sqrMagnitude > Mathf.Pow(ctx.Info.AttackStopDistance, 2f))
				{
					ctx.To(PetState.moveToTarget);
					return;
				}
				_lockedTarget = ctx.Target;
				ctx.AnimationHandler.SubscribeTo(ctx.GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Custom1, false, AttackAnimationCallback);
				_animationTime = ctx.GetAnimationPlayTime(PetAnimationType.Attack);
				_animationTimeElapsed = 0f;
				ctx.PlayAnimation(PetAnimationType.Attack);
			}

			private void AttackAnimationCallback()
			{
				ctx.AnimationHandler.Unsubscribe(ctx.GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Custom1, AttackAnimationCallback);
				if (ctx.CurrentState == this)
				{
					if (_lockedTarget == ctx.Target)
					{
						ctx.AttackTarget();
						return;
					}
					ctx.InAttackState = false;
					To(PetState.idle);
				}
			}

			public override void Update()
			{
				base.Update();
				_animationTimeElapsed += Time.deltaTime;
				if (_animationTimeElapsed >= _animationTime || ctx.Target == null)
				{
					_animationTimeElapsed = 0f;
					ctx.InAttackState = false;
					To(PetState.idle);
				}
				else
				{
					Vector3 worldPosition = new Vector3(ctx.Target.position.x, ctx.ThisTransform.position.y, ctx.Target.position.z);
					ctx.ThisTransform.LookAt(worldPosition);
				}
			}

			public override void Out(PetState toState)
			{
				base.Out(toState);
				ctx.InAttackState = false;
				_animationTime = 0f;
				_animationTimeElapsed = 0f;
				_lockedTarget = null;
			}
		}

		protected class PetDeathState : State<PetState>
		{
			private PetEngine ctx;

			public PetDeathState(PetEngine context)
				: base(PetState.death, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				float animationPlayTime = ctx.GetAnimationPlayTime(PetAnimationType.Dead);
				ctx.DeferredAction(animationPlayTime, OnAnimationDeathEnded);
				ctx.PlayAnimation(PetAnimationType.Dead);
				ctx.PlaySound(ctx.ClipDeath);
				ctx.Owner.PetKilled();
				if (ctx.Info.petExplode)
				{
					ctx.Explode();
				}
			}

			private void OnAnimationDeathEnded()
			{
				ctx.HideModel();
				if (ctx.EffectHide != null)
				{
					ctx.EffectHide.OnEffectCompleted.AddListener(OnAnimationHideEnded);
					ctx.PlayHideEffect();
				}
				else
				{
					OnAnimationHideEnded();
				}
			}

			private void OnAnimationHideEnded()
			{
				if (ctx.EffectHide != null)
				{
					ctx.EffectHide.OnEffectCompleted.RemoveListener(OnAnimationHideEnded);
				}
				ctx.ThisTransform.position = Vector3.down * 10000f;
				ctx.To(PetState.respawn);
			}
		}

		protected class PetRespawnState : State<PetState>
		{
			private PetEngine ctx;

			public PetRespawnState(PetEngine context)
				: base(PetState.respawn, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				ctx._dieTime = Time.realtimeSinceStartup;
			}

			public override void Out(PetState toState)
			{
				base.Out(toState);
			}

			public override void Update()
			{
				base.Update();
				ctx.ThisTransform.position = Vector3.down * 10000f;
				if (!(ctx.RespawnTimeLeft > 0f))
				{
					ctx._dieTime = -1f;
					ctx.CurrentHealth = ctx.Info.HP;
					if (Defs.isMulti && ctx.IsMine)
					{
						ctx.SendSynhCurrentHealth();
					}
					ctx.IsImmortal = true;
					To(PetState.teleport);
				}
			}
		}

		protected class PetTeleportState : State<PetState>
		{
			private PetEngine ctx;

			private bool _teleportRunning;

			private bool _effectPlayed;

			public PetTeleportState(PetEngine context)
				: base(PetState.teleport, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				ctx.HideModel();
				if (!ctx.IsAlive)
				{
					To(PetState.respawn);
				}
				else
				{
					_teleportRunning = true;
				}
			}

			public override void Update()
			{
				if (ctx.CanShowPet() && ctx.OwnerIsGrounded)
				{
					ctx.ShowModel();
					ctx.WarpToOwner();
					ctx.PlayShowEffect();
					To(PetState.idle);
					_teleportRunning = false;
				}
				else
				{
					if (!_effectPlayed)
					{
						_effectPlayed = true;
						ctx.PlayShowEffect();
					}
					ctx.ThisTransform.position = Vector3.down * 10000f;
				}
			}

			public override void Out(PetState toState)
			{
				base.Out(toState);
				_effectPlayed = false;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CLoadBuff_003Ed__144 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PetEngine _003C_003E4__this;

			private ResourceRequest _003Crequest_003E5__1;

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
			public _003CLoadBuff_003Ed__144(int _003C_003E1__state)
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
					_003C_003E4__this._petBuffPrefabLoading = true;
					_003Crequest_003E5__1 = Resources.LoadAsync<GameObject>("Pets/pet_buf");
					_003C_003E2__current = _003Crequest_003E5__1;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E4__this._petBuffPrefabLoading = false;
					if (_003Crequest_003E5__1.isDone)
					{
						_003C_003E4__this._petBuff = UnityEngine.Object.Instantiate(_003Crequest_003E5__1.asset as GameObject);
						_003C_003E4__this._petBuff.transform.parent = _003C_003E4__this.ThisTransform;
						_003C_003E4__this._petBuff.transform.localPosition = Vector3.zero;
						_003C_003E4__this._petBuff.transform.localScale = Vector3.one;
						_003C_003E4__this._petBuff.transform.rotation = Quaternion.identity;
					}
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
		internal sealed class _003C_003Ec__DisplayClass175_0
		{
			public bool animationPlaying;

			internal void _003CPlayDeathEffectsCoroutine_003Eb__0()
			{
				animationPlaying = false;
			}

			internal bool _003CPlayDeathEffectsCoroutine_003Eb__1()
			{
				return animationPlaying;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CPlayDeathEffectsCoroutine_003Ed__175 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PetEngine _003C_003E4__this;

			private float _003CtimeLeft_003E5__1;

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
			public _003CPlayDeathEffectsCoroutine_003Ed__175(int _003C_003E1__state)
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
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass175_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass175_0();
					if (_003C_003E4__this._deathEffectIsPlaying)
					{
						return false;
					}
					_003C_003E4__this._deathEffectIsPlaying = true;
					CS_0024_003C_003E8__locals0.animationPlaying = true;
					float animationPlayTime = _003C_003E4__this.GetAnimationPlayTime(PetAnimationType.Dead);
					_003C_003E4__this.DeferredAction(animationPlayTime, delegate
					{
						CS_0024_003C_003E8__locals0.animationPlaying = false;
					});
					_003C_003E4__this.PlayAnimation(PetAnimationType.Dead);
					_003C_003E2__current = new WaitWhile(() => CS_0024_003C_003E8__locals0.animationPlaying);
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E4__this.PlayHideEffect();
					_003CtimeLeft_003E5__1 = _003C_003E4__this.EffectHide.WaitTime;
					break;
				case 2:
					_003C_003E1__state = -1;
					break;
				}
				if (_003CtimeLeft_003E5__1 > 0f)
				{
					_003CtimeLeft_003E5__1 -= Time.deltaTime;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E4__this.gameObject.Equals(null))
				{
					return false;
				}
				_003C_003E4__this.transform.position = Vector3.down * 10000f;
				_003C_003E4__this._deathEffectIsPlaying = false;
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
		internal sealed class _003CDeferredActionCoroutine_003Ed__177 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public float runAfterSecs;

			public Action act;

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
			public _003CDeferredActionCoroutine_003Ed__177(int _003C_003E1__state)
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
					_003C_003E2__current = new WaitForSeconds(runAfterSecs);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					if (act != null)
					{
						act();
					}
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

		private GameObject _petBuff;

		[ReadOnly]
		public SaltedFloatEps _CurrentHealth = new SaltedFloatEps(1f);

		public string PetName;

		[SerializeField]
		protected PetInfo _info;

		[SerializeField]
		public Transform _lookPoint;

		[SerializeField]
		public PetEffectHandler EffectShow;

		[SerializeField]
		public PetEffectHandler EffectHide;

		[SerializeField]
		protected float PlayProfileAnimationAfterSecs = 3f;

		[SerializeField]
		[Range(0f, 1f)]
		protected float ToRunSpeedPercent = 0.85f;

		public List<PetAnimation> Animations = new List<PetAnimation>
		{
			new PetAnimation
			{
				Type = PetAnimationType.Idle,
				AnimationName = "Idle"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Walk,
				AnimationName = "Walk"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Attack,
				AnimationName = "Attack"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Dead,
				AnimationName = "Dead"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Profile,
				AnimationName = "Profile"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Walk,
				AnimationName = "Run"
			}
		};

		[Header("-== Sounds ==-")]
		public AudioClip ClipWalk;

		public AudioClip ClipAttack;

		public AudioClip ClipReceiveDamage;

		public AudioClip ClipDeath;

		public AudioClip ClipTap;

		[ReadOnly]
		[SerializeField]
		[Header("-== from script setted ==-")]
		protected internal Player_move_c _owner;

		[ReadOnly]
		public Transform Target;

		[SerializeField]
		[ReadOnly]
		protected Player_move_c Offender;

		[ReadOnly]
		public TargetScanner Scanner;

		public Collider BodyCollider;

		public Collider HeadCollider;

		[NonSerialized]
		public Transform ThisTransform;

		[ReadOnly]
		public NetworkPetEngineSynch synchScript;

		[SerializeField]
		[ReadOnly]
		protected internal GameObject _model;

		[SerializeField]
		[ReadOnly]
		protected internal Animation _animator;

		[SerializeField]
		[ReadOnly]
		protected internal AnimationHandler _animationHandler;

		[ReadOnly]
		[SerializeField]
		protected internal PetHighlightComponent _highlightComponentComponentValue;

		[SerializeField]
		[ReadOnly]
		protected internal bool _isMine;

		[NonSerialized]
		public PhotonView photonView;

		private AdvancedEffects advancedEffects;

		private float _dieTime;

		[ReadOnly]
		[SerializeField]
		protected internal float _immortalStartTime;

		private PetHealingComponent _healthComponent;

		[ReadOnly]
		public bool isExploded;

		[ReadOnly]
		[SerializeField]
		protected internal PetAnimation _currAnimation;

		[SerializeField]
		protected internal bool NOT_SCALE_ANIMS;

		[SerializeField]
		[ReadOnly]
		protected internal float _stayTime;

		protected Vector3? _prevPosition;

		[SerializeField]
		[ReadOnly]
		protected internal float _acceleration;

		[SerializeField]
		[ReadOnly]
		protected internal float _speed;

		private const float SpeedMonInterval = 0.1f;

		private float _speedMonTime;

		private float _speedTempAcc;

		private bool _petBuffPrefabLoading;

		private bool _deathEffectIsPlaying;

		private AudioSource _audioSourceOneValue;

		private AudioSource _audioSourceTwoValue;

		public float CurrentHealth
		{
			get
			{
				return _CurrentHealth.value;
			}
			set
			{
				_CurrentHealth.value = value;
				SetCollidersEnabled(value > 0f);
			}
		}

		public Player_move_c Owner
		{
			get
			{
				return _owner;
			}
			private set
			{
				_owner = value;
			}
		}

		public PetInfo Info
		{
			get
			{
				return _info;
			}
		}

		public GameObject Model
		{
			get
			{
				if (_model == null)
				{
					_model = base.gameObject.GetChildGameObject("Body", true);
					if (_model == null)
					{
						UnityEngine.Debug.LogErrorFormat("[PETS] model object not found for pet '{0}'. Please rename root model object to 'Body'", base.gameObject.name);
					}
				}
				return _model;
			}
		}

		public Animation Animator
		{
			get
			{
				if (_animator == null)
				{
					_animator = Model.GetComponent<Animation>();
				}
				return _animator;
			}
		}

		public AnimationHandler AnimationHandler
		{
			get
			{
				if (_animationHandler == null)
				{
					_animationHandler = Model.GetComponent<AnimationHandler>();
				}
				return _animationHandler;
			}
		}

		private PetHighlightComponent _highlightComponent
		{
			get
			{
				if (_highlightComponentComponentValue == null)
				{
					_highlightComponentComponentValue = base.gameObject.GetComponent<PetHighlightComponent>();
				}
				return _highlightComponentComponentValue;
			}
		}

		public bool PetAlive { get; private set; }

		public bool IsAlive
		{
			get
			{
				return CurrentHealth > 0f;
			}
		}

		protected virtual Vector3? MoveToTargetPosition
		{
			get
			{
				if (Target != null)
				{
					return Target.position;
				}
				return null;
			}
		}

		public abstract Vector3 MovePosition { get; }

		public virtual bool CanMoveToPlayer
		{
			get
			{
				return true;
			}
		}

		public bool IsMine
		{
			get
			{
				return _isMine;
			}
			set
			{
				_isMine = value;
			}
		}

		public float RespawnTime
		{
			get
			{
				return Info.RespawnTime;
			}
		}

		public float RespawnTimeLeft
		{
			get
			{
				if (_dieTime < 0f)
				{
					return 0f;
				}
				return _dieTime + Info.RespawnTime - Time.realtimeSinceStartup;
			}
		}

		public bool IsImmortal
		{
			get
			{
				return _immortalStartTime > 0f;
			}
			set
			{
				if (value)
				{
					_immortalStartTime = Time.realtimeSinceStartup;
				}
				else
				{
					_immortalStartTime = 0f;
				}
			}
		}

		public bool isLivingTarget
		{
			get
			{
				return true;
			}
		}

		protected virtual State<PetState> IdleState
		{
			get
			{
				return new PetIdleState(this);
			}
		}

		protected virtual State<PetState> AttackState
		{
			get
			{
				return new PetAttackState(this);
			}
		}

		protected virtual State<PetState> DeathState
		{
			get
			{
				return new PetDeathState(this);
			}
		}

		protected virtual State<PetState> RespawnState
		{
			get
			{
				return new PetRespawnState(this);
			}
		}

		protected virtual State<PetState> TeleportState
		{
			get
			{
				return new PetTeleportState(this);
			}
		}

		protected abstract State<PetState> MoveToOwnerState { get; }

		protected abstract State<PetState> MoveToTargetState { get; }

		public float StayTime
		{
			get
			{
				return _stayTime;
			}
			private set
			{
				_stayTime = value;
			}
		}

		public bool IsStay { get; private set; }

		public float Acceleration
		{
			get
			{
				return _acceleration;
			}
			private set
			{
				_acceleration = value;
			}
		}

		public bool IsMoving
		{
			get
			{
				return Acceleration > 0.01f;
			}
		}

		public float Speed
		{
			get
			{
				return _speed;
			}
			private set
			{
				_speed = value;
			}
		}

		public float ToOwnerDistance
		{
			get
			{
				return Vector3.Distance(ThisTransform.position, Owner.transform.position);
			}
		}

		public float ToTargetDistance
		{
			get
			{
				return Vector3.Distance(ThisTransform.position, Target.position);
			}
		}

		public bool OwnerIsGrounded
		{
			get
			{
				return Owner.mySkinName.firstPersonControl.character.isGrounded;
			}
		}

		public bool InAttackState { get; set; }

		public bool DeathEffectIsPlaying
		{
			get
			{
				return _deathEffectIsPlaying;
			}
		}

		public AudioSource AudioSourceOne
		{
			get
			{
				return _audioSourceOneValue ?? (_audioSourceOneValue = base.gameObject.GetComponentInChildren<AudioSource>("Source1", true));
			}
		}

		public AudioSource AudioSourceTwo
		{
			get
			{
				return _audioSourceTwoValue ?? (_audioSourceTwoValue = base.gameObject.GetComponentInChildren<AudioSource>("Source2", true));
			}
		}

		public PetAnimation GetAnimation(PetAnimationType type)
		{
			return Animations.FirstOrDefault((PetAnimation a) => a.Type == type);
		}

		public string GetAnimationName(PetAnimationType type)
		{
			PetAnimation animation = GetAnimation(type);
			if (animation == null)
			{
				return string.Empty;
			}
			return animation.AnimationName;
		}

		public float GetAnimationPlayTime(PetAnimationType animationType)
		{
			PetAnimation animation = GetAnimation(animationType);
			if (animation == null)
			{
				return 0f;
			}
			return GetAnimationPlayTime(animation.AnimationName);
		}

		public float GetAnimationPlayTime(string clipName)
		{
			foreach (AnimationState item in Animator)
			{
				if (item.clip.name == clipName)
				{
					return item.clip.length * item.speed;
				}
			}
			return 0f;
		}

		public void OwnerAttacked(Player_move_c.TypeKills typeKill, int idKiller)
		{
			if (typeKill == Player_move_c.TypeKills.mob || !Defs.isMulti || Offender != null || !IsAlive || Owner == null || base.CurrentState.StateId == PetState.none || base.CurrentState.StateId == PetState.teleport || base.CurrentState.StateId == PetState.death || base.CurrentState.StateId == PetState.respawn)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.players[i].mySkinName.pixelView != null && Initializer.players[i].mySkinName.pixelView.viewID == idKiller)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && !(Initializer.players[num] == Owner) && Vector3.Distance(Owner.transform.position, Initializer.players[num].transform.position) <= Info.OffenderDetectRange)
			{
				Offender = Initializer.players[num];
			}
		}

		public bool IsEnemyTo(Player_move_c player)
		{
			if (Owner != null && (!Defs.isMulti || GameConnect.isCOOP || Owner.Equals(player) || (GameConnect.isTeamRegim && Owner.myCommand == player.myCommand)))
			{
				return false;
			}
			return true;
		}

		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
		{
			ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, "");
		}

		public virtual void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeShoot, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
		{
			if (!IsAlive || GameConnect.isDaterRegim || IsImmortal)
			{
				return;
			}
			if (Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline))
			{
				damage *= 0.5f;
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					photonView.RPC("ApplyDamageRPC", PhotonTargets.All, damage, killerId);
				}
			}
			else
			{
				ApplyDamageFrom(damage);
			}
		}

		public bool IsDead()
		{
			return CurrentHealth <= 0f;
		}

		public void ApplyDamageFrom(float damage, int idKiller = 0)
		{
			CurrentHealth -= Mathf.Clamp(damage, 0f, float.MaxValue);
			if (_highlightComponent != null)
			{
				_highlightComponent.Hit();
			}
			if (Defs.isMulti && IsMine && CurrentHealth <= 0f && idKiller > 0 && Defs.isInet)
			{
				photonView.RPC("KilledByRPC", PhotonTargets.Others, idKiller);
			}
			if (Defs.isMulti && !IsMine && CurrentHealth <= 0f && !DeathEffectIsPlaying)
			{
				PlayDeathEffects();
			}
		}

		[PunRPC]
		[RPC]
		public void ApplyDamageRPC(float damage, int idKiller)
		{
			ApplyDamageFrom(damage, idKiller);
		}

		[PunRPC]
		[RPC]
		public void KilledByRPC(int idKiller)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.skinNamePixelView.viewID == idKiller)
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddScoreKillPet();
			}
		}

		protected bool IsMoveAnimation(PetAnimationType animationType)
		{
			if (animationType != PetAnimationType.Walk)
			{
				return animationType == PetAnimationType.Run;
			}
			return true;
		}

		public virtual void PlayAnimation(PetAnimationType animationType)
		{
			if (_currAnimation != null && !string.IsNullOrEmpty(_currAnimation.AnimationName))
			{
				Animator[_currAnimation.AnimationName].speed = 1f;
				_currAnimation = null;
			}
			PetAnimation animation = GetAnimation(animationType);
			if (animation == null)
			{
				if (animationType != PetAnimationType.Run)
				{
					return;
				}
				animationType = PetAnimationType.Walk;
				animation = GetAnimation(animationType);
				if (animation == null)
				{
					return;
				}
			}
			Animator.Play(animation.AnimationName);
			_currAnimation = animation;
			if (_currAnimation.Type == PetAnimationType.Dead)
			{
				PlaySound(ClipDeath);
			}
			if (IsMine)
			{
				SetMovementAnimSpeed();
			}
			if (IsMine && Defs.isMulti && animationType != PetAnimationType.Run && animationType != PetAnimationType.Walk)
			{
				synchScript.currentAnimation = animationType;
			}
		}

		public void SetMovementAnimSpeed()
		{
			if (!NOT_SCALE_ANIMS && _currAnimation != null && (_currAnimation.Type == PetAnimationType.Walk || _currAnimation.Type == PetAnimationType.Run))
			{
				Animator[_currAnimation.AnimationName].speed = Speed * _currAnimation.SpeedModificator;
			}
		}

		protected virtual void Awake()
		{
			advancedEffects = base.gameObject.AddComponent<AdvancedEffects>();
			advancedEffects.syncInLocal = true;
			if (Defs.isMulti)
			{
				if (Defs.isInet && GetComponent<PhotonView>() != null)
				{
					photonView = GetComponent<PhotonView>();
					if (photonView.isMine)
					{
						IsMine = true;
					}
				}
			}
			else
			{
				IsMine = true;
			}
			synchScript = GetComponent<NetworkPetEngineSynch>();
			ThisTransform = base.transform;
			Scanner = GetComponent<TargetScanner>();
			Scanner.LookPoint = _lookPoint;
			BodyCollider = base.gameObject.GetComponent<Collider>();
			HeadCollider = base.gameObject.Child("HeadCollider").GetComponent<Collider>();
		}

		public void SetInfo(string id)
		{
			PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(id);
			if (playerPet != null)
			{
				_info = playerPet.Info;
				Scanner.DetectRadius = _info.TargetDetectRange;
				if (IsMine && base.enabled && _info.HealDelaySecs > 0f)
				{
					_healthComponent = base.gameObject.GetOrAddComponent<PetHealingComponent>();
				}
			}
			else if (_healthComponent != null)
			{
				UnityEngine.Object.Destroy(_healthComponent);
			}
		}

		private void Start()
		{
			Initializer.petsObj.Add(base.gameObject);
			SetOwner();
			base.Enabled = IsMine;
			if (IsMine)
			{
				SetName(Singleton<PetsManager>.Instance.GetPlayerPet(Info.Id).PetName);
				InitSM();
				SetInfo(base.gameObject.nameNoClone());
				if (_info == null)
				{
					UnityEngine.Debug.LogErrorFormat("[PETS] info for pet '{0}' not found", base.gameObject.name);
					return;
				}
				CurrentHealth = Info.HP;
				if (Defs.isMulti)
				{
					SendSynhCurrentHealth();
				}
			}
			else
			{
				StopEngine();
			}
			if (Defs.isMulti && Defs.isInet && IsMine)
			{
				PhotonObjectCacher.AddObject(base.gameObject);
			}
			IsImmortal = true;
			if (EffectShow != null)
			{
				EffectShow.gameObject.transform.SetParent(null);
			}
			if (EffectHide != null)
			{
				EffectHide.gameObject.transform.SetParent(null);
			}
		}

		private void OnEnable()
		{
			AnimationHandler.SubscribeTo(GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Custom1, false, OnAnimationAttackPlaying);
		}

		private void OnAnimationAttackPlaying()
		{
			PlaySound(ClipAttack);
		}

		private void OnDisable()
		{
			AnimationHandler.Unsubscribe(GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Custom1, OnAnimationAttackPlaying);
		}

		protected virtual void InitSM()
		{
			Clean();
			Register(IdleState);
			Register(MoveToOwnerState);
			Register(MoveToTargetState);
			Register(AttackState);
			Register(DeathState);
			Register(RespawnState);
			Register(TeleportState);
			if (base.Enabled)
			{
				To(PetState.idle);
			}
		}

		protected virtual void StopEngine()
		{
			Scanner.enabled = false;
		}

		public bool CanShowPet()
		{
			if (!(Owner.transform.position.y < -5000f))
			{
				return !Owner.isKilled;
			}
			return false;
		}

		public virtual float CalculateSpeedMultyplierByDistance()
		{
			if (base.CurrentState.StateId == PetState.moveToOwner)
			{
				if (!(ToOwnerDistance < Info.MaxToOwnerDistance * 0.3f))
				{
					return 1f + 0.05f * ToOwnerDistance;
				}
				return 1f;
			}
			if (base.CurrentState.StateId == PetState.moveToTarget)
			{
				if (!(ToTargetDistance < Info.AttackStopDistance))
				{
					return 1f + 0.1f * ToTargetDistance;
				}
				return 1f;
			}
			return 1f;
		}

		protected virtual void Update()
		{
			if (_prevPosition.HasValue)
			{
				Acceleration = Mathf.Abs(Vector3.Distance(_prevPosition.Value, ThisTransform.position));
				_speedTempAcc += Acceleration;
				_speedMonTime += Time.deltaTime;
				if (_speedMonTime >= 0.1f)
				{
					Speed = _speedTempAcc / _speedMonTime;
					_speedTempAcc = 0f;
					_speedMonTime = 0f;
				}
			}
			_prevPosition = ThisTransform.position;
			StayTime = (IsMoving ? 0f : (StayTime += Time.deltaTime));
			IsStay = StayTime >= PlayProfileAnimationAfterSecs;
			if (Owner != null)
			{
				if (Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline) && IsAlive)
				{
					if (_petBuff != null)
					{
						_petBuff.SetActiveSafe(true);
					}
					else if (!_petBuffPrefabLoading)
					{
						StartCoroutine(LoadBuff());
					}
				}
				else if (_petBuff != null)
				{
					_petBuff.SetActiveSafe(false);
				}
			}
			if (!IsMine || Owner == null)
			{
				return;
			}
			if (Owner.isKilled)
			{
				Target = null;
				Offender = null;
				IsImmortal = true;
				if (base.CurrentState == null || (base.CurrentState.StateId != PetState.teleport && base.CurrentState.StateId != PetState.respawn))
				{
					if (IsMine && Info.petExplode && !isExploded)
					{
						isExploded = true;
						Explode();
					}
					To(PetState.teleport);
				}
				Tick();
				return;
			}
			if (!CanShowPet())
			{
				if (base.CurrentState == null || (base.CurrentState.StateId != PetState.teleport && base.CurrentState.StateId != PetState.respawn))
				{
					To(PetState.teleport);
				}
				Tick();
				return;
			}
			if (IsMine && PetName != Singleton<PetsManager>.Instance.GetPlayerPet(Info.Id).PetName)
			{
				SetName(Singleton<PetsManager>.Instance.GetPlayerPet(Info.Id).PetName);
			}
			if (Owner != null && IsMine && base.Enabled && !Owner.isKilled)
			{
				isExploded = false;
				SetTarget();
				Tick();
			}
			if (base.CurrentState == null || (base.CurrentState.StateId != PetState.attack && base.CurrentState.StateId != PetState.death && base.CurrentState.StateId != PetState.respawn && base.CurrentState.StateId != PetState.teleport))
			{
				SetMovementAnimation();
			}
			if (IsImmortal)
			{
				BlinkImmortal();
			}
		}

		private void SetTarget()
		{
			Target = null;
			if (Offender != null)
			{
				if (Offender.isKilled || Vector3.Distance(ThisTransform.position, Owner.transform.position) > Info.MaxToOwnerDistance)
				{
					Offender = null;
				}
				else if (Offender.gameObject.transform != null)
				{
					Target = Offender.gameObject.transform.root;
				}
			}
			if (Target == null)
			{
				Target = ((Scanner.Target != null) ? Scanner.Target.transform : null);
			}
		}

		private IEnumerator LoadBuff()
		{
			_petBuffPrefabLoading = true;
			ResourceRequest request = Resources.LoadAsync<GameObject>("Pets/pet_buf");
			yield return request;
			_petBuffPrefabLoading = false;
			if (request.isDone)
			{
				_petBuff = UnityEngine.Object.Instantiate(request.asset as GameObject);
				_petBuff.transform.parent = ThisTransform;
				_petBuff.transform.localPosition = Vector3.zero;
				_petBuff.transform.localScale = Vector3.one;
				_petBuff.transform.rotation = Quaternion.identity;
			}
		}

		public void BlinkImmortal()
		{
			if (!(Owner == null))
			{
				_highlightComponent.ImmortalBlinkStart(Owner.maxTimerImmortality);
				if (_immortalStartTime + Owner.maxTimerImmortality <= Time.realtimeSinceStartup)
				{
					IsImmortal = false;
					_highlightComponent.ImmortalBlinkStop();
				}
			}
		}

		public virtual void SetCollidersEnabled(bool enabled)
		{
			if (BodyCollider.enabled != enabled)
			{
				BodyCollider.enabled = enabled;
			}
			if (HeadCollider.enabled != enabled)
			{
				HeadCollider.enabled = enabled;
			}
		}

		public PetAnimationType SetMovementAnimation()
		{
			PetAnimationType petAnimationType = ((!IsMoving) ? (IsStay ? PetAnimationType.Profile : PetAnimationType.Idle) : ((Speed < Info.SpeedModif * Mathf.Clamp(ToRunSpeedPercent, 0f, 1f)) ? PetAnimationType.Walk : PetAnimationType.Run));
			PlayAnimation(petAnimationType);
			PlaySound((petAnimationType == PetAnimationType.Walk || petAnimationType == PetAnimationType.Run) ? ClipWalk : null);
			return petAnimationType;
		}

		public void Destroy()
		{
			base.Enabled = false;
			if (!IsMine)
			{
				return;
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					PhotonNetwork.Destroy(base.gameObject);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void OnDestroy()
		{
			if (Initializer.petsObj.Contains(base.gameObject))
			{
				Initializer.petsObj.Remove(base.gameObject);
			}
			PhotonObjectCacher.RemoveObject(base.gameObject);
			if (EffectShow != null)
			{
				UnityEngine.Object.Destroy(EffectShow.gameObject);
			}
			if (EffectHide != null)
			{
				UnityEngine.Object.Destroy(EffectHide.gameObject);
			}
		}

		public void PlayShowEffect()
		{
			if (!(EffectShow == null))
			{
				EffectShow.gameObject.transform.position = ThisTransform.position;
				EffectShow.Play();
			}
		}

		public void PlayHideEffect()
		{
			if (!(EffectHide == null))
			{
				EffectHide.gameObject.transform.position = ThisTransform.position;
				EffectHide.Play();
			}
		}

		public void SetOwner()
		{
			if (Defs.isMulti && !IsMine)
			{
				if (Defs.isInet)
				{
					Owner = Initializer.GetPlayerMoveCWithPhotonOwnerID(photonView.ownerId);
				}
			}
			else
			{
				Owner = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}

		public bool InRange(Vector3 first, Vector3 second, float range)
		{
			return (first - second).sqrMagnitude <= Mathf.Pow(range, 2f);
		}

		public bool IsVisible(GameObject target, float maxCheckDistance = 200f)
		{
			if (target == null)
			{
				return false;
			}
			BoxCollider component = ((target.transform.childCount > 0) ? target.transform.GetChild(0).GetComponent<BoxCollider>() : (component = target.GetComponent<BoxCollider>()));
			Vector3 vector = ((component != null) ? new Vector3(0f, component.center.y, 0f) : Vector3.zero);
			Vector3 direction = target.transform.position + vector - _lookPoint.position;
			Ray ray = new Ray(_lookPoint.position, direction);
			int layerMask = Tools.AllWithoutDamageCollidersMaskAndWithoutRocket & ~(1 << LayerMask.NameToLayer("Pets"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, maxCheckDistance, layerMask))
			{
				GameObject gameObject = hitInfo.collider.transform.gameObject;
				if (gameObject.Equals(target))
				{
					return true;
				}
				if (gameObject.layer == LayerMask.NameToLayer("MyPlayer"))
				{
					return true;
				}
				if (gameObject.Ancestors().Any((GameObject a) => a.Equals(target)))
				{
					return true;
				}
				return false;
			}
			return false;
		}

		protected void AttackTarget()
		{
			if (Target == null)
			{
				return;
			}
			if (Info.poisonEnabled)
			{
				Owner.PoisonShotWithEffect(Target.root.gameObject, new Player_move_c.PoisonParameters(Info.poisonType, Info.poisonDamagePercent, Info.Attack, Info.poisonTime, Info.poisonCount, Singleton<PetsManager>.Instance.GetFirstUpgrade(Info.Id).Id, WeaponSounds.TypeDead.angel));
			}
			IDamageable component = Target.GetComponent<IDamageable>();
			if (component != null)
			{
				float num = Info.Attack;
				bool flag = false;
				if (Info.criticalHitChance > 0)
				{
					flag = Info.criticalHitChance >= UnityEngine.Random.Range(0, 100);
				}
				if (flag)
				{
					num *= Info.criticalHitCoef;
				}
				if (Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline))
				{
					num *= 2f;
				}
				num *= EffectsController.PetDamageModificator();
				component.ApplyDamage(num, this, flag ? Player_move_c.TypeKills.critical : Player_move_c.TypeKills.pet, WeaponSounds.TypeDead.angel, Singleton<PetsManager>.Instance.GetFirstUpgrade(Info.Id).Id, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
			}
		}

		public void Explode()
		{
			Action act = delegate
			{
				Initializer.TargetsList targetsList = new Initializer.TargetsList(Owner);
				float num = Info.explosionRadius * Info.explosionRadius;
				float num2 = Info.Attack * Info.explosionDamageMultiplier;
				if (Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline))
				{
					num2 *= 2f;
				}
				num2 *= EffectsController.PetDamageModificator();
				foreach (Transform item in targetsList)
				{
					if ((item.position - ThisTransform.position).sqrMagnitude < num)
					{
						IDamageable component = item.GetComponent<IDamageable>();
						if (component != null)
						{
							component.ApplyDamage(num2, this, Player_move_c.TypeKills.pet, WeaponSounds.TypeDead.explosion, Singleton<PetsManager>.Instance.GetFirstUpgrade(Info.Id).Id, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
						}
					}
				}
				SendExplosionRPC();
			};
			DeferredAction(Info.explosionDelay, act);
		}

		private void SendExplosionRPC()
		{
			if (Defs.isMulti && Defs.isInet)
			{
				photonView.RPC("ShowExplosion", PhotonTargets.Others);
			}
			ShowExplosion();
		}

		[PunRPC]
		[RPC]
		public void ShowExplosion()
		{
			string b = (string.IsNullOrEmpty(Info.explosionPrefabName) ? "Explosion1" : Info.explosionPrefabName);
			Vector3 position = ThisTransform.position;
			string text = ResPath.Combine("Explosions", b);
			GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(text);
			if (objectFromName != null)
			{
				objectFromName.transform.position = position;
			}
			Player_move_c myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			if (!(myPlayerMoveC == null) && !IsMine)
			{
				myPlayerMoveC.ImpactPlayer(ThisTransform.position, Info.explosionRadius, Info.explosionImpulse, Info.explosionImpulse, !Defs.isMulti || IsMine);
			}
		}

		public virtual void WarpToOwner()
		{
			ThisTransform.position = MovePosition;
		}

		public virtual void HideModel()
		{
			Model.SetActiveSafe(false);
			if (Model.activeInHierarchy)
			{
				PlayShowEffect();
				Model.SetActive(false);
			}
		}

		public virtual void ShowModel()
		{
			Model.SetActiveSafe(true);
		}

		public void PlayDeathEffects()
		{
			CoroutineRunner.Instance.StartCoroutine(PlayDeathEffectsCoroutine());
		}

		private IEnumerator PlayDeathEffectsCoroutine()
		{
			if (!_deathEffectIsPlaying)
			{
				_deathEffectIsPlaying = true;
				bool animationPlaying = true;
				float animationPlayTime = GetAnimationPlayTime(PetAnimationType.Dead);
				DeferredAction(animationPlayTime, delegate
				{
					animationPlaying = false;
				});
				PlayAnimation(PetAnimationType.Dead);
				yield return new WaitWhile(() => animationPlaying);
				PlayHideEffect();
				float timeLeft = EffectHide.WaitTime;
				while (timeLeft > 0f)
				{
					timeLeft -= Time.deltaTime;
					yield return null;
				}
				if (!gameObject.Equals(null))
				{
					transform.position = Vector3.down * 10000f;
					_deathEffectIsPlaying = false;
				}
			}
		}

		public void DeferredAction(float runAfterSecs, Action act)
		{
			StartCoroutine(DeferredActionCoroutine(runAfterSecs, act));
		}

		private IEnumerator DeferredActionCoroutine(float runAfterSecs, Action act)
		{
			yield return new WaitForSeconds(runAfterSecs);
			if (act != null)
			{
				act();
			}
		}

		public float AddCurrentHealth(float incrementValue)
		{
			float num = CurrentHealth + incrementValue;
			SetCurrentHealth(num);
			return num;
		}

		public void SetCurrentHealth(float newValue)
		{
			float num = Mathf.Clamp(CurrentHealth + newValue, 0f, Info.HP);
			if (num != CurrentHealth)
			{
				CurrentHealth = num;
				if (Defs.isMulti && IsMine)
				{
					SendSynhCurrentHealth();
				}
			}
		}

		public void SendSynhCurrentHealth()
		{
			if (Defs.isInet)
			{
				photonView.RPC("SynhCurrentHealthRPC", PhotonTargets.Others, CurrentHealth);
			}
		}

		[RPC]
		[PunRPC]
		public void SynhCurrentHealthRPC(float _health)
		{
			CurrentHealth = _health;
		}

		public void SetName(string _petName)
		{
			PetName = _petName;
			if (Defs.isMulti && Defs.isInet)
			{
				photonView.RPC("SynhNameRPC", PhotonTargets.OthersBuffered, PetName);
			}
		}

		[PunRPC]
		[RPC]
		public void SynhNameRPC(string _petName)
		{
			PetName = _petName;
		}

		public void OnPhotonPlayerConnected(PhotonPlayer player)
		{
			if (IsMine)
			{
				photonView.RPC("SynhCurrentHealthRPC", player, CurrentHealth);
			}
		}

		public void PlaySound(AudioClip clip)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			AudioSource audioSource = ((clip == ClipAttack || (bool)ClipReceiveDamage) ? AudioSourceTwo : AudioSourceOne);
			if (audioSource == null)
			{
				return;
			}
			if (clip == null)
			{
				audioSource.loop = false;
			}
			else if (clip == audioSource.clip)
			{
				if (!audioSource.isPlaying)
				{
					audioSource.Play();
				}
			}
			else
			{
				audioSource.clip = clip;
				audioSource.Play();
				audioSource.loop = clip == ClipWalk;
			}
		}

		public PetEngine()
		{
		}
	}
}
