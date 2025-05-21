using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public class LeaderboardsMiniGameController : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CUpdateScrollCoroutine_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LeaderboardsMiniGameController _003C_003E4__this;

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
			public _003CUpdateScrollCoroutine_003Ed__24(int _003C_003E1__state)
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
				if (_003C_003E4__this._scrollUpdaiting)
				{
					return false;
				}
				_003C_003E4__this._scrollUpdaiting = true;
				_003C_003E4__this._betterWrapContent.Initialize(() => _003C_003E4__this._viewData.Count - 1, _003C_003E4__this.FillCell);
				if (_003C_003E4__this._playerCellIndex > _003C_003E4__this._betterWrapContent.VisibleCellsCount - 1)
				{
					_003C_003E4__this._betterWrapContent.ScrollTo(_003C_003E4__this._playerCellIndex);
				}
				_003C_003E4__this._scrollUpdaiting = false;
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

		[SerializeField]
		protected internal UIButton _friendsButton;

		[SerializeField]
		protected internal UIButton _topButton;

		[SerializeField]
		protected internal UIScrollView _scrollView;

		[SerializeField]
		protected internal BetterWrapContent _betterWrapContent;

		[SerializeField]
		protected internal LeaderboardsMiniGameCell _baseCell;

		private GameConnect.GameMode? _currentGameMode;

		private MiniGameLeaderboardType? _currentViewType;

		private List<LeaderboardsMiniGameDataRow> _viewData;

		private int _playerCellIndex;

		private bool _needUpdateScroll;

		private bool _scrollUpdaiting;

		private UIScrollManager ScrollManager
		{
			get
			{
				return _scrollView.gameObject.GetOrAddComponent<UIScrollManager>();
			}
		}

		internal GameConnect.GameMode? CurrentGameMode
		{
			get
			{
				return _currentGameMode;
			}
			set
			{
				if (_currentGameMode != value)
				{
					_currentGameMode = value;
					LeaderboardsMiniGameDataManager.PullDataRequest(_currentGameMode);
					UpdateView();
				}
			}
		}

		internal MiniGameLeaderboardType? CurrentViewType
		{
			get
			{
				return _currentViewType;
			}
			set
			{
				if (_currentViewType != value)
				{
					_currentViewType = value;
					if (_currentViewType == MiniGameLeaderboardType.Friends)
					{
						_friendsButton.isEnabled = false;
						_topButton.isEnabled = true;
					}
					else if (_currentViewType == MiniGameLeaderboardType.Top)
					{
						_friendsButton.isEnabled = true;
						_topButton.isEnabled = false;
					}
					LeaderboardsMiniGameDataManager.PullDataRequest(_currentGameMode);
					UpdateView();
				}
			}
		}

		private void Awake()
		{
			LeaderboardsMiniGameDataManager.OnRefreshed += LeaderboardsMiniGameDataManager_OnDataUpdated;
			LeaderboardsMiniGameDataManager.PullDataRequest();
		}

		private void OnDestroy()
		{
			LeaderboardsMiniGameDataManager.OnRefreshed -= LeaderboardsMiniGameDataManager_OnDataUpdated;
		}

		private void LeaderboardsMiniGameDataManager_OnDataUpdated()
		{
			UpdateView();
		}

		public void UpdateView()
		{
			if (!CurrentViewType.HasValue)
			{
				CurrentViewType = MiniGameLeaderboardType.Top;
			}
			UpdateData();
			_needUpdateScroll = true;
		}

		private void LateUpdate()
		{
			if (_needUpdateScroll)
			{
				_needUpdateScroll = false;
				StopCoroutine(UpdateScrollCoroutine());
				StartCoroutine(UpdateScrollCoroutine());
			}
		}

		private IEnumerator UpdateScrollCoroutine()
		{
			if (!_scrollUpdaiting)
			{
				_scrollUpdaiting = true;
				_betterWrapContent.Initialize(() => _viewData.Count - 1, FillCell);
				if (_playerCellIndex > _betterWrapContent.VisibleCellsCount - 1)
				{
					_betterWrapContent.ScrollTo(_playerCellIndex);
				}
				_scrollUpdaiting = false;
			}
			yield break;
		}

		private void UpdateData()
		{
			if (_viewData == null)
			{
				_viewData = new List<LeaderboardsMiniGameDataRow>();
			}
			_viewData.Clear();
			_viewData = LeaderboardsMiniGameDataManager.GetData(CurrentGameMode.Value, CurrentViewType.Value).ToList();
			_playerCellIndex = -1;
			if (!(FriendsController.sharedController != null) || FriendsController.sharedController.id.IsNullOrEmpty())
			{
				return;
			}
			int playerId = Convert.ToInt32(FriendsController.sharedController.id, CultureInfo.InvariantCulture);
			LeaderboardsMiniGameDataRow leaderboardsMiniGameDataRow = _viewData.FirstOrDefault((LeaderboardsMiniGameDataRow r) => r.Id == playerId);
			if (leaderboardsMiniGameDataRow == null)
			{
				leaderboardsMiniGameDataRow = new LeaderboardsMiniGameDataRow
				{
					Id = playerId,
					PlayerName = FriendsController.sharedController.nick
				};
				if (_viewData.Any() && _viewData.Count > 99)
				{
					_viewData.RemoveAt(_viewData.Count - 1);
				}
				_viewData.Add(leaderboardsMiniGameDataRow);
			}
			leaderboardsMiniGameDataRow.Scores = MiniGamesPlayerScoreManager.Instance.GetScore(CurrentGameMode.Value);
			switch (CurrentGameMode.Value)
			{
			case GameConnect.GameMode.TimeBattle:
			case GameConnect.GameMode.DeadlyGames:
			case GameConnect.GameMode.Dater:
			case GameConnect.GameMode.Arena:
			case GameConnect.GameMode.SpeedRun:
			case GameConnect.GameMode.Spleef:
				_viewData = _viewData.OrderByDescending((LeaderboardsMiniGameDataRow d) => d.Scores).ToList();
				break;
			case GameConnect.GameMode.DeathEscape:
				_viewData = _viewData.OrderBy((LeaderboardsMiniGameDataRow d) => d.Scores).ToList();
				if (_viewData.Count > 1)
				{
					int num = 0;
					while (_viewData[0].Scores < 1 && num < _viewData.Count)
					{
						LeaderboardsMiniGameDataRow item = _viewData[0];
						_viewData.Remove(item);
						_viewData.Add(item);
						num++;
					}
				}
				break;
			default:
				UnityEngine.Debug.LogErrorFormat("fail sort items, unknown GameConnect.GameMode: '{0}'", CurrentGameMode.Value);
				break;
			}
			_playerCellIndex = _viewData.IndexOf(leaderboardsMiniGameDataRow);
			if (leaderboardsMiniGameDataRow.Scores < 1)
			{
				_viewData.Remove(leaderboardsMiniGameDataRow);
				LeaderboardsMiniGameDataRow leaderboardsMiniGameDataRow2 = _viewData.FirstOrDefault((LeaderboardsMiniGameDataRow i) => i.Scores < 1);
				if (leaderboardsMiniGameDataRow2 != null)
				{
					_viewData.Insert(_viewData.IndexOf(leaderboardsMiniGameDataRow2), leaderboardsMiniGameDataRow);
				}
				else
				{
					_viewData.Add(leaderboardsMiniGameDataRow);
				}
				_playerCellIndex = _viewData.IndexOf(leaderboardsMiniGameDataRow);
			}
			int num2 = 1;
			foreach (LeaderboardsMiniGameDataRow viewDatum in _viewData)
			{
				viewDatum.Rank = num2;
				num2++;
			}
		}

		private void FillCell(GameObject cellObj, int idx)
		{
			if (idx >= 0 && idx < _viewData.Count)
			{
				LeaderboardsMiniGameDataRow data = _viewData[idx];
				cellObj.GetComponent<LeaderboardsMiniGameCell>().SetData(data, CurrentGameMode.Value, idx == _playerCellIndex, OnCellClicked);
			}
		}

		private void OnCellClicked(LeaderboardsMiniGameCell cell)
		{
			if (!(FriendsController.sharedController == null) && !FriendsController.sharedController.id.IsNullOrEmpty())
			{
				int num = Convert.ToInt32(FriendsController.sharedController.id, CultureInfo.InvariantCulture);
				if (cell.Data.Id != num)
				{
					Action<bool> onCloseEvent = delegate
					{
					};
					FriendsController.ShowProfile(cell.Data.Id.ToString(), ProfileWindowType.other, onCloseEvent);
				}
			}
		}

		public void U_ShowFriends()
		{
			CurrentViewType = MiniGameLeaderboardType.Friends;
			UpdateView();
		}

		public void U_ShowTop()
		{
			CurrentViewType = MiniGameLeaderboardType.Top;
			UpdateView();
		}
	}
}
