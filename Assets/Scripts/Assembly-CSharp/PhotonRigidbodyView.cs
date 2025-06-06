using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Rigidbody View")]
public class PhotonRigidbodyView : MonoBehaviour, IPunObservable
{
	[SerializeField]
	protected internal bool m_SynchronizeVelocity = true;

	[SerializeField]
	protected internal bool m_SynchronizeAngularVelocity = true;

	private Rigidbody m_Body;

	private void Awake()
	{
		m_Body = GetComponent<Rigidbody>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (m_SynchronizeVelocity)
			{
				stream.SendNext(m_Body.linearVelocity);
			}
			if (m_SynchronizeAngularVelocity)
			{
				stream.SendNext(m_Body.angularVelocity);
			}
		}
		else
		{
			if (m_SynchronizeVelocity)
			{
				m_Body.linearVelocity = (Vector3)stream.ReceiveNext();
			}
			if (m_SynchronizeAngularVelocity)
			{
				m_Body.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
	}
}
