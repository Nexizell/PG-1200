using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemEffectFreeGems : LobbyItemEffectPerInterval
	{
		protected override long RewardInterval
		{
			get
			{
				return 86400L;
			}
		}

		protected override bool GiveReward(int dropIntervalsElapsed)
		{
			if (base.LobbyItem == null)
			{
				return false;
			}
			if (MainMenuController.sharedController == null || !MainMenuController.sharedController.mainPanel.activeInHierarchy)
			{
				return false;
			}
			BankController.AddGems((int)(from b in base.LobbyItem.Info.Buffs
				where b.BuffType == LobbyItemInfo.LobbyItemBuffType.FreeGemsPerDay
				select b into m
				select m.Value).Sum() * dropIntervalsElapsed);
			return true;
		}
	}
}
