using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemEffectFreeSpins : LobbyItemEffectPerInterval
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
			if (GiftController.Instance == null)
			{
				return false;
			}
			int num = (int)(from b in base.LobbyItem.Info.Buffs
				where b.BuffType == LobbyItemInfo.LobbyItemBuffType.FreeSpinsPerDay
				select b into m
				select m.Value).Sum();
			GiftController.Instance.IncrementFreeSpins(num * dropIntervalsElapsed);
			return true;
		}
	}
}
