using Holoville.HOTween.Core.Easing;

namespace Holoville.HOTween.Core
{
	public class EaseInfo
	{
		public TweenDelegate.EaseFunc ease;

		public TweenDelegate.EaseFunc inverseEase;

		private static EaseInfo easeInSineInfo = new EaseInfo(Sine.EaseIn, Sine.EaseOut);

		private static EaseInfo easeOutSineInfo = new EaseInfo(Sine.EaseOut, Sine.EaseIn);

		private static EaseInfo easeInOutSineInfo = new EaseInfo(Sine.EaseInOut, null);

		private static EaseInfo easeInQuadInfo = new EaseInfo(Quad.EaseIn, Quad.EaseOut);

		private static EaseInfo easeOutQuadInfo = new EaseInfo(Quad.EaseOut, Quad.EaseIn);

		private static EaseInfo easeInOutQuadInfo = new EaseInfo(Quad.EaseInOut, null);

		private static EaseInfo easeInCubicInfo = new EaseInfo(Cubic.EaseIn, Cubic.EaseOut);

		private static EaseInfo easeOutCubicInfo = new EaseInfo(Cubic.EaseOut, Cubic.EaseIn);

		private static EaseInfo easeInOutCubicInfo = new EaseInfo(Cubic.EaseInOut, null);

		private static EaseInfo easeInQuartInfo = new EaseInfo(Quart.EaseIn, Quart.EaseOut);

		private static EaseInfo easeOutQuartInfo = new EaseInfo(Quart.EaseOut, Quart.EaseIn);

		private static EaseInfo easeInOutQuartInfo = new EaseInfo(Quart.EaseInOut, null);

		private static EaseInfo easeInQuintInfo = new EaseInfo(Quint.EaseIn, Quint.EaseOut);

		private static EaseInfo easeOutQuintInfo = new EaseInfo(Quint.EaseOut, Quint.EaseIn);

		private static EaseInfo easeInOutQuintInfo = new EaseInfo(Quint.EaseInOut, null);

		private static EaseInfo easeInExpoInfo = new EaseInfo(Expo.EaseIn, Expo.EaseOut);

		private static EaseInfo easeOutExpoInfo = new EaseInfo(Expo.EaseOut, Expo.EaseIn);

		private static EaseInfo easeInOutExpoInfo = new EaseInfo(Expo.EaseInOut, null);

		private static EaseInfo easeInCircInfo = new EaseInfo(Circ.EaseIn, Circ.EaseOut);

		private static EaseInfo easeOutCircInfo = new EaseInfo(Circ.EaseOut, Circ.EaseIn);

		private static EaseInfo easeInOutCircInfo = new EaseInfo(Circ.EaseInOut, null);

		private static EaseInfo easeInElasticInfo = new EaseInfo(Elastic.EaseIn, Elastic.EaseOut);

		private static EaseInfo easeOutElasticInfo = new EaseInfo(Elastic.EaseOut, Elastic.EaseIn);

		private static EaseInfo easeInOutElasticInfo = new EaseInfo(Elastic.EaseInOut, null);

		private static EaseInfo easeInBackInfo = new EaseInfo(Back.EaseIn, Back.EaseOut);

		private static EaseInfo easeOutBackInfo = new EaseInfo(Back.EaseOut, Back.EaseIn);

		private static EaseInfo easeInOutBackInfo = new EaseInfo(Back.EaseInOut, null);

		private static EaseInfo easeInBounceInfo = new EaseInfo(Bounce.EaseIn, Bounce.EaseOut);

		private static EaseInfo easeOutBounceInfo = new EaseInfo(Bounce.EaseOut, Bounce.EaseIn);

		private static EaseInfo easeInOutBounceInfo = new EaseInfo(Bounce.EaseInOut, null);

		private static EaseInfo easeInStrongInfo = new EaseInfo(Strong.EaseIn, Strong.EaseOut);

		private static EaseInfo easeOutStrongInfo = new EaseInfo(Strong.EaseOut, Strong.EaseIn);

		private static EaseInfo easeInOutStrongInfo = new EaseInfo(Strong.EaseInOut, null);

		private static EaseInfo defaultEaseInfo = new EaseInfo(Linear.EaseNone, null);

		private EaseInfo(TweenDelegate.EaseFunc p_ease, TweenDelegate.EaseFunc p_inverseEase)
		{
			ease = p_ease;
			inverseEase = p_inverseEase;
		}

		internal static EaseInfo GetEaseInfo(EaseType p_easeType)
		{
			switch (p_easeType)
			{
			case EaseType.EaseInSine:
				return easeInSineInfo;
			case EaseType.EaseOutSine:
				return easeOutSineInfo;
			case EaseType.EaseInOutSine:
				return easeInOutSineInfo;
			case EaseType.EaseInQuad:
				return easeInQuadInfo;
			case EaseType.EaseOutQuad:
				return easeOutQuadInfo;
			case EaseType.EaseInOutQuad:
				return easeInOutQuadInfo;
			case EaseType.EaseInCubic:
				return easeInCubicInfo;
			case EaseType.EaseOutCubic:
				return easeOutCubicInfo;
			case EaseType.EaseInOutCubic:
				return easeInOutCubicInfo;
			case EaseType.EaseInQuart:
				return easeInQuartInfo;
			case EaseType.EaseOutQuart:
				return easeOutQuartInfo;
			case EaseType.EaseInOutQuart:
				return easeInOutQuartInfo;
			case EaseType.EaseInQuint:
				return easeInQuintInfo;
			case EaseType.EaseOutQuint:
				return easeOutQuintInfo;
			case EaseType.EaseInOutQuint:
				return easeInOutQuintInfo;
			case EaseType.EaseInExpo:
				return easeInExpoInfo;
			case EaseType.EaseOutExpo:
				return easeOutExpoInfo;
			case EaseType.EaseInOutExpo:
				return easeInOutExpoInfo;
			case EaseType.EaseInCirc:
				return easeInCircInfo;
			case EaseType.EaseOutCirc:
				return easeOutCircInfo;
			case EaseType.EaseInOutCirc:
				return easeInOutCircInfo;
			case EaseType.EaseInElastic:
				return easeInElasticInfo;
			case EaseType.EaseOutElastic:
				return easeOutElasticInfo;
			case EaseType.EaseInOutElastic:
				return easeInOutElasticInfo;
			case EaseType.EaseInBack:
				return easeInBackInfo;
			case EaseType.EaseOutBack:
				return easeOutBackInfo;
			case EaseType.EaseInOutBack:
				return easeInOutBackInfo;
			case EaseType.EaseInBounce:
				return easeInBounceInfo;
			case EaseType.EaseOutBounce:
				return easeOutBounceInfo;
			case EaseType.EaseInOutBounce:
				return easeInOutBounceInfo;
			case EaseType.EaseInStrong:
				return easeInStrongInfo;
			case EaseType.EaseOutStrong:
				return easeOutStrongInfo;
			case EaseType.EaseInOutStrong:
				return easeInOutStrongInfo;
			default:
				return defaultEaseInfo;
			}
		}
	}
}
