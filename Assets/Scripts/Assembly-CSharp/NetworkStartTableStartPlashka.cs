using UnityEngine;

public class NetworkStartTableStartPlashka : MonoBehaviour
{
	public GameObject plashka;

	private void Start()
	{
		if (GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			base.gameObject.SetActive(false);
			plashka.SetActive(false);
		}
		else if (GameConnect.isCOOP)
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0555;
		}
		else if (GameConnect.isHunger)
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0556;
		}
		else if (GameConnect.isDaterRegim)
		{
			GetComponent<UILabel>().text = LocalizationStore.Get("Key_1539");
		}
		else
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0557;
		}
	}

	private void Update()
	{
	}
}
