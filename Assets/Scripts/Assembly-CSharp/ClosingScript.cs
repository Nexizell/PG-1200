using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.UI;

public sealed class ClosingScript : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ClosingScript _003C_003E4__this;

		private int _003CstartFrameIndex_003E5__1;

		private float _003Cframerate_003E5__2;

		private RectTransform _003CrectTransform_003E5__3;

		private float _003CminYScale_003E5__4;

		private Color _003CtargetColor_003E5__5;

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
		public _003CStart_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			float t;
			float num;
			float t2;
			float num2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				UnityEngine.Debug.LogWarning("Closing...");
				if (_003C_003E4__this.background != null)
				{
					_003C_003E2__current = new WaitForRealSeconds(1f);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003CrectTransform_003E5__3 = _003C_003E4__this.background.rectTransform;
				_003CstartFrameIndex_003E5__1 = Time.frameCount;
				_003Cframerate_003E5__2 = (Application.isEditor ? 300 : ((Application.targetFrameRate > 0) ? Application.targetFrameRate : 60));
				_003CminYScale_003E5__4 = 2f / (float)Screen.height;
				goto IL_00bf;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00bf;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_0181;
				}
				IL_00bf:
				t = (float)(Time.frameCount - _003CstartFrameIndex_003E5__1) / (0.5f * _003Cframerate_003E5__2);
				num = Mathf.Lerp(1f, 0f, t);
				_003CrectTransform_003E5__3.localScale = new Vector3(1f, Mathf.Max(_003CminYScale_003E5__4, num), 1f);
				if (!(num < 0.01f))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003CtargetColor_003E5__5 = new Color(_003C_003E4__this.background.color.r, _003C_003E4__this.background.color.g, _003C_003E4__this.background.color.b, 0.2f);
				goto IL_0181;
				IL_0181:
				t2 = 4f * Time.deltaTime;
				num2 = Mathf.Lerp(_003CrectTransform_003E5__3.localScale.x, 0f, t2);
				_003CrectTransform_003E5__3.localScale = new Vector3(num2, _003CrectTransform_003E5__3.localScale.y, _003CrectTransform_003E5__3.localScale.z);
				_003C_003E4__this.background.color = Color.Lerp(_003C_003E4__this.background.color, _003CtargetColor_003E5__5, t2);
				if (!(num2 < 0.001f))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003CrectTransform_003E5__3 = null;
				_003CtargetColor_003E5__5 = default(Color);
				break;
			}
			UnityEngine.Debug.LogWarning("Quitting intentionally...");
			if (!Application.isEditor)
			{
				Application.Quit();
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

	public RawImage background;

	private IEnumerator Start()
	{
		UnityEngine.Debug.LogWarning("Closing...");
		if (background != null)
		{
			yield return new WaitForRealSeconds(1f);
			RectTransform rectTransform = background.rectTransform;
			int startFrameIndex = Time.frameCount;
			float framerate = (Application.isEditor ? 300 : ((Application.targetFrameRate > 0) ? Application.targetFrameRate : 60));
			float minYScale = 2f / (float)Screen.height;
			while (true)
			{
				float t = (float)(Time.frameCount - startFrameIndex) / (0.5f * framerate);
				float num = Mathf.Lerp(1f, 0f, t);
				rectTransform.localScale = new Vector3(1f, Mathf.Max(minYScale, num), 1f);
				if (num < 0.01f)
				{
					break;
				}
				yield return null;
			}
			Color targetColor = new Color(background.color.r, background.color.g, background.color.b, 0.2f);
			while (true)
			{
				float t2 = 4f * Time.deltaTime;
				float num2 = Mathf.Lerp(rectTransform.localScale.x, 0f, t2);
				rectTransform.localScale = new Vector3(num2, rectTransform.localScale.y, rectTransform.localScale.z);
				background.color = Color.Lerp(background.color, targetColor, t2);
				if (num2 < 0.001f)
				{
					break;
				}
				yield return null;
			}
		}
		UnityEngine.Debug.LogWarning("Quitting intentionally...");
		if (!Application.isEditor)
		{
			Application.Quit();
		}
	}
}
