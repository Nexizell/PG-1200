using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public static class Storager
{
	private static readonly Dictionary<string, Action> _onValueChanged = new Dictionary<string, Action>();

	private static readonly Dictionary<string, SaltedInt> _keychainCache = new Dictionary<string, SaltedInt>();

	private static readonly Dictionary<string, string> _keychainStringCache = new Dictionary<string, string>();

	private const bool useCryptoPlayerPrefs = false;

	private static bool _weaponDigestIsDirty;

	private static HashSet<string> m_keysInKeychainIos = new HashSet<string>();

	private static readonly IDictionary<string, SaltedInt> _protectedIntCache = new Dictionary<string, SaltedInt>();

	private static readonly System.Random _prng = new System.Random();

	private static readonly string[] _expendableKeys = new string[4]
	{
		GearManager.InvisibilityPotion,
		GearManager.Jetpack,
		GearManager.Turret,
		GearManager.Mech
	};

	public static void SubscribeToChanged(string key, Action act)
	{
		if (!key.IsNullOrEmpty())
		{
			if (_onValueChanged.ContainsKey(key))
			{
				Dictionary<string, Action> onValueChanged = _onValueChanged;
				onValueChanged[key] = (Action)Delegate.Combine(onValueChanged[key], act);
			}
			else
			{
				_onValueChanged.Add(key, act);
			}
		}
	}

	public static void UnSubscribeToChanged(string key, Action act)
	{
		if (key.IsNullOrEmpty() || !_onValueChanged.ContainsKey(key))
		{
			return;
		}
		Action action = _onValueChanged[key];
		Delegate[] array = action.GetInvocationList().ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if ((object)array[i] == act)
			{
				action = (Action)Delegate.Remove(action, act);
			}
		}
	}

	private static void InvokeSubscribers(string key)
	{
		if (Application.isPlaying && _onValueChanged.ContainsKey(key))
		{
			Action action = _onValueChanged[key];
			if (action != null)
			{
				action();
			}
		}
	}

	public static bool hasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void setInt(string key, int val)
	{
		if (Tools.IsEditor)
		{
			PlayerPrefs.SetInt(key, val);
		}
		else
		{
			PlayerPrefs.SetInt(key, val);
			_protectedIntCache[key] = new SaltedInt(_prng.Next(), val);
		}
		if (key.Equals("Coins") || key.Equals("GemsCurrency"))
		{
			DigestStorager.Instance.Set(key, val);
		}
		if (_expendableKeys.Contains(key))
		{
			RefreshExpendablesDigest();
		}
		if (WeaponManager.PurchasableWeaponSetContains(key))
		{
			_weaponDigestIsDirty = true;
		}
		InvokeSubscribers(key);
	}

	public static int getInt(string key)
	{
		if (Tools.IsEditor)
		{
			return PlayerPrefs.GetInt(key);
		}
		SaltedInt value;
		if (_protectedIntCache.TryGetValue(key, out value))
		{
			return value.Value;
		}
		if (PlayerPrefs.HasKey(key))
		{
			int @int = PlayerPrefs.GetInt(key);
			_protectedIntCache.Add(key, new SaltedInt(_prng.Next(), @int));
			return @int;
		}
		return 0;
	}

	public static void setString(string key, string val)
	{
		_keychainStringCache[key] = val;
		if (Tools.IsEditor)
		{
			PlayerPrefs.SetString(key, val);
			InvokeSubscribers(key);
		}
		else
		{
			PlayerPrefs.SetString(key, val);
			InvokeSubscribers(key);
		}
	}

	public static string getString(string key)
	{
		string value;
		if (_keychainStringCache.TryGetValue(key, out value))
		{
			return value;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			return PlayerPrefs.GetString(key);
		}
		if (PlayerPrefs.HasKey(key))
		{
			string @string = PlayerPrefs.GetString(key);
			_keychainStringCache.Add(key, @string);
			return @string;
		}
		return string.Empty;
	}

	public static bool IsInitialized(string flagName)
	{
		if (Tools.IsEditor)
		{
			return PlayerPrefs.HasKey(flagName);
		}
		return hasKey(flagName);
	}

	public static void SetInitialized(string flagName)
	{
		setInt(flagName, 0);
	}

	private static void RefreshExpendablesDigest()
	{
		byte[] value = _expendableKeys.SelectMany((string key) => BitConverter.GetBytes(getInt(key))).ToArray();
		DigestStorager.Instance.Set("ExpendablesCount", value);
	}

	public static void RefreshWeaponDigestIfDirty()
	{
		if (_weaponDigestIsDirty)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] > RefreshWeaponsDigest: {0:F3}", Time.realtimeSinceStartup);
			}
			RefreshWeaponsDigest();
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] < RefreshWeaponsDigest: {0:F3}", Time.realtimeSinceStartup);
			}
		}
	}

	private static void RefreshWeaponsDigest()
	{
		int value = WeaponManager.storeIDtoDefsSNMapping.Values.Where((string w) => getInt(w) == 1).Count();
		DigestStorager.Instance.Set("WeaponsCount", value);
		_weaponDigestIsDirty = false;
	}
}
