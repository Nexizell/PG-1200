using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class LeaderboardsView : MonoBehaviour
{
	public enum State
	{
		None = 0,
		Clans = 1,
		Friends = 2,
		BestPlayers = 3,
		Tournament = 4,
		Crafters = 5,
		Default = 3
	}

	[CompilerGenerated]
	internal sealed class _003CSkipFrameAndExecuteCoroutine_003Ed__59 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Action a;

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
		public _003CSkipFrameAndExecuteCoroutine_003Ed__59(int _003C_003E1__state)
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
				if (a == null)
				{
					return false;
				}
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				a();
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

	[CompilerGenerated]
	internal sealed class _003CUpdateGridsAndScrollers_003Ed__61 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardsView _003C_003E4__this;

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
		public _003CUpdateGridsAndScrollers_003Ed__61(int _003C_003E1__state)
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
				_003C_003E4__this._prepared = false;
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				UIWrapContent[] array2 = new UIWrapContent[4] { _003C_003E4__this.friendsGrid, _003C_003E4__this.clansGrid, _003C_003E4__this.tournamentGrid, _003C_003E4__this.craftersGrid };
				foreach (UIWrapContent uIWrapContent in array2)
				{
					if (!(uIWrapContent == null))
					{
						uIWrapContent.SortBasedOnScrollMovement();
						uIWrapContent.WrapContent();
					}
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				UIScrollView[] array = new UIScrollView[4] { _003C_003E4__this.clansScroll, _003C_003E4__this.friendsScroll, _003C_003E4__this.LeagueScroll, _003C_003E4__this.craftersScroll };
				foreach (UIScrollView uIScrollView in array)
				{
					if (!(uIScrollView == null))
					{
						uIScrollView.ResetPosition();
						uIScrollView.UpdatePosition();
					}
				}
				_003C_003E4__this._prepared = true;
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
	internal sealed class _003CStart_003Ed__71 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaderboardsView _003C_003E4__this;

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
		public _003CStart_003Ed__71(int _003C_003E1__state)
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
				UIButton[] array = new UIButton[5] { _003C_003E4__this.clansButton, _003C_003E4__this.friendsButton, _003C_003E4__this.bestPlayersButton, _003C_003E4__this.tournamentButton, _003C_003E4__this.craftersButton };
				foreach (UIButton uIButton in array)
				{
					if (!(uIButton == null))
					{
						ButtonHandler component = uIButton.GetComponent<ButtonHandler>();
						if (component != null)
						{
							component.Clicked += _003C_003E4__this.HandleTabPressed;
						}
					}
				}
				UIScrollView[] array2 = new UIScrollView[5] { _003C_003E4__this.clansScroll, _003C_003E4__this.friendsScroll, _003C_003E4__this.bestPlayersScroll, _003C_003E4__this.LeagueScroll, _003C_003E4__this.craftersScroll };
				foreach (UIScrollView uIScrollView in array2)
				{
					if (!(uIScrollView == null))
					{
						uIScrollView.ResetPosition();
					}
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.friendsGrid.Do(delegate(UIWrapContent w)
				{
					w.SortBasedOnScrollMovement();
					w.WrapContent();
				});
				_003C_003E4__this.bestPlayersGrid.Do(delegate(UIWrapContent w)
				{
					w.SortBasedOnScrollMovement();
					w.WrapContent();
				});
				_003C_003E4__this.clansGrid.Do(delegate(UIWrapContent w)
				{
					w.SortBasedOnScrollMovement();
					w.WrapContent();
				});
				_003C_003E4__this.tournamentGrid.Do(delegate(UIWrapContent w)
				{
					w.SortBasedOnScrollMovement();
					w.WrapContent();
				});
				_003C_003E4__this.craftersGrid.Do(delegate(UIWrapContent w)
				{
					w.SortBasedOnScrollMovement();
					w.WrapContent();
				});
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				int @int = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
				State state = (Enum.IsDefined(typeof(State), @int) ? ((State)@int) : State.BestPlayers);
				_003C_003E4__this.CurrentState = ((state != 0) ? state : State.BestPlayers);
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

	[SerializeField]
	public UIWrapContent clansGrid;

	[SerializeField]
	public UIWrapContent friendsGrid;

	[SerializeField]
	public UIWrapContent bestPlayersGrid;

	[SerializeField]
	public UIWrapContent tournamentGrid;

	[SerializeField]
	public UIWrapContent craftersGrid;

	[SerializeField]
	public ButtonHandler backButton;

	[SerializeField]
	protected internal UIButton clansButton;

	[SerializeField]
	protected internal UIButton friendsButton;

	[SerializeField]
	protected internal UIButton bestPlayersButton;

	[SerializeField]
	protected internal UIButton tournamentButton;

	[SerializeField]
	protected internal UIButton craftersButton;

	[SerializeField]
	protected internal UIScrollView clansScroll;

	[SerializeField]
	protected internal UIScrollView friendsScroll;

	[SerializeField]
	protected internal UIScrollView bestPlayersScroll;

	[SerializeField]
	protected internal UIScrollView tournamentScroll;

	[SerializeField]
	protected internal UIScrollView craftersScroll;

	[SerializeField]
	protected internal UIScrollView LeagueScroll;

	[SerializeField]
	protected internal GameObject friendsTableHeader;

	[SerializeField]
	protected internal GameObject bestPlayersTableHeader;

	[SerializeField]
	protected internal GameObject clansTableHeader;

	[SerializeField]
	protected internal GameObject tournamentTableHeader;

	[SerializeField]
	protected internal GameObject craftersTableHeader;

	[SerializeField]
	public GameObject leaderboardHeader;

	[SerializeField]
	public GameObject leaderboardFooter;

	[SerializeField]
	public GameObject tournamentHeader;

	[SerializeField]
	public GameObject tournamentFooter;

	[SerializeField]
	public GameObject tournamentTableFooter;

	[SerializeField]
	public GameObject clansTableFooter;

	[SerializeField]
	public GameObject clansFooter;

	[SerializeField]
	public GameObject tournamentJoinInfo;

	[SerializeField]
	protected internal UILabel tournamentAward;

	[SerializeField]
	public UILabel expirationLabel;

	[SerializeField]
	public GameObject expirationIconObj;

	[SerializeField]
	public GameObject touchBlocker;

	private IDisposable _backSubscription;

	public const float FarAwayX = 9000f;

	private bool _overallTopFooterActive;

	private bool _leagueTopFooterActive;

	private readonly Rilisoft.Lazy<UIPanel> _leaderboardsPanel;

	private bool _prepared;

	private Dictionary<GameObject, int> _scrollsDefHeights = new Dictionary<GameObject, int>();

	private State _currentState;

	internal const string LeaderboardsTabCache = "Leaderboards.TabCache";

	public bool InTournamentTop { get; set; }

	public bool CanShowClanTableFooter { get; set; }

	private bool IsTournamentMember
	{
		get
		{
			if (RatingSystem.instance != null)
			{
				return RatingSystem.instance.currentLeague >= RatingSystem.RatingLeague.Adamant;
			}
			return false;
		}
	}

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			PlayerPrefs.SetInt("Leaderboards.TabCache", (int)value);
			if (clansButton != null)
			{
				clansButton.isEnabled = value != State.Clans;
				Transform transform = clansButton.gameObject.transform.Find("SpriteLabel");
				if (transform != null)
				{
					transform.gameObject.SetActive(value != State.Clans);
				}
				Transform transform2 = clansButton.gameObject.transform.Find("ChekmarkLabel");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value == State.Clans);
				}
				clansFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == State.Clans);
				});
				clansTableFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == State.Clans && CanShowClanTableFooter);
				});
			}
			if (friendsButton != null)
			{
				friendsButton.isEnabled = value != State.Friends;
				Transform transform3 = friendsButton.gameObject.transform.Find("SpriteLabel");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value != State.Friends);
				}
				Transform transform4 = friendsButton.gameObject.transform.Find("ChekmarkLabel");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(value == State.Friends);
				}
			}
			if (bestPlayersButton != null)
			{
				bestPlayersButton.isEnabled = value != State.BestPlayers;
				Transform transform5 = bestPlayersButton.gameObject.transform.Find("SpriteLabel");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(value != State.BestPlayers);
				}
				Transform transform6 = bestPlayersButton.gameObject.transform.Find("ChekmarkLabel");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(value == State.BestPlayers);
				}
				leaderboardFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == State.BestPlayers);
				});
				leaderboardHeader.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == State.BestPlayers);
				});
			}
			if (tournamentButton != null)
			{
				tournamentButton.isEnabled = value != State.Tournament;
				Transform transform7 = tournamentButton.gameObject.transform.Find("SpriteLabel");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(value != State.Tournament);
				}
				Transform transform8 = tournamentButton.gameObject.transform.Find("ChekmarkLabel");
				if (transform8 != null)
				{
					transform8.gameObject.SetActive(value == State.Tournament);
				}
			}
			if (craftersButton != null)
			{
				craftersButton.isEnabled = value != State.Crafters;
				Transform transform9 = craftersButton.gameObject.transform.Find("SpriteLabel");
				if (transform9 != null)
				{
					transform9.gameObject.SetActive(value != State.Crafters);
				}
				Transform transform10 = craftersButton.gameObject.transform.Find("ChekmarkLabel");
				if (transform10 != null)
				{
					transform10.gameObject.SetActive(value == State.Crafters);
				}
			}
			if (clansTableHeader != null)
			{
				bool active = value == State.Clans;
				clansTableHeader.SetActive(active);
			}
			bestPlayersGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition5 = g.transform.localPosition;
				localPosition5.x = ((value == State.BestPlayers) ? 0f : 9000f);
				g.gameObject.transform.localPosition = localPosition5;
			});
			craftersGrid.Do(delegate
			{
				Vector3 localPosition4 = craftersGrid.transform.localPosition;
				localPosition4.x = ((value == State.Crafters) ? 0f : 9000f);
				craftersGrid.gameObject.transform.localPosition = localPosition4;
			});
			friendsGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition3 = g.transform.localPosition;
				localPosition3.x = ((value == State.Friends) ? 0f : 9000f);
				g.gameObject.transform.localPosition = localPosition3;
			});
			clansGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition2 = g.transform.localPosition;
				localPosition2.x = ((value == State.Clans) ? 0f : 9000f);
				g.gameObject.transform.localPosition = localPosition2;
			});
			tournamentJoinInfo.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(value == State.Tournament && !IsTournamentMember);
			});
			bool canShowTournamentGrid = value == State.Tournament && IsTournamentMember;
			tournamentFooter.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid);
			});
			tournamentHeader.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid);
			});
			tournamentTableFooter.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid && InTournamentTop);
			});
			tournamentGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition = g.transform.localPosition;
				localPosition.x = (canShowTournamentGrid ? 0f : 9000f);
				g.gameObject.transform.localPosition = localPosition;
			});
			UpdateScrollSize(tournamentGrid.gameObject, tournamentTableFooter);
			UpdateScrollSize(clansGrid.gameObject, clansTableFooter);
			bestPlayersTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == State.BestPlayers);
			});
			clansTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == State.Clans);
			});
			friendsTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == State.Friends);
			});
			tournamentTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == State.Tournament && !tournamentJoinInfo.activeInHierarchy);
			});
			if (craftersTableHeader != null)
			{
				craftersTableHeader.SetActive(value == State.Crafters);
			}
			State currentState = _currentState;
			_currentState = value;
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged(currentState, _currentState);
			}
		}
	}

	internal bool Prepared
	{
		get
		{
			return _prepared;
		}
	}

	public event Action<State, State> OnStateChanged;

	public LeaderboardsView()
	{
		_leaderboardsPanel = new Rilisoft.Lazy<UIPanel>(base.GetComponent<UIPanel>);
	}

	public void SetOverallTopFooterActive()
	{
		_overallTopFooterActive = true;
	}

	public void SetLeagueTopFooterActive()
	{
		_leagueTopFooterActive = true;
	}

	private void RefreshGrid(UIGrid grid)
	{
		grid.Reposition();
	}

	private IEnumerator SkipFrameAndExecuteCoroutine(Action a)
	{
		if (a != null)
		{
			yield return new WaitForEndOfFrame();
			a();
		}
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (clansButton != null && gameObject == clansButton.gameObject)
		{
			CurrentState = State.Clans;
		}
		else if (friendsButton != null && gameObject == friendsButton.gameObject)
		{
			CurrentState = State.Friends;
		}
		else if (bestPlayersButton != null && gameObject == bestPlayersButton.gameObject)
		{
			CurrentState = State.BestPlayers;
		}
		else if (tournamentButton != null && gameObject == tournamentButton.gameObject)
		{
			CurrentState = State.Tournament;
		}
		else if (craftersButton != null && gameObject == craftersButton.gameObject)
		{
			CurrentState = State.Crafters;
		}
	}

	private IEnumerator UpdateGridsAndScrollers()
	{
		_prepared = false;
		yield return new WaitForEndOfFrame();
		UIWrapContent[] array = new UIWrapContent[4] { friendsGrid, clansGrid, tournamentGrid, craftersGrid };
		foreach (UIWrapContent uIWrapContent in array)
		{
			if (!(uIWrapContent == null))
			{
				uIWrapContent.SortBasedOnScrollMovement();
				uIWrapContent.WrapContent();
			}
		}
		yield return null;
		UIScrollView[] array2 = new UIScrollView[4] { clansScroll, friendsScroll, LeagueScroll, craftersScroll };
		foreach (UIScrollView uIScrollView in array2)
		{
			if (!(uIScrollView == null))
			{
				uIScrollView.ResetPosition();
				uIScrollView.UpdatePosition();
			}
		}
		_prepared = true;
	}

	private void OnEnable()
	{
		_backSubscription = BackSystem.Instance.Register(delegate
		{
			LeaderboardScript.Instance.ReturnBack(this, null);
		}, "Leaderboards");
		StartCoroutine(UpdateGridsAndScrollers());
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_prepared = false;
	}

	private void UpdateScrollSize(GameObject scrollChildObj, GameObject widgetObject)
	{
		UIPanel componentInParent = scrollChildObj.GetComponentInParent<UIPanel>();
		if (widgetObject == null)
		{
			return;
		}
		UIWidget component = widgetObject.GetComponent<UIWidget>();
		if (!(componentInParent == null) && !(component == null))
		{
			int num = (_scrollsDefHeights.Keys.Contains(scrollChildObj) ? _scrollsDefHeights[scrollChildObj] : (-1));
			if (num >= 0)
			{
				componentInParent.bottomAnchor.absolute = (widgetObject.activeInHierarchy ? (num + component.height) : num);
			}
		}
	}

	private void Awake()
	{
		UIWrapContent[] array = new UIWrapContent[5] { friendsGrid, bestPlayersGrid, clansGrid, tournamentGrid, craftersGrid };
		foreach (UIWrapContent uIWrapContent in array)
		{
			if (!(uIWrapContent == null))
			{
				uIWrapContent.gameObject.SetActive(true);
				Vector3 localPosition = uIWrapContent.transform.localPosition;
				localPosition.x = 9000f;
				uIWrapContent.gameObject.transform.localPosition = localPosition;
			}
		}
		UIPanel componentInParent = clansGrid.gameObject.GetComponentInParent<UIPanel>();
		_scrollsDefHeights.Add(clansGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = friendsGrid.gameObject.GetComponentInParent<UIPanel>();
		_scrollsDefHeights.Add(friendsGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = bestPlayersGrid.gameObject.GetComponentInParent<UIPanel>();
		_scrollsDefHeights.Add(bestPlayersGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = tournamentGrid.gameObject.GetComponentInParent<UIPanel>();
		_scrollsDefHeights.Add(tournamentGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = craftersGrid.gameObject.GetComponentInParent<UIPanel>();
		_scrollsDefHeights.Add(craftersGrid.gameObject, componentInParent.bottomAnchor.absolute);
		OnLocalizeChanged();
		LocalizationStore.AddEventCallAfterLocalize(OnLocalizeChanged);
	}

	private IEnumerator Start()
	{
		UIButton[] array = new UIButton[5] { clansButton, friendsButton, bestPlayersButton, tournamentButton, craftersButton };
		foreach (UIButton uIButton in array)
		{
			if (!(uIButton == null))
			{
				ButtonHandler component = uIButton.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += HandleTabPressed;
				}
			}
		}
		UIScrollView[] array2 = new UIScrollView[5] { clansScroll, friendsScroll, bestPlayersScroll, LeagueScroll, craftersScroll };
		foreach (UIScrollView uIScrollView in array2)
		{
			if (!(uIScrollView == null))
			{
				uIScrollView.ResetPosition();
			}
		}
		yield return null;
		friendsGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		bestPlayersGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		clansGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		tournamentGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		craftersGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		yield return null;
		int @int = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
		State state = (Enum.IsDefined(typeof(State), @int) ? ((State)@int) : State.BestPlayers);
		CurrentState = ((state != 0) ? state : State.BestPlayers);
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(OnLocalizeChanged);
	}

	private void OnLocalizeChanged()
	{
		tournamentAward.text = string.Format(LocalizationStore.Get("Key_1531"), new object[1] { BalanceController.competitionAward.Price });
	}
}
