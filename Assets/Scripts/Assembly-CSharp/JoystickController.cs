using UnityEngine;

public sealed class JoystickController : MonoBehaviour
{
	public static UIJoystick leftJoystick;

	public static TouchPadController rightJoystick;

	public static TouchPadInJoystick leftTouchPad;

	public UIJoystick _leftJoystick;

	public TouchPadController _rightJoystick;

	public TouchPadInJoystick _leftTouchPad;

	private void Awake()
	{
		leftJoystick = _leftJoystick;
		rightJoystick = _rightJoystick;
		leftTouchPad = _leftTouchPad;
	}

	private void OnDestroy()
	{
		leftJoystick = null;
		rightJoystick = null;
		leftTouchPad = null;
	}

	public static bool IsButtonFireUp()
	{
		if (!leftTouchPad.isShooting)
		{
			return !rightJoystick.isShooting;
		}
		return false;
	}

	private void Update()
	{
		leftJoystick.value = GlobalControls.MovementVector;
		if (GlobalControls.DoMobile) return;
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.mySkinName != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(GlobalControls.LookDelta);
		}
		if (GlobalControls.MouseLocked)
		{
			rightJoystick.isShooting = Input.GetKey(KeyCode.Mouse0);
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.currentWeaponSounds != null)
			{
				if (Input.GetKeyDown(Controls.Aim) && !WeaponManager.sharedManager.myPlayerMoveC.isZooming && WeaponManager.sharedManager.currentWeaponSounds.isZooming)
				{
					WeaponManager.sharedManager.myPlayerMoveC.ZoomPress();
				}
				if (Input.GetKeyUp(Controls.Aim) && WeaponManager.sharedManager.myPlayerMoveC.isZooming)
				{
					WeaponManager.sharedManager.myPlayerMoveC.ZoomPress();
				}
			}

			if (Input.GetKeyDown(Controls.Reload) && !WeaponManager.sharedManager.myPlayerMoveC.isReloading)
			{
				WeaponManager.sharedManager.myPlayerMoveC.ReloadPressed();
			}
		}
	}
}
