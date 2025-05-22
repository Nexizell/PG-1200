using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public sealed class HighAssetsLoader : MonoBehaviour
{
	public static readonly string LightmapsFolder = "Lightmap";

	public static readonly string HighFolder = "High";

	public static readonly string AtlasFolder = "Atlas";

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnLevelWasLoaded(int lev)
	{
		/*if (Device.isWeakDevice || (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && !Application.isEditor))
		{
			return;
		}
		LoadHighLightmap();
		Texture2D[] array = Resources.LoadAll<Texture2D>(ResPath.Combine(ResPath.Combine(AtlasFolder, HighFolder), Application.loadedLevelName));
		string value = Application.loadedLevelName + "_Atlas";
		if (array == null || array.Length == 0)
		{
			return;
		}
		Object[] array2 = Object.FindObjectsOfType(typeof(Renderer));
		List<Material> list = new List<Material>();
		Object[] array3 = array2;
		for (int i = 0; i < array3.Length; i++)
		{
			Renderer renderer = (Renderer)array3[i];
			if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.name != null && renderer.sharedMaterial.name.Contains(value) && !list.Contains(renderer.sharedMaterial))
			{
				list.Add(renderer.sharedMaterial);
			}
		}
		List<Texture2D> list2 = new List<Texture2D>();
		Texture2D[] array4 = array;
		foreach (Texture2D item in array4)
		{
			list2.Add(item);
		}
		list.Sort((Material m1, Material m2) => m1.name.CompareTo(m2.name));
		list2.Sort((Texture2D a1, Texture2D a2) => a1.name.CompareTo(a2.name));
		for (int j = 0; j < Mathf.Min(list.Count, list2.Count); j++)
		{
			list[j].mainTexture = list2[j];
		}*/
	}

	public static void LoadHighLightmap()
	{
		/*List<Texture2D> list = (Resources.LoadAll<Texture2D>(ResPath.Combine(ResPath.Combine(LightmapsFolder, HighFolder), Application.loadedLevelName)) ?? new Texture2D[0]).ToList();
		list.Sort((Texture2D lightmap1, Texture2D lightmap2) => lightmap1.name.CompareTo(lightmap2.name));
		if (list.Count > 0)
		{
			SetCurrentLightmap(list[0]);
		}*/
	}

	public static void SetCurrentLightmap(Texture2D lightmap)
	{
		/*LightmapSettings.lightmaps = new LightmapData[1]
		{
			new LightmapData
			{
				lightmapColor = lightmap
			}
		};*/
	}
}
