using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
	public enum StoryPriority
	{
		Green = 0,
		Red = 1,
		MultyWinLimit = 2,
		ArenaLimit = 3
	}

	internal sealed class StoryPriorityComparer : IEqualityComparer<StoryPriority>
	{
		public bool Equals(StoryPriority x, StoryPriority y)
		{
			return x == y;
		}

		public int GetHashCode(StoryPriority obj)
		{
			return (int)obj;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CRunActionAfterDelay_003Ed__66 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003Ci_003E5__1;

		public Action action;

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
		public _003CRunActionAfterDelay_003Ed__66(int _003C_003E1__state)
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
			if (_003Ci_003E5__1 < 30)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (action != null)
			{
				action();
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
	internal sealed class _003C_003Ec__DisplayClass78_0
	{
		public bool success;

		public Action<IDictionary<string, object>> _003C_003E9__0;

		internal void _003CGetOurFbId_003Eb__0(IDictionary<string, object> dict)
		{
			try
			{
				Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
				if (receivedSelfID != null)
				{
					receivedSelfID((string)dict["id"]);
				}
				success = true;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("FacebookController GetOurFbId exception: " + ex);
			}
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetOurFbId_003Ed__78 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private _003C_003Ec__DisplayClass78_0 _003C_003E8__1;

		private float _003CstartTime_003E5__2;

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
		public _003CGetOurFbId_003Ed__78(int _003C_003E1__state)
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass78_0();
				if (!FacebookSupported)
				{
					return false;
				}
				goto IL_004d;
			case 1:
				_003C_003E1__state = -1;
				goto IL_004d;
			case 2:
				{
					_003C_003E1__state = -1;
					if (Time.realtimeSinceStartup - _003CstartTime_003E5__2 < 30f)
					{
						goto IL_00ac;
					}
					goto IL_00d6;
				}
				IL_004d:
				if (FriendsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E8__1.success = false;
				goto IL_00d6;
				IL_00ac:
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
				IL_00d6:
				if (!FB.IsLoggedIn || _003C_003E8__1.success)
				{
					break;
				}
				FBGet("/me", delegate(IDictionary<string, object> dict)
				{
					try
					{
						Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
						if (receivedSelfID != null)
						{
							receivedSelfID((string)dict["id"]);
						}
						_003C_003E8__1.success = true;
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("FacebookController GetOurFbId exception: " + ex);
					}
				});
				_003CstartTime_003E5__2 = Time.realtimeSinceStartup;
				goto IL_00ac;
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

	public const int MaxCountShownGunForLogin = 1;

	public static FacebookController sharedController;

	public string selfID = "";

	private Action<string, object> handlerForPost;

	private bool hasPublishActions;

	private bool isGetPermitionFromSendMessage;

	private string postMessage;

	public List<Friend> friendsList;

	private string titleInvite = "Invite a Friend to Play!";

	private string messageInvite = "Join me in playing a new game!";

	public const int DefaultGreenPriorityLimit = 7;

	public const int DefaultRedPriorityLimit = 3;

	public static readonly Dictionary<StoryPriority, int> StoryPostLimits = new Dictionary<StoryPriority, int>(2, new StoryPriorityComparer())
	{
		{
			StoryPriority.Green,
			7
		},
		{
			StoryPriority.Red,
			3
		}
	};

	private const string PriorityKey = "priority";

	private const string TimeKey = "time";

	private string _appId = string.Empty;

	private bool socialGunEventActive;

	private float _timeSinceLastStoryPostHistoryClean;

	private const string StoryPostHistoryKey = "FacebookControllerStoryPostHistoryKey";

	public const string FacebookScriptAddress = "https://secure.pixelgunserver.com/fb/ogobjects.php";

	public bool InvitePlayerApiIsRunning;

	private TimeSpan DurationSocialGunEvent = TimeSpan.FromDays(1000000.0);

	private TimeSpan TimeBetweenSocialGunBannerSeries = TimeSpan.FromHours(24.0);

	private DateTime socialEventStartTime;

	private const string SocialGunEventStartedKey = "FacebookControllerSocialGunEventStartedKey";

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	public static bool FacebookSupported
	{
		get
		{
			return false;
		}
	}

	public string AppId
	{
		get
		{
			return _appId;
		}
		set
		{
			_appId = value ?? string.Empty;
		}
	}

	public static bool FacebookPost_Old_Supported
	{
		get
		{
			if (FacebookSupported)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || !(sharedController != null) || sharedController.CanPostStoryWithPriority(StoryPriority.Green))
				{
					if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && Defs.isMulti && PhotonNetwork.connected)
					{
						return NetworkStartTable.LocalOrPasswordRoom();
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool SocialGunEventActive
	{
		get
		{
			return socialGunEventActive;
		}
	}

	public static bool LoggingIn { get; private set; }

	public static event Action<bool> SocialGunEventStateChanged;

	public static event Action<string> PostCompleted;

	public static event Action<string> ReceivedSelfID;

	public bool CanPostStoryWithPriority(StoryPriority priority)
	{
		try
		{
			if (priority == StoryPriority.Green)
			{
				return storiesPostHistory.Count < StoryPostLimits[priority];
			}
			return storiesPostHistory.Where((Dictionary<string, object> rec) => int.Parse((string)rec["priority"]) == (int)priority).Count() < StoryPostLimits[priority];
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Caught exception in CanPostStoryWithPriority:\n{0}", ex);
		}
		return false;
	}

	public string GetTimeToEndSocialGunEvent()
	{
		if (!SocialGunEventActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = socialEventStartTime + DurationSocialGunEvent - DateTime.UtcNow;
		return string.Format("{0:00}:{1:00}:{2:00}", new object[3] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
	}

	public bool IsNeedShowGunFroLoginWindow()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin");
		if (socialGunEventActive && @int < 1 && SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			return Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0;
		}
		return false;
	}

	public void UpdateCountShownWindowByShowCondition()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin");
		Storager.setString(Defs.LastTimeShowSocialGun, DateTime.UtcNow.ToString("s"));
		Storager.setInt("FacebookController.CountShownGunForLogin", @int + 1);
	}

	private void UpdateCountShownSocialGunWindowByTimeCondition()
	{
		if (!FacebookSupported)
		{
			return;
		}
		string text = string.Empty;
		if (!Storager.hasKey(Defs.LastTimeShowSocialGun))
		{
			Storager.setString(Defs.LastTimeShowSocialGun, text);
		}
		else
		{
			text = Storager.getString(Defs.LastTimeShowSocialGun);
		}
		if (!string.IsNullOrEmpty(text))
		{
			DateTime result = default(DateTime);
			if (DateTime.TryParse(text, out result) && DateTime.UtcNow - result >= TimeBetweenSocialGunBannerSeries)
			{
				Storager.setInt("FacebookController.CountShownGunForLogin", 0);
			}
		}
	}

	private bool CurrentSocialGunEventState()
	{
		if (FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0 && DateTime.UtcNow - socialEventStartTime < DurationSocialGunEvent && ExpController.LobbyLevel > 1)
		{
			return !MainMenuController.SavedShwonLobbyLevelIsLessThanActual();
		}
		return false;
	}

	private void Awake()
	{
		friendsList = new List<Friend>();
		if (!FacebookSupported)
		{
			return;
		}
		FriendsController.NewFacebookLimitsAvailable += HandleNewFacebookLimitsAvailable;
		string empty = string.Empty;
		if (!Storager.hasKey("FacebookControllerSocialGunEventStartedKey"))
		{
			empty = DateTime.UtcNow.ToString("s");
			Storager.setString("FacebookControllerSocialGunEventStartedKey", empty);
		}
		else
		{
			empty = Storager.getString("FacebookControllerSocialGunEventStartedKey");
			DateTime result = default(DateTime);
			if (!DateTime.TryParse(empty, out result))
			{
				empty = DateTime.UtcNow.ToString("s");
				Storager.setString("FacebookControllerSocialGunEventStartedKey", empty);
			}
		}
		if (DateTime.TryParse(empty, out socialEventStartTime))
		{
			socialGunEventActive = CurrentSocialGunEventState();
		}
		else
		{
			UnityEngine.Debug.LogError("FacebookController: invalid timeStartEvent");
		}
	}

	private void HandleNewFacebookLimitsAvailable(int greenLimit, int redLimit)
	{
		StoryPostLimits[StoryPriority.Green] = greenLimit;
		StoryPostLimits[StoryPriority.Red] = redLimit;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		sharedController = this;
		if (FacebookSupported)
		{
			InitFacebook();
		}
		InitStoryPostHistoryKey();
		LoadStoryPostHistory();
		UpdateCountShownSocialGunWindowByTimeCondition();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveStoryPostHistory();
			return;
		}
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		LoadStoryPostHistory();
		UpdateCountShownSocialGunWindowByTimeCondition();
	}

	private void OnDestroy()
	{
		SaveStoryPostHistory();
		if (FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable -= HandleNewFacebookLimitsAvailable;
		}
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("FacebookControllerStoryPostHistoryKey", Json.Serialize(storiesPostHistory));
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> obj = Json.Deserialize(Storager.getString("FacebookControllerStoryPostHistoryKey")) as List<object>;
			storiesPostHistory.Clear();
			foreach (object item2 in obj)
			{
				Dictionary<string, object> item = item2 as Dictionary<string, object>;
				storiesPostHistory.Add(item);
			}
			CleanStoryPostHistory();
		}
		catch (Exception)
		{
			storiesPostHistory.Clear();
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("FacebookControllerStoryPostHistoryKey"))
		{
			Storager.setString("FacebookControllerStoryPostHistoryKey", "[]");
		}
	}

	private void CleanStoryPostHistory()
	{
		_timeSinceLastStoryPostHistoryClean = Time.realtimeSinceStartup;
		try
		{
			long num = 86400L;
			long minimumValidTime = PromoActionsManager.CurrentUnixTime - num;
			storiesPostHistory.RemoveAll((Dictionary<string, object> entry) => long.Parse((string)entry["time"]) < minimumValidTime);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exeption in CleanStoryPostHistory:\n" + ex);
		}
	}

	private void Update()
	{
		bool flag = CurrentSocialGunEventState();
		if (socialGunEventActive != flag)
		{
			socialGunEventActive = flag;
			Action<bool> socialGunEventStateChanged = FacebookController.SocialGunEventStateChanged;
			if (socialGunEventStateChanged != null)
			{
				socialGunEventStateChanged(SocialGunEventActive);
			}
		}
		if (Time.realtimeSinceStartup - _timeSinceLastStoryPostHistoryClean > 10f)
		{
			CleanStoryPostHistory();
		}
		if (FacebookSupported && !FB.IsLoggedIn && FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id_fb))
		{
			Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
			if (receivedSelfID != null)
			{
				receivedSelfID(string.Empty);
			}
		}
	}

	public static void LogEvent(string eventName, Dictionary<string, object> parameters = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		try
		{
			//FB.LogAppEvent(eventName, null, parameters);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void ShowPostDialog()
	{
		if (FacebookSupported)
		{
			PlayerPrefs.SetInt("PostFacebookCount", PlayerPrefs.GetInt("PostFacebookCount", 0) + 1);
			PlayerPrefs.Save();
			if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				LogEvent("Active_loyal_users_payed");
				PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
			}
			if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				LogEvent("Active_loyal_users");
				PlayerPrefs.SetInt("Active_loyal_users_send", 1);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"link",
					Defs2.ApplicationUrl
				},
				{ "name", "Pixel Gun 3D" },
				{ "picture", "http://pixelgun3d.com/iconforpost.png" },
				{ "caption", "I've just played the super battle in Pixel Gun 3D :)" },
				{ "description", "DOWNLOAD IT FOR FREE AND JOIN ME NOW!" }
			};
			FB.FeedShare("", new Uri((string)dictionary["link"]), (string)dictionary["name"], picture: new Uri((string)dictionary["picture"]), linkCaption: (string)dictionary["caption"], linkDescription: (string)dictionary["description"]);
		}
	}

	public void PostMessage(string _message, Action<string, object> _completionHandler)
	{
		UnityEngine.Debug.Log("Post to Facebook");
	}

	public static void PrintFBResult(IResult result)
	{
	}

	public static void FBGet(string graphPath, Action<IDictionary<string, object>> act, Action<IResult> onError = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.GET, delegate(IGraphResult result)
		{
			try
			{
				PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception ex)
			{
				if (onError != null)
				{
					onError(result);
				}
				UnityEngine.Debug.LogError("Exception FBGet: " + ex);
			}
		});
	}

	internal static void CheckAndGiveFacebookReward(string context)
	{
		if (FacebookSupported)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0 && Storager.getInt(Defs.FacebookRewardGainStarted) == 1 && FB.IsLoggedIn)
			{
				Storager.setInt(Defs.FacebookRewardGainStarted, 0);
				Storager.setInt(Defs.IsFacebookLoginRewardaGained, 1);
				BankController.AddGems(10);
				TutorialQuestManager.Instance.AddFulfilledQuest("loginFacebook");
				QuestMediator.NotifySocialInteraction("loginFacebook");
				ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.SkinsCategory, "super_socialman", 1, false, 0, null, null, false);
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object> { { "Login Facebook", context } });
				AnalyticsFacade.SendCustomEventToAppsFlyer("Virality", new Dictionary<string, string> { { "Login Facebook", context } });
				WeaponManager.AddExclusiveWeaponToWeaponStructures(WeaponManager.SocialGunWN);
			}
		}
	}

	public static void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown", Action onSuccessAfterPublishPermissions = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 1);
		}
		LoggingIn = true;
		try
		{
			FB.LogInWithReadPermissions(new List<string> { "user_friends" }, delegate(ILoginResult result)
			{
				LoggingIn = false;
				PrintFBResult(result);
				CheckAndGiveFacebookReward(context);
				if (FB.IsLoggedIn)
				{
					if (onSuccess != null)
					{
						onSuccess();
					}
					try
					{
						Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
						if (receivedSelfID != null)
						{
							receivedSelfID((string)result.ResultDictionary["user_id"]);
						}
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogError("FacebookController Login ReceivedSelfID exception: " + ex2);
					}
					try
					{
						if (sharedController != null)
						{
							sharedController.InputFacebookFriends();
						}
					}
					catch (Exception ex3)
					{
						UnityEngine.Debug.LogError("FacebookController Login InputFacebookFriends exception: " + ex3);
					}
					CoroutineRunner.Instance.StartCoroutine(RunActionAfterDelay(delegate
					{
						LoggingIn = true;
						try
						{
							FB.LogInWithPublishPermissions(new List<string> { "publish_actions" }, delegate(ILoginResult publishLoginResult)
							{
								LoggingIn = false;
								PrintFBResult(publishLoginResult);
								if (string.IsNullOrEmpty(publishLoginResult.Error) && !publishLoginResult.Cancelled && onSuccessAfterPublishPermissions != null)
								{
									onSuccessAfterPublishPermissions();
								}
							});
						}
						catch (Exception ex4)
						{
							UnityEngine.Debug.LogError("Exception in Facebook Login: " + ex4);
							LoggingIn = false;
						}
					}));
				}
				else if (onFailure != null)
				{
					onFailure();
				}
			});
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in Facebook Login: " + ex);
			LoggingIn = false;
		}
	}

	private static IEnumerator RunActionAfterDelay(Action action)
	{
		for (int i = 0; i < 30; i++)
		{
			yield return null;
		}
		if (action != null)
		{
			action();
		}
	}

	private static void RegisterStoryPostedWithPriority(StoryPriority priority)
	{
		if (!(sharedController == null))
		{
			sharedController.RegisterStoryPostedWithPriorityCore(priority);
		}
	}

	private void RegisterStoryPostedWithPriorityCore(StoryPriority priority)
	{
		if (FacebookSupported)
		{
			List<Dictionary<string, object>> list = storiesPostHistory;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int num = (int)priority;
			dictionary.Add("priority", num.ToString());
			dictionary.Add("time", PromoActionsManager.CurrentUnixTime.ToString());
			list.Add(dictionary);
		}
	}

	public static void PostOpenGraphStory(string action, string obj, StoryPriority priority, Dictionary<string, string> pars = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		RunApiWithAskForPermissions(delegate
		{
			string text = "https://secure.pixelgunserver.com/fb/ogobjects.php?type=" + WWW.EscapeURL(obj);
			if (pars != null)
			{
				foreach (KeyValuePair<string, string> par in pars)
				{
					text = text + "&" + par.Key.Replace(" "[0], "_"[0]) + "=" + WWW.EscapeURL(par.Value);
				}
			}
			FBPost("/me/pixelgun:" + action, new Dictionary<string, string>
			{
				{ obj, text },
				{ "fb:explicitly_shared", "true" }
			}, delegate
			{
			}, delegate(IResult result)
			{
				if (result != null && result.Error == null)
				{
					RegisterStoryPostedWithPriority(priority);
				}
			});
		}, "publish_actions");
	}

	public static void FBPost(string graphPath, Dictionary<string, string> pars, Action<IDictionary<string, object>> act, Action<IResult> actionWithFBResult = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.POST, delegate(IGraphResult result)
		{
			try
			{
				if (actionWithFBResult != null)
				{
					actionWithFBResult(result);
				}
				PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception FBPost: " + ex);
			}
		}, pars);
	}

	public void SetMyId(Action onSuccess = null, bool dontRelogin = false)
	{
		if (!FacebookSupported)
		{
			return;
		}
		RunApiWithAskForPermissions(delegate
		{
			FBGet("/me/friends?fields=id,name,installed&limit=1000000", delegate(IDictionary<string, object> result)
			{
				IList list = result["data"] as IList;
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Result facebook friends" + result.ToString());
				}
				friendsList.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					IDictionary dictionary = list[i] as IDictionary;
					friendsList.Add(new Friend(dictionary["name"] as string, dictionary["id"].ToString(), dictionary["installed"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase)));
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			});
		}, "user_friends", dontRelogin);
	}

	private void InitFacebook()
	{
		if (!FacebookSupported)
		{
			return;
		}
		InitDelegate onInitComplete = delegate
		{
			try
			{
				FB.ActivateApp();
				if (FB.IsLoggedIn)
				{
					StartCoroutine(GetOurFbId());
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogException(ex2);
				UnityEngine.Debug.LogError("[Rilisoft] Exception in onInitComplete calback of FB.Init() method. Stacktrace: " + ex2.StackTrace);
			}
		};
		try
		{
			FB.Init(onInitComplete, delegate
			{
			});
		}
		catch (NotImplementedException ex)
		{
			UnityEngine.Debug.LogWarningFormat("Catching exception during FB.Init(): {0}", ex.Message);
		}
	}

	public static void RunApiWithAskForPermissions(Action runApi, string requiredPermission = "", bool dontRelogin = false, Action onFailToRunApi = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		if (dontRelogin)
		{
			if (FB.IsLoggedIn && runApi != null)
			{
				runApi();
			}
			return;
		}
		int countTryingUpdateToken = 0;
		Action<bool> loginWithRequiredPermissions = null;
		Action loginWithRequiredPermissionsThroughLoginMethod = null;
		Action<bool> checkPermissionsAndRunApi = delegate(bool loginIfNoPermissions)
		{
			if (!string.IsNullOrEmpty(requiredPermission))
			{
				FBGet("/me/permissions?limit=500", delegate(IDictionary<string, object> result)
				{
					if ((from p in result["data"] as List<object>
						where (p as Dictionary<string, object>)["status"].Equals("granted")
						select (string)(p as Dictionary<string, object>)["permission"]).ToList().Contains(requiredPermission))
					{
						runApi();
					}
					else if (loginIfNoPermissions)
					{
						loginWithRequiredPermissions(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				}, delegate(IResult result)
				{
					if (result != null && result.RawResult != null && result.RawResult.Contains("OAuthException"))
					{
						loginWithRequiredPermissionsThroughLoginMethod();
						countTryingUpdateToken++;
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				});
			}
			else
			{
				runApi();
			}
		};
		loginWithRequiredPermissions = delegate(bool bothReadAndWriteLogins)
		{
			Action loginWithRequiredPermissionsOneStep = delegate
			{
				FacebookDelegate<ILoginResult> callback = delegate(ILoginResult result)
				{
					LoggingIn = false;
					PrintFBResult(result);
					if (FB.IsLoggedIn)
					{
						checkPermissionsAndRunApi(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				};
				if (requiredPermission == "publish_actions")
				{
					LoggingIn = true;
					try
					{
						FB.LogInWithPublishPermissions(new List<string> { requiredPermission }, callback);
						return;
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogError("Exception in Facebook Login: " + ex2);
						LoggingIn = false;
						return;
					}
				}
				LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string> { requiredPermission }, callback);
				}
				catch (Exception ex3)
				{
					UnityEngine.Debug.LogError("Exception in Facebook Login: " + ex3);
					LoggingIn = false;
				}
			};
			if (bothReadAndWriteLogins && requiredPermission == "publish_actions")
			{
				LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string>(), delegate(ILoginResult result)
					{
						LoggingIn = false;
						PrintFBResult(result);
						if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
						{
							CoroutineRunner.Instance.StartCoroutine(RunActionAfterDelay(loginWithRequiredPermissionsOneStep));
						}
						else
						{
							UnityEngine.Debug.LogError("LogInWithReadPermissions: ! (string.IsNullOrEmpty(result.Error) && ! result.Cancelled)");
						}
					});
					return;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in Facebook Login: " + ex);
					LoggingIn = false;
					return;
				}
			}
			loginWithRequiredPermissionsOneStep();
		};
		loginWithRequiredPermissionsThroughLoginMethod = delegate
		{
			FB.LogOut();
			Login(null, null, "Unknown", delegate
			{
				checkPermissionsAndRunApi(false);
			});
		};
		if (!FB.IsLoggedIn)
		{
			loginWithRequiredPermissions(requiredPermission == "publish_actions");
		}
		else
		{
			checkPermissionsAndRunApi(true);
		}
	}

	public void InputFacebookFriends(Action onSuccess = null, bool dontRelogin = false)
	{
		if (FacebookSupported)
		{
			SetMyId(onSuccess, dontRelogin);
		}
	}

	public void InvitePlayer(Action onComplete = null)
	{
		if (!FacebookSupported || InvitePlayerApiIsRunning)
		{
			return;
		}
		InvitePlayerApiIsRunning = true;
		RunApiWithAskForPermissions(delegate
		{
			FB.AppRequest(messageInvite, null, null, null, null, "", titleInvite, delegate(IAppRequestResult result)
			{
				InvitePlayerApiIsRunning = false;
				PrintFBResult(result);
				if (onComplete != null)
				{
					onComplete();
				}
			});
		}, "publish_actions", false, delegate
		{
			InvitePlayerApiIsRunning = false;
		});
	}

	private IEnumerator GetOurFbId()
	{
		if (!FacebookSupported)
		{
			yield break;
		}
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		bool success = false;
		while (FB.IsLoggedIn && !success)
		{
			FBGet("/me", delegate(IDictionary<string, object> dict)
			{
				try
				{
					Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
					if (receivedSelfID != null)
					{
						receivedSelfID((string)dict["id"]);
					}
					success = true;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("FacebookController GetOurFbId exception: " + ex);
				}
			});
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 30f);
		}
	}
}
