using UnityEngine;

public class StartHeadUp : MonoBehaviour
{
	private void Start()
	{
		if (GameConnect.isDaterRegim)
		{
			GetComponent<UILabel>().text = string.Empty;
		}
		else if (!Defs.isInet || (Defs.isInet && PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[GameConnect.passwordProperty].Equals("")))
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0560;
		}
		else if (GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0561;
		}
		else if (GameConnect.isCOOP)
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0562;
		}
		else
		{
			GetComponent<UILabel>().text = LocalizationStore.Key_0563;
		}
	}
}
