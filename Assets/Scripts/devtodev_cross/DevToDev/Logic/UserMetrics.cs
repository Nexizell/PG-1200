using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Specific;

namespace DevToDev.Logic
{
	internal class UserMetrics : ISaveable
	{
		private static readonly string EVENTS = "events";

		private LevelData levelData;

		private Dictionary<string, List<Event>> simpleMetrics;

		private Dictionary<string, AggregatedEvent> aggregatedMetrics;

		public LevelData LevelData
		{
			get
			{
				return levelData;
			}
			internal set
			{
				if (value != null)
				{
					levelData = value;
				}
			}
		}

		public int Size
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<string, List<Event>> simpleMetric in simpleMetrics)
				{
					num += simpleMetric.Value.Count;
				}
				num += aggregatedMetrics.Count;
				return num + ((levelData != null) ? (levelData.IsNew ? 1 : 0) : 0);
			}
		}

		public UserMetrics()
		{
		}

		public UserMetrics(int level, bool isNew, Dictionary<string, int> resources)
		{
			levelData = new LevelData(level, isNew);
			levelData.Balance = resources;
			simpleMetrics = new Dictionary<string, List<Event>>();
			aggregatedMetrics = new Dictionary<string, AggregatedEvent>();
		}

		public UserMetrics(ObjectInfo info)
		{
			try
			{
				levelData = info.GetValue("levelData", typeof(LevelData)) as LevelData;
				simpleMetrics = info.GetValue("simpleMetrics", typeof(Dictionary<string, List<Event>>)) as Dictionary<string, List<Event>>;
				aggregatedMetrics = info.GetValue("aggregatedMetrics", typeof(Dictionary<string, AggregatedEvent>)) as Dictionary<string, AggregatedEvent>;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
			if (aggregatedMetrics == null)
			{
				aggregatedMetrics = new Dictionary<string, AggregatedEvent>();
			}
			if (simpleMetrics == null)
			{
				simpleMetrics = new Dictionary<string, List<Event>>();
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("levelData", levelData);
				info.AddValue("simpleMetrics", simpleMetrics);
				info.AddValue("aggregatedMetrics", aggregatedMetrics);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void ClearLevelData()
		{
			if (levelData != null)
			{
				levelData = new LevelData(levelData.Level, false);
			}
		}

		public bool isMetricExist(string metricCode)
		{
			foreach (KeyValuePair<string, List<Event>> simpleMetric in simpleMetrics)
			{
				if (simpleMetric.Key.ToUpper().Equals(metricCode.ToUpper()))
				{
					return true;
				}
			}
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				if (aggregatedMetric.Key.ToUpper().Equals(metricCode.ToUpper()))
				{
					return true;
				}
			}
			return false;
		}

		public void AddProgressionParams(Event metric)
		{
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				if (aggregatedMetric.Value is ProgressionEvent)
				{
					List<string> pendingEvents = (aggregatedMetric.Value as ProgressionEvent).GetPendingEvents();
					if (pendingEvents != null)
					{
						metric.AddPendingEvents(pendingEvents);
					}
					break;
				}
			}
		}

		internal void addMetric(Event metric)
		{
			AddProgressionParams(metric);
			if (metric is AggregatedEvent)
			{
				string metricCode = metric.MetricCode;
				AggregatedEvent aggregatedEvent = metric as AggregatedEvent;
				if (aggregatedMetrics.ContainsKey(metricCode))
				{
					aggregatedMetrics[metricCode].AddEvent(aggregatedEvent);
				}
				else
				{
					aggregatedMetrics.Add(metricCode, aggregatedEvent);
				}
			}
			else
			{
				if (!simpleMetrics.ContainsKey(metric.MetricCode))
				{
					simpleMetrics.Add(metric.MetricCode, new List<Event>());
				}
				simpleMetrics[metric.MetricCode].Add(metric);
			}
		}

		public void ClearUnfinishedProgressionEvent()
		{
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				if (aggregatedMetric.Value is ProgressionEvent)
				{
					ProgressionEvent progressionEvent = aggregatedMetric.Value as ProgressionEvent;
					progressionEvent.ClearUnfinishedEvents();
					break;
				}
			}
		}

		public ProgressionEvent GetProgressionEvent()
		{
			ProgressionEvent result = null;
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				if (aggregatedMetric.Value is ProgressionEvent && !aggregatedMetric.Value.IsNeedToClear())
				{
					result = aggregatedMetric.Value as ProgressionEvent;
				}
			}
			return result;
		}

		public ProgressionEvent GetRemoveProgressionEvent()
		{
			Dictionary<string, AggregatedEvent> dictionary = new Dictionary<string, AggregatedEvent>();
			ProgressionEvent result = null;
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				if (aggregatedMetric.Value is ProgressionEvent)
				{
					if (!aggregatedMetric.Value.IsNeedToClear())
					{
						result = aggregatedMetric.Value as ProgressionEvent;
					}
					else
					{
						dictionary.Add(aggregatedMetric.Key, aggregatedMetric.Value);
					}
				}
				else
				{
					dictionary.Add(aggregatedMetric.Key, aggregatedMetric.Value);
				}
			}
			aggregatedMetrics = dictionary;
			return result;
		}

		public void clearMetrics()
		{
			simpleMetrics.Clear();
			aggregatedMetrics.Clear();
		}

		public void upEarned(string purchaseCurrency, int purchasePrice)
		{
			levelData.upEarned(purchaseCurrency, purchasePrice);
		}

		public void upSpend(string purchaseCurrency, int purchasePrice)
		{
			levelData.upSend(purchaseCurrency, purchasePrice);
		}

		public void upBought(string purchaseCurrency, int purchasePrice)
		{
			levelData.upBought(purchaseCurrency, purchasePrice);
		}

		public JSONNode DataToSend(int maxLevel)
		{
			JSONClass jSONClass = new JSONClass();
			JSONClass jSONClass2 = levelData.DataToSend;
			if (levelData.Level >= maxLevel)
			{
				jSONClass2 = null;
			}
			if (aggregatedMetrics.Count == 0 && simpleMetrics.Count == 0 && jSONClass2 == null)
			{
				return null;
			}
			JSONClass jSONClass3 = new JSONClass();
			if (aggregatedMetrics.Count != 0 || simpleMetrics.Count != 0)
			{
				jSONClass.Add(EVENTS, jSONClass3);
			}
			foreach (KeyValuePair<string, List<Event>> simpleMetric in simpleMetrics)
			{
				JSONArray jSONArray = new JSONArray();
				jSONClass3.Add(simpleMetric.Key, jSONArray);
				foreach (Event item in simpleMetric.Value)
				{
					jSONArray.Add(item.GetAdditionalDataJson());
				}
			}
			foreach (KeyValuePair<string, AggregatedEvent> aggregatedMetric in aggregatedMetrics)
			{
				JSONNode additionalDataJson = aggregatedMetric.Value.GetAdditionalDataJson();
				if (additionalDataJson != null)
				{
					jSONClass3.Add(aggregatedMetric.Key, additionalDataJson);
				}
			}
			if (jSONClass2 != null)
			{
				IEnumerator enumerator4 = jSONClass2.GetEnumerator();
				while (enumerator4.MoveNext() && enumerator4.Current != null)
				{
					KeyValuePair<string, JSONNode> keyValuePair = (KeyValuePair<string, JSONNode>)enumerator4.Current;
					jSONClass.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			if (jSONClass3.Count == 0)
			{
				jSONClass.Remove(EVENTS);
			}
			if (jSONClass3.Count == 0 && jSONClass2 == null)
			{
				return null;
			}
			return jSONClass;
		}

		public void RemoveAppMetricsByType(EventType type)
		{
			string value = EventConst.EventsInfo[type].Value;
			if (simpleMetrics != null && simpleMetrics.ContainsKey(value))
			{
				simpleMetrics.Remove(value);
			}
			if (aggregatedMetrics != null && aggregatedMetrics.ContainsKey(value))
			{
				aggregatedMetrics.Remove(value);
			}
		}

		public PeopleEvent GetRemovePeopleEvent()
		{
			string value = EventConst.EventsInfo[EventType.UserCard].Value;
			if (simpleMetrics != null && simpleMetrics.ContainsKey(value))
			{
				List<Event> list = simpleMetrics[value];
				simpleMetrics.Remove(value);
				if (list.Count > 0)
				{
					return list[0] as PeopleEvent;
				}
			}
			return null;
		}
	}
}
