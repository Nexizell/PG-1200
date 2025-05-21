public class FriendInfo
{
	public string Name { get; protected internal set; }

	public bool IsOnline { get; protected internal set; }

	public string Room { get; protected internal set; }

	public bool IsInRoom
	{
		get
		{
			if (IsOnline)
			{
				return !string.IsNullOrEmpty(Room);
			}
			return false;
		}
	}

	public override string ToString()
	{
		return string.Format("{0}\t is: {1}", new object[2]
		{
			Name,
			(!IsOnline) ? "offline" : (IsInRoom ? "playing" : "on master")
		});
	}
}
