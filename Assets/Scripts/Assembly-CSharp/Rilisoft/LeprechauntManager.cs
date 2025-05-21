using UnityEngine;

namespace Rilisoft
{
	public class LeprechauntManager : Singleton<LeprechauntManager>
	{
		private readonly StoragerIntCachedProperty _comeTimeSeconds = new StoragerIntCachedProperty("leprechaunt_come_time");

		private readonly StoragerIntCachedProperty _lastDropTimeSeconds = new StoragerIntCachedProperty("leprechaunt_last_drop_time");

		private readonly StoragerIntCachedProperty _lifeTime = new StoragerIntCachedProperty("leprechaunt_lifeTime");

		private readonly StoragerIntCachedProperty _rewardCount = new StoragerIntCachedProperty("leprechaunt_rewardCount");

		private readonly StoragerStringCachedProperty _rewardCurrency = new StoragerStringCachedProperty("leprechaunt_rewardCurrency");

		private readonly StoragerIntCachedProperty _dropDelaySecs = new StoragerIntCachedProperty("leprechaunt_dropDelay");

		private readonly PrefsBoolCachedProperty _needReset = new PrefsBoolCachedProperty("leprechaunt_needReset");

		public int LifeTimeSeconds
		{
			get
			{
				return _lifeTime.Value;
			}
			private set
			{
				_lifeTime.Value = value;
			}
		}

		public int RewardCount
		{
			get
			{
				return _rewardCount.Value;
			}
			private set
			{
				_rewardCount.Value = value;
			}
		}

		public string RewardCurrency
		{
			get
			{
				return _rewardCurrency.Value;
			}
			private set
			{
				_rewardCurrency.Value = value;
			}
		}

		public int DropDelaySeconds
		{
			get
			{
				return _dropDelaySecs.Value;
			}
			private set
			{
				_dropDelaySecs.Value = value;
			}
		}

		public long? CurrentTime
		{
			get
			{
				if (FriendsController.ServerTime < 1)
				{
					return null;
				}
				return FriendsController.ServerTime;
			}
		}

		public bool LeprechauntExists
		{
			get
			{
				return _comeTimeSeconds.Value > 0;
			}
		}

		public int? LeprechauntEndTime
		{
			get
			{
				if (!LeprechauntExists)
				{
					return null;
				}
				return _comeTimeSeconds.Value + LifeTimeSeconds;
			}
		}

		public int? LeprechauntLifeTimeLeft
		{
			get
			{
				if (!CurrentTime.HasValue || !LeprechauntExists)
				{
					return null;
				}
				if (!(LeprechauntEndTime - (int)CurrentTime.Value > 0))
				{
					return 0;
				}
				return LeprechauntEndTime - (int)CurrentTime.Value;
			}
		}

		public bool LeprechauntTimeOff
		{
			get
			{
				return _comeTimeSeconds.Value + LifeTimeSeconds < CurrentTime.Value;
			}
		}

		public float? RewardDropSecsLeft
		{
			get
			{
				if (CurrentTime.HasValue)
				{
					return _lastDropTimeSeconds.Value + DropDelaySeconds - CurrentTime.Value;
				}
				return null;
			}
		}

		public bool RewardIsReadyToDrop
		{
			get
			{
				if (LeprechauntExists && RewardDropSecsLeft.HasValue)
				{
					return RewardDropSecsLeft <= 0f;
				}
				return false;
			}
		}

		public int RewardReadyToDrop
		{
			get
			{
				if (!CurrentTime.HasValue || !LeprechauntEndTime.HasValue)
				{
					return -1;
				}
				if (ElapsedDropIntervals <= 0)
				{
					return 0;
				}
				return RewardCount;
			}
		}

		private int ElapsedDropIntervals
		{
			get
			{
				if (!CurrentTime.HasValue || !LeprechauntEndTime.HasValue)
				{
					return -1;
				}
				if (LeprechauntEndTime.Value < CurrentTime.Value)
				{
					return 1;
				}
				return Mathf.CeilToInt((CurrentTime.Value - _lastDropTimeSeconds.Value) / DropDelaySeconds);
			}
		}

		private void Update()
		{
			if (_needReset.Value)
			{
				Reset();
			}
		}

		public void SetLeprechaunt(int liveTimeSeconds, string rewardCurrency, int rewardCount, int rewardDropDelaySeconds = 86400)
		{
			Debug.Log(">>> L: set");
			if (LeprechauntExists)
			{
				LifeTimeSeconds = liveTimeSeconds + LeprechauntLifeTimeLeft.Value;
			}
			else
			{
				LifeTimeSeconds = liveTimeSeconds;
			}
			RewardCurrency = rewardCurrency;
			RewardCount = rewardCount;
			DropDelaySeconds = rewardDropDelaySeconds;
			Reset();
		}

		private void Reset()
		{
			if (!CurrentTime.HasValue)
			{
				_needReset.Value = true;
				return;
			}
			_needReset.Value = false;
			Debug.Log(">>> L: reset");
			_comeTimeSeconds.Value = (int)(CurrentTime - DropDelaySeconds).Value;
			_lastDropTimeSeconds.Value = (int)(CurrentTime.Value - DropDelaySeconds);
			Debug.Log(">>> L: reset to: LifeTimeSeconds: " + LifeTimeSeconds + " RewardCurrency: " + RewardCurrency + " RewardCount " + RewardCount + " DropDelaySeconds " + DropDelaySeconds);
		}

		public void RemoveLeprechaunt()
		{
			Debug.Log("L: remove");
			if (CurrentTime.HasValue && LeprechauntExists)
			{
				_comeTimeSeconds.Value = -1;
			}
		}

		public void DropReward()
		{
			if (CurrentTime.HasValue && LeprechauntEndTime.HasValue && RewardIsReadyToDrop)
			{
				if (RewardCurrency == "GemsCurrency")
				{
					BankController.AddGems(RewardReadyToDrop);
				}
				else
				{
					BankController.AddCoins(RewardReadyToDrop);
				}
				if (!LeprechauntTimeOff)
				{
					_lastDropTimeSeconds.Value = (int)CurrentTime.Value;
				}
				else
				{
					RemoveLeprechaunt();
				}
			}
		}
	}
}
