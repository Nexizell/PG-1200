using System;

namespace Rilisoft
{
	public class SaltedFloatNew
	{
		private byte[] _value;

		private float[] floatBuffer = new float[1];

		public float value
		{
			get
			{
				Buffer.BlockCopy(_value, 0, floatBuffer, 0, floatBuffer.Length);
				return floatBuffer[0];
			}
			set
			{
				floatBuffer[0] = value;
				Buffer.BlockCopy(floatBuffer, 0, _value, 0, _value.Length);
			}
		}

		public SaltedFloatNew(float value)
		{
			_value = new byte[4];
			this.value = value;
		}

		public SaltedFloatNew()
			: this(0f)
		{
		}
	}
}
