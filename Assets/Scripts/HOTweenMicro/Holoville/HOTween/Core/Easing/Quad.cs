namespace Holoville.HOTween.Core.Easing
{
	public static class Quad
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue * (time /= duration) * time + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return (0f - changeValue) * (time /= duration) * (time - 2f) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return changeValue * 0.5f * time * time + startValue;
			}
			return (0f - changeValue) * 0.5f * ((time -= 1f) * (time - 2f) - 1f) + startValue;
		}
	}
}
