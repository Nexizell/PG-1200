using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public abstract class AchievementBindedToMyPlayer : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CTick_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementBindedToMyPlayer _003C_003E4__this;

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
			public _003CTick_003Ed__4(int _003C_003E1__state)
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
				if (_003C_003E4__this.MyPlayer == null)
				{
					_003C_003E4__this._lastHash = 0;
				}
				if (_003C_003E4__this._lastHash == 0 && _003C_003E4__this.MyPlayer != null)
				{
					_003C_003E4__this._lastHash = _003C_003E4__this.MyPlayer.GetHashCode();
					_003C_003E4__this.OnPlayerInstanceSetted();
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

		private int _lastHash;

		protected Player_move_c MyPlayer
		{
			get
			{
				if (!(WeaponManager.sharedManager != null))
				{
					return null;
				}
				return WeaponManager.sharedManager.myPlayerMoveC;
			}
		}

		public AchievementBindedToMyPlayer(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(Tick());
		}

		private IEnumerator Tick()
		{
			while (true)
			{
				if (MyPlayer == null)
				{
					_lastHash = 0;
				}
				if (_lastHash == 0 && MyPlayer != null)
				{
					_lastHash = MyPlayer.GetHashCode();
					OnPlayerInstanceSetted();
				}
				yield return null;
			}
		}

		protected abstract void OnPlayerInstanceSetted();

		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(Tick());
		}
	}
}
