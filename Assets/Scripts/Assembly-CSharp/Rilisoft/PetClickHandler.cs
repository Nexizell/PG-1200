using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class PetClickHandler : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CPlayTapAnimation_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PetClickHandler _003C_003E4__this;

			private float _003CanimTime_003E5__1;

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
			public _003CPlayTapAnimation_003Ed__13(int _003C_003E1__state)
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
					if (_003C_003E4__this._tapAnimIsPlaying)
					{
						return false;
					}
					_003CanimTime_003E5__1 = _003C_003E4__this.GetTapAnimationPlayTime();
					if (_003CanimTime_003E5__1 <= 0f)
					{
						return false;
					}
					_003C_003E4__this._tapAnimIsPlaying = true;
					_003C_003E4__this._myPetEngine.Animator.Play("Tap");
					if (Defs.isSoundFX && _003C_003E4__this._myPetEngine.ClipTap != null)
					{
						_003C_003E4__this._myPetEngine.AudioSourceOne.spatialBlend = 0f;
						_003C_003E4__this._myPetEngine.AudioSourceOne.clip = _003C_003E4__this._myPetEngine.ClipTap;
						_003C_003E4__this._myPetEngine.AudioSourceOne.Play();
					}
					break;
				case 1:
					_003C_003E1__state = -1;
					_003CanimTime_003E5__1 -= Time.deltaTime;
					break;
				}
				if (_003C_003E4__this._tapAnimIsPlaying && _003CanimTime_003E5__1 > 0f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this._tapAnimIsPlaying = false;
				PetAnimation animation = _003C_003E4__this._myPetEngine.GetAnimation(PetAnimationType.Profile);
				if (animation != null)
				{
					_003C_003E4__this._myPetEngine.Animator.Play(animation.AnimationName);
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

		public const string TAP_ANIMATION_NAME = "Tap";

		[SerializeField]
		protected internal float _threshold = 10f;

		private Vector3? _mouseDownPos;

		private PetEngine _myPetEngine;

		private Camera _clickStartCamera;

		private bool _tapAnimIsPlaying;

		private static bool CanClickToPet
		{
			get
			{
				if ((FeedbackMenuController.Instance == null || !FeedbackMenuController.Instance.gameObject.activeSelf) && (PauseGUIController.Instance == null || !PauseGUIController.Instance.IsPaused) && (BankController.Instance == null || BankController.Instance.uiRoot == null || !BankController.Instance.uiRoot.gameObject.activeInHierarchy) && (NewsLobbyController.sharedController == null || !NewsLobbyController.sharedController.isActiveAndEnabled) && (LeaderboardScript.Instance == null || !LeaderboardScript.Instance.UIEnabled) && (FriendsWindowGUI.Instance == null || !FriendsWindowGUI.Instance.cameraObject.activeInHierarchy) && (Nest.Instance == null || !Nest.Instance.BannerIsVisible) && (FriendsController.sharedController == null || !FriendsController.sharedController.ProfileInterfaceActive) && (BannerWindowController.SharedController == null || !BannerWindowController.SharedController.IsAnyBannerShown) && (FreeAwardController.Instance == null || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>()) && (MainMenuController.sharedController == null || !MainMenuController.sharedController.InMiniGamesScreen) && (LobbyCraftController.Instance == null || !LobbyCraftController.Instance.InterfaceEnabled))
				{
					if (!(MainMenuController.sharedController == null))
					{
						if (!MainMenuController.sharedController.SettingsJoysticksPanel.activeInHierarchy)
						{
							return !MainMenuController.sharedController.settingsPanel.activeInHierarchy;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		private void OnDisable()
		{
			_mouseDownPos = null;
			_tapAnimIsPlaying = false;
			PetAnimation animation = _myPetEngine.GetAnimation(PetAnimationType.Profile);
			if (animation != null)
			{
				_myPetEngine.Animator.Play(animation.AnimationName);
			}
		}

		private void Start()
		{
			_myPetEngine = GetPetEngine(base.gameObject);
			if (_myPetEngine == null)
			{
				UnityEngine.Debug.LogError("PetEngine not found");
			}
		}

		private void Update()
		{
			if (_myPetEngine == null)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				_mouseDownPos = Input.mousePosition;
				_clickStartCamera = GetCurrentCamera();
			}
			else if (Input.GetMouseButtonUp(0) && _mouseDownPos.HasValue && !(_clickStartCamera != GetCurrentCamera()) && Vector3.Distance(Input.mousePosition, _mouseDownPos.Value) <= _threshold && CanClickToPet && (!ShopNGUIController.GuiActive || !(_myPetEngine != ShopNGUIController.sharedShop.ShopCharacterInterface.myPetEngine)))
			{
				GameObject touchedGo = null;
				if (CheckTouchToPet(out touchedGo) && touchedGo.Equals(_myPetEngine.gameObject))
				{
					StartCoroutine(PlayTapAnimation());
				}
			}
		}

		private bool CheckTouchToPet(out GameObject touchedGo)
		{
			touchedGo = null;
			Camera currentCamera = GetCurrentCamera();
			if (currentCamera == null)
			{
				return false;
			}
			float maxDistance = Vector3.Distance(currentCamera.transform.position, base.gameObject.transform.position);
			RaycastHit hitInfo;
			if (Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, maxDistance, 1 << base.gameObject.layer) && (hitInfo.transform.gameObject.CompareTag("Pet") || hitInfo.transform.gameObject.IsSubobjectOf(_myPetEngine.gameObject)))
			{
				touchedGo = _myPetEngine.gameObject;
				return true;
			}
			return false;
		}

		private Camera GetCurrentCamera()
		{
			Camera result = null;
			if (ShopNGUIController.GuiActive)
			{
				result = ShopNGUIController.sharedShop.Camera3D;
			}
			else if (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled)
			{
				result = ProfileController.Instance.Camera3D;
			}
			else if (MainMenuController.sharedController != null)
			{
				result = MainMenuController.sharedController.Camera3D;
			}
			return result;
		}

		private IEnumerator PlayTapAnimation()
		{
			if (_tapAnimIsPlaying)
			{
				yield break;
			}
			float animTime = GetTapAnimationPlayTime();
			if (!(animTime <= 0f))
			{
				_tapAnimIsPlaying = true;
				_myPetEngine.Animator.Play("Tap");
				if (Defs.isSoundFX && _myPetEngine.ClipTap != null)
				{
					_myPetEngine.AudioSourceOne.spatialBlend = 0f;
					_myPetEngine.AudioSourceOne.clip = _myPetEngine.ClipTap;
					_myPetEngine.AudioSourceOne.Play();
				}
				while (_tapAnimIsPlaying && animTime > 0f)
				{
					yield return null;
					animTime -= Time.deltaTime;
				}
				_tapAnimIsPlaying = false;
				PetAnimation animation = _myPetEngine.GetAnimation(PetAnimationType.Profile);
				if (animation != null)
				{
					_myPetEngine.Animator.Play(animation.AnimationName);
				}
			}
		}

		private PetEngine GetPetEngine(GameObject petGo)
		{
			if (petGo == null)
			{
				return null;
			}
			PetEngine component = petGo.GetComponent<PetEngine>();
			if (component != null)
			{
				return component;
			}
			component = petGo.GetComponentInParents<PetEngine>();
			if (component != null)
			{
				return component;
			}
			GameObject gameObject = petGo.Ancestors().LastOrDefault();
			if (gameObject != null)
			{
				GameObject[] array = gameObject.Descendants().ToArray();
				foreach (GameObject gameObject2 in array)
				{
					if (gameObject2.GetComponent<PetEngine>() != null)
					{
						return gameObject2.GetComponent<PetEngine>();
					}
				}
			}
			return null;
		}

		private float GetTapAnimationPlayTime()
		{
			foreach (AnimationState item in _myPetEngine.Animator)
			{
				if (item.clip.name == "Tap")
				{
					return item.clip.length * item.speed;
				}
			}
			return 0f;
		}
	}
}
