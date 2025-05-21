using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class WinEventArgs : EventArgs
	{
		public GameConnect.GameMode Mode { get; set; }

		public string Map { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{ "mode", Mode },
				{ "map", Map }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
