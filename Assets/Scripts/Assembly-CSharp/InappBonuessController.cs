using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

internal sealed class InappBonuessController
{
	[Serializable]
	internal class InappRememberedBonuses 
	{
		public List<InappRememberedBonus> Bonuses;
	}

	private const string REMEMBERED_BONUSES_STORAGE_KEY = "InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY";

	private static InappBonuessController s_instance;

	public static InappBonuessController Instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new InappBonuessController();
			}
			return s_instance;
		}
	}

	public static event Action<InappRememberedBonus> OnGiveInappBonus;

	public bool InappBonusAlreadyBought(Dictionary<string, object> bonus)
	{
		if (bonus == null)
		{
			return false;
		}
		try
		{
			string item = bonus["Key"] as string;
			return BalanceController.keysInappBonusActionGiven.Contains(item);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonusAlreadyBought: {0}", ex);
			return false;
		}
	}

	public static Dictionary<string, object> FindInappBonusInBonuses(Dictionary<string, object> bonusToFind, List<Dictionary<string, object>> whereToFind)
	{
		if (bonusToFind == null)
		{
			Debug.LogErrorFormat("FindInappBonusInBonuses: bonusToFind = null");
			return null;
		}
		if (whereToFind == null)
		{
			Debug.LogWarning("FindInappBonusInBonuses: whereToFind = null");
			return null;
		}
		try
		{
			string keyOfToFind = bonusToFind["Key"] as string;
			Dictionary<string, object> dictionary = whereToFind.FirstOrDefault((Dictionary<string, object> bonus) => bonus["Key"] as string == keyOfToFind);
			if (dictionary == null)
			{
				return null;
			}
			object value;
			if (bonusToFind.TryGetValue("Weapon", out value))
			{
				string text = value as string;
				if (!text.IsNullOrEmpty() && dictionary["Weapon"] as string != text)
				{
					return null;
				}
			}
			object value2;
			if (bonusToFind.TryGetValue("Gadgets", out value2))
			{
				List<string> list = value2 as List<string>;
				if (list != null)
				{
					List<string> source = dictionary["Gadgets"] as List<string>;
					if (!list.OrderBy((string x) => x).SequenceEqual(source.OrderBy((string x) => x)))
					{
						return null;
					}
				}
			}
			return dictionary;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in FindInappBonusInBonuses: {0}", ex);
			return null;
		}
	}

	public static bool AreInappBonusesEquals(List<Dictionary<string, object>> left, List<Dictionary<string, object>> right)
	{
		if (left == null && right == null)
		{
			return true;
		}
		if (left == null || right == null)
		{
			return false;
		}
		if (left.Count != right.Count)
		{
			return false;
		}
		foreach (Dictionary<string, object> item in left)
		{
			if (FindInappBonusInBonuses(item, right) == null)
			{
				return false;
			}
		}
		return true;
	}

	public void RememberCurrentBonusForInapp(string inappId, Dictionary<string, object> currentInAppBonus)
	{
		if (inappId.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("RememberCurrentBonusForInapp: inappId.IsNullOrEmpty()");
			return;
		}
		try
		{
			InappRememberedBonuses inappRememberedBonuses = LoadBonusesFromDisk();
			inappRememberedBonuses.Bonuses.RemoveAll((InappRememberedBonus rememberedBonus) => rememberedBonus != null && rememberedBonus.InappId == inappId);
			if (currentInAppBonus != null)
			{
				if (Convert.ToString(currentInAppBonus["ID"]) == inappId)
				{
					InappRememberedBonus actualBonusSizeForInappBonus = GetActualBonusSizeForInappBonus(currentInAppBonus);
					if (actualBonusSizeForInappBonus != null)
					{
						inappRememberedBonuses.Bonuses.Add(actualBonusSizeForInappBonus);
					}
					else
					{
						Debug.LogErrorFormat("RememberCurrentBonusForInapp: bonusForThisInapp == null inappId = {0}, currentInAppBonus = {1}", inappId, Json.Serialize(currentInAppBonus));
					}
				}
				else
				{
					Debug.LogErrorFormat("RememberCurrentBonusForInapp Convert.ToString( currentInAppBonus[ \"ID\" ] ) != inappId, inappId = {0}, currentInAppBonus = {1}", inappId, Json.Serialize(currentInAppBonus));
				}
			}
			SaveBonusesToDisk(inappRememberedBonuses);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in RememberCurrentBonusForInapp: {0}", ex);
		}
	}

	internal static void GiveWeapon(string weaponId)
	{
		if (weaponId.IsNullOrEmpty())
		{
			return;
		}
		int itemCategory = ItemDb.GetItemCategory(weaponId);
		WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(weaponId);
		ShopNGUIController.ProvideItem((ShopNGUIController.CategoryNames)itemCategory, weaponId, 1, false, 0, delegate(string item)
		{
			if ((!GameConnect.isDaterRegim || !(weaponInfo != null) || weaponInfo.IsAvalibleFromFilter(3)) && !GameConnect.isHunger && !GameConnect.isSurvival && !GameConnect.isCOOP && ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.FireBuyAction(item);
			}
		});
		if (WeaponManager.sharedManager != null)
		{
			int currentWeaponIndex = WeaponManager.sharedManager.CurrentWeaponIndex;
			WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
			WeaponManager.sharedManager.CurrentWeaponIndex = currentWeaponIndex;
		}
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateIcons();
			ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory);
			if (ArmoryInfoScreenController.sharedController != null)
			{
				ArmoryInfoScreenController.sharedController.DestroyWindow();
			}
		}
	}

	internal static void GiveLeprechaun(int daysLeprechaun, string currencyLeprechaun, int perDayLeprechaun)
	{
		try
		{
			Singleton<LeprechauntManager>.Instance.SetLeprechaunt(86400 * daysLeprechaun, currencyLeprechaun, perDayLeprechaun);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveLeprechaunForInapp SetLeprechaunt: {0}", ex);
		}
	}

	internal static void GivePets(string petId, int quantity)
	{
		if (petId.IsNullOrEmpty())
		{
			return;
		}
		if (quantity <= 0)
		{
			Debug.LogErrorFormat("GiveBonusForInapp: giving pet, quantity <= 0");
		}
		PetUpdateInfo petUpdateInfo = null;
		for (int i = 0; i < quantity; i++)
		{
			try
			{
				petUpdateInfo = Singleton<PetsManager>.Instance.AddOrUpdatePet(petId);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in GiveBonusForInapp giving pet: {0}", ex);
			}
		}
		try
		{
			if (petUpdateInfo != null)
			{
				ShopNGUIController.sharedShop.EquipPetAndUpdate(petUpdateInfo.PetNew.InfoId);
				Singleton<PetsManager>.Instance.ActualizeEquippedPet();
			}
			else
			{
				Debug.LogErrorFormat("GiveBonusForInapp petUpdateInfo == null");
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.UpdateIcons();
				ShopNGUIController.sharedShop.UpdatePetsCategoryIfNeeded();
				if (ArmoryInfoScreenController.sharedController != null)
				{
					ArmoryInfoScreenController.sharedController.DestroyWindow();
				}
			}
		}
		catch (Exception ex2)
		{
			Debug.LogErrorFormat("Exception in GiveBonusForInapp equip pet and update: {0}", ex2);
		}
	}

	private static bool IsInMatch()
	{
		if (WeaponManager.sharedManager != null)
		{
			return WeaponManager.sharedManager.myPlayerMoveC != null;
		}
		return false;
	}

	internal static void GiveGadgets(List<string> gadgetIds)
	{
		bool flag = IsInMatch();
		for (int i = 0; i < gadgetIds.Count; i++)
		{
			string text = GadgetsInfo.FirstUnboughtOrForOurTier(gadgetIds[i]);
			if (text == null)
			{
				Debug.LogErrorFormat("GiveGadgets: firstUnboughtOrForOurTier == null");
				continue;
			}
			GadgetsInfo.ProvideGadget(text);
			if (!flag)
			{
				ShopNGUIController.EquipGadget(text, (GadgetInfo.GadgetCategory)ItemDb.GetItemCategory(text));
				GadgetsInfo.ActualizeEquippedGadgets();
			}
		}
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateIcons();
			ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory);
			if (ArmoryInfoScreenController.sharedController != null)
			{
				ArmoryInfoScreenController.sharedController.DestroyWindow();
			}
		}
	}

	public InappRememberedBonus GiveBonusForInapp(string inappId)
	{
		Action<InappRememberedBonus> action = delegate(InappRememberedBonus bonus)
		{
			try
			{
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				if (bonus != null)
				{
					StoreKitEventListener.LogVirtualCurrencyPurchased(inappId, bonus.Gems, bonus.Coins);
				}
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in GiveBonusForInapp analytics: {0}", ex2);
			}
			AnalyticsStuff.CustomSpecialOffersSalesBuy(inappId);
		};
		if (inappId.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("GiveBonusForInapp: inappId.IsNullOrEmpty()");
			action(null);
			return null;
		}
		try
		{
			InappRememberedBonus inappRememberedBonus = LoadBonusesFromDisk().Bonuses.FirstOrDefault((InappRememberedBonus bonus) => bonus != null && bonus.InappId == inappId);
			if (inappRememberedBonus == null)
			{
				action(null);
				return null;
			}
			if (inappRememberedBonus.Coins > 0)
			{
				BankController.AddCoins(inappRememberedBonus.Coins, true, AnalyticsConstants.AccrualType.Purchased);
				coinsShop.TryToFireCurrenciesAddEvent("Coins");
			}
			if (inappRememberedBonus.Gems > 0)
			{
				BankController.AddGems(inappRememberedBonus.Gems, true, AnalyticsConstants.AccrualType.Purchased);
				coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
			}
			GivePetForInapp(inappRememberedBonus);
			if (!inappRememberedBonus.ActionStartTime.IsNullOrEmpty())
			{
				BalanceController.AddKeysInappBonusActionGiven(inappRememberedBonus.ActionStartTime);
			}
			else
			{
				Debug.LogErrorFormat("GiveBonusForInapp: bonusForThisInapp.ActionStartTime.IsNullOrEmpty(), inappId = {0}", inappRememberedBonus.InappId ?? "null");
			}
			GiveWeaponForInapp(inappRememberedBonus);
			GiveGadgetsForInapp(inappRememberedBonus);
			GiveLeprechaunForInapp(inappRememberedBonus);
			Action<InappRememberedBonus> onGiveInappBonus = InappBonuessController.OnGiveInappBonus;
			if (onGiveInappBonus != null)
			{
				onGiveInappBonus(inappRememberedBonus);
			}
			action(inappRememberedBonus);
			return inappRememberedBonus;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveBonusForInapp: {0}, inappId = {1}", ex, inappId);
		}
		action(null);
		return null;
	}

	public InappRememberedBonus GetActualBonusSizeForInappBonus(Dictionary<string, object> inappBonus)
	{
		try
		{
			if (inappBonus == null)
			{
				Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: inappBonus == null");
				return null;
			}
			int num = 1;
			int num2 = 0;
			object value;
			if (inappBonus.TryGetValue("Coins", out value))
			{
				num2 = Convert.ToInt32(value);
			}
			int num3 = 0;
			object value2;
			if (inappBonus.TryGetValue("GemsCurrency", out value2))
			{
				num3 = Convert.ToInt32(value2);
			}
			if (num2 == 0 && num3 == 0)
			{
				Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: coins == 0 && gems == 0, inappBonus = {0}", Json.Serialize(inappBonus));
			}
			string value3;
			if (inappBonus.TryGetValue<string>("Key", out value3))
			{
				value3 = value3 ?? string.Empty;
			}
			else
			{
				Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: !inappBonus.TryGetValue( \"Key\" , out actionStartTime ), inappBonus = {0}", Json.Serialize(inappBonus));
				value3 = string.Empty;
			}
			string petId = string.Empty;
			try
			{
				object value4;
				if (inappBonus.TryGetValue("Pet", out value4))
				{
					petId = (value4 as string) ?? string.Empty;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting petId: {0}", ex);
			}
			int quantity = 0;
			try
			{
				object value5;
				if (inappBonus.TryGetValue("Quantity", out value5))
				{
					quantity = Convert.ToInt32(value5);
				}
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting quantity: {0}", ex2);
			}
			string weaponId = string.Empty;
			try
			{
				object value6;
				if (inappBonus.TryGetValue("Weapon", out value6))
				{
					weaponId = (value6 as string) ?? string.Empty;
				}
			}
			catch (Exception ex3)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting weapon: {0}", ex3);
			}
			List<string> list = null;
			try
			{
				object value7;
				if (inappBonus.TryGetValue("Gadgets", out value7))
				{
					list = (value7 as List<string>).OfType<string>().ToList();
				}
			}
			catch (Exception ex4)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting gadget ids: {0}", ex4);
			}
			if (list == null)
			{
				list = new List<string>();
			}
			string currencyLeprechaun = string.Empty;
			try
			{
				object value8;
				if (inappBonus.TryGetValue("CurrencyLeprechaun", out value8))
				{
					currencyLeprechaun = (value8 as string) ?? string.Empty;
				}
			}
			catch (Exception ex5)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr currency: {0}", ex5);
			}
			int daysLeprechaun = 0;
			try
			{
				object value9;
				if (inappBonus.TryGetValue("DaysLeprechaun", out value9))
				{
					daysLeprechaun = Convert.ToInt32(value9);
				}
			}
			catch (Exception ex6)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr days: {0}", ex6);
			}
			int perDayLeprechaun = 0;
			try
			{
				object value10;
				if (inappBonus.TryGetValue("PerDayLeprechaun", out value10))
				{
					perDayLeprechaun = Convert.ToInt32(value10);
				}
			}
			catch (Exception ex7)
			{
				Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr per day: {0}", ex7);
			}
			return new InappRememberedBonus
			{
				InappId = Convert.ToString(inappBonus["ID"]),
				Coins = num2 * num,
				Gems = num3 * num,
				ActionStartTime = value3,
				PetId = petId,
				Quantity = quantity,
				WeaponId = weaponId,
				GadgetIds = list,
				CurrencyLeprechaun = currencyLeprechaun,
				DaysLeprechaun = daysLeprechaun,
				PerDayLeprechaun = perDayLeprechaun
			};
		}
		catch (Exception ex8)
		{
			Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus: {0}", ex8);
			return null;
		}
	}

	private static void GivePetForInapp(InappRememberedBonus bonusForThisInapp)
	{
		string petId = bonusForThisInapp.PetId;
		int quantity = bonusForThisInapp.Quantity;
		GivePets(petId, quantity);
	}

	private static void GiveWeaponForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			GiveWeapon(bonusForThisInapp.WeaponId);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveBonusForInapp: giving weapon: {0}", ex);
		}
	}

	private static void GiveLeprechaunForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			if (bonusForThisInapp != null && bonusForThisInapp.DaysLeprechaun > 0 && bonusForThisInapp.PerDayLeprechaun > 0 && (bonusForThisInapp.CurrencyLeprechaun == "Coins" || bonusForThisInapp.CurrencyLeprechaun == "GemsCurrency"))
			{
				int daysLeprechaun = bonusForThisInapp.DaysLeprechaun;
				string currencyLeprechaun = bonusForThisInapp.CurrencyLeprechaun;
				int perDayLeprechaun = bonusForThisInapp.PerDayLeprechaun;
				GiveLeprechaun(daysLeprechaun, currencyLeprechaun, perDayLeprechaun);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveGadgetsForInapp: giving weapon: {0}", ex);
		}
	}

	private static void GiveGadgetsForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			if (bonusForThisInapp != null && bonusForThisInapp.GadgetIds != null && bonusForThisInapp.GadgetIds.Count > 0)
			{
				GiveGadgets(bonusForThisInapp.GadgetIds);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveGadgetsForInapp: giving weapon: {0}", ex);
		}
	}

	private InappBonuessController()
	{
	}

	private void SaveBonusesToDisk(InappRememberedBonuses bonuses)
	{
		if (bonuses == null)
		{
			Debug.LogErrorFormat("SaveBonusesToDisk: bonuses == null");
			return;
		}
		try
		{
			string val = JsonUtility.ToJson(bonuses);
			Storager.setString("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY", val);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonuessController.SaveBonusesToDisk: {0}", ex);
		}
	}

	private InappRememberedBonuses LoadBonusesFromDisk()
	{
		if (!Storager.hasKey("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY"))
		{
			InappRememberedBonuses bonuses = new InappRememberedBonuses
			{
				Bonuses = new List<InappRememberedBonus>()
			};
			SaveBonusesToDisk(bonuses);
		}
		try
		{
			return JsonUtility.FromJson<InappRememberedBonuses>(Storager.getString("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY"));
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonuessController.LoadBonusesFromDisk: {0}", ex);
			return new InappRememberedBonuses
			{
				Bonuses = new List<InappRememberedBonus>()
			};
		}
	}
}
