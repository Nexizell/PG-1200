using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamageable
{
	public enum TurretType
	{
		AttackTurret = 0,
		ShieldWall = 1
	}

	[CompilerGenerated]
	internal sealed class _003CScanTarget_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003CScanTarget_003Ed__56(int _003C_003E1__state)
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
				_003C_003E2__current = null;
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
	internal sealed class _003CFlashRed_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TurretController _003C_003E4__this;

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
		public _003CFlashRed_003Ed__74(int _003C_003E1__state)
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
				_003C_003E4__this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 0f, 0f, 1f));
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
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

	[Header("Damage settings")]
	public float damageMulty = 1f;

	public float healthSurvival = 18f;

	public float maxRadiusScanTarget = 30f;

	[Header("Main sound settings")]
	public AudioClip turretActivSound;

	public AudioClip turretDeadSound;

	public AudioClip turretDeactivationSound;

	public AudioClip hitSound;

	[Header("Other")]
	public TurretType turretType;

	public Renderer turretRenderer;

	public GameObject explosionAnimObj;

	public GameObject isEnemySprite;

	public Transform healthBar;

	public Transform spherePoint;

	public Transform rayGroundPoint;

	public TextMesh nickLabel;

	public Material inactiveMaterial;

	public Animation animation;

	public bool solidWithEnemiesOnly;

	public bool setMaterialsForEnemy;

	public Material[] enemyMaterials;

	public GameObject workingSound;

	public GameObject workingParticle;

	[HideInInspector]
	public bool isRun;

	protected bool isReady;

	[HideInInspector]
	public bool isKilled;

	[HideInInspector]
	public int numUpdate;

	[HideInInspector]
	public float health = 10000000f;

	[HideInInspector]
	public float maxHealth = 10000000f;

	[HideInInspector]
	public GameObject myPlayer;

	[HideInInspector]
	public Player_move_c myPlayerMoveC;

	[HideInInspector]
	public PhotonView photonView;

	[HideInInspector]
	public bool isEnemyTurret;

	protected float maxRadiusScanTargetSQR;

	protected bool isMine;

	protected bool inScaning;

	protected float maxTimerShot = 0.1f;

	protected Transform target;

	private float timerScanTargetIdle = -1f;

	private float maxTimerScanTargetIdle = 0.5f;

	private Rigidbody rigidBody;

	private Vector3 turretMinPos = new Vector3(0f, float.MaxValue, 0f);

	private int _nickColorInd;

	private bool _isSetNickLabelText;

	private bool wasStickedToPlayer;

	private BoxCollider myCollider;

	private Material[] originalMaterials;

	private bool isSetAsEnemy;

	public Action GadgetOnKill;

	public bool validatePlaceWithSphere = true;

	public float destroyTurretTime = 2f;

	protected string gadgetName;

	public bool isLivingTarget
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		originalMaterials = new Material[turretRenderer.materials.Length];
		for (int i = 0; i < turretRenderer.sharedMaterials.Length; i++)
		{
			originalMaterials[i] = turretRenderer.sharedMaterials[i];
		}
		photonView = PhotonView.Get(this);
		myCollider = GetComponent<BoxCollider>();
		if ((bool)photonView && photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		maxRadiusScanTargetSQR = maxRadiusScanTarget * maxRadiusScanTarget;
		rigidBody = base.transform.GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision col)
	{
		if (isMine && col.gameObject.name == "DeadCollider")
		{
			MinusLive(1000f);
			rigidBody.isKinematic = true;
		}
	}

	private void Start()
	{
		turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		UpdateMaterial();
		if (Defs.isMulti && Defs.isInet)
		{
			isMine = photonView.isMine;
		}
		if (!Defs.isMulti || isMine)
		{
			numUpdate = 0;
			if (!GameConnect.isDaterRegim)
			{
				string text = base.gameObject.nameNoClone();
				string text2 = GadgetsInfo.LastBoughtFor(text);
				if (text2 != null)
				{
					numUpdate = GadgetsInfo.Upgrades[text].IndexOf(text2);
				}
				Player_move_c.SetLayerRecursively(base.gameObject, 9);
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					photonView.RPC("SynchNumUpdateRPC", PhotonTargets.AllBuffered, numUpdate);
				}
			}
			else
			{
				SynchNumUpdateRPC(numUpdate);
			}
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (photonView.ownerId == Initializer.players[i].mySkinName.photonView.ownerId)
					{
						myPlayer = Initializer.players[i].mySkinName.gameObject;
						myPlayerMoveC = myPlayer.GetComponent<SkinName>().playerMoveC;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < Initializer.players.Count; j++)
				{
				}
			}
		}
		else
		{
			myPlayer = WeaponManager.sharedManager.myPlayer;
			myPlayerMoveC = myPlayer.GetComponent<SkinName>().playerMoveC;
		}
		SetPlayerParent();
		if (this is VoodooSnowman)
		{
			if (!Defs.isMulti || isMine)
			{
				Initializer.damageableObjects.Add(base.gameObject);
			}
		}
		else
		{
			Initializer.turretsObj.Add(base.gameObject);
		}
	}

	protected void SetPlayerParent()
	{
		if (!isRun && myPlayerMoveC != null && !wasStickedToPlayer)
		{
			wasStickedToPlayer = true;
			base.transform.parent = myPlayerMoveC.turretPoint.transform;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}

	protected virtual IEnumerator ScanTarget()
	{
		yield return null;
	}

	private void UpdateNickLabelColor()
	{
		if (nickLabel == null)
		{
			return;
		}
		if (GameConnect.isTeamRegim)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC == null || myPlayerMoveC == null)
			{
				if (_nickColorInd != 0)
				{
					nickLabel.color = Color.white;
					_nickColorInd = 0;
				}
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == myPlayerMoveC.myCommand)
			{
				if (_nickColorInd != 1)
				{
					nickLabel.color = Color.blue;
					_nickColorInd = 1;
				}
			}
			else if (_nickColorInd != 2)
			{
				nickLabel.color = Color.red;
				_nickColorInd = 2;
			}
		}
		else if (GameConnect.isDaterRegim)
		{
			if (_nickColorInd != 0)
			{
				nickLabel.color = Color.white;
				_nickColorInd = 0;
			}
		}
		else if (GameConnect.isCOOP || (myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC == myPlayerMoveC))
		{
			if (_nickColorInd != 1)
			{
				nickLabel.color = Color.blue;
				_nickColorInd = 1;
			}
		}
		else if (_nickColorInd != 2)
		{
			nickLabel.color = Color.red;
			_nickColorInd = 2;
		}
	}

	protected virtual void OnKill()
	{
	}

	protected virtual void SearchTarget()
	{
		timerScanTargetIdle -= Time.deltaTime;
		if (timerScanTargetIdle < 0f)
		{
			timerScanTargetIdle = maxTimerScanTargetIdle;
			if (!inScaning)
			{
				StartCoroutine(ScanTarget());
			}
		}
	}

	protected virtual void TargetUpdate()
	{
	}

	private void Update()
	{
		if (!isRun && !wasStickedToPlayer)
		{
			SetPlayerParent();
		}
		if (setMaterialsForEnemy && isEnemyTurret != isSetAsEnemy)
		{
			UpdateMaterial();
			isSetAsEnemy = isEnemyTurret;
		}
		UpdateTurret();
	}

	protected virtual void UpdateTurret()
	{
		if (Defs.isMulti && myPlayerMoveC != null && !_isSetNickLabelText)
		{
			_isSetNickLabelText = true;
			if (nickLabel != null)
			{
				nickLabel.text = FilterBadWorld.FilterString(myPlayerMoveC.mySkinName.NickName);
			}
		}
		UpdateNickLabelColor();
		if (isRun && healthBar != null)
		{
			healthBar.localScale = new Vector3((health > 0f) ? (health / maxHealth) : 0f, 1f, 1f);
		}
		SetStateIsEnemyTurret();
		if (isEnemySprite != null && isEnemySprite.activeSelf != isEnemyTurret)
		{
			isEnemySprite.SetActive(isEnemyTurret);
		}
		isReady = isRun && !isKilled && (animation == null || !animation.IsPlaying("turret_start"));
		if (Defs.isMulti && !isMine)
		{
			return;
		}
		if (Defs.isMulti && WeaponManager.sharedManager.myPlayer == null)
		{
			DestroyTurrel();
		}
		else if (!isRun)
		{
			RaycastHit hitInfo;
			bool num = Physics.Raycast(new Ray(rayGroundPoint.position, Vector3.down), out hitInfo, 1f, Tools.AllWithoutDamageCollidersMask) && hitInfo.distance > 0.05f && hitInfo.distance < 0.7f;
			bool flag = false;
			RaycastHit hitInfo2;
			flag = ((!validatePlaceWithSphere) ? (Physics.Linecast(myPlayerMoveC.myTransform.position, spherePoint.position, out hitInfo2, Tools.AllWithoutMyPlayerMask) && hitInfo2.collider.transform.root != base.transform) : Physics.CheckSphere(spherePoint.position, spherePoint.GetComponent<SphereCollider>().radius, Tools.AllWithoutMyPlayerMask));
			if (num && !flag)
			{
				turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 1f, 1f, 0.08f));
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = true;
				}
			}
			else
			{
				turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 0f, 0f, 0.08f));
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = false;
				}
			}
		}
		else if (isKilled)
		{
			OnKill();
		}
		else
		{
			if (!isReady)
			{
				return;
			}
			if (target != null && (target.position.y < -500f || (!GameConnect.isDaterRegim && target.CompareTag("Player") && target.GetComponent<SkinName>().playerMoveC.isInvisible)))
			{
				target = null;
			}
			if (target == null)
			{
				SearchTarget();
				return;
			}
			TargetUpdate();
			timerScanTargetIdle -= Time.deltaTime;
			if (timerScanTargetIdle < 0f)
			{
				timerScanTargetIdle = maxTimerScanTargetIdle;
				if (!inScaning)
				{
					StartCoroutine(ScanTarget());
				}
			}
			if (!rigidBody.isKinematic)
			{
				if (base.transform.position.y < turretMinPos.y)
				{
					turretMinPos = base.transform.position;
				}
				else
				{
					base.transform.position = turretMinPos;
				}
			}
		}
	}

	
	[PunRPC]
	public void SynchNumUpdateRPC(int _numUpdate)
	{
		if (!GameConnect.isDaterRegim)
		{
			numUpdate = _numUpdate;
			string key = base.gameObject.name.Replace("(Clone)", string.Empty) + ((_numUpdate > 0) ? ("_up" + _numUpdate) : string.Empty);
			if (GadgetsInfo.info.ContainsKey(key))
			{
				SetParametersFromGadgets(GadgetsInfo.info[key]);
			}
		}
	}

	protected virtual void SetParametersFromGadgets(GadgetInfo info)
	{
		if (Defs.isMulti && !GameConnect.isCOOP)
		{
			maxHealth = info.Durability;
			health = maxHealth;
		}
		else
		{
			maxHealth = healthSurvival;
			health = maxHealth;
		}
		damageMulty = info.Damage;
		if (!Defs.isMulti || isMine)
		{
			gadgetName = GadgetsInfo.BaseName(info.Id);
		}
	}

	private void SetStateIsEnemyTurret()
	{
		bool isEnemyTurret2 = isEnemyTurret;
		isEnemyTurret = false;
		if (GameConnect.isTeamRegim)
		{
			if (myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC != null && myPlayerMoveC.myCommand != WeaponManager.sharedManager.myPlayerMoveC.myCommand)
			{
				isEnemyTurret = true;
			}
		}
		else if (Defs.isMulti && !isMine)
		{
			isEnemyTurret = true;
		}
	}

	
	[PunRPC]
	protected void ShotRPC()
	{
		Shot();
	}

	protected virtual void Shot()
	{
	}

	protected bool HitIDestructible(GameObject _obj)
	{
		IDamageable component = _obj.GetComponent<IDamageable>();
		if (component == null && _obj.transform.parent != null)
		{
			component = _obj.transform.parent.GetComponent<IDamageable>();
		}
		if (component != null && WeaponManager.sharedManager.myPlayer != null && component.IsEnemyTo(WeaponManager.sharedManager.myPlayerMoveC))
		{
			component.ApplyDamage(damageMulty, this, Player_move_c.TypeKills.turret, WeaponSounds.TypeDead.angel, gadgetName, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
			return true;
		}
		return false;
	}

	public virtual void StartTurret()
	{
		if (Defs.isMulti && isMine)
		{
			if (Defs.isInet)
			{
				photonView.RPC("StartTurretRPC", PhotonTargets.AllBuffered);
			}
		}
		else if (!Defs.isMulti)
		{
			StartTurretRPC();
		}
		myCollider.enabled = true;
		rigidBody.isKinematic = false;
		rigidBody.useGravity = true;
		Invoke("SetNoUseGravityInvoke", 5f);
	}

	private void SetNoUseGravityInvoke()
	{
		rigidBody.useGravity = false;
		rigidBody.isKinematic = true;
		myCollider.isTrigger = !solidWithEnemiesOnly;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (isMine)
		{
			PlayerConnectedPhoton(player);
		}
	}

	protected virtual void PlayerConnectedPhoton(PhotonPlayer player)
	{
		photonView.RPC("SynchHealth", player, health);
	}

	
	[PunRPC]
	public void SynchHealth(float _health)
	{
		if (health > _health)
		{
			health = _health;
		}
	}

	protected IEnumerator FlashRed()
	{
		turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		yield return null;
		turretRenderer.material.SetColor("_ColorRili", new Color(1f, 0f, 0f, 1f));
		yield return new WaitForSeconds(0.1f);
		turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
	}

	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		ApplyDamage(damage, damageFrom, Player_move_c.TypeKills.none, WeaponSounds.TypeDead.angel, "");
	}

	public virtual void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
	{
		MinusLive(damage, killerId);
	}

	public virtual bool IsEnemyTo(Player_move_c player)
	{
		if (!Defs.isMulti || GameConnect.isCOOP || myPlayerMoveC.Equals(player) || (GameConnect.isTeamRegim && myPlayerMoveC.myCommand == player.myCommand))
		{
			return false;
		}
		return true;
	}

	public bool IsDead()
	{
		return isKilled;
	}

	public void MinusLive(float dm, int idKiller = 0)
	{
		MinusLive(dm, true, idKiller);
	}

	public void MinusLive(float dm, bool isExplosion, int idKiller = 0)
	{
		if (GameConnect.isDaterRegim || !isRun)
		{
			return;
		}
		isExplosion = true;
		if (Defs.isMulti)
		{
			health -= dm;
			if (health < 0f)
			{
				ImKilledRPCWithExplosion(isExplosion);
				dm = 10000f;
			}
			if (Defs.isInet)
			{
				photonView.RPC("MinusLiveRPC", PhotonTargets.All, dm, isExplosion, idKiller);
			}
		}
		else
		{
			MinusLiveReal(dm, isExplosion);
		}
	}

	
	[PunRPC]
	public void MinusLiveRPC(float dm, int idKiller)
	{
		MinusLiveReal(dm, true, idKiller);
	}

	[PunRPC]
	
	public void MinusLiveRPC(float dm, bool isExplosion, int idKiller)
	{
		MinusLiveReal(dm, isExplosion, idKiller);
	}

	public void MinusLiveReal(float dm, bool isExplosion, int idKiller = 0)
	{
		StopCoroutine(FlashRed());
		StartCoroutine(FlashRed());
		if (Defs.isSoundFX && isExplosion)
		{
			GetComponent<AudioSource>().PlayOneShot(hitSound);
		}
		if (isKilled || (Defs.isMulti && !isMine))
		{
			return;
		}
		health -= dm;
		if (Defs.isMulti && Defs.isInet)
		{
			photonView.RPC("SynchHealth", PhotonTargets.Others, health);
		}
		if (!(health < 0f))
		{
			return;
		}
		health = 0f;
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				photonView.RPC("ImKilledRPCWithExplosion", PhotonTargets.AllBuffered, isExplosion);
				photonView.RPC("MeKillRPC", PhotonTargets.All, idKiller);
			}
		}
		else
		{
			ImKilledRPCWithExplosion(isExplosion);
		}
	}

	[PunRPC]
	
	public void MeKillRPC(int idKiller)
	{
		string nick = string.Empty;
		foreach (Player_move_c player in Initializer.players)
		{
			if (!(player.mySkinName.pixelView != null) || player.mySkinName.pixelView.viewID != idKiller)
			{
				continue;
			}
			nick = player.mySkinName.NickName;
			if (player.Equals(WeaponManager.sharedManager.myPlayerMoveC))
			{
				WeaponManager.sharedManager.myPlayerMoveC.ImKill(idKiller, 9);
				if (turretType == TurretType.ShieldWall)
				{
					WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.barrierBreaker);
				}
			}
			break;
		}
		MeKill(nick);
	}

	public void MeKill(string _nick)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(_nick, 9, myPlayer.GetComponent<SkinName>().NickName, Color.white);
		}
	}

	public void SendImKilledRPC()
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				photonView.RPC("ImKilledRPC", PhotonTargets.AllBuffered);
			}
		}
		else
		{
			ImKilledRPC();
		}
	}

	[PunRPC]
	
	public void ImKilledRPC()
	{
		ImKilledRPCWithExplosion(false);
	}

	[PunRPC]
	
	public void ImKilledRPCWithExplosion(bool isExplosion)
	{
		if (isKilled)
		{
			return;
		}
		isKilled = true;
		if (nickLabel != null)
		{
			nickLabel.gameObject.SetActive(false);
		}
		myCollider.enabled = false;
		rigidBody.isKinematic = true;
		if (Defs.isSoundFX)
		{
			if (isExplosion)
			{
				GetComponent<AudioSource>().PlayOneShot(turretDeadSound);
			}
			else
			{
				GetComponent<AudioSource>().PlayOneShot(turretDeactivationSound);
			}
		}
		if (isExplosion)
		{
			if (explosionAnimObj != null)
			{
				explosionAnimObj.SetActive(true);
			}
			if (animation != null)
			{
				animation.Play("turret_dead");
			}
			if (GadgetOnKill != null)
			{
				GadgetOnKill();
			}
		}
		else if (animation != null)
		{
			animation.Play("turret_stop");
		}
		if (workingSound != null)
		{
			workingSound.SetActive(false);
		}
		if (workingParticle != null)
		{
			workingParticle.SetActive(false);
		}
		DeactivateTurret();
		Invoke("DestroyTurrel", destroyTurretTime);
	}

	protected virtual void DeactivateTurret()
	{
	}

	protected virtual void OnDestroyTurret()
	{
	}

	private void DestroyTurrel()
	{
		if (Defs.isMulti)
		{
			if (isMine)
			{
				if (Defs.isInet)
				{
					PhotonNetwork.Destroy(base.gameObject);
				}
			}
			else
			{
				base.transform.position = new Vector3(-1000f, -1000f, -1000f);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	
	[PunRPC]
	public void StartTurretRPC()
	{
		if (nickLabel != null)
		{
			nickLabel.gameObject.SetActive(true);
		}
		myCollider.enabled = true;
		base.transform.parent = null;
		if (Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(turretActivSound);
		}
		Player_move_c.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Default"));
		if (Defs.isInet)
		{
			photonView.synchronization = ViewSynchronization.UnreliableOnChange;
		}
		isRun = true;
		if (Defs.isSoundFX && workingSound != null)
		{
			workingSound.SetActive(true);
		}
		if (workingParticle != null)
		{
			workingParticle.SetActive(true);
		}
		if (animation != null)
		{
			animation.Play("turret_start");
		}
		UpdateMaterial();
		if (solidWithEnemiesOnly && (!Defs.isMulti || isMine || GameConnect.isCOOP || (GameConnect.isTeamRegim && myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC != null && myPlayerMoveC.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand)))
		{
			base.gameObject.layer = LayerMask.NameToLayer("IgnoreRocketsAndBullets");
		}
	}

	private void UpdateMaterial()
	{
		if (isRun)
		{
			turretRenderer.sharedMaterials = ((setMaterialsForEnemy && isEnemyTurret) ? enemyMaterials : originalMaterials);
			return;
		}
		Material[] array = new Material[originalMaterials.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = inactiveMaterial;
		}
		turretRenderer.sharedMaterials = array;
	}

	private void OnDestroy()
	{
		OnDestroyTurret();
		if (!Defs.isMulti || isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Turret, null, false);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		if (this is VoodooSnowman)
		{
			if (!Defs.isMulti || isMine)
			{
				Initializer.damageableObjects.Remove(base.gameObject);
			}
		}
		else
		{
			Initializer.turretsObj.Remove(base.gameObject);
		}
	}

	public Vector3 GetHeadPoint()
	{
		Vector3 position = base.transform.position;
		position.y += myCollider.size.y - 0.5f;
		return position;
	}
}
