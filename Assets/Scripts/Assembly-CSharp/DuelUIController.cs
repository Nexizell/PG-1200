using System;
using Rilisoft;
using UnityEngine;

public class DuelUIController : MonoBehaviour
{
	public DuelPlayerInfo myPlayerInfo;

	public DuelPlayerInfo opponentPlayerInfo;

	public GameObject waitForOpponent;

	public GameObject versusInterface;

	public GameObject endInterface;

	public GameObject changeArea;

	public GameObject rewardPanel;

	public GameObject opponentLeftLabel;

	public UILabel myWinLabel;

	public UILabel myLooseLabel;

	public UILabel myDHLabel;

	public UILabel enemyWinLabel;

	public UILabel enemyLooseLabel;

	public UILabel enemyDHLabel;

	public UILabel expReward;

	public UILabel coinReward;

	public UILabel myPoints;

	public UILabel myKills;

	public UILabel enemyPoints;

	public UILabel enemyKills;

	public UILabel versusTimer;

	public ButtonHandler nextButton;

	public ButtonHandler nextEnemyButton;

	public ButtonHandler revengeButton;

	[HideInInspector]
	public bool showCharacters;

	public GameObject revengePanel;

	public UILabel revengeLabel;

	public string revengeButtonSpriteBlink;

	private string revengeButtonSpriteNormal;

	private UIButton revengeUIButton;

	private bool blinkRevengeButton;

	private bool inBlink;

	private float blinkTime;

	public DuelController duelController
	{
		get
		{
			return DuelController.instance;
		}
	}

	private void Awake()
	{
		nextEnemyButton.Clicked += NextEnemyButtonClick;
		nextButton.Clicked += NextButtonClick;
		revengeButton.Clicked += RevengeButtonClick;
		revengeUIButton = revengeButton.GetComponent<UIButton>();
		revengeButtonSpriteNormal = revengeUIButton.normalSprite;
	}

	public void ShowRevengePanel(bool revengeReceived, bool opponentLeft)
	{
		revengePanel.SetActive(true);
		blinkRevengeButton = revengeReceived;
		if (opponentLeft)
		{
			revengeLabel.text = LocalizationStore.Get("Key_2433");
			revengeButton.GetComponent<UIButton>().isEnabled = false;
		}
		else
		{
			revengeLabel.text = LocalizationStore.Get(revengeReceived ? "Key_2434" : "Key_2435");
		}
	}

	public void HideRevengePanel()
	{
		revengePanel.SetActive(false);
		blinkRevengeButton = false;
		revengeButton.GetComponent<UIButton>().normalSprite = revengeButtonSpriteNormal;
		inBlink = false;
	}

	public void ShowStartInterface()
	{
		WeaponManager.sharedManager.myNetworkStartTable.isShowFinished = true;
		ShowWaitForOpponentInterface();
	}

	public void ShowChangeAreaInterface(bool opponentLeft = false)
	{
		if (duelController.gameStatus == DuelController.GameStatus.End)
		{
			duelController.SendGameLeft();
			duelController.gameStatus = DuelController.GameStatus.ChangeArea;
			showCharacters = true;
			endInterface.SetActive(false);
			waitForOpponent.SetActive(false);
			versusInterface.SetActive(false);
			changeArea.SetActive(true);
			opponentLeftLabel.SetActive(opponentLeft);
		}
	}

	public void ShowWaitForOpponentInterface()
	{
		showCharacters = true;
		waitForOpponent.SetActive(true);
		versusInterface.SetActive(false);
		myPlayerInfo.SetPointInWorld(duelController.myCharacter.hatPoint, duelController.myCharacter.transform);
		opponentPlayerInfo.SetPointInWorld(duelController.enemyCharacter.hatPoint, duelController.enemyCharacter.transform);
	}

	public void ShowVersusUI()
	{
		showCharacters = true;
		endInterface.SetActive(false);
		waitForOpponent.SetActive(false);
		versusInterface.SetActive(true);
		changeArea.SetActive(false);
	}

	public void IngameUI()
	{
		HideRevengePanel();
		WeaponManager.sharedManager.myNetworkStartTable.isShowFinished = false;
		showCharacters = false;
		versusInterface.SetActive(false);
		waitForOpponent.SetActive(false);
		changeArea.SetActive(false);
	}

	public void UpdateCharactersInfo()
	{
		opponentPlayerInfo.gameObject.SetActive(duelController.showEnemyCharacter);
		if (duelController.showEnemyCharacter && duelController.opponentNetworkTable != null)
		{
			opponentPlayerInfo.FillByTable(duelController.opponentNetworkTable);
		}
		myPlayerInfo.gameObject.SetActive(duelController.showMyCharacter);
		if (duelController.showMyCharacter)
		{
			myPlayerInfo.FillByTable(WeaponManager.sharedManager.myNetworkStartTable);
		}
	}

	private void Update()
	{
		UpdateCharactersInfo();
		if (blinkRevengeButton && blinkTime < Time.time)
		{
			inBlink = !inBlink;
			blinkTime = Time.time + 0.3f;
			revengeUIButton.normalSprite = (inBlink ? revengeButtonSpriteBlink : revengeButtonSpriteNormal);
		}
	}

	public void ShowFinishedInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool isWinner, bool deadHeat)
	{
		base.gameObject.SetActive(true);
		coinReward.text = _addCoin.ToString();
		expReward.text = _addExpierence.ToString();
		myWinLabel.gameObject.SetActive(isWinner && !deadHeat);
		myLooseLabel.gameObject.SetActive(!isWinner && !deadHeat);
		myDHLabel.gameObject.SetActive(deadHeat);
		enemyWinLabel.gameObject.SetActive(!isWinner && !deadHeat);
		enemyLooseLabel.gameObject.SetActive(isWinner && !deadHeat);
		enemyDHLabel.gameObject.SetActive(deadHeat);
		if (duelController.opponentNetworkTable != null && duelController.opponentNetworkTable.myPlayerMoveC != null)
		{
			duelController.opponentNetworkTable.myPlayerMoveC.mySkinName.gameObject.SetActive(false);
		}
	}

	public void ShowEndInterface()
	{
		if (Initializer.networkTables.Count < 2)
		{
			ShowChangeAreaInterface();
			return;
		}
		showCharacters = true;
		NetworkStartTable.LocalOrPasswordRoom();
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		endInterface.SetActive(true);
		ScoreTableItem scoreTableItem = null;
		ScoreTableItem scoreTableItem2 = null;
		for (int i = 0; i < myNetworkStartTable.oldPlayersList.Length; i++)
		{
			if (!myNetworkStartTable.oldPlayersList[i].isMine)
			{
				scoreTableItem2 = myNetworkStartTable.oldPlayersList[i];
			}
			else
			{
				scoreTableItem = myNetworkStartTable.oldPlayersList[i];
			}
			if (scoreTableItem != null && scoreTableItem2 != null)
			{
				break;
			}
		}
		myKills.text = ((scoreTableItem.killCount == -1) ? "0" : scoreTableItem.killCount.ToString());
		myPoints.text = ((scoreTableItem.score == -1) ? "0" : scoreTableItem.score.ToString());
		if (scoreTableItem2 != null)
		{
			enemyKills.text = ((scoreTableItem2.killCount == -1) ? "0" : scoreTableItem2.killCount.ToString());
			enemyPoints.text = ((scoreTableItem2.score == -1) ? "0" : scoreTableItem2.score.ToString());
		}
		ExperienceController.sharedController.isShowRanks = true;
		myPlayerInfo.SetPointInWorld(duelController.myCharacter.hatPoint, duelController.myCharacter.transform);
		opponentPlayerInfo.SetPointInWorld(duelController.enemyCharacter.hatPoint, duelController.enemyCharacter.transform);
		duelController.SetShopEvents();
	}

	public void NextButtonClick(object sender, EventArgs e)
	{
		ShowChangeAreaInterface();
	}

	public void NextEnemyButtonClick(object sender, EventArgs e)
	{
		WeaponManager.sharedManager.myNetworkStartTable.RandomRoomClickBtnInDuel();
	}

	public void RevengeButtonClick(object sender, EventArgs e)
	{
		duelController.RevengeRequest();
	}
}
