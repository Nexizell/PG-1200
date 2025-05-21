using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugInt : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[2]
		{
			typeof(float),
			typeof(int)
		};

		internal static Type[] validValueTypes = new Type[1] { typeof(int) };

		private float typedStartVal;

		private float typedEndVal;

		private float changeVal;

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
					_startVal = (typedStartVal = typedEndVal + Convert.ToSingle(value));
				}
				else
				{
					_startVal = (typedStartVal = Convert.ToSingle(value));
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
				_endVal = (typedEndVal = Convert.ToSingle(value));
			}
		}

		public PlugInt(float p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugInt(float p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugInt(float p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugInt(float p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugInt(float p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			float num = changeVal / p_speed;
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
				changeVal = typedEndVal;
				endVal = typedStartVal + typedEndVal;
			}
			else
			{
				changeVal = typedEndVal - typedStartVal;
			}
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			typedStartVal += changeVal * (float)p_diffIncr;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float num = ease(p_totElapsed, typedStartVal, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			SetValue((float)Math.Round(num));
		}
	}
}
