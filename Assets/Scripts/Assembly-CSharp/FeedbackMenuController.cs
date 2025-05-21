using System;
using System.Linq;
using System.Reflection;
using Rilisoft;
using UnityEngine;

public sealed class FeedbackMenuController : MonoBehaviour
{
	public enum State
	{
		FAQ = 0,
		TermsFuse = 1
	}

	private State _currentState;

	public UIButton faqButton;

	public UIButton termsFuseButton;

	public UIButton sendFeedbackButton;

	public UIButton backButton;

	public GameObject textFAQScroll;

	public GameObject textTermsOfUse;

	[SerializeField]
	protected internal UILabel _versionLabel;

	public static FeedbackMenuController Instance { get; private set; }

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			if (faqButton != null)
			{
				faqButton.isEnabled = value != State.FAQ;
				Transform transform = faqButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform != null)
				{
					transform.gameObject.SetActive(value != State.FAQ);
				}
				Transform transform2 = faqButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value == State.FAQ);
				}
				textFAQScroll.SetActive(value == State.FAQ);
			}
			if (termsFuseButton != null)
			{
				termsFuseButton.isEnabled = value != State.TermsFuse;
				Transform transform3 = termsFuseButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value != State.TermsFuse);
				}
				else
				{
					Debug.Log("_spriteLabel=null");
				}
				Transform transform4 = termsFuseButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(value == State.TermsFuse);
				}
				else
				{
					Debug.Log("_spriteLabel=null");
				}
				textTermsOfUse.SetActive(value == State.TermsFuse);
			}
			_currentState = value;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		CurrentState = State.FAQ;
		foreach (UIButton item in new UIButton[2] { faqButton, termsFuseButton }.Where((UIButton b) => b != null))
		{
			ButtonHandler component = item.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleTabPressed;
			}
		}
		if (sendFeedbackButton != null)
		{
			ButtonHandler component2 = sendFeedbackButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += HandleSendFeedback;
			}
		}
		if (backButton != null)
		{
			ButtonHandler component3 = backButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += HandleBackButton;
			}
		}
		string text = "12.0.0";
		if (_versionLabel != null)
		{
			_versionLabel.text = text;
		}
	}

	private void BackButton()
	{
		base.gameObject.SetActive(false);
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.settingsPanel != null)
		{
			MainMenuController.sharedController.settingsPanel.SetActive(true);
		}
	}

	private void HandleBackButton(object sender, EventArgs e)
	{
		BackButton();
	}

	public static void ShowDialogWithCompletion(Action handler)
	{
		if (handler != null)
		{
			handler();
		}
	}

	private void HandleSendFeedback(object sender, EventArgs e)
	{
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (faqButton != null && gameObject == faqButton.gameObject)
		{
			CurrentState = State.FAQ;
		}
		else if (termsFuseButton != null && gameObject == termsFuseButton.gameObject)
		{
			CurrentState = State.TermsFuse;
		}
	}
}
