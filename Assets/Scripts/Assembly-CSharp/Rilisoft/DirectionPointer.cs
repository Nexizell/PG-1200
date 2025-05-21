using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(UIWidget))]
	public class DirectionPointer : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CTurnOffCoroutine_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private float _003Celapsed_003E5__1;

			public DirectionPointer _003C_003E4__this;

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
			public _003CTurnOffCoroutine_003Ed__17(int _003C_003E1__state)
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
					_003Celapsed_003E5__1 = 0f;
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (_003Celapsed_003E5__1 <= _003C_003E4__this._hideTime && _003C_003E4__this.Target == null)
				{
					_003Celapsed_003E5__1 += Time.deltaTime;
					_003C_003E4__this._widget.alpha = Mathf.Lerp(1f, 0.1f, _003Celapsed_003E5__1 / _003C_003E4__this._hideTime);
					float num = Mathf.Lerp(1f, 2f, _003Celapsed_003E5__1 / _003C_003E4__this._hideTime);
					_003C_003E4__this._widget.gameObject.transform.localScale = Vector3.one * num;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.gameObject.SetActive(false);
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

		[SerializeField]
		protected internal DirectionViewTargetType _forPointerType;

		[SerializeField]
		[Range(0f, 3f)]
		protected internal float _hideTime = 0.3f;

		public bool OutOfRange;

		private UIWidget _widgetVal;

		public DirectionViewTargetType ForPointerType
		{
			get
			{
				return _forPointerType;
			}
		}

		public DirectionViewerTarget Target { get; private set; }

		public bool IsInited
		{
			get
			{
				return Target != null;
			}
		}

		private UIWidget _widget
		{
			get
			{
				if (_widgetVal == null)
				{
					_widgetVal = GetComponent<UIWidget>();
				}
				return _widgetVal;
			}
		}

		public void TurnOn(DirectionViewerTarget pointer)
		{
			Target = pointer;
			base.gameObject.SetActive(true);
			_widget.alpha = 1f;
			_widget.gameObject.transform.localScale = Vector3.one;
		}

		public void Hide()
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(TurnOffCoroutine());
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		public void TurnOff()
		{
			Target = null;
			OutOfRange = false;
			Hide();
		}

		private IEnumerator TurnOffCoroutine()
		{
			float elapsed = 0f;
			while (elapsed <= _hideTime && Target == null)
			{
				elapsed += Time.deltaTime;
				_widget.alpha = Mathf.Lerp(1f, 0.1f, elapsed / _hideTime);
				float num = Mathf.Lerp(1f, 2f, elapsed / _hideTime);
				_widget.gameObject.transform.localScale = Vector3.one * num;
				yield return null;
			}
			gameObject.SetActive(false);
		}
	}
}
