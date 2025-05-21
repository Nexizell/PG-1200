using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class DecalProjectorGroupBase : MonoBehaviour
	{
		public GenericDecalsBase GetDecalsBase()
		{
			GenericDecalsBase genericDecalsBase = null;
			Transform parent = base.transform;
			while (parent != null && genericDecalsBase == null)
			{
				genericDecalsBase = parent.GetComponent<GenericDecalsBase>();
				parent = parent.parent;
			}
			return genericDecalsBase;
		}
	}
}
