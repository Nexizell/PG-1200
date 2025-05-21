using UnityEngine;

public class DestroyGameObjectOnPixelgunLow : MonoBehaviour
{
	private void Awake()
	{
		if (Device.isPixelGunLow)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
