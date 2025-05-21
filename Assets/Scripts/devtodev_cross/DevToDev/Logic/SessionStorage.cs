using System;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Logic
{
	internal class SessionStorage : ISaveable
	{
		private long startTime;

		private int startLevel;

		private int endLevel;

		private long endTime;

		private bool isActive;

		private long timeDifference;

		public long StartTime
		{
			get
			{
				return startTime;
			}
			set
			{
				startTime = value;
			}
		}

		public int StartLevel
		{
			get
			{
				return startLevel;
			}
			set
			{
				startLevel = value;
			}
		}

		public int EndLevel
		{
			get
			{
				return endLevel;
			}
			set
			{
				endLevel = value;
			}
		}

		public long EndTime
		{
			get
			{
				return endTime;
			}
			set
			{
				endTime = value;
			}
		}

		public long TimeDifference
		{
			get
			{
				return timeDifference;
			}
			set
			{
				timeDifference = value;
			}
		}

		public bool IsActive
		{
			get
			{
				return isActive;
			}
			set
			{
				isActive = value;
			}
		}

		public SessionStorage()
		{
			StartTime = DeviceHelper.Instance.GetUnixTime() / 1000;
			isActive = false;
		}

		public SessionStorage(ObjectInfo info)
		{
			try
			{
				startTime = (long)info.GetValue("startTime", typeof(long));
				endTime = (long)info.GetValue("endTime", typeof(long));
				timeDifference = (long)info.GetValue("timeDifference", typeof(long));
				endLevel = (int)info.GetValue("endLevel", typeof(int));
				startLevel = (int)info.GetValue("startLevel", typeof(int));
				isActive = (bool)info.GetValue("isActive", typeof(bool));
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
				info.AddValue("startTime", startTime);
				info.AddValue("endTime", endTime);
				info.AddValue("timeDifference", timeDifference);
				info.AddValue("endLevel", endLevel);
				info.AddValue("startLevel", startLevel);
				info.AddValue("isActive", isActive);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}
}
