using UnityEngine;

[DisallowMultipleComponent]
public sealed class TimeLabel : MonoBehaviour
{
	private UILabel _label;

	public UISprite timerBackground;

	public AudioSource timerSound;

	public ParticleSystem timerParticles;

	private Vector3 targetScale = Vector3.one;

	private bool blink;

	private float startTime = 11f;

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti || GameConnect.isSpeedrun);
		_label = GetComponent<UILabel>();
	}

	private void Update()
	{
		if (!InGameGUI.sharedInGameGUI || !_label)
		{
			return;
		}
		if (GameConnect.isSpeedrun)
		{
			_label.text = SpeedrunTrackController.instance.currentPlayerSpeed.ToString("F1");
			return;
		}
		_label.text = InGameGUI.sharedInGameGUI.timeLeft();
		if (GameConnect.isMiniGame)
		{
			return;
		}
		float num = (GameConnect.isDuel ? DuelController.instance.timeLeft : ((float)TimeGameController.sharedController.timerToEndMatch));
		if (num <= startTime && (!GameConnect.isDuel || DuelController.instance.gameStatus == DuelController.GameStatus.Playing))
		{
			float num2 = Mathf.Round(num) - num;
			blink = num2 > 0f;
			_label.transform.localScale = Vector3.MoveTowards(_label.transform.localScale, blink ? (Vector3.one * Mathf.Min(1.4f + (startTime - num) / 20f, 2f)) : Vector3.one, blink ? (12f * Time.deltaTime) : (2.4f * Time.deltaTime));
			_label.color = (blink ? Color.red : Color.white);
			_label.GetComponentInChildren<TweenRotation>().enabled = true;
			_label.GetComponentInChildren<TweenRotation>().PlayForward();
			if (Defs.isSoundFX)
			{
				timerSound.enabled = true;
			}
			timerSound.loop = true;
			if (PauseGUIController.Instance != null && PauseGUIController.Instance.IsPaused)
			{
				timerParticles.gameObject.SetActive(false);
			}
			else
			{
				timerParticles.gameObject.SetActive(true);
			}
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = timerParticles.textureSheetAnimation;
			ParticleSystemCurveMode mode = textureSheetAnimation.frameOverTime.mode;
			textureSheetAnimation.frameOverTime = new ParticleSystem.MinMaxCurve((num - 1f) / 9f);
			if (num < 1f)
			{
				timerParticles.gameObject.SetActive(false);
			}
		}
		else
		{
			timerParticles.gameObject.SetActive(false);
			timerSound.enabled = false;
			_label.color = Color.white;
			_label.transform.localScale = Vector3.one;
			_label.GetComponentInChildren<TweenRotation>().ResetToBeginning();
			_label.GetComponentInChildren<TweenRotation>().enabled = false;
		}
	}
}
