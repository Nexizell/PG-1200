using DevToDev.Utils.Gzip.Zlib;

namespace DevToDev.Core.Utils
{
	public class GZipHelperPlatform
	{
		public static byte[] Pack(byte[] bs)
		{
			return GZipStream.CompressBuffer(bs);
		}

		public static string UnPack(byte[] bs)
		{
			return GZipStream.UncompressString(bs);
		}
	}
}
