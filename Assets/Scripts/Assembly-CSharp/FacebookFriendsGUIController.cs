using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookFriendsGUIController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CRepos_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIGrid grid;

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
		public _003CRepos_003Ed__5(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				grid.Reposition();
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

	public static FacebookFriendsGUIController sharedController;

	public bool _infoRequested;

	private void Start()
	{
		sharedController = this;
	}

	private void Update()
	{
		if (FacebookController.FacebookSupported && FacebookController.sharedController.friendsList != null && FacebookController.sharedController.friendsList.Count != 0 && FriendsController.sharedController.facebookFriendsInfo.Count == 0 && !_infoRequested && FriendsController.sharedController.GetFacebookFriendsCallback == null)
		{
			FriendsController.sharedController.GetFacebookFriendsInfo(GetFacebookFriendsCallback);
			_infoRequested = true;
		}
	}

	private void GetFacebookFriendsCallback()
	{
		if (FriendsController.sharedController == null || FriendsController.sharedController.facebookFriendsInfo == null)
		{
			return;
		}
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		foreach (string key in FriendsController.sharedController.facebookFriendsInfo.Keys)
		{
			bool flag = false;
			if (FriendsController.sharedController.friends != null)
			{
				foreach (string friend in FriendsController.sharedController.friends)
				{
					if (friend.Equals(key))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				dictionary.Add(key, FriendsController.sharedController.facebookFriendsInfo[key]);
			}
		}
		UIGrid componentInChildren = GetComponentInChildren<UIGrid>();
		if (componentInChildren == null)
		{
			return;
		}
		FriendPreview[] array = GetComponentsInChildren<FriendPreview>(true);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		Dictionary<string, FriendPreview> dictionary2 = new Dictionary<string, FriendPreview>();
		FriendPreview[] array2 = array;
		foreach (FriendPreview friendPreview in array2)
		{
			if (friendPreview.id != null && dictionary.ContainsKey(friendPreview.id))
			{
				dictionary2.Add(friendPreview.id, friendPreview);
				continue;
			}
			friendPreview.transform.parent = null;
			UnityEngine.Object.Destroy(friendPreview.gameObject);
		}
		foreach (KeyValuePair<string, Dictionary<string, object>> item in dictionary)
		{
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> item2 in item.Value)
			{
				dictionary3.Add(item2.Key, item2.Value as string);
			}
			if (dictionary2.ContainsKey(item.Key))
			{
				GameObject gameObject = dictionary2[item.Key].gameObject;
				gameObject.GetComponent<FriendPreview>().facebookFriend = true;
				gameObject.GetComponent<FriendPreview>().id = item.Key;
				if (item.Value.ContainsKey("nick"))
				{
					gameObject.GetComponent<FriendPreview>().nm.text = item.Value["nick"] as string;
				}
				if (item.Value.ContainsKey("rank"))
				{
					string text = item.Value["rank"] as string;
					if (text.Equals("0"))
					{
						text = "1";
					}
					gameObject.GetComponent<FriendPreview>().rank.spriteName = "Rank_" + text;
				}
				if (item.Value.ContainsKey("skin"))
				{
					gameObject.GetComponent<FriendPreview>().SetSkin(item.Value["skin"] as string);
				}
				gameObject.GetComponent<FriendPreview>().FillClanAttrs(dictionary3);
				continue;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Friend") as GameObject);
			gameObject2.transform.parent = componentInChildren.transform;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.GetComponent<FriendPreview>().facebookFriend = true;
			gameObject2.GetComponent<FriendPreview>().id = item.Key;
			if (item.Value.ContainsKey("nick"))
			{
				gameObject2.GetComponent<FriendPreview>().nm.text = item.Value["nick"] as string;
			}
			if (item.Value.ContainsKey("rank"))
			{
				string text2 = item.Value["rank"] as string;
				if (text2.Equals("0"))
				{
					text2 = "1";
				}
				gameObject2.GetComponent<FriendPreview>().rank.spriteName = "Rank_" + text2;
			}
			if (item.Value.ContainsKey("skin"))
			{
				gameObject2.GetComponent<FriendPreview>().SetSkin(item.Value["skin"] as string);
			}
			gameObject2.GetComponent<FriendPreview>().FillClanAttrs(dictionary3);
		}
		StartCoroutine(Repos(componentInChildren));
	}

	private IEnumerator Repos(UIGrid grid)
	{
		yield return null;
		grid.Reposition();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			FriendsController.sharedController.facebookFriendsInfo.Clear();
			_infoRequested = false;
		}
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.GetFacebookFriendsCallback = null;
		sharedController = null;
	}
}
