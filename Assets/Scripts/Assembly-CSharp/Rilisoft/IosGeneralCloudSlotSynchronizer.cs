using System.Threading.Tasks;

namespace Rilisoft
{
	public class IosGeneralCloudSlotSynchronizer : IosSimplePushCloudSlotSynchronizer
	{
		public IosGeneralCloudSlotSynchronizer(string cloudKey)
			: base(cloudKey)
		{
		}

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			TaskCompletionSource<CloudPullResult> obj = new TaskCompletionSource<CloudPullResult>();
			obj.SetResult(CloudPullResult.Successful);
			if (IcloudService.IcloudAvailable)
			{
				SetPulled(true);
			}
			return obj.Task;
		}
	}
}
