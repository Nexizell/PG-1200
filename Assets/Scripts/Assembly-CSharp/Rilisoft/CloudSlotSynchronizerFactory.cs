using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal static class CloudSlotSynchronizerFactory
	{
		internal static class DictionaryLoadedListener
		{
			internal static string MergeProgress(string localDataString, string serverDataString)
			{
				Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(localDataString);
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, Dictionary<string, int>>();
				}
				Dictionary<string, Dictionary<string, int>> dictionary2 = CampaignProgress.DeserializeProgress(serverDataString);
				if (dictionary2 == null)
				{
					dictionary2 = new Dictionary<string, Dictionary<string, int>>();
				}
				Dictionary<string, Dictionary<string, int>> dictionary3 = new Dictionary<string, Dictionary<string, int>>();
				foreach (string item in dictionary.Keys.Concat(dictionary2.Keys).Distinct())
				{
					dictionary3.Add(item, new Dictionary<string, int>());
				}
				foreach (KeyValuePair<string, Dictionary<string, int>> item2 in dictionary3)
				{
					Dictionary<string, int> value;
					if (dictionary.TryGetValue(item2.Key, out value))
					{
						foreach (KeyValuePair<string, int> item3 in value)
						{
							item2.Value.Add(item3.Key, item3.Value);
						}
					}
					Dictionary<string, int> value2;
					if (!dictionary2.TryGetValue(item2.Key, out value2))
					{
						continue;
					}
					foreach (KeyValuePair<string, int> item4 in value2)
					{
						int value3;
						if (item2.Value.TryGetValue(item4.Key, out value3))
						{
							item2.Value[item4.Key] = Math.Max(value3, item4.Value);
						}
						else
						{
							item2.Value.Add(item4.Key, item4.Value);
						}
					}
				}
				return CampaignProgress.SerializeProgress(dictionary3);
			}
		}

		private static readonly Dictionary<string, Func<string, string, string>> s_slotNameToMergerMap = new Dictionary<string, Func<string, string, string>>(7)
		{
			{ "CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT", MergePurchases },
			{ "CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT", MergeCampaignProgress },
			{ "CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT", MergeCampaignSecrets },
			{ "CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT", MergeTrophies },
			{ "CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT", MergeSkins },
			{ "CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT", MergeAchievements },
			{ "CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT", MergePets },
			{ "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT", MergeTechnicalInfos },
			{ "CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT", MergeLobbyItems }
		};

		private static readonly Dictionary<string, string> s_slotNamesToCloudKeysIos = new Dictionary<string, string>
		{
			{ "CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT", "purchases_cloud_ios" },
			{ "CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT", "CampaignProgress_CLOUD" },
			{ "CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT", "CampaignProgress_BONUSES_IOS_CLOUD" },
			{ "CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT", "" },
			{ "CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT", "Skins_IOS_CLOUD" },
			{ "CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT", "Achievements_IOS_CLOUD" },
			{ "CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT", "Pets.PlayerPets_IOS_CLOUD" },
			{ "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT", "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT" },
			{ "CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT", "LOBBY_ITEMS_SYNCHRONIZATION_SLOT" }
		};

		private static readonly Dictionary<string, string> s_slotNamesToFilenamesAndroid = new Dictionary<string, string>
		{
			{ "CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT", "Purchases" },
			{ "CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT", "Progress" },
			{ "CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT", "CampaignProgress" },
			{ "CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT", "Trophies" },
			{ "CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT", "Skins" },
			{ "CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT", "Achievements" },
			{ "CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT", "Pets.PlayerPets" },
			{ "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT", "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT" },
			{ "CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT", "CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT" }
		};

		private static Func<string, string, string> GetMergerForSlotName(string slotName)
		{
			if (string.IsNullOrEmpty(slotName))
			{
				Debug.LogError("GetMergerForSlotName(): `slotName` must not be null or empty");
				return TrivialMerge;
			}
			Func<string, string, string> value;
			if (!s_slotNameToMergerMap.TryGetValue(slotName, out value))
			{
				Debug.LogErrorFormat("GetMergerForSlotName(): unknown `slotName` `{0}`", slotName);
				return TrivialMerge;
			}
			if (value == null)
			{
				Debug.LogErrorFormat("GetMergerForSlotName(): merger for `slotName` `{0}` must not be null", slotName);
				return TrivialMerge;
			}
			return value;
		}

		private static string TrivialMerge(string left, string right)
		{
			return left ?? right;
		}

		private static string MergePurchases(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			HashSet<string> hashSet = new HashSet<string>(((Json.Deserialize(left) as List<object>) ?? new List<object>()).Select(Convert.ToString));
			HashSet<string> hashSet2 = new HashSet<string>(((Json.Deserialize(right) as List<object>) ?? new List<object>()).Select(Convert.ToString));
			if (hashSet.IsSupersetOf(hashSet2))
			{
				return left;
			}
			if (hashSet2.IsSupersetOf(hashSet))
			{
				return right;
			}
			hashSet.UnionWith(hashSet2);
			return Json.Serialize(hashSet);
		}

		private static string MergeCampaignProgress(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			return DictionaryLoadedListener.MergeProgress(left, right);
		}

		private static string MergeCampaignSecrets(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			CampaignProgressMemento left2;
			try
			{
				left2 = JsonUtility.FromJson<CampaignProgressMemento>(left);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left CampaignProgressMemento: `{0}`", left);
				return right;
			}
			CampaignProgressMemento right2;
			try
			{
				right2 = JsonUtility.FromJson<CampaignProgressMemento>(right);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right CampaignProgressMemento: `{0}`", right);
				return left;
			}
			return JsonUtility.ToJson(CampaignProgressMemento.Merge(left2, right2));
		}

		private static string MergeTrophies(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			TrophiesMemento trophiesMemento;
			try
			{
				trophiesMemento = JsonUtility.FromJson<TrophiesMemento>(left);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left TrophiesMemento: `{0}`", left);
				return right;
			}
			TrophiesMemento trophiesMemento2;
			try
			{
				trophiesMemento2 = JsonUtility.FromJson<TrophiesMemento>(right);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right TrophiesMemento: `{0}`", right);
				return left;
			}
			TrophiesMemento trophiesMemento3 = TrophiesMemento.Merge(trophiesMemento, trophiesMemento2);
			if (trophiesMemento3.Equals(trophiesMemento))
			{
				return left;
			}
			if (trophiesMemento3.Equals(trophiesMemento2))
			{
				return right;
			}
			return JsonUtility.ToJson(trophiesMemento3);
		}

		private static string MergeSkins(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			SkinsMemento left2;
			try
			{
				left2 = JsonUtility.FromJson<SkinsMemento>(left);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left SkinsMemento: `{0}`", left);
				return right;
			}
			SkinsMemento right2;
			try
			{
				right2 = JsonUtility.FromJson<SkinsMemento>(right);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right SkinsMemento: `{0}`", right);
				return left;
			}
			return JsonUtility.ToJson(SkinsMemento.Merge(left2, right2));
		}

		private static string MergeAchievements(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			AchievementProgressSyncObject one;
			try
			{
				one = AchievementProgressSyncObject.FromJson(left);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left AchievementProgressSyncObject: `{0}`", left);
				return right;
			}
			AchievementProgressSyncObject two;
			try
			{
				two = AchievementProgressSyncObject.FromJson(right);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right AchievementProgressSyncObject: `{0}`", right);
				return left;
			}
			return AchievementProgressSyncObject.ToJson(AchievementProgressData.Merge(one, two));
		}

		private static string MergePets(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			PlayerPets localMemento;
			try
			{
				localMemento = JsonUtility.FromJson<PlayerPets>(left);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left PlayerPets: `{0}`", left);
				return right;
			}
			PlayerPets remoteMemento;
			try
			{
				remoteMemento = JsonUtility.FromJson<PlayerPets>(right);
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right PlayerPets: `{0}`", right);
				return left;
			}
			return JsonUtility.ToJson(PlayerPets.Merge(localMemento, remoteMemento));
		}

		private static string MergeTechnicalInfos(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			TechnicalCloudInfo technicalCloudInfo;
			try
			{
				technicalCloudInfo = JsonUtility.FromJson<TechnicalCloudInfo>(left) ?? new TechnicalCloudInfo();
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize left TechnicalCloudInfo: `{0}`", left);
				return right;
			}
			TechnicalCloudInfo technicalCloudInfo2;
			try
			{
				technicalCloudInfo2 = JsonUtility.FromJson<TechnicalCloudInfo>(right) ?? new TechnicalCloudInfo();
			}
			catch
			{
				Debug.LogErrorFormat("Failed to deserialize right TechnicalCloudInfo: `{0}`", right);
				return left;
			}
			technicalCloudInfo.PlayerIds = technicalCloudInfo.PlayerIds.Union(technicalCloudInfo2.PlayerIds).ToList();
			technicalCloudInfo.TotalInapps = Math.Max(technicalCloudInfo.TotalInapps, technicalCloudInfo2.TotalInapps);
			technicalCloudInfo.SessionDayCount = Math.Max(technicalCloudInfo.SessionDayCount, technicalCloudInfo2.SessionDayCount);
			technicalCloudInfo.InGameSeconds = Math.Max(technicalCloudInfo.InGameSeconds, technicalCloudInfo2.InGameSeconds);
			return JsonUtility.ToJson(technicalCloudInfo);
		}

		private static string MergeLobbyItems(string left, string right)
		{
			if (left == null || right == null)
			{
				return left ?? right;
			}
			try
			{
				LobbyItemPlayerInfoSerializedObject left2 = JsonUtility.FromJson<LobbyItemPlayerInfoSerializedObject>(left);
				LobbyItemPlayerInfoSerializedObject right2 = JsonUtility.FromJson<LobbyItemPlayerInfoSerializedObject>(right);
				return JsonUtility.ToJson(LobbyItemsCloudApplyer.MergeLobbyItems(left2, right2));
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in MergeLobbyItems: {0}", ex);
				return left;
			}
		}

		public static CloudSlotSynchronizer Create(string slotName)
		{
			if (Application.isEditor)
			{
				return new DummyCloudSlotSynchronizer();
			}
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.Android:
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					switch (slotName)
					{
					case "CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT":
						return new AmazonPurchasesCloudSlotSynchronizer();
					case "CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT":
						return new AmazonCampaignProgressCloudSlotSynchronizer();
					case "CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT":
						return new AmazonTrophiesCloudSlotSynchronizer();
					case "CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT":
						return new AmazonGenericCloudSlotSynchronizer("skinsJson");
					case "CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT":
						return new AmazonCampaignSecretsCloudSlotSynchronizer();
					case "CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT":
						return new AmazonGenericCloudSlotSynchronizer("petsJson");
					case "CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT":
						return new AmazonGenericCloudSlotSynchronizer("achievementsJson");
					case "CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT":
						return new AmazonGenericCloudSlotSynchronizer("CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT");
					case "CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT":
						return new AmazonGenericCloudSlotSynchronizer("CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT");
					default:
						return new DummyCloudSlotSynchronizer();
					}
				}
				return new AndroidCloudSlotSynchronizer(FilenameForSlotNameAndroid(slotName), GetMergerForSlotName(slotName));
			case RuntimePlatform.IPhonePlayer:
				if (!(slotName == "CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT"))
				{
					if (slotName == "CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT")
					{
						return new IosTrophiesCloudSlotSynchronizer();
					}
					return new IosGeneralCloudSlotSynchronizer(CloudKeyForSlotNameIos(slotName));
				}
				return new IosPurchasesCloudSlotSynchronizer(CloudKeyForSlotNameIos(slotName));
			case RuntimePlatform.MetroPlayerX64:
				return new DummyCloudSlotSynchronizer();
			default:
				Debug.LogErrorFormat("CloudSlotSynchronizerFactory.Create: unsupported platform. slotName = {0}", slotName);
				return new DummyCloudSlotSynchronizer();
			}
		}

		private static string CloudKeyForSlotNameIos(string slotName)
		{
			if (slotName.IsNullOrEmpty())
			{
				Debug.LogError("CloudKeyForSlotNameIos: slotName.isNullOrEmpty");
				return string.Empty;
			}
			string value;
			if (!s_slotNamesToCloudKeysIos.TryGetValue(slotName, out value))
			{
				Debug.LogErrorFormat("CloudKeyForSlotNameIos: unknown slot name {0}", slotName);
				return string.Empty;
			}
			if (value.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("CloudKeyForSlotNameIos: cloudKey.IsNullOrEmpty, slotName = {0}", slotName);
				return string.Empty;
			}
			return value;
		}

		private static string FilenameForSlotNameAndroid(string slotName)
		{
			if (slotName.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("FilenameForSlotNameAndroid: slotName.isNullOrEmpty");
				return string.Empty;
			}
			string value;
			if (!s_slotNamesToFilenamesAndroid.TryGetValue(slotName, out value))
			{
				Debug.LogErrorFormat("FilenameForSlotNameAndroid: unknown slot name {0}", slotName);
				return string.Empty;
			}
			if (value.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("FilenameForSlotNameAndroid: filename.IsNullOrEmpty, slotName = {0}", slotName);
				return string.Empty;
			}
			return value;
		}
	}
}
