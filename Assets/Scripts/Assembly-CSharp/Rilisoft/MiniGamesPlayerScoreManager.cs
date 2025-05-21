using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal class MiniGamesPlayerScoreManager
	{
		[Serializable]
		internal class ScoresContainer 
		{
			[SerializeField]
			public List<GameScores> Scores;
		}

		[Serializable]
		internal class GameScores 
		{
			[SerializeField]
			public GameConnect.GameMode GameMode;

			[SerializeField]
			public int Scores;
		}

		private const string DATA_KEY = "minigames-player-scores";

		private static Lazy<MiniGamesPlayerScoreManager> _instance = new Lazy<MiniGamesPlayerScoreManager>(() => new MiniGamesPlayerScoreManager());

		private Dictionary<GameConnect.GameMode, int> _scores;

		public static MiniGamesPlayerScoreManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private Dictionary<GameConnect.GameMode, int> Scores
		{
			get
			{
				if (_scores == null)
				{
					_scores = new Dictionary<GameConnect.GameMode, int>();
					foreach (GameScores item in ReadScores())
					{
						_scores.Add(item.GameMode, item.Scores);
					}
				}
				return _scores;
			}
		}

		public void SetScore(GameConnect.GameMode gameMode, int score)
		{
			if (!Scores.ContainsKey(gameMode))
			{
				Scores.Add(gameMode, 0);
			}
			Scores[gameMode] = score;
			SaveScores();
		}

		public void IncrementScore(GameConnect.GameMode gameMode, int score)
		{
			if (!Scores.ContainsKey(gameMode))
			{
				Scores.Add(gameMode, 0);
			}
			Scores[gameMode] += score;
			SaveScores();
		}

		public int GetScore(GameConnect.GameMode gameMode)
		{
			if (!Scores.ContainsKey(gameMode))
			{
				return 0;
			}
			return Scores[gameMode];
		}

		private List<GameScores> ReadScores()
		{
			string @string = Storager.getString("minigames-player-scores");
			if (!@string.IsNullOrEmpty())
			{
				try
				{
					ScoresContainer scoresContainer = JsonUtility.FromJson<ScoresContainer>(@string);
					if (scoresContainer != null && scoresContainer.Scores != null)
					{
						return scoresContainer.Scores;
					}
				}
				catch (Exception exception)
				{
					Debug.LogWarningFormat("Exception caught while parsing '{0}' in `MiniGamesPlayerScoreManager.ReadScores()`: '{1}'", "minigames-player-scores", @string);
					Debug.LogException(exception);
					return new List<GameScores>();
				}
			}
			return new List<GameScores>();
		}

		private void SaveScores()
		{
			List<GameScores> list = new List<GameScores>();
			foreach (KeyValuePair<GameConnect.GameMode, int> score in Scores)
			{
				list.Add(new GameScores
				{
					GameMode = score.Key,
					Scores = score.Value
				});
			}
			string val = JsonUtility.ToJson(new ScoresContainer
			{
				Scores = list
			});
			Storager.setString("minigames-player-scores", val);
		}
	}
}
