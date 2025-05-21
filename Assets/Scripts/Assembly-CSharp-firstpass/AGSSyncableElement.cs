using System.Collections.Generic;

public class AGSSyncableElement : AGSSyncable
{
	public AGSSyncableElement(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public long GetTimestamp()
	{
		return 0L;
	}

	public Dictionary<string, string> GetMetadata()
	{
		return null;
	}
}
