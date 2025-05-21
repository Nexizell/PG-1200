using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWrapCoroutineCore_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public IEnumerator routine;

		public TaskCompletionSource<bool> promise;

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
		public _003CWrapCoroutineCore_003Ed__15(int _003C_003E1__state)
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
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (routine.MoveNext())
			{
				_003C_003E2__current = routine.Current;
				_003C_003E1__state = 1;
				return true;
			}
			promise.SetResult(true);
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

	[CompilerGenerated]
	internal sealed class _003CWaitForSeconds_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003CWaitForSeconds_003Ed__16(int _003C_003E1__state)
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

	[CompilerGenerated]
	internal sealed class _003CWaitForSecondsActionEveryNFrames_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003Ci_003E5__1;

		public int everyNFrames;

		public Action action;

		private float _003CstartTime_003E5__2;

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
		public _003CWaitForSecondsActionEveryNFrames_003Ed__17(int _003C_003E1__state)
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
				_003Ci_003E5__1++;
				if (_003Ci_003E5__1 % everyNFrames == 0 && action != null)
				{
					action();
				}
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__2 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__2 = Time.realtimeSinceStartup;
				_003Ci_003E5__1 = 0;
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

	[CompilerGenerated]
	internal sealed class _003CWaitUntilCoroutine_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Func<bool> func;

		public Action onActiveAction;

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
		public _003CWaitUntilCoroutine_003Ed__19(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitUntil(func);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (onActiveAction != null)
				{
					onActiveAction();
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

	[CompilerGenerated]
	internal sealed class _003CDeferredActionCoroutine_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float runAfterSecs;

		public Action act;

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
		public _003CDeferredActionCoroutine_003Ed__21(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForRealSeconds(runAfterSecs);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (act != null)
				{
					act();
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

	private static CoroutineRunner _instance;

	public static CoroutineRunner Instance
	{
		get
		{
			if (_instance == null)
			{
				try
				{
					GameObject obj = new GameObject("CoroutineRunner");
					_instance = obj.AddComponent<CoroutineRunner>();
					UnityEngine.Object.DontDestroyOnLoad(obj);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("[Rilisoft] CoroutineRunner: Instance exception: " + ex);
				}
			}
			return _instance;
		}
	}

	public static event Action OnEngineUpdate;

	public static event Action<bool> OnEngineApplicationPause;

	public static event Action OnEngineApplicationQuit;

	private void Update()
	{
		if (CoroutineRunner.OnEngineUpdate != null)
		{
			CoroutineRunner.OnEngineUpdate();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (CoroutineRunner.OnEngineApplicationPause != null)
		{
			CoroutineRunner.OnEngineApplicationPause(pause);
		}
	}

	private void OnApplicationQuit()
	{
		if (CoroutineRunner.OnEngineApplicationQuit != null)
		{
			CoroutineRunner.OnEngineApplicationQuit();
		}
	}

	internal IEnumerator WrapCoroutine(IEnumerator routine, TaskCompletionSource<bool> promise)
	{
		if (routine == null)
		{
			throw new ArgumentNullException("routine");
		}
		if (promise == null)
		{
			throw new ArgumentNullException("promise");
		}
		return WrapCoroutineCore(routine, promise);
	}

	private IEnumerator WrapCoroutineCore(IEnumerator routine, TaskCompletionSource<bool> promise)
	{
		while (routine.MoveNext())
		{
			yield return routine.Current;
		}
		promise.SetResult(true);
	}

	public static IEnumerator WaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	public static IEnumerator WaitForSecondsActionEveryNFrames(float tm, Action action, int everyNFrames)
	{
		float startTime = Time.realtimeSinceStartup;
		int i = 0;
		do
		{
			yield return null;
			i++;
			if (i % everyNFrames == 0 && action != null)
			{
				action();
			}
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	public static void WaitUntil(Func<bool> func, Action onActiveAction)
	{
		Instance.StartCoroutine(Instance.WaitUntilCoroutine(func, onActiveAction));
	}

	private IEnumerator WaitUntilCoroutine(Func<bool> func, Action onActiveAction)
	{
		yield return new WaitUntil(func);
		if (onActiveAction != null)
		{
			onActiveAction();
		}
	}

	public static void DeferredAction(float runAfterSecs, Action act)
	{
		Instance.StartCoroutine(Instance.DeferredActionCoroutine(runAfterSecs, act));
	}

	private IEnumerator DeferredActionCoroutine(float runAfterSecs, Action act)
	{
		yield return new WaitForRealSeconds(runAfterSecs);
		if (act != null)
		{
			act();
		}
	}
}
