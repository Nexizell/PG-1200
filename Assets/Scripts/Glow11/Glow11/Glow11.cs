using System.Collections;
using Glow11.Blur;
using UnityEngine;

namespace Glow11
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Glow 11")]
	[ExecuteInEditMode]
	public class Glow11 : MonoBehaviour
	{
		private Camera glowCam;

		private GameObject glowObj;

		private BaseGlowCamera ActiveGlow;

		private bool needsReinitialisation = false;

		[SerializeField]
		private BlurMode _blurMode = BlurMode.Default;

		[SerializeField]
		private bool _reuseDepth = false;

		private bool reuseDepthDisabled = false;

		private bool useRt = false;

		[SerializeField]
		private Resolution _rerenderResolution = Resolution.Full;

		private bool _highPrecsionActive = false;

		[SerializeField]
		private bool _highPrecsion = false;

		public Settings settings;

		public BlurMode blurMode
		{
			get
			{
				return _blurMode;
			}
			set
			{
				_blurMode = value;
				needsReinitialisation = true;
			}
		}

		public bool reuseDepth
		{
			get
			{
				return _reuseDepth;
			}
			set
			{
				_reuseDepth = value;
				needsReinitialisation = true;
			}
		}

		public Resolution rerenderResolution
		{
			get
			{
				return _rerenderResolution;
			}
			set
			{
				_rerenderResolution = value;
				needsReinitialisation = true;
			}
		}

		public bool highPrecisionActive
		{
			get
			{
				return _highPrecsionActive;
			}
		}

		public bool highPrecision
		{
			get
			{
				return _highPrecsion;
			}
			set
			{
				_highPrecsion = value;
				_highPrecsionActive = value && supportsHighPrecision;
				if ((bool)ActiveGlow)
				{
					ActiveGlow.highPrecision = _highPrecsionActive;
				}
			}
		}

		public bool supportsHighPrecision
		{
			get
			{
				return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			}
		}

		private void OnDestroy()
		{
			if ((bool)glowCam)
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(glowObj);
				}
				else
				{
					Object.Destroy(glowObj);
				}
			}
		}

		private void Awake()
		{
			glowObj = new GameObject();
			glowObj.name = "You should never see me";
			glowObj.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
			glowCam = glowObj.AddComponent<Camera>();
			glowCam.enabled = false;
			if (settings == null)
			{
				settings = new Settings();
			}
			highPrecision = _highPrecsion;
		}

		private void OnDisable()
		{
			DestroyCamera();
		}

		private void OnEnable()
		{
			InitCamera();
		}

		private void DestroyCamera()
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(ActiveGlow);
			}
			else
			{
				Object.Destroy(ActiveGlow);
			}
			glowObj.SetActive(false);
		}

		private void AutoDisable()
		{
			Debug.LogWarning("The image effect " + ToString() + " has been disabled as it's not supported on the current platform.");
			base.enabled = false;
		}

		internal bool CheckSupport()
		{
			if (!SystemInfo.supportsImageEffects || SystemInfo.graphicsShaderLevel < 20)
			{
				AutoDisable();
				return false;
			}
			return true;
		}

		internal void InitCamera()
		{
			needsReinitialisation = false;
			if ((bool)ActiveGlow)
			{
				DestroyCamera();
			}
			glowObj.SetActive(true);
			IBlur blur = null;
			useRt = false;
			switch (blurMode)
			{
			case BlurMode.Default:
				blur = new DefaultBlur();
				break;
			case BlurMode.Advanced:
				blur = new AdvancedBlur();
				break;
			case BlurMode.HighQuality:
				blur = new HqBlur();
				break;
			case BlurMode.UnityBlur:
				blur = new UnityBlur();
				break;
			}
			glowCam.enabled = false;
			if (_reuseDepth && QualitySettings.antiAliasing == 0)
			{
				reuseDepthDisabled = false;
				ActiveGlow = glowCam.gameObject.AddComponent<GlowCameraReuse>();
			}
			else if (QualitySettings.antiAliasing != 0 || _rerenderResolution != Resolution.Full)
			{
				reuseDepthDisabled = true;
				useRt = true;
				ActiveGlow = glowCam.gameObject.AddComponent<GlowCameraRerenderOnly>();
			}
			else
			{
				reuseDepthDisabled = true;
				ActiveGlow = glowCam.gameObject.AddComponent<GlowCameraRerender>();
				glowCam.enabled = true;
			}
			ActiveGlow.glow11 = this;
			ActiveGlow.highPrecision = _highPrecsionActive;
			ActiveGlow.parentCamera = GetComponent<Camera>();
			ActiveGlow.blur = blur;
			ActiveGlow.settings = settings;
			ActiveGlow.Init();
		}

		private void Update()
		{
			if (CheckSupport())
			{
				if (reuseDepthDisabled && reuseDepth && QualitySettings.antiAliasing == 0)
				{
					needsReinitialisation = true;
				}
				else if (!reuseDepthDisabled && QualitySettings.antiAliasing != 0)
				{
					needsReinitialisation = true;
				}
				else if (useRt && QualitySettings.antiAliasing == 0 && _rerenderResolution == Resolution.Full)
				{
					needsReinitialisation = true;
				}
				else if (!useRt && (QualitySettings.antiAliasing != 0 || (reuseDepthDisabled && _rerenderResolution != Resolution.Full)))
				{
					needsReinitialisation = true;
				}
				if (needsReinitialisation)
				{
					InitCamera();
				}
			}
		}

		private IEnumerator OnPostRender()
		{
			if (CheckSupport())
			{
				if (!ActiveGlow)
				{
					InitCamera();
				}
				if (reuseDepthDisabled && useRt)
				{
					RenderTexture tmpRt2 = null;
					yield return new WaitForEndOfFrame();
					tmpRt2 = RenderTexture.GetTemporary((int)(float)GetComponent<Camera>().pixelWidth / (int)_rerenderResolution, (int)(float)GetComponent<Camera>().pixelHeight / (int)_rerenderResolution, 16);
					glowCam.targetTexture = tmpRt2;
					glowCam.Render();
					RenderTexture.active = null;
					ActiveGlow.blur.BlurAndBlitBuffer(tmpRt2, null, settings, _highPrecsionActive);
					RenderTexture.ReleaseTemporary(tmpRt2);
				}
			}
		}
	}
}
