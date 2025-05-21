using UnityEngine;

namespace Rilisoft
{
	public class ScreenshotManager
	{
		public static Texture2D TakeScreenshot(Camera cam, int width, int height, TextureFormat textureFormat = TextureFormat.RGB24)
		{
			RenderTexture renderTexture2 = (cam.targetTexture = new RenderTexture(width, height, 24));
			Texture2D texture2D = new Texture2D(width, height, textureFormat, false);
			cam.Render();
			RenderTexture.active = renderTexture2;
			texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			cam.targetTexture = null;
			RenderTexture.active = null;
			if (Application.isPlaying || !Application.isEditor)
			{
				Object.Destroy(renderTexture2);
				return texture2D;
			}
			Object.DestroyImmediate(renderTexture2);
			return texture2D;
		}
	}
}
