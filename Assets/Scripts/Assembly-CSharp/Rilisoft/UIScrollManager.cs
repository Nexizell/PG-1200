using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class UIScrollManager : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject Prefab;

		[SerializeField]
		protected internal bool Populate;

		[SerializeField]
		protected internal bool add;

		[SerializeField]
		protected internal bool clean;

		[SerializeField]
		protected internal bool center;

		[SerializeField]
		protected internal int count;

		[SerializeField]
		protected internal int centerOn;

		private List<string> _wrapItems = new List<string>();

		private UIScrollView _scrollViewValue;

		private UIGrid _gridValue;

		private UIWrapContent _wrapValue;

		private UIScrollView _scrollView
		{
			get
			{
				if (_scrollViewValue == null)
				{
					_scrollViewValue = base.gameObject.GetComponent<UIScrollView>();
				}
				return _scrollViewValue;
			}
		}

		private UIGrid _grid
		{
			get
			{
				if (_gridValue == null)
				{
					_gridValue = base.gameObject.GetComponentInChildren<UIGrid>();
				}
				return _gridValue;
			}
		}

		private UIWrapContent _wrap
		{
			get
			{
				if (_wrapValue == null)
				{
					_wrapValue = base.gameObject.GetComponentInChildren<UIWrapContent>();
				}
				return _wrapValue;
			}
		}

		private UIPanel _panel
		{
			get
			{
				return _scrollView.panel;
			}
		}

		private bool _useWrapContent
		{
			get
			{
				return _wrap != null;
			}
		}

		private void Update()
		{
			if (Populate)
			{
				Populate = false;
				if (_useWrapContent)
				{
					SetupWrap();
					PopulateWrap();
				}
				else
				{
					PopulateGrid();
				}
			}
			if (center)
			{
				center = false;
				if (_useWrapContent)
				{
					ScrollToObjectWrap(centerOn);
				}
				else
				{
					ScrollToObjectGrid(centerOn);
				}
			}
			if (add && !_useWrapContent)
			{
				add = false;
				AddObject();
			}
			if (clean)
			{
				clean = false;
				CleanGrid();
			}
		}

		private void SetupWrap()
		{
			_wrapItems.Clear();
			for (int i = 0; i < 30; i++)
			{
				_wrapItems.Add(i.ToString());
			}
			if (_scrollView.movement == UIScrollView.Movement.Vertical)
			{
				_wrap.minIndex = -(_wrapItems.Count - 1);
				_wrap.maxIndex = 0;
			}
			else if (_scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				_wrap.minIndex = 0;
				_wrap.maxIndex = _wrapItems.Count - 1;
			}
			UIWrapContent wrap = _wrap;
			wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Remove(wrap.onInitializeItem, new UIWrapContent.OnInitializeItem(OnInitializeItem));
			UIWrapContent wrap2 = _wrap;
			wrap2.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap2.onInitializeItem, new UIWrapContent.OnInitializeItem(OnInitializeItem));
		}

		private void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
		{
			int num = 0;
			if (_scrollView.movement == UIScrollView.Movement.Vertical)
			{
				num = realIndex * -1;
			}
			else if (_scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				num = realIndex;
			}
			Debug.Log(">>> index: " + num);
			string text2 = (go.name = _wrapItems[num]);
			go.GetComponentInChildren<UILabel>().text = text2;
		}

		public void ScrollTo(int itemIdx)
		{
			if (_useWrapContent)
			{
				ScrollToObjectWrap(itemIdx);
			}
			else
			{
				ScrollToObjectGrid(itemIdx);
			}
		}

		private void ScrollToObjectGrid(int itemIdx)
		{
			Transform child = _grid.transform.GetChild(itemIdx);
			if (_scrollView.movement == UIScrollView.Movement.Vertical)
			{
				float num = _panel.transform.TransformPoint(new Vector3(0f, _panel.finalClipRegion.y, 0f)).y + _panel.finalClipRegion.w / 2f * _scrollView.transform.lossyScale.y;
				float num2 = (float)(child.GetComponent<UIWidget>().height / 2) * _scrollView.transform.lossyScale.y;
				float num3 = _panel.clipSoftness.y * _scrollView.transform.lossyScale.y;
				Vector3 absolute = Vector3.up * (num - child.position.y - num2 - num3);
				_scrollView.MoveAbsolute(absolute);
			}
			else if (_scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				float num4 = _panel.transform.TransformPoint(new Vector3(_panel.finalClipRegion.x, 0f, 0f)).x - _panel.finalClipRegion.z / 2f * _scrollView.transform.lossyScale.x;
				float num5 = (float)(child.GetComponent<UIWidget>().width / 2) * _scrollView.transform.lossyScale.x;
				float num6 = _panel.clipSoftness.y * _scrollView.transform.lossyScale.x;
				Vector3 absolute2 = Vector3.right * (num4 - child.position.x + num5 + num6);
				_scrollView.MoveAbsolute(absolute2);
			}
			_scrollView.Restrict();
		}

		private void ScrollToObjectWrap(int itemIdx)
		{
			if (_scrollView.movement == UIScrollView.Movement.Vertical)
			{
				float num = _panel.finalClipRegion.y + _panel.finalClipRegion.w / 2f;
				Transform firstWrapItem = GetFirstWrapItem(_scrollView.movement);
				if (firstWrapItem == null)
				{
					return;
				}
				int num2 = Mathf.Abs(Mathf.RoundToInt(firstWrapItem.localPosition.y / (float)_wrap.itemSize));
				float num3 = firstWrapItem.localPosition.y + (float)(firstWrapItem.GetComponent<UIWidget>().height / 2) + _wrap.transform.localPosition.y - num + _panel.clipSoftness.y;
				float num4 = (float)(-(num2 - itemIdx) * _wrap.itemSize) - num3;
				int num5 = ((num4 > 0f) ? (-_wrap.itemSize) : _wrap.itemSize);
				while (num4 != 0f)
				{
					if (Mathf.Abs(num4) > (float)Mathf.Abs(num5))
					{
						_scrollView.MoveRelative(Vector3.up * -num5);
						num4 += (float)num5;
					}
					else
					{
						_scrollView.MoveRelative(Vector3.up * num4);
						num4 = 0f;
					}
					_wrap.WrapContent();
				}
			}
			else if (_scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				float num6 = _panel.finalClipRegion.x - _panel.finalClipRegion.z / 2f;
				Transform firstWrapItem2 = GetFirstWrapItem(_scrollView.movement);
				if (firstWrapItem2 == null)
				{
					return;
				}
				int num7 = Mathf.Abs(Mathf.RoundToInt(firstWrapItem2.localPosition.x / (float)_wrap.itemSize));
				int width = firstWrapItem2.GetComponent<UIWidget>().width;
				float num8 = firstWrapItem2.localPosition.x + (float)(width / 2) + (_wrap.transform.localPosition.x + num6) + _panel.clipSoftness.x;
				float num9 = (float)((num7 - itemIdx) * _wrap.itemSize) + num8;
				int num10 = ((num9 > 0f) ? (-_wrap.itemSize) : _wrap.itemSize);
				while (num9 != 0f)
				{
					if (Mathf.Abs(num9) > (float)Mathf.Abs(num10))
					{
						_scrollView.MoveRelative(Vector3.right * -num10);
						num9 += (float)num10;
					}
					else
					{
						_scrollView.MoveRelative(Vector3.right * num9);
						num9 = 0f;
					}
					_wrap.WrapContent();
				}
			}
			_scrollView.Restrict();
		}

		private Transform GetFirstWrapItem(UIScrollView.Movement movement)
		{
			IEnumerable<GameObject> source = from ch in _wrap.gameObject.Children()
				where ch.activeSelf
				select ch into go
				where go.GetComponent<UIWidget>().isVisible
				select go;
			GameObject gameObject = null;
			switch (movement)
			{
			case UIScrollView.Movement.Vertical:
				gameObject = source.OrderByDescending((GameObject ch) => ch.transform.localPosition.y).FirstOrDefault();
				break;
			case UIScrollView.Movement.Horizontal:
				gameObject = source.OrderBy((GameObject ch) => ch.transform.localPosition.x).FirstOrDefault();
				break;
			}
			if (!(gameObject != null))
			{
				return null;
			}
			return gameObject.transform;
		}

		private void PopulateGrid()
		{
			for (int i = 0; i < count; i++)
			{
				GameObject obj = UnityEngine.Object.Instantiate(Prefab, _grid.transform);
				obj.gameObject.name = Prefab.name + "_" + i;
				obj.gameObject.SetActiveSafe(true);
				_grid.Reposition();
			}
			_scrollView.ResetPosition();
		}

		private void PopulateWrap()
		{
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Prefab, _wrap.transform);
				gameObject.gameObject.SetActiveSafe(true);
				if (_scrollView.movement == UIScrollView.Movement.Vertical)
				{
					OnInitializeItem(gameObject, -i, -i);
				}
				else
				{
					OnInitializeItem(gameObject, i, i);
				}
				_wrap.WrapContent();
			}
			_wrap.SortAlphabetically();
			_scrollView.ResetPosition();
		}

		private void AddObject()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefab, _panel.transform);
			gameObject.gameObject.SetActiveSafe(true);
			_grid.AddChild(gameObject.transform);
		}

		private void CleanGrid()
		{
			for (int i = 0; i < _grid.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(_grid.transform.GetChild(i).gameObject);
			}
		}
	}
}
