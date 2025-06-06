using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class PlatformSpecificContent : MonoBehaviour
	{
		internal enum BuildTargetGroup
		{
			Standalone = 0,
			Mobile = 1
		}

		[SerializeField]
		protected internal GameObject[] m_Content = new GameObject[0];

		[SerializeField]
		protected internal MonoBehaviour[] m_MonoBehaviours = new MonoBehaviour[0];

		[SerializeField]
		protected internal bool m_ChildrenOfThisObject;

		private void OnEnable()
		{
			CheckEnableContent();
		}

		private void CheckEnableContent()
		{
		}

		private void EnableContent(bool enabled)
		{
			if (m_Content.Length != 0)
			{
				GameObject[] content = m_Content;
				foreach (GameObject gameObject in content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (m_ChildrenOfThisObject)
			{
				foreach (Transform item in base.transform)
				{
					item.gameObject.SetActive(enabled);
				}
			}
			if (m_MonoBehaviours.Length != 0)
			{
				MonoBehaviour[] monoBehaviours = m_MonoBehaviours;
				for (int i = 0; i < monoBehaviours.Length; i++)
				{
					monoBehaviours[i].enabled = enabled;
				}
			}
		}
	}
}
