using UnityEngine;

namespace Rilisoft
{
	public sealed class AdventuresMenuController : MonoBehaviour
	{
		[SerializeField]
		protected internal UIButton sandboxButton;

		[SerializeField]
		protected internal float period = 334f;

		private float _distance;

		private void Awake()
		{
		}

		private void OnEnable()
		{
			Refresh();
		}

		private void Refresh()
		{
			sandboxButton.gameObject.SetActive(IsSandboxEnabled());
			Transform parent = sandboxButton.transform.parent;
			float x = (IsSandboxEnabled() ? 0f : (0.5f * period));
			parent.localPosition = new Vector3(x, parent.localPosition.y, parent.localPosition.z);
		}

		private bool IsSandboxEnabled()
		{
			return true;
		}
	}
}
