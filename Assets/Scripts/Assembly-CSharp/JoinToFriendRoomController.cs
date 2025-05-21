using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class JoinToFriendRoomController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetFonLoadingWaitForReset_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public JoinToFriendRoomController _003C_003E4__this;

		public string _mapName;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSetFonLoadingWaitForReset_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E4__this.RemoveLoadingGUI();
				goto IL_0046;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0046;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0046:
				if (WeaponManager.sharedManager == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			if (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			_003C_003E4__this.ShowLoadingGUI(_mapName);
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CMoveToGameScene_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private string _003CmapName_003E5__1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMoveToGameScene_003Ed__46(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			AsyncOperation asyncOperation;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (SceneLoader.ActiveSceneName.Equals("Clans"))
				{
					Defs.isGameFromFriends = false;
					Defs.isGameFromClans = true;
				}
				else
				{
					Defs.isGameFromFriends = true;
					Defs.isGameFromClans = false;
				}
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
				_003CmapName_003E5__1 = infoScene.NameScene;
				WeaponManager.sharedManager.Reset((int)infoScene.AvaliableWeapon);
				goto IL_00b0;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b0;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00b0:
				if (PhotonNetwork.room == null)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				UnityEngine.Debug.Log("map = " + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
				UnityEngine.Debug.Log(_003CmapName_003E5__1);
				LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _003CmapName_003E5__1) as Texture2D;
				LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
				LoadConnectScene.sceneToLoad = _003CmapName_003E5__1;
				LoadConnectScene.noteToShow = null;
				asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
				_003C_003E2__current = asyncOperation;
				_003C_003E1__state = 2;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public int gameMode;

	public string roomName;

	public GameObject connectPanel;

	public UITexture backgroundConnectTexture;

	public UILabel infoBoxLabel;

	public GameObject infoBoxContainer;

	public GameObject passwordPanel;

	public GameObject wrongPasswordLabel;

	public UIInput inputPasswordLabel;

	public static JoinToFriendRoomController Instance;

	private bool _isFaledConnectToRoom;

	private string _passwordRoom;

	private float _timerShowWrongPassword;

	private bool _isBackFromPassword;

	private IDisposable _backSubscription;

	private LoadingNGUIController _loadingNGUIController;

	private void Awake()
	{
		inputPasswordLabel.onSubmit.Add(new EventDelegate(delegate
		{
			EnterPassword(inputPasswordLabel.value);
		}));
	}

	private void Start()
	{
		Instance = this;
	}

	private void OnEnable()
	{
		SubscribeToNetworkEvents();
	}

	private void OnEsc()
	{
		GameConnect.Disconnect();
		closeConnectPanel();
		ResetGameMode();
	}

	private void OnDisable()
	{
		UnsubscribeToNetworkEvents();
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void SubscribeToNetworkEvents()
	{
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Combine(GameConnect.OnJoinRoomFailed, new Action<bool>(OnPhotonJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Combine(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnectToPhoton));
		GameConnect.OnConnectedMaster = (Action)Delegate.Combine(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Combine(GameConnect.OnDisconnected, new Action(OnDisconnectedFromPhoton));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Combine(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
	}

	private void UnsubscribeToNetworkEvents()
	{
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Remove(GameConnect.OnJoinRoomFailed, new Action<bool>(OnPhotonJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Remove(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnectToPhoton));
		GameConnect.OnConnectedMaster = (Action)Delegate.Remove(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Remove(GameConnect.OnDisconnected, new Action(OnDisconnectedFromPhoton));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Remove(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
	}

	public void BackFromPasswordButton()
	{
		_isBackFromPassword = true;
		SetEnabledPasswordPanel(false);
		GameConnect.Disconnect();
		ResetGameMode();
	}

	public void OnClickAcceptPassword()
	{
		EnterPassword(inputPasswordLabel.value);
	}

	public void EnterPassword(string pass)
	{
		if (pass == _passwordRoom)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			StartCoroutine(MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
		else
		{
			_timerShowWrongPassword = 3f;
			wrongPasswordLabel.SetActive(true);
		}
	}

	private void ShowLoadingGUI(string _mapName)
	{
		_loadingNGUIController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		_loadingNGUIController.SceneToLoad = _mapName;
		_loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _mapName);
		_loadingNGUIController.transform.parent = backgroundConnectTexture.transform.parent;
		_loadingNGUIController.transform.localPosition = Vector3.zero;
		_loadingNGUIController.Init();
	}

	private void RemoveLoadingGUI()
	{
		if (_loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(_loadingNGUIController.gameObject);
			_loadingNGUIController = null;
		}
	}

	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		RemoveLoadingGUI();
		while (WeaponManager.sharedManager == null)
		{
			yield return null;
		}
		while (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
		{
			yield return null;
		}
		ShowLoadingGUI(_mapName);
	}

	public void ConnectToRoom(int gameModeCode, string nameRoom, string map)
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(OnEsc, "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(map));
		if (infoScene.isPremium && Storager.getInt(infoScene.NameScene + "Key") != 1 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
		{
			Action successfulUnlockCallback = delegate
			{
			};
			ShowUnlockMapDialog(successfulUnlockCallback, infoScene.NameScene);
			return;
		}
		int tier = ((gameModeCode > 99) ? (gameModeCode % 10000 / 100) : (gameModeCode / 10));
		this.gameMode = gameModeCode % 100;
		roomName = nameRoom;
		Defs.isMulti = true;
		Defs.isInet = true;
		GameConnect.GameMode gameMode = (GameConnect.GameMode)this.gameMode;
		if (gameMode != GameConnect.GameMode.InClanWindow && gameMode != GameConnect.GameMode.InFriendWindow)
		{
			GameConnect.SetGameMode(gameMode);
			ActivityIndicator.IsActiveIndicator = true;
			connectPanel.SetActive(true);
			infoBoxLabel.gameObject.SetActive(false);
			infoBoxContainer.SetActive(false);
			WeaponManager.sharedManager.Reset((int)infoScene.AvaliableWeapon);
			StartCoroutine(SetFonLoadingWaitForReset(infoScene.NameScene));
			PhotonNetwork.autoJoinLobby = false;
			GameConnect.ConnectToPhoton(tier);
		}
		else
		{
			UnityEngine.Debug.LogError("Wrong gamemode!");
		}
	}

	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = coinsShop.thisScript != null && coinsShop.thisScript.enabled;
		}
		if (_timerShowWrongPassword > 0f && wrongPasswordLabel.activeSelf)
		{
			_timerShowWrongPassword -= Time.deltaTime;
		}
		if (_timerShowWrongPassword <= 0f && wrongPasswordLabel.activeSelf)
		{
			wrongPasswordLabel.SetActive(false);
		}
	}

	private void ShowUnlockMapDialog(Action successfulUnlockCallback, string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			UnityEngine.Debug.LogWarning("Level name shoul not be empty.");
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UnlockPremiumMapView")) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		Tools.SetLayerRecursively(gameObject, base.gameObject.layer);
		ActivityIndicator.IsActiveIndicator = false;
		UnlockPremiumMapView unlockPremiumMapView = gameObject.GetComponent<UnlockPremiumMapView>();
		if (unlockPremiumMapView == null)
		{
			UnityEngine.Debug.LogError("UnlockPremiumMapView should not be null.");
			return;
		}
		int value = 0;
		Defs.PremiumMaps.TryGetValue(levelName, out value);
		unlockPremiumMapView.Price = value;
		EventHandler value2 = delegate
		{
			HandleCloseUnlockDialog(unlockPremiumMapView);
		};
		EventHandler value3 = delegate
		{
			HandleUnlockPressed(unlockPremiumMapView, successfulUnlockCallback, levelName);
		};
		unlockPremiumMapView.ClosePressed += value2;
		unlockPremiumMapView.UnlockPressed += value3;
	}

	private void HandleCloseUnlockDialog(UnlockPremiumMapView unlockPremiumMapView)
	{
		closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int priceAmount = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, new ItemPrice(unlockPremiumMapView.Price, "Coins"), delegate
		{
			Storager.setInt(levelName + "Key", 1);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			AnalyticsStuff.LogSales(levelName, "Premium Maps");
			AnalyticsFacade.InAppPurchase(levelName, "Premium Maps", 1, priceAmount, "Coins");
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			successfulUnlockCallback();
			UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
		}, delegate
		{
			StoreKitEventListener.State.PurchaseKey = "In map selection In Friends";
		});
	}

	public void closeConnectPanel()
	{
		CancelInvoke("closeConnectPanel");
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		backgroundConnectTexture.mainTexture = null;
		RemoveLoadingGUI();
		connectPanel.SetActive(false);
		infoBoxLabel.gameObject.SetActive(false);
		infoBoxContainer.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void ShowLabel(string text)
	{
		if (connectPanel.activeSelf)
		{
			infoBoxLabel.text = text;
			infoBoxLabel.gameObject.SetActive(true);
			infoBoxContainer.SetActive(true);
			ActivityIndicator.IsActiveIndicator = false;
			Invoke("closeConnectPanel", 3f);
		}
	}

	private void OnDisconnectedFromPhoton()
	{
		if (_isFaledConnectToRoom)
		{
			ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		else if (_isBackFromPassword)
		{
			closeConnectPanel();
		}
		else
		{
			ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		_isFaledConnectToRoom = false;
		_isBackFromPassword = false;
	}

	private void OnFailedToConnectToPhoton(int parameters)
	{
		ShowLabel(LocalizationStore.Get("Key_1411"));
		UnityEngine.Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + (DisconnectCause)parameters);
	}

	public void OnConnectedToMaster()
	{
		ConnectToRoom();
	}

	public void OnJoinedLobby()
	{
		ConnectToRoom();
	}

	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		UnityEngine.Debug.Log("OnJoinedLobby " + roomName);
		PhotonNetwork.JoinRoom(roomName);
		PlayerPrefs.SetString("RoomName", roomName);
	}

	private void OnPhotonJoinRoomFailed(bool onCreate)
	{
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		_isFaledConnectToRoom = true;
		GameConnect.Disconnect();
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_0137"));
		InfoWindowController.HideProcessing(3f);
		ResetGameMode();
	}

	private void ResetGameMode()
	{
		GameConnect.SetGameMode(GameConnect.GameMode.Deathmatch);
		WeaponManager.sharedManager.Reset();
	}

	private void SetEnabledPasswordPanel(bool enabled)
	{
		passwordPanel.SetActive(enabled);
		if (_loadingNGUIController != null)
		{
			backgroundConnectTexture.mainTexture = (enabled ? _loadingNGUIController.loadingNGUITexture.mainTexture : null);
			_loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	private void OnJoinedRoom()
	{
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		if (PhotonNetwork.room != null)
		{
			_passwordRoom = PhotonNetwork.room.customProperties[GameConnect.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (_passwordRoom.Equals(""))
			{
				PhotonNetwork.isMessageQueueRunning = false;
				StartCoroutine(MoveToGameScene());
				return;
			}
			UnityEngine.Debug.Log("Show Password Panel " + _passwordRoom);
			ActivityIndicator.IsActiveIndicator = false;
			inputPasswordLabel.value = string.Empty;
			SetEnabledPasswordPanel(true);
		}
		else
		{
			GameConnect.Disconnect();
			ShowLabel(LocalizationStore.Get("Key_1410"));
		}
	}

	private IEnumerator MoveToGameScene()
	{
		if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = true;
		}
		else
		{
			Defs.isGameFromFriends = true;
			Defs.isGameFromClans = false;
		}
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
		string mapName = infoScene.NameScene;
		WeaponManager.sharedManager.Reset((int)infoScene.AvaliableWeapon);
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		UnityEngine.Debug.Log("map = " + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
		UnityEngine.Debug.Log(mapName);
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + mapName) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = mapName;
		LoadConnectScene.noteToShow = null;
		yield return Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
	}
}
