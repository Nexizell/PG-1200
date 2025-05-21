using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class ChatController : MonoBehaviour
{
	public struct PrivateMessage
	{
		public string playerIDFrom;

		public string message;

		public double timeStamp;

		public bool isRead;

		public bool isSending;

		public PrivateMessage(string _playerIDFrom, string _message, double _timeStamp, bool _isSending, bool _isRead)
		{
			playerIDFrom = _playerIDFrom;
			message = _message;
			timeStamp = _timeStamp;
			isSending = _isSending;
			isRead = _isRead;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CMyWaitForSeconds_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public float tm;

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
		public _003CMyWaitForSeconds_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__1 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
			}
			_003C_003E2__current = null;
			_003C_003E1__state = 1;
			return true;
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

	public static ChatController sharedController = null;

	public static int countNewPrivateMessage = 0;

	private static string privateMessageKey = "PrivateMessageKey";

	private static string privateMessageSendKey = "PrivateMessageSendKey";

	public static float timerToUpdateMessage;

	public static bool fastSendMessage;

	public static float maxTimerToUpdateMessage = 10f;

	public static Dictionary<string, List<PrivateMessage>> privateMessages = new Dictionary<string, List<PrivateMessage>>();

	public static Dictionary<string, List<PrivateMessage>> privateMessagesForSend = new Dictionary<string, List<PrivateMessage>>();

	private Action _backButtonCallback;

	public static void FillPrivatMessageFromPrefs()
	{
		privateMessages.Clear();
		int num = 0;
		string @string = PlayerPrefs.GetString(privateMessageKey, "{}");
		Dictionary<string, object> dictionary = null;
		dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null && dictionary.Count > 0)
		{
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				foreach (object item2 in item.Value as List<object>)
				{
					Dictionary<string, object> dictionary2 = item2 as Dictionary<string, object>;
					string playerIDFrom = dictionary2["playerIDFrom"] as string;
					string message = dictionary2["message"] as string;
					double timeStamp = Tools.CurrentUnixTime;
					double result;
					if (double.TryParse(dictionary2["timeStamp"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
					{
						timeStamp = result;
					}
					else
					{
						UnityEngine.Debug.LogWarning(string.Format("Could not parse timestamp {0}", new object[1] { item.Value }));
					}
					bool flag = bool.Parse(dictionary2["isRead"] as string);
					if (!flag)
					{
						num++;
					}
					bool isSending = bool.Parse(dictionary2["isSending"] as string);
					AddPrivateMessage(_message: new PrivateMessage(playerIDFrom, message, timeStamp, isSending, flag), _playerIdChating: item.Key);
				}
			}
		}
		countNewPrivateMessage = num;
	}

	public static void FillPrivateMessageForSendFromPrefs()
	{
		privateMessagesForSend.Clear();
		string @string = PlayerPrefs.GetString(privateMessageSendKey, "[]");
		List<object> list = null;
		list = Json.Deserialize(@string) as List<object>;
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (object item in list)
		{
			Dictionary<string, string> obj = item as Dictionary<string, string>;
			string text = obj["to"];
			string message = obj["text"];
			double timeStamp = double.Parse(obj["timeStamp"], NumberStyles.Number, CultureInfo.InvariantCulture);
			bool isSending = true;
			bool isRead = false;
			if (!privateMessagesForSend.ContainsKey(text))
			{
				privateMessagesForSend.Add(text, new List<PrivateMessage>());
			}
			privateMessagesForSend[text].Add(new PrivateMessage(text, message, timeStamp, isSending, isRead));
		}
	}

	public static void SavePrivatMessageInPrefs()
	{
		Dictionary<string, List<Dictionary<string, string>>> dictionary = new Dictionary<string, List<Dictionary<string, string>>>();
		foreach (KeyValuePair<string, List<PrivateMessage>> privateMessage in privateMessages)
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			foreach (PrivateMessage item in privateMessage.Value)
			{
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				dictionary2.Add("playerIDFrom", item.playerIDFrom);
				dictionary2.Add("message", item.message);
				double timeStamp = item.timeStamp;
				dictionary2.Add("timeStamp", timeStamp.ToString("F8", CultureInfo.InvariantCulture));
				bool isRead = item.isRead;
				dictionary2.Add("isRead", isRead.ToString());
				isRead = item.isSending;
				dictionary2.Add("isSending", isRead.ToString());
				list.Add(dictionary2);
			}
			dictionary.Add(privateMessage.Key, list);
		}
		string text = Json.Serialize(dictionary);
		PlayerPrefs.SetString(privateMessageKey, text ?? "{}");
		List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
		foreach (KeyValuePair<string, List<PrivateMessage>> item2 in privateMessagesForSend)
		{
			new List<Dictionary<string, string>>();
			foreach (PrivateMessage item3 in item2.Value)
			{
				Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
				dictionary3.Add("to", item2.Key);
				dictionary3.Add("text", item3.message);
				double timeStamp = item3.timeStamp;
				dictionary3.Add("timeStamp", timeStamp.ToString("F8", CultureInfo.InvariantCulture));
				list2.Add(dictionary3);
			}
		}
		string text2 = Json.Serialize(list2);
		PlayerPrefs.SetString(privateMessageSendKey, text2 ?? "[]");
		PlayerPrefs.Save();
	}

	public static string GetPrivateChatJsonForSend()
	{
		return PlayerPrefs.GetString(privateMessageSendKey, "[]");
	}

	public void ParseUpdateChatMessageResponse(string response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		bool flag = false;
		if (dictionary != null && dictionary.Count > 0)
		{
			if (dictionary.ContainsKey("user_messages_added"))
			{
				foreach (KeyValuePair<string, object> item in dictionary["user_messages_added"] as Dictionary<string, object>)
				{
					double result;
					if (!double.TryParse(item.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
					{
						UnityEngine.Debug.LogWarning(string.Format("Could not parse timestamp {0}    Current culture: {1}, current UI culture: {2}", new object[3]
						{
							item.Value,
							CultureInfo.CurrentCulture.Name,
							CultureInfo.CurrentUICulture.Name
						}));
						continue;
					}
					foreach (KeyValuePair<string, List<PrivateMessage>> item2 in privateMessagesForSend)
					{
						int num = -1;
						for (int i = 0; i < item2.Value.Count; i++)
						{
							PrivateMessage privateMessage = item2.Value[i];
							if (privateMessage.timeStamp == result)
							{
								num = i;
								double result2;
								if (double.TryParse(item.Value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out result2))
								{
									privateMessage.timeStamp = result2;
								}
								string key = item2.Key;
								PrivateMessage message = new PrivateMessage(FriendsController.sharedController.id, privateMessage.message, privateMessage.timeStamp, true, true);
								AddPrivateMessage(key, message);
								flag = true;
								break;
							}
						}
						if (num >= 0)
						{
							item2.Value.RemoveAt(num);
							break;
						}
					}
				}
			}
			if (dictionary.ContainsKey("user_messages"))
			{
				foreach (object item3 in dictionary["user_messages"] as List<object>)
				{
					foreach (KeyValuePair<string, object> item4 in item3 as Dictionary<string, object>)
					{
						Dictionary<string, object> obj = item4.Value as Dictionary<string, object>;
						string text = obj["from"] as string;
						string message2 = obj["text"] as string;
						double result3 = Tools.CurrentUnixTime;
						if (!double.TryParse(item4.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out result3))
						{
							UnityEngine.Debug.LogWarning(string.Format("Could not parse message body key {0};    full response: {1}", new object[2] { item4.Key, response }));
						}
						double timeStamp = result3;
						AddPrivateMessage(text, new PrivateMessage(text, message2, timeStamp, true, false));
						countNewPrivateMessage++;
						if (PrivateChatController.sharedController != null)
						{
							PrivateChatController.sharedController.UpdateFriendItemsInfoAndSort();
						}
						flag = true;
					}
				}
			}
			if (privateMessages != null)
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, List<PrivateMessage>> privateMessage2 in privateMessages)
				{
					if (FriendsController.sharedController.friends.Contains(privateMessage2.Key))
					{
						continue;
					}
					foreach (PrivateMessage item5 in privateMessage2.Value)
					{
						if (!item5.isRead)
						{
							countNewPrivateMessage--;
						}
					}
					list.Add(privateMessage2.Key);
					flag = true;
				}
				foreach (string item6 in list)
				{
					privateMessages.Remove(item6);
				}
			}
		}
		if (flag)
		{
			SavePrivatMessageInPrefs();
			if (PrivateChatController.sharedController != null)
			{
				PrivateChatController.sharedController.UpdateMessageForSelectedUsers();
			}
		}
	}

	public static void AddPrivateMessage(string _playerIdChating, PrivateMessage _message)
	{
		if (!privateMessages.ContainsKey(_playerIdChating))
		{
			privateMessages.Add(_playerIdChating, new List<PrivateMessage>());
		}
		privateMessages[_playerIdChating].Add(_message);
		while (privateMessages[_playerIdChating].Count > Defs.historyPrivateMessageLength)
		{
			if (!privateMessages[_playerIdChating][0].isRead)
			{
				countNewPrivateMessage--;
			}
			privateMessages[_playerIdChating].RemoveAt(0);
		}
	}

	private void Start()
	{
		sharedController = this;
		FillPrivatMessageFromPrefs();
	}

	private void OnDestroy()
	{
		sharedController = null;
		StopAllCoroutines();
	}

	public void SelectPrivateChatMode()
	{
	}

	public void BackButtonClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (_backButtonCallback != null)
		{
			_backButtonCallback();
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene);
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	public void Show(Action exitCallback)
	{
		base.gameObject.SetActive(true);
		_backButtonCallback = exitCallback;
	}
}
