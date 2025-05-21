using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Logic
{
	internal class DataStorage : ISaveable
	{
		private int level;

		private bool isCheater;

		private List<int> tutorialSteps;

		private long lastSendInfoTime;

		public int Level
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

		public bool IsCheater
		{
			get
			{
				return isCheater;
			}
			set
			{
				isCheater = value;
			}
		}

		public long LastSendInfoTime
		{
			get
			{
				return lastSendInfoTime;
			}
			set
			{
				lastSendInfoTime = value;
			}
		}

		public DataStorage()
		{
			level = 1;
			tutorialSteps = new List<int>();
		}

		public DataStorage(ObjectInfo info)
		{
			try
			{
				lastSendInfoTime = (long)info.GetValue("lastSendInfoTime", typeof(long));
				tutorialSteps = info.GetValue("tutorialSteps", typeof(List<int>)) as List<int>;
				level = (int)info.GetValue("level", typeof(int));
				isCheater = (bool)info.GetValue("isCheater", typeof(bool));
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
				info.AddValue("tutorialSteps", tutorialSteps);
				info.AddValue("lastSendInfoTime", lastSendInfoTime);
				info.AddValue("level", level);
				info.AddValue("isCheater", isCheater);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void AddTutorialStep(int tutorialStep)
		{
			tutorialSteps.Add(tutorialStep);
		}

		public bool ContainsTutorialStep(int tutorialStep)
		{
			return tutorialSteps.Contains(tutorialStep);
		}

		public void SetCurrentLevel(int level)
		{
			Level = level;
		}
	}
}
