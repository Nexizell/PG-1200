using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class SpeedrunTrackController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CReturnToRace_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private bool _003CreachedHigh_003E5__1;

		private Player_move_c _003Cplayer_003E5__2;

		public float yPos;

		private Vector3 _003Cvelocity_003E5__3;

		public Vector3 point;

		private bool _003CmoveToPoint_003E5__4;

		public SpeedrunTrackController _003C_003E4__this;

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
		public _003CReturnToRace_003Ed__52(int _003C_003E1__state)
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
				_003Cplayer_003E5__2 = WeaponManager.sharedManager.myPlayerMoveC;
				_003Cplayer_003E5__2.mySkinName.firstPersonControl.character.enabled = false;
				_003Cplayer_003E5__2.mySkinName.firstPersonControl.ResetController();
				_003Cplayer_003E5__2.SetJetpackEnabledRPC(true);
				_003Cplayer_003E5__2.SetJetpackParticleEnabledRPC(true);
				_003CmoveToPoint_003E5__4 = true;
				_003CreachedHigh_003E5__1 = false;
				_003Cvelocity_003E5__3 = Vector3.up * -8f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CmoveToPoint_003E5__4)
			{
				if (!_003CreachedHigh_003E5__1 && _003Cplayer_003E5__2.myPlayerTransform.position.y < yPos)
				{
					_003Cvelocity_003E5__3 = Vector3.Lerp(_003Cvelocity_003E5__3, Vector3.up * 18f, Time.deltaTime * 4f);
				}
				else
				{
					_003CreachedHigh_003E5__1 = true;
					_003Cvelocity_003E5__3 = Vector3.Lerp(_003Cvelocity_003E5__3, (point - _003Cplayer_003E5__2.myPlayerTransform.position).normalized * 18f, Time.deltaTime * 4f);
				}
				_003Cplayer_003E5__2.myPlayerTransform.position += _003Cvelocity_003E5__3 * Time.deltaTime;
				if ((point - _003Cplayer_003E5__2.myPlayerTransform.position).sqrMagnitude < 50f)
				{
					_003CmoveToPoint_003E5__4 = false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003Cplayer_003E5__2.SetJetpackParticleEnabledRPC(false);
			_003Cplayer_003E5__2.SetJetpackEnabledRPC(false);
			_003Cplayer_003E5__2.mySkinName.firstPersonControl.ResetController();
			_003Cplayer_003E5__2.mySkinName.firstPersonControl.character.enabled = true;
			_003C_003E4__this.returningToRace = false;
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

	public static SpeedrunTrackController instance;

	private const int shieldBuyPrice = 1;

	public int visiblePartsCount = 20;

	public int visibleCenter = 10;

	public int partsBeforeHard = 30;

	public int partsBeforeFullHard = 30;

	public int chanceToSpawnHard = 70;

	public int safeZoneDistance = 200;

	public float distanceToGetCoin = 2000f;

	public SpeedrunTrackPart[] allParts;

	public SpeedrunTrackPart[] tutorialParts;

	public Transform startTrackPoint;

	public GameObject followPlayerGO;

	private List<List<List<SpeedrunTrackPart>>>[] partsPool;

	private List<SpeedrunTrackPart> currentTrack;

	private int partsPassed;

	private int lastGroup;

	private int currentTutorialPart;

	private SaltedFloatEps _distancePassed = new SaltedFloatEps(0f);

	private float distancePassedRewarded;

	private float distancePassedSpeed;

	public float startPlayerSpeed = 18f;

	public float distanceToAddSpeed = 100f;

	public float playerAddSpeed = 1f;

	public AudioClip shieldBreakSound;

	[HideInInspector]
	public float currentPlayerSpeed;

	[HideInInspector]
	public bool runStarted;

	private bool hasJetpack;

	private bool hasShield;

	private bool newControlScheme = true;

	public float goTimer = 6f;

	private float timerShowGo = 1f;

	[HideInInspector]
	public bool isShowGo;

	private bool returningToRace;

	private float distancePassed
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

	public bool canBuyShield
	{
		get
		{
			if (!hasShield && !runStarted)
			{
				if (!(Pauser.sharedPauser == null))
				{
					return !Pauser.sharedPauser.paused;
				}
				return true;
			}
			return false;
		}
	}

	public bool canUseJoystick
	{
		get
		{
			if (runStarted)
			{
				return !newControlScheme;
			}
			return true;
		}
	}

	public bool canRespawn
	{
		get
		{
			return distancePassed <= (float)safeZoneDistance;
		}
	}

	public bool showSafeZone
	{
		get
		{
			if (runStarted && !isShowGo)
			{
				return canRespawn;
			}
			return false;
		}
	}

	private void Awake()
	{
		instance = this;
		partsPool = new List<List<List<SpeedrunTrackPart>>>[4];
		for (int i = 0; i < partsPool.Length; i++)
		{
			partsPool[i] = new List<List<List<SpeedrunTrackPart>>>();
		}
		for (int j = 0; j < allParts.Length; j++)
		{
			List<List<List<SpeedrunTrackPart>>> list = partsPool[(int)allParts[j].type];
			int num = allParts[j].group + 1 - list.Count;
			List<List<SpeedrunTrackPart>> item;
			for (int k = 0; k < num; k++)
			{
				item = new List<List<SpeedrunTrackPart>>();
				list.Add(item);
			}
			item = list[allParts[j].group];
			item.Add(new List<SpeedrunTrackPart>());
			allParts[j].poolIndex = item.Count - 1;
			ReturnPart(allParts[j]);
		}
		currentTrack = new List<SpeedrunTrackPart>(visiblePartsCount);
	}

	private void AddCoinInPosition(Vector3 position)
	{
		GameObject gameObject = Initializer.CreateBonusAtPosition(position, VirtualCurrencyBonusType.CoinSpeedrun);
		if (!(gameObject == null))
		{
			CoinBonus component = gameObject.GetComponent<CoinBonus>();
			if (component == null)
			{
				UnityEngine.Debug.LogErrorFormat("Cannot find component '{0}'", component.GetType().Name);
			}
			else
			{
				component.SetPlayer();
			}
		}
	}

	private void StartRun()
	{
		isShowGo = true;
		runStarted = true;
		currentPlayerSpeed = startPlayerSpeed;
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.runnerSpeed = currentPlayerSpeed;
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.isRunner = true;
	}

	public void ResetRun()
	{
		timerShowGo = 1f;
		goTimer = 6f;
		runStarted = false;
		distancePassed = 0f;
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.isRunner = false;
	}

	private void Update()
	{
		GameObject myPlayer = WeaponManager.sharedManager.myPlayer;
		if (myPlayer == null || InGameGUI.sharedInGameGUI == null)
		{
			return;
		}
		goTimer = Mathf.Max(0f, goTimer - Time.deltaTime);
		if (Mathf.FloorToInt(goTimer) <= 0 && !runStarted)
		{
			StartRun();
		}
		if (isShowGo && timerShowGo >= 0f)
		{
			timerShowGo -= Time.deltaTime;
		}
		if (isShowGo && timerShowGo < 0f)
		{
			isShowGo = false;
		}
		if (InGameGUI.sharedInGameGUI.runnerShieldBuy.activeSelf != canBuyShield)
		{
			InGameGUI.sharedInGameGUI.runnerShieldBuy.SetActive(instance.canBuyShield);
			if (canBuyShield)
			{
				InGameGUI.sharedInGameGUI.runnerShieldBuyPrice.text = 1.ToString();
			}
		}
		followPlayerGO.transform.position = myPlayer.transform.position;
		if (currentTrack.Count > visibleCenter && currentTrack[visibleCenter].endPoint.transform.position.z < myPlayer.transform.position.z)
		{
			ReturnPart(currentTrack[0]);
			currentTrack.RemoveAt(0);
			partsPassed++;
		}
		int num = visiblePartsCount - currentTrack.Count;
		for (int i = 0; i < num; i++)
		{
			SpeedrunTrackPart nextPart = GetNextPart();
			nextPart.transform.position = ((currentTrack.Count > 0) ? currentTrack[currentTrack.Count - 1].endPoint.position : startTrackPoint.position);
			currentTrack.Add(nextPart);
			if (nextPart.type == SpeedrunTrackPart.PartType.Reward)
			{
				AddCoinInPosition(nextPart.coinSpawnPoint.transform.position);
			}
		}
		SpeedrunTrackPart speedrunTrackPart = null;
		SpeedrunTrackPart speedrunTrackPart2 = null;
		for (int j = 0; j < currentTrack.Count; j++)
		{
			if (currentTrack[j].endPoint.transform.position.z < myPlayer.transform.position.z)
			{
				speedrunTrackPart = currentTrack[j];
				speedrunTrackPart2 = ((j + 2 >= currentTrack.Count) ? speedrunTrackPart : currentTrack[j + 2]);
			}
		}
		float num2 = -1f;
		float num3 = 1f;
		if (speedrunTrackPart != null)
		{
			num2 = Mathf.Min(speedrunTrackPart.transform.position.y, speedrunTrackPart.endPoint.transform.position.y) - 4f;
			num3 = Mathf.Max(speedrunTrackPart.transform.position.y, speedrunTrackPart.endPoint.transform.position.y) + 10f;
		}
		else
		{
			num2 = startTrackPoint.transform.position.y - 4f;
			num3 = startTrackPoint.transform.position.y + 10f;
		}
		if (num2 > myPlayer.transform.position.y && !returningToRace)
		{
			if (!hasJetpack)
			{
				SuicidePlayer();
			}
			else
			{
				hasJetpack = false;
				returningToRace = true;
				StartCoroutine(ReturnToRace(num3, ((speedrunTrackPart2 != null) ? speedrunTrackPart2.transform.position : startTrackPoint.transform.position) + new Vector3(5f, 0f, 1f)));
			}
		}
		if (!WeaponManager.sharedManager.myPlayerMoveC.isFallDown)
		{
			distancePassed = Mathf.Max(distancePassed, myPlayer.transform.position.z - startTrackPoint.transform.position.z);
		}
		if (distancePassed - distancePassedSpeed >= distanceToAddSpeed)
		{
			distancePassedSpeed = distancePassed;
			currentPlayerSpeed += playerAddSpeed;
			WeaponManager.sharedManager.myPlayerMoveC.mySkinName.firstPersonControl.runnerSpeed = currentPlayerSpeed;
		}
		GlobalGameController.Score = Mathf.RoundToInt(distancePassed);
	}

	private void SuicidePlayer()
	{
		WeaponManager.sharedManager.myPlayerMoveC.SuicideFall();
	}

	public void ApplyDeathColliderShield()
	{
		if (hasShield)
		{
			hasShield = false;
			WeaponManager.sharedManager.myPlayerMoveC.reflectorParticles.SetActive(false);
			if (WeaponManager.sharedManager.myPlayerMoveC.reflectorParticlesDeactivation != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.reflectorParticlesDeactivation.SetActive(true);
			}
			WeaponManager.sharedManager.myPlayerMoveC.SetImmortality(1f);
			WeaponManager.sharedManager.myPlayerMoveC.GetComponent<AudioSource>().PlayOneShot(shieldBreakSound);
		}
	}

	private IEnumerator ReturnToRace(float yPos, Vector3 point)
	{
		Player_move_c player = WeaponManager.sharedManager.myPlayerMoveC;
		player.mySkinName.firstPersonControl.character.enabled = false;
		player.mySkinName.firstPersonControl.ResetController();
		player.SetJetpackEnabledRPC(true);
		player.SetJetpackParticleEnabledRPC(true);
		bool moveToPoint = true;
		bool reachedHigh = false;
		Vector3 velocity = Vector3.up * -8f;
		while (moveToPoint)
		{
			if (!reachedHigh && player.myPlayerTransform.position.y < yPos)
			{
				velocity = Vector3.Lerp(velocity, Vector3.up * 18f, Time.deltaTime * 4f);
			}
			else
			{
				reachedHigh = true;
				velocity = Vector3.Lerp(velocity, (point - player.myPlayerTransform.position).normalized * 18f, Time.deltaTime * 4f);
			}
			player.myPlayerTransform.position += velocity * Time.deltaTime;
			if ((point - player.myPlayerTransform.position).sqrMagnitude < 50f)
			{
				moveToPoint = false;
			}
			yield return null;
		}
		player.SetJetpackParticleEnabledRPC(false);
		player.SetJetpackEnabledRPC(false);
		player.mySkinName.firstPersonControl.ResetController();
		player.mySkinName.firstPersonControl.character.enabled = true;
		returningToRace = false;
	}

	private SpeedrunTrackPart GetTutorialPart()
	{
		SpeedrunTrackPart speedrunTrackPart = tutorialParts[currentTutorialPart];
		currentTutorialPart++;
		return GetPartByParams(speedrunTrackPart.type, speedrunTrackPart.group, speedrunTrackPart.poolIndex);
	}

	private SpeedrunTrackPart GetRewardPart()
	{
		return GetPartByParams(SpeedrunTrackPart.PartType.Reward, 0, 0);
	}

	private SpeedrunTrackPart GetNextPart()
	{
		if (tutorialParts != null && currentTutorialPart < tutorialParts.Length)
		{
			return GetTutorialPart();
		}
		if (distancePassed - distancePassedRewarded >= distanceToGetCoin)
		{
			distancePassedRewarded = distancePassed;
			return GetRewardPart();
		}
		SpeedrunTrackPart.PartType partType = SpeedrunTrackPart.PartType.Easy;
		if (partsPassed >= partsBeforeHard)
		{
			float num = Mathf.Clamp01((float)(partsPassed - partsBeforeHard) / (float)partsBeforeFullHard);
			partType = (((float)UnityEngine.Random.Range(0, 100) < (float)chanceToSpawnHard * num) ? SpeedrunTrackPart.PartType.Hard : SpeedrunTrackPart.PartType.Easy);
		}
		List<List<List<SpeedrunTrackPart>>> list = partsPool[(int)partType];
		if (list.Count == 0)
		{
			list = partsPool[0];
			partType = SpeedrunTrackPart.PartType.Easy;
		}
		int num2 = UnityEngine.Random.Range(0, list.Count);
		if (partType != SpeedrunTrackPart.PartType.Hard && num2 == lastGroup)
		{
			num2++;
			if (num2 >= list.Count)
			{
				num2 = 0;
			}
		}
		lastGroup = num2;
		List<List<SpeedrunTrackPart>> list2 = list[num2];
		int partIndex = UnityEngine.Random.Range(0, list2.Count);
		return GetPartByParams(partType, num2, partIndex);
	}

	private SpeedrunTrackPart GetPartByParams(SpeedrunTrackPart.PartType type, int group, int partIndex)
	{
		List<SpeedrunTrackPart> list = partsPool[(int)type][group][partIndex];
		SpeedrunTrackPart speedrunTrackPart;
		if (list.Count > 1)
		{
			speedrunTrackPart = list[1];
			list.RemoveAt(1);
		}
		else
		{
			speedrunTrackPart = UnityEngine.Object.Instantiate(list[0].gameObject).GetComponent<SpeedrunTrackPart>();
		}
		speedrunTrackPart.gameObject.SetActive(true);
		return speedrunTrackPart;
	}

	private void ReturnPart(SpeedrunTrackPart part)
	{
		part.gameObject.SetActive(false);
		partsPool[(int)part.type][part.group][part.poolIndex].Add(part);
	}

	private void ActivateShield()
	{
		hasShield = true;
		WeaponManager.sharedManager.myPlayerMoveC.reflectorParticles.SetActive(true);
	}

	public void TryBuyShield()
	{
		if (!hasShield)
		{
			ItemPrice price = new ItemPrice(1, "GemsCurrency");
			ShopNGUIController.TryToBuy(InGameGUI.sharedInGameGUI.gameObject, price, delegate
			{
				AnalyticsStuff.MiniGamesSales("shield_speedrun", true);
				AnalyticsStuff.SpeedrunGear("shield_speedrun");
				ActivateShield();
			}, JoystickController.leftJoystick.Reset);
		}
	}

	public void ChangeControlScheme(bool newScheme)
	{
		newControlScheme = newScheme;
	}
}
