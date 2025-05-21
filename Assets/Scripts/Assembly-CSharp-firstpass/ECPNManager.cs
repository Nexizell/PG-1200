using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ECPNManager : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStoreDeviceID_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ECPNManager _003C_003E4__this;

		public string rID;

		public string os;

		private WWW _003Cw_003E5__1;

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
		public _003CStoreDeviceID_003Ed__10(int _003C_003E1__state)
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
				_003C_003E4__this.devToken = rID;
				WWWForm wWWForm = new WWWForm();
				wWWForm.AddField("user", SystemInfo.deviceUniqueIdentifier);
				wWWForm.AddField("OS", os);
				wWWForm.AddField("regID", _003C_003E4__this.devToken);
				_003Cw_003E5__1 = new WWW(_003C_003E4__this.phpFilesLocation + "/RegisterDeviceIDtoDB.php", wWWForm);
				_003C_003E2__current = _003Cw_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003Cw_003E5__1.error == null)
				{
					string text = _003Cw_003E5__1.text;
					_003Cw_003E5__1.Dispose();
					int.Parse(text);
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
	internal sealed class _003CSendECPNmessage_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ECPNManager _003C_003E4__this;

		private WWW _003Cw_003E5__1;

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
		public _003CSendECPNmessage_003Ed__11(int _003C_003E1__state)
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
				WWWForm wWWForm = new WWWForm();
				wWWForm.AddField("user", SystemInfo.deviceUniqueIdentifier);
				_003Cw_003E5__1 = new WWW(_003C_003E4__this.phpFilesLocation + "/SendECPNmessageAll.php", wWWForm);
				_003C_003E2__current = _003Cw_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003Cw_003E5__1.error != null)
				{
					UnityEngine.Debug.Log("Error while sending message to all: " + _003Cw_003E5__1.error);
				}
				else
				{
					UnityEngine.Debug.Log(_003Cw_003E5__1.text);
					_003Cw_003E5__1.Dispose();
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
	internal sealed class _003CDeleteDeviceFromServer_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string rID;

		public ECPNManager _003C_003E4__this;

		private WWW _003Cw_003E5__1;

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
		public _003CDeleteDeviceFromServer_003Ed__12(int _003C_003E1__state)
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
				WWWForm wWWForm = new WWWForm();
				wWWForm.AddField("regID", rID);
				_003Cw_003E5__1 = new WWW(_003C_003E4__this.phpFilesLocation + "/UnregisterDeviceIDfromDB.php", wWWForm);
				_003C_003E2__current = _003Cw_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003Cw_003E5__1.error == null)
				{
					string text = _003Cw_003E5__1.text;
					_003Cw_003E5__1.Dispose();
					int.Parse(text);
					_003C_003E4__this.devToken = "";
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

	public string GoogleCloudMessageProjectID = "339873998127";

	public string phpFilesLocation = "https://secure.pixelgunserver.com/ecpn";

	public string packageName = "com.pixel.gun3d";

	private string devToken;

	public void RequestDeviceToken()
	{
	}

	public void RequestUnregisterDevice()
	{
	}

	public void SendMessageToAll()
	{
		StartCoroutine(SendECPNmessage());
	}

	public string GetDevToken()
	{
		return devToken;
	}

	public void RegisterAndroidDevice(string rID)
	{
		UnityEngine.Debug.Log("DeviceToken: " + rID);
		StartCoroutine(StoreDeviceID(rID, "android"));
	}

	public void UnregisterDevice(string rID)
	{
		UnityEngine.Debug.Log("Unregister DeviceToken: " + rID);
		StartCoroutine(DeleteDeviceFromServer(rID));
	}

	private IEnumerator StoreDeviceID(string rID, string os)
	{
		devToken = rID;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("user", SystemInfo.deviceUniqueIdentifier);
		wWWForm.AddField("OS", os);
		wWWForm.AddField("regID", devToken);
		WWW w = new WWW(phpFilesLocation + "/RegisterDeviceIDtoDB.php", wWWForm);
		yield return w;
		if (w.error == null)
		{
			string text = w.text;
			w.Dispose();
			int.Parse(text);
		}
	}

	private IEnumerator SendECPNmessage()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("user", SystemInfo.deviceUniqueIdentifier);
		WWW w = new WWW(phpFilesLocation + "/SendECPNmessageAll.php", wWWForm);
		yield return w;
		if (w.error != null)
		{
			UnityEngine.Debug.Log("Error while sending message to all: " + w.error);
			yield break;
		}
		UnityEngine.Debug.Log(w.text);
		w.Dispose();
	}

	private IEnumerator DeleteDeviceFromServer(string rID)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("regID", rID);
		WWW w = new WWW(phpFilesLocation + "/UnregisterDeviceIDfromDB.php", wWWForm);
		yield return w;
		if (w.error == null)
		{
			string text = w.text;
			w.Dispose();
			int.Parse(text);
			devToken = "";
		}
	}
}
