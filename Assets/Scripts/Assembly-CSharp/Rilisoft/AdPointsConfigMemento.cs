using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	internal sealed class AdPointsConfigMemento 
	{
		public ChestInLobbyPointMemento ChestInLobby { get; private set; }

		public FreeSpinPointMemento FreeSpin { get; private set; }

		public FreeCheckpointsPointMemento FreeCheckpoints { get; private set; }

		public TicketsPointMemento FreeTickets { get; private set; }

		public ReturnInConnectSceneAdPointMemento ReturnInConnectScene { get; private set; }

		public CampaignAdPointMemento Campaign { get; private set; }

		public SurvivalArenaAdPointMemento SurvivalArena { get; private set; }

		public AfterMatchAdPointMemento AfterMatch { get; private set; }

		public PolygonAdPointMemento Polygon { get; private set; }

		internal static AdPointsConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			AdPointsConfigMemento adPointsConfigMemento = new AdPointsConfigMemento();
			object value;
			dictionary.TryGetValue("chestInLobby", out value);
			adPointsConfigMemento.ChestInLobby = ChestInLobbyPointMemento.FromObject("chestInLobby", value);
			object value2;
			dictionary.TryGetValue("checkpoints", out value2);
			adPointsConfigMemento.FreeCheckpoints = FreeCheckpointsPointMemento.FromObject("checkpoints", value2);
			object value3;
			dictionary.TryGetValue("freeSpin", out value3);
			adPointsConfigMemento.FreeSpin = FreeSpinPointMemento.FromObject("freeSpin", value3);
			object value4;
			dictionary.TryGetValue("tickets", out value4);
			adPointsConfigMemento.FreeTickets = TicketsPointMemento.FromObject("tickets", value4);
			object value5;
			dictionary.TryGetValue("returnInConnectScene", out value5);
			adPointsConfigMemento.ReturnInConnectScene = ReturnInConnectSceneAdPointMemento.FromObject("returnInConnectScene", value5);
			object value6;
			dictionary.TryGetValue("campaign", out value6);
			adPointsConfigMemento.Campaign = CampaignAdPointMemento.FromObject("campaign", value6);
			object value7;
			dictionary.TryGetValue("survivalArena", out value7);
			adPointsConfigMemento.SurvivalArena = SurvivalArenaAdPointMemento.FromObject("survivalArena", value7);
			object value8;
			dictionary.TryGetValue("afterMatch", out value8);
			adPointsConfigMemento.AfterMatch = AfterMatchAdPointMemento.FromObject("afterMatch", value8);
			object value9;
			dictionary.TryGetValue("polygon", out value9);
			adPointsConfigMemento.Polygon = PolygonAdPointMemento.FromObject("polygon", value9);
			return adPointsConfigMemento;
		}
	}
}
