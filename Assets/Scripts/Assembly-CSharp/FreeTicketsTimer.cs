using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public class FreeTicketsTimer : MonoBehaviour
{
	public List<UILabel> periodicalFreeTicketsTimerLabels;

	public GameObject periodicalFreeTicketsTimer;

	private static List<FreeTicketsTimer> allTimers = new List<FreeTicketsTimer>();

	public static bool IsShowTimer
	{
		get
		{
			if (allTimers != null && allTimers.Count > 0)
			{
				return allTimers.Any((FreeTicketsTimer timer) => timer != null && timer.gameObject != null && timer.gameObject.activeInHierarchy);
			}
			return false;
		}
	}

	private void Start()
	{
		allTimers.Add(this);
		UpdateTimer();
	}

	private void Update()
	{
		UpdateTimer();
	}

	private void OnDestroy()
	{
		allTimers.Remove(this);
	}

	private void UpdateTimer()
	{
		try
		{
			periodicalFreeTicketsTimer.SetActiveSafeSelf(FreeTicketsController.Instance.CanShowTimer);
			if (periodicalFreeTicketsTimer.activeSelf)
			{
				string timeStr = RiliExtensions.GetTimeStringDays(FreeTicketsController.Instance.EstimatedTimeUntilNextTicket);
				RiliExtensions.ForEach(periodicalFreeTicketsTimerLabels, delegate(UILabel label)
				{
					label.text = timeStr;
				});
				RiliExtensions.ForEach(periodicalFreeTicketsTimerLabels, delegate(UILabel label)
				{
					label.color = (Mathf.Approximately(LobbyItemsController.GetEffect(LobbyItemInfo.LobbyItemBuffType.FreeTicketsSpeedUp), 0f) ? Color.white : Color.yellow);
				});
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in updating FreeTicketsTimer: {0}", ex);
		}
	}
}
