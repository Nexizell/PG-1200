using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class NewsLobbyController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CUpdateNewsList_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NewsLobbyController _003C_003E4__this;

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
		public _003CUpdateNewsList_003Ed__19(int _003C_003E1__state)
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
				if (_003C_003E4__this.GetNews())
				{
					_003C_003E4__this.UpdateItemsCount();
					_003C_003E4__this.FillData();
					for (int i = 0; i < _003C_003E4__this.newsList.Count; i++)
					{
						_003C_003E4__this.newsList[i].GetComponent<UIToggle>().Set(false);
					}
					_003C_003E4__this.newsList[0].GetComponent<UIToggle>().Set(true);
					_003C_003E4__this.newsScroll.enabled = _003C_003E4__this.newsListInfo.Count > 4;
					_003C_003E4__this.StartCoroutine(_003C_003E4__this.ClearCacheFullPictures());
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				while (_003C_003E4__this.newsList.Count > 0)
				{
					UnityEngine.Object.Destroy(_003C_003E4__this.newsList[_003C_003E4__this.newsList.Count - 1].gameObject);
					_003C_003E4__this.newsList.RemoveAt(_003C_003E4__this.newsList.Count - 1);
				}
				_003C_003E4__this.headerLabel.text = LocalizationStore.Get("Key_1807");
				_003C_003E4__this.dateLabel.text = "";
				_003C_003E4__this.descLabel.text = "";
				_003C_003E4__this.desc2Label.text = "";
				_003C_003E4__this.newsPic.aspectRatio = 200f;
				_003C_003E4__this.newsPic.enabled = false;
				_003C_003E4__this.urlButton.SetActive(false);
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.fullNewsScroll.ResetPosition();
				break;
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

	[CompilerGenerated]
	internal sealed class _003CLoadPictureForFullNews_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NewsLobbyController _003C_003E4__this;

		public string picLink;

		private WWW _003CloadPic_003E5__1;

		private string _003CcachePath_003E5__2;

		public int index;

		private Texture2D _003CpicTexture_003E5__3;

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
		public _003CLoadPictureForFullNews_003Ed__25(int _003C_003E1__state)
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
				_003CpicTexture_003E5__3 = null;
				if (_003C_003E4__this.newsPic.mainTexture != null)
				{
					UnityEngine.Object.Destroy(_003C_003E4__this.newsPic.mainTexture);
					_003C_003E4__this.newsPic.mainTexture = null;
				}
				_003C_003E4__this.newsPic.aspectRatio = 200f;
				_003C_003E4__this.newsPic.mainTexture = null;
				_003CcachePath_003E5__2 = PersistentCache.Instance.GetCachePathByUri(picLink);
				bool flag = false;
				if (!string.IsNullOrEmpty(_003CcachePath_003E5__2))
				{
					try
					{
						bool flag2 = File.Exists(_003CcachePath_003E5__2);
						if (Defs.IsDeveloperBuild)
						{
							string text2 = (Application.isEditor ? ("<color=orange>" + _003CcachePath_003E5__2 + "</color>") : _003CcachePath_003E5__2);
							UnityEngine.Debug.LogFormat("Trying to load news image from cache '{0}': {1}", text2, flag2);
						}
						if (flag2)
						{
							byte[] data = File.ReadAllBytes(_003CcachePath_003E5__2);
							_003CpicTexture_003E5__3 = new Texture2D(2, 2);
							_003CpicTexture_003E5__3.LoadImage(data);
							_003CpicTexture_003E5__3.filterMode = FilterMode.Point;
							flag = true;
						}
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogWarning("Caught exception while reading cached news image. See next message for details.");
						UnityEngine.Debug.LogException(exception2);
					}
					if (!flag)
					{
						_003CloadPic_003E5__1 = Tools.CreateWwwIfNotConnected(picLink);
						if (_003CloadPic_003E5__1 == null)
						{
							return false;
						}
						_003C_003E2__current = _003CloadPic_003E5__1;
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_02bc;
			}
			case 1:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CloadPic_003E5__1.error))
				{
					UnityEngine.Debug.LogWarning("Download pic error: " + _003CloadPic_003E5__1.error);
					return false;
				}
				_003CpicTexture_003E5__3 = new Texture2D(2, 2);
				_003CpicTexture_003E5__3.LoadImage(_003CloadPic_003E5__1.bytes);
				_003CpicTexture_003E5__3.filterMode = FilterMode.Point;
				if (!string.IsNullOrEmpty(_003CcachePath_003E5__2))
				{
					try
					{
						if (Defs.IsDeveloperBuild)
						{
							string text = (Application.isEditor ? ("<color=orange>" + _003CcachePath_003E5__2 + "</color>") : _003CcachePath_003E5__2);
							UnityEngine.Debug.LogFormat("Trying to save news image to cache '{0}'", text);
						}
						string directoryName = Path.GetDirectoryName(_003CcachePath_003E5__2);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						byte[] bytes = _003CloadPic_003E5__1.bytes;
						File.WriteAllBytes(_003CcachePath_003E5__2, bytes);
						_003C_003E4__this.SaveCacheFullPicturesNameFile(picLink);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogWarning("Caught exception while saving news image to cache. See next message for details.");
						UnityEngine.Debug.LogException(exception);
					}
				}
				_003CloadPic_003E5__1.Dispose();
				_003CloadPic_003E5__1 = null;
				_003CloadPic_003E5__1 = null;
				goto IL_02bc;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.newsPic.ResizeCollider();
				goto IL_03cc;
			case 3:
				{
					_003C_003E1__state = -1;
					_003CpicTexture_003E5__3 = null;
					return false;
				}
				IL_02bc:
				if (_003C_003E4__this.selectedIndex == index)
				{
					if (_003C_003E4__this.newsPic.GetComponent<BoxCollider>() == null)
					{
						_003C_003E4__this.newsPic.gameObject.AddComponent<BoxCollider>();
						_003C_003E4__this.newsPic.gameObject.AddComponent<UIDragScrollView>();
						_003C_003E4__this.newsPic.gameObject.AddComponent<UIButton>().onClick.Add(new EventDelegate(_003C_003E4__this.OnURLClick));
					}
					_003C_003E4__this.newsPic.mainTexture = _003CpicTexture_003E5__3;
					_003C_003E4__this.newsPic.aspectRatio = (float)_003CpicTexture_003E5__3.width / (float)_003CpicTexture_003E5__3.height;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				if (_003CpicTexture_003E5__3 != null)
				{
					UnityEngine.Object.Destroy(_003CpicTexture_003E5__3);
				}
				goto IL_03cc;
				IL_03cc:
				_003C_003E2__current = null;
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
	internal sealed class _003CClearCacheFullPictures_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NewsLobbyController _003C_003E4__this;

		private List<string> _003C_cacheListForRemove_003E5__1;

		private List<string> _003C_cacheList_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CClearCacheFullPictures_003Ed__26(int _003C_003E1__state)
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
				List<object> list = Json.Deserialize(PlayerPrefs.GetString(_003C_003E4__this.cacheFullPictureNewsFileNamesKey, "[]")) as List<object>;
				_003C_cacheList_003E5__2 = new List<string>();
				_003C_cacheListForRemove_003E5__1 = new List<string>();
				for (int i = 0; i < list.Count; i++)
				{
					_003C_cacheList_003E5__2.Add(list[i].ToString());
					_003C_cacheListForRemove_003E5__1.Add(list[i].ToString());
				}
				for (int j = 0; j < _003C_003E4__this.newsListInfo.Count; j++)
				{
					if (_003C_003E4__this.newsListInfo[j].ContainsKey("fullpicture") && _003C_cacheListForRemove_003E5__1.Contains(_003C_003E4__this.newsListInfo[j]["fullpicture"].ToString()))
					{
						_003C_cacheListForRemove_003E5__1.Remove(_003C_003E4__this.newsListInfo[j]["fullpicture"].ToString());
					}
				}
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003C_cacheListForRemove_003E5__1.Count)
			{
				File.Delete(PersistentCache.Instance.GetCachePathByUri(_003C_cacheListForRemove_003E5__1[_003Ci_003E5__3]));
				_003C_cacheList_003E5__2.Remove(_003C_cacheListForRemove_003E5__1[_003Ci_003E5__3]);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			PlayerPrefs.SetString(_003C_003E4__this.cacheFullPictureNewsFileNamesKey, Json.Serialize(_003C_cacheList_003E5__2));
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

	public UIGrid newsGrid;

	public UIScrollView newsScroll;

	public UIScrollView fullNewsScroll;

	public UILabel headerLabel;

	public UILabel descLabel;

	public UILabel desc2Label;

	public UILabel dateLabel;

	public UITexture newsPic;

	public string currentURL;

	public string currentNewsName;

	public int selectedIndex;

	public GameObject newsItemPrefab;

	public GameObject urlButton;

	public List<NewsLobbyItem> newsList = new List<NewsLobbyItem>();

	private List<Dictionary<string, object>> newsListInfo = new List<Dictionary<string, object>>();

	private Texture2D[] newsFullPic;

	public static NewsLobbyController sharedController;

	private string cacheFullPictureNewsFileNamesKey = "cacheFullPictureFileNamesKey";

	private void Awake()
	{
		sharedController = this;
	}

	private IEnumerator UpdateNewsList()
	{
		if (GetNews())
		{
			UpdateItemsCount();
			FillData();
			for (int i = 0; i < newsList.Count; i++)
			{
				newsList[i].GetComponent<UIToggle>().Set(false);
			}
			newsList[0].GetComponent<UIToggle>().Set(true);
			newsScroll.enabled = newsListInfo.Count > 4;
			StartCoroutine(ClearCacheFullPictures());
			yield return null;
			fullNewsScroll.ResetPosition();
		}
		else
		{
			while (newsList.Count > 0)
			{
				UnityEngine.Object.Destroy(newsList[newsList.Count - 1].gameObject);
				newsList.RemoveAt(newsList.Count - 1);
			}
			headerLabel.text = LocalizationStore.Get("Key_1807");
			dateLabel.text = "";
			descLabel.text = "";
			desc2Label.text = "";
			newsPic.aspectRatio = 200f;
			newsPic.enabled = false;
			urlButton.SetActive(false);
		}
	}

	private void OnEnable()
	{
		StartCoroutine(UpdateNewsList());
	}

	private void OnDisable()
	{
		if (newsPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(newsPic.mainTexture);
			newsPic.mainTexture = null;
		}
	}

	private bool GetNews()
	{
		string @string = PlayerPrefs.GetString("LobbyNewsKey", "[]");
		newsListInfo = (Json.Deserialize(@string) as List<object>).OfType<Dictionary<string, object>>().ToList();
		if (newsListInfo == null || newsListInfo.Count == 0)
		{
			return false;
		}
		return true;
	}

	private void FillData()
	{
		for (int i = 0; i < newsList.Count; i++)
		{
			Dictionary<string, object> dictionary = newsListInfo[i];
			Dictionary<string, object> dictionary2 = dictionary["short_header"] as Dictionary<string, object>;
			Dictionary<string, object> dictionary3 = dictionary["short_description"] as Dictionary<string, object>;
			if (dictionary2 != null && dictionary3 != null)
			{
				object value;
				if (!dictionary2.TryGetValue(LocalizationManager.CurrentLanguage, out value))
				{
					dictionary2.TryGetValue("English", out value);
				}
				object value2;
				if (!dictionary3.TryGetValue(LocalizationManager.CurrentLanguage, out value2))
				{
					dictionary3.TryGetValue("English", out value2);
				}
				newsList[i].headerLabel.text = (string)value;
				if (Convert.ToInt32(dictionary["readed"]) == 0)
				{
					newsList[i].GetComponent<UISprite>().color = Color.white;
					newsList[i].indicatorNew.SetActive(true);
				}
				else
				{
					newsList[i].GetComponent<UISprite>().color = Color.gray;
					newsList[i].indicatorNew.SetActive(false);
				}
				newsList[i].shortDescLabel.text = (string)value2;
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(Convert.ToInt32(dictionary["date"]) + DateTimeOffset.Now.Offset.Hours * 3600);
				newsList[i].dateLabel.text = currentTimeByUnixTime.Day.ToString("D2") + "." + currentTimeByUnixTime.Month.ToString("D2") + "." + currentTimeByUnixTime.Year + "\n" + currentTimeByUnixTime.Hour + ":" + currentTimeByUnixTime.Minute.ToString("D2");
				object value3;
				if (dictionary.TryGetValue("previewpicture", out value3))
				{
					newsList[i].LoadPreview((string)value3);
				}
			}
		}
	}

	public void OnURLClick()
	{
		if (!string.IsNullOrEmpty(currentURL))
		{
			try
			{
				AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
				{
					{ "Conversion Total", "Source" },
					{ "Conversion By News", currentNewsName }
				});
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception in log News: " + ex);
			}
			Application.OpenURL(currentURL);
		}
	}

	private IEnumerator LoadPictureForFullNews(int index, string picLink)
	{
		Texture2D picTexture = null;
		if (newsPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(newsPic.mainTexture);
			newsPic.mainTexture = null;
		}
		newsPic.aspectRatio = 200f;
		newsPic.mainTexture = null;
		string cachePath = PersistentCache.Instance.GetCachePathByUri(picLink);
		bool flag = false;
		if (!string.IsNullOrEmpty(cachePath))
		{
			try
			{
				bool flag2 = File.Exists(cachePath);
				if (Defs.IsDeveloperBuild)
				{
					string text = (Application.isEditor ? ("<color=orange>" + cachePath + "</color>") : cachePath);
					UnityEngine.Debug.LogFormat("Trying to load news image from cache '{0}': {1}", text, flag2);
				}
				if (flag2)
				{
					byte[] data = File.ReadAllBytes(cachePath);
					picTexture = new Texture2D(2, 2);
					picTexture.LoadImage(data);
					picTexture.filterMode = FilterMode.Point;
					flag = true;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogWarning("Caught exception while reading cached news image. See next message for details.");
				UnityEngine.Debug.LogException(exception);
			}
			if (!flag)
			{
				WWW loadPic = Tools.CreateWwwIfNotConnected(picLink);
				if (loadPic == null)
				{
					yield break;
				}
				yield return loadPic;
				if (!string.IsNullOrEmpty(loadPic.error))
				{
					UnityEngine.Debug.LogWarning("Download pic error: " + loadPic.error);
					yield break;
				}
				picTexture = new Texture2D(2, 2);
				picTexture.LoadImage(loadPic.bytes);
				picTexture.filterMode = FilterMode.Point;
				if (!string.IsNullOrEmpty(cachePath))
				{
					try
					{
						if (Defs.IsDeveloperBuild)
						{
							string text2 = (Application.isEditor ? ("<color=orange>" + cachePath + "</color>") : cachePath);
							UnityEngine.Debug.LogFormat("Trying to save news image to cache '{0}'", text2);
						}
						string directoryName = Path.GetDirectoryName(cachePath);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						byte[] bytes = loadPic.bytes;
						File.WriteAllBytes(cachePath, bytes);
						SaveCacheFullPicturesNameFile(picLink);
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogWarning("Caught exception while saving news image to cache. See next message for details.");
						UnityEngine.Debug.LogException(exception2);
					}
				}
				loadPic.Dispose();
			}
		}
		if (selectedIndex == index)
		{
			if (newsPic.GetComponent<BoxCollider>() == null)
			{
				newsPic.gameObject.AddComponent<BoxCollider>();
				newsPic.gameObject.AddComponent<UIDragScrollView>();
				newsPic.gameObject.AddComponent<UIButton>().onClick.Add(new EventDelegate(OnURLClick));
			}
			newsPic.mainTexture = picTexture;
			newsPic.aspectRatio = (float)picTexture.width / (float)picTexture.height;
			yield return null;
			newsPic.ResizeCollider();
		}
		else if (picTexture != null)
		{
			UnityEngine.Object.Destroy(picTexture);
		}
		yield return null;
	}

	private IEnumerator ClearCacheFullPictures()
	{
		List<object> list = Json.Deserialize(PlayerPrefs.GetString(cacheFullPictureNewsFileNamesKey, "[]")) as List<object>;
		List<string> _cacheList = new List<string>();
		List<string> _cacheListForRemove = new List<string>();
		for (int j = 0; j < list.Count; j++)
		{
			_cacheList.Add(list[j].ToString());
			_cacheListForRemove.Add(list[j].ToString());
		}
		for (int k = 0; k < newsListInfo.Count; k++)
		{
			if (newsListInfo[k].ContainsKey("fullpicture") && _cacheListForRemove.Contains(newsListInfo[k]["fullpicture"].ToString()))
			{
				_cacheListForRemove.Remove(newsListInfo[k]["fullpicture"].ToString());
			}
		}
		for (int i = 0; i < _cacheListForRemove.Count; i++)
		{
			File.Delete(PersistentCache.Instance.GetCachePathByUri(_cacheListForRemove[i]));
			_cacheList.Remove(_cacheListForRemove[i]);
			yield return null;
		}
		PlayerPrefs.SetString(cacheFullPictureNewsFileNamesKey, Json.Serialize(_cacheList));
	}

	private void SaveCacheFullPicturesNameFile(string _nameFile)
	{
		List<object> list = Json.Deserialize(PlayerPrefs.GetString(cacheFullPictureNewsFileNamesKey, "[]")) as List<object>;
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].ToString());
		}
		if (!list2.Contains(_nameFile))
		{
			list2.Add(_nameFile);
		}
		PlayerPrefs.SetString(cacheFullPictureNewsFileNamesKey, Json.Serialize(list2));
	}

	private void SaveReaded()
	{
		PlayerPrefs.SetString("LobbyNewsKey", Json.Serialize(newsListInfo));
		bool flag = false;
		for (int i = 0; i < newsListInfo.Count; i++)
		{
			if (Convert.ToInt32(newsListInfo[i]["readed"]) == 0)
			{
				flag = true;
			}
		}
		PlayerPrefs.SetInt("LobbyIsAnyNewsKey", flag ? 1 : 0);
		MainMenuController.sharedController.newsIndicator.SetActive(flag);
		PlayerPrefs.Save();
	}

	public void SetNewsIndex(int index)
	{
		selectedIndex = index;
		fullNewsScroll.ResetPosition();
		Dictionary<string, object> dictionary = newsListInfo[index];
		Dictionary<string, object> dictionary2 = dictionary["header"] as Dictionary<string, object>;
		Dictionary<string, object> dictionary3 = dictionary["description"] as Dictionary<string, object>;
		Dictionary<string, object> dictionary4 = dictionary["category"] as Dictionary<string, object>;
		if (dictionary2 == null || dictionary3 == null || dictionary4 == null)
		{
			return;
		}
		object value;
		if (!dictionary2.TryGetValue(LocalizationManager.CurrentLanguage, out value))
		{
			dictionary2.TryGetValue("English", out value);
		}
		object value2;
		if (!dictionary3.TryGetValue(LocalizationManager.CurrentLanguage, out value2))
		{
			dictionary3.TryGetValue("English", out value2);
		}
		object value3;
		if (!dictionary4.TryGetValue(LocalizationManager.CurrentLanguage, out value3))
		{
			dictionary4.TryGetValue("English", out value3);
		}
		object value4;
		if (dictionary.TryGetValue("URL", out value4) && !value4.Equals(""))
		{
			currentURL = (string)value4;
			currentNewsName = (dictionary2.ContainsKey("English") ? dictionary2["English"].ToString() : "NO ENGLISH TRANSLATION");
			urlButton.SetActive(true);
		}
		else
		{
			currentURL = "";
			urlButton.SetActive(false);
		}
		headerLabel.text = (string)value;
		string[] array = ((string)value2).Split(new string[1] { "[news-pic]" }, StringSplitOptions.None);
		object value5;
		dictionary.TryGetValue("fullpicture", out value5);
		if (array.Length > 1 && !string.IsNullOrEmpty((string)value5))
		{
			descLabel.text = array[0];
			desc2Label.text = array[1];
			newsPic.enabled = true;
			StartCoroutine(LoadPictureForFullNews(index, (string)value5));
		}
		else
		{
			descLabel.text = (string)value2;
			desc2Label.text = "";
			newsPic.aspectRatio = 200f;
			newsPic.enabled = false;
		}
		DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(Convert.ToInt32(dictionary["date"]) + DateTimeOffset.Now.Offset.Hours * 3600);
		dateLabel.text = "[bababa]" + currentTimeByUnixTime.Day.ToString("D2") + "." + currentTimeByUnixTime.Month.ToString("D2") + "." + currentTimeByUnixTime.Year + " / [-]" + value3;
		try
		{
			if (Convert.ToInt32(dictionary["readed"]) == 0)
			{
				AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
				{
					{ "CTR", "Open" },
					{ "Conversion Total", "Open" },
					{
						"News",
						dictionary2.ContainsKey("English") ? dictionary2["English"].ToString() : "NO ENGLISH TRANSLATION"
					}
				});
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in log News: " + ex);
		}
		dictionary["readed"] = 1;
		FillData();
		SaveReaded();
	}

	private void UpdateItemsCount()
	{
		while (newsList.Count < newsListInfo.Count)
		{
			GameObject gameObject = NGUITools.AddChild(newsGrid.gameObject, newsItemPrefab);
			gameObject.SetActive(true);
			newsList.Add(gameObject.GetComponent<NewsLobbyItem>());
		}
		while (newsList.Count > newsListInfo.Count)
		{
			UnityEngine.Object.Destroy(newsList[newsList.Count - 1].gameObject);
			newsList.RemoveAt(newsList.Count - 1);
		}
		newsGrid.Reposition();
		newsScroll.ResetPosition();
	}

	public void Close()
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController._isCancellationRequested = true;
		}
	}
}
