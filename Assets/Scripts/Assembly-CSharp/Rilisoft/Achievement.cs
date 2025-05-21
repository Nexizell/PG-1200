using System;
using System.Linq;

namespace Rilisoft
{
	public abstract class Achievement : IDisposable
	{
		private readonly AchievementProgress _progress;

		protected AchievementData _data { get; private set; }

		public AchievementData Data
		{
			get
			{
				return _data;
			}
		}

		protected AchievementProgress Progress
		{
			get
			{
				return _progress;
			}
		}

		public int Points
		{
			get
			{
				return Progress.Points;
			}
		}

		public int Stage
		{
			get
			{
				return Progress.Stage;
			}
		}

		public AchievementType Type
		{
			get
			{
				return _data.Type;
			}
		}

		public bool IsActive
		{
			get
			{
				if (this.ActiveCheckers == null)
				{
					return true;
				}
				Delegate[] invocationList = this.ActiveCheckers.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (!((Func<bool>)invocationList[i])())
					{
						return false;
					}
				}
				return true;
			}
		}

		public int Id
		{
			get
			{
				return _data.Id;
			}
		}

		public int MaxStage
		{
			get
			{
				return _data.Thresholds.Length;
			}
		}

		public int ToNextStagePointsLeft
		{
			get
			{
				int toNextStagePointsTotal = ToNextStagePointsTotal;
				if (toNextStagePointsTotal <= 0)
				{
					return -1;
				}
				return toNextStagePointsTotal - _progress.Points;
			}
		}

		public int ToNextStagePointsTotal
		{
			get
			{
				if (_data.Thresholds.Length <= _progress.Stage)
				{
					return -1;
				}
				return _data.Thresholds[_progress.Stage];
			}
		}

		public int ToNextStagePoints
		{
			get
			{
				if (_data.Thresholds.Length > _progress.Stage)
				{
					if (_progress.Stage <= 0)
					{
						return _data.Thresholds[0];
					}
					return _data.Thresholds[_progress.Stage] - _data.Thresholds[_progress.Stage - 1];
				}
				return -1;
			}
		}

		public int PointsLeft
		{
			get
			{
				return _data.Thresholds.Sum() - _progress.Points;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return Progress.Points >= PointsLeft;
			}
		}

		public int CurrentStage
		{
			get
			{
				return _progress.Stage;
			}
		}

		public AchievementProgressData ProgressData
		{
			get
			{
				return _progress.Data;
			}
			set
			{
				int points = _progress.Points;
				int stage = _progress.Stage;
				_progress.Data = value;
				_progress.Data.AchievementId = _data.Id;
				if ((_progress.Points != points || _progress.Stage != stage) && this.OnProgressChanged != null)
				{
					this.OnProgressChanged(_progress.Points != points, _progress.Stage != stage);
				}
			}
		}

		protected event Func<bool> ActiveCheckers;

		public event Action<bool, bool> OnProgressChanged;

		protected static bool CheckTrainigPolygonDisabled()
		{
			return true;
		}

		public Achievement(AchievementData data, AchievementProgressData progressData)
		{
			_data = data;
			_progress = new AchievementProgress(progressData);
			ActiveCheckers += CheckTrainigPolygonDisabled;
		}

		public int PointsToStage(int stageIdx)
		{
			if (_data.Thresholds.Length <= stageIdx)
			{
				return -1;
			}
			return _data.Thresholds[stageIdx] - Progress.Points;
		}

		public int MaxStageForPoints(int pointsCount)
		{
			int result = -1;
			for (int i = 0; i < _data.Thresholds.Length && _data.Thresholds[i] <= pointsCount; i++)
			{
				result = i;
			}
			return result;
		}

		protected void Gain(int increment = 1)
		{
			if (increment != 0 && IsActive)
			{
				_progress.IncrementPoints(increment);
				int stage = Stage;
				IncrementStageIfNeeded();
				if (this.OnProgressChanged != null)
				{
					this.OnProgressChanged(true, stage != Stage);
				}
			}
		}

		protected void SetProgress(int totalPoints)
		{
			if (totalPoints >= 0 && totalPoints != _progress.Points)
			{
				_progress.IncrementPoints(_progress.Points * -1);
				Gain(totalPoints);
			}
		}

		private void IncrementStageIfNeeded()
		{
			if (_progress.Stage < _data.Thresholds.Length && _data.Thresholds[_progress.Stage] <= _progress.Points)
			{
				_progress.IncrementStage();
				IncrementStageIfNeeded();
			}
		}

		public virtual void Dispose()
		{
		}

		public void Log(string msg)
		{
		}

		public static void LogMsg(string msg)
		{
		}
	}
}
