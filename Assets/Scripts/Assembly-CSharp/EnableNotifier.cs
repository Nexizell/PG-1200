using System.Collections.Generic;
using UnityEngine;

public class EnableNotifier : MonoBehaviour
{
	public List<EventDelegate> onEnable = new List<EventDelegate>();

	public bool isSoundFX;

	public bool lateUpdate;

	private bool m_update;

	private void OnEnable()
	{
		if (!isSoundFX)
		{
			if (!lateUpdate)
			{
				EventDelegate.Execute(onEnable);
			}
			else
			{
				m_update = true;
			}
		}
		else if (Defs.isSoundFX)
		{
			EventDelegate.Execute(onEnable);
		}
	}

	private void LateUpdate()
	{
		if (m_update)
		{
			EventDelegate.Execute(onEnable);
			m_update = false;
		}
	}
}
