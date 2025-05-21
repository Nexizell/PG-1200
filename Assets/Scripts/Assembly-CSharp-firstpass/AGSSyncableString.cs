using System.Collections.Generic;

public class AGSSyncableString : AGSSyncableStringElement
{
	public AGSSyncableString(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public void Set(string val)
	{
	}

	public void Set(string val, Dictionary<string, string> metadata)
	{
	}

	public bool IsSet()
	{
		return false;
	}
}
