using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public sealed class FirstPersonControlSharp : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CEnableSecondJump_003Ed__66 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FirstPersonControlSharp _003C_003E4__this;

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
		public _003CEnableSecondJump_003Ed__66(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(0.25f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.secondJumpEnabled = true;
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

	public bool isFirstPersonCamera = true;

	public GameObject thirdCameraPrefab;

	public ThirdPersonCamera thirdCamera;

	public Transform cameraPivot;

	public float forwardSpeed = 4f;

	public float runnerSpeed = 18.5f;

	public float jumpSpeed = 4.5f;

	public float inAirMultiplier = 0.25f;

	public Vector2 rotationSpeed = new Vector2(2f, 1f);

	public float tiltPositiveYAxis = 0.6f;

	public float tiltNegativeYAxis = 0.4f;

	public float tiltXAxisMinimum = 0.1f;

	public string myIp;

	public GameObject playerGameObject;

	public int typeAnim;

	private Transform thisTransform;

	public GameObject camPlayer;

	public CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump = true;

	public bool isMine;

	private Rect fireZone;

	private Rect jumpZone;

	private float timeUpdateAnim;

	public AudioClip jumpClip;

	private Player_move_c _moveC;

	public float gravityMultiplier = 1f;

	private Vector3 mousePosOld = Vector3.zero;

	private bool _invert;

	public bool ninjaJumpUsed = true;

	private bool isInet;

	private bool isMulti;

	private SkinName mySkinName;

	private const string newbieJumperAchievement = "NewbieJumperAchievement";

	private const int maxJumpCount = 10;

	private int oldJumpCount;

	private const string keyNinja = "NinjaJumpsCount";

	private int oldNinjaJumpsCount;

	private Vector3 _movement;

	private Vector2 _cameraMouseDelta;

	private bool jumpBy3dTouch;

	public bool onPlatform;

	public byte indexPlatform;

	public Vector3 addGravitation;

	public bool isRunner;

	private float currentSpeed;

	private Vector3 rinkMovement;

	private bool steptRink;

	private bool secondJumpEnabled = true;

	private bool jump
	{
		get
		{
			return Defs.isJump;
		}
		set
		{
			Defs.isJump = value;
		}
	}

	public Vector2 delta { get; set; }

	private void Awake()
	{
		isInet = Defs.isInet;
		isMulti = Defs.isMulti;
		isFirstPersonCamera = !GameConnect.isDeathEscape && !GameConnect.isSpeedrun;
	}

	private void Start()
	{
		character = GetComponent<CharacterController>();
		_moveC = playerGameObject.GetComponent<Player_move_c>();
		mySkinName = GetComponent<SkinName>();
		thisTransform = GetComponent<Transform>();
		if (isInet)
		{
			isMine = PhotonView.Get(this).isMine;
		}
		if (Defs.isMulti && !isMine)
		{
			camPlayer.SetActive(false);
			character.enabled = false;
		}
		else if (isFirstPersonCamera)
		{
			mySkinName.FPSplayerObject.SetActive(false);
		}
		else
		{
			camPlayer.SetActive(false);
			thirdCamera = UnityEngine.Object.Instantiate(thirdCameraPrefab, camPlayer.transform.position, camPlayer.transform.rotation).GetComponent<ThirdPersonCamera>();
			thirdCamera.cameraPivot = mySkinName.headObj.transform;
			thirdCamera.mouseX = thirdCamera.cameraPivot.rotation.eulerAngles.y;
			NickLabelController.currentCamera = thirdCamera.GetComponent<Camera>();
		}
		if (!isMulti || isMine)
		{
			HandleInvertCamUpdated();
			PauseNGUIController.InvertCamUpdated += HandleInvertCamUpdated;
			oldJumpCount = PlayerPrefs.GetInt("NewbieJumperAchievement", 0);
			oldNinjaJumpsCount = (Storager.hasKey("NinjaJumpsCount") ? Storager.getInt("NinjaJumpsCount") : 0);
		}
	}

	private void HandleInvertCamUpdated()
	{
		_invert = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
	}

	private void OnEndGame()
	{
		if (!isMulti || isMine)
		{
			if ((bool)JoystickController.leftJoystick)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
			}
			if ((bool)JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.gameObject.SetActive(false);
			}
		}
		base.enabled = false;
	}

	
	[PunRPC]
	private void setIp(string _ip)
	{
		myIp = _ip;
	}

	private Vector2 updateKeyboardControls()
	{
		int num = 0;
		int num2 = 0;
		if (Input.GetKey("w"))
		{
			num = 1;
		}
		if (Input.GetKey("s"))
		{
			num = -1;
		}
		if (Input.GetKey("a"))
		{
			num2 = -1;
		}
		if (Input.GetKey("d"))
		{
			num2 = 1;
		}
		return new Vector2(num2, num);
	}

	private void Jump()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowJump = 1000f;
			HintController.instance.HideHintByName("press_jump");
		}
		jump = true;
		canJump = false;
		if (!Defs.isJetpackEnabled && (!WeaponManager.sharedManager.myPlayerMoveC.isMechActive || !WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon)))
		{
			QuestMediator.NotifyJump();
			mySkinName.sendAnimJump();
		}
		if (!TrainingController.TrainingCompleted || (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android && BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer) || !Social.localUser.authenticated)
		{
			return;
		}
		int num = oldJumpCount + 1;
		if (oldJumpCount >= 10)
		{
			return;
		}
		oldJumpCount = num;
		if (num != 10)
		{
			return;
		}
		PlayerPrefs.SetInt("NewbieJumperAchievement", num);
		float newProgress = 100f;
		string achievementID = ((BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer || Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) ? "Jumper_id" : "CgkIr8rGkPIJEAIQAQ");
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			Social.ReportProgress(achievementID, newProgress, delegate(bool success)
			{
				string.Format("Newbie Jumper achievement progress {0:0.0}%: {1}", new object[2] { newProgress, success });
			});
		}
	}

	public void ResetController()
	{
		velocity = Vector3.zero;
	}

	private void Update()
	{
		if ((isMulti && !isMine) || mySkinName.playerMoveC.isKilled || !character.enabled || JoystickController.leftJoystick == null || JoystickController.rightJoystick == null)
		{
			return;
		}
		if (mySkinName.playerMoveC.isRocketJump && character.isGrounded)
		{
			mySkinName.playerMoveC.isRocketJump = false;
		}
		bool flag = false;
		if (isFirstPersonCamera)
		{
			_movement = thisTransform.TransformDirection(new Vector3(JoystickController.leftJoystick.value.x, 0f, JoystickController.leftJoystick.value.y));
		}
		else if (!JoystickController.leftJoystick.value.Equals(Vector2.zero) || isRunner)
		{
			if (GameConnect.isSpeedrun)
			{
				JoystickController.leftJoystick.value = Vector2.zero;
			}
			float num;
			if (JoystickController.leftJoystick.value.Equals(Vector2.zero))
			{
				num = 0f;
			}
			else
			{
				num = Vector2.Angle(Vector2.up, JoystickController.leftJoystick.value);
				if (num < 17f)
				{
					num = 0f;
				}
				else if (num > 163f)
				{
					num = 180f;
				}
				else if (num <= 90f)
				{
					num = (num - 17f) * 1.2328767f;
				}
				else if (num > 90f)
				{
					num = 90f + (num - 90f) * 1.2328767f;
				}
				if (JoystickController.leftJoystick.value.x < 0f)
				{
					num *= -1f;
				}
			}
			float num2 = Quaternion.Angle(thisTransform.rotation, Quaternion.Euler(new Vector3(0f, thirdCamera.transform.rotation.eulerAngles.y + num, 0f)));
			float t = 1f;
			if (num2 > 1080f * Time.deltaTime)
			{
				t = 1080f * Time.deltaTime / num2;
				flag = true;
			}
			thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, Quaternion.Euler(new Vector3(0f, thirdCamera.transform.rotation.eulerAngles.y + num, 0f)), t);
			_movement = thisTransform.TransformDirection(new Vector3(0f, 0f, 1f));
		}
		if (GameConnect.isMiniGame && !MiniGamesController.Instance.isGo)
		{
			_movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining < TrainingState.TapToMove)
		{
			_movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToMove && _movement != Vector3.zero)
		{
			TrainingController.isNextStep = TrainingState.TapToMove;
		}
		if (character.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space) && !WeaponManager.sharedManager.myPlayerMoveC.showChat)
            {
                JoystickController.leftTouchPad.isJumpPressed = true;
                JoystickController.rightJoystick.jumpPressed = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && !WeaponManager.sharedManager.myPlayerMoveC.showChat)
            {
                JoystickController.leftTouchPad.isJumpPressed = false;
                JoystickController.rightJoystick.jumpPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                JoystickController.leftTouchPad.isJumpPressed = false;
                JoystickController.rightJoystick.jumpPressed = false;
            }
        }
		_movement.y = 0f;
		_movement.Normalize();
		Vector2 vector = new Vector2(Mathf.Abs(JoystickController.leftJoystick.value.x), Mathf.Abs(JoystickController.leftJoystick.value.y));
		if (JoystickController.leftTouchPad.isShooting && JoystickController.leftTouchPad.isActiveFireButton)
		{
			vector = new Vector2(0f, 0f);
		}
		float num3;
		if (!isRunner)
		{
			currentSpeed = forwardSpeed;
			num3 = ((vector.y > vector.x) ? vector.y : vector.x);
			if (flag)
			{
				num3 = 0f;
			}
		}
		else
		{
			if (currentSpeed < runnerSpeed)
			{
				currentSpeed += (runnerSpeed - forwardSpeed) * Time.deltaTime;
			}
			else
			{
				currentSpeed = runnerSpeed;
			}
			num3 = 1f;
		}
		_movement *= currentSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * num3;
		bool flag2 = Defs.isJetpackEnabled || (WeaponManager.sharedManager.myPlayerMoveC.isMechActive && WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon));
		if (character.isGrounded)
		{
			if (EffectsController.NinjaJumpEnabled)
			{
				ninjaJumpUsed = false;
			}
			if (!canJump && cameraPivot.GetComponent<UITweener>() != null)
			{
				cameraPivot.GetComponent<UITweener>().PlayForward();
			}
			canJump = true;
			jump = false;
			TouchPadController rightJoystick = JoystickController.rightJoystick;
			if (canJump && (rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed))
			{
				if (!flag2)
				{
					rightJoystick.jumpPressed = false;
				}
				Jump();
			}
			if (jump)
			{
				secondJumpEnabled = false;
				if (!JoystickController.leftTouchPad.isJumpPressed)
				{
					StartCoroutine(EnableSecondJump());
				}
				else
				{
					jumpBy3dTouch = true;
				}
				velocity = Vector3.zero;
				velocity.y = jumpSpeed * EffectsController.JumpModifier;
			}
		}
		else
		{
			if (!JoystickController.leftTouchPad.isJumpPressed && jumpBy3dTouch)
			{
				secondJumpEnabled = true;
				jumpBy3dTouch = false;
			}
			if (jump && mySkinName.interpolateScript.myAnim == 0 && !flag2)
			{
				mySkinName.sendAnimJump();
			}
			TouchPadController rightJoystick2 = JoystickController.rightJoystick;
			TouchPadInJoystick leftTouchPad = JoystickController.leftTouchPad;
			if ((rightJoystick2.jumpPressed || leftTouchPad.isJumpPressed) && ((EffectsController.NinjaJumpEnabled && !ninjaJumpUsed && secondJumpEnabled) || flag2))
			{
				if (!flag2)
				{
					QuestMediator.NotifyJump();
					RegisterNinjAchievment();
				}
				ninjaJumpUsed = true;
				canJump = false;
				if (!flag2)
				{
					mySkinName.sendAnimJump();
				}
				if (Mathf.Abs(character.velocity.y) > 0.05f)
				{
					velocity.y = 1.1f * (jumpSpeed * EffectsController.JumpModifier);
				}
				else
				{
					velocity.y = -1f * (Physics.gravity.y * gravityMultiplier + addGravitation.y) * (1f + Time.deltaTime);
				}
			}
			else if (Mathf.Abs(character.velocity.y) <= 0.05f)
			{
				velocity.y = -0.9f * (Physics.gravity.y * gravityMultiplier + addGravitation.y);
			}
			if (!flag2)
			{
				rightJoystick2.jumpPressed = false;
			}
			if (Mathf.Abs(character.velocity.y) > 0.05f)
			{
				velocity.y += (Physics.gravity.y * gravityMultiplier + addGravitation.y) * Time.deltaTime;
			}
		}
		if (WeaponManager.sharedManager.myPlayerMoveC.IsPlayerEffectActive(Player_move_c.PlayerEffect.rocketFly))
		{
			velocity.y = 25f;
		}
		_movement += velocity;
		_movement += Physics.gravity * gravityMultiplier + addGravitation;
		if (Defs.isMulti && !GameConnect.isCOOP && !WeaponManager.sharedManager.myPlayerMoveC.isImmortality)
		{
			Vector3 zero = Vector3.zero;
			bool flag3 = false;
			for (int i = 0; i < Initializer.singularities.Count; i++)
			{
				if (!Initializer.singularities[i].owner.Equals(WeaponManager.sharedManager.myPlayerMoveC) && (!GameConnect.isTeamRegim || WeaponManager.sharedManager.myPlayerMoveC.myCommand != Initializer.singularities[i].owner.myCommand))
				{
					SingularityHole singularityHole = Initializer.singularities[i];
					Vector3 vector2 = singularityHole.transform.position - base.transform.position;
					float force = singularityHole.GetForce(vector2.sqrMagnitude);
					if (force > 0f)
					{
						zero += vector2.normalized * force;
					}
					if (force < 0f)
					{
						flag3 = true;
					}
				}
			}
			if (zero.sqrMagnitude >= 0f)
			{
				if (zero.y > 0f || flag3)
				{
					_movement.y = 0f;
					velocity.y = 0f;
				}
				_movement += zero;
			}
			for (int j = 0; j < Initializer.players.Count; j++)
			{
				if (Initializer.players[j].IsPlayerEffectActive(Player_move_c.PlayerEffect.attrackPlayer))
				{
					Vector3 normalized = (Initializer.players[j].myPlayerTransform.position + Initializer.players[j].myPlayerTransform.forward * 1.2f - base.transform.position).normalized;
					_movement += normalized * 5f;
				}
			}
		}
		_movement *= Time.deltaTime;
		timeUpdateAnim -= Time.deltaTime;
		if (timeUpdateAnim < 0f)
		{
			if (character.isGrounded)
			{
				timeUpdateAnim = 0.5f;
				if (new Vector2(_movement.x, _movement.z).sqrMagnitude > 0f)
				{
					_moveC.WalkAnimation();
				}
				else
				{
					_moveC.IdleAnimation();
				}
			}
			else
			{
				_moveC.WalkAnimation();
			}
		}
		if ((GameConnect.isSpeedrun || GameConnect.isDeathEscape) && mySkinName.thirdPersonAnimation != null)
		{
			mySkinName.thirdPersonAnimation.isGrounded = character.isGrounded;
			mySkinName.thirdPersonAnimation.VelocityMagnitude = new Vector3(character.velocity.x, 0f, character.velocity.z).magnitude * (GameConnect.isSpeedrun ? 0.45f : 1f);
		}
		Update2();
	}

	private void Update2()
	{
		if (!character.enabled)
		{
			return;
		}
		if (!mySkinName.onRink)
		{
			if (mySkinName.onConveyor)
			{
				_movement += mySkinName.conveyorDirection * Time.deltaTime;
			}
			character.Move(_movement);
			_movement = Vector3.zero;
			steptRink = false;
		}
		else
		{
			if (!steptRink)
			{
				rinkMovement = _movement;
				steptRink = true;
			}
			rinkMovement = Vector3.MoveTowards(rinkMovement, _movement, 0.068f * Time.deltaTime);
			rinkMovement.y = _movement.y;
			character.Move(rinkMovement);
		}
		if (character.isGrounded)
		{
			velocity = Vector3.zero;
		}
		else
		{
			if (mySkinName.onRink)
			{
				rinkMovement = _movement;
			}
			mySkinName.onConveyor = false;
		}
		Vector2 delta = GrabCameraInputDelta();
		if (Device.isPixelGunLow && Defs.isTouchControlSmoothDump)
		{
			MoveCamera(delta);
		}
		if (Defs.isMulti && CameraSceneController.sharedController.killCamController.enabled)
		{
			CameraSceneController.sharedController.killCamController.UpdateMouseX();
		}
	}

	public void MoveCamera(Vector2 delta)
	{
		this.delta = delta;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.SwipeToRotate && delta != Vector2.zero)
		{
			TrainingController.isNextStep = TrainingState.SwipeToRotate;
		}
		float sensitivity = Defs.Sensitivity;
		float num = 1f;
		if (_moveC != null)
		{
			num *= (_moveC.isZooming ? 0.2f : 1f);
		}
		if (isFirstPersonCamera)
		{
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.ApplyDeltaTo(delta, thisTransform, cameraPivot.transform, sensitivity * num, _invert);
			}
		}
		else
		{
			thirdCamera.SetDeltaRotate(delta, sensitivity, _invert);
		}
	}

	private Vector2 GrabCameraInputDelta()
	{
		Vector2 result = Vector2.zero;
		TouchPadController rightJoystick = JoystickController.rightJoystick;
		if (rightJoystick != null)
		{
			result = rightJoystick.GrabDeltaPosition();
		}
		return result;
	}

	private void RegisterNinjAchievment()
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		int num = oldNinjaJumpsCount + 1;
		if (oldNinjaJumpsCount < 50)
		{
			Storager.setInt("NinjaJumpsCount", num);
		}
		oldNinjaJumpsCount = num;
		if (Storager.hasKey("ParkourNinjaAchievementCompleted") || num < 50)
		{
			return;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQAw", 1, delegate(bool success)
			{
				UnityEngine.Debug.Log("Achievement Parkour Ninja incremented: " + success);
			});
		}
		Storager.setInt("ParkourNinjaAchievementCompleted", 1);
	}

	private IEnumerator EnableSecondJump()
	{
		yield return new WaitForSeconds(0.25f);
		secondJumpEnabled = true;
	}

	private void OnDestroy()
	{
		if (!isMulti || isMine)
		{
			PauseNGUIController.InvertCamUpdated -= HandleInvertCamUpdated;
			if (thirdCamera != null)
			{
				UnityEngine.Object.Destroy(thirdCamera.gameObject);
			}
		}
	}

	public void OnPlatformEnter(byte _indexPlatform)
	{
		base.transform.parent = PlatformsList.instance.platforms[_indexPlatform].transform;
		onPlatform = true;
		indexPlatform = _indexPlatform;
	}

	public void OnPlatformExit(int _indexPlatform)
	{
		base.transform.parent = null;
		onPlatform = false;
	}
}
