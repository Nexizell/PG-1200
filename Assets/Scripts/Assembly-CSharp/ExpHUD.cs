using UnityEngine;

public class ExpHUD : MonoBehaviour
{
	public UILabel lbCurLev;

	public UILabel lbExp;

	public UITexture txExp;

	private void OnEnable()
	{
		ExpController.Instance.ExpHudIsVisible = false;
		UpdateHud();
	}

	private void OnDisable()
	{
		if (ExpController.Instance == null)
		{
			Debug.LogWarning("ExpController.Instance == null");
		}
		else
		{
			ExpController.Instance.ExpHudIsVisible = true;
		}
	}

	private void UpdateHud()
	{
		lbCurLev.text = ExperienceController.sharedController.currentLevel.ToString();
		lbExp.text = ExpController.ExpToString();
		if (ExperienceController.sharedController.currentLevel == 36)
		{
			txExp.fillAmount = 1f;
		}
		else
		{
			txExp.fillAmount = ExpController.ProgressExpInPer();
		}
	}
}
