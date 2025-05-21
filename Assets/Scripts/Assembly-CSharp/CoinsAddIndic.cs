using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoinsAddIndic : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003Cblink_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CoinsAddIndic _003C_003E4__this;

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
		public _003Cblink_003Ed__25(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
			case 3:
			case 4:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (_003C_003E4__this.ind == null)
					{
						UnityEngine.Debug.LogWarning("Indicator sprite is null.");
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0068;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0068;
				case 2:
					_003C_003E1__state = -3;
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -3;
					_003C_003E4__this.ind.color = _003C_003E4__this.NormalColor();
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -3;
						_003Ci_003E5__1++;
						break;
					}
					IL_0068:
					_003C_003E4__this.blinking = true;
					_003C_003E1__state = -3;
					_003Ci_003E5__1 = 0;
					break;
				}
				if (_003Ci_003E5__1 < 15)
				{
					_003C_003E4__this.ind.color = _003C_003E4__this.BlinkColor;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E4__this.ind.color = _003C_003E4__this.NormalColor();
				_003C_003Em__Finally1();
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this.blinking = false;
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const float blinkR = 255f;

	private const float blinkG = 255f;

	private const float blinkB = 0f;

	private const float blinkA = 115f;

	private const float blinkGemsR = 0f;

	private const float blinkGemsG = 0f;

	private const float blinkGemsB = 255f;

	private const float blinkGemsA = 115f;

	public bool stopBlinkingOnEnable;

	public bool isGems;

	public bool isX3;

	private UISprite _ind;

	private bool blinking;

	public bool remembeState;

	private int backgroundAdd;

	private UISprite ind
	{
		get
		{
			if (_ind == null)
			{
				_ind = GetComponent<UISprite>();
			}
			return _ind;
		}
	}

	private Color BlinkColor
	{
		get
		{
			if (!isGems)
			{
				return new Color(1f, 1f, 0f, 23f / 51f);
			}
			return new Color(0f, 0f, 1f, 23f / 51f);
		}
	}

	private void Start()
	{
		if (remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= BackgroundEventAdd;
			CoinsMessage.CoinsLabelDisappeared += BackgroundEventAdd;
		}
	}

	private void OnEnable()
	{
		CoinsMessage.CoinsLabelDisappeared += IndicateCoinsAdd;
		if (ind != null)
		{
			ind.color = NormalColor();
		}
		if (backgroundAdd > 0)
		{
			blinking = false;
			IndicateCoinsAdd(backgroundAdd == 1, 2);
			backgroundAdd = 0;
		}
		if (blinking && !stopBlinkingOnEnable)
		{
			StartCoroutine(blink());
		}
		else if (stopBlinkingOnEnable)
		{
			blinking = false;
		}
	}

	private void OnDisable()
	{
		CoinsMessage.CoinsLabelDisappeared -= IndicateCoinsAdd;
	}

	private void OnDestroy()
	{
		if (remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= BackgroundEventAdd;
		}
	}

	private void IndicateCoinsAdd(bool gems, int count)
	{
		if (isGems == gems && !blinking)
		{
			StartCoroutine(blink());
		}
	}

	private Color NormalColor()
	{
		if (!isX3)
		{
			return new Color(0f, 0f, 0f, 23f / 51f);
		}
		return new Color(1f, 0f, 0f, 0.5882353f);
	}

	private IEnumerator blink()
	{
		if (ind == null)
		{
			UnityEngine.Debug.LogWarning("Indicator sprite is null.");
			yield return null;
		}
		blinking = true;
		try
		{
			for (int i = 0; i < 15; i++)
			{
				ind.color = BlinkColor;
				yield return null;
				yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
				ind.color = NormalColor();
				yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
			}
			ind.color = NormalColor();
		}
		finally
		{
			blinking = false;
		}
	}

	private void BackgroundEventAdd(bool gems, int count)
	{
		if (isGems == gems && (BankController.Instance == null || !BankController.Instance.InterfaceEnabled) && GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
		{
			if (gems && isGems)
			{
				backgroundAdd = 1;
			}
			if (!gems && !isGems)
			{
				backgroundAdd = 2;
			}
		}
	}
}
