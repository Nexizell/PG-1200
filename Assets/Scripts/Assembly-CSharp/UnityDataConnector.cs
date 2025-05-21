using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEngine;

public class UnityDataConnector : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CGetData_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UnityDataConnector _003C_003E4__this;

		private float _003CelapsedTime_003E5__1;

		private WWW _003Cwww_003E5__2;

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
		public _003CGetData_003Ed__19(int _003C_003E1__state)
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
				string text = _003C_003E4__this.webServiceUrl + "?ssid=" + _003C_003E4__this.spreadsheetId + "&sheet=" + _003C_003E4__this.worksheetName + "&pass=" + _003C_003E4__this.password + "&action=GetData";
				if (_003C_003E4__this.debugMode)
				{
					UnityEngine.Debug.Log("Connecting to webservice on " + text);
				}
				_003Cwww_003E5__2 = new WWW(text);
				_003CelapsedTime_003E5__1 = 0f;
				_003C_003E4__this.currentStatus = "Stablishing Connection... ";
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003Cwww_003E5__2.isDone)
			{
				_003CelapsedTime_003E5__1 += Time.deltaTime;
				if (!(_003CelapsedTime_003E5__1 >= _003C_003E4__this.maxWaitTime))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.currentStatus = "Max wait time reached, connection aborted.";
				UnityEngine.Debug.Log(_003C_003E4__this.currentStatus);
				_003C_003E4__this.updating = false;
			}
			if (!_003Cwww_003E5__2.isDone || !string.IsNullOrEmpty(_003Cwww_003E5__2.error))
			{
				_003C_003E4__this.currentStatus = "Connection error after" + _003CelapsedTime_003E5__1 + "seconds: " + _003Cwww_003E5__2.error;
				UnityEngine.Debug.LogError(_003C_003E4__this.currentStatus);
				_003C_003E4__this.updating = false;
				return false;
			}
			string text2 = _003Cwww_003E5__2.text;
			UnityEngine.Debug.Log(_003CelapsedTime_003E5__1 + " : " + text2);
			_003C_003E4__this.currentStatus = "Connection stablished, parsing data...";
			if (text2 == "\"Incorrect Password.\"")
			{
				_003C_003E4__this.currentStatus = "Connection error: Incorrect Password.";
				UnityEngine.Debug.LogError(_003C_003E4__this.currentStatus);
				_003C_003E4__this.updating = false;
				return false;
			}
			try
			{
				_003C_003E4__this.ssObjects = JsonMapper.ToObject<JsonData[]>(text2);
			}
			catch
			{
				_003C_003E4__this.currentStatus = "Data error: could not parse retrieved data as json.";
				UnityEngine.Debug.LogError(_003C_003E4__this.currentStatus);
				_003C_003E4__this.updating = false;
				return false;
			}
			_003C_003E4__this.currentStatus = "Data Successfully Retrieved!";
			_003C_003E4__this.updating = false;
			_003C_003E4__this.dataDestinationObject.SendMessage("DoSomethingWithTheData", _003C_003E4__this.ssObjects);
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
	internal sealed class _003CSendData_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UnityDataConnector _003C_003E4__this;

		public string ballName;

		public float collisionMagnitude;

		private float _003CelapsedTime_003E5__1;

		private WWW _003Cwww_003E5__2;

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
		public _003CSendData_003Ed__21(int _003C_003E1__state)
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
				if (!_003C_003E4__this.saveToGS)
				{
					return false;
				}
				string text = _003C_003E4__this.webServiceUrl + "?ssid=" + _003C_003E4__this.spreadsheetId + "&sheet=" + _003C_003E4__this.statisticsWorksheetName + "&pass=" + _003C_003E4__this.password + "&val1=" + ballName + "&val2=" + collisionMagnitude + "&action=SetData";
				if (_003C_003E4__this.debugMode)
				{
					UnityEngine.Debug.Log("Connection String: " + text);
				}
				_003Cwww_003E5__2 = new WWW(text);
				_003CelapsedTime_003E5__1 = 0f;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003Cwww_003E5__2.isDone)
			{
				_003CelapsedTime_003E5__1 += Time.deltaTime;
				if (!(_003CelapsedTime_003E5__1 >= _003C_003E4__this.maxWaitTime))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
			}
			if (!_003Cwww_003E5__2.isDone || !string.IsNullOrEmpty(_003Cwww_003E5__2.error))
			{
				return false;
			}
			string text2 = _003Cwww_003E5__2.text;
			if (text2.Contains("Incorrect Password"))
			{
				return false;
			}
			text2.Contains("RCVD OK");
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

	public string webServiceUrl = "";

	public string spreadsheetId = "";

	public string worksheetName = "";

	public string password = "";

	public float maxWaitTime = 10f;

	public GameObject dataDestinationObject;

	public string statisticsWorksheetName = "Statistics";

	public bool debugMode;

	private bool updating;

	private string currentStatus;

	private JsonData[] ssObjects;

	private bool saveToGS;

	private Rect guiBoxRect;

	private Rect guiButtonRect;

	private Rect guiButtonRect2;

	private Rect guiButtonRect3;

	private void Start()
	{
		updating = false;
		currentStatus = "Offline";
		saveToGS = false;
		guiBoxRect = new Rect(10f, 10f, 310f, 140f);
		guiButtonRect = new Rect(30f, 40f, 270f, 30f);
		guiButtonRect2 = new Rect(30f, 75f, 270f, 30f);
		guiButtonRect3 = new Rect(30f, 110f, 270f, 30f);
	}

	private void OnGUI()
	{
		GUI.Box(guiBoxRect, currentStatus);
		if (GUI.Button(guiButtonRect, "Update From Google Spreadsheet"))
		{
			Connect();
		}
		saveToGS = GUI.Toggle(guiButtonRect2, saveToGS, "Save Stats To Google Spreadsheet");
		if (GUI.Button(guiButtonRect3, "Reset Balls values"))
		{
			dataDestinationObject.SendMessage("ResetBalls");
		}
	}

	private void Connect()
	{
		if (!updating)
		{
			updating = true;
			StartCoroutine(GetData());
		}
	}

	private IEnumerator GetData()
	{
		string text = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + worksheetName + "&pass=" + password + "&action=GetData";
		if (debugMode)
		{
			UnityEngine.Debug.Log("Connecting to webservice on " + text);
		}
		WWW www = new WWW(text);
		float elapsedTime = 0f;
		currentStatus = "Stablishing Connection... ";
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= maxWaitTime)
			{
				currentStatus = "Max wait time reached, connection aborted.";
				UnityEngine.Debug.Log(currentStatus);
				updating = false;
				break;
			}
			yield return null;
		}
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime + "seconds: " + www.error;
			UnityEngine.Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}
		string text2 = www.text;
		UnityEngine.Debug.Log(elapsedTime + " : " + text2);
		currentStatus = "Connection stablished, parsing data...";
		if (text2 == "\"Incorrect Password.\"")
		{
			currentStatus = "Connection error: Incorrect Password.";
			UnityEngine.Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}
		try
		{
			ssObjects = JsonMapper.ToObject<JsonData[]>(text2);
		}
		catch
		{
			currentStatus = "Data error: could not parse retrieved data as json.";
			UnityEngine.Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}
		currentStatus = "Data Successfully Retrieved!";
		updating = false;
		dataDestinationObject.SendMessage("DoSomethingWithTheData", ssObjects);
	}

	public void SaveDataOnTheCloud(string ballName, float collisionMagnitude)
	{
		if (saveToGS)
		{
			StartCoroutine(SendData(ballName, collisionMagnitude));
		}
	}

	private IEnumerator SendData(string ballName, float collisionMagnitude)
	{
		if (!saveToGS)
		{
			yield break;
		}
		string text = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + statisticsWorksheetName + "&pass=" + password + "&val1=" + ballName + "&val2=" + collisionMagnitude + "&action=SetData";
		if (debugMode)
		{
			UnityEngine.Debug.Log("Connection String: " + text);
		}
		WWW www = new WWW(text);
		float elapsedTime = 0f;
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= maxWaitTime)
			{
				break;
			}
			yield return null;
		}
		if (www.isDone && string.IsNullOrEmpty(www.error))
		{
			string text2 = www.text;
			if (!text2.Contains("Incorrect Password"))
			{
				text2.Contains("RCVD OK");
			}
		}
	}
}
