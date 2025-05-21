using System.Collections.Generic;

namespace Rilisoft
{
	public sealed class ModeAccumulativeQuest : AccumulativeQuestBase
	{
		private readonly GameConnect.GameMode _mode;

		public GameConnect.GameMode Mode
		{
			get
			{
				return _mode;
			}
		}

		public ModeAccumulativeQuest(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, GameConnect.GameMode mode, int initialCount = 0)
			: base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			_mode = mode;
		}

		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["mode"] = _mode;
		}
	}
}
