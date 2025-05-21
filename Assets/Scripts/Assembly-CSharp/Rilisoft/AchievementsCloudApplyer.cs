using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementsCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementsCloudApplyer _003C_003E4__this;

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
					UnityEngine.Debug.LogErrorFormat("AchievementsCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("AchievementsCloudApplyer currentPullResult == null");
				}
				try
				{
					AchievementProgressSyncObject achievementProgressSyncObject = (currentResult.IsNullOrEmpty() ? new AchievementProgressSyncObject() : AchievementProgressSyncObject.FromJson(currentResult));
					AchievementProgressSyncObject achievementProgressSyncObject2 = ReadLocalData();
					AchievementProgressSyncObject achievementProgressSyncObject3 = AchievementProgressData.Merge(achievementProgressSyncObject2, achievementProgressSyncObject);
					bool flag = !achievementProgressSyncObject2.Equals(achievementProgressSyncObject3);
					if (flag && !skipApplyingToLocalState)
					{
						SaveLocalData(achievementProgressSyncObject3);
					}
					bool flag2 = !achievementProgressSyncObject.Equals(achievementProgressSyncObject3);
					if (flag2 || achievementProgressSyncObject.Conflicted)
					{
						_003C_003E4__this.SlotSynchronizer.Push(AchievementProgressSyncObject.ToJson(achievementProgressSyncObject3));
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("AchievementsCloudApplyer: Succeeded to apply achievemnts:\n'currentPullResult':{0},\n'localAchievements':{1},\n'cloudAchievements':{2},\n'mergedAchievements':{3},\n'localDirty':{4},\n'cloudDirty':{5}", currentResult, JsonUtility.ToJson(achievementProgressSyncObject2), JsonUtility.ToJson(achievementProgressSyncObject), JsonUtility.ToJson(achievementProgressSyncObject3), flag, flag2);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in AchievementsCloudApplyer.Apply: {0}", ex);
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

		public AchievementsCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("AchievementsCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("AchievementsCloudApplyer currentPullResult == null");
			}
			try
			{
				AchievementProgressSyncObject achievementProgressSyncObject = (currentResult.IsNullOrEmpty() ? new AchievementProgressSyncObject() : AchievementProgressSyncObject.FromJson(currentResult));
				AchievementProgressSyncObject achievementProgressSyncObject2 = ReadLocalData();
				AchievementProgressSyncObject achievementProgressSyncObject3 = AchievementProgressData.Merge(achievementProgressSyncObject2, achievementProgressSyncObject);
				bool flag = !achievementProgressSyncObject2.Equals(achievementProgressSyncObject3);
				if (flag && !skipApplyingToLocalState)
				{
					SaveLocalData(achievementProgressSyncObject3);
				}
				bool flag2 = !achievementProgressSyncObject.Equals(achievementProgressSyncObject3);
				if (flag2 || achievementProgressSyncObject.Conflicted)
				{
					SlotSynchronizer.Push(AchievementProgressSyncObject.ToJson(achievementProgressSyncObject3));
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("AchievementsCloudApplyer: Succeeded to apply achievemnts:\n'currentPullResult':{0},\n'localAchievements':{1},\n'cloudAchievements':{2},\n'mergedAchievements':{3},\n'localDirty':{4},\n'cloudDirty':{5}", currentResult, JsonUtility.ToJson(achievementProgressSyncObject2), JsonUtility.ToJson(achievementProgressSyncObject), JsonUtility.ToJson(achievementProgressSyncObject3), flag, flag2);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in AchievementsCloudApplyer.Apply: {0}", ex);
			}
		}

		private static AchievementProgressSyncObject ReadLocalData()
		{
			return new AchievementProgressSyncObject(AchievementsManager.ReadLocalProgress().ToList());
		}

		private static void SaveLocalData(AchievementProgressSyncObject achievementMemento)
		{
			if (!(Singleton<AchievementsManager>.Instance != null))
			{
				return;
			}
			foreach (AchievementProgressData progressDatum in achievementMemento.ProgressData)
			{
				Singleton<AchievementsManager>.Instance.SetProgress(progressDatum);
			}
			Singleton<AchievementsManager>.Instance.SaveProgresses();
		}
	}
}
