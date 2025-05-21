public class AGSSyncableDeveloperString : AGSSyncable
{
	public AGSSyncableDeveloperString(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public string getCloudValue()
	{
		return null;
	}

	public string getValue()
	{
		return null;
	}

	public bool inConflict()
	{
		return false;
	}

	public bool isSet()
	{
		return false;
	}

	public void markAsResolved()
	{
	}

	public void setValue(string val)
	{
	}
}
