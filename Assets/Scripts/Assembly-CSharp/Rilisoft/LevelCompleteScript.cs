using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class LevelCompleteScript : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CPlayGetCoinsClip_003Ed__67 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LevelCompleteScript _003C_003E4__this;

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
			public _003CPlayGetCoinsClip_003Ed__67(int _003C_003E1__state)
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
					float seconds = ((ExperienceController.sharedController != null) ? ExperienceController.sharedController.exp_1.length : 0.5f);
					_003C_003E2__current = new WaitForSeconds(seconds);
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					if (_003C_003E4__this.awardClip != null && Defs.isSoundFX)
					{
						NGUITools.PlaySound(_003C_003E4__this.awardClip);
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
		internal sealed class _003CDisplayLevelResult_003Ed__90 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LevelCompleteScript _003C_003E4__this;

			private List<GameObject> _003Cstars_003E5__1;

			private int _003CcurrentStarIndex_003E5__2;

			private int _003CcheckboxIndex_003E5__3;

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
			public _003CDisplayLevelResult_003Ed__90(int _003C_003E1__state)
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
				try
				{
					int num;
					switch (_003C_003E1__state)
					{
					default:
						return false;
					case 0:
					{
						_003C_003E1__state = -1;
						_003C_003E1__state = -3;
						_003C_003E4__this.DisplayLevelResultIsRunning = true;
						_003C_003E4__this.menuButton.SetActive(false);
						_003C_003E4__this.retryButton.SetActive(false);
						_003C_003E4__this.nextButton.SetActive(false);
						_003C_003E4__this.shopView.PopRequest();
						_003C_003E4__this._shouldShowFacebookButton = false;
						_003C_003E4__this._shouldShowTwitterButton = false;
						InitializeCoinIndexBound();
						_003Cstars_003E5__1 = new List<GameObject>(3);
						for (int i = 0; i != 3; i++)
						{
							float x = -140f + (float)i * 140f;
							GameObject gameObject3 = UnityEngine.Object.Instantiate(_003C_003E4__this.darkStarPrototypeSprite);
							gameObject3.transform.parent = _003C_003E4__this.darkStarPrototypeSprite.transform.parent;
							gameObject3.transform.localPosition = new Vector3(x, _003C_003E4__this.darkStarPrototypeSprite.transform.localPosition.y, 0f);
							gameObject3.transform.localScale = _003C_003E4__this.darkStarPrototypeSprite.transform.localScale;
							gameObject3.SetActive(true);
							_003Cstars_003E5__1.Add(gameObject3);
						}
						_003CcurrentStarIndex_003E5__2 = 0;
						_003CcheckboxIndex_003E5__3 = 0;
						goto IL_039b;
					}
					case 1:
					{
						_003C_003E1__state = -3;
						GameObject gameObject = UnityEngine.Object.Instantiate(_003C_003E4__this.brightStarPrototypeSprite);
						gameObject.transform.parent = _003C_003E4__this.brightStarPrototypeSprite.transform.parent;
						gameObject.transform.localPosition = _003Cstars_003E5__1[_003CcurrentStarIndex_003E5__2].transform.localPosition;
						gameObject.transform.localScale = _003Cstars_003E5__1[_003CcurrentStarIndex_003E5__2].transform.localScale;
						gameObject.SetActive(true);
						UnityEngine.Object.Destroy(_003Cstars_003E5__1[_003CcurrentStarIndex_003E5__2]);
						GameObject gameObject2 = UnityEngine.Object.Instantiate(_003C_003E4__this.checkboxSpritePrototype);
						gameObject2.transform.parent = _003C_003E4__this.checkboxSpritePrototype.transform.parent;
						gameObject2.transform.localPosition = new Vector3(_003C_003E4__this.checkboxSpritePrototype.transform.localPosition.x, _003C_003E4__this.checkboxSpritePrototype.transform.localPosition.y - 45f * (float)_003CcheckboxIndex_003E5__3, _003C_003E4__this.checkboxSpritePrototype.transform.localPosition.z);
						gameObject2.transform.localScale = _003C_003E4__this.checkboxSpritePrototype.transform.localScale;
						gameObject2.SetActive(true);
						if (_003C_003E4__this.starClips != null && _003CcurrentStarIndex_003E5__2 < _003C_003E4__this.starClips.Length && _003C_003E4__this.starClips[_003CcurrentStarIndex_003E5__2] != null && Defs.isSoundFX)
						{
							NGUITools.PlaySound(_003C_003E4__this.starClips[_003CcurrentStarIndex_003E5__2]);
						}
						num = _003CcurrentStarIndex_003E5__2 + 1;
						_003CcurrentStarIndex_003E5__2 = num;
						goto IL_0389;
					}
					case 2:
						{
							_003C_003E1__state = -3;
							_003C_003E4__this.StartCoroutine(_003C_003E4__this.TryToShowExpiredBanner());
							_003Cstars_003E5__1 = null;
							_003C_003Em__Finally1();
							return false;
						}
						IL_0389:
						num = _003CcheckboxIndex_003E5__3 + 1;
						_003CcheckboxIndex_003E5__3 = num;
						goto IL_039b;
						IL_039b:
						if (_003CcheckboxIndex_003E5__3 < 3)
						{
							if ((_003CcheckboxIndex_003E5__3 != 1 || CurrentCampaignGame.completeInTime) && (_003CcheckboxIndex_003E5__3 != 2 || CurrentCampaignGame.withoutHits))
							{
								_003C_003E2__current = new WaitForSeconds(0.7f);
								_003C_003E1__state = 1;
								return true;
							}
							goto IL_0389;
						}
						UnityEngine.Object.Destroy(_003C_003E4__this.brightStarPrototypeSprite);
						UnityEngine.Object.Destroy(_003C_003E4__this.darkStarPrototypeSprite);
						_003C_003E4__this.menuButton.SetActive(true);
						_003C_003E4__this.retryButton.SetActive(true);
						_003C_003E4__this.nextButton.SetActive(true);
						_003C_003E4__this.shopView.PushRequest();
						_003C_003E4__this._shouldShowFacebookButton = FacebookController.FacebookSupported;
						_003C_003E4__this._shouldShowTwitterButton = TwitterController.TwitterSupported;
						if (_003C_003E4__this._shouldBlinkCoinsIndicatorAfterRewardWindow)
						{
							CoinsMessage.FireCoinsAddedEvent();
						}
						if (_003C_003E4__this._shouldBlinkGemsIndicatorAfterRewardWindow)
						{
							CoinsMessage.FireCoinsAddedEvent(true);
						}
						_003C_003E2__current = new WaitForSeconds(0.7f);
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
				_003C_003E4__this.DisplayLevelResultIsRunning = false;
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass93_0
		{
			public FacebookController.StoryPriority priority;

			internal void _003CTryToShowExpiredBanner_003Eb__0()
			{
				FacebookController.PostOpenGraphStory("get", "star", priority, new Dictionary<string, string> { 
				{
					"chapter",
					CurrentCampaignGame.boXName
				} });
			}
		}

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass93_1
		{
			public FacebookController.StoryPriority priority;

			internal void _003CTryToShowExpiredBanner_003Eb__2()
			{
				FacebookController.PostOpenGraphStory("find", "secret", priority, new Dictionary<string, string> { 
				{
					"chapter",
					CurrentCampaignGame.boXName
				} });
			}
		}

		[CompilerGenerated]
		internal sealed class _003CTryToShowExpiredBanner_003Ed__93 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LevelCompleteScript _003C_003E4__this;

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
			public _003CTryToShowExpiredBanner_003Ed__93(int _003C_003E1__state)
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
							if (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ExpController.Instance != null && ExpController.Instance.WaitingForLevelUpView) || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || _003C_003E4__this.loadingPanel.activeInHierarchy || ExchangeWindow.IsOpened || _003C_003E4__this.RentWindowPoint.childCount != 0)
							{
								break;
							}
							if (_003C_003E4__this._shouldShowAllStarsCollectedRewardWindow && _003C_003E4__this._numOfRewardWindowsShown < 2)
							{
								_003C_003E4__this._shouldShowAllStarsCollectedRewardWindow = false;
								PlayerPrefs.SetInt("AllStarsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
								if ((!FacebookController.FacebookSupported && !TwitterController.TwitterSupported) || Device.isPixelGunLow)
								{
									break;
								}
								_003C_003Ec__DisplayClass93_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass93_0();
								GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/AllStarsNGUI"));
								RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
								CS_0024_003C_003E8__locals0.priority = FacebookController.StoryPriority.Green;
								component.priority = CS_0024_003C_003E8__locals0.priority;
								component.shareAction = delegate
								{
									FacebookController.PostOpenGraphStory("get", "star", CS_0024_003C_003E8__locals0.priority, new Dictionary<string, string> { 
									{
										"chapter",
										CurrentCampaignGame.boXName
									} });
								};
								component.HasReward = false;
								component.twitterStatus = () => string.Format("I've got all the stars in {0} in @PixelGun3D! Play now and try to get them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", new object[1] { BoxNameForTwitter() });
								component.EventTitle = "All Stars";
								foreach (UILabel headerLabel in gameObject.GetComponent<AllStarsRewardSettings>().headerLabels)
								{
									if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
									{
										headerLabel.text = LocalizationStore.Get("Key_1543");
									}
									else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
									{
										headerLabel.text = LocalizationStore.Get("Key_1544");
									}
									else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
									{
										headerLabel.text = LocalizationStore.Get("Key_1545");
									}
								}
								gameObject.transform.parent = _003C_003E4__this.RentWindowPoint;
								Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
								gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
								gameObject.transform.localRotation = Quaternion.identity;
								gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
								break;
							}
							if (!_003C_003E4__this._shouldShowAllSecretsCollectedRewardWindow || _003C_003E4__this._numOfRewardWindowsShown >= 2)
							{
								int num;
								if (Storager.getInt(Defs.PremiumEnabledFromServer) == 1)
								{
									ShopNGUIController.ShowPremimAccountExpiredIfPossible(_003C_003E4__this.RentWindowPoint, "Default");
								}
								else
									num = 0;
								break;
							}
							_003C_003E4__this._shouldShowAllSecretsCollectedRewardWindow = false;
							PlayerPrefs.SetInt("AllSecretsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
							if ((!FacebookController.FacebookSupported && !TwitterController.TwitterSupported) || Device.isPixelGunLow)
							{
								break;
							}
							_003C_003Ec__DisplayClass93_1 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass93_1();
							GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/AllSecretsNGUI"));
							RewardWindowBase component2 = gameObject2.GetComponent<RewardWindowBase>();
							CS_0024_003C_003E8__locals1.priority = FacebookController.StoryPriority.Green;
							component2.priority = CS_0024_003C_003E8__locals1.priority;
							component2.shareAction = delegate
							{
								FacebookController.PostOpenGraphStory("find", "secret", CS_0024_003C_003E8__locals1.priority, new Dictionary<string, string> { 
								{
									"chapter",
									CurrentCampaignGame.boXName
								} });
							};
							component2.HasReward = false;
							component2.twitterStatus = () => string.Format("I've found all coins in {0} in @PixelGun3D! Play now and try to find them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", new object[1] { BoxNameForTwitter() });
							component2.EventTitle = "All Coins";
							foreach (UILabel headerLabel2 in gameObject2.GetComponent<AllSecretsRewardSettings>().headerLabels)
							{
								if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
								{
									headerLabel2.text = LocalizationStore.Get("Key_1540");
								}
								else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
								{
									headerLabel2.text = LocalizationStore.Get("Key_1541");
								}
								else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
								{
									headerLabel2.text = LocalizationStore.Get("Key_1542");
								}
							}
							gameObject2.transform.parent = _003C_003E4__this.RentWindowPoint;
							Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("Default"));
							gameObject2.transform.localPosition = new Vector3(0f, 0f, -130f);
							gameObject2.transform.localRotation = Quaternion.identity;
							gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogWarning("exception in LevelComplete  TryToShowExpiredBanner: " + ex);
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

		public static LevelCompleteScript sharedScript;

		public GameObject miniGamesRetryButton;

		public UILabel[] miniGameMatchPriceLabel;

		public RewardWindowBase rewardWindow;

		public RewardWindowBase rewardWindowSurvival;

		public CampaignLevelCompleteRewardSettings rewardSettings;

		public ArenaRewardWindowSettings survivalRewardWindowSettings;

		public GameObject mainInterface;

		public Transform RentWindowPoint;

		public GameObject mainPanel;

		public GameObject loadingPanel;

		public GameObject quitButton;

		public GameObject menuButton;

		public GameObject retryButton;

		public GameObject nextButton;

		public BankShopViewGuiElement shopView;

		public GameObject brightStarPrototypeSprite;

		public GameObject darkStarPrototypeSprite;

		public GameObject award1coinSprite;

		public GameObject checkboxSpritePrototype;

		public AudioClip[] starClips;

		public AudioClip shopButtonSound;

		public AudioClip awardClip;

		public GameObject survivalResults;

		public GameObject facebookButton;

		public GameObject twitterButton;

		public GameObject backgroundTexture;

		public GameObject backgroundSurvivalTexture;

		public GameObject[] statisticLabels;

		public GameObject gameOverSprite;

		public UICamera uiCamera;

		public GameObject speedrunResultWindow;

		public GameObject speedrunNewRecordLabel;

		public UILabel speedrunCurrentScore;

		public UILabel speedrunBestScore;

		private static LevelCompleteScript _instance = null;

		private int _numOfRewardWindowsShown;

		private bool _hasAwardForMission;

		private bool _shouldBlinkCoinsIndicatorAfterRewardWindow;

		private bool _shouldBlinkGemsIndicatorAfterRewardWindow;

		private bool _shouldShowAllStarsCollectedRewardWindow;

		private bool _shouldShowAllSecretsCollectedRewardWindow;

		private const string AllStarsForBoxRewardWindowIsShownNameBase = "AllStarsForBoxRewardWindowIsShown_";

		private const string AllSecretsForBoxRewardWindowIsShownNameBase = "AllSecretsForBoxRewardWindowIsShown_";

		private IDisposable _backSubscription;

		private static Dictionary<string, string> boxNamesTwitter = new Dictionary<string, string>
		{
			{ "Real", "PIXELATED WORLD" },
			{ "minecraft", "BLOCK WORLD" },
			{ "Crossed", "CROSSED WORLDS" }
		};

		private bool _awardConferred;

		private AudioSource _awardAudioSource;

		private ExperienceController _experienceController;

		private int _oldStarCount;

		private int _starCount;

		private ShopNGUIController _shopInstance;

		private string _nextSceneName = string.Empty;

		private bool _isLastLevel;

		private int? _boxCompletionExperienceAward;

		private bool completedFirstTime;

		private bool _gameOver;

		private bool _shouldShowFacebookButton;

		private bool _shouldShowTwitterButton;

		public static bool IsInterfaceBusy
		{
			get
			{
				if (sharedScript == null)
				{
					return false;
				}
				if (!IsShowRewardWindow() && !sharedScript.DisplayLevelResultIsRunning)
				{
					return sharedScript.DisplaySurvivalResultIsRunning;
				}
				return true;
			}
		}

		public bool DisplayLevelResultIsRunning { get; set; }

		public bool DisplaySurvivalResultIsRunning { get; set; }

		internal static GameResult LastGameResult { private get; set; }

		internal static bool IsRunning
		{
			get
			{
				return _instance != null;
			}
		}

		private bool AllStarsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt("AllStarsForBoxRewardWindowIsShown_" + boXName, 0) == 1;
		}

		private bool AllSecretsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt("AllSecretsForBoxRewardWindowIsShown_" + boXName, 0) == 1;
		}

		private void Awake()
		{
			RewardWindowBase.Shown += HandleRewardWindowShown;
			sharedScript = this;
			EventDelegate.Add(rewardWindow.hideButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(rewardWindow.continueButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(rewardWindow.collect.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(rewardWindow.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(rewardWindow.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(rewardWindowSurvival.hideButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(rewardWindowSurvival.continueButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(rewardWindowSurvival.collect.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(rewardWindowSurvival.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(rewardWindowSurvival.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			FacebookController.StoryPriority priority = FacebookController.StoryPriority.Red;
			rewardWindowSurvival.priority = priority;
			rewardWindowSurvival.twitterPriority = FacebookController.StoryPriority.ArenaLimit;
			rewardWindowSurvival.shareAction = delegate
			{
				FacebookController.PostOpenGraphStory("complete", "fight", priority, new Dictionary<string, string> { 
				{
					"map",
					Defs.SurvivalMaps[Defs.CurrentSurvMapIndex]
				} });
			};
			rewardWindowSurvival.HasReward = false;
			rewardWindowSurvival.twitterStatus = () => "I've beaten ARENA score in @PixelGun3D! Can you beat my record? #pixelgun3d #pixelgun #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
			rewardWindowSurvival.EventTitle = "Arena Survival";
		}

		private void HandleRewardWindowShown()
		{
			_numOfRewardWindowsShown++;
		}

		private static bool IsBox1Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("School");
		}

		private static bool IsBox2Completed()
		{
			return CurrentCampaignGame.levelSceneName.StartsWith("Gluk");
		}

		private static void PostBoxCompletedAchievement()
		{
			string text = string.Empty;
			string achievementName = string.Empty;
			bool num = IsBox1Completed();
			bool flag = IsBox2Completed();
			if (num)
			{
				text = ((BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer) ? "block_world_id" : "CgkIr8rGkPIJEAIQCA");
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					text = "Block_Survivor_id";
				}
				achievementName = "Block World Survivor";
			}
			else if (flag)
			{
				text = "CgkIr8rGkPIJEAIQCQ";
				achievementName = "Dragon Slayer";
			}
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogWarning("Achievement Box Completed: id is null. Scene: " + CurrentCampaignGame.levelSceneName);
			}
			else if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Social.ReportProgress(text, 100.0, delegate(bool success)
				{
					UnityEngine.Debug.LogFormat("Achievement {0} completed: {1}", achievementName, success);
				});
			}
		}

		private IEnumerator PlayGetCoinsClip()
		{
			float seconds = ((ExperienceController.sharedController != null) ? ExperienceController.sharedController.exp_1.length : 0.5f);
			yield return new WaitForSeconds(seconds);
			if (awardClip != null && Defs.isSoundFX)
			{
				NGUITools.PlaySound(awardClip);
			}
		}

		private static string EnglishNameForCompletedLevel(out CampaignLevel campaignLevel)
		{
			campaignLevel = LevelBox.GetLevelBySceneName(CurrentCampaignGame.levelSceneName);
			if (IsBox3Completed())
			{
				return "???";
			}
			if (campaignLevel == null || campaignLevel.localizeKeyForLevelMap == null)
			{
				return "FARM";
			}
			return (LocalizationStore.GetByDefault(campaignLevel.localizeKeyForLevelMap) ?? "FARM").Replace("\n", " ");
		}

		private void GiveAwardForCampaign()
		{
			int num = 0;
			int num2 = 0;
			if (_awardConferred || _hasAwardForMission)
			{
				num = Mathf.Min(InitializeCoinIndexBound(), _starCount);
				num *= ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
				num2 = 0;
				if (_awardConferred)
				{
					num2 = GemsToAddForBox();
					int num3 = CoinsToAddForBox();
					num += num3;
					PostBoxCompletedAchievement();
				}
				if (num > 0)
				{
					BankController.AddCoins(num);
				}
				if (num2 > 0)
				{
					BankController.AddGems(num2);
				}
			}
			int num4 = 0;
			if (_starCount == 3 && _oldStarCount < 3 && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 36)
			{
				num4 += 5;
			}
			if (_boxCompletionExperienceAward.HasValue && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 36)
			{
				num4 += _boxCompletionExperienceAward.Value;
			}
			num4 *= ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
			if (num4 != 0)
			{
				_experienceController.AddExperience(num4);
			}
			if (num > 0 || num2 > 0)
			{
				StartCoroutine(PlayGetCoinsClip());
				if (num2 > 0)
				{
					_shouldBlinkGemsIndicatorAfterRewardWindow = true;
				}
				if (num > 0)
				{
					_shouldBlinkCoinsIndicatorAfterRewardWindow = true;
				}
			}
			bool flag = _awardConferred && IsBox3Completed();
			CampaignLevel campaignLevel = null;
			string text = EnglishNameForCompletedLevel(out campaignLevel);
			string twitterStatus = string.Format("All enemies {0} {1} are defeated in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", new object[2] { campaignLevel.predlog, text });
			string eventTitle = "Level Complete";
			if (_isLastLevel)
			{
				if (IsBox1Completed())
				{
					twitterStatus = "I’ve defeated the RIDER in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 1 Complete";
				}
				else if (IsBox2Completed())
				{
					twitterStatus = "I’ve defeated the DRAGON in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 2 Complete";
				}
				else if (IsBox3Completed())
				{
					twitterStatus = "I’ve defeated the EVIL BUG in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 3 Complete";
				}
			}
			FacebookController.StoryPriority storyPriority = ((!_isLastLevel) ? FacebookController.StoryPriority.Red : FacebookController.StoryPriority.Green);
			rewardWindow.priority = storyPriority;
			rewardWindow.twitterStatus = () => twitterStatus;
			rewardWindow.EventTitle = eventTitle;
			rewardWindow.HasReward = true;
			rewardWindow.shareAction = delegate
			{
				FacebookController.PostOpenGraphStory(_isLastLevel ? "finish" : "complete", _isLastLevel ? "chapter" : "mission", storyPriority, _isLastLevel ? new Dictionary<string, string> { 
				{
					"chapter",
					CurrentCampaignGame.boXName
				} } : new Dictionary<string, string> { 
				{
					"mission",
					CurrentCampaignGame.levelSceneName
				} });
			};
			rewardSettings.normalBackground.SetActive(PremiumAccountController.Instance == null || !PremiumAccountController.Instance.isAccountActive);
			rewardSettings.premiumBackground.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
			foreach (UILabel item in rewardSettings.bossDefeatedHeader)
			{
				item.gameObject.SetActive(_awardConferred);
				if (_awardConferred)
				{
					if (IsBox1Completed())
					{
						item.text = LocalizationStore.Get("Key_1546");
					}
					else if (IsBox2Completed())
					{
						item.text = LocalizationStore.Get("Key_1547");
					}
					else if (IsBox3Completed())
					{
						item.text = LocalizationStore.Get("Key_1548");
					}
				}
			}
			foreach (UILabel boxCompletedLabel in rewardSettings.boxCompletedLabels)
			{
				boxCompletedLabel.gameObject.SetActive(_awardConferred);
				if (_awardConferred)
				{
					if (IsBox1Completed())
					{
						boxCompletedLabel.text = LocalizationStore.Get("Key_1549");
					}
					else if (IsBox2Completed())
					{
						boxCompletedLabel.text = LocalizationStore.Get("Key_1550");
					}
					else if (IsBox3Completed())
					{
						boxCompletedLabel.text = LocalizationStore.Get("Key_1551");
					}
				}
			}
			foreach (UILabel item2 in rewardSettings.missionHeader)
			{
				item2.gameObject.SetActive(!_awardConferred);
			}
			float num5 = (flag ? 0.8f : 1f);
			rewardSettings.coinsReward.gameObject.SetActive(num > 0);
			rewardSettings.coinsReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel coinsRewardLabel in rewardSettings.coinsRewardLabels)
			{
				coinsRewardLabel.text = "+" + num + " " + LocalizationStore.Get("Key_0275");
			}
			rewardSettings.coinsMultiplierContainer.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1 && !_awardConferred);
			foreach (UILabel coinsMultiplierLabel in rewardSettings.coinsMultiplierLabels)
			{
				coinsMultiplierLabel.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
			}
			rewardSettings.gemsReward.gameObject.SetActive(num2 > 0);
			rewardSettings.gemsReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel gemsRewrdLabel in rewardSettings.gemsRewrdLabels)
			{
				gemsRewrdLabel.text = "+" + num2 + " " + LocalizationStore.Get("Key_0951");
			}
			rewardSettings.gemsMultyplierContainer.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1 && !_awardConferred);
			foreach (UILabel gemsMultiplierLabel in rewardSettings.gemsMultiplierLabels)
			{
				gemsMultiplierLabel.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
			}
			rewardSettings.experienceReward.gameObject.SetActive(num4 > 0);
			rewardSettings.experienceReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel experienceRewardLabel in rewardSettings.experienceRewardLabels)
			{
				experienceRewardLabel.text = "+" + num4 + " " + LocalizationStore.Get("Key_0204");
			}
			rewardSettings.expMultiplier.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1);
			foreach (UILabel expMultiplierLabel in rewardSettings.expMultiplierLabels)
			{
				expMultiplierLabel.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
			}
			rewardSettings.badcode.gameObject.SetActive(flag);
			rewardSettings.badcode.localScale = new Vector3(num5, num5, num5);
			rewardSettings.grid.Reposition();
		}

		private void Start()
		{
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			QuestSystem.Instance.SaveQuestProgressIfDirty();
			if (GameConnect.isSurvival)
			{
				backgroundSurvivalTexture.SetActive(true);
			}
			else
			{
				backgroundTexture.SetActive(true);
				if (GameConnect.isSpeedrun)
				{
					string b = "Loading_Speedrun";
					Texture texture = Resources.Load<Texture>(ResPath.Combine(Switcher.LoadingInResourcesPath + (Device.isRetinaAndStrong ? "/Hi" : string.Empty), b));
					if (backgroundTexture != null && texture != null)
					{
						backgroundTexture.GetComponent<UITexture>().mainTexture = texture;
					}
				}
			}
			ActivityIndicator.IsActiveIndicator = false;
			if (LastGameResult == GameResult.Death)
			{
				_gameOver = true;
				LastGameResult = GameResult.None;
			}
			if (!_gameOver && GameConnect.isCampaign)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Completed";
				StoreKitEventListener.State.Parameters["Level"] = CurrentCampaignGame.levelSceneName + " Level Completed";
			}
			else if (_gameOver && GameConnect.isCampaign)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Failed";
				StoreKitEventListener.State.Parameters["Level"] = CurrentCampaignGame.levelSceneName + " Level Failed";
			}
			else if (!_gameOver && GameConnect.isSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Player quit";
				StoreKitEventListener.State.Parameters["Waves"] = StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")) + " Player quit";
			}
			else if (_gameOver && GameConnect.isSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Game over";
				StoreKitEventListener.State.Parameters["Waves"] = StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")) + " Game over";
			}
			_shouldShowFacebookButton = FacebookController.FacebookSupported;
			_shouldShowTwitterButton = TwitterController.TwitterSupported;
			_experienceController = InitializeExperienceController();
			BindButtonHandler(menuButton, HandleMenuButton);
			BindButtonHandler(retryButton, HandleRetryButton);
			BindButtonHandler(nextButton, HandleNextButton);
			BindButtonHandler(quitButton, HandleQuitButton);
			BindButtonHandler(facebookButton, HandleFacebookButton);
			BindButtonHandler(twitterButton, HandleTwitterButton);
			if (GameConnect.isSurvival || GameConnect.isSpeedrun)
			{
				shopView.ViewType = BankShopViewGuiElement.BankShopViewType.Bank;
				shopView.ShowTickets = true;
				shopView.UpdateView();
				shopView.Clicked += ShopView_Clicked;
			}
			else
			{
				shopView.Clicked += HandleShopButton;
			}
			if (GameConnect.isCampaign)
			{
				int num = -1;
				LevelBox levelBox = null;
				foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
				{
					if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
					{
						continue;
					}
					levelBox = campaignBox;
					for (int i = 0; i != campaignBox.levels.Count; i++)
					{
						if (campaignBox.levels[i].sceneName.Equals(CurrentCampaignGame.levelSceneName))
						{
							num = i;
							break;
						}
					}
					break;
				}
				if (levelBox != null)
				{
					_isLastLevel = num >= levelBox.levels.Count - 1;
					_nextSceneName = levelBox.levels[_isLastLevel ? num : (num + 1)].sceneName;
				}
				else
				{
					UnityEngine.Debug.LogError("Current box not found in the list of boxes!");
					_isLastLevel = true;
					_nextSceneName = SceneManager.GetActiveScene().name;
				}
				_oldStarCount = 0;
				_starCount = InitializeStarCount();
				if (!_gameOver)
				{
					if (WeaponManager.sharedManager != null)
					{
						WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
					}
					Dictionary<string, int> dictionary = CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName];
					if (!dictionary.ContainsKey(CurrentCampaignGame.levelSceneName))
					{
						completedFirstTime = true;
						if (_isLastLevel)
						{
							_boxCompletionExperienceAward = levelBox.CompletionExperienceAward;
						}
						dictionary.Add(CurrentCampaignGame.levelSceneName, _starCount);
						CampaignProgress.SaveCampaignProgress();
						try
						{
							string text = null;
							if (_isLastLevel)
							{
								text = (IsBox1Completed() ? "Box 1" : ((!IsBox2Completed()) ? "Box 3" : "Box 2"));
							}
							CampaignLevel campaignLevel;
							string text2 = EnglishNameForCompletedLevel(out campaignLevel);
							AnalyticsStuff.LogCampaign(text2, text);
							AnalyticsFacade.SendCustomEventToFacebook("campaign_level_reached", new Dictionary<string, object>
							{
								{ "level", text2 },
								{
									"box_level",
									string.Format("{0}: {1}", new object[2] { text, text2 })
								}
							});
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError("Exception in LogCampaign(LevelCompleteScript): " + ex);
						}
					}
					else
					{
						_oldStarCount = dictionary[CurrentCampaignGame.levelSceneName];
						dictionary[CurrentCampaignGame.levelSceneName] = Math.Max(_oldStarCount, _starCount);
						CampaignProgress.SaveCampaignProgress();
					}
					CampaignProgress.OpenNewBoxIfPossible();
					var rememberedAmmo = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().ToDictionary((Weapon w) => w.weaponPrefab.nameNoClone(), (Weapon w) => new
					{
						AmmoInClip = w.currentAmmoInClip,
						AmmoInBackpack = w.currentAmmoInBackpack
					});
					Action action = delegate
					{
						IEnumerable<Weapon> source2 = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>();
						foreach (var kvp in rememberedAmmo)
						{
							Weapon weapon = source2.FirstOrDefault((Weapon w) => w.weaponPrefab.nameNoClone() == kvp.Key);
							if (weapon != null)
							{
								weapon.currentAmmoInClip = kvp.Value.AmmoInClip;
								weapon.currentAmmoInBackpack = kvp.Value.AmmoInBackpack;
							}
						}
					};
					if (Application.platform == RuntimePlatform.IPhonePlayer)
					{
						action();
					}
					try
					{
						if (!AllStarsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							int num2 = dictionary.Values.ToList().Sum();
							_shouldShowAllStarsCollectedRewardWindow = num2 == LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels.Count * 3;
						}
						if (!AllSecretsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							List<string> source = LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels.Select((CampaignLevel level) => level.sceneName).ToList();
							HashSet<string> levelsWhereGotCoins = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Coin));
							HashSet<string> levelsWhereGotGems = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Gem));
							_shouldShowAllSecretsCollectedRewardWindow = source.All((string l) => levelsWhereGotCoins.Contains(l) && levelsWhereGotGems.Contains(l));
						}
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception);
					}
					_hasAwardForMission = _starCount > _oldStarCount && InitializeCoinIndexBound() > _oldStarCount;
				}
				_awardConferred = InitializeAwardConferred();
			}
			survivalResults.SetActive(false);
			quitButton.SetActive(false);
			if (!_gameOver)
			{
				if (GameConnect.isCampaign)
				{
					award1coinSprite.SetActive(true);
				}
				GameObject[] array = statisticLabels;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].SetActive(GameConnect.isSurvival);
				}
				if (_starCount > _oldStarCount)
				{
					CoinsMessage.FireCoinsAddedEvent();
				}
			}
			else
			{
				award1coinSprite.SetActive(false);
				nextButton.SetActive(false);
				checkboxSpritePrototype.SetActive(false);
				if (GameConnect.isCampaign && gameOverSprite != null)
				{
					gameOverSprite.SetActive(true);
				}
				GameObject[] array = statisticLabels;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].SetActive(GameConnect.isSurvival);
				}
				if (GameConnect.isCampaign)
				{
					float x = (retryButton.transform.position.x - menuButton.transform.position.x) / 2f;
					Vector3 vector = new Vector3(x, 0f, 0f);
					menuButton.transform.position = retryButton.transform.position - vector;
					retryButton.transform.position += vector;
				}
				menuButton.SetActive(GameConnect.isCampaign);
				if (GameConnect.isCampaign)
				{
					StartCoroutine(TryToShowExpiredBanner());
				}
			}
			if (GameConnect.isSurvival)
			{
				WeaponManager.sharedManager.Reset();
			}
			_instance = this;
			if ((GameConnect.isCampaign && (_awardConferred || _hasAwardForMission)) || (_starCount == 3 && _oldStarCount < 3 && !_gameOver && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 36))
			{
				mainInterface.SetActive(false);
				rewardWindow.gameObject.SetActive(true);
				GiveAwardForCampaign();
			}
			else if (GameConnect.isCampaign)
			{
				mainInterface.SetActive(true);
				rewardWindow.gameObject.SetActive(false);
				if (!_gameOver && brightStarPrototypeSprite != null && darkStarPrototypeSprite != null)
				{
					StartCoroutine(DisplayLevelResult());
				}
			}
			else if (GameConnect.isSurvival)
			{
				int num3 = CalculateExperienceAward(GlobalGameController.Score);
				if (num3 > 0)
				{
					_experienceController.AddExperience(num3 * ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff));
				}
				if (MiniGamesPlayerScoreManager.Instance.GetScore(GameConnect.GameMode.Arena) < GlobalGameController.Score)
				{
					MiniGamesPlayerScoreManager.Instance.SetScore(GameConnect.GameMode.Arena, GlobalGameController.Score);
					FriendsController.sharedController.SendScoreInMiniGames(12, GlobalGameController.Score);
				}
				if (GlobalGameController.HasSurvivalRecord)
				{
					GlobalGameController.HasSurvivalRecord = false;
					DisplaySurvivalResult();
				}
				else
				{
					DisplaySurvivalResult();
				}
			}
			else if (GameConnect.isSpeedrun)
			{
				DisplaySpeedrunResult();
			}
		}

		private void ShopView_Clicked(object sender, EventArgs e)
		{
			ShowBankWindow();
		}

		public void HideRewardWindow()
		{
			ButtonClickSound.TryPlayClick();
			mainInterface.SetActive(true);
			rewardWindow.gameObject.SetActive(false);
			if (GameConnect.isCampaign && brightStarPrototypeSprite != null && darkStarPrototypeSprite != null)
			{
				StartCoroutine(DisplayLevelResult());
			}
		}

		public void HideRewardWindowSurvival()
		{
			ButtonClickSound.TryPlayClick();
			mainInterface.SetActive(true);
			rewardWindowSurvival.gameObject.SetActive(false);
			DisplaySurvivalResult();
		}

		private void OnDestroy()
		{
			_instance = null;
			if (_experienceController != null)
			{
				_experienceController.isShowRanks = false;
			}
			PlayerPrefs.Save();
			RewardWindowBase.Shown -= HandleRewardWindowShown;
			sharedScript = null;
		}

		public void ShowBankWindow()
		{
			if (ShopNGUIController.GuiActive)
			{
				UnityEngine.Debug.LogWarning("ShowBankWindow ShopNGUIController.GuiActive != null");
				return;
			}
			if (BankController.Instance == null)
			{
				UnityEngine.Debug.LogWarning("ShowBankWindow bankController == null");
				return;
			}
			if (BankController.Instance.InterfaceEnabledCoroutineLocked)
			{
				UnityEngine.Debug.LogWarning("ShowBankWindow InterfaceEnabledCoroutineLocked");
				return;
			}
			BankController.Instance.BackRequested += HandleBackFromBankClicked;
			mainInterface.SetActiveSafeSelf(false);
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, "TicketsCurrency");
		}

		private void HandleBackFromBankClicked(object sender, EventArgs e)
		{
			if (ShopNGUIController.GuiActive)
			{
				UnityEngine.Debug.LogWarning("HandleBackFromBankClicked ShopNGUIController.GuiActive  != null");
				return;
			}
			if (BankController.Instance == null)
			{
				UnityEngine.Debug.LogWarning("HandleBackFromBankClicked bankController == null");
				return;
			}
			if (BankController.Instance.InterfaceEnabledCoroutineLocked)
			{
				UnityEngine.Debug.LogWarning("HandleBackFromBankClicked InterfaceEnabledCoroutineLocked");
				return;
			}
			BankController.Instance.BackRequested -= HandleBackFromBankClicked;
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
			mainInterface.SetActiveSafeSelf(true);
		}

		public static bool IsShowRewardWindow()
		{
			if (sharedScript == null)
			{
				return false;
			}
			bool num = sharedScript.rewardWindowSurvival != null && sharedScript.rewardWindowSurvival.gameObject != null && sharedScript.rewardWindowSurvival.gameObject.activeInHierarchy;
			bool flag = sharedScript.rewardWindow != null && sharedScript.rewardWindow.gameObject != null && sharedScript.rewardWindow.gameObject.activeInHierarchy;
			return num || flag;
		}

		private void Update()
		{
			if (_experienceController != null && BankController.Instance != null && !BankController.Instance.InterfaceEnabled && !ShopNGUIController.GuiActive)
			{
				_experienceController.isShowRanks = RentWindowPoint.childCount == 0 && !loadingPanel.activeSelf && (!(sharedScript != null) || !IsShowRewardWindow());
			}
			bool active = FacebookController.FacebookSupported && _shouldShowFacebookButton && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn;
			facebookButton.SetActive(active);
			twitterButton.SetActive(TwitterController.TwitterSupported && _shouldShowTwitterButton && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		}

		private void OnEnable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(delegate
			{
				HandleMenuButton(this, EventArgs.Empty);
			}, "Level Complete");
		}

		private void OnDisable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
				_backSubscription = null;
			}
		}

		private static void BindButtonHandler(GameObject button, EventHandler handler)
		{
			if (button != null)
			{
				ButtonHandler component = button.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += handler;
				}
			}
		}

		private static int CalculateExperienceAward(int score)
		{
			if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel == 36)
			{
				return 0;
			}
			int num = ((!Application.isEditor) ? 1 : 100);
			if (score < 15000 / num)
			{
				return 0;
			}
			if (score < 50000 / num)
			{
				return 10;
			}
			if (score < 100000 / num)
			{
				return 35;
			}
			if (score < 150000 / num)
			{
				return 50;
			}
			return 75;
		}

		private void DisplaySpeedrunResult()
		{
			try
			{
				DisplaySurvivalResultIsRunning = true;
				menuButton.SetActive(false);
				retryButton.SetActive(false);
				nextButton.SetActive(false);
				shopView.PopRequest();
				quitButton.SetActive(false);
				speedrunResultWindow.SetActive(true);
				speedrunCurrentScore.text = GlobalGameController.Score.ToString();
				int score = MiniGamesPlayerScoreManager.Instance.GetScore(GameConnect.GameMode.SpeedRun);
				bool active = false;
				if (score < GlobalGameController.Score)
				{
					MiniGamesPlayerScoreManager.Instance.SetScore(GameConnect.GameMode.SpeedRun, GlobalGameController.Score);
					FriendsController.sharedController.SendScoreInMiniGames(13, GlobalGameController.Score);
					score = GlobalGameController.Score;
					active = true;
				}
				speedrunBestScore.text = score.ToString();
				speedrunNewRecordLabel.SetActive(active);
				try
				{
					miniGamesRetryButton.SetActive(true);
					miniGameMatchPriceLabel.ForEach(delegate(UILabel label)
					{
						label.text = BalanceController.ParametersForMiniGameType(GameConnect.GameMode.SpeedRun).TicketsPrice.ToString();
					});
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in setting miniGameMatchPriceLabel: {0}", ex);
				}
				shopView.PushRequest();
				quitButton.SetActive(true);
				StartCoroutine(TryToShowExpiredBanner());
			}
			finally
			{
				DisplaySurvivalResultIsRunning = false;
			}
		}

		private void DisplaySurvivalResult()
		{
			try
			{
				DisplaySurvivalResultIsRunning = true;
				menuButton.SetActive(false);
				retryButton.SetActive(false);
				nextButton.SetActive(false);
				shopView.PopRequest();
				quitButton.SetActive(false);
				survivalResults.SetActive(true);
				try
				{
					miniGamesRetryButton.SetActive(true);
					miniGameMatchPriceLabel.ForEach(delegate(UILabel label)
					{
						label.text = BalanceController.ParametersForMiniGameType(GameConnect.GameMode.Arena).TicketsPrice.ToString();
					});
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in setting miniGameMatchPriceLabel: {0}", ex);
				}
				shopView.PushRequest();
				quitButton.SetActive(true);
				StartCoroutine(TryToShowExpiredBanner());
			}
			finally
			{
				DisplaySurvivalResultIsRunning = false;
			}
		}

		private static int InitializeCoinIndexBound()
		{
			return Defs.diffGame + 1;
		}

		private static bool IsBox3Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("Code_campaign3");
		}

		private int GemsToAddForBox()
		{
			int result = 0;
			if (IsBox1Completed())
			{
				result = LevelBox.campaignBoxes[0].gems;
			}
			else if (IsBox2Completed())
			{
				result = LevelBox.campaignBoxes[1].gems;
			}
			else if (IsBox3Completed())
			{
				result = LevelBox.campaignBoxes[2].gems;
			}
			return result;
		}

		private int CoinsToAddForBox()
		{
			int result = 0;
			if (IsBox1Completed())
			{
				result = LevelBox.campaignBoxes[0].coins;
			}
			else if (IsBox2Completed())
			{
				result = LevelBox.campaignBoxes[1].coins;
			}
			else if (IsBox3Completed())
			{
				result = LevelBox.campaignBoxes[2].coins;
			}
			return result;
		}

		private IEnumerator DisplayLevelResult()
		{
			try
			{
				DisplayLevelResultIsRunning = true;
				menuButton.SetActive(false);
				retryButton.SetActive(false);
				nextButton.SetActive(false);
				shopView.PopRequest();
				_shouldShowFacebookButton = false;
				_shouldShowTwitterButton = false;
				InitializeCoinIndexBound();
				List<GameObject> stars = new List<GameObject>(3);
				for (int i = 0; i != 3; i++)
				{
					float x = -140f + (float)i * 140f;
					GameObject gameObject = UnityEngine.Object.Instantiate(darkStarPrototypeSprite);
					gameObject.transform.parent = darkStarPrototypeSprite.transform.parent;
					gameObject.transform.localPosition = new Vector3(x, darkStarPrototypeSprite.transform.localPosition.y, 0f);
					gameObject.transform.localScale = darkStarPrototypeSprite.transform.localScale;
					gameObject.SetActive(true);
					stars.Add(gameObject);
				}
				int currentStarIndex = 0;
				int checkboxIndex = 0;
				while (checkboxIndex < 3)
				{
					int num;
					if ((checkboxIndex != 1 || CurrentCampaignGame.completeInTime) && (checkboxIndex != 2 || CurrentCampaignGame.withoutHits))
					{
						yield return new WaitForSeconds(0.7f);
						GameObject obj = UnityEngine.Object.Instantiate(brightStarPrototypeSprite);
						obj.transform.parent = brightStarPrototypeSprite.transform.parent;
						obj.transform.localPosition = stars[currentStarIndex].transform.localPosition;
						obj.transform.localScale = stars[currentStarIndex].transform.localScale;
						obj.SetActive(true);
						UnityEngine.Object.Destroy(stars[currentStarIndex]);
						GameObject obj2 = UnityEngine.Object.Instantiate(checkboxSpritePrototype);
						obj2.transform.parent = checkboxSpritePrototype.transform.parent;
						obj2.transform.localPosition = new Vector3(checkboxSpritePrototype.transform.localPosition.x, checkboxSpritePrototype.transform.localPosition.y - 45f * (float)checkboxIndex, checkboxSpritePrototype.transform.localPosition.z);
						obj2.transform.localScale = checkboxSpritePrototype.transform.localScale;
						obj2.SetActive(true);
						if (starClips != null && currentStarIndex < starClips.Length && starClips[currentStarIndex] != null && Defs.isSoundFX)
						{
							NGUITools.PlaySound(starClips[currentStarIndex]);
						}
						num = currentStarIndex + 1;
						currentStarIndex = num;
					}
					num = checkboxIndex + 1;
					checkboxIndex = num;
				}
				UnityEngine.Object.Destroy(brightStarPrototypeSprite);
				UnityEngine.Object.Destroy(darkStarPrototypeSprite);
				menuButton.SetActive(true);
				retryButton.SetActive(true);
				nextButton.SetActive(true);
				shopView.PushRequest();
				_shouldShowFacebookButton = FacebookController.FacebookSupported;
				_shouldShowTwitterButton = TwitterController.TwitterSupported;
				if (_shouldBlinkCoinsIndicatorAfterRewardWindow)
				{
					CoinsMessage.FireCoinsAddedEvent();
				}
				if (_shouldBlinkGemsIndicatorAfterRewardWindow)
				{
					CoinsMessage.FireCoinsAddedEvent(true);
				}
				yield return new WaitForSeconds(0.7f);
				StartCoroutine(TryToShowExpiredBanner());
			}
			finally
			{
				DisplayLevelResultIsRunning = false;
			}
		}

		private static string BoxNameForTwitter()
		{
			return boxNamesTwitter[CurrentCampaignGame.boXName];
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
					if (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ExpController.Instance != null && ExpController.Instance.WaitingForLevelUpView) || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || loadingPanel.activeInHierarchy || ExchangeWindow.IsOpened || RentWindowPoint.childCount != 0)
					{
						continue;
					}
					if (_shouldShowAllStarsCollectedRewardWindow && _numOfRewardWindowsShown < 2)
					{
						_shouldShowAllStarsCollectedRewardWindow = false;
						PlayerPrefs.SetInt("AllStarsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
						if ((!FacebookController.FacebookSupported && !TwitterController.TwitterSupported) || Device.isPixelGunLow)
						{
							continue;
						}
						GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/AllStarsNGUI"));
						RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
						FacebookController.StoryPriority priority2 = FacebookController.StoryPriority.Green;
						component.priority = priority2;
						component.shareAction = delegate
						{
							FacebookController.PostOpenGraphStory("get", "star", priority2, new Dictionary<string, string> { 
							{
								"chapter",
								CurrentCampaignGame.boXName
							} });
						};
						component.HasReward = false;
						component.twitterStatus = () => string.Format("I've got all the stars in {0} in @PixelGun3D! Play now and try to get them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", new object[1] { BoxNameForTwitter() });
						component.EventTitle = "All Stars";
						foreach (UILabel headerLabel in gameObject.GetComponent<AllStarsRewardSettings>().headerLabels)
						{
							if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
							{
								headerLabel.text = LocalizationStore.Get("Key_1543");
							}
							else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
							{
								headerLabel.text = LocalizationStore.Get("Key_1544");
							}
							else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
							{
								headerLabel.text = LocalizationStore.Get("Key_1545");
							}
						}
						gameObject.transform.parent = RentWindowPoint;
						Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
						gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
						continue;
					}
					if (_shouldShowAllSecretsCollectedRewardWindow && _numOfRewardWindowsShown < 2)
					{
						_shouldShowAllSecretsCollectedRewardWindow = false;
						PlayerPrefs.SetInt("AllSecretsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
						if ((!FacebookController.FacebookSupported && !TwitterController.TwitterSupported) || Device.isPixelGunLow)
						{
							continue;
						}
						GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/AllSecretsNGUI"));
						RewardWindowBase component2 = gameObject2.GetComponent<RewardWindowBase>();
						FacebookController.StoryPriority priority = FacebookController.StoryPriority.Green;
						component2.priority = priority;
						component2.shareAction = delegate
						{
							FacebookController.PostOpenGraphStory("find", "secret", priority, new Dictionary<string, string> { 
							{
								"chapter",
								CurrentCampaignGame.boXName
							} });
						};
						component2.HasReward = false;
						component2.twitterStatus = () => string.Format("I've found all coins in {0} in @PixelGun3D! Play now and try to find them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", new object[1] { BoxNameForTwitter() });
						component2.EventTitle = "All Coins";
						foreach (UILabel headerLabel2 in gameObject2.GetComponent<AllSecretsRewardSettings>().headerLabels)
						{
							if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
							{
								headerLabel2.text = LocalizationStore.Get("Key_1540");
							}
							else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
							{
								headerLabel2.text = LocalizationStore.Get("Key_1541");
							}
							else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
							{
								headerLabel2.text = LocalizationStore.Get("Key_1542");
							}
						}
						gameObject2.transform.parent = RentWindowPoint;
						Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("Default"));
						gameObject2.transform.localPosition = new Vector3(0f, 0f, -130f);
						gameObject2.transform.localRotation = Quaternion.identity;
						gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
						continue;
					}
					if (Storager.getInt(Defs.PremiumEnabledFromServer) == 1)
					{
						ShopNGUIController.ShowPremimAccountExpiredIfPossible(RentWindowPoint, "Default");
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("exception in LevelComplete  TryToShowExpiredBanner: " + ex);
				}
			}
		}

		private void HandleMenuButton(object sender, EventArgs args)
		{
			if (_shopInstance != null)
			{
				return;
			}
			string reasonToDismissInterstitialCampaign = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialCampaign(LastGameResult == GameResult.Death);
			if (string.IsNullOrEmpty(reasonToDismissInterstitialCampaign))
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("<color=magenta>HandleMenuButton()</color>");
				}
				new LevelCompleteInterstitialRunner().Run();
			}
			else
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing interstitial. {0}</color>" : "Dismissing interstitial. {0}", reasonToDismissInterstitialCampaign);
			}
			string sceneName = ((!GameConnect.isCampaign) ? Defs.MainMenuScene : "ChooseLevel");
			Singleton<SceneLoader>.Instance.LoadScene(sceneName);
		}

		private void HandleQuitButton(object sender, EventArgs args)
		{
			string reasonToDismissInterstitialSurvivalArena = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialSurvivalArena(LastGameResult == GameResult.Death);
			if (string.IsNullOrEmpty(reasonToDismissInterstitialSurvivalArena))
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("<color=magenta>HandleQuitButton()</color>");
				}
				new LevelCompleteInterstitialRunner().Run();
			}
			else
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing interstitial. {0}</color>" : "Dismissing interstitial. {0}", reasonToDismissInterstitialSurvivalArena);
			}
			ActivityIndicator.IsActiveIndicator = true;
			loadingPanel.SetActive(true);
			mainPanel.SetActive(false);
			ExperienceController.sharedController.isShowRanks = false;
			Invoke("QuitLevel", 0.1f);
		}

		private void QuitLevel()
		{
			Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.MainMenuScene);
		}

		private static void SetInitialAmmoForAllGuns()
		{
			foreach (Weapon allAvailablePlayerWeapon in WeaponManager.sharedManager.allAvailablePlayerWeapons)
			{
				WeaponSounds component = allAvailablePlayerWeapon.weaponPrefab.GetComponent<WeaponSounds>();
				if (allAvailablePlayerWeapon.currentAmmoInClip + allAvailablePlayerWeapon.currentAmmoInBackpack < component.InitialAmmoWithEffectsApplied + component.ammoInClip)
				{
					allAvailablePlayerWeapon.currentAmmoInClip = component.ammoInClip;
					allAvailablePlayerWeapon.currentAmmoInBackpack = component.InitialAmmoWithEffectsApplied;
				}
				else if (allAvailablePlayerWeapon.currentAmmoInClip < component.ammoInClip)
				{
					int num = Mathf.Min(component.ammoInClip - allAvailablePlayerWeapon.currentAmmoInClip, allAvailablePlayerWeapon.currentAmmoInBackpack);
					allAvailablePlayerWeapon.currentAmmoInClip += num;
					allAvailablePlayerWeapon.currentAmmoInBackpack -= num;
				}
			}
		}

		public void HandleMiniGamesRetry()
		{
			if (_shopInstance != null || (!GameConnect.isSurvival && !GameConnect.isSpeedrun))
			{
				return;
			}
			int ticketsPrice = BalanceController.ParametersForMiniGameType(GameConnect.gameMode).TicketsPrice;
			ItemPrice price = new ItemPrice(ticketsPrice, "TicketsCurrency");
			Action onSuccess = delegate
			{
				try
				{
					AnalyticsStuff.MiniGames(GameConnect.gameMode);
					WeaponManager.sharedManager.Reset();
					GlobalGameController.Score = 0;
					Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, Defs.SurvivalMaps.Length);
					Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading");
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in RandomRoomClickBtnInHunger: {0}", ex);
				}
			};
			ShopNGUIController.TryToBuy(mainInterface, price, onSuccess, null, null, null, null, null, false, false, delegate
			{
				AnalyticsStuff.TicketsSpended(GameConnect.gameMode.ToString(), ticketsPrice);
			});
		}

		private void HandleRetryButton(object sender, EventArgs args)
		{
			if (!(_shopInstance != null))
			{
				SetInitialAmmoForAllGuns();
				GlobalGameController.Score = 0;
				Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading");
			}
		}

		private void HandleFacebookButton(object sender, EventArgs args)
		{
			if (!(_shopInstance != null))
			{
				FacebookController.ShowPostDialog();
			}
		}

		private void HandleTwitterButton(object sender, EventArgs args)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("Send Twitter: " + _SocialMessage());
			}
			else if (!(_shopInstance != null) && TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate(_SocialMessage());
			}
		}

		private void HandleNextButton(object sender, EventArgs args)
		{
			if (!(_shopInstance != null))
			{
				if (!_isLastLevel)
				{
					CurrentCampaignGame.levelSceneName = _nextSceneName;
					SetInitialAmmoForAllGuns();
					LevelArt.endOfBox = false;
					Singleton<SceneLoader>.Instance.LoadScene(LevelArt.ShouldShowArts ? "LevelArt" : "CampaignLoading");
				}
				else
				{
					LevelArt.endOfBox = true;
					Singleton<SceneLoader>.Instance.LoadScene(LevelArt.ShouldShowArts ? "LevelArt" : "ChooseLevel");
				}
			}
		}

		private void GoToChooseLevel()
		{
			Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel");
		}

		private void HandleShopButton(object sender, EventArgs args)
		{
			if (_shopInstance != null)
			{
				return;
			}
			_shopInstance = ShopNGUIController.sharedShop;
			if (_shopInstance != null)
			{
				_shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				if (shopButtonSound != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(shopButtonSound);
				}
				if (GameConnect.isSurvival)
				{
					backgroundSurvivalTexture.SetActive(false);
				}
				else
				{
					backgroundTexture.SetActive(false);
				}
				_shopInstance.resumeAction = HandleResumeFromShop;
			}
			quitButton.SetActive(false);
		}

		private void HandleResumeFromShop()
		{
			if (!(_shopInstance == null))
			{
				ShopNGUIController.GuiActive = false;
				_shopInstance.resumeAction = delegate
				{
				};
				_shopInstance = null;
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
				quitButton.SetActive(GameConnect.isSurvival);
				if (_experienceController != null)
				{
					_experienceController.isShowRanks = true;
				}
				if (GameConnect.isSurvival)
				{
					backgroundSurvivalTexture.SetActive(true);
				}
				else
				{
					backgroundTexture.SetActive(true);
				}
			}
		}

		private static ExperienceController InitializeExperienceController()
		{
			ExperienceController experienceController = null;
			GameObject gameObject = GameObject.FindGameObjectWithTag("ExperienceController");
			if (gameObject != null)
			{
				experienceController = gameObject.GetComponent<ExperienceController>();
			}
			if (experienceController == null)
			{
				UnityEngine.Debug.LogError("Cannot find experience controller.");
			}
			else
			{
				experienceController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
				experienceController.isShowRanks = true;
				if (ExpController.Instance != null)
				{
					ExpController.Instance.InterfaceEnabled = true;
				}
			}
			return experienceController;
		}

		private static int InitializeStarCount()
		{
			int num = 1;
			if (CurrentCampaignGame.completeInTime)
			{
				num++;
			}
			if (CurrentCampaignGame.withoutHits)
			{
				num++;
			}
			return num;
		}

		private bool InitializeAwardConferred()
		{
			if (_isLastLevel)
			{
				return completedFirstTime;
			}
			return false;
		}

		private string _SocialMessage()
		{
			if (GameConnect.isSurvival)
			{
				string text = string.Format(CultureInfo.InvariantCulture, LocalizationStore.GetByDefault("Key_1382"), WavesSurvivedStat.SurvivedWaveCount, PlayerPrefs.GetInt(Defs.KilledZombiesSett, 0), GlobalGameController.Score);
				UnityEngine.Debug.Log(text);
				return text;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(CurrentCampaignGame.levelSceneName);
			if (infoScene != null)
			{
				if (!_gameOver)
				{
					string text2 = string.Format(LocalizationStore.GetByDefault("Key_1382"), new object[2] { infoScene.TranslateName, _starCount });
					UnityEngine.Debug.Log(text2);
					return text2;
				}
				string text3 = string.Format(LocalizationStore.GetByDefault("Key_1380"), new object[1] { infoScene.TranslateName });
				UnityEngine.Debug.Log(text3);
				return text3;
			}
			return "error map";
		}

		public static void SetInputEnabled(bool enabled)
		{
			if (_instance != null)
			{
				_instance.uiCamera.enabled = enabled;
			}
		}
	}
}
