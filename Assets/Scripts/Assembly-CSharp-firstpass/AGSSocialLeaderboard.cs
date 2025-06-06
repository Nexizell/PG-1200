using System;
using UnityEngine.SocialPlatforms;

public class AGSSocialLeaderboard
{
	private readonly AGSLeaderboard leaderboard;

	public long localPlayerScore;

	public int localPlayerRank;

	private TimeScope _timeScope;

	public bool loading
	{
		get
		{
			return !ScoresAvailable();
		}
	}

	public string id { get; set; }

	public UserScope userScope { get; set; }

	public UnityEngine.SocialPlatforms.Range range { get; set; }

	public TimeScope timeScope
	{
		get
		{
			return _timeScope;
		}
		set
		{
			localPlayerScore = -1L;
			localPlayerRank = -1;
			scores = new AGSSocialLeaderboardScore[0];
			_timeScope = value;
			LoadScores(null);
			GameCircleSocial.Instance.RequestLocalUserScore(this);
		}
	}

	public IScore localUserScore
	{
		get
		{
			return new AGSSocialLeaderboardScore(new AGSScore
			{
				player = AGSSocialLocalUser.player,
				scoreValue = localPlayerScore,
				scoreString = localPlayerScore.ToString(),
				rank = localPlayerRank
			}, leaderboard);
		}
	}

	public uint maxRange
	{
		get
		{
			AGSClient.LogGameCircleError("ILeaderboard.maxRange.get is not available for GameCircle");
			return 0u;
		}
	}

	public IScore[] scores { get; set; }

	public string title
	{
		get
		{
			if (leaderboard == null)
			{
				return null;
			}
			return leaderboard.name;
		}
	}

	public AGSSocialLeaderboard(AGSLeaderboard leaderboard)
	{
		if (leaderboard == null)
		{
			AGSClient.LogGameCircleError("AGSSocialLeaderboard constructor \"leaderboard\" argument should not be null");
			this.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
		}
		else
		{
			this.leaderboard = leaderboard;
		}
		id = leaderboard.id;
		scores = new AGSSocialLeaderboardScore[0];
		localPlayerScore = -1L;
		localPlayerRank = -1;
		_timeScope = TimeScope.AllTime;
	}

	public AGSSocialLeaderboard()
	{
		leaderboard = AGSLeaderboard.GetBlankLeaderboard();
		localPlayerScore = -1L;
		localPlayerRank = -1;
		_timeScope = TimeScope.AllTime;
	}

	public bool ScoresAvailable()
	{
		if (leaderboard != null && scores != null && scores.Length != 0 && localPlayerScore > -1)
		{
			return localPlayerRank > -1;
		}
		return false;
	}

	public void SetUserFilter(string[] userIDs)
	{
		AGSClient.LogGameCircleError("ILeaderboard.SetUserFilter is not available for GameCircle");
	}

	public void LoadScores(Action<bool> callback)
	{
		if (leaderboard == null)
		{
			callback(false);
		}
		else
		{
			GameCircleSocial.Instance.RequestScores(this, callback);
		}
	}
}
