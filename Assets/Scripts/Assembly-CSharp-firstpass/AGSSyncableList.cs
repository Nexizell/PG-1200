using System.Collections.Generic;

public class AGSSyncableList : AGSSyncable
{
	public AGSSyncableList(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public void SetMaxSize(int size)
	{
	}

	public int GetMaxSize()
	{
		return 0;
	}

	public bool IsSet()
	{
		return false;
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
	}

	public void Add(string val)
	{
	}
}
