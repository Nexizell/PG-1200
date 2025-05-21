using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestAchievementsForPlayerResponse : AGSRequestAchievementsResponse
{
	public string playerId;

	public new static AGSRequestAchievementsForPlayerResponse FromJSON(string json)
	{
		try
		{
			AGSRequestAchievementsForPlayerResponse aGSRequestAchievementsForPlayerResponse = new AGSRequestAchievementsForPlayerResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestAchievementsForPlayerResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestAchievementsForPlayerResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestAchievementsForPlayerResponse.achievements = new List<AGSAchievement>();
			if (hashtable.ContainsKey("achievements"))
			{
				foreach (Hashtable item in hashtable["achievements"] as ArrayList)
				{
					aGSRequestAchievementsForPlayerResponse.achievements.Add(AGSAchievement.fromHashtable(item));
				}
			}
			aGSRequestAchievementsForPlayerResponse.playerId = (hashtable.ContainsKey("playerId") ? hashtable["playerId"].ToString() : "");
			return aGSRequestAchievementsForPlayerResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestAchievementsForPlayerResponse GetBlankResponseWithError(string error, string playerId = "", int userData = 0)
	{
		return new AGSRequestAchievementsForPlayerResponse
		{
			error = error,
			playerId = playerId,
			userData = userData,
			achievements = new List<AGSAchievement>()
		};
	}

	public static AGSRequestAchievementsForPlayerResponse GetPlatformNotSupportedResponse(string playerId, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", playerId, userData);
	}
}
