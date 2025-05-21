using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PreloadTexture : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CCrt_LoadTexture_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PreloadTexture _003C_003E4__this;

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
		public _003CCrt_LoadTexture_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			Texture mainTexture;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_003b:
				if (string.IsNullOrEmpty(_003C_003E4__this.pathTexture))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				mainTexture = Resources.Load<Texture>(_003C_003E4__this.pathTexture);
				if (_003C_003E4__this.nguiTexture != null)
				{
					_003C_003E4__this.nguiTexture.mainTexture = mainTexture;
				}
				_003C_003E2__current = null;
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

	public string pathTexture;

	public bool clearMemoryOnUnload = true;

	private UITexture nguiTexture;

	private void OnEnable()
	{
		if (Device.IsLoweMemoryDevice)
		{
			if (nguiTexture == null)
			{
				nguiTexture = GetComponent<UITexture>();
			}
			if (nguiTexture != null)
			{
				StartCoroutine(Crt_LoadTexture());
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private IEnumerator Crt_LoadTexture()
	{
		while (string.IsNullOrEmpty(pathTexture))
		{
			yield return null;
		}
		Texture mainTexture = Resources.Load<Texture>(pathTexture);
		if (nguiTexture != null)
		{
			nguiTexture.mainTexture = mainTexture;
		}
		yield return null;
	}

	private void OnDisable()
	{
		if (Device.IsLoweMemoryDevice)
		{
			if (nguiTexture != null)
			{
				nguiTexture.mainTexture = null;
			}
			ActivityIndicator.ClearMemory();
		}
	}
}
