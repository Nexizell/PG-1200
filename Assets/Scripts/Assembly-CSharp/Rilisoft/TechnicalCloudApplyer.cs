using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class TechnicalCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TechnicalCloudApplyer _003C_003E4__this;

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
					UnityEngine.Debug.LogErrorFormat("TechnicalCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("TechnicalCloudApplyer currentPullResult == null");
				}
				try
				{
					TechnicalCloudInfo technicalCloudInfo = (currentResult.IsNullOrEmpty() ? new TechnicalCloudInfo() : (JsonUtility.FromJson<TechnicalCloudInfo>(currentResult) ?? new TechnicalCloudInfo()));
					bool flag = false;
					if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty() && !technicalCloudInfo.PlayerIds.Contains(FriendsController.sharedController.id))
					{
						technicalCloudInfo.PlayerIds.Add(FriendsController.sharedController.id);
						flag = true;
					}
					int @int = Storager.getInt(Defs.sumInnapsKey);
					int num = Math.Max(technicalCloudInfo.TotalInapps, @int);
					if (@int != num)
					{
						Storager.setInt(Defs.sumInnapsKey, num);
					}
					if (technicalCloudInfo.TotalInapps != num)
					{
						technicalCloudInfo.TotalInapps = num;
						flag = true;
					}
					int int2 = PlayerPrefs.GetInt(Defs.SessionDayNumberKey);
					int num2 = Math.Max(technicalCloudInfo.SessionDayCount, int2);
					if (int2 != num2)
					{
						PlayerPrefs.SetInt(Defs.SessionDayNumberKey, num2);
					}
					if (technicalCloudInfo.SessionDayCount != num2)
					{
						technicalCloudInfo.SessionDayCount = num2;
						flag = true;
					}
					float num3 = Tools.GetInGameTimeSeconds() ?? 0f;
					float num4 = Math.Max(technicalCloudInfo.InGameSeconds, num3);
					if (num3 != num4)
					{
						Storager.setString("PlayTime", num4.ToString());
					}
					if (technicalCloudInfo.InGameSeconds != num4)
					{
						technicalCloudInfo.InGameSeconds = num4;
						flag = true;
					}
					string text = JsonUtility.ToJson(technicalCloudInfo);
					Storager.setString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY", text);
					if (flag)
					{
						_003C_003E4__this.SlotSynchronizer.Push(text);
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("TechnicalCloudApplyer: Succeeded to apply tech cloud info:\n'currentPullResult':{0},\n'mergedMementoJson':{1},\n'cloudDirty':{2}", currentResult, text, flag);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in TechnicalCloudApplyer.Apply: {0}", ex);
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

		public TechnicalCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("TechnicalCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("TechnicalCloudApplyer currentPullResult == null");
			}
			try
			{
				TechnicalCloudInfo technicalCloudInfo = (currentResult.IsNullOrEmpty() ? new TechnicalCloudInfo() : (JsonUtility.FromJson<TechnicalCloudInfo>(currentResult) ?? new TechnicalCloudInfo()));
				bool flag = false;
				if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty() && !technicalCloudInfo.PlayerIds.Contains(FriendsController.sharedController.id))
				{
					technicalCloudInfo.PlayerIds.Add(FriendsController.sharedController.id);
					flag = true;
				}
				int @int = Storager.getInt(Defs.sumInnapsKey);
				int num = Math.Max(technicalCloudInfo.TotalInapps, @int);
				if (@int != num)
				{
					Storager.setInt(Defs.sumInnapsKey, num);
				}
				if (technicalCloudInfo.TotalInapps != num)
				{
					technicalCloudInfo.TotalInapps = num;
					flag = true;
				}
				int int2 = PlayerPrefs.GetInt(Defs.SessionDayNumberKey);
				int num2 = Math.Max(technicalCloudInfo.SessionDayCount, int2);
				if (int2 != num2)
				{
					PlayerPrefs.SetInt(Defs.SessionDayNumberKey, num2);
				}
				if (technicalCloudInfo.SessionDayCount != num2)
				{
					technicalCloudInfo.SessionDayCount = num2;
					flag = true;
				}
				float num3 = Tools.GetInGameTimeSeconds() ?? 0f;
				float num4 = Math.Max(technicalCloudInfo.InGameSeconds, num3);
				if (num3 != num4)
				{
					Storager.setString("PlayTime", num4.ToString());
				}
				if (technicalCloudInfo.InGameSeconds != num4)
				{
					technicalCloudInfo.InGameSeconds = num4;
					flag = true;
				}
				string text = JsonUtility.ToJson(technicalCloudInfo);
				Storager.setString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY", text);
				if (flag)
				{
					SlotSynchronizer.Push(text);
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("TechnicalCloudApplyer: Succeeded to apply tech cloud info:\n'currentPullResult':{0},\n'mergedMementoJson':{1},\n'cloudDirty':{2}", currentResult, text, flag);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in TechnicalCloudApplyer.Apply: {0}", ex);
			}
		}
	}
}
