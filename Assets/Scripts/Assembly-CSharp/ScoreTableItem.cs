using UnityEngine;

public class ScoreTableItem
{
	public string name = "";

	public int killCount;

	public int rank = 1;

	public int score;

	public string pixelbookID = "";

	public Texture clanLogo;

	public NetworkStartTable table;

	public bool isDead
	{
		get
		{
			if (!(table == null))
			{
				return table.isDeadInMiniGame;
			}
			return true;
		}
	}

	public bool isMine
	{
		get
		{
			return table.Equals(WeaponManager.sharedManager.myNetworkStartTable);
		}
	}

	public ScoreTableItem(NetworkStartTable table)
	{
		UpdateFromTable(table);
	}

	public void UpdateFromTable(NetworkStartTable table)
	{
		if (!(table == null))
		{
			this.table = table;
			name = table.NamePlayer;
			killCount = ((table.CountKills != -1) ? table.CountKills : table.oldCountKills);
			score = ((table.score != -1) ? table.score : table.scoreOld);
			rank = table.myRanks;
			pixelbookID = table.pixelBookID;
			clanLogo = table.myClanTexture;
		}
	}
}
