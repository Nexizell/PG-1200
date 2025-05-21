using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class TrafficForwardingScript : MonoBehaviour
{
	public EventHandler<TrafficForwardingInfo> Updated;

	private float _trafficForwardingConfigTimestamp;

	private TaskCompletionSource<TrafficForwardingInfo> _trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();

	internal IEnumerator GetTrafficForwardingConfigLoopCoroutine()
	{
		yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
		float seconds = Math.Max(60f, Defs.timeUpdatePixelbookInfo - 60f);
		yield return new WaitForSeconds(seconds);
		yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
		float delaySeconds = Defs.timeUpdatePixelbookInfo;
		while (true)
		{
			if (Time.realtimeSinceStartup - _trafficForwardingConfigTimestamp < delaySeconds)
			{
				yield return null;
			}
			else
			{
				yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
			}
		}
	}

	internal IEnumerator GetTrafficForwardingConfigCoroutine()
	{
		_trafficForwardingConfigTimestamp = Time.realtimeSinceStartup;
		if (((Task)_trafficForwardingPromise.Task).IsCompleted)
		{
			_trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TrafficForwardingConfigUrl);
		yield return response;
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			string message = ((response == null) ? "null" : response.error);
			_trafficForwardingPromise.TrySetException((Exception)new InvalidOperationException(message));
			yield break;
		}
		string text = URLs.Sanitize(response);
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		object value;
		if (dictionary == null)
		{
			Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			_trafficForwardingPromise.TrySetException((Exception)new InvalidOperationException("Couldnot deserialize response: " + text));
		}
		else if (dictionary.TryGetValue("trafficForwarding_v_10.2.0", out value))
		{
			Dictionary<string, object> dictionary2 = value as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				object value2;
				if (dictionary2.TryGetValue("url", out value2))
				{
					string text2 = Convert.ToString(value2);
					text2 = text2 + "&uid=" + FriendsController.sharedController.id + "&device=" + SystemInfo.deviceUniqueIdentifier;
					int minLevel = 0;
					try
					{
						minLevel = Convert.ToInt32(dictionary2["minLevel"]);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogWarning(ex.ToString());
					}
					int maxLevel = 36;
					try
					{
						maxLevel = Convert.ToInt32(dictionary2["maxLevel"]);
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogWarning(ex2.ToString());
					}
					TrafficForwardingInfo result = new TrafficForwardingInfo(text2, minLevel, maxLevel);
					Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
					{
						handler(this, result);
					});
					_trafficForwardingPromise.TrySetResult(result);
				}
				else
				{
					Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
					{
						handler(this, TrafficForwardingInfo.DisabledInstance);
					});
					_trafficForwardingPromise.TrySetResult(TrafficForwardingInfo.DisabledInstance);
				}
			}
			else
			{
				Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
				{
					handler(this, TrafficForwardingInfo.DisabledInstance);
				});
				_trafficForwardingPromise.TrySetException((Exception)new InvalidOperationException("Couldnot deserialize trafficForwarding node: " + Json.Serialize(value)));
			}
		}
		else
		{
			Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			_trafficForwardingPromise.TrySetException((Exception)new InvalidOperationException("Response doesn't contain trafficForwarding node."));
		}
	}

	internal Task<TrafficForwardingInfo> GetTrafficForwardingInfo()
	{
		return _trafficForwardingPromise.Task;
	}
}
