using System;
using Photon;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemSpeedUpWindow : Photon.MonoBehaviour
	{
		public enum CloseResult
		{
			Dismiss = 0,
			SpeedUp = 1,
			BuyNow = 2
		}

		[SerializeField]
		protected internal UILabel _labelSpeedUpItemName;

		[SerializeField]
		protected internal UILabel _labelSpeedUpTimeLeft;

		[SerializeField]
		protected internal UITexture _textureSpeedUpItem;

		[SerializeField]
		protected internal UILabel _labelBuyItemName;

		[SerializeField]
		protected internal UITexture _textureBuyItem;

		[SerializeField]
		protected internal PriceView _btnSpeedUp;

		[SerializeField]
		protected internal PriceView _btnBuy;

		private IDisposable _backSubscription;

		private Action<CloseResult, LobbyItem, LobbyItem> _closeCallback;

		private LobbyItem _speedUpItem;

		private LobbyItem _buyNowItem;

		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		public void U_Dismiss()
		{
			HideWindow(CloseResult.Dismiss);
		}

		public void Show(LobbyItem speedUpItem, LobbyItem buyNowItem, Action<CloseResult, LobbyItem, LobbyItem> closeCallback)
		{
			base.gameObject.SetActive(true);
			_closeCallback = closeCallback;
			_speedUpItem = speedUpItem;
			_buyNowItem = buyNowItem;
			if (_speedUpItem != null)
			{
				_labelSpeedUpItemName.text = LocalizationStore.Get(_speedUpItem.Info.Lkey);
				_textureSpeedUpItem.mainTexture = Resources.Load<Texture>(_speedUpItem.TexturePath);
				_btnSpeedUp.SetPrice(_speedUpItem.PriceSpeedUpLeft);
			}
			if (_buyNowItem != null)
			{
				_labelBuyItemName.text = LocalizationStore.Get(_buyNowItem.Info.Lkey);
				_textureBuyItem.mainTexture = Resources.Load<Texture>(_buyNowItem.TexturePath);
				_btnBuy.SetPrice(_buyNowItem.Info.PriceInstant);
			}
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
			_closeCallback = null;
			_speedUpItem = null;
			_buyNowItem = null;
		}

		private void Update()
		{
			if (!LobbyCraftController.Instance.InterfaceEnabled)
			{
				HideWindow(CloseResult.Dismiss);
			}
			else
			{
				if (_speedUpItem == null)
				{
					return;
				}
				if (_speedUpItem.IsExists)
				{
					HideWindow(CloseResult.Dismiss);
					return;
				}
				if (_btnSpeedUp != null)
				{
					_btnSpeedUp.SetPrice(_speedUpItem.PriceSpeedUpLeft);
				}
				if (_labelSpeedUpTimeLeft != null)
				{
					_labelSpeedUpTimeLeft.text = RiliExtensions.GetTimeString(_speedUpItem.CraftTimeLeft, ":", true);
				}
			}
		}

		private void OnEnable()
		{
			_btnSpeedUp.OnClicked += BtnSpeedUpOnClicked;
			_btnBuy.OnClicked += BtnBuyOnClicked;
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(delegate
			{
				HideWindow(CloseResult.Dismiss);
			}, "craft speedup window");
		}

		private void OnDisable()
		{
			_btnSpeedUp.OnClicked -= BtnSpeedUpOnClicked;
			_btnBuy.OnClicked -= BtnBuyOnClicked;
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
				_backSubscription = null;
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				Hide();
			}
		}

		private void BtnSpeedUpOnClicked()
		{
			HideWindow(CloseResult.SpeedUp);
		}

		private void BtnBuyOnClicked()
		{
			HideWindow(CloseResult.BuyNow);
		}

		private void HideWindow(CloseResult result)
		{
			if (_closeCallback != null)
			{
				_closeCallback(result, _speedUpItem, _buyNowItem);
			}
			Hide();
		}
	}
}
