using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	internal sealed class AmazonCampaignProgressCloudSlotSynchronizer : AmazonCloudSlotSynchronizer
	{
		protected override Task<CloudPullResult> PullCore(AGSGameDataMap dataMap)
		{
			using (AGSGameDataMap aGSGameDataMap = dataMap.GetMap("progressMap"))
			{
				HashSet<string> hashSet = new HashSet<string>(from k in aGSGameDataMap.GetMapKeys()
					where !string.IsNullOrEmpty(k)
					select k);
				Dictionary<string, Dictionary<string, int>> dictionary = new Dictionary<string, Dictionary<string, int>>(hashSet.Count);
				foreach (string item in hashSet)
				{
					using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap(item))
					{
						if (aGSGameDataMap2 == null)
						{
							continue;
						}
						HashSet<string> hashSet2 = new HashSet<string>(aGSGameDataMap2.GetHighestNumberKeys());
						Dictionary<string, int> dictionary2 = new Dictionary<string, int>(hashSet2.Count);
						foreach (string item2 in hashSet2)
						{
							using (AGSSyncableNumber aGSSyncableNumber = aGSGameDataMap2.GetHighestNumber(item2))
							{
								if (aGSSyncableNumber != null)
								{
									int value = aGSSyncableNumber.AsInt();
									dictionary2[item2] = value;
								}
							}
						}
						dictionary[item] = dictionary2;
					}
				}
				string currentResultCore = Json.Serialize(dictionary);
				base.CurrentResultCore = currentResultCore;
				base.PulledCore = true;
			}
			return Task.FromResult<CloudPullResult>(CloudPullResult.Successful);
		}

		protected override void PushCore(AGSGameDataMap dataMap, string data)
		{
			if (data == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = Json.Deserialize(data) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return;
			}
			using (AGSGameDataMap aGSGameDataMap = dataMap.GetMap("progressMap"))
			{
				foreach (KeyValuePair<string, object> item in dictionary)
				{
					Dictionary<string, object> dictionary2 = item.Value as Dictionary<string, object>;
					if (dictionary2 == null)
					{
						continue;
					}
					string key = item.Key;
					using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap(key))
					{
						foreach (KeyValuePair<string, object> item2 in dictionary2)
						{
							int val = Convert.ToInt32(item2.Value);
							string key2 = item2.Key;
							using (AGSSyncableNumber aGSSyncableNumber = aGSGameDataMap2.GetHighestNumber(key2))
							{
								aGSSyncableNumber.Set(val);
							}
						}
					}
				}
			}
		}
	}
}
