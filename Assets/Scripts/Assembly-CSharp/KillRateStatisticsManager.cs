using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

public class KillRateStatisticsManager
{
	private static Dictionary<string, Dictionary<int, int>> weKillOld = new Dictionary<string, Dictionary<int, int>>();

	private static Dictionary<string, Dictionary<int, int>> weWereKilledOld = new Dictionary<string, Dictionary<int, int>>();

	private static bool _initialized = false;

	public const string WeKillKey = "wekill";

	public const string WeWereKilledKey = "wewerekilled";

	public static Dictionary<string, Dictionary<int, int>> WeKillOld
	{
		get
		{
			if (!_initialized)
			{
				Initialize();
			}
			return weKillOld;
		}
	}

	public static Dictionary<string, Dictionary<int, int>> WeWereKilledOld
	{
		get
		{
			if (!_initialized)
			{
				Initialize();
			}
			return weWereKilledOld;
		}
	}

	private static void Initialize()
	{
		ParseKillRate(ref weKillOld, ref weWereKilledOld);
		_initialized = true;
	}

	private static void ParseKillRate(ref Dictionary<string, Dictionary<int, int>> returnWeKill, ref Dictionary<string, Dictionary<int, int>> returnWeWereKilled)
	{
		InitializeKillRateKey();
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("KillRateKeyStatistics")) as Dictionary<string, object>;
		if (dictionary.ContainsKey("version"))
		{
			if (!((string)dictionary["version"]).Equals(GlobalGameController.AppVersion))
			{
				WriteDefaultJson();
				dictionary = Json.Deserialize(Storager.getString("KillRateKeyStatistics")) as Dictionary<string, object>;
			}
		}
		else
		{
			Debug.LogError("ParseKillRate: no version key. Please clear your PlayerPrefs");
			WriteDefaultJson();
			dictionary = Json.Deserialize(Storager.getString("KillRateKeyStatistics")) as Dictionary<string, object>;
		}
		Dictionary<string, object> arg = (dictionary.ContainsKey("wekill") ? (dictionary["wekill"] as Dictionary<string, object>) : new Dictionary<string, object>());
		Dictionary<string, object> arg2 = (dictionary.ContainsKey("wewerekilled") ? (dictionary["wewerekilled"] as Dictionary<string, object>) : new Dictionary<string, object>());
		Action<Dictionary<string, object>, Dictionary<string, Dictionary<int, int>>> obj = delegate(Dictionary<string, object> savedDict, Dictionary<string, Dictionary<int, int>> dict)
		{
			foreach (KeyValuePair<string, object> item in savedDict)
			{
				Dictionary<string, object> obj2 = item.Value as Dictionary<string, object>;
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				foreach (KeyValuePair<string, object> item2 in obj2)
				{
					dictionary2.Add(int.Parse(item2.Key), (int)(long)item2.Value);
				}
				dict.Add(item.Key, dictionary2);
			}
		};
		obj(arg, returnWeKill);
		obj(arg2, returnWeWereKilled);
	}

	private static void WriteDefaultJson()
	{
		Storager.setString("KillRateKeyStatistics", Json.Serialize(new Dictionary<string, object> { 
		{
			"version",
			GlobalGameController.AppVersion
		} }));
	}

	private static void InitializeKillRateKey()
	{
		if (!Storager.hasKey("KillRateKeyStatistics"))
		{
			WriteDefaultJson();
		}
	}
}
