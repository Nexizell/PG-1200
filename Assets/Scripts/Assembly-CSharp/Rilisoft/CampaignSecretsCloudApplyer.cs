using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	public class CampaignSecretsCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass1_0
		{
			public Dictionary<string, LevelProgressMemento> mergedLevels;

			internal bool _003CApply_003Eb__0(LevelProgressMemento level)
			{
				if (level.CoinCount >= mergedLevels[level.LevelId].CoinCount)
				{
					return level.GemCount < mergedLevels[level.LevelId].GemCount;
				}
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CampaignSecretsCloudApplyer _003C_003E4__this;

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
				_003C_003Ec__DisplayClass1_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass1_0();
				if (_003C_003E4__this.SlotSynchronizer == null)
				{
					UnityEngine.Debug.LogErrorFormat("CampaignSecretsCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("CampaignSecretsCloudApplyer currentPullResult == null");
				}
				CampaignProgressMemento left = (currentResult.IsNullOrEmpty() ? new CampaignProgressMemento(false) : JsonUtility.FromJson<CampaignProgressMemento>(currentResult));
				CampaignProgressMemento campaignProgressMemento = LoadMemento();
				CampaignProgressMemento campaignProgressMemento2 = CampaignProgressMemento.Merge(left, campaignProgressMemento);
				CS_0024_003C_003E8__locals0.mergedLevels = campaignProgressMemento2.GetLevelsAsDictionary();
				Func<LevelProgressMemento, bool> predicate = (LevelProgressMemento level) => level.CoinCount < CS_0024_003C_003E8__locals0.mergedLevels[level.LevelId].CoinCount || level.GemCount < CS_0024_003C_003E8__locals0.mergedLevels[level.LevelId].GemCount;
				bool flag = campaignProgressMemento.Levels.Count < campaignProgressMemento2.Levels.Count || campaignProgressMemento.Levels.Any(predicate);
				if (flag && !skipApplyingToLocalState)
				{
					OverwriteMemento(campaignProgressMemento2);
				}
				bool flag2 = left.Levels.Count < campaignProgressMemento2.Levels.Count || left.Levels.Any(predicate);
				if (flag2)
				{
					_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(campaignProgressMemento2));
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("CampaignSecretsCloudApplyer: Succeeded to apply campaign secrets:\n'currentPullResult':{0},\n'localBonusesMemento':{1},\n'mergedMemento':{2},\n'localDirty':{3},\n'cloudDirty':{4}", currentResult, JsonUtility.ToJson(campaignProgressMemento), JsonUtility.ToJson(campaignProgressMemento2), flag, flag2);
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

		public CampaignSecretsCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("CampaignSecretsCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("CampaignSecretsCloudApplyer currentPullResult == null");
			}
			CampaignProgressMemento left = (currentResult.IsNullOrEmpty() ? new CampaignProgressMemento(false) : JsonUtility.FromJson<CampaignProgressMemento>(currentResult));
			CampaignProgressMemento campaignProgressMemento = LoadMemento();
			CampaignProgressMemento campaignProgressMemento2 = CampaignProgressMemento.Merge(left, campaignProgressMemento);
			Dictionary<string, LevelProgressMemento> mergedLevels = campaignProgressMemento2.GetLevelsAsDictionary();
			Func<LevelProgressMemento, bool> predicate = (LevelProgressMemento level) => level.CoinCount < mergedLevels[level.LevelId].CoinCount || level.GemCount < mergedLevels[level.LevelId].GemCount;
			bool flag = campaignProgressMemento.Levels.Count < campaignProgressMemento2.Levels.Count || campaignProgressMemento.Levels.Any(predicate);
			if (flag && !skipApplyingToLocalState)
			{
				OverwriteMemento(campaignProgressMemento2);
			}
			bool flag2 = left.Levels.Count < campaignProgressMemento2.Levels.Count || left.Levels.Any(predicate);
			if (flag2)
			{
				SlotSynchronizer.Push(JsonUtility.ToJson(campaignProgressMemento2));
			}
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("CampaignSecretsCloudApplyer: Succeeded to apply campaign secrets:\n'currentPullResult':{0},\n'localBonusesMemento':{1},\n'mergedMemento':{2},\n'localDirty':{3},\n'cloudDirty':{4}", currentResult, JsonUtility.ToJson(campaignProgressMemento), JsonUtility.ToJson(campaignProgressMemento2), flag, flag2);
			}
		}

		private static CampaignProgressMemento LoadMemento()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.LoadMemento()", "CampaignProgressSynchronizer");
			using (new ScopeLogger(callee, false))
			{
				Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>();
				string[] array = Storager.getString(Defs.LevelsWhereGetCoinS).Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string text in array)
				{
					LevelProgressMemento value;
					if (dictionary.TryGetValue(text, out value))
					{
						value.CoinCount = 1;
						continue;
					}
					value = new LevelProgressMemento(text)
					{
						CoinCount = 1
					};
					dictionary.Add(text, value);
				}
				List<object> list = Json.Deserialize(Storager.getString(Defs.LevelsWhereGotGems)) as List<object>;
				foreach (string item in (list != null) ? list.OfType<string>().ToList() : new List<string>())
				{
					LevelProgressMemento value2;
					if (dictionary.TryGetValue(item, out value2))
					{
						value2.GemCount = 1;
						continue;
					}
					value2 = new LevelProgressMemento(item)
					{
						GemCount = 1
					};
					dictionary.Add(item, value2);
				}
				CampaignProgressMemento result = default(CampaignProgressMemento);
				result.Levels.AddRange(dictionary.Values);
				return result;
			}
		}

		private static void OverwriteMemento(CampaignProgressMemento campaignProgressMemento)
		{
			string[] value = (from l in campaignProgressMemento.Levels
				where l.CoinCount > 0
				select l.LevelId).ToArray();
			string val = string.Join("#", value);
			Storager.setString(Defs.LevelsWhereGetCoinS, val);
			string val2 = Json.Serialize((from l in campaignProgressMemento.Levels
				where l.GemCount > 0
				select l.LevelId).ToList());
			Storager.setString(Defs.LevelsWhereGotGems, val2);
		}
	}
}
