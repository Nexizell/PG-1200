using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshLightmapDataController : MonoBehaviour
{
	public Vector4 m_lightmapScaleOffset;

	private MeshRenderer m_meshRenderer;

	[HideInInspector]
	public int m_lightmapIndex = 1;

	public Texture2D lightmap;

	private void Start()
	{
		m_meshRenderer = GetComponent<MeshRenderer>();
		LoadLmUvScale();
	}

	public void SaveLmUvScale()
	{
		m_meshRenderer = GetComponent<MeshRenderer>();
		m_lightmapScaleOffset = m_meshRenderer.lightmapScaleOffset;
		if (m_lightmapIndex == 1)
		{
			lightmap = LightmapSettings.lightmaps[0].lightmapLight;
		}
	}

	private void LoadLmUvScale()
	{
		m_meshRenderer.lightmapIndex = m_lightmapIndex;
		m_meshRenderer.lightmapScaleOffset = m_lightmapScaleOffset;
		if (m_lightmapIndex == 1 && LightmapSettings.lightmaps.Length < 2 && lightmap != null)
		{
			LightmapData[] obj = new LightmapData[2]
			{
				new LightmapData(),
				null
			};
			obj[0].lightmapLight = LightmapSettings.lightmaps[0].lightmapLight;
			obj[1] = new LightmapData();
			obj[1].lightmapLight = lightmap;
			LightmapSettings.lightmaps = obj;
			LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
		}
	}
}
