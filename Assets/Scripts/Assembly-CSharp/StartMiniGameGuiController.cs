using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

[RequireComponent(typeof(RateMiniGameController))]
public sealed class StartMiniGameGuiController : MonoBehaviour
{
	public Transform freeTicketsTimerPoint;

	public Transform ticketsIndicatorPoint;

	[Obsolete("Use `_rateStarsContainer` instead.")]
	public UIButton rateButton;

	public UITexture miniGameImage;

	public List<UILabel> miniGameName;

	public List<UILabel> miniGameDescription;

	public List<UILabel> priceLabels;

	[SerializeField]
	protected internal GameObject _rateStarsContainer;

	[SerializeField]
	protected internal GameObject _rateUsLabel;

	private RateMiniGameController _rateMiniGameController;

	private LeaderboardsMiniGameController _leaderboardsMiniGameController;

	private IDisposable m_backSubscription;

	public MinigameParameters Parameters { get; private set; }

	public GameConnect.GameMode Type { get; private set; }

	private RateMiniGameController RateMiniGameController
	{
		get
		{
			if (_rateMiniGameController == null)
			{
				_rateMiniGameController = GetComponent<RateMiniGameController>();
			}
			return _rateMiniGameController;
		}
	}

	private LeaderboardsMiniGameController LeaderboardsMiniGameController
	{
		get
		{
			if (_leaderboardsMiniGameController == null)
			{
				_leaderboardsMiniGameController = GetComponent<LeaderboardsMiniGameController>();
			}
			return _leaderboardsMiniGameController;
		}
	}

	private void Awake()
	{
		ChooseMiniGameController.AddFreeTicketsTimerToPoint(freeTicketsTimerPoint);
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

	public void Setup(GameConnect.GameMode typeMiniGame, MinigameParameters pars)
	{
		base.gameObject.SetActiveSafeSelf(true);
		Parameters = pars;
		Type = typeMiniGame;
		RateMiniGameController.GameMode = typeMiniGame;
		LeaderboardsMiniGameController.CurrentGameMode = typeMiniGame;
		bool state = NumberOfMatchesPlayedInThisMode() > 0;
		rateButton.gameObject.SetActiveSafeSelf(false);
		_rateStarsContainer.SetActiveSafeSelf(state);
		_rateUsLabel.SetActiveSafeSelf(state);
		UnsubscribeFromBackSystem();
		m_backSubscription = BackSystem.Instance.Register(HandleBackButtonPressed, "StartMiniGameGuiController");
		RiliExtensions.ForEach(miniGameName, delegate(UILabel label)
		{
			label.text = LocalizationStore.Get((Type == GameConnect.GameMode.Arena) ? "Key_0456" : GameConnect.gameModesLocalizeKey[(int)Type]);
		});
		RiliExtensions.ForEach(priceLabels, delegate(UILabel label)
		{
			label.text = Parameters.TicketsPrice.ToString();
		});
		RiliExtensions.ForEach(miniGameDescription, delegate(UILabel label)
		{
			label.text = LocalizationStore.Get((Type == GameConnect.GameMode.Arena) ? "Key_0974" : GameConnect.gameModesRulesLocalizeKey[(int)Type]);
		});
		miniGameImage.mainTexture = Resources.Load<Texture>(string.Format("MiniGames/Preview/{0}_preview", new object[1] { Type }));
		MainMenuController.sharedController.MarkMiniGameModeAsNotNew((int)Type);
	}

	public void HandleBackButtonPressed()
	{
		base.gameObject.SetActiveSafeSelf(false);
		UnsubscribeFromBackSystem();
	}

	public void HandleRatePressed()
	{
		RateMiniGameController.SetVisibility(true);
	}

	public void HandleGoPressed()
	{
		int ticketsPrice = Parameters.TicketsPrice;
		ItemPrice priceOfTheMatch = new ItemPrice(ticketsPrice, "TicketsCurrency");
		ShopNGUIController.TryToBuy(base.gameObject, priceOfTheMatch, delegate
		{
			if (Type != GameConnect.GameMode.Dater)
			{
				AnalyticsStuff.MiniGames(Type);
			}
			if (Type == GameConnect.GameMode.Arena)
			{
				IncreaseOfMatchesPlayedInThisMode();
				GoToArena();
				MainMenuController.NavigateToMinigame = Type;
			}
			else
			{
				if (Type == GameConnect.GameMode.SpeedRun)
				{
					IncreaseOfMatchesPlayedInThisMode();
					try
					{
						MainMenuController.sharedController.StartSpeedrunButton();
						MainMenuController.NavigateToMinigame = Type;
						return;
					}
					catch (Exception ex)
					{
						Debug.LogErrorFormat("Exception in StartMiniGameGuiController StartSpeedrunButton: {0}", ex);
						return;
					}
				}
				GameConnect.GameMode targetGameMode = Type;
				Action action = delegate
				{
					try
					{
						GameConnect.SetGameMode(targetGameMode);
						MainMenuController.sharedController.connectionControl.ConnectToPhoton(delegate
						{
							MainMenuController.sharedController.connectionControl.JoinRandomRoom(delegate
							{
								MainMenuController.NavigateToMinigame = Type;
								AnalyticsStuff.TicketsSpended(Type.ToString(), ticketsPrice);
								BankController.SpendMoney(priceOfTheMatch);
								IncreaseOfMatchesPlayedInThisMode();
								PhotonNetwork.isMessageQueueRunning = false;
								CoroutineRunner.Instance.StartCoroutine(MainMenuController.MoveToGameScene());
							});
						});
					}
					catch (Exception ex2)
					{
						Debug.LogErrorFormat("Exception in ChooseMiniGameController HandleGoPressed(): {0}", ex2);
					}
				};
				if (targetGameMode == GameConnect.GameMode.DeathEscape)
				{
					if (CheckpointsRewardedVideoController.Instance != null && CheckpointsRewardedVideoController.Instance.AreFreeCheckpointsAvailable())
					{
						if (!CheckpointsRewardedVideoController.Instance.TryShowFreCheckpointsInterface(action))
						{
							action();
						}
					}
					else
					{
						action();
					}
				}
				else
				{
					action();
				}
			}
		}, null, null, null, null, null, false, Type != GameConnect.GameMode.Arena && Type != GameConnect.GameMode.SpeedRun, delegate
		{
			if (Type == GameConnect.GameMode.Arena || Type == GameConnect.GameMode.SpeedRun)
			{
				AnalyticsStuff.TicketsSpended(Type.ToString(), ticketsPrice);
			}
		});
	}

	private static void GoToArena()
	{
		try
		{
			MainMenuController.sharedController.StartSurvivalButton();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ChooseMiniGameController GoToArena: {0}", ex);
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

	private int NumberOfMatchesPlayedInThisMode()
	{
		return Storager.getInt(string.Format("StartMiniGameGuiController.NumberOfMatches_{0}", new object[1] { Type }));
	}

	private void IncreaseOfMatchesPlayedInThisMode()
	{
		Storager.setInt(string.Format("StartMiniGameGuiController.NumberOfMatches_{0}", new object[1] { Type }), NumberOfMatchesPlayedInThisMode() + 1);
	}

	private void OnDestroy()
	{
		UnsubscribeFromBackSystem();
	}
}
