using System;
using UnityEngine;

public class WebViewObject : MonoBehaviour
{
	private Action<string> callback;

	public void Init(Action<string> cb = null)
	{
		callback = cb;
	}

	private void OnDestroy()
	{
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
	}

	public void SetVisibility(bool v)
	{
	}

	public void LoadURL(string url)
	{
	}

	public void EvaluateJS(string js)
	{
	}

	public void CallFromJS(string message)
	{
		if (callback != null)
		{
			callback(message);
		}
	}

	public void goHome()
	{
	}
}
