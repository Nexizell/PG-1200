using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class SceneInfoController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CGetDataFromServerLoop_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SceneInfoController _003C_003E4__this;

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
		public _003CGetDataFromServerLoop_003Ed__28(int _003C_003E1__state)
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
				goto IL_0022;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForRealSeconds(870f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0022;
				}
				IL_0022:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDataFormServer());
				_003C_003E1__state = 1;
				return true;
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
	internal sealed class _003CDownloadDataFormServer_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SceneInfoController _003C_003E4__this;

		private string _003CurlDataAddress_003E5__1;

		private WWW _003CdownloadData_003E5__2;

		private int _003Citer_003E5__3;

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
		public _003CDownloadDataFormServer_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string lData;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._isLoadingDataActive)
				{
					return false;
				}
				_003C_003E4__this._isLoadingDataActive = true;
				_003CurlDataAddress_003E5__1 = UrlForLoadData;
				_003CdownloadData_003E5__2 = null;
				_003Citer_003E5__3 = 3;
				goto IL_00e0;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0091;
			case 2:
			{
				_003C_003E1__state = -1;
				int num = _003Citer_003E5__3 - 1;
				_003Citer_003E5__3 = num;
				goto IL_00e0;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this._isLoadingDataActive = false;
					return false;
				}
				IL_0091:
				if (!_003CdownloadData_003E5__2.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (!string.IsNullOrEmpty(_003CdownloadData_003E5__2.error))
				{
					_003C_003E2__current = new WaitForRealSeconds(5f);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00ec;
				IL_00e0:
				if (_003Citer_003E5__3 > 0)
				{
					_003CdownloadData_003E5__2 = Tools.CreateWwwIfNotConnected(_003CurlDataAddress_003E5__1);
					if (_003CdownloadData_003E5__2 == null)
					{
						return false;
					}
					goto IL_0091;
				}
				goto IL_00ec;
				IL_00ec:
				if (_003CdownloadData_003E5__2 == null || !string.IsNullOrEmpty(_003CdownloadData_003E5__2.error))
				{
					if (Defs.IsDeveloperBuild && _003CdownloadData_003E5__2 != null)
					{
						UnityEngine.Debug.LogWarningFormat("Request to {0} failed: {1}", _003CurlDataAddress_003E5__1, _003CdownloadData_003E5__2.error);
					}
					_003C_003E4__this._isLoadingDataActive = false;
					return false;
				}
				lData = URLs.Sanitize(_003CdownloadData_003E5__2);
				_003C_003E2__current = _003C_003E4__this.ParseLoadData(lData);
				_003C_003E1__state = 3;
				return true;
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
	internal sealed class _003CParseLoadData_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string lData;

		public SceneInfoController _003C_003E4__this;

		private Dictionary<string, object> _003CallData_003E5__1;

		private GameConnect.GameMode _003CcurModeMap_003E5__2;

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
		public _003CParseLoadData_003Ed__30(int _003C_003E1__state)
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
				_003CallData_003E5__1 = Json.Deserialize(lData) as Dictionary<string, object>;
				if (_003CallData_003E5__1 == null)
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogError("Bad response: " + lData);
					}
					_003C_003E4__this._isLoadingDataActive = false;
					return false;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (ExperienceController.sharedController == null)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.copyAllScenes = new List<SceneInfo>();
			_003C_003E4__this.copyModeInfo = new List<AllScenesForMode>();
			_003C_003E4__this.copyModeInfo.Clear();
			if (_003CallData_003E5__1.ContainsKey("allAvaliableMap"))
			{
				List<object> list = _003CallData_003E5__1["allAvaliableMap"] as List<object>;
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
					if (dictionary == null)
					{
						continue;
					}
					string text = "";
					string minVersion = "";
					string maxVersion = "";
					if (dictionary.ContainsKey("nameScene"))
					{
						text = dictionary["nameScene"].ToString();
						if (dictionary.ContainsKey("minV"))
						{
							minVersion = dictionary["minV"].ToString();
						}
						if (dictionary.ContainsKey("maxV"))
						{
							maxVersion = dictionary["maxV"].ToString();
						}
						_003C_003E4__this.AddSceneIfAvaliableVersion(text, minVersion, maxVersion);
					}
				}
			}
			if (_003CallData_003E5__1.ContainsKey("modeMap"))
			{
				List<object> list2 = _003CallData_003E5__1["modeMap"] as List<object>;
				for (int j = 0; j < list2.Count; j++)
				{
					Dictionary<string, object> dictionary2 = list2[j] as Dictionary<string, object>;
					if (dictionary2 == null || !dictionary2.ContainsKey("modeId"))
					{
						continue;
					}
					_003CcurModeMap_003E5__2 = ConvertModeToEnum(dictionary2["modeId"].ToString());
					if (!dictionary2.ContainsKey("scenesForMode"))
					{
						continue;
					}
					List<object> list3 = dictionary2["scenesForMode"] as List<object>;
					for (int k = 0; k < list3.Count; k++)
					{
						Dictionary<string, object> dictionary3 = list3[k] as Dictionary<string, object>;
						if (dictionary3 == null)
						{
							continue;
						}
						bool flag = true;
						if (dictionary3.ContainsKey("minLevPlayerForAval"))
						{
							int num = Convert.ToInt32(dictionary3["minLevPlayerForAval"]);
							if (ExperienceController.sharedController.currentLevel < num)
							{
								flag = false;
							}
						}
						if (dictionary3.ContainsKey("ratingCount"))
						{
							int num2 = Convert.ToInt32(dictionary3["ratingCount"]);
							if (RatingSystem.instance.currentRating < num2)
							{
								flag = false;
							}
						}
						if (flag && dictionary3.ContainsKey("nameScene"))
						{
							_003C_003E4__this.AddSceneInModeGame(dictionary3["nameScene"].ToString(), _003CcurModeMap_003E5__2);
						}
					}
				}
			}
			_003C_003E4__this.OnDataLoaded();
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

	public static SceneInfoController instance = null;

	public List<SceneInfo> allScenes = new List<SceneInfo>();

	public List<AllScenesForMode> modeInfo = new List<AllScenesForMode>();

	private bool _isLoadingDataActive;

	private const float timerUpdateDataFromServer = 870f;

	private List<SceneInfo> copyAllScenes;

	private List<AllScenesForMode> copyModeInfo;

	private static readonly Dictionary<GameConnect.GameMode, int> _modeUnlockLevels = new Dictionary<GameConnect.GameMode, int>(GameRegimeComparer.Instance)
	{
		{
			GameConnect.GameMode.Deathmatch,
			1
		},
		{
			GameConnect.GameMode.TeamFight,
			2
		},
		{
			GameConnect.GameMode.TimeBattle,
			3
		},
		{
			GameConnect.GameMode.FlagCapture,
			4
		},
		{
			GameConnect.GameMode.DeadlyGames,
			5
		},
		{
			GameConnect.GameMode.CapturePoints,
			6
		}
	};

	private Version CurrentVersion
	{
		get
		{
			return new Version(12, 0, 0);
		}
	}

	public static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_amazon.json";
				}
				return string.Empty;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_wp8.json";
			}
			return string.Empty;
		}
	}

	public static event Action onChangeInfoMap;

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ExperienceController.onLevelChange += UpdateListAvaliableMap;
		LocalizationStore.AddEventCallAfterLocalize(OnChangeLocalize);
		UpdateListAvaliableMap();
	}

	private void OnDestroy()
	{
		ExperienceController.onLevelChange -= UpdateListAvaliableMap;
		instance = null;
	}

	public void UpdateListAvaliableMap()
	{
		if (!_isLoadingDataActive)
		{
			_isLoadingDataActive = true;
			TextAsset textAsset = Resources.Load<TextAsset>("mapList");
			if (textAsset != null)
			{
				StartCoroutine(ParseLoadData(textAsset.text));
			}
			else
			{
				UnityEngine.Debug.LogWarning("Bindata == null");
			}
		}
	}

	public SceneInfo GetInfoScene(string nameScene)
	{
		return allScenes.Find((SceneInfo curInf) => StringComparer.OrdinalIgnoreCase.Equals(curInf.gameObject.name, nameScene));
	}

	public SceneInfo GetInfoScene(string nameScene, List<SceneInfo> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((SceneInfo curInf) => StringComparer.OrdinalIgnoreCase.Equals(curInf.gameObject.name, nameScene));
	}

	public SceneInfo GetInfoScene(GameConnect.GameMode needMode, int indexMap)
	{
		SceneInfo infoScene = GetInfoScene(indexMap);
		if (infoScene != null && infoScene.IsAvaliableForMode(needMode))
		{
			return infoScene;
		}
		return null;
	}

	public SceneInfo GetInfoScene(int indexMap)
	{
		return allScenes.Find((SceneInfo curInf) => curInf.indexMap == indexMap);
	}

	public int GetMaxCountMapsInRegims()
	{
		int num = 0;
		foreach (AllScenesForMode item in modeInfo)
		{
			if (item.avaliableScenes.Count > num)
			{
				num = item.avaliableScenes.Count;
			}
		}
		return num;
	}

	public AllScenesForMode GetListScenesForMode(GameConnect.GameMode needMode)
	{
		return modeInfo.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	public AllScenesForMode GetListScenesForMode(GameConnect.GameMode needMode, List<AllScenesForMode> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	public int GetCountScenesForMode(GameConnect.GameMode needMode)
	{
		AllScenesForMode allScenesForMode = modeInfo.Find((AllScenesForMode nM) => nM.mode == needMode);
		if (allScenesForMode != null)
		{
			return allScenesForMode.avaliableScenes.Count;
		}
		return 0;
	}

	private void AddSceneIfAvaliableVersion(string nameScene, string minVersion, string maxVersion)
	{
		if (GetInfoScene(nameScene, copyAllScenes) == null)
		{
			Version currentVersion = CurrentVersion;
			Version version = new Version(maxVersion);
			Version version2 = new Version(minVersion);
			if (currentVersion >= version2 && currentVersion <= version)
			{
				SceneInfo component = (Resources.Load("SceneInfo/" + nameScene) as GameObject).GetComponent<SceneInfo>();
				GameObject obj = UnityEngine.Object.Instantiate(component.gameObject);
				obj.transform.SetParent(base.transform);
				obj.gameObject.name = nameScene;
				component = obj.GetComponent<SceneInfo>();
				component.minAvaliableVersion = minVersion;
				component.maxAvaliableVersion = maxVersion;
				component.UpdateKeyLoaded();
				copyAllScenes.Add(component);
			}
		}
	}

	public bool MapExistInProject(string nameScene)
	{
		return true;
	}

	private void AddSceneInModeGame(string nameScene, GameConnect.GameMode needMode)
	{
		SceneInfo infoScene = GetInfoScene(nameScene, copyAllScenes);
		if (!(infoScene != null))
		{
			return;
		}
		infoScene.AddMode(needMode);
		if (infoScene.IsLoaded)
		{
			AllScenesForMode allScenesForMode = GetListScenesForMode(needMode, copyModeInfo);
			if (allScenesForMode == null)
			{
				allScenesForMode = new AllScenesForMode();
				allScenesForMode.mode = needMode;
				copyModeInfo.Add(allScenesForMode);
			}
			allScenesForMode.AddInfoScene(infoScene);
		}
	}

	private IEnumerator GetDataFromServerLoop()
	{
		while (true)
		{
			yield return StartCoroutine(DownloadDataFormServer());
			yield return new WaitForRealSeconds(870f);
		}
	}

	private IEnumerator DownloadDataFormServer()
	{
		if (_isLoadingDataActive)
		{
			yield break;
		}
		_isLoadingDataActive = true;
		string urlDataAddress = UrlForLoadData;
		WWW downloadData = null;
		int iter = 3;
		while (iter > 0)
		{
			downloadData = Tools.CreateWwwIfNotConnected(urlDataAddress);
			if (downloadData == null)
			{
				yield break;
			}
			while (!downloadData.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(downloadData.error))
			{
				break;
			}
			yield return new WaitForRealSeconds(5f);
			int num = iter - 1;
			iter = num;
		}
		if (downloadData == null || !string.IsNullOrEmpty(downloadData.error))
		{
			if (Defs.IsDeveloperBuild && downloadData != null)
			{
				UnityEngine.Debug.LogWarningFormat("Request to {0} failed: {1}", urlDataAddress, downloadData.error);
			}
			_isLoadingDataActive = false;
		}
		else
		{
			string lData = URLs.Sanitize(downloadData);
			yield return ParseLoadData(lData);
			_isLoadingDataActive = false;
		}
	}

	private IEnumerator ParseLoadData(string lData)
	{
		Dictionary<string, object> allData = Json.Deserialize(lData) as Dictionary<string, object>;
		if (allData == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogError("Bad response: " + lData);
			}
			_isLoadingDataActive = false;
			yield break;
		}
		while (ExperienceController.sharedController == null)
		{
			yield return null;
		}
		copyAllScenes = new List<SceneInfo>();
		copyModeInfo = new List<AllScenesForMode>();
		copyModeInfo.Clear();
		if (allData.ContainsKey("allAvaliableMap"))
		{
			List<object> list = allData["allAvaliableMap"] as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				if (dictionary == null)
				{
					continue;
				}
				string minVersion = "";
				string maxVersion = "";
				if (dictionary.ContainsKey("nameScene"))
				{
					string nameScene = dictionary["nameScene"].ToString();
					if (dictionary.ContainsKey("minV"))
					{
						minVersion = dictionary["minV"].ToString();
					}
					if (dictionary.ContainsKey("maxV"))
					{
						maxVersion = dictionary["maxV"].ToString();
					}
					AddSceneIfAvaliableVersion(nameScene, minVersion, maxVersion);
				}
			}
		}
		if (allData.ContainsKey("modeMap"))
		{
			List<object> list2 = allData["modeMap"] as List<object>;
			for (int j = 0; j < list2.Count; j++)
			{
				Dictionary<string, object> dictionary2 = list2[j] as Dictionary<string, object>;
				if (dictionary2 == null || !dictionary2.ContainsKey("modeId"))
				{
					continue;
				}
				GameConnect.GameMode curModeMap = ConvertModeToEnum(dictionary2["modeId"].ToString());
				if (!dictionary2.ContainsKey("scenesForMode"))
				{
					continue;
				}
				List<object> list3 = dictionary2["scenesForMode"] as List<object>;
				for (int k = 0; k < list3.Count; k++)
				{
					Dictionary<string, object> dictionary3 = list3[k] as Dictionary<string, object>;
					if (dictionary3 == null)
					{
						continue;
					}
					bool flag = true;
					if (dictionary3.ContainsKey("minLevPlayerForAval"))
					{
						int num = Convert.ToInt32(dictionary3["minLevPlayerForAval"]);
						if (ExperienceController.sharedController.currentLevel < num)
						{
							flag = false;
						}
					}
					if (dictionary3.ContainsKey("ratingCount"))
					{
						int num2 = Convert.ToInt32(dictionary3["ratingCount"]);
						if (RatingSystem.instance.currentRating < num2)
						{
							flag = false;
						}
					}
					if (flag && dictionary3.ContainsKey("nameScene"))
					{
						AddSceneInModeGame(dictionary3["nameScene"].ToString(), curModeMap);
					}
				}
			}
		}
		OnDataLoaded();
	}

	public static GameConnect.GameMode ConvertModeToEnum(string modeStr)
	{
		return (GameConnect.GameMode)Enum.Parse(typeof(GameConnect.GameMode), modeStr);
	}

	internal static HashSet<GameConnect.GameMode> GetUnlockedModesByLevel(int level)
	{
		HashSet<GameConnect.GameMode> hashSet = new HashSet<GameConnect.GameMode>();
		foreach (KeyValuePair<GameConnect.GameMode, int> modeUnlockLevel in _modeUnlockLevels)
		{
			if (modeUnlockLevel.Value <= level)
			{
				hashSet.Add(modeUnlockLevel.Key);
			}
		}
		return hashSet;
	}

	private void OnDataLoaded()
	{
		allScenes = copyAllScenes;
		modeInfo = copyModeInfo;
		OnChangeLocalize();
		if (SceneInfoController.onChangeInfoMap != null)
		{
			SceneInfoController.onChangeInfoMap();
		}
		_isLoadingDataActive = false;
	}

	private void OnChangeLocalize()
	{
		for (int i = 0; i < allScenes.Count; i++)
		{
			allScenes[i].UpdateLocalize();
		}
	}
}
