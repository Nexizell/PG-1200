using UnityEngine;

namespace Edelweiss.DecalSystem
{
	internal delegate int CutEdgeDelegate(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB);
}
