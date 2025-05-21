using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AllScenesForMode 
{
	public GameConnect.GameMode mode;

	public List<SceneInfo> avaliableScenes = new List<SceneInfo>();

	public void AddInfoScene(SceneInfo needInfo)
	{
		avaliableScenes.Add(needInfo);
	}
}
