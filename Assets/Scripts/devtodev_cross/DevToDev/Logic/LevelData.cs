using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;

namespace DevToDev.Logic
{
	[Serializable]
	internal class LevelData : ISaveable
	{
		private static readonly string EARNED = "earned";

		private static readonly string SPENT = "spent";

		private static readonly string BOUGHT = "bought";

		private static readonly string BALANCE = "balance";

		private int level = 1;

		private bool isNew;

		private long timestamp;

		private long localDuration;

		private long absoluteDuration;

		private Dictionary<string, int> balance;

		private Dictionary<string, int> spent;

		private Dictionary<string, int> earned;

		private Dictionary<string, int> bought;

		internal int Level
		{
			get
			{
				return level;
			}
			set
			{
				level = value;
			}
		}

		internal long TimeStamp
		{
			get
			{
				return timestamp;
			}
			set
			{
				timestamp = value;
			}
		}

		internal long LocalDuration
		{
			get
			{
				return localDuration;
			}
			set
			{
				localDuration = value;
			}
		}

		internal long AbsoluteDuration
		{
			get
			{
				return absoluteDuration;
			}
			set
			{
				absoluteDuration = value;
			}
		}

		public Dictionary<string, int> Balance
		{
			get
			{
				return balance;
			}
			set
			{
				if (value != null)
				{
					balance = value;
				}
			}
		}

		public bool IsNew
		{
			get
			{
				return isNew;
			}
			set
			{
				isNew = value;
				if (isNew)
				{
					timestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
				}
				else
				{
					timestamp = 0L;
				}
			}
		}

		public JSONClass DataToSend
		{
			get
			{
				JSONClass jSONClass = new JSONClass();
				if (timestamp > 0)
				{
					jSONClass.Add(Event.TIMESTAMP, new JSONData(timestamp));
				}
				if (earned.Count > 0)
				{
					JSONClass jSONClass2 = new JSONClass();
					jSONClass.Add(EARNED, jSONClass2);
					foreach (KeyValuePair<string, int> item in earned)
					{
						jSONClass2.Add(item.Key, new JSONData(item.Value));
					}
				}
				if (spent.Count > 0)
				{
					JSONClass jSONClass3 = new JSONClass();
					jSONClass.Add(SPENT, jSONClass3);
					foreach (KeyValuePair<string, int> item2 in spent)
					{
						jSONClass3.Add(item2.Key, new JSONData(item2.Value));
					}
				}
				if (bought.Count > 0)
				{
					JSONClass jSONClass4 = new JSONClass();
					jSONClass.Add(BOUGHT, jSONClass4);
					foreach (KeyValuePair<string, int> item3 in bought)
					{
						jSONClass4.Add(item3.Key, new JSONData(item3.Value));
					}
				}
				if (balance.Count > 0)
				{
					JSONClass jSONClass5 = new JSONClass();
					jSONClass.Add(BALANCE, jSONClass5);
					foreach (KeyValuePair<string, int> item4 in balance)
					{
						jSONClass5.Add(item4.Key, new JSONData(item4.Value));
					}
				}
				if (jSONClass.Count == 0)
				{
					return null;
				}
				return jSONClass;
			}
		}

		public LevelData()
		{
			balance = new Dictionary<string, int>();
			spent = new Dictionary<string, int>();
			earned = new Dictionary<string, int>();
			bought = new Dictionary<string, int>();
		}

		public LevelData(ObjectInfo info)
		{
			try
			{
				balance = info.GetValue("balance", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				spent = info.GetValue("spent", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				earned = info.GetValue("earned", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				bought = info.GetValue("bought", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				level = (int)info.GetValue("level", typeof(int));
				timestamp = (long)info.GetValue("timestamp", typeof(long));
				localDuration = (long)info.GetValue("localDuration", typeof(long));
				absoluteDuration = (long)info.GetValue("absoluteDuration", typeof(long));
				isNew = (bool)info.GetValue("isNew", typeof(bool));
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
				info.AddValue("balance", balance);
				info.AddValue("spent", spent);
				info.AddValue("earned", earned);
				info.AddValue("bought", bought);
				info.AddValue("level", level);
				info.AddValue("timestamp", timestamp);
				info.AddValue("localDuration", localDuration);
				info.AddValue("absoluteDuration", absoluteDuration);
				info.AddValue("isNew", isNew);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public LevelData(int level, bool isNew)
			: this()
		{
			this.level = level;
			IsNew = isNew;
		}

		public void upSend(string key, int value)
		{
			if (spent.ContainsKey(key))
			{
				int num = spent[key];
				spent[key] = num + value;
			}
			else
			{
				spent.Add(key, value);
			}
		}

		public void upEarned(string key, int value)
		{
			if (earned.ContainsKey(key))
			{
				int num = earned[key];
				earned[key] = num + value;
			}
			else
			{
				earned.Add(key, value);
			}
		}

		public void upBought(string key, int value)
		{
			if (bought.ContainsKey(key))
			{
				int num = bought[key];
				bought[key] = num + value;
			}
			else
			{
				bought.Add(key, value);
			}
		}
	}
}
