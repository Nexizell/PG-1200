using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class CraftScrollItemView : MonoBehaviour, ILobbyItemView
	{
		[CompilerGenerated]
		internal sealed class _003CSelfUpdateCoroutine_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CraftScrollItemView _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CSelfUpdateCoroutine_003Ed__34(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -1;
				}
				else
				{
					_003C_003E1__state = -1;
				}
				if (_003C_003E4__this._item != null)
				{
					if (_003C_003E4__this._item.IsCrafting)
					{
						if (LobbyItemsController.CurrentTime.HasValue)
						{
							_003C_003E4__this._labelCraftTimer.text = RiliExtensions.GetTimeString(_003C_003E4__this._item.CraftTimeLeft, ":", true);
							_003C_003E4__this._btnSpeedUp.SetPrice(_003C_003E4__this._item.PriceSpeedUpLeft);
						}
						else
						{
							_003C_003E4__this._labelCraftTimer.text = RiliExtensions.GetTimeString(_003C_003E4__this._item.Info.CraftTime, ":", true);
							_003C_003E4__this._btnSpeedUp.SetPrice(new ItemPrice(0, _003C_003E4__this._item.Info.PriceSpeedUp.Currency));
						}
					}
					if (_003C_003E4__this._isNew && _003C_003E4__this._objNew.activeInHierarchy && _003C_003E4__this.Widget != null && _003C_003E4__this.Widget.isVisible)
					{
						_003C_003E4__this._isNew = false;
					}
				}
				_003C_003E2__current = new WaitForSeconds(0.2f);
				_003C_003E1__state = 1;
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[SerializeField]
		protected internal UILabel _labelName;

		[SerializeField]
		protected internal UITexture _texture;

		[SerializeField]
		protected internal PriceView _btnCraft;

		[SerializeField]
		protected internal PriceView _btnSpeedUp;

		[SerializeField]
		protected internal PriceView _btnPlace;

		[SerializeField]
		protected internal PriceView _btnRemove;

		[SerializeField]
		protected internal PriceView _labelPrice;

		[SerializeField]
		protected internal GameObject _indicatorSelected;

		[SerializeField]
		protected internal GameObject _indicatorBought;

		[SerializeField]
		protected internal GameObject _indicatorLocked;

		[SerializeField]
		protected internal UILabel _indicatorLockedLabel;

		[SerializeField]
		protected internal UILabel _labelPlaced;

		[SerializeField]
		protected internal UILabel _labelCraftTimer;

		[SerializeField]
		protected internal GameObject _objNew;

		[SerializeField]
		protected internal GameObject _objBuffDescription;

		[SerializeField]
		protected internal TextGroup _labelBuffDescription;

		private UIWidget _widget;

		[ReadOnly]
		[SerializeField]
		private LobbyItem _item;

		private bool _isNew;

		private LobbyCraftController _controller;

		private UIWidget Widget
		{
			get
			{
				if (_widget == null)
				{
					_widget = GetComponent<UIWidget>();
				}
				return _widget;
			}
		}

		public LobbyItem LobbyItem
		{
			get
			{
				return _item;
			}
		}

		public bool IsSelected { get; set; }

		public void Setup(Transform root, LobbyCraftController controller, LobbyItem item)
		{
			_controller = controller;
			_item = item;
			base.gameObject.transform.SetParent(root);
			base.gameObject.transform.localScale = Vector3.one;
			base.gameObject.transform.localPosition = Vector3.zero;
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			UnloadTexture();
			base.gameObject.SetActive(false);
			base.gameObject.transform.SetParent(_controller.gameObject.transform);
			IsSelected = false;
		}

		public void UpdateView()
		{
			_isNew = _item.CanShowIsNew;
			_labelName.text = LocalizationStore.Get(_item.Info.Lkey);
			if (_texture.mainTexture == null || _texture.mainTexture.name != _item.TextureName)
			{
				_texture.mainTexture = Resources.Load<Texture>(_item.TexturePath);
			}
			_objNew.SetActive(false);
			_indicatorSelected.SetActiveSafe(IsSelected);
			_btnCraft.gameObject.SetActive(false);
			_btnSpeedUp.gameObject.SetActive(false);
			_btnPlace.gameObject.SetActive(false);
			_btnRemove.gameObject.SetActive(false);
			_indicatorBought.SetActive(false);
			_indicatorLocked.SetActive(false);
			_labelPrice.gameObject.SetActive(false);
			_labelPlaced.gameObject.SetActive(false);
			_labelCraftTimer.gameObject.SetActive(false);
			if (_item.Info.Level > ExpController.Instance.Rank && !_item.IsExists)
			{
				_indicatorLocked.SetActiveSafe(true);
				_indicatorLockedLabel.text = string.Format(LocalizationStore.Get("Key_3297"), new object[1] { _item.Info.Level });
			}
			else if (!LobbyItemsController.TutorialCompleted && _item.Info.Id != "decor_big_military_1")
			{
				_indicatorLocked.SetActiveSafe(true);
				if (_item.AvailableByLevel)
				{
					_indicatorLockedLabel.text = string.Empty;
					_labelPrice.gameObject.SetActive(true);
					_labelPrice.SetPrice(_item.Info.PriceBuy);
				}
				else
				{
					_indicatorLockedLabel.text = string.Format(LocalizationStore.Get("Key_3297"), new object[1] { _item.Info.Level });
				}
			}
			else
			{
				_objNew.SetActive(_isNew);
				if (_item.IsExists)
				{
					if (_item.IsEquiped)
					{
						if (IsSelected)
						{
							if (LobbyItemsController.SlotCanBeEmpty(_item.Slot))
							{
								_btnRemove.gameObject.SetActiveSafe(true);
							}
							else
							{
								_labelPlaced.gameObject.SetActive(true);
							}
						}
						else
						{
							_labelPlaced.gameObject.SetActive(true);
						}
					}
					else if (IsSelected)
					{
						_btnPlace.gameObject.SetActiveSafe(true);
					}
					else
					{
						_indicatorBought.SetActiveSafe(true);
					}
				}
				else if (_item.IsCrafting)
				{
					_labelCraftTimer.gameObject.SetActive(true);
					_labelCraftTimer.text = RiliExtensions.GetTimeString(_item.CraftTimeLeft, ":", true);
					_btnSpeedUp.gameObject.SetActiveSafe(true);
					_btnSpeedUp.SetPrice(_item.PriceSpeedUpLeft);
				}
				else
				{
					bool flag = _item.Info.PriceBuy.Currency == "GemsCurrency";
					if (IsSelected)
					{
						_btnCraft.gameObject.SetActive(true);
						_btnCraft.SetPrice(_item.Info.PriceBuy);
					}
					else
					{
						_labelPrice.gameObject.SetActiveSafe(true);
						_labelPrice.SetPrice(_item.Info.PriceBuy);
					}
				}
			}
			if (IsSelected && !_item.Info.EffectLkey.IsNullOrEmpty())
			{
				_objBuffDescription.SetActive(true);
				_labelBuffDescription.Text = LocalizationStore.Get(_item.Info.EffectLkey.Trim());
			}
			else
			{
				_objBuffDescription.SetActive(false);
			}
		}

		public void Kill()
		{
			UnloadTexture();
			base.gameObject.transform.SetParent(_controller.gameObject.transform);
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void OnEnable()
		{
			_btnCraft.OnClicked += BtnCraftOnClicked;
			_btnSpeedUp.OnClicked += BtnSpeedUpOnClicked;
			_btnPlace.OnClicked += BtnPlaceOnClicked;
			_btnRemove.OnClicked += BtnRemoveOnClicked;
			StartCoroutine(SelfUpdateCoroutine());
		}

		private void OnDisable()
		{
			_btnCraft.OnClicked -= BtnCraftOnClicked;
			_btnSpeedUp.OnClicked -= BtnSpeedUpOnClicked;
			_btnPlace.OnClicked -= BtnPlaceOnClicked;
			_btnRemove.OnClicked -= BtnRemoveOnClicked;
			if (_item != null && _item.AvailableByLevel)
			{
				_item.PlayerInfo.IsNew = _isNew;
			}
			StopCoroutine(SelfUpdateCoroutine());
		}

		private IEnumerator SelfUpdateCoroutine()
		{
			while (true)
			{
				if (_item != null)
				{
					if (_item.IsCrafting)
					{
						if (LobbyItemsController.CurrentTime.HasValue)
						{
							_labelCraftTimer.text = RiliExtensions.GetTimeString(_item.CraftTimeLeft, ":", true);
							_btnSpeedUp.SetPrice(_item.PriceSpeedUpLeft);
						}
						else
						{
							_labelCraftTimer.text = RiliExtensions.GetTimeString(_item.Info.CraftTime, ":", true);
							_btnSpeedUp.SetPrice(new ItemPrice(0, _item.Info.PriceSpeedUp.Currency));
						}
					}
					if (_isNew && _objNew.activeInHierarchy && Widget != null && Widget.isVisible)
					{
						_isNew = false;
					}
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		private void OnClick()
		{
			LobbyCraftController.Instance.SelectItemOnScrollRequest(LobbyItem);
		}

		private void BtnCraftOnClicked()
		{
			_controller.CraftRequest(this);
		}

		private void BtnSpeedUpOnClicked()
		{
			_controller.SpeedUpRequest(this);
		}

		private void BtnPlaceOnClicked()
		{
			_controller.PlaceRequest(this);
		}

		private void BtnRemoveOnClicked()
		{
			_controller.RemoveRequest(this);
		}

		private void UnloadTexture()
		{
			try
			{
				if (_texture != null && _texture.mainTexture != null)
				{
					Resources.UnloadAsset(_texture.mainTexture);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in UnloadTexture: {0}", ex);
			}
		}
	}
}
