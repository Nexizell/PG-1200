using Rilisoft.WP8;
using UnityEngine;

public sealed class PixelView : MonoBehaviour
{
	private static int viewIdCount = 1000;

	private PhotonView photonView;

	private int localViewID = -1;

	public int viewID
	{
		get
		{
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					return photonView.viewID;
				}
				return localViewID;
			}
			return 0;
		}
	}

	private void Awake()
	{
		if (Defs.isMulti && Defs.isInet)
		{
			photonView = GetComponent<PhotonView>();
			if (photonView == null)
			{
				Debug.LogError("GetComponent<PhotonView>() == null");
			}
		}
	}

	
	private void SendViewID(int id)
	{
		if (localViewID != -1)
		{
			Debug.LogError("Local id is already set! " + localViewID + " (new: " + id + ")");
		}
		localViewID = id;
	}
}
