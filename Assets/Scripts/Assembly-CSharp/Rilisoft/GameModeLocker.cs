using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class GameModeLocker
	{
		private static readonly GameModeLocker s_instance = new GameModeLocker();

		private static readonly Dictionary<GameConnect.GameMode, int> _unlockLevels = new Dictionary<GameConnect.GameMode, int>
		{
			{
				GameConnect.GameMode.TeamFight,
				1
			},
			{
				GameConnect.GameMode.Deathmatch,
				2
			},
			{
				GameConnect.GameMode.FlagCapture,
				3
			},
			{
				GameConnect.GameMode.CapturePoints,
				4
			},
			{
				GameConnect.GameMode.Duel,
				5
			}
		};

		private const string AnimatedForLevelKey = "AnimatedForLevel";

		internal static GameModeLocker Instance
		{
			get
			{
				return s_instance;
			}
		}

		public IEnumerable<GameConnect.GameMode> SupportedModes
		{
			get
			{
				return _unlockLevels.Keys;
			}
		}

		private GameModeLocker()
		{
		}

		public IEnumerable<GameConnect.GameMode> GetOrderedSupportedModes()
		{
			return from kv in _unlockLevels
				orderby kv.Value
				select kv.Key;
		}

		internal int GetAnimatedForLevel()
		{
			return PlayerPrefs.GetInt("AnimatedForLevel", int.MaxValue);
		}

		internal void SetAnimatedForLevel(int level)
		{
			PlayerPrefs.SetInt("AnimatedForLevel", level);
		}

		internal static int GetUnlockLevel(GameConnect.GameMode gameMode)
		{
			return BalanceController.GetUnlockLevel(gameMode);
		}

		private bool IsLocked(GameConnect.GameMode gameMode, int level)
		{
			int unlockLevel = GetUnlockLevel(gameMode);
			return level < unlockLevel;
		}

		internal bool IsLocked(GameConnect.GameMode gameMode)
		{
			if (ExperienceController.sharedController == null)
			{
				return true;
			}
			if (gameMode == GameConnect.GameMode.Duel)
			{
				if (RatingSystem.instance == null)
				{
					return true;
				}
				return RatingSystem.instance.currentRating < 300;
			}
			int currentLevel = ExperienceController.sharedController.currentLevel;
			return IsLocked(gameMode, currentLevel);
		}
	}
}
