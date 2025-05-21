using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

public class ChooseMiniGameController : MonoBehaviour
{
	public Transform freeTicketsTimerPoint;

	public Transform ticketsIndicatorPoint;

	public List<UILabel> welcomeFreeTicketsLabels;

	public List<UILabel> welcomeAboutPeriodicalTicketsLabels;

	public List<UILabel> periodicalFreeTicketsCountLabels;

	public GameObject periodicalFreeTicketsBanner;

	public Transform startMiniGameScreenParent;

	public MiniGameCell campaignCell;

	public MiniGameCell sandboxCell;

	public MiniGameCell arenaCell;

	public MiniGameCell coopSurvivalCell;

	public MiniGameCell deadlyGamesCell;

	public MiniGameCell deathEscapeCell;

	public MiniGameCell spleefCell;

	public MiniGameCell speedRunCell;

	public Action onBackPressed;

	public GameObject welcomeTicketsBanner;

	private IDisposable m_backSubscriptionPeriodicalFreeTicketsBanner;

	private IDisposable m_backSubscriptionWelcomeBanner;

	private IDisposable m_backSubscription;

	public static bool IsFreeTicketsPeriodicalBannerShown { get; private set; }

	public static void AddFreeTicketsTimerToPoint(Transform parent)
	{
		Transform obj = UnityEngine.Object.Instantiate(Resources.Load<Transform>("MiniGames/FreeTicketsTimer"));
		obj.parent = parent;
		obj.localPosition = Vector3.zero;
		obj.localRotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		obj.gameObject.SetLayerRecursively(parent.gameObject.layer);
	}

	private void Awake()
	{
		AddFreeTicketsTimerToPoint(freeTicketsTimerPoint);
	}

	private void Start()
	{
		GameObject gameObject = ticketsIndicatorPoint.gameObject.Descendants().FirstOrDefault((GameObject desc) => desc.GetComponent<UIButton>() != null);
		if (gameObject != null)
		{
			EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(this, "HandleCoinsShop"));
		}
	}

	public void HandleCoinsShop()
	{
		EventHandler handleBackFromBank = null;
		handleBackFromBank = delegate
		{
			BankController.Instance.BackRequested -= handleBackFromBank;
			base.gameObject.SetActive(true);
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
		};
		BankController.Instance.BackRequested += handleBackFromBank;
		base.gameObject.SetActive(false);
		BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, "TicketsCurrency");
	}

	public void HandlePeriodicalFreeTicketsGet()
	{
		FreeTicketsController.Instance.GiveAccumulatedTickets();
		SetPeriodicalFreeTicketsBannerEnabled(false);
	}

	public void HandleMiniGamePressed(MiniGameCell miniGameCell)
	{
		ButtonClickSound.TryPlayClick();
		if (miniGameCell.type == GameConnect.GameMode.Campaign)
		{
			HandleCampaignPressed();
		}
		else
		{
			GoToStartMiniGameScreen(miniGameCell.type, miniGameCell.Parameters);
		}
	}

	public void HandleCampaignPressed()
	{
		try
		{
			MainMenuController.sharedController.MarkMiniGameModeAsNotNew(11);
			MainMenuController.sharedController.StartCampaingButton();
			MainMenuController.NavigateToMinigame = GameConnect.GameMode.Campaign;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ChooseMiniGameController HandleCampaignPressed: {0}", ex);
		}
	}

	public void SetEnabled(GameConnect.GameMode miniGame, bool enabled)
	{
		base.gameObject.SetActiveSafeSelf(enabled);
		UnsubscribeFromBackSystem();
		if (enabled)
		{
			m_backSubscription = BackSystem.Instance.Register(HandleBackPressed, "ChooseMiniGameController SetEnabled");
		}
		if (Storager.getInt("ChooseMiniGameController.FIRST_ENTER_TO_MINI_GAMES") == 0)
		{
			SetWelcomeBannerEnabled(true);
		}
		if (enabled)
		{
			SetupMiniGames();
			if (miniGame != GameConnect.GameMode.Campaign)
			{
				GoToStartMiniGameScreen(miniGame, BalanceController.ParametersForMiniGameType(miniGame));
			}
			else if (FreeTicketsController.Instance.IsCountingFreeTickets() && FreeTicketsController.Instance.NumOfAccumulatedTicketsToGive() > 0)
			{
				SetPeriodicalFreeTicketsBannerEnabled(true);
			}
		}
	}

	public void HandleCloseWelcomeBanner()
	{
		Storager.setInt("ChooseMiniGameController.FIRST_ENTER_TO_MINI_GAMES", 1);
		BankController.AddTickets(BalanceController.CountWelcomeTickets);
		SetWelcomeBannerEnabled(false);
		FreeTicketsController.Instance.ResetTimer();
	}

	public void HandleBackPressed()
	{
		SetEnabled(GameConnect.GameMode.Campaign, false);
		Action action = onBackPressed;
		if (action != null)
		{
			action();
		}
	}

	private void SetPeriodicalFreeTicketsBannerEnabled(bool enabled)
	{
		periodicalFreeTicketsBanner.SetActiveSafeSelf(enabled);
		UnsubscribeFromBackSystemPeriodicalFreeTicketsBanner();
		if (enabled)
		{
			m_backSubscriptionPeriodicalFreeTicketsBanner = BackSystem.Instance.Register(HandlePeriodicalFreeTicketsGet, "ChooseMiniGameController SetPeriodicalFreeTicketsBannerEnabled");
			SetPeriodicalFreeTicketsBannerCount();
		}
		IsFreeTicketsPeriodicalBannerShown = enabled;
	}

	public void SetWelcomeBannerEnabled(bool enabled)
	{
		welcomeTicketsBanner.SetActiveSafeSelf(enabled);
		UnsubscribeFromBackSystemWelcomeBanner();
		if (enabled)
		{
			m_backSubscriptionWelcomeBanner = BackSystem.Instance.Register(HandleCloseWelcomeBanner, "ChooseMiniGameController SetWelcomeBannerEnabled");
			RiliExtensions.ForEach(welcomeFreeTicketsLabels, delegate(UILabel label)
			{
				label.text = BalanceController.CountWelcomeTickets.ToString();
			});
			int freeTicketsPerPack = BalanceController.FreeTicketsPerPack;
			int num = BalanceController.TimeGiveFreeTickets / 3600;
			string welcomeAboutText = string.Format(LocalizationStore.Get("Key_2983"), freeTicketsPerPack, LocalizationStore.Get((freeTicketsPerPack > 1) ? "Key_3157" : "Key_3156"), num, LocalizationStore.Get((num > 1) ? "Key_3155" : "Key_3154"));
			RiliExtensions.ForEach(welcomeAboutPeriodicalTicketsLabels, delegate(UILabel l)
			{
				l.text = welcomeAboutText;
			});
		}
	}

	private void GoToStartMiniGameScreen(GameConnect.GameMode minigameType, MinigameParameters pars)
	{
		if (startMiniGameScreenParent.childCount == 0)
		{
			Transform obj = UnityEngine.Object.Instantiate(Resources.Load<Transform>("PanelMiniGame"));
			obj.parent = startMiniGameScreenParent;
			obj.localPosition = Vector3.zero;
			obj.localScale = Vector3.one;
		}
		startMiniGameScreenParent.GetChild(0).GetComponent<StartMiniGameGuiController>().Setup(minigameType, pars);
	}

	private void SetupMiniGames()
	{
		campaignCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.Campaign));
		sandboxCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.Dater));
		arenaCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.Arena));
		coopSurvivalCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.TimeBattle));
		deadlyGamesCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.DeadlyGames));
		deathEscapeCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.DeathEscape));
		spleefCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.Spleef));
		speedRunCell.Setup(BalanceController.ParametersForMiniGameType(GameConnect.GameMode.SpeedRun));
	}

	private void UnsubscribeFromBackSystemPeriodicalFreeTicketsBanner()
	{
		if (m_backSubscriptionPeriodicalFreeTicketsBanner != null)
		{
			m_backSubscriptionPeriodicalFreeTicketsBanner.Dispose();
			m_backSubscriptionPeriodicalFreeTicketsBanner = null;
		}
	}

	private void UnsubscribeFromBackSystemWelcomeBanner()
	{
		if (m_backSubscriptionWelcomeBanner != null)
		{
			m_backSubscriptionWelcomeBanner.Dispose();
			m_backSubscriptionWelcomeBanner = null;
		}
	}

	private void UnsubscribeFromBackSystem()
	{
		if (m_backSubscription != null)
		{
			m_backSubscription.Dispose();
			m_backSubscription = null;
		}
	}

	private void Update()
	{
		if (!welcomeTicketsBanner.activeSelf && (startMiniGameScreenParent.childCount <= 0 || !startMiniGameScreenParent.GetChild(0).gameObject.activeSelf))
		{
			if (periodicalFreeTicketsBanner.activeSelf && !FreeTicketsController.Instance.IsCountingFreeTickets())
			{
				SetPeriodicalFreeTicketsBannerEnabled(false);
			}
			if (periodicalFreeTicketsBanner.activeSelf)
			{
				SetPeriodicalFreeTicketsBannerCount();
			}
		}
	}

	private void OnDestroy()
	{
		UnsubscribeFromBackSystem();
	}

	private void SetPeriodicalFreeTicketsBannerCount()
	{
		try
		{
			int numOfTicketsToGive = FreeTicketsController.Instance.NumOfAccumulatedTicketsToGive();
			RiliExtensions.ForEach(periodicalFreeTicketsCountLabels, delegate(UILabel l)
			{
				l.text = numOfTicketsToGive.ToString();
			});
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in SetPeriodicalFreeTicketsBannerCount: {0}", ex);
		}
	}
}
