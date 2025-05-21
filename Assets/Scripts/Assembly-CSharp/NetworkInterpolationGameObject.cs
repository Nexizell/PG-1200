using UnityEngine;

public class NetworkInterpolationGameObject : MonoBehaviour
{
	private Quaternion correctPlayerRot = Quaternion.identity;

	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
	}
}
