using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AmazonCampaignSecretsCloudSlotSynchronizer : AmazonCloudSlotSynchronizer
	{
		protected override Task<CloudPullResult> PullCore(AGSGameDataMap dataMap)
		{
			using (AGSGameDataMap aGSGameDataMap = dataMap.GetMap("campaignProgressMap"))
			{
				using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap("levels"))
				{
					HashSet<string> mapKeys = aGSGameDataMap2.GetMapKeys();
					CampaignProgressMemento campaignProgressMemento = default(CampaignProgressMemento);
					foreach (string item in mapKeys)
					{
						AGSGameDataMap map = aGSGameDataMap2.GetMap(item);
						if (map != null)
						{
							LevelProgressMemento levelProgressMemento = new LevelProgressMemento(item);
							AGSSyncableNumber highestNumber = map.GetHighestNumber("coinCount");
							levelProgressMemento.CoinCount = ((highestNumber != null) ? highestNumber.AsInt() : 0);
							AGSSyncableNumber highestNumber2 = map.GetHighestNumber("gemCount");
							levelProgressMemento.GemCount = ((highestNumber2 != null) ? highestNumber2.AsInt() : 0);
							campaignProgressMemento.Levels.Add(levelProgressMemento);
						}
					}
					string currentResultCore = JsonUtility.ToJson(campaignProgressMemento);
					base.CurrentResultCore = currentResultCore;
					base.PulledCore = true;
				}
			}
			return Task.FromResult<CloudPullResult>(CloudPullResult.Successful);
		}

		protected override void PushCore(AGSGameDataMap dataMap, string data)
		{
			if (data == null)
			{
				return;
			}
			CampaignProgressMemento campaignProgressMemento = JsonUtility.FromJson<CampaignProgressMemento>(data);
			using (AGSGameDataMap aGSGameDataMap = dataMap.GetMap("campaignProgressMap"))
			{
				using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap("levels"))
				{
					foreach (LevelProgressMemento level in campaignProgressMemento.Levels)
					{
						AGSGameDataMap map = aGSGameDataMap2.GetMap(level.LevelId);
						if (map != null)
						{
							map.GetHighestNumber("coinCount").Set(level.CoinCount);
							map.GetHighestNumber("gemCount").Set(level.GemCount);
						}
					}
				}
			}
		}
	}
}
