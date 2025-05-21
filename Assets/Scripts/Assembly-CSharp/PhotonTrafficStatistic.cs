using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public class PhotonTrafficStatistic
{
	public static List<Dictionary<string, object>> incomingRPC = new List<Dictionary<string, object>>();

	public static List<Dictionary<string, object>> outcomingRPC = new List<Dictionary<string, object>>();

	private static DateTime timeStartStat;

	private static string nameKey = "name";

	private static string lenKey = "len";

	private static string countKey = "count";

	public static void AddIncomingRPC(string methodName, object[] parameters)
	{
		if (incomingRPC.Count == 0 && outcomingRPC.Count == 0)
		{
			timeStartStat = DateTime.Now;
		}
		long sizeObjects = Tools.GetSizeObjects(parameters);
		bool flag = false;
		foreach (Dictionary<string, object> item in incomingRPC)
		{
			if (item[nameKey].ToString() == methodName)
			{
				item[lenKey] = Convert.ToInt32(item[lenKey]) + sizeObjects;
				item[countKey] = Convert.ToInt32(item[countKey]) + 1;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(nameKey, methodName);
			dictionary.Add(lenKey, sizeObjects);
			dictionary.Add(countKey, 1);
			incomingRPC.Add(dictionary);
		}
	}

	public static void AddOutcomingRPC(string methodName, object[] parameters)
	{
		if (incomingRPC.Count == 0 && outcomingRPC.Count == 0)
		{
			timeStartStat = DateTime.Now;
		}
		long sizeObjects = Tools.GetSizeObjects(parameters);
		bool flag = false;
		foreach (Dictionary<string, object> item in outcomingRPC)
		{
			if (item[nameKey].ToString() == methodName)
			{
				item[lenKey] = Convert.ToInt32(item[lenKey]) + sizeObjects;
				item[countKey] = Convert.ToInt32(item[countKey]) + 1;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(nameKey, methodName);
			dictionary.Add(lenKey, sizeObjects);
			dictionary.Add(countKey, 1);
			outcomingRPC.Add(dictionary);
		}
	}

	public static void ResetAndLogStatistic()
	{
		double totalSeconds = (DateTime.Now - timeStartStat).TotalSeconds;
		string text = "Time:\t" + totalSeconds + "\n\n";
		text += "Out Coming\t \n";
		int num = 0;
		int num2 = 0;
		new List<Dictionary<int, int>>().OrderBy((Dictionary<int, int> i) => i[1]);
		outcomingRPC = outcomingRPC.OrderByDescending((Dictionary<string, object> i) => Convert.ToInt32(i[lenKey])).ToList();
		incomingRPC = incomingRPC.OrderByDescending((Dictionary<string, object> i) => Convert.ToInt32(i[lenKey])).ToList();
		foreach (Dictionary<string, object> item in outcomingRPC)
		{
			text = text + item[nameKey].ToString() + "\t" + item[lenKey].ToString() + "\t" + item[countKey].ToString() + "\n";
			num2 += Convert.ToInt32(item[lenKey]);
		}
		text = text + "Summ traffic:\t" + num2 + "\n\nIn Coming\n";
		foreach (Dictionary<string, object> item2 in incomingRPC)
		{
			text = text + item2[nameKey].ToString() + "\t" + item2[lenKey].ToString() + "\t" + item2[countKey].ToString() + "\n";
			num = Convert.ToInt32(item2[lenKey]);
		}
		text = text + "Summ traffic:\t" + num + "\nIn Coming\n";
		Debug.Log(text);
		UniPasteBoard.SetClipBoardString(text);
		timeStartStat = DateTime.Now;
		incomingRPC.Clear();
		outcomingRPC.Clear();
	}
}
