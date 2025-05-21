using System.Collections.Generic;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween
{
	public class Tweener : ABSTweenComponent
	{
		private float _elapsedDelay;

		internal EaseType _easeType = HOTween.defEaseType;

		internal AnimationCurve _easeAnimationCurve;

		internal float _easeOvershootOrAmplitude = HOTween.defEaseOvershootOrAmplitude;

		internal float _easePeriod = HOTween.defEasePeriod;

		internal bool _pixelPerfect;

		internal bool _speedBased;

		internal float _delay;

		internal float delayCount;

		internal TweenDelegate.TweenCallback onPluginOverwritten;

		internal TweenDelegate.TweenCallbackWParms onPluginOverwrittenWParms;

		internal object[] onPluginOverwrittenParms;

		internal List<ABSTweenPlugin> plugins;

		private object _target;

		private bool isPartialled;

		private EaseType _originalEaseType;

		private PlugVector3Path pv3Path;

		public bool isFrom { get; internal set; }

		public EaseType easeType
		{
			get
			{
				return _easeType;
			}
			set
			{
				_easeType = value;
				int count = plugins.Count;
				for (int i = 0; i < count; i++)
				{
					plugins[i].SetEase(_easeType);
				}
			}
		}

		public AnimationCurve easeAnimationCurve
		{
			get
			{
				return _easeAnimationCurve;
			}
			set
			{
				_easeAnimationCurve = value;
				_easeType = EaseType.AnimationCurve;
				int count = plugins.Count;
				for (int i = 0; i < count; i++)
				{
					plugins[i].SetEase(_easeType);
				}
			}
		}

		public float easeOvershootOrAmplitude
		{
			get
			{
				return _easeOvershootOrAmplitude;
			}
			set
			{
				_easeOvershootOrAmplitude = value;
			}
		}

		public float easePeriod
		{
			get
			{
				return _easePeriod;
			}
			set
			{
				_easePeriod = value;
			}
		}

		public object target
		{
			get
			{
				return _target;
			}
		}

		public bool pixelPerfect
		{
			get
			{
				return _pixelPerfect;
			}
		}

		public bool speedBased
		{
			get
			{
				return _speedBased;
			}
		}

		public float delay
		{
			get
			{
				return _delay;
			}
		}

		public float elapsedDelay
		{
			get
			{
				return _elapsedDelay;
			}
		}

		internal Tweener(object p_target, float p_duration, TweenParms p_parms)
		{
			_target = p_target;
			_duration = p_duration;
			p_parms.InitializeObject(this, _target);
			if (plugins != null && plugins.Count > 0)
			{
				_isEmpty = false;
			}
			SetFullDuration();
		}

		internal override void Kill(bool p_autoRemoveFromHOTween)
		{
			if (!_destroyed)
			{
				if (HOTween.overwriteManager != null)
				{
					HOTween.overwriteManager.RemoveTween(this);
				}
				plugins = null;
				base.Kill(p_autoRemoveFromHOTween);
			}
		}

		public void Play(bool p_skipDelay)
		{
			if (_enabled)
			{
				if (p_skipDelay)
				{
					SkipDelay();
				}
				Play();
			}
		}

		public void PlayForward(bool p_skipDelay)
		{
			if (_enabled)
			{
				if (p_skipDelay)
				{
					SkipDelay();
				}
				PlayForward();
			}
		}

		public override void Rewind()
		{
			Rewind(false);
		}

		public void Rewind(bool p_skipDelay)
		{
			Rewind(false, p_skipDelay);
		}

		public override void Restart()
		{
			Restart(false);
		}

		public void Restart(bool p_skipDelay)
		{
			if (_fullElapsed == 0f)
			{
				PlayForward(p_skipDelay);
			}
			else
			{
				Rewind(true, p_skipDelay);
			}
		}

		internal override void Complete(bool p_autoRemoveFromHOTween)
		{
			if (_enabled && _loops >= 0)
			{
				_fullElapsed = (float.IsPositiveInfinity(_fullDuration) ? _duration : _fullDuration);
				Update(0f, true);
				if (_autoKillOnComplete)
				{
					Kill(p_autoRemoveFromHOTween);
				}
			}
		}

		public void ResetAndChangeParms(TweenType p_tweenType, float p_newDuration, TweenParms p_newParms)
		{
			if (_destroyed)
			{
				TweenWarning.Log("ResetAndChangeParms can't run because the tween was destroyed - set AutoKill or autoKillOnComplete to FALSE if you want to avoid destroying a tween after completion");
				return;
			}
			Reset();
			_duration = p_newDuration;
			if (p_tweenType == TweenType.From)
			{
				p_newParms = p_newParms.IsFrom();
			}
			p_newParms.InitializeObject(this, _target);
			if (plugins != null && plugins.Count > 0)
			{
				_isEmpty = false;
			}
			SetFullDuration();
		}

		protected override void Reset()
		{
			base.Reset();
			isFrom = false;
			plugins = null;
			isPartialled = false;
			pv3Path = null;
			_delay = (_elapsedDelay = (delayCount = 0f));
			_pixelPerfect = false;
			_speedBased = false;
			_easeType = HOTween.defEaseType;
			_easeOvershootOrAmplitude = HOTween.defEaseOvershootOrAmplitude;
			_easePeriod = HOTween.defEasePeriod;
			_originalEaseType = HOTween.defEaseType;
			onPluginOverwritten = null;
			onStepCompleteWParms = null;
			onPluginOverwrittenParms = null;
		}

		protected override void ApplyCallback(bool p_wParms, CallbackType p_callbackType, TweenDelegate.TweenCallback p_callback, TweenDelegate.TweenCallbackWParms p_callbackWParms, params object[] p_callbackParms)
		{
			if (p_callbackType == CallbackType.OnPluginOverwritten)
			{
				onPluginOverwritten = p_callback;
				onPluginOverwrittenWParms = p_callbackWParms;
				onPluginOverwrittenParms = p_callbackParms;
			}
			else
			{
				base.ApplyCallback(p_wParms, p_callbackType, p_callback, p_callbackWParms, p_callbackParms);
			}
		}

		public override bool IsTweening(object p_target)
		{
			if (!_enabled)
			{
				return false;
			}
			if (p_target == _target)
			{
				return !_isPaused;
			}
			return false;
		}

		public override bool IsTweening(string p_id)
		{
			if (!_enabled)
			{
				return false;
			}
			if (base.id == p_id)
			{
				return !_isPaused;
			}
			return false;
		}

		public override bool IsTweening(int p_id)
		{
			if (!_enabled)
			{
				return false;
			}
			if (base.intId == p_id)
			{
				return !_isPaused;
			}
			return false;
		}

		public override bool IsLinkedTo(object p_target)
		{
			return p_target == _target;
		}

		public override List<object> GetTweenTargets()
		{
			List<object> list = new List<object>();
			list.Add(target);
			return list;
		}

		internal override List<IHOTweenComponent> GetTweensById(string p_id)
		{
			List<IHOTweenComponent> list = new List<IHOTweenComponent>();
			if (base.id == p_id)
			{
				list.Add(this);
			}
			return list;
		}

		internal override List<IHOTweenComponent> GetTweensByIntId(int p_intId)
		{
			List<IHOTweenComponent> list = new List<IHOTweenComponent>();
			if (base.intId == p_intId)
			{
				list.Add(this);
			}
			return list;
		}

		public Vector3 GetPointOnPath(float t)
		{
			PlugVector3Path plugVector3PathPlugin = GetPlugVector3PathPlugin();
			if (plugVector3PathPlugin == null)
			{
				return Vector3.zero;
			}
			Startup();
			return plugVector3PathPlugin.GetConstPointOnPath(t);
		}

		public Tweener UsePartialPath(int p_waypointId0, int p_waypointId1)
		{
			EaseType p_newEaseType = ((!isPartialled) ? _easeType : _originalEaseType);
			return UsePartialPath(p_waypointId0, p_waypointId1, -1f, p_newEaseType);
		}

		public Tweener UsePartialPath(int p_waypointId0, int p_waypointId1, float p_newDuration)
		{
			EaseType p_newEaseType = (isPartialled ? _originalEaseType : _easeType);
			return UsePartialPath(p_waypointId0, p_waypointId1, p_newDuration, p_newEaseType);
		}

		public Tweener UsePartialPath(int p_waypointId0, int p_waypointId1, EaseType p_newEaseType)
		{
			return UsePartialPath(p_waypointId0, p_waypointId1, -1f, p_newEaseType);
		}

		public Tweener UsePartialPath(int p_waypointId0, int p_waypointId1, float p_newDuration, EaseType p_newEaseType)
		{
			if (plugins.Count > 1)
			{
				TweenWarning.Log(string.Concat("Applying a partial path on a Tweener (", _target, ") with more than one plugin/property being tweened is not allowed"));
				return this;
			}
			if (pv3Path == null)
			{
				pv3Path = GetPlugVector3PathPlugin();
				if (pv3Path == null)
				{
					TweenWarning.Log(string.Concat("Tweener for ", _target, " contains no PlugVector3Path plugin"));
					return this;
				}
			}
			Startup();
			if (!isPartialled)
			{
				isPartialled = true;
				_originalDuration = _duration;
				_originalEaseType = _easeType;
			}
			int num = ConvertWaypointIdToPathId(pv3Path, p_waypointId0, true);
			int p_pathWaypointId = ConvertWaypointIdToPathId(pv3Path, p_waypointId1, false);
			float waypointsLengthPercentage = pv3Path.GetWaypointsLengthPercentage(num, p_pathWaypointId);
			float p_partialStartPerc = ((num == 0) ? 0f : pv3Path.GetWaypointsLengthPercentage(0, num));
			_duration = ((p_newDuration >= 0f) ? p_newDuration : (_originalDuration * waypointsLengthPercentage));
			_easeType = p_newEaseType;
			pv3Path.SwitchToPartialPath(_duration, p_newEaseType, p_partialStartPerc, waypointsLengthPercentage);
			Startup(true);
			if (!_isPaused)
			{
				Restart(true);
			}
			else
			{
				Rewind(true, true);
			}
			return this;
		}

		public void ResetPath()
		{
			isPartialled = false;
			_duration = (speedBased ? _originalNonSpeedBasedDuration : _originalDuration);
			_easeType = _originalEaseType;
			pv3Path.ResetToFullPath(_duration, _easeType);
			Startup(true);
			if (!_isPaused)
			{
				Restart(true);
			}
			else
			{
				Rewind(true);
			}
		}

		private PlugVector3Path GetPlugVector3PathPlugin()
		{
			if (plugins == null)
			{
				return null;
			}
			int count = plugins.Count;
			for (int i = 0; i < count; i++)
			{
				ABSTweenPlugin aBSTweenPlugin = plugins[i];
				PlugVector3Path plugVector3Path = aBSTweenPlugin as PlugVector3Path;
				if (plugVector3Path != null)
				{
					return plugVector3Path;
				}
			}
			return null;
		}

		internal override bool Update(float p_shortElapsed, bool p_forceUpdate, bool p_isStartupIteration, bool p_ignoreCallbacks)
		{
			return Update(p_shortElapsed, p_forceUpdate, p_isStartupIteration, p_ignoreCallbacks);
		}

		internal bool Update(float p_shortElapsed, bool p_forceUpdate, bool p_isStartupIteration, bool p_ignoreCallbacks, bool p_ignoreDelay = false)
		{
			if (_destroyed)
			{
				return true;
			}
			if (_target == null || _target.Equals(null))
			{
				Kill(false);
				return true;
			}
			if (!_enabled)
			{
				return false;
			}
			if (_isComplete && !_isReversed && !p_forceUpdate)
			{
				return true;
			}
			if (_fullElapsed == 0f && _isReversed && !p_forceUpdate)
			{
				return false;
			}
			if (_isPaused && !p_forceUpdate)
			{
				return false;
			}
			ignoreCallbacks = p_isStartupIteration || p_ignoreCallbacks;
			if (p_ignoreDelay || delayCount == 0f)
			{
				Startup();
				if (!_hasStarted)
				{
					OnStart();
				}
				if (!_isReversed)
				{
					_fullElapsed += p_shortElapsed;
					_elapsed += p_shortElapsed;
				}
				else
				{
					_fullElapsed -= p_shortElapsed;
					_elapsed -= p_shortElapsed;
				}
				if (_fullElapsed > _fullDuration)
				{
					_fullElapsed = _fullDuration;
				}
				else if (_fullElapsed < 0f)
				{
					_fullElapsed = 0f;
				}
			}
			else
			{
				if (_timeScale != 0f)
				{
					_elapsedDelay += p_shortElapsed / _timeScale;
				}
				if (_elapsedDelay < delayCount)
				{
					return false;
				}
				if (_isReversed)
				{
					_fullElapsed = (_elapsed = 0f);
				}
				else
				{
					_fullElapsed = (_elapsed = _elapsedDelay - delayCount);
					if (_fullElapsed > _fullDuration)
					{
						_fullElapsed = _fullDuration;
					}
				}
				_elapsedDelay = delayCount;
				delayCount = 0f;
				Startup();
				if (!_hasStarted)
				{
					OnStart();
				}
			}
			bool flag = _isComplete;
			bool flag2 = !_isReversed && !flag && _elapsed >= _duration;
			SetLoops();
			SetElapsed();
			_isComplete = !_isReversed && _loops >= 0 && _completedLoops >= _loops;
			bool flag3 = !flag && _isComplete;
			float p_totElapsed = ((!_isLoopingBack) ? _elapsed : (_duration - _elapsed));
			int count = plugins.Count;
			for (int i = 0; i < count; i++)
			{
				ABSTweenPlugin aBSTweenPlugin = plugins[i];
				if ((!_isLoopingBack && aBSTweenPlugin.easeReversed) || (_isLoopingBack && _loopType == LoopType.YoyoInverse && !aBSTweenPlugin.easeReversed))
				{
					aBSTweenPlugin.ReverseEase();
				}
				if (_duration > 0f)
				{
					aBSTweenPlugin.Update(p_totElapsed);
					OnPluginUpdated(aBSTweenPlugin);
					continue;
				}
				OnPluginUpdated(aBSTweenPlugin);
				aBSTweenPlugin.Complete();
				if (!flag)
				{
					flag3 = true;
				}
			}
			if (_fullElapsed != prevFullElapsed)
			{
				OnUpdate();
				if (_fullElapsed == 0f)
				{
					if (!_isPaused)
					{
						_isPaused = true;
						OnPause();
					}
					OnRewinded();
				}
			}
			if (flag3)
			{
				if (!_isPaused)
				{
					_isPaused = true;
					OnPause();
				}
				OnComplete();
			}
			else if (flag2)
			{
				OnStepComplete();
			}
			ignoreCallbacks = false;
			prevFullElapsed = _fullElapsed;
			return flag3;
		}

		internal override void SetIncremental(int p_diffIncr)
		{
			if (plugins != null)
			{
				int count = plugins.Count;
				for (int i = 0; i < count; i++)
				{
					plugins[i].ForceSetIncremental(p_diffIncr);
				}
			}
		}

		internal void ForceSetSpeedBasedDuration()
		{
			if (!_speedBased || plugins == null)
			{
				return;
			}
			int count = plugins.Count;
			for (int i = 0; i < count; i++)
			{
				plugins[i].ForceSetSpeedBasedDuration();
			}
			_duration = 0f;
			for (int j = 0; j < count; j++)
			{
				ABSTweenPlugin aBSTweenPlugin = plugins[j];
				if (aBSTweenPlugin.duration > _duration)
				{
					_duration = aBSTweenPlugin.duration;
				}
			}
			SetFullDuration();
		}

		protected override bool GoTo(float p_time, bool p_play, bool p_forceUpdate, bool p_ignoreCallbacks)
		{
			if (!_enabled)
			{
				return false;
			}
			if (p_time > _fullDuration)
			{
				p_time = _fullDuration;
			}
			else if (p_time < 0f)
			{
				p_time = 0f;
			}
			if (!p_forceUpdate && _fullElapsed == p_time)
			{
				if (!_isComplete && p_play)
				{
					Play();
				}
				return _isComplete;
			}
			_fullElapsed = p_time;
			delayCount = 0f;
			_elapsedDelay = _delay;
			Update(0f, true, false, p_ignoreCallbacks);
			if (!_isComplete && p_play)
			{
				Play();
			}
			return _isComplete;
		}

		private void Rewind(bool p_play, bool p_skipDelay)
		{
			if (!_enabled)
			{
				return;
			}
			Startup();
			if (!_hasStarted)
			{
				OnStart();
			}
			_isComplete = false;
			_isLoopingBack = false;
			delayCount = (p_skipDelay ? 0f : _delay);
			_elapsedDelay = (p_skipDelay ? _delay : 0f);
			_completedLoops = 0;
			_fullElapsed = (_elapsed = 0f);
			int count = plugins.Count;
			for (int i = 0; i < count; i++)
			{
				ABSTweenPlugin aBSTweenPlugin = plugins[i];
				if (aBSTweenPlugin.easeReversed)
				{
					aBSTweenPlugin.ReverseEase();
				}
				aBSTweenPlugin.Rewind();
			}
			if (_fullElapsed != prevFullElapsed)
			{
				OnUpdate();
				if (_fullElapsed == 0f)
				{
					OnRewinded();
				}
			}
			prevFullElapsed = _fullElapsed;
			if (p_play)
			{
				Play();
			}
			else
			{
				Pause();
			}
		}

		private void SkipDelay()
		{
			if (delayCount > 0f)
			{
				delayCount = 0f;
				_elapsedDelay = _delay;
				_elapsed = (_fullElapsed = 0f);
			}
		}

		protected override void Startup()
		{
			Startup(false);
		}

		private void Startup(bool p_force)
		{
			if (!p_force && startupDone)
			{
				return;
			}
			int count = plugins.Count;
			for (int i = 0; i < count; i++)
			{
				ABSTweenPlugin aBSTweenPlugin = plugins[i];
				if (!aBSTweenPlugin.wasStarted)
				{
					aBSTweenPlugin.Startup();
				}
			}
			if (_speedBased)
			{
				_originalNonSpeedBasedDuration = _duration;
				_duration = 0f;
				for (int j = 0; j < count; j++)
				{
					ABSTweenPlugin aBSTweenPlugin2 = plugins[j];
					if (aBSTweenPlugin2.duration > _duration)
					{
						_duration = aBSTweenPlugin2.duration;
					}
				}
				SetFullDuration();
			}
			else if (p_force)
			{
				SetFullDuration();
			}
			base.Startup();
		}

		protected override void OnStart()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				if (HOTween.overwriteManager != null)
				{
					HOTween.overwriteManager.AddTween(this);
				}
				base.OnStart();
			}
		}

		internal override void FillPluginsList(List<ABSTweenPlugin> p_plugs)
		{
			if (plugins != null)
			{
				int count = plugins.Count;
				for (int i = 0; i < count; i++)
				{
					p_plugs.Add(plugins[i]);
				}
			}
		}

		private static int ConvertWaypointIdToPathId(PlugVector3Path p_plugVector3Path, int p_waypointId, bool p_isStartingWp)
		{
			if (p_waypointId == -1)
			{
				if (!p_isStartingWp)
				{
					return p_plugVector3Path.path.path.Length - 2;
				}
				return 1;
			}
			if (p_plugVector3Path.hasAdditionalStartingP)
			{
				return p_waypointId + 2;
			}
			return p_waypointId + 1;
		}
	}
}
