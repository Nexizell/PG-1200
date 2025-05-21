using System;

public class Region
{
	public CloudRegionCode Code;

	public string HostAndPort;

	public int Ping;

	public static CloudRegionCode Parse(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionCode result = CloudRegionCode.none;
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			result = (CloudRegionCode)Enum.Parse(typeof(CloudRegionCode), codeAsString);
		}
		return result;
	}

	internal static CloudRegionFlag ParseFlag(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionFlag result = (CloudRegionFlag)0;
		if (Enum.IsDefined(typeof(CloudRegionFlag), codeAsString))
		{
			result = (CloudRegionFlag)Enum.Parse(typeof(CloudRegionFlag), codeAsString);
		}
		return result;
	}

	public override string ToString()
	{
		return string.Format("'{0}' \t{1}ms \t{2}", new object[3] { Code, Ping, HostAndPort });
	}
}
