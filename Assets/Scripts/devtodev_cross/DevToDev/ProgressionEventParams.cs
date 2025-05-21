using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public class ProgressionEventParams : ISaveable
	{
		protected static readonly string SPENT_KEY = "spent";

		protected static readonly string EARNED_KEY = "earned";

		protected static readonly string PARAMS_KEY = "params";

		protected static readonly string SUCCESS_KEY = "success";

		internal string EventName { get; set; }

		internal Dictionary<string, int> Spent { get; set; }

		internal Dictionary<string, int> Earned { get; set; }

		internal long EventStart { get; set; }

		internal long EventFinish { get; set; }

		internal bool? IsSuccessful { get; set; }

		public ProgressionEventParams()
		{
		}

		public ProgressionEventParams(ObjectInfo info)
		{
			try
			{
				EventName = info.GetValue("EventName", typeof(string)) as string;
				Spent = info.GetValue("Spent", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				Earned = info.GetValue("Earned", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				EventStart = (long)info.GetValue("EventStart", typeof(long));
				EventFinish = (long)info.GetValue("EventFinish", typeof(long));
				IsSuccessful = info.GetValue("IsSuccessful", typeof(bool?)) as bool?;
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
				info.AddValue("EventName", EventName);
				info.AddValue("Spent", Spent);
				info.AddValue("Earned", Earned);
				info.AddValue("EventStart", EventStart);
				info.AddValue("EventFinish", EventFinish);
				info.AddValue("IsSuccessful", IsSuccessful);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		internal ProgressionEventParams(ProgressionEventParams peparams)
		{
			Spent = peparams.Spent;
			Earned = peparams.Earned;
			IsSuccessful = peparams.IsSuccessful;
		}

		internal void SetStartTime(long time)
		{
			EventStart = time;
		}

		internal bool IsCorrect()
		{
			if (EventFinish != 0)
			{
				return EventStart != 0;
			}
			return false;
		}

		internal long GetFinishTime()
		{
			return EventFinish;
		}

		internal void SetFinishTime(long finishTime)
		{
			EventFinish = finishTime;
		}

		internal virtual void Merge(ProgressionEventParams value)
		{
			EventFinish = value.EventFinish;
			if (value.Earned != null)
			{
				Earned = value.Earned;
			}
			if (value.Spent != null)
			{
				Spent = value.Spent;
			}
			if (value.IsSuccessful.HasValue)
			{
				IsSuccessful = value.IsSuccessful;
			}
		}

		public void SetSuccessfulCompletion(bool success)
		{
			IsSuccessful = success;
		}

		internal void SetEventName(string eventName)
		{
			EventName = eventName;
		}

		internal string GetEventName()
		{
			return EventName;
		}

		public void SetEarned(Dictionary<string, int> earned)
		{
			if (earned != null)
			{
				Earned = earned;
			}
		}

		public void SetSpent(Dictionary<string, int> spent)
		{
			if (spent != null)
			{
				Spent = spent;
			}
		}

		internal virtual JSONNode ToJson()
		{
			JSONNode jSONNode = new JSONClass();
			if (Spent != null && Spent.Count > 0)
			{
				JSONNode jSONNode2 = new JSONClass();
				foreach (KeyValuePair<string, int> item in Spent)
				{
					jSONNode2.Add(Uri.EscapeDataString(item.Key), new JSONData(item.Value));
				}
				jSONNode.Add(SPENT_KEY, jSONNode2);
			}
			if (Earned != null && Earned.Count > 0)
			{
				JSONNode jSONNode3 = new JSONClass();
				foreach (KeyValuePair<string, int> item2 in Earned)
				{
					jSONNode3.Add(Uri.EscapeDataString(item2.Key), new JSONData(item2.Value));
				}
				jSONNode.Add(EARNED_KEY, jSONNode3);
			}
			JSONNode jSONNode4 = new JSONClass();
			if (IsSuccessful.HasValue)
			{
				jSONNode4.Add(SUCCESS_KEY, new JSONData(IsSuccessful));
			}
			else
			{
				jSONNode4.Add(SUCCESS_KEY, new JSONData(false));
			}
			jSONNode.Add(PARAMS_KEY, jSONNode4);
			return jSONNode;
		}

		internal virtual ProgressionEventParams Clone()
		{
			ProgressionEventParams progressionEventParams = new ProgressionEventParams();
			progressionEventParams.Earned = Earned;
			progressionEventParams.Spent = Spent;
			progressionEventParams.EventName = EventName;
			progressionEventParams.EventStart = EventStart;
			progressionEventParams.EventFinish = EventFinish;
			progressionEventParams.IsSuccessful = IsSuccessful;
			return progressionEventParams;
		}
	}
}
