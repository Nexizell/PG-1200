using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ResourceProfiler
	{
		private readonly IDictionary<Material, MaterialInfo> _materials = new Dictionary<Material, MaterialInfo>();

		private readonly IDictionary<Mesh, MeshInfo> _meshes = new Dictionary<Mesh, MeshInfo>();

		public void Update()
		{
			Renderer[] array = Object.FindObjectsOfType<Renderer>();
			foreach (Renderer renderer in array)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				foreach (Material key in sharedMaterials)
				{
					MaterialInfo value = null;
					if (!_materials.TryGetValue(key, out value))
					{
						value = new MaterialInfo();
						_materials.Add(key, value);
					}
					value.AddRenderer(renderer);
				}
			}
			MeshFilter[] array2 = Object.FindObjectsOfType<MeshFilter>();
			foreach (MeshFilter meshFilter in array2)
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (sharedMesh != null)
				{
					MeshInfo value2 = null;
					if (!_meshes.TryGetValue(sharedMesh, out value2))
					{
						value2 = new MeshInfo();
						_meshes.Add(sharedMesh, value2);
					}
					value2.AddMeshFilter(meshFilter);
				}
			}
		}
	}
}
