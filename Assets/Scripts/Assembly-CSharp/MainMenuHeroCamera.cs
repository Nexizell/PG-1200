using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public class MainMenuHeroCamera : MonoBehaviour
{
	public enum CameraPositionPreset
	{
		Common = 0,
		Low = 1,
		Hight = 2
	}

	public enum LobbyCameraAnimation
	{
		ToIdle = 0,
		ToSettings = 1,
		ToSocial = 2,
		ToLeaderboards = 3,
		ToGacha = 4,
		ToItem = 5
	}

	[Serializable]
	public class LobbyItemsOverrideController 
	{
		[DragAndDrop]
		public string ForItem;

		public LobbyItemInfo.LobbyItemSlot Slot;

		public AnimatorOverrideController OverrideController;
	}

	private PrefsEnumCachedProperty<CameraPositionPreset> _cachedCameraPreset = new PrefsEnumCachedProperty<CameraPositionPreset>("camera_preset");

	[SerializeField]
	protected internal List<LobbyItemsOverrideController> _lobbyItemsOverrideControllers;

	private Animator _animatorValue;

	private Camera _mainCamera;

	private UICamera _uiCamera;

	[SerializeField]
	protected internal CameraPositionPreset _currentCameraPreset;

	private Vector3 _mixFromPosition;

	private Quaternion _mixFromRotation;

	private float _mixPositionMoveTime;

	private float _mixPositionTimeElapsed;

	[Range(0f, 1f)]
	[SerializeField]
	protected internal float _changePresetTime = 0.5f;

	private float _changePresetTimeElapsed;

	private CameraPositionPreset _chengePresetFrom;

	[SerializeField]
	protected internal Vector3 _overridePositionIncrement;

	[SerializeField]
	protected internal Vector3 _overrideRotationIncrement;

	[SerializeField]
	[Header("<<= Position pressets =>>")]
	protected internal Vector3 _lowPositionPreset;

	[SerializeField]
	protected internal Vector3 _lowRotationPreset;

	[SerializeField]
	protected internal Vector3 _hightPositionPreset;

	[SerializeField]
	protected internal Vector3 _hightRotationPreset;

	private bool _lockOverrideCameraPosition;

	private bool _mixPositionForward;

	public static MainMenuHeroCamera Instance { get; private set; }

	private Animator _animator
	{
		get
		{
			if (_animatorValue == null)
			{
				_animatorValue = GetComponentInChildren<Animator>();
			}
			return _animatorValue;
		}
	}

	public Camera MainCamera
	{
		get
		{
			if (_mainCamera == null)
			{
				_mainCamera = GetComponentInChildren<Camera>();
			}
			return _mainCamera;
		}
	}

	public UICamera UICamera
	{
		get
		{
			if (_uiCamera == null)
			{
				_uiCamera = MainCamera.gameObject.GetComponent<UICamera>();
			}
			return _uiCamera;
		}
	}

	public bool IsAnimPlaying { get; private set; }

	private Transform CameraTransform
	{
		get
		{
			return MainCamera.transform;
		}
	}

	private bool MixPositionPlaying
	{
		get
		{
			return _mixPositionTimeElapsed < _mixPositionMoveTime;
		}
	}

	private void Awake()
	{
		SetCameraPreset(_cachedCameraPreset.Value, false, true);
		Instance = this;
	}

	[ContextMenu("Update state")]
	private void UpdateState()
	{
		SetCameraPreset(_currentCameraPreset);
	}

	public CameraPositionPreset GetCurrentCameraPresset()
	{
		return _cachedCameraPreset.Value;
	}

	public void SetCameraPreset(CameraPositionPreset preset, bool playAnimation = true, bool immidiate = false)
	{
		if (immidiate || _cachedCameraPreset.Value != preset)
		{
			switch (preset)
			{
			case CameraPositionPreset.Common:
				_overridePositionIncrement = Vector3.zero;
				_overrideRotationIncrement = Vector3.zero;
				break;
			case CameraPositionPreset.Low:
				_overridePositionIncrement = _lowPositionPreset;
				_overrideRotationIncrement = _lowRotationPreset;
				break;
			case CameraPositionPreset.Hight:
				_overridePositionIncrement = _hightPositionPreset;
				_overrideRotationIncrement = _hightRotationPreset;
				break;
			}
			_currentCameraPreset = preset;
			_chengePresetFrom = _cachedCameraPreset.Value;
			_cachedCameraPreset.Value = preset;
			if (playAnimation)
			{
				_changePresetTimeElapsed = 0f;
				_lockOverrideCameraPosition = false;
			}
		}
	}

	private void OnEnable()
	{
		BankController.OpenBank += OnOpenBankGui;
		BankController.CloseBank += OnCloseBankGui;
		ButOpenGift.onOpen += PlayShowGift;
		GiftBannerWindow.onClose += PlayCloseGift;
	}

	private void OnDisable()
	{
		BankController.OpenBank -= OnOpenBankGui;
		BankController.CloseBank -= OnCloseBankGui;
		ButOpenGift.onOpen -= PlayShowGift;
		GiftBannerWindow.onClose -= PlayCloseGift;
	}

	private void OnOpenBankGui()
	{
		MainCamera.enabled = false;
		UICamera.enabled = false;
	}

	private void OnCloseBankGui()
	{
		MainCamera.enabled = true;
		UICamera.enabled = true;
	}

	public void PlayOpenSocial()
	{
		PlayAnimation(LobbyCameraAnimation.ToSocial);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(false, false);
		}
	}

	public void PlayOpenOptions()
	{
		PlayAnimation(LobbyCameraAnimation.ToSettings);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(false, false);
		}
	}

	public void PlayCloseOptions()
	{
		PlayAnimation(LobbyCameraAnimation.ToIdle);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null && MenuLeaderboardsView.IsNeedShow)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(true, true);
		}
	}

	public void PlayShowGift()
	{
		PlayAnimation(LobbyCameraAnimation.ToGacha);
	}

	public void PlayCloseGift()
	{
		PlayAnimation(LobbyCameraAnimation.ToIdle);
	}

	public bool CanZoomTo(LobbyItem item)
	{
		if (item == null)
		{
			return false;
		}
		LobbyItemsOverrideController lobbyItemsOverrideController = _lobbyItemsOverrideControllers.FirstOrDefault((LobbyItemsOverrideController val) => val.ForItem == item.Info.Id);
		if (lobbyItemsOverrideController == null)
		{
			lobbyItemsOverrideController = _lobbyItemsOverrideControllers.FirstOrDefault((LobbyItemsOverrideController val) => val.Slot == item.Info.Slot);
		}
		if (lobbyItemsOverrideController != null)
		{
			return lobbyItemsOverrideController.OverrideController != null;
		}
		return false;
	}

	public void PlayLobbyItemZoom(LobbyItem item)
	{
		if (item == null)
		{
			Debug.LogError("MainMenuHeroCamera.PlayLobbyItemZoom error: item is null");
			return;
		}
		LobbyItemsOverrideController lobbyItemsOverrideController = _lobbyItemsOverrideControllers.FirstOrDefault((LobbyItemsOverrideController val) => val.ForItem == item.Info.Id);
		if (lobbyItemsOverrideController == null)
		{
			lobbyItemsOverrideController = _lobbyItemsOverrideControllers.FirstOrDefault((LobbyItemsOverrideController val) => val.Slot == item.Info.Slot);
		}
		if (lobbyItemsOverrideController == null || lobbyItemsOverrideController.OverrideController == null)
		{
			Debug.LogErrorFormat("override controller for item '{0}' not found", item.Info.Id);
		}
		else
		{
			_animator.runtimeAnimatorController = lobbyItemsOverrideController.OverrideController;
			PlayAnimation(LobbyCameraAnimation.ToItem);
		}
	}

	public void PlayFromItem()
	{
		PlayAnimation(LobbyCameraAnimation.ToIdle);
	}

	public void OnOpenSingleModePanel()
	{
		StopAllCoroutines();
	}

	public void OnCloseMinigamesGui()
	{
		if (MenuLeaderboardsView.IsNeedShow)
		{
			PlayAnimation(LobbyCameraAnimation.ToIdle);
		}
	}

	public void PlayAnimation(LobbyCameraAnimation animType)
	{
		if (animType == LobbyCameraAnimation.ToIdle)
		{
			MixPosition(1f, false);
			_lockOverrideCameraPosition = false;
		}
		else
		{
			MixPosition(1f, true);
			_lockOverrideCameraPosition = true;
		}
		_animator.SetTrigger(animType.ToString());
	}

	public void MixPosition(float mixIntervalSecs, bool forward)
	{
		_mixFromPosition = CameraTransform.localPosition;
		_mixFromRotation = CameraTransform.localRotation;
		_mixPositionMoveTime = mixIntervalSecs;
		_mixPositionTimeElapsed = 0f;
		_mixPositionForward = forward;
	}

	private void LateUpdate()
	{
		if (_changePresetTimeElapsed < _changePresetTime)
		{
			MixPreset();
		}
		else if (!_lockOverrideCameraPosition && !MixPositionPlaying)
		{
			OverrideCalmPosition();
		}
		else if (_mixPositionTimeElapsed < _mixPositionMoveTime)
		{
			if (_mixPositionForward)
			{
				MixPositionForward();
			}
			else
			{
				MixPsotionBackward();
			}
		}
	}

	private void MixPreset()
	{
		float t = _changePresetTimeElapsed / _changePresetTime;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		switch (_chengePresetFrom)
		{
		case CameraPositionPreset.Common:
			vector = Vector3.zero;
			vector2 = Vector3.zero;
			break;
		case CameraPositionPreset.Low:
			vector = _lowPositionPreset;
			vector2 = _lowRotationPreset;
			break;
		case CameraPositionPreset.Hight:
			vector = _hightPositionPreset;
			vector2 = _hightRotationPreset;
			break;
		}
		Vector3 vector3 = new Vector3(CameraTransform.localPosition.x + vector.x, CameraTransform.localPosition.y + vector.y, CameraTransform.localPosition.z + vector.z);
		Vector3 vector4 = new Vector3(CameraTransform.localPosition.x + _overridePositionIncrement.x, CameraTransform.localPosition.y + _overridePositionIncrement.y, CameraTransform.localPosition.z + _overridePositionIncrement.z);
		Vector3 localPosition = new Vector3(Mathf.Lerp(vector3.x, vector4.x, t), Mathf.Lerp(vector3.y, vector4.y, t), Mathf.Lerp(vector3.z, vector4.z, t));
		CameraTransform.localPosition = localPosition;
		Quaternion a = Quaternion.Euler(CameraTransform.localRotation.eulerAngles.x + vector2.x, CameraTransform.localRotation.eulerAngles.y + vector2.y, CameraTransform.localRotation.eulerAngles.z + vector2.z);
		Quaternion b = Quaternion.Euler(CameraTransform.localRotation.eulerAngles.x + _overrideRotationIncrement.x, CameraTransform.localRotation.eulerAngles.y + _overrideRotationIncrement.y, CameraTransform.localRotation.eulerAngles.z + _overrideRotationIncrement.z);
		CameraTransform.localRotation = Quaternion.Lerp(a, b, t);
		_changePresetTimeElapsed += Time.deltaTime;
	}

	private void MixPositionForward()
	{
		float t = _mixPositionTimeElapsed / _mixPositionMoveTime;
		Vector3 localPosition = new Vector3(Mathf.Lerp(_mixFromPosition.x, CameraTransform.localPosition.x, t), Mathf.Lerp(_mixFromPosition.y, CameraTransform.localPosition.y, t), Mathf.Lerp(_mixFromPosition.z, CameraTransform.localPosition.z, t));
		CameraTransform.localPosition = localPosition;
		CameraTransform.localRotation = Quaternion.Lerp(_mixFromRotation, CameraTransform.localRotation, t);
		_mixPositionTimeElapsed += Time.deltaTime;
	}

	private void MixPsotionBackward()
	{
		float num = _mixPositionTimeElapsed / _mixPositionMoveTime;
		Vector3 localPosition = new Vector3(Mathf.Lerp(_mixFromPosition.x, CameraTransform.localPosition.x, num), Mathf.Lerp(_mixFromPosition.y, CameraTransform.localPosition.y, num), Mathf.Lerp(_mixFromPosition.z, CameraTransform.localPosition.z, num));
		CameraTransform.localPosition = localPosition;
		CameraTransform.localRotation = Quaternion.Lerp(_mixFromRotation, CameraTransform.localRotation, num);
		Vector3 pos;
		Quaternion rot;
		CalculateOverridingCalmLerp(num, out pos, out rot);
		CameraTransform.localPosition = pos;
		CameraTransform.localRotation = rot;
		_mixPositionTimeElapsed += Time.deltaTime;
	}

	private void OverrideCalmPosition()
	{
		Vector3 pos;
		Quaternion rot;
		CalculateOverridingCalmLerp(1f, out pos, out rot);
		CameraTransform.localPosition = pos;
		CameraTransform.localRotation = rot;
	}

	private void CalculateOverridingCalmLerp(float overrideWeight, out Vector3 pos, out Quaternion rot)
	{
		pos = Vector3.zero;
		rot = Quaternion.identity;
		pos = new Vector3(Mathf.Lerp(CameraTransform.localPosition.x, CameraTransform.localPosition.x + _overridePositionIncrement.x, overrideWeight), Mathf.Lerp(CameraTransform.localPosition.y, CameraTransform.localPosition.y + _overridePositionIncrement.y, overrideWeight), Mathf.Lerp(CameraTransform.localPosition.z, CameraTransform.localPosition.z + _overridePositionIncrement.z, overrideWeight));
		Vector3 euler = new Vector3(CameraTransform.localRotation.eulerAngles.x + _overrideRotationIncrement.x, CameraTransform.localRotation.eulerAngles.y + _overrideRotationIncrement.y, CameraTransform.localRotation.eulerAngles.z + _overrideRotationIncrement.z);
		rot = Quaternion.Lerp(CameraTransform.localRotation, Quaternion.Euler(euler), overrideWeight);
	}
}
