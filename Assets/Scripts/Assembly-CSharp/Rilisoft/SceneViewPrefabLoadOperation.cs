using System;
using UnityEngine;

namespace Rilisoft
{
	public class SceneViewPrefabLoadOperation : MonoBehaviour
	{
		private ResourceRequest _request;

		public LobbyItem LoadingItem { get; private set; }

		public Func<LobbyItem, bool> BreakChecker { get; private set; }

		public Action<LobbyItem, GameObject> OnPrefabLoadedAction { get; private set; }

		public bool IsLoading
		{
			get
			{
				if (_request != null)
				{
					return !_request.isDone;
				}
				return false;
			}
		}

		public bool LoadComplete { get; private set; }

		public bool IsBreaking { get; private set; }

		private void Awake()
		{
			base.enabled = false;
		}

		public void Setup(LobbyItem loadingItem, Func<LobbyItem, bool> breakChecker, Action<LobbyItem, GameObject> onPrefabLoadedAction)
		{
			LoadingItem = loadingItem;
			OnPrefabLoadedAction = onPrefabLoadedAction;
			BreakChecker = breakChecker;
			if (loadingItem != null)
			{
				_request = Resources.LoadAsync<GameObject>(LoadingItem.PrefabPath);
				base.enabled = true;
			}
			else
			{
				Break();
			}
		}

		public void Update()
		{
			if (_request == null || LoadingItem == null || OnPrefabLoadedAction == null)
			{
				return;
			}
			if (IsBreaking)
			{
				base.enabled = false;
				return;
			}
			if (BreakChecker != null && BreakChecker(LoadingItem))
			{
				Break();
			}
			if (_request != null && _request.isDone)
			{
				LoadComplete = true;
				GameObject arg = _request.asset as GameObject;
				OnPrefabLoadedAction(LoadingItem, arg);
				base.enabled = false;
			}
		}

		public void Break()
		{
			IsBreaking = true;
			base.enabled = false;
		}
	}
}
