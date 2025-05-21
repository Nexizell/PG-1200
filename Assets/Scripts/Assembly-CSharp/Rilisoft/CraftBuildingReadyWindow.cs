using System;
using UnityEngine;

namespace Rilisoft
{
	public class CraftBuildingReadyWindow : MonoBehaviour
	{
		[SerializeField]
		protected internal TextGroup _itemName;

		[SerializeField]
		protected internal TextGroup _expCount;

		private LobbyItem _lobbyItem;

		private IDisposable _backSubscription;

		public event Action<LobbyItem> OnWindowClose;

		public void Show(LobbyItem lobbyItem)
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(U_ClickOnGetReward, "lobby craft");
			_lobbyItem = lobbyItem;
			if (_lobbyItem != null)
			{
				_itemName.Text = LocalizationStore.Get(_lobbyItem.Info.Lkey);
				_expCount.Text = string.Format(LocalizationStore.Get("Key_3003"), new object[1] { _lobbyItem.Info.ExpForCrafting });
			}
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
				if (_backSubscription != null)
				{
					_backSubscription.Dispose();
					_backSubscription = null;
				}
				if (this.OnWindowClose != null)
				{
					this.OnWindowClose(_lobbyItem);
				}
				_lobbyItem = null;
			}
		}

		public void U_ClickOnGetReward()
		{
			Hide();
		}
	}
}
