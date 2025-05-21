using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MenuBackgroundMusic : MonoBehaviour
{
	public enum LobbyBackgroundClip
	{
		None = 0,
		Classic = 1,
		Ambient = 2,
		Modern = 3
	}

	[CompilerGenerated]
	internal sealed class _003CWaitFreeAwardControllerAndSubscribeCoroutine_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MenuBackgroundMusic _003C_003E4__this;

		private ScopeLogger _003C_003E7__wrap1;

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
		public _003CWaitFreeAwardControllerAndSubscribeCoroutine_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = new ScopeLogger("WaitFreeAwardControllerAndSubscribeCoroutine", false);
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (FreeAwardController.Instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				FreeAwardController.Instance.StateChanged -= _003C_003E4__this.HandleFreeAwardControllerStateChanged;
				FreeAwardController.Instance.StateChanged += _003C_003E4__this.HandleFreeAwardControllerStateChanged;
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(ScopeLogger);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CPlayMusicInternal_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AudioSource audioSource;

		public MenuBackgroundMusic _003C_003E4__this;

		private float _003CtargetVolume_003E5__1;

		private float _003CstartTime_003E5__2;

		private float _003CfadeTime_003E5__3;

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
		public _003CPlayMusicInternal_003Ed__17(int _003C_003E1__state)
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
				_003CtargetVolume_003E5__1 = 1f;
				audioSource.volume = 1f;
				audioSource.Play();
				_003C_003E4__this.currentAudioSource = audioSource;
				_003CstartTime_003E5__2 = Time.realtimeSinceStartup;
				_003CfadeTime_003E5__3 = 0.5f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (Time.realtimeSinceStartup - _003CstartTime_003E5__2 <= _003CfadeTime_003E5__3)
			{
				if (audioSource == null)
				{
					audioSource.volume = 1f;
					return false;
				}
				audioSource.volume = _003CtargetVolume_003E5__1 * (Time.realtimeSinceStartup - _003CstartTime_003E5__2) / _003CfadeTime_003E5__3;
				UnityEngine.Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ PlayMusicInternal " + audioSource.volume);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			audioSource.volume = 1f;
			UnityEngine.Debug.Log("----------------------------------------------------------------- PlayMusicInternal " + audioSource.volume);
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
	internal sealed class _003CStopMusicInternal_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AudioSource audioSource;

		private float _003CcurrentVolume_003E5__1;

		private float _003CstartTime_003E5__2;

		private float _003CfadeTime_003E5__3;

		public MenuBackgroundMusic _003C_003E4__this;

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
		public _003CStopMusicInternal_003Ed__18(int _003C_003E1__state)
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
				_003CcurrentVolume_003E5__1 = 1f;
				_003CstartTime_003E5__2 = Time.realtimeSinceStartup;
				_003CfadeTime_003E5__3 = 0.5f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (Time.realtimeSinceStartup - _003CstartTime_003E5__2 <= _003CfadeTime_003E5__3)
			{
				if (audioSource == null)
				{
					return false;
				}
				audioSource.volume = _003CcurrentVolume_003E5__1 * (1f - (Time.realtimeSinceStartup - _003CstartTime_003E5__2) / _003CfadeTime_003E5__3);
				UnityEngine.Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ StopMusicInternal " + audioSource.volume);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			audioSource.volume = 0f;
			audioSource.Stop();
			_003C_003E4__this.currentAudioSource = null;
			audioSource.volume = 1f;
			UnityEngine.Debug.Log("----------------------------------------------------------------- StopMusicInternal " + audioSource.volume);
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

	public const string KEY_LOBBY_SETTED_BG_MUSIC = "lobby_background_music";

	private List<AudioSource> _customMusicStack = new List<AudioSource>();

	private AudioSource currentAudioSource;

	public static bool keepPlaying = false;

	public static MenuBackgroundMusic sharedMusic;

	private static string[] scenetsToPlayMusicOn = new string[8]
	{
		Defs.MainMenuScene,
		"SettingScene",
		"SkinEditor",
		"ChooseLevel",
		"CampaignChooseBox",
		"ProfileShop",
		"Friends",
		"Clans"
	};

	public static string SettedLobbyBackgrounClip
	{
		get
		{
			return Storager.getString("lobby_background_music");
		}
	}

	public void PlayCustomMusicFrom(GameObject audioSourceObj)
	{
		RemoveNullsFromCustomMusicStack();
		if (audioSourceObj != null && Defs.isSoundMusic)
		{
			AudioSource component = audioSourceObj.GetComponent<AudioSource>();
			PlayMusic(component);
			if (!_customMusicStack.Contains(component))
			{
				if (_customMusicStack.Count > 0)
				{
					StopMusic(_customMusicStack[_customMusicStack.Count - 1]);
				}
				_customMusicStack.Add(audioSourceObj.GetComponent<AudioSource>());
			}
		}
		string value = SceneManager.GetActiveScene().name;
		if (Array.IndexOf(scenetsToPlayMusicOn, value) >= 0)
		{
			Stop();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
		if (gameObject != null)
		{
			AudioSource component2 = gameObject.GetComponent<AudioSource>();
			if (component2 != null)
			{
				StopMusic(component2);
			}
		}
	}

	public void StopCustomMusicFrom(GameObject audioSourceObj)
	{
		RemoveNullsFromCustomMusicStack();
		AudioSource component = audioSourceObj.GetComponent<AudioSource>();
		if (audioSourceObj != null && component != null)
		{
			StopMusic(component);
			_customMusicStack.Remove(component);
		}
		if (_customMusicStack.Count > 0)
		{
			PlayMusic(_customMusicStack[_customMusicStack.Count - 1]);
			return;
		}
		if (Array.IndexOf(scenetsToPlayMusicOn, Application.loadedLevelName) >= 0)
		{
			Play();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
		if (gameObject != null)
		{
			AudioSource component2 = gameObject.GetComponent<AudioSource>();
			if (component2 != null)
			{
				PlayMusic(component2);
			}
		}
	}

	internal void Start()
	{
		sharedMusic = this;
		Defs.isSoundMusic = PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true);
		Defs.isSoundFX = PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		string text = Storager.getString("lobby_background_music");
		if (text.IsNullOrEmpty())
		{
			text = LobbyBackgroundClip.Ambient.ToString();
			Storager.setString("lobby_background_music", text);
		}
		LobbyBackgroundClip? lobbyBackgroundClip = text.ToEnum<LobbyBackgroundClip>(LobbyBackgroundClip.None);
		if (lobbyBackgroundClip.HasValue && lobbyBackgroundClip != LobbyBackgroundClip.None)
		{
			AudioSource component = sharedMusic.GetComponent<AudioSource>();
			if (component != null)
			{
				AudioClip clip = Resources.Load<AudioClip>("MenuMusic/menu_music_" + lobbyBackgroundClip.Value.ToString().ToLower());
				component.clip = clip;
			}
		}
	}

	internal void Play()
	{
		if (Defs.isSoundMusic)
		{
			PlayMusic(GetComponent<AudioSource>());
		}
	}

	public void Stop()
	{
		StopMusic(GetComponent<AudioSource>());
	}

	private void RemoveNullsFromCustomMusicStack()
	{
		List<AudioSource> customMusicStack = _customMusicStack;
		_customMusicStack = new List<AudioSource>();
		foreach (AudioSource item in customMusicStack)
		{
			if (item != null)
			{
				_customMusicStack.Add(item);
			}
		}
	}

	private IEnumerator WaitFreeAwardControllerAndSubscribeCoroutine()
	{
		using (new ScopeLogger("WaitFreeAwardControllerAndSubscribeCoroutine", false))
		{
			while (FreeAwardController.Instance == null)
			{
				yield return null;
			}
			FreeAwardController.Instance.StateChanged -= HandleFreeAwardControllerStateChanged;
			FreeAwardController.Instance.StateChanged += HandleFreeAwardControllerStateChanged;
		}
	}

	private void OnLevelWasLoaded(int idx)
	{
		StopAllCoroutines();
		CoroutineRunner.Instance.StartCoroutine(WaitFreeAwardControllerAndSubscribeCoroutine());
		foreach (AudioSource item in _customMusicStack)
		{
			if (item != null)
			{
				item.Stop();
			}
		}
		_customMusicStack.Clear();
		if (Array.IndexOf(scenetsToPlayMusicOn, Application.loadedLevelName) >= 0 || keepPlaying)
		{
			if (!GetComponent<AudioSource>().isPlaying && PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true))
			{
				PlayMusic(GetComponent<AudioSource>());
			}
		}
		else
		{
			StopMusic(GetComponent<AudioSource>());
		}
		keepPlaying = false;
	}

	private void HandleFreeAwardControllerStateChanged(object sender, FreeAwardController.StateEventArgs e)
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "HandleFreeAwardControllerStateChanged({0} -> {1})", e.OldState, e.State);
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			if (e.State is FreeAwardController.WatchingState)
			{
				Stop();
			}
			else if (e.OldState is FreeAwardController.WatchingState)
			{
				Play();
			}
		}
	}

	public void PlayMusic(AudioSource audioSource)
	{
		if (!(audioSource == null) && Defs.isSoundMusic)
		{
			if (Switcher.comicsSound != null && audioSource != Switcher.comicsSound.GetComponent<AudioSource>())
			{
				UnityEngine.Object.Destroy(Switcher.comicsSound);
				Switcher.comicsSound = null;
			}
			if (PhotonNetwork.connected)
			{
				float num = 0f;
				num = Convert.ToSingle(PhotonNetwork.time) - audioSource.clip.length * (float)Mathf.FloorToInt(Convert.ToSingle(PhotonNetwork.time) / audioSource.clip.length);
				audioSource.time = num;
			}
			audioSource.Play();
		}
	}

	public void StopMusic(AudioSource audioSource)
	{
		if (!(audioSource == null))
		{
			audioSource.Stop();
		}
	}

	private IEnumerator PlayMusicInternal(AudioSource audioSource)
	{
		float targetVolume = 1f;
		audioSource.volume = 1f;
		audioSource.Play();
		currentAudioSource = audioSource;
		float startTime = Time.realtimeSinceStartup;
		float fadeTime = 0.5f;
		while (Time.realtimeSinceStartup - startTime <= fadeTime)
		{
			if (audioSource == null)
			{
				audioSource.volume = 1f;
				yield break;
			}
			audioSource.volume = targetVolume * (Time.realtimeSinceStartup - startTime) / fadeTime;
			UnityEngine.Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ PlayMusicInternal " + audioSource.volume);
			yield return null;
		}
		audioSource.volume = 1f;
		UnityEngine.Debug.Log("----------------------------------------------------------------- PlayMusicInternal " + audioSource.volume);
	}

	private IEnumerator StopMusicInternal(AudioSource audioSource)
	{
		float currentVolume = 1f;
		float startTime = Time.realtimeSinceStartup;
		float fadeTime = 0.5f;
		while (Time.realtimeSinceStartup - startTime <= fadeTime)
		{
			if (audioSource == null)
			{
				yield break;
			}
			audioSource.volume = currentVolume * (1f - (Time.realtimeSinceStartup - startTime) / fadeTime);
			UnityEngine.Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ StopMusicInternal " + audioSource.volume);
			yield return null;
		}
		audioSource.volume = 0f;
		audioSource.Stop();
		currentAudioSource = null;
		audioSource.volume = 1f;
		UnityEngine.Debug.Log("----------------------------------------------------------------- StopMusicInternal " + audioSource.volume);
	}

	private void PlayCurrentMusic()
	{
		if (currentAudioSource != null)
		{
			PlayMusic(currentAudioSource);
		}
	}

	private void PauseCurrentMusic()
	{
		if (currentAudioSource != null)
		{
			currentAudioSource.Pause();
		}
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			PlayCurrentMusic();
		}
		else
		{
			PauseCurrentMusic();
		}
	}

	public static void SetBackgroundClip(LobbyBackgroundClip clipType)
	{
		if (!(sharedMusic != null))
		{
			return;
		}
		AudioSource component = sharedMusic.GetComponent<AudioSource>();
		if (component != null)
		{
			string text = clipType.ToString().ToLower();
			if (SettedLobbyBackgrounClip.ToLower() != text && clipType != 0)
			{
				Storager.setString("lobby_background_music", clipType.ToString());
				AudioClip clip = Resources.Load<AudioClip>("MenuMusic/menu_music_" + text);
				sharedMusic.Stop();
				component.clip = clip;
				sharedMusic.Play();
			}
		}
	}
}
