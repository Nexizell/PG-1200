using UnityEngine;

public sealed class AmmoButtonInGamePanel : MonoBehaviour
{
	public GameObject fullLabel;

	public UIButton myButton;

	public UILabel priceLabel;

	public InGameGUI inGameGui;

	private void Start()
	{
		priceLabel.text = Defs.ammoInGamePanelPrice.ToString();
	}

	private void Update()
	{
		UpdateState();
	}

	private void UpdateState(bool isDelta = true)
	{
		Weapon obj = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
		int currentAmmoInBackpack = obj.currentAmmoInBackpack;
		int maxAmmoWithEffectApplied = obj.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
		bool flag = currentAmmoInBackpack == maxAmmoWithEffectApplied;
		if (flag == myButton.isEnabled || !isDelta)
		{
			fullLabel.SetActive(flag);
			myButton.isEnabled = !flag;
			priceLabel.gameObject.SetActive(!flag);
		}
	}

	private void OnEnable()
	{
		UpdateState(false);
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			Weapon obj = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component = obj.weaponPrefab.GetComponent<WeaponSounds>();
			obj.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
			return;
		}
		ShopNGUIController.TryToBuy(inGameGui.gameObject, new ItemPrice(Defs.ammoInGamePanelPrice, "Coins"), delegate
		{
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
			}
			Weapon obj2 = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component2 = obj2.weaponPrefab.GetComponent<WeaponSounds>();
			obj2.currentAmmoInBackpack = component2.MaxAmmoWithEffectApplied;
		}, JoystickController.leftJoystick.Reset);
	}
}
