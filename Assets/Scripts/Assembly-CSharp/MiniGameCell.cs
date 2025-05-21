using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

public class MiniGameCell : MonoBehaviour
{
	public GameObject newModeIndicator;

	public List<UILabel> price;

	public GameObject openState;

	public GameObject lockedState;

	public List<UISprite> ratingSprites;

	public List<UILabel> levelRequiredLabels;

	public GameConnect.GameMode type;

	private float lastTimeUpdateNew = float.MinValue;

	public MinigameParameters Parameters { get; set; }

	public void Setup(MinigameParameters pars)
	{
		Parameters = pars;
		bool flag = type == GameConnect.GameMode.Campaign || (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel >= pars.LevelRequired);
		openState.SetActiveSafeSelf(flag);
		lockedState.SetActiveSafeSelf(!flag);
		price[0].text = pars.TicketsPrice.ToString();
		RiliExtensions.ForEach(levelRequiredLabels, delegate(UILabel label)
		{
			label.text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { pars.LevelRequired });
		});
		float num = (float)MiniGameRatingDownloader.Instance.GetRatingOrDefault(type);
		for (int i = 0; i < ratingSprites.Count; i++)
		{
			ratingSprites[i].fillAmount = Mathf.Clamp01(num - (float)i);
		}
		GetComponent<UIButton>().isEnabled = flag;
	}

	private void Update()
	{
		try
		{
			if (newModeIndicator != null && Time.realtimeSinceStartup - lastTimeUpdateNew >= 1f)
			{
				newModeIndicator.SetActiveSafeSelf(MainMenuController.sharedController.NewAvailableMiniGameModes().Contains((int)type));
				lastTimeUpdateNew = Time.realtimeSinceStartup;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in updating unlocked on mini game: {0}", ex);
		}
	}
}
