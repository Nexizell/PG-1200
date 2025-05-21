using UnityEngine;

namespace Glow11.Blur
{
	internal class HqBlur : BlurBase, IBlur
	{
		public void BlurAndBlitBuffer(RenderTexture rbuffer, RenderTexture destination, Settings settings, bool highPrecision)
		{
			int baseResolution = (int)settings.baseResolution;
			downsampleMaterial.SetFloat("_Strength", settings.innerStrength / (float)((baseResolution != 4) ? 1 : 4));
			composeMaterial.SetFloat("_Strength", settings.boostStrength);
			RenderTexture temporary = RenderTexture.GetTemporary(rbuffer.width / baseResolution, rbuffer.height / baseResolution, 0, (!highPrecision) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf);
			RenderTexture temporary2 = RenderTexture.GetTemporary(temporary.width, temporary.height, 0, (!highPrecision) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf);
			Graphics.Blit(rbuffer, temporary, downsampleMaterial, (baseResolution != 4) ? 1 : 0);
			for (int i = 0; i < settings.iterations; i++)
			{
				BlurBuffer(temporary, temporary2);
			}
			Graphics.Blit(temporary, destination, composeMaterial, (int)settings.blendMode);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}
	}
}
