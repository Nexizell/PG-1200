using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class CraftCategoryView : MonoBehaviour
	{
		[SerializeField]
		protected internal LobbyItemGroupType _groupType;

		[SerializeField]
		protected internal GameObject _newIndicator;

		[SerializeField]
		protected internal UISprite _icon;

		private Color _baseIconColor;

		private void OnClick()
		{
			LobbyCraftController.Instance.GoToGroup(_groupType);
		}

		private void Awake()
		{
			_baseIconColor = _icon.color;
		}

		private void OnEnable()
		{
			List<LobbyItem> source = Singleton<LobbyItemsController>.Instance.ItemsPerGroup(_groupType, true);
			if (_newIndicator != null)
			{
				bool active = source.Any((LobbyItem i) => i.CanShowIsNew);
				_newIndicator.SetActive(active);
			}
			if (_icon != null)
			{
				if (!LobbyItemsController.TutorialCompleted && !LobbyItemsController.TutorialGroups.Contains(_groupType))
				{
					_icon.color = Color.gray;
				}
				else
				{
					_icon.color = _baseIconColor;
				}
			}
		}
	}
}
