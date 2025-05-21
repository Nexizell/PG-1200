namespace Holoville.HOTween.Core.Easing
{
	public static class Quart
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue * (time /= duration) * time * time * time + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return (0f - changeValue) * ((time = time / duration - 1f) * time * time * time - 1f) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return changeValue * 0.5f * time * time * time * time + startValue;
			}
			return (0f - changeValue) * 0.5f * ((time -= 2f) * time * time * time - 2f) + startValue;
		}
	}
}
