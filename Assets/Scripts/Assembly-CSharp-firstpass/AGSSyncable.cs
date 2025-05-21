using System;
using System.Collections.Generic;

public class AGSSyncable : IDisposable
{
	public enum SyncableMethod
	{
		getDeveloperString = 0,
		getHighestNumber = 1,
		getLowestNumber = 2,
		getLatestNumber = 3,
		getHighNumberList = 4,
		getLowNumberList = 5,
		getLatestNumberList = 6,
		getAccumulatingNumber = 7,
		getLatestString = 8,
		getLatestStringList = 9,
		getStringSet = 10,
		getMap = 11
	}

	public enum HashSetMethod
	{
		getDeveloperStringKeys = 0,
		getHighestNumberKeys = 1,
		getLowestNumberKeys = 2,
		getLatestNumberKeys = 3,
		getHighNumberListKeys = 4,
		getLowNumberListKeys = 5,
		getLatestNumberListKeys = 6,
		getAccumulatingNumberKeys = 7,
		getLatestStringKeys = 8,
		getLatestStringListKeys = 9,
		getStringSetKeys = 10,
		getMapKeys = 11
	}

	protected AmazonJavaWrapper javaObject;

	public AGSSyncable(AmazonJavaWrapper jo)
	{
		javaObject = jo;
	}

	public void Dispose()
	{
		if (javaObject != null)
		{
			javaObject.Dispose();
		}
	}

	protected T GetAGSSyncable<T>(SyncableMethod method)
	{
		return GetAGSSyncable<T>(method, null);
	}

	protected T GetAGSSyncable<T>(SyncableMethod method, string key)
	{
		return default(T);
	}

	protected HashSet<string> GetHashSet(HashSetMethod method)
	{
		return null;
	}
}
