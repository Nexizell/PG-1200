using System;
using System.Collections.Generic;
using UnityEngine;

public class RanksTable : MonoBehaviour
{
	private const int maxCountInCommandPlusOther = 6;

	private const int maxCountInTeam = 5;

	public GameObject panelRanks;

	public GameObject panelRanksTeam;

	public GameObject tekPanel;

	public GameObject modePC1;

	public GameObject modeFC1;

	public GameObject modeTDM1;

	public ActionInTableButton[] playersButtonsDeathmatch;

	public ActionInTableButton[] playersButtonsTeamFight;

	private List<NetworkStartTable> tabs = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsBlue = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsRed = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsWhite = new List<NetworkStartTable>();

	public bool isShowRanks;

	public bool isShowTableWin;

	private bool isTeamMode;

	private string othersStr = "Others";

	public int totalBlue;

	public int totalRed;

	public int sumBlue;

	public int sumRed;

	private void Awake()
	{
		othersStr = LocalizationStore.Get("Key_1224");
		isTeamMode = GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints;
		string value;
		if (GameConnect.gameModesLocalizeKey.TryGetValue(Convert.ToInt32(GameConnect.gameMode), out value))
		{
			UILabel[] gameModeLabel = NetworkStartTableNGUIController.sharedController.gameModeLabel;
			for (int i = 0; i < gameModeLabel.Length; i++)
			{
				gameModeLabel[i].text = LocalizationStore.Get(value);
			}
		}
	}

	private void Start()
	{
		if (isTeamMode)
		{
			panelRanksTeam.SetActive(true);
			panelRanks.SetActive(false);
			modePC1.SetActive(GameConnect.gameMode == GameConnect.GameMode.CapturePoints);
			modeFC1.SetActive(GameConnect.gameMode == GameConnect.GameMode.FlagCapture);
			modeTDM1.SetActive(GameConnect.gameMode == GameConnect.GameMode.TeamFight);
		}
		else
		{
			panelRanksTeam.SetActive(false);
			panelRanks.SetActive(true);
		}
	}

	private void Update()
	{
		if (isShowRanks)
		{
			if (GameConnect.isMiniGame && MiniGamesController.Instance.isGo)
			{
				UpdateRanksForMiniGames();
				return;
			}
			ReloadTabsFromReal();
			UpdateRanksFromTabs();
		}
	}

	private void ReloadTabsFromReal()
	{
		tabsBlue.Clear();
		tabsRed.Clear();
		tabsWhite.Clear();
		tabs.Clear();
		tabs = NetworkStartTable.GetSortedTablesList();
		if (!isTeamMode)
		{
			return;
		}
		for (int i = 0; i < tabs.Count; i++)
		{
			if (tabs[i].myCommand == 1)
			{
				tabsBlue.Add(tabs[i]);
			}
			else if (tabs[i].myCommand == 2)
			{
				tabsRed.Add(tabs[i]);
			}
			else
			{
				tabsWhite.Add(tabs[i]);
			}
		}
	}

	private void FillButtonFromTable(ActionInTableButton button, NetworkStartTable table, int tableIndex, int team = 0)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		bool flag = false;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		int num = 0;
		Texture texture = null;
		flag = table.Equals(myNetworkStartTable);
		int num2 = Mathf.Max(0, table.CountKills);
		int num3 = Mathf.Max(0, table.score);
		text = num2.ToString();
		text2 = num3.ToString();
		text3 = table.pixelBookID.ToString();
		text4 = table.NamePlayer;
		num = table.myRanks;
		texture = table.myClanTexture;
		button.UpdateState(true, flag, team, text4, text2, text, num, texture, text3);
	}

	private void FillButtonFromScoreItem(ActionInTableButton button, ScoreTableItem scoreItem, int team = 0)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		int num = 0;
		Texture texture = null;
		bool isMine = scoreItem.isMine;
		int totalMiliSeconds = Mathf.Max(0, scoreItem.killCount);
		int num2 = Mathf.Max(0, scoreItem.score);
		text = (GameConnect.isDeathEscape ? DeathEscapeUI.FormatTime(totalMiliSeconds) : totalMiliSeconds.ToString());
		text2 = num2.ToString();
		text3 = scoreItem.pixelbookID.ToString();
		text4 = scoreItem.name;
		num = scoreItem.rank;
		texture = scoreItem.clanLogo;
		button.UpdateState(true, isMine, team, text4, text2, text, num, texture, text3, GameConnect.isMiniGame && scoreItem.isDead);
	}

	private void FillDeathmatchButtons(bool oldState = false)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		for (int i = 0; i < playersButtonsDeathmatch.Length; i++)
		{
			if (!oldState && i < tabs.Count)
			{
				FillButtonFromTable(playersButtonsDeathmatch[i], tabs[i], i);
			}
			else if (oldState && myNetworkStartTable.oldPlayersList != null && i < myNetworkStartTable.oldPlayersList.Length)
			{
				FillButtonFromScoreItem(playersButtonsDeathmatch[i], myNetworkStartTable.oldPlayersList[i], i);
			}
			else
			{
				playersButtonsDeathmatch[i].UpdateState(false);
			}
		}
	}

	private void FillMinigamesButtons()
	{
		List<ScoreTableItem> sortedScoresList = NetworkStartTable.GetSortedScoresList();
		for (int i = 0; i < playersButtonsDeathmatch.Length; i++)
		{
			if (i < sortedScoresList.Count)
			{
				FillButtonFromScoreItem(playersButtonsDeathmatch[i], sortedScoresList[i], i);
			}
			else
			{
				playersButtonsDeathmatch[i].UpdateState(false);
			}
		}
	}

	private void FillTeamButtons(bool oldState = false)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		int num = Mathf.Max(0, oldState ? myNetworkStartTable.myCommandOld : myNetworkStartTable.myCommand);
		sumRed = 0;
		sumBlue = 0;
		for (int i = 0; i < playersButtonsTeamFight.Length / 2; i++)
		{
			if (!oldState && i < Mathf.Min(tabsBlue.Count, 5))
			{
				sumBlue += ((tabsBlue[i].CountKills != -1) ? tabsBlue[i].CountKills : 0);
				FillButtonFromTable(playersButtonsTeamFight[i + ((num == 2) ? 6 : 0)], tabsBlue[i], i, num);
			}
			else if (oldState && i < Mathf.Min(myNetworkStartTable.oldBluePlayersList.Length, 5))
			{
				sumBlue += ((myNetworkStartTable.oldBluePlayersList[i].killCount != -1) ? myNetworkStartTable.oldBluePlayersList[i].killCount : 0);
				FillButtonFromScoreItem(playersButtonsTeamFight[i + ((num == 2) ? 6 : 0)], myNetworkStartTable.oldBluePlayersList[i], num);
			}
			else if (totalBlue - sumBlue > 0 && i == 5 && GameConnect.gameMode != GameConnect.GameMode.CapturePoints)
			{
				playersButtonsTeamFight[i + ((num == 2) ? 6 : 0)].UpdateState(true, false, num, othersStr, string.Empty, (totalBlue - sumBlue).ToString(), -1);
			}
			else
			{
				playersButtonsTeamFight[i + ((num == 2) ? 6 : 0)].UpdateState(false);
			}
			if (!oldState && i < Mathf.Min(tabsRed.Count, 5))
			{
				sumRed += ((tabsRed[i].CountKills != -1) ? tabsRed[i].CountKills : 0);
				ActionInTableButton button = playersButtonsTeamFight[i + ((num != 2) ? 6 : 0)];
				NetworkStartTable table = tabsRed[i];
				int tableIndex = i;
				int team;
				switch (num)
				{
				default:
					team = 2;
					break;
				case 2:
					team = 1;
					break;
				case 0:
					team = 0;
					break;
				}
				FillButtonFromTable(button, table, tableIndex, team);
			}
			else if (oldState && i < Mathf.Min(myNetworkStartTable.oldRedPlayersList.Length, 5))
			{
				sumRed += ((myNetworkStartTable.oldRedPlayersList[i].killCount != -1) ? myNetworkStartTable.oldRedPlayersList[i].killCount : 0);
				ActionInTableButton button2 = playersButtonsTeamFight[i + ((num != 2) ? 6 : 0)];
				ScoreTableItem scoreItem = myNetworkStartTable.oldRedPlayersList[i];
				int team2;
				switch (num)
				{
				default:
					team2 = 2;
					break;
				case 2:
					team2 = 1;
					break;
				case 0:
					team2 = 0;
					break;
				}
				FillButtonFromScoreItem(button2, scoreItem, team2);
			}
			else if (totalRed - sumRed > 0 && i == 5 && GameConnect.gameMode != GameConnect.GameMode.CapturePoints)
			{
				ActionInTableButton obj = playersButtonsTeamFight[i + ((num != 2) ? 6 : 0)];
				int command;
				switch (num)
				{
				default:
					command = 2;
					break;
				case 2:
					command = 1;
					break;
				case 0:
					command = 0;
					break;
				}
				obj.UpdateState(true, false, command, othersStr, string.Empty, (totalRed - sumRed).ToString(), -1);
			}
			else
			{
				playersButtonsTeamFight[i + ((num != 2) ? 6 : 0)].UpdateState(false);
			}
		}
		if (oldState && GameConnect.gameMode != GameConnect.GameMode.CapturePoints)
		{
			if (totalBlue < sumBlue)
			{
				totalBlue = sumBlue;
			}
			if (totalRed < sumRed)
			{
				totalRed = sumRed;
			}
		}
		for (int j = 0; j < NetworkStartTableNGUIController.sharedController.totalBlue.Length; j++)
		{
			NetworkStartTableNGUIController.sharedController.totalBlue[j].text = ((num != 2) ? totalBlue.ToString() : totalRed.ToString());
		}
		for (int k = 0; k < NetworkStartTableNGUIController.sharedController.totalRed.Length; k++)
		{
			NetworkStartTableNGUIController.sharedController.totalRed[k].text = ((num != 2) ? totalRed.ToString() : totalBlue.ToString());
		}
	}

	private void UpdateRanksFromTabs()
	{
		if (GameConnect.isCompany)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				totalBlue = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue;
				totalRed = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed;
			}
			else
			{
				totalBlue = GlobalGameController.countKillsBlue;
				totalRed = GlobalGameController.countKillsRed;
			}
		}
		if (GameConnect.isFlag && WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			totalBlue = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1;
			totalRed = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2;
		}
		if (GameConnect.isCapturePoints)
		{
			totalBlue = Mathf.RoundToInt(CapturePointController.sharedController.scoreBlue);
			totalRed = Mathf.RoundToInt(CapturePointController.sharedController.scoreRed);
		}
		if (isTeamMode)
		{
			FillTeamButtons();
		}
		else
		{
			FillDeathmatchButtons();
		}
	}

	public void UpdateRanksFromOldSpisok()
	{
		if (isTeamMode)
		{
			FillTeamButtons(true);
		}
		else
		{
			FillDeathmatchButtons(true);
		}
	}

	public void UpdateRanksForMiniGames()
	{
		FillMinigamesButtons();
	}
}
