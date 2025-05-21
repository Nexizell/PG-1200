using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public class BonusController : MonoBehaviour
{
	public enum TypeBonus
	{
		Ammo = 0,
		Health = 1,
		Armor = 2,
		Chest = 3,
		Grenade = 4,
		Mech = 5,
		JetPack = 6,
		Invisible = 7,
		Turret = 8,
		Gem = 9,
		Weapon = 10
	}

	public static BonusController sharedController;

	public GameObject bonusPrefab;

	public BonusItem[] bonusStack;

	private float creationInterval = 7f;

	public float timerToAddBonus;

	private bool isMulti;

	private bool isInet;

	private bool isStopCreateBonus;

	public bool isBeginCreateBonuses;

	private WeaponManager _weaponManager;

	private GameObject[] bonusCreationZones;

	private ZombieCreator zombieCreator;

	private PhotonView photonView;

	public int maxCountBonus = 5;

	private int activeBonusesCount;

	private int sumProbabilitys;

	private Dictionary<int, int> probabilityBonusDict = new Dictionary<int, int>();

	private Dictionary<int, Dictionary<string, int>> probabilityBonus = new Dictionary<int, Dictionary<string, int>>();

	private void InitStack()
	{
		bonusStack = new BonusItem[maxCountBonus + 6];
		for (int i = 0; i < bonusStack.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(bonusPrefab, Vector3.down * 100f, Quaternion.identity);
			gameObject.transform.parent = base.transform;
			bonusStack[i] = gameObject.GetComponent<BonusItem>();
		}
	}

	private void Awake()
	{
		if (sharedController == null)
		{
			sharedController = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (GameConnect.isSurvival)
		{
			creationInterval = 9f;
		}
		else if (GameConnect.isDuel)
		{
			creationInterval = 15f;
		}
		timerToAddBonus = creationInterval;
		isMulti = Defs.isMulti;
		isInet = Defs.isInet;
		maxCountBonus = (GameConnect.isDuel ? 2 : (GameConnect.isSurvival ? 3 : 5));
	}

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if ((bool)photonView)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		if (maxCountBonus > bonusCreationZones.Length)
		{
			maxCountBonus = bonusCreationZones.Length;
		}
		zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		_weaponManager = WeaponManager.sharedManager;
		SetProbability();
		InitStack();
	}

	private void SetProbability()
	{
		probabilityBonusDict.Clear();
		probabilityBonus.Clear();
		sumProbabilitys = 0;
		if (Defs.isMulti)
		{
			if (GameConnect.isHunger)
			{
				probabilityBonusDict.Add(3, 100);
			}
			else if (SceneLoader.ActiveSceneName.Equals("Knife"))
			{
				probabilityBonusDict.Add(1, 75);
				probabilityBonusDict.Add(2, 25);
			}
			else if (GameConnect.isDaterRegim)
			{
				probabilityBonusDict.Add(0, 100);
			}
			else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				probabilityBonusDict.Add(0, 100);
			}
			else if (GameConnect.isCOOP)
			{
				probabilityBonusDict.Add(0, 50);
				probabilityBonusDict.Add(1, 15);
				probabilityBonusDict.Add(2, 15);
				probabilityBonusDict.Add(10, 20);
			}
			else if (GameConnect.isDuel)
			{
				probabilityBonusDict.Add(0, 50);
				probabilityBonusDict.Add(1, 20);
				probabilityBonusDict.Add(2, 20);
			}
			else if (SceneLoader.ActiveSceneName.Equals("WalkingFortress"))
			{
				probabilityBonusDict.Add(0, 50);
				probabilityBonusDict.Add(1, 10);
				probabilityBonusDict.Add(2, 5);
			}
			else
			{
				probabilityBonusDict.Add(0, 50);
				probabilityBonusDict.Add(1, 10);
				probabilityBonusDict.Add(2, 10);
			}
		}
		else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			probabilityBonusDict.Add(0, 100);
		}
		else
		{
			probabilityBonusDict.Add(0, 55);
			probabilityBonusDict.Add(1, 14);
			probabilityBonusDict.Add(2, 12);
		}
		foreach (KeyValuePair<int, int> item in probabilityBonusDict)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("min", sumProbabilitys);
			sumProbabilitys += item.Value;
			dictionary.Add("max", sumProbabilitys);
			probabilityBonus.Add(item.Key, dictionary);
		}
	}

	public void AddWeaponAfterKillPlayer(string _weaponName, Vector3 _pos)
	{
		photonView.RPC("AddWeaponAfterKillPlayerRPC", PhotonTargets.MasterClient, _weaponName, _pos);
	}

	[RPC]
	[PunRPC]
	private void AddWeaponAfterKillPlayerRPC(string _weaponName, Vector3 _pos)
	{
		PhotonNetwork.InstantiateSceneObject("Weapon_Bonuses/" + _weaponName + "_Bonus", new Vector3(_pos.x, _pos.y - 0.5f, _pos.z), Quaternion.identity, 0, null);
	}

	public void AddBonusAfterKillPlayer(Vector3 _pos)
	{
		if (Defs.isInet)
		{
			photonView.RPC("AddBonusAfterKillPlayerRPC", PhotonTargets.MasterClient, _pos);
		}
	}

	[PunRPC]
	[RPC]
	private void AddBonusAfterKillPlayerRPC(Vector3 _pos)
	{
		AddBonusAfterKillPlayerRPC(IndexBonusOnKill(), _pos);
	}

	[RPC]
	[PunRPC]
	private void AddBonusAfterKillPlayerRPC(int _type, Vector3 _pos)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet && PhotonNetwork.isMasterClient && !GameConnect.isHunger)
			{
				AddBonus(_pos, _type);
			}
		}
		else
		{
			AddBonus(_pos, _type);
		}
	}

	private void AddBonus(Vector3 pos, int _type)
	{
		if (_type == 5 || _type == 8 || _type == 7 || _type == 6 || _type == 4)
		{
			return;
		}
		if (!isMulti && (GameConnect.isCampaign || GameConnect.isSurvival))
		{
			int num = GlobalGameController.EnemiesToKill - zombieCreator.NumOfDeadZombies;
			if ((GameConnect.isCampaign && num <= 0 && !zombieCreator.bossShowm) || (GameConnect.isSurvival && zombieCreator.stopGeneratingBonuses))
			{
				if (GameConnect.isCampaign)
				{
					isStopCreateBonus = true;
				}
				return;
			}
		}
		if (_type == 9)
		{
			if (!CanSpawnGemBonus())
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable[GameConnect.specialBonusProperty] = PhotonNetwork.time + 480.0;
			PhotonNetwork.room.SetCustomProperties(hashtable);
		}
		int num2 = -1;
		if (pos.Equals(Vector3.zero))
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Chest");
			if (activeBonusesCount + array.Length >= maxCountBonus)
			{
				return;
			}
			num2 = UnityEngine.Random.Range(0, bonusCreationZones.Length);
			bool[] array2 = new bool[bonusCreationZones.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = false;
			}
			for (int j = 0; j < bonusStack.Length; j++)
			{
				if (bonusStack[j].isActive && bonusStack[j].mySpawnNumber != -1)
				{
					array2[bonusStack[j].mySpawnNumber] = true;
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].GetComponent<SettingBonus>().numberSpawnZone != -1)
				{
					array2[array[k].GetComponent<SettingBonus>().numberSpawnZone] = true;
				}
			}
			while (array2[num2])
			{
				num2++;
				if (num2 == array2.Length)
				{
					num2 = 0;
				}
			}
			GameObject gameObject = bonusCreationZones[num2];
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
			Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
			pos = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		}
		if (_type != 3)
		{
			int num3 = 0;
			if (num3 >= bonusStack.Length || bonusStack[num3].isActive)
			{
				return;
			}
			if (_type != 10)
			{
				_type = ((_type != -1) ? _type : IndexBonus());
				MakeBonusRPC(num3, _type, pos, (num2 == -1) ? ((float)GetTimeForBonus()) : (-1f), num2);
				if (isMulti && isInet)
				{
					photonView.RPC("MakeBonusRPC", PhotonTargets.Others, num3, _type, pos, (num2 == -1) ? ((float)GetTimeForBonus()) : (-1f), num2);
				}
			}
			else
			{
				int num4 = UnityEngine.Random.Range(0, ChestController.weaponForHungerGames.Length);
				int num5 = ChestController.weaponForHungerGames[num4];
				PhotonNetwork.InstantiateSceneObject("Weapon_Bonuses/Weapon" + num5 + "_Bonus", pos, Quaternion.identity, 0, null).GetComponent<SettingBonus>().SetNumberSpawnZone(num2);
			}
		}
		else if (!isMulti || !isInet)
		{
			UnityEngine.Object.Instantiate(Resources.Load("Bonuses/Bonus_" + _type) as GameObject, pos, Quaternion.identity).GetComponent<SettingBonus>().numberSpawnZone = num2;
		}
		else
		{
			PhotonNetwork.InstantiateSceneObject("Bonuses/Bonus_" + _type, pos, Quaternion.identity, 0, null).GetComponent<SettingBonus>().SetNumberSpawnZone(num2);
		}
	}

	public void AddBonusForHunger(Vector3 pos, int _type, int spawnZoneIndex)
	{
		if (!GameConnect.isHunger)
		{
			return;
		}
		for (int i = 0; i < bonusStack.Length; i++)
		{
			if (!bonusStack[i].isActive)
			{
				MakeBonusRPC(i, _type, pos, -1f, spawnZoneIndex);
				if (isMulti && isInet)
				{
					photonView.RPC("MakeBonusRPC", PhotonTargets.Others, i, _type, pos, -1f, spawnZoneIndex);
				}
				break;
			}
		}
	}

	public void RemoveBonus(int index)
	{
		RemoveBonusRPC(index);
		if (isMulti && isInet)
		{
			photonView.RPC("RemoveBonusRPC", PhotonTargets.Others, index);
		}
	}

	public void GetAndRemoveBonus(int index)
	{
		if (isMulti && isInet && !NetworkStartTable.LocalOrPasswordRoom())
		{
			RemoveBonusWithRewardRPC(PhotonNetwork.player, index);
			photonView.RPC("RemoveBonusWithRewardRPC", PhotonTargets.Others, PhotonNetwork.player, index);
		}
	}

	public void ClearBonuses()
	{
		for (int i = 0; i < bonusStack.Length; i++)
		{
			if (bonusStack[i].isActive)
			{
				RemoveBonusRPC(i);
			}
		}
	}

	private void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (!PhotonNetwork.isMasterClient)
		{
			return;
		}
		for (int i = 0; i < bonusStack.Length; i++)
		{
			if (bonusStack[i].isActive)
			{
				photonView.RPC("MakeBonusRPC", player, i, (int)bonusStack[i].type, bonusStack[i].transform.position, (float)bonusStack[i].expireTime, bonusStack[i].mySpawnNumber);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void MakeBonusRPC(int index, int type, Vector3 position, float expireTime, int zoneNumber)
	{
		if (index < bonusStack.Length && !bonusStack[index].isActive)
		{
			bonusStack[index].ActivateBonus((TypeBonus)type, position, expireTime, zoneNumber, index);
			if (!bonusStack[index].isTimeBonus)
			{
				activeBonusesCount++;
			}
		}
	}

	private void PickupBonus(int index, PhotonPlayer player)
	{
		if (index < bonusStack.Length && bonusStack[index].isActive && !bonusStack[index].isPickedUp)
		{
			bonusStack[index].PickupBonus(player);
		}
	}

	[RPC]
	[PunRPC]
	private void RemoveBonusRPC(int index)
	{
		if (index < bonusStack.Length && bonusStack[index].isActive)
		{
			if (!bonusStack[index].isTimeBonus)
			{
				activeBonusesCount--;
			}
			bonusStack[index].DeactivateBonus();
		}
	}

	[RPC]
	[PunRPC]
	private void RemoveBonusWithRewardRPC(PhotonPlayer sender, int index)
	{
		if (isMulti && isInet && !NetworkStartTable.LocalOrPasswordRoom() && index < bonusStack.Length && bonusStack[index].isActive)
		{
			PickupBonus(index, sender);
		}
	}

	[PunRPC]
	[RPC]
	private void GetBonusRewardRPC(int index)
	{
		if (index >= bonusStack.Length || !bonusStack[index].isActive || !bonusStack[index].isPickedUp)
		{
			return;
		}
		if (bonusStack[index].playerPicked.Equals(PhotonNetwork.player))
		{
			TypeBonus type = bonusStack[index].type;
			if (type == TypeBonus.Gem)
			{
				BankController.AddGems(1);
			}
		}
		RemoveBonusRPC(index);
	}

	private double GetTimeForBonus()
	{
		double result = -1.0;
		if (Defs.isInet)
		{
			result = PhotonNetwork.time + 15.0;
		}
		return result;
	}

	private bool CanSpawnGemBonus()
	{
		if (GameConnect.isHunger || !Defs.isMulti || !Defs.isInet || NetworkStartTable.LocalOrPasswordRoom())
		{
			return false;
		}
		if (PhotonNetwork.room == null || PhotonNetwork.room.customProperties[GameConnect.specialBonusProperty] == null || Convert.ToDouble(PhotonNetwork.room.customProperties[GameConnect.specialBonusProperty]) > PhotonNetwork.time)
		{
			return false;
		}
		return true;
	}

	private int IndexBonus()
	{
		int num = UnityEngine.Random.Range(0, sumProbabilitys);
		foreach (KeyValuePair<int, Dictionary<string, int>> probabilityBonu in probabilityBonus)
		{
			if (num >= probabilityBonu.Value["min"] && num < probabilityBonu.Value["max"])
			{
				return probabilityBonu.Key;
			}
		}
		return 0;
	}

	private int IndexBonusOnKill()
	{
		if (CanSpawnGemBonus() && UnityEngine.Random.Range(0, 100) < 5)
		{
			return 9;
		}
		int num = UnityEngine.Random.Range(0, sumProbabilitys);
		foreach (KeyValuePair<int, Dictionary<string, int>> probabilityBonu in probabilityBonus)
		{
			if (num >= probabilityBonu.Value["min"] && num < probabilityBonu.Value["max"])
			{
				return probabilityBonu.Key;
			}
		}
		return 0;
	}

	private void Update()
	{
		bool flag = false;
		if (isMulti)
		{
			if (isInet)
			{
				flag = PhotonNetwork.isMasterClient;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < bonusStack.Length; i++)
			{
				if (bonusStack[i].isActive && bonusStack[i].isPickedUp)
				{
					photonView.RPC("GetBonusRewardRPC", PhotonTargets.All, i);
				}
			}
		}
		if (!isStopCreateBonus && flag)
		{
			timerToAddBonus -= Time.deltaTime;
		}
		if (timerToAddBonus < 0f)
		{
			timerToAddBonus = creationInterval;
			AddBonus(Vector3.zero, IndexBonus());
		}
	}

	private void OnDestroy()
	{
		if ((bool)photonView)
		{
			PhotonObjectCacher.RemoveObject(base.gameObject);
		}
	}
}
