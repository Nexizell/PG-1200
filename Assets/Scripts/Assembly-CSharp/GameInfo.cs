using UnityEngine;

public class GameInfo : MonoBehaviour
{
	public GameObject openSprite;

	public GameObject closeSprite;

	public SetHeadLabelText mapNameLabel;

	public UITexture mapTexture;

	public UILabel countPlayersLabel;

	public UILabel serverNameLabel;

	public RoomInfo roomInfo;

	public LANBroadcastService.ReceivedMessage roomInfoLocal;

	public int index;

	public GameObject[] SizeMapNameLbl;

	private void Start()
	{
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (ConnectScene.sharedController != null)
		{
			if (Defs.isInet)
			{
				ConnectScene.sharedController.JoinToRoomPhoton(roomInfo);
			}
			else
			{
				ConnectScene.sharedController.JoinToLocalRoom(roomInfoLocal);
			}
		}
	}
}
