using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	internal class CryptoPlayerPrefsFacade
	{
		private static EncryptedPlayerPrefs _encryptedPlayerPrefs;

		internal static EncryptedPlayerPrefs EncryptedPlayerPrefs
		{
			get
			{
				if (_encryptedPlayerPrefs == null)
				{
					HiddenSettings hiddenSettings = ((MiscAppsMenu.Instance != null) ? MiscAppsMenu.Instance.misc : null);
					byte[] array = null;
					if (hiddenSettings == null)
					{
						Debug.LogError("Settings are null.");
						array = new byte[40];
					}
					else
					{
						try
						{
							array = Convert.FromBase64String(hiddenSettings.PersistentCacheManagerKey);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception);
							array = new byte[40];
						}
					}
					_encryptedPlayerPrefs = new EncryptedPlayerPrefs(array);
				}
				return _encryptedPlayerPrefs;
			}
		}

		public static void DeleteKey(string key)
		{
			CryptoPlayerPrefs.DeleteKey(key);
			EncryptedPlayerPrefs.DeleteKey(key);
		}

		public static int GetInt(string key)
		{
			if (EncryptedPlayerPrefs.HasKey(key))
			{
				int result;
				if (int.TryParse(EncryptedPlayerPrefs.GetString(key), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
				{
					return result;
				}
				return 0;
			}
			int @int = CryptoPlayerPrefs.GetInt(key);
			string value = @int.ToString(CultureInfo.InvariantCulture);
			EncryptedPlayerPrefs.SetString(key, value);
			return @int;
		}

		public static string GetString(string key)
		{
			if (EncryptedPlayerPrefs.HasKey(key))
			{
				return EncryptedPlayerPrefs.GetString(key);
			}
			string @string = CryptoPlayerPrefs.GetString(key);
			EncryptedPlayerPrefs.SetString(key, @string);
			return @string;
		}

		public static bool HasKey(string key)
		{
			if (EncryptedPlayerPrefs.HasKey(key))
			{
				return true;
			}
			return CryptoPlayerPrefs.HasKey(key);
		}

		public static void SetInt(string key, int val)
		{
			string value = val.ToString(CultureInfo.InvariantCulture);
			EncryptedPlayerPrefs.SetString(key, value);
		}

		public static void SetString(string key, string val)
		{
			EncryptedPlayerPrefs.SetString(key, val);
		}

		public static void Save()
		{
			PlayerPrefs.Save();
		}
	}
}
