using Glow11.Blur;
using UnityEngine;

namespace Glow11
{
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	internal class BaseGlowCamera : MonoBehaviour
	{
		internal IBlur _blur;

		public Camera parentCamera;

		public Settings settings;

		public Glow11 glow11;

		public bool highPrecision;

		private Shader _glowOnly;

		internal IBlur blur
		{
			get
			{
				return _blur;
			}
			set
			{
				_blur = value;
			}
		}

		protected Shader glowOnly
		{
			get
			{
				if (!_glowOnly)
				{
					_glowOnly = Shader.Find("Hidden/Glow 11/GlowObjects");
				}
				return _glowOnly;
			}
		}

		internal virtual void Init()
		{
		}
	}
}
