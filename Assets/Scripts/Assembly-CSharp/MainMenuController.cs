using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Facebook.Unity;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuController : ControlsSettingsBase
{
	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__122 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pausing;

		public MainMenuController _003C_003E4__this;

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
		public _003COnApplicationPause_003Ed__122(int _003C_003E1__state)
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
				if (!pausing)
				{
					_003C_003E2__current = new WaitForSeconds(1f);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				string text = string.Empty;
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
					{
						MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
						text = "`SuppressShowOnReturnFromPause`";
					}
					else if (ReplaceAdmobPerelivController.sharedController == null)
					{
						text = "`ReplaceAdmobPerelivController.sharedController == null`";
					}
					else if (!FreeAwardController.FreeAwardChestIsInIdleState)
					{
						text = "`FreeAwardChestIsInIdleState == false`";
					}
					else
					{
						text = ConnectScene.GetReasonToDismissFakeInterstitial();
						if (string.IsNullOrEmpty(text))
						{
							ReplaceAdmobPerelivController.IncreaseTimesCounter();
							_003C_003E4__this.StartCoroutine(_003C_003E4__this.LoadAndShowReplaceAdmobPereliv("On return from pause to Lobby"));
						}
					}
				}
				if (!string.IsNullOrEmpty(text) && Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing fake interstitial. {0}</color>" : "Dismissing fake interstitial. {0}", text);
				}
				ReloadFacebookFriends();
				break;
			}
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
	internal sealed class _003CLoadAndShowReplaceAdmobPereliv_003Ed__124 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

		public string context;

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
		public _003CLoadAndShowReplaceAdmobPereliv_003Ed__124(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1 || num == 2)
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
					if (_003C_003E4__this.loadReplaceAdmobPerelivRunning)
					{
						break;
					}
					_003C_003E1__state = -3;
					_003C_003E4__this.loadReplaceAdmobPerelivRunning = true;
					if (!ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
					{
						ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
					}
					goto IL_00ab;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ab;
				case 2:
					{
						_003C_003E1__state = -3;
						if (FreeAwardController.FreeAwardChestIsInIdleState && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown))
						{
							ReplaceAdmobPerelivController.TryShowPereliv(context);
							ReplaceAdmobPerelivController.sharedController.DestroyImage();
						}
						_003C_003Em__Finally1();
						break;
					}
					IL_00ab:
					if (!ReplaceAdmobPerelivController.sharedController.DataLoaded)
					{
						if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
						{
							_003C_003E4__this.loadReplaceAdmobPerelivRunning = false;
							result = false;
							_003C_003Em__Finally1();
						}
						else
						{
							_003C_003E2__current = null;
							_003C_003E1__state = 1;
							result = true;
						}
					}
					else
					{
						_003C_003E2__current = new WaitForSeconds(0.5f);
						_003C_003E1__state = 2;
						result = true;
					}
					goto end_IL_0000;
				}
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
			_003C_003E4__this.loadReplaceAdmobPerelivRunning = false;
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__134 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

		private GameObject _003CprofileGuiRequest_003E5__1;

		private GameServicesController _003CgameServicesController_003E5__2;

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
		public _003CStart_003Ed__134(int _003C_003E1__state)
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
				_003C_003E4__this.LoadConnectSceneCoroutine();
				FriendsController.sharedController.timerSendTimeGame = 0f;
				if (Defs.IsDeveloperBuild && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1 && !TrainingController.TrainingCompleted)
				{
					TrainingController.OnGetProgress();
				}
				if (!TrainingController.TrainingCompleted)
				{
					if (CloudSyncController.AreProgressInCurrentPullResult())
					{
						TrainingController.OnGetProgress();
						CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(false, true));
					}
					else if (m_firstEnterToLobby && !CloudSyncController.Instance.IsAuthenticatedToCloud())
					{
						CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_NotAuthenticated());
					}
				}
				else if (TrainingController.ShouldSyncInLobbyAfterSkippingRancho)
				{
					CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(false, true));
					TrainingController.ShouldSyncInLobbyAfterSkippingRancho = false;
				}
				else if (m_firstEnterToLobby && !CloudSyncController.Instance.IsAuthenticatedToCloud())
				{
					CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_NotAuthenticated_SecondLaunch());
				}
				else if (!CloudSyncController.IsSynchronizingWithCloud)
				{
					CoroutineRunner.Instance.StartCoroutine(CloudSyncController.Instance.PullAndApplyChangesIfNeeded());
				}
				m_firstEnterToLobby = false;
				_003C_003E4__this.UpdateInappBonusChestActiveState();
				LogsManager.DisableLogsIfAllowed();
				FreeTicketsController.Instance.SaveState();
				if (Storager.hasKey("Analytics:af_tutorial_completion"))
				{
					AnalyticsStuff.TrySendOnceToAppsFlyer("tutorial_lobby");
				}
				string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
				string text = FilterBadWorld.FilterString(playerNameOrDefault);
				if (string.IsNullOrEmpty(text) || text.Trim() == string.Empty)
				{
					text = ProfileController.defaultPlayerName;
				}
				if (text != playerNameOrDefault)
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.Log("Saving new name:    " + text);
					}
					PlayerPrefs.SetString("NamePlayer", text);
				}
				Storager.setInt(Defs.ShownLobbyLevelSN, 3);
				_003C_003E4__this.transform.GetChild(0).GetComponent<UICamera>().allowMultiTouch = false;
				SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult += _003C_003E4__this.HandleSocialGunViewLoginCompleted;
				TwitterController.CheckAndGiveTwitterReward("Start");
				FacebookController.CheckAndGiveFacebookReward("Start");
				ReloadFacebookFriends();
				if (FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id))
				{
					ClanIncomingInvitesController.FetchClanIncomingInvites(FriendsController.sharedController.id);
				}
				_003C_003E4__this.CheckIfPendingAward();
				if (_003C_003E4__this.socialButton != null)
				{
					_003C_003E4__this.socialButton.gameObject.SetActive(true);
					_003C_003E4__this.socialButton.GetComponent<ButtonHandler>().Clicked += _003C_003E4__this.HandleSocialButton;
				}
				ExperienceController.RefreshExpControllers();
				PlayerPrefs.SetInt("CountRunMenu", PlayerPrefs.GetInt("CountRunMenu", 0) + 1);
				_003C_003E4__this.premiumTime.gameObject.SetActive(true);
				_003C_003E4__this.InitializeBannerWindow();
				bool isDebugBuild = UnityEngine.Debug.isDebugBuild;
				if (_003C_003E4__this.developerConsole != null)
				{
					_003C_003E4__this.developerConsole.gameObject.SetActive(isDebugBuild);
				}
				_003C_003E4__this.starterPackPanel.gameObject.SetActive(false);
				TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
				if (trafficForwardingScript != null)
				{
					trafficForwardingScript.Updated = (EventHandler<TrafficForwardingInfo>)Delegate.Combine(trafficForwardingScript.Updated, new EventHandler<TrafficForwardingInfo>(_003C_003E4__this.RefreshTrafficForwardingButton));
					Task<TrafficForwardingInfo> val = trafficForwardingScript.GetTrafficForwardingInfo().Filter((Task<TrafficForwardingInfo> t) => ((Task)t).IsCompleted && !((Task)t).IsCanceled && !((Task)t).IsFaulted);
					if (val != null)
					{
						_003C_003E4__this._trafficForwardingUrl = val.Result.Url;
					}
					if (_003C_003E4__this.trafficForwardingButton != null)
					{
						_003C_003E4__this.RefreshTrafficForwardingButton(_003C_003E4__this, (val != null) ? val.Result : TrafficForwardingInfo.DisabledInstance);
					}
				}
				_003C_003E4__this.dayOfValorContainer.gameObject.SetActive(false);
				_003C_003E4__this.stubTexture.mainTexture = ConnectScene.MainLoadingTexture();
				HOTween.Init(true, true, true);
				HOTween.EnableOverwriteManager();
				_003C_003E4__this.idleTimerLastTime = Time.realtimeSinceStartup + 10000000f;
				SettingsController.ControlsClicked += _003C_003E4__this.HandleControlsClicked;
				Defs.isShowUserAgrement = false;
				_003C_003E4__this.completeTraining.SetActive(!_003C_003E4__this.shopButton.GetComponent<UIButton>().isEnabled);
				_003C_003E4__this.mainPanel.SetActive(true);
				_003C_003E4__this.settingsPanel.SetActive(false);
				if (_003C_003E4__this._newsPanel.ObjectIsActive)
				{
					_003C_003E4__this._newsPanel.Value.gameObject.SetActive(false);
				}
				if (_003C_003E4__this._freePanel.ObjectIsActive)
				{
					_003C_003E4__this._freePanel.Value.SetVisible(false);
				}
				_003C_003E4__this.SettingsJoysticksPanel.SetActive(false);
				sharedController = _003C_003E4__this;
				if (_003C_003E4__this.multiplayerButton != null)
				{
					ButtonHandler component = _003C_003E4__this.multiplayerButton.GetComponent<ButtonHandler>();
					if (component != null)
					{
						component.Clicked += _003C_003E4__this.HandleMultiPlayerClicked;
					}
				}
				if (_003C_003E4__this.skinsMakerButton != null)
				{
					if (MainMenu.SkinsMakerSupproted())
					{
						ButtonHandler component2 = _003C_003E4__this.skinsMakerButton.GetComponent<ButtonHandler>();
						if (component2 != null)
						{
							component2.Clicked += _003C_003E4__this.HandleSkinsMakerClicked;
						}
					}
					else
					{
						_003C_003E4__this.skinsMakerButton.SetActive(false);
					}
				}
				if (_003C_003E4__this.profileButton != null)
				{
					ButtonHandler component3 = _003C_003E4__this.profileButton.GetComponent<ButtonHandler>();
					if (component3 != null)
					{
						component3.Clicked += _003C_003E4__this.HandleProfileClicked;
					}
				}
				if (_003C_003E4__this.secondProfileButton != null)
				{
					ButtonHandler component4 = _003C_003E4__this.secondProfileButton.GetComponent<ButtonHandler>();
					if (component4 != null)
					{
						component4.Clicked += _003C_003E4__this.HandleProfileClicked;
					}
				}
				if (_003C_003E4__this.freeButton != null)
				{
					ButtonHandler component5 = _003C_003E4__this.freeButton.GetComponent<ButtonHandler>();
					if (component5 != null)
					{
						component5.Clicked += _003C_003E4__this.HandleFreeClicked;
					}
				}
				if (_003C_003E4__this.shopButton != null)
				{
					ButtonHandler component6 = _003C_003E4__this.shopButton.GetComponent<ButtonHandler>();
					if (component6 != null)
					{
						component6.Clicked += _003C_003E4__this.HandleShopClicked;
					}
				}
				if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
				{
					_003C_003E4__this.HandleShopClicked(null, null);
					PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
					PlayerPrefs.Save();
				}
				_003C_003E4__this.RefreshSettingsButton();
				if (_003C_003E4__this._openFeedBackBtn != null)
				{
					_003C_003E4__this._openFeedBackBtn.Clicked += _003C_003E4__this.HandleSupportButtonClicked;
				}
				if (_003C_003E4__this.friendsButton != null)
				{
					ButtonHandler component7 = _003C_003E4__this.friendsButton.GetComponent<ButtonHandler>();
					if (component7 != null)
					{
						component7.Clicked += _003C_003E4__this.HandleFriendsClicked;
					}
				}
				if (_003C_003E4__this._openNewsBtn != null)
				{
					_003C_003E4__this._openNewsBtn.Clicked += _003C_003E4__this.HandleNewsButtonClicked;
				}
				if (_003C_003E4__this.diclineButton != null)
				{
					ButtonHandler component8 = _003C_003E4__this.diclineButton.GetComponent<ButtonHandler>();
					if (component8 != null)
					{
						component8.Clicked += _003C_003E4__this.HandleDiclineClicked;
					}
				}
				if (_003C_003E4__this.openCraftBtn != null)
				{
					ButtonHandler component9 = _003C_003E4__this.openCraftBtn.GetComponent<ButtonHandler>();
					if (component9 != null)
					{
						component9.Clicked += _003C_003E4__this.HandleCraftButtonClicked;
					}
				}
				if (BankController.Instance != null)
				{
					UnityEngine.Object.DontDestroyOnLoad(BankController.Instance.transform.root.gameObject);
				}
				else
				{
					UnityEngine.Debug.LogWarning("bankController == null");
				}
				if (NavigateToMinigame.HasValue)
				{
					_003C_003E4__this.OpenMiniGames(NavigateToMinigame.Value);
					NavigateToMinigame = null;
					ConnectScene.isReturnFromGame = false;
					isShowConnectSceneOnStart = false;
				}
				_003C_003E2__current = new WaitForSeconds(0.5f);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				PromoActionClick.Click += _003C_003E4__this.HandlePromoActionClicked;
				_003C_003E2__current = new WaitForSeconds(0.5f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				if (friendsOnStart)
				{
					_003C_003E4__this.HandleFriendsClicked(null, null);
					_003C_003E4__this.rotateCamera.gameObject.SetActive(false);
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_097c;
			case 3:
				_003C_003E1__state = -1;
				goto IL_097c;
			case 4:
				_003C_003E1__state = -1;
				goto IL_097c;
			case 5:
				_003C_003E1__state = -1;
				UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(_003CprofileGuiRequest_003E5__1));
				_003CprofileGuiRequest_003E5__1 = null;
				goto IL_0a30;
			case 6:
				{
					_003C_003E1__state = -1;
					goto IL_0ac6;
				}
				IL_097c:
				if ((ConnectScene.isReturnFromGame || isShowConnectSceneOnStart) && _003C_003E4__this.connectScene == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				_drawLoadingProgress = false;
				_003C_003E4__this.stubLoading.SetActive(false);
				ActivityIndicator.IsActiveIndicator = false;
				if (!ShopNGUIController.GuiActive)
				{
					ExperienceController.sharedController.isShowRanks = true;
				}
				BannerWindowController.SharedController.ResetScene();
				UnityEngine.Debug.Log("Start initializing ProfileGui.");
				if (UnityEngine.Object.FindObjectOfType<ProfileController>() == null)
				{
					_003CprofileGuiRequest_003E5__1 = Resources.Load<GameObject>("ProfileGui");
					_003C_003E2__current = _003CprofileGuiRequest_003E5__1;
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_0a30;
				IL_0a30:
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("Training completed: {0}. Authenticating...", TrainingController.TrainingCompleted);
				}
				if (_socialNetworkingInitilized)
				{
					break;
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Social networking is not initialized.");
				}
				_003CgameServicesController_003E5__2 = UnityEngine.Object.FindObjectOfType<GameServicesController>();
				if (_003CgameServicesController_003E5__2 == null)
				{
					GameObject gameObject = new GameObject("Rilisoft.GameServicesController");
					_003CgameServicesController_003E5__2 = gameObject.AddComponent<GameServicesController>();
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					GameCenterSingleton instance = GameCenterSingleton.Instance;
					_003C_003E2__current = null;
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_0ac6;
				IL_0ac6:
				_socialNetworkingInitilized = true;
				_003CgameServicesController_003E5__2.WaitAuthenticationAndIncrementBeginnerAchievement();
				_003CgameServicesController_003E5__2 = null;
				break;
			}
			if (_003C_003E4__this.bannerContainer != null)
			{
				InGameGUI.SetLayerRecursively(_003C_003E4__this.bannerContainer, LayerMask.NameToLayer("Banners"));
			}
			StarterPackController.OnStarterPackEnable += _003C_003E4__this.OnStarterPackContainerShow;
			_003C_003E4__this.OnStarterPackContainerShow(StarterPackController.Get.isEventActive);
			PromoActionsManager.OnDayOfValorEnable += _003C_003E4__this.OnDayOfValorContainerShow;
			_003C_003E4__this.OnDayOfValorContainerShow(PromoActionsManager.sharedManager.IsDayOfValorEventActive);
			if (ReplaceAdmobPerelivController.sharedController != null && ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby && ReplaceAdmobPerelivController.sharedController.DataLoaded)
			{
				ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby = false;
				ReplaceAdmobPerelivController.TryShowPereliv("Lobby after launch");
				ReplaceAdmobPerelivController.sharedController.DestroyImage();
			}
			string abuseKey_f1a4329e = GetAbuseKey_f1a4329e(4054069918u);
			if (Storager.hasKey(abuseKey_f1a4329e))
			{
				string @string = Storager.getString(abuseKey_f1a4329e);
				if (!string.IsNullOrEmpty(@string) && @string != "0")
				{
					long num = DateTime.UtcNow.Ticks >> 1;
					long result = num;
					if (long.TryParse(@string, out result))
					{
						result = Math.Min(num, result);
						Storager.setString(abuseKey_f1a4329e, result.ToString());
					}
					else
					{
						Storager.setString(abuseKey_f1a4329e, num.ToString());
					}
					TimeSpan timeSpan = TimeSpan.FromTicks(num - result);
					if ((Defs.IsDeveloperBuild ? (timeSpan.TotalMinutes >= 3.0) : (timeSpan.TotalDays >= 1.0)) && Application.platform != RuntimePlatform.IPhonePlayer)
					{
						PhotonNetwork.PhotonServerSettings.AppID = "68c9fbdb-682a-411f-a229-1a9786b5835c";
						PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
					}
				}
			}
			_003C_003E4__this.StartCoroutine(_003C_003E4__this.TryToShowExpiredBanner());
			_003C_003E4__this.StopCoroutine("TryToShowMyLobbyLiked");
			_003C_003E4__this.StartCoroutine("TryToShowMyLobbyLiked");
			if (friendsOnStart)
			{
				if (_003C_003E4__this.mainPanel != null)
				{
					_003C_003E4__this.mainPanel.transform.root.gameObject.SetActive(false);
				}
				friendsOnStart = false;
			}
			_003C_003E4__this.newsIndicator.SetActive(PlayerPrefs.GetInt("LobbyIsAnyNewsKey", 0) == 1);
			if (MainMenuController.onLoadMenu != null)
			{
				MainMenuController.onLoadMenu();
			}
			if (MainMenuController.onEnableMenuForAskname != null)
			{
				MainMenuController.onEnableMenuForAskname();
			}
			QuestSystem.Instance.QuestCompleted -= OnCompletedQuest;
			QuestSystem.Instance.QuestCompleted += OnCompletedQuest;
			_003C_003E4__this.InitLobbyCraftController();
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
	internal sealed class _003CTryToShowMyLobbyLiked_003Ed__145 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

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
		public _003CTryToShowMyLobbyLiked_003Ed__145(int _003C_003E1__state)
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
				goto IL_003f;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0065;
			case 3:
				{
					_003C_003E1__state = -1;
					try
					{
						if (_003C_003E4__this.IsSomeWindowOpenExceptSettings() || _003C_003E4__this.settingsPanel.activeInHierarchy || ConnectScene.isEnable || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || _003C_003E4__this.RentExpiredPoint.childCount != 0)
						{
							break;
						}
						int num = FriendsController.sharedController.OurLobbyLikes.Likes - Storager.getInt("MainMenuController.SHOWN_OUR_LIKES_KEY");
						if (num != 0)
						{
							if (num > 0)
							{
								InfoWindowController.ShowAchievementsBox((num == 1 && !FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked.IsNullOrEmpty()) ? string.Format(LocalizationStore.Get("Key_3298"), new object[1] { FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked }) : string.Format(LocalizationStore.Get("Key_3299"), new object[1] { num }), null, "", true);
								FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked = string.Empty;
							}
							Storager.setInt("MainMenuController.SHOWN_OUR_LIKES_KEY", FriendsController.sharedController.OurLobbyLikes.Likes);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("exception in Lobby  TryToShowMyLobbyLiked: " + ex);
					}
					break;
				}
				IL_003f:
				if (FriendsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0065;
				IL_0065:
				if (Time.realtimeSinceStartup - _003C_003E4__this.timeEnteredLobby < 3f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			_003C_003E1__state = 3;
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

	[CompilerGenerated]
	internal sealed class _003CTryToShowExpiredBanner_003Ed__146 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

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
		public _003CTryToShowExpiredBanner_003Ed__146(int _003C_003E1__state)
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
				goto IL_003b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 2:
				{
					_003C_003E1__state = -1;
					try
					{
						if (!_003C_003E4__this.IsSomeWindowOpenExceptSettings() && !_003C_003E4__this.settingsPanel.activeInHierarchy && (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && _003C_003E4__this.RentExpiredPoint.childCount == 0)
						{
							if (!SavedShwonLobbyLevelIsLessThanActual())
							{
								int num;
								if (Storager.getInt(Defs.PremiumEnabledFromServer) == 1)
								{
									ShopNGUIController.ShowPremimAccountExpiredIfPossible(_003C_003E4__this.RentExpiredPoint, "NGUI");
								}
								else
									num = 0;
								break;
							}
							GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LobbyLevels/LobbyLevelTips_" + (Storager.getInt(Defs.ShownLobbyLevelSN) + 1)));
							gameObject.transform.parent = _003C_003E4__this.RentExpiredPoint;
							Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUI"));
							gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
							gameObject.transform.localRotation = Quaternion.identity;
							gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogWarning("exception in Lobby  TryToShowExpiredBanner: " + ex);
					}
					break;
				}
				IL_003b:
				if (FriendsController.sharedController == null || TempItemsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			_003C_003E1__state = 2;
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

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass155_0
	{
		public bool buttonsEnabled;

		internal void _003CSetSocialButtonsState_003Eb__0(UIButton b)
		{
			b.isEnabled = buttonsEnabled;
		}

		internal void _003CSetSocialButtonsState_003Eb__1(UIButton b)
		{
			b.isEnabled = buttonsEnabled;
		}

		internal void _003CSetSocialButtonsState_003Eb__2(UIButton b)
		{
			b.isEnabled = buttonsEnabled;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CSetSocialButtonsState_003Ed__155 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

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
		public _003CSetSocialButtonsState_003Ed__155(int _003C_003E1__state)
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
			if (TrainingController.TrainingCompleted)
			{
				_003C_003Ec__DisplayClass155_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass155_0
				{
					buttonsEnabled = (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty())
				};
				if (!_003C_003E4__this._leaderboardsIsOpening)
				{
					_003C_003E4__this._leaderboardsButtons.Value.ForEach(delegate(UIButton b)
					{
						b.isEnabled = CS_0024_003C_003E8__locals0.buttonsEnabled;
					});
				}
				_003C_003E4__this._friendsButtons.Value.ForEach(delegate(UIButton b)
				{
					b.isEnabled = CS_0024_003C_003E8__locals0.buttonsEnabled;
				});
				_003C_003E4__this._clansButtons.Value.ForEach(delegate(UIButton b)
				{
					b.isEnabled = CS_0024_003C_003E8__locals0.buttonsEnabled;
				});
			}
			_003C_003E2__current = new WaitForSeconds(0.2f);
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

	[CompilerGenerated]
	internal sealed class _003CMoveToGameScene_003Ed__183 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private SceneInfo _003CscInfo_003E5__1;

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
		public _003CMoveToGameScene_003Ed__183(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			AsyncOperation asyncOperation;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (sharedController.InMiniGamesScreen)
				{
					Defs.isGameFromFriends = false;
					Defs.isGameFromClans = false;
				}
				else if (SceneLoader.ActiveSceneName.Equals("Clans"))
				{
					Defs.isGameFromFriends = false;
					Defs.isGameFromClans = true;
				}
				else
				{
					Defs.isGameFromFriends = true;
					Defs.isGameFromClans = false;
				}
				_003CscInfo_003E5__1 = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
				WeaponManager.sharedManager.Reset((int)((_003CscInfo_003E5__1 != null) ? _003CscInfo_003E5__1.AvaliableWeapon : ModeWeapon.all));
				UnityEngine.Debug.Log("MoveToGameScene");
				goto IL_00e3;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00e3;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00e3:
				if (PhotonNetwork.room == null)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
				PhotonNetwork.isMessageQueueRunning = false;
				UnityEngine.Debug.Log("map=" + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
				UnityEngine.Debug.Log(_003CscInfo_003E5__1.NameScene);
				LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _003CscInfo_003E5__1.NameScene) as Texture2D;
				LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
				LoadConnectScene.sceneToLoad = _003CscInfo_003E5__1.NameScene;
				LoadConnectScene.noteToShow = null;
				asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene");
				_003C_003E2__current = asyncOperation;
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

	[CompilerGenerated]
	internal sealed class _003CHideMenuInterfaceCoroutine_003Ed__188 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameObject nickLabelObj;

		public MainMenuController _003C_003E4__this;

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
		public _003CHideMenuInterfaceCoroutine_003Ed__188(int _003C_003E1__state)
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
				if (nickLabelObj != null)
				{
					nickLabelObj.SetActive(false);
				}
				_003C_003E4__this.rotateCamera.gameObject.SetActive(false);
				if (_003C_003E4__this.mainPanel != null)
				{
					_003C_003E4__this.mainPanel.transform.root.gameObject.SetActive(false);
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

	[CompilerGenerated]
	internal sealed class _003CShowRanks_003Ed__200 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003Ci_003E5__1;

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
		public _003CShowRanks_003Ed__200(int _003C_003E1__state)
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
				_003Ci_003E5__1 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__1++;
				break;
			}
			if (_003Ci_003E5__1 < 0)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
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
	internal sealed class _003COpenSettingPanelWithDelay_003Ed__204 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

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
		public _003COpenSettingPanelWithDelay_003Ed__204(int _003C_003E1__state)
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
				_003C_003E4__this.settingsPanel.SetActive(true);
				_003C_003E4__this.mainPanel.SetActive(false);
				if (AnimationGift.instance != null)
				{
					AnimationGift.instance.CheckVisibleGift();
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

	[CompilerGenerated]
	internal sealed class _003CContinueWithCoroutine_003Ed__223 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task task;

		public Action<Task> continuation;

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
		public _003CContinueWithCoroutine_003Ed__223(int _003C_003E1__state)
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
				if (task == null)
				{
					throw new ArgumentNullException("task");
				}
				if (continuation == null)
				{
					return false;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!task.IsCompleted)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			continuation(task);
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
	internal sealed class _003CHandleLeaderboardsClickedCoroutine_003Ed__227 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

		private LeaderboardsView _003Cview_003E5__1;

		public LeaderboardsView.State? toState;

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
		public _003CHandleLeaderboardsClickedCoroutine_003Ed__227(int _003C_003E1__state)
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
				_003C_003E4__this._leaderboardsIsOpening = true;
				_003C_003E4__this._leaderboardScript.Value.Show();
				if (_003C_003E4__this.mainPanel == null || _003C_003E4__this.LeaderboardsPanel == null || !_003C_003E4__this.mainPanel.activeInHierarchy || _003C_003E4__this.LeaderboardsPanel.gameObject.activeInHierarchy || _003C_003E4__this._leaderboardScript.Value == null)
				{
					_003C_003E4__this._leaderboardsIsOpening = false;
					return false;
				}
				Action<Task> continuation = delegate
				{
					_003C_003E4__this.LeaderboardsPanel.gameObject.SetActive(false);
					_003C_003E4__this.mainPanel.SetActive(true);
					if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty())
					{
						UIButton[] value2 = _003C_003E4__this._leaderboardsButtons.Value;
						for (int j = 0; j < value2.Length; j++)
						{
							value2[j].isEnabled = true;
						}
					}
				};
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.ContinueWithCoroutine(_003C_003E4__this._leaderboardScript.Value.GetReturnFuture(), continuation));
				_003C_003E4__this._leaderboardScript.Value.RefreshMyLeaderboardEntries();
				UIButton[] value = _003C_003E4__this._leaderboardsButtons.Value;
				for (int i = 0; i < value.Length; i++)
				{
					value[i].isEnabled = false;
				}
				_003C_003E4__this.LeaderboardsPanel.gameObject.SetActive(true);
				_003C_003E4__this.LeaderboardsPanel.alpha = float.Epsilon;
				_003Cview_003E5__1 = _003C_003E4__this.LeaderboardsPanel.Map((UIPanel p) => p.GetComponent<LeaderboardsView>());
				if (!(_003Cview_003E5__1 != null))
				{
					goto IL_0228;
				}
			}
			if (!_003Cview_003E5__1.Prepared)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (toState.HasValue)
			{
				_003Cview_003E5__1.CurrentState = toState.Value;
			}
			else
			{
				int @int = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
				LeaderboardsView.State state = (Enum.IsDefined(typeof(LeaderboardsView.State), @int) ? ((LeaderboardsView.State)@int) : LeaderboardsView.State.BestPlayers);
				_003Cview_003E5__1.CurrentState = ((state != 0) ? state : LeaderboardsView.State.BestPlayers);
			}
			goto IL_0228;
			IL_0228:
			_003C_003E4__this.mainPanel.SetActive(false);
			_003C_003E4__this.LeaderboardsPanel.alpha = 1f;
			if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty())
			{
				UIButton[] value = _003C_003E4__this._leaderboardsButtons.Value;
				for (int i = 0; i < value.Length; i++)
				{
					value[i].isEnabled = true;
				}
			}
			_003C_003E4__this._leaderboardsIsOpening = false;
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
	internal sealed class _003CShowMiniGames_003Ed__254 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenuController _003C_003E4__this;

		public bool isActive;

		public GameConnect.GameMode miniGame;

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
		public _003CShowMiniGames_003Ed__254(int _003C_003E1__state)
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
			_003C_003E4__this.InMiniGamesScreen = isActive;
			_003C_003E4__this.mainPanel.SetActive(!isActive);
			MainMenuHeroCamera.Instance.MainCamera.enabled = !isActive;
			NickLabelStack.sharedStack.gameObject.SetActive(!isActive);
			if (_003C_003E4__this.miniGamesPoint.childCount == 0)
			{
				Transform transform = UnityEngine.Object.Instantiate(Resources.Load<Transform>("PanelMiniGamesConnect"));
				transform.parent = _003C_003E4__this.miniGamesPoint;
				transform.localScale = Vector3.one;
				transform.localPosition = Vector3.zero;
				transform.GetComponent<ChooseMiniGameController>().onBackPressed = _003C_003E4__this.OnClickBackMiniGamesButton;
				_003C_003E4__this.connectionControl = transform.gameObject.AddComponent<ConnectionControl>();
				_003C_003E4__this.connectionControl.SetConnectPanel(_003C_003E4__this.connectPanel.GetComponent<ConnectPanel>());
			}
			_003C_003E4__this.miniGamesPoint.GetChild(0).GetComponent<ChooseMiniGameController>().SetEnabled(miniGame, isActive);
			FreeAwardShowHandler.CheckShowChest(isActive);
			ExperienceController.SetEnable(isActive && !_003C_003E4__this.stubLoading.activeSelf);
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

	public GameObject questButton;

	public GameObject facebookLoginContainer;

	public GameObject twitterLoginContainer;

	public GameObject facebookConnectedSettings;

	public GameObject facebookDisconnectedSettings;

	public GameObject facebookConnectSettings;

	public GameObject twitterConnectedSettings;

	public GameObject twitterDisconnectedSettings;

	public GameObject twitterConnectSettings;

	public static GameConnect.GameMode? NavigateToMinigame = null;

	[SerializeField]
	[Header("subwindows")]
	protected internal GameObject _subwindowsHandler;

	[SerializeField]
	protected internal PrefabHandler _socialBannerPrefab;

	private LazyObject<SocialGunBannerView> _socialBanner;

	[SerializeField]
	protected internal PrefabHandler _freePanelPrefab;

	private LazyObject<MainMenuFreePanel> _freePanel;

	[Header("MainMenuController properties")]
	public Transform topLeftAnchor;

	public GameObject buySmileButton;

	public UIButton premiumButton;

	public GameObject premium;

	public GameObject daysOfValor;

	public GameObject achievementsButton;

	public GameObject clansButton;

	public GameObject leadersButton;

	public UILabel battleNowLabel;

	public UILabel trainingNowLabel;

	public GameObject friendsGUI;

	public UILabel premiumTime;

	public GameObject premiumUpPlashka;

	public GameObject premiumbottomPlashka;

	public List<GameObject> premiumLevels = new List<GameObject>();

	public GameObject starParticleStarterPackGaemObject;

	public Transform RentExpiredPoint;

	private Transform _pers;

	public GameObject completeTraining;

	public GameObject stubLoading;

	public UITexture stubTexture;

	public MainMenuHeroCamera rotateCamera;

	public static MainMenuController sharedController;

	public static bool isShowConnectSceneOnStart = false;

	public GameObject multiplayerButton;

	public GameObject skinsMakerButton;

	public GameObject friendsButton;

	public GameObject profileButton;

	public GameObject secondProfileButton;

	public GameObject freeButton;

	public GameObject gameCenterButton;

	public GameObject shopButton;

	public GameObject settingsButton;

	public GameObject coinsShopButton;

	public GameObject diclineButton;

	public GameObject UserAgreementPanel;

	public UIButton signOutButton;

	public GameObject mainPanel;

	public GameObject newsIndicator;

	[Header("FeedBack")]
	[SerializeField]
	protected internal ButtonHandler _openFeedBackBtn;

	[SerializeField]
	protected internal PrefabHandler _feedbackPrefab;

	[Header("News")]
	[SerializeField]
	protected internal ButtonHandler _openNewsBtn;

	[SerializeField]
	protected internal PrefabHandler _newsPrefab;

	[Header("Leaderboards")]
	public UIPanel leaderboardsPanel;

	[Header("Misc")]
	public UIToggle notShowAgain;

	public UILabel coinsLabel;

	public GameObject award800to810;

	public UIButton awardOk;

	public GameObject bannerContainer;

	public GameObject nicknameLabel;

	public UIButton developerConsole;

	public UICamera uiCamera;

	public GameObject eventX3Window;

	public UIButton trafficForwardingButton;

	public ButtonHandler openCraftBtn;

	public static bool trafficForwardActive = false;

	private float _eventX3RemainTimeLastUpdateTime;

	private readonly Lazy<UISprite> _newClanIncomingInvitesSprite;

	private AdvertisementController _advertisementController;

	private ShopNGUIController _shopInstance;

	private bool isMultyPress;

	private bool isFriendsPress;

	private List<GameObject> saveOpenPanel = new List<GameObject>();

	public static bool canRotationLobbyPlayer = true;

	private GameObject connectPanel;

	private LazyObject<NewsLobbyController> _newsPanel;

	private LazyObject<FeedbackMenuController> _feedbackPanel;

	public ConnectionControl connectionControl;

	public GameObject connectScene;

	private readonly List<EventHandler> _backSubscribers = new List<EventHandler>();

	private bool loadReplaceAdmobPerelivRunning;

	private float _lastTimeInterstitialShown;

	private static bool _drawLoadingProgress = true;

	public static bool friendsOnStart;

	private static bool _socialNetworkingInitilized;

	private float _timePremiumTimeUpdated;

	private string lastPrintedDismissReason = string.Empty;

	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(() => FreeAwardController.Instance.TimeTamperingDetected());

	private IDisposable _backSubscription;

	private float lastTime;

	private float idleTimerLastTime;

	private float _bankEnteredTime;

	private GameObject _craftPanel;

	private MenuLeaderboardsController _menuLeaderboardsController;

	public UIPanel starterPackPanel;

	public UILabel starterPackTimer;

	public UILabel socialGunEventTimer;

	public UITexture buttonBackground;

	private bool _starterPackEnabled;

	internal const string TrafficForwardedKey = "TrafficForwarded";

	private string _trafficForwardingUrl = "http://pixelgun3d.com/";

	private bool _leaderboardsIsOpening;

	private readonly Lazy<UIButton[]> _leaderboardsButtons;

	private readonly Lazy<UIButton[]> _friendsButtons;

	private readonly Lazy<UIButton[]> _clansButtons;

	private readonly Lazy<LeaderboardScript> _leaderboardScript;

	private static bool m_firstEnterToLobby = true;

	private float timeEnteredLobby = float.MinValue;

	public List<int> shownMinigameModes = new List<int>();

	private const string SHOWN_OUR_LIKES_KEY = "MainMenuController.SHOWN_OUR_LIKES_KEY";

	private const string MINIGAME_SHOWN_MODES_KEY = "MainMenuController.MINIGAME_SHOWN_MODES_KEY";

	public UIWidget dayOfValorContainer;

	public UILabel dayOfValorTimer;

	private bool _dayOfValorEnabled;

	public Transform miniGamesPoint;

	[Header("Social panel settings")]
	public UIButton socialButton;

	public static bool trainingCompleted { get; set; }

	public bool FreePanelIsActive
	{
		get
		{
			return _freePanel.ObjectIsActive;
		}
	}

	private Transform pers
	{
		get
		{
			if (_pers == null && PersConfigurator.currentConfigurator != null)
			{
				_pers = PersConfigurator.currentConfigurator.transform;
			}
			return _pers;
		}
	}

	public Camera Camera3D
	{
		get
		{
			if (!(rotateCamera != null))
			{
				return null;
			}
			return rotateCamera.gameObject.GetComponentInChildren<Camera>("Main Camera", true);
		}
	}

	public static bool ShopOpened
	{
		get
		{
			if (sharedController != null)
			{
				return sharedController._shopInstance != null;
			}
			return false;
		}
	}

	public UIPanel LeaderboardsPanel
	{
		get
		{
			return _leaderboardScript.Value.Panel;
		}
	}

	public static Vector3 InitialRotation
	{
		get
		{
			return new Vector3(-0.33f, 138f, -0.28f);
		}
	}

	public static string RateUsURL
	{
		get
		{
			string result = Defs2.ApplicationUrl;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				result = "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return result;
		}
	}

	public bool LeaderboardsIsOpening
	{
		get
		{
			return _leaderboardsIsOpening;
		}
	}

	public bool InMiniGamesScreen { get; private set; }

	public static event Action onLoadMenu;

	public static event Action onEnableMenuForAskname;

	public static event Action<bool> onActiveMainMenu;

	public event EventHandler BackPressed
	{
		add
		{
			_backSubscribers.Add(value);
		}
		remove
		{
			_backSubscribers.Remove(value);
		}
	}

	private void Awake()
	{
		stubLoading.SetActive(true);
		if (TrainingController.TrainingCompleted)
		{
			PetsManager.LoadPetsToMemory();
		}
		_socialBanner = new LazyObject<SocialGunBannerView>(_socialBannerPrefab.ResourcePath, _subwindowsHandler);
		_freePanel = new LazyObject<MainMenuFreePanel>(_freePanelPrefab.ResourcePath, _subwindowsHandler);
		_newsPanel = new LazyObject<NewsLobbyController>(_newsPrefab.ResourcePath, _subwindowsHandler);
		_feedbackPanel = new LazyObject<FeedbackMenuController>(_feedbackPrefab.ResourcePath, _subwindowsHandler);
		GameObject original = Resources.Load("ConnectPanel") as GameObject;
		connectPanel = UnityEngine.Object.Instantiate(original);
		connectPanel.transform.SetParent(uiCamera.transform, false);
		connectPanel.SetActive(false);
		InitLobbyCraftController();
		try
		{
			if (!Storager.hasKey("MainMenuController.MINIGAME_SHOWN_MODES_KEY"))
			{
				Storager.setString("MainMenuController.MINIGAME_SHOWN_MODES_KEY", "[]");
			}
			shownMinigameModes = GetShownMiniGamesFromDisk();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in load minigame news: {0}", ex);
		}
		Storager.getInt("MainMenuController.SHOWN_OUR_LIKES_KEY");
		timeEnteredLobby = Time.realtimeSinceStartup;
	}

	private static List<int> GetShownMiniGamesFromDisk()
	{
		return (Json.Deserialize(Storager.getString("MainMenuController.MINIGAME_SHOWN_MODES_KEY")) as List<object>).Select((object o) => Convert.ToInt32(o)).ToList();
	}

	public void SaveShowPanelAndClose()
	{
		if (!(mainPanel != null))
		{
			return;
		}
		saveOpenPanel.Clear();
		for (int i = 0; i < mainPanel.transform.childCount; i++)
		{
			GameObject gameObject = mainPanel.transform.GetChild(i).gameObject;
			if (!(gameObject.GetComponent<UICamera>() != null) && gameObject.activeSelf)
			{
				saveOpenPanel.Add(gameObject);
				gameObject.SetActive(false);
			}
		}
	}

	public void ShowSavePanel(bool needClear = true)
	{
		for (int i = 0; i < saveOpenPanel.Count; i++)
		{
			GameObject gameObject = saveOpenPanel[i];
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		if (needClear)
		{
			saveOpenPanel.Clear();
		}
	}

	private void InvokeLastBackHandler()
	{
		if (_backSubscribers.Count != 0)
		{
			_backSubscribers[_backSubscribers.Count - 1].Do(delegate(EventHandler lastHandler)
			{
				lastHandler(this, EventArgs.Empty);
			});
		}
	}

	public MainMenuController()
	{
		_newClanIncomingInvitesSprite = new Lazy<UISprite>(() => clansButton.Map((GameObject c) => c.GetComponentsInChildren<UISprite>(true).FirstOrDefault((UISprite s) => "NewMessages".Equals(s.name))));
		_leaderboardsButtons = new Lazy<UIButton[]>(() => leadersButton.GetComponentsInChildren<UIButton>(true));
		_friendsButtons = new Lazy<UIButton[]>(() => friendsButton.GetComponentsInChildren<UIButton>(true));
		_clansButtons = new Lazy<UIButton[]>(() => clansButton.GetComponentsInChildren<UIButton>(true));
		_leaderboardScript = new Lazy<LeaderboardScript>(UnityEngine.Object.FindObjectOfType<LeaderboardScript>);
	}

	public static bool IsLevelUpOrBannerShown()
	{
		bool num = ExperienceController.sharedController != null && ExperienceController.sharedController.isShowNextPlashka;
		bool flag = BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown;
		return num || flag;
	}

	public static bool ShowBannerOrLevelup()
	{
		if (!IsLevelUpOrBannerShown() && !FriendsWindowGUI.Instance.InterfaceEnabled && !MainMenu.BlockInterface)
		{
			return Defs.isShowUserAgrement;
		}
		return true;
	}

	public static void DoMemoryConsumingTaskInEmptyScene(Action action, Action onSeparateSceneCaseAction = null)
	{
		if (Device.IsLoweMemoryDevice)
		{
			CleanUpAndDoAction.action = onSeparateSceneCaseAction ?? action;
			SceneManager.LoadScene("LoadAnotherApp");
		}
		else if (action != null)
		{
			action();
		}
	}

	public void HandleFacebookLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (FB.IsLoggedIn)
		{
			FB.LogOut();
			return;
		}
		DoMemoryConsumingTaskInEmptyScene(delegate
		{
			FacebookController.Login(null, null, "Options");
		});
	}

	public void HandleTwitterLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (TwitterController.IsLoggedIn && TwitterController.Instance != null)
		{
			TwitterController.Instance.Logout();
			return;
		}
		DoMemoryConsumingTaskInEmptyScene(delegate
		{
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.Login(null, null, "Options");
			}
		});
	}

	private IEnumerator OnApplicationPause(bool pausing)
	{
		if (pausing)
		{
			yield break;
		}
		yield return new WaitForSeconds(1f);
		string text = string.Empty;
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
			{
				MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
				text = "`SuppressShowOnReturnFromPause`";
			}
			else if (ReplaceAdmobPerelivController.sharedController == null)
			{
				text = "`ReplaceAdmobPerelivController.sharedController == null`";
			}
			else if (!FreeAwardController.FreeAwardChestIsInIdleState)
			{
				text = "`FreeAwardChestIsInIdleState == false`";
			}
			else
			{
				text = ConnectScene.GetReasonToDismissFakeInterstitial();
				if (string.IsNullOrEmpty(text))
				{
					ReplaceAdmobPerelivController.IncreaseTimesCounter();
					StartCoroutine(LoadAndShowReplaceAdmobPereliv("On return from pause to Lobby"));
				}
			}
		}
		if (!string.IsNullOrEmpty(text) && Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing fake interstitial. {0}</color>" : "Dismissing fake interstitial. {0}", text);
		}
		ReloadFacebookFriends();
	}

	private IEnumerator LoadAndShowReplaceAdmobPereliv(string context)
	{
		if (loadReplaceAdmobPerelivRunning)
		{
			yield break;
		}
		try
		{
			loadReplaceAdmobPerelivRunning = true;
			if (!ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
			{
				ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
			}
			while (!ReplaceAdmobPerelivController.sharedController.DataLoaded)
			{
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
				{
					loadReplaceAdmobPerelivRunning = false;
					yield break;
				}
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			if (FreeAwardController.FreeAwardChestIsInIdleState && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown))
			{
				ReplaceAdmobPerelivController.TryShowPereliv(context);
				ReplaceAdmobPerelivController.sharedController.DestroyImage();
			}
		}
		finally
		{
			loadReplaceAdmobPerelivRunning = false;
		}
	}

	public void OnSocialGunEventButtonClick()
	{
		if (!_leaderboardsIsOpening && !(SkinEditorController.sharedController != null) && !(BannerWindowController.SharedController == null))
		{
			_socialBanner.Value.Show();
		}
	}

	private void OnDestroy()
	{
		if (NickLabelStack.sharedStack != null && NickLabelStack.sharedStack.gameObject != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(true);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.GetComponent<TrafficForwardingScript>().Do(delegate(TrafficForwardingScript tf)
			{
				tf.Updated = (EventHandler<TrafficForwardingInfo>)Delegate.Remove(tf.Updated, new EventHandler<TrafficForwardingInfo>(RefreshTrafficForwardingButton));
			});
		}
		SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult -= HandleSocialGunViewLoginCompleted;
		StarterPackController.OnStarterPackEnable -= OnStarterPackContainerShow;
		PromoActionsManager.OnDayOfValorEnable -= OnDayOfValorContainerShow;
		PromoActionClick.Click -= HandlePromoActionClicked;
		SettingsController.ControlsClicked -= base.HandleControlsClicked;
		sharedController = null;
		if (FreeAwardController.Instance != null)
		{
			FreeAwardController.Instance.transform.root.Map((Transform t) => t.gameObject).Do(UnityEngine.Object.Destroy);
		}
	}

	private void OnGUI()
	{
		if (!Launcher.UsingNewLauncher && _drawLoadingProgress)
		{
			ActivityIndicator.LoadingProgress = 1f;
		}
	}

	private void InitializeBannerWindow()
	{
		_advertisementController = base.gameObject.GetComponent<AdvertisementController>();
		if (_advertisementController == null)
		{
			_advertisementController = base.gameObject.AddComponent<AdvertisementController>();
		}
		BannerWindowController.SharedController.advertiseController = _advertisementController;
	}

	private void CheckIfPendingAward()
	{
		DateTime result;
		if (Storager.hasKey("Ads.PendingRewardTimestamp") && DateTime.TryParse(Storager.getString("Ads.PendingRewardTimestamp"), out result))
		{
			FreeAwardController.Instance.GiveAwardAndIncrementCount(result);
			Storager.setInt("PendingInterstitial", 0);
		}
		if (Storager.hasKey("PendingInterstitial") && Storager.getInt("PendingInterstitial") > 0)
		{
			Storager.setInt("PendingInterstitial", 0);
		}
	}

	private static void ReloadFacebookFriends()
	{
		if (FacebookController.FacebookSupported && FacebookController.sharedController != null && FB.IsLoggedIn)
		{
			FacebookController.sharedController.InputFacebookFriends(null, true);
		}
	}

	public void RefreshSettingsButton()
	{
		if (!(settingsButton == null))
		{
			ButtonHandler component = settingsButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleSettingsClicked;
			}
			UIButton component2 = settingsButton.GetComponent<UIButton>();
			if (component2 != null)
			{
				component2.isEnabled = TrainingController.TrainingCompleted;
			}
		}
	}

	private new IEnumerator Start()
	{
		LoadConnectSceneCoroutine();
		FriendsController.sharedController.timerSendTimeGame = 0f;
		if (Defs.IsDeveloperBuild && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1 && !TrainingController.TrainingCompleted)
		{
			TrainingController.OnGetProgress();
		}
		if (!TrainingController.TrainingCompleted)
		{
			if (CloudSyncController.AreProgressInCurrentPullResult())
			{
				TrainingController.OnGetProgress();
				CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(false, true));
			}
			else if (m_firstEnterToLobby && !CloudSyncController.Instance.IsAuthenticatedToCloud())
			{
				CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_NotAuthenticated());
			}
		}
		else if (TrainingController.ShouldSyncInLobbyAfterSkippingRancho)
		{
			CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(false, true));
			TrainingController.ShouldSyncInLobbyAfterSkippingRancho = false;
		}
		else if (m_firstEnterToLobby && !CloudSyncController.Instance.IsAuthenticatedToCloud())
		{
			CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_NotAuthenticated_SecondLaunch());
		}
		else if (!CloudSyncController.IsSynchronizingWithCloud)
		{
			CoroutineRunner.Instance.StartCoroutine(CloudSyncController.Instance.PullAndApplyChangesIfNeeded());
		}
		m_firstEnterToLobby = false;
		UpdateInappBonusChestActiveState();
		LogsManager.DisableLogsIfAllowed();
		FreeTicketsController.Instance.SaveState();
		if (Storager.hasKey("Analytics:af_tutorial_completion"))
		{
			AnalyticsStuff.TrySendOnceToAppsFlyer("tutorial_lobby");
		}
		string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
		string text = FilterBadWorld.FilterString(playerNameOrDefault);
		if (string.IsNullOrEmpty(text) || text.Trim() == string.Empty)
		{
			text = ProfileController.defaultPlayerName;
		}
		if (text != playerNameOrDefault)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("Saving new name:    " + text);
			}
			PlayerPrefs.SetString("NamePlayer", text);
		}
		Storager.setInt(Defs.ShownLobbyLevelSN, 3);
		transform.GetChild(0).GetComponent<UICamera>().allowMultiTouch = false;
		SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult += HandleSocialGunViewLoginCompleted;
		TwitterController.CheckAndGiveTwitterReward("Start");
		FacebookController.CheckAndGiveFacebookReward("Start");
		ReloadFacebookFriends();
		if (FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			ClanIncomingInvitesController.FetchClanIncomingInvites(FriendsController.sharedController.id);
		}
		CheckIfPendingAward();
		if (socialButton != null)
		{
			socialButton.gameObject.SetActive(true);
			socialButton.GetComponent<ButtonHandler>().Clicked += HandleSocialButton;
		}
		ExperienceController.RefreshExpControllers();
		PlayerPrefs.SetInt("CountRunMenu", PlayerPrefs.GetInt("CountRunMenu", 0) + 1);
		premiumTime.gameObject.SetActive(true);
		InitializeBannerWindow();
		bool isDebugBuild = UnityEngine.Debug.isDebugBuild;
		if (developerConsole != null)
		{
			developerConsole.gameObject.SetActive(isDebugBuild);
		}
		starterPackPanel.gameObject.SetActive(false);
		TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
		if (trafficForwardingScript != null)
		{
			trafficForwardingScript.Updated = (EventHandler<TrafficForwardingInfo>)Delegate.Combine(trafficForwardingScript.Updated, new EventHandler<TrafficForwardingInfo>(RefreshTrafficForwardingButton));
			Task<TrafficForwardingInfo> val = trafficForwardingScript.GetTrafficForwardingInfo().Filter((Task<TrafficForwardingInfo> t) => ((Task)t).IsCompleted && !((Task)t).IsCanceled && !((Task)t).IsFaulted);
			if (val != null)
			{
				_trafficForwardingUrl = val.Result.Url;
			}
			if (trafficForwardingButton != null)
			{
				RefreshTrafficForwardingButton(this, (val != null) ? val.Result : TrafficForwardingInfo.DisabledInstance);
			}
		}
		dayOfValorContainer.gameObject.SetActive(false);
		stubTexture.mainTexture = ConnectScene.MainLoadingTexture();
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		Start();
		idleTimerLastTime = Time.realtimeSinceStartup + 10000000f;
		SettingsController.ControlsClicked += base.HandleControlsClicked;
		Defs.isShowUserAgrement = false;
		completeTraining.SetActive(!shopButton.GetComponent<UIButton>().isEnabled);
		mainPanel.SetActive(true);
		settingsPanel.SetActive(false);
		if (_newsPanel.ObjectIsActive)
		{
			_newsPanel.Value.gameObject.SetActive(false);
		}
		if (_freePanel.ObjectIsActive)
		{
			_freePanel.Value.SetVisible(false);
		}
		SettingsJoysticksPanel.SetActive(false);
		sharedController = this;
		if (multiplayerButton != null)
		{
			ButtonHandler component = multiplayerButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleMultiPlayerClicked;
			}
		}
		if (skinsMakerButton != null)
		{
			if (MainMenu.SkinsMakerSupproted())
			{
				ButtonHandler component2 = skinsMakerButton.GetComponent<ButtonHandler>();
				if (component2 != null)
				{
					component2.Clicked += HandleSkinsMakerClicked;
				}
			}
			else
			{
				skinsMakerButton.SetActive(false);
			}
		}
		if (profileButton != null)
		{
			ButtonHandler component3 = profileButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += HandleProfileClicked;
			}
		}
		if (secondProfileButton != null)
		{
			ButtonHandler component4 = secondProfileButton.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += HandleProfileClicked;
			}
		}
		if (freeButton != null)
		{
			ButtonHandler component5 = freeButton.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += HandleFreeClicked;
			}
		}
		if (shopButton != null)
		{
			ButtonHandler component6 = shopButton.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += HandleShopClicked;
			}
		}
		if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
		{
			HandleShopClicked(null, null);
			PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
			PlayerPrefs.Save();
		}
		RefreshSettingsButton();
		if (_openFeedBackBtn != null)
		{
			_openFeedBackBtn.Clicked += HandleSupportButtonClicked;
		}
		if (friendsButton != null)
		{
			ButtonHandler component7 = friendsButton.GetComponent<ButtonHandler>();
			if (component7 != null)
			{
				component7.Clicked += HandleFriendsClicked;
			}
		}
		if (_openNewsBtn != null)
		{
			_openNewsBtn.Clicked += HandleNewsButtonClicked;
		}
		if (diclineButton != null)
		{
			ButtonHandler component8 = diclineButton.GetComponent<ButtonHandler>();
			if (component8 != null)
			{
				component8.Clicked += HandleDiclineClicked;
			}
		}
		if (openCraftBtn != null)
		{
			ButtonHandler component9 = openCraftBtn.GetComponent<ButtonHandler>();
			if (component9 != null)
			{
				component9.Clicked += HandleCraftButtonClicked;
			}
		}
		if (BankController.Instance != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(BankController.Instance.transform.root.gameObject);
		}
		else
		{
			UnityEngine.Debug.LogWarning("bankController == null");
		}
		if (NavigateToMinigame.HasValue)
		{
			OpenMiniGames(NavigateToMinigame.Value);
			NavigateToMinigame = null;
			ConnectScene.isReturnFromGame = false;
			isShowConnectSceneOnStart = false;
		}
		yield return new WaitForSeconds(0.5f);
		PromoActionClick.Click += HandlePromoActionClicked;
		yield return new WaitForSeconds(0.5f);
		if (friendsOnStart)
		{
			HandleFriendsClicked(null, null);
			rotateCamera.gameObject.SetActive(false);
			yield return null;
		}
		while ((ConnectScene.isReturnFromGame || isShowConnectSceneOnStart) && connectScene == null)
		{
			yield return null;
		}
		_drawLoadingProgress = false;
		stubLoading.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
		if (!ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		BannerWindowController.SharedController.ResetScene();
		UnityEngine.Debug.Log("Start initializing ProfileGui.");
		if (UnityEngine.Object.FindObjectOfType<ProfileController>() == null)
		{
			GameObject profileGuiRequest = Resources.Load<GameObject>("ProfileGui");
			yield return profileGuiRequest;
			UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(profileGuiRequest));
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Training completed: {0}. Authenticating...", TrainingController.TrainingCompleted);
		}
		if (!_socialNetworkingInitilized)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Social networking is not initialized.");
			}
			GameServicesController gameServicesController = UnityEngine.Object.FindObjectOfType<GameServicesController>();
			if (gameServicesController == null)
			{
				GameObject gameObject = new GameObject("Rilisoft.GameServicesController");
				gameServicesController = gameObject.AddComponent<GameServicesController>();
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterSingleton instance = GameCenterSingleton.Instance;
				yield return null;
			}
			_socialNetworkingInitilized = true;
			gameServicesController.WaitAuthenticationAndIncrementBeginnerAchievement();
		}
		if (bannerContainer != null)
		{
			InGameGUI.SetLayerRecursively(bannerContainer, LayerMask.NameToLayer("Banners"));
		}
		StarterPackController.OnStarterPackEnable += OnStarterPackContainerShow;
		OnStarterPackContainerShow(StarterPackController.Get.isEventActive);
		PromoActionsManager.OnDayOfValorEnable += OnDayOfValorContainerShow;
		OnDayOfValorContainerShow(PromoActionsManager.sharedManager.IsDayOfValorEventActive);
		if (ReplaceAdmobPerelivController.sharedController != null && ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby && ReplaceAdmobPerelivController.sharedController.DataLoaded)
		{
			ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby = false;
			ReplaceAdmobPerelivController.TryShowPereliv("Lobby after launch");
			ReplaceAdmobPerelivController.sharedController.DestroyImage();
		}
		string abuseKey_f1a4329e = GetAbuseKey_f1a4329e(4054069918u);
		if (Storager.hasKey(abuseKey_f1a4329e))
		{
			string @string = Storager.getString(abuseKey_f1a4329e);
			if (!string.IsNullOrEmpty(@string) && @string != "0")
			{
				long num = DateTime.UtcNow.Ticks >> 1;
				long result = num;
				if (long.TryParse(@string, out result))
				{
					result = Math.Min(num, result);
					Storager.setString(abuseKey_f1a4329e, result.ToString());
				}
				else
				{
					Storager.setString(abuseKey_f1a4329e, num.ToString());
				}
				TimeSpan timeSpan = TimeSpan.FromTicks(num - result);
			}
		}
		StartCoroutine(TryToShowExpiredBanner());
		StopCoroutine("TryToShowMyLobbyLiked");
		StartCoroutine("TryToShowMyLobbyLiked");
		if (friendsOnStart)
		{
			if (mainPanel != null)
			{
				mainPanel.transform.root.gameObject.SetActive(false);
			}
			friendsOnStart = false;
		}
		newsIndicator.SetActive(PlayerPrefs.GetInt("LobbyIsAnyNewsKey", 0) == 1);
		if (MainMenuController.onLoadMenu != null)
		{
			MainMenuController.onLoadMenu();
		}
		if (MainMenuController.onEnableMenuForAskname != null)
		{
			MainMenuController.onEnableMenuForAskname();
		}
		QuestSystem.Instance.QuestCompleted -= OnCompletedQuest;
		QuestSystem.Instance.QuestCompleted += OnCompletedQuest;
		InitLobbyCraftController();
	}

	private void InitLobbyCraftController()
	{
		if (!(LobbyCraftController.Instance == null))
		{
			return;
		}
		UnityEngine.Object @object = Resources.Load("NguiWindows/CraftGUI");
		if (!(@object == null))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(mainPanel.transform.root);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				int siblingIndex = mainPanel.gameObject.transform.GetSiblingIndex();
				gameObject.transform.SetSiblingIndex(siblingIndex + 1);
			}
		}
	}

	private static void OnCompletedQuest(object sender, QuestCompletedEventArgs e)
	{
		AccumulativeQuestBase accumulativeQuestBase = e.Quest as AccumulativeQuestBase;
		if (accumulativeQuestBase != null)
		{
			InfoWindowController.ShowQuestBox(string.Empty, QuestConstants.GetAccumulativeQuestDescriptionByType(accumulativeQuestBase));
		}
	}

	private void HandleSocialGunViewLoginCompleted(bool success)
	{
		if (!(mainPanel == null))
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/" + (success ? "PanelAuthSucces" : "PanelAuthFailed")));
			obj.transform.parent = (_freePanel.ObjectIsActive ? _freePanel.Value.gameObject.transform : mainPanel.transform);
			Player_move_c.SetLayerRecursively(obj, mainPanel.layer);
			obj.transform.localPosition = new Vector3(0f, 0f, -130f);
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void HandleClansClicked()
	{
		if (_shopInstance != null || ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		((Action)delegate
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				GoClans();
			}
		})();
	}

	private void GoClans()
	{
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName);
	}

	private static string GetAbuseKey_f1a4329e(uint pad)
	{
		return (0x354E43A7u ^ pad).ToString("x");
	}

	public static bool IsShowRentExpiredPoint()
	{
		if (sharedController == null)
		{
			return false;
		}
		Transform rentExpiredPoint = sharedController.RentExpiredPoint;
		if (rentExpiredPoint == null)
		{
			return false;
		}
		return rentExpiredPoint.childCount > 0;
	}

	public static bool SavedShwonLobbyLevelIsLessThanActual()
	{
		return Storager.getInt(Defs.ShownLobbyLevelSN) < ExpController.LobbyLevel;
	}

	public bool IsSomeWindowOpenExceptSettings()
	{
		if (!ShopNGUIController.GuiActive && (!(FreeAwardController.Instance != null) || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>()) && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown) && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !_freePanel.ObjectIsActive && !_feedbackPanel.ObjectIsActive && (!(ProfileController.Instance != null) || !ProfileController.Instance.InterfaceEnabled) && (!(FriendsWindowGUI.Instance != null) || !FriendsWindowGUI.Instance.InterfaceEnabled) && !stubLoading.activeInHierarchy && !InMiniGamesScreen && !UserAgreementPanel.activeInHierarchy && !SettingsJoysticksPanel.activeInHierarchy && (!(LeaderboardsPanel != null) || !LeaderboardsPanel.gameObject.activeInHierarchy))
		{
			return ExchangeWindow.IsOpened;
		}
		return true;
	}

	private IEnumerator TryToShowMyLobbyLiked()
	{
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - timeEnteredLobby < 3f)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (IsSomeWindowOpenExceptSettings() || settingsPanel.activeInHierarchy || ConnectScene.isEnable || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || RentExpiredPoint.childCount != 0)
				{
					continue;
				}
				int num = FriendsController.sharedController.OurLobbyLikes.Likes - Storager.getInt("MainMenuController.SHOWN_OUR_LIKES_KEY");
				if (num != 0)
				{
					if (num > 0)
					{
						InfoWindowController.ShowAchievementsBox((num == 1 && !FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked.IsNullOrEmpty()) ? string.Format(LocalizationStore.Get("Key_3298"), new object[1] { FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked }) : string.Format(LocalizationStore.Get("Key_3299"), new object[1] { num }), null, "", true);
						FriendsController.sharedController.OurLobbyLikes.LastPlayerThatLiked = string.Empty;
					}
					Storager.setInt("MainMenuController.SHOWN_OUR_LIKES_KEY", FriendsController.sharedController.OurLobbyLikes.Likes);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("exception in Lobby  TryToShowMyLobbyLiked: " + ex);
			}
		}
	}

	private IEnumerator TryToShowExpiredBanner()
	{
		while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (!IsSomeWindowOpenExceptSettings() && !settingsPanel.activeInHierarchy && (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && RentExpiredPoint.childCount == 0)
				{
					if (SavedShwonLobbyLevelIsLessThanActual())
					{
						GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LobbyLevels/LobbyLevelTips_" + (Storager.getInt(Defs.ShownLobbyLevelSN) + 1)));
						obj.transform.parent = RentExpiredPoint;
						Player_move_c.SetLayerRecursively(obj, LayerMask.NameToLayer("NGUI"));
						obj.transform.localPosition = new Vector3(0f, 0f, -130f);
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = new Vector3(1f, 1f, 1f);
					}
					else if (Storager.getInt(Defs.PremiumEnabledFromServer) == 1)
					{
						ShopNGUIController.ShowPremimAccountExpiredIfPossible(RentExpiredPoint, "NGUI");
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("exception in Lobby  TryToShowExpiredBanner: " + ex);
			}
		}
	}

	public void HandleDeveloperConsoleClicked()
	{
	}

	public void HandlePromoActionClicked(string tg)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (tg != null && tg == "StickersPromoActionsPanelKey")
		{
			ButtonClickSound.Instance.PlayClick();
			BuySmileBannerController.openedFromPromoActions = true;
			return;
		}
		int num = -1;
		if (ShopNGUIController.sharedShop != null)
		{
			try
			{
				num = ItemDb.GetItemCategory(tg);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in getting category of promo action item on click: " + ex);
			}
			if (num != -1)
			{
				ShopNGUIController.sharedShop.CategoryToChoose = (ShopNGUIController.CategoryNames)num;
			}
			ShopNGUIController.sharedShop.SetItemToShow(new ShopNGUIController.ShopItem(tg, (ShopNGUIController.CategoryNames)num));
			ShopNGUIController.sharedShop.IsInShopFromPromoPanel(true, tg);
		}
		if (num != -1)
		{
			HandleShopClicked(null, EventArgs.Empty);
		}
	}

	public bool PromoOffersPanelShouldBeShown()
	{
		if (_shopInstance == null)
		{
			return !ShowBannerOrLevelup();
		}
		return false;
	}

	private void Update()
	{
		UpdateInappBonusChestActiveState();
		if (InMiniGamesScreen && !stubLoading.activeSelf && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (settingsPanel.activeInHierarchy)
		{
			if (facebookConnectedSettings.activeSelf != (FacebookController.FacebookSupported && FB.IsLoggedIn))
			{
				facebookConnectedSettings.SetActive(FacebookController.FacebookSupported && FB.IsLoggedIn);
			}
			if (facebookDisconnectedSettings.activeSelf != (FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 1))
			{
				facebookDisconnectedSettings.SetActive(FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 1);
			}
			if (facebookConnectSettings.activeSelf != (FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0))
			{
				facebookConnectSettings.SetActive(FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0);
			}
			if (twitterConnectedSettings.activeSelf != (TwitterController.TwitterSupported && TwitterController.IsLoggedIn))
			{
				twitterConnectedSettings.SetActive(TwitterController.TwitterSupported && TwitterController.IsLoggedIn);
			}
			if (twitterDisconnectedSettings.activeSelf != (TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 1))
			{
				twitterDisconnectedSettings.SetActive(TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 1);
			}
			if (twitterConnectSettings.activeSelf != (TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 0))
			{
				twitterConnectSettings.SetActive(TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 0);
			}
			if (facebookLoginContainer != null)
			{
				facebookLoginContainer.SetActive(FacebookController.FacebookSupported);
			}
			if (twitterLoginContainer != null)
			{
				twitterLoginContainer.SetActive(TwitterController.TwitterSupported);
			}
		}
		bool active = (Storager.getInt(Defs.PremiumEnabledFromServer) == 1 || PremiumAccountController.Instance.isAccountActive) && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel >= 3;
		premium.SetActive(active);
		premiumButton.isEnabled = Storager.getInt(Defs.PremiumEnabledFromServer) == 1;
		if (premiumUpPlashka.activeSelf != (!(PremiumAccountController.Instance != null) || !PremiumAccountController.Instance.isAccountActive))
		{
			premiumUpPlashka.SetActive(!(PremiumAccountController.Instance != null) || !PremiumAccountController.Instance.isAccountActive);
		}
		if (premiumbottomPlashka.activeSelf != (PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive))
		{
			premiumbottomPlashka.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
		}
		if (PremiumAccountController.Instance != null)
		{
			long num = PremiumAccountController.Instance.GetDaysToEndAllAccounts();
			for (int i = 0; i < premiumLevels.Count; i++)
			{
				bool flag = false;
				if (num > 0 && num < 3 && i == 0)
				{
					flag = true;
				}
				if (num >= 3 && num < 7 && i == 1)
				{
					flag = true;
				}
				if (num >= 7 && num < 30 && i == 2)
				{
					flag = true;
				}
				if (num >= 30 && i == 3)
				{
					flag = true;
				}
				if (premiumLevels[i].activeSelf != flag)
				{
					premiumLevels[i].SetActive(flag);
				}
			}
			if (Time.realtimeSinceStartup - _timePremiumTimeUpdated >= 1f)
			{
				premiumTime.text = PremiumAccountController.Instance.GetTimeToEndAllAccounts();
				_timePremiumTimeUpdated = Time.realtimeSinceStartup;
			}
		}
		bool flag2 = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive;
		if (starParticleStarterPackGaemObject != null && starParticleStarterPackGaemObject.activeInHierarchy != flag2)
		{
			starParticleStarterPackGaemObject.SetActive(flag2);
		}
		if (Time.realtimeSinceStartup - _eventX3RemainTimeLastUpdateTime >= 0.5f)
		{
			_eventX3RemainTimeLastUpdateTime = Time.realtimeSinceStartup;
			if (_dayOfValorEnabled)
			{
				dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
			}
		}
		if (_isCancellationRequested)
		{
			MainMenuController mainMenuController = sharedController;
			if (SettingsJoysticksPanel.activeSelf)
			{
				SettingsJoysticksPanel.SetActive(false);
				settingsPanel.SetActive(true);
			}
			else if (_freePanel.ObjectIsActive)
			{
				if (_shopInstance == null && !ShowBannerOrLevelup())
				{
					mainPanel.SetActive(true);
					if (_freePanel.ObjectIsLoaded)
					{
						_freePanel.Value.SetVisible(false);
					}
					rotateCamera.PlayCloseOptions();
					AnimationGift.instance.CheckVisibleGift();
				}
			}
			else if (_newsPanel.ObjectIsActive)
			{
				_newsPanel.DestroyValue();
				mainPanel.SetActive(true);
			}
			else if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
			else if ((!(settingsPanel != null) || !settingsPanel.activeInHierarchy) && (!_freePanel.ObjectIsLoaded || !_freePanel.Value.gameObject.activeInHierarchy) && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive && (!(ProfileController.Instance != null) || !ProfileController.Instance.InterfaceEnabled))
			{
				if (PremiumAccountScreenController.Instance != null)
				{
					PremiumAccountScreenController.Instance.Hide();
				}
				else if (mainMenuController != null && mainMenuController.InMiniGamesScreen)
				{
					mainMenuController.OnClickBackMiniGamesButton();
				}
				else
				{
					PlayerPrefs.Save();
					Application.Quit();
				}
			}
			_isCancellationRequested = false;
		}
		if (rotateCamera != null && !rotateCamera.IsAnimPlaying)
		{
			RotateCharacter(() => canRotationLobbyPlayer && !ShopNGUIController.GuiActive && !SettingsJoysticksPanel.activeInHierarchy);
		}
		if (Time.realtimeSinceStartup - idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			ReturnPersTonNormState();
		}
		if (_starterPackEnabled)
		{
			starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
		}
		RefreshChestButton();
		if (_newClanIncomingInvitesSprite.Value != null)
		{
			if (ClanIncomingInvitesController.CurrentRequest == null || !((Task)ClanIncomingInvitesController.CurrentRequest).IsCompleted)
			{
				_newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else if (((Task)ClanIncomingInvitesController.CurrentRequest).IsCanceled || ((Task)ClanIncomingInvitesController.CurrentRequest).IsFaulted)
			{
				_newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else
			{
				_newClanIncomingInvitesSprite.Value.gameObject.SetActive(ClanIncomingInvitesController.CurrentRequest.Result.Count > 0);
			}
		}
	}

	public void RotateCharacter(Func<bool> canProcess)
	{
		RilisoftRotator.RotateCharacter(pers, RilisoftRotator.RotationRateForCharacterInMenues, TouchZoneForRotation(), ref idleTimerLastTime, ref lastTime, canProcess);
	}

	private Rect TouchZoneForRotation()
	{
		return settingsPanel.activeInHierarchy ? new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height) : ((!(MenuLeaderboardsController.sharedController != null) || !MenuLeaderboardsController.sharedController.IsOpened) ? new Rect(0.2f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height) : new Rect(0.38f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height));
	}

	private IEnumerator SetSocialButtonsState()
	{
		while (true)
		{
			if (TrainingController.TrainingCompleted)
			{
				bool buttonsEnabled = FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty();
				if (!_leaderboardsIsOpening)
				{
					_leaderboardsButtons.Value.ForEach(delegate(UIButton b)
					{
						b.isEnabled = buttonsEnabled;
					});
				}
				_friendsButtons.Value.ForEach(delegate(UIButton b)
				{
					b.isEnabled = buttonsEnabled;
				});
				_clansButtons.Value.ForEach(delegate(UIButton b)
				{
					b.isEnabled = buttonsEnabled;
				});
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void RefreshChestButton()
	{
	}

	private void HandleEscape()
	{
		if (_backSubscribers.Count > 0)
		{
			InvokeLastBackHandler();
		}
		else
		{
			_isCancellationRequested = true;
		}
	}

	private void ReturnPersTonNormState()
	{
		HOTween.Kill(pers);
		idleTimerLastTime += 1000000f;
		HOTween.To(pers, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(InitialRotation)).Ease(EaseType.Linear).OnComplete((TweenDelegate.TweenCallback)delegate
		{
			idleTimerLastTime += 1000000f;
		}));
	}

	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	private new void OnEnable()
	{
		base.OnEnable();
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Main Menu Controller");
		RewardedLikeButton[] componentsInChildren = GetComponentsInChildren<RewardedLikeButton>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Refresh();
		}
		if (ExperienceController.sharedController != null && !stubLoading.activeSelf && !ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.InterfaceEnabled = true;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(true);
		}
		if (MainMenuController.onEnableMenuForAskname != null)
		{
			MainMenuController.onEnableMenuForAskname();
		}
		StartCoroutine(SetSocialButtonsState());
		if (!TrainingController.TrainingCompleted && HintController.instance != null)
		{
			AskNameManager.onComplete += HintController.instance.ShowCurrentHintObjectLabel;
		}
		StopCoroutine("TryToShowMyLobbyLiked");
		StartCoroutine("TryToShowMyLobbyLiked");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(false);
		}
		StopCoroutine(SetSocialButtonsState());
		if (!TrainingController.TrainingCompleted && HintController.instance != null)
		{
			AskNameManager.onComplete -= HintController.instance.ShowCurrentHintObjectLabel;
		}
	}

	private void HandleDiclineClicked(object sender, EventArgs e)
	{
		Defs.isShowUserAgrement = false;
		UserAgreementPanel.SetActive(false);
	}

	public void ShowBankWindow()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (LeaderboardsIsOpening)
		{
			return;
		}
		if (_shopInstance != null)
		{
			UnityEngine.Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested += HandleBackFromBankClicked;
		if ((!(GiftBannerWindow.instance == null) && GiftBannerWindow.instance.IsShow) || !ShowBannerOrLevelup())
		{
			_bankEnteredTime = Time.realtimeSinceStartup;
			ButtonClickSound.Instance.PlayClick();
			if (mainPanel != null)
			{
				mainPanel.transform.root.gameObject.SetActive(false);
			}
			if (nicknameLabel != null)
			{
				nicknameLabel.transform.root.gameObject.SetActive(false);
			}
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, null);
		}
	}

	private void HandleBackFromBankClicked(object sender, EventArgs e)
	{
		if (_shopInstance != null)
		{
			UnityEngine.Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested -= HandleBackFromBankClicked;
		BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
		if (nicknameLabel != null)
		{
			nicknameLabel.transform.root.gameObject.SetActive(true);
		}
		if (mainPanel != null)
		{
			mainPanel.transform.root.gameObject.SetActive(true);
		}
		if (InMiniGamesScreen)
		{
			ExperienceController.SetEnable(true);
		}
	}

	private void HandleSupportButtonClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		settingsPanel.SetActive(false);
		_feedbackPanel.Value.gameObject.SetActive(true);
	}

	public void StartCampaingButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		((Action)delegate
		{
			Defs.isMulti = false;
			GameConnect.SetGameMode(GameConnect.GameMode.Campaign);
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset();
			StoreKitEventListener.State.PurchaseKey = "In game";
			new Dictionary<string, string>
			{
				{
					Defs.RankParameterKey,
					ExperienceController.sharedController.currentLevel.ToString()
				},
				{
					Defs.MultiplayerModesKey,
					StoreKitEventListener.State.Mode
				}
			};
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = "CampaignChooseBox";
			LoadConnectScene.noteToShow = null;
			SceneManager.LoadScene(Defs.PromSceneName);
		})();
	}

	private void HandleCampaingClicked(object sender, EventArgs e)
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			StartCampaingButton();
		}
	}

	public void StartSpeedrunButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		((Action)delegate
		{
			Defs.isMulti = false;
			GameConnect.SetGameMode(GameConnect.GameMode.SpeedRun);
			CurrentCampaignGame.levelSceneName = "";
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset();
			StoreKitEventListener.State.PurchaseKey = "In game";
			new Dictionary<string, string>
			{
				{
					Defs.RankParameterKey,
					ExperienceController.sharedController.currentLevel.ToString()
				},
				{
					Defs.MultiplayerModesKey,
					StoreKitEventListener.State.Mode
				}
			};
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading");
		})();
	}

	public void StartSurvivalButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		((Action)delegate
		{
			Defs.isMulti = false;
			GameConnect.SetGameMode(GameConnect.GameMode.Arena);
			CurrentCampaignGame.levelSceneName = "";
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset();
			StoreKitEventListener.State.PurchaseKey = "In game";
			new Dictionary<string, string>
			{
				{
					Defs.RankParameterKey,
					ExperienceController.sharedController.currentLevel.ToString()
				},
				{
					Defs.MultiplayerModesKey,
					StoreKitEventListener.State.Mode
				}
			};
			Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, Defs.SurvivalMaps.Length);
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading");
		})();
	}

	public void HandleSurvivalClicked()
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			StartSurvivalButton();
		}
	}

	public void HandleSandboxClicked()
	{
		if (_shopInstance != null || ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!ProtocolListGetter.currentVersionIsSupported)
		{
			BannerWindowController bannerWindowController = BannerWindowController.SharedController;
			if (bannerWindowController != null)
			{
				bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
			}
		}
		else
		{
			GoSandBox();
		}
	}

	public void GoSandBox()
	{
		GameConnect.SetGameMode(GameConnect.GameMode.Dater);
		connectionControl.ConnectToPhoton(JoinToRoomAfterConnect);
	}

	private void JoinToRoomAfterConnect()
	{
		connectionControl.JoinRandomRoom(OnJoinedRoom);
	}

	private void OnJoinedRoom()
	{
		PhotonNetwork.isMessageQueueRunning = false;
		StartCoroutine(MoveToGameScene());
	}

	public static IEnumerator MoveToGameScene()
	{
		if (sharedController.InMiniGamesScreen)
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = false;
		}
		else if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = true;
		}
		else
		{
			Defs.isGameFromFriends = true;
			Defs.isGameFromClans = false;
		}
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
		WeaponManager.sharedManager.Reset((int)((scInfo != null) ? scInfo.AvaliableWeapon : ModeWeapon.all));
		UnityEngine.Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		PhotonNetwork.isMessageQueueRunning = false;
		UnityEngine.Debug.Log("map=" + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
		UnityEngine.Debug.Log(scInfo.NameScene);
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + scInfo.NameScene) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = scInfo.NameScene;
		LoadConnectScene.noteToShow = null;
		yield return Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene");
	}

	private void GoMulty()
	{
		if (!(mainPanel == null) && !(connectScene == null))
		{
			MainMenuHeroCamera.Instance.MainCamera.enabled = false;
			NickLabelStack.sharedStack.gameObject.SetActive(false);
			BannerWindowController.SharedController.ResetScene();
			ConnectScene.isReturnFromGame = false;
			Defs.isMulti = true;
			MenuBackgroundMusic.keepPlaying = true;
			mainPanel.SetActive(false);
			connectScene.SetActive(true);
		}
	}

	public void OnClickMultiplyerButton()
	{
		ButtonClickSound.Instance.PlayClick();
		((Action)delegate
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				GoMulty();
			}
		})();
	}

	public void HandleMultiPlayerClicked(object sender, EventArgs e)
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			OnClickMultiplyerButton();
		}
	}

	private void HandleSkinsMakerClicked(object sender, EventArgs e)
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			PlayerPrefs.SetInt(Defs.SkinEditorMode, 0);
			GlobalGameController.EditingCape = 0;
			GlobalGameController.EditingLogo = 0;
			Singleton<SceneLoader>.Instance.LoadScene("SkinEditor");
		}
	}

	private IEnumerator HideMenuInterfaceCoroutine(GameObject nickLabelObj)
	{
		yield return null;
		if (nickLabelObj != null)
		{
			nickLabelObj.SetActive(false);
		}
		rotateCamera.gameObject.SetActive(false);
		if (mainPanel != null)
		{
			mainPanel.transform.root.gameObject.SetActive(false);
		}
	}

	private void GoFriens()
	{
		MenuBackgroundMusic.keepPlaying = true;
		if (FriendsWindowGUI.Instance == null)
		{
			UnityEngine.Debug.LogWarning("FriendsWindowController.Instance == null");
		}
		else
		{
			if (ShowBannerOrLevelup())
			{
				return;
			}
			FriendsController.sharedController.GetFriendsData(true);
			ButtonClickSound.Instance.PlayClick();
			GameObject nickLabelObj = null;
			if (NickLabelStack.sharedStack != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(false);
			}
			if (!friendsOnStart)
			{
				StartCoroutine(HideMenuInterfaceCoroutine(nickLabelObj));
			}
			FriendsWindowGUI.Instance.ShowInterface(delegate
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
				rotateCamera.gameObject.SetActive(true);
				if (mainPanel != null)
				{
					mainPanel.transform.root.gameObject.SetActive(true);
				}
			});
			FriendsController.sharedController.DownloadDataAboutPossibleFriends();
		}
	}

	private void HandleFriendsClicked(object sender, EventArgs e)
	{
		if (_leaderboardsIsOpening || _shopInstance != null || ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		((Action)delegate
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				GoFriens();
			}
		})();
	}

	private void HandleNewsButtonClicked(object sender, EventArgs e)
	{
		if (!_leaderboardsIsOpening && !(_shopInstance != null) && !ShowBannerOrLevelup() && !FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			_newsPanel.Value.gameObject.SetActive(true);
			mainPanel.SetActive(false);
		}
	}

	private void HandleCraftButtonClicked(object sender, EventArgs e)
	{
		if (!_leaderboardsIsOpening && !(_shopInstance != null) && !ShowBannerOrLevelup() && !FriendsWindowGUI.Instance.InterfaceEnabled && !(mainPanel == null))
		{
			InitLobbyCraftController();
			if (LobbyCraftController.Instance != null)
			{
				mainPanel.SetActive(false);
				LobbyCraftController.Instance.InterfaceEnabled = true;
			}
		}
	}

	private void HandleProfileClicked(object sender, EventArgs e)
	{
		if (!_leaderboardsIsOpening)
		{
			GoToProfile();
		}
	}

	public void GoToProfile()
	{
		if (_shopInstance != null || ShowBannerOrLevelup() || FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 0);
		if (ProfileController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("ProfileController.Instance == null");
		}
		else
		{
			if (ShowBannerOrLevelup())
			{
				return;
			}
			ButtonClickSound.Instance.PlayClick();
			if (NickLabelStack.sharedStack.gameObject != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(false);
			}
			if (mainPanel != null)
			{
				mainPanel.transform.root.gameObject.SetActive(false);
			}
			ProfileController.Instance.ShowInterface(delegate
			{
				if (NickLabelStack.sharedStack.gameObject != null)
				{
					NickLabelStack.sharedStack.gameObject.SetActive(true);
				}
				if (mainPanel != null)
				{
					mainPanel.transform.root.gameObject.SetActive(true);
				}
			});
		}
	}

	private void HandleFreeClicked(object sender, EventArgs e)
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			settingsPanel.SetActive(false);
			_freePanel.Value.SetVisible(true);
		}
	}

	private void HandleGameServicesClicked(object sender, EventArgs e)
	{
	}

	private void HandleResumeFromShop()
	{
		if (_shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance = null;
			if (NickLabelStack.sharedStack != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
			}
			if (StarterPackController.Get != null && StarterPackController.Get.isEventActive)
			{
				StarterPackController.Get.CheckShowStarterPack();
			}
			StartCoroutine(ShowRanks());
		}
	}

	public static IEnumerator ShowRanks()
	{
		for (int i = 0; i < 0; i++)
		{
			yield return null;
		}
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	private static void UnequipSniperRifleAndArmryArmoIfNeeded()
	{
		try
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				int trainingStep = AnalyticsStuff.TrainingStep;
				if (Storager.getInt("Training.NoviceArmorUsedKey") != 1 && trainingStep < 12 && Storager.getString(Defs.ArmorNewEquppedSN) != Defs.ArmorNewNoneEqupped)
				{
					ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
				}
				if (trainingStep < 10 && WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null && (from w in WeaponManager.sharedManager.playerWeapons.OfType<Weapon>()
					select w.weaponPrefab.GetComponent<WeaponSounds>()).FirstOrDefault((WeaponSounds ws) => ws.categoryNabor - 1 == 4) != null)
				{
					WeaponManager.sharedManager.SaveWeaponSet(Defs.MultiplayerWSSN, "", 4);
					WeaponManager.sharedManager.Reset();
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in UnequipSniperRifleAndArmryArmoIfNeeded: " + ex);
		}
	}

	public void HandleShopClicked(object sender, EventArgs e)
	{
		if (!(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) != 1)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			_shopInstance = ShopNGUIController.sharedShop;
			if (_shopInstance != null)
			{
				UnequipSniperRifleAndArmryArmoIfNeeded();
				_shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				_shopInstance.resumeAction = HandleResumeFromShop;
			}
			else
			{
				UnityEngine.Debug.LogWarning("sharedShop == null");
			}
		}
	}

	private void HandleSettingsClicked(object sender, EventArgs e)
	{
		if (!_leaderboardsIsOpening && !(_shopInstance != null) && !ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			rotateCamera.PlayOpenOptions();
			ButtonClickSound.Instance.PlayClick();
			StartCoroutine(OpenSettingPanelWithDelay());
		}
	}

	private IEnumerator OpenSettingPanelWithDelay()
	{
		yield return null;
		settingsPanel.SetActive(true);
		mainPanel.SetActive(false);
		if (AnimationGift.instance != null)
		{
			AnimationGift.instance.CheckVisibleGift();
		}
	}

	public void RateUs()
	{
		if (!ShopOpened && !ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL(RateUsURL);
		}
	}

	public static void SetInputEnabled(bool enabled)
	{
		if (sharedController != null && !ShopNGUIController.GuiActive)
		{
			sharedController.uiCamera.enabled = enabled;
		}
	}

	private void OnStarterPackContainerShow(bool enable)
	{
		Task<TrafficForwardingInfo> val = FriendsController.sharedController.Map((FriendsController f) => f.GetComponent<TrafficForwardingScript>()).Map((TrafficForwardingScript t) => t.GetTrafficForwardingInfo()).Filter((Task<TrafficForwardingInfo> t) => ((Task)t).IsCompleted && !((Task)t).IsCanceled && !((Task)t).IsFaulted);
		bool flag = (val == null || !TrafficForwardingEnabled(val.Result)) && enable;
		starterPackPanel.gameObject.SetActive(flag);
		if (flag)
		{
			buttonBackground.mainTexture = StarterPackController.Get.GetCurrentPackImage();
		}
		_starterPackEnabled = flag;
		starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
	}

	public void HandleTrafficForwardingClicked()
	{
		if (string.IsNullOrEmpty(_trafficForwardingUrl))
		{
			UnityEngine.Debug.LogError("HandleTrafficForwardingClicked() called while trafficForwardingUrl is empty.");
			return;
		}
		try
		{
			int @int = PlayerPrefs.GetInt("TrafficForwarded", 0);
			PlayerPrefs.SetInt("TrafficForwarded", @int + 1);
			AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Press);
			FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.click);
		}
		finally
		{
			TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
			if (trafficForwardingScript != null)
			{
				Task<TrafficForwardingInfo> trafficForwardingInfo = trafficForwardingScript.GetTrafficForwardingInfo();
				TrafficForwardingInfo e = ((((Task)trafficForwardingInfo).IsCompleted && !((Task)trafficForwardingInfo).IsCanceled && !((Task)trafficForwardingInfo).IsFaulted) ? trafficForwardingInfo.Result : TrafficForwardingInfo.DisabledInstance);
				RefreshTrafficForwardingButton(this, e);
			}
			else
			{
				RefreshTrafficForwardingButton(this, TrafficForwardingInfo.DisabledInstance);
			}
		}
		Application.OpenURL(_trafficForwardingUrl);
	}

	private bool TrafficForwardingEnabled(TrafficForwardingInfo e)
	{
		if (PlayerPrefs.GetInt("TrafficForwarded", 0) < 1 && !SavedShwonLobbyLevelIsLessThanActual() && TrainingController.TrainingCompleted && e.Enabled && ExperienceController.sharedController.currentLevel >= e.MinLevel)
		{
			return ExperienceController.sharedController.currentLevel <= e.MaxLevel;
		}
		return false;
	}

	private void RefreshTrafficForwardingButton(object sender, TrafficForwardingInfo e)
	{
		if (e == null)
		{
			UnityEngine.Debug.LogError("Null TrafficForwardingInfo passed.");
			e = TrafficForwardingInfo.DisabledInstance;
		}
		_trafficForwardingUrl = e.Url;
		bool enabled = false;
		try
		{
			if (!(this == null))
			{
				enabled = TrafficForwardingEnabled(e);
				if (enabled && PlayerPrefs.GetInt(Defs.TrafficForwardingShowAnalyticsSent, 0) == 0)
				{
					AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Show);
					PlayerPrefs.SetInt(Defs.TrafficForwardingShowAnalyticsSent, 1);
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.newView);
				}
				else if (enabled)
				{
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.view);
				}
				trafficForwardActive = enabled;
				ButtonBannerHUD.OnUpdateBanners();
				trafficForwardingButton.Do(delegate(UIButton tf)
				{
					tf.gameObject.SetActive(enabled);
				});
			}
		}
		finally
		{
			OnStarterPackContainerShow(!enabled && StarterPackController.Get.isEventActive);
		}
	}

	public void OnShowBannerGift()
	{
		BannerWindowController bannerWindowController = BannerWindowController.SharedController;
		if (!(bannerWindowController == null))
		{
			bannerWindowController.ForceShowBanner(BannerWindowType.GiftBonuse);
		}
	}

	public void HandleLeaderboardsClicked()
	{
		ShowLeaderboards();
	}

	public void ShowLeaderboards(LeaderboardsView.State? state = null)
	{
		if (mainPanel.activeInHierarchy && !_leaderboardsIsOpening && !InMiniGamesScreen && !FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			StartCoroutine(HandleLeaderboardsClickedCoroutine(state));
		}
	}

	private IEnumerator ContinueWithCoroutine(Task task, Action<Task> continuation)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		if (continuation != null)
		{
			while (!task.IsCompleted)
			{
				yield return null;
			}
			continuation(task);
		}
	}

	private IEnumerator HandleLeaderboardsClickedCoroutine(LeaderboardsView.State? toState = null)
	{
		_leaderboardsIsOpening = true;
		_leaderboardScript.Value.Show();
		if (mainPanel == null || LeaderboardsPanel == null || !mainPanel.activeInHierarchy || LeaderboardsPanel.gameObject.activeInHierarchy || _leaderboardScript.Value == null)
		{
			_leaderboardsIsOpening = false;
			yield break;
		}
		Action<Task> continuation = delegate
		{
			LeaderboardsPanel.gameObject.SetActive(false);
			mainPanel.SetActive(true);
			if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty())
			{
				UIButton[] value2 = _leaderboardsButtons.Value;
				for (int j = 0; j < value2.Length; j++)
				{
					value2[j].isEnabled = true;
				}
			}
		};
		StartCoroutine(ContinueWithCoroutine(_leaderboardScript.Value.GetReturnFuture(), continuation));
		_leaderboardScript.Value.RefreshMyLeaderboardEntries();
		UIButton[] value = _leaderboardsButtons.Value;
		for (int i = 0; i < value.Length; i++)
		{
			value[i].isEnabled = false;
		}
		LeaderboardsPanel.gameObject.SetActive(true);
		LeaderboardsPanel.alpha = float.Epsilon;
		LeaderboardsView view = LeaderboardsPanel.Map((UIPanel p) => p.GetComponent<LeaderboardsView>());
		if (view != null)
		{
			while (!view.Prepared)
			{
				yield return null;
			}
			if (toState.HasValue)
			{
				view.CurrentState = toState.Value;
			}
			else
			{
				int @int = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
				LeaderboardsView.State state = (Enum.IsDefined(typeof(LeaderboardsView.State), @int) ? ((LeaderboardsView.State)@int) : LeaderboardsView.State.BestPlayers);
				view.CurrentState = ((state != 0) ? state : LeaderboardsView.State.BestPlayers);
			}
		}
		mainPanel.SetActive(false);
		LeaderboardsPanel.alpha = 1f;
		if (FriendsController.sharedController != null && !FriendsController.sharedController.id.IsNullOrEmpty())
		{
			value = _leaderboardsButtons.Value;
			for (int i = 0; i < value.Length; i++)
			{
				value[i].isEnabled = true;
			}
		}
		_leaderboardsIsOpening = false;
	}

	public bool InappBonusChestCanShow()
	{
		if (!SettingsJoysticksPanel.activeInHierarchy && !settingsPanel.activeInHierarchy && !FreePanelIsActive && !InMiniGamesScreen)
		{
			if (FeedbackMenuController.Instance != null)
			{
				return !FeedbackMenuController.Instance.gameObject.activeInHierarchy;
			}
			return true;
		}
		return false;
	}

	private void UpdateInappBonusChestActiveState()
	{
		bool flag = BalanceController.isActiveInnapBonus() && InappBonusChestCanShow();
		if (InAppBonusLobbyController.Instance != null && InAppBonusLobbyController.Instance.Enabled != flag)
		{
			InAppBonusLobbyController.Instance.Enabled = flag;
		}
	}

	private void LoadConnectSceneCoroutine()
	{
		GameObject original = Resources.Load("ConnectScene/ConnectSceneInLobby") as GameObject;
		connectScene = UnityEngine.Object.Instantiate(original);
		connectScene.transform.parent = base.transform;
		connectScene.transform.localScale = Vector3.one;
		connectScene.transform.localPosition = Vector3.zero;
	}

	public List<int> NewAvailableMiniGameModes()
	{
		return AvailableMinigamesForOurLevel().Except(shownMinigameModes).ToList();
	}

	public void MarkMiniGameModeAsNotNew(int mode)
	{
		if (!shownMinigameModes.Contains(mode))
		{
			shownMinigameModes.Add(mode);
			Storager.setString("MainMenuController.MINIGAME_SHOWN_MODES_KEY", Json.Serialize(shownMinigameModes));
		}
	}

	public static void MarkAllMiniGamesAsShown()
	{
		try
		{
			List<int> obj = GetShownMiniGamesFromDisk().Union(AvailableMinigamesForOurLevel()).ToList();
			Storager.setString("MainMenuController.MINIGAME_SHOWN_MODES_KEY", Json.Serialize(obj));
			if (sharedController != null)
			{
				sharedController.shownMinigameModes = obj;
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in MarkAllMiniGamesAsShown: {0}", ex);
		}
	}

	private static List<int> AvailableMinigamesForOurLevel()
	{
		return (from mg in GameConnect.MiniGames
			where mg == GameConnect.GameMode.Campaign || BalanceController.ParametersForMiniGameType(mg).LevelRequired <= ExperienceController.sharedController.currentLevel
			select (int)mg).ToList();
	}

	private void OnDayOfValorContainerShow(bool enable)
	{
		dayOfValorContainer.gameObject.SetActive(enable);
		_dayOfValorEnabled = enable;
		dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
	}

	public void HandlePremiumClicked()
	{
		ShopNGUIController.ShowPremimAccountExpiredIfPossible(RentExpiredPoint, "NGUI", "", false);
	}

	private IEnumerator ShowMiniGames(GameConnect.GameMode miniGame, bool isActive)
	{
		InMiniGamesScreen = isActive;
		mainPanel.SetActive(!isActive);
		MainMenuHeroCamera.Instance.MainCamera.enabled = !isActive;
		NickLabelStack.sharedStack.gameObject.SetActive(!isActive);
		if (miniGamesPoint.childCount == 0)
		{
			Transform transform = UnityEngine.Object.Instantiate(Resources.Load<Transform>("PanelMiniGamesConnect"));
			transform.parent = miniGamesPoint;
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			transform.GetComponent<ChooseMiniGameController>().onBackPressed = OnClickBackMiniGamesButton;
			connectionControl = transform.gameObject.AddComponent<ConnectionControl>();
			connectionControl.SetConnectPanel(connectPanel.GetComponent<ConnectPanel>());
		}
		miniGamesPoint.GetChild(0).GetComponent<ChooseMiniGameController>().SetEnabled(miniGame, isActive);
		FreeAwardShowHandler.CheckShowChest(isActive);
		ExperienceController.SetEnable(isActive && !stubLoading.activeSelf);
		yield break;
	}

	public void OnClickMiniGamesButton()
	{
		((Action)delegate
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				OpenMiniGames(GameConnect.GameMode.Campaign);
			}
		})();
	}

	public void OnClickBackMiniGamesButton()
	{
		StartCoroutine(ShowRanks());
		StartCoroutine(ShowMiniGames(GameConnect.GameMode.Campaign, false));
		rotateCamera.OnCloseMinigamesGui();
	}

	private void OpenMiniGames(GameConnect.GameMode miniGame)
	{
		if (LeaderboardsIsOpening)
		{
			return;
		}
		if (!ProtocolListGetter.currentVersionIsSupported)
		{
			BannerWindowController bannerWindowController = BannerWindowController.SharedController;
			if (bannerWindowController != null)
			{
				bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
			}
		}
		else
		{
			StartCoroutine(ShowMiniGames(miniGame, true));
			rotateCamera.OnOpenSingleModePanel();
		}
	}

	private void HandleSocialButton(object sender, EventArgs e)
	{
		if (!_leaderboardsIsOpening && !FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			rotateCamera.PlayOpenSocial();
			ButtonClickSound.Instance.PlayClick();
			_freePanel.Value.SetVisible(true);
			mainPanel.SetActive(false);
			AnimationGift.instance.CheckVisibleGift();
		}
	}
}
