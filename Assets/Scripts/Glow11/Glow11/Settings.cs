using System;
using UnityEngine;

namespace Glow11
{
	[Serializable]
	public class Settings
	{
		public int downsampleSteps = 2;

		public Resolution baseResolution = Resolution.Quarter;

		public DownsampleResolution downsampleResolution = DownsampleResolution.Quarter;

		public int iterations = 3;

		public float blurSpread = 0.6f;

		public float innerStrength = 1f;

		public float outerStrength = 1f;

		public float boostStrength = 1f;

		public BlendMode blendMode = BlendMode.Additive;

		public DownsampleBlendMode downsampleBlendMode = DownsampleBlendMode.Max;

		public AnimationCurve falloff = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public float falloffScale = 1f;

		public int radius = 4;

		public bool normalize = true;
	}
}
