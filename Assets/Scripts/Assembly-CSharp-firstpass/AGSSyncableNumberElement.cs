public class AGSSyncableNumberElement : AGSSyncableElement
{
	public AGSSyncableNumberElement(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public long AsLong()
	{
		return 0L;
	}

	public double AsDouble()
	{
		return 0.0;
	}

	public int AsInt()
	{
		return 0;
	}

	public string AsString()
	{
		return null;
	}
}
