using System.Collections;
using Holoville.HOTween.Core;

namespace Holoville.HOTween
{
	public interface IHOTweenComponent
	{
		string id { get; set; }

		int intId { get; set; }

		bool autoKillOnComplete { get; set; }

		bool enabled { get; set; }

		float timeScale { get; set; }

		int loops { get; set; }

		LoopType loopType { get; set; }

		float position { get; set; }

		float duration { get; }

		float fullDuration { get; }

		float elapsed { get; }

		float fullElapsed { get; }

		UpdateType updateType { get; }

		int completedLoops { get; }

		bool isEmpty { get; }

		bool isReversed { get; }

		bool isLoopingBack { get; }

		bool isPaused { get; }

		bool hasStarted { get; }

		bool isComplete { get; }

		bool isSequenced { get; }

		void Kill();

		void Play();

		void PlayForward();

		void PlayBackwards();

		void Pause();

		void Rewind();

		void Restart();

		void Reverse(bool p_forcePlay = false);

		void Complete();

		bool GoTo(float p_time);

		bool GoToAndPlay(float p_time);

		IEnumerator WaitForCompletion();

		IEnumerator WaitForRewind();

		void ApplyCallback(CallbackType p_callbackType, TweenDelegate.TweenCallback p_callback);

		void ApplyCallback(CallbackType p_callbackType, TweenDelegate.TweenCallbackWParms p_callback, params object[] p_callbackParms);

		bool IsTweening(object p_target);

		bool IsLinkedTo(object p_target);
	}
}
