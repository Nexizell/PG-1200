using UnityEngine;

namespace Rilisoft
{
	public class GuiElement : GuiElementBase
	{
		[SerializeField]
		protected internal string _showOnScene;

		public override bool IsVisible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		protected override void WhenPush()
		{
			base.gameObject.SetActiveSafe(true);
		}

		protected override void WhenPop()
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActiveSafe(false);
			}
			Dispose();
		}

		protected override void Start()
		{
			if (_showOnScene.IsNotEmpty())
			{
				Singleton<SceneLoader>.Instance.OnSceneLoading += OnSceneLoading;
				Singleton<SceneLoader>.Instance.OnSceneLoaded += OnSceneLoaded;
				if (SceneLoader.ActiveSceneName == _showOnScene)
				{
					PushRequest();
				}
			}
		}

		private void OnSceneLoaded(SceneLoadInfo sceneLoadInfo)
		{
			if (SceneLoader.ActiveSceneName == _showOnScene)
			{
				PushRequest();
			}
		}

		private void OnSceneLoading(SceneLoadInfo sceneLoadInfo)
		{
			if (_showOnScene.IsNotEmpty() && _showOnScene != sceneLoadInfo.SceneName && base.gameObject != null)
			{
				Object.Destroy(base.gameObject);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Singleton<SceneLoader>.Instance.OnSceneLoaded -= OnSceneLoaded;
		}
	}
}
