using UnityEngine;

public class TestControlSpeedrun : MonoBehaviour
{
	private SettingsToggleButtons toggle;

	private bool lastCheck;

	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (lastCheck != toggle.IsChecked)
		{
			lastCheck = toggle.IsChecked;
			SpeedrunTrackController.instance.ChangeControlScheme(toggle.IsChecked);
		}
	}
}
