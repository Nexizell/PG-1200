using Holoville.HOTween.Core;

namespace Holoville.HOTween
{
	public class TweenVar
	{
		public float duration;

		private float _value;

		private float _startVal;

		private float _endVal;

		private EaseType _easeType;

		private float _elapsed;

		private float changeVal;

		private TweenDelegate.EaseFunc ease;

		public float startVal
		{
			get
			{
				return _startVal;
			}
			set
			{
				_startVal = value;
				SetChangeVal();
			}
		}

		public float endVal
		{
			get
			{
				return _endVal;
			}
			set
			{
				_endVal = value;
				SetChangeVal();
			}
		}

		public EaseType easeType
		{
			get
			{
				return _easeType;
			}
			set
			{
				_easeType = value;
				ease = EaseInfo.GetEaseInfo(_easeType).ease;
			}
		}

		public float value
		{
			get
			{
				return _value;
			}
		}

		public float elapsed
		{
			get
			{
				return _elapsed;
			}
		}

		public TweenVar(float p_startVal, float p_endVal, float p_duration)
			: this(p_startVal, p_endVal, p_duration, EaseType.Linear)
		{
		}

		public TweenVar(float p_startVal, float p_endVal, float p_duration, EaseType p_easeType)
		{
			startVal = (_value = p_startVal);
			endVal = p_endVal;
			duration = p_duration;
			easeType = p_easeType;
		}

		public float Update(float p_elapsed)
		{
			return Update(p_elapsed, false);
		}

		public float Update(float p_elapsed, bool p_relative)
		{
			_elapsed = (p_relative ? (_elapsed + p_elapsed) : p_elapsed);
			if (_elapsed > duration)
			{
				_elapsed = duration;
			}
			else if (_elapsed < 0f)
			{
				_elapsed = 0f;
			}
			_value = ease(_elapsed, _startVal, changeVal, duration, HOTween.defEaseOvershootOrAmplitude, HOTween.defEasePeriod);
			return _value;
		}

		private void SetChangeVal()
		{
			changeVal = endVal - startVal;
		}
	}
}
