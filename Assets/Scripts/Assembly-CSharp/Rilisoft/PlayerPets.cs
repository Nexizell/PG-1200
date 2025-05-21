using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PlayerPets 
	{
		internal enum MergeVariantOrigin
		{
			Local = 0,
			Remote = 1
		}

		[SerializeField]
		protected internal List<PlayerPet> m_pets = new List<PlayerPet>();

		private bool m_conflicted;

		public List<PlayerPet> Pets
		{
			get
			{
				return m_pets;
			}
			set
			{
				m_pets = value;
			}
		}

		internal bool Conflicted
		{
			get
			{
				return m_conflicted;
			}
			set
			{
				m_conflicted = value;
			}
		}

		public PlayerPets()
			: this(false)
		{
		}

		internal PlayerPets(bool conflicted)
		{
			m_pets = new List<PlayerPet>();
			m_conflicted = conflicted;
		}

		internal static PlayerPets Merge(PlayerPets localMemento, PlayerPets remoteMemento)
		{
			try
			{
				var first = localMemento.Pets.Select((PlayerPet pet) => new
				{
					Pet = pet,
					Origin = MergeVariantOrigin.Local
				});
				var second = remoteMemento.Pets.Select((PlayerPet pet) => new
				{
					Pet = pet,
					Origin = MergeVariantOrigin.Remote
				});
				IEnumerable<PlayerPet> collection = (from petWithOrigin in first.Concat(second)
					group petWithOrigin by Singleton<PetsManager>.Instance.GetAllUpgrades(petWithOrigin.Pet.InfoId).FirstOrDefault()).Select(petsFromSameUpgradesChain =>
				{
					Func<PlayerPet, int> indexOfPetInUpgrades = (PlayerPet pet) => pet.Info.Up;
					var anon = petsFromSameUpgradesChain.Aggregate((highestUpPetAccumulated, currentPetWithOrigin) => (highestUpPetAccumulated != null && indexOfPetInUpgrades(currentPetWithOrigin.Pet) <= indexOfPetInUpgrades(highestUpPetAccumulated.Pet)) ? highestUpPetAccumulated : currentPetWithOrigin);
					string infoId = anon.Pet.InfoId;
					int points = 0;
					var anon2 = petsFromSameUpgradesChain.FirstOrDefault(petWithOrigin => petWithOrigin.Origin == MergeVariantOrigin.Local);
					if (anon2 != null)
					{
						points = anon2.Pet.Points;
					}
					else if (anon.Pet.Info.Up == 0)
					{
						points = 1;
					}
					var anon3 = petsFromSameUpgradesChain.Aggregate((petWithNewestNameAccumulated, currentPetWithOrigin) => (petWithNewestNameAccumulated != null && currentPetWithOrigin.Pet.NameTimestamp <= petWithNewestNameAccumulated.Pet.NameTimestamp) ? petWithNewestNameAccumulated : currentPetWithOrigin);
					return new PlayerPet
					{
						InfoId = infoId,
						Points = points,
						PetName = anon3.Pet.PetName,
						NameTimestamp = anon3.Pet.NameTimestamp
					};
				});
				PlayerPets playerPets = new PlayerPets(localMemento.Conflicted || remoteMemento.Conflicted);
				playerPets.Pets.AddRange(collection);
				return playerPets;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in merge pets memenots: {0}", ex);
			}
			return new PlayerPets(false)
			{
				Pets = localMemento.Pets
			};
		}
	}
}
