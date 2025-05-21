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
	public class PurchasesCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PurchasesCloudApplyer _003C_003E4__this;

			public bool skipApplyingToLocalState;

			private int _003CwriteCounter_003E5__1;

			private IEnumerable<string> _003CpurchasesToSendToCloud_003E5__2;

			private string _003CcurrentPullResult_003E5__3;

			private IEnumerable<string> _003CnewPurchasesFromCloud_003E5__4;

			private List<string> _003CcloudPurchases_003E5__5;

			private List<string> _003ClocalPurchases_003E5__6;

			private IEnumerator<string> _003C_003E7__wrap1;

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
				int num = _003C_003E1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						_003C_003Em__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					bool flag;
					switch (_003C_003E1__state)
					{
					default:
						return false;
					case 0:
					{
						_003C_003E1__state = -1;
						if (_003C_003E4__this.SlotSynchronizer == null)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer.Apply: SlotSynchronizer == null");
							return false;
						}
						_003CcurrentPullResult_003E5__3 = _003C_003E4__this.SlotSynchronizer.CurrentResult;
						if (_003CcurrentPullResult_003E5__3 == null)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer currentPullResult == null");
						}
						if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.ResetLockSet)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: WeaponManager.sharedManager.ResetLockSet point 1");
						}
						_003CcurrentPullResult_003E5__3 = _003CcurrentPullResult_003E5__3 ?? string.Empty;
						List<object> list = new List<object>();
						if (_003CcurrentPullResult_003E5__3 != string.Empty)
						{
							try
							{
								list = Json.Deserialize(_003CcurrentPullResult_003E5__3) as List<object>;
								if (list == null)
								{
									list = new List<object>();
								}
							}
							catch (Exception ex2)
							{
								UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in Deserialize: {0}", ex2);
							}
						}
						_003CcloudPurchases_003E5__5 = list.OfType<string>().ToList();
						_003ClocalPurchases_003E5__6 = (from itemId in ItemDb.AllItemIds()
							where itemId != null && Storager.getInt(itemId) > 0
							select itemId).ToList();
						if (CloudSyncController.CheckSettingsOfLobbyBackground | skipApplyingToLocalState)
						{
							_003ClocalPurchases_003E5__6.Add("Environment.Terrains.SpecificalTerrainsMarkedFlag");
							CloudSyncController.CheckSettingsOfLobbyBackground = false;
						}
						_003CnewPurchasesFromCloud_003E5__4 = _003CcloudPurchases_003E5__5.Except(_003ClocalPurchases_003E5__6).Except(new string[1] { "freeSpinsCount" });
						_003CpurchasesToSendToCloud_003E5__2 = _003ClocalPurchases_003E5__6.Except(_003CcloudPurchases_003E5__5).Except(new string[1] { "freeSpinsCount" });
						_003CwriteCounter_003E5__1 = 0;
						_003C_003E7__wrap1 = _003CnewPurchasesFromCloud_003E5__4.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0277;
					}
					case 1:
						_003C_003E1__state = -3;
						if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.ResetLockSet)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: WeaponManager.sharedManager.ResetLockSet point 1");
						}
						goto IL_0277;
					case 2:
						{
							_003C_003E1__state = -1;
							return false;
						}
						IL_0277:
						while (_003C_003E7__wrap1.MoveNext())
						{
							string current = _003C_003E7__wrap1.Current;
							if (skipApplyingToLocalState)
							{
								break;
							}
							if (!(current == "Environment.Terrains.SpecificalTerrainsMarkedFlag"))
							{
								Storager.setInt(current, 1);
								_003CwriteCounter_003E5__1++;
								if (_003CwriteCounter_003E5__1 % 15 == 0)
								{
									_003C_003E2__current = null;
									_003C_003E1__state = 1;
									return true;
								}
							}
						}
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = null;
						flag = _003CpurchasesToSendToCloud_003E5__2.Count() > 0;
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.LogFormat("PurchasesCloudApplyer: Succeeded to apply purchases:\n'currentPullResult':{0},\n'newPurchasesFromCloud':{1},\n'purchasesToSendToCloud':{2},\n'cloudDirty':{3},\n'cloudPurchases':{4},\n'localPurchases':{5}", _003CcurrentPullResult_003E5__3, Json.Serialize(_003CnewPurchasesFromCloud_003E5__4.ToList()), Json.Serialize(_003CpurchasesToSendToCloud_003E5__2.ToList()), flag, Json.Serialize(_003CcloudPurchases_003E5__5), Json.Serialize(_003ClocalPurchases_003E5__6));
						}
						try
						{
							if (flag)
							{
								List<string> obj = _003CcloudPurchases_003E5__5.Union(_003CpurchasesToSendToCloud_003E5__2).ToList();
								_003C_003E4__this.SlotSynchronizer.Push(Json.Serialize(obj));
							}
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in sending to cloud: {0}", ex);
						}
						if (skipApplyingToLocalState)
						{
							return false;
						}
						_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(DoCustomActions(_003CnewPurchasesFromCloud_003E5__4));
						_003C_003E1__state = 2;
						return true;
					}
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void _003C_003Em__Finally1()
			{
				_003C_003E1__state = -1;
				if (_003C_003E7__wrap1 != null)
				{
					_003C_003E7__wrap1.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		internal sealed class _003CDoCustomActions_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private int _003ClevelBefore_003E5__1;

			public IEnumerable<string> newPurchasesFromCloud;

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
			public _003CDoCustomActions_003Ed__2(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					try
					{
						WeaponManager.GivePreviousUpgradesOfCrystalSword();
					}
					catch (Exception ex4)
					{
						UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in GivePreviousUpgradesOfCrystalSword: {0}", ex4);
					}
					_003ClevelBefore_003E5__1 = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
					try
					{
						ExperienceController.RefreshExpControllers();
						ExperienceController.SendAnalyticsForLevelsFromCloud(_003ClevelBefore_003E5__1);
					}
					catch (Exception ex5)
					{
						UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in refreshing exp controllers and sending analytics: {0}", ex5);
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				case 1:
				{
					_003C_003E1__state = -1;
					if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > _003ClevelBefore_003E5__1)
					{
						MainMenuController.MarkAllMiniGamesAsShown();
					}
					IEnumerable<string> source = newPurchasesFromCloud.Intersect(WeaponManager.GotchaGuns.Concat(new List<string> { Defs.IsFacebookLoginRewardaGained }));
					WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(source.ToList());
					if (source.Count() > 10)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0118;
				}
				case 2:
					_003C_003E1__state = -1;
					goto IL_0118;
				case 3:
					{
						_003C_003E1__state = -1;
						try
						{
							WeaponManager.FixFirstTags();
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in WeaponManager.FixFirstTags: {0}", ex);
						}
						try
						{
							GadgetsInfo.FixFirstsForOurTier();
						}
						catch (Exception ex2)
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in GadgetsInfo.FixFirstsForOurTier: {0}", ex2);
						}
						if (PromoActionsManager.sharedManager != null)
						{
							int lev = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
							int num = ExpController.TierForLevel(_003ClevelBefore_003E5__1);
							if (ExpController.TierForLevel(lev) > num)
							{
								PromoActionsManager.sharedManager.ReplaceUnlockedItemsWith(new List<string>());
								PromoActionsManager.sharedManager.RemoveViewedUnlockedItems();
							}
							else
							{
								foreach (string item in newPurchasesFromCloud)
								{
									PromoActionsManager.sharedManager.RemoveItemFromUnlocked(item);
								}
							}
							PromoActionsManager.FireUnlockedItemsUpdated();
						}
						else
						{
							UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: DoCustomActions: PromoActionsManager.sharedManager == null");
						}
						try
						{
							if (PlayerPanel.instance != null)
							{
								PlayerPanel.instance.UpdateExp();
							}
						}
						catch (Exception ex3)
						{
							UnityEngine.Debug.LogErrorFormat("Exception in DoCustomActions, updating PlayerPanel: {0}", ex3);
						}
						return false;
					}
					IL_0118:
					foreach (string item2 in newPurchasesFromCloud)
					{
						try
						{
							ItemRecord value;
							if (!ItemDb.allRecordsWithStorageIds.TryGetValue(item2, out value) || !WeaponManager.RemoveGunFromAllTryGunRelated(value.Tag))
							{
								continue;
							}
							try
							{
								if (ABTestController.useBuffSystem)
								{
									BuffSystem.instance.RemoveGunBuff();
								}
								else
								{
									KillRateCheck.RemoveGunBuff();
								}
							}
							catch
							{
							}
						}
						catch
						{
						}
					}
					try
					{
						if (!TrainingController.TrainingCompleted && ((ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1) || newPurchasesFromCloud.Contains("Armor_Army_1")))
						{
							TrainingController.OnGetProgress();
						}
					}
					catch (Exception ex6)
					{
						UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in trying to complete training: {0}", ex6);
					}
					try
					{
						if (HintController.instance != null)
						{
							HintController.instance.ShowNext();
						}
					}
					catch (Exception ex7)
					{
						UnityEngine.Debug.LogErrorFormat("Exception in PurchasesCloudApplyer HintController.instance.ShowNext(): {0}", ex7);
					}
					try
					{
						WeaponManager.sharedManager.ReequipItemsAfterCloudSync();
					}
					catch (Exception ex8)
					{
						UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in ReequipItemsAfterCloudSync: {0}", ex8);
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
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

		public PurchasesCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentPullResult = SlotSynchronizer.CurrentResult;
			if (currentPullResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer currentPullResult == null");
			}
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.ResetLockSet)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: WeaponManager.sharedManager.ResetLockSet point 1");
			}
			currentPullResult = currentPullResult ?? string.Empty;
			List<object> list = new List<object>();
			if (currentPullResult != string.Empty)
			{
				try
				{
					list = Json.Deserialize(currentPullResult) as List<object>;
					if (list == null)
					{
						list = new List<object>();
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in Deserialize: {0}", ex);
				}
			}
			List<string> cloudPurchases = list.OfType<string>().ToList();
			List<string> localPurchases = (from itemId in ItemDb.AllItemIds()
				where itemId != null && Storager.getInt(itemId) > 0
				select itemId).ToList();
			if (CloudSyncController.CheckSettingsOfLobbyBackground || skipApplyingToLocalState)
			{
				localPurchases.Add("Environment.Terrains.SpecificalTerrainsMarkedFlag");
				CloudSyncController.CheckSettingsOfLobbyBackground = false;
			}
			IEnumerable<string> newPurchasesFromCloud = cloudPurchases.Except(localPurchases).Except(new string[1] { "freeSpinsCount" });
			IEnumerable<string> purchasesToSendToCloud = localPurchases.Except(cloudPurchases).Except(new string[1] { "freeSpinsCount" });
			int writeCounter = 0;
			foreach (string item in newPurchasesFromCloud)
			{
				if (skipApplyingToLocalState)
				{
					break;
				}
				if (item == "Environment.Terrains.SpecificalTerrainsMarkedFlag")
				{
					continue;
				}
				Storager.setInt(item, 1);
				writeCounter++;
				if (writeCounter % 15 == 0)
				{
					yield return null;
					if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.ResetLockSet)
					{
						UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: WeaponManager.sharedManager.ResetLockSet point 1");
					}
				}
			}
			bool flag = purchasesToSendToCloud.Count() > 0;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("PurchasesCloudApplyer: Succeeded to apply purchases:\n'currentPullResult':{0},\n'newPurchasesFromCloud':{1},\n'purchasesToSendToCloud':{2},\n'cloudDirty':{3},\n'cloudPurchases':{4},\n'localPurchases':{5}", currentPullResult, Json.Serialize(newPurchasesFromCloud.ToList()), Json.Serialize(purchasesToSendToCloud.ToList()), flag, Json.Serialize(cloudPurchases), Json.Serialize(localPurchases));
			}
			try
			{
				if (flag)
				{
					List<string> obj = cloudPurchases.Union(purchasesToSendToCloud).ToList();
					SlotSynchronizer.Push(Json.Serialize(obj));
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in sending to cloud: {0}", ex2);
			}
			if (!skipApplyingToLocalState)
			{
				yield return CoroutineRunner.Instance.StartCoroutine(DoCustomActions(newPurchasesFromCloud));
			}
		}

		private static IEnumerator DoCustomActions(IEnumerable<string> newPurchasesFromCloud)
		{
			try
			{
				WeaponManager.GivePreviousUpgradesOfCrystalSword();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in GivePreviousUpgradesOfCrystalSword: {0}", ex);
			}
			int levelBefore = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
			try
			{
				ExperienceController.RefreshExpControllers();
				ExperienceController.SendAnalyticsForLevelsFromCloud(levelBefore);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in refreshing exp controllers and sending analytics: {0}", ex2);
			}
			yield return null;
			if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > levelBefore)
			{
				MainMenuController.MarkAllMiniGamesAsShown();
			}
			IEnumerable<string> source = newPurchasesFromCloud.Intersect(WeaponManager.GotchaGuns.Concat(new List<string> { Defs.IsFacebookLoginRewardaGained }));
			WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(source.ToList());
			if (source.Count() > 10)
			{
				yield return null;
			}
			foreach (string item in newPurchasesFromCloud)
			{
				try
				{
					ItemRecord value;
					if (!ItemDb.allRecordsWithStorageIds.TryGetValue(item, out value) || !WeaponManager.RemoveGunFromAllTryGunRelated(value.Tag))
					{
						continue;
					}
					try
					{
						if (ABTestController.useBuffSystem)
						{
							BuffSystem.instance.RemoveGunBuff();
						}
						else
						{
							KillRateCheck.RemoveGunBuff();
						}
					}
					catch
					{
					}
				}
				catch
				{
				}
			}
			try
			{
				if (!TrainingController.TrainingCompleted && ((ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1) || newPurchasesFromCloud.Contains("Armor_Army_1")))
				{
					TrainingController.OnGetProgress();
				}
			}
			catch (Exception ex3)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in trying to complete training: {0}", ex3);
			}
			try
			{
				if (HintController.instance != null)
				{
					HintController.instance.ShowNext();
				}
			}
			catch (Exception ex4)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in PurchasesCloudApplyer HintController.instance.ShowNext(): {0}", ex4);
			}
			try
			{
				WeaponManager.sharedManager.ReequipItemsAfterCloudSync();
			}
			catch (Exception ex5)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in ReequipItemsAfterCloudSync: {0}", ex5);
			}
			yield return null;
			try
			{
				WeaponManager.FixFirstTags();
			}
			catch (Exception ex6)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in WeaponManager.FixFirstTags: {0}", ex6);
			}
			try
			{
				GadgetsInfo.FixFirstsForOurTier();
			}
			catch (Exception ex7)
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: Exception in GadgetsInfo.FixFirstsForOurTier: {0}", ex7);
			}
			if (PromoActionsManager.sharedManager != null)
			{
				int lev = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
				int num = ExpController.TierForLevel(levelBefore);
				if (ExpController.TierForLevel(lev) > num)
				{
					PromoActionsManager.sharedManager.ReplaceUnlockedItemsWith(new List<string>());
					PromoActionsManager.sharedManager.RemoveViewedUnlockedItems();
				}
				else
				{
					foreach (string item2 in newPurchasesFromCloud)
					{
						PromoActionsManager.sharedManager.RemoveItemFromUnlocked(item2);
					}
				}
				PromoActionsManager.FireUnlockedItemsUpdated();
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat("PurchasesCloudApplyer: DoCustomActions: PromoActionsManager.sharedManager == null");
			}
			try
			{
				if (PlayerPanel.instance != null)
				{
					PlayerPanel.instance.UpdateExp();
				}
			}
			catch (Exception ex8)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DoCustomActions, updating PlayerPanel: {0}", ex8);
			}
		}
	}
}
