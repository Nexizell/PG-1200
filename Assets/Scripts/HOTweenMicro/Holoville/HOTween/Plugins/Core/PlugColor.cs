using System;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	public class PlugColor : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(Color) };

		internal static Type[] validValueTypes = new Type[1] { typeof(Color) };

		private Color typedStartVal;

		private Color typedEndVal;

		private Color diffChangeVal;

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
					_startVal = (typedStartVal = typedEndVal + (Color)value);
				}
				else
				{
					_startVal = (typedStartVal = (Color)value);
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
				_endVal = (typedEndVal = (Color)value);
			}
		}

		public PlugColor(Color p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugColor(Color p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugColor(Color p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugColor(Color p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugColor(Color p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			float num = 1f / p_speed;
			if (num < 0f)
			{
				num = 0f - num;
			}
			return num;
		}

		protected override void SetChangeVal()
		{
			if (isRelative && !tweenObj.isFrom)
			{
				typedEndVal = typedStartVal + typedEndVal;
			}
			diffChangeVal = typedEndVal - typedStartVal;
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			typedStartVal += diffChangeVal * p_diffIncr;
			typedEndVal += diffChangeVal * p_diffIncr;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float num = ease(p_totElapsed, 0f, 1f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			SetValue(new Color(typedStartVal.r + diffChangeVal.r * num, typedStartVal.g + diffChangeVal.g * num, typedStartVal.b + diffChangeVal.b * num, typedStartVal.a + diffChangeVal.a * num));
		}
	}
}
