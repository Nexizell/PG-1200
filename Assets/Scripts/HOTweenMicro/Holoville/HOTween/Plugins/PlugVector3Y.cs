using System;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugVector3Y : PlugVector3X
	{
		internal override int pluginId
		{
			get
			{
				return 2;
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
					typedStartVal = ((Vector3)_startVal).y;
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
					typedEndVal = ((Vector3)_endVal).y;
				}
				else
				{
					_endVal = (typedEndVal = Convert.ToSingle(value));
				}
			}
		}

		public PlugVector3Y(float p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugVector3Y(float p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugVector3Y(float p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugVector3Y(float p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugVector3Y(float p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		internal override void Rewind()
		{
			Vector3 vector = (Vector3)GetValue();
			vector.y = typedStartVal;
			SetValue(vector);
		}

		internal override void Complete()
		{
			Vector3 vector = (Vector3)GetValue();
			vector.y = typedEndVal;
			SetValue(vector);
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			Vector3 vector = (Vector3)GetValue();
			vector.y = ease(p_totElapsed, typedStartVal, changeVal, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			if (tweenObj.pixelPerfect)
			{
				vector.y = (int)vector.y;
			}
			SetValue(vector);
		}
	}
}
