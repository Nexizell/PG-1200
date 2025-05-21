using System;
using Rilisoft;
using UnityEngine;

public class FreeTicketsController : MonoBehaviour
{
	[Serializable]
	internal class RememberedState 
	{
		public int AccumulatedTickets;

		public float TimeRemains;

		public long TimeOfAppPause;

		public float SpeedModif = 1f;
	}

	private SaltedInt _accumulatedTicketsSalted;

	private float timeRemainsToNextTicketPack;

	private bool shouldProcessServerTimeEvent;

	private static FreeTicketsController instance;

	private const string REMEMBERED_STATE_KEY = "FreeTicketsController.REMEMBERED_STATE_KEY";

	public static FreeTicketsController Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject obj = new GameObject("FreeTicketsController");
				UnityEngine.Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<FreeTicketsController>();
			}
			return instance;
		}
	}

	public bool CanShowTimer
	{
		get
		{
			if (IsCountingFreeTickets())
			{
				return NumOfAccumulatedTicketsToGive() < BalanceController.MaxFreeTickets;
			}
			return false;
		}
	}

	public int EstimatedTimeUntilNextTicket
	{
		get
		{
			return (int)(timeRemainsToNextTicketPack / CurrentTimeSpeedModifier());
		}
	}

	public int EstimatedTimeUntilAllTicketsInSeconds
	{
		get
		{
			try
			{
				int num = BalanceController.MaxFreeTickets - accumulatedTickets;
				if (num <= 0)
				{
					return 0;
				}
				int num2 = num / BalanceController.FreeTicketsPerPack;
				if (num % BalanceController.FreeTicketsPerPack != 0)
				{
					num2++;
				}
				float num3 = (float)(num2 * BalanceController.TimeGiveFreeTickets) - ((float)BalanceController.TimeGiveFreeTickets - timeRemainsToNextTicketPack);
				num3 /= CurrentTimeSpeedModifier();
				return Mathf.Max(0, (int)num3);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in EstimatedTimeUntilAllTicketsInSeconds: {0}", ex);
				return 0;
			}
		}
	}

	private int accumulatedTickets
	{
		get
		{
			return _accumulatedTicketsSalted.Value;
		}
		set
		{
			_accumulatedTicketsSalted = new SaltedInt(SaltedInt._prng.Next(), value);
		}
	}

	public int NumOfAccumulatedTicketsToGive()
	{
		return accumulatedTickets;
	}

	public void GiveAccumulatedTickets()
	{
		try
		{
			BankController.AddTickets(accumulatedTickets);
			accumulatedTickets = 0;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveAccumulatedTickets: {0}", ex);
		}
	}

	public bool IsCountingFreeTickets()
	{
		if (FriendsController.ServerTime > 0)
		{
			return Storager.getInt("ChooseMiniGameController.FIRST_ENTER_TO_MINI_GAMES") > 0;
		}
		return false;
	}

	private void Start()
	{
		FriendsController.ServerTimeUpdated += FriendsController_ServerTimeUpdated;
		shouldProcessServerTimeEvent = true;
		timeRemainsToNextTicketPack = BalanceController.TimeGiveFreeTickets;
	}

	private void OnDestroy()
	{
		FriendsController.ServerTimeUpdated -= FriendsController_ServerTimeUpdated;
	}

	private void TruncateTickets()
	{
		if (accumulatedTickets >= BalanceController.MaxFreeTickets)
		{
			accumulatedTickets = BalanceController.MaxFreeTickets;
			timeRemainsToNextTicketPack = BalanceController.TimeGiveFreeTickets;
		}
	}

	public void ResetTimer()
	{
		timeRemainsToNextTicketPack = BalanceController.TimeGiveFreeTickets;
	}

	private void FriendsController_ServerTimeUpdated()
	{
		if (!shouldProcessServerTimeEvent)
		{
			return;
		}
		shouldProcessServerTimeEvent = false;
		ResetTimer();
		if (!IsCountingFreeTickets())
		{
			return;
		}
		if (!Storager.hasKey("FreeTicketsController.REMEMBERED_STATE_KEY"))
		{
			Storager.setString("FreeTicketsController.REMEMBERED_STATE_KEY", JsonUtility.ToJson(new RememberedState()));
			return;
		}
		try
		{
			RememberedState rememberedState = JsonUtility.FromJson<RememberedState>(Storager.getString("FreeTicketsController.REMEMBERED_STATE_KEY"));
			if (rememberedState.TimeOfAppPause == 0L)
			{
				return;
			}
			timeRemainsToNextTicketPack = rememberedState.TimeRemains;
			accumulatedTickets = rememberedState.AccumulatedTickets;
			float num = (float)(int)(FriendsController.ServerTime - rememberedState.TimeOfAppPause) * rememberedState.SpeedModif;
			if (num >= timeRemainsToNextTicketPack)
			{
				accumulatedTickets += BalanceController.FreeTicketsPerPack;
				num -= timeRemainsToNextTicketPack;
				int num2 = (int)(num / (float)BalanceController.TimeGiveFreeTickets);
				int num3 = num2 * BalanceController.FreeTicketsPerPack;
				accumulatedTickets += num3;
				num -= (float)(BalanceController.TimeGiveFreeTickets * num2);
				timeRemainsToNextTicketPack = (float)BalanceController.TimeGiveFreeTickets - num;
			}
			else
			{
				timeRemainsToNextTicketPack -= num;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in FriendsController_ServerTimeUpdated: {0}", ex);
		}
		TruncateTickets();
	}

	private static float CurrentTimeSpeedModifier()
	{
		float result = 1f;
		try
		{
			float effect = LobbyItemsController.GetEffect(LobbyItemInfo.LobbyItemBuffType.FreeTicketsSpeedUp);
			if (effect > 1f)
			{
				result = effect;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in CurrentTimeSpeedModifier: {0}", ex);
		}
		return result;
	}

	private void Update()
	{
		if (IsCountingFreeTickets())
		{
			if (accumulatedTickets < BalanceController.MaxFreeTickets)
			{
				timeRemainsToNextTicketPack -= Time.unscaledDeltaTime * CurrentTimeSpeedModifier();
			}
			if (timeRemainsToNextTicketPack <= 0f)
			{
				accumulatedTickets += BalanceController.FreeTicketsPerPack;
				TruncateTickets();
				timeRemainsToNextTicketPack = BalanceController.TimeGiveFreeTickets;
			}
			if (FreeTicketsTimer.IsShowTimer && !ChooseMiniGameController.IsFreeTicketsPeriodicalBannerShown && accumulatedTickets > 0)
			{
				GiveAccumulatedTickets();
			}
		}
	}

	public void SaveState()
	{
		if (IsCountingFreeTickets())
		{
			Storager.setString("FreeTicketsController.REMEMBERED_STATE_KEY", JsonUtility.ToJson(new RememberedState
			{
				AccumulatedTickets = accumulatedTickets,
				TimeRemains = timeRemainsToNextTicketPack,
				TimeOfAppPause = FriendsController.ServerTime,
				SpeedModif = CurrentTimeSpeedModifier()
			}));
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveState();
		}
		else
		{
			shouldProcessServerTimeEvent = true;
		}
	}
}
