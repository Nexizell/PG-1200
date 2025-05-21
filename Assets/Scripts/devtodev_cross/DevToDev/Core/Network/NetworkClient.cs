using System;
using System.Collections;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev.Core.Network
{
	internal class NetworkClient
	{
		private static float TIMEOUT_MS = 7f;

		private Request request;

		private uint attemptsCount;

		private readonly OnRequestSend onRequestSend;

		private WWW www;

		public uint MaxAttempts { get; set; }

		public int Timeout { get; set; }

		public NetworkClient(OnRequestSend onRequestSend)
		{
			this.onRequestSend = onRequestSend;
			attemptsCount = 0u;
			MaxAttempts = NetworkConfig.DefaultMaxAttempts;
		}

		public void Send(Request request, object callbackData = null)
		{
			this.request = request;
			TryToSend(callbackData);
		}

		private void TryToSend(object callbackData)
		{
			attemptsCount++;
			Send(callbackData);
		}

		private IEnumerator WaitForTimeout(float timeout)
		{
			yield return new WaitForSeconds(timeout);
			if (www != null)
			{
				www.Dispose();
				www = null;
			}
		}

		private IEnumerator WaitForRequest(object callbackData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				while (www != null && !www.isDone)
				{
					yield return null;
				}
			}
			else
			{
				yield return www;
			}
			Response response;
			if (www == null)
			{
				Log.E("Response failure: www is null");
				response = new Response(null, 408, ResponseState.Failure);
			}
			else if (!www.responseHeaders.ContainsKey("STATUS"))
			{
				response = (UnityPlayerPlatform.isUnityWSAPlatform() ? ((www.responseHeaders.Count <= 0) ? new Response(www.text, 400, ResponseState.Failure) : new Response(www.text, 200, ResponseState.Success)) : (UnityPlayerPlatform.isWebGLPlayer() ? ((www.responseHeaders.Count <= 0) ? new Response(www.text, 400, ResponseState.Failure) : new Response(www.text, 200, ResponseState.Success)) : (UnityPlayerPlatform.isUnityWebPlatform() ? ((!string.IsNullOrEmpty(www.error)) ? new Response(www.text, 400, ResponseState.Failure) : new Response(www.text, 200, ResponseState.Success)) : ((!string.IsNullOrEmpty(www.error)) ? new Response(www.text, 400, ResponseState.Failure) : new Response(www.text, 200, ResponseState.Success)))));
			}
			else
			{
				string text = www.responseHeaders["STATUS"];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(' ');
					if (array.Length > 2)
					{
						int result = 0;
						response = ((!int.TryParse(array[1], out result)) ? new Response(www.text, 400, ResponseState.Failure) : new Response(www.text, result, ResponseState.Success));
					}
					else
					{
						response = new Response(www.text, 400, ResponseState.Failure);
					}
				}
				else
				{
					response = new Response(www.text, 400, ResponseState.Failure);
				}
			}
			if (www != null)
			{
				www.Dispose();
				www = null;
			}
			if (onRequestSend != null)
			{
				onRequestSend(response, callbackData);
			}
		}

		private void Send(object callbackData)
		{
			try
			{
				www = new WWW(request.Url, (request.PostData == null) ? null : request.PostData);
				SDKClient.Instance.AsyncOperationDispatcher.StartCoroutine(WaitForTimeout(TIMEOUT_MS));
				SDKClient.Instance.AsyncOperationDispatcher.StartCoroutine(WaitForRequest(callbackData));
			}
			catch (Exception ex)
			{
				Log.E("Error in request: " + ex.Message + "\n" + ex.StackTrace);
				Response response = new Response("", 502, ResponseState.Failure);
				if (onRequestSend != null)
				{
					onRequestSend(response, callbackData);
				}
			}
		}
	}
}
