using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class MyUIInput : UIInput
{
	[CompilerGenerated]
	internal sealed class _003CReSelect_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MyUIInput _003C_003E4__this;

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
		public _003CReSelect_003Ed__19(int _003C_003E1__state)
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
				_003C_003E4__this.DeselectInput();
				_003C_003E2__current = new WaitForRealSeconds(0.3f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.isSelected = true;
				return false;
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

	[NonSerialized]
	public float heightKeyboard;

	public Action onKeyboardInter;

	public Action onKeyboardCancel;

	public Action onKeyboardVisible;

	public Action onKeyboardHide;

	private bool isKeyboardVisible;

	private float timerlog = 0.3f;

	private bool _selectAfterPause;

	private void Awake()
	{
		hideInput = false;
	}

	protected override void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			OnSelectEvent();
		}
		else if (!hideInput)
		{
			OnDeselectEvent();
		}
	}

	public void DeselectInput()
	{
		OnDeselectEventCustom();
	}

	protected void OnDeselectEventCustom()
	{
		if (mDoInit)
		{
			Init();
		}
		if (UIInput.mKeyboard != null)
		{
			UIInput.mKeyboard.active = false;
			UIInput.mKeyboard = null;
		}
		if (label != null)
		{
			mValue = base.value;
			if (string.IsNullOrEmpty(mValue))
			{
				label.text = mDefaultText;
				label.color = mDefaultColor;
			}
			else
			{
				label.text = mValue;
			}
			Input.imeCompositionMode = IMECompositionMode.Auto;
			label.alignment = mAlignment;
		}
		base.isSelected = false;
		UIInput.selection = null;
		UpdateLabel();
	}

	private new void Update()
	{
		if (Application.isEditor)
		{
			if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown("enter")) && onKeyboardInter != null)
			{
				onKeyboardInter();
			}
			if (Input.GetKeyDown(KeyCode.KeypadPlus) && onKeyboardVisible != null)
			{
				onKeyboardVisible();
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				if (onKeyboardHide != null)
				{
					onKeyboardHide();
				}
				DeselectInput();
			}
		}
		base.Update();
	}

	public float GetKeyboardHeight()
	{
		return heightKeyboard;
	}

	private void SetKeyboardHeight()
	{
		heightKeyboard = TouchScreenKeyboard.area.height;
	}

	private void OnDestroy()
	{
		base.OnSelect(false);
		DeselectInput();
	}

	private void OnEnable()
	{
		DeviceOrientationMonitor.OnOrientationChange += OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		DeviceOrientationMonitor.OnOrientationChange -= OnDeviceOrientationChanged;
		base.OnSelect(false);
		DeselectInput();
		base.Cleanup();
	}

	private void OnDeviceOrientationChanged(DeviceOrientation ori)
	{
		if (base.isSelected)
		{
			StartCoroutine(ReSelect());
		}
	}

	private IEnumerator ReSelect()
	{
		DeselectInput();
		yield return new WaitForRealSeconds(0.3f);
		isSelected = true;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			_selectAfterPause = base.isSelected;
			if (base.isSelected)
			{
				base.isSelected = false;
			}
		}
		else
		{
			if (_selectAfterPause)
			{
				base.isSelected = true;
			}
			_selectAfterPause = false;
		}
	}
}
