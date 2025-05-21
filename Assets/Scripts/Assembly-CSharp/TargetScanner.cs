using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CCheckTargets_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TargetScanner _003C_003E4__this;

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
		public _003CCheckTargets_003Ed__5(int _003C_003E1__state)
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
				Initializer.TargetsList targetsList;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (GameConnect.isDaterRegim)
					{
						return false;
					}
					goto IL_0035;
				case 1:
					_003C_003E1__state = -3;
					break;
				case 2:
					{
						_003C_003E1__state = -1;
						_003CclosestTargetObj_003E5__2 = null;
						goto IL_0035;
					}
					IL_0035:
					_003CclosestTargetObj_003E5__2 = null;
					_003CclosestTarget_003E5__1 = float.MaxValue;
					targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
					_003C_003E7__wrap1 = targetsList.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Transform current = _003C_003E7__wrap1.Current;
					if (current == null || current == _003C_003E4__this.gameObject || current == WeaponManager.sharedManager.myPlayer)
					{
						continue;
					}
					Vector3 vector = current.position - _003C_003E4__this.transform.position;
					Vector3 vector2 = ((_003C_003E4__this.LookPoint != null) ? _003C_003E4__this.LookPoint.position : _003C_003E4__this.transform.position);
					float sqrMagnitude = vector.sqrMagnitude;
					if ((sqrMagnitude < _003CclosestTarget_003E5__1 && sqrMagnitude < Mathf.Pow(_003C_003E4__this.DetectRadius, 2f)) || GameConnect.isDaterRegim)
					{
						BoxCollider component = ((current.childCount > 0) ? current.GetChild(0).GetComponent<BoxCollider>() : (component = current.GetComponent<BoxCollider>()));
						Vector3 vector3 = ((component != null) ? new Vector3(0f, component.center.y, 0f) : Vector3.zero);
						Vector3 direction = current.position + vector3 - vector2;
						RaycastHit hitInfo;
						if (Physics.Raycast(new Ray(vector2, direction), layerMask: Tools.AllWithoutDamageCollidersMaskAndWithoutRocket & ~(1 << LayerMask.NameToLayer("Pets")), hitInfo: out hitInfo, maxDistance: _003C_003E4__this.DetectRadius))
						{
							GameObject gameObject = hitInfo.collider.gameObject;
							bool flag = false;
							if (gameObject.Equals(current.gameObject))
							{
								flag = true;
							}
							else if (gameObject.transform.parent != null)
							{
								if (gameObject.transform.parent.Equals(current) || gameObject.transform.parent.Equals(current.parent))
								{
									flag = true;
								}
								else if (gameObject.transform.parent.gameObject.IsSubobjectOf(current.gameObject))
								{
									flag = true;
								}
							}
							if (flag)
							{
								_003CclosestTarget_003E5__1 = sqrMagnitude;
								_003CclosestTargetObj_003E5__2 = current.gameObject;
							}
						}
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				_003C_003E4__this.Target = _003CclosestTargetObj_003E5__2;
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.UpdateFrequency);
				_003C_003E1__state = 2;
				return true;
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

	public float DetectRadius = 30f;

	[Range(0f, 2f)]
	public float UpdateFrequency = 0.3f;

	[ReadOnly]
	public GameObject Target;

	public Transform LookPoint;

	private void OnEnable()
	{
		StartCoroutine(CheckTargets());
	}

	private IEnumerator CheckTargets()
	{
		if (GameConnect.isDaterRegim)
		{
			yield break;
		}
		while (true)
		{
			GameObject closestTargetObj = null;
			float closestTarget = float.MaxValue;
			Initializer.TargetsList targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
			foreach (Transform item in targetsList)
			{
				if (item == null || item == this.gameObject || item == WeaponManager.sharedManager.myPlayer)
				{
					continue;
				}
				Vector3 vector = item.position - transform.position;
				Vector3 vector2 = ((LookPoint != null) ? LookPoint.position : transform.position);
				float sqrMagnitude = vector.sqrMagnitude;
				if ((sqrMagnitude < closestTarget && sqrMagnitude < Mathf.Pow(DetectRadius, 2f)) || GameConnect.isDaterRegim)
				{
					BoxCollider boxCollider = ((item.childCount > 0) ? item.GetChild(0).GetComponent<BoxCollider>() : item.GetComponent<BoxCollider>());
					Vector3 vector3 = ((boxCollider != null) ? new Vector3(0f, boxCollider.center.y, 0f) : Vector3.zero);
					Vector3 direction = item.position + vector3 - vector2;
					RaycastHit hitInfo;
					if (Physics.Raycast(new Ray(vector2, direction), layerMask: Tools.AllWithoutDamageCollidersMaskAndWithoutRocket & ~(1 << LayerMask.NameToLayer("Pets")), hitInfo: out hitInfo, maxDistance: DetectRadius))
					{
						GameObject gameObject = hitInfo.collider.gameObject;
						bool flag = false;
						if (gameObject.Equals(item.gameObject))
						{
							flag = true;
						}
						else if (gameObject.transform.parent != null)
						{
							if (gameObject.transform.parent.Equals(item) || gameObject.transform.parent.Equals(item.parent))
							{
								flag = true;
							}
							else if (gameObject.transform.parent.gameObject.IsSubobjectOf(item.gameObject))
							{
								flag = true;
							}
						}
						if (flag)
						{
							closestTarget = sqrMagnitude;
							closestTargetObj = item.gameObject;
						}
					}
				}
				yield return null;
			}
			Target = closestTargetObj;
			yield return new WaitForSeconds(UpdateFrequency);
		}
	}
}
