using Rilisoft;
using UnityEngine;

public sealed class ExpView : MonoBehaviour
{
	public UIRoot interfaceHolder;

	public Camera experienceCamera;

	public UISprite experienceFrame;

	[SerializeField]
	protected internal PrefabHandler _levelUpPanelPrefab;

	[SerializeField]
	protected internal PrefabHandler _levelUpPanelTierPrefab;

	public GameObject objHUD;

	[SerializeField]
	protected internal GameObject _levelUpPanelsContainer;

	private LazyObject<LevelUpWithOffers> _levelUpPanelValue;

	private LazyObject<LevelUpWithOffers> _levelUpPanelTierValue;

	private LevelUpWithOffers _currentLevelUpPanel;

	private LeveUpPanelShowOptions _levelUpPanelOptions;

	public bool LevelUpPanelOpened
	{
		get
		{
			if (!_levelUpPanel.ObjectIsActive)
			{
				return _levelUpPanelTier.ObjectIsActive;
			}
			return true;
		}
	}

	public LevelUpWithOffers CurrentVisiblePanel
	{
		get
		{
			if (_levelUpPanel.ObjectIsActive)
			{
				return _levelUpPanel.Value;
			}
			if (_levelUpPanelTier.ObjectIsActive)
			{
				return _levelUpPanelTier.Value;
			}
			return null;
		}
	}

	private LazyObject<LevelUpWithOffers> _levelUpPanel
	{
		get
		{
			if (_levelUpPanelValue == null)
			{
				_levelUpPanelValue = new LazyObject<LevelUpWithOffers>(_levelUpPanelPrefab.ResourcePath, _levelUpPanelsContainer);
			}
			return _levelUpPanelValue;
		}
	}

	private LazyObject<LevelUpWithOffers> _levelUpPanelTier
	{
		get
		{
			if (_levelUpPanelTierValue == null)
			{
				_levelUpPanelTierValue = new LazyObject<LevelUpWithOffers>(_levelUpPanelTierPrefab.ResourcePath, _levelUpPanelsContainer);
			}
			return _levelUpPanelTierValue;
		}
	}

	public bool VisibleHUD
	{
		get
		{
			return objHUD.activeSelf;
		}
		set
		{
			objHUD.SetActive(value);
		}
	}

	public LeveUpPanelShowOptions LevelUpPanelOptions
	{
		get
		{
			if (_levelUpPanelOptions == null)
			{
				_levelUpPanelOptions = new LeveUpPanelShowOptions();
			}
			return _levelUpPanelOptions;
		}
	}

	private void Awake()
	{
		Singleton<SceneLoader>.Instance.OnSceneLoading += delegate
		{
			_levelUpPanel.DestroyValue();
			_levelUpPanelTier.DestroyValue();
		};
	}

	public void ShowLevelUpPanel()
	{
		_currentLevelUpPanel = (LevelUpPanelOptions.ShowTierView ? _levelUpPanelTier.Value : _levelUpPanel.Value);
		_currentLevelUpPanel.SetCurrentRank(LevelUpPanelOptions.CurrentRank.ToString());
		_currentLevelUpPanel.SetRewardPrice("+" + LevelUpPanelOptions.CoinsReward + "\n" + LocalizationStore.Get("Key_0275"));
		_currentLevelUpPanel.SetGemsRewardPrice("+" + LevelUpPanelOptions.GemsReward + "\n" + LocalizationStore.Get("Key_0951"));
		_currentLevelUpPanel.SetAddHealthCount(string.Format(LocalizationStore.Get("Key_1856"), new object[1] { ExperienceController.sharedController.AddHealthOnCurLevel.ToString() }));
		_currentLevelUpPanel.SetItems(LevelUpPanelOptions.NewItems);
		_currentLevelUpPanel.shareScript.share.IsChecked = LevelUpPanelOptions.ShareButtonEnabled;
		ExpController.ShowTierPanel(_currentLevelUpPanel.gameObject);
		_currentLevelUpPanel.RunArmorAnimation(LevelUpPanelOptions.IdOfGivenPreviousTierBestArmor);
	}

	public void ToBonus(int starterGemsReward, int starterCoinsReward)
	{
		if (_currentLevelUpPanel != null)
		{
			_currentLevelUpPanel.SetStarterBankValues(starterGemsReward, starterCoinsReward);
			_currentLevelUpPanel.shareScript.animatorLevel.SetTrigger("Bonus");
		}
	}
}
