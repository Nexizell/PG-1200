using System;
using UnityEngine;

public class NoodlePermissionGranter : MonoBehaviour
{
	public enum NoodleAndroidPermission
	{
		WRITE_EXTERNAL_STORAGE = 0,
		ACCESS_COARSE_LOCATION = 1
	}

	public static Action<bool> PermissionRequestCallback;

	public static EventHandler<EventArgs> PermissionRequestFinished;

	private static NoodlePermissionGranter instance;

	private static bool initialized;

	private static AndroidJavaClass noodlePermissionGranterClass;

	private static AndroidJavaObject activity;

	private const string WRITE_EXTERNAL_STORAGE = "WRITE_EXTERNAL_STORAGE";

	private const string PERMISSION_GRANTED = "PERMISSION_GRANTED";

	private const string PERMISSION_DENIED = "PERMISSION_DENIED";

	private const string NOODLE_PERMISSION_GRANTER = "NoodlePermissionGranter";

	public static void GrantPermission(NoodleAndroidPermission permission)
	{
		if (!initialized)
		{
			initialize();
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			noodlePermissionGranterClass.CallStatic("grantPermission", activity, (int)permission);
		}
	}

	public void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (base.name != "NoodlePermissionGranter")
		{
			base.name = "NoodlePermissionGranter";
		}
	}

	private static void initialize()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (instance == null)
			{
				GameObject obj = new GameObject();
				instance = obj.AddComponent<NoodlePermissionGranter>();
				obj.name = "NoodlePermissionGranter";
			}
			noodlePermissionGranterClass = new AndroidJavaClass("com.noodlecake.unityplugins.NoodlePermissionGranter");
			activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			initialized = true;
		}
	}

	private void permissionRequestCallbackInternal(string message)
	{
		bool obj = message == "PERMISSION_GRANTED";
		if (PermissionRequestCallback != null)
		{
			PermissionRequestCallback(obj);
		}
	}
}
