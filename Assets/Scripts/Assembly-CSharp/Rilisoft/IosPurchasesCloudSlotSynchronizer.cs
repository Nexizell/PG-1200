using System.Threading.Tasks;

namespace Rilisoft
{
	public class IosPurchasesCloudSlotSynchronizer : IosSimplePushCloudSlotSynchronizer
	{
		public IosPurchasesCloudSlotSynchronizer(string cloudKey)
			: base(cloudKey)
		{
		}

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			TaskCompletionSource<CloudPullResult> obj = new TaskCompletionSource<CloudPullResult>();
			obj.SetResult(CloudPullResult.Successful);
			bool icloudAvailable = IcloudService.IcloudAvailable;
			return obj.Task;
		}
	}
}
