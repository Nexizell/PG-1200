using System;
using System.Collections;
using UnityEngine;

public sealed class SkinsManagerPixlGun : MonoBehaviour
{
	public Hashtable skins = new Hashtable();

	public static SkinsManagerPixlGun sharedManager;

	private void OnLevelWasLoaded(int idx)
	{
		if (skins.Count > 0)
		{
			skins.Clear();
		}
		string path;
		if (Defs.isMulti && GameConnect.isCOOP && !GameConnect.isCompany)
		{
			path = "EnemySkins/COOP/";
		}
		else
		{
			if (Defs.isMulti || GameConnect.isCOOP || GameConnect.isCompany)
			{
				return;
			}
			path = ((!GameConnect.isSurvival) ? ("EnemySkins/Level" + (TrainingController.TrainingCompleted ? CurrentCampaignGame.currentLevel.ToString() : "3")) : Defs.SurvSkinsPath);
		}
		UnityEngine.Object[] array = Resources.LoadAll(path);
		try
		{
			UnityEngine.Object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Texture texture = (Texture)array2[i];
				skins.Add(texture.name, texture);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Exception in SkinsManagerPixlGun: " + ex);
		}
	}

	private void Start()
	{
		sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		sharedManager = null;
	}
}
