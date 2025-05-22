using System.Collections.Generic;
using Rilisoft.WP8;
using RilisoftBot;
using UnityEngine;

public class VoodooSnowman : TurretController
{
	private IDamageable voodooTarget;

	private bool targetSelected;

	private float healthToDamage;

	private const float damageCoef = 0.4f;

	private bool myPlayerInTarget;

	private float snowmanTime = 5f;

	public override void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
	{
		if (targetSelected && !voodooTarget.Equals(null) && !voodooTarget.IsDead())
		{
			StopCoroutine(FlashRed());
			StartCoroutine(FlashRed());
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().PlayOneShot(hitSound);
			}
			float num = 0f;
			if (damage > healthToDamage)
			{
				num = healthToDamage;
				healthToDamage = 0f;
			}
			else
			{
				num = damage;
				healthToDamage -= damage;
			}
			voodooTarget.ApplyDamage(num, damageFrom, typeKill, typeDead, gadgetName, killerId);
		}
	}

	public override bool IsEnemyTo(Player_move_c player)
	{
		if (!myPlayerMoveC.Equals(player))
		{
			return false;
		}
		return true;
	}

	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (isReady && (!Defs.isMulti || isMine) && !targetSelected)
		{
			SelectVoodooTarget();
		}
		if (targetSelected && (voodooTarget.Equals(null) || healthToDamage <= 0f || voodooTarget.IsDead()))
		{
			if (GadgetOnKill != null)
			{
				GadgetOnKill();
			}
			SendImKilledRPC();
		}
	}

	private void SelectVoodooTarget()
	{
		if (Defs.isMulti && !GameConnect.isCOOP)
		{
			List<Player_move_c> list = new List<Player_move_c>(9);
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (!Initializer.players[i].Equals(myPlayerMoveC) && !Initializer.players[i].isKilled && !Initializer.players[i].isImmortality && Initializer.players[i].myDamageable.IsEnemyTo(myPlayerMoveC))
				{
					list.Add(Initializer.players[i]);
				}
			}
			if (list.Count > 0)
			{
				Player_move_c player_move_c = list[Random.Range(0, list.Count)];
				if (Defs.isMulti && Defs.isInet)
				{
					photonView.RPC("SetTargetRPC", player_move_c.photonView.owner);
				}
				voodooTarget = player_move_c.myDamageable;
				int armorCountFor = Wear.GetArmorCountFor(player_move_c.mySkinName.currentArmor, player_move_c.mySkinName.currentHat);
				float num = ExperienceController.HealthByLevel[player_move_c.myNetworkStartTable.myRanks];
				healthToDamage = ((float)armorCountFor + num) * 0.4f;
			}
		}
		else if (Initializer.enemiesObj.Count > 0)
		{
			GameObject gameObject = Initializer.enemiesObj[Random.Range(0, Initializer.enemiesObj.Count)];
			if (gameObject != null)
			{
				voodooTarget = gameObject.GetComponent<IDamageable>();
				healthToDamage = gameObject.GetComponent<BaseBot>().baseHealth * 0.4f;
			}
		}
		if (voodooTarget != null)
		{
			targetSelected = true;
		}
	}

	[PunRPC]
	
	public void SetTargetRPC()
	{
		myPlayerInTarget = true;
		WeaponManager.sharedManager.myPlayerMoveC.PlayerEffectRPC(15, snowmanTime);
	}

	protected override void SetParametersFromGadgets(GadgetInfo info)
	{
		base.SetParametersFromGadgets(info);
		if (Defs.isMulti && !isMine)
		{
			snowmanTime = info.Duration;
		}
	}

	protected override void DeactivateTurret()
	{
		base.DeactivateTurret();
		if (myPlayerInTarget)
		{
			myPlayerInTarget = false;
			WeaponManager.sharedManager.myPlayerMoveC.RemoveEffect(Player_move_c.PlayerEffect.voodooSnowman);
		}
	}

	protected override void OnDestroyTurret()
	{
		base.OnDestroyTurret();
		if (myPlayerInTarget)
		{
			myPlayerInTarget = false;
			WeaponManager.sharedManager.myPlayerMoveC.RemoveEffect(Player_move_c.PlayerEffect.voodooSnowman);
		}
	}

	public override void StartTurret()
	{
		base.StartTurret();
	}
}
