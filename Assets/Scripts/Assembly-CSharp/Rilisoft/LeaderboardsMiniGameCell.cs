using System;
using UnityEngine;

namespace Rilisoft
{
	public class LeaderboardsMiniGameCell : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _firstPlaceObj;

		[SerializeField]
		protected internal GameObject _secondPlaceObj;

		[SerializeField]
		protected internal GameObject _thirdPlaceObj;

		[SerializeField]
		protected internal GameObject _selfPlayerHighlighterObj;

		[SerializeField]
		protected internal GameObject _placeObj;

		[SerializeField]
		protected internal UILabel _placeLabel;

		[SerializeField]
		protected internal UILabel _playerNameLabel;

		[SerializeField]
		protected internal UILabel _ratingLabel;

		[SerializeField]
		protected internal UISprite _gameModeIcon;

		private GameConnect.GameMode _gameMode;

		private bool _isThisPlayer;

		private Action<LeaderboardsMiniGameCell> _onCellClicked;

		public LeaderboardsMiniGameDataRow Data { get; private set; }

		public void SetData(LeaderboardsMiniGameDataRow data, GameConnect.GameMode gameMode, bool isThisPlayer, Action<LeaderboardsMiniGameCell> onCellClicked)
		{
			Data = data;
			_gameMode = gameMode;
			_isThisPlayer = isThisPlayer;
			_onCellClicked = onCellClicked;
			_selfPlayerHighlighterObj.SetActiveSafeSelf(isThisPlayer);
			_firstPlaceObj.SetActiveSafeSelf(Data.Rank == 1);
			_secondPlaceObj.SetActiveSafeSelf(Data.Rank == 2);
			_thirdPlaceObj.SetActiveSafeSelf(Data.Rank == 3);
			_placeObj.SetActiveSafeSelf(Data.Rank > 3);
			_placeLabel.text = Data.Rank.ToString();
			_playerNameLabel.text = Data.PlayerName;
			string text = string.Empty;
			if (Data.Scores > 0)
			{
				switch (_gameMode)
				{
				case GameConnect.GameMode.TimeBattle:
				case GameConnect.GameMode.DeadlyGames:
				case GameConnect.GameMode.Dater:
				case GameConnect.GameMode.Arena:
				case GameConnect.GameMode.SpeedRun:
				case GameConnect.GameMode.Spleef:
					text = Data.Scores.ToString();
					break;
				case GameConnect.GameMode.DeathEscape:
					text = DeathEscapeUI.FormatTime(Data.Scores);
					break;
				default:
					Debug.LogErrorFormat("fail set scores format, unknown GameConnect.GameMode: '{0}'", _gameMode);
					break;
				}
			}
			else
			{
				text = "---";
			}
			_ratingLabel.text = text;
			string empty = string.Empty;
			switch (_gameMode)
			{
			case GameConnect.GameMode.Dater:
				empty = "leaderboard_like";
				break;
			case GameConnect.GameMode.DeathEscape:
				empty = "leaderboard_time";
				break;
			case GameConnect.GameMode.SpeedRun:
				empty = "leaderboard_distance";
				break;
			default:
				empty = "leaderboard_score";
				break;
			}
			_gameModeIcon.spriteName = empty;
		}

		private void OnClick()
		{
			if (_onCellClicked != null)
			{
				_onCellClicked(this);
			}
		}
	}
}
