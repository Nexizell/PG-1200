namespace Holoville.HOTween.Core.Easing
{
	public static class Bounce
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue - EaseOut(duration - time, 0f, changeValue, duration, -1f, -1f) + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration) < 0.36363637f)
			{
				return changeValue * (7.5625f * time * time) + startValue;
			}
			if (time < 0.72727275f)
			{
				return changeValue * (7.5625f * (time -= 0.54545456f) * time + 0.75f) + startValue;
			}
			if (time < 0.90909094f)
			{
				return changeValue * (7.5625f * (time -= 0.8181818f) * time + 0.9375f) + startValue;
			}
			return changeValue * (7.5625f * (time -= 21f / 22f) * time + 63f / 64f) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time < duration * 0.5f)
			{
				return EaseIn(time * 2f, 0f, changeValue, duration, -1f, -1f) * 0.5f + startValue;
			}
			return EaseOut(time * 2f - duration, 0f, changeValue, duration, -1f, -1f) * 0.5f + changeValue * 0.5f + startValue;
		}
	}
}
