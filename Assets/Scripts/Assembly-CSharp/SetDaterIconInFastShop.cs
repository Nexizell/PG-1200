using UnityEngine;

public class SetDaterIconInFastShop : MonoBehaviour
{
	public string daterIconName;

	private void Awake()
	{
		if (GameConnect.isDaterRegim)
		{
			GetComponent<UISprite>().spriteName = daterIconName;
		}
	}
}
