using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class LogHandler : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CRegisterLogCallbackCoroutine_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LogHandler _003C_003E4__this;

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
		public _003CRegisterLogCallbackCoroutine_003Ed__3(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(0.5f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (!_003C_003E4__this._cancelled)
				{
					Application.RegisterLogCallback(_003C_003E4__this.HandleLog);
					_003C_003E4__this._registered = true;
				}
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

	private bool _cancelled;

	private bool _registered;

	private string _logString = string.Empty;

	private string _stackTrace = string.Empty;

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnEnable()
	{
		StartCoroutine(RegisterLogCallbackCoroutine());
	}

	private void OnDisable()
	{
		_cancelled = true;
		if (_registered)
		{
			Application.RegisterLogCallback(null);
		}
	}

	private IEnumerator RegisterLogCallbackCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		if (!_cancelled)
		{
			Application.RegisterLogCallback(HandleLog);
			_registered = true;
		}
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (LogType.Exception == type)
		{
			_logString = logString;
			_stackTrace = stackTrace;
		}
	}
}
