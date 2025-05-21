using UnityEngine;

namespace Holoville.HOTween.Core.Easing
{
	internal class EaseCurve
	{
		private AnimationCurve animCurve;

		public EaseCurve(AnimationCurve p_animCurve)
		{
			animCurve = p_animCurve;
		}

		public float Evaluate(float time, float startValue, float changeValue, float duration, float unusedOvershoot, float unusedPeriod)
		{
			float time2 = animCurve[animCurve.length - 1].time;
			float num = time / duration;
			float num2 = animCurve.Evaluate(num * time2);
			return changeValue * num2 + startValue;
		}
	}
}
