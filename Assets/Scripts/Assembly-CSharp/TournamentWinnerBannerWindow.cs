using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public class TournamentWinnerBannerWindow : BannerWindow
{
	private static readonly PrefsBoolCachedProperty _canShow = new PrefsBoolCachedProperty("TournamentWinnerBannerWindow_needShow");

	[SerializeField]
	protected internal GameObject _coinsIconObj;

	[SerializeField]
	protected internal GameObject _gemsIconObj;

	[SerializeField]
	protected internal TextGroup _RewardCoinsTextGroup;

	[SerializeField]
	protected internal TextGroup _RewardGemsTextGroup;

	public static bool CanShow
	{
		get
		{
			return _canShow.Value;
		}
		set
		{
			_canShow.Value = value;
		}
	}

	public override void Show()
	{
		base.Show();
		if (BalanceController.competitionAward == null)
		{
			return;
		}
		if (BalanceController.competitionAward.Currency == "Coins")
		{
			_RewardCoinsTextGroup.Do(delegate(TextGroup t)
			{
				t.Text = BalanceController.competitionAward.Price.ToString();
			});
			_coinsIconObj.Do(delegate(GameObject o)
			{
				o.SetActive(true);
			});
			_gemsIconObj.Do(delegate(GameObject o)
			{
				o.SetActive(false);
			});
		}
		else
		{
			_RewardGemsTextGroup.Do(delegate(TextGroup t)
			{
				t.Text = BalanceController.competitionAward.Price.ToString();
			});
			_coinsIconObj.Do(delegate(GameObject o)
			{
				o.SetActive(false);
			});
			_gemsIconObj.Do(delegate(GameObject o)
			{
				o.SetActive(true);
			});
		}
	}

	public void HideButtonAction()
	{
		CanShow = false;
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.TournamentWunner);
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	private void OnDisable()
	{
		CanShow = false;
		if (BalanceController.competitionAward != null)
		{
			if (BalanceController.competitionAward.Currency == "Coins")
			{
				BankController.AddCoins(BalanceController.competitionAward.Price);
			}
			else
			{
				BankController.AddGems(BalanceController.competitionAward.Price);
			}
		}
		Singleton<EggsManager>.Instance.AddEgg("egg_tournament_winner");
		CoroutineRunner.Instance.StartCoroutine(BankController.WaitForIndicationGems((BalanceController.competitionAward != null) ? BalanceController.competitionAward.Currency : "Coins"));
	}
}
