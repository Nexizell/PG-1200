using UnityEngine;

namespace Rilisoft
{
	public class GuiElementHandler : MonoBehaviour
	{
		[SerializeField]
		protected internal PrefabHandler _prefabHandler;

		[SerializeField]
		protected internal string _showOnScene;

		[ReadOnly]
		[SerializeField]
		protected internal GuiElementBase _element;

		private void Start()
		{
			if (_showOnScene.IsNotEmpty())
			{
				Singleton<SceneLoader>.Instance.OnSceneLoading += OnSceneLoading;
				Singleton<SceneLoader>.Instance.OnSceneLoaded += OnSceneLoaded;
				if (SceneLoader.ActiveSceneName == _showOnScene)
				{
					ShowElement();
				}
			}
		}

		private void OnSceneLoaded(SceneLoadInfo sceneLoadInfo)
		{
			if (SceneLoader.ActiveSceneName == _showOnScene)
			{
				ShowElement();
			}
		}

		private void OnSceneLoading(SceneLoadInfo sceneLoadInfo)
		{
			if (_showOnScene.IsNotEmpty() && _showOnScene != sceneLoadInfo.SceneName && _element != null)
			{
				Object.Destroy(_element);
			}
		}

		private void OnDestroy()
		{
			Singleton<SceneLoader>.Instance.OnSceneLoaded -= OnSceneLoaded;
			if (_element != null)
			{
				_element.PopRequest();
			}
		}

		[ContextMenu("UpdateView")]
		public virtual void ShowElement()
		{
			if (_element != null)
			{
				_element.PushRequest();
			}
			else if (_prefabHandler.Prefab != null && _prefabHandler.Prefab.ContainsComponent<GuiElementBase>())
			{
				GameObject gameObject = Object.Instantiate(_prefabHandler.Prefab);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				_element = gameObject.GetComponent<GuiElementBase>();
				_element.PushRequest();
			}
		}

		public virtual void HideElement()
		{
			_element.PopRequest();
		}
	}
}
