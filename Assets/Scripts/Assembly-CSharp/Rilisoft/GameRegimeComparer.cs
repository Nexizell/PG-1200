using System.Collections.Generic;

namespace Rilisoft
{
	public sealed class GameRegimeComparer : IEqualityComparer<GameConnect.GameMode>
	{
		private static readonly GameRegimeComparer s_instance = new GameRegimeComparer();

		public static GameRegimeComparer Instance
		{
			get
			{
				return s_instance;
			}
		}

		public bool Equals(GameConnect.GameMode x, GameConnect.GameMode y)
		{
			return x == y;
		}

		public int GetHashCode(GameConnect.GameMode obj)
		{
			return (int)obj;
		}
	}
}
