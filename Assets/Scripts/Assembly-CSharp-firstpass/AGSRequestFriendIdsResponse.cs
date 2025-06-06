using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestFriendIdsResponse : AGSRequestResponse
{
	public List<string> friendIds;

	public static AGSRequestFriendIdsResponse FromJSON(string json)
	{
		try
		{
			AGSRequestFriendIdsResponse aGSRequestFriendIdsResponse = new AGSRequestFriendIdsResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestFriendIdsResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestFriendIdsResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestFriendIdsResponse.friendIds = new List<string>();
			if (hashtable.ContainsKey("friendIds"))
			{
				foreach (string item in hashtable["friendIds"] as ArrayList)
				{
					aGSRequestFriendIdsResponse.friendIds.Add(item);
				}
			}
			return aGSRequestFriendIdsResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestFriendIdsResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		return new AGSRequestFriendIdsResponse
		{
			error = error,
			userData = userData,
			friendIds = new List<string>()
		};
	}

	public static AGSRequestFriendIdsResponse GetPlatformNotSupportedResponse(int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}
