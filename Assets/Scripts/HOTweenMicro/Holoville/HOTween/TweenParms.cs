using System;
using System.Collections.Generic;
using System.Reflection;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween
{
	public class TweenParms : ABSTweenComponentParms
	{
		private class HOTPropData
		{
			public readonly string propName;

			public readonly object endValOrPlugin;

			public readonly bool isRelative;

			public HOTPropData(string p_propName, object p_endValOrPlugin, bool p_isRelative)
			{
				propName = p_propName;
				endValOrPlugin = p_endValOrPlugin;
				isRelative = p_isRelative;
			}
		}

		private static readonly Dictionary<Type, string> _TypeToShortString = new Dictionary<Type, string>(8)
		{
			{
				typeof(Vector2),
				"Vector2"
			},
			{
				typeof(Vector3),
				"Vector3"
			},
			{
				typeof(Vector4),
				"Vector4"
			},
			{
				typeof(Quaternion),
				"Quaternion"
			},
			{
				typeof(Color),
				"Color"
			},
			{
				typeof(Color32),
				"Color32"
			},
			{
				typeof(Rect),
				"Rect"
			},
			{
				typeof(string),
				"String"
			},
			{
				typeof(int),
				"Int32"
			}
		};

		private bool pixelPerfect;

		private bool speedBased;

		private EaseType easeType = HOTween.defEaseType;

		private AnimationCurve easeAnimCurve;

		private float easeOvershootOrAmplitude = HOTween.defEaseOvershootOrAmplitude;

		private float easePeriod = HOTween.defEasePeriod;

		private float delay;

		private List<HOTPropData> propDatas;

		private bool isFrom;

		private TweenDelegate.TweenCallback onPluginOverwritten;

		private TweenDelegate.TweenCallbackWParms onPluginOverwrittenWParms;

		private object[] onPluginOverwrittenParms;

		public bool hasProps
		{
			get
			{
				return propDatas != null;
			}
		}

		internal void InitializeObject(Tweener p_tweenObj, object p_target)
		{
			InitializeOwner(p_tweenObj);
			if (speedBased)
			{
				easeType = EaseType.Linear;
			}
			p_tweenObj._pixelPerfect = pixelPerfect;
			p_tweenObj._speedBased = speedBased;
			p_tweenObj._easeType = easeType;
			p_tweenObj._easeAnimationCurve = easeAnimCurve;
			p_tweenObj._easeOvershootOrAmplitude = easeOvershootOrAmplitude;
			p_tweenObj._easePeriod = easePeriod;
			p_tweenObj._delay = (p_tweenObj.delayCount = delay);
			p_tweenObj.isFrom = isFrom;
			p_tweenObj.onPluginOverwritten = onPluginOverwritten;
			p_tweenObj.onPluginOverwrittenWParms = onPluginOverwrittenWParms;
			p_tweenObj.onPluginOverwrittenParms = onPluginOverwrittenParms;
			p_tweenObj.plugins = new List<ABSTweenPlugin>();
			Type type = p_target.GetType();
			FieldInfo fieldInfo = null;
			int count = propDatas.Count;
			for (int i = 0; i < count; i++)
			{
				HOTPropData hOTPropData = propDatas[i];
				PropertyInfo property = type.GetProperty(hOTPropData.propName);
				if (property == null)
				{
					fieldInfo = type.GetField(hOTPropData.propName);
					if (fieldInfo == null)
					{
						TweenWarning.Log(string.Concat("\"", p_target, ".", hOTPropData.propName, "\" is missing, static, or not public. The tween for this property will not be created."));
						continue;
					}
				}
				ABSTweenPlugin aBSTweenPlugin = hOTPropData.endValOrPlugin as ABSTweenPlugin;
				ABSTweenPlugin aBSTweenPlugin2;
				if (aBSTweenPlugin != null)
				{
					aBSTweenPlugin2 = aBSTweenPlugin;
					if (!aBSTweenPlugin2.ValidateTarget(p_target))
					{
						TweenWarning.Log(string.Concat(Utils.SimpleClassName(aBSTweenPlugin2.GetType()), " : Invalid target (", p_target, "). The tween for this property will not be created."));
						continue;
					}
					if (aBSTweenPlugin2.initialized)
					{
						aBSTweenPlugin2 = aBSTweenPlugin2.CloneBasic();
					}
				}
				else
				{
					aBSTweenPlugin2 = null;
					switch ((property == null) ? (_TypeToShortString.ContainsKey(fieldInfo.FieldType) ? _TypeToShortString[fieldInfo.FieldType] : "") : (_TypeToShortString.ContainsKey(property.PropertyType) ? _TypeToShortString[property.PropertyType] : ""))
					{
					case "Vector2":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugVector2.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugVector2((Vector2)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "Vector3":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugVector3.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugVector3((Vector3)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "Vector4":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugVector4.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugVector4((Vector4)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "Quaternion":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugQuaternion.validValueTypes))
						{
							aBSTweenPlugin2 = ((!(hOTPropData.endValOrPlugin is Vector3)) ? new PlugQuaternion((Quaternion)hOTPropData.endValOrPlugin, hOTPropData.isRelative) : new PlugQuaternion((Vector3)hOTPropData.endValOrPlugin, hOTPropData.isRelative));
						}
						break;
					case "Color":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugColor.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugColor((Color)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "Color32":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugColor32.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugColor32((Color32)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "Rect":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugRect.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugRect((Rect)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					case "String":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugString.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugString(hOTPropData.endValOrPlugin.ToString(), hOTPropData.isRelative);
						}
						break;
					case "Int32":
						if (ValidateValue(hOTPropData.endValOrPlugin, PlugInt.validValueTypes))
						{
							aBSTweenPlugin2 = new PlugInt((int)hOTPropData.endValOrPlugin, hOTPropData.isRelative);
						}
						break;
					default:
						try
						{
							aBSTweenPlugin2 = new PlugFloat(Convert.ToSingle(hOTPropData.endValOrPlugin), hOTPropData.isRelative);
						}
						catch (Exception)
						{
							TweenWarning.Log(string.Concat("No valid plugin for animating \"", p_target, ".", hOTPropData.propName, "\" (of type ", (property != null) ? property.PropertyType : fieldInfo.FieldType, "). The tween for this property will not be created."));
							continue;
						}
						break;
					}
					if (aBSTweenPlugin2 == null)
					{
						TweenWarning.Log(string.Concat("The end value set for \"", p_target, ".", hOTPropData.propName, "\" tween is invalid. The tween for this property will not be created."));
						continue;
					}
				}
				aBSTweenPlugin2.Init(p_tweenObj, hOTPropData.propName, easeType, type, property, fieldInfo);
				p_tweenObj.plugins.Add(aBSTweenPlugin2);
			}
		}

		public TweenParms PixelPerfect()
		{
			pixelPerfect = true;
			return this;
		}

		public TweenParms SpeedBased()
		{
			return SpeedBased(true);
		}

		public TweenParms SpeedBased(bool p_speedBased)
		{
			speedBased = p_speedBased;
			return this;
		}

		public TweenParms Ease(EaseType p_easeType)
		{
			return Ease(p_easeType, HOTween.defEaseOvershootOrAmplitude, HOTween.defEasePeriod);
		}

		public TweenParms Ease(EaseType p_easeType, float p_overshoot)
		{
			return Ease(p_easeType, p_overshoot, HOTween.defEasePeriod);
		}

		public TweenParms Ease(EaseType p_easeType, float p_amplitude, float p_period)
		{
			easeType = p_easeType;
			easeOvershootOrAmplitude = p_amplitude;
			easePeriod = p_period;
			return this;
		}

		public TweenParms Ease(AnimationCurve p_easeAnimationCurve)
		{
			easeType = EaseType.AnimationCurve;
			easeAnimCurve = p_easeAnimationCurve;
			return this;
		}

		public TweenParms Delay(float p_delay)
		{
			delay = p_delay;
			return this;
		}

		public TweenParms Pause()
		{
			return Pause(true);
		}

		public TweenParms Pause(bool p_pause)
		{
			isPaused = p_pause;
			return this;
		}

		public TweenParms NewProp(string p_propName, ABSTweenPlugin p_plugin)
		{
			return NewProp(p_propName, p_plugin, false);
		}

		public TweenParms NewProp(string p_propName, object p_endVal)
		{
			return NewProp(p_propName, p_endVal, false);
		}

		public TweenParms NewProp(string p_propName, object p_endVal, bool p_isRelative)
		{
			propDatas = null;
			return Prop(p_propName, p_endVal, p_isRelative);
		}

		public TweenParms Prop(string p_propName, ABSTweenPlugin p_plugin)
		{
			return Prop(p_propName, p_plugin, false);
		}

		public TweenParms Prop(string p_propName, object p_endVal)
		{
			return Prop(p_propName, p_endVal, false);
		}

		public TweenParms Prop(string p_propName, object p_endVal, bool p_isRelative)
		{
			if (propDatas == null)
			{
				propDatas = new List<HOTPropData>();
			}
			propDatas.Add(new HOTPropData(p_propName, p_endVal, p_isRelative));
			return this;
		}

		public TweenParms Id(string p_id)
		{
			id = p_id;
			return this;
		}

		public TweenParms IntId(int p_intId)
		{
			intId = p_intId;
			return this;
		}

		public TweenParms AutoKill(bool p_active)
		{
			autoKillOnComplete = p_active;
			return this;
		}

		public TweenParms UpdateType(UpdateType p_updateType)
		{
			updateType = p_updateType;
			return this;
		}

		public TweenParms TimeScale(float p_timeScale)
		{
			timeScale = p_timeScale;
			return this;
		}

		public TweenParms Loops(int p_loops)
		{
			return Loops(p_loops, HOTween.defLoopType);
		}

		public TweenParms Loops(int p_loops, LoopType p_loopType)
		{
			loops = p_loops;
			loopType = p_loopType;
			return this;
		}

		public TweenParms OnStart(TweenDelegate.TweenCallback p_function)
		{
			onStart = p_function;
			return this;
		}

		public TweenParms OnStart(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onStartWParms = p_function;
			onStartParms = p_funcParms;
			return this;
		}

		public TweenParms OnUpdate(TweenDelegate.TweenCallback p_function)
		{
			onUpdate = p_function;
			return this;
		}

		public TweenParms OnUpdate(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onUpdateWParms = p_function;
			onUpdateParms = p_funcParms;
			return this;
		}

		public TweenParms OnPluginUpdated(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onPluginUpdatedWParms = p_function;
			onPluginUpdatedParms = p_funcParms;
			return this;
		}

		public TweenParms OnPause(TweenDelegate.TweenCallback p_function)
		{
			onPause = p_function;
			return this;
		}

		public TweenParms OnPause(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onPauseWParms = p_function;
			onPauseParms = p_funcParms;
			return this;
		}

		public TweenParms OnPlay(TweenDelegate.TweenCallback p_function)
		{
			onPlay = p_function;
			return this;
		}

		public TweenParms OnPlay(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onPlayWParms = p_function;
			onPlayParms = p_funcParms;
			return this;
		}

		public TweenParms OnRewinded(TweenDelegate.TweenCallback p_function)
		{
			onRewinded = p_function;
			return this;
		}

		public TweenParms OnRewinded(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onRewindedWParms = p_function;
			onRewindedParms = p_funcParms;
			return this;
		}

		public TweenParms OnStepComplete(TweenDelegate.TweenCallback p_function)
		{
			onStepComplete = p_function;
			return this;
		}

		public TweenParms OnStepComplete(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onStepCompleteWParms = p_function;
			onStepCompleteParms = p_funcParms;
			return this;
		}

		public TweenParms OnStepComplete(GameObject p_sendMessageTarget, string p_methodName, object p_value = null, SendMessageOptions p_options = SendMessageOptions.RequireReceiver)
		{
			onStepCompleteWParms = HOTween.DoSendMessage;
			onStepCompleteParms = new object[4] { p_sendMessageTarget, p_methodName, p_value, p_options };
			return this;
		}

		public TweenParms OnComplete(TweenDelegate.TweenCallback p_function)
		{
			onComplete = p_function;
			return this;
		}

		public TweenParms OnComplete(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onCompleteWParms = p_function;
			onCompleteParms = p_funcParms;
			return this;
		}

		public TweenParms OnComplete(GameObject p_sendMessageTarget, string p_methodName, object p_value = null, SendMessageOptions p_options = SendMessageOptions.RequireReceiver)
		{
			onCompleteWParms = HOTween.DoSendMessage;
			onCompleteParms = new object[4] { p_sendMessageTarget, p_methodName, p_value, p_options };
			return this;
		}

		public TweenParms OnPluginOverwritten(TweenDelegate.TweenCallback p_function)
		{
			onPluginOverwritten = p_function;
			return this;
		}

		public TweenParms OnPluginOverwritten(TweenDelegate.TweenCallbackWParms p_function, params object[] p_funcParms)
		{
			onPluginOverwrittenWParms = p_function;
			onPluginOverwrittenParms = p_funcParms;
			return this;
		}

		public TweenParms KeepEnabled(Behaviour p_target)
		{
			if (p_target == null)
			{
				manageBehaviours = false;
				return this;
			}
			return KeepEnabled(new Behaviour[1] { p_target }, true);
		}

		public TweenParms KeepEnabled(GameObject p_target)
		{
			if (p_target == null)
			{
				manageGameObjects = false;
				return this;
			}
			return KeepEnabled(new GameObject[1] { p_target }, true);
		}

		public TweenParms KeepEnabled(Behaviour[] p_targets)
		{
			return KeepEnabled(p_targets, true);
		}

		public TweenParms KeepEnabled(GameObject[] p_targets)
		{
			return KeepEnabled(p_targets, true);
		}

		public TweenParms KeepDisabled(Behaviour p_target)
		{
			if (p_target == null)
			{
				manageBehaviours = false;
				return this;
			}
			return KeepEnabled(new Behaviour[1] { p_target }, false);
		}

		public TweenParms KeepDisabled(GameObject p_target)
		{
			if (p_target == null)
			{
				manageGameObjects = false;
				return this;
			}
			return KeepEnabled(new GameObject[1] { p_target }, false);
		}

		public TweenParms KeepDisabled(Behaviour[] p_targets)
		{
			return KeepEnabled(p_targets, false);
		}

		public TweenParms KeepDisabled(GameObject[] p_targets)
		{
			return KeepEnabled(p_targets, false);
		}

		private TweenParms KeepEnabled(Behaviour[] p_targets, bool p_enabled)
		{
			manageBehaviours = true;
			if (p_enabled)
			{
				managedBehavioursOn = p_targets;
			}
			else
			{
				managedBehavioursOff = p_targets;
			}
			return this;
		}

		private TweenParms KeepEnabled(GameObject[] p_targets, bool p_enabled)
		{
			manageGameObjects = true;
			if (p_enabled)
			{
				managedGameObjectsOn = p_targets;
			}
			else
			{
				managedGameObjectsOff = p_targets;
			}
			return this;
		}

		internal TweenParms IsFrom()
		{
			isFrom = true;
			return this;
		}

		private static bool ValidateValue(object p_val, Type[] p_validVals)
		{
			return Array.IndexOf(p_validVals, p_val.GetType()) != -1;
		}
	}
}
