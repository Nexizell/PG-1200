using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PetInfo 
{
	public enum BehaviourType
	{
		Ground = 0,
		Flying = 1
	}

	public enum Parameter
	{
		Durability = 0,
		Attack = 1,
		Cooldown = 2
	}

	public enum Effect
	{
		Healing = 0,
		Flamethrower = 1,
		Poison = 2
	}

	public List<GadgetInfo.Parameter> Parameters;

	public List<WeaponSounds.Effects> Effects;

	public string Id;

	public int Up;

	public string IdWithoutUp;

	public BehaviourType Behaviour;

	public ItemDb.ItemRarity Rarity;

	public int Tier;

	public string Lkey;

	public int ToUpPoints;

	public float AttackDistance;

	public float AttackStopDistance;

	public float MinToOwnerDistance;

	public float MaxToOwnerDistance;

	public float TargetDetectRange;

	public float OffenderDetectRange = 10f;

	public float ToTargetTeleportDistance;

	public bool poisonEnabled;

	public Player_move_c.PoisonType poisonType;

	public int poisonCount;

	public float poisonTime;

	public float poisonDamagePercent;

	public int criticalHitChance;

	public float criticalHitCoef;

	public bool petExplode;

	public float explosionRadius;

	public float explosionImpulse;

	public float explosionDamageMultiplier;

	public string explosionPrefabName;

	public float explosionDelay = 0.5f;

	private Vector3 m_positionInBanners;

	private Vector3 m_rotationInBanners;

	public float HealDelaySecs;

	public float HealPowerSelf;

	public float HealPowerOwner;

	public float HP
	{
		get
		{
			if (BalanceController.hpPets.ContainsKey(Id))
			{
				return BalanceController.hpPets[Id];
			}
			return 9f;
		}
	}

	public float Attack
	{
		get
		{
			if (BalanceController.damagePets.ContainsKey(Id))
			{
				return BalanceController.damagePets[Id];
			}
			return 1f;
		}
	}

	public float SurvivalAttack
	{
		get
		{
			if (BalanceController.survivalDamagePets.ContainsKey(Id))
			{
				return BalanceController.survivalDamagePets[Id];
			}
			return 1f;
		}
	}

	public int DPS
	{
		get
		{
			if (BalanceController.dpsPets.ContainsKey(Id))
			{
				return BalanceController.dpsPets[Id];
			}
			return 1;
		}
	}

	public float SpeedModif
	{
		get
		{
			if (BalanceController.speedPets.ContainsKey(Id))
			{
				return BalanceController.speedPets[Id];
			}
			return 4f;
		}
	}

	public float RespawnTime
	{
		get
		{
			if (BalanceController.respawnTimePets.ContainsKey(Id))
			{
				return BalanceController.respawnTimePets[Id];
			}
			return 4f;
		}
	}

	public float Cashback
	{
		get
		{
			if (BalanceController.cashbackPets.ContainsKey(Id))
			{
				return BalanceController.cashbackPets[Id];
			}
			return 4f;
		}
	}

	public Vector3 PositionInBanners
	{
		get
		{
			return m_positionInBanners;
		}
		set
		{
			m_positionInBanners = value;
		}
	}

	public Vector3 RotationInBanners
	{
		get
		{
			return m_rotationInBanners;
		}
		set
		{
			m_rotationInBanners = value;
		}
	}

	public string GetRelativePrefabPath()
	{
		return string.Format("Pets/Content/{0}_up0", new object[1] { IdWithoutUp });
	}
}
