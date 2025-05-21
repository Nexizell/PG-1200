using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CameraTouchControlScheme_LowPassFilter : CameraTouchControlScheme
{
	[CompilerGenerated]
	internal sealed class _003CCancelLimitDrag_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CameraTouchControlScheme_LowPassFilter _003C_003E4__this;

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
		public _003CCancelLimitDrag_003Ed__9(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.dragClampInterval);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.limitDrag = false;
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

	public float dragClampInterval = 1.5f;

	public float dragClamp = 1f;

	public float lerpCoeff = 0.25f;

	private Vector2? _accumulatedDrag;

	private Vector2? _unfilteredAccumulatedDrag;

	private bool limitDrag;

	private bool firstDrag;

	private Vector2? _azimuthTilt;

	public override void OnPress(bool isDown)
	{
		if (isDown)
		{
			_accumulatedDrag = Vector2.zero;
			_unfilteredAccumulatedDrag = Vector2.zero;
		}
		else
		{
			_accumulatedDrag = null;
			_unfilteredAccumulatedDrag = null;
		}
		firstDrag = isDown;
		limitDrag = isDown;
		if (isDown)
		{
			if ((bool)JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StartCoroutine(CancelLimitDrag());
			}
		}
		else if ((bool)JoystickController.rightJoystick)
		{
			JoystickController.rightJoystick.StopCoroutine(CancelLimitDrag());
		}
	}

	private IEnumerator CancelLimitDrag()
	{
		yield return new WaitForSeconds(dragClampInterval);
		limitDrag = false;
	}

	public override void OnDrag(Vector2 delta)
	{
		if (!firstDrag)
		{
			_deltaPosition = delta;
		}
		firstDrag = false;
		if (limitDrag)
		{
			limitDrag = false;
			if ((bool)JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StopCoroutine(CancelLimitDrag());
			}
			_deltaPosition = Vector2.ClampMagnitude(delta, dragClamp);
		}
		if (_accumulatedDrag.HasValue && _unfilteredAccumulatedDrag.HasValue)
		{
			Vector2 deltaPosition = _deltaPosition;
			Vector2 vector = _unfilteredAccumulatedDrag.Value + deltaPosition;
			Vector2 value = Vector2.Lerp(_accumulatedDrag.Value, vector, lerpCoeff);
			_accumulatedDrag = value;
			_unfilteredAccumulatedDrag = vector;
		}
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(_deltaPosition);
		Reset();
	}

	public override void Reset()
	{
		_deltaPosition = Vector2.zero;
		_accumulatedDrag = null;
		_unfilteredAccumulatedDrag = null;
	}

	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (_accumulatedDrag.HasValue)
		{
			if (_azimuthTilt.HasValue)
			{
				Vector2 value = _accumulatedDrag.Value;
				float num = sensitivity / 30f;
				yawTransform.rotation = Quaternion.Euler(0f, _azimuthTilt.Value.x + value.x * num, 0f);
				float num2 = _azimuthTilt.Value.y;
				if (num2 > 180f)
				{
					num2 -= 360f;
				}
				float num3 = num2 + value.y * (invert ? 1f : (-1f)) * num;
				if (num3 > 80f)
				{
					num3 = 80f;
				}
				if (num3 < -65f)
				{
					num3 = -65f;
				}
				pitchTransform.localRotation = Quaternion.Euler(num3, 0f, 0f);
			}
			else
			{
				_azimuthTilt = new Vector2(yawTransform.rotation.eulerAngles.y, pitchTransform.localEulerAngles.x);
			}
		}
		else
		{
			_azimuthTilt = null;
		}
	}
}
