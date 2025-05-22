using System.Collections.Generic;
using Photon;
using Rilisoft.WP8;
using UnityEngine;

public class WeaponBonus : Photon.MonoBehaviour
{
	public GameObject weaponPrefab;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	private bool oldIsMaster;

	public WeaponManager _weaponManager;

	private bool isHunger;

	public bool isKilled;

	private void Start()
	{
		string text = base.gameObject.name.Substring(0, base.gameObject.name.Length - 13);
		weaponPrefab = Resources.Load<GameObject>("Weapons/" + text);
		_weaponManager = WeaponManager.sharedManager;
		isHunger = GameConnect.isHunger;
		_player = WeaponManager.sharedManager.myPlayer;
		_playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		if (!GameConnect.isSurvival && !isHunger && !GameConnect.isCOOP)
		{
			GameObject obj = Object.Instantiate(Resources.Load("BonusFX"), Vector3.zero, Quaternion.identity) as GameObject;
			obj.transform.parent = base.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.layer = base.gameObject.layer;
			ZombieCreator.SetLayerRecursively(obj, base.gameObject.layer);
		}
	}

	private void Update()
	{
		if (!oldIsMaster && PhotonNetwork.isMasterClient && isKilled)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		oldIsMaster = PhotonNetwork.isMasterClient;
		float num = 120f;
		base.transform.Rotate(base.transform.InverseTransformDirection(Vector3.up), num * Time.deltaTime);
		if (runLoading)
		{
			return;
		}
		if (_player == null || _playerMoveC == null)
		{
			_player = WeaponManager.sharedManager.myPlayer;
			_playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (_playerMoveC == null || _playerMoveC.isGrenadePress || isKilled || _playerMoveC.isKilled || !(Vector3.SqrMagnitude(base.transform.position - _player.transform.position) < 2.25f))
		{
			return;
		}
		_playerMoveC.AddWeapon(weaponPrefab);
		isKilled = true;
		if (GameConnect.isSurvival || (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None) || Defs.isMulti)
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingController.isNextStep = TrainingState.GetTheGun;
			}
			if (!Defs.isMulti)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				base.photonView.RPC("DestroyRPC", PhotonTargets.AllBuffered);
			}
			if (GameConnect.isHunger)
			{
				_playerMoveC.myNetworkStartTable.AddToScore(2);
			}
			return;
		}
		string[] array = Storager.getString(Defs.WeaponsGotInCampaign).Split('#');
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		if (!list.Contains(LevelBox.weaponsFromBosses[Application.loadedLevelName]))
		{
			list.Add(LevelBox.weaponsFromBosses[Application.loadedLevelName]);
			Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#", list.ToArray()));
		}
		Object.Destroy(base.gameObject);
		runLoading = true;
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
		}
		Object.Instantiate(Resources.Load("PauseONGuiDrawer") as GameObject).transform.parent = base.transform;
	}

	[PunRPC]
	
	public void DestroyRPC()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.transform.position = new Vector3(0f, -10000f, 0f);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	private void OnDestroy()
	{
		if (!GameConnect.isSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != 0))
		{
			bool isHunger2 = isHunger;
		}
	}
}
