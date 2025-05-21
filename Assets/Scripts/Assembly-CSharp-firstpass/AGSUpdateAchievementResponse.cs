using System;
using System.Collections;

public class AGSUpdateAchievementResponse : AGSRequestResponse
{
	public string achievementId;

	public static AGSUpdateAchievementResponse FromJSON(string json)
	{
		try
		{
			AGSUpdateAchievementResponse aGSUpdateAchievementResponse = new AGSUpdateAchievementResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSUpdateAchievementResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSUpdateAchievementResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSUpdateAchievementResponse.achievementId = (hashtable.ContainsKey("achievementId") ? hashtable["achievementId"].ToString() : "");
			return aGSUpdateAchievementResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSUpdateAchievementResponse GetBlankResponseWithError(string error, string achievementId = "", int userData = 0)
	{
		return new AGSUpdateAchievementResponse
		{
			error = error,
			userData = userData,
			achievementId = achievementId
		};
	}

	public static AGSUpdateAchievementResponse GetPlatformNotSupportedResponse(string achievementId, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", achievementId, userData);
	}
}
