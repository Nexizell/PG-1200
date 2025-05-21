using UnityEngine;

public class HideInLocal : MonoBehaviour
{
	private void Start()
	{
		if (!Defs.isInet || GameConnect.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
	}
}
