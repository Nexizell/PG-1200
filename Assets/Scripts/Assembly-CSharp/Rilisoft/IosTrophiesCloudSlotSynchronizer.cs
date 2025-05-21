using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	public class IosTrophiesCloudSlotSynchronizer : IosCloudSlotSynchronizer
	{
		private const string TrophiesNegativeCloudKey = "RatingNegative_CLOUD";

		private const string TrophiesPositiveCloudKey = "RatingPositive_CLOUD";

		private const string SEASON_CLOUD_KEY = "Season_CLOUD";

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			TaskCompletionSource<CloudPullResult> obj = new TaskCompletionSource<CloudPullResult>();
			obj.SetResult(CloudPullResult.Successful);
			bool icloudAvailable = IcloudService.IcloudAvailable;
			return obj.Task;
		}

		public override void Push(string data)
		{
			if (data == null)
			{
				Debug.LogErrorFormat("IosTrophiesCloudSlotSynchronizer.Push: data == null");
			}
			else
			{
				bool icloudAvailable = IcloudService.IcloudAvailable;
			}
		}
	}
}
