using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
	public class PlaceTargetWithMouse : MonoBehaviour
	{
		public float surfaceOffset = 1.5f;

		public GameObject setTargetOn;

		private void Update()
		{
			RaycastHit hitInfo;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
			{
				base.transform.position = hitInfo.point + hitInfo.normal * surfaceOffset;
				if (setTargetOn != null)
				{
					setTargetOn.SendMessage("SetTarget", base.transform);
				}
			}
		}
	}
}
