using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class PetsCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PetsCloudApplyer _003C_003E4__this;

			public bool skipApplyingToLocalState;

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
			public _003CApply_003Ed__1(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				if (_003C_003E1__state != 0)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (_003C_003E4__this.SlotSynchronizer == null)
				{
					UnityEngine.Debug.LogErrorFormat("PetsCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("PetsCloudApplyer currentPullResult == null");
				}
				try
				{
					PlayerPets playerPets = (currentResult.IsNullOrEmpty() ? new PlayerPets(false) : JsonUtility.FromJson<PlayerPets>(currentResult));
					PlayerPets playerPets2 = PetsManager.LoadPlayerPetsMemento();
					PlayerPets playerPets3 = PlayerPets.Merge(playerPets2, playerPets);
					bool flag = IsDirtyComparedToMergedMemento(playerPets2, playerPets3);
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("PetsCloudApplyer: Local progress is dirty: {0}", flag);
					}
					if (flag && !skipApplyingToLocalState)
					{
						PetsManager.OverwritePlayerPetsMemento(playerPets3);
						PetsManager.LoadPetsToMemory();
					}
					bool flag2 = IsDirtyComparedToMergedMemento(playerPets, playerPets3);
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("PetsCloudApplyer: Cloud progress is dirty: {0}", flag2);
					}
					if (flag2)
					{
						_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(playerPets3));
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("PetsCloudApplyer: Succeeded to apply pets:\n'currentPullResult':{0},\n'cloudPetsMemento':{1},\n'localPetsMemento':{2},\n'mergedMemento':{3},\n'localDirty':{4},\n'cloudDirty':{5}", currentResult, JsonUtility.ToJson(playerPets), JsonUtility.ToJson(playerPets2), JsonUtility.ToJson(playerPets3), flag, flag2);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in PetsCloudApplyer.Apply: {0}", ex);
				}
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

		public PetsCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("PetsCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("PetsCloudApplyer currentPullResult == null");
			}
			try
			{
				PlayerPets playerPets = (currentResult.IsNullOrEmpty() ? new PlayerPets(false) : JsonUtility.FromJson<PlayerPets>(currentResult));
				PlayerPets playerPets2 = PetsManager.LoadPlayerPetsMemento();
				PlayerPets playerPets3 = PlayerPets.Merge(playerPets2, playerPets);
				bool flag = IsDirtyComparedToMergedMemento(playerPets2, playerPets3);
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("PetsCloudApplyer: Local progress is dirty: {0}", flag);
				}
				if (flag && !skipApplyingToLocalState)
				{
					PetsManager.OverwritePlayerPetsMemento(playerPets3);
					PetsManager.LoadPetsToMemory();
				}
				bool flag2 = IsDirtyComparedToMergedMemento(playerPets, playerPets3);
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("PetsCloudApplyer: Cloud progress is dirty: {0}", flag2);
				}
				if (flag2)
				{
					SlotSynchronizer.Push(JsonUtility.ToJson(playerPets3));
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("PetsCloudApplyer: Succeeded to apply pets:\n'currentPullResult':{0},\n'cloudPetsMemento':{1},\n'localPetsMemento':{2},\n'mergedMemento':{3},\n'localDirty':{4},\n'cloudDirty':{5}", currentResult, JsonUtility.ToJson(playerPets), JsonUtility.ToJson(playerPets2), JsonUtility.ToJson(playerPets3), flag, flag2);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in PetsCloudApplyer.Apply: {0}", ex);
			}
		}

		private static bool IsDirtyComparedToMergedMemento(PlayerPets otherPetsMemento, PlayerPets mergedMemento)
		{
			try
			{
				if (mergedMemento.Pets.Select((PlayerPet pet) => pet.InfoId).Except(otherPetsMemento.Pets.Select((PlayerPet pet) => pet.InfoId)).Any())
				{
					return true;
				}
				IOrderedEnumerable<PlayerPet> source = mergedMemento.Pets.OrderBy((PlayerPet pet) => pet.InfoId);
				IOrderedEnumerable<PlayerPet> source2 = otherPetsMemento.Pets.OrderBy((PlayerPet pet) => pet.InfoId);
				return !source.Select((PlayerPet pet) => pet.PetName).SequenceEqual(source2.Select((PlayerPet pet) => pet.PetName)) || !source.Select((PlayerPet pet) => pet.NameTimestamp).SequenceEqual(source2.Select((PlayerPet pet) => pet.NameTimestamp));
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in IsDirtyComparedToMergedMemento dirtyCondition: {0}", ex);
			}
			return false;
		}
	}
}
