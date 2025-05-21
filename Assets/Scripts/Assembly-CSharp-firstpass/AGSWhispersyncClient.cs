using System;
using UnityEngine;

public class AGSWhispersyncClient : MonoBehaviour
{
	public static string failReason;

	public static event Action OnNewCloudDataEvent;

	public static event Action OnDataUploadedToCloudEvent;

	public static event Action OnThrottledEvent;

	public static event Action OnDiskWriteCompleteEvent;

	public static event Action OnFirstSynchronizeEvent;

	public static event Action OnAlreadySynchronizedEvent;

	public static event Action OnSyncFailedEvent;

	static AGSWhispersyncClient()
	{
	}

	public static AGSGameDataMap GetGameData()
	{
		return null;
	}

	public static void Synchronize()
	{
	}

	public static void Flush()
	{
	}

	public static void OnNewCloudData()
	{
		if (AGSWhispersyncClient.OnNewCloudDataEvent != null)
		{
			AGSWhispersyncClient.OnNewCloudDataEvent();
		}
	}

	public static void OnDataUploadedToCloud()
	{
		if (AGSWhispersyncClient.OnDataUploadedToCloudEvent != null)
		{
			AGSWhispersyncClient.OnDataUploadedToCloudEvent();
		}
	}

	public static void OnThrottled()
	{
		if (AGSWhispersyncClient.OnThrottledEvent != null)
		{
			AGSWhispersyncClient.OnThrottledEvent();
		}
	}

	public static void OnDiskWriteComplete()
	{
		if (AGSWhispersyncClient.OnDiskWriteCompleteEvent != null)
		{
			AGSWhispersyncClient.OnDiskWriteCompleteEvent();
		}
	}

	public static void OnFirstSynchronize()
	{
		if (AGSWhispersyncClient.OnFirstSynchronizeEvent != null)
		{
			AGSWhispersyncClient.OnFirstSynchronizeEvent();
		}
	}

	public static void OnAlreadySynchronized()
	{
		if (AGSWhispersyncClient.OnAlreadySynchronizedEvent != null)
		{
			AGSWhispersyncClient.OnAlreadySynchronizedEvent();
		}
	}

	public static void OnSyncFailed(string failReason)
	{
		AGSWhispersyncClient.failReason = failReason;
		if (AGSWhispersyncClient.OnSyncFailedEvent != null)
		{
			AGSWhispersyncClient.OnSyncFailedEvent();
		}
	}
}
