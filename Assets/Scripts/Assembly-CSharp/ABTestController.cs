using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ABTestController : MonoBehaviour
{
	public enum ABTestCohortsType
	{
		NONE = 0,
		A = 1,
		B = 2,
		SKIP = 3
	}

	[CompilerGenerated]
	internal sealed class _003CUpdateConfigsAllABTests_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private List<ABTestBase>.Enumerator _003C_003E7__wrap1;

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
		public _003CUpdateConfigsAllABTests_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
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
					_003C_003E7__wrap1 = currentABTests.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003E7__wrap1.Current.UpdateABTestConfig();
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ABTestBase>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static List<ABTestBase> currentABTests = new List<ABTestBase>();

	public static bool useBuffSystem;

	public bool isRunABTest
	{
		get
		{
			return true;
		}
	}

	private void Start()
	{
		currentABTests.Add(new ABTestBankOneCurrency());
		if (TrainingController.TrainingCompleted)
		{
			SkipAllNotStartedTests();
		}
		InitAllABTests();
		StartCoroutine(UpdateConfigsAllABTests());
	}

	private void SkipAllNotStartedTests()
	{
		foreach (ABTestBase currentABTest in currentABTests)
		{
			if (currentABTest.cohort == ABTestCohortsType.NONE)
			{
				currentABTest.cohort = ABTestCohortsType.SKIP;
			}
		}
	}

	private void InitAllABTests()
	{
		foreach (ABTestBase currentABTest in currentABTests)
		{
			currentABTest.InitTest();
		}
	}

	private IEnumerator UpdateConfigsAllABTests()
	{
		foreach (ABTestBase currentABTest in currentABTests)
		{
			currentABTest.UpdateABTestConfig();
			yield return null;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		StartCoroutine(UpdateConfigsAllABTests());
	}
}
