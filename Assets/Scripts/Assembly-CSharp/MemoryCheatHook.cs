using UnityEngine;

public class MemoryCheatHook
{
	private static int[] refNumbers;

	private static float[] refNumbersFloat;

	private static float checkTime = 0f;

	private static int checkProgress = 0;

	private static int step = 25;

	private static bool initialized = false;

	public static void Initialize()
	{
		if (!initialized)
		{
			refNumbers = new int[1000];
			for (int i = 0; i < refNumbers.Length; i++)
			{
				refNumbers[i] = i;
			}
			refNumbersFloat = new float[1000];
			for (int j = 0; j < refNumbersFloat.Length; j++)
			{
				refNumbersFloat[j] = j;
			}
			initialized = true;
		}
	}

	public static bool CheckForCheating()
	{
		if (!initialized)
		{
			Initialize();
		}
		if (checkTime > 0f)
		{
			checkTime -= Time.unscaledDeltaTime;
			return false;
		}
		int num = checkProgress * step;
		for (int i = num; i < num + step; i++)
		{
			if (!refNumbers[i].Equals(i))
			{
				return true;
			}
		}
		for (int j = num; j < num + step; j++)
		{
			if (!refNumbersFloat[j].AlmostEquals(j, 0.1f))
			{
				return true;
			}
		}
		checkProgress++;
		if (checkProgress * step >= refNumbers.Length)
		{
			checkTime = 2f;
			checkProgress = 0;
		}
		return false;
	}

	public static void Test()
	{
		refNumbersFloat[Random.Range(0, refNumbersFloat.Length)] = Random.Range(0f, 1000f);
	}
}
