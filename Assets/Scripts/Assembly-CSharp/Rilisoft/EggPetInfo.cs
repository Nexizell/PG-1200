using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class EggPetInfo 
	{
		[SerializeField]
		[Tooltip("Тип пета")]
		public ItemDb.ItemRarity Rarity;

		[SerializeField]
		[Tooltip("Идентификатор пета")]
		public string PetId;

		[Tooltip("шанс получения")]
		[SerializeField]
		public float Chance;
	}
}
