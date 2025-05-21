using System;
using UnityEngine;

namespace Rilisoft
{
	public class PriceView : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel _labelPrice;

		[SerializeField]
		protected internal GameObject _objGems;

		[SerializeField]
		protected internal GameObject _objCoins;

		[SerializeField]
		protected internal UILabel _labelFree;

		public event Action OnClicked;

		public void SetPrice(ItemPrice price)
		{
			SetPrice(price.Price, price.Currency == "GemsCurrency");
		}

		public void SetPrice(int price, bool isGems)
		{
			if (price > 0)
			{
				if (_labelPrice != null)
				{
					_labelPrice.gameObject.SetActive(true);
					_labelPrice.text = price.ToString();
				}
				if (_objGems != null)
				{
					_objGems.SetActive(isGems);
				}
				if (_objCoins != null)
				{
					_objCoins.SetActive(!isGems);
				}
				if (_labelFree != null)
				{
					_labelFree.gameObject.SetActive(false);
				}
			}
			else
			{
				if (_labelFree != null)
				{
					_labelFree.gameObject.SetActive(true);
				}
				if (_labelPrice != null)
				{
					_labelPrice.gameObject.SetActive(false);
				}
				if (_objGems != null)
				{
					_objGems.SetActive(false);
				}
				if (_objCoins != null)
				{
					_objCoins.SetActive(false);
				}
			}
		}

		private void OnClick()
		{
			if (this.OnClicked != null)
			{
				this.OnClicked();
			}
		}

		[ContextMenu("Execute")]
		private void Execute()
		{
		}
	}
}
