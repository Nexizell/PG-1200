using UnityEngine;

public class DamageInRadiusEffect : MonoBehaviour
{
	private WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.explosion;

	private Player_move_c.TypeKills typeKilsIconChat = Player_move_c.TypeKills.explosion;

	private WeaponSounds weaponSounds;

	private float raduisDetectTarget = 10f;

	private float hitTime = 1.2f;

	private float damageMultiplayer = 1f;

	private float slowdownCoef = 1f;

	private string weaponName;

	private float nextHitTime;

	private bool active;

	private bool isPoison;

	private bool isSlowdown;

	public void ActivateEffect(float damageMultiplayer, float radiusDetectTarget, float time, string weaponName, WeaponSounds.TypeDead typeDead, Player_move_c.TypeKills typeKills)
	{
		active = true;
		this.damageMultiplayer = damageMultiplayer;
		this.weaponName = weaponName;
		this.typeDead = typeDead;
		typeKilsIconChat = typeKills;
		raduisDetectTarget = radiusDetectTarget;
		hitTime = time;
	}

	public void ActivatePoisonEffect(float damageMultiplayer, float radiusDetectTarget, float time, WeaponSounds weapon, Player_move_c.PoisonType type)
	{
		active = true;
		isPoison = true;
		this.damageMultiplayer = damageMultiplayer;
		weaponSounds = weapon;
		raduisDetectTarget = radiusDetectTarget;
		hitTime = time;
	}

	public void ActivateSlowdonEffect(WeaponSounds weapon)
	{
		active = true;
		isSlowdown = true;
		weaponSounds = weapon;
		slowdownCoef = weapon.slowdownCoeff;
		hitTime = weapon.slowdownTime;
	}

	private void OnDisable()
	{
		active = false;
		isPoison = false;
	}

	private void Update()
	{
		if (active && nextHitTime < Time.time)
		{
			nextHitTime = Time.time + hitTime;
			HitTargets();
		}
	}

	private void HitTargets()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC == null)
		{
			return;
		}
		foreach (Transform item in new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, Defs.isMulti && !isSlowdown))
		{
			if ((item.position - base.transform.position).sqrMagnitude < raduisDetectTarget * raduisDetectTarget)
			{
				if (isPoison)
				{
					WeaponManager.sharedManager.myPlayerMoveC.PoisonShotWithEffect(item.gameObject, damageMultiplayer, weaponSounds);
				}
				else if (isSlowdown)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SlowdownTarget(item.gameObject, hitTime, slowdownCoef);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.DamageTarget(item.gameObject, damageMultiplayer, weaponName, typeDead, typeKilsIconChat);
				}
			}
		}
	}
}
