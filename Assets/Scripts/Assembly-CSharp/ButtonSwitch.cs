using System;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
	[Header("GO to show when enabled")]
	[SerializeField]
	protected internal GameObject enabledGo;

	[Header("GO to show when disabled")]
	[SerializeField]
	protected internal GameObject disbledGo;

	[Header("Does button should play click sound?")]
	[SerializeField]
	protected internal bool noSound;

	[Header("Должна ли кнопка нажиматься сама или активироваться из кода")]
	[SerializeField]
	protected internal bool isAutomatic = true;

	[SerializeField]
	protected internal UITweener tweenOn;

	[SerializeField]
	protected internal UITweener tweenOff;

	private Collider collider
	{
		get
		{
			return base.gameObject.GetComponent<Collider>();
		}
	}

	public bool HasClickedHandlers
	{
		get
		{
			return this.Clicked != null;
		}
	}

	private bool isEnable
	{
		get
		{
			if (collider == null)
			{
				return false;
			}
			return collider.enabled;
		}
		set
		{
			if (collider != null)
			{
				collider.enabled = value;
			}
		}
	}

	public event EventHandler Clicked;

	private void OnClick()
	{
		if (isAutomatic && isEnable)
		{
			if (ButtonClickSound.Instance != null && !noSound)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			Switch(false);
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}
	}

	public void DoClick()
	{
		if (isEnable)
		{
			OnClick();
		}
	}

	public void Switch(bool isEnabled)
	{
		enabledGo.SetActive(isEnabled);
		disbledGo.SetActive(!isEnabled);
		isEnable = isEnabled;
		if (isEnabled)
		{
			if ((bool)tweenOn)
			{
				tweenOn.ResetToBeginning();
				tweenOn.PlayForward();
			}
		}
		else if ((bool)tweenOff)
		{
			tweenOff.ResetToBeginning();
			tweenOff.PlayForward();
		}
	}
}
