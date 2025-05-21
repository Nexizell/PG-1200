using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class BotHealth : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CresetHurtAudio_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003CresetHurtAudio_003Ed__1(int _003C_003E1__state)
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
				_hurtAudioIsPlaying = true;
				_003C_003E2__current = new WaitForSeconds(tm);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_hurtAudioIsPlaying = false;
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
	internal sealed class _003CFlash_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BotHealth _003C_003E4__this;

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
		public _003CFlash_003Ed__18(int _003C_003E1__state)
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
				SetTextureRecursivelyFrom(_003C_003E4__this._modelChild, _003C_003E4__this.hitTexture);
				_003C_003E2__current = new WaitForSeconds(0.125f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				SetTextureRecursivelyFrom(_003C_003E4__this._modelChild, _003C_003E4__this._skin);
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

	public static bool _hurtAudioIsPlaying;

	private static SkinsManagerPixlGun _skinsManager;

	public string myName = "Bot";

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private bool _flashing;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Texture _skin;

	private bool _weaponCreated;

	private IEnumerator resetHurtAudio(float tm)
	{
		_hurtAudioIsPlaying = true;
		yield return new WaitForSeconds(tm);
		_hurtAudioIsPlaying = false;
	}

	public bool RequestPlayHurt(float tm)
	{
		if (_hurtAudioIsPlaying)
		{
			return false;
		}
		StartCoroutine(resetHurtAudio(tm));
		return true;
	}

	private void Awake()
	{
		if (GameConnect.isCOOP)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				_modelChild = transform.gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		_soundClips = _modelChild.GetComponent<Sounds>();
		if (GameConnect.isSurvival && TrainingController.TrainingCompleted)
		{
			if (ZombieCreator.sharedCreator.currentWave == 0)
			{
				_soundClips.notAttackingSpeed *= 0.75f;
				_soundClips.attackingSpeed *= 0.75f;
				_soundClips.health *= 0.7f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 1)
			{
				_soundClips.notAttackingSpeed *= 0.85f;
				_soundClips.attackingSpeed *= 0.85f;
				_soundClips.health *= 0.8f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 2)
			{
				_soundClips.notAttackingSpeed *= 0.9f;
				_soundClips.attackingSpeed *= 0.9f;
				_soundClips.health *= 0.9f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 7)
			{
				_soundClips.notAttackingSpeed *= 1.25f;
				_soundClips.attackingSpeed *= 1.25f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 9)
			{
				_soundClips.health *= 1.25f;
			}
		}
		ai = GetComponent<BotAI>();
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		if (base.gameObject.name.IndexOf("Boss") == -1)
		{
			_skin = SetSkinForObj(_modelChild);
			return;
		}
		Renderer componentInChildren = _modelChild.GetComponentInChildren<Renderer>();
		_skin = componentInChildren.material.mainTexture;
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
		SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static string SkinNameForObj(string objName)
	{
		if (!GameConnect.isSurvival)
		{
			if (TrainingController.TrainingCompleted)
			{
				return objName + "_Level" + CurrentCampaignGame.currentLevel;
			}
			return objName + "_Level3";
		}
		return objName;
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (Transform item in obj.transform)
		{
			if (!item.name.Equals("Ears"))
			{
				if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
				{
					item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
				SetTextureRecursivelyFrom(item.gameObject, txt);
			}
		}
	}

	private IEnumerator Flash()
	{
		_flashing = true;
		SetTextureRecursivelyFrom(_modelChild, hitTexture);
		yield return new WaitForSeconds(0.125f);
		SetTextureRecursivelyFrom(_modelChild, _skin);
		_flashing = false;
	}

	private void _CreateBonusWeapon()
	{
		if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName) && base.gameObject.name.Contains("Boss") && !_weaponCreated)
		{
			string weaponName = LevelBox.weaponsFromBosses[Application.loadedLevelName];
			Vector3 pos = base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f);
			((GetComponent<BotMovement>()._gameController.weaponBonus != null) ? BonusCreator._CreateBonusFromPrefab(GetComponent<BotMovement>()._gameController.weaponBonus, pos) : BonusCreator._CreateBonus(weaponName, pos)).AddComponent<GotToNextLevel>();
			GetComponent<BotMovement>()._gameController.weaponBonus = null;
			_weaponCreated = true;
		}
	}

	public void adjustHealth(float _health, Transform target)
	{
		if (_health < 0f && !_flashing)
		{
			StartCoroutine(Flash());
		}
		_soundClips.health += _health;
		if (_soundClips.health < 0f)
		{
			_soundClips.health = 0f;
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			_CreateBonusWeapon();
			IsLife = false;
		}
		else if (_soundClips.health == 0f)
		{
			_CreateBonusWeapon();
			IsLife = false;
		}
		else
		{
			GlobalGameController.Score += 5;
		}
		if (RequestPlayHurt(_soundClips.hurt.length) && Defs.isSoundFX)
		{
			GetComponent<AudioSource>().PlayOneShot(_soundClips.hurt);
		}
		if ((target.CompareTag("Player") && !target.GetComponent<SkinName>().playerMoveC.isInvisible) || target.CompareTag("Turret"))
		{
			ai.SetTarget(target, true);
		}
	}

	public bool getIsLife()
	{
		return IsLife;
	}
}
