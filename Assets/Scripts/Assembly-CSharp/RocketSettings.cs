using UnityEngine;
using ZeichenKraftwerk;

public class RocketSettings : MonoBehaviour
{
	public enum TypeFlyRocket
	{
		Rocket = 0,
		Grenade = 1,
		Bullet = 2,
		MegaBullet = 3,
		Autoaim = 4,
		Bomb = 5,
		AutoaimBullet = 6,
		Ball = 7,
		GravityRocket = 8,
		Lightning = 9,
		AutoTarget = 10,
		StickyBomb = 11,
		Ghost = 12,
		ChargeRocket = 13,
		ToxicBomb = 14,
		GrenadeBouncing = 15,
		SingularityGrenade = 16,
		NuclearGrenade = 17,
		StickyMine = 18,
		Molotov = 19,
		Drone = 20,
		FakeBonus = 21,
		BlackMark = 22,
		Firework = 23,
		HomingGrenade = 24,
		SlowdownGrenade = 25
	}

	[Header("General settings")]
	public TypeFlyRocket typeFly;

	public WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.explosion;

	public Player_move_c.TypeKills typeKilsIconChat = Player_move_c.TypeKills.explosion;

	public float lifeTime = 7f;

	public float startForce = 190f;

	[Header("Particles")]
	public GameObject flyParticle;

	public TrailRenderer trail;

	public Rotator droneRotator;

	public GameObject droneParticle;

	[Header("Size detect collider")]
	public Vector3 sizeBoxCollider = new Vector3(0.15f, 0.15f, 0.75f);

	public Vector3 centerBoxCollider = new Vector3(0f, 0f, 0f);

	[Header("For AutoTarget, Autoaim")]
	public float autoRocketForce = 15f;

	[Header("For AutoTarget, StickyBomb, ToxicBomb")]
	public float raduisDetectTarget = 5f;

	[Header("For AutoTarget, StickyBomb, ToxicBomb")]
	public float toxicHitTime = 1f;

	public float toxicDamageMultiplier = 0.2f;

	[Header("For StickyBomb, ToxicBomb")]
	public GameObject stickedParticle;

	[Header("For Lightning")]
	public int countJumpLightning = 2;

	public float raduisDetectTargetLightning = 5f;

	[Header("For charge weapon")]
	public float chargeScaleMin = 0.7f;

	public float chargeScaleMax = 1.2f;
}
