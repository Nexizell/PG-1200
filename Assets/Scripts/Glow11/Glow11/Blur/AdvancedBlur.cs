using UnityEngine;

namespace Glow11.Blur
{
	internal class AdvancedBlur : IBlur
	{
		protected Material blurMaterial = null;

		protected Material composeMaterial = null;

		protected Material downsampleMaterial = null;

		private Matrix4x4 offsetsMatrix;

		private Matrix4x4 weightsMatrix;

		public AdvancedBlur()
		{
			Shader shader = Shader.Find("Hidden/Glow 11/Compose");
			Shader shader2 = Shader.Find("Hidden/Glow 11/Advanced Blur");
			Shader shader3 = Shader.Find("Hidden/Glow 11/Downsample");
			composeMaterial = new Material(shader);
			composeMaterial.hideFlags = HideFlags.DontSaveInEditor;
			blurMaterial = new Material(shader2);
			blurMaterial.hideFlags = HideFlags.DontSaveInEditor;
			downsampleMaterial = new Material(shader3);
			downsampleMaterial.hideFlags = HideFlags.DontSaveInEditor;
		}

		protected void BlurBuffer(RenderTexture buffer, RenderTexture buffer2, int passOffset)
		{
			blurMaterial.SetMatrix("_offsets", offsetsMatrix);
			blurMaterial.SetMatrix("_weights", weightsMatrix);
			Graphics.Blit(buffer, buffer2, blurMaterial, passOffset);
			Graphics.Blit(buffer2, buffer, blurMaterial, passOffset + 1);
		}

		public void BlurAndBlitBuffer(RenderTexture rbuffer, RenderTexture destination, Settings settings, bool highPrecision)
		{
			int radius = settings.radius;
			float[] array = new float[radius + 2];
			float num = 1f / (float)(radius + 1);
			float num2 = 0f;
			for (int i = 0; i <= radius; i++)
			{
				num2 += (array[i] = settings.falloff.Evaluate(1f - (float)i * num));
			}
			num2 = num2 * 2f - array[0];
			for (int j = 0; j <= radius; j++)
			{
				array[j] *= settings.falloffScale;
			}
			if (settings.normalize)
			{
				for (int k = 0; k <= radius; k++)
				{
					array[k] /= num2;
				}
			}
			int num3 = Mathf.CeilToInt((float)radius / 2f);
			weightsMatrix[0] = array[0];
			offsetsMatrix[0] = 0f;
			for (int l = 0; l <= num3 - 1; l++)
			{
				float num4 = array[l * 2 + 1];
				float num5 = array[l * 2 + 2];
				float num6 = l * 2 + 1;
				float num7 = l * 2 + 2;
				weightsMatrix[l + 1] = num4 + num5;
				offsetsMatrix[l + 1] = (num6 * num4 + num7 * num5) / weightsMatrix[l + 1];
			}
			int baseResolution = (int)settings.baseResolution;
			downsampleMaterial.SetFloat("_Strength", 1f / (float)((baseResolution != 4) ? 1 : 4));
			RenderTexture temporary = RenderTexture.GetTemporary(rbuffer.width / baseResolution, rbuffer.height / baseResolution, 0, (!highPrecision) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf);
			RenderTexture temporary2 = RenderTexture.GetTemporary(temporary.width, temporary.height, 0, (!highPrecision) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf);
			Graphics.Blit(rbuffer, temporary, downsampleMaterial, (baseResolution != 4) ? 1 : 0);
			int passOffset = num3 * 2 - 2;
			for (int m = 0; m < settings.iterations; m++)
			{
				BlurBuffer(temporary, temporary2, passOffset);
			}
			composeMaterial.SetFloat("_Strength", settings.boostStrength);
			Graphics.Blit(temporary, destination, composeMaterial, (int)settings.blendMode);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}
	}
}
