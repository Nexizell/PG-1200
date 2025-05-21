using UnityEngine;

namespace DevToDev.Core.Utils
{
	internal class UnityPlayerPlatform
	{
		public static bool isUnityWSAPlatform()
		{
			if (Application.platform == RuntimePlatform.MetroPlayerARM)
			{
				return true;
			}
			if (Application.platform == RuntimePlatform.MetroPlayerX64)
			{
				return true;
			}
			if (Application.platform == RuntimePlatform.MetroPlayerX86)
			{
				return true;
			}
			return false;
		}

		public static bool isUnityWebPlatform()
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				return true;
			}
			return false;
		}

		public static bool isWebGLPlayer()
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				return true;
			}
			return false;
		}
	}
}
