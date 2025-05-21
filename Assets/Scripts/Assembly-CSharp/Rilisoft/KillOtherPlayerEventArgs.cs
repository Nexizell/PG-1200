using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class KillOtherPlayerEventArgs : EventArgs
	{
		public GameConnect.GameMode Mode { get; set; }

		public ShopNGUIController.CategoryNames WeaponSlot { get; set; }

		public bool Headshot { get; set; }

		public bool Grenade { get; set; }

		public bool Revenge { get; set; }

		public bool VictimIsFlagCarrier { get; set; }

		public bool IsInvisible { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{ "mode", Mode },
				{ "weaponSlot", WeaponSlot },
				{ "headshot", Headshot },
				{ "grenade", Grenade },
				{ "revenge", Revenge },
				{ "victimIsFlagCarrier", VictimIsFlagCarrier },
				{ "isInvisible", IsInvisible }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
