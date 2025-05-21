using System;
using Photon;
using Rilisoft.WP8;
using UnityEngine;

namespace Rilisoft
{
	public sealed class NetworkPetEngineSynch : Photon.MonoBehaviour
	{
		internal struct MovementHistoryEntry
		{
			public Vector3 playerPos;

			public Quaternion playerRot;

			public PetAnimationType anim;

			public double timeStamp;

			public bool teleported;
		}

		private MovementHistoryEntry[] movementHistory;

		private Vector3 correctPlayerPos;

		private double correctPlayerTime;

		private Quaternion correctPlayerRot = Quaternion.identity;

		private Transform thisTransform;

		private double myTime;

		private int historyLengh = 8;

		private bool isHistoryClear = true;

		public PetAnimationType currentAnimation;

		private int myAnimOld;

		public bool isTeleported;

		private bool isFirstSnapshot = true;

		private bool isMine;

		private bool isFirstHistoryFull;

		private PetEngine engine;

		private bool _effectPlayed;

		private bool isHided;

		private void Awake()
		{
			if (!Defs.isMulti)
			{
				base.enabled = false;
			}
			thisTransform = base.transform;
			correctPlayerPos = new Vector3(0f, -10000f, 0f);
			movementHistory = new MovementHistoryEntry[historyLengh];
			for (int i = 0; i < historyLengh; i++)
			{
				movementHistory[i].timeStamp = 0.0;
			}
			myTime = 1.0;
			engine = GetComponent<PetEngine>();
			engine.OnStateChanged += Engine_OnStateChanged;
		}

		private void Start()
		{
			if (Defs.isInet && base.photonView.isMine)
			{
				isMine = true;
			}
		}

		private void Engine_OnStateChanged(PetState currentState, PetState prevState)
		{
			switch (currentState)
			{
			case PetState.death:
				Call_IsVisible(false, true);
				return;
			case PetState.teleport:
				Call_IsVisible(false, false);
				break;
			}
			if (prevState == PetState.teleport)
			{
				Call_IsVisible(true, true);
			}
		}

		private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				stream.SendNext(thisTransform.position);
				stream.SendNext((short)(thisTransform.rotation.eulerAngles.y * 10f));
				stream.SendNext(PhotonNetwork.time);
				stream.SendNext((byte)currentAnimation);
				stream.SendNext(Convert.ToByte(isTeleported));
				isTeleported = false;
				if (Application.isEditor)
				{
					PhotonTrafficStatistic.AddOutcomingRPC("Stream " + GetType().ToString(), stream.ToArray());
				}
				return;
			}
			if (Application.isEditor)
			{
				PhotonTrafficStatistic.AddIncomingRPC("Stream " + GetType().ToString(), stream.ToArray());
			}
			if (!isFirstSnapshot)
			{
				correctPlayerPos = (Vector3)stream.ReceiveNext();
				correctPlayerRot = Quaternion.Euler(0f, (float)(short)stream.ReceiveNext() / 10f, 0f);
				int num = 0;
				correctPlayerTime = (double)stream.ReceiveNext();
				num = (byte)stream.ReceiveNext();
				isTeleported = Convert.ToBoolean(stream.ReceiveNext());
				if (isTeleported || Mathf.Abs((float)myTime - (float)correctPlayerTime) > 1000f)
				{
					isHistoryClear = true;
					myTime = correctPlayerTime;
				}
				if (!isHided)
				{
					AddNewSnapshot(correctPlayerPos, correctPlayerRot, correctPlayerTime, num, isTeleported);
				}
			}
			else
			{
				isFirstSnapshot = false;
			}
		}

		private void Update()
		{
			if (isMine || engine.Owner == null)
			{
				return;
			}
			if (!engine.IsAlive || (engine.Owner != null && engine.Owner.isKilled))
			{
				engine.IsImmortal = true;
			}
			if (isHided || !engine.IsAlive)
			{
				return;
			}
			if (!isHistoryClear)
			{
				double num = ((!(myTime + (double)Time.deltaTime < movementHistory[movementHistory.Length - 1].timeStamp)) ? (myTime + (double)Time.deltaTime) : (myTime + (double)(Time.deltaTime * 1.5f)));
				int num2 = 0;
				for (int i = 0; i < movementHistory.Length && movementHistory[i].timeStamp > myTime; i++)
				{
					num2 = i;
				}
				if (num2 == 0)
				{
					isHistoryClear = true;
				}
				if ((movementHistory[num2].timeStamp - myTime > 4.0 && num2 > 0) || movementHistory[num2].teleported)
				{
					num2--;
					thisTransform.position = movementHistory[num2].playerPos;
					thisTransform.rotation = movementHistory[num2].playerRot;
					myTime = movementHistory[num2].timeStamp;
					engine.ThisTransform.position = engine.MovePosition;
					engine.ShowModel();
					engine.PlayShowEffect();
				}
				else
				{
					float t = (float)((num - myTime) / (movementHistory[num2].timeStamp - myTime));
					thisTransform.position = Vector3.Lerp(thisTransform.position, movementHistory[num2].playerPos, t);
					if (!Device.isPixelGunLow)
					{
						thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, movementHistory[num2].playerRot, t);
					}
					else
					{
						thisTransform.rotation = movementHistory[num2].playerRot;
					}
					myTime = num;
					switch (movementHistory[num2].anim)
					{
					case PetAnimationType.Attack:
						engine.PlayAnimation(movementHistory[num2].anim);
						break;
					case PetAnimationType.Dead:
						engine.PlayAnimation(movementHistory[num2].anim);
						engine.PlaySound(engine.ClipDeath);
						break;
					default:
						engine.SetMovementAnimation();
						engine.SetMovementAnimSpeed();
						break;
					}
					if (movementHistory[num2].teleported)
					{
						engine.ShowModel();
						engine.PlayShowEffect();
					}
				}
			}
			else if (!isHistoryClear || movementHistory[movementHistory.Length - 1].teleported)
			{
				thisTransform.position = movementHistory[movementHistory.Length - 1].playerPos;
				thisTransform.rotation = movementHistory[movementHistory.Length - 1].playerRot;
				myTime = movementHistory[movementHistory.Length - 1].timeStamp;
				engine.ShowModel();
				engine.PlayShowEffect();
			}
			if (engine.Owner != null && engine.Owner.isKilled)
			{
				if (!_effectPlayed)
				{
					_effectPlayed = true;
					engine.PlayShowEffect();
				}
				base.transform.position = Vector3.down * 10000f;
			}
			else
			{
				_effectPlayed = false;
				if (engine.IsImmortal)
				{
					engine.BlinkImmortal();
				}
			}
		}

		private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp, int _anim, bool teleported)
		{
			for (int num = movementHistory.Length - 1; num > 0; num--)
			{
				movementHistory[num] = movementHistory[num - 1];
			}
			movementHistory[0].playerPos = playerPos;
			movementHistory[0].playerRot = playerRot;
			movementHistory[0].timeStamp = timeStamp;
			movementHistory[0].anim = (PetAnimationType)_anim;
			movementHistory[0].teleported = teleported;
			if (isHistoryClear && movementHistory[movementHistory.Length - 1].timeStamp > myTime)
			{
				isHistoryClear = false;
				myTime = movementHistory[movementHistory.Length - 1].timeStamp;
				if (!isFirstHistoryFull)
				{
					thisTransform.position = movementHistory[movementHistory.Length - 1].playerPos;
					thisTransform.rotation = movementHistory[movementHistory.Length - 1].playerRot;
					isFirstHistoryFull = true;
				}
			}
		}

		private void Call_IsVisible(bool state, bool showHideEffect)
		{
			if (Defs.isMulti && isMine)
			{
				if (!state)
				{
					isHistoryClear = true;
				}
				else
				{
					isHistoryClear = true;
					isTeleported = true;
				}
				if (Defs.isInet)
				{
					base.photonView.RPC("IsVisible_RPC", PhotonTargets.Others, Convert.ToByte(state), Convert.ToByte(showHideEffect));
				}
			}
		}

		[RPC]
		[PunRPC]
		private void IsVisible_RPC(byte stateByte, byte showHideEffectByte)
		{
			if (isMine)
			{
				return;
			}
			bool num = Convert.ToBoolean(stateByte);
			bool flag = Convert.ToBoolean(showHideEffectByte);
			if (!num)
			{
				isHided = true;
				if (flag)
				{
					engine.Animator.Play(engine.GetAnimationName(PetAnimationType.Dead));
					engine.PlaySound(engine.ClipDeath);
					float animationPlayTime = engine.GetAnimationPlayTime(PetAnimationType.Dead);
					engine.DeferredAction(animationPlayTime, delegate
					{
						engine.HideModel();
						engine.Animator.Play(engine.GetAnimationName(PetAnimationType.Idle));
						engine.PlayHideEffect();
						engine.EffectHide.OnEffectCompleted.AddListener(OnEffectHideCompleted);
					});
				}
				else
				{
					engine.PlayShowEffect();
					base.transform.position = Vector3.down * 10000f;
				}
			}
			else
			{
				isHided = false;
			}
		}

		private void OnEffectHideCompleted()
		{
			engine.EffectHide.OnEffectCompleted.RemoveListener(OnEffectHideCompleted);
			base.transform.position = Vector3.down * 10000f;
		}
	}
}
