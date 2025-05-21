using System;
using System.Reflection;
using Holoville.HOTween.Core;
using Holoville.HOTween.Core.Easing;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	public abstract class ABSTweenPlugin
	{
		protected object _startVal;

		protected object _endVal;

		protected float _duration;

		private bool _initialized;

		private bool _easeReversed;

		protected string _propName;

		internal Type targetType;

		protected TweenDelegate.EaseFunc ease;

		protected bool isRelative;

		protected bool ignoreAccessor;

		private EaseType easeType;

		private EaseInfo easeInfo;

		private EaseCurve easeCurve;

		internal bool wasStarted;

		private bool speedBasedDurationWasSet;

		private int prevCompletedLoops;

		private bool _useSpeedTransformAccessors;

		private Transform _transformTarget;

		private TweenDelegate.HOAction<Vector3> _setTransformVector3;

		private TweenDelegate.HOFunc<Vector3> _getTransformVector3;

		private TweenDelegate.HOAction<Quaternion> _setTransformQuaternion;

		private TweenDelegate.HOFunc<Quaternion> _getTransformQuaternion;

		internal PropertyInfo propInfo;

		internal FieldInfo fieldInfo;

		protected Tweener tweenObj;

		protected abstract object startVal { get; set; }

		protected abstract object endVal { get; set; }

		internal bool initialized
		{
			get
			{
				return _initialized;
			}
		}

		internal float duration
		{
			get
			{
				return _duration;
			}
		}

		internal bool easeReversed
		{
			get
			{
				return _easeReversed;
			}
		}

		internal string propName
		{
			get
			{
				return _propName;
			}
		}

		internal virtual int pluginId
		{
			get
			{
				return -1;
			}
		}

		protected ABSTweenPlugin(object p_endVal, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
		}

		protected ABSTweenPlugin(object p_endVal, EaseType p_easeType, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
			easeType = p_easeType;
			easeInfo = EaseInfo.GetEaseInfo(p_easeType);
			ease = easeInfo.ease;
		}

		protected ABSTweenPlugin(object p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
			easeType = EaseType.AnimationCurve;
			easeCurve = new EaseCurve(p_easeAnimCurve);
			easeInfo = null;
			ease = easeCurve.Evaluate;
		}

		internal virtual void Init(Tweener p_tweenObj, string p_propertyName, EaseType p_easeType, Type p_targetType, PropertyInfo p_propertyInfo, FieldInfo p_fieldInfo)
		{
			_initialized = true;
			tweenObj = p_tweenObj;
			_propName = p_propertyName;
			targetType = p_targetType;
			if ((easeType != EaseType.AnimationCurve && easeInfo == null) || tweenObj.speedBased || (easeType == EaseType.AnimationCurve && easeCurve == null))
			{
				SetEase(p_easeType);
			}
			_duration = tweenObj.duration;
			if (targetType == typeof(Transform))
			{
				_transformTarget = p_tweenObj.target as Transform;
				_useSpeedTransformAccessors = true;
				switch (_propName)
				{
				case "position":
					_setTransformVector3 = delegate(Vector3 value)
					{
						_transformTarget.position = value;
					};
					_getTransformVector3 = () => _transformTarget.position;
					break;
				case "localPosition":
					_setTransformVector3 = delegate(Vector3 value)
					{
						_transformTarget.localPosition = value;
					};
					_getTransformVector3 = () => _transformTarget.localPosition;
					break;
				case "localScale":
					_setTransformVector3 = delegate(Vector3 value)
					{
						_transformTarget.localScale = value;
					};
					_getTransformVector3 = () => _transformTarget.localScale;
					break;
				case "rotation":
					_setTransformQuaternion = delegate(Quaternion value)
					{
						_transformTarget.rotation = value;
					};
					_getTransformQuaternion = () => _transformTarget.rotation;
					break;
				case "localRotation":
					_setTransformQuaternion = delegate(Quaternion value)
					{
						_transformTarget.localRotation = value;
					};
					_getTransformQuaternion = () => _transformTarget.localRotation;
					break;
				default:
					_transformTarget = null;
					_useSpeedTransformAccessors = false;
					break;
				}
			}
			if (!_useSpeedTransformAccessors)
			{
				propInfo = p_propertyInfo;
				fieldInfo = p_fieldInfo;
			}
		}

		internal void Startup()
		{
			Startup(false);
		}

		internal void Startup(bool p_onlyCalcSpeedBasedDur)
		{
			if (wasStarted)
			{
				TweenWarning.Log(string.Concat("Startup() for plugin ", this, " (target: ", tweenObj.target, ") has already been called. Startup() won't execute twice."));
				return;
			}
			object obj = null;
			object obj2 = null;
			if (p_onlyCalcSpeedBasedDur)
			{
				if (tweenObj.speedBased && !speedBasedDurationWasSet)
				{
					obj = _startVal;
					obj2 = _endVal;
				}
			}
			else
			{
				wasStarted = true;
			}
			if (tweenObj.isFrom)
			{
				object obj3 = _endVal;
				endVal = GetValue();
				startVal = obj3;
			}
			else
			{
				endVal = _endVal;
				startVal = GetValue();
			}
			SetChangeVal();
			if (tweenObj.speedBased && !speedBasedDurationWasSet)
			{
				_duration = GetSpeedBasedDuration(_duration);
				speedBasedDurationWasSet = true;
				if (p_onlyCalcSpeedBasedDur)
				{
					_startVal = obj;
					_endVal = obj2;
				}
			}
		}

		internal void ForceSetSpeedBasedDuration()
		{
			if (!speedBasedDurationWasSet)
			{
				Startup(true);
			}
		}

		internal virtual bool ValidateTarget(object p_target)
		{
			return true;
		}

		internal void Update(float p_totElapsed)
		{
			if (tweenObj.loopType == LoopType.Incremental)
			{
				if (prevCompletedLoops != tweenObj.completedLoops)
				{
					int num = tweenObj.completedLoops;
					if (tweenObj._loops != -1 && num >= tweenObj._loops)
					{
						num--;
					}
					int num2 = num - prevCompletedLoops;
					if (num2 != 0)
					{
						SetIncremental(num2);
						prevCompletedLoops = num;
					}
				}
			}
			else if (prevCompletedLoops != 0)
			{
				SetIncremental(-prevCompletedLoops);
				prevCompletedLoops = 0;
			}
			if (p_totElapsed > _duration)
			{
				p_totElapsed = _duration;
			}
			DoUpdate(p_totElapsed);
		}

		protected abstract void DoUpdate(float p_totElapsed);

		internal virtual void Rewind()
		{
			SetValue(startVal);
		}

		internal virtual void Complete()
		{
			SetValue(_endVal);
		}

		internal void ReverseEase()
		{
			_easeReversed = !_easeReversed;
			if (easeType != EaseType.AnimationCurve && easeInfo.inverseEase != null)
			{
				ease = (_easeReversed ? easeInfo.inverseEase : easeInfo.ease);
			}
		}

		internal void SetEase(EaseType p_easeType)
		{
			easeType = p_easeType;
			if (easeType == EaseType.AnimationCurve)
			{
				if (tweenObj._easeAnimationCurve != null)
				{
					easeCurve = new EaseCurve(tweenObj._easeAnimationCurve);
					ease = easeCurve.Evaluate;
				}
				else
				{
					easeType = EaseType.EaseOutQuad;
					easeInfo = EaseInfo.GetEaseInfo(easeType);
					ease = easeInfo.ease;
				}
			}
			else
			{
				easeInfo = EaseInfo.GetEaseInfo(easeType);
				ease = easeInfo.ease;
			}
			if (_easeReversed && easeInfo.inverseEase != null)
			{
				ease = easeInfo.inverseEase;
			}
		}

		protected abstract float GetSpeedBasedDuration(float p_speed);

		internal ABSTweenPlugin CloneBasic()
		{
			return Activator.CreateInstance(GetType(), (tweenObj != null && tweenObj.isFrom) ? _startVal : _endVal, easeType, isRelative) as ABSTweenPlugin;
		}

		protected abstract void SetChangeVal();

		internal void ForceSetIncremental(int p_diffIncr)
		{
			SetIncremental(p_diffIncr);
		}

		protected abstract void SetIncremental(int p_diffIncr);

		protected virtual void SetValue(object p_value)
		{
			if (_useSpeedTransformAccessors)
			{
				if (_setTransformVector3 != null)
				{
					_setTransformVector3((Vector3)p_value);
				}
				else
				{
					_setTransformQuaternion((Quaternion)p_value);
				}
				return;
			}
			if (propInfo != null)
			{
				try
				{
					propInfo.SetValue(tweenObj.target, p_value, null);
					return;
				}
				catch (InvalidCastException)
				{
					propInfo.SetValue(tweenObj.target, (int)Math.Floor((float)p_value), null);
					return;
				}
				catch (ArgumentException)
				{
					propInfo.SetValue(tweenObj.target, (int)Math.Floor((float)p_value), null);
					return;
				}
			}
			try
			{
				fieldInfo.SetValue(tweenObj.target, p_value);
			}
			catch (InvalidCastException)
			{
				fieldInfo.SetValue(tweenObj.target, (int)Math.Floor((float)p_value));
			}
			catch (ArgumentException)
			{
				fieldInfo.SetValue(tweenObj.target, (int)Math.Floor((float)p_value));
			}
		}

		protected virtual object GetValue()
		{
			if (_useSpeedTransformAccessors)
			{
				if (_getTransformVector3 != null)
				{
					return _getTransformVector3();
				}
				return _getTransformQuaternion();
			}
			if (propInfo != null)
			{
				return propInfo.GetGetMethod().Invoke(tweenObj.target, null);
			}
			return fieldInfo.GetValue(tweenObj.target);
		}
	}
}
