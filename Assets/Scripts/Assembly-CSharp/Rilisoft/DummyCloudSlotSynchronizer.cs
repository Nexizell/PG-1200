using System.Threading.Tasks;

namespace Rilisoft
{
	public class DummyCloudSlotSynchronizer : CloudSlotSynchronizer
	{
		public override string CurrentResult
		{
			get
			{
				return string.Empty;
			}
		}

		public override bool Pulled
		{
			get
			{
				return true;
			}
		}

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			return Task.FromResult<CloudPullResult>(CloudPullResult.LoginFailed);
		}

		public override void Push(string data)
		{
		}
	}
}
