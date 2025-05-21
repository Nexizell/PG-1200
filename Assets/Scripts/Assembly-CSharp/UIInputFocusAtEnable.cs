using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(UIInput))]
public class UIInputFocusAtEnable : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetSelected_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIInputFocusAtEnable _003C_003E4__this;

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
		public _003CSetSelected_003Ed__6(int _003C_003E1__state)
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
				_003C_003E4__this._input.isSelected = false;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this._input.isSelected = true;
				break;
			}
			if (!_003C_003E4__this._input.isSelected)
			{
				_003C_003E2__current = new WaitForRealSeconds(0.3f);
				_003C_003E1__state = 2;
				return true;
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

	private const float FOCUS_DELAY = 0.3f;

	[Tooltip("Применить только один раз")]
	[SerializeField]
	protected internal bool _onlyOnce;

	[ReadOnly]
	[SerializeField]
	protected internal UIInput _input;

	private bool _alreadyTurned;

	private void Awake()
	{
		_input = GetComponent<UIInput>();
		if (_input == null)
		{
			UnityEngine.Debug.LogError("input not found");
		}
	}

	private void OnEnable()
	{
		if (!_onlyOnce || !_alreadyTurned)
		{
			StartCoroutine(SetSelected());
			_alreadyTurned = true;
		}
	}

	private IEnumerator SetSelected()
	{
		_input.isSelected = false;
		yield return null;
		while (!_input.isSelected)
		{
			yield return new WaitForRealSeconds(0.3f);
			_input.isSelected = true;
		}
	}
}
