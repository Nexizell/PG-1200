using UnityEngine;

namespace Glow11.Blur
{
	internal interface IBlur
	{
		void BlurAndBlitBuffer(RenderTexture rbuffer, RenderTexture destination, Settings settings, bool highPrecision);
	}
}
