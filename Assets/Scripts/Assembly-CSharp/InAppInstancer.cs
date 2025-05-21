using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public sealed class InAppInstancer : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InAppInstancer _003C_003E4__this;

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
		public _003CStart_003Ed__1(int _003C_003E1__state)
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
				if (Launcher.UsingNewLauncher)
				{
					return false;
				}
				if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
				{
					UnityEngine.Object.Instantiate(_003C_003E4__this.inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				if (!_003C_003E4__this._amazonGamecircleManagerInitialized)
				{
					_003C_003E4__this.StartCoroutine(_003C_003E4__this.InitializeAmazonGamecircleManager());
					_003C_003E4__this._amazonGamecircleManagerInitialized = true;
				}
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
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

	[CompilerGenerated]
	internal sealed class _003CInitializeAmazonGamecircleManager_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InAppInstancer _003C_003E4__this;

		private ScopeLogger _003C_003E7__wrap1;

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
		public _003CInitializeAmazonGamecircleManager_003Ed__2(int _003C_003E1__state)
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
				{
					_003C_003E1__state = -1;
					string callee = string.Format(CultureInfo.InvariantCulture, "{0}.InitializeAmazonGamecircleManager()", _003C_003E4__this.GetType().Name);
					_003C_003E7__wrap1 = new ScopeLogger(callee, true);
					_003C_003E1__state = -3;
					UnityEngine.Object.DontDestroyOnLoad(new GameObject("Rilisoft.AmazonGameCircleManager", typeof(GameCircleManager)));
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -3;
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(ScopeLogger);
					return false;
				}
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

	public GameObject inAppGameObjectPrefab;

	private bool _amazonGamecircleManagerInitialized;

	private bool _amazonIapManagerInitialized;

	private string _leaderboardId = string.Empty;

	private IEnumerator Start()
	{
		if (Launcher.UsingNewLauncher)
		{
			yield break;
		}
		if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
		{
			UnityEngine.Object.Instantiate(inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
			yield return null;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (!_amazonGamecircleManagerInitialized)
			{
				StartCoroutine(InitializeAmazonGamecircleManager());
				_amazonGamecircleManagerInitialized = true;
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		}
	}

	private IEnumerator InitializeAmazonGamecircleManager()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.InitializeAmazonGamecircleManager()", GetType().Name);
		using (new ScopeLogger(callee, true))
		{
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("Rilisoft.AmazonGameCircleManager", typeof(GameCircleManager)));
			yield return null;
		}
	}
}
