using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlashFire : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CRailgunRayCoroutine_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FlashFire _003C_003E4__this;

		public Player_move_c moveC;

		private Ray _003Cray_003E5__1;

		private int _003C_countReflection_003E5__2;

		private bool _003CisReflection_003E5__3;

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
		public _003CRailgunRayCoroutine_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003C_countReflection_003E5__2++;
				if (_003CisReflection_003E5__3 && _003C_countReflection_003E5__2 < _003C_003E4__this.ws.countReflectionRay)
				{
					goto IL_00b5;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003Cray_003E5__1 = new Ray(_003C_003E4__this.gunFlashObj.transform.parent.position, _003C_003E4__this.gunFlashObj.transform.parent.parent.forward);
				_003CisReflection_003E5__3 = false;
				_003C_countReflection_003E5__2 = 0;
				if (_003C_003E4__this.ws.countReflectionRay != 1)
				{
					goto IL_00b5;
				}
				WeaponManager.AddRay(_003Cray_003E5__1.origin, _003Cray_003E5__1.direction, _003C_003E4__this.ws.railName);
			}
			return false;
			IL_00b5:
			Player_move_c.RayHitsInfo hitsFromRay = moveC.GetHitsFromRay(_003Cray_003E5__1, false);
			bool num2 = _003C_countReflection_003E5__2 == 0 && !hitsFromRay.obstacleFound;
			Vector3 direction = _003Cray_003E5__1.direction;
			float len = (num2 ? 150f : hitsFromRay.lenRay);
			WeaponManager.AddRay(_003Cray_003E5__1.origin, direction, _003C_003E4__this.ws.railName, len);
			if (hitsFromRay.obstacleFound)
			{
				_003Cray_003E5__1 = hitsFromRay.rayReflect;
				_003CisReflection_003E5__3 = true;
			}
			_003C_003E2__current = new WaitForSeconds((float)_003C_countReflection_003E5__2 * 0.05f);
			_003C_003E1__state = 1;
			return true;
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

	public GameObject gunFlashObj;

	public float timeFireAction = 0.2f;

	private float activeTime;

	private WeaponSounds ws;

	private void Awake()
	{
		ws = GetComponent<WeaponSounds>();
	}

	private void Start()
	{
		if (gunFlashObj == null)
		{
			foreach (Transform item in base.transform)
			{
				bool flag = false;
				if (item.gameObject.name.Equals("BulletSpawnPoint"))
				{
					foreach (Transform item2 in item)
					{
						if (item2.gameObject.name.Equals("GunFlash"))
						{
							flag = true;
							gunFlashObj = item2.gameObject;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		WeaponManager.SetGunFlashActive(gunFlashObj, false);
	}

	private void Update()
	{
		if (activeTime > 0f)
		{
			activeTime -= Time.deltaTime;
			if (activeTime <= 0f)
			{
				WeaponManager.SetGunFlashActive(gunFlashObj, false);
			}
		}
	}

	public void StartGunFlash()
	{
		if (base.gameObject.activeInHierarchy)
		{
			WeaponManager.SetGunFlashActive(gunFlashObj, true);
			activeTime = timeFireAction;
		}
	}

	public void StartGunFlashDelayed(float delay)
	{
		Invoke("StartGunFlash", delay);
	}

	public void StartRailgunRay(Player_move_c moveC)
	{
		if (ws != null && ws.railgun && gunFlashObj != null)
		{
			StartCoroutine(RailgunRayCoroutine(moveC));
		}
	}

	public IEnumerator RailgunRayCoroutine(Player_move_c moveC)
	{
		Ray ray = new Ray(gunFlashObj.transform.parent.position, gunFlashObj.transform.parent.parent.forward);
		bool isReflection = false;
		int _countReflection = 0;
		if (ws.countReflectionRay == 1)
		{
			WeaponManager.AddRay(ray.origin, ray.direction, ws.railName);
			yield break;
		}
		do
		{
			Player_move_c.RayHitsInfo hitsFromRay = moveC.GetHitsFromRay(ray, false);
			bool num = _countReflection == 0 && !hitsFromRay.obstacleFound;
			Vector3 direction = ray.direction;
			float len = (num ? 150f : hitsFromRay.lenRay);
			WeaponManager.AddRay(ray.origin, direction, ws.railName, len);
			if (hitsFromRay.obstacleFound)
			{
				ray = hitsFromRay.rayReflect;
				isReflection = true;
			}
			yield return new WaitForSeconds((float)_countReflection * 0.05f);
			_countReflection++;
		}
		while (isReflection && _countReflection < ws.countReflectionRay);
	}
}
