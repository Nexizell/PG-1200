using UnityEngine;

public class DaterLikeGadget : ThrowGadget
{
	public GameObject currentGrenade;

	public override string GrenadeGadgetId
	{
		get
		{
			return "Like";
		}
	}

	public DaterLikeGadget()
		: base(null)
	{
	}

	public override void PreUse()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GrenadePress(this);
	}

	public override void Use()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GrenadeFire();
	}

	public override void CreateRocket(WeaponSounds weapon)
	{
		Rocket rocket = Player_move_c.CreateRocket(weapon, Vector3.down * 10000f, Quaternion.identity);
		if (Defs.isMulti)
		{
			bool isInet = Defs.isInet;
		}
		currentGrenade = rocket.gameObject;
	}

	public override void ThrowGrenade()
	{
		if (!(currentGrenade == null))
		{
			Rocket component = currentGrenade.GetComponent<Rocket>();
			float num = ((component.currentRocketSettings != null) ? component.currentRocketSettings.startForce : 150f);
			currentGrenade.GetComponent<Rigidbody>().isKinematic = false;
			currentGrenade.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(0f, -5f, 0f) * (num * WeaponManager.sharedManager.myPlayerMoveC.myTransform.forward));
			currentGrenade.GetComponent<Rigidbody>().useGravity = true;
			component.RunGrenade();
		}
	}
}
