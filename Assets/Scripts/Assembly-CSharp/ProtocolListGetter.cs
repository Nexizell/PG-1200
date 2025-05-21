using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class ProtocolListGetter : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ProtocolListGetter _003C_003E4__this;

		private WWWForm _003Cform_003E5__1;

		private WaitForSeconds _003CwaitForSeconds_003E5__2;

		private WWW _003Cdownload_003E5__3;

		private string _003Cresponse_003E5__4;

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
		public _003CStart_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string value;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				UnityEngine.Object.DontDestroyOnLoad(_003C_003E4__this.gameObject);
				if (!Storager.hasKey(_003C_003E4__this.CurrentVersionSupportedKey))
				{
					Storager.setInt(_003C_003E4__this.CurrentVersionSupportedKey, 1);
				}
				goto IL_007a;
			case 1:
				_003C_003E1__state = -1;
				goto IL_007a;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00fc;
			case 3:
				_003C_003E1__state = -1;
				_003Cresponse_003E5__4 = URLs.Sanitize(_003Cdownload_003E5__3);
				if (string.IsNullOrEmpty(_003Cdownload_003E5__3.error) && !string.IsNullOrEmpty(_003Cresponse_003E5__4) && UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log(_003Cresponse_003E5__4);
				}
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__3.error))
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogWarning("ProtocolListGetter error: " + _003Cdownload_003E5__3.error);
					}
					_003C_003E2__current = _003CwaitForSeconds_003E5__2;
					_003C_003E1__state = 4;
					return true;
				}
				if (string.IsNullOrEmpty(_003Cdownload_003E5__3.error) && !string.IsNullOrEmpty(_003Cresponse_003E5__4))
				{
					if ("no".Equals(_003Cresponse_003E5__4))
					{
						currentVersionIsSupported = false;
						Storager.setInt(_003C_003E4__this.CurrentVersionSupportedKey, 0);
					}
					else
					{
						currentVersionIsSupported = true;
						Storager.setInt(_003C_003E4__this.CurrentVersionSupportedKey, 1);
					}
					return false;
				}
				_003C_003E2__current = _003CwaitForSeconds_003E5__2;
				_003C_003E1__state = 5;
				return true;
			case 4:
				_003C_003E1__state = -1;
				goto IL_00fc;
			case 5:
				{
					_003C_003E1__state = -1;
					_003Cdownload_003E5__3 = null;
					_003Cresponse_003E5__4 = null;
					goto IL_00fc;
				}
				IL_00fc:
				_003Cdownload_003E5__3 = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, _003Cform_003E5__1);
				if (_003Cdownload_003E5__3 == null)
				{
					_003C_003E2__current = _003CwaitForSeconds_003E5__2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = _003Cdownload_003E5__3;
				_003C_003E1__state = 3;
				return true;
				IL_007a:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				currentVersionIsSupported = Storager.getInt(_003C_003E4__this.CurrentVersionSupportedKey) == 1;
				_003CwaitForSeconds_003E5__2 = new WaitForSeconds(10f);
				value = CurrentPlatform + ":" + GlobalGameController.AppVersion;
				_003Cform_003E5__1 = new WWWForm();
				_003Cform_003E5__1.AddField("action", "check_version");
				_003Cform_003E5__1.AddField("app_version", value);
				goto IL_00fc;
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

	public static bool currentVersionIsSupported = true;

	private string CurrentVersionSupportedKey = "CurrentVersionSupportedKey" + GlobalGameController.AppVersion;

	public static int CurrentPlatform
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return 0;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return 1;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return 3;
			}
			return 101;
		}
	}

	private IEnumerator Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!Storager.hasKey(CurrentVersionSupportedKey))
		{
			Storager.setInt(CurrentVersionSupportedKey, 1);
		}
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		currentVersionIsSupported = Storager.getInt(CurrentVersionSupportedKey) == 1;
		WaitForSeconds waitForSeconds = new WaitForSeconds(10f);
		string value = CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "check_version");
		form.AddField("app_version", value);
		string response;
		while (true)
		{
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form);
			if (download == null)
			{
				yield return waitForSeconds;
				continue;
			}
			yield return download;
			response = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log(response);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("ProtocolListGetter error: " + download.error);
				}
				yield return waitForSeconds;
			}
			else
			{
				if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response))
				{
					break;
				}
				yield return waitForSeconds;
			}
		}
		if ("no".Equals(response))
		{
			currentVersionIsSupported = false;
			Storager.setInt(CurrentVersionSupportedKey, 0);
		}
		else
		{
			currentVersionIsSupported = true;
			Storager.setInt(CurrentVersionSupportedKey, 1);
		}
	}
}
