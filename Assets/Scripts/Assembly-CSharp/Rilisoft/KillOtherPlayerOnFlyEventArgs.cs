using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class KillOtherPlayerOnFlyEventArgs : EventArgs
	{
		public bool IamFly { get; set; }

		public bool KilledPlayerFly { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{ "iamFly", IamFly },
				{ "killedPlayerFly", KilledPlayerFly }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
