using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	public class CampaignProgressCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CampaignProgressCloudApplyer _003C_003E4__this;

			public bool skipApplyingToLocalState;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CApply_003Ed__1(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				if (_003C_003E1__state != 0)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (_003C_003E4__this.SlotSynchronizer == null)
				{
					UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer currentPullResult == null");
				}
				Dictionary<string, Dictionary<string, int>> dictionary = (currentResult.IsNullOrEmpty() ? new Dictionary<string, Dictionary<string, int>>() : CampaignProgress.DeserializeProgress(currentResult));
				if (!skipApplyingToLocalState)
				{
					MergeUpdateLocalProgress(dictionary);
					CampaignProgress.ActualizeComicsViews();
					WeaponManager.ActualizeWeaponsForCampaignProgress();
					try
					{
						if (!dictionary.ContainsKey("_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY"))
						{
							ChooseBox.UnlockReachedBoxes();
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("Exception in unlocking reached boxes: {0}", ex);
					}
				}
				try
				{
					bool flag = !dictionary.ContainsKey("_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY") || CampaignProgress.boxesLevelsAndStars.Keys.Except(dictionary.Keys).Any() || CampaignProgress.boxesLevelsAndStars.SelectMany((KeyValuePair<string, Dictionary<string, int>> kvp) => kvp.Value.Values).Sum() != dictionary.SelectMany((KeyValuePair<string, Dictionary<string, int>> kvp) => kvp.Value.Values).Sum();
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("CampaignProgressCloudApplyer: Succeeded to apply campaign progress:\n'currentPullResult':{0},\n'cloudProgress':{1},\n'cloudDirty':{2}", currentResult, Json.Serialize(dictionary), flag);
					}
					if (flag)
					{
						Dictionary<string, Dictionary<string, int>> progressDictionary = CampaignProgress.GetProgressDictionary("CampaignProgress");
						progressDictionary["_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY"] = new Dictionary<string, int>();
						_003C_003E4__this.SlotSynchronizer.Push(CampaignProgress.SerializeProgress(progressDictionary));
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer: Exception in trying to write to cloud: {0}", ex2);
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public CampaignProgressCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer currentPullResult == null");
			}
			Dictionary<string, Dictionary<string, int>> dictionary = (currentResult.IsNullOrEmpty() ? new Dictionary<string, Dictionary<string, int>>() : CampaignProgress.DeserializeProgress(currentResult));
			if (!skipApplyingToLocalState)
			{
				MergeUpdateLocalProgress(dictionary);
				CampaignProgress.ActualizeComicsViews();
				WeaponManager.ActualizeWeaponsForCampaignProgress();
				try
				{
					if (!dictionary.ContainsKey("_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY"))
					{
						ChooseBox.UnlockReachedBoxes();
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in unlocking reached boxes: {0}", ex);
				}
			}
			try
			{
				bool flag = !dictionary.ContainsKey("_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY") || CampaignProgress.boxesLevelsAndStars.Keys.Except(dictionary.Keys).Any() || CampaignProgress.boxesLevelsAndStars.SelectMany((KeyValuePair<string, Dictionary<string, int>> kvp) => kvp.Value.Values).Sum() != dictionary.SelectMany((KeyValuePair<string, Dictionary<string, int>> kvp) => kvp.Value.Values).Sum();
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("CampaignProgressCloudApplyer: Succeeded to apply campaign progress:\n'currentPullResult':{0},\n'cloudProgress':{1},\n'cloudDirty':{2}", currentResult, Json.Serialize(dictionary), flag);
				}
				if (flag)
				{
					Dictionary<string, Dictionary<string, int>> progressDictionary = CampaignProgress.GetProgressDictionary("CampaignProgress");
					progressDictionary["_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY"] = new Dictionary<string, int>();
					SlotSynchronizer.Push(CampaignProgress.SerializeProgress(progressDictionary));
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("CampaignProgressCloudApplyer: Exception in trying to write to cloud: {0}", ex2);
			}
		}

		private static void MergeUpdateLocalProgress(IDictionary<string, Dictionary<string, int>> incomingProgress)
		{
			foreach (KeyValuePair<string, Dictionary<string, int>> item in incomingProgress)
			{
				if (item.Key == "_11_4_TO_11_5_CAMPAIGN_BOXES_UNLOCK_SYNC_COMPLETED_KEY")
				{
					continue;
				}
				Dictionary<string, int> value;
				if (CampaignProgress.boxesLevelsAndStars.TryGetValue(item.Key, out value))
				{
					foreach (KeyValuePair<string, int> item2 in item.Value)
					{
						int value2;
						if (value.TryGetValue(item2.Key, out value2))
						{
							value[item2.Key] = Math.Max(value2, item2.Value);
						}
						else
						{
							value.Add(item2.Key, item2.Value);
						}
					}
				}
				else
				{
					CampaignProgress.boxesLevelsAndStars.Add(item.Key, item.Value);
				}
			}
			CampaignProgress.OpenNewBoxIfPossible();
		}
	}
}
