using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class FriendProfileView : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CLoadLobbyItems_003Ed__154 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendProfileView _003C_003E4__this;

		private List<string>.Enumerator _003C_003E7__wrap1;

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
		public _003CLoadLobbyItems_003Ed__154(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E4__this.preloadedLobbyItemsForSeeLobby.Clear();
					_003C_003E7__wrap1 = _003C_003E4__this.lobby.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>("LobbyItemsContent/" + current);
					if (resourceRequest != null)
					{
						_003C_003E4__this.preloadedLobbyItemsForSeeLobby.Add(resourceRequest);
						_003C_003E2__current = resourceRequest;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<string>.Enumerator);
				try
				{
					if (_003C_003E4__this.panelSeeLobby != null && _003C_003E4__this.panelSeeLobby.activeInHierarchy)
					{
						LobbyCraftController.Instance.SetSceneViews(_003C_003E4__this.lobby, false);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in LoadLobbyItems: {0}", ex);
				}
				return false;
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
			((IDisposable)_003C_003E7__wrap1).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CRequestUpdate_003Ed__164 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendProfileView _003C_003E4__this;

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
		public _003CRequestUpdate_003Ed__164(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
			}
			if (_003C_003E4__this.UpdateRequested != null)
			{
				_003C_003E4__this.UpdateRequested();
			}
			_003C_003E2__current = new WaitForSeconds(5f);
			_003C_003E1__state = 1;
			return true;
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

	public UILabel[] playerNameSeeLobby;

	public GameObject likeContainer;

	public GameObject dislikeContainer;

	public UIButton likeLobby;

	public UIButton dislikeLobby;

	public UILabel likesCount;

	public Transform pers;

	public GameObject[] bootsPoint;

	private CharacterInterface characterInterface;

	public GameObject background;

	public GameObject allInterfacePanel;

	public GameObject panelSeeLobby;

	public UIButton seeLobby;

	public UISprite rankSprite;

	public UILabel friendCountLabel;

	public UILabel friendLocationLabel;

	public UILabel friendGameModeLabel;

	public UILabel friendNameLabel;

	public UILabel survivalScoreLabel;

	public UILabel winCountLabel;

	public UILabel totalWinCountLabel;

	public UILabel clanName;

	public UILabel friendIdLabel;

	public UILabel[] titlesLabel;

	public UITexture clanLogo;

	[Header("Online state settings")]
	public UILabel inFriendStateLabel;

	[Header("Online state settings")]
	public UILabel offlineStateLabel;

	[Header("Online state settings")]
	public UILabel playingStateLabel;

	public UISprite inFriendState;

	public UISprite offlineState;

	public UISprite playingState;

	public GameObject playingStateInfoContainer;

	[Header("Buttons settings")]
	public UIButton backButton;

	public UIButton joinButton;

	public UIButton sendMyIdButton;

	public UIButton chatButton;

	public UIButton inviteToClanButton;

	public UIButton addFriendButton;

	public UIButton removeFriendButton;

	public UITable buttonAlignContainer;

	public UILabel addOrRemoveButtonLabel;

	public UISprite notConnectJoinButtonSprite;

	public UISprite addFrienButtonSentState;

	public UISprite addClanButtonSentState;

	private List<string> lobby = new List<string>();

	private List<object> preloadedLobbyItemsForSeeLobby = new List<object>();

	private IDisposable _backSubscription;

	private bool _escapePressed;

	private float lastTime;

	private float idleTimerLastTime;

	private readonly Lazy<Rect> _touchZone = new Lazy<Rect>(() => new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height));

	private static Vector3 initialRot = new Vector3(0f, -180f, 0f);

	private const string DefaultStatisticString = "-";

	private Vector3? initialPersPosProfile;

	private Vector3? initialPersPosLobby;

	private Vector3? initialScaleProfile;

	private Vector3? initialScaleLobby;

	private IDisposable m_backSubscriptionSeeLobby;

	private bool lobbyCameraEnabledOnEnter;

	private string layerBeforeEnter;

	public int LikesNumFromServer { get; set; }

	public int LikesNumAdding { get; set; }

	public GameObject characterModel
	{
		get
		{
			return characterInterface.gameObject;
		}
	}

	public bool IsCanConnectToFriend { get; set; }

	public string FriendLocation { get; set; }

	public int FriendCount { get; set; }

	public string FriendName { get; set; }

	public OnlineState Online { get; set; }

	public int Rank { get; set; }

	public int SurvivalScore { get; set; }

	public string Username { get; set; }

	public int WinCount { get; set; }

	public int TotalWinCount { get; set; }

	public string FriendGameMode { get; set; }

	public string FriendId { get; set; }

	public string NotConnectCondition { get; set; }

	public event Action BackButtonClickEvent;

	public event Action JoinButtonClickEvent;

	public event Action CopyMyIdButtonClickEvent;

	public event Action ChatButtonClickEvent;

	public event Action AddButtonClickEvent;

	public event Action RemoveButtonClickEvent;

	public event Action InviteToClanButtonClickEvent;

	public event Action UpdateRequested;

	public void SetLobby(List<string> newLobby)
	{
		if (SceneManagerHelper.ActiveSceneName != Defs.MainMenuScene || newLobby == null || newLobby.Count == 0)
		{
			return;
		}
		try
		{
			List<string> first = newLobby.OrderBy((string x) => x).ToList();
			if (!first.SequenceEqual(lobby))
			{
				lobby = first;
				CoroutineRunner.Instance.StartCoroutine(LoadLobbyItems());
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in SetLobby: {0}", ex);
		}
	}

	public void HandleLikeClicked()
	{
		if (!(FriendsController.sharedController == null))
		{
			if (FriendId.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogErrorFormat("HandleLikeClicked friend id is null or empty");
			}
			else
			{
				FriendsController.sharedController.SendLikeLobby(FriendId, true);
			}
		}
	}

	public void HandleDislikeClicked()
	{
		if (!(FriendsController.sharedController == null))
		{
			if (FriendId.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogErrorFormat("HandleDislikeClicked friend id is null or empty");
			}
			else
			{
				FriendsController.sharedController.SendLikeLobby(FriendId, false);
			}
		}
	}

	public void HandleSeeLobby()
	{
		SetSeeLobbyEnabled(true);
	}

	public void HandleBackSeeLobby()
	{
		SetSeeLobbyEnabled(false);
	}

	private void SetSeeLobbyEnabled(bool enabled)
	{
		if (background == null || SceneManagerHelper.ActiveSceneName != Defs.MainMenuScene)
		{
			return;
		}
		UnsubscribeFromBackSystemSeeLobby();
		if (enabled)
		{
			m_backSubscriptionSeeLobby = BackSystem.Instance.Register(HandleBackSeeLobby, "FriendProfileView HandleBackSeeLobby");
			lobbyCameraEnabledOnEnter = MainMenuController.sharedController.rotateCamera.gameObject.activeSelf;
			layerBeforeEnter = LayerMask.LayerToName(characterInterface.gameObject.layer);
		}
		background.SetActiveSafeSelf(!enabled);
		allInterfacePanel.SetActiveSafeSelf(!enabled);
		panelSeeLobby.SetActiveSafeSelf(enabled);
		MainMenuController.sharedController.rotateCamera.gameObject.SetActiveSafeSelf(enabled || lobbyCameraEnabledOnEnter);
		if (MainMenuController.sharedController.InMiniGamesScreen)
		{
			MainMenuController.sharedController.miniGamesPoint.GetChild(0).gameObject.SetActive(!enabled);
			MainMenuHeroCamera.Instance.MainCamera.enabled = enabled;
		}
		if (enabled)
		{
			LobbyCraftController.Instance.SetSceneViews(lobby, false);
		}
		else
		{
			LobbyCraftController.Instance.SetMySceneViews(true);
		}
		if (!initialPersPosLobby.HasValue)
		{
			initialPersPosLobby = PersConfigurator.currentConfigurator.characterInterface.transform.position;
			initialPersPosProfile = characterInterface.transform.position;
			initialScaleLobby = PersConfigurator.currentConfigurator.characterInterface.transform.localScale;
			initialScaleProfile = characterInterface.transform.localScale;
		}
		PersConfigurator.currentConfigurator.characterInterface.transform.localPosition += (enabled ? new Vector3(0f, -100f, 0f) : new Vector3(0f, 100f, 0f));
		GameObject myPet = PersConfigurator.currentConfigurator.characterInterface.myPet;
		if (myPet != null)
		{
			myPet.transform.localPosition += (enabled ? new Vector3(0f, -100f, 0f) : new Vector3(0f, 100f, 0f));
		}
		characterInterface.transform.parent = (enabled ? PersConfigurator.currentConfigurator.characterInterface.transform.parent : pers);
		characterInterface.transform.position = (enabled ? initialPersPosLobby.Value : initialPersPosProfile.Value);
		characterInterface.transform.localRotation = Quaternion.identity;
		pers.transform.rotation = Quaternion.Euler(initialRot);
		PersConfigurator.currentConfigurator.transform.localRotation = Quaternion.Euler(MainMenuController.InitialRotation);
		characterInterface.transform.localScale = (enabled ? initialScaleLobby.Value : initialScaleProfile.Value);
		characterInterface.gameObject.SetLayerRecursively(LayerMask.NameToLayer(enabled ? "Default" : layerBeforeEnter));
		try
		{
			RiliExtensions.ForEach(BackgroundsManager.GetObjectsForHide(false), delegate(GameObject o)
			{
				BackgroundsManager.SetObjectVisible(o, !enabled);
			});
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in setting background objects visible: {0}", ex);
		}
		if (enabled)
		{
			playerNameSeeLobby.ForEach(delegate(UILabel l)
			{
				l.text = FriendName ?? string.Empty;
			});
			UpdateLikes();
		}
	}

	private void UpdateLikes()
	{
		if (SceneManagerHelper.ActiveSceneName != Defs.MainMenuScene || FriendsController.sharedController == null)
		{
			return;
		}
		if (FriendId.IsNullOrEmpty())
		{
			UnityEngine.Debug.LogErrorFormat("UpdateLikes FriendId.IsNullOrEmpty()");
			return;
		}
		try
		{
			bool flag = FriendsController.sharedController.PlayersLikesAlreadySentTo.Contains(FriendId);
			dislikeContainer.SetActiveSafeSelf(flag);
			likeContainer.SetActiveSafeSelf(!flag);
			bool flag2 = FriendsController.sharedController.IsSendingLikeForPlayer(FriendId);
			likeLobby.isEnabled = !flag2;
			dislikeLobby.isEnabled = !flag2;
			likesCount.text = Mathf.Max(0, LikesNumFromServer + LikesNumAdding).ToString();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in UpdateLikes: {0}", ex);
		}
	}

	private void UnsubscribeFromBackSystemSeeLobby()
	{
		if (m_backSubscriptionSeeLobby != null)
		{
			m_backSubscriptionSeeLobby.Dispose();
			m_backSubscriptionSeeLobby = null;
		}
	}

	public void Reset()
	{
		IsCanConnectToFriend = false;
		FriendLocation = string.Empty;
		FriendCount = 0;
		FriendName = string.Empty;
		Online = (FriendsController.IsPlayerOurFriend(FriendId) ? OnlineState.offline : OnlineState.none);
		Rank = 0;
		SurvivalScore = 0;
		Username = string.Empty;
		WinCount = 0;
		if (characterModel != null)
		{
			Texture texture = Resources.Load<Texture>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1"));
			if (texture != null)
			{
				characterInterface.SetSkin(texture);
			}
		}
		SetOnlineState(Online);
		characterInterface.RemoveBoots();
		characterInterface.RemoveHat();
		characterInterface.RemoveMask();
		characterInterface.RemoveCape();
		characterInterface.RemoveArmor();
		SetEnableAddButton(true);
		SetEnableInviteClanButton(true);
	}

	public void SetBoots(string name)
	{
		characterInterface.UpdateBoots(name);
	}

	private void SetOnlineState(OnlineState onlineState)
	{
		bool isStateOffline = onlineState == OnlineState.offline;
		bool isStateInFriends = onlineState == OnlineState.inFriends;
		bool isStatePlaying = onlineState == OnlineState.playing;
		offlineStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStateOffline);
		});
		inFriendStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStateInFriends);
		});
		playingStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStatePlaying);
		});
		offlineState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStateOffline);
		});
		inFriendState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStateInFriends);
		});
		playingState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStatePlaying);
		});
		if (playingStateInfoContainer != null)
		{
			playingStateInfoContainer.SetActive(isStatePlaying);
		}
	}

	public void SetStockCape(string capeName)
	{
		if (string.IsNullOrEmpty(capeName))
		{
			UnityEngine.Debug.LogWarning("Name of cape should not be empty.");
		}
		else
		{
			characterInterface.UpdateCape(capeName);
		}
	}

	public void SetCustomCape(byte[] capeBytes)
	{
		capeBytes = capeBytes ?? new byte[0];
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(capeBytes);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		characterInterface.UpdateCape("cape_Custom", texture2D);
	}

	public void SetArmor(string armorName)
	{
		if (string.IsNullOrEmpty(armorName))
		{
			UnityEngine.Debug.LogWarning("Name of armor should not be empty.");
		}
		else
		{
			characterInterface.UpdateArmor(armorName);
		}
	}

	public void SetHat(string hatName)
	{
		if (string.IsNullOrEmpty(hatName))
		{
			UnityEngine.Debug.LogWarning("Name of hat should not be empty.");
		}
		else
		{
			characterInterface.UpdateHat(hatName);
		}
	}

	public void SetMask(string maskName)
	{
		if (string.IsNullOrEmpty(maskName))
		{
			UnityEngine.Debug.LogWarning("Name of mask should not be empty.");
		}
		else
		{
			characterInterface.UpdateMask(maskName);
		}
	}

	public void SetSkin(byte[] skinBytes)
	{
		skinBytes = skinBytes ?? new byte[0];
		if (characterModel != null)
		{
			Func<byte[], Texture2D> func = delegate(byte[] bytes)
			{
				Texture2D texture2D = new Texture2D(64, 32);
				texture2D.filterMode = FilterMode.Point;
				texture2D.LoadImage(bytes);
				texture2D.Apply();
				return texture2D;
			};
			Texture2D skin = ((skinBytes.Length != 0) ? func(skinBytes) : Resources.Load<Texture2D>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")));
			characterInterface.SetSkin(skin);
		}
	}

	private void Awake()
	{
		DisableRemoveFriendButton();
		if (SceneManagerHelper.ActiveSceneName != Defs.MainMenuScene && seeLobby != null)
		{
			seeLobby.gameObject.SetActiveSafeSelf(false);
		}
		GameObject original = Resources.Load("Character_model") as GameObject;
		characterInterface = UnityEngine.Object.Instantiate(original).GetComponent<CharacterInterface>();
		characterInterface.GetComponent<CharacterInterface>().usePetFromStorager = false;
		characterInterface.transform.SetParent(pers, false);
		characterInterface.SetCharacterType(true, true, false);
		Player_move_c.SetLayerRecursively(characterInterface.gameObject, pers.gameObject.layer);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		Reset();
		if (backButton != null)
		{
			EventDelegate.Add(backButton.onClick, OnBackButtonClick);
		}
		if (joinButton != null)
		{
			EventDelegate.Add(joinButton.onClick, OnJoinButtonClick);
		}
		if (sendMyIdButton != null)
		{
			EventDelegate.Add(sendMyIdButton.onClick, OnSendMyIdButtonClick);
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				sendMyIdButton.gameObject.SetActive(false);
			}
		}
		if (chatButton != null)
		{
			EventDelegate.Add(chatButton.onClick, OnChatButtonClick);
		}
		if (addFriendButton != null)
		{
			EventDelegate.Add(addFriendButton.onClick, OnAddButtonClick);
		}
		if (removeFriendButton != null)
		{
			EventDelegate.Add(removeFriendButton.onClick, OnRemoveButtonClick);
		}
		if (inviteToClanButton != null)
		{
			EventDelegate.Add(inviteToClanButton.onClick, OnInviteToClanButtonClick);
		}
		FriendsController.OnSendLikeLobby += FriendsController_OnSendLikeLobby;
	}

	private void FriendsController_OnSendLikeLobby(string playerId, bool isPositiv, int likesCount)
	{
		if (FriendId.IsNullOrEmpty())
		{
			UnityEngine.Debug.LogErrorFormat("FriendsController_OnSendLikeLobby friend id is null or empty");
		}
		else if (!(FriendId != playerId))
		{
			LikesNumFromServer = likesCount;
		}
	}

	private void OnDestroy()
	{
		FriendsController.OnSendLikeLobby -= FriendsController_OnSendLikeLobby;
	}

	private void OnDisable()
	{
		StopCoroutine("RequestUpdate");
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		if (SceneManagerHelper.ActiveSceneName == Defs.MainMenuScene)
		{
			preloadedLobbyItemsForSeeLobby.Clear();
		}
	}

	private void OnEnable()
	{
		StartCoroutine("RequestUpdate");
		idleTimerLastTime = Time.realtimeSinceStartup + 1000000f;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Friend Profile");
		if (SceneManagerHelper.ActiveSceneName == Defs.MainMenuScene)
		{
			preloadedLobbyItemsForSeeLobby.Clear();
		}
		lobby = new List<string>();
		UpdateSeeLobbyButton();
	}

	private IEnumerator LoadLobbyItems()
	{
		preloadedLobbyItemsForSeeLobby.Clear();
		foreach (string item in lobby)
		{
			ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>("LobbyItemsContent/" + item);
			if (resourceRequest != null)
			{
				preloadedLobbyItemsForSeeLobby.Add(resourceRequest);
				yield return resourceRequest;
			}
		}
		try
		{
			if (panelSeeLobby != null && panelSeeLobby.activeInHierarchy)
			{
				LobbyCraftController.Instance.SetSceneViews(lobby, false);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in LoadLobbyItems: {0}", ex);
		}
	}

	private void HandleEscape()
	{
		if (!InfoWindowController.IsActive)
		{
			_escapePressed = true;
		}
	}

	private void OnBackButtonClick()
	{
		if (this.BackButtonClickEvent != null)
		{
			this.BackButtonClickEvent();
		}
	}

	private void OnJoinButtonClick()
	{
		if (this.JoinButtonClickEvent != null)
		{
			this.JoinButtonClickEvent();
		}
	}

	private void OnSendMyIdButtonClick()
	{
		if (this.CopyMyIdButtonClickEvent != null)
		{
			this.CopyMyIdButtonClickEvent();
		}
	}

	private void OnChatButtonClick()
	{
		if (this.ChatButtonClickEvent != null)
		{
			this.ChatButtonClickEvent();
		}
	}

	private void OnAddButtonClick()
	{
		if (this.AddButtonClickEvent != null)
		{
			this.AddButtonClickEvent();
		}
	}

	private void OnRemoveButtonClick()
	{
		if (this.RemoveButtonClickEvent != null)
		{
			this.RemoveButtonClickEvent();
		}
	}

	private void OnInviteToClanButtonClick()
	{
		if (this.InviteToClanButtonClickEvent != null)
		{
			this.InviteToClanButtonClickEvent();
		}
	}

	private IEnumerator RequestUpdate()
	{
		while (true)
		{
			if (this.UpdateRequested != null)
			{
				this.UpdateRequested();
			}
			yield return new WaitForSeconds(5f);
		}
	}

	private void DisableRemoveFriendButton()
	{
		if (SceneManagerHelper.ActiveSceneName == "Clans")
		{
			removeFriendButton.gameObject.SetActiveSafeSelf(false);
		}
	}

	private void Update()
	{
		if (_escapePressed)
		{
			_escapePressed = false;
			OnBackButtonClick();
			return;
		}
		UpdateLightweight();
		float rotationRateForCharacterInMenues = RilisoftRotator.RotationRateForCharacterInMenues;
		Rect value = _touchZone.Value;
		if (panelSeeLobby != null && panelSeeLobby.activeSelf)
		{
			if (MainMenuController.sharedController != null && !MainMenuController.sharedController.gameObject.activeInHierarchy)
			{
				try
				{
					MainMenuController.sharedController.RotateCharacter(null);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in MainMenuController.sharedController.RotateCharacter: {0}", ex);
				}
			}
			UpdateLikes();
		}
		else
		{
			RilisoftRotator.RotateCharacter(pers, rotationRateForCharacterInMenues, value, ref idleTimerLastTime, ref lastTime);
		}
		if (Time.realtimeSinceStartup - idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			ReturnPersTonNormState();
		}
		DisableRemoveFriendButton();
		UpdateSeeLobbyButton();
	}

	public void UpdateSeeLobbyButton()
	{
		if (seeLobby != null)
		{
			bool flag = !FriendId.IsNullOrEmpty() && FriendsController.sharedController != null && FriendsController.sharedController.id == FriendId;
			seeLobby.gameObject.SetActiveSafeSelf(!flag);
			seeLobby.isEnabled = lobby != null && lobby.Count > 0 && !flag;
		}
	}

	private void ReturnPersTonNormState()
	{
		HOTween.Kill(pers);
		idleTimerLastTime += 1000000f;
		HOTween.To(pers, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(initialRot)).Ease(EaseType.Linear).OnComplete((TweenDelegate.TweenCallback)delegate
		{
			idleTimerLastTime += 1000000f;
		}));
	}

	private void UpdateLightweight()
	{
		if (friendLocationLabel != null)
		{
			friendLocationLabel.text = FriendLocation ?? string.Empty;
		}
		if (friendCountLabel != null)
		{
			friendCountLabel.text = ((FriendCount < 0) ? "-" : FriendCount.ToString());
		}
		if (friendNameLabel != null)
		{
			friendNameLabel.text = FriendName ?? string.Empty;
		}
		SetOnlineState(Online);
		notConnectJoinButtonSprite.alpha = (IsCanConnectToFriend ? 0f : 1f);
		if (rankSprite != null)
		{
			string text = "Rank_" + Rank;
			if (!rankSprite.spriteName.Equals(text))
			{
				rankSprite.spriteName = text;
			}
		}
		if (survivalScoreLabel != null)
		{
			survivalScoreLabel.text = ((SurvivalScore < 0) ? "-" : SurvivalScore.ToString());
		}
		if (winCountLabel != null)
		{
			winCountLabel.text = ((WinCount < 0) ? "-" : WinCount.ToString());
		}
		if (totalWinCountLabel != null)
		{
			totalWinCountLabel.text = ((TotalWinCount < 0) ? "-" : TotalWinCount.ToString());
		}
		if (friendGameModeLabel != null)
		{
			friendGameModeLabel.text = FriendGameMode;
		}
		if (friendIdLabel != null)
		{
			friendIdLabel.text = FriendId;
		}
	}

	public void SetTitle(string titleText)
	{
		for (int i = 0; i < titlesLabel.Length; i++)
		{
			titlesLabel[i].text = titleText;
		}
	}

	private void SetActiveAndRepositionButtons(GameObject button, bool isActive)
	{
		bool activeSelf = button.activeSelf;
		button.SetActive(isActive);
		if (activeSelf != isActive)
		{
			buttonAlignContainer.Reposition();
			buttonAlignContainer.repositionNow = true;
		}
	}

	public void SetActiveChatButton(bool isActive)
	{
		SetActiveAndRepositionButtons(chatButton.gameObject, isActive);
	}

	public void SetActiveInviteButton(bool isActive)
	{
		SetActiveAndRepositionButtons(inviteToClanButton.gameObject, isActive);
	}

	public void SetActiveAddButton(bool isActive)
	{
		SetActiveAndRepositionButtons(addFriendButton.gameObject, isActive);
	}

	public void SetActiveAddButtonSent(bool isActive)
	{
		SetActiveAndRepositionButtons(addFrienButtonSentState.gameObject, isActive);
	}

	public void SetActiveAddClanButtonSent(bool isActive)
	{
		SetActiveAndRepositionButtons(addClanButtonSentState.gameObject, isActive);
	}

	public void SetActiveRemoveButton(bool isActive)
	{
		SetActiveAndRepositionButtons(removeFriendButton.gameObject, isActive);
	}

	public void SetEnableAddButton(bool enable)
	{
		if (addFriendButton != null)
		{
			addFriendButton.isEnabled = enable;
		}
	}

	public void SetEnableRemoveButton(bool enable)
	{
		if (removeFriendButton != null)
		{
			removeFriendButton.isEnabled = enable;
		}
	}

	public void SetEnableInviteClanButton(bool enable)
	{
		if (inviteToClanButton != null)
		{
			inviteToClanButton.isEnabled = enable;
		}
	}
}
