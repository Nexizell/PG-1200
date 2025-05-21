using Rilisoft;
using UnityEngine;

public sealed class BestPlayersPresser : MonoBehaviour
{
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsGUIController component = NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>();
		component.friendsPanel.gameObject.SetActive(false);
		component.leaderboardsView.gameObject.SetActive(true);
		component.RequestLeaderboards();
	}
}
