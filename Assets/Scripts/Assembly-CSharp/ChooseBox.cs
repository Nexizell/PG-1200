using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Unity.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ChooseBox : MonoBehaviour
{
	public static ChooseBox instance;

	public Transform ticketsIndicatorPoint;

	private List<Texture> boxPreviews = new List<Texture>();

	private List<Texture> closedBoxPreviews = new List<Texture>();

	public ChooseBoxNGUIController nguiController;

	public Transform gridTransform;

	private bool _escapePressed;

	private IDisposable _backSubscription;

	public void HandleCoinsShop()
	{
		EventHandler handleBackFromBank = null;
		handleBackFromBank = delegate
		{
			BankController.Instance.BackRequested -= handleBackFromBank;
			nguiController.allInterfaceHolder.SetActive(true);
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
		};
		BankController.Instance.BackRequested += handleBackFromBank;
		nguiController.allInterfaceHolder.SetActive(false);
		BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, "TicketsCurrency");
	}

	private void LoadBoxPreviews()
	{
		for (int i = 0; i < LevelBox.campaignBoxes.Count; i++)
		{
			Texture item = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) as Texture;
			boxPreviews.Add(item);
			Texture item2 = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme + "_closed")) as Texture;
			closedBoxPreviews.Add(item2);
		}
	}

	private void UnloadBoxPreviews()
	{
		boxPreviews.Clear();
		Resources.UnloadUnusedAssets();
	}

	public static string BoxUnlockedStoragerKey(string boxName)
	{
		return string.Format("BoughtCampaignBox_{0}", new object[1] { boxName });
	}

	private static bool IsCampaignBoxBought(string boxName)
	{
		return Storager.getInt(BoxUnlockedStoragerKey(boxName)) > 0;
	}

	private static void SetCampaignBoxBought(string boxName)
	{
		Storager.setInt(BoxUnlockedStoragerKey(boxName), 1);
	}

	private static bool IsBoxEnabled(int boxNumber)
	{
		string boxName = LevelBox.campaignBoxes[boxNumber].name;
		if (IsAchievedBoxByProgress(boxNumber))
		{
			return IsCampaignBoxBought(boxName);
		}
		return false;
	}

	private static bool IsAchievedBoxByProgress(int boxNumber)
	{
		return IsCompletedAllLevelsInBoxPreviousTo(boxNumber);
	}

	public static void UnlockReachedBoxes()
	{
		try
		{
			for (int i = 1; i < LevelBox.campaignBoxes.Count - 1; i++)
			{
				if (IsAchievedBoxByProgress(i))
				{
					SetCampaignBoxBought(LevelBox.campaignBoxes[i].name);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UnlockReachedBoxes: {0}", ex);
		}
	}

	private void Start()
	{
		GameObject gameObject = ticketsIndicatorPoint.gameObject.Descendants().FirstOrDefault((GameObject desc) => desc.GetComponent<UIButton>() != null);
		if (gameObject != null)
		{
			EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(this, "HandleCoinsShop"));
		}
		instance = this;
		if (nguiController.startButton != null)
		{
			ButtonHandler component = nguiController.startButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleStartClicked;
			}
		}
		if (nguiController.backButton != null)
		{
			ButtonHandler component2 = nguiController.backButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += HandleBackClicked;
			}
		}
		StoreKitEventListener.State.Mode = "Campaign";
		StoreKitEventListener.State.Parameters.Clear();
		UpdateBoxesGui();
	}

	private void UpdateBoxesGui()
	{
		int num = Math.Min(LevelBox.campaignBoxes.Count, gridTransform.childCount);
		for (int i = 0; i < num; i++)
		{
			Texture mainTexture = ((i == 0 || IsBoxEnabled(i)) ? Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) : (Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme + "_closed")) ?? Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme))));
			Transform child = gridTransform.GetChild(i);
			child.GetComponent<UITexture>().mainTexture = mainTexture;
			Transform transform = child.FindChild("NeedMoreStarsLabel");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			else
			{
				Debug.LogWarning("Could not find “NeedMoreStarsLabel”.");
			}
			Transform transform2 = child.FindChild("CaptionLabel");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogWarning("Could not find “CaptionLabel”.");
			}
		}
	}

	private void HandleStartClicked(object sender, EventArgs e)
	{
		if (nguiController.selectIndexMap == 0 || IsBoxEnabled(nguiController.selectIndexMap))
		{
			StartNBox(nguiController.selectIndexMap);
		}
	}

	public void StartNameBox(string _nameBox)
	{
		if (_nameBox.Equals("Box_1"))
		{
			StartNBox(0);
		}
		else if (_nameBox.Equals("Box_2"))
		{
			if (IsBoxEnabled(1))
			{
				StartNBox(1);
			}
		}
		else if (_nameBox.Equals("Box_3") && IsBoxEnabled(2))
		{
			StartNBox(2);
		}
	}

	public void StartNBox(int n)
	{
		ButtonClickSound.Instance.PlayClick();
		CurrentCampaignGame.boXName = LevelBox.campaignBoxes[n].name;
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ChooseLevel";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	private void HandleBackClicked(object sender, EventArgs e)
	{
		_escapePressed = true;
	}

	private void OnDestroy()
	{
		instance = null;
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		UnloadBoxPreviews();
	}

	public void HandleUnlockButon()
	{
		int priceInTickets = BalanceController.UnlockCampaignBoxPrice(nguiController.selectIndexMap);
		ItemPrice price = new ItemPrice(priceInTickets, "TicketsCurrency");
		ShopNGUIController.TryToBuy(nguiController.allInterfaceHolder, price, delegate
		{
			SetCampaignBoxBought(LevelBox.campaignBoxes[nguiController.selectIndexMap].name);
			UpdateBoxesGui();
		}, null, null, null, null, null, false, false, delegate
		{
			AnalyticsStuff.TicketsSpended((nguiController.selectIndexMap == 1) ? "Box2" : "Box3", priceInTickets);
		});
	}

	private void Update()
	{
		if (_escapePressed)
		{
			ButtonClickSound.Instance.PlayClick();
			Resources.UnloadUnusedAssets();
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.noteToShow = null;
			LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
			SceneManager.LoadScene(Defs.PromSceneName);
			_escapePressed = false;
		}
		if (nguiController.startButton != null)
		{
			nguiController.startButton.gameObject.SetActive(nguiController.selectIndexMap == 0 || IsBoxEnabled(nguiController.selectIndexMap));
		}
		nguiController.unlockButton.gameObject.SetActive(nguiController.selectIndexMap != 0 && nguiController.selectIndexMap != LevelBox.campaignBoxes.Count - 1 && !IsCampaignBoxBought(LevelBox.campaignBoxes[nguiController.selectIndexMap].name));
		nguiController.unlockButton.isEnabled = nguiController.unlockButton.gameObject.activeSelf && IsAchievedBoxByProgress(nguiController.selectIndexMap) && !IsCampaignBoxBought(LevelBox.campaignBoxes[nguiController.selectIndexMap].name);
		if (nguiController.unlockButton.gameObject.activeInHierarchy)
		{
			RiliExtensions.ForEach(nguiController.unlockPrice, delegate(UILabel label)
			{
				label.text = BalanceController.UnlockCampaignBoxPrice(nguiController.selectIndexMap).ToString();
			});
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
			_escapePressed = true;
		}, "Choose Box");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	public static bool IsCompletedAllLevelsInBoxPreviousTo(int boxIndex)
	{
		if (boxIndex == 0)
		{
			return true;
		}
		bool result = false;
		LevelBox levelBox = LevelBox.campaignBoxes[boxIndex - 1];
		Dictionary<string, int> value;
		if (CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out value))
		{
			if (boxIndex == 1 && value.Count >= 9)
			{
				result = true;
			}
			if (boxIndex == 2 && value.Count >= 6)
			{
				result = true;
			}
			if (boxIndex == 3 && value.Count >= 5)
			{
				result = true;
			}
		}
		return result;
	}
}
