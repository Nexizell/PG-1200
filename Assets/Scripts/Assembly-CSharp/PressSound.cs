using UnityEngine;

public class PressSound : MonoBehaviour
{
	public AudioSource source;

	private float lastYPosition;

	private bool pressPlayed;

	private void PlaySound()
	{
		pressPlayed = true;
		source.Play();
	}

	private void Update()
	{
		float y = base.transform.position.y;
		if (pressPlayed)
		{
			if (lastYPosition > y)
			{
				pressPlayed = false;
			}
		}
		else if (lastYPosition < y)
		{
			PlaySound();
		}
		lastYPosition = y;
	}
}
