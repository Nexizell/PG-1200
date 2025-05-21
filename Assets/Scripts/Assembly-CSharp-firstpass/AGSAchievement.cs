using System;
using System.Collections;

public class AGSAchievement
{
	public string title;

	public string id;

	public string description;

	public float progress;

	public int pointValue;

	public bool isHidden;

	public bool isUnlocked;

	public int position;

	public DateTime dateUnlocked;

	public static AGSAchievement fromHashtable(Hashtable hashtable)
	{
		try
		{
			return new AGSAchievement
			{
				title = hashtable["achievementTitle"].ToString(),
				id = hashtable["achievementId"].ToString(),
				description = hashtable["achievementDescription"].ToString(),
				progress = float.Parse(hashtable["achievementProgress"].ToString()),
				pointValue = int.Parse(hashtable["achievementPointValue"].ToString()),
				position = int.Parse(hashtable["achievementPosition"].ToString()),
				isUnlocked = bool.Parse(hashtable["achievementUnlocked"].ToString()),
				isHidden = bool.Parse(hashtable["achievementHidden"].ToString()),
				dateUnlocked = getTimefromEpochTime(long.Parse(hashtable["achievementDateUnlocked"].ToString()))
			};
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError("Returning blank achievement due to exception getting achievement from hashtable: " + ex.ToString());
			return GetBlankAchievement();
		}
	}

	public static AGSAchievement GetBlankAchievement()
	{
		return new AGSAchievement
		{
			title = "",
			id = "",
			description = "",
			pointValue = 0,
			isHidden = false,
			isUnlocked = false,
			progress = 0f,
			position = 0,
			dateUnlocked = DateTime.MinValue
		};
	}

	private static DateTime getTimefromEpochTime(long javaTimeStamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(javaTimeStamp).ToLocalTime();
	}

	public override string ToString()
	{
		return string.Format("title: {0}, id: {1}, pointValue: {2}, hidden: {3}, unlocked: {4}, progress: {5}, position: {6}, date: {7} ", title, id, pointValue, isHidden, isUnlocked, progress, position, dateUnlocked);
	}
}
