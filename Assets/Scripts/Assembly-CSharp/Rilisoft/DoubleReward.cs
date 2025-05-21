using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DoubleReward
	{
		private static readonly DoubleReward s_instance = new DoubleReward();

		private static readonly GameConnect.GameMode[] _supportedModes = new GameConnect.GameMode[5]
		{
			GameConnect.GameMode.TeamFight,
			GameConnect.GameMode.Deathmatch,
			GameConnect.GameMode.FlagCapture,
			GameConnect.GameMode.CapturePoints,
			GameConnect.GameMode.Duel
		};

		private const string STORAGER_KEYS_INITIALIZED_KEY = "DoubleReward.STORAGER_KEYS_INITIALIZED_KEY";

		internal bool Enabled { get; set; }

		internal int RewardFactor { get; set; }

		internal static DoubleReward Instance
		{
			get
			{
				return s_instance;
			}
		}

		internal IEnumerable<GameConnect.GameMode> SupportedModes
		{
			get
			{
				return _supportedModes;
			}
		}

		private DoubleReward()
		{
			Enabled = true;
			RewardFactor = 2;
		}

		internal long? EstimateNextTimeForDoubleRewardInSeconds()
		{
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return null;
			}
			return Convert.ToInt64(Math.Floor(serverTime.Value.Date.AddDays(1.0).Subtract(serverTime.Value).TotalSeconds));
		}

		internal DateTime? GetDoubleRewardDate(GameConnect.GameMode gameMode)
		{
			InitializeStoragerKeysIfNeeded();
			DateTime result;
			if (!DateTime.TryParse(Storager.getString(FormatStorageKey(gameMode)), out result))
			{
				return null;
			}
			return result;
		}

		internal bool NeedDoubleReward(GameConnect.GameMode gameMode)
		{
			if (!Enabled)
			{
				return false;
			}
			if (ExperienceController.sharedController == null || ExperienceController.sharedController.currentLevel < 2)
			{
				return false;
			}
			if (Array.IndexOf(_supportedModes, gameMode) < 0)
			{
				return false;
			}
			if (GameModeLocker.Instance.IsLocked(gameMode))
			{
				return false;
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return false;
			}
			InitializeStoragerKeysIfNeeded();
			DateTime result;
			if (!DateTime.TryParse(Storager.getString(FormatStorageKey(gameMode)), out result))
			{
				return true;
			}
			return result.Date < serverTime.Value.Date;
		}

		internal void InitializeDoubleRewardDate(DateTime? dateTime)
		{
			if (dateTime.HasValue)
			{
				GameConnect.GameMode[] supportedModes = _supportedModes;
				foreach (GameConnect.GameMode gameMode in supportedModes)
				{
					SaveDoubleRewardDate(gameMode, dateTime);
				}
			}
		}

		internal void SaveDoubleRewardDate(GameConnect.GameMode gameMode, DateTime? dateTime)
		{
			if (dateTime.HasValue)
			{
				string val = dateTime.Value.ToString("s");
				Storager.setString(FormatStorageKey(gameMode), val);
			}
		}

		internal void SendAnalyticsIfNeededForMode(GameConnect.GameMode gameMode)
		{
			try
			{
				if (NeedDoubleReward(gameMode))
				{
					string storagerKey = FormatStorageAnalyticsKey(gameMode);
					DateTime? serverTime = FriendsController.GetServerTime();
					Action action = delegate
					{
						AnalyticsStuff.FirstWinOfTheDayGameModesStart(gameMode);
						string val = serverTime.Value.ToString("s");
						Storager.setString(storagerKey, val);
					};
					InitializeStoragerKeysIfNeeded();
					DateTime result;
					if (!DateTime.TryParse(Storager.getString(storagerKey), out result))
					{
						action();
					}
					else if (result.Date < serverTime.Value.Date)
					{
						action();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in SendAnalyticsIfNeededForMode: {0}", ex);
			}
		}

		internal static void InitializeStoragerKeysIfNeeded()
		{
			if (Storager.getInt("DoubleReward.STORAGER_KEYS_INITIALIZED_KEY") > 0)
			{
				return;
			}
			try
			{
				GameConnect.GameMode[] supportedModes = _supportedModes;
				foreach (GameConnect.GameMode gameMode in supportedModes)
				{
					Storager.setString(FormatStorageKey(gameMode), string.Empty);
					Storager.setString(FormatStorageAnalyticsKey(gameMode), string.Empty);
				}
				Storager.setInt("DoubleReward.STORAGER_KEYS_INITIALIZED_KEY", 1);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in InitializeStoragerKeysIfNeeded: {0}", ex);
			}
		}

		private static string FormatStorageKey(GameConnect.GameMode gameMode)
		{
			return "LastDateDoubleReward." + gameMode;
		}

		private static string FormatStorageAnalyticsKey(GameConnect.GameMode gameMode)
		{
			return "LastDateDoubleReward.Analytics." + gameMode;
		}
	}
}
