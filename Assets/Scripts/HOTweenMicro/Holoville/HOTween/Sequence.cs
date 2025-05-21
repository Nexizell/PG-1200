using System.Collections.Generic;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween
{
	public class Sequence : ABSTweenComponent
	{
		private enum SeqItemType
		{
			Interval = 0,
			Tween = 1,
			Callback = 2
		}

		private class HOTSeqItem
		{
			public readonly SeqItemType seqItemType;

			public readonly TweenDelegate.TweenCallback callback;

			public readonly TweenDelegate.TweenCallbackWParms callbackWParms;

			public readonly object[] callbackParms;

			public float startTime;

			private readonly float _duration;

			public readonly ABSTweenComponent twMember;

			public float duration
			{
				get
				{
					if (twMember == null)
					{
						return _duration;
					}
					return twMember.duration;
				}
			}

			public HOTSeqItem(float p_startTime, ABSTweenComponent p_twMember)
			{
				startTime = p_startTime;
				twMember = p_twMember;
				twMember.autoKillOnComplete = false;
				seqItemType = SeqItemType.Tween;
			}

			public HOTSeqItem(float p_startTime, float p_duration)
			{
				seqItemType = SeqItemType.Interval;
				startTime = p_startTime;
				_duration = p_duration;
			}

			public HOTSeqItem(float p_startTime, TweenDelegate.TweenCallback p_callback, TweenDelegate.TweenCallbackWParms p_callbackWParms, params object[] p_callbackParms)
			{
				seqItemType = SeqItemType.Callback;
				startTime = p_startTime;
				callback = p_callback;
				callbackWParms = p_callbackWParms;
				callbackParms = p_callbackParms;
			}
		}

		private bool hasCallbacks;

		private int prevIncrementalCompletedLoops;

		private float prevElapsed;

		private List<HOTSeqItem> items;

		internal override bool steadyIgnoreCallbacks
		{
			get
			{
				return _steadyIgnoreCallbacks;
			}
			set
			{
				_steadyIgnoreCallbacks = value;
				if (items == null)
				{
					return;
				}
				int count = items.Count;
				for (int i = 0; i < count; i++)
				{
					HOTSeqItem hOTSeqItem = items[i];
					if (hOTSeqItem.twMember != null)
					{
						hOTSeqItem.twMember.steadyIgnoreCallbacks = value;
					}
				}
			}
		}

		public Sequence()
			: this(null)
		{
		}

		public Sequence(SequenceParms p_parms)
		{
			if (p_parms != null)
			{
				p_parms.InitializeSequence(this);
			}
			_isPaused = true;
			HOTween.AddSequence(this);
		}

		public void AppendCallback(TweenDelegate.TweenCallback p_callback)
		{
			InsertCallback(_duration, p_callback);
		}

		public void AppendCallback(TweenDelegate.TweenCallbackWParms p_callback, params object[] p_callbackParms)
		{
			InsertCallback(_duration, p_callback, p_callbackParms);
		}

		public void AppendCallback(GameObject p_sendMessageTarget, string p_methodName, object p_value, SendMessageOptions p_options = SendMessageOptions.RequireReceiver)
		{
			InsertCallback(_duration, p_sendMessageTarget, p_methodName, p_value, p_options);
		}

		public void InsertCallback(float p_time, TweenDelegate.TweenCallback p_callback)
		{
			InsertCallback(p_time, p_callback, null, null);
		}

		public void InsertCallback(float p_time, TweenDelegate.TweenCallbackWParms p_callback, params object[] p_callbackParms)
		{
			InsertCallback(p_time, null, p_callback, p_callbackParms);
		}

		public void InsertCallback(float p_time, GameObject p_sendMessageTarget, string p_methodName, object p_value, SendMessageOptions p_options = SendMessageOptions.RequireReceiver)
		{
			TweenDelegate.TweenCallbackWParms p_callbackWParms = HOTween.DoSendMessage;
			object[] p_callbackParms = new object[4] { p_sendMessageTarget, p_methodName, p_value, p_options };
			InsertCallback(p_time, null, p_callbackWParms, p_callbackParms);
		}

		private void InsertCallback(float p_time, TweenDelegate.TweenCallback p_callback, TweenDelegate.TweenCallbackWParms p_callbackWParms, params object[] p_callbackParms)
		{
			hasCallbacks = true;
			HOTSeqItem item = new HOTSeqItem(p_time, p_callback, p_callbackWParms, p_callbackParms);
			if (items == null)
			{
				items = new List<HOTSeqItem> { item };
			}
			else
			{
				bool flag = false;
				int count = items.Count;
				for (int i = 0; i < count; i++)
				{
					if (items[i].startTime >= p_time)
					{
						items.Insert(i, item);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					items.Add(item);
				}
			}
			_isEmpty = false;
		}

		public float AppendInterval(float p_duration)
		{
			return Append(null, p_duration);
		}

		public float Append(IHOTweenComponent p_twMember)
		{
			return Append(p_twMember, 0f);
		}

		private float Append(IHOTweenComponent p_twMember, float p_duration)
		{
			if (items == null)
			{
				if (p_twMember == null)
				{
					return Insert(0f, null, p_duration);
				}
				return Insert(0f, p_twMember);
			}
			if (p_twMember != null)
			{
				HOTween.RemoveFromTweens(p_twMember);
				((ABSTweenComponent)p_twMember).contSequence = this;
				CheckSpeedBasedTween(p_twMember);
			}
			HOTSeqItem hOTSeqItem = ((p_twMember != null) ? new HOTSeqItem(_duration, p_twMember as ABSTweenComponent) : new HOTSeqItem(_duration, p_duration));
			items.Add(hOTSeqItem);
			_duration += hOTSeqItem.duration;
			SetFullDuration();
			_isEmpty = false;
			return _duration;
		}

		public float PrependInterval(float p_duration)
		{
			return Prepend(null, p_duration);
		}

		public float Prepend(IHOTweenComponent p_twMember)
		{
			return Prepend(p_twMember, 0f);
		}

		private float Prepend(IHOTweenComponent p_twMember, float p_duration)
		{
			if (items == null)
			{
				return Insert(0f, p_twMember);
			}
			if (p_twMember != null)
			{
				HOTween.RemoveFromTweens(p_twMember);
				((ABSTweenComponent)p_twMember).contSequence = this;
				CheckSpeedBasedTween(p_twMember);
			}
			HOTSeqItem hOTSeqItem = ((p_twMember != null) ? new HOTSeqItem(0f, p_twMember as ABSTweenComponent) : new HOTSeqItem(0f, p_duration));
			float num = hOTSeqItem.duration;
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				items[i].startTime += num;
			}
			items.Insert(0, hOTSeqItem);
			_duration += num;
			SetFullDuration();
			_isEmpty = false;
			return _duration;
		}

		public float Insert(float p_time, IHOTweenComponent p_twMember)
		{
			return Insert(p_time, p_twMember, 0f);
		}

		private float Insert(float p_time, IHOTweenComponent p_twMember, float p_duration)
		{
			if (p_twMember != null)
			{
				HOTween.RemoveFromTweens(p_twMember);
				((ABSTweenComponent)p_twMember).contSequence = this;
				CheckSpeedBasedTween(p_twMember);
			}
			HOTSeqItem hOTSeqItem = ((p_twMember != null) ? new HOTSeqItem(p_time, p_twMember as ABSTweenComponent) : new HOTSeqItem(p_time, p_duration));
			if (items == null)
			{
				items = new List<HOTSeqItem> { hOTSeqItem };
				_duration = hOTSeqItem.startTime + hOTSeqItem.duration;
				SetFullDuration();
				_isEmpty = false;
				return _duration;
			}
			bool flag = false;
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				if (items[i].startTime >= p_time)
				{
					items.Insert(i, hOTSeqItem);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				items.Add(hOTSeqItem);
			}
			_duration = Mathf.Max(hOTSeqItem.startTime + hOTSeqItem.duration, _duration);
			SetFullDuration();
			_isEmpty = false;
			return _duration;
		}

		public void Clear(SequenceParms p_parms = null)
		{
			Kill(false);
			Reset();
			hasCallbacks = false;
			prevIncrementalCompletedLoops = (prevIncrementalCompletedLoops = 0);
			_destroyed = false;
			if (p_parms != null)
			{
				p_parms.InitializeSequence(this);
			}
			_isPaused = true;
		}

		internal override void Kill(bool p_autoRemoveFromHOTween)
		{
			if (_destroyed)
			{
				return;
			}
			if (items != null)
			{
				int count = items.Count;
				for (int i = 0; i < count; i++)
				{
					HOTSeqItem hOTSeqItem = items[i];
					if (hOTSeqItem.seqItemType == SeqItemType.Tween)
					{
						hOTSeqItem.twMember.Kill(false);
					}
				}
				items = null;
			}
			base.Kill(p_autoRemoveFromHOTween);
		}

		public override void Rewind()
		{
			Rewind(false);
		}

		public override void Restart()
		{
			if (_fullElapsed == 0f)
			{
				PlayForward();
			}
			else
			{
				Rewind(true);
			}
		}

		public override bool IsTweening(object p_target)
		{
			if (!_enabled || items == null)
			{
				return false;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.twMember.IsTweening(p_target))
				{
					return true;
				}
			}
			return false;
		}

		public override bool IsTweening(string p_id)
		{
			if (!_enabled || items == null)
			{
				return false;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.twMember.IsTweening(p_id))
				{
					return true;
				}
			}
			return false;
		}

		public override bool IsTweening(int p_id)
		{
			if (!_enabled || items == null)
			{
				return false;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.twMember.IsTweening(p_id))
				{
					return true;
				}
			}
			return false;
		}

		public override bool IsLinkedTo(object p_target)
		{
			if (items == null)
			{
				return false;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.twMember.IsLinkedTo(p_target))
				{
					return true;
				}
			}
			return false;
		}

		public override List<object> GetTweenTargets()
		{
			if (items == null)
			{
				return null;
			}
			List<object> list = new List<object>();
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					list.AddRange(hOTSeqItem.twMember.GetTweenTargets());
				}
			}
			return list;
		}

		public List<Tweener> GetTweenersByTarget(object p_target)
		{
			List<Tweener> list = new List<Tweener>();
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType != SeqItemType.Tween)
				{
					continue;
				}
				Tweener tweener = hOTSeqItem.twMember as Tweener;
				if (tweener != null)
				{
					if (tweener.target == p_target)
					{
						list.Add(tweener);
					}
				}
				else
				{
					list.AddRange(((Sequence)hOTSeqItem.twMember).GetTweenersByTarget(p_target));
				}
			}
			return list;
		}

		internal override List<IHOTweenComponent> GetTweensById(string p_id)
		{
			List<IHOTweenComponent> list = new List<IHOTweenComponent>();
			if (base.id == p_id)
			{
				list.Add(this);
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					list.AddRange(hOTSeqItem.twMember.GetTweensById(p_id));
				}
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
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					list.AddRange(hOTSeqItem.twMember.GetTweensByIntId(p_intId));
				}
			}
			return list;
		}

		internal void Remove(ABSTweenComponent p_tween)
		{
			if (items == null)
			{
				return;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.twMember == p_tween)
				{
					items.RemoveAt(i);
					break;
				}
			}
			if (items.Count == 0)
			{
				if (base.isSequenced)
				{
					contSequence.Remove(this);
				}
				Kill(!base.isSequenced);
			}
		}

		internal override void Complete(bool p_autoRemoveFromHOTween)
		{
			if (_enabled && items != null && _loops >= 0)
			{
				_fullElapsed = _fullDuration;
				Update(0f, true);
				if (_autoKillOnComplete)
				{
					Kill(p_autoRemoveFromHOTween);
				}
			}
		}

		internal override bool Update(float p_shortElapsed, bool p_forceUpdate, bool p_isStartupIteration, bool p_ignoreCallbacks)
		{
			if (_destroyed)
			{
				return true;
			}
			if (items == null)
			{
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
			Startup();
			if (!_hasStarted)
			{
				OnStart();
			}
			bool flag = _isComplete;
			bool flag2 = !_isReversed && !flag && _elapsed >= _duration;
			SetLoops();
			SetElapsed();
			_isComplete = !_isReversed && _loops >= 0 && _completedLoops >= _loops;
			bool flag3 = !flag && _isComplete;
			if (_loopType == LoopType.Incremental)
			{
				if (prevIncrementalCompletedLoops != _completedLoops)
				{
					int num = _completedLoops;
					if (_loops != -1 && num >= _loops)
					{
						num--;
					}
					int num2 = num - prevIncrementalCompletedLoops;
					if (num2 != 0)
					{
						SetIncremental(num2);
						prevIncrementalCompletedLoops = num;
					}
				}
			}
			else if (prevIncrementalCompletedLoops != 0)
			{
				SetIncremental(-prevIncrementalCompletedLoops);
				prevIncrementalCompletedLoops = 0;
			}
			int count = items.Count;
			if (hasCallbacks && !_isPaused)
			{
				List<HOTSeqItem> list = null;
				for (int i = 0; i < count; i++)
				{
					HOTSeqItem hOTSeqItem = items[i];
					if (hOTSeqItem.seqItemType != SeqItemType.Callback)
					{
						continue;
					}
					bool flag4 = prevCompletedLoops != _completedLoops;
					bool flag5 = (_loopType == LoopType.Yoyo || _loopType == LoopType.YoyoInverse) && ((_isLoopingBack && !flag4) || (flag4 && !_isLoopingBack));
					float num3 = (_isLoopingBack ? (_duration - _elapsed) : _elapsed);
					float num4 = (_isLoopingBack ? (_duration - prevElapsed) : prevElapsed);
					if ((!_isLoopingBack) ? ((!flag5 && (hOTSeqItem.startTime <= num3 || _completedLoops != prevCompletedLoops) && hOTSeqItem.startTime >= num4) || (hOTSeqItem.startTime <= num3 && ((!_isComplete && _completedLoops != prevCompletedLoops) || hOTSeqItem.startTime >= num4))) : ((flag5 && (hOTSeqItem.startTime >= num3 || _completedLoops != prevCompletedLoops) && hOTSeqItem.startTime <= num4) || (hOTSeqItem.startTime >= num3 && ((!_isComplete && _completedLoops != prevCompletedLoops) || hOTSeqItem.startTime <= num4))))
					{
						if (list == null)
						{
							list = new List<HOTSeqItem>();
						}
						if (hOTSeqItem.startTime > num3)
						{
							list.Insert(0, hOTSeqItem);
						}
						else
						{
							list.Add(hOTSeqItem);
						}
					}
				}
				if (list != null)
				{
					foreach (HOTSeqItem item in list)
					{
						if (item.callback != null)
						{
							item.callback();
						}
						else if (item.callbackWParms != null)
						{
							item.callbackWParms(new TweenEvent(this, item.callbackParms));
						}
					}
				}
			}
			if (_duration > 0f)
			{
				float num5 = ((!_isLoopingBack) ? _elapsed : (_duration - _elapsed));
				for (int num6 = count - 1; num6 > -1; num6--)
				{
					HOTSeqItem hOTSeqItem = items[num6];
					if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.startTime > num5)
					{
						if (hOTSeqItem.twMember.duration > 0f)
						{
							hOTSeqItem.twMember.GoTo(num5 - hOTSeqItem.startTime, p_forceUpdate, true);
						}
						else
						{
							hOTSeqItem.twMember.Rewind();
						}
					}
				}
				for (int j = 0; j < count; j++)
				{
					HOTSeqItem hOTSeqItem = items[j];
					if (hOTSeqItem.seqItemType == SeqItemType.Tween && hOTSeqItem.startTime <= num5)
					{
						if (hOTSeqItem.twMember.duration > 0f)
						{
							hOTSeqItem.twMember.GoTo(num5 - hOTSeqItem.startTime, p_forceUpdate);
						}
						else
						{
							hOTSeqItem.twMember.Complete();
						}
					}
				}
			}
			else
			{
				for (int num7 = count - 1; num7 > -1; num7--)
				{
					HOTSeqItem hOTSeqItem = items[num7];
					if (hOTSeqItem.seqItemType == SeqItemType.Tween)
					{
						hOTSeqItem.twMember.Complete();
					}
				}
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
			prevElapsed = _elapsed;
			prevFullElapsed = _fullElapsed;
			prevCompletedLoops = _completedLoops;
			return flag3;
		}

		internal override void SetIncremental(int p_diffIncr)
		{
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					hOTSeqItem.twMember.SetIncremental(p_diffIncr);
				}
			}
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
			if (_fullElapsed == p_time && !p_forceUpdate)
			{
				if (!_isComplete && p_play)
				{
					Play();
				}
				return _isComplete;
			}
			_fullElapsed = p_time;
			Update(0f, true, false, p_ignoreCallbacks);
			if (!_isComplete && p_play)
			{
				Play();
			}
			return _isComplete;
		}

		private void Rewind(bool p_play)
		{
			if (!_enabled || items == null)
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
			_completedLoops = 0;
			_fullElapsed = (_elapsed = 0f);
			int num = items.Count - 1;
			for (int num2 = num; num2 > -1; num2--)
			{
				HOTSeqItem hOTSeqItem = items[num2];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					hOTSeqItem.twMember.Rewind();
				}
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

		private void TweenStartupIteration()
		{
			bool flag = !steadyIgnoreCallbacks;
			if (flag)
			{
				steadyIgnoreCallbacks = true;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					hOTSeqItem.twMember.Update(hOTSeqItem.twMember.duration, true, true);
				}
			}
			for (int num = count - 1; num > -1; num--)
			{
				HOTSeqItem hOTSeqItem = items[num];
				if (hOTSeqItem.seqItemType == SeqItemType.Tween)
				{
					hOTSeqItem.twMember.Rewind();
				}
			}
			if (flag)
			{
				steadyIgnoreCallbacks = false;
			}
		}

		private static void CheckSpeedBasedTween(IHOTweenComponent p_twMember)
		{
			Tweener tweener = p_twMember as Tweener;
			if (tweener != null && tweener._speedBased)
			{
				tweener.ForceSetSpeedBasedDuration();
			}
		}

		protected override void Startup()
		{
			if (!startupDone)
			{
				TweenStartupIteration();
				base.Startup();
			}
		}

		internal override void FillPluginsList(List<ABSTweenPlugin> p_plugs)
		{
			if (items == null)
			{
				return;
			}
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				HOTSeqItem hOTSeqItem = items[i];
				if (hOTSeqItem.twMember != null)
				{
					Sequence sequence = hOTSeqItem.twMember as Sequence;
					if (sequence != null)
					{
						sequence.FillPluginsList(p_plugs);
					}
					else
					{
						hOTSeqItem.twMember.FillPluginsList(p_plugs);
					}
				}
			}
		}
	}
}
