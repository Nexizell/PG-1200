using System;
using Rilisoft;
using UnityEngine;

public class ExchangeWindow : MonoBehaviour
{
	[SerializeField]
	protected internal UILabel _countLabel;

	[SerializeField]
	protected internal TextGroup _priceLabel;

	[SerializeField]
	protected internal AudioClip _soundOpen;

	[SerializeField]
	protected internal AudioClip _soundCloseConfirm;

	[SerializeField]
	protected internal AudioClip _soundCloseDismiss;

	private Action<WindowCloseResult> _onCloseCallback;

	private static ExchangeWindow _instanceValue;

	private bool _openFromArmory;

	private IDisposable _backSubscription;

	private static ExchangeWindow Instance
	{
		get
		{
			if (_instanceValue == null)
			{
				_instanceValue = InfoWindowController.Instance.ExchangeWindow;
			}
			return _instanceValue;
		}
	}

	public static bool IsOpened
	{
		get
		{
			if (Instance != null)
			{
				return Instance.gameObject.activeInHierarchy;
			}
			return false;
		}
	}

	public static void Show(int exchengeCoinsCount, int exchangePrice, Action<WindowCloseResult> onCloseCallback)
	{
		if (!IsOpened)
		{
			Instance._openFromArmory = ShopNGUIController.GuiActive;
			Instance._countLabel.text = string.Format(LocalizationStore.Get("Key_2978"), new object[1] { exchengeCoinsCount });
			Instance._priceLabel.Text = exchangePrice.ToString();
			Instance._onCloseCallback = onCloseCallback;
			Instance.gameObject.SetActive(true);
			if (Defs.isSoundFX && Instance._soundOpen != null)
			{
				NGUITools.PlaySound(Instance._soundOpen);
			}
		}
	}

	public static void Hide()
	{
		if (IsOpened)
		{
			Instance.Dismiss();
		}
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(Dismiss, "ExchangeWindow");
	}

	private void Update()
	{
		if (ActivityIndicator.IsActiveIndicator && IsOpened)
		{
			Dismiss();
		}
		if (_openFromArmory && !ShopNGUIController.GuiActive)
		{
			Dismiss();
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Dismiss();
		}
	}

	private void Hide(WindowCloseResult result)
	{
		if (_onCloseCallback != null)
		{
			_onCloseCallback(result);
			_onCloseCallback = null;
		}
		Instance.gameObject.SetActive(false);
		if (Defs.isSoundFX)
		{
			if (result == WindowCloseResult.Confirm && _soundCloseConfirm != null)
			{
				NGUITools.PlaySound(_soundCloseConfirm);
			}
			else if (result == WindowCloseResult.Confirm && _soundCloseDismiss != null)
			{
				NGUITools.PlaySound(_soundCloseDismiss);
			}
		}
	}

	public void Confirm()
	{
		Hide(WindowCloseResult.Confirm);
	}

	public void Dismiss()
	{
		Hide(WindowCloseResult.Dismiss);
	}
}
