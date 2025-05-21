using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class LeaderboardScript : MonoBehaviour
{
	internal enum GridState
	{
		Empty = 0,
		FillingWithCache = 1,
		Cache = 2,
		FillingWithResponse = 3,
		Response = 4
	}

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass36_0
	{
		public LeaderboardItemViewModel me;

		internal bool _003CFillGrids_003Eb__2(LeaderboardItemViewModel i)
		{
			return i.Id == me.Id;
		}

		internal bool _003CFillGrids_003Eb__3(LeaderboardItemViewModel i)
		{
			return i.Id != me.Id;
		}

		internal bool _003CFillGrids_003Eb__5(LeaderboardItemViewModel i)
		{
			return i.Id == me.Id;
		}

		internal bool _003CFillGrids_003Eb__6(LeaderboardItemViewModel i)
		{
			return i.Id != me.Id;
		}

		internal bool _003CFillGrids_003Eb__7(LeaderboardItemViewModel i)
		{
			return i.WinCount <= me.WinCount;
		}

		internal bool _003CFillGrids_003Eb__9(LeaderboardItemViewModel i)
		{
			return i.Id == me.Id;
		}

		internal bool _003CFillGrids_003Eb__10(LeaderboardItemViewModel i)
		{
			return i.Id != me.Id;
		}

		internal void _003CFillGrids_003Eb__14(UILabel n)
		{
			n.text = me.Nickname;
		}
	}

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass36_1
	{
		public string clanId;

		internal bool _003CFillGrids_003Eb__18(LeaderboardItemViewModel c)
		{
			return c.Id == clanId;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillGrids_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public string playerId;

		public string response;

		public GridState state;

		private Dictionary<string, object> _003Cd_003E5__1;

		private _003C_003Ec__DisplayClass36_0 _003C_003E8__2;

		private Coroutine _003CfillClansCoroutine_003E5__3;

		private Coroutine _003CfillTournamentCoroutine_003E5__4;

		private Coroutine _003CfillCraftersCoroutine_003E5__5;

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
		public _003CFillGrids_003Ed__36(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			case -2:
			case -1:
			case 0:
			case 1:
				break;
			}
		}

		private bool MoveNext()
		{
			try
			{
				object value;
				object value2;
				int result;
				List<LeaderboardItemViewModel> list9;
				HashSet<string> hashSet;
				List<LeaderboardItemViewModel> list10;
				Coroutine coroutine2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					goto IL_0055;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0055;
				case 2:
				{
					_003C_003E1__state = -3;
					List<LeaderboardItemViewModel> list = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_likes", _003Cd_003E5__1);
					LeaderboardItemViewModel leaderboardItemViewModel = list.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == _003C_003E8__2.me.Id);
					int winCount = ((!(Singleton<LobbyItemsController>.Instance == null)) ? Singleton<LobbyItemsController>.Instance.GetLikeCount() : 0);
					if (leaderboardItemViewModel == null)
					{
						leaderboardItemViewModel = new LeaderboardItemViewModel
						{
							Id = _003C_003E8__2.me.Id,
							Nickname = _003C_003E8__2.me.Nickname,
							Rank = _003C_003E8__2.me.Rank,
							WinCount = winCount,
							Highlight = true
						};
						list.Add(leaderboardItemViewModel);
					}
					else
					{
						leaderboardItemViewModel.Nickname = _003C_003E8__2.me.Nickname;
						leaderboardItemViewModel.Rank = _003C_003E8__2.me.Rank;
						leaderboardItemViewModel.WinCount = winCount;
						leaderboardItemViewModel.Highlight = true;
					}
					List<LeaderboardItemViewModel> list2 = GroupAndOrder(list);
					_003CfillCraftersCoroutine_003E5__5 = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillCraftersGrid(_003C_003E4__this.CraftersGrid, list2, state));
					List<LeaderboardItemViewModel> list3 = LeaderboardsController.ParseLeaderboardEntries(playerId, "best_players", _003Cd_003E5__1);
					if (list3.Any())
					{
						if (list3.All((LeaderboardItemViewModel i) => i.Id != _003C_003E8__2.me.Id))
						{
							LeaderboardItemViewModel item = list3.OrderByDescending((LeaderboardItemViewModel i) => i.WinCount).Last();
							list3.Remove(item);
							list3.Add(_003C_003E8__2.me);
						}
						else
						{
							LeaderboardItemViewModel leaderboardItemViewModel2 = list3.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == _003C_003E8__2.me.Id);
							if (leaderboardItemViewModel2 != null)
							{
								leaderboardItemViewModel2.Nickname = _003C_003E8__2.me.Nickname;
								leaderboardItemViewModel2.Rank = _003C_003E8__2.me.Rank;
								leaderboardItemViewModel2.WinCount = _003C_003E8__2.me.WinCount;
								leaderboardItemViewModel2.Highlight = _003C_003E8__2.me.Highlight;
								leaderboardItemViewModel2.ClanName = _003C_003E8__2.me.ClanName;
								leaderboardItemViewModel2.ClanLogo = _003C_003E8__2.me.ClanLogo;
							}
						}
					}
					List<LeaderboardItemViewModel> list4 = GroupAndOrder(list3);
					_003C_003E4__this.AddCacheInProfileInfo(list3);
					Coroutine coroutine = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillPlayersGrid(_003C_003E4__this.TopPlayersGrid, list4, state));
					list3.Clear();
					List<LeaderboardItemViewModel> list5 = LeaderboardsController.ParseLeaderboardEntries(playerId, "competition", _003Cd_003E5__1);
					if (list5.Any())
					{
						if (list5.All((LeaderboardItemViewModel i) => i.Id != _003C_003E8__2.me.Id))
						{
							if (list5.Any((LeaderboardItemViewModel i) => i.WinCount <= _003C_003E8__2.me.WinCount))
							{
								LeaderboardItemViewModel item2 = list5.OrderByDescending((LeaderboardItemViewModel i) => i.WinCount).Last();
								list5.Remove(item2);
								list5.Add(_003C_003E8__2.me);
							}
						}
						else
						{
							LeaderboardItemViewModel leaderboardItemViewModel3 = list5.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == _003C_003E8__2.me.Id);
							if (leaderboardItemViewModel3 != null)
							{
								leaderboardItemViewModel3.Nickname = _003C_003E8__2.me.Nickname;
								leaderboardItemViewModel3.Rank = _003C_003E8__2.me.Rank;
								leaderboardItemViewModel3.WinCount = _003C_003E8__2.me.WinCount;
								leaderboardItemViewModel3.Highlight = _003C_003E8__2.me.Highlight;
								leaderboardItemViewModel3.ClanName = _003C_003E8__2.me.ClanName;
								leaderboardItemViewModel3.ClanLogo = _003C_003E8__2.me.ClanLogo;
							}
						}
					}
					_003C_003E4__this.LeaderboardView.InTournamentTop = list5.Any() && list5.All((LeaderboardItemViewModel i) => i.Id != _003C_003E8__2.me.Id);
					if (_003C_003E4__this.FooterTableTournament != null)
					{
						if (_003C_003E4__this.LeaderboardView.InTournamentTop)
						{
							_003C_003E4__this.FooterTableTournament.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
							{
								n.text = LocalizationStore.Get("Key_0053");
							});
							_003C_003E4__this.FooterTableTournament.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
							{
								n.text = _003C_003E8__2.me.Nickname;
							});
							_003C_003E4__this.FooterTableTournament.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
							{
								n.text = RatingSystem.instance.currentRating.ToString();
							});
							_003C_003E4__this.FooterTableTournament.SetActiveSafe(true);
						}
						else
						{
							_003C_003E4__this.FooterTableTournament.SetActiveSafe(false);
						}
					}
					List<LeaderboardItemViewModel> list6 = GroupAndOrder(list5);
					_003CfillTournamentCoroutine_003E5__4 = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillTournamentGrid(_003C_003E4__this.TournamentGrid, list6, state));
					list5.Clear();
					List<LeaderboardItemViewModel> list7 = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_clans", _003Cd_003E5__1);
					List<LeaderboardItemViewModel> list8 = GroupAndOrder(list7);
					_003CfillClansCoroutine_003E5__3 = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillClansGrid(_003C_003E4__this.TopClansGrid, list8, state));
					if (_003C_003E4__this.ClansTableFooter != null)
					{
						_003C_003Ec__DisplayClass36_1 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass36_1
						{
							clanId = FriendsController.sharedController.Map((FriendsController s) => s.ClanID)
						};
						if (string.IsNullOrEmpty(CS_0024_003C_003E8__locals0.clanId))
						{
							_003C_003E4__this.LeaderboardView.CanShowClanTableFooter = false;
							_003C_003E4__this.ClansTableFooter.SetActive(false);
						}
						else
						{
							LeaderboardItemViewModel leaderboardItemViewModel4 = list7.FirstOrDefault((LeaderboardItemViewModel c) => c.Id == CS_0024_003C_003E8__locals0.clanId);
							_003C_003E4__this.LeaderboardView.CanShowClanTableFooter = leaderboardItemViewModel4 == null;
							_003C_003E4__this.ClansTableFooter.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
							{
								n.text = LocalizationStore.Get("Key_0053");
							});
							_003C_003E4__this.ClansTableFooter.transform.FindChild("LabelMembers").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
							{
								n.text = string.Empty;
							});
							_003C_003E4__this.ClansTableFooter.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel w)
							{
								w.text = string.Empty;
							});
							UILabel o = _003C_003E4__this.ClansTableFooter.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>());
							o.Do(delegate(UILabel cl)
							{
								cl.text = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty);
							});
							o.Map((UILabel cl) => cl.GetComponentsInChildren<UITexture>(true).FirstOrDefault()).Do(delegate(UITexture t)
							{
								SetClanLogo(t, FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty));
							});
						}
					}
					_003C_003E2__current = coroutine;
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					_003C_003E1__state = -3;
					_003C_003E2__current = _003CfillClansCoroutine_003E5__3;
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -3;
					_003C_003E2__current = _003CfillTournamentCoroutine_003E5__4;
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -3;
					_003C_003E2__current = _003CfillCraftersCoroutine_003E5__5;
					_003C_003E1__state = 6;
					return true;
				case 6:
					{
						_003C_003E1__state = -3;
						_003C_003E8__2 = null;
						_003Cd_003E5__1 = null;
						_003CfillCraftersCoroutine_003E5__5 = null;
						_003CfillTournamentCoroutine_003E5__4 = null;
						_003CfillClansCoroutine_003E5__3 = null;
						_003C_003Em__Finally1();
						return false;
					}
					IL_0055:
					if (_003C_003E4__this._fillLock)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E4__this._fillLock = true;
					_003C_003E1__state = -3;
					_003C_003E8__2 = new _003C_003Ec__DisplayClass36_0();
					if (string.IsNullOrEmpty(playerId))
					{
						throw new ArgumentException("Player id should not be empty", "playerId");
					}
					_003Cd_003E5__1 = Json.Deserialize(response ?? string.Empty) as Dictionary<string, object>;
					if (_003Cd_003E5__1 == null)
					{
						UnityEngine.Debug.LogWarning("Leaderboards response is ill-formed.");
						_003Cd_003E5__1 = new Dictionary<string, object>();
					}
					else if (_003Cd_003E5__1.Count == 0)
					{
						UnityEngine.Debug.LogWarning("Leaderboards response contains no elements.");
					}
					_003C_003E8__2.me = new LeaderboardItemViewModel
					{
						Id = playerId,
						Nickname = ProfileController.GetPlayerNameOrDefault(),
						Rank = ExperienceController.sharedController.currentLevel,
						WinCount = RatingSystem.instance.currentRating,
						Highlight = true,
						ClanName = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty),
						ClanLogo = FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty)
					};
					if (_003Cd_003E5__1.TryGetValue("me", out value) && (value as Dictionary<string, object>).TryGetValue("wins", out value2))
					{
						int value3 = Convert.ToInt32(value2);
						PlayerPrefs.SetInt("TotalWinsForLeaderboards", value3);
					}
					if (_003Cd_003E5__1.ContainsKey("competition_id") && int.TryParse(_003Cd_003E5__1["competition_id"].ToString(), out result) && FriendsController.sharedController != null && result > FriendsController.sharedController.currentCompetition)
					{
						FriendsController.sharedController.SendRequestGetCurrentcompetition();
					}
					list9 = LeaderboardsController.ParseLeaderboardEntries(playerId, "friends", _003Cd_003E5__1);
					hashSet = new HashSet<string>(FriendsController.sharedController.friends);
					if (FriendsController.sharedController != null)
					{
						for (int num = list9.Count - 1; num >= 0; num--)
						{
							LeaderboardItemViewModel leaderboardItemViewModel5 = list9[num];
							Dictionary<string, object> value4;
							if (hashSet.Contains(leaderboardItemViewModel5.Id) && FriendsController.sharedController.friendsInfo.TryGetValue(leaderboardItemViewModel5.Id, out value4))
							{
								try
								{
									Dictionary<string, object> dictionary = value4["player"] as Dictionary<string, object>;
									leaderboardItemViewModel5.Nickname = Convert.ToString(dictionary["nick"]);
									leaderboardItemViewModel5.Rank = Convert.ToInt32(dictionary["rank"]);
									object value5;
									if (dictionary.TryGetValue("clan_name", out value5))
									{
										leaderboardItemViewModel5.ClanName = Convert.ToString(value5);
									}
									object value6;
									if (dictionary.TryGetValue("clan_logo", out value6))
									{
										leaderboardItemViewModel5.ClanLogo = Convert.ToString(value6);
									}
								}
								catch (KeyNotFoundException)
								{
									UnityEngine.Debug.LogError("Failed to process cached friend: " + leaderboardItemViewModel5.Id);
								}
							}
							else
							{
								list9.RemoveAt(num);
							}
						}
					}
					list9.Add(_003C_003E8__2.me);
					list10 = GroupAndOrder(list9);
					coroutine2 = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillFriendsGrid(_003C_003E4__this.TopFriendsGrid, list10, state));
					_003C_003E2__current = coroutine2;
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this._fillLock = false;
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass44_0
	{
		public GameObject gridGo;

		public GridState state;

		public LeaderboardScript _003C_003E4__this;

		internal void _003CFillClansGrid_003Eb__0(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			_003C_003E4__this.FillClanItem(gridGo, _003C_003E4__this._clansList, state, index, go);
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillClansGrid_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public GameObject gridGo;

		public GridState state;

		public List<LeaderboardItemViewModel> list;

		private UIWrapContent _003Cwrap_003E5__1;

		private _003C_003Ec__DisplayClass44_0 _003C_003E8__2;

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
		public _003CFillClansGrid_003Ed__44(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass44_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.gridGo = gridGo;
				_003C_003E8__2.state = state;
				if (list == null)
				{
					throw new ArgumentNullException("list");
				}
				_003Cwrap_003E5__1 = _003C_003E8__2.gridGo.GetComponent<UIWrapContent>();
				if (_003Cwrap_003E5__1 == null)
				{
					throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
				}
				_003Cwrap_003E5__1.minIndex = Math.Min(-list.Count + 1, _003Cwrap_003E5__1.maxIndex);
				_003Cwrap_003E5__1.onInitializeItem = null;
				UIWrapContent uIWrapContent = _003Cwrap_003E5__1;
				uIWrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
				{
					int index = -realIndex;
					_003C_003E8__2._003C_003E4__this.FillClanItem(_003C_003E8__2.gridGo, _003C_003E8__2._003C_003E4__this._clansList, _003C_003E8__2.state, index, go);
				});
				int childCount = _003C_003E8__2.gridGo.transform.childCount;
				if (childCount == 0)
				{
					UnityEngine.Debug.LogError("No children in grid.");
					return false;
				}
				Transform child = _003C_003E8__2.gridGo.transform.GetChild(childCount - 1);
				if (child == null)
				{
					UnityEngine.Debug.LogError("Cannot find prototype for item.");
					return false;
				}
				_003C_003E4__this._clansList.Clear();
				_003C_003E4__this._clansList.AddRange(list);
				GameObject gameObject = child.gameObject;
				gameObject.SetActive(_003C_003E4__this._clansList.Count > 0);
				int num = Math.Min(15, _003C_003E4__this._clansList.Count);
				for (int i = 0; i != num; i++)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = _003C_003E4__this._clansList[i];
					GameObject gameObject2;
					if (i < childCount)
					{
						gameObject2 = _003C_003E8__2.gridGo.transform.GetChild(i).gameObject;
					}
					else
					{
						gameObject2 = NGUITools.AddChild(_003C_003E8__2.gridGo, gameObject);
						gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
					}
					_003C_003E4__this.FillClanItem(_003C_003E8__2.gridGo, _003C_003E4__this._clansList, _003C_003E8__2.state, i, gameObject2);
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003Cwrap_003E5__1.SortBasedOnScrollMovement();
				_003Cwrap_003E5__1.WrapContent();
				UIScrollView component = _003C_003E8__2.gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.enabled = true;
					component.ResetPosition();
					component.UpdatePosition();
				}
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass45_0
	{
		public GameObject gridGo;

		public GridState state;

		public LeaderboardScript _003C_003E4__this;

		internal void _003CFillCraftersGrid_003Eb__0(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			_003C_003E4__this.FillIndividualItem(gridGo, _003C_003E4__this._craftersList, state, index, go);
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillCraftersGrid_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public GameObject gridGo;

		public GridState state;

		public List<LeaderboardItemViewModel> list;

		private UIWrapContent _003Cwrap_003E5__1;

		private _003C_003Ec__DisplayClass45_0 _003C_003E8__2;

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
		public _003CFillCraftersGrid_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			UIScrollView component2;
			UIWrapContent component3;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass45_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.gridGo = gridGo;
				_003C_003E8__2.state = state;
				if (list == null)
				{
					throw new ArgumentNullException("list");
				}
				_003Cwrap_003E5__1 = _003C_003E8__2.gridGo.GetComponent<UIWrapContent>();
				if (_003Cwrap_003E5__1 == null)
				{
					throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
				}
				_003Cwrap_003E5__1.minIndex = Math.Min(-list.Count + 1, _003Cwrap_003E5__1.maxIndex);
				_003Cwrap_003E5__1.onInitializeItem = null;
				UIWrapContent uIWrapContent = _003Cwrap_003E5__1;
				uIWrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
				{
					int index = -realIndex;
					_003C_003E8__2._003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E8__2._003C_003E4__this._craftersList, _003C_003E8__2.state, index, go);
				});
				int childCount = _003C_003E8__2.gridGo.transform.childCount;
				if (childCount == 0)
				{
					UnityEngine.Debug.LogError("No children in grid.");
					return false;
				}
				_003C_003E4__this._craftersList.Clear();
				_003C_003E4__this._craftersList.AddRange(list);
				Transform child = _003C_003E8__2.gridGo.transform.GetChild(childCount - 1);
				if (child == null)
				{
					UnityEngine.Debug.LogError("Cannot find prototype for item.");
					return false;
				}
				GameObject gameObject = child.gameObject;
				gameObject.SetActive(_003C_003E4__this._craftersList.Count > 0);
				int num = Math.Min(15, _003C_003E4__this._craftersList.Count);
				for (int i = 0; i != num; i++)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = _003C_003E4__this._craftersList[i];
					GameObject gameObject2;
					if (i < childCount)
					{
						gameObject2 = _003C_003E8__2.gridGo.transform.GetChild(i).gameObject;
					}
					else
					{
						gameObject2 = NGUITools.AddChild(_003C_003E8__2.gridGo, gameObject);
						gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
					}
					_003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E4__this._craftersList, _003C_003E8__2.state, i, gameObject2);
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003Cwrap_003E5__1.SortBasedOnScrollMovement();
				_003Cwrap_003E5__1.WrapContent();
				UIScrollView component = _003C_003E8__2.gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
				if (!(component != null))
				{
					break;
				}
				component.enabled = true;
				component.ResetPosition();
				component.UpdatePosition();
				goto IL_02e8;
			}
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_02e8;
				}
				IL_02e8:
				if (!_003C_003E4__this.gameObject.activeInHierarchy)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				component2 = _003C_003E4__this.CraftersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
				component3 = _003C_003E4__this.CraftersGrid.GetComponent<UIWrapContent>();
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.ScrollGridTo(component2, component3, _003C_003E4__this._craftersList, FriendsController.sharedController.id));
				break;
			}
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass46_0
	{
		public GameObject gridGo;

		public GridState state;

		public LeaderboardScript _003C_003E4__this;

		internal void _003CFillPlayersGrid_003Eb__0(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			_003C_003E4__this.FillIndividualItem(gridGo, _003C_003E4__this._playersList, state, index, go);
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillPlayersGrid_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public GameObject gridGo;

		public GridState state;

		public List<LeaderboardItemViewModel> list;

		private UIWrapContent _003Cwrap_003E5__1;

		private _003C_003Ec__DisplayClass46_0 _003C_003E8__2;

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
		public _003CFillPlayersGrid_003Ed__46(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			UIScrollView component2;
			UIWrapContent component3;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass46_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.gridGo = gridGo;
				_003C_003E8__2.state = state;
				if (list == null)
				{
					throw new ArgumentNullException("list");
				}
				_003Cwrap_003E5__1 = _003C_003E8__2.gridGo.GetComponent<UIWrapContent>();
				if (_003Cwrap_003E5__1 == null)
				{
					throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
				}
				_003Cwrap_003E5__1.minIndex = Math.Min(-list.Count + 1, _003Cwrap_003E5__1.maxIndex);
				_003Cwrap_003E5__1.onInitializeItem = null;
				UIWrapContent uIWrapContent = _003Cwrap_003E5__1;
				uIWrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
				{
					int index = -realIndex;
					_003C_003E8__2._003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E8__2._003C_003E4__this._playersList, _003C_003E8__2.state, index, go);
				});
				int childCount = _003C_003E8__2.gridGo.transform.childCount;
				if (childCount == 0)
				{
					UnityEngine.Debug.LogError("No children in grid.");
					return false;
				}
				Transform child = _003C_003E8__2.gridGo.transform.GetChild(childCount - 1);
				if (child == null)
				{
					UnityEngine.Debug.LogError("Cannot find prototype for item.");
					return false;
				}
				_003C_003E4__this._playersList.Clear();
				_003C_003E4__this._playersList.AddRange(list);
				GameObject gameObject = child.gameObject;
				gameObject.SetActive(_003C_003E4__this._playersList.Count > 0);
				int num = Math.Min(15, _003C_003E4__this._playersList.Count);
				for (int i = 0; i != num; i++)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = _003C_003E4__this._playersList[i];
					GameObject gameObject2;
					if (i < childCount)
					{
						gameObject2 = _003C_003E8__2.gridGo.transform.GetChild(i).gameObject;
					}
					else
					{
						gameObject2 = NGUITools.AddChild(_003C_003E8__2.gridGo, gameObject);
						gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
					}
					_003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E4__this._playersList, _003C_003E8__2.state, i, gameObject2);
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003Cwrap_003E5__1.SortBasedOnScrollMovement();
				_003Cwrap_003E5__1.WrapContent();
				UIScrollView component = _003C_003E8__2.gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
				if (!(component != null))
				{
					break;
				}
				component.enabled = true;
				component.ResetPosition();
				component.UpdatePosition();
				goto IL_02e8;
			}
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_02e8;
				}
				IL_02e8:
				if (!_003C_003E4__this.gameObject.activeInHierarchy)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				component2 = _003C_003E4__this.TopPlayersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
				component3 = _003C_003E4__this.TopPlayersGrid.GetComponent<UIWrapContent>();
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.ScrollGridTo(component2, component3, _003C_003E4__this._playersList, FriendsController.sharedController.id));
				break;
			}
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass52_0
	{
		public string viewId;

		internal bool _003CScrollGridTo_003Eb__0(LeaderboardItemViewModel i)
		{
			return i.Id == viewId;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CScrollGridTo_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string viewId;

		public LeaderboardScript _003C_003E4__this;

		public UIScrollView scroll;

		public UIWrapContent wrapContent;

		public List<LeaderboardItemViewModel> data;

		private _003C_003Ec__DisplayClass52_0 _003C_003E8__1;

		private int _003CvisibleItemsCount_003E5__2;

		private int _003CitemHeight_003E5__3;

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
		public _003CScrollGridTo_003Ed__52(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass52_0();
				_003C_003E8__1.viewId = viewId;
				if (_003C_003E4__this.ScrollRunning)
				{
					return false;
				}
				_003C_003E4__this.ScrollRunning = true;
				if (scroll == null)
				{
					_003C_003E4__this.ScrollRunning = false;
					return false;
				}
				if (wrapContent == null)
				{
					_003C_003E4__this.ScrollRunning = false;
					return false;
				}
				_003CitemHeight_003E5__3 = wrapContent.itemSize;
				float height = scroll.gameObject.GetComponent<UIPanel>().height;
				_003CvisibleItemsCount_003E5__2 = (int)(height / (float)_003CitemHeight_003E5__3);
				UnityEngine.Debug.Log("=>>> visible items count: " + _003CvisibleItemsCount_003E5__2);
				if (data.Count <= _003CvisibleItemsCount_003E5__2)
				{
					_003C_003E4__this.ScrollRunning = false;
					return false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				if (_003C_003E8__1.viewId.IsNullOrEmpty())
				{
					scroll.MoveRelative(scroll.panel.clipOffset);
					_003C_003E4__this.ScrollRunning = false;
					return false;
				}
				LeaderboardItemViewModel leaderboardItemViewModel = data.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == _003C_003E8__1.viewId);
				if (leaderboardItemViewModel == null)
				{
					_003C_003E4__this.ScrollRunning = false;
					return false;
				}
				int value = data.IndexOf(leaderboardItemViewModel);
				int num = data.Count - _003CvisibleItemsCount_003E5__2;
				value = Mathf.Clamp(value, 0, num);
				if (value > num)
				{
					value = num;
				}
				float num2 = _003CitemHeight_003E5__3 * value;
				if (!_003C_003E4__this._startScrollPos.HasValue)
				{
					_003C_003E4__this._startScrollPos = scroll.panel.gameObject.transform.localPosition;
				}
				scroll.panel.clipOffset = Vector2.zero;
				scroll.panel.gameObject.transform.localPosition = _003C_003E4__this._startScrollPos.Value;
				float num3 = Mathf.Abs(num2);
				while (num3 > 0f)
				{
					float num4 = _003CitemHeight_003E5__3 * _003CvisibleItemsCount_003E5__2 - 1;
					if (!(num3 > 0f))
					{
						continue;
					}
					if (num3 - num4 < 0f)
					{
						scroll.MoveRelative(new Vector3(0f, num3));
						num3 = 0f;
						continue;
					}
					num3 -= num4;
					if (num2 < 0f)
					{
						num4 *= -1f;
					}
					scroll.MoveRelative(new Vector3(0f, num4));
				}
				if (value + _003CvisibleItemsCount_003E5__2 >= data.Count)
				{
					scroll.MoveRelative(new Vector3(0f, -60f));
				}
				_003C_003E4__this.ScrollRunning = false;
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass53_0
	{
		public GameObject gridGo;

		public GridState state;

		public LeaderboardScript _003C_003E4__this;

		internal void _003CFillFriendsGrid_003Eb__0(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			_003C_003E4__this.FillIndividualItem(gridGo, _003C_003E4__this._friendsList, state, index, go);
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillFriendsGrid_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public GameObject gridGo;

		public GridState state;

		public List<LeaderboardItemViewModel> list;

		private UIWrapContent _003Cwrap_003E5__1;

		private _003C_003Ec__DisplayClass53_0 _003C_003E8__2;

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
		public _003CFillFriendsGrid_003Ed__53(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass53_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.gridGo = gridGo;
				_003C_003E8__2.state = state;
				if (this.list == null)
				{
					throw new ArgumentNullException("list");
				}
				_003Cwrap_003E5__1 = _003C_003E8__2.gridGo.GetComponent<UIWrapContent>();
				if (_003Cwrap_003E5__1 == null)
				{
					throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
				}
				_003Cwrap_003E5__1.minIndex = Math.Min(-this.list.Count + 1, _003Cwrap_003E5__1.maxIndex);
				_003Cwrap_003E5__1.onInitializeItem = null;
				UIWrapContent uIWrapContent = _003Cwrap_003E5__1;
				uIWrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
				{
					int index = -realIndex;
					_003C_003E8__2._003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E8__2._003C_003E4__this._friendsList, _003C_003E8__2.state, index, go);
				});
				int childCount = _003C_003E8__2.gridGo.transform.childCount;
				if (childCount == 0)
				{
					UnityEngine.Debug.LogError("No children in grid.");
					return false;
				}
				Transform child = _003C_003E8__2.gridGo.transform.GetChild(childCount - 1);
				if (child == null)
				{
					UnityEngine.Debug.LogError("Cannot find prototype for item.");
					return false;
				}
				_003C_003E4__this._friendsList.Clear();
				_003C_003E4__this._friendsList.AddRange(this.list);
				GameObject gameObject = child.gameObject;
				gameObject.SetActive(_003C_003E4__this._friendsList.Count > 0);
				int num = Math.Min(15, _003C_003E4__this._friendsList.Count);
				for (int i = 0; i != num; i++)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = _003C_003E4__this._friendsList[i];
					GameObject gameObject2;
					if (i < childCount)
					{
						gameObject2 = _003C_003E8__2.gridGo.transform.GetChild(i).gameObject;
					}
					else
					{
						gameObject2 = NGUITools.AddChild(_003C_003E8__2.gridGo, gameObject);
						gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
					}
					_003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E4__this._friendsList, _003C_003E8__2.state, i, gameObject2);
				}
				int childCount2 = _003C_003E8__2.gridGo.transform.childCount;
				List<Transform> list = new List<Transform>(Math.Max(0, childCount2 - num));
				for (int j = this.list.Count; j < childCount2; j++)
				{
					list.Add(_003C_003E8__2.gridGo.transform.GetChild(j));
				}
				foreach (Transform item in list)
				{
					NGUITools.Destroy(item);
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003Cwrap_003E5__1.SortBasedOnScrollMovement();
				_003Cwrap_003E5__1.WrapContent();
				UIScrollView component = _003C_003E8__2.gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.enabled = true;
					component.ResetPosition();
					component.UpdatePosition();
				}
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass54_0
	{
		public GameObject gridGo;

		public GridState state;

		public LeaderboardScript _003C_003E4__this;

		internal void _003CFillTournamentGrid_003Eb__0(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			_003C_003E4__this.FillIndividualItem(gridGo, _003C_003E4__this._tournamentList, state, index, go);
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFillTournamentGrid_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		public GameObject gridGo;

		public GridState state;

		public List<LeaderboardItemViewModel> list;

		private UIWrapContent _003Cwrap_003E5__1;

		private _003C_003Ec__DisplayClass54_0 _003C_003E8__2;

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
		public _003CFillTournamentGrid_003Ed__54(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass54_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.gridGo = gridGo;
				_003C_003E8__2.state = state;
				if (list == null)
				{
					throw new ArgumentNullException("list");
				}
				_003Cwrap_003E5__1 = _003C_003E8__2.gridGo.GetComponent<UIWrapContent>();
				if (_003Cwrap_003E5__1 == null)
				{
					throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
				}
				_003Cwrap_003E5__1.minIndex = Math.Min(-list.Count + 1, _003Cwrap_003E5__1.maxIndex);
				_003Cwrap_003E5__1.onInitializeItem = null;
				UIWrapContent uIWrapContent = _003Cwrap_003E5__1;
				uIWrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
				{
					int index = -realIndex;
					_003C_003E8__2._003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E8__2._003C_003E4__this._tournamentList, _003C_003E8__2.state, index, go);
				});
				int childCount = _003C_003E8__2.gridGo.transform.childCount;
				if (childCount == 0)
				{
					UnityEngine.Debug.LogError("No children in grid.");
					return false;
				}
				Transform child = _003C_003E8__2.gridGo.transform.GetChild(childCount - 1);
				if (child == null)
				{
					UnityEngine.Debug.LogError("Cannot find prototype for item.");
					return false;
				}
				_003C_003E4__this._tournamentList.Clear();
				_003C_003E4__this._tournamentList.AddRange(list);
				GameObject gameObject = child.gameObject;
				gameObject.SetActive(_003C_003E4__this._tournamentList.Count > 0);
				int num = Math.Min(15, _003C_003E4__this._tournamentList.Count);
				for (int i = 0; i != num; i++)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = _003C_003E4__this._tournamentList[i];
					GameObject gameObject2;
					if (i < childCount)
					{
						gameObject2 = _003C_003E8__2.gridGo.transform.GetChild(i).gameObject;
					}
					else
					{
						gameObject2 = NGUITools.AddChild(_003C_003E8__2.gridGo, gameObject);
						gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
					}
					_003C_003E4__this.FillIndividualItem(_003C_003E8__2.gridGo, _003C_003E4__this._tournamentList, _003C_003E8__2.state, i, gameObject2);
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003Cwrap_003E5__1.SortBasedOnScrollMovement();
				_003Cwrap_003E5__1.WrapContent();
				UIScrollView component = _003C_003E8__2.gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.enabled = true;
					component.ResetPosition();
					component.UpdatePosition();
				}
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

	[CompilerGenerated]
	internal sealed class _003CShowCoroutine_003Ed__80 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardScript _003C_003E4__this;

		private string _003Cresponse_003E5__1;

		private string _003CplayerId_003E5__2;

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
		public _003CShowCoroutine_003Ed__80(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string result;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!_003C_003E4__this._isInit)
				{
					if (_003C_003E4__this.LeaderboardView != null)
					{
						if (_003C_003E4__this.LeaderboardView.backButton != null)
						{
							_003C_003E4__this.LeaderboardView.backButton.Clicked += _003C_003E4__this.ReturnBack;
						}
						_003C_003E4__this.LeaderboardView.OnStateChanged += _003C_003E4__this.OnLeaderboardViewStateChanged;
					}
					PlayerClicked += _003C_003E4__this.HandlePlayerClicked;
					LocalizationStore.AddEventCallAfterLocalize(_003C_003E4__this.UpdateLocs);
					_003C_003E4__this._isInit = true;
				}
				if (FriendsController.sharedController == null)
				{
					UnityEngine.Debug.LogError("Friends controller is null.");
					return false;
				}
				_003CplayerId_003E5__2 = FriendsController.sharedController.id;
				if (string.IsNullOrEmpty(_003CplayerId_003E5__2))
				{
					UnityEngine.Debug.LogError("Player id should not be null.");
					return false;
				}
				if (_currentRequestPromise == null)
				{
					_currentRequestPromise = new TaskCompletionSource<string>();
					FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(_003CplayerId_003E5__2, _currentRequestPromise));
				}
				if (!((Task)_003C_003E4__this.CurrentRequest).IsCompleted)
				{
					_003Cresponse_003E5__1 = PlayerPrefs.GetString(LeaderboardsResponseCache, string.Empty);
					if (string.IsNullOrEmpty(_003Cresponse_003E5__1))
					{
						_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillGrids(string.Empty, _003CplayerId_003E5__2, _003C_003E4__this._state));
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E4__this._state = GridState.FillingWithCache;
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillGrids(_003Cresponse_003E5__1, _003CplayerId_003E5__2, _003C_003E4__this._state));
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0248;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0228;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this._state = GridState.Cache;
				goto IL_0228;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0248;
			case 4:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this._state = GridState.Response;
					break;
				}
				IL_0228:
				_003Cresponse_003E5__1 = null;
				goto IL_0248;
				IL_0248:
				if (!((Task)_003C_003E4__this.CurrentRequest).IsCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				if (((Task)_003C_003E4__this.CurrentRequest).IsCanceled)
				{
					UnityEngine.Debug.LogWarning("Request is cancelled.");
					break;
				}
				if (((Task)_003C_003E4__this.CurrentRequest).IsFaulted)
				{
					UnityEngine.Debug.LogException((Exception)(object)((Task)_003C_003E4__this.CurrentRequest).Exception);
					break;
				}
				result = _003C_003E4__this.CurrentRequest.Result;
				_003C_003E4__this._state = GridState.FillingWithResponse;
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FillGrids(result, _003CplayerId_003E5__2, _003C_003E4__this._state));
				_003C_003E1__state = 4;
				return true;
			}
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

	[CompilerGenerated]
	internal sealed class _003CLoadLeaderboardsCoroutine_003Ed__87 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TaskCompletionSource<string> requestPromise;

		public string playerId;

		private TaskCompletionSource<string> _003CnewRequestPromise_003E5__1;

		private WWW _003Crequest_003E5__2;

		private TaskCompletionSource<string> _003CnewRequestPromise_003E5__3;

		private TaskCompletionSource<string> _003CnewRequestPromise_003E5__4;

		private string _003CresponseText_003E5__5;

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
		public _003CLoadLeaderboardsCoroutine_003Ed__87(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int leagueId;
			WWWForm wWWForm;
			int i;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (requestPromise == null)
				{
					throw new ArgumentNullException("requestPromise");
				}
				if (((Task)requestPromise.Task).IsCanceled)
				{
					return false;
				}
				if (string.IsNullOrEmpty(playerId))
				{
					throw new ArgumentException("Player id should not be null.", "playerId");
				}
				if (FriendsController.sharedController == null)
				{
					throw new InvalidOperationException("Friends controller should not be null.");
				}
				if (string.IsNullOrEmpty(FriendsController.sharedController.id))
				{
					UnityEngine.Debug.LogWarning("Current player id is empty.");
					requestPromise.TrySetException((Exception)new InvalidOperationException("Current player id is empty."));
					return false;
				}
				DateTime result;
				if (DateTime.TryParse(PlayerPrefs.GetString(LeaderboardsResponseCacheTimestamp, string.Empty), out result))
				{
					float num = (float)(result + TimeSpan.FromMinutes(Defs.pauseUpdateLeaderboard) - DateTime.UtcNow).TotalSeconds;
					if (num > 3600f)
					{
						num = 0f;
					}
					_003C_003E2__current = new WaitForSeconds(num);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0129;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_0129;
			case 2:
				_003C_003E1__state = -1;
				FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _003CnewRequestPromise_003E5__1));
				return false;
			case 3:
				_003C_003E1__state = -1;
				goto IL_02a0;
			case 4:
				_003C_003E1__state = -1;
				FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _003CnewRequestPromise_003E5__3));
				return false;
			case 5:
				{
					_003C_003E1__state = -1;
					FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _003CnewRequestPromise_003E5__4));
					return false;
				}
				IL_0129:
				if (((Task)requestPromise.Task).IsCanceled)
				{
					return false;
				}
				leagueId = GetLeagueId();
				wWWForm = new WWWForm();
				wWWForm.AddField("action", "get_leaderboards_wins_tiers");
				wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
				{
					ProtocolListGetter.CurrentPlatform,
					GlobalGameController.AppVersion
				}));
				wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
				i = ExpController.OurTierForAnyPlace();
				wWWForm.AddField("tier", i);
				wWWForm.AddField("auth", FriendsController.Hash("get_leaderboards_wins_tiers"));
				wWWForm.AddField("league_id", leagueId);
				wWWForm.AddField("competition_id", FriendsController.sharedController.currentCompetition);
				_003Crequest_003E5__2 = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
				if (_003Crequest_003E5__2 == null)
				{
					requestPromise.TrySetException((Exception)new InvalidOperationException("Request forbidden while connected."));
					_003CnewRequestPromise_003E5__1 = new TaskCompletionSource<string>();
					_currentRequestPromise = _003CnewRequestPromise_003E5__1;
					_003C_003E2__current = new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_02a0;
				IL_02a0:
				if (!_003Crequest_003E5__2.isDone)
				{
					if (((Task)requestPromise.Task).IsCanceled)
					{
						return false;
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				if (!string.IsNullOrEmpty(_003Crequest_003E5__2.error))
				{
					requestPromise.TrySetException((Exception)new InvalidOperationException(_003Crequest_003E5__2.error));
					UnityEngine.Debug.LogError(_003Crequest_003E5__2.error);
					_003CnewRequestPromise_003E5__3 = new TaskCompletionSource<string>();
					_currentRequestPromise = _003CnewRequestPromise_003E5__3;
					_003C_003E2__current = new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
					_003C_003E1__state = 4;
					return true;
				}
				_003CresponseText_003E5__5 = URLs.Sanitize(_003Crequest_003E5__2);
				if (string.IsNullOrEmpty(_003CresponseText_003E5__5) || _003CresponseText_003E5__5 == "fail")
				{
					string message = string.Format("Leaderboars response: '{0}'", new object[1] { _003CresponseText_003E5__5 });
					requestPromise.TrySetException((Exception)new InvalidOperationException(message));
					UnityEngine.Debug.LogWarning(message);
					_003CnewRequestPromise_003E5__4 = new TaskCompletionSource<string>();
					_currentRequestPromise = _003CnewRequestPromise_003E5__4;
					_003C_003E2__current = new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
					_003C_003E1__state = 5;
					return true;
				}
				requestPromise.TrySetResult(_003CresponseText_003E5__5);
				PlayerPrefs.SetString(LeaderboardsResponseCache, _003CresponseText_003E5__5);
				PlayerPrefs.SetString(LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));
				return false;
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

	private float _expirationTimeSeconds;

	private float _expirationNextUpateTimeSeconds;

	private bool _fillLock;

	private const int VisibleItemMaxCount = 15;

	private readonly List<LeaderboardItemViewModel> _clansList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _friendsList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _playersList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _craftersList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _tournamentList = new List<LeaderboardItemViewModel>(201);

	private Vector3? _startScrollPos;

	private bool _scrollRunningVal;

	[SerializeField]
	protected internal GameObject _viewHandler;

	[SerializeField]
	protected internal PrefabHandler _viewPrefab;

	private LazyObject<LeaderboardsView> _view;

	private UIPanel _panelVal;

	private bool _isInit;

	private Lazy<MainMenuController> _mainMenuController;

	private TaskCompletionSource<bool> _returnPromise = new TaskCompletionSource<bool>();

	private bool _profileIsOpened;

	private static TaskCompletionSource<string> _currentRequestPromise;

	private GridState _state;

	private LeaderboardsView LeaderboardView
	{
		get
		{
			return _view.Value;
		}
	}

	public UILabel ExpirationLabel
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.expirationLabel;
			}
			return null;
		}
	}

	public GameObject ExpirationIconObject
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.expirationIconObj;
			}
			return null;
		}
	}

	private GameObject TopFriendsGrid
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.friendsGrid.gameObject;
			}
			return null;
		}
	}

	private GameObject TopPlayersGrid
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.bestPlayersGrid.gameObject;
			}
			return null;
		}
	}

	private GameObject TopClansGrid
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.clansGrid.gameObject;
			}
			return null;
		}
	}

	private GameObject TournamentGrid
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.tournamentGrid.gameObject;
			}
			return null;
		}
	}

	private GameObject CraftersGrid
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			if (LeaderboardView.craftersGrid == null)
			{
				return null;
			}
			return LeaderboardView.craftersGrid.gameObject;
		}
	}

	private GameObject ClansTableFooter
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.clansTableFooter;
			}
			return null;
		}
	}

	private GameObject Headerleaderboard
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.leaderboardHeader;
			}
			return null;
		}
	}

	private GameObject FooterLeaderboard
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.leaderboardFooter;
			}
			return null;
		}
	}

	private GameObject FooterTournament
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.tournamentFooter;
			}
			return null;
		}
	}

	private GameObject FooterTableTournament
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.tournamentTableFooter;
			}
			return null;
		}
	}

	private GameObject HeaderTournament
	{
		get
		{
			if (!(LeaderboardView == null))
			{
				return LeaderboardView.tournamentHeader;
			}
			return null;
		}
	}

	private bool ScrollRunning
	{
		get
		{
			return _scrollRunningVal;
		}
		set
		{
			_scrollRunningVal = value;
		}
	}

	public static LeaderboardScript Instance { get; private set; }

	public UIPanel Panel
	{
		get
		{
			if (_panelVal == null)
			{
				_panelVal = ((_view != null && _view.ObjectIsLoaded) ? _view.Value.gameObject.GetComponent<UIPanel>() : null);
			}
			return _panelVal;
		}
	}

	public bool UIEnabled
	{
		get
		{
			if (_view != null)
			{
				return _view.ObjectIsActive;
			}
			return false;
		}
	}

	private Task<string> CurrentRequest
	{
		get
		{
			return _currentRequestPromise.Map((TaskCompletionSource<string> p) => p.Task);
		}
	}

	private static string LeaderboardsResponseCache
	{
		get
		{
			return "Leaderboards.Tier.ResponseCache-v2";
		}
	}

	public static string LeaderboardsResponseCacheTimestamp
	{
		get
		{
			return "Leaderboards.New.ResponseCacheTimestamp-v2";
		}
	}

	public static event EventHandler<ClickedEventArgs> PlayerClicked;

	private void OnLeaderboardViewStateChanged(LeaderboardsView.State fromState, LeaderboardsView.State toState)
	{
		if (fromState != toState)
		{
			switch (toState)
			{
			case LeaderboardsView.State.BestPlayers:
			{
				UIScrollView component3 = TopPlayersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
				UIWrapContent component4 = TopPlayersGrid.GetComponent<UIWrapContent>();
				StartCoroutine(ScrollGridTo(component3, component4, _playersList, FriendsController.sharedController.id));
				break;
			}
			case LeaderboardsView.State.Crafters:
			{
				UIScrollView component = CraftersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
				UIWrapContent component2 = CraftersGrid.GetComponent<UIWrapContent>();
				StartCoroutine(ScrollGridTo(component, component2, _craftersList, FriendsController.sharedController.id));
				break;
			}
			}
		}
	}

	private void UpdateLocs()
	{
		if (ClansTableFooter != null)
		{
			ClansTableFooter.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
			{
				n.text = LocalizationStore.Get("Key_0053");
			});
		}
	}

	private IEnumerator FillGrids(string response, string playerId, GridState state)
	{
		while (_fillLock)
		{
			yield return null;
		}
		_fillLock = true;
		try
		{
			if (string.IsNullOrEmpty(playerId))
			{
				throw new ArgumentException("Player id should not be empty", "playerId");
			}
			Dictionary<string, object> d = Json.Deserialize(response ?? string.Empty) as Dictionary<string, object>;
			if (d == null)
			{
				UnityEngine.Debug.LogWarning("Leaderboards response is ill-formed.");
				d = new Dictionary<string, object>();
			}
			else if (d.Count == 0)
			{
				UnityEngine.Debug.LogWarning("Leaderboards response contains no elements.");
			}
			LeaderboardItemViewModel me = new LeaderboardItemViewModel
			{
				Id = playerId,
				Nickname = ProfileController.GetPlayerNameOrDefault(),
				Rank = ExperienceController.sharedController.currentLevel,
				WinCount = RatingSystem.instance.currentRating,
				Highlight = true,
				ClanName = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty),
				ClanLogo = FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty)
			};
			object value;
			object value2;
			if (d.TryGetValue("me", out value) && (value as Dictionary<string, object>).TryGetValue("wins", out value2))
			{
				int value3 = Convert.ToInt32(value2);
				PlayerPrefs.SetInt("TotalWinsForLeaderboards", value3);
			}
			int result;
			if (d.ContainsKey("competition_id") && int.TryParse(d["competition_id"].ToString(), out result) && FriendsController.sharedController != null && result > FriendsController.sharedController.currentCompetition)
			{
				FriendsController.sharedController.SendRequestGetCurrentcompetition();
			}
			List<LeaderboardItemViewModel> list = LeaderboardsController.ParseLeaderboardEntries(playerId, "friends", d);
			HashSet<string> hashSet = new HashSet<string>(FriendsController.sharedController.friends);
			if (FriendsController.sharedController != null)
			{
				for (int num = list.Count - 1; num >= 0; num--)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = list[num];
					Dictionary<string, object> value4;
					if (hashSet.Contains(leaderboardItemViewModel.Id) && FriendsController.sharedController.friendsInfo.TryGetValue(leaderboardItemViewModel.Id, out value4))
					{
						try
						{
							Dictionary<string, object> dictionary = value4["player"] as Dictionary<string, object>;
							leaderboardItemViewModel.Nickname = Convert.ToString(dictionary["nick"]);
							leaderboardItemViewModel.Rank = Convert.ToInt32(dictionary["rank"]);
							object value5;
							if (dictionary.TryGetValue("clan_name", out value5))
							{
								leaderboardItemViewModel.ClanName = Convert.ToString(value5);
							}
							object value6;
							if (dictionary.TryGetValue("clan_logo", out value6))
							{
								leaderboardItemViewModel.ClanLogo = Convert.ToString(value6);
							}
						}
						catch (KeyNotFoundException)
						{
							UnityEngine.Debug.LogError("Failed to process cached friend: " + leaderboardItemViewModel.Id);
						}
					}
					else
					{
						list.RemoveAt(num);
					}
				}
			}
			list.Add(me);
			List<LeaderboardItemViewModel> list2 = GroupAndOrder(list);
			yield return StartCoroutine(FillFriendsGrid(TopFriendsGrid, list2, state));
			List<LeaderboardItemViewModel> list3 = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_likes", d);
			LeaderboardItemViewModel leaderboardItemViewModel2 = list3.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == me.Id);
			int winCount = ((!(Singleton<LobbyItemsController>.Instance == null)) ? Singleton<LobbyItemsController>.Instance.GetLikeCount() : 0);
			if (leaderboardItemViewModel2 == null)
			{
				leaderboardItemViewModel2 = new LeaderboardItemViewModel
				{
					Id = me.Id,
					Nickname = me.Nickname,
					Rank = me.Rank,
					WinCount = winCount,
					Highlight = true
				};
				list3.Add(leaderboardItemViewModel2);
			}
			else
			{
				leaderboardItemViewModel2.Nickname = me.Nickname;
				leaderboardItemViewModel2.Rank = me.Rank;
				leaderboardItemViewModel2.WinCount = winCount;
				leaderboardItemViewModel2.Highlight = true;
			}
			List<LeaderboardItemViewModel> list4 = GroupAndOrder(list3);
			Coroutine fillCraftersCoroutine = StartCoroutine(FillCraftersGrid(CraftersGrid, list4, state));
			List<LeaderboardItemViewModel> list5 = LeaderboardsController.ParseLeaderboardEntries(playerId, "best_players", d);
			if (list5.Any())
			{
				if (list5.All((LeaderboardItemViewModel i) => i.Id != me.Id))
				{
					LeaderboardItemViewModel item = list5.OrderByDescending((LeaderboardItemViewModel i) => i.WinCount).Last();
					list5.Remove(item);
					list5.Add(me);
				}
				else
				{
					LeaderboardItemViewModel leaderboardItemViewModel3 = list5.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == me.Id);
					if (leaderboardItemViewModel3 != null)
					{
						leaderboardItemViewModel3.Nickname = me.Nickname;
						leaderboardItemViewModel3.Rank = me.Rank;
						leaderboardItemViewModel3.WinCount = me.WinCount;
						leaderboardItemViewModel3.Highlight = me.Highlight;
						leaderboardItemViewModel3.ClanName = me.ClanName;
						leaderboardItemViewModel3.ClanLogo = me.ClanLogo;
					}
				}
			}
			List<LeaderboardItemViewModel> list6 = GroupAndOrder(list5);
			AddCacheInProfileInfo(list5);
			Coroutine coroutine = StartCoroutine(FillPlayersGrid(TopPlayersGrid, list6, state));
			list5.Clear();
			List<LeaderboardItemViewModel> list7 = LeaderboardsController.ParseLeaderboardEntries(playerId, "competition", d);
			if (list7.Any())
			{
				if (list7.All((LeaderboardItemViewModel i) => i.Id != me.Id))
				{
					if (list7.Any((LeaderboardItemViewModel i) => i.WinCount <= me.WinCount))
					{
						LeaderboardItemViewModel item2 = list7.OrderByDescending((LeaderboardItemViewModel i) => i.WinCount).Last();
						list7.Remove(item2);
						list7.Add(me);
					}
				}
				else
				{
					LeaderboardItemViewModel leaderboardItemViewModel4 = list7.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == me.Id);
					if (leaderboardItemViewModel4 != null)
					{
						leaderboardItemViewModel4.Nickname = me.Nickname;
						leaderboardItemViewModel4.Rank = me.Rank;
						leaderboardItemViewModel4.WinCount = me.WinCount;
						leaderboardItemViewModel4.Highlight = me.Highlight;
						leaderboardItemViewModel4.ClanName = me.ClanName;
						leaderboardItemViewModel4.ClanLogo = me.ClanLogo;
					}
				}
			}
			LeaderboardView.InTournamentTop = list7.Any() && list7.All((LeaderboardItemViewModel i) => i.Id != me.Id);
			if (FooterTableTournament != null)
			{
				if (LeaderboardView.InTournamentTop)
				{
					FooterTableTournament.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = LocalizationStore.Get("Key_0053");
					});
					FooterTableTournament.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = me.Nickname;
					});
					FooterTableTournament.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = RatingSystem.instance.currentRating.ToString();
					});
					FooterTableTournament.SetActiveSafe(true);
				}
				else
				{
					FooterTableTournament.SetActiveSafe(false);
				}
			}
			List<LeaderboardItemViewModel> list8 = GroupAndOrder(list7);
			Coroutine fillTournamentCoroutine = StartCoroutine(FillTournamentGrid(TournamentGrid, list8, state));
			list7.Clear();
			List<LeaderboardItemViewModel> list9 = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_clans", d);
			List<LeaderboardItemViewModel> list10 = GroupAndOrder(list9);
			Coroutine fillClansCoroutine = StartCoroutine(FillClansGrid(TopClansGrid, list10, state));
			if (ClansTableFooter != null)
			{
				string clanId = FriendsController.sharedController.Map((FriendsController s) => s.ClanID);
				if (string.IsNullOrEmpty(clanId))
				{
					LeaderboardView.CanShowClanTableFooter = false;
					ClansTableFooter.SetActive(false);
				}
				else
				{
					LeaderboardItemViewModel leaderboardItemViewModel5 = list9.FirstOrDefault((LeaderboardItemViewModel c) => c.Id == clanId);
					LeaderboardView.CanShowClanTableFooter = leaderboardItemViewModel5 == null;
					ClansTableFooter.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = LocalizationStore.Get("Key_0053");
					});
					ClansTableFooter.transform.FindChild("LabelMembers").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = string.Empty;
					});
					ClansTableFooter.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel w)
					{
						w.text = string.Empty;
					});
					UILabel o = ClansTableFooter.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>());
					o.Do(delegate(UILabel cl)
					{
						cl.text = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty);
					});
					o.Map((UILabel cl) => cl.GetComponentsInChildren<UITexture>(true).FirstOrDefault()).Do(delegate(UITexture t)
					{
						SetClanLogo(t, FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty));
					});
				}
			}
			yield return coroutine;
			yield return fillClansCoroutine;
			yield return fillTournamentCoroutine;
			yield return fillCraftersCoroutine;
		}
		finally
		{
			_fillLock = false;
		}
	}

	private void AddCacheInProfileInfo(List<LeaderboardItemViewModel> _list)
	{
		foreach (LeaderboardItemViewModel item in _list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("nick", item.Nickname);
			dictionary.Add("rank", item.Rank);
			dictionary.Add("clan_name", item.ClanName);
			dictionary.Add("clan_logo", item.ClanLogo);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("player", dictionary);
			if (!FriendsController.sharedController.profileInfo.ContainsKey(item.Id))
			{
				FriendsController.sharedController.profileInfo.Add(item.Id, dictionary2);
			}
			else
			{
				FriendsController.sharedController.profileInfo[item.Id] = dictionary2;
			}
		}
	}

	private IEnumerator FillClansGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			FillClanItem(gridGo, _clansList, state, index, go);
		});
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			UnityEngine.Debug.LogError("No children in grid.");
			yield break;
		}
		Transform child = gridGo.transform.GetChild(childCount - 1);
		if (child == null)
		{
			UnityEngine.Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_clansList.Clear();
		_clansList.AddRange(list);
		GameObject gameObject = child.gameObject;
		gameObject.SetActive(_clansList.Count > 0);
		int num = Math.Min(15, _clansList.Count);
		for (int i = 0; i != num; i++)
		{
			LeaderboardItemViewModel leaderboardItemViewModel = _clansList[i];
			GameObject gameObject2;
			if (i < childCount)
			{
				gameObject2 = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				gameObject2 = NGUITools.AddChild(gridGo, gameObject);
				gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillClanItem(gridGo, _clansList, state, i, gameObject2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView component = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (component != null)
		{
			component.enabled = true;
			component.ResetPosition();
			component.UpdatePosition();
		}
	}

	private IEnumerator FillCraftersGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			FillIndividualItem(gridGo, _craftersList, state, index, go);
		});
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			UnityEngine.Debug.LogError("No children in grid.");
			yield break;
		}
		_craftersList.Clear();
		_craftersList.AddRange(list);
		Transform child = gridGo.transform.GetChild(childCount - 1);
		if (child == null)
		{
			UnityEngine.Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		GameObject gameObject = child.gameObject;
		gameObject.SetActive(_craftersList.Count > 0);
		int num = Math.Min(15, _craftersList.Count);
		for (int i = 0; i != num; i++)
		{
			LeaderboardItemViewModel leaderboardItemViewModel = _craftersList[i];
			GameObject gameObject2;
			if (i < childCount)
			{
				gameObject2 = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				gameObject2 = NGUITools.AddChild(gridGo, gameObject);
				gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _craftersList, state, i, gameObject2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView component = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (component != null)
		{
			component.enabled = true;
			component.ResetPosition();
			component.UpdatePosition();
			while (!this.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			UIScrollView component2 = CraftersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
			UIWrapContent component3 = CraftersGrid.GetComponent<UIWrapContent>();
			StartCoroutine(ScrollGridTo(component2, component3, _craftersList, FriendsController.sharedController.id));
		}
	}

	private IEnumerator FillPlayersGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			FillIndividualItem(gridGo, _playersList, state, index, go);
		});
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			UnityEngine.Debug.LogError("No children in grid.");
			yield break;
		}
		Transform child = gridGo.transform.GetChild(childCount - 1);
		if (child == null)
		{
			UnityEngine.Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_playersList.Clear();
		_playersList.AddRange(list);
		GameObject gameObject = child.gameObject;
		gameObject.SetActive(_playersList.Count > 0);
		int num = Math.Min(15, _playersList.Count);
		for (int i = 0; i != num; i++)
		{
			LeaderboardItemViewModel leaderboardItemViewModel = _playersList[i];
			GameObject gameObject2;
			if (i < childCount)
			{
				gameObject2 = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				gameObject2 = NGUITools.AddChild(gridGo, gameObject);
				gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _playersList, state, i, gameObject2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView component = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (component != null)
		{
			component.enabled = true;
			component.ResetPosition();
			component.UpdatePosition();
			while (!this.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			UIScrollView component2 = TopPlayersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
			UIWrapContent component3 = TopPlayersGrid.GetComponent<UIWrapContent>();
			StartCoroutine(ScrollGridTo(component2, component3, _playersList, FriendsController.sharedController.id));
		}
	}

	private IEnumerator ScrollGridTo(UIScrollView scroll, UIWrapContent wrapContent, List<LeaderboardItemViewModel> data, string viewId = null)
	{
		if (ScrollRunning)
		{
			yield break;
		}
		ScrollRunning = true;
		if (scroll == null)
		{
			ScrollRunning = false;
			yield break;
		}
		if (wrapContent == null)
		{
			ScrollRunning = false;
			yield break;
		}
		int itemHeight = wrapContent.itemSize;
		float height = scroll.gameObject.GetComponent<UIPanel>().height;
		int visibleItemsCount = (int)(height / (float)itemHeight);
		UnityEngine.Debug.Log("=>>> visible items count: " + visibleItemsCount);
		if (data.Count <= visibleItemsCount)
		{
			ScrollRunning = false;
			yield break;
		}
		yield return null;
		if (viewId.IsNullOrEmpty())
		{
			scroll.MoveRelative(scroll.panel.clipOffset);
			ScrollRunning = false;
			yield break;
		}
		LeaderboardItemViewModel leaderboardItemViewModel = data.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == viewId);
		if (leaderboardItemViewModel == null)
		{
			ScrollRunning = false;
			yield break;
		}
		int value = data.IndexOf(leaderboardItemViewModel);
		int num = data.Count - visibleItemsCount;
		value = Mathf.Clamp(value, 0, num);
		if (value > num)
		{
			value = num;
		}
		float num2 = itemHeight * value;
		if (!_startScrollPos.HasValue)
		{
			_startScrollPos = scroll.panel.gameObject.transform.localPosition;
		}
		scroll.panel.clipOffset = Vector2.zero;
		scroll.panel.gameObject.transform.localPosition = _startScrollPos.Value;
		float num3 = Mathf.Abs(num2);
		while (num3 > 0f)
		{
			float num4 = itemHeight * visibleItemsCount - 1;
			if (!(num3 > 0f))
			{
				continue;
			}
			if (num3 - num4 < 0f)
			{
				scroll.MoveRelative(new Vector3(0f, num3));
				num3 = 0f;
				continue;
			}
			num3 -= num4;
			if (num2 < 0f)
			{
				num4 *= -1f;
			}
			scroll.MoveRelative(new Vector3(0f, num4));
		}
		if (value + visibleItemsCount >= data.Count)
		{
			scroll.MoveRelative(new Vector3(0f, -60f));
		}
		ScrollRunning = false;
	}

	private IEnumerator FillFriendsGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			FillIndividualItem(gridGo, _friendsList, state, index, go);
		});
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			UnityEngine.Debug.LogError("No children in grid.");
			yield break;
		}
		Transform child = gridGo.transform.GetChild(childCount - 1);
		if (child == null)
		{
			UnityEngine.Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_friendsList.Clear();
		_friendsList.AddRange(list);
		GameObject gameObject = child.gameObject;
		gameObject.SetActive(_friendsList.Count > 0);
		int num = Math.Min(15, _friendsList.Count);
		for (int i = 0; i != num; i++)
		{
			LeaderboardItemViewModel leaderboardItemViewModel = _friendsList[i];
			GameObject gameObject2;
			if (i < childCount)
			{
				gameObject2 = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				gameObject2 = NGUITools.AddChild(gridGo, gameObject);
				gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _friendsList, state, i, gameObject2);
		}
		int childCount2 = gridGo.transform.childCount;
		List<Transform> list2 = new List<Transform>(Math.Max(0, childCount2 - num));
		for (int j = list.Count; j < childCount2; j++)
		{
			list2.Add(gridGo.transform.GetChild(j));
		}
		foreach (Transform item in list2)
		{
			NGUITools.Destroy(item);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView component = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (component != null)
		{
			component.enabled = true;
			component.ResetPosition();
			component.UpdatePosition();
		}
	}

	private IEnumerator FillTournamentGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, (UIWrapContent.OnInitializeItem)delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			FillIndividualItem(gridGo, _tournamentList, state, index, go);
		});
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			UnityEngine.Debug.LogError("No children in grid.");
			yield break;
		}
		Transform child = gridGo.transform.GetChild(childCount - 1);
		if (child == null)
		{
			UnityEngine.Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_tournamentList.Clear();
		_tournamentList.AddRange(list);
		GameObject gameObject = child.gameObject;
		gameObject.SetActive(_tournamentList.Count > 0);
		int num = Math.Min(15, _tournamentList.Count);
		for (int i = 0; i != num; i++)
		{
			LeaderboardItemViewModel leaderboardItemViewModel = _tournamentList[i];
			GameObject gameObject2;
			if (i < childCount)
			{
				gameObject2 = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				gameObject2 = NGUITools.AddChild(gridGo, gameObject);
				gameObject2.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _tournamentList, state, i, gameObject2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView component = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (component != null)
		{
			component.enabled = true;
			component.ResetPosition();
			component.UpdatePosition();
		}
	}

	internal void RefreshMyLeaderboardEntries()
	{
		foreach (LeaderboardItemViewModel friends in _friendsList)
		{
			if (friends != null && friends.Id == FriendsController.sharedController.id)
			{
				friends.Nickname = ProfileController.GetPlayerNameOrDefault();
				friends.ClanName = FriendsController.sharedController.clanName ?? string.Empty;
				break;
			}
		}
	}

	private void FillIndividualItem(GameObject grid, List<LeaderboardItemViewModel> list, GridState state, int index, GameObject newItem)
	{
		if (index >= list.Count || index < 0)
		{
			return;
		}
		LeaderboardItemViewModel item = list[index];
		int num = index + 1;
		item.Place = num;
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(item);
			if (component.background != null)
			{
				if ((float)num % 2f > 0f)
				{
					Color color = new Color(0.8f, 0.9f, 1f);
					component.GetComponent<UIButton>().defaultColor = color;
					component.background.color = color;
				}
				else
				{
					Color color2 = new Color(1f, 1f, 1f);
					component.GetComponent<UIButton>().defaultColor = color2;
					component.background.color = color2;
				}
			}
		}
		component.Clicked += delegate(object sender, ClickedEventArgs e)
		{
			LeaderboardScript.PlayerClicked.Do(delegate(EventHandler<ClickedEventArgs> handler)
			{
				handler(this, e);
			});
			if (Application.isEditor && Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Format("Clicked: {0}", new object[1] { e.Id }));
			}
		};
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] array = new Transform[3]
		{
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map((UILabel l) => l.transform),
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map((UILabel l) => l.transform),
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map((UILabel l) => l.transform)
		};
		int p2 = 0;
		while (p2 != array.Length)
		{
			array[p2].Do(delegate(Transform l)
			{
				l.gameObject.SetActive(p2 + 1 == item.Place && item.WinCount > 0);
			});
			int num2 = p2 + 1;
			p2 = num2;
		}
		newItem.transform.FindChild("LabelsPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel p)
		{
			p.text = ((item.Place > 3) ? item.Place.ToString(CultureInfo.InvariantCulture) : string.Empty);
		});
	}

	private void FillClanItem(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state, int index, GameObject newItem)
	{
		if (index >= list.Count)
		{
			return;
		}
		LeaderboardItemViewModel item = list[index];
		item.Place = index + 1;
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(item);
		}
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] array = new Transform[3]
		{
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map((UILabel l) => l.transform),
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map((UILabel l) => l.transform),
			componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map((UILabel l) => l.transform)
		};
		int p2 = 0;
		while (p2 != array.Length)
		{
			array[p2].Do(delegate(Transform l)
			{
				l.gameObject.SetActive(p2 + 1 == item.Place);
			});
			int num = p2 + 1;
			p2 = num;
		}
		newItem.transform.FindChild("LabelsPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel p)
		{
			p.text = ((item.Place > 3) ? item.Place.ToString(CultureInfo.InvariantCulture) : string.Empty);
		});
	}

	internal static void SetClanLogo(UITexture s, Texture2D clanLogoTexture)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		s.mainTexture = ((clanLogoTexture != null) ? UnityEngine.Object.Instantiate(clanLogoTexture) : null);
		mainTexture.Do(UnityEngine.Object.Destroy);
	}

	internal static void SetClanLogo(UITexture s, string clanLogo)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		if (string.IsNullOrEmpty(clanLogo))
		{
			s.mainTexture = null;
		}
		else
		{
			s.mainTexture = LeaderboardItemViewModel.CreateLogoFromBase64String(clanLogo);
		}
		mainTexture.Do(UnityEngine.Object.Destroy);
	}

	private static List<LeaderboardItemViewModel> GroupAndOrder(List<LeaderboardItemViewModel> items)
	{
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
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
				list.Add(item2);
			}
			num += item.Count();
		}
		return list;
	}

	public static int GetLeagueId()
	{
		return (int)RatingSystem.instance.currentLeague;
	}

	internal static void RequestLeaderboards(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			UnityEngine.Debug.LogError("Friends controller is null.");
			return;
		}
		if (_currentRequestPromise != null)
		{
			_currentRequestPromise.TrySetCanceled();
		}
		PlayerPrefs.SetString(LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(2.0)).ToString("s", CultureInfo.InvariantCulture));
		_currentRequestPromise = new TaskCompletionSource<string>();
		FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _currentRequestPromise));
	}

	private void HandlePlayerClicked(object sender, ClickedEventArgs e)
	{
		if (e == null || e.Id.IsNullOrEmpty() || (FriendsController.sharedController != null && e.Id == FriendsController.sharedController.id))
		{
			return;
		}
		if (Panel == null)
		{
			UnityEngine.Debug.LogError("Leaderboards panel not found.");
			return;
		}
		Panel.alpha = float.Epsilon;
		Panel.gameObject.SetActive(false);
		Action<bool> onCloseEvent = delegate
		{
			Panel.gameObject.SetActive(true);
			TopFriendsGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TopClansGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TournamentGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			CraftersGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TopFriendsGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			TopClansGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			TournamentGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			CraftersGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			Panel.alpha = 1f;
			_profileIsOpened = false;
		};
		_profileIsOpened = true;
		FriendsController.ShowProfile(e.Id, ProfileWindowType.other, onCloseEvent);
	}

	private void Awake()
	{
		Instance = this;
		_view = new LazyObject<LeaderboardsView>(_viewPrefab.ResourcePath, _viewHandler);
		_mainMenuController = new Lazy<MainMenuController>(() => MainMenuController.sharedController);
	}

	private void OnDestroy()
	{
		if (_currentRequestPromise != null)
		{
			_currentRequestPromise.TrySetCanceled();
		}
		_currentRequestPromise = null;
		LeaderboardScript.PlayerClicked = null;
		FriendsController.DisposeProfile();
		_mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed -= ReturnBack;
		});
		LocalizationStore.DelEventCallAfterLocalize(UpdateLocs);
	}

	public void Show()
	{
		StartCoroutine(ShowCoroutine());
	}

	private IEnumerator ShowCoroutine()
	{
		if (!_isInit)
		{
			if (LeaderboardView != null)
			{
				if (LeaderboardView.backButton != null)
				{
					LeaderboardView.backButton.Clicked += ReturnBack;
				}
				LeaderboardView.OnStateChanged += OnLeaderboardViewStateChanged;
			}
			PlayerClicked += HandlePlayerClicked;
			LocalizationStore.AddEventCallAfterLocalize(UpdateLocs);
			_isInit = true;
		}
		if (FriendsController.sharedController == null)
		{
			UnityEngine.Debug.LogError("Friends controller is null.");
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			UnityEngine.Debug.LogError("Player id should not be null.");
			yield break;
		}
		if (_currentRequestPromise == null)
		{
			_currentRequestPromise = new TaskCompletionSource<string>();
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _currentRequestPromise));
		}
		if (!((Task)CurrentRequest).IsCompleted)
		{
			string response = PlayerPrefs.GetString(LeaderboardsResponseCache, string.Empty);
			if (string.IsNullOrEmpty(response))
			{
				yield return StartCoroutine(FillGrids(string.Empty, playerId, _state));
			}
			else
			{
				_state = GridState.FillingWithCache;
				yield return StartCoroutine(FillGrids(response, playerId, _state));
				_state = GridState.Cache;
			}
		}
		while (!((Task)CurrentRequest).IsCompleted)
		{
			yield return null;
		}
		if (((Task)CurrentRequest).IsCanceled)
		{
			UnityEngine.Debug.LogWarning("Request is cancelled.");
			yield break;
		}
		if (((Task)CurrentRequest).IsFaulted)
		{
			UnityEngine.Debug.LogException((Exception)(object)((Task)CurrentRequest).Exception);
			yield break;
		}
		string result = CurrentRequest.Result;
		_state = GridState.FillingWithResponse;
		yield return StartCoroutine(FillGrids(result, playerId, _state));
		_state = GridState.Response;
	}

	public void FillGrids(string rawData)
	{
		StartCoroutine(FillGrids(rawData, FriendsController.sharedController.id, GridState.Response));
	}

	private static string FormatExpirationLabel(float expirationTimespanSeconds)
	{
		if (expirationTimespanSeconds < 0f)
		{
			throw new ArgumentOutOfRangeException("expirationTimespanSeconds");
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(expirationTimespanSeconds);
		try
		{
			return string.Format(LocalizationStore.Get("Key_2857"), new object[3]
			{
				Convert.ToInt32(Math.Floor(timeSpan.TotalDays)),
				timeSpan.Hours,
				timeSpan.Minutes
			});
		}
		catch
		{
			if (timeSpan.TotalHours < 1.0)
			{
				return string.Format("{0}:{1:00}", new object[2] { timeSpan.Minutes, timeSpan.Seconds });
			}
			if (timeSpan.TotalDays < 1.0)
			{
				return string.Format("{0}:{1:00}:{2:00}", new object[3] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
			}
			return string.Format("{0}d {1}:{2:00}:{3:00}", Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
	}

	private void Update()
	{
		if (!_isInit || !(Time.realtimeSinceStartup > _expirationNextUpateTimeSeconds))
		{
			return;
		}
		_expirationNextUpateTimeSeconds = Time.realtimeSinceStartup + 5f;
		if (!(ExpirationLabel != null))
		{
			return;
		}
		if (_state == GridState.Empty)
		{
			ExpirationLabel.text = LocalizationStore.Key_0348;
			return;
		}
		float num = FriendsController.sharedController.expirationTimeCompetition - Time.realtimeSinceStartup;
		if (num <= 0f)
		{
			ExpirationLabel.text = string.Empty;
			ExpirationIconObject.SetActiveSafe(false);
		}
		else
		{
			ExpirationLabel.text = FormatExpirationLabel(num);
			ExpirationIconObject.SetActiveSafe(true);
		}
	}

	private void OnDisable()
	{
		ScrollRunning = false;
	}

	private static IEnumerator LoadLeaderboardsCoroutine(string playerId, TaskCompletionSource<string> requestPromise)
	{
		if (requestPromise == null)
		{
			throw new ArgumentNullException("requestPromise");
		}
		if (((Task)requestPromise.Task).IsCanceled)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be null.", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			throw new InvalidOperationException("Friends controller should not be null.");
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			UnityEngine.Debug.LogWarning("Current player id is empty.");
			requestPromise.TrySetException((Exception)new InvalidOperationException("Current player id is empty."));
			yield break;
		}
		DateTime result;
		if (DateTime.TryParse(PlayerPrefs.GetString(LeaderboardsResponseCacheTimestamp, string.Empty), out result))
		{
			float num = (float)(result + TimeSpan.FromMinutes(Defs.pauseUpdateLeaderboard) - DateTime.UtcNow).TotalSeconds;
			if (num > 3600f)
			{
				num = 0f;
			}
			yield return new WaitForSeconds(num);
		}
		if (((Task)requestPromise.Task).IsCanceled)
		{
			yield break;
		}
		int leagueId = GetLeagueId();
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_leaderboards_wins_tiers");
		wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		}));
		wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
		int i = ExpController.OurTierForAnyPlace();
		wWWForm.AddField("tier", i);
		wWWForm.AddField("auth", FriendsController.Hash("get_leaderboards_wins_tiers"));
		wWWForm.AddField("league_id", leagueId);
		wWWForm.AddField("competition_id", FriendsController.sharedController.currentCompetition);
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
		if (request == null)
		{
			requestPromise.TrySetException((Exception)new InvalidOperationException("Request forbidden while connected."));
			TaskCompletionSource<string> newRequestPromise3 = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise3));
			yield break;
		}
		while (!request.isDone)
		{
			if (((Task)requestPromise.Task).IsCanceled)
			{
				yield break;
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			requestPromise.TrySetException((Exception)new InvalidOperationException(request.error));
			UnityEngine.Debug.LogError(request.error);
			TaskCompletionSource<string> newRequestPromise2 = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise2));
			yield break;
		}
		string responseText = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(responseText) || responseText == "fail")
		{
			string message = string.Format("Leaderboars response: '{0}'", new object[1] { responseText });
			requestPromise.TrySetException((Exception)new InvalidOperationException(message));
			UnityEngine.Debug.LogWarning(message);
			TaskCompletionSource<string> newRequestPromise = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise));
		}
		else
		{
			requestPromise.TrySetResult(responseText);
			PlayerPrefs.SetString(LeaderboardsResponseCache, responseText);
			PlayerPrefs.SetString(LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));
		}
	}

	public Task GetReturnFuture()
	{
		if (((Task)_returnPromise.Task).IsCompleted)
		{
			_returnPromise = new TaskCompletionSource<bool>();
		}
		_mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed -= ReturnBack;
		});
		_mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed += ReturnBack;
		});
		return (Task)(object)_returnPromise.Task;
	}

	public void ReturnBack(object sender, EventArgs e)
	{
		if (!_profileIsOpened)
		{
			TopFriendsGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TopClansGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TournamentGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			CraftersGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			TopFriendsGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			TopClansGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			TournamentGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			CraftersGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			_returnPromise.TrySetResult(true);
			_mainMenuController.Value.Do(delegate(MainMenuController m)
			{
				m.BackPressed -= ReturnBack;
			});
		}
	}
}
