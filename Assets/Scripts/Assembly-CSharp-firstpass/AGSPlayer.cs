using System;
using System.Collections;

public class AGSPlayer
{
	public readonly string alias;

	public readonly string playerId;

	public readonly string avatarUrl;

	private const string aliasKey = "alias";

	private const string playerIdKey = "playerId";

	private const string avatarUrlKey = "avatarUrl";

	public static AGSPlayer fromHashtable(Hashtable playerDataAsHashtable)
	{
		try
		{
			return new AGSPlayer(playerDataAsHashtable.ContainsKey("alias") ? playerDataAsHashtable["alias"].ToString() : "", playerDataAsHashtable.ContainsKey("playerId") ? playerDataAsHashtable["playerId"].ToString() : "", playerDataAsHashtable.ContainsKey("avatarUrl") ? playerDataAsHashtable["avatarUrl"].ToString() : "");
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError("Returning blank player due to exception getting player from hashtable: " + ex.ToString());
			return GetBlankPlayer();
		}
	}

	public static AGSPlayer GetBlankPlayer()
	{
		return new AGSPlayer("", "", "");
	}

	public static AGSPlayer BlankPlayerWithID(string playerId)
	{
		return new AGSPlayer("", playerId, "");
	}

	private AGSPlayer(string alias, string playerId, string avatarUrl)
	{
		this.alias = alias;
		this.playerId = playerId;
		this.avatarUrl = avatarUrl;
	}

	public override string ToString()
	{
		return string.Format("alias: {0}, playerId: {1}, avatarUrl: {2}", new object[3] { alias, playerId, avatarUrl });
	}
}
