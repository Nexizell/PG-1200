using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.WP8;
using UnityEngine;

public sealed class TurretController_MusicBox : TurretController
{
	[CompilerGenerated]
	internal sealed class _003CScanTarget_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TurretController_MusicBox _003C_003E4__this;

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
		public _003CScanTarget_003Ed__5(int _003C_003E1__state)
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
					Vector3 vector = current.position - _003C_003E4__this.transform.position;
					new Vector3(vector.x, 0f, vector.z);
					float sqrMagnitude = vector.sqrMagnitude;
					if (sqrMagnitude < _003CclosestTarget_003E5__1 && sqrMagnitude < _003C_003E4__this.maxRadiusScanTargetSQR)
					{
						Vector3 vector2 = Vector3.zero;
						BoxCollider component = current.GetComponent<BoxCollider>();
						if (component != null)
						{
							vector2 = component.center;
						}
						RaycastHit hitInfo;
						if (Physics.Raycast(new Ray(_003C_003E4__this.tower.position, current.position + vector2 - _003C_003E4__this.tower.position), out hitInfo, _003C_003E4__this.maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && (hitInfo.collider.gameObject == current.gameObject || (hitInfo.collider.gameObject.transform.parent != null && (hitInfo.collider.gameObject.transform.parent.Equals(current) || hitInfo.collider.gameObject.transform.parent.Equals(current.parent)))))
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

	[Header("MusicBox settings")]
	public AudioClip musicDater;

	public Transform tower;

	public Transform gun;

	private bool isPlayMusicDater;

	protected override void SearchTarget()
	{
		base.SearchTarget();
		if (isPlayMusicDater)
		{
			PlayMusic(false);
			if (Defs.isInet)
			{
				photonView.RPC("PlayMusic", PhotonTargets.Others, false);
			}
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
			Vector3 vector = item.position - transform.position;
			new Vector3(vector.x, 0f, vector.z);
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < closestTarget && sqrMagnitude < maxRadiusScanTargetSQR)
			{
				Vector3 vector2 = Vector3.zero;
				BoxCollider component = item.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector2 = component.center;
				}
				RaycastHit hitInfo;
				if (Physics.Raycast(new Ray(tower.position, item.position + vector2 - tower.position), out hitInfo, maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && (hitInfo.collider.gameObject == item.gameObject || (hitInfo.collider.gameObject.transform.parent != null && (hitInfo.collider.gameObject.transform.parent.Equals(item) || hitInfo.collider.gameObject.transform.parent.Equals(item.parent)))))
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
		if (!isPlayMusicDater)
		{
			PlayMusic(true);
			if (Defs.isInet)
			{
				photonView.RPC("PlayMusic", PhotonTargets.Others, true);
			}
		}
	}

	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (isPlayMusicDater)
		{
			tower.Rotate(new Vector3(0f, 0f, 180f * Time.deltaTime));
			gun.Rotate(new Vector3(180f * Time.deltaTime, 0f, 0f));
		}
	}

	[RPC]
	[PunRPC]
	private void PlayMusic(bool isPlay)
	{
		if (isPlayMusicDater == isPlay)
		{
			return;
		}
		isPlayMusicDater = isPlay;
		if (isPlay)
		{
			if (Defs.isSoundFX)
			{
				GetComponent<AudioSource>().loop = true;
				GetComponent<AudioSource>().clip = musicDater;
				GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	protected override void PlayerConnectedPhoton(PhotonPlayer player)
	{
		base.PlayerConnectedPhoton(player);
		photonView.RPC("PlayMusic", player, isPlayMusicDater);
	}
}
