using UnityEngine;

namespace Rilisoft
{
	public interface ILobbyItemView
	{
		LobbyItem LobbyItem { get; }

		bool IsSelected { get; set; }

		void Setup(Transform root, LobbyCraftController controller, LobbyItem item);

		void Hide();

		void UpdateView();

		void Kill();
	}
}
