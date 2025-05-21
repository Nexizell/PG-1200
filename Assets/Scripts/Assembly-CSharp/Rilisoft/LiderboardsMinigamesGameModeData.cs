using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class LiderboardsMinigamesGameModeData 
	{
		public GameConnect.GameMode GameMode;

		public List<LeaderboardsMiniGameDataRow> BestPlayers = new List<LeaderboardsMiniGameDataRow>();

		public List<LeaderboardsMiniGameDataRow> Friends = new List<LeaderboardsMiniGameDataRow>();
	}
}
