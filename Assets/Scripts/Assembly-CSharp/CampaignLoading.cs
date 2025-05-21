using Rilisoft;
using UnityEngine;

public sealed class CampaignLoading : MonoBehaviour
{
	public static readonly string DesignersTestMap = "Coliseum";

	public UITexture backgroundUiTexture;

	public GameObject survivalNotesOverlay;

	public GameObject campaignNotesOverlay;

	public GameObject trainingNotesOverlay;

	public GameObject ordinaryAwardLabel;

	public GameObject stackOfCoinsLabel;

	public UILabel[] levelNameLabels;

	public Texture loadingNote;

	private Texture fonToDraw;

	private Texture plashkaCoins;

	private Rect plashkaCoinsRect;

	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = true;
		string b;
		if (GameConnect.isCampaign)
		{
			if (TrainingController.TrainingCompleted)
			{
				int num = 0;
				LevelBox levelBox = null;
				foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
				{
					if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
					{
						continue;
					}
					levelBox = campaignBox;
					foreach (CampaignLevel level in campaignBox.levels)
					{
						if (level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
						{
							num = campaignBox.levels.IndexOf(level);
							break;
						}
					}
				}
				bool flag = false;
				flag = num >= levelBox.levels.Count - 1;
				bool flag2 = false;
				if (!CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(CurrentCampaignGame.levelSceneName))
				{
					flag2 = true;
				}
				bool flag3 = flag2 && flag;
				b = (flag3 ? "gey_15" : "gey_1");
				if (ordinaryAwardLabel != null)
				{
					ordinaryAwardLabel.SetActive(!flag3);
				}
				if (stackOfCoinsLabel != null)
				{
					stackOfCoinsLabel.SetActive(flag3);
				}
				if (campaignNotesOverlay != null)
				{
					campaignNotesOverlay.SetActive(true);
				}
			}
			else
			{
				b = "Restore";
				if (trainingNotesOverlay != null)
				{
					trainingNotesOverlay.SetActive(true);
				}
			}
		}
		else
		{
			b = "gey_surv";
			if (GameConnect.isSurvival && survivalNotesOverlay != null)
			{
				survivalNotesOverlay.SetActive(true);
			}
			if (GameConnect.isSpeedrun)
			{
				levelNameLabels[0].gameObject.SetActive(true);
				for (int i = 0; i < levelNameLabels.Length; i++)
				{
					levelNameLabels[i].text = LocalizationStore.Get(GameConnect.gameModesLocalizeKey[(int)GameConnect.gameMode]);
				}
			}
		}
		plashkaCoins = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", b));
		float num2 = (float)(TrainingController.TrainingCompleted ? 500 : 484) * Defs.Coef;
		float num3 = (float)(TrainingController.TrainingCompleted ? 244 : 279) * Defs.Coef;
		plashkaCoinsRect = new Rect(((float)Screen.width - num2) / 2f, (float)Screen.height * 0.8f - num3 / 2f, num2, num3);
		string text = "";
		if (!TrainingController.TrainingCompleted)
		{
			text = "Training";
		}
		else if (GameConnect.isSurvival)
		{
			text = Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length];
		}
		else if (GameConnect.isCampaign)
		{
			text = CurrentCampaignGame.levelSceneName;
		}
		else if (GameConnect.isSpeedrun)
		{
			text = "Speedrun";
		}
		else
		{
			Debug.LogError("No scene for this mode!");
		}
		string b2 = "Loading_" + text;
		fonToDraw = Resources.Load<Texture>(ResPath.Combine(Switcher.LoadingInResourcesPath + (Device.isRetinaAndStrong ? "/Hi" : string.Empty), b2));
		if (backgroundUiTexture != null)
		{
			backgroundUiTexture.mainTexture = fonToDraw;
		}
		Invoke("Load", 2f);
	}

	private void Load()
	{
		string text = "";
		if (!TrainingController.TrainingCompleted)
		{
			text = "Training";
		}
		else if (GameConnect.isSurvival)
		{
			text = Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length];
		}
		else if (GameConnect.isCampaign)
		{
			text = CurrentCampaignGame.levelSceneName;
		}
		else if (GameConnect.isSpeedrun)
		{
			text = "Speedrun";
		}
		if (!string.IsNullOrEmpty(text))
		{
			Singleton<SceneLoader>.Instance.LoadScene(text);
		}
	}
}
