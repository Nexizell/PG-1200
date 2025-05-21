using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class HideAtChildOf : MonoBehaviour
	{
		[SerializeField]
		protected internal string _rootObjectName;

		private void Start()
		{
			if (_rootObjectName.IsNullOrEmpty())
			{
				return;
			}
			_rootObjectName = _rootObjectName.ToLower();
			foreach (GameObject item in base.gameObject.Ancestors())
			{
				if (item.name.ToLower() == _rootObjectName)
				{
					base.gameObject.SetActive(false);
					break;
				}
			}
		}
	}
}
