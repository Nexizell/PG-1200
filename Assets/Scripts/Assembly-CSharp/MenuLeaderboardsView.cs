using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class MenuLeaderboardsView : MonoBehaviour
{
	public enum State
	{
		Friends = 0,
		BestPlayers = 1,
		Clans = 2
	}

	[CompilerGenerated]
	internal sealed class _003CSetGrid_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UISprite temporaryBackground;

		public UIGrid grid;

		public IList<LeaderboardItemViewModel> value;

		private UIScrollView _003CscrollView_003E5__1;

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
		public _003CSetGrid_003Ed__42(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
			case 2:
			case 3:
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
				break;
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
					_003C_003E1__state = -1;
					temporaryBackground.gameObject.SetActive(true);
					_003C_003E1__state = -3;
					if (!(grid == null))
					{
						goto IL_0076;
					}
					result = false;
					_003C_003Em__Finally1();
					goto end_IL_0000;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -3;
					_003CscrollView_003E5__1.ResetPosition();
					_003CscrollView_003E5__1.UpdatePosition();
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					result = true;
					goto end_IL_0000;
				case 3:
					{
						_003C_003E1__state = -3;
						_003CscrollView_003E5__1.enabled = value.Count >= 10;
						break;
					}
					IL_0076:
					if (!grid.gameObject.activeInHierarchy)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						IEnumerable<LeaderboardItemViewModel> enumerable;
						if (value != null)
						{
							enumerable = value.Where((LeaderboardItemViewModel it) => it != null);
						}
						else
						{
							IEnumerable<LeaderboardItemViewModel> enumerable2 = new List<LeaderboardItemViewModel>();
							enumerable = enumerable2;
						}
						IEnumerable<LeaderboardItemViewModel> enumerable3 = enumerable;
						List<Transform> childList = grid.GetChildList();
						for (int i = 0; i != childList.Count; i++)
						{
							UnityEngine.Object.Destroy(childList[i].gameObject);
						}
						childList.Clear();
						grid.Reposition();
						foreach (LeaderboardItemViewModel item in enumerable3)
						{
							GameObject gameObject = (item.Highlight ? (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardSelectedItem")) as GameObject) : (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardItem")) as GameObject));
							if (gameObject != null)
							{
								LeaderboardItemView component = gameObject.GetComponent<LeaderboardItemView>();
								if (component != null)
								{
									component.Reset(item);
									gameObject.transform.parent = grid.transform;
									grid.AddChild(gameObject.transform);
									gameObject.transform.localScale = Vector3.one;
								}
							}
						}
						grid.Reposition();
						_003CscrollView_003E5__1 = grid.transform.parent.gameObject.GetComponent<UIScrollView>();
						if (!(_003CscrollView_003E5__1 != null))
						{
							break;
						}
						_003CscrollView_003E5__1.enabled = true;
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						result = true;
					}
					goto end_IL_0000;
				}
				_003CscrollView_003E5__1 = null;
				_003C_003Em__Finally1();
				result = false;
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			temporaryBackground.gameObject.SetActive(false);
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CUpdateGridsAndScrollers_003Ed__43 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MenuLeaderboardsView _003C_003E4__this;

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
		public _003CUpdateGridsAndScrollers_003Ed__43(int _003C_003E1__state)
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
				foreach (UIGrid item in new UIGrid[3] { _003C_003E4__this.friendsGrid, _003C_003E4__this.bestPlayersGrid, _003C_003E4__this.clansGrid }.Where((UIGrid g) => g != null))
				{
					item.Reposition();
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				foreach (UIScrollView item2 in new UIScrollView[3] { _003C_003E4__this.friendsScroll, _003C_003E4__this.bestPlayersScroll, _003C_003E4__this.clansScroll }.Where((UIScrollView s) => s != null))
				{
					item2.ResetPosition();
					item2.UpdatePosition();
				}
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

	public UIGrid friendsGrid;

	public UIGrid bestPlayersGrid;

	public UIGrid clansGrid;

	public UIButton friendsButton;

	public UIButton bestPlayersButton;

	public UIButton clansButton;

	public UIDragScrollView friendsPanel;

	public UIDragScrollView bestPlayersPanel;

	public UIDragScrollView clansPanel;

	public UIScrollView friendsScroll;

	public UIScrollView bestPlayersScroll;

	public UIScrollView clansScroll;

	public LeaderboardItemView footer;

	public LeaderboardItemView clanFooter;

	public UISprite temporaryBackground;

	public UISprite bestPlayersDefaultSprite;

	public UISprite clansDefaultSprite;

	public UILabel nickOrClanName;

	public ToggleButton btnLeaderboards;

	public GameObject opened;

	private State _currentState;

	private Vector3 _desiredPosition = Vector3.zero;

	private Vector3 _outOfScreenPosition = Vector3.zero;

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			friendsButton.isEnabled = value != State.Friends;
			Transform transform = friendsButton.transform.FindChild("IdleLabel");
			Transform transform2 = friendsButton.transform.FindChild("ActiveLabel");
			if (transform != null && (bool)transform2)
			{
				transform.gameObject.SetActive(value != State.Friends);
				transform2.gameObject.SetActive(value == State.Friends);
			}
			bestPlayersButton.isEnabled = value != State.BestPlayers;
			Transform transform3 = bestPlayersButton.transform.FindChild("IdleLabel");
			Transform transform4 = bestPlayersButton.transform.FindChild("ActiveLabel");
			if (transform3 != null && (bool)transform4)
			{
				transform3.gameObject.SetActive(value != State.BestPlayers);
				transform4.gameObject.SetActive(value == State.BestPlayers);
			}
			clansButton.isEnabled = value != State.Clans;
			Transform transform5 = clansButton.transform.FindChild("IdleLabel");
			Transform transform6 = clansButton.transform.FindChild("ActiveLabel");
			if (transform5 != null && (bool)transform6)
			{
				transform5.gameObject.SetActive(value != State.Clans);
				transform6.gameObject.SetActive(value == State.Clans);
			}
			if (nickOrClanName != null)
			{
				nickOrClanName.text = ((value == State.Clans) ? LocalizationStore.Get("Key_0257") : LocalizationStore.Get("Key_0071"));
			}
			friendsPanel.transform.localPosition = ((value == State.Friends) ? _desiredPosition : _outOfScreenPosition);
			bestPlayersPanel.transform.localPosition = ((value == State.BestPlayers) ? _desiredPosition : _outOfScreenPosition);
			clansPanel.transform.localPosition = ((value == State.Clans) ? _desiredPosition : _outOfScreenPosition);
			_currentState = value;
		}
	}

	public static bool IsNeedShow
	{
		get
		{
			bool hasFriends = FriendsController.HasFriends;
			return false;
		}
	}

	public static int PageSize
	{
		get
		{
			return 9;
		}
	}

	public IList<LeaderboardItemViewModel> FriendsList
	{
		set
		{
			StartCoroutine(SetGrid(friendsGrid, value, temporaryBackground));
		}
	}

	public IList<LeaderboardItemViewModel> BestPlayersList
	{
		set
		{
			StartCoroutine(SetGrid(bestPlayersGrid, value, temporaryBackground));
			if (bestPlayersDefaultSprite != null)
			{
				bestPlayersDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(bestPlayersDefaultSprite);
				bestPlayersDefaultSprite = null;
			}
		}
	}

	public IList<LeaderboardItemViewModel> ClansList
	{
		set
		{
			StartCoroutine(SetGrid(clansGrid, value, temporaryBackground));
			if (clansDefaultSprite != null)
			{
				clansDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(clansDefaultSprite);
				clansDefaultSprite = null;
			}
		}
	}

	public LeaderboardItemViewModel SelfStats
	{
		set
		{
			footer.Reset(value);
			footer.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	public LeaderboardItemViewModel SelfClanStats
	{
		set
		{
			clanFooter.Reset(value);
			clanFooter.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	private void OnEnable()
	{
		StartCoroutine(UpdateGridsAndScrollers());
	}

	private void Awake()
	{
		footer.gameObject.SetActive(false);
		clanFooter.gameObject.SetActive(false);
		if (bestPlayersDefaultSprite != null)
		{
			bestPlayersDefaultSprite.gameObject.SetActive(true);
		}
		if (clansDefaultSprite != null)
		{
			clansDefaultSprite.gameObject.SetActive(true);
		}
		temporaryBackground.gameObject.SetActive(false);
	}

	private void Start()
	{
		_desiredPosition = friendsPanel.transform.localPosition;
		_outOfScreenPosition = new Vector3(9000f, _desiredPosition.y, _desiredPosition.z);
		foreach (UIButton item in new UIButton[3] { friendsButton, bestPlayersButton, clansButton }.Where((UIButton b) => b != null))
		{
			ButtonHandler component = item.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleTabPressed;
			}
		}
		foreach (UIScrollView item2 in new UIScrollView[3] { friendsScroll, bestPlayersScroll, clansScroll }.Where((UIScrollView s) => s != null))
		{
			item2.ResetPosition();
		}
		CurrentState = State.BestPlayers;
		bool isNeedShow = IsNeedShow;
		Show(isNeedShow, false);
		btnLeaderboards.IsChecked = false;
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (gameObject == friendsButton.gameObject)
		{
			CurrentState = State.Friends;
		}
		else if (gameObject == bestPlayersButton.gameObject)
		{
			CurrentState = State.BestPlayers;
		}
		else if (gameObject == clansButton.gameObject)
		{
			CurrentState = State.Clans;
		}
	}

	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, UISprite temporaryBackground)
	{
		temporaryBackground.gameObject.SetActive(true);
		try
		{
			if (grid == null)
			{
				yield break;
			}
			while (!grid.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			IEnumerable<LeaderboardItemViewModel> enumerable;
			if (value != null)
			{
				enumerable = value.Where((LeaderboardItemViewModel it) => it != null);
			}
			else
			{
				IEnumerable<LeaderboardItemViewModel> enumerable2 = new List<LeaderboardItemViewModel>();
				enumerable = enumerable2;
			}
			IEnumerable<LeaderboardItemViewModel> enumerable3 = enumerable;
			List<Transform> childList = grid.GetChildList();
			for (int i = 0; i != childList.Count; i++)
			{
				UnityEngine.Object.Destroy(childList[i].gameObject);
			}
			childList.Clear();
			grid.Reposition();
			foreach (LeaderboardItemViewModel item in enumerable3)
			{
				GameObject gameObject = (item.Highlight ? (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardSelectedItem")) as GameObject) : (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardItem")) as GameObject));
				if (gameObject != null)
				{
					LeaderboardItemView component = gameObject.GetComponent<LeaderboardItemView>();
					if (component != null)
					{
						component.Reset(item);
						gameObject.transform.parent = grid.transform;
						grid.AddChild(gameObject.transform);
						gameObject.transform.localScale = Vector3.one;
					}
				}
			}
			grid.Reposition();
			UIScrollView scrollView = grid.transform.parent.gameObject.GetComponent<UIScrollView>();
			if (scrollView != null)
			{
				scrollView.enabled = true;
				yield return null;
				scrollView.ResetPosition();
				scrollView.UpdatePosition();
				yield return null;
				scrollView.enabled = value.Count >= 10;
			}
		}
		finally
		{
			temporaryBackground.gameObject.SetActive(false);
		}
	}

	private IEnumerator UpdateGridsAndScrollers()
	{
		foreach (UIGrid item in new UIGrid[3] { friendsGrid, bestPlayersGrid, clansGrid }.Where((UIGrid g) => g != null))
		{
			item.Reposition();
		}
		yield return null;
		foreach (UIScrollView item2 in new UIScrollView[3] { friendsScroll, bestPlayersScroll, clansScroll }.Where((UIScrollView s) => s != null))
		{
			item2.ResetPosition();
			item2.UpdatePosition();
		}
	}

	public void Show(bool needShow, bool animate)
	{
	}
}
