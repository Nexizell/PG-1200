using UnityEngine;

namespace Glow11
{
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	internal class GlowCameraReuse : BaseGlowCamera
	{
		private GlowCameraReuseHelper helper;

		public RenderTexture screenRt;

		private RenderTexture tmpRt;

		private void ActivateHelper()
		{
			if ((bool)parentCamera && !helper)
			{
				helper = parentCamera.gameObject.AddComponent<GlowCameraReuseHelper>();
				helper.hideFlags = HideFlags.HideInInspector;
				helper.glowCam = this;
			}
		}

		private void OnDisable()
		{
			if (!Application.isEditor)
			{
				helper.glowCam = null;
				Object.Destroy(helper);
			}
			else
			{
				helper.glowCam = null;
			}
		}

		private void OnEnable()
		{
			ActivateHelper();
		}

		internal override void Init()
		{
			ActivateHelper();
		}

		private void OnPreCull()
		{
			ActivateHelper();
			GetComponent<Camera>().CopyFrom(parentCamera);
			GetComponent<Camera>().backgroundColor = Color.black;
			GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
			GetComponent<Camera>().SetReplacementShader(base.glowOnly, "RenderEffect");
			GetComponent<Camera>().renderingPath = RenderingPath.VertexLit;
			tmpRt = RenderTexture.GetTemporary(screenRt.width, screenRt.height);
			RenderTexture.active = tmpRt;
			GL.Clear(false, true, Color.black);
			GetComponent<Camera>().targetTexture = tmpRt;
			GetComponent<Camera>().SetTargetBuffers(tmpRt.colorBuffer, screenRt.depthBuffer);
		}

		private void OnPostRender()
		{
			base.blur.BlurAndBlitBuffer(tmpRt, screenRt, settings, highPrecision);
			RenderTexture.ReleaseTemporary(tmpRt);
		}
	}
}
