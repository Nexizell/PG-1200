using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class TrophiesCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TrophiesCloudApplyer _003C_003E4__this;

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
					UnityEngine.Debug.LogErrorFormat("TrophiesCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("TrophiesCloudApplyer currentPullResult == null");
				}
				if (skipApplyingToLocalState)
				{
					TrophiesMemento trophiesMemento = new TrophiesMemento(0, 0, 0, false);
					_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(trophiesMemento));
					return false;
				}
				if (Defs.IsDeveloperBuild && !FriendsController.sharedController.CurrentServerSeason.HasValue)
				{
					UnityEngine.Debug.Log("Current season not received yet, waiting...");
				}
				if (!skipApplyingToLocalState && !FriendsController.sharedController.CurrentServerSeason.HasValue)
				{
					return false;
				}
				try
				{
					currentResult = currentResult ?? string.Empty;
					TrophiesMemento trophiesMemento2 = ((currentResult != string.Empty) ? JsonUtility.FromJson<TrophiesMemento>(currentResult) : new TrophiesMemento(0, 0, 0, false));
					if (FriendsController.sharedController.CurrentServerSeason.HasValue && trophiesMemento2.Season > FriendsController.sharedController.CurrentServerSeason.Value + 1)
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.LogErrorFormat("Cloud season is too high. cloudSeason: {0}, serverSeason: {1}", trophiesMemento2.Season, FriendsController.sharedController.CurrentServerSeason.Value);
						}
						return false;
					}
					int negativeRating = RatingSystem.instance.negativeRating;
					int positiveRating = RatingSystem.instance.positiveRating;
					int currentCompetition = FriendsController.sharedController.currentCompetition;
					bool flag = false;
					bool flag2 = false;
					if (trophiesMemento2.Season == 0)
					{
						if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
						{
							flag2 = true;
						}
						else
						{
							int num = positiveRating;
							if (trophiesMemento2.TrophiesPositive > positiveRating)
							{
								num = trophiesMemento2.TrophiesPositive;
								RatingSystem.instance.positiveRating = num;
								flag = true;
							}
							else if (trophiesMemento2.TrophiesPositive < positiveRating)
							{
								flag2 = true;
							}
							int num2 = negativeRating;
							if (trophiesMemento2.TrophiesNegative > negativeRating)
							{
								num2 = Math.Min(trophiesMemento2.TrophiesNegative, RatingSystem.instance.positiveRating);
								RatingSystem.instance.negativeRating = num2;
								flag = true;
							}
							else if (trophiesMemento2.TrophiesNegative < negativeRating)
							{
								flag2 = true;
							}
							int num3 = num - num2;
							int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
							if (num3 > trophiesSeasonThreshold)
							{
								int num4 = num3 - trophiesSeasonThreshold;
								num2 += num4;
								RatingSystem.instance.negativeRating = num2;
								flag = true;
								flag2 = true;
								TournamentAvailableBannerWindow.CanShow = true;
							}
						}
					}
					else if (trophiesMemento2.Season > currentCompetition)
					{
						FriendsController.sharedController.currentCompetition = trophiesMemento2.Season;
						RatingSystem.instance.negativeRating = trophiesMemento2.TrophiesNegative;
						RatingSystem.instance.positiveRating = trophiesMemento2.TrophiesPositive;
						flag = true;
					}
					else if (trophiesMemento2.Season == currentCompetition)
					{
						int num5 = positiveRating;
						if (trophiesMemento2.TrophiesPositive > positiveRating)
						{
							num5 = trophiesMemento2.TrophiesPositive;
							RatingSystem.instance.positiveRating = num5;
							flag = true;
						}
						else if (trophiesMemento2.TrophiesPositive < positiveRating)
						{
							flag2 = true;
						}
						int num6 = negativeRating;
						if (trophiesMemento2.TrophiesNegative > negativeRating)
						{
							num6 = Math.Min(trophiesMemento2.TrophiesNegative, RatingSystem.instance.positiveRating);
							RatingSystem.instance.negativeRating = num6;
							flag = true;
						}
						else if (trophiesMemento2.TrophiesNegative < negativeRating)
						{
							flag2 = true;
						}
					}
					else
					{
						flag2 = true;
					}
					if (flag && !skipApplyingToLocalState)
					{
						RatingSystem.instance.UpdateLeagueEvent();
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("TrophiesCloudApplyer: Succeeded to apply trophies:\n'currentPullResult':{0},\n'localTrophiesNegative':{1},\n'localTrophiesPositive':{2},\n'cloudTrophies':{3},\n'conflicted':{4},\n'localDirty':{5},\n'cloudDirty':{6}", currentResult, negativeRating, positiveRating, trophiesMemento2, trophiesMemento2.Conflicted, flag, flag2);
					}
					if (flag2)
					{
						TrophiesMemento trophiesMemento3 = new TrophiesMemento(RatingSystem.instance.negativeRating, RatingSystem.instance.positiveRating, FriendsController.sharedController.currentCompetition, false);
						_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(trophiesMemento3));
					}
					if (flag && !skipApplyingToLocalState)
					{
						try
						{
							if (PlayerPanel.instance != null)
							{
								PlayerPanel.instance.UpdateRating();
							}
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogErrorFormat("Exception in TrophiesCloudApplyer, updating PlayerPanel: {0}", ex);
						}
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in TrophiesCloudApplyer.Apply: {0}", ex2);
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

		public TrophiesCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("TrophiesCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("TrophiesCloudApplyer currentPullResult == null");
			}
			if (skipApplyingToLocalState)
			{
				TrophiesMemento trophiesMemento = new TrophiesMemento(0, 0, 0, false);
				SlotSynchronizer.Push(JsonUtility.ToJson(trophiesMemento));
				yield break;
			}
			if (Defs.IsDeveloperBuild && !FriendsController.sharedController.CurrentServerSeason.HasValue)
			{
				UnityEngine.Debug.Log("Current season not received yet, waiting...");
			}
			if (!skipApplyingToLocalState && !FriendsController.sharedController.CurrentServerSeason.HasValue)
			{
				yield break;
			}
			try
			{
				currentResult = currentResult ?? string.Empty;
				TrophiesMemento trophiesMemento2 = ((currentResult != string.Empty) ? JsonUtility.FromJson<TrophiesMemento>(currentResult) : new TrophiesMemento(0, 0, 0, false));
				if (FriendsController.sharedController.CurrentServerSeason.HasValue && trophiesMemento2.Season > FriendsController.sharedController.CurrentServerSeason.Value + 1)
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogErrorFormat("Cloud season is too high. cloudSeason: {0}, serverSeason: {1}", trophiesMemento2.Season, FriendsController.sharedController.CurrentServerSeason.Value);
					}
					yield break;
				}
				int negativeRating = RatingSystem.instance.negativeRating;
				int positiveRating = RatingSystem.instance.positiveRating;
				int currentCompetition = FriendsController.sharedController.currentCompetition;
				bool flag = false;
				bool flag2 = false;
				if (trophiesMemento2.Season == 0)
				{
					if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
					{
						flag2 = true;
					}
					else
					{
						int num = positiveRating;
						if (trophiesMemento2.TrophiesPositive > positiveRating)
						{
							num = trophiesMemento2.TrophiesPositive;
							RatingSystem.instance.positiveRating = num;
							flag = true;
						}
						else if (trophiesMemento2.TrophiesPositive < positiveRating)
						{
							flag2 = true;
						}
						int num2 = negativeRating;
						if (trophiesMemento2.TrophiesNegative > negativeRating)
						{
							num2 = Math.Min(trophiesMemento2.TrophiesNegative, RatingSystem.instance.positiveRating);
							RatingSystem.instance.negativeRating = num2;
							flag = true;
						}
						else if (trophiesMemento2.TrophiesNegative < negativeRating)
						{
							flag2 = true;
						}
						int num3 = num - num2;
						int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
						if (num3 > trophiesSeasonThreshold)
						{
							int num4 = num3 - trophiesSeasonThreshold;
							num2 += num4;
							RatingSystem.instance.negativeRating = num2;
							flag = true;
							flag2 = true;
							TournamentAvailableBannerWindow.CanShow = true;
						}
					}
				}
				else if (trophiesMemento2.Season > currentCompetition)
				{
					FriendsController.sharedController.currentCompetition = trophiesMemento2.Season;
					RatingSystem.instance.negativeRating = trophiesMemento2.TrophiesNegative;
					RatingSystem.instance.positiveRating = trophiesMemento2.TrophiesPositive;
					flag = true;
				}
				else if (trophiesMemento2.Season == currentCompetition)
				{
					if (trophiesMemento2.TrophiesPositive > positiveRating)
					{
						int trophiesPositive = trophiesMemento2.TrophiesPositive;
						RatingSystem.instance.positiveRating = trophiesPositive;
						flag = true;
					}
					else if (trophiesMemento2.TrophiesPositive < positiveRating)
					{
						flag2 = true;
					}
					if (trophiesMemento2.TrophiesNegative > negativeRating)
					{
						int negativeRating2 = Math.Min(trophiesMemento2.TrophiesNegative, RatingSystem.instance.positiveRating);
						RatingSystem.instance.negativeRating = negativeRating2;
						flag = true;
					}
					else if (trophiesMemento2.TrophiesNegative < negativeRating)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
				if (flag && !skipApplyingToLocalState)
				{
					RatingSystem.instance.UpdateLeagueEvent();
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("TrophiesCloudApplyer: Succeeded to apply trophies:\n'currentPullResult':{0},\n'localTrophiesNegative':{1},\n'localTrophiesPositive':{2},\n'cloudTrophies':{3},\n'conflicted':{4},\n'localDirty':{5},\n'cloudDirty':{6}", currentResult, negativeRating, positiveRating, trophiesMemento2, trophiesMemento2.Conflicted, flag, flag2);
				}
				if (flag2)
				{
					TrophiesMemento trophiesMemento3 = new TrophiesMemento(RatingSystem.instance.negativeRating, RatingSystem.instance.positiveRating, FriendsController.sharedController.currentCompetition, false);
					SlotSynchronizer.Push(JsonUtility.ToJson(trophiesMemento3));
				}
				if (!flag || skipApplyingToLocalState)
				{
					yield break;
				}
				try
				{
					if (PlayerPanel.instance != null)
					{
						PlayerPanel.instance.UpdateRating();
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in TrophiesCloudApplyer, updating PlayerPanel: {0}", ex);
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in TrophiesCloudApplyer.Apply: {0}", ex2);
			}
		}
	}
}
