using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class FlyingPetEngine : PetEngine
	{
		internal class PetMoveToOwnerState : State<PetState>
		{
			private FlyingPetEngine ctx;

			private const float _resetMonitorInterval = 0.3f;

			private float _resetMonitorTimeElapsed;

			public PetMoveToOwnerState(FlyingPetEngine context)
				: base(PetState.moveToOwner, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				if (!ctx.IsVisible(ctx.Owner.gameObject))
				{
					To(PetState.teleport);
					return;
				}
				ctx.TargetPosMon.StartMonitoring(() => ctx.Owner.transform.position);
				ctx.CheckIsMovingStart();
			}

			public override void Update()
			{
				base.Update();
				if (ctx.Owner == null || !ctx.IsAlive)
				{
					To(PetState.idle);
					return;
				}
				if (!ctx.InRange(ctx.MovePosition, ctx.ThisTransform.position, ctx.Info.MaxToOwnerDistance) || ctx.Owner.isKilled)
				{
					To(PetState.teleport);
					return;
				}
				if (ctx.InRange(ctx.ThisTransform.position, ctx.MovePosition, ctx.Info.MinToOwnerDistance))
				{
					To(PetState.idle);
					return;
				}
				if (ctx.Target != null)
				{
					To(PetState.moveToTarget);
					return;
				}
				_resetMonitorTimeElapsed += Time.deltaTime;
				if (_resetMonitorTimeElapsed >= 0.3f)
				{
					_resetMonitorTimeElapsed = 0f;
					if (ctx.IsVisible(ctx.Owner.gameObject))
					{
						ctx.TargetPosMon.Reset();
					}
				}
				ctx.Move();
			}

			public override void Out(PetState toState)
			{
				base.Out(toState);
				ctx.TargetPosMon.StopMonitoring();
				_resetMonitorTimeElapsed = 0f;
				ctx.CheckIsMovingStop();
			}
		}

		internal class PetMoveToTargetState : State<PetState>
		{
			private FlyingPetEngine ctx;

			private const float _resetMonitorInterval = 0.3f;

			private float _resetMonitorTimeElapsed;

			public PetMoveToTargetState(FlyingPetEngine context)
				: base(PetState.moveToTarget, (StateMachine<PetState>)context)
			{
				ctx = context;
			}

			public override void In(PetState fromState)
			{
				base.In(fromState);
				if (ctx.Target == null)
				{
					To(PetState.idle);
					return;
				}
				ctx.TargetPosMon.StartMonitoring(() => ctx.MoveToTargetPosition.Value);
				ctx.CheckIsMovingStart();
			}

			public override void Update()
			{
				base.Update();
				if (ctx.Target == null || !ctx.IsAlive || !ctx.MoveToTargetPosition.HasValue)
				{
					To(PetState.idle);
					return;
				}
				if (!ctx.InRange(ctx.ThisTransform.position, ctx.Owner.transform.position, ctx.Info.MaxToOwnerDistance))
				{
					To(PetState.teleport);
					return;
				}
				if (ctx.Owner.isKilled)
				{
					To(PetState.teleport);
					return;
				}
				if (ctx.Target != null && ctx.InRange(ctx.ThisTransform.position, ctx.MoveToTargetPosition.Value, ctx.Info.AttackDistance))
				{
					if (!ctx.InAttackState)
					{
						To(PetState.attack);
					}
					return;
				}
				_resetMonitorTimeElapsed += Time.deltaTime;
				if (_resetMonitorTimeElapsed >= 0.3f)
				{
					_resetMonitorTimeElapsed = 0f;
					if (ctx.IsVisible(ctx.Target.gameObject))
					{
						ctx.TargetPosMon.Reset();
					}
				}
				ctx.Move();
			}

			public override void Out(PetState toState)
			{
				base.Out(toState);
				ctx.TargetPosMon.StopMonitoring();
				_resetMonitorTimeElapsed = 0f;
				ctx.CheckIsMovingStop();
			}
		}

		[CompilerGenerated]
		internal sealed class _003CCheckIsMoving_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FlyingPetEngine _003C_003E4__this;

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
			public _003CCheckIsMoving_003Ed__19(int _003C_003E1__state)
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
					_003C_003E4__this._unwaikableElapsedTime = 0f;
				}
				if (_003C_003E4__this.IsMoving)
				{
					_003C_003E4__this._unwaikableElapsedTime = 0f;
				}
				else
				{
					_003C_003E4__this._unwaikableElapsedTime += Time.deltaTime;
					if (_003C_003E4__this._unwaikableElapsedTime >= 1f)
					{
						_003C_003E4__this._unwaikableElapsedTime = 0f;
						_003C_003E4__this.To(PetState.teleport);
						_003C_003E4__this.StopCoroutine("CheckIsMoving");
					}
				}
				_003C_003E2__current = null;
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

		public CharacterController Character;

		private TargetPositionMonitor _posMonVal;

		private Collider _characterControllerCollider;

		private const float _unwaikableTeleportTime = 1f;

		private float _unwaikableElapsedTime;

		public override Vector3 MovePosition
		{
			get
			{
				if (!(base.Owner.GetPointForFlyingPet() != null))
				{
					return base.Owner.transform.position;
				}
				return base.Owner.GetPointForFlyingPet().position;
			}
		}

		protected override Vector3? MoveToTargetPosition
		{
			get
			{
				if (Target != null)
				{
					return new Vector3(Target.position.x, Target.position.y + 1.5f, Target.position.z);
				}
				return null;
			}
		}

		protected override State<PetState> MoveToOwnerState
		{
			get
			{
				return new PetMoveToOwnerState(this);
			}
		}

		protected override State<PetState> MoveToTargetState
		{
			get
			{
				return new PetMoveToTargetState(this);
			}
		}

		private TargetPositionMonitor TargetPosMon
		{
			get
			{
				if (_posMonVal == null)
				{
					_posMonVal = GetComponent<TargetPositionMonitor>() ?? base.gameObject.AddComponent<TargetPositionMonitor>();
				}
				return _posMonVal;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			Character = GetComponent<CharacterController>();
			Collider[] components = base.gameObject.GetComponents<Collider>();
			foreach (Collider collider in components)
			{
				if (collider != BodyCollider)
				{
					_characterControllerCollider = collider;
				}
			}
		}

		protected override void StopEngine()
		{
			base.StopEngine();
			Character.enabled = false;
		}

		protected override void InitSM()
		{
			base.InitSM();
			if (Character != null)
			{
				Character.enabled = base.IsMine;
			}
		}

		public void Move()
		{
			Vector3 vector = TargetPosMon.GetCurrentPoint();
			if (Vector3.Distance(ThisTransform.position, vector) < 0.2f)
			{
				vector = (TargetPosMon.HasNextPoint() ? TargetPosMon.GetNextPoint() : Vector3.zero);
			}
			if (!(vector == Vector3.zero))
			{
				Quaternion b = Quaternion.LookRotation(vector - base.transform.position, Vector3.up);
				ThisTransform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
				Vector3 normalized = (vector - ThisTransform.position).normalized;
				float num = EffectsController.PetSpeedModificator();
				if (base.CurrentState.StateId == PetState.moveToTarget)
				{
					float max = base.Info.SpeedModif * num;
					normalized *= Mathf.Clamp(CalculateSpeedMultyplierByDistance() * base.Info.SpeedModif * num, 0f, max) * 0.015f;
				}
				else
				{
					normalized *= CalculateSpeedMultyplierByDistance() * base.Info.SpeedModif * num * 0.015f;
				}
				Character.Move(normalized);
				PlaySound(ClipWalk);
			}
		}

		private IEnumerator CheckIsMoving()
		{
			_unwaikableElapsedTime = 0f;
			while (true)
			{
				if (IsMoving)
				{
					_unwaikableElapsedTime = 0f;
				}
				else
				{
					_unwaikableElapsedTime += Time.deltaTime;
					if (_unwaikableElapsedTime >= 1f)
					{
						_unwaikableElapsedTime = 0f;
						To(PetState.teleport);
						StopCoroutine("CheckIsMoving");
					}
				}
				yield return null;
			}
		}

		public void CheckIsMovingStart()
		{
			StopCoroutine("CheckIsMoving");
			StartCoroutine("CheckIsMoving");
		}

		public void CheckIsMovingStop()
		{
			StopCoroutine("CheckIsMoving");
		}

		public override void SetCollidersEnabled(bool enabled)
		{
			base.SetCollidersEnabled(enabled);
			if (_characterControllerCollider.enabled != enabled)
			{
				_characterControllerCollider.enabled = enabled;
			}
		}
	}
}
