using System;
using Photon;
using UnityEngine;

public class PlayerSynchStream : Photon.MonoBehaviour
{
	internal struct MovementHistoryEntry
	{
		public Vector3 playerPos;

		public Quaternion playerRot;

		public Quaternion weaponRot;

		public int anim;

		public bool weAreSteals;

		public double timeStamp;

		public bool onPlatform;

		public byte indexPlatform;
	}

	private bool iskilled;

	private bool oldIsKilled;

	public int sglajEnabled;

	public bool sglajEnabledVidos;

	private Vector3 correctPlayerPos;

	private double correctPlayerTime;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Quaternion correctWeaponRot = Quaternion.identity;

	public Player_move_c playerMovec;

	public bool isStartAngel;

	private Transform myTransform;

	private double myTime;

	private MovementHistoryEntry[] movementHistory;

	private int historyLengh = 5;

	private bool isHitoryClear = true;

	public int myAnim;

	private int myAnimOld;

	public SkinName skinName;

	public bool weAreSteals;

	public bool isTeleported;

	private bool isFirstSnapshot = true;

	private bool isMine;

	private bool isFirstHistoryFull;

	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		myTransform = base.transform;
		correctPlayerPos = new Vector3(0f, -10000f, 0f);
		movementHistory = new MovementHistoryEntry[historyLengh];
		for (int i = 0; i < historyLengh; i++)
		{
			movementHistory[i].timeStamp = 0.0;
		}
		myTime = 1.0;
	}

	private void Start()
	{
		if (GameConnect.isSpleef)
		{
			historyLengh = 3;
		}
		if (Defs.isInet && base.photonView.isMine)
		{
			isMine = true;
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			iskilled = playerMovec.isKilled;
			if (playerMovec.CurHealth <= 0f)
			{
				iskilled = true;
			}
			stream.SendNext(playerMovec.mySkinName.firstPersonControl.onPlatform ? myTransform.localPosition : myTransform.position);
			stream.SendNext((short)(myTransform.rotation.eulerAngles.y * 10f));
			stream.SendNext((short)(playerMovec.myTransform.localRotation.eulerAngles.x * 10f));
			stream.SendNext(PhotonNetwork.time);
			stream.SendNext((byte)myAnim);
			byte b = (byte)(Convert.ToInt32(iskilled) | (Convert.ToInt32(EffectsController.WeAreStealth) << 1) | (Convert.ToInt32(playerMovec.isImmortality) << 2) | (Convert.ToInt32(isTeleported || playerMovec.wasTimeJump) << 3) | (Convert.ToInt32(playerMovec.mySkinName.firstPersonControl.onPlatform) << 4));
			stream.SendNext(b);
			if (playerMovec.mySkinName.firstPersonControl.onPlatform)
			{
				stream.SendNext(playerMovec.mySkinName.firstPersonControl.indexPlatform);
			}
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
			correctWeaponRot = Quaternion.Euler((float)(short)stream.ReceiveNext() / 10f, 0f, 0f);
			correctPlayerTime = (double)stream.ReceiveNext();
			int anim = (byte)stream.ReceiveNext();
			byte b2 = (byte)stream.ReceiveNext();
			oldIsKilled = iskilled;
			iskilled = Convert.ToBoolean(b2 & 1);
			playerMovec.isKilled = iskilled;
			if (iskilled || Mathf.Abs((float)myTime - (float)correctPlayerTime) > 1000f)
			{
				isHitoryClear = true;
				myTime = correctPlayerTime;
			}
			bool flag = Convert.ToBoolean(b2 & 2);
			playerMovec.isImmortality = Convert.ToBoolean(b2 & 4);
			isTeleported = Convert.ToBoolean(b2 & 8);
			if (isTeleported)
			{
				isHitoryClear = true;
				myTime = correctPlayerTime;
				myTransform.position = correctPlayerPos;
				myTransform.rotation = correctPlayerRot;
			}
			bool flag2 = Convert.ToBoolean(b2 & 0x10);
			byte indexPlatform = 0;
			if (flag2)
			{
				indexPlatform = (byte)stream.ReceiveNext();
			}
			AddNewSnapshot(correctPlayerPos, correctPlayerRot, correctWeaponRot, correctPlayerTime, anim, flag, flag2, indexPlatform);
		}
		else
		{
			isFirstSnapshot = false;
		}
	}

	public void StartAngel()
	{
		isStartAngel = true;
	}

	private void Update()
	{
		if (isMine)
		{
			return;
		}
		if (!playerMovec.isWeaponSet && myTransform.position.y > -8000f)
		{
			myTransform.position = new Vector3(0f, -10000f, 0f);
			return;
		}
		if (iskilled)
		{
			if (!oldIsKilled)
			{
				oldIsKilled = iskilled;
				isStartAngel = false;
			}
			if (myTransform.position.y > -8000f)
			{
				myTransform.position = new Vector3(0f, -10000f, 0f);
			}
		}
		else if (!oldIsKilled && !isHitoryClear && (sglajEnabled > 0 || sglajEnabledVidos || playerMovec.isInvisible))
		{
			double num = ((!(myTime + (double)Time.deltaTime < movementHistory[movementHistory.Length - 1].timeStamp)) ? (myTime + (double)Time.deltaTime) : (myTime + (double)(Time.deltaTime * 1.5f)));
			int num2 = 0;
			for (int i = 0; i < movementHistory.Length && movementHistory[i].timeStamp > myTime; i++)
			{
				num2 = i;
			}
			if (num2 == 0)
			{
				isHitoryClear = true;
			}
			if (movementHistory[num2].timeStamp - myTime > 4.0 && num2 > 0)
			{
				num2--;
				myTransform.position = (movementHistory[num2].onPlatform ? PlatformsList.instance.platforms[movementHistory[num2].indexPlatform].thisTransform.position : Vector3.zero) + movementHistory[num2].playerPos;
				myTransform.rotation = movementHistory[num2].playerRot;
				playerMovec.myTransform.rotation = movementHistory[num2].weaponRot;
				myTime = movementHistory[num2].timeStamp;
			}
			else
			{
				float t = (float)((num - myTime) / (movementHistory[num2].timeStamp - myTime));
				if (movementHistory[num2].onPlatform)
				{
					myTransform.parent = PlatformsList.instance.platforms[movementHistory[num2].indexPlatform].thisTransform;
					myTransform.localPosition = Vector3.Lerp(myTransform.localPosition, movementHistory[num2].playerPos, t);
				}
				else
				{
					myTransform.parent = null;
					myTransform.position = Vector3.Lerp(myTransform.position, movementHistory[num2].playerPos, t);
				}
				if (!Device.isPixelGunLow)
				{
					myTransform.rotation = Quaternion.Lerp(myTransform.rotation, movementHistory[num2].playerRot, t);
				}
				else
				{
					myTransform.rotation = movementHistory[num2].playerRot;
				}
				if (sglajEnabledVidos)
				{
					playerMovec.myTransform.localRotation = Quaternion.Lerp(playerMovec.myTransform.localRotation, movementHistory[num2].weaponRot, t);
				}
				else
				{
					playerMovec.myTransform.localRotation = movementHistory[num2].weaponRot;
				}
				myTime = num;
				if (myAnim != movementHistory[num2].anim)
				{
					skinName.SetAnim(movementHistory[num2].anim, movementHistory[num2].weAreSteals);
					myAnim = movementHistory[num2].anim;
				}
			}
		}
		else if (!isHitoryClear)
		{
			myTransform.position = (movementHistory[movementHistory.Length - 1].onPlatform ? PlatformsList.instance.platforms[movementHistory[movementHistory.Length - 1].indexPlatform].thisTransform.position : Vector3.zero) + movementHistory[movementHistory.Length - 1].playerPos;
			myTransform.rotation = movementHistory[movementHistory.Length - 1].playerRot;
			playerMovec.myTransform.localRotation = movementHistory[movementHistory.Length - 1].weaponRot;
			myTime = movementHistory[movementHistory.Length - 1].timeStamp;
		}
		if (isStartAngel && myTransform.position.y > -8000f)
		{
			myTransform.position = new Vector3(0f, -10000f, 0f);
		}
	}

	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, Quaternion weaponRot, double timeStamp, int _anim, bool _weAreSteals, bool _onPlatform, byte _indexPlatform)
	{
		for (int num = movementHistory.Length - 1; num > 0; num--)
		{
			movementHistory[num] = movementHistory[num - 1];
		}
		movementHistory[0].playerPos = playerPos;
		movementHistory[0].playerRot = playerRot;
		movementHistory[0].weaponRot = weaponRot;
		movementHistory[0].timeStamp = timeStamp;
		movementHistory[0].anim = _anim;
		movementHistory[0].weAreSteals = _weAreSteals;
		movementHistory[0].onPlatform = _onPlatform;
		movementHistory[0].indexPlatform = _indexPlatform;
		if (isHitoryClear && movementHistory[movementHistory.Length - 1].timeStamp > myTime)
		{
			isHitoryClear = false;
			myTime = movementHistory[movementHistory.Length - 1].timeStamp;
			if (!isFirstHistoryFull)
			{
				myTransform.position = (movementHistory[movementHistory.Length - 1].onPlatform ? PlatformsList.instance.platforms[movementHistory[movementHistory.Length - 1].indexPlatform].thisTransform.position : Vector3.zero) + movementHistory[movementHistory.Length - 1].playerPos;
				myTransform.rotation = movementHistory[movementHistory.Length - 1].playerRot;
				playerMovec.myTransform.rotation = movementHistory[movementHistory.Length - 1].weaponRot;
				isFirstHistoryFull = true;
			}
		}
	}
}
