using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.UI;

public sealed class ComicsCampaign : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ComicsCampaign _003C_003E4__this;

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
		public _003CStart_003Ed__9(int _003C_003E1__state)
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
				Texture texture = Resources.Load<Texture>(GetNameForIndex(_003C_003E4__this._frameCount + 1, LevelArt.endOfBox));
				_003C_003E4__this._hasSecondPage = texture != null;
				if (_003C_003E4__this._isFirstLaunch)
				{
					_003C_003E4__this.SetSkipHandler(null);
					_003C_003E4__this.backButton.gameObject.SetActive(false);
				}
				else if (_003C_003E4__this._hasSecondPage)
				{
					_003C_003E4__this.SetSkipHandler(_003C_003E4__this.GotoNextPage);
				}
				else
				{
					_003C_003E4__this.SetSkipHandler(_003C_003E4__this.GotoLevelOrBoxmap);
				}
				if (_003C_003E4__this.background != null)
				{
					_003C_003E4__this.background.texture = Resources.Load<Texture>("Arts_background_" + CurrentCampaignGame.boXName);
				}
				for (int i = 0; i != _003C_003E4__this._frameCount; i++)
				{
					string nameForIndex = GetNameForIndex(i + 1, LevelArt.endOfBox);
					Texture texture2 = Resources.Load<Texture>(nameForIndex);
					if (texture2 == null)
					{
						UnityEngine.Debug.LogWarning("Texture is null: " + nameForIndex);
						break;
					}
					_003C_003E4__this.comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture2.width);
					_003C_003E4__this.comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture2.height);
					_003C_003E4__this.comicFrames[i].texture = texture2;
					_003C_003E4__this.comicFrames[i].color = new Color(1f, 1f, 1f, 0f);
					string term = (LevelArt.endOfBox ? string.Format("{0}_{1}", new object[2]
					{
						CurrentCampaignGame.boXName,
						i
					}) : string.Format("{0}_{1}", new object[2]
					{
						CurrentCampaignGame.levelSceneName,
						i
					}));
					_003C_003E4__this._subtitles[i] = LocalizationStore.Get(term) ?? string.Empty;
				}
				_003C_003E4__this._coroutine = _003C_003E4__this.StartCoroutine(_003C_003E4__this.FadeInCoroutine());
				_003C_003E2__current = _003C_003E4__this._coroutine;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._hasSecondPage)
				{
					_003C_003E4__this.SetSkipHandler(_003C_003E4__this.GotoNextPage);
				}
				else
				{
					_003C_003E4__this.SetSkipHandler(_003C_003E4__this.GotoLevelOrBoxmap);
				}
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

	[CompilerGenerated]
	internal sealed class _003CFadeInCoroutine_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ComicsCampaign _003C_003E4__this;

		private RawImage _003Cf_003E5__1;

		private int _003Ci_003E5__2;

		private int _003CcomicFrameIndex_003E5__3;

		public Action skipHandler;

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
		public _003CFadeInCoroutine_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CcomicFrameIndex_003E5__3 = 0;
				goto IL_0161;
			case 1:
				_003C_003E1__state = -1;
				num = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num;
				goto IL_00fc;
			case 2:
				{
					_003C_003E1__state = -1;
					_003Cf_003E5__1 = null;
					goto IL_0151;
				}
				IL_0161:
				if (_003CcomicFrameIndex_003E5__3 != _003C_003E4__this.comicFrames.Length)
				{
					_003Cf_003E5__1 = _003C_003E4__this.comicFrames[_003CcomicFrameIndex_003E5__3];
					if (!(_003Cf_003E5__1 == null))
					{
						if (_003C_003E4__this.subtitlesText != null)
						{
							_003C_003E4__this.subtitlesText.text = _003C_003E4__this._subtitles[_003CcomicFrameIndex_003E5__3];
						}
						_003Ci_003E5__2 = 0;
						goto IL_00fc;
					}
					goto IL_0151;
				}
				if (skipHandler != null)
				{
					_003C_003E4__this.SetSkipHandler(skipHandler);
				}
				return false;
				IL_00fc:
				if (_003Ci_003E5__2 != 30)
				{
					float a = Mathf.InverseLerp(0f, 30f, _003Ci_003E5__2);
					_003Cf_003E5__1.color = new Color(1f, 1f, 1f, a);
					_003C_003E2__current = new WaitForRealSeconds(1f / 30f);
					_003C_003E1__state = 1;
					return true;
				}
				_003Cf_003E5__1.color = new Color(1f, 1f, 1f, 1f);
				_003C_003E2__current = new WaitForRealSeconds(2f);
				_003C_003E1__state = 2;
				return true;
				IL_0151:
				num = _003CcomicFrameIndex_003E5__3 + 1;
				_003CcomicFrameIndex_003E5__3 = num;
				goto IL_0161;
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

	public RawImage background;

	public RawImage[] comicFrames = new RawImage[4];

	public Button skipButton;

	public Button backButton;

	public Text subtitlesText;

	private string[] _subtitles = new string[4]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	private int _frameCount;

	private bool _hasSecondPage;

	private bool _isFirstLaunch = true;

	private Coroutine _coroutine;

	private Action _skipHandler;

	public void HandleSkipPressed()
	{
		ButtonClickSound.TryPlayClick();
		UnityEngine.Debug.Log("[Skip] pressed.");
		if (_skipHandler != null)
		{
			_skipHandler();
		}
	}

	public void HandleBackPressed()
	{
		ButtonClickSound.TryPlayClick();
		Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel");
	}

	private bool DetermineIfFirstLaunch()
	{
		if (LevelArt.endOfBox)
		{
			return !(Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0]).Any(CurrentCampaignGame.boXName.Equals);
		}
		return !(Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0]).Any(CurrentCampaignGame.levelSceneName.Equals);
	}

	private void Awake()
	{
		if (subtitlesText != null)
		{
			subtitlesText.transform.parent.gameObject.SetActive(LocalizationStore.CurrentLanguage != "English");
		}
		_frameCount = Math.Min(4, comicFrames.Length);
		_isFirstLaunch = DetermineIfFirstLaunch();
	}

	private IEnumerator Start()
	{
		Texture texture = Resources.Load<Texture>(GetNameForIndex(_frameCount + 1, LevelArt.endOfBox));
		_hasSecondPage = texture != null;
		if (_isFirstLaunch)
		{
			SetSkipHandler(null);
			backButton.gameObject.SetActive(false);
		}
		else if (_hasSecondPage)
		{
			SetSkipHandler(GotoNextPage);
		}
		else
		{
			SetSkipHandler(GotoLevelOrBoxmap);
		}
		if (background != null)
		{
			background.texture = Resources.Load<Texture>("Arts_background_" + CurrentCampaignGame.boXName);
		}
		for (int i = 0; i != _frameCount; i++)
		{
			string nameForIndex = GetNameForIndex(i + 1, LevelArt.endOfBox);
			Texture texture2 = Resources.Load<Texture>(nameForIndex);
			if (texture2 == null)
			{
				UnityEngine.Debug.LogWarning("Texture is null: " + nameForIndex);
				break;
			}
			comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture2.width);
			comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture2.height);
			comicFrames[i].texture = texture2;
			comicFrames[i].color = new Color(1f, 1f, 1f, 0f);
			string term = (LevelArt.endOfBox ? string.Format("{0}_{1}", new object[2]
			{
				CurrentCampaignGame.boXName,
				i
			}) : string.Format("{0}_{1}", new object[2]
			{
				CurrentCampaignGame.levelSceneName,
				i
			}));
			_subtitles[i] = LocalizationStore.Get(term) ?? string.Empty;
		}
		_coroutine = StartCoroutine(FadeInCoroutine());
		yield return _coroutine;
		if (_hasSecondPage)
		{
			SetSkipHandler(GotoNextPage);
		}
		else
		{
			SetSkipHandler(GotoLevelOrBoxmap);
		}
	}

	private void GotoNextPage()
	{
		if (_isFirstLaunch)
		{
			SetSkipHandler(null);
		}
		else
		{
			SetSkipHandler(GotoLevelOrBoxmap);
		}
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		for (int i = 0; i != comicFrames.Length; i++)
		{
			if (!(comicFrames[i] == null))
			{
				comicFrames[i].texture = null;
				comicFrames[i].color = new Color(1f, 1f, 1f, 0f);
				_subtitles[i] = string.Empty;
			}
		}
		Resources.UnloadUnusedAssets();
		for (int j = 0; j != _frameCount; j++)
		{
			Texture texture = Resources.Load<Texture>(GetNameForIndex(_frameCount + j + 1, LevelArt.endOfBox));
			if (texture == null)
			{
				break;
			}
			comicFrames[j].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture.width);
			comicFrames[j].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture.height);
			comicFrames[j].texture = texture;
			string term = (LevelArt.endOfBox ? string.Format("{0}_{1}", new object[2]
			{
				CurrentCampaignGame.boXName,
				_frameCount + j
			}) : string.Format("{0}_{1}", new object[2]
			{
				CurrentCampaignGame.levelSceneName,
				_frameCount + j
			}));
			_subtitles[j] = LocalizationStore.Get(term) ?? string.Empty;
		}
		_coroutine = StartCoroutine(FadeInCoroutine(GotoLevelOrBoxmap));
	}

	private void GotoLevelOrBoxmap()
	{
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		if (LevelArt.endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf(array, CurrentCampaignGame.boXName) == -1)
			{
				List<string> list = new List<string>(array);
				list.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, list.ToArray());
			}
		}
		else
		{
			string[] array2 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (Array.IndexOf(array2, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> list2 = new List<string>(array2);
				list2.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, list2.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene(LevelArt.endOfBox ? "ChooseLevel" : "CampaignLoading");
	}

	private void SetSkipHandler(Action skipHandler)
	{
		_skipHandler = skipHandler;
		if (skipButton != null)
		{
			skipButton.gameObject.SetActive(skipHandler != null);
		}
	}

	private IEnumerator FadeInCoroutine(Action skipHandler = null)
	{
		int comicFrameIndex = 0;
		while (comicFrameIndex != comicFrames.Length)
		{
			RawImage f = comicFrames[comicFrameIndex];
			int num;
			if (!(f == null))
			{
				if (subtitlesText != null)
				{
					subtitlesText.text = _subtitles[comicFrameIndex];
				}
				for (int i = 0; i != 30; i = num)
				{
					float a = Mathf.InverseLerp(0f, 30f, i);
					f.color = new Color(1f, 1f, 1f, a);
					yield return new WaitForRealSeconds(1f / 30f);
					num = i + 1;
				}
				f.color = new Color(1f, 1f, 1f, 1f);
				yield return new WaitForRealSeconds(2f);
			}
			num = comicFrameIndex + 1;
			comicFrameIndex = num;
		}
		if (skipHandler != null)
		{
			SetSkipHandler(skipHandler);
		}
	}

	private static string GetNameForIndex(int num, bool endOfBox)
	{
		return ResPath.Combine("Arts", ResPath.Combine(endOfBox ? CurrentCampaignGame.boXName : CurrentCampaignGame.levelSceneName, num.ToString()));
	}
}
