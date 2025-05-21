using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	internal sealed class AmazonPurchasesCloudSlotSynchronizer : AmazonCloudSlotSynchronizer
	{
		protected override Task<CloudPullResult> PullCore(AGSGameDataMap dataMap)
		{
			return Task.FromResult<CloudPullResult>(CloudPullResult.Successful);
		}

		protected override void PushCore(AGSGameDataMap dataMap, string data)
		{
			if (data != null)
			{
				bool flag = Json.Deserialize(data) is List<object>;
			}
		}
	}
}
