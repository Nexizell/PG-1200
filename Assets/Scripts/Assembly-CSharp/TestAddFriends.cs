using System.Collections.Generic;
using UnityEngine;

public sealed class TestAddFriends : MonoBehaviour
{
	private void OnClick()
	{
		Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
		{
			{ "Added Friends", "Test" },
			{ "Deleted Friends", "Add" }
		};
		FriendsController.sharedController.SendInvitation("123", socialEventParameters);
	}
}
