using System.Collections.Generic;
using UnityEngine;

public sealed class AddFrendsButtonInTableRangs : MonoBehaviour
{
	public int ID;

	private void OnPress(bool isDown)
	{
		if (!isDown)
		{
			Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
			{
				{ "Added Friends", "AddFrendsButtonInTableRangs" },
				{ "Deleted Friends", "Add" }
			};
			FriendsController.sharedController.SendInvitation(ID.ToString(), socialEventParameters);
			if (!FriendsController.sharedController.notShowAddIds.Contains(ID.ToString()))
			{
				FriendsController.sharedController.notShowAddIds.Add(ID.ToString());
			}
			base.gameObject.SetActive(false);
		}
	}
}
