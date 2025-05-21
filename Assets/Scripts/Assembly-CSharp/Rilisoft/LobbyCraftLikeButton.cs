using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyCraftLikeButton : MonoBehaviour
	{
		[SerializeField]
		protected internal ButtonHandler _buttonHandler;

		private void OnEnable()
		{
			UpdateButton();
			FriendsController.UpdatedLastLikeLobbyPlayers = (Action)Delegate.Combine(FriendsController.UpdatedLastLikeLobbyPlayers, new Action(UpdateButton));
		}

		private void OnDisable()
		{
			FriendsController.UpdatedLastLikeLobbyPlayers = (Action)Delegate.Remove(FriendsController.UpdatedLastLikeLobbyPlayers, new Action(UpdateButton));
		}

		private void UpdateButton()
		{
			List<string> lastLikesPlayers = FriendsController.lastLikesPlayers;
			if (lastLikesPlayers == null || !lastLikesPlayers.Any())
			{
				_buttonHandler.gameObject.SetActive(false);
				return;
			}
			bool active = lastLikesPlayers.Any((string id) => FriendsController.GetFullPlayerDataById(id) != null);
			_buttonHandler.gameObject.SetActive(active);
		}
	}
}
