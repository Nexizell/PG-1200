using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftScroll : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CcrtUpdateListButton_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftScroll _003C_003E4__this;

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
		public _003CcrtUpdateListButton_003Ed__12(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (GiftBannerWindow.instance == null)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (_003C_003E4__this.wrapScript == null)
			{
				_003C_003E4__this.wrapScript = _003C_003E4__this.parentButton.GetComponent<UIWrapContent>();
			}
			_003C_003E4__this.listItemData = GiftController.Instance.Slots;
			_003C_003E4__this.SetButtonCount(_003C_003E4__this.listItemData.Count);
			for (int i = 0; i < _003C_003E4__this.listButton.Count; i++)
			{
				GiftHUDItem giftHUDItem = _003C_003E4__this.listButton[i];
				giftHUDItem.gameObject.name = i + "_" + _003C_003E4__this.listItemData[i].gift.Id;
				giftHUDItem.SetInfoButton(_003C_003E4__this.listItemData[i]);
			}
			_003C_003E4__this.Sort();
			return false;
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

	[CompilerGenerated]
	internal sealed class _003CCrtSort_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftScroll _003C_003E4__this;

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
		public _003CCrtSort_003Ed__14(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				GiftBannerWindow.instance.UpdateSizeScroll();
				_003C_003E4__this.scView.ResetPosition();
				if (_003C_003E4__this.wrapScript != null)
				{
					_003C_003E4__this.wrapScript.SortAlphabetically();
					_003C_003E4__this.wrapScript.WrapContent();
				}
				_003C_003E4__this.scView.restrictWithinPanel = true;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.scView.disableDragIfFits = false;
				_003C_003E4__this.listButton[0].InCenter();
				GiftBannerWindow.instance.UpdateSizeScroll();
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

	private List<SlotInfo> listItemData = new List<SlotInfo>();

	public List<GiftHUDItem> listButton = new List<GiftHUDItem>();

	public GiftHUDItem exampleBut;

	public GameObject parentButton;

	public UIWrapContent wrapScript;

	public UIScrollView scView;

	public BoxCollider scrollAreaCollider;

	public static bool canReCreateSlots = true;

	private void Awake()
	{
		if ((bool)exampleBut)
		{
			exampleBut.gameObject.SetActive(false);
		}
		scView = GetComponentInParent<UIScrollView>();
	}

	private void OnEnable()
	{
		GiftController.OnChangeSlots += UpdateListButton;
		UpdateListButton();
	}

	private void OnDisable()
	{
		GiftController.OnChangeSlots -= UpdateListButton;
	}

	public void UpdateListButton()
	{
		if (canReCreateSlots && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(crtUpdateListButton());
		}
	}

	private IEnumerator crtUpdateListButton()
	{
		while (GiftBannerWindow.instance == null)
		{
			yield return null;
		}
		if (wrapScript == null)
		{
			wrapScript = parentButton.GetComponent<UIWrapContent>();
		}
		listItemData = GiftController.Instance.Slots;
		SetButtonCount(listItemData.Count);
		for (int i = 0; i < listButton.Count; i++)
		{
			GiftHUDItem giftHUDItem = listButton[i];
			giftHUDItem.gameObject.name = i + "_" + listItemData[i].gift.Id;
			giftHUDItem.SetInfoButton(listItemData[i]);
		}
		Sort();
	}

	public void Sort(bool skipCheckPossible = false)
	{
		if (!canReCreateSlots || skipCheckPossible)
		{
			GiftBannerWindow.instance.UpdateSizeScroll();
			StartCoroutine(CrtSort());
		}
	}

	private IEnumerator CrtSort()
	{
		yield return null;
		GiftBannerWindow.instance.UpdateSizeScroll();
		scView.ResetPosition();
		if (wrapScript != null)
		{
			wrapScript.SortAlphabetically();
			wrapScript.WrapContent();
		}
		scView.restrictWithinPanel = true;
		yield return null;
		scView.disableDragIfFits = false;
		listButton[0].InCenter();
		GiftBannerWindow.instance.UpdateSizeScroll();
	}

	private void SetButtonCount(int needCount)
	{
		if (listButton.Count < needCount)
		{
			for (int i = listButton.Count; i < needCount; i++)
			{
				GiftHUDItem item = CreateButton();
				listButton.Add(item);
			}
		}
		else if (listButton.Count > needCount)
		{
			int num = listButton.Count - needCount;
			for (int j = 0; j < num; j++)
			{
				GameObject obj = listButton[listButton.Count - 1].gameObject;
				listButton[listButton.Count - 1] = null;
				listButton.RemoveAt(listButton.Count - 1);
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	private GiftHUDItem CreateButton()
	{
		GameObject obj = UnityEngine.Object.Instantiate(exampleBut.gameObject, Vector3.zero, Quaternion.identity);
		obj.SetActive(true);
		GiftHUDItem component = obj.GetComponent<GiftHUDItem>();
		obj.transform.parent = parentButton.transform;
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	public void AnimScrollGift(int num)
	{
		if (listButton.Count > num)
		{
			listButton[num].InCenter(true, listButton.Count);
		}
	}

	public void SetCanDraggable(bool val)
	{
		if ((bool)scrollAreaCollider)
		{
			scrollAreaCollider.enabled = val;
		}
		for (int i = 0; i < listButton.Count; i++)
		{
			listButton[i].colliderForDrag.enabled = val;
		}
	}

	[ContextMenu("Sort gift")]
	private void TestSortGift()
	{
		Sort();
	}

	[ContextMenu("Center main gift")]
	private void TestCenterGift()
	{
		if (listButton.Count > 6)
		{
			listButton[0].InCenter();
		}
	}
}
