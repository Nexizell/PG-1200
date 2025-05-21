using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

public class TouchPadInJoystick : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CReCalcRects_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TouchPadInJoystick _003C_003E4__this;

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
		public _003CReCalcRects_003Ed__12(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.CalcRects();
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
	internal sealed class _003C_SetIsFirstFrame_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003Ctm_003E5__1;

		public TouchPadInJoystick _003C_003E4__this;

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
		public _003C_SetIsFirstFrame_003Ed__15(int _003C_003E1__state)
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
				if (!(Time.realtimeSinceStartup - _003Ctm_003E5__1 < 0.1f))
				{
					_003C_003E4__this._isFirstFrame = false;
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003Ctm_003E5__1 = Time.realtimeSinceStartup;
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

	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TouchPadInJoystick _003C_003E4__this;

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
		public _003CStart_003Ed__16(int _003C_003E1__state)
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
				PauseNGUIController.PlayerHandUpdated += _003C_003E4__this.SetSideAndCalcRects;
				ControlsSettingsBase.ControlsChanged += _003C_003E4__this.SetShouldRecalcRects;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.CalcRects();
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

	public Transform fireSprite;

	public Transform jumpSprite;

	public bool isShooting;

	public bool isJumpPressed;

	public InGameGUI inGameGUI;

	public bool isActiveFireButton;

	public GameObject joystickSprite;

	private Rect _fireRect;

	private bool _shouldRecalcRects;

	private bool _isFirstFrame = true;

	private bool _joyActive = true;

	private Player_move_c _playerMoveC;

	private bool pressured;

	private IEnumerator ReCalcRects()
	{
		yield return null;
		yield return null;
		CalcRects();
	}

	public void SetJoystickActive(bool active)
	{
		_joyActive = active;
		if (!active)
		{
			isShooting = false;
			isJumpPressed = false;
		}
	}

	private void OnEnable()
	{
		isShooting = false;
		if (_shouldRecalcRects)
		{
			StartCoroutine(ReCalcRects());
		}
		_shouldRecalcRects = false;
		StartCoroutine(_SetIsFirstFrame());
	}

	private IEnumerator _SetIsFirstFrame()
	{
		float tm = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - tm < 0.1f);
		_isFirstFrame = false;
	}

	private IEnumerator Start()
	{
		PauseNGUIController.PlayerHandUpdated += SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged += SetShouldRecalcRects;
		yield return null;
		yield return null;
		CalcRects();
	}

	private void SetSideAndCalcRects()
	{
		SetShouldRecalcRects();
	}

	private void SetShouldRecalcRects()
	{
		_shouldRecalcRects = true;
	}

	private bool IsActiveFireButton()
	{
		if ((!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None) || Defs.isTurretWeapon)
		{
			return false;
		}
		if (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.On)
		{
			return true;
		}
		if (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Sniper && _playerMoveC != null && _playerMoveC.isZooming)
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (_playerMoveC == null)
		{
			if (Defs.isMulti && WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayer != null)
			{
				_playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			}
			else
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					_playerMoveC = gameObject.GetComponent<SkinName>().playerMoveC;
				}
			}
		}
		if (!_joyActive)
		{
			isShooting = false;
			isJumpPressed = false;
			return;
		}
		isActiveFireButton = IsActiveFireButton();
		bool flag = isActiveFireButton && GameConnect.gameMode != GameConnect.GameMode.DeathEscape && GameConnect.gameMode != GameConnect.GameMode.SpeedRun && (!GameConnect.isMiniGame || MiniGamesController.Instance.isGo);
		if (flag != fireSprite.gameObject.activeSelf)
		{
			fireSprite.gameObject.SetActive(flag);
		}
		bool isSpeedrun = GameConnect.isSpeedrun;
		if (isSpeedrun != jumpSprite.gameObject.activeSelf)
		{
			jumpSprite.gameObject.SetActive(isSpeedrun);
			if (isSpeedrun)
			{
				jumpSprite.localPosition = new Vector3(0f - JoystickController.rightJoystick.jumpSprite.localPosition.x, JoystickController.rightJoystick.jumpSprite.localPosition.y, 0f);
			}
		}
	}

	private void OnPressure(float pressure)
	{
		if (GameConnect.isDeathEscape || GameConnect.isSpeedrun)
		{
			return;
		}
		if (Defs.touchPressureSupported && Defs.isUseJump3DTouch && pressure > Defs.touchPressurePower)
		{
			if (!pressured)
			{
				pressured = true;
				isJumpPressed = true;
				if (TrainingController.sharedController != null)
				{
					TrainingController.sharedController.Hide3dTouchJump();
				}
			}
		}
		else
		{
			pressured = false;
			isJumpPressed = false;
		}
	}

	private void OnPress(bool isDown)
	{
		if (GameConnect.isSpeedrun)
		{
			isJumpPressed = isDown;
		}
		else
		{
			if (!_joyActive || inGameGUI.playerMoveC == null)
			{
				return;
			}
			if (_fireRect.width.Equals(0f))
			{
				CalcRects();
			}
			if (!_isFirstFrame)
			{
				if (GameConnect.gameMode != GameConnect.GameMode.DeathEscape && isDown && _fireRect.Contains(UICamera.lastTouchPosition) && fireSprite.gameObject.activeSelf)
				{
					isShooting = true;
				}
				if (!isDown)
				{
					isShooting = false;
					pressured = false;
					isJumpPressed = false;
				}
			}
		}
	}

	private void CalcRects()
	{
		Transform relativeTo = NGUITools.GetRoot(base.gameObject).transform.GetChild(0).GetChild(0).GetComponent<Camera>()
			.transform;
		float num = 768f;
		float num2 = num * ((float)Screen.width / (float)Screen.height);
		List<object> list = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]")) as List<object>;
		if (list == null)
		{
			list = new List<object>();
			UnityEngine.Debug.LogWarning(list.GetType().FullName);
		}
		int[] array = list.Select(Convert.ToInt32).ToArray();
		if (array.Length == 0)
		{
			array = Defs.controlButtonSizes;
		}
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(relativeTo, fireSprite, true);
		float num3 = 60f;
		if (array.Length > 6)
		{
			num3 = (float)array[6] * 0.5f;
		}
		bounds.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		_fireRect = new Rect((bounds.center.x - num3) * Defs.Coef, (bounds.center.y - num3) * Defs.Coef, 2f * num3 * Defs.Coef, 2f * num3 * Defs.Coef);
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged -= SetShouldRecalcRects;
	}
}
