using UnityEngine;

public class CustomPanelConnectScene : MonoBehaviour
{
	private static CustomPanelConnectScene _instance;

	[Header("Custom panel")]
	public GameObject customPanel;

	public SetHeadLabelText headCustomPanel;

	public UIScrollView scrollGames;

	public UIGrid gridGames;

	public UISprite fonGames;

	public Transform gridGamesTransform;

	public DisableObjectFromTimer gameIsfullLabel;

	public DisableObjectFromTimer incorrectPasswordLabel;

	public GameObject gameInfoItemPrefab;

	public GameObject showSearchPanelBtn;

	public GameObject chooseMapLabelSmall;

	public GameObject connectToWiFIInCustomLabel;

	public GameObject createRoomBtn;

	public UIButton createRoomUIBtn;

	[Header("Create panel")]
	public GameObject createPanel;

	public UIInput nameServerInput;

	public UIInput setPasswordInput;

	public DisableObjectFromTimer nameAlreadyUsedLabel;

	public GameObject goToCreateRoomBtn;

	public GameObject connectToWiFIInCreateLabel;

	[Header("Search panel")]
	public GameObject searchPanel;

	public UIInput searchInput;

	public GameObject searchBtn;

	[Header("Enter password")]
	public GameObject enterPasswordPanel;

	public UIInput enterPasswordInput;

	public GameObject joinRoomFromEnterPasswordBtn;

	private bool isFirstUpdateLocalServerList;

	public static CustomPanelConnectScene Instance
	{
		get
		{
			if (_instance == null && ConnectScene.sharedController != null)
			{
				_instance = Object.Instantiate(Resources.Load("ConnectScene/CustomPanelConnectScene") as GameObject).GetComponent<CustomPanelConnectScene>();
				_instance.transform.parent = ConnectScene.sharedController.transform.GetChild(0);
				_instance.transform.localScale = Vector3.one;
				_instance.transform.localPosition = Vector3.zero;
			}
			return _instance;
		}
	}

	private void Update()
	{
		if (!Defs.isInet)
		{
			if (Instance.customPanel.activeSelf)
			{
				if (isFirstUpdateLocalServerList)
				{
					ConnectScene.sharedController.UpdateLocalServersList();
				}
				else
				{
					isFirstUpdateLocalServerList = true;
				}
			}
			connectToWiFIInCreateLabel.SetActive(!CheckLocalAvailability());
			connectToWiFIInCustomLabel.SetActive(!CheckLocalAvailability());
			if (createRoomUIBtn.isEnabled != CheckLocalAvailability())
			{
				createRoomUIBtn.isEnabled = CheckLocalAvailability();
			}
		}
		else
		{
			if (connectToWiFIInCreateLabel.activeSelf)
			{
				connectToWiFIInCreateLabel.SetActive(false);
			}
			if (connectToWiFIInCreateLabel.activeSelf)
			{
				connectToWiFIInCustomLabel.SetActive(false);
			}
		}
	}

	private bool CheckLocalAvailability()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		_instance = null;
	}
}
