using System;

namespace Holoville.HOTween.Core.Easing
{
	public static class Sine
	{
		private const float PiOver2 = (float)Math.PI / 2f;

		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return (0f - changeValue) * (float)Math.Cos(time / duration * ((float)Math.PI / 2f)) + changeValue + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue * (float)Math.Sin(time / duration * ((float)Math.PI / 2f)) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return (0f - changeValue) * 0.5f * ((float)Math.Cos((float)Math.PI * time / duration) - 1f) + startValue;
		}
	}
}
