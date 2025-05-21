using UnityEngine;

namespace Rilisoft.TimeManagment
{
	public class StoragedTimer : Timer
	{
		protected long _lastTickTime;

		private long _lastSaveTime;

		private bool _waitServerTimeAndInit;

		private bool startAfterInit;

		protected string StorageKey
		{
			get
			{
				return string.Format("timer_{0}", new object[1] { _id });
			}
		}

		public StoragedTimer(string id, float duration, float speedMultiplier = 1f)
			: base(id, duration, speedMultiplier)
		{
			if (CurrentTime.HasValue)
			{
				Init();
				return;
			}
			CoroutineRunner.OnEngineUpdate += EngineUpdate;
			_waitServerTimeAndInit = true;
		}

		private void EngineUpdate()
		{
			if (_waitServerTimeAndInit && CurrentTime.HasValue)
			{
				CoroutineRunner.OnEngineUpdate -= EngineUpdate;
				_waitServerTimeAndInit = false;
				Init();
			}
		}

		private void Init()
		{
			int num = LoadTime();
			if (num > 0)
			{
				_lastTickTime = num;
				float num2 = (float)(CurrentTime.Value - num) * _speedMultiplier;
				float num3 = _duration * _speedMultiplier;
				_leftTime = num3 - num2;
			}
			else
			{
				_lastTickTime = CurrentTime.Value;
				SaveTime();
				_leftTime = _duration;
			}
			_lastSaveTime = CurrentTime.Value;
			if (startAfterInit)
			{
				startAfterInit = false;
				Start();
			}
		}

		public override void Start()
		{
			if (_waitServerTimeAndInit)
			{
				startAfterInit = true;
			}
			else
			{
				base.Start();
			}
		}

		public override void Stop()
		{
			if (_waitServerTimeAndInit)
			{
				startAfterInit = false;
			}
			else
			{
				base.Stop();
			}
		}

		public override void Reset()
		{
			base.Reset();
			_leftTime = base.Duration;
			_lastTickTime = CurrentTime.Value;
			SaveTime();
		}

		protected override void Tick()
		{
			if (_leftTime > 0f)
			{
				_leftTime -= (float)(CurrentTime.Value - _lastUpdateTime) * _speedMultiplier;
			}
			else
			{
				int obj = Mathf.Max(Mathf.FloorToInt(Mathf.Abs(_leftTime) / base.Duration), 1);
				_leftTime = base.Duration;
				_lastTickTime = CurrentTime.Value;
				if (OnTick != null)
				{
					OnTick(obj);
				}
			}
			_lastUpdateTime = CurrentTime.Value;
		}

		protected virtual void SaveTime()
		{
			if (CurrentTime.HasValue)
			{
				int val = (int)_lastTickTime;
				Storager.setInt(StorageKey, val);
			}
		}

		protected int LoadTime()
		{
			return Storager.getInt(StorageKey);
		}
	}
}
