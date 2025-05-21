using UnityEngine;

namespace Glow11.Blur
{
	internal class DefaultBlur : BlurBase, IBlur
	{
		private bool maxFallback = false;

		protected Material composeMaxMaterial = null;

		public DefaultBlur()
		{
			Shader shader = Shader.Find("Hidden/Glow 11/Compose Max");
			if (!shader.isSupported)
			{
				shader = Shader.Find("Hidden/Glow 11/Compose Max Fallback");
				maxFallback = true;
			}
			composeMaxMaterial = new Material(shader);
			composeMaxMaterial.hideFlags = HideFlags.DontSaveInEditor;
		}

		public void BlurAndBlitBuffer(RenderTexture rbuffer, RenderTexture destination, Settings settings, bool highPrecision)
		{
			int baseResolution = (int)settings.baseResolution;
			int downsampleResolution = (int)settings.downsampleResolution;
			RenderTexture[] array = new RenderTexture[settings.downsampleSteps * 2];
			RenderTexture renderTexture = null;
			RenderTextureFormat format = ((!highPrecision) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf);
			downsampleMaterial.SetFloat("_Strength", settings.innerStrength / (float)((baseResolution != 4) ? 1 : 4));
			RenderTexture temporary = RenderTexture.GetTemporary(rbuffer.width / baseResolution, rbuffer.height / baseResolution, 0, format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(temporary.width, temporary.height, 0, format);
			Graphics.Blit(rbuffer, temporary, downsampleMaterial, (baseResolution != 4) ? 1 : 0);
			downsampleMaterial.SetFloat("_Strength", settings.innerStrength / (float)((downsampleResolution != 4) ? 1 : 4));
			renderTexture = temporary;
			for (int i = 0; i < settings.downsampleSteps; i++)
			{
				int num = renderTexture.width / downsampleResolution;
				int num2 = renderTexture.height / downsampleResolution;
				if (num == 0 || num2 == 0)
				{
					break;
				}
				array[i * 2] = RenderTexture.GetTemporary(num, num2, 0, format);
				array[i * 2 + 1] = RenderTexture.GetTemporary(num, num2, 0, format);
				Graphics.Blit(renderTexture, array[i * 2], downsampleMaterial, (downsampleResolution != 4) ? 1 : 0);
				renderTexture = array[i * 2];
			}
			for (int num3 = settings.downsampleSteps - 1; num3 >= 0; num3--)
			{
				if (!(array[num3 * 2] == null))
				{
					BlurBuffer(array[num3 * 2], array[num3 * 2 + 1]);
					RenderTexture renderTexture2 = ((num3 <= 0) ? temporary : array[(num3 - 1) * 2]);
					if (settings.downsampleBlendMode == DownsampleBlendMode.Max)
					{
						if (maxFallback)
						{
							composeMaxMaterial.SetTexture("_DestTex", renderTexture2);
						}
						composeMaxMaterial.SetFloat("_Strength", settings.outerStrength / ((float)num3 / 2f + 1f));
						Graphics.Blit(array[num3 * 2], renderTexture2, composeMaxMaterial);
					}
					else
					{
						composeMaterial.SetFloat("_Strength", settings.outerStrength / ((float)num3 / 2f + 1f));
						Graphics.Blit(array[num3 * 2], renderTexture2, composeMaterial, (int)settings.downsampleBlendMode);
					}
				}
			}
			BlurBuffer(temporary, temporary2);
			composeMaterial.SetFloat("_Strength", settings.boostStrength);
			Graphics.Blit(temporary, destination, composeMaterial, (int)settings.blendMode);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			for (int j = 0; j < settings.downsampleSteps; j++)
			{
				RenderTexture.ReleaseTemporary(array[j * 2]);
				RenderTexture.ReleaseTemporary(array[j * 2 + 1]);
			}
		}
	}
}
