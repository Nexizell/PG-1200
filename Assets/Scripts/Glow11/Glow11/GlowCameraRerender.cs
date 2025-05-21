using UnityEngine;

namespace Glow11
{
	[AddComponentMenu("")]
	internal class GlowCameraRerender : BaseGlowCamera
	{
		private void OnPreCull()
		{
			GetComponent<Camera>().CopyFrom(parentCamera);
			GetComponent<Camera>().backgroundColor = Color.black;
			GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
			GetComponent<Camera>().SetReplacementShader(base.glowOnly, "RenderType");
			GetComponent<Camera>().renderingPath = RenderingPath.VertexLit;
			GetComponent<Camera>().depth = parentCamera.depth + 0.1f;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.blur.BlurAndBlitBuffer(source, destination, settings, highPrecision);
		}
	}
}
