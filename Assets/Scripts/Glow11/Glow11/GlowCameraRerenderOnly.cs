using UnityEngine;

namespace Glow11
{
	[AddComponentMenu("")]
	internal class GlowCameraRerenderOnly : BaseGlowCamera
	{
		private void OnPreCull()
		{
			RenderTexture targetTexture = GetComponent<Camera>().targetTexture;
			GetComponent<Camera>().CopyFrom(parentCamera);
			GetComponent<Camera>().backgroundColor = Color.black;
			GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
			GetComponent<Camera>().SetReplacementShader(base.glowOnly, "RenderType");
			GetComponent<Camera>().renderingPath = RenderingPath.VertexLit;
			GetComponent<Camera>().targetTexture = targetTexture;
		}
	}
}
