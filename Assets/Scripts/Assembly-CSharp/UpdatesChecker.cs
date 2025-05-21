using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class UpdatesChecker : MonoBehaviour
{
	internal enum Store
	{
		Ios = 0,
		Play = 1,
		Wp8 = 2,
		Amazon = 3,
		Unknown = 4
	}

	[CompilerGenerated]
	internal sealed class _003CCheckUpdatesCoroutine_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Store store;

		private WWW _003Crequest_003E5__1;

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
		public _003CCheckUpdatesCoroutine_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string text2;
			WWWForm wWWForm;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 2:
				{
					_003C_003E1__state = -1;
					if (!string.IsNullOrEmpty(_003Crequest_003E5__1.error))
					{
						UnityEngine.Debug.LogWarningFormat("Error while receiving version: {0}", _003Crequest_003E5__1.error);
						return false;
					}
					string text = URLs.Sanitize(_003Crequest_003E5__1);
					if (string.IsNullOrEmpty(text))
					{
						UnityEngine.Debug.Log("response is empty");
						return false;
					}
					if (Application.isEditor)
					{
						UnityEngine.Debug.Log("UpdatesChecker: " + text);
					}
					if (text.Equals("no"))
					{
						GlobalGameController.NewVersionAvailable = true;
						UnityEngine.Debug.Log("NewVersionAvailable: true");
					}
					return false;
				}
				IL_003b:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				text2 = string.Format("{0}:{1}", new object[2]
				{
					(int)store,
					GlobalGameController.AppVersion
				});
				if (Application.isEditor)
				{
					UnityEngine.Debug.LogFormat("Sending version: {0}", text2);
				}
				wWWForm = new WWWForm();
				wWWForm.AddField("action", "check_shop_version");
				wWWForm.AddField("app_version", text2);
				_003Crequest_003E5__1 = Tools.CreateWwwIfNotConnected("https://pixelgunserver.com/~rilisoft/action.php", wWWForm);
				if (_003Crequest_003E5__1 == null)
				{
					return false;
				}
				_003C_003E2__current = _003Crequest_003E5__1;
				_003C_003E1__state = 2;
				return true;
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

	private Store _currentStore;

	private const string ActionAddress = "https://pixelgunserver.com/~rilisoft/action.php";

	private IEnumerator CheckUpdatesCoroutine(Store store)
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		string text = string.Format("{0}:{1}", new object[2]
		{
			(int)store,
			GlobalGameController.AppVersion
		});
		if (Application.isEditor)
		{
			UnityEngine.Debug.LogFormat("Sending version: {0}", text);
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "check_shop_version");
		wWWForm.AddField("app_version", text);
		WWW request = Tools.CreateWwwIfNotConnected("https://pixelgunserver.com/~rilisoft/action.php", wWWForm);
		if (request == null)
		{
			yield break;
		}
		yield return request;
		if (!string.IsNullOrEmpty(request.error))
		{
			UnityEngine.Debug.LogWarningFormat("Error while receiving version: {0}", request.error);
			yield break;
		}
		string text2 = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(text2))
		{
			UnityEngine.Debug.Log("response is empty");
			yield break;
		}
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("UpdatesChecker: " + text2);
		}
		if (text2.Equals("no"))
		{
			GlobalGameController.NewVersionAvailable = true;
			UnityEngine.Debug.Log("NewVersionAvailable: true");
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_currentStore = Store.Unknown;
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			_currentStore = Store.Ios;
			break;
		case RuntimePlatform.Android:
			switch (Defs.AndroidEdition)
			{
			case Defs.RuntimeAndroidEdition.GoogleLite:
				_currentStore = Store.Play;
				break;
			case Defs.RuntimeAndroidEdition.Amazon:
				_currentStore = Store.Amazon;
				break;
			}
			break;
		case RuntimePlatform.MetroPlayerX64:
			_currentStore = Store.Wp8;
			break;
		}
	}

	private void Start()
	{
		StartCoroutine(CheckUpdatesCoroutine(_currentStore));
	}

	private void OnApplicationPause(bool pause)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(">>> UpdatesChecker.OnApplicationPause()");
		}
		if (!pause)
		{
			StartCoroutine(CheckUpdatesCoroutine(_currentStore));
		}
	}
}
