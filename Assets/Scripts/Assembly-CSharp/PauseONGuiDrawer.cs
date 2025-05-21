using System;
using UnityEngine;

public sealed class PauseONGuiDrawer : MonoBehaviour
{
	public Action act;

	private void OnGUI()
	{
		if (act != null)
		{
			act();
		}
	}
}
