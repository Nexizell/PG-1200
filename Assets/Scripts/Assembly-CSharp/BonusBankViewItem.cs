using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class BonusBankViewItem : RealInappBankViewItem
{
	internal abstract class UIFiller
	{
		public void Fill(BonusBankViewItem bonusBankViewItem)
		{
			if (bonusBankViewItem == null)
			{
				UnityEngine.Debug.LogErrorFormat("CurrencyBonusUIFiller.Fill bonusBankViewItem == null");
				return;
			}
			InappRememberedBonus actualBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters);
			bonusBankViewItem.plus.SetActiveSafeSelf(actualBonus.Coins > 0 && actualBonus.Gems > 0);
			bool flag = true;
			try
			{
				object value;
				if (bonusBankViewItem.InappBonusParameters.TryGetValue("isGems", out value) && value != null)
				{
					flag = Convert.ToBoolean(value);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in get isGems action: {0}", ex);
			}
			if (flag)
			{
				RiliExtensions.ForEach(bonusBankViewItem.coinsQuantityLabels, delegate(UILabel label)
				{
					label.text = actualBonus.Coins.ToString();
				});
				RiliExtensions.ForEach(bonusBankViewItem.gemsQuantityLabels, delegate(UILabel label)
				{
					label.text = actualBonus.Gems.ToString();
				});
				bonusBankViewItem.gemsQuantityLabels.First().transform.SetAsFirstSibling();
				bonusBankViewItem.coinsQuantityLabels.First().transform.SetAsLastSibling();
			}
			else
			{
				RiliExtensions.ForEach(bonusBankViewItem.coinsQuantityLabels, delegate(UILabel label)
				{
					label.text = actualBonus.Coins.ToString();
				});
				RiliExtensions.ForEach(bonusBankViewItem.gemsQuantityLabels, delegate(UILabel label)
				{
					label.text = actualBonus.Gems.ToString();
				});
				bonusBankViewItem.coinsQuantityLabels.First().transform.SetAsFirstSibling();
				bonusBankViewItem.gemsQuantityLabels.First().transform.SetAsLastSibling();
			}
			bonusBankViewItem.coinsQuantityLabels.First().gameObject.SetActiveSafeSelf(actualBonus.Coins > 0);
			bonusBankViewItem.gemsQuantityLabels.First().gameObject.SetActiveSafeSelf(actualBonus.Gems > 0);
			bonusBankViewItem.quantitiesTable.Reposition();
			bonusBankViewItem.currence.SetActiveSafeSelf(false);
			bonusBankViewItem.pets.SetActiveSafeSelf(false);
			bonusBankViewItem.weapon.SetActiveSafeSelf(false);
			bonusBankViewItem.gadget.SetActiveSafeSelf(false);
			bonusBankViewItem.leprechaun.SetActiveSafeSelf(false);
			RiliExtensions.ForEach(bonusBankViewItem.profitLabels, delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(false);
			});
			FillProfit(bonusBankViewItem);
			FillCore(bonusBankViewItem);
		}

		public void SetIcon(BonusBankViewItem bonusBankViewItem)
		{
			if (bonusBankViewItem == null)
			{
				UnityEngine.Debug.LogErrorFormat("CurrencyBonusUIFiller.SetIcon bonusBankViewItem == null");
			}
			else
			{
				SetIconCore(bonusBankViewItem);
			}
		}

		protected abstract void FillCore(BonusBankViewItem bonusBankViewItem);

		protected abstract void SetIconCore(BonusBankViewItem bonusBankViewItem);

		protected virtual void FillProfit(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				decimal num = Math.Round(CalculateProfit(bonusBankViewItem.MarketProduct, bonusBankViewItem.InappBonusParameters), 0, MidpointRounding.AwayFromZero);
				string text = CurrencyOfProfit(bonusBankViewItem.MarketProduct);
				string text2 = num.ToString(CultureInfo.InvariantCulture);
				bool flag = text.Contains("$") || text.IndexOf("usd", StringComparison.OrdinalIgnoreCase) != -1;
				string text3 = string.Format("{0}{1}", new object[2]
				{
					flag ? text : text2,
					flag ? text2 : text
				});
				string profitText = string.Format(LocalizationStore.Get("Key_2865"), new object[1] { text3 });
				RiliExtensions.ForEach(bonusBankViewItem.profitLabels, delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(true);
				});
				RiliExtensions.ForEach(bonusBankViewItem.profitLabels, delegate(UILabel label)
				{
					label.text = profitText;
				});
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in UIFiller.FillProfit: {0}", ex);
				RiliExtensions.ForEach(bonusBankViewItem.profitLabels, delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}
	}

	internal class CurrencyBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.currence.SetActiveSafeSelf(true);
			RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(false);
			});
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.icon.gameObject.SetActiveSafeSelf(true);
		}
	}

	internal class PetBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.pets.SetActiveSafeSelf(true);
			FillDescription(bonusBankViewItem);
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				bonusBankViewItem.petIcon.gameObject.SetActiveSafeSelf(true);
				Texture itemIcon = ItemDb.GetItemIcon(bonusBankViewItem.InappBonusParameters["Pet"] as string, ShopNGUIController.CategoryNames.PetsCategory);
				bonusBankViewItem.petIcon.mainTexture = itemIcon;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in PetBonusUIFiller.SetIconCore: {0}", ex);
			}
		}

		protected override void FillProfit(BonusBankViewItem bonusBankViewItem)
		{
		}

		private static void FillDescription(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
				object value;
				if (bonusBankViewItem.InappBonusParameters.TryGetValue("Quantity", out value))
				{
					Convert.ToInt32(value);
					string text = null;
					object value2;
					if (bonusBankViewItem.InappBonusParameters.TryGetValue("Pet", out value2))
					{
						text = value2 as string;
						if (text.IsNullOrEmpty())
						{
							UnityEngine.Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no petId.IsNullOrEmpty() in parameters: {0}", Json.Serialize(bonusBankViewItem.InappBonusParameters));
							return;
						}
						string petName = null;
						string text2 = text;
						if (text2 == Singleton<PetsManager>.Instance.GetIdWithoutUp(text2))
						{
							text2 += "_up0";
						}
						PetInfo value3;
						if (PetsInfo.info.TryGetValue(text2, out value3) && value3 != null)
						{
							petName = LocalizationStore.Get(value3.Lkey);
							if (petName.IsNullOrEmpty())
							{
								UnityEngine.Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no petName.IsNullOrEmpty() in parameters: {0}", Json.Serialize(bonusBankViewItem.InappBonusParameters));
								return;
							}
							RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
							{
								label.text = string.Format(LocalizationStore.Get("Key_2903"), new object[1] { petName });
							});
							RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
							{
								label.gameObject.SetActiveSafeSelf(true);
							});
						}
						else
						{
							UnityEngine.Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no such pet {0}, parameters of action = {1}", text, Json.Serialize(bonusBankViewItem.InappBonusParameters));
						}
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no Pet in parameters: {0}", Json.Serialize(bonusBankViewItem.InappBonusParameters));
					}
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no quantity in parameters: {0}", Json.Serialize(bonusBankViewItem.InappBonusParameters));
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in FillDescription: {0}", ex);
				RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}
	}

	internal class RedWeaponBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.weapon.SetActiveSafeSelf(true);
			RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
			{
				label.text = LocalizationStore.Get("Key_2891");
			});
			RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(true);
			});
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				bonusBankViewItem.redWeaponIcon.gameObject.SetActiveSafeSelf(true);
				string tag = bonusBankViewItem.InappBonusParameters["Weapon"] as string;
				Texture itemIcon = ItemDb.GetItemIcon(tag, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tag));
				bonusBankViewItem.redWeaponIcon.mainTexture = itemIcon;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in RedWeaponBonusUIFiller.SetIconCore: {0}", ex);
			}
		}
	}

	internal class GadgetsBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.gadget.SetActiveSafeSelf(true);
			bool isGems = true;
			try
			{
				if (InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters).Coins > 0)
				{
					isGems = false;
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in GadgetsBonusUIFiller.FillCore: {0}", ex);
			}
			RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
			{
				label.text = (isGems ? LocalizationStore.Get("Key_2904") : LocalizationStore.Get("Key_2904"));
			});
			RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(true);
			});
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				List<string> list = (bonusBankViewItem.InappBonusParameters["Gadgets"] as List<string>).OfType<string>().ToList();
				switch (list.Count)
				{
				case 1:
					bonusBankViewItem.leftGadget.mainTexture = null;
					bonusBankViewItem.centerGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]));
					bonusBankViewItem.rightGadget.mainTexture = null;
					break;
				case 2:
					bonusBankViewItem.leftGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]));
					bonusBankViewItem.centerGadget.mainTexture = null;
					bonusBankViewItem.rightGadget.mainTexture = ItemDb.GetItemIcon(list[1], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[1]));
					break;
				case 3:
					bonusBankViewItem.leftGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]));
					bonusBankViewItem.centerGadget.mainTexture = ItemDb.GetItemIcon(list[1], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[1]));
					bonusBankViewItem.rightGadget.mainTexture = ItemDb.GetItemIcon(list[2], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[2]));
					break;
				default:
					UnityEngine.Debug.LogErrorFormat("GadgetsBonusUIFiller.FillCore: unsupported number of gadgets {0}", list.Count);
					break;
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in GadgetsBonusUIFiller.FillCore: {0}", ex);
			}
		}
	}

	internal class LeprechaunBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.leprechaun.SetActiveSafeSelf(true);
			try
			{
				InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters);
				int perDayCurrency = actualBonusSizeForInappBonus.PerDayLeprechaun;
				if (perDayCurrency > 0)
				{
					RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
					{
						label.text = string.Format(LocalizationStore.Get("Key_2898"), new object[1] { perDayCurrency });
					});
					RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
					{
						label.gameObject.SetActiveSafeSelf(true);
					});
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("Error in LeprechaunBonusUIFiller FillCore: perDayCurrency <= 0,  {0}", Json.Serialize(bonusBankViewItem.InappBonusParameters));
					RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
					{
						label.gameObject.SetActiveSafeSelf(false);
					});
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in LeprechaunBonusUIFiller FillCore: {0}", ex);
				RiliExtensions.ForEach(bonusBankViewItem.descriptionLabels, delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
		}
	}

	internal class UniqueWeaponBonusUIFiller : UIFiller
	{
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
		}

		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
		}
	}

	[CompilerGenerated]
	internal sealed class _003CFinishItemsLeftAnimation_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BonusBankViewItem _003C_003E4__this;

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
		public _003CFinishItemsLeftAnimation_003Ed__24(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForRealSeconds(0.2f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.IsAnimatingItemsLeft = false;
				if (_003C_003E4__this.IsPacksLeftShown)
				{
					_003C_003E4__this.UpdateItemsLeft();
				}
				else
				{
					_003C_003E4__this.UpdateTimeLabels();
				}
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

	public GameObject currence;

	public GameObject pets;

	public GameObject weapon;

	public GameObject gadget;

	public GameObject leprechaun;

	public UITexture petIcon;

	public UITexture leftGadget;

	public UITexture centerGadget;

	public UITexture rightGadget;

	public UITexture redWeaponIcon;

	public UITexture leprechaunIcon;

	public GameObject plus;

	public List<UILabel> descriptionLabels;

	public TweenScale itemsLeftTweener;

	public TweenColor itemsLeftColorTweener;

	public List<UILabel> itemsLeftLabels;

	public UITable quantitiesTable;

	public List<UILabel> coinsQuantityLabels;

	public List<UILabel> gemsQuantityLabels;

	public List<UILabel> profitLabels;

	private UIFiller m_filler;

	private int ItemsLeft { get; set; }

	private long TimeLeft { get; set; }

	private bool IsPacksLeftShown { get; set; }

	private bool IsAnimatingItemsLeft { get; set; }

	private UIFiller Filler
	{
		get
		{
			if (m_filler == null)
			{
				string text = base.InappBonusParameters["action"] as string;
				switch (text)
				{
				case "currence":
					m_filler = new CurrencyBonusUIFiller();
					break;
				case "pet":
					m_filler = new PetBonusUIFiller();
					break;
				case "gadget":
					m_filler = new GadgetsBonusUIFiller();
					break;
				case "weapon":
					m_filler = new RedWeaponBonusUIFiller();
					break;
				case "leprechaun":
					m_filler = new LeprechaunBonusUIFiller();
					break;
				default:
					UnityEngine.Debug.LogErrorFormat("Unknown action type:  {0}", text);
					m_filler = new CurrencyBonusUIFiller();
					break;
				}
			}
			return m_filler;
		}
		set
		{
			m_filler = value;
		}
	}

	public override void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, BankExchangeItemData exchangeInfo, EventHandler clickHandler)
	{
		if (product == null)
		{
			UnityEngine.Debug.LogErrorFormat("BonusBankViewItem.Setup: product == null");
			return;
		}
		if (product.Id == null)
		{
			UnityEngine.Debug.LogErrorFormat("BonusBankViewItem.Setup: product.Id == null");
			return;
		}
		base.Setup(product, newPurchaseInfo, (BankExchangeItemData)null, clickHandler);
		List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
		if (currentInnapBonus != null)
		{
			base.InappBonusParameters = currentInnapBonus.FirstOrDefault((Dictionary<string, object> bonus) => Convert.ToString(bonus["ID"]) == base.MarketProduct.Id);
		}
		else
		{
			base.InappBonusParameters = null;
		}
		try
		{
			Filler.Fill(this);
			object value;
			if (base.InappBonusParameters.TryGetValue("Type", out value) && value != null && value is string)
			{
				IsPacksLeftShown = value as string == "packs";
			}
			else
			{
				IsPacksLeftShown = false;
			}
			if (IsPacksLeftShown)
			{
				ItemsLeft = Convert.ToInt32(base.InappBonusParameters["Pack"]);
				UpdateItemsLeft();
			}
			else
			{
				TimeLeft = Convert.ToInt64(base.InappBonusParameters["End"]);
				UpdateTimeLabels();
			}
			SetIcon();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in BonusBankViewItem.Setup: {0}", ex);
		}
	}

	protected override void Update()
	{
		base.Update();
		try
		{
			List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
			Dictionary<string, object> dictionary = InappBonuessController.FindInappBonusInBonuses(base.InappBonusParameters, currentInnapBonus);
			if (dictionary == null)
			{
				UnityEngine.Debug.LogWarningFormat("BonusBankViewItem.Update: currentInappBonusAction == null");
				return;
			}
			object value = (IsPacksLeftShown ? dictionary["Pack"] : dictionary["End"]);
			bool flag = false;
			flag = ((!IsPacksLeftShown) ? (Convert.ToInt64(value) != TimeLeft) : (Convert.ToInt32(value) != ItemsLeft));
			if (!IsAnimatingItemsLeft && flag)
			{
				if (IsPacksLeftShown)
				{
					ItemsLeft = Convert.ToInt32(value);
				}
				else
				{
					TimeLeft = Convert.ToInt64(value);
				}
				itemsLeftTweener.ResetToBeginning();
				itemsLeftTweener.PlayForward();
				if (itemsLeftColorTweener != null)
				{
					itemsLeftColorTweener.ResetToBeginning();
					itemsLeftColorTweener.PlayForward();
				}
				IsAnimatingItemsLeft = true;
				StartCoroutine(FinishItemsLeftAnimation());
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in BonusBankViewItem.Update: {0}", ex);
		}
	}

	private void UpdateItemsLeft()
	{
		string localizationKey = "Key_2864";
		string text = base.InappBonusParameters["action"] as string;
		switch (text)
		{
		case "pet":
		case "weapon":
		case "leprechaun":
			localizationKey = "Key_2896";
			break;
		default:
			UnityEngine.Debug.LogErrorFormat("UpdateItemsLeft Unknown action type:  {0}", text);
			break;
		case "currence":
		case "gadget":
			break;
		}
		RiliExtensions.ForEach(itemsLeftLabels, delegate(UILabel label)
		{
			label.text = string.Format(LocalizationStore.Get(localizationKey), new object[1] { ItemsLeft.ToString() });
		});
	}

	private void UpdateTimeLabels()
	{
		string formattedTimeLeft = ((TimeLeft >= 86400) ? string.Format("{0} {1}", new object[2]
		{
			LocalizationStore.Get("Key_1125"),
			RiliExtensions.GetTimeStringDays(TimeLeft)
		}) : RiliExtensions.GetTimeString(TimeLeft));
		RiliExtensions.ForEach(itemsLeftLabels, delegate(UILabel label)
		{
			label.text = formattedTimeLeft;
		});
	}

	private IEnumerator FinishItemsLeftAnimation()
	{
		yield return new WaitForRealSeconds(0.2f);
		IsAnimatingItemsLeft = false;
		if (IsPacksLeftShown)
		{
			UpdateItemsLeft();
		}
		else
		{
			UpdateTimeLabels();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void OnEnable()
	{
		IsAnimatingItemsLeft = false;
	}

	protected override void OnDisable()
	{
	}

	protected override void SetIcon()
	{
		Filler.SetIcon(this);
	}

	private static string CurrencyOfProfit(IMarketProduct product)
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			return "%";
		}
		if (product == null)
		{
			UnityEngine.Debug.LogErrorFormat("CurrencyOfProfit: product == null");
			return string.Empty;
		}
		return product.Currency ?? string.Empty;
	}

	private static decimal CalculateProfit(IMarketProduct product, Dictionary<string, object> inappBonusAction)
	{
		decimal d = Convert.ToDecimal(inappBonusAction["Profit"], CultureInfo.InvariantCulture);
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			return decimal.Multiply(100m, d);
		}
		if (product == null)
		{
			UnityEngine.Debug.LogErrorFormat("CalculateProfit: product == null");
			return 0m;
		}
		return decimal.Multiply(product.PriceValue, d);
	}
}
