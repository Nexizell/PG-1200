using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Core
{
	public abstract class ABSTweenComponent : IHOTweenComponent
	{
		internal string _id = "";

		internal int _intId = -1;

		internal bool _autoKillOnComplete = true;

		internal bool _enabled = true;

		internal float _timeScale = HOTween.defTimeScale;

		internal int _loops = 1;

		internal LoopType _loopType = HOTween.defLoopType;

		internal UpdateType _updateType = HOTween.defUpdateType;

		internal bool _isPaused;

		internal bool ignoreCallbacks;

		internal bool _steadyIgnoreCallbacks;

		internal Sequence contSequence;

		internal bool startupDone;

		internal TweenDelegate.TweenCallback onStart;

		internal TweenDelegate.TweenCallbackWParms onStartWParms;

		internal object[] onStartParms;

		internal TweenDelegate.TweenCallback onUpdate;

		internal TweenDelegate.TweenCallbackWParms onUpdateWParms;

		internal object[] onUpdateParms;

		internal TweenDelegate.TweenCallback onPluginUpdated;

		internal TweenDelegate.TweenCallbackWParms onPluginUpdatedWParms;

		internal object[] onPluginUpdatedParms;

		internal TweenDelegate.TweenCallback onPause;

		internal TweenDelegate.TweenCallbackWParms onPauseWParms;

		internal object[] onPauseParms;

		internal TweenDelegate.TweenCallback onPlay;

		internal TweenDelegate.TweenCallbackWParms onPlayWParms;

		internal object[] onPlayParms;

		internal TweenDelegate.TweenCallback onRewinded;

		internal TweenDelegate.TweenCallbackWParms onRewindedWParms;

		internal object[] onRewindedParms;

		internal TweenDelegate.TweenCallback onStepComplete;

		internal TweenDelegate.TweenCallbackWParms onStepCompleteWParms;

		internal object[] onStepCompleteParms;

		internal TweenDelegate.TweenCallback onComplete;

		internal TweenDelegate.TweenCallbackWParms onCompleteWParms;

		internal object[] onCompleteParms;

		protected int _completedLoops;

		protected float _duration;

		protected float _originalDuration;

		protected float _originalNonSpeedBasedDuration;

		protected float _fullDuration;

		protected float _elapsed;

		protected float _fullElapsed;

		protected bool _destroyed;

		protected bool _isEmpty = true;

		protected bool _isReversed;

		protected bool _isLoopingBack;

		protected bool _hasStarted;

		protected bool _isComplete;

		protected float prevFullElapsed;

		protected int prevCompletedLoops;

		internal bool manageBehaviours;

		internal bool manageGameObjects;

		internal Behaviour[] managedBehavioursOn;

		internal Behaviour[] managedBehavioursOff;

		internal GameObject[] managedGameObjectsOn;

		internal GameObject[] managedGameObjectsOff;

		internal bool[] managedBehavioursOriginalState;

		internal bool[] managedGameObjectsOriginalState;

		internal virtual bool steadyIgnoreCallbacks
		{
			get
			{
				return _steadyIgnoreCallbacks;
			}
			set
			{
				_steadyIgnoreCallbacks = value;
			}
		}

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public int intId
		{
			get
			{
				return _intId;
			}
			set
			{
				_intId = value;
			}
		}

		public bool autoKillOnComplete
		{
			get
			{
				return _autoKillOnComplete;
			}
			set
			{
				_autoKillOnComplete = value;
			}
		}

		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
			}
		}

		public float timeScale
		{
			get
			{
				return _timeScale;
			}
			set
			{
				_timeScale = value;
			}
		}

		public int loops
		{
			get
			{
				return _loops;
			}
			set
			{
				_loops = value;
				SetFullDuration();
			}
		}

		public LoopType loopType
		{
			get
			{
				return _loopType;
			}
			set
			{
				_loopType = value;
			}
		}

		public float position
		{
			get
			{
				if (_loops >= 1)
				{
					return _fullElapsed;
				}
				return _elapsed;
			}
			set
			{
				GoTo(value, !_isPaused);
			}
		}

		public float duration
		{
			get
			{
				return _duration;
			}
		}

		public float fullDuration
		{
			get
			{
				return _fullDuration;
			}
		}

		public float elapsed
		{
			get
			{
				return _elapsed;
			}
		}

		public float fullElapsed
		{
			get
			{
				return _fullElapsed;
			}
		}

		public UpdateType updateType
		{
			get
			{
				return _updateType;
			}
		}

		public int completedLoops
		{
			get
			{
				return _completedLoops;
			}
		}

		public bool destroyed
		{
			get
			{
				return _destroyed;
			}
		}

		public bool isEmpty
		{
			get
			{
				return _isEmpty;
			}
		}

		public bool isReversed
		{
			get
			{
				return _isReversed;
			}
		}

		public bool isLoopingBack
		{
			get
			{
				return _isLoopingBack;
			}
		}

		public bool isPaused
		{
			get
			{
				return _isPaused;
			}
		}

		public bool hasStarted
		{
			get
			{
				return _hasStarted;
			}
		}

		public bool isComplete
		{
			get
			{
				return _isComplete;
			}
		}

		public bool isSequenced
		{
			get
			{
				return contSequence != null;
			}
		}

		public void Kill()
		{
			Kill(true);
		}

		internal virtual void Kill(bool p_autoRemoveFromHOTween)
		{
			if (!_destroyed)
			{
				_destroyed = (_isEmpty = true);
				if (p_autoRemoveFromHOTween)
				{
					HOTween.Kill(this);
				}
			}
		}

		public void Play()
		{
			if (_enabled)
			{
				PlayIfPaused();
			}
		}

		private void PlayIfPaused()
		{
			if (_isPaused && ((!_isReversed && !_isComplete) || (_isReversed && _fullElapsed > 0f)))
			{
				_isPaused = false;
				OnPlay();
			}
		}

		public void PlayForward()
		{
			if (_enabled)
			{
				_isReversed = false;
				PlayIfPaused();
			}
		}

		public void PlayBackwards()
		{
			if (_enabled)
			{
				_isReversed = true;
				PlayIfPaused();
			}
		}

		public void Pause()
		{
			if (_enabled && !_isPaused)
			{
				_isPaused = true;
				OnPause();
			}
		}

		public abstract void Rewind();

		public abstract void Restart();

		public void Reverse(bool p_forcePlay = false)
		{
			if (_enabled)
			{
				_isReversed = !_isReversed;
				if (p_forcePlay)
				{
					Play();
				}
			}
		}

		public void Complete()
		{
			Complete(true);
		}

		public bool GoTo(float p_time)
		{
			return GoTo(p_time, false, false, false);
		}

		public bool GoTo(float p_time, bool p_forceUpdate)
		{
			return GoTo(p_time, false, p_forceUpdate, false);
		}

		internal bool GoTo(float p_time, bool p_forceUpdate, bool p_ignoreCallbacks)
		{
			return GoTo(p_time, false, p_forceUpdate, p_ignoreCallbacks);
		}

		public bool GoToAndPlay(float p_time)
		{
			return GoTo(p_time, true, false, false);
		}

		public bool GoToAndPlay(float p_time, bool p_forceUpdate)
		{
			return GoTo(p_time, true, p_forceUpdate, false);
		}

		internal bool GoToAndPlay(float p_time, bool p_forceUpdate, bool p_ignoreCallbacks)
		{
			return GoTo(p_time, true, p_forceUpdate, p_ignoreCallbacks);
		}

		public IEnumerator WaitForCompletion()
		{
			while (!_isComplete)
			{
				yield return 0;
			}
		}

		public IEnumerator WaitForRewind()
		{
			while (_fullElapsed > 0f)
			{
				yield return 0;
			}
		}

		protected virtual void Reset()
		{
			_id = "";
			_intId = -1;
			_autoKillOnComplete = true;
			_enabled = true;
			_timeScale = HOTween.defTimeScale;
			_loops = 1;
			_loopType = HOTween.defLoopType;
			_updateType = HOTween.defUpdateType;
			_isPaused = false;
			_completedLoops = 0;
			_duration = (_originalDuration = (_originalNonSpeedBasedDuration = (_fullDuration = 0f)));
			_elapsed = (_fullElapsed = 0f);
			_isEmpty = true;
			_isReversed = (_isLoopingBack = (_hasStarted = (_isComplete = false)));
			startupDone = false;
			onStart = null;
			onStartWParms = null;
			onStartParms = null;
			onUpdate = null;
			onUpdateWParms = null;
			onUpdateParms = null;
			onPluginUpdated = null;
			onPluginUpdatedWParms = null;
			onPluginUpdatedParms = null;
			onStepComplete = null;
			onStepCompleteWParms = null;
			onStepCompleteParms = null;
			onComplete = null;
			onCompleteWParms = null;
			onCompleteParms = null;
			onPause = null;
			onPauseWParms = null;
			onPauseParms = null;
			onPlay = null;
			onPlayWParms = null;
			onPlayParms = null;
			onRewinded = null;
			onRewindedWParms = null;
			onRewindedParms = null;
			manageBehaviours = false;
			managedBehavioursOff = null;
			managedBehavioursOn = null;
			managedBehavioursOriginalState = null;
			manageGameObjects = false;
			managedGameObjectsOff = null;
			managedGameObjectsOn = null;
			managedGameObjectsOriginalState = null;
		}

		public void ApplyCallback(CallbackType p_callbackType, TweenDelegate.TweenCallback p_callback)
		{
			ApplyCallback(false, p_callbackType, p_callback, null, null);
		}

		public void ApplyCallback(CallbackType p_callbackType, TweenDelegate.TweenCallbackWParms p_callback, params object[] p_callbackParms)
		{
			ApplyCallback(true, p_callbackType, null, p_callback, p_callbackParms);
		}

		public void ApplyCallback(CallbackType p_callbackType, GameObject p_sendMessageTarget, string p_methodName, object p_value, SendMessageOptions p_options = SendMessageOptions.RequireReceiver)
		{
			TweenDelegate.TweenCallbackWParms p_callbackWParms = HOTween.DoSendMessage;
			object[] p_callbackParms = new object[4] { p_sendMessageTarget, p_methodName, p_value, p_options };
			ApplyCallback(true, p_callbackType, null, p_callbackWParms, p_callbackParms);
		}

		protected virtual void ApplyCallback(bool p_wParms, CallbackType p_callbackType, TweenDelegate.TweenCallback p_callback, TweenDelegate.TweenCallbackWParms p_callbackWParms, params object[] p_callbackParms)
		{
			switch (p_callbackType)
			{
			case CallbackType.OnStart:
				onStart = p_callback;
				onStartWParms = p_callbackWParms;
				onStartParms = p_callbackParms;
				break;
			case CallbackType.OnUpdate:
				onUpdate = p_callback;
				onUpdateWParms = p_callbackWParms;
				onUpdateParms = p_callbackParms;
				break;
			case CallbackType.OnStepComplete:
				onStepComplete = p_callback;
				onStepCompleteWParms = p_callbackWParms;
				onStepCompleteParms = p_callbackParms;
				break;
			case CallbackType.OnComplete:
				onComplete = p_callback;
				onCompleteWParms = p_callbackWParms;
				onCompleteParms = p_callbackParms;
				break;
			case CallbackType.OnPlay:
				onPlay = p_callback;
				onPlayWParms = p_callbackWParms;
				onPlayParms = p_callbackParms;
				break;
			case CallbackType.OnPause:
				onPause = p_callback;
				onPauseWParms = p_callbackWParms;
				onPauseParms = p_callbackParms;
				break;
			case CallbackType.OnRewinded:
				onRewinded = p_callback;
				onRewindedWParms = p_callbackWParms;
				onRewindedParms = p_callbackParms;
				break;
			case CallbackType.OnPluginOverwritten:
				TweenWarning.Log("ApplyCallback > OnPluginOverwritten type is available only with Tweeners and not with Sequences");
				break;
			}
		}

		public abstract bool IsTweening(object p_target);

		public abstract bool IsTweening(string p_id);

		public abstract bool IsTweening(int p_id);

		public abstract bool IsLinkedTo(object p_target);

		public abstract List<object> GetTweenTargets();

		internal abstract List<IHOTweenComponent> GetTweensById(string p_id);

		internal abstract List<IHOTweenComponent> GetTweensByIntId(int p_intId);

		internal abstract void Complete(bool p_doAutoKill);

		internal bool Update(float p_elapsed)
		{
			return Update(p_elapsed, false, false, false);
		}

		internal bool Update(float p_elapsed, bool p_forceUpdate)
		{
			return Update(p_elapsed, p_forceUpdate, false, false);
		}

		internal bool Update(float p_elapsed, bool p_forceUpdate, bool p_isStartupIteration)
		{
			return Update(p_elapsed, p_forceUpdate, p_isStartupIteration, false);
		}

		internal abstract bool Update(float p_elapsed, bool p_forceUpdate, bool p_isStartupIteration, bool p_ignoreCallbacks);

		internal abstract void SetIncremental(int p_diffIncr);

		protected abstract bool GoTo(float p_time, bool p_play, bool p_forceUpdate, bool p_ignoreCallbacks);

		protected virtual void Startup()
		{
			startupDone = true;
		}

		protected virtual void OnStart()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				_hasStarted = true;
				if (onStart != null)
				{
					onStart();
				}
				else if (onStartWParms != null)
				{
					onStartWParms(new TweenEvent(this, onStartParms));
				}
				OnPlay();
			}
		}

		protected void OnUpdate()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				if (onUpdate != null)
				{
					onUpdate();
				}
				else if (onUpdateWParms != null)
				{
					onUpdateWParms(new TweenEvent(this, onUpdateParms));
				}
			}
		}

		protected void OnPluginUpdated(ABSTweenPlugin p_plugin)
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				if (onPluginUpdated != null)
				{
					onPluginUpdated();
				}
				else if (onPluginUpdatedWParms != null)
				{
					onPluginUpdatedWParms(new TweenEvent(this, onPluginUpdatedParms, p_plugin));
				}
			}
		}

		protected void OnPause()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				ManageObjects(false);
				if (onPause != null)
				{
					onPause();
				}
				else if (onPauseWParms != null)
				{
					onPauseWParms(new TweenEvent(this, onPauseParms));
				}
			}
		}

		protected void OnPlay()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				ManageObjects(true);
				if (onPlay != null)
				{
					onPlay();
				}
				else if (onPlayWParms != null)
				{
					onPlayWParms(new TweenEvent(this, onPlayParms));
				}
			}
		}

		protected void OnRewinded()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				if (onRewinded != null)
				{
					onRewinded();
				}
				else if (onRewindedWParms != null)
				{
					onRewindedWParms(new TweenEvent(this, onRewindedParms));
				}
			}
		}

		protected void OnStepComplete()
		{
			if (!steadyIgnoreCallbacks && !ignoreCallbacks)
			{
				if (onStepComplete != null)
				{
					onStepComplete();
				}
				else if (onStepCompleteWParms != null)
				{
					onStepCompleteWParms(new TweenEvent(this, onStepCompleteParms));
				}
			}
		}

		protected void OnComplete()
		{
			_isComplete = true;
			OnStepComplete();
			if (!steadyIgnoreCallbacks && !ignoreCallbacks && (onComplete != null || onCompleteWParms != null))
			{
				if (HOTween.isUpdateLoop)
				{
					HOTween.onCompletes.Add(this);
				}
				else
				{
					OnCompleteDispatch();
				}
			}
		}

		internal void OnCompleteDispatch()
		{
			if (onComplete != null)
			{
				onComplete();
			}
			else if (onCompleteWParms != null)
			{
				onCompleteWParms(new TweenEvent(this, onCompleteParms));
			}
		}

		protected void SetFullDuration()
		{
			_fullDuration = ((_loops < 0) ? float.PositiveInfinity : (_duration * (float)_loops));
		}

		protected void SetElapsed()
		{
			if (_duration == 0f || (_loops >= 0 && _completedLoops >= _loops))
			{
				_elapsed = _duration;
			}
			else if (_fullElapsed < _duration)
			{
				_elapsed = _fullElapsed;
			}
			else
			{
				_elapsed = _fullElapsed % _duration;
			}
		}

		protected void SetLoops()
		{
			if (_duration == 0f)
			{
				_completedLoops = 1;
			}
			else
			{
				float num = _fullElapsed / _duration;
				int num2 = (int)Math.Ceiling(num);
				if ((float)num2 - num < 1E-07f)
				{
					_completedLoops = num2;
				}
				else
				{
					_completedLoops = num2 - 1;
				}
			}
			_isLoopingBack = _loopType != 0 && _loopType != LoopType.Incremental && ((_loops > 0 && ((_completedLoops < _loops && _completedLoops % 2 != 0) || (_completedLoops >= _loops && _completedLoops % 2 == 0))) || (_loops < 0 && _completedLoops % 2 != 0));
		}

		protected void ManageObjects(bool isPlay)
		{
			if (manageBehaviours)
			{
				int num = 0;
				if (managedBehavioursOn != null)
				{
					num = managedBehavioursOn.Length;
					for (int i = 0; i < managedBehavioursOn.Length; i++)
					{
						Behaviour behaviour = managedBehavioursOn[i];
						if (!(behaviour == null))
						{
							if (isPlay)
							{
								managedBehavioursOriginalState[i] = behaviour.enabled;
								behaviour.enabled = true;
							}
							else
							{
								behaviour.enabled = managedBehavioursOriginalState[i];
							}
						}
					}
				}
				if (managedBehavioursOff != null)
				{
					for (int j = 0; j < managedBehavioursOff.Length; j++)
					{
						Behaviour behaviour2 = managedBehavioursOff[j];
						if (!(behaviour2 == null))
						{
							if (isPlay)
							{
								managedBehavioursOriginalState[num + j] = behaviour2.enabled;
								behaviour2.enabled = false;
							}
							else
							{
								behaviour2.enabled = managedBehavioursOriginalState[j + num];
							}
						}
					}
				}
			}
			if (!manageGameObjects)
			{
				return;
			}
			int num2 = 0;
			if (managedGameObjectsOn != null)
			{
				num2 = managedGameObjectsOn.Length;
				for (int k = 0; k < managedGameObjectsOn.Length; k++)
				{
					GameObject gameObject = managedGameObjectsOn[k];
					if (!(gameObject == null))
					{
						if (isPlay)
						{
							managedGameObjectsOriginalState[k] = gameObject.active;
							gameObject.active = true;
						}
						else
						{
							gameObject.active = managedGameObjectsOriginalState[k];
						}
					}
				}
			}
			if (managedGameObjectsOff == null)
			{
				return;
			}
			for (int l = 0; l < managedGameObjectsOff.Length; l++)
			{
				GameObject gameObject2 = managedGameObjectsOff[l];
				if (!(gameObject2 == null))
				{
					if (isPlay)
					{
						managedGameObjectsOriginalState[num2 + l] = gameObject2.active;
						gameObject2.active = false;
					}
					else
					{
						gameObject2.active = managedGameObjectsOriginalState[l + num2];
					}
				}
			}
		}

		internal abstract void FillPluginsList(List<ABSTweenPlugin> p_plugs);
	}
}
