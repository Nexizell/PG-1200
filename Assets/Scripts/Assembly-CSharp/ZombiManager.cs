using System;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public sealed class ZombiManager : MonoBehaviour
{
	public static ZombiManager sharedManager;

	public double timeGame;

	public float nextTimeSynch;

	public float nextAddZombi;

	public List<string> zombiePrefabs = new List<string>();

	private List<string[]> _enemies = new List<string[]>();

	private GameObject[] _enemyCreationZones;

	public bool startGame;

	public double maxTimeGame = 240.0;

	public PhotonView photonView;

	public bool isPizzaMap;

	private void Awake()
	{
		try
		{
			string[] array = null;
			isPizzaMap = SceneLoader.ActiveSceneName.Equals("Pizza") || SceneLoader.ActiveSceneName.Equals("Paradise_Night");
			array = ((!isPizzaMap) ? new string[11]
			{
				"1", "79", "2", "3", "57", "16", "56", "27", "73", "9",
				"39"
			} : new string[11]
			{
				"86", "90", "88", "91", "84", "87", "82", "81", "92", "80",
				"83"
			});
			string[] array2 = array;
			foreach (string text in array2)
			{
				string item = "Enemies/Enemy" + text + "_go";
				zombiePrefabs.Add(item);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void Start()
	{
		if (!Defs.isMulti || !GameConnect.isCOOP)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		sharedManager = this;
		try
		{
			nextAddZombi = 5f;
			_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			photonView = PhotonView.Get(this);
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	
	[PunRPC]
	private void synchTime(float _time)
	{
	}

	public void EndMatch()
	{
		if (PhotonNetwork.isMasterClient)
		{
			startGame = false;
			timeGame = 0.0;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win("");
		}
	}

	private void Update()
	{
		try
		{
			bool flag = Initializer.players.Count > 0;
			if (flag != startGame)
			{
				startGame = flag;
				timeGame = 0.0;
				nextTimeSynch = 0f;
				nextAddZombi = 0f;
			}
			if (!startGame)
			{
				return;
			}
			timeGame = maxTimeGame - (double)MiniGamesController.Instance.gameTimer;
			if (PhotonNetwork.isMasterClient && MiniGamesController.Instance.isGo && timeGame > (double)nextAddZombi && Initializer.enemiesObj.Count < 15)
			{
				float num = 4f;
				if (timeGame > maxTimeGame * 0.4000000059604645)
				{
					num = 3f;
				}
				if (timeGame > maxTimeGame * 0.800000011920929)
				{
					num = 2f;
				}
				nextAddZombi += num;
				int value = Initializer.players.Count - ((!(Application.loadedLevelName == "Arena")) ? 1 : 2);
				value = Mathf.Clamp(value, 1, 15);
				Debug.LogWarning(">>> ZOMBIE COUNT " + value);
				addZombies(value);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void addZombies(int count)
	{
		for (int i = 0; i < count; i++)
		{
			addZombi();
		}
	}

	private void addZombi()
	{
		GameObject gameObject = _enemyCreationZones[UnityEngine.Random.Range(0, _enemyCreationZones.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		int index = 0;
		double num = timeGame / maxTimeGame * 100.0;
		if (isPizzaMap)
		{
			if (num < 15.0)
			{
				index = UnityEngine.Random.Range(0, 4);
			}
			if (num >= 15.0 && num < 30.0)
			{
				index = UnityEngine.Random.Range(0, 5);
			}
			if (num >= 30.0 && num < 45.0)
			{
				index = UnityEngine.Random.Range(0, 6);
			}
			if (num >= 45.0 && num < 60.0)
			{
				index = UnityEngine.Random.Range(1, 7);
			}
			if (num >= 60.0 && num < 75.0)
			{
				index = UnityEngine.Random.Range(1, 9);
			}
			if (num >= 75.0)
			{
				index = UnityEngine.Random.Range(3, 11);
			}
		}
		else
		{
			if (num < 15.0)
			{
				index = UnityEngine.Random.Range(0, 3);
			}
			if (num >= 15.0 && num < 30.0)
			{
				index = UnityEngine.Random.Range(0, 5);
			}
			if (num >= 30.0 && num < 45.0)
			{
				index = UnityEngine.Random.Range(0, 6);
			}
			if (num >= 45.0 && num < 60.0)
			{
				index = UnityEngine.Random.Range(1, 8);
			}
			if (num >= 60.0 && num < 75.0)
			{
				index = UnityEngine.Random.Range(3, 10);
			}
			if (num >= 75.0)
			{
				index = UnityEngine.Random.Range(3, 11);
			}
		}
		PhotonNetwork.InstantiateSceneObject(zombiePrefabs[index], position, Quaternion.identity, 0, null);
	}

	private void OnDestroy()
	{
		sharedManager = null;
	}
}
