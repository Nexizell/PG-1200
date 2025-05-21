using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalsMesh<D, P, DM> : GenericDecalsMeshBase where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM>
	{
		protected D m_Decals;

		private List<P> m_Projectors = new List<P>();

		internal RemoveRangeDelegate m_RemoveRangeDelegate;

		public D Decals
		{
			get
			{
				return m_Decals;
			}
		}

		internal List<P> Projectors
		{
			get
			{
				return m_Projectors;
			}
		}

		public P ActiveDecalProjector
		{
			get
			{
				P result = (P)null;
				if (m_Projectors.Count != 0)
				{
					result = m_Projectors[m_Projectors.Count - 1];
				}
				return result;
			}
		}

		internal abstract void InitializeDelegates();

		public void Initialize(D a_Decals)
		{
			m_Decals = a_Decals;
			ClearAll();
		}

		protected override void RecalculateUVs()
		{
			if (!((UnityEngine.Object)m_Decals != (UnityEngine.Object)null) || m_Decals.CurrentUVMode != 0)
			{
				return;
			}
			foreach (P projector in m_Projectors)
			{
				P current = projector;
				if (!current.IsUV1ProjectionCalculated)
				{
					CalculateProjectedUV1(current);
				}
			}
		}

		protected override bool HasUV2LightmappingMode()
		{
			bool result = true;
			if ((UnityEngine.Object)m_Decals != (UnityEngine.Object)null && m_Decals.CurrentUV2Mode != UV2Mode.Lightmapping)
			{
				result = false;
			}
			return result;
		}

		protected override void RecalculateUV2s()
		{
			if (!((UnityEngine.Object)m_Decals != (UnityEngine.Object)null) || m_Decals.CurrentUV2Mode != UV2Mode.Project)
			{
				return;
			}
			foreach (P projector in m_Projectors)
			{
				if (!projector.IsUV2ProjectionCalculated)
				{
					CalculateProjectedUV2(projector);
				}
			}
		}

		protected override void RecalculateTangents()
		{
			if (!((UnityEngine.Object)m_Decals != (UnityEngine.Object)null) || m_Decals.CurrentTangentsMode != TangentsMode.Project)
			{
				return;
			}
			foreach (P projector in m_Projectors)
			{
				if (!projector.IsTangentProjectionCalculated)
				{
					CalculateProjectedTangents(projector);
				}
			}
		}

		public void CalculateProjectedUV1ForActiveProjector()
		{
			CalculateProjectedUV1(ActiveDecalProjector);
		}

		public void CalculateProjectedUV2ForActiveProjector()
		{
			CalculateProjectedUV2(ActiveDecalProjector);
		}

		private void CalculateProjectedUV1(GenericDecalProjectorBase a_Projector)
		{
			Matrix4x4 a_DecalsToProjectorMatrix = a_Projector.WorldToProjectorMatrix * m_Decals.CachedTransform.localToWorldMatrix;
			UVRectangle a_UVRectangle = m_Decals.uvRectangles[a_Projector.UV1RectangleIndex];
			List<Vector2> uVs = m_UVs;
			int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
			CalculateProjectedUV(a_DecalsToProjectorMatrix, a_UVRectangle, uVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			a_Projector.IsUV1ProjectionCalculated = true;
		}

		private void CalculateProjectedUV2(GenericDecalProjectorBase a_Projector)
		{
			Matrix4x4 a_DecalsToProjectorMatrix = a_Projector.WorldToProjectorMatrix * m_Decals.CachedTransform.localToWorldMatrix;
			UVRectangle a_UVRectangle = m_Decals.uv2Rectangles[a_Projector.UV2RectangleIndex];
			List<Vector2> uV2s = m_UV2s;
			int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
			CalculateProjectedUV(a_DecalsToProjectorMatrix, a_UVRectangle, uV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			a_Projector.IsUV2ProjectionCalculated = true;
		}

		private void CalculateProjectedUV(Matrix4x4 a_DecalsToProjectorMatrix, UVRectangle a_UVRectangle, List<Vector2> a_UVs, int a_LowerIndex, int a_UpperIndex)
		{
			Vector2 lowerLeftUV = a_UVRectangle.lowerLeftUV;
			Vector2 size = a_UVRectangle.Size;
			while (a_UVs.Count < a_LowerIndex)
			{
				a_UVs.Add(Vector2.zero);
			}
			for (int i = a_LowerIndex; i <= a_UpperIndex; i++)
			{
				Vector3 v = base.Vertices[i];
				v = a_DecalsToProjectorMatrix.MultiplyPoint3x4(v);
				float x = lowerLeftUV.x + (v.x + 0.5f) * size.x;
				float y = lowerLeftUV.y + (v.z + 0.5f) * size.y;
				Vector2 vector = new Vector2(x, y);
				if (i < a_UVs.Count)
				{
					a_UVs[i] = vector;
				}
				else
				{
					a_UVs.Add(vector);
				}
			}
		}

		private void CalculateProjectedTangents(GenericDecalProjectorBase a_Projector)
		{
			while (m_Tangents.Count < a_Projector.DecalsMeshLowerVertexIndex)
			{
				m_Tangents.Add(Vector4.zero);
			}
			Matrix4x4 transpose = (m_Decals.CachedTransform.localToWorldMatrix * a_Projector.WorldToProjectorMatrix).inverse.transpose;
			Matrix4x4 transpose2 = (a_Projector.ProjectorToWorldMatrix * m_Decals.CachedTransform.worldToLocalMatrix).inverse.transpose;
			for (int i = a_Projector.DecalsMeshLowerVertexIndex; i <= a_Projector.DecalsMeshUpperVertexIndex; i++)
			{
				Vector3 v = base.Normals[i];
				v = transpose.MultiplyVector(v);
				v.z = 0f;
				if (Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f))
				{
					v = new Vector3(0f, 1f, 0f);
				}
				v = new Vector3(v.y, 0f - v.x, v.z);
				v = transpose2.MultiplyVector(v);
				v.Normalize();
				Vector4 vector = new Vector4(v.x, v.y, v.z, -1f);
				if (i < m_Tangents.Count)
				{
					m_Tangents[i] = vector;
				}
				else
				{
					m_Tangents.Add(vector);
				}
			}
		}

		public virtual void ClearAll()
		{
			foreach (P projector in m_Projectors)
			{
				P current = projector;
				current.ResetDecalsMesh();
			}
			m_Projectors.Clear();
			base.Vertices.Clear();
			base.Normals.Clear();
			base.Tangents.Clear();
			base.UVs.Clear();
			base.UV2s.Clear();
			base.Triangles.Clear();
			if ((UnityEngine.Object)m_Decals != (UnityEngine.Object)null)
			{
				m_Decals.LinkedDecalsMesh = null;
			}
		}

		public bool ContainsProjector(P a_Projector)
		{
			return m_Projectors.Contains(a_Projector);
		}

		public void AddProjector(P a_Projector)
		{
			if (a_Projector == null)
			{
				throw new ArgumentNullException("Projector parameter is not allowed to be null!");
			}
			if (a_Projector.DecalsMesh != null)
			{
				throw new InvalidOperationException("Projector is already used in this or another instance!");
			}
			if ((UnityEngine.Object)m_Decals == (UnityEngine.Object)null)
			{
				throw new NullReferenceException("Projectors can only be added if the decals is not null!");
			}
			if (m_Decals.LinkedDecalsMesh != null && m_Decals.LinkedDecalsMesh != this)
			{
				throw new InvalidOperationException("The decals instance is already linked to another decals mesh.");
			}
			P activeDecalProjector = ActiveDecalProjector;
			if (activeDecalProjector != null)
			{
				activeDecalProjector.IsActiveProjector = false;
			}
			if (m_Projectors.Count == 0)
			{
				InitializeDelegates();
			}
			m_Projectors.Add(a_Projector);
			a_Projector.DecalsMesh = this as DM;
			a_Projector.IsActiveProjector = true;
			SetRangesForAddedProjector(a_Projector);
			m_Decals.LinkedDecalsMesh = this;
		}

		internal virtual void SetRangesForAddedProjector(P a_Projector)
		{
			a_Projector.DecalsMeshLowerVertexIndex = m_Vertices.Count;
			a_Projector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
			a_Projector.DecalsMeshLowerTriangleIndex = m_Triangles.Count;
			a_Projector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
		}

		public void RemoveProjector(P a_Projector)
		{
			if (a_Projector.DecalsMesh != this)
			{
				throw new InvalidOperationException("Unable to remove a projector that is not part of this instance.");
			}
			int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
			int decalsMeshVertexCount = a_Projector.DecalsMeshVertexCount;
			int decalsMeshLowerTriangleIndex = a_Projector.DecalsMeshLowerTriangleIndex;
			int decalsMeshTriangleCount = a_Projector.DecalsMeshTriangleCount;
			if (decalsMeshTriangleCount > 0)
			{
				m_Triangles.RemoveRange(decalsMeshLowerTriangleIndex, decalsMeshTriangleCount);
			}
			for (int i = decalsMeshLowerTriangleIndex; i < m_Triangles.Count; i++)
			{
				int num = m_Triangles[i];
				if (num > decalsMeshUpperVertexIndex)
				{
					m_Triangles[i] = num - decalsMeshVertexCount;
				}
			}
			RemoveRangesOfVertexAlignedLists(decalsMeshLowerVertexIndex, decalsMeshVertexCount);
			int a_BoneIndexOffset = BoneIndexOffset(a_Projector);
			RemoveBonesAndAdjustBoneWeightIndices(a_Projector);
			if (a_Projector.IsActiveProjector)
			{
				Projectors.RemoveAt(Projectors.Count - 1);
			}
			else
			{
				int num2 = Projectors.IndexOf(a_Projector);
				for (int j = num2 + 1; j < Projectors.Count; j++)
				{
					P a_Projector2 = Projectors[j];
					AdjustProjectorIndices(a_Projector2, decalsMeshVertexCount, decalsMeshTriangleCount, a_BoneIndexOffset);
				}
				Projectors.RemoveAt(num2);
			}
			a_Projector.ResetDecalsMesh();
			if (m_Projectors.Count == 0)
			{
				m_Decals.LinkedDecalsMesh = null;
			}
		}

		internal virtual void RemoveRangesOfVertexAlignedLists(int a_LowerIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_LowerIndex, a_Count);
			if (a_LowerIndex < m_Normals.Count)
			{
				m_Normals.RemoveRange(a_LowerIndex, a_Count);
			}
			if (a_LowerIndex < m_UVs.Count)
			{
				m_UVs.RemoveRange(a_LowerIndex, a_Count);
			}
			if (a_LowerIndex < m_UV2s.Count)
			{
				m_UV2s.RemoveRange(a_LowerIndex, a_Count);
			}
			if (a_LowerIndex < m_Tangents.Count)
			{
				m_Tangents.RemoveRange(a_LowerIndex, a_Count);
			}
		}

		internal virtual int BoneIndexOffset(P a_Projector)
		{
			return 0;
		}

		internal virtual void RemoveBonesAndAdjustBoneWeightIndices(P a_Projector)
		{
		}

		internal virtual void AdjustProjectorIndices(P a_Projector, int a_VertexIndexOffset, int a_TriangleIndexOffset, int a_BoneIndexOffset)
		{
			a_Projector.DecalsMeshLowerVertexIndex -= a_VertexIndexOffset;
			a_Projector.DecalsMeshUpperVertexIndex -= a_VertexIndexOffset;
			a_Projector.DecalsMeshLowerTriangleIndex -= a_TriangleIndexOffset;
			a_Projector.DecalsMeshUpperTriangleIndex -= a_TriangleIndexOffset;
		}

		public abstract void OffsetActiveProjectorVertices();

		public void RemoveTriangleAt(int a_Index)
		{
			m_Triangles.RemoveRange(a_Index, 3);
		}

		public void RemoveTrianglesAt(int a_Index, int a_Count)
		{
			m_Triangles.RemoveRange(a_Index, 3 * a_Count);
		}

		internal void RemoveAndAdjustIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices)
		{
			AdjustTriangleIndices(a_LowerTriangleIndex, a_RemovedIndices);
			RemoveIndices(a_RemovedIndices);
		}

		private void AdjustTriangleIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices)
		{
			for (int i = a_LowerTriangleIndex; i < m_Triangles.Count; i++)
			{
				m_Triangles[i] = a_RemovedIndices.AdjustedIndex(m_Triangles[i]);
			}
			P activeDecalProjector = ActiveDecalProjector;
			activeDecalProjector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
		}

		internal void RemoveIndices(RemovedIndices a_RemovedIndices)
		{
			int num = -1;
			int num2 = 0;
			for (int num3 = m_Vertices.Count - 1; num3 >= 0; num3--)
			{
				bool flag = a_RemovedIndices.IsRemovedIndex(num3);
				if (flag)
				{
					if (num == -1)
					{
						num = num3;
						num2 = 1;
					}
					else
					{
						num = num3;
						num2++;
					}
				}
				if ((!flag || num3 == 0) && num != -1)
				{
					m_RemoveRangeDelegate(num, num2);
					num = -1;
					num2 = 0;
				}
			}
			P activeDecalProjector = ActiveDecalProjector;
			activeDecalProjector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
		}
	}
}
