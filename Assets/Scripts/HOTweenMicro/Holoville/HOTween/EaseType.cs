using System;

namespace Holoville.HOTween
{
	public enum EaseType
	{
		Linear = 0,
		EaseInSine = 1,
		EaseOutSine = 2,
		EaseInOutSine = 3,
		EaseInQuad = 4,
		EaseOutQuad = 5,
		EaseInOutQuad = 6,
		EaseInCubic = 7,
		EaseOutCubic = 8,
		EaseInOutCubic = 9,
		EaseInQuart = 10,
		EaseOutQuart = 11,
		EaseInOutQuart = 12,
		EaseInQuint = 13,
		EaseOutQuint = 14,
		EaseInOutQuint = 15,
		EaseInExpo = 16,
		EaseOutExpo = 17,
		EaseInOutExpo = 18,
		EaseInCirc = 19,
		EaseOutCirc = 20,
		EaseInOutCirc = 21,
		EaseInElastic = 22,
		EaseOutElastic = 23,
		EaseInOutElastic = 24,
		EaseInBack = 25,
		EaseOutBack = 26,
		EaseInOutBack = 27,
		EaseInBounce = 28,
		EaseOutBounce = 29,
		EaseInOutBounce = 30,
		AnimationCurve = 31,
		[Obsolete("Use EaseInQuint instead.")]
		EaseInStrong = 32,
		[Obsolete("Use EaseOutQuint instead.")]
		EaseOutStrong = 33,
		[Obsolete("Use EaseInOutQuint instead.")]
		EaseInOutStrong = 34
	}
}
