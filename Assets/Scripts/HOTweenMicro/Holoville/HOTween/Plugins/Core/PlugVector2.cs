using System;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	public class PlugVector2 : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(Vector2) };

		internal static Type[] validValueTypes = new Type[1] { typeof(Vector2) };

		private Vector2 typedStartVal;

		private Vector2 typedEndVal;

		private Vector2 changeVal;

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
					_startVal = (typedStartVal = typedEndVal + (Vector2)value);
				}
				else
				{
					_startVal = (typedStartVal = (Vector2)value);
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
				_endVal = (typedEndVal = (Vector2)value);
			}
		}

		public PlugVector2(Vector2 p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugVector2(Vector2 p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugVector2(Vector2 p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugVector2(Vector2 p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugVector2(Vector2 p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			float num = changeVal.magnitude / p_speed;
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
				changeVal = new Vector2(typedEndVal.x - typedStartVal.x, typedEndVal.y - typedStartVal.y);
			}
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			typedStartVal += changeVal * p_diffIncr;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float num = ease(p_totElapsed, 0f, 1f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			if (tweenObj.pixelPerfect)
			{
				SetValue(new Vector2((int)(typedStartVal.x + changeVal.x * num), (int)(typedStartVal.y + changeVal.y * num)));
			}
			else
			{
				SetValue(new Vector2(typedStartVal.x + changeVal.x * num, typedStartVal.y + changeVal.y * num));
			}
		}
	}
}
