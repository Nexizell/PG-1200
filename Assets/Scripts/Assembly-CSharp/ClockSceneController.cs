using System;
using UnityEngine;

public class ClockSceneController : MonoBehaviour
{
	public enum TypeHand
	{
		minutes = 0,
		hour = 1
	}

	public TypeHand type;

	private Transform thisTransform;

	public DisableObjectFromTimer bats;

	private int oldValue = -1000;

	private void Start()
	{
		thisTransform = base.transform;
		UpdateAngle();
	}

	private void Update()
	{
		UpdateAngle();
	}

	private void UpdateAngle()
	{
		DateTime now = DateTime.Now;
		int num = ((type == TypeHand.minutes) ? now.Minute : (now.Hour * 60 + now.Minute));
		if (num != oldValue)
		{
			if (bats != null && num < oldValue && num == 0)
			{
				bats.timer = 10f;
				bats.gameObject.SetActive(true);
			}
			oldValue = num;
			if (type == TypeHand.hour && num >= 720)
			{
				num -= 720;
			}
			float y = 360f * (float)num / ((type == TypeHand.minutes) ? 60f : 720f);
			thisTransform.localRotation = Quaternion.Euler(new Vector3(0f, y, 0f));
		}
	}
}
