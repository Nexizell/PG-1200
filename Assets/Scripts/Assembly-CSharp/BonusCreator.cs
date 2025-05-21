using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class BonusCreator : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CAddWeapon_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BonusCreator _003C_003E4__this;

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
		public _003CAddWeapon_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num;
			GameObject gameObject;
			BoxCollider component;
			Vector2 vector;
			Rect rect;
			Vector3 pos;
			float max;
			int num2;
			GameObject value;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0022;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0061;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0061;
				}
				IL_0061:
				if (_003C_003E4__this._zombieCreator.stopGeneratingBonuses)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				num = GlobalGameController.EnemiesToKill - _003C_003E4__this._zombieCreator.NumOfDeadZombies;
				if (GameConnect.isCampaign && num <= 0 && !_003C_003E4__this._zombieCreator.bossShowm)
				{
					return false;
				}
				gameObject = _003C_003E4__this._bonusCreationZones[UnityEngine.Random.Range(0, _003C_003E4__this._bonusCreationZones.Length)];
				component = gameObject.GetComponent<BoxCollider>();
				vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
				rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
				pos = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
				max = _003C_003E4__this._DistrSum();
				do
				{
					num2 = 0;
					float num3 = UnityEngine.Random.Range(0f, max);
					float num4 = 0f;
					for (int i = 0; i < _003C_003E4__this._weaponsProbDistr.Count; i++)
					{
						if (num3 < num4 + (float)(int)_003C_003E4__this._weaponsProbDistr[i])
						{
							num2 = i;
							break;
						}
						num4 += (float)(int)_003C_003E4__this._weaponsProbDistr[i];
					}
				}
				while (num2 == _003C_003E4__this._lastWeapon || (GameConnect.isSurvival && !ZombieCreator.survivalAvailableWeapons.Contains(_003C_003E4__this.weaponPrefabs[num2].name)) || !ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName((_003C_003E4__this.weaponPrefabs[num2] as GameObject).name.Replace("(Clone)", "")).Tag));
				value = _CreateBonus(((GameObject)_003C_003E4__this.weaponPrefabs[num2]).name, pos);
				_003C_003E4__this._weapons.Add(value);
				if (_003C_003E4__this._weapons.Count > (GameConnect.isSurvival ? 3 : 5))
				{
					UnityEngine.Object.Destroy((GameObject)_003C_003E4__this._weapons[0]);
					_003C_003E4__this._weapons.RemoveAt(0);
				}
				goto IL_0022;
				IL_0022:
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.weaponCreationInterval);
				_003C_003E1__state = 1;
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

	public GameObject[] bonusPrefabs;

	public float creationInterval = 15f;

	public float weaponCreationInterval = 30f;

	private UnityEngine.Object[] weaponPrefabs;

	private int _lastWeapon = -1;

	private bool _isMultiplayer;

	private ArrayList bonuses = new ArrayList();

	private ArrayList _weapons = new ArrayList();

	public WeaponManager _weaponManager;

	private GameObject[] _bonusCreationZones;

	private ZombieCreator _zombieCreator;

	private ArrayList _weaponsProbDistr = new ArrayList();

	private float _DistrSum()
	{
		float num = 0f;
		foreach (int item in _weaponsProbDistr)
		{
			num += (float)item;
		}
		return num;
	}

	private void Awake()
	{
		if (GameConnect.isSurvival)
		{
			creationInterval = 9f;
			weaponCreationInterval = 15f;
		}
		if (Defs.isMulti)
		{
			_isMultiplayer = true;
		}
		else
		{
			_isMultiplayer = false;
		}
		if (!_isMultiplayer)
		{
			weaponPrefabs = WeaponManager.sharedManager.weaponsInGame;
			UnityEngine.Object[] array = weaponPrefabs;
			for (int i = 0; i < array.Length; i++)
			{
				WeaponSounds component = ((GameObject)array[i]).GetComponent<WeaponSounds>();
				_weaponsProbDistr.Add(component.Probability);
			}
		}
	}

	private void Start()
	{
		_bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		_zombieCreator = base.gameObject.GetComponent<ZombieCreator>();
		_weaponManager = WeaponManager.sharedManager;
	}

	public void BeginCreateBonuses()
	{
		if ((!Application.isEditor || !GameConnect.isSurvival || SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length])) && GameConnect.isSurvival)
		{
			StartCoroutine(AddWeapon());
		}
	}

	public void addBonusFromPhotonRPC(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		GameObject obj = UnityEngine.Object.Instantiate(bonusPrefabs[_indexForType(_type)], _pos, rot);
		obj.GetComponent<PhotonView>().viewID = _id;
		obj.GetComponent<SettingBonus>().typeOfMass = _type;
	}

	private int _indexForType(int type)
	{
		int result = 0;
		switch (type)
		{
		case 9:
		case 10:
			result = 1;
			break;
		case 8:
			result = 2;
			break;
		}
		return result;
	}

	private IEnumerator AddWeapon()
	{
		while (true)
		{
			yield return new WaitForSeconds(weaponCreationInterval);
			while (_zombieCreator.stopGeneratingBonuses)
			{
				yield return null;
			}
			int num = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (GameConnect.isCampaign && num <= 0 && !_zombieCreator.bossShowm)
			{
				break;
			}
			GameObject gameObject = _bonusCreationZones[UnityEngine.Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
			Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
			Vector3 pos = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
			float max = _DistrSum();
			int num2;
			do
			{
				num2 = 0;
				float num3 = UnityEngine.Random.Range(0f, max);
				float num4 = 0f;
				for (int i = 0; i < _weaponsProbDistr.Count; i++)
				{
					if (num3 < num4 + (float)(int)_weaponsProbDistr[i])
					{
						num2 = i;
						break;
					}
					num4 += (float)(int)_weaponsProbDistr[i];
				}
			}
			while (num2 == _lastWeapon || (GameConnect.isSurvival && !ZombieCreator.survivalAvailableWeapons.Contains(weaponPrefabs[num2].name)) || !ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName((weaponPrefabs[num2] as GameObject).name.Replace("(Clone)", "")).Tag));
			GameObject value = _CreateBonus(((GameObject)weaponPrefabs[num2]).name, pos);
			_weapons.Add(value);
			if (_weapons.Count > (GameConnect.isSurvival ? 3 : 5))
			{
				UnityEngine.Object.Destroy((GameObject)_weapons[0]);
				_weapons.RemoveAt(0);
			}
		}
	}

	public static GameObject _CreateBonusPrefab(string _weaponName)
	{
		GameObject gameObject = Resources.Load("Weapon_Bonuses/" + _weaponName + "_Bonus") as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.Log("null");
			return null;
		}
		return gameObject;
	}

	public static GameObject _CreateBonus(string _weaponName, Vector3 pos)
	{
		GameObject gameObject = _CreateBonusPrefab(_weaponName);
		if (gameObject == null)
		{
			UnityEngine.Debug.Log("null");
			return null;
		}
		return _CreateBonusFromPrefab(gameObject, pos);
	}

	public static GameObject _CreateBonusFromPrefab(UnityEngine.Object bonus, Vector3 pos)
	{
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(bonus, pos, Quaternion.identity);
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		return obj;
	}

	private int _curLevel()
	{
		if (Defs.isMulti)
		{
			return GlobalGameController.currentLevel;
		}
		return CurrentCampaignGame.currentLevel;
	}
}
