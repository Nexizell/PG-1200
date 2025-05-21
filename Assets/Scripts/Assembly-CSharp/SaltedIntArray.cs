using Rilisoft;

public class SaltedIntArray
{
	public SaltedInt[] intArray;

	public int this[int index]
	{
		get
		{
			return intArray[index].Value;
		}
		set
		{
			intArray[index] = new SaltedInt(SaltedInt._prng.Next(), value);
		}
	}

	public int Length
	{
		get
		{
			return intArray.Length;
		}
	}

	public SaltedIntArray(int capacity)
	{
		intArray = new SaltedInt[capacity];
	}

	public SaltedIntArray(int[] defaultArray)
	{
		intArray = new SaltedInt[defaultArray.Length];
		for (int i = 0; i < defaultArray.Length; i++)
		{
			intArray[i] = new SaltedInt(SaltedInt._prng.Next(), defaultArray[i]);
		}
	}
}
