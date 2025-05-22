using System.Collections.Generic;
using Rilisoft.WP8;
using UnityEngine;

public sealed class AdvancedEffects : MonoBehaviour
{
	public enum AdvancedEffect
	{
		none = 0,
		burning = 1,
		charm = 2
	}

	public struct ActiveAdvancedEffect
	{
		public AdvancedEffect effect;

		public float lifeTime;

		public Player_move_c sender;

		public ActiveAdvancedEffect(AdvancedEffect effect, float time)
		{
			this.effect = effect;
			lifeTime = Time.time + time;
			sender = null;
		}

		public ActiveAdvancedEffect(AdvancedEffect effect, float time, int sender)
		{
			this.effect = effect;
			lifeTime = Time.time + time;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.players[i].skinNamePixelView.viewID == sender)
				{
					this.sender = Initializer.players[i];
					return;
				}
			}
			this.sender = null;
		}

		public ActiveAdvancedEffect UpdateTime(float time)
		{
			return new ActiveAdvancedEffect(effect, time);
		}

		public ActiveAdvancedEffect UpdateTime(float time, int sender)
		{
			return new ActiveAdvancedEffect(effect, time, sender);
		}
	}

	public const float charmDamageMultiplier = 0.5f;

	public bool syncInLocal;

	private bool isMine;

	private PhotonView _photonView;

	private List<ActiveAdvancedEffect> playerEffects = new List<ActiveAdvancedEffect>(3);

	private GameObject burningEffect;

	private GameObject charmEffect;

	private void Start()
	{
		_photonView = GetComponent<PhotonView>();
		isMine = !Defs.isMulti || _photonView == null || (Defs.isInet && _photonView.isMine);
	}

	public void SendAdvancedEffect(int effectIndex, float effectTime, int senderPixelID = -1)
	{
		if (Defs.isMulti && Defs.isInet)
		{
			if (senderPixelID == -1)
			{
				_photonView.RPC("AdvancedEffectRPC", PhotonTargets.Others, effectIndex, effectTime);
			}
			else
			{
				_photonView.RPC("AdvancedEffectWithSenderRPC", PhotonTargets.Others, effectIndex, effectTime, senderPixelID);
			}
		}
		AdvancedEffectRPC(effectIndex, effectTime);
	}

	public void SendAdvancedEffect(PhotonPlayer photonPlayer, int effectIndex, float effectTime = 0f, int senderPixelID = -1)
	{
		if (Defs.isMulti && Defs.isInet)
		{
			if (senderPixelID == -1)
			{
				_photonView.RPC("AdvancedEffectRPC", photonPlayer, effectIndex, effectTime);
			}
			else
			{
				_photonView.RPC("AdvancedEffectWithSenderRPC", photonPlayer, effectIndex, effectTime, senderPixelID);
			}
		}
	}

	
	[PunRPC]
	public void AdvancedEffectRPC(int effectIndex, float effectTime)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == (AdvancedEffect)effectIndex)
			{
				playerEffects[i] = playerEffects[i].UpdateTime(effectTime);
				return;
			}
		}
		playerEffects.Add(new ActiveAdvancedEffect((AdvancedEffect)effectIndex, effectTime));
		ActivateAdvancedEffect((AdvancedEffect)effectIndex);
	}

	
	[PunRPC]
	public void AdvancedEffectWithSenderRPC(int effectIndex, float effectTime, int senderPixelID)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == (AdvancedEffect)effectIndex && (effectIndex != 2 || playerEffects[i].sender.mySkinName.pixelView.viewID.Equals(senderPixelID)))
			{
				playerEffects[i] = playerEffects[i].UpdateTime(effectTime, senderPixelID);
				return;
			}
		}
		playerEffects.Add(new ActiveAdvancedEffect((AdvancedEffect)effectIndex, effectTime, senderPixelID));
		ActivateAdvancedEffect((AdvancedEffect)effectIndex);
	}

	private float GetCenterPosition()
	{
		if (base.transform.childCount > 0)
		{
			BoxCollider component = base.transform.GetChild(0).GetComponent<BoxCollider>();
			if (component != null)
			{
				return component.center.y;
			}
		}
		return 0f;
	}

	private void ActivateAdvancedEffect(AdvancedEffect effect)
	{
		switch (effect)
		{
		case AdvancedEffect.burning:
			burningEffect = ParticleStacks.instance.fireStack.GetParticle();
			if (burningEffect != null)
			{
				burningEffect.transform.SetParent(base.transform, false);
				burningEffect.transform.localPosition = Vector3.up * GetCenterPosition();
			}
			break;
		case AdvancedEffect.charm:
			charmEffect = ParticleStacks.instance.charmStack.GetParticle();
			if (charmEffect != null)
			{
				charmEffect.transform.SetParent(base.transform, false);
				charmEffect.transform.localPosition = Vector3.up * GetCenterPosition();
			}
			break;
		}
	}

	private void DeactivateAdvancedEffect(AdvancedEffect effect)
	{
		switch (effect)
		{
		case AdvancedEffect.burning:
			if (burningEffect != null && ParticleStacks.instance != null)
			{
				burningEffect.transform.parent = null;
				ParticleStacks.instance.fireStack.ReturnParticle(burningEffect);
				burningEffect = null;
			}
			break;
		case AdvancedEffect.charm:
			if (charmEffect != null && ParticleStacks.instance != null)
			{
				charmEffect.transform.parent = null;
				ParticleStacks.instance.charmStack.ReturnParticle(charmEffect);
				charmEffect = null;
			}
			break;
		}
	}

	public bool IsEffectActive(AdvancedEffect effect, Player_move_c sender)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == effect && sender.Equals(playerEffects[i].sender))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsEffectActive(AdvancedEffect effect)
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		for (int i = 0; i < playerEffects.Count; i++)
		{
			if (playerEffects[i].lifeTime < Time.time)
			{
				AdvancedEffect effect = playerEffects[i].effect;
				playerEffects.RemoveAt(i);
				i--;
				if (!IsEffectActive(effect))
				{
					DeactivateAdvancedEffect(effect);
				}
			}
		}
	}

	private void OnDestroy()
	{
		int num;
		for (num = 0; num < playerEffects.Count; num++)
		{
			DeactivateAdvancedEffect(playerEffects[num].effect);
			playerEffects.RemoveAt(num);
			num--;
		}
	}
}
