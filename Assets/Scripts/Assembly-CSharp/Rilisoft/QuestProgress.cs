using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft.DictionaryExtensions;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	public sealed class QuestProgress : IDisposable
	{
		private bool _disposed;

		private const bool TutorialQuestsSupported = true;

		private static WeakReference _supportedMapsCache;

		private static WeakReference _supportedModesCache;

		private readonly IDictionary<int, List<QuestBase>> _currentQuests = new Dictionary<int, List<QuestBase>>(3);

		private readonly IDictionary<int, List<QuestBase>> _previousQuests = new Dictionary<int, List<QuestBase>>(3);

		private readonly List<QuestBase> _tutorialQuests = new List<QuestBase>();

		private readonly QuestEvents _events;

		private readonly string _configVersion;

		private readonly DateTime _timestamp;

		private readonly float _timeLeftSeconds;

		private bool _dirty;

		private long _day;

		public string ConfigVersion
		{
			get
			{
				return _configVersion;
			}
		}

		public long Day
		{
			get
			{
				return _day;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return _timestamp;
			}
		}

		public float TimeLeftSeconds
		{
			get
			{
				return _timeLeftSeconds;
			}
		}

		public bool AnyActiveQuest
		{
			get
			{
				return GetActiveQuests().Any((KeyValuePair<int, QuestBase> q) => q.Value != null && !q.Value.Rewarded);
			}
		}

		public int Count
		{
			get
			{
				return _currentQuests.Count + _previousQuests.Count;
			}
		}

		public bool Disposed
		{
			get
			{
				return _disposed;
			}
		}

		public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

		public QuestProgress(string configVersion, long day, DateTime timestamp, float timeLeftSeconds, QuestProgress oldQuestProgress = null)
		{
			if (string.IsNullOrEmpty(configVersion))
			{
				throw new ArgumentException("ConfigId should not be empty.", "configVersion");
			}
			_events = QuestMediator.Events;
			_events.Win += HandleWin;
			_events.KillOtherPlayer += HandleKillOtherPlayer;
			_events.KillOtherPlayerWithFlag += HandleKillOtherPlayerWithFlag;
			_events.Capture += HandleCapture;
			_events.KillMonster += HandleKillMonster;
			_events.BreakSeries += HandleBreakSeries;
			_events.MakeSeries += HandleMakeSeries;
			_events.SurviveWaveInArena += HandleSurviveInArena;
			_events.GetGotcha += HandleGetGotcha;
			_events.SocialInteraction += HandleSocialInteraction;
			_configVersion = configVersion;
			_timestamp = timestamp;
			_timeLeftSeconds = timeLeftSeconds;
			_day = day;
			if (oldQuestProgress != null)
			{
				_tutorialQuests = oldQuestProgress._tutorialQuests;
				foreach (QuestBase tutorialQuest in _tutorialQuests)
				{
					tutorialQuest.Changed += OnQuestChangedCheckCompletion;
				}
			}
			UnityEngine.Random.seed = (int)Tools.CurrentUnixTime;
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> currentQuest in _currentQuests)
			{
				string key = currentQuest.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list = new List<object>(2);
				foreach (QuestBase item in currentQuest.Value)
				{
					list.Add(item.ToJson());
				}
				dictionary[key] = list;
			}
			Dictionary<string, List<object>> dictionary2 = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> previousQuest in _previousQuests)
			{
				string key2 = previousQuest.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list2 = new List<object>(2);
				foreach (QuestBase item2 in previousQuest.Value)
				{
					list2.Add(item2.ToJson());
				}
				dictionary2[key2] = list2;
			}
			List<object> list3 = new List<object>(_tutorialQuests.Count);
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				list3.Add(tutorialQuest.ToJson());
			}
			return new Dictionary<string, object>(3)
			{
				{ "day", _day },
				{
					"timestamp",
					Timestamp.ToString("s", CultureInfo.InvariantCulture)
				},
				{
					"timeLeftSeconds",
					TimeLeftSeconds.ToString(CultureInfo.InvariantCulture)
				},
				{ "tutorialQuests", list3 },
				{ "previousQuests", dictionary2 },
				{ "currentQuests", dictionary }
			};
		}

		public void UpdateQuests(long day, Dictionary<string, object> rawQuests, IDictionary<int, List<QuestBase>> newQuests)
		{
			if (newQuests == null)
			{
				return;
			}
			_day = day;
			Dictionary<int, IList<QuestBase>> dictionary = _previousQuests.Keys.Concat(_currentQuests.Keys).Distinct().ToDictionary((int s) => s, (int s) => GetActiveQuestsBySlot(s, true));
			ClearQuests(_previousQuests);
			foreach (KeyValuePair<int, IList<QuestBase>> item in dictionary)
			{
				int key = item.Key;
				IList<QuestBase> value = item.Value;
				foreach (QuestBase item2 in value)
				{
					item2.Changed -= OnQuestChangedCheckCompletion;
					if (!item2.Rewarded)
					{
						item2.Changed += OnQuestChangedCheckCompletion;
					}
				}
				_previousQuests[key] = new List<QuestBase>(value.Where((QuestBase q) => !q.Rewarded));
			}
			ClearQuests(_currentQuests);
			foreach (KeyValuePair<int, List<QuestBase>> newQuest in newQuests)
			{
				int key2 = newQuest.Key;
				List<QuestBase> value2 = newQuest.Value;
				List<QuestBase> value3;
				if (!_previousQuests.TryGetValue(key2, out value3))
				{
					value3 = new List<QuestBase>();
				}
				if (value3.FirstOrDefault().Map((QuestBase q) => q.CalculateProgress() < 1m && !q.Rewarded))
				{
					continue;
				}
				foreach (QuestBase item3 in value2)
				{
					item3.Changed -= OnQuestChangedCheckCompletion;
					item3.Changed += OnQuestChangedCheckCompletion;
				}
				_currentQuests[key2] = new List<QuestBase>(value2);
			}
			if (rawQuests != null)
			{
				Difficulty[] allowedDifficulties = _previousQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => kv.Value.Select((QuestBase q) => q.Difficulty)).Distinct().ToArray();
				ParseQuests(rawQuests, day, allowedDifficulties, _previousQuests);
			}
			_dirty = true;
		}

		public void PopulateQuests(IDictionary<int, List<QuestBase>> currentQuests, IDictionary<int, List<QuestBase>> previousQuests)
		{
			if (currentQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> currentQuest in currentQuests)
				{
					foreach (QuestBase item in currentQuest.Value)
					{
						item.Changed += OnQuestChangedCheckCompletion;
					}
					_currentQuests[currentQuest.Key] = new List<QuestBase>(currentQuest.Value);
				}
			}
			if (previousQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> previousQuest in previousQuests)
				{
					foreach (QuestBase item2 in previousQuest.Value)
					{
						item2.Changed += OnQuestChangedCheckCompletion;
					}
					_previousQuests[previousQuest.Key] = new List<QuestBase>(previousQuest.Value);
				}
			}
			_dirty = true;
		}

		public void FillTutorialQuests(List<object> questJsons)
		{
			if (questJsons == null)
			{
				return;
			}
			TutorialQuestManager.Instance.FillTutorialQuests(questJsons, Day, _tutorialQuests);
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				tutorialQuest.Changed -= OnQuestChangedCheckCompletion;
				tutorialQuest.Changed += OnQuestChangedCheckCompletion;
			}
			_dirty = true;
		}

		public static IDictionary<int, List<QuestBase>> RestoreQuests(Dictionary<string, object> rawQuests)
		{
			Difficulty[] allowedDifficulties = new Difficulty[3]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			return ParseQuests(rawQuests, null, allowedDifficulties);
		}

		public static IDictionary<int, List<QuestBase>> CreateQuests(Dictionary<string, object> rawQuests, long day, Difficulty[] allowedDifficulties)
		{
			if (allowedDifficulties == null)
			{
				allowedDifficulties = new Difficulty[3]
				{
					Difficulty.Easy,
					Difficulty.Normal,
					Difficulty.Hard
				};
			}
			return ParseQuests(rawQuests, day, allowedDifficulties);
		}

		internal void DebugDecrementDay()
		{
			long newDay = _day - 172800;
			foreach (QuestBase item in from q in _previousQuests.Values.SelectMany((List<QuestBase> q) => q).Concat(_currentQuests.Values.SelectMany((List<QuestBase> q) => q)).Concat(_tutorialQuests)
				where newDay < q.Day
				select q)
			{
				item.DebugSetDay(newDay);
			}
			_day = newDay;
			_dirty = true;
		}

		private static IDictionary<int, List<QuestBase>> ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties)
		{
			Dictionary<int, List<QuestBase>> dictionary = new Dictionary<int, List<QuestBase>>(3);
			ParseQuests(rawQuests, dayOption, allowedDifficulties, dictionary);
			return dictionary;
		}

		private static HashSet<ShopNGUIController.CategoryNames> InitializeExcludedWeaponSlots(int slot)
		{
			HashSet<ShopNGUIController.CategoryNames> hashSet = new HashSet<ShopNGUIController.CategoryNames>();
			if (QuestSystem.Instance == null || QuestSystem.Instance.QuestProgress == null)
			{
				return hashSet;
			}
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = QuestSystem.Instance.QuestProgress.GetActiveQuestBySlot(slot) as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				hashSet.Add(weaponSlotAccumulativeQuest.WeaponSlot);
			}
			return hashSet;
		}

		private static void ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> actualResult)
		{
			if (actualResult == null || rawQuests == null || rawQuests.Count == 0)
			{
				return;
			}
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			bool flag = !dayOption.HasValue;
			IDictionary<int, List<QuestBase>> existingQuests = ((QuestSystem.Instance.QuestProgress != null) ? QuestSystem.Instance.QuestProgress.GetActiveQuests().ToDictionary((KeyValuePair<int, QuestBase> kv) => kv.Key, (KeyValuePair<int, QuestBase> kv) => new List<QuestBase> { kv.Value }) : new Dictionary<int, List<QuestBase>>());
			if (allowedDifficulties.Length == 0)
			{
				allowedDifficulties = new Difficulty[3]
				{
					Difficulty.Easy,
					Difficulty.Normal,
					Difficulty.Hard
				};
			}
			IDictionary<int, List<Dictionary<string, object>>> dictionary = (flag ? ExtractQuests(rawQuests) : FilterQuests(rawQuests, allowedDifficulties, existingQuests));
			Difficulty[] array = new Difficulty[3]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			foreach (KeyValuePair<int, List<Dictionary<string, object>>> item5 in dictionary)
			{
				int key = item5.Key;
				List<QuestBase> value;
				if (!actualResult.TryGetValue(key, out value))
				{
					value = new List<QuestBase>(2);
				}
				HashSet<ShopNGUIController.CategoryNames> hashSet = InitializeExcludedWeaponSlots(key);
				foreach (Dictionary<string, object> item6 in item5.Value)
				{
					string text = item6.TryGet("id") as string;
					if (text == null)
					{
						continue;
					}
					if (!QuestConstants.IsSupported(text))
					{
						Debug.LogWarning("Quest is not supported: " + text);
						continue;
					}
					Difficulty difficulty = Difficulty.None;
					object value2 = null;
					Difficulty[] array2 = array;
					foreach (Difficulty difficulty2 in array2)
					{
						if (item6.TryGetValue(QuestConstants.GetDifficultyKey(difficulty2), out value2))
						{
							difficulty = difficulty2;
							break;
						}
					}
					Dictionary<string, object> dictionary2 = value2 as Dictionary<string, object>;
					if (dictionary2 == null || difficulty == Difficulty.None)
					{
						continue;
					}
					try
					{
						Reward reward = Reward.Create(dictionary2["reward"] as List<object>);
						int requiredCount = Convert.ToInt32(dictionary2.TryGet("parameter") ?? ((object)1));
						object value3 = item6.TryGet("day");
						long day = (dayOption.HasValue ? dayOption.Value : Convert.ToInt64(value3));
						bool rewarded = item6.TryGet("rewarded").Map(Convert.ToBoolean);
						bool active = item6.TryGet("active").Map(Convert.ToBoolean);
						int initialCount = item6.TryGet("currentCount").Map(Convert.ToInt32);
						switch (text)
						{
						case "killInMode":
						case "winInMode":
						{
							GameConnect.GameMode? gameMode = ExtractModeFromQuestDescription(item6, flag, text);
							if (gameMode.HasValue)
							{
								ModeAccumulativeQuest item3 = new ModeAccumulativeQuest(text, day, key, difficulty, reward, active, rewarded, requiredCount, gameMode.Value, initialCount);
								value.Add(item3);
							}
							break;
						}
						case "winInMap":
						{
							string text2 = ExtractMapFromQuestDescription(item6, flag);
							if (!string.IsNullOrEmpty(text2))
							{
								MapAccumulativeQuest item4 = new MapAccumulativeQuest(text, day, key, difficulty, reward, active, rewarded, requiredCount, text2, initialCount);
								value.Add(item4);
							}
							break;
						}
						case "killWithWeapon":
						case "killNpcWithWeapon":
						{
							ShopNGUIController.CategoryNames? categoryNames = ExtractWeaponSlotFromQuestDescription(item6, flag, hashSet);
							if (categoryNames.HasValue)
							{
								hashSet.Add(categoryNames.Value);
								WeaponSlotAccumulativeQuest item2 = new WeaponSlotAccumulativeQuest(text, day, key, difficulty, reward, active, rewarded, requiredCount, categoryNames.Value, initialCount);
								value.Add(item2);
							}
							break;
						}
						default:
						{
							SimpleAccumulativeQuest item = new SimpleAccumulativeQuest(text, day, key, difficulty, reward, active, rewarded, requiredCount, initialCount);
							value.Add(item);
							break;
						}
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
				actualResult[key] = value;
			}
		}

		public QuestBase GetActiveQuestBySlot(int slot)
		{
			QuestBase activeTutorialQuest = GetActiveTutorialQuest();
			if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
			{
				return activeTutorialQuest;
			}
			List<QuestBase> value = null;
			_previousQuests.TryGetValue(slot, out value);
			QuestBase questBase = value.Map((List<QuestBase> ps) => ps.FirstOrDefault());
			if (questBase != null && !questBase.Rewarded)
			{
				return questBase;
			}
			List<QuestBase> value2 = null;
			_currentQuests.TryGetValue(slot, out value2);
			QuestBase questBase2 = value2.Map((List<QuestBase> cs) => cs.FirstOrDefault());
			if (questBase2 != null)
			{
				return questBase2;
			}
			return questBase;
		}

		public QuestInfo GetActiveQuestInfoBySlot(int slot)
		{
			IList<QuestBase> activeQuestsBySlot = GetActiveQuestsBySlot(slot);
			Func<IList<QuestBase>> skipMethod = delegate
			{
				foreach (int item in _currentQuests.Keys.Concat(_previousQuests.Keys).Concat(_tutorialQuests.Select((QuestBase q) => q.Slot)).Distinct())
				{
					List<QuestBase> activeQuestsBySlotReference = GetActiveQuestsBySlotReference(item);
					if (activeQuestsBySlotReference != null && activeQuestsBySlotReference.Count > 0 && slot == item)
					{
						activeQuestsBySlotReference.RemoveAt(0);
						_dirty = true;
					}
					List<QuestBase> activeQuestsBySlotReference2 = GetActiveQuestsBySlotReference(item, true);
					if (activeQuestsBySlotReference2.Count > 1)
					{
						activeQuestsBySlotReference2.RemoveRange(1, activeQuestsBySlotReference2.Count - 1);
						List<QuestBase> value;
						if (activeQuestsBySlotReference2[0].CalculateProgress() >= 1m && _currentQuests.TryGetValue(item, out value) && value.Count > 1)
						{
							value.RemoveRange(1, value.Count - 1);
						}
						_dirty = true;
					}
				}
				return GetActiveQuestsBySlot(slot);
			};
			bool forcedSkip = _tutorialQuests == GetActiveQuestsBySlotReference(slot);
			return new QuestInfo(activeQuestsBySlot, skipMethod, forcedSkip);
		}

		public QuestInfo GetRandomQuestInfo()
		{
			List<QuestInfo> list = (from qi in _previousQuests.Keys.Concat(_currentQuests.Keys).Concat(_tutorialQuests.Select((QuestBase q) => q.Slot)).Distinct()
					.Select(GetActiveQuestInfoBySlot)
				where qi.Quest != null && !qi.Quest.Rewarded
				select qi).ToList();
			if (list.Count < 1)
			{
				return null;
			}
			QuestInfo questInfo = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				QuestInfo questInfo2 = list[i];
				if (questInfo2.Quest == null)
				{
					continue;
				}
				if (questInfo.Quest == null)
				{
					questInfo = list[i];
					continue;
				}
				AccumulativeQuestBase accumulativeQuestBase = questInfo.Quest as AccumulativeQuestBase;
				AccumulativeQuestBase accumulativeQuestBase2 = questInfo2.Quest as AccumulativeQuestBase;
				if (accumulativeQuestBase != null && accumulativeQuestBase2 != null)
				{
					if (accumulativeQuestBase2.RequiredCount - accumulativeQuestBase2.CurrentCount < accumulativeQuestBase.RequiredCount - accumulativeQuestBase.CurrentCount)
					{
						questInfo = questInfo2;
					}
				}
				else if (questInfo.Quest.CalculateProgress() < questInfo2.Quest.CalculateProgress())
				{
					questInfo = questInfo2;
				}
			}
			return questInfo;
		}

		public IDictionary<int, QuestBase> GetActiveQuests()
		{
			IEnumerable<int> enumerable = _previousQuests.Keys.Concat(_currentQuests.Keys).Concat(_tutorialQuests.Select((QuestBase q) => q.Slot)).Distinct();
			Dictionary<int, QuestBase> dictionary = new Dictionary<int, QuestBase>();
			foreach (int item in enumerable)
			{
				QuestBase activeQuestBySlot = GetActiveQuestBySlot(item);
				if (activeQuestBySlot != null)
				{
					dictionary[item] = activeQuestBySlot;
				}
			}
			return dictionary;
		}

		internal bool TryRemoveTutorialQuest(string questId)
		{
			if (questId == null)
			{
				return false;
			}
			int num = _tutorialQuests.FindIndex((QuestBase q) => questId.Equals(q.Id, StringComparison.Ordinal));
			if (num < 0)
			{
				return false;
			}
			_tutorialQuests.RemoveAt(num);
			_dirty = true;
			return true;
		}

		private List<QuestBase> GetActiveQuestsBySlotReference(int slot, bool ignoreTutorialQuests = false)
		{
			if (!ignoreTutorialQuests)
			{
				QuestBase activeTutorialQuest = GetActiveTutorialQuest();
				if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
				{
					return _tutorialQuests;
				}
			}
			List<QuestBase> value;
			_previousQuests.TryGetValue(slot, out value);
			if (value.Map((List<QuestBase> qs) => qs.Count > 0 && qs.All((QuestBase q) => !q.Rewarded)))
			{
				return value;
			}
			List<QuestBase> value2;
			if (_currentQuests.TryGetValue(slot, out value2) && value2.Map((List<QuestBase> qs) => qs.Count > 0))
			{
				return value2;
			}
			return value;
		}

		private IList<QuestBase> GetActiveQuestsBySlot(int slot, bool ignoreTutorialQuests = false)
		{
			List<QuestBase> activeQuestsBySlotReference = GetActiveQuestsBySlotReference(slot, ignoreTutorialQuests);
			if (activeQuestsBySlotReference == null)
			{
				return new List<QuestBase>();
			}
			return new List<QuestBase>(activeQuestsBySlotReference);
		}

		private QuestBase GetTutorialQuestById(string id)
		{
			if (id == null)
			{
				return null;
			}
			QuestBase activeTutorialQuest = GetActiveTutorialQuest();
			if (activeTutorialQuest == null)
			{
				return null;
			}
			if (!id.Equals(activeTutorialQuest.Id, StringComparison.Ordinal))
			{
				return null;
			}
			return activeTutorialQuest;
		}

		private QuestBase GetActiveTutorialQuest()
		{
			if (_tutorialQuests.Count == 0)
			{
				return null;
			}
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				if (!tutorialQuest.Rewarded)
				{
					return tutorialQuest;
				}
			}
			return null;
		}

		private QuestBase GetQuestById(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			return _previousQuests.Keys.Concat(_currentQuests.Keys).Concat(_tutorialQuests.Select((QuestBase q) => q.Slot)).Distinct()
				.Select(GetActiveQuestBySlot)
				.FirstOrDefault((QuestBase q) => q.Id.Equals(id, StringComparison.Ordinal));
		}

		[Obsolete]
		public AccumulativeQuestBase GetRandomInProgressAccumQuest()
		{
			List<AccumulativeQuestBase> list = (from qs in _previousQuests.Values
				where qs.Count > 0
				select qs.First() into q
				where q.CalculateProgress() < 1m
				select q).OfType<AccumulativeQuestBase>().ToList();
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
			List<AccumulativeQuestBase> list2 = (from qs in _previousQuests.Values
				where qs.Count > 0
				select qs.First() into q
				where !q.Rewarded
				select q).OfType<AccumulativeQuestBase>().ToList();
			if (list2.Count > 0)
			{
				return list2[UnityEngine.Random.Range(0, list2.Count)];
			}
			AccumulativeQuestBase[] array = (from qs in _currentQuests.Values
				where qs.Count > 0
				select qs.First() into q
				where q.CalculateProgress() < 1m
				select q).OfType<AccumulativeQuestBase>().ToArray();
			if (array.Length != 0)
			{
				return array[UnityEngine.Random.Range(0, array.Length)];
			}
			AccumulativeQuestBase[] array2 = (from qs in _currentQuests.Values
				where qs.Count > 0
				select qs.First() into q
				where !q.Rewarded
				select q).OfType<AccumulativeQuestBase>().ToArray();
			if (array2.Length != 0)
			{
				return array2[UnityEngine.Random.Range(0, array2.Length)];
			}
			return null;
		}

		public bool HasUnrewaredAccumQuests()
		{
			IEnumerable<QuestBase> first = from qs in _currentQuests.Values
				where qs.Count > 0
				select qs[0];
			IEnumerable<QuestBase> second = from qs in _previousQuests.Values
				where qs.Count > 0
				select qs[0];
			return first.Concat(second).Concat(_tutorialQuests).OfType<AccumulativeQuestBase>()
				.Any((AccumulativeQuestBase q) => q.CalculateProgress() >= 1m && !q.Rewarded);
		}

		public bool IsDirty()
		{
			if (!_dirty && !_currentQuests.Values.SelectMany((List<QuestBase> q) => q).Any((QuestBase q) => q.Dirty) && !_previousQuests.Values.SelectMany((List<QuestBase> q) => q).Any((QuestBase q) => q.Dirty))
			{
				return _tutorialQuests.Any((QuestBase q) => q.Dirty);
			}
			return true;
		}

		public void SetClean()
		{
			foreach (List<QuestBase> value in _currentQuests.Values)
			{
				foreach (QuestBase item in value)
				{
					item.SetClean();
				}
			}
			foreach (List<QuestBase> value2 in _previousQuests.Values)
			{
				foreach (QuestBase item2 in value2)
				{
					item2.SetClean();
				}
			}
			_dirty = false;
		}

		private void ClearQuests(IDictionary<int, List<QuestBase>> quests)
		{
			if (quests == null)
			{
				return;
			}
			foreach (KeyValuePair<int, List<QuestBase>> quest in quests)
			{
				quest.Value.Clear();
			}
			quests.Clear();
		}

		private static IDictionary<int, List<Dictionary<string, object>>> ExtractQuests(Dictionary<string, object> rawQuests)
		{
			Dictionary<int, List<Dictionary<string, object>>> dictionary = new Dictionary<int, List<Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				int result;
				if (int.TryParse(rawQuest.Key, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
				{
					List<object> list = rawQuest.Value as List<object>;
					if (list == null)
					{
						dictionary[result] = new List<Dictionary<string, object>>();
						continue;
					}
					List<Dictionary<string, object>> value = list.OfType<Dictionary<string, object>>().ToList();
					dictionary[result] = value;
				}
			}
			return dictionary;
		}

		private static IDictionary<int, List<Dictionary<string, object>>> FilterQuests(Dictionary<string, object> rawQuests, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> existingQuests)
		{
			if (existingQuests == null)
			{
				existingQuests = new Dictionary<int, List<QuestBase>>();
			}
			Dictionary<int, Dictionary<string, Dictionary<string, object>>> dictionary = new Dictionary<int, Dictionary<string, Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				Dictionary<string, object> dictionary2 = rawQuest.Value as Dictionary<string, object>;
				object value;
				if (dictionary2 == null || !dictionary2.TryGetValue("slot", out value) || !QuestConstants.IsSupported(rawQuest.Key))
				{
					continue;
				}
				try
				{
					int key = Convert.ToInt32(value, NumberFormatInfo.InvariantInfo);
					Dictionary<string, Dictionary<string, object>> value2;
					if (!dictionary.TryGetValue(key, out value2))
					{
						value2 = (dictionary[key] = new Dictionary<string, Dictionary<string, object>>(3));
					}
					value2[rawQuest.Key] = dictionary2;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			List<Difficulty> list = new List<Difficulty>(dictionary.Count);
			for (int i = 0; i != dictionary.Count; i++)
			{
				Difficulty item = allowedDifficulties[i % allowedDifficulties.Length];
				list.Add(item);
			}
			ShuffleInPlace(list);
			Dictionary<int, List<Dictionary<string, object>>> dictionary4 = new Dictionary<int, List<Dictionary<string, object>>>();
			Dictionary<int, Dictionary<string, Dictionary<string, object>>>.Enumerator enumerator2 = dictionary.GetEnumerator();
			List<Difficulty> source = new List<Difficulty>
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			int num = 0;
			while (enumerator2.MoveNext())
			{
				int key2 = enumerator2.Current.Key;
				Dictionary<string, Dictionary<string, object>> value3 = enumerator2.Current.Value;
				List<QuestBase> value4;
				existingQuests.TryGetValue(key2, out value4);
				Difficulty chosenDifficulty = list[num];
				string chosenDifficultyKey = QuestConstants.GetDifficultyKey(chosenDifficulty);
				List<KeyValuePair<string, Dictionary<string, object>>> list2 = value3.Where((KeyValuePair<string, Dictionary<string, object>> kv) => kv.Value.ContainsKey(chosenDifficultyKey)).ToList();
				if (list2.Count == 0)
				{
					value3.Clear();
				}
				else
				{
					if (list2.Count > 1)
					{
						string existingQuestId = value4.Map((List<QuestBase> l) => l.FirstOrDefault()).Map((QuestBase q) => q.Id);
						list2.RemoveAll((KeyValuePair<string, Dictionary<string, object>> kv) => StringComparer.OrdinalIgnoreCase.Equals(kv.Key, existingQuestId));
					}
					List<int> list3 = Enumerable.Range(0, list2.Count).ToList();
					ShuffleInPlace(list3);
					KeyValuePair<string, Dictionary<string, object>> keyValuePair = list2[list3[0]];
					keyValuePair.Value["id"] = keyValuePair.Key;
					value3.Clear();
					value3[keyValuePair.Key] = keyValuePair.Value;
					List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>(2) { keyValuePair.Value.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value) };
					if (value4.Map((List<QuestBase> l) => l.Count == 0, true))
					{
						KeyValuePair<string, Dictionary<string, object>> keyValuePair2 = list2[list3[list3.Count - 1]];
						keyValuePair2.Value["id"] = keyValuePair2.Key;
						list4.Add(keyValuePair2.Value.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value));
					}
					dictionary4[key2] = list4;
					foreach (Difficulty item2 in source.Where((Difficulty d) => d != chosenDifficulty))
					{
						string difficultyKey = QuestConstants.GetDifficultyKey(item2);
						foreach (Dictionary<string, object> item3 in list4)
						{
							item3.Remove(difficultyKey);
						}
					}
				}
				num++;
			}
			return dictionary4;
		}

		private static string ExtractMapFromQuestDescription(Dictionary<string, object> q, bool restore)
		{
			if (q == null || q.Count == 0)
			{
				return string.Empty;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("map", out value))
				{
					return null;
				}
				return Convert.ToString(value);
			}
			string[] supportedMaps = GetSupportedMaps();
			object value2;
			if (!q.TryGetValue("maps", out value2))
			{
				return supportedMaps[UnityEngine.Random.Range(0, supportedMaps.Length - 1)];
			}
			List<object> list = value2 as List<object>;
			if (list == null)
			{
				return string.Empty;
			}
			string[] array = list.OfType<string>().Intersect(supportedMaps).ToArray();
			if (array.Length == 0)
			{
				return string.Empty;
			}
			return array[UnityEngine.Random.Range(0, array.Length - 1)];
		}

		private static string[] GetSupportedMaps()
		{
			if (_supportedMapsCache != null && _supportedMapsCache.IsAlive)
			{
				return (string[])_supportedMapsCache.Target;
			}
			HashSet<GameConnect.GameMode> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(ExperienceController.sharedController.Map((ExperienceController xp) => xp.currentLevel, 1));
			unlockedModesByLevel.Remove(GameConnect.GameMode.Dater);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (SceneInfo allScene in SceneInfoController.instance.allScenes)
			{
				if (allScene.isPremium || allScene.NameScene == "Developer_Scene" || allScene.NameScene == "Matrix")
				{
					continue;
				}
				foreach (GameConnect.GameMode item in unlockedModesByLevel)
				{
					if (allScene.IsAvaliableForMode(item))
					{
						hashSet.Add(allScene.NameScene);
					}
				}
			}
			string[] array = hashSet.ToArray();
			_supportedMapsCache = new WeakReference(array, false);
			return array;
		}

		private static ShopNGUIController.CategoryNames? ExtractWeaponSlotFromQuestDescription(Dictionary<string, object> q, bool restore, HashSet<ShopNGUIController.CategoryNames> excluded)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("weaponSlot", out value))
				{
					return null;
				}
				return QuestConstants.ParseWeaponSlot(Convert.ToString(value));
			}
			if (excluded == null)
			{
				excluded = new HashSet<ShopNGUIController.CategoryNames>();
			}
			List<ShopNGUIController.CategoryNames> list = Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).Cast<ShopNGUIController.CategoryNames>().Where(ShopNGUIController.IsWeaponCategory)
				.ToList();
			object value2;
			if (!q.TryGetValue("weaponSlots", out value2))
			{
				List<ShopNGUIController.CategoryNames> list2 = list.Except(excluded).ToList();
				list2 = ((list2.Count > 0) ? list2 : list);
				return list2[UnityEngine.Random.Range(0, list2.Count - 1)];
			}
			List<object> list3 = value2 as List<object>;
			if (list3 == null)
			{
				return null;
			}
			IEnumerable<string> enumerable = list3.OfType<string>();
			List<ShopNGUIController.CategoryNames> list4 = new List<ShopNGUIController.CategoryNames>();
			foreach (string item in enumerable)
			{
				ShopNGUIController.CategoryNames? categoryNames = QuestConstants.ParseWeaponSlot(item);
				if (categoryNames.HasValue && list.Contains(categoryNames.Value))
				{
					list4.Add(categoryNames.Value);
				}
			}
			if (list4.Count == 0)
			{
				return null;
			}
			List<ShopNGUIController.CategoryNames> list5 = list4.Except(excluded).ToList();
			list5 = ((list5.Count > 0) ? list5 : list4);
			return list5[UnityEngine.Random.Range(0, list5.Count - 1)];
		}

		private static GameConnect.GameMode? ExtractModeFromQuestDescription(Dictionary<string, object> q, bool restore, string questId)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("mode", out value))
				{
					return null;
				}
				return QuestConstants.ParseMode(Convert.ToString(value));
			}
			GameConnect.GameMode[] supportedModes = GetSupportedModes();
			List<GameConnect.GameMode> list = new List<GameConnect.GameMode>(supportedModes.Length);
			GameConnect.GameMode[] array = supportedModes;
			foreach (GameConnect.GameMode gameMode in array)
			{
				if (gameMode != GameConnect.GameMode.TimeBattle || !StringComparer.OrdinalIgnoreCase.Equals("killInMode", questId))
				{
					list.Add(gameMode);
				}
			}
			object value2;
			if (!q.TryGetValue("modes", out value2))
			{
				if (list.Count <= 0)
				{
					return null;
				}
				return list[UnityEngine.Random.Range(0, list.Count - 1)];
			}
			List<object> list2 = value2 as List<object>;
			if (list2 == null)
			{
				return null;
			}
			IEnumerable<string> enumerable = list2.OfType<string>();
			List<GameConnect.GameMode> list3 = new List<GameConnect.GameMode>();
			foreach (string item in enumerable)
			{
				GameConnect.GameMode? gameMode2 = QuestConstants.ParseMode(item);
				if (gameMode2.HasValue && list.Contains(gameMode2.Value))
				{
					list3.Add(gameMode2.Value);
				}
			}
			if (list3.Count == 0)
			{
				return null;
			}
			return list3[UnityEngine.Random.Range(0, list3.Count - 1)];
		}

		private static GameConnect.GameMode[] GetSupportedModes()
		{
			if (_supportedModesCache != null && _supportedModesCache.IsAlive)
			{
				return (GameConnect.GameMode[])_supportedModesCache.Target;
			}
			GameConnect.GameMode[] array = SceneInfoController.GetUnlockedModesByLevel(ExperienceController.sharedController.Map((ExperienceController xp) => xp.currentLevel, 1)).ToArray();
			_supportedModesCache = new WeakReference(array, false);
			return array;
		}

		private static void ShuffleInPlace<T>(List<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.Count >= 2)
			{
				for (int num = list.Count - 1; num >= 1; num--)
				{
					int index = UnityEngine.Random.Range(0, num);
					T value = list[index];
					list[index] = list[num];
					list[num] = value;
				}
			}
		}

		private static List<T> Shuffle<T>(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<T> list2 = list.ToList();
			ShuffleInPlace(list2);
			return list2;
		}

		public void FilterFulfilledTutorialQuests()
		{
			_tutorialQuests.RemoveAll((QuestBase tq) => TutorialQuestManager.Instance.CheckQuestIfFulfilled(tq.Id) && tq.CalculateProgress() < 1m);
		}

		private void OnQuestChangedCheckCompletion(object sender, EventArgs e)
		{
			QuestBase quest = sender as QuestBase;
			if (quest == null || !(quest.CalculateProgress() >= 1m))
			{
				return;
			}
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.OnQuestChangedCheckCompletion({1})", GetType().Name, quest.Id);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				this.QuestCompleted.Do(delegate(EventHandler<QuestCompletedEventArgs> handler)
				{
					handler(this, new QuestCompletedEventArgs
					{
						Quest = quest
					});
				});
			}
		}

		private void HandleWin(object sender, WinEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleWin(): " + e);
			}
			QuestBase questById = GetQuestById("winInMap");
			if (questById != null)
			{
				MapAccumulativeQuest mapAccumulativeQuest = questById as MapAccumulativeQuest;
				if (mapAccumulativeQuest != null)
				{
					mapAccumulativeQuest.IncrementIf(mapAccumulativeQuest.Map.Equals(e.Map, StringComparison.Ordinal));
				}
			}
			questById = GetQuestById("winInMode");
			if (questById != null)
			{
				ModeAccumulativeQuest modeAccumulativeQuest = questById as ModeAccumulativeQuest;
				if (modeAccumulativeQuest != null)
				{
					modeAccumulativeQuest.IncrementIf(modeAccumulativeQuest.Mode == e.Mode);
				}
			}
		}

		private void HandleKillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayer(): " + e);
			}
			QuestBase questById = GetQuestById("killInMode");
			if (questById != null)
			{
				(questById as ModeAccumulativeQuest).Do(delegate(ModeAccumulativeQuest quest)
				{
					quest.IncrementIf(e.Mode == quest.Mode);
				});
			}
			questById = GetQuestById("killWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(delegate(WeaponSlotAccumulativeQuest quest)
				{
					quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot);
				});
			}
			Dictionary<string, SimpleAccumulativeQuest> dictionary = new string[3] { "killViaHeadshot", "killWithGrenade", "revenge" }.Select(GetQuestById).OfType<SimpleAccumulativeQuest>().ToDictionary((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			SimpleAccumulativeQuest value;
			if (dictionary.TryGetValue("killViaHeadshot", out value))
			{
				value.IncrementIf(e.Headshot);
			}
			if (dictionary.TryGetValue("killWithGrenade", out value))
			{
				value.IncrementIf(e.Grenade);
			}
			if (dictionary.TryGetValue("revenge", out value))
			{
				value.IncrementIf(e.Revenge);
			}
		}

		private void HandleKillOtherPlayerWithFlag(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayerWithFlag(): " + e);
			}
			QuestBase questById = GetQuestById("killFlagCarriers");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		private void HandleCapture(object sender, CaptureEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleCapture(): " + e);
			}
			Dictionary<string, SimpleAccumulativeQuest> dictionary = new string[2] { "captureFlags", "capturePoints" }.Select(GetQuestById).OfType<SimpleAccumulativeQuest>().ToDictionary((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			SimpleAccumulativeQuest value;
			if (dictionary.TryGetValue("capturePoints", out value))
			{
				value.IncrementIf(e.Mode == GameConnect.GameMode.CapturePoints);
			}
			if (dictionary.TryGetValue("captureFlags", out value))
			{
				value.IncrementIf(e.Mode == GameConnect.GameMode.FlagCapture);
			}
		}

		private void HandleKillMonster(object sender, KillMonsterEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillMonster(): " + e);
			}
			QuestBase questById = GetQuestById("killInCampaign");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.IncrementIf(e.Campaign);
				});
			}
			questById = GetQuestById("killNpcWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(delegate(WeaponSlotAccumulativeQuest quest)
				{
					quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot);
				});
			}
		}

		private void HandleBreakSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleBreakSeries()");
			}
			QuestBase questById = GetQuestById("breakSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		private void HandleMakeSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleMakeSeries()");
			}
			QuestBase questById = GetQuestById("makeSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		private void HandleSurviveInArena(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleSurviveInArena()");
			}
			QuestBase questById = GetQuestById("surviveWavesInArena");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		private void HandleGetGotcha(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleGetGotcha()");
			}
			QuestBase tutorialQuestById = GetTutorialQuestById("getGotcha");
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		private void HandleSocialInteraction(object sender, SocialInteractionEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("HandleSocialInteraction('{0}')", e.Kind);
			}
			QuestBase tutorialQuestById = GetTutorialQuestById(e.Kind);
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment();
				});
			}
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				tutorialQuest.Changed -= OnQuestChangedCheckCompletion;
			}
			foreach (QuestBase item in _currentQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => kv.Value))
			{
				item.Changed -= OnQuestChangedCheckCompletion;
			}
			foreach (QuestBase item2 in _previousQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => kv.Value))
			{
				item2.Changed -= OnQuestChangedCheckCompletion;
			}
			_events.Win -= HandleWin;
			_events.KillOtherPlayer -= HandleKillOtherPlayer;
			_events.KillOtherPlayerWithFlag -= HandleKillOtherPlayerWithFlag;
			_events.Capture -= HandleCapture;
			_events.KillMonster -= HandleKillMonster;
			_events.BreakSeries -= HandleBreakSeries;
			_events.MakeSeries -= HandleMakeSeries;
			_events.SurviveWaveInArena -= HandleSurviveInArena;
			this.QuestCompleted = null;
			_disposed = true;
		}
	}
}
