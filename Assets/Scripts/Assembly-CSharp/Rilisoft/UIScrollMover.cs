using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.NullExtensions;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(UIScrollView))]
	public sealed class UIScrollMover : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CMoveCoroutine_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public UIScrollMover _003C_003E4__this;

			public float offset;

			public float moveSpeed;

			private float _003Cleft_003E5__1;

			public Action onMoveEnded;

			public bool isHorizontal;

			private Transform _003CpanelTransform_003E5__2;

			public bool detectEmptySides;

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
			public _003CMoveCoroutine_003Ed__15(int _003C_003E1__state)
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
					if (_003C_003E4__this._rotateCoroutineIsRunning)
					{
						return false;
					}
					_003C_003E4__this._rotateCoroutineIsRunning = true;
					_003C_003E4__this._scrollView.enabled = false;
					_003C_003E4__this._scrollView.restrictWithinPanel = false;
					SpringPanel component = _003C_003E4__this._scrollView.gameObject.GetComponent<SpringPanel>();
					if (component != null && component.enabled)
					{
						component.enabled = false;
						component.target = component.transform.localPosition;
					}
					_003CpanelTransform_003E5__2 = _003C_003E4__this._scrollView.panel.cachedTransform;
					_003Cleft_003E5__1 = Mathf.Abs(offset);
					break;
				}
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (_003Cleft_003E5__1 > 0f)
				{
					float num = (_003C_003E4__this.InstantMoving ? offset : (offset * moveSpeed * Time.unscaledDeltaTime * 0.1f));
					if (_003Cleft_003E5__1 - Mathf.Abs(num) < 0f)
					{
						num = ((_003Cleft_003E5__1 - Mathf.Abs(num) * num > 0f) ? 1 : (-1));
						_003Cleft_003E5__1 = -1f;
					}
					if (!_003C_003E4__this._scrollView.shouldMove)
					{
						_003C_003E4__this.MoveEnded();
						if (onMoveEnded != null)
						{
							onMoveEnded();
						}
						return false;
					}
					if (isHorizontal)
					{
						_003CpanelTransform_003E5__2.localPosition -= new Vector3(num, 0f, 0f);
						Vector4 vector = _003C_003E4__this._scrollView.panel.clipOffset;
						vector.x += num;
						_003C_003E4__this._scrollView.panel.clipOffset = vector;
					}
					else
					{
						_003CpanelTransform_003E5__2.localPosition -= new Vector3(0f, num, 0f);
						Vector4 vector2 = _003C_003E4__this._scrollView.panel.clipOffset;
						vector2.y += num;
						_003C_003E4__this._scrollView.panel.clipOffset = vector2;
					}
					_003Cleft_003E5__1 -= Mathf.Abs(num);
					if (detectEmptySides)
					{
						Bounds bounds = _003C_003E4__this._scrollView.bounds;
						Vector3 vector3 = _003C_003E4__this._scrollView.panel.CalculateConstrainOffset(bounds.min, bounds.max);
						if (Mathf.Abs(vector3.x) > 0f)
						{
							if (isHorizontal)
							{
								_003CpanelTransform_003E5__2.localPosition += new Vector3(vector3.x, 0f, 0f);
								Vector4 vector4 = _003C_003E4__this._scrollView.panel.clipOffset;
								vector4.x -= vector3.x;
								_003C_003E4__this._scrollView.panel.clipOffset = vector4;
							}
							else
							{
								_003CpanelTransform_003E5__2.localPosition += new Vector3(0f, vector3.y, 0f);
								Vector4 vector5 = _003C_003E4__this._scrollView.panel.clipOffset;
								vector5.y -= vector3.y;
								_003C_003E4__this._scrollView.panel.clipOffset = vector5;
							}
							_003Cleft_003E5__1 = -1f;
						}
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.MoveEnded();
				if (onMoveEnded != null)
				{
					onMoveEnded();
				}
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

		private bool _rotateCoroutineIsRunning;

		private UIScrollView _scrollView;

		public float MoveSpeed = 1f;

		public bool DetectEmptySides = true;

		public bool InstantMoving;

		[SerializeField]
		protected internal GameObject _itemsContainer;

		public bool IsMoving
		{
			get
			{
				return _rotateCoroutineIsRunning;
			}
		}

		private GameObject ItemsContainer
		{
			get
			{
				if (_itemsContainer == null)
				{
					_scrollView.GetComponentInChildren<UIGrid>().Do(delegate(UIGrid grid)
					{
						_itemsContainer = grid.gameObject;
					});
				}
				return _itemsContainer;
			}
		}

		private void Awake()
		{
			_scrollView = GetComponent<UIScrollView>();
		}

		private void Start()
		{
			_scrollView.gameObject.GetComponent<UIPanel>().UpdateAnchors();
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			MoveEnded();
		}

		private IEnumerable<Transform> AllItems()
		{
			return from o in ItemsContainer.Children()
				select o.GetComponent<Transform>();
		}

		public void MoveTo(int itemSiblingIndex, Action onMoveEnded = null)
		{
			if (!_scrollView.shouldMove)
			{
				if (onMoveEnded != null)
				{
					onMoveEnded();
				}
				return;
			}
			List<Transform> list = (from i in AllItems()
				orderby i.localPosition.x
				select i).ToList();
			if (list.Count >= 3 && itemSiblingIndex >= 0 && itemSiblingIndex <= list.Count - 1)
			{
				Transform transform = list[itemSiblingIndex];
				float moveSpeed = MoveSpeed * (float)list.Count;
				Vector3[] worldCorners = _scrollView.panel.worldCorners;
				Vector3 position = (worldCorners[2] + worldCorners[0]) * 0.5f;
				Transform cachedTransform = _scrollView.panel.cachedTransform;
				Vector3 vector = cachedTransform.InverseTransformPoint(transform.transform.position);
				Vector3 vector2 = cachedTransform.InverseTransformPoint(position);
				Vector3 vector3 = vector - vector2;
				if (!_scrollView.canMoveHorizontally)
				{
					vector3.x = 0f;
				}
				if (!_scrollView.canMoveVertically)
				{
					vector3.y = 0f;
				}
				vector3.z = 0f;
				StartCoroutine(MoveCoroutine(vector3.x, _scrollView.canMoveHorizontally, DetectEmptySides, moveSpeed, onMoveEnded));
			}
		}

		private IEnumerator MoveCoroutine(float offset, bool isHorizontal, bool detectEmptySides, float moveSpeed, Action onMoveEnded)
		{
			if (_rotateCoroutineIsRunning)
			{
				yield break;
			}
			_rotateCoroutineIsRunning = true;
			_scrollView.enabled = false;
			_scrollView.restrictWithinPanel = false;
			SpringPanel component = _scrollView.gameObject.GetComponent<SpringPanel>();
			if (component != null && component.enabled)
			{
				component.enabled = false;
				component.target = component.transform.localPosition;
			}
			Transform panelTransform = _scrollView.panel.cachedTransform;
			float left = Mathf.Abs(offset);
			while (left > 0f)
			{
				float num = (InstantMoving ? offset : (offset * moveSpeed * Time.unscaledDeltaTime * 0.1f));
				if (left - Mathf.Abs(num) < 0f)
				{
					num = ((left - Mathf.Abs(num) * num > 0f) ? 1 : (-1));
					left = -1f;
				}
				if (!_scrollView.shouldMove)
				{
					MoveEnded();
					if (onMoveEnded != null)
					{
						onMoveEnded();
					}
					yield break;
				}
				if (isHorizontal)
				{
					panelTransform.localPosition -= new Vector3(num, 0f, 0f);
					Vector4 vector = _scrollView.panel.clipOffset;
					vector.x += num;
					_scrollView.panel.clipOffset = vector;
				}
				else
				{
					panelTransform.localPosition -= new Vector3(0f, num, 0f);
					Vector4 vector2 = _scrollView.panel.clipOffset;
					vector2.y += num;
					_scrollView.panel.clipOffset = vector2;
				}
				left -= Mathf.Abs(num);
				if (detectEmptySides)
				{
					Bounds bounds = _scrollView.bounds;
					Vector3 vector3 = _scrollView.panel.CalculateConstrainOffset(bounds.min, bounds.max);
					if (Mathf.Abs(vector3.x) > 0f)
					{
						if (isHorizontal)
						{
							panelTransform.localPosition += new Vector3(vector3.x, 0f, 0f);
							Vector4 vector4 = _scrollView.panel.clipOffset;
							vector4.x -= vector3.x;
							_scrollView.panel.clipOffset = vector4;
						}
						else
						{
							panelTransform.localPosition += new Vector3(0f, vector3.y, 0f);
							Vector4 vector5 = _scrollView.panel.clipOffset;
							vector5.y -= vector3.y;
							_scrollView.panel.clipOffset = vector5;
						}
						left = -1f;
					}
				}
				yield return null;
			}
			MoveEnded();
			if (onMoveEnded != null)
			{
				onMoveEnded();
			}
		}

		private void MoveEnded()
		{
			_rotateCoroutineIsRunning = false;
			_scrollView.enabled = true;
			_scrollView.restrictWithinPanel = true;
		}
	}
}
