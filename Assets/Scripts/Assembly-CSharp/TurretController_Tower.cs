using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class TurretController_Tower : TurretController
{
	[CompilerGenerated]
	internal sealed class _003CScanTarget_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TurretController_Tower _003C_003E4__this;

		private float _003CclosestTarget_003E5__1;

		private GameObject _003CclosestTargetObj_003E5__2;

		private IEnumerator<Transform> _003C_003E7__wrap1;

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
		public _003CScanTarget_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.inScaning = true;
					_003CclosestTargetObj_003E5__2 = null;
					_003CclosestTarget_003E5__1 = float.MaxValue;
					Initializer.TargetsList targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
					_003C_003E7__wrap1 = targetsList.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Transform current = _003C_003E7__wrap1.Current;
					Vector3 to = current.position - _003C_003E4__this.transform.position;
					Vector3 from = new Vector3(to.x, 0f, to.z);
					float sqrMagnitude = to.sqrMagnitude;
					if (sqrMagnitude < _003CclosestTarget_003E5__1 && sqrMagnitude < _003C_003E4__this.maxRadiusScanTargetSQR && Vector3.Angle(from, to) < _003C_003E4__this.maxRotateX)
					{
						Vector3 vector = Vector3.zero;
						BoxCollider boxCollider = current.GetComponent<BoxCollider>();
						if (boxCollider == null && current.CompareTag("Enemy"))
						{
							for (int i = 0; i < current.childCount; i++)
							{
								BoxCollider component = current.GetChild(i).GetComponent<BoxCollider>();
								if (component != null)
								{
									boxCollider = component;
									break;
								}
							}
						}
						if (boxCollider != null)
						{
							vector = boxCollider.transform.rotation * boxCollider.center;
						}
						RaycastHit hitInfo;
						if (Physics.Raycast(new Ray(_003C_003E4__this.tower.position, current.transform.position + vector - _003C_003E4__this.tower.position), out hitInfo, _003C_003E4__this.maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && hitInfo.collider.gameObject.transform.root.Equals(current.root))
						{
							_003CclosestTarget_003E5__1 = sqrMagnitude;
							_003CclosestTargetObj_003E5__2 = current.gameObject;
						}
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				if (_003CclosestTargetObj_003E5__2 != null)
				{
					_003C_003E4__this.target = _003CclosestTargetObj_003E5__2.transform;
				}
				else
				{
					_003C_003E4__this.target = null;
				}
				_003C_003E4__this.inScaning = false;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("Tower settings")]
	public Transform tower;

	public Transform gun;

	private float idleAlphaY;

	private float idleRotateSpeedY = 20f;

	private float maxDeltaRotateY = 60f;

	private float maxRotateX = 75f;

	private float minRotateX = -60f;

	private float speedRotateY = 220f;

	private float speedRotateX = 30f;

	private float timerShot;

	protected override void SearchTarget()
	{
		base.SearchTarget();
		if (Mathf.Abs(idleAlphaY) < 0.5f)
		{
			idleAlphaY = UnityEngine.Random.Range(-1f * maxDeltaRotateY / 2f, maxDeltaRotateY / 2f);
		}
		else
		{
			float num = Time.deltaTime * idleRotateSpeedY * Mathf.Abs(idleAlphaY) / idleAlphaY;
			idleAlphaY -= num;
			tower.localRotation = Quaternion.Euler(new Vector3(0f, 0f, tower.localRotation.eulerAngles.z + num));
		}
		if (Mathf.Abs(gun.localRotation.eulerAngles.x) > 1f)
		{
			gun.Rotate((float)((!(gun.localRotation.eulerAngles.x < 180f)) ? 1 : (-1)) * speedRotateX * Time.deltaTime, 0f, 0f);
		}
	}

	protected override IEnumerator ScanTarget()
	{
		inScaning = true;
		GameObject closestTargetObj = null;
		float closestTarget = float.MaxValue;
		Initializer.TargetsList targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
		foreach (Transform item in targetsList)
		{
			Vector3 to = item.position - transform.position;
			Vector3 from = new Vector3(to.x, 0f, to.z);
			float sqrMagnitude = to.sqrMagnitude;
			if (sqrMagnitude < closestTarget && sqrMagnitude < maxRadiusScanTargetSQR && Vector3.Angle(from, to) < maxRotateX)
			{
				Vector3 vector = Vector3.zero;
				BoxCollider boxCollider = item.GetComponent<BoxCollider>();
				if (boxCollider == null && item.CompareTag("Enemy"))
				{
					for (int i = 0; i < item.childCount; i++)
					{
						BoxCollider component = item.GetChild(i).GetComponent<BoxCollider>();
						if (component != null)
						{
							boxCollider = component;
							break;
						}
					}
				}
				if (boxCollider != null)
				{
					vector = boxCollider.transform.rotation * boxCollider.center;
				}
				RaycastHit hitInfo;
				if (Physics.Raycast(new Ray(tower.position, item.transform.position + vector - tower.position), out hitInfo, maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && hitInfo.collider.gameObject.transform.root.Equals(item.root))
				{
					closestTarget = sqrMagnitude;
					closestTargetObj = item.gameObject;
				}
			}
			yield return null;
		}
		if (closestTargetObj != null)
		{
			target = closestTargetObj.transform;
		}
		else
		{
			target = null;
		}
		inScaning = false;
	}

	protected override void TargetUpdate()
	{
		base.TargetUpdate();
		bool flag = false;
		Vector2 to = new Vector2(target.position.x, target.position.z) - new Vector2(tower.position.x, tower.position.z);
		float deltaAngles = GetDeltaAngles(tower.rotation.eulerAngles.y, Mathf.Abs(to.x) / to.x * Vector2.Angle(Vector2.up, to));
		float num = (0f - speedRotateY) * Time.deltaTime * Mathf.Abs(deltaAngles) / deltaAngles;
		if (Mathf.Abs(deltaAngles) < 10f)
		{
			flag = true;
		}
		if (Mathf.Abs(num) > Mathf.Abs(deltaAngles))
		{
			num = 0f - deltaAngles;
		}
		if (Mathf.Abs(num) > 0.001f)
		{
			tower.Rotate(0f, 0f, num);
		}
		Vector3 vector = Vector3.zero;
		BoxCollider boxCollider = target.GetComponent<BoxCollider>();
		if (boxCollider == null && target.CompareTag("Enemy"))
		{
			for (int i = 0; i < target.childCount; i++)
			{
				BoxCollider component = target.GetChild(i).GetComponent<BoxCollider>();
				if (component != null)
				{
					boxCollider = component;
					break;
				}
			}
		}
		if (boxCollider != null)
		{
			vector = boxCollider.transform.rotation * boxCollider.center;
		}
		float angle = -180f * Mathf.Atan((target.position.y + vector.y - tower.position.y) / Vector3.Distance(target.position + vector, base.transform.position)) / (float)Math.PI;
		float deltaAngles2 = GetDeltaAngles(gun.localRotation.eulerAngles.x, angle);
		num = (0f - speedRotateX) * Time.deltaTime * Mathf.Abs(deltaAngles2) / deltaAngles2;
		if (Mathf.Abs(num) > Mathf.Abs(deltaAngles2))
		{
			num = 0f - deltaAngles2;
		}
		if (Mathf.Abs(num) > 0.001f)
		{
			gun.Rotate(num, 0f, 0f);
		}
		if (!flag)
		{
			return;
		}
		timerShot -= Time.deltaTime;
		if (timerShot < 0f)
		{
			if (!Defs.isMulti)
			{
				ShotRPC();
			}
			else if (Defs.isInet)
			{
				photonView.RPC("ShotRPC", PhotonTargets.All);
			}
			timerShot = maxTimerShot;
		}
	}

	private float GetDeltaAngles(float angle1, float angle2)
	{
		if (angle1 < 0f)
		{
			angle1 += 360f;
		}
		if (angle2 < 0f)
		{
			angle2 += 360f;
		}
		float num = angle1 - angle2;
		if (Mathf.Abs(num) > 180f)
		{
			num = ((!(angle1 > angle2)) ? (num + 360f) : (num - 360f));
		}
		return num;
	}

	protected override void OnKill()
	{
		if (gun.rotation.x > minRotateX)
		{
			gun.Rotate(speedRotateX * Time.deltaTime, 0f, 0f);
		}
	}

	protected override void UpdateTurret()
	{
		base.UpdateTurret();
	}
}
