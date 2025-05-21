using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class PersConfigurator : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PersConfigurator _003C_003E4__this;

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
		public _003CStart_003Ed__14(int _003C_003E1__state)
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
				_003C_003E4__this.ResetWeapon();
				_003C_003E4__this.SetCurrentSkin();
				ShopNGUIController.sharedShop.onEquipSkinAction = delegate
				{
					_003C_003E4__this.SetCurrentSkin();
				};
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.UpdateWear();
				ShopNGUIController.sharedShop.wearEquipAction = delegate
				{
					_003C_003E4__this.UpdateWear();
				};
				ShopNGUIController.sharedShop.wearUnequipAction = delegate
				{
					_003C_003E4__this.UpdateWear();
				};
				ShopNGUIController.ShowArmorChanged += _003C_003E4__this.UpdateWear;
				ShopNGUIController.ShowWearChanged += _003C_003E4__this.UpdateWear;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (NickLabelStack.sharedStack == null)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			NickLabelController.currentCamera = Camera.main;
			if (NickLabelController.currentCamera == null && MainMenuHeroCamera.Instance != null)
			{
				NickLabelController.currentCamera = MainMenuHeroCamera.Instance.MainCamera;
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

	public GameObject gun;

	private GameObject weapon;

	private NickLabelController _label;

	private GameObject shadow;

	private AnimationClip profile;

	public static PersConfigurator currentConfigurator { get; private set; }

	public CharacterInterface characterInterface { get; private set; }

	private void Awake()
	{
		currentConfigurator = this;
		GameObject original = Resources.Load("Character_model") as GameObject;
		characterInterface = UnityEngine.Object.Instantiate(original).GetComponent<CharacterInterface>();
		characterInterface.transform.SetParent(base.transform, false);
		characterInterface.SetCharacterType(false, false, false);
		ShopNGUIController.DisableLightProbesRecursively(characterInterface.gameObject);
	}

	private IEnumerator Start()
	{
		ResetWeapon();
		SetCurrentSkin();
		ShopNGUIController.sharedShop.onEquipSkinAction = delegate
		{
			SetCurrentSkin();
		};
		yield return new WaitForEndOfFrame();
		UpdateWear();
		ShopNGUIController.sharedShop.wearEquipAction = delegate
		{
			UpdateWear();
		};
		ShopNGUIController.sharedShop.wearUnequipAction = delegate
		{
			UpdateWear();
		};
		ShopNGUIController.ShowArmorChanged += UpdateWear;
		ShopNGUIController.ShowWearChanged += UpdateWear;
		while (NickLabelStack.sharedStack == null)
		{
			yield return null;
		}
		NickLabelController.currentCamera = Camera.main;
		if (NickLabelController.currentCamera == null && MainMenuHeroCamera.Instance != null)
		{
			NickLabelController.currentCamera = MainMenuHeroCamera.Instance.MainCamera;
		}
	}

	public void ResetWeapon()
	{
		if (this.weapon != null)
		{
			UnityEngine.Object.Destroy(this.weapon);
		}
		this.weapon = null;
		WeaponManager sharedManager = WeaponManager.sharedManager;
		GameObject gameObject = null;
		List<Weapon> list = new List<Weapon>();
		foreach (Weapon allAvailablePlayerWeapon in sharedManager.allAvailablePlayerWeapons)
		{
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(allAvailablePlayerWeapon.weaponPrefab.name.Replace("(Clone)", "")).Tag))
			{
				list.Add(allAvailablePlayerWeapon);
			}
		}
		if (list.Count == 0)
		{
			foreach (Weapon allAvailablePlayerWeapon2 in sharedManager.allAvailablePlayerWeapons)
			{
				if (allAvailablePlayerWeapon2.weaponPrefab.name.Replace("(Clone)", "") == WeaponManager.PistolWN)
				{
					gameObject = allAvailablePlayerWeapon2.weaponPrefab;
					break;
				}
			}
		}
		else
		{
			gameObject = list[UnityEngine.Random.Range(0, list.Count)].weaponPrefab;
		}
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("pref == null");
		}
		else
		{
			UnityEngine.Debug.Log("ProfileAnims/" + gameObject.name + "_Profile");
			profile = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.parent = characterInterface.gunPoint.transform;
			weapon = gameObject2;
			weapon.transform.localPosition = Vector3.zero;
			weapon.transform.localRotation = Quaternion.identity;
			if (profile != null)
			{
				weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().AddClip(profile, "Profile");
				weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
			}
			gun = gameObject2.GetComponent<WeaponSounds>().bonusPrefab;
			WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(gameObject2.nameNoClone());
			if (skinForWeapon != null)
			{
				skinForWeapon.SetTo(gameObject2);
			}
			SetCurrentSkin();
		}
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject3 in array)
		{
			if (gameObject3.name.Equals("GunFlash"))
			{
				gameObject3.SetActive(false);
			}
		}
		ResetArmor();
		ShopNGUIController.DisableLightProbesRecursively(characterInterface.gameObject);
	}

	private void SetCurrentSkin()
	{
		characterInterface.SetSkin(SkinsController.currentSkinForPers, (weapon != null) ? weapon.GetComponent<WeaponSounds>() : null);
	}

	public void UpdateWear()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN);
		characterInterface.UpdateCape(@string, null, !ShopNGUIController.ShowWear);
		string string2 = Storager.getString("MaskEquippedSN");
		characterInterface.UpdateMask(string2, !ShopNGUIController.ShowWear);
		string text = Storager.getString(Defs.HatEquppedSN);
		string string3 = Storager.getString(Defs.VisualHatArmor);
		if (!string.IsNullOrEmpty(string3) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(text) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(text) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(string3))
		{
			text = string3;
		}
		characterInterface.UpdateHat(text, !ShopNGUIController.ShowWear);
		string string4 = Storager.getString(Defs.BootsEquppedSN);
		characterInterface.UpdateBoots(string4, !ShopNGUIController.ShowWear);
		ShopNGUIController.SetPersHatVisible(characterInterface.hatPoint);
		ResetArmor();
	}

	private void ResetArmor()
	{
		string text = Storager.getString(Defs.ArmorNewEquppedSN);
		string @string = Storager.getString(Defs.VisualArmor);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(text) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(text) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			text = @string;
		}
		characterInterface.UpdateArmor(text, weapon);
		ShopNGUIController.SetPersArmorVisible(characterInterface.armorPoint);
	}

	private void Update()
	{
		if (!(Camera.main != null))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			RaycastHit hitInfo;
			if (Input.GetTouch(i).phase == TouchPhase.Began && Physics.Raycast(ray, out hitInfo, 1000f, -5) && hitInfo.collider.gameObject.name.Equals("MainMenu_Pers"))
			{
				PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 1);
				ConnectScene.GoToProfile();
				break;
			}
		}
	}

	private void OnDestroy()
	{
		if (ShopNGUIController.sharedShop != null)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
		}
		if (profile != null)
		{
			Resources.UnloadAsset(profile);
		}
		ShopNGUIController.ShowArmorChanged -= UpdateWear;
		ShopNGUIController.ShowWearChanged -= UpdateWear;
		currentConfigurator = null;
	}
}
