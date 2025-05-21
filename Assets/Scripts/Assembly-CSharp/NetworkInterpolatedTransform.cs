using UnityEngine;

public sealed class NetworkInterpolatedTransform : MonoBehaviour
{
	private bool iskilled;

	private bool oldIsKilled;

	public Player_move_c playerMovec;

	public bool isStartAngel;

	public Vector3 correctPlayerPos;

	public Quaternion correctPlayerRot = Quaternion.identity;

	private Transform myTransform;

	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
		correctPlayerPos = new Vector3(0f, -10000f, 0f);
		myTransform = base.transform;
	}

	public void StartAngel()
	{
		isStartAngel = true;
	}

	private void Update()
	{
	}
}
