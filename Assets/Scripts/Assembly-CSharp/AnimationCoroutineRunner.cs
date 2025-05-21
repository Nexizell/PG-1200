using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationCoroutineRunner : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CPlay_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool useTimeScale;

		public Animation animation;

		public string clipName;

		private float _003C_timeAtLastFrame_003E5__1;

		private float _003C_progressTime_003E5__2;

		private AnimationState _003C_currState_003E5__3;

		public AnimationCoroutineRunner _003C_003E4__this;

		private bool _003CisPlaying_003E5__4;

		public Action onComplete;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CPlay_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!useTimeScale)
				{
					_003C_currState_003E5__3 = animation[clipName];
					_003CisPlaying_003E5__4 = true;
					_003C_progressTime_003E5__2 = 0f;
					_003C_timeAtLastFrame_003E5__1 = 0f;
					float num = 0f;
					float num2 = 0f;
					animation.Play(clipName);
					_003C_timeAtLastFrame_003E5__1 = Time.realtimeSinceStartup;
					goto IL_0168;
				}
				animation.Play(clipName);
				break;
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this == null || _003C_003E4__this.gameObject == null)
				{
					return false;
				}
				goto IL_0168;
			case 2:
				{
					_003C_003E1__state = -1;
					if (onComplete != null)
					{
						onComplete();
					}
					_003C_currState_003E5__3 = null;
					break;
				}
				IL_0168:
				if (_003CisPlaying_003E5__4)
				{
					try
					{
						float num = Time.realtimeSinceStartup;
						float num2 = num - _003C_timeAtLastFrame_003E5__1;
						_003C_timeAtLastFrame_003E5__1 = num;
						_003C_progressTime_003E5__2 += num2;
						_003C_currState_003E5__3.normalizedTime = _003C_progressTime_003E5__2 / _003C_currState_003E5__3.length;
						animation.Sample();
						if (_003C_progressTime_003E5__2 >= _003C_currState_003E5__3.length)
						{
							if (_003C_currState_003E5__3.wrapMode != WrapMode.Loop)
							{
								_003CisPlaying_003E5__4 = false;
							}
							else
							{
								_003C_progressTime_003E5__2 = 0f;
							}
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("Exception in AnimationCoroutineRunner Play: {0}", ex);
					}
					_003C_003E2__current = new WaitForEndOfFrame();
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public void StartPlay(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		StartCoroutine(Play(animation, clipName, useTimeScale, onComplete));
	}

	public IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		if (!useTimeScale)
		{
			AnimationState _currState = animation[clipName];
			bool isPlaying = true;
			float _progressTime = 0f;
			animation.Play(clipName);
			float _timeAtLastFrame = Time.realtimeSinceStartup;
			while (isPlaying)
			{
				try
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					float num = realtimeSinceStartup - _timeAtLastFrame;
					_timeAtLastFrame = realtimeSinceStartup;
					_progressTime += num;
					_currState.normalizedTime = _progressTime / _currState.length;
					animation.Sample();
					if (_progressTime >= _currState.length)
					{
						if (_currState.wrapMode != WrapMode.Loop)
						{
							isPlaying = false;
						}
						else
						{
							_progressTime = 0f;
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in AnimationCoroutineRunner Play: {0}", ex);
				}
				yield return new WaitForEndOfFrame();
				if (this == null || gameObject == null)
				{
					yield break;
				}
			}
			yield return null;
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else
		{
			animation.Play(clipName);
		}
	}
}
