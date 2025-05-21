using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class Decals : GenericDecals<Decals, DecalProjectorBase, DecalsMesh>
	{
		private List<DecalsMeshRenderer> m_DecalsMeshRenderers = new List<DecalsMeshRenderer>();

		public override bool CastShadows
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.castShadows;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.castShadows = value;
				}
			}
		}

		public override bool ReceiveShadows
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.receiveShadows;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.receiveShadows = value;
				}
			}
		}

		public override bool UseLightProbes
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.useLightProbes;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.useLightProbes = value;
				}
			}
		}

		public override Transform LightProbeAnchor
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.probeAnchor;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.probeAnchor = value;
				}
			}
		}

		public DecalsMeshRenderer[] DecalsMeshRenderers
		{
			get
			{
				return m_DecalsMeshRenderers.ToArray();
			}
		}

		public bool IsDecalsMeshRenderer(MeshRenderer a_MeshRenderer)
		{
			bool result = false;
			foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers)
			{
				if (a_MeshRenderer == decalsMeshRenderer.MeshRenderer)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		protected override void ApplyMaterialToMeshRenderers()
		{
			foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers)
			{
				decalsMeshRenderer.MeshRenderer.material = base.CurrentMaterial;
			}
		}

		public override void OnEnable()
		{
			InitializeDecalsMeshRenderers();
			if (m_DecalsMeshRenderers.Count == 0)
			{
				PushDecalsMeshRenderer();
			}
		}

		public override void InitializeDecalsMeshRenderers()
		{
			m_DecalsMeshRenderers.Clear();
			Transform cachedTransform = base.CachedTransform;
			for (int i = 0; i < cachedTransform.childCount; i++)
			{
				Transform child = cachedTransform.GetChild(i);
				DecalsMeshRenderer component = child.GetComponent<DecalsMeshRenderer>();
				if (component != null)
				{
					m_DecalsMeshRenderers.Add(component);
				}
			}
		}

		public void UpdateDecalsMeshes(DecalsMesh a_DecalsMesh)
		{
			if (a_DecalsMesh.Vertices.Count <= 65535)
			{
				if (m_DecalsMeshRenderers.Count == 0)
				{
					PushDecalsMeshRenderer();
				}
				else if (m_DecalsMeshRenderers.Count > 1)
				{
					while (m_DecalsMeshRenderers.Count > 1)
					{
						PopDecalsMeshRenderer();
					}
				}
				DecalsMeshRenderer a_DecalsMeshRenderer = m_DecalsMeshRenderers[0];
				ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer, a_DecalsMesh);
			}
			else
			{
				int num = 0;
				for (int i = 0; i < a_DecalsMesh.Projectors.Count; i++)
				{
					GenericDecalProjectorBase a_FirstProjector = a_DecalsMesh.Projectors[i];
					GenericDecalProjectorBase a_LastProjector = a_DecalsMesh.Projectors[i];
					if (num >= m_DecalsMeshRenderers.Count)
					{
						PushDecalsMeshRenderer();
					}
					DecalsMeshRenderer a_DecalsMeshRenderer2 = m_DecalsMeshRenderers[num];
					int num2 = 0;
					int num3 = i;
					GenericDecalProjectorBase genericDecalProjectorBase = a_DecalsMesh.Projectors[i];
					while (i < a_DecalsMesh.Projectors.Count && num2 + genericDecalProjectorBase.DecalsMeshVertexCount <= 65535)
					{
						a_LastProjector = genericDecalProjectorBase;
						num2 += genericDecalProjectorBase.DecalsMeshVertexCount;
						i++;
						if (i < a_DecalsMesh.Projectors.Count)
						{
							genericDecalProjectorBase = a_DecalsMesh.Projectors[i];
						}
					}
					if (num3 != i)
					{
						ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer2, a_DecalsMesh, a_FirstProjector, a_LastProjector);
						num++;
					}
				}
				while (num + 1 < m_DecalsMeshRenderers.Count)
				{
					PopDecalsMeshRenderer();
				}
			}
			SetDecalsMeshesAreNotOptimized();
		}

		private void PushDecalsMeshRenderer()
		{
			GameObject gameObject = new GameObject("Decals Mesh Renderer");
			Transform transform = gameObject.transform;
			transform.parent = base.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			DecalsMeshRenderer decalsMeshRenderer = AddDecalsMeshRendererComponentToGameObject(gameObject);
			decalsMeshRenderer.MeshRenderer.material = base.CurrentMaterial;
			m_DecalsMeshRenderers.Add(decalsMeshRenderer);
		}

		private void PopDecalsMeshRenderer()
		{
			DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers[m_DecalsMeshRenderers.Count - 1];
			if (Application.isPlaying)
			{
				Object.Destroy(decalsMeshRenderer.MeshFilter.mesh);
				Object.Destroy(decalsMeshRenderer.gameObject);
			}
			m_DecalsMeshRenderers.RemoveAt(m_DecalsMeshRenderers.Count - 1);
		}

		private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh)
		{
			Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			mesh.vertices = a_DecalsMesh.Vertices.ToArray();
			if (base.CurrentNormalsMode != 0)
			{
				mesh.normals = a_DecalsMesh.Normals.ToArray();
			}
			if (base.CurrentTangentsMode != 0)
			{
				mesh.tangents = a_DecalsMesh.Tangents.ToArray();
			}
			mesh.uv = a_DecalsMesh.UVs.ToArray();
			if (base.CurrentUV2Mode != 0)
			{
				mesh.uv2 = a_DecalsMesh.UV2s.ToArray();
			}
			mesh.triangles = a_DecalsMesh.Triangles.ToArray();
		}

		private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh, GenericDecalProjectorBase a_FirstProjector, GenericDecalProjectorBase a_LastProjector)
		{
			int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
			int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
			int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
			Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			Vector3[] a_Array = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array, a_DecalsMesh.Vertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.vertices = a_Array;
			int[] a_Array2 = new int[decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex + 1];
			CopyListRangeToArray(ref a_Array2, a_DecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);
			for (int i = 0; i < a_Array2.Length; i++)
			{
				a_Array2[i] -= decalsMeshLowerVertexIndex;
			}
			mesh.triangles = a_Array2;
			Vector2[] a_Array3 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array3, a_DecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.uv = a_Array3;
			if (base.CurrentUV2Mode != 0 && base.CurrentUV2Mode != UV2Mode.Lightmapping)
			{
				Vector2[] a_Array4 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array4, a_DecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.uv2 = a_Array4;
			}
			if (base.CurrentNormalsMode != 0)
			{
				Vector3[] a_Array5 = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array5, a_DecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.normals = a_Array5;
			}
			if (base.CurrentTangentsMode != 0)
			{
				Vector4[] a_Array6 = new Vector4[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array6, a_DecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.tangents = a_Array6;
			}
		}

		private static void CopyListRangeToArray<T>(ref T[] a_Array, List<T> a_List, int a_LowerListIndex, int a_UpperListIndex)
		{
			int num = 0;
			for (int i = a_LowerListIndex; i <= a_UpperListIndex; i++)
			{
				a_Array[num] = a_List[i];
				num++;
			}
		}

		private Mesh MeshOfDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer)
		{
			Mesh mesh;
			if (Application.isPlaying)
			{
				if (a_DecalsMeshRenderer.MeshFilter.mesh == null)
				{
					mesh = new Mesh();
					mesh.name = "Decal Mesh";
					a_DecalsMeshRenderer.MeshFilter.mesh = mesh;
				}
				else
				{
					mesh = a_DecalsMeshRenderer.MeshFilter.mesh;
					mesh.Clear();
				}
			}
			else if (a_DecalsMeshRenderer.MeshFilter.sharedMesh == null)
			{
				mesh = new Mesh();
				mesh.name = "Decal Mesh";
				a_DecalsMeshRenderer.MeshFilter.sharedMesh = mesh;
			}
			else
			{
				mesh = a_DecalsMeshRenderer.MeshFilter.sharedMesh;
				mesh.Clear();
			}
			return mesh;
		}

		public override void OptimizeDecalsMeshes()
		{
			base.OptimizeDecalsMeshes();
			foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers)
			{
				if (Application.isPlaying)
				{
					if (decalsMeshRenderer.MeshFilter != null && decalsMeshRenderer.MeshFilter.mesh != null)
					{
						var o_322_6_638672877513154334 = decalsMeshRenderer.MeshFilter.mesh;
					}
				}
				else if (decalsMeshRenderer.MeshFilter != null && decalsMeshRenderer.MeshFilter.sharedMesh != null)
				{
					var o_327_5_638672877513384693 = decalsMeshRenderer.MeshFilter.sharedMesh;
				}
			}
		}

		protected abstract DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
	}
}
