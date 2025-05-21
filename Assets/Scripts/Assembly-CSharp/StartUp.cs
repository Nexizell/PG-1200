using UnityEngine;

public class StartUp : MonoBehaviour
{
	private void Start()
	{
		if (!Application.isEditor)
		{
			AppsFlyer.setAppsFlyerKey("Cja8TmYiYqwrDDFHJykmjD");
		}
	}
}
