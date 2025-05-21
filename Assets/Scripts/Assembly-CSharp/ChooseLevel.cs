using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public sealed class ChooseLevel : MonoBehaviour
{
	internal sealed class LevelInfo
	{
		public bool Enabled { get; set; }

		public string Name { get; set; }

		public int StarGainedCount { get; set; }
	}

	public GameObject BonusGun3Box;

	public GameObject panel;

	public GameObject[] starEnabledPrototypes;

	public GameObject[] starDisabledPrototypes;

	public GameObject gainedStarCountLabel;

	public GameObject backButton;

	public BankShopViewGuiElement shopView;

	public ButtonHandler nextButton;

	public GameObject[] boxOneLevelButtons;

	public GameObject[] boxTwoLevelButtons;

	public GameObject[] boxThreeLevelButtons;

	public AudioClip shopButtonSound;

	public GameObject backgroundHolder;

	public GameObject backgroundHolder_2;

	public GameObject backgroundHolder_3;

	public GameObject[] boxContents;

	public static ChooseLevel sharedChooseLevel;

	private float _timeStarted;

	private IDisposable _backSubscription;

	private int _boxIndex;

	private GameObject[] _boxLevelButtons;

	private string _gainedStarCount = string.Empty;

	private IList<LevelInfo> _levelInfos = new List<LevelInfo>();

	public ShopNGUIController _shopInstance;

	private float _timeWhenShopWasClosed;

	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = false;
		ExperienceController.RefreshExpControllers();
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		StoreKitEventListener.State.PurchaseKey = "In Map";
		StoreKitEventListener.State.Parameters.Clear();
		sharedChooseLevel = this;
		_timeStarted = Time.realtimeSinceStartup;
		bool draggableLayout = false;
		_boxIndex = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == CurrentCampaignGame.boXName);
		if (_boxIndex == -1)
		{
			Debug.LogWarning("Box not found in list!");
			throw new InvalidOperationException("Box not found in list!");
		}
		bool flag = true;
		_levelInfos = (flag ? InitializeLevelInfos(draggableLayout) : InitializeLevelInfosWithTestData(draggableLayout));
		_gainedStarCount = InitializeGainStarCount(_levelInfos);
		if (CurrentCampaignGame.boXName == "Real")
		{
			_boxLevelButtons = boxOneLevelButtons;
			backgroundHolder.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName == "minecraft")
		{
			_boxLevelButtons = boxTwoLevelButtons;
			backgroundHolder_2.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName == "Crossed")
		{
			_boxLevelButtons = boxThreeLevelButtons;
			backgroundHolder_3.SetActive(true);
			string[] array = Storager.getString(Defs.WeaponsGotInCampaign).Split('#');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					array[i] = string.Empty;
				}
			}
			BonusGun3Box.SetActive(array == null || !array.Contains(WeaponManager.BugGunWN));
		}
		else
		{
			Debug.LogWarning("Unknown box: " + CurrentCampaignGame.boXName);
		}
		InitializeLevelButtons();
		InitializeFixedDisplay();
		InitializeNextButton(_levelInfos, nextButton);
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(delegate
		{
			if (_shopInstance == null)
			{
				HandleBackButton(this, EventArgs.Empty);
			}
		}, "Choose Level");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void InitializeNextButton(IList<LevelInfo> levels, ButtonHandler nextButton)
	{
		if (levels == null)
		{
			throw new ArgumentNullException("levels");
		}
		if (nextButton == null)
		{
			throw new ArgumentNullException("nextButton");
		}
		LevelInfo level = levels.LastOrDefault((LevelInfo l) => l.Enabled && l.StarGainedCount == 0);
		nextButton.gameObject.SetActive(level != null);
		if (level != null)
		{
			nextButton.Clicked += delegate
			{
				HandleLevelButton(level.Name);
			};
		}
	}

	private void InitializeFixedDisplay()
	{
		if (backButton != null)
		{
			backButton.GetComponent<ButtonHandler>().Clicked += HandleBackButton;
		}
		if (shopView != null)
		{
			shopView.Clicked += HandleShopButton;
		}
		if (gainedStarCountLabel != null)
		{
			gainedStarCountLabel.GetComponent<UILabel>().text = _gainedStarCount;
		}
	}

	private void HandleBackButton(object sender, EventArgs args)
	{
		if (!(_shopInstance != null) && !(Time.time - _timeWhenShopWasClosed < 1f))
		{
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = "CampaignChooseBox";
			LoadConnectScene.noteToShow = null;
			Application.LoadLevel(Defs.PromSceneName);
		}
	}

	private void HandleShopButton(object sender, EventArgs args)
	{
		if (!(_shopInstance == null))
		{
			return;
		}
		_shopInstance = ShopNGUIController.sharedShop;
		if (_shopInstance != null)
		{
			_shopInstance.SetInGame(false);
			ShopNGUIController.GuiActive = true;
			if (shopButtonSound != null && Defs.isSoundFX)
			{
				NGUITools.PlaySound(shopButtonSound);
			}
			panel.gameObject.SetActive(false);
			_shopInstance.resumeAction = HandleResumeFromShop;
		}
	}

	private void HandleResumeFromShop()
	{
		panel.gameObject.SetActive(true);
		if (_shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
			}
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance = null;
			_timeWhenShopWasClosed = Time.time;
		}
	}

	private void InitializeLevelButtons()
	{
		if (starEnabledPrototypes != null)
		{
			GameObject[] array = starEnabledPrototypes;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (starDisabledPrototypes != null)
		{
			GameObject[] array = starDisabledPrototypes;
			foreach (GameObject gameObject2 in array)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
		}
		if (boxContents != null)
		{
			for (int j = 0; j != boxContents.Length; j++)
			{
				boxContents[j].SetActive(j == _boxIndex);
			}
			if (_boxLevelButtons == null)
			{
				throw new InvalidOperationException("Box level buttons are null.");
			}
			GameObject[] array = _boxLevelButtons;
			foreach (GameObject gameObject3 in array)
			{
				if (gameObject3 != null)
				{
					UIButton component = gameObject3.GetComponent<UIButton>();
					if (component != null)
					{
						component.isEnabled = false;
					}
				}
			}
			int num = Math.Min(_levelInfos.Count, _boxLevelButtons.Length);
			for (int k = 0; k != num; k++)
			{
				LevelInfo levelInfo = _levelInfos[k];
				GameObject gameObject4 = _boxLevelButtons[k];
				gameObject4.transform.parent = gameObject4.transform.parent;
				gameObject4.GetComponent<UIButton>().isEnabled = levelInfo.Enabled;
				UISprite componentInChildren = gameObject4.GetComponentInChildren<UISprite>();
				if (componentInChildren == null)
				{
					Debug.LogWarning("Could not find background of level button.");
				}
				else
				{
					UILabel componentInChildren2 = componentInChildren.GetComponentInChildren<UILabel>();
					if (componentInChildren2 == null)
					{
						Debug.LogWarning("Could not find caption of level button.");
					}
					else
					{
						componentInChildren2.applyGradient = levelInfo.Enabled;
					}
				}
				gameObject4.AddComponent<ButtonHandler>();
				string levelName = levelInfo.Name;
				gameObject4.GetComponent<ButtonHandler>().Clicked += delegate
				{
					HandleLevelButton(levelName);
				};
				gameObject4.SetActive(true);
				for (int l = 0; l != starEnabledPrototypes.Length; l++)
				{
					if (levelInfo.Enabled)
					{
						GameObject gameObject5 = starEnabledPrototypes[l];
						if (!(gameObject5 == null))
						{
							GameObject obj = UnityEngine.Object.Instantiate(gameObject5);
							obj.transform.parent = gameObject4.transform;
							obj.GetComponent<UIToggle>().value = l < levelInfo.StarGainedCount;
							obj.transform.localPosition = gameObject5.transform.localPosition;
							obj.transform.localScale = gameObject5.transform.localScale;
							obj.SetActive(true);
						}
					}
				}
			}
			array = starEnabledPrototypes;
			foreach (GameObject gameObject6 in array)
			{
				if (gameObject6 != null)
				{
					UnityEngine.Object.Destroy(gameObject6);
				}
			}
			array = starDisabledPrototypes;
			foreach (GameObject gameObject7 in array)
			{
				if (gameObject7 != null)
				{
					UnityEngine.Object.Destroy(gameObject7);
				}
			}
			return;
		}
		throw new InvalidOperationException("boxContents == 0");
	}

	private void HandleLevelButton(string levelName)
	{
		if (!(_shopInstance != null) && !(Time.realtimeSinceStartup - _timeStarted < 0.15f))
		{
			CurrentCampaignGame.levelSceneName = levelName;
			WeaponManager.sharedManager.Reset();
			LevelArt.endOfBox = false;
			Singleton<SceneLoader>.Instance.LoadScene(LevelArt.ShouldShowArts ? "LevelArt" : "CampaignLoading");
		}
	}

	private static IList<LevelInfo> InitializeLevelInfosWithTestData(bool draggableLayout = false)
	{
		return new List<LevelInfo>
		{
			new LevelInfo
			{
				Enabled = true,
				Name = "Cementery",
				StarGainedCount = 1
			},
			new LevelInfo
			{
				Enabled = true,
				Name = "City",
				StarGainedCount = 3
			},
			new LevelInfo
			{
				Enabled = false,
				Name = "Hospital"
			}
		};
	}

	private static IList<LevelInfo> InitializeLevelInfos(bool draggableLayout = false)
	{
		List<LevelInfo> list = new List<LevelInfo>();
		string boxName = CurrentCampaignGame.boXName;
		int num = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == boxName);
		if (num == -1)
		{
			Debug.LogWarning("Box not found in list!");
			return list;
		}
		List<CampaignLevel> levels = LevelBox.campaignBoxes[num].levels;
		Dictionary<string, int> value;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(boxName, out value))
		{
			Debug.LogWarning("Box not found in dictionary: " + boxName);
			value = new Dictionary<string, int>();
		}
		for (int i = 0; i != levels.Count; i++)
		{
			string sceneName = levels[i].sceneName;
			int value2 = 0;
			value.TryGetValue(sceneName, out value2);
			LevelInfo item = new LevelInfo
			{
				Enabled = (i <= value.Count),
				Name = sceneName,
				StarGainedCount = value2
			};
			list.Add(item);
		}
		return list;
	}

	private static string InitializeGainStarCount(IList<LevelInfo> levelInfos)
	{
		int num = 3 * levelInfos.Count;
		int num2 = 0;
		foreach (LevelInfo levelInfo in levelInfos)
		{
			num2 += levelInfo.StarGainedCount;
		}
		return string.Format("{0}/{1}", new object[2] { num2, num });
	}

	public static string GetGainStarsString()
	{
		return InitializeGainStarCount(InitializeLevelInfos());
	}

	private void OnDestroy()
	{
		sharedChooseLevel = null;
	}
}
