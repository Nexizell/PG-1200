using System.Text;

namespace DevToDev.Core.Utils
{
	internal class MD5Helper
	{
		internal byte[] ComputeHash(byte[] bs)
		{
			return new MD5Managed().ComputeHash(bs);
		}

		public static string GetMd5(byte[] bs)
		{
			if (bs != null)
			{
				MD5Helper mD5Helper = new MD5Helper();
				byte[] array = mD5Helper.ComputeHash(bs);
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array2 = array;
				foreach (byte b in array2)
				{
					stringBuilder.Append(b.ToString("x2").ToLower());
				}
				return stringBuilder.ToString();
			}
			return "";
		}
	}
}
