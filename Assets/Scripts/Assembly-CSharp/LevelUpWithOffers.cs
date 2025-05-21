using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using I2.Loc;
using Rilisoft;
using UnityEngine;

public class LevelUpWithOffers : MonoBehaviour
{
	public struct ItemDesc
	{
		public string tag;

		public ShopNGUIController.CategoryNames category;
	}

	[CompilerGenerated]
	internal sealed class _003CUpdatePanelsAndAnchors_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LevelUpWithOffers _003C_003E4__this;

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
		public _003CUpdatePanelsAndAnchors_003Ed__15(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				Player_move_c.PerformActionRecurs(_003C_003E4__this.transform.parent.parent.parent.gameObject, delegate(Transform t)
				{
					UIPanel component2 = t.GetComponent<UIPanel>();
					if (component2 != null)
					{
						component2.Refresh();
					}
				});
				Player_move_c.PerformActionRecurs(_003C_003E4__this.transform.parent.parent.parent.gameObject, delegate(Transform t)
				{
					UIRect component = t.GetComponent<UIRect>();
					if (component != null)
					{
						component.UpdateAnchors();
					}
				});
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
	internal sealed class _003CGemsStarterAnimation_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LevelUpWithOffers _003C_003E4__this;

		private float _003Cseconds_003E5__1;

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
		public _003CGemsStarterAnimation_003Ed__29(int _003C_003E1__state)
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
				_003Cseconds_003E5__1 = 0f;
				_003C_003E4__this.SetGemsLabel(0);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Cseconds_003E5__1 < 1f)
			{
				for (int i = 0; i < _003C_003E4__this.gemsStarterBank.Length; i++)
				{
					_003C_003E4__this.SetGemsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, _003C_003E4__this.gemsStarterBankValue, _003Cseconds_003E5__1)));
				}
				_003Cseconds_003E5__1 += Time.deltaTime;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.SetGemsLabel(Mathf.RoundToInt(_003C_003E4__this.gemsStarterBankValue));
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

	[CompilerGenerated]
	internal sealed class _003CCoinsStarterAnimation_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LevelUpWithOffers _003C_003E4__this;

		private float _003Cseconds_003E5__1;

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
		public _003CCoinsStarterAnimation_003Ed__30(int _003C_003E1__state)
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
				_003Cseconds_003E5__1 = 0f;
				_003C_003E4__this.SetCoinsLabel(0);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Cseconds_003E5__1 < 1f)
			{
				for (int i = 0; i < _003C_003E4__this.coinsStarterBank.Length; i++)
				{
					_003C_003E4__this.SetCoinsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, _003C_003E4__this.coinsStarterBankValue, _003Cseconds_003E5__1)));
				}
				_003Cseconds_003E5__1 += Time.deltaTime;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.SetCoinsLabel(Mathf.RoundToInt(_003C_003E4__this.coinsStarterBankValue));
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

	public GameObject giveArmorAnimation;

	public List<UILabel> giveArmorLabels;

	public UITexture giveArmorTexture;

	public RewardWindowBase shareScript;

	public UILabel[] rewardGemsPriceLabel;

	public UILabel[] currentRankLabel;

	public UILabel[] rewardPriceLabel;

	public UILabel[] healthLabel;

	public UILabel[] gemsStarterBank;

	public UILabel[] coinsStarterBank;

	public List<UILabel> youReachedLabels;

	public NewAvailableItemInShop[] items;

	public bool isTierLevelUp;

	private float gemsStarterBankValue;

	private float coinsStarterBankValue;

	private IEnumerator UpdatePanelsAndAnchors()
	{
		yield return new WaitForEndOfFrame();
		Player_move_c.PerformActionRecurs(transform.parent.parent.parent.gameObject, delegate(Transform t)
		{
			UIPanel component2 = t.GetComponent<UIPanel>();
			if (component2 != null)
			{
				component2.Refresh();
			}
		});
		Player_move_c.PerformActionRecurs(transform.parent.parent.parent.gameObject, delegate(Transform t)
		{
			UIRect component = t.GetComponent<UIRect>();
			if (component != null)
			{
				component.UpdateAnchors();
			}
		});
	}

	private void Awake()
	{
		if (!isTierLevelUp)
		{
			FacebookController.StoryPriority levelupPriority = FacebookController.StoryPriority.Red;
			shareScript.priority = levelupPriority;
			shareScript.shareAction = delegate
			{
				FacebookController.PostOpenGraphStory("reach", "level", levelupPriority, new Dictionary<string, string> { 
				{
					"level",
					ExperienceController.sharedController.currentLevel.ToString()
				} });
			};
			shareScript.HasReward = true;
			shareScript.twitterStatus = () => string.Format("I've reached level {0} in @PixelGun3D! Come to the battle and try to defeat me! #pixelgun3d #pixelgun #3d #pg3d #fps http://goo.gl/8fzL9u", new object[1] { ExperienceController.sharedController.currentLevel });
			shareScript.EventTitle = "Level-up";
		}
		else
		{
			FacebookController.StoryPriority tierupPriority = FacebookController.StoryPriority.Green;
			shareScript.priority = tierupPriority;
			shareScript.shareAction = delegate
			{
				FacebookController.PostOpenGraphStory("unlock", "new weapon", tierupPriority, new Dictionary<string, string> { 
				{
					"new weapon",
					(ExpController.Instance.OurTier + 1).ToString()
				} });
			};
			shareScript.HasReward = true;
			shareScript.twitterStatus = () => "I've unlocked cool new weapons in @PixelGun3D! Let’s try them out! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
			shareScript.EventTitle = "Tier-up";
		}
	}

	[ContextMenu("Update")]
	public void OnEnable()
	{
		StartCoroutine(UpdatePanelsAndAnchors());
	}

	public void RunArmorAnimation(string givenArmorId)
	{
		bool flag = !givenArmorId.IsNullOrEmpty();
		if (shareScript == null)
		{
			UnityEngine.Debug.LogErrorFormat("RunArmorAnimation: shareScript == null");
		}
		else
		{
			if (giveArmorAnimation == null)
			{
				return;
			}
			giveArmorAnimation.SetActiveSafeSelf(flag);
			Animator component = shareScript.GetComponent<Animator>();
			if (component == null)
			{
				UnityEngine.Debug.LogErrorFormat("RunArmorAnimation: animator == null");
				return;
			}
			component.SetBool("Armor", flag);
			if (!flag)
			{
				return;
			}
			if (giveArmorLabels != null && giveArmorLabels.Count != 0 && !(giveArmorTexture == null))
			{
				try
				{
					string itemName = ItemDb.GetItemName(givenArmorId, ShopNGUIController.CategoryNames.ArmorCategory);
					RiliExtensions.ForEach(giveArmorLabels, delegate(UILabel label)
					{
						label.text = itemName;
					});
					Texture itemIcon = ItemDb.GetItemIcon(givenArmorId, ShopNGUIController.CategoryNames.ArmorCategory);
					giveArmorTexture.mainTexture = itemIcon;
					return;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in RunArmorAnimation: {0}", ex);
					return;
				}
			}
			UnityEngine.Debug.LogErrorFormat("RunArmorAnimation: giveArmorLabels == null || giveArmorLabels.Count == 0 || giveArmorTexture == null");
		}
	}

	private void OnDisable()
	{
		ShowIndicationMoney();
	}

	private void OnDestroy()
	{
		ShowIndicationMoney();
	}

	private void ShowIndicationMoney()
	{
		BankController.canShowIndication = true;
		BankController.UpdateAllIndicatorsMoney();
	}

	public void SetCurrentRank(string currentRank)
	{
		for (int i = 0; i < currentRankLabel.Length; i++)
		{
			currentRankLabel[i].text = LocalizationStore.Get("Key_0226").ToUpper() + " " + currentRank + "!";
		}
		string text = "";
		switch (ProfileController.CurOrderCup)
		{
		case 0:
			text = ScriptLocalization.Get("Key_1938");
			break;
		case 1:
			text = ScriptLocalization.Get("Key_1939");
			break;
		case 2:
			text = ScriptLocalization.Get("Key_1940");
			break;
		case 3:
			text = ScriptLocalization.Get("Key_1941");
			break;
		case 4:
			text = ScriptLocalization.Get("Key_1942");
			break;
		case 5:
			text = ScriptLocalization.Get("Key_1943");
			break;
		}
		foreach (UILabel youReachedLabel in youReachedLabels)
		{
			youReachedLabel.text = text;
		}
	}

	public void SetRewardPrice(string rewardPrice)
	{
		for (int i = 0; i < rewardPriceLabel.Length; i++)
		{
			rewardPriceLabel[i].text = rewardPrice;
		}
	}

	public void SetGemsRewardPrice(string gemsReward)
	{
		for (int i = 0; i < rewardGemsPriceLabel.Length; i++)
		{
			rewardGemsPriceLabel[i].text = gemsReward;
		}
	}

	public void SetAddHealthCount(string count)
	{
		if (healthLabel != null)
		{
			for (int i = 0; i < healthLabel.Length; i++)
			{
				healthLabel[i].text = count;
			}
		}
	}

	private void SetGemsLabel(int value)
	{
		for (int i = 0; i < gemsStarterBank.Length; i++)
		{
			gemsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1531"), new object[1] { value });
		}
	}

	private void SetCoinsLabel(int value)
	{
		for (int i = 0; i < coinsStarterBank.Length; i++)
		{
			coinsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1530"), new object[1] { value });
		}
	}

	public IEnumerator GemsStarterAnimation()
	{
		float seconds = 0f;
		SetGemsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < gemsStarterBank.Length; i++)
			{
				SetGemsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, gemsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		SetGemsLabel(Mathf.RoundToInt(gemsStarterBankValue));
	}

	public IEnumerator CoinsStarterAnimation()
	{
		float seconds = 0f;
		SetCoinsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < coinsStarterBank.Length; i++)
			{
				SetCoinsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, coinsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		SetCoinsLabel(Mathf.RoundToInt(coinsStarterBankValue));
	}

	public void SetStarterBankValues(int gemsReward, int coinsReward)
	{
		gemsStarterBankValue = gemsReward;
		coinsStarterBankValue = coinsReward;
		SetGemsLabel(0);
		SetCoinsLabel(0);
	}

	public void SetItems(List<string> itemTags)
	{
		if (items == null || items.Length == 0)
		{
			return;
		}
		for (int i = 0; i < items.Length; i++)
		{
			items[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < itemTags.Count; j++)
		{
			items[j].gameObject.SetActive(true);
			string text = itemTags[j];
			int itemCategory = ItemDb.GetItemCategory(text);
			items[j]._tag = text;
			items[j].category = (ShopNGUIController.CategoryNames)itemCategory;
			items[j].itemImage.mainTexture = ItemDb.GetItemIcon(text, (ShopNGUIController.CategoryNames)itemCategory, 1);
			foreach (UILabel item in items[j].itemName)
			{
				item.text = ItemDb.GetItemName(text, (ShopNGUIController.CategoryNames)itemCategory);
			}
			items[j].GetComponent<UIButton>().isEnabled = (!GameConnect.isHunger && !GameConnect.isSurvival && !GameConnect.isCOOP && !GameConnect.isSpleef) || text == null || ItemDb.GetByTag(text) == null;
		}
	}

	public void Close()
	{
		ExpController.Instance.HandleContinueButton(base.gameObject);
	}
}
