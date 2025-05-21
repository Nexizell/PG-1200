using System;
using Rilisoft;
using UnityEngine;

public class PauseGUIController : MonoBehaviour
{
	[SerializeField]
	protected internal UIButton _btnResume;

	[SerializeField]
	protected internal UIButton _btnExit;

	[SerializeField]
	protected internal UIButton _btnFinish;

	[SerializeField]
	protected internal UIButton _btnSettings;

	[SerializeField]
	protected internal UIButton _btnBank;

	[SerializeField]
	protected internal GameObject _btnsPanel;

	[SerializeField]
	protected internal GameObject _confirmDialogPanel;

	[SerializeField]
	protected internal UIButton _btnDialogYes;

	[SerializeField]
	protected internal UIButton _btnDialogNo;

	[SerializeField]
	protected internal UILabel _btnDialogText;

	[SerializeField]
	protected internal PrefabHandler _settingsPrefab;

	private IDisposable _backSubscription;

	private LazyObject<PauseNGUIController> _pauseNguiLazy;

	private bool _shopOpened;

	private float _lastBackFromShopTime;

	public static PauseGUIController Instance { get; private set; }

	public PauseNGUIController SettingsPanel
	{
		get
		{
			if (_pauseNguiLazy == null)
			{
				_pauseNguiLazy = new LazyObject<PauseNGUIController>(_settingsPrefab.ResourcePath, InGameGUI.sharedInGameGUI.SubpanelsContainer);
			}
			return _pauseNguiLazy.Value;
		}
	}

	public bool IsPaused
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	private bool InPauseShop
	{
		get
		{
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				return InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen;
			}
			return false;
		}
	}

	private void HandleButtonFinishClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (GameConnect.isDeathEscape && CheckpointController.instance != null && CheckpointController.instance.runFinishedOnce && MiniGamesController.Instance != null)
		{
			MiniGamesController.Instance.FinishGame();
		}
	}

	private void HandleButtonExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		_confirmDialogPanel.SetActive(false);
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.blockedCollider.SetActive(true);
			InGameGUI.sharedInGameGUI.playerMoveC.QuitGame();
		}
		else if (PhotonNetwork.room != null)
		{
			coinsShop.hideCoinsShop();
			coinsPlashka.hidePlashka();
			Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
			PhotonNetwork.LeaveRoom();
		}
	}

	private void HandleDialogBackClick()
	{
		ButtonClickSound.Instance.PlayClick();
		ShowConfirmDialog(false, 0);
	}

	private void ShowConfirmDialog(bool visible, int ratingValue)
	{
		ButtonClickSound.Instance.PlayClick();
		_btnsPanel.SetActive(!visible);
		_confirmDialogPanel.SetActive(visible);
		if (visible)
		{
			_btnDialogText.text = string.Format(LocalizationStore.Get("Key_2950"), new object[1] { Mathf.Abs(ratingValue).ToString() });
			if (string.IsNullOrEmpty(_btnDialogText.text))
			{
				_btnDialogText.text = ratingValue.ToString();
			}
		}
	}

	private void Awake()
	{
		Instance = this;
		_btnResume.GetComponent<ButtonHandler>().Clicked += delegate
		{
			Close();
		};
		_btnExit.GetComponent<ButtonHandler>().Clicked += delegate
		{
			if (!InPauseShop && !(Time.realtimeSinceStartup < _lastBackFromShopTime + 0.5f))
			{
				if (WeaponManager.sharedManager.myNetworkStartTable != null)
				{
					int currentRatingChange = WeaponManager.sharedManager.myNetworkStartTable.GetCurrentRatingChange(true);
					if (currentRatingChange < 0)
					{
						ShowConfirmDialog(true, currentRatingChange);
						return;
					}
				}
				HandleButtonExitClick();
			}
		};
		_btnFinish.GetComponent<ButtonHandler>().Clicked += delegate
		{
			if (!InPauseShop)
			{
				HandleButtonFinishClick();
			}
		};
		_btnDialogYes.GetComponent<ButtonHandler>().Clicked += delegate
		{
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.exitFromMenu = true;
			}
			HandleButtonExitClick();
		};
		_btnDialogNo.GetComponent<ButtonHandler>().Clicked += delegate
		{
			HandleDialogBackClick();
		};
		if (_btnBank != null)
		{
			_btnBank.GetComponent<ButtonHandler>().Clicked += delegate
			{
				ShopClickButton();
			};
		}
		_btnSettings.GetComponent<ButtonHandler>().Clicked += delegate
		{
			SettingsPanel.gameObject.SetActive(true);
		};
	}

	private void ShopClickButton()
	{
		if (!InPauseShop)
		{
			ButtonClickSound.Instance.PlayClick();
			ExperienceController.sharedController.isShowRanks = false;
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.GoToShopFromPause();
			}
		}
	}

	private void OnEnable()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && _btnBank != null)
		{
			_btnBank.gameObject.SetActive(false);
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(Close, "Pause window");
		if (GameConnect.isDeathEscape && CheckpointController.instance != null)
		{
			_btnFinish.gameObject.SetActive(CheckpointController.instance.runFinishedOnce);
			_btnExit.gameObject.SetActive(!CheckpointController.instance.runFinishedOnce);
		}
		ShopTapReceiver.AddClickHndIfNotExist(ShopClickButton);
	}

	private void Update()
	{
		if (!InPauseShop)
		{
			if (_shopOpened)
			{
				_lastBackFromShopTime = Time.realtimeSinceStartup;
			}
			_shopOpened = false;
			if (!Defs.isMulti)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
		else
		{
			_shopOpened = true;
			_lastBackFromShopTime = float.PositiveInfinity;
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		ShopTapReceiver.ShopClicked -= ShopClickButton;
	}

	private void Close()
	{
		if (!InPauseShop)
		{
			ShowConfirmDialog(false, 0);
			ButtonClickSound.Instance.PlayClick();
			Debug.Log((InGameGUI.sharedInGameGUI != null) + " " + (InGameGUI.sharedInGameGUI.playerMoveC != null));
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.SetPause();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
		}
	}
}
