using UnityEngine;

namespace Rilisoft
{
	public class BankShopView : MonoBehaviour
	{
		[Header("object bindings")]
		public GameObject[] coinsObjects;

		public GameObject[] ticketObjects;

		public UISprite frameX3Indicators;

		public Transform ObjectForCenter;

		public GameObject ShopButton;

		public GameObject BankButton;

		public GameObject BgSimple;

		public GameObject BgX3;

		public GameObject FrameX3Shop;

		public GameObject FrameX3Bank;

		private UIButton _shopButtonScriptValue;

		private ButtonHandler _shopButtonHandlerValue;

		private BoxCollider _shopButtonColliderValue;

		private UIButton _bankButtonScriptValue;

		private ButtonHandler _bankButtonHandlerValue;

		private BoxCollider _bankButtonColliderValue;

		public UIButton ShopButtonScript
		{
			get
			{
				if (_shopButtonScriptValue == null)
				{
					_shopButtonScriptValue = ShopButton.GetComponent<UIButton>();
				}
				return _shopButtonScriptValue;
			}
		}

		public ButtonHandler ShopButtonHandler
		{
			get
			{
				if (_shopButtonHandlerValue == null)
				{
					_shopButtonHandlerValue = ShopButton.GetComponent<ButtonHandler>();
				}
				return _shopButtonHandlerValue;
			}
		}

		public BoxCollider ShopButtonCollider
		{
			get
			{
				if (_shopButtonColliderValue == null)
				{
					_shopButtonColliderValue = ShopButton.GetComponent<BoxCollider>();
				}
				return _shopButtonColliderValue;
			}
		}

		public UIButton BankButtonScript
		{
			get
			{
				if (_bankButtonScriptValue == null)
				{
					_bankButtonScriptValue = BankButton.GetComponent<UIButton>();
				}
				return _bankButtonScriptValue;
			}
		}

		public ButtonHandler BankButtonHandler
		{
			get
			{
				if (_bankButtonHandlerValue == null)
				{
					_bankButtonHandlerValue = BankButton.GetComponent<ButtonHandler>();
				}
				return _bankButtonHandlerValue;
			}
		}

		public BoxCollider BankButtonCollider
		{
			get
			{
				if (_bankButtonColliderValue == null)
				{
					_bankButtonColliderValue = BankButton.GetComponent<BoxCollider>();
				}
				return _bankButtonColliderValue;
			}
		}
	}
}
