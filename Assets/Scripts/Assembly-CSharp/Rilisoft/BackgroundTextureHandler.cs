using UnityEngine;

namespace Rilisoft
{
	public class BackgroundTextureHandler : MonoBehaviour
	{
		public bool SetupCleanTexture;

		private void OnEnable()
		{
			SetTexture();
		}

		private void SetTexture()
		{
			Texture2D texture2D = GetTexture();
			if (texture2D == null)
			{
				texture2D = Resources.Load<Texture2D>("coinsFon");
			}
			UITexture component = GetComponent<UITexture>();
			if (component != null)
			{
				component.mainTexture = texture2D;
				return;
			}
			Renderer component2 = GetComponent<Renderer>();
			if (component2 != null)
			{
				component2.sharedMaterial.SetTexture("_MainTex", texture2D);
			}
		}

		private Texture2D GetTexture()
		{
			if (SetupCleanTexture)
			{
				return BackgroundsManager.LoadingBackgroudTexture;
			}
			return BackgroundsManager.LoadingBackgroudTextureBlured;
		}
	}
}
