using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestAchievementsResponse : AGSRequestResponse
{
	public List<AGSAchievement> achievements;

	public static AGSRequestAchievementsResponse FromJSON(string json)
	{
		try
		{
			AGSRequestAchievementsResponse aGSRequestAchievementsResponse = new AGSRequestAchievementsResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestAchievementsResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestAchievementsResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestAchievementsResponse.achievements = new List<AGSAchievement>();
			if (hashtable.ContainsKey("achievements"))
			{
				foreach (Hashtable item in hashtable["achievements"] as ArrayList)
				{
					aGSRequestAchievementsResponse.achievements.Add(AGSAchievement.fromHashtable(item));
				}
			}
			return aGSRequestAchievementsResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestAchievementsResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		return new AGSRequestAchievementsResponse
		{
			error = error,
			userData = userData,
			achievements = new List<AGSAchievement>()
		};
	}

	public static AGSRequestAchievementsResponse GetPlatformNotSupportedResponse(int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}
