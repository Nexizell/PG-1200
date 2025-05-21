using UnityEngine;

public class ScoreTableLabel : MonoBehaviour
{
	private void Start()
	{
		if (GameConnect.isCOOP)
		{
			GetComponent<UILabel>().text = LocalizationStore.Get("Key_0190");
		}
		else if (GameConnect.isFlag)
		{
			GetComponent<UILabel>().text = LocalizationStore.Get("Key_1006");
		}
		else
		{
			GetComponent<UILabel>().text = LocalizationStore.Get("Key_0191");
		}
	}

	private void Update()
	{
	}
}
