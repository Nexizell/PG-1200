using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StickerPackScroll : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CcrtUpdateListButton_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StickerPackScroll _003C_003E4__this;

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
		public _003CcrtUpdateListButton_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (_003C_003E4__this.sortScript == null)
				{
					_003C_003E4__this.sortScript = _003C_003E4__this.parentButton.GetComponent<UIGrid>();
				}
				_003C_003E4__this.listItemData = StickersController.GetAvaliablePack();
				BtnPackItem btnPackItem = null;
				for (int i = 0; i < _003C_003E4__this.listButton.Count; i++)
				{
					BtnPackItem btnPackItem2 = _003C_003E4__this.listButton[i];
					if (_003C_003E4__this.listItemData.Contains(btnPackItem2.typePack))
					{
						btnPackItem2.transform.parent = _003C_003E4__this.parentButton.transform;
						btnPackItem2.gameObject.SetActive(true);
						if (btnPackItem == null)
						{
							btnPackItem = btnPackItem2;
						}
					}
					else
					{
						btnPackItem2.transform.parent = _003C_003E4__this.transform;
						btnPackItem2.gameObject.SetActive(false);
					}
				}
				if (btnPackItem != null)
				{
					_003C_003E4__this.ShowPack(btnPackItem.typePack);
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.Sort();
				return false;
			}
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

	public List<TypePackSticker> listItemData = new List<TypePackSticker>();

	public List<BtnPackItem> listButton = new List<BtnPackItem>();

	public GameObject parentButton;

	public TypePackSticker curShowPack;

	private UIGrid sortScript;

	private void Awake()
	{
		listButton.Clear();
		listButton.AddRange(GetComponentsInChildren<BtnPackItem>(true));
	}

	private void OnEnable()
	{
		UpdateListButton();
		StickersController.onBuyPack += UpdateListButton;
	}

	private void OnDisable()
	{
		StickersController.onBuyPack -= UpdateListButton;
	}

	public void UpdateListButton()
	{
		StartCoroutine(crtUpdateListButton());
	}

	private IEnumerator crtUpdateListButton()
	{
		if (sortScript == null)
		{
			sortScript = parentButton.GetComponent<UIGrid>();
		}
		listItemData = StickersController.GetAvaliablePack();
		BtnPackItem btnPackItem = null;
		for (int i = 0; i < listButton.Count; i++)
		{
			BtnPackItem btnPackItem2 = listButton[i];
			if (listItemData.Contains(btnPackItem2.typePack))
			{
				btnPackItem2.transform.parent = parentButton.transform;
				btnPackItem2.gameObject.SetActive(true);
				if (btnPackItem == null)
				{
					btnPackItem = btnPackItem2;
				}
			}
			else
			{
				btnPackItem2.transform.parent = transform;
				btnPackItem2.gameObject.SetActive(false);
			}
		}
		if (btnPackItem != null)
		{
			ShowPack(btnPackItem.typePack);
		}
		yield return null;
		Sort();
	}

	public void Sort()
	{
		if (sortScript != null)
		{
			parentButton.SetActive(false);
			parentButton.SetActive(true);
			sortScript.Reposition();
		}
	}

	public void ShowPack(TypePackSticker val)
	{
		for (int i = 0; i < listButton.Count; i++)
		{
			BtnPackItem btnPackItem = listButton[i];
			if (btnPackItem.typePack == val)
			{
				btnPackItem.ShowPack();
				curShowPack = btnPackItem.typePack;
			}
			else
			{
				btnPackItem.HidePack();
			}
		}
	}
}
