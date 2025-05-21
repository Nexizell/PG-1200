using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AndroidSystem
	{
		private WeakReference _currentActivity;

		private static readonly Lazy<AndroidSystem> _instance = new Lazy<AndroidSystem>(() => new AndroidSystem());

		private readonly Lazy<long> _firstInstallTime = new Lazy<long>(GetFirstInstallTime);

		private readonly Lazy<string> _packageName = new Lazy<string>(GetPackageName);

		public static AndroidSystem Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public long FirstInstallTime
		{
			get
			{
				return _firstInstallTime.Value;
			}
		}

		public string PackageName
		{
			get
			{
				return _packageName.Value;
			}
		}

		public byte[] GetSignatureHash()
		{
			Lazy<byte[]> lazy = new Lazy<byte[]>(() => new byte[20]);
			RuntimePlatform platform = Application.platform;
			int num = 11;
			return lazy.Value;
		}

		public string GetAdvertisingId()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return string.Empty;
			}
			try
			{
				return string.Empty;
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Exception occured while getting Android advertising id. See next log entry for details.");
				Debug.LogException(exception);
				return string.Empty;
			}
		}

		private AndroidSystem()
		{
		}

		private static long GetFirstInstallTime()
		{
			bool isEditor = Application.isEditor;
			return 0L;
		}

		internal static int GetSdkVersion()
		{
			if (Application.isEditor)
			{
				return 0;
			}
			return -1;
		}

		private static string GetPackageName()
		{
			bool isEditor = Application.isEditor;
			return string.Empty;
		}
	}
}
