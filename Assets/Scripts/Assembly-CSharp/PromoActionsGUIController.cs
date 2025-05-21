using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class PromoActionsGUIController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsGUIController _003C_003E4__this;

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
		public _003CStart_003Ed__9(int _003C_003E1__state)
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
				PromoActionsManager.ActionsUUpdated += _003C_003E4__this.UpdateAfterDelayHandler;
				ShopNGUIController.GunBought += _003C_003E4__this.MarkUpdateOnEnable;
				WeaponManager.TryGunExpired += _003C_003E4__this.MarkUpdateOnEnable;
				StickersController.onBuyPack += _003C_003E4__this.MarkUpdateOnEnable;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.initiallyUpdated = true;
				return false;
			}
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

	[CompilerGenerated]
	internal sealed class _003CUpdateAfterDelay_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsGUIController _003C_003E4__this;

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
		public _003CUpdateAfterDelay_003Ed__13(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.HandleUpdated();
				return false;
			}
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

	[CompilerGenerated]
	internal sealed class _003CHandleUpdateCoroutine_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PromoActionsGUIController _003C_003E4__this;

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
		public _003CHandleUpdateCoroutine_003Ed__21(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return false;
				}
				PromoActionPreview[] componentsInChildren = _003C_003E4__this.wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
				foreach (PromoActionPreview obj in componentsInChildren)
				{
					obj.transform.parent = null;
					UnityEngine.Object.Destroy(obj.gameObject);
				}
				_003C_003E4__this.wrapContent.SortAlphabetically();
				if (!TrainingController.TrainingCompleted)
				{
					if (_003C_003E4__this.fonPromoPanel.activeSelf)
					{
						_003C_003E4__this.fonPromoPanel.SetActive(false);
					}
					return false;
				}
				List<string> list = new List<string>();
				List<string> news = PromoActionsManager.sharedManager.news;
				List<string> topSellers = PromoActionsManager.sharedManager.topSellers;
				List<string> list2 = PromoActionsManager.sharedManager.discounts.Keys.Union(WeaponManager.sharedManager.TryGunPromos.Keys).ToList();
				List<string> list3 = news.Except(FilterPurchases(news)).Random(5).ToList();
				list.AddRange(list3);
				List<string> list4 = topSellers.Except(list).ToList();
				list4 = list4.Except(FilterPurchases(list4)).Random(2).ToList();
				list.AddRange(list4);
				List<string> list5 = list2.Except(list).ToList();
				int count = 8 - list3.Count;
				list5 = list5.Except(FilterPurchases(list5)).Random(count).ToList();
				list.AddRange(list5);
				Dictionary<string, PromoActionMenu> dictionary = new Dictionary<string, PromoActionMenu>();
				foreach (string item in list)
				{
					PromoActionMenu promoActionMenu = new PromoActionMenu
					{
						tg = item
					};
					if (news.Contains(promoActionMenu.tg))
					{
						promoActionMenu.isNew = true;
					}
					if (topSellers.Contains(promoActionMenu.tg))
					{
						promoActionMenu.isTopSeller = true;
					}
					if (list2.Contains(promoActionMenu.tg))
					{
						promoActionMenu.isDiscounted = true;
						try
						{
							bool onlyServerDiscount;
							promoActionMenu.discount = ShopNGUIController.DiscountFor(promoActionMenu.tg, out onlyServerDiscount);
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError("Exception in pam.discount = ShopNGUIController.DiscountFor(key,out unused): " + ex);
						}
						promoActionMenu.price = ShopNGUIController.GetItemPrice(promoActionMenu.tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(promoActionMenu.tg)).Price;
					}
					dictionary.Add(item, promoActionMenu);
				}
				string key_ = LocalizationStore.Key_0419;
				foreach (string key in dictionary.Keys)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("PromoAction") as GameObject);
					gameObject.transform.parent = _003C_003E4__this.wrapContent.transform;
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					PromoActionPreview component2 = gameObject.GetComponent<PromoActionPreview>();
					ItemPrice itemPrice = null;
					if (!(key == "StickersPromoActionsPanelKey"))
					{
						string text = ItemDb.GetShopIdByTag(key) ?? key;
						if (!string.IsNullOrEmpty(text))
						{
							itemPrice = ItemDb.GetPriceByShopId(text, (ShopNGUIController.CategoryNames)(-1));
						}
					}
					if (dictionary[key].isDiscounted)
					{
						component2.sale.gameObject.SetActive(true);
						component2.Discount = dictionary[key].discount;
						component2.sale.text = string.Format("{0}\n{1}%", new object[2]
						{
							key_,
							dictionary[key].discount
						});
						component2.coins.text = dictionary[key].price.ToString();
					}
					else if (itemPrice != null)
					{
						component2.coins.text = itemPrice.Price.ToString();
					}
					if (itemPrice != null)
					{
						component2.currencyImage.spriteName = (itemPrice.Currency.Equals("Coins") ? "ingame_coin" : "gem_znachek");
						component2.currencyImage.width = (itemPrice.Currency.Equals("Coins") ? 30 : 34);
						component2.currencyImage.height = (itemPrice.Currency.Equals("Coins") ? 30 : 24);
						component2.coins.color = (itemPrice.Currency.Equals("Coins") ? new Color(1f, 0.8627f, 0f) : new Color(0.3176f, 0.8117f, 1f));
					}
					else
					{
						component2.coins.gameObject.SetActive(false);
						component2.currencyImage.gameObject.SetActive(false);
					}
					if (key == "StickersPromoActionsPanelKey")
					{
						component2.stickersLabel.SetActive(true);
					}
					component2.topSeller.gameObject.SetActive(dictionary[key].isTopSeller);
					component2.newItem.gameObject.SetActive(dictionary[key].isNew);
					if (key == "StickersPromoActionsPanelKey")
					{
						component2.button.tweenTarget = component2.stickerTexture.gameObject;
						component2.icon.mainTexture = null;
						component2.icon = component2.stickerTexture;
						component2.pressed = component2.stickerTexture.mainTexture;
						component2.unpressed = component2.stickerTexture.mainTexture;
					}
					else
					{
						int itemCategory = ItemDb.GetItemCategory(key);
						Texture itemIcon = ItemDb.GetItemIcon(key, (ShopNGUIController.CategoryNames)itemCategory);
						if (itemIcon != null)
						{
							component2.unpressed = itemIcon;
							component2.icon.mainTexture = itemIcon;
						}
						if (itemIcon != null)
						{
							component2.pressed = itemIcon;
						}
					}
					component2.tg = key;
				}
				_003C_003E4__this.noOffersLabel.gameObject.SetActive(dictionary.Count == 0 && PromoActionsManager.ActionsAvailable);
				_003C_003E4__this.checkInternetLabel.gameObject.SetActive(dictionary.Count == 0 && !PromoActionsManager.ActionsAvailable);
				if (_003C_003E4__this.fonPromoPanel.activeSelf != (dictionary.Count != 0))
				{
					_003C_003E4__this.fonPromoPanel.SetActive(dictionary.Count != 0);
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				PromoActionPreview[] array = _003C_003E4__this.wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
				if (array == null)
				{
					array = new PromoActionPreview[0];
				}
				Comparison<PromoActionPreview> comparison = delegate(PromoActionPreview pap1, PromoActionPreview pap2)
				{
					int num2 = 0;
					int num3 = 0;
					if (pap1.tg == "StickersPromoActionsPanelKey")
					{
						num2 += 1000;
					}
					if (pap2.tg == "StickersPromoActionsPanelKey")
					{
						num3 += 1000;
					}
					if (pap1.newItem.gameObject.activeSelf)
					{
						num2 += 100;
					}
					if (pap2.newItem.gameObject.activeSelf)
					{
						num3 += 100;
					}
					if (pap1.topSeller.gameObject.activeSelf)
					{
						num2 += 50;
					}
					if (pap2.topSeller.gameObject.activeSelf)
					{
						num3 += 50;
					}
					if (pap1.sale.gameObject.activeSelf)
					{
						num2 += 10;
					}
					if (pap2.sale.gameObject.activeSelf)
					{
						num3 += 10;
					}
					return num3 - num2;
				};
				Array.Sort(array, comparison);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.name = i.ToString("D7");
				}
				_003C_003E4__this.wrapContent.SortAlphabetically();
				_003C_003E4__this.wrapContent.WrapContent();
				Transform transform = null;
				if (array.Length != 0)
				{
					transform = array[0].transform;
				}
				if (transform != null)
				{
					float num = transform.localPosition.x - 9f;
					Transform parent = _003C_003E4__this.wrapContent.transform.parent;
					if (parent != null)
					{
						UIPanel component = parent.GetComponent<UIPanel>();
						if (component != null)
						{
							component.clipOffset = new Vector2(num, component.clipOffset.y);
							parent.localPosition = new Vector3(0f - num, parent.localPosition.y, parent.localPosition.z);
						}
					}
				}
				_003C_003E4__this.wrapContent.SortAlphabetically();
				_003C_003E4__this.wrapContent.WrapContent();
				_003C_003E4__this.wrapContent.transform.parent.GetComponent<UIScrollView>().enabled = _003C_003E4__this.wrapContent.transform.childCount <= 0 || _003C_003E4__this.wrapContent.transform.childCount > 3;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.wrapContent.SortAlphabetically();
				_003C_003E4__this.wrapContent.WrapContent();
				_003C_003E4__this.wrapContent.transform.GetComponent<MyCenterOnChild>().Recenter();
				return false;
			}
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

	public const int ITEMS_COUNT_NEWS = 5;

	public const int ITEMS_COUNT_TOPSELLER = 2;

	public const int ITEMS_COUNT_DISCOUNT = 3;

	public const string StickersPromoActionsPanelKey = "StickersPromoActionsPanelKey";

	public UIWrapContent wrapContent;

	public UILabel noOffersLabel;

	public UILabel checkInternetLabel;

	public GameObject fonPromoPanel;

	private bool initiallyUpdated;

	private bool updateOnEnable;

	private int refreshPromoPanelCntr;

	private IEnumerator Start()
	{
		PromoActionsManager.ActionsUUpdated += UpdateAfterDelayHandler;
		ShopNGUIController.GunBought += MarkUpdateOnEnable;
		WeaponManager.TryGunExpired += MarkUpdateOnEnable;
		StickersController.onBuyPack += MarkUpdateOnEnable;
		yield return null;
		initiallyUpdated = true;
	}

	private void OnEnable()
	{
		if (updateOnEnable || !initiallyUpdated)
		{
			StartCoroutine(UpdateAfterDelay());
		}
		updateOnEnable = false;
	}

	private void UpdateAfterDelayHandler()
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(UpdateAfterDelay());
		}
	}

	private IEnumerator UpdateAfterDelay()
	{
		yield return null;
		HandleUpdated();
	}

	private void OnDestroy()
	{
		PromoActionsManager.ActionsUUpdated -= UpdateAfterDelayHandler;
		ShopNGUIController.GunBought -= MarkUpdateOnEnable;
		WeaponManager.TryGunExpired -= MarkUpdateOnEnable;
		StickersController.onBuyPack -= MarkUpdateOnEnable;
	}

	private void Update()
	{
		Transform transform = wrapContent.transform;
		if (transform.childCount <= 0)
		{
			return;
		}
		UIPanel component = transform.parent.GetComponent<UIPanel>();
		if (transform.childCount <= 3)
		{
			float num = 0f;
			foreach (Transform item in transform)
			{
				num += item.localPosition.x;
			}
			num /= (float)transform.childCount;
			if (component != null)
			{
				wrapContent.WrapContent();
				component.GetComponent<UIScrollView>().SetDragAmount(0.5f, 0f, false);
			}
		}
		if (refreshPromoPanelCntr % 10 == 0)
		{
			component.Refresh();
		}
		refreshPromoPanelCntr++;
	}

	public void MarkUpdateOnEnable()
	{
		updateOnEnable = true;
		if (base.gameObject.activeInHierarchy)
		{
			HandleUpdated();
		}
	}

	private void HandleUpdated()
	{
		StartCoroutine(HandleUpdateCoroutine());
	}

	public static string FilterForLoadings(string tg, List<string> alreadyUsed)
	{
		if (tg == null || alreadyUsed == null)
		{
			return null;
		}
		string text = WeaponManager.FirstUnboughtTag(tg);
		string text2 = "";
		try
		{
			text2 = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text]];
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Exception in FilterForLoadings:  idefFirstUnobught = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnobught] ]:  " + ex);
			return null;
		}
		if (!Storager.hasKey(text2))
		{
			Storager.setInt(text2, 0);
		}
		bool flag = Storager.getInt(text2) > 0;
		WeaponSounds weaponSounds = null;
		UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
		for (int i = 0; i < weaponsInGame.Length; i++)
		{
			GameObject gameObject = (GameObject)weaponsInGame[i];
			if (ItemDb.GetByPrefabName(gameObject.name).Tag.Equals(text))
			{
				weaponSounds = gameObject.GetComponent<WeaponSounds>();
				break;
			}
		}
		if (weaponSounds == null)
		{
			return null;
		}
		if (!flag && weaponSounds.tier <= ExpController.Instance.OurTier && !alreadyUsed.Contains(ItemDb.GetByPrefabName(weaponSounds.name.Replace("(Clone)", "")).Tag))
		{
			return text;
		}
		WeaponSounds weaponSounds2 = weaponSounds;
		if (!flag)
		{
			string text3 = WeaponManager.LastBoughtTag(text);
			if (text3 != null)
			{
				List<string> list = null;
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (upgrade.Contains(text3))
					{
						list = upgrade;
						break;
					}
				}
				for (int num = list.IndexOf(text3); num >= 0; num--)
				{
					bool flag2 = false;
					weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
					for (int i = 0; i < weaponsInGame.Length; i++)
					{
						GameObject gameObject2 = (GameObject)weaponsInGame[i];
						if (ItemDb.GetByPrefabName(gameObject2.name).Tag.Equals(list[num]))
						{
							WeaponSounds component = gameObject2.GetComponent<WeaponSounds>();
							if (component.tier <= ExpController.Instance.OurTier)
							{
								flag2 = true;
								weaponSounds2 = component;
								break;
							}
						}
					}
					if (flag2)
					{
						break;
					}
				}
			}
		}
		float num2 = 1f;
		if (weaponSounds2 != null)
		{
			num2 = weaponSounds2.DamageByTier[weaponSounds2.tier] / weaponSounds2.lengthForShot;
		}
		float initialDPS = num2;
		if (flag && weaponSounds.tier > ExpController.Instance.OurTier && weaponSounds2 != null)
		{
			try
			{
				initialDPS = num2 * (weaponSounds2.DamageByTier[ExpController.Instance.OurTier] / weaponSounds2.DamageByTier[weaponSounds2.tier]);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.Log("Exception in FilterForLoadings:  if (bought && ws.tier > ExpController.Instance.OurTier && lastBoughtInOurTierWS != null):  " + ex2);
			}
		}
		List<string> list2 = new List<string> { tg };
		foreach (List<string> upgrade2 in WeaponUpgrades.upgrades)
		{
			if (upgrade2.Contains(tg))
			{
				list2 = upgrade2;
				break;
			}
		}
		List<string> list3 = new List<string>();
		List<GameObject> list4 = new List<GameObject>();
		weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
		for (int i = 0; i < weaponsInGame.Length; i++)
		{
			GameObject gameObject3 = (GameObject)weaponsInGame[i];
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(gameObject3.name);
			if (list2.Contains(byPrefabName.Tag) || gameObject3.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier || gameObject3.GetComponent<WeaponSounds>().campaignOnly || gameObject3.name.Equals(WeaponManager.AlienGunWN) || gameObject3.name.Equals(WeaponManager.BugGunWN) || gameObject3.name.Equals(WeaponManager.SimpleFlamethrower_WN) || gameObject3.name.Equals(WeaponManager.CampaignRifle_WN) || gameObject3.name.Equals(WeaponManager.Rocketnitza_WN) || gameObject3.name.Equals(WeaponManager.PistolWN) || gameObject3.name.Equals(WeaponManager.SocialGunWN) || gameObject3.name.Equals(WeaponManager.DaterFreeWeaponPrefabName) || gameObject3.name.Equals(WeaponManager.KnifeWN) || gameObject3.name.Equals(WeaponManager.ShotgunWN) || gameObject3.name.Equals(WeaponManager.MP5WN) || ItemDb.IsTemporaryGun(byPrefabName.Tag) || (byPrefabName.Tag != null && WeaponManager.GotchaGuns.Contains(byPrefabName.Tag)))
			{
				continue;
			}
			string text4 = WeaponManager.FirstUnboughtTag(byPrefabName.Tag);
			if (alreadyUsed.Contains(text4) || list3.Contains(text4))
			{
				continue;
			}
			bool flag3 = false;
			UnityEngine.Object[] weaponsInGame2 = WeaponManager.sharedManager.weaponsInGame;
			for (int j = 0; j < weaponsInGame2.Length; j++)
			{
				GameObject gameObject4 = (GameObject)weaponsInGame2[j];
				if (ItemDb.GetByPrefabName(gameObject4.name).Tag.Equals(text4))
				{
					flag3 = gameObject4.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier;
					break;
				}
			}
			if (flag3)
			{
				continue;
			}
			try
			{
				if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text4]]) > 0)
				{
					continue;
				}
			}
			catch (Exception ex3)
			{
				UnityEngine.Debug.Log("Exception in FilterForLoadings:  defFirstUnobughtOther = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnboughtOthers] ]:  " + ex3);
			}
			list3.Add(text4);
			weaponsInGame2 = WeaponManager.sharedManager.weaponsInGame;
			for (int j = 0; j < weaponsInGame2.Length; j++)
			{
				GameObject gameObject5 = (GameObject)weaponsInGame2[j];
				if (ItemDb.GetByPrefabName(gameObject5.name).Tag.Equals(text4))
				{
					list4.Add(gameObject5);
					break;
				}
			}
		}
		list4.Sort(delegate(GameObject go1, GameObject go2)
		{
			float num3 = 1f;
			float num4 = go1.GetComponent<WeaponSounds>().DamageByTier[go1.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			num3 = go2.GetComponent<WeaponSounds>().DamageByTier[go2.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			return (int)(num4 - num3);
		});
		GameObject gameObject6 = list4.Find((GameObject obj) => obj.GetComponent<WeaponSounds>().DamageByTier[obj.GetComponent<WeaponSounds>().tier] / obj.GetComponent<WeaponSounds>().lengthForShot >= initialDPS);
		if (gameObject6 == null)
		{
			gameObject6 = ((list4.Count > 0) ? list4[list4.Count - 1] : null);
		}
		if (!(gameObject6 != null))
		{
			return null;
		}
		return ItemDb.GetByPrefabName(gameObject6.name).Tag;
	}

	public static List<string> FilterPurchases(IEnumerable<string> input, bool filterNextTierUpgrades = false, bool filterWeapons = true, bool filterRentedTempWeapons = false, bool checkWear = true)
	{
		List<string> list2 = new List<string>();
		Dictionary<string, WeaponSounds> dictionary = WeaponManager.sharedManager.weaponsInGame.Select((UnityEngine.Object w) => ((GameObject)w).GetComponent<WeaponSounds>()).ToDictionary((WeaponSounds ws) => ws.name, (WeaponSounds ws) => ws);
		HashSet<string> hashSet = new HashSet<string>(WeaponManager.sharedManager.FilteredShopListsForPromos.SelectMany((List<GameObject> l) => l.Select((GameObject g) => g.name.Replace("(Clone)", ""))));
		foreach (string item in input)
		{
			if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(item) || item == "Armor_Novice")
			{
				list2.Add(item);
				continue;
			}
			ItemRecord byTag = ItemDb.GetByTag(item);
			bool flag = byTag != null && byTag.TemporaryGun;
			bool flag2 = true;
			if ((byTag == null || !hashSet.Contains(byTag.PrefabName)) && WeaponManager.tagToStoreIDMapping.ContainsKey(item))
			{
				flag2 = false;
			}
			if (filterWeapons && (!flag2 || (flag2 && !flag && WeaponManager.tagToStoreIDMapping.ContainsKey(item) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[item]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[item]]) > 0) || (filterRentedTempWeapons && flag2 && flag && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(item))))
			{
				list2.Add(item);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (checkWear)
			{
				foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> item2 in Wear.wear)
				{
					foreach (List<string> item3 in item2.Value)
					{
						if (item3.Contains(item))
						{
							flag4 = true;
							if (!TempItemsController.PriceCoefs.ContainsKey(item))
							{
								int num = item3.IndexOf(item);
								bool flag5 = Storager.getInt(item) == 0;
								bool flag6 = Wear.TierForWear(item) <= ExpController.OurTierForAnyPlace();
								bool flag7 = Wear.LeagueForWear(item, item2.Key) <= (int)RatingSystem.instance.currentLeague;
								flag3 = ((num == 0 && flag5 && flag6) || (num > 0 && flag5 && Storager.getInt(item3[num - 1]) > 0 && (!filterNextTierUpgrades || flag6))) && flag7;
							}
							break;
						}
					}
				}
			}
			if (!flag3 && (SkinsController.skinsNamesForPers.ContainsKey(item) || item.Equals("CustomSkinID")))
			{
				flag4 = true;
				flag3 = false;
			}
			if (flag4 && !flag3 && !TempItemsController.PriceCoefs.ContainsKey(item))
			{
				list2.Add(item);
			}
			if (WeaponManager.sharedManager == null || ExpController.Instance == null)
			{
				continue;
			}
			WeaponSounds value;
			if (filterWeapons && byTag != null && dictionary.TryGetValue(byTag.PrefabName, out value) && value != null)
			{
				if (value.tier > ExpController.Instance.OurTier)
				{
					list2.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("Sniper") && (!value.IsAvalibleFromFilter(2) || value.name == WeaponManager.PistolWN || value.name == WeaponManager.KnifeWN))
				{
					list2.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("Knife") && value.categoryNabor != 3)
				{
					list2.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("LoveIsland") && !value.IsAvalibleFromFilter(3))
				{
					list2.Add(item);
				}
			}
			if (!flag4 && !WeaponManager.tagToStoreIDMapping.ContainsKey(item))
			{
				list2.Add(item);
			}
			if (TempItemsController.PriceCoefs.ContainsKey(item))
			{
				list2.Add(item);
			}
		}
		try
		{
			if (ShopNGUIController.NoviceArmorAvailable)
			{
				list2 = list2.Union(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory].SelectMany((List<string> list) => list)).ToList();
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in FilterPurchases removing all armor: " + ex);
		}
		return list2;
	}

	private IEnumerator HandleUpdateCoroutine()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			yield break;
		}
		PromoActionPreview[] componentsInChildren = wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
		foreach (PromoActionPreview obj in componentsInChildren)
		{
			obj.transform.parent = null;
			UnityEngine.Object.Destroy(obj.gameObject);
		}
		wrapContent.SortAlphabetically();
		if (!TrainingController.TrainingCompleted)
		{
			if (fonPromoPanel.activeSelf)
			{
				fonPromoPanel.SetActive(false);
			}
			yield break;
		}
		List<string> list = new List<string>();
		List<string> news = PromoActionsManager.sharedManager.news;
		List<string> topSellers = PromoActionsManager.sharedManager.topSellers;
		List<string> list2 = PromoActionsManager.sharedManager.discounts.Keys.Union(WeaponManager.sharedManager.TryGunPromos.Keys).ToList();
		List<string> list3 = news.Except(FilterPurchases(news)).Random(5).ToList();
		list.AddRange(list3);
		List<string> list4 = topSellers.Except(list).ToList();
		list4 = list4.Except(FilterPurchases(list4)).Random(2).ToList();
		list.AddRange(list4);
		List<string> list5 = list2.Except(list).ToList();
		int count = 8 - list3.Count;
		list5 = list5.Except(FilterPurchases(list5)).Random(count).ToList();
		list.AddRange(list5);
		Dictionary<string, PromoActionMenu> dictionary = new Dictionary<string, PromoActionMenu>();
		foreach (string item in list)
		{
			PromoActionMenu promoActionMenu = new PromoActionMenu
			{
				tg = item
			};
			if (news.Contains(promoActionMenu.tg))
			{
				promoActionMenu.isNew = true;
			}
			if (topSellers.Contains(promoActionMenu.tg))
			{
				promoActionMenu.isTopSeller = true;
			}
			if (list2.Contains(promoActionMenu.tg))
			{
				promoActionMenu.isDiscounted = true;
				try
				{
					bool onlyServerDiscount;
					promoActionMenu.discount = ShopNGUIController.DiscountFor(promoActionMenu.tg, out onlyServerDiscount);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in pam.discount = ShopNGUIController.DiscountFor(key,out unused): " + ex);
				}
				promoActionMenu.price = ShopNGUIController.GetItemPrice(promoActionMenu.tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(promoActionMenu.tg)).Price;
			}
			dictionary.Add(item, promoActionMenu);
		}
		string key_ = LocalizationStore.Key_0419;
		foreach (string key in dictionary.Keys)
		{
			GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load("PromoAction") as GameObject);
			obj2.transform.parent = wrapContent.transform;
			obj2.transform.localScale = new Vector3(1f, 1f, 1f);
			PromoActionPreview component = obj2.GetComponent<PromoActionPreview>();
			ItemPrice itemPrice = null;
			if (!(key == "StickersPromoActionsPanelKey"))
			{
				string text = ItemDb.GetShopIdByTag(key) ?? key;
				if (!string.IsNullOrEmpty(text))
				{
					itemPrice = ItemDb.GetPriceByShopId(text, (ShopNGUIController.CategoryNames)(-1));
				}
			}
			if (dictionary[key].isDiscounted)
			{
				component.sale.gameObject.SetActive(true);
				component.Discount = dictionary[key].discount;
				component.sale.text = string.Format("{0}\n{1}%", new object[2]
				{
					key_,
					dictionary[key].discount
				});
				component.coins.text = dictionary[key].price.ToString();
			}
			else if (itemPrice != null)
			{
				component.coins.text = itemPrice.Price.ToString();
			}
			if (itemPrice != null)
			{
				component.currencyImage.spriteName = (itemPrice.Currency.Equals("Coins") ? "ingame_coin" : "gem_znachek");
				component.currencyImage.width = (itemPrice.Currency.Equals("Coins") ? 30 : 34);
				component.currencyImage.height = (itemPrice.Currency.Equals("Coins") ? 30 : 24);
				component.coins.color = (itemPrice.Currency.Equals("Coins") ? new Color(1f, 0.8627f, 0f) : new Color(0.3176f, 0.8117f, 1f));
			}
			else
			{
				component.coins.gameObject.SetActive(false);
				component.currencyImage.gameObject.SetActive(false);
			}
			if (key == "StickersPromoActionsPanelKey")
			{
				component.stickersLabel.SetActive(true);
			}
			component.topSeller.gameObject.SetActive(dictionary[key].isTopSeller);
			component.newItem.gameObject.SetActive(dictionary[key].isNew);
			if (key == "StickersPromoActionsPanelKey")
			{
				component.button.tweenTarget = component.stickerTexture.gameObject;
				component.icon.mainTexture = null;
				component.icon = component.stickerTexture;
				component.pressed = component.stickerTexture.mainTexture;
				component.unpressed = component.stickerTexture.mainTexture;
			}
			else
			{
				int itemCategory = ItemDb.GetItemCategory(key);
				Texture itemIcon = ItemDb.GetItemIcon(key, (ShopNGUIController.CategoryNames)itemCategory);
				if (itemIcon != null)
				{
					component.unpressed = itemIcon;
					component.icon.mainTexture = itemIcon;
				}
				if (itemIcon != null)
				{
					component.pressed = itemIcon;
				}
			}
			component.tg = key;
		}
		noOffersLabel.gameObject.SetActive(dictionary.Count == 0 && PromoActionsManager.ActionsAvailable);
		checkInternetLabel.gameObject.SetActive(dictionary.Count == 0 && !PromoActionsManager.ActionsAvailable);
		if (fonPromoPanel.activeSelf != (dictionary.Count != 0))
		{
			fonPromoPanel.SetActive(dictionary.Count != 0);
		}
		yield return null;
		PromoActionPreview[] array = wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
		if (array == null)
		{
			array = new PromoActionPreview[0];
		}
		Comparison<PromoActionPreview> comparison = delegate(PromoActionPreview pap1, PromoActionPreview pap2)
		{
			int num2 = 0;
			int num3 = 0;
			if (pap1.tg == "StickersPromoActionsPanelKey")
			{
				num2 += 1000;
			}
			if (pap2.tg == "StickersPromoActionsPanelKey")
			{
				num3 += 1000;
			}
			if (pap1.newItem.gameObject.activeSelf)
			{
				num2 += 100;
			}
			if (pap2.newItem.gameObject.activeSelf)
			{
				num3 += 100;
			}
			if (pap1.topSeller.gameObject.activeSelf)
			{
				num2 += 50;
			}
			if (pap2.topSeller.gameObject.activeSelf)
			{
				num3 += 50;
			}
			if (pap1.sale.gameObject.activeSelf)
			{
				num2 += 10;
			}
			if (pap2.sale.gameObject.activeSelf)
			{
				num3 += 10;
			}
			return num3 - num2;
		};
		Array.Sort(array, comparison);
		for (int j = 0; j < array.Length; j++)
		{
			array[j].gameObject.name = j.ToString("D7");
		}
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		Transform transform = null;
		if (array.Length != 0)
		{
			transform = array[0].transform;
		}
		if (transform != null)
		{
			float num = transform.localPosition.x - 9f;
			Transform parent = wrapContent.transform.parent;
			if (parent != null)
			{
				UIPanel component2 = parent.GetComponent<UIPanel>();
				if (component2 != null)
				{
					component2.clipOffset = new Vector2(num, component2.clipOffset.y);
					parent.localPosition = new Vector3(0f - num, parent.localPosition.y, parent.localPosition.z);
				}
			}
		}
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		wrapContent.transform.parent.GetComponent<UIScrollView>().enabled = wrapContent.transform.childCount <= 0 || wrapContent.transform.childCount > 3;
		yield return null;
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		wrapContent.transform.GetComponent<MyCenterOnChild>().Recenter();
	}
}
