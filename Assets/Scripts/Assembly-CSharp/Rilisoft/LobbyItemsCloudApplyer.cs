using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemsCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass1_0
		{
			public bool localDirty;

			internal LobbyItemPlayerInfo _003CApply_003Eb__2(LobbyItemPlayerInfo item)
			{
				if (item.EquipTime == -1)
				{
					item.EquipTime = FriendsController.ServerTime;
					localDirty = true;
				}
				return item;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyItemsCloudApplyer _003C_003E4__this;

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
					UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer currentPullResult == null");
				}
				try
				{
					_003C_003Ec__DisplayClass1_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass1_0();
					LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject = (currentResult.IsNullOrEmpty() ? new LobbyItemPlayerInfoSerializedObject() : (JsonUtility.FromJson<LobbyItemPlayerInfoSerializedObject>(currentResult) ?? new LobbyItemPlayerInfoSerializedObject()));
					if (lobbyItemPlayerInfoSerializedObject.Infos == null)
					{
						lobbyItemPlayerInfoSerializedObject.Infos = new List<LobbyItemPlayerInfo>();
					}
					CS_0024_003C_003E8__locals0.localDirty = false;
					List<LobbyItemPlayerInfo> source = (from i in Singleton<LobbyItemsController>.Instance.AllItems
						where i != null && i.PlayerInfo != null
						select i.PlayerInfo).ToList();
					LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject2 = new LobbyItemPlayerInfoSerializedObject
					{
						Infos = source.Select(delegate(LobbyItemPlayerInfo item)
						{
							if (item.EquipTime == -1)
							{
								item.EquipTime = FriendsController.ServerTime;
								CS_0024_003C_003E8__locals0.localDirty = true;
							}
							return item;
						}).ToList()
					};
					string text = JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject2);
					LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject3 = MergeLobbyItems(lobbyItemPlayerInfoSerializedObject2, lobbyItemPlayerInfoSerializedObject);
					LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject4 = new LobbyItemPlayerInfoSerializedObject
					{
						Infos = lobbyItemPlayerInfoSerializedObject3.Infos.Where((LobbyItemPlayerInfo item) => item.IsCrafted && !item.IsCraftedAndNotShown).ToList()
					};
					if (!CS_0024_003C_003E8__locals0.localDirty)
					{
						CS_0024_003C_003E8__locals0.localDirty = _003C_003E4__this.IsDifferent(lobbyItemPlayerInfoSerializedObject2, lobbyItemPlayerInfoSerializedObject3);
					}
					bool flag = _003C_003E4__this.IsDifferent(lobbyItemPlayerInfoSerializedObject, lobbyItemPlayerInfoSerializedObject4);
					if (CS_0024_003C_003E8__locals0.localDirty && !skipApplyingToLocalState)
					{
						try
						{
							LobbyItemsController.SaveLobbyItemsPlayerData(lobbyItemPlayerInfoSerializedObject3);
							Singleton<LobbyItemsController>.Instance.ReReadPlayerData();
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer: Exception in SaveLobbyItemsPlayerData + ReReadPlayerData: {0}", ex);
						}
					}
					if (flag)
					{
						_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject4));
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'currentPullResult':{0}", currentResult);
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'cloudLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject));
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'localLobbyItems':{0}", text);
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'mergedLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject3));
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'mergedCloudLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject4));
						UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items: 'localDirty':{0} 'cloudDirty':{1}", CS_0024_003C_003E8__locals0.localDirty, flag);
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in TechnicalCloudApplyer.Apply: {0}", ex2);
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

		public LobbyItemsCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer currentPullResult == null");
			}
			try
			{
				LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject = (currentResult.IsNullOrEmpty() ? new LobbyItemPlayerInfoSerializedObject() : (JsonUtility.FromJson<LobbyItemPlayerInfoSerializedObject>(currentResult) ?? new LobbyItemPlayerInfoSerializedObject()));
				if (lobbyItemPlayerInfoSerializedObject.Infos == null)
				{
					lobbyItemPlayerInfoSerializedObject.Infos = new List<LobbyItemPlayerInfo>();
				}
				bool localDirty = false;
				List<LobbyItemPlayerInfo> source = (from i in Singleton<LobbyItemsController>.Instance.AllItems
					where i != null && i.PlayerInfo != null
					select i.PlayerInfo).ToList();
				LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject2 = new LobbyItemPlayerInfoSerializedObject
				{
					Infos = source.Select(delegate(LobbyItemPlayerInfo item)
					{
						if (item.EquipTime == -1)
						{
							item.EquipTime = FriendsController.ServerTime;
							localDirty = true;
						}
						return item;
					}).ToList()
				};
				string text = JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject2);
				LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject3 = MergeLobbyItems(lobbyItemPlayerInfoSerializedObject2, lobbyItemPlayerInfoSerializedObject);
				LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject4 = new LobbyItemPlayerInfoSerializedObject
				{
					Infos = lobbyItemPlayerInfoSerializedObject3.Infos.Where((LobbyItemPlayerInfo item) => item.IsCrafted && !item.IsCraftedAndNotShown).ToList()
				};
				if (!localDirty)
				{
					localDirty = IsDifferent(lobbyItemPlayerInfoSerializedObject2, lobbyItemPlayerInfoSerializedObject3);
				}
				bool flag = IsDifferent(lobbyItemPlayerInfoSerializedObject, lobbyItemPlayerInfoSerializedObject4);
				if (localDirty && !skipApplyingToLocalState)
				{
					try
					{
						LobbyItemsController.SaveLobbyItemsPlayerData(lobbyItemPlayerInfoSerializedObject3);
						Singleton<LobbyItemsController>.Instance.ReReadPlayerData();
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("LobbyItemsCloudApplyer: Exception in SaveLobbyItemsPlayerData + ReReadPlayerData: {0}", ex);
					}
				}
				if (flag)
				{
					SlotSynchronizer.Push(JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject4));
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'currentPullResult':{0}", currentResult);
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'cloudLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject));
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'localLobbyItems':{0}", text);
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'mergedLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject3));
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items:\n'mergedCloudLobbyItems':{0}", JsonUtility.ToJson(lobbyItemPlayerInfoSerializedObject4));
					UnityEngine.Debug.LogFormat("LobbyItemsCloudApplyer: Succeeded to apply lobby items: 'localDirty':{0} 'cloudDirty':{1}", localDirty, flag);
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in TechnicalCloudApplyer.Apply: {0}", ex2);
			}
		}

		public static LobbyItemPlayerInfoSerializedObject MergeLobbyItems(LobbyItemPlayerInfoSerializedObject left, LobbyItemPlayerInfoSerializedObject right)
		{
			LobbyItemInfo value;
			List<LobbyItemPlayerInfo> infos = (from item in (from item in left.Infos.Concat(right.Infos)
					group item by item.InfoId).Select(delegate(IGrouping<string, LobbyItemPlayerInfo> grouping)
				{
					LobbyItemPlayerInfo lobbyItemPlayerInfo = grouping.First();
					if (grouping.Count() == 1)
					{
						LobbyItemPlayerInfo lobbyItemPlayerInfo2 = new LobbyItemPlayerInfo();
						lobbyItemPlayerInfo2.CopyValues(lobbyItemPlayerInfo);
						return lobbyItemPlayerInfo2;
					}
					LobbyItemPlayerInfo lobbyItemPlayerInfo3 = grouping.ElementAt(1);
					LobbyItemPlayerInfo lobbyItemPlayerInfo4 = new LobbyItemPlayerInfo
					{
						InfoId = lobbyItemPlayerInfo.InfoId,
						CraftStarted = lobbyItemPlayerInfo.CraftStarted,
						IsCrafted = true,
						IsCraftedAndNotShown = false,
						IsNew = false
					};
					if (lobbyItemPlayerInfo.EquipTime >= lobbyItemPlayerInfo3.EquipTime)
					{
						lobbyItemPlayerInfo4.IsEquiped = lobbyItemPlayerInfo.IsEquiped;
						lobbyItemPlayerInfo4.EquipTime = lobbyItemPlayerInfo.EquipTime;
					}
					else if (lobbyItemPlayerInfo.EquipTime < lobbyItemPlayerInfo3.EquipTime)
					{
						lobbyItemPlayerInfo4.IsEquiped = lobbyItemPlayerInfo3.IsEquiped;
						lobbyItemPlayerInfo4.EquipTime = lobbyItemPlayerInfo3.EquipTime;
					}
					return lobbyItemPlayerInfo4;
				})
				group item by LobbyItemsInfo.info.TryGetValue(item.InfoId, out value) ? value.Slot : LobbyItemInfo.LobbyItemSlot.device_2).SelectMany(delegate(IGrouping<LobbyItemInfo.LobbyItemSlot, LobbyItemPlayerInfo> grouping)
			{
				List<LobbyItemPlayerInfo> list = (from item in grouping
					orderby item.EquipTime descending, item.IsEquiped descending
					select item).ToList();
				for (int i = 1; i < list.Count; i++)
				{
					list[i].IsEquiped = false;
				}
				return grouping;
			}).ToList();
			return new LobbyItemPlayerInfoSerializedObject
			{
				Infos = infos
			};
		}

		private bool IsDifferent(LobbyItemPlayerInfoSerializedObject before, LobbyItemPlayerInfoSerializedObject after)
		{
			if (before.Infos.Count != after.Infos.Count)
			{
				return true;
			}
			IOrderedEnumerable<string> first = from item in before.Infos
				select string.Format("{0}{1}{2}{3}{4}", item.InfoId, item.EquipTime, item.IsEquiped, item.IsCrafted, item.IsCraftedAndNotShown) into id
				orderby id
				select id;
			IOrderedEnumerable<string> second = from item in after.Infos
				select string.Format("{0}{1}{2}{3}{4}", item.InfoId, item.EquipTime, item.IsEquiped, item.IsCrafted, item.IsCraftedAndNotShown) into id
				orderby id
				select id;
			return !first.SequenceEqual(second);
		}
	}
}
