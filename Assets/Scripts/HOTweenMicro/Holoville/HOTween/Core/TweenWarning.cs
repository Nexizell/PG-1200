using UnityEngine;

namespace Holoville.HOTween.Core
{
	internal static class TweenWarning
	{
		internal static void Log(string p_message)
		{
			Log(p_message, false);
		}

		internal static void Log(string p_message, bool p_verbose)
		{
			if (HOTween.warningLevel != 0 && (!p_verbose || HOTween.warningLevel == WarningLevel.Verbose))
			{
				Debug.LogWarning("HOTween : " + p_message);
			}
		}
	}
}
