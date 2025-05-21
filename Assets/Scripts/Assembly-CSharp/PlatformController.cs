using UnityEngine;

public class PlatformController : MonoBehaviour
{
	public Vector3 fromPoint;

	public Vector3 toPoint;

	public float duration;

	[ReadOnly]
	public byte index;

	[ReadOnly]
	public Transform thisTransform;

	private void Start()
	{
		thisTransform = base.transform;
		UpdatePos();
	}

	private void Update()
	{
		UpdatePos();
	}

	private void UpdatePos()
	{
		float num = (float)(PhotonNetwork.time % (double)duration) / duration * 2f;
		if (num <= 1f)
		{
			thisTransform.position = Vector3.Lerp(fromPoint, toPoint, num);
		}
		else
		{
			thisTransform.position = Vector3.Lerp(toPoint, fromPoint, num - 1f);
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && col.Equals(WeaponManager.sharedManager.myPlayerMoveC.bodyCollayder))
		{
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.OnPlatformEnter(index);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && col.Equals(WeaponManager.sharedManager.myPlayerMoveC.bodyCollayder))
		{
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.OnPlatformExit(index);
		}
	}
}
