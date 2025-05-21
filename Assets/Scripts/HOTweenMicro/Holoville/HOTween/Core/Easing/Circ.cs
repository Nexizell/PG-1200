using System;

namespace Holoville.HOTween.Core.Easing
{
	public static class Circ
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return (0f - changeValue) * ((float)Math.Sqrt(1f - (time /= duration) * time) - 1f) + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue * (float)Math.Sqrt(1f - (time = time / duration - 1f) * time) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return (0f - changeValue) * 0.5f * ((float)Math.Sqrt(1f - time * time) - 1f) + startValue;
			}
			return changeValue * 0.5f * ((float)Math.Sqrt(1f - (time -= 2f) * time) + 1f) + startValue;
		}
	}
}
