using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Rilisoft;
using UnityEngine;

public sealed class RemotePushNotificationController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CReciveUpdateDataToServer_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RemotePushNotificationController _003C_003E4__this;

		public string deviceToken;

		private bool _003CfriendsControllerIsNotInitialized_003E5__1;

		private WWW _003Crequest_003E5__2;

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
		public _003CReciveUpdateDataToServer_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
			case 2:
			case 3:
			case 4:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			case -2:
			case -1:
			case 0:
				break;
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
				{
					_003C_003E1__state = -1;
					string callee = string.Format("{0}.ReciveUpdateDataToServer('{1}')", new object[2]
					{
						_003C_003E4__this.GetType().Name,
						deviceToken
					});
					_003C_003E7__wrap1 = new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor);
					_003C_003E1__state = -3;
					if (_003C_003E4__this._isResponceRuning)
					{
						result = false;
						break;
					}
					_003C_003E4__this._isResponceRuning = true;
					_003CfriendsControllerIsNotInitialized_003E5__1 = FriendsController.sharedController == null;
					if (Defs.IsDeveloperBuild && FriendsController.sharedController == null)
					{
						UnityEngine.Debug.Log("Waiting FriendsController being initialized...");
					}
					goto IL_00f0;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_00f0;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0122;
				case 3:
					_003C_003E1__state = -3;
					goto IL_015e;
				case 4:
					{
						_003C_003E1__state = -3;
						try
						{
							if (!string.IsNullOrEmpty(_003Crequest_003E5__2.error))
							{
								if (Defs.IsDeveloperBuild)
								{
									UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): error = " + _003Crequest_003E5__2.error);
								}
								result = false;
								break;
							}
							if (!string.IsNullOrEmpty(_003Crequest_003E5__2.text))
							{
								if (Defs.IsDeveloperBuild)
								{
									UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): request.text = " + _003Crequest_003E5__2.text);
								}
								if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
								{
									string text = JsonUtility.ToJson(new RemotePushRegistrationMemento(deviceToken, DateTime.UtcNow, GlobalGameController.AppVersion));
									if (Defs.IsDeveloperBuild)
									{
										UnityEngine.Debug.LogFormat("Saving remote push registration: '{0}'", text);
									}
									PlayerPrefs.SetString("RemotePushRegistration", text);
								}
							}
							goto IL_0582;
						}
						finally
						{
							_003C_003E4__this._isResponceRuning = false;
						}
					}
					IL_0582:
					_003Crequest_003E5__2 = null;
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(ScopeLogger);
					result = false;
					goto end_IL_0000;
					IL_015e:
					if (string.IsNullOrEmpty(FriendsController.sharedController.id))
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						result = true;
					}
					else
					{
						_003C_003E4__this._isStartUpdateRecive = true;
						WWWForm wWWForm = new WWWForm();
						string text2 = string.Format("{0}:{1}", new object[2]
						{
							ProtocolListGetter.CurrentPlatform,
							GlobalGameController.AppVersion
						});
						string id = FriendsController.sharedController.id;
						string currentLanguageCode = LocalizationStore.GetCurrentLanguageCode();
						string text3 = Storager.getInt("PayingUser").ToString();
						string text4 = PlayerPrefs.GetString("Last Payment Time", string.Empty);
						if (string.IsNullOrEmpty(text4))
						{
							text4 = "None";
						}
						string text5 = DateTimeOffset.Now.Offset.Hours.ToString();
						string text6 = Storager.getInt("Coins").ToString();
						string text7 = Storager.getInt("GemsCurrency").ToString();
						string text8 = ExperienceController.GetCurrentLevel().ToString();
						wWWForm.AddField("app_version", text2);
						wWWForm.AddField("device_token", deviceToken);
						wWWForm.AddField("uniq_id", id);
						wWWForm.AddField("is_paying", text3);
						wWWForm.AddField("last_payment_date", text4);
						wWWForm.AddField("utc_shift", text5);
						wWWForm.AddField("coins", text6);
						wWWForm.AddField("gems", text7);
						wWWForm.AddField("level", text8);
						wWWForm.AddField("language", currentLanguageCode);
						wWWForm.AddField("allow_invites", Defs.isEnableRemoteInviteFromFriends ? 1 : 0);
						int num = 0;
						if (Application.platform == RuntimePlatform.Android)
						{
							try
							{
								num = AndroidSystem.GetSdkVersion();
							}
							catch (Exception exception)
							{
								UnityEngine.Debug.LogException(exception);
							}
						}
						if (Application.platform == RuntimePlatform.Android)
						{
							wWWForm.AddField("os", num);
						}
						else
						{
							wWWForm.AddField("os", SystemInfo.operatingSystem);
						}
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): form data");
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.AppendLine("app_version: " + text2);
							stringBuilder.AppendLine("device_token: " + deviceToken);
							stringBuilder.AppendLine("uniq_id: " + id);
							stringBuilder.AppendLine("is_paying: " + text3);
							stringBuilder.AppendLine("last_payment_date: " + text4);
							stringBuilder.AppendLine("utc_shift: " + text5);
							stringBuilder.AppendLine("coins: " + text6);
							stringBuilder.AppendLine("gems: " + text7);
							stringBuilder.AppendLine("level: " + text8);
							stringBuilder.AppendLine("language: " + currentLanguageCode);
							stringBuilder.AppendLine("androidSdkLevel: " + num);
							UnityEngine.Debug.Log(stringBuilder.ToString());
						}
						Dictionary<string, string> headers = new Dictionary<string, string> { 
						{
							"Authorization",
							FriendsController.HashForPush(wWWForm.data)
						} };
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("Trying to send device token to server: " + deviceToken);
						}
						_003Crequest_003E5__2 = Tools.CreateWwwIf(true, _003C_003E4__this.UrlPushNotificationServer, wWWForm, "RemotePushNotificationController.ReciveUpdateDataToServer()", headers);
						if (_003Crequest_003E5__2 == null)
						{
							result = false;
							break;
						}
						_003C_003E2__current = _003Crequest_003E5__2;
						_003C_003E1__state = 4;
						result = true;
					}
					goto end_IL_0000;
					IL_0122:
					if (Defs.IsDeveloperBuild && FriendsController.sharedController.id == null)
					{
						UnityEngine.Debug.Log("Waiting FriendsController.id being initialized...");
					}
					goto IL_015e;
					IL_00f0:
					if (FriendsController.sharedController == null)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						if (!_003CfriendsControllerIsNotInitialized_003E5__1)
						{
							goto IL_0122;
						}
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						result = true;
					}
					goto end_IL_0000;
				}
				_003C_003Em__Finally1();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
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

	public static RemotePushNotificationController Instance;

	private bool _isResponceRuning;

	private bool _isStartUpdateRecive;

	private const string RemotePushRegistrationKey = "RemotePushRegistration";

	private string UrlPushNotificationServer
	{
		get
		{
			return "https://secure.pixelgunserver.com/push_service";
		}
	}

	public void UpdateDataOnServer()
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento = LoadRemotePushRegistrationMemento();
		if (!string.IsNullOrEmpty(remotePushRegistrationMemento.RegistrationId))
		{
			StartCoroutine(ReciveUpdateDataToServer(remotePushRegistrationMemento.RegistrationId));
		}
	}

	private static RemotePushRegistrationMemento ParseRemotePushRegistrationMemento(string remotePushRegistrationJson)
	{
		try
		{
			return JsonUtility.FromJson<RemotePushRegistrationMemento>(remotePushRegistrationJson);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
			return new RemotePushRegistrationMemento(string.Empty, DateTime.MinValue, string.Empty);
		}
	}

	private static RemotePushRegistrationMemento LoadRemotePushRegistrationMemento()
	{
		return ParseRemotePushRegistrationMemento(PlayerPrefs.GetString("RemotePushRegistration", "{}"));
	}

	private static bool IsDeviceRegistred()
	{
		return !string.IsNullOrEmpty(GetRemotePushNotificationToken());
	}

	private static bool CheckIfExpired(RemotePushRegistrationMemento remotePushRegistrationMemento)
	{
		DateTime result;
		if (DateTime.TryParse(remotePushRegistrationMemento.RegistrationTime, out result) && DateTime.UtcNow - result < TimeSpan.FromDays(2.0))
		{
			return false;
		}
		return true;
	}

	internal static string GetRemotePushNotificationToken()
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento = LoadRemotePushRegistrationMemento();
		if (CheckIfExpired(remotePushRegistrationMemento))
		{
			return string.Empty;
		}
		return remotePushRegistrationMemento.RegistrationId;
	}

	private IEnumerator ReciveUpdateDataToServer(string deviceToken)
	{
		string callee = string.Format("{0}.ReciveUpdateDataToServer('{1}')", new object[2]
		{
			GetType().Name,
			deviceToken
		});
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
		{
			if (_isResponceRuning)
			{
				yield break;
			}
			_isResponceRuning = true;
			bool friendsControllerIsNotInitialized = FriendsController.sharedController == null;
			if (Defs.IsDeveloperBuild && FriendsController.sharedController == null)
			{
				UnityEngine.Debug.Log("Waiting FriendsController being initialized...");
			}
			while (FriendsController.sharedController == null)
			{
				yield return null;
			}
			if (friendsControllerIsNotInitialized)
			{
				yield return null;
			}
			if (Defs.IsDeveloperBuild && FriendsController.sharedController.id == null)
			{
				UnityEngine.Debug.Log("Waiting FriendsController.id being initialized...");
			}
			while (string.IsNullOrEmpty(FriendsController.sharedController.id))
			{
				yield return null;
			}
			_isStartUpdateRecive = true;
			WWWForm wWWForm = new WWWForm();
			string text = string.Format("{0}:{1}", new object[2]
			{
				ProtocolListGetter.CurrentPlatform,
				GlobalGameController.AppVersion
			});
			string id = FriendsController.sharedController.id;
			string currentLanguageCode = LocalizationStore.GetCurrentLanguageCode();
			string text2 = Storager.getInt("PayingUser").ToString();
			string text3 = PlayerPrefs.GetString("Last Payment Time", string.Empty);
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "None";
			}
			string text4 = DateTimeOffset.Now.Offset.Hours.ToString();
			string text5 = Storager.getInt("Coins").ToString();
			string text6 = Storager.getInt("GemsCurrency").ToString();
			string text7 = ExperienceController.GetCurrentLevel().ToString();
			wWWForm.AddField("app_version", text);
			wWWForm.AddField("device_token", deviceToken);
			wWWForm.AddField("uniq_id", id);
			wWWForm.AddField("is_paying", text2);
			wWWForm.AddField("last_payment_date", text3);
			wWWForm.AddField("utc_shift", text4);
			wWWForm.AddField("coins", text5);
			wWWForm.AddField("gems", text6);
			wWWForm.AddField("level", text7);
			wWWForm.AddField("language", currentLanguageCode);
			wWWForm.AddField("allow_invites", Defs.isEnableRemoteInviteFromFriends ? 1 : 0);
			int num = 0;
			if (Application.platform == RuntimePlatform.Android)
			{
				try
				{
					num = AndroidSystem.GetSdkVersion();
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				wWWForm.AddField("os", num);
			}
			else
			{
				wWWForm.AddField("os", SystemInfo.operatingSystem);
			}
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): form data");
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("app_version: " + text);
				stringBuilder.AppendLine("device_token: " + deviceToken);
				stringBuilder.AppendLine("uniq_id: " + id);
				stringBuilder.AppendLine("is_paying: " + text2);
				stringBuilder.AppendLine("last_payment_date: " + text3);
				stringBuilder.AppendLine("utc_shift: " + text4);
				stringBuilder.AppendLine("coins: " + text5);
				stringBuilder.AppendLine("gems: " + text6);
				stringBuilder.AppendLine("level: " + text7);
				stringBuilder.AppendLine("language: " + currentLanguageCode);
				stringBuilder.AppendLine("androidSdkLevel: " + num);
				UnityEngine.Debug.Log(stringBuilder.ToString());
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Authorization", FriendsController.HashForPush(wWWForm.data));
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Trying to send device token to server: " + deviceToken);
			}
			WWW request = Tools.CreateWwwIf(true, UrlPushNotificationServer, wWWForm, "RemotePushNotificationController.ReciveUpdateDataToServer()", dictionary);
			if (request == null)
			{
				yield break;
			}
			yield return request;
			try
			{
				if (!string.IsNullOrEmpty(request.error))
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): error = " + request.error);
					}
					yield break;
				}
				if (string.IsNullOrEmpty(request.text))
				{
					yield break;
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): request.text = " + request.text);
				}
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
				{
					string text8 = JsonUtility.ToJson(new RemotePushRegistrationMemento(deviceToken, DateTime.UtcNow, GlobalGameController.AppVersion));
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("Saving remote push registration: '{0}'", text8);
					}
					PlayerPrefs.SetString("RemotePushRegistration", text8);
				}
			}
			finally
			{
				_isResponceRuning = false;
			}
		}
	}
}
