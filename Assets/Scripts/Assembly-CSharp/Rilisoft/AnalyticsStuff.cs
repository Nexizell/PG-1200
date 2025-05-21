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
	public class AnalyticsStuff
	{
		public enum LogTrafficForwardingMode
		{
			Show = 0,
			Press = 1
		}

		[CompilerGenerated]
		internal sealed class _003CWaitInitializationThenLogGameDayCountCoroutine_003Ed__62 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

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
			public _003CWaitInitializationThenLogGameDayCountCoroutine_003Ed__62(int _003C_003E1__state)
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
					_003C_003E2__current = new WaitUntil(() => AnalyticsFacade.FacebookFacade != null);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					LogGameDayCount();
					return false;
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

		private const string dailyGiftEventNameBase = "Daily Gift";

		private const string WeaponsSpecialOffersEvent = "Weapons Special Offers";

		private static int trainingStep = -1;

		private static bool trainingStepLoaded = false;

		private static string trainingProgressKey = "TrainingStepKeyAnalytics";

		public static int TrainingStep
		{
			get
			{
				LoadTrainingStep();
				return trainingStep;
			}
		}

		internal static void DeathEscapeDeath(float distance)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Death"] = ((int)Math.Round((double)distance / 10.0) * 10).ToString();
				AnalyticsFacade.SendCustomEvent("Death Escape", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeDeath: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void DeathEscapeCheckpointUsed(string value)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Checkpoint Used"] = value;
				AnalyticsFacade.SendCustomEvent("Death Escape", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeCheckpointUsed: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void DeathEscapeLapTime(float time)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Lap Time"] = ((int)Math.Round((double)time / 10.0) * 10).ToString();
				AnalyticsFacade.SendCustomEvent("Death Escape", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeLapTime: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void DeathEscapeRestart(string value)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Restart"] = value;
				AnalyticsFacade.SendCustomEvent("Death Escape", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeRestart: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void DeathEscapeAds(bool isShow)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Ads"] = (isShow ? "Show" : "Completed");
				AnalyticsFacade.SendCustomEvent("Death Escape", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in DeathEscapeAds: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void SpeedrunDistance(int distance)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Distance"] = ((int)Math.Round((double)distance / 50.0) * 50).ToString();
				AnalyticsFacade.SendCustomEvent("Speed Run", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpeedrunDistance: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void SpeedrunGear(string gearId)
		{
			if (gearId.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogErrorFormat("SpeedrunGear gearId.isNullOrEmpty");
				return;
			}
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Gear"] = gearId;
				AnalyticsFacade.SendCustomEvent("Speed Run", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpeedrunGear: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void FirstWinOfTheDayGameModesCompleted(GameConnect.GameMode gameMode)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Game Modes"] = gameMode.ToString() + " x2 Completed";
				dictionary["Total"] = "Completed";
				AnalyticsFacade.SendCustomEvent("First Win Of The Day Total", dictionary);
				AnalyticsFacade.SendCustomEvent("First Win Of The Day" + GetPayingSuffixNo10(), dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in FirstWinOfTheDayGameModesCompleted: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void FirstWinOfTheDayGameModesStart(GameConnect.GameMode gameMode)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Game Modes"] = gameMode.ToString() + " x2";
				dictionary["Total"] = "x2";
				AnalyticsFacade.SendCustomEvent("First Win Of The Day Total", dictionary);
				AnalyticsFacade.SendCustomEvent("First Win Of The Day" + GetPayingSuffixNo10(), dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in FirstWinOfTheDayGameModesStart: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void SpleefTime(float time)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Time"] = ((int)Math.Round((double)time / 10.0) * 10).ToString();
				AnalyticsFacade.SendCustomEvent("Spleef", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpleefTime: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void SpleefWeaponsDeath(string weaponId)
		{
			if (weaponId.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogErrorFormat("SpleefWeaponsDeath weaponId.isNullOrEmpty");
				return;
			}
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Weapons Death"] = weaponId;
				AnalyticsFacade.SendCustomEvent("Spleef", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpleefWeaponsDeath: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void SpleefWeaponsSurvive(string weaponId)
		{
			if (weaponId.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogErrorFormat("SpleefWeaponsSurvive weaponId.isNullOrEmpty");
				return;
			}
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Weapons Survive"] = weaponId;
				AnalyticsFacade.SendCustomEvent("Spleef", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SpleefWeaponsSurvive: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void MiniGamesSales(string itemId, bool isGear)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary[isGear ? "Gear" : "Tickets"] = itemId;
				AnalyticsFacade.SendCustomEvent("Mini Games Sales Total", dictionary);
				AnalyticsFacade.SendCustomEvent("Mini Games Sales" + GetPayingSuffixNo10(), dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in MiniGamesSales: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void MiniGamesRating(int rating, GameConnect.GameMode mode)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary[mode.ToString()] = rating.ToString();
				AnalyticsFacade.SendCustomEvent("Mini Games Rating", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in MiniGamesRating: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void HousingSettings(MainMenuHeroCamera.CameraPositionPreset preset)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Camera"] = ((preset == MainMenuHeroCamera.CameraPositionPreset.Common) ? "Normal" : preset.ToString());
				AnalyticsFacade.SendCustomEvent("Housing Settings", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in HousingSettings: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void CustomSpecialOffersSalesBuy(string inappId)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Buy"] = inappId;
				AnalyticsFacade.SendCustomEvent("Custom Special Offers Sales", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in CustomSpecialOffersSalesBuy: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void CustomSpecialOffersSalesShown(string inappId)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Show"] = inappId;
				AnalyticsFacade.SendCustomEvent("Custom Special Offers Sales", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in CustomSpecialOffersSalesShown: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void AllCombatActivity(string mode)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Total"] = mode;
				AnalyticsFacade.SendCustomEvent("All Combat Activity", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in AllCombatActivity: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void MiniGames(GameConnect.GameMode miniGame)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				dictionary["Total"] = miniGame.ToString();
				dictionary[string.Format("{0} By Tier", new object[1] { miniGame.ToString() })] = (ExpController.OurTierForAnyPlace() + 1).ToString();
				AnalyticsFacade.SendCustomEvent("Mini Games", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in MiniGames: {0}", ex);
			}
			ParametersCache.Release(dictionary);
			Dictionary<string, object> dictionary2 = ParametersCache.Acquire();
			try
			{
				dictionary2[string.Format("Tier {0}", new object[1] { ExpController.OurTierForAnyPlace() + 1 })] = miniGame.ToString();
				AnalyticsFacade.SendCustomEvent("Mini Games By Tier", dictionary2);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in Mini Games By Tier: {0}", ex2);
			}
			ParametersCache.Release(dictionary2);
			Dictionary<string, object> dictionary3 = ParametersCache.Acquire();
			try
			{
				dictionary3[RatingSystem.instance.currentLeague.ToString()] = miniGame.ToString();
				AnalyticsFacade.SendCustomEvent("Mini Games By Leagues", dictionary3);
			}
			catch (Exception ex3)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in Mini Games By Leagues: {0}", ex3);
			}
			ParametersCache.Release(dictionary3);
			AllCombatActivity("Mini Games");
		}

		internal static void TicketsSpended(string item, int priceInTickets)
		{
			Dictionary<string, object> dictionary = ParametersCache.Acquire();
			try
			{
				int earnedTickets;
				int purchasedTickets;
				BankController.GetCurrentTickets(out earnedTickets, out purchasedTickets);
				int num = priceInTickets / 2 + ((priceInTickets % 2 == 1) ? 1 : 0);
				if (purchasedTickets >= num)
				{
					dictionary["Bought"] = item;
				}
				else
				{
					dictionary["Free"] = item;
				}
				AnalyticsFacade.SendCustomEvent("Tickets Spended", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in TicketsSpended: {0}", ex);
			}
			ParametersCache.Release(dictionary);
		}

		internal static void TrySendOnceToFacebook(string eventName, Lazy<Dictionary<string, object>> eventParams, Version excludeVersion)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (excludeVersion != null)
			{
				try
				{
					if (new Version(Switcher.InitialAppVersion) <= excludeVersion)
					{
						return;
					}
				}
				catch
				{
				}
			}
			string key = "Analytics:" + eventName;
			if (!Storager.hasKey(key) || string.IsNullOrEmpty(Storager.getString(key)))
			{
				Storager.setString(key, "True");
				Dictionary<string, object> parameters = ((eventParams != null) ? eventParams.Value : null);
				AnalyticsFacade.SendCustomEventToFacebook(eventName, null, parameters, false);
			}
		}

		internal static void TrySendOnceToAppsFlyer(string eventName, Lazy<Dictionary<string, string>> eventParams, Version excludeVersion)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (excludeVersion == null)
			{
				throw new ArgumentNullException("excludeVersion");
			}
			try
			{
				if (new Version(Switcher.InitialAppVersion) <= excludeVersion)
				{
					return;
				}
			}
			catch
			{
				return;
			}
			string key = "Analytics:" + eventName;
			if (!Storager.hasKey(key) || string.IsNullOrEmpty(Storager.getString(key)))
			{
				Storager.setString(key, Json.Serialize(eventParams));
				AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams.Value);
			}
		}

		public static void TrySendOnceToAppsFlyer(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			string key = "Analytics:" + eventName;
			if (!Storager.hasKey(key) || string.IsNullOrEmpty(Storager.getString(key)))
			{
				Storager.setString(key, "{}");
				AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, new Dictionary<string, string>());
			}
		}

		internal static void SendInGameDay(int newInGameDayIndex)
		{
			if (!(new Version(Switcher.InitialAppVersion) <= new Version(11, 2, 3, 0)))
			{
				Dictionary<string, object> dictionary = ParametersCache.Acquire();
				dictionary.Add("Day", newInGameDayIndex.ToString(CultureInfo.InvariantCulture));
				AnalyticsFacade.SendCustomEvent("InGameDay", dictionary);
				ParametersCache.Release(dictionary);
			}
		}

		public static void LogCampaign(string map, string boxName)
		{
			try
			{
				if (string.IsNullOrEmpty(map))
				{
					UnityEngine.Debug.LogError("LogCampaign string.IsNullOrEmpty(map)");
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object> { { "Maps", map } };
				if (boxName != null)
				{
					dictionary.Add("Boxes", boxName);
				}
				AnalyticsFacade.SendCustomEvent("Campaign", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogCampaign: " + ex);
			}
		}

		public static void LogMultiplayer()
		{
			try
			{
				string text = GameConnect.gameMode.ToString();
				if (GameConnect.isDaterRegim)
				{
					text = "Sandbox";
				}
				if (text == null)
				{
					UnityEngine.Debug.LogError("LogMultiplayer modeName == null");
					return;
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{ "Game Modes", text },
					{
						text + " By Tier",
						ExpController.OurTierForAnyPlace() + 1
					}
				};
				AnalyticsFacade.SendCustomEvent("Multiplayer", eventParams);
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				try
				{
					int indexMap = Convert.ToInt32(PhotonNetwork.room.customProperties[GameConnect.mapProperty]);
					SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(indexMap);
					dictionary["Total"] = infoScene.NameScene;
					dictionary[text] = infoScene.NameScene;
				}
				catch (Exception ex)
				{
					dictionary["Error"] = ((object)ex).GetType().Name;
					UnityEngine.Debug.LogException(ex);
				}
				AnalyticsFacade.SendCustomEvent("Maps", dictionary);
				Dictionary<string, object> eventParams2 = new Dictionary<string, object> { 
				{
					"Tier " + (ExpController.OurTierForAnyPlace() + 1),
					text
				} };
				AnalyticsFacade.SendCustomEvent("Game Modes By Tier", eventParams2);
				Dictionary<string, object> eventParams3 = new Dictionary<string, object> { 
				{
					RatingSystem.instance.currentLeague.ToString(),
					text
				} };
				AnalyticsFacade.SendCustomEvent("Game Modes By Leagues", eventParams3);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError("Exception in LogMultiplayer: " + ex2);
			}
		}

		public static void LogSandboxTimeGamePopularity(int timeGame, bool isStart)
		{
			try
			{
				string key = ((timeGame == 5 || timeGame == 10 || timeGame == 15) ? ("Time " + timeGame) : "Other");
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					key,
					isStart ? "Start" : "End"
				} };
				AnalyticsFacade.SendCustomEvent("Sandbox", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Sandbox exception: " + ex);
			}
		}

		public static void LogFirstBattlesKillRate(int battleIndex, float killRate)
		{
			try
			{
				string text = "";
				text = ((killRate < 0.4f) ? "<0,4" : ((killRate < 0.6f) ? "0,4 - 0,6" : ((killRate < 0.8f) ? "0,6 - 0,8" : ((killRate < 1f) ? "0,8 - 1" : ((killRate < 1.2f) ? "1 - 1,2" : ((killRate < 1.5f) ? "1,2 - 1,5" : ((killRate < 2f) ? "1,5 - 2" : ((!(killRate < 3f)) ? ">3" : "2 - 3"))))))));
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"Battle " + battleIndex,
					text
				} };
				AnalyticsFacade.SendCustomEvent("First Battles KillRate", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogFirstBattlesKillRate: " + ex);
			}
		}

		public static void LogFirstBattlesResult(int battleIndex, bool winner)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"Battle " + battleIndex,
					winner ? "Win" : "Lose"
				} };
				AnalyticsFacade.SendCustomEvent("First Battles Result", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogFirstBattlesResult: " + ex);
			}
		}

		public static void LogABTest(string nameTest, string nameCohort, bool isStart = true)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					nameTest,
					isStart ? nameCohort : ("Excluded " + nameCohort)
				} };
				AnalyticsFacade.SendCustomEvent("A/B Test", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("A/B Test exception: " + ex);
			}
		}

		public static void LogArenaWavesPassed(int countWaveComplite)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"Waves Passed",
					(countWaveComplite < 9) ? countWaveComplite.ToString() : ">=9"
				} };
				AnalyticsFacade.SendCustomEvent("Arena", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("ArenaFirst  exception: " + ex);
			}
		}

		public static void LogArenaFirst(bool isPause, bool isMoreOneWave)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"First",
					isPause ? "Quit" : (isMoreOneWave ? "Complete" : "Fail")
				} };
				AnalyticsFacade.SendCustomEvent("Arena", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("ArenaFirst  exception: " + ex);
			}
		}

		public static void Tutorial(AnalyticsConstants.TutorialState step, int count_battle = 0)
		{
			try
			{
				LoadTrainingStep();
				if ((int)step <= trainingStep && ((step != AnalyticsConstants.TutorialState.Battle_Start && step != AnalyticsConstants.TutorialState.Battle_End) || trainingStep >= 90))
				{
					return;
				}
				int num = trainingStep;
				trainingStep = (int)step;
				string text = trainingStep + "_" + step;
				if (step == AnalyticsConstants.TutorialState.Get_Progress)
				{
					text = trainingStep.ToString() + "_" + num + "_" + step.ToString();
				}
				if (step == AnalyticsConstants.TutorialState.Battle_Start || step == AnalyticsConstants.TutorialState.Battle_End)
				{
					if (step == AnalyticsConstants.TutorialState.Battle_Start)
					{
						int @int = PlayerPrefs.GetInt("SendingStartButtle", -1);
						if (count_battle <= @int)
						{
							return;
						}
						PlayerPrefs.SetInt("SendingStartButtle", count_battle);
					}
					text = trainingStep.ToString() + "_" + count_battle + "_" + step.ToString();
				}
				FriendsController.SendToturialEvent((int)step, text);
				AnalyticsFacade.Tutorial(step);
				AnalyticsFacade.SendCustomEvent("Tutorial", new Dictionary<string, object> { { "Progress", text } });
				switch (step)
				{
				case AnalyticsConstants.TutorialState.Portal:
					AnalyticsFacade.SendCustomEventToFacebook("training_controls", null);
					break;
				case AnalyticsConstants.TutorialState.Connect_Scene:
					AnalyticsFacade.SendCustomEventToFacebook("training_armory", null);
					break;
				case AnalyticsConstants.TutorialState.Finished:
					AnalyticsFacade.SendCustomEventToFacebook("training_completed", null);
					break;
				}
				if (step > AnalyticsConstants.TutorialState.Portal)
				{
					SaveTrainingStep();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in Tutorial: " + ex);
			}
		}

		public static void SaveTrainingStep()
		{
			if (trainingStepLoaded)
			{
				Storager.setInt(trainingProgressKey, trainingStep);
			}
		}

		public static void LogDailyGiftPurchases(string packId)
		{
			try
			{
				if (string.IsNullOrEmpty(packId))
				{
					UnityEngine.Debug.LogError("LogDailyGiftPurchases: string.IsNullOrEmpty(packId)");
					return;
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"Purchases",
					ReadableNameForInApp(packId)
				} };
				AnalyticsFacade.SendCustomEvent("Daily Gift Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Daily Gift" + GetPayingSuffixNo10(), eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogDailyGiftPurchases: " + ex);
			}
		}

		public static void LogDailyGift(string giftId, GiftCategoryType giftCategoryType, int count, bool isForMoneyGift)
		{
			try
			{
				if (string.IsNullOrEmpty(giftId))
				{
					UnityEngine.Debug.LogError("LogDailyGift: string.IsNullOrEmpty(giftId)");
					return;
				}
				if (SkinsController.shopKeyFromNameSkin.ContainsKey(giftId))
				{
					giftId = "Skin";
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{ "Chance", giftId },
					{
						"Spins",
						isForMoneyGift ? "Paid" : "Free"
					}
				};
				AnalyticsFacade.SendCustomEvent("Daily Gift Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Daily Gift" + GetPayingSuffixNo10(), eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogDailyGift: " + ex);
			}
		}

		public static void LogTrafficForwarding(LogTrafficForwardingMode mode)
		{
			try
			{
				string text = ((mode == LogTrafficForwardingMode.Show) ? "Button Show" : "Button Pressed");
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{ "Conversion", text },
					{
						text + " Levels",
						(!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel
					},
					{
						text + " Tiers",
						ExpController.OurTierForAnyPlace() + 1
					},
					{
						text + " Paying",
						StoreKitEventListener.IsPayingUser() ? "TRUE" : "FALSE"
					}
				};
				AnalyticsFacade.SendCustomEvent("Pereliv Button", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogTrafficForwarding: " + ex);
			}
		}

		public static void LogWEaponsSpecialOffers_MoneySpended(string packId)
		{
			try
			{
				if (string.IsNullOrEmpty(packId))
				{
					UnityEngine.Debug.LogError("LogWEaponsSpecialOffers_MoneySpended: string.IsNullOrEmpty(packId)");
					return;
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object> { 
				{
					"Money Spended",
					ReadableNameForInApp(packId)
				} };
				AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Weapons Special Offers" + GetPayingSuffixNo10(), eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogWEaponsSpecialOffers_MoneySpended: " + ex);
			}
		}

		public static void LogWEaponsSpecialOffers_Conversion(bool show, string weaponId = null)
		{
			try
			{
				if (!show && string.IsNullOrEmpty(weaponId))
				{
					UnityEngine.Debug.LogError("LogWEaponsSpecialOffers_Conversion: string.IsNullOrEmpty(weaponId)");
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object> { 
				{
					"Conversion",
					show ? "Show" : "Buy"
				} };
				try
				{
					float num = (ABTestController.useBuffSystem ? BuffSystem.instance.GetKillrateByInteractions() : KillRateCheck.instance.GetKillRate());
					string text = ((num <= 0.5f) ? "Weak" : ((num <= 1.2f) ? "Normal" : "Strong"));
					string key = string.Format("Conversion {0} Players", new object[1] { text });
					if (!show)
					{
						dictionary.Add("Currency Spended", weaponId);
						dictionary.Add("Buy (Tier)", ExpController.OurTierForAnyPlace() + 1);
						dictionary.Add("Buy (Level)", (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
						dictionary.Add(key, "Buy");
					}
					else
					{
						dictionary.Add("Show (Tier)", ExpController.OurTierForAnyPlace() + 1);
						dictionary.Add("Show (Level)", (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
						dictionary.Add(key, "Show");
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in LogWEaponsSpecialOffers_Conversion adding paramters: " + ex);
				}
				AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", dictionary);
				AnalyticsFacade.SendCustomEvent("Weapons Special Offers" + GetPayingSuffixNo10(), dictionary);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError("Exception in LogWEaponsSpecialOffers_Conversion: " + ex2);
			}
		}

		public static string AnalyticsReadableCategoryNameFromOldCategoryName(string categoryParameterName)
		{
			categoryParameterName = NewSkinCategoryToOldSkinCategory(categoryParameterName);
			if (categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueHatsCategory) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory))
			{
				categoryParameterName = "League";
			}
			return categoryParameterName;
		}

		private static string NewSkinCategoryToOldSkinCategory(string categoryParameterName)
		{
			if (categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryMale) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryFemale) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategorySpecial) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryPremium) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryEditor) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueSkinsCategory))
			{
				categoryParameterName = "Skins";
			}
			return categoryParameterName;
		}

		public static void LogEggDelivery(string eggId)
		{
			if (string.IsNullOrEmpty(eggId))
			{
				UnityEngine.Debug.LogWarning("LogEggDelivery: egg id is empty.");
				return;
			}
			Dictionary<string, object> eventParams = new Dictionary<string, object>(1) { { "Eggs Delivery", eggId } };
			AnalyticsFacade.SendCustomEvent("Eggs Drop Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Eggs Drop" + GetPayingSuffixNo10(), eventParams);
		}

		public static void LogHatching(string petId, Egg egg)
		{
			if (egg == null)
			{
				UnityEngine.Debug.LogWarning("LogHatching: egg is null.");
				return;
			}
			string key = DetermineHatchingParameterName(egg);
			string value = petId ?? string.Empty;
			Dictionary<string, object> eventParams = new Dictionary<string, object> { { key, value } };
			AnalyticsFacade.SendCustomEvent("Eggs Drop Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Eggs Drop" + GetPayingSuffixNo10(), eventParams);
		}

		internal static void LogDailyVideoRewarded(int countWithinCurrentDay)
		{
			string value = countWithinCurrentDay.ToString(CultureInfo.InvariantCulture);
			Dictionary<string, object> dictionary = new Dictionary<string, object> { { "Rewarded", value } };
			if (countWithinCurrentDay == 1)
			{
				dictionary["Unique Users"] = "Rewarded";
			}
			AnalyticsFacade.SendCustomEvent("Ads Total", dictionary);
			AnalyticsFacade.SendCustomEvent("Ads" + GetPayingSuffixNo10(), dictionary);
		}

		internal static void LogInterstitialStarted(int countWithinCurrentDay)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object> { { "Interstitial", "Start" } };
			if (countWithinCurrentDay == 1)
			{
				dictionary["Unique Users"] = "Interstitial";
			}
			AnalyticsFacade.SendCustomEvent("Ads Total", dictionary);
			AnalyticsFacade.SendCustomEvent("Ads" + GetPayingSuffixNo10(), dictionary);
		}

		internal static void LogBattleInviteSent()
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object> { { "Conversion", "Send Invite" } };
			AnalyticsFacade.SendCustomEvent("Push Notifications", eventParams);
		}

		internal static void LogBattleInviteAccepted()
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object> { { "Conversion", "Accept Invite" } };
			AnalyticsFacade.SendCustomEvent("Push Notifications", eventParams);
		}

		private static string DetermineHatchingParameterName(Egg egg)
		{
			if (egg.HatchedType == EggHatchedType.Champion)
			{
				return "Drop Super Incubator";
			}
			if (egg.HatchedType == EggHatchedType.League)
			{
				return "Drop Champion";
			}
			string text = egg.Data.Rare.ToString();
			string text2 = egg.HatchedType.ToString();
			return string.Format("Drop {0} {1}", new object[2] { text, text2 });
		}

		public static void LogBestSales(string itemId, ShopNGUIController.CategoryNames category)
		{
			try
			{
				if (string.IsNullOrEmpty(itemId))
				{
					UnityEngine.Debug.LogError("LogBestSales: string.IsNullOrEmpty(itemId)");
					return;
				}
				string text = null;
				switch (category)
				{
				case ShopNGUIController.CategoryNames.BestWeapons:
					text = "Weapons";
					break;
				case ShopNGUIController.CategoryNames.BestWear:
					text = "Wear";
					break;
				case ShopNGUIController.CategoryNames.BestGadgets:
					text = "Gadgets";
					break;
				default:
					UnityEngine.Debug.LogErrorFormat("LogBestSales: incorrect category: {0}", category);
					return;
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object> { { text, itemId } };
				AnalyticsFacade.SendCustomEvent("Best Sales" + " Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Best Sales" + GetPayingSuffixNo10(), eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogBestSales: " + ex);
			}
		}

		public static void LogSales(string itemId, string categoryParameterName, bool isDaterWeapon = false)
		{
			try
			{
				if (string.IsNullOrEmpty(itemId))
				{
					UnityEngine.Debug.LogError("LogSales: string.IsNullOrEmpty(itemId)");
					return;
				}
				if (string.IsNullOrEmpty(categoryParameterName))
				{
					UnityEngine.Debug.LogError("LogSales: string.IsNullOrEmpty(categoryParameterName)");
					return;
				}
				categoryParameterName = AnalyticsReadableCategoryNameFromOldCategoryName(categoryParameterName);
				string salesCategory = SalesConstants.Instance.GetSalesCategory(categoryParameterName);
				Dictionary<string, object> dictionary = new Dictionary<string, object> { { categoryParameterName, itemId } };
				if (isDaterWeapon)
				{
					dictionary.Add("Dater Weapons", itemId);
				}
				AnalyticsFacade.SendCustomEvent(salesCategory + " Total", dictionary);
				AnalyticsFacade.SendCustomEvent(salesCategory + GetPayingSuffixNo10(), dictionary);
				if (StoreKitEventListener.IsPayingUser())
				{
					AnalyticsFacade.SendCustomEvent(salesCategory + " Tier " + (ExpController.OurTierForAnyPlace() + 1) + " " + GetPayingSuffixNo10(), dictionary);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogSales: " + ex);
			}
		}

		public static void RateUsFake(bool rate, int stars, bool sendNegativFeedback = false)
		{
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object> { 
				{
					"Efficiency",
					rate ? "Rate" : "Later"
				} };
				if (rate)
				{
					dictionary.Add("Rating (Stars)", stars);
				}
				if (stars > 0 && stars < 4)
				{
					dictionary.Add("Negative Feedback", sendNegativFeedback ? "Sended" : "Not sended");
				}
				AnalyticsFacade.SendCustomEvent("Rate Us Fake", dictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in RateUsFake: " + ex);
			}
		}

		public static string ReadableNameForInApp(string purchaseId)
		{
			if (!StoreKitEventListener.inAppsReadableNames.ContainsKey(purchaseId))
			{
				return purchaseId;
			}
			return StoreKitEventListener.inAppsReadableNames[purchaseId];
		}

		private static void LogGameDayCount()
		{
			string text = DateTime.UtcNow.ToString("yyyy-MM-dd");
			try
			{
				Dictionary<string, object> dictionary = (Json.Deserialize(PlayerPrefs.GetString("Analytics.GameDayCount", string.Empty)) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				if (dictionary.Count == 0)
				{
					int num = 1;
					dictionary.Add(text, num);
					string value = Json.Serialize(dictionary);
					PlayerPrefs.SetString("Analytics.GameDayCount", value);
					AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object> { { "count", num } });
					return;
				}
				KeyValuePair<string, object> keyValuePair = dictionary.First();
				object value2 = keyValuePair.Value;
				if (text == keyValuePair.Key)
				{
					object value3 = value2;
					AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object> { { "count", value3 } });
					return;
				}
				int num2 = Convert.ToInt32(value2) + 1;
				string value4 = Json.Serialize(new Dictionary<string, object> { { text, num2 } });
				PlayerPrefs.SetString("Analytics.GameDayCount", value4);
				AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object> { { "count", num2 } });
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		public static void LogAchievementEarned(int achievementId, int newStage)
		{
			try
			{
				if (newStage < 1 || newStage > 3)
				{
					UnityEngine.Debug.LogError(string.Format("invalid achievement newStage : '{0}'", new object[1] { newStage }));
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Earned", string.Format("{0}_{1}", new object[2] { achievementId, newStage }));
				Dictionary<string, object> eventParams = dictionary;
				AnalyticsFacade.SendCustomEvent("Achievements", eventParams);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in LogAchievementEarned: " + ex);
			}
		}

		public static void LogLobbyItemBuy(LobbyItem lobbyItem)
		{
			string eventKeyForLobbyItemGroup = GetEventKeyForLobbyItemGroup(LobbyItemsController.GetRootGroupForSlot(lobbyItem.Slot));
			if (!eventKeyForLobbyItemGroup.IsEmpty())
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object>(1) { 
				{
					eventKeyForLobbyItemGroup,
					lobbyItem.Info.Id
				} };
				AnalyticsFacade.SendCustomEvent("Housing Items Sales Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Housing Items Sales" + GetPayingSuffixNo10(), eventParams);
				string text = "Housing Items Sales Tier " + (ExpController.OurTierForAnyPlace() + 1);
				AnalyticsFacade.SendCustomEvent(text + " Total", eventParams);
				AnalyticsFacade.SendCustomEvent(text + GetPayingSuffixNo10(), eventParams);
			}
		}

		public static void LogLobbyItemSpeedUpBuy(LobbyItem lobbyItem, int gemsPrice)
		{
			string eventKeyForLobbyItemGroup = GetEventKeyForLobbyItemGroup(LobbyItemsController.GetRootGroupForSlot(lobbyItem.Slot));
			if (!eventKeyForLobbyItemGroup.IsEmpty())
			{
				int b = Mathf.RoundToInt((float)gemsPrice / 27f);
				b = Mathf.Max(1, b);
				Dictionary<string, object> eventParams = new Dictionary<string, object>(1) { { eventKeyForLobbyItemGroup, b } };
				AnalyticsFacade.SendCustomEvent("Housing Time Sales Total", eventParams);
				AnalyticsFacade.SendCustomEvent("Housing Time Sales" + GetPayingSuffixNo10(), eventParams);
			}
		}

		private static string GetEventKeyForLobbyItemGroup(LobbyItemGroupType slot)
		{
			string result = string.Empty;
			switch (slot)
			{
			case LobbyItemGroupType.Devices:
				result = "Devices";
				break;
			case LobbyItemGroupType.Buildings:
				result = "Buildings";
				break;
			case LobbyItemGroupType.Decorations:
				result = "Decorations";
				break;
			case LobbyItemGroupType.Backgrounds:
				result = "Background";
				break;
			case LobbyItemGroupType.PetKennel:
				result = "Pet Kennel";
				break;
			case LobbyItemGroupType.Effects:
				result = "Effects";
				break;
			}
			return result;
		}

		internal static IEnumerator WaitInitializationThenLogGameDayCountCoroutine()
		{
			yield return new WaitUntil(() => AnalyticsFacade.FacebookFacade != null);
			LogGameDayCount();
		}

		internal static void LogProgressInExperience(int levelBase1, int tierBase1)
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object>(2)
			{
				{ "Tier", tierBase1 },
				{ "Level", levelBase1 }
			};
			AnalyticsFacade.SendCustomEvent("Active Users Progress Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Active Users Progress" + GetPayingSuffixNo10(), eventParams);
		}

		private static void LoadTrainingStep()
		{
			if (!trainingStepLoaded)
			{
				if (!Storager.hasKey(trainingProgressKey))
				{
					trainingStep = -1;
					Storager.setInt(trainingProgressKey, trainingStep);
				}
				else
				{
					trainingStep = Storager.getInt(trainingProgressKey);
				}
				trainingStepLoaded = true;
			}
		}

		public static string GetPayingSuffixNo10()
		{
			if (!StoreKitEventListener.IsPayingUser())
			{
				return " (Non Paying)";
			}
			return " (Paying)";
		}
	}
}
