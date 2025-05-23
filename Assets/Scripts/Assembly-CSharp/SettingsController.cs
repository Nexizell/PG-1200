using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class SettingsController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CHandleRestoreClickedCoroutine_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CHandleRestoreClickedCoroutine_003Ed__44(int _003C_003E1__state)
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
				_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(true, false, true));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
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
	internal sealed class _003CHandleSyncClickedCoroutine_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SettingsController _003C_003E4__this;

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
		public _003CHandleSyncClickedCoroutine_003Ed__46(int _003C_003E1__state)
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
				_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(true, false, true));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				try
				{
					_003C_003E4__this.RefreshSignOutButton();
					_003C_003E4__this.SetSyncLabelText();
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in HandleSyncClicked SynchronizeWithCloud onComplete: {0}", ex);
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

	public MainMenuHeroCamera rotateCamera;

	public UIButton backButton;

	public UIButton controlsButton;

	public UIButton syncButton;

	public UIButton signOutButton;

	public GameObject controlsSettings;

	public GameObject tapPanel;

	public GameObject swipePanel;

	public GameObject mainPanel;

	public UISlider sensitivitySlider;

	public UISlider pressureSensitivitySlider;

	public SettingsToggleButtons chatToggleButtons;

	public SettingsToggleButtons musicToggleButtons;

	public SettingsToggleButtons soundToggleButtons;

	public SettingsToggleButtons invertCameraToggleButtons;

	public SettingsToggleButtons jump3dTouchToggleButtons;

	public SettingsToggleButtons shoot3dTouchToggleButtons;

	public SettingsToggleButtons hideJumpAndShootButtons;

	public SettingsToggleButtons leftHandedToggleButtons;

	public SettingsToggleButtons switchingWeaponsToggleButtons;

	public SettingsToggleButtons fps60ToggleButtons;

	public SettingsToggleButtons inviteLocalToggleButton;

	public SettingsToggleButtons inviteRemoteToogleButton;

	public Texture googlePlayServicesTexture;

	[SerializeField]
	protected internal ToggleGroupHalper _lobbySoundsToggleGroup;

	private IDisposable _backSubscription;

	private bool _backRequested;

	private float _cachedSensitivity;

	private float _cachedPressure;

	public const int SensitivityLowerBound = 6;

	public const int SensitivityUpperBound = 19;

	public const float PressureLowerBound = 0.3f;

	public const float PressureUpperBound = 0.88f;

	public static event Action ControlsClicked;

	private void Awake()
	{
		_lobbySoundsToggleGroup.OnSelectedToggleChanged += OnLobbySoundToggleChanged;
	}

	private void OnLobbySoundToggleChanged(string name, bool active)
	{
		if (active)
		{
			MenuBackgroundMusic.LobbyBackgroundClip? lobbyBackgroundClip = name.ToEnum<MenuBackgroundMusic.LobbyBackgroundClip>(MenuBackgroundMusic.LobbyBackgroundClip.None);
			if (lobbyBackgroundClip.HasValue)
			{
				MenuBackgroundMusic.SetBackgroundClip(lobbyBackgroundClip.Value);
			}
		}
	}

	public static void SwitchChatSetting(bool on, Action additional = null)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Chat] button clicked: " + on);
		}
		if (Defs.IsChatOn != on)
		{
			Defs.IsChatOn = on;
			if (additional != null)
			{
				additional();
			}
		}
	}

	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	public static void ChangeLeftHandedRightHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Left Handed] button clicked: " + isChecked);
		}
		if (GlobalGameController.LeftHanded != isChecked)
		{
			GlobalGameController.LeftHanded = isChecked;
			PlayerPrefs.SetInt(Defs.LeftHandedSN, isChecked ? 1 : 0);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
			if (SettingsController.ControlsClicked != null)
			{
				SettingsController.ControlsClicked();
			}
			if (!isChecked && Application.isEditor)
			{
				UnityEngine.Debug.Log("Left-handed Layout Enabled");
			}
		}
	}

	public static void ChangeSwitchingWeaponHanded(bool isChecked, Action handler = null)
	{
		/*if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Switching Weapon button clicked: " + isChecked);
		}
		if (GlobalGameController.switchingWeaponSwipe == isChecked)
		{
			GlobalGameController.switchingWeaponSwipe = !isChecked;
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, GlobalGameController.switchingWeaponSwipe ? 1 : 0);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
		}*/
	}

	private void SetSyncLabelText()
	{
		UILabel uILabel = null;
		Transform transform = syncButton.transform.Find("Label");
		if (transform != null)
		{
			uILabel = transform.gameObject.GetComponent<UILabel>();
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			if (uILabel != null)
			{
				uILabel.text = LocalizationStore.Get("Key_0080");
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && uILabel != null)
		{
			uILabel.text = LocalizationStore.Get("Key_0935");
		}
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(delegate
		{
			HandleBackFromSettings(this, EventArgs.Empty);
		}, "Settings");
		RefreshSignOutButton();
	}

	internal void RefreshSignOutButton()
	{
		if (signOutButton != null)
		{
			if (Application.isEditor)
			{
				signOutButton.gameObject.SetActive(true);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				signOutButton.gameObject.SetActive(GpgFacade.Instance.IsAuthenticated());
			}
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
		if (backButton != null)
		{
			backButton.GetComponent<ButtonHandler>().Clicked += HandleBackFromSettings;
		}
		if (controlsButton != null)
		{
			controlsButton.GetComponent<ButtonHandler>().Clicked += HandleControlsClicked;
		}
		if (syncButton != null)
		{
			ButtonHandler component = syncButton.GetComponent<ButtonHandler>();
			SetSyncLabelText();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				syncButton.gameObject.SetActive(true);
				component.Clicked += HandleRestoreClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				syncButton.gameObject.SetActive(true);
				component.Clicked += HandleSyncClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				bool flag = false;
				flag = StarterPackController.Get.IsNeedResoreStarterPackWp8();
				syncButton.gameObject.SetActive(flag);
				component.Clicked += HandleSyncClicked;
			}
		}
		if (sensitivitySlider != null)
		{
			float num = Mathf.Clamp(Defs.Sensitivity, 6f, 19f);
			float num2 = num - 6f;
			sensitivitySlider.value = num2 / 13f;
			_cachedSensitivity = num;
		}
		else
		{
			UnityEngine.Debug.LogWarning("sensitivitySlider == null");
		}
		if (pressureSensitivitySlider != null)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				float num3 = Mathf.Clamp(Defs.touchPressurePower, 0.3f, 0.88f);
				float num4 = num3 - 0.3f;
				pressureSensitivitySlider.value = num4 / 0.58f;
				_cachedPressure = num3;
			}
			else
			{
				pressureSensitivitySlider.gameObject.SetActive(false);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("sensitivitySlider == null");
		}
		if (inviteLocalToggleButton != null)
		{
			inviteLocalToggleButton.IsChecked = Defs.isEnableLocalInviteFromFriends;
			inviteLocalToggleButton.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				Defs.isEnableLocalInviteFromFriends = e.IsChecked;
			};
		}
		if (inviteRemoteToogleButton != null)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite))
			{
				inviteRemoteToogleButton.gameObject.SetActive(true);
				inviteRemoteToogleButton.IsChecked = Defs.isEnableRemoteInviteFromFriends;
				inviteRemoteToogleButton.Clicked += delegate(object sender, ToggleButtonEventArgs e)
				{
					Defs.isEnableRemoteInviteFromFriends = e.IsChecked;
					RemotePushNotificationController.Instance.UpdateDataOnServer();
				};
			}
			else
			{
				inviteRemoteToogleButton.gameObject.SetActive(false);
			}
		}
		musicToggleButtons.IsChecked = Defs.isSoundMusic;
		musicToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("[Music] button clicked: " + e.IsChecked);
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
			MenuBackgroundMusic menuBackgroundMusic = ((gameObject != null) ? gameObject.GetComponent<MenuBackgroundMusic>() : null);
			if (Defs.isSoundMusic != e.IsChecked)
			{
				Defs.isSoundMusic = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
				PlayerPrefs.Save();
				if (menuBackgroundMusic != null)
				{
					if (e.IsChecked)
					{
						menuBackgroundMusic.Play();
					}
					else
					{
						menuBackgroundMusic.Stop();
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("menuBackgroundMusic == null");
				}
			}
		};
		soundToggleButtons.IsChecked = Defs.isSoundFX;
		soundToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("[Sound] button clicked: " + e.IsChecked);
			}
			if (Defs.isSoundFX != e.IsChecked)
			{
				Defs.isSoundFX = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
				PlayerPrefs.Save();
			}
		};
		chatToggleButtons.IsChecked = Defs.IsChatOn;
		chatToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			SwitchChatSetting(e.IsChecked);
		};
		invertCameraToggleButtons.IsChecked = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		invertCameraToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("[Invert Camera] button clicked: " + e.IsChecked);
			}
			if (PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1 != e.IsChecked)
			{
				PlayerPrefs.SetInt(Defs.InvertCamSN, Convert.ToInt32(e.IsChecked));
				PlayerPrefs.Save();
			}
		};
		if (leftHandedToggleButtons != null)
		{
			leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			leftHandedToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				ChangeLeftHandedRightHanded(e.IsChecked);
			};
		}
		if (fps60ToggleButtons != null)
		{
			fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			fps60ToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				Set60FPSEnable(e.IsChecked);
			};
		}
		if (switchingWeaponsToggleButtons != null)
		{
			switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
			switchingWeaponsToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				ChangeSwitchingWeaponHanded(e.IsChecked);
			};
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			shoot3dTouchToggleButtons.gameObject.SetActive(true);
			shoot3dTouchToggleButtons.IsChecked = Defs.isUseShoot3DTouch;
			shoot3dTouchToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isUseShoot3DTouch = e.IsChecked;
				hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			};
		}
		else
		{
			shoot3dTouchToggleButtons.gameObject.SetActive(false);
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			jump3dTouchToggleButtons.gameObject.SetActive(true);
			jump3dTouchToggleButtons.IsChecked = Defs.isUseJump3DTouch;
			jump3dTouchToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isUseJump3DTouch = e.IsChecked;
				hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			};
		}
		else
		{
			jump3dTouchToggleButtons.gameObject.SetActive(false);
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			hideJumpAndShootButtons.IsChecked = Defs.isJumpAndShootButtonOn;
			hideJumpAndShootButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isJumpAndShootButtonOn = e.IsChecked;
			};
		}
		else
		{
			hideJumpAndShootButtons.gameObject.SetActive(false);
		}
		_lobbySoundsToggleGroup.SelectToggle(MenuBackgroundMusic.SettedLobbyBackgrounClip);
	}

	private void Update()
	{
		if (_backRequested)
		{
			_backRequested = false;
			mainPanel.SetActive(true);
			base.gameObject.SetActive(false);
			if (rotateCamera == null)
			{
				GameObject gameObject = GameObject.Find("Camera_Rotate");
				if (gameObject != null)
				{
					rotateCamera = gameObject.GetComponent<MainMenuHeroCamera>();
				}
			}
			if (rotateCamera != null)
			{
				rotateCamera.PlayCloseOptions();
			}
			if (AnimationGift.instance != null)
			{
				AnimationGift.instance.CheckVisibleGift();
			}
			return;
		}
		float num = Mathf.Clamp(sensitivitySlider.value * 13f + 6f, 6f, 19f);
		if (_cachedSensitivity != num)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("New sensitivity: " + num);
			}
			Defs.Sensitivity = num;
			_cachedSensitivity = num;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			float num2 = Mathf.Clamp(pressureSensitivitySlider.value * 0.58f + 0.3f, 0.3f, 0.88f);
			if (_cachedPressure != num2)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("New pressure: " + num2);
				}
				Defs.touchPressurePower = num2;
				_cachedPressure = num2;
			}
		}
		if (syncButton != null && syncButton.gameObject.activeInHierarchy)
		{
			syncButton.isEnabled = !CloudSyncController.IsSynchronizingWithCloud;
		}
	}

	private void HandleBackFromSettings(object sender, EventArgs e)
	{
		_backRequested = true;
	}

	private void HandleControlsClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Controls] button clicked.");
		}
		controlsSettings.SetActive(true);
		tapPanel.SetActive(!GlobalGameController.switchingWeaponSwipe);
		swipePanel.SetActive(false);
		swipePanel.transform.parent.gameObject.SetActive(!GlobalGameController.switchingWeaponSwipe);
		base.gameObject.SetActive(false);
		if (SettingsController.ControlsClicked != null)
		{
			SettingsController.ControlsClicked();
		}
	}

	private void HandleRestoreClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Restore] button clicked.");
		}
		CoroutineRunner.Instance.StartCoroutine(HandleRestoreClickedCoroutine());
	}

	private static IEnumerator HandleRestoreClickedCoroutine()
	{
		yield return CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(true, false, true));
	}

	private void HandleSyncClicked(object sender, EventArgs e)
	{
		CoroutineRunner.Instance.StartCoroutine(HandleSyncClickedCoroutine());
	}

	private IEnumerator HandleSyncClickedCoroutine()
	{
		yield return CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(true, false, true));
		try
		{
			RefreshSignOutButton();
			SetSyncLabelText();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in HandleSyncClicked SynchronizeWithCloud onComplete: {0}", ex);
		}
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
		_lobbySoundsToggleGroup.OnSelectedToggleChanged -= OnLobbySoundToggleChanged;
	}

	private void HandleLocalizationChanged()
	{
		SetSyncLabelText();
	}
}
