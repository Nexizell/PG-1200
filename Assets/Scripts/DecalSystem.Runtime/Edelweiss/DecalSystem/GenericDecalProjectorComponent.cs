using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalProjectorComponent<D, P, DM> : GenericDecalProjectorBaseComponent where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM>
	{
		public D GetDecals()
		{
			D val = (D)null;
			Transform transform = base.CachedTransform;
			while (transform != null && (Object)val == (Object)null)
			{
				val = transform.GetComponent<D>();
				transform = transform.parent;
			}
			return val;
		}
	}
}
