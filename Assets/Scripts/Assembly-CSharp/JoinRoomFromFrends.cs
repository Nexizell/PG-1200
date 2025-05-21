using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class JoinRoomFromFrends : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetFonLoadingWaitForReset_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public JoinRoomFromFrends _003C_003E4__this;

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
		public _003CSetFonLoadingWaitForReset_003Ed__32(int _003C_003E1__state)
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
	internal sealed class _003CMoveToGameScene_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private SceneInfo _003CscInfo_003E5__1;

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
		public _003CMoveToGameScene_003Ed__49(int _003C_003E1__state)
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
				_003CscInfo_003E5__1 = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
				WeaponManager.sharedManager.Reset((int)((_003CscInfo_003E5__1 != null) ? _003CscInfo_003E5__1.AvaliableWeapon : ModeWeapon.all));
				UnityEngine.Debug.Log("MoveToGameScene");
				goto IL_00c9;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00c9;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00c9:
				if (PhotonNetwork.room == null)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				UnityEngine.Debug.Log("map=" + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
				UnityEngine.Debug.Log(_003CscInfo_003E5__1.NameScene);
				LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _003CscInfo_003E5__1.NameScene) as Texture2D;
				LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
				LoadConnectScene.sceneToLoad = _003CscInfo_003E5__1.NameScene;
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

	public int game_mode;

	public string room_name;

	public static JoinRoomFromFrends sharedJoinRoomFromFrends;

	public GameObject friendsPanel;

	public GameObject connectPanel;

	public static GameObject friendProfilePanel;

	public UILabel label;

	public GameObject plashkaLabel;

	private bool isFaledConnectToRoom;

	private bool oldActivFriendPanel;

	private bool oldActivProfileProfile;

	public UITexture fonConnectTexture;

	private string passwordRoom;

	public GameObject WrongPasswordLabel;

	private float timerShowWrongPassword;

	public GameObject PasswordPanel;

	private bool isBackFromPassword;

	public UIInput inputPassworLabel;

	public GameObject objectForOffWhenUlockDialog;

	private IDisposable _backSubscription;

	private LoadingNGUIController _loadingNGUIController;

	private void Start()
	{
		sharedJoinRoomFromFrends = this;
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
		sharedJoinRoomFromFrends = null;
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
		isBackFromPassword = true;
		SetEnabledPasswordPanel(false);
		GameConnect.Disconnect();
		ResetGameMode();
		UnityEngine.Debug.Log("BackFromPasswordButton");
	}

	public void EnterPassword(string pass)
	{
		if (pass == passwordRoom)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			StartCoroutine(MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
		else
		{
			timerShowWrongPassword = 3f;
			WrongPasswordLabel.SetActive(true);
		}
	}

	private void ShowLoadingGUI(string _mapName)
	{
		_loadingNGUIController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		_loadingNGUIController.SceneToLoad = _mapName;
		_loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _mapName);
		_loadingNGUIController.transform.parent = fonConnectTexture.transform.parent;
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

	public void ConnectToRoom(int _game_mode, string _room_name, string _map)
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(OnEsc, "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(_map));
		if (infoScene.isPremium && Storager.getInt(infoScene.NameScene + "Key") != 1 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
		{
			if (objectForOffWhenUlockDialog != null)
			{
				objectForOffWhenUlockDialog.SetActive(false);
			}
			Action successfulUnlockCallback = delegate
			{
			};
			ShowUnlockMapDialog(successfulUnlockCallback, infoScene.NameScene);
			return;
		}
		int tier = _game_mode % 10000 / 100;
		game_mode = _game_mode % 100;
		room_name = _room_name;
		Defs.isMulti = true;
		Defs.isInet = true;
		GameConnect.GameMode gameMode = (GameConnect.GameMode)game_mode;
		if (gameMode != GameConnect.GameMode.InClanWindow && gameMode != GameConnect.GameMode.InFriendWindow)
		{
			GameConnect.SetGameMode(gameMode);
			ActivityIndicator.IsActiveIndicator = true;
			oldActivFriendPanel = friendsPanel.activeSelf;
			if (friendProfilePanel != null)
			{
				oldActivProfileProfile = friendProfilePanel.activeSelf;
			}
			connectPanel.SetActive(true);
			friendsPanel.SetActive(false);
			if (friendProfilePanel != null)
			{
				friendProfilePanel.SetActive(false);
			}
			label.gameObject.SetActive(false);
			plashkaLabel.SetActive(false);
			UnityEngine.Debug.Log("fonConnectTexture.mainTexture=" + _map + " " + infoScene.NameScene);
			WeaponManager.sharedManager.Reset(GameConnect.isDaterRegim ? 3 : 0);
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
		if (timerShowWrongPassword > 0f && WrongPasswordLabel.activeSelf)
		{
			timerShowWrongPassword -= Time.deltaTime;
		}
		if (timerShowWrongPassword <= 0f && WrongPasswordLabel.activeSelf)
		{
			WrongPasswordLabel.SetActive(false);
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
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
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
		if (objectForOffWhenUlockDialog != null)
		{
			objectForOffWhenUlockDialog.SetActive(true);
		}
		closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int priceAmount = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy((FriendsWindowGUI.Instance != null) ? FriendsWindowGUI.Instance.gameObject : unlockPremiumMapView.gameObject, new ItemPrice(unlockPremiumMapView.Price, "Coins"), delegate
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
		fonConnectTexture.mainTexture = null;
		RemoveLoadingGUI();
		connectPanel.SetActive(false);
		label.gameObject.SetActive(false);
		plashkaLabel.SetActive(false);
		friendsPanel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void ShowLabel(string text)
	{
		if (connectPanel.activeSelf)
		{
			label.text = text;
			label.gameObject.SetActive(true);
			plashkaLabel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = false;
			Invoke("closeConnectPanel", 3f);
		}
	}

	private void OnDisconnectedFromPhoton()
	{
		if (isFaledConnectToRoom)
		{
			ShowLabel("Game is unavailable...");
		}
		else if (isBackFromPassword)
		{
			closeConnectPanel();
		}
		else
		{
			ShowLabel("Can't connect ...");
		}
		isFaledConnectToRoom = false;
		isBackFromPassword = false;
		UnityEngine.Debug.Log("OnDisconnectedFromPhoton");
	}

	private void OnFailedToConnectToPhoton(int parameters)
	{
		ShowLabel("Can't connect ...");
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
		UnityEngine.Debug.Log("OnJoinedLobby " + room_name);
		PhotonNetwork.JoinRoom(room_name);
		PlayerPrefs.SetString("RoomName", room_name);
	}

	private void OnPhotonJoinRoomFailed(bool onCreate)
	{
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		isFaledConnectToRoom = true;
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
		PasswordPanel.SetActive(enabled);
		if (_loadingNGUIController != null)
		{
			fonConnectTexture.gameObject.SetActive(enabled);
			fonConnectTexture.mainTexture = (enabled ? _loadingNGUIController.loadingNGUITexture.mainTexture : null);
			_loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	private void OnJoinedRoom()
	{
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		GlobalGameController.healthMyPlayer = 0f;
		if (PhotonNetwork.room != null)
		{
			passwordRoom = PhotonNetwork.room.customProperties[GameConnect.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (passwordRoom.Equals(""))
			{
				PhotonNetwork.isMessageQueueRunning = false;
				StartCoroutine(MoveToGameScene());
				return;
			}
			UnityEngine.Debug.Log("Show Password Panel " + passwordRoom);
			ActivityIndicator.IsActiveIndicator = false;
			inputPassworLabel.value = "";
			SetEnabledPasswordPanel(true);
		}
		else
		{
			GameConnect.Disconnect();
			ShowLabel("Game is unavailable...");
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
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
		WeaponManager.sharedManager.Reset((int)((scInfo != null) ? scInfo.AvaliableWeapon : ModeWeapon.all));
		UnityEngine.Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		UnityEngine.Debug.Log("map=" + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
		UnityEngine.Debug.Log(scInfo.NameScene);
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + scInfo.NameScene) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = scInfo.NameScene;
		LoadConnectScene.noteToShow = null;
		yield return Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
	}
}
