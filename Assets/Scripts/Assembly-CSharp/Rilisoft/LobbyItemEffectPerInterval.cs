using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public abstract class LobbyItemEffectPerInterval : LobbyItemEffectHandler
	{
		[CompilerGenerated]
		internal sealed class _003CTickCoroutine_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyItemEffectPerInterval _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CTickCoroutine_003Ed__7(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -1;
				}
				else
				{
					_003C_003E1__state = -1;
				}
				if (_003C_003E4__this.LobbyItem != null)
				{
					if (_003C_003E4__this.LobbyItem.PlayerInfo.IsEquiped)
					{
						if (FriendsController.ServerTime > 0)
						{
							if (_003C_003E4__this.PlaceTime.Value < 1)
							{
								_003C_003E4__this.PlaceTime.Value = (int)FriendsController.ServerTime;
							}
							_003C_003E4__this.TimeLeft = _003C_003E4__this.PlaceTime.Value + _003C_003E4__this.RewardInterval - FriendsController.ServerTime;
							if (_003C_003E4__this.PlaceTime.Value + _003C_003E4__this.RewardInterval < FriendsController.ServerTime)
							{
								int dropIntervalsElapsed = Mathf.FloorToInt((FriendsController.ServerTime - _003C_003E4__this.PlaceTime.Value) / _003C_003E4__this.RewardInterval);
								if (_003C_003E4__this.GiveReward(dropIntervalsElapsed))
								{
									long num2 = (FriendsController.ServerTime - _003C_003E4__this.PlaceTime.Value) % _003C_003E4__this.RewardInterval;
									_003C_003E4__this.PlaceTime.Value = (int)(FriendsController.ServerTime - num2);
								}
							}
						}
					}
					else if (_003C_003E4__this.PlaceTime.Value > 0)
					{
						_003C_003E4__this.PlaceTime.Value = -1;
					}
				}
				_003C_003E2__current = new WaitForRealSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		protected StoragerIntCachedProperty PlaceTime;

		[ReadOnly]
		[SerializeField]
		protected float TimeLeft;

		protected abstract long RewardInterval { get; }

		protected override void Setup()
		{
			PlaceTime = new StoragerIntCachedProperty(base.StorragerKey);
			if (PlaceTime.Value == 0)
			{
				PlaceTime.Value = (int)(FriendsController.ServerTime - RewardInterval);
			}
		}

		public override void Remove()
		{
			PlaceTime.Value = -1;
			UnityEngine.Object.Destroy(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			StartCoroutine(TickCoroutine());
		}

		private IEnumerator TickCoroutine()
		{
			while (true)
			{
				if (LobbyItem != null)
				{
					if (LobbyItem.PlayerInfo.IsEquiped)
					{
						if (FriendsController.ServerTime > 0)
						{
							if (PlaceTime.Value < 1)
							{
								PlaceTime.Value = (int)FriendsController.ServerTime;
							}
							TimeLeft = PlaceTime.Value + RewardInterval - FriendsController.ServerTime;
							if (PlaceTime.Value + RewardInterval < FriendsController.ServerTime)
							{
								int dropIntervalsElapsed = Mathf.FloorToInt((FriendsController.ServerTime - PlaceTime.Value) / RewardInterval);
								if (GiveReward(dropIntervalsElapsed))
								{
									long num = (FriendsController.ServerTime - PlaceTime.Value) % RewardInterval;
									PlaceTime.Value = (int)(FriendsController.ServerTime - num);
								}
							}
						}
					}
					else if (PlaceTime.Value > 0)
					{
						PlaceTime.Value = -1;
					}
				}
				yield return new WaitForRealSeconds(1f);
			}
		}

		protected abstract bool GiveReward(int dropIntervalsElapsed);

		public LobbyItemEffectPerInterval()
		{
		}
	}
}
