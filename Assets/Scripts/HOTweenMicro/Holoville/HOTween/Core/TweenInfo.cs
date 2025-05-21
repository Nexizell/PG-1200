using System.Collections.Generic;

namespace Holoville.HOTween.Core
{
	public class TweenInfo
	{
		public ABSTweenComponent tween;

		public bool isSequence;

		public List<object> targets;

		public bool isPaused
		{
			get
			{
				return tween.isPaused;
			}
		}

		public bool isComplete
		{
			get
			{
				return tween.isComplete;
			}
		}

		public bool isEnabled
		{
			get
			{
				return tween.enabled;
			}
		}

		public TweenInfo(ABSTweenComponent tween)
		{
			this.tween = tween;
			isSequence = tween is Sequence;
			targets = tween.GetTweenTargets();
		}
	}
}
