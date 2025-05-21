using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalProjectorBase
	{
		private bool m_IsActiveProjector;

		private int m_DecalsMeshLowerVertexIndex;

		private int m_DecalsMeshUpperVertexIndex;

		private int m_DecalsMeshLowerTriangleIndex;

		private int m_DecalsMeshUpperTriangleIndex;

		private bool m_IsUV1ProjectionCalculated;

		private bool m_IsUV2ProjectionCalculated;

		private bool m_IsTangentProjectionCalculated;

		public bool IsActiveProjector
		{
			get
			{
				return m_IsActiveProjector;
			}
			internal set
			{
				m_IsActiveProjector = value;
			}
		}

		public int DecalsMeshLowerVertexIndex
		{
			get
			{
				return m_DecalsMeshLowerVertexIndex;
			}
			internal set
			{
				m_DecalsMeshLowerVertexIndex = value;
			}
		}

		public int DecalsMeshUpperVertexIndex
		{
			get
			{
				return m_DecalsMeshUpperVertexIndex;
			}
			internal set
			{
				m_DecalsMeshUpperVertexIndex = value;
			}
		}

		public int DecalsMeshVertexCount
		{
			get
			{
				return DecalsMeshUpperVertexIndex - DecalsMeshLowerVertexIndex + 1;
			}
		}

		public int DecalsMeshLowerTriangleIndex
		{
			get
			{
				return m_DecalsMeshLowerTriangleIndex;
			}
			internal set
			{
				m_DecalsMeshLowerTriangleIndex = value;
			}
		}

		public int DecalsMeshUpperTriangleIndex
		{
			get
			{
				return m_DecalsMeshUpperTriangleIndex;
			}
			internal set
			{
				m_DecalsMeshUpperTriangleIndex = value;
			}
		}

		public int DecalsMeshTriangleCount
		{
			get
			{
				return DecalsMeshUpperTriangleIndex - DecalsMeshLowerTriangleIndex + 1;
			}
		}

		public bool IsUV1ProjectionCalculated
		{
			get
			{
				return m_IsUV1ProjectionCalculated;
			}
			internal set
			{
				m_IsUV1ProjectionCalculated = value;
			}
		}

		public bool IsUV2ProjectionCalculated
		{
			get
			{
				return m_IsUV2ProjectionCalculated;
			}
			internal set
			{
				m_IsUV2ProjectionCalculated = value;
			}
		}

		public bool IsTangentProjectionCalculated
		{
			get
			{
				return m_IsTangentProjectionCalculated;
			}
			internal set
			{
				m_IsTangentProjectionCalculated = value;
			}
		}

		public abstract Vector3 Position { get; }

		public abstract Quaternion Rotation { get; }

		public abstract Vector3 Scale { get; }

		public abstract float CullingAngle { get; }

		public abstract float MeshOffset { get; }

		public abstract int UV1RectangleIndex { get; }

		public abstract int UV2RectangleIndex { get; }

		public Matrix4x4 ProjectorToWorldMatrix
		{
			get
			{
				return Matrix4x4.TRS(Position, Rotation, Scale);
			}
		}

		public Matrix4x4 WorldToProjectorMatrix
		{
			get
			{
				return ProjectorToWorldMatrix.inverse;
			}
		}

		public Bounds WorldBounds()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(Position, Rotation, Vector3.one);
			Vector3 vector = 0.5f * Scale;
			Vector3 vector2 = new Vector3(0f, 0f - Mathf.Abs(vector.y), 0f);
			Vector3 center = matrix4x.MultiplyPoint3x4(Vector3.zero);
			Bounds result = new Bounds(center, Vector3.zero);
			center = vector2 + new Vector3(vector.x, vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, 0f - vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, 0f - vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			return result;
		}
	}
}
