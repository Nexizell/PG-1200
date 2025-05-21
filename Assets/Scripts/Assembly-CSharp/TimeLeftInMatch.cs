using UnityEngine;

public class TimeLeftInMatch : MonoBehaviour
{
	public UILabel timeLabel;

	public GameObject waitLabel;

	private void Start()
	{
		if (GameConnect.gameMode == GameConnect.GameMode.Deathmatch || GameConnect.gameMode == GameConnect.GameMode.TimeBattle)
		{
			base.transform.localPosition += Vector3.up * 53f;
		}
	}

	private void Update()
	{
		bool flag = Initializer.players.Count > 0 && !GameConnect.isDaterRegim && !GameConnect.isMiniGame && TimeGameController.sharedController != null && TimeGameController.sharedController.timerToEndMatch > 0.0 && !TimeGameController.sharedController.isEndMatch && TimeGameController.sharedController.timerToEndMatch < 1200.0;
		waitLabel.SetActive(flag && TimeGameController.sharedController.timerToEndMatch < 16.0);
		timeLabel.gameObject.SetActive(flag);
		timeLabel.transform.parent.gameObject.SetActive(flag);
		if (flag)
		{
			float timeDown = (float)TimeGameController.sharedController.timerToEndMatch;
			timeLabel.text = Player_move_c.FormatTime(timeDown);
			timeLabel.color = (waitLabel.activeSelf ? Color.red : Color.white);
		}
	}
}
