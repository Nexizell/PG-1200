namespace Rilisoft.TimeManagment
{
	public class StoragedSafityTimer : StoragedTimer
	{
		public override long? CurrentTime
		{
			get
			{
				if (FriendsController.ServerTime > 0)
				{
					return FriendsController.ServerTime;
				}
				return null;
			}
		}

		public StoragedSafityTimer(string id, float duration, float speedMultiplier = 1f)
			: base(id, duration, speedMultiplier)
		{
		}
	}
}
