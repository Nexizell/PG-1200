using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugSetColor : ABSTweenPlugin
	{
		public enum ColorName
		{
			_Color = 0,
			_SpecColor = 1,
			_Emission = 2,
			_ReflectColor = 3
		}

		internal static Type[] validTargetTypes = new Type[1] { typeof(Material) };

		internal static Type[] validPropTypes = new Type[1] { typeof(Color) };

		internal static Type[] validValueTypes = new Type[1] { typeof(Color) };

		private Color typedStartVal;

		private Color typedEndVal;

		private float changeVal;

		private Color diffChangeVal;

		private string colorName;

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

		public PlugSetColor(Color p_endVal)
			: this(p_endVal, false)
		{
		}

		public PlugSetColor(Color p_endVal, EaseType p_easeType)
			: this(p_endVal, p_easeType, false)
		{
		}

		public PlugSetColor(Color p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetColor(Color p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetColor(Color p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
			ignoreAccessor = true;
		}

		public PlugSetColor Property(ColorName p_colorName)
		{
			return Property(p_colorName.ToString());
		}

		public PlugSetColor Property(string p_propertyName)
		{
			colorName = p_propertyName;
			return this;
		}

		internal override bool ValidateTarget(object p_target)
		{
			return p_target is Material;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float t = ease(p_totElapsed, 0f, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			SetValue(Color.Lerp(typedStartVal, typedEndVal, t));
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
			changeVal = 1f;
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

		protected override void SetValue(object p_value)
		{
			((Material)tweenObj.target).SetColor(colorName, (Color)p_value);
		}

		protected override object GetValue()
		{
			return ((Material)tweenObj.target).GetColor(colorName);
		}
	}
}
