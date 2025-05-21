using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Specific;

namespace DevToDev.Logic
{
	internal class MetricsStorage : ISaveable
	{
		private Dictionary<string, UserMetrics> userMetrics;

		public int Size
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
				{
					num += userMetric.Value.Size;
				}
				return num;
			}
		}

		public MetricsStorage()
		{
			userMetrics = new Dictionary<string, UserMetrics>();
		}

		public MetricsStorage(ObjectInfo info)
		{
			try
			{
				userMetrics = info.GetValue("userMetrics", typeof(Dictionary<string, UserMetrics>)) as Dictionary<string, UserMetrics>;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("userMetrics", userMetrics);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public MetricsStorage(int level)
			: this()
		{
			setLevel(level, null, false);
		}

		public MetricsStorage(int level, MetricsStorage oldData)
			: this(level)
		{
			if (oldData == null)
			{
				return;
			}
			ProgressionEvent progressionEvent = null;
			using (Dictionary<string, UserMetrics>.Enumerator enumerator = oldData.userMetrics.GetEnumerator())
			{
				while (enumerator.MoveNext() && (progressionEvent = enumerator.Current.Value.GetProgressionEvent()) == null)
				{
				}
			}
			if (progressionEvent != null)
			{
				progressionEvent = new ProgressionEvent(progressionEvent);
				progressionEvent.RemoveSentMetrics();
			}
			foreach (KeyValuePair<string, UserMetrics> userMetric in oldData.userMetrics)
			{
				if (userMetric.Value.LevelData.Level > level)
				{
					userMetric.Value.clearMetrics();
					if (userMetrics.ContainsKey(userMetric.Key))
					{
						userMetrics[userMetric.Key] = userMetric.Value;
					}
					else
					{
						userMetrics.Add(userMetric.Key, userMetric.Value);
					}
				}
			}
			if (progressionEvent != null)
			{
				AddMetric(level, progressionEvent);
			}
		}

		public void AddPeopleEvent(int level, PeopleEvent peopleEvent)
		{
			AddMetric(level, peopleEvent);
		}

		public void ClearUnfinishedProgressionEvent()
		{
			foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
			{
				userMetric.Value.ClearUnfinishedProgressionEvent();
			}
		}

		public PeopleEvent GetRemovePeopleEvent()
		{
			PeopleEvent peopleEvent = null;
			foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
			{
				peopleEvent = userMetric.Value.GetRemovePeopleEvent();
				if (peopleEvent != null)
				{
					return peopleEvent;
				}
			}
			return null;
		}

		public void AddMetric(int level, Event metric)
		{
			if (!(metric is ProgressionEvent))
			{
				Log.R("Metric {0} added to storage", metric.MetricName);
			}
			string key = level.ToString();
			if (!userMetrics.ContainsKey(key))
			{
				userMetrics.Add(key, new UserMetrics(level, false, null));
			}
			userMetrics[key].addMetric(metric);
		}

		public void RemoveAllMetricsByType(EventType type)
		{
			foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
			{
				userMetric.Value.RemoveAppMetricsByType(type);
			}
		}

		public bool IsMetricExist(string metricCode)
		{
			foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
			{
				if (userMetric.Value.isMetricExist(metricCode))
				{
					return true;
				}
			}
			return false;
		}

		public string PrepareToSend()
		{
			JSONClass jSONClass = new JSONClass();
			try
			{
				int num = 0;
				foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
				{
					num = Math.Max(num, int.Parse(userMetric.Key));
				}
				foreach (KeyValuePair<string, UserMetrics> userMetric2 in userMetrics)
				{
					JSONNode jSONNode = userMetric2.Value.DataToSend(num);
					if (!(jSONNode == null))
					{
						jSONClass.Add(userMetric2.Key, jSONNode);
					}
				}
			}
			catch (Exception ex)
			{
				Log.E(ex.Message + "\n" + ex.StackTrace);
			}
			if (jSONClass.Count == 0)
			{
				return null;
			}
			return jSONClass.ToJSON(0);
		}

		private ProgressionEvent getProgressionEventMetric()
		{
			ProgressionEvent result = null;
			using (Dictionary<string, UserMetrics>.Enumerator enumerator = userMetrics.GetEnumerator())
			{
				while (enumerator.MoveNext() && (result = enumerator.Current.Value.GetRemoveProgressionEvent()) == null)
				{
				}
			}
			return result;
		}

		public void setLevel(int level, Dictionary<string, int> resources, bool isNew)
		{
			string key = level.ToString();
			string key2 = (level + 1).ToString();
			ProgressionEvent progressionEventMetric = getProgressionEventMetric();
			if (!userMetrics.ContainsKey(key))
			{
				userMetrics.Add(key, new UserMetrics(level, isNew, resources));
			}
			else
			{
				userMetrics[key].LevelData.IsNew = isNew;
				userMetrics[key].LevelData.Balance = resources;
			}
			if (progressionEventMetric != null)
			{
				userMetrics[key].addMetric(progressionEventMetric);
			}
			if (!userMetrics.ContainsKey(key2))
			{
				userMetrics.Add(key2, new UserMetrics(level + 1, false, null));
			}
		}

		public void upSpend(int level, int purchasePrice, string purchaseCurrency)
		{
			string key = (level + 1).ToString();
			if (userMetrics.ContainsKey(key))
			{
				userMetrics[key].upSpend(purchaseCurrency, purchasePrice);
			}
		}

		public void upBought(int level, int purchasePrice, string purchaseCurrency)
		{
			string key = (level + 1).ToString();
			if (userMetrics.ContainsKey(key))
			{
				userMetrics[key].upBought(purchaseCurrency, purchasePrice);
			}
		}

		public void upEarned(int level, int purchasePrice, string purchaseCurrency)
		{
			string key = (level + 1).ToString();
			if (userMetrics.ContainsKey(key))
			{
				userMetrics[key].upEarned(purchaseCurrency, purchasePrice);
			}
		}

		public void ClearLevelData()
		{
			foreach (KeyValuePair<string, UserMetrics> userMetric in userMetrics)
			{
				userMetric.Value.ClearLevelData();
			}
		}
	}
}
