using UnityEngine;

public class ExchangeTicketsBankViewItem : ExchangeBankViewItem
{
	protected override void SetIcon()
	{
		string path = "Textures/Bank/Static_Bank_Textures/Tickets_Shop_" + base.ExchangeInfo.InAppId;
		icon.mainTexture = Resources.Load<Texture>(path);
	}
}
