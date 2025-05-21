using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class SceneLoader : Singleton<SceneLoader>
	{
		[CompilerGenerated]
		internal sealed class _003CWaitSceneIsLoaded_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AsyncOperation op;

			public SceneLoader _003C_003E4__this;

			public SceneLoadInfo loadInfo;

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
			public _003CWaitSceneIsLoaded_003Ed__17(int _003C_003E1__state)
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
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (!op.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E4__this.OnSceneLoaded != null)
				{
					_003C_003E4__this.OnSceneLoaded(loadInfo);
				}
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

		public const string SCENE_INFOS_ASSET_PATH = "Assets/Resources/ScenesList.asset";

		private ScenesList _scenesList;

		[SerializeField]
		private List<SceneLoadInfo> _loadingHistory = new List<SceneLoadInfo>();

		private ScenesList ScenesList
		{
			get
			{
				if (_scenesList == null)
				{
					_scenesList = Resources.Load<ScenesList>("ScenesList");
				}
				return _scenesList;
			}
		}

		public static string ActiveSceneName
		{
			get
			{
				return SceneManager.GetActiveScene().name ?? string.Empty;
			}
		}

		public event Action<SceneLoadInfo> OnSceneLoading;

		public event Action<SceneLoadInfo> OnSceneLoaded;

		private void OnInstanceCreated()
		{
			if (ScenesList == null)
			{
				throw new Exception("scenes list is null");
			}
			List<IGrouping<string, ExistsSceneInfo>> source = (from i in ScenesList.Infos
				group i by i.Name into g
				where g.Count() > 1
				select g).ToList();
			if (source.Any())
			{
				string text = source.Select((IGrouping<string, ExistsSceneInfo> g) => g.Key).Aggregate((string cur, string next) => string.Format("{0},{1}{2}", new object[3]
				{
					cur,
					next,
					Environment.NewLine
				}));
				UnityEngine.Debug.LogError("[SCENELOADER] duplicate scenes: " + text);
			}
			else
			{
				OnSceneLoaded += _loadingHistory.Add;
			}
		}

		public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = default(SceneLoadInfo);
			sceneLoadInfo.SceneName = sceneInfo.Name;
			sceneLoadInfo.LoadMode = mode;
			SceneLoadInfo obj = sceneLoadInfo;
			if (this.OnSceneLoading != null)
			{
				this.OnSceneLoading(obj);
			}
			SceneManager.LoadScene(sceneName, mode);
			if (this.OnSceneLoaded != null)
			{
				this.OnSceneLoaded(obj);
			}
		}

		public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = default(SceneLoadInfo);
			sceneLoadInfo.SceneName = sceneInfo.Name;
			sceneLoadInfo.LoadMode = mode;
			SceneLoadInfo sceneLoadInfo2 = sceneLoadInfo;
			if (this.OnSceneLoading != null)
			{
				this.OnSceneLoading(sceneLoadInfo2);
			}
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, mode);
			Singleton<SceneLoader>.Instance.StartCoroutine(WaitSceneIsLoaded(sceneLoadInfo2, asyncOperation));
			return asyncOperation;
		}

		public ExistsSceneInfo GetSceneInfo(string sceneName)
		{
			ExistsSceneInfo existsSceneInfo = (string.IsNullOrEmpty(Path.GetDirectoryName(sceneName)) ? ScenesList.Infos.FirstOrDefault((ExistsSceneInfo i) => i.Name == sceneName) : ScenesList.Infos.FirstOrDefault((ExistsSceneInfo i) => i.Path == sceneName));
			if (existsSceneInfo == null)
			{
				throw new ArgumentException(string.Format("Unknown scene : '{0}'", new object[1] { sceneName }));
			}
			return existsSceneInfo;
		}

		private IEnumerator WaitSceneIsLoaded(SceneLoadInfo loadInfo, AsyncOperation op)
		{
			while (!op.isDone)
			{
				yield return null;
			}
			if (this.OnSceneLoaded != null)
			{
				this.OnSceneLoaded(loadInfo);
			}
		}
	}
}
