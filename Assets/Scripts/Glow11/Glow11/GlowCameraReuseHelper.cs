using UnityEngine;

namespace Glow11
{
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	internal class GlowCameraReuseHelper : MonoBehaviour
	{
		internal GlowCameraReuse glowCam;

		private void OnPreCull()
		{
			if (!glowCam)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this);
				}
				else
				{
					Object.DestroyImmediate(this);
				}
			}
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!glowCam)
			{
				OnPreCull();
			}
			else if (glowCam.glow11.CheckSupport())
			{
				glowCam.screenRt = source;
				glowCam.gameObject.GetComponent<Camera>().Render();
				Graphics.Blit(source, destination);
			}
		}
	}
}
