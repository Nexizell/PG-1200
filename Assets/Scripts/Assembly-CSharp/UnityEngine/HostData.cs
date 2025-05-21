using System;

namespace UnityEngine
{
	public sealed class HostData
	{
		public string comment { get; set; }

		public int connectedPlayers { get; set; }

		public string gameName { get; set; }

		public string gameType { get; set; }

		public string guid { get; set; }

		public string[] ip { get; set; }

		public bool passwordProtected { get; set; }

		public int playerLimit { get; set; }

		public int port { get; set; }

		public bool useNat { get; set; }

		public HostData()
		{
			Debug.LogError("HostData is not supported on Windows Phone 8.");
			throw new NotSupportedException("HostData is not supported on Windows Phone 8.");
		}
	}
}
