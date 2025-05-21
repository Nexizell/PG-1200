using UnityEngine;

public class visibleObjPhoton : MonoBehaviour
{
	public PlayerSynchStream lerpScript;

	public bool isVisible;

	private void Awake()
	{
		if (!Defs.isMulti || !Defs.isInet)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
	}

	private void OnBecameVisible()
	{
		isVisible = true;
		if (lerpScript != null)
		{
			lerpScript.sglajEnabled++;
		}
	}

	private void OnBecameInvisible()
	{
		isVisible = false;
		if (lerpScript != null)
		{
			lerpScript.sglajEnabled--;
		}
	}
}
