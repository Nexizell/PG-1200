using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementPacifist : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CUpdate_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementPacifist _003C_003E4__this;

			private Player_move_c _003CplayerMoveC_003E5__1;

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
			public _003CUpdate_003Ed__3(int _003C_003E1__state)
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
					if (_003C_003E4__this.IsCompleted)
					{
						return false;
					}
					_003CplayerMoveC_003E5__1 = WeaponManager.sharedManager.myPlayerMoveC;
				}
				if (_003C_003E4__this._failing)
				{
					return false;
				}
				if (_003CplayerMoveC_003E5__1 == null)
				{
					return false;
				}
				if (!_003C_003E4__this.PossibleGainInCurrentGameMode())
				{
					_003C_003E4__this._failing = true;
					return false;
				}
				if (GameConnect.isMiniGame && _003CplayerMoveC_003E5__1.liveTime >= (float)_003C_003E4__this.ToNextStagePointsLeft)
				{
					int num2 = _003C_003E4__this.MaxStageForPoints(Mathf.RoundToInt(_003CplayerMoveC_003E5__1.liveTime));
					if (num2 > -1)
					{
						_003C_003E4__this.SetProgress(_003C_003E4__this.PointsToStage(num2));
					}
					if (_003C_003E4__this.IsCompleted)
					{
						return false;
					}
				}
				_003C_003E2__current = null;
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

		private bool _failing;

		public AchievementPacifist(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			if (!base.IsCompleted)
			{
				Player_move_c.OnMyPlayerMoveCCreated += Player_move_c_OnMyPlayerMoveCCreated;
				Player_move_c.OnMyPlayerMoveCDestroyed += Player_move_c_OnMyPlayerMoveCDestroyed;
				Player_move_c.OnMyShootingStateSchanged += Player_move_c_OnMyShootingStateSchanged;
			}
		}

		private void Player_move_c_OnMyPlayerMoveCCreated()
		{
			if (!GameConnect.isHunger && !GameConnect.isDaterRegim && Defs.isMulti)
			{
				AchievementsManager.Awaiter.Register(Update());
			}
		}

		private IEnumerator Update()
		{
			if (IsCompleted)
			{
				yield break;
			}
			Player_move_c playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			while (!_failing && !(playerMoveC == null))
			{
				if (!PossibleGainInCurrentGameMode())
				{
					_failing = true;
					break;
				}
				if (GameConnect.isMiniGame && playerMoveC.liveTime >= (float)ToNextStagePointsLeft)
				{
					int num = MaxStageForPoints(Mathf.RoundToInt(playerMoveC.liveTime));
					if (num > -1)
					{
						SetProgress(PointsToStage(num));
					}
					if (IsCompleted)
					{
						break;
					}
				}
				yield return null;
			}
		}

		private bool PossibleGainInCurrentGameMode()
		{
			if (GameConnect.gameMode != GameConnect.GameMode.Campaign && GameConnect.gameMode != GameConnect.GameMode.CapturePoints && GameConnect.gameMode != 0 && GameConnect.gameMode != GameConnect.GameMode.Duel && GameConnect.gameMode != GameConnect.GameMode.FlagCapture && GameConnect.gameMode != GameConnect.GameMode.TeamFight)
			{
				return GameConnect.gameMode == GameConnect.GameMode.TimeBattle;
			}
			return true;
		}

		private void Player_move_c_OnMyPlayerMoveCDestroyed(float liveTime)
		{
			_failing = false;
		}

		private void Player_move_c_OnMyShootingStateSchanged(bool obj)
		{
			AchievementsManager.Awaiter.Remove(Update());
			_failing = obj;
		}

		public override void Dispose()
		{
			Player_move_c.OnMyPlayerMoveCCreated -= Player_move_c_OnMyPlayerMoveCCreated;
			Player_move_c.OnMyPlayerMoveCDestroyed -= Player_move_c_OnMyPlayerMoveCDestroyed;
			Player_move_c.OnMyShootingStateSchanged -= Player_move_c_OnMyShootingStateSchanged;
		}
	}
}
