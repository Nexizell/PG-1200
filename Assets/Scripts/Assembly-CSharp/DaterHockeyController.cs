using Rilisoft.WP8;
using UnityEngine;

public class DaterHockeyController : MonoBehaviour
{
	public float coefForce = 200f;

	public int score1;

	public int score2;

	private bool isForceMyPlayer;

	private float timeSendForce = 0.3f;

	private float timerToSendForce = -1f;

	private PhotonView photonView;

	private Rigidbody thisRigidbody;

	private Transform thisTransform;

	private Vector3 resetPositionPoint;

	private bool isFirstSynhPos = true;

	private bool isResetPosition;

	private Vector3 synchPos;

	private Quaternion synchRot;

	private bool isMine;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
		thisRigidbody = GetComponent<Rigidbody>();
		thisTransform = base.transform;
		resetPositionPoint = thisTransform.position;
	}

	private void Start()
	{
		isMine = !Defs.isMulti || (Defs.isInet && photonView.isMine);
	}

	private void Update()
	{
		if (isForceMyPlayer && WeaponManager.sharedManager.myPlayer == null)
		{
			isForceMyPlayer = false;
		}
		if (isForceMyPlayer)
		{
			timerToSendForce -= Time.deltaTime;
			if (timerToSendForce < 0f)
			{
				timerToSendForce = timeSendForce;
				AddForce(Vector3.Normalize(thisTransform.position - WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.position) * coefForce);
			}
		}
		if (!isMine)
		{
			thisTransform.position = Vector3.Lerp(thisTransform.position, synchPos, Time.deltaTime * 5f);
			thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, synchRot, Time.deltaTime * 5f);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			isForceMyPlayer = true;
			return;
		}
		if (isMine && collider.gameObject.name.Equals("Gates1"))
		{
			ResetPosition();
		}
		if (isMine && collider.gameObject.name.Equals("Gates2"))
		{
			ResetPosition();
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			isForceMyPlayer = false;
		}
		if (isMine && collider.gameObject.name.Equals("Stadium"))
		{
			ResetPosition();
		}
	}

	[PunRPC]
	[RPC]
	private void AddForceRPC(Vector3 _force)
	{
		GetComponent<Rigidbody>().AddForce(_force);
	}

	private void AddForce(Vector3 _force)
	{
		if (Defs.isInet)
		{
			photonView.RPC("AddForceRPC", PhotonTargets.All, _force);
		}
		else
		{
			AddForceRPC(_force);
		}
	}

	private void ResetPosition()
	{
		thisTransform.position = resetPositionPoint;
		thisRigidbody.velocity = Vector3.zero;
		thisRigidbody.angularVelocity = Vector3.zero;
		isResetPosition = true;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(thisTransform.position);
			stream.SendNext(thisTransform.rotation);
			stream.SendNext(thisRigidbody.velocity);
			stream.SendNext(thisRigidbody.angularVelocity);
			stream.SendNext(isResetPosition);
			return;
		}
		synchPos = (Vector3)stream.ReceiveNext();
		synchRot = (Quaternion)stream.ReceiveNext();
		thisRigidbody.velocity = (Vector3)stream.ReceiveNext();
		thisRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
		isResetPosition = (bool)stream.ReceiveNext();
		if (isFirstSynhPos)
		{
			thisTransform.position = synchPos;
			thisTransform.rotation = synchRot;
			isFirstSynhPos = false;
			isResetPosition = false;
		}
	}
}
