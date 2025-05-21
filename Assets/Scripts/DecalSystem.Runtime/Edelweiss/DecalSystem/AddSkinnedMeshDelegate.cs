using UnityEngine;

namespace Edelweiss.DecalSystem
{
	internal delegate void AddSkinnedMeshDelegate(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed);
}
