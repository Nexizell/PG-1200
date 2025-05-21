using System;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public class LocationEventParams : ProgressionEventParams
	{
		private static readonly string DIFFICULTY_KEY = "difficulty";

		private static readonly string SOURCE_KEY = "source";

		private static readonly string DURATION_KEY = "duration";

		internal int? Difficulty { get; set; }

		internal string SourceLocationId { get; set; }

		internal long? Duration { get; set; }

		public LocationEventParams()
		{
		}

		public LocationEventParams(ObjectInfo info)
			: base(info)
		{
			try
			{
				Difficulty = info.GetValue("Difficulty", typeof(int?)) as int?;
				SourceLocationId = info.GetValue("SourceLocationId", typeof(string)) as string;
				Duration = info.GetValue("Duration", typeof(int?)) as int?;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("Difficulty", Difficulty);
				info.AddValue("SourceLocationId", SourceLocationId);
				info.AddValue("Duration", Duration);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		internal LocationEventParams(LocationEventParams leparams)
		{
			base.Spent = leparams.Spent;
			base.Earned = leparams.Earned;
			Difficulty = leparams.Difficulty;
			Duration = leparams.Duration;
			base.IsSuccessful = leparams.IsSuccessful;
			SourceLocationId = leparams.SourceLocationId;
		}

		internal override void Merge(ProgressionEventParams value)
		{
			base.Merge(value);
			LocationEventParams locationEventParams = value as LocationEventParams;
			if (locationEventParams.Difficulty.HasValue)
			{
				Difficulty = locationEventParams.Difficulty;
			}
			if (locationEventParams.SourceLocationId != null)
			{
				SourceLocationId = locationEventParams.SourceLocationId;
			}
			if (locationEventParams.Duration.HasValue)
			{
				Duration = locationEventParams.Duration;
			}
		}

		public void SetDifficulty(int difficulty)
		{
			Difficulty = difficulty;
		}

		public void SetSource(string sourceLocationId)
		{
			SourceLocationId = sourceLocationId;
		}

		public void SetDuration(long duration)
		{
			Duration = duration;
		}

		internal override JSONNode ToJson()
		{
			JSONNode jSONNode = base.ToJson();
			JSONNode jSONNode2 = jSONNode[ProgressionEventParams.PARAMS_KEY];
			if (Difficulty.HasValue)
			{
				jSONNode2.Add(DIFFICULTY_KEY, new JSONData(Difficulty));
			}
			if (Duration.HasValue)
			{
				if (Duration > 0)
				{
					jSONNode2.Add(DURATION_KEY, new JSONData(Duration));
				}
			}
			else
			{
				long num = (base.EventFinish - base.EventStart) / 1000;
				if (num > 0)
				{
					jSONNode2.Add(DURATION_KEY, new JSONData(num));
				}
			}
			if (SourceLocationId != null)
			{
				jSONNode2.Add(SOURCE_KEY, new JSONData(SourceLocationId));
			}
			if (Difficulty.HasValue)
			{
				jSONNode2.Add(DIFFICULTY_KEY, new JSONData(Difficulty));
			}
			return jSONNode;
		}

		internal override ProgressionEventParams Clone()
		{
			LocationEventParams locationEventParams = new LocationEventParams();
			locationEventParams.Earned = base.Earned;
			locationEventParams.Spent = base.Spent;
			locationEventParams.EventName = base.EventName;
			locationEventParams.EventStart = base.EventStart;
			locationEventParams.EventFinish = base.EventFinish;
			locationEventParams.Difficulty = Difficulty;
			locationEventParams.SourceLocationId = SourceLocationId;
			locationEventParams.IsSuccessful = base.IsSuccessful;
			locationEventParams.Duration = Duration;
			return locationEventParams;
		}
	}
}
