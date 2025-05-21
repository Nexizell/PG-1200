namespace Holoville.HOTween.Core.Easing
{
	public static class Back
	{
		public static float EaseIn(float time, float startValue, float changeValue, float duration, float overshoot, float unusedPeriod)
		{
			return changeValue * (time /= duration) * time * ((overshoot + 1f) * time - overshoot) + startValue;
		}

		public static float EaseOut(float time, float startValue, float changeValue, float duration, float overshoot, float unusedPeriod)
		{
			return changeValue * ((time = time / duration - 1f) * time * ((overshoot + 1f) * time + overshoot) + 1f) + startValue;
		}

		public static float EaseInOut(float time, float startValue, float changeValue, float duration, float overshoot, float unusedPeriod)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return changeValue * 0.5f * (time * time * (((overshoot *= 1.525f) + 1f) * time - overshoot)) + startValue;
			}
			return changeValue / 2f * ((time -= 2f) * time * (((overshoot *= 1.525f) + 1f) * time + overshoot) + 2f) + startValue;
		}
	}
}
