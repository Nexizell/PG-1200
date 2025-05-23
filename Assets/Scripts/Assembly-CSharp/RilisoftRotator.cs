using System;
using Rilisoft;
using UnityEngine;

public class RilisoftRotator : MonoBehaviour
{
	public float rate = 10f;

	private Transform _transform;

	public static float RotationRateForCharacterInMenues
	{
		get
		{
			return -120f * ((BuildSettings.BuildTargetPlatform == RuntimePlatform.Android) ? 2f : 0.5f);
		}
	}

	public static void RotateCharacter(Transform character, float rotationRate, Rect touchZone, ref float idleTimerStartedTime, ref float lastTimeRotated, Func<bool> canProcess = null)
	{
		if (canProcess == null || canProcess())
		{
			if (MobileRelay.touchCount > 0)
			{
				Touch touch = MobileRelay.GetTouch(0);
				if (touch.phase == TouchPhase.Moved && touchZone.Contains(touch.position))
				{
					idleTimerStartedTime = Time.realtimeSinceStartup;
					character.Rotate(Vector3.up, touch.deltaPosition.x * rotationRate * 0.5f * (Time.realtimeSinceStartup - lastTimeRotated));
				}
			}
		}
		lastTimeRotated = Time.realtimeSinceStartup;
	}

	private void Start()
	{
		_transform = base.transform;
	}

	private void Update()
	{
		_transform.Rotate(Vector3.forward, rate * Time.deltaTime, Space.Self);
	}
}
