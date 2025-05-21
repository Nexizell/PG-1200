using System;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	public class PlugRect : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(Rect) };

		internal static Type[] validValueTypes = new Type[1] { typeof(Rect) };

		private Rect typedStartVal;

		private Rect typedEndVal;

		private Rect diffChangeVal;

		protected override object startVal
		{
			get
			{
				return _startVal;
			}
			set
			{
				if (tweenObj.isFrom && isRelative)
				{
					typedStartVal = (Rect)value;
					typedStartVal.x += typedEndVal.x;
					typedStartVal.y += typedEndVal.y;
					typedStartVal.width += typedEndVal.width;
					typedStartVal.height += typedEndVal.height;
					_startVal = typedStartVal;
				}
				else
				{
					_startVal = (typedStartVal = (Rect)value);
				}
			}
		}

		protected override object endVal
		{
			get
			{
				return _endVal;
			}
			set
			{
				_endVal = (typedEndVal = (Rect)value);
			}
		}

		public PlugRect(Rect p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugRect(Rect p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugRect(Rect p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugRect(Rect p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugRect(Rect p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			float num = typedEndVal.width - typedStartVal.width;
			float num2 = typedEndVal.height - typedStartVal.height;
			float num3 = (float)Math.Sqrt(num * num + num2 * num2);
			float num4 = num3 / p_speed;
			if (num4 < 0f)
			{
				num4 = 0f - num4;
			}
			return num4;
		}

		protected override void SetChangeVal()
		{
			if (isRelative && !tweenObj.isFrom)
			{
				typedEndVal.x += typedStartVal.x;
				typedEndVal.y += typedStartVal.y;
				typedEndVal.width += typedStartVal.width;
				typedEndVal.height += typedStartVal.height;
			}
			diffChangeVal = default(Rect);
			diffChangeVal.x = typedEndVal.x - typedStartVal.x;
			diffChangeVal.y = typedEndVal.y - typedStartVal.y;
			diffChangeVal.width = typedEndVal.width - typedStartVal.width;
			diffChangeVal.height = typedEndVal.height - typedStartVal.height;
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			Rect rect = new Rect(diffChangeVal.x, diffChangeVal.y, diffChangeVal.width, diffChangeVal.height);
			rect.x *= p_diffIncr;
			rect.y *= p_diffIncr;
			rect.width *= p_diffIncr;
			rect.height *= p_diffIncr;
			typedStartVal.x += rect.x;
			typedStartVal.y += rect.y;
			typedStartVal.width += rect.width;
			typedStartVal.height += rect.height;
			typedEndVal.x += rect.x;
			typedEndVal.y += rect.y;
			typedEndVal.width += rect.width;
			typedEndVal.height += rect.height;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float num = ease(p_totElapsed, 0f, 1f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			Rect rect = default(Rect);
			rect.x = typedStartVal.x + diffChangeVal.x * num;
			rect.y = typedStartVal.y + diffChangeVal.y * num;
			rect.width = typedStartVal.width + diffChangeVal.width * num;
			rect.height = typedStartVal.height + diffChangeVal.height * num;
			SetValue(rect);
		}
	}
}
