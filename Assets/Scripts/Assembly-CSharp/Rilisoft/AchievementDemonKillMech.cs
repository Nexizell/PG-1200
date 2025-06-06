namespace Rilisoft
{
	public class AchievementDemonKillMech : AchievementBindedToMyPlayer
	{
		public AchievementDemonKillMech(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			bool isCompleted = base.IsCompleted;
		}

		protected override void OnPlayerInstanceSetted()
		{
			if (base.MyPlayer != null)
			{
				base.MyPlayer.OnMyKillMechInDemon += MyPlayer_OnMyKillMechInDemon;
			}
		}

		private void MyPlayer_OnMyKillMechInDemon()
		{
			if (!base.IsCompleted)
			{
				Gain();
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			if (base.MyPlayer != null)
			{
				base.MyPlayer.OnMyKillMechInDemon -= MyPlayer_OnMyKillMechInDemon;
			}
		}
	}
}
