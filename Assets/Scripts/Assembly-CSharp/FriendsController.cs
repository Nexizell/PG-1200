using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using SimpleJSON;
using UnityEngine;

public sealed class FriendsController : MonoBehaviour
{
	public delegate void OnChangeClanName(string newName);

	public enum PossiblleOrigin
	{
		None = 0,
		Local = 1,
		Facebook = 2,
		RandomPlayer = 3
	}

	public enum NotConnectCondition
	{
		level = 0,
		platform = 1,
		map = 2,
		clientVersion = 3,
		InChat = 4,
		None = 5,
		league = 6
	}

	public class ResultParseOnlineData
	{
		public string mapIndex;

		public bool isPlayerInChat;

		public NotConnectCondition notConnectCondition;

		private string _gameRegim;

		private string _gameMode;

		public string gameMode
		{
			get
			{
				return _gameMode;
			}
			set
			{
				_gameMode = value;
				_gameRegim = _gameMode.Substring(_gameMode.Length - 1);
			}
		}

		public bool IsCanConnect
		{
			get
			{
				return notConnectCondition == NotConnectCondition.None;
			}
		}

		public OnlineState GetOnlineStatus()
		{
			switch (Convert.ToInt32(_gameRegim))
			{
			case 6:
				return OnlineState.inFriends;
			case 7:
				return OnlineState.inClans;
			default:
				return OnlineState.playing;
			}
		}

		public string GetGameModeName()
		{
			IDictionary<int, string> gameModesLocalizeKey = GameConnect.gameModesLocalizeKey;
			if (!gameModesLocalizeKey.ContainsKey(int.Parse(_gameRegim)))
			{
				return string.Empty;
			}
			return LocalizationStore.Get(gameModesLocalizeKey[int.Parse(_gameRegim)]);
		}

		public string GetMapName()
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(mapIndex));
			if (infoScene == null)
			{
				return string.Empty;
			}
			return infoScene.TranslateName;
		}

		public string GetNotConnectConditionString()
		{
			if (IsCanConnect)
			{
				return string.Empty;
			}
			string result = string.Empty;
			switch (notConnectCondition)
			{
			case NotConnectCondition.clientVersion:
				result = LocalizationStore.Get("Key_1418");
				break;
			case NotConnectCondition.level:
				result = LocalizationStore.Get("Key_1420");
				break;
			case NotConnectCondition.map:
				result = LocalizationStore.Get("Key_1419");
				break;
			case NotConnectCondition.platform:
				result = LocalizationStore.Get("Key_1414");
				break;
			case NotConnectCondition.league:
				result = LocalizationStore.Get("Key_2953");
				break;
			}
			return result;
		}

		public string GetNotConnectConditionShortString()
		{
			if (IsCanConnect)
			{
				return string.Empty;
			}
			string result = string.Empty;
			switch (notConnectCondition)
			{
			case NotConnectCondition.clientVersion:
				result = LocalizationStore.Get("Key_1573");
				break;
			case NotConnectCondition.level:
				result = LocalizationStore.Get("Key_1574");
				break;
			case NotConnectCondition.map:
				result = LocalizationStore.Get("Key_1575");
				break;
			case NotConnectCondition.platform:
				result = LocalizationStore.Get("Key_1576");
				break;
			case NotConnectCondition.InChat:
				result = LocalizationStore.Get("Key_1577");
				break;
			case NotConnectCondition.league:
				result = LocalizationStore.Get("Key_2954");
				break;
			}
			return result;
		}
	}

	public enum TypeTrafficForwardingLog
	{
		newView = 0,
		view = 1,
		click = 2
	}

	public static bool isDebugLogWWW = true;

	public int Banned = -1;

	public static float onlineDelta = 60f;

	public static Dictionary<string, Dictionary<string, string>> mapPopularityDictionary = new Dictionary<string, Dictionary<string, string>>();

	public static bool readyToOperate = false;

	public static FriendsController sharedController = null;

	private string currentCompetitionKey = "currentCompetitionKey";

	private int _currentCompetition = -1;

	private float _expirationTimeCompetition = -1f;

	private static bool _expirationTimeCompetitionInit = false;

	private static string expirationTimeCompetitionKey = "expirationTimeCompetitionKey";

	private HashSet<string> playersLikesAlreadySentTo = new HashSet<string>();

	private static bool _advertEnabled = false;

	private bool friendsReceivedOnce;

	[ReadOnly]
	[SerializeField]
	protected internal string _clanId;

	public string clanLeaderID;

	public string clanLogo;

	public string clanName;

	public int NumberOfFriendsRequests;

	public int NumberOffFullInfoRequests;

	public int NumberOfBestPlayersRequests;

	public int NumberOfClanInfoRequests;

	public int NumberOfCreateClanRequests;

	private float lastTouchTm;

	public bool idle;

	private List<int> ids = new List<int>();

	public List<string> friendsDeletedLocal = new List<string>();

	public string JoinClanSent;

	public const string AccountCreated = "AccountCreated";

	private string _id;

	internal List<string> friends = new List<string>();

	internal readonly List<Dictionary<string, string>> clanMembers = new List<Dictionary<string, string>>();

	internal List<string> invitesFromUs = new List<string>();

	internal List<string> invitesToUs = new List<string>();

	internal List<Dictionary<string, string>> ClanInvites = new List<Dictionary<string, string>>();

	internal readonly List<string> ClanSentInvites = new List<string>();

	internal readonly List<string> clanSentInvitesLocal = new List<string>();

	internal readonly List<string> clanCancelledInvitesLocal = new List<string>();

	internal readonly List<string> clanDeletedLocal = new List<string>();

	internal readonly Dictionary<string, Dictionary<string, object>> playersInfo = new Dictionary<string, Dictionary<string, object>>();

	internal readonly Dictionary<string, Dictionary<string, object>> friendsInfo = new Dictionary<string, Dictionary<string, object>>();

	internal readonly Dictionary<string, Dictionary<string, object>> clanFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	internal readonly Dictionary<string, Dictionary<string, object>> profileInfo = new Dictionary<string, Dictionary<string, object>>();

	internal readonly Dictionary<string, Dictionary<string, string>> onlineInfo = new Dictionary<string, Dictionary<string, string>>();

	internal readonly List<string> notShowAddIds = new List<string>();

	internal readonly Dictionary<string, Dictionary<string, object>> facebookFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	public string alphaIvory;

	public string nick;

	public string skin;

	public int rank;

	public int coopScore;

	public int survivalScore;

	internal SaltedInt wins = new SaltedInt(641227346);

	public Dictionary<string, object> ourInfo;

	public string id_fb;

	private const string FriendsKey = "FriendsKey";

	private const string ToUsKey = "ToUsKey";

	private const string PlayerInfoKey = "PlayerInfoKey";

	private const string FriendsInfoKey = "FriendsInfoKey";

	private const string ClanFriendsInfoKey = "ClanFriendsInfoKey";

	private const string ClanInvitesKey = "ClanInvitesKey";

	private const string PixelbookSettingsKey = "PixelbookSettingsKey";

	public const string LobbyNewsKey = "LobbyNewsKey";

	public const string LobbyIsAnyNewsKey = "LobbyIsAnyNewsKey";

	public const string PixelFilterWordsKey = "PixelFilterWordsKey";

	public const string PixelFilterSymbolsKey = "PixelFilterSymbolsKey";

	private float timerUpdatePixelbookSetting = 900f;

	private NewDayWatcher _newDayWatcher;

	private static long localServerTime;

	private static float tickForServerTime;

	private static bool isUpdateServerTimeAfterRun;

	private bool isGetServerTimeFromMainUrl = true;

	public static bool isInitPixelbookSettingsFromServer;

	private string FacebookIDKey = "FacebookIDKey";

	private List<string> sendingLikesForLobbyPlayers = new List<string>();

	public OnChangeClanName onChangeClanName;

	private string _prevClanName;

	public bool dataSent;

	private bool infoLoaded;

	public static float timeOutSendUpdatePlayerFromConnectScene;

	public string tempClanID;

	public string tempClanLogo;

	public string tempClanName;

	public string tempClanCreatorID;

	private bool _shouldStopOnline;

	private bool _shouldStopOnlineWithClanInfo;

	private bool _shouldStopRefrClanOnline;

	public Action GetFacebookFriendsCallback;

	private string _inputToken;

	private KeyValuePair<string, int>? _winCountTimestamp;

	private bool ReceivedLastOnline;

	private bool getCohortInfo;

	private float timeSendUpdatePlayer;

	private string oldLobby = string.Empty;

	public const float TimeUpdateFriendAndClanData = 20f;

	public float timerUpdateFriend = 20f;

	public static Action OnShowBoxProcessFriendsData;

	public static Action OnHideBoxProcessFriendsData;

	private bool _shouldStopRefreshingInfo;

	private float deltaTimeInGame;

	private float sendingTime;

	private bool firstUpdateAfterApplicationPause;

	public Dictionary<string, PossiblleOrigin> getPossibleFriendsResult = new Dictionary<string, PossiblleOrigin>();

	private bool isUpdateInfoAboutAllFriends;

	public float timerSendTimeGame = 30f;

	public static Action UpdatedLastLikeLobbyPlayers;

	private static string keylastLikesPlayers;

	private static List<string> _lastLikesPlayers;

	public static Action UpdateFriendsInfoAction;

	public Dictionary<string, string> clicksJoinByFriends = new Dictionary<string, string>();

	private static FriendProfileController _friendProfileController;

	private static DateTime timeSendTrafficForwarding;

	private static bool _isConfigNameAdvertInit;

	private static string _configNameABTestAdvert;

	public static bool isReadABTestAdvertConfig;

	private OurLobbyLikes ourLobbyLikes;

	private const string OUR_LOBBY_LIKES_KEY = "FriendsController.OUR_LOBBY_LIKES_KEY";

	private const string LOBBY_LIKES_SEND_KEY = "FriendProfileView.LOBBY_LIKES_SEND_KEY";

	public int currentCompetition
	{
		get
		{
			if (_currentCompetition < 0)
			{
				_currentCompetition = Storager.getInt(currentCompetitionKey);
			}
			return _currentCompetition;
		}
		internal set
		{
			_currentCompetition = value;
			Storager.setInt(currentCompetitionKey, _currentCompetition);
		}
	}

	public float expirationTimeCompetition
	{
		get
		{
			if (!_expirationTimeCompetitionInit)
			{
				PlayerPrefs.GetInt(expirationTimeCompetitionKey, 0);
			}
			_expirationTimeCompetitionInit = true;
			return _expirationTimeCompetition;
		}
		private set
		{
			_expirationTimeCompetition = value + Time.realtimeSinceStartup;
			PlayerPrefs.SetInt(expirationTimeCompetitionKey, Mathf.RoundToInt(value));
			_expirationTimeCompetitionInit = true;
		}
	}

	public HashSet<string> PlayersLikesAlreadySentTo
	{
		get
		{
			return playersLikesAlreadySentTo;
		}
	}

	public bool ClanLimitReached
	{
		get
		{
			FriendsController friendsController = sharedController;
			return friendsController.clanMembers.Count + friendsController.ClanSentInvites.Count + friendsController.clanSentInvitesLocal.Count >= friendsController.ClanLimit;
		}
	}

	public int ClanLimit
	{
		get
		{
			return Defs.maxMemberClanCount;
		}
	}

	internal static bool PolygonEnabled
	{
		get
		{
			return Defs.IsDeveloperBuild;
		}
	}

	internal static bool AdvertEnabled
	{
		get
		{
			return _advertEnabled;
		}
		set
		{
			_advertEnabled = value;
		}
	}

	public static bool ClanDataSettted { get; private set; }

	public static int CurrentPlatform
	{
		get
		{
			return ProtocolListGetter.CurrentPlatform;
		}
	}

	public string ClanID
	{
		get
		{
			return _clanId;
		}
		set
		{
			_clanId = value;
			if (FriendsController.OnClanIdSetted != null)
			{
				FriendsController.OnClanIdSetted(_clanId);
			}
		}
	}

	public static long ServerTime
	{
		get
		{
			if (isUpdateServerTimeAfterRun)
			{
				return localServerTime;
			}
			return -1L;
		}
	}

	public static string actionAddress
	{
		get
		{
			return URLs.Friends;
		}
	}

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	private NewDayWatcher NewDayWatcher
	{
		get
		{
			if (_newDayWatcher == null)
			{
				NewDayWatcher newDayWatcher = base.gameObject.GetComponent<NewDayWatcher>() ?? base.gameObject.AddComponent<NewDayWatcher>();
				_newDayWatcher = newDayWatcher;
			}
			return _newDayWatcher;
		}
	}

	internal int? CurrentServerSeason { get; private set; }

	public KeyValuePair<string, int>? WinCountTimestamp
	{
		get
		{
			return _winCountTimestamp;
		}
	}

	public Dictionary<string, object> getInfoPlayerResult { get; private set; }

	public List<string> findPlayersByParamResult { get; private set; }

	public static bool HasFriends
	{
		get
		{
			string @string = PlayerPrefs.GetString("FriendsKey", "[]");
			if (!string.IsNullOrEmpty(@string))
			{
				return @string != "[]";
			}
			return false;
		}
	}

	public static List<string> lastLikesPlayers
	{
		get
		{
			if (_lastLikesPlayers == null)
			{
				List<object> list = Json.Deserialize(PlayerPrefs.GetString(keylastLikesPlayers, "[]")) as List<object>;
				_lastLikesPlayers = new List<string>();
				for (int i = 0; i < list.Count; i++)
				{
					_lastLikesPlayers.Add(list[i].ToString());
				}
			}
			return _lastLikesPlayers;
		}
		set
		{
			_lastLikesPlayers = value;
			PlayerPrefs.SetString(keylastLikesPlayers, Json.Serialize(_lastLikesPlayers));
		}
	}

	public bool ProfileInterfaceActive
	{
		get
		{
			if (_friendProfileController == null)
			{
				return false;
			}
			return _friendProfileController.FriendProfileGo.Map((GameObject g) => g.activeInHierarchy);
		}
	}

	public static string configNameABTestAdvert
	{
		get
		{
			if (!_isConfigNameAdvertInit)
			{
				_configNameABTestAdvert = PlayerPrefs.GetString("CNAdvert", "none");
				_isConfigNameAdvertInit = true;
			}
			return _configNameABTestAdvert;
		}
		set
		{
			_isConfigNameAdvertInit = true;
			_configNameABTestAdvert = value;
			PlayerPrefs.SetString("CNAdvert", _configNameABTestAdvert);
		}
	}

	public OurLobbyLikes OurLobbyLikes
	{
		get
		{
			return ourLobbyLikes;
		}
	}

	public static event Action<string, bool, int> OnSendLikeLobby;

	public static event Action<string, bool> OnFailSendLikeLobby;

	public static event Action FriendsUpdated;

	public static event Action ClanUpdated;

	public static event Action FullInfoUpdated;

	public static event Action ServerTimeUpdated;

	public static event Action MapPopularityUpdated;

	public event Action FailedSendNewClan;

	public event Action<int> ReturnNewIDClan;

	public static event Action<string> OnClanIdSetted;

	public static event Action<int, int> NewFacebookLimitsAvailable;

	public static event Action<int, int, int, int> NewTwitterLimitsAvailable;

	public static event Action<int, int, int, int> NewCheaterDetectParametersAvailable;

	public static event Action OurInfoUpdated;

	internal static DateTime? GetServerTime()
	{
		long serverTime = ServerTime;
		if (serverTime <= 0)
		{
			return null;
		}
		return Tools.GetCurrentTimeByUnixTime(serverTime);
	}

	internal static DateTime GetServerTimeOrFallbackToLocalUtc()
	{
		long serverTime = ServerTime;
		if (serverTime <= 0)
		{
			return DateTime.UtcNow;
		}
		return Tools.GetCurrentTimeByUnixTime(serverTime);
	}

	public void FastGetPixelbookSettings()
	{
		timerUpdatePixelbookSetting = -1f;
	}

	private IEnumerator GetPixelbookSettingsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		timerUpdatePixelbookSetting = Defs.timeUpdatePixelbookInfo;
		while (true)
		{
			yield return StartCoroutine(GetPixelbookSettings());
			while (timerUpdatePixelbookSetting > 0f)
			{
				timerUpdatePixelbookSetting -= Time.unscaledDeltaTime;
				yield return null;
			}
			timerUpdatePixelbookSetting = Defs.timeUpdatePixelbookInfo;
		}
	}

	private IEnumerator GetNewsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(GetLobbyNews(false));
			yield return new WaitForSeconds(Defs.timeUpdateNews);
		}
	}

	private IEnumerator GetFiltersSettings()
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.FilterBadWord);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		if (!string.IsNullOrEmpty(response.error))
		{
			UnityEngine.Debug.LogWarning("FilterBadWord response error: " + response.error);
			yield break;
		}
		string text = URLs.Sanitize(response);
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("FilterBadWord response is empty");
			yield break;
		}
		Dictionary<string, object> obj = Json.Deserialize(text) as Dictionary<string, object>;
		string value = Json.Serialize(obj["Words"]);
		string value2 = Json.Serialize(obj["Symbols"]);
		PlayerPrefs.SetString("PixelFilterWordsKey", value);
		PlayerPrefs.SetString("PixelFilterSymbolsKey", value2);
		PlayerPrefs.Save();
		FilterBadWorld.InitBadLists();
	}

	private IEnumerator GetBuffSettings(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		string url = (ABTestController.useBuffSystem ? URLs.BuffSettings1050 : URLs.BuffSettings1031);
		string value = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(value))
		{
			responseText = value;
		}
		else
		{
			WWW response;
			while (true)
			{
				response = Tools.CreateWwwIfNotConnected(url);
				if (response == null)
				{
					yield return new WaitForSeconds(20f);
					continue;
				}
				yield return response;
				if (!string.IsNullOrEmpty(response.error))
				{
					UnityEngine.Debug.LogWarning("GetBuffSettings response error: " + response.error);
					yield return new WaitForSeconds(20f);
					continue;
				}
				responseText = URLs.Sanitize(response);
				if (!string.IsNullOrEmpty(responseText))
				{
					break;
				}
				UnityEngine.Debug.LogWarning("GetBuffSettings response is empty");
				yield return new WaitForSeconds(20f);
			}
			PersistentCacheManager.Instance.SetValue(response.url, responseText);
		}
		Storager.setString("BuffsParam", responseText);
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.TryLoadConfig();
		}
	}

	private IEnumerator GetRatingSystemConfig()
	{
		while (true)
		{
			new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(URLs.RatingSystemConfigURL);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetRatingSystemConfig error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			string text = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(text))
			{
				Storager.setString("rSCKeyV3", text);
				RatingSystem.instance.ParseConfig();
			}
			yield return StartCoroutine(MyWaitForSeconds(1800f));
		}
	}

	private IEnumerator GetLobbyNews(bool fromPause)
	{
		string lobbyNews = URLs.LobbyNews;
		string value = PersistentCacheManager.Instance.GetValue(lobbyNews);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(lobbyNews);
			if (response == null)
			{
				yield break;
			}
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarning("GetLobbyNews response error: " + response.error);
				yield break;
			}
			text = URLs.Sanitize(response);
			PersistentCacheManager.Instance.SetValue(response.url, text);
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning("GetLobbyNews response is empty");
			yield break;
		}
		string @string = PlayerPrefs.GetString("LobbyNewsKey", "[]");
		bool flag = false;
		List<object> list = Json.Deserialize(@string) as List<object>;
		List<Dictionary<string, object>> list2 = ((list != null) ? list.OfType<Dictionary<string, object>>().ToList() : new List<Dictionary<string, object>>());
		List<object> list3 = Json.Deserialize(text) as List<object>;
		List<Dictionary<string, object>> list4 = ((list3 != null) ? list3.OfType<Dictionary<string, object>>().ToList() : new List<Dictionary<string, object>>());
		if (list4.Count == 0)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < list4.Count; i++)
			{
				list4[i]["readed"] = 0;
				bool flag2 = false;
				for (int j = 0; j < list2.Count; j++)
				{
					if (Convert.ToInt32(list2[j]["date"]) == Convert.ToInt32(list4[i]["date"]))
					{
						flag2 = true;
						if (list2[j].ContainsKey("readed"))
						{
							list4[i]["readed"] = list2[j]["readed"];
						}
						break;
					}
				}
				try
				{
					if (!flag2)
					{
						AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
						{
							{ "CTR", "Show" },
							{ "Conversion Total", "Show" }
						});
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in log News (CTR = Show, Conversion Total = Show): " + ex);
				}
				if (Convert.ToInt32(list4[i]["readed"]) == 0)
				{
					flag = true;
				}
			}
		}
		PlayerPrefs.SetString("LobbyNewsKey", Json.Serialize(list4));
		PlayerPrefs.SetInt("LobbyIsAnyNewsKey", flag ? 1 : 0);
		PlayerPrefs.Save();
		if (flag && MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.newsIndicator.SetActive(true);
		}
	}

	private IEnumerator GetTimeFromServerLoop()
	{
		isUpdateServerTimeAfterRun = false;
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		while (string.IsNullOrEmpty(id))
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(GetTimeFromServer());
			float timerUpdate = ((!isUpdateServerTimeAfterRun) ? Defs.timeUpdateServerTime : 1200f);
			if (FriendsController.ServerTimeUpdated != null)
			{
				FriendsController.ServerTimeUpdated();
			}
			SendX2REwardAnalytics();
			while (timerUpdate > 0f)
			{
				timerUpdate -= Time.unscaledDeltaTime;
				yield return null;
			}
		}
	}

	public static void SendX2REwardAnalytics()
	{
		if (ServerTime == -1)
		{
			return;
		}
		try
		{
			foreach (GameConnect.GameMode supportedMode in DoubleReward.Instance.SupportedModes)
			{
				DoubleReward.Instance.SendAnalyticsIfNeededForMode(supportedMode);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in sending analytics : {0}", ex);
		}
	}

	private IEnumerator GetTimeFromServer()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_time");
		wWWForm.AddField("app_version", string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		}));
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_time"));
		WWW download = Tools.CreateWww(isGetServerTimeFromMainUrl ? URLs.Friends : URLs.TimeOnSecure, wWWForm);
		if (download != null)
		{
			yield return download;
			string text = URLs.Sanitize(download);
			long result;
			if (!string.IsNullOrEmpty(download.error))
			{
				UnityEngine.Debug.LogWarning("get_time error:    " + download.error);
				isGetServerTimeFromMainUrl = !isGetServerTimeFromMainUrl;
			}
			else if (long.TryParse(text, out result))
			{
				localServerTime = result;
				tickForServerTime = 0f;
				isUpdateServerTimeAfterRun = true;
			}
			else
			{
				UnityEngine.Debug.LogError("Could not parse response: " + text);
				isGetServerTimeFromMainUrl = !isGetServerTimeFromMainUrl;
			}
		}
	}

	private IEnumerator GetLocalMultySetting()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.getInt("localMultyKey") == 2)
		{
			yield break;
		}
		string url = URLs.SetLocalMultyUrl;
		while (true)
		{
			WWW response = Tools.CreateWwwIfNotConnected(url);
			if (response == null)
			{
				break;
			}
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarning("LocalMultySetting response error: " + response.error);
				yield return new WaitForSeconds(5f);
				continue;
			}
			string responseText = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(responseText))
			{
				UnityEngine.Debug.LogWarning("LocalMultySetting response is empty");
				yield return new WaitForSeconds(5f);
				continue;
			}
			List<object> _config = Json.Deserialize(responseText) as List<object>;
			if (_config == null)
			{
				UnityEngine.Debug.LogWarning("LocalMultySetting _config == null");
				yield return new WaitForSeconds(5f);
				continue;
			}
			bool flag = false;
			for (int i = 0; i < _config.Count; i++)
			{
				if (_config[i].ToString() == GlobalGameController.AppVersion)
				{
					flag = true;
					break;
				}
			}
			Storager.setInt("localMultyKey", flag ? 1 : 2);
			if (flag)
			{
				yield return new WaitForSeconds(15f);
				continue;
			}
			break;
		}
	}

	private IEnumerator GetPixelbookSettings()
	{
		string url = URLs.PixelbookSettings;
		string value = PersistentCacheManager.Instance.GetValue(url);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(url);
			if (response == null)
			{
				yield break;
			}
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarning("PixelbookSettings response error: " + response.error);
				yield break;
			}
			text = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogWarning("PixelbookSettings response is empty");
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(url, text);
		}
		Storager.setString("PixelbookSettingsKey", text);
		UpdatePixelbookSettingsFromPrefs();
		isInitPixelbookSettingsFromServer = true;
	}

	public static void UpdatePixelbookSettingsFromPrefs()
	{
		if (!Storager.hasKey("PixelbookSettingsKey"))
		{
			Storager.setString("PixelbookSettingsKey", "");
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("PixelbookSettingsKey")) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("MaxFriendCount"))
		{
			return;
		}
		if (dictionary.ContainsKey("FriendsUrl"))
		{
			URLs.Friends = Convert.ToString(dictionary["FriendsUrl"]);
		}
		if (dictionary.ContainsKey("MaxFriendCount"))
		{
			Defs.maxCountFriend = Convert.ToInt32(dictionary["MaxFriendCount"]);
		}
		if (dictionary.ContainsKey("MaxMemberClanCount"))
		{
			Defs.maxMemberClanCount = Convert.ToInt32(dictionary["MaxMemberClanCount"]);
		}
		if (dictionary.ContainsKey("TimeUpdateFriendInfo"))
		{
			Defs.timeUpdateFriendInfo = Convert.ToInt32(dictionary["TimeUpdateFriendInfo"]);
		}
		if (dictionary.ContainsKey("TimeUpdateOnlineInGame"))
		{
			Defs.timeUpdateOnlineInGame = Convert.ToInt32(dictionary["TimeUpdateOnlineInGame"]);
		}
		if (dictionary.ContainsKey("TimeUpdateInfoInProfile"))
		{
			Defs.timeUpdateInfoInProfile = Convert.ToInt32(dictionary["TimeUpdateInfoInProfile"]);
		}
		if (dictionary.ContainsKey("TimeUpdateLeaderboardIfNullResponce"))
		{
			Defs.timeUpdateLeaderboardIfNullResponce = Convert.ToInt32(dictionary["TimeUpdateLeaderboardIfNullResponce"]);
		}
		if (dictionary.ContainsKey("TimeBlockRefreshFriendDate"))
		{
			Defs.timeBlockRefreshFriendDate = Convert.ToInt32(dictionary["TimeBlockRefreshFriendDate"]);
		}
		if (dictionary.ContainsKey("TimeWaitLoadPossibleFriends"))
		{
			Defs.timeWaitLoadPossibleFriends = Convert.ToInt32(dictionary["TimeWaitLoadPossibleFriends"]);
		}
		if (dictionary.ContainsKey("PauseUpdateLeaderboard"))
		{
			Defs.pauseUpdateLeaderboard = Convert.ToInt32(dictionary["PauseUpdateLeaderboard"]);
		}
		if (dictionary.ContainsKey("TimeUpdatePixelbookInfo"))
		{
			Defs.timeUpdatePixelbookInfo = Convert.ToInt32(dictionary["TimeUpdatePixelbookInfo"]);
		}
		if (dictionary.ContainsKey("HistoryPrivateMessageLength"))
		{
			Defs.historyPrivateMessageLength = Convert.ToInt32(dictionary["HistoryPrivateMessageLength"]);
		}
		if (dictionary.ContainsKey("OutgoingInviteTimeoutMinutes"))
		{
			BattleInviteListener.Instance.OutgoingInviteTimeout = TimeSpan.FromMinutes(Convert.ToSingle(dictionary["OutgoingInviteTimeoutMinutes"]));
		}
		if (dictionary.ContainsKey("TimerIntervalDelaysFirstsEggs"))
		{
			List<object> list = dictionary["TimerIntervalDelaysFirstsEggs"] as List<object>;
			Nest.timerIntervalDelays.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				Nest.timerIntervalDelays.Add(Convert.ToInt64(list[i]));
			}
		}
		if (dictionary.ContainsKey("TimeUpdateStartCheckIfNullResponce"))
		{
			Defs.timeUpdateStartCheckIfNullResponce = Convert.ToInt32(dictionary["TimeUpdateStartCheckIfNullResponce"]);
		}
		if (dictionary.ContainsKey("TimeoutSendUpdatePlayerFromConnectScene"))
		{
			timeOutSendUpdatePlayerFromConnectScene = Convert.ToInt32(dictionary["TimeoutSendUpdatePlayerFromConnectScene"]);
		}
		if (dictionary.ContainsKey("EnableLogForIDs") && sharedController != null && !string.IsNullOrEmpty(sharedController.id))
		{
			foreach (object item in dictionary["EnableLogForIDs"] as List<object>)
			{
				if (sharedController.id == item.ToString())
				{
					LogsManager.EnableLogsFromServer();
					break;
				}
			}
		}
		if (dictionary.ContainsKey("FacebookLimits"))
		{
			try
			{
				Dictionary<string, object> obj = dictionary["FacebookLimits"] as Dictionary<string, object>;
				int arg = (int)(long)obj["GreenLimit"];
				int arg2 = (int)(long)obj["RedLimit"];
				Action<int, int> newFacebookLimitsAvailable = FriendsController.NewFacebookLimitsAvailable;
				if (newFacebookLimitsAvailable != null)
				{
					newFacebookLimitsAvailable(arg, arg2);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		if (dictionary.ContainsKey("TwitterLimits"))
		{
			try
			{
				Dictionary<string, object> obj2 = dictionary["TwitterLimits"] as Dictionary<string, object>;
				int arg3 = (int)(long)obj2["GreenLimit"];
				int arg4 = (int)(long)obj2["RedLimit"];
				int arg5 = (int)(long)obj2["MultyWinLimit"];
				int arg6 = (int)(long)obj2["ArenaLimit"];
				Action<int, int, int, int> newTwitterLimitsAvailable = FriendsController.NewTwitterLimitsAvailable;
				if (newTwitterLimitsAvailable != null)
				{
					newTwitterLimitsAvailable(arg3, arg4, arg5, arg6);
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
			}
		}
		if (!dictionary.ContainsKey("CheaterDetectParameters"))
		{
			return;
		}
		try
		{
			Dictionary<string, object> obj3 = dictionary["CheaterDetectParameters"] as Dictionary<string, object>;
			Dictionary<string, object> obj4 = obj3["Paying"] as Dictionary<string, object>;
			int arg7 = (int)(long)obj4["Coins"];
			int arg8 = (int)(long)obj4["GemsCurrency"];
			Dictionary<string, object> obj5 = obj3["NonPaying"] as Dictionary<string, object>;
			int arg9 = (int)(long)obj5["Coins"];
			int arg10 = (int)(long)obj5["GemsCurrency"];
			Action<int, int, int, int> newCheaterDetectParametersAvailable = FriendsController.NewCheaterDetectParametersAvailable;
			if (newCheaterDetectParametersAvailable != null)
			{
				newCheaterDetectParametersAvailable(arg7, arg8, arg9, arg10);
			}
		}
		catch (Exception exception3)
		{
			UnityEngine.Debug.LogException(exception3);
		}
	}

	private static void FillListDictionary(string key, List<Dictionary<string, string>> list)
	{
		List<object> list2 = Json.Deserialize(PlayerPrefs.GetString(key, "[]")) as List<object>;
		if (list2 == null || list2.Count <= 0)
		{
			return;
		}
		foreach (Dictionary<string, object> item in list2.OfType<Dictionary<string, object>>())
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item2 in item)
			{
				string text = item2.Value as string;
				if (text != null)
				{
					dictionary.Add(item2.Key, text);
				}
			}
			list.Add(dictionary);
		}
	}

	private static List<string> FillList(string key)
	{
		List<string> list = new List<string>();
		List<object> list2 = Json.Deserialize(PlayerPrefs.GetString(key, "[]")) as List<object>;
		if (list2 != null && list2.Count > 0)
		{
			foreach (string item in list2.OfType<string>())
			{
				list.Add(item);
			}
		}
		return list;
	}

	private static void FillDictionary(string key, Dictionary<string, Dictionary<string, object>> dictionary)
	{
		string text = string.Empty;
		using (new StopwatchLogger("Storager extracting " + key))
		{
			text = PlayerPrefs.GetString(key, "{}");
		}
		UnityEngine.Debug.Log(key + " (length): " + text.Length);
		Dictionary<string, object> dictionary2 = null;
		using (new StopwatchLogger("Json decoding " + key))
		{
			dictionary2 = Json.Deserialize(text) as Dictionary<string, object>;
		}
		if (dictionary2 == null || dictionary2.Count <= 0)
		{
			return;
		}
		UnityEngine.Debug.Log(key + " (count): " + dictionary2.Count);
		using (new StopwatchLogger("Dictionary copying " + key))
		{
			foreach (KeyValuePair<string, object> item in dictionary2)
			{
				Dictionary<string, object> dictionary3 = item.Value as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					dictionary.Add(item.Key, dictionary3);
				}
			}
		}
	}

	private void Awake()
	{
		if (!Storager.hasKey(FacebookIDKey))
		{
			Storager.setString(FacebookIDKey, string.Empty);
		}
		id_fb = Storager.getString(FacebookIDKey);
		sharedController = this;
		NewDayWatcher.NewDay += NewDayWatcher_NewDay;
		if (!Storager.hasKey("FriendProfileView.LOBBY_LIKES_SEND_KEY"))
		{
			Storager.setString("FriendProfileView.LOBBY_LIKES_SEND_KEY", "[]");
		}
		try
		{
			playersLikesAlreadySentTo = new HashSet<string>((Json.Deserialize(Storager.getString("FriendProfileView.LOBBY_LIKES_SEND_KEY")) as List<object>).OfType<string>());
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in initializing playersLikesAlreadySentTo: {0}", ex);
		}
		if (!Storager.hasKey("FriendsController.OUR_LOBBY_LIKES_KEY"))
		{
			Storager.setString("FriendsController.OUR_LOBBY_LIKES_KEY", "{}");
		}
		ourLobbyLikes = JsonUtility.FromJson<OurLobbyLikes>(Storager.getString("FriendsController.OUR_LOBBY_LIKES_KEY"));
	}

	private void NewDayWatcher_NewDay(object sender, EventArgs e)
	{
		SendX2REwardAnalytics();
	}

	public IEnumerable<float> InitController()
	{
		string text = alphaIvory ?? string.Empty;
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogError("Secret is empty!");
		}
		StartCoroutine("GetABTestAdvertConfig");
		StartCoroutine("GetCurrentcompetition");
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted && (!Storager.hasKey("abTestBalansConfig2Key") || string.IsNullOrEmpty(Storager.getString("abTestBalansConfig2Key"))))
		{
			Storager.setString("abTestBalansConfig2Key", string.Empty);
			StartCoroutine(GetABTestBalansConfigName());
		}
		else
		{
			getCohortInfo = true;
			if (Defs.isABTestBalansCohortActual)
			{
				StartCoroutine(GetABTestBalansCohortNameActual());
			}
		}
		Task firstResponse = PersistentCacheManager.Instance.FirstResponse;
		StopCoroutine("GetBanList");
		StartCoroutine("GetBanList");
		UpdatePopularityMaps();
		StartCoroutine(GetGeoIP());
		StartCoroutine("GetTimeFromServerLoop");
		StartCoroutine(GetLocalMultySetting());
		StartCoroutine(SendGameTimeLoop());
		UpdatePixelbookSettingsFromPrefs();
		StartCoroutine(GetPixelbookSettingsLoop(firstResponse));
		StartCoroutine(GetNewsLoop(firstResponse));
		ProfileController.LoadStatisticFromKeychain();
		TrafficForwardingScript trafficForwardingScript = GetComponent<TrafficForwardingScript>() ?? gameObject.AddComponent<TrafficForwardingScript>();
		StartCoroutine(trafficForwardingScript.GetTrafficForwardingConfigLoopCoroutine());
		StartCoroutine(GetFiltersSettings());
		StartCoroutine(GetBuffSettings(firstResponse));
		StartCoroutine(GetRatingSystemConfig());
		if (FacebookController.FacebookSupported)
		{
			FacebookController.ReceivedSelfID += HandleReceivedSelfID;
		}
		lastTouchTm = Time.realtimeSinceStartup + 15f;
		friends = FillList("FriendsKey");
		StartSendReview();
		yield return 0f;
		invitesToUs = FillList("ToUsKey");
		yield return 0f;
		FillDictionary("PlayerInfoKey", playersInfo);
		FillDictionary("FriendsInfoKey", friendsInfo);
		FillDictionary("ClanFriendsInfoKey", clanFriendsInfo);
		yield return 0f;
		FillListDictionary("ClanInvitesKey", ClanInvites);
		yield return 0f;
		FillClickJoinFriendsListByCachedValue();
		yield return 0f;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!Storager.hasKey("AccountCreated"))
		{
			Storager.setString("AccountCreated", string.Empty);
		}
		id = Storager.getString("AccountCreated");
		if (string.IsNullOrEmpty(id))
		{
			UnityEngine.Debug.Log("Account id: null or empty    Calling CreatePlayer()...");
			StartCoroutine(CreatePlayer());
		}
		else
		{
			UnityEngine.Debug.LogFormat("Account id: {0}    Calling CheckOurIdExists()...", id);
			StartCoroutine(CheckOurIDExists());
		}
		SyncClickJoinFriendsListWithListFriends();
	}

	private void HandleReceivedSelfID(string idfb)
	{
		if (idfb != null && (string.IsNullOrEmpty(id_fb) || !idfb.Equals(id_fb)))
		{
			id_fb = idfb;
			if (!Storager.hasKey(FacebookIDKey))
			{
				Storager.setString(FacebookIDKey, string.Empty);
			}
			Storager.setString(FacebookIDKey, id_fb);
			SendOurData();
		}
	}

	public void UnbanUs(Action onSuccess)
	{
	}

	public void ChangeClanLogo()
	{
		if (readyToOperate)
		{
			StartCoroutine(_ChangeClanLogo());
		}
	}

	public void GetOurWins()
	{
		if (readyToOperate)
		{
			StartCoroutine(_GetOurWins());
		}
	}

	public void SendRoundWon()
	{
		if (readyToOperate)
		{
			int num = -1;
			if (PhotonNetwork.room != null)
			{
				num = (int)GameConnect.gameMode;
			}
			if (num != -1)
			{
				StartCoroutine(_SendRoundWon(num));
			}
		}
	}

	public static string Hash(string action, string token = null)
	{
		if (action == null)
		{
			UnityEngine.Debug.LogWarning("Hash: action is null");
			return string.Empty;
		}
		string text = token ?? ((sharedController != null) ? sharedController.id : null);
		if (text == null)
		{
			UnityEngine.Debug.LogWarning("Hash: Token is null");
			return string.Empty;
		}
		return "hi";
	}

	public static string HashForPush(byte[] responceData)
	{
		if (responceData == null)
		{
			UnityEngine.Debug.LogWarning("HashForPush: responceData is null");
			return string.Empty;
		}
		return "hi";
	}

	public bool IsShowAdd(string _pixelBookID)
	{
		bool flag = true;
		if (friends.Count >= Defs.maxCountFriend || _pixelBookID.Equals("-1") || _pixelBookID.Equals(sharedController.id))
		{
			return false;
		}
		if (sharedController.friends.Contains(_pixelBookID))
		{
			return false;
		}
		return !sharedController.notShowAddIds.Contains(_pixelBookID);
	}

	private IEnumerator _GetOurWins()
	{
		while (string.IsNullOrEmpty(sharedController.id) || !TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		string value = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "get_info_by_id");
		form.AddField("app_version", value);
		form.AddField("id", id);
		form.AddField("uniq_id", sharedController.id);
		form.AddField("auth", Hash("get_info_by_id"));
		string response;
		while (true)
		{
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, form);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			response = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(download.error))
			{
				string.IsNullOrEmpty(response);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("_GetOurWins error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("_GetOurWins fail.");
			}
			yield return StartCoroutine(MyWaitForSeconds(10f));
		}
		Dictionary<string, object> dictionary = ParseInfo(response);
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" _GetOurWins newInfo = null");
			}
			yield break;
		}
		ourInfo = dictionary;
		SaveProfileData();
		if (FriendsController.OurInfoUpdated != null)
		{
			FriendsController.OurInfoUpdated();
		}
	}

	private void SaveProfileData()
	{
		if (ourInfo != null && ourInfo.ContainsKey("wincount"))
		{
			int num = 0;
			Dictionary<string, object> dict = ourInfo["wincount"] as Dictionary<string, object>;
			num = 0;
			dict.TryGetValue<int>("0", out num);
			Storager.setInt(Defs.RatingDeathmatch, num);
			num = 0;
			dict.TryGetValue<int>("2", out num);
			Storager.setInt(Defs.RatingTeamBattle, num);
			num = 0;
			dict.TryGetValue<int>("3", out num);
			Storager.setInt(Defs.RatingHunger, num);
			num = 0;
			dict.TryGetValue<int>("4", out num);
			Storager.setInt(Defs.RatingCapturePoint, num);
		}
	}

	public void SendLikeLobby(string idLobbyPlayer, bool isPositiv)
	{
		StartCoroutine(SendLikeLobbyCoroutine(idLobbyPlayer, isPositiv));
	}

	public bool IsSendingLikeForPlayer(string idLobbyPlayer)
	{
		if (!idLobbyPlayer.IsNullOrEmpty())
		{
			return sendingLikesForLobbyPlayers.Contains(idLobbyPlayer);
		}
		return false;
	}

	private IEnumerator SendLikeLobbyCoroutine(string idLobbyPlayer, bool isPositiv)
	{
		if (idLobbyPlayer.IsNullOrEmpty())
		{
			UnityEngine.Debug.LogErrorFormat("SendLikeLobbyCoroutine idLobbyPlayer.IsNullOrEmpty()");
			yield break;
		}
		string value = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "evaluate_lobby");
		wWWForm.AddField("app_version", value);
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("like", isPositiv ? 1 : (-1));
		string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
		playerNameOrDefault = FilterBadWorld.FilterString(playerNameOrDefault);
		if (playerNameOrDefault.Length > 20)
		{
			playerNameOrDefault = playerNameOrDefault.Substring(0, 20);
		}
		wWWForm.AddField("name", playerNameOrDefault);
		wWWForm.AddField("player_id", idLobbyPlayer);
		wWWForm.AddField("auth", Hash("evaluate_lobby"));
		wWWForm.AddField("abuse_method", Storager.getInt("AbuseMethod"));
		sendingLikesForLobbyPlayers.Add(idLobbyPlayer);
		WWW request = Tools.CreateWww(actionAddress, wWWForm);
		yield return request;
		string text = URLs.Sanitize(request);
		sendingLikesForLobbyPlayers.Remove(idLobbyPlayer);
		if (string.IsNullOrEmpty(request.error) && !string.IsNullOrEmpty(text) && text != "exists" && text != "fail")
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("_Send evaluate_lobby: " + text);
			}
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("players_lobby"))
			{
				int num = Convert.ToInt32(dictionary["players_lobby"]);
				UpdateCountLikeInInfo(idLobbyPlayer, num);
				if (isPositiv)
				{
					playersLikesAlreadySentTo.Add(idLobbyPlayer);
				}
				else
				{
					playersLikesAlreadySentTo.Remove(idLobbyPlayer);
				}
				Action<string, bool, int> onSendLikeLobby = FriendsController.OnSendLikeLobby;
				if (onSendLikeLobby != null)
				{
					onSendLikeLobby(idLobbyPlayer, isPositiv, num);
				}
				yield break;
			}
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_Send evaluate_lobby: fail response=" + text + " error=" + request.error);
		}
		Action<string, bool> onFailSendLikeLobby = FriendsController.OnFailSendLikeLobby;
		if (onFailSendLikeLobby != null)
		{
			onFailSendLikeLobby(idLobbyPlayer, isPositiv);
		}
	}

	private void UpdateCountLikeInInfo(string _idPlayer, int countLike)
	{
		if (friendsInfo.ContainsKey(_idPlayer))
		{
			Dictionary<string, object> dictionary = friendsInfo[_idPlayer];
			if (dictionary.ContainsKey("player"))
			{
				Dictionary<string, object> dictionary2 = dictionary["player"] as Dictionary<string, object>;
				if (dictionary2.ContainsKey("lobby_likes"))
				{
					dictionary2["lobby_likes"] = countLike;
				}
			}
		}
		if (profileInfo.ContainsKey(_idPlayer))
		{
			Dictionary<string, object> dictionary3 = profileInfo[_idPlayer];
			if (dictionary3.ContainsKey("player"))
			{
				Dictionary<string, object> dictionary4 = dictionary3["player"] as Dictionary<string, object>;
				if (dictionary4.ContainsKey("lobby_likes"))
				{
					dictionary4["lobby_likes"] = countLike;
				}
			}
		}
		if (!clanFriendsInfo.ContainsKey(_idPlayer))
		{
			return;
		}
		Dictionary<string, object> dictionary5 = clanFriendsInfo[_idPlayer];
		if (dictionary5.ContainsKey("player"))
		{
			Dictionary<string, object> dictionary6 = dictionary5["player"] as Dictionary<string, object>;
			if (dictionary6.ContainsKey("lobby_likes"))
			{
				dictionary6["lobby_likes"] = countLike;
			}
		}
	}

	public void SendScoreInMiniGames(int _mode, int _scores)
	{
		StartCoroutine(SendScoreInMiniGamesCoroutine(_mode, _scores));
	}

	private IEnumerator SendScoreInMiniGamesCoroutine(int _mode, int _scores)
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "set_player_scores");
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("mode", _mode);
			wWWForm.AddField("scores", _scores);
			wWWForm.AddField("auth", Hash("set_player_scores"));
			WWW request = Tools.CreateWww(actionAddress, wWWForm);
			yield return request;
			string response = URLs.Sanitize(request);
			if (string.IsNullOrEmpty(request.error) && !string.IsNullOrEmpty(response) && UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("_Send set_player_scores: " + response);
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Send set_player_scores error: " + request.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Send set_player_scores fail.");
				}
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			break;
		}
	}

	private IEnumerator _SendRoundWon(int mode)
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "round_won");
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("mode", mode);
			wWWForm.AddField("auth", Hash("round_won"));
			WWW roundWonRequest = Tools.CreateWww(actionAddress, wWWForm);
			yield return roundWonRequest;
			string response = URLs.Sanitize(roundWonRequest);
			if (string.IsNullOrEmpty(roundWonRequest.error) && !string.IsNullOrEmpty(response) && UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("_SendRoundWon: " + response);
			}
			if (!string.IsNullOrEmpty(roundWonRequest.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("_SendRoundWon error: " + roundWonRequest.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_SendRoundWon fail.");
			}
			yield return StartCoroutine(MyWaitForSeconds(10f));
		}
		PlayerPrefs.SetInt("TotalWinsForLeaderboards", PlayerPrefs.GetInt("TotalWinsForLeaderboards", 0) + 1);
	}

	private IEnumerator _ChangeClanLogo()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "change_logo");
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("id_clan", ClanID);
			wWWForm.AddField("logo", clanLogo);
			wWWForm.AddField("id", id);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("auth", Hash("change_logo"));
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			string response = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("_ChangeClanLogo: " + response);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("_ChangeClanLogo error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("_ChangeClanLogo fail.");
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			break;
		}
	}

	public void ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		if (readyToOperate)
		{
			StartCoroutine(_ChangeClanName(newNm, onSuccess, onFailure));
		}
	}

	private IEnumerator _ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "change_clan_name");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_clan", ClanID);
		wWWForm.AddField("id", id);
		string value = FilterBadWorld.FilterString(newNm);
		wWWForm.AddField("name", value);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("change_clan_name"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			if (onFailure != null)
			{
				onFailure("Request skipped.");
			}
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_ChangeClanName: " + text);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_ChangeClanName error: " + download.error);
			}
			if (onFailure != null)
			{
				onFailure(download.error);
			}
		}
		else if (!string.IsNullOrEmpty(text) && text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_ChangeClanName fail.");
			}
			if (onFailure != null)
			{
				onFailure(text);
			}
		}
		else if (onSuccess != null)
		{
			onSuccess();
		}
	}

	private IEnumerator GetGeoIP()
	{
		Dictionary<string, object> geoDict;
		while (true)
		{
			WWW download = Tools.CreateWwwIfNotConnected(URLs.GeoIPUrl);
			if (download == null)
			{
				yield break;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetGeoIP error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			string response = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(response) || response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetGeoIP fail.");
				}
				yield return StartCoroutine(MyWaitForSeconds(18f));
				continue;
			}
			geoDict = Json.Deserialize(response) as Dictionary<string, object>;
			if (geoDict != null)
			{
				break;
			}
			if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" GetGeoIP dict = null");
			}
			yield return StartCoroutine(MyWaitForSeconds(20f));
		}
		if (geoDict.ContainsKey("country"))
		{
			string text = geoDict["country"].ToString();
			if (!string.IsNullOrEmpty(text))
			{
				Storager.setString(Defs.countryKey, text);
			}
		}
		if (geoDict.ContainsKey("continent"))
		{
			string text2 = geoDict["continent"].ToString();
			if (!string.IsNullOrEmpty(text2))
			{
				Storager.setString(Defs.continentKey, text2);
			}
		}
	}

	private IEnumerator BlockCloud(int typeBlock)
	{
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "update_abuse_info");
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("uniq_id", sharedController.id);
			wWWForm.AddField("auth", Hash("update_abuse_info"));
			wWWForm.AddField("block_id", Storager.getInt("HackDetected"));
			wWWForm.AddField("abuse_method", Storager.getInt("AbuseMethod"));
			WWW wWW = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (wWW == null || string.IsNullOrEmpty(wWW.error))
			{
				yield return null;
			}
		}
	}

	public void UpdatePopularityMaps()
	{
		StopCoroutine("GetPopularityMap");
		StartCoroutine("GetPopularityMap");
	}

	private IEnumerator GetPopularityMap()
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		Dictionary<string, object> dict;
		while (true)
		{
			WWW download = Tools.CreateWwwIfNotConnected(URLs.PopularityMapUrl);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			string response = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("CheckMapPopularity error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("CheckMapPopularity fail.");
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			object obj = Json.Deserialize(response);
			dict = obj as Dictionary<string, object>;
			if (dict != null)
			{
				break;
			}
			if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" GetPopularityMap dict = null");
			}
			yield return StartCoroutine(MyWaitForSeconds(10f));
		}
		mapPopularityDictionary.Clear();
		foreach (KeyValuePair<string, object> item in dict)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, object> dictionary2 = item.Value as Dictionary<string, object>;
			if (dictionary2 == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, object> item2 in dictionary2)
			{
				dictionary.Add(item2.Key, item2.Value.ToString());
			}
			if (dictionary.Count > 0 && !mapPopularityDictionary.ContainsKey(item.Key))
			{
				mapPopularityDictionary.Add(item.Key, dictionary);
			}
		}
		if (FriendsController.MapPopularityUpdated != null)
		{
			FriendsController.MapPopularityUpdated();
		}
	}

	private IEnumerator GetABTestBalansConfigName()
	{
		string response2;
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "get_cohort_name");
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("device", SystemInfo.deviceUniqueIdentifier);
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(1f));
				continue;
			}
			yield return download;
			response2 = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("get_cohort_name error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(1f));
				continue;
			}
			if (!"fail".Equals(response2))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("get_cohort_name fail.");
			}
			yield return StartCoroutine(MyWaitForSeconds(1f));
		}
		if ("skip".Equals(response2))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("get_cohort_name skip");
			}
			getCohortInfo = true;
		}
		else
		{
			response2 = response2.Replace("/", "-");
			StartCoroutine(GetABTestBalansConfig(response2, true));
		}
	}

	private IEnumerator GetABTestBalansConfig(string nameConfig, bool isFirst)
	{
		WWW download;
		while (true)
		{
			new WWWForm();
			download = Tools.CreateWwwIfNotConnected(nameConfig.Equals("none") ? URLs.ABTestBalansURL : (URLs.ABTestBalansFolderURL + nameConfig + ".json"));
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(1f));
				continue;
			}
			yield return download;
			if (nameConfig.Equals("none") && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted))
			{
				getCohortInfo = true;
				yield break;
			}
			if (string.IsNullOrEmpty(download.error))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("GetABTestBalansConfig error: " + download.error);
			}
			yield return StartCoroutine(MyWaitForSeconds(1f));
		}
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(text) && text != Storager.getString("abTestBalansConfig2Key"))
		{
			Storager.setString("abTestBalansConfig2Key", text);
			getCohortInfo = true;
			if (!nameConfig.Equals("none"))
			{
				Defs.isABTestBalansNoneSkip = true;
			}
			BalanceController.sharedController.ParseConfig(isFirst);
			if (isFirst && Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
			{
				StartCoroutine(GetABTestBalansCohortNameActual());
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("GetConfigABtestBalans");
			}
		}
	}

	private IEnumerator GetABTestBalansCohortNameActual()
	{
		while (true)
		{
			new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(URLs.ABTestBalansActualCohortNameURL);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(60f));
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetABTestBalansCohortNameActual error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(60f));
				continue;
			}
			string text = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(text))
			{
				Dictionary<string, object> _nameCohortDict = Json.Deserialize(text) as Dictionary<string, object>;
				if (_nameCohortDict == null)
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.LogWarning("GetABTestBalansCohortNameActual parse error");
					}
					yield return StartCoroutine(MyWaitForSeconds(60f));
					continue;
				}
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log("GetConfigABtestBalans");
				}
				if (!Convert.ToString(_nameCohortDict["ActualCohortNameB"]).Equals(Defs.abTestBalansCohortName))
				{
					break;
				}
				StartCoroutine(GetABTestBalansConfig(Defs.abTestBalansCohortName, false));
				yield return StartCoroutine(MyWaitForSeconds(900f));
			}
			else
			{
				yield return StartCoroutine(MyWaitForSeconds(60f));
			}
		}
		Defs.isABTestBalansCohortActual = false;
		ResetABTestsBalans();
	}

	public static void ResetABTestsBalans()
	{
		Storager.setString("abTestBalansConfig2Key", string.Empty);
		if (BalanceController.sharedController != null)
		{
			BalanceController.sharedController.ParseConfig();
		}
		Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
		Defs.abTestBalansCohortName = string.Empty;
		Defs.isABTestBalansCohortActual = false;
	}

	private IEnumerator GetBanList()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		int ban;
		while (true)
		{
			if (string.IsNullOrEmpty(id))
			{
				yield return null;
				continue;
			}
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("id", id);
			WWW download = Tools.CreateWwwIfNotConnected(URLs.BanURL, wWWForm);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetBanList error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (int.TryParse(URLs.Sanitize(download), out ban))
			{
				break;
			}
			UnityEngine.Debug.LogWarning("GetBanList cannot parse ban!");
			yield return StartCoroutine(MyWaitForSeconds(10f));
		}
		Banned = ban;
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("GetBanList Banned: " + Banned);
		}
	}

	private IEnumerator CheckOurIDExists()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		string response;
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "start_check");
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			wWWForm.AddField("uniq_id", sharedController.id);
			wWWForm.AddField("device_model", SystemInfo.deviceModel);
			wWWForm.AddField("type_device", Device.isPixelGunLow ? 1 : 2);
			wWWForm.AddField("auth", Hash("start_check"));
			wWWForm.AddField("abuse_method", Storager.getInt("AbuseMethod"));
			bool hasValue = Launcher.PackageInfo.HasValue;
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
				continue;
			}
			yield return download;
			response = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("CheckOurIDExists error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
				continue;
			}
			if (!"fail".Equals(response))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("CheckOurIDExists fail.");
			}
			yield return StartCoroutine(MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
		}
		int result;
		if (!int.TryParse(response, out result))
		{
			Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
			if (dictionary == null)
			{
				UnityEngine.Debug.LogWarning("CheckOurIDExists cannot parse clan info!");
			}
			else
			{
				object value;
				if (dictionary.TryGetValue("id", out value) && value != null && !value.Equals("null"))
				{
					ClanID = Convert.ToString(value);
				}
				object value2;
				if (dictionary.TryGetValue("creator_id", out value2) && value2 != null && !value2.Equals("null"))
				{
					clanLeaderID = value2 as string;
				}
				object value3;
				if (dictionary.TryGetValue("name", out value3) && value3 != null && !value3.Equals("null"))
				{
					_prevClanName = clanName;
					clanName = value3 as string;
					if (!_prevClanName.Equals(clanName) && onChangeClanName != null)
					{
						onChangeClanName(clanName);
					}
				}
				object value4;
				if (dictionary.TryGetValue("logo", out value4) && value4 != null && !value4.Equals("null"))
				{
					clanLogo = value4 as string;
				}
			}
		}
		else
		{
			Storager.setString("AccountCreated", response);
			id = response;
			onlineInfo.Clear();
			friends.Clear();
			invitesFromUs.Clear();
			playersInfo.Clear();
			invitesToUs.Clear();
			ClanInvites.Clear();
			notShowAddIds.Clear();
			SaveCurrentState();
			PlayerPrefs.Save();
		}
		readyToOperate = true;
		StartCoroutine(GetFriendsDataLoop());
		StartCoroutine(GetClanDataLoop());
		GetOurLAstOnline();
		StartCoroutine(RequestWinCountTimestampCoroutine());
		GetOurWins();
	}

	public void InitOurInfo()
	{
		nick = ProfileController.GetPlayerNameOrDefault();
		byte[] inArray = SkinsController.currentSkinForPers.EncodeToPNG();
		skin = Convert.ToBase64String(inArray);
		rank = ExperienceController.sharedController.currentLevel;
		wins = Storager.getInt("Rating");
		survivalScore = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
		coopScore = PlayerPrefs.GetInt(Defs.COOPScore, 0);
		infoLoaded = true;
	}

	public IEnumerator WaitForReadyToOperateAndUpdatePlayer()
	{
		while (!readyToOperate)
		{
			yield return null;
		}
		StartCoroutine(UpdatePlayer(true));
	}

	public void SendOurData(bool SendSkin = false)
	{
		if (readyToOperate)
		{
			StartCoroutine(UpdatePlayer(SendSkin));
		}
	}

	public void SendOurDataInConnectScene()
	{
		if (!(Time.realtimeSinceStartup - timeSendUpdatePlayer < timeOutSendUpdatePlayerFromConnectScene) && readyToOperate)
		{
			StartCoroutine(UpdatePlayer(false));
		}
	}

	private void SaveCurrentState()
	{
		if (friends != null)
		{
			string text = Json.Serialize(friends);
			PlayerPrefs.SetString("FriendsKey", text ?? "[]");
		}
		if (invitesToUs != null)
		{
			string text2 = Json.Serialize(invitesToUs);
			PlayerPrefs.SetString("ToUsKey", text2 ?? "[]");
		}
		if (playersInfo != null)
		{
			string text3 = Json.Serialize(playersInfo);
			PlayerPrefs.SetString("PlayerInfoKey", text3 ?? "{}");
		}
		if (friendsInfo != null)
		{
			string text4 = Json.Serialize(friendsInfo);
			PlayerPrefs.SetString("FriendsInfoKey", text4 ?? "{}");
		}
		if (clanFriendsInfo != null)
		{
			string text5 = Json.Serialize(clanFriendsInfo);
			PlayerPrefs.SetString("ClanFriendsInfoKey", text5 ?? "{}");
		}
		if (ClanInvites != null)
		{
			string text6 = Json.Serialize(ClanInvites);
			PlayerPrefs.SetString("ClanInvitesKey", text6 ?? "[]");
		}
		UpdateCachedClickJoinListValue();
	}

	private void DumpCurrentState()
	{
	}

	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			DumpCurrentState();
			AnalyticsStuff.SaveTrainingStep();
			SaveSentLikes();
			SaveOurLikes();
			yield break;
		}
		isUpdateServerTimeAfterRun = false;
		firstUpdateAfterApplicationPause = true;
		yield return null;
		yield return null;
		yield return null;
		StartSendReview();
		if (GiftBannerWindow.instance != null)
		{
			GiftBannerWindow.instance.ForceCloseAll();
		}
		StopCoroutine("GetBanList");
		StartCoroutine("GetBanList");
		if (Defs.isABTestBalansCohortActual)
		{
			StopCoroutine(GetABTestBalansCohortNameActual());
			StartCoroutine(GetABTestBalansCohortNameActual());
		}
		StopCoroutine("GetABTestAdvertConfig");
		StartCoroutine("GetABTestAdvertConfig");
		StartCoroutine("GetCurrentcompetition");
		StartCoroutine("GetCurrentcompetition");
		UpdatePopularityMaps();
		StopCoroutine("GetTimeFromServerLoop");
		StartCoroutine("GetTimeFromServerLoop");
		StartCoroutine(GetFiltersSettings());
		StartCoroutine(GetLobbyNews(true));
		Task firstResponse = PersistentCacheManager.Instance.FirstResponse;
		StartCoroutine(GetBuffSettings(firstResponse));
		StartCoroutine(GetRatingSystemConfig());
		GetFriendsData(true);
		FastGetPixelbookSettings();
		if (SceneLoader.ActiveSceneName.Equals("Friends") && FriendsGUIController.ShowProfile && FriendProfileController.currentFriendId != null && readyToOperate)
		{
			StartCoroutine(UpdatePlayerInfoById(FriendProfileController.currentFriendId));
		}
		if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			if (!string.IsNullOrEmpty(ClanID))
			{
				StartCoroutine(GetClanDataOnce());
			}
			if (ClansGUIController.AtAddPanel)
			{
				StartCoroutine(GetAllPlayersOnline());
			}
			else
			{
				StartCoroutine(GetClanPlayersOnline());
			}
			if (ClansGUIController.ShowProfile && FriendProfileController.currentFriendId != null && readyToOperate)
			{
				StartCoroutine(UpdatePlayerInfoById(FriendProfileController.currentFriendId));
			}
		}
	}

	private void SaveSentLikes()
	{
		try
		{
			Storager.setString("FriendProfileView.LOBBY_LIKES_SEND_KEY", Json.Serialize(playersLikesAlreadySentTo.ToList()));
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in saving playersLikesAlreadySentTo: {0}", ex);
		}
	}

	private void SaveOurLikes()
	{
		try
		{
			Storager.setString("FriendsController.OUR_LOBBY_LIKES_KEY", JsonUtility.ToJson(ourLobbyLikes));
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in saving ourLobbyLikes: {0}", ex);
		}
	}

	private void OnDestroy()
	{
		NewDayWatcher.NewDay -= NewDayWatcher_NewDay;
		DumpCurrentState();
	}

	public void SendInvitation(string personId, Dictionary<string, object> socialEventParameters)
	{
		if (!string.IsNullOrEmpty(personId) && readyToOperate)
		{
			if (socialEventParameters == null)
			{
				throw new ArgumentNullException("socialEventParameters");
			}
			StartCoroutine(FriendRequest(personId, socialEventParameters));
		}
	}

	public void SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(nameClan) && !string.IsNullOrEmpty(skinClan) && readyToOperate)
		{
			StartCoroutine(_SendCreateClan(personId, nameClan, skinClan, ErrorHandler));
		}
		else if (ErrorHandler != null)
		{
			ErrorHandler("Error: FALSE:  ! string.IsNullOrEmpty (personId) && ! string.IsNullOrEmpty (nameClan) && ! string.IsNullOrEmpty (skinClan) && readyToOperate");
		}
	}

	public static void SendPlayerInviteToClan(string personId, Action<bool, bool> callbackResult = null)
	{
		if (!(sharedController == null) && !string.IsNullOrEmpty(personId) && readyToOperate)
		{
			sharedController.StartCoroutine(sharedController.SendClanInvitation(personId, callbackResult));
		}
	}

	public void AcceptInvite(string accepteeId, Action<bool> action = null)
	{
		if (!string.IsNullOrEmpty(accepteeId) && readyToOperate)
		{
			StartCoroutine(AcceptFriend(accepteeId, action));
		}
	}

	public void AcceptClanInvite(string recordId)
	{
		if (!string.IsNullOrEmpty(recordId) && readyToOperate)
		{
			StartCoroutine(_AcceptClanInvite(recordId));
		}
	}

	private IEnumerator _AcceptClanInvite(string recordId)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "accept_invite");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", id);
		wWWForm.AddField("id_clan", recordId);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("accept_invite"));
		WWW acceptInviteRequest = Tools.CreateWww(actionAddress, wWWForm);
		JoinClanSent = recordId;
		yield return acceptInviteRequest;
		string text = URLs.Sanitize(acceptInviteRequest);
		if (string.IsNullOrEmpty(acceptInviteRequest.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Accept clan invite: " + text);
		}
		if (!string.IsNullOrEmpty(acceptInviteRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_AcceptClanInvite error: " + acceptInviteRequest.error);
			}
			JoinClanSent = null;
		}
		else if (!string.IsNullOrEmpty(text) && text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_AcceptClanInvite fail.");
			}
			JoinClanSent = null;
		}
		else
		{
			clanLogo = tempClanLogo;
			ClanID = tempClanID;
			clanName = tempClanName;
			clanLeaderID = tempClanCreatorID;
		}
	}

	public void StartRefreshingOnline()
	{
		if (readyToOperate)
		{
			_shouldStopOnline = false;
			StartCoroutine(RefreshOnlinePlayer());
		}
	}

	public void StopRefreshingOnline()
	{
		if (readyToOperate)
		{
			_shouldStopOnline = true;
		}
	}

	public void StartRefreshingOnlineWithClanInfo()
	{
		if (readyToOperate)
		{
			_shouldStopOnlineWithClanInfo = false;
			StartCoroutine(RefreshOnlinePlayerWithClanInfo());
		}
	}

	public void StopRefreshingOnlineWithClanInfo()
	{
		if (readyToOperate)
		{
			_shouldStopOnlineWithClanInfo = true;
		}
	}

	private IEnumerator RefreshOnlinePlayerWithClanInfo()
	{
		while (true)
		{
			if (idle)
			{
				yield return null;
				continue;
			}
			StartCoroutine(GetAllPlayersOnlineWithClanInfo());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !_shouldStopOnlineWithClanInfo);
			if (_shouldStopOnlineWithClanInfo)
			{
				break;
			}
		}
		_shouldStopOnlineWithClanInfo = false;
	}

	private IEnumerator RefreshOnlinePlayer()
	{
		while (true)
		{
			if (idle)
			{
				yield return null;
				continue;
			}
			StartCoroutine(GetAllPlayersOnline());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !_shouldStopOnline);
			if (_shouldStopOnline)
			{
				break;
			}
		}
		_shouldStopOnline = false;
	}

	public void StartRefreshingClanOnline()
	{
		if (readyToOperate)
		{
			_shouldStopRefrClanOnline = false;
			StartCoroutine(RefreshClanOnline());
		}
	}

	public void StopRefreshingClanOnline()
	{
		if (readyToOperate)
		{
			_shouldStopRefrClanOnline = true;
		}
	}

	private IEnumerator RefreshClanOnline()
	{
		while (true)
		{
			if (idle)
			{
				yield return null;
				continue;
			}
			StartCoroutine(GetClanPlayersOnline());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !_shouldStopRefrClanOnline);
			if (_shouldStopRefrClanOnline)
			{
				break;
			}
		}
		_shouldStopRefrClanOnline = false;
	}

	private IEnumerator GetClanPlayersOnline()
	{
		if (!readyToOperate)
		{
			yield break;
		}
		List<string> list = new List<string>();
		foreach (Dictionary<string, string> clanMember in clanMembers)
		{
			string value;
			if (clanMember.TryGetValue("id", out value))
			{
				list.Add(value);
			}
		}
		yield return StartCoroutine(_GetOnlineForPlayerIDs(list));
	}

	private IEnumerator GetAllPlayersOnline()
	{
		if (readyToOperate)
		{
			yield return StartCoroutine(_GetOnlineForPlayerIDs(friends));
		}
	}

	private IEnumerator GetAllPlayersOnlineWithClanInfo()
	{
		if (readyToOperate)
		{
			yield return StartCoroutine(_GetOnlineWithClanInfoForPlayerIDs(friends));
		}
	}
	
	private IEnumerator _GetOnlineForPlayerIDs(List<string> ids)
	{
		if (ids.Count == 0)
		{
			yield break;
		}
		string text = Json.Serialize(ids);
		if (text == null)
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_all_players_online");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", id);
		wWWForm.AddField("ids", text);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_all_players_online"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text2 = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text2) && (UnityEngine.Debug.isDebugBuild || Application.isEditor))
		{
			UnityEngine.Debug.Log(text2);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("GetAllPlayersOnline error: " + download.error);
			}
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text2) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetAllPlayersOnline info = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in dictionary.Keys)
		{
			Dictionary<string, object> obj = dictionary[key] as Dictionary<string, object>;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item in obj)
			{
				dictionary3.Add(item.Key, item.Value as string);
			}
			dictionary2.Add(key, dictionary3);
		}
		onlineInfo.Clear();
		foreach (string key2 in dictionary2.Keys)
		{
			Dictionary<string, string> dictionary4 = dictionary2[key2];
			int num = int.Parse(dictionary4["game_mode"]);
			int num2 = num - num / 100 * 100;
			if (num2 == 2 || num2 == 0 || num2 == 5 || num2 == 4 || num2 == 6 || num2 == 7)
			{
				if (!onlineInfo.ContainsKey(key2))
				{
					onlineInfo.Add(key2, dictionary4);
				}
				else
				{
					onlineInfo[key2] = dictionary4;
				}
			}
		}
	}

	private IEnumerator _GetOnlineWithClanInfoForPlayerIDs(List<string> ids)
	{
		if (ids.Count == 0)
		{
			yield break;
		}
		string text = Json.Serialize(ids);
		if (text == null)
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_all_players_online_with_clan_info");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", id);
		wWWForm.AddField("ids", text);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_all_players_online_with_clan_info"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text2 = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text2) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(text2);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_GetOnlineWithClanInfoForPlayerIDs error: " + download.error);
			}
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text2) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs allDict = null");
			}
			yield break;
		}
		Dictionary<string, object> dictionary2 = dictionary["online"] as Dictionary<string, object>;
		if (dictionary2 == null)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs __list = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> dictionary3 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in dictionary2.Keys)
		{
			Dictionary<string, object> obj = dictionary2[key] as Dictionary<string, object>;
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item in obj)
			{
				dictionary4.Add(item.Key, item.Value as string);
			}
			dictionary3.Add(key, dictionary4);
		}
		onlineInfo.Clear();
		foreach (string key2 in dictionary3.Keys)
		{
			Dictionary<string, string> dictionary5 = dictionary3[key2];
			int num = int.Parse(dictionary5["game_mode"]);
			int num2 = num - num / 100 * 100;
			if (num2 == 2 || num2 == 0 || num2 == 5 || num2 == 4 || num2 == 6 || num2 == 7)
			{
				if (!onlineInfo.ContainsKey(key2))
				{
					onlineInfo.Add(key2, dictionary5);
				}
				else
				{
					onlineInfo[key2] = dictionary5;
				}
			}
		}
		Dictionary<string, object> dictionary6 = dictionary["clan_info"] as Dictionary<string, object>;
		if (dictionary6 == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs clanInfo = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> dictionary7 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key3 in dictionary6.Keys)
		{
			Dictionary<string, object> obj2 = dictionary6[key3] as Dictionary<string, object>;
			Dictionary<string, string> dictionary8 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item2 in obj2)
			{
				dictionary8.Add(item2.Key, Convert.ToString(item2.Value));
			}
			dictionary7.Add(key3, dictionary8);
		}
		foreach (string key4 in dictionary7.Keys)
		{
			Dictionary<string, string> dictionary9 = dictionary7[key4];
			if (!sharedController.playersInfo.ContainsKey(key4))
			{
				continue;
			}
			Dictionary<string, object> dictionary10 = sharedController.playersInfo[key4];
			if (dictionary10.ContainsKey("player"))
			{
				Dictionary<string, object> dictionary11 = dictionary10["player"] as Dictionary<string, object>;
				if (dictionary11.ContainsKey("clan_creator_id"))
				{
					dictionary11["clan_creator_id"] = dictionary9["clan_creator_id"];
				}
				else
				{
					dictionary11.Add("clan_creator_id", dictionary9["clan_creator_id"]);
				}
				if (dictionary11.ContainsKey("clan_name"))
				{
					dictionary11["clan_name"] = dictionary9["clan_name"];
				}
				else
				{
					dictionary11.Add("clan_name", dictionary9["clan_name"]);
				}
				if (dictionary11.ContainsKey("clan_logo"))
				{
					dictionary11["clan_logo"] = dictionary9["clan_logo"];
				}
				else
				{
					dictionary11.Add("clan_logo", dictionary9["clan_logo"]);
				}
			}
		}
	}

	public void GetFacebookFriendsInfo(Action callb)
	{
		if (readyToOperate)
		{
			StartCoroutine(_GetFacebookFriendsInfo(callb));
		}
	}

	private IEnumerator _GetFacebookFriendsInfo(Action callb)
	{
		if (!FacebookController.FacebookSupported || FacebookController.sharedController.friendsList == null)
		{
			yield break;
		}
		GetFacebookFriendsCallback = callb;
		List<string> list = new List<string>();
		foreach (Friend friends in FacebookController.sharedController.friendsList)
		{
			list.Add(friends.id);
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_info_by_facebook_ids");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		wWWForm.AddField("id", id);
		string text = Json.Serialize(list);
		wWWForm.AddField("ids", text);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_info_by_facebook_ids"));
		UnityEngine.Debug.LogFormat("Facebook json: {0}", text);
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			GetFacebookFriendsCallback = null;
			yield break;
		}
		yield return download;
		string text2 = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text2) && (UnityEngine.Debug.isDebugBuild || Application.isEditor))
		{
			UnityEngine.Debug.Log(text2);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("_GetFacebookFriendsInfo error: " + download.error);
			}
			GetFacebookFriendsCallback = null;
			yield break;
		}
		List<object> list2 = Json.Deserialize(text2) as List<object>;
		if (list2 == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" _GetFacebookFriendsInfo info = null");
			}
			GetFacebookFriendsCallback = null;
			yield break;
		}
		foreach (Dictionary<string, object> item in list2)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> item2 in item)
			{
				dictionary.Add(item2.Key, item2.Value);
			}
			object value;
			if (dictionary.TryGetValue("id", out value))
			{
				facebookFriendsInfo.Add(value as string, dictionary);
			}
		}
		if (GetFacebookFriendsCallback != null)
		{
			GetFacebookFriendsCallback();
		}
		GetFacebookFriendsCallback = null;
	}

	private IEnumerator UpdatePlayerOnlineLoop()
	{
		while (true)
		{
			if (idle)
			{
				yield return null;
				continue;
			}
			int num = -1;
			int num2 = (int)GameConnect.myPlatformConnect;
			if (PhotonNetwork.room != null)
			{
				num = (int)GameConnect.gameMode;
				if (!string.IsNullOrEmpty(PhotonNetwork.room.customProperties[GameConnect.passwordProperty].ToString()))
				{
					num2 = 3;
				}
			}
			if (num != -1)
			{
				StartCoroutine(UpdatePlayerOnline(100000 + num2 * 10000 + GameConnect.gameTier * 100 + num));
			}
			yield return StartCoroutine(MyWaitForSeconds(Defs.timeUpdateOnlineInGame));
		}
	}

	public void SendAddPurchaseEvent(string purchaseId, string transactionId, float parsedPrice, string currencyCode, string countryCode)
	{
		int inapp = 0;
		int num = Array.IndexOf(StoreKitEventListener.coinIds, purchaseId);
		if (num != -1)
		{
			inapp = VirtualCurrencyHelper.coinPriceIds[num];
		}
		else
		{
			num = Array.IndexOf(StoreKitEventListener.gemsIds, purchaseId);
			if (num != -1)
			{
				inapp = VirtualCurrencyHelper.gemsPriceIds[num];
			}
		}
		StartCoroutine(AddPurchaseEvent(inapp, purchaseId, transactionId, parsedPrice, currencyCode, countryCode));
	}

	private IEnumerator AddPurchaseEvent(int inapp, string purchaseId, string transactionId, float parsedPrice, string currencyCode, string countryCode)
	{
		WaitForSeconds awaiter = new WaitForSeconds(5f);
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "add_purchase");
			wWWForm.AddField("auth", Hash("add_purchase"));
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			wWWForm.AddField("type_device", Device.isPixelGunLow ? 1 : 2);
			int i = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
			wWWForm.AddField("rank", i);
			wWWForm.AddField("inapp", inapp);
			wWWForm.AddField("transactionId", transactionId);
			wWWForm.AddField("parsedPrice", Mathf.RoundToInt(parsedPrice * 1000f).ToString());
			wWWForm.AddField("currencyCode", currencyCode);
			wWWForm.AddField("countryCode", countryCode);
			wWWForm.AddField("tier", ExpController.OurTierForAnyPlace());
			string @string = Storager.getString(Defs.countryKey);
			if (!string.IsNullOrEmpty(@string))
			{
				wWWForm.AddField("country", @string);
			}
			string string2 = Storager.getString(Defs.continentKey);
			if (!string.IsNullOrEmpty(string2))
			{
				wWWForm.AddField("continent", string2);
			}
			if (Defs.abTestBalansCohort != 0 && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
			{
				wWWForm.AddField("cohortName", Defs.abTestBalansCohortName);
			}
			if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
			{
				string value = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
				wWWForm.AddField("cohort_ad", value);
			}
			foreach (ABTestBase currentABTest in ABTestController.currentABTests)
			{
				if (currentABTest.cohort == ABTestController.ABTestCohortsType.A || currentABTest.cohort == ABTestController.ABTestCohortsType.B)
				{
					wWWForm.AddField(currentABTest.currentFolder, currentABTest.cohortName);
				}
			}
			wWWForm.AddField("purchaseId", purchaseId);
			float result = 0f;
			float result2 = 0f;
			if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime"), out result))
			{
				wWWForm.AddField("playTime", Mathf.RoundToInt(result));
			}
			if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch"), out result2))
			{
				wWWForm.AddField("playTimeInMatch", Mathf.RoundToInt(result2));
			}
			WWW addPurchaseEventRequest = Tools.CreateWww(actionAddress, wWWForm);
			yield return addPurchaseEventRequest;
			string response = URLs.Sanitize(addPurchaseEventRequest);
			if (!string.IsNullOrEmpty(addPurchaseEventRequest.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("AddPurchaseEvent error: " + addPurchaseEventRequest.error);
				}
				yield return awaiter;
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("AddPurchaseEvent fail.");
				}
				yield return awaiter;
				continue;
			}
			break;
		}
	}

	public static void SendToturialEvent(int _event, string _progress)
	{
		CoroutineRunner.Instance.StartCoroutine(AddToturialEvent(_event, _progress));
	}

	private static IEnumerator AddToturialEvent(int _event, string _progress)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("event_id", _event);
		wWWForm.AddField("progress", _progress);
		wWWForm.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
		wWWForm.AddField("device_model", SystemInfo.deviceModel);
		wWWForm.AddField("type_device", Device.isPixelGunLow ? 1 : 2);
		wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		wWWForm.AddField("version", GlobalGameController.AppVersion);
		wWWForm.AddField("release", (!Defs.IsDeveloperBuild) ? 1 : 0);
		WWW addToturialEventRequest = Tools.CreateWww("https://acct.pixelgunserver.com/events/add_event.php", wWWForm);
		yield return addToturialEventRequest;
		string text = URLs.Sanitize(addToturialEventRequest);
		if (!string.IsNullOrEmpty(addToturialEventRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("Toturial Event error: " + addToturialEventRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(text) && text.Equals("fail") && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning("Toturial Event fail.");
		}
	}

	public void SendRequestGetCurrentcompetition()
	{
		StartCoroutine("GetCurrentcompetition");
	}

	public IEnumerator GetCurrentcompetition()
	{
		string response;
		while (true)
		{
			if (string.IsNullOrEmpty(id))
			{
				yield return null;
				continue;
			}
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "get_current_competition");
			wWWForm.AddField("auth", Hash("get_current_competition"));
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("uniq_id", id);
			WWW getCurrentSeasonRequest = Tools.CreateWww(actionAddress, wWWForm);
			yield return getCurrentSeasonRequest;
			if (!string.IsNullOrEmpty(getCurrentSeasonRequest.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("GetCurrentcompetitionRequest error: " + getCurrentSeasonRequest.error);
				}
				yield return new WaitForSeconds(20f);
				continue;
			}
			response = URLs.Sanitize(getCurrentSeasonRequest);
			if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetCurrentcompetitionnRequest fail.");
			}
			yield return new WaitForSeconds(20f);
		}
		ParseResponseCurrenCompetion(response);
	}

	public IEnumerator SynchRating(int rating)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "synch_rating_tiers");
		wWWForm.AddField("auth", Hash("synch_rating_tiers"));
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("rating", rating);
		wWWForm.AddField("abuse_method", Storager.getInt("AbuseMethod"));
		wWWForm.AddField("tier", ExpController.OurTierForAnyPlace());
		wWWForm.AddField("competition_id", currentCompetition);
		WWW synchRatingRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return synchRatingRequest;
		string text = URLs.Sanitize(synchRatingRequest);
		if (!string.IsNullOrEmpty(synchRatingRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("synchRatingRequest error: " + synchRatingRequest.error);
			}
		}
		else if (string.IsNullOrEmpty(text) || (!text.Equals("fail") && !text.Equals("ok")))
		{
			ParseResponseCurrenCompetion(text);
		}
	}

	private void ParseResponseCurrenCompetion(string _response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(_response) as Dictionary<string, object>;
		bool flag = false;
		int result;
		if (dictionary.ContainsKey("competition_id") && int.TryParse(dictionary["competition_id"].ToString(), out result))
		{
			CurrentServerSeason = result;
			if (currentCompetition != result)
			{
				StartNewCompetion();
				flag = true;
				currentCompetition = result;
			}
		}
		if (dictionary.ContainsKey("competition_time"))
		{
			expirationTimeCompetition = Convert.ToSingle(dictionary["competition_time"]);
		}
		bool flag2 = false;
		if (dictionary.ContainsKey("reward"))
		{
			Dictionary<string, object> dictionary2 = dictionary["reward"] as Dictionary<string, object>;
			if (dictionary2 != null && dictionary2.ContainsKey("place") && Convert.ToInt32(dictionary2["place"]) <= BalanceController.countPlaceAwardInCompetion)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			if (flag2)
			{
				TournamentWinnerBannerWindow.CanShow = true;
			}
			else if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
			{
				TournamentLooserBannerWindow.CanShow = true;
			}
			int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
			if (RatingSystem.instance.currentRating > trophiesSeasonThreshold)
			{
				int num = RatingSystem.instance.currentRating - trophiesSeasonThreshold;
				RatingSystem.instance.negativeRating += num;
				RatingSystem.instance.UpdateLeagueEvent();
			}
		}
	}

	private void StartNewCompetion()
	{
		if (!string.IsNullOrEmpty(id))
		{
			LeaderboardScript.RequestLeaderboards(id);
		}
	}

	private IEnumerator UpdatePlayerOnline(int gameMode)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "update_player_online");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", id);
		wWWForm.AddField("game_mode", gameMode.ToString("D6"));
		wWWForm.AddField("room_name", (PhotonNetwork.room != null && PhotonNetwork.room.name != null) ? PhotonNetwork.room.name : string.Empty);
		wWWForm.AddField("map", (PhotonNetwork.room != null && PhotonNetwork.room.customProperties != null) ? PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString() : string.Empty);
		wWWForm.AddField("protocol", GlobalGameController.MultiplayerProtocolVersion);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("update_player_online"));
		wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		wWWForm.AddField("type_device", Device.isPixelGunLow ? 1 : 2);
		wWWForm.AddField("game_time", Mathf.RoundToInt(deltaTimeInGame));
		sendingTime = Mathf.RoundToInt(deltaTimeInGame);
		wWWForm.AddField("tier", ExpController.OurTierForAnyPlace());
		if (Defs.useRatingLobbySystem)
		{
			wWWForm.AddField("league", (int)RatingSystem.instance.currentLeague);
		}
		if (Defs.abTestBalansCohort != 0 && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
		{
			wWWForm.AddField("cohortName", Defs.abTestBalansCohortName);
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string value = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
			wWWForm.AddField("cohort_ad", value);
		}
		foreach (ABTestBase currentABTest in ABTestController.currentABTests)
		{
			if (currentABTest.cohort == ABTestController.ABTestCohortsType.A || currentABTest.cohort == ABTestController.ABTestCohortsType.B)
			{
				wWWForm.AddField(currentABTest.currentFolder, currentABTest.cohortName);
			}
		}
		wWWForm.AddField("paying", Storager.getInt("PayingUser").ToString());
		int i = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
		wWWForm.AddField("rank", i);
		WWW updatePlayerOnlineRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return updatePlayerOnlineRequest;
		string text = URLs.Sanitize(updatePlayerOnlineRequest);
		if (!string.IsNullOrEmpty(updatePlayerOnlineRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("UpdatePlayerOnline error: " + updatePlayerOnlineRequest.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(text) && text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("UpdatePlayerOnline fail.");
			}
			yield break;
		}
		deltaTimeInGame -= sendingTime;
		if (!string.IsNullOrEmpty(text) && !text.Equals("ok"))
		{
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("fight_invites"))
			{
				ParseFightInvite(dictionary["fight_invites"] as List<object>);
			}
		}
	}

	private IEnumerator GetToken()
	{
		string value = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "create_player_intent");
		form.AddField("app_version", value);
		string response;
		while (true)
		{
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, form);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			response = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("create_player_intent error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("create_player_intent fail.");
				}
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (response != null)
			{
				break;
			}
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("create_player_intent response == null");
			}
			yield return StartCoroutine(MyWaitForSeconds(10f));
		}
		_inputToken = response;
	}

	private IEnumerator CreatePlayer()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		Dictionary<string, object> responseDict;
		while (true)
		{
			yield return StartCoroutine(GetToken());
			while (string.IsNullOrEmpty(_inputToken))
			{
				yield return null;
			}
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "create_player");
			wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			wWWForm.AddField("device", SystemInfo.deviceUniqueIdentifier);
			wWWForm.AddField("device_model", SystemInfo.deviceModel);
			wWWForm.AddField("app_version", appVersionField);
			wWWForm.AddField("block_param", 1);
			string text = Hash("create_player", _inputToken);
			wWWForm.AddField("auth", text);
			wWWForm.AddField("token", _inputToken);
			if (Defs.IsDeveloperBuild)
			{
				wWWForm.AddField("dev", 1);
			}
			string tokenHashString = string.Format("token:hash = {0}:{1}", new object[2] { _inputToken, text });
			_inputToken = null;
			bool canPrintSecuritySensitiveInfo = UnityEngine.Debug.isDebugBuild || Defs.IsDeveloperBuild;
			if (canPrintSecuritySensitiveInfo)
			{
				UnityEngine.Debug.Log("CreatePlayer: Trying to perform request for “" + tokenHashString + "”...");
			}
			WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			yield return download;
			string response = URLs.Sanitize(download);
			if (canPrintSecuritySensitiveInfo)
			{
				UnityEngine.Debug.LogFormat("CreatePlayer: Response for '{0}' received:    '{1}'", tokenHashString, response);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				UnityEngine.Debug.LogWarning("CreatePlayer error:    " + download.error);
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				UnityEngine.Debug.LogWarning("CreatePlayer failed.");
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (string.IsNullOrEmpty(response))
			{
				UnityEngine.Debug.LogWarning("CreatePlayer response is empty.");
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			responseDict = Json.Deserialize(response) as Dictionary<string, object>;
			if (responseDict == null)
			{
				UnityEngine.Debug.LogWarning("CreatePlayer responseDict is empty.");
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (!responseDict.ContainsKey("id"))
			{
				break;
			}
			long resultId;
			if (!long.TryParse(responseDict["id"].ToString(), out resultId))
			{
				UnityEngine.Debug.LogWarning("CreatePlayer parsing error in response:    “" + response + "”");
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			if (resultId < 1)
			{
				UnityEngine.Debug.LogWarning("CreatePlayer bad id:    “" + response + "”");
				yield return StartCoroutine(MyWaitForSeconds(10f));
				continue;
			}
			UnityEngine.Debug.Log("CreatePlayer succeeded with response:    “" + responseDict["id"].ToString() + "”");
			Storager.setString("AccountCreated", responseDict["id"].ToString());
			id = responseDict["id"].ToString();
			break;
		}
		int result;
		if (responseDict.ContainsKey("block_value") && int.TryParse(responseDict["block_value"].ToString(), out result) && result > 0)
		{
			Storager.setInt("HackDetected", result);
		}
		readyToOperate = true;
		StartCoroutine(GetFriendsDataLoop());
		StartCoroutine(GetClanDataLoop());
		GetOurLAstOnline();
		GetOurWins();
	}

	private void SetWinCountTimestamp(string timestamp, int winCount)
	{
		_winCountTimestamp = new KeyValuePair<string, int>(timestamp, winCount);
		string text = string.Format("{{ \"{0}\": {1} }}", new object[2] { timestamp, winCount });
		Storager.setString("Win Count Timestamp", text);
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("Setting win count timestamp:    " + text);
		}
	}

	public bool TryIncrementWinCountTimestamp()
	{
		if (!_winCountTimestamp.HasValue)
		{
			return false;
		}
		_winCountTimestamp = new KeyValuePair<string, int>(_winCountTimestamp.Value.Key, _winCountTimestamp.Value.Value + 1);
		return true;
	}

	private IEnumerator RequestWinCountTimestampCoroutine()
	{
		yield break;
	}

	private void GetOurLAstOnline()
	{
		StartCoroutine(GetInfoByEverydayDelta());
		ReceivedLastOnline = true;
		StartCoroutine(UpdatePlayerOnlineLoop());
	}

	public void DownloadInfoByEverydayDelta()
	{
		StartCoroutine(GetInfoByEverydayDelta());
	}

	private IEnumerator GetInfoByEverydayDelta()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_player_online");
		wWWForm.AddField("id", id);
		wWWForm.AddField("app_version", "*:*.*.*");
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_player_online"));
		WWW getPlayerOnlineRequest = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm, "*:*.*.*");
		if (getPlayerOnlineRequest == null)
		{
			yield return StartCoroutine(MyWaitForSeconds(120f));
			yield break;
		}
		yield return getPlayerOnlineRequest;
		string response = URLs.Sanitize(getPlayerOnlineRequest);
		if (!string.IsNullOrEmpty(getPlayerOnlineRequest.error))
		{
			UnityEngine.Debug.LogWarning("GetInfoByEverydayDelta()    Error: " + getPlayerOnlineRequest.error);
			yield return StartCoroutine(MyWaitForSeconds(120f));
			yield break;
		}
		if ("fail".Equals(response))
		{
			UnityEngine.Debug.LogWarning("GetInfoByEverydayDelta()    Fail returned.");
			yield return StartCoroutine(MyWaitForSeconds(120f));
			yield break;
		}
		JSONNode data = JSON.Parse(response);
		if (data == null)
		{
			UnityEngine.Debug.LogWarning("GetInfoByEverydayDelta()    Cannot deserialize response: " + response);
			yield return StartCoroutine(MyWaitForSeconds(120f));
			yield break;
		}
		string value = data["delta"].Value;
		float result;
		if (float.TryParse(value, out result))
		{
			if (result > 82800f)
			{
				NotificationController.isGetEveryDayMoney = true;
				if (Storager.getInt(Defs.NeedTakeMarathonBonus) == 0)
				{
					Storager.setInt(Defs.NeedTakeMarathonBonus, 1);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("GetInfoByEverydayDelta()    Cannot parse delta: " + value);
			yield return StartCoroutine(MyWaitForSeconds(120f));
		}
	}

	private string GetAccesoriesString()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN);
		string value;
		if (@string == "cape_Custom")
		{
			value = Tools.DeserializeJson<CapeMemento>(PlayerPrefs.GetString("NewUserCape")).Cape;
			if (string.IsNullOrEmpty(value))
			{
				value = SkinsController.StringFromTexture(Resources.Load<Texture2D>("cape_CustomTexture"));
			}
		}
		else
		{
			value = string.Empty;
		}
		string string2 = Storager.getString(Defs.HatEquppedSN);
		string string3 = Storager.getString(Defs.BootsEquppedSN);
		string string4 = Storager.getString("MaskEquippedSN");
		string string5 = Storager.getString(Defs.ArmorNewEquppedSN);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("type", "0");
		dictionary.Add("name", @string);
		dictionary.Add("skin", value);
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		dictionary2.Add("type", "1");
		dictionary2.Add("name", string2);
		dictionary2.Add("skin", string.Empty);
		Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
		dictionary3.Add("type", "2");
		dictionary3.Add("name", string3);
		dictionary3.Add("skin", string.Empty);
		Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
		dictionary4.Add("type", "3");
		dictionary4.Add("name", string5);
		dictionary4.Add("skin", string.Empty);
		Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
		dictionary5.Add("type", "4");
		dictionary5.Add("name", string4);
		dictionary5.Add("skin", string.Empty);
		return Json.Serialize(new List<Dictionary<string, string>> { dictionary, dictionary2, dictionary3, dictionary4, dictionary5 });
	}

	public void SendAccessories()
	{
		if (readyToOperate)
		{
			StartCoroutine(UpdateAccessories());
		}
	}

	private IEnumerator UpdateAccessories()
	{
		if (string.IsNullOrEmpty(id))
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "update_accessories");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("auth", Hash("update_accessories"));
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("accessories", GetAccesoriesString());
		WWW updateAccessoriesRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return updateAccessoriesRequest;
		string text = URLs.Sanitize(updateAccessoriesRequest);
		if (string.IsNullOrEmpty(updateAccessoriesRequest.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("UpdateAccessories: " + text);
		}
		if (!string.IsNullOrEmpty(updateAccessoriesRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("UpdateAccessories error: " + updateAccessoriesRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(text) && text.Equals("fail") && (UnityEngine.Debug.isDebugBuild || Application.isEditor))
		{
			UnityEngine.Debug.LogWarning("UpdateAccessories fail.");
		}
	}

	private IEnumerator UpdatePlayer(bool sendSkin)
	{
		while (!ReceivedLastOnline || !infoLoaded || !getCohortInfo)
		{
			yield return null;
		}
		timeSendUpdatePlayer = Time.realtimeSinceStartup;
		InitOurInfo();
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "update_player");
		wWWForm.AddField("id", id);
		string nick2 = nick;
		string text = FilterBadWorld.FilterString(nick);
		if (text.Length > 20)
		{
			text = text.Substring(0, 20);
		}
		wWWForm.AddField("nick", text);
		wWWForm.AddField("skin", sendSkin ? skin : string.Empty);
		wWWForm.AddField("rank", rank);
		wWWForm.AddField("wins", wins.Value);
		string @string = Storager.getString(Defs.countryKey);
		if (!string.IsNullOrEmpty(@string))
		{
			wWWForm.AddField("country", @string);
		}
		string string2 = Storager.getString(Defs.continentKey);
		if (!string.IsNullOrEmpty(string2))
		{
			wWWForm.AddField("continent", string2);
		}
		if (Defs.IsDeveloperBuild)
		{
			wWWForm.AddField("developer", 1);
		}
		wWWForm.AddField("cohortName", Defs.abTestBalansCohortName);
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string value = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
			wWWForm.AddField("cohort_ad", value);
		}
		foreach (ABTestBase currentABTest in ABTestController.currentABTests)
		{
			if (currentABTest.cohort == ABTestController.ABTestCohortsType.A || currentABTest.cohort == ABTestController.ABTestCohortsType.B)
			{
				wWWForm.AddField(currentABTest.currentFolder, currentABTest.cohortName);
			}
		}
		string _lobby = Json.Serialize(Singleton<LobbyItemsController>.Instance.GetPlayerEquipedItemsIds());
		if (oldLobby != _lobby)
		{
			wWWForm.AddField("lobby", _lobby);
		}
		int @int = PlayerPrefs.GetInt("TotalWinsForLeaderboards", 0);
		wWWForm.AddField("total_wins", @int);
		wWWForm.AddField("id_fb", id_fb ?? string.Empty);
		wWWForm.AddField("device", SystemInfo.deviceUniqueIdentifier);
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		BankController.GiveInitialNumOfCoins();
		wWWForm.AddField("coins", Storager.getInt("Coins").ToString());
		wWWForm.AddField("gems", Storager.getInt("GemsCurrency").ToString());
		wWWForm.AddField("paying", Storager.getInt("PayingUser").ToString());
		wWWForm.AddField("kill_cnt", ProfileController.countGameTotalKills.Value);
		wWWForm.AddField("death_cnt", ProfileController.countGameTotalDeaths.Value);
		float result = 0f;
		float result2 = 0f;
		if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime"), out result))
		{
			wWWForm.AddField("playTime", Mathf.RoundToInt(result));
		}
		if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch"), out result2))
		{
			wWWForm.AddField("playTimeInMatch", Mathf.RoundToInt(result2));
		}
		List<object> list = Json.Deserialize(Storager.getString("LastKillRates")) as List<object>;
		if (list != null && list.Count == 2)
		{
			int[] array = (list[0] as List<object>).Select((object o) => Convert.ToInt32(o)).ToArray();
			int[] array2 = (list[1] as List<object>).Select((object o) => Convert.ToInt32(o)).ToArray();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num += array[i];
			}
			for (int j = 0; j < array2.Length; j++)
			{
				num2 += array2[j];
			}
			wWWForm.AddField("kill_cnt_month", num);
			wWWForm.AddField("death_cnt_month", num2);
		}
		if (ProfileController.countGameTotalHit.Value != 0)
		{
			int i2 = Mathf.RoundToInt(100f * (float)ProfileController.countGameTotalHit.Value / (float)ProfileController.countGameTotalShoot.Value);
			wWWForm.AddField("accuracy", i2);
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			string advertisingId = AndroidSystem.Instance.GetAdvertisingId();
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Android advertising id: " + advertisingId);
			}
			wWWForm.AddField("ad_id", advertisingId);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string empty = string.Empty;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("iOS advertising id: " + empty);
			}
			wWWForm.AddField("ad_id", empty);
		}
		wWWForm.AddField("accessories", GetAccesoriesString());
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("game", "0");
		dictionary.Add("max_score", survivalScore.ToString());
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		dictionary2.Add("game", "1");
		dictionary2.Add("max_score", coopScore.ToString());
		Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
		dictionary3.Add("game", "2");
		dictionary3.Add("max_score", Storager.getInt("DaterDayLived").ToString());
		string value2 = Json.Serialize(new List<Dictionary<string, string>> { dictionary, dictionary2, dictionary3 });
		wWWForm.AddField("scores", value2);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("update_player"));
		wWWForm.AddField("coins_bought", Storager.getInt(Defs.AllCurrencyBought + "Coins").ToString());
		wWWForm.AddField("gems_bought", Storager.getInt(Defs.AllCurrencyBought + "GemsCurrency").ToString());
		bool killRateStatisticsWasSent = false;
		try
		{
			bool flag = true;
			string string3 = PlayerPrefs.GetString(Defs.LastSendKillRateTimeKey, string.Empty);
			DateTime result3;
			if (!string.IsNullOrEmpty(string3) && DateTime.TryParse(string3, out result3))
			{
				TimeSpan timeSpan = TimeSpan.FromHours(20.0);
				flag = DateTime.UtcNow - result3 >= timeSpan;
			}
			if (flag)
			{
				Dictionary<string, Dictionary<int, int>>.KeyCollection keys = KillRateStatisticsManager.WeWereKilledOld.Keys;
				Dictionary<string, Dictionary<int, int>> dictionary4 = new Dictionary<string, Dictionary<int, int>>();
				foreach (string item in keys)
				{
					Dictionary<int, int> dictionary5 = (KillRateStatisticsManager.WeKillOld.ContainsKey(item) ? KillRateStatisticsManager.WeKillOld[item] : new Dictionary<int, int>());
					Dictionary<int, int> dictionary6 = KillRateStatisticsManager.WeWereKilledOld[item];
					if (dictionary5 == null)
					{
						UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: weKill == null  " + item);
						continue;
					}
					if (dictionary6 == null)
					{
						UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: weWereKilled == null  " + item);
						continue;
					}
					Dictionary<int, int>.KeyCollection keys2 = dictionary6.Keys;
					Dictionary<int, int> dictionary7 = new Dictionary<int, int>();
					foreach (int item2 in keys2)
					{
						int num3 = dictionary6[item2];
						if (num3 == 0)
						{
							UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: weWereKilledTier == 0 " + item);
							continue;
						}
						int value3 = (dictionary5.ContainsKey(item2) ? dictionary5[item2] : 0) * 1000 / num3;
						dictionary7.Add(item2, value3);
					}
					WeaponSounds weaponInfoByPrefabName = ItemDb.GetWeaponInfoByPrefabName(item);
					if (weaponInfoByPrefabName == null)
					{
						UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: weaponInfo == null  " + item);
						continue;
					}
					string text2 = weaponInfoByPrefabName.shopNameNonLocalized;
					if (text2 == null)
					{
						UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: readableWeaponName == null  " + item);
						continue;
					}
					string text3 = null;
					try
					{
						text3 = ItemDb.GetByPrefabName(weaponInfoByPrefabName.name.Replace("(Clone)", "")).Tag;
					}
					catch (Exception ex)
					{
						if (Application.isEditor)
						{
							UnityEngine.Debug.LogWarning("Exception  weaponInfoTag = ItemDb.GetByPrefabName(weaponInfo.name.Replace(\"(Clone)\",\"\")).Tag:  " + ex);
						}
					}
					if (text3 == null)
					{
						UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: weaponInfo.tag == null  " + item);
						continue;
					}
					if (item == WeaponManager.SocialGunWN || WeaponManager.GotchaGuns.Contains(text3))
					{
						text2 = text2 + "__DPS_TIER_" + (Storager.getInt("RememberedTierWhenObtainGun_" + item) + 1);
					}
					dictionary4.Add(text2, dictionary7);
				}
				if (dictionary4.Count > 0)
				{
					string text4 = Json.Serialize(new Dictionary<string, object>
					{
						{
							"version",
							GlobalGameController.AppVersion
						},
						{ "kill_rate", dictionary4 }
					});
					wWWForm.AddField("kill_rate", text4);
					killRateStatisticsWasSent = true;
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log(string.Format("<color=white>kill_rate: {0}</color>", new object[1] { text4 }));
					}
				}
			}
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogError("Exception adding kill_rate to update_player: " + ex2);
		}
		WWW updatePlayerRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return updatePlayerRequest;
		string text5 = URLs.Sanitize(updatePlayerRequest);
		if (string.IsNullOrEmpty(updatePlayerRequest.error) && !string.IsNullOrEmpty(text5) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Update: " + text5);
		}
		if (!string.IsNullOrEmpty(updatePlayerRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("Update error: " + updatePlayerRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(text5) && text5.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("Update fail.");
			}
			oldLobby = string.Empty;
		}
		else if (killRateStatisticsWasSent)
		{
			oldLobby = _lobby;
			PlayerPrefs.SetString(Defs.LastSendKillRateTimeKey, DateTime.UtcNow.ToString("s"));
		}
	}

	private IEnumerator GetClanDataLoop()
	{
		while (true)
		{
			if (idle || !SceneLoader.ActiveSceneName.Equals("Clans") || string.IsNullOrEmpty(ClanID))
			{
				yield return null;
				continue;
			}
			StartCoroutine(GetClanDataOnce());
			yield return StartCoroutine(MyWaitForSeconds(20f));
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private void TrySendEventShowBoxProcessFriendsData()
	{
		if (OnShowBoxProcessFriendsData != null)
		{
			OnShowBoxProcessFriendsData();
		}
	}

	private void TrySendEventHideBoxProcessFriendsData()
	{
		if (OnHideBoxProcessFriendsData != null)
		{
			OnHideBoxProcessFriendsData();
		}
	}

	private IEnumerator GetClanDataOnce()
	{
		if (!readyToOperate)
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_clan_info");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_clan_info"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		NumberOfClanInfoRequests++;
		try
		{
			yield return download;
		}
		finally
		{
			NumberOfClanInfoRequests--;
		}
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetClanDataOnce error: " + download.error);
			}
			yield break;
		}
		if (text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetClanDataOnce fail.");
			}
			yield break;
		}
		int result;
		if (string.IsNullOrEmpty(text) || int.TryParse(text, out result))
		{
			ClearClanData();
			yield break;
		}
		_UpdateClanMembers(text);
		if (FriendsController.ClanUpdated != null)
		{
			FriendsController.ClanUpdated();
		}
	}

	public void ClearClanData()
	{
		ClanID = null;
		clanName = "";
		clanLogo = "";
		clanLeaderID = "";
		clanMembers.Clear();
		ClanSentInvites.Clear();
		clanSentInvitesLocal.Clear();
	}

	private void _UpdateClanMembers(string text)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _UpdateClanMembers dict = null");
			}
			return;
		}
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			switch (item.Key)
			{
			case "info":
			{
				Dictionary<string, object> dictionary2 = item.Value as Dictionary<string, object>;
				if (dictionary2 == null)
				{
					break;
				}
				object value;
				if (dictionary2.TryGetValue("name", out value))
				{
					_prevClanName = clanName;
					clanName = value as string;
					if (!_prevClanName.Equals(clanName) && onChangeClanName != null)
					{
						onChangeClanName(clanName);
					}
				}
				object value2;
				if (dictionary2.TryGetValue("logo", out value2))
				{
					clanLogo = value2 as string;
				}
				object value3;
				if (dictionary2.TryGetValue("creator_id", out value3))
				{
					clanLeaderID = value3 as string;
				}
				break;
			}
			case "players":
			{
				List<object> list2 = item.Value as List<object>;
				if (list2 != null)
				{
					clanMembers.Clear();
					foreach (Dictionary<string, object> item2 in list2)
					{
						Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
						foreach (KeyValuePair<string, object> item3 in item2)
						{
							if (item3.Value is string)
							{
								dictionary3.Add(item3.Key, item3.Value as string);
							}
						}
						clanMembers.Add(dictionary3);
					}
				}
				List<string> toRem__ = new List<string>();
				foreach (string item4 in clanDeletedLocal)
				{
					bool flag = false;
					foreach (Dictionary<string, string> clanMember in clanMembers)
					{
						if (clanMember.ContainsKey("id") && clanMember["id"].Equals(item4))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						toRem__.Add(item4);
					}
				}
				clanDeletedLocal.RemoveAll((string obj) => toRem__.Contains(obj));
				break;
			}
			case "invites":
			{
				ClanSentInvites.Clear();
				List<object> list = item.Value as List<object>;
				if (list == null)
				{
					break;
				}
				foreach (string item5 in list)
				{
					int result;
					if (int.TryParse(item5, out result) && !ClanSentInvites.Contains(result.ToString()))
					{
						ClanSentInvites.Add(result.ToString());
						clanSentInvitesLocal.Remove(result.ToString());
					}
				}
				break;
			}
			}
		}
		List<string> toRem = new List<string>();
		foreach (string item6 in clanCancelledInvitesLocal)
		{
			if (!ClanSentInvites.Contains(item6))
			{
				toRem.Add(item6);
			}
		}
		clanCancelledInvitesLocal.RemoveAll((string obj) => toRem.Contains(obj));
		if (FriendsController.ClanUpdated != null)
		{
			FriendsController.ClanUpdated();
		}
		ClanDataSettted = true;
		if (clanMembers.Count > 3 && clanLeaderID == id)
		{
			AnalyticsStuff.TrySendOnceToFacebook("create_clan_3", null, null);
		}
	}

	private void _UpdateFriends(string text, bool requestAllInfo)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		invitesFromUs.Clear();
		invitesToUs.Clear();
		friends.Clear();
		ClanInvites.Clear();
		friendsDeletedLocal.Clear();
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		object value;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _UpdateFriends dict = null");
			}
		}
		else if (dictionary.TryGetValue("friends", out value))
		{
			List<object> list = value as List<object>;
			if (list == null)
			{
				if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning(" _UpdateFriends __list = null");
				}
				return;
			}
			_ProcessFriendsList(list, requestAllInfo);
			object value2;
			if (dictionary.TryGetValue("clans_invites", out value2))
			{
				List<object> list2 = value2 as List<object>;
				if (list2 == null)
				{
					if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogWarning(" _UpdateFriends clanInv = null");
					}
				}
				else
				{
					_ProcessClanInvitesList(list2);
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning(" _UpdateFriends clanInvObj!");
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning(" _UpdateFriends friendsObj!");
		}
	}

	private void _ProcessClanInvitesList(List<object> clanInv)
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		foreach (Dictionary<string, object> item in clanInv)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item2 in item)
			{
				dictionary.Add(item2.Key, item2.Value as string);
			}
			list.Add(dictionary);
		}
		ClanInvites.Clear();
		ClanInvites = list;
	}

	private void _ProcessFriendsList(List<object> __list, bool requestAllInfo)
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		foreach (Dictionary<string, object> item in __list)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item2 in item)
			{
				dictionary.Add(item2.Key, item2.Value as string);
			}
			list.Add(dictionary);
		}
		foreach (Dictionary<string, string> item3 in list)
		{
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			if (item3["whom"].Equals(id) && item3["status"].Equals("0"))
			{
				foreach (string key in item3.Keys)
				{
					if (!key.Equals("whom") && !key.Equals("status"))
					{
						try
						{
							dictionary2.Add(key.Equals("who") ? "friend" : key, item3[key]);
						}
						catch
						{
						}
					}
				}
				invitesToUs.Add(dictionary2["friend"]);
				notShowAddIds.Remove(item3["who"]);
			}
			if (!item3["status"].Equals("1"))
			{
				continue;
			}
			string text = (item3["who"].Equals(id) ? "who" : "whom");
			string text2 = (text.Equals("who") ? "whom" : "who");
			foreach (string key2 in item3.Keys)
			{
				if (!key2.Equals(text) && !key2.Equals("status"))
				{
					dictionary2.Add(key2.Equals(text2) ? "friend" : key2, item3[key2]);
				}
			}
			friends.Add(dictionary2["friend"]);
			notShowAddIds.Remove(item3[text2]);
		}
		if (requestAllInfo)
		{
			UpdatePLayersInfo();
		}
		else
		{
			_UpdatePlayersInfo();
		}
	}

	private void _UpdatePlayersInfo()
	{
		List<string> list = new List<string>();
		list.AddRange(friends);
		list.AddRange(invitesToUs);
		if (list.Count > 0)
		{
			StartCoroutine(GetInfoAboutNPlayers(list));
		}
	}

	private IEnumerator GetInfoAboutNPlayers()
	{
		List<string> list = new List<string>();
		list.AddRange(friends);
		list.AddRange(invitesToUs);
		if (list.Count != 0)
		{
			yield return StartCoroutine(GetInfoAboutNPlayers(list));
		}
	}

	public void GetInfoAboutPlayers(List<string> ids)
	{
		StartCoroutine(GetInfoAboutNPlayers(ids));
	}

	public IEnumerator GetInfoAboutNPlayers(List<string> ids)
	{
		string text = Json.Serialize(ids);
		if (text == null)
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_all_short_info_by_id");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("ids", text);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_all_short_info_by_id"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text2 = URLs.Sanitize(download);
		TrySendEventHideBoxProcessFriendsData();
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoAboutNPlayers error: " + download.error);
			}
			yield break;
		}
		if (text2.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoAboutNPlayers fail.");
			}
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text2) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" GetInfoAboutNPlayers info = null");
			}
		}
		else
		{
			UpdateInfoAboutNPlayers(dictionary);
		}
	}

	private void UpdateInfoAboutNPlayers(Dictionary<string, object> __list)
	{
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		foreach (string key in __list.Keys)
		{
			Dictionary<string, object> obj = __list[key] as Dictionary<string, object>;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> item in obj)
			{
				dictionary2.Add(item.Key, item.Value);
			}
			dictionary.Add(key, dictionary2);
		}
		foreach (string key2 in dictionary.Keys)
		{
			Dictionary<string, object> value = dictionary[key2];
			bool flag = false;
			if (friends.Contains(key2))
			{
				flag = true;
				if (friendsInfo.ContainsKey(key2))
				{
					friendsInfo[key2] = value;
				}
				else
				{
					friendsInfo.Add(key2, value);
				}
			}
			if (!flag)
			{
				if (profileInfo.ContainsKey(key2))
				{
					profileInfo[key2] = value;
				}
				else
				{
					profileInfo.Add(key2, value);
				}
				if (!sharedController.id.Equals(key2) && FindFriendsFromLocalLAN.lanPlayerInfo.Contains(key2) && !getPossibleFriendsResult.ContainsKey(key2))
				{
					getPossibleFriendsResult.Add(key2, PossiblleOrigin.Local);
				}
			}
			if (playersInfo.ContainsKey(key2))
			{
				playersInfo[key2] = value;
			}
			else
			{
				playersInfo.Add(key2, value);
			}
		}
		isUpdateInfoAboutAllFriends = false;
		if (FriendsController.FriendsUpdated != null)
		{
			FriendsController.FriendsUpdated();
		}
		SaveCurrentState();
	}

	public void UpdatePLayersInfo()
	{
		if (readyToOperate)
		{
			StartCoroutine(GetInfoAboutNPlayers());
		}
	}

	public void StartRefreshingInfo(string playerId)
	{
		if (readyToOperate)
		{
			_shouldStopRefreshingInfo = false;
			StartCoroutine(GetIfnoAboutPlayerLoop(playerId));
		}
	}

	public void StopRefreshingInfo()
	{
		if (readyToOperate)
		{
			_shouldStopRefreshingInfo = true;
		}
	}

	private IEnumerator GetIfnoAboutPlayerLoop(string playerId)
	{
		while (true)
		{
			if (idle)
			{
				yield return null;
				continue;
			}
			StartCoroutine(UpdatePlayerInfoById(playerId));
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !_shouldStopRefreshingInfo);
			if (_shouldStopRefreshingInfo)
			{
				break;
			}
		}
	}

	public IEnumerator GetInfoByIdCoroutine(string playerId)
	{
		getInfoPlayerResult = null;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_info_by_id");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", playerId);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_info_by_id"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		NumberOffFullInfoRequests++;
		try
		{
			yield return download;
		}
		finally
		{
			NumberOffFullInfoRequests--;
		}
		string text = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Info for id " + playerId + ": " + text);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoById error: " + download.error);
			}
			yield break;
		}
		if (text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoById fail.");
			}
			yield break;
		}
		Dictionary<string, object> dictionary = ParseInfo(text);
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetInfoById newInfo = null");
			}
		}
		else
		{
			getInfoPlayerResult = dictionary;
		}
	}

	public IEnumerator GetInfoByParamCoroutine(string param)
	{
		findPlayersByParamResult = null;
		if (string.IsNullOrEmpty(param))
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_users_info_by_param");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("param", param);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("get_users_info_by_param"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		TrySendEventShowBoxProcessFriendsData();
		yield return download;
		TrySendEventHideBoxProcessFriendsData();
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoById error: " + download.error);
			}
			yield break;
		}
		if (text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetInfoById fail.");
			}
			yield break;
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text))
		{
			bool isDebugBuild = UnityEngine.Debug.isDebugBuild;
		}
		List<object> list = Json.Deserialize(text) as List<object>;
		if (list == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetInfoByParam newInfo = null");
			}
		}
		else
		{
			if (list == null || list.Count <= 0)
			{
				yield break;
			}
			findPlayersByParamResult = new List<string>();
			foreach (object item in list)
			{
				Dictionary<string, object> dictionary = item as Dictionary<string, object>;
				string text2 = Convert.ToString(dictionary["id"]);
				findPlayersByParamResult.Add(text2);
				if (profileInfo.ContainsKey(text2))
				{
					profileInfo[text2]["player"] = dictionary;
					continue;
				}
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("player", dictionary);
				profileInfo.Add(text2, dictionary2);
			}
		}
	}

	private IEnumerator UpdatePlayerInfoById(string playerId)
	{
		yield return StartCoroutine(GetInfoByIdCoroutine(playerId));
		if (getInfoPlayerResult == null)
		{
			yield break;
		}
		playersInfo[playerId] = getInfoPlayerResult;
		bool flag = false;
		if (friends.Contains(playerId))
		{
			flag = true;
			if (friendsInfo.ContainsKey(playerId))
			{
				friendsInfo[playerId] = getInfoPlayerResult;
			}
			else
			{
				friendsInfo.Add(playerId, getInfoPlayerResult);
			}
		}
		if (clanFriendsInfo.ContainsKey(playerId))
		{
			clanFriendsInfo[playerId] = getInfoPlayerResult;
			flag = true;
		}
		if (!flag)
		{
			if (profileInfo.ContainsKey(playerId))
			{
				profileInfo[playerId] = getInfoPlayerResult;
			}
			else
			{
				profileInfo.Add(playerId, getInfoPlayerResult);
			}
		}
		if (playersInfo.ContainsKey(playerId))
		{
			playersInfo[playerId] = getInfoPlayerResult;
		}
		else
		{
			playersInfo.Add(playerId, getInfoPlayerResult);
		}
		if (FriendsController.FullInfoUpdated != null)
		{
			FriendsController.FullInfoUpdated();
		}
	}

	private Dictionary<string, object> ParseInfo(string info)
	{
		return Json.Deserialize(info) as Dictionary<string, object>;
	}

	public IEnumerator FriendRequest(string personId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackAnswer = null)
	{
		if (socialEventParameters == null)
		{
			throw new ArgumentNullException("socialEventParameters");
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "friend_request");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", id);
		wWWForm.AddField("whom", personId);
		wWWForm.AddField("type", 0);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("friend_request"));
		WWW friendRequest = Tools.CreateWww(actionAddress, wWWForm);
		TrySendEventShowBoxProcessFriendsData();
		yield return friendRequest;
		string text = URLs.Sanitize(friendRequest);
		TrySendEventHideBoxProcessFriendsData();
		if (string.IsNullOrEmpty(friendRequest.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Friend request: " + text);
		}
		bool flag = false;
		if (!string.IsNullOrEmpty(friendRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("FriendRequest error: " + friendRequest.error);
			}
			if (callbackAnswer != null)
			{
				callbackAnswer(false, false);
				flag = true;
			}
		}
		else if (text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("FriendRequest fail.");
			}
			if (callbackAnswer != null)
			{
				callbackAnswer(false, false);
				flag = true;
			}
		}
		if (text.Equals("ok"))
		{
			TutorialQuestManager.Instance.AddFulfilledQuest("addFriend");
			QuestMediator.NotifySocialInteraction("addFriend");
			if (invitesToUs.Contains(personId))
			{
				invitesToUs.Remove(personId);
				friends.Add(personId);
			}
			else
			{
				invitesFromUs.Add(personId);
			}
			AnalyticsFacade.SendCustomEvent("Social", socialEventParameters);
		}
		if (callbackAnswer != null && !flag)
		{
			callbackAnswer(true, text.Equals("exist"));
		}
	}

	private IEnumerator _SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "create_clan");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id", personId);
		string value = FilterBadWorld.FilterString(nameClan);
		wWWForm.AddField("name", value);
		wWWForm.AddField("logo", skinClan);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("create_clan"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			if (ErrorHandler != null)
			{
				ErrorHandler("Request skipped.");
			}
			yield break;
		}
		NumberOfCreateClanRequests++;
		float tm = Time.realtimeSinceStartup;
		try
		{
			while (!download.isDone && string.IsNullOrEmpty(download.error) && Time.realtimeSinceStartup - tm < 25f)
			{
				yield return null;
			}
		}
		finally
		{
			NumberOfCreateClanRequests--;
		}
		bool flag = !download.isDone && string.IsNullOrEmpty(download.error) && Time.realtimeSinceStartup - tm >= 25f;
		string text = (flag ? string.Empty : URLs.Sanitize(download));
		if (!flag && string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_SendCreateClan request: " + text);
		}
		int result;
		if (flag || !string.IsNullOrEmpty(download.error))
		{
			string text2 = (flag ? "TIMEOUT" : download.error);
			if (ErrorHandler != null)
			{
				ErrorHandler(text2);
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_SendCreateClan error: " + text2);
			}
			if (flag)
			{
				download.Dispose();
			}
		}
		else if ("fail".Equals(text))
		{
			if (ErrorHandler != null)
			{
				ErrorHandler("fail");
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_SendCreateClan fail.");
			}
		}
		else if (int.TryParse(text, out result))
		{
			if (result != -1)
			{
				ClanID = result.ToString();
			}
			if (this.ReturnNewIDClan != null)
			{
				this.ReturnNewIDClan(result);
			}
		}
	}

	public void ExitClan(string who = null)
	{
		if (readyToOperate)
		{
			StartCoroutine(_ExitClan(who));
		}
	}

	private IEnumerator _ExitClan(string who)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "exit_clan");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", who ?? id);
		wWWForm.AddField("id_clan", ClanID);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("exit_clan"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_ExitClan: " + text);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_ExitClan error: " + download.error);
			}
		}
		else if ("fail".Equals(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning("_ExitClan fail.");
		}
	}

	public void DeleteClan()
	{
		if (readyToOperate && ClanID != null)
		{
			StartCoroutine(_DeleteClan());
		}
	}

	private IEnumerator _DeleteClan()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "delete_clan");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_clan", ClanID);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("delete_clan"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_DeleteClan: " + text);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_DeleteClan error: " + download.error);
			}
		}
		else if ("fail".Equals(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning("_DeleteClan fail.");
		}
	}

	private IEnumerator SendClanInvitation(string personID, Action<bool, bool> callbackResult = null)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "invite_to_clan");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", personID);
		wWWForm.AddField("id_clan", ClanID);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("invite_to_clan"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			if (callbackResult != null)
			{
				callbackResult(false, false);
			}
			clanSentInvitesLocal.Remove(personID);
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("SendClanInvitation: " + text);
		}
		bool flag = false;
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("SendClanInvitation error: " + download.error);
			}
			if (callbackResult != null)
			{
				flag = true;
				callbackResult(false, false);
			}
			clanSentInvitesLocal.Remove(personID);
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("SendClanInvitation fail.");
			}
			if (callbackResult != null)
			{
				flag = true;
				callbackResult(false, false);
			}
			clanSentInvitesLocal.Remove(personID);
		}
		if (text.Equals("ok") && !ClanSentInvites.Contains(personID))
		{
			ClanSentInvites.Add(personID);
		}
		if (callbackResult != null && !flag)
		{
			callbackResult(true, text.Equals("exist"));
		}
	}

	private IEnumerator AcceptFriend(string accepteeId, Action<bool> action = null)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "accept_friend");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("player_id", id);
		wWWForm.AddField("acceptee_id", accepteeId);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("accept_friend"));
		WWW acceptFriendRequest = Tools.CreateWww(actionAddress, wWWForm);
		TrySendEventShowBoxProcessFriendsData();
		yield return acceptFriendRequest;
		string text = URLs.Sanitize(acceptFriendRequest);
		TrySendEventHideBoxProcessFriendsData();
		if (!string.IsNullOrEmpty(acceptFriendRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("AcceptFriend error: " + acceptFriendRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("AcceptFriend fail.");
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if (string.IsNullOrEmpty(acceptFriendRequest.error) && !string.IsNullOrEmpty(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Accept friend: " + text);
			}
			if (invitesToUs.Contains(accepteeId))
			{
				invitesToUs.Remove(accepteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (!friends.Contains(accepteeId))
			{
				friends.Add(accepteeId);
			}
			QuestMediator.NotifySocialInteraction("addFriend");
			if (action != null)
			{
				action(true);
			}
		}
	}

	public static void DeleteFriend(string rejecteeId, Action<bool> action = null)
	{
		if (!(sharedController == null) && readyToOperate)
		{
			sharedController.StartCoroutine(sharedController.DeleteFriendCoroutine(rejecteeId, action));
		}
	}

	private IEnumerator DeleteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "reject_friendship");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("rejectee_id", rejecteeId);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("reject_friendship"));
		WWW rejectFriendshipRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return rejectFriendshipRequest;
		string text = URLs.Sanitize(rejectFriendshipRequest);
		if (!string.IsNullOrEmpty(rejectFriendshipRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("Reject_friendship error: " + rejectFriendshipRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("Reject_friendship fail.");
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if (string.IsNullOrEmpty(rejectFriendshipRequest.error) && !string.IsNullOrEmpty(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("reject_friendship: " + text);
			}
			if (friends.Contains(rejecteeId))
			{
				friends.Remove(rejecteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (action != null)
			{
				action(true);
			}
		}
	}

	public void RejectInvite(string rejecteeId, Action<bool> action = null)
	{
		if (readyToOperate)
		{
			StartCoroutine(RejectInviteFriendCoroutine(rejecteeId, action));
		}
	}

	public void RejectClanInvite(string clanID, string playerID = null)
	{
		if (!string.IsNullOrEmpty(clanID) && readyToOperate)
		{
			StartCoroutine(_RejectClanInvite(clanID, playerID));
		}
	}

	private IEnumerator RejectInviteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "reject_invite_friendship");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("rejectee_id", rejecteeId);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("reject_invite_friendship"));
		WWW rejectInviteFriendshipRequest = Tools.CreateWww(actionAddress, wWWForm);
		TrySendEventShowBoxProcessFriendsData();
		yield return rejectInviteFriendshipRequest;
		TrySendEventHideBoxProcessFriendsData();
		string text = URLs.Sanitize(rejectInviteFriendshipRequest);
		if (!string.IsNullOrEmpty(rejectInviteFriendshipRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("RejectFriend error: " + rejectInviteFriendshipRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("RejectFriend fail.");
			}
			if (action != null)
			{
				action(false);
			}
		}
		else if (string.IsNullOrEmpty(rejectInviteFriendshipRequest.error) && !string.IsNullOrEmpty(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Reject friend: " + text);
			}
			if (invitesToUs.Contains(rejecteeId))
			{
				invitesToUs.Remove(rejecteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (action != null)
			{
				action(true);
			}
		}
	}

	private IEnumerator _RejectClanInvite(string clanID, string playerID)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "reject_invite");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", playerID ?? id);
		wWWForm.AddField("id_clan", clanID);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("reject_invite"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			clanCancelledInvitesLocal.Remove(playerID);
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_RejectClanInvite error: " + download.error);
			}
			clanCancelledInvitesLocal.Remove(playerID);
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_RejectClanInvite fail.");
			}
			clanCancelledInvitesLocal.Remove(playerID);
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_RejectClanInvite: " + text);
		}
	}

	public void DeleteClanMember(string memebrID)
	{
		if (!string.IsNullOrEmpty(memebrID) && readyToOperate)
		{
			StartCoroutine(_DeleteClanMember(memebrID));
		}
	}

	private IEnumerator _DeleteClanMember(string memberID)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "delete_clan_member");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", memberID);
		wWWForm.AddField("id_clan", ClanID);
		wWWForm.AddField("id", id);
		wWWForm.AddField("uniq_id", sharedController.id);
		wWWForm.AddField("auth", Hash("delete_clan_member"));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			clanDeletedLocal.Remove(memberID);
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_DeleteClanMember error: " + download.error);
			}
			clanDeletedLocal.Remove(memberID);
		}
		else if ("fail".Equals(text))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("_DeleteClanMember fail.");
			}
			clanDeletedLocal.Remove(memberID);
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("_DeleteClanMember: " + text);
		}
	}

	private void Update()
	{
		if (isUpdateServerTimeAfterRun)
		{
			tickForServerTime += Time.unscaledDeltaTime;
			if (tickForServerTime >= 1f)
			{
				localServerTime++;
				tickForServerTime -= 1f;
			}
		}
		if (!firstUpdateAfterApplicationPause)
		{
			deltaTimeInGame += Time.unscaledDeltaTime;
		}
		else
		{
			firstUpdateAfterApplicationPause = false;
		}
		if (Banned == 1 && PhotonNetwork.connected)
		{
			GameConnect.Disconnect();
		}
		if (Input.touchCount > 0)
		{
			if (Time.realtimeSinceStartup - lastTouchTm > 30f)
			{
				idle = true;
			}
		}
		else
		{
			lastTouchTm = Time.realtimeSinceStartup;
			idle = false;
		}
	}

	private string GetJsonIdsFacebookFriends()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Start GetJsonIdsFacebookFriends");
		}
		FacebookController facebookController = FacebookController.sharedController;
		if (facebookController == null)
		{
			return "[]";
		}
		if (facebookController.friendsList == null || facebookController.friendsList.Count == 0)
		{
			return "[]";
		}
		List<string> list = new List<string>();
		for (int i = 0; i < facebookController.friendsList.Count; i++)
		{
			Friend friend = facebookController.friendsList[i];
			list.Add(friend.id);
		}
		string text = Json.Serialize(list);
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("GetJsonIdsFacebookFriends: " + text);
		}
		return text;
	}

	private IEnumerator GetPossibleFriendsList(int playerLevel, int platformId, string clientVersion)
	{
		WWWForm wWWForm = new WWWForm();
		string text = "possible_friends_list";
		string value = string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		});
		wWWForm.AddField("action", text);
		wWWForm.AddField("app_version", value);
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("auth", Hash(text));
		if (FindFriendsFromLocalLAN.lanPlayerInfo.Count > 0)
		{
			wWWForm.AddField("local_ids", Json.Serialize(FindFriendsFromLocalLAN.lanPlayerInfo));
		}
		string jsonIdsFacebookFriends = GetJsonIdsFacebookFriends();
		wWWForm.AddField("ids", jsonIdsFacebookFriends);
		wWWForm.AddField("rank", playerLevel.ToString());
		wWWForm.AddField("platform_id", platformId.ToString());
		wWWForm.AddField("version", clientVersion);
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string text2 = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetPossibleFriendsList error: " + download.error);
			}
			yield break;
		}
		if (text2.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("GetPossibleFriendsList fail.");
			}
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text2) as Dictionary<string, object>;
		if (dictionary == null)
		{
			yield break;
		}
		getPossibleFriendsResult.Clear();
		if (dictionary.ContainsKey("local_users"))
		{
			List<object> list = dictionary["local_users"] as List<object>;
			if (list != null && list.Count > 0)
			{
				foreach (Dictionary<string, object> item in list.OfType<Dictionary<string, object>>())
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> item2 in item)
					{
						dictionary2.Add(item2.Key, item2.Value);
					}
					string text3 = Convert.ToString(dictionary2["id"]);
					if (profileInfo.ContainsKey(text3))
					{
						Dictionary<string, object> dictionary3 = profileInfo[text3]["player"] as Dictionary<string, object>;
						dictionary3["nick"] = dictionary2["nick"];
						dictionary3["rank"] = dictionary2["rank"];
						dictionary3["clan_logo"] = dictionary2["clan_logo"];
						dictionary3["clan_name"] = dictionary2["clan_name"];
						dictionary3["skin"] = dictionary2["skin"];
						profileInfo[text3]["player"] = dictionary3;
					}
					else
					{
						Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
						dictionary3.Add("nick", dictionary2["nick"]);
						dictionary3.Add("rank", dictionary2["rank"]);
						dictionary3.Add("clan_logo", dictionary2["clan_logo"]);
						dictionary3.Add("clan_name", dictionary2["clan_name"]);
						dictionary3.Add("skin", dictionary2["skin"]);
						Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
						dictionary4.Add("player", dictionary3);
						profileInfo.Add(text3, dictionary4);
					}
					if (!getPossibleFriendsResult.ContainsKey(text3) && !friends.Contains(text3))
					{
						getPossibleFriendsResult.Add(text3, PossiblleOrigin.Local);
					}
				}
			}
		}
		if (dictionary.ContainsKey("facebook_users"))
		{
			List<object> list2 = dictionary["facebook_users"] as List<object>;
			if (list2 != null && list2.Count > 0)
			{
				foreach (Dictionary<string, object> item3 in list2.OfType<Dictionary<string, object>>())
				{
					string text4 = Convert.ToString(item3["id"]);
					if (IsPlayerOurFriend(text4))
					{
						continue;
					}
					Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> item4 in item3)
					{
						dictionary5.Add(item4.Key, item4.Value);
					}
					if (profileInfo.ContainsKey(text4))
					{
						Dictionary<string, object> dictionary6 = profileInfo[text4]["player"] as Dictionary<string, object>;
						dictionary6["nick"] = dictionary5["nick"];
						dictionary6["rank"] = dictionary5["rank"];
						dictionary6["clan_logo"] = dictionary5["clan_logo"];
						dictionary6["clan_name"] = dictionary5["clan_name"];
						dictionary6["skin"] = dictionary5["skin"];
						profileInfo[text4]["player"] = dictionary6;
					}
					else
					{
						Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
						dictionary6.Add("nick", dictionary5["nick"]);
						dictionary6.Add("rank", dictionary5["rank"]);
						dictionary6.Add("clan_logo", dictionary5["clan_logo"]);
						dictionary6.Add("clan_name", dictionary5["clan_name"]);
						dictionary6.Add("skin", dictionary5["skin"]);
						Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
						dictionary7.Add("player", dictionary6);
						profileInfo.Add(text4, dictionary7);
					}
					if (!getPossibleFriendsResult.ContainsKey(text4) && !friends.Contains(text4))
					{
						getPossibleFriendsResult.Add(text4, PossiblleOrigin.Facebook);
					}
				}
			}
		}
		if (dictionary.ContainsKey("users"))
		{
			List<object> list3 = dictionary["users"] as List<object>;
			if (list3 != null && list3.Count > 0)
			{
				foreach (Dictionary<string, object> item5 in list3.OfType<Dictionary<string, object>>())
				{
					Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> item6 in item5)
					{
						dictionary8.Add(item6.Key, item6.Value);
					}
					string text5 = Convert.ToString(dictionary8["id"]);
					if (profileInfo.ContainsKey(text5))
					{
						Dictionary<string, object> dictionary9 = profileInfo[text5]["player"] as Dictionary<string, object>;
						dictionary9["nick"] = dictionary8["nick"];
						dictionary9["rank"] = dictionary8["rank"];
						dictionary9["clan_logo"] = dictionary8["clan_logo"];
						dictionary9["clan_name"] = dictionary8["clan_name"];
						dictionary9["skin"] = dictionary8["skin"];
						profileInfo[text5]["player"] = dictionary9;
					}
					else
					{
						Dictionary<string, object> dictionary9 = new Dictionary<string, object>();
						dictionary9.Add("nick", dictionary8["nick"]);
						dictionary9.Add("rank", dictionary8["rank"]);
						dictionary9.Add("clan_logo", dictionary8["clan_logo"]);
						dictionary9.Add("clan_name", dictionary8["clan_name"]);
						dictionary9.Add("skin", dictionary8["skin"]);
						Dictionary<string, object> dictionary10 = new Dictionary<string, object>();
						dictionary10.Add("player", dictionary9);
						profileInfo.Add(text5, dictionary10);
					}
					if (!getPossibleFriendsResult.ContainsKey(text5) && !friends.Contains(text5))
					{
						getPossibleFriendsResult.Add(text5, PossiblleOrigin.RandomPlayer);
					}
				}
			}
		}
		if (FriendsController.FriendsUpdated != null)
		{
			FriendsController.FriendsUpdated();
		}
	}

	public void DownloadDataAboutPossibleFriends()
	{
		int currentLevel = ExperienceController.GetCurrentLevel();
		int myPlatformConnect = (int)GameConnect.myPlatformConnect;
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		StartCoroutine(GetPossibleFriendsList(currentLevel, myPlatformConnect, multiplayerProtocolVersion));
	}

	private IEnumerator ClearAllFriendsInvitesCoroutine()
	{
		WWWForm wWWForm = new WWWForm();
		string text = "delete_friend_invites";
		string value = string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		});
		wWWForm.AddField("action", text);
		wWWForm.AddField("app_version", value);
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("auth", Hash(text));
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			yield break;
		}
		TrySendEventShowBoxProcessFriendsData();
		yield return download;
		TrySendEventHideBoxProcessFriendsData();
		if (FriendsWindowController.Instance != null)
		{
			FriendsWindowController.Instance.statusBar.clearAllInviteButton.isEnabled = true;
		}
		string text2 = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("ClearAllFriendsInvites error: " + download.error);
			}
		}
		else if (text2.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("ClearAllFriendsInvites fail.");
			}
		}
		else if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(text2))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("ClearAllFriendsInvites: " + text2);
			}
			if (invitesToUs != null)
			{
				invitesToUs.Clear();
			}
			if (FriendsController.FriendsUpdated != null)
			{
				FriendsController.FriendsUpdated();
			}
		}
	}

	public void GetFriendsData(bool _isUpdateInfoAboutAllFriends = false)
	{
		timerUpdateFriend = -1f;
		if (_isUpdateInfoAboutAllFriends)
		{
			isUpdateInfoAboutAllFriends = true;
		}
	}

	private IEnumerator SendGameTimeLoop()
	{
		while (true)
		{
			if (!readyToOperate || idle || string.IsNullOrEmpty(sharedController.id))
			{
				yield return null;
				continue;
			}
			while (timerSendTimeGame > 0f)
			{
				if (PhotonNetwork.room == null)
				{
					timerSendTimeGame -= Time.unscaledDeltaTime;
				}
				else
				{
					timerSendTimeGame = 30f;
				}
				yield return null;
			}
			yield return StartCoroutine(SendGameTime());
			timerSendTimeGame = 30f;
		}
	}

	private IEnumerator SendGameTime()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "update_game_time");
		wWWForm.AddField("auth", Hash("update_game_time"));
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("uniq_id", sharedController.id);
		sendingTime = Mathf.RoundToInt(deltaTimeInGame);
		wWWForm.AddField("game_time", Mathf.RoundToInt(sendingTime));
		if (MainMenuController.sharedController != null)
		{
			wWWForm.AddField("get_likes", 1);
		}
		wWWForm.AddField("tier", ExpController.OurTierForAnyPlace());
		wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		if (Defs.abTestBalansCohort != 0 && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
		{
			wWWForm.AddField("cohortName", Defs.abTestBalansCohortName);
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string value = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
			wWWForm.AddField("cohort_ad", value);
		}
		foreach (ABTestBase currentABTest in ABTestController.currentABTests)
		{
			if (currentABTest.cohort == ABTestController.ABTestCohortsType.A || currentABTest.cohort == ABTestController.ABTestCohortsType.B)
			{
				wWWForm.AddField(currentABTest.currentFolder, currentABTest.cohortName);
			}
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Send delta time: " + sendingTime);
		}
		WWW updateGameTimeRequest = Tools.CreateWww(actionAddress, wWWForm);
		yield return updateGameTimeRequest;
		string text = URLs.Sanitize(updateGameTimeRequest);
		if (!string.IsNullOrEmpty(updateGameTimeRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("updateGameTimeRequest error: " + updateGameTimeRequest.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(text) && text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("updateGameTimeRequest fail.");
			}
			yield break;
		}
		deltaTimeInGame -= sendingTime;
		if (!string.IsNullOrEmpty(text) && !text.Equals("ok"))
		{
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("fight_invites"))
			{
				ParseFightInvite(dictionary["fight_invites"] as List<object>);
			}
			if (dictionary != null && dictionary.ContainsKey("likes"))
			{
				OnUpdateLastLikesLobby(dictionary["likes"] as Dictionary<string, object>);
			}
		}
	}

	private void OnUpdateLastLikesLobby(Dictionary<string, object> _responseDict)
	{
		if (ourLobbyLikes == null)
		{
			UnityEngine.Debug.LogErrorFormat("OnUpdateLikeLobbyCount ourLobbyLikes == null");
			return;
		}
		if (_responseDict != null && _responseDict.ContainsKey("lobby_likes"))
		{
			ourLobbyLikes.Likes = Convert.ToInt32(_responseDict["lobby_likes"]);
		}
		if (_responseDict == null || !_responseDict.ContainsKey("lobby_last_like"))
		{
			return;
		}
		Dictionary<string, object> dictionary = _responseDict["lobby_last_like"] as Dictionary<string, object>;
		if (dictionary.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			if (!lastLikesPlayers.Contains(item.Key))
			{
				lastLikesPlayers.Add(item.Key);
			}
		}
		while (lastLikesPlayers.Count > 10)
		{
			lastLikesPlayers.RemoveAt(0);
		}
		lastLikesPlayers = lastLikesPlayers;
		UpdateInfoAboutNPlayers(dictionary);
		Dictionary<string, object> dictionary2 = dictionary[lastLikesPlayers[lastLikesPlayers.Count - 1]] as Dictionary<string, object>;
		if (dictionary2.ContainsKey("player"))
		{
			Dictionary<string, object> dictionary3 = dictionary2["player"] as Dictionary<string, object>;
			if (dictionary3.ContainsKey("nick"))
			{
				ourLobbyLikes.LastPlayerThatLiked = dictionary3["nick"].ToString();
			}
		}
		if (UpdatedLastLikeLobbyPlayers != null)
		{
			UpdatedLastLikeLobbyPlayers();
		}
	}

	private IEnumerator GetFriendsDataLoop()
	{
		while (true)
		{
			if (!readyToOperate || idle || string.IsNullOrEmpty(sharedController.id) || !TrainingController.TrainingCompleted)
			{
				yield return null;
				continue;
			}
			yield return StartCoroutine(UpdateFriendsInfo(isUpdateInfoAboutAllFriends));
			while (timerUpdateFriend > 0f)
			{
				if (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled)
				{
					timerUpdateFriend -= Time.unscaledDeltaTime;
				}
				yield return null;
			}
			timerUpdateFriend = Defs.timeUpdateFriendInfo;
		}
	}

	private IEnumerator UpdateFriendsInfo(bool _isUpdateInfoAboutAllFriends)
	{
		WWWForm wWWForm = new WWWForm();
		string text = "update_friends_info";
		string value = string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		});
		wWWForm.AddField("action", text);
		wWWForm.AddField("app_version", value);
		wWWForm.AddField("uniq_id", id);
		wWWForm.AddField("auth", Hash(text));
		bool flag = FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled;
		wWWForm.AddField("from_friends", flag ? 1 : 0);
		if (FriendsWindowController.IsActiveFriendListTab() && friends.Count > 0)
		{
			string text2 = Json.Serialize(friends);
			if (text2 != null)
			{
				wWWForm.AddField("get_all_players_online", text2);
			}
		}
		wWWForm.AddField("private_messages", ChatController.GetPrivateChatJsonForSend());
		WWW updateFriendsInfoRequest = Tools.CreateWww(actionAddress, wWWForm, "from_friends: " + flag);
		yield return updateFriendsInfoRequest;
		string text3 = URLs.Sanitize(updateFriendsInfoRequest);
		invitesToUs.Clear();
		if (!string.IsNullOrEmpty(updateFriendsInfoRequest.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("update_frieds_info error: " + updateFriendsInfoRequest.error);
			}
			TrySendEventHideBoxProcessFriendsData();
		}
		else if (text3.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("update_frieds_info fail.");
			}
			TrySendEventHideBoxProcessFriendsData();
		}
		else if (string.IsNullOrEmpty(updateFriendsInfoRequest.error) && !string.IsNullOrEmpty(text3))
		{
			ParseUpdateFriendsInfoResponse(text3, _isUpdateInfoAboutAllFriends);
			notShowAddIds.Clear();
			if (UpdateFriendsInfoAction != null)
			{
				UpdateFriendsInfoAction();
			}
		}
	}

	public void SendInviteFightToPlayer(string _idFriend)
	{
		StartCoroutine(SendInviteFightToPlayerCoroutine(_idFriend));
	}

	private IEnumerator SendInviteFightToPlayerCoroutine(string _idFriend)
	{
		string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
		playerNameOrDefault = FilterBadWorld.FilterString(playerNameOrDefault);
		WWWForm wWWForm = new WWWForm();
		string value = string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		});
		wWWForm.AddField("action", "send_fight_invite");
		wWWForm.AddField("app_version", value);
		wWWForm.AddField("uniq_id", id ?? string.Empty);
		wWWForm.AddField("name", playerNameOrDefault ?? string.Empty);
		wWWForm.AddField("reciever_id", _idFriend ?? string.Empty);
		wWWForm.AddField("auth", Hash("send_fight_invite"));
		if (Application.isEditor)
		{
			UnityEngine.Debug.LogFormat("`HandleCallFriend to Action Server()`: `{0}`", Encoding.UTF8.GetString(wWWForm.data, 0, wWWForm.data.Length));
		}
		WWW request = Tools.CreateWww(actionAddress, wWWForm);
		yield return request;
		string text = URLs.Sanitize(request);
		if (!string.IsNullOrEmpty(request.error))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("send_fight_invite error: " + request.error);
			}
		}
		else if (text.Equals("fail"))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning("send_fight_invite fail.");
			}
		}
		else if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("send_fight_invite: " + text);
		}
	}

	public void ParseFightInvite(List<object> _invites)
	{
		if (!Defs.isEnableLocalInviteFromFriends)
		{
			return;
		}
		for (int i = 0; i < _invites.Count; i++)
		{
			Dictionary<string, object> dictionary = _invites[i] as Dictionary<string, object>;
			if (dictionary.ContainsKey("id") && dictionary.ContainsKey("name"))
			{
				string friendId = dictionary["id"].ToString();
				string nickname = dictionary["name"].ToString();
				BattleInviteListener.Instance.NotifyBattleIncomingInvite(friendId, nickname);
			}
		}
	}

	private void ParseUpdateFriendsInfoResponse(string response, bool _isUpdateInfoAboutAllFriends)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		List<string> list = new List<string>();
		HashSet<string> hashSet = new HashSet<string>(friends);
		HashSet<string> hashSet2 = new HashSet<string>(invitesToUs);
		if (dictionary.ContainsKey("friends"))
		{
			friends.Clear();
			List<object> list2 = dictionary["friends"] as List<object>;
			for (int i = 0; i < list2.Count; i++)
			{
				string text = list2[i] as string;
				if (getPossibleFriendsResult.ContainsKey(text))
				{
					getPossibleFriendsResult.Remove(text);
				}
				friends.Add(text);
				if ((!_isUpdateInfoAboutAllFriends && !friendsInfo.ContainsKey(text)) || _isUpdateInfoAboutAllFriends)
				{
					list.Add(text);
				}
			}
		}
		if (dictionary.ContainsKey("invites"))
		{
			invitesToUs.Clear();
			List<object> list3 = dictionary["invites"] as List<object>;
			for (int j = 0; j < list3.Count; j++)
			{
				string text2 = list3[j] as string;
				if (!friends.Contains(text2))
				{
					invitesToUs.Add(text2);
				}
				if ((!_isUpdateInfoAboutAllFriends && !friendsInfo.ContainsKey(text2) && !clanFriendsInfo.ContainsKey(text2) && !profileInfo.ContainsKey(text2)) || _isUpdateInfoAboutAllFriends)
				{
					list.Add(text2);
				}
			}
		}
		if (dictionary.ContainsKey("invites_outcoming"))
		{
			invitesFromUs.Clear();
			List<object> list4 = dictionary["invites_outcoming"] as List<object>;
			for (int k = 0; k < list4.Count; k++)
			{
				string item = list4[k] as string;
				if (!friends.Contains(item))
				{
					invitesFromUs.Add(item);
				}
			}
		}
		if (_isUpdateInfoAboutAllFriends)
		{
			List<string> list5 = new List<string>(friends);
			List<string> list6 = new List<string>();
			list5.AddRange(invitesToUs);
			foreach (KeyValuePair<string, Dictionary<string, object>> item2 in friendsInfo)
			{
				if (!list5.Contains(item2.Key))
				{
					list6.Add(item2.Key);
				}
			}
			if (list6.Count > 0)
			{
				for (int l = 0; l < list6.Count; l++)
				{
					friendsInfo.Remove(list6[l]);
				}
				SaveCurrentState();
			}
		}
		if (dictionary.ContainsKey("onLines"))
		{
			string response2 = Json.Serialize(dictionary["onLines"]);
			ParseOnlinesResponse(response2);
		}
		if (list.Count > 0)
		{
			StartCoroutine(GetInfoAboutNPlayers(list));
		}
		else
		{
			if ((!hashSet.SetEquals(friends) || !hashSet2.SetEquals(invitesToUs)) && FriendsController.FriendsUpdated != null)
			{
				FriendsController.FriendsUpdated();
			}
			TrySendEventHideBoxProcessFriendsData();
		}
		if (dictionary.ContainsKey("chat"))
		{
			string response3 = Json.Serialize(dictionary["chat"]);
			if (ChatController.sharedController != null)
			{
				ChatController.sharedController.ParseUpdateChatMessageResponse(response3);
			}
		}
		if (dictionary.ContainsKey("fight_invites"))
		{
			ParseFightInvite(dictionary["fight_invites"] as List<object>);
		}
	}

	private void ParseOnlinesResponse(string response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetAllPlayersOnline info = null");
			}
			return;
		}
		Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in dictionary.Keys)
		{
			Dictionary<string, object> obj = dictionary[key] as Dictionary<string, object>;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item in obj)
			{
				dictionary3.Add(item.Key, item.Value as string);
			}
			dictionary2.Add(key, dictionary3);
		}
		onlineInfo.Clear();
		foreach (string key2 in dictionary2.Keys)
		{
			Dictionary<string, string> dictionary4 = dictionary2[key2];
			int num = int.Parse(dictionary4["game_mode"]);
			int num2 = num - num / 100 * 100;
			if (num2 == 2 || num2 == 0 || num2 == 5 || num2 == 4 || num2 == 6 || num2 == 7)
			{
				if (!onlineInfo.ContainsKey(key2))
				{
					onlineInfo.Add(key2, dictionary4);
				}
				else
				{
					onlineInfo[key2] = dictionary4;
				}
			}
		}
	}

	public void ClearAllFriendsInvites()
	{
		StartCoroutine(ClearAllFriendsInvitesCoroutine());
	}

	public void UpdateRecordByFriendsJoinClick(string friendId)
	{
		if (clicksJoinByFriends.ContainsKey(friendId))
		{
			clicksJoinByFriends[friendId] = DateTime.UtcNow.ToString("s");
		}
		else
		{
			clicksJoinByFriends.Add(friendId, DateTime.UtcNow.ToString("s"));
		}
	}

	public DateTime GetDateLastClickJoinFriend(string friendId)
	{
		if (!clicksJoinByFriends.ContainsKey(friendId))
		{
			return DateTime.MaxValue;
		}
		DateTime result;
		if (!DateTime.TryParse(clicksJoinByFriends[friendId], out result))
		{
			return result;
		}
		return DateTime.MaxValue;
	}

	private void ClearListClickJoinFriends()
	{
		clicksJoinByFriends.Clear();
		PlayerPrefs.SetString("CachedFriendsJoinClickList", string.Empty);
	}

	private void UpdateCachedClickJoinListValue()
	{
		if (clicksJoinByFriends.Count != 0)
		{
			string text = Json.Serialize(clicksJoinByFriends);
			PlayerPrefs.SetString("CachedFriendsJoinClickList", text ?? string.Empty);
		}
	}

	private void FillClickJoinFriendsListByCachedValue()
	{
		string @string = PlayerPrefs.GetString("CachedFriendsJoinClickList", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			clicksJoinByFriends.Add(item.Key, Convert.ToString(item.Value));
		}
	}

	private void SyncClickJoinFriendsListWithListFriends()
	{
		if (clicksJoinByFriends.Count == 0)
		{
			return;
		}
		if (friends.Count == 0)
		{
			ClearListClickJoinFriends();
			return;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> clicksJoinByFriend in clicksJoinByFriends)
		{
			if (!playersInfo.ContainsKey(clicksJoinByFriend.Key))
			{
				list.Add(clicksJoinByFriend.Key);
			}
		}
		if (list.Count != 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				string key = list[i];
				clicksJoinByFriends.Remove(key);
			}
			UpdateCachedClickJoinListValue();
		}
	}

	public static ResultParseOnlineData ParseOnlineData(Dictionary<string, string> onlineData)
	{
		string gameModeString = onlineData["game_mode"];
		string protocolString = onlineData["protocol"];
		string mapIndex = string.Empty;
		if (onlineData.ContainsKey("map"))
		{
			mapIndex = onlineData["map"];
		}
		return ParseOnlineData(gameModeString, protocolString, mapIndex);
	}

	private static ResultParseOnlineData ParseOnlineData(string gameModeString, string protocolString, string mapIndex)
	{
		int num = int.Parse(gameModeString);
		num = ((num <= 9999) ? (-1) : ((num - 100000) / 10000));
		int result = -1;
		if (!int.TryParse(gameModeString, out result))
		{
			result = -1;
		}
		else if (result > 9999)
		{
			result %= 10000;
			result /= 100;
		}
		ResultParseOnlineData resultParseOnlineData = new ResultParseOnlineData();
		bool flag = num == -1 || num != (int)GameConnect.myPlatformConnect;
		int tierForRoom = GameConnect.GetTierForRoom();
		bool flag2 = result == -1 || tierForRoom != result;
		bool flag3 = num == 3;
		int num2 = Convert.ToInt32(gameModeString);
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		resultParseOnlineData.gameMode = gameModeString;
		resultParseOnlineData.mapIndex = mapIndex;
		bool flag4 = num2 - 100000 == 6;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(mapIndex));
		bool flag5 = ((infoScene != null && infoScene.IsAvaliableForMode(GameConnect.GameMode.Dater)) ? true : false);
		if (flag4)
		{
			resultParseOnlineData.notConnectCondition = NotConnectCondition.InChat;
		}
		else if (!flag3 && flag && !flag5)
		{
			resultParseOnlineData.notConnectCondition = NotConnectCondition.platform;
		}
		else if (!flag3 && flag2 && !flag5)
		{
			resultParseOnlineData.notConnectCondition = (Defs.useRatingLobbySystem ? NotConnectCondition.league : NotConnectCondition.level);
		}
		else if (multiplayerProtocolVersion != protocolString)
		{
			resultParseOnlineData.notConnectCondition = NotConnectCondition.clientVersion;
		}
		else if (infoScene == null)
		{
			resultParseOnlineData.notConnectCondition = NotConnectCondition.map;
		}
		else
		{
			resultParseOnlineData.notConnectCondition = NotConnectCondition.None;
		}
		resultParseOnlineData.isPlayerInChat = flag4;
		return resultParseOnlineData;
	}

	private static void SendEmailWithMyId()
	{
		MailUrlBuilder mailUrlBuilder = new MailUrlBuilder();
		mailUrlBuilder.to = string.Empty;
		mailUrlBuilder.subject = LocalizationStore.Get("Key_1565");
		if (!(sharedController != null))
		{
			string empty = string.Empty;
		}
		else
		{
			string id2 = sharedController.id;
		}
		string format = LocalizationStore.Get("Key_1508");
		mailUrlBuilder.body = string.Format(format, new object[1] { sharedController.id });
		Application.OpenURL(mailUrlBuilder.GetUrl());
	}

	public static void SendMyIdByEmail()
	{
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1572"), SendEmailWithMyId);
		});
	}

	public static void CopyMyIdToClipboard()
	{
		CopyPlayerIdToClipboard(sharedController.id);
	}

	public static void CopyPlayerIdToClipboard(string playerId)
	{
		UniPasteBoard.SetClipBoardString(playerId);
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1618"));
	}

	public static void JoinToFriendRoom(string friendId)
	{
		if (!(sharedController == null) && sharedController.onlineInfo.ContainsKey(friendId))
		{
			Dictionary<string, string> dictionary = sharedController.onlineInfo[friendId];
			int result;
			int.TryParse(dictionary["game_mode"], out result);
			string nameRoom = dictionary["room_name"];
			string text = dictionary["map"];
			JoinToFriendRoomController instance = JoinToFriendRoomController.Instance;
			if (SceneInfoController.instance.GetInfoScene(int.Parse(text)) != null && instance != null)
			{
				instance.ConnectToRoom(result, nameRoom, text);
				sharedController.UpdateRecordByFriendsJoinClick(friendId);
			}
		}
	}

	public static bool IsPlayerOurFriend(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		return sharedController.friends.Contains(playerId);
	}

	public static bool IsPlayerOurClanMember(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		for (int i = 0; i < sharedController.clanMembers.Count; i++)
		{
			if (sharedController.clanMembers[i]["id"].Equals(playerId))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsSelfClanLeader()
	{
		if (sharedController == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(sharedController.clanLeaderID))
		{
			return false;
		}
		return sharedController.clanLeaderID.Equals(sharedController.id);
	}

	public static void SendFriendshipRequest(string playerId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackResult)
	{
		if (!(sharedController == null))
		{
			if (socialEventParameters == null)
			{
				throw new ArgumentNullException("socialEventParameters");
			}
			sharedController.StartCoroutine(sharedController.FriendRequest(playerId, socialEventParameters, callbackResult));
		}
	}

	public static Dictionary<string, object> GetFullPlayerDataById(string playerId)
	{
		if (sharedController == null)
		{
			return null;
		}
		Dictionary<string, object> value;
		if (sharedController.friendsInfo.TryGetValue(playerId, out value))
		{
			return value;
		}
		if (sharedController.clanFriendsInfo.TryGetValue(playerId, out value))
		{
			return value;
		}
		if (sharedController.profileInfo.TryGetValue(playerId, out value))
		{
			return value;
		}
		UnityEngine.Debug.Log(playerId);
		if (sharedController.playersInfo.TryGetValue(playerId, out value))
		{
			return value;
		}
		return null;
	}

	public static bool IsFriendsMax()
	{
		if (sharedController == null)
		{
			return false;
		}
		return sharedController.friends.Count >= Defs.maxCountFriend;
	}

	public static bool IsFriendsDataExist()
	{
		if (sharedController == null)
		{
			return false;
		}
		if (sharedController.friends.Count > 0)
		{
			return sharedController.friendsInfo.Count > 0;
		}
		return false;
	}

	public static bool IsFriendsOrLocalDataExist()
	{
		if (sharedController == null)
		{
			return false;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, PossiblleOrigin> item in sharedController.getPossibleFriendsResult)
		{
			if (sharedController.profileInfo.ContainsKey(item.Key) && item.Value == PossiblleOrigin.Local)
			{
				list.Add(item.Key);
			}
		}
		if (sharedController.friends.Count <= 0 || sharedController.friendsInfo.Count <= 0)
		{
			return list.Count > 0;
		}
		return true;
	}

	public static bool IsPossibleFriendsDataExist()
	{
		if (sharedController == null)
		{
			return false;
		}
		if (sharedController.getPossibleFriendsResult.Count > 0)
		{
			return sharedController.profileInfo.Count > 0;
		}
		return false;
	}

	public static bool IsFriendInvitesDataExist()
	{
		if (sharedController == null)
		{
			return false;
		}
		if (sharedController.invitesToUs.Count > 0)
		{
			if (sharedController.clanFriendsInfo.Count <= 0)
			{
				return sharedController.profileInfo.Count > 0;
			}
			return true;
		}
		return false;
	}

	public static bool IsDataAboutFriendDownload(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		if (sharedController.friendsInfo.ContainsKey(playerId))
		{
			return true;
		}
		if (sharedController.clanFriendsInfo.ContainsKey(playerId))
		{
			return true;
		}
		if (sharedController.profileInfo.ContainsKey(playerId))
		{
			return true;
		}
		return false;
	}

	public static void ShowProfile(string id, ProfileWindowType type, Action<bool> onCloseEvent = null)
	{
		if (_friendProfileController == null)
		{
			_friendProfileController = new FriendProfileController(onCloseEvent);
		}
		_friendProfileController.HandleProfileClickedCore(id, type, onCloseEvent);
	}

	public static void DisposeProfile()
	{
		if (_friendProfileController != null)
		{
			_friendProfileController.Dispose();
			_friendProfileController = null;
		}
	}

	public static bool IsMyPlayerId(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(sharedController.id))
		{
			return false;
		}
		return sharedController.id.Equals(playerId);
	}

	public static bool IsAlreadySendInvitePlayer(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		return sharedController.invitesFromUs.Contains(playerId);
	}

	public static PossiblleOrigin GetPossibleFriendFindOrigin(string playerId)
	{
		if (sharedController == null)
		{
			return PossiblleOrigin.None;
		}
		if (!sharedController.getPossibleFriendsResult.ContainsKey(playerId))
		{
			return PossiblleOrigin.None;
		}
		return sharedController.getPossibleFriendsResult[playerId];
	}

	public static bool IsAlreadySendClanInvitePlayer(string playerId)
	{
		if (sharedController == null)
		{
			return false;
		}
		return sharedController.ClanSentInvites.Contains(playerId);
	}

	public static bool IsMaxClanMembers()
	{
		if (sharedController == null)
		{
			return false;
		}
		return sharedController.clanMembers.Count >= Defs.maxMemberClanCount;
	}

	public static void StartSendReview()
	{
		if (sharedController != null)
		{
			sharedController.StopCoroutine("WaitReviewAndSend");
			sharedController.StartCoroutine("WaitReviewAndSend");
		}
	}

	private IEnumerator WaitReviewAndSend()
	{
		while (ReviewController.ExistReviewForSend)
		{
			yield return StartCoroutine(SendReviewForPlayerWithID(ReviewController.ReviewRating, ReviewController.ReviewMsg));
			yield return new WaitForSeconds(600f);
		}
	}

	public IEnumerator SendReviewForPlayerWithID(int rating, string msgReview)
	{
		if (ReviewController.isSending)
		{
			yield break;
		}
		ReviewController.isSending = true;
		string playerId = sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			ReviewController.isSending = false;
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "set_feedback");
		wWWForm.AddField("text", msgReview);
		wWWForm.AddField("rating", rating);
		wWWForm.AddField("version", GlobalGameController.AppVersion);
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform);
		wWWForm.AddField("device_model", SystemInfo.deviceModel);
		wWWForm.AddField("auth", Hash("set_feedback"));
		wWWForm.AddField("uniq_id", playerId);
		WWW download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
		if (download == null)
		{
			ReviewController.isSending = false;
			yield break;
		}
		yield return download;
		string text = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			ReviewController.isSending = false;
			UnityEngine.Debug.LogFormat("Error send review: {0}", download.error);
			yield break;
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Review send for id " + playerId + ": " + text);
		}
		ReviewController.isSending = false;
		ReviewController.ExistReviewForSend = false;
	}

	public static void LogPromoTrafficForwarding(TypeTrafficForwardingLog type)
	{
		if (type != TypeTrafficForwardingLog.view || !((DateTime.Now - timeSendTrafficForwarding).TotalMinutes < 60.0))
		{
			if (type == TypeTrafficForwardingLog.view || type == TypeTrafficForwardingLog.newView)
			{
				timeSendTrafficForwarding = DateTime.Now;
			}
			if (sharedController != null)
			{
				sharedController.StartCoroutine(sharedController.SendPromoTrafficForwarding(type));
			}
		}
	}

	public IEnumerator SendPromoTrafficForwarding(TypeTrafficForwardingLog type)
	{
		WWW download;
		while (true)
		{
			if (string.IsNullOrEmpty(id))
			{
				yield break;
			}
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("SendPromoTrafficForwarding:" + type);
			}
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "promo_pgw_stat_update");
			wWWForm.AddField("auth", Hash("promo_pgw_stat_update"));
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("is_paying", Storager.getInt("PayingUser"));
			wWWForm.AddField("platform", ProtocolListGetter.CurrentPlatform);
			if (type == TypeTrafficForwardingLog.click)
			{
				wWWForm.AddField("add_click", 1);
			}
			if (type == TypeTrafficForwardingLog.newView)
			{
				wWWForm.AddField("add_new_view", 1);
			}
			if (type == TypeTrafficForwardingLog.newView || type == TypeTrafficForwardingLog.view)
			{
				wWWForm.AddField("add_view", 1);
			}
			download = Tools.CreateWwwIfNotConnected(actionAddress, wWWForm);
			if (download == null)
			{
				yield break;
			}
			yield return download;
			if (download != null && string.IsNullOrEmpty(download.error))
			{
				break;
			}
			if (Application.isEditor && download != null && !string.IsNullOrEmpty(download.error))
			{
				UnityEngine.Debug.LogWarning("Error send log promo_pgw_stat_update: " + download.error);
			}
			yield return new WaitForSeconds(600f);
		}
		if (Application.isEditor)
		{
			string text = URLs.Sanitize(download);
			UnityEngine.Debug.Log("SendPromoTrafficForwarding(" + type.ToString() + "):" + text);
		}
	}

	private IEnumerator GetABTestAdvertConfig()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.SKIP)
		{
			yield break;
		}
		string text;
		while (true)
		{
			WWW download = Tools.CreateWww(URLs.ABTestAdvertURL);
			if (download == null)
			{
				yield return StartCoroutine(MyWaitForSeconds(30f));
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetABTestAdvertConfig error: " + download.error);
				}
				yield return StartCoroutine(MyWaitForSeconds(30f));
			}
			else
			{
				text = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(text))
				{
					break;
				}
			}
		}
		Storager.setString("abTestAdvertConfigKey", text);
		ParseABTestAdvertConfig();
	}

	public static void ParseABTestAdvertConfig(bool isFromReset = false)
	{
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.SKIP)
		{
			isReadABTestAdvertConfig = true;
		}
		if (!Storager.hasKey("abTestAdvertConfigKey") || string.IsNullOrEmpty(Storager.getString("abTestAdvertConfigKey")))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("abTestAdvertConfigKey")) as Dictionary<string, object>;
		if (dictionary != null && dictionary.ContainsKey("enableABTest"))
		{
			if (Convert.ToInt32(dictionary["enableABTest"]) == 1 && Defs.cohortABTestAdvert != Defs.ABTestCohortsType.SKIP)
			{
				configNameABTestAdvert = Convert.ToString(dictionary["configName"]);
				if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.NONE)
				{
					Defs.cohortABTestAdvert = (Defs.ABTestCohortsType)UnityEngine.Random.Range(1, 3);
					string nameCohort = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
					AnalyticsStuff.LogABTest("Advert", nameCohort);
					if (sharedController != null)
					{
						sharedController.SendOurData();
					}
				}
				if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
				{
					Dictionary<string, object> obj = dictionary["settings-b"] as Dictionary<string, object>;
					FreeAwardController.appId = Convert.ToString(obj["appId"]);
					FreeAwardController.securityToken = Convert.ToString(obj["token"]);
					AdsConfigManager.configFromABTestAdvert = Json.Serialize(dictionary["config"]);
				}
				else
				{
					Dictionary<string, object> obj2 = dictionary["settings-a"] as Dictionary<string, object>;
					FreeAwardController.appId = Convert.ToString(obj2["appId"]);
					FreeAwardController.securityToken = Convert.ToString(obj2["token"]);
				}
			}
			else if (!isFromReset)
			{
				ResetABTestAdvert();
			}
		}
		isReadABTestAdvertConfig = true;
	}

	public static void ResetABTestAdvert()
	{
		if (Defs.cohortABTestAdvert != Defs.ABTestCohortsType.SKIP)
		{
			if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
			{
				string nameCohort = ((Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A) ? "AdvertA_" : "AdvertB_") + configNameABTestAdvert;
				AnalyticsStuff.LogABTest("Advert", nameCohort, false);
				if (sharedController != null)
				{
					sharedController.SendOurData();
				}
			}
			Defs.cohortABTestAdvert = Defs.ABTestCohortsType.SKIP;
			ParseABTestAdvertConfig(true);
			FreeAwardController.appId = "00000";
			FreeAwardController.securityToken = "00000000000000000000000000000000";
			AdsConfigManager.configFromABTestAdvert = string.Empty;
		}
		isReadABTestAdvertConfig = true;
	}

	static FriendsController()
	{
		FriendsController.FriendsUpdated = null;
		FriendsController.FullInfoUpdated = null;
		FriendsController.ServerTimeUpdated = null;
		FriendsController.MapPopularityUpdated = null;
		tickForServerTime = 0f;
		isInitPixelbookSettingsFromServer = false;
		timeOutSendUpdatePlayerFromConnectScene = (Defs.IsDeveloperBuild ? 36f : 360f);
		keylastLikesPlayers = "keylastLikesPlayers";
		_lastLikesPlayers = null;
		timeSendTrafficForwarding = new DateTime(2000, 1, 1, 12, 0, 0);
		_configNameABTestAdvert = "none";
		isReadABTestAdvertConfig = false;
	}
}
