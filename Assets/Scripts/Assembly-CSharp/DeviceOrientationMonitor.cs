using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class DeviceOrientationMonitor : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CCheckForChange_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private WaitForRealSeconds _003Cdelay_003E5__1;

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
		public _003CCheckForChange_003Ed__11(int _003C_003E1__state)
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
				CurrentOrientation = Input.deviceOrientation;
				_003Cdelay_003E5__1 = new WaitForRealSeconds(CheckDelay);
			}
			DeviceOrientation deviceOrientation = Input.deviceOrientation;
			if ((deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight) && CurrentOrientation != Input.deviceOrientation)
			{
				CurrentOrientation = Input.deviceOrientation;
				DeviceOrientationMonitor.OnOrientationChange(CurrentOrientation);
			}
			_003C_003E2__current = _003Cdelay_003E5__1;
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

	public static float CheckDelay;

	public static DeviceOrientation CurrentOrientation { get; private set; }

	public static event Action<DeviceOrientation> OnOrientationChange;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		StartCoroutine(CheckForChange());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator CheckForChange()
	{
		CurrentOrientation = Input.deviceOrientation;
		WaitForRealSeconds delay = new WaitForRealSeconds(CheckDelay);
		while (true)
		{
			DeviceOrientation deviceOrientation = Input.deviceOrientation;
			if ((deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight) && CurrentOrientation != Input.deviceOrientation)
			{
				CurrentOrientation = Input.deviceOrientation;
				DeviceOrientationMonitor.OnOrientationChange(CurrentOrientation);
			}
			yield return delay;
		}
	}

	static DeviceOrientationMonitor()
	{
		DeviceOrientationMonitor.OnOrientationChange = delegate
		{
		};
		CheckDelay = 0.5f;
	}
}
