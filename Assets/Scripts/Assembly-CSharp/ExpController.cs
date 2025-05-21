using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class ExpController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CHandleShopButtonFromTierPanelCoroutine_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameObject tierPanel;

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
		public _003CHandleShopButtonFromTierPanelCoroutine_003Ed__31(int _003C_003E1__state)
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
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(CurrentFilterMap());
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				ShopNGUIController.sharedShop.resumeAction = null;
				ShopNGUIController.GuiActive = true;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				HideTierPanel(tierPanel);
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
	internal sealed class _003CHandleShopButtonFromNewAvailableItemCoroutine_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ShopNGUIController.CategoryNames category;

		public string itemTag;

		public GameObject tierPanel;

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
		public _003CHandleShopButtonFromNewAvailableItemCoroutine_003Ed__32(int _003C_003E1__state)
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
				if (ShopNGUIController.IsWeaponCategory(category))
				{
					itemTag = WeaponManager.FirstUnboughtOrForOurTier(itemTag) ?? itemTag;
				}
				ShopNGUIController.sharedShop.SetItemToShow(new ShopNGUIController.ShopItem(itemTag, category));
				ShopNGUIController.sharedShop.CategoryToChoose = category;
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				ShopNGUIController.sharedShop.resumeAction = null;
				ShopNGUIController.GuiActive = true;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				HideTierPanel(tierPanel);
				ShopNGUIController.sharedShop.AdjustCategoryGridCells();
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
	internal sealed class _003CWaitAndUpdateExperience_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExpController _003C_003E4__this;

		public bool showLevelUpPanel;

		private bool _003CisTierLevelup_003E5__1;

		private int _003Ctier_003E5__2;

		public int newRank;

		private List<string> _003CitemsToShow_003E5__3;

		private int _003CcoinsReward_003E5__4;

		private int _003CgemsReward_003E5__5;

		public string idOfGivenPreviousTierBestArmor;

		public AudioClip sound;

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
		public _003CWaitAndUpdateExperience_003Ed__47(int _003C_003E1__state)
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
				_003C_003E4__this.WaitingForLevelUpView = showLevelUpPanel;
				_003Ctier_003E5__2 = Array.BinarySearch(LevelsForTiers, ExperienceController.sharedController.currentLevel);
				_003CisTierLevelup_003E5__1 = 0 <= _003Ctier_003E5__2 && _003Ctier_003E5__2 < LevelsForTiers.Length;
				if (_003CisTierLevelup_003E5__1)
				{
					if (PromoActionsManager.sharedManager != null)
					{
						try
						{
							List<string> itemsViewed = WeaponManager.GetNewWeaponsForTier(_003Ctier_003E5__2).Concat(GadgetsInfo.GetNewGadgetsForTier(_003Ctier_003E5__2)).ToList();
							PromoActionsManager.sharedManager.ReplaceUnlockedItemsWith(itemsViewed);
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogErrorFormat("Exception in WaitAndUpdateExperience ReplaceUnlockedItemsWith: {0}", ex);
						}
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat("ShopNguiController.Update: PromoActionsManager.sharedManager == null");
					}
				}
				_003C_003E2__current = new WaitForSeconds(1.2f);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				if (!showLevelUpPanel || !(ExperienceController.sharedController != null))
				{
					break;
				}
				_003CitemsToShow_003E5__3 = new List<string>();
				if (_003CisTierLevelup_003E5__1)
				{
					switch (_003Ctier_003E5__2)
					{
					case 1:
						_003CitemsToShow_003E5__3 = new List<string>
						{
							"Armor_Steel_1",
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Antihero_Rifle_1_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.frank_sheepone_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.AcidCannon_Tag)
						};
						break;
					case 2:
						_003CitemsToShow_003E5__3 = new List<string>
						{
							"Armor_Royal_1",
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.DragonGun_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.charge_rifle_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.loud_piggy_Tag)
						};
						break;
					case 3:
						_003CitemsToShow_003E5__3 = new List<string>
						{
							"Armor_Almaz_1",
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Dark_Matter_Generator_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.autoaim_bazooka_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Photon_Pistol_Tag)
						};
						break;
					case 4:
						_003CitemsToShow_003E5__3 = new List<string>
						{
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.RailRevolverBuy_3_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.RayMinigun_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Autoaim_RocketlauncherBuy_3_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Impulse_Sniper_RifleBuy_3_Tag)
						};
						break;
					case 5:
						_003CitemsToShow_003E5__3 = new List<string>
						{
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.PX_3000_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.StormHammer_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Sunrise_Tag),
							_003C_003E4__this.SubstituteTempGunIfReplaced(WeaponTags.Bastion_Tag)
						};
						break;
					}
					_003C_003E4__this._experienceView.LevelUpPanelOptions.ShowTierView = true;
				}
				else
				{
					_003C_003E4__this._experienceView.LevelUpPanelOptions.ShowTierView = false;
				}
				_003C_003E4__this._experienceView.LevelUpPanelOptions.ShareButtonEnabled = true;
				int num = Math.Max(0, newRank - 1);
				_003CcoinsReward_003E5__4 = ExperienceController.addCoinsFromLevels[num];
				_003CgemsReward_003E5__5 = ExperienceController.addGemsFromLevels[num];
				if (NetworkStartTableNGUIController.sharedController != null)
				{
					_003C_003E4__this._sameSceneIndicator = true;
					goto IL_03d5;
				}
				if (Application.loadedLevelName == "LevelComplete")
				{
					_003C_003E4__this._sameSceneIndicator = true;
					goto IL_044c;
				}
				goto IL_047b;
			}
			case 2:
				_003C_003E1__state = -1;
				goto IL_03d5;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_044c;
				}
				IL_03d5:
				if (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				if (!_003C_003E4__this._sameSceneIndicator || NetworkStartTableNGUIController.sharedController == null)
				{
					_003C_003E4__this.WaitingForLevelUpView = false;
					return false;
				}
				goto IL_047b;
				IL_044c:
				if (_003C_003E4__this._sameSceneIndicator && LevelCompleteScript.IsInterfaceBusy)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				if (!_003C_003E4__this._sameSceneIndicator)
				{
					_003C_003E4__this.WaitingForLevelUpView = false;
					return false;
				}
				goto IL_047b;
				IL_047b:
				_003C_003E4__this._experienceView.LevelUpPanelOptions.NewItems = _003CitemsToShow_003E5__3;
				_003C_003E4__this._experienceView.LevelUpPanelOptions.CurrentRank = newRank;
				_003C_003E4__this._experienceView.LevelUpPanelOptions.CoinsReward = _003CcoinsReward_003E5__4;
				_003C_003E4__this._experienceView.LevelUpPanelOptions.GemsReward = _003CgemsReward_003E5__5;
				_003C_003E4__this._experienceView.LevelUpPanelOptions.IdOfGivenPreviousTierBestArmor = idOfGivenPreviousTierBestArmor;
				_003C_003E4__this._experienceView.ShowLevelUpPanel();
				_003CitemsToShow_003E5__3 = null;
				break;
			}
			_003C_003E4__this.WaitingForLevelUpView = false;
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(sound);
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

	[SerializeField]
	protected internal ExpView _experienceView;

	[SerializeField]
	protected internal RankIndicatorGuiElement _rankIndicator;

	public const int MaxLobbyLevel = 3;

	private bool _starterBannerShowed;

	public static readonly int[] LevelsForTiers = new int[6] { 1, 7, 12, 17, 22, 27 };

	private static ExpController _instance;

	private bool _sameSceneIndicator;

	public bool LevelUpPanelOpened
	{
		get
		{
			if (_experienceView != null)
			{
				return _experienceView.LevelUpPanelOpened;
			}
			return false;
		}
	}

	public Camera ExpCamera
	{
		get
		{
			if (!(_experienceView != null))
			{
				return null;
			}
			return _experienceView.experienceCamera;
		}
	}

	public bool ExpHudIsVisible
	{
		get
		{
			if (_experienceView != null)
			{
				return _experienceView.VisibleHUD;
			}
			return false;
		}
		set
		{
			if (_experienceView != null)
			{
				_experienceView.VisibleHUD = value;
			}
		}
	}

	public static ExpController Instance
	{
		get
		{
			return _instance;
		}
	}

	public static int LobbyLevel
	{
		get
		{
			return 3;
		}
	}

	public bool IsLevelUpShown { get; private set; }

	public bool InterfaceEnabled
	{
		get
		{
			if (_experienceView != null && _experienceView.interfaceHolder != null)
			{
				return _experienceView.interfaceHolder.gameObject.activeInHierarchy;
			}
			return false;
		}
		set
		{
			SetInterfaceEnabled(value);
		}
	}

	public bool WaitingForLevelUpView { get; private set; }

	public int OurTier
	{
		get
		{
			if (ExperienceController.sharedController != null)
			{
				return TierForLevel(ExperienceController.sharedController.currentLevel);
			}
			return 0;
		}
	}

	public int Rank
	{
		get
		{
			return Mathf.Clamp(ExperienceController.sharedController.currentLevel, 1, 36);
		}
	}

	public static event Action LevelUpShown;

	public static int OurTierForAnyPlace()
	{
		if (!(Instance != null))
		{
			return GetOurTier();
		}
		return Instance.OurTier;
	}

	private void SetInterfaceEnabled(bool value)
	{
		if (value && _experienceView != null)
		{
			bool flag = Application.loadedLevelName != Defs.MainMenuScene || ShopNGUIController.GuiActive || (MainMenuController.sharedController != null && MainMenuController.sharedController.InMiniGamesScreen) || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled) || ConnectScene.isEnable;
			if (_rankIndicator.IsVisible != flag)
			{
				if (flag)
				{
					_rankIndicator.PushRequest();
				}
				else
				{
					_rankIndicator.PopRequest();
				}
			}
		}
		if (InterfaceEnabled == value || !(_experienceView != null) || !(_experienceView.interfaceHolder != null))
		{
			return;
		}
		_experienceView.interfaceHolder.gameObject.SetActive(value);
		if (value && _experienceView.experienceCamera != null)
		{
			AudioListener component = _experienceView.experienceCamera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}

	public void HandleContinueButton(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
		}
		if (!_starterBannerShowed && ExperienceController.sharedController.currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			_starterBannerShowed = true;
			_experienceView.ToBonus(BalanceController.startCapitalGems, BalanceController.startCapitalCoins);
		}
		else
		{
			_starterBannerShowed = false;
			HideTierPanel(tierPanel);
		}
	}

	public void HandleShopButtonFromTierPanel(GameObject tierPanel)
	{
		StartCoroutine(HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	public void HandleNewAvailableItem(GameObject tierPanel, NewAvailableItemInShop itemInfo)
	{
		if (GameConnect.isHunger || GameConnect.isSurvival || GameConnect.isCOOP || GameConnect.isSpleef)
		{
			return;
		}
		if (CurrentFilterMap() != 0)
		{
			int[] target = new int[0];
			bool flag = true;
			if (itemInfo != null && itemInfo._tag != null)
			{
				flag = ItemDb.GetByTag(itemInfo._tag) != null;
				if (flag)
				{
					target = ItemDb.GetItemFilterMap(itemInfo._tag);
				}
			}
			if (flag && !target.Contains(CurrentFilterMap()))
			{
				HandleShopButtonFromTierPanel(tierPanel);
				return;
			}
		}
		if (ShopNGUIController.sharedShop == null)
		{
			UnityEngine.Debug.LogWarning("ShopNGUIController.sharedShop == null");
		}
		else
		{
			if (!(itemInfo == null))
			{
				string text = itemInfo._tag ?? string.Empty;
				UnityEngine.Debug.Log("Available item:   " + text + "    " + itemInfo.category);
				StartCoroutine(HandleShopButtonFromNewAvailableItemCoroutine(tierPanel, text, itemInfo.category));
				return;
			}
			UnityEngine.Debug.LogWarning("itemInfo == null");
		}
		StartCoroutine(HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	public static int CurrentFilterMap()
	{
		if (!Defs.filterMaps.ContainsKey(Application.loadedLevelName))
		{
			return 0;
		}
		return Defs.filterMaps[Application.loadedLevelName];
	}

	private IEnumerator HandleShopButtonFromTierPanelCoroutine(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(CurrentFilterMap());
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		HideTierPanel(tierPanel);
	}

	private IEnumerator HandleShopButtonFromNewAvailableItemCoroutine(GameObject tierPanel, string itemTag, ShopNGUIController.CategoryNames category)
	{
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			itemTag = WeaponManager.FirstUnboughtOrForOurTier(itemTag) ?? itemTag;
		}
		ShopNGUIController.sharedShop.SetItemToShow(new ShopNGUIController.ShopItem(itemTag, category));
		ShopNGUIController.sharedShop.CategoryToChoose = category;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		HideTierPanel(tierPanel);
		ShopNGUIController.sharedShop.AdjustCategoryGridCells();
	}

	public static int TierForLevel(int lev)
	{
		for (int i = 0; i < LevelsForTiers.Length - 1; i++)
		{
			if (lev < LevelsForTiers[i + 1])
			{
				return i;
			}
		}
		return LevelsForTiers.Length - 1;
	}

	public static int GetOurTier()
	{
		return TierForLevel(ExperienceController.GetCurrentLevelWithUpdateCorrection());
	}

	private void AddExperience(string idOfGivenPreviousTierBestArmor, int oldLevel, int oldExperience, int addend, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
	{
		if (_experienceView == null || ExperienceController.sharedController == null)
		{
			return;
		}
		int num = oldExperience + addend;
		int num2 = ExperienceController.MaxExpLevels[oldLevel];
		if (num >= num2)
		{
			AudioClip sound = levelup;
			if (tierup != null && Array.IndexOf(LevelsForTiers, oldLevel + 1) > 0)
			{
				sound = tierup;
			}
			if (oldLevel < 35)
			{
				int num3 = oldLevel + 1;
				int newExperience = num - num2;
				StartCoroutine(WaitAndUpdateExperience(idOfGivenPreviousTierBestArmor, num3, newExperience, ExperienceController.MaxExpLevels[num3], true, sound));
			}
			else if (oldLevel == 35)
			{
				int num4 = oldLevel + 1;
				int newExperience2 = ExperienceController.MaxExpLevels[num4];
				StartCoroutine(WaitAndUpdateExperience(idOfGivenPreviousTierBestArmor, num4, newExperience2, ExperienceController.MaxExpLevels[num4], true, sound));
			}
			else
			{
				int num5 = 36;
				int newExperience3 = ExperienceController.MaxExpLevels[num5];
				StartCoroutine(WaitAndUpdateExperience(idOfGivenPreviousTierBestArmor, num5, newExperience3, ExperienceController.MaxExpLevels[num5], false, exp2));
			}
		}
	}

	public bool IsRenderedWithCamera(Camera c)
	{
		if (_experienceView != null && _experienceView.experienceCamera != null)
		{
			return _experienceView.experienceCamera == c;
		}
		return false;
	}

	private string SubstituteTempGunIfReplaced(string constTg)
	{
		if (constTg == null)
		{
			return null;
		}
		KeyValuePair<string, string> keyValuePair = WeaponManager.replaceConstWithTemp.Find((KeyValuePair<string, string> kvp) => kvp.Key.Equals(constTg));
		if (keyValuePair.Key == null || keyValuePair.Value == null)
		{
			return constTg;
		}
		if (!TempItemsController.GunsMappingFromTempToConst.ContainsKey(keyValuePair.Value))
		{
			return keyValuePair.Value;
		}
		return constTg;
	}

	private IEnumerator WaitAndUpdateExperience(string idOfGivenPreviousTierBestArmor, int newRank, int newExperience, int newBound, bool showLevelUpPanel, AudioClip sound)
	{
		WaitingForLevelUpView = showLevelUpPanel;
		int tier = Array.BinarySearch(LevelsForTiers, ExperienceController.sharedController.currentLevel);
		bool isTierLevelup = 0 <= tier && tier < LevelsForTiers.Length;
		if (isTierLevelup)
		{
			if (PromoActionsManager.sharedManager != null)
			{
				try
				{
					List<string> itemsViewed = WeaponManager.GetNewWeaponsForTier(tier).Concat(GadgetsInfo.GetNewGadgetsForTier(tier)).ToList();
					PromoActionsManager.sharedManager.ReplaceUnlockedItemsWith(itemsViewed);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in WaitAndUpdateExperience ReplaceUnlockedItemsWith: {0}", ex);
				}
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat("ShopNguiController.Update: PromoActionsManager.sharedManager == null");
			}
		}
		yield return new WaitForSeconds(1.2f);
		if (showLevelUpPanel && ExperienceController.sharedController != null)
		{
			List<string> itemsToShow = new List<string>();
			if (isTierLevelup)
			{
				switch (tier)
				{
				case 1:
					itemsToShow = new List<string>
					{
						"Armor_Steel_1",
						SubstituteTempGunIfReplaced(WeaponTags.Antihero_Rifle_1_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.frank_sheepone_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.AcidCannon_Tag)
					};
					break;
				case 2:
					itemsToShow = new List<string>
					{
						"Armor_Royal_1",
						SubstituteTempGunIfReplaced(WeaponTags.DragonGun_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.charge_rifle_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.loud_piggy_Tag)
					};
					break;
				case 3:
					itemsToShow = new List<string>
					{
						"Armor_Almaz_1",
						SubstituteTempGunIfReplaced(WeaponTags.Dark_Matter_Generator_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.autoaim_bazooka_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Photon_Pistol_Tag)
					};
					break;
				case 4:
					itemsToShow = new List<string>
					{
						SubstituteTempGunIfReplaced(WeaponTags.RailRevolverBuy_3_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.RayMinigun_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Autoaim_RocketlauncherBuy_3_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Impulse_Sniper_RifleBuy_3_Tag)
					};
					break;
				case 5:
					itemsToShow = new List<string>
					{
						SubstituteTempGunIfReplaced(WeaponTags.PX_3000_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.StormHammer_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Sunrise_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Bastion_Tag)
					};
					break;
				}
				_experienceView.LevelUpPanelOptions.ShowTierView = true;
			}
			else
			{
				_experienceView.LevelUpPanelOptions.ShowTierView = false;
			}
			_experienceView.LevelUpPanelOptions.ShareButtonEnabled = true;
			int num = Math.Max(0, newRank - 1);
			int coinsReward = ExperienceController.addCoinsFromLevels[num];
			int gemsReward = ExperienceController.addGemsFromLevels[num];
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				_sameSceneIndicator = true;
				while (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
				{
					yield return null;
				}
				if (!_sameSceneIndicator || NetworkStartTableNGUIController.sharedController == null)
				{
					WaitingForLevelUpView = false;
					yield break;
				}
			}
			else if (Application.loadedLevelName == "LevelComplete")
			{
				_sameSceneIndicator = true;
				while (_sameSceneIndicator && LevelCompleteScript.IsInterfaceBusy)
				{
					yield return null;
				}
				if (!_sameSceneIndicator)
				{
					WaitingForLevelUpView = false;
					yield break;
				}
			}
			_experienceView.LevelUpPanelOptions.NewItems = itemsToShow;
			_experienceView.LevelUpPanelOptions.CurrentRank = newRank;
			_experienceView.LevelUpPanelOptions.CoinsReward = coinsReward;
			_experienceView.LevelUpPanelOptions.GemsReward = gemsReward;
			_experienceView.LevelUpPanelOptions.IdOfGivenPreviousTierBestArmor = idOfGivenPreviousTierBestArmor;
			_experienceView.ShowLevelUpPanel();
		}
		WaitingForLevelUpView = false;
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sound);
		}
	}

	public static float GetPercentage(int experience)
	{
		if (ExperienceController.sharedController == null)
		{
			return 0f;
		}
		int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		return (float)Mathf.Clamp(experience, 0, num) / (float)num;
	}

	public static float ProgressExpInPer()
	{
		return GetPercentage(ExperienceController.sharedController.CurrentExperience);
	}

	public static string FormatExperienceLabel(int xp, int bound)
	{
		string result = LocalizationStore.Get("Key_0928");
		string result2 = string.Format("{0} {1}/{2}", new object[3]
		{
			LocalizationStore.Get("Key_0204"),
			xp,
			bound
		});
		if (xp != bound && ExperienceController.sharedController.currentLevel != 36)
		{
			return result2;
		}
		return result;
	}

	public static string ExpToString()
	{
		int currentLevel = ExperienceController.sharedController.currentLevel;
		return FormatExperienceLabel(ExperienceController.sharedController.CurrentExperience, ExperienceController.MaxExpLevels[currentLevel]);
	}

	private static string FormatLevelLabel(int level)
	{
		return string.Format("{0} {1}", new object[2]
		{
			LocalizationStore.Key_0226,
			level
		});
	}

	private void Awake()
	{
		ExperienceController.OnPlayerProgressChanged += ExperienceControllerOnPlayerProgressChanged;
		if (!SceneLoader.ActiveSceneName.EndsWith("Workbench"))
		{
			InterfaceEnabled = false;
		}
		Singleton<SceneLoader>.Instance.OnSceneLoading += delegate
		{
			if (_experienceView != null)
			{
				LevelUpWithOffers currentVisiblePanel = _experienceView.CurrentVisiblePanel;
				if (currentVisiblePanel != null)
				{
					HideTierPanel(currentVisiblePanel.gameObject);
				}
			}
			IsLevelUpShown = false;
		};
	}

	private void Start()
	{
		if (_instance != null)
		{
			UnityEngine.Debug.LogWarning("ExpController is not null while starting.");
		}
		_instance = this;
	}

	private void OnDestroy()
	{
		ExperienceController.OnPlayerProgressChanged -= ExperienceControllerOnPlayerProgressChanged;
		_instance = null;
	}

	private void Update()
	{
		if (ExperienceController.sharedController != null)
		{
			SetInterfaceEnabled(ExperienceController.sharedController.isShowRanks);
		}
	}

	private void ExperienceControllerOnPlayerProgressChanged(ExperienceController.PlayerProgress progress)
	{
		AddExperience(progress.IdOfGivenPreviousTierBestArmor, progress.OldLevel, progress.OldExp, progress.IncrementExp, progress.Exp2, progress.Levelup, progress.Tierup);
	}

	public static void ShowTierPanel(GameObject tierPanel)
	{
		if (!(tierPanel != null))
		{
			return;
		}
		tierPanel.SetActive(true);
		if (Instance != null)
		{
			Instance.IsLevelUpShown = true;
			Action levelUpShown = ExpController.LevelUpShown;
			if (levelUpShown != null)
			{
				levelUpShown();
			}
		}
		MainMenuController.SetInputEnabled(false);
		LevelCompleteScript.SetInputEnabled(false);
	}

	public static void HideTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(false);
			if (Instance != null)
			{
				Instance.IsLevelUpShown = false;
			}
			MainMenuController.SetInputEnabled(true);
			LevelCompleteScript.SetInputEnabled(true);
		}
	}

	private void OnLevelWasLoaded(int index)
	{
		_sameSceneIndicator = false;
	}
}
