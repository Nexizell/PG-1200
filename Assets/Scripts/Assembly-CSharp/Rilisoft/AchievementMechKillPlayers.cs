using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public class AchievementMechKillPlayers : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CWaitMechOff_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementMechKillPlayers _003C_003E4__this;

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
			public _003CWaitMechOff_003Ed__4(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (WeaponManager.sharedManager != null && (bool)WeaponManager.sharedManager.myPlayerMoveC && WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this._meshStateListen = false;
				if (_003C_003E4__this._killedCounter >= _003C_003E4__this.ToNextStagePointsLeft)
				{
					int num = _003C_003E4__this.MaxStageForPoints(_003C_003E4__this._killedCounter);
					if (num > -1)
					{
						_003C_003E4__this.SetProgress(_003C_003E4__this.PointsToStage(num));
					}
				}
				_003C_003E4__this._killedCounter = 0;
				return false;
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

		private bool _meshStateListen;

		private int _killedCounter;

		public AchievementMechKillPlayers(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayer += QuestMediator_Events_KillOtherPlayer;
		}

		private void QuestMediator_Events_KillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				_killedCounter++;
				if (!_meshStateListen)
				{
					_meshStateListen = true;
					AchievementsManager.Awaiter.Register(WaitMechOff());
				}
			}
		}

		private IEnumerator WaitMechOff()
		{
			while (WeaponManager.sharedManager != null && (bool)WeaponManager.sharedManager.myPlayerMoveC && WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				yield return null;
			}
			_meshStateListen = false;
			if (_killedCounter >= ToNextStagePointsLeft)
			{
				int num = MaxStageForPoints(_killedCounter);
				if (num > -1)
				{
					SetProgress(PointsToStage(num));
				}
			}
			_killedCounter = 0;
		}

		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayer -= QuestMediator_Events_KillOtherPlayer;
		}
	}
}
