using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
	[NonSerialized]
	public Player_move_c myPlayer;

	public bool isLivingTarget
	{
		get
		{
			return true;
		}
	}

	private void Awake()
	{
		myPlayer = GetComponent<SkinName>().playerMoveC;
	}

	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, "");
	}

	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerViewId = 0)
	{
		if (GameConnect.isDaterRegim || GameConnect.isSpleef || GameConnect.isDeathEscape)
		{
			return;
		}
		Vector3 posKiller = Vector3.zero;
		if (damageFrom != null)
		{
			MonoBehaviour monoBehaviour = damageFrom as MonoBehaviour;
			posKiller = monoBehaviour.transform.position;
			bool flag = false;
			if (damageFrom is PlayerDamageable)
			{
				flag = (damageFrom as PlayerDamageable).myPlayer.IsPlayerEffectActive(Player_move_c.PlayerEffect.charm, myPlayer);
			}
			if (monoBehaviour.GetComponent<AdvancedEffects>() != null)
			{
				flag = monoBehaviour.GetComponent<AdvancedEffects>().IsEffectActive(AdvancedEffects.AdvancedEffect.charm, myPlayer);
			}
			if (flag)
			{
				damage *= 0.5f;
			}
			if (typeKill != Player_move_c.TypeKills.reflector && myPlayer.IsGadgetEffectActive(Player_move_c.GadgetEffect.reflector))
			{
				damage /= 2f;
				damageFrom.ApplyDamage(damage, this, Player_move_c.TypeKills.reflector, typeDead, weaponName, myPlayer.skinNamePixelView.viewID);
			}
			if (myPlayer.IsGadgetEffectActive(Player_move_c.GadgetEffect.protectionAmulet))
			{
				damage *= 0.25f;
			}
		}
		myPlayer.GetDamage(damage, typeKill, typeDead, posKiller, weaponName, killerViewId);
	}

	public bool IsEnemyTo(Player_move_c player)
	{
		if (!Defs.isMulti || (!player.Equals(myPlayer) && (GameConnect.isCOOP || (GameConnect.isTeamRegim && myPlayer.myCommand == player.myCommand))))
		{
			return false;
		}
		return true;
	}

	public bool IsDead()
	{
		if (!myPlayer.isKilled)
		{
			return myPlayer.isImmortality;
		}
		return true;
	}
}
