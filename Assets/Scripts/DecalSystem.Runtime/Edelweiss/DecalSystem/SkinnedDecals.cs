using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class SkinnedDecals : GenericDecals<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
	{
		private List<SkinnedDecalsMeshRenderer> m_SkinnedDecalsMeshRenderers = new List<SkinnedDecalsMeshRenderer>();

		public override bool CastShadows
		{
			get
			{
				return SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.castShadows;
			}
			set
			{
				SkinnedDecalsMeshRenderer[] skinnedDecalsMeshRenderers = SkinnedDecalsMeshRenderers;
				foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in skinnedDecalsMeshRenderers)
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.castShadows = value;
				}
			}
		}

		public override bool ReceiveShadows
		{
			get
			{
				return SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.receiveShadows;
			}
			set
			{
				SkinnedDecalsMeshRenderer[] skinnedDecalsMeshRenderers = SkinnedDecalsMeshRenderers;
				foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in skinnedDecalsMeshRenderers)
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.receiveShadows = value;
				}
			}
		}

		public override bool UseLightProbes
		{
			get
			{
				return SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.useLightProbes;
			}
			set
			{
				SkinnedDecalsMeshRenderer[] skinnedDecalsMeshRenderers = SkinnedDecalsMeshRenderers;
				foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in skinnedDecalsMeshRenderers)
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.useLightProbes = value;
				}
			}
		}

		public override Transform LightProbeAnchor
		{
			get
			{
				return SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.probeAnchor;
			}
			set
			{
				SkinnedDecalsMeshRenderer[] skinnedDecalsMeshRenderers = SkinnedDecalsMeshRenderers;
				foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in skinnedDecalsMeshRenderers)
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.probeAnchor = value;
				}
			}
		}

		public SkinnedDecalsMeshRenderer[] SkinnedDecalsMeshRenderers
		{
			get
			{
				return m_SkinnedDecalsMeshRenderers.ToArray();
			}
		}

		public bool IsSkinnedDecalsMeshRenderer(SkinnedMeshRenderer a_SkinnedMeshRenderer)
		{
			bool result = false;
			foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in m_SkinnedDecalsMeshRenderers)
			{
				if (a_SkinnedMeshRenderer == skinnedDecalsMeshRenderer.SkinnedMeshRenderer)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		protected override void ApplyMaterialToMeshRenderers()
		{
			foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in m_SkinnedDecalsMeshRenderers)
			{
				if (Application.isPlaying)
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.material = base.CurrentMaterial;
				}
				else
				{
					skinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMaterial = base.CurrentMaterial;
				}
			}
		}

		public override void OnEnable()
		{
			InitializeDecalsMeshRenderers();
			if (m_SkinnedDecalsMeshRenderers.Count == 0)
			{
				PushSkinnedDecalsMeshRenderer();
			}
		}

		public override void InitializeDecalsMeshRenderers()
		{
			m_SkinnedDecalsMeshRenderers.Clear();
			Transform cachedTransform = base.CachedTransform;
			for (int i = 0; i < cachedTransform.childCount; i++)
			{
				Transform child = cachedTransform.GetChild(i);
				SkinnedDecalsMeshRenderer component = child.GetComponent<SkinnedDecalsMeshRenderer>();
				if (component != null)
				{
					m_SkinnedDecalsMeshRenderers.Add(component);
				}
			}
		}

		public void UpdateSkinnedDecalsMeshes(SkinnedDecalsMesh a_SkinnedDecalsMesh)
		{
			if (a_SkinnedDecalsMesh.Vertices.Count <= 65535)
			{
				if (m_SkinnedDecalsMeshRenderers.Count == 0)
				{
					PushSkinnedDecalsMeshRenderer();
				}
				else if (m_SkinnedDecalsMeshRenderers.Count > 1)
				{
					while (m_SkinnedDecalsMeshRenderers.Count > 1)
					{
						PopSkinnedDecalsMeshRenderer();
					}
				}
				SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer = m_SkinnedDecalsMeshRenderers[0];
				ApplyToSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer, a_SkinnedDecalsMesh);
			}
			else
			{
				int num = 0;
				for (int i = 0; i < a_SkinnedDecalsMesh.Projectors.Count; i++)
				{
					GenericDecalProjectorBase a_FirstProjector = a_SkinnedDecalsMesh.Projectors[i];
					GenericDecalProjectorBase a_LastProjector = a_SkinnedDecalsMesh.Projectors[i];
					if (num >= m_SkinnedDecalsMeshRenderers.Count)
					{
						PushSkinnedDecalsMeshRenderer();
					}
					SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer2 = m_SkinnedDecalsMeshRenderers[num];
					int num2 = 0;
					int num3 = i;
					GenericDecalProjectorBase genericDecalProjectorBase = a_SkinnedDecalsMesh.Projectors[i];
					while (i < a_SkinnedDecalsMesh.Projectors.Count && num2 + genericDecalProjectorBase.DecalsMeshVertexCount <= 65535)
					{
						a_LastProjector = genericDecalProjectorBase;
						num2 += genericDecalProjectorBase.DecalsMeshVertexCount;
						i++;
						if (i < a_SkinnedDecalsMesh.Projectors.Count)
						{
							genericDecalProjectorBase = a_SkinnedDecalsMesh.Projectors[i];
						}
					}
					if (num3 != i)
					{
						ApplyToSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer2, a_SkinnedDecalsMesh, a_FirstProjector, a_LastProjector);
						num++;
					}
				}
				while (num + 1 < m_SkinnedDecalsMeshRenderers.Count)
				{
					PopSkinnedDecalsMeshRenderer();
				}
			}
			SetDecalsMeshesAreNotOptimized();
		}

		private void PushSkinnedDecalsMeshRenderer()
		{
			GameObject gameObject = new GameObject("Decals Mesh Renderer");
			Transform transform = gameObject.transform;
			transform.parent = base.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer = AddSkinnedDecalsMeshRendererComponentToGameObject(gameObject);
			skinnedDecalsMeshRenderer.SkinnedMeshRenderer.material = base.CurrentMaterial;
			m_SkinnedDecalsMeshRenderers.Add(skinnedDecalsMeshRenderer);
		}

		private void PopSkinnedDecalsMeshRenderer()
		{
			SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer = m_SkinnedDecalsMeshRenderers[m_SkinnedDecalsMeshRenderers.Count - 1];
			if (Application.isPlaying)
			{
				Object.Destroy(skinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh);
				Object.Destroy(skinnedDecalsMeshRenderer.gameObject);
			}
			m_SkinnedDecalsMeshRenderers.RemoveAt(m_SkinnedDecalsMeshRenderers.Count - 1);
		}

		private void ApplyToSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer, SkinnedDecalsMesh a_SkinnedDecalsMesh)
		{
			Mesh mesh = MeshOfSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			if (a_SkinnedDecalsMesh.OriginalVertices.Count == 0)
			{
				mesh.vertices = new Vector3[1];
				if (base.CurrentNormalsMode != 0)
				{
					mesh.normals = new Vector3[1];
				}
				if (base.CurrentTangentsMode != 0)
				{
					mesh.tangents = new Vector4[1];
				}
				mesh.uv = new Vector2[1];
				if (base.CurrentUV2Mode != 0)
				{
					mesh.uv2 = new Vector2[1];
				}
				mesh.boneWeights = new BoneWeight[1];
				mesh.bindposes = new Matrix4x4[1];
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = new Transform[1] { a_SkinnedDecalsMeshRenderer.transform };
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = false;
			}
			else
			{
				mesh.vertices = a_SkinnedDecalsMesh.OriginalVertices.ToArray();
				if (base.CurrentNormalsMode != 0)
				{
					mesh.normals = a_SkinnedDecalsMesh.Normals.ToArray();
				}
				if (base.CurrentTangentsMode != 0)
				{
					mesh.tangents = a_SkinnedDecalsMesh.Tangents.ToArray();
				}
				mesh.uv = a_SkinnedDecalsMesh.UVs.ToArray();
				if (base.CurrentUV2Mode != 0)
				{
					mesh.uv2 = a_SkinnedDecalsMesh.UV2s.ToArray();
				}
				mesh.boneWeights = a_SkinnedDecalsMesh.BoneWeights.ToArray();
				mesh.triangles = a_SkinnedDecalsMesh.Triangles.ToArray();
				mesh.bindposes = a_SkinnedDecalsMesh.BindPoses.ToArray();
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = a_SkinnedDecalsMesh.Bones.ToArray();
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = true;
			}
		}

		private void ApplyToSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer, SkinnedDecalsMesh a_SkinnedDecalsMesh, GenericDecalProjectorBase a_FirstProjector, GenericDecalProjectorBase a_LastProjector)
		{
			int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
			int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
			int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
			Mesh mesh = MeshOfSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			Vector3[] a_Array = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array, a_SkinnedDecalsMesh.OriginalVertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.vertices = a_Array;
			BoneWeight[] a_Array2 = new BoneWeight[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array2, a_SkinnedDecalsMesh.BoneWeights, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.boneWeights = a_Array2;
			int[] a_Array3 = new int[decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex + 1];
			CopyListRangeToArray(ref a_Array3, a_SkinnedDecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);
			for (int i = 0; i < a_Array3.Length; i++)
			{
				a_Array3[i] -= decalsMeshLowerVertexIndex;
			}
			mesh.triangles = a_Array3;
			Vector2[] a_Array4 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array4, a_SkinnedDecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.uv = a_Array4;
			if (base.CurrentUV2Mode != 0 && base.CurrentUV2Mode != UV2Mode.Lightmapping)
			{
				Vector2[] a_Array5 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array5, a_SkinnedDecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.uv2 = a_Array5;
			}
			if (base.CurrentNormalsMode != 0)
			{
				Vector3[] a_Array6 = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array6, a_SkinnedDecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.normals = a_Array6;
			}
			if (base.CurrentTangentsMode != 0)
			{
				Vector4[] a_Array7 = new Vector4[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array7, a_SkinnedDecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.tangents = a_Array7;
			}
			Matrix4x4[] bindposes = a_SkinnedDecalsMesh.BindPoses.ToArray();
			mesh.bindposes = bindposes;
			Transform[] bones = a_SkinnedDecalsMesh.Bones.ToArray();
			a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = bones;
			a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
			a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = true;
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

		private Mesh MeshOfSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer)
		{
			Mesh mesh;
			if (a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh == null)
			{
				mesh = new Mesh();
				mesh.name = "Skinned Decal Mesh";
				a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh = mesh;
			}
			else
			{
				mesh = a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh;
				mesh.Clear();
			}
			return mesh;
		}

		public override void OptimizeDecalsMeshes()
		{
			base.OptimizeDecalsMeshes();
			foreach (SkinnedDecalsMeshRenderer skinnedDecalsMeshRenderer in m_SkinnedDecalsMeshRenderers)
			{
				if (skinnedDecalsMeshRenderer.SkinnedMeshRenderer != null && skinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh != null)
				{
					var o_352_5_638672877514323878 = skinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh;
				}
			}
		}

		protected abstract SkinnedDecalsMeshRenderer AddSkinnedDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
	}
}
