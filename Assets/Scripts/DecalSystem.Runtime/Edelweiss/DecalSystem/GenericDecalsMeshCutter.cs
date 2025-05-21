using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalsMeshCutter<D, P, DM> where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM>
	{
		internal DM m_DecalsMesh;

		internal P m_ActiveProjector;

		internal List<RelativeVertexLocation> m_RelativeVertexLocations = new List<RelativeVertexLocation>();

		internal List<int> m_ObsoleteTriangleIndices = new List<int>();

		internal CutEdges m_CutEdges = new CutEdges();

		internal RemovedIndices m_RemovedIndices = new RemovedIndices();

		internal CutEdgeDelegate m_GetCutEdgeDelegate;

		internal CutEdgeDelegate m_CreateCutEdgeDelegate;

		internal abstract void InitializeDelegates();

		public void CutDecalsPlanes(DM a_DecalsMesh)
		{
			if (a_DecalsMesh == null)
			{
				throw new ArgumentNullException("The decals mesh argument is null!");
			}
			if (a_DecalsMesh.ActiveDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector of the decals mesh is null!");
			}
			m_DecalsMesh = a_DecalsMesh;
			m_ActiveProjector = a_DecalsMesh.ActiveDecalProjector;
			InitializeDelegates();
			D decals = a_DecalsMesh.Decals;
			Matrix4x4 matrix4x = decals.CachedTransform.worldToLocalMatrix * m_ActiveProjector.ProjectorToWorldMatrix;
			Matrix4x4 transpose = matrix4x.inverse.transpose;
			Vector3 vector = matrix4x.MultiplyPoint3x4(new Vector3(0.5f, 0f, 0f));
			Vector3 normalized = transpose.MultiplyVector(new Vector3(1f, 0f, 0f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
			vector = matrix4x.MultiplyPoint3x4(new Vector3(-0.5f, 0f, 0f));
			normalized = transpose.MultiplyVector(new Vector3(-1f, 0f, 0f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
			vector = matrix4x.MultiplyPoint3x4(new Vector3(0f, 0f, 0.5f));
			normalized = transpose.MultiplyVector(new Vector3(0f, 0f, 1f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
			vector = matrix4x.MultiplyPoint3x4(new Vector3(0f, 0f, -0.5f));
			normalized = transpose.MultiplyVector(new Vector3(0f, 0f, -1f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
			vector = matrix4x.MultiplyPoint3x4(new Vector3(0f, 0f, 0f));
			normalized = transpose.MultiplyVector(new Vector3(0f, 1f, 0f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
			vector = matrix4x.MultiplyPoint3x4(new Vector3(0f, -1f, 0f));
			normalized = transpose.MultiplyVector(new Vector3(0f, -1f, 0f)).normalized;
			PlaneDecalsMeshCutter(a_Plane: new Plane(normalized, -vector), a_DecalsMesh: a_DecalsMesh);
		}

		internal void PlaneDecalsMeshCutter(DM a_DecalsMesh, Plane a_Plane)
		{
			m_RelativeVertexLocations.Clear();
			m_ObsoleteTriangleIndices.Clear();
			m_CutEdges.Clear();
			m_RemovedIndices.Clear();
			int decalsMeshLowerVertexIndex = m_ActiveProjector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = m_ActiveProjector.DecalsMeshUpperVertexIndex;
			int num = decalsMeshLowerVertexIndex;
			Vector3 a_PlaneOrigin = PlaneOrigin(a_Plane);
			Vector3 normal = a_Plane.normal;
			for (int i = decalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
			{
				Vector3 a_Vertex = a_DecalsMesh.Vertices[i];
				RelativeVertexLocation item = VertexLocationRelativeToPlane(a_PlaneOrigin, normal, a_Vertex);
				m_RelativeVertexLocations.Add(item);
			}
			int count = a_DecalsMesh.Triangles.Count;
			int decalsMeshLowerTriangleIndex = m_ActiveProjector.DecalsMeshLowerTriangleIndex;
			for (int j = decalsMeshLowerTriangleIndex; j < count; j += 3)
			{
				int num2 = a_DecalsMesh.Triangles[j];
				int num3 = a_DecalsMesh.Triangles[j + 1];
				int num4 = a_DecalsMesh.Triangles[j + 2];
				RelativeVertexLocation relativeVertexLocation = m_RelativeVertexLocations[num2 - num];
				RelativeVertexLocation relativeVertexLocation2 = m_RelativeVertexLocations[num3 - num];
				RelativeVertexLocation relativeVertexLocation3 = m_RelativeVertexLocations[num4 - num];
				if (relativeVertexLocation != RelativeVertexLocation.Inside && relativeVertexLocation2 != RelativeVertexLocation.Inside && relativeVertexLocation3 != RelativeVertexLocation.Inside)
				{
					m_ObsoleteTriangleIndices.Add(j);
					continue;
				}
				Vector3 vector = a_DecalsMesh.Vertices[num2];
				Vector3 vector2 = a_DecalsMesh.Vertices[num3];
				Vector3 vector3 = a_DecalsMesh.Vertices[num4];
				bool flag = Intersect(relativeVertexLocation, relativeVertexLocation2);
				bool flag2 = Intersect(relativeVertexLocation, relativeVertexLocation3);
				bool flag3 = Intersect(relativeVertexLocation2, relativeVertexLocation3);
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				if (flag)
				{
					DecalRay a_Ray = new DecalRay(vector, vector2 - vector);
					float a_IntersectionFactorAB = IntersectionFactor(a_Ray, a_Plane);
					num5 = m_GetCutEdgeDelegate(num, num2, num3, vector, vector2, relativeVertexLocation == RelativeVertexLocation.Inside, a_IntersectionFactorAB);
				}
				if (flag2)
				{
					DecalRay a_Ray2 = new DecalRay(vector, vector3 - vector);
					float a_IntersectionFactorAB2 = IntersectionFactor(a_Ray2, a_Plane);
					num6 = m_GetCutEdgeDelegate(num, num2, num4, vector, vector3, relativeVertexLocation == RelativeVertexLocation.Inside, a_IntersectionFactorAB2);
				}
				if (flag3)
				{
					DecalRay a_Ray3 = new DecalRay(vector2, vector3 - vector2);
					float a_IntersectionFactorAB3 = IntersectionFactor(a_Ray3, a_Plane);
					num7 = m_GetCutEdgeDelegate(num, num3, num4, vector2, vector3, relativeVertexLocation2 == RelativeVertexLocation.Inside, a_IntersectionFactorAB3);
				}
				if (flag && flag2)
				{
					if (relativeVertexLocation == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j + 1] = num5;
						a_DecalsMesh.Triangles[j + 2] = num6;
						continue;
					}
					a_DecalsMesh.Triangles[j] = num5;
					a_DecalsMesh.Triangles.Add(num4);
					a_DecalsMesh.Triangles.Add(num6);
					a_DecalsMesh.Triangles.Add(num5);
				}
				else if (flag && flag3)
				{
					if (relativeVertexLocation2 == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j] = num5;
						a_DecalsMesh.Triangles[j + 2] = num7;
						continue;
					}
					a_DecalsMesh.Triangles[j + 1] = num5;
					a_DecalsMesh.Triangles.Add(num4);
					a_DecalsMesh.Triangles.Add(num5);
					a_DecalsMesh.Triangles.Add(num7);
				}
				else if (flag2 && flag3)
				{
					if (relativeVertexLocation3 == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j] = num6;
						a_DecalsMesh.Triangles[j + 1] = num7;
						continue;
					}
					a_DecalsMesh.Triangles[j + 2] = num6;
					a_DecalsMesh.Triangles.Add(num3);
					a_DecalsMesh.Triangles.Add(num7);
					a_DecalsMesh.Triangles.Add(num6);
				}
				else if (flag && relativeVertexLocation3 == RelativeVertexLocation.OnPlane)
				{
					if (relativeVertexLocation == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j + 1] = num5;
					}
					else
					{
						a_DecalsMesh.Triangles[j] = num5;
					}
				}
				else if (flag2 && relativeVertexLocation2 == RelativeVertexLocation.OnPlane)
				{
					if (relativeVertexLocation == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j + 2] = num6;
					}
					else
					{
						a_DecalsMesh.Triangles[j] = num6;
					}
				}
				else if (flag3 && relativeVertexLocation == RelativeVertexLocation.OnPlane)
				{
					if (relativeVertexLocation2 == RelativeVertexLocation.Inside)
					{
						a_DecalsMesh.Triangles[j + 2] = num7;
					}
					else
					{
						a_DecalsMesh.Triangles[j + 1] = num7;
					}
				}
			}
			for (int num8 = m_ObsoleteTriangleIndices.Count - 1; num8 >= 0; num8--)
			{
				int num9 = m_ObsoleteTriangleIndices[num8];
				int num10 = 1;
				while (num8 > 0 && num9 - 3 == m_ObsoleteTriangleIndices[num8 - 1])
				{
					num9 -= 3;
					num10++;
					num8--;
				}
				a_DecalsMesh.RemoveTrianglesAt(num9, num10);
			}
			for (int k = 0; k < m_RelativeVertexLocations.Count; k++)
			{
				if (m_RelativeVertexLocations[k] == RelativeVertexLocation.Outside)
				{
					m_RemovedIndices.AddRemovedIndex(k + num);
				}
			}
			a_DecalsMesh.RemoveAndAdjustIndices(decalsMeshLowerTriangleIndex, m_RemovedIndices);
			m_ActiveProjector.IsUV1ProjectionCalculated = false;
			m_ActiveProjector.IsUV2ProjectionCalculated = false;
			m_ActiveProjector.IsTangentProjectionCalculated = false;
		}

		private bool Intersect(RelativeVertexLocation a_VertexLocation1, RelativeVertexLocation a_VertexLocation2)
		{
			return a_VertexLocation1 != 0 && a_VertexLocation2 != 0 && a_VertexLocation1 != a_VertexLocation2;
		}

		private float IntersectionFactor(DecalRay a_Ray, Plane a_Plane)
		{
			float num = Vector3.Dot(a_Plane.normal, a_Ray.direction);
			float num2 = a_Plane.distance - Vector3.Dot(a_Plane.normal, a_Ray.origin);
			return num2 / num;
		}

		private Vector3 PlaneOrigin(Plane a_Plane)
		{
			return a_Plane.distance * a_Plane.normal;
		}

		private RelativeVertexLocation VertexLocationRelativeToPlane(Vector3 a_PlaneOrigin, Vector3 a_PlaneNormal, Vector3 a_Vertex)
		{
			float num = Vector3.Dot(a_Vertex - a_PlaneOrigin, a_PlaneNormal);
			if (Mathf.Approximately(num, 0f))
			{
				return RelativeVertexLocation.OnPlane;
			}
			if (num < 0f)
			{
				return RelativeVertexLocation.Inside;
			}
			return RelativeVertexLocation.Outside;
		}

		internal int CutEdge(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
		{
			CutEdge a_CutEdge = new CutEdge(a_IndexA, a_IndexB);
			if (m_CutEdges.HasCutEdge(a_CutEdge))
			{
				return m_CutEdges[a_CutEdge].ModifiedIndex;
			}
			return m_CreateCutEdgeDelegate(a_RelativeVertexLocationsOffset, a_IndexA, a_IndexB, a_VertexA, a_VertexB, a_IsVertexAInside, a_IntersectionFactorAB);
		}
	}
}
