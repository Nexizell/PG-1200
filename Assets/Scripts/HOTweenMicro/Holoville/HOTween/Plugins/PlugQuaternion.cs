using System;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugQuaternion : ABSTweenPlugin
	{
		internal static Type[] validPropTypes = new Type[1] { typeof(Quaternion) };

		internal static Type[] validValueTypes = new Type[2]
		{
			typeof(Vector3),
			typeof(Quaternion)
		};

		private Vector3 typedStartVal;

		private Vector3 typedEndVal;

		private Vector3 changeVal;

		private bool beyond360;

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
					typedStartVal = typedEndVal + ((value is Quaternion) ? ((Quaternion)value).eulerAngles : ((Vector3)value));
					_startVal = Quaternion.Euler(typedStartVal);
				}
				else if (value is Quaternion)
				{
					_startVal = value;
					typedStartVal = ((Quaternion)value).eulerAngles;
				}
				else
				{
					_startVal = Quaternion.Euler((Vector3)value);
					typedStartVal = (Vector3)value;
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
				if (value is Quaternion)
				{
					_endVal = value;
					typedEndVal = ((Quaternion)value).eulerAngles;
				}
				else
				{
					_endVal = Quaternion.Euler((Vector3)value);
					typedEndVal = (Vector3)value;
				}
			}
		}

		public PlugQuaternion(Quaternion p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugQuaternion(Quaternion p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugQuaternion(Quaternion p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugQuaternion(Quaternion p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugQuaternion(Vector3 p_endVal)
			: base(p_endVal, false)
		{
		}

		public PlugQuaternion(Vector3 p_endVal, EaseType p_easeType)
			: base(p_endVal, p_easeType, false)
		{
		}

		public PlugQuaternion(Vector3 p_endVal, bool p_isRelative)
			: base(p_endVal, p_isRelative)
		{
		}

		public PlugQuaternion(Vector3 p_endVal, EaseType p_easeType, bool p_isRelative)
			: base(p_endVal, p_easeType, p_isRelative)
		{
		}

		public PlugQuaternion(Vector3 p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		public PlugQuaternion(Quaternion p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
			: base(p_endVal, p_easeAnimCurve, p_isRelative)
		{
		}

		public PlugQuaternion Beyond360()
		{
			return Beyond360(true);
		}

		public PlugQuaternion Beyond360(bool p_beyond360)
		{
			beyond360 = p_beyond360;
			return this;
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			float num = changeVal.magnitude / (p_speed * 360f);
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
				return;
			}
			if (beyond360)
			{
				changeVal = typedEndVal - typedStartVal;
				return;
			}
			Vector3 vector = typedEndVal;
			if (vector.x > 360f)
			{
				vector.x %= 360f;
			}
			if (vector.y > 360f)
			{
				vector.y %= 360f;
			}
			if (vector.z > 360f)
			{
				vector.z %= 360f;
			}
			changeVal = vector - typedStartVal;
			float num = ((changeVal.x > 0f) ? changeVal.x : (0f - changeVal.x));
			if (num > 180f)
			{
				changeVal.x = ((changeVal.x > 0f) ? (0f - (360f - num)) : (360f - num));
			}
			num = ((changeVal.y > 0f) ? changeVal.y : (0f - changeVal.y));
			if (num > 180f)
			{
				changeVal.y = ((changeVal.y > 0f) ? (0f - (360f - num)) : (360f - num));
			}
			num = ((changeVal.z > 0f) ? changeVal.z : (0f - changeVal.z));
			if (num > 180f)
			{
				changeVal.z = ((changeVal.z > 0f) ? (0f - (360f - num)) : (360f - num));
			}
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			typedStartVal += changeVal * p_diffIncr;
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			float num = ease(p_totElapsed, 0f, 1f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			SetValue(Quaternion.Euler(new Vector3(typedStartVal.x + changeVal.x * num, typedStartVal.y + changeVal.y * num, typedStartVal.z + changeVal.z * num)));
		}
	}
}
