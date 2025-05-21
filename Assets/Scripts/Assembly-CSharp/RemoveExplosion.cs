using UnityEngine;

public sealed class RemoveExplosion : MonoBehaviour
{
	private void Start()
	{
		if (GetComponent<ParticleSystem>() != null)
		{
			float duration = GetComponent<ParticleSystem>().duration;
		}
		if ((bool)GetComponent<AudioSource>() && GetComponent<AudioSource>().enabled && Defs.isSoundFX)
		{
			GetComponent<AudioSource>().Play();
		}
		Invoke("Remove", 7f);
	}

	private void Remove()
	{
		Object.Destroy(base.gameObject);
	}
}
