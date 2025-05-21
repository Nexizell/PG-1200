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
	}

	public void SaveLmUvScale()
	{
	}

	private void LoadLmUvScale()
	{
	}
}
