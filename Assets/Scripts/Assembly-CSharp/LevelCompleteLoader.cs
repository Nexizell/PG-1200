using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelCompleteLoader : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CloadNext_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CloadNext_003Ed__5(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(0.25f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				SceneManager.LoadScene(sceneName);
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

	public static Action action = null;

	public static string sceneName = "";

	private Texture fon;

	public UICamera myUICam;

	private Texture loadingNote;

	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = true;
		if (!sceneName.Equals("LevelComplete"))
		{
			fon = ConnectScene.MainLoadingTexture();
		}
		else
		{
			string path = "LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/LevelComplete_back";
			if (GameConnect.isSurvival)
			{
				path = "GameOver_Coliseum";
			}
			if (GameConnect.isSpeedrun)
			{
				string b = "Loading_Speedrun";
				path = ResPath.Combine(Switcher.LoadingInResourcesPath + (Device.isRetinaAndStrong ? "/Hi" : string.Empty), b);
			}
			fon = Resources.Load<Texture>(path);
		}
		UITexture uITexture = new GameObject().AddComponent<UITexture>();
		uITexture.mainTexture = fon;
		uITexture.SetRect(0f, 0f, 1366f, 768f);
		uITexture.transform.SetParent(myUICam.transform, false);
		uITexture.transform.localScale = Vector3.one;
		uITexture.transform.localPosition = Vector3.zero;
		StartCoroutine(loadNext());
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	private IEnumerator loadNext()
	{
		yield return new WaitForSeconds(0.25f);
		SceneManager.LoadScene(sceneName);
	}
}
