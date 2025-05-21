namespace Rilisoft
{
	public class PetUpdateInfo
	{
		public PlayerPet PetOld;

		public PlayerPet PetNew;

		public bool PetAdded
		{
			get
			{
				return PetOld == null;
			}
		}

		public bool Upgraded
		{
			get
			{
				if (PetOld != null && PetNew != null)
				{
					return PetOld.InfoId != PetNew.InfoId;
				}
				return false;
			}
		}

		public bool PetPointsAdded
		{
			get
			{
				if (PetOld != null && PetNew != null)
				{
					return PetOld.Points != PetNew.Points;
				}
				return false;
			}
		}

		public PetUpdateInfo()
		{
		}

		public PetUpdateInfo(PlayerPet petOld, PlayerPet petNew)
		{
			PetOld = petOld;
			PetNew = petNew;
		}
	}
}
