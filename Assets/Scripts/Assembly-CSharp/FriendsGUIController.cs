using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class FriendsGUIController : MonoBehaviour, IFriendsGUIController
{
	[CompilerGenerated]
	internal sealed class _003CSortFriendPreviewsAfterDelay_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsGUIController _003C_003E4__this;

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
		public _003CSortFriendPreviewsAfterDelay_003Ed__28(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this._SortFriendPreviews();
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
	internal sealed class _003C__UpdateGUI_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsGUIController _003C_003E4__this;

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
		public _003C__UpdateGUI_003Ed__30(int _003C_003E1__state)
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
				_003C_003E4__this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
				_003C_003E4__this.invitationsGrid.GetComponentsInChildren<Invitation>(true);
				_003C_003E4__this.sentInvitationsGrid.GetComponentsInChildren<Invitation>(true);
				Invitation[] componentsInChildren = _003C_003E4__this.ClanInvitationsGrid.GetComponentsInChildren<Invitation>(true);
				List<Invitation> list = new List<Invitation>();
				List<string> list2 = new List<string>();
				Invitation[] array = componentsInChildren;
				foreach (Invitation invitation in array)
				{
					bool flag = false;
					foreach (Dictionary<string, string> clanInvite in FriendsController.sharedController.ClanInvites)
					{
						Dictionary<string, string> dictionary = clanInvite;
					}
					if (!flag)
					{
						list.Add(invitation);
					}
					else if (invitation.id != null)
					{
						list2.Add(invitation.id);
					}
				}
				foreach (Invitation item in list)
				{
					item.transform.parent = null;
					UnityEngine.Object.Destroy(item.gameObject);
				}
				foreach (Dictionary<string, string> clanInvite2 in FriendsController.sharedController.ClanInvites)
				{
					if (!list2.Contains(clanInvite2["id"]))
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Invitation") as GameObject);
						gameObject.transform.parent = _003C_003E4__this.ClanInvitationsGrid.transform;
						gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
						gameObject.GetComponent<Invitation>().IsClanInv = true;
						if (clanInvite2.ContainsKey("id"))
						{
							gameObject.GetComponent<Invitation>().id = clanInvite2["id"];
							gameObject.GetComponent<Invitation>().recordId = clanInvite2["id"];
						}
						if (clanInvite2.ContainsKey("name"))
						{
							gameObject.GetComponent<Invitation>().nm.text = clanInvite2["name"];
						}
						string value;
						if (clanInvite2.TryGetValue("logo", out value) && !string.IsNullOrEmpty(value))
						{
							gameObject.GetComponent<Invitation>().clanLogoString = value;
						}
					}
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.invitationsGrid.Reposition();
				_003C_003E4__this.sentInvitationsGrid.Reposition();
				_003C_003E4__this.ClanInvitationsGrid.Reposition();
				_003C_003E4__this.timeOfLastSort = Time.realtimeSinceStartup;
				_003C_003E4__this._SortFriendPreviews();
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

	public static Action UpdaeOnlineEvent;

	public GameObject multyButton;

	public GameObject receivingPlashka;

	public UIWrapContent friendsGrid;

	public UIGrid invitationsGrid;

	public UIGrid sentInvitationsGrid;

	public UIGrid ClanInvitationsGrid;

	public LeaderboardsView leaderboardsView;

	public UIPanel friendsPanel;

	public UIPanel inboxPanel;

	public UIPanel friendProfilePanel;

	public UIPanel facebookFriensPanel;

	public UIPanel bestPlayersPanel;

	public GameObject fon;

	public GameObject newMEssage;

	public GameObject canAddLAbel;

	private float timeOfLastSort;

	public static bool ShowProfile;

	private bool invitationsInitialized;

	private float _timeLastFriendsScrollUpdate;

	private FriendProfileController _friendProfileController;

	private LeaderboardsController _leaderboardsController;

	public static void RaiseUpdaeOnlineEvent()
	{
		if (UpdaeOnlineEvent != null)
		{
			UpdaeOnlineEvent();
		}
	}

	public void HandleProfileButton()
	{
		if (!(ProfileController.Instance != null))
		{
			return;
		}
		IFriendsGUIController hidable = this;
		hidable.Hide(true);
		ProfileController.Instance.ShowInterface(delegate
		{
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
			}
			hidable.Hide(false);
		});
	}

	void IFriendsGUIController.Hide(bool h)
	{
		friendsPanel.gameObject.SetActive(!h);
		fon.SetActive(!h);
		ShowProfile = h;
	}

	public void ShowBestPlayers(bool h)
	{
		friendsPanel.gameObject.SetActive(!h);
		leaderboardsView.gameObject.SetActive(h);
	}

	public void RequestLeaderboards()
	{
		if (_leaderboardsController != null)
		{
			_leaderboardsController.RequestLeaderboards();
		}
	}

	public void MultyButtonHandler(object sender, EventArgs e)
	{
		Defs.isMulti = true;
	}

	private void Start()
	{
		StoreKitEventListener.State.Mode = "Friends";
		StoreKitEventListener.State.PurchaseKey = "In friends";
		StoreKitEventListener.State.Parameters.Clear();
		if (multyButton != null)
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				multyButton.gameObject.SetActive(false);
			}
			else
			{
				ButtonHandler component = multyButton.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += MultyButtonHandler;
				}
			}
		}
		timeOfLastSort = Time.realtimeSinceStartup;
		Defs.ProfileFromFriends = 0;
		_friendProfileController = new FriendProfileController(this);
		if (leaderboardsView != null && _leaderboardsController == null)
		{
			_leaderboardsController = leaderboardsView.gameObject.AddComponent<LeaderboardsController>();
			_leaderboardsController.LeaderboardsView = leaderboardsView;
			_leaderboardsController.FriendsGuiController = this;
			_leaderboardsController.PlayerId = Storager.getString("AccountCreated");
		}
		FriendsController.sharedController.StartRefreshingOnline();
		StartCoroutine(SortFriendPreviewsAfterDelay());
	}

	private void OnEnable()
	{
		FriendsController.FriendsUpdated += UpdateGUI;
		StartCoroutine(__UpdateGUI());
	}

	public void UpdateGUI()
	{
		StartCoroutine(__UpdateGUI());
	}

	private IEnumerator SortFriendPreviewsAfterDelay()
	{
		yield return null;
		yield return null;
		_SortFriendPreviews();
	}

	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] array = friendsGrid.GetComponentsInChildren<FriendPreview>(false);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		Array.Sort(array, (FriendPreview fp1, FriendPreview fp2) => fp1.name.CompareTo(fp2.name));
		string text = null;
		float num = 0f;
		if (array.Length != 0)
		{
			text = array[0].gameObject.name;
			Transform parent = friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					num = array[0].transform.localPosition.x - component.clipOffset.x;
				}
			}
		}
		Array.Sort(componentsInChildren, delegate(FriendPreview fp1, FriendPreview fp2)
		{
			if (fp1.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp1.id))
			{
				return 1;
			}
			if (fp2.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp2.id))
			{
				return -1;
			}
			string s = FriendsController.sharedController.onlineInfo[fp1.id]["delta"];
			string s2 = FriendsController.sharedController.onlineInfo[fp1.id]["game_mode"];
			int num3 = int.Parse(s);
			int num4 = int.Parse(s2);
			int num5 = (((float)num3 > FriendsController.onlineDelta || (num4 > 99 && (num4 - 1000000) / 100000 != (int)GameConnect.myPlatformConnect && (num4 - 1000000) / 100000 != 3)) ? 2 : ((num4 == -1) ? 1 : 0));
			string s3 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
			string s4 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
			int num6 = int.Parse(s3);
			int num7 = int.Parse(s4);
			int num8 = (((float)num6 > FriendsController.onlineDelta || (num7 > 99 && (num7 - 1000000) / 100000 != (int)GameConnect.myPlatformConnect && (num7 - 1000000) / 100000 != 3)) ? 2 : ((num7 <= -1) ? 1 : 0));
			int result;
			int result2;
			return (num5 == num8 && int.TryParse(fp1.id, out result) && int.TryParse(fp2.id, out result2)) ? (result - result2) : (num5 - num8);
		});
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.name = i.ToString("D7");
		}
		friendsGrid.SortAlphabetically();
		friendsGrid.WrapContent();
		Transform transform = null;
		if (text != null)
		{
			FriendPreview[] array2 = componentsInChildren;
			foreach (FriendPreview friendPreview in array2)
			{
				if (friendPreview.name.Equals(text))
				{
					transform = friendPreview.transform;
					break;
				}
			}
		}
		if (transform == null && componentsInChildren.Length != 0 && friendsGrid.gameObject.activeInHierarchy)
		{
			transform = componentsInChildren[0].transform;
		}
		if (transform != null)
		{
			float num2 = transform.localPosition.x - num;
			Transform parent2 = friendsGrid.transform.parent;
			if (parent2 != null)
			{
				UIPanel component2 = parent2.GetComponent<UIPanel>();
				if (component2 != null)
				{
					component2.clipOffset = new Vector2(num2, component2.clipOffset.y);
					parent2.localPosition = new Vector3(0f - num2, parent2.localPosition.y, parent2.localPosition.z);
				}
			}
		}
		friendsGrid.WrapContent();
	}

	private IEnumerator __UpdateGUI()
	{
		friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		invitationsGrid.GetComponentsInChildren<Invitation>(true);
		sentInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		Invitation[] componentsInChildren = ClanInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		List<Invitation> list = new List<Invitation>();
		List<string> list2 = new List<string>();
		Invitation[] array = componentsInChildren;
		foreach (Invitation invitation in array)
		{
			bool flag = false;
			foreach (Dictionary<string, string> clanInvite in FriendsController.sharedController.ClanInvites)
			{
				Dictionary<string, string> dictionary = clanInvite;
			}
			if (!flag)
			{
				list.Add(invitation);
			}
			else if (invitation.id != null)
			{
				list2.Add(invitation.id);
			}
		}
		foreach (Invitation item in list)
		{
			item.transform.parent = null;
			UnityEngine.Object.Destroy(item.gameObject);
		}
		foreach (Dictionary<string, string> clanInvite2 in FriendsController.sharedController.ClanInvites)
		{
			if (!list2.Contains(clanInvite2["id"]))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Invitation") as GameObject);
				gameObject.transform.parent = ClanInvitationsGrid.transform;
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponent<Invitation>().IsClanInv = true;
				if (clanInvite2.ContainsKey("id"))
				{
					gameObject.GetComponent<Invitation>().id = clanInvite2["id"];
					gameObject.GetComponent<Invitation>().recordId = clanInvite2["id"];
				}
				if (clanInvite2.ContainsKey("name"))
				{
					gameObject.GetComponent<Invitation>().nm.text = clanInvite2["name"];
				}
				string value;
				if (clanInvite2.TryGetValue("logo", out value) && !string.IsNullOrEmpty(value))
				{
					gameObject.GetComponent<Invitation>().clanLogoString = value;
				}
			}
		}
		yield return null;
		invitationsGrid.Reposition();
		sentInvitationsGrid.Reposition();
		ClanInvitationsGrid.Reposition();
		timeOfLastSort = Time.realtimeSinceStartup;
		_SortFriendPreviews();
	}

	private void Update()
	{
		if (receivingPlashka != null && FriendsController.sharedController != null)
		{
			if ((friendsPanel != null && friendsPanel.gameObject.activeInHierarchy) || (inboxPanel != null && inboxPanel.gameObject.activeInHierarchy))
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOfFriendsRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (_friendProfileController != null && _friendProfileController.FriendProfileGo != null && _friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOffFullInfoRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (leaderboardsView != null && leaderboardsView.gameObject.activeInHierarchy)
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOfBestPlayersRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else
			{
				receivingPlashka.SetActive(false);
			}
		}
		friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = friendsGrid.transform.childCount > 4;
		if (friendsGrid.transform.childCount > 0 && friendsGrid.transform.childCount <= 4 && Time.realtimeSinceStartup - _timeLastFriendsScrollUpdate > 0.5f)
		{
			_timeLastFriendsScrollUpdate = Time.realtimeSinceStartup;
			float num = 0f;
			foreach (Transform item in friendsGrid.transform)
			{
				num += item.localPosition.x;
			}
			num /= (float)friendsGrid.transform.childCount;
			Transform parent = friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					component.clipOffset = new Vector2(num, component.clipOffset.y);
					parent.localPosition = new Vector3(0f - num, parent.localPosition.y, parent.localPosition.z);
				}
			}
		}
		if (Time.realtimeSinceStartup - timeOfLastSort > 10f)
		{
			if (UpdaeOnlineEvent != null)
			{
				UpdaeOnlineEvent();
			}
			timeOfLastSort = Time.realtimeSinceStartup;
			_SortFriendPreviews();
		}
		newMEssage.SetActive(FriendsController.sharedController.invitesToUs.Count > 0 || FriendsController.sharedController.ClanInvites.Count > 0);
		canAddLAbel.SetActive(FriendsController.sharedController.friends.Count == 0);
	}

	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= UpdateGUI;
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		_friendProfileController.Dispose();
		ShowProfile = false;
	}
}
