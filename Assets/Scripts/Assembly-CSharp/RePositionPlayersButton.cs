using UnityEngine;

public class RePositionPlayersButton : MonoBehaviour
{
	public Vector3 positionInCommand;

	private void Start()
	{
		if (GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			base.transform.localPosition += positionInCommand;
		}
	}
}
