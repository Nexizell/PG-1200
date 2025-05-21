using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementJetPackFlying : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CCheck_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private float _003C_timeElapsed_003E5__1;

			public AchievementJetPackFlying _003C_003E4__this;

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
			public _003CCheck_003Ed__2(int _003C_003E1__state)
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
					_003C_timeElapsed_003E5__1 = 0f;
				}
				if (Defs.isJetpackEnabled && Defs.isJump)
				{
					_003C_timeElapsed_003E5__1 += Time.deltaTime;
				}
				else if (_003C_timeElapsed_003E5__1 > 0f)
				{
					float result = 0f;
					object obj = _003C_003E4__this.Progress.CustomDataGet("ftime");
					if (obj != null)
					{
						float.TryParse((string)obj, out result);
					}
					result += _003C_timeElapsed_003E5__1;
					_003C_003E4__this.Progress.CustomDataSet("ftime", result.ToString());
					int num2 = (int)(result / 60f);
					if (num2 != _003C_003E4__this.Progress.Points)
					{
						_003C_003E4__this.SetProgress(num2);
					}
					_003C_timeElapsed_003E5__1 = 0f;
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

		private const string DATA_KEY = "ftime";

		public AchievementJetPackFlying(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(Check());
		}

		private IEnumerator Check()
		{
			float _timeElapsed = 0f;
			while (true)
			{
				if (Defs.isJetpackEnabled && Defs.isJump)
				{
					_timeElapsed += Time.deltaTime;
				}
				else if (_timeElapsed > 0f)
				{
					float result = 0f;
					object obj = Progress.CustomDataGet("ftime");
					if (obj != null)
					{
						float.TryParse((string)obj, out result);
					}
					result += _timeElapsed;
					Progress.CustomDataSet("ftime", result.ToString());
					int num = (int)(result / 60f);
					if (num != Progress.Points)
					{
						SetProgress(num);
					}
					_timeElapsed = 0f;
				}
				yield return null;
			}
		}

		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(Check());
		}
	}
}
