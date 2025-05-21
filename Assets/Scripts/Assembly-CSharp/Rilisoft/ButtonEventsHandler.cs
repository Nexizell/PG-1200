using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	public class ButtonEventsHandler : MonoBehaviour
	{
		public UnityEvent OnClickEvent = new UnityEvent();

		private void OnClick()
		{
			if (OnClickEvent != null)
			{
				OnClickEvent.Invoke();
			}
		}
	}
}
