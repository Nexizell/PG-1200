using UnityEngine;

public class DuelPlayersPosition : MonoBehaviour
{
	public Transform firstPlayer;

	public Transform secondPlayer;

	private Vector3 firstPlayerStartPosition;

	private Vector3 secondPlayerStartPosition;

	private void Start()
	{
		firstPlayerStartPosition = firstPlayer.localPosition;
		secondPlayerStartPosition = secondPlayer.localPosition;
	}

	private void Update()
	{
		float num = (float)Screen.width / (float)Screen.height - 1.3333334f;
		firstPlayer.localPosition = firstPlayerStartPosition * (1f + num);
		secondPlayer.localPosition = secondPlayerStartPosition * (1f + num);
	}
}
