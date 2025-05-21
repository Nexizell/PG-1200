using UnityEngine;

public class FirecracketsGadget : ImmediateGadget
{
	private float nextHitTime;

	public FirecracketsGadget(GadgetInfo _info)
		: base(_info)
	{
	}

	public override void PreUse()
	{
	}

	public override void Update()
	{
		if (nextHitTime < Time.time && _durationTime.value > 0f)
		{
			nextHitTime = Time.time + 2f;
			WeaponManager.sharedManager.myPlayerMoveC.FirecracketsShot(Info);
		}
	}

	public override void Use()
	{
		StartUseTimer();
		nextHitTime = Time.time + 2f;
	}

	public override void OnTimeExpire()
	{
		StartCooldown();
	}
}
