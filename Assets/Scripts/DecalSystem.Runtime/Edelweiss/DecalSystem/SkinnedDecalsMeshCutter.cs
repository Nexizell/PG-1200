using System;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public class SkinnedDecalsMeshCutter : GenericDecalsMeshCutter<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
	{
		private BoneWeightElement[] m_BoneWeightElements = new BoneWeightElement[8];

		internal override void InitializeDelegates()
		{
			m_GetCutEdgeDelegate = base.CutEdge;
			bool flag = m_DecalsMesh.Decals.CurrentTangentsMode == TangentsMode.Target;
			bool flag2 = m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV || m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV2;
			bool flag3 = m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV2;
			if (!flag && !flag2 && !flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (!flag && !flag2 && flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (!flag && flag2 && !flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (!flag && flag2 && flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (flag && !flag2 && !flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (flag && !flag2 && flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (flag && flag2 && !flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
			else if (flag && flag2 && flag3)
			{
				m_CreateCutEdgeDelegate = CutEdgeUnoptimized;
			}
		}

		private int CutEdgeUnoptimized(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
		{
			CutEdge a_CutEdge = new CutEdge(a_IndexA, a_IndexB);
			Vector3 a = m_DecalsMesh.OriginalVertices[a_IndexA];
			Vector3 b = m_DecalsMesh.OriginalVertices[a_IndexB];
			Vector3 a2 = m_DecalsMesh.Normals[a_IndexA];
			Vector3 b2 = m_DecalsMesh.Normals[a_IndexB];
			BoneWeight a_BoneWeight = m_DecalsMesh.BoneWeights[a_IndexA];
			BoneWeight a_BoneWeight2 = m_DecalsMesh.BoneWeights[a_IndexB];
			int count = m_DecalsMesh.Vertices.Count;
			m_DecalsMesh.OriginalVertices.Add(Vector3.Lerp(a, b, a_IntersectionFactorAB));
			m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
			m_DecalsMesh.Normals.Add(Vector3.Lerp(a2, b2, a_IntersectionFactorAB));
			m_DecalsMesh.BoneWeights.Add(LerpBoneWeights(a_BoneWeight, a_BoneWeight2, a_IntersectionFactorAB));
			m_ActiveProjector.DecalsMeshUpperVertexIndex += 1;
			if (m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV || m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				m_DecalsMesh.UVs.Add(Vector2.Lerp(m_DecalsMesh.UVs[a_IndexA], m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
			}
			if (m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				m_DecalsMesh.UV2s.Add(Vector2.Lerp(m_DecalsMesh.UV2s[a_IndexA], m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
			}
			if (m_DecalsMesh.Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				m_DecalsMesh.Tangents.Add(Vector4.Lerp(m_DecalsMesh.Tangents[a_IndexA], m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
			}
			if (a_IsVertexAInside)
			{
				a_CutEdge.newVertex2Index = count;
				m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
			}
			else
			{
				a_CutEdge.newVertex1Index = count;
				m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
			}
			m_CutEdges.AddEdge(a_CutEdge);
			return count;
		}

		private BoneWeight LerpBoneWeights(BoneWeight a_BoneWeight1, BoneWeight a_BoneWeight2, float a_Factor)
		{
			BoneWeightElement boneWeightElement = default(BoneWeightElement);
			float num = 1f - a_Factor;
			boneWeightElement.index = a_BoneWeight1.boneIndex0;
			boneWeightElement.weight = a_BoneWeight1.weight0 * num;
			m_BoneWeightElements[0] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight1.boneIndex1;
			boneWeightElement.weight = a_BoneWeight1.weight1 * num;
			m_BoneWeightElements[1] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight1.boneIndex2;
			boneWeightElement.weight = a_BoneWeight1.weight2 * num;
			m_BoneWeightElements[2] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight1.boneIndex3;
			boneWeightElement.weight = a_BoneWeight1.weight3 * num;
			m_BoneWeightElements[3] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight2.boneIndex0;
			boneWeightElement.weight = a_BoneWeight2.weight0 * a_Factor;
			m_BoneWeightElements[4] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight2.boneIndex1;
			boneWeightElement.weight = a_BoneWeight2.weight1 * a_Factor;
			m_BoneWeightElements[5] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight2.boneIndex2;
			boneWeightElement.weight = a_BoneWeight2.weight2 * a_Factor;
			m_BoneWeightElements[6] = boneWeightElement;
			boneWeightElement.index = a_BoneWeight2.boneIndex3;
			boneWeightElement.weight = a_BoneWeight2.weight3 * a_Factor;
			m_BoneWeightElements[7] = boneWeightElement;
			for (int i = 0; i < 4; i++)
			{
				int index = m_BoneWeightElements[i].index;
				for (int j = 4; j < 8; j++)
				{
					int index2 = m_BoneWeightElements[j].index;
					if (index == index2)
					{
						m_BoneWeightElements[i].weight = m_BoneWeightElements[i].weight + m_BoneWeightElements[j].weight;
						m_BoneWeightElements[j].weight = 0f;
						m_BoneWeightElements[j].index = 0;
					}
				}
			}
			Array.Sort(m_BoneWeightElements);
			float num2 = 1f / (m_BoneWeightElements[0].weight + m_BoneWeightElements[1].weight + m_BoneWeightElements[2].weight + m_BoneWeightElements[3].weight);
			BoneWeight result = default(BoneWeight);
			result.boneIndex0 = m_BoneWeightElements[0].index;
			result.weight0 = m_BoneWeightElements[0].weight * num2;
			result.boneIndex1 = m_BoneWeightElements[1].index;
			result.weight1 = m_BoneWeightElements[1].weight * num2;
			result.boneIndex2 = m_BoneWeightElements[2].index;
			result.weight2 = m_BoneWeightElements[2].weight * num2;
			result.boneIndex3 = m_BoneWeightElements[3].index;
			result.weight3 = m_BoneWeightElements[3].weight * num2;
			return result;
		}
	}
}
