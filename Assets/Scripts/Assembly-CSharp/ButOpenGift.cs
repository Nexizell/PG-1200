using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Rilisoft;
using UnityEngine;

public class ButOpenGift : MonoBehaviour
{
	public static ButOpenGift instance;

	public string nameShaderColor = "_Color";

	public float speedAnim = 1f;

	public Color normalColor = Color.white;

	public Color pressColor = Color.white;

	public Color animColor = Color.white;

	public Material[] allHighlightMaterial;

	public Collider colActive;

	public Collider colNormal;

	public bool _activeHighLight;

	public bool _isPressBut;

	public bool _isCanGetGift;

	public Color curColorHS;

	private NickLabelController labelOverGift;

	public BindedBillboard Billboard;

	private bool _canShowLabel;

	public bool ActiveHighLight
	{
		get
		{
			return _activeHighLight;
		}
		set
		{
			if (_activeHighLight != value)
			{
				_activeHighLight = value;
				if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
				{
					_activeHighLight = false;
				}
				UpdateHUDStateGift();
			}
		}
	}

	public bool IsPressBut
	{
		get
		{
			return _isPressBut;
		}
		set
		{
			_isPressBut = value;
			UpdateHUDStateGift();
		}
	}

	public bool IsCanGetGift
	{
		get
		{
			return _isCanGetGift;
		}
		set
		{
			_isCanGetGift = value;
			SetStateColliderActive(_isCanGetGift);
			UpdateHUDStateGift();
		}
	}

	public bool CanShowLabel
	{
		get
		{
			if (this.CanShowNickLabel == null)
			{
				return true;
			}
			Delegate[] invocationList = this.CanShowNickLabel.GetInvocationList();
			foreach (Delegate @delegate in invocationList)
			{
				if ((object)@delegate != null && !(bool)@delegate.DynamicInvoke())
				{
					return false;
				}
			}
			return true;
		}
	}

	private bool CanTap
	{
		get
		{
			if (!MainMenuController.SavedShwonLobbyLevelIsLessThanActual() && TrainingController.TrainingCompleted && GiftController.Instance.ActiveGift && AnimationGift.instance.objGift.activeSelf && (MainMenuController.sharedController == null || !MainMenuController.sharedController.LeaderboardsIsOpening) && (FriendsWindowGUI.Instance == null || !FriendsWindowGUI.Instance.InterfaceEnabled) && (MainMenuController.sharedController == null || !MainMenuController.sharedController.rotateCamera.IsAnimPlaying))
			{
				if (!(LobbyCraftController.Instance == null))
				{
					return !LobbyCraftController.Instance.InterfaceEnabled;
				}
				return true;
			}
			return false;
		}
	}

	public static event Action onOpen;

	public event Func<bool> CanShowNickLabel;

	public void AddCanShowNickLabelChecker(Func<bool> checker)
	{
		CanShowNickLabel -= checker;
		CanShowNickLabel += checker;
		UpdateHUDStateGift();
	}

	public void RemoveCanShowNickLabelChecker(Func<bool> checker)
	{
		CanShowNickLabel -= checker;
		UpdateHUDStateGift();
	}

	public void Click()
	{
		if (CanTap)
		{
			GiftScroll.canReCreateSlots = true;
			ButtonClickSound.Instance.PlayClick();
			MainMenuController.canRotationLobbyPlayer = false;
			GiftBannerWindow.isForceClose = false;
			GiftBannerWindow.isActiveBanner = true;
			if (ButOpenGift.onOpen != null)
			{
				ButOpenGift.onOpen();
			}
			MainMenuController.sharedController.SaveShowPanelAndClose();
			MainMenuController.sharedController.OnShowBannerGift();
			GiftBannerWindow.instance.SetVisibleBanner(false);
			ActiveHighLight = false;
			UpdateHUDStateGift();
		}
	}

	private void OnPress(bool isDown)
	{
		if (CanTap)
		{
			IsPressBut = isDown;
			UpdateHUDStateGift();
		}
	}

	private void OnDragOut()
	{
		OnPress(false);
	}

	private void Awake()
	{
		instance = this;
		HOTween.Init();
		_activeHighLight = false;
		_isPressBut = false;
		_isCanGetGift = false;
		LobbyCraftController.OnInterfaceEnabledChanged += UpdateHUDStateGift;
		AddCanShowNickLabelChecker(NickLabelVisibleChecker_LobbyCraft);
	}

	private void OnEnable()
	{
		if (Billboard != null)
		{
			Billboard.BindTo(() => AnimationGift.instance.objGift.transform);
		}
	}

	private void Start()
	{
		GiftController.Instance.TryGetData();
		UpdateHUDStateGift();
	}

	private void OnDestroy()
	{
		instance = null;
		LobbyCraftController.OnInterfaceEnabledChanged -= UpdateHUDStateGift;
		RemoveCanShowNickLabelChecker(NickLabelVisibleChecker_LobbyCraft);
	}

	private bool NickLabelVisibleChecker_LobbyCraft()
	{
		if (!(LobbyCraftController.Instance == null))
		{
			return !LobbyCraftController.Instance.InterfaceEnabled;
		}
		return true;
	}

	public void UpdateHUDStateGift()
	{
		if (TrainingController.TrainingCompleted && GiftController.Instance != null && GiftController.Instance.ActiveGift)
		{
			if (ActiveHighLight)
			{
				if (IsPressBut)
				{
					SetStateClick(true);
				}
				else if (IsCanGetGift)
				{
					SetStateAnim();
				}
				else
				{
					SetStateClick(false);
				}
				if (CanShowLabel)
				{
					ShowLabelTap();
				}
				else
				{
					HideLabelTap();
				}
			}
			else
			{
				SetActiveHighLight(false);
				HideLabelTap();
			}
		}
		else
		{
			SetActiveHighLight(false);
			HideLabelTap();
			_isPressBut = false;
			_isCanGetGift = false;
			_activeHighLight = false;
		}
	}

	public void OpenGift()
	{
		if (GiftBannerWindow.instance != null)
		{
			GiftBannerWindow.instance.SetVisibleBanner(true);
		}
	}

	public void CloseGift()
	{
		if (GiftBannerWindow.instance != null)
		{
			GiftBannerWindow.instance.CloseBannerEndAnimtion();
		}
		ActiveHighLight = true;
		MainMenuController.canRotationLobbyPlayer = true;
	}

	private void SetStateClick(bool val)
	{
		StopAnim();
		if (val)
		{
			SetColor(pressColor);
		}
		else
		{
			SetColor(normalColor);
		}
	}

	private void SetStateAnim()
	{
		AnimHS2();
	}

	private void SetActiveHighLight(bool val)
	{
		if (!val)
		{
			StopAnim();
			SetColor(new Color(0f, 0f, 0f, 0f));
		}
	}

	private void StopAnim()
	{
		HOTween.Kill(this);
	}

	private void AnimHS2()
	{
		HOTween.Kill(this);
		HOTween.To(this, speedAnim, new TweenParms().Prop("curColorHS", animColor).OnUpdate((TweenDelegate.TweenCallback)delegate
		{
			SetColor(curColorHS);
		}).OnComplete(AnimHS1));
	}

	private void AnimHS1()
	{
		HOTween.Kill(this);
		HOTween.To(this, speedAnim, new TweenParms().Prop("curColorHS", normalColor).OnUpdate((TweenDelegate.TweenCallback)delegate
		{
			SetColor(curColorHS);
		}).OnComplete(AnimHS2));
	}

	private void SetColor(Color needColor)
	{
		curColorHS = needColor;
		if (allHighlightMaterial != null)
		{
			for (int i = 0; i < allHighlightMaterial.Length; i++)
			{
				allHighlightMaterial[i].SetColor(nameShaderColor, needColor);
			}
		}
	}

	private void ShowLabelTap()
	{
		if (Nest.Instance != null)
		{
			Nest.Instance.NickLabelVisiblCheckers -= NestNickLabelVisibleBlocker;
		}
		if (!(labelOverGift != null) || !labelOverGift.gameObject.activeSelf)
		{
			if (labelOverGift == null && NickLabelStack.sharedStack != null)
			{
				labelOverGift = NickLabelStack.sharedStack.GetNextCurrentLabel();
				labelOverGift.StartShow(NickLabelController.TypeNickLabel.GetGift, AnimationGift.instance.transform);
			}
			if (!labelOverGift.gameObject.activeSelf)
			{
				labelOverGift.gameObject.SetActive(true);
			}
		}
	}

	public void HideLabelTap()
	{
		if (TrainingController.TrainingCompleted && GiftController.Instance != null && GiftController.Instance.ActiveGift && Nest.Instance != null)
		{
			Nest.Instance.NickLabelVisiblCheckers -= NestNickLabelVisibleBlocker;
			Nest.Instance.NickLabelVisiblCheckers += NestNickLabelVisibleBlocker;
		}
		if (!(labelOverGift == null) && labelOverGift.gameObject.activeSelf && labelOverGift != null && labelOverGift.gameObject.activeSelf)
		{
			labelOverGift.gameObject.SetActive(false);
		}
	}

	private bool NestNickLabelVisibleBlocker()
	{
		return false;
	}

	private void SetStateColliderActive(bool val)
	{
		if ((bool)colActive)
		{
			colActive.enabled = val;
		}
		if ((bool)colNormal)
		{
			colNormal.enabled = !val;
		}
	}
}
