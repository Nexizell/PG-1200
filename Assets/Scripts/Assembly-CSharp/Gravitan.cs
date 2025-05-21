using UnityEngine;

public class Gravitan : MonoBehaviour
{
	public Vector3 gravitation;

	private void OnTriggerEnter(Collider col)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && col.Equals(WeaponManager.sharedManager.myPlayerMoveC.bodyCollayder))
		{
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.addGravitation = gravitation;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && col.Equals(WeaponManager.sharedManager.myPlayerMoveC.bodyCollayder))
		{
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.addGravitation = Vector3.zero;
		}
	}
}
