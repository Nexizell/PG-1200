using Photon;
using UnityEngine;

public class InterolationGameObject : Photon.MonoBehaviour
{
	internal struct MovementHistoryEntry
	{
		public Vector3 playerPos;

		public Quaternion playerRot;

		public double timeStamp;
	}

	public int historyLengh = 5;

	public bool isSynchPosition;

	public bool isSynchRotation;

	public bool isLocalTrasformSynch;

	public bool syncOneAxis;

	public bool sglajEnabled;

	private Quaternion correctPlayerRot;

	private Vector3 correctPlayerPos;

	private double correctPlayerTime;

	private double myTime;

	private Transform myTransform;

	private MovementHistoryEntry[] movementHistory;

	private bool isHitoryClear = true;

	private bool isMine;

	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		myTransform = base.transform;
		movementHistory = new MovementHistoryEntry[historyLengh];
		for (int i = 0; i < historyLengh; i++)
		{
			movementHistory[i].timeStamp = 0.0;
		}
		myTime = 1.0;
	}

	private void Start()
	{
		if (Defs.isInet && base.photonView.isMine)
		{
			isMine = true;
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (isSynchPosition)
			{
				stream.SendNext(isLocalTrasformSynch ? myTransform.localPosition : myTransform.position);
			}
			if (isSynchRotation)
			{
				if (syncOneAxis)
				{
					stream.SendNext(myTransform.localRotation.eulerAngles.x);
				}
				else
				{
					stream.SendNext(isLocalTrasformSynch ? myTransform.localRotation : myTransform.rotation);
				}
			}
			stream.SendNext(PhotonNetwork.time);
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
		if (isSynchPosition)
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
		}
		if (isSynchRotation)
		{
			if (syncOneAxis)
			{
				correctPlayerRot = Quaternion.Euler((float)stream.ReceiveNext(), 0f, 0f);
			}
			else
			{
				correctPlayerRot = (Quaternion)stream.ReceiveNext();
			}
		}
		correctPlayerTime = (double)stream.ReceiveNext();
		AddNewSnapshot(correctPlayerPos, correctPlayerRot, correctPlayerTime);
	}

	private void Update()
	{
		if (isMine)
		{
			return;
		}
		if (sglajEnabled && !isHitoryClear)
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
			float t = (float)((num - myTime) / (movementHistory[num2].timeStamp - myTime));
			if (isLocalTrasformSynch)
			{
				if (isSynchPosition)
				{
					myTransform.localPosition = Vector3.Lerp(myTransform.localPosition, movementHistory[num2].playerPos, t);
				}
				if (isSynchRotation)
				{
					myTransform.localRotation = Quaternion.Lerp(myTransform.localRotation, movementHistory[num2].playerRot, t);
				}
			}
			else
			{
				if (isSynchPosition)
				{
					myTransform.position = Vector3.Lerp(myTransform.position, movementHistory[num2].playerPos, t);
				}
				if (isSynchRotation)
				{
					myTransform.rotation = Quaternion.Lerp(myTransform.rotation, movementHistory[num2].playerRot, t);
				}
			}
			myTime = num;
		}
		else
		{
			if (isHitoryClear)
			{
				return;
			}
			if (isLocalTrasformSynch)
			{
				if (isSynchPosition)
				{
					myTransform.localPosition = movementHistory[movementHistory.Length - 1].playerPos;
				}
				if (isSynchRotation)
				{
					myTransform.localRotation = movementHistory[movementHistory.Length - 1].playerRot;
				}
			}
			else
			{
				if (isSynchPosition)
				{
					myTransform.position = movementHistory[movementHistory.Length - 1].playerPos;
				}
				if (isSynchRotation)
				{
					myTransform.rotation = movementHistory[movementHistory.Length - 1].playerRot;
				}
			}
			myTime = movementHistory[movementHistory.Length - 1].timeStamp;
		}
	}

	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp)
	{
		for (int num = movementHistory.Length - 1; num > 0; num--)
		{
			movementHistory[num] = movementHistory[num - 1];
		}
		movementHistory[0].playerPos = playerPos;
		movementHistory[0].playerRot = playerRot;
		movementHistory[0].timeStamp = timeStamp;
		if (isHitoryClear && movementHistory[movementHistory.Length - 1].timeStamp > myTime)
		{
			isHitoryClear = false;
			myTime = movementHistory[movementHistory.Length - 1].timeStamp;
		}
	}
}
