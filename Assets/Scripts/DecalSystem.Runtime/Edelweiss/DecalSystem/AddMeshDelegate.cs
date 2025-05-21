using UnityEngine;

namespace Edelweiss.DecalSystem
{
	internal delegate void AddMeshDelegate(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed);
}
