using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(Renderer))]
	public class MaterialColorSwitcher : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CChangeColor_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MaterialColorSwitcher _003C_003E4__this;

			public int toIdx;

			private float _003Celapsed_003E5__1;

			private Color _003CstartColor_003E5__2;

			private Color _003Ccolor_003E5__3;

			public float time;

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
			public _003CChangeColor_003Ed__8(int _003C_003E1__state)
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
					_003CstartColor_003E5__2 = _003C_003E4__this._mat.color;
					_003Ccolor_003E5__3 = _003C_003E4__this.Colors[toIdx];
					_003Celapsed_003E5__1 = 0f;
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (_003Celapsed_003E5__1 < time)
				{
					_003Celapsed_003E5__1 += Time.deltaTime;
					_003C_003E4__this._mat.color = Color.Lerp(_003CstartColor_003E5__2, _003Ccolor_003E5__3, _003Celapsed_003E5__1 / time);
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this._changed = true;
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

		public List<Color> Colors = new List<Color>();

		public float ToColorTime = 1f;

		private Material _mat;

		private int _colorIdx = -1;

		private bool _changed = true;

		private void Awake()
		{
			_mat = GetComponent<Renderer>().material;
		}

		private void OnEnable()
		{
			StopAllCoroutines();
			_changed = true;
		}

		private void Update()
		{
			if (_changed)
			{
				_changed = false;
				_colorIdx = ((Colors.Count - 1 > _colorIdx) ? (_colorIdx + 1) : 0);
				StartCoroutine(ChangeColor(_colorIdx, ToColorTime));
			}
		}

		private IEnumerator ChangeColor(int toIdx, float time)
		{
			Color startColor = _mat.color;
			Color color = Colors[toIdx];
			float elapsed = 0f;
			while (elapsed < time)
			{
				elapsed += Time.deltaTime;
				_mat.color = Color.Lerp(startColor, color, elapsed / time);
				yield return null;
			}
			_changed = true;
		}
	}
}
