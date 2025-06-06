using UnityEngine;

public class BtnBannerSmile : ButtonBannerBase
{
	public string tagForClick = "";

	public override bool BannerIsActive()
	{
		return !StickersController.IsBuyAllPack();
	}

	public override void OnClickButton()
	{
		MainMenuController.sharedController.HandlePromoActionClicked(tagForClick);
	}

	public override void OnHide()
	{
	}

	public override void OnShow()
	{
	}

	public override void OnChangeLocalize()
	{
	}
}
