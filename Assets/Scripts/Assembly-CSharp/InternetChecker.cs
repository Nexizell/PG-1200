using System;
using System.IO;
using System.Net;
using UnityEngine;

public sealed class InternetChecker : MonoBehaviour
{
	public static bool InternetAvailable;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void CheckForInternetConn()
	{
		string htmlFromUri = GetHtmlFromUri("http://google.com");
		if (htmlFromUri == "")
		{
			InternetAvailable = false;
		}
		else if (!htmlFromUri.Contains("schema.org/WebPage"))
		{
			InternetAvailable = false;
		}
		else
		{
			InternetAvailable = true;
		}
	}

	public static string GetHtmlFromUri(string resource)
	{
		string text = string.Empty;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult))
			{
				if (httpWebResponse.StatusCode < (HttpStatusCode)299 && httpWebResponse.StatusCode >= HttpStatusCode.OK)
				{
					Debug.Log("Trying to check internet");
					using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
					{
						char[] array = new char[80];
						streamReader.Read(array, 0, array.Length);
						char[] array2 = array;
						foreach (char c in array2)
						{
							text += c;
						}
					}
				}
			}
		}
		catch
		{
			return string.Empty;
		}
		return text;
	}
}
