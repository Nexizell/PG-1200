using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using RilisoftBot;
using UnityEngine;

public sealed class ZombieCreator : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003C_DrawFirstMessage_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003C_DrawFirstMessage_003Ed__32(int _003C_003E1__state)
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
				if (!GameConnect.isSurvival)
				{
					return false;
				}
				goto IL_0042;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0042;
			case 2:
				{
					_003C_003E1__state = -1;
					InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
					break;
				}
				IL_0042:
				if (InGameGUI.sharedInGameGUI == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
				{
					InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
					InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = string.Format("{0} 1", new object[1] { LocalizationStore.Key_0349 });
					_003C_003E2__current = new WaitForSeconds(2f);
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
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
	internal sealed class _003C_DrawWaveMessage_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003Ci_003E5__1;

		public Action act;

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
		public _003C_DrawWaveMessage_003Ed__33(int _003C_003E1__state)
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
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(true);
				}
				_003C_003E2__current = new WaitForSeconds(5f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(false);
				}
				if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
				{
					InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
					_003Ci_003E5__1 = 5;
					goto IL_0117;
				}
				goto IL_0135;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__1--;
				goto IL_0117;
			case 3:
				{
					_003C_003E1__state = -1;
					if (InGameGUI.sharedInGameGUI != null)
					{
						InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(false);
					}
					return false;
				}
				IL_0135:
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(true);
				}
				act();
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 3;
				return true;
				IL_0117:
				if (_003Ci_003E5__1 > 0)
				{
					InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = _003Ci_003E5__1.ToString();
					_003C_003E2__current = new WaitForSeconds(1f);
					_003C_003E1__state = 2;
					return true;
				}
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
				goto IL_0135;
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

	[CompilerGenerated]
	internal sealed class _003C_PrerenderBoss_003Ed__62 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ZombieCreator _003C_003E4__this;

		private ObjectPrerenderer _003Cop_003E5__1;

		private GameObject _003Cprer_003E5__2;

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
		public _003C_PrerenderBoss_003Ed__62(int _003C_003E1__state)
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
				_003Cprer_003E5__2 = UnityEngine.Object.Instantiate(Resources.Load("ObjectPrerenderer") as GameObject, new Vector3(0f, 0f, -10000f), Quaternion.identity);
				_003Cop_003E5__1 = _003Cprer_003E5__2.GetComponentInChildren<ObjectPrerenderer>();
				if ((bool)_003Cop_003E5__1)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(_003C_003E4__this.boss.transform.GetChild(0).gameObject);
					gameObject.transform.parent = _003Cop_003E5__1.transform;
					gameObject.transform.localPosition = new Vector3(0f, 0f, 2.7f);
					gameObject.layer = _003Cop_003E5__1.gameObject.layer;
					SetLayerRecursively(gameObject, gameObject.layer);
					if (_003C_003E4__this.weaponBonus != null)
					{
						GameObject gameObject2 = BonusCreator._CreateBonusFromPrefab(_003C_003E4__this.weaponBonus, Vector3.zero);
						gameObject2.transform.parent = _003Cop_003E5__1.transform;
						gameObject2.transform.localPosition = new Vector3(1.5f, 0f, 3f);
						gameObject2.layer = _003Cop_003E5__1.gameObject.layer;
						SetLayerRecursively(gameObject2, gameObject2.layer);
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Cop_003E5__1.Render_();
				UnityEngine.Object.Destroy(_003Cprer_003E5__2);
				break;
			}
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
	internal sealed class _003CAddZombies_003Ed__75 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ZombieCreator _003C_003E4__this;

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
		public _003CAddZombies_003Ed__75(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int zombiesInWave;
			string[] array;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0022;
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._generatingZombiesIsStopped)
				{
					goto IL_01c2;
				}
				goto IL_01ea;
			case 2:
				{
					_003C_003E1__state = -1;
					if (GameConnect.isSurvival)
					{
						_003C_003E4__this._genWithThisTimeInterval++;
						if (_003C_003E4__this._genWithThisTimeInterval == 3 && _003C_003E4__this._indexInTimesArray < _003C_003E4__this._intervalArr.Length - 1)
						{
							_003C_003E4__this._indexInTimesArray++;
						}
						_003C_003E4__this.curInterval = _003C_003E4__this._intervalArr[_003C_003E4__this._indexInTimesArray];
					}
					if (_003C_003E4__this.NumOfDeadZombies + _003C_003E4__this.NumOfLiveZombies >= NumOfEnemisesToKill && !GameConnect.isSurvival)
					{
						return false;
					}
					goto IL_0022;
				}
				IL_0022:
				zombiesInWave = GlobalGameController.ZombiesInWave;
				zombiesInWave = Mathf.Min(zombiesInWave, GlobalGameController.SimultaneousEnemiesOnLevelConstraint - _003C_003E4__this.NumOfLiveZombies);
				zombiesInWave = Mathf.Min(zombiesInWave, NumOfEnemisesToKill - (_003C_003E4__this.NumOfDeadZombies + _003C_003E4__this.NumOfLiveZombies));
				array = null;
				if (!GameConnect.isSurvival)
				{
					array = ((CurrentCampaignGame.currentLevel != 0) ? _003C_003E4__this._enemies[CurrentCampaignGame.currentLevel - 1] : new string[1] { "1" });
				}
				else
				{
					int index = ((_003C_003E4__this.currentWave >= _enemiesInWaves.Count) ? (_enemiesInWaves.Count - 1) : _003C_003E4__this.currentWave);
					array = _enemiesInWaves[index].ToArray();
				}
				for (int i = 0; i < zombiesInWave; i++)
				{
					int index2 = UnityEngine.Random.Range(0, array.Length);
					GameObject spawnZone = (GameConnect.isSurvival ? _003C_003E4__this._enemyCreationZones[i % _003C_003E4__this._enemyCreationZones.Length] : _003C_003E4__this._enemyCreationZones[UnityEngine.Random.Range(0, _003C_003E4__this._enemyCreationZones.Length)]);
					Vector3 position = _003C_003E4__this._createPos(spawnZone);
					UnityEngine.Object.Instantiate(GameConnect.isSurvival ? _003C_003E4__this.waveZombiePrefabs[index2] : _003C_003E4__this.zombiePrefabs[index2], position, Quaternion.identity);
				}
				if (GameConnect.isSurvival && _003C_003E4__this.NumOfDeadZombies + _003C_003E4__this.NumOfLiveZombies >= NumOfEnemisesToKill)
				{
					_003C_003E4__this._generatingZombiesIsStopped = true;
					goto IL_01c2;
				}
				goto IL_01ea;
				IL_01c2:
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
				IL_01ea:
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.curInterval);
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

	private GameObject boss;

	public GameObject weaponBonus;

	public static ZombieCreator sharedCreator;

	private static int _ZombiesInWave;

	public int currentWave;

	private static List<List<string>> _enemiesInWaves;

	private static readonly HashSet<string> _allEnemiesSurvival;

	public List<GameObject> waveZombiePrefabs = new List<GameObject>();

	public static Dictionary<int, int> bossesSurvival;

	public static List<List<string>> WeaponsAddedInWaves;

	public static List<string> survivalAvailableWeapons;

	private bool _generatingZombiesIsStopped;

	private int totalNumOfKilledEnemies;

	private AudioClip bossMus;

	private static int? _enemyCountInSurvivalWave;

	public GUIStyle labelStyle;

	private int[] _intervalArr = new int[3] { 6, 4, 3 };

	private int _genWithThisTimeInterval;

	private int _indexInTimesArray;

	private string _msg = "";

	private GameObject[] _teleports;

	public bool bossShowm;

	public bool stopGeneratingBonuses;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private bool _isMultiplayer;

	private SaltedInt _numOfLiveZombies = 0;

	private SaltedInt _numOfDeadZombies = 0;

	private SaltedInt _numOfDeadZombsSinceLastFast = 0;

	public float curInterval = 10f;

	private GameObject[] _enemyCreationZones;

	private List<string[]> _enemies = new List<string[]>();

	public GameObject[] bossGuads { get; private set; }

	public static int EnemyCountInSurvivalWave
	{
		get
		{
			if (!_enemyCountInSurvivalWave.HasValue)
			{
				return DefaultEnemyCountInSurvivalWave;
			}
			return _enemyCountInSurvivalWave.Value;
		}
		set
		{
			_enemyCountInSurvivalWave = value;
		}
	}

	public static int DefaultEnemyCountInSurvivalWave
	{
		get
		{
			return _ZombiesInWave;
		}
	}

	public int NumOfLiveZombies
	{
		get
		{
			return _numOfLiveZombies.Value;
		}
		set
		{
			_numOfLiveZombies = value;
		}
	}

	public bool IsLasTMonsRemains
	{
		get
		{
			if (NumOfDeadZombies + 1 == NumOfEnemisesToKill)
			{
				return !bossShowm;
			}
			return false;
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return _numOfDeadZombies.Value;
		}
		set
		{
			if (bossShowm)
			{
				bossShowm = false;
				if (!GameConnect.isSurvival)
				{
					if (ZombieCreator.BossKilled != null)
					{
						ZombieCreator.BossKilled();
					}
					if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
					{
						GameObject[] teleports = _teleports;
						for (int i = 0; i < teleports.Length; i++)
						{
							teleports[i].SetActive(true);
						}
						GameObject gameObject = _teleports.Map((GameObject[] ts) => ts.FirstOrDefault());
						if (gameObject != null)
						{
							PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
							if (component != null)
							{
								component.RemovePointOfInterest();
								Transform transform = gameObject.transform;
								GameObject gameObject2 = new GameObject("Teleport Replacement");
								gameObject2.transform.position = new Vector3(transform.position.x, component.ParentCamera.transform.position.y, transform.position.z);
								component.SetPointOfInterest(gameObject2.transform, Vector3.zero);
							}
						}
						return;
					}
					GameObject gameObject3 = (from g in UnityEngine.Object.FindObjectsOfType<GotToNextLevel>()
						select g.gameObject).FirstOrDefault();
					if (gameObject3 != null)
					{
						PlayerArrowToPortalController component2 = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
						if (component2 != null)
						{
							component2.RemovePointOfInterest();
							component2.SetPointOfInterest(gameObject3.transform, 0f * Vector3.up);
						}
					}
				}
				else
				{
					totalNumOfKilledEnemies++;
					NumOfLiveZombies--;
					NextWave();
				}
				return;
			}
			int num = value - _numOfDeadZombies.Value;
			_numOfDeadZombies = value;
			totalNumOfKilledEnemies += num;
			NumOfLiveZombies -= num;
			if (!GameConnect.isSurvival && NumOfEnemisesToKill - _numOfDeadZombies.Value <= 5 && Initializer.enemiesObj.Count > 0)
			{
				PlayerArrowToPortalController component3 = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
				Transform transform2 = null;
				float num2 = float.MaxValue;
				for (int j = 0; j < Initializer.enemiesObj.Count; j++)
				{
					if (Initializer.enemiesObj[j].GetComponent<BaseBot>().health > 0f)
					{
						float sqrMagnitude = (WeaponManager.sharedManager.myPlayer.transform.position - Initializer.enemiesObj[j].transform.position).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							transform2 = Initializer.enemiesObj[j].transform;
							num2 = sqrMagnitude;
						}
					}
				}
				component3.RemovePointOfInterest();
				if (transform2 != null)
				{
					BoxCollider componentInChildren = transform2.gameObject.GetComponentInChildren<BoxCollider>();
					Vector3 localOffset = ((componentInChildren != null) ? componentInChildren.center : (1.618f * Vector3.up));
					component3.RemovePointOfInterest();
					component3.SetPointOfInterest(transform2, localOffset);
				}
			}
			if (!GameConnect.isSurvival)
			{
				_numOfDeadZombsSinceLastFast = _numOfDeadZombsSinceLastFast.Value + num;
				if (_numOfDeadZombsSinceLastFast.Value == 12)
				{
					if (curInterval > 5f)
					{
						curInterval -= 5f;
					}
					_numOfDeadZombsSinceLastFast = 0;
				}
				if (IsLasTMonsRemains && ZombieCreator.LastEnemy != null)
				{
					ZombieCreator.LastEnemy();
				}
			}
			if (GameConnect.isSurvival && NumOfDeadZombies == NumOfEnemisesToKill - 1)
			{
				stopGeneratingBonuses = true;
			}
			if (_numOfDeadZombies.Value < NumOfEnemisesToKill)
			{
				return;
			}
			if (GameConnect.isSurvival)
			{
				if (bossesSurvival.ContainsKey(currentWave))
				{
					CreateBoss();
				}
				else
				{
					NextWave();
				}
				return;
			}
			if (CurrentCampaignGame.currentLevel == 0)
			{
				GameObject[] teleports = _teleports;
				foreach (GameObject obj in teleports)
				{
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
					{
						TrainingController.isNextStep = TrainingState.KillZombie;
					}
					obj.SetActive(true);
				}
				return;
			}
			CreateBoss();
			if (!(bossMus != null))
			{
				return;
			}
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (gameObject4 != null && (bool)gameObject4.GetComponent<AudioSource>())
			{
				gameObject4.GetComponent<AudioSource>().Stop();
				gameObject4.GetComponent<AudioSource>().clip = bossMus;
				if (Defs.isSoundMusic)
				{
					gameObject4.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	public static int NumOfEnemisesToKill
	{
		get
		{
			if (!GameConnect.isSurvival)
			{
				return GlobalGameController.EnemiesToKill;
			}
			return EnemyCountInSurvivalWave;
		}
	}

	public static event Action LastEnemy;

	public static event Action BossKilled;

	private IEnumerator _DrawFirstMessage()
	{
		if (GameConnect.isSurvival)
		{
			while (InGameGUI.sharedInGameGUI == null)
			{
				yield return null;
			}
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
			{
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = string.Format("{0} 1", new object[1] { LocalizationStore.Key_0349 });
				yield return new WaitForSeconds(2f);
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
			}
		}
	}

	private IEnumerator _DrawWaveMessage(Action act)
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(true);
		}
		yield return new WaitForSeconds(5f);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(false);
		}
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
		{
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
			for (int i = 5; i > 0; i--)
			{
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = i.ToString();
				yield return new WaitForSeconds(1f);
			}
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(true);
		}
		act();
		yield return new WaitForSeconds(1f);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		sharedCreator = null;
		if (GameConnect.isSurvival)
		{
			PlayerPrefs.SetInt(Defs.KilledZombiesSett, totalNumOfKilledEnemies);
			int @int = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0);
			if (totalNumOfKilledEnemies > @int)
			{
				PlayerPrefs.SetInt(Defs.KilledZombiesMaxSett, totalNumOfKilledEnemies);
			}
			WavesSurvivedStat.SurvivedWaveCount = currentWave;
			int int2 = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0);
			if (currentWave > int2)
			{
				PlayerPrefs.SetInt(Defs.WavesSurvivedMaxS, currentWave);
			}
		}
	}

	private void _UpdateIntervalStructures()
	{
		_genWithThisTimeInterval = 0;
		_indexInTimesArray = 0;
		curInterval = _intervalArr[_indexInTimesArray];
	}

	static ZombieCreator()
	{
		sharedCreator = null;
		_ZombiesInWave = 45;
		_enemiesInWaves = new List<List<string>>();
		_allEnemiesSurvival = new HashSet<string>();
		bossesSurvival = new Dictionary<int, int>();
		WeaponsAddedInWaves = new List<List<string>>();
		survivalAvailableWeapons = new List<string>();
		List<string> item = new List<string>
		{
			WeaponManager.PistolWN,
			WeaponManager.ShotgunWN,
			WeaponManager.MP5WN
		};
		List<string> item2 = new List<string>
		{
			WeaponManager.AK47WN,
			WeaponManager.RevolverWN
		};
		List<string> item3 = new List<string>
		{
			WeaponManager.M16_2WN,
			WeaponManager.ObrezWN
		};
		List<string> item4 = new List<string> { WeaponManager.MachinegunWN };
		List<string> item5 = new List<string> { WeaponManager.AlienGunWN };
		WeaponsAddedInWaves.Add(item);
		WeaponsAddedInWaves.Add(item2);
		WeaponsAddedInWaves.Add(item3);
		WeaponsAddedInWaves.Add(item4);
		WeaponsAddedInWaves.Add(item5);
	}

	private void SetWaveNumberInGUI()
	{
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.SurvivalWaveNumber != null)
		{
			InGameGUI.sharedInGameGUI.SurvivalWaveNumber.text = string.Format("{0} {1}", new object[2]
			{
				LocalizationStore.Get("Key_0349"),
				currentWave + 1
			});
		}
	}

	public void NextWave()
	{
		currentWave++;
		QuestMediator.NotifySurviveWaveInArena(currentWave);
		StoreKitEventListener.State.Parameters.Clear();
		StoreKitEventListener.State.Parameters.Add("Waves", ((currentWave < 10) ? string.Concat(currentWave + 1) : (currentWave / 10 * 10 + "-" + ((currentWave / 10 + 1) * 10 - 1))) + " In game");
		StartCoroutine(_DrawWaveMessage(delegate
		{
			_UpdateIntervalStructures();
			_numOfDeadZombies = 0;
			_numOfDeadZombsSinceLastFast = 0;
			_SetZombiePrefabs();
			_UpdateAvailableWeapons();
			_generatingZombiesIsStopped = false;
			stopGeneratingBonuses = false;
			SetWaveNumberInGUI();
		}));
		Vector3 position = ((!SceneLoader.ActiveSceneName.Equals("Pizza")) ? new Vector3(0f, 1f, 0f) : new Vector3(-7.83f, 0.46f, -2.44f));
		GameObject gameObject = Initializer.CreateBonusAtPosition(position, VirtualCurrencyBonusType.Coin);
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

	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		if (!(go == null))
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layerNumber;
			}
		}
	}

	private IEnumerator _PrerenderBoss()
	{
		GameObject prer = UnityEngine.Object.Instantiate(Resources.Load("ObjectPrerenderer") as GameObject, new Vector3(0f, 0f, -10000f), Quaternion.identity);
		ObjectPrerenderer op = prer.GetComponentInChildren<ObjectPrerenderer>();
		if ((bool)op)
		{
			GameObject obj = UnityEngine.Object.Instantiate(boss.transform.GetChild(0).gameObject);
			obj.transform.parent = op.transform;
			obj.transform.localPosition = new Vector3(0f, 0f, 2.7f);
			obj.layer = op.gameObject.layer;
			SetLayerRecursively(obj, obj.layer);
			if (weaponBonus != null)
			{
				GameObject obj2 = BonusCreator._CreateBonusFromPrefab(weaponBonus, Vector3.zero);
				obj2.transform.parent = op.transform;
				obj2.transform.localPosition = new Vector3(1.5f, 0f, 3f);
				obj2.layer = op.gameObject.layer;
				SetLayerRecursively(obj2, obj2.layer);
			}
			yield return null;
			op.Render_();
			UnityEngine.Object.Destroy(prer);
		}
	}

	private void TryCreateBossGuard(GameObject bossObj)
	{
		bossGuads = null;
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(bossObj.transform);
		if (botScriptForObject == null)
		{
			return;
		}
		int num = botScriptForObject.guards.Length;
		if (num != 0)
		{
			bossGuads = new GameObject[num];
			for (int i = 0; i < num; i++)
			{
				GameObject original = botScriptForObject.guards[i];
				bossGuads[i] = UnityEngine.Object.Instantiate(original);
				bossGuads[i].name = string.Format("{0}{1}", new object[2]
				{
					"BossGuard",
					i + 1
				});
				bossGuads[i].SetActive(false);
			}
		}
	}

	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", GetType().Name);
		using (new ScopeLogger(callee, false))
		{
			if (Defs.isMulti)
			{
				return;
			}
			if (GameConnect.isSurvival)
			{
				_enemiesInWaves.Clear();
				if (SceneLoader.ActiveSceneName.Equals("Pizza"))
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					List<string> list4 = new List<string>();
					List<string> list5 = new List<string>();
					List<string> list6 = new List<string>();
					list.Add("88");
					list.Add("85");
					list.Add("86");
					list2.Add("85");
					list2.Add("87");
					list2.Add("82");
					list2.Add("81");
					list2.Add("88");
					list3.Add("86");
					list3.Add("82");
					list3.Add("84");
					list3.Add("81");
					list3.Add("88");
					list3.Add("87");
					list4.Add("81");
					list4.Add("82");
					list4.Add("86");
					list4.Add("80");
					list4.Add("83");
					list4.Add("87");
					list4.Add("84");
					list5.Add("81");
					list5.Add("86");
					list5.Add("88");
					list5.Add("80");
					list5.Add("83");
					list5.Add("82");
					list5.Add("84");
					list5.Add("87");
					list6.Add("81");
					list6.Add("80");
					list6.Add("83");
					list6.Add("82");
					list6.Add("84");
					list6.Add("87");
					_enemiesInWaves.Add(list);
					_enemiesInWaves.Add(list2);
					_enemiesInWaves.Add(list3);
					_enemiesInWaves.Add(list4);
					_enemiesInWaves.Add(list5);
					_enemiesInWaves.Add(list6);
				}
				else
				{
					List<string> list7 = new List<string>();
					List<string> list8 = new List<string>();
					List<string> list9 = new List<string>();
					List<string> list10 = new List<string>();
					List<string> list11 = new List<string>();
					List<string> list12 = new List<string>();
					List<string> list13 = new List<string>();
					List<string> list14 = new List<string>();
					List<string> list15 = new List<string>();
					List<string> list16 = new List<string>();
					List<string> list17 = new List<string>();
					List<string> list18 = new List<string>();
					List<string> list19 = new List<string>();
					List<string> list20 = new List<string>();
					list7.Add("1");
					list7.Add("2");
					list7.Add("15");
					list8.Add("1");
					list8.Add("2");
					list8.Add("15");
					list8.Add("77");
					list8.Add("12");
					list9.Add("3");
					list9.Add("9");
					list9.Add("10");
					list9.Add("11");
					list9.Add("12");
					list9.Add("57");
					list10.Add("49");
					list10.Add("9");
					list10.Add("24");
					list10.Add("29");
					list10.Add("38");
					list10.Add("74");
					list10.Add("48");
					list10.Add("10");
					list11.Add("80");
					list11.Add("81");
					list11.Add("82");
					list11.Add("83");
					list11.Add("84");
					list11.Add("85");
					list11.Add("86");
					list11.Add("87");
					list11.Add("88");
					list12.Add("37");
					list12.Add("46");
					list12.Add("47");
					list12.Add("57");
					list12.Add("24");
					list12.Add("74");
					list12.Add("50");
					list12.Add("20");
					list12.Add("51");
					list13.Add("74");
					list13.Add("57");
					list13.Add("20");
					list13.Add("66");
					list13.Add("60");
					list13.Add("50");
					list13.Add("53");
					list13.Add("33");
					list13.Add("24");
					list13.Add("46");
					list14.Add("74");
					list14.Add("57");
					list14.Add("49");
					list14.Add("66");
					list14.Add("60");
					list14.Add("50");
					list14.Add("53");
					list14.Add("59");
					list14.Add("24");
					list14.Add("46");
					list15.Add("21");
					list15.Add("22");
					list15.Add("53");
					list15.Add("75");
					list15.Add("50");
					list15.Add("17");
					list15.Add("73");
					list15.Add("6");
					list15.Add("49");
					list15.Add("79");
					list16.Add("59");
					list16.Add("60");
					list16.Add("62");
					list16.Add("54");
					list16.Add("55");
					list16.Add("56");
					list16.Add("74");
					list16.Add("75");
					list16.Add("53");
					list17.Add("58");
					list17.Add("64");
					list17.Add("65");
					list18.Add("74");
					list18.Add("57");
					list18.Add("20");
					list18.Add("66");
					list18.Add("60");
					list18.Add("50");
					list18.Add("53");
					list18.Add("33");
					list18.Add("24");
					list18.Add("46");
					list19.Add("74");
					list19.Add("57");
					list19.Add("49");
					list19.Add("66");
					list19.Add("60");
					list19.Add("50");
					list19.Add("53");
					list19.Add("59");
					list19.Add("24");
					list19.Add("46");
					list20.Add("21");
					list20.Add("22");
					list20.Add("53");
					list20.Add("75");
					list20.Add("50");
					list20.Add("17");
					list20.Add("73");
					list20.Add("6");
					list20.Add("49");
					list20.Add("79");
					_enemiesInWaves.Add(list7);
					_enemiesInWaves.Add(list8);
					_enemiesInWaves.Add(list9);
					_enemiesInWaves.Add(list10);
					_enemiesInWaves.Add(list11);
					_enemiesInWaves.Add(list12);
					_enemiesInWaves.Add(list13);
					_enemiesInWaves.Add(list14);
					_enemiesInWaves.Add(list15);
					_enemiesInWaves.Add(list16);
					_enemiesInWaves.Add(list17);
					_enemiesInWaves.Add(list18);
					_enemiesInWaves.Add(list19);
					_enemiesInWaves.Add(list20);
				}
				foreach (List<string> enemiesInWave in _enemiesInWaves)
				{
					foreach (string item in enemiesInWave)
					{
						_allEnemiesSurvival.Add(item);
					}
				}
			}
			stopGeneratingBonuses = false;
			sharedCreator = this;
			if (!GameConnect.isSurvival && CurrentCampaignGame.currentLevel != 0)
			{
				string b = "Boss" + CurrentCampaignGame.currentLevel;
				boss = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", b))) as GameObject;
				TryCreateBossGuard(boss);
				boss.SetActive(false);
				if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
				{
					string weaponName = LevelBox.weaponsFromBosses[Application.loadedLevelName];
					weaponBonus = BonusCreator._CreateBonusPrefab(weaponName);
				}
				StartCoroutine(_PrerenderBoss());
				bossMus = Resources.Load("Snd/boss_campaign") as AudioClip;
			}
			GlobalGameController.curThr = GlobalGameController.thrStep;
			_enemies.Add(new string[6] { "1", "2", "1", "11", "12", "13" });
			_enemies.Add(new string[6] { "30", "31", "32", "33", "34", "77" });
			_enemies.Add(new string[9] { "1", "2", "3", "9", "10", "12", "14", "15", "78" });
			_enemies.Add(new string[7] { "1", "2", "4", "11", "9", "16", "78" });
			_enemies.Add(new string[7] { "1", "2", "4", "9", "11", "10", "12" });
			_enemies.Add(new string[6] { "43", "44", "45", "46", "47", "73" });
			_enemies.Add(new string[3] { "6", "7", "7" });
			_enemies.Add(new string[7] { "1", "2", "8", "10", "11", "12", "76" });
			_enemies.Add(new string[3] { "18", "19", "20" });
			_enemies.Add(new string[6] { "21", "22", "23", "24", "25", "75" });
			_enemies.Add(new string[2] { "1", "15" });
			_enemies.Add(new string[8] { "1", "3", "9", "10", "14", "15", "16", "78" });
			_enemies.Add(new string[4] { "8", "21", "22", "79" });
			_enemies.Add(new string[5] { "26", "27", "28", "29", "57" });
			_enemies.Add(new string[6] { "35", "36", "37", "38", "48", "57" });
			_enemies.Add(new string[5] { "39", "40", "41", "42", "74" });
			_enemies.Add(new string[4] { "53", "55", "57", "61" });
			_enemies.Add(new string[4] { "59", "56", "54", "60" });
			_enemies.Add(new string[4] { "67", "68", "66", "69" });
			_enemies.Add(new string[3] { "70", "71", "72" });
			_enemies.Add(new string[4] { "58", "63", "64", "65" });
			UpdateBotPrefabs();
			if (GameConnect.isSurvival)
			{
				_SetZombiePrefabs();
			}
			survivalAvailableWeapons.Clear();
			_UpdateAvailableWeapons();
			_UpdateIntervalStructures();
			StartCoroutine(_DrawFirstMessage());
		}
	}

	private void _SetZombiePrefabs()
	{
		waveZombiePrefabs.Clear();
		int index = ((currentWave >= _enemiesInWaves.Count) ? (_enemiesInWaves.Count - 1) : currentWave);
		foreach (GameObject zombiePrefab in zombiePrefabs)
		{
			string item = zombiePrefab.name.Substring("Enemy".Length).Substring(0, zombiePrefab.name.Substring("Enemy".Length).IndexOf("_"));
			if (_enemiesInWaves[index].Contains(item))
			{
				waveZombiePrefabs.Add(zombiePrefab);
			}
		}
	}

	private void _UpdateAvailableWeapons()
	{
		if (currentWave >= WeaponsAddedInWaves.Count)
		{
			return;
		}
		foreach (string item in WeaponsAddedInWaves[currentWave])
		{
			survivalAvailableWeapons.Add(item);
		}
	}

	private void UpdateBotPrefabs()
	{
		zombiePrefabs.Clear();
		string[] array = null;
		array = (GameConnect.isSurvival ? _allEnemiesSurvival.ToArray() : ((CurrentCampaignGame.currentLevel != 0) ? _enemies[CurrentCampaignGame.currentLevel - 1] : new string[1] { "1" }));
		string[] array2 = array;
		foreach (string text in array2)
		{
			GameObject item = Resources.Load("Enemies/Enemy" + text + "_go") as GameObject;
			zombiePrefabs.Add(item);
		}
	}

	private void Start()
	{
		if (GameConnect.isSurvival)
		{
			StoreKitEventListener.State.Parameters.Clear();
			StoreKitEventListener.State.Parameters.Add("Waves", currentWave + 1 + " In game");
		}
		labelStyle.fontSize = Mathf.RoundToInt(50f * Defs.Coef);
		labelStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		if (Defs.isMulti)
		{
			_isMultiplayer = true;
		}
		else
		{
			_isMultiplayer = false;
		}
		_teleports = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] teleports = _teleports;
		for (int i = 0; i < teleports.Length; i++)
		{
			teleports[i].SetActive(false);
		}
		if (!_isMultiplayer)
		{
			_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			if (!GameConnect.isSurvival)
			{
				_ResetInterval();
			}
		}
	}

	private void _ResetInterval()
	{
		curInterval = Mathf.Max(1f, curInterval);
	}

	public void BeganCreateEnemies()
	{
		if (!Application.isEditor || !GameConnect.isSurvival || SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length]))
		{
			if (GameConnect.isSurvival)
			{
				SetWaveNumberInGUI();
			}
			StartCoroutine(AddZombies());
		}
	}

	internal static int GetEnemiesToKill(string sceneName)
	{
		if (GameConnect.isSurvival)
		{
			return EnemyCountInSurvivalWave;
		}
		return GlobalGameController.GetEnemiesToKill(sceneName);
	}

	public static int GetCountMobsForLevel()
	{
		Dictionary<string, int> counCreateMobsInLevel = Switcher.counCreateMobsInLevel;
		string levelSceneName = CurrentCampaignGame.levelSceneName;
		if (counCreateMobsInLevel.ContainsKey(levelSceneName))
		{
			return counCreateMobsInLevel[levelSceneName];
		}
		return GlobalGameController.ZombiesInWave;
	}

	private IEnumerator AddZombies()
	{
		do
		{
			int zombiesInWave = GlobalGameController.ZombiesInWave;
			zombiesInWave = Mathf.Min(zombiesInWave, GlobalGameController.SimultaneousEnemiesOnLevelConstraint - NumOfLiveZombies);
			zombiesInWave = Mathf.Min(zombiesInWave, NumOfEnemisesToKill - (NumOfDeadZombies + NumOfLiveZombies));
			string[] array;
			if (!GameConnect.isSurvival)
			{
				array = ((CurrentCampaignGame.currentLevel != 0) ? _enemies[CurrentCampaignGame.currentLevel - 1] : new string[1] { "1" });
			}
			else
			{
				int index = ((currentWave >= _enemiesInWaves.Count) ? (_enemiesInWaves.Count - 1) : currentWave);
				array = _enemiesInWaves[index].ToArray();
			}
			for (int i = 0; i < zombiesInWave; i++)
			{
				int index2 = UnityEngine.Random.Range(0, array.Length);
				GameObject spawnZone = (GameConnect.isSurvival ? _enemyCreationZones[i % _enemyCreationZones.Length] : _enemyCreationZones[UnityEngine.Random.Range(0, _enemyCreationZones.Length)]);
				Vector3 position = _createPos(spawnZone);
				UnityEngine.Object.Instantiate(GameConnect.isSurvival ? waveZombiePrefabs[index2] : zombiePrefabs[index2], position, Quaternion.identity);
			}
			if (GameConnect.isSurvival && NumOfDeadZombies + NumOfLiveZombies >= NumOfEnemisesToKill)
			{
				_generatingZombiesIsStopped = true;
				do
				{
					yield return new WaitForEndOfFrame();
				}
				while (_generatingZombiesIsStopped);
			}
			yield return new WaitForSeconds(curInterval);
			if (GameConnect.isSurvival)
			{
				_genWithThisTimeInterval++;
				if (_genWithThisTimeInterval == 3 && _indexInTimesArray < _intervalArr.Length - 1)
				{
					_indexInTimesArray++;
				}
				curInterval = _intervalArr[_indexInTimesArray];
			}
		}
		while (NumOfDeadZombies + NumOfLiveZombies < NumOfEnemisesToKill || GameConnect.isSurvival);
	}

	private Vector3 _createPos(GameObject spawnZone)
	{
		BoxCollider component = spawnZone.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * spawnZone.transform.localScale.x, component.size.z * spawnZone.transform.localScale.z);
		Rect rect = new Rect(spawnZone.transform.position.x - vector.x / 2f, spawnZone.transform.position.z - vector.y / 2f, vector.x, vector.y);
		return new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), spawnZone.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
	}

	private void ShowGuards(Vector3 bossPosition)
	{
		if (bossGuads != null)
		{
			for (int i = 0; i < bossGuads.Length; i++)
			{
				bossGuads[i].transform.position = BaseBot.GetPositionSpawnGuard(bossPosition);
				bossGuads[i].transform.rotation = Quaternion.identity;
				bossGuads[i].SetActive(true);
			}
		}
	}

	private void CreateBoss()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
		if (!gameObject2)
		{
			return;
		}
		GameObject[] enemyCreationZones = _enemyCreationZones;
		foreach (GameObject gameObject3 in enemyCreationZones)
		{
			float num2 = Vector3.SqrMagnitude(gameObject2.transform.position - gameObject3.transform.position);
			float num3 = Mathf.Abs(gameObject2.transform.position.y - gameObject3.transform.position.y);
			if (num2 > 225f && num2 < num && num3 < 2.5f)
			{
				num = num2;
				gameObject = gameObject3;
			}
		}
		if (!gameObject)
		{
			gameObject = _enemyCreationZones[0];
		}
		Vector3 vector = _createPos(gameObject);
		if (boss != null)
		{
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("BossRespawnPoint");
			if (gameObject4 != null)
			{
				vector = gameObject4.transform.position;
			}
			boss.transform.position = vector;
			boss.transform.rotation = Quaternion.identity;
			boss.SetActive(true);
			ShowGuards(vector);
		}
		else if (GameConnect.isSurvival && bossesSurvival.ContainsKey(currentWave))
		{
			string b = "Boss" + bossesSurvival[currentWave];
			boss = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", b))) as GameObject;
			boss.transform.position = vector;
			boss.transform.rotation = Quaternion.identity;
		}
		if (boss != null && !GameConnect.isSurvival)
		{
			PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
			if (component != null)
			{
				BoxCollider componentInChildren = boss.gameObject.GetComponentInChildren<BoxCollider>();
				Vector3 localOffset = ((componentInChildren != null) ? componentInChildren.center : (1.618f * Vector3.up));
				component.RemovePointOfInterest();
				component.SetPointOfInterest(boss.transform, localOffset, Color.red);
			}
		}
		boss = null;
		bossShowm = true;
	}
}
