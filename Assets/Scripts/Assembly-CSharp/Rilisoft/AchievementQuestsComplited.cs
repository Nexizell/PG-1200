using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public class AchievementQuestsComplited : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CWaitQuestSystem_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementQuestsComplited _003C_003E4__this;

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
			public _003CWaitQuestSystem_003Ed__1(int _003C_003E1__state)
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
				if (QuestSystem.Instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				QuestSystem.Instance.QuestCompleted += _003C_003E4__this.QuestSystem_Instance_QuestCompleted;
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

		public AchievementQuestsComplited(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(WaitQuestSystem());
		}

		private IEnumerator WaitQuestSystem()
		{
			while (QuestSystem.Instance == null)
			{
				yield return null;
			}
			QuestSystem.Instance.QuestCompleted += QuestSystem_Instance_QuestCompleted;
		}

		private void QuestSystem_Instance_QuestCompleted(object sender, QuestCompletedEventArgs e)
		{
			Gain();
		}

		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(WaitQuestSystem());
			if (QuestSystem.Instance != null)
			{
				QuestSystem.Instance.QuestCompleted -= QuestSystem_Instance_QuestCompleted;
			}
		}
	}
}
