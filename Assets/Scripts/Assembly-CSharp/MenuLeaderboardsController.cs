using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class MenuLeaderboardsController : MonoBehaviour
{
	public static MenuLeaderboardsController sharedController;

	private const string MenuLeaderboardsResponseCache = "MenuLeaderboardsFriendsCache";

	private MenuLeaderboardsView _menuLeaderboardsView;

	private string _playerId = string.Empty;

	internal const bool NewLeaderboards = true;

	public bool IsOpened
	{
		get
		{
			return menuLeaderboardsView.opened.activeSelf;
		}
	}

	public MenuLeaderboardsView menuLeaderboardsView
	{
		get
		{
			return _menuLeaderboardsView;
		}
	}

	public void RefreshWithCache()
	{
		if (PlayerPrefs.HasKey("MenuLeaderboardsFriendsCache"))
		{
			string @string = PlayerPrefs.GetString("MenuLeaderboardsFriendsCache");
			FillListsWithResponseText(@string);
		}
	}

	private IEnumerator Start()
	{
		sharedController = this;
		using (new StopwatchLogger("MenuLeaderboardsController.Start()"))
		{
			_menuLeaderboardsView = GetComponent<MenuLeaderboardsView>();
			_playerId = Storager.getString("AccountCreated");
			if (PlayerPrefs.HasKey("MenuLeaderboardsFriendsCache"))
			{
				string @string = PlayerPrefs.GetString("MenuLeaderboardsFriendsCache");
				foreach (float item in FillListsWithResponseTextAsync(@string))
				{
					float num = item;
					yield return null;
				}
			}
			else
			{
				TransitToFallbackState();
			}
		}
	}

	private void OnDestroy()
	{
		sharedController = null;
	}

	private void TransitToFallbackState()
	{
		LeaderboardItemViewModel item = new LeaderboardItemViewModel
		{
			Id = _playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			WinCount = RatingSystem.instance.currentRating,
			Place = 1,
			Highlight = true
		};
		IList<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>(MenuLeaderboardsView.PageSize) { item };
		for (int i = 0; i < MenuLeaderboardsView.PageSize - 1; i++)
		{
			list.Add(LeaderboardItemViewModel.Empty);
		}
		_menuLeaderboardsView.FriendsList = list;
	}

	private void FillListsWithResponseText(string responseText)
	{
		foreach (float item in FillListsWithResponseTextAsync(responseText))
		{
			float num = item;
		}
	}

	private IEnumerable<float> FillListsWithResponseTextAsync(string responseText)
	{
		Dictionary<string, object> response = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (response == null)
		{
			UnityEngine.Debug.LogWarning("Leaderboards response is ill-formed.");
			yield break;
		}
		if (!response.Any())
		{
			UnityEngine.Debug.LogWarning("Leaderboards response contains no elements.");
			yield break;
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Menu Leaderboards response:    " + responseText);
		}
		LeaderboardItemViewModel selfStats = new LeaderboardItemViewModel
		{
			Id = _playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			WinCount = RatingSystem.instance.currentRating,
			Highlight = true
		};
		Func<IList<LeaderboardItemViewModel>, IList<LeaderboardItemViewModel>> groupAndOrder = delegate(IList<LeaderboardItemViewModel> items)
		{
			List<LeaderboardItemViewModel> list2 = new List<LeaderboardItemViewModel>();
			IOrderedEnumerable<IGrouping<int, LeaderboardItemViewModel>> orderedEnumerable = from vm in items
				group vm by vm.WinCount into g
				orderby g.Key descending
				select g;
			int num = 1;
			foreach (IGrouping<int, LeaderboardItemViewModel> item in orderedEnumerable)
			{
				foreach (LeaderboardItemViewModel item2 in item.OrderByDescending((LeaderboardItemViewModel vm) => vm.Rank))
				{
					item2.Place = num;
					list2.Add(item2);
				}
				num += item.Count();
			}
			return list2;
		};
		if (string.IsNullOrEmpty(_playerId))
		{
			UnityEngine.Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		List<LeaderboardItemViewModel> list = LeaderboardsController.ParseLeaderboardEntries(_playerId, "friends", response);
		list.Add(selfStats);
		IList<LeaderboardItemViewModel> orderedFriendsList = groupAndOrder(list);
		for (int i = orderedFriendsList.Count; i < MenuLeaderboardsView.PageSize; i++)
		{
			orderedFriendsList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.2f;
		List<LeaderboardItemViewModel> arg = LeaderboardsController.ParseLeaderboardEntries(_playerId, "best_players", response);
		IList<LeaderboardItemViewModel> orderedBestPlayersList = groupAndOrder(arg);
		for (int j = orderedBestPlayersList.Count; j < MenuLeaderboardsView.PageSize; j++)
		{
			orderedBestPlayersList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.4f;
		List<LeaderboardItemViewModel> arg2 = LeaderboardsController.ParseLeaderboardEntries(FriendsController.sharedController.ClanID, "top_clans", response);
		IList<LeaderboardItemViewModel> orderedClansList = groupAndOrder(arg2);
		for (int k = orderedClansList.Count; k < MenuLeaderboardsView.PageSize; k++)
		{
			orderedClansList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.6f;
		if (_menuLeaderboardsView != null)
		{
			bool flag = orderedBestPlayersList.FirstOrDefault((LeaderboardItemViewModel item) => item.Id == _playerId) != null;
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("We are in top: " + flag);
			}
			_menuLeaderboardsView.FriendsList = orderedFriendsList;
			_menuLeaderboardsView.BestPlayersList = orderedBestPlayersList;
			_menuLeaderboardsView.ClansList = orderedClansList;
			_menuLeaderboardsView.SelfStats = (flag ? LeaderboardItemViewModel.Empty : FulfillSelfStats(selfStats, response));
			object value;
			if (response.TryGetValue("my_clan", out value))
			{
				Dictionary<string, object> dictionary = value as Dictionary<string, object>;
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("My Clan: " + Json.Serialize(value));
				}
				if (dictionary == null)
				{
					UnityEngine.Debug.Log("myClanDictionary == null    Result type: " + value.GetType());
				}
				else
				{
					LeaderboardItemViewModel leaderboardItemViewModel;
					if (dictionary.ContainsKey("place"))
					{
						leaderboardItemViewModel = LeaderboardItemViewModel.Empty;
					}
					else
					{
						leaderboardItemViewModel = new LeaderboardItemViewModel
						{
							Id = (FriendsController.sharedController.ClanID ?? string.Empty),
							Nickname = (FriendsController.sharedController.clanName ?? string.Empty),
							WinCount = int.MinValue,
							Place = int.MinValue,
							Highlight = true
						};
						object value2;
						if (dictionary.TryGetValue("name", out value2))
						{
							leaderboardItemViewModel.Nickname = Convert.ToString(value2);
						}
						object value3;
						if (dictionary.TryGetValue("place", out value3))
						{
							leaderboardItemViewModel.Place = Convert.ToInt32(value3);
						}
						object value4;
						if (dictionary.TryGetValue("wins", out value4))
						{
							leaderboardItemViewModel.WinCount = Convert.ToInt32(value4);
						}
					}
					_menuLeaderboardsView.SelfClanStats = leaderboardItemViewModel;
				}
			}
			else
			{
				_menuLeaderboardsView.SelfClanStats = LeaderboardItemViewModel.Empty;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("_menuLeaderboardsView == null");
		}
		yield return 1f;
	}

	private static LeaderboardItemViewModel FulfillSelfStats(LeaderboardItemViewModel selfStats, Dictionary<string, object> response)
	{
		LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel
		{
			Id = selfStats.Id,
			Nickname = selfStats.Nickname,
			WinCount = selfStats.WinCount,
			Place = int.MinValue,
			Highlight = true
		};
		object value;
		if (response.TryGetValue("me", out value))
		{
			Dictionary<string, object> dictionary = value as Dictionary<string, object>;
			if (dictionary != null)
			{
				try
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(dictionary["wins"]);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		}
		return leaderboardItemViewModel;
	}

	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			UnityEngine.Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		UnityEngine.Debug.Log("MenuLeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_menu_leaderboards");
		wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		}));
		wWWForm.AddField("id", playerId);
		wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
		wWWForm.AddField("auth", FriendsController.Hash("get_menu_leaderboards"));
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.NumberOfBestPlayersRequests++;
		}
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
		yield return request;
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.NumberOfBestPlayersRequests--;
		}
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
			return;
		}
		PlayerPrefs.SetString("MenuLeaderboardsFriendsCache", text);
		LeaderboardScript.Instance.FillGrids(text);
	}

	public void OnBtnLeaderboardsOnClick()
	{
	}

	public void OnBtnLeaderboardsOffClick()
	{
	}
}
