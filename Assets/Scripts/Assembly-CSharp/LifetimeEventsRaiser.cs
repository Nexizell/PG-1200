using System;
using UnityEngine;

public class LifetimeEventsRaiser : MonoBehaviour
{
	private static LifetimeEventsRaiser s_instance;

	public static LifetimeEventsRaiser Instance
	{
		get
		{
			if (s_instance == null)
			{
				try
				{
					GameObject obj = new GameObject("LifetimeEventsRaiser");
					s_instance = obj.AddComponent<LifetimeEventsRaiser>();
					UnityEngine.Object.DontDestroyOnLoad(obj);
				}
				catch (Exception ex)
				{
					Debug.LogError("[Rilisoft] LifetimeEventsRaiser: Instance exception: " + ex);
				}
			}
			return s_instance;
		}
	}

	public event Action<bool> ApplicationPause;

	private void OnApplicationPause(bool pause)
	{
		if (this.ApplicationPause != null)
		{
			this.ApplicationPause(pause);
		}
	}
}
