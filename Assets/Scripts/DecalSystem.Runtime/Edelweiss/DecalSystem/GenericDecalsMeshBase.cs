using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalsMeshBase
	{
		protected List<Vector2> m_UVs = new List<Vector2>();

		protected List<Vector2> m_UV2s = new List<Vector2>();

		protected List<Vector3> m_Vertices = new List<Vector3>();

		protected List<Vector3> m_Normals = new List<Vector3>();

		protected List<Vector4> m_Tangents = new List<Vector4>();

		protected List<int> m_Triangles = new List<int>();

		internal RemovedIndices m_RemovedIndices = new RemovedIndices();

		public List<Vector2> UVs
		{
			get
			{
				RecalculateUVs();
				return m_UVs;
			}
		}

		public List<Vector2> UV2s
		{
			get
			{
				if (Application.isPlaying && HasUV2LightmappingMode())
				{
					throw new InvalidOperationException("The lightmap for the UV2s can not be recalculated if the application is playing!");
				}
				RecalculateUV2s();
				return m_UV2s;
			}
		}

		public List<Vector3> Vertices
		{
			get
			{
				return m_Vertices;
			}
		}

		public List<Vector3> Normals
		{
			get
			{
				return m_Normals;
			}
		}

		public List<Vector4> Tangents
		{
			get
			{
				RecalculateTangents();
				return m_Tangents;
			}
		}

		public List<int> Triangles
		{
			get
			{
				return m_Triangles;
			}
		}

		protected abstract void RecalculateUVs();

		protected abstract bool HasUV2LightmappingMode();

		protected abstract void RecalculateUV2s();

		protected abstract void RecalculateTangents();
	}
}
