using System;
using System.Collections.Generic;
using UnityEngine;

public class LightMapUvSaver : MonoBehaviour
{
	[Serializable]
	public class MeshRenderWithName 
	{
		public string name;

		public Vector4 uvTiling;

		public string parentName = "";

		public int siblingIndex;

		public int lmIndex;
	}

	public List<MeshRenderWithName> meshRenderersToSave;

	private List<MeshRenderer> meshRenders = new List<MeshRenderer>();

	public Texture2D[] oldLightmaps;

	[ContextMenu("Save LM UV")]
	private void SaveLightMapsUV()
	{
		if (meshRenderersToSave.Count > 0)
		{
			meshRenderersToSave.Clear();
		}
		if (meshRenders.Count > 0)
		{
			meshRenders.Clear();
		}
		meshRenders.AddRange(UnityEngine.Object.FindObjectsOfType<MeshRenderer>());
		foreach (MeshRenderer meshRender in meshRenders)
		{
			if (meshRender.lightmapScaleOffset != new Vector4(1f, 1f, 0f, 0f))
			{
				if (meshRender.transform.parent != null)
				{
					meshRenderersToSave.Add(new MeshRenderWithName
					{
						parentName = meshRender.transform.parent.gameObject.name,
						siblingIndex = meshRender.transform.GetSiblingIndex(),
						name = meshRender.gameObject.name,
						uvTiling = meshRender.lightmapScaleOffset,
						lmIndex = meshRender.lightmapIndex
					});
				}
				else
				{
					meshRenderersToSave.Add(new MeshRenderWithName
					{
						siblingIndex = meshRender.transform.GetSiblingIndex(),
						name = meshRender.gameObject.name,
						uvTiling = meshRender.lightmapScaleOffset
					});
				}
			}
		}
	}

	[ContextMenu("Load LM UV")]
	private void LoadLightMapUV()
	{
		LightmapData[] array = new LightmapData[oldLightmaps.Length];
		for (int i = 0; i < oldLightmaps.Length; i++)
		{
			array[i] = new LightmapData();
			array[i].lightmapLight = oldLightmaps[i];
		}
		LightmapSettings.lightmaps = array;
		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
		foreach (MeshRenderWithName item in meshRenderersToSave)
		{
			MeshRenderer meshRenderer = FindRendererByNameAndIndex(item.name, item.siblingIndex, item.parentName);
			meshRenderer.lightmapScaleOffset = item.uvTiling;
			meshRenderer.lightmapIndex = item.lmIndex;
		}
	}

	private MeshRenderer FindRendererByNameAndIndex(string name, int siblingIndex, string parentName)
	{
		MeshRenderer[] array = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
		List<MeshRenderer> list = new List<MeshRenderer>();
		if (parentName == "")
		{
			MeshRenderer[] array2 = array;
			foreach (MeshRenderer meshRenderer in array2)
			{
				if (meshRenderer.gameObject.name == name && meshRenderer.transform.GetSiblingIndex() == siblingIndex)
				{
					list.Add(meshRenderer);
				}
			}
		}
		else
		{
			MeshRenderer[] array2 = array;
			foreach (MeshRenderer meshRenderer2 in array2)
			{
				if (meshRenderer2.gameObject.name == name && meshRenderer2.transform.GetSiblingIndex() == siblingIndex && meshRenderer2.transform.parent.gameObject.name == parentName)
				{
					list.Add(meshRenderer2);
				}
			}
		}
		return list[0];
	}
}
