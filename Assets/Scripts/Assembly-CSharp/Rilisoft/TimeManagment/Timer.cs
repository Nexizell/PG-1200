using System;
using System.Collections.Generic;

namespace Rilisoft.TimeManagment
{
	public class Timer : IDisposable
	{
		private static Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();

		public Action<int> OnTick;

		protected string _id;

		protected float _duration;

		protected float _leftTime;

		protected float _speedMultiplier = 1f;

		protected long _lastUpdateTime;

		public bool IsStarted { get; protected set; }

		public string Id
		{
			get
			{
				return Id;
			}
		}

		public float Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				if (value >= 0f)
				{
					_duration = value;
				}
			}
		}

		public float TimeLeft
		{
			get
			{
				return _leftTime;
			}
		}

		public float SpeedMultiplier
		{
			get
			{
				return _speedMultiplier;
			}
			set
			{
				if (value >= 0f)
				{
					_speedMultiplier = value;
				}
			}
		}

		public float TimeLeftWithSpeedMultiplier
		{
			get
			{
				return TimeLeft / _speedMultiplier;
			}
		}

		public float TimeElapsedWithSpeedMultiplier
		{
			get
			{
				return Duration * SpeedMultiplier - TimeLeftWithSpeedMultiplier;
			}
		}

		public virtual long? CurrentTime
		{
			get
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
				return (long)(DateTime.UtcNow - dateTime).TotalSeconds;
			}
		}

		public static bool TimerExists(string id)
		{
			return _timers.ContainsKey(id);
		}

		public static T GetTimer<T>(string id) where T : Timer
		{
			if (!_timers.ContainsKey(id))
			{
				return null;
			}
			return _timers[id] as T;
		}

		public static void DestroyTimer(string id)
		{
			if (_timers.ContainsKey(id))
			{
				_timers[id].Dispose();
				_timers.Remove(id);
			}
		}

		public Timer(string id, float duration, float speedMultiplier = 1f)
		{
			if (_timers.ContainsKey(id))
			{
				throw new ArgumentException(string.Format("timer with id '{0}' allready exists", new object[1] { id }));
			}
			_timers.Add(id, this);
			_id = id;
			_duration = duration;
			_speedMultiplier = speedMultiplier;
			_leftTime = duration;
		}

		public virtual void Start()
		{
			CoroutineRunner.OnEngineUpdate -= OnEngineUpdate;
			CoroutineRunner.OnEngineUpdate += OnEngineUpdate;
			IsStarted = true;
		}

		public virtual void Stop()
		{
			IsStarted = false;
		}

		public virtual void Restart()
		{
			Stop();
			Reset();
			Start();
		}

		public virtual void Reset()
		{
			if (!CurrentTime.HasValue)
			{
				throw new Exception("CurrentTime has not value");
			}
			_leftTime = Duration;
			_lastUpdateTime = CurrentTime.Value;
		}

		private void OnEngineUpdate()
		{
			if (IsStarted && CurrentTime.HasValue)
			{
				if (_lastUpdateTime < 1)
				{
					_lastUpdateTime = (CurrentTime.HasValue ? CurrentTime.Value : (-1));
				}
				if (_lastUpdateTime >= 1)
				{
					Tick();
				}
			}
		}

		protected virtual void Tick()
		{
			_leftTime -= (float)(CurrentTime.Value - _lastUpdateTime) * _speedMultiplier;
			_lastUpdateTime = CurrentTime.Value;
			if (_leftTime <= 0f)
			{
				_leftTime = Duration;
				if (OnTick != null)
				{
					OnTick(1);
				}
			}
		}

		public virtual void Dispose()
		{
			_timers.Remove(_id);
			CoroutineRunner.OnEngineUpdate -= OnEngineUpdate;
		}
	}
}
