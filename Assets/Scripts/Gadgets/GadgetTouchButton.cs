using UnityEngine;

public class GadgetTouchButton : MonoBehaviour
{
	public GadgetInfo.GadgetCategory category = GadgetInfo.GadgetCategory.Throwing;

	public UITexture duration;

	public UITexture cooldown;

	public GameObject cooldownEnds;

	public UITweener cooldownEndsSpin;

	[SerializeField]
	private UITexture gadgetIcon;

	private GadgetInfo Info;

	private Gadget gadget;

	private bool activeGadgetButton;

	private bool isGadgetReady;

	private bool gadgetPressed;

	private bool isDaterButton;

	public GameObject daterIcon;

	public GameObject daterCounter;

	public GameObject daterPrice;

	public UILabel daterCounterLabel;

	public GameObject cantUseGadgetObj;

	public UILabel killsNeededLabel;

	public AudioSource sound;

	private int cachedLikeCount = -1;

	private void OnEnable()
	{
		if (GameConnect.isDaterRegim)
		{
			SetupDaterButton();
		}
		else
		{
			Setup();
		}
	}

	private void Setup()
	{
		Gadget value = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(category, out value))
		{
			if (value == null)
			{
				gameObject.SetActive(false);
				enabled = false;
				return;
			}
			SetupGadgetButton(ItemDb.GetItemIcon(value.Info.Id, (ShopNGUIController.CategoryNames)category), value);
		}
	}

	public void SetupDaterButton()
	{
		if (category == GadgetInfo.GadgetCategory.Throwing)
		{
			isDaterButton = true;
			gadgetIcon.gameObject.SetActive(false);
			daterIcon.SetActive(true);
			daterCounter.SetActive(true);
		}
	}

	public void SetupGadgetButton(Texture _gadgetIcon, Gadget _gadget)
	{
		gadgetIcon.mainTexture = _gadgetIcon;
		gadget = _gadget;
	}

	private void Update()
	{
		if (GameConnect.isDaterRegim)
		{
			daterCounter.SetActive(WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount > 0);
			daterPrice.SetActive(WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount <= 0);
			if (cachedLikeCount != WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount)
			{
				daterCounterLabel.text = WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount.ToString();
			}
			return;
		}
		duration.fillAmount = InGameGadgetSet.CurrentSet[category].ExpirationProgress;
		cooldown.fillAmount = InGameGadgetSet.CurrentSet[category].CooldownProgress;
		cooldown.gameObject.SetActive(cooldown.fillAmount > 0f);
		duration.gameObject.SetActive(duration.fillAmount > 0f);
		bool canUse = InGameGadgetSet.CurrentSet[category].CanUse;
		if (canUse && !isGadgetReady)
		{
			isGadgetReady = true;
			PlayGetAnimation();
		}
		isGadgetReady = canUse;
	}

	private void PlayGetAnimation()
	{
		cooldownEnds.GetComponent<UITweener>().ResetToBeginning();
		cooldownEnds.GetComponent<UITweener>().PlayForward();
		cooldownEndsSpin.ResetToBeginning();
		cooldownEndsSpin.PlayForward();
		sound.Play();
	}

	public void DeactivateGadgetButton()
	{
	}

	private void PreUseGadget()
	{
	}

	private void UseGadget()
	{
	}

	public static bool IsBuyGrenadeActive()
	{
		return GameConnect.isDaterRegim;
	}
}
