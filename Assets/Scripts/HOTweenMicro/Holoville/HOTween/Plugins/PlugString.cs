using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugString : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(string) };

		internal static Type[] validValueTypes = new Type[1] { typeof(string) };

		private string typedStartVal;

		private string typedEndVal;

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
					_startVal = (typedStartVal = typedEndVal + ((value == null) ? "" : value.ToString()));
				}
				else
				{
					_startVal = (typedStartVal = ((value == null) ? "" : value.ToString()));
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
				_endVal = (typedEndVal = ((value == null) ? "" : value.ToString()));
			}
		}

		public PlugString(string p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugString(string p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugString(string p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugString(string p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugString(string p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
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
			changeVal = typedEndVal.Length;
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			if (p_diffIncr > 0)
			{
				while (p_diffIncr > 0)
				{
					typedStartVal += typedEndVal;
					p_diffIncr--;
				}
			}
			else
			{
				typedStartVal = typedStartVal.Substring(0, typedStartVal.Length + typedEndVal.Length * p_diffIncr);
			}
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			int num = (int)Math.Round(ease(p_totElapsed, 0f, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod));
			string value = ((!isRelative) ? (typedEndVal.Substring(0, num) + (((float)num >= changeVal || num >= typedStartVal.Length) ? "" : typedStartVal.Substring(num))) : (typedStartVal + typedEndVal.Substring(0, num)));
			SetValue(value);
		}
	}
}
