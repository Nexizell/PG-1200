using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CUpdatePetCoroutine_003Ed__105 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CharacterInterface _003C_003E4__this;

		private ResourceRequest _003Crequest_003E5__1;

		private string _003CcurrentPet_003E5__2;

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
		public _003CUpdatePetCoroutine_003Ed__105(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			GameObject gameObject;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				_003CcurrentPet_003E5__2 = _003C_003E4__this.idPetForSetInCoroutine;
				if (!(_003C_003E4__this.myPet == null) && !(_003C_003E4__this.myPet.nameNoClone() != Singleton<PetsManager>.Instance.GetFirstUpgrade(_003CcurrentPet_003E5__2).Id))
				{
					goto IL_028f;
				}
				string relativePrefabPath = Singleton<PetsManager>.Instance.GetRelativePrefabPath(_003CcurrentPet_003E5__2);
				gameObject = null;
				_003Crequest_003E5__1 = Resources.LoadAsync<GameObject>(relativePrefabPath);
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			gameObject = (GameObject)_003Crequest_003E5__1.asset;
			if (gameObject == null)
			{
				UnityEngine.Debug.LogErrorFormat("[PETS] prefab for pet '{0}' not found", _003CcurrentPet_003E5__2);
				return false;
			}
			if (_003C_003E4__this.myPet != null)
			{
				_003C_003E4__this.myPet.Destroy();
				_003C_003E4__this.myPet = null;
			}
			_003C_003E4__this.myPet = UnityEngine.Object.Instantiate(gameObject);
			Behaviour[] components = _003C_003E4__this.myPet.GetComponents<Behaviour>();
			foreach (Behaviour behaviour in components)
			{
				if (behaviour.GetType() != typeof(Transform))
				{
					behaviour.enabled = false;
				}
			}
			if (_003C_003E4__this.myPet.GetComponent<PetClickHandler>() == null)
			{
				_003C_003E4__this.myPet.AddComponent<PetClickHandler>();
			}
			else
			{
				_003C_003E4__this.myPet.GetComponent<PetClickHandler>().enabled = true;
			}
			if (_003C_003E4__this.gameObject.Ancestors().Any((GameObject d) => d.name == "MainMenu_Pers_MainMenu"))
			{
				PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(_003CcurrentPet_003E5__2);
				if (playerPet != null)
				{
					PetIndicationController component = _003C_003E4__this.myPet.GetComponent<PetIndicationController>();
					component.enabled = true;
					component.isUpdateNameFromInfo = true;
					component.IconObject.SetActive(false);
					_003C_003E4__this.myPet.GetComponent<PetEngine>().IsMine = true;
					component.LabelObj.SetActive(true);
					component.TextMesh.text = playerPet.PetName;
					component.CreateFrameLabel();
				}
			}
			else
			{
				PetIndicationController component2 = _003C_003E4__this.myPet.GetComponent<PetIndicationController>();
				component2.IconObject.SetActive(false);
				component2.LabelObj.SetActive(false);
			}
			_003Crequest_003E5__1 = null;
			goto IL_028f;
			IL_028f:
			_003C_003E4__this.myPet.GetComponent<PetEngine>().SetInfo(_003CcurrentPet_003E5__2);
			_003C_003E4__this.myPet.transform.position = ((_003C_003E4__this.myPet.GetComponent<FlyingPetEngine>() != null) ? _003C_003E4__this.posFlying : _003C_003E4__this.posGroundPet);
			_003C_003E4__this.myPet.transform.rotation = ((_003C_003E4__this.myPet.GetComponent<FlyingPetEngine>() != null) ? _003C_003E4__this.rotFlying : _003C_003E4__this.rotGroundPet);
			Tools.SetLayerRecursively(_003C_003E4__this.myPet, _003C_003E4__this.gameObject.layer);
			if (!_003C_003E4__this.useLightprobes)
			{
				ShopNGUIController.DisableLightProbesRecursively(_003C_003E4__this.myPet);
			}
			_003C_003E4__this.myPet.SetActive(_003C_003E4__this.enabled);
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

	public Transform armorPoint;

	public Transform leftBootPoint;

	public Transform rightBootPoint;

	public Transform capePoint;

	public Transform hatPoint;

	public Transform maskPoint;

	public Transform gunPoint;

	public Transform pointArmRight;

	public Transform pointArmLeft;

	public Transform pointFlyingPet;

	public Transform pointGroundPet;

	public Transform pointFlyingPetArmory;

	public Transform pointGroundPetArmory;

	public Transform pointFlyingPetProfile;

	public Transform pointGroundPetProfile;

	public Transform flyPointDuelLeft;

	public Transform groundPointDuelLeft;

	public Transform flyPointDuelRight;

	public Transform groundPointDuelRight;

	private Vector3 posGroundPet;

	private Vector3 posFlying;

	private Quaternion rotGroundPet;

	private Quaternion rotFlying;

	public Animation animation;

	public GameObject leftHand;

	public GameObject rightHand;

	public GameObject simpleCharacter;

	public GameObject skinCharacter;

	public Material shopMaterial;

	public Renderer[] renderers;

	public BoxCollider[] colliders;

	public float scaleShop = 150f;

	public Vector3 positionShop;

	public Vector3 rotationShop;

	public int shopTier = 10;

	public GameObject weapon;

	public GameObject gun;

	public Texture skinForPers;

	public bool useLightprobes;

	public bool usePetFromStorager = true;

	public bool enemyInDuel;

	public bool isDuelInstance;

	private bool isInArmory;

	private Vector3 deltaPosFromGroundPet = Vector3.zero;

	private string idPetForSetInCoroutine;

	public string CurrentCapeId { get; private set; }

	public Texture CurrentCapeTexture { get; private set; }

	public string CurrentBootsId { get; private set; }

	public string CurrentHatId { get; private set; }

	public string CurrentMaskId { get; private set; }

	public GameObject myPet { get; private set; }

	public PetEngine myPetEngine
	{
		get
		{
			if (!(myPet != null))
			{
				return null;
			}
			return myPet.GetComponent<PetEngine>();
		}
	}

	public void SetSimpleCharacter()
	{
		simpleCharacter.SetActive(true);
		skinCharacter.SetActive(false);
	}

	public void SetCharacterType(bool haveHands, bool staticAnim, bool forSkinsMaker)
	{
		if (!haveHands)
		{
			SetHandsVisible(false);
		}
		PlayAnimation(staticAnim ? "default" : "Idle");
		if (forSkinsMaker)
		{
			SetShopMaterial();
			ActivateColliders();
		}
	}

	private void SetHandsVisible(bool show)
	{
		leftHand.SetActive(show);
		rightHand.SetActive(show);
	}

	private void PlayAnimation(string anim)
	{
		animation.clip = animation.GetClip(anim);
		animation.Play();
	}

	private void SetShopMaterial()
	{
		Renderer[] array = renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = shopMaterial;
		}
	}

	private void ActivateColliders()
	{
		BoxCollider[] array = colliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		PetsManager.OnPetsUpdated += PetsSynchronizer_OnPetsUpdated;
		if (usePetFromStorager)
		{
			UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
		else if (isDuelInstance && myPet == null && !string.IsNullOrEmpty(idPetForSetInCoroutine))
		{
			UpdatePet(idPetForSetInCoroutine);
		}
		if (myPet != null)
		{
			myPet.SetActive(true);
		}
		if (!(weapon == null) && (bool)weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().GetClip("Profile"))
		{
			weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
		}
	}

	private void PetsSynchronizer_OnPetsUpdated()
	{
		if (usePetFromStorager)
		{
			UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
	}

	private void Start()
	{
		if (base.transform.root.gameObject.nameNoClone() == "ShopNGUI")
		{
			isInArmory = true;
			posFlying = pointFlyingPetArmory.position;
			posGroundPet = pointGroundPetArmory.position;
			rotFlying = pointFlyingPetArmory.rotation;
			rotGroundPet = pointGroundPetArmory.rotation;
			deltaPosFromGroundPet = posGroundPet - base.transform.position;
		}
		else if (isDuelInstance)
		{
			if (enemyInDuel)
			{
				posGroundPet = groundPointDuelRight.position;
				rotGroundPet = groundPointDuelRight.rotation;
				posFlying = flyPointDuelRight.position;
				rotFlying = flyPointDuelRight.rotation;
			}
			else
			{
				posGroundPet = groundPointDuelLeft.position;
				rotGroundPet = groundPointDuelLeft.rotation;
				posFlying = flyPointDuelLeft.position;
				rotFlying = flyPointDuelLeft.rotation;
			}
		}
		else
		{
			isInArmory = false;
			if (base.transform.root.gameObject.nameNoClone() == "ProfileGui")
			{
				posGroundPet = pointGroundPetProfile.position;
				rotGroundPet = pointGroundPetProfile.rotation;
				posFlying = pointFlyingPetProfile.position;
				rotFlying = pointFlyingPetProfile.rotation;
			}
			else
			{
				posGroundPet = pointGroundPet.position;
				rotGroundPet = pointGroundPet.rotation;
				posFlying = pointFlyingPet.position;
				rotFlying = pointFlyingPet.rotation;
			}
			rotFlying = pointFlyingPet.rotation;
			rotGroundPet = pointGroundPet.rotation;
		}
		if (usePetFromStorager)
		{
			ShopNGUIController.EquippedPet += EquippedPet;
			ShopNGUIController.UnequippedPet += UnequipPet;
			UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
	}

	private void Update()
	{
		if (isInArmory && myPet != null && myPet.GetComponent<GroundPetEngine>() != null)
		{
			myPet.transform.position = base.transform.position + deltaPosFromGroundPet;
		}
	}

	private void OnDisable()
	{
		PetsManager.OnPetsUpdated -= PetsSynchronizer_OnPetsUpdated;
		if (myPet != null)
		{
			myPet.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		ShopNGUIController.EquippedPet -= EquippedPet;
		ShopNGUIController.UnequippedPet -= UnequipPet;
	}

	public void UpdateHat(string hat, bool isInvisible = false)
	{
		if (hat.IsNullOrEmpty())
		{
			hat = Defs.HatNoneEqupped;
		}
		CurrentHatId = hat;
		RemoveHat();
		if (hat.Equals(Defs.HatNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Hats/" + hat) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("hatPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		if (!useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		Transform transform2 = gameObject2.transform;
		gameObject2.transform.parent = hatPoint.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
		if (base.transform.parent != null)
		{
			Player_move_c.SetLayerRecursively(gameObject2, base.transform.parent.gameObject.layer);
		}
	}

	public void RemoveHat()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < hatPoint.childCount; i++)
		{
			list.Add(hatPoint.GetChild(i));
		}
		foreach (Transform item in list)
		{
			item.parent = null;
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void UpdateCape(string cape, Texture capeTex = null, bool isInvisible = false)
	{
		if (cape.IsNullOrEmpty())
		{
			cape = Defs.CapeNoneEqupped;
		}
		CurrentCapeId = cape;
		CurrentCapeTexture = capeTex;
		RemoveCape();
		if (cape.Equals(Defs.CapeNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load(ResPath.Combine(Defs.CapesDir, cape)) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("capePrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		if (!useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		gameObject2.transform.parent = capePoint.transform;
		gameObject2.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		if (capeTex != null)
		{
			gameObject2.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
			Player_move_c.SetTextureRecursivelyFrom(gameObject2, capeTex, new GameObject[0]);
		}
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	public void RemoveCape()
	{
		for (int i = 0; i < capePoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(capePoint.transform.GetChild(i).gameObject);
		}
	}

	public void UpdateMask(string mask, bool isInvisible = false)
	{
		if (mask.IsNullOrEmpty())
		{
			mask = "MaskNoneEquipped";
		}
		CurrentMaskId = mask;
		RemoveMask();
		if (mask.Equals("MaskNoneEquipped"))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Masks/" + mask) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("maskPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		if (!useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		gameObject2.transform.parent = maskPoint.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	public void RemoveMask()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < maskPoint.childCount; i++)
		{
			list.Add(maskPoint.GetChild(i));
		}
		foreach (Transform item in list)
		{
			item.parent = null;
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void UpdateBoots(string bs, bool isInvisible = false)
	{
		if (bs.IsNullOrEmpty())
		{
			bs = Defs.BootsNoneEqupped;
		}
		CurrentBootsId = bs;
		RemoveBoots();
		if (bs.Equals(Defs.BootsNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Boots/BootPrefab") as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.SetParent(leftBootPoint.transform, false);
			gameObject3.transform.SetParent(rightBootPoint.transform, false);
			gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
			Player_move_c.SetLayerRecursively(gameObject3, base.gameObject.layer);
			gameObject2.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[bs]);
			gameObject3.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[bs]);
			if (isInvisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject3.transform, false);
			}
		}
	}

	public void RemoveBoots()
	{
		foreach (Transform item in leftBootPoint.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		foreach (Transform item2 in rightBootPoint.transform)
		{
			UnityEngine.Object.Destroy(item2.gameObject);
		}
	}

	public void UpdateArmor(string armor, bool isInvisible)
	{
		UpdateArmor(armor, pointArmLeft, pointArmRight, isInvisible);
	}

	public void UpdateArmor(string armor, GameObject _weapon, bool isInvisible)
	{
		WeaponSounds component = _weapon.GetComponent<WeaponSounds>();
		UpdateArmor(armor, component.LeftArmorHand, component.RightArmorHand, isInvisible);
	}

	public void UpdateArmor(string armor)
	{
		UpdateArmor(armor, pointArmLeft, pointArmRight);
	}

	public void UpdateArmor(string armor, GameObject _weapon)
	{
		WeaponSounds component = _weapon.GetComponent<WeaponSounds>();
		UpdateArmor(armor, component.LeftArmorHand, component.RightArmorHand);
	}

	public void UpdateArmor(string armor, Transform leftArmorHand, Transform rightArmorHand, bool isInvisible = false)
	{
		RemoveArmor();
		if (armor.Equals(Defs.ArmorNewNoneEqupped))
		{
			return;
		}
		string @string = Storager.getString(Defs.VisualArmor);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			armor = @string;
		}
		GameObject gameObject = Resources.Load("Armor/" + armor) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("armorPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		if (!useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		ArmorRefs component = gameObject2.transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (component.leftBone != null && leftArmorHand != null)
			{
				component.leftBone.parent = leftArmorHand;
				component.leftBone.localPosition = Vector3.zero;
				component.leftBone.localRotation = Quaternion.identity;
				component.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component.rightBone != null && rightArmorHand != null)
			{
				component.rightBone.parent = rightArmorHand;
				component.rightBone.localPosition = Vector3.zero;
				component.rightBone.localRotation = Quaternion.identity;
				component.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
			gameObject2.transform.parent = armorPoint.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		}
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	public void RemoveArmor()
	{
		if (armorPoint.childCount <= 0)
		{
			return;
		}
		Transform child = armorPoint.GetChild(0);
		ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (component.leftBone != null)
			{
				component.leftBone.parent = child.GetChild(0);
			}
			if (component.rightBone != null)
			{
				component.rightBone.parent = child.GetChild(0);
			}
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	public void SetSkin(Texture skin, WeaponSounds weapon = null, GadgetArmoryItem gadget = null)
	{
		if (skin == null)
		{
			return;
		}
		skin.filterMode = FilterMode.Point;
		skinForPers = skin;
		List<GameObject> list = new List<GameObject>(new GameObject[6] { capePoint.gameObject, hatPoint.gameObject, leftBootPoint.gameObject, rightBootPoint.gameObject, armorPoint.gameObject, maskPoint.gameObject });
		if (weapon != null)
		{
			if (weapon.LeftArmorHand != null)
			{
				list.Add(weapon.LeftArmorHand.gameObject);
			}
			if (weapon.RightArmorHand != null)
			{
				list.Add(weapon.RightArmorHand.gameObject);
			}
			if (weapon.grenatePoint != null)
			{
				list.Add(weapon.grenatePoint.gameObject);
			}
			if (weapon.bonusPrefab != null)
			{
				list.Add(weapon.bonusPrefab);
			}
			List<GameObject> listWeaponAnimEffects = weapon.GetListWeaponAnimEffects();
			if (listWeaponAnimEffects != null)
			{
				list.AddRange(listWeaponAnimEffects);
			}
		}
		if (gadget != null)
		{
			if (gadget.gadgetPoint != null)
			{
				list.Add(gadget.gadgetPoint);
			}
			if (gadget.noFillPersSkinObjects != null)
			{
				list.AddRange(gadget.noFillPersSkinObjects);
			}
			if (gadget.LeftArmorHand != null)
			{
				list.Add(gadget.LeftArmorHand.gameObject);
			}
			if (gadget.RightArmorHand != null)
			{
				list.Add(gadget.RightArmorHand.gameObject);
			}
		}
		Player_move_c.SetTextureRecursivelyFrom(base.gameObject, skin, list.ToArray());
	}

	public void SetWeapon(string weaponName)
	{
		SetWeapon(weaponName, weaponName, "");
	}

	public void SetWeapon(string weaponName, string alternativeWeapons)
	{
		SetWeapon(weaponName, alternativeWeapons, "");
	}

	public void SetWeapon(string weaponName, string alternativeWeapons, string skinId)
	{
		if (armorPoint.childCount > 0)
		{
			ArmorRefs component = armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = armorPoint.GetChild(0).GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = armorPoint.GetChild(0).GetChild(0);
				}
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (Transform item in gunPoint)
		{
			list.Add(item);
		}
		foreach (Transform item2 in list)
		{
			item2.parent = null;
			UnityEngine.Object.Destroy(item2.gameObject);
		}
		if (weaponName != null && alternativeWeapons != null && WeaponManager.Removed150615_PrefabNames.Contains(weaponName))
		{
			weaponName = alternativeWeapons;
		}
		GameObject gameObject = Resources.Load("Weapons/" + weaponName) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("pref == null");
			return;
		}
		UnityEngine.Debug.Log("ProfileAnims/" + gameObject.name + "_Profile");
		AnimationClip animationClip = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		gameObject2.transform.parent = gunPoint.transform;
		weapon = gameObject2;
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localRotation = Quaternion.identity;
		if (animationClip != null)
		{
			weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().AddClip(animationClip, "Profile");
			weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
		}
		gun = gameObject2.GetComponent<WeaponSounds>().bonusPrefab;
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject3 in array)
		{
			if (gameObject3.name.Equals("GunFlash"))
			{
				gameObject3.SetActive(false);
			}
		}
		SetSkin(skinForPers, weapon.GetComponent<WeaponSounds>());
		if (!skinId.IsNullOrEmpty() && gameObject2 != null)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin != null)
			{
				skin.SetTo(gameObject2);
			}
		}
		if (armorPoint.childCount == 0)
		{
			return;
		}
		WeaponSounds component2 = gameObject2.GetComponent<WeaponSounds>();
		ArmorRefs component3 = armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
		if (component3 != null)
		{
			if (component3.leftBone != null && component2.LeftArmorHand != null)
			{
				component3.leftBone.parent = component2.LeftArmorHand;
				component3.leftBone.localPosition = Vector3.zero;
				component3.leftBone.localRotation = Quaternion.identity;
				component3.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component3.rightBone != null && component2.RightArmorHand != null)
			{
				component3.rightBone.parent = component2.RightArmorHand;
				component3.rightBone.localPosition = Vector3.zero;
				component3.rightBone.localRotation = Quaternion.identity;
				component3.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	public void EquippedPet(string newPet, string oldPet)
	{
		UpdatePet(newPet);
	}

	public void UnequipPet(string oldPet)
	{
		UpdatePet();
	}

	public void UpdatePet(string _petId = "")
	{
		string text = (idPetForSetInCoroutine = ((string.IsNullOrEmpty(_petId) && usePetFromStorager) ? Singleton<PetsManager>.Instance.GetEqipedPetId() : _petId));
		if (!base.gameObject.activeInHierarchy)
		{
			myPet.Destroy();
			myPet = null;
		}
		else if (_petId.IsNullOrEmpty() && myPet != null)
		{
			myPet.Destroy();
			myPet = null;
		}
		else if (string.IsNullOrEmpty(text) && myPet != null)
		{
			myPet.Destroy();
			myPet = null;
		}
		else if (!text.IsNullOrEmpty())
		{
			StopCoroutine("UpdatePetCoroutine");
			StartCoroutine("UpdatePetCoroutine");
		}
	}

	private IEnumerator UpdatePetCoroutine()
	{
		string currentPet = idPetForSetInCoroutine;
		if (myPet == null || myPet.nameNoClone() != Singleton<PetsManager>.Instance.GetFirstUpgrade(currentPet).Id)
		{
			string relativePrefabPath = Singleton<PetsManager>.Instance.GetRelativePrefabPath(currentPet);
			ResourceRequest request = Resources.LoadAsync<GameObject>(relativePrefabPath);
			while (!request.isDone)
			{
				yield return null;
			}
			GameObject gameObject = (GameObject)request.asset;
			if (gameObject == null)
			{
				UnityEngine.Debug.LogErrorFormat("[PETS] prefab for pet '{0}' not found", currentPet);
				yield break;
			}
			if (myPet != null)
			{
				myPet.Destroy();
				myPet = null;
			}
			myPet = UnityEngine.Object.Instantiate(gameObject);
			Behaviour[] components = myPet.GetComponents<Behaviour>();
			foreach (Behaviour behaviour in components)
			{
				if (behaviour.GetType() != typeof(Transform))
				{
					behaviour.enabled = false;
				}
			}
			if (myPet.GetComponent<PetClickHandler>() == null)
			{
				myPet.AddComponent<PetClickHandler>();
			}
			else
			{
				myPet.GetComponent<PetClickHandler>().enabled = true;
			}
			if (this.gameObject.Ancestors().Any((GameObject d) => d.name == "MainMenu_Pers_MainMenu"))
			{
				PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(currentPet);
				if (playerPet != null)
				{
					PetIndicationController component = myPet.GetComponent<PetIndicationController>();
					component.enabled = true;
					component.isUpdateNameFromInfo = true;
					component.IconObject.SetActive(false);
					myPet.GetComponent<PetEngine>().IsMine = true;
					component.LabelObj.SetActive(true);
					component.TextMesh.text = playerPet.PetName;
					component.CreateFrameLabel();
				}
			}
			else
			{
				PetIndicationController component2 = myPet.GetComponent<PetIndicationController>();
				component2.IconObject.SetActive(false);
				component2.LabelObj.SetActive(false);
			}
		}
		myPet.GetComponent<PetEngine>().SetInfo(currentPet);
		myPet.transform.position = ((myPet.GetComponent<FlyingPetEngine>() != null) ? posFlying : posGroundPet);
		myPet.transform.rotation = ((myPet.GetComponent<FlyingPetEngine>() != null) ? rotFlying : rotGroundPet);
		Tools.SetLayerRecursively(myPet, this.gameObject.layer);
		if (!useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(myPet);
		}
		myPet.SetActive(enabled);
	}
}
