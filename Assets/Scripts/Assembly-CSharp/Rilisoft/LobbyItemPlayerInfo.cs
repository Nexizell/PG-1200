using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class LobbyItemPlayerInfo 
	{
		[SerializeField]
		public string InfoId;

		[SerializeField]
		public long CraftStarted;

		[SerializeField]
		public bool IsCrafted;

		[SerializeField]
		public bool IsCraftedAndNotShown;

		[SerializeField]
		public bool IsNew;

		[SerializeField]
		public bool IsEquiped;

		[SerializeField]
		public long EquipTime;

		public void CopyValues(LobbyItemPlayerInfo source)
		{
			InfoId = source.InfoId;
			CraftStarted = source.CraftStarted;
			IsCrafted = source.IsCrafted;
			IsCraftedAndNotShown = source.IsCraftedAndNotShown;
			IsNew = source.IsNew;
			IsEquiped = source.IsEquiped;
			EquipTime = source.EquipTime;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			LobbyItemPlayerInfo lobbyItemPlayerInfo = obj as LobbyItemPlayerInfo;
			if (lobbyItemPlayerInfo == null)
			{
				return false;
			}
			if (InfoId == lobbyItemPlayerInfo.InfoId && CraftStarted == lobbyItemPlayerInfo.CraftStarted && IsCrafted == lobbyItemPlayerInfo.IsCrafted && IsCraftedAndNotShown == lobbyItemPlayerInfo.IsCraftedAndNotShown && IsNew == lobbyItemPlayerInfo.IsNew && IsEquiped == lobbyItemPlayerInfo.IsEquiped)
			{
				return EquipTime == lobbyItemPlayerInfo.EquipTime;
			}
			return false;
		}
	}
}
