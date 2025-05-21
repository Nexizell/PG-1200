using UnityEngine;

public class SpeedrunTrackPart : MonoBehaviour
{
	public enum PartType
	{
		Easy = 0,
		Hard = 1,
		Reward = 2,
		Tutorial = 3
	}

	public Transform endPoint;

	public Transform coinSpawnPoint;

	public int group;

	public PartType type;

	[HideInInspector]
	public int poolIndex;

	private void Awake()
	{
	}

	private void Update()
	{
	}
}
