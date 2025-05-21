using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal class LeaderboardsMiniGameDataManager
	{
		[CompilerGenerated]
		internal sealed class _003CPullCoroutine_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public int gameMode;

			private WWW _003Crequest_003E5__1;

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
			public _003CPullCoroutine_003Ed__13(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				string id;
				WWWForm wWWForm;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					goto IL_003b;
				case 1:
					_003C_003E1__state = -1;
					goto IL_003b;
				case 2:
					{
						_003C_003E1__state = -1;
						ApplyRequest(_003Crequest_003E5__1);
						return false;
					}
					IL_003b:
					if (FriendsController.sharedController == null || FriendsController.sharedController.id.IsNullOrEmpty())
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					id = FriendsController.sharedController.id;
					UnityEngine.Debug.LogFormat("LeaderboardsMiniGameDataManager.PullCoroutine(GameMode.{0})", gameMode);
					wWWForm = new WWWForm();
					wWWForm.AddField("action", "get_leaderboards_scores");
					wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
					{
						ProtocolListGetter.CurrentPlatform,
						GlobalGameController.AppVersion
					}));
					wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
					wWWForm.AddField("auth", FriendsController.Hash("get_leaderboards_scores"));
					if (gameMode >= 0)
					{
						wWWForm.AddField("mode", gameMode);
					}
					if (Application.isEditor)
					{
						string @string = Encoding.UTF8.GetString(wWWForm.data, 0, wWWForm.data.Length);
						UnityEngine.Debug.LogFormat("LeaderboardsMiniGameDataManager.PullCoroutine form text: '{0}'", @string);
					}
					_003Crequest_003E5__1 = Tools.CreateWwwIfNotConnected(URLs.Friends, wWWForm);
					_003C_003E2__current = _003Crequest_003E5__1;
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

		private const string PREFS_KEY = "leaderboards_minigames";

		private static string testStr = "{\"best_players\":{\"0\":[{\"nick\":\"The Gold Hazop\",\"rank\":17,\"id\":524937,\"scores\":999},{\"nick\":\"khight\",\"rank\":7,\"id\":997506,\"scores\":996}],\"1\":[{\"nick\":\"ricardnavascues\",\"rank\":17,\"id\":614937,\"scores\":998},{\"nick\":\"derpturtle\",\"rank\":27,\"id\":14317,\"scores\":995}]},\"friends\":[{\"id\":50862,\"0\":0,\"1\":0,\"2\":0,\"3\":0,\"4\":0,\"5\":0,\"6\":866},{\"id\":314399,\"0\":0,\"1\":0,\"2\":0,\"3\":856,\"4\":0,\"5\":0,\"6\":0}]}";

		private const float REQUEST_DELAY_SECS = 1200f;

		private static List<LiderboardsMinigamesGameModeData> _currentData;

		private static Dictionary<int, long> _lastRequests = new Dictionary<int, long>();

		private static List<LiderboardsMinigamesGameModeData> CurrentData
		{
			get
			{
				if (_currentData == null)
				{
					string @string = PlayerPrefs.GetString("leaderboards_minigames");
					if (!@string.IsNullOrEmpty())
					{
						try
						{
							LiderboardsMinigamesDataHandler liderboardsMinigamesDataHandler = JsonUtility.FromJson<LiderboardsMinigamesDataHandler>(@string);
							if (liderboardsMinigamesDataHandler != null)
							{
								_currentData = liderboardsMinigamesDataHandler.Data;
							}
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogWarningFormat("Exception caught while parsing '{0}' in `LeaderboardsMiniGameDataManager.CurrentData`: '{1}'", "leaderboards_minigames", @string);
							UnityEngine.Debug.LogException(exception);
							_currentData = new List<LiderboardsMinigamesGameModeData>();
						}
					}
				}
				return _currentData;
			}
			set
			{
				_currentData = value;
			}
		}

		public static event Action OnRefreshed;

		public static List<LeaderboardsMiniGameDataRow> GetData(GameConnect.GameMode gameMode, MiniGameLeaderboardType type)
		{
			if (CurrentData == null)
			{
				return new List<LeaderboardsMiniGameDataRow>();
			}
			LiderboardsMinigamesGameModeData liderboardsMinigamesGameModeData = CurrentData.FirstOrDefault((LiderboardsMinigamesGameModeData d) => d.GameMode == gameMode);
			if (liderboardsMinigamesGameModeData == null)
			{
				return new List<LeaderboardsMiniGameDataRow>();
			}
			switch (type)
			{
			case MiniGameLeaderboardType.Friends:
				return liderboardsMinigamesGameModeData.Friends;
			case MiniGameLeaderboardType.Top:
				return liderboardsMinigamesGameModeData.BestPlayers;
			default:
				return new List<LeaderboardsMiniGameDataRow>();
			}
		}

		public static void PullDataRequest(GameConnect.GameMode? gameMode = null)
		{
			if (CurrentData != null && !gameMode.HasValue)
			{
				return;
			}
			if (CurrentData == null)
			{
				gameMode = null;
			}
			int num = (int)((!gameMode.HasValue) ? ((GameConnect.GameMode)(-1)) : gameMode.Value);
			if (_lastRequests.ContainsKey(num))
			{
				if ((float)_lastRequests[num] + 1200f > (float)FriendsController.ServerTime)
				{
					return;
				}
				_lastRequests[num] = FriendsController.ServerTime;
			}
			else
			{
				_lastRequests.Add(num, FriendsController.ServerTime);
			}
			CoroutineRunner.Instance.StartCoroutine(PullCoroutine(num));
		}

		private static IEnumerator PullCoroutine(int gameMode)
		{
			while (FriendsController.sharedController == null || FriendsController.sharedController.id.IsNullOrEmpty())
			{
				yield return null;
			}
			string id = FriendsController.sharedController.id;
			UnityEngine.Debug.LogFormat("LeaderboardsMiniGameDataManager.PullCoroutine(GameMode.{0})", gameMode);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "get_leaderboards_scores");
			wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
			{
				ProtocolListGetter.CurrentPlatform,
				GlobalGameController.AppVersion
			}));
			wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
			wWWForm.AddField("auth", FriendsController.Hash("get_leaderboards_scores"));
			if (gameMode >= 0)
			{
				wWWForm.AddField("mode", gameMode);
			}
			if (Application.isEditor)
			{
				string @string = Encoding.UTF8.GetString(wWWForm.data, 0, wWWForm.data.Length);
				UnityEngine.Debug.LogFormat("LeaderboardsMiniGameDataManager.PullCoroutine form text: '{0}'", @string);
			}
			WWW request = Tools.CreateWwwIfNotConnected(URLs.Friends, wWWForm);
			yield return request;
			ApplyRequest(request);
		}

		private static void ApplyRequest(WWW request)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("LeaderboardsMiniGameDataManager minigame data request completed");
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
				UnityEngine.Debug.LogWarning("LeaderboardsMiniGameDataManager response is empty.");
				return;
			}
			List<LiderboardsMinigamesGameModeData> list = null;
			try
			{
				list = ParseResponse(text);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(string.Format("LeaderboardsMiniGameDataManager: parse responce '{0}' error: '{1}'", new object[2] { text, ex }));
				return;
			}
			if (list == null)
			{
				UnityEngine.Debug.LogWarning(string.Format("LeaderboardsMiniGameDataManager: parse responce '{0}' error: parsed result is null", new object[1] { text }));
				return;
			}
			if (!list.Any())
			{
				UnityEngine.Debug.LogWarning(string.Format("LeaderboardsMiniGameDataManager: parse responce '{0}' error: parsed result is empty", new object[1] { text }));
				return;
			}
			if (CurrentData != null)
			{
				foreach (LiderboardsMinigamesGameModeData incomingDataGame in list)
				{
					LiderboardsMinigamesGameModeData liderboardsMinigamesGameModeData = CurrentData.FirstOrDefault((LiderboardsMinigamesGameModeData d) => d.GameMode == incomingDataGame.GameMode);
					if (liderboardsMinigamesGameModeData != null)
					{
						CurrentData.Remove(liderboardsMinigamesGameModeData);
					}
					CurrentData.Add(incomingDataGame);
				}
			}
			else
			{
				CurrentData = list;
			}
			string value = JsonUtility.ToJson(new LiderboardsMinigamesDataHandler
			{
				Data = CurrentData
			});
			PlayerPrefs.SetString("leaderboards_minigames", value);
			if (LeaderboardsMiniGameDataManager.OnRefreshed != null)
			{
				LeaderboardsMiniGameDataManager.OnRefreshed();
			}
		}

		private static List<LiderboardsMinigamesGameModeData> ParseResponse(string responseStr)
		{
			List<LiderboardsMinigamesGameModeData> list = new List<LiderboardsMinigamesGameModeData>();
			Dictionary<string, object> dictionary = Json.Deserialize(responseStr ?? string.Empty) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return list;
			}
			if (dictionary.ContainsKey("best_players"))
			{
				Dictionary<string, object> dictionary2 = dictionary.ParseJSONField("best_players", (object o) => o as Dictionary<string, object>);
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> item2 in dictionary2)
					{
						GameConnect.GameMode gameMode2 = (GameConnect.GameMode)ConvertToInt32Invariant(item2.Key);
						LiderboardsMinigamesGameModeData liderboardsMinigamesGameModeData = new LiderboardsMinigamesGameModeData
						{
							GameMode = gameMode2
						};
						list.Add(liderboardsMinigamesGameModeData);
						List<object> list2 = item2.Value as List<object>;
						if (list2 == null)
						{
							continue;
						}
						foreach (object item3 in list2)
						{
							Dictionary<string, object> dictionary3 = item3 as Dictionary<string, object>;
							if (dictionary3 != null)
							{
								LeaderboardsMiniGameDataRow leaderboardsMiniGameDataRow = new LeaderboardsMiniGameDataRow();
								leaderboardsMiniGameDataRow.Id = dictionary3.ParseJSONField("id", (object o) => Convert.ToInt32(o, CultureInfo.InvariantCulture));
								leaderboardsMiniGameDataRow.Rank = dictionary3.ParseJSONField("rank", (object o) => Convert.ToInt32(o, CultureInfo.InvariantCulture));
								leaderboardsMiniGameDataRow.Scores = dictionary3.ParseJSONField("scores", (object o) => Convert.ToInt32(o, CultureInfo.InvariantCulture));
								leaderboardsMiniGameDataRow.PlayerName = dictionary3.ParseJSONField("nick", (object o) => o.ToString());
								liderboardsMinigamesGameModeData.BestPlayers.Add(leaderboardsMiniGameDataRow);
							}
						}
					}
				}
			}
			if (dictionary.ContainsKey("friends"))
			{
				List<object> list3 = dictionary["friends"] as List<object>;
				if (list3 != null)
				{
					foreach (object item4 in list3)
					{
						Dictionary<string, object> dictionary4 = item4 as Dictionary<string, object>;
						if (!dictionary4.ContainsKey("id"))
						{
							continue;
						}
						int id = ConvertToInt32Invariant(dictionary4["id"]);
						string playerName = string.Empty;
						if (dictionary4.ContainsKey("name"))
						{
							playerName = dictionary4["name"].ToString();
						}
						foreach (KeyValuePair<string, object> item5 in dictionary4)
						{
							if (!(item5.Key == "id") && !(item5.Key == "name"))
							{
								GameConnect.GameMode gameMode = (GameConnect.GameMode)ConvertToInt32Invariant(item5.Key);
								LiderboardsMinigamesGameModeData liderboardsMinigamesGameModeData2 = list.FirstOrDefault((LiderboardsMinigamesGameModeData d) => d.GameMode == gameMode);
								if (liderboardsMinigamesGameModeData2 == null)
								{
									liderboardsMinigamesGameModeData2 = new LiderboardsMinigamesGameModeData
									{
										GameMode = gameMode
									};
									list.Add(liderboardsMinigamesGameModeData2);
								}
								LeaderboardsMiniGameDataRow item = new LeaderboardsMiniGameDataRow
								{
									Id = id,
									Scores = ConvertToInt32Invariant(item5.Value),
									PlayerName = playerName
								};
								liderboardsMinigamesGameModeData2.Friends.Add(item);
							}
						}
					}
				}
			}
			return list;
		}

		private static int ConvertToInt32Invariant(object o)
		{
			return Convert.ToInt32(o, CultureInfo.InvariantCulture);
		}
	}
}
