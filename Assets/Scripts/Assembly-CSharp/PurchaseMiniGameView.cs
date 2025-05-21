using Rilisoft;
using UnityEngine;

public sealed class PurchaseMiniGameView : MonoBehaviour
{
	public UITexture icon;

	public UILabel[] price;

	public UILabel[] itemName;

	public UISprite priceCurrency;

	public UIButton[] buttons;

	public GameObject equipped;

	public PurchaseMiniGameItem Item { get; private set; }

	public void Setup(PurchaseMiniGameItem item)
	{
		Item = item;
		icon.mainTexture = ItemDb.GetItemIcon(item.Id, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(item.Id));
		itemName.ForEach(delegate(UILabel lab)
		{
			lab.text = ItemDb.GetItemNameByTag(item.Id);
		});
		price.ForEach(delegate(UILabel lab)
		{
			lab.text = ((item.price.Price > 0) ? item.price.Price.ToString() : string.Empty);
		});
		priceCurrency.gameObject.SetActiveSafeSelf(item.price.Price > 0);
	}
}
