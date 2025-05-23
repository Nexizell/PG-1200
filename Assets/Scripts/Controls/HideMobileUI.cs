using UnityEngine;

public class HideMobileUI : MonoBehaviour
{

	public UIPanel joystickPanel;

	private void Start()
	{
		if (!GlobalControls.DoMobile)
		{
			joystickPanel.alpha = 0;
		}
	}
}
