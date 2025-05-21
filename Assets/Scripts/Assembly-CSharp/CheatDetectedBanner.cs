using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatDetectedBanner : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSendCheatTypeOnServer_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private WWW _003Cdownload_003E5__1;

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
		public _003CSendCheatTypeOnServer_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			WWWForm wWWForm;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0026;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0026;
			case 2:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__1.error))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003Cdownload_003E5__1 = null;
				goto IL_0026;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_0026;
				}
				IL_0026:
				wWWForm = new WWWForm();
				wWWForm.AddField("action", "update_abuse_info");
				wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
				wWWForm.AddField("uniq_id", accID);
				wWWForm.AddField("auth", FriendsController.Hash("update_abuse_info", accID));
				wWWForm.AddField("block_id", Storager.getInt("HackDetected"));
				_003Cdownload_003E5__1 = Tools.CreateWww(FriendsController.actionAddress, wWWForm);
				if (_003Cdownload_003E5__1 == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = _003Cdownload_003E5__1;
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

	public const string HackDetectedKey = "HackDetected";

	public UITexture txFon;

	public UIButton exitButton;

	private static string accID = "";

	private bool skipFrame;

	private bool progressCleared;

	public static void ShowAndClearProgress()
	{
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("Cheat");
	}

	private static void ClearAllProgress()
	{
		accID = Storager.getString("AccountCreated");
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		bool isEditor = Application.isEditor;
		Storager.setInt("HackDetected", 1);
		Storager.setString("AccountCreated", accID);
		PlayerPrefs.Save();
		CoroutineRunner.Instance.StartCoroutine(CloudSyncController.Instance.ApplyChanges(true));
		CoroutineRunner.Instance.StartCoroutine(SendCheatTypeOnServer());
	}

	private void Awake()
	{
		RemoveObjects();
		txFon.mainTexture = ConnectScene.MainLoadingTexture();
		exitButton.onClick.Add(new EventDelegate(OnExitButtonClick));
	}

	private void Update()
	{
		if (!skipFrame)
		{
			skipFrame = true;
		}
		else if (!progressCleared)
		{
			progressCleared = true;
			ClearAllProgress();
		}
	}

	private void RemoveObjects()
	{
		MonoBehaviour[] array = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].transform.root.Equals(base.transform.root))
			{
				UnityEngine.Object.Destroy(array[i].gameObject);
			}
		}
	}

	private void OnExitButtonClick()
	{
		Application.Quit();
	}

	public static IEnumerator SendCheatTypeOnServer()
	{
		while (true)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("action", "update_abuse_info");
			wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			wWWForm.AddField("uniq_id", accID);
			wWWForm.AddField("auth", FriendsController.Hash("update_abuse_info", accID));
			wWWForm.AddField("block_id", Storager.getInt("HackDetected"));
			WWW download = Tools.CreateWww(FriendsController.actionAddress, wWWForm);
			if (download == null)
			{
				yield return null;
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				yield return null;
			}
		}
	}
}
