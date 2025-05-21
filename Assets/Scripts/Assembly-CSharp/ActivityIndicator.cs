using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using I2.Loc;
using UnityEngine;

public sealed class ActivityIndicator : MonoBehaviour
{
	public static ActivityIndicator instance;

	public float rotSpeed = 180f;

	private Vector3 vectRotateSpeed;

	public string text;

	public Camera needCam;

	public GameObject panelWindowLoading;

	public GameObject panelIndicator;

	public GameObject objIndicator;

	public GameObject panelProgress;

	public UILabel lbLoading;

	public UILabel lbPercentLoading;

	public UILabel legendLabel;

	public UITexture[] txFon;

	public UITexture txProgressBar;

	public UIFont bitmapFont;

	private static float curPers;

	private bool canClearMemory = true;

	internal const string DefaultLegendLabel = "Please reboot your device if frozen.";

	public static float LoadingProgress
	{
		get
		{
			return curPers;
		}
		set
		{
			if (instance != null)
			{
				curPers = value;
				curPers = Mathf.Clamp01(curPers);
				if (curPers < 0f)
				{
					curPers = 0f;
				}
				if (curPers > 1f)
				{
					curPers = 1f;
				}
				if (instance.txProgressBar != null)
				{
					instance.txProgressBar.fillAmount = curPers;
				}
				if ((bool)instance.lbPercentLoading)
				{
					instance.lbPercentLoading.text = string.Format("{0}%", new object[1] { Mathf.RoundToInt(curPers * 100f) });
				}
			}
		}
	}

	public static bool IsShowWindowLoading
	{
		set
		{
			if (instance != null)
			{
				if (!value && instance.txFon != null)
				{
					instance.txFon[0].mainTexture = null;
				}
				if (instance.panelWindowLoading != null)
				{
					instance.panelWindowLoading.SetActive(value);
				}
			}
		}
	}

	public static bool IsActiveIndicator
	{
		get
		{
			if (instance == null || instance.panelIndicator == null)
			{
				return false;
			}
			return instance.panelIndicator.activeSelf;
		}
		set
		{
			if (!(instance == null))
			{
				if (instance.panelIndicator != null)
				{
					instance.panelIndicator.SetActive(value);
				}
				if (instance.needCam != null)
				{
					instance.needCam.Render();
				}
				if (!value)
				{
					instance.HandleLocalizationChanged();
				}
			}
		}
	}

	public void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		vectRotateSpeed = new Vector3(0f, rotSpeed, 0f);
		LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
	}

	private void Start()
	{
		OnEnable();
		lbLoading.GetComponent<Localize>().enabled = true;
		if (Launcher.UsingNewLauncher)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (objIndicator != null)
		{
			objIndicator.transform.Rotate(vectRotateSpeed * Time.unscaledDeltaTime);
		}
	}

	private void OnDestroy()
	{
		instance = null;
		LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
	}

	private void HandleLocalizationChanged()
	{
		if (lbLoading != null)
		{
			string text = LocalizationStore.Get("Key_0853");
			this.text = (("Key_0853" == text) ? "Please reboot your device if frozen." : text);
			lbLoading.text = this.text;
		}
	}

	private void OnEnable()
	{
		HandleLocalizationChanged();
	}

	public static void SetLoadingFon(Texture needFon)
	{
		if (!(instance == null) && !(instance.txFon[0] == null))
		{
			instance.txFon[0].mainTexture = needFon;
		}
	}

	public IEnumerable<float> ReplaceLoadingFon(Texture needFon, float duration)
	{
		txFon[1].mainTexture = needFon;
		txFon[1].alpha = 0f;
		float _curDuration = 0f;
		yield return 0f;
		while (_curDuration < duration)
		{
			_curDuration += Time.deltaTime;
			float num = _curDuration / duration;
			Mathf.Min(num, 1f);
			txFon[1].alpha = num;
			yield return num;
		}
		txFon[1].mainTexture = null;
		txFon[0].mainTexture = needFon;
	}

	public static void SetActiveWithCaption(string caption)
	{
		if (instance != null && instance.lbLoading != null)
		{
			instance.lbLoading.text = caption ?? string.Empty;
		}
		IsActiveIndicator = true;
	}

	public static void ClearMemory()
	{
		if (instance != null && instance.canClearMemory)
		{
			instance.StartCoroutine(instance.Crt_ClearMemory());
		}
	}

	private IEnumerator Crt_ClearMemory()
	{
		if (canClearMemory)
		{
			canClearMemory = false;
			yield return null;
			GC.Collect();
			yield return null;
			Resources.UnloadUnusedAssets();
			yield return null;
			canClearMemory = true;
		}
	}
}
