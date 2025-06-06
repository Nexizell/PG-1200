using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(UIGrid))]
public class LeagueItemsView : MonoBehaviour
{
	[SerializeField]
	protected internal UILabel _headerText;

	private UIGrid _grid;

	private LeagueItemStot[] _slots;

	private void Awake()
	{
		_grid = GetComponent<UIGrid>();
		_slots = GetComponentsInChildren<LeagueItemStot>(true);
	}

	public void Repaint(RatingSystem.RatingLeague league)
	{
		int num = 0;
		foreach (WeaponSkin item in WeaponSkinsManager.SkinsForLeague(league))
		{
			LeagueItemStot obj = _slots[num];
			Texture itemIcon = ItemDb.GetItemIcon(item.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory);
			obj.Set(itemIcon, RatingSystem.instance.currentLeague >= league, WeaponSkinsManager.IsBoughtSkin(item.Id));
			num++;
		}
		List<string> list = Wear.LeagueItemsByLeagues()[league];
		foreach (string item2 in list)
		{
			LeagueItemStot obj2 = _slots[num];
			Texture itemIcon2 = ItemDb.GetItemIcon(item2, ShopNGUIController.CategoryNames.HatsCategory);
			List<Wear.LeagueItemState> statesForItem = GetStatesForItem(item2);
			obj2.Set(itemIcon2, statesForItem.Contains(Wear.LeagueItemState.Open), statesForItem.Contains(Wear.LeagueItemState.Purchased));
			num++;
		}
		foreach (SkinItem item3 in (from kvp in SkinsController.sharedController.skinItemsDict
			where kvp.Value.currentLeague == league
			select kvp.Value).ToList())
		{
			LeagueItemStot leagueItemStot = _slots[num];
			Texture texture = Resources.Load<Texture>(string.Format("LeagueSkinsProfileImages/league{0}_skin_profile", new object[1] { (int)(league + 1) }));
			bool isForMoneySkin = false;
			leagueItemStot.Set(texture, RatingSystem.instance.currentLeague >= league, SkinsController.IsSkinBought(item3.name, out isForMoneySkin));
			num++;
		}
		_headerText.gameObject.SetActive(list.Any());
		_grid.Reposition();
	}

	private List<Wear.LeagueItemState> GetStatesForItem(string itemId)
	{
		List<Wear.LeagueItemState> res = new List<Wear.LeagueItemState>();
		Dictionary<Wear.LeagueItemState, List<string>> items = Wear.LeagueItems();
		RiliExtensions.ForEachEnum(delegate(Wear.LeagueItemState val)
		{
			if (items[val].Contains(itemId))
			{
				res.Add(val);
			}
		});
		return res;
	}
}
