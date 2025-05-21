using UnityEngine;

public class TestShader : MonoBehaviour
{
	public Material matBlock;

	public Material killBlock;

	public float timerToNext;

	public float timeToNext = 50f;

	public float timerToBlink;

	public float timeToBlink = 2f;

	public Color[] Colors;

	public Color colorNow;

	public bool add;

	private int nextColor = 1;

	private int currentColor;

	private void Update()
	{
		ChangeColor();
		GridBlink();
	}

	private void ChangeColor()
	{
		timerToNext += Time.deltaTime;
		if (timerToNext >= timeToNext)
		{
			currentColor = nextColor;
			nextColor++;
			if (nextColor >= Colors.Length)
			{
				nextColor = 0;
			}
			timerToNext = 0f;
		}
		colorNow = Color.Lerp(Colors[currentColor], Colors[nextColor], timerToNext / timeToNext);
		matBlock.SetColor("_Color2", colorNow);
	}

	private void GridBlink()
	{
		if (timerToBlink > timeToBlink)
		{
			add = false;
		}
		if (timerToBlink < 0f)
		{
			add = true;
		}
		if (add)
		{
			timerToBlink += Time.deltaTime;
		}
		else
		{
			timerToBlink -= Time.deltaTime;
		}
		matBlock.SetFloat("_Shadow", 0.3f + timerToBlink / timeToBlink * 0.7f);
		killBlock.SetFloat("_Shadow", 0.3f + timerToBlink / timeToBlink * 0.7f);
	}
}
