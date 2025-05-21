namespace Rilisoft
{
	public class SaltedFloatEps
	{
		private static float[] saltValues = new float[10] { 139.01f, -7789f, 0.134f, 88.22f, -9.1783f, 2355.1f, -1919.87f, 98.02f, 21.13f, -512.7f };

		private float origValue;

		private int saltIndex;

		private float saltedValue;

		public float value
		{
			get
			{
				float num = saltedValue + saltValues[saltIndex];
				if (Abs(origValue - num) > 0.1f)
				{
					origValue = num;
					return num;
				}
				return origValue;
			}
			set
			{
				saltIndex++;
				if (saltIndex >= saltValues.Length)
				{
					saltIndex = 0;
				}
				origValue = value;
				saltedValue = value - saltValues[saltIndex];
			}
		}

		public SaltedFloatEps(float value)
		{
			this.value = value;
		}

		public SaltedFloatEps()
			: this(0f)
		{
		}

		private float Abs(float value)
		{
			if (value > 0f)
			{
				return value;
			}
			return value * -1f;
		}
	}
}
