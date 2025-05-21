using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class KillMonsterEventArgs : EventArgs
	{
		public ShopNGUIController.CategoryNames WeaponSlot { get; set; }

		public bool Campaign { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{ "weaponSlot", WeaponSlot },
				{ "campaign", Campaign }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
