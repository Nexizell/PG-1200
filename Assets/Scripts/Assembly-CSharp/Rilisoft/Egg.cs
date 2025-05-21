using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class Egg
	{
		public EggData Data { get; private set; }

		public PlayerEgg PlayerEggData { get; private set; }

		public int Id
		{
			get
			{
				return PlayerEggData.Id;
			}
		}

		public bool OnIncubator
		{
			get
			{
				if (PlayerEggData != null)
				{
					return PlayerEggData.IncubationStart > 0;
				}
				return false;
			}
		}

		private long CurrentTime
		{
			get
			{
				if (!(Data.Id == "egg_Training"))
				{
					return EggsManager.CurrentTime;
				}
				return RiliExtensions.SystemTime;
			}
		}

		public long? IncubationTimeLeft
		{
			get
			{
				if (OnIncubator && CurrentTime < 0)
				{
					return null;
				}
				return PlayerEggData.IncubationStart + Data.Secs - CurrentTime;
			}
		}

		public long? IncubationTimeElapsed
		{
			get
			{
				if (OnIncubator && CurrentTime < 0)
				{
					return null;
				}
				return CurrentTime - PlayerEggData.IncubationStart;
			}
		}

		public int WinsLeft
		{
			get
			{
				if (HatchedType != EggHatchedType.Wins)
				{
					return int.MaxValue;
				}
				return Data.Wins - PlayerEggData.Wins;
			}
		}

		public int RatingLeft
		{
			get
			{
				if (HatchedType != EggHatchedType.Rating || RatingSystem.instance == null)
				{
					return int.MaxValue;
				}
				return Mathf.Clamp(Data.Rating - PlayerEggData.Rating, 0, int.MaxValue);
			}
		}

		public EggHatchedType HatchedType
		{
			get
			{
				return Data.HatchedType;
			}
		}

		public bool CheckReady()
		{
			if (PlayerEggData.IsReady)
			{
				return true;
			}
			switch (HatchedType)
			{
			case EggHatchedType.Time:
				if (OnIncubator && IncubationTimeLeft.HasValue && IncubationTimeLeft.Value <= 0)
				{
					return true;
				}
				break;
			case EggHatchedType.League:
				if (RatingSystem.instance != null && RatingSystem.instance.currentLeague >= Data.League)
				{
					return true;
				}
				break;
			case EggHatchedType.Wins:
				if (PlayerEggData.Wins >= Data.Wins)
				{
					return true;
				}
				break;
			case EggHatchedType.Champion:
				return true;
			case EggHatchedType.Rating:
				return RatingLeft <= 0;
			}
			return false;
		}

		public Egg(EggData staticData, PlayerEgg playerData)
		{
			Data = staticData;
			PlayerEggData = playerData;
		}

		public PetInfo DropPet()
		{
			if (Data.Id == "egg_tournament_winner")
			{
				return Singleton<PetsManager>.Instance.GetInfo("pet_griffin");
			}
			PetInfo result = null;
			EggPetInfo[] array = Data.Pets.Where((EggPetInfo p) => p.Chance > 0f).ToArray();
			if (!array.Any())
			{
				return null;
			}
			float max = array.Sum((EggPetInfo p) => p.Chance);
			float num = Random.Range(0f, max);
			float num2 = 0f;
			EggPetInfo[] array2 = array;
			foreach (EggPetInfo eggPetInfo in array2)
			{
				num2 += eggPetInfo.Chance;
				if (num2 > num)
				{
					result = (eggPetInfo.PetId.IsNullOrEmpty() ? Singleton<PetsManager>.Instance.GetRandomInfo(eggPetInfo.Rarity) : Singleton<PetsManager>.Instance.GetInfo(eggPetInfo.PetId));
					break;
				}
			}
			return result;
		}

		public string GetRelativeMeshTexturePath()
		{
			if (HatchedType == EggHatchedType.League)
			{
				return string.Format("Eggs/Textures/egg_champion_{0}_texture", new object[1] { Data.League.ToString() });
			}
			return string.Format("Eggs/Textures/{0}_texture", new object[1] { Data.Id });
		}

		public static void LogErrorFormat(string format, params object[] args)
		{
		}

		public static void LogFormat(string format, params object[] args)
		{
		}
	}
}
