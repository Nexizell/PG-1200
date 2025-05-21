using UnityEngine;

public class TestControllThirdCameraSettings : MonoBehaviour
{
	public UISlider slider;

	public UILabel label;

	public int sliderInd;

	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}

	public void OnChangeValueSlider()
	{
		if (ThirdPersonCamera.instance != null && GameConnect.isDeathEscape)
		{
			ThirdPersonCamera.instance.speedRotateXOnGo = Mathf.RoundToInt(slider.value * 200f);
			label.text = ThirdPersonCamera.instance.speedRotateXOnGo.ToString();
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null && GameConnect.isSpeedrun)
		{
			switch (sliderInd)
			{
			case 0:
			{
				float playerAddSpeed = 6.5f + slider.value * 40f;
				SpeedrunTrackController.instance.startPlayerSpeed = playerAddSpeed;
				label.text = playerAddSpeed.ToString("F1");
				break;
			}
			case 1:
			{
				float playerAddSpeed = 1f + slider.value * 500f;
				SpeedrunTrackController.instance.distanceToAddSpeed = playerAddSpeed;
				label.text = playerAddSpeed.ToString("F1");
				break;
			}
			case 2:
			{
				float playerAddSpeed = slider.value * 2f;
				SpeedrunTrackController.instance.playerAddSpeed = playerAddSpeed;
				label.text = playerAddSpeed.ToString("F1");
				break;
			}
			}
		}
	}
}
