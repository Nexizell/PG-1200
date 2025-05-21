using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class LeaderboardsExample : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CPopulateLeaderboards_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardsExample _003C_003E4__this;

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
		public _003CPopulateLeaderboards_003Ed__3(int _003C_003E1__state)
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
				if (_003C_003E4__this.menuLeaderboardsView == null)
				{
					UnityEngine.Debug.LogError("menuLeaderboardsView == null");
					return false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				System.Random random = new System.Random();
				IList<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
				for (int i = 0; i != 42; i++)
				{
					LeaderboardItemViewModel item = new LeaderboardItemViewModel
					{
						Rank = (1000 - i) % 13,
						Nickname = "Player_" + random.Next(100),
						WinCount = random.Next(1000),
						Place = i + 1,
						Highlight = (i > 11 && i % 7 == 3)
					};
					list.Add(item);
				}
				for (int j = list.Count; j < MenuLeaderboardsView.PageSize; j++)
				{
					list.Add(LeaderboardItemViewModel.Empty);
				}
				_003C_003E4__this.menuLeaderboardsView.BestPlayersList = list;
				IList<LeaderboardItemViewModel> list2 = new List<LeaderboardItemViewModel>();
				for (int k = 0; k != 7; k++)
				{
					LeaderboardItemViewModel item2 = new LeaderboardItemViewModel
					{
						Rank = (100 - k) % 13,
						Nickname = "Player_" + k * 13 + 2,
						WinCount = 190 + 3 * k,
						Place = 5 * k + 3,
						Highlight = (k % 6 == 2)
					};
					list2.Add(item2);
				}
				for (int l = list2.Count; l < MenuLeaderboardsView.PageSize; l++)
				{
					list2.Add(LeaderboardItemViewModel.Empty);
				}
				_003C_003E4__this.menuLeaderboardsView.FriendsList = list2;
				return false;
			}
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

	public LeaderboardsView leaderboardsView;

	public MenuLeaderboardsView menuLeaderboardsView;

	private void Start()
	{
		StartCoroutine(PopulateLeaderboards());
	}

	private IEnumerator PopulateLeaderboards()
	{
		if (menuLeaderboardsView == null)
		{
			UnityEngine.Debug.LogError("menuLeaderboardsView == null");
			yield break;
		}
		yield return null;
		System.Random random = new System.Random();
		IList<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		for (int i = 0; i != 42; i++)
		{
			LeaderboardItemViewModel item = new LeaderboardItemViewModel
			{
				Rank = (1000 - i) % 13,
				Nickname = "Player_" + random.Next(100),
				WinCount = random.Next(1000),
				Place = i + 1,
				Highlight = (i > 11 && i % 7 == 3)
			};
			list.Add(item);
		}
		for (int j = list.Count; j < MenuLeaderboardsView.PageSize; j++)
		{
			list.Add(LeaderboardItemViewModel.Empty);
		}
		menuLeaderboardsView.BestPlayersList = list;
		IList<LeaderboardItemViewModel> list2 = new List<LeaderboardItemViewModel>();
		for (int k = 0; k != 7; k++)
		{
			LeaderboardItemViewModel item2 = new LeaderboardItemViewModel
			{
				Rank = (100 - k) % 13,
				Nickname = "Player_" + k * 13 + 2,
				WinCount = 190 + 3 * k,
				Place = 5 * k + 3,
				Highlight = (k % 6 == 2)
			};
			list2.Add(item2);
		}
		for (int l = list2.Count; l < MenuLeaderboardsView.PageSize; l++)
		{
			list2.Add(LeaderboardItemViewModel.Empty);
		}
		menuLeaderboardsView.FriendsList = list2;
	}
}
