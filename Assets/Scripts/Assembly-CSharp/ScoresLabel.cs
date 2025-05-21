using UnityEngine;

public class ScoresLabel : MonoBehaviour
{
	private UILabel _label;

	private bool isHunger;

	private string scoreLocalize;

	private void Start()
	{
		isHunger = GameConnect.isHunger;
		base.gameObject.SetActive(GameConnect.isSurvival || GameConnect.isCOOP || isHunger || GameConnect.isSpeedrun);
		_label = GetComponent<UILabel>();
		scoreLocalize = (isHunger ? LocalizationStore.Key_0351 : LocalizationStore.Key_0190);
	}

	private void Update()
	{
		if (isHunger)
		{
			_label.text = string.Format("{0}", new object[1] { (Initializer.players != null) ? (Initializer.players.Count - 1) : 0 });
		}
		else
		{
			_label.text = string.Format("{0}", new object[1] { GlobalGameController.Score });
		}
	}
}
