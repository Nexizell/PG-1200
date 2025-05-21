using System;
using System.Reflection;
using UnityEngine;

public class NumberVersionLabel : MonoBehaviour
{
	private Version CurrentVersion
	{
		get
		{
			return new Version(12, 0, 0);
		}
	}

	private void Start()
	{
		UILabel component = GetComponent<UILabel>();
		if (component != null)
		{
			component.text = CurrentVersion.ToString();
		}
	}
}
