using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PlayerPet 
	{
		[SerializeField]
		public int Points;

		public string PetName;

		[SerializeField]
		protected internal string _infoId;

		private PetInfo _info;

		[SerializeField]
		protected internal long m_timestamp;

		public long NameTimestamp
		{
			get
			{
				return m_timestamp;
			}
			set
			{
				m_timestamp = value;
			}
		}

		public string InfoId
		{
			get
			{
				return _infoId;
			}
			set
			{
				_infoId = value;
				_info = null;
			}
		}

		public PetInfo Info
		{
			get
			{
				if (_info == null)
				{
					_info = (PetsManager.Infos.ContainsKey(InfoId) ? PetsManager.Infos[_infoId] : null);
				}
				return _info;
			}
		}

		internal static PlayerPet Merge(PlayerPet left, PlayerPet right)
		{
			return null;
		}
	}
}
