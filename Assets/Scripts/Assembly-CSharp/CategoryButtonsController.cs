using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class CategoryButtonsController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CAnimateButtons_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CategoryButtonsController _003C_003E4__this;

		private float _003CanimationTimer_003E5__1;

		private BtnCategory _003CcurrentButton_003E5__2;

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
		public _003CAnimateButtons_003Ed__10(int _003C_003E1__state)
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
				_003CanimationTimer_003E5__1 = _003C_003E4__this.animTime;
				goto IL_006e;
			case 1:
				_003C_003E1__state = -1;
				goto IL_006e;
			case 2:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.buttonsTable.Reposition();
					goto IL_00d8;
				}
				IL_006e:
				if (_003CanimationTimer_003E5__1 > 0f)
				{
					_003CanimationTimer_003E5__1 -= Time.unscaledDeltaTime;
					_003C_003E4__this.buttonsTable.Reposition();
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E4__this.currentBtnName == null)
				{
					break;
				}
				_003CcurrentButton_003E5__2 = _003C_003E4__this.buttons.FirstOrDefault((BtnCategory btn) => btn.btnName == _003C_003E4__this.currentBtnName);
				goto IL_00d8;
				IL_00d8:
				if (_003CcurrentButton_003E5__2 != null && _003CcurrentButton_003E5__2.IsAnimationPlayed)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003CcurrentButton_003E5__2 = null;
				break;
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

	public BtnCategory[] buttons;

	public float scaleMultypler = 1.1f;

	public float animTime = 0.7f;

	public UITable buttonsTable;

	public string currentBtnName;

	private List<Action<BtnCategory>> _actions;

	public List<Action<BtnCategory>> actions
	{
		get
		{
			if (_actions == null)
			{
				_actions = new List<Action<BtnCategory>>();
			}
			return _actions;
		}
	}

	private void Start()
	{
		buttonsTable.Reposition();
		BtnCategory[] array = buttons;
		foreach (BtnCategory obj in array)
		{
			obj.scaleMultypler = scaleMultypler;
			obj.animTime = animTime;
		}
	}

	public void BtnClicked(string btnName, bool instant = false)
	{
		buttons.ForEach(delegate(BtnCategory b)
		{
			b.isDefault = b.btnName == btnName;
			b.isPressed = b.isDefault;
		});
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].btnName == btnName)
			{
				buttons[i].isPressed = true;
				buttons[i].wasPressed = true;
				StartCoroutine(buttons[i].SetButtonPressed(true, instant));
				currentBtnName = btnName;
			}
			else
			{
				buttons[i].isPressed = false;
				StartCoroutine(buttons[i].SetButtonPressed(false));
			}
		}
		StartCoroutine(AnimateButtons());
	}

	private IEnumerator AnimateButtons()
	{
		float animationTimer = animTime;
		while (animationTimer > 0f)
		{
			animationTimer -= Time.unscaledDeltaTime;
			buttonsTable.Reposition();
			yield return null;
		}
		if (currentBtnName != null)
		{
			BtnCategory currentButton = buttons.FirstOrDefault((BtnCategory btn) => btn.btnName == currentBtnName);
			while (currentButton != null && currentButton.IsAnimationPlayed)
			{
				yield return null;
				buttonsTable.Reposition();
			}
		}
	}
}
