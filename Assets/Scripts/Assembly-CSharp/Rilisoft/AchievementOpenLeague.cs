using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementOpenLeague : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CWaitRatingSystem_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementOpenLeague _003C_003E4__this;

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
			public _003CWaitRatingSystem_003Ed__1(int _003C_003E1__state)
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
				if (RatingSystem.instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.OnRatingUpdated();
				RatingSystem.OnRatingUpdate += _003C_003E4__this.OnRatingUpdated;
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

		public AchievementOpenLeague(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			if (!base._data.League.HasValue)
			{
				UnityEngine.Debug.LogErrorFormat("achievement '{0}' without value", base._data.Id);
			}
			else
			{
				AchievementsManager.Awaiter.Register(WaitRatingSystem());
			}
		}

		private IEnumerator WaitRatingSystem()
		{
			while (RatingSystem.instance == null)
			{
				yield return null;
			}
			OnRatingUpdated();
			RatingSystem.OnRatingUpdate += OnRatingUpdated;
		}

		private void OnRatingUpdated()
		{
			if (base._data.League.Value == RatingSystem.instance.currentLeague && base.Progress.Points < base.PointsLeft)
			{
				Gain();
			}
		}

		public override void Dispose()
		{
			RatingSystem.OnRatingUpdate -= OnRatingUpdated;
			AchievementsManager.Awaiter.Remove(WaitRatingSystem());
		}
	}
}
