using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class CoinBonus : MonoBehaviour
{
	public GameObject player;

	public AudioClip CoinItemUpAudioClip;

	private Player_move_c test;

	public VirtualCurrencyBonusType BonusType { private get; set; }

	public static event Action StartBlinkShop;

	public static List<string> GetLevelsWhereGotBonus(VirtualCurrencyBonusType bonusType)
	{
		switch (bonusType)
		{
		case VirtualCurrencyBonusType.Coin:
			return Storager.getString(Defs.LevelsWhereGetCoinS).Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		case VirtualCurrencyBonusType.Gem:
		{
			List<object> list = Json.Deserialize(Storager.getString(Defs.LevelsWhereGotGems)) as List<object>;
			if (list == null)
			{
				return new List<string>();
			}
			return list.OfType<string>().ToList();
		}
		default:
			return new List<string>();
		}
	}

	internal static string[] GetLevelsWhereGotCoins()
	{
		return Storager.getString(Defs.LevelsWhereGetCoinS).Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);
	}

	internal static IEnumerable<string> GetLevelsWhereGotGems()
	{
		string @string = Storager.getString(Defs.LevelsWhereGotGems);
		if (!(Json.Deserialize(@string) is List<object>))
		{
			return new string[0];
		}
		return @string.OfType<string>();
	}

	public static bool SetLevelsWhereGotBonus(string[] levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string levelsWhereGotBonusSerialized = string.Empty;
		switch (bonusType)
		{
		case VirtualCurrencyBonusType.Coin:
			levelsWhereGotBonusSerialized = string.Join("#", levelsWhereGotBonus);
			break;
		case VirtualCurrencyBonusType.Gem:
			levelsWhereGotBonusSerialized = Json.Serialize(levelsWhereGotBonus);
			break;
		}
		return SetLevelsWhereGotBonus(levelsWhereGotBonusSerialized, bonusType);
	}

	public static bool SetLevelsWhereGotBonus(List<string> levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string levelsWhereGotBonusSerialized = string.Empty;
		switch (bonusType)
		{
		case VirtualCurrencyBonusType.Coin:
			levelsWhereGotBonusSerialized = string.Join("#", levelsWhereGotBonus.ToArray());
			break;
		case VirtualCurrencyBonusType.Gem:
			levelsWhereGotBonusSerialized = Json.Serialize(levelsWhereGotBonus);
			break;
		}
		return SetLevelsWhereGotBonus(levelsWhereGotBonusSerialized, bonusType);
	}

	internal static bool SetLevelsWhereGotBonus(string levelsWhereGotBonusSerialized, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonusSerialized == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonusAsString");
		}
		switch (bonusType)
		{
		case VirtualCurrencyBonusType.Coin:
			Storager.setString(Defs.LevelsWhereGetCoinS, levelsWhereGotBonusSerialized);
			return true;
		case VirtualCurrencyBonusType.Gem:
			Storager.setString(Defs.LevelsWhereGotGems, levelsWhereGotBonusSerialized);
			return true;
		default:
			return false;
		}
	}

	public void SetPlayer()
	{
		test = WeaponManager.sharedManager.myPlayerMoveC;
		player = WeaponManager.sharedManager.myPlayer;
	}

	private void Update()
	{
		if (test == null || player == null || BonusType == VirtualCurrencyBonusType.None || Vector3.SqrMagnitude(base.transform.position - player.transform.position) > ((BonusType == VirtualCurrencyBonusType.CoinSpeedrun) ? 64f : 2.25f))
		{
			return;
		}
		try
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				int num = ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
				if (!GameConnect.isSurvival && !Defs.isMulti)
				{
					num = 1;
				}
				switch (BonusType)
				{
				case VirtualCurrencyBonusType.Coin:
				case VirtualCurrencyBonusType.CoinSpeedrun:
				{
					int int2 = Storager.getInt("Coins");
					Storager.setInt("Coins", int2 + num);
					AnalyticsFacade.CurrencyAccrual(num, "Coins");
					break;
				}
				case VirtualCurrencyBonusType.Gem:
				{
					int @int = Storager.getInt("GemsCurrency");
					Storager.setInt("GemsCurrency", @int + num);
					AnalyticsFacade.CurrencyAccrual(num, "GemsCurrency");
					break;
				}
				}
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
			}
			CoinsMessage.FireCoinsAddedEvent(BonusType == VirtualCurrencyBonusType.Gem, 1);
			if (!test.isSurvival && TrainingController.TrainingCompleted)
			{
				List<string> levelsWhereGotBonus = GetLevelsWhereGotBonus(BonusType);
				string item = SceneManager.GetActiveScene().name;
				if (!levelsWhereGotBonus.Contains(item))
				{
					levelsWhereGotBonus.Add(item);
					SetLevelsWhereGotBonus(levelsWhereGotBonus, BonusType);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingController.isNextStep = TrainingState.GetTheCoin;
				if (CoinBonus.StartBlinkShop != null)
				{
					CoinBonus.StartBlinkShop();
				}
			}
		}
		finally
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
