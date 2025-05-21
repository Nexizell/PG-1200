using System;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	public class BankShopViewGuiElement : GuiElementBase
	{
		public enum BankShopViewType
		{
			Shop = 0,
			Bank = 1
		}

		[SerializeField]
		[Header("settings")]
		protected internal BankShopViewType _viewType;

		[SerializeField]
		public bool HideButtons;

		public bool ShowTickets;

		public bool ShowCoins = true;

		[SerializeField]
		public bool InteractionEnabled = true;

		[SerializeField]
		public bool X3Enabled;

		[SerializeField]
		protected internal PrefabHandler _prefabHandler = new PrefabHandler();

		public BankShopViewType ViewType
		{
			get
			{
				return _viewType;
			}
			set
			{
				_viewType = value;
			}
		}

		public BankShopView View { get; private set; }

		public override bool IsVisible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		private bool MovedTickets { get; set; }

		public event EventHandler Clicked;

		protected override void OnEnable()
		{
			UpdateView();
			if (View != null)
			{
				PushRequest();
			}
		}

		[ContextMenu("UpdateView")]
		public void UpdateView()
		{
			if (View == null)
			{
				View = GetComponentInChildren<BankShopView>();
			}
			if (_prefabHandler.Prefab == null && View == null)
			{
				return;
			}
			if (View == null)
			{
				if (!_prefabHandler.Prefab.ContainsComponent<BankShopView>())
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(_prefabHandler.Prefab);
				gameObject.SetLayerRecursively(base.gameObject.layer);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				View = gameObject.GetComponent<BankShopView>();
			}
			if (View == null)
			{
				return;
			}
			View.ShopButton.GetComponent<ButtonHandler>().Do(delegate(ButtonHandler h)
			{
				h.Clicked -= OnViewClicked;
				h.Clicked += OnViewClicked;
			});
			View.BankButton.GetComponent<ButtonHandler>().Do(delegate(ButtonHandler h)
			{
				h.Clicked -= OnViewClicked;
				h.Clicked += OnViewClicked;
			});
			if (InteractionEnabled)
			{
				if (View.ShopButtonScript != null && !View.ShopButtonScript.enabled)
				{
					View.ShopButtonScript.enabled = true;
					View.ShopButtonCollider.enabled = true;
					View.ShopButtonScript.state = UIButtonColor.State.Normal;
				}
				if (View.BankButtonScript != null && !View.BankButtonScript.enabled)
				{
					View.BankButtonScript.enabled = true;
					View.BankButtonCollider.enabled = true;
					View.BankButtonScript.state = UIButtonColor.State.Normal;
				}
			}
			else
			{
				if (View.ShopButtonScript != null && View.ShopButtonScript.enabled)
				{
					View.ShopButtonScript.enabled = false;
					View.ShopButtonCollider.enabled = false;
					View.ShopButtonScript.state = UIButtonColor.State.Disabled;
				}
				if (View.BankButtonScript != null && View.BankButtonScript.enabled)
				{
					View.BankButtonScript.enabled = false;
					View.BankButtonCollider.enabled = false;
					View.BankButtonScript.state = UIButtonColor.State.Disabled;
				}
			}
			if (HideButtons)
			{
				View.ShopButton.SetActiveSafe(false);
				View.BankButton.SetActiveSafe(false);
				View.ObjectForCenter.localPosition = new Vector3(0f, View.ObjectForCenter.localPosition.y, View.ObjectForCenter.localPosition.z);
			}
			else if (_viewType == BankShopViewType.Shop)
			{
				View.ShopButton.SetActiveSafe(true);
				View.BankButton.SetActiveSafe(false);
				View.ObjectForCenter.localPosition = new Vector3(0f - View.ShopButton.transform.localPosition.x, View.ObjectForCenter.localPosition.y, View.ObjectForCenter.localPosition.z);
			}
			else if (_viewType == BankShopViewType.Bank)
			{
				View.ShopButton.SetActiveSafe(false);
				View.BankButton.SetActiveSafe(true);
				View.ObjectForCenter.localPosition = new Vector3(0f - View.BankButton.transform.localPosition.x, View.ObjectForCenter.localPosition.y, View.ObjectForCenter.localPosition.z);
			}
			if (PromoActionsManager.sharedManager != null)
			{
				X3Enabled = PromoActionsManager.sharedManager.IsEventX3Active;
			}
			View.BgSimple.SetActiveSafe(!X3Enabled);
			View.BgX3.SetActiveSafe(X3Enabled);
			View.FrameX3Shop.SetActiveSafe(X3Enabled);
			View.FrameX3Bank.SetActiveSafe(X3Enabled);
			GameObject[] ticketObjects = View.ticketObjects;
			for (int i = 0; i < ticketObjects.Length; i++)
			{
				ticketObjects[i].SetActiveSafeSelf(ShowTickets);
			}
			ticketObjects = View.coinsObjects;
			for (int i = 0; i < ticketObjects.Length; i++)
			{
				ticketObjects[i].SetActiveSafeSelf(ShowCoins);
			}
			if (!ShowCoins && !MovedTickets)
			{
				ticketObjects = View.ticketObjects;
				foreach (GameObject gameObject2 in ticketObjects)
				{
					gameObject2.transform.localPosition = new Vector3(gameObject2.transform.localPosition.x + 112f, gameObject2.transform.localPosition.y, gameObject2.transform.localPosition.z);
				}
				MovedTickets = true;
			}
			View.ShopButtonCollider.center = new Vector3((ShowTickets && ShowCoins) ? (-221) : (-169), View.ShopButtonCollider.center.y, View.ShopButtonCollider.center.z);
			View.ShopButtonCollider.size = new Vector2((ShowTickets && ShowCoins) ? 460 : 347, View.ShopButtonCollider.size.y);
			View.BankButtonCollider.center = new Vector3((ShowTickets && ShowCoins) ? (-192) : (-155), View.BankButtonCollider.center.y, View.BankButtonCollider.center.z);
			View.BankButtonCollider.size = new Vector2((ShowTickets && ShowCoins) ? 400 : 320, View.BankButtonCollider.size.y);
			View.frameX3Indicators.width = ((ShowTickets && ShowCoins) ? 344 : 230);
		}

		private void OnViewClicked(object sender, EventArgs eventArgs)
		{
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}

		protected override void WhenPush()
		{
			View.gameObject.SetActiveSafe(true);
			UpdateView();
		}

		protected override void WhenPop()
		{
			View.gameObject.gameObject.SetActive(false);
		}

		protected override void OnDisable()
		{
			if (base.IsTopInStack)
			{
				PopRequest();
			}
		}
	}
}
