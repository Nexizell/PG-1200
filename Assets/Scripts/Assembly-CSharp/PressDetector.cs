using System;
using UnityEngine;

public sealed class PressDetector : MonoBehaviour
{
	public static EventHandler<EventArgs> PressedDown;

	private void OnPress(bool isDown)
	{
		EventHandler<EventArgs> pressedDown = PressedDown;
		if (pressedDown != null)
		{
			pressedDown(base.gameObject, EventArgs.Empty);
		}
	}
}
