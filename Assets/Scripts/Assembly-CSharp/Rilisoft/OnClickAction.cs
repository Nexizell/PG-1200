using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	public class OnClickAction : MonoBehaviour
	{
		[SerializeField]
		protected internal UnityEvent _events;

		private void OnClick()
		{
			if (_events != null)
			{
				_events.Invoke();
			}
		}
	}
}
