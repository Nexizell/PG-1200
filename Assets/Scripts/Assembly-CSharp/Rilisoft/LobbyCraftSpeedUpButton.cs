using UnityEngine;

namespace Rilisoft
{
	public class LobbyCraftSpeedUpButton : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _container;

		[SerializeField]
		protected internal TextGroup _nameTextGroup;

		[SerializeField]
		protected internal TextGroup _timeLeftTextGroup;

		[SerializeField]
		protected internal PriceView _button;

		[SerializeField]
		protected internal GameObject _objInternetDisabled;

		private void OnEnable()
		{
			_container.SetActive(false);
			if (Singleton<LobbyItemsController>.Instance.CraftingItem != null)
			{
				OnCraftStarted(Singleton<LobbyItemsController>.Instance.CraftingItem);
			}
			_button.OnClicked += OnButtonClicked;
			LobbyItemsController.OnCraftStarted += OnCraftStarted;
			LobbyItemsController.OnCraftFinished += OnCraftFinished;
			LocalizationStore.AddEventCallAfterLocalize(OnLocalizationChanged);
		}

		private void OnDisable()
		{
			_button.OnClicked -= OnButtonClicked;
			LobbyItemsController.OnCraftStarted -= OnCraftStarted;
			LobbyItemsController.OnCraftFinished -= OnCraftFinished;
			LocalizationStore.DelEventCallAfterLocalize(OnLocalizationChanged);
		}

		private void OnButtonClicked()
		{
			if (LobbyCraftController.Instance != null)
			{
				LobbyCraftController.Instance.SpeedUpCraftingItem();
			}
		}

		private void OnCraftStarted(LobbyItem obj)
		{
			_container.SetActive(true);
			_nameTextGroup.Text = LocalizationStore.Get(obj.Info.Lkey);
		}

		private void OnCraftFinished(LobbyItem obj)
		{
			_container.SetActive(false);
		}

		private void OnLocalizationChanged()
		{
			if (Singleton<LobbyItemsController>.Instance.CraftingItem != null)
			{
				_nameTextGroup.Text = LocalizationStore.Get(Singleton<LobbyItemsController>.Instance.CraftingItem.Info.Lkey);
			}
		}

		private void Update()
		{
			if (_container.activeInHierarchy && Singleton<LobbyItemsController>.Instance.CraftingItem != null)
			{
				_objInternetDisabled.SetActiveSafeSelf(!LobbyItemsController.CurrentTime.HasValue);
				if (LobbyItemsController.CurrentTime.HasValue)
				{
					_timeLeftTextGroup.Text = RiliExtensions.GetTimeString(Singleton<LobbyItemsController>.Instance.CraftingItem.CraftTimeLeft, ":", true);
					_button.SetPrice(Singleton<LobbyItemsController>.Instance.CraftingItem.PriceSpeedUpLeft);
				}
				else
				{
					_timeLeftTextGroup.Text = RiliExtensions.GetTimeString(Singleton<LobbyItemsController>.Instance.CraftingItem.Info.CraftTime, ":", true);
					_button.SetPrice(new ItemPrice(0, Singleton<LobbyItemsController>.Instance.CraftingItem.Info.PriceSpeedUp.Currency));
				}
			}
		}
	}
}
