using System;

public class AmazonJavaWrapper : IDisposable
{
	public object getJavaObject()
	{
		return null;
	}

	public IntPtr GetRawObject()
	{
		return (IntPtr)0;
	}

	public IntPtr GetRawClass()
	{
		return (IntPtr)0;
	}

	public void Set<FieldType>(string fieldName, FieldType type)
	{
	}

	public FieldType Get<FieldType>(string fieldName)
	{
		return default(FieldType);
	}

	public void SetStatic<FieldType>(string fieldName, FieldType type)
	{
	}

	public FieldType GetStatic<FieldType>(string fieldName)
	{
		return default(FieldType);
	}

	public void CallStatic(string method, params object[] args)
	{
	}

	public void Call(string method, params object[] args)
	{
	}

	public ReturnType CallStatic<ReturnType>(string method, params object[] args)
	{
		return default(ReturnType);
	}

	public ReturnType Call<ReturnType>(string method, params object[] args)
	{
		return default(ReturnType);
	}

	public void Dispose()
	{
	}
}
