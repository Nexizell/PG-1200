using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckpointController : MonoBehaviour
{
	public static CheckpointController instance;

	public PlayerCheckpoint startCheckpoint;

	private PlayerCheckpoint currentCheckpoint;

	public PlayerCheckpoint savedCheckpoint;

	private List<PlayerCheckpoint> savedCheckpointsList = new List<PlayerCheckpoint>(10);

	private int scoreSync;

	private float scoreNextSyncTime;

	private SaltedFloatEps _distancePassed = new SaltedFloatEps(0f);

	public float reachedDistance;

	private int _saveCheckpointPrice = 1;

	[HideInInspector]
	public bool canSaveCheckpoint;

	[HideInInspector]
	public bool runFinishedOnce;

	private float lapTime;

	[HideInInspector]
	public int bestLapTimeMili;

	[HideInInspector]
	public int lapRecordTimeMili;

	private bool hitRecord;

	private bool inRun;

	private bool _finishedLap;

	private bool usedFreeSave;

	public float distancePassed
	{
		get
		{
			return _distancePassed.value;
		}
		set
		{
			_distancePassed.value = value;
		}
	}

	public int saveCheckpointPrice
	{
		get
		{
			return _saveCheckpointPrice;
		}
	}

	public int lapTimeMili
	{
		get
		{
			return Mathf.FloorToInt(lapTime * 100f);
		}
	}

	public bool finishedLap
	{
		get
		{
			return _finishedLap;
		}
	}

	private void Awake()
	{
		instance = this;
		PlayerCheckpoint playerCheckpoint = startCheckpoint;
		float num = 0f;
		while (playerCheckpoint != null)
		{
			playerCheckpoint.distance = num;
			if (playerCheckpoint.next != null)
			{
				num += (playerCheckpoint.next.transform.position - playerCheckpoint.transform.position).magnitude;
				playerCheckpoint = playerCheckpoint.next;
			}
			else
			{
				playerCheckpoint = null;
			}
		}
		lapRecordTimeMili = MiniGamesPlayerScoreManager.Instance.GetScore(GameConnect.gameMode);
	}

	public void ResetCheckpoint()
	{
		for (int i = 0; i < savedCheckpointsList.Count; i++)
		{
			savedCheckpointsList[i].SetCheckpointSaved(false, false);
		}
		savedCheckpointsList.Clear();
		_saveCheckpointPrice = 1;
		savedCheckpoint = null;
		currentCheckpoint = startCheckpoint;
		reachedDistance = currentCheckpoint.distance;
	}

	public int GetFreeCheckpointsCount()
	{
		int num = Storager.getInt("FreeCheckpointsKey");
		if (!usedFreeSave)
		{
			num++;
		}
		return num;
	}

	public void TrySaveOnCheckpoint()
	{
		if (!canSaveCheckpoint || currentCheckpoint.savedInCheckpoint)
		{
			return;
		}
		if (!usedFreeSave)
		{
			usedFreeSave = true;
			SaveOnCheckpoint(currentCheckpoint);
			AnalyticsStuff.DeathEscapeCheckpointUsed("Free");
			return;
		}
		int @int = Storager.getInt("FreeCheckpointsKey");
		if (@int > 0)
		{
			Storager.setInt("FreeCheckpointsKey", @int - 1);
			SaveOnCheckpoint(currentCheckpoint);
			AnalyticsStuff.DeathEscapeCheckpointUsed("Ads");
			return;
		}
		ItemPrice price = new ItemPrice(saveCheckpointPrice, "GemsCurrency");
		ShopNGUIController.TryToBuy(InGameGUI.sharedInGameGUI.gameObject, price, delegate
		{
			AnalyticsStuff.MiniGamesSales("checkpoint", true);
			SaveOnCheckpoint(currentCheckpoint);
			AnalyticsStuff.DeathEscapeCheckpointUsed("Bought");
		}, JoystickController.leftJoystick.Reset);
	}

	private void SaveOnCheckpoint(PlayerCheckpoint point)
	{
		PlayerCheckpoint playerCheckpoint = point;
		while (playerCheckpoint != null)
		{
			playerCheckpoint.SetCheckpointSaved(true, point.Equals(playerCheckpoint));
			savedCheckpointsList.Add(playerCheckpoint);
			playerCheckpoint = ((!(playerCheckpoint.back != null)) ? null : playerCheckpoint.back);
		}
		savedCheckpoint = point;
		canSaveCheckpoint = false;
	}

	public void RegisterInCheckpoint(PlayerCheckpoint point, bool playerIn)
	{
		if (playerIn)
		{
			SetCheckpoint(point);
			canSaveCheckpoint = point.isSavingCheckpoint && !point.savedInCheckpoint;
			if (point.next == null)
			{
				FinishRun();
			}
		}
		else
		{
			canSaveCheckpoint = false;
		}
	}

	private void SetCheckpoint(PlayerCheckpoint point)
	{
		currentCheckpoint = point;
		if (point.next == null)
		{
			UpdateDistanceScore(point.distance);
		}
	}

	public void StartRun()
	{
		if (!inRun)
		{
			inRun = true;
			_finishedLap = false;
			lapTime = 0f;
			ResetCheckpoint();
		}
	}

	public void StartNewLap()
	{
		runFinishedOnce = true;
		StartRun();
		WeaponManager.sharedManager.myPlayerMoveC.SetPlayerInSpawnPoint();
	}

	public void FinishRun()
	{
		if (inRun)
		{
			_finishedLap = true;
			inRun = false;
			bestLapTimeMili = ((bestLapTimeMili == 0) ? lapTimeMili : Mathf.Min(bestLapTimeMili, lapTimeMili));
			WeaponManager.sharedManager.myNetworkStartTable.CountKills = bestLapTimeMili;
			WeaponManager.sharedManager.myNetworkStartTable.SynhCountKills();
			GlobalGameController.CountKills = WeaponManager.sharedManager.myNetworkStartTable.CountKills;
			int score = MiniGamesPlayerScoreManager.Instance.GetScore(GameConnect.gameMode);
			if (score == 0 || score > bestLapTimeMili)
			{
				MiniGamesPlayerScoreManager.Instance.SetScore(GameConnect.gameMode, bestLapTimeMili);
				FriendsController.sharedController.SendScoreInMiniGames((int)GameConnect.gameMode, bestLapTimeMili);
				lapRecordTimeMili = bestLapTimeMili;
				hitRecord = true;
			}
			else
			{
				hitRecord = false;
				lapRecordTimeMili = score;
			}
			ShowLapFinishedInterface();
			AnalyticsStuff.DeathEscapeLapTime(lapTime);
			if (runFinishedOnce)
			{
				AnalyticsStuff.DeathEscapeRestart("Completed");
			}
		}
	}

	public void EndRun()
	{
		_finishedLap = false;
	}

	public void ShowLapFinishedInterface()
	{
		if (InGameGUI.sharedInGameGUI != null || !InGameGUI.sharedInGameGUI.panelFinishLap.activeSelf)
		{
			InGameGUI.sharedInGameGUI.ShowLapFinishedInterface(lapTimeMili, bestLapTimeMili, lapRecordTimeMili, hitRecord);
		}
	}

	private void UpdateDistanceScore(float distance)
	{
		distancePassed = Mathf.Max(distance, distancePassed);
		WeaponManager.sharedManager.myNetworkStartTable.score = Mathf.Max(WeaponManager.sharedManager.myNetworkStartTable.score, (int)distancePassed);
	}

	private void Update()
	{
		if (!inRun)
		{
			return;
		}
		GameObject myPlayer = WeaponManager.sharedManager.myPlayer;
		if (!(myPlayer == null) && !(currentCheckpoint == null) && !(currentCheckpoint.next == null))
		{
			lapTime += Time.deltaTime;
			if (scoreSync < WeaponManager.sharedManager.myNetworkStartTable.score && scoreNextSyncTime < Time.time)
			{
				scoreNextSyncTime = Time.time + 1f;
				scoreSync = WeaponManager.sharedManager.myNetworkStartTable.score;
				WeaponManager.sharedManager.myNetworkStartTable.SynhScore();
			}
			float magnitude = (currentCheckpoint.next.transform.position - myPlayer.transform.position).magnitude;
			reachedDistance = currentCheckpoint.next.distance - magnitude;
			UpdateDistanceScore(reachedDistance);
		}
	}
}
