using System;
using Rilisoft;
using UnityEngine;

public class Weapon
{
	private static System.Random _prng = new System.Random(15237);

	public GameObject weaponPrefab;

	private SaltedInt _currentAmmoInBackpack;

	private SaltedInt _currentAmmoInClip;

	public int currentAmmoInBackpack
	{
		get
		{
			return _currentAmmoInBackpack.Value;
		}
		set
		{
			_currentAmmoInBackpack = new SaltedInt(_prng.Next(), value);
		}
	}

	public int currentAmmoInClip
	{
		get
		{
			return _currentAmmoInClip.Value;
		}
		set
		{
			_currentAmmoInClip = new SaltedInt(_prng.Next(), value);
		}
	}
}
