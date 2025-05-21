using UnityEngine;

namespace Glow11.Blur
{
	internal abstract class BlurBase
	{
		protected Material blurMaterial = null;

		protected Material composeMaterial = null;

		protected Material downsampleMaterial = null;

		private bool glMode = true;

		protected Vector2[] blurOffsetsHorizontal = new Vector2[5]
		{
			new Vector2(0f, 0f),
			new Vector2(-1.3846154f, 0f),
			new Vector2(1.3846154f, 0f),
			new Vector2(-3.2307692f, 0f),
			new Vector2(3.2307692f, 0f)
		};

		protected Vector2[] blurOffsetsVertical = new Vector2[5]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, -1.3846154f),
			new Vector2(0f, 1.3846154f),
			new Vector2(0f, -3.2307692f),
			new Vector2(0f, 3.2307692f)
		};

		public BlurBase()
		{
			Shader shader = Shader.Find("Hidden/Glow 11/Compose");
			Shader shader2 = Shader.Find("Hidden/Glow 11/Blur GL");
			if (!shader2.isSupported)
			{
				glMode = false;
				shader2 = Shader.Find("Hidden/Glow 11/Blur");
			}
			Shader shader3 = Shader.Find("Hidden/Glow 11/Downsample");
			composeMaterial = new Material(shader);
			composeMaterial.hideFlags = HideFlags.DontSaveInEditor;
			blurMaterial = new Material(shader2);
			blurMaterial.hideFlags = HideFlags.DontSaveInEditor;
			downsampleMaterial = new Material(shader3);
			downsampleMaterial.hideFlags = HideFlags.DontSaveInEditor;
		}

		protected void BlurBuffer(RenderTexture buffer, RenderTexture buffer2)
		{
			if (glMode)
			{
				Graphics.BlitMultiTap(buffer, buffer2, blurMaterial, blurOffsetsHorizontal);
				Graphics.BlitMultiTap(buffer2, buffer, blurMaterial, blurOffsetsVertical);
				return;
			}
			blurMaterial.SetFloat("_offset1", 1.3846154f);
			blurMaterial.SetFloat("_offset2", 3.2307692f);
			Graphics.Blit(buffer, buffer2, blurMaterial, 0);
			Graphics.Blit(buffer2, buffer, blurMaterial, 1);
		}
	}
}
