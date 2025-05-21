using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSPlayerClient : MonoBehaviour
{
	public static event Action<AGSRequestPlayerResponse> RequestLocalPlayerCompleted;

	public static event Action<AGSRequestFriendIdsResponse> RequestFriendIdsCompleted;

	public static event Action<AGSRequestBatchFriendsResponse> RequestBatchFriendsCompleted;

	public static event Action<bool> OnSignedInStateChangedEvent;

	[Obsolete("PlayerReceivedEvent is deprecated. Use RequestLocalPlayerCompleted instead.")]
	public static event Action<AGSPlayer> PlayerReceivedEvent;

	[Obsolete("PlayerFailedEvent is deprecated. Use RequestLocalPlayerCompleted instead.")]
	public static event Action<string> PlayerFailedEvent;

	static AGSPlayerClient()
	{
	}

	public static void RequestLocalPlayer(int userData = 0)
	{
		AGSRequestPlayerResponse platformNotSupportedResponse = AGSRequestPlayerResponse.GetPlatformNotSupportedResponse(userData);
		if (AGSPlayerClient.PlayerFailedEvent != null)
		{
			AGSPlayerClient.PlayerFailedEvent(platformNotSupportedResponse.error);
		}
		if (AGSPlayerClient.RequestLocalPlayerCompleted != null)
		{
			AGSPlayerClient.RequestLocalPlayerCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestFriendIds(int userData = 0)
	{
		if (AGSPlayerClient.RequestFriendIdsCompleted != null)
		{
			AGSPlayerClient.RequestFriendIdsCompleted(AGSRequestFriendIdsResponse.GetPlatformNotSupportedResponse(userData));
		}
	}

	public static void RequestBatchFriends(List<string> friendIds, int userData = 0)
	{
		if (AGSPlayerClient.RequestBatchFriendsCompleted != null)
		{
			AGSPlayerClient.RequestBatchFriendsCompleted(AGSRequestBatchFriendsResponse.GetPlatformNotSupportedResponse(friendIds, userData));
		}
	}

	public static void LocalPlayerFriendsComplete(string json)
	{
		if (AGSPlayerClient.RequestFriendIdsCompleted != null)
		{
			AGSPlayerClient.RequestFriendIdsCompleted(AGSRequestFriendIdsResponse.FromJSON(json));
		}
	}

	public static void BatchFriendsRequestComplete(string json)
	{
		if (AGSPlayerClient.RequestBatchFriendsCompleted != null)
		{
			AGSPlayerClient.RequestBatchFriendsCompleted(AGSRequestBatchFriendsResponse.FromJSON(json));
		}
	}

	public static void PlayerReceived(string json)
	{
		AGSRequestPlayerResponse aGSRequestPlayerResponse = AGSRequestPlayerResponse.FromJSON(json);
		if (!aGSRequestPlayerResponse.IsError() && AGSPlayerClient.PlayerReceivedEvent != null)
		{
			AGSPlayerClient.PlayerReceivedEvent(aGSRequestPlayerResponse.player);
		}
		if (AGSPlayerClient.RequestLocalPlayerCompleted != null)
		{
			AGSPlayerClient.RequestLocalPlayerCompleted(aGSRequestPlayerResponse);
		}
	}

	public static void PlayerFailed(string json)
	{
		AGSRequestPlayerResponse aGSRequestPlayerResponse = AGSRequestPlayerResponse.FromJSON(json);
		if (aGSRequestPlayerResponse.IsError() && AGSPlayerClient.PlayerFailedEvent != null)
		{
			AGSPlayerClient.PlayerFailedEvent(aGSRequestPlayerResponse.error);
		}
		if (AGSPlayerClient.RequestLocalPlayerCompleted != null)
		{
			AGSPlayerClient.RequestLocalPlayerCompleted(aGSRequestPlayerResponse);
		}
	}

	public static bool IsSignedIn()
	{
		return false;
	}

	public static void OnSignedInStateChanged(bool isSignedIn)
	{
		if (AGSPlayerClient.OnSignedInStateChangedEvent != null)
		{
			AGSPlayerClient.OnSignedInStateChangedEvent(isSignedIn);
		}
	}
}
