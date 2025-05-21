using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlickButton : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CBlink_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BlickButton _003C_003E4__this;

		private int _003Ci_003E5__1;

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
		public _003CBlink_003Ed__8(int _003C_003E1__state)
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
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(_003C_003E4__this.firstSdvig));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_007a;
			case 2:
				_003C_003E1__state = -1;
				goto IL_007a;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.blickSprite.gameObject.SetActive(true);
				_003Ci_003E5__1 = 0;
				goto IL_0155;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__1++;
					goto IL_0155;
				}
				IL_0155:
				if (_003Ci_003E5__1 < _003C_003E4__this.countFrame)
				{
					_003C_003E4__this.blickSprite.spriteName = _003C_003E4__this.baseNameSprite + _003Ci_003E5__1;
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(_003C_003E4__this.blickSpeed));
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003E4__this.blickSprite.gameObject.SetActive(false);
				goto IL_007a;
				IL_007a:
				if (_003C_003E4__this.baseButton.state == UIButtonColor.State.Disabled)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(_003C_003E4__this.blickPeriod));
				_003C_003E1__state = 3;
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
	internal sealed class _003CMyWaitForSeconds_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public float tm;

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
		public _003CMyWaitForSeconds_003Ed__9(int _003C_003E1__state)
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
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__1 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
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

	public float firstSdvig;

	public float blickPeriod = 3f;

	public float blickSpeed = 0.3f;

	public UISprite blickSprite;

	public UIButton baseButton;

	public string baseNameSprite;

	public int countFrame;

	private void Start()
	{
		blickSprite.gameObject.SetActive(false);
		StartCoroutine(Blink());
	}

	private IEnumerator Blink()
	{
		yield return StartCoroutine(MyWaitForSeconds(firstSdvig));
		while (true)
		{
			if (baseButton.state == UIButtonColor.State.Disabled)
			{
				yield return null;
				continue;
			}
			yield return StartCoroutine(MyWaitForSeconds(blickPeriod));
			blickSprite.gameObject.SetActive(true);
			for (int i = 0; i < countFrame; i++)
			{
				blickSprite.spriteName = baseNameSprite + i;
				yield return StartCoroutine(MyWaitForSeconds(blickSpeed));
			}
			blickSprite.gameObject.SetActive(false);
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private void Update()
	{
	}
}
