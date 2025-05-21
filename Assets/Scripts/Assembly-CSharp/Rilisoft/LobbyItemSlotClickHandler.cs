using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public class LobbyItemSlotClickHandler : MonoBehaviour
	{
		private void OnEnable()
		{
			UpdateColliders();
		}

		[ContextMenu("Update colliders")]
		private void UpdateColliders()
		{
			foreach (Collider item in from c in GetComponentsInChildren<Collider>()
				where !c.isTrigger
				select c)
			{
				ButtonEventsHandler orAddComponent = item.gameObject.GetOrAddComponent<ButtonEventsHandler>();
				orAddComponent.OnClickEvent.RemoveListener(OnClick);
				orAddComponent.OnClickEvent.AddListener(OnClick);
			}
		}

		private void OnClick()
		{
			ILobbyItemView componentInChildren = base.gameObject.GetComponentInChildren<ILobbyItemView>();
			if (componentInChildren != null && LobbyCraftController.Instance != null)
			{
				LobbyCraftController.Instance.SelectItemOnSceneRequest(componentInChildren.LobbyItem);
			}
		}
	}
}
