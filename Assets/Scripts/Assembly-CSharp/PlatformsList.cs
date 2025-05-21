using UnityEngine;

[ExecuteInEditMode]
public class PlatformsList : MonoBehaviour
{
	public static PlatformsList instance;

	[ReadOnly]
	public PlatformController[] platforms;

	private void Start()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = this;
	}

	private void Update()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			platforms = base.gameObject.transform.GetComponentsInChildren<PlatformController>();
			for (byte b = 0; b < platforms.Length; b++)
			{
				platforms[b].index = b;
			}
		}
	}
}
