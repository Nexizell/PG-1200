using UnityEngine;

public sealed class AddLabel : MonoBehaviour
{
	private bool isBigPorog;

	private bool isBigPorogOld;

	private void Start()
	{
		if (GameConnect.isCompany || GameConnect.isCOOP)
		{
			base.gameObject.SetActive(false);
		}
	}
}
