using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class GameServicesController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWaitAndIncrementBeginnerAchievementCoroutine_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003CnewGamesStartedCount_003E5__1;

		private float _003Cstep_003E5__2;

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
		public _003CWaitAndIncrementBeginnerAchievementCoroutine_003Ed__3(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				int @int = PlayerPrefs.GetInt("GamesStartedCount", 0);
				_003CnewGamesStartedCount_003E5__1 = @int + 1;
				PlayerPrefs.SetInt("GamesStartedCount", _003CnewGamesStartedCount_003E5__1);
				_003Cstep_003E5__2 = 20f;
				RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
				if (buildTargetPlatform != RuntimePlatform.IPhonePlayer)
				{
					if (buildTargetPlatform != RuntimePlatform.Android)
					{
						break;
					}
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
					{
						UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
						goto IL_00b5;
					}
					if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
					{
					}
					break;
				}
				UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
				goto IL_017a;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b5;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_017a;
				}
				IL_00b5:
				if (!Social.localUser.authenticated)
				{
					_003C_003E2__current = new WaitForSeconds(2f);
					_003C_003E1__state = 1;
					return true;
				}
				UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
				UnityEngine.Debug.Log("Trying to increment Beginner achievement...");
				GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQBg", 1, delegate(bool success)
				{
					UnityEngine.Debug.Log("Achievement Beginner incremented: " + success);
				});
				break;
				IL_017a:
				if (!Social.localUser.authenticated)
				{
					_003C_003E2__current = new WaitForSeconds(2f);
					_003C_003E1__state = 2;
					return true;
				}
				UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
				UnityEngine.Debug.Log(string.Format("Trying to report {0} achievement...", new object[1] { "beginner_id" }));
				Social.ReportProgress("beginner_id", (float)_003CnewGamesStartedCount_003E5__1 * _003Cstep_003E5__2, delegate(bool success)
				{
					UnityEngine.Debug.Log(string.Format("Achievement {0} incremented: {1}", new object[2] { "beginner_id", success }));
				});
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

	private static GameServicesController _instance;

	public static GameServicesController Instance
	{
		get
		{
			return _instance;
		}
	}

	public void WaitAuthenticationAndIncrementBeginnerAchievement()
	{
		using (new StopwatchLogger("WaitAuthenticationAndIncrementBeginnerAchievement()"))
		{
			StartCoroutine(WaitAndIncrementBeginnerAchievementCoroutine());
		}
	}

	private static IEnumerator WaitAndIncrementBeginnerAchievementCoroutine()
	{
		int @int = PlayerPrefs.GetInt("GamesStartedCount", 0);
		int newGamesStartedCount = @int + 1;
		PlayerPrefs.SetInt("GamesStartedCount", newGamesStartedCount);
		float step = 20f;
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.Android:
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
				while (!Social.localUser.authenticated)
				{
					yield return new WaitForSeconds(2f);
				}
				UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
				UnityEngine.Debug.Log("Trying to increment Beginner achievement...");
				GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQBg", 1, delegate(bool success)
				{
					UnityEngine.Debug.Log("Achievement Beginner incremented: " + success);
				});
			}
			else if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
			while (!Social.localUser.authenticated)
			{
				yield return new WaitForSeconds(2f);
			}
			UnityEngine.Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
			UnityEngine.Debug.Log(string.Format("Trying to report {0} achievement...", new object[1] { "beginner_id" }));
			Social.ReportProgress("beginner_id", (float)newGamesStartedCount * step, delegate(bool success)
			{
				UnityEngine.Debug.Log(string.Format("Achievement {0} incremented: {1}", new object[2] { "beginner_id", success }));
			});
			break;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			UnityEngine.Debug.LogWarning(GetType().Name + " already exists.");
		}
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
