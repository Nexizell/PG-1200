using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class StarView : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _foregroundObject;

		internal event EventHandler Clicked;

		internal void SetVisibility(bool visible)
		{
			if (!(_foregroundObject == null))
			{
				_foregroundObject.SetActive(visible);
			}
		}

		private void OnClick()
		{
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}
	}
}
