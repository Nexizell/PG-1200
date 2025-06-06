using UnityEngine;

namespace HeurekaGames
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		private static object _lock = new object();

		private static bool applicationIsQuitting = false;

		public static T Instance
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning(string.Concat("[Singleton] Instance '", typeof(T), "' already destroyed on application quit. Won't create again - returning null."));
					return null;
				}
				lock (_lock)
				{
					if ((Object)_instance == (Object)null)
					{
						_instance = (T)Object.FindObjectOfType(typeof(T));
						if (Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
							return _instance;
						}
						if ((Object)_instance == (Object)null)
						{
							GameObject obj = new GameObject();
							_instance = obj.AddComponent<T>();
							obj.name = "(singleton) " + typeof(T).ToString();
							Debug.Log(string.Concat("[Singleton] An instance of ", typeof(T), " was created."));
						}
						else
						{
							Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
						}
					}
					return _instance;
				}
			}
		}

		public void OnDestroy()
		{
			applicationIsQuitting = true;
		}
	}
}
