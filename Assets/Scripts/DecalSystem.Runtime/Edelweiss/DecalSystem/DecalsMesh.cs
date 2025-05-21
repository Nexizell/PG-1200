using System;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public class DecalsMesh : GenericDecalsMesh<Decals, DecalProjectorBase, DecalsMesh>
	{
		private AddMeshDelegate m_AddMeshDelegate;

		public DecalsMesh()
		{
		}

		public DecalsMesh(Decals a_Decals)
		{
			m_Decals = a_Decals;
		}

		internal override void InitializeDelegates()
		{
			bool flag = m_Decals.CurrentTangentsMode == TangentsMode.Target;
			bool flag2 = m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2;
			bool flag3 = m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2;
			if (!flag && !flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormals;
				m_AddMeshDelegate = AddOptimizedVerticesNormals;
			}
			else if (!flag && !flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsUV2s;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsUV2s;
			}
			else if (!flag && flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsUVs;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsTangentsUVs;
			}
			else if (!flag && flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsUVsUV2s;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsUVsUV2s;
			}
			else if (flag && !flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsTangents;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsTangents;
			}
			else if (flag && !flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsTangentsUV2s;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsTangentsUV2s;
			}
			else if (flag && flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsTangentsUVs;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsTangentsUVs;
			}
			else if (flag && flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveOptimizedVerticesNormalsTangentsUVsUV2s;
				m_AddMeshDelegate = AddOptimizedVerticesNormalsTangentsUVsUV2s;
			}
		}

		public void Add(Mesh a_Mesh, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
		{
			DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			if (activeDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
			}
			if (m_Decals.CurrentUVMode == UVMode.Project && (0 > activeDecalProjector.UV1RectangleIndex || activeDecalProjector.UV1RectangleIndex >= m_Decals.uvRectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.Project && (0 > activeDecalProjector.UV2RectangleIndex || activeDecalProjector.UV2RectangleIndex >= m_Decals.uv2Rectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
			}
			if (a_Mesh == null)
			{
				throw new ArgumentNullException("The mesh parameter is not allowed to be null!");
			}
			Vector3[] vertices = a_Mesh.vertices;
			Vector3[] normals = a_Mesh.normals;
			Vector2[] array = null;
			Vector2[] array2 = null;
			Vector4[] array3 = null;
			int[] triangles = a_Mesh.triangles;
			if (triangles == null)
			{
				throw new NullReferenceException("The triangles in the mesh are null!");
			}
			if (vertices == null)
			{
				return;
			}
			bool flag = false;
			if (normals == null || normals.Length == 0)
			{
				flag = true;
				a_Mesh.RecalculateNormals();
				normals = a_Mesh.normals;
			}
			else if (vertices.Length != normals.Length)
			{
				throw new FormatException("The number of vertices in the mesh does not match the number of normals!");
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				array3 = a_Mesh.tangents;
				if (array3 == null)
				{
					throw new NullReferenceException("The tangents in the mesh are null!");
				}
				if (vertices.Length != array3.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of tangents!");
				}
			}
			if (m_Decals.CurrentUVMode == UVMode.TargetUV)
			{
				array = a_Mesh.uv;
				if (array == null)
				{
					throw new NullReferenceException("The uv's in the mesh are null!");
				}
				if (vertices.Length != array.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				array = a_Mesh.uv2;
				if (array == null)
				{
					throw new NullReferenceException("The uv2's in the mesh are null!");
				}
				if (vertices.Length != array.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
				}
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV)
			{
				array2 = a_Mesh.uv;
				if (array2 == null)
				{
					throw new NullReferenceException("The uv's in the mesh are null!");
				}
				if (vertices.Length != array2.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				array2 = a_Mesh.uv2;
				if (array2 == null)
				{
					throw new NullReferenceException("The uv2's in the mesh are null!");
				}
				if (vertices.Length != array2.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
				}
			}
			Add(vertices, normals, array3, array, array2, triangles, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
			if (flag)
			{
				a_Mesh.normals = null;
			}
		}

		public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, int[] a_Triangles, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
		{
			DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			if (activeDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
			}
			if (m_Decals.CurrentUVMode == UVMode.Project && (0 > activeDecalProjector.UV1RectangleIndex || activeDecalProjector.UV1RectangleIndex >= m_Decals.uvRectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.Project && (0 > activeDecalProjector.UV2RectangleIndex || activeDecalProjector.UV2RectangleIndex >= m_Decals.uv2Rectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
			}
			if (a_Vertices == null)
			{
				throw new ArgumentNullException("The vertices parameter is not allowed to be null!");
			}
			if (a_Normals == null)
			{
				throw new ArgumentNullException("The normals parameter is not allowed to be null!");
			}
			if (a_Triangles == null)
			{
				throw new ArgumentNullException("The triangles parameter is not allowed to be null!");
			}
			if (a_Vertices.Length != a_Normals.Length)
			{
				throw new FormatException("The number of vertices does not match the number of normals!");
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				if (a_Tangents == null)
				{
					throw new ArgumentNullException("The tangents parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_Tangents.Length)
				{
					throw new FormatException("The number of vertices does not match the number of tangents!");
				}
			}
			if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				if (a_UVs == null)
				{
					throw new ArgumentNullException("The uv parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_UVs.Length)
				{
					throw new FormatException("The number of vertices does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				if (a_UV2s == null)
				{
					throw new NullReferenceException("The uv2 parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_UV2s.Length)
				{
					throw new FormatException("The number of vertices does not match the number of uv2's!");
				}
			}
			Vector3 v = new Vector3(0f, 1f, 0f);
			Matrix4x4 worldToLocalMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
			v = (activeDecalProjector.ProjectorToWorldMatrix * worldToLocalMatrix).inverse.transpose.MultiplyVector(v).normalized;
			Matrix4x4 a_MeshToDecalsMatrix = worldToLocalMatrix * a_MeshToWorldMatrix;
			Matrix4x4 transpose = a_MeshToDecalsMatrix.inverse.transpose;
			float a_CullingDotProduct = Mathf.Cos(activeDecalProjector.CullingAngle * ((float)Math.PI / 180f));
			int count = m_Vertices.Count;
			m_AddMeshDelegate(a_Vertices, a_Normals, a_Tangents, a_UVs, a_UV2s, v, a_CullingDotProduct, a_MeshToDecalsMatrix, transpose);
			float num = Determinant(a_WorldToMeshMatrix);
			if (!(num < 0f))
			{
				for (int i = 0; i < a_Triangles.Length; i += 3)
				{
					int a_Index = a_Triangles[i];
					int a_Index2 = a_Triangles[i + 1];
					int a_Index3 = a_Triangles[i + 2];
					if (!m_RemovedIndices.IsRemovedIndex(a_Index) && !m_RemovedIndices.IsRemovedIndex(a_Index2) && !m_RemovedIndices.IsRemovedIndex(a_Index3))
					{
						a_Index = count + m_RemovedIndices.AdjustedIndex(a_Index);
						a_Index2 = count + m_RemovedIndices.AdjustedIndex(a_Index2);
						a_Index3 = count + m_RemovedIndices.AdjustedIndex(a_Index3);
						m_Triangles.Add(a_Index);
						m_Triangles.Add(a_Index2);
						m_Triangles.Add(a_Index3);
					}
				}
			}
			else
			{
				Debug.Log(Mathf.Approximately(num, 0f));
				Debug.Log(Determinant(a_WorldToMeshMatrix));
				Debug.Log(num + ", " + a_WorldToMeshMatrix[0, 0] + ", " + a_WorldToMeshMatrix[1, 1] + ", " + a_WorldToMeshMatrix[2, 2]);
				for (int j = 0; j < a_Triangles.Length; j += 3)
				{
					int a_Index4 = a_Triangles[j];
					int a_Index5 = a_Triangles[j + 1];
					int a_Index6 = a_Triangles[j + 2];
					if (!m_RemovedIndices.IsRemovedIndex(a_Index4) && !m_RemovedIndices.IsRemovedIndex(a_Index5) && !m_RemovedIndices.IsRemovedIndex(a_Index6))
					{
						a_Index4 = count + m_RemovedIndices.AdjustedIndex(a_Index4);
						a_Index5 = count + m_RemovedIndices.AdjustedIndex(a_Index5);
						a_Index6 = count + m_RemovedIndices.AdjustedIndex(a_Index6);
						m_Triangles.Add(a_Index5);
						m_Triangles.Add(a_Index4);
						m_Triangles.Add(a_Index6);
					}
				}
			}
			m_RemovedIndices.Clear();
			activeDecalProjector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
			activeDecalProjector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
			activeDecalProjector.IsTangentProjectionCalculated = false;
			activeDecalProjector.IsUV1ProjectionCalculated = false;
			activeDecalProjector.IsUV2ProjectionCalculated = false;
		}

		private float Determinant(Matrix4x4 a_Matrix)
		{
			return a_Matrix.m00 * (a_Matrix.m11 * (a_Matrix.m22 * a_Matrix.m33 - a_Matrix.m32 * a_Matrix.m23) - a_Matrix.m12 * (a_Matrix.m21 * a_Matrix.m33 - a_Matrix.m31 * a_Matrix.m23) + a_Matrix.m13 * (a_Matrix.m21 * a_Matrix.m32 - a_Matrix.m31 * a_Matrix.m22)) - a_Matrix.m01 * (a_Matrix.m10 * (a_Matrix.m22 * a_Matrix.m33 - a_Matrix.m32 * a_Matrix.m23) - a_Matrix.m12 * (a_Matrix.m20 * a_Matrix.m33 - a_Matrix.m30 * a_Matrix.m23) + a_Matrix.m13 * (a_Matrix.m20 * a_Matrix.m32 - a_Matrix.m30 * a_Matrix.m22)) + a_Matrix.m02 * (a_Matrix.m10 * (a_Matrix.m21 * a_Matrix.m33 - a_Matrix.m31 * a_Matrix.m23) - a_Matrix.m11 * (a_Matrix.m20 * a_Matrix.m33 - a_Matrix.m30 * a_Matrix.m23) + a_Matrix.m13 * (a_Matrix.m20 * a_Matrix.m31 - a_Matrix.m30 * a_Matrix.m21)) - a_Matrix.m03 * (a_Matrix.m10 * (a_Matrix.m21 * a_Matrix.m32 - a_Matrix.m31 * a_Matrix.m22) - a_Matrix.m11 * (a_Matrix.m20 * a_Matrix.m32 - a_Matrix.m30 * a_Matrix.m22) + a_Matrix.m12 * (a_Matrix.m20 * a_Matrix.m31 - a_Matrix.m30 * a_Matrix.m21));
		}

		private void UnoptimizedAdd(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
				{
					Vector4 item2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
					m_Tangents.Add(item2);
				}
				if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
				{
					m_UVs.Add(a_UVs[i]);
				}
				if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
				{
					m_UV2s.Add(a_UV2s[i]);
				}
			}
		}

		private void AddOptimizedVerticesNormals(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
			}
		}

		private void AddOptimizedVerticesNormalsTangents(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector4 item2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_Tangents.Add(item2);
			}
		}

		private void AddOptimizedVerticesNormalsUVs(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_UVs.Add(a_UVs[i]);
			}
		}

		private void AddOptimizedVerticesNormalsTangentsUVs(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector4 item2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_Tangents.Add(item2);
				m_UVs.Add(a_UVs[i]);
			}
		}

		private void AddOptimizedVerticesNormalsUV2s(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_UV2s.Add(a_UV2s[i]);
			}
		}

		private void AddOptimizedVerticesNormalsTangentsUV2s(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector4 item2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_Tangents.Add(item2);
				m_UV2s.Add(a_UV2s[i]);
			}
		}

		private void AddOptimizedVerticesNormalsUVsUV2s(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_UVs.Add(a_UVs[i]);
				m_UV2s.Add(a_UV2s[i]);
			}
		}

		private void AddOptimizedVerticesNormalsTangentsUVsUV2s(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector4 item2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
				m_Vertices.Add(item);
				m_Normals.Add(normalized);
				m_Tangents.Add(item2);
				m_UVs.Add(a_UVs[i]);
				m_UV2s.Add(a_UV2s[i]);
			}
		}

		public void Add(Terrain a_Terrain, Matrix4x4 a_TerrainToDecalsMatrix)
		{
			DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			if (activeDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
			}
			if (m_Decals.CurrentUVMode == UVMode.Project && (0 > activeDecalProjector.UV1RectangleIndex || activeDecalProjector.UV1RectangleIndex >= m_Decals.uvRectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.Project && (0 > activeDecalProjector.UV2RectangleIndex || activeDecalProjector.UV2RectangleIndex >= m_Decals.uv2Rectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
			}
			if (a_Terrain == null)
			{
				throw new ArgumentNullException("The terrain parameter is not allowed to be null!");
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				throw new InvalidOperationException("Terrains don't have tangents!");
			}
			if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				throw new InvalidOperationException("Terrains don't have uv's!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				throw new InvalidOperationException("Terrains don't have uv2's!");
			}
			Bounds bounds = activeDecalProjector.WorldBounds();
			bounds.center -= a_Terrain.transform.position;
			TerrainData terrainData = a_Terrain.terrainData;
			Matrix4x4 transpose = a_TerrainToDecalsMatrix.inverse.transpose;
			if (!(terrainData != null))
			{
				return;
			}
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			if (min.x > max.x)
			{
				float x = min.x;
				min.x = max.x;
				max.x = x;
			}
			if (min.z > max.z)
			{
				float z = min.z;
				min.z = max.z;
				max.z = z;
			}
			Vector3 size = terrainData.size;
			min.x = Mathf.Clamp(min.x, 0f, size.x);
			max.x = Mathf.Clamp(max.x, 0f, size.x);
			min.z = Mathf.Clamp(min.z, 0f, size.z);
			max.z = Mathf.Clamp(max.z, 0f, size.z);
			Vector3 heightmapScale = terrainData.heightmapScale;
			int num = Mathf.FloorToInt(min.x / heightmapScale.x);
			int num2 = Mathf.FloorToInt(min.z / heightmapScale.z);
			int num3 = Mathf.CeilToInt(max.x / heightmapScale.x);
			int num4 = Mathf.CeilToInt(max.z / heightmapScale.z);
			int count = base.Vertices.Count;
			int count2 = base.Triangles.Count;
			if (num < num3 && num2 < num4)
			{
				float num5 = 1f / (float)(terrainData.heightmapWidth - 1);
				float num6 = 1f / (float)(terrainData.heightmapHeight - 1);
				for (int i = num; i <= num3; i++)
				{
					float x2 = (float)i * heightmapScale.x;
					for (int j = num2; j <= num4; j++)
					{
						float height = terrainData.GetHeight(i, j);
						float z2 = (float)j * heightmapScale.z;
						Vector3 item = a_TerrainToDecalsMatrix.MultiplyPoint3x4(new Vector3(x2, height, z2));
						Vector3 interpolatedNormal = terrainData.GetInterpolatedNormal((float)i * num5, (float)j * num6);
						interpolatedNormal = transpose.MultiplyVector(interpolatedNormal);
						interpolatedNormal.Normalize();
						m_Vertices.Add(item);
						m_Normals.Add(interpolatedNormal);
					}
				}
				int num7 = num3 - num + 1;
				int num8 = num4 - num2 + 1;
				int num9 = num7 - 1;
				int num10 = num8 - 1;
				for (int k = 0; k < num9; k++)
				{
					for (int l = 0; l < num10; l++)
					{
						int num11 = count + l + k * num8;
						int item2 = num11 + 1;
						int num12 = num11 + num8;
						int item3 = num12 + 1;
						m_Triangles.Add(num11);
						m_Triangles.Add(item2);
						m_Triangles.Add(item3);
						m_Triangles.Add(num11);
						m_Triangles.Add(item3);
						m_Triangles.Add(num12);
					}
				}
			}
			float num13 = Mathf.Cos(activeDecalProjector.CullingAngle * ((float)Math.PI / 180f));
			if (!Mathf.Approximately(num13, -1f))
			{
				Vector3 v = new Vector3(0f, 1f, 0f);
				Matrix4x4 worldToLocalMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
				v = (activeDecalProjector.ProjectorToWorldMatrix * worldToLocalMatrix).inverse.transpose.MultiplyVector(v).normalized;
				for (int m = count; m < m_Vertices.Count; m++)
				{
					Vector3 rhs = m_Normals[m];
					if (num13 > Vector3.Dot(v, rhs))
					{
						m_RemovedIndices.AddRemovedIndex(m);
					}
				}
				for (int num14 = m_Triangles.Count - 1; num14 >= count2; num14 -= 3)
				{
					int a_Index = m_Triangles[num14];
					int a_Index2 = m_Triangles[num14 - 1];
					int a_Index3 = m_Triangles[num14 - 2];
					if (m_RemovedIndices.IsRemovedIndex(a_Index) || m_RemovedIndices.IsRemovedIndex(a_Index2) || m_RemovedIndices.IsRemovedIndex(a_Index3))
					{
						m_Triangles.RemoveRange(num14 - 2, 3);
					}
					else
					{
						int value = m_RemovedIndices.AdjustedIndex(a_Index);
						int value2 = m_RemovedIndices.AdjustedIndex(a_Index2);
						int value3 = m_RemovedIndices.AdjustedIndex(a_Index3);
						m_Triangles[num14] = value;
						m_Triangles[num14 - 1] = value2;
						m_Triangles[num14 - 2] = value3;
					}
				}
				RemoveIndices(m_RemovedIndices);
				m_RemovedIndices.Clear();
			}
			activeDecalProjector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
			activeDecalProjector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
			activeDecalProjector.IsUV1ProjectionCalculated = false;
			activeDecalProjector.IsUV2ProjectionCalculated = false;
			activeDecalProjector.IsTangentProjectionCalculated = false;
		}

		private void RemoveUnoptimized(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				m_UVs.RemoveRange(a_StartIndex, a_Count);
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				m_UV2s.RemoveRange(a_StartIndex, a_Count);
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				m_Tangents.RemoveRange(a_StartIndex, a_Count);
			}
		}

		private void RemoveOptimizedVerticesNormals(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsTangents(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_Tangents.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsUVs(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_UVs.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsTangentsUVs(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_Tangents.RemoveRange(a_StartIndex, a_Count);
			m_UVs.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsUV2s(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_UV2s.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsTangentsUV2s(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_Tangents.RemoveRange(a_StartIndex, a_Count);
			m_UV2s.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsUVsUV2s(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_UVs.RemoveRange(a_StartIndex, a_Count);
			m_UV2s.RemoveRange(a_StartIndex, a_Count);
		}

		private void RemoveOptimizedVerticesNormalsTangentsUVsUV2s(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_Tangents.RemoveRange(a_StartIndex, a_Count);
			m_UVs.RemoveRange(a_StartIndex, a_Count);
			m_UV2s.RemoveRange(a_StartIndex, a_Count);
		}

		public override void OffsetActiveProjectorVertices()
		{
			DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			float meshOffset = activeDecalProjector.MeshOffset;
			if (!Mathf.Approximately(meshOffset, 0f))
			{
				Matrix4x4 worldToLocalMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
				Matrix4x4 transpose = worldToLocalMatrix.transpose;
				int decalsMeshLowerVertexIndex = activeDecalProjector.DecalsMeshLowerVertexIndex;
				int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
				for (int i = decalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
				{
					Vector3 v = transpose.MultiplyVector(m_Normals[i]).normalized * meshOffset;
					v = worldToLocalMatrix.MultiplyVector(v);
					m_Vertices[i] += v;
				}
			}
		}
	}
}
