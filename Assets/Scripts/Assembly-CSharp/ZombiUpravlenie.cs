using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft.WP8;
using UnityEngine;
using UnityEngine.AI;

public sealed class ZombiUpravlenie : MonoBehaviour
{
	public delegate void DelayedCallback();

	[CompilerGenerated]
	internal sealed class _003CresetDeathAudio_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float tm;

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
		public _003CresetDeathAudio_003Ed__1(int _003C_003E1__state)
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
				_deathAudioPlaying = true;
				_003C_003E2__current = new WaitForSeconds(tm);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_deathAudioPlaying = false;
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
	internal sealed class _003CFlash_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ZombiUpravlenie _003C_003E4__this;

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
		public _003CFlash_003Ed__41(int _003C_003E1__state)
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
				_003C_003E4__this._flashing = true;
				BotHealth.SetTextureRecursivelyFrom(_003C_003E4__this._modelChild, _003C_003E4__this.hitTexture);
				_003C_003E2__current = new WaitForSeconds(0.125f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				BotHealth.SetTextureRecursivelyFrom(_003C_003E4__this._modelChild, _003C_003E4__this._skin);
				_003C_003E4__this._flashing = false;
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
	internal sealed class _003C___DelayedCallback_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float time;

		public DelayedCallback callback;

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
		public _003C___DelayedCallback_003Ed__56(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(time);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				callback();
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

	public static bool _deathAudioPlaying;

	public GameObject playerKill;

	private Player_move_c healthDown;

	private GameObject player;

	private float CurLifeTime;

	private string idleAnim = "Idle";

	private string normWalkAnim = "Norm_Walk";

	private string zombieWalkAnim = "Zombie_Walk";

	private string offAnim = "Zombie_Off";

	private string deathAnim = "Zombie_Dead";

	private string onAnim = "Zombie_On";

	private string attackAnim = "Zombie_Attack";

	private string shootAnim;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private bool falling;

	private NavMeshAgent _nma;

	private BoxCollider _modelChildCollider;

	private ZombiManager _gameController;

	public bool deaded;

	public Transform target;

	public float health;

	private PhotonView photonView;

	public Texture hitTexture;

	private Texture _skin;

	private static SkinsManagerPixlGun _skinsManager;

	private bool _flashing;

	public int typeZombInMas;

	private float timeToUpdateTarget = 5f;

	private float timeToUpdateNavMesh;

	public int tekAnim = -1;

	private float timeToResetPath;

	private IEnumerator resetDeathAudio(float tm)
	{
		_deathAudioPlaying = true;
		yield return new WaitForSeconds(tm);
		_deathAudioPlaying = false;
	}

	public bool RequestPlayDeath(float tm)
	{
		if (_deathAudioPlaying)
		{
			return false;
		}
		StartCoroutine(resetDeathAudio(tm));
		return true;
	}

	private void Awake()
	{
		try
		{
			_modelChild = base.transform.GetChild(0).gameObject;
			health = _modelChild.GetComponent<Sounds>().health;
			if (Defs.isMulti && !GameConnect.isCOOP)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!GameConnect.isCOOP)
			{
				base.enabled = false;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

	[PunRPC]
	
	private void setHealthRPC(float _health)
	{
		health = _health;
	}

	
	[PunRPC]
	private void flashRPC()
	{
		StartCoroutine(Flash());
	}

	
	[PunRPC]
	public void SlowdownRPC(float coef)
	{
	}

	public void setHealth(float _health, bool isFlash)
	{
		photonView.RPC("setHealthRPC", PhotonTargets.All, _health);
		if (isFlash && !_flashing)
		{
			StartCoroutine(Flash());
			photonView.RPC("flashRPC", PhotonTargets.Others);
		}
	}

	public static Texture SetSkinForObj(GameObject go)
	{
		if (!_skinsManager)
		{
			_skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		Texture texture = null;
		string text = SkinNameForObj(go.name);
		if (!(texture = _skinsManager.skins[text] as Texture))
		{
			UnityEngine.Debug.Log("No skin: " + text);
		}
		BotHealth.SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static string SkinNameForObj(string objName)
	{
		return objName;
	}

	private IEnumerator Flash()
	{
		_flashing = true;
		BotHealth.SetTextureRecursivelyFrom(_modelChild, hitTexture);
		yield return new WaitForSeconds(0.125f);
		BotHealth.SetTextureRecursivelyFrom(_modelChild, _skin);
		_flashing = false;
	}

	private void Start()
	{
		try
		{
			_skin = SetSkinForObj(_modelChild);
			_nma = GetComponent<NavMeshAgent>();
			_modelChildCollider = _modelChild.GetComponent<BoxCollider>();
			shootAnim = offAnim;
			player = GameObject.FindGameObjectWithTag("Player");
			_gameController = GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<ZombiManager>();
			_soundClips = _modelChild.GetComponent<Sounds>();
			CurLifeTime = _soundClips.timeToHit;
			target = null;
			_modelChild.GetComponent<Animation>().Stop();
			Walk();
			_soundClips.attackingSpeed += UnityEngine.Random.Range(0f - _soundClips.attackingSpeedRandomRange[0], _soundClips.attackingSpeedRandomRange[1]);
			photonView = PhotonView.Get(this);
			_skin = SetSkinForObj(_modelChild);
			if (photonView.isMine)
			{
				photonView.RPC("setHealthRPC", PhotonTargets.All, _soundClips.health);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

	public void setId(int _id)
	{
		photonView.RPC("setIdRPC", PhotonTargets.All, _id);
	}

	
	[PunRPC]
	public void setIdRPC(int _id)
	{
		GetComponent<PhotonView>().viewID = _id;
	}

	private void Update()
	{
		try
		{
			if (!ZombiManager.sharedManager.startGame)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				if (!photonView.isMine)
				{
					return;
				}
				if (!deaded)
				{
					if (target != null && target.CompareTag("Player") && target.GetComponent<SkinName>().playerMoveC.isInvisible)
					{
						target = null;
					}
					if (target != null && timeToUpdateTarget > 0f)
					{
						timeToUpdateTarget -= Time.deltaTime;
						float num = Vector3.SqrMagnitude(target.position - base.transform.position);
						Vector3 vector = new Vector3(target.position.x, base.transform.position.y, target.position.z);
						if (num >= _soundClips.attackDistance * _soundClips.attackDistance)
						{
							timeToUpdateNavMesh -= Time.deltaTime;
							if (timeToUpdateNavMesh < 0f)
							{
								_nma.SetDestination(vector);
								_nma.speed = _soundClips.attackingSpeed * Mathf.Pow(1.05f, GlobalGameController.AllLevelsCompleted);
								timeToUpdateNavMesh = 0.5f;
							}
							CurLifeTime = _soundClips.timeToHit;
							PlayZombieRun();
						}
						else
						{
							if (_nma.path != null)
							{
								_nma.ResetPath();
							}
							CurLifeTime -= Time.deltaTime;
							base.transform.LookAt(vector);
							if (CurLifeTime <= 0f)
							{
								CurLifeTime = _soundClips.timeToHit;
								if (Defs.isSoundFX)
								{
									GetComponent<AudioSource>().PlayOneShot(_soundClips.bite);
								}
								target.CompareTag("Player");
								if (target.CompareTag("Turret"))
								{
									target.GetComponent<TurretController>().MinusLive(_soundClips.damagePerHit);
								}
							}
							PlayZombieAttack();
						}
					}
					else
					{
						timeToResetPath -= Time.deltaTime;
						if (timeToResetPath <= 0f)
						{
							timeToResetPath = 5f;
							_nma.ResetPath();
							Vector3 vector2 = new Vector3(-20 + UnityEngine.Random.Range(0, 40), base.transform.position.y, -20 + UnityEngine.Random.Range(0, 40));
							base.transform.LookAt(vector2);
							_nma.SetDestination(vector2);
							_nma.speed = _soundClips.notAttackingSpeed;
						}
						GameObject[] array = GameObject.FindGameObjectsWithTag("Turret");
						if (Initializer.players.Count > 0)
						{
							timeToUpdateTarget = 5f;
							float num2 = Vector3.SqrMagnitude(base.transform.position - Initializer.players[0].myPlayerTransform.position);
							target = Initializer.players[0].myPlayerTransform;
							foreach (Player_move_c player in Initializer.players)
							{
								if (!player.isInvisible)
								{
									float num3 = Vector3.SqrMagnitude(base.transform.position - player.myPlayerTransform.position);
									if (num3 < num2)
									{
										num2 = num3;
										target = player.myPlayerTransform;
									}
								}
							}
							GameObject[] array2 = array;
							foreach (GameObject gameObject in array2)
							{
								if (gameObject.GetComponent<TurretController>().isRun)
								{
									float num4 = Vector3.SqrMagnitude(base.transform.position - gameObject.transform.position);
									if (num4 < num2)
									{
										num2 = num4;
										target = gameObject.transform;
									}
								}
							}
						}
					}
					if (health <= 0f)
					{
						photonView.RPC("Death", PhotonTargets.All);
					}
				}
				else if (falling)
				{
					float num5 = 7f;
					base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - num5 * Time.deltaTime, base.transform.position.z);
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

	[PunRPC]
	
	private void Death()
	{
		if (!GameConnect.isCOOP)
		{
			return;
		}
		if (_nma != null)
		{
			_nma.enabled = false;
		}
		float num = 0.1f;
		if (Defs.isSoundFX && _soundClips != null)
		{
			if (RequestPlayDeath(_soundClips.death.length))
			{
				GetComponent<AudioSource>().PlayOneShot(_soundClips.death);
			}
			num = _soundClips.death.length;
		}
		_modelChild.GetComponent<Animation>().Stop();
		if ((bool)_modelChild.GetComponent<Animation>()[deathAnim])
		{
			_modelChild.GetComponent<Animation>().Play(deathAnim);
			num = Mathf.Max(num, _modelChild.GetComponent<Animation>()[deathAnim].length);
			CodeAfterDelay(_modelChild.GetComponent<Animation>()[deathAnim].length * 1.25f, StartFall);
		}
		else
		{
			StartFall();
		}
		CodeAfterDelay(num, DestroySelf);
		_modelChild.GetComponent<BoxCollider>().enabled = false;
		deaded = true;
		GetComponent<SpawnMonster>().ShouldMove = false;
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void StartFall()
	{
		falling = true;
	}

	private void Walk()
	{
		_modelChild.GetComponent<Animation>().Stop();
		if ((bool)_modelChild.GetComponent<Animation>()[normWalkAnim])
		{
			_modelChild.GetComponent<Animation>().CrossFade(normWalkAnim);
		}
		else
		{
			_modelChild.GetComponent<Animation>().CrossFade(zombieWalkAnim);
		}
	}

	public void PlayZombieRun()
	{
		if (tekAnim != 1 && GameConnect.isCOOP)
		{
			if ((bool)_modelChild.GetComponent<Animation>()[zombieWalkAnim])
			{
				_modelChild.GetComponent<Animation>().CrossFade(zombieWalkAnim);
			}
			photonView.RPC("PlayZombieRunRPC", PhotonTargets.Others);
		}
		tekAnim = 1;
	}

	public void PlayZombieAttack()
	{
		if (tekAnim != 2 && GameConnect.isCOOP)
		{
			if ((bool)_modelChild.GetComponent<Animation>()[attackAnim])
			{
				_modelChild.GetComponent<Animation>().CrossFade(attackAnim);
			}
			else if ((bool)_modelChild.GetComponent<Animation>()[shootAnim])
			{
				_modelChild.GetComponent<Animation>().CrossFade(shootAnim);
			}
			photonView.RPC("PlayZombieAttackRPC", PhotonTargets.Others);
		}
		tekAnim = 2;
	}

	
	[PunRPC]
	public void PlayZombieRunRPC()
	{
		if ((bool)_modelChild.GetComponent<Animation>()[zombieWalkAnim])
		{
			_modelChild.GetComponent<Animation>().CrossFade(zombieWalkAnim);
		}
		tekAnim = 1;
	}

	
	[PunRPC]
	public void PlayZombieAttackRPC()
	{
		if ((bool)_modelChild.GetComponent<Animation>()[attackAnim])
		{
			_modelChild.GetComponent<Animation>().CrossFade(attackAnim);
		}
		else if ((bool)_modelChild.GetComponent<Animation>()[shootAnim])
		{
			_modelChild.GetComponent<Animation>().CrossFade(shootAnim);
		}
		tekAnim = 2;
	}

	public void CodeAfterDelay(float delay, DelayedCallback callback)
	{
		StartCoroutine(___DelayedCallback(delay, callback));
	}

	private IEnumerator ___DelayedCallback(float time, DelayedCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}
}
