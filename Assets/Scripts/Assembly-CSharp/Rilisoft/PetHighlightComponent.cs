using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class PetHighlightComponent : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CDamageCoroutine_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PetHighlightComponent _003C_003E4__this;

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
			public _003CDamageCoroutine_003Ed__12(int _003C_003E1__state)
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
					if (_003C_003E4__this._damageCoroutineIsRunnig)
					{
						_003C_003E4__this.ResetHit();
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0051;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0051;
				case 2:
					{
						_003C_003E1__state = -1;
						_003C_003E4__this.ResetHit();
						return false;
					}
					IL_0051:
					_003C_003E4__this._damageCoroutineIsRunnig = true;
					_003C_003E4__this._rend.material.mainTexture = _003C_003E4__this._damageTexture;
					_003C_003E2__current = new WaitForSeconds(_003C_003E4__this._splashTime);
					_003C_003E1__state = 2;
					return true;
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

		[CompilerGenerated]
		internal sealed class _003CImmortalBlinkCoroutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private float _003CelapsedTime_003E5__1;

			private int _003CloopsCount_003E5__2;

			private float _003CloopTime_003E5__3;

			public PetHighlightComponent _003C_003E4__this;

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
			public _003CImmortalBlinkCoroutine_003Ed__16(int _003C_003E1__state)
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
					_003CloopTime_003E5__3 = 0.4f;
					_003CloopsCount_003E5__2 = 1;
					_003CelapsedTime_003E5__1 = 0f;
				}
				_003CelapsedTime_003E5__1 += Time.deltaTime;
				float t = ((_003CloopsCount_003E5__2 % 2 != 0) ? (_003CelapsedTime_003E5__1 / _003CloopTime_003E5__3) : (_003CelapsedTime_003E5__1 / _003CloopTime_003E5__3 * -1f));
				_003C_003E4__this.SetColor(Color.Lerp(_003C_003E4__this._baseColor, _003C_003E4__this._immortalColor, t));
				if (_003CelapsedTime_003E5__1 > _003CloopTime_003E5__3)
				{
					_003CelapsedTime_003E5__1 = 0f;
					_003CloopsCount_003E5__2++;
				}
				_003C_003E2__current = null;
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
		protected internal Texture2D _damageTexture;

		[SerializeField]
		protected internal Renderer _rend;

		[SerializeField]
		protected internal Color _immortalColor;

		[SerializeField]
		[Range(0f, 2f)]
		protected internal float _splashTime = 0.3f;

		[ReadOnly]
		[SerializeField]
		protected internal Texture _baseTexture;

		private Color _baseColor;

		[SerializeField]
		protected internal string _shaderColorProp = "_ColorRili";

		private bool _damageCoroutineIsRunnig;

		private bool _immortalBlinkStarted;

		private void Awake()
		{
			_baseTexture = _rend.material.mainTexture;
			if (_rend.material.HasProperty(_shaderColorProp))
			{
				_baseColor = _rend.material.GetColor(_shaderColorProp);
				return;
			}
			UnityEngine.Debug.LogError(string.Format("shader property '{0}' not found", new object[1] { _shaderColorProp }));
		}

		private void OnDisable()
		{
			ResetHit();
			ImmortalBlinkStop();
		}

		public void Hit()
		{
			StopCoroutine(DamageCoroutine());
			StartCoroutine(DamageCoroutine());
		}

		private void ResetHit()
		{
			_damageCoroutineIsRunnig = false;
			_rend.material.mainTexture = _baseTexture;
		}

		private IEnumerator DamageCoroutine()
		{
			if (_damageCoroutineIsRunnig)
			{
				ResetHit();
				yield return null;
			}
			_damageCoroutineIsRunnig = true;
			_rend.material.mainTexture = _damageTexture;
			yield return new WaitForSeconds(_splashTime);
			ResetHit();
		}

		public void ImmortalBlinkStart(float time)
		{
			if (!_immortalBlinkStarted)
			{
				StartCoroutine("ImmortalBlinkCoroutine");
				_immortalBlinkStarted = true;
			}
		}

		public void ImmortalBlinkStop()
		{
			StopCoroutine("ImmortalBlinkCoroutine");
			_immortalBlinkStarted = false;
			SetColor(_baseColor);
		}

		private IEnumerator ImmortalBlinkCoroutine()
		{
			float loopTime = 0.4f;
			int loopsCount = 1;
			float elapsedTime = 0f;
			while (true)
			{
				elapsedTime += Time.deltaTime;
				float t = ((loopsCount % 2 != 0) ? (elapsedTime / loopTime) : (elapsedTime / loopTime * -1f));
				SetColor(Color.Lerp(_baseColor, _immortalColor, t));
				if (elapsedTime > loopTime)
				{
					elapsedTime = 0f;
					loopsCount++;
				}
				yield return null;
			}
		}

		private void SetColor(Color color)
		{
			if (_rend.material.HasProperty(_shaderColorProp))
			{
				_rend.material.SetColor(_shaderColorProp, color);
				return;
			}
			UnityEngine.Debug.LogError(string.Format("shader property '{0}' not found", new object[1] { _shaderColorProp }));
		}
	}
}
