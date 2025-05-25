using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class BtnCategory : MonoBehaviour
{
	private bool _isEnable = true;

	public Transform[] holdScale;

	public GameObject lockSprite;

	public UISprite normalState;

	public UISprite pressedState;

	public UITweener btnAnimation;

	public CategoryButtonsController btnController;

	public bool isPressed;

	public bool isDefault;

	private Color alphaColor;

	private Color normalColor;

	private Color pressedColor;

	[HideInInspector]
	public float scaleMultypler = 1.1f;

	[HideInInspector]
	public float animTime = 0.7f;

	public string btnName;

	[HideInInspector]
	public bool wasPressed;

	private bool isAnimationPlayed;

	[SerializeField]
	public List<BtnCategoryDependent> _dependents;

	[SerializeField]
	protected internal UILabel[] _doubleRewardLabels;

	[SerializeField]
	protected internal GameConnect.GameMode _gameMode;

	private NewDayWatcher _newDayWatcher;

	public bool isEnable
	{
		get
		{
			return _isEnable;
		}
		set
		{
			_isEnable = value;
			if (lockSprite != null)
			{
				lockSprite.SetActive(!_isEnable);
			}
		}
	}

	public bool IsAnimationPlayed
	{
		get
		{
			return isAnimationPlayed;
		}
	}

	private NewDayWatcher NewDayWatcher
	{
		get
		{
			if (_newDayWatcher == null)
			{
				GameObject gameObject = ((base.transform.parent != null) ? base.transform.parent.gameObject : base.gameObject);
				NewDayWatcher newDayWatcher = gameObject.GetComponent<NewDayWatcher>() ?? gameObject.AddComponent<NewDayWatcher>();
				_newDayWatcher = newDayWatcher;
			}
			return _newDayWatcher;
		}
	}

	internal GameConnect.GameMode GameMode
	{
		get
		{
			return _gameMode;
		}
	}

	public event EventHandler Clicked;

	private void OnEnable()
	{
		ResetButton();
		RefreshDoubleRewardLabels();
		if (NewDayWatcher != null)
		{
			NewDayWatcher.NewDay += HandleNewDay;
		}
	}

	private void OnDisable()
	{
		if (NewDayWatcher != null)
		{
			NewDayWatcher.NewDay -= HandleNewDay;
		}
		ResetButton();
	}

	private void HandleNewDay(object sender, EventArgs e)
	{
		RefreshDoubleRewardLabels();
	}

	private void RefreshDoubleRewardLabels()
	{
		if (_doubleRewardLabels == null || _doubleRewardLabels.Length == 0)
		{
			return;
		}
		bool flag = DoubleReward.Instance.NeedDoubleReward(_gameMode);
		UILabel[] doubleRewardLabels = _doubleRewardLabels;
		foreach (UILabel uILabel in doubleRewardLabels)
		{
			if (uILabel == null)
			{
				continue;
			}
			uILabel.gameObject.SetActive(flag);
			if (flag)
			{
				try
				{
					string format = LocalizationStore.Get("Key_3146");
					uILabel.text = string.Format(CultureInfo.InvariantCulture, format, DoubleReward.Instance.RewardFactor);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
					uILabel.gameObject.SetActive(false);
				}
			}
		}
	}

	private void ResetButton()
	{
		isAnimationPlayed = false;
		if (isDefault)
		{
			btnController.currentBtnName = btnName;
			isPressed = true;
			wasPressed = true;
		}
		else
		{
			isPressed = false;
		}
		alphaColor = new Color(1f, 1f, 1f, 0.04f);
		normalColor = new Color(1f, 1f, 1f, 1f);
		pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
		if (isPressed)
		{
			base.transform.localScale = Vector3.one * scaleMultypler;
			for (int i = 0; i < holdScale.Length; i++)
			{
				holdScale[i].localScale = Vector3.one / scaleMultypler;
			}
			normalState.color = alphaColor;
			pressedState.color = normalColor;
		}
		else
		{
			base.transform.localScale = Vector3.one;
			for (int j = 0; j < holdScale.Length; j++)
			{
				holdScale[j].localScale = Vector3.one;
			}
			normalState.color = normalColor;
			pressedState.color = alphaColor;
		}
		btnController.buttonsTable.Reposition();
		SetDependentsState(isPressed);
	}

	public IEnumerator SetButtonPressed(bool isButtonPressed, bool instant = false)
	{
		while (isAnimationPlayed)
		{
			yield return null;
		}
		if (isButtonPressed != isPressed)
		{
			yield break;
		}
		SetDependentsState(isButtonPressed);
		if (isButtonPressed)
		{
			if (instant)
			{
				normalState.color = Color.Lerp(alphaColor, normalColor, 0f);
				pressedState.color = Color.Lerp(normalColor, alphaColor, 0f);
				transform.localScale = Vector3.Lerp(Vector3.one * scaleMultypler, Vector3.one, 0f);
				isAnimationPlayed = false;
				for (int i = 0; i < holdScale.Length; i++)
				{
					holdScale[i].localScale = Vector3.Lerp(Vector3.one / scaleMultypler, Vector3.one, 0f);
				}
				yield break;
			}
			isAnimationPlayed = true;
			float animationTimer2 = animTime;
			while (animationTimer2 > 0f && isPressed)
			{
				animationTimer2 -= Time.unscaledDeltaTime;
				float t = animationTimer2 / animTime;
				normalState.color = Color.Lerp(alphaColor, normalColor, t);
				pressedState.color = Color.Lerp(normalColor, alphaColor, t);
				transform.localScale = Vector3.Lerp(Vector3.one * scaleMultypler, Vector3.one, t);
				for (int j = 0; j < holdScale.Length; j++)
				{
					holdScale[j].localScale = Vector3.Lerp(Vector3.one / scaleMultypler, Vector3.one, t);
				}
				yield return null;
			}
			isAnimationPlayed = false;
		}
		else if (wasPressed)
		{
			isAnimationPlayed = true;
			float animationTimer = 0f;
			while (animationTimer < animTime)
			{
				animationTimer += Time.unscaledDeltaTime;
				float t2 = animationTimer / animTime;
				normalState.color = Color.Lerp(alphaColor, normalColor, t2);
				pressedState.color = Color.Lerp(normalColor, alphaColor, t2);
				transform.localScale = Vector3.Lerp(Vector3.one * scaleMultypler, Vector3.one, t2);
				for (int k = 0; k < holdScale.Length; k++)
				{
					holdScale[k].localScale = Vector3.Lerp(Vector3.one / scaleMultypler, Vector3.one, t2);
				}
				yield return null;
			}
			wasPressed = false;
			isAnimationPlayed = false;
		}
		else
		{
			normalState.color = normalColor;
			pressedState.color = alphaColor;
			transform.localScale = Vector3.one;
			for (int l = 0; l < holdScale.Length; l++)
			{
				holdScale[l].localScale = Vector3.one;
			}
			yield return null;
		}
	}

	private void OnClick()
	{
		if (!isEnable)
		{
			return;
		}
		EventHandler clicked = this.Clicked;
		if (clicked != null)
		{
			clicked(this, EventArgs.Empty);
		}
		if (isPressed || isAnimationPlayed)
		{
			return;
		}
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		btnController.BtnClicked(btnName);
		if (btnAnimation != null)
		{
			btnAnimation.ResetToBeginning();
			btnAnimation.PlayForward();
		}
		try
		{
			if (btnController.actions != null && btnController.buttons != null)
			{
				int num = btnController.buttons.ToList().IndexOf(this);
				if (num != -1 && btnController.actions.Count > num && btnController.actions[num] != null)
				{
					btnController.actions[num](this);
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in invoking action in BtnCategory: " + ex);
		}
	}

	private void OnPress(bool pressed)
	{
		if (isEnable && !isPressed)
		{
			if (pressed)
			{
				normalState.color = pressedColor;
			}
			else
			{
				normalState.color = normalColor;
			}
		}
	}

	private void SetDependentsState(bool buttonSelected)
	{
		foreach (BtnCategoryDependent dependent in _dependents)
		{
			if (dependent != null && !(dependent.Dependent == null))
			{
				bool flag = (dependent.InvertVisible ? (!buttonSelected) : buttonSelected);
				if (dependent.Dependent.activeSelf != flag)
				{
					dependent.Dependent.SetActive(flag);
				}
			}
		}
	}
}
