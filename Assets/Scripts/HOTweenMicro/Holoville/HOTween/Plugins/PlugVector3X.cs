using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugVector3X : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(Vector3) };

		internal static Type[] validValueTypes = new Type[1] { typeof(float) };

		protected float typedStartVal;

		protected float typedEndVal;

		protected float changeVal;

		internal override int pluginId
		{
			get
			{
				return 1;
			}
		}

		protected override object startVal
		{
			get
			{
				return _startVal;
			}
			set
			{
				if (tweenObj.isFrom)
				{
					if (isRelative)
					{
						_startVal = (typedStartVal = typedEndVal + Convert.ToSingle(value));
					}
					else
					{
						_startVal = (typedStartVal = Convert.ToSingle(value));
					}
				}
				else
				{
					_startVal = value;
					typedStartVal = ((Vector3)_startVal).x;
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
				if (tweenObj.isFrom)
				{
					_endVal = value;
					typedEndVal = ((Vector3)_endVal).x;
				}
				else
				{
					_endVal = (typedEndVal = Convert.ToSingle(value));
				}
			}
		}

		public PlugVector3X(float p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugVector3X(float p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugVector3X(float p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugVector3X(float p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugVector3X(float p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
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

		internal override void Rewind()
		{
			Vector3 vector = (Vector3)GetValue();
			vector.x = typedStartVal;
			SetValue(vector);
		}

		internal override void Complete()
		{
			Vector3 vector = (Vector3)GetValue();
			vector.x = typedEndVal;
			SetValue(vector);
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
			Vector3 vector = (Vector3)GetValue();
			vector.x = ease(p_totElapsed, typedStartVal, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			if (tweenObj.pixelPerfect)
			{
				vector.x = (int)vector.x;
			}
			SetValue(vector);
		}
	}
}
