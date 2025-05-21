using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public sealed class SkinName : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetCapeCurrentModel_003Ed__63 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string cape;

		private LoadAsyncTool.ObjectRequest _003Crequest_003E5__1;

		public SkinName _003C_003E4__this;

		public Texture capeTex;

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
		public _003CSetCapeCurrentModel_003Ed__63(int _003C_003E1__state)
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
				if (Device.isPixelGunLow)
				{
					return false;
				}
				_003Crequest_003E5__1 = LoadAsyncTool.Get("Capes/" + cape);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			GameObject gameObject = _003Crequest_003E5__1.asset as GameObject;
			if (gameObject == null)
			{
				return false;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			Transform transform = gameObject2.transform;
			transform.parent = _003C_003E4__this.capesPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			if (cape.Equals("cape_Custom"))
			{
				gameObject2.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
				Player_move_c.SetTextureRecursivelyFrom(gameObject2, capeTex, new GameObject[0]);
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
	internal sealed class _003CSetArmorModel_003Ed__68 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SkinName _003C_003E4__this;

		private LoadAsyncTool.ObjectRequest _003Crequest_003E5__1;

		public bool invisible;

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
		public _003CSetArmorModel_003Ed__68(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			GameObject gameObject;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				gameObject = null;
				if (Device.isPixelGunLow && !string.IsNullOrEmpty(_003C_003E4__this.currentArmor) && _003C_003E4__this.currentArmor != Defs.ArmorNewNoneEqupped)
				{
					gameObject = Resources.Load<GameObject>("Armor_Low");
					goto IL_00b5;
				}
				_003Crequest_003E5__1 = LoadAsyncTool.Get("Armor/" + _003C_003E4__this.currentArmor);
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			gameObject = _003Crequest_003E5__1.asset as GameObject;
			_003Crequest_003E5__1 = null;
			goto IL_00b5;
			IL_00b5:
			if (gameObject == null)
			{
				return false;
			}
			Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
			if (Device.isPixelGunLow)
			{
				try
				{
					transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material = Resources.Load<Material>("LowPolyArmorMaterials/" + _003C_003E4__this.currentArmor + "_low");
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception setting material for low armor: " + _003C_003E4__this.currentArmor + "   exception: " + ex);
				}
			}
			if (invisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
			}
			ArmorRefs component = transform.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (_003C_003E4__this.playerMoveC != null && _003C_003E4__this.playerMoveC.transform.childCount > 0)
				{
					WeaponSounds myCurrentWeaponSounds = _003C_003E4__this.playerMoveC.myCurrentWeaponSounds;
					component.leftBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.LeftArmorHand;
					component.rightBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.RightArmorHand;
				}
				transform.parent = _003C_003E4__this.armorPoint.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = new Vector3(1f, 1f, 1f);
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
	internal sealed class _003CSetBootsModel_003Ed__71 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private LoadAsyncTool.ObjectRequest _003Crequest_003E5__1;

		public SkinName _003C_003E4__this;

		public bool isInvisible;

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
		public _003CSetBootsModel_003Ed__71(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				if (Device.isPixelGunLow)
				{
					goto IL_0141;
				}
				_003Crequest_003E5__1 = LoadAsyncTool.Get("Boots/BootPrefab");
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			GameObject gameObject = _003Crequest_003E5__1.asset as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject);
				gameObject2.transform.SetParent(_003C_003E4__this.LeftBootPoint.transform, false);
				gameObject3.transform.SetParent(_003C_003E4__this.RightBootPoint.transform, false);
				gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
				gameObject2.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[_003C_003E4__this.currentBoots]);
				gameObject3.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[_003C_003E4__this.currentBoots]);
				if (isInvisible)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
					ShopNGUIController.SetRenderersVisibleFromPoint(gameObject3.transform, false);
				}
			}
			_003Crequest_003E5__1 = null;
			goto IL_0141;
			IL_0141:
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
	internal sealed class _003CSetMaskModel_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SkinName _003C_003E4__this;

		private LoadAsyncTool.ObjectRequest _003Crequest_003E5__1;

		public bool isInvisible;

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
		public _003CSetMaskModel_003Ed__74(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				if (Device.isPixelGunLow)
				{
					goto IL_00db;
				}
				_003Crequest_003E5__1 = LoadAsyncTool.Get("Masks/" + _003C_003E4__this.currentMask);
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			GameObject gameObject = _003Crequest_003E5__1.asset as GameObject;
			if (gameObject != null)
			{
				Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
				transform.parent = _003C_003E4__this.maskPoint.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				if (isInvisible)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
				}
			}
			_003Crequest_003E5__1 = null;
			goto IL_00db;
			IL_00db:
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
	internal sealed class _003CSetHatModel_003Ed__77 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SkinName _003C_003E4__this;

		private LoadAsyncTool.ObjectRequest _003Crequest_003E5__1;

		public bool invisible;

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
		public _003CSetHatModel_003Ed__77(int _003C_003E1__state)
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
				if (Device.isPixelGunLow)
				{
					return false;
				}
				_003Crequest_003E5__1 = LoadAsyncTool.Get("Hats/" + _003C_003E4__this.currentHat);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003Crequest_003E5__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			GameObject gameObject = _003Crequest_003E5__1.asset as GameObject;
			if (gameObject == null)
			{
				return false;
			}
			Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
			transform.parent = _003C_003E4__this.hatsPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			if (invisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
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
	internal sealed class _003C_SetAndResetImpactedByTrampoline_003Ed__97 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SkinName _003C_003E4__this;

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
		public _003C_SetAndResetImpactedByTrampoline_003Ed__97(int _003C_003E1__state)
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
				_003C_003E4__this._impactedByTramp = true;
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this._impactedByTramp = false;
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

	[NonSerialized]
	public string currentHat;

	[NonSerialized]
	public string currentArmor;

	[NonSerialized]
	public string currentCape;

	[NonSerialized]
	public Texture currentCapeTex;

	[NonSerialized]
	public string currentBoots;

	[NonSerialized]
	public string currentMask;

	[NonSerialized]
	public string currentPet;

	[NonSerialized]
	public string currentGadgetSupport;

	[NonSerialized]
	public string currentGadgetTools;

	[NonSerialized]
	public string currentGadgetThrowing;

	[NonSerialized]
	public bool _currentIsWearInvisible;

	public Transform onGroundEffectsPoint;

	public GameObject playerGameObject;

	public Player_move_c playerMoveC;

	public string skinName;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public GameObject armorPoint;

	public GameObject maskPoint;

	public GameObject LeftBootPoint;

	public GameObject RightBootPoint;

	public string NickName;

	public GameObject camPlayer;

	public GameObject headObj;

	public GameObject bodyLayer;

	public CharacterController character;

	public PhotonView photonView;

	public PixelView pixelView;

	public int typeAnim;

	public WeaponManager _weaponManager;

	public bool isInet;

	public bool isLocal;

	public bool isMine;

	public bool isMulti;

	public AudioClip walkAudio;

	public AudioClip jumpAudio;

	public AudioClip jumpDownAudio;

	public AudioClip walkMechBear;

	public AudioClip[] jumpSpeedrunAudio;

	public bool isPlayDownSound;

	public GameObject FPSplayerObject;

	public PlayerSynchStream interpolateScript;

	private bool _impactedByTramp;

	public bool onRink;

	public bool onConveyor;

	public Vector3 conveyorDirection;

	private ImpactReceiverTrampoline _irt;

	private bool _armorPopularityCacheIsDirty;

	public FirstPersonControlSharp firstPersonControl;

	private bool wearDisabled;

	public int currentAnim;

	public AreaVisitMonitor areaVisitMonitor;

	private bool _playWalkSound;

	public ThirdPersonAnimationWrapper thirdPersonAnimation;

	private AudioSource _audio;

	public AudioClip walkMech
	{
		get
		{
			if (!GameConnect.isDaterRegim)
			{
				if (!(playerMoveC.currentMech != null))
				{
					return null;
				}
				return playerMoveC.currentMech.stepSound;
			}
			return walkMechBear;
		}
		set
		{
		}
	}

	public void MoveCamera(Vector2 delta)
	{
		firstPersonControl.MoveCamera(delta);
	}

	public void BlockFirstPersonController()
	{
		firstPersonControl.enabled = false;
	}

	public void sendAnimJump()
	{
		int num = ((!character.isGrounded) ? 2 : 0);
		if (interpolateScript.myAnim == num)
		{
			return;
		}
		if (Defs.isSoundFX && num == 2 && !EffectsController.WeAreStealth)
		{
			if ((GameConnect.isSpeedrun || GameConnect.isDeathEscape) && jumpSpeedrunAudio != null && jumpSpeedrunAudio.Length != 0)
			{
				NGUITools.PlaySound(jumpSpeedrunAudio[UnityEngine.Random.Range(0, jumpSpeedrunAudio.Length)]);
			}
			NGUITools.PlaySound(jumpAudio);
		}
		interpolateScript.myAnim = num;
		interpolateScript.weAreSteals = EffectsController.WeAreStealth;
		SetAnim(num, EffectsController.WeAreStealth);
	}

	[PunRPC]
	[RPC]
	public void SetAnim(int _typeAnim, bool stealth)
	{
		string animation = "Idle";
		currentAnim = _typeAnim;
		switch (_typeAnim)
		{
		case 0:
			animation = "Idle";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
			_playWalkSound = false;
			break;
		case 1:
			animation = "Walk";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 2:
			animation = "Jump";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
			break;
		}
		switch (_typeAnim)
		{
		case 4:
			animation = "Walk_Back";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 5:
			animation = "Walk_Left";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 6:
			animation = "Walk_Right";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		}
		if (_typeAnim == 7)
		{
			animation = "Jetpack_Run_Front";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 8)
		{
			animation = "Jetpack_Run_Back";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 9)
		{
			animation = "Jetpack_Run_Left";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 10)
		{
			animation = "Jetpack_Run_Righte";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 11)
		{
			animation = "Jetpack_Idle";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (isMulti && !isMine && GameConnect.isDeathEscape && thirdPersonAnimation != null)
		{
			thirdPersonAnimation.isGrounded = _typeAnim != 2;
			thirdPersonAnimation.VelocityMagnitude = ((_typeAnim == 1 || _typeAnim == 4 || _typeAnim == 5 || _typeAnim == 6) ? firstPersonControl.forwardSpeed : 0f);
		}
		if ((!isMulti || isMine) && firstPersonControl.isFirstPersonCamera)
		{
			return;
		}
		if (playerMoveC.isMechActive || playerMoveC.isBearActive)
		{
			if (GameConnect.isDaterRegim)
			{
				playerMoveC.mechBearBodyAnimation.Play(animation);
			}
			else if (playerMoveC.currentMech != null)
			{
				playerMoveC.currentMech.bodyAnimation.Play(animation);
			}
		}
		FPSplayerObject.GetComponent<Animation>().Play(animation);
		if (capesPoint.transform.childCount > 0 && capesPoint.transform.GetChild(0).GetComponent<Animation>().GetClip(animation) != null)
		{
			capesPoint.transform.GetChild(0).GetComponent<Animation>().Play(animation);
		}
	}

	[RPC]
	[PunRPC]
	private void SetAnim(int _typeAnim)
	{
		SetAnim(_typeAnim, true);
	}

	[PunRPC]
	private void setCapeCustomRPC(byte[] _skinByte)
	{
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width == 12 && texture2D.height == 16)
		{
			currentCapeTex = texture2D;
			currentCape = "cape_Custom";
			SetCapeModel(currentCape, currentCapeTex, _currentIsWearInvisible);
		}
	}

	[RPC]
	private void setCapeCustomRPCLocal(string str)
	{
		byte[] data = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width == 12 && texture2D.height == 16)
		{
			SetCapeModel("cape_Custom", texture2D, _currentIsWearInvisible);
		}
	}

	private IEnumerator SetCapeCurrentModel(string cape, Texture capeTex)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Capes/" + cape);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject gameObject = request.asset as GameObject;
		if (!(gameObject == null))
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			Transform obj = gameObject2.transform;
			obj.parent = capesPoint.transform;
			obj.localPosition = Vector3.zero;
			obj.localRotation = Quaternion.identity;
			if (cape.Equals("cape_Custom"))
			{
				gameObject2.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
				Player_move_c.SetTextureRecursivelyFrom(gameObject2, capeTex, new GameObject[0]);
			}
		}
	}

	private void UpdateEffectsOnPlayerMoveC()
	{
		if (playerMoveC != null)
		{
			playerMoveC.UpdateEffectsForCurrentWeapon(currentCape, currentMask, currentHat);
		}
		else
		{
			UnityEngine.Debug.LogError("playerMoveC.UpdateEffectsForCurrentWeapon playerMoveC == null");
		}
	}

	[PunRPC]
	[RPC]
	private void setCapeRPC(string _currentCape)
	{
		SetCapeModel(_currentCape, null, _currentIsWearInvisible);
	}

	private void SetCapeModel(string cape, Texture tex, bool isInvisible)
	{
		currentCapeTex = tex;
		currentCape = cape;
		if (capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(capesPoint.transform.GetChild(i).gameObject);
			}
		}
		UpdateEffectsOnPlayerMoveC();
		if (!isInvisible && !(cape == ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.CapesCategory)))
		{
			StartCoroutine(SetCapeCurrentModel(cape, tex));
		}
	}

	[PunRPC]
	[RPC]
	private void SetArmorVisInvisibleRPC(string _currentArmor, bool _isInviseble)
	{
		if (armorPoint.transform.childCount > 0)
		{
			for (int i = 0; i < armorPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(armorPoint.transform.GetChild(i).gameObject);
			}
		}
		currentArmor = _currentArmor;
		if (!_isInviseble)
		{
			StartCoroutine(SetArmorModel(_isInviseble));
		}
	}

	private IEnumerator SetArmorModel(bool invisible)
	{
		GameObject gameObject;
		if (Device.isPixelGunLow && !string.IsNullOrEmpty(currentArmor) && currentArmor != Defs.ArmorNewNoneEqupped)
		{
			gameObject = Resources.Load<GameObject>("Armor_Low");
		}
		else
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Armor/" + currentArmor);
			while (!request.isDone)
			{
				yield return null;
			}
			gameObject = request.asset as GameObject;
		}
		if (gameObject == null)
		{
			yield break;
		}
		Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
		if (Device.isPixelGunLow)
		{
			try
			{
				transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material = Resources.Load<Material>("LowPolyArmorMaterials/" + currentArmor + "_low");
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Exception setting material for low armor: " + currentArmor + "   exception: " + ex);
			}
		}
		if (invisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
		}
		ArmorRefs component = transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (playerMoveC != null && playerMoveC.transform.childCount > 0)
			{
				WeaponSounds myCurrentWeaponSounds = playerMoveC.myCurrentWeaponSounds;
				component.leftBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.LeftArmorHand;
				component.rightBone.GetComponent<SetPosInArmor>().target = myCurrentWeaponSounds.RightArmorHand;
			}
			transform.parent = armorPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	[PunRPC]
	[RPC]
	private void setBootsRPC(string _currentBoots)
	{
		SetBoots(_currentBoots, _currentIsWearInvisible);
	}

	private void SetBoots(string itemId, bool isInvisible)
	{
		if (LeftBootPoint.transform.childCount > 0)
		{
			UnityEngine.Object.Destroy(LeftBootPoint.transform.GetChild(0).gameObject);
		}
		if (RightBootPoint.transform.childCount > 0)
		{
			UnityEngine.Object.Destroy(RightBootPoint.transform.GetChild(0).gameObject);
		}
		currentBoots = itemId;
		if (!currentBoots.IsNullOrEmpty() && !currentBoots.Equals(Defs.BootsNoneEqupped) && !isInvisible)
		{
			StartCoroutine(SetBootsModel(isInvisible));
		}
	}

	private IEnumerator SetBootsModel(bool isInvisible)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Boots/BootPrefab");
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject gameObject = request.asset as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.SetParent(LeftBootPoint.transform, false);
			gameObject3.transform.SetParent(RightBootPoint.transform, false);
			gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
			gameObject2.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[currentBoots]);
			gameObject3.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[currentBoots]);
			if (isInvisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject3.transform, false);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetMaskRPC(string _currentMask)
	{
		SetMask(_currentMask, _currentIsWearInvisible);
	}

	private void SetMask(string itemId, bool isInvisible)
	{
		if (maskPoint.transform.childCount > 0)
		{
			for (int i = 0; i < maskPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(maskPoint.transform.GetChild(i).gameObject);
			}
		}
		currentMask = itemId;
		UpdateEffectsOnPlayerMoveC();
		if (!isInvisible)
		{
			StartCoroutine(SetMaskModel(isInvisible));
		}
	}

	private IEnumerator SetMaskModel(bool isInvisible)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Masks/" + currentMask);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject gameObject = request.asset as GameObject;
		if (gameObject != null)
		{
			Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
			transform.parent = maskPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			if (isInvisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetHatWithInvisebleRPC(string _currentHat, bool _isHatInviseble)
	{
		SetHat(_currentHat, _isHatInviseble || _currentIsWearInvisible);
	}

	private void SetHat(string itemId, bool isInvisible)
	{
		if (hatsPoint.transform.childCount > 0)
		{
			for (int i = 0; i < hatsPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(hatsPoint.transform.GetChild(i).gameObject);
			}
		}
		currentHat = itemId;
		UpdateEffectsOnPlayerMoveC();
		if (!isInvisible)
		{
			StartCoroutine(SetHatModel(isInvisible));
		}
	}

	private IEnumerator SetHatModel(bool invisible)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Hats/" + currentHat);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject gameObject = request.asset as GameObject;
		if (!(gameObject == null))
		{
			Transform transform = UnityEngine.Object.Instantiate(gameObject).transform;
			transform.parent = hatsPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			if (invisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(transform, false);
			}
		}
	}

	private void Awake()
	{
		isLocal = !Defs.isInet;
		firstPersonControl = GetComponent<FirstPersonControlSharp>();
		_audio = GetComponent<AudioSource>();
		photonView = PhotonView.Get(this);
		wearDisabled = GameConnect.isHunger || GameConnect.isSpleef || GameConnect.isDeathEscape || GameConnect.isSurvival || GameConnect.isCOOP;
		if (GameConnect.isSpeedrun || GameConnect.isDeathEscape)
		{
			CreateThirdPersonPlayer();
			FPSplayerObject.SetActive(false);
		}
	}

	private void CreateThirdPersonPlayer()
	{
		GameObject original = Resources.Load("Player/FPS_Player_runner") as GameObject;
		thirdPersonAnimation = UnityEngine.Object.Instantiate(original, base.transform).GetComponent<ThirdPersonAnimationWrapper>();
		thirdPersonAnimation.transform.localPosition = playerMoveC.fpsPlayerBody.transform.localPosition;
		thirdPersonAnimation.visObj.lerpScript = interpolateScript;
	}

	private void Start()
	{
		_weaponManager = WeaponManager.sharedManager;
		playerMoveC = playerGameObject.GetComponent<Player_move_c>();
		character = base.transform.GetComponent<CharacterController>();
		isMulti = Defs.isMulti;
		pixelView = GetComponent<PixelView>();
		if ((bool)photonView && photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		isInet = Defs.isInet;
		if (isInet)
		{
			isMine = photonView.isMine;
		}
		if (!Defs.isMulti || (Defs.isInet && photonView.isMine))
		{
			base.gameObject.layer = 11;
			bodyLayer.layer = 11;
			headObj.layer = 11;
		}
		if (isMine)
		{
			SetWearVisible();
			SetCape();
			SetHat();
			SetBoots();
			SetArmor();
			SetMask();
			SetPet();
			SetGadgetes();
		}
		if (areaVisitMonitor != null)
		{
			areaVisitMonitor.gameObject.SetActive(isMine);
		}
	}

	private void OnDestroy()
	{
		if (_armorPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveArmorPopularity();
			_armorPopularityCacheIsDirty = false;
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void SetMask(PhotonPlayer player = null)
	{
		if (wearDisabled)
		{
			return;
		}
		string text = (currentMask = Storager.getString("MaskEquippedSN"));
		UpdateEffectsOnPlayerMoveC();
		if (Defs.isMulti && isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetMaskRPC", PhotonTargets.Others, text);
			}
			else
			{
				photonView.RPC("SetMaskRPC", player, text);
			}
		}
	}

	public void SetCape(PhotonPlayer player = null)
	{
		if (wearDisabled)
		{
			return;
		}
		string text = (currentCape = Storager.getString(Defs.CapeEquppedSN));
		UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (!text.Equals("cape_Custom"))
		{
			if (isInet)
			{
				if (player == null)
				{
					photonView.RPC("setCapeRPC", PhotonTargets.Others, text);
				}
				else
				{
					photonView.RPC("setCapeRPC", player, text);
				}
			}
		}
		else
		{
			if (!text.Equals("cape_Custom"))
			{
				return;
			}
			Texture2D capeUserTexture = SkinsController.capeUserTexture;
			byte[] array = capeUserTexture.EncodeToPNG();
			if (capeUserTexture.width == 12 && capeUserTexture.height == 16 && isInet)
			{
				if (player == null)
				{
					photonView.RPC("setCapeCustomRPC", PhotonTargets.Others, array);
				}
				else
				{
					photonView.RPC("setCapeCustomRPC", player, array);
				}
			}
		}
	}

	public void SetArmor(PhotonPlayer player = null)
	{
		if (wearDisabled || GameConnect.isDaterRegim)
		{
			return;
		}
		string text = (currentArmor = Storager.getString(Defs.ArmorNewEquppedSN));
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowArmor;
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetArmorVisInvisibleRPC", PhotonTargets.Others, text, flag);
			}
			else
			{
				photonView.RPC("SetArmorVisInvisibleRPC", player, text, flag);
			}
		}
		IncrementArmorPopularity(text);
	}

	public void SetBoots(PhotonPlayer player = null)
	{
		string text = (currentBoots = Storager.getString(Defs.BootsEquppedSN));
		if (wearDisabled)
		{
			currentBoots = string.Empty;
		}
		else if (Defs.isMulti && isInet)
		{
			if (player == null)
			{
				photonView.RPC("setBootsRPC", PhotonTargets.Others, text);
			}
			else
			{
				photonView.RPC("setBootsRPC", player, text);
			}
		}
	}

	public void SetPet(PhotonPlayer player = null)
	{
		currentPet = Singleton<PetsManager>.Instance.GetEqipedPetId();
		if (Defs.isMulti && Defs.isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetPetRPC", PhotonTargets.Others, currentPet);
			}
			else
			{
				photonView.RPC("SetPetRPC", player, currentPet);
			}
		}
	}

	public void SetWearVisible(PhotonPlayer player = null)
	{
		if (player == null && _currentIsWearInvisible == !ShopNGUIController.ShowWear)
		{
			return;
		}
		_currentIsWearInvisible = !ShopNGUIController.ShowWear;
		if (!wearDisabled && Defs.isMulti && (!isInet || PhotonNetwork.connected) && isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetWearIsInvisibleRPC", PhotonTargets.Others, _currentIsWearInvisible);
			}
			else
			{
				photonView.RPC("SetWearIsInvisibleRPC", player, _currentIsWearInvisible);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetWearIsInvisibleRPC(bool isInvisible)
	{
		_currentIsWearInvisible = isInvisible;
		SetMask(currentMask, _currentIsWearInvisible);
		SetBoots(currentBoots, _currentIsWearInvisible);
		SetCapeModel(currentCape, currentCapeTex, _currentIsWearInvisible);
		SetHat(currentHat, _currentIsWearInvisible);
	}

	[RPC]
	[PunRPC]
	private void SetPetRPC(string _currentPet)
	{
		currentPet = _currentPet;
	}

	public void SetGadgetes(PhotonPlayer player = null)
	{
		string text = string.Empty;
		Gadget value = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Support, out value))
		{
			text = value.Info.Id;
		}
		string text2 = string.Empty;
		Gadget value2 = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Throwing, out value2))
		{
			text2 = value2.Info.Id;
		}
		string text3 = string.Empty;
		Gadget value3 = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Tools, out value3))
		{
			text3 = value3.Info.Id;
		}
		currentGadgetSupport = text;
		currentGadgetThrowing = text2;
		currentGadgetTools = text3;
		if (Defs.isMulti && isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetGadgetesRPC", PhotonTargets.Others, currentGadgetSupport, currentGadgetThrowing, currentGadgetTools);
			}
			else
			{
				photonView.RPC("SetGadgetesRPC", player, currentGadgetSupport, currentGadgetThrowing, currentGadgetTools);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetGadgetesRPC(string _currentGadgetSupport, string _currentGadgetTrowing, string _currentGadgetTools)
	{
		currentGadgetSupport = _currentGadgetSupport;
		currentGadgetThrowing = _currentGadgetTrowing;
		currentGadgetTools = _currentGadgetTools;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if ((bool)photonView && photonView.isMine)
		{
			SetWearVisible(player);
			SetHat(player);
			SetCape(player);
			SetBoots(player);
			SetArmor(player);
			SetMask(player);
			SetPet(player);
			SetGadgetes(player);
		}
	}

	public void SetHat(PhotonPlayer player = null)
	{
		if (wearDisabled)
		{
			return;
		}
		string text = Storager.getString(Defs.HatEquppedSN);
		if (text != null && (GameConnect.isHunger || GameConnect.isDaterRegim) && !Wear.NonArmorHat(text))
		{
			text = "hat_NoneEquipped";
		}
		currentHat = text;
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowHat && !Wear.NonArmorHat(text);
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetHatWithInvisebleRPC", PhotonTargets.Others, text, flag);
			}
			else
			{
				photonView.RPC("SetHatWithInvisebleRPC", player, text, flag);
			}
		}
	}

	private void Update()
	{
		if ((isMulti && isMine) || !isMulti)
		{
			if (playerMoveC.isKilled)
			{
				isPlayDownSound = false;
			}
			int num = 0;
			if ((character.velocity.y > 0.01f || character.velocity.y < -0.01f) && !character.isGrounded && !Defs.isJetpackEnabled)
			{
				num = 2;
			}
			else if (character.velocity.x != 0f || character.velocity.z != 0f)
			{
				if (character.isGrounded)
				{
					if (firstPersonControl.isFirstPersonCamera)
					{
						float x = JoystickController.leftJoystick.value.x;
						float y = JoystickController.leftJoystick.value.y;
						num = ((Mathf.Abs(y) >= Mathf.Abs(x)) ? ((y >= 0f) ? 1 : 4) : ((!(x >= 0f)) ? 5 : 6));
					}
					else
					{
						num = 1;
					}
				}
				else if (Defs.isJetpackEnabled)
				{
					float x2 = JoystickController.leftJoystick.value.x;
					float y2 = JoystickController.leftJoystick.value.y;
					num = ((Mathf.Abs(y2) >= Mathf.Abs(x2)) ? ((!(y2 >= 0f)) ? 8 : 7) : ((!(x2 >= 0f)) ? 9 : 10));
				}
			}
			else if (Defs.isJetpackEnabled && !character.isGrounded)
			{
				num = 11;
			}
			if (character.velocity.y < -2.5f && !character.isGrounded)
			{
				isPlayDownSound = true;
			}
			if (isPlayDownSound && character.isGrounded)
			{
				if (Defs.isSoundFX && !EffectsController.WeAreStealth)
				{
					NGUITools.PlaySound(jumpDownAudio);
				}
				isPlayDownSound = false;
			}
			if (num != typeAnim)
			{
				typeAnim = num;
				if (((isMulti && isMine) || !isMulti) && typeAnim != 2)
				{
					interpolateScript.myAnim = typeAnim;
					interpolateScript.weAreSteals = EffectsController.WeAreStealth;
					SetAnim(typeAnim, EffectsController.WeAreStealth);
				}
			}
		}
		if (_playWalkSound)
		{
			AudioClip audioClip = ((playerMoveC.isMechActive || playerMoveC.isBearActive) ? walkMech : walkAudio);
			if (!_audio.isPlaying || _audio.clip != audioClip)
			{
				_audio.loop = false;
				_audio.clip = audioClip;
				_audio.Play();
			}
		}
	}

	public IEnumerator _SetAndResetImpactedByTrampoline()
	{
		_impactedByTramp = true;
		yield return new WaitForSeconds(0.1f);
		_impactedByTramp = false;
	}

	private void OnControllerColliderHit(ControllerColliderHit col)
	{
		onRink = false;
		if ((!isMulti || isMine) && _irt != null && !_impactedByTramp)
		{
			UnityEngine.Object.Destroy(_irt);
			_irt = null;
		}
		if (col.gameObject.CompareTag("Conveyor") && (!isMulti || isMine))
		{
			if (!onConveyor)
			{
				conveyorDirection = Vector3.zero;
			}
			onConveyor = true;
			Conveyor component = col.transform.GetComponent<Conveyor>();
			if (component.accelerateSpeed)
			{
				conveyorDirection = Vector3.Lerp(conveyorDirection, col.transform.forward * component.maxspeed, component.acceleration);
			}
			else
			{
				conveyorDirection = col.transform.forward * component.maxspeed;
			}
			return;
		}
		onConveyor = false;
		if (col.gameObject.CompareTag("Rink") && (!isMulti || isMine))
		{
			onRink = true;
		}
		else if (!_impactedByTramp && (col.gameObject.CompareTag("Trampoline") || col.gameObject.CompareTag("ConveyorTrampoline")) && (!isMulti || isMine))
		{
			if (_irt == null)
			{
				_irt = base.gameObject.AddComponent<ImpactReceiverTrampoline>();
			}
			if (col.gameObject.CompareTag("Trampoline"))
			{
				TrampolineParameters component2 = col.gameObject.GetComponent<TrampolineParameters>();
				_irt.AddImpact(col.transform.up, (component2 != null) ? component2.force : 45f);
			}
			else
			{
				_irt.AddImpact(col.transform.forward, conveyorDirection.magnitude * 1.4f);
				conveyorDirection = Vector3.zero;
			}
			if (Defs.isSoundFX)
			{
				AudioSource component3 = col.gameObject.GetComponent<AudioSource>();
				if (component3 != null)
				{
					component3.Play();
				}
			}
			StartCoroutine(_SetAndResetImpactedByTrampoline());
		}
		else if ((!isMulti || isMine) && IsDeadCollider(col.gameObject) && !playerMoveC.isKilled)
		{
			if (GameConnect.isSpeedrun)
			{
				SpeedrunTrackController.instance.ApplyDeathColliderShield();
			}
			if (!GameConnect.isSpeedrun || !playerMoveC.isImmortality)
			{
				DeadColliderParam component4 = col.gameObject.GetComponent<DeadColliderParam>();
				isPlayDownSound = false;
				playerMoveC.KillSelf((component4 != null) ? component4.typeDead : WeaponSounds.TypeDead.angel);
			}
		}
	}

	private bool IsDeadCollider(GameObject go)
	{
		return go.name == "DeadCollider";
	}

	private void OnTriggerEnter(Collider col)
	{
		if (isMulti && !isMine)
		{
			return;
		}
		if (col.gameObject.name.Equals("DamageCollider"))
		{
			col.gameObject.GetComponent<DamageCollider>().RegisterPlayer();
		}
		if (col.gameObject.name.Equals("FallCollider"))
		{
			playerMoveC.SuicideFall();
		}
		if ((GameConnect.isSpeedrun || GameConnect.isDeathEscape) && IsDeadCollider(col.gameObject) && !playerMoveC.isKilled)
		{
			if (GameConnect.isSpeedrun)
			{
				SpeedrunTrackController.instance.ApplyDeathColliderShield();
			}
			if (!GameConnect.isSpeedrun || !playerMoveC.isImmortality)
			{
				DeadColliderParam component = col.gameObject.GetComponent<DeadColliderParam>();
				isPlayDownSound = false;
				playerMoveC.KillSelf((component != null) ? component.typeDead : WeaponSounds.TypeDead.angel);
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if ((!isMulti || isMine) && col.gameObject.GetComponent<DamageCollider>() != null)
		{
			col.gameObject.GetComponent<DamageCollider>().UnregisterPlayer();
		}
	}

	private void IncrementArmorPopularity(string currentArmor)
	{
		if (isInet && isMulti && isMine)
		{
			string key = "None";
			if (currentArmor != Defs.ArmorNewNoneEqupped)
			{
				key = ItemDb.GetItemNameNonLocalized(currentArmor, currentArmor, ShopNGUIController.CategoryNames.ArmorCategory, "Unknown");
			}
			Statistics.Instance.IncrementArmorPopularity(key);
			_armorPopularityCacheIsDirty = true;
		}
	}
}
