using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Rilisoft;
using UnityEngine;

public sealed class LevelArt : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CShowArts_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LevelArt _003C_003E4__this;

		private float _003CprevTime_003E5__1;

		private float _003CstartTime_003E5__2;

		private Texture _003CnewComicsTexture_003E5__3;

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
		public _003CShowArts_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string path;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				path = string.Empty;
				_003CnewComicsTexture_003E5__3 = null;
				goto IL_002f;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this._alphaForComics += (Time.time - _003CprevTime_003E5__1) / _003C_003E4__this._delayShowComics;
				_003CprevTime_003E5__1 = Time.time;
				if (!(Time.time - _003CstartTime_003E5__2 < _003C_003E4__this._delayShowComics) || _003C_003E4__this._isSkipComics)
				{
					_003C_003E4__this._isSkipComics = false;
					_003C_003E4__this._alphaForComics = 1f;
					if (!(_003CnewComicsTexture_003E5__3 != null) || _003C_003E4__this._currentComicsImageIndex % 4 == 0)
					{
						_003C_003E2__current = new WaitForSeconds(_003C_003E4__this._delayShowComics);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_002f;
				}
				goto IL_0197;
			case 2:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this._isShowButton = true;
					return false;
				}
				IL_002f:
				_003CnewComicsTexture_003E5__3 = null;
				_003C_003E4__this._currentComicsImageIndex++;
				path = _003C_003E4__this._NameForNumber(_003C_003E4__this._currentComicsImageIndex);
				_003CnewComicsTexture_003E5__3 = Resources.Load<Texture>(path);
				if (_003CnewComicsTexture_003E5__3 != null)
				{
					if (_003C_003E4__this._comicsTextures.Count == 4)
					{
						_003C_003E4__this._comicsTextures.Clear();
					}
					_003C_003E4__this._comicsTextures.Add(_003CnewComicsTexture_003E5__3);
					string text = (endOfBox ? string.Format("{0}_{1}", new object[2]
					{
						CurrentCampaignGame.boXName,
						_003C_003E4__this._currentComicsImageIndex - 1
					}) : string.Format("{0}_{1}", new object[2]
					{
						CurrentCampaignGame.levelSceneName,
						_003C_003E4__this._currentComicsImageIndex - 1
					}));
					_003C_003E4__this._currentSubtitle = LocalizationStore.Get(text) ?? string.Empty;
					if (text.Equals(_003C_003E4__this._currentSubtitle))
					{
						_003C_003E4__this._currentSubtitle = string.Empty;
					}
					Resources.UnloadUnusedAssets();
					_003C_003E4__this._alphaForComics = 0f;
					_003CprevTime_003E5__1 = Time.time;
					_003CstartTime_003E5__2 = Time.time;
					goto IL_0197;
				}
				_003C_003E4__this.GoToLevel();
				return false;
				IL_0197:
				_003C_003E2__current = new WaitForEndOfFrame();
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

	private const int ComicsOnScreen = 4;

	public static readonly bool ShouldShowArts = true;

	public GUIStyle startButton;

	public static bool endOfBox = false;

	public GUIStyle labelsStyle;

	public float widthBackLabel = 770f;

	public float heightBackLabel = 100f;

	private float _alphaForComics;

	private int _currentComicsImageIndex;

	private bool _isFirstLaunch = true;

	public float _delayShowComics = 3f;

	private bool _isSkipComics;

	private int _countOfComics = 4;

	private Texture _backgroundComics;

	private List<Texture> _comicsTextures = new List<Texture>();

	private bool _isShowButton;

	private string _currentSubtitle;

	private bool _needShowSubtitle;

	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void Start()
	{
		_needShowSubtitle = LocalizationStore.CurrentLanguage != "English";
		labelsStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		labelsStyle.fontSize = Mathf.RoundToInt(20f * Defs.Coef);
		if (Resources.Load<Texture>(_NameForNumber(5)) != null)
		{
			_countOfComics *= 2;
		}
		StartCoroutine("ShowArts");
		_backgroundComics = Resources.Load<Texture>("Arts_background_" + CurrentCampaignGame.boXName);
		if (endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(CurrentCampaignGame.boXName))
				{
					_isFirstLaunch = false;
					break;
				}
			}
		}
		else
		{
			string[] array = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(CurrentCampaignGame.levelSceneName))
				{
					_isFirstLaunch = false;
					break;
				}
			}
		}
		_isShowButton = !_isFirstLaunch;
	}

	private void GoToLevel()
	{
		if (endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf(array, CurrentCampaignGame.boXName) == -1)
			{
				List<string> list = new List<string>();
				string[] array2 = array;
				foreach (string item in array2)
				{
					list.Add(item);
				}
				list.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, list.ToArray());
			}
		}
		else
		{
			string[] array3 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (!endOfBox && Array.IndexOf(array3, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> list2 = new List<string>();
				string[] array2 = array3;
				foreach (string item2 in array2)
				{
					list2.Add(item2);
				}
				list2.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, list2.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene(endOfBox ? "ChooseLevel" : "CampaignLoading");
	}

	private string _NameForNumber(int num)
	{
		return ResPath.Combine("Arts", ResPath.Combine(endOfBox ? CurrentCampaignGame.boXName : CurrentCampaignGame.levelSceneName, num.ToString()));
	}

	private IEnumerator ShowArts()
	{
		string empty = string.Empty;
		Texture newComicsTexture;
		do
		{
			_currentComicsImageIndex++;
			string path = _NameForNumber(_currentComicsImageIndex);
			newComicsTexture = Resources.Load<Texture>(path);
			if (newComicsTexture != null)
			{
				if (_comicsTextures.Count == 4)
				{
					_comicsTextures.Clear();
				}
				_comicsTextures.Add(newComicsTexture);
				string text = (endOfBox ? string.Format("{0}_{1}", new object[2]
				{
					CurrentCampaignGame.boXName,
					_currentComicsImageIndex - 1
				}) : string.Format("{0}_{1}", new object[2]
				{
					CurrentCampaignGame.levelSceneName,
					_currentComicsImageIndex - 1
				}));
				_currentSubtitle = LocalizationStore.Get(text) ?? string.Empty;
				if (text.Equals(_currentSubtitle))
				{
					_currentSubtitle = string.Empty;
				}
				Resources.UnloadUnusedAssets();
				_alphaForComics = 0f;
				float prevTime = Time.time;
				float startTime = Time.time;
				do
				{
					yield return new WaitForEndOfFrame();
					_alphaForComics += (Time.time - prevTime) / _delayShowComics;
					prevTime = Time.time;
				}
				while (Time.time - startTime < _delayShowComics && !_isSkipComics);
				_isSkipComics = false;
				_alphaForComics = 1f;
				continue;
			}
			GoToLevel();
			yield break;
		}
		while (newComicsTexture != null && _currentComicsImageIndex % 4 != 0);
		yield return new WaitForSeconds(_delayShowComics);
		_isShowButton = true;
	}

	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void OnGUI()
	{
	}

	public static string WrappedText(string text)
	{
		int num = 30;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = 0;
		int num3 = 0;
		while (num2 < text.Length)
		{
			stringBuilder.Append(text[num2]);
			if (text[num2] == '\n')
			{
				stringBuilder.Append('\n');
			}
			if (num3 >= num && text[num2] == ' ')
			{
				stringBuilder.Append("\n\n");
				num3 = 0;
			}
			num2++;
			num3++;
		}
		return stringBuilder.ToString();
	}
}
