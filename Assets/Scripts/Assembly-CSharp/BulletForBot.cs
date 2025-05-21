using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class BulletForBot : MonoBehaviour
{
	public delegate void OnBulletDamageDelegate(GameObject targetDamage, Vector3 positionDamage);

	[CompilerGenerated]
	internal sealed class _003CApplyForce_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BulletForBot _003C_003E4__this;

		public Vector3 force;

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
		public _003CApplyForce_003Ed__29(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
				return false;
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

	[NonSerialized]
	public float lifeTime;

	private float _bulletSpeed;

	private Vector3 _startPos;

	private Vector3 _endPos;

	private bool _isFrienlyFire;

	private float _startBulletTime;

	private bool doDamage = true;

	private bool _isMoveByPhysics;

	public bool needDestroyByStop { get; set; }

	public bool IsUse { get; private set; }

	public event OnBulletDamageDelegate OnBulletDamage;

	public void StartBullet(Vector3 startPos, Vector3 endPos, float bulletSpeed, bool isFriendlyFire, bool doDamage)
	{
		_isMoveByPhysics = false;
		_startPos = startPos;
		_endPos = endPos;
		_isFrienlyFire = isFriendlyFire;
		_bulletSpeed = bulletSpeed;
		base.transform.position = _startPos;
		IsUse = true;
		base.transform.gameObject.SetActive(true);
		_startBulletTime = Time.realtimeSinceStartup;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		this.doDamage = doDamage;
	}

	private void StopBullet()
	{
		if (needDestroyByStop)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (_isMoveByPhysics)
		{
			SetVisible(false);
		}
		else
		{
			base.transform.gameObject.SetActive(false);
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		IsUse = false;
		if (_isMoveByPhysics)
		{
			EnablePhysicsGravityControll(false);
		}
	}

	private void OnTriggerEnter(Collider collisionObj)
	{
		CollisionEvent(collisionObj.gameObject);
	}

	private void OnCollisionEnter(Collision collisionObj)
	{
		CollisionEvent(collisionObj.gameObject);
	}

	private void CollisionEvent(GameObject collisionObj)
	{
		if (!IsUse)
		{
			return;
		}
		Transform root = collisionObj.transform.root;
		if (!(base.transform.root == root.transform.root) && (_isFrienlyFire || !root.tag.Equals("Enemy")))
		{
			if (root.tag.Equals("Player") || root.tag.Equals("Turret") || root.tag.Equals("Pet"))
			{
				CheckRunDamageEvent(root.gameObject);
			}
			else if (_isFrienlyFire && root.tag.Equals("Enemy"))
			{
				CheckRunDamageEvent(root.gameObject);
			}
			else
			{
				CheckRunDamageEvent(null);
			}
		}
	}

	private void CheckRunDamageEvent(GameObject target)
	{
		if (this.OnBulletDamage != null)
		{
			if (doDamage)
			{
				this.OnBulletDamage(target, base.transform.position);
			}
			StopBullet();
		}
	}

	private void Update()
	{
		if (IsUse)
		{
			if (!_isMoveByPhysics)
			{
				Vector3 vector = _endPos - _startPos;
				base.transform.position += vector.normalized * _bulletSpeed * Time.deltaTime;
			}
			if (Time.realtimeSinceStartup - _startBulletTime >= lifeTime)
			{
				StopBullet();
			}
		}
	}

	private void EnablePhysicsGravityControll(bool enable)
	{
		GetComponent<Rigidbody>().useGravity = enable;
		GetComponent<Rigidbody>().isKinematic = !enable;
	}

	public void ApplyForceFroBullet(Vector3 startPos, Vector3 endPos, bool isFriendlyFire, float forceValue, Vector3 forceVector, bool doDamage)
	{
		_isMoveByPhysics = true;
		_isFrienlyFire = isFriendlyFire;
		_startBulletTime = Time.realtimeSinceStartup;
		base.transform.position = startPos;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		SetVisible(true);
		EnablePhysicsGravityControll(true);
		IsUse = true;
		this.doDamage = doDamage;
		StartCoroutine(ApplyForce(forceVector * forceValue));
	}

	private IEnumerator ApplyForce(Vector3 force)
	{
		yield return new WaitForEndOfFrame();
		GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}

	private void SetVisible(bool enable)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(enable);
		}
		if (GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().enabled = enable;
		}
		if (GetComponent<ParticleSystem>() != null)
		{
			GetComponent<ParticleSystem>().enableEmission = enable;
		}
		if (GetComponent<Collider>() != null)
		{
			GetComponent<Collider>().enabled = enable;
		}
	}
}
