namespace Holoville.HOTween.Core.Easing
{
	public static class Linear
	{
		public static float EaseNone(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return changeValue * time / duration + startValue;
		}
	}
}
