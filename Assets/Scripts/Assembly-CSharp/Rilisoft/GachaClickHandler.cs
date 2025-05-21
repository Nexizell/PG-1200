using UnityEngine;

namespace Rilisoft
{
	public class GachaClickHandler : MonoBehaviour
	{
		private void OnClick()
		{
			if (ButOpenGift.instance != null)
			{
				ButOpenGift.instance.Click();
			}
		}
	}
}
