using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugSetFloat : ABSTweenPlugin
	{
		internal static Type[] validTargetTypes = new Type[1] { typeof(Material) };

		internal static Type[] validPropTypes = new Type[1] { typeof(Color) };

		internal static Type[] validValueTypes = new Type[1] { typeof(float) };

		private float typedStartVal;

		private float typedEndVal;

		private float changeVal;

		private string floatName;

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

		public PlugSetFloat(float p_endVal)
			: this(p_endVal, false)
		{
		}

		public PlugSetFloat(float p_endVal, EaseType p_easeType)
			: this(p_endVal, p_easeType, false)
		{
		}

		public PlugSetFloat(float p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetFloat(float p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetFloat(float p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetFloat Property(string p_propertyName)
		{
			floatName = p_propertyName;
			return this;
		}

		internal override bool ValidateTarget(object p_target)
		{
			return p_target is Material;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			SetValue(ease(p_totElapsed, typedStartVal, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod));
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

		protected override void SetValue(object p_value)
		{
			((Material)tweenObj.target).SetFloat(floatName, Convert.ToSingle(p_value));
		}

		protected override object GetValue()
		{
			return ((Material)tweenObj.target).GetFloat(floatName);
		}
	}
}
