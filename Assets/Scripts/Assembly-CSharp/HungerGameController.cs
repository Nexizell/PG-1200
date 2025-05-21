using UnityEngine;

public sealed class HungerGameController : MiniGamesController
{
	protected override void CheckMatchEndConditions()
	{
		bool flag = InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.pausePanel.activeSelf;
		if (Initializer.players.Count == 1 && MiniGamesController.Instance.isGo && timeInGame > 10f && !flag)
		{
			WeaponManager.sharedManager.myNetworkStartTable.WinInHunger();
		}
	}
}
