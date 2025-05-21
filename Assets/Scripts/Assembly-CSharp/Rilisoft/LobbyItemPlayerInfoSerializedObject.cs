using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class LobbyItemPlayerInfoSerializedObject 
	{
		[SerializeField]
		public List<LobbyItemPlayerInfo> Infos = new List<LobbyItemPlayerInfo>();
	}
}
