using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class TwitterController : MonoBehaviour
{
	internal abstract class TwitterFacadeBase
	{
		public abstract void Init(string consumerKey, string consumerSecret);

		public abstract bool IsLoggedIn();

		public abstract void PostStatusUpdate(string status);

		public abstract void ShowLoginDialog(Action WP8customOnSuccessLogin = null);

		public abstract void Logout();
	}

	internal class IosTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			throw new NotSupportedException();
		}

		public string LoggedInUsername()
		{
			throw new NotSupportedException();
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		public override void Logout()
		{
		}
	}

	internal class AndroidTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			throw new NotSupportedException();
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		public override void Logout()
		{
		}
	}

	internal class DummyTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		public override void Logout()
		{
		}
	}

	[Serializable]
	[CompilerGenerated]
	internal sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Action<string> _003C_003E9__15_2;

		public static Action<string> _003C_003E9__15_3;

		internal void _003CPostStatusUpdate_003Eb__15_2(string unused)
		{
		}

		internal void _003CPostStatusUpdate_003Eb__15_3(string unused)
		{
		}
	}

	public static TwitterController Instance;

	public static readonly Dictionary<FacebookController.StoryPriority, int> StoryPostLimits;

	public const int DefaultGreenPriorityLimit = 7;

	public const int DefaultRedPriorityLimit = 3;

	public const int DefaultMultyWinPriorityLimit = 1;

	public const int DefaultArenaPriorityLimit = 1;

	private const string DefaultCallerContext = "Unknown";

	private string _loginContext = "Unknown";

	private static readonly Lazy<TwitterFacadeBase> _twitterFacade;

	private bool _postInProcess;

	private float _timeSinceLastStoryPostHistoryClean;

	private const string StoryPostHistoryKey = "TwitterControllerStoryPostHistoryKey";

	private const string PriorityKey = "priority";

	private const string TimeKey = "time";

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	public static bool IsLoggedIn
	{
		get
		{
			return TwitterFacade.IsLoggedIn();
		}
	}

	public static bool TwitterSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	public static bool TwitterSupported_OldPosts
	{
		get
		{
			if (TwitterSupported && BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && Instance != null)
			{
				return !Instance.CanPostStatusUpdateWithPriority(FacebookController.StoryPriority.Green);
			}
			return false;
		}
	}

	private static TwitterFacadeBase TwitterFacade
	{
		get
		{
			return _twitterFacade.Value;
		}
	}

	private void Awake()
	{
		Instance = this;
		if (TwitterSupported)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = "13K6E5YAJvXSEaig0GVVFd68K";
			empty2 = "I4DtR7TC0OU26OMYI0hLmP1jhVHfNuPRMskbYDIoOS2xYBBWdS";
			TwitterFacade.Init(empty, empty2);
			FriendsController.NewTwitterLimitsAvailable += HandleNewTwitterLimitsAvailable;
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitStoryPostHistoryKey();
		LoadStoryPostHistory();
	}

	public void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown")
	{
		if (TwitterSupported)
		{
			if (Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 0)
			{
				Storager.setInt(Defs.TwitterRewardGainStarted, 1);
			}
			_loginContext = context;
			TwitterFacade.ShowLoginDialog();
		}
	}

	public void Logout()
	{
		if (TwitterSupported)
		{
			TwitterFacade.Logout();
		}
	}

	public void PostStatusUpdate(string status, FacebookController.StoryPriority priority)
	{
		if (TwitterSupported)
		{
			PostStatusUpdate(status, delegate
			{
				RegisterStoryPostedWithPriorityCore(priority);
			});
		}
	}

	public void PostStatusUpdate(string status, Action customOnSuccess = null)
	{
		if (!TwitterSupported)
		{
			return;
		}
		Action<string> postAction = null;
		Action<object> action = default(Action<object>);
		postAction = delegate
		{
			if (!_postInProcess)
			{
				if (customOnSuccess != null)
				{
					if (action == null)
					{
						action = delegate
						{
							customOnSuccess();
						};
					}
					if (_003C_003Ec._003C_003E9__15_2 == null)
					{
						_003C_003Ec._003C_003E9__15_2 = delegate
						{
						};
					}
				}
				_postInProcess = true;
				TwitterFacade.PostStatusUpdate(status);
			}
		};
		if (_003C_003Ec._003C_003E9__15_3 == null)
		{
			_003C_003Ec._003C_003E9__15_3 = delegate
			{
			};
		}
		if (TwitterFacade.IsLoggedIn())
		{
			postAction(string.Empty);
			return;
		}
		TwitterFacade.ShowLoginDialog(delegate
		{
			postAction(string.Empty);
		});
	}

	public bool CanPostStatusUpdateWithPriority(FacebookController.StoryPriority priority)
	{
		try
		{
			if (priority == FacebookController.StoryPriority.Green)
			{
				return storiesPostHistory.Count < StoryPostLimits[priority];
			}
			return storiesPostHistory.Where((Dictionary<string, object> rec) => int.Parse((string)rec["priority"]) == (int)priority).Count() < StoryPostLimits[priority];
		}
		catch (Exception ex)
		{
			Debug.LogError("Exeption in CanPostStoryWithPriority:\n" + ex);
		}
		return false;
	}

	public static void CheckAndGiveTwitterReward(string context)
	{
		if (TwitterSupported && Storager.getInt(Defs.IsTwitterLoginRewardaGained) == 0 && Storager.getInt(Defs.TwitterRewardGainStarted) == 1 && TwitterFacade.IsLoggedIn())
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 0);
			Storager.setInt(Defs.IsTwitterLoginRewardaGained, 1);
			BankController.AddGems(10);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginTwitter");
			QuestMediator.NotifySocialInteraction("loginTwitter");
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object> { 
			{
				"Login Twitter",
				context ?? "Unknown"
			} });
			AnalyticsFacade.SendCustomEventToAppsFlyer("Virality", new Dictionary<string, string> { 
			{
				"Login Twitter",
				context ?? "Unknown"
			} });
		}
	}

	private void OnTwitterLogin(string result)
	{
		CheckAndGiveTwitterReward(_loginContext);
		Debug.Log(string.Format("TwitterController.OnTwitterLogin(“{0}”)    {1}", new object[2] { result, _loginContext }));
		_loginContext = "Unknown";
	}

	private void OnTwitterLoginFailed(string error)
	{
		Debug.Log(string.Format("TwitterController.OnTwitterLoginFailed(“{0}”)    {1}", new object[2] { error, _loginContext }));
		_loginContext = "Unknown";
	}

	private void OnTwitterPost(object result)
	{
		_postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPost: " + result) ?? "");
	}

	private void OnTwitterPostFailed(string _error)
	{
		_postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPostFailed: " + _error) ?? "");
	}

	private void HandleNewTwitterLimitsAvailable(int greenLimit, int redLimit, int multyWinLimit, int arenaLimit)
	{
		StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
		StoryPostLimits[FacebookController.StoryPriority.MultyWinLimit] = multyWinLimit;
		StoryPostLimits[FacebookController.StoryPriority.ArenaLimit] = arenaLimit;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveStoryPostHistory();
		}
		else
		{
			LoadStoryPostHistory();
		}
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - _timeSinceLastStoryPostHistoryClean > 10f)
		{
			CleanStoryPostHistory();
		}
	}

	private void OnDestroy()
	{
		SaveStoryPostHistory();
		if (TwitterSupported)
		{
			FriendsController.NewTwitterLimitsAvailable -= HandleNewTwitterLimitsAvailable;
		}
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("TwitterControllerStoryPostHistoryKey", Json.Serialize(storiesPostHistory));
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> obj = Json.Deserialize(Storager.getString("TwitterControllerStoryPostHistoryKey")) as List<object>;
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
			Debug.LogError("TwitterController Exeption in CleanStoryPostHistory:\n" + ex);
		}
	}

	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (TwitterSupported)
		{
			List<Dictionary<string, object>> list = storiesPostHistory;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int num = (int)priority;
			dictionary.Add("priority", num.ToString());
			dictionary.Add("time", PromoActionsManager.CurrentUnixTime.ToString());
			list.Add(dictionary);
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("TwitterControllerStoryPostHistoryKey"))
		{
			Storager.setString("TwitterControllerStoryPostHistoryKey", "[]");
		}
	}

	static TwitterController()
	{
		StoryPostLimits = new Dictionary<FacebookController.StoryPriority, int>(4, new FacebookController.StoryPriorityComparer())
		{
			{
				FacebookController.StoryPriority.Green,
				7
			},
			{
				FacebookController.StoryPriority.Red,
				3
			},
			{
				FacebookController.StoryPriority.MultyWinLimit,
				1
			},
			{
				FacebookController.StoryPriority.ArenaLimit,
				1
			}
		};
		_twitterFacade = new Lazy<TwitterFacadeBase>(InitializeFacade);
	}

	private static TwitterFacadeBase InitializeFacade()
	{
		if (!TwitterSupported)
		{
			return new DummyTwitterFacade();
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			return new IosTwitterFacade();
		case RuntimePlatform.Android:
			return new AndroidTwitterFacade();
		default:
			return new DummyTwitterFacade();
		}
	}
}
