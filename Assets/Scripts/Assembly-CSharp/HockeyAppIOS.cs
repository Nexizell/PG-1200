using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HockeyAppIOS : MonoBehaviour
{
	public enum AuthenticatorType
	{
		Anonymous = 0,
		Device = 1,
		HockeyAppUser = 2,
		HockeyAppEmail = 3,
		WebAuth = 4
	}

	[CompilerGenerated]
	internal sealed class _003CSendLogs_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public HockeyAppIOS _003C_003E4__this;

		public List<string> logs;

		private string _003Curl_003E5__1;

		private WWW _003Cwww_003E5__2;

		private string _003Clog_003E5__3;

		private List<string>.Enumerator _003C_003E7__wrap1;

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
		public _003CSendLogs_003Ed__20(int _003C_003E1__state)
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
					string text = "api/2/apps/[APPID]/crashes/upload";
					_003Curl_003E5__1 = _003C_003E4__this.GetBaseURL() + text.Replace("[APPID]", _003C_003E4__this.appID);
					_003C_003E7__wrap1 = logs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					if (string.IsNullOrEmpty(_003Cwww_003E5__2.error))
					{
						try
						{
							File.Delete(_003Clog_003E5__3);
						}
						catch (Exception ex)
						{
							if (UnityEngine.Debug.isDebugBuild)
							{
								UnityEngine.Debug.Log("Failed to delete exception log: " + ex);
							}
						}
					}
					_003Cwww_003E5__2 = null;
					_003Clog_003E5__3 = null;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Clog_003E5__3 = _003C_003E7__wrap1.Current;
					WWWForm wWWForm = _003C_003E4__this.CreateForm(_003Clog_003E5__3);
					string text2 = wWWForm.headers["Content-Type"].ToString();
					text2 = text2.Replace("\"", "");
					Dictionary<string, string> headers = new Dictionary<string, string> { { "Content-Type", text2 } };
					_003Cwww_003E5__2 = new WWW(_003Curl_003E5__1, wWWForm.data, headers);
					_003C_003E2__current = _003Cwww_003E5__2;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<string>.Enumerator);
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

	protected const string HOCKEYAPP_BASEURL = "https://rink.hockeyapp.net/";

	protected const string HOCKEYAPP_CRASHESPATH = "api/2/apps/[APPID]/crashes/upload";

	protected const string LOG_FILE_DIR = "/logs/";

	protected const int MAX_CHARS = 199800;

	[Header("HockeyApp Setup")]
	public string appID = "your-hockey-app-id";

	public string serverURL = "your-custom-server-url";

	[Header("Authentication")]
	public AuthenticatorType authenticatorType;

	public string secret = "your-hockey-app-secret";

	[Header("Crashes & Exceptions")]
	public bool autoUploadCrashes;

	public bool exceptionLogging = true;

	[Header("Metrics")]
	public bool userMetrics = true;

	[Header("Version Updates")]
	public bool updateAlert = true;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void GameViewLoaded(string message)
	{
	}

	protected virtual List<string> GetLogHeaders()
	{
		return new List<string>();
	}

	protected virtual WWWForm CreateForm(string log)
	{
		return new WWWForm();
	}

	protected virtual List<string> GetLogFiles()
	{
		return new List<string>();
	}

	protected virtual IEnumerator SendLogs(List<string> logs)
	{
		string text = "api/2/apps/[APPID]/crashes/upload";
		string url = GetBaseURL() + text.Replace("[APPID]", appID);
		foreach (string log in logs)
		{
			WWWForm wWWForm = CreateForm(log);
			string text2 = wWWForm.headers["Content-Type"].ToString();
			text2 = text2.Replace("\"", "");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Content-Type", text2);
			WWW www = new WWW(url, wWWForm.data, dictionary);
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				continue;
			}
			try
			{
				File.Delete(log);
			}
			catch (Exception ex)
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log("Failed to delete exception log: " + ex);
				}
			}
		}
	}

	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
	}

	protected virtual string GetBaseURL()
	{
		return "";
	}

	protected virtual string GetAuthenticatorTypeString()
	{
		return "";
	}

	protected virtual bool IsConnected()
	{
		return false;
	}

	protected virtual void HandleException(string logString, string stackTrace)
	{
	}

	public void OnHandleLogCallback(string logString, string stackTrace, LogType type)
	{
	}
}
