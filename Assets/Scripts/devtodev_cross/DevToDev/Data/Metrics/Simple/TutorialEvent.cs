using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class TutorialEvent : SimpleEvent
	{
		private static readonly string TUTOR_STEP = "step";

		private int tutorStep;

		public int TutorStep
		{
			get
			{
				return tutorStep;
			}
		}

		public TutorialEvent()
		{
		}

		public TutorialEvent(ObjectInfo info)
			: base(info)
		{
		}

		public TutorialEvent(int state)
			: base(EventType.Tutorial)
		{
			tutorStep = state;
			parameters.Add(TUTOR_STEP, state);
		}

		public override bool IsEqualToMetric(Event other)
		{
			return IsParameterEqual(other, TUTOR_STEP);
		}
	}
}
