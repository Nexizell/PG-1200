using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AmazonTrophiesCloudSlotSynchronizer : AmazonCloudSlotSynchronizer
	{
		protected override Task<CloudPullResult> PullCore(AGSGameDataMap dataMap)
		{
			return Task.FromResult<CloudPullResult>(CloudPullResult.Successful);
		}

		protected override void PushCore(AGSGameDataMap dataMap, string data)
		{
			if (data != null)
			{
				JsonUtility.FromJson<TrophiesMemento>(data);
			}
		}
	}
}
