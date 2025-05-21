using System.Collections.Generic;

public class AGSSyncableStringSet : AGSSyncable
{
	public AGSSyncableStringSet(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public void Add(string val)
	{
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
	}

	public AGSSyncableStringElement Get(string val)
	{
		return GetAGSSyncable<AGSSyncableStringElement>(SyncableMethod.getStringSet, val);
	}

	public bool Contains(string val)
	{
		return false;
	}

	public bool IsSet()
	{
		return false;
	}

	public HashSet<AGSSyncableStringElement> GetValues()
	{
		return null;
	}
}
