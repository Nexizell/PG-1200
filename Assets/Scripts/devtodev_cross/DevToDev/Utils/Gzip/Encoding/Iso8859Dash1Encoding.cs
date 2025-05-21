using System;
using System.Text;

namespace DevToDev.Utils.Gzip.Encoding
{
	internal class Iso8859Dash1Encoding : System.Text.Encoding
	{
		public override string WebName
		{
			get
			{
				return "iso-8859-1";
			}
		}

		public static int CharacterCount
		{
			get
			{
				return 256;
			}
		}

		public override int GetBytes(char[] chars, int start, int count, byte[] bytes, int byteIndex)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", "null array");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "null array");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (chars.Length - start < count)
			{
				throw new ArgumentOutOfRangeException("chars");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			for (int i = 0; i < count; i++)
			{
				char c = chars[start + i];
				if (c >= 'ÿ')
				{
					bytes[byteIndex + i] = 63;
				}
				else
				{
					bytes[byteIndex + i] = (byte)c;
				}
			}
			return count;
		}

		public override int GetChars(byte[] bytes, int start, int count, char[] chars, int charIndex)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", "null array");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "null array");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (bytes.Length - start < count)
			{
				throw new ArgumentOutOfRangeException("bytes");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			for (int i = 0; i < count; i++)
			{
				chars[charIndex + i] = (char)bytes[i + start];
			}
			return count;
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count;
		}

		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
