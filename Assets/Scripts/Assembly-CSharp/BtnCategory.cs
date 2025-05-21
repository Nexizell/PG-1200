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
	[CompilerGenerated]
	internal sealed class _003CSetButtonPressed_003Ed__38 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BtnCategory _003C_003E4__this;

		public bool isButtonPressed;

		public bool instant;

		private float _003CanimationTimer_003E5__1;

		private float _003CanimationTimer_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSetButtonPressed_003Ed__38(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0043;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0043;
			case 2:
				_003C_003E1__state = -1;
				goto IL_02af;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0421;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0043:
				if (_003C_003E4__this.isAnimationPlayed)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (isButtonPressed != _003C_003E4__this.isPressed)
				{
					return false;
				}
				_003C_003E4__this.SetDependentsState(isButtonPressed);
				if (isButtonPressed)
				{
					if (instant)
					{
						_003C_003E4__this.normalState.color = Color.Lerp(_003C_003E4__this.alphaColor, _003C_003E4__this.normalColor, 0f);
						_003C_003E4__this.pressedState.color = Color.Lerp(_003C_003E4__this.normalColor, _003C_003E4__this.alphaColor, 0f);
						_003C_003E4__this.transform.localScale = Vector3.Lerp(Vector3.one * _003C_003E4__this.scaleMultypler, Vector3.one, 0f);
						_003C_003E4__this.isAnimationPlayed = false;
						for (int i = 0; i < _003C_003E4__this.holdScale.Length; i++)
						{
							_003C_003E4__this.holdScale[i].localScale = Vector3.Lerp(Vector3.one / _003C_003E4__this.scaleMultypler, Vector3.one, 0f);
						}
						break;
					}
					_003C_003E4__this.isAnimationPlayed = true;
					_003CanimationTimer_003E5__1 = _003C_003E4__this.animTime;
					goto IL_02af;
				}
				if (_003C_003E4__this.wasPressed)
				{
					_003C_003E4__this.isAnimationPlayed = true;
					_003CanimationTimer_003E5__2 = 0f;
					goto IL_0421;
				}
				_003C_003E4__this.normalState.color = _003C_003E4__this.normalColor;
				_003C_003E4__this.pressedState.color = _003C_003E4__this.alphaColor;
				_003C_003E4__this.transform.localScale = Vector3.one;
				for (int j = 0; j < _003C_003E4__this.holdScale.Length; j++)
				{
					_003C_003E4__this.holdScale[j].localScale = Vector3.one;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 4;
				return true;
				IL_02af:
				if (_003CanimationTimer_003E5__1 > 0f && _003C_003E4__this.isPressed)
				{
					_003CanimationTimer_003E5__1 -= Time.unscaledDeltaTime;
					float t = _003CanimationTimer_003E5__1 / _003C_003E4__this.animTime;
					_003C_003E4__this.normalState.color = Color.Lerp(_003C_003E4__this.alphaColor, _003C_003E4__this.normalColor, t);
					_003C_003E4__this.pressedState.color = Color.Lerp(_003C_003E4__this.normalColor, _003C_003E4__this.alphaColor, t);
					_003C_003E4__this.transform.localScale = Vector3.Lerp(Vector3.one * _003C_003E4__this.scaleMultypler, Vector3.one, t);
					for (int k = 0; k < _003C_003E4__this.holdScale.Length; k++)
					{
						_003C_003E4__this.holdScale[k].localScale = Vector3.Lerp(Vector3.one / _003C_003E4__this.scaleMultypler, Vector3.one, t);
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E4__this.isAnimationPlayed = false;
				break;
				IL_0421:
				if (_003CanimationTimer_003E5__2 < _003C_003E4__this.animTime)
				{
					_003CanimationTimer_003E5__2 += Time.unscaledDeltaTime;
					float t2 = _003CanimationTimer_003E5__2 / _003C_003E4__this.animTime;
					_003C_003E4__this.normalState.color = Color.Lerp(_003C_003E4__this.alphaColor, _003C_003E4__this.normalColor, t2);
					_003C_003E4__this.pressedState.color = Color.Lerp(_003C_003E4__this.normalColor, _003C_003E4__this.alphaColor, t2);
					_003C_003E4__this.transform.localScale = Vector3.Lerp(Vector3.one * _003C_003E4__this.scaleMultypler, Vector3.one, t2);
					for (int l = 0; l < _003C_003E4__this.holdScale.Length; l++)
					{
						_003C_003E4__this.holdScale[l].localScale = Vector3.Lerp(Vector3.one / _003C_003E4__this.scaleMultypler, Vector3.one, t2);
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003E4__this.wasPressed = false;
				_003C_003E4__this.isAnimationPlayed = false;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

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
