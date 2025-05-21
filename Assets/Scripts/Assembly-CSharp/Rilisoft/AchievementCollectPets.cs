using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public class AchievementCollectPets : Achievement
	{
		[CompilerGenerated]
		internal sealed class _003CWaitPetsManager_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementCollectPets _003C_003E4__this;

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
			public _003CWaitPetsManager_003Ed__1(int _003C_003E1__state)
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
				if (Singleton<PetsManager>.Instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.UpdateProgress();
				Singleton<PetsManager>.Instance.OnPlayerPetAdded += _003C_003E4__this.PetsManager_Instance_OnPlayerPetAdded;
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

		public AchievementCollectPets(AchievementData data, AchievementProgressData progressData)
			: base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(WaitPetsManager());
		}

		private IEnumerator WaitPetsManager()
		{
			while (Singleton<PetsManager>.Instance == null)
			{
				yield return null;
			}
			UpdateProgress();
			Singleton<PetsManager>.Instance.OnPlayerPetAdded += PetsManager_Instance_OnPlayerPetAdded;
		}

		private void PetsManager_Instance_OnPlayerPetAdded(string petId)
		{
			UpdateProgress();
		}

		private void UpdateProgress()
		{
			if (!(Singleton<PetsManager>.Instance == null))
			{
				int num = Singleton<PetsManager>.Instance.PlayerPets.Count();
				if (base.Points < num)
				{
					SetProgress(num);
				}
			}
		}

		public override void Dispose()
		{
			if (Singleton<PetsManager>.Instance != null)
			{
				Singleton<PetsManager>.Instance.OnPlayerPetAdded -= PetsManager_Instance_OnPlayerPetAdded;
			}
		}
	}
}
