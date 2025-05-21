using System.Collections;

public class AGSLeaderboard
{
	public string name;

	public string id;

	public string displayText;

	public string scoreFormat;

	public string imageUrl;

	public static AGSLeaderboard fromHashtable(Hashtable hashtable)
	{
		return new AGSLeaderboard
		{
			name = hashtable["leaderboardName"].ToString(),
			id = hashtable["leaderboardId"].ToString(),
			displayText = hashtable["leaderboardDisplayText"].ToString(),
			scoreFormat = hashtable["leaderboardScoreFormat"].ToString(),
			imageUrl = hashtable["leaderboardImageUrl"].ToString()
		};
	}

	public static AGSLeaderboard GetBlankLeaderboard()
	{
		return new AGSLeaderboard
		{
			name = "",
			id = "",
			displayText = "",
			scoreFormat = "",
			imageUrl = ""
		};
	}

	public override string ToString()
	{
		return string.Format("name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}, imageUrl: {4}", name, id, displayText, scoreFormat, imageUrl);
	}
}
