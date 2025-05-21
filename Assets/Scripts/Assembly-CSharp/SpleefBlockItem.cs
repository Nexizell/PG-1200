using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpleefBlockItem : MonoBehaviour, IDamageable
{
	[CompilerGenerated]
	internal sealed class _003CDisolve_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003Calfa_003E5__1;

		public SpleefBlockItem _003C_003E4__this;

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
		public _003CDisolve_003Ed__24(int _003C_003E1__state)
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
				_003Calfa_003E5__1 = 1.5f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Calfa_003E5__1 > 0f)
			{
				_003Calfa_003E5__1 -= Time.deltaTime;
				_003C_003E4__this.mesh.material.SetFloat("_Burn", _003Calfa_003E5__1);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.gameObject.SetActive(false);
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
	internal sealed class _003CHitEffect_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SpleefBlockItem _003C_003E4__this;

		private float _003C_timer_003E5__1;

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
		public _003CHitEffect_003Ed__28(int _003C_003E1__state)
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
				if (_003C_003E4__this.hitMaterial == null)
				{
					_003C_003E4__this.hitMaterial = ((_003C_003E4__this.mesh != null) ? _003C_003E4__this.mesh.material : null);
					_003C_003E4__this.hitMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.7f));
				}
				if (_003C_003E4__this.isRunHitEffect)
				{
					_003C_003E4__this.replayHitEffect = true;
					return false;
				}
				_003C_003E4__this.isRunHitEffect = true;
				goto IL_00bf;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0180;
			case 2:
				_003C_003E1__state = -1;
				goto IL_01f1;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01f1:
				if (!_003C_003E4__this.replayHitEffect)
				{
					_003C_003E4__this.isRunHitEffect = false;
					break;
				}
				goto IL_00bf;
				IL_00bf:
				if (Defs.isSoundFX && _003C_003E4__this.curAudioSorce != null && SpleefBlockItemsController.instance.hitSound != null)
				{
					_003C_003E4__this.curAudioSorce.enabled = true;
					_003C_003E4__this.curAudioSorce.PlayOneShot(SpleefBlockItemsController.instance.hitSound);
				}
				_003C_003E4__this.replayHitEffect = false;
				if (!_003C_003E4__this.isKilled)
				{
					_003C_003E4__this.mesh.sharedMaterial = _003C_003E4__this.hitMaterial;
				}
				_003C_timer_003E5__1 = 0.07f;
				goto IL_0180;
				IL_0180:
				if (_003C_timer_003E5__1 > 0f && !_003C_003E4__this.replayHitEffect)
				{
					_003C_timer_003E5__1 -= Time.deltaTime;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (!_003C_003E4__this.isKilled)
				{
					_003C_003E4__this.mesh.sharedMaterial = _003C_003E4__this.GetMaterialByStage(_003C_003E4__this.GetCurrentStage());
				}
				if (_003C_003E4__this.replayHitEffect)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_01f1;
			}
			if (_003C_003E4__this.curAudioSorce.isPlaying)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			}
			_003C_003E4__this.curAudioSorce.enabled = false;
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

	public float live = 5f;

	public short index;

	public Material disolveMaterial;

	[NonSerialized]
	public bool isDamageble;

	public bool isKilled;

	private bool oldIsMaster;

	private Material hitMaterial;

	private AudioSource curAudioSorce;

	private Renderer mesh;

	private float startHealth;

	private bool isRunHitEffect;

	private bool replayHitEffect;

	public bool isLivingTarget
	{
		get
		{
			return false;
		}
	}

	private void Start()
	{
		startHealth = live;
		Initializer.damageableObjects.Add(base.gameObject);
		mesh = GetComponent<Renderer>();
		mesh.sharedMaterial = GetMaterialByStage(GetCurrentStage());
		curAudioSorce = GetComponent<AudioSource>();
		curAudioSorce.enabled = false;
	}

	private Material GetMaterialByStage(int stage)
	{
		return SpleefBlockItemsController.instance.stageMaterials[stage];
	}

	private int GetCurrentStage()
	{
		int result = 0;
		float num = live / startHealth;
		if (num < 0.3f)
		{
			result = 2;
		}
		else if (num < 0.99f)
		{
			result = 1;
		}
		return result;
	}

	private void OnDestroy()
	{
		Initializer.damageableObjects.Remove(base.gameObject);
	}

	private void Update()
	{
		if (PhotonNetwork.connected)
		{
			if (!oldIsMaster && PhotonNetwork.isMasterClient && isKilled)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			oldIsMaster = PhotonNetwork.isMasterClient;
		}
	}

	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, "");
	}

	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerViewId = 0)
	{
		GetDamage(damage);
	}

	public bool IsEnemyTo(Player_move_c player)
	{
		return true;
	}

	public bool IsDead()
	{
		return isKilled;
	}

	public void GetDamage(float damage)
	{
		if (live <= 0f || isKilled)
		{
			return;
		}
		isDamageble = true;
		live -= damage;
		if (live <= 0f)
		{
			damage = 10000000f;
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.AddToScore(1);
			}
		}
		Hit();
		SpleefBlockItemsController.instance.GetDamage(index, damage);
	}

	public void GetDamageRPC(float _minus)
	{
		isDamageble = true;
		if (!isKilled)
		{
			live -= _minus;
			Hit();
		}
	}

	public void KilledChest()
	{
		if (!isKilled)
		{
			isKilled = true;
			if (mesh == null)
			{
				mesh = GetComponent<Renderer>();
			}
			if (mesh != null)
			{
				mesh.material = disolveMaterial;
			}
			if (Defs.isSoundFX && curAudioSorce != null && SpleefBlockItemsController.instance.deadSound != null)
			{
				curAudioSorce.enabled = true;
				curAudioSorce.PlayOneShot(SpleefBlockItemsController.instance.deadSound);
			}
			StartCoroutine(Disolve());
			GetComponent<BoxCollider>().enabled = false;
		}
	}

	private IEnumerator Disolve()
	{
		float alfa = 1.5f;
		while (alfa > 0f)
		{
			alfa -= Time.deltaTime;
			mesh.material.SetFloat("_Burn", alfa);
			yield return null;
		}
		gameObject.SetActive(false);
	}

	private void Hit()
	{
		if (live <= 0f)
		{
			KilledChest();
		}
		else if (!isKilled)
		{
			StartCoroutine(HitEffect());
		}
	}

	private IEnumerator HitEffect()
	{
		if (hitMaterial == null)
		{
			hitMaterial = ((mesh != null) ? mesh.material : null);
			hitMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.7f));
		}
		if (isRunHitEffect)
		{
			replayHitEffect = true;
			yield break;
		}
		isRunHitEffect = true;
		do
		{
			if (Defs.isSoundFX && curAudioSorce != null && SpleefBlockItemsController.instance.hitSound != null)
			{
				curAudioSorce.enabled = true;
				curAudioSorce.PlayOneShot(SpleefBlockItemsController.instance.hitSound);
			}
			replayHitEffect = false;
			if (!isKilled)
			{
				mesh.sharedMaterial = hitMaterial;
			}
			float _timer = 0.07f;
			while (_timer > 0f && !replayHitEffect)
			{
				_timer -= Time.deltaTime;
				yield return null;
			}
			if (!isKilled)
			{
				mesh.sharedMaterial = GetMaterialByStage(GetCurrentStage());
			}
			if (replayHitEffect)
			{
				yield return null;
			}
		}
		while (replayHitEffect);
		isRunHitEffect = false;
		while (curAudioSorce.isPlaying)
		{
			yield return null;
		}
		curAudioSorce.enabled = false;
	}
}
