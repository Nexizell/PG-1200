using System.Collections.Generic;
using Rilisoft;

public static class EffectsController
{
	private static float slowdownCoeff = 1f;

	public static float SlowdownCoeff
	{
		get
		{
			return slowdownCoeff;
		}
		set
		{
			slowdownCoeff = value;
		}
	}

	public static float JumpModifier
	{
		get
		{
			float num = 1f;
			if (GameConnect.wearEffectsDisabled)
			{
				return num;
			}
			num += (Storager.getString(Defs.HatEquppedSN).Equals("hat_Samurai") ? 0.05f : 0f);
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Custom") ? 0.05f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("boots_gray") ? 0.05f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("StormTrooperBoots_Up1") ? 0.1f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("StormTrooperBoots_Up2") ? 0.15f : 0f);
			num += (Storager.getString("MaskEquippedSN").Equals("mask_demolition") ? 0.05f : 0f);
			num += (Storager.getString("MaskEquippedSN").Equals("mask_demolition_up1") ? 0.1f : 0f);
			num += (Storager.getString("MaskEquippedSN").Equals("mask_demolition_up2") ? 0.15f : 0f);
			num += (Storager.getString(Defs.HatEquppedSN).Equals("league3_hat_afro") ? 0.08f : 0f);
			num += (Storager.getString(Defs.HatEquppedSN).Equals("league6_hat_tiara") ? 0.08f : 0f);
			return num * SlowdownCoeff;
		}
	}

	public static bool NinjaJumpEnabled
	{
		get
		{
			if (GameConnect.wearEffectsDisabled || (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isMechActive))
			{
				return false;
			}
			if (!Storager.getString(Defs.BootsEquppedSN).Equals("boots_tabi") && !Storager.getString(Defs.BootsEquppedSN).Equals("boots_black") && !Storager.getString(Defs.BootsEquppedSN).Equals("BerserkBoots_Up1"))
			{
				return Storager.getString(Defs.BootsEquppedSN).Equals("BerserkBoots_Up2");
			}
			return true;
		}
	}

	public static float ExplosionImpulseRadiusIncreaseCoef
	{
		get
		{
			if (GameConnect.wearEffectsDisabled)
			{
				return 0f;
			}
			return (Storager.getString(Defs.BootsEquppedSN).Equals("boots_green") ? 0.05f : 0f) + (Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up1") ? 0.1f : 0f) + (Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up2") ? 0.15f : 0f) + (Storager.getString(Defs.HatEquppedSN).Equals("league3_hat_afro") ? 0.08f : 0f);
		}
	}

	public static float GrenadeExplosionDamageIncreaseCoef
	{
		get
		{
			if (GameConnect.wearEffectsDisabled)
			{
				return 0f;
			}
			return (Storager.getString(Defs.BootsEquppedSN).Equals("boots_green") ? 0.1f : 0f) + (Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up1") ? 0.15f : 0f) + (Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up2") ? 0.25f : 0f);
		}
	}

	public static float GrenadeExplosionRadiusIncreaseCoef
	{
		get
		{
			float num = 1f;
			if (GameConnect.wearEffectsDisabled)
			{
				return num;
			}
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_RoyalKnight") ? 0.2f : 0f);
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("DemolitionCape_Up1") ? 0.3f : 0f);
			return num + (Storager.getString(Defs.CapeEquppedSN).Equals("DemolitionCape_Up2") ? 0.5f : 0f);
		}
	}

	public static float SelfExplosionDamageDecreaseCoef
	{
		get
		{
			if (GameConnect.wearEffectsDisabled)
			{
				return 1f;
			}
			return 1f * (Storager.getString(Defs.HatEquppedSN).Equals("hat_KingsCrown") ? 0.8f : 1f) * (Storager.getString(Defs.HatEquppedSN).Equals("hat_DiamondHelmet") ? 0.8f : 1f) * (Storager.getString(Defs.CapeEquppedSN).Equals("cape_RoyalKnight") ? 0.8f : 1f) * (Storager.getString(Defs.CapeEquppedSN).Equals("DemolitionCape_Up1") ? 0.7f : 1f) * (Storager.getString(Defs.CapeEquppedSN).Equals("DemolitionCape_Up2") ? 0.5f : 1f) * (Storager.getString(Defs.HatEquppedSN).Equals("league4_hat_mushroom") ? 0.5f : 1f);
		}
	}

	public static bool WeAreStealth
	{
		get
		{
			if (GameConnect.wearEffectsDisabled)
			{
				return false;
			}
			if (!Storager.getString(Defs.BootsEquppedSN).Equals("boots_blue") && !Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up1") && !Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up2"))
			{
				return Storager.getString(Defs.HatEquppedSN).Equals("league4_hat_mushroom");
			}
			return true;
		}
	}

	public static float ArmorBonus
	{
		get
		{
			float num = 0f;
			if (GameConnect.wearEffectsDisabled)
			{
				return num;
			}
			if (Storager.getString(Defs.HatEquppedSN).Equals("league6_hat_tiara"))
			{
				num += 3f;
			}
			if (Storager.getString(Defs.BootsEquppedSN).Equals("boots_red"))
			{
				num += 1f;
			}
			if (Storager.getString(Defs.BootsEquppedSN).Equals("HitmanBoots_Up1"))
			{
				num += 2f;
			}
			if (Storager.getString(Defs.BootsEquppedSN).Equals("HitmanBoots_Up2"))
			{
				num += 3f;
			}
			if (Storager.getString("MaskEquippedSN").Equals("mask_berserk"))
			{
				num += 1f;
			}
			else if (Storager.getString("MaskEquippedSN").Equals("mask_berserk_up1"))
			{
				num += 2f;
			}
			else if (Storager.getString("MaskEquippedSN").Equals("mask_berserk_up2"))
			{
				num += 3f;
			}
			return num;
		}
	}

	public static float IcnreaseEquippedArmorPercentage
	{
		get
		{
			float num = 1f;
			if (GameConnect.wearEffectsDisabled)
			{
				return num;
			}
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_BloodyDemon") ? 0.1f : 0f);
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("BerserkCape_Up1") ? 0.15f : 0f);
			return num + (Storager.getString(Defs.CapeEquppedSN).Equals("BerserkCape_Up2") ? 0.25f : 0f);
		}
	}

	public static float RegeneratingArmorTime
	{
		get
		{
			float result = 0f;
			if (GameConnect.wearEffectsDisabled)
			{
				return result;
			}
			if (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Archimage"))
			{
				result = 12f;
			}
			if (Storager.getString(Defs.CapeEquppedSN).Equals("HitmanCape_Up1"))
			{
				result = 10f;
			}
			if (Storager.getString(Defs.HatEquppedSN).Equals("league5_hat_brain"))
			{
				result = 9f;
			}
			if (Storager.getString(Defs.CapeEquppedSN).Equals("HitmanCape_Up2"))
			{
				result = 8f;
			}
			return result;
		}
	}

	public static bool IsRegeneratingArmor
	{
		get
		{
			return RegeneratingArmorTime > 0f;
		}
	}

	public static float AmmoModForCategory(int i)
	{
		float num = 1f;
		if (GameConnect.wearEffectsDisabled)
		{
			return num;
		}
		if (Storager.getString(Defs.HatEquppedSN).Equals("league2_hat_cowboyhat"))
		{
			num += 0.13f;
		}
		switch (i)
		{
		case 0:
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("boots_gray") ? 0.1f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("StormTrooperBoots_Up1") ? 0.15f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("StormTrooperBoots_Up2") ? 0.25f : 0f);
			break;
		case 3:
			num += (Storager.getString("MaskEquippedSN").Equals("mask_engineer") ? 0.1f : 0f);
			num += (Storager.getString("MaskEquippedSN").Equals("mask_engineer_up1") ? 0.15f : 0f);
			num += (Storager.getString("MaskEquippedSN").Equals("mask_engineer_up2") ? 0.25f : 0f);
			break;
		}
		return num;
	}

	public static float DamageModifsByCats(int i)
	{
		List<float> list = new List<float>(6);
		for (int j = 0; j < 6; j++)
		{
			list.Add(0f);
		}
		if (GameConnect.wearEffectsDisabled)
		{
			if (i < 0 || i >= list.Count)
			{
				return 0f;
			}
			return list[i];
		}
		if (Storager.getString(Defs.HatEquppedSN).Equals("league6_hat_tiara"))
		{
			for (int k = 0; k < list.Count; k++)
			{
				list[k] += 0.13f;
			}
		}
		list[0] += (Storager.getString(Defs.HatEquppedSN).Equals("hat_Headphones") ? 0.1f : 0f);
		list[2] += (Storager.getString("MaskEquippedSN").Equals("hat_ManiacMask") ? 0.1f : 0f);
		list[2] += (Storager.getString(Defs.HatEquppedSN).Equals("hat_Samurai") ? 0.1f : 0f);
		list[1] += (Storager.getString(Defs.HatEquppedSN).Equals("hat_SeriousManHat") ? 0.1f : 0f);
		list[0] += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_EliteCrafter") ? 0.1f : 0f);
		list[0] += (Storager.getString(Defs.CapeEquppedSN).Equals("StormTrooperCape_Up1") ? 0.15f : 0f);
		list[0] += (Storager.getString(Defs.CapeEquppedSN).Equals("StormTrooperCape_Up2") ? 0.25f : 0f);
		list[1] += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Archimage") ? 0.1f : 0f);
		list[1] += (Storager.getString(Defs.CapeEquppedSN).Equals("HitmanCape_Up1") ? 0.15f : 0f);
		list[1] += (Storager.getString(Defs.CapeEquppedSN).Equals("HitmanCape_Up2") ? 0.25f : 0f);
		list[2] += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_BloodyDemon") ? 0.1f : 0f);
		list[2] += (Storager.getString(Defs.CapeEquppedSN).Equals("BerserkCape_Up1") ? 0.15f : 0f);
		list[2] += (Storager.getString(Defs.CapeEquppedSN).Equals("BerserkCape_Up2") ? 0.25f : 0f);
		list[5] += (Storager.getString(Defs.HatEquppedSN).Equals("league1_hat_hitman") ? 0.15f : 0f);
		if (i < 0 || i >= list.Count)
		{
			return 0f;
		}
		return list[i];
	}

	public static float SpeedModifier(int i)
	{
		if (GameConnect.wearEffectsDisabled || (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isMechActive))
		{
			return 1f;
		}
		float num = WeaponManager.sharedManager.currentWeaponSounds.speedModifier * (Storager.getString(Defs.HatEquppedSN).Equals("hat_KingsCrown") ? 1.05f : 1f) * (Storager.getString(Defs.HatEquppedSN).Equals("hat_Samurai") ? 1.05f : 1f) * (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Custom") ? 1.05f : 1f) * (Storager.getString(Defs.HatEquppedSN).Equals("league6_hat_tiara") ? 1.08f : 1f) * (Storager.getString(Defs.HatEquppedSN).Equals("league3_hat_afro") ? 1.08f : 1f) * SlowdownCoeff;
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN).Equals("boots_red"))
		{
			num *= 1.05f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN).Equals("HitmanBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN).Equals("HitmanBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN).Equals("boots_black"))
		{
			num *= 1.05f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN).Equals("BerserkBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN).Equals("BerserkBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots"))
		{
			num *= 1.05f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN).Equals("boots_blue"))
		{
			num *= 1.05f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN).Equals("boots_green"))
		{
			num *= 1.05f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN).Equals("DemolitionBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN").Equals("mask_trooper"))
		{
			num *= 1.05f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN").Equals("mask_trooper_up1"))
		{
			num *= 1.1f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN").Equals("mask_trooper_up2"))
		{
			num *= 1.15f;
		}
		return num;
	}

	public static float AddingForPotionDuration(string potion)
	{
		float num = 0f;
		if (GameConnect.wearEffectsDisabled)
		{
			return num;
		}
		if (potion == null)
		{
			return num;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN).Equals("boots_blue"))
		{
			num += 5f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up1"))
		{
			num += 10f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN).Equals("SniperBoots_Up2"))
		{
			num += 15f;
		}
		if (!GameConnect.isDaterRegim)
		{
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Engineer") ? 10f : 0f);
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Engineer_Up1") ? 15f : 0f);
			num += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_Engineer_Up2") ? 20f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots") ? 10f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots_Up1") ? 15f : 0f);
			num += (Storager.getString(Defs.BootsEquppedSN).Equals("EngineerBoots_Up2") ? 20f : 0f);
			num += (Storager.getString(Defs.HatEquppedSN).Equals("league5_hat_brain") ? 13f : 0f);
		}
		return num;
	}

	public static float GetReloadAnimationSpeed(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = "";
		}
		if (currentMask == null)
		{
			currentMask = "";
		}
		if (currentHat == null)
		{
			currentHat = "";
		}
		float num = 1f;
		if (GameConnect.wearEffectsDisabled)
		{
			return num;
		}
		if (currentHat.Equals("league2_hat_cowboyhat"))
		{
			num += 0.13f;
		}
		switch (categoryNabor)
		{
		case 1:
			num += (currentCape.Equals("cape_EliteCrafter") ? 0.1f : 0f);
			num += (currentCape.Equals("StormTrooperCape_Up1") ? 0.15f : 0f);
			num += (currentCape.Equals("StormTrooperCape_Up2") ? 0.25f : 0f);
			break;
		case 2:
			num += (currentMask.Equals("mask_hitman_1") ? 0.1f : 0f);
			num += (currentMask.Equals("mask_hitman_1_up1") ? 0.15f : 0f);
			num += (currentMask.Equals("mask_hitman_1_up2") ? 0.25f : 0f);
			break;
		case 4:
			num += (currentCape.Equals("cape_Engineer") ? 0.1f : 0f);
			num += (currentCape.Equals("cape_Engineer_Up1") ? 0.15f : 0f);
			num += (currentCape.Equals("cape_Engineer_Up2") ? 0.25f : 0f);
			break;
		case 5:
			num += (currentCape.Equals("cape_SkeletonLord") ? 0.1f : 0f);
			num += (currentCape.Equals("SniperCape_Up1") ? 0.15f : 0f);
			num += (currentCape.Equals("SniperCape_Up2") ? 0.25f : 0f);
			break;
		case 6:
			num += (currentCape.Equals("cape_RoyalKnight") ? 0.1f : 0f);
			num += (currentCape.Equals("DemolitionCape_Up1") ? 0.15f : 0f);
			num += (currentCape.Equals("DemolitionCape_Up2") ? 0.25f : 0f);
			break;
		}
		return num;
	}

	public static float AddingForHeadshot(int cat)
	{
		if (GameConnect.wearEffectsDisabled)
		{
			return 0f;
		}
		List<float> list = new List<float>(6);
		for (int i = 0; i < 6; i++)
		{
			list.Add(0f);
		}
		if (Storager.getString(Defs.HatEquppedSN).Equals("league5_hat_brain"))
		{
			for (int j = 0; j < 6; j++)
			{
				list[j] += 0.13f;
			}
		}
		list[4] += (Storager.getString(Defs.CapeEquppedSN).Equals("cape_SkeletonLord") ? 0.1f : 0f);
		list[4] += (Storager.getString(Defs.CapeEquppedSN).Equals("SniperCape_Up1") ? 0.15f : 0f);
		list[4] += (Storager.getString(Defs.CapeEquppedSN).Equals("SniperCape_Up2") ? 0.25f : 0f);
		if (cat < 0 || cat >= list.Count)
		{
			return 0f;
		}
		return list[cat];
	}

	public static float GetChanceToIgnoreHeadshot(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = "";
		}
		if (currentMask == null)
		{
			currentMask = "";
		}
		if (currentHat == null)
		{
			currentHat = "";
		}
		float num = 0f;
		if (GameConnect.wearEffectsDisabled)
		{
			return num;
		}
		if (currentHat.Equals("league4_hat_mushroom"))
		{
			num += 0.13f;
		}
		switch ((ShopNGUIController.CategoryNames)(categoryNabor - 1))
		{
		case ShopNGUIController.CategoryNames.MeleeCategory:
			if (currentCape.Equals("cape_BloodyDemon"))
			{
				num += 0.1f;
			}
			else if (currentCape.Equals("BerserkCape_Up1"))
			{
				num += 0.15f;
			}
			else if (currentCape.Equals("BerserkCape_Up2"))
			{
				num += 0.25f;
			}
			break;
		case ShopNGUIController.CategoryNames.SniperCategory:
			if (currentMask.Equals("mask_sniper"))
			{
				num += 0.1f;
			}
			else if (currentMask.Equals("mask_sniper_up1"))
			{
				num += 0.15f;
			}
			else if (currentMask.Equals("mask_sniper_up2"))
			{
				num += 0.25f;
			}
			break;
		}
		return num;
	}

	public static float PetSpeedModificator()
	{
		float effect = LobbyItemsController.GetEffect(LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier);
		return 1f + effect;
	}

	public static float PetDamageModificator()
	{
		float effect = LobbyItemsController.GetEffect(LobbyItemInfo.LobbyItemBuffType.PetDamadeMultiplier);
		return 1f + effect;
	}
}
