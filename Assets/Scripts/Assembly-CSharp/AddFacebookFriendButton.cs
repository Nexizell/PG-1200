using System.Collections.Generic;
using UnityEngine;

public sealed class AddFacebookFriendButton : MonoBehaviour
{
	private void OnClick()
	{
		FriendPreview component = base.transform.parent.GetComponent<FriendPreview>();
		ButtonClickSound.Instance.PlayClick();
		string id = component.id;
		if (id != null)
		{
			if (component.ClanInvite)
			{
				FriendsController.SendPlayerInviteToClan(id);
				FriendsController.sharedController.clanSentInvitesLocal.Add(id);
			}
			else
			{
				Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
				{
					{ "Added Friends", "Find Friends: Facebook" },
					{ "Deleted Friends", "Add" },
					{ "Search Friends", "Add" }
				};
				FriendsController.sharedController.SendInvitation(id, socialEventParameters);
			}
		}
		if (!component.ClanInvite)
		{
			component.DisableButtons();
		}
	}
}
