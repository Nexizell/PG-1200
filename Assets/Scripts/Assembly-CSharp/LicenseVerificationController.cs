using System;

internal static class LicenseVerificationController
{
	internal struct PackageInfo
	{
		internal string PackageName { get; set; }

		internal string SignatureHash { get; set; }
	}

	[Flags]
	internal enum PackageInfoFlags
	{
		GetSignatures = 0x40
	}

	internal static PackageInfo GetPackageInfo()
	{
		PackageInfo result = default(PackageInfo);
		result.PackageName = string.Empty;
		result.SignatureHash = string.Empty;
		return result;
	}
}
