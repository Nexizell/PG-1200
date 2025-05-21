using UnityEngine;

namespace Edelweiss.DecalSystem
{
	internal struct DecalRay
	{
		public Vector3 origin;

		public Vector3 direction;

		public DecalRay(Vector3 a_Origin, Vector3 a_Direction)
		{
			origin = a_Origin;
			direction = a_Direction;
		}
	}
}
