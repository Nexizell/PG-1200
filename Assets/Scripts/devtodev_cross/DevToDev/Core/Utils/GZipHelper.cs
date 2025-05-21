namespace DevToDev.Core.Utils
{
	internal class GZipHelper
	{
		public byte[] Pack(byte[] bs)
		{
			return GZipHelperPlatform.Pack(bs);
		}

		public string UnPack(byte[] bs)
		{
			return GZipHelperPlatform.UnPack(bs);
		}
	}
}
