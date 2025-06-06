using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class QuestSystem : MonoBehaviour
{
	private bool _enabled;

	internal const string QuestProgressKey = "QuestProgress";

	internal const string DefaultAvailabilityKey = "QuestSystem.DefaultAvailability";

	private static readonly Rilisoft.Lazy<QuestSystem> _instance = new Rilisoft.Lazy<QuestSystem>(InitializeInstance);

	private const int _questConfigClientVersion = 29;

	private Coroutine _getConfigLoopCoroutine;

	private Coroutine _getTutorialQuestsConfigLoopCoroutine;

	private QuestProgress _questProgress;

	private DateTime? _startupTimeAccordingToServer;

	public static QuestSystem Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public QuestProgress QuestProgress
	{
		get
		{
			return _questProgress;
		}
	}

	internal bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			PlayerPrefs.SetInt("QuestSystem.DefaultAvailability", Convert.ToInt32(value));
			if (_enabled != value)
			{
				_enabled = value;
				if (value)
				{
					InitializeQuestProgress();
				}
				else if (_questProgress != null)
				{
					_questProgress.Dispose();
					_questProgress = null;
				}
				EventHandler updated = this.Updated;
				if (updated != null)
				{
					updated(this, EventArgs.Empty);
				}
			}
		}
	}

	public bool AnyActiveQuest
	{
		get
		{
			if (Enabled && QuestProgress != null)
			{
				return QuestProgress.AnyActiveQuest;
			}
			return false;
		}
	}

	internal int QuestConfigClientVersion
	{
		get
		{
			return 29;
		}
	}

	public event EventHandler Updated;

	public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

	public void Initialize()
	{
	}

	private void Start()
	{
		if (!Enabled)
		{
			UnityEngine.Debug.Log("QuestSystem.Start(): disabled");
		}
		else
		{
			InitializeQuestProgress();
		}
	}

	private void InitializeQuestProgress()
	{
		_questProgress = LoadQuestProgress();
		if (_questProgress != null)
		{
			_questProgress.QuestCompleted += HandleQuestCompleted;
			if (!TutorialQuestManager.Instance.Received)
			{
				_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
				_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			}
		}
		this.Updated.Do(delegate(EventHandler handler)
		{
			handler(this, EventArgs.Empty);
		});
		_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(false));
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.OnApplicationPause({0}): disabled", pauseStatus);
		}
		else if (pauseStatus)
		{
			SaveQuestProgress(_questProgress);
			TutorialQuestManager.Instance.SaveIfDirty();
		}
		else
		{
			_getConfigLoopCoroutine.Do(base.StopCoroutine);
			_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(true));
		}
	}

	internal void DebugDecrementDay()
	{
		if (Enabled)
		{
			if (_questProgress != null)
			{
				_questProgress.DebugDecrementDay();
			}
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
	}

	internal void ForceGetConfig()
	{
		_getConfigLoopCoroutine.Do(base.StopCoroutine);
		_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(false));
	}

	private void HandleQuestCompleted(object sender, QuestCompletedEventArgs e)
	{
		if (!Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.HandleQuestCompleted('{0}'): disabled", e.Quest.Id);
			return;
		}
		SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
		this.QuestCompleted.Do(delegate(EventHandler<QuestCompletedEventArgs> handler)
		{
			handler(sender, e);
		});
	}

	private Task<string> GetQuestConfig()
	{
		TaskCompletionSource<string> val = new TaskCompletionSource<string>();
		StartCoroutine(GetQuestConfigCoroutine(val));
		return val.Task;
	}

	private IEnumerator GetQuestConfigCoroutine(TaskCompletionSource<string> tcs)
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.QuestConfig);
		if (response == null)
		{
			tcs.TrySetException((Exception)new InvalidOperationException("Skipped quest config request because the player is connected."));
			yield break;
		}
		yield return response;
		try
		{
			if (string.IsNullOrEmpty(response.error))
			{
				string text = ((response.text != null) ? URLs.Sanitize(response) : string.Empty);
				tcs.TrySetResult(text);
			}
			else
			{
				tcs.TrySetException((Exception)new InvalidOperationException(response.error));
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("<color=teal>QuestSystem.GetQuestConfigCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
	}

	private Task<string> GetConfigUpdate()
	{
		TaskCompletionSource<string> val = new TaskCompletionSource<string>();
		StartCoroutine(GetConfigUpdateCoroutine(val));
		return val.Task;
	}

	private IEnumerator GetConfigUpdateCoroutine(TaskCompletionSource<string> tcs)
	{
		while (string.IsNullOrEmpty(FriendsController.sharedController.Map((FriendsController fc) => fc.id)))
		{
			yield return null;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "get_quest_version_info");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
		wWWForm.AddField("auth", FriendsController.Hash("get_quest_version_info"));
		WWW response = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
		if (response == null)
		{
			tcs.TrySetException((Exception)new InvalidOperationException("Cannot send request while connected."));
			yield break;
		}
		yield return response;
		try
		{
			if (string.IsNullOrEmpty(response.error))
			{
				string text = ((response.text != null) ? URLs.Sanitize(response) : string.Empty);
				tcs.TrySetResult(text);
			}
			else
			{
				tcs.TrySetException((Exception)new InvalidOperationException(response.error));
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("<color=teal>QuestSystem.GetConfigUpdateCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
	}

	private IEnumerator GetTutorialQuestsConfigOnceCoroutine()
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TutorialQuestConfig);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		try
		{
			if (!string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarningFormat("Failed to load tutorial quests: {0}", response.error);
				yield break;
			}
			string text = ((response.text != null) ? URLs.Sanitize(response) : string.Empty);
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary == null)
			{
				UnityEngine.Debug.LogWarningFormat("Failed to parse config: '{0}'", text);
				yield break;
			}
			List<object> list = dictionary.TryGet("quests") as List<object>;
			if (_questProgress != null && !TutorialQuestManager.Instance.Received)
			{
				if (list != null)
				{
					TutorialQuestManager.Instance.SetReceived();
				}
				_questProgress.FillTutorialQuests(list);
				this.Updated.Do(delegate(EventHandler handler)
				{
					handler(this, EventArgs.Empty);
				});
				SaveQuestProgressIfDirty();
				TutorialQuestManager.Instance.SaveIfDirty();
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("<color=teal>QuestSystem.GetTutorialQuestsConfigOnceCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
	}

	private IEnumerator GetConfigOnceCoroutine(bool resumed)
	{
		if (!Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", resumed);
			yield break;
		}
		Task<string> configUpdateRequest = GetConfigUpdate();
		while (!((Task)configUpdateRequest).IsCompleted)
		{
			yield return null;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (((Task)configUpdateRequest).IsFaulted)
		{
			UnityEngine.Debug.LogWarning(((object)((Exception)(object)((Task)configUpdateRequest).Exception).InnerException) ?? ((object)((Task)configUpdateRequest).Exception));
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(configUpdateRequest.Result) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("GetConfigOnceCoroutine(): Bad update response: " + configUpdateRequest.Result);
			yield break;
		}
		string empty = string.Empty;
		long day = 0L;
		string version;
		float timeLeftSeconds;
		DateTime timestamp;
		try
		{
			int num = Convert.ToInt32(dictionary["version"]);
			int questConfigClientVersion = QuestConfigClientVersion;
			version = string.Format("{0}.{1}", new object[2] { num, questConfigClientVersion });
			day = Convert.ToInt64(dictionary["day"]);
			timeLeftSeconds = (float)Convert.ToDouble(dictionary["timeLeftSeconds"], CultureInfo.InvariantCulture);
			long unixTime = Convert.ToInt64(dictionary["timestamp"], CultureInfo.InvariantCulture);
			timestamp = Tools.GetCurrentTimeByUnixTime(unixTime);
			_startupTimeAccordingToServer = timestamp - TimeSpan.FromSeconds(realtimeSinceStartup);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			yield break;
		}
		if (_questProgress != null && _questProgress.ConfigVersion == version && _questProgress.Day == day)
		{
			yield break;
		}
		if (!Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", resumed);
			yield break;
		}
		Task<string> questConfigRequest = GetQuestConfig();
		while (!((Task)questConfigRequest).IsCompleted)
		{
			yield return null;
		}
		if (((Task)questConfigRequest).IsFaulted)
		{
			UnityEngine.Debug.LogWarning(((Task)questConfigRequest).Exception);
			yield break;
		}
		Dictionary<string, object> rawQuests = Json.Deserialize(questConfigRequest.Result) as Dictionary<string, object>;
		if (rawQuests == null)
		{
			UnityEngine.Debug.LogWarning("GetConfigOnceCoroutine(): Bad config response: " + questConfigRequest.Result);
			yield break;
		}
		List<Difficulty> allowedDifficulties = new List<Difficulty>
		{
			Difficulty.Easy,
			Difficulty.Normal,
			Difficulty.Hard
		};
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 3 && allowedDifficulties.Remove(Difficulty.Hard))
		{
			allowedDifficulties.Add(Difficulty.Normal);
		}
		Rilisoft.Lazy<IDictionary<int, List<QuestBase>>> lazy = new Rilisoft.Lazy<IDictionary<int, List<QuestBase>>>(() => QuestProgress.CreateQuests(rawQuests, day, allowedDifficulties.ToArray()));
		if (_questProgress == null)
		{
			_questProgress = new QuestProgress(version, day, timestamp, timeLeftSeconds);
			_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
			_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			_questProgress.QuestCompleted += HandleQuestCompleted;
			_questProgress.PopulateQuests(lazy.Value, null);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		else if (!_questProgress.ConfigVersion.Equals(version, StringComparison.Ordinal))
		{
			_questProgress.Dispose();
			_questProgress.QuestCompleted -= HandleQuestCompleted;
			_questProgress = new QuestProgress(version, day, timestamp, timeLeftSeconds, _questProgress);
			_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
			_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			_questProgress.QuestCompleted += HandleQuestCompleted;
			_questProgress.PopulateQuests(lazy.Value, null);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		else if (_questProgress.Day < day)
		{
			_questProgress.UpdateQuests(day, rawQuests, lazy.Value);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
	}

	public void SaveQuestProgressIfDirty()
	{
		if (_questProgress == null || !_questProgress.IsDirty())
		{
			return;
		}
		try
		{
			SaveQuestProgress(_questProgress);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private IEnumerator GetConfigLoopCoroutine(bool resumed)
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		float delaySeconds = (Application.isEditor ? 30f : 600f);
		Coroutine configCoroutine = null;
		while (Enabled)
		{
			if (configCoroutine != null)
			{
				StopCoroutine(configCoroutine);
			}
			configCoroutine = StartCoroutine(GetConfigOnceCoroutine(resumed));
			yield return new WaitForRealSeconds(delaySeconds);
		}
		UnityEngine.Debug.LogFormat("QuestSystem.GetConfigLoopCoroutine({0}): disabled", resumed);
	}

	private IEnumerator GetTutorialQuestConfigLoopCoroutine()
	{
		float delaySeconds = (Application.isEditor ? 30f : 600f);
		Coroutine configCoroutine = null;
		while (_questProgress == null || !TutorialQuestManager.Instance.Received)
		{
			if (!Enabled)
			{
				UnityEngine.Debug.Log("QuestSystem.GetTutorialQuestConfigLoopCoroutine({0}): disabled");
				break;
			}
			if (configCoroutine != null)
			{
				StopCoroutine(configCoroutine);
			}
			configCoroutine = StartCoroutine(GetTutorialQuestsConfigOnceCoroutine());
			yield return new WaitForRealSeconds(delaySeconds);
		}
	}

	private QuestProgress LoadQuestProgress()
	{
		if (!Storager.hasKey("QuestProgress"))
		{
			return null;
		}
		string @string = Storager.getString("QuestProgress");
		if (string.IsNullOrEmpty(@string))
		{
			return null;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return null;
		}
		if (dictionary.Count == 0)
		{
			return null;
		}
		Func<string, Version> createVersion = delegate(string v)
		{
			try
			{
				return new Version(v);
			}
			catch
			{
				return new Version(0, 0, 0, 0);
			}
		};
		string text = ((dictionary.Count == 1) ? dictionary.Keys.First() : dictionary.Keys.Select((string k) => new KeyValuePair<string, Version>(k, createVersion(k))).Aggregate((KeyValuePair<string, Version> l, KeyValuePair<string, Version> r) => (!(l.Value > r.Value)) ? r : l).Key);
		Dictionary<string, object> dictionary2 = dictionary[text] as Dictionary<string, object>;
		if (dictionary2 == null)
		{
			return null;
		}
		object value;
		if (!dictionary2.TryGetValue("day", out value))
		{
			return null;
		}
		object value2;
		if (!dictionary2.TryGetValue("timeLeftSeconds", out value2))
		{
			return null;
		}
		object value3;
		if (!dictionary2.TryGetValue("timestamp", out value3))
		{
			return null;
		}
		QuestProgress questProgress = null;
		try
		{
			long day = Convert.ToInt64(value, CultureInfo.InvariantCulture);
			DateTime timestamp = Convert.ToDateTime(value3, CultureInfo.InvariantCulture);
			float timeLeftSeconds = (float)Convert.ToDouble(value2, CultureInfo.InvariantCulture);
			questProgress = new QuestProgress(text, day, timestamp, timeLeftSeconds);
			Dictionary<string, object> dictionary3 = dictionary2["currentQuests"] as Dictionary<string, object>;
			if (dictionary3 == null)
			{
				return questProgress;
			}
			Dictionary<string, object> dictionary4 = dictionary2["previousQuests"] as Dictionary<string, object>;
			if (dictionary4 == null)
			{
				return questProgress;
			}
			IDictionary<int, List<QuestBase>> currentQuests = QuestProgress.RestoreQuests(dictionary3);
			IDictionary<int, List<QuestBase>> previousQuests = QuestProgress.RestoreQuests(dictionary4);
			questProgress.PopulateQuests(currentQuests, previousQuests);
			List<object> questJsons = dictionary2.TryGet("tutorialQuests") as List<object>;
			questProgress.FillTutorialQuests(questJsons);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
		return questProgress;
	}

	private static void SaveQuestProgress(QuestProgress questProgress)
	{
		if (questProgress != null)
		{
			Dictionary<string, object> value = questProgress.ToJson();
			string text = Json.Serialize(new Dictionary<string, object> { { questProgress.ConfigVersion, value } });
			if (questProgress.Count == 0)
			{
				UnityEngine.Debug.LogWarning("SaveQuestProgress(): Bad progress: " + text);
				Storager.setString("QuestProgress", "{}");
			}
			else
			{
				Storager.setString("QuestProgress", text);
				questProgress.SetClean();
			}
		}
	}

	private static QuestSystem InitializeInstance()
	{
		QuestSystem questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
		if (questSystem != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(questSystem.gameObject);
			return questSystem;
		}
		GameObject obj = new GameObject("Rilisoft.QuestSystem");
		UnityEngine.Object.DontDestroyOnLoad(obj);
		QuestSystem questSystem2 = obj.AddComponent<QuestSystem>();
		int value = 0;
		questSystem2._enabled = Convert.ToBoolean(value);
		return questSystem2;
	}
}
