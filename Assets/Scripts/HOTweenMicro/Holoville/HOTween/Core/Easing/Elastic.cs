using System;

namespace Holoville.HOTween.Core.Easing
{
	public static class Elastic
	{
		private const float TwoPi = (float)Math.PI * 2f;

		public static float EaseIn(float time, float startValue, float changeValue, float duration)
		{
			return EaseIn(time, startValue, changeValue, duration, 0f, 0f);
		}

		public static float EaseIn(float time, float startValue, float changeValue, float duration, float amplitude, float period)
		{
			if (time == 0f)
			{
				return startValue;
			}
			if ((time /= duration) == 1f)
			{
				return startValue + changeValue;
			}
			if (period == 0f)
			{
				period = duration * 0.3f;
			}
			float num;
			if (amplitude == 0f || (changeValue > 0f && amplitude < changeValue) || (changeValue < 0f && amplitude < 0f - changeValue))
			{
				amplitude = changeValue;
				num = period / 4f;
			}
			else
			{
				num = period / ((float)Math.PI * 2f) * (float)Math.Asin(changeValue / amplitude);
			}
			return 0f - amplitude * (float)Math.Pow(2.0, 10f * (time -= 1f)) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period) + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration)
		{
			return EaseOut(time, startValue, changeValue, duration, 0f, 0f);
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float amplitude, float period)
		{
			if (time == 0f)
			{
				return startValue;
			}
			if ((time /= duration) == 1f)
			{
				return startValue + changeValue;
			}
			if (period == 0f)
			{
				period = duration * 0.3f;
			}
			float num;
			if (amplitude == 0f || (changeValue > 0f && amplitude < changeValue) || (changeValue < 0f && amplitude < 0f - changeValue))
			{
				amplitude = changeValue;
				num = period / 4f;
			}
			else
			{
				num = period / ((float)Math.PI * 2f) * (float)Math.Asin(changeValue / amplitude);
			}
			return amplitude * (float)Math.Pow(2.0, -10f * time) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period) + changeValue + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration)
		{
			return EaseInOut(time, startValue, changeValue, duration, 0f, 0f);
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float amplitude, float period)
		{
			if (time == 0f)
			{
				return startValue;
			}
			if ((time /= duration * 0.5f) == 2f)
			{
				return startValue + changeValue;
			}
			if (period == 0f)
			{
				period = duration * 0.45000002f;
			}
			float num;
			if (amplitude == 0f || (changeValue > 0f && amplitude < changeValue) || (changeValue < 0f && amplitude < 0f - changeValue))
			{
				amplitude = changeValue;
				num = period / 4f;
			}
			else
			{
				num = period / ((float)Math.PI * 2f) * (float)Math.Asin(changeValue / amplitude);
			}
			if (time < 1f)
			{
				return -0.5f * (amplitude * (float)Math.Pow(2.0, 10f * (time -= 1f)) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period)) + startValue;
			}
			return amplitude * (float)Math.Pow(2.0, -10f * (time -= 1f)) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period) * 0.5f + changeValue + startValue;
		}
	}
}
