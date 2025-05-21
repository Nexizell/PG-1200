using System;

namespace Holoville.HOTween.Core.Easing
{
	public static class Expo
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time == 0f)
			{
				return startValue;
			}
			return changeValue * (float)Math.Pow(2.0, 10f * (time / duration - 1f)) + startValue - changeValue * 0.001f;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time == duration)
			{
				return startValue + changeValue;
			}
			return changeValue * (0f - (float)Math.Pow(2.0, -10f * time / duration) + 1f) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time == 0f)
			{
				return startValue;
			}
			if (time == duration)
			{
				return startValue + changeValue;
			}
			if ((time /= duration * 0.5f) < 1f)
			{
				return changeValue * 0.5f * (float)Math.Pow(2.0, 10f * (time - 1f)) + startValue;
			}
			return changeValue * 0.5f * (0f - (float)Math.Pow(2.0, -10f * (time -= 1f)) + 2f) + startValue;
		}
	}
}
