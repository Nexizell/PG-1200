using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
	public PlayerCheckpoint back;

	public PlayerCheckpoint next;

	public bool isSavingCheckpoint;

	public GameObject checkPointRespawnPoint;

	public GameObject canSaveParticle;

	public GameObject savedParticle;

	[HideInInspector]
	public bool savedInCheckpoint;

	private Color alphaColor = new Color(1f, 1f, 1f, 0.5f);

	private Color alphaColorGreen = new Color(0.5f, 1f, 0.5f, 0.5f);

	private Color alphaColorBlue = new Color(0.5f, 0.5f, 1f, 0.5f);

	[HideInInspector]
	public float distance;

	private void Awake()
	{
		if (savedParticle != null)
		{
			savedParticle.SetActive(false);
		}
		if (isSavingCheckpoint && canSaveParticle != null)
		{
			canSaveParticle.SetActive(true);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
		{
			CheckpointController.instance.RegisterInCheckpoint(this, true);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
		{
			CheckpointController.instance.RegisterInCheckpoint(this, false);
		}
	}

	public void SetCheckpointSaved(bool saved, bool showSaved)
	{
		savedInCheckpoint = saved;
		if (canSaveParticle != null)
		{
			canSaveParticle.SetActive(!saved);
		}
		if (savedParticle != null)
		{
			savedParticle.SetActive(saved && showSaved);
		}
	}
}
