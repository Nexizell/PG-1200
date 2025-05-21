using System;
using System.Collections;

public class AGSSubmitScoreResponse : AGSRequestResponse
{
	public string leaderboardId;

	public static AGSSubmitScoreResponse FromJSON(string json)
	{
		try
		{
			AGSSubmitScoreResponse aGSSubmitScoreResponse = new AGSSubmitScoreResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSSubmitScoreResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSSubmitScoreResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSSubmitScoreResponse.leaderboardId = (hashtable.ContainsKey("leaderboardId") ? hashtable["leaderboardId"].ToString() : "");
			return aGSSubmitScoreResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSSubmitScoreResponse GetBlankResponseWithError(string error, string leaderboardId = "", int userData = 0)
	{
		return new AGSSubmitScoreResponse
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId
		};
	}

	public static AGSSubmitScoreResponse GetPlatformNotSupportedResponse(string leaderboardId, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, userData);
	}
}
