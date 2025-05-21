using UnityEngine;

public class MainMenuPersClickHandler : MonoBehaviour
{
	private void OnClick()
	{
		if ((!(MainMenuController.sharedController != null) || !(MainMenuController.sharedController.mainPanel != null) || MainMenuController.sharedController.mainPanel.activeInHierarchy) && TrainingController.TrainingCompleted)
		{
			if (ProfileController.Instance != null)
			{
				ProfileController.Instance.SetStaticticTab(ProfileStatTabType.Multiplayer);
			}
			MainMenuController.sharedController.GoToProfile();
		}
	}
}
