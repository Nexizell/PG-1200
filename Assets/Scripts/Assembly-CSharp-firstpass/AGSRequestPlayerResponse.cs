using System;
using System.Collections;

public class AGSRequestPlayerResponse : AGSRequestResponse
{
	public AGSPlayer player;

	public static AGSRequestPlayerResponse FromJSON(string json)
	{
		try
		{
			AGSRequestPlayerResponse aGSRequestPlayerResponse = new AGSRequestPlayerResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestPlayerResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestPlayerResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestPlayerResponse.player = (hashtable.ContainsKey("player") ? AGSPlayer.fromHashtable(hashtable["player"] as Hashtable) : AGSPlayer.GetBlankPlayer());
			return aGSRequestPlayerResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestPlayerResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		return new AGSRequestPlayerResponse
		{
			error = error,
			userData = userData,
			player = AGSPlayer.GetBlankPlayer()
		};
	}

	public static AGSRequestPlayerResponse GetPlatformNotSupportedResponse(int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}
