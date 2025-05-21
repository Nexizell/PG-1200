using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class InGameTimeKeeper : MonoBehaviour
	{
		private const string DailyInGameTimeKey = "DailyInGameTime";

		private static InGameTimeKeeper s_instance;

		private double _accumulatedInGameTimeSeconds;

		private DateTime _start;

		internal static InGameTimeKeeper Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = CoroutineRunner.Instance.GetOrAddComponent<InGameTimeKeeper>();
				}
				return s_instance;
			}
		}

		internal TimeSpan CurrentInGameTime
		{
			get
			{
				return GetInGameTime(DateTime.UtcNow);
			}
		}

		internal void Initialize()
		{
		}

		internal void Save()
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan inGameTime = GetInGameTime(utcNow);
			string value = Json.Serialize(new Dictionary<string, double> { 
			{
				utcNow.ToString("yyyy-MM-dd"),
				inGameTime.TotalSeconds
			} });
			PlayerPrefs.SetString("DailyInGameTime", value);
		}

		internal TimeSpan GetInGameTime(DateTime dateTime)
		{
			if (!(_start.Date < dateTime.Date))
			{
				return dateTime - _start + TimeSpan.FromSeconds(_accumulatedInGameTimeSeconds);
			}
			return dateTime.TimeOfDay;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void Start()
		{
			DateTime dateTime = (_start = DateTime.UtcNow);
			Dictionary<string, object> dictionary = Json.Deserialize(PlayerPrefs.GetString("DailyInGameTime", string.Empty)) as Dictionary<string, object>;
			if (dictionary == null)
			{
				_accumulatedInGameTimeSeconds = 0.0;
				return;
			}
			string key = dateTime.ToString("yyyy-MM-dd");
			double value;
			if (dictionary.TryGetValue<double>(key, out value))
			{
				_accumulatedInGameTimeSeconds = value;
			}
			else
			{
				_accumulatedInGameTimeSeconds = 0.0;
			}
		}

		private void OnApplicationPause(bool pause)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (pause)
			{
				if (_start.Date < utcNow.Date)
				{
					_start = utcNow.Date;
					_accumulatedInGameTimeSeconds = 0.0;
				}
				double totalSeconds = (utcNow - _start).TotalSeconds;
				_accumulatedInGameTimeSeconds += totalSeconds;
				_start = utcNow;
			}
			else
			{
				_start = utcNow;
			}
		}
	}
}
