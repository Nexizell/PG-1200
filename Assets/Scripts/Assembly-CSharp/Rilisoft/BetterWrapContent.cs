using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	public class BetterWrapContent : MonoBehaviour
	{
		[SerializeField]
		protected internal int _itemSize = 100;

		[SerializeField]
		protected internal bool _setCellsOnAwake = true;

		private UIPanel _panel;

		private UIScrollView _scroll;

		private GameObject _cellItem;

		private Func<int> _maxIndexGetter;

		private int _firstCellDataIndex = -1;

		[SerializeField]
		protected internal int _index = 3;

		[SerializeField]
		protected internal List<string> _dataColl;

		private int MaxIndex
		{
			get
			{
				if (_maxIndexGetter != null)
				{
					return _maxIndexGetter();
				}
				return 0;
			}
		}

		public int VisibleCellsCount
		{
			get
			{
				return (int)(_panel.height / (float)_itemSize);
			}
		}

		private int TotalCellsCount
		{
			get
			{
				return VisibleCellsCount + 2;
			}
		}

		private int _lastCellDataIndex
		{
			get
			{
				if (_firstCellDataIndex < 0)
				{
					return -1;
				}
				return _firstCellDataIndex + TotalCellsCount;
			}
		}

		public event Action<GameObject, int> OnUpdateCell;

		private void Awake()
		{
			Initialize();
			if (_setCellsOnAwake && !Application.isEditor)
			{
				SetCells();
			}
		}

		[ContextMenu("Initialize")]
		public void Initialize()
		{
			_panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			_scroll = _panel.GetComponent<UIScrollView>();
			_scroll.GetComponent<UIPanel>().onClipMove = OnClipMove;
		}

		public void Initialize(Func<int> maxIndexGetter, Action<GameObject, int> onUpdateCell)
		{
			Initialize();
			_maxIndexGetter = maxIndexGetter;
			OnUpdateCell -= onUpdateCell;
			OnUpdateCell += onUpdateCell;
			SetCells();
		}

		[ContextMenu("Set Cells")]
		private void SetCells()
		{
			int childCount = base.gameObject.transform.childCount;
			if (childCount < 1)
			{
				return;
			}
			_cellItem = base.transform.GetChild(0).gameObject;
			base.gameObject.SetChildSiblingIndexesByPositions(true, false, true);
			if (childCount > TotalCellsCount)
			{
				while (base.gameObject.Children().Count() > TotalCellsCount)
				{
					GameObject obj = base.gameObject.Children().Last();
					obj.SetActive(false);
					obj.transform.SetParent(null);
					UnityEngine.Object.Destroy(obj);
				}
			}
			else if (childCount < TotalCellsCount)
			{
				while (base.gameObject.Children().Count() < TotalCellsCount)
				{
					NGUITools.AddChild(base.gameObject, _cellItem.gameObject);
				}
			}
			OrderCells();
			_firstCellDataIndex = 0;
			int num = 0;
			foreach (GameObject item in base.gameObject.Children())
			{
				if (MaxIndex >= num)
				{
					item.SetActiveSafeSelf(true);
					if (this.OnUpdateCell != null)
					{
						this.OnUpdateCell(item.gameObject, num);
					}
				}
				else
				{
					item.SetActiveSafeSelf(false);
				}
				num++;
			}
			_scroll.Restrict();
			_scroll.ResetPosition();
		}

		[ContextMenu("Order Cells")]
		private void OrderCells()
		{
			int num = 0;
			foreach (GameObject item in base.gameObject.Children())
			{
				Transform transform = item.transform;
				transform.localPosition = new Vector3(transform.localPosition.x, -num * _itemSize, transform.localPosition.z);
				num++;
			}
		}

		[ContextMenu("Scroll")]
		private void Scroll()
		{
			ScrollTo(_index);
		}

		public void ScrollTo(int idx)
		{
			if (idx < 0 || idx > MaxIndex)
			{
				return;
			}
			int num = Mathf.Max(0, MaxIndex - VisibleCellsCount + 1);
			if (idx > num)
			{
				idx = num;
			}
			if (_scroll.movement == UIScrollView.Movement.Vertical)
			{
				float num2 = _panel.finalClipRegion.y + _panel.finalClipRegion.w / 2f;
				Transform child = base.transform.GetChild(0);
				if (child == null)
				{
					return;
				}
				int num3 = Mathf.Abs(Mathf.RoundToInt(child.localPosition.y / (float)_itemSize));
				float num4 = child.localPosition.y + (float)(child.GetComponent<UIWidget>().height / 2) + base.transform.localPosition.y - num2 + _panel.clipSoftness.y;
				float num5 = (float)(-(num3 - idx) * _itemSize) - num4;
				_scroll.MoveRelative(Vector3.up * num5);
			}
			_scroll.RestrictWithinBounds(true);
		}

		[ContextMenu("Set Test Data")]
		private void SetTestData()
		{
			_dataColl = new List<string>();
			for (int i = 0; i < 21; i++)
			{
				_dataColl.Add("item_" + i);
			}
			Initialize(() => _dataColl.Count - 1, BetterWrapContent_OnUpdateCell);
		}

		private void BetterWrapContent_OnUpdateCell(GameObject cellObj, int idx)
		{
			cellObj.name = _dataColl[idx];
			cellObj.GetComponentInChildren<UILabel>().text = cellObj.name;
		}

		private void OnClipMove(UIPanel panel)
		{
			Wrap();
		}

		private void Wrap()
		{
			float num = (float)(_itemSize * base.transform.childCount) * 0.5f;
			Vector3[] worldCorners = _panel.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				Vector3 position = worldCorners[i];
				position = base.transform.InverseTransformPoint(position);
				worldCorners[i] = position;
			}
			Vector3 vector = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
			if (_scroll.movement != UIScrollView.Movement.Vertical)
			{
				return;
			}
			Transform child = base.transform.GetChild(0);
			Transform child2 = base.transform.GetChild(base.transform.childCount - 1);
			float num2 = child.localPosition.y - vector.y;
			float num3 = child2.localPosition.y - vector.y;
			if (num2 > num)
			{
				if (_lastCellDataIndex <= MaxIndex)
				{
					child.SetSiblingIndex(base.transform.childCount);
					child.localPosition = new Vector3(child.localPosition.x, child2.localPosition.y - (float)_itemSize, 0f);
					_firstCellDataIndex++;
					if (this.OnUpdateCell != null)
					{
						this.OnUpdateCell(child.gameObject, _lastCellDataIndex - 1);
					}
					Wrap();
				}
			}
			else if (num3 < 0f - num && _firstCellDataIndex - 1 >= 0)
			{
				child2.SetSiblingIndex(0);
				child2.localPosition = new Vector3(child.localPosition.x, child.localPosition.y + (float)_itemSize, 0f);
				_firstCellDataIndex--;
				if (this.OnUpdateCell != null)
				{
					this.OnUpdateCell(child2.gameObject, _firstCellDataIndex);
				}
				Wrap();
			}
		}
	}
}
