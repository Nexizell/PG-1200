using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class LeaderboardsController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CGetLeaderboardsCoroutine_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string playerId;

		private WWWForm _003Cform_003E5__1;

		public LeaderboardsController _003C_003E4__this;

		private WWW _003Crequest_003E5__2;

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
		public _003CGetLeaderboardsCoroutine_003Ed__13(int _003C_003E1__state)
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
				if (string.IsNullOrEmpty(playerId))
				{
					UnityEngine.Debug.LogWarning("Player id should not be empty.");
					return false;
				}
				UnityEngine.Debug.Log("LeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
				_003Cform_003E5__1 = new WWWForm();
				_003Cform_003E5__1.AddField("action", "get_leaderboards_league");
				_003Cform_003E5__1.AddField("app_version", string.Format("{0}:{1}", new object[2]
				{
					ProtocolListGetter.CurrentPlatform,
					GlobalGameController.AppVersion
				}));
				_003Cform_003E5__1.AddField("id", playerId);
				_003Cform_003E5__1.AddField("league_id", LeaderboardScript.GetLeagueId());
				_003Cform_003E5__1.AddField("uniq_id", FriendsController.sharedController.id);
				_003Cform_003E5__1.AddField("auth", FriendsController.Hash("get_leaderboards_league"));
				if (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
				{
					UnityEngine.Debug.Log("Waiting previous leaderboards request...");
					goto IL_013a;
				}
				goto IL_0147;
			case 1:
				_003C_003E1__state = -1;
				goto IL_013a;
			case 2:
				{
					_003C_003E1__state = -1;
					FriendsController.sharedController.NumberOfBestPlayersRequests--;
					_003C_003E4__this.HandleRequestCompleted(_003Crequest_003E5__2);
					return false;
				}
				IL_013a:
				if (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0147;
				IL_0147:
				FriendsController.sharedController.NumberOfBestPlayersRequests++;
				_003Crequest_003E5__2 = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, _003Cform_003E5__1);
				_003C_003E2__current = _003Crequest_003E5__2;
				_003C_003E1__state = 2;
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

	private LeaderboardsView _leaderboardsView;

	private FriendsGUIController _friendsGuiController;

	private string _playerId = string.Empty;

	public LeaderboardsView LeaderboardsView
	{
		private get
		{
			return _leaderboardsView;
		}
		set
		{
			_leaderboardsView = value;
		}
	}

	public FriendsGUIController FriendsGuiController
	{
		private get
		{
			return _friendsGuiController;
		}
		set
		{
			_friendsGuiController = value;
		}
	}

	public string PlayerId
	{
		private get
		{
			return _playerId;
		}
		set
		{
			_playerId = value ?? string.Empty;
		}
	}

	public void RequestLeaderboards()
	{
		if (!string.IsNullOrEmpty(_playerId))
		{
			StartCoroutine(GetLeaderboardsCoroutine(_playerId));
		}
		else
		{
			UnityEngine.Debug.Log("Player id should not be empty.");
		}
	}

	internal static List<LeaderboardItemViewModel> ParseLeaderboardEntries(string entryId, string leaderboardName, Dictionary<string, object> response)
	{
		if (string.IsNullOrEmpty(leaderboardName))
		{
			throw new ArgumentException("Leaderbord should not be empty.", "leaderboardName");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		object value;
		if (!response.TryGetValue(leaderboardName, out value))
		{
			return list;
		}
		List<object> list2 = value as List<object>;
		if (list2 == null)
		{
			return list;
		}
		foreach (Dictionary<string, object> item in list2.OfType<Dictionary<string, object>>())
		{
			LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel();
			object value2;
			if (item.TryGetValue("id", out value2))
			{
				leaderboardItemViewModel.Id = Convert.ToString(value2);
				leaderboardItemViewModel.Highlight = !string.IsNullOrEmpty(leaderboardItemViewModel.Id) && leaderboardItemViewModel.Id.Equals(entryId);
			}
			object value3;
			int result;
			if (item.TryGetValue("rank", out value3) && int.TryParse(value3 as string, out result))
			{
				leaderboardItemViewModel.Rank = result;
			}
			else if (item.TryGetValue("member_cnt", out value3))
			{
				try
				{
					leaderboardItemViewModel.Rank = Convert.ToInt32(value3);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			object value4;
			if (item.TryGetValue("nick", out value4))
			{
				leaderboardItemViewModel.Nickname = (value4 as string) ?? string.Empty;
			}
			else if (item.TryGetValue("name", out value4))
			{
				leaderboardItemViewModel.Nickname = (value4 as string) ?? string.Empty;
			}
			try
			{
				object value5;
				if (item.TryGetValue("trophies", out value5))
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
				}
				else if (item.TryGetValue("wins", out value5))
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
				}
				else if (item.TryGetValue("win", out value5))
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
				}
				else if (item.TryGetValue("like", out value5))
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
			}
			object value6;
			if (item.TryGetValue("logo", out value6))
			{
				leaderboardItemViewModel.ClanLogo = (value6 as string) ?? string.Empty;
			}
			else
			{
				leaderboardItemViewModel.ClanLogo = string.Empty;
			}
			object value7;
			if (item.TryGetValue("name", out value7))
			{
				leaderboardItemViewModel.ClanName = (value7 as string) ?? string.Empty;
			}
			else if (item.TryGetValue("clan_name", out value7))
			{
				leaderboardItemViewModel.ClanName = (value7 as string) ?? string.Empty;
			}
			else
			{
				leaderboardItemViewModel.ClanName = string.Empty;
			}
			list.Add(leaderboardItemViewModel);
		}
		return list;
	}

	private void Start()
	{
		RequestLeaderboards();
	}

	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			UnityEngine.Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		UnityEngine.Debug.Log("LeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
		WWWForm form = new WWWForm();
		form.AddField("action", "get_leaderboards_league");
		form.AddField("app_version", string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		}));
		form.AddField("id", playerId);
		form.AddField("league_id", LeaderboardScript.GetLeagueId());
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_leaderboards_league"));
		if (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
		{
			UnityEngine.Debug.Log("Waiting previous leaderboards request...");
			while (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
			{
				yield return null;
			}
		}
		FriendsController.sharedController.NumberOfBestPlayersRequests++;
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form);
		yield return request;
		FriendsController.sharedController.NumberOfBestPlayersRequests--;
		HandleRequestCompleted(request);
	}

	private void HandleRequestCompleted(WWW request)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("HandleRequestCompleted()");
		}
		if (request == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			UnityEngine.Debug.LogWarning(request.error);
			return;
		}
		string text = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("Leaderboars response is empty.");
		}
		else
		{
			LeaderboardScript.Instance.FillGrids(text);
		}
	}
}
