using System;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PersistentCache
	{
		private readonly string _persistentDataPath = string.Empty;

		private static readonly Lazy<PersistentCache> _instance = new Lazy<PersistentCache>(() => new PersistentCache());

		public string PersistentDataPath
		{
			get
			{
				return _persistentDataPath;
			}
		}

		public static PersistentCache Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public PersistentCache()
		{
			try
			{
				string persistentDataPath = Application.persistentDataPath;
				if (!string.IsNullOrEmpty(persistentDataPath))
				{
					if (!Directory.Exists(persistentDataPath))
					{
						Directory.CreateDirectory(persistentDataPath);
					}
					_persistentDataPath = persistentDataPath;
				}
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Caught exception while persistent data path initialization. See next error message for details.");
				Debug.LogException(exception);
			}
		}

		public string GetCachePathByUri(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(_persistentDataPath))
			{
				return string.Empty;
			}
			try
			{
				string text = string.Concat(new Uri(url).Segments).TrimStart('/');
				return Path.Combine(_persistentDataPath, text);
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
